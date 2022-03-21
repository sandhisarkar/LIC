using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Odbc;
using Logistics_reports;
using LItems;
using NovaNet.wfe;

namespace ImageHeaven
{
    public partial class frmSelection : Form
    {
        private OdbcConnection sqlCon;
        //ivate LogisticControl lgs;
        private string reportName;
        public frmSelection(OdbcConnection prmCon,string prmReportName)
        {
            InitializeComponent();
            sqlCon = prmCon;
            reportName = prmReportName;
        }

        private void frmSelection_Load(object sender, EventArgs e)
        {
            PopulateProjectCombo();
        }
        private void PopulateProjectCombo()
        {
            DataSet ds = new DataSet();

            //dbcon = new NovaNet.Utils.dbCon();

            wfeProject tmpProj = new wfeProject(sqlCon);
            //cmbProject.Items.Add("Select");
            ds = tmpProj.GetAllValues();
            cmbProject.DataSource = ds.Tables[0];
            cmbProject.DisplayMember = ds.Tables[0].Columns[1].ToString();
            cmbProject.ValueMember = ds.Tables[0].Columns[0].ToString();
        }

        private void PopulateBatchCombo()
        {
            string projKey = null;
            DataSet ds = new DataSet();

            //dbcon = new NovaNet.Utils.dbCon();

            wfeBatch tmpBatch = new wfeBatch(sqlCon);
            if (cmbProject.SelectedValue != null)
            {
                projKey = cmbProject.SelectedValue.ToString();
                ds = tmpBatch.GetAllValues(Convert.ToInt32(projKey));
                cmbBatch.DataSource = ds.Tables[0];
                cmbBatch.DisplayMember = ds.Tables[0].Columns[1].ToString();
                cmbBatch.ValueMember = ds.Tables[0].Columns[0].ToString();
            }
        }

        private void cmbProject_Leave(object sender, EventArgs e)
        {
            PopulateBatchCombo();
        }

        private void cmdGenerate_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            if ((cmbProject.SelectedValue != null) && (cmbBatch.SelectedValue != null))
            {
                //if (reportName == "ExceptionRpt")
                //{
                //    lgs = new LogisticControl(sqlCon);
                //    ds = lgs.GetExceptionData(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
                //    frmRptViewer viewer = new frmRptViewer(ds, reportName, Convert.ToInt32(cmbBatch.SelectedValue.ToString()),sqlCon);
                //    viewer.ShowDialog(this);
                //}
                //if (reportName == "RMFSummary")
                //{
                //    frmRptViewer viewer = new frmRptViewer(reportName, Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()),sqlCon);
                //    viewer.ShowDialog(this);
                //}
                //if (reportName == "RMFChallan")
                //{
                //    lgs = new LogisticControl(sqlCon);
                //    ds = lgs.GetRMFChallanDetails(Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
                //    frmRptViewer viewer = new frmRptViewer(ds, reportName, Convert.ToInt32(cmbBatch.SelectedValue.ToString()),sqlCon);
                //    viewer.ShowDialog(this);
                //}
                if ((reportName == "UATAnxA") || ((reportName == "BatchSummary")))
                {
                    if ((cmbProject.SelectedValue != null) && (cmbBatch.SelectedValue != null))
                    {
                        CtrlBox ctrlBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), 0);
                        wfeBox box = new wfeBox(sqlCon, ctrlBox);
                        frmRptViewer viewer = new frmRptViewer(reportName, box, sqlCon);
                        viewer.ShowDialog(this);
                    }
                }
            }
        }
    }
}