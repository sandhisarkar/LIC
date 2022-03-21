/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 6/4/2009
 * Time: 5:31 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace ImageHeaven
{
	/// <summary>
	/// Description of aeScanControl.
	/// </summary>
	public partial class aeScanControl : Form
	{
		string imageName = null;
		string scanPath = null;
		string sourcePath = null;
		string destinationPath = null;
		public aeScanControl(string prmImageName,string prmScanPath,string prmSourcePath,string prmDestinationPath)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.Text = "B'Zer - Scanner settings";
			imageName = prmImageName;
			scanPath = prmScanPath;
			sourcePath=prmSourcePath;
			destinationPath = prmDestinationPath;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		
		
		void CmdOkClick(object sender, EventArgs e)
		{
            //int dpiValue=0;
            //int colorValue=0;
			
            //if(rdo200Dpi.Checked == true)
            //{
            //    dpiValue = 200;
            //}
            //else
            //{
            //    dpiValue = 300;
            //}
			
            //if(rdoBlackWhite.Checked == true)
            //{
            //    colorValue = ihConstants.SCAN_BLACK_WHITE;
            //}
            //else if(rdoColor.Checked == true)
            //{
            //    colorValue = ihConstants.SCAN_COLOR;
            //}
            //else
            //{
            //    colorValue = ihConstants.SCAN_GRAY_SCALE;
            //}
			//ADFScanUtils scanUtil=new ADFScanUtils();
//			if(scanUtil.ADFSingleScan(imageName,scanPath,dpiValue,colorValue) == false)
//			{
//				if(File.Exists(sourcePath)==true)
//				{
//					File.Copy(sourcePath,destinationPath,true);
//				}
//			}
			this.Close();
		}
		
		void AeScanControlLoad(object sender, EventArgs e)
		{
			rdo200Dpi.Checked = true;
			rdoBlackWhite.Checked=true;
		}
		
		void CmdCancelClick(object sender, EventArgs e)
		{
			
			this.Close();
		}
	}
}
