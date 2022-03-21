/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 19/2/2008
 * Time: 4:24 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class aeBatch: frmAddEdit
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
			this.label1 = new System.Windows.Forms.Label();
			this.cmbProject = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtCode = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Project:";
			// 
			// cmbProject
			// 
			this.cmbProject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbProject.FormattingEnabled = true;
			this.cmbProject.Location = new System.Drawing.Point(84, 12);
			this.cmbProject.Name = "cmbProject";
			this.cmbProject.Size = new System.Drawing.Size(288, 21);
			this.cmbProject.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtName);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.txtCode);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Location = new System.Drawing.Point(27, 50);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(428, 80);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Batch";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(57, 45);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(365, 20);
			this.txtName.TabIndex = 7;
			this.txtName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtNameKeyPress);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(6, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Name:";
			// 
			// txtCode
			// 
			this.txtCode.Location = new System.Drawing.Point(57, 19);
			this.txtCode.Name = "txtCode";
			this.txtCode.Size = new System.Drawing.Size(365, 20);
			this.txtCode.TabIndex = 5;
			this.txtCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCodeKeyPress);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(6, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Code:";
			// 
			// aeBatch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ClientSize = new System.Drawing.Size(481, 223);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmbProject);
			this.Controls.Add(this.label1);
			this.Name = "aeBatch";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "aeBatch";
			this.Load += new System.EventHandler(this.AeBatchLoad);
			this.ResizeBegin += new System.EventHandler(this.AeBatchResizeBegin);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TextBox txtCode;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbProject;
		
		private System.Windows.Forms.Label label1;
		
		
	}
}
