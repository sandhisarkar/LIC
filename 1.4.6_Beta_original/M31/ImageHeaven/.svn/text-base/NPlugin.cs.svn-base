using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using NP;

namespace LoadPlugin
{
    public class NPlugin
    {
        //Internally holding the plugin object
        private List<NIPlugin> intPlugin = new List<NIPlugin>();

        public NPlugin(string FilePath)
        {
            SetPlugin(FilePath);
        }
        public bool LoadAnother(string FilePath)
        {
            SetPlugin(FilePath);
            return true;
        }
        private bool SetPlugin(string FilePath)
        {
            Assembly asm;
            PluginType pt = PluginType.Unknown;
            Type PluginClass = null;
            if (!File.Exists(FilePath))
                return false;
            
            asm = Assembly.LoadFile(FilePath);
            if (asm != null)
            {
                foreach (Type type in asm.GetTypes())
                {
                    //If the type is of abstract type then no point in loading and heading along with the .dll
                    if (type.IsAbstract)
                        continue;
                    object[] attribs = type.GetCustomAttributes(typeof(PluginAttribute), true);
                    if (attribs.Length > 0)
                    {
                        foreach (PluginAttribute pa in attribs)
                        {
                            pt = pa.Type;
                        }
                        PluginClass = type;
                    }
                }
            }
            if (pt == PluginType.Unknown)
                return false;
            intPlugin.Add(Activator.CreateInstance(PluginClass) as NIPlugin);
            return true;
        }
        public List<NIPlugin> GetPlugin { get { return intPlugin; } }
    }
}
