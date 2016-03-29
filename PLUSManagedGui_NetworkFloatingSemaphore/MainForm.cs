using System;
using System.IO;
using System.Windows.Forms;
using com.softwarekey.Client.Gui;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.Licensing.Network;

namespace com.softwarekey.Client.Sample
{
    /// <summary>The sample application's main form.</summary>
    public partial class MainForm : Form
    {
        //TODO: IMPORTANT: You will need to update several areas of this code for your application...
        //      * IMPORTANT: You need to update the static properties in the LicenseConfiguration class before copying it into your application!!!
        //      * IMPORTANT: Settings for your licensing GUI may be configured by selecting the LicensingGui component on the form in which the component
        //                   has been added.  Once selected, Visual Studio includes a "Properties" pane, which may be used to configure properties.  These
        //                   properties allow you to configure things including (but not limited to): what types of activations and license management
        //                   actions are allowed, splash screen colors and graphics, custom dialog icons, etc...
        //      * IMPORTANT: Read the TODO comments in the license implementation class to understand changes required before copying it into your application!!!
        //                   This includes ANY/ALL sample license implementation classes, such as ReadOnlyLicense and SelfSignedLicense.
        //      * IMPORTANT: Trigger Codes are a feature which allows you simplify activation for customers which need to activate from a very disconnected location
        //                   by allowing the customer to activate by phone or fax.
        //                   WARNING: TRIGGER CODES SHOULD ONLY BE USED WHEN ABSOLUTELY NECESSARY!
        //                            Like any other challenge/response scheme, trigger codes introduce the potential to be exploited by hackers to make key generators
        //                            (which are programs that issue unauthorized activations for your application) available if you allow the use of trigger codes.
        //                            If the customer is able to activate using another computer or device's Internet connection, or by email, then using the manual
        //                            request file is strongly recommended, as this is a much more robust and secure approach for activating disconnected devices or
        //                            systems.
        //      * If your application has any initialization code or logic, you may want or need to add or move it to the InitializeApplicationSettings method so it
        //        is run while the splash screen is being displayed.
        //      * The ManualActionLoadRequested and ManualActionSaveRequested methods show basic manual action session state save & restore functionality, but
        //        is disabled by default for security reasons.  View the comments around these methods for additional information.  Also, be careful when
        //        removing the event wire-up in your project using the designer, as removing the event handlers in the designer will REMOVE the implementations
        //        in this file (which simply call to the LoadManualActionSessionState and SaveManualActionSessionState methods in the LicenseConfiguration class).
        //      * The status shown in this sample's main form can be customized in the UpdateMainStatusLabel method.
        //      * If your application needs to show the status of additional items (such as license parameters, features, or modules), then you should review the
        //        comments in the UpdateLicenseStatus method.

        #region Constructors
        /// <summary>Default main form constructor</summary>
        public MainForm()
        {
            InitializeComponent();

            InitializeLicensingObjects();
        }
        #endregion

        #region Private Licensing Member Variables
        private bool m_LastLicenseValidationResult = false;
        private DateTime m_LastLicenseValidationTime = DateTime.UtcNow;
        private SampleLicense m_License;
        private NetworkSemaphore m_Semaphore;
        private LicenseInvalidCountdownForm m_CountdownDlg;
        #endregion

        #region Private Licensing Method Delegates
        /// <summary>Delegate for invoking <see cref="InvalidSemaphoreThread"/></summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private delegate void InvalidSemaphoreThreadDelegate(object sender, EventArgs e);

        /// <summary>Delegate for invoking <see cref="PostProcessingUpdates"/></summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private delegate void PostProcessingUpdatesDelegate(object sender, LicenseManagementActionCompleteEventArgs e);

        /// <summary>Delegate for invoking <see cref="InitializeApplicationSettings"/>.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private delegate void SplashWorkDelegate(object sender, EventArgs e);

        /// <summary>Delegate for invoking <see cref="SplashWorkCompleted"/>.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private delegate void SplashWorkCompletedDelegate(object sender, EventArgs e);
        #endregion

