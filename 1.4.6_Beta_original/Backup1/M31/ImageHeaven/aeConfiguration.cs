/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 13/3/2009
 * Time: 1:19 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using NovaNet.Utils;

namespace ImageHeaven
{
	/// <summary>
	/// Description of aeConfiguration.
	/// </summary>
	public partial class aeConfiguration : Form
	{
		private ImageConfig config=null;
		private DataRow dr=null;
		
		public aeConfiguration()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.Text="B'Zer - Configuration Window";
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void AeConfigurationLoad(object sender, EventArgs e)
		{
			grdImageKeyShrt.DataSource=ImageKeyShortCutTable();
			grdImageKeyShrt.Columns[0].ReadOnly=true;
            grdImageKeyShrt.Columns[2].Visible = false;
			DataGridViewTextBoxColumn cColumn=(DataGridViewTextBoxColumn)grdImageKeyShrt.Columns[1];
			cColumn.MaxInputLength=1;
			
			grdImageValue.DataSource=ImageRelatedValue();
			grdImageValue.Columns[0].ReadOnly=true;
            grdImageValue.Columns[2].Visible = false;
            //grdImageValue.Columns[2].f = false;
			DataGridViewTextBoxColumn cImageValue=(DataGridViewTextBoxColumn)grdImageValue.Columns[1];
			cImageValue.MaxInputLength=3;
			
			dgvIndexKeys.DataSource=IndexKeyShortCutTable();
			dgvIndexKeys.Columns[0].ReadOnly=true;
            dgvIndexKeys.Columns[2].Visible = false;
            //grdImageValue.Columns[2].f = false;
			DataGridViewTextBoxColumn cIndex=(DataGridViewTextBoxColumn)grdImageValue.Columns[1];
			cIndex.MaxInputLength=3;

            grdScanning.DataSource = ScanConfig();
            grdScanning.Columns[0].ReadOnly = true;
            grdScanning.Columns[2].Visible = false;
            //grdImageValue.Columns[2].f = false;
            DataGridViewTextBoxColumn cScan = (DataGridViewTextBoxColumn)grdScanning.Columns[1];
            cScan.MaxInputLength = 1;

		}
		
		public DataTable dt = new DataTable();
        /// <summary>
        /// for making datatable column at runtime
        /// </summary>
        public DataTable ImageKeyShortCutTable()
        {
        	config=new ImageConfig(ihConstants.CONFIG_FILE_PATH);
        	DataTable dt=new DataTable();
        	
            dt.Columns.Add("Key1");
            dt.Columns.Add("Value1");
            dt.Columns.Add("Key2");
//            dt.Columns.Add("Value2");
//            dt.Columns.Add("Key3");
//            dt.Columns.Add("Value3");
            
            dr = dt.NewRow();
            dr["Key1"] = "Crop";
            dr["Key2"] = "CROP";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.CROP_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Skew Right";
            dr["Key2"] = "SKEWRIGHT";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.SKEW_RIGHT_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Skew Left";
            dr["Key2"] = "SKEWLEFT";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.SKEW_LEFT_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
			dr = dt.NewRow();                        
            dr["Key1"] = "Zoom In";
            dr["Key2"] = "ZOOMIN";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.ZOOM_IN_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Zoom Out";
            dr["Key2"] = "ZOOMOUT";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.ZOOM_OUT_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Auto Crop";
            dr["Key2"] = "AUTOCROP";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.AUTO_CROP_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();                        
            dr["Key1"] = "Rotate Right";
            dr["Key2"] = "ROTATERIGHT";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.ROTATE_RIGHT_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow(); 
            dr["Key1"] = "Rotate Left";
            dr["Key2"] = "ROTATELEFT";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.ROTATE_LEFT_KEY).Remove(1,1);
            dt.Rows.Add(dr); 
            
