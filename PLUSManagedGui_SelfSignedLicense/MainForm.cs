using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using com.softwarekey.Client.Gui;
using com.softwarekey.Client.Licensing;

namespace com.softwarekey.Client.Sample
{
    /// <summary>This sample application's main form.</summary>
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
        /// <summary>Creates a new <see cref="MainForm"/> object.</summary>
        public MainForm()
        {
            //Initialize the form's components (this is required for your form to function properly).
            InitializeComponent();

            //Initialize the License and LicenseGui objects.
            InitializeLicensingObjects();
        }
        #endregion

        #region Private Licensing Member Variables
        private bool m_LastLicenseValidationResult = false;
        private DateTime m_LastLicenseValidationTime = DateTime.UtcNow;
        private SampleLicense m_License;
        #endregion

        #region Private Licensing Methods
        /// <summary>Initializes licensing objects.</summary>
        /// <remarks>This is called by the <see cref="MainForm"/> constructor.</remarks>
        private void InitializeLicensingObjects()
        {
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
                //It looks like our system clock was back-dated while the application was running.
                m_LastLicenseValidationResult = false;
                m_License.LastError = new LicenseError(LicenseError.ERROR_SYSTEM_TIME_INVALID);
            }
            else if (m_License.LastError.ErrorNumber != LicenseError.ERROR_COULD_NOT_LOAD_LICENSE)
            {
                //If the license was loaded, validate it.
                m_LastLicenseValidationResult = m_License.Validate();
                m_LastLicenseValidationTime = DateTime.UtcNow;
            }

            //Update the license status on the form.
            UpdateLicenseStatus();

