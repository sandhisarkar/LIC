/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 17/2/2009
 * Time: 3:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
namespace ImageHeaven
{
	public abstract class frmAddEdit: Form
	{
		protected System.Windows.Forms.Button cmdSave;
		private System.Windows.Forms.Button cmdCancel;
		protected System.Windows.Forms.StatusStrip statusStrip1;
		
		public frmAddEdit()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panelButton = new System.Windows.Forms.Panel();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.panelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 408);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(503, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panelButton
            // 
            this.panelButton.Controls.Add(this.cmdSave);
            this.panelButton.Controls.Add(this.cmdCancel);
            this.panelButton.Location = new System.Drawing.Point(279, 240);
            this.panelButton.Name = "panelButton";
            this.panelButton.Size = new System.Drawing.Size(192, 40);
            this.panelButton.TabIndex = 3;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(7, 9);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(86, 22);
            this.cmdSave.TabIndex = 2;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.CmdSaveClick);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(99, 9);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(86, 22);
            this.cmdCancel.TabIndex = 3;
            this.cmdCancel.Text = "Close";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.CmdCancelClick);
            // 
            // frmAddEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 430);
            this.Controls.Add(this.panelButton);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "B\'Zer - Add/Edit";
            this.Load += new System.EventHandler(this.frmAddEditLoad);
            this.panelButton.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		protected System.Windows.Forms.Panel panelButton;
		void frmAddEditLoad(object sender, EventArgs e)
		{
			//this.Text = s
			//Utils.dbCon dbcon = new Utils.dbCon();
			//dbcon.Connect();
		}
		
		void CmdCancelClick(object sender, EventArgs e)
		{
			this.Close();
		}
		
		protected abstract void CmdSaveClick(object sender, EventArgs e);
	}
}
