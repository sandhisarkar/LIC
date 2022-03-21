/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 13/4/2008
 * Time: 6:21 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using NovaNet.Utils;
using NovaNet.wfe;
using LItems;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using System.IO;

namespace ImageHeaven
{
	/// <summary>
	/// Description of aeExport.
	/// </summary>
	public partial class aeExport : Form,StateData
	{
        MemoryStream stateLog;
        byte[] tmpWrite;
		NovaNet.Utils.dbCon dbcon;
		OdbcConnection sqlCon=null;
		eSTATES[] state;
		wfeProject tmpProj=null;
		DataSet ds=null;
		string batchCount;
		CtrlBox ctrlBox = null;
		wfeBox box = null;
		CtrlPolicy ctrPol = null;
//		CtrlPolicy ctrlPolicy = null;
		wfePolicy policy = null;
        //wfePolicy wPolicy = null;
        //CtrlPolicy pPolicy = null;
		CtrlImage pImage = null;
		wfeImage wImage = null;
		wfeBatch wBatch=null;
		private udtPolicy policyData=null;
		StreamWriter sw;
        StreamWriter expLog;
		FileorFolder exportFile;
		string error = null;
        Credentials crd = new Credentials();
        private long expImageCount = 0;
        private long expPolicyCount = 0;
        private CtrlBox pBox = null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        public Imagery img;
        private ImageConfig config = null;
		public aeExport(OdbcConnection prmCon,Credentials prmCrd)
		{
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.Text = "Export: Box level";
			dbcon=new NovaNet.Utils.dbCon();
            sqlCon = prmCon;
            crd = prmCrd;
            exMailLog.SetNextLogger(exTxtLog);
            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
        MemoryStream StateData.StateLog()
        {
            return stateLog;
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
				//batchCount = ds.Tables[0].Rows.Count;
				cmbBatch.DataSource=ds.Tables[0];
				cmbBatch.DisplayMember=ds.Tables[0].Columns[1].ToString();
				cmbBatch.ValueMember=ds.Tables[0].Columns[0].ToString();
			}
		}
		private void PopulateBox()
		{
			string batchKey=null;
			DataSet ds=new DataSet();
			
			dbcon=new NovaNet.Utils.dbCon();
			
			int policyCount;
			
			ctrlBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()),0);
			wfeBox tmpBox=new wfeBox(sqlCon,ctrlBox);
			if(cmbBatch.SelectedValue != null)
			{
				lvwExportList.Items.Clear();
				batchKey=cmbBatch.SelectedValue.ToString();
				state =new eSTATES[3];
				state[0] = eSTATES.BOX_FQC;
				state[1] = eSTATES.BOX_INDEXED;
                state[2] = eSTATES.BOX_EXPORTED;
				ds=tmpBox.GetExportableBox(state);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ctrPol = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(ds.Tables[0].Rows[i]["box_number"].ToString()), 0);
                            policy = new wfePolicy(sqlCon, ctrPol);
                            if (chkReExport.Checked == false)
                            {
                                state = new eSTATES[4];
                                state[0] = eSTATES.POLICY_CHECKED;
                                state[1] = eSTATES.POLICY_FQC;
                                state[2] = eSTATES.POLICY_INDEXED;
                                state[3] = eSTATES.POLICY_EXCEPTION;
                            }
                            else
                            {
                                state = new eSTATES[5];
                                state[0] = eSTATES.POLICY_CHECKED;
                                state[1] = eSTATES.POLICY_FQC;
                                state[2] = eSTATES.POLICY_INDEXED;
                                state[3] = eSTATES.POLICY_EXCEPTION;
                                state[4] = eSTATES.POLICY_EXPORTED;
                            }
                            policyCount = policy.GetPolicyCount(state);
                            state = new eSTATES[2];
                            state[0] = eSTATES.POLICY_ON_HOLD;
                            state[1] = eSTATES.POLICY_MISSING;
                            int holdMissingPolCount = policy.GetPolicyCount(state);
                            ListViewItem lvwItem = lvwExportList.Items.Add(ds.Tables[0].Rows[i]["box_number"].ToString());
                            lvwItem.SubItems.Add(policyCount.ToString());
                            lvwItem.SubItems.Add(holdMissingPolCount.ToString());
                        }
                    }
                }
			}
		}
		private void PopulateProjectCombo()
		{
			DataSet ds=new DataSet();
			
			tmpProj=new wfeProject(sqlCon);
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
			PopulateBox();
		}
		void AeExportLoad(object sender, EventArgs e)
		{
            //System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
			PopulateProjectCombo();
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            string Val = config.GetValue(ihConstants.EXPORT_FOLDER_SECTION, ihConstants.EXPORT_FOLDER_KEY).Trim();
            int len = Val.IndexOf('\0');
            Val = Val.Substring(0,len);
            if (Val == string.Empty)
            {
                config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
                config.SetValue(ihConstants.EXPORT_FOLDER_SECTION, ihConstants.EXPORT_FOLDER_KEY, ihConstants._EXPORT_DRIVE);
            }
		}
        DataRelation SetRelationPolicyRawData(DataSet pDs, DataSet rDs)
        {
            DataColumn dcpolicyNm = pDs.Tables[0].Columns["policy_number"];
            DataColumn dcRwPolicyNm = rDs.Tables[0].Columns["policy_no"];
            DataColumn[] dcPolicy = new DataColumn[1];
            DataColumn[] dcRaw = new DataColumn[1];
            dcPolicy[0] = dcpolicyNm;
            dcRaw[0] = dcRwPolicyNm;
            return pDs.Relations.Add("PolicyMasterToRawData", dcpolicyNm, dcRwPolicyNm);
        }
		void CmdExportClick(object sender, EventArgs e)
		{
			string divisionCode;
			string branchCode;
			string batchSerial;
			string boxno = string.Empty;
			ArrayList arrPolicy=new ArrayList();
			wQuery pQuery=new ihwQuery(sqlCon);
			string policyNumber = string.Empty;
			DataTable policyDtls = new DataTable();
            DataTable imageDs = new DataTable();
			string imagePath;
			string vendorCode;
			string versionNumber;
			string ImageCount = string.Empty;
			string DocTypeCount=string.Empty;
			//string totalBatchCount;
			string Scanuploadflag;
			string scanneddate;
			//string IncrementedScan;
			string Policyholdername;
			string DOB;
			string dateofcommencement;
			string cust_id;
			string[] imageName;
			string batchPath = string.Empty;
			string exportPath = string.Empty;
			int status;
			string policyPath;
			//string imagePath;
            string fileName = string.Empty;
			string documentType=null;
			//string FileName=null;
			string rootPath;
			exportFile = new FileorFolder();
			ArrayList docType = new ArrayList();
			ArrayList multiPageFileName = new ArrayList();
            OdbcTransaction exportTrans = null;
            bool expBol = true;
            OdbcConnection expSqlCon = null;
            int totPolicyCount = 0;
            string serial_no;
            int policyTotPage = 0;
            bool proposalExists;
            bool signatureExists;
            int pgCountWhlErr = 0;
            string appendPath = string.Empty;
            int tmp = 0;
            OdbcDataAdapter pAdp = new OdbcDataAdapter();
            OdbcDataAdapter iAdp = null;
            DataSet pDs = new DataSet();
            DataSet rDs = new DataSet();
            DataSet iDs = null;
            int maxSerial = 0;
            int normalSerial = 0;
            string[] lastLine = null;
            string tempPath = string.Empty;
            int totImage =0;
            string expFolder;
            //System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.LowLatency;
				try
				{
                    config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
                    expFolder = config.GetValue(ihConstants.EXPORT_FOLDER_SECTION, ihConstants.EXPORT_FOLDER_KEY).Trim();
                    int len = expFolder.IndexOf('\0');
                    expFolder = expFolder.Substring(0,len);

					tmpProj=new wfeProject(sqlCon);
                    ds = new DataSet();
					ds = tmpProj.GetConfiguration();
					
					vendorCode = ds.Tables[0].Rows[0]["VENDOR_CODE"].ToString();
					versionNumber = ds.Tables[0].Rows[0]["VERSION_NUMBER"].ToString();
                    ds.Dispose();
                    txtMsg.Text = string.Empty;
                    ds = new DataSet();
                    ds = tmpProj.GetMainConfiguration(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
                    divisionCode = ds.Tables[0].Rows[0]["DO_CODE"].ToString();
                    branchCode = ds.Tables[0].Rows[0]["BO_CODE"].ToString();
					wBatch = new wfeBatch(sqlCon);
					batchPath = wBatch.GetPath(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
                    //batchPath = "C:"; 
                    if (expFolder != string.Empty)
                    {
                        if (Directory.Exists(expFolder))
                        {
                            appendPath = expFolder;
                            txtMsg.Text = "Export folder : \r\n";
                            txtMsg.Text = txtMsg.Text + appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\r\n";
                            txtMsg.SelectionStart = txtMsg.Text.Length;
                            txtMsg.ScrollToCaret();
                            txtMsg.Refresh();
                        }
                        else
                        {

                            appendPath = ihConstants._EXPORT_DRIVE;
                            txtMsg.Text = "Given folder does not exists, export folder : \r\n";
                            txtMsg.Text = txtMsg.Text +  appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\r\n";
                            txtMsg.SelectionStart = txtMsg.Text.Length;
                            txtMsg.ScrollToCaret();
                            txtMsg.Refresh();
                        }
                    }
                    else
                    {
                        appendPath = ihConstants._EXPORT_DRIVE;
                        txtMsg.Text = "Export to default folder : \r\n";
                        txtMsg.Text = txtMsg.Text + appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\r\n";
                        txtMsg.SelectionStart = txtMsg.Text.Length;
                        txtMsg.ScrollToCaret();
                        txtMsg.Refresh();
                    }
                    
                    tempPath = appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + ihConstants._LOCAL_APPEND_TEXT_FOLDER;
                    if (Directory.Exists(appendPath + ihConstants._EXPORT_FOLDER + "\\" + ihConstants._LOCAL_APPEND_TEXT_FOLDER) == false)
                    {
                        Directory.CreateDirectory(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + ihConstants._LOCAL_APPEND_TEXT_FOLDER);
                    }
                    if (Directory.Exists(appendPath + "\\" + ihConstants._EXPORT_FOLDER) == false)
                    {
                        Directory.CreateDirectory(appendPath + "\\" + ihConstants._EXPORT_FOLDER);
                        Directory.CreateDirectory(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text);
                    }
                    else
                    {
                        if (Directory.Exists(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text))
                        {
                            if (chkReExport.Checked == true)
                            {
                                Directory.Delete(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text, true);
                                Directory.CreateDirectory(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text);
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text);
                        }
                    }
                    if(wBatch.GetBatchStatus(Convert.ToInt32(cmbBatch.SelectedValue.ToString())) == (int) eSTATES.BATCH_READY_FOR_UAT)
                    {
                        DateTime stDt = DateTime.Now;
                        txtMsg.Text = txtMsg.Text + "Batch Number : \r\n";
                        txtMsg.Text = txtMsg.Text + cmbBatch.Text + "\r\n";
                        txtMsg.SelectionStart = txtMsg.Text.Length;
                        txtMsg.ScrollToCaret();
                        txtMsg.Refresh();

                        tblExp.SelectedIndex = 1;
                        DateTime stDtTot = DateTime.Now;
                        NovaNet.Utils.dbCon dbcon;
                        dbcon = new NovaNet.Utils.dbCon();
                        //expSqlCon = dbcon.Connect();

                        ctrlBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()),0);
                        box = new wfeBox(sqlCon, ctrlBox);
                        policy = new wfePolicy(sqlCon);
                        
                        pDs = policy.GetAllPolicyDetails(box, out pAdp);
                        rDs = policy.GetAllPolicyDetailsRaw(box);
                        maxSerial = 0;
                        batchCount = policy.GetBatchSerial(box);
                        batchSerial = divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount;
                        fileName = vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + ".TXT";
                        if(chkReExport.Checked != true)
                        {
	                        if (File.Exists(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text + "\\" + fileName))
	                        {
	                            lastLine = File.ReadAllLines(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text + "\\" + fileName);
	                            maxSerial =Convert.ToInt32(lastLine[lastLine.Length - 1].Substring(0, 5));
	                        }
                        }
                        else
                        {
                        	maxSerial = 0;
                        }
                        
					    for (int i=0;i<lvwExportList.Items.Count;i++) //Counter for Box
					    {
						    if(lvwExportList.Items[i].Checked == true)
						    {
                                if ((lvwExportList.Items[i].SubItems[1].Text.ToString() != "0") || ((lvwExportList.Items[i].SubItems[2].Text.ToString() != "0")))
                                {
                                    DataRow[] dtr = null;
                                    sw = new StreamWriter(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text + "\\" + fileName, true);
                                    
                                    state = new eSTATES[0];
                                    if (pDs.Tables.Count > 0)
                                    {
                                        
                                        if (pDs.Tables[0].Rows.Count > 0)
                                        {

                                            dtr = pDs.Tables[0].Select("proj_key = " + Convert.ToInt32(cmbProject.SelectedValue.ToString()) + " and batch_key=" + Convert.ToInt32(cmbBatch.SelectedValue.ToString()) + " and box_number=" + Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()));
                                            //rDtR = rDs.Tables[0].Select("policy_no=" + policyNumber);
                                        }
                                    }
                                    boxno = lvwExportList.Items[i].SubItems[0].Text.ToString().PadLeft(3, Convert.ToChar("0"));
                                    //ctrlBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()));
                                    //box = new wfeBox(sqlCon, ctrlBox);
                                    //arrPolicy = pQuery.GetItems(eITEMS.POLICY, state, box);
                                    expImageCount = 0;
                                    expPolicyCount = 0;
                                    pgCountWhlErr = 0;
                                    expBol = true;

                                    //txtMsg.Text = txtMsg.Text + "Wait while exporting box : ";
                                    //txtMsg.Text = txtMsg.Text + boxno + "\r\n";
                                    //txtMsg.SelectionStart = txtMsg.Text.Length;
                                    //txtMsg.ScrollToCaret();
                                    //txtMsg.Refresh();

                                    iAdp = new OdbcDataAdapter();
                                    ///For all policy details

                                    //drelPolicyRaw = SetRelationPolicyRawData(pDs, rDs);
                                    pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxno), Convert.ToInt32("0"), string.Empty, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);
                                    iDs = new DataSet();
                                    iDs = wImage.GetAllImages(out iAdp);

                                    for (int j = 0; j < dtr.Length; j++) //Loop within box for all the policies
                                    {
                                        int docCount = 0;
                                        //DataRow[] pDtR = new DataRow[1];
                                        DataRow[] rDtR = new DataRow[1];
                                        DataRow[] iDtr = null;
                                        //expBol = true;
                                        pgCountWhlErr = 0;
                                        //exportTrans = expSqlCon.BeginTransaction();
                                        boxno = lvwExportList.Items[i].SubItems[0].Text.ToString().PadLeft(3, Convert.ToChar("0"));
                                        expPolicyCount = expPolicyCount + 1;
                                        //imageDs = new DataSet();
                                        //ctrPol = (CtrlPolicy)arrPolicy[j];
                                        policyNumber = dtr[j]["policy_number"].ToString(); //ctrPol.PolicyNumber.ToString();
                                        policy = new wfePolicy(sqlCon, ctrPol);
                                        if (pDs.Tables.Count > 0)
                                        {
                                            if (pDs.Tables[0].Rows.Count > 0)
                                            {
                                                //policyDtls = pDs.Tables[0];
                                                //pDtR = policyDtls.Select("proj_key = " + Convert.ToInt32(cmbProject.SelectedValue.ToString()) + " and batch_key=" + Convert.ToInt32(cmbBatch.SelectedValue.ToString()) + " and box_number=" + Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()) + " and policy_number=" + policyNumber);
                                                rDtR = rDs.Tables[0].Select("trim(policy_no)=" + policyNumber);
                                            }
                                        }

                                        //batchCount = rDtR[0]["batch_serial"].ToString(); 
                                        //policyDtls.Rows[j]["serial_number"].ToString();
                                        
                                        //policyData = (udtPolicy)policy.LoadValuesFromDB();
                                        //batchPath = appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text;
                                        policyPath = batchPath + "\\" + lvwExportList.Items[i].SubItems[0].Text.ToString() + "\\" + policyNumber;


                                        imagePath = "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber.PadLeft(9,'0');
                                        rootPath = imagePath;
                                        
                                        
                                        //if (Directory.Exists(".\\" + ihConstants._LOCAL_APPEND_TEXT_FOLDER) == false)
                                        //{
                                        //    Directory.CreateDirectory(".\\" + ihConstants._LOCAL_APPEND_TEXT_FOLDER);
                                        //}
                                        //if (File.Exists(batchPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + fileName))
                                        //{
                                        //    File.Move(batchPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + fileName, ".\\" + ihConstants._LOCAL_APPEND_TEXT_FOLDER + "\\" + fileName);
                                        //}
                                        //sw = new StreamWriter(".\\" + ihConstants._LOCAL_APPEND_TEXT_FOLDER + "\\" + fileName, true);
                                        //policyDtls	= policy.GetPolicyDetails();

                                        Policyholdername = rDtR[0]["name_of_policyholder"].ToString().PadRight(30, Convert.ToChar(" ")); //policyDtls.Rows[j]["name_of_policyholder"].ToString().PadRight(30, Convert.ToChar(" "));
                                        DOB = rDtR[0]["date_of_birth"].ToString(); //policyDtls.Rows[j]["date_of_birth"].ToString();
                                        dateofcommencement = rDtR[0]["date_of_commencement"].ToString(); //policyDtls.Rows[j]["date_of_commencement"].ToString();
                                        scanneddate = dtr[j]["scanned_date"].ToString(); //policyDtls.Rows[j]["scanned_date"].ToString();

                                        Scanuploadflag = ihConstants.SCAN_SUCCESS_FLAG; //dtr[j]["Scan_upload_flag"].ToString(); //policyDtls.Rows[j]["Scan_upload_flag"].ToString();
                                        cust_id = rDtR[0]["customer_id"].ToString(); //policyDtls.Rows[j]["customer_id"].ToString();
                                        status = Convert.ToInt32(dtr[j]["status"].ToString()); //Convert.ToInt32(policyDtls.Rows[j]["status"].ToString());
                                        if ((status != (int)eSTATES.POLICY_EXPORTED) && (chkReExport.Checked != true))
                                        {
                                            toolStatus.Text ="Box: " + lvwExportList.Items[i].SubItems[0].Text.ToString() + " policy - " + policyNumber;
                                            //txtMsg.Text = txtMsg.Text + " Policy : " + policyNumber;
                                            //txtMsg.SelectionStart = txtMsg.Text.Length;
                                            //txtMsg.ScrollToCaret();
                                            //txtMsg.Refresh();
                                            //Application.DoEvents();
                                        }
                                        if (chkReExport.Checked == true)
                                        {
                                            toolStatus.Text = "Box: " + lvwExportList.Items[i].SubItems[0].Text.ToString() + " policy - " + policyNumber;
                                            //txtMsg.Text = txtMsg.Text + " Policy : " + policyNumber;
                                            //txtMsg.SelectionStart = txtMsg.Text.Length;
                                            //txtMsg.ScrollToCaret();
                                            //txtMsg.Refresh();
                                            //  Application.DoEvents();
                                        }
                                        if (string.IsNullOrEmpty(scanneddate) && (status != (int)eSTATES.POLICY_MISSING))
                                        {
                                            DataSet dt = policy.GetMaxScannedDate();
                                            scanneddate = dt.Tables[0].Rows[0]["scanned_date"].ToString();
                                            dt.Dispose();
                                        }
                                        serial_no = rDtR[0]["serial_number"].ToString().PadLeft(5, Convert.ToChar("0"));
                                        normalSerial = Convert.ToInt32(serial_no);
                                        if ((status != (int)eSTATES.POLICY_MISSING) && ((status != (int)eSTATES.POLICY_ON_HOLD)) && (status != (int)eSTATES.POLICY_SCANNED) && (status != (int)eSTATES.POLICY_QC) && (status != (int)eSTATES.POLICY_INITIALIZED) && (status != (int)eSTATES.POLICY_EXCEPTION) && (status != (int)eSTATES.POLICY_NOT_INDEXED) && ((status != (int)eSTATES.POLICY_EXPORTED) || chkReExport.Checked == true))
                                        {

                                            /////For update policy status
                                            //CtrlPolicy exppPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()), Convert.ToInt32(policyNumber));
                                            //wfePolicy expwPolicy = new wfePolicy(sqlCon, exppPolicy);
                                            //expwPolicy.UpdateStatus(eSTATES.POLICY_EXPORTED, crd);

                                            /////Update image status
                                            //CtrlImage exppImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxno), Convert.ToInt32(policyNumber), string.Empty, string.Empty);
                                            //wfeImage expwImage = new wfeImage(expSqlCon, exppImage);
                                            //expwImage.UpdateAllImageStatus(eSTATES.PAGE_EXPORTED, crd, exportTrans);

                                            if (status == (int)eSTATES.POLICY_INDEXED)
                                            {
                                                imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                            }
                                            else if (status == (int)eSTATES.POLICY_FQC)
                                            {
                                                imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                                            }
                                            else
                                            {
                                                imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                                            }
                                            if (Directory.Exists(policyPath + "\\" + ihConstants._FQC_FOLDER))
                                            {
                                                imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                                            }
                                            else
                                            {
                                                imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                            }
                                            exportPath = appendPath + "\\" + ihConstants._EXPORT_FOLDER +  "\\" + cmbBatch.Text + "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber.PadLeft(9,'0');
                                            //appendPath = exportPath; //".\\" + ihConstants._LOCAL_APPEND_IMAGE_FOLDER;
                                            if (Directory.Exists(exportPath) == false)
                                            {
                                                Directory.CreateDirectory(exportPath);
                                            }
                                            if (Directory.Exists(tempPath) == false)
                                            {
                                                Directory.CreateDirectory(tempPath);
                                            }
                                            if (imagePath != string.Empty)
                                            {
                                                DeleteLocalFile(imagePath, tempPath);
                                            }
                                            ////Check whether rescanned images have not been indexed
                                            //if (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == true)
                                            //{
                                            //    MessageBox.Show("Items rescanned for " + policyNumber + " have not been indexed, aborting...");
                                            //    ///Rollback transaction
                                            //    exportTrans.Rollback();
                                            //    expBol = false;
                                            //    break;
                                            //}
                                            //state = new eSTATES[5];
                                            //state[0] = eSTATES.PAGE_FQC;
                                            //state[1] = eSTATES.PAGE_INDEXED;
                                            //state[2] = eSTATES.PAGE_CHECKED;
                                            //state[3] = eSTATES.PAGE_EXCEPTION;
                                            //state[4] = eSTATES.PAGE_EXPORTED;
                                            //iDtr = new DataRow[iDs.Tables[0].Rows.Count];
                                            if (iDs.Tables.Count > 0)
                                            {
                                                if (iDs.Tables[0].Rows.Count > 0)
                                                {
                                                    //imageDs = iDs.Tables[0];
                                                    iDtr = iDs.Tables[0].Select("proj_key = " + Convert.ToInt32(cmbProject.SelectedValue.ToString()) + " and batch_key=" + Convert.ToInt32(cmbBatch.SelectedValue.ToString()) + " and box_number=" + Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()) + " and policy_number=" + policyNumber);
                                                }
                                            }

                                            ImageCount = iDtr.Length.ToString().PadLeft(3, Convert.ToChar("0"));

                                            int pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.PROPOSALFORM_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            int pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            policyNumber = policyNumber.PadLeft(9, '0');
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.PROPOSALFORM_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.PROPOSALFORM_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.PROPOSALFORM_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.PROPOSALFORM_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.PHOTOADDENDUM_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.PHOTOADDENDUM_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.PHOTOADDENDUM_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.PHOTOADDENDUM_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();

                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.PHOTOADDENDUM_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype

                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.PROPOSALREVIEWSLIP_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.PROPOSALREVIEWSLIP_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.PROPOSALREVIEWSLIP_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.PROPOSALREVIEWSLIP_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                        tempPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.PROPOSALREVIEWSLIP_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.POLICYLOANS_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.POLICYLOANS_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.POLICYLOANS_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.POLICYLOANS_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                        tempPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.POLICYLOANS_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.MEDICALREPORT_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.MEDICALREPORT_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.MEDICALREPORT_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.MEDICALREPORT_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.MEDICALREPORT_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.NOMINATION_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.NOMINATION_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.NOMINATION_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.NOMINATION_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.NOMINATION_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.ASSIGNMENT_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.ASSIGNMENT_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.ASSIGNMENT_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.ASSIGNMENT_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.ASSIGNMENT_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.ALTERATION_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.ALTERATION_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.ALTERATION_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.ALTERATION_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.ALTERATION_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.CLAIMS_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.CLAIMS_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.CLAIMS_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.CLAIMS_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.CLAIMS_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.CORRESPONDENCE_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.CORRESPONDENCE_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.CORRESPONDENCE_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.CORRESPONDENCE_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.CORRESPONDENCE_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.PROPOSALENCLOSERS_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.PROPOSALENCLOSERS_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.PROPOSALENCLOSERS_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.PROPOSALENCLOSERS_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.PROPOSALENCLOSERS_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.POLICYBOND_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.POLICYBOND_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.POLICYBOND_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.POLICYBOND_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.POLICYBOND_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.SURRENDER_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.SURRENDER_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.SURRENDER_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.SURRENDER_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.SURRENDER_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.REVIVALS_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.REVIVALS_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.REVIVALS_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.REVIVALS_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.REVIVALS_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.SIGNATUREPAGE_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.SIGNATUREPAGE_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.SIGNATUREPAGE_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.SIGNATUREPAGE_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.SIGNATUREPAGE_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.OTHERS_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            pgCurrent = 0; 	//Temporary variable where count is increased when
                                            //the doc type is found, so that the name can be stored
                                            //in the right index

                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.OTHERS_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.OTHERS_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.OTHERS_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                    
                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.OTHERS_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            pgCount = 0;
                                            for (tmp = 0; tmp < iDtr.Length; tmp++)
                                            {
                                                if (iDtr[tmp]["doc_type"].ToString() == ihConstants.KYCDOCUMENT_FILE)
                                                    pgCount++;
                                            }
                                            //End: Calculates count for doc types
                                            imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                            //the doc type is found, so that the name can be stored
                                            //in the right index
                                            pgCurrent = 0;
                                            //Rotate through all the images having doctype = Proposal Form
                                            for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                            {
                                                if (iDtr[propCount]["doc_type"].ToString() == ihConstants.KYCDOCUMENT_FILE)
                                                {
                                                    DateTime dt = DateTime.Now;
                                                    if (pgCurrent == 0)
                                                    {
                                                        docType.Add(ihConstants.KYCDOCUMENT_FILE);
                                                        multiPageFileName.Add(policyNumber + "_" + ihConstants.KYCDOCUMENT_FILE + ".TIF");
                                                        docCount++;
                                                    }
                                                    //Changed on 19/09/2009 for managing file copying problem in FQC
                                                    if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                    {
                                                        imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    }
                                                    imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();

                                                    expImageCount = expImageCount + 1;
                                                    policyTotPage = policyTotPage + 1;
                                                    pgCountWhlErr = pgCountWhlErr + 1;
                                                    proposalExists = true;

                                                    DateTime edDt = DateTime.Now;
                                                    TimeSpan tsp = edDt - dt;
                                                    //Increment the variable if doctype is found
                                                    pgCurrent++;
                                                }
                                                //MessageBox.Show(tsp.Milliseconds.ToString());
                                            }
                                            if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.KYCDOCUMENT_FILE + ".TIF") == false)
                                            {
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }

                                            pgCount = 0; //Holds the pagecount for a given doctype
                                            DocTypeCount = docCount.ToString().PadLeft(2, Convert.ToChar("0"));
                                            if (policyTotPage <= ihConstants._MAX_POLICY_PAGE_COUNT)
                                            {
                                                MessageBox.Show("Policy - " + policyNumber + " has less pages, aborting...");
                                                ///Rollback transaction
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            /*
                                            if ((proposalExists != true) || (signatureExists != true))
                                            {
                                                MessageBox.Show("Proposal form or signature page missing for the policy - " + policyNumber + ", aborting...");
                                                ///Rollback transaction
                                                exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                             * */

                                        }
                                        else
                                        {
                                            if ((status == (int)eSTATES.POLICY_MISSING))
                                            {
                                                boxno = "000";
                                                ImageCount = "000";
                                                DocTypeCount = "00";
                                                Scanuploadflag = "02";
                                                //exportPath = batchPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber;
                                                //if (Directory.Exists(exportPath) == false)
                                                //{
                                                //    Directory.CreateDirectory(exportPath);
                                                //}
                                            }
                                            else
                                            {
                                                if ((status == (int)eSTATES.POLICY_INITIALIZED))
                                                {
                                                    MessageBox.Show("Item " + policyNumber + " have not been scanned, aborting...");
                                                    ///Rollback transaction
                                                    //exportTrans.Rollback();
                                                    expBol = false;
                                                    break;
                                                }
                                                if ((status == (int)eSTATES.POLICY_NOT_INDEXED))
                                                {
                                                    MessageBox.Show("Item " + policyNumber + " have not been indexed, aborting...");
                                                    ///Rollback transaction
                                                    //exportTrans.Rollback();
                                                    expBol = false;
                                                    break;
                                                }
                                                if ((status == (int)eSTATES.POLICY_SCANNED))
                                                {
                                                    MessageBox.Show("QC not done for policy - " + policyNumber + " , aborting...");
                                                    ///Rollback transaction
                                                    //exportTrans.Rollback();
                                                    expBol = false;
                                                    break;
                                                }
                                                if ((status == (int)eSTATES.POLICY_EXCEPTION))
                                                {
                                                    MessageBox.Show("LIC exception not cleared for the policy - " + policyNumber + " , aborting...");
                                                    ///Rollback transaction
                                                    //exportTrans.Rollback();
                                                    expBol = false;
                                                    break;
                                                }
                                                if ((status == (int)eSTATES.POLICY_QC))
                                                {
                                                    MessageBox.Show("Indexing not done for policy - " + policyNumber + " , aborting...");
                                                    ///Rollback transaction
                                                    //exportTrans.Rollback();
                                                    expBol = false;
                                                    break;
                                                }
                                                if ((status == (int)eSTATES.POLICY_ON_HOLD))
                                                {
                                                    ImageCount = "000";
                                                    DocTypeCount = "00";
                                                    Scanuploadflag = "02";
                                                    exportPath = batchPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber;
                                                }
                                            }
                                        }
                                        for (int p = 0; p < docType.Count; p++)
                                        {
                                            documentType = documentType + "," + docType[p].ToString() + "," + multiPageFileName[p].ToString(); ;
                                        }
                                        totPolicyCount = totPolicyCount + 1;
                                        if ((status != (int)eSTATES.POLICY_EXPORTED) || (chkReExport.Checked == true))
                                        {
                                            if ((status != (int)eSTATES.POLICY_MISSING) && ((status != (int)eSTATES.POLICY_ON_HOLD)) && (status != (int)eSTATES.POLICY_SCANNED) && (status != (int)eSTATES.POLICY_QC) && (status != (int)eSTATES.POLICY_INITIALIZED) && (status != (int)eSTATES.POLICY_EXCEPTION) && (status != (int)eSTATES.POLICY_NOT_INDEXED))
                                            {
                                                dtr[j]["status"] = eSTATES.POLICY_EXPORTED;
                                                ///For updating all image status in datarow
//                                                for (int st = 0; st < iDtr.Length; st++)
//                                                {
//                                                    iDtr[st]["status"] = eSTATES.PAGE_EXPORTED;
//                                                }
                                            }
                                            if (normalSerial > maxSerial)
                                            {
                                                sw.WriteLine(string.Concat
                                                             (serial_no + ","
                                                              , policyNumber.PadLeft(9,'0') + ","
                                                              , divisionCode + ","
                                                              , branchCode.PadRight(4, Convert.ToChar(" ")) + ","
                                                              , batchSerial + ","
                                                              , boxno + ","
                                                              , Scanuploadflag + ","
                                                              , scanneddate + ","
                                                              , ihConstants.INCREMENTEDSCAN + ","
                                                              , Policyholdername + ","
                                                              , DOB + ","
                                                              , dateofcommencement + ","
                                                              , cust_id.PadRight(16, Convert.ToChar(" ")) + ","
                                                              , ImageCount + ","
                                                              , DocTypeCount + ","
                                                              , rootPath
                                                              , documentType));
                                                if (Directory.Exists(tempPath))
                                                {
                                                    Directory.Delete(tempPath, true);
                                                }
                                            }
                                            //txtMsg.Text = txtMsg.Text + "  Page : " + ImageCount + "\r\n";
                                            //txtMsg.SelectionStart = txtMsg.Text.Length;
                                            //txtMsg.ScrollToCaret();
                                            //txtMsg.Refresh();
                                            totImage = totImage + Convert.ToInt32(ImageCount);
                                            toolStatus.Text = toolStatus.Text + " Page : " + ImageCount + "/" + totImage;
                                            Application.DoEvents();
                                            //if (tempPath != string.Empty)
                                            //    DeleteLocalFile(tempPath, exportPath);
                                        }
                                        //sw.Close();
                                        //sw.Dispose();
                                        //sw.Flush();
                                        imageDs.Dispose();
                                        docType.Clear();
                                        multiPageFileName.Clear();
                                        documentType = string.Empty;
                                        proposalExists = false;
                                        signatureExists = false;
                                        policyTotPage = 0;
                                        //exportTrans.Commit();
                                        DateTime endDt = DateTime.Now;
                                        TimeSpan tp = endDt - stDt;
                                        //MessageBox.Show("Total time-" + tp.Milliseconds);
                                    }
                                    
                                    img.Close();
                                    img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
                                    expLog = new StreamWriter(appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text + "\\" + "Export_Log.txt", true);
                                    expLog.WriteLine("Project Name - " + cmbProject.Text);
                                    expLog.WriteLine("Batch Name - " + cmbBatch.Text);
                                    expLog.WriteLine("Box number - " + lvwExportList.Items[i].SubItems[0].Text.ToString());
                                    expLog.WriteLine("Policy Exported - " + (expPolicyCount));
                                    expLog.WriteLine("Images Exported - " + expImageCount);
                                    if (expBol == true)  
                                    {
                                        ///Update box status
                                        pBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxno));
                                        box = new wfeBox(sqlCon, pBox);
                                        box.UpdateStatus(eSTATES.BOX_EXPORTED, exportTrans);

                                        toolStatus.Text = "Box Number - " + lvwExportList.Items[i].SubItems[0].Text.ToString() + " Exported successfully........";

                                        txtMsg.Text = txtMsg.Text + "Exported successfully \r\n";
                                        txtMsg.Text = txtMsg.Text + "Summary........ \r\n";
                                        txtMsg.Text = txtMsg.Text + "Total Policy : " + expPolicyCount + " Total Images : " + expImageCount + "\r\n";
                                        txtMsg.SelectionStart = txtMsg.Text.Length;
                                        txtMsg.ScrollToCaret();
                                        txtMsg.Refresh();
                                        sw.Close();
                                        if (Directory.Exists(tempPath))
                                        {
                                            Directory.Delete(tempPath, true);
                                        }

                                    }
                                    else
                                    {
                                        sw.Close();
                                        expLog.WriteLine("Error in policy - " + policyNumber);
                                        expLog.Close();

                                        toolStatus.Text = "Error while exporting selected box.........";
                                        //exportTrans.Rollback();
                                        if (Directory.Exists(exportPath))
                                        {
                                            Directory.Delete(exportPath, true);
                                        }
                                        txtMsg.Text = txtMsg.Text + "\r\n" + "Export error in : " + policyNumber + "\r\n";
                                        txtMsg.Text = txtMsg.Text + "Summary........ \r\n";
                                        txtMsg.Text = txtMsg.Text + "Total Policy : " + (expPolicyCount - 1) + " Total Images Exported : " + (expImageCount - pgCountWhlErr) + "\r\n";
                                        txtMsg.SelectionStart = txtMsg.Text.Length;
                                        txtMsg.ScrollToCaret();
                                        txtMsg.Refresh();
                                        //if ((appendPath != string.Empty) && (exportPath != string.Empty))
                                        //{
                                        //    DeleteLocalFile(appendPath, exportPath);
                                        //    break;
                                        //}
                                        if (Directory.Exists(tempPath))
                                        {
                                            Directory.Delete(tempPath, true);
                                        }
                                        break;
                                    }
                                    expLog.Close();
                                    sw.Close();
                                }
                        }
				    }
					    //update policy_master set status = dataset.tables[0].rows[i]["status"] where policy_master.proj_key=x and batch_key=y and box_number=z and policy_number=p;
                        if ((pAdp != null) && (pDs != null))
                        {
                            //                            OdbcCommandBuilder pCommandBuilder = new OdbcCommandBuilder(pAdp);
                            //                            pAdp.UpdateCommand = pCommandBuilder.GetUpdateCommand();
                            //                            pAdp.Update(pDs.Tables[0]);
                            UpdateAllPolicy(pDs);
                        }
