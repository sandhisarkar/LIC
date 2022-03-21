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
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[1];
            NovaNet.wfe.eSTATES[] imageState = new NovaNet.wfe.eSTATES[1];
            state[0] = NovaNet.wfe.eSTATES.POLICY_QC;
            imageState[0] = NovaNet.wfe.eSTATES.PAGE_QC;
            this.components = new System.ComponentModel.Container();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.BoxDtls = new wSelect.BoxDetails(wBox, sqlCon, state, imageState, crd);
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureControl = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.txtRegional = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPolicyNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCommDt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.type = new System.Windows.Forms.GroupBox();
            this.lvwDockTypes = new System.Windows.Forms.ListView();
            this.clmDockType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmShrtCut = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtDOB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
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
            this.groupBox2.SuspendLayout();
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
            this.BoxDtls.NextClicked += new wSelect.NextClickedHandler(this.BoxDtlsNextClicked);
            this.BoxDtls.PreviousClicked += new wSelect.PreviousClickedHandler(this.BoxDtls_PreviousClicked);
            this.BoxDtls.PolicyChanged += new wSelect.PolicyChangeHandler(this.BoxDtlsPolicyChanged);
            this.BoxDtls.BoxLoaded += new wSelect.BoxDetailsLoaded(this.BoxDtlsLoaded);
            this.BoxDtls.LstDelIamgeInsert += new wSelect.LstDelImageKeyPress(this.BoxDtlsLstDelIamgeInsert);
            this.BoxDtls.LstImageIndex += new wSelect.LstImageIndexKeyPress(this.BoxDtlsLstImageIndex);
            this.BoxDtls.BoxMouseClick += new wSelect.BoxDetailsMouseClick(this.BoxDtlsBoxMouseClick);
            this.BoxDtls.LstImgClick += new wSelect.LstImageClick(this.BoxDtls_LstImgClick);
            this.BoxDtls.LstDelImgClick += new wSelect.LstDelImageClick(this.BoxDtls_LstDelImgClick);
            this.BoxDtls.Load += new System.EventHandler(this.BoxDtlsLoaded);
            this.BoxDtls.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AeImageQCKeyPress);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureControl);
            this.panel1.ForeColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(259, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 707);
            this.panel1.TabIndex = 3;
            // 
            // pictureControl
            // 
            this.pictureControl.Location = new System.Drawing.Point(0, 0);
            this.pictureControl.Name = "pictureControl";
            this.pictureControl.Size = new System.Drawing.Size(512, 704);
            this.pictureControl.TabIndex = 0;
            this.pictureControl.TabStop = false;
            this.pictureControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.BoxDtlsBoxMouseClick);
            this.pictureControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseDown);
            this.pictureControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseMove);
            this.pictureControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseUp);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.txtCommDt);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.type);
            this.panel2.Controls.Add(this.txtDOB);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(781, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(249, 748);
            this.panel2.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblResult);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.txtPolicyNumber);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(2, 358);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(242, 196);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Record Details";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(10, 168);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(0, 13);
            this.lblResult.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmdUpdate);
            this.groupBox2.Controls.Add(this.txtRegional);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Location = new System.Drawing.Point(10, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 140);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Name : Language";
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.Location = new System.Drawing.Point(144, 110);
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.Size = new System.Drawing.Size(75, 23);
            this.cmdUpdate.TabIndex = 3;
            this.cmdUpdate.Text = "Update";
            this.cmdUpdate.UseVisualStyleBackColor = true;
            this.cmdUpdate.Click += new System.EventHandler(this.CmdUpdateClick);
            // 
            // txtRegional
            // 
            this.txtRegional.Font = new System.Drawing.Font("Arial Unicode MS", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRegional.Location = new System.Drawing.Point(6, 71);
            this.txtRegional.Name = "txtRegional";
            this.txtRegional.Size = new System.Drawing.Size(213, 33);
            this.txtRegional.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "English:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Regional:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(6, 32);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(213, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtPolicyNumber
            // 
            this.txtPolicyNumber.Enabled = false;
            this.txtPolicyNumber.Location = new System.Drawing.Point(60, 15);
            this.txtPolicyNumber.Name = "txtPolicyNumber";
            this.txtPolicyNumber.Size = new System.Drawing.Size(175, 20);
            this.txtPolicyNumber.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number:";
            // 
            // txtCommDt
            // 
            this.txtCommDt.Enabled = false;
            this.txtCommDt.Location = new System.Drawing.Point(71, 586);
            this.txtCommDt.Name = "txtCommDt";
            this.txtCommDt.Size = new System.Drawing.Size(128, 20);
            this.txtCommDt.TabIndex = 1;
            this.txtCommDt.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 589);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Comct. Dt:";
            this.label4.Visible = false;
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
            // txtDOB
            // 
            this.txtDOB.Enabled = false;
            this.txtDOB.Location = new System.Drawing.Point(71, 560);
            this.txtDOB.Name = "txtDOB";
            this.txtDOB.Size = new System.Drawing.Size(172, 20);
            this.txtDOB.TabIndex = 1;
            this.txtDOB.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 560);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "DOB:";
            this.label3.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 726);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(781, 22);
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
            this.Name = "aeIndexing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "aeIndexing";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AeIndexingFormClosing);
            this.Load += new System.EventHandler(this.AeIndexingLoad);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.aeIndexing_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AeImageQCKeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AeImageQCKeyUp);
            this.Resize += new System.EventHandler(this.aeIndexing_Resize);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureControl)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel2;
		
		
		
		
		private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureControl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtRegional;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Button cmdUpdate;
	}
}
