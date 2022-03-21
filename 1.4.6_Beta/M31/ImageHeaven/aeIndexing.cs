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

namespace ImageHeaven
{
	/// <summary>
	/// Description of aeIndexing.
	/// </summary>
	public partial class aeIndexing : Form, StateData
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
		
		  
		//private DummyToolbox m_toolbox = new DummyToolbox();
		private OdbcConnection sqlCon=null;
		private bool m_bSaveLayout = true;
		private DeserializeDockContent m_deserializeDockContent;
		private FloatToolbox m_toolbox = new FloatToolbox();
		//private MagickNet.Image imgQc;
		private string imgFileName=null;
		//private ImageQC objQc=new ImageQC();
        NovaNet.Utils.ImageManupulation delImage;
        private wfeBox wBox=null;
        private CtrlPolicy pPolicy=null;
        private CtrlImage pImage=null;
        private static string docType;
		private CtrlBox pBox=null;
		private string indexFilePath=null;
		NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
		

		bool hasPhotoBol;
		bool pageDelInsrt;
		string policyPath=string.Empty;
		/// <summary>
		/// For drawing rectangle
		/// </summary>
		private int	cropX;
    	private int cropY;
    	private int cropWidth;
    	private int cropHeight;
        private double constRotateAngle;
		private double skewXAngle;
		private double skewYAngle;
		private long fileSize;
    	private int OperationInProgress;
        private bool IndexingOperation = false;
        private ListBox delImgList = null;
    	//private Bitmap cropBitmap;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	
    	private Pen cropPen;
    	private int cropPenSize = 1;
    	private System.Drawing.Color cropPenColor= System.Drawing.Color.Blue;

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

        private int zoomWidth;
        private int zoomHeight;
        private Size zoomSize = new Size();
        private int keyPressed=1;
        private ImageConfig config = null;
        MemoryStream stateLog;
        byte[] tmpWrite;
        //GD objects
        //private GdPicture.GdPictureImaging imgQc = new GdPicture.GdPictureImaging();
		//private int ino;
        private Label lblImageSize = null;

        private ListBox policyLst= null;
		private ListBox imageLst = null;
		private ListBox imageDelLst = null;
		private CtrlPolicy ctrlPolicy =null;
		private wfePolicy policy = null;
		private udtPolicy policyData=null;
		private FileorFolder fileMove=null;
		private string sourceFilePath=null;
		private string indexFolderName=null;
        private Credentials crd = new Credentials();
        private int policyLen = 0;
		private Imagery img;
		    
		public struct IconInfo
		{
		  public bool fIcon;
		  public int xHotspot;
		  public int yHotspot;
		  public IntPtr hbmMask;
		  public IntPtr hbmColor;
		}
    	
    	[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);

		[DllImport("user32.dll")]
		public static extern IntPtr CreateIconIndirect(ref IconInfo icon);
    	
		public aeIndexing(wfeBox prmBox,OdbcConnection prmCon,Credentials prmCrd)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			sqlCon=prmCon;
			wBox = prmBox;
            crd = prmCrd;
			InitializeComponent();

			img = IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
			//img = IgrFactory.GetImagery(Constants.IGR_GDPICTURE);
			
			m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
			this.Text="Image Indexing";
            exMailLog.SetNextLogger(exTxtLog);
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public aeIndexing()
		{			
			InitializeComponent();			
			
			m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
			this.Text="Image Indexing";
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
		private static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
		{
		  IntPtr ptr = bmp.GetHicon();
		  IconInfo tmp = new IconInfo();
		  GetIconInfo(ptr, ref tmp);
		  tmp.xHotspot = xHotSpot;
		  tmp.yHotspot = yHotSpot;
		  tmp.fIcon = false;
		  ptr = CreateIconIndirect(ref tmp);
		  return new Cursor(ptr);
		}

		void AeIndexingLoad(object sender, EventArgs e)
		{
            try
            {
                if(lblImageSize != null)
                    lblImageSize.ForeColor = Color.Black;

                System.Windows.Forms.ToolTip bttnToolTip = new System.Windows.Forms.ToolTip();
                ReadConfigKey();

                string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
                ArrayList arrPolicy = new ArrayList();

                wQuery pQuery = new ihwQuery(sqlCon);

                if (File.Exists(configFile))
                    dockPanel.LoadFromXml(configFile, m_deserializeDockContent);

                delImage = new NovaNet.Utils.ImageManupulation(CropRegister);
                prmButtonCrop.Text = "Crop";
                bttnToolTip.SetToolTip(prmButtonCrop, "Shortcut Key-" + cropKey);
                m_toolbox.AddButton(prmButtonCrop, delImage);
                prmButtonCrop.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(AutoCrop);
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
                //			System.Windows.Forms.Button prmButtonSkewLeft = new System.Windows.Forms.Button();
                //			prmButtonSkewLeft.Text="Skew Left";
                //            m_toolbox.AddButton(prmButtonSkewLeft,delImage);

                delImage = new NovaNet.Utils.ImageManupulation(NoiseRemove);
                bttnToolTip.SetToolTip(prmButtonNoiseRemove, "Shortcut Key-" + noiseRemovalLKey);
                prmButtonNoiseRemove.Text = "Despacle";
                prmButtonNoiseRemove.AutoSize = true;
                m_toolbox.AddButton(prmButtonNoiseRemove, delImage);
                prmButtonNoiseRemove.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(CleanImageRegister);
                bttnToolTip.SetToolTip(prmButtonCleanImg, "Shortcut Key-" + cleanKey);
                prmButtonCleanImg.Text = "Clean";
                prmButtonCleanImg.AutoSize = true;
                m_toolbox.AddButton(prmButtonCleanImg, delImage);
                prmButtonCleanImg.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(ImageCopy);
                bttnToolTip.SetToolTip(prmButtonCopyImage, "Shortcut Key-(Control+z)");
                prmButtonCopyImage.Text = "Copy Original";
                prmButtonCopyImage.AutoSize = true;
                m_toolbox.AddButton(prmButtonCopyImage, delImage);
                prmButtonCopyImage.ForeColor = Color.Black;

                delImage = new NovaNet.Utils.ImageManupulation(ImageDelete);
                bttnToolTip.SetToolTip(prmButtonDelImage, "Shortcut Key-" + deleteKey);
                prmButtonDelImage.Text = "Delete";
                prmButtonDelImage.AutoSize = true;
                m_toolbox.AddButton(prmButtonDelImage, delImage);
                prmButtonDelImage.ForeColor = Color.Black;
                //delImage = new NovaNet.Utils.ImageManupulation(RescanImage);
                //prmButtonRescan.Text="Rescan";
                //prmButtonRescan.AutoSize=true;
                //m_toolbox.AddButton(prmButtonRescan,delImage); // should be added later

                txtRegional.Text = string.Empty;
                this.WindowState = FormWindowState.Maximized;
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                if (policyLst.SelectedItem != null)
                {
                    //wfePolicy wPolicy = new wfePolicy(sqlCon);
                    //int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_INDEXED);
                    this.Text = this.Text;// +"                                       Today you have done " + count + " ";
                    BoxDtls.PopulateDelList(Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    markNotReadyHoldPolicyToolStripMenuItem.Visible = false;
                    markReadyToolStripMenuItem.Visible = false;
                    imageLst.Enabled = true;
                    imageLst.SelectionMode = SelectionMode.One;
                    DisplayDockTypes();
                    DisplayDocTypeCount();

                    DisplayName();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while loading indexing form... " + ex.Message.ToString());
            }
		}

        private void DisplayName()
        {
            txtRegional.Text = string.Empty;
            Label keyno = (Label)BoxDtls.Controls["lblNRCNo"];
            pPolicy = new CtrlPolicy(0, 0, 0, 0);
            wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
            txtRegional.Text = wPolicy.Search(keyno.Text, txtName.Text.Trim());
            this.txtRegional.AutoCompleteCustomSource = wPolicy.Search(txtName.Text.Trim());
            this.txtRegional.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtRegional.AutoCompleteSource = AutoCompleteSource.CustomSource;
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
				sourcePath = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
				destinationPath = sourceFilePath + "\\" + imageLst.SelectedItem.ToString();
				aeScanControl scControl=new aeScanControl(imageLst.SelectedItem.ToString(),indexFolderName,sourcePath,destinationPath);
				scControl.ShowDialog(this);
				imgFileName = sourcePath;
				
				//Open the source file
				img.LoadBitmapFromFile(imgFileName);
				//Show the image back in picture box
				pictureControl.Image = img.GetBitmap();
			}
			pageDelInsrt = false;
			return 0;
		}
		void AeIndexingFormClosing(object sender, FormClosingEventArgs e)
		{
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            UpdateAllPolicyStatus();
		}

        public void aeIndexing_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                panel1.Left = BoxDtls.Right +2 ;
                panel1.Width = (panel2.Left - BoxDtls.Right)-2 ;
                panel1.Top = panel2.Top;
                panel1.Height = this.ClientSize.Height-15;
                pictureControl.Height = panel1.Height - 3;
            }
            //pictureControl.Width = this.ClientSize.Width;
        }

