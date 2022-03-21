/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 6/4/2009
 * Time: 5:31 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class aeScanControl
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
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.rdoGrayScale = new System.Windows.Forms.RadioButton();
			this.rdoColor = new System.Windows.Forms.RadioButton();
			this.rdoBlackWhite = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.rdo300Dpi = new System.Windows.Forms.RadioButton();
			this.rdo200Dpi = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cmdOk = new System.Windows.Forms.Button();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.groupBox4.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox4
			// 
			this.groupBox4.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox4.Controls.Add(this.rdoGrayScale);
			this.groupBox4.Controls.Add(this.rdoColor);
			this.groupBox4.Controls.Add(this.rdoBlackWhite);
			this.groupBox4.Location = new System.Drawing.Point(6, 86);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(214, 103);
			this.groupBox4.TabIndex = 1;
			this.groupBox4.TabStop = false;
			// 
			// rdoGrayScale
			// 
			this.rdoGrayScale.AutoSize = true;
			this.rdoGrayScale.Location = new System.Drawing.Point(32, 40);
			this.rdoGrayScale.Name = "rdoGrayScale";
			this.rdoGrayScale.Size = new System.Drawing.Size(74, 17);
			this.rdoGrayScale.TabIndex = 2;
			this.rdoGrayScale.TabStop = true;
			this.rdoGrayScale.Text = "GrayScale";
			this.rdoGrayScale.UseVisualStyleBackColor = true;
			// 
			// rdoColor
			// 
			this.rdoColor.BackColor = System.Drawing.SystemColors.Control;
			this.rdoColor.Location = new System.Drawing.Point(32, 59);
			this.rdoColor.Name = "rdoColor";
			this.rdoColor.Size = new System.Drawing.Size(104, 24);
			this.rdoColor.TabIndex = 1;
			this.rdoColor.TabStop = true;
			this.rdoColor.Text = "Color";
			this.rdoColor.UseVisualStyleBackColor = false;
			// 
			// rdoBlackWhite
			// 
			this.rdoBlackWhite.BackColor = System.Drawing.SystemColors.Control;
			this.rdoBlackWhite.Location = new System.Drawing.Point(32, 14);
			this.rdoBlackWhite.Name = "rdoBlackWhite";
			this.rdoBlackWhite.Size = new System.Drawing.Size(104, 20);
			this.rdoBlackWhite.TabIndex = 0;
			this.rdoBlackWhite.TabStop = true;
			this.rdoBlackWhite.Text = "Black and White";
			this.rdoBlackWhite.UseVisualStyleBackColor = false;
			// 
			// groupBox3
			// 
			this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox3.Controls.Add(this.rdo300Dpi);
			this.groupBox3.Controls.Add(this.rdo200Dpi);
			this.groupBox3.Location = new System.Drawing.Point(6, 19);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(214, 61);
			this.groupBox3.TabIndex = 0;
			this.groupBox3.TabStop = false;
			// 
			// rdo300Dpi
			// 
			this.rdo300Dpi.BackColor = System.Drawing.SystemColors.Control;
			this.rdo300Dpi.Location = new System.Drawing.Point(32, 31);
			this.rdo300Dpi.Name = "rdo300Dpi";
			this.rdo300Dpi.Size = new System.Drawing.Size(104, 24);
			this.rdo300Dpi.TabIndex = 1;
			this.rdo300Dpi.TabStop = true;
			this.rdo300Dpi.Text = "300 DPI";
			this.rdo300Dpi.UseVisualStyleBackColor = false;
			// 
			// rdo200Dpi
			// 
			this.rdo200Dpi.BackColor = System.Drawing.SystemColors.Control;
			this.rdo200Dpi.Location = new System.Drawing.Point(32, 14);
			this.rdo200Dpi.Name = "rdo200Dpi";
			this.rdo200Dpi.Size = new System.Drawing.Size(104, 20);
			this.rdo200Dpi.TabIndex = 0;
			this.rdo200Dpi.TabStop = true;
			this.rdo200Dpi.Text = "200 DPI";
			this.rdo200Dpi.UseVisualStyleBackColor = false;
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
			this.groupBox2.Controls.Add(this.groupBox4);
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Location = new System.Drawing.Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(226, 211);
			this.groupBox2.TabIndex = 14;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Scanner Control Values";
			// 
			// cmdOk
			// 
			this.cmdOk.Location = new System.Drawing.Point(82, 229);
			this.cmdOk.Name = "cmdOk";
			this.cmdOk.Size = new System.Drawing.Size(75, 23);
			this.cmdOk.TabIndex = 15;
			this.cmdOk.Text = "Ok";
			this.cmdOk.UseVisualStyleBackColor = true;
			this.cmdOk.Click += new System.EventHandler(this.CmdOkClick);
			// 
			// cmdCancel
			// 
			this.cmdCancel.Location = new System.Drawing.Point(163, 229);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 16;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.CmdCancelClick);
			// 
			// aeScanControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(250, 258);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdOk);
			this.Controls.Add(this.groupBox2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "aeScanControl";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "B'Zer - Scanner settings";
			this.Load += new System.EventHandler(this.AeScanControlLoad);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdOk;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton rdo200Dpi;
		private System.Windows.Forms.RadioButton rdo300Dpi;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton rdoBlackWhite;
		private System.Windows.Forms.RadioButton rdoColor;
		private System.Windows.Forms.RadioButton rdoGrayScale;
		private System.Windows.Forms.GroupBox groupBox4;
	}
}
