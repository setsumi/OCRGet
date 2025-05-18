using ShareX.HelpersLib;
using System;
using System.Runtime.InteropServices;

namespace OCRGet
{
    internal static class Winapi
    {
        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool SetForegroundWindow(IntPtr hWnd);

        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, [MarshalAs(UnmanagedType.Bool)] bool fAttach);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        //[DllImport("user32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool IsIconic(IntPtr hWnd);

        //[DllImport("gdi32.dll")]
        //public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);

        //[DllImport("gdi32.dll")]
        //public static extern bool DeleteDC(IntPtr hDC);

        //public const int DESKTOPVERTRES = 117;
        //public const int DESKTOPHORZRES = 118;
        //[DllImport("gdi32.dll")]
        //public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();
    }
}
