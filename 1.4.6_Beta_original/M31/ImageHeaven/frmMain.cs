/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 18/2/2009
 * Time: 11:43 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using NovaNet.wfe;
using NovaNet.Utils;
using System.Data.Odbc;
using System.Data;
using LItems;
using System.Reflection;
using LoadPlugin;
using NP;
using System.Collections.Generic;
using System.IO;

namespace ImageHeaven
{
	/// <summary>
	/// Description of frmMain.
	/// </summary>
	public partial class frmMain : Form
	{
		static wItem wi;
		//NovaNet.Utils.dbCon dbcon;
		frmMain mainForm;
		OdbcConnection sqlCon=null;
        private Credentials crd = new Credentials();
        static int colorMode;
        dbCon dbcon;
        //
        NovaNet.Utils.GetProfile pData;
        NovaNet.Utils.ChangePassword pCPwd;
        NovaNet.Utils.Profile p;
        public static NovaNet.Utils.IntrRBAC rbc;
        private short logincounter;
        //

		public static string projectName=null;
		public static string batchName=null;
		public static string boxNumber=null;
		public static string projectVal=null;
		public static string batchVal=null;
        List<NIPlugin> np = new List<NIPlugin>();
		
		public frmMain(OdbcConnection pCon)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
            AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
            this.Text ="B'Zer" + "           Version: " + assemName.Version.ToString();
			InitializeComponent();
            sqlCon = pCon;
            
            logincounter = 0;
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
        public frmMain()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            AssemblyName assemName = Assembly.GetExecutingAssembly().GetName();
            this.Text = "B'Zer" + "           Version: " + assemName.Version.ToString();
            InitializeComponent();

            logincounter = 0;
            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //
        }
//		public frmMain(string prmProject)
//		{
//			projectVal=prmProject;
//		}
		
