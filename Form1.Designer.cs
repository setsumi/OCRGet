namespace OCRGet
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnOpen = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.btnRecognize = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnRegion = new System.Windows.Forms.Button();
            this.tmrSnap = new System.Windows.Forms.Timer(this.components);
            this.btnCopy = new System.Windows.Forms.Button();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.grpbSettings = new System.Windows.Forms.GroupBox();
            this.chkRemoveSpaces = new System.Windows.Forms.CheckBox();
            this.chkRemoveLinebreaks = new System.Windows.Forms.CheckBox();
            this.chkShowProgress = new System.Windows.Forms.CheckBox();
            this.chkClearCache = new System.Windows.Forms.CheckBox();
            this.udQuality = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkRestore = new System.Windows.Forms.CheckBox();
            this.chkAutorecognize = new System.Windows.Forms.CheckBox();
            this.chkAutocopy = new System.Windows.Forms.CheckBox();
            this.tmrCopy = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkScale = new System.Windows.Forms.CheckBox();
            this.chkDetectOrientation = new System.Windows.Forms.CheckBox();
            this.rdbEngine3 = new System.Windows.Forms.RadioButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.rdbEngine2 = new System.Windows.Forms.RadioButton();
            this.rdbEngine1 = new System.Windows.Forms.RadioButton();
            this.chkIsTable = new System.Windows.Forms.CheckBox();
            this.rdbOCR1 = new System.Windows.Forms.RadioButton();
            this.rdbOCR2 = new System.Windows.Forms.RadioButton();
            this.btnInvoke1 = new System.Windows.Forms.Button();
            this.grpbOCR = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tmrStartup = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chkScaleFactor = new System.Windows.Forms.CheckBox();
            this.nudScaleFactor = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpbSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udQuality)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.grpbOCR.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScaleFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(12, 11);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "&Open...";
            this.toolTip1.SetToolTip(this.btnOpen, "Open Image (Ctrl+O)");
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.BackColor = System.Drawing.SystemColors.Window;
            this.txtResult.Location = new System.Drawing.Point(331, 41);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(287, 97);
            this.txtResult.TabIndex = 5;
            // 
            // btnRecognize
            // 
            this.btnRecognize.Location = new System.Drawing.Point(245, 73);
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.Size = new System.Drawing.Size(75, 30);
            this.btnRecognize.TabIndex = 2;
            this.btnRecognize.Text = "&Recognize";
            this.toolTip1.SetToolTip(this.btnRecognize, "(Ctrl+R)");
            this.btnRecognize.UseVisualStyleBackColor = true;
            this.btnRecognize.Click += new System.EventHandler(this.btnRecognize_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(224, 307);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // btnRegion
            // 
            this.btnRegion.Location = new System.Drawing.Point(93, 11);
            this.btnRegion.Name = "btnRegion";
            this.btnRegion.Size = new System.Drawing.Size(75, 23);
            this.btnRegion.TabIndex = 1;
            this.btnRegion.Text = "Region...(&S)";
            this.toolTip1.SetToolTip(this.btnRegion, "Region Snap (Ctrl+S), global (Ctrl+Alt+S)");
            this.btnRegion.UseVisualStyleBackColor = true;
            this.btnRegion.Click += new System.EventHandler(this.btnRegion_Click);
            // 
            // tmrSnap
            // 
            this.tmrSnap.Interval = 50;
            this.tmrSnap.Tick += new System.EventHandler(this.tmrSnap_Tick);
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(543, 11);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 4;
            this.btnCopy.Text = "&Copy";
            this.toolTip1.SetToolTip(this.btnCopy, "Copy Recognized Text (Ctrl+C)");
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmbLanguage.Location = new System.Drawing.Point(406, 11);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(131, 21);
            this.cmbLanguage.TabIndex = 3;
            // 
            // grpbSettings
            // 
            this.grpbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbSettings.Controls.Add(this.nudScaleFactor);
            this.grpbSettings.Controls.Add(this.chkScaleFactor);
            this.grpbSettings.Controls.Add(this.chkRemoveSpaces);
            this.grpbSettings.Controls.Add(this.chkRemoveLinebreaks);
            this.grpbSettings.Controls.Add(this.chkShowProgress);
            this.grpbSettings.Controls.Add(this.chkClearCache);
            this.grpbSettings.Controls.Add(this.udQuality);
            this.grpbSettings.Controls.Add(this.label1);
            this.grpbSettings.Controls.Add(this.chkRestore);
            this.grpbSettings.Controls.Add(this.chkAutorecognize);
            this.grpbSettings.Controls.Add(this.chkAutocopy);
            this.grpbSettings.Location = new System.Drawing.Point(245, 236);
            this.grpbSettings.Name = "grpbSettings";
            this.grpbSettings.Size = new System.Drawing.Size(373, 112);
            this.grpbSettings.TabIndex = 8;
            this.grpbSettings.TabStop = false;
            this.grpbSettings.Text = "Program settings";
            // 
            // chkRemoveSpaces
            // 
            this.chkRemoveSpaces.AutoSize = true;
            this.chkRemoveSpaces.Location = new System.Drawing.Point(146, 88);
            this.chkRemoveSpaces.Name = "chkRemoveSpaces";
            this.chkRemoveSpaces.Size = new System.Drawing.Size(103, 17);
            this.chkRemoveSpaces.TabIndex = 4;
            this.chkRemoveSpaces.Text = "Remove spaces";
            this.chkRemoveSpaces.UseVisualStyleBackColor = true;
            this.chkRemoveSpaces.CheckedChanged += new System.EventHandler(this.chkRemoveLinebreaks_CheckedChanged);
            // 
            // chkRemoveLinebreaks
            // 
            this.chkRemoveLinebreaks.AutoSize = true;
            this.chkRemoveLinebreaks.Location = new System.Drawing.Point(23, 88);
            this.chkRemoveLinebreaks.Name = "chkRemoveLinebreaks";
            this.chkRemoveLinebreaks.Size = new System.Drawing.Size(117, 17);
            this.chkRemoveLinebreaks.TabIndex = 3;
            this.chkRemoveLinebreaks.Text = "Remove linebreaks";
            this.chkRemoveLinebreaks.UseVisualStyleBackColor = true;
            this.chkRemoveLinebreaks.CheckedChanged += new System.EventHandler(this.chkRemoveLinebreaks_CheckedChanged);
            // 
            // chkShowProgress
            // 
            this.chkShowProgress.AutoSize = true;
            this.chkShowProgress.Location = new System.Drawing.Point(271, 88);
            this.chkShowProgress.Name = "chkShowProgress";
            this.chkShowProgress.Size = new System.Drawing.Size(96, 17);
            this.chkShowProgress.TabIndex = 10;
            this.chkShowProgress.Text = "Show progress";
            this.toolTip1.SetToolTip(this.chkShowProgress, "Show pop-up indicator while web request is in progress.\r\nNote: Deactivates \"Resto" +
        "re app after region snap\" option.");
            this.chkShowProgress.UseVisualStyleBackColor = true;
            // 
            // chkClearCache
            // 
            this.chkClearCache.AutoSize = true;
            this.chkClearCache.Location = new System.Drawing.Point(271, 65);
            this.chkClearCache.Name = "chkClearCache";
            this.chkClearCache.Size = new System.Drawing.Size(83, 17);
            this.chkClearCache.TabIndex = 9;
            this.chkClearCache.Text = "Clear cache";
            this.toolTip1.SetToolTip(this.chkClearCache, "Delete region snap files when program starts");
            this.chkClearCache.UseVisualStyleBackColor = true;
            // 
            // udQuality
            // 
            this.udQuality.BackColor = System.Drawing.SystemColors.Window;
            this.udQuality.Location = new System.Drawing.Point(308, 40);
            this.udQuality.Name = "udQuality";
            this.udQuality.Size = new System.Drawing.Size(56, 20);
            this.udQuality.TabIndex = 8;
            this.udQuality.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(225, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "JPEG quality";
            this.toolTip1.SetToolTip(this.label1, "Quality of snapped images saved to cache");
            // 
            // chkRestore
            // 
            this.chkRestore.AutoSize = true;
            this.chkRestore.Location = new System.Drawing.Point(23, 65);
            this.chkRestore.Name = "chkRestore";
            this.chkRestore.Size = new System.Drawing.Size(166, 17);
            this.chkRestore.TabIndex = 2;
            this.chkRestore.Text = "Restore app after region snap";
            this.toolTip1.SetToolTip(this.chkRestore, "Have no effect when \"Show progress\" is used");
            this.chkRestore.UseVisualStyleBackColor = true;
            // 
            // chkAutorecognize
            // 
            this.chkAutorecognize.AutoSize = true;
            this.chkAutorecognize.Location = new System.Drawing.Point(23, 19);
            this.chkAutorecognize.Name = "chkAutorecognize";
            this.chkAutorecognize.Size = new System.Drawing.Size(162, 17);
            this.chkAutorecognize.TabIndex = 0;
            this.chkAutorecognize.Text = "Auto recognize after opening";
            this.chkAutorecognize.UseVisualStyleBackColor = true;
            // 
            // chkAutocopy
            // 
            this.chkAutocopy.AutoSize = true;
            this.chkAutocopy.Location = new System.Drawing.Point(23, 42);
            this.chkAutocopy.Name = "chkAutocopy";
            this.chkAutocopy.Size = new System.Drawing.Size(136, 17);
            this.chkAutocopy.TabIndex = 1;
            this.chkAutocopy.Text = "Auto copy resulting text";
            this.chkAutocopy.UseVisualStyleBackColor = true;
            // 
            // tmrCopy
            // 
            this.tmrCopy.Tick += new System.EventHandler(this.tmrCopy_Tick);
            // 
            // chkScale
            // 
            this.chkScale.AutoSize = true;
            this.chkScale.Location = new System.Drawing.Point(10, 38);
            this.chkScale.Name = "chkScale";
            this.chkScale.Size = new System.Drawing.Size(53, 17);
            this.chkScale.TabIndex = 2;
            this.chkScale.Text = "Scale";
            this.toolTip1.SetToolTip(this.chkScale, "If set to true, the api does some internal upscaling.\r\nThis can improve the OCR r" +
        "esult significantly,\r\nespecially for low-resolution PDF scans.");
            this.chkScale.UseVisualStyleBackColor = true;
            // 
            // chkDetectOrientation
            // 
            this.chkDetectOrientation.AutoSize = true;
            this.chkDetectOrientation.Location = new System.Drawing.Point(10, 15);
            this.chkDetectOrientation.Name = "chkDetectOrientation";
            this.chkDetectOrientation.Size = new System.Drawing.Size(110, 17);
            this.chkDetectOrientation.TabIndex = 1;
            this.chkDetectOrientation.Text = "Detect orientation";
            this.toolTip1.SetToolTip(this.chkDetectOrientation, resources.GetString("chkDetectOrientation.ToolTip"));
            this.chkDetectOrientation.UseVisualStyleBackColor = true;
            // 
            // rdbEngine3
            // 
            this.rdbEngine3.AutoSize = true;
            this.rdbEngine3.ContextMenuStrip = this.contextMenuStrip1;
            this.rdbEngine3.Location = new System.Drawing.Point(132, 60);
            this.rdbEngine3.Name = "rdbEngine3";
            this.rdbEngine3.Size = new System.Drawing.Size(67, 17);
            this.rdbEngine3.TabIndex = 6;
            this.rdbEngine3.Tag = "2";
            this.rdbEngine3.Text = "Engine 3";
            this.toolTip1.SetToolTip(this.rdbEngine3, resources.GetString("rdbEngine3.ToolTip"));
            this.rdbEngine3.UseVisualStyleBackColor = true;
            this.rdbEngine3.CheckedChanged += new System.EventHandler(this.rdbEngine1_CheckedChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem2.Text = "toolStripMenuItem2";
            // 
            // rdbEngine2
            // 
            this.rdbEngine2.AutoSize = true;
            this.rdbEngine2.ContextMenuStrip = this.contextMenuStrip1;
            this.rdbEngine2.Location = new System.Drawing.Point(132, 37);
            this.rdbEngine2.Name = "rdbEngine2";
            this.rdbEngine2.Size = new System.Drawing.Size(67, 17);
            this.rdbEngine2.TabIndex = 5;
            this.rdbEngine2.Tag = "1";
            this.rdbEngine2.Text = "Engine 2";
            this.toolTip1.SetToolTip(this.rdbEngine2, resources.GetString("rdbEngine2.ToolTip"));
            this.rdbEngine2.UseVisualStyleBackColor = true;
            this.rdbEngine2.CheckedChanged += new System.EventHandler(this.rdbEngine1_CheckedChanged);
            // 
            // rdbEngine1
            // 
            this.rdbEngine1.AutoSize = true;
            this.rdbEngine1.ContextMenuStrip = this.contextMenuStrip1;
            this.rdbEngine1.Location = new System.Drawing.Point(132, 14);
            this.rdbEngine1.Name = "rdbEngine1";
            this.rdbEngine1.Size = new System.Drawing.Size(67, 17);
            this.rdbEngine1.TabIndex = 4;
            this.rdbEngine1.Tag = "0";
            this.rdbEngine1.Text = "Engine 1";
            this.toolTip1.SetToolTip(this.rdbEngine1, resources.GetString("rdbEngine1.ToolTip"));
            this.rdbEngine1.UseVisualStyleBackColor = true;
            this.rdbEngine1.CheckedChanged += new System.EventHandler(this.rdbEngine1_CheckedChanged);
            // 
            // chkIsTable
            // 
            this.chkIsTable.AutoSize = true;
            this.chkIsTable.Location = new System.Drawing.Point(10, 61);
            this.chkIsTable.Name = "chkIsTable";
            this.chkIsTable.Size = new System.Drawing.Size(63, 17);
            this.chkIsTable.TabIndex = 3;
            this.chkIsTable.Text = "is Table";
            this.toolTip1.SetToolTip(this.chkIsTable, resources.GetString("chkIsTable.ToolTip"));
            this.chkIsTable.UseVisualStyleBackColor = true;
            // 
            // rdbOCR1
            // 
            this.rdbOCR1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbOCR1.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdbOCR1.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rdbOCR1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdbOCR1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbOCR1.Location = new System.Drawing.Point(473, 145);
            this.rdbOCR1.Name = "rdbOCR1";
            this.rdbOCR1.Size = new System.Drawing.Size(24, 18);
            this.rdbOCR1.TabIndex = 7;
            this.rdbOCR1.TabStop = true;
            this.rdbOCR1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.rdbOCR1, "Activate");
            this.rdbOCR1.UseVisualStyleBackColor = true;
            // 
            // rdbOCR2
            // 
            this.rdbOCR2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbOCR2.Appearance = System.Windows.Forms.Appearance.Button;
            this.rdbOCR2.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rdbOCR2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rdbOCR2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.rdbOCR2.Location = new System.Drawing.Point(337, 145);
            this.rdbOCR2.Name = "rdbOCR2";
            this.rdbOCR2.Size = new System.Drawing.Size(24, 18);
            this.rdbOCR2.TabIndex = 6;
            this.rdbOCR2.TabStop = true;
            this.rdbOCR2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.rdbOCR2, "Activate");
            this.rdbOCR2.UseVisualStyleBackColor = true;
            // 
            // btnInvoke1
            // 
            this.btnInvoke1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInvoke1.Location = new System.Drawing.Point(32, 93);
            this.btnInvoke1.Name = "btnInvoke1";
            this.btnInvoke1.Size = new System.Drawing.Size(55, 23);
            this.btnInvoke1.TabIndex = 9;
            this.btnInvoke1.Text = "invoke1";
            this.btnInvoke1.UseVisualStyleBackColor = true;
            this.btnInvoke1.Visible = false;
            this.btnInvoke1.Click += new System.EventHandler(this.btnInvoke1_Click);
            // 
            // grpbOCR
            // 
            this.grpbOCR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbOCR.Controls.Add(this.linkLabel1);
            this.grpbOCR.Controls.Add(this.chkIsTable);
            this.grpbOCR.Controls.Add(this.chkScale);
            this.grpbOCR.Controls.Add(this.chkDetectOrientation);
            this.grpbOCR.Controls.Add(this.rdbEngine3);
            this.grpbOCR.Controls.Add(this.rdbEngine2);
            this.grpbOCR.Controls.Add(this.rdbEngine1);
            this.grpbOCR.Location = new System.Drawing.Point(406, 148);
            this.grpbOCR.Name = "grpbOCR";
            this.grpbOCR.Size = new System.Drawing.Size(212, 86);
            this.grpbOCR.TabIndex = 7;
            this.grpbOCR.TabStop = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(7, -1);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(54, 13);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "ocr.space";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // tmrStartup
            // 
            this.tmrStartup.Enabled = true;
            this.tmrStartup.Tick += new System.EventHandler(this.tmrStartup_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 355);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.ShowItemToolTips = true;
            this.statusStrip1.Size = new System.Drawing.Size(630, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(127, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(394, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolStripStatusLabel2.ToolTipText = "1234";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel4.Text = "|";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(84, 17);
            this.toolStripStatusLabel3.Text = "Esc - minimize";
            this.toolStripStatusLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(245, 148);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(145, 86);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "WIndows OCR";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Information";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkScaleFactor
            // 
            this.chkScaleFactor.AutoSize = true;
            this.chkScaleFactor.Location = new System.Drawing.Point(212, 19);
            this.chkScaleFactor.Name = "chkScaleFactor";
            this.chkScaleFactor.Size = new System.Drawing.Size(83, 17);
            this.chkScaleFactor.TabIndex = 5;
            this.chkScaleFactor.Text = "Scale factor";
            this.toolTip1.SetToolTip(this.chkScaleFactor, "Scale snapped image, which improves OCR results");
            this.chkScaleFactor.UseVisualStyleBackColor = true;
            // 
            // nudScaleFactor
            // 
            this.nudScaleFactor.DecimalPlaces = 1;
            this.nudScaleFactor.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudScaleFactor.Location = new System.Drawing.Point(308, 17);
            this.nudScaleFactor.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudScaleFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudScaleFactor.Name = "nudScaleFactor";
            this.nudScaleFactor.Size = new System.Drawing.Size(56, 20);
            this.nudScaleFactor.TabIndex = 6;
            this.nudScaleFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 377);
            this.Controls.Add(this.btnRegion);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.rdbOCR1);
            this.Controls.Add(this.rdbOCR2);
            this.Controls.Add(this.btnRecognize);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.grpbOCR);
            this.Controls.Add(this.btnInvoke1);
            this.Controls.Add(this.grpbSettings);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.txtResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "OCRGet";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.grpbSettings.ResumeLayout(false);
            this.grpbSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udQuality)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.grpbOCR.ResumeLayout(false);
            this.grpbOCR.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudScaleFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnRecognize;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnRegion;
        private System.Windows.Forms.Timer tmrSnap;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.GroupBox grpbSettings;
        private System.Windows.Forms.CheckBox chkRestore;
        private System.Windows.Forms.CheckBox chkAutorecognize;
        private System.Windows.Forms.CheckBox chkAutocopy;
        private System.Windows.Forms.NumericUpDown udQuality;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkClearCache;
        private System.Windows.Forms.Timer tmrCopy;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnInvoke1;
        private System.Windows.Forms.CheckBox chkShowProgress;
        private System.Windows.Forms.GroupBox grpbOCR;
        private System.Windows.Forms.CheckBox chkScale;
        private System.Windows.Forms.CheckBox chkDetectOrientation;
        private System.Windows.Forms.RadioButton rdbEngine3;
        private System.Windows.Forms.RadioButton rdbEngine2;
        private System.Windows.Forms.RadioButton rdbEngine1;
        private System.Windows.Forms.CheckBox chkIsTable;
        private System.Windows.Forms.Timer tmrStartup;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.CheckBox chkRemoveLinebreaks;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton rdbOCR1;
        private System.Windows.Forms.RadioButton rdbOCR2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.CheckBox chkRemoveSpaces;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.NumericUpDown nudScaleFactor;
        private System.Windows.Forms.CheckBox chkScaleFactor;
    }
}