        #region Private Form Event Handlers
        /// <summary>Handles the Main Form load event</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //If evaluation envelope being used, show warning message.
            if (m_License.LastError.ErrorNumber == LicenseError.ERROR_PLUS_EVALUATION_WARNING)
            {
                MessageBox.Show("Warning: (" + m_License.LastError.ErrorNumber + ") " + m_License.LastError.ErrorString, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                m_License.LastError = new LicenseError(LicenseError.ERROR_NONE);
            }

            sampleLicensingGui.ShowDialog(LicensingGuiDialog.SplashScreen);
        }

        /// <summary>Handles the Main Form close event</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Semaphore != null)
            {
                m_Semaphore.Close(); // close our network session if it is open
                m_Semaphore = null;
            }
        }

        /// <summary>Handles the form shown event.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            //Update the license status on the form.
            UpdateLicenseStatus();

            //Check the license validation result.
            if (!m_LastLicenseValidationResult ||
                LicenseTypes.Unlicensed == m_License.TypeOfLicense ||
                m_License.IsTimeLimitedWarningRequired ||
                File.Exists(LicenseConfiguration.ManualActionSessionStateFilePath))
            {
                //If license validation failed, if this is an evaluation period,
                //or if there is a manual action request pending processing,
                //show the license management form when the application starts.
                sampleLicensingGui.ShowDialog(LicensingGuiDialog.LicenseManagement, this, FormStartPosition.CenterParent);
            }

