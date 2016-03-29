using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.Licensing.Network;
using com.softwarekey.Client.Utils;
using com.softwarekey.Client.WebService.XmlNetworkFloatingService;

namespace CloudControlledNetworkLicensing
{
    public partial class MainForm : Form
    {
        #region Private Member Variables
        private static readonly List<SystemIdentifierAlgorithm> m_IdentifierAlgorithms = new List<SystemIdentifierAlgorithm>(
            Environment.OSVersion.Platform == PlatformID.Win32NT ?
                /*Algorithms to use on Windows:*/
                new SystemIdentifierAlgorithm[] { new NicIdentifierAlgorithm(), new ComputerNameIdentifierAlgorithm(), new HardDiskVolumeSerialIdentifierAlgorithm(HardDiskVolumeSerialFilterType.OperatingSystemRootVolume), new BiosUuidIdentifierAlgorithm(), new ProcessorIdentifierAlgorithm(new ProcessorIdentifierAlgorithmTypes[] { ProcessorIdentifierAlgorithmTypes.ProcessorName, ProcessorIdentifierAlgorithmTypes.ProcessorVendor, ProcessorIdentifierAlgorithmTypes.ProcessorVersion }) } :
                /*Algorithms to use on all other platforms (Note that you can add HardDiskVolumeSerialIdentifierAlgorithm for Linux, but it is not presently supported in OS X):*/
                new SystemIdentifierAlgorithm[] { new NicIdentifierAlgorithm(), new ComputerNameIdentifierAlgorithm() } );
        //TODO: If you need to disable encryption and/or digital signatures, modify the values passed in for the last two arguments below.
        private NetworkSession m_NetworkSession = null;
        private bool m_LastPollSuccessful = true;
        private int m_RetryCount = 0;
        private string m_CertificatePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Session.xml");
        private static readonly AuthorEncryptionKey m_EncryptionKey = new AuthorEncryptionKey("fU0A1/WpQ6w+5IA86V3b8KbrCiUmuc4La/XWHImOJO1VGQDXNehvITgHpRPlA+fk",
            "AkEo4gAzZGURTHc9XTFGvTsyP0C4A/GI8rhu9/oKXxFOvBJUQiVZKGTo78l3BOCpBJjTOp/2MWXBsySwzBd+61f/OljWYMVhtt7SPSrE1nUh6Rwo3d7HYV/FG3HB2nWf8KgitmEEnQFq1JVULp" +
            "/+3QZW/KYHGnA21hATKcqjxzX+HKNo1bBurARYU5Axnn1dbPyZUdYKEjmZNPtheyXh8sVi/tO9atnYv7jtbTtbraYfs/uc2tJIpIkvXI1kzQNoAoPaVqlCvKQS9Qq189zjqoncQPomLd6WZj+e" +
            "1SUjHe3LV6T7g9tW9KSPJFaMoqMRswOtRmQTXYZcCJbrAJx5VvPSe5klVKnQRMwMcl9AxLyH4Vhe95cO1pXeQIFPoyL1SEYiR7672GID5LrMai/64tN/ApyaKSbB2bz1g6VzchU5L40a3aV9Ef" +
            "tt90Y2zcqGpdsmSfj5jiSpLoLaXeVqQWDGAC2GGIrlfY4ewKoXXg1WNYFwKizhUd2Zt+FvygFTx/Xif3HGhOteH603bALNLcqrRtwrH6FWYqL9Lhyst0izAnxRTqSLGL4gfwtt3J0HybU5mxa1" +
            "AuNkFUNs+Sok/VJ+q2sq4UdYyT2KpcOQ0DmdjcvkNBYztCn+VHXtInz7Cp9cZwCvndYyr3+1Xn/2AssFST1KIyMBJIleUUY+A5GiRLrkyMOrhO4WS3AlUKdfmIwu/U/BX6ZvMaiyXDfLFgREcm" +
            "E4RIaszUWfCJ6zVogG2/k5UCh17DxKXrgNxQgj0PPMhFr4JKpl7FkiIJQAVtr3V7tMpdUBe+T8tgljXGpKBcaGkP+gvhpgJLmPhoCssBI2rlkiIK5dSDYR4zhP6EPIhtIURt5ZraHRwPCdeZIL" +
            "f3LKaidYT7SbfQtIAyUkspjdcdSGUBG5/hMw6L50U3nEs62Pl7N9Hl479vN/LSyCp4g8Wzt8J8HgqXe+lKFeth3HmIH0SedOGW789fqvQP8/Bd5/Em+O9t3jvoYvnzauQqo6ks/tdDWlPNqx8W" +
            "DfmaAjifPjuvnl2t9hSgOvqpJ0oq2RwfY2ydsw8Xm0mfq7oSaudraKdUHaFbQ35wLfxOWgfEsxw2Plk4Im6J5PVPEqGEbBcvUBJO1eVCNRDeYcJ6aySjlS+ldc8d3ADRzKFzvDi5w+6zPmu242" +
            "QbVuktyefY9s6ODlmKVxpONoG1w/bE64LIX0Gbt0fGc4tjZq9IHC5PLkR6XAYKd+57F5iFp9jzz86eKzKSoZK7M+3oJZ9egTr7esN5IRAjlMQoUw6DRG8vvrWEUVf7cmJCtsNRY1xgMYlmppZ3" +
            "+tzapKJbfqluPzfiCQSCdfIXbxv9L+LXKmHjgi9RQC9Ln2ajEEgVNKcfqW8wF0FbOCzismQoqe0qyPMFtkxOtdeHi5kTaTDA34ILukyP/2gpy6Wsag9hkxcojjsCiJNJ/iCjki0x00CQZhvCxj" +
            "HpvTwSv7HtEZXrRD7d2qrjz2XmC0/UBGaRTy2dPISQj9ZSDkeAmTPcmNPwV6DhuFyCzuT3xz+NBIa5o0PIC9/lMJW0I2hnMpglQK29B1SzXL2KYe6wFvQvywP0K7GQunwT1STKA/0ebcy+G9YG" +
            "jKDuLBVBzgfa+ULSp5Y26g96G/f8fSwK0OIfqwkH/51wS2N+lPqrCVtM0EiPIdUZDHUeEysVX0lc0rR7FGG4JR/molctuJhKaPoEFbavf3K8wP8ENLjzy3AiWWbH5fn84DKob7KflLntVgtxs2" +
            "rHLExkR7JJtpnxw0jAAg6tFPCVQ2uVv22qJth7t9xCmP8PvQCv3opSjzcvivX2KVQ1SQVqGJsG0Qjcqh7LTtYq0uXVOoY9WMo2l84EJd4OKL74kqZQiG/xA88Eg7Cx+34UlE7wG/+D3v5d6qNF" +
            "goI8C6gKwxtGbC1tNShzMf1Z6feS97R5U1iXc+0LplEi5DPvmsYGui/tbsxUSqh9qQ3QmmcovyZH4CSIf6p/moR+hcBZjPxg2nHen/y+/afjPTLCQKqiyW926FnArVMELEIKjiu78Pwhjjorgh" +
            "cU+fj7Lr9NPZbkr0npulf65o/1RFfGpozzweUn5si4j3bT+UoiUbYfKb6k29gVfSWFyPpK9cmxYfEDWb5/S09P/oYg==", false);
        #endregion

