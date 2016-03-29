using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Network license browse for folder dialog</summary>
    public partial class NetworkLicenseBrowseForm : Form
    {
        #region Private Member Variables
        private string m_selectedPath = "";
        #endregion

        #region Constructors
        /// <summary>Network license browse dialog</summary>
        public NetworkLicenseBrowseForm()
        {
            InitializeComponent();

            try
            {
                pathTextBox.Text = LicenseConfiguration.PathRegistryValue;
                pathTextBox.ForeColor = SystemColors.ControlText;
                OKButton.Focus();
            }
            catch { }
        }
        #endregion

        #region Public Properties
        /// <summary>The path selected by the user.</summary>
        public string SelectedPath
        {
            get { return m_selectedPath; }
            set { m_selectedPath = value; }
        }
        #endregion

        #region Event Handlers
        /// <summary>Ok button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(pathTextBox.Text))
            {
                errorProvider.SetError(pathTextBox, "The specified path is invalid or unavailable.");
                invalidPathLabel.Visible = true;
            }
            else
            {
                m_selectedPath = pathTextBox.Text;

                try
                {
                    LicenseConfiguration.PathRegistryValue = m_selectedPath;
                }
                catch { }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>Browse button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            invalidPathLabel.Visible = false;

            using (FolderBrowserDialog fbDlg = new FolderBrowserDialog())
            {
                if (fbDlg.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = fbDlg.SelectedPath;

                    pathTextBox.ForeColor = SystemColors.ControlText;
                    OKButton.Focus();
                }
            }
        }

        /// <summary>Text box path click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void textBoxPath_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            invalidPathLabel.Visible = false;

            if (pathTextBox.Text == @"Example: \\server\share")
            {
                pathTextBox.Text = "";
                pathTextBox.ForeColor = SystemColors.ControlText;
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
                pathTextBox.ForeColor = SystemColors.GrayText;
                
                if (pathTextBox.Focused)
                    browseButton.Focus();
            }
        }

        /// <summary>Text box path enter event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void textBoxPath_Enter(object sender, EventArgs e)
        {
            if (pathTextBox.Text == @"Example: \\server\share")
            {
                pathTextBox.Text = "";
                pathTextBox.ForeColor = SystemColors.ControlText;
            }
        }
        #endregion
    }
}
