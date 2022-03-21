/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 9/3/2009
 * Time: 7:35 PM
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

//using System.Drawing.Bitmap;
//using System.Drawing.Graphics;
//using Graphics.DrawImage;




namespace ImageHeaven
{
	/// <summary>
	/// Description of Form2.
	/// </summary>
	public partial class aeImageQC : Form,StateData
	{
			
			System.Windows.Forms.Button prmButtonCrop = new Button();
            System.Windows.Forms.Button prmButtonAutoCrp = new Button();            
            System.Windows.Forms.Button prmButtonRotateRight = new Button();
            System.Windows.Forms.Button prmButtonRotateLeft = new Button();
			System.Windows.Forms.Button prmButtonZoomIn = new Button();
            System.Windows.Forms.Button prmButtonZoomOut = new Button();
			System.Windows.Forms.Button prmButtonSkewRight  = new Button();
			System.Windows.Forms.Button prmButtonSkewLeft  = new Button();
			System.Windows.Forms.Button prmButtonNoiseRemove  = new Button();
			System.Windows.Forms.Button prmButtonCleanImg  = new Button();
			System.Windows.Forms.Button prmButtonCopyImage  = new Button();
			System.Windows.Forms.Button prmButtonDelImage = new Button() ;
			System.Windows.Forms.Button prmButtonRescan  = new Button() ;
			System.Windows.Forms.Button prmPhotoCrop = new Button();
			System.Windows.Forms.Button prmEndPhotoCrop  = new Button();
			System.Windows.Forms.Button prmGetPhoto  = new Button();
			System.Windows.Forms.Button prmNext=null;
			System.Windows.Forms.Button prmPrevious=null;
//            System.Windows.Forms.Label prmProject = null;
//            System.Windows.Forms.Label prmBatch = null;
//            System.Windows.Forms.Label prmBox = null;
//		
		//private DummyToolbox m_toolbox = new DummyToolbox();
		private OdbcConnection sqlCon=null;
		private bool m_bSaveLayout = true;
		private DeserializeDockContent m_deserializeDockContent;
		private FloatToolbox m_toolbox = new FloatToolbox();
		//private MagickNet.Image imgQc;
		private string imgFileName=null;
        NovaNet.Utils.ImageManupulation delImage;
        private wfeBox wBox=null;
        private CtrlPolicy pPolicy=null;
        private CtrlImage pImage=null;
		private CtrlBox pBox=null;
		NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
		private bool photoCropOperation=false;
		private bool getPhotoOperation=false;
		private string error=null;
        private Credentials crd = new Credentials();
        MemoryStream stateLog;
        byte[] tmpWrite;
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
        //ImageCodecInfo info = null;
		//EncoderParameters ep =null;
		/// <summary>
		/// For drawing rectangle
		/// </summary>
		private int	cropX;
    	private int cropY;
    	private int cropWidth;
    	private int cropHeight;
        private double constRotateAngle;
    	private int OperationInProgress;
    	private bool hasPhotoBol;
    	private bool delinsrtBol = false;
    	//private Bitmap cropBitmap;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev,Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	
    	private Pen cropPen;
    	private int cropPenSize = 1;
    	private System.Drawing.Color cropPenColor= System.Drawing.Color.Blue;
        
        
        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed=1;
        private ImageConfig config = null;
		
        private ListBox policyLst= null;
        private ListBox delImgList = null;
        private Label lblImageSize = null;
		private ListBox imageLst = null;
		private ListBox imageDelLst = null;
		private CtrlPolicy ctrlPolicy =null;
		private wfePolicy policy = null;
		private udtPolicy policyData=null;
		private FileorFolder fileMove=null;
		private string sourceFilePath=null;
		private string qcFolderName=null;
		private ToolTip tp=new ToolTip();
		private Imagery img;
        private string policy_Path = string.Empty;
        private int policyLen = 0;
		public aeImageQC(wfeBox prmBox,OdbcConnection prmCon,Credentials prmCrd)
		{
			sqlCon=prmCon;
			wBox = prmBox;
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
            crd = prmCrd;
			InitializeComponent();
			img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
			//img = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);
			
			m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
			this.Text="Image Quality Control";
            exMailLog.SetNextLogger(exTxtLog);
		}
		public aeImageQC()
		{			
			InitializeComponent();
			
			m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
			this.Text="Image Quality Control";

            exMailLog.SetNextLogger(exTxtLog);
            
		}
		void Button1Click(object sender, EventArgs e)
		{
			m_toolbox.Show(dockPanel);
		}
        MemoryStream StateData.StateLog()
        {
            return stateLog;
        }
		void Form2Load(object sender, EventArgs e)
		{
            try
            {
                ReadConfigKey();

                prmNext = (Button)BoxDtls.Controls["cmdNext"];
                prmPrevious = (Button)BoxDtls.Controls["cmdPrevious"];
                prmNext.ForeColor = Color.Black;
                prmPrevious.ForeColor = Color.Black;
                if(lblImageSize != null)
                    lblImageSize.ForeColor = Color.Black;
                System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();

                string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
                ArrayList arrPolicy = new ArrayList();

                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(aeImageQC));

                wQuery pQuery = new ihwQuery(sqlCon);

                tp.ShowAlways = true;
                if (File.Exists(configFile))
                    dockPanel.LoadFromXml(configFile, m_deserializeDockContent);

                delImage = new NovaNet.Utils.ImageManupulation(CropRegister);
                tp.SetToolTip(prmButtonCrop, "Crop");
                prmButtonCrop.Text = "Crop";
                bttnToolTip.SetToolTip(prmButtonCrop, "Shortcut Key-" + cropKey);
                //Bitmap bmp = new Bitmap("D:\\subhajit\\Projects\\EDMS\\Working_Code\\M31\\ImageHeaven\\Resources\\crop.png");
                //prmButtonCrop.Image = bmp; //((System.Drawing.Image)(resources.GetObject("crop.png")));
                m_toolbox.AddButton(prmButtonCrop, delImage);
                prmButtonCrop.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(AutoCrop);
                //prmButtonAutoCrp = new System.Windows.Forms.Button();
                bttnToolTip.SetToolTip(prmButtonAutoCrp, "Shortcut Key-" + autoCropKey);
                prmButtonAutoCrp.Text = "Auto-Crop";
                m_toolbox.AddButton(prmButtonAutoCrp, delImage);
                prmButtonAutoCrp.ForeColor = Color.Black;

                config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
                constRotateAngle = Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.ROTATE_ANGLE_KEY).Replace("\0", ""));
                delImage = new NovaNet.Utils.ImageManupulation(RotateRight);
                bttnToolTip.SetToolTip(prmButtonRotateRight, "Shortcut Key-" + rotateRKey);
                prmButtonRotateRight.Text = "Rotate Right";
                m_toolbox.AddButton(prmButtonRotateRight, delImage);
                prmButtonRotateRight.ForeColor = Color.Black;

                //delImage = ZoomOut;
                delImage = new NovaNet.Utils.ImageManupulation(RotateLeft);
                bttnToolTip.SetToolTip(prmButtonRotateLeft, "Shortcut Key-" + rotateLKey);
                prmButtonRotateLeft.Text = "Rotate Left";
                m_toolbox.AddButton(prmButtonRotateLeft, delImage);
                prmButtonRotateLeft.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(ZoomIn);
                bttnToolTip.SetToolTip(prmButtonZoomIn, "Shortcut Key-" + zoomInKey);
                prmButtonZoomIn.Text = "Zoom In";
                m_toolbox.AddButton(prmButtonZoomIn, delImage);
                prmButtonZoomIn.ForeColor = Color.Black;

