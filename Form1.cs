using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using IniParser;
using IniParser.Model;
using System.Threading;
using WindowsInput;
using ShareX.HelpersLib;
using Windows.Media.Ocr;
using Windows.Globalization;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OCRGet
{


    public partial class Form1 : Form
    {
        // P/Invoke constants
        private const int WM_SYSCOMMAND = 0x112;
        private const int MF_STRING = 0x0;
        private const int MF_SEPARATOR = 0x800;
        // P/Invoke declarations
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIDNewItem, string lpNewItem);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InsertMenu(IntPtr hMenu, int uPosition, int uFlags, int uIDNewItem, string lpNewItem);

        // ID for the Open item on the system menu
        private const int SYSMENU_OPEN_ID = 0x1;

        struct Credential
        {
            public string Name, Url, Key, Useragent;
        }

        struct OcrLanguage
        {
            public readonly string Name, OcrSpace, WinOcr;

            public OcrLanguage(string OcrSpace, string WinOcr, string Name)
            {
                this.OcrSpace = OcrSpace;
                this.WinOcr = WinOcr;
                this.Name = Name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        private readonly OcrLanguage[] _lnglist = {
                new OcrLanguage("ara", "ar", "Arabic"),
                new OcrLanguage("bul", "bg", "Bulgarian"),
                new OcrLanguage("chs", "zh-Hans", "Chinese(Simplified)"),
                new OcrLanguage("cht", "zh-Hant", "Chinese(Traditional)"),
                new OcrLanguage("hrv", "hr", "Croatian"),
                new OcrLanguage("cze", "cs", "Czech"),
                new OcrLanguage("dan", "da", "Danish"),
                new OcrLanguage("dut", "nl", "Dutch"),
                new OcrLanguage("eng", "en", "English"),
                new OcrLanguage("fin", "fi", "Finnish"),
                new OcrLanguage("fre", "fr", "French"),
                new OcrLanguage("ger", "de", "German"),
                new OcrLanguage("gre", "el", "Greek"),
                new OcrLanguage("hun", "hu", "Hungarian"),
                new OcrLanguage("kor", "ko", "Korean"),
                new OcrLanguage("ita", "it", "Italian"),
                new OcrLanguage("jpn", "ja", "Japanese"),
                new OcrLanguage("pol", "pl", "Polish"),
                new OcrLanguage("por", "pt", "Portuguese"),
                new OcrLanguage("rus", "ru", "Russian"),
                new OcrLanguage("slv", "sl", "Slovenian"),
                new OcrLanguage("spa", "es", "Spanish"),
                new OcrLanguage("swe", "sv", "Swedish"),
                new OcrLanguage("tur", "tr", "Turkish"),
                new OcrLanguage("hin", "hi", "Hindi ↓ engine 3 only ↓"), // Engine 3 exclusive from here on
                new OcrLanguage("kan", "kn", "Kannada"),
                new OcrLanguage("per", "fa", "Persian (Fari)"),
                new OcrLanguage("tel", "te", "Telugu"),
                new OcrLanguage("tam", "ta", "Tamil"),
                new OcrLanguage("tai", "th", "Thai"),
                new OcrLanguage("vie", "vi", "Vietnamese")
            };

        private FormDraw _formd1 = null; // region draw form
        private FormDraw _formn1 = null; // OSD notify form

        private string p_imagepath { get; set; }
        private string p_imagesize { get; set; }

        private Credential[] _creds;
        private int[] _enginecreds = { 0, 0, 0 };

        private Credential p_cred
        {
            get { return _creds[_enginecreds[p_engine - 1]]; }
        }

        private int p_engine
        {
            get
            {
                int v = 1;
                if (rdbEngine2.Checked) v = 2;
                else if (rdbEngine3.Checked) v = 3;
                return v;
            }
            set
            {
                switch (value)
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
        }

        private int p_ocr
        {
            get
            {
                int v = 1;
                if (rdbOCR2.Checked) v = 2;
                return v;
            }
            set
            {
                switch (value)
                {
                    case 2:
                        rdbOCR2.Checked = true;
                        break;
                    default:
                        rdbOCR1.Checked = true;
                        break;
                }
            }
        }

        private string p_language { get; set; }
        private string p_resulttext { get; set; }
        private int p_nettimeout { get; set; }

        private FileIniDataParser _config;
        private IniData _inidata;
        private string _inifile;
        private int _hotkeyID = 0;
        private FileSystemWatcher _watcher;
        private string _autoloadFile = "";
        private string[] _imageExtensions = { ".jpg", ".jpeg", ".png" };

        public Form1()
        {
            InitializeComponent();

            UiStatusMessage("Ready.", StatusMessageType.SM_Plain);
            var apistatus = "https://status.ocr.space/";
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel1.Links.Add(0, 0, apistatus);
            toolTip1.SetToolTip(linkLabel1, apistatus);
            lbLastAction.Text = "";

            cmbLanguage.Items.Clear();
            foreach (var item in _lnglist)
            {
                cmbLanguage.Items.Add(item);
            }

            // create cache folder
            if (!Directory.Exists(getCacheDir()))
            {
                Directory.CreateDirectory(getCacheDir());
            }

            InitFSwatcher();
            LoadConfig();

            if (chkClearCache.Checked)
                ClearCache();

            RegisterHotkey();
        }

        private void ClearCache()
        {
            string path = getCacheDir();
            string[] wildcards = _imageExtensions.Select(item => "*" + item).ToArray();
            var result = wildcards
                .SelectMany(wc => Directory.EnumerateFiles(path, wc, SearchOption.AllDirectories))
                .ToList();
            foreach (var file in result)
            {
                if (File.Exists(file))
                {
                    if (chkRecycle.Checked)
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(file,
                            Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                            Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    }
                    else
                    {
                        FileInfo fi = new FileInfo(file);
                        fi.IsReadOnly = false;
                        fi.Delete();
                    }
                }
            }
        }

        private void InitFSwatcher()
        {
            _watcher = new FileSystemWatcher(getCacheDir());
            _watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.FileName |
                NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size;
            _watcher.Changed += WatcherOnChanged;
            _watcher.Created += WatcherOnChanged;
            _watcher.Renamed += WatcherOnRenamed;
            _watcher.IncludeSubdirectories = true;
        }

        private void LoadConfig()
        {
            _config = new FileIniDataParser();
            _inifile = Path.ChangeExtension(Application.ExecutablePath, ".ini");

            // Create fresh INI file
            if (!File.Exists(_inifile))
            {
                // config file only values
                _inidata = new IniData();
                _inidata.Sections.AddSection("general");
                _inidata["general"].AddKey("cred-number", "2");
                _inidata["general"].AddKey("cred-name1", "Default - OCRGet");
                _inidata["general"].AddKey("cred-apiurl1", "http://api.ocr.space/Parse/Image");
                _inidata["general"].AddKey("cred-apikey1", "helloworld");
                _inidata["general"].AddKey("cred-useragent1", "OCRGet");
                _inidata["general"].AddKey("cred-name2", "Default - Firefox");
                _inidata["general"].AddKey("cred-apiurl2", "http://api.ocr.space/Parse/Image");
                _inidata["general"].AddKey("cred-apikey2", "helloworld");
                _inidata["general"].AddKey("cred-useragent2", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36");
                _inidata["general"].AddKey("fontsize", "9");

                // ui values
                _inidata["general"].AddKey("language", "16");
                _inidata["general"].AddKey("autocopy", "True");
                _inidata["general"].AddKey("autorecognize", "True");
                _inidata["general"].AddKey("autorecognizeDelay", "0");
                _inidata["general"].AddKey("restoreapp", "True");
                _inidata["general"].AddKey("jpegquality", "95");
                _inidata["general"].AddKey("clearcache", "False");
                _inidata["general"].AddKey("showprogress", "False");
                _inidata["general"].AddKey("scaleCaptured", "True");
                _inidata["general"].AddKey("scaleFactor", "2");

                _inidata["general"].AddKey("detectOrientation", "False");
                _inidata["general"].AddKey("scale", "True");
                _inidata["general"].AddKey("removeLinebreaks", "False");
                _inidata["general"].AddKey("removeSpaces", "False");
                _inidata["general"].AddKey("isTable", "False");
                _inidata["general"].AddKey("engine", "1");
                _inidata["general"].AddKey("ocr", "1");
                _inidata["general"].AddKey("engine1cred", "0");
                _inidata["general"].AddKey("engine2cred", "0");
                _inidata["general"].AddKey("engine3cred", "0");

                _inidata["general"].AddKey("quicklang1", "-1");
                _inidata["general"].AddKey("quicklang1name", "");
                _inidata["general"].AddKey("quicklang2", "8");
                _inidata["general"].AddKey("quicklang2name", "eng");
                _inidata["general"].AddKey("quicklang3", "16");
                _inidata["general"].AddKey("quicklang3name", "jpn");

                _inidata["general"].AddKey("nettimeout", "15");

                _config.WriteFile(_inifile, _inidata);
            }

            _inidata = _config.ReadFile(_inifile);

            //try
            //{
            _creds = new Credential[int.Parse(_inidata["general"]["cred-number"])];
            contextMenuStrip1.Items.Clear();
            for (int i = 1; i <= _creds.Length; i++)
            {
                _creds[i - 1].Name = _inidata["general"][$"cred-name{i}"];
                _creds[i - 1].Url = _inidata["general"][$"cred-apiurl{i}"];
                _creds[i - 1].Key = _inidata["general"][$"cred-apikey{i}"];
                _creds[i - 1].Useragent = _inidata["general"][$"cred-useragent{i}"];

                ToolStripMenuItem item = new ToolStripMenuItem(_creds[i - 1].Name);
                item.Tag = i - 1;
                contextMenuStrip1.Items.Add(item);
            }

            // Program options
            Font f = new Font(txtResult.Font.FontFamily, float.Parse(_inidata["general"]["fontsize"]));
            txtResult.Font = f;
            chkAutocopy.Checked = bool.Parse(_inidata["general"]["autocopy"]);
            chkAutorecognize.Checked = bool.Parse(_inidata["general"]["autorecognize"]);
            string v;
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "autorecognizeDelay", out v);
            udAutorecognize.Value = decimal.Parse(string.IsNullOrEmpty(v) ? "0" : v);
            chkRestore.Checked = bool.Parse(_inidata["general"]["restoreapp"]);
            udQuality.Value = decimal.Parse(_inidata["general"]["jpegquality"]);
            chkClearCache.Checked = bool.Parse(_inidata["general"]["clearcache"]);
            chkShowProgress.Checked = bool.Parse(_inidata["general"]["showprogress"]);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "scaleCaptured", out v);
            chkScaleFactor.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "True" : v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "scaleFactor", out v);
            nudScaleFactor.Value = decimal.Parse(string.IsNullOrEmpty(v) ? "2" : v);

            // ocr.space options
            chkDetectOrientation.Checked = bool.Parse(_inidata["general"]["detectOrientation"]);
            chkScale.Checked = bool.Parse(_inidata["general"]["scale"]);
            chkRemoveLinebreaks.Checked = bool.Parse(_inidata["general"]["removeLinebreaks"]);
            chkRemoveSpaces.Checked = bool.Parse(_inidata["general"]["removeSpaces"]);
            chkIsTable.Checked = bool.Parse(_inidata["general"]["isTable"]);
            _enginecreds[0] = int.Parse(_inidata["general"]["engine1cred"]);
            _enginecreds[1] = int.Parse(_inidata["general"]["engine2cred"]);
            _enginecreds[2] = int.Parse(_inidata["general"]["engine3cred"]);
            p_engine = int.Parse(_inidata["general"]["engine"]);
            p_ocr = int.Parse(_inidata["general"]["ocr"]);

            v = _inidata["general"]["quicklang1"];
            btnQuickLng1.Tag = v == null ? -1 : int.Parse(v);
            v = _inidata["general"]["quicklang2"];
            btnQuickLng2.Tag = v == null ? -1 : int.Parse(v);
            v = _inidata["general"]["quicklang3"];
            btnQuickLng3.Tag = v == null ? -1 : int.Parse(v);
            v = _inidata["general"]["quicklang1name"];
            btnQuickLng1.Text = v == null ? "" : v;
            v = _inidata["general"]["quicklang2name"];
            btnQuickLng2.Text = v == null ? "" : v;
            v = _inidata["general"]["quicklang3name"];
            btnQuickLng3.Text = v == null ? "" : v;
            if ((int)btnQuickLng1.Tag >= 0) toolTip1.SetToolTip(btnQuickLng1, _lnglist[(int)btnQuickLng1.Tag].Name);
            if ((int)btnQuickLng2.Tag >= 0) toolTip1.SetToolTip(btnQuickLng2, _lnglist[(int)btnQuickLng2.Tag].Name);
            if ((int)btnQuickLng3.Tag >= 0) toolTip1.SetToolTip(btnQuickLng3, _lnglist[(int)btnQuickLng3.Tag].Name);
            cmbLanguage.SelectedIndex = int.Parse(_inidata["general"]["language"]);

            v = _inidata["general"]["nettimeout"];
            p_nettimeout = v == null ? 15 : int.Parse(v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "autoload", out v);
            chkAutoLoad.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "False" : v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "autoloadrestoreapp", out v);
            chkRestoreAutoLoad.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "False" : v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "clearcacherecyclebin", out v);
            chkRecycle.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "False" : v);
            //}
            //catch { }
        }

        private void SaveConfig()
        {
            _inidata["general"]["fontsize"] = txtResult.Font.Size.ToString();

            _inidata["general"]["language"] = cmbLanguage.SelectedIndex.ToString();
            _inidata["general"]["autocopy"] = chkAutocopy.Checked.ToString();
            _inidata["general"]["autorecognize"] = chkAutorecognize.Checked.ToString();
            _inidata["general"]["autorecognizeDelay"] = udAutorecognize.Value.ToString();
            _inidata["general"]["restoreapp"] = chkRestore.Checked.ToString();
            _inidata["general"]["jpegquality"] = udQuality.Value.ToString();
            _inidata["general"]["clearcache"] = chkClearCache.Checked.ToString();
            _inidata["general"]["showprogress"] = chkShowProgress.Checked.ToString();
            _inidata["general"]["scaleCaptured"] = chkScaleFactor.Checked.ToString();
            _inidata["general"]["scaleFactor"] = nudScaleFactor.Value.ToString();

            _inidata["general"]["detectOrientation"] = chkDetectOrientation.Checked.ToString();
            _inidata["general"]["scale"] = chkScale.Checked.ToString();
            _inidata["general"]["removeLinebreaks"] = chkRemoveLinebreaks.Checked.ToString();
            _inidata["general"]["removeSpaces"] = chkRemoveSpaces.Checked.ToString();
            _inidata["general"]["isTable"] = chkIsTable.Checked.ToString();
            _inidata["general"]["engine"] = p_engine.ToString();
            _inidata["general"]["ocr"] = p_ocr.ToString();
            _inidata["general"]["engine1cred"] = _enginecreds[0].ToString();
            _inidata["general"]["engine2cred"] = _enginecreds[1].ToString();
            _inidata["general"]["engine3cred"] = _enginecreds[2].ToString();

            _inidata["general"]["quicklang1"] = btnQuickLng1.Tag.ToString();
            _inidata["general"]["quicklang1name"] = btnQuickLng1.Text;
            _inidata["general"]["quicklang2"] = btnQuickLng2.Tag.ToString();
            _inidata["general"]["quicklang2name"] = btnQuickLng2.Text;
            _inidata["general"]["quicklang3"] = btnQuickLng3.Tag.ToString();
            _inidata["general"]["quicklang3name"] = btnQuickLng3.Text;

            _inidata["general"]["nettimeout"] = p_nettimeout.ToString();
            _inidata["general"]["autoload"] = chkAutoLoad.Checked.ToString();
            _inidata["general"]["autoloadrestoreapp"] = chkRestoreAutoLoad.Checked.ToString();
            _inidata["general"]["clearcacherecyclebin"] = chkRecycle.Checked.ToString();

            _config.WriteFile(_inifile, _inidata);
        }

        private void OpenFile(string file)
        {
            p_imagepath = file;
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = Image.FromFile(p_imagepath);
            FileInfo fileInfo = new FileInfo(p_imagepath);
            this.Text = "OCRGet - " + fileInfo.Name;
            p_imagesize = ((float)fileInfo.Length / 1024f / 1024f).ToString("0.00") + " MB";
            UiStatusMessage("Loaded " + p_imagesize, StatusMessageType.SM_Ok);

            if (chkAutorecognize.Checked)
            {
                int delay = (int)udAutorecognize.Value;
                if (delay > 0)
                {
                    tmrAutorecognize.Interval = delay;
                    tmrAutorecognize.Enabled = true;
                }
                else
                {
                    Recognize(p_ocr);
                }
            }
        }
        private void tmrAutorecognize_Tick(object sender, EventArgs e)
        {
            tmrAutorecognize.Enabled = false;
            Recognize(p_ocr);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Images(.jpg;.jpeg;.png)|*.jpg;*.jpeg;*.png";
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                OpenFile(fileDlg.FileName);
            }
        }

        void formn1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString("Requesting OCR responce...", new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold), Brushes.Lime, 0, 0);
        }

        private void Recognize(int ocr)
        {
            txtResult.Focus();
            txtResult.SelectionLength = 0;

            txtResult.Text = "";
            p_resulttext = "";
            p_language = getLanguage(ocr);

            switch (ocr)
            {
                case 1:
                    lbLastAction.Text = "ocr.space -->";
                    RecognizeOcrSpace();
                    break;
                case 2:
                    lbLastAction.Text = "Win OCR -->";
                    RecognizeWinOcr();
                    break;
                default:
                    throw new ArgumentException("Recognize() : Unknown OCR.");
            }
        }

        async private void RecognizeWinOcr()
        {
            var language = new Language(p_language);
            if (!OcrEngine.IsLanguageSupported(language))
            {
                p_resulttext = $"'{language.LanguageTag}' is not supported in this system. Consult Information option.";
                RecognizeFinish();
                return;
            }
            var stream = File.OpenRead(p_imagepath);
            var decoder = await BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
            var bitmap = await decoder.GetSoftwareBitmapAsync();
            var engine = OcrEngine.TryCreateFromLanguage(language);
            var ocrResult = await engine.RecognizeAsync(bitmap).AsTask();
            p_resulttext = ocrResult.Text;
            stream.Close(); // free the file
            RecognizeFinish();

            //var _engine = OcrEngine.TryCreateFromLanguage(new Language("en-US"));
            //var file = await StorageFile.GetFileFromPathAsync("");
            //var _stream = await file.OpenAsync(FileAccessMode.Read);
            //var _decoder = await BitmapDecoder.CreateAsync(_stream);
            //var softwareBitmap = await _decoder.GetSoftwareBitmapAsync();
            //var _ocrResult = await _engine.RecognizeAsync(softwareBitmap);
            ////_ocrResult.Text;
        }

        private void RecognizeOcrSpace()
        {
            if (chkShowProgress.Checked)
            {
                _formn1 = new FormDraw();
                _formn1.Opacity = 1.0;
                _formn1.BackColor = Color.Black;
                _formn1.Width = 200;
                _formn1.Height = 20;
                _formn1.Left = 0;
                _formn1.Top = Screen.PrimaryScreen.Bounds.Height - _formn1.Height;
                _formn1.Paint += new PaintEventHandler(formn1_Paint);
                _formn1.Show();
            }

            btnOpen.Enabled = false;
            btnRegion.Enabled = false;
            btnRecognize.Enabled = false;
            UiStatusMessage($"Contacting: {p_cred.Url}", StatusMessageType.SM_Warning);

            Thread t = new Thread(new ParameterizedThreadStart(RecognizeThread));
            t.Start(this);
        }

        private static void RecognizeThread(object o)
        {
            Form1 form = (Form1)o;

            // Read file data
            FileStream fs = new FileStream(form.p_imagepath, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("apikey", form.p_cred.Key);
            if (form.p_engine != 2)
                postParameters.Add("language", form.p_language);
            postParameters.Add("file", new FormUpload.FileParameter(data, "image.jpg", "image"));

            postParameters.Add("detectOrientation", form.chkDetectOrientation.Checked.ToString().ToLower());
            postParameters.Add("scale", form.chkScale.Checked.ToString().ToLower());
            postParameters.Add("isTable", form.chkIsTable.Checked.ToString().ToLower());
            postParameters.Add("OCREngine", form.p_engine.ToString());

            // Create request and receive response
            HttpWebResponse webResponse;
            try
            {
                webResponse = FormUpload.MultipartFormDataPost(form.p_cred.Url, form.p_cred.Useragent,
                    postParameters, form.p_nettimeout * 1000);
            }
            catch (Exception e)
            {
                form.p_resulttext = "ERROR: " + e.Message;
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
                        form.p_resulttext = form.p_resulttext + ocrResult.ParsedResults[i].ParsedText;
                    }
                }
                else
                {
                    form.p_resulttext = "ERROR: " + fullResponse;
                }
            }
            catch
            {
                form.p_resulttext = "ERROR: " + fullResponse;
            }

        RecognizeEend:
            form.Invoke((MethodInvoker)delegate
            {
                form.lbLastAction.Text = "ocr.space -->";
                form.RecognizeFinish();
            });
        }

        private void RecognizeFinish()
        {
            if (_formn1 != null)
            {
                _formn1.Hide();
                _formn1.Dispose();
                _formn1 = null;
            }

            // remove linebreaks
            if (chkRemoveLinebreaks.Checked)
                p_resulttext = p_resulttext.Replace("\n", "").Replace("\r", "");
            // remove spaces
            if (chkRemoveSpaces.Checked)
                p_resulttext = p_resulttext.Replace(" ", "");

            txtResult.Text = p_resulttext;
            btnOpen.Enabled = true;
            btnRegion.Enabled = true;
            btnRecognize.Enabled = true;
            UiStatusMessage(p_imagesize + " Done", StatusMessageType.SM_Ok);

            if (chkAutocopy.Checked)
            {
                btnCopy_Click(this, null);
            }

            if (chkRestore.Checked && chkShowProgress.Checked && this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
        }

        private void btnRecognize_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(p_imagepath)) return;
            Recognize(p_ocr);
        }

        private void btnRegion_Click(object sender, EventArgs e)
        {
            _formd1 = new FormDraw();
            _formd1.VisibleChanged += new EventHandler(this.FormDrawOnVisible);
            _formd1.Show();
            this.WindowState = FormWindowState.Minimized;
            _formd1.Activate();
        }

        private void FormDrawOnVisible(object sender, EventArgs e)
        {
            if (!_formd1.Visible)
            {
                tmrSnap.Enabled = true;
            }
        }

        private void tmrSnap_Tick(object sender, EventArgs e)
        {
            tmrSnap.Enabled = false;
            Rectangle rect = new Rectangle(_formd1.area.X, _formd1.area.Y, _formd1.area.Width, _formd1.area.Height);
            if (rect.Width > 10 && rect.Height > 10)
            {
                using (Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    Bitmap bmpResult = bmp;
                    // get image from screen
                    g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

                    // scale image
                    if (chkScaleFactor.Checked)
                    {
                        bmpResult = ImageHelper.ResizeImage(bmp,
                            (int)(bmp.Width * (float)nudScaleFactor.Value),
                            (int)(bmp.Height * (float)nudScaleFactor.Value));
                    }

                    _watcher.EnableRaisingEvents = false;
                    // save image to file
                    p_imagepath = getCacheDir() + DateTime.Now.ToString("yyyyMMdd-HHmmss-fff") + ".jpg";
                    Jpeg.Save(bmpResult, p_imagepath, (long)udQuality.Value);
                    if (chkAutoLoad.Checked)
                        _watcher.EnableRaisingEvents = true;
                }

                OpenFile(p_imagepath);

                if (chkRestore.Checked && !chkShowProgress.Checked)
                    this.WindowState = FormWindowState.Normal;
            }
            else // snap cancelled
            {
                if (chkRestore.Checked)
                    this.WindowState = FormWindowState.Normal;
            }
            _formd1.Dispose();
            _formd1 = null;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _watcher.EnableRaisingEvents = false;
            UnregisterHotkey();
            SaveConfig();
        }

        private string getLanguage(int ocr)
        {

            var ln = _lnglist[cmbLanguage.SelectedIndex];
            string rv;
            switch (ocr)
            {
                case 1:
                    rv = ln.OcrSpace;
                    break;
                case 2:
                    rv = ln.WinOcr;
                    break;
                default:
                    throw new Exception("getLanguage() : Unknown OCR.");
            }
            return rv;
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
                var txt = (txtResult.SelectionLength > 0) ? txtResult.SelectedText : txtResult.Text;
                try
                {
                    Clipboard.SetText(txt);
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
                var extension = Path.GetExtension(file).ToLower();
                if (File.Exists(file) && !string.IsNullOrEmpty(extension) && _imageExtensions.Any(ext => ext == extension))
                {
                    OpenFile(file);
                    return;
                }
                UiStatusMessage("Not an image file", StatusMessageType.SM_Error);
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
            else if (keyData == (Keys.Control | Keys.W)) // rewrite file
            {
                btnRewrite_Click(this, null);
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
            _hotkeyID = NativeMethods.GlobalAddAtom(uniqueID);
            if (_hotkeyID == 0)
            {
                throw new Exception("RegisterHotkey() : Unable to generate unique hotkey ID: " + _hotkeyID);
            }

            // Ctrl-Alt-S
            if (!NativeMethods.RegisterHotKey(this.Handle, _hotkeyID, (uint)Modifiers.Control | (uint)Modifiers.Alt,
                (uint)ShareX.HelpersLib.VirtualKeyCode.KEY_S))
            {
                NativeMethods.GlobalDeleteAtom((ushort)_hotkeyID);
                throw new Exception("RegisterHotkey() : Unable to register hotkey Ctrl-Alt-S with ID: " + _hotkeyID);
            }
        }

        private void UnregisterHotkey()
        {
            if (_hotkeyID > 0)
            {
                bool result = NativeMethods.UnregisterHotKey(this.Handle, _hotkeyID);
                if (result)
                {
                    NativeMethods.GlobalDeleteAtom((ushort)_hotkeyID);
                    _hotkeyID = 0;
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
                if (btnRegion.Enabled && _formd1 == null)
                    btnRegion_Click(this, null);
                return;
            }

            base.WndProc(ref m);

            if ((m.Msg == WM_SYSCOMMAND) && ((int)m.WParam == SYSMENU_OPEN_ID))
            {
                var psi = new ProcessStartInfo
                {
                    FileName = getCacheDir(),
                    UseShellExecute = true
                };
                Process.Start(psi);
            }

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var menu = sender as ContextMenuStrip;
            if (menu != null)
            {
                Control controlSelected = menu.SourceControl; // engine radiobutton that called menu
                foreach (ToolStripMenuItem item in menu.Items)
                {
                    item.Checked = false;
                }
                int credindex = _enginecreds[Convert.ToInt32(controlSelected.Tag)];
                var menuitem = menu.Items[credindex] as ToolStripMenuItem;
                menuitem.Checked = true;
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var menu = sender as ContextMenuStrip;
            var menuitem = e.ClickedItem as ToolStripMenuItem;
            if (menu != null)
            {
                Control controlSelected = menu.SourceControl;
                _enginecreds[Convert.ToInt32(controlSelected.Tag)] = Convert.ToInt32(menuitem.Tag);
            }
            // update credential UI just in case we updated selected (focused) engine
            UiUpdateCredential();
        }

        private void UiUpdateCredential()
        {
            var item = statusStrip1.Items[1] as ToolStripStatusLabel;
            item.Text = p_cred.Name;
            item.ToolTipText = $"Credential: {p_cred.Name}" + Environment.NewLine +
                $"API key: {p_cred.Key}" + Environment.NewLine + $"URL: {p_cred.Url}" + Environment.NewLine +
                $"Useragent: {p_cred.Useragent}";
        }

        private void rdbEngine1_CheckedChanged(object sender, EventArgs e)
        {
            UiUpdateCredential();
        }

        private void chkRemoveLinebreaks_CheckedChanged(object sender, EventArgs e)
        {
            string text = txtResult.Text;
            // remove linebreaks
            if (chkRemoveLinebreaks.Checked)
                text = text.Replace("\n", "").Replace("\r", "");
            // remove spaces
            if (chkRemoveSpaces.Checked)
                text = text.Replace(" ", "");
            txtResult.Text = text;
        }

        enum StatusMessageType { SM_Ok, SM_Warning, SM_Error, SM_Plain }
        private void UiStatusMessage(string message, StatusMessageType type)
        {
            statusStrip1.Items[0].Text = message;
            switch (type)
            {
                case StatusMessageType.SM_Ok:
                    statusStrip1.Items[0].ForeColor = SystemColors.ControlText;
                    statusStrip1.Items[0].BackColor = Color.LightGreen;
                    break;
                case StatusMessageType.SM_Warning:
                    statusStrip1.Items[0].ForeColor = SystemColors.ControlText;
                    statusStrip1.Items[0].BackColor = Color.Yellow;
                    break;
                case StatusMessageType.SM_Error:
                    statusStrip1.Items[0].ForeColor = Color.Yellow;
                    statusStrip1.Items[0].BackColor = Color.Red;
                    break;
                case StatusMessageType.SM_Plain:
                    statusStrip1.Items[0].ForeColor = SystemColors.ControlText;
                    statusStrip1.Items[0].BackColor = SystemColors.Control;
                    break;
            }
        }

        private void btnWinOcrInfo_Click(object sender, EventArgs e)
        {
            var form = new FormHelp();
            form.Text = "Windows OCR Information";
            form.richText.LoadFile(Path.GetDirectoryName(Application.ExecutablePath) +
                Path.DirectorySeparatorChar + "Info.rtf");
            form.Show(this);
        }

        private void btnRewrite_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(p_imagepath)) return;
            if (!File.Exists(p_imagepath)) return;

            _watcher.EnableRaisingEvents = false;
            // free the file
            pictureBox1.Image.Dispose();
            File.SetLastWriteTime(p_imagepath, DateTime.Now);
            // restore file image
            pictureBox1.Image = Image.FromFile(p_imagepath);
            if (chkAutoLoad.Checked)
                _watcher.EnableRaisingEvents = true;

            UiStatusMessage("Rewritten: " + Path.GetFileName(p_imagepath), StatusMessageType.SM_Ok);
        }

        private void btnWinOcrRecognize_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(p_imagepath)) return;
            Recognize(2); // Win OCR
        }

        private void btnQuickLng1_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Tag == null) return;
            if ((int)button.Tag < 0) return;
            cmbLanguage.SelectedIndex = (int)button.Tag;
        }

        private void cmbLanguage_SelectedValueChanged(object sender, EventArgs e)
        {
            int index = cmbLanguage.SelectedIndex;
            btnQuickLng1.BackColor = SystemColors.ControlLight;
            btnQuickLng2.BackColor = SystemColors.ControlLight;
            btnQuickLng3.BackColor = SystemColors.ControlLight;
            if (index == (int)btnQuickLng1.Tag) btnQuickLng1.BackColor = Color.FromArgb(192, 255, 255);
            if (index == (int)btnQuickLng2.Tag) btnQuickLng2.BackColor = Color.FromArgb(192, 255, 255);
            if (index == (int)btnQuickLng3.Tag) btnQuickLng3.BackColor = Color.FromArgb(192, 255, 255);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            IntPtr hSysMenu = GetSystemMenu(this.Handle, false);
            AppendMenu(hSysMenu, MF_SEPARATOR, 0, string.Empty);
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_OPEN_ID, "&Explore cache folder...");
        }

        private void chkAutoLoad_CheckedChanged(object sender, EventArgs e)
        {
            _watcher.EnableRaisingEvents = chkAutoLoad.Checked;
        }

        private void WatcherOnChanged(object sender, FileSystemEventArgs e)
        {
            if (Directory.Exists(e.FullPath))
                return;
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                case WatcherChangeTypes.Changed:
                    Invoke((MethodInvoker)delegate
                    {
                        tmrAutoload.Stop();
                        _autoloadFile = e.FullPath;
                        tmrAutoload.Start();
                    });
                    break;
            }
        }

        private void WatcherOnRenamed(object source, RenamedEventArgs e)
        {
            if (Directory.Exists(e.FullPath) || Directory.Exists(e.OldFullPath))
                return;
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Renamed:
                    Invoke((MethodInvoker)delegate
                    {
                        tmrAutoload.Stop();
                        _autoloadFile = e.FullPath;
                        tmrAutoload.Start();
                    });
                    break;
            }
        }

        private void tmrAutoload_Tick(object sender, EventArgs e)
        {
            tmrAutoload.Stop();

            var extension = Path.GetExtension(_autoloadFile).ToLower();
            if (!string.IsNullOrEmpty(extension) && _imageExtensions.Any(ext => ext == extension))
            {
                OpenFile(_autoloadFile);
                if (chkRestoreAutoLoad.Checked)
                {
                    if (this.WindowState == FormWindowState.Minimized)
                        this.WindowState = FormWindowState.Normal;
                    this.Activate();
                }
            }
        }
    } // Form1
}
