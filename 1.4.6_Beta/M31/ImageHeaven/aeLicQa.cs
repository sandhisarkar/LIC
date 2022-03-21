/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 4/4/2009
 * Time: 5:57 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.Odbc;
using NovaNet.Utils;
using NovaNet.wfe;
using LItems;
using System.Collections;
using System.Drawing.Imaging;
using System.IO;

namespace ImageHeaven
{
	/// <summary>
	/// Description of aeLicQa.
	/// </summary>
	public partial class aeLicQa : Form
	{
		OdbcConnection sqlCon=null;
		NovaNet.Utils.dbCon dbcon=null;
		CtrlPolicy pPolicy=null;
		private CtrlImage pImage=null;
		wfePolicy wPolicy=null;
		wfeImage wImage=null;
		private string boxNo=null;
		private string policyNumber=null;
        private string projCode = null;
        private string batchCode = null;
        private string picPath = null;
		private udtPolicy policyData=null;
		string policyPath=null;
		private int policyStatus=0;
		private int clickedIndexValue;
		private CtrlBox pBox=null;
		private int selBoxNo;
        string[] imageName;
        int policyRowIndex;
        //private CtrlBatch pBatch = null;

		//private MagickNet.Image imgQc;
		string imagePath=null;
		string photoPath=null;
		//private CtrlBox pBox=null;
		private Imagery img;
		private Imagery imgAll;
        private Credentials crd = new Credentials();
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev,Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        private string imgFileName = string.Empty;
        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed = 1;
        private DataTable gTable;
        ihwQuery wQ;
        private string selDocType = string.Empty;
        private int currntPg = 0;
        private bool firstDoc = true;
        private string prevDoc;
        private int policyLen = 0;
		public aeLicQa(OdbcConnection prmCon,Credentials prmCrd)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			this.Name = "LIC quality control" ;
			InitializeComponent();
            sqlCon = prmCon;
			img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
			//img = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);
			imgAll= IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            crd = prmCrd;
            exMailLog.SetNextLogger(exTxtLog);
            
