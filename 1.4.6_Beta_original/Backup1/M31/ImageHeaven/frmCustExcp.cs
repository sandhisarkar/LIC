using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using System.IO;
using NovaNet.wfe;
using NovaNet.Utils;
using LItems;

namespace ImageHeaven
{
    public partial class frmCustExcp : Form
    {
        private OdbcConnection sqlCon;
        private wfeBox wBox;
        private wfeBatch pBatch;
        private wfeProject pProject;
        private wfePolicy pPolicy;
        private DataSet ds = new DataSet();
        public frmCustExcp(OdbcConnection pCon,wfeBox pBox)
        {
            InitializeComponent();
            sqlCon = pCon;
            wBox = pBox;
        }

        private void frmCustExcp_Load(object sender, EventArgs e)
        {
            pBatch=new wfeBatch(sqlCon);
			pProject=new wfeProject(sqlCon);
            lblProject.Text = pProject.GetProjectName(wBox.ctrlBox.ProjectCode);
            lblBatch.Text = pBatch.GetBatchName(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
            PopulateGridView();
        }
        private void PopulateGridView()
        {
            CtrlPolicy ctPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, 0, 0);
            pPolicy = new wfePolicy(sqlCon, ctPolicy);
            ds = pPolicy.GetCustExcpList();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0 )
                {
                    dgvList.DataSource = ds.Tables[0];
                    dgvList.Columns[0].Width = 30;
                    dgvList.Columns[1].Width = 70;
                    dgvList.Columns[2].Width = 100;
                    dgvList.Columns[3].Width = 120;
                    dgvList.Columns[4].Width = 200;
                    dgvList.Columns[5].Width = 100;
                }
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            Stream myStream;
            string txtContent;
            System.Windows.Forms.SaveFileDialog svFile = new SaveFileDialog();
            svFile.Filter = "Text files (*.txt)|*.txt";
            svFile.FileName = lblBatch.Text;
            svFile.FilterIndex = 2;
            svFile.RestoreDirectory = true;

            if (svFile.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = svFile.OpenFile()) != null)
                {
                    StreamWriter wText = new StreamWriter(myStream);
                    wText.Write("Box number  Policy number      Problem Type                Image Name                                    Remarks              User                 Status   \n");
                    wText.Write("---------- --------------  ------------------ -------------------------------------         ---------------------------- -----------------    ---------------\n");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        txtContent = ds.Tables[0].Rows[i][0].ToString() + "            " + ds.Tables[0].Rows[i][1].ToString() + "            " + ds.Tables[0].Rows[i][2].ToString() + "               " + ds.Tables[0].Rows[i][3].ToString() + "                       " + ds.Tables[0].Rows[i][4].ToString() + "             " + ds.Tables[0].Rows[i][5].ToString() + "              " + ds.Tables[0].Rows[i][6].ToString() + "\n";

                        wText.Write(txtContent);
                        wText.Write("-----------------------------------------------------------------------------------------------------------------------------\n");
                    }
                    wText.Flush();
                    wText.Close();
                    myStream.Close();
                }
            }
        }

        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
