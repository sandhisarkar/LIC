/*
 * Created by SharpDevelop.
 * User: user
 * Date: 18/01/2007
 * Time: 10:17 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class frmPolicyList
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
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.grdStatus = new System.Windows.Forms.DataGridView();
			this.lblProject = new System.Windows.Forms.Label();
			this.lblBatch = new System.Windows.Forms.Label();
			this.lblBox = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdStatus)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblBox);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.lblBatch);
			this.groupBox1.Controls.Add(this.lblProject);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(2, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(718, 104);
			this.groupBox1.TabIndex = 7;
			this.groupBox1.TabStop = false;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 22);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Project:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 46);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Batch:";
			// 
			// grdStatus
			// 
			this.grdStatus.AllowUserToAddRows = false;
			this.grdStatus.AllowUserToDeleteRows = false;
			this.grdStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grdStatus.Location = new System.Drawing.Point(2, 122);
			this.grdStatus.MultiSelect = false;
			this.grdStatus.Name = "grdStatus";
			this.grdStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grdStatus.Size = new System.Drawing.Size(798, 517);
			this.grdStatus.TabIndex = 8;
			// 
			// lblProject
			// 
			this.lblProject.Location = new System.Drawing.Point(59, 22);
			this.lblProject.Name = "lblProject";
			this.lblProject.Size = new System.Drawing.Size(112, 23);
			this.lblProject.TabIndex = 7;
			this.lblProject.Text = "label2";
			// 
			// lblBatch
			// 
			this.lblBatch.Location = new System.Drawing.Point(59, 45);
			this.lblBatch.Name = "lblBatch";
			this.lblBatch.Size = new System.Drawing.Size(116, 23);
			this.lblBatch.TabIndex = 8;
			this.lblBatch.Text = "label4";
			// 
			// lblBox
			// 
			this.lblBox.Location = new System.Drawing.Point(59, 68);
			this.lblBox.Name = "lblBox";
			this.lblBox.Size = new System.Drawing.Size(152, 23);
			this.lblBox.TabIndex = 10;
			this.lblBox.Text = "label5";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(10, 69);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(38, 13);
			this.label6.TabIndex = 9;
			this.label6.Text = "Batch:";
			// 
			// frmPolicyList
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(802, 517);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grdStatus);
			this.Name = "frmPolicyList";
			this.Text = "frmPolicyList";
			this.Load += new System.EventHandler(this.FrmPolicyListLoad);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.grdStatus)).EndInit();
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Label lblProject;
		private System.Windows.Forms.Label lblBatch;
		private System.Windows.Forms.Label lblBox;
		private System.Windows.Forms.DataGridView grdStatus;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}
