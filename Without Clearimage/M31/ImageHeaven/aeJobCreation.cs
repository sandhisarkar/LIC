/*
 * Created by SharpDevelop.
 * User: user
 * Date: 4/12/2008
 * Time: 10:18 AM
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
    /// Description of aePageCount.
    /// </summary>
    public partial class aeJobCreation : Form
    {
        //NovaNet.Utils.dbCon dbcon;
        OdbcConnection sqlCon = null;
        eSTATES[] state;
        CtrlPolicy ctrlPolicy = null;
        wfePolicy policy = null;
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        Credentials crd = new Credentials();

        public aeJobCreation(OdbcConnection prmCon,Credentials prmCrd)
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //

            InitializeComponent();
            sqlCon = prmCon;
            this.Text = "B'Zer - Job Creation";
            crd = prmCrd;
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }

        void CmbBatchSelectedIndexChanged(object sender, EventArgs e)
        {

        }

        void CmbProjectLeave(object sender, EventArgs e)
        {
            PopulateBatchCombo();
        }
        private void PopulateBatchCombo()
        {
            string projKey = null;
            DataSet ds = new DataSet();
            
            dbcon = new NovaNet.Utils.dbCon();
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
        private void PopulateBoxCombo()
        {
            string batchKey = null;
            DataSet ds = new DataSet();
            
            dbcon = new NovaNet.Utils.dbCon();
            wfeBox tmpBox = new wfeBox(sqlCon);
            if (cmbBatch.SelectedValue != null)
            {
                batchKey = cmbBatch.SelectedValue.ToString();
                state = new eSTATES[2];
                state[0] = eSTATES.POLICY_INITIALIZED;
                state[1] = eSTATES.POLICY_CREATED;
                ds = tmpBox.GetBox(state, Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(batchKey));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbBox.DataSource = ds.Tables[0];
                    cmbBox.DisplayMember = ds.Tables[0].Columns[0].ToString();
                }
            }
        }
        private void PopulateProjectCombo()
        {
            DataSet ds = new DataSet();
            
            dbcon = new NovaNet.Utils.dbCon();
            wfeProject tmpProj = new wfeProject(sqlCon);
            //cmbProject.Items.Add("Select");
            ds = tmpProj.GetAllValues();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cmbProject.DataSource = ds.Tables[0];
                cmbProject.DisplayMember = ds.Tables[0].Columns[1].ToString();
                cmbProject.ValueMember = ds.Tables[0].Columns[0].ToString();
            }
        }

        void CmbBatchLeave(object sender, EventArgs e)
        {
            PopulateBoxCombo();
        }

        void CmbBoxLeave(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            state= new eSTATES[0];
            dbcon = new NovaNet.Utils.dbCon();
            DataRow dr;
            int cont = 0;

            stMsg.Text = string.Empty;
            if ((cmbProject.SelectedValue != null) && (cmbBatch.SelectedValue != null) && (cmbBox.SelectedValue != null))
            {
                dt.Columns.Add("SrlNo");
                dt.Columns.Add("Policy");
                dt.Columns.Add("Name");
                dt.Columns.Add("PageCount");
                dt.Columns.Add("Exception");
                dt.Columns.Add("Photo");

                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(cmbBox.Text), 0);
                policy = new wfePolicy(sqlCon, ctrlPolicy);

                ds = policy.GetPolicyList(state);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if ((int)eSTATES.POLICY_INITIALIZED == Convert.ToInt32(ds.Tables[0].Rows[i]["status"].ToString()) || (int)eSTATES.POLICY_MISSING == Convert.ToInt32(ds.Tables[0].Rows[i]["status"].ToString()) || (int)eSTATES.POLICY_ON_HOLD == Convert.ToInt32(ds.Tables[0].Rows[i]["status"].ToString()) || (int)eSTATES.POLICY_CREATED == Convert.ToInt32(ds.Tables[0].Rows[i]["status"].ToString()))
                    {
                        dr = dt.NewRow();
                        dr["SrlNo"] = (i - cont) + 1;
                        dr["Policy"] = ds.Tables[0].Rows[i]["policy_no"].ToString();

                        ctrlPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(cmbBox.Text), Convert.ToInt32(ds.Tables[0].Rows[i]["policy_no"].ToString()));
                        policy = new wfePolicy(sqlCon, ctrlPolicy);
                        int pStatus = policy.GetPolicyStatus();
                        if (pStatus == (int)eSTATES.POLICY_ON_HOLD)
                        {
                            dr["Exception"] = ihConstants._HOLD_POLICY_EXCEPTION;
                        }
                        if (pStatus == (int)eSTATES.POLICY_MISSING)
                        {
                            dr["Exception"] = ihConstants._MISSING_POLICY_EXCEPTION;
                        }

                        dr["Name"] = ds.Tables[0].Rows[i]["name_of_policyholder"].ToString();
                        if ((pStatus != (int)eSTATES.POLICY_MISSING) && (pStatus != (int)eSTATES.POLICY_ON_HOLD))
                        {
                            if (ds.Tables[0].Rows[i]["count_of_pages"].ToString() != string.Empty)
                            {
                                dr["PageCount"] = ds.Tables[0].Rows[i]["count_of_pages"].ToString();
                            }
                            else
                            {
                                dr["PageCount"] = "15"; // For default value in page count column
                            }
                        }
                        else
                        {
                            dr["PageCount"] = "0";
                        }
                        //else
                        //{
                        //    dr["PageCount"]=0;
                        //}
                        if (ds.Tables[0].Rows[i]["photo"].ToString() != string.Empty)
                        {
                            dr["photo"] = ds.Tables[0].Rows[i]["photo"].ToString();
                        }
                        else
                        {
                            dr["Photo"] = ihConstants._POLICY_DOES_NOT_CONTAINS_PHOTO;
                        }
                        dt.Rows.Add(dr);
                    }
                    else
                    {
                        cont = cont + 1;
                    }
                }
                grdPolicy.DataSource = dt;
                grdPolicy.Columns["SrlNo"].ReadOnly = true;
                grdPolicy.Columns["Policy"].ReadOnly = true;
                grdPolicy.Columns["Name"].ReadOnly = true;
                grdPolicy.Columns["Name"].Width = 200;
                grdPolicy.Columns["PageCount"].ReadOnly = false;
                grdPolicy.Columns["Exception"].ReadOnly = false;
                grdPolicy.Columns["Photo"].ReadOnly = false;
                //for (int i = 0; i < grdPolicy.Rows.Count; i++)
                //{
                //    if (grdPolicy.Rows[i].Cells[3].Value.ToString() != string.Empty)
                //    {
                //        if (Convert.ToInt32(grdPolicy.Rows[i].Cells[3].Value.ToString()) == 0)
                //        {
                //            grdPolicy.Rows[i].ReadOnly = true;
                //        }
                //    }
                //    if (grdPolicy.Rows[i].Cells[4].Value.ToString() != string.Empty)
                //    {
                //        if (Convert.ToInt32(grdPolicy.Rows[i].Cells[4].Value.ToString()) == 1)
                //        {
                //            grdPolicy.Rows[i].ReadOnly = true;
                //        }
                //    }
                //}
            }
        }

        void aeJobCreationLoad(object sender, EventArgs e)
        {
            cmbProject.TabIndex = 1;
            PopulateProjectCombo();
        }
        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!(char.IsDigit(e.KeyChar)))
            {

                if (e.KeyChar != '\b') //allow the backspace key
                {

                    e.Handled = true;

                }

            }

        }

        void GrdPolicyEditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox)
            {

                TextBox tb = e.Control as TextBox;

                tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);

            }
        }

        void GrdPolicyCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.FormattedValue.ToString() != string.Empty)
            {
                if ((e.ColumnIndex == 4) || (e.ColumnIndex == 5))
                {
                    if (Convert.ToInt32(e.FormattedValue.ToString()) > 1)
                    {
                        grdPolicy.RefreshEdit();
                    }
                }
            }
            else
            {
                if (e.ColumnIndex != 4)
                {
                    grdPolicy.RefreshEdit();
                }
            }
            //			if(Convert.ToInt32(e.FormattedValue) > 20)
            //			{
            //				grdPolicy.RefreshEdit();
            //			}
        }

        void GrdPolicyRowLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        void CmdSaveClick(object sender, EventArgs e)
        {
            string policyNumber;
            int pageCount;
            DialogResult result;
            int photo;
            NovaNet.wfe.eSTATES state = new NovaNet.wfe.eSTATES();
            result = MessageBox.Show("Are you sure, you want to save details ?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (Validation() == true)
                {
                    for (int i = 0; i < grdPolicy.Rows.Count; i++)
                    {
                        policyNumber = grdPolicy.Rows[i].Cells[1].Value.ToString();
                        ctrlPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(cmbBox.Text), Convert.ToInt32(policyNumber));
                        policy = new wfePolicy(sqlCon, ctrlPolicy);
                        if (grdPolicy.Rows[i].Cells[3].Value.ToString() != string.Empty)
                        {
                            pageCount = Convert.ToInt32(grdPolicy.Rows[i].Cells[3].Value.ToString());
                            photo = Convert.ToInt32(grdPolicy.Rows[i].Cells[5].Value.ToString());
                            if(grdPolicy.Rows[i].Cells[4].Value.ToString().Trim() != string.Empty)
                            {
	                            if (Convert.ToInt32(grdPolicy.Rows[i].Cells[4].Value.ToString().Trim()) == 1)
	                            {
	                                state = NovaNet.wfe.eSTATES.POLICY_MISSING;
	                            }
	                            if (Convert.ToInt32(grdPolicy.Rows[i].Cells[4].Value.ToString().Trim()) == 0)
	                            {
	                                state = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
	                            }
                            }
                            else
                            {
                            	state = NovaNet.wfe.eSTATES.POLICY_CREATED;
                            }
                            if (policy.SavePolicyPageCount(pageCount, photo, state,crd) == true)
                            {
                                if (grdPolicy.Rows[i].Cells[4].Value.ToString().Trim() != string.Empty)
                                {
                                    policy.SaveInventoryInException(crd, Convert.ToInt32(grdPolicy.Rows[i].Cells[4].Value.ToString().Trim()));
                                }
                                //MessageBox.Show(this, "Details saved successfully....", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show(this, "Error while saving the details....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                            }
                        }
                    }
                    policy.DeleteFromInventory();
                }
            }
        }

        private bool Validation()
        {
            bool validBol = true;
            for (int i = 0; i < grdPolicy.Rows.Count; i++)
            {
                if (grdPolicy.Rows[i].Cells[3].Value.ToString() != string.Empty)
                {
                    if (Convert.ToInt32(grdPolicy.Rows[i].Cells[3].Value.ToString()) == 0)
                    {
                        if (grdPolicy.Rows[i].Cells[4].Value.ToString() == string.Empty)
                        {
                            MessageBox.Show("For page count 0 you need to provide exception type", "Validation error");
                            grdPolicy.Rows[i].Selected = true;
                            validBol = false;
                            break;
                        }
                    }
                }
            }
            return validBol;
        }

        void CmdCancelClick(object sender, EventArgs e)
        {
            this.Close();
        }

        void GrdPolicyCellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //			if(e.ColumnIndex == 4)
            //			{
            //				if(Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[3].Value.ToString()) == 0)
            //				{
            //					if(grdPolicy.Rows[e.RowIndex].Cells[4].Value.ToString() == string.Empty)
            //					{
            //						MessageBox.Show("For page count 0 you need to provide exception type","Validation Error");
            //					}	
            //				}
            //			}
        }

        void GrdPolicyCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //			string policyNo = grdPolicy.Rows[e.RowIndex].Cells[1].Value.ToString();
            //			ctrlPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()),Convert.ToInt32(cmbBox.Text),Convert.ToInt32(policyNo));
            //			policy = new wfePolicy(sqlCon, ctrlPolicy);
            //			
            //			
            //			if(policy.GetInventoryInExcp() == ihConstants._HOLD_POLICY_EXCEPTION)
            //			{
            //				grdPolicy.Rows[e.RowIndex].Cells[4].Value = ihConstants._HOLD_POLICY_EXCEPTION;
            //			}
            //			if(policy.GetInventoryInExcp() == ihConstants._MISSING_POLICY_EXCEPTION)
            //			{
            //				grdPolicy.Rows[e.RowIndex].Cells[4].Value = ihConstants._MISSING_POLICY_EXCEPTION;
            //			}
        }
        
        void CmbProjectSelectedIndexChanged(object sender, EventArgs e)
        {
        	
        }
    }
}