        #region Constructors
        /// <summary>Creates a new <see cref="MainForm"/></summary>
        public MainForm()
        {
            m_NetworkSession = new NetworkSession(m_EncryptionKey, true, true, m_IdentifierAlgorithms, m_CertificatePath, false); ;
            InitializeComponent();
        }
        #endregion

        #region Private Methods
        /// <summary>Determines whether or not the last web service response received warrants closing the current session.</summary>
        /// <returns>Returns false if the session should be allowed to remain open, or returns true if the session should be closed.</returns>
        private bool ShouldLastResponseRevokeSession()
        {
            bool shouldRevoke = false;

            if (m_NetworkSession.LastError.ErrorNumber == LicenseError.ERROR_WEBSERVICE_RETURNED_FAILURE)
            {
                switch (m_NetworkSession.LastError.ExtendedErrorNumber)
                {
                    case 5017: //The License ID's status is not OK.
                    case 5022: //SOLO Server determined that the system time is not valid.
                    case 5027: //The Network Session has been closed or has expired.
                        shouldRevoke = true;
                        break;
                }
            }

            return shouldRevoke;
        }

        /// <summary>Method for refreshing the Network Session status and all of the form elements for every/any action performed</summary>
        private void RefreshSessionInformation()
        {
            NetworkSessionCertificate certificate = m_NetworkSession.Certificate;

            if (certificate == null || string.IsNullOrEmpty(certificate.SessionID))
            {
                openSessionButton.Enabled = true;
                usePollTimerCheckBox.Enabled = true;
                licenseIDTextBox.Enabled = true;
                passwordTextBox.Enabled = true;
                pollSessionButton.Enabled = false;
                closeSessionButton.Enabled = false;
                checkoutButton.Enabled = false;
                checkInButton.Enabled = false;

                sessionIDTextBox.Text = "";
                allocatedUntilDateTextBox.Text = "";
                seatsAvailableTextBox.Text = "";
                totalSeatsTextBox.Text = "";

                checkoutMinMaxLabel.Visible = false;
                checkoutStatusLabel.Text = "Not applicable";
                checkoutDurationTextBox.Enabled = false;

                if (pollTimer.Enabled)
                    pollTimer.Stop();

                return;
            }

            openSessionButton.Enabled = false;
            usePollTimerCheckBox.Enabled = false;
            licenseIDTextBox.Enabled = false;
            passwordTextBox.Enabled = false;
            pollSessionButton.Enabled = true;
            closeSessionButton.Enabled = true;

            sessionIDTextBox.Text = certificate.SessionID;
            allocatedUntilDateTextBox.Text = certificate.AllocatedUntilDate.ToLocalTime().ToString();
            seatsAvailableTextBox.Text = certificate.SeatsAvailable.ToString();
            totalSeatsTextBox.Text = certificate.TotalSeats.ToString();

            //make sure we pass basic validation
            NetworkSessionValidation validation = new NetworkSessionValidation(m_NetworkSession);
            if (!validation.Validate())
            {
                MessageBox.Show(this, "Your session is no longer valid!  Reason: " + validation.LastError.ErrorString);
                //close the session
                this.closeSessionButton_Click(this, new EventArgs());
                return;
            }

            //setup check-out/check-in form elements
            if (certificate.CheckoutDurationMinimum > 0 && certificate.CheckoutDurationMaximum > 0)
            {
                checkoutDurationTextBox.Enabled = true;
                checkoutButton.Enabled = !certificate.CheckedOut;
                checkInButton.Enabled = certificate.CheckedOut;

                if (certificate.CheckedOut && certificate.AllocatedUntilDate.Subtract(DateTime.UtcNow).TotalMilliseconds > 0)
                {
                    checkoutExpiredTimer.Interval = (int)certificate.AllocatedUntilDate.Subtract(DateTime.UtcNow).TotalMilliseconds;
                    checkoutExpiredTimer.Start();
                    m_NetworkSession.LockCertificate();
                    checkoutStatusLabel.Text = "Checked out";
                }
                else
                {
                    m_NetworkSession.UnlockCertificate();
                    if (certificate.AllocatedUntilDate.Subtract(DateTime.UtcNow).TotalMilliseconds < 0)
                    {
                        //the checked out session has expired -- notify the user and start with a fresh, unopened session
                        MessageBox.Show(this, "Your session has expired!", "Expired", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        m_NetworkSession.ClearSession();
                        RefreshSessionInformation();
                    }
                    checkoutStatusLabel.Text = "Not checked out";
                }

                if (certificate.CheckoutDurationMinimum == certificate.CheckoutDurationMaximum)
                {
                    checkoutMinMaxLabel.Visible = false;
                    checkoutDurationTextBox.ReadOnly = true;
                    checkoutDurationTextBox.Text = certificate.CheckoutDurationMaximum.ToString();
                }
                else
                {
                    checkoutDurationTextBox.ReadOnly = false;
                    checkoutMinMaxLabel.Visible = true;
                    checkoutMinMaxLabel.Text = "Must be between " + certificate.CheckoutDurationMinimum.ToString() + " and " + certificate.CheckoutDurationMaximum.ToString() + " hours.";
                }
            }
            else
            {
                checkoutStatusLabel.Text = "Not applicable";
                checkoutDurationTextBox.Enabled = false;
                checkoutButton.Enabled = false;
                checkInButton.Enabled = false;
            }

            //setup the poll timer
            if (usePollTimerCheckBox.Checked && !certificate.CheckedOut)
            {
                if (m_LastPollSuccessful)
                {
                    pollTimer.Stop();
                    pollTimer.Interval = certificate.PollFrequency * 1000;
                    pollTimer.Start();
                }
                else
                {
                    if (m_RetryCount >= certificate.PollRetryCount)
                    {
                        pollTimer.Stop();
                        return;
                    }
                    else
                    {
                        pollTimer.Stop();
                        pollTimer.Interval = certificate.PollRetryFrequency * 1000;
                        pollTimer.Start();
                    }
                }
            }
            else
            {
                if (pollTimer.Enabled)
                    pollTimer.Stop();
            }
        }
        #endregion

        #region Private Event Handler Methods
        /// <summary>The Open Session button click event handler</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void openSessionButton_Click(object sender, EventArgs e)
        {
            int licenseId = 0;

            //validate the License ID
            if (string.IsNullOrEmpty(licenseIDTextBox.Text) || !int.TryParse(licenseIDTextBox.Text, out licenseId))
            {
                MessageBox.Show(this, "Please enter a properly formatted License ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                licenseIDTextBox.Focus();
                return;
            }

            //validate the password
            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                MessageBox.Show(this, "Please enter a password.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                passwordTextBox.Focus();
                return;
            }

            using (XmlNetworkFloatingService client = new XmlNetworkFloatingService())
            {
                client.Url = ConfigurationManager.AppSettings["NetworkFloatingServiceEndpointUrl"];
                if (!this.m_NetworkSession.OpenSession(licenseId, passwordTextBox.Text, client))
                {
                    MessageBox.Show(this, "An error occurred while trying to open the session. " + m_NetworkSession.LastError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.m_NetworkSession.ClearSession();
                }
            }

            this.RefreshSessionInformation();
        }

        /// <summary>The Poll Session button click event handler</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void pollSessionButton_Click(object sender, EventArgs e)
        {
            //call the web service
            using (XmlNetworkFloatingService webservice = new XmlNetworkFloatingService())
            {
                webservice.Url = ConfigurationManager.AppSettings["NetworkFloatingServiceEndpointUrl"];

                if (!m_NetworkSession.PollSession(webservice))
                {
                    MessageBox.Show(this, "The session poll failed. " + m_NetworkSession.LastError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    closeSessionButton_Click(sender, e);
                }
            }
            this.RefreshSessionInformation();
        }

        /// <summary>The Close Session button click event handler</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void closeSessionButton_Click(object sender, EventArgs e)
        {
            //call the web service
            using (XmlNetworkFloatingService webservice = new XmlNetworkFloatingService())
            {
                webservice.Url = ConfigurationManager.AppSettings["NetworkFloatingServiceEndpointUrl"];
                if (!m_NetworkSession.CloseSession(webservice))
                {
                    MessageBox.Show(this, "The session could not be closed. " + m_NetworkSession.LastError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            this.m_NetworkSession.ClearSession();
            this.RefreshSessionInformation();
        }

        /// <summary>The Check-Out button click event handler</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void checkoutButton_Click(object sender, EventArgs e)
        {
            if (m_NetworkSession.Certificate.CheckedOut)
            {
                MessageBox.Show(this, "Your network session has already been checked out.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (File.Exists(m_CertificatePath))
            {
                MessageBox.Show(this, "Another process has checked out a network session.  You may only check out from one instance of this application at a time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            decimal requestedCheckoutDuration = 0;

            if (string.IsNullOrEmpty(checkoutDurationTextBox.Text) || !decimal.TryParse(checkoutDurationTextBox.Text, out requestedCheckoutDuration))
            {
                MessageBox.Show(this, "Please enter a properly-formatted number of hours for which you would like to have this session checked out.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                checkoutDurationTextBox.Focus();
                return;
            }

            if (requestedCheckoutDuration < m_NetworkSession.Certificate.CheckoutDurationMinimum || requestedCheckoutDuration > m_NetworkSession.Certificate.CheckoutDurationMaximum)
            {
                if (m_NetworkSession.Certificate.CheckoutDurationMinimum == m_NetworkSession.Certificate.CheckoutDurationMaximum)
                {
                    MessageBox.Show(this, "You may only request to check out the session for " + m_NetworkSession.Certificate.CheckoutDurationMinimum.ToString() + " hours.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    MessageBox.Show(this, "You may only request to check out the session for between " + m_NetworkSession.Certificate.CheckoutDurationMinimum.ToString() + " and " +
                        m_NetworkSession.Certificate.CheckoutDurationMaximum.ToString() + " hours.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                checkoutDurationTextBox.Focus();
                return;
            }

            //call the web service
            using (XmlNetworkFloatingService webservice = new XmlNetworkFloatingService())
            {
                webservice.Url = ConfigurationManager.AppSettings["NetworkFloatingServiceEndpointUrl"];
                if (!m_NetworkSession.CheckoutSession(requestedCheckoutDuration, webservice))
                {
                    if (ShouldLastResponseRevokeSession())
                    {
                        MessageBox.Show(this, "The session will be closed.  Reason: " + LicenseError.GetWebServiceErrorMessage(m_NetworkSession.LastError.ExtendedErrorNumber), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        closeSessionButton_Click(sender, e);
                        return;
                    }

                    MessageBox.Show(this, "The session could not be checked out. " + m_NetworkSession.LastError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            this.RefreshSessionInformation();
        }

        /// <summary>The Check-In button click event handler</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void checkInButton_Click(object sender, EventArgs e)
        {
            if (!m_NetworkSession.Certificate.CheckedOut)
            {
                MessageBox.Show(this, "Your network session must be checked out before you can check it back in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            //call the web service
            using (XmlNetworkFloatingService webservice = new XmlNetworkFloatingService())
            {
                webservice.Url = ConfigurationManager.AppSettings["NetworkFloatingServiceEndpointUrl"];
                if (!m_NetworkSession.CheckinSession(webservice))
                {
                    if (ShouldLastResponseRevokeSession())
                    {
                        MessageBox.Show(this, "The session will be closed.  Reason: " + LicenseError.GetWebServiceErrorMessage(m_NetworkSession.LastError.ExtendedErrorNumber), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        closeSessionButton_Click(sender, e);
                        return;
                    }

                    MessageBox.Show(this, "The session could not be checked in. " + m_NetworkSession.LastError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            this.RefreshSessionInformation();
        }

        /// <summary>Poll timer tick handler</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void pollTimer_Tick(object sender, EventArgs e)
        {
            pollSessionButton_Click(sender, e);
        }

        /// <summary>Checkout timer tick handler</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void checkoutExpiredTimer_Tick(object sender, EventArgs e)
        {
            checkoutExpiredTimer.Stop();
            RefreshSessionInformation();
        }

        /// <summary>Form Load event handler</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["NetworkFloatingServiceEndpointUrl"]))
            {
                MessageBox.Show(this, "The application configuration appears to be missing or is missing information required for this program to run.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                this.Close();
                return;
            }

            if (m_EncryptionKey.LastError.ErrorNumber == LicenseError.ERROR_PLUS_EVALUATION_WARNING)
            {
                MessageBox.Show("Warning: (" + m_EncryptionKey.LastError.ErrorNumber + ") " + m_EncryptionKey.LastError.ErrorString, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (File.Exists(m_CertificatePath))
            {
                try
                {
                    m_NetworkSession = new NetworkSession(m_EncryptionKey, true, true, m_IdentifierAlgorithms, m_CertificatePath, true);
                    m_NetworkSession.LockCertificate();
                    this.RefreshSessionInformation();
                    /* Validate the checked-out session against the server during application startup.
                     * 
                     * This will help prevent a user from checking-out a session, backing-up the session
                     * XML certificate file, checking the session in, closing the application,
                     * restoring the session XML certificate file, and then running the application
                     * again to continue to use the checked-out session, even though it was already
                     * checked back in. */
                    if (this.m_NetworkSession.Certificate != null)
                    {
                        //call the web service to try and poll the session
                        using (XmlNetworkFloatingService webservice = new XmlNetworkFloatingService())
                        {
                            webservice.Url = ConfigurationManager.AppSettings["NetworkFloatingServiceEndpointUrl"];

                            if (!m_NetworkSession.PollSession(webservice) && ShouldLastResponseRevokeSession())
                            {
                                MessageBox.Show(this, "The session poll failed. " + m_NetworkSession.LastError.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                closeSessionButton_Click(sender, e);
                            }
                        }
                        this.RefreshSessionInformation();
                    }
                    return;
                }
                catch (IOException)
                {
                    this.m_NetworkSession.ClearSession();
                    MessageBox.Show(this, "A session has been checked out, but appears to be in use by another process.  The application will now terminate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
            }
        }

        /// <summary>FormClosing event handler</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.m_NetworkSession.Certificate != null && !this.m_NetworkSession.Certificate.CheckedOut)
                this.closeSessionButton_Click(this, new EventArgs());
        }
        #endregion
    }
}
