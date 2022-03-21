/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 13/4/2008
 * Time: 6:21 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class aeExport
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbBatch = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbProject = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tblExp = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lvwExportList = new System.Windows.Forms.ListView();
            this.Boxes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cmdSave = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.cmdExport = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmdValidate = new System.Windows.Forms.Button();
            this.chkReExport = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tblExp.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbBatch);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbProject);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 70);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // cmbBatch
            // 
            this.cmbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBatch.FormattingEnabled = true;
            this.cmbBatch.Location = new System.Drawing.Point(69, 40);
            this.cmbBatch.Name = "cmbBatch";
            this.cmbBatch.Size = new System.Drawing.Size(175, 21);
            this.cmbBatch.TabIndex = 3;
            this.cmbBatch.SelectedIndexChanged += new System.EventHandler(this.CmbBatchSelectedIndexChanged);
            this.cmbBatch.Leave += new System.EventHandler(this.CmbBatchLeave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Batch:";
            // 
            // cmbProject
            // 
            this.cmbProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProject.FormattingEnabled = true;
            this.cmbProject.Location = new System.Drawing.Point(69, 13);
            this.cmbProject.Name = "cmbProject";
            this.cmbProject.Size = new System.Drawing.Size(175, 21);
            this.cmbProject.TabIndex = 1;
            this.cmbProject.Leave += new System.EventHandler(this.CmbProjectLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tblExp);
            this.groupBox2.Location = new System.Drawing.Point(12, 107);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(322, 320);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // tblExp
            // 
            this.tblExp.Controls.Add(this.tabPage1);
            this.tblExp.Controls.Add(this.tabPage2);
            this.tblExp.Location = new System.Drawing.Point(7, 11);
            this.tblExp.Name = "tblExp";
            this.tblExp.SelectedIndex = 0;
            this.tblExp.Size = new System.Drawing.Size(309, 303);
            this.tblExp.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lvwExportList);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(301, 277);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Select Box";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lvwExportList
            // 
            this.lvwExportList.CheckBoxes = true;
            this.lvwExportList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Boxes,
            this.colDock,
            this.columnHeader1});
            this.lvwExportList.FullRowSelect = true;
            this.lvwExportList.GridLines = true;
            this.lvwExportList.Location = new System.Drawing.Point(8, 15);
            this.lvwExportList.Name = "lvwExportList";
            this.lvwExportList.Size = new System.Drawing.Size(287, 256);
            this.lvwExportList.TabIndex = 7;
            this.lvwExportList.UseCompatibleStateImageBehavior = false;
            this.lvwExportList.View = System.Windows.Forms.View.Details;
            // 
            // Boxes
            // 
            this.Boxes.Text = "Boxes";
            // 
            // colDock
            // 
            this.colDock.Text = "Docket Count";
            this.colDock.Width = 100;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Hold/Missing Count";
            this.columnHeader1.Width = 120;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.cmdSave);
            this.tabPage2.Controls.Add(this.txtMsg);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(301, 277);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Result";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(158, 248);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 2;
            this.cmdSave.Text = "Save List";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.BackColor = System.Drawing.Color.Gainsboro;
            this.txtMsg.Location = new System.Drawing.Point(6, 6);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(227, 236);
            this.txtMsg.TabIndex = 1;
            // 
            // cmdExport
            // 
            this.cmdExport.Location = new System.Drawing.Point(178, 433);
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(75, 23);
            this.cmdExport.TabIndex = 2;
            this.cmdExport.Text = "Export";
            this.cmdExport.UseVisualStyleBackColor = true;
            this.cmdExport.Click += new System.EventHandler(this.CmdExportClick);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(259, 433);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Close";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 459);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(346, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStatus
            // 
            this.toolStatus.Name = "toolStatus";
            this.toolStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // cmdValidate
            // 
            this.cmdValidate.Location = new System.Drawing.Point(74, 433);
            this.cmdValidate.Name = "cmdValidate";
            this.cmdValidate.Size = new System.Drawing.Size(98, 23);
            this.cmdValidate.TabIndex = 4;
            this.cmdValidate.Text = "Validate Image";
            this.cmdValidate.UseVisualStyleBackColor = true;
            this.cmdValidate.Visible = false;
            this.cmdValidate.Click += new System.EventHandler(this.cmdValidate_Click);
            // 
            // chkReExport
            // 
            this.chkReExport.AutoSize = true;
            this.chkReExport.Location = new System.Drawing.Point(12, 8);
            this.chkReExport.Name = "chkReExport";
            this.chkReExport.Size = new System.Drawing.Size(73, 17);
            this.chkReExport.TabIndex = 5;
            this.chkReExport.Text = "Re-Export";
            this.chkReExport.UseVisualStyleBackColor = true;
            this.chkReExport.CheckedChanged += new System.EventHandler(this.chkReExport_CheckedChanged);
            // 
            // aeExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 481);
            this.Controls.Add(this.chkReExport);
            this.Controls.Add(this.cmdValidate);
            this.Controls.Add(this.cmdExport);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "aeExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "aeExport";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AeExportFormClosing);
            this.Load += new System.EventHandler(this.AeExportLoad);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tblExp.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolStripStatusLabel toolStatus;
        private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdExport;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbProject;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbBatch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tblExp;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ListView lvwExportList;
        private System.Windows.Forms.ColumnHeader Boxes;
        private System.Windows.Forms.ColumnHeader colDock;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdValidate;
        private System.Windows.Forms.CheckBox chkReExport;
        private System.Windows.Forms.ColumnHeader columnHeader1;
	}
}
