using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using IniParser;
using IniParser.Model;
using System.Threading;

namespace OCRGet
{


    public partial class Form1 : Form
    {

        private FormDraw formd1 = null;
        private FormDraw formn1 = null;
        private string imagepath { get; set; }
        private string imagesize { get; set; }

        private string apiurl { get; set; }
        private string apikey { get; set; }
        private string useragent { get; set; }

        private string language { get; set; }
        private string resulttext { get; set; }

        private FileIniDataParser config;
        private IniData inidata;
        private String inifile;

        public Form1()
        {
            InitializeComponent();

            lblStatus.Text = "";
            linkLabel1.Links.Add(11, linkLabel1.Text.Length - 11, "https://status.ocr.space/");

            LoadConfig();

            if (chkClearCache.Checked)
            {
                foreach (string f in Directory.EnumerateFiles(getCacheDir(), "*.jpg"))
                {
                    File.Delete(f);
                }
            }

        }

        private void LoadConfig()
        {
            config = new FileIniDataParser();
            inifile = Path.ChangeExtension(Application.ExecutablePath, ".ini");
            inidata = config.ReadFile(inifile);

            apiurl = inidata["general"]["apiurl"];
            apikey = inidata["general"]["apikey"];
            useragent = inidata["general"]["useragent"];
            cmbLanguage.SelectedIndex = int.Parse(inidata["general"]["language"]);
            chkAutocopy.Checked = bool.Parse(inidata["general"]["autocopy"]);
            chkAutorecognize.Checked = bool.Parse(inidata["general"]["autorecognize"]);
            chkRestore.Checked = bool.Parse(inidata["general"]["restoreapp"]);
            udQuality.Value = decimal.Parse(inidata["general"]["jpegquality"]);
            chkClearCache.Checked = bool.Parse(inidata["general"]["clearcache"]);
            chkShowProgress.Checked = bool.Parse(inidata["general"]["showprogress"]);

            //inidata.Sections.AddSection("general");
            //inidata["general"].AddKey("apiurl", "http://api.ocr.space/Parse/Image");
            //inidata["general"].AddKey("apikey", "helloworld");
            //inidata["general"].AddKey("useragent", "OCRGet");
            //inidata["general"].AddKey("language", "16");
            //inidata["general"].AddKey("autocopy", "true");
            //SaveConfig();
        }

        private void SaveConfig()
        {
            inidata["general"]["language"] = cmbLanguage.SelectedIndex.ToString();
            inidata["general"]["autocopy"] = chkAutocopy.Checked.ToString();
            inidata["general"]["autorecognize"] = chkAutorecognize.Checked.ToString();
            inidata["general"]["restoreapp"] = chkRestore.Checked.ToString();
            inidata["general"]["jpegquality"] = udQuality.Value.ToString();
            inidata["general"]["clearcache"] = chkClearCache.Checked.ToString();
            inidata["general"]["showprogress"] = chkShowProgress.Checked.ToString();
            config.WriteFile(inifile, inidata);
        }

        private void OpenFile(string file)
        {
            imagepath = file;
            pictureBox1.Image = Image.FromFile(imagepath);
            FileInfo fileInfo = new FileInfo(imagepath);
            this.Text = "OCRGet - " + fileInfo.Name;
            imagesize = ((float)fileInfo.Length / 1024f / 1024f).ToString("0.00") + " MB";
            lblStatus.Text = imagesize;
            lblStatus.ForeColor = SystemColors.ControlText;
            lblStatus.BackColor = Color.LightGreen;

            if (chkAutorecognize.Checked)
                Recognize();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "JPEG files(*.jpg;*.jpeg)|*.jpg;*.jpeg";
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                OpenFile(fileDlg.FileName);
            }
        }

