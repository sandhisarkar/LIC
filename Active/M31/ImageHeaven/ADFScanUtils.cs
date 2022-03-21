/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 2/4/2009
 * Time: 4:52 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NovaNet.Utils;
using NovaNet.wfe;
using WIA;
using LItems;
using System.Data.Odbc;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Collections;

using System.Windows.Forms;namespace ImageHeaven
{
	/// <summary>
	/// Description of ADFScanUtils.
	/// </summary>
	public class ADFScanUtils
	{
		private string DeviceID;
		private NotifyImageDetails delImageDtls=null;
		private NotifyPageCountMismatch delPageCount=null;
		private aePolicyScan aeScan=null;
		private string err=null;
		
		
		public ADFScanUtils()
		{
			
		}

		public bool RegisterNotification(NotifyImageDetails prmNt)
		{
			delImageDtls = prmNt;
			return true;
		}
		public bool RegisterNotification(NotifyPageCountMismatch prmNt)
		{
			delPageCount = prmNt;
			return true;
		}
		public bool ADFBatchScan(string prmPolicy,string prmPath,int prmPageCount,bool prmHasPhoto,int prmPhotoPage)
		{
			
			object Object1 = null;
  			object Object2 = null;
			bool hasMorePages = true;
			int x = 1;
			int numPages = 0;
			bool exitPoliy=true;
			char leftPad = Convert.ToChar("0");
			long imageSize;
			IItem item;
			bool blankBol=true;
			Device d;
			WIA.ImageFile img = null;
			bool fatalError = false;
			Device WiaDev = null;
			
			
				while (exitPoliy == true)
				{
					//Choose the scanner
					
					
						WIA.CommonDialog WiaCommonDialog = new WIA.CommonDialog();
						MessageBox.Show("Hi");
						try
						{
							d = WiaCommonDialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false,false);
						
							if (d != null)
							{
								this.DeviceID = d.DeviceID;
							}
							DeviceManager manager = new DeviceManagerClass();
							
							foreach (DeviceInfo info in manager.DeviceInfos)
							{
								if (info.DeviceID == this.DeviceID)
								{
									WIA.Properties infoprop = null;
									infoprop = info.Properties;
									
									//connect to scanner
									WiaDev = info.Connect();
									break;
								}
							}
						}
						catch (Exception ex)
						{
							err=ex.Message.ToString();
							MessageBox.Show("Error while connecting to the scanner","B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
							return fatalError = true;
						}
					
					//Start Scan
					if(fatalError == false)
					{
						try
						{
							WIA.Item Item = WiaDev.Items[1] as WIA.Item;
							item = d.Items[1];
							if((prmHasPhoto == true) && (x == prmPhotoPage))
							{
								setItem(item,"6146",1);
								setItem(item, "6147", 200);
			            		setItem(item, "6148",200);
							}
							else
							{
								setItem(item,"6146",4);
								setItem(item, "6147", 200);
			            		setItem(item, "6148",200);
							}
							WIA.ImageProcess ImageProcess1 = new WIA.ImageProcess();
							Object1 = (Object)"Convert";
							ImageProcess1.Filters.Add(ImageProcess1.FilterInfos.get_Item(ref Object1).FilterID,0);
							Object1 = (Object)"FormatID";
							Object2 = (Object)WIA.FormatID.wiaFormatTIFF;
							ImageProcess1.Filters[1].Properties.get_Item(ref Object1).set_Value(ref Object2);
							
							if((x == prmPhotoPage) && (prmHasPhoto == true))
							{
								Object1 = (Object)"Compression";
								Object2 = (Object)"LZW";
							}
							else
							{
								Object1 = (Object)"Compression";
								Object2 = (Object)"CCITT4";
							}
							if(prmHasPhoto == false)
							{
								Object1 = (Object)"Compression";
								Object2 = (Object)"CCITT4";
							}
							ImageProcess1.Filters[1].Properties.get_Item(ref Object1).set_Value(ref Object2);
							
							Object1 = null;
							Object2 = null; 
							
							//img = (ImageFile)WiaCommonDialog.ShowTransfer(Item,wiaFormatJPEG,false);
							img = (ImageFile)d.Items[1].Transfer(ihConstants.wiaFormatTIFF);
							WIA.ImageProcess pros=new ImageProcessClass();
							
							//Save to file
							img = ImageProcess1.Apply(img);
							
							//Get color information
							Image imgProcess = Image.FromStream(new System.IO.MemoryStream((byte[])img.FileData.get_BinaryData()));
							
							
							string varImageFileName = prmPath + "\\" + prmPolicy + "_" + x.ToString().PadLeft(3,leftPad) + "_"  + "A" + ".TIF";
							
							if (File.Exists(varImageFileName))
							{
								File.Delete(varImageFileName);
							}
							img.SaveFile(varImageFileName);
							imageSize = img.Height * img.Width;
							//Call back here to notify image name and size
							aeScan = new aePolicyScan();
							
							
							blankBol = DetectBlankPage(varImageFileName);
							
							if(blankBol == true)
							{
								exitPoliy = GetBatchSplitter(varImageFileName);
								if(exitPoliy == false)
								{
									MessageBox.Show("Black page found");
									if(numPages != prmPageCount)
									{
										MessageBox.Show("Scanned page count mismatch with total page count entired at the time of inventry in","Count mismatch",MessageBoxButtons.OK);
										delPageCount(prmPageCount,numPages);
									}
									File.Delete(varImageFileName);
								}
							}
							else if(blankBol == false)
							{
								MessageBox.Show("Blank page found");
								x = x - 1;
								File.Delete(varImageFileName);
							}
							if((blankBol == true) && (exitPoliy == true))
							{
								delImageDtls(prmPolicy + "_" + x.ToString().PadLeft(3,leftPad) + "_"  + "A" + ".TIF",imageSize);
							}
							//numPages++;
							img = null;
							
							Item = null;
							//determine if there are any more pages waiting
							Property documentHandlingSelect = null;
							Property documentHandlingStatus = null;
							foreach (Property prop in WiaDev.Properties)
							{
								if (prop.PropertyID == WIA_PROPERTIES.WIA_DPS_DOCUMENT_HANDLING_SELECT)
								{
									documentHandlingSelect = prop;
		
								}
								if (prop.PropertyID == WIA_PROPERTIES.WIA_DPS_DOCUMENT_HANDLING_STATUS)
									documentHandlingStatus = prop;
							}
							//tw.Close();
							hasMorePages = false; //assume there are no more pages
							if (documentHandlingSelect != null)
								//may not exist on flatbed scanner but required for feeder
							{
								//check for document feeder
								if ((Convert.ToUInt32(documentHandlingSelect.get_Value()) & WIA_DPS_DOCUMENT_HANDLING_SELECT.FEEDER) != 0)
								{
									hasMorePages = ((Convert.ToUInt32(documentHandlingStatus.get_Value()) & WIA_DPS_DOCUMENT_HANDLING_STATUS.FEED_READY) != 0);
								}
							}
							x++;
							numPages++;
						}
						catch (Exception ex)
						{
							err = ex.Message.ToString();
							MessageBox.Show("Feeder is empty, please refill it and press Ok","B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Information);
							continue;
						}
					}
				else
				{
					break;
				}
			}
			return fatalError;
		}
		
		public bool ADFSingleScan(string prmImageName,string prmPath,int prmDpi,int prmScanMode)
		{
			
			object Object1 = null;
  			object Object2 = null;
			bool hasMorePages = true;
			int x = 1;
			int numPages = 0;
			char leftPad = Convert.ToChar("0");
			long imageSize;
			IItem item;
			Device d;
			WIA.ImageFile img = null;
			bool fatalError = false;
			Device WiaDev = null;
			
						WIA.CommonDialog WiaCommonDialog = new WIA.CommonDialog();
						MessageBox.Show("Hi");
						try
						{
							d = WiaCommonDialog.ShowSelectDevice(WiaDeviceType.ScannerDeviceType, false,false);
						
							if (d != null)
							{
								this.DeviceID = d.DeviceID;
							}
							DeviceManager manager = new DeviceManagerClass();
							
							foreach (DeviceInfo info in manager.DeviceInfos)
							{
								if (info.DeviceID == this.DeviceID)
								{
									WIA.Properties infoprop = null;
									infoprop = info.Properties;
									
									//connect to scanner
									WiaDev = info.Connect();
									break;
								}
							}
						}
						catch (Exception ex)
						{
							err=ex.Message.ToString();
							MessageBox.Show("Error while connecting to the scanner","B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
							return fatalError = true;
						}
					
					//Start Scan
					if(fatalError == false)
					{
						try
						{
							WIA.Item Item = WiaDev.Items[1] as WIA.Item;
							item = d.Items[1];
							setItem(item,"6146",prmScanMode);
							setItem(item, "6147", prmDpi);
		            		setItem(item, "6148", prmDpi);
							WIA.ImageProcess ImageProcess1 = new WIA.ImageProcess();
							Object1 = (Object)"Convert";
							ImageProcess1.Filters.Add(ImageProcess1.FilterInfos.get_Item(ref Object1).FilterID,0);
							Object1 = (Object)"FormatID";
							Object2 = (Object)WIA.FormatID.wiaFormatTIFF;
							ImageProcess1.Filters[1].Properties.get_Item(ref Object1).set_Value(ref Object2);
							if(prmScanMode == ihConstants.SCAN_BLACK_WHITE)
							{
								Object1 = (Object)"Compression";
								Object2 = (Object)"CCITT4";
								ImageProcess1.Filters[1].Properties.get_Item(ref Object1).set_Value(ref Object2);
							}
							Object1 = null;
							Object2 = null; 
							
							//img = (ImageFile)WiaCommonDialog.ShowTransfer(Item,wiaFormatJPEG,false);
							img = (ImageFile)d.Items[1].Transfer(ihConstants.wiaFormatTIFF);
							WIA.ImageProcess pros=new ImageProcessClass();
							
							//Save to file
							img = ImageProcess1.Apply(img);
							
							//Get color information
							Image imgProcess = Image.FromStream(new System.IO.MemoryStream((byte[])img.FileData.get_BinaryData()));
							
							
							string varImageFileName = prmPath + "\\" + prmImageName;
							
							if (File.Exists(varImageFileName))
							{
								File.Delete(varImageFileName);
							}
							img.SaveFile(varImageFileName);
							imageSize = img.Height * img.Width;
							//Call back here to notify image name and size
							aeScan = new aePolicyScan();
							
							
							//blankBol = DetectBlankPage(varImageFileName);
							
//							if(blankBol == true)
//							{
//								exitPoliy = GetBatchSplitter(varImageFileName);
//								if(exitPoliy == false)
//								{
//									MessageBox.Show("Black page found");
//									File.Delete(varImageFileName);
//								}
//							}
//							else if(blankBol == false)
//							{
//								MessageBox.Show("Blank page found");
//								x = x - 1;
//								File.Delete(varImageFileName);
//							}
//							if((blankBol == true) && (exitPoliy == true))
//							{
//								delImageDtls(prmPolicy + "_" + x.ToString().PadLeft(3,leftPad) + "_"  + "A" + ".TIF",imageSize);
//							}
							numPages++;
							img = null;
							
							Item = null;
							//determine if there are any more pages waiting
							Property documentHandlingSelect = null;
							Property documentHandlingStatus = null;
							foreach (Property prop in WiaDev.Properties)
							{
								if (prop.PropertyID == WIA_PROPERTIES.WIA_DPS_DOCUMENT_HANDLING_SELECT)
								{
									documentHandlingSelect = prop;
		
								}
								if (prop.PropertyID == WIA_PROPERTIES.WIA_DPS_DOCUMENT_HANDLING_STATUS)
									documentHandlingStatus = prop;
							}
							//tw.Close();
							hasMorePages = false; //assume there are no more pages
							if (documentHandlingSelect != null)
								//may not exist on flatbed scanner but required for feeder
							{
								//check for document feeder
								if ((Convert.ToUInt32(documentHandlingSelect.get_Value()) & WIA_DPS_DOCUMENT_HANDLING_SELECT.FEEDER) != 0)
								{
									hasMorePages = ((Convert.ToUInt32(documentHandlingStatus.get_Value()) & WIA_DPS_DOCUMENT_HANDLING_STATUS.FEED_READY) != 0);
								}
							}
							x++;
						}
						catch (Exception ex)
						{
							err = ex.Message.ToString();
							MessageBox.Show("Feeder is empty, please refill it and press Ok","B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Information);
							fatalError = false;
						}
					}
			return fatalError;
		}
		private bool GetBatchSplitter(string prmFile)
		{
			Bitmap bmp = new Bitmap(prmFile);
			bool bmpImg=true;
			//int [,] SamplingMatrix = GetSamplingMatrix();
			DateTime dt = DateTime.Now;
			int p=0;
			//SamplingMatrix[i,0],SamplingMatrix[i,1]
			
			for(int i=600;i < 700;i++)
			{
				for(int j=600;j < 700;j++)
				{
					string name = bmp.GetPixel(i,j).Name;
					if(bmp.GetPixel(i,j).Name == "ff000000")
					{
						bmpImg = false;
					}
					else
					{
						if(p == 100)
						{
							bmpImg = true;
							break;
						}
						p=p+1;
					}
				}
				if (bmpImg == true)
					break;
			}
//			TimeSpan duration = DateTime.Now - dt;
//			MessageBox.Show(duration.ToString());
			bmp.Dispose();
			return bmpImg;
		}
		private bool DetectBlankPage(string prmFileName)
		{
			Bitmap bmp = new Bitmap(prmFileName);
			bool bmpImg=true;
			//int [,] SamplingMatrix = GetSamplingMatrix();
			DateTime dt = DateTime.Now;
			int p=0;
			//SamplingMatrix[i,0],SamplingMatrix[i,1]
			
			for(int i=200;i < 600;i++)
			{
				for(int j=200;j < 600;j++)
				{
					if(bmp.GetPixel(i,j).Name == "ffffffff")
					{
						bmpImg = false;
					}
					else
					{
						if(p == 25)
						{
							bmpImg = true;
							break;
						}
						p=p+1;
					}
				}
				if (bmpImg == true)
					break;
			}
//			TimeSpan duration = DateTime.Now - dt;
//			MessageBox.Show(duration.ToString());
			bmp.Dispose();
			return bmpImg;
		}
		private void setItem(IItem item, object property, object value)
        {
            WIA.Property aProperty = item.Properties.get_Item(ref property);
            aProperty.set_Value(ref value);
        }
//		int [,] GetSamplingMatrix()
//		{
//			int min=0;
//			int max=1000;
//			Random random = new Random();
//			
//			int [,] tmp = new int[500,2];
//			for (int i=0; i<500; i++)
//			{
//				for (int j=0; j < 2; j++)
//				{
//					tmp[i,j]=random.Next(min, max); 
//				}
//			}
//			return tmp;
//		}
	}
	
	class WIA_DPS_DOCUMENT_HANDLING_SELECT
	{
		public const uint FEEDER = 0x00000001;
		public const uint FLATBED = 0x00000002;
	}

	class WIA_DPS_DOCUMENT_HANDLING_STATUS
	{
		public const uint FEED_READY = 0x00000001;
	}

	class WIA_PROPERTIES
	{
		public const uint WIA_RESERVED_FOR_NEW_PROPS = 1024;
		public const uint WIA_DIP_FIRST = 2;
		public const uint WIA_DPA_FIRST  =  WIA_DIP_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
		public const uint WIA_DPC_FIRST  = WIA_DPA_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
		//
		// Scanner only device properties (DPS)
		//
		public const uint WIA_DPS_FIRST    =                      WIA_DPC_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
		public const uint WIA_DPS_DOCUMENT_HANDLING_STATUS  =     WIA_DPS_FIRST + 13;
		public const uint WIA_DPS_DOCUMENT_HANDLING_SELECT  =     WIA_DPS_FIRST + 14;
	}

	class WIA_ERRORS
	{
		public const uint BASE_VAL_WIA_ERROR = 0x80210000;
		public const uint WIA_ERROR_PAPER_EMPTY  = BASE_VAL_WIA_ERROR + 3;
	}
}