			//img = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		private void PopulateBatchCombo()
		{
			string projKey=null;
			DataSet ds=new DataSet();
			
			dbcon=new NovaNet.Utils.dbCon();
            NovaNet.wfe.eSTATES[] bState = new NovaNet.wfe.eSTATES[2];
			wfeBatch tmpBatch=new wfeBatch(sqlCon);
			if(cmbProject.SelectedValue != null)
			{
				projKey=cmbProject.SelectedValue.ToString();
                projCode = projKey;
                wQ = new ihwQuery(sqlCon);
                if (wQ.GetSysConfigValue(ihConstants.CENT_PERCENT_FQC_KEY) == ihConstants.CENT_PERCENT_FQC_VALUE)
                {
                    ds = tmpBatch.GetAllValues(Convert.ToInt32(projKey));
                }
                else
                {
                    bState[0] = eSTATES.BATCH_FQC;
                    bState[1] = eSTATES.BATCH_READY_FOR_UAT;
                    ds = tmpBatch.GetAllValues(Convert.ToInt32(projKey),bState);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbBatch.DataSource = ds.Tables[0];
                    cmbBatch.DisplayMember = ds.Tables[0].Columns[1].ToString();
                    cmbBatch.ValueMember = ds.Tables[0].Columns[0].ToString();
                }
                else
                {
                    cmbBatch.DataSource = ds.Tables[0];
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
            if (ds.Tables[0].Rows.Count > 0)
            {
                cmbProject.DataSource = ds.Tables[0];
                cmbProject.DisplayMember = ds.Tables[0].Columns[1].ToString();
                cmbProject.ValueMember = ds.Tables[0].Columns[0].ToString();
            }
		}
		
		private void PopulateBoxDetails()
		{
			string batchKey=null;
			DataSet ds=new DataSet();
            CtrlBox cBox = new CtrlBox((int)cmbProject.SelectedValue,(int)cmbBatch.SelectedValue,0);
			dbcon=new NovaNet.Utils.dbCon();
			
			wfeBox tmpBox=new wfeBox(sqlCon,cBox);
			DataTable dt=new DataTable();
			DataSet imageCount = new DataSet();
        	DataRow dr;
        	int indexPolicyCont=0;
        	double avgSize;
        	string totSize;
        	string totPage;
        	NovaNet.wfe.eSTATES[] state=new NovaNet.wfe.eSTATES[5];
            NovaNet.wfe.eSTATES[] policyState = new NovaNet.wfe.eSTATES[5];
        	
            dt.Columns.Add("BoxNo");
            dt.Columns.Add("Policies");
            dt.Columns.Add("Ready");
			dt.Columns.Add("ScannedPages");
			dt.Columns.Add("Avg_Size");
			dt.Columns.Add("TotalSize");
			
			if(cmbBatch.SelectedValue != null)
			{
				batchKey=cmbBatch.SelectedValue.ToString();
                batchCode = batchKey;
				ds=tmpBox.GetAllBox(Convert.ToInt32(batchKey));
				if(ds.Tables[0].Rows.Count>0)
				{
					for(int i=0;i< ds.Tables[0].Rows.Count;i++)
					{
						 dr = dt.NewRow();
						 dr["BoxNo"] = ds.Tables[0].Rows[i]["box_number"];
	            		 dr["Policies"] = ds.Tables[0].Rows[i]["policy_number"].ToString();
	            		 
	            		 pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()),Convert.ToInt32(ds.Tables[0].Rows[i]["box_number"].ToString()),0);
		    			 wPolicy=new wfePolicy(sqlCon,pPolicy);
		    			 
            			 policyState[0]=NovaNet.wfe.eSTATES.POLICY_INDEXED;
                         policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                         policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                         policyState[3] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                         policyState[4] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
		    			 indexPolicyCont = wPolicy.GetPolicyCount(policyState);
		    			 
		    			 dr["Ready"] = indexPolicyCont;
		    			 
		    			 pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()),Convert.ToInt32(ds.Tables[0].Rows[i]["box_number"].ToString()),0,string.Empty,string.Empty);
		    			 wImage  = new wfeImage(sqlCon, pImage);
		    			 
	    			      state[0]=eSTATES.PAGE_INDEXED;
			              state[1]=eSTATES.PAGE_FQC;
			              state[2]=eSTATES.PAGE_CHECKED;
			              state[3]=eSTATES.PAGE_EXCEPTION;
                          state[4] = eSTATES.PAGE_EXPORTED;
                          //state[5] = eSTATES.PAGE_ON_HOLD;
				         imageCount = wImage.GetReadyImageCount(state,policyState);
		    			 totPage =imageCount .Tables[0].Rows[0]["page_count"].ToString();
		    			 dr["ScannedPages"] = totPage;
		    			 totSize=imageCount.Tables[0].Rows[0]["index_size"].ToString();
                         if (totSize != string.Empty)
                         {
                             dr["TotalSize"] = Math.Round(Convert.ToDouble(totSize), 2);
                         }
                         else
                         {
                             dr["TotalSize"] = string.Empty;
                         }
						 
		    			 if((totSize != string.Empty) && (totPage != "0"))
		    			 {
		    			 	avgSize =Math.Round(Convert.ToDouble(totSize)  / Convert.ToDouble(totPage),2);
		    			 	dr["Avg_Size"] = avgSize.ToString();
		    			 }
		    			
		    			 dt.Rows.Add(dr);
					}
					grdBox.DataSource = dt;
                    grdBox.ForeColor = Color.Black;
				}
			}
		}
		void CmbProjectLeave(object sender, EventArgs e)
		{
			PopulateBatchCombo();
		}
		
		void AeLicQaLoad(object sender, EventArgs e)
		{
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();
            System.Windows.Forms.ToolTip otherToolTip = new System.Windows.Forms.ToolTip();
            this.WindowState = FormWindowState.Maximized;
			PopulateProjectCombo();
			rdoShowAll.Checked = true;
            cmdZoomIn.ForeColor = Color.Black;
            cmdZoomOut.ForeColor = Color.Black;
            chkRejectBatch.Visible = false;
            bttnToolTip.SetToolTip(cmdZoomIn,"Shortcut Key- (+)") ;
            bttnToolTip.SetToolTip(cmdZoomOut,"Shortcut Key- (-)") ;
            label6.ForeColor = Color.Black;
            label7.ForeColor = Color.Black;
            label8.ForeColor = Color.Black;
            label9.ForeColor = Color.Black;
            txtPolicyNumber.ForeColor = Color.DarkRed;
            txtName.ForeColor = Color.DarkRed;
		}
		
		void CmbBatchLeave(object sender, EventArgs e)
		{
            try
            {
                if ((cmbProject.SelectedValue != null) && (cmbBatch.SelectedValue != null))
                {
                    wfeBox wBox;
                    PopulateBoxDetails();
                    eSTATES state = new eSTATES();

                    eSTATES[] tempState = new eSTATES[5];
                    eSTATES[] policyState = new eSTATES[5];
                    pBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), 0);
                    wBox = new wfeBox(sqlCon, pBox);
                    lblTotPolicies.Text = wBox.GetTotalPolicies(state).ToString();
                    lblPolRcvd.Text = Convert.ToString((Convert.ToInt32(lblTotPolicies.Text) - Convert.ToInt32(wBox.GetTotalPolicies(eSTATES.POLICY_MISSING))));
                    lblPolHold.Text = wBox.GetTotalPolicies(eSTATES.POLICY_ON_HOLD).ToString();

                    policyState[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                    policyState[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                    policyState[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                    policyState[3] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                    policyState[4] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
                    lblScannedPol.Text = wBox.GetTotalPolicies(policyState).ToString();
                    lblBatchSz.Text = wBox.GetTotalBatchSize().ToString();
                    tempState[0] = eSTATES.PAGE_INDEXED;
                    tempState[1] = eSTATES.PAGE_FQC;
                    tempState[2] = eSTATES.PAGE_CHECKED;
                    tempState[3] = eSTATES.PAGE_EXCEPTION;
                    tempState[4] = eSTATES.PAGE_EXPORTED;
                    int scannedPol = Convert.ToInt32(lblScannedPol.Text);
                    lblAvgDocketSz.Text = Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToDouble(lblBatchSz.Text) / scannedPol), 2));
                    lblTotImages.Text = wBox.GetTotalImageCount(tempState, false, policyState).ToString();
                    lblSigCount.Text = wBox.GetTotalImageCount(tempState, true, policyState).ToString();
                    lblNetImageCount.Text = Convert.ToString(wBox.GetTotalImageCount(tempState, false, policyState) - wBox.GetTotalImageCount(tempState, true, policyState));
                    double bSize = Convert.ToInt32(lblBatchSz.Text) * 1024;
                    double tImage = Convert.ToInt32(lblTotImages.Text);
                    double aImageSize = bSize / tImage;
                    lblAvgImageSize.Text = Math.Round(aImageSize,1).ToString() + " KB";
                    wfeBatch wBatch = new wfeBatch(sqlCon);
                    if (wBatch.GetBatchStatus(Convert.ToInt32(cmbBatch.SelectedValue.ToString())) == (int)eSTATES.BATCH_READY_FOR_UAT)
                    {
                        chkReadyUat.Enabled = false;
                        chkReadyUat.Checked = true;
                        cmdAccepted.Enabled = false;
                        cmdRejected.Enabled = false;
                    }
                    else
                    {
                        chkReadyUat.Enabled = true;
                        chkReadyUat.Checked = false;
                        cmdAccepted.Enabled = true;
                        cmdRejected.Enabled = true;
                    }
                    CheckBatchRejection(cmbBatch.SelectedValue.ToString());
                    lblTotPol.Text = wBox.GetLICCheckedCount().ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating batch information........" + "  " + ex.Message);
            }
		}
		void PolicyDetails(string prmBoxNo)
		{
			DataTable dt=new DataTable();
			DataRow dr;
			DataSet ds = new DataSet();
			DataSet dsPolicy = new DataSet();
			DataSet dsImage = new DataSet();
			eSTATES[] filterState = new eSTATES[1];
        	double avgSize;
        	string totSize = string.Empty;
        	string totPage;
        	string yr;
        	string mm;
        	string dd;
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[6];

			dt.Columns.Add("SrlNo");
            dt.Columns.Add("NRCNo");
            dt.Columns.Add("Policy");
            dt.Columns.Add("Name");
            dt.Columns.Add("RegionalName");
            dt.Columns.Add("CommencementDt");
			dt.Columns.Add("DOB");
			dt.Columns.Add("ScannedPages");
			dt.Columns.Add("TotalSize");
			dt.Columns.Add("Avg_Size");
			dt.Columns.Add("STATUS");
            dt.Columns.Add("POLICYSTATUS");
			dt.Columns.Add("PROPOSALFORM");
            dt.Columns.Add("PHOTOADDENDUM");
			dt.Columns.Add("PROPOSALENCLOSERS");
            dt.Columns.Add("SIGNATUREPAGE");
            dt.Columns.Add("MEDICALREPORT");
			dt.Columns.Add("PROPOSALREVIEWSLIP");
			dt.Columns.Add("POLICYBOND");
			dt.Columns.Add("NOMINATION");
			dt.Columns.Add("ASSIGNMENT");
            dt.Columns.Add("ALTERATION");
            dt.Columns.Add("REVIVALS");
			dt.Columns.Add("POLICYLOANS");
			dt.Columns.Add("SURRENDER");
			dt.Columns.Add("CLAIMS");
			dt.Columns.Add("CORRESPONDENCE");
			dt.Columns.Add("OTHERS");
            dt.Columns.Add("KYCDOCUMENT");
            
            
			if((prmBoxNo != string.Empty) && (prmBoxNo != null) && (cmbProject.SelectedValue.ToString() != string.Empty) && (cmbProject.SelectedValue.ToString() != null) && (cmbBatch.SelectedValue.ToString() != string.Empty) && ((cmbBatch.SelectedValue.ToString() != null)))
			{
				boxNo = prmBoxNo;
				pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()),Convert.ToInt32(prmBoxNo),0);
				wPolicy=new wfePolicy(sqlCon,pPolicy);
				if(rdoShowAll.Checked == true)
				{
					eSTATES[] allState = new eSTATES[0];
					dsPolicy = wPolicy.GetPolicyList(allState);
				}
				if(rdoChecked.Checked == true)
				{
					filterState[0]=eSTATES.POLICY_CHECKED;
					dsPolicy = wPolicy.GetPolicyList(filterState);
				}
				if(rdoExceptions.Checked == true)
				{
					filterState[0]=eSTATES.POLICY_EXCEPTION;
					dsPolicy = wPolicy.GetPolicyList(filterState);
				}
				
				if(rdoOnHold.Checked == true)
				{
					filterState[0]=eSTATES.POLICY_ON_HOLD;
					dsPolicy = wPolicy.GetPolicyList(filterState);
				}
                if (rdoMissing.Checked == true)
                {
                    filterState[0] = eSTATES.POLICY_MISSING;
                    dsPolicy = wPolicy.GetPolicyList(filterState);
                }

                if (rdo150.Checked == true)
                {
                    eSTATES[] allState = new eSTATES[0];
                    dsPolicy = wPolicy.GetPolicyList(allState);
                }

                for (int i = 0; i < dsPolicy.Tables[0].Rows.Count; i++)
                {
                    pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(prmBoxNo), Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["policy_no"].ToString()), string.Empty, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);

                    //NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[4];
                    state[0] = NovaNet.wfe.eSTATES.PAGE_EXCEPTION;
                    state[1] = NovaNet.wfe.eSTATES.PAGE_INDEXED;
                    state[2] = NovaNet.wfe.eSTATES.PAGE_CHECKED;
                    state[3] = NovaNet.wfe.eSTATES.PAGE_FQC;
                    state[4] = NovaNet.wfe.eSTATES.PAGE_EXPORTED;
                    state[5] = NovaNet.wfe.eSTATES.PAGE_ON_HOLD;
                    dsImage = wImage.GetPolicyWiseImageInfo(state);
                    if (rdo150.Checked == true)
                    {
                        
                        totSize = dsImage.Tables[0].Rows[0]["qc_size"].ToString();
                        if (totSize != String.Empty)
                        {
                            double totFileSize = Convert.ToDouble(totSize) / 1024;
                            if (Convert.ToDouble(totFileSize) > ihConstants._DOCKET_MAX_SIZE)
                            {
                                if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) != (int)eSTATES.POLICY_SCANNED) && (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) != (int)eSTATES.POLICY_QC) && (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) != (int)eSTATES.POLICY_ON_HOLD))
                                {
                                    dr = dt.NewRow();
                                    dr["SrlNo"] = i + 1;
                                    dr["NRCNo"] = dsPolicy.Tables[0].Rows[i]["key_no"].ToString();
                                    dr["Policy"] = dsPolicy.Tables[0].Rows[i]["policy_no"].ToString();
                                    dr["Name"] = dsPolicy.Tables[0].Rows[i]["name_of_policyholder"].ToString();
                                    dr["RegionalName"] = dsPolicy.Tables[0].Rows[i]["unicode_name"].ToString();
                                    yr = dsPolicy.Tables[0].Rows[i]["date_of_birth"].ToString().Substring(0, 4);


                                    mm = dsPolicy.Tables[0].Rows[i]["date_of_birth"].ToString().Substring(4, 2);
                                    dd = dsPolicy.Tables[0].Rows[i]["date_of_birth"].ToString().Substring(6, 2);
                                    dr["DOB"] = yr + "/" + mm + "/" + dd;

                                    yr = dsPolicy.Tables[0].Rows[i]["date_of_commencement"].ToString().Substring(0, 4);

                                    mm = dsPolicy.Tables[0].Rows[i]["date_of_commencement"].ToString().Substring(4, 2);
                                    dd = dsPolicy.Tables[0].Rows[i]["date_of_commencement"].ToString().Substring(6, 2);
                                    dr["CommencementDt"] = yr + "/" + mm + "/" + dd;

                                    pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(prmBoxNo), Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["policy_no"].ToString()), string.Empty, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);

                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) != (int)eSTATES.POLICY_MISSING))
                                    {
                                        totPage = dsImage.Tables[0].Rows[0]["page_count"].ToString();
                                    }
                                    else
                                    {
                                        totPage = "0";
                                    }
                                    dr["ScannedPages"] = totPage;
                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) != (int)eSTATES.POLICY_MISSING))
                                    {
                                        totSize = dsImage.Tables[0].Rows[0]["qc_size"].ToString();
                                    }
                                    else
                                    {
                                        totSize = string.Empty;
                                    }
                                    if (totSize != string.Empty)
                                    {
                                        totSize = Convert.ToString(Math.Round(Convert.ToDouble(totSize), 2));
                                    }
                                    dr["TotalSize"] = totSize;

                                    dr["STATUS"] = dsPolicy.Tables[0].Rows[i]["status"];

                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_INDEXED) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_FQC) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_NOT_INDEXED))
                                    {
                                        dr["POLICYSTATUS"] = "Indexed";
                                    }
                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_ON_HOLD))
                                    {
                                        dr["POLICYSTATUS"] = "On hold";
                                    }
                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_MISSING))
                                    {
                                        dr["POLICYSTATUS"] = "Missing";
                                    }
                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_EXCEPTION))
                                    {
                                        dr["POLICYSTATUS"] = "In exception";
                                    }
                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_CHECKED))
                                    {
                                        dr["POLICYSTATUS"] = "Checked";
                                    }
                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_EXPORTED))
                                    {
                                        dr["POLICYSTATUS"] = "Exported";
                                    }
                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_SCANNED) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_QC) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_ON_HOLD))
                                    {
                                        dr["ScannedPages"] = "0";
                                        dr["TotalSize"] = string.Empty;
                                        totPage = "0";
                                        totSize = string.Empty;
                                    }
                                    if ((totSize != string.Empty) && (totPage != "0"))
                                    {
                                        avgSize = Convert.ToDouble(totSize) / Convert.ToDouble(totPage);
                                        dr["Avg_Size"] =Convert.ToString(Math.Round(avgSize,2));
                                    }
                                    if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_SCANNED) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_QC) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_ON_HOLD) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_MISSING))
                                    {
                                        dr["PROPOSALFORM"] = "0";
                                        dr["PHOTOADDENDUM"] = "0";
                                        dr["PROPOSALENCLOSERS"] = "0";
                                        dr["SIGNATUREPAGE"] = "0";
                                        dr["MEDICALREPORT"] = "0";
                                        dr["PROPOSALREVIEWSLIP"] = "0";
                                        dr["POLICYBOND"] = "0";
                                        dr["NOMINATION"] = "0";
                                        dr["ASSIGNMENT"] = "0";
                                        dr["ALTERATION"] = "0";
                                        dr["REVIVALS"] = "0";
                                        dr["POLICYLOANS"] = "0";
                                        dr["SURRENDER"] = "0";
                                        dr["CLAIMS"] = "0";
                                        dr["CORRESPONDENCE"] = "0";
                                        dr["OTHERS"] = "0";
                                        dr["KYCDOCUMENT"] = "0";
                                    }
                                    else
                                    {
                                        dr["PROPOSALFORM"] = wImage.GetDocTypeCount(ihConstants.PROPOSALFORM_FILE);
                                        dr["PHOTOADDENDUM"] = wImage.GetDocTypeCount(ihConstants.PHOTOADDENDUM_FILE);
                                        dr["PROPOSALENCLOSERS"] = wImage.GetDocTypeCount(ihConstants.PROPOSALENCLOSERS_FILE);
                                        dr["SIGNATUREPAGE"] = wImage.GetDocTypeCount(ihConstants.SIGNATUREPAGE_FILE);
                                        dr["MEDICALREPORT"] = wImage.GetDocTypeCount(ihConstants.MEDICALREPORT_FILE);
                                        dr["PROPOSALREVIEWSLIP"] = wImage.GetDocTypeCount(ihConstants.PROPOSALREVIEWSLIP_FILE);
                                        dr["POLICYBOND"] = wImage.GetDocTypeCount(ihConstants.POLICYBOND_FILE);
                                        dr["NOMINATION"] = wImage.GetDocTypeCount(ihConstants.NOMINATION_FILE);
                                        dr["ASSIGNMENT"] = wImage.GetDocTypeCount(ihConstants.ASSIGNMENT_FILE);
                                        dr["ALTERATION"] = wImage.GetDocTypeCount(ihConstants.ALTERATION_FILE);
                                        dr["REVIVALS"] = wImage.GetDocTypeCount(ihConstants.REVIVALS_FILE);
                                        dr["POLICYLOANS"] = wImage.GetDocTypeCount(ihConstants.POLICYLOANS_FILE);
                                        dr["SURRENDER"] = wImage.GetDocTypeCount(ihConstants.SURRENDER_FILE);
                                        dr["CLAIMS"] = wImage.GetDocTypeCount(ihConstants.CLAIMS_FILE);
                                        dr["CORRESPONDENCE"] = wImage.GetDocTypeCount(ihConstants.CORRESPONDENCE_FILE);
                                        dr["OTHERS"] = wImage.GetDocTypeCount(ihConstants.OTHERS_FILE);
                                        dr["KYCDOCUMENT"] = wImage.GetDocTypeCount(ihConstants.KYCDOCUMENT_FILE);
                                    }
                                    dt.Rows.Add(dr);
                                }
                            }
                        }
                    }
                    else
                    {
                        dr = dt.NewRow();
                        dr["SrlNo"] = i + 1;

                        dr["NRCNo"] = dsPolicy.Tables[0].Rows[i]["key_no"].ToString();
                        dr["Policy"] = dsPolicy.Tables[0].Rows[i]["policy_no"].ToString();
                        dr["Name"] = dsPolicy.Tables[0].Rows[i]["name_of_policyholder"].ToString();
                        dr["RegionalName"] = dsPolicy.Tables[0].Rows[i]["unicode_name"].ToString();
                        yr = dsPolicy.Tables[0].Rows[i]["date_of_birth"].ToString().Substring(0, 4);


                        mm = dsPolicy.Tables[0].Rows[i]["date_of_birth"].ToString().Substring(4, 2);
                        dd = dsPolicy.Tables[0].Rows[i]["date_of_birth"].ToString().Substring(6, 2);
                        dr["DOB"] = yr + "/" + mm + "/" + dd;

                        yr = dsPolicy.Tables[0].Rows[i]["date_of_commencement"].ToString().Substring(0, 4);

                        mm = dsPolicy.Tables[0].Rows[i]["date_of_commencement"].ToString().Substring(4, 2);
                        dd = dsPolicy.Tables[0].Rows[i]["date_of_commencement"].ToString().Substring(6, 2);
                        dr["CommencementDt"] = yr + "/" + mm + "/" + dd;


                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) != (int)eSTATES.POLICY_MISSING))
                        {
                            totPage = dsImage.Tables[0].Rows[0]["page_count"].ToString();
                        }
                        else
                        {
                            totPage = "0";
                        }
                        dr["ScannedPages"] = totPage;
                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) != (int)eSTATES.POLICY_MISSING))
                        {
                            totSize = dsImage.Tables[0].Rows[0]["qc_size"].ToString();
                        }
                        else
                        {
                            totSize = string.Empty;
                        }
                        if (totSize != string.Empty)
                        {
                            totSize = Convert.ToString(Math.Round(Convert.ToDouble(totSize), 2));
                        }
                        dr["TotalSize"] = totSize;
                        dr["STATUS"] = dsPolicy.Tables[0].Rows[i]["status"];
                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_INDEXED) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_FQC) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_NOT_INDEXED))
                        {
                            dr["POLICYSTATUS"] = "Indexed";
                        }
                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_ON_HOLD))
                        {
                            dr["POLICYSTATUS"] = "On hold";
                        }
                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_MISSING))
                        {
                            dr["POLICYSTATUS"] = "Missing";
                        }
                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_EXCEPTION))
                        {
                            dr["POLICYSTATUS"] = "In exception";
                        }
                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_CHECKED))
                        {
                            dr["POLICYSTATUS"] = "Checked";
                        }
                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_EXPORTED))
                        {
                            dr["POLICYSTATUS"] = "Exported";
                        }
                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_SCANNED) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_QC) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_ON_HOLD) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_NOT_INDEXED))
                        {
                            dr["ScannedPages"] = "0";
                            dr["TotalSize"] = string.Empty;
                            totPage = "0";
                            totSize = string.Empty;
                        }
                        if ((totSize != string.Empty) && (totPage != "0"))
                        {
                            avgSize = Convert.ToDouble(totSize) / Convert.ToDouble(totPage);
                            dr["Avg_Size"] = Convert.ToString(Math.Round(avgSize, 2));
                        }
                        if ((Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_SCANNED) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_QC) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_ON_HOLD) || (Convert.ToInt32(dsPolicy.Tables[0].Rows[i]["status"].ToString()) == (int)eSTATES.POLICY_MISSING))
                        {
                            dr["PROPOSALFORM"] = "0";
                            dr["PHOTOADDENDUM"] = "0";
                            dr["PROPOSALENCLOSERS"] = "0";
                            dr["SIGNATUREPAGE"] = "0";
                            dr["MEDICALREPORT"] = "0";
                            dr["PROPOSALREVIEWSLIP"] = "0";
                            dr["POLICYBOND"] = "0";
                            dr["NOMINATION"] = "0";
                            dr["ASSIGNMENT"] = "0";
                            dr["ALTERATION"] = "0";
                            dr["REVIVALS"] = "0";
                            dr["POLICYLOANS"] = "0";
                            dr["SURRENDER"] = "0";
                            dr["CLAIMS"] = "0";
                            dr["CORRESPONDENCE"] = "0";
                            dr["OTHERS"] = "0";
                            dr["KYCDOCUMENT"] = "0";
                        }
                        else
                        {
                            dr["PROPOSALFORM"] = wImage.GetDocTypeCount(ihConstants.PROPOSALFORM_FILE);
                            dr["PHOTOADDENDUM"] = wImage.GetDocTypeCount(ihConstants.PHOTOADDENDUM_FILE);
                            dr["PROPOSALENCLOSERS"] = wImage.GetDocTypeCount(ihConstants.PROPOSALENCLOSERS_FILE);
                            dr["SIGNATUREPAGE"] = wImage.GetDocTypeCount(ihConstants.SIGNATUREPAGE_FILE);
                            dr["MEDICALREPORT"] = wImage.GetDocTypeCount(ihConstants.MEDICALREPORT_FILE);
                            dr["PROPOSALREVIEWSLIP"] = wImage.GetDocTypeCount(ihConstants.PROPOSALREVIEWSLIP_FILE);
                            dr["POLICYBOND"] = wImage.GetDocTypeCount(ihConstants.POLICYBOND_FILE);
                            dr["NOMINATION"] = wImage.GetDocTypeCount(ihConstants.NOMINATION_FILE);
                            dr["ASSIGNMENT"] = wImage.GetDocTypeCount(ihConstants.ASSIGNMENT_FILE);
                            dr["ALTERATION"] = wImage.GetDocTypeCount(ihConstants.ALTERATION_FILE);
                            dr["REVIVALS"] = wImage.GetDocTypeCount(ihConstants.REVIVALS_FILE);
                            dr["POLICYLOANS"] = wImage.GetDocTypeCount(ihConstants.POLICYLOANS_FILE);
                            dr["SURRENDER"] = wImage.GetDocTypeCount(ihConstants.SURRENDER_FILE);
                            dr["CLAIMS"] = wImage.GetDocTypeCount(ihConstants.CLAIMS_FILE);
                            dr["CORRESPONDENCE"] = wImage.GetDocTypeCount(ihConstants.CORRESPONDENCE_FILE);
                            dr["OTHERS"] = wImage.GetDocTypeCount(ihConstants.OTHERS_FILE);
                            dr["KYCDOCUMENT"] = wImage.GetDocTypeCount(ihConstants.KYCDOCUMENT_FILE);
                        }

                        dt.Rows.Add(dr);
                    }
                }
				if(dt.Rows.Count > 0)
				{
					grdPolicy.DataSource = ds;
					grdPolicy.DataSource = dt;
				}
				else
				{
					grdPolicy.DataSource = ds;
				}

                if ((grdPolicy.Rows.Count > 0))
                {
                    for (int l = 0; l < grdPolicy.Rows.Count; l++)
                    {
                        if (Convert.ToInt32(grdPolicy.Rows[l].Cells[10].Value.ToString()) == (int)eSTATES.POLICY_CHECKED)
                        {
                            grdPolicy.Rows[l].DefaultCellStyle.ForeColor = Color.Black;
                            grdPolicy.Rows[l].DefaultCellStyle.BackColor = Color.Green;
                        }
                        if ((Convert.ToInt32(grdPolicy.Rows[l].Cells[10].Value.ToString()) == (int)eSTATES.POLICY_EXCEPTION) || (Convert.ToInt32(grdPolicy.Rows[l].Cells[10].Value.ToString()) == (int)eSTATES.POLICY_EXCEPTION))
                        {
                            grdPolicy.Rows[l].DefaultCellStyle.ForeColor = Color.Black;
                            grdPolicy.Rows[l].DefaultCellStyle.BackColor = Color.Red;
                        }
                        if ((Convert.ToInt32(grdPolicy.Rows[l].Cells[10].Value.ToString()) == (int)eSTATES.POLICY_ON_HOLD))
                        {
                            grdPolicy.Rows[l].DefaultCellStyle.ForeColor = Color.Black;
                            grdPolicy.Rows[l].DefaultCellStyle.BackColor = Color.Turquoise;
                        }
                        if ((Convert.ToInt32(grdPolicy.Rows[l].Cells[10].Value.ToString()) == (int)eSTATES.POLICY_MISSING))
                        {
                            grdPolicy.Rows[l].DefaultCellStyle.ForeColor = Color.Black;
                            grdPolicy.Rows[l].DefaultCellStyle.BackColor = Color.Magenta;
                        }
                    }

                }
                if (dt.Rows.Count > 0)
                {
                    grdPolicy.Columns[8].Visible = false;
                    grdPolicy.Columns[0].Width = 40;
                    grdPolicy.Columns[1].Width = 150;
                    grdPolicy.Columns[2].Width = 120;
                    grdPolicy.Columns[3].Width = 200;
                    grdPolicy.Columns[4].Width = 200;
                    grdPolicy.Columns[5].Visible = false;
                    grdPolicy.Columns[6].Width = 100;
                    grdPolicy.Columns[7].Width = 60;
                    grdPolicy.Columns[8].Width = 60;
                    grdPolicy.Columns[9].Width = 60;
                    //grdPolicy.Columns[4].DefaultCellStyle.Font = new Font("Arial Unicode MS", 16F, GraphicsUnit.Pixel);
                    grdPolicy.Columns[10].Visible = false;
                    grdPolicy.Columns[11].Visible = false;
                    grdPolicy.Columns[12].Visible = false;
                    grdPolicy.Columns[13].Visible = false;
                    grdPolicy.Columns[14].Visible = false;
                    grdPolicy.Columns[15].Visible = false;
                    grdPolicy.Columns[16].Visible = false;
                    grdPolicy.Columns[17].Visible = false;
                    grdPolicy.Columns[18].Visible = false;
                    grdPolicy.Columns[19].Visible = false;
                    grdPolicy.Columns[20].Visible = false;
                    grdPolicy.Columns[21].Visible = false;
                    grdPolicy.Columns[22].Visible = false;
                    grdPolicy.Columns[23].Visible = false;
                    grdPolicy.Columns[24].Visible = false;
                    grdPolicy.Columns[25].Visible = false;
                    grdPolicy.Columns[26].Visible = false;
                    grdPolicy.Columns[27].Visible = false;
                    grdPolicy.Columns[28].Visible = false;
                }
                
			}
		}
		
		void GrdBoxCellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			selBoxNo =Convert.ToInt32(grdBox.Rows[e.RowIndex].Cells[0].Value.ToString());
			PolicyDetails(grdBox.Rows[e.RowIndex].Cells[0].Value.ToString());
            grdPolicy.ForeColor = Color.Black;
		}
        private string GetPolicyPath(int policyNo)
        {
            //policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = wBatch.GetPath(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
            return batchPath + "\\" + boxNo + "\\" + policyNo;
        }
		void GrdPolicyCellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
            try
            {
                ClearPicBox();
                
                firstDoc = true;
                DataSet expDs = new DataSet();
                clickedIndexValue = e.RowIndex;
                picControl.Image = null;
                lstImage.Items.Clear();
                DisplayDockType();
                policyNumber = grdPolicy.Rows[e.RowIndex].Cells[2].Value.ToString();
                policyLen = policyNumber.Length;
                txtPolicyNumber.Text = policyNumber;
                txtName.Text = grdPolicy.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtDOB.Text = grdPolicy.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtCommDt.Text = grdPolicy.Rows[e.RowIndex].Cells[5].Value.ToString();
                policyRowIndex = e.RowIndex;
                if (Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells[7].Value.ToString()) > 0)
                {
                    for (int i = 0; i < grdPolicy.Columns.Count - 12; i++) 
                    {
                        lvwDockTypes.Items[i].SubItems[1].Text = grdPolicy.Rows[e.RowIndex].Cells[i + 12].Value.ToString();
                    }
                    lblTotFiles.Text = Convert.ToString(Math.Round(Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells[7].Value.ToString()), 2));
                    lblAvgSize.Text = Convert.ToString(Math.Round(Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells[9].Value.ToString()), 2)) + " KB";
                    //lblDock.Text = Convert.ToString(Math.Round(Convert.ToDouble(grdPolicy.Rows[e.RowIndex].Cells[7].Value.ToString()), 2)) + " KB";
                    policyStatus = Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[10].Value.ToString());
                    txtRegionalName.Text = grdPolicy.Rows[e.RowIndex].Cells[4].Value.ToString();
                    if (policyStatus == (int)eSTATES.POLICY_EXPORTED)
                    {
                        cmdAccepted.Enabled = false;
                        cmdRejected.Enabled = false;
                    }
                    else
                    {
                        cmdAccepted.Enabled = true;
                        cmdRejected.Enabled = true;
                    }
                    //lstImage.Items.Clear();
                    pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxNo), Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[2].Value.ToString()));
                    wfePolicy policy = new wfePolicy(sqlCon, pPolicy);
                    //policyData = (udtPolicy)policy.LoadValuesFromDB();
                    policyPath = GetPolicyPath(Convert.ToInt32(policyNumber)); //policyData.policy_path;
                    expDs = policy.GetAllException();
                    if (expDs.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["missing_img_exp"].ToString()) == 1)
                        {
                            chkMissingImg.Checked = true;
                        }
                        else
                        {
                            chkMissingImg.Checked = false;
                        }

                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["crop_clean_exp"].ToString()) == 1)
                        {
                            chkCropClean.Checked = true;
                        }
                        else
                        {
                            chkCropClean.Checked = false;
                        }

                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["poor_scan_exp"].ToString()) == 1)
                        {
                            chkPoorScan.Checked = true;
                        }
                        else
                        {
                            chkPoorScan.Checked = false;
                        }
                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["wrong_indexing_exp"].ToString()) == 1)
                        {
                            chkIndexing.Checked = true;
                        }
                        else
                        {
                            chkIndexing.Checked = false;
                        }
                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["linked_policy_exp"].ToString()) == 1)
                        {
                            chkLinkedPolicy.Checked = true;
                        }
                        else
                        {
                            chkLinkedPolicy.Checked = false;
                        }
                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["decision_misd_exp"].ToString()) == 1)
                        {
                            chkDesicion.Checked = true;
                        }
                        else
                        {
                            chkDesicion.Checked = false;
                        }
                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["extra_page_exp"].ToString()) == 1)
                        {
                            chkExtraPage.Checked = true;
                        }
                        else
                        {
                            chkExtraPage.Checked = false;
                        }
                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["decision_misd_exp"].ToString()) == 1)
                        {
                            chkDesicion.Checked = true;
                        }
                        else
                        {
                            chkDesicion.Checked = false;
                        }
                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["rearrange_exp"].ToString()) == 1)
                        {
                            chkRearrange.Checked = true;
                        }
                        else
                        {
                            chkRearrange.Checked = false;
                        }
                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["other_exp"].ToString()) == 1)
                        {
                            chkOther.Checked = true;
                        }
                        else
                        {
                            chkOther.Checked = false;
                        }
                        if (Convert.ToInt32(expDs.Tables[0].Rows[0]["move_to_respective_policy_exp"].ToString()) == 1)
                        {
                            chkMove.Checked = true;
                        }
                        else
                        {
                            chkMove.Checked = false;
                        }
                        txtComments.Text = expDs.Tables[0].Rows[0]["comments"].ToString() + "\r\n";
                        txtComments.SelectionStart = txtComments.Text.Length;
                        txtComments.ScrollToCaret();
                        txtComments.Refresh();
                    }
                    else
                    {
                        chkMissingImg.Checked = false;
                        chkCropClean.Checked = false;
                        chkPoorScan.Checked = false;
                        chkIndexing.Checked = false;
                        chkLinkedPolicy.Checked = false;
                        chkDesicion.Checked = false;
                        chkExtraPage.Checked = false;
                        chkDesicion.Checked = false;
                        chkRearrange.Checked = false;
                        chkOther.Checked = false;
                        chkMove.Checked = false;
                        txtComments.Text = string.Empty;
                    }

                    ArrayList arrImage = new ArrayList();
                    wQuery pQuery = new ihwQuery(sqlCon);
                    eSTATES[] state = new eSTATES[5];
                    state[0] = eSTATES.POLICY_CHECKED;
                    state[1] = eSTATES.POLICY_FQC;
                    state[2] = eSTATES.POLICY_INDEXED;
                    state[3] = eSTATES.POLICY_EXCEPTION;
                    state[4] = eSTATES.POLICY_EXPORTED;
                    CtrlImage ctrlImage;
                    arrImage = pQuery.GetItems(eITEMS.LIC_QA_PAGE, state, policy);
                    for (int i = 0; i < arrImage.Count; i++)
                    {
                        ctrlImage = (CtrlImage)arrImage[i];
                        if (ctrlImage.DocType != string.Empty)
                        {
                            lstImage.Items.Add(ctrlImage.ImageName + "-" + ctrlImage.DocType);
                        }
                        else
                            lstImage.Items.Add(ctrlImage.ImageName);
                    }
                    tabControl1.SelectedIndex = 1;
                    if (lstImage.Items.Count > 0)
                    {
                        lstImage.SelectedIndex = 0;
                        cmdAccepted.Enabled = true;
                        cmdRejected.Enabled = true;
                    }
                    
                }
                else
                {
                    cmdAccepted.Enabled = false;
                    cmdRejected.Enabled = false;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while getting the information of the selected policy.....");
                exMailLog.Log(ex);
            }
		}
		void DisplayDockType()
		{
			lvwDockTypes.Items.Clear();
			ListViewItem lvwItem = lvwDockTypes.Items.Add(ihConstants.PROPOSALFORM_FILE);
			lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add(ihConstants.PHOTOADDENDUM_FILE);
            lvwItem.SubItems.Add("0");

			lvwItem=lvwDockTypes.Items.Add(ihConstants.PROPOSALENCLOSERS_FILE);
			lvwItem.SubItems.Add("0");

			lvwItem=lvwDockTypes.Items.Add(ihConstants.SIGNATUREPAGE_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.MEDICALREPORT_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.PROPOSALREVIEWSLIP_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.POLICYBOND_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.NOMINATION_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.ASSIGNMENT_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.ALTERATION_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.REVIVALS_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.POLICYLOANS_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.SURRENDER_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.CLAIMS_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.CORRESPONDENCE_FILE);
			lvwItem.SubItems.Add("0");
			
			lvwItem=lvwDockTypes.Items.Add(ihConstants.OTHERS_FILE);
			lvwItem.SubItems.Add("0");

            lvwItem = lvwDockTypes.Items.Add(ihConstants.KYCDOCUMENT_FILE);
            lvwItem.SubItems.Add("0");
		}
		
		void LstImageSelectedIndexChanged(object sender, EventArgs e)
		{
			int pos;
			string changedImage=null;
			double fileSize;
			string currntDoc ;
			wfeImage wImage = null;
			//string photoImageName=null;

            try
            {
                pos = lstImage.SelectedItem.ToString().IndexOf("-");
                changedImage = lstImage.SelectedItem.ToString().Substring(0, pos);
                //changedImage=lstImage.SelectedItem.ToString().Substring(0,pos);
                pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxNo), Convert.ToInt32(policyNumber), changedImage, string.Empty);
                wImage = new wfeImage(sqlCon, pImage);
                changedImage = wImage.GetIndexedImageName();

                if ((policyStatus == (int)eSTATES.POLICY_INDEXED) || (policyStatus == (int)eSTATES.POLICY_CHECKED) || (policyStatus == (int)eSTATES.POLICY_EXCEPTION) || (policyStatus == (int)eSTATES.POLICY_EXPORTED))
                {
                    if (Directory.Exists(policyPath + "\\" + ihConstants._FQC_FOLDER))
                    {
                        picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                        imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                        if (changedImage.Substring(policyLen, 6) == "_000_A")
                        {
                            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                            if (File.Exists(imgFileName) == false)
                            {
                                imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
                                picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                            }
                            //img.SaveAsTiff(policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage, IGRComressionTIFF.JPEG);
                            photoPath = imagePath;
                        }
                        else
                        {
                            imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                            if (File.Exists(imgFileName) == false)
                            {
                                imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
                                picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                            }
                        }
                    }
                    else
                    {
                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
                        picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                        if (changedImage.Substring(policyLen, 6) == "_000_A")
                        {
                            imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
                            img.LoadBitmapFromFile(imgFileName);
                            //img.SaveAsTiff(policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage, IGRComressionTIFF.JPEG);
                            photoPath = imagePath;
                        }
                        else
                        {
                            imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
                            
                        }
                    }

                }
                else
                {
                    picPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                    imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                    if (changedImage.Substring(policyLen, 6) == "_000_A")
                    {
                        imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                        if (File.Exists(imgFileName) == false)
                        {
                            imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
                            picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                        }
                        //img.SaveAsTiff(policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage, IGRComressionTIFF.JPEG);
                        photoPath = imagePath;
                    }
                    else
                    {
                        imgFileName = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImage;
                        if (File.Exists(imgFileName) == false)
                        {
                            imgFileName = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImage;
                            picPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                        }
                    }

                }
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);

                fileSize = info.Length;
                fileSize = fileSize / 1024;
                lblImageSize.Text = Convert.ToString(Math.Round(fileSize, 2)) + " KB";
                img.LoadBitmapFromFile(imgFileName);
                int dashPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                //currntDoc = lstImage.Items[lstImage.SelectedIndex].ToString().Substring(dashPos);
                
                //if ((prevDoc != currntDoc))
                //{
                //    ListViewItem lvwItem = lvwDockTypes.FindItemWithText(currntDoc);
                //    lvwDockTypes.Items[lvwItem.Index].Selected = true;
                //}
                //firstDoc = false;
                if (imgFileName != string.Empty)
                {
                    ChangeSize();
                }
                //prevDoc = currntDoc;
                //ChangeSize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while generating the preview....");
                exMailLog.Log(ex);
            }
		}
		
		private void ChangeSize()
		{
            Image imgTot = null;
            try
            {
                if (img.IsValid() == true)
                {
                	if(img.GetBitmap().PixelFormat== PixelFormat.Format1bppIndexed)
            		{
                        picControl.Height = tabControl1.Height-75;
                        picControl.Width = tabControl2.Width-30;
	                    if (!System.IO.File.Exists(imgFileName)) return;
                        Image newImage;
                        imgAll.LoadBitmapFromFile(imgFileName);
                        if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                        {
                            imgAll.GetLZW("tmp1.TIF");
                            imgTot = Image.FromFile("tmp1.TIF");
                            newImage = imgTot;
                            //File.Delete("tmp1.TIF");
                        }
                        else
                        {
                            newImage = System.Drawing.Image.FromFile(imgFileName);
                        }

	                    double scaleX = (double)picControl.Width / (double)newImage.Width;
	                    double scaleY = (double)picControl.Height / (double)newImage.Height;
	                    double Scale = Math.Min(scaleX, scaleY);
	                    int w = (int)(newImage.Width * Scale);
	                    int h = (int)(newImage.Height * Scale);
	                    picControl.Width = w;
	                    picControl.Height = h;
	                    picControl.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
	                    newImage.Dispose();
	                    picControl.Refresh();
                        if (imgTot != null)
                        {
                            imgTot.Dispose();
                            imgTot = null;
                            if (File.Exists("tmp1.tif"))
                                File.Delete("tmp1.TIF");
                        }
                	}
                	else
                	{
                        picControl.Height = tabControl1.Height - 75;
                        picControl.Width = tabControl2.Width - 100;
                		img.LoadBitmapFromFile(imgFileName);
	                	picControl.Image=img.GetBitmap();
	                	picControl.SizeMode= PictureBoxSizeMode.StretchImage;
	                	picControl.Refresh();
                	}
                }
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error ..." + ex.Message, "Error");
            }
		}
		void CmdNextClick(object sender, EventArgs e)
		{
            ListViewItem lvwItem;
            if (tabControl2.SelectedIndex == 0)
            {
                if (lstImage.Items.Count > 0)
                {
                    if ((lstImage.Items.Count - 1) != lstImage.SelectedIndex)
                    {
                        lstImage.SelectedIndex = lstImage.SelectedIndex + 1;
                    }
                }
                if (tabControl2.SelectedIndex == 1)
                {
                    if (lstImage.SelectedIndex != 0)
                    {
                        int dashPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                        string currntDoc = lstImage.Items[lstImage.SelectedIndex - 1].ToString().Substring(dashPos);
                        string prevDoc = lstImage.Items[lstImage.SelectedIndex].ToString().Substring(dashPos);
                        if (currntDoc != prevDoc)
                        {
                            lvwItem = lvwDockTypes.FindItemWithText(prevDoc);
                            lvwDockTypes.Items[lvwItem.Index].Selected = true;
                            lvwDockTypes.Focus();
                            //lstImage.Focus();
                        }
                    }
                }
            }
		}
		
		void CmdPreviousClick(object sender, EventArgs e)
		{
            ListViewItem lvwItem;
            if (tabControl2.SelectedIndex == 0)
            {
                if (lstImage.SelectedIndex != 0)
                {
                    lstImage.SelectedIndex = lstImage.SelectedIndex - 1;
                }
                if (tabControl2.SelectedIndex == 1)
                {
                    if (lstImage.SelectedIndex != 0)
                    {
                        int dashPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                        string currntDoc = lstImage.Items[lstImage.SelectedIndex].ToString().Substring(dashPos);
                        string prevDoc = lstImage.Items[lstImage.SelectedIndex + 1].ToString().Substring(dashPos);
                        if (currntDoc != prevDoc)
                        {
                            lvwItem = lvwDockTypes.FindItemWithText(currntDoc);
                            lvwDockTypes.Items[lvwItem.Index].Selected = true;
                            lvwDockTypes.Focus();
                        }
                    }
                }
            }
		}
		
		void CmdAcceptedClick(object sender, EventArgs e)
		{
			string pageName;
            try
            {
                if (crd.role == ihConstants._LIC_ROLE)
                {
                    if (chkReadyUat.Checked == false)
                    {
                        if (lstImage.Items.Count > 0)
                        {
                            pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxNo), Convert.ToInt32(policyNumber));
                            wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                            wPolicy.UpdateStatus(eSTATES.POLICY_CHECKED, crd);

                            //for (int i = 0; i < lstImage.Items.Count; i++)
                            //{
                            //    pageName = lstImage.Items[i].ToString().Substring(0, lstImage.Items[i].ToString().IndexOf("-"));
                            //    pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxNo), Convert.ToInt32(policyNumber), pageName, string.Empty);
                            //    wfeImage wImage = new wfeImage(sqlCon, pImage);
                            //    wImage.UpdateStatus(eSTATES.PAGE_CHECKED, crd);
                            //}
                            CtrlImage exppImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxNo), Convert.ToInt32(policyNumber), string.Empty, string.Empty);
                            wfeImage expwImage = new wfeImage(sqlCon, exppImage);
                            expwImage.UpdateAllImageStatus(eSTATES.PAGE_CHECKED, crd);

                            wPolicy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_SOLVED, ihConstants._LIC_QA_POLICY_CHECKED);
                            grdPolicy.Rows[policyRowIndex].DefaultCellStyle.BackColor = Color.Green;
                            if ((wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_INDEXED) || (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_FQC) || (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_NOT_INDEXED))
                            {
                                grdPolicy.Rows[policyRowIndex].Cells[9].Value = "Indexed";
                            }
                            if ((wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_ON_HOLD))
                            {
                                grdPolicy.Rows[policyRowIndex].Cells[9].Value = "On hold";
                            }
                            if (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_MISSING)
                            {
                                grdPolicy.Rows[policyRowIndex].Cells[9].Value = "Missing";
                            }
                            if (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_EXCEPTION)
                            {
                                grdPolicy.Rows[policyRowIndex].Cells[9].Value = "In exception";
                            }
                            if (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED)
                            {
                                grdPolicy.Rows[policyRowIndex].Cells[9].Value = "Checked";
                            }
                            tabControl1.SelectedIndex = 0;
                            //tabControl2.SelectedIndex = 0;
                            CheckBatchRejection(cmbBatch.SelectedValue.ToString());
                        }
                    }
                    else
                    {
                        MessageBox.Show("This batch is already marked as ready for UAT.....");
                    }
                }
                else
                {
                    MessageBox.Show("You are not authorized to do this.....");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
		}

        private void ThumbnailChangeSize(string fName)
        {
            Image imgTot = null;
            try
            {
                //picBig.Height = tabControl1.Height - 75;
                //picBig.Width = tabControl2.Width - 30;
                //if (!System.IO.File.Exists(fName)) return;
                //Image newImage;
                //imgAll.LoadBitmapFromFile(fName);
                //if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                //{
                //    imgAll.GetLZW("tmp1.TIF");
                //    imgTot = Image.FromFile("tmp1.TIF");
                //    newImage = imgTot;
                //}
                //else
                //{
                //    newImage = System.Drawing.Image.FromFile(fName);
                //}
                //double scaleX = (double)picBig.Width / (double)newImage.Width;
                //double scaleY = (double)picBig.Height / (double)newImage.Height;
                //double Scale = Math.Min(scaleX, scaleY);
                //int w = (int)(newImage.Width * Scale);
                //int h = (int)(newImage.Height * Scale);
                //picBig.Width = w;
                //picBig.Height = h;
                //picBig.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                //newImage.Dispose();
                //picBig.Refresh();
                //if (imgTot != null)
                //{
                //    imgTot.Dispose();
                //    imgTot = null;
                //    if (File.Exists("tmp1.tif"))
                //        File.Delete("tmp1.TIF");
                //}
            }
            catch (Exception ex)
            {
                exMailLog.Log(ex);
                MessageBox.Show("Error ..." + ex.Message, "Error");
            }
        }


		void CmdRejectedClick(object sender, EventArgs e)
		{
			bool expBol=false;
			policyException udtExp = new policyException();
			NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
			string pageName=null;
            if (crd.role == ihConstants._LIC_ROLE)
            {
                if (chkReadyUat.Checked == false)
                {
                    if (lstImage.Items.Count > 0)
                    {
                        pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxNo), Convert.ToInt32(policyNumber));
                        wfePolicy policy = new wfePolicy(sqlCon, pPolicy);
                        if (chkCropClean.Checked == true)
                        {
                            udtExp.crop_clean_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.crop_clean_exp = 0;
                        }

                        if (chkDesicion.Checked == true)
                        {
                            udtExp.decision_misd_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.decision_misd_exp = 0;
                        }

                        if (chkExtraPage.Checked == true)
                        {
                            udtExp.extra_page_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.extra_page_exp = 0;
                        }

                        if (chkLinkedPolicy.Checked == true)
                        {
                            udtExp.linked_policy_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.linked_policy_exp = 0;
                        }

                        if (chkMissingImg.Checked == true)
                        {
                            udtExp.missing_img_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.missing_img_exp = 0;
                        }
                        if (chkMove.Checked == true)
                        {
                            udtExp.move_to_respective_policy_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.move_to_respective_policy_exp = 0;
                        }
                        if (chkOther.Checked == true)
                        {
                            udtExp.other_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.other_exp = 0;
                        }

                        if (chkPoorScan.Checked == true)
                        {
                            udtExp.poor_scan_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.poor_scan_exp = 0;
                        }
                        if (chkRearrange.Checked == true)
                        {
                            udtExp.rearrange_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.rearrange_exp = 0;
                        }
                        if (chkIndexing.Checked == true)
                        {
                            udtExp.wrong_indexing_exp = 1;
                            expBol = true;
                        }
                        else
                        {
                            udtExp.wrong_indexing_exp = 0;
                        }
                        udtExp.comments = txtComments.Text;
                        //udtExp.status = ihConstants._LIC_QA_POLICY_EXCEPTION;
                        if (expBol == true)
                        {
                            udtExp.solved = ihConstants._POLICY_EXCEPTION_NOT_SOLVED;
                            if (policy.UpdateQaPolicyException(crd, udtExp) == true)
                            {
                                if (policy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_NOT_SOLVED, ihConstants._LIC_QA_POLICY_EXCEPTION) == true)
                                {
                                    policy.UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);

                                    CtrlImage exppImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxNo), Convert.ToInt32(policyNumber), string.Empty, string.Empty);
                                    wfeImage expwImage = new wfeImage(sqlCon, exppImage);
                                    expwImage.UpdateAllImageStatus(eSTATES.PAGE_EXCEPTION, crd);
                                    grdPolicy.Rows[policyRowIndex].DefaultCellStyle.BackColor = Color.Red;
                                    if ((policy.GetPolicyStatus() == (int)eSTATES.POLICY_INDEXED) || (policy.GetPolicyStatus() == (int)eSTATES.POLICY_FQC) || (policy.GetPolicyStatus() == (int)eSTATES.POLICY_NOT_INDEXED))
                                    {
                                        grdPolicy.Rows[policyRowIndex].Cells[9].Value = "Indexed";
                                    }
                                    if ((policy.GetPolicyStatus() == (int)eSTATES.POLICY_ON_HOLD))
                                    {
                                        grdPolicy.Rows[policyRowIndex].Cells[9].Value = "On hold";
                                    }
                                    if (policy.GetPolicyStatus() == (int)eSTATES.POLICY_MISSING)
                                    {
                                        grdPolicy.Rows[policyRowIndex].Cells[9].Value = "Missing";
                                    }
                                    if (policy.GetPolicyStatus() == (int)eSTATES.POLICY_EXCEPTION)
                                    {
                                        grdPolicy.Rows[policyRowIndex].Cells[9].Value = "In exception";
                                    }
                                    if (policy.GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED)
                                    {
                                        grdPolicy.Rows[policyRowIndex].Cells[9].Value = "Checked";
                                    }
                                    //box.UpdateStatus(eSTATES.BOX_CONFLICT);
                                }
                            }
                            tabControl1.SelectedIndex = 0;
                            //tabControl2.SelectedIndex = 0;
                            CheckBatchRejection(cmbBatch.SelectedValue.ToString());
                        }
                        else
                        {
                            MessageBox.Show("Provide atleast one exception type", "B'Zer", MessageBoxButtons.OK);
                        }

                    }
                }
                else
                {
                    MessageBox.Show("This batch is already marked as ready for UAT.....");
                }
            }
            else
            {
                MessageBox.Show("You are not authorized to do this.....");
            }
			
		}
        private void CheckBatchRejection(string pBatchKey)
        {
            wfeBatch wBatch = new wfeBatch(sqlCon);
            wQ = new ihwQuery(sqlCon);
            if (chkReadyUat.Checked == false)
            {
                if (wQ.GetSysConfigValue(ihConstants.BATCH_REJECTION_KEY) != ihConstants.BATCH_REJECTION_VALUE)
                {
                    if (wBatch.PolicyWithLICException(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(pBatchKey)) == true)
                    {
                        chkRejectBatch.Visible = true;
                    }
                    else
                    {
                        chkRejectBatch.Visible = false;
                    }
                }
            }
            else
            {
                chkRejectBatch.Visible = false;
            }
        }
		void GrdPolicyCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
            //if((grdPolicy.Rows.Count > 0) && (e.RowIndex != null))
            //{
            //    if(Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[8].Value.ToString()) == (int) eSTATES.POLICY_CHECKED)
            //    {
            //        e.CellStyle.ForeColor = Color.Green; 
            //    }
            //    if((Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[8].Value.ToString()) == (int) eSTATES.POLICY_EXCEPTION) || (Convert.ToInt32(grdPolicy.Rows[e.RowIndex].Cells[8].Value.ToString()) == (int) eSTATES.POLICY_EXCEPTION))
            //    {
            //        e.CellStyle.ForeColor = Color.Red; 
            //    }
               
            //}
		}
		
		void TabControl1SelectedIndexChanged(object sender, EventArgs e)
		{
            //picBig.Visible = false;
            //panelBig.Visible = false;
            //picBig.Image = null;
            pgOne.Visible = false;
            pgThree.Visible = false;
            pgTwo.Visible = false;
            if ((grdBox.Rows.Count > 0) && (grdPolicy.Rows.Count > 0))
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    if (grdPolicy.Rows.Count > 0)
                    {
                        if (policyRowIndex < (grdPolicy.Rows.Count-1))
                        {
                            if (grdPolicy.Rows[policyRowIndex].Cells[1] != null)
                            {
                                if (grdPolicy.Rows[policyRowIndex + 1].Displayed == false)
                                {
                                    grdPolicy.FirstDisplayedScrollingRowIndex = policyRowIndex;
                                }
                                grdPolicy.Rows[policyRowIndex + 1].Selected = true;
                                grdPolicy.CurrentCell = grdPolicy.Rows[policyRowIndex + 1].Cells[1];
                                policyRowIndex = policyRowIndex + 1;
                            }
                        }
                    }
                }
                if (tabControl1.SelectedIndex == 1)
                {

                    if (lstImage.Items.Count > 0)
                    {
                        pPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxNo), Convert.ToInt32(policyNumber));
                        wfePolicy policy = new wfePolicy(sqlCon, pPolicy);
                        if (policy.GetLicExpCount() == 0)
                        {
                            if (policy.InitiateQaPolicyException(crd) == true)
                            {
                                policy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_INITIALIZED, ihConstants._LIC_QA_POLICY_VIEWED);
                            }
                        }
                    }
                }
                CtrlBox pBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue), Convert.ToInt32(cmbBatch.SelectedValue), 0);
                wfeBox wBox = new wfeBox(sqlCon, pBox);
                lblTotPol.Text =  wBox.GetLICCheckedCount().ToString();
            }
		}
		
		void GroupBox1Enter(object sender, EventArgs e)
		{
			
		}
		
		void AeLicQaFormClosing(object sender, FormClosingEventArgs e)
		{
			//sqlCon.Close();
		}

        
		
		void RdoShowAllClick(object sender, EventArgs e)
		{
			if((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
				PolicyDetails(selBoxNo.ToString());
		}
		
		void RdoCheckedClick(object sender, EventArgs e)
		{	if((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
				PolicyDetails(selBoxNo.ToString());
		}
		
		void RdoExceptionsClick(object sender, EventArgs e)
		{
			if((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
				PolicyDetails(selBoxNo.ToString());
		}
		
		void RdoOnHoldClick(object sender, EventArgs e)
		{
			if((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
				PolicyDetails(selBoxNo.ToString());
		}
		
		void RdoOnHoldCheckedChanged(object sender, EventArgs e)
		{
			
		}
		
		void RdoShowAllCheckedChanged(object sender, EventArgs e)
		{
			
		}
        private bool GetThumbnailImageAbort()
        {
            return false;
        }
        private void lvwDockTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgOne.Visible = false;
            pgTwo.Visible = false;
            pgThree.Visible = false;
            currntPg = 0;
            //if (tabControl2.SelectedIndex == 1)
            //{
                for (int i = 0; i < lvwDockTypes.Items.Count; i++)
                {
                    if (lvwDockTypes.Items[i].Selected == true)
                    {
                        selDocType = lvwDockTypes.Items[i].SubItems[0].Text;
                        ShowThumbImage(selDocType);
                        for (int j = 0; j < lstImage.Items.Count; j++)
                        {
                            string srchStr = lstImage.Items[j].ToString();
                            if (srchStr.IndexOf(selDocType) > 0)
                            {
                                lstImage.SelectedIndex = j;
                                break;
                            }
                        }
                        lstImage.Focus();
                    }
                }
            //}
        }
        private void ShowThumbImage(string pDocType)
        {
            DataSet ds = new DataSet();
            string imageFileName;
            Image imgNew = null;
            IContainerControl icc = tabControl2.GetContainerControl();
            
            //tabControl2.SelectedIndex = 1;
            //picBig.Visible = false;
            //panelBig.Visible = false;
            //picBig.Image = null;
            System.Drawing.Image imgThumbNail = null;

            pImage = new CtrlImage(Convert.ToInt32(projCode), Convert.ToInt32(batchCode), Convert.ToInt32(boxNo), Convert.ToInt32(policyNumber), string.Empty, pDocType);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            ds = wImage.GetAllIndexedImageName();
            ClearPicBox();
            if (ds.Tables[0].Rows.Count > 0)
            {
                imageName = new string[ds.Tables[0].Rows.Count];
                if (ds.Tables[0].Rows.Count <= 6)
                {
                    pgOne.Visible = true;
                    pgTwo.Visible = false;
                    pgThree.Visible = false;
                }
                if ((ds.Tables[0].Rows.Count > 6) && (ds.Tables[0].Rows.Count <= 12))
                {
                    pgOne.Visible = true;
                    pgTwo.Visible = true;
                    pgThree.Visible = false;
                }
                if ((ds.Tables[0].Rows.Count > 12) && (ds.Tables[0].Rows.Count <= 14))
                {
                    pgOne.Visible = true;
                    pgTwo.Visible = true;
                    pgThree.Visible = true;
                }
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    imageFileName = picPath + "\\" + ds.Tables[0].Rows[j][0].ToString();
                    imgAll.LoadBitmapFromFile(imageFileName);

                    if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        try
                        {
                            imgAll.GetLZW("tmp.TIF");
                            imgNew = Image.FromFile("tmp.TIF");
                            imgThumbNail = imgNew;
                        }
                        catch (Exception ex)
                        {
                            string err = ex.Message;
                        }
                    }
                    else
                    {
                        imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                    }
                    imageName[j] = imageFileName;
                    if (!System.IO.File.Exists(imageFileName)) return;
                    //imgThumbNail = Image.FromFile(imageFileName);
                    double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                    double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                    double Scale = Math.Min(scaleX, scaleY);
                    int w = (int)(imgThumbNail.Width * Scale);
                    int h = (int)(imgThumbNail.Height * Scale);
                    w = w - 5;
                    imgThumbNail = imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                    if (j == 0)
                    {
                        pictureBox1.Image = imgThumbNail;
                        pictureBox1.Tag = imageFileName;
                    }
                    if (j == 1)
                    {
                        pictureBox2.Image = imgThumbNail;
                        pictureBox2.Tag = imageFileName;
                    }
                    if (j == 2)
                    {
                        pictureBox3.Image = imgThumbNail;
                        pictureBox3.Tag = imageFileName;
                    }
                    if (j == 3)
                    {
                        pictureBox4.Image = imgThumbNail;
                        pictureBox4.Tag = imageFileName;
                    }
                    if (j == 4)
                    {
                        pictureBox5.Image = imgThumbNail;
                        pictureBox5.Tag = imageFileName;
                    }
                    if (j == 5)
                    {
                        pictureBox6.Image = imgThumbNail;
                        pictureBox6.Tag = imageFileName;
                    }
                    if (imgNew != null)
                    {
                        imgNew.Dispose();
                        imgNew = null;
                        if (File.Exists("tmp.tif"))
                            File.Delete("tmp.TIF");
                    }
                }
            }
            else
            {
                ClearPicBox();
                imageName = null;
            }
             
        }
        void ClearPicBox()
        {
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            pictureBox3.Image = null;
            pictureBox4.Image = null;
            pictureBox5.Image = null;
            pictureBox6.Image = null;
        }

        private void rdo150_CheckedChanged(object sender, EventArgs e)
        {
            //if ((grdPolicy.Rows.Count > 0))
            //{
            //    for (int l = 0; l < grdPolicy.Rows.Count; l++)
            //    {
            //        if (Convert.ToDouble(grdPolicy.Rows[l].Cells[7].Value.ToString()) == ihConstants._DOCKET_MAX_SIZE)
            //        {
            //            grdPolicy.Rows[l] DefaultCellStyle.ForeColor = Color.Green;
            //        }
            //    }

            //}
        }

        private void rdo150_Click(object sender, EventArgs e)
        {
            if ((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
                PolicyDetails(selBoxNo.ToString());
        }

        private int GetDocTypePos()
        {
            string currntDoc;
            int index = 0;
            string srchStr;
               for (int i = 0; i < lvwDockTypes.Items.Count; i++)
                {
                    if (lvwDockTypes.Items[i].Selected == true)
                    {
                        currntDoc = lvwDockTypes.Items[i].SubItems[0].Text;
                        for (int j = 0; j < lstImage.Items.Count; j++)
                        {
                            srchStr = lstImage.Items[j].ToString();
                            if (srchStr.IndexOf(currntDoc) > 0)
                            {
                                index = j;
                                break;
                            }
                        }
                        break;
                    }
                }
            return index;
        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            
            //Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 1)
                {
                    //ThumbnailChangeSize(pictureBox1.Tag.ToString());
                    
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 0 + GetDocTypePos();

                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            
			//Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 2)
                {
                    
                    //ThumbnailChangeSize(pictureBox2.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 1 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 3)
                {
                    
                    //ThumbnailChangeSize(pictureBox3.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 2 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox4_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 4)
                {
                    
                    //ThumbnailChangeSize(pictureBox4.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 3 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox5_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 5)
                {
                    
                    //ThumbnailChangeSize(pictureBox5.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 4 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void pictureBox6_DoubleClick(object sender, EventArgs e)
        {
            //Bitmap bmp;
            //picBig.Image = null;
            if (imageName != null)
            {
                if (imageName.Length >= 6)
                {
                    
                    //ThumbnailChangeSize(pictureBox6.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 5 + GetDocTypePos();
                    if (lstIndex < lstImage.Items.Count)
                    {
                        lstImage.SelectedIndex = lstIndex;
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
        }

        private void aeLicQa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //if (picBig.Visible == true)
                //{
                //    picBig.Visible = false;
                //    panelBig.Visible = false;
                //    picBig.Image = null;
                //}
            }
        }

        private void aeLicQa_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                groupBox2.Height = (this.ClientSize.Height - groupBox1.Height) - 40;
                groupBox2.Width = (this.ClientSize.Width - 10);
                tabControl2.Width = tabControl1.Width - (pictureBox.Width+50);
                tabControl2.Height = pictureBox.Height;
                //MessageBox.Show("Height - " + pictureBox1.Height + " Width - " + pictureBox1.Width);
                //panel3.Dock = DockStyle.None;
                //panel3.Width = tabControl2.Width;
                //panel3.Height = tabControl2.Height;
                //picControl.Height = tabControl2.Height - 100;
                //picControl.Width = tabControl2.Width - 80;
                //MessageBox.Show("Height - " + picControl.Height + " Width - " + picControl.Width);
                //MessageBox.Show("Height - " + tabControl2.Height + " Width - " + tabControl2.Width);
            }
        }

        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                //if (picBig.Visible == true)
                //{
                //    picBig.Visible = false;
                //    panelBig.Visible = false;
                //    picBig.Image = null;
                //}
            }
            if (e.KeyCode == Keys.Subtract)
            {
                ZoomOut();
            }
            if (e.KeyCode == Keys.Add)
            {
                ZoomIn();
            }
        }

		
		void GrdPolicyCellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			
		}
		
		void ChkMissingImgCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkMissingImg.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Missing image \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace =imgNumber + "-" + origDoctype + " Missing image \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
		}
		
		void ChkExtraPageCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkExtraPage.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Extra page \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace =imgNumber + "-" + origDoctype + " Extra page \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
		}
		
		void ChkIndexingCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkIndexing.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Wrong indexing \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Wrong indexing \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
		}
		
				
		void TxtCommentsKeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode== Keys.Enter)
			{
				
			}
		}
		
		void ChkMoveCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkMove.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Move to respective policy \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Move to respective policy \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
		}
		
		
		void ChkRearrangeCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkRearrange.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Rearrange error \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Rearrange error \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
		}
		
		void ChkLinkedPolicyCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;

            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                string imgNumber;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkLinkedPolicy.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Linked policy problem \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Linked policy problem \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
		}
		
		void ChkCropCleanCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_")+1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkCropClean.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Crop clean problem \r\n" ;
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Crop clean problem \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
		}
		
		void ChkPoorScanCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkPoorScan.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Poor scan quality \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace =imgNumber + "-" + origDoctype + " Poor scan quality \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
		}
		
		void ChkDesicionCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkDesicion.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Desicion misd \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace = imgNumber + "-" + origDoctype + " Desicion misd \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
		}
		
		void ChkOtherCheckedChanged(object sender, EventArgs e)
		{
            int tifPos;
            string origDoctype = string.Empty;
            string imgNumber;
            if (lstImage.SelectedIndex >= 0)
            {
                tifPos = lstImage.SelectedItem.ToString().IndexOf("-") + 1;
                imgNumber = lstImage.SelectedItem.ToString().Substring((lstImage.SelectedItem.ToString().IndexOf("_") + 1), 5);
                if (tifPos > 0)
                {
                    origDoctype = lstImage.SelectedItem.ToString().Substring(tifPos);
                }
                if (chkOther.Checked)
                {
                    txtComments.Text = txtComments.Text + imgNumber + "-" + origDoctype + " Other \r\n";
                    txtComments.SelectionStart = txtComments.Text.Length;
                    txtComments.ScrollToCaret();
                    txtComments.Refresh();
                }
                else
                {
                    string strToReplace;
                    strToReplace =imgNumber + "-" + origDoctype + " Other \r\n";
                    txtComments.Text = txtComments.Text.Replace(strToReplace, "");
                }
            }
        }
        /// <summary>
        /// addedd in version 1.0.0.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkReadyUat_Click(object sender, EventArgs e)
        {
            DialogResult dlg;
            wfeBatch wBatch = new wfeBatch(sqlCon);
            ///changed in version 1.0.2
            if (crd.role == ihConstants._LIC_ROLE)
            {
                if ((cmbProject.SelectedValue != null) && (cmbBatch.SelectedValue != null))
                {
                    if (1==1)
                    {
                        if ((grdBox.Rows.Count > 0) && (grdPolicy.Rows.Count > 0))
                        {
                            if (wBatch.PolicyWithLICException(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString())) == false)
                            {
                                if (chkReadyUat.Checked == true)
                                {
                                    dlg = MessageBox.Show(this, "Are you sure, this batch is ready for UAT?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (dlg == DialogResult.Yes)
                                    {
                                        wBatch.UpdateStatus(eSTATES.BATCH_READY_FOR_UAT, Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
                                        chkReadyUat.Checked = true;
                                        chkReadyUat.Enabled = false;
                                    }
                                    else
                                    {
                                        chkReadyUat.Checked = false;
                                        chkReadyUat.Enabled = true;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("One or more policies is in exception stage, clear the exceptions before proceeding....");
                                chkReadyUat.Checked = false;
                                chkReadyUat.Enabled = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Populate the box and policy details.....");
                        }
                    }
                    else
                    {
                        DialogResult rslt = MessageBox.Show(this, "Mandatory document missing in one or more policies, do you want to check the list.....", "Missing mandatory doc types", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (rslt == DialogResult.Yes)
                        {
                            frmMandPolicyList frmMiss = new frmMandPolicyList(gTable, cmbBatch.Text);
                            frmMiss.ShowDialog(this);
                            gTable.Clear();
                            gTable.Dispose();
                            chkReadyUat.Checked = false;
                        }
                        else
                        {
                            if ((grdBox.Rows.Count > 0) && (grdPolicy.Rows.Count > 0))
                            {
                                if (wBatch.PolicyWithLICException(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString())) == false)
                                {
                                    if (chkReadyUat.Checked == true)
                                    {
                                        dlg = MessageBox.Show(this, "Are you sure, this batch is ready for UAT?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (dlg == DialogResult.Yes)
                                        {
                                            wBatch.UpdateStatus(eSTATES.BATCH_READY_FOR_UAT, Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
                                            chkReadyUat.Checked = true;
                                            chkReadyUat.Enabled = false;
                                        }
                                        else
                                        {
                                            chkReadyUat.Checked = false;
                                            chkReadyUat.Enabled = true;
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("One or more policies is in exception stage, clear the exceptions before proceeding....");
                                    chkReadyUat.Checked = false;
                                    chkReadyUat.Enabled = true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Populate the box and policy details.....");
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("You are not authorized to do this.....");
                chkReadyUat.Checked = false;
            }
        }
        private bool GetMissingPoliyLst()
        {
            CtrlPolicy ctrlPolicy;
            wfePolicy wPolicy;
            bool missingDoc = false;
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()),0,0);
            wPolicy = new wfePolicy(sqlCon, ctrlPolicy);
            eSTATES[] pState = new eSTATES[4];
            DataSet pDs = new DataSet();
            DataSet iDs = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            
            dt.Columns.Add("Boxnumber");
            dt.Columns.Add("Policy");
            dt.Columns.Add("ProposalForm");
            dt.Columns.Add("ProposalReviewSlip");
            dt.Columns.Add("PolicyBond");
            dt.Columns.Add("SignaturePage");

            pState[0] = eSTATES.POLICY_CHECKED;
            pState[1] = eSTATES.POLICY_FQC;
            pState[2] = eSTATES.POLICY_EXCEPTION;
            pState[3] = eSTATES.POLICY_INDEXED;
            pDs = wPolicy.GetPolicyList(pState);
            if (pDs.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < pDs.Tables[0].Rows.Count; i++)
                {
                    ctrlPolicy = new CtrlPolicy(0, Convert.ToInt32(cmbBatch.SelectedValue.ToString()),0, Convert.ToInt32(pDs.Tables[0].Rows[i][0].ToString()));
                    wPolicy = new wfePolicy(sqlCon, ctrlPolicy);
                    iDs = wPolicy.GetMissingDocumentPolicyLst();
                    if (iDs.Tables[0].Rows.Count < 4)
                    {
                        dr = dt.NewRow();
                        dr["Boxnumber"] = pDs.Tables[0].Rows[i]["box_number"].ToString();
                        dr["Policy"] = pDs.Tables[0].Rows[i]["policy_no"].ToString();
                        for (int j = 0; j < iDs.Tables[0].Rows.Count; j++)
                        {
                            if (iDs.Tables[0].Rows[j][0].ToString() == ihConstants.PROPOSALFORM_FILE)
                            {
                                dr["ProposalForm"] = iDs.Tables[0].Rows[j][1].ToString();
                            }
                            
                            if (iDs.Tables[0].Rows[j][0].ToString() == ihConstants.PROPOSALREVIEWSLIP_FILE)
                            {
                                dr["ProposalReviewSlip"] = iDs.Tables[0].Rows[j][1].ToString();
                            }
                            
                            if (iDs.Tables[0].Rows[j][0].ToString() == ihConstants.POLICYBOND_FILE)
                            {
                                dr["PolicyBond"] = iDs.Tables[0].Rows[j][1].ToString();
                            }
                            
                            if (iDs.Tables[0].Rows[j][0].ToString() == ihConstants.SIGNATUREPAGE_FILE)
                            {
                                dr["SignaturePage"] = iDs.Tables[0].Rows[j][1].ToString();
                            }
                        }
                        missingDoc = true;
                        dt.Rows.Add(dr);
                    }
                }
            }
            if (missingDoc == true)
            {
                gTable = dt;
                return true;
            }
            else
                return false;
        }
        private void chkReadyUat_CheckedChanged(object sender, EventArgs e)
        {

        }

        int ZoomIn()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    picControl.Dock = DockStyle.None;
                    //OperationInProgress = ihConstants._OTHER_OPERATION;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height * (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width * (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    picControl.Width = Convert.ToInt32(Convert.ToDouble(picControl.Width) * 1.2);
                    picControl.Height = Convert.ToInt32(Convert.ToDouble(picControl.Height) * 1.2);
                    picControl.Refresh();
                    ChangeZoomSize();

                    //delinsrtBol = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
                exMailLog.Log(ex);
            }
            return 0;
        }
        private void ChangeZoomSize()
        {
            if (!System.IO.File.Exists(imgFileName)) return;
            Image newImage = Image.FromFile(imgFileName);
            double scaleX = (double)picControl.Width / (double)newImage.Width;
            double scaleY = (double)picControl.Height / (double)newImage.Height;
            double Scale = Math.Min(scaleX, scaleY);
            int w = (int)(newImage.Width * Scale);
            int h = (int)(newImage.Height * Scale);
            picControl.Width = w;
            picControl.Height = h;
            picControl.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
            picControl.Invalidate();
            newImage.Dispose();
        }
        int ZoomOut()
        {
            try
            {
                if (keyPressed > 0)
                {
                    picControl.Dock = DockStyle.None;
                    //OperationInProgress = ihConstants._OTHER_OPERATION;
                    keyPressed = keyPressed + 1;
                    zoomHeight = Convert.ToInt32(img.GetBitmap().Height / (1.2));
                    zoomWidth = Convert.ToInt32(img.GetBitmap().Width / (1.2));
                    zoomSize.Height = zoomHeight;
                    zoomSize.Width = zoomWidth;

                    picControl.Width = Convert.ToInt32(Convert.ToDouble(picControl.Width) / 1.2);
                    picControl.Height = Convert.ToInt32(Convert.ToDouble(picControl.Height) / 1.2);
                    picControl.Refresh();
                    ChangeZoomSize();
                    //delinsrtBol = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
            }
            return 0;
        }

        private void cmdZoomIn_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void cmdZoomOut_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void rdoMissing_Click(object sender, EventArgs e)
        {
            if ((selBoxNo.ToString() != string.Empty) && (selBoxNo != 0))
                PolicyDetails(selBoxNo.ToString());
        }

        private void chkRejectBatch_Click(object sender, EventArgs e)
        {
            DialogResult rslt;
            wfeBatch wBatch = new wfeBatch(sqlCon);
            DataSet blankDs = new DataSet();
            if (crd.role == ihConstants._LIC_ROLE)
            {
                if (chkReadyUat.Checked == false)
                {
                    if (chkRejectBatch.Checked == true)
                    {
                        if ((cmbBatch.SelectedValue != null) && (cmbProject.SelectedValue != null))
                        {
                            rslt = MessageBox.Show(this, "Are you sure you want to reject this batch?", "Batch rejection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (rslt == DialogResult.Yes)
                            {
                                if (wBatch.RejectBatch(Convert.ToInt32(cmbBatch.SelectedValue), eSTATES.BATCH_CREATED,eSTATES.BOX_INDEXED, eSTATES.POLICY_INDEXED, eSTATES.PAGE_INDEXED))
                                {
                                    MessageBox.Show("Batch rejected successfully.....");
                                    PopulateProjectCombo();
                                    PopulateBatchCombo();
                                    lblTotPolicies.Text = string.Empty;
                                    lblPolRcvd.Text = string.Empty;
                                    lblPolHold.Text = string.Empty;
                                    lblScannedPol.Text = string.Empty;
                                    lblBatchSz.Text = string.Empty;
                                    lblAvgDocketSz.Text = string.Empty;
                                    lblTotImages.Text = string.Empty;
                                    lblSigCount.Text = string.Empty;
                                    lblNetImageCount.Text = string.Empty;
                                    grdBox.DataSource = blankDs;
                                    grdPolicy.DataSource = blankDs;
                                    ClearPicBox();
                                    chkRejectBatch.Visible = false;
                                    chkReadyUat.Checked = false;
                                }
                                else
                                {
                                    MessageBox.Show("Error while updating the result, aborting.....");
                                    chkRejectBatch.Checked = false;
                                }
                            }
                            else
                            {
                                chkRejectBatch.Checked = false;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("This batch already gone for UAT, can not be rejected......");
                    chkRejectBatch.Checked = false;
                }
            }
            else
            {
                MessageBox.Show("You are not authorized to do this.....");
            }
        }

        private void pgTwo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;
            
            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            for (int i = 6; i < imageName.Length; i++)
            {
                imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                    imgAll.LoadBitmapFromFile(imageFileName);
                    currntPg = 1;
                    if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        try
                        {
                            imgAll.GetLZW("tmp.TIF");
                            imgNew = Image.FromFile("tmp.TIF");
                            imgThumbNail = imgNew;
                        }
                        catch (Exception ex)
                        {
                            string err = ex.Message;
                        }
                    }
                    else
                    {
                        imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                    }
                    double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                    double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                    double Scale = Math.Min(scaleX, scaleY);
                    int w = (int)(imgThumbNail.Width * Scale);
                    int h = (int)(imgThumbNail.Height * Scale);
                    w = w - 5;
                    imgThumbNail = imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                    if (i == 6)
                    {
                        pictureBox1.Image = imgThumbNail;
                        pictureBox1.Tag = imageFileName;
                    }
                    if (i == 7)
                    {
                        pictureBox2.Image = imgThumbNail;
                        pictureBox2.Tag = imageFileName;
                    }
                    if (i == 8)
                    {
                        pictureBox3.Image = imgThumbNail;
                        pictureBox3.Tag = imageFileName;
                    }
                    if (i == 9)
                    {
                        pictureBox4.Image = imgThumbNail;
                        pictureBox4.Tag = imageFileName;
                    }
                    if (i == 10)
                    {
                        pictureBox5.Image = imgThumbNail;
                        pictureBox5.Tag = imageFileName;
                    }
                    if (i == 11)
                    {
                        pictureBox6.Image = imgThumbNail;
                        pictureBox6.Tag = imageFileName;
                    }
                    if (imgNew != null)
                    {
                        imgNew.Dispose();
                        imgNew = null;
                        if (File.Exists("tmp.tif"))
                            File.Delete("tmp.TIF");
                    }
            }
        }

        private void pgOne_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;
            
            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            for (int i = 0; i < imageName.Length; i++)
            {
                imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);

                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                currntPg = 0;
                if (i == 0)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 1)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 2)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (i == 3)
                {
                    pictureBox4.Image = imgThumbNail;
                    pictureBox4.Tag = imageFileName;
                }
                if (i == 4)
                {
                    pictureBox5.Image = imgThumbNail;
                    pictureBox5.Tag = imageFileName;
                }
                if (i == 5)
                {
                    pictureBox6.Image = imgThumbNail;
                    pictureBox6.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        private void pgThree_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string imageFileName;
            Image imgNew = null;
            tabControl2.SelectedIndex = 1;
            
            System.Drawing.Image imgThumbNail = null;
            ClearPicBox();
            for (int i = 0; i < imageName.Length; i++)
            {
                imageFileName = imageName[i];
                if (!System.IO.File.Exists(imageFileName)) return;
                imgAll.LoadBitmapFromFile(imageFileName);
                currntPg = 2;
                if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                {
                    try
                    {
                        imgAll.GetLZW("tmp.TIF");
                        imgNew = Image.FromFile("tmp.TIF");
                        imgThumbNail = imgNew;
                    }
                    catch (Exception ex)
                    {
                        string err = ex.Message;
                    }
                }
                else
                {
                    imgThumbNail = System.Drawing.Image.FromFile(imageFileName);
                }
                double scaleX = (double)pictureBox1.Width / (double)imgThumbNail.Width;
                double scaleY = (double)pictureBox1.Height / (double)imgThumbNail.Height;
                double Scale = Math.Min(scaleX, scaleY);
                int w = (int)(imgThumbNail.Width * Scale);
                int h = (int)(imgThumbNail.Height * Scale);
                w = w - 5;
                imgThumbNail = imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

                if (i == 12)
                {
                    pictureBox1.Image = imgThumbNail;
                    pictureBox1.Tag = imageFileName;
                }
                if (i == 13)
                {
                    pictureBox2.Image = imgThumbNail;
                    pictureBox2.Tag = imageFileName;
                }
                if (i == 14)
                {
                    pictureBox3.Image = imgThumbNail;
                    pictureBox3.Tag = imageFileName;
                }
                if (imgNew != null)
                {
                    imgNew.Dispose();
                    imgNew = null;
                    if (File.Exists("tmp.tif"))
                        File.Delete("tmp.TIF");
                }
            }
        }

        private void tabControl2_TabIndexChanged(object sender, EventArgs e)
        {
            if (imgFileName != string.Empty)
            {
                if(tabControl2.SelectedIndex == 0)
                    ChangeSize();
                ThumbnailChangeSize(imgFileName);
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListViewItem lvwItem;
            string currntDoc = string.Empty;
            if (tabControl2.SelectedIndex == 1)
            {
                firstDoc = false;
                for (int i = 0; i < lvwDockTypes.Items.Count; i++)
                {
                    if (lvwDockTypes.Items[i].Selected == true)
                    {
                        currntDoc = lvwDockTypes.Items[i].SubItems[0].Text;
                        break;
                    }
                }
                if (currntDoc != string.Empty)
                {
                    lvwItem = lvwDockTypes.FindItemWithText(currntDoc);
                    lvwDockTypes.Items[lvwItem.Index].Selected = true;
                }
            }
            else
            {
                ChangeSize();
            }
            lvwDockTypes.Focus();
        }
	}
}
