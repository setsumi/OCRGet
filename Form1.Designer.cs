﻿namespace OCRGet
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.grpbSettings = new System.Windows.Forms.GroupBox();
            this.chkShowProgress = new System.Windows.Forms.CheckBox();
            this.chkClearCache = new System.Windows.Forms.CheckBox();
            this.udQuality = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkRestore = new System.Windows.Forms.CheckBox();
            this.chkAutorecognize = new System.Windows.Forms.CheckBox();
            this.chkAutocopy = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tmrCopy = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkScale = new System.Windows.Forms.CheckBox();
            this.chkDetectOrientation = new System.Windows.Forms.CheckBox();
            this.rdbEngine3 = new System.Windows.Forms.RadioButton();
            this.rdbEngine2 = new System.Windows.Forms.RadioButton();
            this.rdbEngine1 = new System.Windows.Forms.RadioButton();
            this.chkIsTable = new System.Windows.Forms.CheckBox();
            this.btnInvoke1 = new System.Windows.Forms.Button();
            this.grpbOCR = new System.Windows.Forms.GroupBox();
            this.chkRemoveLinebreaks = new System.Windows.Forms.CheckBox();
            this.tmrStartup = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.grpbSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.udQuality)).BeginInit();
            this.grpbOCR.SuspendLayout();
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
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.BackColor = System.Drawing.SystemColors.Window;
            this.txtResult.Location = new System.Drawing.Point(331, 41);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(287, 97);
            this.txtResult.TabIndex = 3;
            // 
            // btnRecognize
            // 
            this.btnRecognize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 41);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(224, 287);
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
            this.toolTip1.SetToolTip(this.btnRegion, "Region Snap (Ctrl+S)");
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
            this.btnCopy.TabIndex = 5;
            this.btnCopy.Text = "&Copy";
            this.toolTip1.SetToolTip(this.btnCopy, "Copy Recognized Text (Ctrl+C)");
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblStatus.Location = new System.Drawing.Point(174, 18);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(56, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "lblStatus";
            // 
            // cmbLanguage
            // 
            this.cmbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Items.AddRange(new object[] {
            "Arabic",
            "Bulgarian",
            "Chinese(Simplified)",
            "Chinese(Traditional)",
            "Croatian",
            "Czech",
            "Danish",
            "Dutch",
            "English",
            "Finnish",
            "French",
            "German",
            "Greek",
            "Hungarian",
            "Korean",
            "Italian",
            "Japanese",
            "Norwegian",
            "Polish",
            "Portuguese",
            "Russian",
            "Slovenian",
            "Spanish",
            "Swedish",
            "Turkish",
            "Hindi --- Engine 3 ----",
            "Kannada",
            "Persian (Fari)",
            "Telugu",
            "Tamil",
            "Thai",
            "Vietnamese"});
            this.cmbLanguage.Location = new System.Drawing.Point(406, 11);
            this.cmbLanguage.Name = "cmbLanguage";
            this.cmbLanguage.Size = new System.Drawing.Size(131, 21);
            this.cmbLanguage.TabIndex = 4;
            // 
            // grpbSettings
            // 
            this.grpbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.grpbSettings.Controls.Add(this.chkShowProgress);
            this.grpbSettings.Controls.Add(this.chkClearCache);
            this.grpbSettings.Controls.Add(this.udQuality);
            this.grpbSettings.Controls.Add(this.label1);
            this.grpbSettings.Controls.Add(this.chkRestore);
            this.grpbSettings.Controls.Add(this.chkAutorecognize);
            this.grpbSettings.Controls.Add(this.chkAutocopy);
            this.grpbSettings.Location = new System.Drawing.Point(245, 231);
            this.grpbSettings.Name = "grpbSettings";
            this.grpbSettings.Size = new System.Drawing.Size(373, 97);
            this.grpbSettings.TabIndex = 6;
            this.grpbSettings.TabStop = false;
            this.grpbSettings.Text = "Program";
            // 
            // chkShowProgress
            // 
            this.chkShowProgress.AutoSize = true;
            this.chkShowProgress.Location = new System.Drawing.Point(231, 65);
            this.chkShowProgress.Name = "chkShowProgress";
            this.chkShowProgress.Size = new System.Drawing.Size(96, 17);
            this.chkShowProgress.TabIndex = 6;
            this.chkShowProgress.Text = "Show progress";
            this.toolTip1.SetToolTip(this.chkShowProgress, "Show pop-up indicator while web request is in progress ");
            this.chkShowProgress.UseVisualStyleBackColor = true;
            // 
            // chkClearCache
            // 
            this.chkClearCache.AutoSize = true;
            this.chkClearCache.Location = new System.Drawing.Point(231, 42);
            this.chkClearCache.Name = "chkClearCache";
            this.chkClearCache.Size = new System.Drawing.Size(83, 17);
            this.chkClearCache.TabIndex = 5;
            this.chkClearCache.Text = "Clear cache";
            this.toolTip1.SetToolTip(this.chkClearCache, "Delete region snap files when program starts");
            this.chkClearCache.UseVisualStyleBackColor = true;
            // 
            // udQuality
            // 
            this.udQuality.BackColor = System.Drawing.SystemColors.Window;
            this.udQuality.Location = new System.Drawing.Point(311, 18);
            this.udQuality.Name = "udQuality";
            this.udQuality.Size = new System.Drawing.Size(56, 20);
            this.udQuality.TabIndex = 4;
            this.udQuality.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(228, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "JPEG quality";
            // 
            // chkRestore
            // 
            this.chkRestore.AutoSize = true;
            this.chkRestore.Location = new System.Drawing.Point(23, 65);
            this.chkRestore.Name = "chkRestore";
            this.chkRestore.Size = new System.Drawing.Size(166, 17);
            this.chkRestore.TabIndex = 2;
            this.chkRestore.Text = "Restore app after region snap";
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
            this.chkAutocopy.Size = new System.Drawing.Size(107, 17);
            this.chkAutocopy.TabIndex = 1;
            this.chkAutocopy.Text = "Auto copy results";
            this.chkAutocopy.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(505, 318);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(112, 13);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "powered by ocr.space";
            this.toolTip1.SetToolTip(this.linkLabel1, "Show API status");
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // tmrCopy
            // 
            this.tmrCopy.Tick += new System.EventHandler(this.tmrCopy_Tick);
            // 
            // chkScale
            // 
            this.chkScale.AutoSize = true;
            this.chkScale.Location = new System.Drawing.Point(22, 42);
            this.chkScale.Name = "chkScale";
            this.chkScale.Size = new System.Drawing.Size(53, 17);
            this.chkScale.TabIndex = 4;
            this.chkScale.Text = "Scale";
            this.toolTip1.SetToolTip(this.chkScale, "If set to true, the api does some internal upscaling.\r\nThis can improve the OCR r" +
        "esult significantly,\r\nespecially for low-resolution PDF scans.");
            this.chkScale.UseVisualStyleBackColor = true;
            // 
            // chkDetectOrientation
            // 
            this.chkDetectOrientation.AutoSize = true;
            this.chkDetectOrientation.Location = new System.Drawing.Point(22, 19);
            this.chkDetectOrientation.Name = "chkDetectOrientation";
            this.chkDetectOrientation.Size = new System.Drawing.Size(110, 17);
            this.chkDetectOrientation.TabIndex = 3;
            this.chkDetectOrientation.Text = "Detect orientation";
            this.toolTip1.SetToolTip(this.chkDetectOrientation, resources.GetString("chkDetectOrientation.ToolTip"));
            this.chkDetectOrientation.UseVisualStyleBackColor = true;
            // 
            // rdbEngine3
            // 
            this.rdbEngine3.AutoSize = true;
            this.rdbEngine3.Location = new System.Drawing.Point(288, 56);
            this.rdbEngine3.Name = "rdbEngine3";
            this.rdbEngine3.Size = new System.Drawing.Size(67, 17);
            this.rdbEngine3.TabIndex = 2;
            this.rdbEngine3.Text = "Engine 3";
            this.toolTip1.SetToolTip(this.rdbEngine3, resources.GetString("rdbEngine3.ToolTip"));
            this.rdbEngine3.UseVisualStyleBackColor = true;
            // 
            // rdbEngine2
            // 
            this.rdbEngine2.AutoSize = true;
            this.rdbEngine2.Location = new System.Drawing.Point(288, 33);
            this.rdbEngine2.Name = "rdbEngine2";
            this.rdbEngine2.Size = new System.Drawing.Size(67, 17);
            this.rdbEngine2.TabIndex = 1;
            this.rdbEngine2.Text = "Engine 2";
            this.toolTip1.SetToolTip(this.rdbEngine2, resources.GetString("rdbEngine2.ToolTip"));
            this.rdbEngine2.UseVisualStyleBackColor = true;
            // 
            // rdbEngine1
            // 
            this.rdbEngine1.AutoSize = true;
            this.rdbEngine1.Checked = true;
            this.rdbEngine1.Location = new System.Drawing.Point(288, 10);
            this.rdbEngine1.Name = "rdbEngine1";
            this.rdbEngine1.Size = new System.Drawing.Size(67, 17);
            this.rdbEngine1.TabIndex = 0;
            this.rdbEngine1.TabStop = true;
            this.rdbEngine1.Text = "Engine 1";
            this.toolTip1.SetToolTip(this.rdbEngine1, resources.GetString("rdbEngine1.ToolTip"));
            this.rdbEngine1.UseVisualStyleBackColor = true;
            // 
            // chkIsTable
            // 
            this.chkIsTable.AutoSize = true;
            this.chkIsTable.Location = new System.Drawing.Point(138, 42);
            this.chkIsTable.Name = "chkIsTable";
            this.chkIsTable.Size = new System.Drawing.Size(63, 17);
            this.chkIsTable.TabIndex = 6;
            this.chkIsTable.Text = "is Table";
            this.toolTip1.SetToolTip(this.chkIsTable, resources.GetString("chkIsTable.ToolTip"));
            this.chkIsTable.UseVisualStyleBackColor = true;
            // 
            // btnInvoke1
            // 
            this.btnInvoke1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInvoke1.Location = new System.Drawing.Point(21, 292);
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
            this.grpbOCR.Controls.Add(this.chkIsTable);
            this.grpbOCR.Controls.Add(this.chkRemoveLinebreaks);
            this.grpbOCR.Controls.Add(this.chkScale);
            this.grpbOCR.Controls.Add(this.chkDetectOrientation);
            this.grpbOCR.Controls.Add(this.rdbEngine3);
            this.grpbOCR.Controls.Add(this.rdbEngine2);
            this.grpbOCR.Controls.Add(this.rdbEngine1);
            this.grpbOCR.Location = new System.Drawing.Point(245, 144);
            this.grpbOCR.Name = "grpbOCR";
            this.grpbOCR.Size = new System.Drawing.Size(373, 81);
            this.grpbOCR.TabIndex = 10;
            this.grpbOCR.TabStop = false;
            this.grpbOCR.Text = "OCR";
            // 
            // chkRemoveLinebreaks
            // 
            this.chkRemoveLinebreaks.AutoSize = true;
            this.chkRemoveLinebreaks.Location = new System.Drawing.Point(138, 19);
            this.chkRemoveLinebreaks.Name = "chkRemoveLinebreaks";
            this.chkRemoveLinebreaks.Size = new System.Drawing.Size(117, 17);
            this.chkRemoveLinebreaks.TabIndex = 5;
            this.chkRemoveLinebreaks.Text = "Remove linebreaks";
            this.chkRemoveLinebreaks.UseVisualStyleBackColor = true;
            // 
            // tmrStartup
            // 
            this.tmrStartup.Enabled = true;
            this.tmrStartup.Tick += new System.EventHandler(this.tmrStartup_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 319);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Esc - minimize";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 340);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grpbOCR);
            this.Controls.Add(this.btnInvoke1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnRegion);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnRecognize);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.grpbSettings);
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
            this.grpbOCR.ResumeLayout(false);
            this.grpbOCR.PerformLayout();
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
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbLanguage;
        private System.Windows.Forms.GroupBox grpbSettings;
        private System.Windows.Forms.CheckBox chkRestore;
        private System.Windows.Forms.CheckBox chkAutorecognize;
        private System.Windows.Forms.CheckBox chkAutocopy;
        private System.Windows.Forms.NumericUpDown udQuality;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkClearCache;
        private System.Windows.Forms.LinkLabel linkLabel1;
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
        private System.Windows.Forms.CheckBox chkRemoveLinebreaks;
        private System.Windows.Forms.CheckBox chkIsTable;
        private System.Windows.Forms.Timer tmrStartup;
        private System.Windows.Forms.Label label2;
    }
}

