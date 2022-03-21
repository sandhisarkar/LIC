 /* Created by SharpDevelop.
 * User: SubhajitB
 * Date: 18/2/2009
 * Time: 11:52 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using NovaNet.wfe;
using NovaNet.Utils;
using System.Data.Odbc;
using LItems;
using System.Net;

namespace ImageHeaven 
{
	/// <summary>
	/// Description of aeProject.
	/// </summary>
    public partial class aeProject : frmAddEdit
	{
		wItem objProject;
		OdbcConnection sqlCon=null;
		/*
		public aeProject()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			this.Text="B'Zer - Add Project";
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		*/
		public aeProject(wItem prmCmd,OdbcConnection prmCon)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
            sqlCon = prmCon;
			objProject = (wfeProject) prmCmd;
			if (objProject.GetMode()==Constants._ADDING)
				this.Text = "B'Zer - Add Project";
			else
				this.Text = "B'Zer - Edit Project";
					
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		protected override void CmdSaveClick(object sender, EventArgs e)
		{
			try
			{
				NovaNet.Utils.dbCon dbcon=new NovaNet.Utils.dbCon();
				
				
				wfeProject crtProject=new wfeProject(sqlCon);
				
				udtProject udtProj=new udtProject();
				udtProj.Code=txtProjectName.Text;
				udtProj.Project_Path=txtScannedLoc.Text;
				udtProj.Created_DTTM=dbcon.GetCurrenctDTTM(1,sqlCon);
				if (crtProject.TransferValues(udtProj)==true)
				{
					statusStrip1.Items.Clear();
					statusStrip1.Items.Add("Status: Data SucessFully Saved");
					statusStrip1.ForeColor=System.Drawing.Color.Black;
					ClearAllField();
				}
				else
				{
					statusStrip1.Items.Clear();
					statusStrip1.Items.Add("Status: Data Can not be Saved");
					statusStrip1.ForeColor=System.Drawing.Color.Red;
				}
			}
			catch (KeyCheckException ex)
			{
				MessageBox.Show(ex.Message,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			catch (DbCommitException dbex)
			{
				MessageBox.Show(dbex.Message,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			catch(CreateFolderException folex)
			{
				MessageBox.Show(folex.Message,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			catch (DBConnectionException conex)
			{
				MessageBox.Show(conex.Message,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			catch(INIFileException iniex)
			{
				MessageBox.Show(iniex.Message,"B'Zer",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			//throw new NotImplementedException();
		}
		
		void AeProjectResizeBegin(object sender, EventArgs e)
		{
            //MessageBox.Show(Parent.Name);
            base.Width=this.Width;
			base.Height=this.Height;
		}
		
		void AeProjectLoad(object sender, EventArgs e)
		{
			panelButton.Location = new System.Drawing.Point(282,130);
			cmdSave.Enabled = false;
		}
				
		void CmdBrowseScannLocClick(object sender, EventArgs e)
		{
            string path = string.Empty;
            int pos = 0;
            FolderBrowserDialog browseForComputer = new FolderBrowserDialog();
            string origIp = string.Empty;
            System.IO.FileStream fs;

            if (browseForComputer.ShowDialog() == DialogResult.OK)
            {
                txtScannedLoc.Text = browseForComputer.SelectedPath;
                path = txtScannedLoc.Text;
                if (path != string.Empty)
                {
                    cmdSave.Enabled = true;
                    pos = path.IndexOf("\\\\");
                    if (pos != -1)
                    {
                        int posSnd = path.IndexOf("\\",2);
                        string compName = path.Substring(pos+2, posSnd-2);
                        string restPath = path.Substring(posSnd);
                        if (compName != string.Empty)
                        {
                            try
                            {
                                IPHostEntry ip = Dns.GetHostEntry(compName);
                                IPAddress[] IpA = ip.AddressList;
                                for (int i = 0; i < IpA.Length; i++)
                                {
                                    origIp = IpA[i].ToString();
                                }
                                path = "\\\\" + origIp + restPath;
                                fs = new System.IO.FileStream(path + "\\temp.txt", System.IO.FileMode.Append);
                                fs.Close();
                                System.IO.File.Delete(path +"\\temp.txt");
                                cmdSave.Enabled = true;
                                txtScannedLoc.Text = path;
                            }
                            catch (Exception ex)
                            {
                                cmdSave.Enabled = false;
                                MessageBox.Show(ex.Message.ToString());
                            }
                        }
                    }
                    else
                    {
                        cmdSave.Enabled = false;
                        MessageBox.Show("Selected drive is invalid. Please select folder from network drive..");
                    }
                }
            }
            else
            {
                cmdSave.Enabled = false;
            }
		}
//		void CmdCleanLocClick(object sender, System.EventArgs e)
//		{
//			folderBrowserDialog.ShowDialog();
//			txtCleanedLoc.Text=folderBrowserDialog.SelectedPath;
//		}
//		
//		void CmdExportLocClick(object sender, System.EventArgs e)
//		{
//			folderBrowserDialog.ShowDialog();
//			txtExportLoc.Text=folderBrowserDialog.SelectedPath;
//		}	
		
		
		void TxtProjectNameLeave(object sender, EventArgs e)
		{
//			Utils.dbCon dbcon=new Utils.dbCon();
//			sqlCon=new OdbcConnection();
//			sqlCon=dbcon.Connect();
//			wfeProject proj=new wfeProject(sqlCon);
//			if (proj.KeyCheck(txtProjectName.Text)==true)
//			{
//				statusStrip1.Items.Add("Error: Data Already Exists");
//				statusStrip1.ForeColor=System.Drawing.Color.Red;
//			}
//			else
//			{
//				statusStrip1.Items.Clear();
//			}
		}
		void TxtProjectNameKeyPress(object sender, KeyPressEventArgs e)
		{
			if ((e.KeyChar==92) || (e.KeyChar==39) || (e.KeyChar==47))
			{
				e.Handled=true;
			}
		}
		private void ClearAllField()
		{
			txtProjectName.Text=string.Empty;
			txtScannedLoc.Text=string.Empty;
		}
	}
}
