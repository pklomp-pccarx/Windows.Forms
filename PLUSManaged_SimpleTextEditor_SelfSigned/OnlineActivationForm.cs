using System;
using System.Windows.Forms;
using com.softwarekey.Client.Licensing;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Form for activating a protected application online via SOLO Server.</summary>
    public partial class OnlineActivationForm : DevExpress.XtraEditors.XtraForm
    {
        #region Private Member Variables
        private MainForm m_mainDialog;
        private SampleLicense m_license;
        #endregion

        #region Constructors
        /// <summary>Creates a new <see cref="OnlineActivationForm"/>.</summary>
        /// <param name="mainDialog">A reference to the <see cref="MainForm"/>.</param>
        /// <param name="license">The license object being activated.</param>
        public OnlineActivationForm(MainForm mainDialog, SampleLicense license)
        {
            m_mainDialog = mainDialog;
            m_license = license;

            InitializeComponent();
        }
        #endregion

        #region Private Methods
        /// <summary>Cancel button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>Activate button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void activateButton_Click(object sender, EventArgs e)
        {
            Int32 licenseId = 0;
            string password = passwordTextBox.Text;
            bool successful = false;

            if (string.IsNullOrEmpty(licenseIDTextBox.Text))
            {
                MessageBox.Show(this, "Please enter a License ID.", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                licenseIDTextBox.Focus();
                return;
            }

            if (!Int32.TryParse(licenseIDTextBox.Text, out licenseId))
            {
                MessageBox.Show(this, "The License ID may only contain numbers.", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                licenseIDTextBox.Focus();
                return;
            }

            if (!Int32.TryParse(licenseIDTextBox.Text, out licenseId))
            {
                MessageBox.Show(this, "The License ID may only contain numbers.", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                licenseIDTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                MessageBox.Show(this, "Enter your password.", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                passwordTextBox.Focus();
                return;
            }

            m_license.InstallationName = installationNameTextBox.Text;

            successful = m_license.ActivateOnline(licenseId, password);
            
            if (successful)
            {
                MessageBox.Show(this, "Activation Successful!", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                m_mainDialog.ReloadLicense();
                Close();
            }
            else
            {
                MessageBox.Show(this, "Activation Failed." + Environment.NewLine + Environment.NewLine + m_license.LastError.ToString(), "Activation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>Form shown event handler.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void OnlineActivationForm_Shown(object sender, EventArgs e)
        {
            if (m_license.LicenseID > 0)
            {
                licenseIDTextBox.Text = m_license.LicenseID.ToString();
                passwordTextBox.Focus();
            }
            else
            {
                licenseIDTextBox.Focus();
            }
        }
        #endregion
    }
}
