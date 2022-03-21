using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageHeaven
{
    public partial class frmPolicyLog : Form
    {
        private DataSet ds;
        private string policyNo;
        public frmPolicyLog(DataSet pDs,string pPolicyNo)
        {
            policyNo = pPolicyNo;
            ds = pDs;
            InitializeComponent();
        }

        private void frmPolicyLog_Load(object sender, EventArgs e)
        {
            ListViewItem lvwItem;
            lblPolicyNumber.Text = policyNo;
            //Color shaded1 = Color.LemonChiffon;
            //Color shaded2 = Color.LightCyan;
            //int i = 0;
            lvw.Items.Clear();
            if (ds != null)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (j == 0)
                    {
                        lvwItem = lvw.Items.Add("Scanned User");
                        lvwItem.SubItems.Add(ds.Tables[0].Rows[0][0].ToString());
                    }

                    if (j == 1)
                    {
                        lvwItem = lvw.Items.Add("QC User");
                        lvwItem.SubItems.Add(ds.Tables[0].Rows[0][1].ToString());
                    }
                    if (j == 2)
                    {
                        lvwItem = lvw.Items.Add("Index User");
                        lvwItem.SubItems.Add(ds.Tables[0].Rows[0][2].ToString());
                    }
                    if (j == 3)
                    {
                        lvwItem = lvw.Items.Add("FQC User");
                        lvwItem.SubItems.Add(ds.Tables[0].Rows[0][3].ToString());
                    }
                    //lvwItem.SubItems.Add(ds.Tables[0].Rows[j][2].ToString());
                    //if (i++ % 2 == 1)
                    //{
                    //    lvwItem.BackColor = shaded1;
                    //    lvwItem.UseItemStyleForSubItems = true;
                    //}
                    //else
                    //{
                    //    lvwItem.BackColor = shaded2;
                    //    lvwItem.UseItemStyleForSubItems = true;
                    //}
                }
            }
        }
    }
}
