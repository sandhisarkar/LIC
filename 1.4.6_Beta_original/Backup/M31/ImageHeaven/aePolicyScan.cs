/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 1/4/2008
 * Time: 4:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.Odbc;
using System.Data;
using System.Collections;
using System.Threading;
using System.IO;
using LItems;
using NovaNet.Utils;
using NovaNet.wfe;
using GdiPlusLib;
using TwainLib;
using System.Runtime.InteropServices;

namespace ImageHeaven
{
	/// <summary>
	/// Description of aePolicyScan.
	/// </summary>
	public partial class aePolicyScan : Form,IMessageFilter
	{
		private wfeBox wBox=null;
		private OdbcConnection sqlCon;
		private wfeBatch pBatch=null;
		private wfeProject pProject=null;
		//private ADFScanUtils scanUtil=null;
		private wfeBatch wBatch=null;
		NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
		private CtrlPolicy pPolicy=null;
		private CtrlBox pBox=null;
		//private TImgDisp timg = new TImgDisp();
		string scanFolder=null;
		//private IContainer components;
		private bool	msgfilter;
		private Twain	tw;
		//private int		picnumber = 0;
        private int i;
        private int j;
        ArrayList policyList;
        private CtrlImage pImage = null;
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);	
		string batchPath=null;
		string scanDate;
		int pageCount;
		bool hasphoto;
        bool policyChanged = true;
        private bool colorMode;
        int blackBol;
        private long Page2=0;
		//Bitmap picBmp;
		ci tmpImg;
        bool SaveInColor = true;
        Credentials crd = new Credentials();
        bool hasImage = false;
        string scanSeparatorType="1";
        private int scanMode;

		public aePolicyScan(wfeBox prmBox,OdbcConnection prmCon,Credentials prmCrd,int prmMode)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			sqlCon=prmCon;
			wBox = prmBox;
            scanMode = prmMode;
			InitializeComponent();
			this.Text="Batch Scanning";
			tmpImg =(ci) IgrFactory.GetImagery(Constants.IGR_CLEARIMAGE);
            crd = prmCrd;
            exMailLog.SetNextLogger(exTxtLog);
            
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		public aePolicyScan()
		{			
			InitializeComponent();			
			this.Text="Scan Centre";
            exMailLog.SetNextLogger(exTxtLog);
		}
		
		
		void AePolicyScanLoad(object sender, EventArgs e)
		{
            DisplayValues();
            tw = new Twain();
            tw.Init(this.Handle);
            ShowPolicy();
            PrevImages();
            cmdCancelScan.ForeColor = Color.Black;
            cmdScan.ForeColor = Color.Black;
            lblBatch.ForeColor = Color.RoyalBlue;
            lblBox.ForeColor = Color.RoyalBlue;
            lblCurrentPolicy.ForeColor = Color.RoyalBlue;
            lblNextPolicy.ForeColor = Color.RoyalBlue;
            lblPageCount.ForeColor = Color.RoyalBlue;
            lblPicSize.ForeColor = Color.RoyalBlue;
            lblProjectName.ForeColor = Color.RoyalBlue;
            lblSize.ForeColor = Color.Black;
            label1.ForeColor=Color.Black;
            label2.ForeColor=Color.Black;
            label3.ForeColor=Color.Black;
            label4.ForeColor=Color.Black;
            label5.ForeColor=Color.Black;
            label6.ForeColor=Color.Black;
            label9.ForeColor=Color.Black;
            cmdScan.Enabled = true;
            cmdDelete.Enabled = true;
            cmdCancelScan.Enabled = false;
            ImageConfig config = new ImageConfig(ihConstants.CONFIG_FILE_PATH);
            scanSeparatorType = config.GetValue(ihConstants._SCAN_SECTION, ihConstants._SCAN_KEY).Replace("\0", "");
		}
		void DisplayValues()
		{
			pBatch=new wfeBatch(sqlCon);
			pProject=new wfeProject(sqlCon);
			lblProjectName.Text=pProject.GetProjectName(wBox.ctrlBox.ProjectCode);
			lblBatch.Text=pBatch.GetBatchName(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey);
			lblBox.Text=wBox.ctrlBox.BoxNumber.ToString();
		}
        /// <summary>
        /// It's a message filter interface
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            TwainCommand cmd = tw.PassMessage(ref m);
            if (cmd == TwainCommand.Not)
            {
                //this.Refresh();
                return false;
            }

