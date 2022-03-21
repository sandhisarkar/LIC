/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 18/3/2008
 * Time: 4:03 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using NovaNet.Utils;
using NovaNet.wfe;
using System.Data;
using System.Data.Odbc;
using System.IO;
using LItems;

namespace ImageHeaven
{
	/// <summary>
	/// Description of aeBoxSelection.
	/// </summary>
	public partial class aeBoxSelection : Form
	{
		NovaNet.Utils.dbCon dbcon;
		OdbcConnection sqlCon=null;
		eSTATES[] state;
		
		public aeBoxSelection()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.Text = "B'Zer - Fetch Docket";
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public aeBoxSelection(eSTATES[] prmState,OdbcConnection prmCon)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            sqlCon = prmCon;
			this.Text = "B'Zer - Fetch Docket";
			state=prmState;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void AeBoxSelectionLoad(object sender, EventArgs e)
		{
			PopulateProjectCombo();
			PopulateBatchCombo();
		}
		private void PopulateBatchCombo()
		{
			string projKey=null;
			DataSet ds=new DataSet();
			
			dbcon=new NovaNet.Utils.dbCon();
			
			wfeBatch tmpBatch=new wfeBatch(sqlCon);
			if(cmbProject.SelectedValue != null)
			{
				projKey=cmbProject.SelectedValue.ToString();
				ds=tmpBatch.GetAllValues(Convert.ToInt32(projKey));
				cmbBatch.DataSource=ds.Tables[0];
				cmbBatch.DisplayMember=ds.Tables[0].Columns[1].ToString();
				cmbBatch.ValueMember=ds.Tables[0].Columns[0].ToString();
			}
		}
		private void PopulateBoxCombo()
		{
			string batchKey=null;
			DataSet ds=new DataSet();
			
			dbcon=new NovaNet.Utils.dbCon();
			
			wfeBox tmpBox=new wfeBox(sqlCon);
			if(cmbBatch.SelectedValue != null)
			{
                if (state[0] != eSTATES.POLICY_SCANNED)
                {
                    batchKey = cmbBatch.SelectedValue.ToString();
                    ds = tmpBox.GetBox(state, Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(batchKey));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cmbBox.DataSource = ds.Tables[0];
                        cmbBox.DisplayMember = ds.Tables[0].Columns[0].ToString();
                    }
                }
                else
                {
                    batchKey = cmbBatch.SelectedValue.ToString();
                    ds = tmpBox.GetScannedBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(batchKey));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cmbBox.DataSource = ds.Tables[0];
                        cmbBox.DisplayMember = ds.Tables[0].Columns[0].ToString();
                    }
                }
			}
		}
		private void PopulateProjectCombo()
		{
			DataSet ds=new DataSet();
			
			dbcon=new NovaNet.Utils.dbCon();
			
			wfeProject tmpProj=new wfeProject(sqlCon);
			//cmbProject.Items.Add("Select");
			ds=tmpProj.GetAllValues();
			cmbProject.DataSource=ds.Tables[0];
			cmbProject.DisplayMember=ds.Tables[0].Columns[1].ToString();
			cmbProject.ValueMember=ds.Tables[0].Columns[0].ToString();
		}
		void CmbProjectLeave(object sender, EventArgs e)
		{
			PopulateBatchCombo();
		}
		void CmbBatchLeave(object sender, EventArgs e)
		{
			PopulateBoxCombo();
		}
		void CmdFetchClick(object sender, EventArgs e)
		{
			frmMain frm= new frmMain();
		    
		    //this.Hide();
		    this.Close();
            NovaNet.wfe.ScanNotify del;
		    if((cmbProject.Text.ToString() != string.Empty) && (cmbBatch.Text.ToString() != string.Empty) && (cmbBox.Text.ToString() != string.Empty))
		    {
			    CtrlBox ctrlBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()),Convert.ToInt32(cmbBox.Text));
			    wItem box = new wfeBox(sqlCon, ctrlBox);
                int sType = ihConstants._SCAN_WITHOUT_PHOTO;
                if (chkPhotoScan.Checked == true)
                {
                    sType = ihConstants._SCAN_WITH_PHOTO;
                }
                del = new ScanNotify(frm.SetValues);
                del(box, sType);
		    }
		}
		
		void CmbBatchSelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
	}
}
