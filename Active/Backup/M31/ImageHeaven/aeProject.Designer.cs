/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 18/2/2009
 * Time: 11:52 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
 using System.Windows.Forms;
 
namespace ImageHeaven
{
	partial class aeProject: frmAddEdit
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cmdBrowseScannLoc = new System.Windows.Forms.Button();
			this.txtScannedLoc = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtProjectName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Controls.Add(this.txtProjectName);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(11, 11);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(481, 115);
			this.panel1.TabIndex = 0;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cmdBrowseScannLoc);
			this.groupBox1.Controls.Add(this.txtScannedLoc);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(20, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(438, 62);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Project Location";
			// 
			// cmdBrowseScannLoc
			// 
			this.cmdBrowseScannLoc.Location = new System.Drawing.Point(401, 21);
			this.cmdBrowseScannLoc.Name = "cmdBrowseScannLoc";
			this.cmdBrowseScannLoc.Size = new System.Drawing.Size(29, 23);
			this.cmdBrowseScannLoc.TabIndex = 8;
			this.cmdBrowseScannLoc.Text = "....";
			this.cmdBrowseScannLoc.UseVisualStyleBackColor = true;
			this.cmdBrowseScannLoc.Click += new System.EventHandler(this.CmdBrowseScannLocClick);
			// 
			// txtScannedLoc
			// 
			this.txtScannedLoc.Enabled = false;
			this.txtScannedLoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtScannedLoc.Location = new System.Drawing.Point(71, 23);
			this.txtScannedLoc.Name = "txtScannedLoc";
			this.txtScannedLoc.Size = new System.Drawing.Size(359, 20);
			this.txtScannedLoc.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(5, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(70, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Folder Name:";
			// 
			// txtProjectName
			// 
			this.txtProjectName.Location = new System.Drawing.Point(91, 14);
			this.txtProjectName.Name = "txtProjectName";
			this.txtProjectName.Size = new System.Drawing.Size(359, 20);
			this.txtProjectName.TabIndex = 1;
			this.txtProjectName.Leave += new System.EventHandler(this.TxtProjectNameLeave);
			this.txtProjectName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtProjectNameKeyPress);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(25, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			// 
			// aeProject
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(503, 207);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Controls.Add(this.panel1);
			this.Name = "aeProject";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "B'Zer - Project Add/Edit";
			this.Load += new System.EventHandler(this.AeProjectLoad);
			this.ResizeBegin += new System.EventHandler(this.AeProjectResizeBegin);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtProjectName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtScannedLoc;
		
		
		private System.Windows.Forms.Button cmdBrowseScannLoc;
		
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		//private System.Windows.Forms.MaskedTextBox maskedTextBox1;
		
		
				
	}
}
