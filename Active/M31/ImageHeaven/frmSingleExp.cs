/*
 * Created by SharpDevelop.
 * User: user
 * Date: 18/08/2009
 * Time: 11:40 PM
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
using System.Collections;

namespace ImageHeaven
{
	/// <summary>
	/// Description of frmSingleExp.
	/// </summary>
	public partial class frmSingleExp : Form,StateData
	{
		OdbcConnection sqlCon = null;
        Credentials crd = new Credentials();
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        eSTATES[] state;
        CtrlPolicy ctrlPolicy = null;
        wfePolicy policy = null;
        MemoryStream stateLog;
        byte[] tmpWrite;
		wfeProject tmpProj=null;
		DataSet ds=null;
		string batchCount;
		CtrlBox ctrlBox = null;
		wfeBox box = null;
		CtrlPolicy ctrPol = null;
		CtrlImage pImage = null;
		wfeImage wImage = null;
		wfeBatch wBatch=null;
		private udtPolicy policyData=null;
		StreamWriter sw;
        StreamWriter expLog;
		FileorFolder exportFile;
		string error = null;
        private long expImageCount = 0;
        private long expPolicyCount = 0;
        private CtrlBox pBox = null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        private Imagery img;
		public frmSingleExp(OdbcConnection prmCon,Credentials prmCrd)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			sqlCon = prmCon;
            //this.Text = "B'Zer - Job Creation";
            crd = prmCrd;
            img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void FrmSingleExpLoad(object sender, EventArgs e)
		{
			PopulateProjectCombo();
		}
		MemoryStream StateData.StateLog()
        {
            return stateLog;
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
                state = new eSTATES[1];
                state[0] = eSTATES.POLICY_EXPORTED;
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
		
		void CmbProjectLeave(object sender, EventArgs e)
		{
			PopulateBatchCombo();
		}
		
		void CmbBatchLeave(object sender, EventArgs e)
		{
			PopulateBoxCombo();
		}
		
		void CmbBoxLeave(object sender, EventArgs e)
		{
			DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            state= new eSTATES[1];
            dbcon = new NovaNet.Utils.dbCon();
            

            if ((cmbProject.SelectedValue != null) && (cmbBatch.SelectedValue != null) && (cmbBox.SelectedValue != null))
            {
                dt.Columns.Add("SrlNo");
                dt.Columns.Add("Policy");
                dt.Columns.Add("Name");
                

                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(cmbBox.Text), 0);
                policy = new wfePolicy(sqlCon, ctrlPolicy);
				
                state[0]=eSTATES.POLICY_EXPORTED;
                ds = policy.GetPolicyList(state);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                	if ((int)eSTATES.POLICY_EXPORTED == Convert.ToInt32(ds.Tables[0].Rows[i]["status"].ToString()))
                    {
                		ListViewItem lvwItem = lvwExportList.Items.Add((i+1).ToString());
			    		lvwItem.SubItems.Add(ds.Tables[0].Rows[i]["policy_no"].ToString());
			    		lvwItem.SubItems.Add(ds.Tables[0].Rows[i]["name_of_policyholder"].ToString());
                    }
                }
            }
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
			DataSet policyDtls = new DataSet();
			string imagePath;
			string vendorCode;
			string versionNumber;
			string ImageCount = string.Empty;
			string DocTypeCount=string.Empty;
			//string totalBatchCount;
			DataSet imageDs;
			string Scanuploadflag;
			string scanneddate;
			//string IncrementedScan;
			string Policyholdername;
			string DOB;
			string dateofcommencement;
			string cust_id;
			string[] imageName;
			string batchPath;
			string exportPath;
			int status;
			string policyPath;
			//string imagePath;
			string fileName;
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
				try
				{
					
                    //state[0] = eSTATES.POLICY_FQC;
                    //state[1] = eSTATES.POLICY_INDEXED;
                    //state[2] = eSTATES.POLICY_CHECKED;

                    

					tmpProj=new wfeProject(sqlCon);
                    ds = new DataSet();
					ds = tmpProj.GetConfiguration();
					
					vendorCode = ds.Tables[0].Rows[0]["VENDOR_CODE"].ToString();
					versionNumber = ds.Tables[0].Rows[0]["VERSION_NUMBER"].ToString();
                    ds.Dispose();

                    ds = new DataSet();
                    ds = tmpProj.GetMainConfiguration(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
                    divisionCode = ds.Tables[0].Rows[0]["DO_CODE"].ToString();
                    branchCode = ds.Tables[0].Rows[0]["BO_CODE"].ToString();
					wBatch = new wfeBatch(sqlCon);
					batchPath = wBatch.GetPath(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()));
					
					if(Directory.Exists(batchPath + "\\" + ihConstants._RE_EXPORT_FOLDER) == false)
					{
						Directory.CreateDirectory(batchPath + "\\" + ihConstants._RE_EXPORT_FOLDER);
					}
					for (int i=0;i<lvwExportList.Items.Count;i++)
					{
                        
						if(lvwExportList.Items[i].Checked == true)
						{
                            state = new eSTATES[0];
                            //Start transaction, only open for export purpose
                            NovaNet.Utils.dbCon dbcon;
                            dbcon = new NovaNet.Utils.dbCon();
                            expSqlCon = dbcon.Connect();
                            exportTrans = expSqlCon.BeginTransaction();

                            boxno = cmbBox.Text.PadLeft(3, Convert.ToChar("0"));
                            expImageCount = 0;
                            expPolicyCount = 0;
			            	expPolicyCount = expPolicyCount + 1;
			            	imageDs = new DataSet();
			            	policyNumber = lvwExportList.Items[i].SubItems[1].Text;
			            	ctrPol = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()),Convert.ToInt32(cmbBatch.SelectedValue.ToString()),Convert.ToInt32(cmbBox.Text),Convert.ToInt32(policyNumber));
						    policy = new wfePolicy(sqlCon, ctrPol);
						    batchCount = policy.GetBatchSerial();
						    batchSerial = divisionCode + "-" + branchCode.PadRight(4,Convert.ToChar(" ")) + "-" + batchCount;
						    //policyData=(udtPolicy)policy.LoadValuesFromDB();
						    policyPath=batchPath + "\\" + cmbBox.Text.ToString() + "\\" + policyNumber;
						    toolStatus.Text = "Wait while exporting policy - " + policyNumber;
						    imagePath ="\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4,Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber.PadLeft(9,'0');
						    rootPath = imagePath;
                            fileName = vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + ".TXT";
						    sw = new StreamWriter(batchPath + "\\" + ihConstants._RE_EXPORT_FOLDER + "\\" + fileName, true);
                            
						    policyDtls	= policy.GetPolicyDetails();
							Policyholdername = policyDtls.Tables[0].Rows[0]["name_of_policyholder"].ToString().PadRight(30,Convert.ToChar(" "));
						    DOB = policyDtls.Tables[0].Rows[0]["date_of_birth"].ToString();
						    dateofcommencement = policyDtls.Tables[0].Rows[0]["date_of_commencement"].ToString();
						    scanneddate = policyDtls.Tables[0].Rows[0]["scanned_date"].ToString();
						    Scanuploadflag = policyDtls.Tables[0].Rows[0]["Scan_upload_flag"].ToString();
						    cust_id = policyDtls.Tables[0].Rows[0]["customer_id"].ToString();
						    status =Convert.ToInt32(policyDtls.Tables[0].Rows[0]["status"].ToString());
                            serial_no = policyDtls.Tables[0].Rows[0]["serial_number"].ToString().PadLeft(5, Convert.ToChar("0"));
                            if ((status != (int)eSTATES.POLICY_MISSING) && ((status != (int)eSTATES.POLICY_ON_HOLD))&&(status != (int)eSTATES.POLICY_SCANNED)&&(status != (int)eSTATES.POLICY_QC)&&(status != (int)eSTATES.POLICY_INITIALIZED))
                            {
                                ///For update policy status
                                CtrlPolicy exppPolicy = new CtrlPolicy(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(lvwExportList.Items[i].SubItems[0].Text.ToString()), Convert.ToInt32(policyNumber));
                                wfePolicy expwPolicy = new wfePolicy(expSqlCon, exppPolicy);
                                expwPolicy.UpdateStatus(eSTATES.POLICY_EXPORTED, crd, exportTrans);

                                ///Update image status
                                CtrlImage exppImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxno), Convert.ToInt32(policyNumber), string.Empty, string.Empty);
                                wfeImage expwImage = new wfeImage(expSqlCon, exppImage);
                                expwImage.UpdateAllImageStatus(eSTATES.PAGE_EXPORTED, crd, exportTrans);

                                
                                if (Directory.Exists(policyPath + "\\" + ihConstants._FQC_FOLDER))
                                {
                                    imagePath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                                }
                                else
                                {
                                    imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                }

                                exportPath = batchPath + "\\" + ihConstants._RE_EXPORT_FOLDER + "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber.PadLeft(9,'0');
                                if (Directory.Exists(exportPath) == false)
                                {
                                    Directory.CreateDirectory(exportPath);
                                }

                                pImage = new CtrlImage(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxno), Convert.ToInt32(policyNumber), string.Empty, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                               
                                //Check whether rescanned images have not been indexed
                                if (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == true)
                                {
                                	MessageBox.Show("Items rescanned for " + policyNumber + " have not been indexed, aborting...");
                                    ///Rollback transaction
                                    exportTrans.Rollback();
                                    expBol = false;
                                	break;
                                }
                                state = new eSTATES[6];
                                state[0] = eSTATES.PAGE_INDEXED;
                                state[1] = eSTATES.PAGE_FQC;
                                state[2] = eSTATES.PAGE_CHECKED;
                                state[3] = eSTATES.PAGE_EXCEPTION;
                                state[4] = eSTATES.PAGE_EXPORTED;
                                state[5] = eSTATES.PAGE_ON_HOLD;
                                ImageCount = wImage.GetImageCount(state).ToString().PadLeft(3, Convert.ToChar("0"));
                                DocTypeCount = wImage.GetDocTypeCount(state).ToString().PadLeft(2, Convert.ToChar("0"));

                                
                                imageDs = wImage.GetIndexedImageName(ihConstants.PROPOSALFORM_FILE);
                                policyNumber = policyNumber.PadLeft(9,'0');
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];

                                    docType.Add(ihConstants.PROPOSALFORM_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.PROPOSALFORM_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.PROPOSALFORM_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.PHOTOADDENDUM_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];

                                    docType.Add(ihConstants.PHOTOADDENDUM_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.PHOTOADDENDUM_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.PHOTOADDENDUM_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.PROPOSALREVIEWSLIP_FILE);
                                
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.PROPOSALREVIEWSLIP_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.PROPOSALREVIEWSLIP_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.PROPOSALREVIEWSLIP_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.POLICYLOANS_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.POLICYLOANS_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.POLICYLOANS_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.POLICYLOANS_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.MEDICALREPORT_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.MEDICALREPORT_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.MEDICALREPORT_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.MEDICALREPORT_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.NOMINATION_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.NOMINATION_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.NOMINATION_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.NOMINATION_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.ASSIGNMENT_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.ASSIGNMENT_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.ASSIGNMENT_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.ASSIGNMENT_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.ALTERATION_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.ALTERATION_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.ALTERATION_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.ALTERATION_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.CLAIMS_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.CLAIMS_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.CLAIMS_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.CLAIMS_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.CORRESPONDENCE_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.CORRESPONDENCE_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.CORRESPONDENCE_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.CORRESPONDENCE_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.PROPOSALENCLOSERS_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.PROPOSALENCLOSERS_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.PROPOSALENCLOSERS_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.PROPOSALENCLOSERS_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.POLICYBOND_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.POLICYBOND_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.POLICYBOND_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.POLICYBOND_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.SURRENDER_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.SURRENDER_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.SURRENDER_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.SURRENDER_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.REVIVALS_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.REVIVALS_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.REVIVALS_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.REVIVALS_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.SIGNATUREPAGE_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.SIGNATUREPAGE_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.SIGNATUREPAGE_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.SIGNATUREPAGE_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.OTHERS_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];
                                    docType.Add(ihConstants.OTHERS_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.OTHERS_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }

                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.OTHERS_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                                imageDs = wImage.GetIndexedImageName(ihConstants.KYCDOCUMENT_FILE);
                                if (imageDs.Tables[0].Rows.Count > 0)
                                {
                                    //status =Convert.ToInt32(imageDs.Tables[0].Rows[propCount]["status"].ToString());
                                    imageName = new string[imageDs.Tables[0].Rows.Count];

                                    docType.Add(ihConstants.KYCDOCUMENT_FILE);
                                    multiPageFileName.Add(policyNumber + "_" + ihConstants.KYCDOCUMENT_FILE + ".TIF");
                                    for (int propCount = 0; propCount < imageDs.Tables[0].Rows.Count; propCount++)
                                    {
                                        if (File.Exists(imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString()) == false)
                                        {
                                            imagePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                                        }
                                        imageName[propCount] = imagePath + "\\" + imageDs.Tables[0].Rows[propCount]["page_index_name"].ToString();
                                        expImageCount = expImageCount + 1;
                                    }
                                    if (img.CombineTif(imageName, exportPath + "\\" + policyNumber + "_" + ihConstants.KYCDOCUMENT_FILE + ".TIF") == false)
                                    {
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if ((status == (int)eSTATES.POLICY_MISSING))
                                {
                                    boxno = "000";
                                    ImageCount = "000";
                                    DocTypeCount = "00";
                                    Scanuploadflag = "02";
                                    exportPath = batchPath + "\\" + ihConstants._RE_EXPORT_FOLDER + "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber;
                                    if (Directory.Exists(exportPath) == false)
                                    {
                                        Directory.CreateDirectory(exportPath);
                                    }
                                }
                                else
                                {
                                    if ((status == (int)eSTATES.POLICY_INITIALIZED))
                                    {
                                        MessageBox.Show("Item " + policyNumber + " have not been scanned, aborting...");
                                        ///Rollback transaction
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                    if ((status == (int)eSTATES.POLICY_SCANNED))
                                    {
                                        MessageBox.Show("QC not done for policy - " + policyNumber + " , aborting...");
                                        ///Rollback transaction
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                    if ((status == (int)eSTATES.POLICY_QC))
                                    {
                                        MessageBox.Show("Indexing not done for policy - " + policyNumber + " , aborting...");
                                        ///Rollback transaction
                                        exportTrans.Rollback();
                                        expBol = false;
                                        break;
                                    }
                                    if ((status == (int)eSTATES.POLICY_ON_HOLD))
                                    {
                                        ImageCount = "000";
                                        DocTypeCount = "00";
                                        Scanuploadflag = "02";
                                        exportPath = batchPath + "\\" + ihConstants._EXPORT_FOLDER + "\\" + vendorCode + "_" + divisionCode + "-" + branchCode.PadRight(4, Convert.ToChar(" ")) + "-" + batchCount + "_" + versionNumber + "\\" + policyNumber;
                                        if (Directory.Exists(exportPath) == false)
                                        {
                                            Directory.CreateDirectory(exportPath);
                                        }
                                    }
                                }
                            }
							for(int p=0;p < docType.Count;p++)
							{
								documentType = documentType + "," + docType[p].ToString() + "," + multiPageFileName[p].ToString();;
							}
                            totPolicyCount = totPolicyCount + 1;
							sw.WriteLine(string.Concat
							             (serial_no + ","
							              ,policyNumber + ","
							              ,divisionCode + ","
                                          , branchCode.PadRight(4, Convert.ToChar(" ")) + ","
							              ,batchSerial + ","
							              ,boxno + ","
							              ,Scanuploadflag + ","
							              ,scanneddate + ","
							              ,ihConstants.INCREMENTEDSCAN + ","
							              ,Policyholdername + ","
							              ,DOB + ","
							              ,dateofcommencement + ","
							              ,cust_id.PadRight(16,Convert.ToChar(" ")) + ","
							              ,ImageCount + ","
							              ,DocTypeCount + ","
							              ,rootPath
							              ,documentType));
							sw.Close();
							imageDs.Dispose();
							docType.Clear();
							multiPageFileName.Clear();
							documentType = string.Empty;
                            
		                    expLog = new StreamWriter(batchPath + "\\" + ihConstants._RE_EXPORT_FOLDER + "\\" + "Export_Log.txt", true);
		                    expLog.WriteLine("Project Name - " + cmbProject.Text);
		                    expLog.WriteLine("Batch Name - " + cmbBatch.Text);
		                    expLog.WriteLine("Box number - " + cmbBox.Text);
		                    expLog.WriteLine("Policy Exported - " + (expPolicyCount));
		                    expLog.WriteLine("Images Exported - " + expImageCount);
                        if (expBol == true)
                        {
                            ///Update box status
                            pBox = new CtrlBox(Convert.ToInt32(cmbProject.SelectedValue.ToString()), Convert.ToInt32(cmbBatch.SelectedValue.ToString()), Convert.ToInt32(boxno));
                            box = new wfeBox(sqlCon, pBox);
                            exportTrans.Commit();
                            toolStatus.Text = "Policy Number - " + lvwExportList.Items[i].SubItems[1].Text + "Exported successfully........";
                        }
                        else
                        {
                            expLog.WriteLine("Error in policy - " + policyNumber);
                            expLog.Close();
                            toolStatus.Text = "Error while exporting selected box.........";
                        }
                        expLog.Close();
                    	expSqlCon.Close();
                    	sw.Close();
					}
                    
                    
				}
                
			}
			catch(Exception ex)
			{
				error=ex.Message;
			   	MessageBox.Show("Error while exporting data...... " + error,"Export Error",MessageBoxButtons.OK);
                //expLog.WriteLine("Error while exporting policy - " + policyNumber);
                //expLog.Close();
                ///Rollback transaction
                sw.Close();
                exportTrans.Rollback();
                expSqlCon.Close();
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + cmbProject.Text + " ,Batch-" + cmbBatch.Text + " ,Box number-" + boxno + " ,Policy number-" + policyNumber + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
		}
	}
}
