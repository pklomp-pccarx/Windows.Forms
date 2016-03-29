using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Network license browse for folder dialog</summary>
    /// <note type="caution"><para>IMPORTANT: When using Network Floating Licensing features (via the NetworkSemaphore class) with protected applications, Windows Vista/Server 2008 or later is required for both the client computers accessing the protected application, and the file server hosting the share where the semaphore files will be stored.</para></note>
    public partial class NetworkLicenseBrowseForm : Form
    {
        #region Private Member Variables
        private string m_SelectedPath = "";
        #endregion

        #region Public Properties
        /// <summary>The path selected by the user.</summary>
        public string SelectedPath
        {
            get { return m_SelectedPath; }
            set { m_SelectedPath = value; }
        }
        #endregion

        #region Public Methods
        /// <summary>Network license browse dialog</summary>
        public NetworkLicenseBrowseForm()
        {
            InitializeComponent();

            try
            {
                pathTextBox.Text = LicenseConfiguration.PathRegistryValue;

                if (!string.IsNullOrEmpty(pathTextBox.Text))
                {
                    pathTextBox.ForeColor = Color.Black;
                    okButton.Focus();
                }
            }
            catch { }
        }
        #endregion

        #region Private Methods
        /// <summary>Ok button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void okButton_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(pathTextBox.Text))
            {
                errorProvider.SetError(pathTextBox, "The specified path is invalid or unavailable.");
                invalidPathLabel.Visible = true;
            }
            else
            {
                m_SelectedPath = pathTextBox.Text;

                try
                {
                    LicenseConfiguration.PathRegistryValue = m_SelectedPath;
                }
                catch { }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>Browse button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void browseButton_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            invalidPathLabel.Visible = false;

            FolderBrowserDialog fbDlg = new FolderBrowserDialog();

            if (fbDlg.ShowDialog() == DialogResult.OK)
            {
                pathTextBox.Text = fbDlg.SelectedPath;

                pathTextBox.ForeColor = Color.Black;
                okButton.Focus();
            }
        }

        /// <summary>Text box path click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void pathTextBox_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            invalidPathLabel.Visible = false;

            if (pathTextBox.Text == @"Example: \\server\share")
            {
                pathTextBox.Text = "";
                pathTextBox.ForeColor = Color.Black;
            }
        }

        /// <summary>Simple open file folder dialog click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void SimpleOpenFileFolderDialog_Click(object sender, EventArgs e)
        {
            if (pathTextBox.Text == "")
            {
                pathTextBox.Text = @"Example: \\server\share";
                pathTextBox.ForeColor = Color.DarkGray;
                
                if (pathTextBox.Focused)
                    browseButton.Focus();
            }
        }

        /// <summary>Text box path enter event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void pathTextBox_Enter(object sender, EventArgs e)
        {
            if (pathTextBox.Text == @"Example: \\server\share")
            {
                pathTextBox.Text = "";
                pathTextBox.ForeColor = Color.Black;
            }
        }
        #endregion
    }
}
