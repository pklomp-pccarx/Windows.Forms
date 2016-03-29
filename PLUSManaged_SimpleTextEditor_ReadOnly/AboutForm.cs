using System;
using System.Drawing;
using System.Windows.Forms;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Different status icons.</summary>
    internal enum LicenseStatusIcon
    {
        /// <summary>Don't display any icon in the list for this entry.</summary>
        None = -1,
        /// <summary>The status of the entry is Ok.</summary>
        Ok = 0,
        /// <summary>The entry's status is invalid, possibly due to an error.</summary>
        Error = 1,
        /// <summary>The entry's status is Ok, but may include some additional information worth informing the user about.</summary>
        Information = 2,
        /// <summary>The item, module, or feature is unavailable or disabled.</summary>
        Unavailable = 3,
        /// <summary>The entry's status is Ok, but requires the user's attention.</summary>
        Warning = 4
    }

    /// <summary>AboutForm form class implementation</summary>
    public partial class AboutForm : Form
    {
        #region Private Member Variables
        private MainForm m_MainForm = null;
        #endregion

        #region Public Constructors
        /// <summary>Creates a new <see cref="AboutForm"/>.</summary>
        /// <param name="mainForm">A reference to the <see cref="MainForm"/>.</param>
        public AboutForm(MainForm mainForm)
        {
            m_MainForm = mainForm;
            InitializeComponent();
        }
        #endregion

        #region Private Event Handler Methods
        /// <summary>Form load implementation</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void AboutForm_Load(object sender, EventArgs e)
        {
            versionLabel.Text += LicenseConfiguration.ThisProductVersion;

            LoadStatus();
        }

        /// <summary>OK button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>Activate Online button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void activateOnlineButton_Click(object sender, EventArgs e)
        {
            using (OnlineActivationForm activationDialog = new OnlineActivationForm(m_MainForm, m_MainForm.License))
            {
                activationDialog.ShowDialog(this);
            }
            LoadStatus();
        }

        /// <summary>Activate Manually button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void activateManuallyButton_Click(object sender, EventArgs e)
        {
            using (ManualActivationForm activationDialog = new ManualActivationForm(m_MainForm, m_MainForm.License))
            {
                activationDialog.ShowDialog(this);
            }
            LoadStatus();
        }

        /// <summary>Deactivate button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void deactivateButton_Click(object sender, EventArgs e)
        {
            if (m_MainForm.License.DeactivateOnline())
            {
                MessageBox.Show(this, "The license has been deactivated successfully.", "Deactivation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "The license was not deactivated.  Error: (" + m_MainForm.License.LastError.ErrorNumber + ")" + m_MainForm.License.LastError.ErrorString, "Deactivation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            m_MainForm.ReloadLicense();
            LoadStatus();
        }

        /// <summary>Refresh License button click event handler</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void refreshLicenseButton_Click(object sender, EventArgs e)
        {
            if (m_MainForm.License.RefreshLicense())
            {
                MessageBox.Show(this, "The license has been refreshed successfully.", "License Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "The license was not refreshed.  Error: (" + m_MainForm.License.LastError.ErrorNumber + ")" + m_MainForm.License.LastError.ErrorString, "License Refresh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            m_MainForm.ReloadLicense();
            LoadStatus();
        }
        #endregion

        #region Private Methods
        /// <summary>Loads the status</summary>
        private void LoadStatus()
        {
            registrationInfoLabel.Text = m_MainForm.LicenseRegistrationInfo;
            statusListView.Items.Clear();

            ListViewItem licenseItem = new ListViewItem("License");
            licenseItem.ImageIndex = (int)(m_MainForm.IsLicenseValid ? LicenseStatusIcon.Ok : LicenseStatusIcon.Error);
            licenseItem.SubItems.Add(m_MainForm.LicenseStatus);
            licenseItem.ToolTipText = m_MainForm.LicenseStatus;
            statusListView.Items.Add(licenseItem);

            foreach (Feature f in m_MainForm.AllFeatures.ListFeatures.Values)
            {
                ListViewItem item = new ListViewItem(f.DisplayName);
                item.ImageIndex = (int)(f.Enabled ? LicenseStatusIcon.Ok : LicenseStatusIcon.Unavailable);
                string status = (f.Enabled ? "Enabled" : "Disabled");
                item.SubItems.Add(status);
                item.ToolTipText = status;
                statusListView.Items.Add(item);
            }

            statusListView.Refresh();

            refreshLicenseButton.Visible = !string.IsNullOrEmpty(m_MainForm.License.InstallationID);
            deactivateButton.Visible = !string.IsNullOrEmpty(m_MainForm.License.InstallationID);

            if (m_MainForm.License.LastError.ErrorNumber == Licensing.LicenseError.ERROR_PLUS_EVALUATION_INVALID)
            {
                activateOnlineButton.Enabled = false;
                activateManuallyButton.Enabled = false;
                refreshLicenseButton.Enabled = false;
            }
        }
        #endregion
    }
}