            dr = dt.NewRow();
            dr["Key1"] = "Noise Remove";
            dr["Key2"] = "NOISEREMOVAL";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.NOISE_REMOVE_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Clean";
            dr["Key2"] = "CLEAN";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION,ihConstants.CLEAN_KEY).Remove(1,1);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Key1"] = "Delete";
            dr["Key2"] = "DELETE";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1);
            dt.Rows.Add(dr);
            return dt;
        }
		
        public DataTable IndexKeyShortCutTable()
        {
        	config=new ImageConfig(ihConstants.CONFIG_FILE_PATH);
        	DataTable dt=new DataTable();
        	
            dt.Columns.Add("Key1");
            dt.Columns.Add("Value1");
            dt.Columns.Add("Key2");
            
            dr = dt.NewRow();
            dr["Key1"] = "Proposal Form";
            dr["Key2"] = "PROPOSALFORM";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.PROPOSALFORM_KEY).Remove(1,1);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Key1"] = "Photo Addendum";
            dr["Key2"] = "PHOTOADDENDUM";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHOTOADDENDUM_KEY).Remove(1, 1);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Key1"] = "Proposal Encloser";
            dr["Key2"] = "PROPOSALENCLOSERS";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.PROPOSALENCLOSERS_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Signature Page";
            dr["Key2"] = "SIGNATUREPAGE";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.SIGNATUREPAGE_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
			dr = dt.NewRow();                        
            dr["Key1"] = "Medical Report";
            dr["Key2"] = "MEDICALREPORT";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.MEDICALREPORT_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Proposal Review Slip";
            dr["Key2"] = "PROPOSALREVIEWSLIP";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.PROPOSALREVIEWSLIP_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Policy Bond";
            dr["Key2"] = "POLICYBOND";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.POLICYBOND_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();                        
            dr["Key1"] = "Nomination";
            dr["Key2"] = "NOMINATION";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.NOMINATION_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow(); 
            dr["Key1"] = "Assignment";
            dr["Key2"] = "ASSIGNMENT";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.ASSIGNMENT_KEY).Remove(1,1);
            dt.Rows.Add(dr); 
            
            dr = dt.NewRow();
            dr["Key1"] = "Alteration";
            dr["Key2"] = "ALTERATION";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.ALTERATION_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Revivals";
            dr["Key2"] = "REVIVALS";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.REVIVALS_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Policy Loans";
            dr["Key2"] = "POLICYLOANS";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.POLICYLOANS_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Surrender";
            dr["Key2"] = "SURRENDER";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.SURRENDER_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Claims";
            dr["Key2"] = "CLAIMS";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.CLAIMS_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Correspondence";
            dr["Key2"] = "CORRESPONDENCE";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.CORRESPONDENCE_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr["Key1"] = "Others";
            dr["Key2"] = "OTHERS";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.OTHERS_KEY).Remove(1,1);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Key1"] = "KYC Documents";
            dr["Key2"] = "KYCDOCUMENT";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.KYCDOCUMENT_KEY).Remove(1, 1);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Key1"] = "Delete";
            dr["Key2"] = "DELETE";
            dr["Value1"] = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION,ihConstants.DELETE_KEY).Remove(1,1);
            dt.Rows.Add(dr);
            
            
            return dt;
        }
        public DataTable ImageRelatedValue()
        {
        	
        	config=new ImageConfig(ihConstants.CONFIG_FILE_PATH);
        	DataTable dt=new DataTable();
        	
            dt.Columns.Add("Key1");
            dt.Columns.Add("Value1");
            dt.Columns.Add("Key2");

            dr = dt.NewRow();
            dr["Key1"] = "Rotate Angle";
            dr["Key2"] = "ROTATEANGLE";
            dr["Value1"] = config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION,ihConstants.ROTATE_ANGLE_KEY).Replace("\0","");
            dt.Rows.Add(dr);
            