            switch (cmd)
            {
                case TwainCommand.CloseRequest:
                    {
                        EndingScan();
                        tw.CloseSrc();
                        break;
                    }
                case TwainCommand.CloseOk:
                    {
                        EndingScan();
                        tw.CloseSrc();
                        break;
                    }
                case TwainCommand.DeviceEvent:
                    {
                        System.Diagnostics.Debug.Print("here");
                        break;
                    }
                case TwainCommand.TransferReady:
                    {
                        Twain.ImageNotification delMan = new Twain.ImageNotification(GetImage);
                        int pics = tw.TransferPictures(GetImage, this);
                        EndingScan();
                        if (pics != -1)
                        {
	                        if (colorMode)
	                        {
                                if (!msgfilter)
                                {
                                    //this.Enabled = false;
                                    msgfilter = true;
                                    Application.AddMessageFilter(this);
                                }
                                tw.Acquire(false, true);
                                /*
                                if (pics == TwainLib.Twain.__COLOUR)
		                        {
		                            if (!msgfilter)
		                            {
		                                //this.Enabled = false;
		                                msgfilter = true;
		                                Application.AddMessageFilter(this);
		                            }
		
		                            tw.Acquire(false,true);
		                        }
		                        if (pics == TwainLib.Twain.__BLACKWHITE)
		                        {
		                            if (!msgfilter)
		                            {
		                                //this.Enabled = false;
		                                msgfilter = true;
		                                Application.AddMessageFilter(this);
		                            }
		
		                            tw.Acquire(false,false);
		                        }
                                 * */
	                        }
	                        else
	                        {
	                        	if (pics > 0)
	                        	{
		                            if (!msgfilter)
		                            {
		                                //this.Enabled = false;
		                                msgfilter = true;
		                                Application.AddMessageFilter(this);
		                            }
		                            tw.Acquire(false,false);
	                        	}
	                        }
                        }
                        break;
                    }
            }

