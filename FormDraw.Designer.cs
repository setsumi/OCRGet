namespace OCRGet
{
    partial class FormDraw
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDraw));
            this.SuspendLayout();
            // 
            // FormDraw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDraw";
            this.Text = "OCRGet Region Snap Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDraw_FormClosing);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FormDraw_KeyPress);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDraw_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormDraw_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FormDraw_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion


    }
}