                //delImage = ZoomOut;
                delImage = new NovaNet.Utils.ImageManupulation(ZoomOut);
                bttnToolTip.SetToolTip(prmButtonZoomOut, "Shortcut Key-" + zoomOutKey);
                prmButtonZoomOut.Text = "Zoom Out";
                m_toolbox.AddButton(prmButtonZoomOut, delImage);
                prmButtonZoomOut.ForeColor = Color.Black;

                config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
                //skewXAngle =Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.SKEW_X_KEY).Replace("\0", ""));
                //skewYAngle=Convert.ToDouble(config.GetValue(ihConstants.IMAGE_RELATED_VALUE_SECTION, ihConstants.SKEW_Y_KEY).Replace("\0", ""));
                delImage = new NovaNet.Utils.ImageManupulation(SkewRight);
                bttnToolTip.SetToolTip(prmButtonSkewRight, "Shortcut Key-" + skewRKey);
                prmButtonSkewRight.Text = "Deskew";
                m_toolbox.AddButton(prmButtonSkewRight, delImage);
                prmButtonSkewRight.ForeColor = Color.Black;

                //            delImage = new NovaNet.Utils.ImageManupulation(SkewLeft);
                //			//prmButtonSkewLeft = new System.Windows.Forms.Button();
                //			prmButtonSkewLeft.Text="Skew Left";
                //            m_toolbox.AddButton(prmButtonSkewLeft,delImage);

                delImage = new NovaNet.Utils.ImageManupulation(NoiseRemove);
                bttnToolTip.SetToolTip(prmButtonNoiseRemove, "Shortcut Key-" + noiseRemovalLKey);
                prmButtonNoiseRemove.Text = "Despacle";
                //prmButtonNoiseRemove.AutoSize=true;
                m_toolbox.AddButton(prmButtonNoiseRemove, delImage);
                prmButtonNoiseRemove.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(CleanImageRegister);
                bttnToolTip.SetToolTip(prmButtonCleanImg, "Shortcut Key-" + cleanKey);
                prmButtonCleanImg.Text = "Clean";
                //prmButtonCleanImg.AutoSize=true;
                m_toolbox.AddButton(prmButtonCleanImg, delImage);
                prmButtonCleanImg.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(ImageCopy);
                bttnToolTip.SetToolTip(prmButtonCopyImage, "Shortcut Key-(Control+z)");
                prmButtonCopyImage.Text = "Copy Original";
                //prmButtonCopyImage.AutoSize=true;
                m_toolbox.AddButton(prmButtonCopyImage, delImage);
                prmButtonCopyImage.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(ImageDelete);
                bttnToolTip.SetToolTip(prmButtonDelImage, "Shortcut Key-" + deleteKey);
                prmButtonDelImage.Text = "Delete";
                //prmButtonDelImage.AutoSize=true;
                m_toolbox.AddButton(prmButtonDelImage, delImage);
                prmButtonDelImage.ForeColor = Color.Black;
                delImage = new NovaNet.Utils.ImageManupulation(PhotoCropRegister);
                //prmPhotoCrop = new System.Windows.Forms.Button();
                prmPhotoCrop.Text = "Photo Crop";
                //prmPhotoCrop.AutoSize=true;
                m_toolbox.AddButton(prmPhotoCrop, delImage);
                prmPhotoCrop.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(EndPhotoCrop);
                //prmEndPhotoCrop = new System.Windows.Forms.Button();
                prmEndPhotoCrop.Text = "End Edit(Photo)";
                //prmEndPhotoCrop.AutoSize=true;
                m_toolbox.AddButton(prmEndPhotoCrop, delImage);
                prmEndPhotoCrop.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(GetPhoto);
                //prmEndPhotoCrop = new System.Windows.Forms.Button();
                prmGetPhoto.Text = "Get Photo";
                //prmGetPhoto.AutoSize=true;
                m_toolbox.AddButton(prmGetPhoto, delImage);
                prmGetPhoto.ForeColor = Color.Black;

                this.WindowState = FormWindowState.Maximized;
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                imageLst.SelectionMode = SelectionMode.One;
                if (policyLst.SelectedItem != null)
                {
                    wfePolicy wPolicy = new wfePolicy(sqlCon);
                    int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_QC);
                    this.Text = this.Text + "                                       Today you have done " + count;
                    BoxDtls.PopulateDelList(Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    tssBatch.Visible = false;
                }

