using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NovaNet.Utils;
using NovaNet.wfe;
using LItems;
using System.Data.Odbc;

namespace ImageHeaven
{
    public partial class frmRepeatScan : Form
    {
        OdbcConnection sqlCon = null;
        Credentials crd = new Credentials();
        public frmRepeatScan(OdbcConnection pCon,Credentials pCrd)
        {
            InitializeComponent();
            sqlCon = pCon;
            crd = pCrd;
        }

        private void frmRepeatScan_Load(object sender, EventArgs e)
        {
            PopulateProjectCombo();
        }
        private void PopulateProjectCombo()
        {
            DataSet ds = new DataSet();
            wfeProject tmpProj = new wfeProject(sqlCon);
            //cmbProject.Items.Add("Select");
            ds = tmpProj.GetAllValues();
            cmbProjects.DataSource = ds.Tables[0];
            cmbProjects.DisplayMember = ds.Tables[0].Columns[1].ToString();
            cmbProjects.ValueMember = ds.Tables[0].Columns[0].ToString();
        }
        private void PopulateBatchCombo()
        {
            string projKey = null;
            DataSet ds = new DataSet();
            wfeBatch tmpBatch = new wfeBatch(sqlCon);
            if (cmbProjects.SelectedValue != null)
            {
                projKey = cmbProjects.SelectedValue.ToString();
                ds = tmpBatch.GetAllValues(Convert.ToInt32(projKey));
                cmbBatch.DataSource = ds.Tables[0];
                cmbBatch.DisplayMember = ds.Tables[0].Columns[1].ToString();
                cmbBatch.ValueMember = ds.Tables[0].Columns[0].ToString();
            }
        }

        private void cmbProjects_Leave(object sender, EventArgs e)
        {
            PopulateBatchCombo();
        }

        private void cmbBatch_Leave(object sender, EventArgs e)
        {
            GenerateListView();
        }
        private void GenerateListView()
        {
            wfeBox tmpBox = new wfeBox(sqlCon);
            DataSet ds = tmpBox.GetBox(Convert.ToInt32(cmbProjects.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), eSTATES.BOX_READY_TO_REPEAT_SCAN);
            if (ds != null)
            {
                lvwBox.Items.Clear();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ListViewItem lvwItem = lvwBox.Items.Add((i+1).ToString());
                            //listView1.Items.Add(ds.Tables[0].Rows[i]["problem_type"].ToString());
                            lvwItem.SubItems.Add(ds.Tables[0].Rows[i]["box_number"].ToString());
                        }
                    }
                }
            }
        }

        private void cmdReadyQC_Click(object sender, EventArgs e)
        {
            wfeBox tmpBox =null;
            CtrlBox pBox = null;
            DialogResult dr = MessageBox.Show("Are you sure, you want to forward those selected boxes to QC stage?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                for (int i = 0; i < lvwBox.Items.Count; i++)
                {
                    if (lvwBox.Items[i].Checked == true)
                    {
                        int boxNumber =Convert.ToInt32(lvwBox.Items[i].SubItems[1].Text);
                        pBox = new CtrlBox(Convert.ToInt32(cmbProjects.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()),boxNumber);
                        tmpBox = new wfeBox(sqlCon, pBox);
                        tmpBox.UpdateStatus(eSTATES.BOX_SCANNED);
                    }
                }
                GenerateListView();
            }
        }

        private void cmdRepeatScan_Click(object sender, EventArgs e)
        {
            wfeBox tmpBox = null;
            CtrlBox pBox = null;
            wfePolicy tmpPolicy = null;
            CtrlPolicy pPolicy = null;
            DialogResult dr = MessageBox.Show("Are you sure, you want to repeat scanning of the selected boxes?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                for (int i = 0; i < lvwBox.Items.Count; i++)
                {
                    if (lvwBox.Items[i].Checked == true)
                    {
                        int boxNumber = Convert.ToInt32(lvwBox.Items[i].SubItems[1].Text);
                        pBox = new CtrlBox(Convert.ToInt32(cmbProjects.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNumber);
                        tmpBox = new wfeBox(sqlCon, pBox);
                        OdbcTransaction pTrans = sqlCon.BeginTransaction();
                        if (tmpBox.UpdateStatus(eSTATES.BOX_CREATED, pTrans))
                        {
                            pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProjects.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()),Convert.ToInt32(boxNumber),0);
                            tmpPolicy = new wfePolicy(sqlCon,pPolicy);
                            if (tmpPolicy.UpdateAllPolicyStatus(eSTATES.POLICY_CREATED, crd, pTrans))
                            {
                                pTrans.Commit();
                            }
                            else
                            {
                                pTrans.Rollback();
                            }
                        }
                        else
                        {
                            pTrans.Rollback();
                        }
                    }
                }
                GenerateListView();
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
