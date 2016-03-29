using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;
using com.softwarekey.Client.Licensing;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Main form</summary>
    public partial class MainForm : Form
    {
        #region Private Member Variables
        private SampleReadOnlyLicense m_License;
        private SampleSelfSignedLicense m_EvaluationLicense;
        private SampleLicense m_CurrentLicense;
        private bool m_IsLicenseValid = false;
        private bool m_IsEvaluation = false;
        #endregion

        #region Constructors
        /// <summary>Default main form constructor</summary>
        public MainForm()
        {
            InitializeComponent();
            m_License = new SampleReadOnlyLicense();
            m_CurrentLicense = m_License;
        }
        #endregion

        #region Public Methods
        /// <summary>Reloads the license file and refreshes the status on the main form.</summary>
        public void ReloadLicense()
        {
            m_License = new SampleReadOnlyLicense();
            m_EvaluationLicense = new SampleSelfSignedLicense();
            m_CurrentLicense = m_License;

            if (!m_License.LoadFile(LicenseConfiguration.LicenseFilePath))
            {
                if (m_License.LastError.ErrorNumber == LicenseError.ERROR_PLUS_EVALUATION_INVALID)
                {
                    //Invalid Protection PLUS 5 SDK evaluation envelope.
                    statusTextLabel.Text = m_License.LastError.ErrorString;
                    activateButton.Enabled = false;
                    activateManuallyButton.Enabled = false;
                    refreshLicenseButton.Enabled = false;
                    deactivateButton.Enabled = false;
                    return;
                }

                m_CurrentLicense = m_EvaluationLicense;
                m_IsEvaluation = true;

                if (!m_EvaluationLicense.LoadFile(LicenseConfiguration.LicenseFilePath))
                {
                    if (!m_EvaluationLicense.CreateFreshEvaluation())
                    {
                        statusTextLabel.Text = "Invalid.  " + m_License.LastError.ErrorString;
                        return;
                    }
                    else
                    {
                        if (!m_EvaluationLicense.LoadFile(LicenseConfiguration.LicenseFilePath))
                        {
                            statusTextLabel.Text = "Invalid.  " + m_License.LastError.ErrorString;
                            return;
                        }
                    }
                }
            }
            else
            {
                m_CurrentLicense = m_License;
                m_IsEvaluation = false;
            }

            RefreshLicenseStatus();
        }

        /// <summary>Refreshes the license status on the main form</summary>
        public void RefreshLicenseStatus()
        {
            if (!m_CurrentLicense.IsEvaluation)
            {
                refreshLicenseButton.Enabled = true;
                deactivateButton.Enabled = true;
            }
            else
            {
                refreshLicenseButton.Enabled = false;
                deactivateButton.Enabled = false;
            }

            m_IsLicenseValid = m_CurrentLicense.Validate();
            if (!m_IsLicenseValid)
            {
                statusTextLabel.Text = "The license is invalid or expired.";
                return;
            }

            if (m_CurrentLicense.IsEvaluation)
            {
                refreshLicenseButton.Enabled = false;
                deactivateButton.Enabled = false;
                TimeSpan ts = m_CurrentLicense.EffectiveEndDate - DateTime.Now.Date;
                statusTextLabel.Text = "Trial expires in " + Math.Round(ts.TotalDays, 0).ToString() + " days.";
            }
            else
            {
                refreshLicenseButton.Enabled = true;
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
            }
        }
        #endregion

        #region Private Methods
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

        /// <summary>Close form button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_IsEvaluation && m_IsLicenseValid)
            {
                //we're shutting down, so just try to save what we can
                int ignore1, ignore2;
                m_EvaluationLicense.WriteAliases(out ignore1, out ignore2);
                m_EvaluationLicense.WriteLicenseFile(LicenseConfiguration.LicenseFilePath);
            }
        }

        /// <summary>Deactivate License button click event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void deactivateButton_Click(object sender, EventArgs e)
        {
            if (!m_IsEvaluation)
            {
                if (m_License.DeactivateOnline())
                {
                    m_EvaluationLicense = new SampleSelfSignedLicense();
                    File.Delete(LicenseConfiguration.LicenseFilePath);
                    if (m_EvaluationLicense.CreateExpiredEvaluation())
                    {
                        m_CurrentLicense = m_EvaluationLicense;
                        MessageBox.Show("The license has been deactivated successfully.", "License Deactivation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("The license was not deactivated.  Error: (" + m_License.LastError.ErrorNumber + ")" + m_License.LastError.ErrorString, "License Deactivation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("The license was not deactivated.  Error: (" + m_License.LastError.ErrorNumber + ")" + m_License.LastError.ErrorString, "License Deactivation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                ReloadLicense();
            }
        }
        #endregion
    }
}