                else
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading QC form..." + " " + ex.Message);
            }
		}
		
		private IDockContent GetContentFromPersistString(string persistString)
		{
				return m_toolbox;
		}
		int RescanImage()
		{
			string sourcePath = null;
			string destinationPath = null;
			
			imageLst = (ListBox)BoxDtls.Controls["lstImage"];
			if(imageLst.SelectedItem.ToString() != string.Empty)
			{
				sourcePath = qcFolderName + "\\" + imageLst.SelectedItem.ToString();
				destinationPath = sourceFilePath + "\\" + imageLst.SelectedItem.ToString();
				aeScanControl scControl=new aeScanControl(imageLst.SelectedItem.ToString(),qcFolderName,sourcePath,destinationPath);
				scControl.ShowDialog(this);
				img.LoadBitmapFromFile(sourcePath);
				imgFileName = sourcePath;
				pictureControl.Image = img.GetBitmap();
				
			}
			return 0;
		}

		void Form2FormClosing(object sender, FormClosingEventArgs e)
		{
			string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            UpdateAllPolicyStatus();
			//sqlCon.Close();
//            if (m_bSaveLayout)
//                dockPanel.SaveAsXml(configFile);
//            else if (File.Exists(configFile))
//                File.Delete(configFile);
		}

        public void aeImageQC_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                panel1.Left = BoxDtls.Right + 30;
                panel1.Width =(dockPanel.Width- BoxDtls.Right )-30;
                //panel1.Top = panel2.Top;
                panel1.Height = this.ClientSize.Height-20;
                pictureControl.Height = panel1.Height - 3;
                m_toolbox.Height = this.ClientSize.Height - BoxDtls.Height;
            }
            //pictureControl.Width = this.ClientSize.Width;
        }

        void UpdateAllPolicyStatus()
        {
            //string policyPath;
            //string photoPath = null;
            wfeImage wImage;
            //string changedImageName;
            System.IO.FileInfo info;
            //long fileSize;
            wfePolicy wPolicy;
            wfeBox box;
            
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];


            if (policyLst.Items.Count > 0)
            {
                for (int i = 0; i < policyLst.Items.Count; i++)
                {
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.Items[i].ToString()), string.Empty, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.Items[i].ToString()));
                    wPolicy = new wfePolicy(sqlCon, pPolicy);
                    if (wImage.GetImageCount(eSTATES.PAGE_SCANNED) == false)
                    {
                        crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                        wPolicy.UpdateStatus(eSTATES.POLICY_QC,crd,true);
                        //wPolicy.UnLockPolicy();
                        ///update into transaction log
                        wPolicy.UpdateTransactionLog(eSTATES.POLICY_QC, crd);
                    }

                    pBox = new CtrlBox(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber);
                    box = new wfeBox(sqlCon, pBox);
                    NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
                    state[0] = NovaNet.wfe.eSTATES.POLICY_SCANNED;

                    if (wPolicy.GetPolicyCount(state) == 0)
                    {
                        box.UpdateStatus(eSTATES.BOX_QC);
                    }
                    /////For checking the box status
                    //NovaNet.wfe.eSTATES[] boxState = new NovaNet.wfe.eSTATES[1];
                    //boxState[0] = NovaNet.wfe.eSTATES.BOX_SCANNED;

                    //wBatch = new wfeBatch(sqlCon);
                    //if (box.GetBoxCount(boxState) == 0)
                    //{
                    //    ///Update the batch status
                    //    wBatch.UpdateStatus(eSTATES.BATCH_QC,wBox.ctrlBox.BatchKey);
                    //}
                    /*
                    if (policy.GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
                    {
                        hasPhotoBol = true;
                    }
                    else
                    {
                        hasPhotoBol = false;
                    }
                    if (hasPhotoBol == true)
                    {
                        policyData = (udtPolicy)wPolicy.LoadValuesFromDB();
                        policyPath = policyData.policy_path;
                        if (Directory.Exists(policyPath + "\\" + ihConstants._QC_FOLDER))
                        {
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.Items[i].ToString()), policyLst.Items[i].ToString() + "_000_A.TIF", string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                            if (changedImageName == string.Empty)
                            {
                                photoPath = policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + policyLst.Items[i].ToString() + "_000_A.TIF";
                            }
                            else
                            {
                                photoPath = policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + changedImageName;
                            }
                            if (File.Exists(photoPath))
                            {
                                img.LoadBitmapFromFile(photoPath);
                                img.SaveFile(photoPath);
                                info = new System.IO.FileInfo(photoPath);
                                fileSize = info.Length;
                                fileSize = fileSize / 1024;
                                UpdateImageSize(fileSize);
                            }

                        }
                    }
                    */

                }

            }
        }
		int CropPhoto(Rectangle udtRect)
		{
			Bitmap bmpImage;
			double htRatio = 0;
			double wdRatio = 0;
			imageLst = (ListBox)BoxDtls.Controls["lstImage"];
			long fileSize;
			
			foreach (ImageCodecInfo ice in ImageCodecInfo.GetImageEncoders())
			{
				if (ice.MimeType == "image/tiff")
				{
				//info = ice;
				break;
				}
			}
			try
			{
				if(OperationInProgress==ihConstants._PHOTO_CROP)
				{
					Rectangle rect = udtRect;
					bmpImage = img.GetBitmap();
					//Calculate the ratio for Picturebox to actual image
					htRatio = Convert.ToDouble(bmpImage.Size.Height)/Convert.ToDouble(pictureControl.Height);
					rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
					wdRatio  = (Convert.ToDouble(bmpImage.Size.Width)/Convert.ToDouble(pictureControl.Width));
					rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width)*wdRatio);
					rect.X = Convert.ToInt32(rect.X * wdRatio);
					rect.Y = Convert.ToInt32(rect.Y * htRatio);
					
					//Crop
					img.Crop(rect);
					
					//Set the new image file name
					imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString().Substring(0,policyLen) + "_000_A.TIF";
					
					//Call the save routine
					img.SaveFile(imgFileName);
					img.LoadBitmapFromFile(imgFileName);
					pictureControl.Image=img.GetBitmap();
					//Show the image back in picture box
//					pictureControl.Width = panel1.Width - 2;
//	                pictureControl.Height = panel1.Height - 2;
//	                if (!System.IO.File.Exists(imgFileName)) return 0;
//	                Image newImage = Image.FromFile(imgFileName);
//	                
//	                pictureControl.Image = newImage.GetThumbnailImage(pictureControl.Width, pictureControl.Height, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
//	                newImage.Dispose();
										
					//Change the size of the image in relation to the canvas
					ChangeSize();
					panel1.Left = BoxDtls.Right + 4;
                	panel1.Width =(dockPanel.Width- BoxDtls.Right )- 6;
                	//panel1.Top = panel2.Top;
                	panel1.Height = this.ClientSize.Height-20;
					//Calculate and show file info
					System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
					
					//Update file info in db
					UpdateImageSize(fileSize);
					delinsrtBol = false;
                    BoxDtls.Controls["cmdNext"].Focus();
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error while cropping the photo","Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			photoCropOperation = true;
			//ChangeSize();
			//imgQc.Write(imgFileName);
			//pictureControl.Image = MagickNet.Image.ToBitmap(imgQc);
			return 0;
		}
		
		int GetPhoto()
		{

            try
            {
                prmButtonRescan.Enabled = false;
                prmButtonCopyImage.Enabled = false;
                prmPhotoCrop.Enabled = false;
                //imageLst = (ListBox)BoxDtls.Controls["lstImage"];

                //img.UpdateImageSize(crd, eSTATES.PAGE_QC,fileSize);
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";
                if (File.Exists(imgFileName))
                {
                    /*
                    if (ino > 0)
                        imgQc.ReleaseGdPictureImage(ino);
	    		
                    ino=imgQc.CreateGdPictureImageFromFile(imgFileName);
                    imgQc.SaveAsTIFF(ino,imgFileName,GdPicture.TiffCompression.TiffCompressionJPEG);
                    Bitmap newBmp = new Bitmap(pictureControl.Width,pictureControl.Height);
                    newBmp = imgQc.GetBitmapFromGdPictureImage(ino);                           
                    pictureControl.Image = newBmp;//imgQc.GetBitmapFromGdPictureImage(ino);
                    */
                    //img.LoadBitmapFromFile(imgFileName);
                    //img.SaveAsTiff(imgFileName, IGRComressionTIFF.JPEG);
                    ChangeSize(imgFileName);
                    //pictureControl.Image=img.GetBitmap();
                    getPhotoOperation = true;
                    prmNext = (Button)BoxDtls.Controls["cmdNext"];
                    prmPrevious = (Button)BoxDtls.Controls["cmdPrevious"];
                    prmNext.Enabled = false;
                    prmPrevious.Enabled = false;
                    //prmButtonSkewRight.Enabled = false;
                    prmButtonDelImage.Enabled = false;
                    delinsrtBol = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
			return 0;
		}
		
		int EndPhotoCrop()
		{
			string photoName;
			wfeImage wImg;
			long fileSize;

            try
            {
                System.IO.FileInfo info = null;
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";
                if (File.Exists(imgFileName))
                {
                    if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()))
                    {
                        File.Delete(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                    }
                    OperationInProgress = ihConstants._NO_OPERATION;
                    if (photoCropOperation == true)
                    {
                        info = new System.IO.FileInfo(imgFileName);
                        fileSize = info.Length;
                        fileSize = fileSize / 1024;

                        policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                        imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                        photoName = imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF";

                        pImage = new CtrlImage(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(policyLst.SelectedItem.ToString()), photoName, string.Empty);

                        wImg = new wfeImage(sqlCon, pImage);
                        if (wImg.DeletePage() == true)
                        {
                            wImg.Save(crd, eSTATES.PAGE_QC, fileSize, ihConstants._PHOTO_PAGE, 0, photoName);
                        }
                        //Save the colour photograph into 000
                        //img.SaveAsTiff(imgFileName,IGRComressionTIFF.JPEG);
                    }
                    //Now load the black and white image	
                }
                imgFileName = qcFolderName + "\\" + imageLst.SelectedItem.ToString();

                ChangeSize(imgFileName);
                if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                }
                if (File.Exists(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString());
                }
                prmButtonCrop.Enabled = true;
                prmButtonAutoCrp.Enabled = true;
                prmButtonRotateRight.Enabled = true;
                prmButtonRotateLeft.Enabled = true;
                prmButtonZoomIn.Enabled = true;
                prmButtonZoomOut.Enabled = true;
                prmButtonSkewRight.Enabled = true;
                prmButtonSkewLeft.Enabled = true;
                prmButtonNoiseRemove.Enabled = true;
                prmButtonCleanImg.Enabled = true;
                prmButtonCopyImage.Enabled = true;
                prmButtonDelImage.Enabled = true;
                prmButtonRescan.Enabled = true;
                photoCropOperation = false;
                getPhotoOperation = false;
                prmPhotoCrop.Enabled = true;
                prmGetPhoto.Enabled = true;
                prmButtonSkewRight.Enabled = true;
                prmNext = (Button)BoxDtls.Controls["cmdNext"];
                prmPrevious = (Button)BoxDtls.Controls["cmdPrevious"];
                prmNext.Enabled = true;
                prmPrevious.Enabled = true;
                delinsrtBol = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
			return 0;
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
		
		int Crop(Rectangle udtRect)
		{
			double htRatio = 0;
			double wdRatio = 0;
			long fileSize;
			Bitmap bmpImage=null;
			try
			{
				Rectangle rect = udtRect;
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
					//pictureControl.Image = img.GetBitmap();
					//bool ret = ToBitmap.scanToImage.gdSaveFile(imgQc,ino,imgFileName);
					//pictureControl.Image = imgQc.GetBitmapFromGdPictureImage(ino);
				}
				//Change the size of the image in relation to the canvas
				ChangeSize();
				
				System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
				fileSize = info.Length;
				fileSize = fileSize / 1024;
                lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImageSize.Text = fileSize.ToString() + " KB";
				UpdateImageSize(fileSize);
                BoxDtls.Controls["cmdNext"].Focus();
				//bmpCrop.Dispose();
				delinsrtBol = false;
			}
			catch(Exception ex)
			{
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
				MessageBox.Show("Error while cropping the image" + ex.Message,"Crop error");
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
					Bitmap bmpImage = img.GetBitmap();
					htRatio = Convert.ToDouble(bmpImage.Size.Height)/Convert.ToDouble(pictureControl.Height);
					rect.Height = Convert.ToInt32(Convert.ToDouble(rect.Height) * htRatio);
					wdRatio  = (Convert.ToDouble(bmpImage.Size.Width)/Convert.ToDouble(pictureControl.Width));
					rect.Width = Convert.ToInt32(Convert.ToDouble(rect.Width)*wdRatio);

					rect.X = Convert.ToInt32(rect.X * wdRatio);
					rect.Y = Convert.ToInt32(rect.Y * htRatio);
					
					img.Clean(rect);
					img.SaveFile(imgFileName);
					img.LoadBitmapFromFile(imgFileName);
					//pictureControl.Image = img.GetBitmap();
					//Change the size of the image in relation to the canvas
					ChangeSize();
					
					System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
                    lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                    lblImageSize.Text = fileSize.ToString() + " KB";
					UpdateImageSize(fileSize);
					//bmpCrop.Dispose();
					delinsrtBol = false;
                    BoxDtls.Controls["cmdNext"].Focus();
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
			string sourceFileName=null;
			string destFileName=null;
			string qcFilePath=null;

            delinsrtBol = true;
            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageLst.SelectedItem.ToString(), string.Empty);
            if (UpdateState(eSTATES.PAGE_DELETED, imageLst.SelectedItem.ToString()) == true)
            {
                qcDelPath = sourceFilePath + "\\" + ihConstants._DELETE_FOLDER;
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                sourceFileName = sourceFilePath + "\\" + imageLst.SelectedItem.ToString();
                destFileName = qcDelPath + "\\" + imageLst.SelectedItem.ToString();
                qcFilePath = qcFolderName + "\\" + imageLst.SelectedItem.ToString();
                if (FileorFolder.CreateFolder(qcDelPath) == true)
                {
                    if (File.Exists(destFileName) == false)
                    {
                        File.Move(sourceFileName, destFileName);
                        File.Delete(qcFilePath);
                    }
                }
                BoxDtls.DeleteNotify(imageLst.SelectedIndex);
                ShowImage(true);
                BoxDtls.Controls["cmdNext"].Focus();
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

		int ImageCopy()
		{
			string policyPath;
			string policyName;
			string changedImageName=string.Empty;
			DataSet ds = new DataSet();

			try
			{
				delinsrtBol = false;
				policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
				imageLst = (ListBox)BoxDtls.Controls["lstImage"];
				
				//Check for policy has photo or not
				ctrlPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey,wBox.ctrlBox.BoxNumber,Convert.ToInt32(policyLst.SelectedItem.ToString()));
			    policy = new wfePolicy(sqlCon, ctrlPolicy);
				if(policy.GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
				{
					if((imageLst.SelectedIndex+1) == ihConstants._PHOTO_PAGE_POSITION)
					{
						prmPhotoCrop.Enabled= true;
						prmEndPhotoCrop.Enabled = true;
						hasPhotoBol = true;
					}
					else
					{
						prmPhotoCrop.Enabled= false;
						prmEndPhotoCrop.Enabled = false;
						hasPhotoBol = false;
					}
				}
				else
				{
					prmPhotoCrop.Enabled= false;
					prmEndPhotoCrop.Enabled = false;
					hasPhotoBol = false;
				}
			    
				policyName=policyLst.SelectedItem.ToString();
				changedImageName=imageLst.SelectedItem.ToString();
			    //policyData=(udtPolicy)policy.LoadValuesFromDB();
                policyPath = GetPolicyPath(); //policyData.policy_path;
			    fileMove=new FileorFolder();
			    string sourcePath=policyPath + "\\" + ihConstants._SCAN_FOLDER;
			    string destPath=policyPath + "\\" + ihConstants._QC_FOLDER;
			    sourceFilePath=sourcePath;
			    qcFolderName=destPath;
				    
		            
		            if(((imageLst.SelectedIndex+1) == ihConstants._PHOTO_PAGE_POSITION) && (hasPhotoBol==true))
					{
                          img.LoadBitmapFromFile(sourcePath + "\\" + changedImageName);
                          
                          if (img.GetBitmap().PixelFormat != PixelFormat.Format1bppIndexed)
                          {
                              img.ToBitonal();
                          }
                          pictureControl.Image = img.GetBitmap();
                          img.SaveFile(destPath + "\\" + changedImageName);  
					}
		            else
		            {
		            	//ShowImage();
                      img.LoadBitmapFromFile(sourcePath + "\\" + changedImageName);
                      img.SaveFile(destPath + "\\" + changedImageName);
                      img.LoadBitmapFromFile(imgFileName);
					}
				ChangeSize();
				ds.Dispose();
				pictureControl.Refresh();
                BoxDtls.Controls["cmdNext"].Focus();
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error while copying the original image","Copy Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
			}
			return 0;
		}
        int AutoCrop()
        {
        	long fileSize;
        	try
        	{
    			//Auto Crop
    			img.AutoCrop();
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
				delinsrtBol = false;
				
				UpdateImageSize(fileSize);
                BoxDtls.Controls["cmdNext"].Focus();
        	}
        	catch(Exception ex)
        	{
        		MessageBox.Show("Error while auto cropping " + ex.Message,"Auto Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
        	}
            return 0;
        }
		int CropRegister()
		{
			OperationInProgress = ihConstants._CROP;
			delinsrtBol = false;
			return 0;
		}
		
        int CleanImageRegister()
        {
            OperationInProgress = ihConstants._CLEAN;
            delinsrtBol = false;
            //ChangeSize();
            return 0; 
        }
        
        int PhotoCropRegister()
        {
        	delinsrtBol = false;
        	imageLst = (ListBox)BoxDtls.Controls["lstImage"];
        	if((hasPhotoBol == true) && ((imageLst.SelectedIndex+1) == ihConstants._PHOTO_PAGE_POSITION))
        	{
        		if(File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()) == false)
        		{
        			File.Copy(sourceFilePath + "\\" + imageLst.SelectedItem.ToString(),qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
        		}
	        	imgFileName = qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString();
	        	
	        	img.LoadBitmapFromFile(imgFileName);
	        	//pictureControl.Image = img.GetBitmap();
	        	
		        ChangeSize();
	            OperationInProgress = ihConstants._PHOTO_CROP;
	            prmButtonCrop.Enabled = false;
            	prmButtonAutoCrp.Enabled = false;
            	prmButtonRotateRight.Enabled = false;
            	prmButtonRotateLeft.Enabled = false;
				prmButtonZoomIn.Enabled = false;
            	prmButtonZoomOut.Enabled = false;
				prmButtonSkewRight.Enabled = false;
				prmButtonSkewLeft.Enabled = false;
				prmButtonNoiseRemove.Enabled = false;
				prmButtonCleanImg.Enabled = false;
				prmButtonCopyImage.Enabled = false;
				prmButtonDelImage.Enabled = false;
				prmButtonRescan.Enabled = false;
				prmGetPhoto.Enabled = false;
				//prmButtonSkewRight.Enabled = true;
        	}
            //ChangeSize();
            return 0; 
        }
        
        int ZoomIn()
        {
        	try
        	{
        		if (img.IsValid() == true)
        		{
                    pictureControl.Dock = DockStyle.None;
		            //OperationInProgress = ihConstants._OTHER_OPERATION;
		            keyPressed = keyPressed + 1;
		            zoomHeight =Convert.ToInt32(img.GetBitmap().Height * (1.2));
		            zoomWidth = Convert.ToInt32(img.GetBitmap().Width * (1.2));
		            zoomSize.Height = zoomHeight;
		            zoomSize.Width = zoomWidth;
		            
		            pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) * 1.2);
		            pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) * 1.2);
		            pictureControl.Refresh();
                    ChangeZoomSize();
		            delinsrtBol = false;
                    BoxDtls.Controls["cmdNext"].Focus();
        		}
        	}
        	catch(Exception ex)
        	{
        		MessageBox.Show("Error while zooming the image " + ex.Message,"Zoom Error");
                exMailLog.Log(ex);
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
                    pictureControl.Dock = DockStyle.None;
		            //OperationInProgress = ihConstants._OTHER_OPERATION;
		            keyPressed = keyPressed + 1;
		            zoomHeight =Convert.ToInt32(img.GetBitmap().Height / (1.2));
		            zoomWidth = Convert.ToInt32(img.GetBitmap().Width / (1.2));
		            zoomSize.Height = zoomHeight;
		            zoomSize.Width = zoomWidth;
		            
		            pictureControl.Width = Convert.ToInt32(Convert.ToDouble(pictureControl.Width) / 1.2);
		            pictureControl.Height = Convert.ToInt32(Convert.ToDouble(pictureControl.Height) / 1.2);
		            pictureControl.Refresh();
                    ChangeZoomSize();
		            delinsrtBol = false;
                    BoxDtls.Controls["cmdNext"].Focus();
	            }
        	}
            catch(Exception ex)
        	{
                MessageBox.Show("Error while zooming the image " + ex.Message, "Zoom Error");
        		error = ex.Message;
        	}
            return 0;
        }
        
        int RotateRight()
        {
        	long fileSize;
        	
            //OperationInProgress = ihConstants._OTHER_OPERATION;
			
            try
            {
    			//Rotate right +90
    			img.RotateRight();
				//Call the save routine
    			img.SaveFile(imgFileName);
                img.LoadBitmapFromFile(imgFileName);
				//Show the image back in picture box
				//pictureControl.Image = img.GetBitmap();
                ChangeSize();
	            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
				fileSize = info.Length;
				fileSize = fileSize / 1024;
				delinsrtBol = false;
				UpdateImageSize(fileSize);
                BoxDtls.Controls["cmdNext"].Focus();
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
        
        int RotateLeft()
        {
        	long fileSize;
        	
            //OperationInProgress = ihConstants._OTHER_OPERATION;
			
            try
            {
            	if (img.IsValid() == true)
            	{
	    			//Rotate right -90
	    			img.RotateLeft();
					//Call the save routine
	    			img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
					//Show the image back in picture box
					//pictureControl.Image = img.GetBitmap();
                    ChangeSize();
		            delinsrtBol = false;
		            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
					UpdateImageSize(fileSize);
                    BoxDtls.Controls["cmdNext"].Focus();
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
		//Deskew`		
		int SkewRight()
        {
			long fileSize;
            //OperationInProgress = ihConstants._OTHER_OPERATION;
			try
			{
				if (img.IsValid() == true)
				{
					//Auto Deskew
					img.AutoDeSkew(true);
					//Call the save routine
					img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
					//Show the image back in picture box
					//pictureControl.Image = img.GetBitmap();
					ChangeSize();
					System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
	                lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
	                lblImageSize.Text = fileSize.ToString() + " KB";
					UpdateImageSize(fileSize);
                    BoxDtls.Controls["cmdNext"].Focus();
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
        int AutoSkew()
        {
            long fileSize;
            //OperationInProgress = ihConstants._OTHER_OPERATION;
            try
            {
                if (img.IsValid() == true)
                {
                    //Auto Deskew
                    if (img.AutoDeSkew() == IGRStatus.Success)
                    {
                        //Call the save routine
                        img.SaveFile(imgFileName);
                        img.LoadBitmapFromFile(imgFileName);
                        System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                        fileSize = info.Length;
                        fileSize = fileSize / 1024;
                        lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                        lblImageSize.Text = fileSize.ToString() + " KB";
                        UpdateImageSize(fileSize);
                    }
                    BoxDtls.Controls["cmdNext"].Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while rotate the image" + ex.Message, "Rotation Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
            return 0;
        }
		int SkewLeft()
        {
//            OperationInProgress = ihConstants._OTHER_OPERATION;
//
//            //rotateAngle = rotateAngle + constRotateAngle;
//            imgQc = objQc.Skew(imgQc,(-skewXAngle),(-skewYAngle));
//            pictureControl.Image = MagickNet.Image.ToBitmap(imgQc);
//            pictureControl.Refresh();
//            //imgQc.Write(imgFileName);

            return 0;
        }
		
		int NoiseRemove()
        {
			long fileSize;
			try
			{
				if (img.IsValid() == true)
				{
					//Auto Deskew
					img.Despeckle();
					//Call the save routine
					img.SaveFile(imgFileName);
                    img.LoadBitmapFromFile(imgFileName);
					//Show the image back in picture box
					//pictureControl.Image = img.GetBitmap();
                    ChangeSize();
		            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
					UpdateImageSize(fileSize);
                    BoxDtls.Controls["cmdNext"].Focus();
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

        void BoxDtlsLstImageIndex(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            char DELETE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
            if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == DELETE)
            {
                ImageDelete();
            }
            if ((policyLst.SelectedIndex + 1) != (policyLst.Items.Count))
            {
                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
            }
        }

		void AeImageQCKeyUp(object sender, KeyEventArgs e)
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
                imageLst = imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                if (imageLst.Items.Count > 0)
                {
                    UpdateState(eSTATES.PAGE_QC, imageLst.SelectedItem.ToString());
                }
                BoxDtls.MoveNext();
                //ChangeSize();
                ShowImage(false);
            }
            if (e.KeyCode == Keys.Left)
            {
                imageLst = imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                if (imageLst.Items.Count > 0)
                {
                    UpdateState(eSTATES.PAGE_QC, imageLst.SelectedItem.ToString());
                }
                BoxDtls.MovePrevious();
                ShowImage(false);
            }
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
		
		void AeImageQCKeyPress(object sender, KeyPressEventArgs e)
		{
			
		}
        
		void PictureControlMouseDown(object sender, MouseEventArgs e)
		{
			if ((OperationInProgress == ihConstants._CROP) || (OperationInProgress == ihConstants._PHOTO_CROP))
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
			else
			{
				Cursor=Cursors.Default;
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

            if ((OperationInProgress == ihConstants._CROP) || (OperationInProgress == ihConstants._PHOTO_CROP))
            {
                
                if ((pictureControl.Image!=null) && (cropPen!=null))
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
            else
            {
                Cursor=Cursors.Default;
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
				
		void PictureControlMouseUp(object sender, MouseEventArgs e)
        {
            
            cropWidth = Math.Abs(e.X - cropX); 
            cropHeight = Math.Abs(e.Y - cropY);
            

            Cursor = Cursors.Default;
            
            if((OperationInProgress==ihConstants._CROP) || (OperationInProgress == ihConstants._PHOTO_CROP) || (OperationInProgress == ihConstants._CLEAN))
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
                    if (OperationInProgress == ihConstants._PHOTO_CROP)
                    {
                        CropPhoto(rect);
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
            pictureControl.Refresh();
        }
		
		void StatusStrip1ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			
		}		

		void BoxDtlsPolicyChanged(object sender, EventArgs e)
		{
			string policyPath;

            if (policyLst.SelectedItem != null)
            {
                EnableDisbleControls(true);
                delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
                if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                {
                    delImgList.SetSelected(delImgList.SelectedIndex, false);
                }
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                policy_Path = GetPolicyPath();
                BoxDtls.PopulateDelList(Convert.ToInt32(policyLst.SelectedItem.ToString()));
                ShowImage(false);
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                //policyData=(udtPolicy)policy.LoadValuesFromDB();
                //policyPath = GetPolicyPath();
                policyLen = policyLst.SelectedItem.ToString().Length;
                DataSet ds = policy.GetPolicyDetails();
                Label lblName = (Label)BoxDtls.Controls["lblName"];
                if(ds.Tables[0].Rows.Count > 0)
                    lblName.Text = "Name: " + ds.Tables[0].Rows[0]["name_of_policyholder"].ToString();              
            }
		}
        void BoxDtls_LstNextKey(object sender, KeyEventArgs e)
        {
            
        }
        /// <summary>
        /// Added in version 1.0.0.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BoxDtls_PreviousClicked(object sender, EventArgs e)
        {
            imageLst = imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];


            if (imageLst.Items.Count > 0)
            {
                EnableDisbleControls(true);
                ShowImage(false);

                delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
                if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                {
                    delImgList.SetSelected(delImgList.SelectedIndex, false);
                }

                if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
                }
                if (File.Exists(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString()))
                {
                    File.Delete(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString());
                }
            }
        }
		void BoxDtlsImageChanged(object sender, EventArgs e)
		{
            imageLst = imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            //if (File.Exists(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString()))
            //{
            //    File.Delete(qcFolderName + "\\" + "1_" + imageLst.SelectedItem.ToString());
            //}
            //if (File.Exists(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString()))
            //{
            //    File.Delete(qcFolderName + "\\" + "0_" + imageLst.SelectedItem.ToString());
            //}
            if ((imageLst.SelectedIndex + 1) == imageLst.Items.Count)
            {
                if (imageLst.Items.Count > 0)
                {
                    UpdateState(eSTATES.PAGE_QC, imageLst.SelectedItem.ToString());
                }
            }
		}
		void BoxDtlsNextClicked(object sender, EventArgs e)
		{

            imageLst = imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];

            
            if (imageLst.Items.Count > 0)
            {
                EnableDisbleControls(true);
            }
            delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
            if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
            {
                delImgList.SetSelected(delImgList.SelectedIndex, false);
            }
			
			if(imageLst.Items.Count>0)
			{
				UpdateState(eSTATES.PAGE_QC,imageLst.SelectedItem.ToString());
			}
			BoxDtls.MoveNext();
            ShowImage(false);//added in version 1.0.0.1
            //ChangeSize();//added in version 1.0.0.1
		}
		bool UpdateState(eSTATES prmPageSate,string prmPageName)
		{
			double fileSize;
            bool delBol = false;
			NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
			
			pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),prmPageName,string.Empty);
		    wfeImage wImage  = new wfeImage(sqlCon, pImage);
            if (wImage.UpdateStatus(prmPageSate, crd) == true)
            {
                delBol = true;
            }
		    if(delinsrtBol == false)
		    {
			    System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
				
				fileSize = info.Length;
			    fileSize = fileSize / 1024;
			    
				wImage.UpdateImageSize(crd,eSTATES.PAGE_QC,fileSize);
		    }
            return delBol;
		}
		void ShowImage(bool prmOverWrite)
		{
			string policyPath;
			string policyName;
			string changedImageName=string.Empty;
			DataSet ds = new DataSet();
			int photoImageName;
            try
            {
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                if (policyLst.SelectedItem != null)
                {
                    //Check for policy has photo or not
                    ctrlPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    policy = new wfePolicy(sqlCon, ctrlPolicy);

                    //policyData = (udtPolicy)policy.LoadValuesFromDB();
                    
                    policyPath = policy_Path;
                    //ds	= policy.GetPolicyList();
                    //pageCount = wPolicy.GetPolicyPageCount();
                    //prmPhotoCrop = new System.Windows.Forms.Button();
                    //prmEndPhotoCrop = new System.Windows.Forms.Button();
                    //tssBatch.Text = string.Empty;
                    if (policy.GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
                    {
                        if ((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION)
                        {
                            prmPhotoCrop.Enabled = true;
                            prmEndPhotoCrop.Enabled = true;
                            hasPhotoBol = true;
                            if (File.Exists(policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF") == false)
                            {
                                lblNote.Text = "Warning: This policy contains photo, crop it with photo crop button.";
                                lblNote.Visible = true;
                                lblNote.ForeColor = Color.Red;
                            }
                            else
                            {
                                lblNote.Visible = true;
                                lblNote.Text = "Photo crop has been done, select GetPhoto to check it";
                                lblNote.ForeColor = Color.Green;
                            }
                        }
                        else
                        {
                            prmPhotoCrop.Enabled = false;
                            prmEndPhotoCrop.Enabled = false;
                            hasPhotoBol = false;
                            lblNote.Visible = false;
                        }
                    }
                    else
                    {
                        prmPhotoCrop.Enabled = false;
                        prmEndPhotoCrop.Enabled = false;
                        hasPhotoBol = false;
                        lblNote.Visible = false;
                    }
                    policyName = policyLst.SelectedItem.ToString();
                    changedImageName = imageLst.SelectedItem.ToString();

                    //			ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    //		    policy = new wfePolicy(sqlCon, ctrlPolicy);

                    fileMove = new FileorFolder();
                    string sourcePath = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                    string destPath = policyPath + "\\" + ihConstants._QC_FOLDER;
                    sourceFilePath = sourcePath;
                    qcFolderName = destPath;
                    if (FileorFolder.CreateFolder(destPath) == true)
                    {
                        if (File.Exists(destPath + "\\" + changedImageName) == false)
                        {
                            fileMove.MoveFile(sourcePath, destPath, changedImageName, true);
                        }
                        imgFileName = destPath + "\\" + changedImageName;


                        if (File.Exists(destPath + "\\" + "0_" + changedImageName))
                        {
                            File.Delete(destPath + "\\" + "0_" + changedImageName);
                        }
                        photoImageName = 100; //Convert.ToInt32(changedImageName.Substring(10, 3));
                        if (((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION) && (hasPhotoBol == true) && ihConstants._PHOTO_PAGE_POSITION == photoImageName)
                        {
                            if ((File.Exists(destPath + "\\" + "1_" + changedImageName) == false) && (File.Exists(destPath + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF") == false))
                            {
                                File.Copy(imgFileName, destPath + "\\" + "0_" + changedImageName);
                                img.ConvertTo1Bpp(destPath + "\\" + "0_" + changedImageName, destPath + "\\" + "1_" + changedImageName);
                                File.Move(imgFileName, destPath + "\\" + "2_" + changedImageName);
                                File.Move(destPath + "\\" + "1_" + changedImageName, imgFileName);
                                File.Move(destPath + "\\" + "2_" + changedImageName, destPath + "\\" + "1_" + changedImageName);
                                prmGetPhoto.Enabled = true;
                                prmPhotoCrop.Enabled = true;
                                prmEndPhotoCrop.Enabled = true;
                            }
                            if (File.Exists(destPath + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF") == true)
                            {
                                prmGetPhoto.Enabled = true;
                            }
                        }
                        else
                        {
                            prmGetPhoto.Enabled = false;
                            prmPhotoCrop.Enabled = false;
                            prmEndPhotoCrop.Enabled = false;
                        }
                        img.LoadBitmapFromFile(imgFileName);
                        AutoSkew();
                        ChangeSize();
                        //pictureControl.Image = img.GetBitmap();
                        System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                        long fileSize = info.Length;
                        fileSize = fileSize / 1024;
                        lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                        lblImageSize.Text = fileSize.ToString() + " KB";
                    }
                    else
                        MessageBox.Show("Error while creaing QC folder");
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show("Error while showing the image " + ex.Message ,"Image error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
		}

        void ShowImage()
        {
            string policyPath;
            string policyName;
            string changedImageName = string.Empty;
            DataSet ds = new DataSet();
            int photoImageName;
            try
            {
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];

                if (policyLst.SelectedItem != null)
                {
                    //Check for policy has photo or not
                    ctrlPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    policy = new wfePolicy(sqlCon, ctrlPolicy);

                    
                    policyPath = policy_Path;
                    //ds	= policy.GetPolicyList();
                    //pageCount = wPolicy.GetPolicyPageCount();
                    //prmPhotoCrop = new System.Windows.Forms.Button();
                    //prmEndPhotoCrop = new System.Windows.Forms.Button();
                    //tssBatch.Text = string.Empty;
                    if (policy.GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
                    {

                        if ((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION)
                        {
                            prmPhotoCrop.Enabled = true;
                            prmEndPhotoCrop.Enabled = true;
                            hasPhotoBol = true;
                            if (File.Exists(policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF") == false)
                            {
                                //tssBatch.Text = "This policy contains photo";
                                lblNote.Visible = true;
                            }
                            else
                            {
                                lblNote.Visible = true;
                                lblNote.Text = "Photo crop has been done, select GetPhoto to check it";
                                lblNote.ForeColor = Color.Green;
                            }
                        }
                        else
                        {
                            prmPhotoCrop.Enabled = false;
                            prmEndPhotoCrop.Enabled = false;
                            hasPhotoBol = false;
                            lblNote.Visible = false;
                        }
                    }
                    else
                    {
                        prmPhotoCrop.Enabled = false;
                        prmEndPhotoCrop.Enabled = false;
                        hasPhotoBol = false;
                        lblNote.Visible = false;
                    }
                    policyName = policyLst.SelectedItem.ToString();
                    changedImageName = imageLst.SelectedItem.ToString();
                    fileMove = new FileorFolder();
                    string sourcePath = policyPath + "\\" + ihConstants._SCAN_FOLDER;
                    string destPath = policyPath + "\\" + ihConstants._QC_FOLDER;
                    sourceFilePath = sourcePath;
                    qcFolderName = destPath;
                    if (FileorFolder.CreateFolder(destPath) == true)
                    {

                        fileMove.MoveFile(sourcePath, destPath, changedImageName, true);

                        imgFileName = destPath + "\\" + changedImageName;


                        if (File.Exists(destPath + "\\" + "0_" + changedImageName))
                        {
                            File.Delete(destPath + "\\" + "0_" + changedImageName);
                        }
                        photoImageName = Convert.ToInt32(changedImageName.Substring(10, 3));
                        if (((imageLst.SelectedIndex + 1) == ihConstants._PHOTO_PAGE_POSITION) && (hasPhotoBol == true) && ihConstants._PHOTO_PAGE_POSITION == photoImageName)
                        {
                            if ((File.Exists(destPath + "\\" + "1_" + changedImageName) == false) && (File.Exists(destPath + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF") == false))
                            {
                                File.Copy(imgFileName, destPath + "\\" + "0_" + changedImageName);
                                img.ConvertTo1Bpp(destPath + "\\" + "0_" + changedImageName, destPath + "\\" + "1_" + changedImageName);
                                File.Move(imgFileName, destPath + "\\" + "2_" + changedImageName);
                                File.Move(destPath + "\\" + "1_" + changedImageName, imgFileName);
                                File.Move(destPath + "\\" + "2_" + changedImageName, destPath + "\\" + "1_" + changedImageName);
                                prmGetPhoto.Enabled = true;
                                prmPhotoCrop.Enabled = true;
                                prmEndPhotoCrop.Enabled = true;
                            }
                            if (File.Exists(destPath + "\\" + imageLst.SelectedItem.ToString().Substring(0, policyLen) + "_000_A.TIF") == true)
                            {
                                prmGetPhoto.Enabled = true;
                            }
                        }
                        else
                        {
                            prmGetPhoto.Enabled = false;
                            prmPhotoCrop.Enabled = false;
                            prmEndPhotoCrop.Enabled = false;
                        }
                        img.LoadBitmapFromFile(imgFileName);
                        AutoSkew();
                        ChangeSize();
                        //pictureControl.Image = img.GetBitmap();
                        System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                        long fileSize = info.Length;
                        fileSize = fileSize / 1024;
                        lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                        lblImageSize.Text = fileSize.ToString() + " KB";
                    }
                    else
                        MessageBox.Show("Error while creaing QC folder");
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show("Error while showing the image " + ex.Message ,"Image error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
        }
        private string GetPolicyPath()
        {
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = wBatch.GetPath(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
            return batchPath + "\\" + wBox.ctrlBox.BoxNumber + "\\" + policyLst.SelectedItem.ToString();
        }
		void BoxDtlsLoaded(object sender, EventArgs e)
		{
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            if (policyLst.SelectedItem != null)
            {
                ctrlPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                policy_Path = GetPolicyPath();
                ShowImage(false);
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                //policyData=(udtPolicy)policy.LoadValuesFromDB();
                //policyPath = GetPolicyPath();
                policyLen = policyLst.SelectedItem.ToString().Length;
                DataSet ds = policy.GetPolicyDetails();
                Label lblName = (Label)BoxDtls.Controls["lblName"];
                if (ds.Tables[0].Rows.Count > 0)
                    lblName.Text = "Name: " + ds.Tables[0].Rows[0]["name_of_policyholder"].ToString();              
            }
			//ChangeSize();
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
                if (pictureControl.Dock != DockStyle.Fill)
                {
                    //pictureControl.Dock = DockStyle.Fill;
                }
                if (!System.IO.File.Exists(imgFileName)) return;
                    Image newImage = img.GetBitmap();
                if (newImage.PixelFormat == PixelFormat.Format1bppIndexed)
                {
                    pictureControl.Image = null;
                	pictureControl.Width = panel1.Width - 2;
                	pictureControl.Height = panel1.Height - 2;
	                if (!System.IO.File.Exists(imgFileName)) return;
	                double scaleX = (double)pictureControl.Width / (double)newImage.Width;
	                double scaleY = (double)pictureControl.Height / (double)newImage.Height;
	                double Scale = Math.Min(scaleX, scaleY);
	                int w = (int)(newImage.Width * Scale);
	                int h = (int)(newImage.Height * Scale);
	                pictureControl.Width = w;
	                pictureControl.Height = h;
                    pictureControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
	                newImage.Dispose();
                }
                else
                {
                	img.LoadBitmapFromFile(imgFileName);
                	pictureControl.Image=img.GetBitmap();
                	pictureControl.SizeMode= PictureBoxSizeMode.StretchImage;
                	//pictureControl.Dock= DockStyle.Fill;
                }
            }
		}
		catch(Exception ex)
		{
            exMailLog.Log(ex);
			MessageBox.Show("Error while" + ex.Message,"Error");
		}
	}
        private void ChangeSize(string fName)
        {
            Image imgTot=null;
            try
            {
                if (img.IsValid() == true)
                {
                    if (pictureControl.Dock != DockStyle.Fill)
                    {
                        //pictureControl.Dock = DockStyle.Fill;
                    }
                    pictureControl.Width = panel1.Width - 2;
                    pictureControl.Height = panel1.Height - 5;
                    if (!System.IO.File.Exists(fName)) return;
                    Image newImage;
                    img.LoadBitmapFromFile(fName);
                    if (img.GetBitmap().PixelFormat == PixelFormat.Format24bppRgb)
                    {
                        img.GetLZW("tmp.TIF");
                        imgTot = Image.FromFile("tmp.TIF");
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
                    pictureControl.Width = w;
                    pictureControl.Height = h-5;
                    pictureControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    if (imgTot != null)
                    {
                        imgTot.Dispose();
                        imgTot = null;
                        if (File.Exists("tmp.tif"))
                            File.Delete("tmp.TIF");
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
                exMailLog.Log(ex);
                MessageBox.Show("Error while cropping the image" + ex.Message, "Crop error");
            }
        }
        private bool GetThumbnailImageAbort()
        {
            return false;
        }

        void BoxDtls_LstDelImgClick(object sender, EventArgs e)
        {
            string delFileName;

            delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
            if (delImgList.Items.Count > 0)
            {
                if (delImgList.SelectedIndex >= 0)
                {
                    EnableDisbleControls(false);
                    delFileName = sourceFilePath + "\\" + ihConstants._DELETE_FOLDER;
                    string[] searchFileName = Directory.GetFiles(delFileName, delImgList.SelectedItem.ToString());
                    //For searching deleted file in deleted folder.
                    if (searchFileName.Length >= 0)
                    {
                        delFileName = searchFileName[0];

                        img.LoadBitmapFromFile(delFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                        ChangeSize(delFileName);
                    }
                }
            }
        }
        void EnableDisbleControls(bool prmControl)
        {
            prmButtonCrop.Enabled=prmControl;
            prmButtonAutoCrp.Enabled=prmControl;
            prmButtonRotateRight.Enabled=prmControl;
            prmButtonRotateLeft.Enabled=prmControl;
            prmButtonZoomIn.Enabled=prmControl; 
            prmButtonZoomOut.Enabled=prmControl;
            prmButtonSkewRight.Enabled=prmControl;
            prmButtonSkewLeft.Enabled=prmControl;
            prmButtonNoiseRemove.Enabled=prmControl;
            prmButtonCleanImg.Enabled=prmControl;
            prmButtonCopyImage.Enabled=prmControl;
            prmButtonDelImage.Enabled=prmControl;
            prmButtonRescan.Enabled=prmControl;
        }
	void BoxDtlsLstDelIamgeInsert(object sender, KeyEventArgs e)
	{
		string delPath=null;
		string sourceFileName=null;
		string delFileName=null;
		string qcFilePath=null;
		int photoPageCount;
		
		if (e.KeyCode == Keys.Insert)
		{
            imageDelLst = (ListBox)BoxDtls.Controls["lstImageDel"];
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            delinsrtBol = true;
            if (imageDelLst.Items.Count > 0)
            {
                pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageDelLst.SelectedItem.ToString(), string.Empty);
                if (UpdateState(eSTATES.PAGE_QC, imageDelLst.SelectedItem.ToString()) == true)
                {
                    delPath = sourceFilePath + "\\" + ihConstants._DELETE_FOLDER;
                    imageDelLst = (ListBox)BoxDtls.Controls["lstImageDel"];
                    sourceFileName = sourceFilePath + "\\" + imageDelLst.SelectedItem.ToString();
                    delFileName = delPath + "\\" + imageDelLst.SelectedItem.ToString();
                    qcFilePath = qcFolderName + "\\" + imageDelLst.SelectedItem.ToString();
                    photoPageCount = 100; //Convert.ToInt32(imageDelLst.SelectedItem.ToString().Substring(10, 3));
                    if (File.Exists(delFileName) == true)
                    {
                        File.Move(delFileName, sourceFileName);
                        if (ihConstants._PHOTO_PAGE_POSITION == photoPageCount)
                        {
                            img.ConvertTo1Bpp(sourceFileName, qcFilePath);
                        }
                        else
                        {
                            File.Copy(sourceFileName, qcFilePath);
                        }
                    }

                    BoxDtls.InsertNotify(imageLst.SelectedIndex);
                    EnableDisbleControls(true);

                    if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                    {
                        delImgList.SetSelected(delImgList.SelectedIndex, false);
                    }
                }
            }
		}
	} 
	void MarkExceptionToolStripMenuItemClick(object sender, System.EventArgs e)
		{
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			imageLst = (ListBox)BoxDtls.Controls["lstImage"];
			pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()),imageLst.SelectedItem.ToString(),string.Empty);
		    wfeImage wImage  = new wfeImage(sqlCon, pImage);
		    aeCustomExp frmExp=new aeCustomExp(wImage,sqlCon,crd);
			//frmExp.MdiParent=this;
//			frmExp.Height = this.ClientRectangle.Height;
//			frmExp.Width=this.ClientRectangle.Width;
			frmExp.ShowDialog(this);
		}
	void BoxDtlsBoxMouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Point pt = new Point();
			pt.X = e.X;
			pt.Y = e.Y;
			if(e.Button == MouseButtons.Right)
			{
				contextMenuStrip1.Show(BoxDtls,pt);
			}
		}

        private void aeImageQC_KeyDown(object sender, KeyEventArgs e)
        {
        	config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            if (e.KeyCode == Keys.F5)
            {
                UpdateAllPolicyStatus();
                wfePolicy wPolicy = new wfePolicy(sqlCon);
                int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_QC);
                this.Text = "Image Quality Control";
                this.Text = this.Text + "                                       Today you have done " + count + " ";
                BoxDtls.RefreshNotify();
                if (policyLst.Items.Count > 0)
                {
                    //ShowImage(false);
                }
                else
                {
                    imageLst.Items.Clear();
                    if (policyLst.Items.Count == 0)
                    {
                        EnableDisbleControls(false);
                        prmEndPhotoCrop.Enabled = false;
                        prmGetPhoto.Enabled = false;
                        prmPhotoCrop.Enabled = false;
                        pictureControl.Image = null;
                    }
                }
            }
            if (e.KeyCode == Keys.Space)
            {
            	if ((this.BoxDtls.ActiveControl==null) || (this.BoxDtls.ActiveControl.Name!="cmdNext" && this.BoxDtls.ActiveControl.Name!="cmdPrevious"))
            	{
	            	object o = new object();
	            	EventArgs a = new KeyEventArgs(Keys.None);
	            	BoxDtlsNextClicked(o, a);
            	}
            	/*
	            imageLst = imageLst = (ListBox)BoxDtls.Controls["lstImage"];
	            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
	
	            
	            if (imageLst.Items.Count > 0)
	            {
	                EnableDisbleControls(true);
	            }
	            delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
	            if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
	            {
	                delImgList.SetSelected(delImgList.SelectedIndex, false);
	            }
				
				if(imageLst.Items.Count>0)
				{
					UpdateState(eSTATES.PAGE_QC,imageLst.SelectedItem.ToString());
				}
				BoxDtls.MoveNext();
	            ShowImage(false);//added in version 1.0.0.1
            	*/
            }
			char DELETE = Convert.ToChar(config.GetValue(ihConstants.IMAGE_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
			char rslt;
			if(char.TryParse(e.KeyCode.ToString(),out rslt))
			{
				if (Convert.ToChar(e.KeyCode.ToString().ToUpper()) == DELETE)
			    {
	           		ImageDelete();
	           		return;
	            }
			}
            if ((e.KeyCode == Keys.Z) && (e.Control))
            {
                ImageCopy();
            }
        }
 }
}
