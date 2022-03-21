/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 27/3/2009
 * Time: 11:27 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Imaging; 
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;
using WeifenLuo.WinFormsUI.Docking;
using System.Runtime.InteropServices;
using DockSample.Customization;
using System.IO;
using DockSample;
using NovaNet.Utils;
using NovaNet.wfe;
using System.Data;
using System.Data.Odbc;
using System.Collections;
using LItems;
//using AForge.Imaging;
//using AForge;
//using AForge.Imaging.Filters;
using TwainLib;
using GdiPlusLib;
using System.Threading;

namespace ImageHeaven
{
	/// <summary>
	/// Description of aeFQC.
	/// </summary>
	public partial class aeFQC : Form, IMessageFilter ,StateData
	{
        private bool indexingOn = false;
        //private DummyToolbox m_toolbox = new DummyToolbox();
        private OdbcConnection sqlCon = null;
        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        private FloatToolbox m_toolbox = new FloatToolbox();
        //private MagickNet.Image imgQc;
        private string imgFileName = null;
        //private ImageQC objQc=new ImageQC();
        NovaNet.Utils.ImageManupulation delImage;
        private wfeBox wBox = null;
        private CtrlPolicy pPolicy = null;
        private CtrlImage pImage = null;
        private static string docType;
        private CtrlBox pBox = null;
        private string indexFilePath = null;
        NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
        long fileSize;
        Credentials crd = new Credentials();
        int firstImage = 0;
        int lastImage = 0;
        ///For key config
        string cropKey;
        string zoomInKey;
        string zoomOutKey;
        string autoCropKey;
        string rotateRKey;
        string rotateLKey;
        string skewRKey;
        string skewLKey;
        string noiseRemovalLKey;
        string cleanKey;
        string deleteKey;
        private const int THUMBNAIL_DATA = 0x501B;
        /// <summary>
        /// For drawing rectangle
        /// </summary>
        private int cropX;
        private int cropY;
        private int cropWidth;
        private int cropHeight;
        private double constRotateAngle;
        private double skewXAngle;
        private double skewYAngle;
        bool hasPhotoBol;
        private int OperationInProgress;
		string[] imageName;
        //private Bitmap cropBitmap;
        delegate void ch();
        private Pen cropPen;
        private int cropPenSize = 1;
        private System.Drawing.Color cropPenColor = System.Drawing.Color.Blue;

        private Label lblImageSize = null;
        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed = 1;
        private ImageConfig config = null;
        private string policyPath = string.Empty;
        private ListBox delImgList = null;
        private ComboBox cmbBox = null;
        //GD objects
        //private GdPicture.GdPictureImaging imgQc = new GdPicture.GdPictureImaging();
        //private int ino;
        MemoryStream stateLog;
        byte[] tmpWrite;
        private ListBox policyLst = null;
        private ListBox imageLst = null;
        private ListBox imageDelLst = null;
        private CtrlPolicy ctrlPolicy = null;
        private wfePolicy policy = null;
        private udtPolicy policyData = null;
        private FileorFolder fileMove = null;
        private string sourceFilePath = null;
        private string indexFolderName = null;
        private string scanFolder = null;
        private string qcFolder = null;
        private string error;
        private bool insertFlag = false;
        private int lvwIndex;
        //Scanning
        private Twain twScan;
        private bool colorMode;
        private bool msgfilter;
        private int scanWhat = 0;
        private Label lblBatch;
        //private long Page2=0;
        
        private Imagery img;
        private Imagery imgAll;
        private Imagery imgBond;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        private string selDocType = string.Empty;
        private int currntPg = 0;
        private bool firstDoc = true;
        private string prevDoc;
        private int policyLen = 0;
        System.Windows.Forms.Button prmButtonCrop = new Button();
        System.Windows.Forms.Button prmButtonAutoCrp = new Button();
        System.Windows.Forms.Button prmButtonRotateRight = new Button();
        System.Windows.Forms.Button prmButtonRotateLeft = new Button();
        System.Windows.Forms.Button prmButtonZoomIn = new Button();
        System.Windows.Forms.Button prmButtonZoomOut = new Button();
        System.Windows.Forms.Button prmButtonSkewRight = new Button();
        System.Windows.Forms.Button prmButtonSkewLeft = new Button();
        System.Windows.Forms.Button prmButtonNoiseRemove = new Button();
        System.Windows.Forms.Button prmButtonCleanImg = new Button();
        System.Windows.Forms.Button prmButtonCopyImage = new Button();
        System.Windows.Forms.Button prmButtonDelImage = new Button();
        System.Windows.Forms.Button prmButtonRescan = new Button();
        System.Windows.Forms.Button prmButtonScan = new Button();
        System.Windows.Forms.Button prmButtonCopyTo = new Button();
        System.Windows.Forms.Button prmButtonImportImage = new Button();
        System.Windows.Forms.Button prmButtonMoveTo = new Button();
        System.Windows.Forms.Button prmButtonCopyImageTo = new Button();
        System.Windows.Forms.Button prmButtonCopyProposalForm = new Button();
        System.Windows.Forms.Button prmButtonCopyProposalReviewSlip = new Button();
    	
		public aeFQC(wfeBox prmBox,OdbcConnection prmCon,Credentials prmCrd)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
			sqlCon=prmCon;
			wBox = prmBox;
            crd = prmCrd;
			InitializeComponent();
            
			img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
			imgAll = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            imgBond = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
			//img = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);
			//imgAll = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);
			m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
			this.Text="Final QC";
            exMailLog.SetNextLogger(exTxtLog);
            
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public aeFQC()
		{			
			InitializeComponent();

			m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
			this.Text="Final QC";
            exMailLog.SetNextLogger(exTxtLog);
            
		}
        MemoryStream StateData.StateLog()
        {
            return stateLog;
        }
        /// <summary>
        /// It's a message filter interface
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            TwainCommand cmd = twScan.PassMessage(ref m);
            if (cmd == TwainCommand.Not)
            {
                this.Refresh();
                return false;
            }
            switch (cmd)
            {
                case TwainCommand.CloseRequest:
                    {
                        EndingScan();
                        twScan.CloseSrc();
                        break;
                    }
                case TwainCommand.CloseOk:
                    {
                        EndingScan();
                        twScan.CloseSrc();
                        break;
                    }
                case TwainCommand.DeviceEvent:
                    {
                        break;
                    }
                case TwainCommand.TransferReady:
                    {
                        Twain.ImageNotification delMan;
                        int pics;
                        if(scanWhat == ihConstants.SCAN_RE_FQC)
                        {
                            delMan  = new Twain.ImageNotification(GetImage);
                        	pics = twScan.TransferPicturesFixed(GetImage, this);
                        }
                        if(scanWhat == ihConstants.SCAN_NEW_FQC)
                        {
                            delMan = new Twain.ImageNotification(GetImageNew);
                        	pics = twScan.TransferPicturesFixed(GetImageNew, this);
                        }
                        twScan.CloseSrc();
                        EndingScan();
                        break;
                    }
            }

            return true;
        }		
		void Button1Click(object sender, EventArgs e)
		{
			m_toolbox.Show(dockPanel);
		}
		
