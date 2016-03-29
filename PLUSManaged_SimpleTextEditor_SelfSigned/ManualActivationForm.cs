using System;
using System.Windows.Forms;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Form for activating manually via SOLO Server or License Manager.</summary>
    public partial class ManualActivationForm : Form
    {
        #region Private Member Variables
        private MainForm m_MainDialog;
        private SampleLicense m_License;
        #endregion

        #region Constructors
        /// <summary>Creates a new <see cref="ManualActivationForm"/>.</summary>
        /// <param name="mainDialog">A reference to the <see cref="MainForm"/>.</param>
        /// <param name="license">The license object being activated.</param>
        public ManualActivationForm(MainForm mainDialog, SampleLicense license)
        {
            m_MainDialog = mainDialog;
            m_License = license;
            InitializeComponent();
        }
        #endregion

        #region Private Event Handler Methods
        /// <summary>Activate button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void activateButton_Click(object sender, EventArgs e)
        {
            string lfContent = "";
            bool successful = false;

            activateButton.Enabled = false;
            Cursor = Cursors.WaitCursor;

            //TODO: set the value of m_license.CurrentSessionCode to the value stored when the Generate button was clicked
            successful = m_License.ProcessActivateInstallationLicenseFileResponse(activationCodeTextBox.Text, ref lfContent);

            activateButton.Enabled = true;
            Cursor = Cursors.Default;

            //TODO: clear the stored session code value here
            if (!successful)
            {
                MessageBox.Show(this, "Activation Failed.  Reason: " + m_License.LastError.ErrorString, "Manual Activation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (m_License.SaveLicenseFile(lfContent))
            {
                MessageBox.Show(this, "Activation Successful!", "Manual Activation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                m_License.ResetSessionCode();
                m_MainDialog.ReloadLicense();
                Close();
            }
            else
            {
                MessageBox.Show(this, "Activation Failed.  Reason: " + m_License.LastError.ErrorString, "Manual Activation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>Activation code text changed event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void activationCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            activateButton.Enabled = !string.IsNullOrEmpty(activationCodeTextBox.Text);
        }

        /// <summary>Activation Page button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void activationPageButton_Click(object sender, EventArgs e)
        {
            WebServiceHelper.OpenManualRequestUrl();
        }

        /// <summary>Copy button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void copyButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(activationRequestTextBox.Text, false, 100, 10);
        }

        /// <summary>Close button click event handler method.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>Generate Request button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void generateRequestButton_Click(object sender, EventArgs e)
        {
            Int32 licenseId = 0;
            string password = passwordTextBox.Text;
            string request = "";

            m_License.ResetSessionCode();
            //TODO: store the value of m_license.CurrentSessionCode in some hidden location

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

            generateRequestButton.Enabled = false;
            Cursor = Cursors.WaitCursor;

            request = m_License.GetActivationInstallationLicenseFileRequest(licenseId, password);
            activationRequestTextBox.Text = request;

            generateRequestButton.Enabled = true;
            Cursor = Cursors.Default;

            copyButton.Enabled = true;
            pasteButton.Enabled = true;
            activationPageButton.Enabled = true;
            activationCodeTextBox.Enabled = true;
        }

        /// <summary>Form shown event handler.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void ManualActivationForm_Shown(object sender, EventArgs e)
        {
            if (m_License.LicenseID > 0)
            {
                licenseIDTextBox.Text = m_License.LicenseID.ToString();
                passwordTextBox.Focus();
            }
            else
            {
                licenseIDTextBox.Focus();
            }
        }

        /// <summary>Paste button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void pasteButton_Click(object sender, EventArgs e)
        {
            activationCodeTextBox.Text = Clipboard.GetText();
        }
        #endregion
    }
}