        private void UpdateAllPolicyStatus()
        {
            string photoPath = null;
            wfeImage wImage;
            string changedImageName;
            System.IO.FileInfo info;
            long fileSize;
            wfePolicy wPolicy;
            wfeBox box;
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            
            try
            {
                if (policyLst.Items.Count > 0)
                {
                    for (int i = 0; i < policyLst.Items.Count; i++)
                    {
                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.Items[i].ToString()), string.Empty, string.Empty);
                        wImage = new wfeImage(sqlCon, pImage);
                        pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.Items[i].ToString()));
                        wPolicy = new wfePolicy(sqlCon, pPolicy);
                        if ((wImage.GetImageCount(eSTATES.PAGE_QC) == false)) // && (wImage.GetImageCount(eSTATES.PAGE_ON_HOLD) == false))
                        {
                            crd.created_dttm = dbcon.GetCurrenctDTTM(1, sqlCon);
                            wPolicy.UpdateStatus(eSTATES.POLICY_INDEXED, crd, true);
                            wPolicy.UnLockPolicy();
                            ///update into transaction log
                            wPolicy.UpdateTransactionLog(eSTATES.POLICY_INDEXED, crd);
                        }

                        pBox = new CtrlBox(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber);
                        box = new wfeBox(sqlCon, pBox);
                        NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[2];
                        state[0] = NovaNet.wfe.eSTATES.POLICY_QC;
                        state[1] = NovaNet.wfe.eSTATES.POLICY_SCANNED;
                        if (wPolicy.GetPolicyCount(state) == 0)
                        {
                            box.UpdateStatus(eSTATES.BOX_INDEXED);
                        }

                        //NovaNet.wfe.eSTATES[] boxState = new NovaNet.wfe.eSTATES[1];
                        //boxState[0] = NovaNet.wfe.eSTATES.BOX_SCANNED;

