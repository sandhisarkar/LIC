/*
 * Created by SharpDevelop.
 * User: SubhajitB
 * Date: 9/4/2009
 * Time: 3:27 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace ImageHeaven
{
	partial class aeFQC
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
            NovaNet.wfe.eSTATES[] state = new NovaNet.wfe.eSTATES[7];
            NovaNet.wfe.eSTATES[] imageState = new NovaNet.wfe.eSTATES[1];
            state[0] = NovaNet.wfe.eSTATES.POLICY_FQC;
            state[1] = NovaNet.wfe.eSTATES.POLICY_INDEXED;
            state[2] = NovaNet.wfe.eSTATES.POLICY_EXCEPTION;
            state[3] = NovaNet.wfe.eSTATES.POLICY_ON_HOLD;
            state[4] = NovaNet.wfe.eSTATES.POLICY_CHECKED;
            state[5] = NovaNet.wfe.eSTATES.POLICY_NOT_INDEXED;
            state[6] = NovaNet.wfe.eSTATES.POLICY_EXPORTED;
            this.components = new System.ComponentModel.Container();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureControl = new System.Windows.Forms.PictureBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lnkPage3 = new System.Windows.Forms.LinkLabel();
            this.lnkPage2 = new System.Windows.Forms.LinkLabel();
            this.lnkPage1 = new System.Windows.Forms.LinkLabel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmdCustExcp = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cmdFetch = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cmbDocType = new System.Windows.Forms.ComboBox();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.rdoLIC = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cmdOk = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtExceptionType = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.cmdResolved = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.txtCommDt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDOB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPolicyNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.type = new System.Windows.Forms.GroupBox();
            this.lvwDockTypes = new System.Windows.Forms.ListView();
            this.clmDockType = new System.Windows.Forms.ColumnHeader();
            this.clmShrtCut = new System.Windows.Forms.ColumnHeader();
            this.clmCount = new System.Windows.Forms.ColumnHeader();
            this.BoxDtls = new wSelect.BoxDetails();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.markNotReadyHoldPolicyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.markReadyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowBatchSummary = new System.Windows.Forms.ToolStripMenuItem();
            this.lblPolicyNumber = new System.Windows.Forms.Label();
            this.cmbPolicy = new System.Windows.Forms.ComboBox();
            this.fileDlg = new System.Windows.Forms.OpenFileDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureControl)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.type.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dockPanel
            // 
            this.dockPanel.ActiveAutoHideContent = null;
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.Location = new System.Drawing.Point(0, 0);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(1028, 746);
            this.dockPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl2);
            this.panel1.Location = new System.Drawing.Point(257, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(512, 712);
            this.panel1.TabIndex = 4;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(512, 712);
            this.tabControl2.TabIndex = 2;
            // 
            // tabPage3
            // 
            this.tabPage3.AutoScroll = true;
            this.tabPage3.Controls.Add(this.pictureControl);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(504, 686);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Single";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureControl
            // 
            this.pictureControl.Location = new System.Drawing.Point(-1, 3);
            this.pictureControl.Name = "pictureControl";
            this.pictureControl.Size = new System.Drawing.Size(502, 680);
            this.pictureControl.TabIndex = 1;
            this.pictureControl.TabStop = false;
            this.pictureControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseMove);
            this.pictureControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseDown);
            this.pictureControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureControlMouseUp);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lnkPage3);
            this.tabPage4.Controls.Add(this.lnkPage2);
            this.tabPage4.Controls.Add(this.lnkPage1);
            this.tabPage4.Controls.Add(this.pictureBox2);
            this.tabPage4.Controls.Add(this.pictureBox6);
            this.tabPage4.Controls.Add(this.pictureBox5);
            this.tabPage4.Controls.Add(this.pictureBox4);
            this.tabPage4.Controls.Add(this.pictureBox3);
            this.tabPage4.Controls.Add(this.pictureBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(504, 686);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "All";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lnkPage3
            // 
            this.lnkPage3.Location = new System.Drawing.Point(510, 639);
            this.lnkPage3.Name = "lnkPage3";
            this.lnkPage3.Size = new System.Drawing.Size(76, 15);
            this.lnkPage3.TabIndex = 1;
            this.lnkPage3.TabStop = true;
            this.lnkPage3.Text = "Page three";
            this.lnkPage3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPage3_LinkClicked);
            // 
            // lnkPage2
            // 
            this.lnkPage2.Location = new System.Drawing.Point(510, 623);
            this.lnkPage2.Name = "lnkPage2";
            this.lnkPage2.Size = new System.Drawing.Size(55, 16);
            this.lnkPage2.TabIndex = 1;
            this.lnkPage2.TabStop = true;
            this.lnkPage2.Text = "Page two";
            this.lnkPage2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPage2_LinkClicked);
            // 
            // lnkPage1
            // 
            this.lnkPage1.Location = new System.Drawing.Point(510, 607);
            this.lnkPage1.Name = "lnkPage1";
            this.lnkPage1.Size = new System.Drawing.Size(55, 16);
            this.lnkPage1.TabIndex = 1;
            this.lnkPage1.TabStop = true;
            this.lnkPage1.Text = "Page one";
            this.lnkPage1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkPage1_LinkClicked);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Location = new System.Drawing.Point(175, 19);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(163, 328);
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.DoubleClick += new System.EventHandler(this.PictureBox2DoubleClick);
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox6.Location = new System.Drawing.Point(341, 353);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(163, 325);
            this.pictureBox6.TabIndex = 0;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.DoubleClick += new System.EventHandler(this.PictureBox6DoubleClick);
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox5.Location = new System.Drawing.Point(6, 353);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(163, 325);
            this.pictureBox5.TabIndex = 0;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.DoubleClick += new System.EventHandler(this.PictureBox5DoubleClick);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox4.Location = new System.Drawing.Point(175, 353);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(163, 325);
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.DoubleClick += new System.EventHandler(this.PictureBox4DoubleClick);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox3.Location = new System.Drawing.Point(341, 19);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(163, 328);
            this.pictureBox3.TabIndex = 0;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.DoubleClick += new System.EventHandler(this.PictureBox3DoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(6, 19);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(163, 328);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DoubleClick += new System.EventHandler(this.PictureBox1DoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmdCustExcp);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.cmdFetch);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.cmbDocType);
            this.panel2.Controls.Add(this.rdoAll);
            this.panel2.Controls.Add(this.rdoLIC);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.type);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(770, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(258, 746);
            this.panel2.TabIndex = 5;
            // 
            // cmdCustExcp
            // 
            this.cmdCustExcp.Location = new System.Drawing.Point(6, 682);
            this.cmdCustExcp.Name = "cmdCustExcp";
            this.cmdCustExcp.Size = new System.Drawing.Size(240, 23);
            this.cmdCustExcp.TabIndex = 11;
            this.cmdCustExcp.Text = "Show Custom Exception List";
            this.cmdCustExcp.UseVisualStyleBackColor = true;
            this.cmdCustExcp.Click += new System.EventHandler(this.cmdCustExcp_Click);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(179, 609);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(54, 14);
            this.label10.TabIndex = 10;
            this.label10.Text = "Count";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 609);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 14);
            this.label9.TabIndex = 9;
            this.label9.Text = "Doc types";
            // 
            // cmdFetch
            // 
            this.cmdFetch.Location = new System.Drawing.Point(169, 653);
            this.cmdFetch.Name = "cmdFetch";
            this.cmdFetch.Size = new System.Drawing.Size(75, 23);
            this.cmdFetch.TabIndex = 8;
            this.cmdFetch.Text = "Fetch";
            this.cmdFetch.UseVisualStyleBackColor = true;
            this.cmdFetch.Click += new System.EventHandler(this.CmdFetchClick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(179, 625);
            this.textBox1.MaxLength = 2;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(61, 20);
            this.textBox1.TabIndex = 7;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            ">",
            "=",
            "<"});
            this.comboBox1.Location = new System.Drawing.Point(134, 626);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(40, 21);
            this.comboBox1.TabIndex = 6;
            // 
            // cmbDocType
            // 
            this.cmbDocType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDocType.FormattingEnabled = true;
            this.cmbDocType.Items.AddRange(new object[] {
            "Proposal_form",
            "Photo_addendum",
            "Proposal_enclosures",
            "Signature_page",
            "Medical_report",
            "Proposal_review_slip",
            "Policy_bond",
            "Nomination",
            "Assignment",
            "Alteration",
            "Revivals",
            "Policy_loans",
            "Surrender",
            "Claims",
            "Correspondence",
            "Others",
            "KYC_documents"});
            this.cmbDocType.Location = new System.Drawing.Point(6, 626);
            this.cmbDocType.Name = "cmbDocType";
            this.cmbDocType.Size = new System.Drawing.Size(125, 21);
            this.cmbDocType.TabIndex = 5;
            this.cmbDocType.SelectedIndexChanged += new System.EventHandler(this.cmbDocType_SelectedIndexChanged);
            // 
            // rdoAll
            // 
            this.rdoAll.Location = new System.Drawing.Point(6, 582);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(73, 24);
            this.rdoAll.TabIndex = 4;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "Show All";
            this.rdoAll.UseVisualStyleBackColor = true;
            this.rdoAll.CheckedChanged += new System.EventHandler(this.RdoAllCheckedChanged);
            // 
            // rdoLIC
            // 
            this.rdoLIC.Location = new System.Drawing.Point(85, 582);
            this.rdoLIC.Name = "rdoLIC";
            this.rdoLIC.Size = new System.Drawing.Size(104, 24);
            this.rdoLIC.TabIndex = 3;
            this.rdoLIC.TabStop = true;
            this.rdoLIC.Text = "LIC Exception";
            this.rdoLIC.UseVisualStyleBackColor = true;
            this.rdoLIC.CheckedChanged += new System.EventHandler(this.RdoLICCheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tabControl1);
            this.groupBox2.Location = new System.Drawing.Point(5, 375);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(239, 201);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Exceptions";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(236, 179);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.cmdOk);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.txtComments);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.txtExceptionType);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(228, 153);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "LIC QA";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cmdOk
            // 
            this.cmdOk.Location = new System.Drawing.Point(126, 125);
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.Size = new System.Drawing.Size(96, 23);
            this.cmdOk.TabIndex = 8;
            this.cmdOk.Text = "Mark ok (Policy)";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.CmdOkClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Comments";
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(6, 26);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtComments.Size = new System.Drawing.Size(216, 93);
            this.txtComments.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(3, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Type";
            // 
            // txtExceptionType
            // 
            this.txtExceptionType.Location = new System.Drawing.Point(6, 26);
            this.txtExceptionType.Multiline = true;
            this.txtExceptionType.Name = "txtExceptionType";
            this.txtExceptionType.Size = new System.Drawing.Size(216, 46);
            this.txtExceptionType.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listView1);
            this.tabPage2.Controls.Add(this.cmdResolved);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(228, 153);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Custom";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(7, 23);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(215, 73);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Exception Type";
            this.columnHeader1.Width = 100;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Remarks";
            this.columnHeader2.Width = 100;
            // 
            // cmdResolved
            // 
            this.cmdResolved.Location = new System.Drawing.Point(147, 102);
            this.cmdResolved.Name = "cmdResolved";
            this.cmdResolved.Size = new System.Drawing.Size(75, 23);
            this.cmdResolved.TabIndex = 2;
            this.cmdResolved.Text = "Resolved";
            this.cmdResolved.UseVisualStyleBackColor = true;
            this.cmdResolved.Click += new System.EventHandler(this.cmdClick);
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 16);
            this.label7.TabIndex = 0;
            this.label7.Text = "Exception Type";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdUpdate);
            this.groupBox1.Controls.Add(this.txtCommDt);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtDOB);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtPolicyNumber);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 237);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(239, 132);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Policy Details";
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.Location = new System.Drawing.Point(140, 97);
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.Size = new System.Drawing.Size(75, 23);
            this.cmdUpdate.TabIndex = 2;
            this.cmdUpdate.Text = "Update";
            this.cmdUpdate.UseVisualStyleBackColor = true;
            this.cmdUpdate.Click += new System.EventHandler(this.CmdUpdateClick);
            // 
            // txtCommDt
            // 
            this.txtCommDt.Enabled = false;
            this.txtCommDt.Location = new System.Drawing.Point(160, 71);
            this.txtCommDt.Name = "txtCommDt";
            this.txtCommDt.Size = new System.Drawing.Size(55, 20);
            this.txtCommDt.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Com.Dt:";
            // 
            // txtDOB
            // 
            this.txtDOB.Enabled = false;
            this.txtDOB.Location = new System.Drawing.Point(41, 71);
            this.txtDOB.Name = "txtDOB";
            this.txtDOB.Size = new System.Drawing.Size(64, 20);
            this.txtDOB.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "DOB:";
            // 
            // txtName
            // 
            this.txtName.Enabled = false;
            this.txtName.Location = new System.Drawing.Point(41, 45);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(174, 20);
            this.txtName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Name:";
            // 
            // txtPolicyNumber
            // 
            this.txtPolicyNumber.Enabled = false;
            this.txtPolicyNumber.Location = new System.Drawing.Point(41, 19);
            this.txtPolicyNumber.Name = "txtPolicyNumber";
            this.txtPolicyNumber.Size = new System.Drawing.Size(174, 20);
            this.txtPolicyNumber.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number:";
            // 
            // type
            // 
            this.type.Controls.Add(this.lvwDockTypes);
            this.type.Location = new System.Drawing.Point(5, 3);
            this.type.Name = "type";
            this.type.Size = new System.Drawing.Size(239, 228);
            this.type.TabIndex = 0;
            this.type.TabStop = false;
            this.type.Text = "DockTypes";
            // 
            // lvwDockTypes
            // 
            this.lvwDockTypes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmDockType,
            this.clmShrtCut,
            this.clmCount});
            this.lvwDockTypes.GridLines = true;
            this.lvwDockTypes.Location = new System.Drawing.Point(7, 19);
            this.lvwDockTypes.Name = "lvwDockTypes";
            this.lvwDockTypes.Size = new System.Drawing.Size(221, 202);
            this.lvwDockTypes.TabIndex = 0;
            this.lvwDockTypes.UseCompatibleStateImageBehavior = false;
            this.lvwDockTypes.View = System.Windows.Forms.View.Details;
            this.lvwDockTypes.SelectedIndexChanged += new System.EventHandler(this.lvwDockTypes_SelectedIndexChanged);
            this.lvwDockTypes.Click += new System.EventHandler(this.lvwDockTypes_Click);
            // 
            // clmDockType
            // 
            this.clmDockType.Text = "DocTypes";
            this.clmDockType.Width = 140;
            // 
            // clmShrtCut
            // 
            this.clmShrtCut.Text = "Keys";
            this.clmShrtCut.Width = 40;
            // 
            // clmCount
            // 
            this.clmCount.Text = "Count";
            this.clmCount.Width = 40;
            // 
            // BoxDtls
            // 
            this.BoxDtls.Location = new System.Drawing.Point(0, 0);
            this.BoxDtls.Name = "BoxDtls";
            this.BoxDtls.Size = new System.Drawing.Size(240, 453);
            this.BoxDtls.TabIndex = 9;
            this.BoxDtls.LstImageIndex += new wSelect.LstImageIndexKeyPress(this.BoxDtlsLstImageIndex);
            this.BoxDtls.BoxLoaded += new wSelect.BoxDetailsLoaded(this.BoxDtlsLoaded);
            this.BoxDtls.cmbChanged += new wSelect.ComboValueChanged(this.BoxDtls_cmbChanged);
            this.BoxDtls.LstDelIamgeInsert += new wSelect.LstDelImageKeyPress(this.BoxDtlsLstDelIamgeInsert);
            this.BoxDtls.NextClicked += new wSelect.NextClickedHandler(this.BoxDtlsNextClicked);
            this.BoxDtls.PreviousClicked += new wSelect.PreviousClickedHandler(this.BoxDtls_PreviousClicked);
            this.BoxDtls.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BoxDtlsMouseDown);
            this.BoxDtls.LstDelImgClick += new wSelect.LstDelImageClick(this.BoxDtls_LstDelImgClick);
            this.BoxDtls.BoxMouseClick += new wSelect.BoxDetailsMouseClick(this.BoxDtlsBoxMouseClick);
            this.BoxDtls.LstImgClick += new wSelect.LstImageClick(this.BoxDtls_LstImgClick);
            this.BoxDtls.ImageChanged += new wSelect.ImageChangeHandler(this.BoxDtlsImageChanged);
            this.BoxDtls.PolicyChanged += new wSelect.PolicyChangeHandler(this.BoxDtlsPolicyChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.markNotReadyHoldPolicyToolStripMenuItem,
            this.markReadyToolStripMenuItem,
            this.ShowBatchSummary});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(225, 70);
            this.contextMenuStrip1.Click += new System.EventHandler(this.HoldPolicyToolStripMenuItemClick);
            // 
            // markNotReadyHoldPolicyToolStripMenuItem
            // 
            this.markNotReadyHoldPolicyToolStripMenuItem.Name = "markNotReadyHoldPolicyToolStripMenuItem";
            this.markNotReadyHoldPolicyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.markNotReadyHoldPolicyToolStripMenuItem.Text = "Mark Not Ready (Hold Policy)";
            this.markNotReadyHoldPolicyToolStripMenuItem.Click += new System.EventHandler(this.HoldPolicyToolStripMenuItemClick);
            // 
            // markReadyToolStripMenuItem
            // 
            this.markReadyToolStripMenuItem.Name = "markReadyToolStripMenuItem";
            this.markReadyToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.markReadyToolStripMenuItem.Text = "Mark Ready";
            this.markReadyToolStripMenuItem.Click += new System.EventHandler(this.markReadyToolStripMenuItemClick);
            // 
            // ShowBatchSummary
            // 
            this.ShowBatchSummary.Name = "ShowBatchSummary";
            this.ShowBatchSummary.Size = new System.Drawing.Size(224, 22);
            this.ShowBatchSummary.Text = "Show Batch Summay";
            this.ShowBatchSummary.Visible = false;
            this.ShowBatchSummary.Click += new System.EventHandler(this.ShowBatchSummary_Click);
            // 
            // lblPolicyNumber
            // 
            this.lblPolicyNumber.AutoSize = true;
            this.lblPolicyNumber.Location = new System.Drawing.Point(12, 499);
            this.lblPolicyNumber.Name = "lblPolicyNumber";
            this.lblPolicyNumber.Size = new System.Drawing.Size(82, 13);
            this.lblPolicyNumber.TabIndex = 11;
            this.lblPolicyNumber.Text = "Copy/Move To:";
            // 
            // cmbPolicy
            // 
            this.cmbPolicy.FormattingEnabled = true;
            this.cmbPolicy.Location = new System.Drawing.Point(98, 423);
            this.cmbPolicy.Name = "cmbPolicy";
            this.cmbPolicy.Size = new System.Drawing.Size(132, 21);
            this.cmbPolicy.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 426);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 18);
            this.label8.TabIndex = 14;
            this.label8.Text = "Copy/Move To:";
            // 
            // aeFQC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 746);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbPolicy);
            this.Controls.Add(this.BoxDtls);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.lblPolicyNumber);
            this.KeyPreview = true;
            this.Name = "aeFQC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "aeFQC";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.AeFQCLoad);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AeFQCKeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.AeFQCKeyUp);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AeFQCFormClosing);
            this.Resize += new System.EventHandler(this.aeFQC_Resize);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.aeFQC_KeyDown);
            this.panel1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureControl)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.type.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.LinkLabel lnkPage1;
		private System.Windows.Forms.LinkLabel lnkPage2;
		private System.Windows.Forms.LinkLabel lnkPage3;
		private System.Windows.Forms.Button cmdCustExcp;
		private System.Windows.Forms.ComboBox cmbDocType;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button cmdFetch;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		
		
		private System.Windows.Forms.RadioButton rdoLIC;
		private System.Windows.Forms.RadioButton rdoAll;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button cmdResolved;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ToolStripMenuItem markReadyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem markNotReadyHoldPolicyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowBatchSummary;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.Button cmdOk;
		private System.Windows.Forms.TextBox txtExceptionType;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtComments;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox2;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		private System.Windows.Forms.ColumnHeader clmCount;
		private System.Windows.Forms.ColumnHeader clmShrtCut;
		private System.Windows.Forms.ColumnHeader clmDockType;
		private System.Windows.Forms.ListView lvwDockTypes;
		private System.Windows.Forms.GroupBox type;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtPolicyNumber;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtName;
		
		
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDOB;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtCommDt;
		private System.Windows.Forms.Button cmdUpdate;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
		private wSelect.BoxDetails BoxDtls;
        //private System.Windows.Forms.Button cmdReScan;
		
		
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
		
		
        private System.Windows.Forms.PictureBox pictureControl;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.Label lblPolicyNumber;
        private System.Windows.Forms.ComboBox cmbPolicy;
		
		
		
		
        private System.Windows.Forms.OpenFileDialog fileDlg;
	}
}
