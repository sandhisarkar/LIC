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
    public partial class frmJobDistribution : Form
    {
        wfePolicy wPolicy = null;
        OdbcConnection sqlCon;
        public frmJobDistribution(OdbcConnection prmCon)
        {
            InitializeComponent();
            sqlCon = prmCon;
        }

        private void frmJobDistribution_Load(object sender, EventArgs e)
        {
            //PopulateProjectCombo();
            //PopulateBatchCombo();
            GenTreeView();
        }
        /*
        private void PopulateBatchCombo()
        {
            string projKey = null;
            DataSet ds = new DataSet();

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
        */
       /*
        private void PopulateProjectCombo()
        {
            DataSet ds = new DataSet();
            wfeProject tmpProj = new wfeProject(sqlCon);
            //cmbProject.Items.Add("Select");
            ds = tmpProj.GetAllValues();
            cmbProject.DataSource = ds.Tables[0];
            cmbProject.DisplayMember = ds.Tables[0].Columns[1].ToString();
            cmbProject.ValueMember = ds.Tables[0].Columns[0].ToString();
        }
*/
        private void GenTreeView()
        {
        	DataSet ds = new DataSet();
        	DataSet batchDs=new DataSet();
            wfeProject tmpProj = new wfeProject(sqlCon);
            wfeBatch tmpBatch = new wfeBatch(sqlCon);
            //cmbProject.Items.Add("Select");
            ds = tmpProj.GetAllValues();
            if (ds != null)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TreeNode nd = new TreeNode();
                    nd.Text = ds.Tables[0].Rows[i][1].ToString();
                    nd.Tag = ds.Tables[0].Rows[i][0].ToString();
                    trv.Nodes.Add(nd); //create project nodes
                    batchDs = tmpBatch.GetAllValues(Convert.ToInt32(ds.Tables[0].Rows[i][0].ToString()));
                    if(batchDs != null)
                    {
                    	for(int j = 0; j < batchDs.Tables[0].Rows.Count; j++)
                    	{
                    		TreeNode ndChild = new TreeNode();
		                    ndChild.Text = batchDs.Tables[0].Rows[j][1].ToString();
		                    ndChild.Tag = batchDs.Tables[0].Rows[j][0].ToString();
		                    trv.Nodes[i].Nodes.Add(ndChild); // create batch nodes
		                    for(int k=1;k<=10;k++)
		                    {
		                    	TreeNode ndChild2 = new TreeNode();
		                    	ndChild2.Text = k.ToString();
		                    	ndChild2.Tag = "Box";
		                    	trv.Nodes[i].Nodes[j].Nodes.Add(ndChild2); // create box nodes
		                    	ndChild2=null;
		                    }
		                    ndChild=null;
                    	}
                    }
                    nd = null;
                }
            }
        }
        /*
        void CmbProjectLeave(object sender, EventArgs e)
        {
            PopulateBatchCombo();
        }
*/
        private void cmdFetch_Click(object sender, EventArgs e)
        {
            //PopulateView();
        }

        private void PopulateView(int prmProjKey,int prmBatchKey,int prmBox)
        {
            DataSet dsAwtJobCrt = new DataSet();
            DataSet dsAwtAdf = new DataSet();
            DataSet dsAwtQc = new DataSet();
            DataSet dsAwtIndex = new DataSet();
            DataSet dsIndexed = new DataSet();
            DataSet dslicExcp = new DataSet();
            DataSet dsOnHold = new DataSet();
            DataSet dsMissing = new DataSet();
            //DataSet dsAwtQc = new DataSet();
            DataSet dsIndexIncom = new DataSet();
            DataSet dsExport = new DataSet();
            
            CtrlPolicy pPolicy = null;
            wfeBox tmpBox = new wfeBox(sqlCon);
            DataTable dt = new DataTable();
            DataSet imageCount = new DataSet();
            DataRow dr;
            NovaNet.wfe.eSTATES[] policyState;
            NovaNet.wfe.eSTATES[] policyStateIndexed = new NovaNet.wfe.eSTATES[2];
            dt.Columns.Add("Srl Number");
            dt.Columns.Add("Awt_JobCreation");
            dt.Columns.Add("Awt_ADF");
            dt.Columns.Add("Awt_QC");
            dt.Columns.Add("Awt_Index");
            dt.Columns.Add("Indexed");
            dt.Columns.Add("LicException");
            dt.Columns.Add("On Hold");
            dt.Columns.Add("Missing");
            dt.Columns.Add("Index incomplete");
            dt.Columns.Add("Exported");
			try
			{
	            pPolicy = new CtrlPolicy(prmProjKey,prmBatchKey,prmBox, 0);
	            wPolicy = new wfePolicy(sqlCon, pPolicy);
	            //populate dataset for job creation
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_INITIALIZED;
	            dsAwtJobCrt=wPolicy.GetPolicyList(policyState);
	
	            //populate dataset for awt adf
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_CREATED;
	            dsAwtAdf=wPolicy.GetPolicyList(policyState);
	            
	            //populate dataset for awt qc
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_SCANNED;
	            dsAwtQc=wPolicy.GetPolicyList(policyState);
	            
	            //populate dataset for awt index
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_QC;
	            dsAwtIndex=wPolicy.GetPolicyList(policyState);
	            
	            //populate dataset for indexed
	            policyState = new NovaNet.wfe.eSTATES[3];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
	            policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
	            policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
	            dsIndexed=wPolicy.GetPolicyList(policyState);
	            
	            //populate dataset for lic Exception
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
	            dslicExcp=wPolicy.GetPolicyList(policyState);
	            
	            //populate dataset for on hold
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
	            dsOnHold=wPolicy.GetPolicyList(policyState);
	
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_MISSING;
	            dsMissing=wPolicy.GetPolicyList(policyState);
	
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
	            dsExport=wPolicy.GetPolicyList(policyState);
	            
	            policyState = new NovaNet.wfe.eSTATES[1];
	            policyState[0] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
	            dsIndexIncom=wPolicy.GetPolicyList(policyState);
	            
	            for (int i = 0; i < 100; i++)
	            {
	                dr = dt.NewRow();
	                dr["Srl Number"] = i+1;
	
	                
	
	                if((i+1)<=dsAwtJobCrt.Tables[0].Rows.Count)
	                {
	                	dr["Awt_JobCreation"] = dsAwtJobCrt.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Awt_JobCreation"] = string.Empty;
	                }
	                
	                if((i+1)<=dsAwtAdf.Tables[0].Rows.Count)
	                {
	                	dr["Awt_ADF"] = dsAwtAdf.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Awt_ADF"] = string.Empty;
	                }
					if((i+1)<=dsAwtQc.Tables[0].Rows.Count)
	                {
	                	dr["Awt_QC"] = dsAwtQc.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Awt_QC"] = string.Empty;
	                }
	                if((i+1)<=dsAwtIndex.Tables[0].Rows.Count)
	                {
	                	dr["Awt_Index"] = dsAwtIndex.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Awt_Index"] = string.Empty;
	                }
	                if((i+1)<=dsIndexed.Tables[0].Rows.Count)
	                {
	                	dr["Indexed"] = dsIndexed.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Indexed"] = string.Empty;
	                }
	                if((i+1)<=dslicExcp.Tables[0].Rows.Count)
	                {
	                	dr["LicException"] = dslicExcp.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["LicException"] = string.Empty;
	                }
	                if((i+1)<=dsOnHold.Tables[0].Rows.Count)
	                {
	                	dr["On Hold"] = dsOnHold.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["On Hold"] = string.Empty;
	                }
	                if((i+1)<=dsMissing.Tables[0].Rows.Count)
	                {
	                	dr["Missing"] = dsMissing.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Missing"] = string.Empty;
	                }
	                if((i+1)<=dsIndexIncom.Tables[0].Rows.Count)
	                {
	                	dr["index incomplete"] = dsIndexIncom.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["index incomplete"] = string.Empty;
	                }
	                if((i+1)<=dsExport.Tables[0].Rows.Count)
	                {
	                	dr["Exported"] = dsExport.Tables[0].Rows[i][0].ToString();
	                }
	                else
	                {
	                	dr["Exported"] = string.Empty;
	                }
	                dt.Rows.Add(dr);
	            }
	            grdStatus.DataSource = dt;
	            grdStatus.ForeColor = Color.Black;
			}
			catch(Exception ex)
			{
                string err = ex.Message;
				MessageBox.Show("Error while populating the policy list......");
			}
        }
        private void frmJobDistribution_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //PopulateView();
            }
        }
        
        void GrdStatusDoubleClick(object sender, EventArgs e)
        {
        	
        }
        
        void TrvNodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        	DataSet ds = new DataSet();
            //CtrlPolicy pPolicy = null;
            //NovaNet.wfe.eSTATES[] policyState;
            NovaNet.wfe.eSTATES[] policyStateIndexed = new NovaNet.wfe.eSTATES[2];
            
        	if(e.Node.Tag.ToString() == "Box")
        	{
        		int batchKey=Convert.ToInt32( e.Node.Parent.Tag.ToString());
        		int projectKey=Convert.ToInt32( e.Node.Parent.Parent.Tag.ToString());
        		int boxNumber=Convert.ToInt32(e.Node.Text);
        		PopulateView(projectKey,batchKey,boxNumber);
        	}
        }
        
        void Label6Click(object sender, EventArgs e)
        {
        	
        }
    }
}