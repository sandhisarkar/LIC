/*
 * Created by SharpDevelop.
 * User: user
 * Date: 18/08/2009
 * Time: 11:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class frmSingleExp
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSingleExp));
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cmbBatch = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbProject = new System.Windows.Forms.ComboBox();
			this.cmbBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lvwExportList = new System.Windows.Forms.ListView();
			this.Policy = new System.Windows.Forms.ColumnHeader();
			this.polHoldName = new System.Windows.Forms.ColumnHeader();
			this.cmdExport = new System.Windows.Forms.Button();
			this.srlno = new System.Windows.Forms.ColumnHeader();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lvwExportList);
			this.groupBox2.Location = new System.Drawing.Point(4, 109);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(363, 319);
			this.groupBox2.TabIndex = 24;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Policy Details";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cmbBatch);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.cmbProject);
			this.groupBox1.Controls.Add(this.cmbBox);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(1, 2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(366, 101);
			this.groupBox1.TabIndex = 23;
			this.groupBox1.TabStop = false;
			// 
			// cmbBatch
			// 
			this.cmbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBatch.FormattingEnabled = true;
			this.cmbBatch.Location = new System.Drawing.Point(50, 40);
			this.cmbBatch.Name = "cmbBatch";
			this.cmbBatch.Size = new System.Drawing.Size(186, 21);
			this.cmbBatch.TabIndex = 2;
			this.cmbBatch.Leave += new System.EventHandler(this.CmbBatchLeave);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(16, 70);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(28, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Box:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(7, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Batch:";
			// 
			// cmbProject
			// 
			this.cmbProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbProject.FormattingEnabled = true;
			this.cmbProject.Location = new System.Drawing.Point(50, 13);
			this.cmbProject.Name = "cmbProject";
			this.cmbProject.Size = new System.Drawing.Size(186, 21);
			this.cmbProject.TabIndex = 1;
			this.cmbProject.Leave += new System.EventHandler(this.CmbProjectLeave);
			// 
			// cmbBox
			// 
			this.cmbBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbBox.FormattingEnabled = true;
			this.cmbBox.Location = new System.Drawing.Point(50, 67);
			this.cmbBox.Name = "cmbBox";
			this.cmbBox.Size = new System.Drawing.Size(186, 21);
			this.cmbBox.TabIndex = 3;
			this.cmbBox.Leave += new System.EventHandler(this.CmbBoxLeave);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(3, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Project:";
			// 
			// lvwExportList
			// 
			this.lvwExportList.CheckBoxes = true;
			this.lvwExportList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
									this.srlno,
									this.Policy,
									this.polHoldName});
			this.lvwExportList.FullRowSelect = true;
			this.lvwExportList.GridLines = true;
			this.lvwExportList.Location = new System.Drawing.Point(8, 19);
			this.lvwExportList.Name = "lvwExportList";
			this.lvwExportList.Size = new System.Drawing.Size(347, 294);
			this.lvwExportList.TabIndex = 7;
			this.lvwExportList.UseCompatibleStateImageBehavior = false;
			this.lvwExportList.View = System.Windows.Forms.View.Details;
			// 
			// Policy
			// 
			this.Policy.Text = "Policy Number";
			this.Policy.Width = 100;
			// 
			// polHoldName
			// 
			this.polHoldName.Text = "Policy Holder Name";
			this.polHoldName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.polHoldName.Width = 200;
			// 
			// cmdExport
			// 
			this.cmdExport.Location = new System.Drawing.Point(292, 434);
			this.cmdExport.Name = "cmdExport";
			this.cmdExport.Size = new System.Drawing.Size(75, 23);
			this.cmdExport.TabIndex = 25;
			this.cmdExport.Text = "Export";
			this.cmdExport.UseVisualStyleBackColor = true;
			this.cmdExport.Click += new System.EventHandler(this.CmdExportClick);
			// 
			// srlno
			// 
			this.srlno.Text = "SrlNo";
			this.srlno.Width = 40;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.toolStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 469);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(369, 22);
			this.statusStrip1.TabIndex = 26;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStatus
			// 
			this.toolStatus.Name = "toolStatus";
			this.toolStatus.Size = new System.Drawing.Size(0, 17);
			// 
			// frmSingleExp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(369, 491);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.cmdExport);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmSingleExp";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ReExport: Policy level";
			this.Load += new System.EventHandler(this.FrmSingleExpLoad);
			this.groupBox2.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripStatusLabel toolStatus;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ColumnHeader srlno;
		private System.Windows.Forms.Button cmdExport;
		private System.Windows.Forms.ColumnHeader polHoldName;
		private System.Windows.Forms.ColumnHeader Policy;
		private System.Windows.Forms.ListView lvwExportList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbBox;
		private System.Windows.Forms.ComboBox cmbProject;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbBatch;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
	}
}
