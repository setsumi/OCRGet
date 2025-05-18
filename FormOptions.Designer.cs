namespace OCRGet
{
    partial class FormOptions
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
            this.lblPrompt = new System.Windows.Forms.Label();
            this.txtHotkeyDisplay = new System.Windows.Forms.TextBox();
            this.chkWinkey = new System.Windows.Forms.CheckBox();
            this.lblCurrentRegistered = new System.Windows.Forms.Label();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.chkProxy = new System.Windows.Forms.CheckBox();
            this.lblProxyHost = new System.Windows.Forms.Label();
            this.txtProxyHost = new System.Windows.Forms.TextBox();
            this.lblProxyPort = new System.Windows.Forms.Label();
            this.udProxyPort = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.udProxyPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(12, 19);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(138, 13);
            this.lblPrompt.TabIndex = 2;
            this.lblPrompt.Text = "Region Snap global hotkey:";
            // 
            // txtHotkeyDisplay
            // 
            this.txtHotkeyDisplay.Location = new System.Drawing.Point(12, 44);
            this.txtHotkeyDisplay.Name = "txtHotkeyDisplay";
            this.txtHotkeyDisplay.ReadOnly = true;
            this.txtHotkeyDisplay.Size = new System.Drawing.Size(252, 20);
            this.txtHotkeyDisplay.TabIndex = 4;
            this.txtHotkeyDisplay.Enter += new System.EventHandler(this.txtHotkeyDisplay_Enter);
            this.txtHotkeyDisplay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtHotkeyDisplay_KeyDown);
            // 
            // chkWinkey
            // 
            this.chkWinkey.AutoSize = true;
            this.chkWinkey.Location = new System.Drawing.Point(279, 46);
            this.chkWinkey.Name = "chkWinkey";
            this.chkWinkey.Size = new System.Drawing.Size(65, 17);
            this.chkWinkey.TabIndex = 5;
            this.chkWinkey.Text = "Win key";
            this.toolTip1.SetToolTip(this.chkWinkey, "Do not press Win key, use this checkbox instead");
            this.chkWinkey.UseVisualStyleBackColor = true;
            this.chkWinkey.CheckedChanged += new System.EventHandler(this.chkWinkey_CheckedChanged);
            // 
            // lblCurrentRegistered
            // 
            this.lblCurrentRegistered.AutoSize = true;
            this.lblCurrentRegistered.Location = new System.Drawing.Point(12, 67);
            this.lblCurrentRegistered.Name = "lblCurrentRegistered";
            this.lblCurrentRegistered.Size = new System.Drawing.Size(51, 13);
            this.lblCurrentRegistered.TabIndex = 6;
            this.lblCurrentRegistered.Text = "Currently:";
            // 
            // btnSet
            // 
            this.btnSet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSet.Location = new System.Drawing.Point(230, 177);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 0;
            this.btnSet.Text = "Set";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(311, 177);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "disable";
            this.toolTip1.SetToolTip(this.button1, "(Backspace)");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkProxy
            // 
            this.chkProxy.AutoSize = true;
            this.chkProxy.Location = new System.Drawing.Point(12, 102);
            this.chkProxy.Name = "chkProxy";
            this.chkProxy.Size = new System.Drawing.Size(105, 17);
            this.chkProxy.TabIndex = 7;
            this.chkProxy.Text = "Use HTTP proxy";
            this.chkProxy.UseVisualStyleBackColor = true;
            this.chkProxy.CheckedChanged += new System.EventHandler(this.chkProxy_CheckedChanged);
            // 
            // lblProxyHost
            // 
            this.lblProxyHost.AutoSize = true;
            this.lblProxyHost.Location = new System.Drawing.Point(9, 132);
            this.lblProxyHost.Name = "lblProxyHost";
            this.lblProxyHost.Size = new System.Drawing.Size(29, 13);
            this.lblProxyHost.TabIndex = 8;
            this.lblProxyHost.Text = "Host";
            // 
            // txtProxyHost
            // 
            this.txtProxyHost.Location = new System.Drawing.Point(44, 129);
            this.txtProxyHost.Name = "txtProxyHost";
            this.txtProxyHost.Size = new System.Drawing.Size(167, 20);
            this.txtProxyHost.TabIndex = 9;
            this.txtProxyHost.TextChanged += new System.EventHandler(this.txtProxyHost_TextChanged);
            // 
            // lblProxyPort
            // 
            this.lblProxyPort.AutoSize = true;
            this.lblProxyPort.Location = new System.Drawing.Point(217, 132);
            this.lblProxyPort.Name = "lblProxyPort";
            this.lblProxyPort.Size = new System.Drawing.Size(26, 13);
            this.lblProxyPort.TabIndex = 10;
            this.lblProxyPort.Text = "Port";
            // 
            // udProxyPort
            // 
            this.udProxyPort.Location = new System.Drawing.Point(249, 130);
            this.udProxyPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.udProxyPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udProxyPort.Name = "udProxyPort";
            this.udProxyPort.Size = new System.Drawing.Size(70, 20);
            this.udProxyPort.TabIndex = 12;
            this.udProxyPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.udProxyPort.ValueChanged += new System.EventHandler(this.udProxyPort_ValueChanged);
            // 
            // FormOptions
            // 
            this.AcceptButton = this.btnSet;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(417, 212);
            this.Controls.Add(this.udProxyPort);
            this.Controls.Add(this.lblProxyPort);
            this.Controls.Add(this.txtProxyHost);
            this.Controls.Add(this.lblProxyHost);
            this.Controls.Add(this.chkProxy);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.lblCurrentRegistered);
            this.Controls.Add(this.chkWinkey);
            this.Controls.Add(this.txtHotkeyDisplay);
            this.Controls.Add(this.lblPrompt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.udProxyPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.TextBox txtHotkeyDisplay;
        private System.Windows.Forms.CheckBox chkWinkey;
        private System.Windows.Forms.Label lblCurrentRegistered;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkProxy;
        private System.Windows.Forms.Label lblProxyHost;
        private System.Windows.Forms.TextBox txtProxyHost;
        private System.Windows.Forms.Label lblProxyPort;
        private System.Windows.Forms.NumericUpDown udProxyPort;
    }
}