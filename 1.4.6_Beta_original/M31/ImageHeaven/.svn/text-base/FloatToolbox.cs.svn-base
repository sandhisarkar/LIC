using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class FloatToolbox : ToolWindow
    {

    	Button[] btns = new Button[30];
    	NovaNet.Utils.ImageManupulation[] delTest = new NovaNet.Utils.ImageManupulation[30];
    	int btnIndex = 0;
        public FloatToolbox()
        {
            InitializeComponent();
        }
        public bool AddButton(System.Windows.Forms.Button prmBut, NovaNet.Utils.ImageManupulation prmTest)
        {
    		int x = 10;
    		int y = 50;
    		int h = 23;
    		int w = 90;
        	btns[btnIndex] = prmBut;
        	delTest[btnIndex] = prmTest;
        	//btns[btnIndex].Text=txt;
        	this.groupBox3.Controls.Add(btns[btnIndex]);
        	

        	if ((0==btnIndex) || (btnIndex % 2 == 0))
        			x = 10;
        	else
        			x = 100;

        		y = (25 * Convert.ToInt32( (btnIndex/2)))+20;
        		btns[btnIndex].Left= x;
        		btns[btnIndex].Top = y;
        		btns[btnIndex].Height = h;
        		btns[btnIndex].Width = w;
        		x+=w;
        
        	this.btns[btnIndex].Click += new System.EventHandler(this.btnClick);
        	btnIndex++;
        	groupBox3.Refresh();
        	return true;
        }
        void btnClick(object sender, EventArgs e)
        {
        	for (int i=0; i<=btnIndex; i++)
        	{
        		if (sender.Equals(btns[i]))
        		    {
        		    	delTest[i].Invoke();
        		    	break;
        		    }
        	}
        }
    }
}