using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.Licensing.Network;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Main form</summary>
    /// <note type="caution"><para>IMPORTANT: When using Network Floating Licensing features (via the NetworkSemaphore class) with protected applications, Windows Vista/Server 2008 or later is required for both the client computers accessing the protected application, and the file server hosting the share where the semaphore files will be stored.</para></note>
    public partial class MainForm : Form
    {
        #region Private Member Variables
        private SampleLicense m_License;
        private NetworkSemaphore m_Semaphore;
        LicenseInvalidCountdownForm m_CountdownDialog;
        #endregion    

        #region Constructors
        /// <summary>Default main form constructor</summary>
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Public Methods
        /// <summary>Reloads the license file and refreshes the status on the main form.</summary>
        public void ReloadLicense()
        {
            if (!m_License.LoadFile(LicenseConfiguration.LicenseFilePath))
            {
                refreshLicenseButton.Enabled = false;
                deactivateButton.Enabled = false;

                if (m_License.LastError.ErrorNumber == LicenseError.ERROR_PLUS_EVALUATION_INVALID)
                {
                    //Invalid Protection PLUS 5 SDK evaluation envelope.
                    statusTextLabel.Text = m_License.LastError.ErrorString;
                    activateButton.Enabled = false;
                    activateManuallyButton.Enabled = false;
                }
                else
                {
                    switch (m_License.LastError.ErrorNumber)
                    {
                        case LicenseError.ERROR_COULD_NOT_LOAD_LICENSE:
                            statusTextLabel.Text = string.Format("No license found - activation is required.", m_License.LastError.ErrorNumber);
                            break;
                        case LicenseError.ERROR_LICENSE_NOT_EFFECTIVE_YET:
                            string effectiveAsOf;

                            DateTime local = m_License.EffectiveStartDate.ToLocalTime();
                            int daysUntilEffective = (int)local.Subtract(DateTime.Now.Date).TotalDays;

                            if (1 < daysUntilEffective)
                            {
                                effectiveAsOf = string.Format("{0} ({1} days).", local.ToLongDateString(), daysUntilEffective);
                            }
                            else if (1 == daysUntilEffective)
                            {
                                effectiveAsOf = "tomorrow.";
                            }
                            else
                            {
                                effectiveAsOf = string.Format("{0} today.", local.ToShortTimeString());
                            }

                            statusTextLabel.Text = string.Format("The license is not effective until {1}", m_License.LastError.ErrorNumber);
                            break;
                        default:
                            statusTextLabel.Text = "Invalid.  " + m_License.LastError.ErrorString;
                            break;
                    }
                }

                userCountLabel.Text = "N/A";

                if (m_Semaphore != null)
                {
                    m_Semaphore.Close(); // close our network session if it is open
                    m_Semaphore = null;
                }

                return;
            }

            refreshLicenseButton.Enabled = true;
            deactivateButton.Enabled = true;

            RefreshLicenseStatus();
        }

        /// <summary>Refreshes the license status on the main form</summary>
        public void RefreshLicenseStatus()
        {
            refreshLicenseButton.Enabled = (m_License.InstallationID.Length > 0);
            
            if (!m_License.Validate())
            {
                if (m_Semaphore != null)
                {
                    m_Semaphore.Close(); // close our network session if it is open
                    m_Semaphore = null;
                }

                statusTextLabel.Text = "The license is invalid or expired.";
                userCountLabel.Text = "N/A";
            }
            else
            {
                if (m_Semaphore == null)
                {
                    m_Semaphore = new NetworkSemaphore(Path.GetDirectoryName(LicenseConfiguration.LicenseFilePath), LicenseConfiguration.NetworkSemaphorePrefix, m_License.LicenseCounter, true, 15, true);
                    m_Semaphore.Invalid += new NetworkSemaphore.InvalidEventHandler(InvalidSemaphoreHandler);

                    if (!m_Semaphore.Open() && m_Semaphore.LastError.ErrorNumber == LicenseError.ERROR_NETWORK_LICENSE_FULL) // try to open a network session
                    {
                        NetworkLicenseSearchForm searchDlg = new NetworkLicenseSearchForm(m_Semaphore); // try to search for an open network seat

                        if (searchDlg.ShowDialog() != DialogResult.OK)
                        {
                            statusTextLabel.Text = "Unable to establish a network session. " + m_Semaphore.LastError;
                            userCountLabel.Text = "N/A";
                            m_Semaphore = null;
                        }
                    }
                    else if (m_Semaphore.LastError.ErrorNumber != LicenseError.ERROR_NONE)
                    {
                        statusTextLabel.Text = "Unable to establish a network session. " + m_Semaphore.LastError;
                        userCountLabel.Text = "N/A";
                        m_Semaphore = null;
                    }
                }

                if (m_Semaphore != null && m_Semaphore.IsValid)
                {
                    StringBuilder registerInfo = new StringBuilder();

                    //Check if first name is not empty and not unregistered
                    if (m_License.Customer.FirstName != "" && m_License.Customer.FirstName != "UNREGISTERED")
                    {
                        registerInfo.Append("Registered To: ");

                        //Append first name
                        registerInfo.Append(m_License.Customer.FirstName);
                    }

                    //Check if last name is not empty and not unregistered
                    if (m_License.Customer.LastName != "" && m_License.Customer.LastName != "UNREGISTERED")
                    {
                        if (registerInfo.ToString() == "")
                        {
                            registerInfo.Append("Registered To:");
                        }
                        registerInfo.Append(" ");

                        //Append last name
                        registerInfo.Append(m_License.Customer.LastName);
                    }

                    //Check if company name is not empty and not unregistered
                    if (m_License.Customer.CompanyName != "" && m_License.Customer.CompanyName != "UNREGISTERED")
                    {
                        if (registerInfo.ToString() == "")
                        {
                            registerInfo.Append("Registered To:");
                        }
                        registerInfo.Append(" ");

                        //Append company name
                        registerInfo.Append("[" + m_License.Customer.CompanyName + "]");
                    }

                    if (registerInfo.ToString() != "")
                    {
                        registerInfo.Append(Environment.NewLine);
                    }

                    //Append license ID
                    registerInfo.Append("License ID: " + m_License.LicenseID);

                    statusTextLabel.Text = "Fully Licensed." + Environment.NewLine +
                                         registerInfo.ToString();

                    userCountLabel.Text = m_Semaphore.SeatsActive.ToString() + " out of " + m_License.LicenseCounter.ToString(); // display how many network users are running the application
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>Main form Load event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            using (NetworkLicenseBrowseForm browseDlg = new NetworkLicenseBrowseForm())
            {
                if (browseDlg.ShowDialog() == DialogResult.OK)
                {
                    LicenseConfiguration.LicenseFilePath = browseDlg.SelectedPath;
                }
                else
                {
                    Application.Exit();
                }
            }

            m_License = new SampleLicense();

            if (m_License.LastError.ErrorNumber == LicenseError.ERROR_PLUS_EVALUATION_WARNING)
            {
                MessageBox.Show("Warning: (" + m_License.LastError.ErrorNumber + ") " + m_License.LastError.ErrorString, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            ReloadLicense();
        }

        private delegate void InvalidSemaphoreThreadDelegate(object sender, EventArgs e);

        /// <summary>Invalid semaphore handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void InvalidSemaphoreHandler(object sender, EventArgs e) // fires when the network session is no longer valid
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new InvalidSemaphoreThreadDelegate(InvalidSemaphoreHandler), new Object[] { sender, e }); // invoke this method using our UI thread delegate
            }
            else
            {
                if (!m_Semaphore.Open()) // see if we can re-open our network session
                {
                    while (true)
                    {
                        DialogResult mbResult = MessageBox.Show("Your network session is no longer valid. " + m_Semaphore.LastError + "\n\nPress 'Retry' to attempt to re-establish your network session. Pressing 'Cancel' will give you 60 seconds to save your work and exit the application.", "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);

                        if (mbResult == DialogResult.Cancel)
                        {
                            m_CountdownDialog = new LicenseInvalidCountdownForm(60); // exit the application in 60 seconds
                            m_CountdownDialog.Show();

                            break;
                        }

                        if (!m_Semaphore.Open()) // see if we can re-open our network session
                        {
                            using (NetworkLicenseSearchForm searchDlg = new NetworkLicenseSearchForm(m_Semaphore)) // try to search for an open network seat
                            {
                                if (searchDlg.ShowDialog() == DialogResult.OK)
                                    break;
                            }
                        }
                        else
                            break;
                    }
                }
            }
        }

        /// <summary>Exit button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>Activate Online button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void activateButton_Click(object sender, EventArgs e)
        {
            using (OnlineActivationForm activationDialog= new OnlineActivationForm(this, m_License))
            {
                activationDialog.ShowDialog();
            }
        }

        /// <summary>Activate Manually button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void activateManuallyButton_Click(object sender, EventArgs e)
        {
            using (ManualActivationForm activationDialog = new ManualActivationForm(this, m_License))
            {
                activationDialog.ShowDialog();
            }
        }

        /// <summary>Refresh License button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void refreshLicenseButton_Click(object sender, EventArgs e)
        {
            if (m_License.RefreshLicense())
            {
                MessageBox.Show("The license has been refreshed successfully.", "License Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The license was not refreshed.  Error: (" + m_License.LastError.ErrorNumber + ")" + m_License.LastError.ErrorString, "License Refresh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            ReloadLicense();
        }

        /// <summary>Deactivate License button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void deactivateButton_Click(object sender, EventArgs e)
        {
            if (m_License.DeactivateOnline())
            {
                MessageBox.Show("The license has been deactivated successfully.", "License Deactivation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The license was not deactivated.  Error: (" + m_License.LastError.ErrorNumber + ")" + m_License.LastError.ErrorString, "License Refresh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            ReloadLicense();
        }

        /// <summary>Close form button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Semaphore != null)
            {
                m_Semaphore.Close(); // close our network session if it is open
                m_Semaphore = null;
            }
        }
        #endregion
    }
}