            //Update the status
            this.criticalFeatureButton.Enabled = UpdateLicenseStatus();
        }

        /// <summary>Handles the critical feature button click event.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void criticalFeatureButton_Click(object sender, EventArgs e)
        {
            if (SampleLicense.IsClockBackdatedAtRuntime(m_LastLicenseValidationTime))
            {
                //It looks like our system clock was back-dated while the application was running.
                m_LastLicenseValidationResult = false;
                m_License.LastError = new LicenseError(LicenseError.ERROR_SYSTEM_TIME_INVALID);
            }
            else if (m_LastLicenseValidationResult && m_License.TypeOfLicense != LicenseTypes.FullNonExpiring)
            {
                //If the license is time-limited, and prior validation has passed, check to make sure the license is not expired (since time has passed since the last validation).
                LicenseEffectiveDateValidation dateValidation = new LicenseEffectiveDateValidation(m_License);
                if (!dateValidation.Validate())
                {
                    m_LastLicenseValidationResult = false;
                    m_LastLicenseValidationTime = DateTime.UtcNow;
                    m_License.LastError = dateValidation.LastError;
                }
            }

            if (!m_LastLicenseValidationResult)
            {
                //Double check the license validation result.  This prevents a hacker from sending a window message
                //to enable the button regardless of the validation result.
                UpdateLicenseStatus();
                sampleLicensingGui.ShowDialog(LicensingGuiDialog.LicenseManagement, this, FormStartPosition.CenterParent);
                return;
            }

            MessageBox.Show("This is a critical feature!");
        }

        /// <summary>Handles the Manage License button click event.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void manageLicenseButton_Click(object sender, EventArgs e)
        {
            ManageLicense();
        }

        /// <summary>Handles the Exit button click event.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion

        #region Private Licensing Event Handler Methods
        /// <summary>The method that performs the work, which is run asynchronously by the splash screen.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        /// <remarks><para>This method handles the <see cref="LicensingGui.SplashDoWork"/> event.</para></remarks>
        private void InitializeApplicationSettings(object sender, EventArgs e)
        {
            //Load and initialize the license file.
            m_LastLicenseValidationResult = m_License.InitializeLicense();

            //If License is initialized properly, display status. If we created a fresh evaluation, clear the prior error generated from being unable to load an existing license file.
            if (m_LastLicenseValidationResult && m_License.LastError.ErrorNumber == LicenseError.ERROR_COULD_NOT_LOAD_LICENSE)
                m_License.LastError = new LicenseError(LicenseError.ERROR_NONE);

            //this.criticalFeatureButton.Enabled = UpdateLicenseStatus();

            if (m_Semaphore != null)
            {
                m_Semaphore.Close();
                m_Semaphore = null;
            }

            if (!m_LastLicenseValidationResult && !(m_License.IsWritable) &&
                m_License.ProductOption.OptionType == LicenseProductOption.ProductOptionType.DownloadableLicenseWithTriggerCodeValidation)
            {
                //The license is a downloadable license file, and is not supported since the application license is not a WritableLicense...
                try
                {
                    //Try to delete the downloadable file.
                    File.Delete(LicenseConfiguration.LicenseFilePath);

                    //Re-initialize the settings and the license objects so none of the information from the downloadable license carries over.
                    m_License = SampleLicense.CreateNewLicense(sampleLicensingGui);
                    sampleLicensingGui.ApplicationLicense = m_License;

                    //Try to load and initialize the license file again.
                    m_LastLicenseValidationResult = m_License.InitializeLicense();
                }
                catch (Exception ex)
                {
                    statusLabel.Text = "Error: " + ex.Message;
                }
            }

            m_LastLicenseValidationTime = DateTime.UtcNow;
        }

        /// <summary>Performs post-processing updates.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        /// <remarks><para>This method handles the <see cref="LicensingGui.LicenseManagementActionComplete"/> event.</para></remarks>
        private void PostProcessingUpdates(object sender, LicenseManagementActionCompleteEventArgs e)
        {
            SampleLicense.PostProcessingUpdates(ref m_License, ref sampleLicensingGui, ref e, out m_LastLicenseValidationResult);
            m_LastLicenseValidationTime = DateTime.UtcNow;

            //Update SeatsTotal when Semaphore is not null
            if (m_Semaphore != null)
            {
                m_Semaphore.SeatsTotal = m_License.LicenseCounter;
                if (m_Semaphore.SeatsAvailable < 0)
                {
                    m_Semaphore.Close();
                    m_Semaphore = null;
                }
            }

            //Update license LastError if license activation has been completed successfully in post-Processing updates
            if (m_LastLicenseValidationResult && e.LastError.ErrorNumber == LicenseError.ERROR_NONE)
                m_License.LastError = new LicenseError(LicenseError.ERROR_NONE);
            UpdateLicenseStatus();
        }

        /// <summary>Occurs when the splash screen has finished running the <see cref="InitializeApplicationSettings"/> function asynchronously.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        /// <remarks><para>This method handles the <see cref="LicensingGui.SplashWorkCompleteEvent"/> event.</para></remarks>
        private void SplashWorkCompleted(object sender, EventArgs e)
        {
            Show();
        }

        /// <summary>Occurs when a manual action should be loaded and resumed.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        /// <remarks><para>This method handles the <see cref="LicensingGui.ManualActionLoadRequested"/> event.</para></remarks>
        private void ManualActionLoadRequested(object sender, ManualActionLoadRequestedEventArgs e)
        {
            e.Contents = SampleLicense.LoadManualActionSessionState();
        }

        /// <summary>Occurs when a manual's action should be saved so it may be resumed later.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        /// <remarks><para>This method handles the <see cref="LicensingGui.ManualActionSaveRequested"/> event.</para></remarks>
        private void ManualActionSaveRequested(object sender, ManualActionSaveRequestedEventArgs e)
        {
            SampleLicense.SaveManualActionSessionState(e.Contents);
        }

        /// <summary>Fires when the network session is no longer valid</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void InvalidSemaphoreHandler(object sender, EventArgs e)
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
                            m_CountdownDlg = new LicenseInvalidCountdownForm(60); // exit the application in 60 seconds
                            m_CountdownDlg.Show();

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
        #endregion

        #region Private Licensing Methods
        /// <summary>Displays <see cref="NetworkLicenseBrowseForm"/> Initializes licensing objects.</summary>
        private void InitializeLicensingObjects()
        {
            using (NetworkLicenseBrowseForm dialog = new NetworkLicenseBrowseForm())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    LicenseConfiguration.LicenseFilePath = dialog.SelectedPath;
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            //Initialize the license object.
            m_License = SampleLicense.CreateNewLicense(sampleLicensingGui);

            //Now pass the initialized license object to the LicenseingGui component.
            sampleLicensingGui.ApplicationLicense = m_License;

            //Configure the LicensingGui component with the trigger code seed data (only applicable with writable/self-signed licenses).
            sampleLicensingGui.TriggerCodeSeed = LicenseConfiguration.TriggerCodeSeed;
            sampleLicensingGui.RegKey2Seed = LicenseConfiguration.RegKey2Seed;

            //Wire-up event handlers for the LicensingGui component.  Although this is typically done in the designer, this sample code intentionally
            //initializes to a) make it easier for you to copy-and-paste; and b) prevent the sample functions referenced here from being deleted when
            //the event handlers are changed in the designer.
            sampleLicensingGui.LicenseManagementActionComplete += new EventHandler<LicenseManagementActionCompleteEventArgs>(PostProcessingUpdates);
            sampleLicensingGui.ManualActionLoadRequested += new EventHandler<ManualActionLoadRequestedEventArgs>(ManualActionLoadRequested);
            sampleLicensingGui.ManualActionSaveRequested += new EventHandler<ManualActionSaveRequestedEventArgs>(ManualActionSaveRequested);
            sampleLicensingGui.SplashDoWork += new EventHandler<EventArgs>(InitializeApplicationSettings);
            sampleLicensingGui.SplashWorkCompleteEvent += new EventHandler<EventArgs>(SplashWorkCompleted);
        }

        /// <summary>Shows the <see cref="LicensingGui"/> object's license management form.</summary>
        private void ManageLicense()
        {
            if (LicenseConfiguration.EncryptionKey.LastError.ErrorNumber != LicenseError.ERROR_NONE &&
               LicenseConfiguration.EncryptionKey.LastError.ErrorNumber != LicenseError.ERROR_PLUS_EVALUATION_WARNING)
            {
                m_License.LastError = LicenseConfiguration.EncryptionKey.LastError;
            }
            else if (SampleLicense.IsClockBackdatedAtRuntime(m_LastLicenseValidationTime))
            {
                m_LastLicenseValidationResult = false;
                m_License.LastError = new LicenseError(LicenseError.ERROR_SYSTEM_TIME_INVALID);
            }
            else if (m_License.LastError.ErrorNumber != LicenseError.ERROR_COULD_NOT_LOAD_LICENSE)
            {
                m_LastLicenseValidationResult = m_License.Validate();
                m_LastLicenseValidationTime = DateTime.UtcNow;
                statusLabel.Text = "Status: " + m_License.LastError.ErrorString;
            }

            this.criticalFeatureButton.Enabled = UpdateLicenseStatus();

            sampleLicensingGui.ShowDialog(LicensingGuiDialog.LicenseManagement, this, FormStartPosition.CenterParent);

            //Update the status
            this.criticalFeatureButton.Enabled = UpdateLicenseStatus();
        }

        /// <summary>Updates the License's status in the <see cref="LicensingGui"/> and checks if network seats are available.</summary>
        /// <returns>Returns true if the network seats are not depleted, false otherwise</returns>
        private bool UpdateLicenseStatus()
        {
            SampleLicense license = SampleLicense.CreateNewLicense(sampleLicensingGui);
            license.LoadFile(LicenseConfiguration.LicenseFilePath);
            if (license.LicenseID != m_License.LicenseID || license.InstallationID != m_License.InstallationID)
            {
                sampleLicensingGui.ApplicationLicense = license;
                m_License = license;
                if (m_Semaphore != null)
                {
                    m_Semaphore.Close();
                    m_Semaphore = null;
                }
            }

            if (!m_LastLicenseValidationResult || m_License.LastError.ErrorNumber != LicenseError.ERROR_NONE)
            {
                statusLabel.Text = m_License.GenerateNetworkLicenseStatusString(m_LastLicenseValidationResult, m_Semaphore);
                m_License.InitializeNetworkLicenseStatusEntries(m_LastLicenseValidationResult, sampleLicensingGui, m_Semaphore);

                //Check seats count and set the status, when No seats available and license re-activation completed with errors(No activation left or Invalid License ID and/or Password).
                if (m_License.LastError.ErrorNumber == LicenseError.ERROR_WEBSERVICE_RETURNED_FAILURE && m_Semaphore == null)
                {
                    m_Semaphore = new NetworkSemaphore(Path.GetDirectoryName(LicenseConfiguration.LicenseFilePath), LicenseConfiguration.NetworkSemaphorePrefix, m_License.LicenseCounter, true, 15, true);
                    if (!m_Semaphore.Open() && m_Semaphore.LastError.ErrorNumber == LicenseError.ERROR_NETWORK_LICENSE_FULL) // try to open a network session
                    {
                        statusLabel.Text = m_License.GenerateLicenseStatusString(m_LastLicenseValidationResult) + "No Seats Available.";
                        m_License.InitializeLicenseStatusEntries(m_LastLicenseValidationResult, sampleLicensingGui);
                        sampleLicensingGui.LicenseStatusEntries.Add(new LicenseStatusEntry(LicenseStatusIcon.Information, "Network Seats", "No Seats Available"));
                    }
                    m_Semaphore = null;
                }

                return false;
            }

            if (!m_License.Validate())
            {
                if (m_Semaphore != null)
                {
                    m_Semaphore.Close(); // close our network session if it is open
                    m_Semaphore = null;
                }
                statusLabel.Text = "The license is invalid or expired";
                m_License.InitializeNetworkLicenseStatusEntries(m_LastLicenseValidationResult, sampleLicensingGui, m_Semaphore);
                return false;
            }
            else
            {
                if (m_Semaphore == null)
                {
                    m_Semaphore = new NetworkSemaphore(Path.GetDirectoryName(LicenseConfiguration.LicenseFilePath), LicenseConfiguration.NetworkSemaphorePrefix, m_License.LicenseCounter, true, 15, true);
                    m_Semaphore.Invalid += new NetworkSemaphore.InvalidEventHandler(InvalidSemaphoreHandler);

                    if (!m_Semaphore.Open() && m_Semaphore.LastError.ErrorNumber == LicenseError.ERROR_NETWORK_LICENSE_FULL) // try to open a network session
                    {
                        using (NetworkLicenseSearchForm searchDialog = new NetworkLicenseSearchForm(m_Semaphore))
                        {
                            if (searchDialog.ShowDialog() != DialogResult.OK)
                            {
                                statusLabel.Text = m_License.GenerateLicenseStatusString(m_LastLicenseValidationResult) + "No Seats Available.";
                                m_License.InitializeLicenseStatusEntries(m_LastLicenseValidationResult, sampleLicensingGui);
                                sampleLicensingGui.LicenseStatusEntries.Add(new LicenseStatusEntry(LicenseStatusIcon.Information, "Network Seats", "No Seats Available"));
                                m_Semaphore = null;
                            }
                        }
                    }
                    else if (m_Semaphore.LastError.ErrorNumber != LicenseError.ERROR_NONE)
                    {
                        statusLabel.Text = "Status: Unable to establish a network session. " + m_Semaphore.LastError;
                        statusLabel.Text += "\nUsers: N/A";
                        m_Semaphore = null;
                    }
                    else
                    {
                        statusLabel.Text = m_License.GenerateNetworkLicenseStatusString(m_LastLicenseValidationResult, m_Semaphore);
                        m_License.InitializeNetworkLicenseStatusEntries(m_LastLicenseValidationResult, sampleLicensingGui, m_Semaphore);
                    }
                    return false;
                }
                if (m_Semaphore != null && m_Semaphore.IsValid)
                {
                    statusLabel.Text = m_License.GenerateNetworkLicenseStatusString(m_LastLicenseValidationResult, m_Semaphore);
                    m_License.InitializeNetworkLicenseStatusEntries(m_LastLicenseValidationResult, sampleLicensingGui, m_Semaphore);
                }
                return true;
            }
        }
        #endregion
    }
}