		void AeFQCLoad(object sender, EventArgs e)
		{
            

            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
            //wfePolicy wPolicy = new wfePolicy(sqlCon);
            //int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_FQC);
            this.Text = this.Text;// +"                                       Today you have done " + count + " ";

            System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();
            ReadConfigKey();

			string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
			ArrayList arrPolicy=new ArrayList();
			
			twScan = new Twain();
            twScan.Init(this.Handle);
            
			wQuery pQuery=new ihwQuery(sqlCon);
			
			if (File.Exists(configFile))
				dockPanel.LoadFromXml(configFile, m_deserializeDockContent);

            
			delImage = new NovaNet.Utils.ImageManupulation(CropRegister);
			prmButtonCrop.Text="Crop";
            bttnToolTip.SetToolTip(prmButtonCrop, "Shortcut Key-" + cropKey);
			m_toolbox.AddButton(prmButtonCrop,delImage);

            delImage = new NovaNet.Utils.ImageManupulation(AutoCrop);
            prmButtonAutoCrp.Text = "Auto-Crop";
            bttnToolTip.SetToolTip(prmButtonAutoCrp, "Shortcut Key-" + autoCropKey);
            m_toolbox.AddButton(prmButtonAutoCrp, delImage);

            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            constRotateAngle =Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.ROTATE_ANGLE_KEY).Replace("\0", ""));
            delImage = new NovaNet.Utils.ImageManupulation(RotateRight);
            bttnToolTip.SetToolTip(prmButtonRotateRight, "Shortcut Key-" + rotateRKey);
            prmButtonRotateRight.Text = "Rotate Right";
            m_toolbox.AddButton(prmButtonRotateRight, delImage);

            //delImage = ZoomOut;
            delImage = new NovaNet.Utils.ImageManupulation(RotateLeft);
            prmButtonRotateLeft.Text = "Rotate Left";
            bttnToolTip.SetToolTip(prmButtonRotateLeft, "Shortcut Key-" + rotateLKey);
            m_toolbox.AddButton(prmButtonRotateLeft, delImage);
            
            delImage = new NovaNet.Utils.ImageManupulation(ZoomIn);
			prmButtonZoomIn.Text="Zoom In";
            bttnToolTip.SetToolTip(prmButtonZoomIn, "Shortcut Key-" + zoomInKey);
            m_toolbox.AddButton(prmButtonZoomIn,delImage);

            //delImage = ZoomOut;
            delImage = new NovaNet.Utils.ImageManupulation(ZoomOut);
            prmButtonZoomOut.Text = "Zoom Out";
            bttnToolTip.SetToolTip(prmButtonZoomOut, "Shortcut Key-" + zoomOutKey);
            m_toolbox.AddButton(prmButtonZoomOut, delImage);
            
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            //skewXAngle =Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.SKEW_X_KEY).Replace("\0", ""));
            //skewYAngle=Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.SKEW_Y_KEY).Replace("\0", ""));
            delImage = new NovaNet.Utils.ImageManupulation(SkewRight);
			prmButtonSkewRight.Text="DeSkew";
            bttnToolTip.SetToolTip(prmButtonSkewRight, "Shortcut Key-" + skewRKey);
            m_toolbox.AddButton(prmButtonSkewRight,delImage);

            delImage = new NovaNet.Utils.ImageManupulation(NoiseRemove);
            prmButtonNoiseRemove.Text="Despacle";
            bttnToolTip.SetToolTip(prmButtonNoiseRemove, "Shortcut Key-" + noiseRemovalLKey);
			prmButtonNoiseRemove.AutoSize=true;
            m_toolbox.AddButton(prmButtonNoiseRemove,delImage);
            
            delImage = new NovaNet.Utils.ImageManupulation(CleanImageRegister);
			prmButtonCleanImg.Text="Clean";
            bttnToolTip.SetToolTip(prmButtonCleanImg, "Shortcut Key-" + cleanKey);
			prmButtonCleanImg.AutoSize=true;
            m_toolbox.AddButton(prmButtonCleanImg,delImage);
            
            delImage = new NovaNet.Utils.ImageManupulation(ImageCopy);
            bttnToolTip.SetToolTip(prmButtonCopyImage, "Shortcut Key-(Control+z)");
			prmButtonCopyImage.Text="Copy Original";
			prmButtonCopyImage.AutoSize=true;
            m_toolbox.AddButton(prmButtonCopyImage,delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImageDelete);
            bttnToolTip.SetToolTip(prmButtonDelImage, "Shortcut Key-" + deleteKey);
            prmButtonDelImage.Text = "Delete";
            prmButtonDelImage.AutoSize = true;
            m_toolbox.AddButton(prmButtonDelImage, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(RescanImage);
            prmButtonRescan.Text="Rescan";
            prmButtonRescan.AutoSize=true;
            m_toolbox.AddButton(prmButtonRescan,delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ScanImage);
            prmButtonScan.Text = "Scan";
            prmButtonScan.AutoSize = true;
            m_toolbox.AddButton(prmButtonScan, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImportImage);
            prmButtonCopyTo.Text="Import Bond";
            prmButtonCopyTo.AutoSize = true;
            prmButtonCopyTo.Enabled = false;
            m_toolbox.AddButton(prmButtonCopyTo, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImportImageFromDir);
            prmButtonImportImage.Text = "Import Image";
            prmButtonImportImage.AutoSize = true;
            m_toolbox.AddButton(prmButtonImportImage, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(MoveImage);
            prmButtonMoveTo.Text = "Move To";
            prmButtonMoveTo.AutoSize = true;
            m_toolbox.AddButton(prmButtonMoveTo, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(CopyImageTo);
            prmButtonCopyImageTo.Text = "Copy To";
            prmButtonCopyImageTo.AutoSize = true;
            m_toolbox.AddButton(prmButtonCopyImageTo, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImportProposalForm);
            prmButtonCopyProposalForm.Text = "Import PF";
            prmButtonCopyProposalReviewSlip.Enabled = false;
            //bttnToolTip.SetToolTip(prmButtonCopyProposalForm, "Shortcut Key-" + autoCropKey);
            m_toolbox.AddButton(prmButtonCopyProposalForm, delImage);

            delImage = new NovaNet.Utils.ImageManupulation(ImportProposalReviewSlip);
            prmButtonCopyProposalReviewSlip.Text = "Import PRS";
            prmButtonCopyProposalForm.Enabled = false;
            //bttnToolTip.SetToolTip(prmButtonCopyProposalReviewSlip, "Shortcut Key-" + autoCropKey);
            m_toolbox.AddButton(prmButtonCopyProposalReviewSlip, delImage);

            imageLst= (ListBox)BoxDtls.Controls["lstImage"];
            imageDelLst= (ListBox)BoxDtls.Controls["lstImageDel"];
            policyLst= (ListBox)BoxDtls.Controls["lstPolicy"];
            Label delLabel = (Label) BoxDtls.Controls["label3"];
            //imageLst.SelectionMode = SelectionMode.MultiExtended;
            PopulatePolicyCombo();
            PopulateListView();
            //delLabel.Visible=false;
            //imageDelLst.Visible=false;
            imageLst.Enabled=true;
            policyLst.Enabled = true;
            
            ShowAllException();
            this.WindowState = FormWindowState.Maximized;
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            BoxDtls.PopulateDelList(Convert.ToInt32(policyLst.SelectedItem.ToString()));
            ///changed in version 1.0.0.1
            DisplayDockTypes();
            DisplayDocTypeCount();
            ihwQuery wqs = new ihwQuery(sqlCon);
            if (wqs.GetSysConfigValue(ihConstants.OTHER_THAN_POLICYBOND_IMPORT_NEEDED_KEY) == ihConstants.OTHER_THAN_POLICYBOND_IMPORT_NEEDED_VALUE)
            {
                prmButtonCopyProposalForm.Visible = true;
                prmButtonCopyProposalReviewSlip.Visible = true;
            }
            else
            {
                prmButtonCopyProposalForm.Visible = false;
                prmButtonCopyProposalReviewSlip.Visible = false;
            }
		}

        void cmdCustExcp_Click(object sender, System.EventArgs e)
        {
            frmCustExcp frmCust = new frmCustExcp(sqlCon,wBox);
            frmCust.Show();
        }
		private IDockContent GetContentFromPersistString(string persistString)
		{
				return m_toolbox;
		}
		
		private int RescanImage()
		{
            if(twScan.Select()==false)
				return 0;
            if (!msgfilter)
            {
                //this.Enabled = false;
                msgfilter = true;
                Application.AddMessageFilter(this);
            }
            DialogResult result = MessageBox.Show("Do you want to scan in color mode?", "Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                colorMode = true;
            }
            else
            {
                colorMode = false;
            }
            scanWhat = ihConstants.SCAN_RE_FQC;
            bool isOk = twScan.AcquireFixed(false, colorMode,1,0);
            if (!isOk)
            {
            	MessageBox.Show("Error in acquiring from scanner","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            	EndingScan();
            }        	
            return 1;
		}

        private int ImportImageFromDir()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[4];
            wfeImage wImage = null;
            string newImageName = string.Empty;
            string origImageName = string.Empty;
            try
            {
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageName = imageLst.SelectedItem.ToString();
                ///get the file name to copy
                fileDlg.Filter = "TIF File|*.TIF";
                fileDlg.FileName = string.Empty;
                fileDlg.Title = "B!nb - TIF Files";
                fileDlg.ShowDialog();
                imageName = fileDlg.FileName.ToString();
                //status = Convert.ToInt32(cmbPolicy.SelectedValue.ToString());
                if ((imageName != null) && ((imageName != string.Empty)))
                {

                    //copy to
                    pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    wPolicy = new wfePolicy(sqlCon, pPolicy);
                    string newPolicyPath = GetPolicyPath(Convert.ToInt32(policyLst.SelectedItem.ToString())); //wPolicy.GetPolicyPath();

                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()), string.Empty, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    int imgCount = wImage.GetImageCount();
                    imgCount++;
                    int pageCount = wImage.GetMaxPageCount();
                    pageCount++;
                    newImageName = policyLst.SelectedItem.ToString() + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                    if ((wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_FQC) || (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED) || (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_EXPORTED))
                    {
                        if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                        {
                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + newImageName, true);
                        }
                        //else
                        //{
                        //    if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                        //    {
                        //        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //    }
                        //    else
                        //    {
                        //        Directory.CreateDirectory(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER);
                        //        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //    }
                        //}
                    }
                    else
                    {
                        if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                        {
                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + newImageName, true);
                        }
                        //if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                        //{
                        //    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //}
                        //else
                        //{
                        //    Directory.CreateDirectory(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER);
                        //    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //}
                    }
                    //MessageBox.Show(ds.Tables[0].Rows[i]["status"].ToString());
                    System.IO.FileInfo info = new System.IO.FileInfo(fileDlg.FileName.ToString());
                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    //crd.created_by = "ADMIN";
                    crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), newImageName, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    //wImage.Save(crd, eSTATES.PAGE_FQC, fileSize, ihConstants._NORMAL_PAGE, imgCount,string.Empty);
                    if (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_EXPORTED)
                    {
                        wImage.Save(crd, eSTATES.PAGE_EXPORTED,fileSize, ihConstants._NORMAL_PAGE,imgCount, newImageName);
                    }
                    else
                    {
                        if (wImage.Save(crd, eSTATES.PAGE_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, imgCount, newImageName) == true)
                        {
                            policy.UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                        }
                    }
                }
                if (policyLst.Items.Count != 1)
                {
                    if ((policyLst.SelectedIndex) != (policyLst.Items.Count - 1))
                    {
                        policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                        policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                    }
                    else
                    {
                        policyLst.SelectedIndex = 0;
                        policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                    }
                }
                else
                {
                    BoxDtls.RefreshNotify();
                }
                MessageBox.Show("Image has been imported successfully.....");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while importing the selected image" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return 1;
        }

        private void PopulatePolicyCombo()
        {
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[5];

            pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber),0);
            wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
            
            
            state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            state[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
            state[3] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[4] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;

            ds = wPolicy.GetPolicyList(state);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cmbPolicy.DataSource = ds.Tables[0];
                cmbPolicy.DisplayMember = ds.Tables[0].Columns["policy_no"].ToString();
                cmbPolicy.ValueMember = ds.Tables[0].Columns["status"].ToString();
            }
        }

        private int ImportImage()
        {
            string imageName = null;
            wfePolicy wPolicy =null;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[5];
            wfeImage wImage = null;
            string newImageName = string.Empty;
			string origImageName=string.Empty;
            string policyNo = string.Empty;
            string batchName = string.Empty;
            string boxNumber = string.Empty;
            string tmpBondPath = Path.GetDirectoryName(Application.ExecutablePath);
            try
            {
                ihwQuery wQ = new ihwQuery(sqlCon);

                wfeBatch pBatch = new wfeBatch(sqlCon);
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                
                state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                state[1] = NovaNet.wfe.eSTATES.POLICY_FQC;
                state[2] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                state[3] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
                state[4] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
                
                fileDlg.Filter = "TIF File|*.TIF";
                fileDlg.FileName = string.Empty;
                fileDlg.Title = "B'Zer - TIF Files";
                fileDlg.ShowDialog();
                imageName = fileDlg.FileName.ToString();

                    if ((imageName != null) && ((imageName != string.Empty)))
                    {
                    	int j=0;
	                    for (int i = 0; i < policyLst.Items.Count; i++)
	                        {
	                            //MessageBox.Show(cmbPolicy.DisplayMember);
	                            //copy to
                                policyNo = policyLst.Items[i].ToString();
                                batchName = pBatch.GetBatchName(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
                                boxNumber = wBox.ctrlBox.BoxNumber.ToString();
                                
                                if (wQ.GetSysConfigValue(ihConstants.SPECIALBOND_KEY) == ihConstants.SPECIALBOND_VALUE)
                                {
                                    ///This is for auto text add
                                    Image image = Image.FromFile(fileDlg.FileName.ToString());
                                    Bitmap bmp = new Bitmap(image);

                                    bmp.SetResolution(200, 200);
                                    Graphics g = Graphics.FromImage(bmp);
                                    g.DrawString(batchName, new Font("Calibri", 11), Brushes.Black, new PointF(348, 395));
                                    g.DrawString(boxNumber, new Font("Calibri", 11), Brushes.Black, new PointF(781, 395));
                                    g.DrawString(policyNo, new Font("Calibri", 11), Brushes.Black, new PointF(1099, 395));
                                    bmp.Save(tmpBondPath + "\\tempBond.tif", ImageFormat.Tiff);
                                    imgBond.LoadBitmapFromFile(tmpBondPath + "\\tempBond.TIF");
                                    imgBond.ConvertTo1Bpp(tmpBondPath + "\\tempBond.TIF", tmpBondPath + "\\tempBond.TIF");
                                    imageName = tmpBondPath + "\\tempBond.TIF";
                                }
	                            pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo));
	                            wPolicy = new wfePolicy(sqlCon, pPolicy);
                                string newPolicyPath = GetPolicyPath(Convert.ToInt32(policyNo)); //wPolicy.GetPolicyPath();

                                pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo), string.Empty, string.Empty);
	                            wImage = new wfeImage(sqlCon, pImage);
	                            int imgCount = wImage.GetImageCount();
                                int pageCount = wImage.GetMaxPageCount();
                                pageCount++;
	                            imgCount++;
                                newImageName = policyNo + "_" + pageCount.ToString().PadLeft(3, '0') + "_A-Policy_bond.TIF";
                                origImageName = policyNo + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                                string status = wPolicy.GetPolicyStatus().ToString();
                                if ((Convert.ToInt32(status) != (int)eSTATES.POLICY_ON_HOLD))
                                {
                                    if ((Convert.ToInt32(status) == (int)eSTATES.POLICY_FQC) || ((Convert.ToInt32(status) == (int)eSTATES.POLICY_CHECKED)) || (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED))
                                    {
                                        if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                        {
                                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                        }
                                        else
                                        {
                                            if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                            {
                                                File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                                File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                        {
                                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                            File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                        }
                                    }
                                    //MessageBox.Show(ds.Tables[0].Rows[i]["status"].ToString());
                                    System.IO.FileInfo info = new System.IO.FileInfo(fileDlg.FileName.ToString());
                                    fileSize = info.Length;
                                    fileSize = fileSize / 1024;
                                    //crd.created_by = "ADMIN";
                                    crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo), origImageName, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);
                                    //wImage.Save(crd, eSTATES.PAGE_FQC, fileSize, ihConstants._NORMAL_PAGE, imgCount,string.Empty);
                                    if (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED)
                                    {
                                        wImage.Save(crd, eSTATES.PAGE_EXPORTED, ihConstants.POLICYBOND_FILE, newImageName, fileSize, imgCount);
                                    }
                                    else
                                    {
                                        wImage.Save(crd, eSTATES.PAGE_FQC, ihConstants.POLICYBOND_FILE, newImageName, fileSize, imgCount);
                                    }
                                    //wPolicy.UpdateStatus(eSTATES.POLICY_INDEXED,crd);
                                    j = j + 1;
                                }
	                            //Refresh the image list
                                if (File.Exists(tmpBondPath + "\\tempBond.TIF"))
                                {
                                    File.Delete(tmpBondPath + "\\tempBond.TIF");
                                }
	                        }
	                    if(j>0)
	                    {
                            if (policyLst.Items.Count != 1)
                            {
                                if ((policyLst.SelectedIndex) != (policyLst.Items.Count + 1))
                                {
                                    policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                    policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                                }
                                else
                                {
                                    policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                                    policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                }
                            }
                            else
                            {
                                imageLst.Items.Add(origImageName + "-" + ihConstants.POLICYBOND_FILE);
                            }
	                        MessageBox.Show("Image has been imported successfully in " + j + " policies.....");
                            prmButtonCopyTo.Enabled = false;
	                    }
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while importing the selected image" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return 1;
        }

        private int ImportProposalForm()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[5];
            wfeImage wImage = null;
            string newImageName = string.Empty;
            string origImageName = string.Empty;
            string policyNo = string.Empty;
            string batchName = string.Empty;
            string boxNumber = string.Empty;
            string tmpBondPath = Path.GetDirectoryName(Application.ExecutablePath);
            try
            {
                ihwQuery wQ = new ihwQuery(sqlCon);

                wfeBatch pBatch = new wfeBatch(sqlCon);
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];

                fileDlg.Filter = "TIF File|*.TIF";
                fileDlg.FileName = string.Empty;
                fileDlg.Title = "B'Zer - TIF Files";
                fileDlg.ShowDialog();
                imageName = fileDlg.FileName.ToString();

                if ((imageName != null) && ((imageName != string.Empty)))
                {
                    int j = 0;
                    for (int i = 0; i < policyLst.Items.Count; i++)
                    {
                        //MessageBox.Show(cmbPolicy.DisplayMember);
                        //copy to
                        policyNo = policyLst.Items[i].ToString();
                        batchName = pBatch.GetBatchName(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
                        boxNumber = wBox.ctrlBox.BoxNumber.ToString();

                        if (wQ.GetSysConfigValue(ihConstants.SPECIALBOND_KEY) == ihConstants.SPECIALBOND_VALUE)
                        {
                            ///This is for auto text add
                            Image image = Image.FromFile(fileDlg.FileName.ToString());
                            Bitmap bmp = new Bitmap(image);

                            bmp.SetResolution(200, 200);
                            Graphics g = Graphics.FromImage(bmp);
                            g.DrawString(batchName, new Font("Calibri", 11), Brushes.Black, new PointF(348, 395));
                            g.DrawString(boxNumber, new Font("Calibri", 11), Brushes.Black, new PointF(781, 395));
                            g.DrawString(policyNo, new Font("Calibri", 11), Brushes.Black, new PointF(1099, 395));
                            bmp.Save(tmpBondPath + "\\tempBond.tif", ImageFormat.Tiff);
                            imgBond.LoadBitmapFromFile(tmpBondPath + "\\tempBond.TIF");
                            imgBond.ConvertTo1Bpp(tmpBondPath + "\\tempBond.TIF", tmpBondPath + "\\tempBond.TIF");
                            imageName = tmpBondPath + "\\tempBond.TIF";
                        }
                        pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo));
                        wPolicy = new wfePolicy(sqlCon, pPolicy);
                        string newPolicyPath = GetPolicyPath(Convert.ToInt32(policyNo)); //wPolicy.GetPolicyPath();

                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo), string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        int imgCount = wImage.GetImageCount();
                        int pageCount = wImage.GetMaxPageCount();
                        pageCount++;
                        imgCount++;
                        newImageName = policyNo + "_" + pageCount.ToString().PadLeft(3, '0') + "_A-Proposal_form.TIF";
                        origImageName = policyNo + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                        string status = wPolicy.GetPolicyStatus().ToString();
                        if ((Convert.ToInt32(status) != (int)eSTATES.POLICY_ON_HOLD))
                        {
                            if ((Convert.ToInt32(status) == (int)eSTATES.POLICY_FQC) || ((Convert.ToInt32(status) == (int)eSTATES.POLICY_CHECKED)) || (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED))
                            {
                                if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                {
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                }
                                else
                                {
                                    if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                    {
                                        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                    }
                                }
                            }
                            else
                            {
                                if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                {
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                }
                            }
                            //MessageBox.Show(ds.Tables[0].Rows[i]["status"].ToString());
                            System.IO.FileInfo info = new System.IO.FileInfo(fileDlg.FileName.ToString());
                            fileSize = info.Length;
                            fileSize = fileSize / 1024;
                            //crd.created_by = "ADMIN";
                            crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo), origImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            //wImage.Save(crd, eSTATES.PAGE_FQC, fileSize, ihConstants._NORMAL_PAGE, imgCount,string.Empty);
                            if (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED)
                            {
                                wImage.Save(crd, eSTATES.PAGE_EXPORTED, ihConstants.PROPOSALFORM_FILE, newImageName, fileSize, imgCount);
                            }
                            else
                            {
                                wImage.Save(crd, eSTATES.PAGE_FQC, ihConstants.PROPOSALFORM_FILE, newImageName, fileSize, imgCount);
                            }
                            //wPolicy.UpdateStatus(eSTATES.POLICY_INDEXED,crd);
                            j = j + 1;
                        }
                        //Refresh the image list   
                        if (File.Exists(tmpBondPath + "\\tempBond.TIF"))
                        {
                            File.Delete(tmpBondPath + "\\tempBond.TIF");
                        }
                    }
                    if (j > 0)
                    {
                        if (policyLst.Items.Count != 1)
                        {
                            if ((policyLst.SelectedIndex) != (policyLst.Items.Count + 1))
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                            }
                            else
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            }
                        }
                        else
                        {
                            imageLst.Items.Add(origImageName + "-" + ihConstants.PROPOSALFORM_FILE);
                        }
                        MessageBox.Show("Image has been imported successfully into " + j + " policies.....");
                        prmButtonCopyProposalForm.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while importing the selected image" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return 1;
        }

        private int ImportProposalReviewSlip()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[5];
            wfeImage wImage = null;
            string newImageName = string.Empty;
            string origImageName = string.Empty;
            string policyNo = string.Empty;
            string batchName = string.Empty;
            string boxNumber = string.Empty;
            string tmpBondPath = Path.GetDirectoryName(Application.ExecutablePath);
            try
            {
                ihwQuery wQ = new ihwQuery(sqlCon);

                wfeBatch pBatch = new wfeBatch(sqlCon);
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];

                fileDlg.Filter = "TIF File|*.TIF";
                fileDlg.FileName = string.Empty;
                fileDlg.Title = "B'Zer - TIF Files";
                fileDlg.ShowDialog();
                imageName = fileDlg.FileName.ToString();

                if ((imageName != null) && ((imageName != string.Empty)))
                {
                    int j = 0;
                    for (int i = 0; i < policyLst.Items.Count; i++)
                    {
                        //MessageBox.Show(cmbPolicy.DisplayMember);
                        //copy to
                        policyNo = policyLst.Items[i].ToString();
                        batchName = pBatch.GetBatchName(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
                        boxNumber = wBox.ctrlBox.BoxNumber.ToString();

                        if (wQ.GetSysConfigValue(ihConstants.SPECIALBOND_KEY) == ihConstants.SPECIALBOND_VALUE)
                        {
                            ///This is for auto text add
                            Image image = Image.FromFile(fileDlg.FileName.ToString());
                            Bitmap bmp = new Bitmap(image);

                            bmp.SetResolution(200, 200);
                            Graphics g = Graphics.FromImage(bmp);
                            g.DrawString(batchName, new Font("Calibri", 11), Brushes.Black, new PointF(348, 395));
                            g.DrawString(boxNumber, new Font("Calibri", 11), Brushes.Black, new PointF(781, 395));
                            g.DrawString(policyNo, new Font("Calibri", 11), Brushes.Black, new PointF(1099, 395));
                            bmp.Save(tmpBondPath + "\\tempBond.tif", ImageFormat.Tiff);
                            imgBond.LoadBitmapFromFile(tmpBondPath + "\\tempBond.TIF");
                            imgBond.ConvertTo1Bpp(tmpBondPath + "\\tempBond.TIF", tmpBondPath + "\\tempBond.TIF");
                            imageName = tmpBondPath + "\\tempBond.TIF";
                        }
                        pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo));
                        wPolicy = new wfePolicy(sqlCon, pPolicy);
                        string newPolicyPath = GetPolicyPath(Convert.ToInt32(policyNo)); //wPolicy.GetPolicyPath();

                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo), string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        int imgCount = wImage.GetImageCount();
                        int pageCount = wImage.GetMaxPageCount();
                        pageCount++;
                        imgCount++;
                        newImageName = policyNo + "_" + pageCount.ToString().PadLeft(3, '0') + "_A-Proposal_review_slip.TIF";
                        origImageName = policyNo + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                        string status = wPolicy.GetPolicyStatus().ToString();
                        if ((Convert.ToInt32(status) != (int)eSTATES.POLICY_ON_HOLD))
                        {
                            if ((Convert.ToInt32(status) == (int)eSTATES.POLICY_FQC) || ((Convert.ToInt32(status) == (int)eSTATES.POLICY_CHECKED)) || (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED))
                            {
                                if (Directory.Exists(newPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                {
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                }
                                else
                                {
                                    if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                    {
                                        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                        File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                    }
                                }
                            }
                            else
                            {
                                if (Directory.Exists(newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER))
                                {
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                    File.Copy(imageName, newPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + origImageName, true);
                                }
                            }
                            //MessageBox.Show(ds.Tables[0].Rows[i]["status"].ToString());
                            System.IO.FileInfo info = new System.IO.FileInfo(fileDlg.FileName.ToString());
                            fileSize = info.Length;
                            fileSize = fileSize / 1024;
                            //crd.created_by = "ADMIN";
                            crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo), origImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            //wImage.Save(crd, eSTATES.PAGE_FQC, fileSize, ihConstants._NORMAL_PAGE, imgCount,string.Empty);
                            if (Convert.ToInt32(status) == (int)eSTATES.POLICY_EXPORTED)
                            {
                                wImage.Save(crd, eSTATES.PAGE_EXPORTED, ihConstants.PROPOSALREVIEWSLIP_FILE, newImageName, fileSize, imgCount);
                            }
                            else
                            {
                                wImage.Save(crd, eSTATES.PAGE_FQC, ihConstants.PROPOSALREVIEWSLIP_FILE, newImageName, fileSize, imgCount);
                            }
                            //wPolicy.UpdateStatus(eSTATES.POLICY_INDEXED,crd);
                            j = j + 1;
                        }
                        //Refresh the image list   
                        if (File.Exists(tmpBondPath + "\\tempBond.TIF"))
                        {
                            File.Delete(tmpBondPath + "\\tempBond.TIF");
                        }
                    }
                    if (j > 0)
                    {
                        if (policyLst.Items.Count != 1)
                        {
                            if ((policyLst.SelectedIndex) != (policyLst.Items.Count + 1))
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                            }
                            else
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            }
                        }
                        else
                        {
                            imageLst.Items.Add(origImageName + "-" + ihConstants.PROPOSALREVIEWSLIP_FILE);
                        }
                        MessageBox.Show("Image has been imported successfully in " + j + " policies.....");
                        prmButtonCopyProposalReviewSlip.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while importing the selected image" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return 1;
        }

        private int MoveImage()
        {
            string imageName = null;
            wfePolicy wPolicy =null;
            string newImageName = string.Empty;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
            wfeImage wImage = null;
            int pos =0;
            string originalFileName = string.Empty;
            System.IO.FileInfo info = null;
            string origDoctype=null;
            string toMoveOriginalFileN;
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            bool imagemoved = false;
            int movedImgCount = 0;
            try
            {
                if (cmbPolicy.SelectedValue != null)
                {
                    if (policyLst.SelectedItem.ToString() != cmbPolicy.Text)
                    {
                        for (int i = 0; i < imageLst.Items.Count; i++)
                        {
                            if (imageLst.GetSelected(i))
                            {
                                movedImgCount++;
                                imageName = imageLst.Items[i].ToString();
                                pos = imageName.IndexOf("-");
                                if (pos > 0)
                                {
                                    //imageName = imageName.Substring(0);
                                    originalFileName = imageName.Substring(0, pos);
                                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), originalFileName, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);
                                    imageName = wImage.GetIndexedImageName();
                                    origDoctype = imageLst.Items[i].ToString().Substring(pos + 1);
                                }
                                else
                                {
                                    imageName = imageLst.Items[i].ToString();
                                    originalFileName = imageName;
                                }
                                //MessageBox.Show(cmbPolicy.Text);
                                pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                                wPolicy = new wfePolicy(sqlCon, pPolicy);
                                string fromPolicyPath = GetPolicyPath(Convert.ToInt32(policyLst.SelectedItem.ToString())); //wPolicy.GetPolicyPath();

                                pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(cmbPolicy.Text), string.Empty, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                                int imgCount = wImage.GetImageCount();
                                imgCount++;
                                int pageCount = wImage.GetMaxPageCount();
                                pageCount = pageCount + 1;
                                pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(cmbPolicy.Text));
                                wPolicy = new wfePolicy(sqlCon, pPolicy);
                                string toPolicyPath = GetPolicyPath(Convert.ToInt32(cmbPolicy.Text)); //wPolicy.GetPolicyPath();
                                if (pos <= 0)
                                {
                                    newImageName = cmbPolicy.Text + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                                    toMoveOriginalFileN = newImageName;
                                }
                                else
                                {
                                    newImageName = cmbPolicy.Text + "_" + pageCount.ToString().PadLeft(3, '0') + "_A-" + origDoctype + ".TIF";
                                    toMoveOriginalFileN = cmbPolicy.Text + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                                }


                                //if ((Convert.ToInt32(cmbPolicy.SelectedValue) == (int)eSTATES.POLICY_FQC) || ((Convert.ToInt32(cmbPolicy.SelectedValue) == (int)eSTATES.POLICY_CHECKED)) || ((Convert.ToInt32(cmbPolicy.SelectedValue) == (int)eSTATES.POLICY_EXCEPTION)))
                                //{
                                if (Directory.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                {
                                    if (Directory.Exists(toPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                    {
                                        if (File.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName))
                                        {
                                            File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                            //File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                            //File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._QC_FOLDER + "\\" + toMoveOriginalFileN, true);
                                            File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + toMoveOriginalFileN, true);
                                            File.Delete(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName);
                                            info = new System.IO.FileInfo(toPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName);
                                            imagemoved = true;
                                        }
                                    }
                                    else
                                    {
                                        if (File.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName))
                                        {
                                            File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                            File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + toMoveOriginalFileN, true);
                                            info = new System.IO.FileInfo(toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName);
                                            //File.Delete(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName);
                                            imagemoved = true;
                                        }
                                    }
                                }
                                if (imagemoved == true)
                                {
                                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), originalFileName, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);
                                    if (wImage.DeletePage())
                                    {
                                        imagemoved = true;
                                        UpdateAllStatus();
                                    }
                                    else
                                    {
                                        imagemoved = false;
                                    }
                                }
                                fileSize = info.Length;
                                fileSize = fileSize / 1024;
                                pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(cmbPolicy.Text), toMoveOriginalFileN, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                                if (imagemoved == true)
                                {
                                    if (pos <= 0)
                                    {
                                        if (wImage.Save(crd, eSTATES.PAGE_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, imgCount, string.Empty))
                                        {
                                            wPolicy.UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                                        }
                                    }
                                    else
                                    {
                                        if (img.GetBitmap().PixelFormat != PixelFormat.Format24bppRgb)
                                        {
                                            if (wImage.Save(crd, eSTATES.PAGE_FQC, origDoctype, newImageName, fileSize, imgCount))
                                            {
                                                if (wPolicy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                                                {
                                                    wPolicy.UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                                                }
                                                else if ((wPolicy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                                                {
                                                    wPolicy.UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                                                }
                                                else
                                                {
                                                    wPolicy.UpdateStatus(eSTATES.POLICY_FQC, crd);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DataSet imageDs = wImage.GetIndexedImageName(origDoctype);
                                            int maxSrl = imgCount - 1;
                                            if (wImage.RearrangePoposalDoctype(imageDs, maxSrl))
                                            {
                                                if (wImage.Save(crd, eSTATES.PAGE_FQC, origDoctype, newImageName, fileSize, imgCount))
                                                {
                                                    if (wPolicy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                                                    {
                                                        wPolicy.UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                                                    }
                                                    else if ((wPolicy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                                                    {
                                                        wPolicy.UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                                                    }
                                                    else
                                                    {
                                                        wPolicy.UpdateStatus(eSTATES.POLICY_FQC, crd);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    
                                }
                                
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Image move to same policy is not possible.....");
                        
                    }
                }
                else
                {
                    MessageBox.Show("Given policy is not a valid policy.....");
                    
                }
                if (imagemoved == true)
                {
                    MessageBox.Show(movedImgCount + " Images moved successfully.....");
                    //if ((policyLst.SelectedIndex) != (policyLst.Items.Count - 1))
                    //{
                    //    policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                    //    policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                    //}
                    //else
                    //{
                    //    policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                    //    policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                    //}
                    int selImageIndx = imageLst.SelectedIndex;
                    imageLst.Items.RemoveAt(imageLst.SelectedIndex);
                    if ((selImageIndx) != imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = selImageIndx;
                    }
                    else
                    {
                        imageLst.SelectedIndex = selImageIndx - 1;
                    }
                    ShowImage(false);
                    DisplayDocTypeCount();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while move the image to the destination policy..... " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return 1;
        }

        private int CopyImageTo()
        {
            string imageName = null;
            wfePolicy wPolicy = null;
            string newImageName = string.Empty;
            DataSet ds = new DataSet();
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
            wfeImage wImage = null;
            int pos = 0;
            string originalFileName = string.Empty;
            System.IO.FileInfo info = null;
            string origDoctype = null;
            string toMoveOriginalFileN;
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            bool imagemoved = false;
            try
            {
                if (cmbPolicy.SelectedValue != null)
                {
                    if (policyLst.SelectedItem.ToString() != cmbPolicy.Text)
                    {
                        imageName = imageLst.SelectedItem.ToString();
                        pos = imageName.IndexOf("-");
                        if (pos > 0)
                        {
                            //imageName = imageName.Substring(0);
                            originalFileName = imageName.Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), originalFileName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            imageName = wImage.GetIndexedImageName();
                            origDoctype = imageLst.SelectedItem.ToString().Substring(pos + 1);
                        }
                        else
                        {
                            imageName = imageLst.SelectedItem.ToString();
                            originalFileName = imageName;
                        }
                        //MessageBox.Show(cmbPolicy.Text);
                        pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                        wPolicy = new wfePolicy(sqlCon, pPolicy);
                        string fromPolicyPath = GetPolicyPath(Convert.ToInt32(policyLst.SelectedItem.ToString())); //wPolicy.GetPolicyPath();

                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(cmbPolicy.Text), string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        int imgCount = wImage.GetImageCount();
                        imgCount++;
                        int pageCount = wImage.GetMaxPageCount();
                        pageCount = pageCount + 1;
                        pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(cmbPolicy.Text));
                        wPolicy = new wfePolicy(sqlCon, pPolicy);
                        string toPolicyPath = GetPolicyPath(Convert.ToInt32(cmbPolicy.Text)); //wPolicy.GetPolicyPath();
                        if (pos <= 0)
                        {
                            newImageName = cmbPolicy.Text + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                            toMoveOriginalFileN = newImageName;
                        }
                        else
                        {
                            newImageName = cmbPolicy.Text + "_" + pageCount.ToString().PadLeft(3, '0') + "_A-" + origDoctype + ".TIF";
                            toMoveOriginalFileN = cmbPolicy.Text + "_" + pageCount.ToString().PadLeft(3, '0') + "_A.TIF";
                        }

                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), originalFileName, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        //if ((Convert.ToInt32(cmbPolicy.SelectedValue) == (int)eSTATES.POLICY_FQC) || ((Convert.ToInt32(cmbPolicy.SelectedValue) == (int)eSTATES.POLICY_CHECKED)) || ((Convert.ToInt32(cmbPolicy.SelectedValue) == (int)eSTATES.POLICY_EXCEPTION)))
                        //{
                            if (Directory.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                            {
                                if (Directory.Exists(toPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                                {
                                    if (File.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName))
                                    {
                                        File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                                        //File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                        //File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._QC_FOLDER + "\\" + toMoveOriginalFileN, true);
                                        File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + toMoveOriginalFileN, true);
                                        //File.Delete(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName);
                                        info = new System.IO.FileInfo(toPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName);
                                        imagemoved = true;
                                    }
                                }
                                else
                                {
                                    if (File.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName))
                                    {
                                        File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                                        File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + toMoveOriginalFileN, true);
                                        info = new System.IO.FileInfo(toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName);
                                        imagemoved = true;
                                        //File.Delete(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName);
                                    }
                                }
                            }
                            //else
                            //{
                            //    if (Directory.Exists(toPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                            //    {
                            //        File.Copy(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + newImageName, true);
                            //        File.Copy(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                            //        File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._QC_FOLDER + "\\" + toMoveOriginalFileN, true);
                            //        File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + toMoveOriginalFileN, true);
                            //        //File.Delete(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName);
                            //        imagemoved = true;
                            //    }

                            //    if (File.Exists(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName))
                            //    {
                            //        //File.Copy(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                            //        //File.Delete(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName);
                            //    }
                            //    info = new System.IO.FileInfo(toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName);
                            //}
                        //}
                        //else
                        //{
                        //    if (Directory.Exists(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER))
                        //    {
                        //        File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //        //File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._QC_FOLDER + "\\" + toMoveOriginalFileN, true);
                        //        File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + toMoveOriginalFileN, true);
                        //        if (File.Exists(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName))
                        //        {
                        //            //File.Copy(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //            //File.Delete(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName);
                        //            //File.Delete(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName);
                        //        }
                        //        imagemoved = true;
                        //        info = new System.IO.FileInfo(toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName);
                        //    }
                        //    //else
                        //    //{
                        //    //    File.Copy(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //    //    File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._QC_FOLDER + "\\" + toMoveOriginalFileN, true);
                        //    //    File.Copy(fromPolicyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + toMoveOriginalFileN, true);
                        //    //    if (File.Exists(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName))
                        //    //    {
                        //    //        //File.Copy(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName, toPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + newImageName, true);
                        //    //        //File.Delete(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName);
                        //    //    }
                        //    //    imagemoved = true;
                        //    //    info = new System.IO.FileInfo(fromPolicyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageName);
                        //    //}
                        //}
                        //MessageBox.Show(ds.Tables[0].Rows[i]["status"].ToString());

                        fileSize = info.Length;
                        fileSize = fileSize / 1024;
                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(cmbPolicy.Text), toMoveOriginalFileN, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        if (imagemoved == true)
                        {
                            if (pos <= 0)
                            {
                                if (img.GetBitmap().PixelFormat != PixelFormat.Format24bppRgb)
                                {
                                    if (wImage.Save(crd, eSTATES.PAGE_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, imgCount, string.Empty))
                                    {
                                        wPolicy.UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                                    }
                                }
                                else
                                {
                                    DataSet imageDs = wImage.GetIndexedImageName(ihConstants.PROPOSALFORM_FILE);
                                    int maxSrl = imgCount - 1;
                                    if (wImage.RearrangePoposalDoctype(imageDs, maxSrl))
                                    {
                                        if (wImage.Save(crd, eSTATES.PAGE_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, imgCount, string.Empty))
                                        {
                                            wPolicy.UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (img.GetBitmap().PixelFormat != PixelFormat.Format24bppRgb)
                                {
                                    if (wImage.Save(crd, eSTATES.PAGE_FQC, origDoctype, newImageName, fileSize, imgCount))
                                    {
                                        wPolicy.UpdateStatus(eSTATES.POLICY_FQC, crd);
                                    }
                                }
                                else
                                {
                                    DataSet imageDs = wImage.GetIndexedImageName(ihConstants.PROPOSALFORM_FILE);
                                    int maxSrl = imgCount - 1;
                                    if (wImage.RearrangePoposalDoctype(imageDs, maxSrl))
                                    {
                                        if (wImage.Save(crd, eSTATES.PAGE_FQC, origDoctype, newImageName, fileSize, imgCount))
                                        {
                                            wPolicy.UpdateStatus(eSTATES.POLICY_FQC, crd);
                                        }
                                    }
                                }
                            }
                            MessageBox.Show("Image copied successfully.....");
                        }
                        if ((policyLst.SelectedIndex) != (policyLst.Items.Count - 1))
                        {
                            policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                        }
                        else
                        {
                            policyLst.SelectedIndex = policyLst.SelectedIndex - 1;
                            policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Image copy to same policy is not possible.....");

                    }
                }
                else
                {
                    MessageBox.Show("Given policy is not a valid policy.....");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while move the image to the destination policy..... " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + newImageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return 1;
        }

		private int ScanImage()
		{
            DialogResult result ;
            bool isOk;
            if(twScan.Select()==false)
				return 0;
            
            if (!msgfilter)
            {
                //this.Enabled = false;
                msgfilter = true;
                Application.AddMessageFilter(this);
            }
            result = MessageBox.Show("Do you want to scan in color mode?", "Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                colorMode = true;
            }
            else
            {
                colorMode = false;
            }

            
            scanWhat = ihConstants.SCAN_NEW_FQC;
            
            result = MessageBox.Show("Do you want to scan in duplex mode?", "Scan mode", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                isOk = twScan.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 1);
            }
            else
            {
                isOk = twScan.AcquireFixed(false, colorMode, ihConstants.MAX_NO_SCAN_FQC, 0);
            }
            if (!isOk)
            {
            	MessageBox.Show("Error in acquiring from scanner","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            	EndingScan();
            }        	
            return 1;
		}
		void AeFQCFormClosing(object sender, FormClosingEventArgs e)
		{
			string photoPath=string.Empty;
			wfeImage wImage;
			string changedImageName;
			string policyPath;
			string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			System.IO.FileInfo info;
			long fileSize;
            wfeBatch wBatch;
			try
			{
				if(policyLst.Items.Count > 0)
				{
					for(int i=0; i<policyLst.Items.Count;i++)
					{
						pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.Items[i].ToString()),string.Empty,string.Empty);
			    		wImage  = new wfeImage(sqlCon, pImage);
			    		
                        NovaNet.wfe.eSTATES[] polState=new NovaNet.wfe.eSTATES[1];
						ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.Items[i].ToString()));
					    policy = new wfePolicy(sqlCon, ctrlPolicy);

                        if ((wImage.GetImageCount(eSTATES.PAGE_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_EXPORTED) == false) && (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
                        {
                            if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                            {
                                policy.UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                            }
                            else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                            {
                                policy.UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                            }
                            else
                            {
                                policy.UpdateStatus(eSTATES.POLICY_FQC, crd);
                            }
                        }
                        
					    pBox = new CtrlBox(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey,wBox.ctrlBox.BoxNumber);
					    wBox = new wfeBox(sqlCon,pBox);
					    NovaNet.wfe.eSTATES[] state=new NovaNet.wfe.eSTATES[3];
			            state[0]=NovaNet.wfe.eSTATES.POLICY_INDEXED;
                        state[1] = NovaNet.wfe.eSTATES.POLICY_QC;
                        state[2] = NovaNet.wfe.eSTATES.POLICY_SCANNED;
					    if(policy.GetPolicyCount(state) == 0)
					    {
					    	wBox.UpdateStatus(eSTATES.BOX_FQC);
					    }

                        ///For checking the box status
                        NovaNet.wfe.eSTATES[] boxState = new NovaNet.wfe.eSTATES[3];
                        boxState[0] = NovaNet.wfe.eSTATES.BOX_SCANNED;
                        boxState[1] = NovaNet.wfe.eSTATES.BOX_INDEXED;
                        boxState[2] = NovaNet.wfe.eSTATES.BOX_QC;

                        wBatch = new wfeBatch(sqlCon);
                        //int cont = wBox.GetBoxCount(boxState);
                        if (wBox.GetBoxCount(boxState) == 0)
                        {
                            ///Update the batch status
                            wBatch.UpdateStatus(eSTATES.BATCH_FQC, wBox.ctrlBox.BatchKey);
                        }
						
					    if(policy.GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
						{
							hasPhotoBol = true;
						}
						else
						{
							hasPhotoBol = false;
						}
						
					    //policyData=(udtPolicy)policy.LoadValuesFromDB();
                        policyPath = GetPolicyPath(); //policyData.policy_path;
					    if(Directory.Exists(policyPath + "\\" + ihConstants._FQC_FOLDER))
					    {
					    	pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.Items[i].ToString()),policyLst.Items[i].ToString() + "_000_A.TIF",string.Empty);
							wImage  = new wfeImage(sqlCon, pImage);
							changedImageName = wImage.GetIndexedImageName();
							if(changedImageName == string.Empty)
							{
					    		photoPath = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + policyLst.Items[i].ToString() + "_000_A.TIF";
							}
							else
							{
								photoPath = policyPath + "\\" + ihConstants._FQC_FOLDER + "\\" + changedImageName;
							}
					    	if(File.Exists(photoPath))
					    	{
								//Open the source file
								img.LoadBitmapFromFile(photoPath);				
								//Show the image back in picture box
								img.SaveFile(photoPath);
                                info = new System.IO.FileInfo(photoPath);
                                fileSize = info.Length;
                                fileSize = fileSize / 1024;
                                //UpdateImageSize(fileSize);
					    	}
					    }
					    
					}
				}
				//sqlCon.Close();
//	            if (m_bSaveLayout)
//	                dockPanel.SaveAsXml(configFile);
//	            else if (File.Exists(configFile))
//	                File.Delete(configFile);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error - " + ex.Message);
                exMailLog.Log(ex);
			}
		}
		
		int Crop(Rectangle rect)
		{
			Bitmap bmpImage = new Bitmap(pictureControl.Image);
			double htRatio = 0;
			double wdRatio = 0;
			
			try
			{
				if (img.IsValid() == true)
				{
					bmpImage = img.GetBitmap();
					
					htRatio = Convert.ToDouble(bmpImage.Size.Height)/Convert.ToDouble(pictureControl.Height);
					rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
					wdRatio  = (Convert.ToDouble(bmpImage.Size.Width)/Convert.ToDouble(pictureControl.Width));
					rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width)*wdRatio);
	
					rect.X = Convert.ToInt32(rect.X * wdRatio);
					rect.Y = Convert.ToInt32(rect.Y * htRatio);

					img.Crop(rect);
					img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
				}
				ChangeSize();
				
				System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
				fileSize = info.Length;
				fileSize = fileSize / 1024;
                lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImageSize.Text = fileSize.ToString() + " KB";
				UpdateImageSize(fileSize);
			}
		
			catch(Exception ex)
			{
				MessageBox.Show("Error while cropping the image","Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imageName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}		
			return 0;		
		}
		int Clean(Rectangle rect)
		{
            	//Rectangle rect  = new Rectangle(e.X,e.Y, 6,6);

				double htRatio = 0;
				double wdRatio = 0;
				long fileSize;
				try
				{
					if (img.IsValid() == true)
					{
						Bitmap bmpImage = img.GetBitmap();
					
						htRatio = Convert.ToDouble(bmpImage.Size.Height)/Convert.ToDouble(pictureControl.Height);
						rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
						wdRatio  = (Convert.ToDouble(bmpImage.Size.Width)/Convert.ToDouble(pictureControl.Width));
						rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width)*wdRatio);
	
						rect.X = Convert.ToInt32(rect.X * wdRatio);
						rect.Y = Convert.ToInt32(rect.Y * htRatio);
						//MessageBox.Show("Before filling " + gdi.GetBitDepth(imageNo).ToString());
						img.Clean(rect);
						img.SaveFile(imgFileName);
						img.LoadBitmapFromFile(imgFileName);
					}
					//Change the size of the image in relation to the canvas
					ChangeSize();
					
					System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
                    lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                    lblImageSize.Text = fileSize.ToString() + " KB";
					UpdateImageSize(fileSize);
					//bmpCrop.Dispose();
					}
					catch(Exception ex)
					{
                        stateLog = new MemoryStream();
                        tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                        stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                        exMailLog.Log(ex, this);
						MessageBox.Show("Error while cleaning the image" + ex.Message,"Crop error");
						return 0;
					}
				return 1;
		}		
		int ImageDelete()
		{
			string qcDelPath=null;
            string sourcePath = null;
            string desPath = null;
            string fileName = null;
			int pos;
            string originalImage = null;
            string scanPath = null;
            try
            {
                qcDelPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                if (imageLst.SelectedItem.ToString() != string.Empty)
                {
                    //Build string for supposed to be file in indexing folder
                    sourcePath = sourceFilePath + "\\" + imageLst.SelectedItem.ToString();
                    //Build string for file in FQC folder
                    desPath = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
                }
                //Wild card search: To save in the indexing folder
                fileName = imageLst.SelectedItem.ToString();
                
                pos = fileName.IndexOf("-");
                if (pos > 0)
                {
                    originalImage = fileName.Substring(0, pos - 4) + "*" + ".TIF";
                    //string[] searchFileName = Directory.GetFiles(sourceFilePath, originalImage);
                    ////For the file in index folder
                    //if (searchFileName.Length >= 0)
                    //    sourcePath = searchFileName[0];
                    //For the file in FQC folder
                    string[] searchFileName = Directory.GetFiles(indexFolderName, originalImage);
                    if (searchFileName.Length >= 0)
                    {
                        desPath = searchFileName[0];
                    }
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), fileName.Substring(0, pos - 4) + ".TIF", string.Empty);
                }
                else
                {
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageLst.SelectedItem.ToString(), string.Empty);
                }
                
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                string changedImageName = wImage.GetIndexedImageName();
                scanPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + pImage.ImageName;
                
                if (pos>0)
                {
                    int i = imageLst.SelectedItem.ToString().IndexOf("Signature_page");
                    if (imageLst.SelectedItem.ToString().IndexOf("Signature_page") <= 0)
                    {
                    	if(UpdateState(eSTATES.PAGE_DELETED, fileName.Substring(0, pos - 4) + ".TIF", string.Empty, string.Empty))
                    	{
	                        if (FileorFolder.CreateFolder(qcDelPath) == true)
	                        {
	                            if (File.Exists(desPath) == true)
	                            {
	                                if (changedImageName != string.Empty)
	                                {
	                                    File.Move(desPath, qcDelPath + "\\" + changedImageName);
	                                }
	                                else
	                                {
	                                    File.Move(desPath, qcDelPath + "\\" + imageLst.SelectedItem.ToString());
	                                }
                                    if(File.Exists(scanPath))
	                                    File.Delete(scanPath);
	                            }
	                        }
                    	}
                    }
                    else
                    {
                        //pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(lblCurrentPolicy.Text.ToString()), imagename, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        if(wImage.DeleteImage() == true)
                        {
                            if (FileorFolder.CreateFolder(qcDelPath) == true)
                            {
                                if (File.Exists(desPath) == true)
                                {
                                    //File.Delete(sourcePath);
                                    File.Delete(desPath);
                                    if (File.Exists(scanPath))
                                        File.Delete(scanPath);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //if (imageLst.SelectedItem.ToString().IndexOf("Signature_page") == 0)
                    //{
                    	if(UpdateState(eSTATES.PAGE_DELETED, imageLst.SelectedItem.ToString()))
                    	{
	                        if (FileorFolder.CreateFolder(qcDelPath) == true)
	                        {
	                            if (File.Exists(desPath) == true)
	                            {
	                                if (changedImageName != string.Empty)
	                                {
	                                    File.Move(desPath, qcDelPath + "\\" + changedImageName);
	                                }
	                                else
	                                {
	                                    File.Move(desPath, qcDelPath + "\\" + imageLst.SelectedItem.ToString());
	                                }
                                    if (File.Exists(scanPath))
                                        File.Delete(scanPath);
	                            }
	                        }
                    	}
                    //}
                    //else
                    //{
                    //    wImage = new wfeImage(sqlCon, pImage);
                    //    if (wImage.DeleteImage() == true)
                    //    {
                    //        if (FileorFolder.CreateFolder(qcDelPath) == true)
                    //        {
                    //            if (File.Exists(desPath) == true)
                    //            {
                    //                //File.Delete(sourcePath);
                    //                File.Delete(desPath);
                    //            }
                    //        }
                    //    }
                    //}
                }
                BoxDtls.DeleteNotify(imageLst.SelectedIndex);
                ShowImage(false);
                DisplayDocTypeCount();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while delete this file..." + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + fileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
                return 1;
            }
            return 0;
		}
	    private static ImageCodecInfo GetEncoderInfo(String mimeType)
	    {
	        int j;
	        ImageCodecInfo[] encoders;
	        encoders = ImageCodecInfo.GetImageEncoders();
	        for(j = 0; j < encoders.Length; ++j)
	        {
	            if(encoders[j].MimeType == mimeType)
	                return encoders[j];
	        }
	        return null;
	    }
		//Should be changed, some problem.
		int ImageCopy()
		{
            DialogResult dlg;
            string path = string.Empty;
            string changedImageName = string.Empty;
            string scanImageName = string.Empty;
            wfeImage wImage = null;
            int pos;
            string fPath = string.Empty;

            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            
            if (imageLst.SelectedIndex >= 0)
            {
                changedImageName = imageLst.SelectedItem.ToString();
                scanImageName = changedImageName;
                pos = imageLst.SelectedItem.ToString().IndexOf("-");

                if (pos > 0)
                {
                    changedImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                    scanImageName = changedImageName;
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), changedImageName, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    changedImageName = wImage.GetIndexedImageName();
                }
            }
            dlg = DialogResult.Yes; //MessageBox.Show(this, "Do you want to copy it from scan folder?", "Copy", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dlg == DialogResult.Yes)
            {
                path = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                fPath = path + "\\" + scanImageName;
            }
            //else
            //{
            //    path = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            //    fPath = path + "\\" + changedImageName;
            //}
            
            if (File.Exists(fPath))
            {
                ShowImage(true, path);
                //File.Copy(fPath, imgFileName);
                ChangeSize(fPath);
            }
            else
            {
                MessageBox.Show("This image is not present in the folder......");
            }
			return 0;
		}
        int AutoCrop()
        {
        	try
        	{
        		if (img.IsValid() == true)
				{
	    			//Auto Crop
	    			img.AutoCrop();
					//Call the save routine
	    			img.SaveFile(imgFileName);
					//Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
				}
				//ChangeSize();
	            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
				fileSize = info.Length;
				fileSize = fileSize / 1024;
	            lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
	            lblImageSize.Text = fileSize.ToString() + " KB";
				UpdateImageSize(fileSize);
        	}
        	catch(Exception ex)
			{
				MessageBox.Show("Error while auto cropping the image","Auto Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
				error = ex.Message;
			}        	
	        return 0;
        }

        int Background()
        {
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Crop
                    img.BackGround();
                    //Call the save routine
                    img.SaveFile(imgFileName);
                    //Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
                }
                ChangeSize();
                System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                fileSize = info.Length;
                fileSize = fileSize / 1024;
                lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImageSize.Text = fileSize.ToString() + " KB";
                UpdateImageSize(fileSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while auto cropping the image", "Auto Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
                error = ex.Message;
            }
            return 0;
        }
		int CropRegister()
		{
			OperationInProgress = ihConstants._CROP;
			return 0;
		}
        int CleanImageRegister()
        {
            OperationInProgress = ihConstants._CLEAN;
            //ChangeSize();
            return 0; 
        }
        int ZoomIn()
        {
        	try
        	{
        		if (img.IsValid() == true)
        		{
		            OperationInProgress = ihConstants._OTHER_OPERATION;
		            keyPressed = keyPressed + 1;
		            zoomHeight =Convert.ToInt32(img.GetBitmap().Height * (1.2));
		            zoomWidth = Convert.ToInt32(img.GetBitmap().Width * (1.2));
		            zoomSize.Height = zoomHeight;
		            zoomSize.Width = zoomWidth;
		            
		            pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) * 1.2);
		            pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) * 1.2);
		            pictureControl.Refresh();
                    ChangeZoomSize();                   
        		}
        	}
        	catch(Exception ex)
        	{
        		MessageBox.Show("Error while auto cropping " + ex.Message,"Auto Crop Error");

        		error = ex.Message;
        	}
        	return 0;
        }
        private void ChangeZoomSize()
        {
            if (!System.IO.File.Exists(imgFileName)) return;
            Image newImage = Image.FromFile(imgFileName);
            double scaleX = (double)pictureControl.Width / (double)newImage.Width;
            double scaleY = (double)pictureControl.Height / (double)newImage.Height;
            double Scale = Math.Min(scaleX, scaleY);
            int w = (int)(newImage.Width * Scale);
            int h = (int)(newImage.Height * Scale);
            //pictureControl.Width = w;
            //pictureControl.Height = h;
            pictureControl.Image = newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
            newImage.Dispose();
        }
        int ZoomOut()
        {
        	try
        	{
	            if (keyPressed > 0)
	            {
		            OperationInProgress = ihConstants._OTHER_OPERATION;
		            keyPressed = keyPressed + 1;
		            zoomHeight =Convert.ToInt32(img.GetBitmap().Height / (1.2));
		            zoomWidth = Convert.ToInt32(img.GetBitmap().Width / (1.2));
		            zoomSize.Height = zoomHeight;
		            zoomSize.Width = zoomWidth;
		            
		            pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) / 1.2);
		            pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) / 1.2);
		            pictureControl.Refresh();
                    ChangeZoomSize();
	            }
        	}
            catch(Exception ex)
        	{
        		MessageBox.Show("Error while auto cropping " + ex.Message,"Auto Crop Error");
        		error = ex.Message;
        	}
            return 0;
        }
        
        int RotateRight()
        {
        	long fileSize;
        	
            OperationInProgress = ihConstants._OTHER_OPERATION;
			
            try
            {
            	if (img.IsValid() == true)
            	{
	    			//Rotate right +90
	    			img.RotateRight();
					//Call the save routine
	    			img.SaveFile(imgFileName);
					//Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();

		            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
					//delinsrtBol = false;
					UpdateImageSize(fileSize);
            	}
            }
            catch(Exception ex)
            {
            	MessageBox.Show("Error while rotate the image" + ex.Message,"Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return 0;
        }
        
        void BoxDtlsMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			
		}
        
        int RotateLeft()
        {
        	long fileSize;
        	
            OperationInProgress = ihConstants._OTHER_OPERATION;
			
            try
            {
            	if (img.IsValid() == true)
            	{
	    			//Rotate right -90
	    			img.RotateLeft();
					//Call the save routine
	    			img.SaveFile(imgFileName);
					//Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
		            //delinsrtBol = false;
		            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
					UpdateImageSize(fileSize);
            	}
            }
            catch(Exception ex)
            {
            	MessageBox.Show("Error while rotate the image" + ex.Message,"Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            //imgQc.Write(imgFileName);

            return 0;

        }		
		void DockPanelPreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
		{
			//MessageBox.Show("Key pressed");
		}
		
		void UpdateImageSize(long prmSize)
		{
			//string photoName;
			wfeImage img;
			//long fileSize;
			
				policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
				imageLst = (ListBox)BoxDtls.Controls["lstImage"];
				
				pImage = new CtrlImage(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey,wBox.ctrlBox.BoxNumber,Convert.ToInt32(policyLst.SelectedItem.ToString()),imageLst.SelectedItem.ToString(),string.Empty);
				img = new wfeImage(sqlCon,pImage);
				img.UpdateImageSize(crd, eSTATES.PAGE_QC,prmSize);
		}
		int SkewRight()
        {
			long fileSize;
            OperationInProgress = ihConstants._OTHER_OPERATION;
			try
			{
            	if (img.IsValid() == true)
            	{
					//Auto Deskew
					img.AutoDeSkew();
					//Call the save routine
					img.SaveFile(imgFileName);
					//Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
					ChangeSize();
					System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
                	lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                	lblImageSize.Text = fileSize.ToString() + " KB";
					UpdateImageSize(fileSize);
            	}
			}
            catch(Exception ex)
            {
            	MessageBox.Show("Error while rotate the image" + ex.Message,"Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return 0;        
		}
		
		int SkewLeft()
        {
            OperationInProgress = ihConstants._OTHER_OPERATION;

//            //rotateAngle = rotateAngle + constRotateAngle;
//            imgQc = objQc.Skew(imgQc,(-skewXAngle),(-skewYAngle));
//            pictureControl.Image = MagickNet.Image.ToBitmap(imgQc);
//            pictureControl.Refresh();
            //imgQc.Write(imgFileName);

            return 0;
        }
		
		int NoiseRemove()
        {
			long fileSize;
            ComboBox noiseVal = (ComboBox)BoxDtls.Controls["cmbDesValue"];

			try
			{
				if (img.IsValid() == true)
				{
					//Auto Deskew
					//img.Despeckle();
                    img.Despeckle(Convert.ToInt32(Convert.ToInt32(noiseVal.Text)));
					//Call the save routine
					img.SaveFile(imgFileName);
					//Show the image back in picture box
                    img.LoadBitmapFromFile(imgFileName);
                    ChangeSize();
	            	System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
            		lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
            		lblImageSize.Text = fileSize.ToString() + " KB";
					UpdateImageSize(fileSize);
				}
			}
            catch(Exception ex)
            {
            	MessageBox.Show("Error while rotating the image" + ex.Message,"Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }			
            return 0;
        }
        void ReadConfigKey()
        {
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            cropKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.CROP_KEY).Remove(1, 1).Trim();
            zoomInKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ZOOM_IN_KEY).Remove(1, 1).Trim();
            zoomOutKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ZOOM_OUT_KEY).Remove(1, 1).Trim();
            autoCropKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.AUTO_CROP_KEY).Remove(1, 1).Trim();
            rotateRKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ROTATE_RIGHT_KEY).Remove(1, 1).Trim();
            rotateLKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.ROTATE_LEFT_KEY).Remove(1, 1).Trim();
            skewRKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.SKEW_RIGHT_KEY).Remove(1, 1).Trim();
            skewLKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.SKEW_LEFT_KEY).Remove(1, 1).Trim();
            noiseRemovalLKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.NOISE_REMOVE_KEY).Remove(1, 1).Trim();
            cleanKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.CLEAN_KEY).Remove(1, 1).Trim();
            deleteKey = config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim();
        }
		void AeFQCKeyUp(object sender, KeyEventArgs e)
		{
            int cropKeyVal = GetKeyVal(cropKey.ToUpper());
            int zoomInKeyVal = GetKeyVal(zoomInKey.ToUpper());
            int zoomOutKeyVal = GetKeyVal(zoomOutKey.ToUpper());
            int autoCropKeyVal = GetKeyVal(autoCropKey.ToUpper());
            int rotateRKeyVal = GetKeyVal(rotateRKey.ToUpper());
            int rotateLKeyVal = GetKeyVal(rotateLKey.ToUpper());
            int skewRKeyVal = GetKeyVal(skewRKey.ToUpper());
            int skewLKeyVal = GetKeyVal(skewLKey.ToUpper());
            int noiseRemovalLKeyVal = GetKeyVal(noiseRemovalLKey.ToUpper());
            int cleanKeyVal = GetKeyVal(cleanKey.ToUpper());
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            if ((int)e.KeyData == cropKeyVal)
		    {
            	OperationInProgress = ihConstants._CROP;
            }
            if ((int)e.KeyData == (zoomInKeyVal+64))
            {
            	ZoomIn();
            }
            if ((int)e.KeyData == (zoomOutKeyVal))
            {
            	ZoomOut();
            }
            
            if ((int)e.KeyData == (autoCropKeyVal))
            {
                AutoCrop();
            }
            
            if ((int)e.KeyData == (rotateRKeyVal))
            {
            	RotateRight();
            }
            if ((int)e.KeyData == (rotateLKeyVal))
            {
            	RotateLeft();
            }
            
            if ((int)e.KeyData == (skewRKeyVal))
            {
            	SkewRight();
            }
            if ((int)e.KeyData == (skewLKeyVal))
            {
            	SkewLeft();
            }
            
            if ((int)e.KeyData == (noiseRemovalLKeyVal))
            {
            	NoiseRemove();
            }
            if ((int)e.KeyData == (cleanKeyVal))
            {
            	OperationInProgress=ihConstants._CLEAN;
            }
            if(e.KeyCode== Keys.Escape)
            {
            	OperationInProgress=ihConstants._NO_OPERATION;
            	Cursor=Cursors.No;
            	Cursor=Cursors.Default;
            	pictureControl.Cursor=Cursors.Default;
            }
            if(e.KeyCode==Keys.F11)
            {
            	BoxDtls.MoveUp();
            }
            if(e.KeyCode==Keys.F12)
            {
            	BoxDtls.MoveDown();
            }
            if (e.KeyCode == Keys.Right)
            {
                BoxDtls.MoveNext();
                ShowImage(false);
                //ChangeSize();
                DisplayDocTypeCount();
            }
            if (e.KeyCode == Keys.Left)
            {
                BoxDtls.MovePrevious();
                ShowImage(false);
                //ChangeSize();
            }
            if (e.KeyCode == Keys.Up)
            {
                //BoxDtls.MovePrevious();
                ShowImage(false);
                ChangeSize();
            }
            if (e.KeyCode == Keys.Down)
            {
                //BoxDtls.MoveNext();
                ShowImage(false);
                ChangeSize();
                //DisplayDocTypeCount();
            }
            //if (e.KeyCode == Keys.F12)
            //{
            //    Background();
            //}
		}
        private int GetKeyVal(string key)
        {
            int cropKeyVal=0;

            foreach (char c in key)
            {
                cropKeyVal = (int)c;
            }
            return cropKeyVal;
        }
		

		void AeFQCKeyPress(object sender, KeyPressEventArgs e)
		{
			//e.
			//MessageBox.Show(e.KeyChar);
		}
		
		void PictureControlMouseDown(object sender, MouseEventArgs e)
		{
            if (OperationInProgress == ihConstants._CROP)
			{
				if(e.Button == MouseButtons.Left)
				{
					
	                cropX = e.X;
	                cropY = e.Y;
	
	                cropPen = new Pen(cropPenColor, cropPenSize);
	                cropPen.DashStyle = DashStyle.Solid;
	                Cursor = Cursors.Cross;
	            	pictureControl.Refresh();
				}
			}
            if(OperationInProgress==ihConstants._CLEAN)
            {
            	if(e.Button == MouseButtons.Left)
				{
	                cropX = e.X;
	                cropY = e.Y;
					
	                //MessageBox.Show("X-" + cropX + "Y-" + cropY);
	                cropPen = new Pen(cropPenColor, cropPenSize);
	                cropPen.DashStyle = DashStyle.Solid;
	                Cursor = Cursors.Cross;
	            	pictureControl.Refresh();            	
            	}
            }
            if(e.Button== MouseButtons.Right)
            {
            	OperationInProgress=ihConstants._NO_OPERATION;
            	Cursor=Cursors.No;
            	Cursor=Cursors.Default;
            	pictureControl.Cursor=Cursors.Default;
            }
		}
		
		
		void PictureControlMouseMove(object sender, MouseEventArgs e)
        {

            if (OperationInProgress == ihConstants._CROP)
			{
				Cursor=Cursors.Cross;
	            if ((pictureControl.Image!=null) && (cropPen!=null))
	            {
		            if(e.Button == MouseButtons.Left)
		            {
		                pictureControl.Refresh();
		                cropWidth = Math.Abs(e.X - cropX); 
		                cropHeight = Math.Abs(e.Y - cropY);
						pictureControl.CreateGraphics().DrawRectangle(cropPen, ((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
		            }
	            }
            }
            if (OperationInProgress == ihConstants._CLEAN)
			{
	            if ((pictureControl.Image!=null))
	            {
		            if(e.Button == MouseButtons.Left)
		            {
		            	Cursor=Cursors.Cross;
		                pictureControl.Refresh();
		                cropWidth = Math.Abs(e.X - cropX); 
		                cropHeight = Math.Abs(e.Y - cropY);
						pictureControl.CreateGraphics().DrawRectangle(cropPen, ((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
		            }
	            }
            }
            if(e.Button== MouseButtons.Right)
            {
                Cursor=Cursors.Default;
                OperationInProgress=ihConstants._NO_OPERATION;
            }
        }

        public void aeFQC_Resize(object sender,EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                panel1.Left = BoxDtls.Right +10 ;
                panel1.Width = (panel2.Left - BoxDtls.Right);
                panel1.Top = panel2.Top;
                panel1.Height = this.ClientSize.Height-20;
                ChangeSize(); 
            }
            //pictureControl.Width = this.ClientSize.Width;
        }
void PictureControlMouseUp(object sender, MouseEventArgs e)
        {
            
            cropWidth = Math.Abs(e.X - cropX); 
            cropHeight = Math.Abs(e.Y - cropY);
            

            Cursor = Cursors.Default;
            if(OperationInProgress==ihConstants._CROP  || (OperationInProgress == ihConstants._CLEAN))
            {
			//Create the rectangle on which to operate
				if((cropWidth > 1) && (e.Button== MouseButtons.Left))
	            {
					//Works both ways
					Rectangle rect  = new Rectangle(((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
	                if (OperationInProgress == ihConstants._CROP)
	            	{
	            		Crop(rect);
	            	}
	                if (OperationInProgress == ihConstants._CLEAN)
	            	{
	            		Clean(rect);
	            	}
	            }
				else
					Cursor=Cursors.Default;
            }
            
            if(e.Button== MouseButtons.Right)
            {
                Cursor=Cursors.Default;
                OperationInProgress=ihConstants._NO_OPERATION;
            }

        }
//		PictureControlMouseUp(object sender, MouseEventArgs e)
//        {
//            
//            cropWidth = e.X - cropX;
//            cropHeight = e.Y - cropY;
//            
//
//            Cursor = Cursors.Default;
//            if(OperationInProgress==ihConstants._CROP  || (OperationInProgress == ihConstants._CLEAN))
//            {
//            //Create the rectangle on which to operate
//                if((cropWidth > 1) && (e.Button== MouseButtons.Left))
//                {
//                    //Works both ways
//                    Rectangle rect  = new Rectangle(((cropX > e.X) ? e.X : cropX), ((cropY > e.Y) ? e.Y : cropY), cropWidth, cropHeight);
//                    if (OperationInProgress == ihConstants._CROP)
//                    {
//                        Crop(rect);
//                    }
//                    if (OperationInProgress == ihConstants._CLEAN)
//                    {
//                        Clean(rect);
//                    }
//                }
//                else
//                    Cursor=Cursors.Default;
//            }
//            
//            if(e.Button== MouseButtons.Right)
//            {
//                Cursor=Cursors.Default;
//                OperationInProgress=ihConstants._NO_OPERATION;
//            }
//
//        }
		void StatusStrip1ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			
		}
        void CreateAllFolders()
        {
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                //policyData = (udtPolicy)policy.LoadValuesFromDB();
                policyPath = GetPolicyPath();
                indexFolderName = policyPath + "\\" + ihConstants._FQC_FOLDER;
                sourceFilePath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                scanFolder = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                qcFolder = policyPath + "\\" + ihConstants._QC_FOLDER;
                if (Directory.Exists(policyPath) == false)
                {
                    Directory.CreateDirectory(policyPath);
                    if (!Directory.Exists(scanFolder))
                    {
                        Directory.CreateDirectory(scanFolder);
                    }
                    if (Directory.Exists(indexFolderName) == false)
                    {
                        Directory.CreateDirectory(indexFolderName);
                    }
                }
                else
                {
                    if (!Directory.Exists(scanFolder))
                    {
                        Directory.CreateDirectory(scanFolder);
                    }
                    if (Directory.Exists(indexFolderName) == false)
                    {
                        Directory.CreateDirectory(indexFolderName);
                    }
                }
            }
        }
        private string GetPolicyPath()
        {
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = wBatch.GetPath(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
            return batchPath + "\\" + wBox.ctrlBox.BoxNumber + "\\" + policyLst.SelectedItem.ToString();
        }
        private string GetPolicyPath(int policyNo)
        {
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = wBatch.GetPath(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
            return batchPath + "\\" + wBox.ctrlBox.BoxNumber + "\\" + policyNo;
        }
		void BoxDtlsPolicyChanged(object sender, EventArgs e)
		{   
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
            DateTime stdt = new DateTime();
            DateTime enddt = new DateTime();
			stdt = DateTime.Now;
			ShowAllException();
            if (tabControl1.SelectedIndex == 1)
            {
                PopulateListView();
            }
            lnkPage1.Visible = false;
            lnkPage2.Visible = false;
            lnkPage3.Visible = false;
            EnableDisbleControls(true);
            try
            {
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                if (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
                {
                    EnableDisbleControls(true);
                    //policyData = (udtPolicy)policy.LoadValuesFromDB();

                    indexFolderName = GetPolicyPath() + "\\" + ihConstants._FQC_FOLDER;
                    sourceFilePath = GetPolicyPath() + "\\" + ihConstants._INDEXING_FOLDER;
                    scanFolder = GetPolicyPath() + "\\" + ihConstants._SCAN_FOLDER;
                    qcFolder = GetPolicyPath() + "\\" + ihConstants._QC_FOLDER;
                    imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                    if (imageLst.Items.Count == 0)
                    {
                        pictureControl.Image = null;
                    }
                    ShowPolicyDetails();
                    
                    //DataSet ds = policy.GetPolicyDetails();
                    Label lblName = (Label)BoxDtls.Controls["lblName"];
                    //if (ds.Tables[0].Rows.Count > 0)
                    lblName.Text = "Name: " + txtName.Text; //ds.Tables[0].Rows[0]["name_of_policyholder"].ToString();              
                    policyLen = policyLst.SelectedItem.ToString().Length;
                    UpdateAllStatus();
                    //DataSet pDs = policy.GetPolicyLog();
                    //if (pDs.Tables.Count > 0)
                    //{
                    //    if (pDs.Tables[0].Rows.Count > 0)
                    //    {
                    //        this.Text = this.Text + " QC User: " + pDs.Tables[0].Rows[0]["qc_user"].ToString();
                    //        this.Text = this.Text + " Index User: " + pDs.Tables[0].Rows[0]["index_user"].ToString();
                    //        this.Text = this.Text + " FQC User: " + pDs.Tables[0].Rows[0]["fqc_user"].ToString();
                    //    }
                    //}
                }
                else
                {
                    Image img = null;
                    pictureControl.Image = img;
                    EnableDisbleControls(false);
                }
                //}
                if (imageLst.Items.Count > 0)
                {
                    policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                    BoxDtls.PopulateDelList(Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    DisplayDocTypeCount();
                    ShowImage(false);
                    //ChangeSize();
                }
                CreateAllFolders();
                if (tabControl2.SelectedIndex == 1)
                {
                    lvwDockTypes.Items[lvwIndex].Selected = true;
                    lvwDockTypes.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while copying all images from Index folder " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes(wBox.ctrlBox.ProjectCode.ToString());
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            enddt = DateTime.Now;
            TimeSpan ts = enddt - stdt;
            //PopulateListView();
            //MessageBox.Show("Total time for execution - " + ts.ToString());
		}
		void ShowPolicyDetails()
		{
			string policyPath;
			DataSet ds=new DataSet();
			
			cmdUpdate.Enabled=false;
			txtName.Enabled=false;
			txtCommDt.Enabled=false;
			txtDOB.Enabled=false;
			
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			imageLst= (ListBox)BoxDtls.Controls["lstImage"];
			ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()));
		    policy = new wfePolicy(sqlCon, ctrlPolicy);
		    //policyData=(udtPolicy)policy.LoadValuesFromDB();
            policyPath = GetPolicyPath(); //policyData.policy_path;
		    ds	= policy.GetPolicyDetails();
		    txtPolicyNumber.Text=ds.Tables[0].Rows[0]["policy_no"].ToString();
		    txtName.Text=ds.Tables[0].Rows[0]["name_of_policyholder"].ToString();
		    txtDOB.Text=ds.Tables[0].Rows[0]["date_of_birth"].ToString();
		    txtCommDt.Text=ds.Tables[0].Rows[0]["date_of_commencement"].ToString();
		    if(txtName.Text.ToString().Trim() == string.Empty) 
		    {
		    	cmdUpdate.Enabled=true;
		    	txtName.Enabled=true;
		    }
		    if (txtDOB.Text.ToString().Trim() == string.Empty)
		    {
		    	txtDOB.Enabled=true;
		    	cmdUpdate.Enabled=true;
		    }
		    if(txtCommDt.Text.ToString().Trim() == string.Empty)
		    {
		    	txtCommDt.Enabled=true;
		    	cmdUpdate.Enabled=true;
		    }
		}
		void BoxDtlsImageChanged(object sender, EventArgs e)
		{
            DateTime stdt = DateTime.Now;
            
            DateTime enddt = DateTime.Now;
            TimeSpan tp = enddt - stdt;
            if(tabControl1.SelectedIndex == 1)
                PopulateListView();
            //MessageBox.Show(tp.Milliseconds.ToString());
		}
        private void PopulateListView()
		{
			DataSet ds = new DataSet();
			string imageName=string.Empty;
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			imageLst = (ListBox)BoxDtls.Controls["lstImage"];
			
			/* Changed by Rahul: 29 May, 09
			 * To avoid Null Reference in imageLst.SelectedItem during swapping items within the list
			 * Occurs when user requests rearrangement of items
			 * */
			//int pos = imageLst.SelectedItem.ToString().IndexOf("-");
			int pos = imageLst.Text.ToString().IndexOf("-");
			if(pos > 0)
			{
				//imageName=imageLst.SelectedItem.Substring(0,pos);
				imageName=imageLst.Text.Substring(0,pos);
		    }
			else
			{
				//imageName = imageLst.SelectedItem.ToString();
				imageName = imageLst.Text.ToString();
			}
			pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),imageName,string.Empty);
		    wfeImage wImage  = new wfeImage(sqlCon, pImage);
		    
		    ds = wImage.GetCustomException(ihConstants._NOT_RESOLVED);
		    if(ds.Tables[0].Rows.Count>0)
		    {
		    	string custExcp=string.Empty;
		    	listView1.Items.Clear();
		    	for(int i=0;i<ds.Tables[0].Rows.Count;i++)
		    	{
		    		ListViewItem lvwItem = listView1.Items.Add(ds.Tables[0].Rows[i]["problem_type"].ToString());
		    		//listView1.Items.Add(ds.Tables[0].Rows[i]["problem_type"].ToString());
		    		lvwItem.SubItems.Add(ds.Tables[0].Rows[i]["Remarks"].ToString());
		    	}
		    	//txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
		    }
		    else
		    {
		    	listView1.Items.Clear();
		    	//txtRemarks.Text = string.Empty;
		    }
		}
		void BoxDtlsNextClicked(object sender, EventArgs e)
		{
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];

            string policyNo = policyLst.SelectedItem.ToString();

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo));
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                EnableDisbleControls(true);
            }
            else
            {
                EnableDisbleControls(false);
            }
            
            //ClearPicBox();
            //BoxDtls.SetCurrentSelection(imageLst.SelectedIndex);
            //ClearSelection();
			BoxDtls.MoveNext();
            ShowImage(false);
            //ChangeSize();
            //DisplayDocTypeCount();
		}

		bool UpdateState(eSTATES prmPageSate,string prmPageName,string prmDocType,string prmIndexImageName)
		{
			double fileSize;
			bool success = false;
			NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
            try
            {
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);

                pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), prmPageName, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                if (wImage.UpdateStatusAndDockType(prmPageSate, prmDocType, prmIndexImageName, crd))
                {
                    if (prmPageSate == eSTATES.PAGE_DELETED)
                    {
                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem), string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);

                        if ((wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (policy.GetPolicyStatus() != (int)eSTATES.POLICY_EXPORTED) && (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
                        {
                            if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                            {
                                policy.UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                            }
                            else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                            {
                                policy.UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                            }
                            else
                            {
                                policy.UpdateStatus(eSTATES.POLICY_FQC, crd);
                            }
                            policy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                        }
                        success = true;
                    }
                }
                else
                {
                    success = false;
                }
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                if ((prmPageSate != eSTATES.PAGE_DELETED) && (insertFlag != true))
                {
                    if ((indexFilePath != null) && (indexingOn == false))
                    {
                        System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);

                        fileSize = info.Length;
                        fileSize = fileSize / 1024;

                        wImage.UpdateImageSize(crd, eSTATES.PAGE_FQC, fileSize);
                    }
                    pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                	wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
//                
                    UpdateAllStatus();
                	pBox = new CtrlBox(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber);
	                wfeBox box = new wfeBox(sqlCon, pBox);

                    NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
                    state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                    state[1] = NovaNet.wfe.eSTATES.POLICY_QC;
                    state[2] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

	                if (wPolicy.GetPolicyCount(state) == 0)
	                {
	                    box.UpdateStatus(eSTATES.BOX_FQC);
	                }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updaing the status " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Policy-" + policyLst.SelectedItem.ToString() + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
                success = false;
            }
            
            insertFlag = false;
            return success;
		}
        bool UpdateState(eSTATES prmPageSate, string prmPageName)
        {
            double fileSize;
            bool success = false;
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();
            try
            {
                pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), prmPageName, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                if(wImage.UpdateStatus(prmPageSate, crd))
                {
                	success = true;
                }
                else
                {
                	success = false;
                }
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                if ((prmPageSate != eSTATES.PAGE_DELETED) && (insertFlag != true))
                {
                    System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);

                    fileSize = info.Length;
                    fileSize = fileSize / 1024;

                    wImage.UpdateImageSize(crd, eSTATES.PAGE_FQC, fileSize);
                }
                pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                if ((wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_EXPORTED) == false) && (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
                {
                    crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                    if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                    {
                        policy.UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                    }
                    else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                    {
                        policy.UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                    }
                    else
                    {
                        policy.UpdateStatus(eSTATES.POLICY_FQC, crd);
                    }
                    ///update into transaction log
                    wPolicy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                }
                pBox = new CtrlBox(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber);
                wfeBox box = new wfeBox(sqlCon, pBox);
                NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[3];
                state[0] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                state[1] = NovaNet.wfe.eSTATES.POLICY_QC;
                state[2] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

                if (wPolicy.GetPolicyCount(state) == 0)
                {
                    box.UpdateStatus(eSTATES.BOX_FQC);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updaing the status " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Policy-" + policyLst.SelectedItem.ToString() + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
                success = false;
            }
            insertFlag = false;
            return success;
        }
		void ShowImage(bool prmOverWrite)
		{
			
			string policyName;
			string changedImageName=string.Empty;
			wfeImage wImage = null;
			string photoImageName = null;
			
			int pos;
			//((ListBox)BoxDtls.Controls["lstPolicy"]).GetItemText();
			try
			{
				policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
				imageLst = (ListBox)BoxDtls.Controls["lstImage"];
				policyName=policyLst.SelectedItem.ToString();
				if(imageLst.SelectedIndex >= 0 )
				{
					changedImageName=imageLst.SelectedItem.ToString();
					
					pos = imageLst.SelectedItem.ToString().IndexOf("-");
					
					if(pos > 0)
					{
						changedImageName=imageLst.SelectedItem.ToString().Substring(0,pos);
						pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),changedImageName,string.Empty);
						wImage  = new wfeImage(sqlCon, pImage);
						changedImageName = wImage.GetIndexedImageName();
					}
					ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()));
				    policy = new wfePolicy(sqlCon, ctrlPolicy);
				    if(policy.GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
					{
						hasPhotoBol = true;
					}
					else
					{
						hasPhotoBol = false;
					}
				    //policyData=(udtPolicy)policy.LoadValuesFromDB();
                    policyPath = GetPolicyPath(); //policyData.policy_path;
				    fileMove=new FileorFolder();
				    string sourcePath=policyPath + "\\" + ihConstants._INDEXING_FOLDER;
				    string destPath=policyPath + "\\" + ihConstants._FQC_FOLDER;
				    sourceFilePath=sourcePath;
				    indexFolderName=destPath;
                    if (Directory.Exists(destPath) == false)
                    {
                        //Directory.CreateDirectory(destPath);
                        //FileorFolder.MoveFiles(sourcePath, destPath);
                        FileorFolder.RenameFolder(sourcePath, destPath);
                    }
				    if(pos <= 0)
				    	{
					   		//fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
				    	}
				    	//prmButtonRescan.Enabled = true;
				    	//prmButtonSkewRight.Enabled = true;
					    imgFileName = destPath + "\\" + changedImageName;
					    if(hasPhotoBol == true)
					    {
					    	if((changedImageName.Substring(policyLen,6) == "_000_A") && (pos <= 0))					    
					    	{
								//Open the source file
								img.LoadBitmapFromFile(imgFileName);				
								//Show the image back in picture box
								//pictureControl.Image = img.GetBitmap();
						    	prmButtonRescan.Enabled = false;
						    	prmButtonSkewRight.Enabled = false;
						    }
						    else if((changedImageName.Substring(policyLen,6) == "_000_A") && (pos > 0))
								{
									photoImageName=imageLst.SelectedItem.ToString().Substring(0,pos);
									pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),photoImageName,string.Empty);
									wImage  = new wfeImage(sqlCon, pImage);
									changedImageName = wImage.GetIndexedImageName();
									if(pos > 0)
				    				{
					   					//fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
				    				}
									imgFileName=destPath + "\\" + changedImageName;
									//Open the source file
									img.LoadBitmapFromFile(imgFileName);				
									//Show the image back in picture box
									//pictureControl.Image = img.GetBitmap();
						    		prmButtonRescan.Enabled = false;
						    		prmButtonSkewRight.Enabled = false;
								}
						    else
						    {
						    	if(pos > 0)
						    	{
						    		photoImageName=imageLst.SelectedItem.ToString().Substring(0,pos);
									pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),photoImageName,string.Empty);
									wImage  = new wfeImage(sqlCon, pImage);
									changedImageName = wImage.GetIndexedImageName();
									if(pos > 0)
				    				{
					   					//fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
				    				}
								imgFileName=destPath + "\\" + changedImageName;						    	
						    	}
								//Open the source file
								img.LoadBitmapFromFile(imgFileName);				
								//Show the image back in picture box
								//pictureControl.Image = img.GetBitmap();
						    }
					    }
					    else
						{
							if(pos > 0)
								{
									photoImageName=imageLst.SelectedItem.ToString().Substring(0,pos);
									//fileMove.MoveFile(sourcePath,destPath,changedImageName,prmOverWrite);
									pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),photoImageName,string.Empty);
									wImage  = new wfeImage(sqlCon, pImage);
									changedImageName = wImage.GetIndexedImageName();
								}
							imgFileName=destPath + "\\" + changedImageName;						    	
							//Open the source file
							img.LoadBitmapFromFile(imgFileName);				
							//Show the image back in picture box
							//pictureControl.Image = img.GetBitmap();
                        }
                        pictureControl.Refresh();
                        System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                        long fileSize = info.Length;
                        fileSize = fileSize / 1024;
                        lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                        lblImageSize.Text = fileSize.ToString() + " KB";
					}
                        ChangeSize();
			}
			
			catch(Exception ex)
			{
				
				MessageBox.Show("Error while showing the image","Image error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
		}
        void ShowImage(bool prmOverWrite,string pFPath)
        {

            string policyName;
            string changedImageName = string.Empty;
            wfeImage wImage = null;
            string photoImageName = null;

            int pos;
            //((ListBox)BoxDtls.Controls["lstPolicy"]).GetItemText();
            try
            {
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                policyName = policyLst.SelectedItem.ToString();
                if (imageLst.SelectedIndex >= 0)
                {
                    changedImageName = imageLst.SelectedItem.ToString();

                    pos = imageLst.SelectedItem.ToString().IndexOf("-");

                    if (pos > 0)
                    {
                        changedImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), changedImageName, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        changedImageName = wImage.GetIndexedImageName();
                    }
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (policy.GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
                    {
                        hasPhotoBol = true;
                    }
                    else
                    {
                        hasPhotoBol = false;
                    }
                    //policyData = (udtPolicy)policy.LoadValuesFromDB();
                    policyPath = GetPolicyPath(); //policyData.policy_path;
                    fileMove = new FileorFolder();
                    string sourcePath = pFPath;
                    string destPath = policyPath + "\\" + ihConstants._FQC_FOLDER;
                    sourceFilePath = sourcePath;
                    indexFolderName = destPath;
                    //if (Directory.Exists(destPath) == false)
                    //{
                    //    Directory.CreateDirectory(destPath);
                    //    FileorFolder.MoveFiles(sourcePath, destPath);
                    //}

                    //if (pos <= 0)
                    //{
                    //    fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                    //}
                    //prmButtonRescan.Enabled = true;
                    //prmButtonSkewRight.Enabled = true;
                    if (Directory.Exists(destPath) == false)
                    {
                        //Directory.CreateDirectory(destPath);
                        //FileorFolder.MoveFiles(sourcePath, destPath);
                        FileorFolder.RenameFolder(sourcePath, destPath);
                    }
                    imgFileName = destPath + "\\" + changedImageName;
                    if (hasPhotoBol == true)
                    {
                        if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos <= 0))
                        {
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                            prmButtonRescan.Enabled = false;
                            prmButtonSkewRight.Enabled = false;
                        }
                        else if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos > 0))
                        {
                            photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), photoImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                            if (pos > 0)
                            {
                                //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                            }
                            imgFileName = destPath + "\\" + changedImageName;
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                            prmButtonRescan.Enabled = false;
                            prmButtonSkewRight.Enabled = false;
                        }
                        else
                        {
                            if (pos > 0)
                            {
                                photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                                pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), photoImageName, string.Empty);
                                wImage = new wfeImage(sqlCon, pImage);
                                changedImageName = wImage.GetIndexedImageName();
                                if (pos > 0)
                                {
                                    //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                                }
                                imgFileName = destPath + "\\" + changedImageName;
                            }
                            //Open the source file
                            img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();
                        }
                    }
                    else
                    {
                        if (pos > 0)
                        {
                            photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), photoImageName, string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                        }
                        imgFileName = destPath + "\\" + changedImageName;
                        //Open the source file
                        img.LoadBitmapFromFile(imgFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                    }
                    pictureControl.Refresh();
                    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                    long fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                    lblImageSize.Text = fileSize.ToString() + " KB";
                }
                ChangeSize();
            }

            catch (Exception ex)
            {

                MessageBox.Show("Error while showing the image", "Image error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
        }
		void BoxDtlsLoaded(object sender, EventArgs e)
		{
			ShowImage(false);
            CreateAllFolders();
			//ChangeSize();
			ShowPolicyDetails();
            UpdateAllStatus();
            //DataSet pDs = policy.GetPolicyLog();
            //if (pDs.Tables.Count > 0)
            //{
            //    if (pDs.Tables[0].Rows.Count > 0)
            //    {
            //        this.Text = this.Text + " QC User: " + pDs.Tables[0].Rows[0]["qc_user"].ToString();
            //        this.Text = this.Text + " Index User: " + pDs.Tables[0].Rows[0]["index_user"].ToString();
            //        this.Text = this.Text + " FQC User: " + pDs.Tables[0].Rows[0]["fqc_user"].ToString();
            //    }
            //}
            //PopulateListView();
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                EnableDisbleControls(true);
                //PopulateListView();
                //policyData = (udtPolicy)policy.LoadValuesFromDB();
                //indexFolderName = policyData.policy_path + "\\" + ihConstants._FQC_FOLDER;
                //sourceFilePath = policyData.policy_path + "\\" + ihConstants._INDEXING_FOLDER;
                //string[] replFiles = Directory.GetFiles(sourceFilePath);
                //for (int i = 0; i < replFiles.Length; i++)
                //{
                //    if (File.Exists(indexFolderName + "\\" + Path.GetFileName(replFiles[i])) == false)
                //        File.Copy(replFiles[i], indexFolderName + "\\" + Path.GetFileName(replFiles[i]), false);
                //}
                DataSet ds = policy.GetPolicyDetails();
                Label lblName = (Label)BoxDtls.Controls["lblName"];
                if (ds.Tables[0].Rows.Count > 0)
                    lblName.Text = "Name: " + ds.Tables[0].Rows[0]["name_of_policyholder"].ToString();
                policyLen = policyLst.SelectedItem.ToString().Length;
                indexFolderName = GetPolicyPath() + "\\" + ihConstants._FQC_FOLDER;
                scanFolder = GetPolicyPath() + "\\" + ihConstants._SCAN_FOLDER;
            }
            else
            {
                EnableDisbleControls(false);
            }
		}
		void PictureControlBackgroundImageChanged(object sender, System.EventArgs e)
		{
		}
        public Image CreateThumbnail(Image pImage, int lnWidth, int lnHeight)
        {

            Bitmap bmp = new Bitmap(lnWidth, lnHeight);
            try
            {

                DateTime stdt = DateTime.Now;

                //create a new Bitmap the size of the new image

                //create a new graphic from the Bitmap
                Graphics graphic = Graphics.FromImage((Image)bmp);
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //draw the newly resized image
                graphic.DrawImage(pImage, 0, 0, lnWidth, lnHeight);
                //dispose and free up the resources
                graphic.Dispose();
                DateTime dt = DateTime.Now;
                TimeSpan tp = dt - stdt;
                //MessageBox.Show(tp.Milliseconds.ToString());
                //return the image

            }
            catch
            {
                return null;
            }
            return (Image)bmp;
        }
	private void ChangeSize()
	{
        try
        {
            if (img.IsValid() == true)
            {
                //panel1.Width = (panel2.Left - BoxDtls.Right);
                //panel1.Height = this.ClientSize.Height - 20;
                if (!System.IO.File.Exists(imgFileName)) return;
                    Image newImage = img.GetBitmap();
            	if(newImage.PixelFormat == PixelFormat.Format1bppIndexed)
            	{
                    pictureControl.Image = null;
	                pictureControl.Width = panel1.Width - 10;
                    pictureControl.Height = tabControl2.Height - 30;
	                double scaleX = (double)pictureControl.Width / (double)newImage.Width;
	                double scaleY = (double)pictureControl.Height / (double)newImage.Height;
	                double Scale = Math.Min(scaleX, scaleY);
	                int w = (int)(newImage.Width * Scale);
	                int h = (int)(newImage.Height * Scale);
	                pictureControl.Width = w;
	                pictureControl.Height = h;
                    //pictureControl.Image = CreateThumbnail(imgFileName, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    pictureControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    newImage.Dispose();
                    //////////////
	                //pictureControl.Refresh();
                    if (Convert.ToDouble(fileSize) < 60)
                    {
                        lblImageSize.ForeColor = Color.Black;
                    }
                    else
                    {
                        lblImageSize.ForeColor = Color.Red;
                    }
            	}
                else
				{
					//pictureControl.Width =panel1.Width - 2;
	                //pictureControl.Height =panel1.Height-2;
                    pictureControl.Width = panel1.Width - 50;
                    pictureControl.Height = tabControl2.Height - 30;
					img.LoadBitmapFromFile(imgFileName);
                	pictureControl.Image=img.GetBitmap();
                	pictureControl.SizeMode= PictureBoxSizeMode.StretchImage;
                	pictureControl.Refresh();
                    if (Convert.ToDouble(fileSize) < 20)
                    {
                        lblImageSize.ForeColor = Color.Black;
                    }
                    else
                    {
                        lblImageSize.ForeColor = Color.Red;
                    }
				}
            }
        }
		catch(Exception ex)
		{
			error=ex.Message;
			MessageBox.Show("Error while cropping the image" + ex.Message,"Crop error");
		}
	}
 
        private bool GetThumbnailImageAbort()
        {
            return false;
        }

        private void ChangeSize(string fName)
        {
            Image imgTot =null;
            try
            {
                if (img.IsValid() == true)
                {
                    pictureControl.Width = panel1.Width - 5;
                    pictureControl.Height = panel1.Height - 5;
                    if (!System.IO.File.Exists(fName)) return;
                    Image newImage;
                    imgAll.LoadBitmapFromFile(fName);
                    if (imgAll.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        imgAll.GetLZW("tmp1.TIF");
                        imgTot = Image.FromFile("tmp1.TIF");
                        newImage = imgTot;
                        //File.Delete("tmp1.TIF");
                    }
                    else
                    {
                        newImage = System.Drawing.Image.FromFile(fName);
                    }
                    double scaleX = (double)pictureControl.Width / (double)newImage.Width;
                    double scaleY = (double)pictureControl.Height / (double)newImage.Height;
                    double Scale = Math.Min(scaleX, scaleY);
                    int w = (int)(newImage.Width * Scale);
                    int h = (int)(newImage.Height * Scale);
                    pictureControl.Width = w-5;
                    pictureControl.Height = h-5;
                    pictureControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    newImage.Dispose();
                    pictureControl.Refresh();
                    if (imgTot != null)
                    {
                        imgTot.Dispose();
                        imgTot = null;
                        if (File.Exists("tmp1.tif"))
                            File.Delete("tmp1.TIF");
                    }
                    if (newImage != null)
                    {
                        newImage.Dispose();
                        newImage = null;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                MessageBox.Show("Error while cropping the image" + ex.Message, "Crop error");
            }
        }
        void UpdateAllStatus()
        {
            string imageName;
            int pos = 0;
            wfeImage wImage = null;
            eSTATES pageState;
            DateTime stDt = DateTime.Now;
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            
            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem),string.Empty,string.Empty);
			wImage  = new wfeImage(sqlCon, pImage);

            if ((wImage.GetImageCount(eSTATES.PAGE_NOT_INDEXED) == false) && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false) && (wImage.GetImageCount(eSTATES.PAGE_RESCANNED_NOT_INDEXED) == false) && (policy.GetPolicyStatus() != (int)eSTATES.POLICY_EXPORTED) && (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD))
            {
                if (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_CHECKED)
                {
                    policy.UpdateStatus(eSTATES.POLICY_CHECKED, crd);
                    pageState = eSTATES.PAGE_CHECKED;
                }
                else if ((policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION) || (policy.GetLICLogStatus() == ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED))
                {
                    policy.UpdateStatus(eSTATES.POLICY_EXCEPTION, crd);
                    pageState = eSTATES.PAGE_EXCEPTION;
                }
                else
                {
                    policy.UpdateStatus(eSTATES.POLICY_FQC, crd);
                    pageState = eSTATES.PAGE_FQC;
                }
                if (crd.role != ihConstants._ADMINISTRATOR_ROLE)
                {
                    policy.UpdateTransactionLog(eSTATES.POLICY_FQC, crd);
                }
                //wfePolicy wPolicy = new wfePolicy(sqlCon);
                //int count =  wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_FQC);
                this.Text = "Final QC";
                this.Text = this.Text;// +"                       Today you have done " + count + " ";
                for (int i = 0; i < imageLst.Items.Count; i++)
                {
                    imageName = imageLst.Items[i].ToString();
                    pos = imageName.IndexOf("-");
                    if (pos > 0)
                    {
                        imageName = imageName.Substring(0, pos);
                    }
                    //To get the index file name from del list selected file name.
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageName, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    wImage.UpdateStatus(pageState,crd);
                }
            }
            DateTime endDt = DateTime.Now;
            TimeSpan tpm = endDt - stDt;
            //MessageBox.Show(tpm.Milliseconds.ToString());
        }


	void BoxDtlsLstDelIamgeInsert(object sender, KeyEventArgs e)
	{
		string delPath=null;
		string sourceFileName=null;
		string qcFilePath=null;
        string sourcePath = null;
        string[] searchFileName;
        try
        {
            if (e.KeyCode == Keys.Insert)
            {

                delPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;
                imageDelLst = (ListBox)BoxDtls.Controls["lstImageDel"];
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                if (imageDelLst.Items.Count > 0)
                {
                    imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    policy = new wfePolicy(sqlCon, ctrlPolicy);

                    //policyData = (udtPolicy)policy.LoadValuesFromDB();
                    policyPath = GetPolicyPath(); //policyData.policy_path;
                    //To get the index file name from del list selected file name.
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageDelLst.SelectedItem.ToString(), string.Empty);
                    wfeImage wImage = new wfeImage(sqlCon, pImage);
                    string changedImageName = wImage.GetIndexedImageName();

                    if (changedImageName == string.Empty)
                    {
                        sourceFileName = sourceFilePath + "\\" + imageDelLst.SelectedItem.ToString();
                        qcFilePath = indexFolderName + "\\" + imageDelLst.SelectedItem.ToString();
                    }
                    else
                    {
                        sourceFileName = sourceFilePath + "\\" + changedImageName;
                        qcFilePath = indexFolderName + "\\" + changedImageName;
                    }
                    int pos = imageDelLst.SelectedItem.ToString().IndexOf(".TIF");
                    searchFileName = Directory.GetFiles(delPath, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                    //For searching deleted file in deleted folder.
                    if (searchFileName.Length <= 0)
                    {
                        //delPath = policyPath + "\\" + ihConstants._QC_FOLDER;
                        //searchFileName = Directory.GetFiles(delPath, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                        //if (searchFileName.Length <= 0)
                        //{
                            delPath = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                            searchFileName = Directory.GetFiles(delPath, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                            if (searchFileName.Length > 0)
                            {
                                sourcePath = searchFileName[0];
                            }
                        //}
                        //else
                        //{
                        //    sourcePath = searchFileName[0];
                        //}
                    }
                    else
                    {
                        sourcePath = searchFileName[0];
                    }
                    string scanFilePath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + imageDelLst.SelectedItem.ToString();
                    if (sourcePath != string.Empty)
                    {
                        if (File.Exists(sourcePath) == true)
                        {
                            File.Move(sourcePath, qcFilePath);
                            if(File.Exists(scanFilePath) == false)
                                File.Copy(qcFilePath,scanFilePath);
                        }
                        //pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),imageDelLst.SelectedItem.ToString(),string.Empty);
                        //		    wfeImage wImage  = new wfeImage(sqlCon, pImage);
                        //		    wImage.UpdateStatus(eSTATES.PAGE_DELETED);
                        insertFlag = true;
                        int dashedPos = changedImageName.IndexOf("-");
                        if (dashedPos > 0)
                        {
                            string docType = changedImageName.Substring(dashedPos+1);
                            int tifPos = docType.IndexOf(".TIF");
                            if (tifPos > 0)
                            {
                                docType = docType.Substring(0, tifPos);
                                UpdateState(eSTATES.PAGE_FQC, imageDelLst.SelectedItem.ToString(), docType, changedImageName);
                            }
                        }
                        else
                        {
                            pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                            wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                            wPolicy.UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
                            UpdateState(eSTATES.PAGE_NOT_INDEXED, imageDelLst.SelectedItem.ToString());
                        }
                        BoxDtls.InsertNotify(imageLst.SelectedIndex);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error while doing the operation.... " + ex.Message);
        }
	}

    private void DisplayDockTypes()
    {
        config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
        char PROPOSALFORM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALFORM_KEY).Remove(1, 1).Trim());
        char PHOTOADDENDUM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHOTOADDENDUM_KEY).Remove(1, 1).Trim());
        char PROPOSALENCLOSERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALENCLOSERS_KEY).Remove(1, 1).Trim());
        char SIGNATUREPAGE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SIGNATUREPAGE_KEY).Remove(1, 1).Trim());
        char MEDICALREPORT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MEDICALREPORT_KEY).Remove(1, 1).Trim());
        char PROPOSALREVIEWSLIP = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALREVIEWSLIP_KEY).Remove(1, 1).Trim());
        char POLICYBOND = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.POLICYBOND_KEY).Remove(1, 1).Trim());
        char NOMINATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.NOMINATION_KEY).Remove(1, 1).Trim());
        char ASSIGNMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ASSIGNMENT_KEY).Remove(1, 1).Trim());
        char ALTERATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ALTERATION_KEY).Remove(1, 1).Trim());
        char REVIVALS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.REVIVALS_KEY).Remove(1, 1).Trim());
        char POLICYLOANS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.POLICYLOANS_KEY).Remove(1, 1).Trim());
        char SURRENDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SURRENDER_KEY).Remove(1, 1).Trim());
        char CLAIMS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CLAIMS_KEY).Remove(1, 1).Trim());
        char CORRESPONDENCE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CORRESPONDENCE_KEY).Remove(1, 1).Trim());
        char OTHERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.OTHERS_KEY).Remove(1, 1).Trim());
        char KYCDOCUMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.KYCDOCUMENT_KEY).Remove(1, 1).Trim());
        char DELETE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
        lvwDockTypes.Items.Clear();
        ListViewItem lvwItem = lvwDockTypes.Items.Add(ihConstants.PROPOSALFORM_FILE);
        lvwItem.SubItems.Add(PROPOSALFORM.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.PHOTOADDENDUM_FILE);
        lvwItem.SubItems.Add(PHOTOADDENDUM.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.PROPOSALENCLOSERS_FILE);
        lvwItem.SubItems.Add(PROPOSALENCLOSERS.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.SIGNATUREPAGE_FILE);
        lvwItem.SubItems.Add(SIGNATUREPAGE.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.MEDICALREPORT_FILE);
        lvwItem.SubItems.Add(MEDICALREPORT.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.PROPOSALREVIEWSLIP_FILE);
        lvwItem.SubItems.Add(PROPOSALREVIEWSLIP.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.POLICYBOND_FILE);
        lvwItem.SubItems.Add(POLICYBOND.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.NOMINATION_FILE);
        lvwItem.SubItems.Add(NOMINATION.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.ASSIGNMENT_FILE);
        lvwItem.SubItems.Add(ASSIGNMENT.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.ALTERATION_FILE);
        lvwItem.SubItems.Add(ALTERATION.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.REVIVALS_FILE);
        lvwItem.SubItems.Add(REVIVALS.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.POLICYLOANS_FILE);
        lvwItem.SubItems.Add(POLICYLOANS.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.SURRENDER_FILE);
        lvwItem.SubItems.Add(SURRENDER.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.CLAIMS_FILE);
        lvwItem.SubItems.Add(CLAIMS.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.CORRESPONDENCE_FILE);
        lvwItem.SubItems.Add(CORRESPONDENCE.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.OTHERS_FILE);
        lvwItem.SubItems.Add(OTHERS.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add("DELETE");
        lvwItem.SubItems.Add(DELETE.ToString());
        lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add(ihConstants.KYCDOCUMENT_FILE);
        lvwItem.SubItems.Add(KYCDOCUMENT.ToString());
        lvwItem.SubItems.Add("0");
    }
	void BoxDtlsLstImageIndex(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
            DialogResult rlst;
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            char PROPOSALFORM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALFORM_KEY).Remove(1, 1).Trim());
            char PHOTOADDENDUM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHOTOADDENDUM_KEY).Remove(1, 1).Trim());
            char PROPOSALENCLOSERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALENCLOSERS_KEY).Remove(1, 1).Trim());
            char SIGNATUREPAGE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SIGNATUREPAGE_KEY).Remove(1, 1).Trim());
            char MEDICALREPORT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.MEDICALREPORT_KEY).Remove(1, 1).Trim());
            char PROPOSALREVIEWSLIP = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALREVIEWSLIP_KEY).Remove(1, 1).Trim());
            char POLICYBOND = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.POLICYBOND_KEY).Remove(1, 1).Trim());
            char NOMINATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.NOMINATION_KEY).Remove(1, 1).Trim());
            char ASSIGNMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ASSIGNMENT_KEY).Remove(1, 1).Trim());
            char ALTERATION = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.ALTERATION_KEY).Remove(1, 1).Trim());
            char REVIVALS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.REVIVALS_KEY).Remove(1, 1).Trim());
            char POLICYLOANS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.POLICYLOANS_KEY).Remove(1, 1).Trim());
            char SURRENDER = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.SURRENDER_KEY).Remove(1, 1).Trim());
            char CLAIMS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CLAIMS_KEY).Remove(1, 1).Trim());
            char CORRESPONDENCE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.CORRESPONDENCE_KEY).Remove(1, 1).Trim());
            char OTHERS = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.OTHERS_KEY).Remove(1, 1).Trim());
            char KYCDOCUMENT = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.KYCDOCUMENT_KEY).Remove(1, 1).Trim());
            char DELETE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
			int pos=0;
			int index;
			bool bolKey=false;
           	string selImageName=null;
           	string originalFileName=null;
           	string indexFileName=null;
           	string docType=null;
            bool indexedBol = false;
            bool sigBol = false;
           
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
           
            string policyNo = policyLst.SelectedItem.ToString();
            string origDoctype = string.Empty;
            int tifPos = imageLst.SelectedItem.ToString().ToString().IndexOf("-") + 1;
            if (tifPos > 0)
            {
                origDoctype = imageLst.SelectedItem.ToString().Substring(tifPos);
            }
			if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == DELETE)
		    {
           		ImageDelete();
           		return;
            }
            if (origDoctype == "Signature_page")
            {
                MessageBox.Show("You can not change signature page. You can only delete it...");
                return;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PROPOSALFORM)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PROPOSALFORM_FILE);
           		docType=ihConstants.PROPOSALFORM_FILE;
           		bolKey=true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PHOTOADDENDUM)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PHOTOADDENDUM_FILE);
                docType = ihConstants.PHOTOADDENDUM_FILE;
                bolKey = true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PROPOSALENCLOSERS)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PROPOSALENCLOSERS_FILE);
           		docType=ihConstants.PROPOSALENCLOSERS_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == SIGNATUREPAGE)
		    {
                if (origDoctype != ihConstants.SIGNATUREPAGE_FILE)
                {
                    indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.SIGNATUREPAGE_FILE);
                    docType = ihConstants.PROPOSALFORM_FILE;
                    bolKey = true;
                    sigBol = true;
                }
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == MEDICALREPORT)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.MEDICALREPORT_FILE);
           		docType=ihConstants.MEDICALREPORT_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PROPOSALREVIEWSLIP)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PROPOSALREVIEWSLIP_FILE);
           		docType=ihConstants.PROPOSALREVIEWSLIP_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == POLICYBOND)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.POLICYBOND_FILE);
           		docType=ihConstants.POLICYBOND_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == NOMINATION)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.NOMINATION_FILE);
           		docType=ihConstants.NOMINATION_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ASSIGNMENT)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ASSIGNMENT_FILE);
           		docType=ihConstants.ASSIGNMENT_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ALTERATION)
		    {
           		indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(),ihConstants.ALTERATION_FILE);
           		docType=ihConstants.ALTERATION_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == REVIVALS)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.REVIVALS_FILE);
           		docType=ihConstants.REVIVALS_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == POLICYLOANS)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.POLICYLOANS_FILE);
           		docType=ihConstants.POLICYLOANS_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == SURRENDER)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.SURRENDER_FILE);
           		docType=ihConstants.SURRENDER_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == CLAIMS)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.CLAIMS_FILE);
           		docType=ihConstants.CLAIMS_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == CORRESPONDENCE)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.CORRESPONDENCE_FILE);
           		docType=ihConstants.CORRESPONDENCE_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == OTHERS)
		    {
                indexedBol=ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.OTHERS_FILE);
           		docType=ihConstants.OTHERS_FILE;
           		bolKey=true;
            }
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == KYCDOCUMENT)
            {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.KYCDOCUMENT_FILE);
                docType = ihConstants.KYCDOCUMENT_FILE;
                bolKey = true;
            }  	
         
           	if(bolKey == true)
           	{
           		pos = imageLst.SelectedItem.ToString().IndexOf("-");
           		if(pos <= 0)
	           	{
	           		selImageName = imageLst.SelectedItem.ToString() + "-";
	           		pos = imageLst.SelectedItem.ToString().IndexOf(".TIF") + 5;
	           		selImageName=selImageName.Insert(pos,docType);
	           	}
	           	else
	           	{
	           		selImageName = imageLst.SelectedItem.ToString().Substring(0,(pos+1));
	           		selImageName = selImageName + docType;
	           	}
                if (indexedBol == true)
                {
                    originalFileName = GetFileName(imageLst.SelectedItem.ToString(), docType);
                    pos = originalFileName.ToString().IndexOf(".TIF");
                    indexFileName = selImageName.Substring(0, pos) + "-" + docType + ".TIF";
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo));
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    indexingOn = true;
                    if ((policy.GetPolicyStatus() == (int)eSTATES.POLICY_FQC) || (policy.GetPolicyStatus() == (int)eSTATES.POLICY_INDEXED) || (policy.GetPolicyStatus() == (int)eSTATES.POLICY_NOT_INDEXED) || (policy.GetPolicyStatus() == (int)eSTATES.POLICY_EXCEPTION) || (policy.GetPolicyStatus() == (int)eSTATES.POLICY_CHECKED))
                    {
                        UpdateState(eSTATES.PAGE_FQC, originalFileName, docType, indexFileName);
                    }
                    else if (policy.GetPolicyStatus() == (int)eSTATES.POLICY_ON_HOLD)
                    {
                        UpdateState(eSTATES.PAGE_ON_HOLD, originalFileName, docType, indexFileName);
                    }
                    else if (policy.GetPolicyStatus() == (int)eSTATES.POLICY_EXPORTED)
                    {
                        UpdateState(eSTATES.PAGE_EXPORTED, originalFileName, docType, indexFileName);
                    }
                    indexingOn = false;
                }
                index = imageLst.SelectedIndex;
                //imageLst.Text.Replace(imageLst.SelectedItem.ToString(), selImageName);
                imageLst.Items[index] = selImageName;
                imageLst.Refresh();
                //imageLst.Items.RemoveAt(index);
                //imageLst.Text = selImageName;
                //selImageName = selImageName + " - " + docType;
                //imageLst.Items.Insert(index,selImageName );


                if ((index + 1) != imageLst.Items.Count)
                {
                    if (docType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        imageLst.SelectedIndex = index + 1;
                        ShowIndexedImage();
                    }
                    if (sigBol == true)
                    {
                        if ((index + 2) != imageLst.Items.Count)
                        {
                            imageLst.SelectedIndex = index + 2;
                            ShowIndexedImage();
                        }
                        else
                        {
                            if ((policyLst.SelectedIndex + 1) != policyLst.Items.Count)
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            }
                        }
                    }
                }
                else
                {
                    if ((policyLst.SelectedIndex + 1) != (policyLst.Items.Count))
                    {
                        policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                    }
                }
           	}
           	DisplayDocTypeCount();
		}
        void ShowIndexedImage()
        {
            string imageName;
            int pos = 0;
            string docType = string.Empty;

            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            try
            {
                imageName = imageLst.SelectedItem.ToString();
                pos = imageName.IndexOf("-");
                if (pos > 0)
                {
                    docType = imageName.Substring(pos);
                    pos = imageName.IndexOf(".TIF");
                    imageName = imageName.Substring(0, pos) + docType + ".TIF";
                    imgFileName = indexFolderName + "\\" + imageName;
                }
                else
                {
                    imgFileName = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
                }

                //Open the source file
                img.LoadBitmapFromFile(imgFileName);
                //Show the image back in picture box
                //pictureControl.Image = img.GetBitmap();
                ChangeSize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while showing image..." + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
        }
        private void DisplayDocTypeCount()
        {

            int pos;

            DisplayDockTypes();
            for (int i = 0; i < imageLst.Items.Count; i++)
            {
                pos = imageLst.Items[i].ToString().IndexOf("-");
                docType = imageLst.Items[i].ToString().Substring(pos + 1);
                if (docType == ihConstants.PROPOSALFORM_FILE)
                {
                    lvwDockTypes.Items[0].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[0].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.PHOTOADDENDUM_FILE)
                {
                    lvwDockTypes.Items[1].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[1].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.PROPOSALENCLOSERS_FILE)
                {
                    lvwDockTypes.Items[2].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[2].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.SIGNATUREPAGE_FILE)
                {
                    lvwDockTypes.Items[3].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[3].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.MEDICALREPORT_FILE)
                {
                    lvwDockTypes.Items[4].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[4].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.PROPOSALREVIEWSLIP_FILE)
                {
                    lvwDockTypes.Items[5].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[5].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.POLICYBOND_FILE)
                {
                    lvwDockTypes.Items[6].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[6].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.NOMINATION_FILE)
                {
                    lvwDockTypes.Items[7].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[7].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.ASSIGNMENT_FILE)
                {
                    lvwDockTypes.Items[8].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[8].SubItems[2].Text) + 1));
                } if (docType == ihConstants.ALTERATION_FILE)
                {
                    lvwDockTypes.Items[9].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[9].SubItems[2].Text) + 1));
                } if (docType == ihConstants.REVIVALS_FILE)
                {
                    lvwDockTypes.Items[10].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[10].SubItems[2].Text) + 1));
                } if (docType == ihConstants.POLICYLOANS_FILE)
                {
                    lvwDockTypes.Items[11].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[11].SubItems[2].Text) + 1));
                } if (docType == ihConstants.SURRENDER_FILE)
                {
                    lvwDockTypes.Items[12].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[12].SubItems[2].Text) + 1));
                } if (docType == ihConstants.CLAIMS_FILE)
                {
                    lvwDockTypes.Items[13].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[13].SubItems[2].Text) + 1));
                } if (docType == ihConstants.CORRESPONDENCE_FILE)
                {
                    lvwDockTypes.Items[14].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[14].SubItems[2].Text) + 1));
                } if (docType == ihConstants.OTHERS_FILE)
                {
                    lvwDockTypes.Items[15].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[15].SubItems[2].Text) + 1));
                }
                if (docType == ihConstants.KYCDOCUMENT_FILE)
                {
                    lvwDockTypes.Items[17].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[17].SubItems[2].Text) + 1));
                }
            }
            //imageDelLst = (ListBox)BoxDtls.Controls["lstImageDel"];
            lvwDockTypes.Items[16].SubItems[2].Text =Convert.ToString(imageDelLst.Items.Count);
        }
	
	void ShowAllException()
	{
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			DataSet expDs = new DataSet();
			pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()));
		    wfePolicy policy = new wfePolicy(sqlCon, pPolicy);

            txtExceptionType.Text = string.Empty;
			expDs = policy.GetAllException();
			if(expDs.Tables[0].Rows.Count > 0)
			{
				
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["missing_img_exp"].ToString()) == 1)
				{
					txtExceptionType.Text = "Missing image";
				}
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["crop_clean_exp"].ToString()) == 1)
				{
					txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Crop clean exception";
				}
				
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["poor_scan_exp"].ToString()) == 1)
				{
					txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Poor quality of scan";
				}
				
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["wrong_indexing_exp"].ToString()) == 1)
				{
					txtExceptionType.Text =  txtExceptionType.Text + "\r\n" + "Wrong indexing";
				}
				
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["linked_policy_exp"].ToString()) == 1)
				{
					txtExceptionType.Text =  txtExceptionType.Text + "\r\n" + "Linked policy exception";
				}
				
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["decision_misd_exp"].ToString()) == 1)
				{
					txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Decision misd Exception";
				}
				
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["extra_page_exp"].ToString()) == 1)
				{
					txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Extra page exception";
				}
				
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["rearrange_exp"].ToString()) == 1)
				{
					txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Rearrange exception";
				}
				
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["other_exp"].ToString()) == 1)
				{
					txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Other exception";
				}
				
				if(Convert.ToInt32(expDs.Tables[0].Rows[0]["move_to_respective_policy_exp"].ToString()) == 1)
				{
					txtExceptionType.Text = txtExceptionType.Text + "\r\n" + "Move to respective policy exception";
				}
				
				txtComments.Text = expDs.Tables[0].Rows[0]["comments"].ToString();
				//MessageBox.Show ("This policy has some exception, details are given at right side","Policy Exception",MessageBoxButtons.OK);
			}
			else
			{
				txtExceptionType.Text = string.Empty;	
				txtComments.Text = string.Empty;
			}
	}
	
	private bool ChangeAndMoveFile(string prmSourceFileName,string prmDocType)
		{
            string indexFileName=null;
			string sourceFile=null;
            string sourcePath = null;
			int pos;
            string fileCount = string.Empty;
            string signatureFqcFile = string.Empty;
            string signatureIndexFile = string.Empty;
            CtrlImage ctrlImg;
            
            wfeImage wimg;
            string propIndexFileName = string.Empty;
            string origDoctype = string.Empty;

            try
            {
                pos = prmSourceFileName.ToString().IndexOf("-");
                int tifPos = prmSourceFileName.ToString().IndexOf("-") + 1;
                if (tifPos > 0)
                {
                    origDoctype = prmSourceFileName.Substring(tifPos);
                }
                if (origDoctype != prmDocType)
                {
                    if (pos <= 0)
                    {
                        if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                        {
                            pos = prmSourceFileName.ToString().IndexOf("TIF") - 1;
                            indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos) + "-" + prmDocType + ".TIF";
                            sourceFile = indexFolderName + "\\" + prmSourceFileName;
                            indexFilePath = indexFileName;
                            //Indexing in index folder also
                            string fileName = imageLst.SelectedItem.ToString();
                            string wildCardFileName = fileName;
                            string[] searchFileName = Directory.GetFiles(indexFolderName, wildCardFileName);
                            //For the file in index folder
                            if (searchFileName.Length >= 0)
                                sourcePath = searchFileName[0];
                        }
                        else
                        {
                            fileCount = imageLst.SelectedItem.ToString().Substring(policyLen, 4);
                            pos = prmSourceFileName.ToString().IndexOf("TIF") - 1;
                            indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos) + "-" + ihConstants.PROPOSALFORM_FILE + ".TIF";
                            signatureFqcFile = indexFolderName + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";

                            signatureIndexFile = sourceFilePath + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";
                            propIndexFileName = sourceFilePath + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.PROPOSALFORM_FILE + ".TIF";

                            sourceFile = indexFolderName + "\\" + prmSourceFileName;
                            indexFilePath = indexFileName;
                            //Indexing in index folder also
                            string fileName = imageLst.SelectedItem.ToString();
                            string wildCardFileName = fileName;
                            string[] searchFileName = Directory.GetFiles(indexFolderName, wildCardFileName);
                            //For the file in index folder
                            if (searchFileName.Length >= 0)
                            {
                                sourcePath = searchFileName[0];
                            }

                        }
                    }
                    else
                    {
                        if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                        {
                            prmSourceFileName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), prmSourceFileName, string.Empty);
                            wfeImage wImage = new wfeImage(sqlCon, pImage);
                            prmSourceFileName = wImage.GetIndexedImageName();
                            sourceFile = indexFolderName + "\\" + prmSourceFileName;
                            indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos - 3) + prmDocType + ".TIF";
                            indexFilePath = indexFileName;

                            //Indexing in index folder also
                            string fileName = imageLst.SelectedItem.ToString();
                            string wildCardFileName = fileName.Substring(0, tifPos - 5) + "*" + ".TIF";
                            string[] searchFileName = Directory.GetFiles(indexFolderName, wildCardFileName);
                            //For the file in index folder
                            if (searchFileName.Length >= 0)
                                sourcePath = searchFileName[0];
                        }
                        else
                        {
                            fileCount = imageLst.SelectedItem.ToString().Substring(policyLen, 4);
                            pos = prmSourceFileName.ToString().IndexOf("TIF") - 1;
                            indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos) + "-" + ihConstants.PROPOSALFORM_FILE + ".TIF";
                            signatureFqcFile = indexFolderName + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";

                            signatureIndexFile = sourceFilePath + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";
                            propIndexFileName = sourceFilePath + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_A-" + ihConstants.PROPOSALFORM_FILE + ".TIF";

                            prmSourceFileName = imageLst.SelectedItem.ToString().Substring(0, tifPos-1);
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), prmSourceFileName, string.Empty);
                            wfeImage wImage = new wfeImage(sqlCon, pImage);
                            prmSourceFileName = wImage.GetIndexedImageName();

                            sourceFile = indexFolderName + "\\" + prmSourceFileName;
                            indexFilePath = indexFileName;
                            //Indexing in index folder also
                            string fileName = imageLst.SelectedItem.ToString();
                            string wildCardFileName = fileName.Substring(0, tifPos - 5) + "*" + ".TIF";
                            string[] searchFileName = Directory.GetFiles(indexFolderName, wildCardFileName);
                            //For the file in index folder
                            if (searchFileName.Length >= 0)
                            {
                                sourcePath = searchFileName[0];
                            }
                        }
                    }
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        if (File.Exists(indexFileName) == false)
                        {
                            File.Copy(sourceFile, indexFileName, true);
                            //if (prmSourceFileName.ToString().IndexOf("-") > 0)
                            //{
                            //    File.Copy(sourcePath, sourceFilePath + "\\" + prmSourceFileName.Substring(0, pos - 3) + prmDocType + ".TIF", true);
                            //}
                            //else
                            //{
                            //    File.Copy(sourcePath, sourceFilePath + "\\" + prmSourceFileName.Substring(0, pos) + "-" + prmDocType + ".TIF", true);
                            //}
                            //File.Delete(sourceFile);
                            System.IO.FileInfo info = new System.IO.FileInfo(indexFileName);
                            long fileSize = info.Length;
                            fileSize = fileSize / 1024;
                            UpdateImageSize(fileSize);
                            File.Delete(sourcePath);
                            img.LoadBitmapFromFile(indexFileName);
                            int i = imageLst.SelectedIndex;
                            //BoxDtls.MoveNext();
                        }
                    }
                    else
                    {
                            if (File.Exists(indexFileName) == false)
                            {
                                File.Copy(sourceFile, indexFileName);
                                //File.Copy(sourceFile, propIndexFileName);
                                File.Delete(sourceFile);
                            }
                            if (File.Exists(signatureFqcFile) == false)
                            {
                                File.Copy(indexFileName, signatureFqcFile);
                                //File.Copy(signatureFqcFile, signatureIndexFile);
                                //File.Delete(sourcePath);
                                ctrlImg = new CtrlImage(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(policyLst.SelectedItem), policyLst.SelectedItem.ToString() + fileCount + "_B" + ".TIF", string.Empty);
                                crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                                wimg = new wfeImage(sqlCon, ctrlImg);
                                int imgCount = wimg.GetImageCount();
                                imgCount++;
                                System.IO.FileInfo info = new System.IO.FileInfo(signatureFqcFile);
                                double fileSize = info.Length;
                                fileSize = fileSize / 1024;

                                wimg.Save(crd, eSTATES.PAGE_INDEXED,ihConstants.SIGNATUREPAGE_FILE, policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF", fileSize, imgCount);

                                imageLst.Items.Insert(imageLst.SelectedIndex + 1, policyLst.SelectedItem.ToString() + fileCount + "_B.TIF-" + ihConstants.SIGNATUREPAGE_FILE);
                            }
                            
                            
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while changing the index name " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + " ,File name-" + indexFileName + "Doc type-" + docType + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
                return false;
            }
			
		}

        void EnableDisbleControls(bool prmControl)
        {
            prmButtonCrop.Enabled = prmControl;
            prmButtonAutoCrp.Enabled = prmControl;
            prmButtonRotateRight.Enabled = prmControl;
            prmButtonRotateLeft.Enabled = prmControl;
            prmButtonZoomIn.Enabled = prmControl;
            prmButtonZoomOut.Enabled = prmControl;
            prmButtonSkewRight.Enabled = prmControl;
            //prmButtonSkewLeft.Enabled = prmControl;
            prmButtonNoiseRemove.Enabled = prmControl;
            prmButtonCleanImg.Enabled = prmControl;
            prmButtonCopyImage.Enabled = prmControl;
            prmButtonDelImage.Enabled = prmControl;
            prmButtonRescan.Enabled = prmControl;
            prmButtonScan.Enabled = prmControl;
            //prmButtonCopyTo.Enabled = prmControl;
            prmButtonMoveTo.Enabled = prmControl;
            prmButtonCopyImageTo.Enabled = prmControl;
        }
        void BoxDtls_cmbChanged(object sender, EventArgs e)
        {
            cmbBox = (ComboBox)BoxDtls.Controls["cmbBox"];
            CtrlBox cBox = new CtrlBox(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey,Convert.ToInt32(cmbBox.Text));
            wfeBox fBox = new wfeBox(sqlCon, cBox);
            wBox = fBox;
            BoxDtls.RefreshNotify(fBox);
            prmButtonCopyTo.Enabled = false;
            PopulatePolicyCombo();
        }
        void BoxDtls_LstDelImgClick(object sender, EventArgs e)
        {
            string delFileName;
            string[] searchFileName;
            try
            {
                delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
                if (delImgList.Items.Count > 0)
                {
                    EnableDisbleControls(false);


                    policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                    int pos = imageDelLst.SelectedItem.ToString().IndexOf(".TIF");
                    //ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    //policy = new wfePolicy(sqlCon, ctrlPolicy);

                    //policyData = (udtPolicy)policy.LoadValuesFromDB();
                    //policyPath = policyData.policy_path;
                    delFileName = delFileName = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;
                    searchFileName = Directory.GetFiles(delFileName, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                    //For searching deleted file in deleted folder.
                    if (searchFileName.Length <= 0)
                    {
                        //delFileName = policyPath + "\\" + ihConstants._QC_FOLDER;
                        //searchFileName = Directory.GetFiles(delFileName, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                        //if (searchFileName.Length <= 0)
                        //{
                        
                        searchFileName = Directory.GetFiles(delFileName, imageDelLst.SelectedItem.ToString().Substring(0, pos) + "*.TIF", SearchOption.AllDirectories);
                        if (searchFileName.Length > 0)
                        {
                            delFileName = searchFileName[0];
                        }
                        //}
                        //else
                        //{
                        //    delFileName = searchFileName[0];
                        //}
                    }
                    else
                    {
                        delFileName = searchFileName[0];
                    }

                    //searchFileName = Directory.GetFiles(delFileName, delImgList.SelectedItem.ToString());
                    //For searching deleted file in deleted folder.
                    if (searchFileName.Length >= 0)
                    {
                        delFileName = searchFileName[0];

                        img.LoadBitmapFromFile(delFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                    }
                    ChangeSize(delFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading from deleted folder in scan....." + ex.Message);
            }
        }
    
	private string GetFileName(string prmSelFileName,string prmDocType)
	{
		string modFileName=null;
		int pos;
		
		pos = prmSelFileName.IndexOf("-");
		if(pos <= 0)
		{
			modFileName = prmSelFileName;
		}
		else
		{
			modFileName = prmSelFileName.Substring(0,pos);
		}
		return modFileName;
	}
	void CmdUpdateClick(object sender, System.EventArgs e)
		{
			pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()));
		    wfePolicy wPolicy=new wfePolicy(sqlCon,pPolicy);
            if ((txtName.Text.Trim() != string.Empty) && (txtDOB.Text.Trim() != string.Empty) && (txtCommDt.Text.Trim() != string.Empty))
            {
                wPolicy.UpdatePolicyDetails(txtName.Text.PadRight(30,' '), txtDOB.Text, txtCommDt.Text);
                ShowPolicyDetails();
            }
		}
	void CmdOkClick(object sender, System.EventArgs e)
		{
			NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
			
			imageLst = (ListBox)BoxDtls.Controls["lstImage"];
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			if(imageLst.Items.Count > 0)
			{
                pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                if (wPolicy.GetPolicyStatus() == (int)eSTATES.POLICY_EXCEPTION)
                {
                    if (wPolicy.QaExceptionStatus(ihConstants._POLICY_EXCEPTION_RECTIFIED, ihConstants._LIC_QA_POLICY_EXCEPTION_SLOVED) == true)
                    {
                        MessageBox.Show("Successfully updated....");
                    }
                }
                ShowAllException();
			}
		}
        void CmdFetchClick(object sender, System.EventArgs e)
        {
            int i;
            
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[7];
            
            state[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[1] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            state[2] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[3] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
            state[4] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
            state[5] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
            state[6] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
            if (int.TryParse(textBox1.Text,out i))
            {
                if ((cmbDocType.Text != string.Empty) && (comboBox1.Text != string.Empty) && (textBox1.Text != string.Empty))
                {
                    BoxDtls.RefreshNotify(cmbDocType.Text, comboBox1.Text, Convert.ToInt32(textBox1.Text),state);
                    policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];

                    if (policyLst.Items.Count <= 0)
                    {
                        MessageBox.Show("No policy found for this search....");
                        rdoAll.Checked = false;
                        rdoAll.Checked = true;
                        prmButtonCopyTo.Enabled = false;
                        prmButtonCopyProposalForm.Enabled = false;
                        prmButtonCopyProposalReviewSlip.Enabled = false;
                    }
                    else
                    {
                        if ((cmbDocType.Text == "Policy_bond") || (cmbDocType.Text == "Proposal_form") || (cmbDocType.Text == "Proposal_review_slip"))
                        {
                            if (cmbDocType.Text == "Policy_bond")
                            {
                                if ((textBox1.Text.Trim() == "0") || (textBox1.Text.Trim() == "1"))
                                {
                                    prmButtonCopyTo.Enabled = true;
                                }
                                else
                                {
                                    prmButtonCopyTo.Enabled = false;
                                }
                            }
                            if (cmbDocType.Text == "Proposal_form")
                            {
                                if ((textBox1.Text.Trim() == "0"))
                                {
                                    prmButtonCopyProposalForm.Enabled = true;
                                }
                                else
                                {
                                    prmButtonCopyProposalForm.Enabled = false;
                                }
                            }
                            if (cmbDocType.Text == "Proposal_review_slip")
                            {
                                if ((textBox1.Text.Trim() == "0"))
                                {
                                    prmButtonCopyProposalReviewSlip.Enabled = true;
                                }
                                else
                                {
                                    prmButtonCopyProposalReviewSlip.Enabled = false;
                                }
                            }
                        }
                        else
                        {
                            prmButtonCopyTo.Enabled = false;
                        }
                        rdoAll.Checked = false;
                        rdoLIC.Checked = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Give a valid number in the count field.....");
            }
        }
        private void EndingScan()
        {
            if (msgfilter)
            {
                Application.RemoveMessageFilter(this);
                msgfilter = false;
                this.Enabled = true;
                this.Activate();
            }
        }
        public void GetImageNew(System.IntPtr prmHBmp)
        {
            char leftPad = Convert.ToChar("0");
            
			string sourcePath = null;
            string desPath = null;
            string qcPath = null;
            string scanPath = null;
            int imgCount=0;
            int pageCount = 0;
            string flName;
            string scanDate;
            bool success = false;
            try
            {
            	policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            	pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),string.Empty,string.Empty);
				wfeImage wImage  = new wfeImage(sqlCon, pImage);
				
				imgCount=wImage.GetImageCount();
                pageCount = wImage.GetMaxPageCount();
				
				if(imgCount >= 0)
    			{
					imgCount = imgCount + 1;
                    pageCount = pageCount + 1;
					flName=policyLst.SelectedItem.ToString() + "_" + pageCount.ToString().PadLeft(3,'0') + "_A.TIF";
		        	//Build string for supposed to be file in indexing folder
		        	//sourcePath = sourceFilePath + "\\" + flName;
		            //Build string for file in FQC folder
		            desPath = indexFolderName + "\\" + flName;
                    //Build string for file in QC folder
                    scanPath = scanFolder + "\\" + flName;
                    //Build string for file in scan folder
                    //qcPath = qcFolder + "\\" + flName;
		        	//ino = gdScanImg.CreateGdPictureImageFromDIB(prmHBmp);
                    pPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                    scanDate = dbcon.GetCurrenctDTTM(1, sqlCon);
                    wPolicy.UpdateScanDetails(scanDate, ihConstants.SCAN_SUCCESS_FLAG);

					img.LoadBitmapFromDIB(prmHBmp);
                    if (Directory.Exists(indexFolderName))
                    {
                    	if(img.SaveFile(desPath) == IGRStatus.Success)
                    	{
                    		if (Directory.Exists(scanFolder))
		                    {
		                    	if(img.SaveFile(scanPath) == IGRStatus.Success)
		                    	{
		                    		success = true;
		                    	}
		                    }
                    	}
                    }
                    //if (Directory.Exists(sourceFilePath))
                    //{
                    //    img.SaveFile(sourcePath);
                    //}
                    //if (Directory.Exists(qcFolder))
                    //{
                    //    img.SaveFile(qcPath);
                    //}
                    if(success == true)
                    {
						//pictureControl.Image = img.GetBitmap();
	                    ChangeSize();
			        	SetListboxValue(flName,imgCount);
	                    if (prmHBmp != IntPtr.Zero)
	                    {
	                        Marshal.FreeHGlobal(prmHBmp);
	                        prmHBmp = IntPtr.Zero;
	                    }
                    }
                    else
                    {
                    	twScan.CloseSrc();
                    	MessageBox.Show("Error while saving....");
		                if (prmHBmp != IntPtr.Zero)
		                {
		                    Marshal.FreeHGlobal(prmHBmp);
		                    prmHBmp = IntPtr.Zero;
		                }
                    }
				}
            }
            catch (Exception ex)
            {
            	twScan.CloseSrc();
            	MessageBox.Show("Error - " + ex.Message);
                //exMailLog.Log(ex);
                if (prmHBmp != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(prmHBmp);
                    prmHBmp = IntPtr.Zero;
                }
            }
        }
        public void GetImage(System.IntPtr prmHBmp)
        {
            char leftPad = Convert.ToChar("0");
			string sourcePath = null;
            string desPath = null;
            int pos = 0;
            string fileName;
            try
            {
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if(imageLst.SelectedItem.ToString() != string.Empty)
            {
            	//Build string for supposed to be file in indexing folder
                sourcePath = sourceFilePath + "\\" + imageLst.SelectedItem.ToString();
                //Build string for file in FQC folder
                desPath = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
            }
			//Wild card search: To save in the indexing folder
			fileName = imageLst.SelectedItem.ToString();
			pos = fileName.IndexOf("-");
            if(pos > 0)
			{
				string originalImage = fileName.Substring(0,pos-4) + "*" + ".TIF";
                //string[] searchFileName = Directory.GetFiles(sourceFilePath, originalImage);
                ////For the file in index folder
                //if (searchFileName.Length >= 0)
                //    sourcePath = searchFileName[0];
                //For the file in FQC folder
                string[] searchFileName = Directory.GetFiles(indexFolderName, originalImage);
                if (searchFileName.Length >= 0)
                	desPath = searchFileName[0];
			}
			//End: Wild card search
				img.LoadBitmapFromDIB(prmHBmp);
				img.SaveFile(desPath);
				//img.SaveFile(sourcePath);
                ChangeSize(desPath);
                if (prmHBmp != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(prmHBmp);
                    prmHBmp = IntPtr.Zero;
                }
            }
            catch (Exception ex)
            {
            	twScan.CloseSrc();
            	MessageBox.Show("Error - " + ex.Message);
                exMailLog.Log(ex);
            }
        }
		void SetListboxValue(string prmIamgeName,int prmSrlNo)
		{
			CtrlImage ctrlImg; 
			
			long fileSize;
			System.IO.FileInfo info = new System.IO.FileInfo(indexFolderName + "\\" + prmIamgeName);
			
			fileSize = info.Length;
		    fileSize = fileSize / 1024;

		    wfeImage img;
		    imageLst = (ListBox)BoxDtls.Controls["lstImage"];
		    policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			imageLst.Items.Add(prmIamgeName);
			ctrlImg = new CtrlImage(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey,wBox.ctrlBox.BoxNumber,Convert.ToInt32(policyLst.SelectedItem.ToString()),prmIamgeName,string.Empty);
			
			img = new wfeImage(sqlCon,ctrlImg);
            img.Save(crd, eSTATES.PAGE_RESCANNED_NOT_INDEXED, fileSize, ihConstants._NORMAL_PAGE, prmSrlNo, prmIamgeName);

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            policy.UpdateStatus(eSTATES.POLICY_NOT_INDEXED, crd);
		}        
        
		void BoxDtlsBoxMouseClick(object sender, MouseEventArgs e)
		{
            DataSet policyDtls = new DataSet();

			Point pt = new Point();
			pt.X = e.X;
			pt.Y = e.Y;
			if(e.Button == MouseButtons.Right )
			{
				ihwQuery wQ = new ihwQuery(sqlCon);
				if  (wQ.GetSysConfigValue(ihConstants.ISADMINHOLD_KEY) == ihConstants.ISADMINHOLD_VALUE)
				{
					if (crd.role.Equals("Admin"))
					{
					    policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                	    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                	    policy = new wfePolicy(sqlCon, ctrlPolicy);

                	    int polStatus = policy.GetPolicyStatus();
                	    if (polStatus ==(int) eSTATES.POLICY_ON_HOLD)
                	    {
                    	    markNotReadyHoldPolicyToolStripMenuItem.Enabled = false;
                    	    markReadyToolStripMenuItem.Enabled = true;
                	    }
                	    else
                	    {
                    	    markNotReadyHoldPolicyToolStripMenuItem.Enabled = true;
                    	    markReadyToolStripMenuItem.Enabled = false;
                	    }
					    contextMenuStrip1.Show(BoxDtls,pt);
					}
				}
				else
				{
					policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                	ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                	policy = new wfePolicy(sqlCon, ctrlPolicy);

                	int polStatus = policy.GetPolicyStatus();
                	if (polStatus ==(int) eSTATES.POLICY_ON_HOLD)
                	{
                    	markNotReadyHoldPolicyToolStripMenuItem.Enabled = false;
                    	markReadyToolStripMenuItem.Enabled = true;
                	}
                	else
                	{
                    	markNotReadyHoldPolicyToolStripMenuItem.Enabled = true;
                    	markReadyToolStripMenuItem.Enabled = false;
                	}
					contextMenuStrip1.Show(BoxDtls,pt);
				}
			}
		}
		
		void HoldPolicyToolStripMenuItemClick(object sender, EventArgs e)
		{
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			imageLst = (ListBox)BoxDtls.Controls["lstImage"];
				
			
			string policyNo = policyLst.SelectedItem.ToString();
			try
			{
				if(policyNo != string.Empty)
				{
					ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyNo));
				    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (policy.UpdateStatus(eSTATES.POLICY_ON_HOLD, crd))
                    {
                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), string.Empty, string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);
                        wImage.TotalImageUpdateStatus(eSTATES.PAGE_ON_HOLD);
                        EnableDisbleControls(false);
                    }
				}
				contextMenuStrip1.Hide();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error while updating policy status......" + ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}
		
		void markReadyToolStripMenuItemClick(object sender, EventArgs e)
		{
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			string policyNo = policyLst.SelectedItem.ToString();
			try
			{
				if(policyNo != string.Empty)
				{
					ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyNo));
				    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (policy.UpdateStatus(eSTATES.POLICY_FQC, crd))
                    {
                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), string.Empty, string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);
                        wImage.TotalImageUpdateStatus(eSTATES.PAGE_FQC);
                        EnableDisbleControls(true);
                        CreateAllFolders();
                    }
				}
				contextMenuStrip1.Hide();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error while updating policy status...." + ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}

        //void ShowBatchSummary(object sender, EventArgs e)
        //{
        //    frmRptViewer viewer = new frmRptViewer("BatchSummary", wBox, sqlCon);
        //    viewer.ShowDialog(this);
        //}
		void cmdClick(object sender, EventArgs e)
		{
			string imageName;
			
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			imageLst = (ListBox)BoxDtls.Controls["lstImage"];
			int pos = imageLst.SelectedItem.ToString().IndexOf("-");		
			if(pos > 0)
			{
				imageName=imageLst.SelectedItem.ToString().Substring(0,pos);
		    }
			else
			{
				imageName = imageLst.SelectedItem.ToString();
			}
			pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),imageName,string.Empty);
		    wfeImage wImage  = new wfeImage(sqlCon, pImage);
		    for(int i=0;i<listView1.Items.Count;i++)
		    {
		    	if(listView1.Items[i].Checked == true)
		    		wImage.UpdateCustomException(ihConstants._RESOLVED,listView1.Items[i].Text,crd);
		    }
		    PopulateListView();
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
        private void ClearSelection()
        {
            for (int i = 0; i < imageLst.Items.Count; i++)
            {
                if (imageLst.GetSelected(i))
                {
                    imageLst.SetSelected(i, false);
                }
            }
        }
        private void lvwDockTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lnkPage1.Visible = false;
            lnkPage2.Visible = false;
            lnkPage3.Visible = false;
            currntPg = 0;
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            //if (tabControl2.SelectedIndex == 1)
            //{
            for (int i = 0; i < lvwDockTypes.Items.Count; i++)
            {
                if (lvwDockTypes.Items[i].Selected == true)
                {
                    selDocType = lvwDockTypes.Items[i].SubItems[0].Text;
                    ShowThumbImage(selDocType);
                    for (int j = 0; j < imageLst.Items.Count; j++)
                    {
                        string srchStr = imageLst.Items[j].ToString();
                        if (srchStr.IndexOf(selDocType) > 0)
                        {
                            //ClearSelection();
                            imageLst.SelectedIndex = j;
                            break;
                        }
                    }
                    imageLst.Focus();
                }
            }
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
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            pImage = new CtrlImage(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(policyLst.SelectedItem), string.Empty, pDocType);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            ds = wImage.GetAllIndexedImageName();
            ClearPicBox();
            if (ds.Tables[0].Rows.Count > 0)
            {
                imageName = new string[ds.Tables[0].Rows.Count];
                if (ds.Tables[0].Rows.Count <= 6)
                {
                    lnkPage1.Visible = true;
                    lnkPage2.Visible = false;
                    lnkPage3.Visible = false;
                }
                if ((ds.Tables[0].Rows.Count > 6) && (ds.Tables[0].Rows.Count <= 12))
                {
                    lnkPage1.Visible = true;
                    lnkPage2.Visible = true;
                    lnkPage3.Visible = false;
                }
                if ((ds.Tables[0].Rows.Count > 12) && (ds.Tables[0].Rows.Count <= 14))
                {
                    lnkPage1.Visible = true;
                    lnkPage2.Visible = true;
                    lnkPage3.Visible = true;
                }
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    imageFileName = indexFolderName + "\\" + ds.Tables[0].Rows[j][0].ToString();
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
                    imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

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
        private void lnkPage2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
                imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

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

        private void lnkPage1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
                imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
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

        private void lnkPage3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
                imgThumbNail = CreateThumbnail(imgThumbNail, w, h); //imgThumbNail.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);

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

        void PictureBox1DoubleClick(object sender, System.EventArgs e)
		{
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if (imageName != null)
            {
                if (imageName.Length >= 1)
                {
                    //ThumbnailChangeSize(pictureBox1.Tag.ToString());

                    int lstIndex;
                    lstIndex = (currntPg * 6) + 0 + GetDocTypePos();

                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
		}
        void PictureBox2DoubleClick(object sender, System.EventArgs e)
		{
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if (imageName != null)
            {
                if (imageName.Length >= 2)
                {

                    //ThumbnailChangeSize(pictureBox2.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 1 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
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
        private void lvwDockTypes_Click(object sender, EventArgs e)
        {
         
        }

        private void BoxDtls_PreviousClicked(object sender, EventArgs e)
        {
            
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];

            string policyNo = policyLst.SelectedItem.ToString();

            ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo));
            policy = new wfePolicy(sqlCon, ctrlPolicy);
            if (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
            {
                EnableDisbleControls(true);
            }
            else
            {
                EnableDisbleControls(false);
            }
            //BoxDtls.SetCurrentSelection(imageLst.SelectedIndex);
            //ClearSelection();
            //BoxDtls.MovePrevious();
            ShowImage(false);
        }

        private void ShowBatchSummary_Click(object sender, EventArgs e)
        {
            frmRptViewer viewer = new frmRptViewer("BatchSummary", wBox, sqlCon);
            viewer.ShowDialog(this);
        }
        void BoxDtls_LstImgClick(object sender, System.EventArgs e)
        {
            DateTime stdt = DateTime.Now;
            tabControl2.SelectedIndex = 0;
            ShowImage(false);
            DateTime enddt = DateTime.Now;
            TimeSpan tp = enddt - stdt;
            //MessageBox.Show(tp.Milliseconds.ToString());
            //ChangeSize();
        }
		void PictureBox3DoubleClick(object sender, EventArgs e)
		{
            //picBig.Image = null;
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if (imageName != null)
            {
                if (imageName.Length >= 3)
                {

                    //ThumbnailChangeSize(pictureBox3.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 2 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
		}
        void aeFQC_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    if (picBig.Visible == true)
            //    {
            //        picBig.Visible = false;
            //        panelBig.Visible = false;
            //        picBig.Image = null;
            //    }
            //}
            ///For checking todays production count
            if ((e.KeyCode == Keys.F9))
            {
                wfePolicy wPolicy = new wfePolicy(sqlCon);
                int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_FQC);
                frmProductionCount frmProd = new frmProductionCount(count);
                frmProd.ShowDialog(this);
            }
            if ((e.KeyCode == Keys.L) && (e.Control))
            {
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                DataSet pDs = policy.GetPolicyLog();
                if (pDs.Tables.Count > 0)
                {
                    if (pDs.Tables[0].Rows.Count > 0)
                    {
                        frmPolicyLog log = new frmPolicyLog(pDs,ctrlPolicy.PolicyNumber.ToString());
                        log.Show();
                    }
                }
                else
                {
                    MessageBox.Show("No log information available for this policy....");
                }
            }
        }
		void PictureBox4DoubleClick(object sender, EventArgs e)
		{
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if (imageName != null)
            {
                if (imageName.Length >= 4)
                {

                    //ThumbnailChangeSize(pictureBox4.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 3 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
		}
		
		void PictureBox5DoubleClick(object sender, EventArgs e)
		{
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if (imageName != null)
            {
                if (imageName.Length >= 5)
                {

                    //ThumbnailChangeSize(pictureBox5.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 4 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
		}
        
		void PictureBox6DoubleClick(object sender, EventArgs e)
		{
            //Bitmap bmp;
            //picBig.Image = null;
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if (imageName != null)
            {
                if (imageName.Length >= 6)
                {

                    //ThumbnailChangeSize(pictureBox6.Tag.ToString());
                    int lstIndex;
                    lstIndex = (currntPg * 6) + 5 + GetDocTypePos();
                    if (lstIndex < imageLst.Items.Count)
                    {
                        imageLst.SelectedIndex = lstIndex;
                        ShowImage(true);
                    }
                    tabControl2.SelectedIndex = 0;
                    lvwDockTypes.Focus();
                    //picBig.Visible = true;
                    //panelBig.Visible = true;
                }
            }
		}
        private int GetDocTypePos()
        {
            string currntDoc;
            int index = 0;
            string srchStr;
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            for (int i = 0; i < lvwDockTypes.Items.Count; i++)
            {
                if (lvwDockTypes.Items[i].Selected == true)
                {
                    currntDoc = lvwDockTypes.Items[i].SubItems[0].Text;
                    for (int j = 0; j < imageLst.Items.Count; j++)
                    {
                        srchStr = imageLst.Items[j].ToString();
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
        void RdoLICCheckedChanged(object sender, System.EventArgs e)
		{
            if (rdoLIC.Checked == true)
            {
                NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
                state[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                BoxDtls.RefreshNotify(state);
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                prmButtonCopyTo.Enabled = false;
                prmButtonCopyProposalForm.Enabled = false;
                prmButtonCopyProposalReviewSlip.Enabled = false;
                if (policyLst.Items.Count <= 0)
                {
                    MessageBox.Show("No LIC exception found for this box....");
                    rdoAll.Checked = true;
                }
            }
		}
        void RdoAllCheckedChanged(object sender, System.EventArgs e)
		{
            if (rdoAll.Checked == true)
            {
                NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[7];
                state[0] = NovaNet.wfe.eSTATES.POLICY_FQC;
                state[1] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
                state[2] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
                state[3] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
                state[4] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
                state[5] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
                state[6] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
                BoxDtls.RefreshNotify(state);
                prmButtonCopyTo.Enabled = false;
                prmButtonCopyProposalForm.Enabled = false;
                prmButtonCopyProposalReviewSlip.Enabled = false;
            }
		}

        private void cmbDocType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                PopulateListView();
            }
        }
	}
}