		public void SetValues(wItem pBox)
		{
			wi = (wfeBox) pBox;
			
		}
        public void SetValues(wItem pBox,int prmMode)
        {
            wi = (wfeBox)pBox;
            colorMode = prmMode;
        }  
		void ProjectToolStripMenuItemClick(object sender, EventArgs e)
		{
            try
            {
                aeProject dispProject;
                wi = new wfeProject(sqlCon);
                dispProject = new aeProject(wi, sqlCon);
                dispProject.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
		}
		void ProjectToolStripMenuItem1Click(object sender, EventArgs e)
		{
			aePageCount pagecount = new aePageCount(sqlCon,crd);
			pagecount.ShowDialog(this);		
		}
        private List<string> GetFiles(string Path, string ext)
        {
            List<string> fls = new List<string>();
            if (Directory.Exists(Path))
            {
                foreach (string f in Directory.GetFiles(Path, "*." + ext))
                {
                    fls.Add(new FileInfo(f).FullName);
                }
            }
            else
            {
                throw new System.Exception("Folder doesn't exist");
            }
            return fls;
        }
		void FrmMainLoad(object sender, EventArgs e)
		{
            int k;
            dbcon = new NovaNet.Utils.dbCon();
            //System.Collections.Generic.List<NIPlugin> plgList = new System.Collections.Generic.List<NIPlugin>();
            try
            {

                string dllPaths = string.Empty;

                if (sqlCon.State == ConnectionState.Open)
                {
                    pData = getData;
                    pCPwd = getCPwd;
                    rbc = new NovaNet.Utils.RBAC(sqlCon, dbcon, pData, pCPwd);
                    GetChallenge gc = new GetChallenge(getData);
                    gc.ShowDialog(this);
                    ///get credential for the logged user
                    crd = rbc.getCredentials(p);
                    NPlugin nPlug = null;
                    dllPaths = Path.GetDirectoryName(Application.ExecutablePath) + @"\Reports";
                    List<string> fls = GetFiles(dllPaths, "DLL");
                    IEnumerator<string> itrFname = fls.GetEnumerator();
                    ToolStripMenuItem mnu = (ToolStripMenuItem)menuStrip1.Items[4];
                    ToolStripMenuItem tlc = null;
                    tlc = (ToolStripMenuItem)mnu.DropDownItems.Add("Plugin Reports");
                    while (itrFname.MoveNext())
                    {
                        nPlug = new NPlugin(itrFname.Current);
                        np = nPlug.GetPlugin;
                        IEnumerator<NIPlugin> itr = np.GetEnumerator();

                        while (itr.MoveNext())
                        {
                            NIPluginReport nr = (NIPluginReport)itr.Current;
                            nr.Init(sqlCon);
                            tlc.DropDownItems.Add(nr.EntryPoint);
                            if (nr.GetParams == true)
                                tlc.DropDownItems[tlc.DropDownItems.Count - 1].Click += delegate { nr.Show(this, p); };
                        }
                    }
                    ///changed in version 1.0.2
                    stsName.Text = "User name - " + p.UserName;
                    stsRole.Text = "Role - " + p.Role_des;
                    if (p.Role_des != ihConstants._ADMINISTRATOR_ROLE)
                    {
                        DataSet ds = rbc.getResource(p.UserId);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                for (int j = 0; j < menuStrip1.Items.Count; j++)
                                {
                                    ///For parent menus
                                    if ((ds.Tables[0].Rows[i]["resource_id"].ToString()) == menuStrip1.Items[j].Name.ToString())
                                    {
                                        menuStrip1.Items[j].Visible = true;
                                        menuStrip1.Items[j].Enabled = true;
                                    }
                                }
                                ///For New menus
                                for (k = 0; k < itemsToolStripMenuItem.DropDownItems.Count; k++)
                                {
                                    if ((ds.Tables[0].Rows[i]["resource_id"].ToString()) == itemsToolStripMenuItem.DropDownItems[k].Name.ToString())
                                    {
                                        itemsToolStripMenuItem.DropDownItems[k].Visible = true;
                                        itemsToolStripMenuItem.DropDownItems[k].Enabled = true;
                                    }
                                }

                                ///For transaction menus
                                for (k = 0; k < transactinToolStripMenuItem.DropDownItems.Count; k++)
                                {
                                    if ((ds.Tables[0].Rows[i]["resource_id"].ToString()) == transactinToolStripMenuItem.DropDownItems[k].Name.ToString())
                                    {
                                        transactinToolStripMenuItem.DropDownItems[k].Visible = true;
                                        transactinToolStripMenuItem.DropDownItems[k].Enabled = true;
                                    }
                                }

                                ///For LIC menus
                                for (k = 0; k < toolStripMenuItem2.DropDownItems.Count; k++)
                                {
                                    if ((ds.Tables[0].Rows[i]["resource_id"].ToString()) == toolStripMenuItem2.DropDownItems[k].Name.ToString())
                                    {
                                        toolStripMenuItem2.DropDownItems[k].Visible = true;
                                        toolStripMenuItem2.DropDownItems[k].Enabled = true;
                                    }
                                }

                                ///For Tools menus
                                for (k = 0; k < toolsToolStripMenuItem.DropDownItems.Count; k++)
                                {
                                    if ((ds.Tables[0].Rows[i]["resource_id"].ToString()) == toolsToolStripMenuItem.DropDownItems[k].Name.ToString())
                                    {
                                        toolsToolStripMenuItem.DropDownItems[k].Visible = true;
                                        toolsToolStripMenuItem.DropDownItems[k].Enabled = true;
                                    }
                                }

                                ///For Tools menus
                                reportsToolStripMenuItem.Visible = true;
                                for (k = 0; k < reportsToolStripMenuItem.DropDownItems.Count; k++)
                                {
                                    if ((ds.Tables[0].Rows[i]["resource_id"].ToString()) == reportsToolStripMenuItem.DropDownItems[k].Name.ToString())
                                    {
                                        reportsToolStripMenuItem.DropDownItems[k].Visible = true;
                                        reportsToolStripMenuItem.DropDownItems[k].Enabled = true;
                                    }
                                }

                                ///For enable/disable toolstrip button
                                for (k = 0; k < toolStrip1.Items.Count; k++)
                                {
                                    if ((ds.Tables[0].Rows[i]["resource_id"].ToString()) == toolStrip1.Items[k].Name.ToString())
                                    {
                                        toolStrip1.Items[k].Visible = true;
                                        toolStrip1.Items[k].Enabled = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < menuStrip1.Items.Count; j++)
                        {
                            ///For parent menus
                            menuStrip1.Items[j].Visible = true;
                            menuStrip1.Items[j].Enabled = true;
                        }
                        ///For New menus
                        for (k = 0; k < itemsToolStripMenuItem.DropDownItems.Count; k++)
                        {
                            itemsToolStripMenuItem.DropDownItems[k].Visible = true;
                            itemsToolStripMenuItem.DropDownItems[k].Enabled = true;
                        }

                        ///For transaction menus
                        for (k = 0; k < transactinToolStripMenuItem.DropDownItems.Count; k++)
                        {
                            transactinToolStripMenuItem.DropDownItems[k].Visible = true;
                            transactinToolStripMenuItem.DropDownItems[k].Enabled = true;
                        }

                        ///For LIC menus
                        for (k = 0; k < toolStripMenuItem2.DropDownItems.Count; k++)
                        {
                            toolStripMenuItem2.DropDownItems[k].Visible = true;
                            toolStripMenuItem2.DropDownItems[k].Enabled = true;
                        }

                        ///For Tools menus
                        for (k = 0; k < toolsToolStripMenuItem.DropDownItems.Count; k++)
                        {
                            toolsToolStripMenuItem.DropDownItems[k].Visible = true;
                            toolsToolStripMenuItem.DropDownItems[k].Enabled = true;
                        }

                        ///For Tools menus
                        for (k = 0; k < reportsToolStripMenuItem.DropDownItems.Count; k++)
                        {
                            reportsToolStripMenuItem.DropDownItems[k].Visible = true;
                            reportsToolStripMenuItem.DropDownItems[k].Enabled = true;
                        }

                        ///For enable/disable toolstrip button
                        for (k = 0; k < toolStrip1.Items.Count; k++)
                        {
                            toolStrip1.Items[k].Visible = true;
                            toolStrip1.Items[k].Enabled = true;
                        }
                    }
                    toolRepeatScanning.Visible = false;
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (DBConnectionException dbex)
            {
                //MessageBox.Show(dbex.Message, "Image Heaven", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string err = dbex.Message;
                this.Close();
            }
            //catch (Exception ex)
            //{

            //}

		}

        // Used for Login
        void getData(ref NovaNet.Utils.Profile prmp)
        {
            int i;
            p = prmp;
            for (i = 1; i <= 2; i++)
            {
                if (rbc.authenticate(p.UserId, p.Password) == false)
                {
                    if (logincounter == 2)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        logincounter++;
                        GetChallenge ogc = new GetChallenge(getData);
                        ogc.ShowDialog(this);
                    }
                }
                else
                {
                    if (rbc.CheckUserIsLogged(p.UserId))
                    {

                        p = rbc.getProfile();
                        crd = rbc.getCredentials(p);
                        if (crd.role != ihConstants._ADMINISTRATOR_ROLE)
                        {
                            rbc.LockedUser(p.UserId, crd.created_dttm);
                        }
                        break;
                    }
                    else
                    {
                        p.UserId = null;
                        p.UserName = null;
                        GetChallenge ogc = new GetChallenge(getData);
                        ogc.ShowDialog(this);
                    }
                }
            }
        }

		void BatchToolStripMenuItemClick(object sender, EventArgs e)
		{
			frmAddEdit dispBatch;
			mainForm=new frmMain();
			wi = new wfeBatch(sqlCon);
			dispBatch = new aeBatch(wi,sqlCon);
			dispBatch.ShowDialog(mainForm);
		}

		void UploadCSVToolStripMenuItemClick(object sender, EventArgs e)
		{
			aeCSV csvUploader=new aeCSV(sqlCon,crd);
			mainForm=new frmMain();
			csvUploader.ShowDialog(mainForm);
		}
		
		
		void ConfigurationToolStripMenuItemClick(object sender, EventArgs e)
		{
			aeConfiguration csvUploader=new aeConfiguration();
			mainForm=new frmMain();
			csvUploader.ShowDialog(mainForm);
		}
		
		
		void QualityControlCentreToolStripMenuItemClick(object sender, EventArgs e)
		{
			eSTATES[] state=new eSTATES[1];
            state[0]=eSTATES.POLICY_SCANNED;
            
			aeBoxSelection box=new aeBoxSelection(state,sqlCon);
			//mainForm=new frmMain();
			wi=null;
			box.ShowDialog(this);
			wfeBox tmpBox = (wfeBox)wi;
            if (tmpBox != null)
            {
                if ((tmpBox.ctrlBox.ProjectCode.ToString() != null) && (tmpBox.ctrlBox.BatchKey.ToString() != null) && (tmpBox.ctrlBox.BoxNumber.ToString() != null))
                {
                    Form activeChild = this.ActiveMdiChild;
                    if (activeChild == null)
                    {
                        aeImageQC frmQc = new aeImageQC(tmpBox, sqlCon, crd);
                        frmQc.MdiParent = this;
                        frmQc.Height = this.ClientRectangle.Height;
                        frmQc.Width = this.ClientRectangle.Width;
                        frmQc.Show();
                    }
                }
            }
		}
		
		void IndexingToolStripMenuItemClick(object sender, EventArgs e)
		{
			eSTATES[] state=new eSTATES[2];
            state[0]=eSTATES.POLICY_QC;
            state[1] = eSTATES.POLICY_ON_HOLD; // should be removed after demo
            
			aeBoxSelection box=new aeBoxSelection(state,sqlCon);
			wfeBox tmpBox=null;
			wi=null;
			box.ShowDialog(this);
			tmpBox = (wfeBox)wi;
			if(tmpBox != null)
			{
				if((tmpBox.ctrlBox.ProjectCode.ToString()!=null) && (tmpBox.ctrlBox.BatchKey.ToString()!=null) && (tmpBox.ctrlBox.BoxNumber.ToString()!=null))
				{
					//wfeBox tmpBox = (wfeBox)wi;
					Form activeChild=this.ActiveMdiChild;
						if(activeChild==null)
						{
							aeIndexing frmIndex=new aeIndexing(tmpBox,sqlCon,crd);
							frmIndex.MdiParent=this;
							frmIndex.Height = this.ClientRectangle.Height;
							frmIndex.Width=this.ClientRectangle.Width;
							frmIndex.Show();
						}
				}
			}
		}
		
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			eSTATES[] state=new eSTATES[1];
            state[0]=eSTATES.POLICY_CREATED;
			aeBoxSelection box=new aeBoxSelection(state,sqlCon);
            box.chkPhotoScan.Visible = true;
			wfeBox tmpBox=null;
			wi=null;
			box.ShowDialog(this);
			tmpBox = (wfeBox)wi;
			if(tmpBox != null)
			{
				if((tmpBox.ctrlBox.ProjectCode.ToString()!=null) && (tmpBox.ctrlBox.BatchKey.ToString()!=null) && (tmpBox.ctrlBox.BoxNumber.ToString()!=null))
				{
					//wfeBox tmpBox = (wfeBox)wi;
					Form activeChild=this.ActiveMdiChild;
						if(activeChild==null)
						{
							aePolicyScan frmScan=new aePolicyScan(tmpBox,sqlCon,crd,colorMode);
							frmScan.MdiParent=this;
							frmScan.Height = this.ClientRectangle.Height;
							frmScan.Width=this.ClientRectangle.Width;
							frmScan.Show();
						}
				}
			}
		}
		
		void LICToolStripMenuItemClick(object sender, EventArgs e)
		{
			//Form activeChild=this.ActiveMdiChild;
            //if(activeChild==null)
            //{
				aeLicQa frmLicQA=new aeLicQa(sqlCon,crd);
				//frmLicQA.MdiParent=this;
				frmLicQA.Height = this.ClientRectangle.Height;
				frmLicQA.Width=this.ClientRectangle.Width;
				frmLicQA.ShowDialog(this);
			//}
		}
		
		void ExpertQualityControlCentreToolStripMenuItemClick(object sender, EventArgs e)
		{
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[7];
            state[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[1] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            state[2] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[3] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
            state[4] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
            state[5] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
            state[6] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
			aeBoxSelection box=new aeBoxSelection(state,sqlCon);
			wfeBox tmpBox=null;
			wi=null;
			box.ShowDialog(this);
			tmpBox = (wfeBox)wi;
			if(tmpBox != null)
			{
				if((tmpBox.ctrlBox.ProjectCode.ToString()!=null) && (tmpBox.ctrlBox.BatchKey.ToString()!=null) && (tmpBox.ctrlBox.BoxNumber.ToString()!=null))
				{
					//wfeBox tmpBox = (wfeBox)wi;
					Form activeChild=this.ActiveMdiChild;
						if(activeChild==null)
						{
							aeFQC frmFQC=new aeFQC(tmpBox,sqlCon,crd);
							//frmFQC.MdiParent=this;
							frmFQC.Height = this.ClientRectangle.Height;
							frmFQC.Width=this.ClientRectangle.Width;
							frmFQC.ShowDialog(this);
						}
				}
			}
		}
		
		void ExportToolStripMenuItemClick(object sender, EventArgs e)
		{
			aeExport export=new aeExport(sqlCon,crd);
			export.ShowDialog(this);
		}
		void reExportToolStripMenuItemClick(object sender, EventArgs e)
		{
			frmSingleExp exp = new frmSingleExp(sqlCon,crd);
			exp.ShowDialog(this);
		}
		void ToolStripButton1Click(object sender, EventArgs e)
		{
            eSTATES[] state = new eSTATES[1];
            state[0] = eSTATES.POLICY_CREATED;
            aeBoxSelection box = new aeBoxSelection(state, sqlCon);
            box.chkPhotoScan.Visible = true;
            wfeBox tmpBox = null;
            wi = null;
            box.ShowDialog(this);
            tmpBox = (wfeBox)wi;
			if(tmpBox != null)
			{
				if((tmpBox.ctrlBox.ProjectCode.ToString()!=null) && (tmpBox.ctrlBox.BatchKey.ToString()!=null) && (tmpBox.ctrlBox.BoxNumber.ToString()!=null))
				{
					//wfeBox tmpBox = (wfeBox)wi;
					Form activeChild=this.ActiveMdiChild;
						if(activeChild==null)
						{
							aePolicyScan frmScan=new aePolicyScan(tmpBox,sqlCon,crd,colorMode);
							frmScan.MdiParent=this;
							frmScan.Height = this.ClientRectangle.Height;
							frmScan.Width=this.ClientRectangle.Width;
							frmScan.Show();
						}
				}
			}
		}
		
		void ToolStripButton3Click(object sender, EventArgs e)
		{
			eSTATES[] state=new eSTATES[1];
            state[0]=eSTATES.POLICY_SCANNED;
            
			aeBoxSelection box=new aeBoxSelection(state,sqlCon);
			//mainForm=new frmMain();
			wi=null;
			box.ShowDialog(this);
			wfeBox tmpBox = (wfeBox)wi;
			if(tmpBox != null)
			{
				if((tmpBox.ctrlBox.ProjectCode.ToString()!=null) && (tmpBox.ctrlBox.BatchKey.ToString()!=null) && (tmpBox.ctrlBox.BoxNumber.ToString()!=null))
				{
					Form activeChild=this.ActiveMdiChild;
					if(activeChild==null)
					{
						aeImageQC frmQc=new aeImageQC(tmpBox,sqlCon,crd);
						frmQc.MdiParent=this;
						frmQc.Height = this.ClientRectangle.Height;
						frmQc.Width=this.ClientRectangle.Width;
						frmQc.Show();
					}
				}
			}
		}
		
		void ToolStripButton2Click(object sender, EventArgs e)
		{
			eSTATES[] state=new eSTATES[1];
            state[0]=eSTATES.POLICY_QC;
            
			aeBoxSelection box=new aeBoxSelection(state,sqlCon);
			wfeBox tmpBox=null;
			wi=null;
			box.ShowDialog(this);
			tmpBox = (wfeBox)wi;
			if(tmpBox != null)
			{
				if((tmpBox.ctrlBox.ProjectCode.ToString()!=null) && (tmpBox.ctrlBox.BatchKey.ToString()!=null) && (tmpBox.ctrlBox.BoxNumber.ToString()!=null))
				{
					//wfeBox tmpBox = (wfeBox)wi;
					Form activeChild=this.ActiveMdiChild;
						if(activeChild==null)
						{
							aeIndexing frmIndex=new aeIndexing(tmpBox,sqlCon,crd);
							frmIndex.MdiParent=this;
							frmIndex.Height = this.ClientRectangle.Height;
							frmIndex.Width=this.ClientRectangle.Width;
							frmIndex.Show();
						}
				}
			}
		}
		
		void ToolStripButton4Click(object sender, EventArgs e)
        {
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[7];
            state[0] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[1] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            state[2] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[3] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
            state[4] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
            state[5] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
            state[6] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
            aeBoxSelection box=new aeBoxSelection(state,sqlCon);
			wfeBox tmpBox=null;
			wi=null;
			box.ShowDialog(this);
			tmpBox = (wfeBox)wi;
			if(tmpBox != null)
			{
				if((tmpBox.ctrlBox.ProjectCode.ToString()!=null) && (tmpBox.ctrlBox.BatchKey.ToString()!=null) && (tmpBox.ctrlBox.BoxNumber.ToString()!=null))
				{
					//wfeBox tmpBox = (wfeBox)wi;
					Form activeChild=this.ActiveMdiChild;
						if(activeChild==null)
						{
							aeFQC frmFQC=new aeFQC(tmpBox,sqlCon,crd);
							//frmFQC.MdiParent=this;
							frmFQC.Height = this.ClientRectangle.Height;
							frmFQC.Width=this.ClientRectangle.Width;
							frmFQC.Visible=false;
							frmFQC.ShowDialog(this);
						}
				}
			}
		}

        private void mnuJobCreation_Click(object sender, EventArgs e)
        {
            aeJobCreation jobCrt = new aeJobCreation(sqlCon,crd);
            jobCrt.ShowDialog(this);		
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PwdChange pwdCh = new PwdChange(ref p, getCPwd);
            pwdCh.ShowDialog(this);
        }

        private void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewUser nwUsr = new AddNewUser(getnwusrData, sqlCon);
            nwUsr.ShowDialog(this);
        }
        // Used for password change
        void getCPwd(ref NovaNet.Utils.Profile prmpwd)
        {
            p = prmpwd;
            rbc.changePassword(p.UserId, p.UserName, p.Password);
        }
        // Used for add new user
        void getnwusrData(ref NovaNet.Utils.Profile prmp)
        {
            p = prmp;
            if (rbc.addUser(p.UserId, p.UserName, p.Role_des, p.Password) == false)
            {
                AddNewUser nwUsr = new AddNewUser(getnwusrData, sqlCon);
                nwUsr.ShowDialog(this);
            }
        }

        private void uATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSelection frmSel = new frmSelection(sqlCon, "UATAnxA");
            frmSel.ShowDialog(this);
        }

        private void batchSummeryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSelection frmSel = new frmSelection(sqlCon, "BatchSummary");
            frmSel.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About frmSel = new About();
            frmSel.ShowDialog(this);
        }

        private void boxSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmJobDistribution frmJob = new frmJobDistribution(sqlCon);
            frmJob.Show(this);
        }

        private void userManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("B'Zer.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error While Opening User Manual (PDF Format)\n" + ex.Message, "User Manual", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (p.UserId != string.Empty)
            {
                rbc.UnLockedUser(p.UserId);
                Application.Exit();
            }
        }

        private void toolOnlineUser_Click(object sender, EventArgs e)
        {
            frmLoggedUser loged = new frmLoggedUser(rbc,crd);
            loged.ShowDialog(this);
        }

        private void toolRepeatScanning_Click(object sender, EventArgs e)
        {
            frmRepeatScan rptScan = new frmRepeatScan(sqlCon, crd);
            rptScan.ShowDialog();
        }
	}
}