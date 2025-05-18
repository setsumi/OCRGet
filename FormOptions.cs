using ShareX.HelpersLib;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OCRGet
{
    public partial class FormOptions : Form
    {
        private Keys _capturedKeyCode = Keys.None;
        private Modifiers _capturedModifiers = Modifiers.None;

        public Keys SelectedKeyCode { get; private set; } = Keys.None;
        public Modifiers SelectedModifiers { get; private set; } = Modifiers.None;
        public bool UseProxy { get; private set; } = false;
        public string ProxyHost { get; private set; } = "";
        public int ProxyPort { get; private set; } = 1;

        public FormOptions(Keys snapKey, Modifiers snapKeyModifiers, bool useProxy, string proxyHost, int proxyPort)
        {
            InitializeComponent();

            SelectedKeyCode = snapKey;
            SelectedModifiers = snapKeyModifiers;
            txtHotkeyDisplay.Text = "Click here and press the desired hotkey...";
            UpdateCurrentRegisteredDisplay();

            UseProxy = useProxy;
            ProxyHost = proxyHost;
            ProxyPort = proxyPort;
            chkProxy.Checked = useProxy;
            txtProxyHost.Text = proxyHost;
            udProxyPort.Value = proxyPort;
            chkProxy_CheckedChanged(null, null);
        }

        private void UpdateCurrentRegisteredDisplay()
        {
            if (SelectedKeyCode != Keys.None)
            {
                lblCurrentRegistered.Text = $"Currently: {FormatHotkeyInputString(SelectedModifiers, SelectedKeyCode)}";
                chkWinkey.Checked = (SelectedModifiers & Modifiers.Win) == Modifiers.Win;
            }
            else
            {
                lblCurrentRegistered.Text = "Currently: None";
            }
        }

        private string FormatHotkeyInputString(Modifiers modifiers, Keys keyCode)
        {
            List<string> parts = new List<string>();
            if ((modifiers & Modifiers.Control) == Modifiers.Control) parts.Add("Ctrl");
            if ((modifiers & Modifiers.Alt) == Modifiers.Alt) parts.Add("Alt");
            if ((modifiers & Modifiers.Shift) == Modifiers.Shift) parts.Add("Shift");
            if ((modifiers & Modifiers.Win) == Modifiers.Win) parts.Add("Win");

            if (keyCode != Keys.None)
            {
                if (parts.Count == 0 && keyCode == Keys.Back)
                {
                    return "Disable hotkey";
                }
                else
                {
                    parts.Add(new KeysConverter().ConvertToString(keyCode));
                }
            }

            if (parts.Count == 0) // No key and no modifiers recorded
            {
                return "Press desired hotkey...";
            }

            return string.Join(" + ", parts);
        }


        private void txtHotkeyDisplay_Enter(object sender, EventArgs e)
        {
            _capturedModifiers = Modifiers.None;
            _capturedKeyCode = Keys.None;
            txtHotkeyDisplay.Text = FormatHotkeyInputString(_capturedModifiers, _capturedKeyCode);
        }

        private void txtHotkeyDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            // Prevent the key from being processed by the TextBox itself
            e.SuppressKeyPress = true;
            e.Handled = true;

            _capturedModifiers = Modifiers.None;
            _capturedKeyCode = Keys.None;

            if (e.Control) _capturedModifiers |= Modifiers.Control;
            if (e.Alt) _capturedModifiers |= Modifiers.Alt;
            if (e.Shift) _capturedModifiers |= Modifiers.Shift;
            // The Windows key (Win) is harder to capture reliably with e.Modifiers.
            if (chkWinkey.Checked) _capturedModifiers |= Modifiers.Win;

            // Check if the key pressed is a modifier key itself
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey ||
                e.KeyCode == Keys.Menu || // Alt key
                e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
            {
                // Only a modifier was pressed, don't set it as the main key code
                // but show it in the textbox.
                _capturedKeyCode = Keys.None;
            }
            else if (e.KeyCode != Keys.None)
            {
                _capturedKeyCode = e.KeyCode;
            }

            txtHotkeyDisplay.Text = FormatHotkeyInputString(_capturedModifiers, _capturedKeyCode);
        }

        private void chkWinkey_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as CheckBox).Checked)
            {
                _capturedModifiers |= Modifiers.Win;
            }
            else
            {
                _capturedModifiers &= ~Modifiers.Win;
            }
            txtHotkeyDisplay.Text = "Click here and press the desired hotkey...";
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            // validate hotkey input
            if (_capturedKeyCode == Keys.None && _capturedModifiers != Modifiers.None && _capturedModifiers != Modifiers.Win)
            {
                MessageBox.Show("Please press a valid key combination that includes a non-modifier key (e.g., A, F5, Space).",
                                "Invalid Hotkey", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // remove Win from checkbox if alone
            if (_capturedKeyCode == Keys.None && _capturedModifiers == Modifiers.Win)
            {
                _capturedModifiers = Modifiers.None;
            }

            SelectedKeyCode = _capturedKeyCode;
            SelectedModifiers = _capturedModifiers;

            // validate proxy input
            if (UseProxy && string.IsNullOrEmpty(ProxyHost))
            {
                MessageBox.Show("Please input a valid proxy host.", "Invalid Proxy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chkWinkey.Checked = false;
            _capturedKeyCode = Keys.Back;
            _capturedModifiers = Modifiers.None;
            txtHotkeyDisplay.Text = FormatHotkeyInputString(_capturedModifiers, _capturedKeyCode);
        }

        private void chkProxy_CheckedChanged(object sender, EventArgs e)
        {
            bool chk = chkProxy.Checked;
            UseProxy = chk;
            txtProxyHost.Enabled = chk;
            udProxyPort.Enabled = chk;
        }

        private void txtProxyHost_TextChanged(object sender, EventArgs e)
        {
            ProxyHost = txtProxyHost.Text;
        }

        private void udProxyPort_ValueChanged(object sender, EventArgs e)
        {
            ProxyPort = (int)udProxyPort.Value;
        }
    }
}
