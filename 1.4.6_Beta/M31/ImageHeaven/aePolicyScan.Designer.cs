/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 1/4/2008
 * Time: 4:51 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class aePolicyScan
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
            if (disposing)
            {
                if (components != null)
                {
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scanPic = new System.Windows.Forms.PictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstImageName = new System.Windows.Forms.ListBox();
            this.cmdCancelScan = new System.Windows.Forms.Button();
            this.cmdScan = new System.Windows.Forms.Button();
            this.lblPicSize = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblPageCount = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblNextPolicy = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblCurrentPolicy = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblBox = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblBatch = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.conHold = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.conMarkHold = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scanPic)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.conHold.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.cmdDelete);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.cmdCancelScan);
            this.panel1.Controls.Add(this.cmdScan);
            this.panel1.Controls.Add(this.lblPicSize);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(956, 709);
            this.panel1.TabIndex = 0;
            this.panel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseClick);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDelete.ForeColor = System.Drawing.Color.Red;
            this.cmdDelete.Location = new System.Drawing.Point(6, 599);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(212, 51);
            this.cmdDelete.TabIndex = 19;
            this.cmdDelete.Text = "Delete Images of Scanned Policy";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Controls.Add(this.scanPic);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(301, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(655, 709);
            this.panel2.TabIndex = 18;
            // 
            // scanPic
            // 
            this.scanPic.Dock = System.Windows.Forms.DockStyle.Right;
            this.scanPic.Location = new System.Drawing.Point(3, 0);
            this.scanPic.Name = "scanPic";
            this.scanPic.Size = new System.Drawing.Size(652, 709);
            this.scanPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.scanPic.TabIndex = 0;
            this.scanPic.TabStop = false;
            // 
            // groupBox5
            // 
            this.groupBox5.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.lblSize);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.lstImageName);
            this.groupBox5.Location = new System.Drawing.Point(0, 243);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(222, 305);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Image List";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(85, 199);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 16);
            this.label8.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(91, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 16);
            this.label7.TabIndex = 2;
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSize.Location = new System.Drawing.Point(85, 282);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(0, 16);
            this.lblSize.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 282);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Image Size:";
            // 
            // lstImageName
            // 
            this.lstImageName.FormattingEnabled = true;
            this.lstImageName.Location = new System.Drawing.Point(6, 19);
            this.lstImageName.Name = "lstImageName";
            this.lstImageName.Size = new System.Drawing.Size(205, 251);
            this.lstImageName.TabIndex = 0;
            this.lstImageName.SelectedIndexChanged += new System.EventHandler(this.lstImageName_SelectedIndexChanged);
            this.lstImageName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstImageName_KeyDown);
            this.lstImageName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstImageName_KeyPress);
            // 
            // cmdCancelScan
            // 
            this.cmdCancelScan.BackColor = System.Drawing.SystemColors.Control;
            this.cmdCancelScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdCancelScan.Location = new System.Drawing.Point(139, 554);
            this.cmdCancelScan.Name = "cmdCancelScan";
            this.cmdCancelScan.Size = new System.Drawing.Size(79, 39);
            this.cmdCancelScan.TabIndex = 16;
            this.cmdCancelScan.Text = "Cancel Scanning";
            this.cmdCancelScan.UseVisualStyleBackColor = false;
            this.cmdCancelScan.Click += new System.EventHandler(this.cmdCancelScan_Click);
            // 
            // cmdScan
            // 
            this.cmdScan.BackColor = System.Drawing.SystemColors.Control;
            this.cmdScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdScan.Location = new System.Drawing.Point(6, 554);
            this.cmdScan.Name = "cmdScan";
            this.cmdScan.Size = new System.Drawing.Size(79, 39);
            this.cmdScan.TabIndex = 16;
            this.cmdScan.Text = "Start Scanning";
            this.cmdScan.UseVisualStyleBackColor = false;
            this.cmdScan.Click += new System.EventHandler(this.CmdScanClick);
            // 
            // lblPicSize
            // 
            this.lblPicSize.AutoSize = true;
            this.lblPicSize.Location = new System.Drawing.Point(74, 182);
            this.lblPicSize.Name = "lblPicSize";
            this.lblPicSize.Size = new System.Drawing.Size(0, 13);
            this.lblPicSize.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.lblPageCount);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lblNextPolicy);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblCurrentPolicy);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lblBatch);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblProjectName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(222, 234);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control Details";
            // 
            // lblPageCount
            // 
            this.lblPageCount.AutoSize = true;
            this.lblPageCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageCount.ForeColor = System.Drawing.Color.Red;
            this.lblPageCount.Location = new System.Drawing.Point(93, 190);
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size(15, 16);
            this.lblPageCount.TabIndex = 11;
            this.lblPageCount.Text = "0";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(15, 190);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 16);
            this.label9.TabIndex = 10;
            this.label9.Text = "NRC No.:";
            // 
            // lblNextPolicy
            // 
            this.lblNextPolicy.AutoSize = true;
            this.lblNextPolicy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextPolicy.Location = new System.Drawing.Point(93, 155);
            this.lblNextPolicy.Name = "lblNextPolicy";
            this.lblNextPolicy.Size = new System.Drawing.Size(0, 16);
            this.lblNextPolicy.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(15, 158);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 16);
            this.label6.TabIndex = 8;
            this.label6.Text = "Next Record:";
            // 
            // lblCurrentPolicy
            // 
            this.lblCurrentPolicy.AutoSize = true;
            this.lblCurrentPolicy.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPolicy.Location = new System.Drawing.Point(112, 127);
            this.lblCurrentPolicy.Name = "lblCurrentPolicy";
            this.lblCurrentPolicy.Size = new System.Drawing.Size(0, 16);
            this.lblCurrentPolicy.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(15, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "Current Record:";
            // 
            // lblBox
            // 
            this.lblBox.AutoSize = true;
            this.lblBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBox.Location = new System.Drawing.Point(65, 96);
            this.lblBox.Name = "lblBox";
            this.lblBox.Size = new System.Drawing.Size(0, 16);
            this.lblBox.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 16);
            this.label4.TabIndex = 4;
            this.label4.Text = "Box:";
            // 
            // lblBatch
            // 
            this.lblBatch.AutoSize = true;
            this.lblBatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBatch.Location = new System.Drawing.Point(65, 63);
            this.lblBatch.Name = "lblBatch";
            this.lblBatch.Size = new System.Drawing.Size(0, 16);
            this.lblBatch.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Batch:";
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProjectName.Location = new System.Drawing.Point(72, 30);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(0, 16);
            this.lblProjectName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(222, 709);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // conHold
            // 
            this.conHold.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.conMarkHold});
            this.conHold.Name = "conHold";
            this.conHold.Size = new System.Drawing.Size(153, 48);
            // 
            // conMarkHold
            // 
            this.conMarkHold.Name = "conMarkHold";
            this.conMarkHold.Size = new System.Drawing.Size(152, 22);
            this.conMarkHold.Text = "Mark Hold";
            this.conMarkHold.Click += new System.EventHandler(this.conMarkHold_Click);
            // 
            // aePolicyScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 709);
            this.Controls.Add(this.panel1);
            this.Name = "aePolicyScan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "aePolicyScan";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AePolicyScanFormClosing);
            this.Load += new System.EventHandler(this.AePolicyScanLoad);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scanPic)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.conHold.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lblPageCount;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSize;
		private System.Windows.Forms.Label lblNextPolicy;
		private System.Windows.Forms.ListBox lstImageName;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button cmdScan;
		private System.Windows.Forms.Label lblPicSize;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblProjectName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblBatch;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblBox;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblCurrentPolicy;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button cmdCancelScan;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox scanPic;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.ContextMenuStrip conHold;
        private System.Windows.Forms.ToolStripMenuItem conMarkHold;
	}
}
