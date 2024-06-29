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
using Microsoft.WindowsAPICodePack.Dialogs;

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

        // IDs for the items on the system menu
        private const int SYSMENU_EXPLORE_CACHE = 0x1;
        private const int SYSMENU_ABOUTBOX = 0x2;

        enum ImageType { Snapped, External, Dontknow }

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

        private Credential[] _creds = null;
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

        private FileIniDataParser _config = null;
        private IniData _inidata = null;
        private string _inifile;
        private int _hotkeyID = 0;
        private FileSystemWatcher _watcher = null;
        private string _autoloadFile = "";
        private string _autoloadFolder = "";
        private string[] _imageExtensions = { ".jpg", ".jpeg", ".png" };
        private Point _imageScroll = new Point(0, 0);
        private MemoryImage _imgOriginal = new MemoryImage();
        private MemoryImage _imgProcessed = new MemoryImage();
        private bool _isBooting = true;
        private bool _allowFormShow = true; // used for hiding form to tray on start

        public Form1()
        {
            InitializeComponent();

            UiStatusMessage("Ready.", StatusMessageType.SM_Plain);
            var apistatus = "https://status.ocr.space/";
            linkLabel1.LinkBehavior = LinkBehavior.HoverUnderline;
            linkLabel1.Links.Add(0, 0, apistatus);
            toolTip1.SetToolTip(linkLabel1, apistatus);
            lbLastAction.Text = "";
            lbMarkerSnap.Visible = false;
            lbMarkerExtern.Visible = false;
            nudScaleFactor.MouseWheel += new MouseEventHandler(this.NudScrollHandler);
            nudScaleExtern.MouseWheel += new MouseEventHandler(this.NudScrollHandler);
            udQuality.MouseWheel += new MouseEventHandler(this.NudScrollHandler);
            nudAutorecognize.MouseWheel += new MouseEventHandler(this.NudScrollHandler);
            nudAutowrite.MouseWheel += new MouseEventHandler(this.NudScrollHandler);

            cmbLanguage.Items.Clear();
            foreach (var item in _lnglist)
            {
                cmbLanguage.Items.Add(item);
            }

            // create cache folder
            if (!Directory.Exists(GetCacheDir()))
            {
                Directory.CreateDirectory(GetCacheDir());
            }

            LoadConfig();

            if (chkClearCache.Checked)
                ClearCache();

            RegisterHotkey();
            InitFSwatcher(_autoloadFolder);

            // command line arguments
            string[] args = Environment.GetCommandLineArgs();
            foreach (var arg in args)
            {
                if (arg == Application.ExecutablePath) continue;
                switch (arg.ToLower())
                {
                    case "/tray":
                        _allowFormShow = false;
                        UiHide();
                        break;
                    default:
                        MessageBox.Show($"Invalid command line argument: {arg}", "OCRGet", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }

            _isBooting = false;
        }

        private void ClearCache()
        {
            string path = GetCacheDir();
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

        private void InitFSwatcher(string folder)
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
                _watcher = null;
            }
            _watcher = new FileSystemWatcher(folder);
            _watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.FileName |
                NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size;
            _watcher.Changed += WatcherOnChanged;
            _watcher.Created += WatcherOnChanged;
            _watcher.Renamed += WatcherOnRenamed;
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = chkAutoLoad.Checked;
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
            string v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "autorecognizeDelay", out v);
            nudAutorecognize.Value = decimal.Parse(string.IsNullOrEmpty(v) ? "0" : v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "autowrite", out v);
            chkAutowrite.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "False" : v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "autowriteDelay", out v);
            nudAutowrite.Value = decimal.Parse(string.IsNullOrEmpty(v) ? "0" : v);
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
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "scaleExternal", out v);
            chkScaleExtern.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "False" : v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "scaleFactorExternal", out v);
            nudScaleExtern.Value = decimal.Parse(string.IsNullOrEmpty(v) ? "2" : v);

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
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "autoloadFolder", out v);
            _autoloadFolder = string.IsNullOrEmpty(v) ? GetCacheDir() : v;
            toolTip1.SetToolTip(btnLoadFromFolder, _autoloadFolder);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "autoloadrestoreapp", out v);
            chkRestoreAutoLoad.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "False" : v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "clearcacherecyclebin", out v);
            chkRecycle.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "False" : v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "zoomimage", out v);
            chkZoom.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "True" : v);
            v = "";
            _inidata.TryGetKey("general" + _inidata.SectionKeySeparator + "processedimage", out v);
            chkProcessed.Checked = bool.Parse(string.IsNullOrEmpty(v) ? "False" : v);
            //}
            //catch { }
        }

        private void SaveConfig()
        {
            _inidata["general"]["fontsize"] = txtResult.Font.Size.ToString();

            _inidata["general"]["language"] = cmbLanguage.SelectedIndex.ToString();
            _inidata["general"]["autocopy"] = chkAutocopy.Checked.ToString();
            _inidata["general"]["autorecognize"] = chkAutorecognize.Checked.ToString();
            _inidata["general"]["autorecognizeDelay"] = nudAutorecognize.Value.ToString();
            _inidata["general"]["autowrite"] = chkAutowrite.Checked.ToString();
            _inidata["general"]["autowriteDelay"] = nudAutowrite.Value.ToString();
            _inidata["general"]["restoreapp"] = chkRestore.Checked.ToString();
            _inidata["general"]["jpegquality"] = udQuality.Value.ToString();
            _inidata["general"]["clearcache"] = chkClearCache.Checked.ToString();
            _inidata["general"]["showprogress"] = chkShowProgress.Checked.ToString();
            _inidata["general"]["scaleCaptured"] = chkScaleFactor.Checked.ToString();
            _inidata["general"]["scaleFactor"] = nudScaleFactor.Value.ToString();
            _inidata["general"]["scaleExternal"] = chkScaleExtern.Checked.ToString();
            _inidata["general"]["scaleFactorExternal"] = nudScaleExtern.Value.ToString();

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
            _inidata["general"]["autoloadFolder"] = _autoloadFolder;
            _inidata["general"]["autoloadrestoreapp"] = chkRestoreAutoLoad.Checked.ToString();
            _inidata["general"]["clearcacherecyclebin"] = chkRecycle.Checked.ToString();
            _inidata["general"]["zoomimage"] = chkZoom.Checked.ToString();
            _inidata["general"]["processedimage"] = chkProcessed.Checked.ToString();

            _config.WriteFile(_inifile, _inidata);
        }

        private void OpenFile(string file, ImageType imagetype)
        {
            p_imagepath = file;

            if (imagetype == ImageType.Dontknow)
            {
                var folder = Path.GetDirectoryName(file);
                imagetype = string.Compare(folder, GetCacheDir(), true) == 0 ? ImageType.Snapped : ImageType.External;
            }

            // update marker
            bool snapped = imagetype == ImageType.Snapped;
            lbMarkerSnap.Visible = snapped;
            lbMarkerExtern.Visible = !snapped;

            _imgOriginal.FromFile(file);
            ProcessImage(_imgOriginal, _imgProcessed, imagetype);
            ShowImage();

            FileInfo fileInfo = new FileInfo(file);
            this.Text = "OCRGet - " + fileInfo.Name;
            p_imagesize = ((float)fileInfo.Length / 1024f / 1024f).ToString("0.00") + " MB";
            UiStatusMessage("Loaded " + p_imagesize, StatusMessageType.SM_Ok);

            if (chkAutorecognize.Checked)
            {
                int delay = (int)nudAutorecognize.Value;
                if (delay > 0)
                {
                    tmrAutorecognize.Stop();
                    tmrAutorecognize.Interval = delay;
                    tmrAutorecognize.Start();
                }
                else
                {
                    tmrAutorecognize_Tick(null, null);
                }
            }

            if (chkAutowrite.Checked)
            {
                int delay = (int)nudAutowrite.Value;
                if (delay > 0)
                {
                    tmrAutowrite.Stop();
                    tmrAutowrite.Interval = delay;
                    tmrAutowrite.Start();
                }
                else
                {
                    tmrAutowrite_Tick(null, null);
                }
            }
        }

        private void ShowImage()
        {
            if (_imgOriginal.Bitmap == null) return;

            // reset image scroll
            _imageScroll.X = 0;
            _imageScroll.Y = 0;
            panel1.ForceScroll(0, 0);

            pictureBox1.Image = chkProcessed.Checked ? _imgProcessed.Bitmap : _imgOriginal.Bitmap;
        }

        private void ProcessImage(MemoryImage from, MemoryImage to, ImageType imagetype)
        {
            var chk = imagetype == ImageType.External ? chkScaleExtern : chkScaleFactor;
            var nud = imagetype == ImageType.External ? nudScaleExtern : nudScaleFactor;
            if (chk.Checked && nud.Value != 1)
            {
                // scale image
                using (Bitmap result = ImageHelper.ResizeImage(from.Bitmap,
                    (int)(from.Bitmap.Width * (float)nud.Value),
                    (int)(from.Bitmap.Height * (float)nud.Value)))
                {
                    var ms = new MemoryStream(); // stream is stored in MemoryImage
                    Jpeg.Save(result, ms, 100);
                    to.FromStream(ms);
                }
            }
            else
            {
                to.From(from);
            }
        }

        private void tmrAutorecognize_Tick(object sender, EventArgs e)
        {
            tmrAutorecognize.Stop();
            Recognize(p_ocr);
        }

        private void tmrAutowrite_Tick(object sender, EventArgs e)
        {
            tmrAutowrite.Stop();
            btnRewrite_Click(null, null);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Images (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                OpenFile(dlg.FileName, ImageType.Dontknow);
            }
        }

        void formn1_Paint(object sender, PaintEventArgs e)
        {
            var form = sender as Form;
            var text = form.Tag as string;
            SizeF textSize = e.Graphics.MeasureString(text, form.Font);
            _formn1.Left = Screen.PrimaryScreen.WorkingArea.Left;
            _formn1.Top = Screen.PrimaryScreen.WorkingArea.Top + Screen.PrimaryScreen.WorkingArea.Height - (int)textSize.Height;
            _formn1.Width = (int)textSize.Width;
            _formn1.Height = (int)textSize.Height;
            e.Graphics.DrawString(text, form.Font, Brushes.Lime, 0, 0);
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
                RecognizeFinish(true);
                return;
            }
            var stream = new MemoryStream(_imgProcessed.Bytes);
            var decoder = await BitmapDecoder.CreateAsync(stream.AsRandomAccessStream());
            var bitmap = await decoder.GetSoftwareBitmapAsync();
            var engine = OcrEngine.TryCreateFromLanguage(language);
            var ocrResult = await engine.RecognizeAsync(bitmap).AsTask();
            p_resulttext = ocrResult.Text;
            bitmap.Dispose();
            stream.Dispose();
            RecognizeFinish(false);
        }

        private void RecognizeOcrSpace()
        {
            if (chkShowProgress.Checked)
            {
                _formn1 = new FormDraw();
                _formn1.Tag = "Requesting OCR responce...";
                _formn1.Font = new Font(FontFamily.GenericSansSerif, 11, FontStyle.Bold);
                _formn1.Left = 0; _formn1.Top = 0; _formn1.Width = 1; _formn1.Height = 1;
                _formn1.Opacity = 1.0;
                _formn1.BackColor = Color.Black;
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
            bool error = false;

            byte[] data = form._imgProcessed.Bytes;

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
                error = true;
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
                    error = true;
                    form.p_resulttext = "ERROR: " + fullResponse;
                }
            }
            catch
            {
                error = true;
                form.p_resulttext = "ERROR: " + fullResponse;
            }

        RecognizeEend:
            form.Invoke((MethodInvoker)delegate
            {
                form.lbLastAction.Text = "ocr.space -->";
                form.RecognizeFinish(error);
            });
        }

        private void RecognizeFinish(bool error)
        {
            if (_formn1 != null)
            {
                _formn1.Hide();
                _formn1.Dispose();
                _formn1 = null;
            }

            if (!error)
            {
                // remove linebreaks
                if (chkRemoveLinebreaks.Checked)
                    p_resulttext = p_resulttext.Replace("\n", "").Replace("\r", "");
                // remove spaces
                if (chkRemoveSpaces.Checked)
                    p_resulttext = p_resulttext.Replace(" ", "");
            }

            txtResult.Tag = error ? "error" : null;
            txtResult.Text = p_resulttext;
            btnOpen.Enabled = true;
            btnRegion.Enabled = true;
            btnRecognize.Enabled = true;
            UiStatusMessage(p_imagesize + " Done", StatusMessageType.SM_Ok);

            if (!error && chkAutocopy.Checked)
            {
                btnCopy_Click(this, null);
            }

            if (chkRestore.Checked && chkShowProgress.Checked && this.WindowState == FormWindowState.Minimized)
            {
                FormHelpers.BringWindowToFront(this.Handle, this.Visible);
            }
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
                    // get image from screen
                    g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
                    // save image to file
                    _watcher.EnableRaisingEvents = false;
                    p_imagepath = Path.Combine(GetCacheDir(), "snap" + DateTime.Now.ToString("yyyyMMdd-HHmmss-fff") + ".jpg");
                    Jpeg.Save(bmp, p_imagepath, (long)udQuality.Value);
                    if (chkAutoLoad.Checked)
                        _watcher.EnableRaisingEvents = true;
                }

                OpenFile(p_imagepath, ImageType.Snapped);

                if (chkRestore.Checked && !chkShowProgress.Checked)
                    FormHelpers.BringWindowToFront(this.Handle, this.Visible);
            }
            else // snap cancelled
            {
                if (chkRestore.Checked)
                    FormHelpers.BringWindowToFront(this.Handle, this.Visible);
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

        private string GetCacheDir()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache");
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
                    OpenFile(file, ImageType.Dontknow);
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

        const int WM_NCRBUTTONDOWN = 0x00A4;
        const int HTMINBUTTON = 8;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case (int)WindowsMessages.HOTKEY:
                    {
                        //ushort id = (ushort)m.WParam;
                        //Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                        //Modifiers modifier = (Modifiers)((int)m.LParam & 0xFFFF);
                        //OnKeyPressed(id, key, modifier);

                        // only one hotkey yet so no need to check for modifiers/keys
                        if (btnRegion.Enabled && _formd1 == null)
                            btnRegion_Click(this, null);
                    }
                    break;
                case WM_SYSCOMMAND:
                    switch ((int)m.WParam)
                    {
                        case SYSMENU_EXPLORE_CACHE:
                            {
                                var psi = new ProcessStartInfo
                                {
                                    FileName = GetCacheDir(),
                                    UseShellExecute = true
                                };
                                Process.Start(psi);
                            }
                            break;
                        case SYSMENU_ABOUTBOX:
                            MessageBox.Show($"OCRGet v{Application.ProductVersion.Remove(Application.ProductVersion.LastIndexOf("."))}" +
                                "\n\nRightClick minimize button - hide to tray." +
                                "\n\nCommand line:" +
                                "\n    /tray - start hidden to tray." +
                                "\n\nNote: When hidden to tray \"Restore app\" options have no effect.", "About...", MessageBoxButtons.OK);
                            break;
                    }
                    break;
                case WM_NCRBUTTONDOWN: // non client area right click
                    if (m.WParam == (IntPtr)HTMINBUTTON) // minimize button
                    {
                        UiHide();
                        return; // do not process
                    }
                    break;
            }

            base.WndProc(ref m);
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

        private void UiHide()
        {
            this.Hide();
            notifyIcon1.Visible = true;
        }

        private void UiShow()
        {
            _allowFormShow = true;
            notifyIcon1.Visible = false;
            this.Show();
            this.Restore();
            this.BringToFront();
            this.Activate();
            FormHelpers.BringWindowToFront(this.Handle, this.Visible);
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
            if ((string)txtResult.Tag == "error") return; // don't mangle error mesage

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
            if (_imgProcessed.Bitmap == null) return;

            // make file name
            string dir = Path.GetDirectoryName(p_imagepath);
            string name = Path.GetFileNameWithoutExtension(p_imagepath) + "processed";
            string path = Path.Combine(dir, name + ".jpg");
            // save file
            _watcher.EnableRaisingEvents = false;
            Jpeg.Save(_imgProcessed.Bitmap, path, (long)udQuality.Value);
            if (chkAutoLoad.Checked)
                _watcher.EnableRaisingEvents = true;

            UiStatusMessage("Written: " + Path.GetFileName(path), StatusMessageType.SM_Ok);
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
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_EXPLORE_CACHE, "&Explore cache folder");
            AppendMenu(hSysMenu, MF_STRING, SYSMENU_ABOUTBOX, "About/help...");
        }

        private void chkAutoLoad_CheckedChanged(object sender, EventArgs e)
        {
            if (_isBooting) return;
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
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        tmrAutoload.Stop();
                        _autoloadFile = e.FullPath;
                        tmrAutoload.Start();
                    }));
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
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        tmrAutoload.Stop();
                        _autoloadFile = e.FullPath;
                        tmrAutoload.Start();
                    }));
                    break;
            }
        }

        private void tmrAutoload_Tick(object sender, EventArgs e)
        {
            tmrAutoload.Stop();

            var extension = Path.GetExtension(_autoloadFile).ToLower();
            if (!string.IsNullOrEmpty(extension) && _imageExtensions.Any(ext => ext == extension))
            {
                OpenFile(_autoloadFile, ImageType.External);
                if (chkRestoreAutoLoad.Checked)
                {
                    FormHelpers.BringWindowToFront(this.Handle, this.Visible);
                }
            }
        }

        private void chkZoom_CheckedChanged(object sender, EventArgs e)
        {
            if (chkZoom.Checked)
            {
                _imageScroll.Y = panel1.VerticalScroll.Value;
                _imageScroll.X = panel1.HorizontalScroll.Value;
                panel1.ForceScroll(0, 0);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Width = panel1.Size.Width - 2;
                pictureBox1.Height = panel1.Size.Height - 2;
                pictureBox1.Anchor |= AnchorStyles.Right | AnchorStyles.Bottom;
            }
            else
            {
                pictureBox1.Anchor ^= AnchorStyles.Right | AnchorStyles.Bottom;
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                panel1.ForceScroll(_imageScroll.Y, _imageScroll.X);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            chkZoom.Checked = pictureBox1.SizeMode != PictureBoxSizeMode.Zoom;
        }

        private void chkProcessed_CheckedChanged(object sender, EventArgs e)
        {
            ShowImage();
        }

        private void btnLoadFromFolder_Click(object sender, EventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.EnsurePathExists = true;
            dialog.Multiselect = false;
            dialog.DefaultDirectory = _autoloadFolder;
            dialog.InitialDirectory = _autoloadFolder;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                _autoloadFolder = dialog.FileName;
                toolTip1.SetToolTip(btnLoadFromFolder, _autoloadFolder);
                InitFSwatcher(_autoloadFolder);
            }
        }

        /// <summary>
        /// Fix mouse wheel as single increment instead of multiple
        /// </summary>
        private void NudScrollHandler(object sender, MouseEventArgs e)
        {
            NumericUpDown control = (NumericUpDown)sender;
            ((HandledMouseEventArgs)e).Handled = true;
            decimal value = control.Value + ((e.Delta > 0) ? control.Increment : -control.Increment);
            control.Value = Math.Max(control.Minimum, Math.Min(value, control.Maximum));
        }

        private void nudScaleFactor_ValueChanged(object sender, EventArgs e)
        {
            if (!lbMarkerSnap.Visible && !lbMarkerExtern.Visible) return;
            ImageType type = lbMarkerSnap.Visible ? ImageType.Snapped : ImageType.External;
            if (type == ImageType.Snapped && (sender != nudScaleFactor && sender != chkScaleFactor)) return;
            if (type == ImageType.External && (sender != nudScaleExtern && sender != chkScaleExtern)) return;

            ProcessImage(_imgOriginal, _imgProcessed, type);
            ShowImage();
            chkZoom.Checked = false;
            chkProcessed.Checked = true;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                UiShow();
            }
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(_allowFormShow ? value : false);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UiShow();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    } // Form1

    internal static class FormHelpers
    {
        /// <summary>
        /// Panel extension method fixing scrollbars not properly updated by default
        /// </summary>
        public static void ForceScroll(this Panel panel, int v, int h)
        {
            panel.VerticalScroll.Value = v;
            panel.VerticalScroll.Value = v;
            panel.HorizontalScroll.Value = h;
            panel.HorizontalScroll.Value = h;
        }

        public static bool BringWindowToFront(IntPtr hwnd, bool doit)
        {
            uint lForeThreadID;
            uint lThisThreadID;
            bool lReturn = true;

            if (!doit) return lReturn;

            // Make a window, specified by its handle (hwnd)
            // the foreground window.

            // If it is already the foreground window, exit.
            if (hwnd != NativeMethods.GetForegroundWindow())
            {

                // Get the threads for this window and the foreground window.
                lForeThreadID = NativeMethods.GetWindowThreadProcessId(NativeMethods.GetForegroundWindow(), out _);
                lThisThreadID = NativeMethods.GetWindowThreadProcessId(hwnd, out _);

                // By sharing input state, threads share their concept of
                // the active window.

                if (lForeThreadID != lThisThreadID)
                {
                    // Attach the foreground thread to this window.
                    Winapi.AttachThreadInput(lForeThreadID, lThisThreadID, true);
                    // Make this window the foreground window.
                    lReturn = NativeMethods.SetForegroundWindow(hwnd);
                    // Detach the foreground window's thread from this window.
                    Winapi.AttachThreadInput(lForeThreadID, lThisThreadID, false);
                }
                else
                {
                    lReturn = NativeMethods.SetForegroundWindow(hwnd);
                }

                // Restore this window to its normal size.
                if (NativeMethods.IsIconic(hwnd))
                {
                    Winapi.ShowWindow(hwnd, WindowShowStyle.Restore);
                }
                else
                {
                    Winapi.ShowWindow(hwnd, WindowShowStyle.Show);
                }
            }
            return lReturn;
        }
    } // FormHelpers

    internal class MemoryImage
    {
        public byte[] Bytes { get; private set; }
        public Bitmap Bitmap { get; private set; }
        private MemoryStream _stream = null; // needed to save image
        private bool _referenced = false;

        public MemoryImage()
        {
            Bytes = null;
            Bitmap = null;
        }

        /// <summary>
        /// Resets _referenced flag.
        /// </summary>
        private void Free()
        {
            _referenced = false;
            Bytes = null;
            Bitmap?.Dispose();
            Bitmap = null;
            _stream?.Dispose();
            _stream = null;
        }

        public void FromFile(string path)
        {
            if (!_referenced)
            {
                Free();
            }
            Bytes = File.ReadAllBytes(path);
            _stream = new MemoryStream(Bytes);
            Bitmap = new Bitmap(_stream);
        }

        /// <summary>
        /// Take stream and keep it.
        /// </summary>
        public void FromStream(MemoryStream stream)
        {
            if (!_referenced)
            {
                Free();
            }
            Bytes = stream.ToArray();
            Bitmap = new Bitmap(stream);
            _stream = stream;
        }

        /// <summary>
        /// Assignment by reference!
        /// </summary>
        public void From(MemoryImage image)
        {
            if (!_referenced)
            {
                Free();
            }
            _referenced = true;
            Bytes = image.Bytes;
            Bitmap = image.Bitmap;
            _stream = image._stream;
        }
    } // MemoryImage
}

namespace System.Windows.Forms
{
    public static class Extensions
    {
        public static void Restore(this Form form)
        {
            if (form.WindowState == FormWindowState.Minimized)
            {
                NativeMethods.ShowWindow(form.Handle, (int)WindowShowStyle.Restore);
            }
        }
    }
}
