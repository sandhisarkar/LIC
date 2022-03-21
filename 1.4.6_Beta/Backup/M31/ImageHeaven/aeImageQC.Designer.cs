/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 9/3/2009
 * Time: 7:35 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class aeImageQC
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
			NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
            NovaNet.wfe.eSTATES[] imageState = new NovaNet.wfe.eSTATES[1];
            state[0] = NovaNet.wfe.eSTATES.POLICY_SCANNED;
            imageState[0] = NovaNet.wfe.eSTATES.PAGE_SCANNED;
			this.components = new System.ComponentModel.Container();
			this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tx = new System.Windows.Forms.ToolStripStatusLabel();
			this.ty = new System.Windows.Forms.ToolStripStatusLabel();
			this.tw = new System.Windows.Forms.ToolStripStatusLabel();
			this.th = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssProject = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssBatch = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssBox = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssPolicy = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssProjectVal = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssBatchVal = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolExcp = new System.Windows.Forms.ToolStripStatusLabel();
			this.tssExcpDetails = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.BoxDtls = new wSelect.BoxDetails(wBox, sqlCon, state, imageState,crd);
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureControl = new System.Windows.Forms.PictureBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.markExceptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lblNote = new System.Windows.Forms.Label();
			this.statusStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureControl)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dockPanel
			// 
			this.dockPanel.ActiveAutoHideContent = null;
			this.dockPanel.BackColor = System.Drawing.Color.Gainsboro;
			this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel.DockBottomPortion = 150;
			this.dockPanel.DockLeftPortion = 200;
			this.dockPanel.DockRightPortion = 200;
			this.dockPanel.DockTopPortion = 150;
			this.dockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
			this.dockPanel.Location = new System.Drawing.Point(0, 0);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.RightToLeftLayout = true;
			this.dockPanel.Size = new System.Drawing.Size(989, 726);
			this.dockPanel.TabIndex = 5;
			this.dockPanel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.DockPanelPreviewKeyDown);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.tx,
									this.ty,
									this.tw,
									this.th,
									this.tssProject,
									this.tssBatch,
									this.tssBox,
									this.tssPolicy,
									this.tssProjectVal,
									this.tssBatchVal,
									this.toolExcp,
									this.tssExcpDetails,
									this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 726);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(989, 22);
			this.statusStrip1.TabIndex = 13;
			this.statusStrip1.Text = "statusStrip1";
			this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.StatusStrip1ItemClicked);
			// 
			// tx
			// 
			this.tx.Name = "tx";
			this.tx.Size = new System.Drawing.Size(0, 17);
			// 
			// ty
			// 
			this.ty.Name = "ty";
			this.ty.Size = new System.Drawing.Size(0, 17);
			// 
			// tw
			// 
			this.tw.Name = "tw";
			this.tw.Size = new System.Drawing.Size(0, 17);
			// 
			// th
			// 
			this.th.Name = "th";
			this.th.Size = new System.Drawing.Size(0, 17);
			// 
			// tssProject
			// 
			this.tssProject.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tssProject.Name = "tssProject";
			this.tssProject.Size = new System.Drawing.Size(0, 17);
			// 
			// tssBatch
			// 
			this.tssBatch.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tssBatch.Name = "tssBatch";
			this.tssBatch.Size = new System.Drawing.Size(0, 17);
			// 
			// tssBox
			// 
			this.tssBox.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tssBox.Name = "tssBox";
			this.tssBox.Size = new System.Drawing.Size(0, 17);
			// 
			// tssPolicy
			// 
			this.tssPolicy.Name = "tssPolicy";
			this.tssPolicy.Size = new System.Drawing.Size(0, 17);
			// 
			// tssProjectVal
			// 
			this.tssProjectVal.Name = "tssProjectVal";
			this.tssProjectVal.Size = new System.Drawing.Size(0, 17);
			this.tssProjectVal.Visible = false;
			// 
			// tssBatchVal
			// 
			this.tssBatchVal.Name = "tssBatchVal";
			this.tssBatchVal.Size = new System.Drawing.Size(0, 17);
			this.tssBatchVal.Visible = false;
			// 
			// toolExcp
			// 
			this.toolExcp.Name = "toolExcp";
			this.toolExcp.Size = new System.Drawing.Size(0, 17);
			// 
			// tssExcpDetails
			// 
			this.tssExcpDetails.Name = "tssExcpDetails";
			this.tssExcpDetails.Size = new System.Drawing.Size(0, 17);
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// BoxDtls
			// 
			this.BoxDtls.Location = new System.Drawing.Point(0, 0);
			this.BoxDtls.Name = "BoxDtls";
			this.BoxDtls.Size = new System.Drawing.Size(252, 439);
			this.BoxDtls.TabIndex = 0;
			this.BoxDtls.LstImageIndex += new wSelect.LstImageIndexKeyPress(this.BoxDtlsLstImageIndex);
			this.BoxDtls.BoxLoaded += new wSelect.BoxDetailsLoaded(this.BoxDtlsLoaded);
			this.BoxDtls.LstDelIamgeInsert += new wSelect.LstDelImageKeyPress(this.BoxDtlsLstDelIamgeInsert);
			this.BoxDtls.LstNextKey += new wSelect.LstNextKeyPress(this.BoxDtls_LstNextKey);
			this.BoxDtls.NextClicked += new wSelect.NextClickedHandler(this.BoxDtlsNextClicked);
			this.BoxDtls.LstDelImgClick += new wSelect.LstDelImageClick(this.BoxDtls_LstDelImgClick);
			this.BoxDtls.BoxMouseClick += new wSelect.BoxDetailsMouseClick(this.BoxDtlsBoxMouseClick);
			this.BoxDtls.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BoxDtls_LstNextKey);
			this.BoxDtls.ImageChanged += new wSelect.ImageChangeHandler(this.BoxDtlsImageChanged);
			this.BoxDtls.PolicyChanged += new wSelect.PolicyChangeHandler(this.BoxDtlsPolicyChanged);
            this.BoxDtls.PreviousClicked +=new wSelect.PreviousClickedHandler(BoxDtls_PreviousClicked);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.pictureControl);
			this.panel1.Location = new System.Drawing.Point(355, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(634, 726);
			this.panel1.TabIndex = 20;
			// 
			// pictureControl
			// 
			this.pictureControl.Location = new System.Drawing.Point(3, 0);
			this.pictureControl.Name = "pictureControl";
			this.pictureControl.Size = new System.Drawing.Size(625, 726);
			this.pictureControl.TabIndex = 20;
			this.pictureControl.TabStop = false;
			this.pictureControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseMove);
			this.pictureControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseDown);
			this.pictureControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseUp);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.markExceptionToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(159, 26);
			// 
			// markExceptionToolStripMenuItem
			// 
			this.markExceptionToolStripMenuItem.Name = "markExceptionToolStripMenuItem";
			this.markExceptionToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
			this.markExceptionToolStripMenuItem.Text = "Mark Exception";
			this.markExceptionToolStripMenuItem.Click += new System.EventHandler(this.MarkExceptionToolStripMenuItemClick);
			// 
			// lblNote
			// 
			this.lblNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNote.ForeColor = System.Drawing.Color.Red;
			this.lblNote.Location = new System.Drawing.Point(12, 404);
			this.lblNote.Name = "lblNote";
			this.lblNote.Size = new System.Drawing.Size(219, 26);
			this.lblNote.TabIndex = 24;
			this.lblNote.Text = "Warning: This policy contains photo, crop it with photo crop button.";
			// 
			// aeImageQC
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(989, 748);
			this.Controls.Add(this.lblNote);
			this.Controls.Add(this.BoxDtls);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.dockPanel);
			this.Controls.Add(this.statusStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "aeImageQC";
			this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
			this.Text = "Form2";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.Load += new System.EventHandler(this.Form2Load);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AeImageQCKeyPress);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AeImageQCKeyUp);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form2FormClosing);
			this.Resize += new System.EventHandler(this.aeImageQC_Resize);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.aeImageQC_KeyDown);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureControl)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Label lblNote;
		private System.Windows.Forms.ToolStripStatusLabel toolExcp;
		public System.Windows.Forms.ToolStripStatusLabel tssExcpDetails;
		private System.Windows.Forms.ToolStripMenuItem markExceptionToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Panel panel1;
		private wSelect.BoxDetails BoxDtls;
		
		
		public System.Windows.Forms.ToolStripStatusLabel tssBatchVal;
		public System.Windows.Forms.ToolStripStatusLabel tssProjectVal;
		public System.Windows.Forms.ToolStripStatusLabel tssPolicy;
		public System.Windows.Forms.ToolStripStatusLabel tssBox;
		public System.Windows.Forms.ToolStripStatusLabel tssBatch;
		public System.Windows.Forms.ToolStripStatusLabel tssProject;
		private System.Windows.Forms.ToolStripStatusLabel th;
		private System.Windows.Forms.ToolStripStatusLabel tw;
		private System.Windows.Forms.ToolStripStatusLabel ty;
		private System.Windows.Forms.ToolStripStatusLabel tx;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.PictureBox pictureControl;
		//private System.Windows.Forms.PictureBox picControl = new PictureBox();
		
		
		
		
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
	}
}
