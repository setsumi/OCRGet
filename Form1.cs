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
using WindowsInput.Native;
using WindowsInput;
using ShareX.HelpersLib;

namespace OCRGet
{


    public partial class Form1 : Form
    {

        private FormDraw formd1 = null; // region draw form
        private FormDraw formn1 = null; // OSD notify form
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
        private int hotkeyID = 0;

        public Form1()
        {
            InitializeComponent();

            lblStatus.Text = "";
            linkLabel1.Links.Add(11, linkLabel1.Text.Length - 11, "https://status.ocr.space/");

            // create cache folder
            if (!Directory.Exists(getCacheDir()))
            {
                Directory.CreateDirectory(getCacheDir());
            }

            LoadConfig();

            // clear cache
            if (chkClearCache.Checked)
            {
                foreach (string f in Directory.EnumerateFiles(getCacheDir(), "*.jpg"))
                {
                    File.Delete(f);
                }
            }

            RegisterHotkey();
        }

        private void LoadConfig()
        {
            config = new FileIniDataParser();
            inifile = Path.ChangeExtension(Application.ExecutablePath, ".ini");

            if (!File.Exists(inifile))
            {
                // config file only values
                inidata = new IniData();
                inidata.Sections.AddSection("general");
                inidata["general"].AddKey("apiurl", "http://api.ocr.space/Parse/Image");
                inidata["general"].AddKey("apikey", "helloworld");
                inidata["general"].AddKey("useragent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36");
                inidata["general"].AddKey("fontsize", "9");

                // ui values
                inidata["general"].AddKey("language", "16");
                inidata["general"].AddKey("autocopy", "True");
                inidata["general"].AddKey("autorecognize", "True");
                inidata["general"].AddKey("restoreapp", "True");
                inidata["general"].AddKey("jpegquality", "90");
                inidata["general"].AddKey("clearcache", "False");
                inidata["general"].AddKey("showprogress", "False");

                inidata["general"].AddKey("detectOrientation", "False");
                inidata["general"].AddKey("scale", "True");
                inidata["general"].AddKey("removeLinebreaks", "False");
                inidata["general"].AddKey("isTable", "False");
                inidata["general"].AddKey("engine", "1");

                config.WriteFile(inifile, inidata);
            }

            inidata = config.ReadFile(inifile);

            try
            {
                apiurl = inidata["general"]["apiurl"];
                apikey = inidata["general"]["apikey"];
                useragent = inidata["general"]["useragent"];
                Font f = new Font(txtResult.Font.FontFamily, float.Parse(inidata["general"]["fontsize"]));
                txtResult.Font = f;

                cmbLanguage.SelectedIndex = int.Parse(inidata["general"]["language"]);
                chkAutocopy.Checked = bool.Parse(inidata["general"]["autocopy"]);
                chkAutorecognize.Checked = bool.Parse(inidata["general"]["autorecognize"]);
                chkRestore.Checked = bool.Parse(inidata["general"]["restoreapp"]);
                udQuality.Value = decimal.Parse(inidata["general"]["jpegquality"]);
                chkClearCache.Checked = bool.Parse(inidata["general"]["clearcache"]);
                chkShowProgress.Checked = bool.Parse(inidata["general"]["showprogress"]);

                chkDetectOrientation.Checked = bool.Parse(inidata["general"]["detectOrientation"]);
                chkScale.Checked = bool.Parse(inidata["general"]["scale"]);
                chkRemoveLinebreaks.Checked = bool.Parse(inidata["general"]["removeLinebreaks"]);
                chkIsTable.Checked = bool.Parse(inidata["general"]["isTable"]);
                int engine = int.Parse(inidata["general"]["engine"]);
                switch (engine)
                {
                    case 2:
                        rdbEngine2.Checked = true;
                        break;
                    case 3:
                        rdbEngine3.Checked = true;
                        break;
                    default:
                        rdbEngine1.Checked = true;
                        break;
                }
            }
            catch { }
        }