//                                    ///Batch update for Image
//                                    if ((iAdp != null) && (iDs != null))
//                                    {
//                                        OdbcCommandBuilder iCommandBuilder = new OdbcCommandBuilder(iAdp);
//                                        iAdp.UpdateCommand = iCommandBuilder.GetUpdateCommand();
//                                        iAdp.Update(iDs.Tables[0]);
//                                    }
					    //expSqlCon.Close();
                        DateTime endDtTot = DateTime.Now;
                        TimeSpan tspTot = endDtTot - stDt;
                        MessageBox.Show("Total time - " + tspTot.Minutes);
                 }
                else
                {
                    MessageBox.Show("This batch is not ready for UAT, export is not possible....");
                }       
			}
			catch(Exception ex)
			{
				error=ex.Message;
			   	MessageBox.Show("Error while exporting data...... " + error,"Export Error",MessageBoxButtons.OK);
                //exportTrans.Rollback();
                if (Directory.Exists(exportPath))
                {
                    Directory.Delete(exportPath, true);
                }
                ///Batch update for policy
//                if ((pAdp != null) && (pDs != null))
//                {
//                    OdbcCommandBuilder pCommandBuilder = new OdbcCommandBuilder(pAdp);
//                    pAdp.UpdateCommand = pCommandBuilder.GetUpdateCommand();
//                    pAdp.Update(pDs.Tables[0]);
//                }
				UpdateAllPolicy(pDs);
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
//                ///Batch update for Image
//                if ((iAdp != null) && (iDs != null))
//                {
//                    OdbcCommandBuilder iCommandBuilder = new OdbcCommandBuilder(iAdp);
//                    iAdp.UpdateCommand = iCommandBuilder.GetUpdateCommand();
//                    iAdp.Update(iDs.Tables[0]);
//                }
                expSqlCon.Close();
                sw.Close();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + cmbProject.Text + " ,Batch-" + cmbBatch.Text + " ,Box number-" + boxno + " ,Policy number-" + policyNumber + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
                //DeleteLocalFile(appendPath, exportPath);
			}
			finally
			{
                if (sw != null)
                {
                    sw.Close();
                }
                PopulateBox();
                //if ((fileName != string.Empty) && (batchPath != string.Empty))
                //{
                //    File.Copy(".\\" + ihConstants._LOCAL_APPEND_TEXT_FOLDER + "\\" + fileName, batchPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + fileName);
                //    File.Delete(".\\" + ihConstants._LOCAL_APPEND_TEXT_FOLDER + "\\" + fileName);
                //}
			}
		}
		private void UpdateAllPolicy(DataSet pDs)
		{
			CtrlPolicy exppPolicy; 
			wfePolicy expwPolicy ;
			int policyNo;
			int boxNo;
			for(int i=0;i<pDs.Tables[0].Rows.Count;i++)
			{
				policyNo =Convert.ToInt32(pDs.Tables[0].Rows[i]["policy_number"].ToString());
				boxNo = Convert.ToInt32(pDs.Tables[0].Rows[i]["box_number"].ToString());
				exppPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), boxNo,policyNo);
                expwPolicy = new wfePolicy(sqlCon, exppPolicy);
                expwPolicy.UpdateStatus(Convert.ToInt32(pDs.Tables[0].Rows[i]["status"].ToString()), crd);
			}
		}
        /*
        void BatchUpdate(OdbcDataAdapter prmPAdap, OdbcDataAdapter prmIAdap)
        {
            ///Batch update for policy
            OdbcCommandBuilder pCommandBuilder = new OdbcCommandBuilder(prmPAdap);
            prmPAdap.UpdateCommand = pCommandBuilder.GetUpdateCommand();
            prmPAdap.Update(pDs.Tables[0]);

            ///Batch update for Image
            OdbcCommandBuilder iCommandBuilder = new OdbcCommandBuilder(iAdp);
            iAdp.UpdateCommand = iCommandBuilder.GetUpdateCommand();
            iAdp.Update(iDs.Tables[0]);
        }
         */ 
        void DeleteLocalFile(string pSourcePath,string pDestPath)
        {
            string[] replFiles = Directory.GetFiles(pSourcePath);
            try
            {
                if (replFiles.Length > 0)
                {
                    for (int i = 0; i < replFiles.Length; i++)
                    {
                        File.Copy(replFiles[i], pDestPath + "\\" + Path.GetFileName(replFiles[i]), false);
                        //File.Delete(replFiles[i]);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
		
		void AeExportFormClosing(object sender, FormClosingEventArgs e)
		{
			//sqlCon.Close();
		}

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
		
		void CmbBatchSelectedIndexChanged(object sender, EventArgs e)
		{
			
		}

        private void cmdSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog svFile = new SaveFileDialog();
            Stream myStream;
            svFile.Filter = "Text files (*.txt)|*.txt";
            svFile.FileName = cmbBatch.Text + "_Export_Result.txt";
            svFile.FilterIndex = 2;
            svFile.RestoreDirectory = true;

            if (svFile.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = svFile.OpenFile()) != null)
                {
                    StreamWriter wText = new StreamWriter(myStream);
                    wText.Write(txtMsg.Text);
                    wText.Flush();
                    wText.Close();
                    myStream.Close();
                }
            }
        }

        private void cmdValidate_Click(object sender, EventArgs e)
        {
            
            string boxno = string.Empty;
            ArrayList arrPolicy = new ArrayList();
            wQuery pQuery = new ihwQuery(sqlCon);
            string policyNumber = string.Empty;
            DataTable policyDtls = new DataTable();
            DataTable imageDs = new DataTable();
            string imagePath;
            
            string ImageCount = string.Empty;
            string DocTypeCount = string.Empty;
            string[] imageName;
            string batchPath = string.Empty;
            string exportPath = string.Empty;
            int status;
            string policyPath;
            //string imagePath;
            string fileName = string.Empty;
            string documentType = null;
            //string FileName=null;
            string rootPath;
            exportFile = new FileorFolder();
            ArrayList docType = new ArrayList();
            ArrayList multiPageFileName = new ArrayList();
            OdbcTransaction exportTrans = null;
            bool expBol = true;
            OdbcConnection expSqlCon = null;
            int totPolicyCount = 0;
            string serial_no;
            int policyTotPage = 0;
            bool proposalExists;
            bool signatureExists;
            int pgCountWhlErr = 0;
            string appendPath = string.Empty;
            int tmp = 0;
            OdbcDataAdapter pAdp = new OdbcDataAdapter();
            OdbcDataAdapter iAdp = null;
            DataSet pDs = new DataSet();
            DataSet rDs = new DataSet();
            DataSet iDs = null;
            int maxSerial = 0;
            int normalSerial = 0;
            string[] lastLine = null;
            string tempPath = string.Empty;
            int validity = 0;
            //System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.LowLatency;
            wBatch = new wfeBatch(sqlCon);
                batchPath = wBatch.GetPath(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()));

                if ((wBatch.GetBatchStatus(Convert.ToInt32(cmbBatch.SelectedValue.ToString())) == (int)eSTATES.BATCH_READY_FOR_UAT) || (wBatch.GetBatchStatus(Convert.ToInt32(cmbBatch.SelectedValue.ToString())) != (int)eSTATES.BATCH_READY_FOR_UAT))
                {
                    DateTime stDt = DateTime.Now;
                    txtMsg.Text = "Batch Number : \r\n";
                    txtMsg.Text = txtMsg.Text + cmbBatch.Text + "\r\n";
                    txtMsg.SelectionStart = txtMsg.Text.Length;
                    txtMsg.ScrollToCaret();
                    txtMsg.Refresh();

                    tblExp.SelectedIndex = 1;
                    DateTime stDtTot = DateTime.Now;
                    NovaNet.Utils.dbCon dbcon;
                    dbcon = new NovaNet.Utils.dbCon();
                    //expSqlCon = dbcon.Connect();

                    ctrlBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), 0);
                    box = new wfeBox(sqlCon, ctrlBox);
                    policy = new wfePolicy(sqlCon);

                    pDs = policy.GetAllPolicyDetails(box, out pAdp);
                    maxSerial = 0;

                    for (int i = 0; i < lvwExportList.Items.Count; i++) //Counter for Box
                    {
                        if (lvwExportList.Items[i].Checked == true)
                        {
                            if ((lvwExportList.Items[i].SubItems[1].Text.ToString() == "0") || (lvwExportList.Items[i].SubItems[1].Text.ToString() != "0"))
                            {
                                DataRow[] dtr = null;
                                DataRow[] pDtR = new DataRow[1];
                                state = new eSTATES[0];
                                if (pDs.Tables.Count > 0)
                                {

                                    if (pDs.Tables[0].Rows.Count > 0)
                                    {

                                        dtr = pDs.Tables[0].Select("proj_key = " + Convert.ToInt32(cmbProject.SelectedValue.ToString()) + " and batch_key=" + Convert.ToInt32(cmbBatch.SelectedValue.ToString()) + " and box_number=" + Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()));
                                        //rDtR = rDs.Tables[0].Select("policy_no=" + policyNumber);
                                    }
                                }
                                boxno = lvwExportList.Items[i].SubItems[0].Text.ToString().PadLeft(3, Convert.ToChar("0"));
                                //ctrlBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()));
                                //box = new wfeBox(sqlCon, ctrlBox);
                                //arrPolicy = pQuery.GetItems(eITEMS.POLICY, state, box);
                                expImageCount = 0;
                                expPolicyCount = 0;
                                pgCountWhlErr = 0;
                                expBol = true;

                                txtMsg.Text = txtMsg.Text + "Wait while validating box : ";
                                txtMsg.Text = txtMsg.Text + boxno + "\r\n";
                                txtMsg.SelectionStart = txtMsg.Text.Length;
                                txtMsg.ScrollToCaret();
                                txtMsg.Refresh();

                                iAdp = new OdbcDataAdapter();
                                ///For all policy details

                                //drelPolicyRaw = SetRelationPolicyRawData(pDs, rDs);
                                pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxno), Convert.ToInt32("0"), string.Empty, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                                iDs = new DataSet();
                                iDs = wImage.GetAllImages(out iAdp);

                                for (int j = 0; j < dtr.Length; j++) //Loop within box for all the policies
                                {
                                    int docCount = 0;
                                    validity = 0;
                                    DataRow[] iDtr = null;
                                    //expBol = true;
                                    pgCountWhlErr = 0;
                                    //exportTrans = expSqlCon.BeginTransaction();
                                    boxno = lvwExportList.Items[i].SubItems[0].Text.ToString().PadLeft(3, Convert.ToChar("0"));
                                    expPolicyCount = expPolicyCount + 1;
                                    //imageDs = new DataSet();
                                    //ctrPol = (CtrlPolicy)arrPolicy[j];
                                    policyNumber = dtr[j]["policy_number"].ToString(); //ctrPol.PolicyNumber.ToString();
                                    if (pDs.Tables.Count > 0)
                                    {
                                        if (pDs.Tables[0].Rows.Count > 0)
                                        {
                                            policyDtls = pDs.Tables[0];
                                            pDtR = policyDtls.Select("proj_key = " + Convert.ToInt32(cmbProject.SelectedValue.ToString()) + " and batch_key=" + Convert.ToInt32(cmbBatch.SelectedValue.ToString()) + " and box_number=" + Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()) + " and policy_number=" + policyNumber);
                                        }
                                    }

                                    
                                    policy = new wfePolicy(sqlCon, ctrPol);

                                    policyPath = batchPath + "\\" + lvwExportList.Items[i].SubItems[0].Text.ToString() + "\\" + policyNumber;

                                    status = Convert.ToInt32(pDtR[0]["status"].ToString());
                                    if ((status != (int)eSTATES.POLICY_EXPORTED) && (chkReExport.Checked != true))
                                    {
                                        toolStatus.Text = "Box: " + lvwExportList.Items[i].SubItems[0].Text.ToString() + " policy - " + policyNumber;
                                        txtMsg.Text = txtMsg.Text + " Policy: " + policyNumber;
                                        txtMsg.SelectionStart = txtMsg.Text.Length;
                                        txtMsg.ScrollToCaret();
                                        txtMsg.Refresh();
                                        Application.DoEvents();
                                    }
                                    if (chkReExport.Checked == true)
                                    {
                                        toolStatus.Text = "Box: " + lvwExportList.Items[i].SubItems[0].Text.ToString() + " policy - " + policyNumber;
                                        txtMsg.Text = txtMsg.Text + " Policy: " + policyNumber; 
                                        txtMsg.SelectionStart = txtMsg.Text.Length;
                                        txtMsg.ScrollToCaret();
                                        txtMsg.Refresh();
                                        Application.DoEvents();
                                    }
                                    if ((status != (int)eSTATES.POLICY_MISSING) && ((status != (int)eSTATES.POLICY_ON_HOLD)) && (status != (int)eSTATES.POLICY_SCANNED) && (status != (int)eSTATES.POLICY_QC) && (status != (int)eSTATES.POLICY_INITIALIZED) && (status != (int)eSTATES.POLICY_EXCEPTION) && (status != (int)eSTATES.POLICY_NOT_INDEXED) && ((status != (int)eSTATES.POLICY_EXPORTED) || chkReExport.Checked == true))
                                    {

                                        /////For update policy status
                                        //CtrlPolicy exppPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()), Convert.ToInt32(policyNumber));
                                        //wfePolicy expwPolicy = new wfePolicy(sqlCon, exppPolicy);
                                        //expwPolicy.UpdateStatus(eSTATES.POLICY_EXPORTED, crd);

                                        /////Update image status
                                        //CtrlImage exppImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxno), Convert.ToInt32(policyNumber), string.Empty, string.Empty);
                                        //wfeImage expwImage = new wfeImage(expSqlCon, exppImage);
                                        //expwImage.UpdateAllImageStatus(eSTATES.PAGE_EXPORTED, crd, exportTrans);

                                        if (status == (int)eSTATES.POLICY_INDEXED)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        else if (status == (int)eSTATES.POLICY_FQC)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                                        }
                                        else
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                                        }
                                        if (Directory.Exists(policyPath + "\\" + ihConstants._FQC_FOLDER))
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                                        }
                                        else
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        //exportPath = appendPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + cmbBatch.Text + "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber;
                                        if (iDs.Tables.Count > 0)
                                        {
                                            if (iDs.Tables[0].Rows.Count > 0)
                                            {
                                                //imageDs = iDs.Tables[0];
                                                iDtr = iDs.Tables[0].Select("proj_key = " + Convert.ToInt32(cmbProject.SelectedValue.ToString()) + " and batch_key=" + Convert.ToInt32(cmbBatch.SelectedValue.ToString()) + " and box_number=" + Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()) + " and policy_number=" + policyNumber);
                                            }
                                        }

                                        ImageCount = iDtr.Length.ToString().PadLeft(3, Convert.ToChar("0"));

                                        int pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.PROPOSALFORM_FILE)
                                                pgCount++;
                                        }

                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        int pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form

                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.PROPOSALFORM_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.PROPOSALFORM_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.PROPOSALFORM_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.PHOTOADDENDUM_FILE)
                                                pgCount++;
                                        }

                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.PHOTOADDENDUM_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.PHOTOADDENDUM_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.PHOTOADDENDUM_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    //tempPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.KYCDOCUMENT_FILE)
                                                pgCount++;
                                        }

                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.KYCDOCUMENT_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.KYCDOCUMENT_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.KYCDOCUMENT_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    //tempPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.PROPOSALREVIEWSLIP_FILE)
                                                pgCount++;
                                        }

                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.PROPOSALREVIEWSLIP_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.PROPOSALREVIEWSLIP_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.PROPOSALREVIEWSLIP_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                    //tempPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.POLICYLOANS_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.POLICYLOANS_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.POLICYLOANS_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.POLICYLOANS_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.MEDICALREPORT_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.MEDICALREPORT_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.MEDICALREPORT_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.MEDICALREPORT_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //                                                imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.NOMINATION_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.NOMINATION_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.NOMINATION_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.NOMINATION_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }

                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.ASSIGNMENT_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.ASSIGNMENT_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.ASSIGNMENT_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.ASSIGNMENT_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.ALTERATION_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.ALTERATION_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.ALTERATION_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.ALTERATION_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.CLAIMS_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.CLAIMS_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.CLAIMS_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.CLAIMS_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.CORRESPONDENCE_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.CORRESPONDENCE_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.CORRESPONDENCE_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.CORRESPONDENCE_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.PROPOSALENCLOSERS_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.PROPOSALENCLOSERS_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.PROPOSALENCLOSERS_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.PROPOSALENCLOSERS_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.POLICYBOND_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.POLICYBOND_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.POLICYBOND_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.POLICYBOND_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.SURRENDER_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.SURRENDER_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.SURRENDER_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.SURRENDER_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.REVIVALS_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.REVIVALS_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.REVIVALS_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.REVIVALS_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.SIGNATUREPAGE_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.SIGNATUREPAGE_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.SIGNATUREPAGE_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.SIGNATUREPAGE_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        pgCount = 0; //Holds the pagecount for a given doctype
                                        for (tmp = 0; tmp < iDtr.Length; tmp++)
                                        {
                                            if (iDtr[tmp]["doc_type"].ToString() == ihConstants.OTHERS_FILE)
                                                pgCount++;
                                        }
                                        //End: Calculates count for doc types
                                        imageName = new string[pgCount]; //Initialize the array with no. of images for the doc type

                                        pgCurrent = 0; 	//Temporary variable where count is increased when
                                        //the doc type is found, so that the name can be stored
                                        //in the right index

                                        //Rotate through all the images having doctype = Proposal Form
                                        for (int propCount = 0; propCount < iDtr.Length; propCount++)
                                        {
                                            if (iDtr[propCount]["doc_type"].ToString() == ihConstants.OTHERS_FILE)
                                            {
                                                DateTime dt = DateTime.Now;
                                                if (pgCurrent == 0)
                                                {
                                                    docType.Add(ihConstants.OTHERS_FILE);
                                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.OTHERS_FILE + ".TIF");
                                                    docCount++;
                                                }
                                                //Changed on 19/09/2009 for managing file copying problem in FQC
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                                }
                                                //        imageName[pgCurrent] = tempPath + "\\" + iDtr[propCount]["page_index_name"].ToString();
                                                if (File.Exists(imagePath + "\\" + iDtr[propCount]["page_index_name"].ToString()) == false)
                                                {
                                                    txtMsg.Text = txtMsg.Text + " Image invalid: " + iDtr[propCount]["page_index_name"].ToString();
                                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                                    txtMsg.ScrollToCaret();
                                                    txtMsg.Refresh();
                                                    validity = validity + 1;
                                                }
                                                expImageCount = expImageCount + 1;
                                                policyTotPage = policyTotPage + 1;
                                                pgCountWhlErr = pgCountWhlErr + 1;
                                                proposalExists = true;

                                                DateTime edDt = DateTime.Now;
                                                TimeSpan tsp = edDt - dt;
                                                //Increment the variable if doctype is found
                                                pgCurrent++;
                                            }
                                            //MessageBox.Show(tsp.Milliseconds.ToString());
                                        }

                                        DocTypeCount = docCount.ToString().PadLeft(2, Convert.ToChar("0"));
                                        if (policyTotPage <= ihConstants._MAX_POLICY_PAGE_COUNT)
                                        {
                                            MessageBox.Show("Policy - " + policyNumber + " has less pages, aborting...");
                                            ///Rollback transaction
                                            //exportTrans.Rollback();
                                            expBol = false;
                                            break;
                                        }
                                        /*
                                        if ((proposalExists != true) || (signatureExists != true))
                                        {
                                            MessageBox.Show("Proposal form or signature page missing for the policy - " + policyNumber + ", aborting...");
                                            ///Rollback transaction
                                            exportTrans.Rollback();
                                            expBol = false;
                                            break;
                                        }
                                         * */

                                    }
                                    else
                                    {
                                        if ((status == (int)eSTATES.POLICY_MISSING))
                                        {
                                            boxno = "000";
                                            ImageCount = "000";
                                            DocTypeCount = "00";
                                            //Scanuploadlag = "02";
                                            //exportPath = batchPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber;
                                            //if (Directory.Exists(exportPath) == false)
                                            //{
                                            //    Directory.CreateDirectory(exportPath);
                                            //}
                                        }
                                        else
                                        {
                                            if ((status == (int)eSTATES.POLICY_INITIALIZED))
                                            {
                                                MessageBox.Show("Item " + policyNumber + " have not been scanned, aborting...");
                                                ///Rollback transaction
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            if ((status == (int)eSTATES.POLICY_NOT_INDEXED))
                                            {
                                                MessageBox.Show("Item " + policyNumber + " have not been indexed, aborting...");
                                                ///Rollback transaction
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            if ((status == (int)eSTATES.POLICY_SCANNED))
                                            {
                                                MessageBox.Show("QC not done for policy - " + policyNumber + " , aborting...");
                                                ///Rollback transaction
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            if ((status == (int)eSTATES.POLICY_EXCEPTION))
                                            {
                                                MessageBox.Show("LIC exception not cleared for the policy - " + policyNumber + " , aborting...");
                                                ///Rollback transaction
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            if ((status == (int)eSTATES.POLICY_QC))
                                            {
                                                MessageBox.Show("Indexing not done for policy - " + policyNumber + " , aborting...");
                                                ///Rollback transaction
                                                //exportTrans.Rollback();
                                                expBol = false;
                                                break;
                                            }
                                            if ((status == (int)eSTATES.POLICY_ON_HOLD))
                                            {
                                                ImageCount = "000";
                                                DocTypeCount = "00";
                                              //  Scanuploadflag = "02";
                                                //exportPath = batchPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber;
                                            }
                                        }
                                    }
                                    if (validity == 0)
                                    {
                                        txtMsg.Text = txtMsg.Text + " : Valid";
                                        txtMsg.SelectionStart = txtMsg.Text.Length;
                                        txtMsg.ScrollToCaret();
                                        txtMsg.Refresh();
                                    }
                                    txtMsg.Text = txtMsg.Text + "  Page : " + ImageCount + "\r\n";
                                    txtMsg.SelectionStart = txtMsg.Text.Length;
                                    txtMsg.ScrollToCaret();
                                    txtMsg.Refresh();
                                    toolStatus.Text = toolStatus.Text + " Page : " + ImageCount;
                                    //Application.DoEvents();
                                    //if (tempPath != string.Empty)
                                    //    DeleteLocalFile(tempPath, exportPath);
                                }
                                //sw.Close();
                                //sw.Dispose();
                                //sw.Flush();
                                imageDs.Dispose();
                                docType.Clear();
                                multiPageFileName.Clear();
                                documentType = string.Empty;
                                proposalExists = false;
                                signatureExists = false;
                                policyTotPage = 0;
                                //exportTrans.Commit();
                                
                            }
                        }
                    }
                    DateTime endDt = DateTime.Now;
                    TimeSpan tp = endDt - stDt;
                    MessageBox.Show("Validation finished......");
                }
                
            }
        private bool LoadImage(string pPath)
        {
            
            if(img.LoadBitmapFromFile(pPath) == IGRStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void chkReExport_CheckedChanged(object sender, EventArgs e)
        {
            PopulateBox();
        }
	}
}
