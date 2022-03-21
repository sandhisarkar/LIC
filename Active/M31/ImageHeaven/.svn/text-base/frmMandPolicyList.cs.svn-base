using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageHeaven
{
    public partial class frmMandPolicyList : Form
    {
        private DataTable ds;
        private string batchName;
        public frmMandPolicyList(DataTable pDs,string pBatch)
        {
            ds = pDs;
            batchName = pBatch;
            InitializeComponent();
        }

        private void frmMandPolicyList_Load(object sender, EventArgs e)
        {
            grdPolicy.DataSource = ds;
            lblBatchName.Text = batchName;
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdExpList_Click(object sender, EventArgs e)
        {
            Stream myStream;
            string txtContent;
            svFile.Filter = "Text files (*.txt)|*.txt";
            svFile.FileName = batchName;
            svFile.FilterIndex = 2;
            svFile.RestoreDirectory = true;
            
            if (svFile.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = svFile.OpenFile()) != null)
                {
                    StreamWriter wText = new StreamWriter(myStream);
                    wText.Write("Box number  Policy number  ProposalForm count ProposalReviewSlip count  PolicyBond count  SignaturePage count \n");
                    wText.Write("---------- --------------  ------------------ ------------------------- ---------------- ---------------------------------\n");
                    for (int i = 0; i < ds.Rows.Count; i++)
                    {
                        txtContent = ds.Rows[i][0].ToString() + "            " + ds.Rows[i][1].ToString() + "            " + ds.Rows[i][2].ToString() + "               " + ds.Rows[i][3].ToString() + "                       " + ds.Rows[i][4].ToString() + "                  " + ds.Rows[i][5].ToString() + "\n";

                        wText.Write(txtContent);
                        wText.Write("-----------------------------------------------------------------------------------------------------------------------------\n");
                    }
                    wText.Flush();
                    wText.Close();
                    myStream.Close();
                }
            } 
        }
    }
}