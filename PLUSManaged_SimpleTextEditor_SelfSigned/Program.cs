using System;
using System.Windows.Forms;

namespace com.softwarekey.Client.Sample
{
    /// <summary>The main entry point Program class implementation</summary>
    static class Program
    {
        #region Public Static Methods
        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        #endregion
    }
}