        private void SaveConfig()
        {
            inidata["general"]["fontsize"] = txtResult.Font.Size.ToString();

            inidata["general"]["language"] = cmbLanguage.SelectedIndex.ToString();
            inidata["general"]["autocopy"] = chkAutocopy.Checked.ToString();
            inidata["general"]["autorecognize"] = chkAutorecognize.Checked.ToString();
            inidata["general"]["restoreapp"] = chkRestore.Checked.ToString();
            inidata["general"]["jpegquality"] = udQuality.Value.ToString();
            inidata["general"]["clearcache"] = chkClearCache.Checked.ToString();
            inidata["general"]["showprogress"] = chkShowProgress.Checked.ToString();

            inidata["general"]["detectOrientation"] = chkDetectOrientation.Checked.ToString();
            inidata["general"]["scale"] = chkScale.Checked.ToString();
            inidata["general"]["removeLinebreaks"] = chkRemoveLinebreaks.Checked.ToString();
            inidata["general"]["isTable"] = chkIsTable.Checked.ToString();
            int engine = 1;
            if (rdbEngine2.Checked) engine = 2;
            else if (rdbEngine3.Checked) engine = 3;
            inidata["general"]["engine"] = engine.ToString();

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
            if (!form.rdbEngine2.Checked) postParameters.Add("language", form.language);
            postParameters.Add("file", new FormUpload.FileParameter(data, "image.jpg", "image"));

            postParameters.Add("detectOrientation", form.chkDetectOrientation.Checked.ToString().ToLower());
            postParameters.Add("scale", form.chkScale.Checked.ToString().ToLower());
            postParameters.Add("isTable", form.chkIsTable.Checked.ToString().ToLower());
            int engine = 1;
            if (form.rdbEngine2.Checked) engine = 2;
            else if (form.rdbEngine3.Checked) engine = 3;
            postParameters.Add("OCREngine", engine.ToString());

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
                    if (form.chkRemoveLinebreaks.Checked)
                    {
                        form.resulttext = form.resulttext.Replace("\n", "").Replace("\r", "");
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

            if (chkRestore.Checked && chkShowProgress.Checked && this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
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

                if (chkRestore.Checked && !chkShowProgress.Checked)
                    this.WindowState = FormWindowState.Normal;
            }
            else // snap cancelled
            {
                if (chkRestore.Checked)
                    this.WindowState = FormWindowState.Normal;
            }
            formd1.Dispose();
            formd1 = null;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotkey();
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
                case 25: // Engine 3 from here on
                    strLang = "hin";
                    break;
                case 26:
                    strLang = "kan";
                    break;
                case 27:
                    strLang = "per";
                    break;
                case 28:
                    strLang = "tel";
                    break;
                case 29:
                    strLang = "tam";
                    break;
                case 30:
                    strLang = "tai";
                    break;
                case 31:
                    strLang = "vie";
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
            if (keyData == (Keys.Control | Keys.S)) // region snap
            {
                if (btnRegion.Enabled)
                    btnRegion_Click(this, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.O)) // open image
            {
                if (btnOpen.Enabled)
                    btnOpen_Click(this, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.R)) // recognize
            {
                if (btnRecognize.Enabled)
                    btnRecognize_Click(this, null);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.C)) // copy text
            {
                btnCopy_Click(this, null);
                return true;
            }
            else if (keyData == (Keys.Escape)) // minimize
            {
                this.WindowState = FormWindowState.Minimized;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void tmrStartup_Tick(object sender, EventArgs e)
        {
            tmrStartup.Enabled = false;

            // show form underlined Alt-keys
            InputSimulator sim = new InputSimulator();
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MENU);
            sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.MENU);
        }

        private void RegisterHotkey()
        {
            string uniqueID = Guid.NewGuid().ToString("N");
            hotkeyID = NativeMethods.GlobalAddAtom(uniqueID);
            if (hotkeyID == 0)
            {
                throw new Exception("Unable to generate unique hotkey ID: " + hotkeyID);
            }

            // Ctrl-Alt-S
            if (!NativeMethods.RegisterHotKey(this.Handle, hotkeyID, (uint)Modifiers.Control | (uint)Modifiers.Alt,
                (uint)ShareX.HelpersLib.VirtualKeyCode.KEY_S))
            {
                NativeMethods.GlobalDeleteAtom((ushort)hotkeyID);
                throw new Exception("Unable to register hotkey Ctrl-Alt-S with ID: " + hotkeyID);
            }
        }

        private void UnregisterHotkey()
        {
            if (hotkeyID > 0)
            {
                bool result = NativeMethods.UnregisterHotKey(this.Handle, hotkeyID);
                if (result)
                {
                    NativeMethods.GlobalDeleteAtom((ushort)hotkeyID);
                    hotkeyID = 0;
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WindowsMessages.HOTKEY)
            {
                //ushort id = (ushort)m.WParam;
                //Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                //Modifiers modifier = (Modifiers)((int)m.LParam & 0xFFFF);
                //OnKeyPressed(id, key, modifier);

                // only one hotkey yet so no need to check for modifiers/keys
                if (btnRegion.Enabled && formd1 == null)
                    btnRegion_Click(this, null);
                return;
            }
            base.WndProc(ref m);
        }

    }
}
