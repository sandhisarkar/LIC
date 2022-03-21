namespace ImageHeaven
{
    partial class frmRepeatScan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProjects = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBatch = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvwBox = new System.Windows.Forms.ListView();
            this.clmnSerial = new System.Windows.Forms.ColumnHeader();
            this.clmnBox = new System.Windows.Forms.ColumnHeader();
            this.cmdReadyQC = new System.Windows.Forms.Button();
            this.cmdRepeatScan = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbBatch);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbProjects);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 96);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Projects:";
            // 
            // cmbProjects
            // 
            this.cmbProjects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProjects.FormattingEnabled = true;
            this.cmbProjects.Location = new System.Drawing.Point(63, 28);
            this.cmbProjects.Name = "cmbProjects";
            this.cmbProjects.Size = new System.Drawing.Size(181, 21);
            this.cmbProjects.TabIndex = 0;
            this.cmbProjects.Leave += new System.EventHandler(this.cmbProjects_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Batch:";
            // 
            // cmbBatch
            // 
            this.cmbBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBatch.FormattingEnabled = true;
            this.cmbBatch.Location = new System.Drawing.Point(63, 55);
            this.cmbBatch.Name = "cmbBatch";
            this.cmbBatch.Size = new System.Drawing.Size(181, 21);
            this.cmbBatch.TabIndex = 1;
            this.cmbBatch.Leave += new System.EventHandler(this.cmbBatch_Leave);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvwBox);
            this.groupBox2.Location = new System.Drawing.Point(2, 106);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(255, 260);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ready Box";
            // 
            // lvwBox
            // 
            this.lvwBox.CheckBoxes = true;
            this.lvwBox.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmnSerial,
            this.clmnBox});
            this.lvwBox.GridLines = true;
            this.lvwBox.Location = new System.Drawing.Point(12, 19);
            this.lvwBox.Name = "lvwBox";
            this.lvwBox.Size = new System.Drawing.Size(232, 224);
            this.lvwBox.TabIndex = 2;
            this.lvwBox.UseCompatibleStateImageBehavior = false;
            this.lvwBox.View = System.Windows.Forms.View.Details;
            // 
            // clmnSerial
            // 
            this.clmnSerial.Text = "Serial Number";
            this.clmnSerial.Width = 100;
            // 
            // clmnBox
            // 
            this.clmnBox.Text = "Box Number";
            this.clmnBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clmnBox.Width = 100;
            // 
            // cmdReadyQC
            // 
            this.cmdReadyQC.Location = new System.Drawing.Point(2, 373);
            this.cmdReadyQC.Name = "cmdReadyQC";
            this.cmdReadyQC.Size = new System.Drawing.Size(83, 23);
            this.cmdReadyQC.TabIndex = 3;
            this.cmdReadyQC.Text = "Ready For QC";
            this.cmdReadyQC.UseVisualStyleBackColor = true;
            this.cmdReadyQC.Click += new System.EventHandler(this.cmdReadyQC_Click);
            // 
            // cmdRepeatScan
            // 
            this.cmdRepeatScan.Location = new System.Drawing.Point(91, 373);
            this.cmdRepeatScan.Name = "cmdRepeatScan";
            this.cmdRepeatScan.Size = new System.Drawing.Size(83, 23);
            this.cmdRepeatScan.TabIndex = 4;
            this.cmdRepeatScan.Text = "Repeat Scan";
            this.cmdRepeatScan.UseVisualStyleBackColor = true;
            this.cmdRepeatScan.Click += new System.EventHandler(this.cmdRepeatScan_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(180, 373);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(77, 23);
            this.cmdCancel.TabIndex = 5;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // frmRepeatScan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 403);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdRepeatScan);
            this.Controls.Add(this.cmdReadyQC);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmRepeatScan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Repeat Scan/ QC Control";
            this.Load += new System.EventHandler(this.frmRepeatScan_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbProjects;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView lvwBox;
        private System.Windows.Forms.ColumnHeader clmnSerial;
        private System.Windows.Forms.ColumnHeader clmnBox;
        private System.Windows.Forms.Button cmdReadyQC;
        private System.Windows.Forms.Button cmdRepeatScan;
        private System.Windows.Forms.Button cmdCancel;
    }
}