namespace ImageHeaven
{
    partial class frmJobDistribution
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmJobDistribution));
            this.grdStatus = new System.Windows.Forms.DataGridView();
            this.trv = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // grdStatus
            // 
            this.grdStatus.AllowUserToAddRows = false;
            this.grdStatus.AllowUserToDeleteRows = false;
            this.grdStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdStatus.Location = new System.Drawing.Point(189, 0);
            this.grdStatus.MultiSelect = false;
            this.grdStatus.Name = "grdStatus";
            this.grdStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdStatus.Size = new System.Drawing.Size(817, 596);
            this.grdStatus.TabIndex = 6;
            this.grdStatus.DoubleClick += new System.EventHandler(this.GrdStatusDoubleClick);
            // 
            // trv
            // 
            this.trv.CheckBoxes = true;
            this.trv.Location = new System.Drawing.Point(1, 4);
            this.trv.Name = "trv";
            this.trv.Size = new System.Drawing.Size(182, 592);
            this.trv.TabIndex = 7;
            this.trv.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TrvNodeMouseClick);
            // 
            // frmJobDistribution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 598);
            this.Controls.Add(this.trv);
            this.Controls.Add(this.grdStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "frmJobDistribution";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Policy Status: Box Wise";
            this.Load += new System.EventHandler(this.frmJobDistribution_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmJobDistribution_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.grdStatus)).EndInit();
            this.ResumeLayout(false);

        }
        private System.Windows.Forms.TreeView trv;

        #endregion

        private System.Windows.Forms.DataGridView grdStatus;
    }
}