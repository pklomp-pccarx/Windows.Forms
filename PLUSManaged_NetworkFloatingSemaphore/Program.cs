using System;
using System.Windows.Forms;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Program class</summary>
    /// <note type="caution"><para>IMPORTANT: When using Network Floating Licensing features (via the NetworkSemaphore class) with protected applications, Windows Vista/Server 2008 or later is required for both the client computers accessing the protected application, and the file server hosting the share where the semaphore files will be stored.</para></note>
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
