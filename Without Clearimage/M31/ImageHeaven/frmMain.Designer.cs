/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 18/2/2009
 * Time: 11:43 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class frmMain
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.itemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transactinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uploadCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuJobCreation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageQCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expertQualityControlCentreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reexportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.lICToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changePasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newUserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolOnlineUser = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boxSummaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchSummeryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userManualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.sts = new System.Windows.Forms.StatusStrip();
            this.stsName = new System.Windows.Forms.ToolStripStatusLabel();
            this.stsRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.sts.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itemsToolStripMenuItem,
            this.transactinToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolsToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(579, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // itemsToolStripMenuItem
            // 
            this.itemsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem,
            this.batchToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.itemsToolStripMenuItem.Enabled = false;
            this.itemsToolStripMenuItem.Name = "itemsToolStripMenuItem";
            this.itemsToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.itemsToolStripMenuItem.Text = "New";
            this.itemsToolStripMenuItem.Visible = false;
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.Enabled = false;
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.projectToolStripMenuItem.Text = "Project";
            this.projectToolStripMenuItem.Visible = false;
            this.projectToolStripMenuItem.Click += new System.EventHandler(this.ProjectToolStripMenuItemClick);
            // 
            // batchToolStripMenuItem
            // 
            this.batchToolStripMenuItem.Enabled = false;
            this.batchToolStripMenuItem.Name = "batchToolStripMenuItem";
            this.batchToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.batchToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.batchToolStripMenuItem.Text = "Batch";
            this.batchToolStripMenuItem.Visible = false;
            this.batchToolStripMenuItem.Click += new System.EventHandler(this.BatchToolStripMenuItemClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // transactinToolStripMenuItem
            // 
            this.transactinToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uploadCSVToolStripMenuItem,
            this.projectToolStripMenuItem1,
            this.mnuJobCreation,
            this.toolStripSeparator1,
            this.toolStripMenuItem1,
            this.imageQCToolStripMenuItem,
            this.indexingToolStripMenuItem,
            this.expertQualityControlCentreToolStripMenuItem,
            this.toolStripSeparator2,
            this.exportToolStripMenuItem,
            this.reexportToolStripMenuItem});
            this.transactinToolStripMenuItem.Enabled = false;
            this.transactinToolStripMenuItem.Name = "transactinToolStripMenuItem";
            this.transactinToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.transactinToolStripMenuItem.Text = "Transactions";
            this.transactinToolStripMenuItem.Visible = false;
            // 
            // uploadCSVToolStripMenuItem
            // 
            this.uploadCSVToolStripMenuItem.Enabled = false;
            this.uploadCSVToolStripMenuItem.Name = "uploadCSVToolStripMenuItem";
            this.uploadCSVToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.uploadCSVToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.uploadCSVToolStripMenuItem.Text = "Upload CSV";
            this.uploadCSVToolStripMenuItem.Visible = false;
            this.uploadCSVToolStripMenuItem.Click += new System.EventHandler(this.UploadCSVToolStripMenuItemClick);
            // 
            // projectToolStripMenuItem1
            // 
            this.projectToolStripMenuItem1.Enabled = false;
            this.projectToolStripMenuItem1.Name = "projectToolStripMenuItem1";
            this.projectToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.projectToolStripMenuItem1.Size = new System.Drawing.Size(233, 22);
            this.projectToolStripMenuItem1.Text = "Inventory In";
            this.projectToolStripMenuItem1.Visible = false;
            this.projectToolStripMenuItem1.Click += new System.EventHandler(this.ProjectToolStripMenuItem1Click);
            // 
            // mnuJobCreation
            // 
            this.mnuJobCreation.Enabled = false;
            this.mnuJobCreation.Name = "mnuJobCreation";
            this.mnuJobCreation.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.mnuJobCreation.Size = new System.Drawing.Size(233, 22);
            this.mnuJobCreation.Text = "Job Creation";
            this.mnuJobCreation.Visible = false;
            this.mnuJobCreation.Click += new System.EventHandler(this.mnuJobCreation_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(230, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItem1.Size = new System.Drawing.Size(233, 22);
            this.toolStripMenuItem1.Text = "Batch Scan";
            this.toolStripMenuItem1.Visible = false;
            this.toolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1Click);
            // 
            // imageQCToolStripMenuItem
            // 
            this.imageQCToolStripMenuItem.Enabled = false;
            this.imageQCToolStripMenuItem.Name = "imageQCToolStripMenuItem";
            this.imageQCToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.imageQCToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.imageQCToolStripMenuItem.Text = "Image Quality Control ";
            this.imageQCToolStripMenuItem.Visible = false;
            this.imageQCToolStripMenuItem.Click += new System.EventHandler(this.QualityControlCentreToolStripMenuItemClick);
            // 
            // indexingToolStripMenuItem
            // 
            this.indexingToolStripMenuItem.Enabled = false;
            this.indexingToolStripMenuItem.Name = "indexingToolStripMenuItem";
            this.indexingToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.indexingToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.indexingToolStripMenuItem.Text = "Indexing";
            this.indexingToolStripMenuItem.Visible = false;
            this.indexingToolStripMenuItem.Click += new System.EventHandler(this.IndexingToolStripMenuItemClick);
            // 
            // expertQualityControlCentreToolStripMenuItem
            // 
            this.expertQualityControlCentreToolStripMenuItem.Enabled = false;
            this.expertQualityControlCentreToolStripMenuItem.Name = "expertQualityControlCentreToolStripMenuItem";
            this.expertQualityControlCentreToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.expertQualityControlCentreToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.expertQualityControlCentreToolStripMenuItem.Text = "Quality Control(Final)";
            this.expertQualityControlCentreToolStripMenuItem.Visible = false;
            this.expertQualityControlCentreToolStripMenuItem.Click += new System.EventHandler(this.ExpertQualityControlCentreToolStripMenuItemClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(230, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Enabled = false;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Visible = false;
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItemClick);
            // 
            // reexportToolStripMenuItem
            // 
            this.reexportToolStripMenuItem.Enabled = false;
            this.reexportToolStripMenuItem.Name = "reexportToolStripMenuItem";
            this.reexportToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.reexportToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.reexportToolStripMenuItem.Text = "ReExport";
            this.reexportToolStripMenuItem.Visible = false;
            this.reexportToolStripMenuItem.Click += new System.EventHandler(this.reExportToolStripMenuItemClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lICToolStripMenuItem});
            this.toolStripMenuItem2.Enabled = false;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.toolStripMenuItem2.Size = new System.Drawing.Size(35, 20);
            this.toolStripMenuItem2.Text = "LIC";
            this.toolStripMenuItem2.Visible = false;
            // 
            // lICToolStripMenuItem
            // 
            this.lICToolStripMenuItem.Enabled = false;
            this.lICToolStripMenuItem.Name = "lICToolStripMenuItem";
            this.lICToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.lICToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.lICToolStripMenuItem.Text = "LIC Audit";
            this.lICToolStripMenuItem.Visible = false;
            this.lICToolStripMenuItem.Click += new System.EventHandler(this.LICToolStripMenuItemClick);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.changePasswordToolStripMenuItem,
            this.newUserToolStripMenuItem,
            this.toolOnlineUser});
            this.toolsToolStripMenuItem.Enabled = false;
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            this.toolsToolStripMenuItem.Visible = false;
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Enabled = false;
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.configurationToolStripMenuItem.Text = "Configuration";
            this.configurationToolStripMenuItem.Visible = false;
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.ConfigurationToolStripMenuItemClick);
            // 
            // changePasswordToolStripMenuItem
            // 
            this.changePasswordToolStripMenuItem.Enabled = false;
            this.changePasswordToolStripMenuItem.Name = "changePasswordToolStripMenuItem";
            this.changePasswordToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.changePasswordToolStripMenuItem.Text = "Change Password";
            this.changePasswordToolStripMenuItem.Visible = false;
            this.changePasswordToolStripMenuItem.Click += new System.EventHandler(this.changePasswordToolStripMenuItem_Click);
            // 
            // newUserToolStripMenuItem
            // 
            this.newUserToolStripMenuItem.Enabled = false;
            this.newUserToolStripMenuItem.Name = "newUserToolStripMenuItem";
            this.newUserToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.newUserToolStripMenuItem.Text = "New User";
            this.newUserToolStripMenuItem.Visible = false;
            this.newUserToolStripMenuItem.Click += new System.EventHandler(this.newUserToolStripMenuItem_Click);
            // 
            // toolOnlineUser
            // 
            this.toolOnlineUser.Enabled = false;
            this.toolOnlineUser.Name = "toolOnlineUser";
            this.toolOnlineUser.Size = new System.Drawing.Size(171, 22);
            this.toolOnlineUser.Text = "Online Users";
            this.toolOnlineUser.Click += new System.EventHandler(this.toolOnlineUser_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boxSummaryToolStripMenuItem,
            this.batchSummeryToolStripMenuItem});
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.reportsToolStripMenuItem.Text = "Reports";
            this.reportsToolStripMenuItem.Visible = false;
            // 
            // boxSummaryToolStripMenuItem
            // 
            this.boxSummaryToolStripMenuItem.Enabled = false;
            this.boxSummaryToolStripMenuItem.Name = "boxSummaryToolStripMenuItem";
            this.boxSummaryToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.boxSummaryToolStripMenuItem.Text = "Box Summary";
            this.boxSummaryToolStripMenuItem.Click += new System.EventHandler(this.boxSummaryToolStripMenuItem_Click);
            // 
            // batchSummeryToolStripMenuItem
            // 
            this.batchSummeryToolStripMenuItem.Enabled = false;
            this.batchSummeryToolStripMenuItem.Name = "batchSummeryToolStripMenuItem";
            this.batchSummeryToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.batchSummeryToolStripMenuItem.Text = "Batch Summary";
            this.batchSummeryToolStripMenuItem.Click += new System.EventHandler(this.batchSummeryToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.userManualToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Visible = false;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // userManualToolStripMenuItem
            // 
            this.userManualToolStripMenuItem.Name = "userManualToolStripMenuItem";
            this.userManualToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.userManualToolStripMenuItem.Text = "User Manual";
            this.userManualToolStripMenuItem.Click += new System.EventHandler(this.userManualToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImeMode = System.Windows.Forms.ImeMode.HangulFull;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3,
            this.toolStripButton1,
            this.toolStripLabel1,
            this.toolStripButton3,
            this.toolStripButton2,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(579, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "Batch Scanning";
            this.toolStripButton1.Visible = false;
            this.toolStripButton1.Click += new System.EventHandler(this.ToolStripButton1Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(0, 22);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Quality Control";
            this.toolStripButton3.Visible = false;
            this.toolStripButton3.Click += new System.EventHandler(this.ToolStripButton3Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Indexing ";
            this.toolStripButton2.Visible = false;
            this.toolStripButton2.Click += new System.EventHandler(this.ToolStripButton2Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Final Quality Control";
            this.toolStripButton4.Visible = false;
            this.toolStripButton4.Click += new System.EventHandler(this.ToolStripButton4Click);
            // 
            // sts
            // 
            this.sts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsName,
            this.stsRole});
            this.sts.Location = new System.Drawing.Point(0, 428);
            this.sts.Name = "sts";
            this.sts.Size = new System.Drawing.Size(579, 22);
            this.sts.TabIndex = 5;
            this.sts.Text = "statusStrip1";
            this.sts.Visible = false;
            // 
            // stsName
            // 
            this.stsName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stsName.ForeColor = System.Drawing.Color.Blue;
            this.stsName.Name = "stsName";
            this.stsName.Size = new System.Drawing.Size(0, 17);
            // 
            // stsRole
            // 
            this.stsRole.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stsRole.ForeColor = System.Drawing.Color.Blue;
            this.stsRole.Name = "stsRole";
            this.stsRole.Size = new System.Drawing.Size(0, 17);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::ImageHeaven.Properties.Resources.final_screen;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(579, 450);
            this.Controls.Add(this.sts);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "B\'Zer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMainLoad);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.sts.ResumeLayout(false);
            this.sts.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripButton toolStripButton3;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem reexportToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem expertQualityControlCentreToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lICToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem indexingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem transactinToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem imageQCToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem uploadCSVToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem batchToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem itemsToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuJobCreation;
        private System.Windows.Forms.ToolStripMenuItem changePasswordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newUserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchSummeryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem boxSummaryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userManualToolStripMenuItem;
        private System.Windows.Forms.StatusStrip sts;
        private System.Windows.Forms.ToolStripStatusLabel stsName;
        public System.Windows.Forms.ToolStripStatusLabel stsRole;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolOnlineUser;
	}
}
