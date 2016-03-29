using System;
using System.Text;
using System.Windows.Forms;
using com.softwarekey.Client.Licensing;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Main form</summary>
    public partial class MainForm : Form
    {
        #region Private Member Variables
        private SampleLicense m_License;
        #endregion

        #region Constructors
        /// <summary>Default main form constructor</summary>
        public MainForm()
        {
            InitializeComponent();
            m_License = new SampleLicense();
        }
        #endregion

        #region Internal Methods
        /// <summary>Reloads the license file and refreshes the status on the main form.</summary>
        internal void ReloadLicense()
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

                return;
            }

            refreshLicenseButton.Enabled = true;
            deactivateButton.Enabled = true;

            RefreshLicenseStatus();
        }

        /// <summary>Refreshes the license status on the main form</summary>
        internal void RefreshLicenseStatus()
        {
            refreshLicenseButton.Enabled = (m_License.InstallationID.Length > 0);
            if (!m_License.Validate())
            {
                statusTextLabel.Text = "The license is invalid or expired.";
                return;
            }

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

            statusTextLabel.Text = "Fully Licensed. " + Environment.NewLine +
                                 registerInfo.ToString();
        }
        #endregion

        #region Private Event Handler Methods
        /// <summary>Activate Online button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void activateButton_Click(object sender, EventArgs e)
        {
            using (OnlineActivationForm activationDialog = new OnlineActivationForm(this, m_License))
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
                MessageBox.Show("The license was not deactivated.  Error: (" + m_License.LastError.ErrorNumber + ")" + m_License.LastError.ErrorString, "License Deactivation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            ReloadLicense();
        }

        /// <summary>Exit button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
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

        /// <summary>Main form Load event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (m_License.LastError.ErrorNumber == LicenseError.ERROR_PLUS_EVALUATION_WARNING)
            {
                MessageBox.Show("Warning: (" + m_License.LastError.ErrorNumber + ") " + m_License.LastError.ErrorString, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            ReloadLicense();
        }
        #endregion
    }
}