        void formn1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString("Requesting OCR responce...", new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), Brushes.Lime, 0, 0);
        }

        private void Recognize()
        {
            if (chkShowProgress.Checked)
            {
                formn1 = new FormDraw();
                formn1.Opacity = 1.0;
                formn1.BackColor = Color.Black;
                formn1.Width = 200;
                formn1.Height = 20;
                formn1.Left = 0;
                formn1.Top = Screen.PrimaryScreen.Bounds.Height - formn1.Height;
                formn1.Paint += new PaintEventHandler(formn1_Paint);
                formn1.Show();
            }

            txtResult.Text = "";
            resulttext = "";
            language = getLanguage();

            btnOpen.Enabled = false;
            btnRegion.Enabled = false;
            btnRecognize.Enabled = false;
            lblStatus.Text = "Contacting " + apiurl + " ...";
            lblStatus.ForeColor = SystemColors.ControlText;
            lblStatus.BackColor = Color.Yellow;

            Thread t = new Thread(new ParameterizedThreadStart(RecognizeThread));
            t.Start(this);
        }

        private static void RecognizeThread(object o)
        {
            Form1 form = (Form1)o;

            // Read file data
            FileStream fs = new FileStream(form.imagepath, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("apikey", form.apikey);
            postParameters.Add("language", form.language);
            postParameters.Add("file", new FormUpload.FileParameter(data, "image.jpg", "image"));

            // Create request and receive response
            HttpWebResponse webResponse;
            try
            {
                webResponse = FormUpload.MultipartFormDataPost(form.apiurl, form.useragent, postParameters);
            }
            catch (Exception e)
            {
                form.resulttext = "ERROR: " + e.Message;
                goto RecognizeEend;
            }

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();

            try
            {
                Rootobject ocrResult = JsonConvert.DeserializeObject<Rootobject>(fullResponse);
                if (ocrResult.OCRExitCode == 1)
                {
                    for (int i = 0; i < ocrResult.ParsedResults.Count(); i++)
                    {
                        form.resulttext = form.resulttext + ocrResult.ParsedResults[i].ParsedText;
                    }
                }
                else
                {
                    form.resulttext = "ERROR: " + fullResponse;
                }
            }
            catch
            {
                form.resulttext = "ERROR: " + fullResponse;
            }

        RecognizeEend:
            form.btnInvoke1_Click(form, null);
        }

        private void btnInvoke1_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
                BeginInvoke(new MethodInvoker(RecognizeFinish));
        }
        private void RecognizeFinish()
        {
            if (formn1 != null)
            {
                formn1.Hide();
                formn1.Dispose();
                formn1 = null;
            }

            txtResult.Text = resulttext;
            btnOpen.Enabled = true;
            btnRegion.Enabled = true;
            btnRecognize.Enabled = true;
            lblStatus.Text = imagesize + " Done";
            lblStatus.ForeColor = SystemColors.ControlText;
            lblStatus.BackColor = Color.LightGreen;

            if (chkAutocopy.Checked)
            {
                btnCopy_Click(this, null);
            }
        }

        private void btnRecognize_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(imagepath)) return;
            Recognize();
        }

        private void btnRegion_Click(object sender, EventArgs e)
        {
            formd1 = new FormDraw();
            formd1.VisibleChanged += new EventHandler(this.FormDrawOnVisible);
            formd1.Show();
            this.WindowState = FormWindowState.Minimized;
            formd1.Activate();
        }

        private void FormDrawOnVisible(object sender, EventArgs e)
        {
            if (!formd1.Visible)
            {
                tmrSnap.Enabled = true;
            }
        }

        private void tmrSnap_Tick(object sender, EventArgs e)
        {
            tmrSnap.Enabled = false;
            Rectangle rect = new Rectangle(formd1.area.X, formd1.area.Y, formd1.area.Width, formd1.area.Height);
            if (rect.Width > 10 && rect.Height > 10)
            {
                using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb))
                {
                    Graphics g = Graphics.FromImage(bmp);
                    g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                    imagepath = getCacheDir() + DateTime.Now.ToString("yyyyMMdd-HHmmss-fff") + ".jpg";
                    Jpeg.Save(bmp, imagepath, (long)udQuality.Value);
                    g.Dispose();
                }
                OpenFile(imagepath);
            }
            formd1.Dispose();

            if (chkRestore.Checked)
                this.WindowState = FormWindowState.Normal;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveConfig();
        }

        private string getLanguage()
        {
            string strLang = "";
            switch (cmbLanguage.SelectedIndex)
            {
                case 0:
                    strLang = "ara";
                    break;
                case 1:
                    strLang = "bul";
                    break;
                case 2:
                    strLang = "chs";
                    break;
                case 3:
                    strLang = "cht";
                    break;
                case 4:
                    strLang = "hrv";
                    break;
                case 5:
                    strLang = "cze";
                    break;
                case 6:
                    strLang = "dan";
                    break;
                case 7:
                    strLang = "dut";
                    break;
                case 8:
                    strLang = "eng";
                    break;
                case 9:
                    strLang = "fin";
                    break;
                case 10:
                    strLang = "fre";
                    break;
                case 11:
                    strLang = "ger";
                    break;
                case 12:
                    strLang = "gre";
                    break;
                case 13:
                    strLang = "hun";
                    break;
                case 14:
                    strLang = "kor";
                    break;
                case 15:
                    strLang = "ita";
                    break;
                case 16:
                    strLang = "jpn";
                    break;
                case 17:
                    strLang = "nor";
                    break;
                case 18:
                    strLang = "pol";
                    break;
                case 19:
                    strLang = "por";
                    break;
                case 20:
                    strLang = "rus";
                    break;
                case 21:
                    strLang = "slv";
                    break;
                case 22:
                    strLang = "spa";
                    break;
                case 23:
                    strLang = "swe";
                    break;
                case 24:
                    strLang = "tur";
                    break;
            }
            return strLang;
        }

        private string getCacheDir()
        {
            return AppDomain.CurrentDomain.BaseDirectory + "cache\\";
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            tmrCopy.Enabled = false;
            if (!string.IsNullOrWhiteSpace(txtResult.Text))
            {
                try
                {
                    Clipboard.SetText(txtResult.Text);
                }
                catch
                {
                    tmrCopy.Enabled = true;
                }
            }
        }

        private void tmrCopy_Tick(object sender, EventArgs e)
        {
            tmrCopy.Enabled = false;
            btnCopy_Click(this, null);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.Extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    fileInfo.Extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
                {
                    OpenFile(file);
                    return;
                }
                lblStatus.Text = "Not a JPEG file";
                lblStatus.ForeColor = Color.Yellow;
                lblStatus.BackColor = Color.Red;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.R))
            {
                if (btnRegion.Enabled)
                    btnRegion_Click(this, null);
                return true;
            } 
            else if (keyData == (Keys.Control | Keys.O))
            {
                if (btnOpen.Enabled)
                    btnOpen_Click(this, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }


}