                        //wBatch = new wfeBatch(sqlCon);
                        //if (box.GetBoxCount(boxState) == 0)
                        //{
                        //    ///Update the batch status
                        //    wBatch.UpdateStatus(eSTATES.BATCH_QC, wBox.ctrlBox.BatchKey);
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
                        policyData = (udtPolicy)wPolicy.LoadValuesFromDB();
                        policyPath = policyData.policy_path;
                        if (Directory.Exists(policyPath + "\\" + ihConstants._INDEXING_FOLDER))
                        {
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.Items[i].ToString()), policyLst.Items[i].ToString() + "_000_A.TIF", string.Empty);
                            wImage = new wfeImage(sqlCon, pImage);
                            changedImageName = wImage.GetIndexedImageName();
                            if (changedImageName == string.Empty)
                            {
                                photoPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + policyLst.Items[i].ToString() + "_000_A.TIF";
                            }
                            else
                            {
                                photoPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + changedImageName;
                            }
                            if (File.Exists(photoPath))
                            {
                                //Open the source file
                                img.LoadBitmapFromFile(photoPath);
                                //Show the image back in picture box
                                img.SaveFile(photoPath);
                                info = new System.IO.FileInfo(photoPath);
                                fileSize = info.Length;
                                fileSize = fileSize / 1024;
                                wImage.UpdateImageSize(crd, eSTATES.PAGE_INDEXED, fileSize);
                            }
                        }
                        */
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error - " + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + photoPath + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
            }
        }
		int Crop(Rectangle udtRect)
		{

			Bitmap bmpImage = new Bitmap(pictureControl.Image);
			double htRatio = 0;
			double wdRatio = 0;
            DateTime st = DateTime.Now;
            DateTime end = DateTime.Now;
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
				}
				ChangeSize();
				
				System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
				fileSize = info.Length;
				fileSize = fileSize / 1024;
                lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImageSize.Text = fileSize.ToString() + " KB";
				UpdateImageSize(fileSize);
				pageDelInsrt = false;
                end = DateTime.Now;
                TimeSpan duration = end - st;
                //MessageBox.Show("Total- " + duration.Milliseconds.ToString());
			}
		
			catch(Exception ex)
			{
				MessageBox.Show("Error while cropping the image","Crop Error");
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + imgFileName + "\n");
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
						//pictureControl.Image = img.GetBitmap();
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
					//delinsrtBol = false;
	
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
			string originalFile=null;
            string scanPath = null;
			int pos;
            wfeImage wImage;

			try
			{
				qcDelPath=policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;	

				imageLst= (ListBox)BoxDtls.Controls["lstImage"];
				delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
                pageDelInsrt = true;
                pos = imageLst.SelectedItem.ToString().IndexOf("-");
                if (pos <= 0)
                {
                    destFileName = imageLst.SelectedItem.ToString();
                    originalFile = destFileName;
                }
                else
                {
                    pos = imageLst.SelectedItem.ToString().IndexOf(".TIF");
                    destFileName = imageLst.SelectedItem.ToString().Substring(0, pos) + imageLst.SelectedItem.ToString().Substring(pos + 4) + ".TIF";
                    originalFile = imageLst.SelectedItem.ToString().Substring(0, pos) + ".TIF";
                }
                scanPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + originalFile;
                pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageLst.SelectedItem.ToString(), string.Empty);
                if (imageLst.SelectedItem.ToString().IndexOf("Signature_page") <= 0)
                {
                    if (UpdateState(eSTATES.PAGE_DELETED, originalFile, Convert.ToInt32(policyLst.SelectedItem.ToString())) == true)
                    {

                        sourceFileName = indexFolderName + "\\" + destFileName;
                        destFileName = qcDelPath + "\\" + originalFile;
                        qcFilePath = sourceFilePath + "\\" + originalFile;
                        if (FileorFolder.CreateFolder(qcDelPath) == true)
                        {
                            if (File.Exists(destFileName) == false)
                            {
                                File.Move(sourceFileName, destFileName);
                                File.Delete(scanPath);
                            }
                        }


                        
                    }
                }
                else
                {
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), originalFile, string.Empty);
                    wImage = new wfeImage(sqlCon, pImage);
                    if (wImage.DeleteImage() == true)
                    {
                        sourceFileName = indexFolderName + "\\" + destFileName;
                        destFileName = qcDelPath + "\\" + originalFile;
                        qcFilePath = sourceFilePath + "\\" + originalFile;
                        if (FileorFolder.CreateFolder(qcDelPath) == true)
                        {
                            if (File.Exists(destFileName) == false)
                            {
                                File.Delete(sourceFileName);
                                File.Delete(scanPath);
                            }
                        }
                    }
                }
                BoxDtls.DeleteNotify(imageLst.SelectedIndex);
                DisplayDocTypeCount();
                ShowImage(true);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error while doing the operation..." + ex.Message);
			}
		    return 0;
		}
		void UpdateImageSize(long prmSize)
		{
			//string photoName;
			wfeImage img;
			//long fileSize;
			//System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
			
				policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
				imageLst = (ListBox)BoxDtls.Controls["lstImage"];
				
				pImage = new CtrlImage(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey,wBox.ctrlBox.BoxNumber,Convert.ToInt32(policyLst.SelectedItem.ToString()),imageLst.SelectedItem.ToString(),string.Empty);
				img = new wfeImage(sqlCon,pImage);
				img.UpdateImageSize(crd, eSTATES.PAGE_QC,prmSize);
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
            string path;
            string changedImageName = string.Empty;
            string scanImageName = string.Empty;
            wfeImage wImage = null;
            int pos;
            string sPath;
            string iPath;
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
            
            path = policyPath + "\\" + ihConstants._SCAN_FOLDER;
            sPath = path + "\\" + scanImageName;

            path = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
            iPath = path + "\\" + changedImageName;
            if (File.Exists(sPath))
            {
                File.Copy(sPath, iPath, true);
                img.LoadBitmapFromFile(iPath);
                ChangeSize(iPath);
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
					//pictureControl.Image = img.GetBitmap();
                    img.LoadBitmapFromFile(imgFileName);
				}
	            ChangeSize();
	            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
				fileSize = info.Length;
				fileSize = fileSize / 1024;
                lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                lblImageSize.Text = fileSize.ToString() + " KB";
				UpdateImageSize(fileSize);
				pageDelInsrt = false;
        	}
        	catch(Exception ex)
			{
				MessageBox.Show("Error while auto cropping the image","Auto Crop Error");
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
			pageDelInsrt = false;
			return 0;
		}
        int CleanImageRegister()
        {
            OperationInProgress = ihConstants._CLEAN;
            pageDelInsrt = false;
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
                    //pictureControl.Dock = DockStyle.None;
        		}
        	}
        	catch(Exception ex)
        	{
        		MessageBox.Show("Error while zooming the image" + ex.Message,"Zoom Error");
                exMailLog.Log(ex);
        	}
        	return 0;
        }
        private void ChangeZoomSize()
        {
            if (!System.IO.File.Exists(imgFileName)) return;
            if(img.GetBitmap().PixelFormat== PixelFormat.Format1bppIndexed)
            {
            	pictureControl.SizeMode= PictureBoxSizeMode.Normal;
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
            else
            {
            	img.LoadBitmapFromFile(imgFileName);
            	pictureControl.Image=img.GetBitmap();
            	pictureControl.SizeMode= PictureBoxSizeMode.StretchImage;
            }
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
                    //pictureControl.Dock = DockStyle.None;
	            }
        	}
            catch(Exception ex)
        	{
        		MessageBox.Show("Error while zooming the image" + ex.Message,"Zoom Error");
                exMailLog.Log(ex);
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
                    img.LoadBitmapFromFile(imgFileName);
					//Show the image back in picture box
					//pictureControl.Image = img.GetBitmap();
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
                    img.LoadBitmapFromFile(imgFileName);
					//Show the image back in picture box
					//pictureControl.Image = img.GetBitmap();
					ChangeSize();
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
                    img.LoadBitmapFromFile(imgFileName);
					//Show the image back in picture box
					//pictureControl.Image = img.GetBitmap();
					ChangeSize();
					System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
					fileSize = info.Length;
					fileSize = fileSize / 1024;
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
            deleteKey = config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim();
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
            if (!(this.ActiveControl is TextBox))
            {

                if ((int)e.KeyData == cropKeyVal)
                {
                    OperationInProgress = ihConstants._CROP;
                }
                if ((int)e.KeyData == (zoomInKeyVal + 64))
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
                    OperationInProgress = ihConstants._CLEAN;
                }
                if (e.KeyCode == Keys.Escape)
                {
                    OperationInProgress = ihConstants._NO_OPERATION;
                    Cursor = Cursors.No;
                    Cursor = Cursors.Default;
                    pictureControl.Cursor = Cursors.Default;
                }
                if (e.KeyCode == Keys.F11)
                {
                    BoxDtls.MoveUp();
                }
                if (e.KeyCode == Keys.F12)
                {
                    BoxDtls.MoveDown();
                }

                if (e.KeyCode == Keys.Right)
                {
                    IndexingOperation = false;

                    //BoxDtls.MoveNext();
                    EnableDisbleControls(true);
                    ShowImage(false);
                    DisplayDocTypeCount();
                }
                if (e.KeyCode == Keys.Left)
                {
                    //BoxDtls.MovePrevious();
                    EnableDisbleControls(true);
                    ShowImage(false);
                }
                if (e.KeyCode == Keys.Up)
                {
                    //BoxDtls.MovePrevious();
                    EnableDisbleControls(true);
                    ShowImage(false);
                    ChangeSize();
                }
                if (e.KeyCode == Keys.Down)
                {
                    //BoxDtls.MoveNext();
                    EnableDisbleControls(true);
                    ShowImage(false);
                    ChangeSize();
                    DisplayDocTypeCount();
                }
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
                Cursor=Cursors.Cross;
                if ((pictureControl.Image!=null))
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
		void StatusStrip1ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			
		}		

		void BoxDtlsPolicyChanged(object sender, EventArgs e)
		{
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if (policyLst.SelectedItem != null)
            {
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                if (policy.GetPolicyStatus() != (int)eSTATES.POLICY_ON_HOLD)
                {
                    
                    EnableDisbleControls(true);
                    policyData = (udtPolicy)policy.LoadValuesFromDB();
                    indexFolderName = policyData.policy_path + "\\" + ihConstants._INDEXING_FOLDER;
                    BoxDtls.PopulateDelList(Convert.ToInt32(policyLst.SelectedItem.ToString()));
                    ShowImage(false);
                    ShowPolicyDetails();
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
                else
                {
                    EnableDisbleControls(false);
                }
                lblResult.Text = string.Empty;
                DisplayName();
            }
		}

        private bool PersonalValidation()
        {
            bool valid = true;
            string yr;
            string mm;
            string dd;
            try
            {

                if (txtRegional.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Regional Name field is empty, input a valid name of the policy holder...");
                    txtName.Focus();
                    valid = false;
                }
            }
            catch (Exception ex)
            {
                valid = false;
            }
            return valid;
        }

        void UpdateAllImageList()
        {
            string originalFileName = null;
            int pos = 0;
            int policyNumber;

            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if (policyLst.Items.Count > (policyLst.SelectedIndex + 1))
            {
                policyNumber = Convert.ToInt32(policyLst.Items[policyLst.SelectedIndex - 1].ToString());
            }
            else
            {
                policyNumber = Convert.ToInt32(policyLst.Items[policyLst.SelectedIndex].ToString());
            }
            originalFileName = GetFileName(imageLst.SelectedItem.ToString(), docType);
            pos = originalFileName.ToString().IndexOf(".TIF");
            //indexFileName = selImageName.Substring(0, pos) + "-" + docType + ".TIF";
            //UpdateState(eSTATES.PAGE_INDEXED, originalFileName, docType, indexFileName, Convert.ToInt32(policyLst.SelectedItem.ToString()));
        }
		void ShowPolicyDetails()
		{
			
			DataSet ds=new DataSet();
			
			cmdUpdate.Enabled=true;
			txtName.Enabled=false;
			txtCommDt.Enabled=false;
			txtDOB.Enabled=false;
            txtName.Text = string.Empty;
            txtCommDt.Text = string.Empty;
            txtDOB.Text = string.Empty;
			policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
			imageLst= (ListBox)BoxDtls.Controls["lstImage"];
			ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey),Convert.ToInt32(wBox.ctrlBox.BoxNumber),Convert.ToInt32(policyLst.SelectedItem.ToString()));
		    policy = new wfePolicy(sqlCon, ctrlPolicy);
		    //policyData=(udtPolicy)policy.LoadValuesFromDB();
		    policyPath=GetPolicyPath();
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
        /*
		void BoxDtlsImageChanged(object sender, EventArgs e)
		{
			string imageName;
            int pos = 0;
            string docType = string.Empty;
            DataSet ds = new DataSet();

            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            ///This section is used after index operation
            if (IndexingOperation == true)
            {
                
            }
            else
            {
                ShowImage(false);
                ChangeSize();
                
                DateTime st = DateTime.Now;

                if (imageLst.SelectedItem != null)
                {
                    pos = imageLst.SelectedItem.ToString().IndexOf("-");
                    if (pos > 0)
                    {
                        imageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                    }
                    else
                    {
                        imageName = imageLst.SelectedItem.ToString();
                    }
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageName, string.Empty);
                    wfeImage wImage = new wfeImage(sqlCon, pImage);

                    ds = wImage.GetCustomException(ihConstants._NOT_RESOLVED);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        toolStripStatusLabel1.Text = "This image has some custom exception";
                        string custExcp = string.Empty;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            custExcp = custExcp + " - " + ds.Tables[0].Rows[i]["problem_type"].ToString();
                        }
                        toolStripStatusLabel2.Text = "Exception Details" + custExcp;
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = string.Empty;
                        toolStripStatusLabel2.Text = string.Empty;
                    }
                    DateTime end = DateTime.Now;
                    TimeSpan duration = end - st;
                    //MessageBox.Show(duration.Milliseconds.ToString());
                }
            }
		}
         */
		void MarkExceptionToolStripMenuItemClick(object sender, System.EventArgs e)
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
		    aeCustomExp frmExp=new aeCustomExp(wImage,sqlCon,crd);
		    frmExp.ShowDialog(this);
		}
        void HoldPolicyToolStripMenuItemClick(object sender, EventArgs e)
        {
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];


            string policyNo = policyLst.SelectedItem.ToString();
            try
            {
                if (policyNo != string.Empty)
                {
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo));
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
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating policy status......" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void markReadyToolStripMenuItemClick(object sender, EventArgs e)
        {
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            string policyNo = policyLst.SelectedItem.ToString();
            try
            {
                if (policyNo != string.Empty)
                {
                    ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo));
                    policy = new wfePolicy(sqlCon, ctrlPolicy);
                    if (policy.UpdateStatus(eSTATES.POLICY_INDEXED, crd))
                    {
                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), string.Empty, string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);
                        wImage.TotalImageUpdateStatus(eSTATES.PAGE_INDEXED);
                        EnableDisbleControls(true);
                    }
                }
                contextMenuStrip1.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating policy status...." + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
		void BoxDtlsNextClicked(object sender, EventArgs e)
		{
            DialogResult rlst;
            DateTime stdt = DateTime.Now;
            if (PersonalValidation())
            {
                imageLst = imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];

                IndexingOperation = false;
                if (imageLst.Items.Count > 0)
                {
                    EnableDisbleControls(true);
                }
                delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
                if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                {
                    delImgList.SetSelected(delImgList.SelectedIndex, false);
                }
                if (imageLst.Items.Count == (imageLst.SelectedIndex + 1))
                {
                    if (MandatoryDocTypeChecking() == false)
                    {
                        rlst = MessageBox.Show(this, "Mandatory document missing, do you want to proceed?", "Missing", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (rlst == DialogResult.Yes)
                        {
                            BoxDtls.MoveNext(true);
                        }
                    }
                    else
                    {
                        BoxDtls.MoveNext(true);
                    }
                }
                else
                {
                    BoxDtls.MoveNext(true);
                }
                ShowImage(false);
                DateTime eddt = DateTime.Now;
                TimeSpan sp = eddt - stdt;
                //MessageBox.Show(sp.Milliseconds.ToString());
                //DisplayDocTypeCount();
            }
		}
		void UpdateState(eSTATES prmPageSate,string prmPageName,string prmDocType,string prmIndexImageName,int prmPolicyNumber)
		{
			NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
			
			pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode),Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()),Convert.ToInt32(wBox.ctrlBox.BoxNumber),prmPolicyNumber,prmPageName,string.Empty);
		    wfeImage wImage  = new wfeImage(sqlCon, pImage);
		    wImage.UpdateStatusAndDockType(prmPageSate,prmDocType,prmIndexImageName,crd);
		    
		    //imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            //if(pageDelInsrt == false)
            //{
            //    System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);
				
            //    fileSize = info.Length;
            //    fileSize = fileSize / 1024;
            //    wImage.UpdateImageSize(crd,eSTATES.PAGE_INDEXED,fileSize);
				
            //}
		    
		    
		}
        bool UpdateState(eSTATES prmPageSate, string prmPageName, int prmPolicyNumber)
        {
            bool delBol = false;
            double fileSize;
            NovaNet.Utils.dbCon dbcon = new NovaNet.Utils.dbCon();

            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), prmPolicyNumber, prmPageName, string.Empty);
            wfeImage wImage = new wfeImage(sqlCon, pImage);
            if (wImage.UpdateStatus(prmPageSate, crd) == true)
            {
                delBol = true;
            }
            
            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            if (pageDelInsrt == false)
            {
                System.IO.FileInfo info = new System.IO.FileInfo(indexFilePath);

                fileSize = info.Length;
                fileSize = fileSize / 1024;
                wImage.UpdateImageSize(crd, eSTATES.PAGE_INDEXED, fileSize);

            }

            return delBol;
        }
        /*
        void ShowImage(bool prmOverWrite, string pFPath)
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
                    if (Directory.Exists(destPath) == false)
                    {
                        //Directory.CreateDirectory(destPath);
                        FileorFolder.RenameFolder(sourcePath, destPath);
                    }
                    if (pos <= 0)
                    {
                        //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                    }
                    //prmButtonRescan.Enabled = true;
                    //prmButtonSkewRight.Enabled = true;
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
                            //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
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
         */
        private string GetPolicyPath()
        {
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            wfeBatch wBatch = new wfeBatch(sqlCon);
            string batchPath = wBatch.GetPath(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
            return batchPath + "\\" + wBox.ctrlBox.BoxNumber + "\\" + policyLst.SelectedItem.ToString();
        }
		void ShowImage(bool prmOverWrite)
		{
			string policyName;
			string changedImageName=string.Empty;
			string photoImageName=null;
			wfeImage wImage =null;
			
			int pos;
			//((ListBox)BoxDtls.Controls["lstPolicy"]).GetItemText();
			try
			{
				policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
				imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                if (policyLst.SelectedItem != null)
                {
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
                        string sourcePath = policyPath + "\\" + ihConstants._QC_FOLDER;
                        string destPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER;
                        sourceFilePath = sourcePath;
                        indexFolderName = destPath;
                        
                        if (FileorFolder.RenameFolder(sourceFilePath,destPath) == true)
                        {
                            if (pos <= 0)
                            {
                                //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                            }
                            imgFileName = destPath + "\\" + changedImageName;
                            //Open the source file
                            //img.LoadBitmapFromFile(imgFileName);
                            //Show the image back in picture box
                            //pictureControl.Image = img.GetBitmap();

                            prmButtonRescan.Enabled = true;
                            prmButtonSkewRight.Enabled = true;
                            if (hasPhotoBol == true)
                            {
                                if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos <= 0))
                                {
                                    imgFileName = destPath + "\\" + changedImageName;
                                    //Open the source file
                                    img.LoadBitmapFromFile(imgFileName);
                                    //Show the image back in picture box
                                    //pictureControl.Image = img.GetBitmap();
                                    prmButtonRescan.Enabled = false;
                                    //prmButtonSkewRight.Enabled = false;
                                }
                                else if ((changedImageName.Substring(policyLen, 6) == "_000_A") && (pos > 0))
                                {
                                    photoImageName = imageLst.Items[0].ToString().Substring(0, pos);
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
                                    //prmButtonSkewRight.Enabled = false;
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
                                        //Open the source file
                                        img.LoadBitmapFromFile(imgFileName);
                                        //Show the image back in picture box
                                        //pictureControl.Image = img.GetBitmap();
                                    }
                                    else
                                    {
                                        imgFileName = destPath + "\\" + changedImageName;

                                        //Open the source file
                                        img.LoadBitmapFromFile(imgFileName);
                                        //Show the image back in picture box
                                        //pictureControl.Image = img.GetBitmap();
                                    }
                                }
                            }
                            else
                            {
                                if (pos > 0)
                                {
                                    photoImageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                                    //fileMove.MoveFile(sourcePath, destPath, changedImageName, prmOverWrite);
                                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), photoImageName, string.Empty);
                                    wImage = new wfeImage(sqlCon, pImage);
                                    changedImageName = wImage.GetIndexedImageName();
                                }
                                imgFileName = destPath + "\\" + changedImageName;
                                img.LoadBitmapFromFile(imgFileName);
                            }
                            pictureControl.Refresh();
                            System.IO.FileInfo info = new System.IO.FileInfo(imgFileName);
                            long fileSize = info.Length;
                            fileSize = fileSize / 1024;
                            lblImageSize = (Label)BoxDtls.Controls["lblImageSize"];
                            lblImageSize.Text = fileSize.ToString() + " KB";
                        }
                        else
                            MessageBox.Show("Error while creaing index folder");
                    }
                    ChangeSize();
                }
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
        private bool GetThumbnailImageAbort()
        {
            return false;
        }
		void BoxDtlsLoaded(object sender, EventArgs e)
		{
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
            if (policyLst.SelectedItem != null)
            {
                ctrlPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);
                ShowImage(false);
                ShowPolicyDetails();
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
		}
		
		void PictureControlBackgroundImageChanged(object sender, System.EventArgs e)
		{
		}
	private void ChangeSize()
	{
		try
		{
			if (img.IsValid() == true)
			{
                if (!System.IO.File.Exists(imgFileName)) return;
                    Image newImage = img.GetBitmap();
                if (newImage.PixelFormat == PixelFormat.Format1bppIndexed)
                {
                    pictureControl.Image = null;
                    pictureControl.Width = panel1.Width - 2;
                    pictureControl.Height = panel1.Height - 2;
                    
                    double scaleX = (double)pictureControl.Width / (double)newImage.Width;
                    double scaleY = (double)pictureControl.Height / (double)newImage.Height;
                    double Scale = Math.Min(scaleX, scaleY);
                    int w = (int)(newImage.Width * Scale);
                    int h = (int)(newImage.Height * Scale);
                    pictureControl.Width = w;
                    pictureControl.Height = h;
                    pictureControl.Image = CreateThumbnail(newImage, w, h); //CreateThumbnail(imgFileName, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    newImage.Dispose();
				}
				else
				{
					pictureControl.Width = panel1.Width - 2;
	                pictureControl.Height = panel1.Height-2;
					img.LoadBitmapFromFile(imgFileName);
                	pictureControl.Image=img.GetBitmap();
                	pictureControl.SizeMode= PictureBoxSizeMode.StretchImage;
				}
			}
		}
		catch(Exception ex)
		{
            exMailLog.Log(ex);
			MessageBox.Show("Error ..." + ex.Message,"Error");
		}	
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
            graphic.DrawImage(pImage, 0, 0,lnWidth ,lnHeight);
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
        private void ChangeSize(string fName)
        {
            Image imgTot = null;

            try
            {
                if (img.IsValid() == true)
                {
                    pictureControl.Width = panel1.Width - 2;
                    pictureControl.Height = panel1.Height - 2;
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
                    pictureControl.Height = h;
                    pictureControl.Image = CreateThumbnail(newImage, w, h); //newImage.GetThumbnailImage(w, h, new System.Drawing.Image.GetThumbnailImageAbort(GetThumbnailImageAbort), IntPtr.Zero);
                    newImage.Dispose();
                    pictureControl.Refresh();
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
                MessageBox.Show("Error ..." + ex.Message, "Error");
            }
        }
	void BoxDtlsLstDelIamgeInsert(object sender, KeyEventArgs e)
	{
		string delPath=null;
		string sourceFileName=null;
		string delFileName=null;
		string qcFilePath=null;
		string scanFilePath;
		string indexPath;
		
		try
		{
            if (e.KeyCode == Keys.Insert)
            {
                imageDelLst = (ListBox)BoxDtls.Controls["lstImageDel"];
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];

                pageDelInsrt = true;
                if (imageDelLst.Items.Count > 0)
                {
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageDelLst.SelectedItem.ToString(), string.Empty);
                    if (UpdateState(eSTATES.PAGE_QC, imageDelLst.SelectedItem.ToString(), Convert.ToInt32(policyLst.SelectedItem.ToString())) == true)
                    {
                        //if (File.Exists(policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER + "\\" + imageDelLst.SelectedItem.ToString()))
                        //{
                        //    scanFilePath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER + "\\" + imageDelLst.SelectedItem.ToString();
                        //    qcFilePath = policyPath + "\\" + ihConstants._QC_FOLDER + "\\" + imageDelLst.SelectedItem.ToString();
                        //    indexPath = policyPath + "\\" + ihConstants._INDEXING_FOLDER + "\\" + imageDelLst.SelectedItem.ToString();
                        //    if (File.Exists(scanFilePath) == true)
                        //    {
                        //        File.Move(scanFilePath, indexPath);
                        //        //File.Copy(qcFilePath, indexPath);
                        //    }
                        //}
                        //else
                        //{
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageDelLst.SelectedItem.ToString(), string.Empty);
                            wfeImage wImage = new wfeImage(sqlCon, pImage);
                            string changedImageName = wImage.GetIndexedImageName();

                            delPath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;

                            sourceFileName = sourceFilePath + "\\" + imageDelLst.SelectedItem.ToString();
                            delFileName = delPath + "\\" + imageDelLst.SelectedItem.ToString();
                            scanFilePath = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + imageDelLst.SelectedItem.ToString();
                            if (changedImageName == string.Empty)
                            {
                                qcFilePath = indexFolderName + "\\" + imageDelLst.SelectedItem.ToString();
                            }
                            else
                            {
                                qcFilePath = indexFolderName + "\\" + changedImageName;
                            }
                            if (File.Exists(delFileName) == true)
                            {
                                File.Move(delFileName, qcFilePath);
                                File.Copy(qcFilePath, scanFilePath,true);
                            }
                        //}

                        BoxDtls.InsertNotify(imageLst.SelectedIndex);
                        EnableDisbleControls(true);
                        delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
                        if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                        {
                            delImgList.SetSelected(delImgList.SelectedIndex, false);
                        }
                    }
                }
            }
		}
		catch(Exception Ex)
		{
			MessageBox.Show(Ex.Message);
		}
	}
	
	private void DisplayDockTypes()
	{
		config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
			char PROPOSALFORM=Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALFORM_KEY).Remove(1, 1).Trim());
            char PHOTOADDENDUM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHOTOADDENDUM_KEY).Remove(1, 1).Trim());
			char PROPOSALENCLOSERS=Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALENCLOSERS_KEY).Remove(1, 1).Trim());
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
		ListViewItem lvwItem = lvwDockTypes.Items.Add("PROPOSAL FORM");
		lvwItem.SubItems.Add(PROPOSALFORM.ToString());
		lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add("PHOTO ADDENDUM");
        lvwItem.SubItems.Add(PHOTOADDENDUM.ToString());
        lvwItem.SubItems.Add("0");

		lvwItem=lvwDockTypes.Items.Add("PROPOSAL ENCLOSERS");
		lvwItem.SubItems.Add(PROPOSALENCLOSERS.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("SIGNATURE PAGE");
		lvwItem.SubItems.Add(SIGNATUREPAGE.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("MEDICAL REPORT");
		lvwItem.SubItems.Add(MEDICALREPORT.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("PROPOSAL REVIEW SLIP");
		lvwItem.SubItems.Add(PROPOSALREVIEWSLIP.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("POLICY BOND");
		lvwItem.SubItems.Add(POLICYBOND.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("NOMINATION");
		lvwItem.SubItems.Add(NOMINATION.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("ASSIGNMENT");
		lvwItem.SubItems.Add(ASSIGNMENT.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("ALTERATION");
		lvwItem.SubItems.Add(ALTERATION.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("REVIVALS");
		lvwItem.SubItems.Add(REVIVALS.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("POLICY LOANS");
		lvwItem.SubItems.Add(POLICYLOANS.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("SURRENDER");
		lvwItem.SubItems.Add(SURRENDER.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("CLAIMS");
		lvwItem.SubItems.Add(CLAIMS.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("CORRESPONDENCE");
		lvwItem.SubItems.Add(CORRESPONDENCE.ToString());
		lvwItem.SubItems.Add("0");
		
		lvwItem=lvwDockTypes.Items.Add("OTHERS");
		lvwItem.SubItems.Add(OTHERS.ToString());
		lvwItem.SubItems.Add("0");

        lvwItem = lvwDockTypes.Items.Add("KYC DOCUMENT");
        lvwItem.SubItems.Add(KYCDOCUMENT.ToString());
        lvwItem.SubItems.Add("0");

//		lvwItem=lvwDockTypes.Items.Add("DELETE");
//		lvwItem.SubItems.Add(DELETE.ToString());
//		lvwItem.SubItems.Add("0");
	}
	void BoxDtlsLstImageIndex(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
            DialogResult rlst;
			config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
			char PROPOSALFORM=Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALFORM_KEY).Remove(1, 1).Trim());
            char PHOTOADDENDUM = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PHOTOADDENDUM_KEY).Remove(1, 1).Trim());
			char PROPOSALENCLOSERS=Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.PROPOSALENCLOSERS_KEY).Remove(1, 1).Trim());
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
           	bool sigBol = false;
            DateTime st = DateTime.Now;
            
            bool indexedBol = false;
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];

            string policyNo = policyLst.SelectedItem.ToString();

            string origDoctype = string.Empty;
            int tifPos = imageLst.SelectedItem.ToString().ToString().IndexOf("-") + 1;
            if (tifPos > 0)
            {
                origDoctype = imageLst.SelectedItem.ToString().Substring(tifPos);
            }
            //if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == DELETE)
            //{
            //    ImageDelete();
            //    return;
            //}
            if ((origDoctype == "Signature_page") && (Convert.ToChar(e.KeyChar.ToString().ToUpper()) != DELETE))
            {
                MessageBox.Show("You can not change signature page. You can only delete it...");
                return;
            }

           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PROPOSALFORM)
		    {
           		indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(),ihConstants.PROPOSALFORM_FILE);
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
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PROPOSALENCLOSERS_FILE);
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
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.MEDICALREPORT_FILE);
           		docType=ihConstants.MEDICALREPORT_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == PROPOSALREVIEWSLIP)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.PROPOSALREVIEWSLIP_FILE);
           		docType=ihConstants.PROPOSALREVIEWSLIP_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == POLICYBOND)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.POLICYBOND_FILE);
           		docType=ihConstants.POLICYBOND_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == NOMINATION)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.NOMINATION_FILE);
           		docType=ihConstants.NOMINATION_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ASSIGNMENT)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ASSIGNMENT_FILE);
           		docType=ihConstants.ASSIGNMENT_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == ALTERATION)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.ALTERATION_FILE);
           		docType=ihConstants.ALTERATION_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == REVIVALS)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.REVIVALS_FILE);
           		docType=ihConstants.REVIVALS_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == POLICYLOANS)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.POLICYLOANS_FILE);
           		docType=ihConstants.POLICYLOANS_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == SURRENDER)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.SURRENDER_FILE);
           		docType=ihConstants.SURRENDER_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == CLAIMS)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.CLAIMS_FILE);
           		docType=ihConstants.CLAIMS_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == CORRESPONDENCE)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.CORRESPONDENCE_FILE);
           		docType=ihConstants.CORRESPONDENCE_FILE;
           		bolKey=true;
            }
           	if (Convert.ToChar(e.KeyChar.ToString().ToUpper()) == OTHERS)
		    {
                indexedBol = ChangeAndMoveFile(imageLst.SelectedItem.ToString(), ihConstants.OTHERS_FILE);
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
                    UpdateState(eSTATES.PAGE_INDEXED, originalFileName, docType, indexFileName, Convert.ToInt32(policyLst.SelectedItem.ToString()));
                }
                 
	           	index=imageLst.SelectedIndex;
                //imageLst.Text.Replace(imageLst.SelectedItem.ToString(), selImageName);
                imageLst.Items[index] = selImageName;
                imageLst.Refresh();
				if((index+1) != imageLst.Items.Count)
	           	{
                    if (sigBol == false)
                    {
                        imageLst.SelectedIndex = index + 1;
                        ShowIndexedImage();
                    }
                    if(sigBol == true)
                    {
                        if ((index + 2) != imageLst.Items.Count)
                        {
                            if ((imageLst.SelectedIndex + 1) != (imageLst.Items.Count))
                            {
                                imageLst.SelectedIndex = index + 2;
                                ShowIndexedImage();
                            }
                            else
                            {
                                if ((policyLst.SelectedIndex) != policyLst.Items.Count)
                                {
                                    if (PersonalValidation())
                                    {
                                        policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if ((policyLst.SelectedIndex + 1) != policyLst.Items.Count)
                            {
                                if (PersonalValidation())
                                {
                                    policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                }
                            }
                        }
                    }
	           	}
				else 
				{
					if((policyLst.SelectedIndex+1) != (policyLst.Items.Count))
					{
                        if (PersonalValidation())
                        {
                            if (imageLst.Items.Count == (imageLst.SelectedIndex + 1))
                            {
                                if (MandatoryDocTypeChecking() == false)
                                {
                                    rlst = MessageBox.Show(this, "Mandatory document(Proposal form, Reviewslip, Policybond, Signature page) missing, do you want to proceed?", "Missing", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (rlst == DialogResult.Yes)
                                    {
                                        policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                                    }
                                }
                                else
                                    policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            }
                            else
                            {
                                policyLst.SelectedIndex = policyLst.SelectedIndex + 1;
                            }
                        }
					}
				}
           	}
           	DisplayDocTypeCount();
            imageLst.Refresh();
            DateTime end = DateTime.Now;
            TimeSpan duration = end - st;
            //MessageBox.Show("Total - " + duration.Milliseconds.ToString());
		}
        /// <summary>
        /// For showing the deleted image preview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BoxDtls_LstDelImgClick(object sender, EventArgs e)
        {
            string delFileName;

            delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
            if (delImgList.Items.Count > 0)
            {
                if (delImgList.SelectedIndex >= 0)
                {
                    EnableDisbleControls(false);
                    if (File.Exists(policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER + "\\" + delImgList.SelectedItem.ToString()))
                    {
                        delFileName = policyPath + "\\" + ihConstants._SCAN_FOLDER + "\\" + ihConstants._DELETE_FOLDER;
                    }
                    else
                    {
                        delFileName = sourceFilePath + "\\" + ihConstants._DELETE_FOLDER;
                    }

                    string[] searchFileName = Directory.GetFiles(delFileName, delImgList.SelectedItem.ToString());
                    //For searching deleted file in deleted folder.
                    if (searchFileName.Length >= 0)
                    {
                        delFileName = searchFileName[0];

                        img.LoadBitmapFromFile(delFileName);
                        ChangeSize(delFileName);
                        //Show the image back in picture box
                        //pictureControl.Image = img.GetBitmap();
                    }
                }
            }
        }
        /// <summary>
        /// For enable and disable control buttons
        /// </summary>
        /// <param name="prmControl"></param>
        void EnableDisbleControls(bool prmControl)
        {
            prmButtonCrop.Enabled = prmControl;
            prmButtonAutoCrp.Enabled = prmControl;
            prmButtonRotateRight.Enabled = prmControl;
            prmButtonRotateLeft.Enabled = prmControl;
            prmButtonZoomIn.Enabled = prmControl;
            prmButtonZoomOut.Enabled = prmControl;
            prmButtonSkewRight.Enabled = prmControl;
            prmButtonSkewLeft.Enabled = prmControl;
            prmButtonNoiseRemove.Enabled = prmControl;
            prmButtonCleanImg.Enabled = prmControl;
            prmButtonCopyImage.Enabled = prmControl;
            prmButtonDelImage.Enabled = prmControl;
            prmButtonRescan.Enabled = prmControl;
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
                    if (File.Exists(indexFolderName + "\\" + imageName) == false)
                    {
                        File.Copy(sourceFilePath + "\\" + imageName, indexFolderName + "\\" + imageName);
                    }
                    imgFileName = indexFolderName + "\\" + imageLst.SelectedItem.ToString();
                }
                img.LoadBitmapFromFile(imgFileName);
                //Open the source file
                ChangeSize();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while showing image..." + ex.Message);
                exMailLog.Log(ex);
            }
        }
	private void DisplayDocTypeCount()
	{
		
		int pos;
		
		DisplayDockTypes();
		for(int i=0;i < imageLst.Items.Count;i++)
		{
			pos = imageLst.Items[i].ToString().IndexOf("-");
			docType=imageLst.Items[i].ToString().Substring(pos+1);
			if(docType == ihConstants.PROPOSALFORM_FILE)
			{
				lvwDockTypes.Items[0].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[0].SubItems[2].Text) + 1));
			}
            if (docType == ihConstants.PHOTOADDENDUM_FILE)
            {
                lvwDockTypes.Items[1].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[1].SubItems[2].Text) + 1));
            }
			if(docType == ihConstants.PROPOSALENCLOSERS_FILE)
			{
				lvwDockTypes.Items[2].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[2].SubItems[2].Text) + 1));
			}
			if(docType == ihConstants.SIGNATUREPAGE_FILE)
			{
				lvwDockTypes.Items[3].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[3].SubItems[2].Text) + 1));
			}
			if(docType == ihConstants.MEDICALREPORT_FILE)
			{
				lvwDockTypes.Items[4].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[4].SubItems[2].Text) + 1));
			}
			if(docType == ihConstants.PROPOSALREVIEWSLIP_FILE)
			{
				lvwDockTypes.Items[5].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[5].SubItems[2].Text) + 1));
			}
			if(docType == ihConstants.POLICYBOND_FILE)
			{
				lvwDockTypes.Items[6].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[6].SubItems[2].Text) + 1));
			}
			if(docType == ihConstants.NOMINATION_FILE)
			{
				lvwDockTypes.Items[7].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[7].SubItems[2].Text) + 1));
			}
			if(docType == ihConstants.ASSIGNMENT_FILE)
			{
				lvwDockTypes.Items[8].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[8].SubItems[2].Text) + 1));
			}if(docType == ihConstants.ALTERATION_FILE)
			{
				lvwDockTypes.Items[9].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[9].SubItems[2].Text) + 1));
			}if(docType == ihConstants.REVIVALS_FILE)
			{
				lvwDockTypes.Items[10].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[10].SubItems[2].Text) + 1));
			}if(docType == ihConstants.POLICYLOANS_FILE)
			{
				lvwDockTypes.Items[11].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[11].SubItems[2].Text) + 1));
			}if(docType == ihConstants.SURRENDER_FILE)
			{
				lvwDockTypes.Items[12].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[12].SubItems[2].Text) + 1));
			}if(docType == ihConstants.CLAIMS_FILE)
			{
				lvwDockTypes.Items[13].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[13].SubItems[2].Text) + 1));
			}if(docType == ihConstants.CORRESPONDENCE_FILE)
			{
				lvwDockTypes.Items[14].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[14].SubItems[2].Text) + 1));
			}if(docType == ihConstants.OTHERS_FILE)
			{
				lvwDockTypes.Items[15].SubItems[2].Text =Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[15].SubItems[2].Text) + 1));
			}
            if (docType == ihConstants.KYCDOCUMENT_FILE)
            {
                lvwDockTypes.Items[16].SubItems[2].Text = Convert.ToString((Convert.ToInt32(lvwDockTypes.Items[16].SubItems[2].Text) + 1));
            }
		}
		//imageDelLst = (ListBox)BoxDtls.Controls["lstImageDel"];
		//lvwDockTypes.Items[15].SubItems[2].Text =Convert.ToString(imageDelLst.Items.Count);
	}

        private bool MandatoryDocTypeChecking()
        {
            System.Collections.Hashtable dp = new Hashtable();
            dp.Add(ihConstants.PROPOSALFORM_FILE, false);
            dp.Add(ihConstants.PROPOSALREVIEWSLIP_FILE, false);
            dp.Add(ihConstants.POLICYBOND_FILE, false);
            dp.Add(ihConstants.SIGNATUREPAGE_FILE, false);
            int pos;
            for (int i = 0; i < imageLst.Items.Count; i++)
            {
                pos = imageLst.Items[i].ToString().IndexOf("-");
                docType = imageLst.Items[i].ToString().Substring(pos + 1);
                if (docType == ihConstants.PROPOSALFORM_FILE)
                {
                    dp[ihConstants.PROPOSALFORM_FILE] = true;
                }
                if (docType == ihConstants.PROPOSALREVIEWSLIP_FILE)
                {
                    dp[ihConstants.PROPOSALREVIEWSLIP_FILE] = true;
                }
                if (docType == ihConstants.POLICYBOND_FILE)
                {
                    dp[ihConstants.POLICYBOND_FILE] = true;
                }
                if (docType == ihConstants.SIGNATUREPAGE_FILE)
                {
                    dp[ihConstants.SIGNATUREPAGE_FILE] = true;
                }
            }
            for (int j = 0; j < dp.Count; j++)
            {
                foreach (bool isOk in dp.Values)
                    if (isOk == false)
                        return false;
            }
            return true;
        }
        private bool ChangeAndMoveFile(string prmSourceFileName, string prmDocType)
		{
			string indexFileName=null;
			string sourceFile=null;
            CtrlImage ctrlImg;
            wfeImage wimg;
			int pos;
            int tifPos;
            string origDoctype = string.Empty;
            

			//newFileName=GetFileName(prmSourceFileName,prmDocType);
			
			//indexFileName=indexFolderName + "\\" + prmSourceFileName;
            try
            {
                pos = prmSourceFileName.ToString().IndexOf("-");
                tifPos = prmSourceFileName.ToString().IndexOf("-") + 1;
                if (tifPos > 0)
                {
                    origDoctype = prmSourceFileName.Substring(tifPos);
                }
                if (pos <= 0)
                {
                    pos = prmSourceFileName.ToString().IndexOf("TIF") - 1;
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos) + "-" + prmDocType + ".TIF";
                        sourceFile = indexFolderName + "\\" + prmSourceFileName;
                        indexFilePath = indexFileName;
                    }
                    else
                    {
                        indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos) + "-" + ihConstants.PROPOSALFORM_FILE + ".TIF";
                        sourceFile = indexFolderName + "\\" + prmSourceFileName;
                        indexFilePath = indexFileName;
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
                    }
                    else
                    {
                        if (origDoctype != ihConstants.SIGNATUREPAGE_FILE)
                        {
                            prmSourceFileName = imageLst.SelectedItem.ToString().Substring(0, pos);
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), prmSourceFileName, string.Empty);
                            wfeImage wImage = new wfeImage(sqlCon, pImage);
                            prmSourceFileName = wImage.GetIndexedImageName();
                            sourceFile = indexFolderName + "\\" + prmSourceFileName;
                            indexFileName = indexFolderName + "\\" + prmSourceFileName.Substring(0, pos - 3) + ihConstants.PROPOSALFORM_FILE + ".TIF";
                            indexFilePath = indexFileName;
                        }
                    }
                }
                string fileCount = imageLst.SelectedItem.ToString().Substring(policyLen, 4);
                string propFileName = indexFolderName + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";
                if ((File.Exists(propFileName) == false) || (File.Exists(indexFileName)== false))
                {
                    if (prmDocType != ihConstants.SIGNATUREPAGE_FILE)
                    {
                        if (origDoctype != prmDocType)
                        {
                            File.Copy(sourceFile, indexFileName, true);
                            File.Delete(sourceFile);
                        }
                    }
                    else
                    {
                        if (origDoctype != ihConstants.SIGNATUREPAGE_FILE)
                        {
                            if (File.Exists(indexFileName) == false)
                            {
                                File.Copy(sourceFile, indexFileName, true);
                                File.Delete(sourceFile);
                            }
                            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                            fileCount = imageLst.SelectedItem.ToString().Substring(policyLen, 4);
                            propFileName = indexFolderName + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF";
                            if (File.Exists(propFileName) == false)
                            {
                                ctrlImg = new CtrlImage(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(policyLst.SelectedItem), policyLst.SelectedItem.ToString() + fileCount + "_B" + ".TIF", string.Empty);
                                wimg = new wfeImage(sqlCon, ctrlImg);
                                int imgCount = wimg.GetImageCount();
                                imgCount++;
                                System.IO.FileInfo info = new System.IO.FileInfo(indexFileName);
                                double fileSize = info.Length;
                                fileSize = fileSize / 1024;

                                if (wimg.Save(crd, eSTATES.PAGE_INDEXED, ihConstants.SIGNATUREPAGE_FILE, policyLst.SelectedItem.ToString() + fileCount + "_B-" + ihConstants.SIGNATUREPAGE_FILE + ".TIF", fileSize, imgCount) == true)
                                {
                                    File.Copy(indexFileName, propFileName, true);
                                    //File.Copy(indexFileName, sourceFilePath + "\\" + policyLst.SelectedItem.ToString() + fileCount + "_B" + ".TIF", true);
                                    imageLst.Items.Insert(imageLst.SelectedIndex + 1, policyLst.SelectedItem.ToString() + fileCount + "_B.TIF-" + ihConstants.SIGNATUREPAGE_FILE);
                                }
                            }
                            indexFileName = propFileName;
                        }
                    }
                }
                //img.LoadBitmapFromFile(indexFileName);
                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while changing the index name......" + ex.Message);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Project-" + wBox.ctrlBox.ProjectCode + " ,Batch-" + wBox.ctrlBox.BatchKey + " ,Box-" + wBox.ctrlBox.BoxNumber + "Image name-" + sourceFile + "Doc type-" + prmDocType + "\n");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(ex, this);
                return false;
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

            if (!string.IsNullOrEmpty(txtRegional.Text))
            {
                Label keyno = (Label)BoxDtls.Controls["lblNRCNo"];
                pPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                if (wPolicy.UpdateRegionalName(txtName.Text.Trim(), txtRegional.Text.Trim(), keyno.Text))
                {
                    lblResult.Text = "Successfully Updated...";
                    lblResult.ForeColor = Color.Green;
                }
                else
                {
                    lblResult.Text = "Error, Try Again...";
                    lblResult.ForeColor = Color.Red;
                }
                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                imageLst.Focus();
                ShowPolicyDetails();
            }
            else
            {
                lblResult.Text = "Blank Regional Name...";
                lblResult.ForeColor = Color.Red;
            }
		}

        void BoxDtls_LstImgClick(object sender, System.EventArgs e)
        {
            EnableDisbleControls(true);
            ShowImage(false);
        }
		void BoxDtlsBoxMouseClick(object sender, MouseEventArgs e)
		{
			Point pt = new Point();
			pt.X = e.X;
			pt.Y = e.Y;
			if(e.Button == MouseButtons.Right)
			{
                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()));
                policy = new wfePolicy(sqlCon, ctrlPolicy);

                int polStatus = policy.GetPolicyStatus();
                if (polStatus == (int)eSTATES.POLICY_ON_HOLD)
                {
                    markNotReadyHoldPolicyToolStripMenuItem.Enabled = false;
                    markReadyToolStripMenuItem.Enabled = true;
                }
                else
                {
                    markNotReadyHoldPolicyToolStripMenuItem.Enabled = true;
                    markReadyToolStripMenuItem.Enabled = false;
                }
                contextMenuStrip1.Show(BoxDtls, pt);
			}
		}

        private void BoxDtls_PreviousClicked(object sender, EventArgs e)
        {
            imageLst = imageLst = (ListBox)BoxDtls.Controls["lstImage"];
            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];

            //IndexingOperation = false;
            if (imageLst.Items.Count > 0)
            {
                EnableDisbleControls(true);

                delImgList = (ListBox)BoxDtls.Controls["lstImageDel"];
                if ((delImgList.SelectedIndex >= 0) && (delImgList.Items.Count > 0))
                {
                    delImgList.SetSelected(delImgList.SelectedIndex, false);
                }
                ShowImage(false);
                //DisplayDocTypeCount();
            }
        }

        private void aeIndexing_KeyDown(object sender, KeyEventArgs e)
        {
            DialogResult rlst;
            try
            {
                if (e.KeyCode == Keys.F5)
                {
                    ShowPolicyDetails();
                    if (PersonalValidation())
                    {
                        if (MandatoryDocTypeChecking() == false)
                        {
                            rlst = MessageBox.Show(this, "Mandatory document missing, do you want to proceed?", "Missing", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (rlst == DialogResult.Yes)
                            {
                                UpdateAllPolicyStatus();
                                //wfePolicy wPolicy = new wfePolicy(sqlCon);
                                //int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_INDEXED);
                                this.Text = "Image Indexing";
                                this.Text = this.Text;// +"                                       Today you have done " + count + " ";
                                BoxDtls.RefreshNotify();
                                policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                                imageLst = (ListBox)BoxDtls.Controls["lstImage"];
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
                                        pictureControl.Image = null;
                                        DisplayDockTypes();
                                    }
                                }
                            }
                        }
                        else
                        {
                            UpdateAllPolicyStatus();
                            //wfePolicy wPolicy = new wfePolicy(sqlCon);
                            //int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_INDEXED);
                            //this.Text = "Image Indexing";
                            //this.Text = this.Text + "                                       Today you have done " + count + " ";
                            BoxDtls.RefreshNotify();
                            policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                            imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                            if (policyLst.Items.Count > 0)
                            {
                                ShowImage(false);
                            }
                            else
                            {
                                imageLst.Items.Clear();
                                if (policyLst.Items.Count == 0)
                                {
                                    EnableDisbleControls(false);
                                    pictureControl.Image = null;
                                    DisplayDockTypes();
                                }
                            }
                        }
                        
                    }
                }
                if ((e.KeyCode == Keys.Z) && (e.Control))
                {
                    ImageCopy();
                }
                ///For checking todays production count
                if ((e.KeyCode == Keys.F9))
                {
                    wfePolicy wPolicy = new wfePolicy(sqlCon);
                    int count = wPolicy.GetTransactionLogCount(wBox.ctrlBox.BatchKey.ToString(), dbcon.GetCurrenctDTTM(2, sqlCon), crd.created_by, eSTATES.POLICY_INDEXED);
                    frmProductionCount frmProd = new frmProductionCount(count);
                    frmProd.ShowDialog(this);
                }
                if ((e.KeyCode == Keys.C) && (e.Control))
                {
                    string imageName;
                    policyLst = (ListBox)BoxDtls.Controls["lstPolicy"];
                    imageLst = (ListBox)BoxDtls.Controls["lstImage"];
                    int pos = imageLst.SelectedItem.ToString().IndexOf("-");
                    if (pos > 0)
                    {
                        imageName = imageLst.SelectedItem.ToString().Substring(0, pos);
                    }
                    else
                    {
                        imageName = imageLst.SelectedItem.ToString();
                    }
                    pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyLst.SelectedItem.ToString()), imageName, string.Empty);
                    wfeImage wImage = new wfeImage(sqlCon, pImage);
                    aeCustomExp frmExp = new aeCustomExp(wImage, sqlCon, crd);
                    frmExp.ShowDialog(this);
                }
                char DELETE = Convert.ToChar(config.GetValue(ihConstants.INDEX_SHORTCUT_KEY_SECTION, ihConstants.DELETE_KEY).Remove(1, 1).Trim());
                char rslt;
                if (char.TryParse(e.KeyCode.ToString(), out rslt))
                {
                    if (Convert.ToChar(e.KeyCode.ToString().ToUpper()) == DELETE)
                    {
                        ImageDelete();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        private Microsoft.Win32.RegistryKey GetRegKey()
        {
            // Create key in HKLM without Release
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\Inlite\\" + Application.ProductName);
            return key;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
	}
}
