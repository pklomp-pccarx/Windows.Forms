using System;
using System.Windows.Forms;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Program class</summary>
    public static class Program
    {
        #region Public Static Methods
        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        #endregion
    }
}