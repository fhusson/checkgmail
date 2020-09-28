using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace CheckGMail
{
    [SupportedOSPlatform("windows")]
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var settings = new SettingsForm(false))
            {
                Application.Run();
            }
        }
    }
}