//            dr = dt.NewRow();
//            dr["Key1"] = "Black ";
//            dr["Key2"] = "SKEWX";
//            dr["Value1"] = config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION,ihConstants.SKEW_X_KEY).Replace("\0","");
//            dt.Rows.Add(dr);
//            
//            dr = dt.NewRow();
//            dr["Key1"] = "Skew Angle-Y";
//            dr["Key2"] = "SKEWY";
//            dr["Value1"] = config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION,ihConstants.SKEW_Y_KEY).Replace("\0","");
//            dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["Key1"] = "Zoom To()";
            //dr["Key2"] = "ZOOMFACTOR";
            //dr["Value1"] = config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.ZOOM_FACTOR_KEY).Replace("\0", "");
            //dt.Rows.Add(dr);
            
            return dt;
        }
        public DataTable ScanConfig()
        {

            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            DataTable dt = new DataTable();

            dt.Columns.Add("Key1");
            dt.Columns.Add("Value1");
            dt.Columns.Add("Key2");

            dr = dt.NewRow();
            dr["Key1"] = "Page Splitter";
            dr["Key2"] = "PAGESPLITTER";
            dr["Value1"] = config.GetValue(ihConstants._SCAN_SECTION, ihConstants._SCAN_KEY).Replace("\0", "");
            dt.Rows.Add(dr);
            return dt;
        }
		void GrdImageKeyShrtCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
            bool validBol = true;

            
			foreach(DataGridViewRow row in grdImageKeyShrt.Rows)
			{
				//string valuei=row.Cells[1].FormattedValue.ToString();
                if (row.Cells[1].FormattedValue.ToString().Contains(e.FormattedValue.ToString().ToUpper()))
                {
                    if (row.Index != grdImageKeyShrt.CurrentCell.RowIndex)
                    {
                        MessageBox.Show("This shortcut key already assigned, try with another", "B'Zer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Cancel = true;
                        grdImageKeyShrt.RefreshEdit();
                        validBol = false;
                        break;
                    }
                    //for (int i = 0; i < dgvIndexKeys.Rows.Count; i++)
                    //{

                    //    if (dgvIndexKeys.Rows[i].Cells[1].Value != null)
                    //    {
                    //        if (row.Cells[1].FormattedValue.ToString().Trim() == dgvIndexKeys.Rows[i].Cells[1].Value.ToString().Trim())
                    //        {
                    //            MessageBox.Show("This shortcut key already assigned, try with another", "B'Zer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            e.Cancel = true;
                    //            grdImageKeyShrt.RefreshEdit();
                    //            validBol = false;
                    //            break;
                    //        }
                    //    }
                    //}
                }
			}
            if ((validBol == true) && (e.FormattedValue.ToString().Length==1))
            {
                //MessageBox.Show(grdImageKeyShrt.Rows[e.RowIndex].Cells[2].Value.ToString());
                grdImageKeyShrt.Rows[e.RowIndex].Cells[e.ColumnIndex].Value=grdImageKeyShrt.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
                config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
                config.SetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, grdImageKeyShrt.Rows[e.RowIndex].Cells[2].Value.ToString(), e.FormattedValue.ToString());
            }
            if (e.FormattedValue.ToString() == string.Empty)
            {
                grdImageKeyShrt.RefreshEdit();
            }
		}
		
		
		void GrdImageValueCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			bool validBol = true;

            if ((validBol == true) && (e.FormattedValue.ToString().Length <= 3))
            {
                //MessageBox.Show(grdImageKeyShrt.Rows[e.RowIndex].Cells[2].Value.ToString());
                grdImageValue.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = grdImageValue.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
                config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
                config.SetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, grdImageValue.Rows[e.RowIndex].Cells[2].Value.ToString(), e.FormattedValue.ToString());
            }
            if (e.FormattedValue.ToString() == string.Empty)
            {
                grdImageValue.RefreshEdit();
            }
		}
		
		void GrdImageKeyShrtCellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			grdImageKeyShrt.Rows[e.RowIndex].Cells[e.ColumnIndex].Value=grdImageKeyShrt.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
		}

        private void grdImageKeyShrt_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            grdImageKeyShrt.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = grdImageKeyShrt.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {

            if ((char.IsDigit(e.KeyChar)))
            {

                if ((e.KeyChar == '\b') || (e.KeyChar == '0') || (e.KeyChar == '1')) //allow the backspace key
                {

                    e.Handled = false;

                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }

        }
        
		void DgvIndexKeysCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
		{
			bool validBol = true;
			foreach(DataGridViewRow row in dgvIndexKeys.Rows)
			{
				string valuei=row.Cells[1].FormattedValue.ToString();
				if(row.Cells[1].FormattedValue.ToString().Contains(e.FormattedValue.ToString()))
				{
					if(row.Index!=dgvIndexKeys.CurrentCell.RowIndex)
					{
						MessageBox.Show("This shortcut key already assigned, try with another","B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
						e.Cancel=true;
						dgvIndexKeys.RefreshEdit();
                        validBol = false;
                        break;
						//grdImageKeyShrt.Rows[e.RowIndex].Cells[e.ColumnIndex].Value=grdImageKeyShrt.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
					}
                    //for (int i = 0; i < grdImageKeyShrt.Rows.Count; i++)
                    //{
                    //    if (grdImageKeyShrt.Rows[i].Cells[1].Value != null)
                    //    {
                    //        if (row.Cells[1].FormattedValue.ToString().Trim() == grdImageKeyShrt.Rows[i].Cells[1].Value.ToString().Trim())
                    //        {
                    //            MessageBox.Show("This shortcut key already assigned, try with another", "B'Zer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            e.Cancel = true;
                    //            dgvIndexKeys.RefreshEdit();
                    //            validBol = false;
                    //            break;
                    //        }
                    //    }
                    //}
				}
			}
            if ((validBol == true) && (e.FormattedValue.ToString().Length <= 3))
            {
                //MessageBox.Show(grdImageKeyShrt.Rows[e.RowIndex].Cells[2].Value.ToString());
                dgvIndexKeys.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvIndexKeys.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
                config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
                config.SetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, dgvIndexKeys.Rows[e.RowIndex].Cells[2].Value.ToString(), e.FormattedValue.ToString());
            }
            if (e.FormattedValue.ToString() == string.Empty)
            {
                dgvIndexKeys.RefreshEdit();
            }
		}
		
		void DgvIndexKeysRowLeave(object sender, DataGridViewCellEventArgs e)
		{
			dgvIndexKeys.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = dgvIndexKeys.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
		}
		
		
		void DgvIndexKeysCellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			dgvIndexKeys.Rows[e.RowIndex].Cells[e.ColumnIndex].Value=dgvIndexKeys.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
		}
		
		void GrdImageValueEditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			if (e.Control is TextBox)
            {

                TextBox tb = e.Control as TextBox;

                tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);

            }
		}

        private void grdScanning_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex==1)
            {
                grdScanning.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = e.FormattedValue;
                config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
                config.SetValue(ihConstants._SCAN_SECTION, grdScanning.Rows[e.RowIndex].Cells[2].Value.ToString(), e.FormattedValue.ToString());
            }
            if (e.FormattedValue.ToString() == string.Empty)
            {
                grdScanning.RefreshEdit();
            }
        }

        private void grdScanning_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox)
            {

                TextBox tb = e.Control as TextBox;

                tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);

            }
        }

        private void grdScanning_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //grdScanning.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = grdScanning.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
        }
        private void grdScanning_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            grdScanning.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = grdScanning.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
        }
	}
}