            //Show the license management dialog.
            sampleLicensingGui.ShowDialog(LicensingGuiDialog.LicenseManagement, this, FormStartPosition.CenterParent);
        }
        
        /// <summary>Updates the controls on the form based on the status of the license.</summary>
        private void UpdateLicenseStatus()
        {
            //Update the controls on this form.
            criticalFeatureButton.Enabled = m_LastLicenseValidationResult;

            //Update the status label on the form.
            statusLabel.Text = m_License.GenerateLicenseStatusString(m_LastLicenseValidationResult);

            //Add the license status entries.
            m_License.InitializeLicenseStatusEntries(m_LastLicenseValidationResult, sampleLicensingGui);
        }
        #endregion

        #region Private Licensing Event Handler Methods
        /// <summary>The method that performs the work, which is run asynchronously by the splash screen.</summary>
        /// <remarks><para>This method handles the <see cref="LicensingGui.SplashDoWork"/> event.</para></remarks>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void InitializeApplicationSettings(object sender, EventArgs e)
        {
            //Load and initialize the license file.
            m_LastLicenseValidationResult = m_License.InitializeLicense();

            //If we created a fresh evaluation, clear the prior error generated from being unable to load an existing license file.
            if (m_LastLicenseValidationResult && m_License.LastError.ErrorNumber == LicenseError.ERROR_COULD_NOT_LOAD_LICENSE)
                m_License.LastError = new LicenseError(LicenseError.ERROR_NONE);

            if (!m_LastLicenseValidationResult &&
                !(m_License.IsWritable) &&
                m_License.ProductOption.OptionType == LicenseProductOption.ProductOptionType.DownloadableLicenseWithTriggerCodeValidation)
            {
                //The license is a downloadable license file, and is not supported since the application license is not a WritableLicense...
                string licenseFilePath = (File.Exists(LicenseConfiguration.VolumeLicenseFilePath) ?
                    LicenseConfiguration.VolumeLicenseFilePath :
                    LicenseConfiguration.LicenseFilePath);
                try
                {
                    //Try to delete the downloadable file.
                    File.Delete(licenseFilePath);

                    //Re-initialize the settings and the license objects so none of the information from the downloadable license carries over.
                    m_License = SampleLicense.CreateNewLicense(sampleLicensingGui);
                    sampleLicensingGui.ApplicationLicense = m_License;

                    //Try to load and initialize the license file again.
                    m_LastLicenseValidationResult = m_License.InitializeLicense();
                }
                catch (Exception)
                {
                    //We were unable to delete the downloadable license file.
                }
            }

            m_LastLicenseValidationTime = DateTime.UtcNow;

            //TODO: If your application has any of its own initialization logic, you can add it here so it runs while the
            //      splash screen is being displayed.
        }

        /// <summary>Occurs when a manual action should be loaded and resumed.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        /// <remarks>
        /// <para>This method handles the <see cref="LicensingGui.ManualActionLoadRequested"/> event.</para>
        /// <note type="caution">
        /// <para>
        /// TODO: IMPORTANT: Either Implement your own handlers for loading and saving a manual request state/session, or update
        /// the ManualActionIV and ManualActionKey properties (in the LicenseConfiguration class) with randomized data which is
        /// unique for each of your applications. The implementations provided are simple and only designed to show you the
        /// functionality which you can achieve by saving and restoring manual action sessions.  However, it is VERY IMPORTANT
        /// that you take measures to hide and protect the saved request data/file so users may not easily restore and replay
        /// old responses (known as a replay attack).
        /// </para>
        /// </note>
        /// </remarks>
        private void ManualActionLoadRequested(object sender, ManualActionLoadRequestedEventArgs e)
        {
            e.Contents = SampleLicense.LoadManualActionSessionState();
        }

        /// <summary>Occurs when a manual's action should be saved so it may be resumed later.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        /// <remarks>
        /// <para>This method handles the <see cref="LicensingGui.ManualActionSaveRequested"/> event.</para>
        /// <note type="caution">
        /// <para>
        /// TODO: IMPORTANT: Either Implement your own handlers for loading and saving a manual request state/session, or update
        /// the ManualActionIV and ManualActionKey properties (in the LicenseConfiguration class) with randomized data which is
        /// unique for each of your applications. The implementations provided are simple and only designed to show you the
        /// functionality which you can achieve by saving and restoring manual action sessions.  However, it is VERY IMPORTANT
        /// that you take measures to hide and protect the saved request data/file so users may not easily restore and replay
        /// old responses (known as a replay attack).
        /// </para>
        /// </note>
        /// </remarks>
        private void ManualActionSaveRequested(object sender, ManualActionSaveRequestedEventArgs e)
        {
            SampleLicense.SaveManualActionSessionState(e.Contents);
        }

        /// <summary>Performs post-processing updates.</summary>
        /// <remarks><para>This method handles the <see cref="LicensingGui.LicenseManagementActionComplete"/> event.</para></remarks>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void PostProcessingUpdates(object sender, LicenseManagementActionCompleteEventArgs e)
        {
            SampleLicense.PostProcessingUpdates(ref m_License, ref sampleLicensingGui, ref e, out m_LastLicenseValidationResult);
            m_LastLicenseValidationTime = DateTime.UtcNow;

            //Update the license status entries and the controls on this form.
            UpdateLicenseStatus();
        }

        /// <summary>Occurs when the splash screen has finished running the <see cref="InitializeApplicationSettings"/> function asynchronously.</summary>
        /// <remarks><para>This method handles the <see cref="LicensingGui.SplashWorkCompleteEvent"/> event.</para></remarks>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void SplashWorkCompleted(object sender, EventArgs e)
        {
            Show();
        }
        #endregion

        #region Private Form Event Handlers
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

        /// <summary>Handles the exit button click event.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void exitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>Handles the FormClosing event.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_License.UnloadLicense();
        }

        /// <summary>Handles the form load event.</summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //Show warning message if evaluation envelope being used.
            if (m_License.LastError.ErrorNumber == LicenseError.ERROR_PLUS_EVALUATION_WARNING)
            {
                MessageBox.Show("Warning: (" + m_License.LastError.ErrorNumber + ") " + m_License.LastError.ErrorString, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //Display the splash screen.
            sampleLicensingGui.ShowDialog(LicensingGuiDialog.SplashScreen);
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
        }

        /// <summary>Handles the manage license button click event.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments</param>
        private void manageLicenseButton_Click(object sender, EventArgs e)
        {
            ManageLicense();
        }
        #endregion
    }
}
