/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 27/3/2009
 * Time: 11:27 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System.Drawing;
namespace ImageHeaven
{
	partial class aeIndexing
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
            //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
            NovaNet.wfe.eSTATES[] imageState = new NovaNet.wfe.eSTATES[1];
            state[0] = NovaNet.wfe.eSTATES.POLICY_QC;
            imageState[0] = NovaNet.wfe.eSTATES.PAGE_QC;
            
			this.components = new System.ComponentModel.Container();
			this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.BoxDtls = new wSelect.BoxDetails(wBox, sqlCon, state, imageState,crd);
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureControl = new System.Windows.Forms.PictureBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cmdUpdate = new System.Windows.Forms.Button();
			this.txtCommDt = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.txtDOB = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPolicyNumber = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.type = new System.Windows.Forms.GroupBox();
			this.lvwDockTypes = new System.Windows.Forms.ListView();
			this.clmDockType = new System.Windows.Forms.ColumnHeader();
			this.clmShrtCut = new System.Windows.Forms.ColumnHeader();
			this.clmCount = new System.Windows.Forms.ColumnHeader();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.markExceptionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markNotReadyHoldPolicyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markReadyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureControl)).BeginInit();
			this.panel2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.type.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();

			// 
			// dockPanel
			// 
			this.dockPanel.ActiveAutoHideContent = null;
			this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel.Location = new System.Drawing.Point(0, 0);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.Size = new System.Drawing.Size(1030, 748);
			this.dockPanel.TabIndex = 0;
			// 
			// BoxDtls
			// 
			this.BoxDtls.Location = new System.Drawing.Point(0, 0);
			this.BoxDtls.Name = "BoxDtls";
			this.BoxDtls.Size = new System.Drawing.Size(253, 450);
			this.BoxDtls.TabIndex = 2;
			this.BoxDtls.LstImageIndex += new wSelect.LstImageIndexKeyPress(this.BoxDtlsLstImageIndex);
			this.BoxDtls.Load += new System.EventHandler(this.BoxDtlsLoaded);
			this.BoxDtls.BoxLoaded += new wSelect.BoxDetailsLoaded(this.BoxDtlsLoaded);
			this.BoxDtls.LstDelIamgeInsert += new wSelect.LstDelImageKeyPress(this.BoxDtlsLstDelIamgeInsert);
			this.BoxDtls.NextClicked += new wSelect.NextClickedHandler(this.BoxDtlsNextClicked);
			this.BoxDtls.PreviousClicked += new wSelect.PreviousClickedHandler(this.BoxDtls_PreviousClicked);
			this.BoxDtls.LstDelImgClick += new wSelect.LstDelImageClick(this.BoxDtls_LstDelImgClick);
			this.BoxDtls.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AeImageQCKeyPress);
			this.BoxDtls.BoxMouseClick += new wSelect.BoxDetailsMouseClick(this.BoxDtlsBoxMouseClick);
			this.BoxDtls.LstImgClick += new wSelect.LstImageClick(this.BoxDtls_LstImgClick);
			this.BoxDtls.PolicyChanged += new wSelect.PolicyChangeHandler(this.BoxDtlsPolicyChanged);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.pictureControl);
			this.panel1.Location = new System.Drawing.Point(259, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(513, 707);
			this.panel1.TabIndex = 3;
            this.panel1.ForeColor = Color.Black;
			// 
			// pictureControl
			// 
			this.pictureControl.Location = new System.Drawing.Point(0, 0);
			this.pictureControl.Name = "pictureControl";
			this.pictureControl.Size = new System.Drawing.Size(512, 704);
			this.pictureControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
			this.pictureControl.TabIndex = 0;
			this.pictureControl.TabStop = false;
			this.pictureControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseMove);
			this.pictureControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BoxDtlsBoxMouseClick);
			this.pictureControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseDown);
			this.pictureControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseUp);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.groupBox1);
			this.panel2.Controls.Add(this.type);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel2.Location = new System.Drawing.Point(778, 0);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(252, 748);
			this.panel2.TabIndex = 4;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cmdUpdate);
			this.groupBox1.Controls.Add(this.txtCommDt);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.txtDOB);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtName);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.txtPolicyNumber);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(2, 358);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(242, 170);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Policy Details";
			// 
			// cmdUpdate
			// 
			this.cmdUpdate.Location = new System.Drawing.Point(156, 131);
			this.cmdUpdate.Name = "cmdUpdate";
			this.cmdUpdate.Size = new System.Drawing.Size(75, 23);
			this.cmdUpdate.TabIndex = 2;
			this.cmdUpdate.Text = "Update";
			this.cmdUpdate.UseVisualStyleBackColor = true;
			this.cmdUpdate.Click += new System.EventHandler(this.CmdUpdateClick);
			// 
			// txtCommDt
			// 
			this.txtCommDt.Enabled = false;
			this.txtCommDt.Location = new System.Drawing.Point(59, 105);
			this.txtCommDt.Name = "txtCommDt";
			this.txtCommDt.Size = new System.Drawing.Size(128, 20);
			this.txtCommDt.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 108);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(57, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Comct. Dt:";
			// 
			// txtDOB
			// 
			this.txtDOB.Enabled = false;
			this.txtDOB.Location = new System.Drawing.Point(59, 79);
			this.txtDOB.Name = "txtDOB";
			this.txtDOB.Size = new System.Drawing.Size(172, 20);
			this.txtDOB.TabIndex = 1;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 79);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(33, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "DOB:";
			// 
			// txtName
			// 
			this.txtName.Enabled = false;
			this.txtName.Location = new System.Drawing.Point(59, 53);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(172, 20);
			this.txtName.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 56);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Name:";
			// 
			// txtPolicyNumber
			// 
			this.txtPolicyNumber.Enabled = false;
			this.txtPolicyNumber.Location = new System.Drawing.Point(59, 27);
			this.txtPolicyNumber.Name = "txtPolicyNumber";
			this.txtPolicyNumber.Size = new System.Drawing.Size(172, 20);
			this.txtPolicyNumber.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 30);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Number:";
			// 
			// type
			// 
			this.type.Controls.Add(this.lvwDockTypes);
			this.type.Location = new System.Drawing.Point(3, 3);
			this.type.Name = "type";
			this.type.Size = new System.Drawing.Size(241, 349);
			this.type.TabIndex = 0;
			this.type.TabStop = false;
			this.type.Text = "DocTypes";
			// 
			// lvwDockTypes
			// 
			this.lvwDockTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.clmDockType,
									this.clmShrtCut,
									this.clmCount});
			this.lvwDockTypes.GridLines = true;
			this.lvwDockTypes.Location = new System.Drawing.Point(6, 19);
			this.lvwDockTypes.Name = "lvwDockTypes";
			this.lvwDockTypes.Size = new System.Drawing.Size(228, 324);
			this.lvwDockTypes.TabIndex = 0;
			this.lvwDockTypes.UseCompatibleStateImageBehavior = false;
			this.lvwDockTypes.View = System.Windows.Forms.View.Details;
			// 
			// clmDockType
			// 
			this.clmDockType.Text = "DockTypes";
			this.clmDockType.Width = 150;
			// 
			// clmShrtCut
			// 
			this.clmShrtCut.Text = "Keys";
			this.clmShrtCut.Width = 40;
			// 
			// clmCount
			// 
			this.clmCount.Text = "Count";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStripStatusLabel1,
									this.toolStripStatusLabel2});
			this.statusStrip1.Location = new System.Drawing.Point(0, 726);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(778, 22);
			this.statusStrip1.TabIndex = 6;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.markExceptionToolStripMenuItem,
                                    this.markNotReadyHoldPolicyToolStripMenuItem,
									this.markReadyToolStripMenuItem});
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
            // markNotReadyHoldPolicyToolStripMenuItem
            // 
            this.markNotReadyHoldPolicyToolStripMenuItem.Name = "markNotReadyHoldPolicyToolStripMenuItem";
            this.markNotReadyHoldPolicyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.markNotReadyHoldPolicyToolStripMenuItem.Text = "Mark Not Ready (Hold Policy)";
            this.markNotReadyHoldPolicyToolStripMenuItem.Click += new System.EventHandler(this.HoldPolicyToolStripMenuItemClick);
            // 
            // markReadyToolStripMenuItem
            // 
            this.markReadyToolStripMenuItem.Name = "markReadyToolStripMenuItem";
            this.markReadyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.markReadyToolStripMenuItem.Text = "Mark Ready";
            this.markReadyToolStripMenuItem.Click += new System.EventHandler(this.markReadyToolStripMenuItemClick);

			// 
			// aeIndexing
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1030, 748);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.BoxDtls);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.dockPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "aeIndexing";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "aeIndexing";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.Load += new System.EventHandler(this.AeIndexingLoad);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AeImageQCKeyPress);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AeImageQCKeyUp);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AeIndexingFormClosing);
			this.Resize += new System.EventHandler(this.aeIndexing_Resize);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.aeIndexing_KeyDown);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureControl)).EndInit();
			this.panel2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.type.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
        private System.Windows.Forms.ToolStripMenuItem markReadyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem markNotReadyHoldPolicyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem markExceptionToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ColumnHeader clmShrtCut;
		private System.Windows.Forms.ListView lvwDockTypes;
        private wSelect.BoxDetails BoxDtls;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		private System.Windows.Forms.ColumnHeader clmCount;
		private System.Windows.Forms.ColumnHeader clmDockType;
		private System.Windows.Forms.GroupBox type;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPolicyNumber;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtName;
		
		
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDOB;
		private System.Windows.Forms.Label label4;
		
		void AeIndexingKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
		}
		private System.Windows.Forms.TextBox txtCommDt;
		private System.Windows.Forms.Button cmdUpdate;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel2;
		
		
		
		
		private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureControl;
	}
}
