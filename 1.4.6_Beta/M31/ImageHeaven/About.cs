/*
 * Created by SharpDevelop.
 * User: RakeshS
 * Date: 29/06/2009
 * Time: 3:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Generic;
using VersionCheck;

namespace ImageHeaven
{
	/// <summary>
	/// Description of About.
	/// </summary>
	public partial class About : Form
	{
		public About()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			
		}

        private void About_Load(object sender, EventArgs e)
        {
            string vRsion;
            Color shaded1 = Color.LemonChiffon;
            Color shaded2 = Color.LightCyan;
            int i = 0;
            ListViewItem lvwItem = new ListViewItem();
            AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
            lblVersion.Text = "Version: " + assemName.Version.ToString();
            List<string> _asm = new List<string>();
            _asm.Add("M31");
            _asm.Add("ImageHeaven");
            _asm.Add("LItems");
            _asm.Add("NIPlugin");
            _asm.Add("NovaNet.Utils");
            _asm.Add("NovaNet.wfe");
            _asm.Add("TwainUtils");
            _asm.Add("wSelect");
            
            List<AssemblyDetails> _ad = HealthInfo.GetAssemblyDetails(_asm);
            foreach (AssemblyDetails _iad in _ad)
            {
                vRsion = _iad.vMajor;
                lvwItem = lvwAsm.Items.Add(_iad.FullName);
                lvwItem.SubItems.Add(vRsion);
                lvwItem.SubItems.Add(_iad.CodeBase);

                if (i++ % 2 == 1)
                {
                    lvwItem.BackColor = shaded1;
                    lvwItem.UseItemStyleForSubItems = true;
                }
                else
                {
                    lvwItem.BackColor = shaded2;
                    lvwItem.UseItemStyleForSubItems = true;
                }
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
	}
}
