using ShareX.HelpersLib;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OCRGet
{
    internal static class Helpers
    {
        public static void MessageError(string message, string title = "OCRGet")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        public static string FormatHotkeyString(Modifiers modifiers, Keys keyCode)
        {
            List<string> parts = new List<string>();
            if ((modifiers & Modifiers.Control) == Modifiers.Control) parts.Add("Ctrl");
            if ((modifiers & Modifiers.Alt) == Modifiers.Alt) parts.Add("Alt");
            if ((modifiers & Modifiers.Shift) == Modifiers.Shift) parts.Add("Shift");
            if ((modifiers & Modifiers.Win) == Modifiers.Win) parts.Add("Win");

            if (keyCode != Keys.None)
            {
                parts.Add(new KeysConverter().ConvertToString(keyCode));
            }

            if (parts.Count == 0)
            {
                return "None";
            }

            return string.Join("+", parts);
        }
    }
}
