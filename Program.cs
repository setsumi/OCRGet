using System;
using System.Threading;
using System.Windows.Forms;

namespace OCRGet
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // fix region snap and blurred UI
            Winapi.SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!System.Diagnostics.Debugger.IsAttached)
            {
                Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }

            Application.Run(new Form1());
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Helpers.MessageError((e.ExceptionObject as Exception).Message, "Unhandled UI Exception");
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Helpers.MessageError(e.Exception.Message, "Unhandled Thread Exception");
        }
    }
}
