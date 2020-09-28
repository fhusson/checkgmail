using Microsoft.Win32;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace CheckGMail
{
    [SupportedOSPlatform("windows")]
    static public class StartApplicationOnWindowsStartup
    {
        private const string PATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";

        static public void Enable()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(PATH, true))
            {
                key.SetValue(Application.ProductName, Application.ExecutablePath);
            }
        }

        static public void Disable()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(PATH, true))
            {
                key.DeleteValue(Application.ProductName, false);
            }
        }

        static public bool IsEnabled()
        {
            bool result = false;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(PATH, false))
            {
                result = (key.GetValue(Application.ProductName) != null);
            }

            return result;
        }
    }
}
