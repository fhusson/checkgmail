﻿using System;
using System.Windows.Forms;

namespace CheckGMail
{
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
            
            _ = new SettingsForm()
            {
                Visible = false
            };
            Application.Run();
        }
    }
}
