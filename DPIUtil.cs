using System;
using System.Drawing;
using System.Windows.Forms;

namespace OCRGet
{
    public static class DPIUtil
    {
        private static void GetMonitorResolution(Control control, out int resX, out int resY)
        {
            var hdc = Winapi.CreateDC(Screen.FromControl(control).DeviceName, "", "", IntPtr.Zero);
            resX = Winapi.GetDeviceCaps(hdc, Winapi.DESKTOPHORZRES);
            resY = Winapi.GetDeviceCaps(hdc, Winapi.DESKTOPVERTRES);
            Winapi.DeleteDC(hdc);
        }

        private static void GetMonitorScaleFactor(Control control, out double scaleX, out double scaleY)
        {
            GetMonitorResolution(control, out int resX, out int resY);
            Rectangle bounds = Screen.FromControl(control).Bounds;
            scaleX = (double)resX / (double)bounds.Width;
            scaleY = (double)resY / (double)bounds.Height;
        }

        public static Point CorrectMousePos(Control control, int mouseX, int mouseY)
        {
            GetMonitorScaleFactor(control, out double scaleX, out double scaleY);
            return control.PointToScreen(new Point((int)(mouseX * scaleX), (int)(mouseY * scaleY)));
        }
    }
}