            return true;
        }
        private void ReadyForQC()
        {
            wfeBox tmpBox = null;
            CtrlBox pBox = null;
            int boxNumber = wBox.ctrlBox.BoxNumber;
            pBox = new CtrlBox(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey, boxNumber);
            tmpBox = new wfeBox(sqlCon, pBox);
            tmpBox.UpdateStatus(eSTATES.BOX_SCANNED);   
        }
        private void ReadyForRepeatScan()
        {
            wfeBox tmpBox = null;
            CtrlBox pBox = null;
            wfePolicy tmpPolicy = null;
            CtrlPolicy pPolicy = null;
            
            int boxNumber = wBox.ctrlBox.BoxNumber;
            pBox = new CtrlBox(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey, boxNumber);
            tmpBox = new wfeBox(sqlCon, pBox);
            OdbcTransaction pTrans = sqlCon.BeginTransaction();
            if (tmpBox.UpdateStatus(eSTATES.BOX_CREATED, pTrans))
            {
                pPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey,wBox.ctrlBox.BoxNumber, 0);
                tmpPolicy = new wfePolicy(sqlCon, pPolicy);
                if (tmpPolicy.UpdateAllPolicyStatus(eSTATES.POLICY_CREATED, crd, pTrans))
                {
                    pTrans.Commit();
                }
                else
                {
                    pTrans.Rollback();
                }
            }
            else
            {
                pTrans.Rollback();
            }
        }

        public void GetImage(System.IntPtr prmHBmp)
        {
            bool blankBol=true;
            bool exitPoly=true;
            string tifFileName;
            wfePolicy wPolicy =null;
            char leftPad = Convert.ToChar("0");
            CtrlPolicy ctrPolCurrent = null;
        	CtrlPolicy ctrPolNext = null;
            bool success = false;
                if (blackBol == 0)
                {
                    //if (colorMode==true)
                    //{
                    //    tw.ReqChangeMode(TwainLib.Twain.__BLACKWHITE);
                    //}
					if(policyList.Count>0)
					{
						
                        //For showing the next and current policy
                        if ((policyList.Count > (j + 1)))
                        {
                            ctrPolCurrent = (CtrlPolicy)policyList[j];
                            ctrPolNext = (CtrlPolicy)policyList[j + 1];
                        }
                        else if (policyList.Count == (j + 1))
                        {
                            ctrPolCurrent = (CtrlPolicy)policyList[j];
                            ctrPolNext = (CtrlPolicy)policyList[j];
                        }
                        else
                        {
                            tw.CloseSrc();
                            EndingScan();
                            
                            if (prmHBmp != IntPtr.Zero)
                            {
                                Marshal.FreeHGlobal(prmHBmp);
                                prmHBmp = IntPtr.Zero;
                            }
                            DialogResult dr = MessageBox.Show("You want to repeat scan this box?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (dr == DialogResult.Yes)
                            {
                                ReadyForRepeatScan();
                                ShowPolicy();
                                PrevImages();
                            }
                            else
                            {
                                ReadyForQC();
                            }
                            return;
                        }
                        
                        lblCurrentPolicy.Text = ctrPolCurrent.PolicyNumber.ToString();
                        lblNextPolicy.Text = ctrPolNext.PolicyNumber.ToString();
                        lblCurrentPolicy.Refresh();
                        lblNextPolicy.Refresh();
                        this.Text = "Batch Scanning            " + " Project- " + lblProjectName.Text + "  Batch- " + lblBatch.Text + "  Box- " + lblBox.Text + " Current Policy- " + lblCurrentPolicy.Text + " Next Policy- " + lblNextPolicy.Text;
	                    scanFolder = batchPath + "\\" + ctrPolCurrent.BoxNumber + "\\" + ctrPolCurrent.PolicyNumber.ToString() + "\\" + ihConstants._SCAN_FOLDER;
	                    if (FileorFolder.CreateFolder(scanFolder) == true)
	                    {
	                        if (policyChanged == true)
	                        {
	                            pPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, ctrPolCurrent.PolicyNumber);
	                            wPolicy = new wfePolicy(sqlCon, pPolicy);
	                            pageCount = wPolicy.GetPolicyPageCount();
	                            lblPageCount.Text = pageCount.ToString();
	                            policyChanged = false;
	                            if (wPolicy.GetPolicyPhotoStatus() == ihConstants._POLICY_CONTAINS_PHOTO)
	                            {
	                                hasphoto = true;
	                            }
	                            else
	                            {
	                                hasphoto = false;
	                            }
	                        }
	                        i = i + 1;
	                        //bitmapFileName = scanFolder + "\\" + ctrPolCurrent.PolicyNumber + "_" + i.ToString().PadLeft(3, leftPad) + "_" + "A" + ".BMP";
	                        tifFileName = scanFolder + "\\" + ctrPolCurrent.PolicyNumber + "_" + i.ToString().PadLeft(3, leftPad) + "_" + "A" + ".TIF";
	                        if (tw.GetScanMode()==TwainLib.Twain.__COLOUR)
	                        {
	                        	Page2++;
	                        	if ((hasphoto==false))
	                        	{
                                    if (tmpImg.selfcheck == 1)
                                    {
                                        MessageBox.Show("check fail");
                                    }
	                        		tmpImg.LoadBitmapFromDIB(prmHBmp);
	                        		tmpImg.ToBitonal();
	                        		if(tmpImg.SaveFile(tifFileName) == IGRStatus.Success)
	                        		{
	                        			success = true;
	                        		}
	                        		else
	                        		{
	                        			success = false;
	                        		}
	                        	}
	                        	else
	                        	{
                                    if (tmpImg.selfcheck == 1)
                                    {
                                        MessageBox.Show("check fail");
                                    }
                                    tmpImg.LoadBitmapFromDIB(prmHBmp);
	                        		if ((SaveInColor==false) && (hasImage == false))
	                        		{
                                        tmpImg.ToBitonal();	
	                        		}
                                    if ((SaveInColor == true) && (hasImage == true))
                                    {
                                        tmpImg.ToBitonal();
                                    }
                                    if(tmpImg.SaveFile(tifFileName) == IGRStatus.Success)
	                        		{
	                        			success = true;
	                        		}
	                        		else
	                        		{
	                        			success = false;
	                        		}
                                    SaveInColor = false;
                                    hasImage = false;
	                        	}
	                        }
	                        else
	                        {
                                if (tmpImg.selfcheck == 1)
                                {
                                    MessageBox.Show("check fail");
                                }
	                			tmpImg.LoadBitmapFromDIB(prmHBmp);
	                			if(tmpImg.SaveFile(tifFileName) == IGRStatus.Success)
                        		{
                        			success = true;
                        		}
                        		else
                        		{
                        			success = false;
                        		}
	                        }
                            if(tmpImg.IsBlankPage() == IGRStatus.Success)
                            {
                                blankBol = false;
                            }
                            if (blankBol == false)
                            {
                                if (scanSeparatorType.ToString().Trim() == ihConstants.SCAN_SEPARATOR_BLACK)
                                    exitPoly = GetBatchSplitter(tmpImg.GetBitmap());
                                else
                                    exitPoly = !tmpImg.isSeparator(ihConstants.SCAN_SEPARATOR_COMPARE_STRING);

                                if (exitPoly == false)
                                {
                                    blackBol = 1;
                                    if ((i - 1) != pageCount)
                                    {
                                        //MessageBox.Show("Scanned page count mismatch with total page count entired at the time of inventry in", "Count mismatch", MessageBoxButtons.OK);
                                        UpdatePageCountExceptionLog(pageCount, i);
                                    }

                                    File.Delete(tifFileName);
                                    SaveInColor = true;
                                    scanDate = dbcon.GetCurrenctDTTM(1, sqlCon);
                                    pPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, ctrPolCurrent.PolicyNumber);
                                    wPolicy = new wfePolicy(sqlCon, pPolicy);
                                    crd.created_dttm = scanDate;
                                    wPolicy.UpdateStatus(eSTATES.POLICY_SCANNED, crd);

                                    ///insert into transaction log
                                    if (wPolicy.SearchForTransaction() <= 0)
                                    {
                                        wPolicy.UpdateTransactionLog(eSTATES.POLICY_SCANNED, crd);
                                    }
                                    wPolicy.UpdateScanDetails(scanDate, ihConstants.SCAN_SUCCESS_FLAG);
                                    pBox = new CtrlBox(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber);

                                    //............Change for repeat scanning on 18/01/2010
                                    wfeBox box = new wfeBox(sqlCon, pBox);
                                    NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
                                    state[0] = NovaNet.wfe.eSTATES.POLICY_CREATED;
                                    if (wPolicy.GetPolicyCount(state) == 0)
                                    {
                                        box.UpdateStatus(eSTATES.BOX_READY_TO_REPEAT_SCAN);
                                    }
                                    //....................................................

                                    policyChanged = true;
                                    lstImageName.Items.Clear();

                                    CtrlImage pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(ctrPolNext.PolicyNumber), string.Empty, string.Empty);
                                    wfeImage wImage = new wfeImage(sqlCon, pImage);

                                    int imageCount = wImage.GetImageCount();
                                    
                                    i = imageCount;

                                    j = j + 1;
                                    //For showing the next and current policy
                                    if ((policyList.Count > (j + 1)))
                                    {
                                        ctrPolCurrent = (CtrlPolicy)policyList[j];
                                        ctrPolNext = (CtrlPolicy)policyList[j + 1];
                                    }
                                    else if (policyList.Count == (j + 1))
                                    {
                                        ctrPolCurrent = (CtrlPolicy)policyList[j];
                                        ctrPolNext = (CtrlPolicy)policyList[j];
                                    }
                                    else
                                    {
                                        tw.CloseSrc();
                                        EndingScan();
                                        MessageBox.Show("No more policies are ready to be scanned....");
                                        if (prmHBmp != IntPtr.Zero)
                                        {
                                            Marshal.FreeHGlobal(prmHBmp);
                                            prmHBmp = IntPtr.Zero;
                                        }
                                        DialogResult dr = MessageBox.Show("You want to repeat scan this box?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                        if (dr == DialogResult.Yes)
                                        {
                                            ReadyForRepeatScan();
                                            ShowPolicy();
                                            PrevImages();
                                        }
                                        else
                                        {
                                            ReadyForQC();
                                        }
                                        return;
                                    }
                                    lblCurrentPolicy.Text = ctrPolCurrent.PolicyNumber.ToString();
                                    lblNextPolicy.Text = ctrPolNext.PolicyNumber.ToString();
                                    lblCurrentPolicy.Refresh();
                                    lblNextPolicy.Refresh();
                                    PrevImages();
                                    this.Text = "Batch Scanning            " + " Project- " + lblProjectName.Text + "  Batch- " + lblBatch.Text + "  Box- " + lblBox.Text + " Current Policy- " + lblCurrentPolicy.Text + " Next Policy- " + lblNextPolicy.Text;
                                }
                            }
                            else
                            {
                                i = i - 1;
                                //bmpTif.Dispose();
                                File.Delete(tifFileName);
                            }
	                        //File.Delete(bitmapFileName);
	                        if(success == true)
	                        {
		                        if ((blankBol == false) && (exitPoly != false))
		                        {
		                            SetListboxValue(ctrPolCurrent.PolicyNumber + "_" + i.ToString().PadLeft(3, leftPad) + "_" + "A" + ".TIF",i);
	                                //tmpImg.LoadBitmapFromDIB(prmHBmp);
	                                if (tmpImg.IsValid())
	                                {
	                                    scanPic.Image = null;
	                                    scanPic.Image = tmpImg.GetBitmap();
                                        scanPic.Refresh();
	                                }
		                        }
	                        }
	                        else
	                        {
	                        	MessageBox.Show("Error while saving files aborting....");
								if (prmHBmp != IntPtr.Zero)
				                {
				                    Marshal.FreeHGlobal(prmHBmp);
				                    prmHBmp = IntPtr.Zero;
				                }
                                tw.CloseSrc();
                                EndingScan();
	                        }
	                    }
					}
					else
					{
                        MessageBox.Show("No more policies remain to be scanned....");
                        tw.CloseSrc();
                        EndingScan();
                        DialogResult dr = MessageBox.Show("You want to repeat scan this box?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.Yes)
                        {
                            ReadyForRepeatScan();
                            ShowPolicy();
                            PrevImages();
                        }
                        else
                        {
                            ReadyForQC();
                        }
					}
                }
                else
                {
                    blackBol = 0;   
                }
                if (prmHBmp != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(prmHBmp);
                    prmHBmp = IntPtr.Zero;
                }
            
        }

        /// <summary>
        /// Initialize box and all policy status, added on 28/07/2010------------------------------------
        /// </summary>
        private void InitializePolicyAndBox()
        {
            pBox = new CtrlBox(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber);
            wfeBox box = new wfeBox(sqlCon, pBox);
            if (box.UpdateStatus(eSTATES.BOX_CREATED) == true)
            {
                pPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, 0);
                wfePolicy wPolicy = new wfePolicy(sqlCon, pPolicy);
                wPolicy.UpdateAllPolicyStatus(eSTATES.POLICY_CREATED, crd);
                wPolicy.DeleteTransLogEntry();
            }
        }
        /// <summary>
        /// This is used for detect black page
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns>bool</returns>
        private bool GetBatchSplitter(Bitmap bmp)
        {
            bool bmpImg = true;

            int p = 0;

            for (int i = ImageHeaven.ihConstants._BATCHSPLITTER_STARTX; i < (ImageHeaven.ihConstants._BATCHSPLITTER_STARTX + ImageHeaven.ihConstants._BATCHSPLITTER_XLEN); i++)
            {
                for (int j = ImageHeaven.ihConstants._BATCHSPLITTER_STARTY; j < (ImageHeaven.ihConstants._BATCHSPLITTER_STARTY + ImageHeaven.ihConstants._BATCHSPLITTER_YLEN) ; j++)
                {
                    //string name = bmp.GetPixel(i, j).Name;
                    if (bmp.GetPixel(i, j).Name == "ff000000")
                    {
                        bmpImg = false;
                    }
                    else
                    {
                        if (p == 200)
                        {
                            bmpImg = true;
                            break;
                        }
                        p = p + 1;
                    }
                }
                if (bmpImg == true)
                    break;
            }
            bmp.Dispose();
            return bmpImg;
        }
        ///// <summary>
        ///// This is used for detect blank pages
        ///// </summary>
        ///// <param name="prmBmp"></param>
        ///// <returns>bool</returns>
        //private bool DetectBlankPage(Bitmap bmp)
        //{
            
        //    bool bmpImg = true;
        //    //int [,] SamplingMatrix = GetSamplingMatrix();
        //    DateTime dt = DateTime.Now;
        //    int p = 0;
        //    //SamplingMatrix[i,0],SamplingMatrix[i,1]

        //    for (int i = 200; i < 800; i++)
        //    {
        //        for (int j = 200; j < 800; j++)
        //        {
        //            if (bmp.GetPixel(i, j).Name == "ffffffff")
        //            {
        //                bmpImg = false;
        //            }
        //            else
        //            {
        //                if (p == 25)
        //                {
        //                    bmpImg = true;
        //                    break;
        //                }
        //                p = p + 1;
        //            }
        //        }
        //        if (bmpImg == true)
        //            break;
        //    }
        //   // bmp.Dispose();
        //    return bmpImg;
        //}
        /// <summary>
        /// Save image for group 4 compression
        /// </summary>
        /// <param name="prmSourceBmp"></param>
        /// <param name="prmDestFileName"></param>
        //private void SaveInTif(Bitmap prmSourceBmp, string prmDestFileName,EncoderValue prmEncoder)
        //{
        //    EncoderParameters ep = new EncoderParameters(1);

        //    //get ImageCodecInfo, generate tif format

        //    ImageCodecInfo info = null;
        //    foreach (ImageCodecInfo ice in ImageCodecInfo.GetImageEncoders())
        //    {
        //        if (ice.MimeType == "image/tiff")
        //        {
        //            info = ice;
        //            break;
        //        }
        //    }
        //    Image img = prmSourceBmp;
        //    ep.Param[0] = new EncoderParameter(Encoder.Compression, Convert.ToInt32(prmEncoder));
        //    img.Save(prmDestFileName, info, ep);
        //}
        /// <summary>
        /// Call for ending the scanning
        /// </summary>
        private void EndingScan()
        {
            if (msgfilter)
            {
                Application.RemoveMessageFilter(this);
                msgfilter = false;
                this.Enabled = true;
                this.Activate();
                cmdScan.Enabled = true;
                cmdCancelScan.Enabled = false;
            }
        }
		public void UpdatePageCountExceptionLog(int prmPageToBeScanned,int prmPageScanned)
		{
			Credentials crd = new Credentials();
		
			pPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey,wBox.ctrlBox.BoxNumber,Convert.ToInt32(lblCurrentPolicy.Text));
			wfePolicy wPolicy=new wfePolicy(sqlCon,pPolicy);
			//crd.created_by = "ADMIN";
			crd.created_dttm = dbcon.GetCurrenctDTTM(1,sqlCon);
			wPolicy.SavePolicyPageCountException(crd,prmPageToBeScanned,prmPageScanned);
		}
		private ArrayList GetPolicyList()
		{
			ArrayList arrPolicy=new ArrayList();
			wQuery pQuery=new ihwQuery(sqlCon,scanMode);
			eSTATES[] state=new eSTATES[1];
            state[0]=eSTATES.POLICY_CREATED;
            arrPolicy=pQuery.GetItems(eITEMS.POLICY,state,wBox);
            return arrPolicy;
		}
		
		void SetListboxValue(string prmIamgeName,int prmSrlNo)
		{
			CtrlImage ctrlImg; 
			//Credentials crd=new Credentials();
			long fileSize;
			System.IO.FileInfo info = new System.IO.FileInfo(scanFolder + "\\" + prmIamgeName);
			
			fileSize = info.Length;
		    fileSize = fileSize / 1024;
			lblSize.Text = fileSize.ToString() + " KB";
			if(fileSize > 50)
			{
				lblSize.ForeColor = Color.Red;
			}
			else
			{
				lblSize.ForeColor = Color.Black;
			}
			wfeImage img;
			lstImageName.Items.Add(prmIamgeName);
            lstImageName.Refresh();
			ctrlImg = new CtrlImage(wBox.ctrlBox.ProjectCode,wBox.ctrlBox.BatchKey,wBox.ctrlBox.BoxNumber,Convert.ToInt32(lblCurrentPolicy.Text),prmIamgeName,string.Empty);
			img = new wfeImage(sqlCon,ctrlImg);
			img.Save(crd,eSTATES.PAGE_SCANNED,fileSize,ihConstants._NORMAL_PAGE,prmSrlNo,prmIamgeName);
		}
        private bool ShowPolicy()
        {
            CtrlPolicy ctrPolCurrent = null;
            CtrlPolicy ctrPolNext = null;

            policyList = GetPolicyList();
            if (policyList.Count > 0)
            {
                j = 0;
                if (policyList.Count > 1)
                {
                    ctrPolCurrent = (CtrlPolicy)policyList[0];
                    ctrPolNext = (CtrlPolicy)policyList[1];
                }
                else
                {
                    ctrPolCurrent = (CtrlPolicy)policyList[0];
                    ctrPolNext = (CtrlPolicy)policyList[0];
                }
                lblCurrentPolicy.Text = ctrPolCurrent.PolicyNumber.ToString();
                lblNextPolicy.Text = ctrPolNext.PolicyNumber.ToString();
                this.Text = "Batch Scanning            " + " Project- " + lblProjectName.Text + "  Batch- " + lblBatch.Text + "  Box- " + lblBox.Text + " Current Policy- " + lblCurrentPolicy.Text + " Next Policy- " + lblNextPolicy.Text;
                return true;
            }
            else
            {
                MessageBox.Show("No more policies remain to be scanned....");
                cmdScan.Enabled = false;
                return false;
            }
        }
        void PrevImages()
        {
            eSTATES[] prmPolicyState;

            ArrayList arrImage = new ArrayList();
            wQuery pQuery = new ihwQuery(sqlCon);
            CtrlPolicy ctrlPolicy = new CtrlPolicy(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey, wBox.ctrlBox.BoxNumber, Convert.ToInt32(lblCurrentPolicy.Text));
            wItem policy = new wfePolicy(sqlCon, ctrlPolicy);
            wBatch = new wfeBatch(sqlCon);
            batchPath = wBatch.GetPath(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
            CtrlImage ctrlImage;

            prmPolicyState = new eSTATES[1];
            prmPolicyState[0] = eSTATES.POLICY_CREATED;
            arrImage = pQuery.GetItems(eITEMS.PAGE, prmPolicyState, policy);

            lstImageName.Items.Clear();
            if (arrImage.Count > 0)
            {
                for (int l = 0; l < arrImage.Count; l++)
                {
                    ctrlImage = (CtrlImage)arrImage[l];
                    lstImageName.Items.Add(ctrlImage.ImageName);
                }

                scanFolder = batchPath + "\\" + lblBox.Text + "\\" + lblCurrentPolicy.Text + "\\" + ihConstants._SCAN_FOLDER;
            }
            lstImageName.Refresh();
        }
		void CmdScanClick(object sender, EventArgs e)
		{
            int imageCount = 0;
            
            tw.Select();
            SaveInColor = true;

            if (!msgfilter)
            {
                //this.Enabled = false;
                msgfilter = true;
                Application.AddMessageFilter(this);
            }
            
            //DialogResult result = MessageBox.Show("Do you want to scan in color mode?", "Scan", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (scanMode==ihConstants._SCAN_WITH_PHOTO)
            {
                colorMode = true;
            }
            else
            {
                colorMode = false;
            }
            //policyList = GetPolicyList();
            if(ShowPolicy()==true)
            {
                j = 0;
                wBatch = new wfeBatch(sqlCon);
                batchPath = wBatch.GetPath(wBox.ctrlBox.ProjectCode, wBox.ctrlBox.BatchKey);
                CtrlImage pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(lblCurrentPolicy.Text), string.Empty, string.Empty);
                wfeImage wImage = new wfeImage(sqlCon, pImage);
                imageCount = wImage.GetMaxPageCount();
                if (imageCount > 0)
                {
                    hasImage = true;
                }
				i = imageCount;
	            bool isOk = tw.Acquire(true, colorMode);
	            if (isOk)
	            {
                    cmdScan.Enabled = false;
                    cmdCancelScan.Enabled = true;
                    cmdDelete.Enabled = false;
	            }
	            else
	            {
	            	MessageBox.Show("Error in starting scanner...");
	            	EndingScan();
                    cmdScan.Enabled = true;
                    cmdCancelScan.Enabled = false;
                    cmdDelete.Enabled = true;
	            }
            }
            else
            {
            	//MessageBox.Show("No more policies remain to be scanned....");
				tw.CloseSrc();
				EndingScan();
                cmdScan.Enabled = false;
                cmdCancelScan.Enabled = false;
            }
		}

        private void cmdCancelScan_Click(object sender, EventArgs e)
        {
            tw.GetCancelNote();
            cmdDelete.Enabled = true;
        }	
		
		void AePolicyScanFormClosing(object sender, FormClosingEventArgs e)
		{
			if (tw != null)
			{
				tw.CloseSrc();
				EndingScan();
			}
		}

        private void lstImageName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string imagename = null;
            string fileName = null;
            long fileSize;

            try
            {
                if (lstImageName.SelectedIndex >= 0)
                {
                    imagename = lstImageName.SelectedItem.ToString();
                    fileName = scanFolder + "\\" + imagename;
                    tmpImg.LoadBitmapFromFile(fileName);
                    scanPic.Image = tmpImg.GetBitmap();

                    System.IO.FileInfo info = new System.IO.FileInfo(fileName);

                    fileSize = info.Length;
                    fileSize = fileSize / 1024;
                    lblSize.Text = fileSize.ToString() + " KB";
                    if (fileSize > 50)
                    {
                        lblSize.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblSize.ForeColor = Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while showing the image.." + ex.Message);
                exMailLog.Log(ex);
            }
        }

        private void lstImageName_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void lstImageName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                string imagename = null;
                string fileName = null;

                try
                {
                    if (e.KeyCode == Keys.Delete)
                    {
                        imagename = lstImageName.SelectedItem.ToString();
                        fileName = scanFolder + "\\" + imagename;

                        pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(lblCurrentPolicy.Text.ToString()), imagename, string.Empty);
                        wfeImage wImage = new wfeImage(sqlCon, pImage);
                        if (wImage.DeleteImage() == true)
                        {
                            //wImage.DeleteImage();
                            File.Delete(fileName);
                            lstImageName.Items.RemoveAt(lstImageName.SelectedIndex);
                            if (lstImageName.Items.Count > (lstImageName.SelectedIndex + 1))
                            {
                                lstImageName.SelectedIndex = lstImageName.SelectedIndex + 1;
                            }
                            lstImageName.Refresh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while showing the image.." + ex.Message);
                    exMailLog.Log(ex);
                }
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            frmRescanPolicy fmRescan = new frmRescanPolicy(wBox, sqlCon, crd,scanMode);
            fmRescan.ShowDialog(this);
            ShowPolicy();
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Point pt = new Point();
            pt.X = e.X;
            pt.Y = e.Y;
            if (e.Button == MouseButtons.Right)
            {
                ihwQuery wQ = new ihwQuery(sqlCon);
                if (wQ.GetSysConfigValue(ihConstants.SCAN_TIME_HOLD_KEY) == ihConstants.SCAN_TIME_HOLD_VALUE)
                {
                    conHold.Show(panel1, pt);
                }
            }
        }

        private void conMarkHold_Click(object sender, EventArgs e)
        {
            string policyNo = lblCurrentPolicy.Text;
            try
            {
                DialogResult ds = MessageBox.Show("Are you sure, you want to hold this policy?", "Hold policy", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (ds == DialogResult.Yes)
                {
                    if (policyNo != string.Empty)
                    {
                        CtrlPolicy ctrlPolicy = new CtrlPolicy(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo));
                        wfePolicy policy = new wfePolicy(sqlCon, ctrlPolicy);
                        if (policy.UpdateStatus(eSTATES.POLICY_ON_HOLD, crd))
                        {
                            pImage = new CtrlImage(Convert.ToInt32(wBox.ctrlBox.ProjectCode), Convert.ToInt32(wBox.ctrlBox.BatchKey.ToString()), Convert.ToInt32(wBox.ctrlBox.BoxNumber), Convert.ToInt32(policyNo), string.Empty, string.Empty);
                            wfeImage wImage = new wfeImage(sqlCon, pImage);
                            wImage.TotalImageUpdateStatus(eSTATES.PAGE_ON_HOLD);
                            lstImageName.Items.Clear();
                            scanPic.Image = null;
                            if (policy.DeleteAllPage())
                            {
                                string policyFolder = batchPath + "\\" + wBox.ctrlBox.BoxNumber + "\\" + policyNo;
                                //scanFolder = policyFolder + "\\" + ihConstants._SCAN_FOLDER;
                                if (Directory.Exists(policyFolder))
                                {
                                   Directory.Delete(policyFolder, true);
                                }
                            }
                        }
                    }
                    conHold.Hide();
                    ShowPolicy();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while updating policy status......" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
	}
}
