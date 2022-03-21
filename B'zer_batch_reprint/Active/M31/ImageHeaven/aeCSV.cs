/*
 * Created by SharpDevelop.
 * User: user
 * Date: 01/03/2009
 * Time: 6:36 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using NovaNet.Utils;
using System.Data;
using System.Data.Odbc;
using LItems;
using NovaNet.wfe;
using System.IO;

namespace ImageHeaven
{
	
	
	/// <summary>
	/// Description of aeCSV.
	/// </summary>
	public partial class aeCSV : Form,StateData
	{
		NovaNet.Utils.dbCon dbcon;
		//Credentials udtCrd;
		ControlInfo udtInfo;
        MemoryStream stateLog;
        byte[] tmpWrite;
		OdbcConnection sqlCon=null;
        Credentials crd = new Credentials();
        public static NovaNet.Utils.exLog.Logger exMailLog = new NovaNet.Utils.exLog.emailLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev, Constants._MAIL_TO, Constants._MAIL_FROM, Constants._SMTP);
        public static NovaNet.Utils.exLog.Logger exTxtLog = new NovaNet.Utils.exLog.txtLogger("./errLog.log", NovaNet.Utils.exLog.LogLevel.Dev);
        public string fDoCode;
        public string fBoCode;

		public aeCSV(OdbcConnection prmCon,Credentials prmCrd)
		{
			InitializeComponent();
            sqlCon = prmCon;
			this.Text="B'Zer - CSV Uploader";
            crd = prmCrd;
            exMailLog.SetNextLogger(exTxtLog);
            
		}
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
//		void AeCSVResizeBegin(object sender, EventArgs e)
//		{
//			Parent.Width=this.Width;
//			Parent.Height=this.Height;
//		}
		
//		void AeCSVLoad(object sender, EventArgs e)
//		{
//			panelButton.Location = new System.Drawing.Point(446,403);
//		}
		//Purpose is to set the percentage of progress in the progress bar
		public void SetPrecentage(int prmPercentage)
		{
			progressBar1.Value=prmPercentage;
		}
		protected  void CmdSaveClick(object sender, EventArgs e)
		{
			NotifyProgress delPercentage = new NotifyProgress(SetPrecentage);
			
			
				try 
				{
					if((cmbProject.Text!=string.Empty) && (cmbBatch.Text!=string.Empty))
					{
						NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
						
						
						wItemCreator boxCreate=new BoxCreator(sqlCon);
						boxCreate.RegisterNotification(delPercentage);
						
						udtInfo=new ControlInfo();
						
						udtInfo.batch_Key=Convert.ToInt32(cmbBatch.SelectedValue.ToString());
						udtInfo.proj_Key=Convert.ToInt32(cmbProject.SelectedValue.ToString());
						udtInfo.csvPath=txtPath.Text;
						
						statusStrip1.Items.Add("Status: Wait While Uploading the CSV......");
						
						int policyStatus = (int)eSTATES.POLICY_INITIALIZED;
						int boxStatus = (int)eSTATES.BOX_CREATED;
						if (boxCreate.CreateBox(crd,udtInfo,boxStatus,policyStatus)==true)
						{
							statusStrip1.Items.Clear();
							statusStrip1.Items.Add("Status: CSV SucessFully Uploaded");
						}
						else
						{
							statusStrip1.Items.Clear();
							statusStrip1.Items.Add("Status: Uploading Cannot be Completed");
						}
					}
				}
				catch(DbCommitException svex)
				{
					statusStrip1.Items.Clear();
					MessageBox.Show(svex.Message,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    stateLog = new MemoryStream();
                    tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while Commit");
                    stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                    exMailLog.Log(svex, this);
				}
				catch (KeyCheckException keyex)
				{
					statusStrip1.Items.Clear();
					MessageBox.Show(keyex.Message,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    stateLog = new MemoryStream();
                    tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while Key Check");
                    stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                    exMailLog.Log(keyex, this);
				}	
				catch(DuplicateCsvException dupex)
				{
					statusStrip1.Items.Clear();
					MessageBox.Show(dupex.Message,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    stateLog = new MemoryStream();
                    tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Duplicate CSV");
                    stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                    exMailLog.Log(dupex, this);
				}
		}
		
		void CmdPathClick(object sender, EventArgs e)
		{
			dlgCSV.Filter="CSV File|*.CSV";
			dlgCSV.FileName=string.Empty;
			dlgCSV.Title="B'Zer - CSV Files";
			dlgCSV.ShowDialog();
			txtPath.Text=dlgCSV.FileName.ToString();
            string fName= Path.GetFileName(txtPath.Text.Trim());
            string bName ="soft-" + cmbBatch.Text + ".csv";
            if (fName.ToUpper() == bName.ToUpper())
            {
                cmdSave.Enabled = true;
                cmdUpload.Enabled = true;
            }
            else
            {
                cmdSave.Enabled = false;
                cmdUpload.Enabled = false;
                MessageBox.Show("This csv file is not for this batch...");
            }
		}
		
		void CmdUploadClick(object sender, EventArgs e)
		{
			DataColumn pgCount=new DataColumn("PageCount");
			
			try 
			{
				if (txtPath.Text.Trim()!=string.Empty)
				{
					csvReader readCsv=new csvReader(txtPath.Text);
					DataSet dsReader=new DataSet();
					dsReader=readCsv.ReadData();
                    if (ValidateCsv(dsReader))
                    {
                        cmdSave.Enabled = true;
                    }
                    else
                    {
                        cmdSave.Enabled = false;
                    }
					grdCsv.DataSource=dsReader.Tables[0];
				}
			}
			catch(CSVReadException csvex)
			{
				MessageBox.Show(csvex.Message,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
                stateLog = new MemoryStream();
                tmpWrite = new System.Text.ASCIIEncoding().GetBytes("Error while read CSV");
                stateLog.Write(tmpWrite, 0, tmpWrite.Length);
                exMailLog.Log(csvex, this);
			}
            catch (Exception ex)
            {
                MessageBox.Show("Error while reading from the CSV file " + ex.Message, "Read error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                exMailLog.Log(ex);
            }
		}
        bool ValidateCsv(DataSet pDs)
        {
            string csvDoCode = string.Empty;
            string csvBocode = string.Empty;
            bool valid = true;
            string fName = Path.GetFileName(txtPath.Text.Trim());

            for (int i = 0; i < pDs.Tables[0].Rows.Count; i++)
            {
                csvDoCode = pDs.Tables[0].Rows[i][1].ToString();
                csvBocode = pDs.Tables[0].Rows[i][2].ToString();
                if ((csvDoCode.Length == 0) || (csvDoCode.Length != 3))
                {
                    MessageBox.Show("Length of DO code in input CSV is wrong.....");
                    valid = false;
                    break;
                }
                if (csvBocode.Length == 0)
                {
                    MessageBox.Show("Length of BO code in input CSV is wrong.....");
                    valid = false;
                    break;
                }
                if (fName.IndexOf(csvBocode) <= 0)
                {
                    MessageBox.Show("Mismatch found with BO code in input CSV and CSV file name.....");
                    valid = false;
                    break;
                }
                if ((i > 0) && (csvBocode != pDs.Tables[0].Rows[i-1][2].ToString()))
                {
                    MessageBox.Show("BO code mismatch between rows in input CSV.....");
                    valid = false;
                    break;
                }
                if (fName.IndexOf(csvDoCode) <= 0)
                {
                    MessageBox.Show("Mismatch found with DO code in input CSV and CSV file name.....");
                    valid = false;
                    break;
                }
                if (fName.IndexOf(pDs.Tables[0].Rows[i][3].ToString()) <= 0 )
                {
                    string y = pDs.Tables[0].Rows[i][3].ToString();
                    MessageBox.Show("Invalid serial number found in the input CSV name.....");
                    valid = false;
                    break;
                }
                if (pDs.Tables[0].Rows[i][3].ToString().Length != 4)
                {
                    MessageBox.Show("Invalid length in serial number in the input CSV name.....");
                    valid = false;
                    break;
                }
                if (pDs.Tables[0].Rows[i][5].ToString().Length != 9)
                {
                    MessageBox.Show("Invalid policy number found in the input CSV.....");
                    valid = false;
                    break;
                }
                //if ((i > 0) && (Convert.ToInt32(pDs.Tables[0].Rows[i][5].ToString()) !=Convert.ToInt32(pDs.Tables[0].Rows[i - 1][5].ToString()) +1))
                //{
                //    MessageBox.Show("Policy number not in order in input CSV.....");
                //    valid = false;
                //    break;
                //}
                if (pDs.Tables[0].Rows[i][7].ToString().Contains(@"\"))
                {
                    MessageBox.Show("Special charecter found in the name field in input CSV name.....");
                    valid = false;
                    break;
                }
                if (pDs.Tables[0].Rows[i][7].ToString().Contains("/"))
                {
                    MessageBox.Show("Special charecter found in the name field in input CSV name.....");
                    valid = false;
                    break;
                }
                if (pDs.Tables[0].Rows[i]["CartonNo"].ToString().Length != 3)
                {
                    MessageBox.Show("Invalid length found in box number field in input CSV name.....");
                    valid = false;
                    break;
                }
            }
            return valid;
        }
        MemoryStream StateData.StateLog()
        {
            return stateLog;
        }
		void AeCSVLoad(object sender, EventArgs e)
		{
			PopulateProjectCombo();
			PopulateBatchCombo();
			statusStrip1.Text=string.Empty;
            cmdSave.Enabled = false;
            cmdUpload.Enabled = false;
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
		void CmbProjectLeave(object sender, EventArgs e)
		{
			PopulateBatchCombo();
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
                ds = tmpBatch.GetAllValues(Convert.ToInt32(projKey));
                cmbBatch.DataSource = ds.Tables[0];
                cmbBatch.DisplayMember = ds.Tables[0].Columns[1].ToString();
                cmbBatch.ValueMember = ds.Tables[0].Columns[0].ToString();
            }	//cmbBatch.Items.Insert(0,"Select");
		}
		
		
		
		void GrdCsvSelectionChanged(object sender, EventArgs e)
		{
			//MessageBox.Show("Hi");
		}
		
		void GrdCsvDataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			if (e.Context == DataGridViewDataErrorContexts.Commit)
			{
				MessageBox.Show ("Page Count Should be in Between 0-99","Error");
			}
		}
		
		void GrdCsvCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if(e.ColumnIndex==22)
			{
				e.CellStyle.ForeColor=Color.Red;
			}
		}

		void CmdCancelClick(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
