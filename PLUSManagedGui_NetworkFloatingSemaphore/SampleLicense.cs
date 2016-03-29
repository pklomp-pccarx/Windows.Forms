using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using com.softwarekey.Client.Gui;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.WebService.XmlLicenseFileService;

namespace com.softwarekey.Client.Sample
{
    #region Public Enumerations
    /// <summary>Types of licenses supported by this example implementation.</summary>
    public enum LicenseTypes
    {
        /// <summary>No valid license present.</summary>
        Unlicensed = 0,
        /// <summary>An activated, full, non-expiring license.</summary>
        FullNonExpiring = 1,
        /// <summary>An activated, time-limited license.</summary>
        TimeLimited = 10
    }
    #endregion

    internal partial class SampleLicense
    {
        #region Private Member Variables
        private LicensingGui m_Settings;
        #endregion

        #region Internal Properties
        /// <summary>Gets the type of license issued.</summary>
        internal LicenseTypes TypeOfLicense
        {
            get { return DetermineLicenseType(this); }
        }

        /// <summary>Gets whether the OnlineRefresh License Attempt is due or not</summary>
        /// <remarks>
        /// <para>This method is called by license refreshing and status checking logic in the license implementation class
        /// (or the class or classes that implement <see cref="License"/> or <see cref="WritableLicense"/>).</para>
        /// </remarks>
        internal bool IsRefreshLicenseAttemptDue
        {
            get
            {
                //Calculate the date difference between signature date and the current date
                TimeSpan dateDiff = DateTime.UtcNow.Subtract(SignatureDate);

                if (LicenseConfiguration.RefreshLicenseAlwaysRequired ||
                    (LicenseConfiguration.RefreshLicenseAttemptFrequency > 0 &&
                        (dateDiff.TotalDays > LicenseConfiguration.RefreshLicenseAttemptFrequency ||
                        (LicenseConfiguration.RefreshLicenseRequireFrequency > 0 && dateDiff.TotalDays > LicenseConfiguration.RefreshLicenseRequireFrequency))))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>Gets whether the OnlineRefresh License is Required or not</summary>
        /// <remarks>
        /// <para>This method is called by license refreshing and status checking logic in the license implementation class
        /// (or the class or classes that implement <see cref="License"/> or <see cref="WritableLicense"/>).</para>
        /// </remarks>
        internal bool IsRefreshLicenseRequired
        {
            get
            {
                //Calculate the date difference between signature date and the current date
                TimeSpan dateDiff = DateTime.UtcNow.Subtract(SignatureDate);

                if (LicenseConfiguration.RefreshLicenseAlwaysRequired ||
                    (LicenseConfiguration.RefreshLicenseRequireFrequency > 0 && dateDiff.TotalDays > LicenseConfiguration.RefreshLicenseRequireFrequency))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>Gets whether or not the effective end date is within the time-limited license warning period.</summary>
        /// <returns>Returns true if the expiration is within the warning period.  Returns false if the license is outside of the warning period, or if the warning period is not applicable.</returns>
        internal bool IsTimeLimitedWarningRequired
        {
            get
            {
                //Just return false if the warning period is not configured or not applicable.
                if (LicenseConfiguration.TimeLimitedWarningDays <= 0 || TypeOfLicense == LicenseTypes.FullNonExpiring)
                    return false;

                //Determine the number of days left.
                double daysLeft = EffectiveEndDate.Subtract(DateTime.UtcNow).TotalDays;

                //If the license is not already expired and is within the warning period, return true.
                if (daysLeft >= 0 && daysLeft <= LicenseConfiguration.TimeLimitedWarningDays)
                    return true;

                return false;
            }
        }
        #endregion

        #region Internal Methods
        /// <summary>Generates a string containing a description of a license error.</summary>
        /// <remarks><para>This method is called from <see cref="GenerateLicenseStatusEntry"/>.</para></remarks>
        /// <returns>Returns a string containing the description of a license error.</returns>
        internal string GenerateLicenseErrorString()
        {
            StringBuilder status = new StringBuilder();

            switch (LastError.ErrorNumber)
            {
                case LicenseError.ERROR_COULD_NOT_LOAD_LICENSE:
                    status.Append(LastError.ErrorNumber);
                    status.Append(": ");
                    status.Append("License not found - activation is required.");
                    break;
                case LicenseError.ERROR_COULD_NOT_LOAD_VOLUME_DOWNLOADABLE_LICENSE:
                    status.Append(LastError.ErrorNumber);
                    status.Append(": ");
                    status.Append((ProductOption.OptionType == LicenseProductOption.ProductOptionType.VolumeLicense) ? "Volume" : "Downloadable");
                    status.Append(" license not found.");
                    break;
                case LicenseError.ERROR_LICENSE_NOT_EFFECTIVE_YET:
                    status.Append(LastError.ErrorNumber);
                    status.Append(": ");
                    if (ProductOption.OptionType == LicenseProductOption.ProductOptionType.DownloadableLicenseWithTriggerCodeValidation)
                    {
                        status.Append("Activation required.");
                        break;
                    }
                    status.Append("License not effective until ");
                    DateTime local = EffectiveStartDate.ToLocalTime();

                    int daysUntilEffective = (int)local.Subtract(DateTime.Now.Date).TotalDays;
                    if (1 < daysUntilEffective)
                    {
                        status.Append(local.ToLongDateString());
                        status.Append(" (");
                        status.Append(daysUntilEffective);
                        status.Append(" days).");
                    }
                    else if (1 == daysUntilEffective)
                    {
                        status.Append("tomorrow.");
                    }
                    else
                    {
                        status.Append(local.ToShortTimeString() + " today.");
                    }
                    break;
                case LicenseError.ERROR_LICENSE_EXPIRED:
                    status.Append(LastError.ErrorNumber);
                    status.Append(": ");
                    status.Append("License invalid or expired.");
                    break;
                case LicenseError.ERROR_WEBSERVICE_RETURNED_FAILURE:
                    //Web service error message.
                    status.Append(LastError.ExtendedErrorNumber);
                    status.Append(": ");
                    status.Append(LicenseError.GetWebServiceErrorMessage(LastError.ExtendedErrorNumber));
                    break;
                default:
                    //Show a standard error message.
                    status.Append(LastError.ErrorNumber);
                    status.Append(": ");
                    status.Append(LastError.ToString());
                    break;
            }

            return status.ToString();
        }

        /// <summary>Generates the <see cref="LicenseStatusEntry"/> objects displayed on the license management form.</summary>
        /// <remarks>
        /// <para>This method is called from the application's primary form.  The call to this method is typically located in a common method
        /// that is used to update controls on the form based on the new license status.  For example, this method is called "UpdateLicenseStatus"
        /// in the sample applications.</para>
        /// </remarks>
        /// <param name="lastValidationSuccessful">Whether or not the last license validation was successful.</param>
        /// <param name="licGui">The <see cref="LicensingGui"/> object which will display the status.</param>
        internal void InitializeLicenseStatusEntries(bool lastValidationSuccessful, LicensingGui licGui)
        {
            licGui.LicenseStatusEntries.Clear();
            licGui.LicenseStatusEntries.Add(GenerateLicenseStatusEntry(lastValidationSuccessful));

            //TODO: If you have other license parameters (such as network seats, allowed application/feature uses remaining, etc...),
            //      or modules/features which should have a status entry displayed, add the entries here. Some rough examples are
            //      provided in the comments immediately below.
            //licGui.LicenseStatusEntries.Add(new LicenseStatusEntry(LicenseStatusIcon.Ok, "Ok Test", "This is an Ok test item."));
            //licGui.LicenseStatusEntries.Add(new LicenseStatusEntry(LicenseStatusIcon.Information, "Information Test", "This is an Information test item."));
            //licGui.LicenseStatusEntries.Add(new LicenseStatusEntry(LicenseStatusIcon.Unavailable, "Unavailable Test", "This is an Unavailable test item."));
            //licGui.LicenseStatusEntries.Add(new LicenseStatusEntry(LicenseStatusIcon.Warning, "Warning Test", "This is an Warning test item."));
        }

        /// <summary>Generates the license status text to display.</summary>
        /// <remarks><para>This method is called from <see cref="InitializeLicenseStatusEntries"/>.</para></remarks>
        /// <returns>Returns a string containing the license status text displayed to the user.</returns>
        internal LicenseStatusEntry GenerateLicenseStatusEntry(bool lastValidationSuccessful)
        {
            LicenseStatusIcon entryIcon;
            StringBuilder status = new StringBuilder();

            if (lastValidationSuccessful)
            {
                //The license is valid, so just set the status text to "Ok."
                entryIcon = LicenseStatusIcon.Ok;
                if (LicenseTypes.Unlicensed == TypeOfLicense)
                    status.Append("Evaluation");
                else
                    status.Append("OK");

                if (ProductOption.OptionType == LicenseProductOption.ProductOptionType.VolumeLicense)
                {
                    status.Append(" (Volume License)");
                }
                else if (ProductOption.OptionType == LicenseProductOption.ProductOptionType.DownloadableLicenseWithTriggerCodeValidation)
                {
                    status.Append(" (Downloaded, Validated)");
                }

                //If it is a time-limited or evaluation license, then show additional information about its expiration.
                if (LicenseTypes.TimeLimited == TypeOfLicense ||
                    LicenseTypes.Unlicensed == TypeOfLicense)
                {
                    DateTime local = EffectiveEndDate.ToLocalTime();
                    TimeSpan timeLeft = local.Subtract(DateTime.Now.Date);

                    status.Append(" - Expires ");
                    if (1 > timeLeft.TotalDays)
                    {
                        status.Append(local.ToShortTimeString() + " today.");
                    }
                    else if (1 == (int)timeLeft.TotalDays)
                    {
                        status.Append("tomorrow.");
                    }
                    else
                    {
                        status.Append(local.ToLongDateString());
                        status.Append(" (" + ((int)timeLeft.TotalDays).ToString() + " days).");
                    }

                    if (timeLeft.TotalDays <= LicenseConfiguration.TimeLimitedWarningDays)
                    {
                        entryIcon = LicenseStatusIcon.Warning;
                    }
                }
            }
            else
            {
                entryIcon = LicenseStatusIcon.Error;
                status.Append(GenerateLicenseErrorString());
            }

            return new LicenseStatusEntry(entryIcon, "License", status.ToString());
        }

        /// <summary>Generates a string containing the overall license status for display on the main form of sample applications.</summary>
        /// <remarks>
        /// <para>This method is called from the application's primary form.  The call to this method is typically located in a common method
        /// that is used to update controls on the form based on the new license status.  For example, this method is called "UpdateLicenseStatus"
        /// in the sample applications.</para>
        /// </remarks>
        /// <param name="validationResult">The last validation result, which should be true only when the license has passed validation.</param>
        /// <returns>Returns a string containing description of the application's license status.</returns>
        internal string GenerateLicenseStatusString(bool validationResult)
        {
            //TODO: Customize this for your application if/as needed.
            //Update the status label on the form.
            StringBuilder status = new StringBuilder();
            if (validationResult)
            {
                if (LicenseTypes.Unlicensed == TypeOfLicense)
                {
                    status.AppendLine("Status: Evaluation");
                }
                else if (Customer.Unregistered)
                {
                    status.AppendLine("Status: Activated, unregistered.");
                }
                else
                {
                    status.AppendLine("Status: Activated, registered to:");
                    string name = "";
                    if (!string.IsNullOrEmpty(Customer.FirstName))
                        name = Customer.FirstName;
                    if (!string.IsNullOrEmpty(Customer.LastName))
                        name += " " + Customer.LastName;
                    status.AppendLine(name.Trim());
                    if (!string.IsNullOrEmpty(Customer.CompanyName))
                        status.AppendLine(Customer.CompanyName);
                }
            }
            else
            {
                status.AppendLine("Status: Invalid.  Click the \"Manage License\" button to activate.");
            }

            return status.ToString();
        }

        /// <summary>Processes a new license, which is received after successfully processing an activation or refresh action (either manually or online).</summary>
        /// <remarks>
        /// <para>This method is not applicable for trigger code activation.</para>
        /// <para>This method is called by <see cref="PostProcessingUpdates"/>.</para>
        /// </remarks>
        /// <param name="e">Event arguments</param>
        /// <param name="newValidationSuccessful">Whether or not the new license was validated successfully.</param>
        internal void ProcessNewLicense(ref LicenseManagementActionCompleteEventArgs e, out bool newValidationSuccessful)
        {
            //Save, reload, and revalidate the new license.
            if (!SaveLicenseFile(e.NewLicenseContent, true))
            {
                //The new license could not be saved.
                e.PostProcessingSuccessful = false;
                e.LastError = LastError;
                newValidationSuccessful = false;
                return;
            }

            //Now try to reload the new license.
            if (!LoadFile(LicenseConfiguration.LicenseFilePath))
            {
                //The new license could not be loaded.
                newValidationSuccessful = false;
                return;
            }

            //Perform any volume/downloadable license post-activation processing.
            ProcessNewVolumeDownloadableLicense(ref e);

            //Now re-validate the license.
            newValidationSuccessful = Validate();
        }

        /// <summary>Processes volume/downloadable license updates after an activation or refresh action has been processed (either manually or online).</summary>
        /// <remarks>
        /// <para>This method is not applicable for trigger code activation.</para>
        /// <para>This method is called by <see cref="ProcessNewLicense"/>.</para>
        /// </remarks>
        /// <param name="e">Event arguments</param>
        internal void ProcessNewVolumeDownloadableLicense(ref LicenseManagementActionCompleteEventArgs e)
        {
            //If this is a volume or downloadable license, we need to save it as such before revalidating it...
            if (ProductOption.OptionType != LicenseProductOption.ProductOptionType.VolumeLicense &&
                ProductOption.OptionType != LicenseProductOption.ProductOptionType.DownloadableLicenseWithTriggerCodeValidation)
            {
                //This is not a volume or downloadable license...

                try
                {
                    //If we activated a non-volume license after having already used a volume license file, try to delete it.
                    if (File.Exists(LicenseConfiguration.VolumeLicenseFilePath))
                        File.Delete(LicenseConfiguration.VolumeLicenseFilePath);
                }
                catch (Exception ex)
                {
                    //If we failed to delete the volume license file, show an error and re-initialize the license.  If we
                    //don't do this, the application would revert to using the volume license the next time it starts.
                    e.LastError = new LicenseError(LicenseError.ERROR_COULD_NOT_DELETE_FILE, ex);
                    e.PostProcessingSuccessful = false;
                    InitializeLicense();
                }

                return;
            }

            //If we reach this code, we activated a volume or downloadable license.
            if (IsWritable)
            {
                //We saved a writable license initially, so we need to also save the read-only copy of the volume license for validation purposes.
                if (!SaveVolumeLicenseFile(e.NewLicenseContent))
                {
                    //The volume license could not be saved.
                    e.LastError = LastError;
                    e.PostProcessingSuccessful = false;
                }
            }
            else
            {
                try
                {
                    //Move/rename the read-only license file saved initially file for consistency.
                    File.Move(LicenseConfiguration.LicenseFilePath, LicenseConfiguration.VolumeLicenseFilePath);
                }
                catch (Exception)
                {
                    //If we failed to move the file, just ignore it since it should still load without issue.
                }
            }
        }

        /// <summary>Refreshes the license file.</summary>
        /// <returns>Returns true if the license file was refreshed successfully.  If false is returned, see the <see cref="License.LastError"/> property for details.</returns>
        internal bool RefreshLicense()
        {
            string licenseContent = "";

            //initialize the object used for calling the web service method
            using (XmlLicenseFileService ws = m_Settings.CreateNewXmlLicenseFileServiceObject())
            {
                if (null == ws)
                {
                    LastError = new LicenseError(LicenseError.ERROR_WEBSERVICE_INVALID_CONFIGURATION);
                    return false;
                }

                if (!base.RefreshLicense(ws, ref licenseContent))
                {
                    if (SampleLicense.ShouldLicenseBeRevoked(LastError.ExtendedErrorNumber))
                    {
                        RemoveLicense();
                    }
                    return false;
                }
            }

            //try to save the license file to the file system
            if (!SaveLicenseFile(licenseContent, false))
                return false;

            return true;
        }

        /// <summary>Saves a new license file to the file system.</summary>
        /// <param name="licenseContent">The new license file content to save to disk.</param>
        /// <returns>Returns true if the license file was saved successfully.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool SaveVolumeLicenseFile(string licenseContent)
        {
            try
            {
                File.WriteAllText(LicenseConfiguration.VolumeLicenseFilePath, licenseContent);
            }
            catch (Exception ex)
            {
                LastError = new LicenseError(LicenseError.ERROR_COULD_NOT_SAVE_LICENSE, ex);
                return false;
            }

            return true;
        }
        #endregion

        #region Internal Static Methods
        /// <summary>Creates a new <see cref="SampleLicense"/></summary>
        /// <param name="gui">The <see cref="LicensingGui"/> object used by the main form.</param>
        /// <returns>Returns a new <see cref="SampleLicense"/> object.</returns>
        internal static SampleLicense CreateNewLicense(LicensingGui gui)
        {
            return new SampleLicense(gui);
        }

        /// <summary>Determines the type of license.</summary>
        /// <remarks>
        /// <para>This method is called from any code or logic that needs to know the type of license which was issued/activated.
        /// This includes areas of code that update your application's main form's controls, anything that compiles and/or reports
        /// the license status, and code that validates the license.</para>
        /// </remarks>
        /// <param name="lic">The <see cref="License"/> object.</param>
        /// <returns>Returns the type of license.</returns>
        internal static LicenseTypes DetermineLicenseType(License lic)
        {
            LicenseTypes type;

            switch (lic.TriggerCode)
            {
                case 1:
                case 6: //Network Floating
                case 18:
                case 28:
                    type = LicenseTypes.FullNonExpiring;
                    break;
                case 10:
                case 11:
                case 29:
                    type = LicenseTypes.TimeLimited;
                    break;
                default:
                    type = LicenseTypes.Unlicensed;
                    break;
            }

            return type;
        }

        /// <summary>Determines whether or not the system clock has been backdated during runtime using a date logged at runtime.</summary>
        /// <remarks>
        /// <para>This validation helps prevent users from backdating the clock while the application is running to gain unauthorized access to your application and its features.</para>
        /// <note type="caution">
        /// <para>TODO: IMPORTANT:</para>
        /// <para>Make sure your application checks for back-dating (and for any failed license validation) before allowing any important/critical code or features to run.  This
        /// is critical because it is possible to click controls and run their code even when hidden or disabled.</para>
        /// <para>Additionally, make sure the <see cref="LicenseConfiguration.RuntimeBackdateThresholdSeconds"/> property is configured with a threshold that is both secure
        /// enough to prevent users from reasonably back-dating their clocks while also avoiding potentially raising false alarms when a system clock changes as a result of
        /// a typical synchronization with Internet time (NTP).</para>
        /// </note>
        /// </remarks>
        /// <param name="lastLoggedUtcTime">The last UTC date/time logged at runtime.  This is usually logged immediately after license validation occurs.</param>
        /// <returns>Returns true if the clock appears to have been back-dated while the application was running, or false if the clock appears to have a valid date/time.</returns>
        internal static bool IsClockBackdatedAtRuntime(DateTime lastLoggedUtcTime)
        {
            return (DateTime.UtcNow.Subtract(lastLoggedUtcTime).TotalSeconds <= (-1 * LicenseConfiguration.RuntimeBackdateThresholdSeconds));
        }

        /// <summary>Performs post-processing updates for actions performed through the <see cref="LicensingGui"/>.</summary>
        /// <remarks><para>This method is called by the <see cref="LicensingGui.LicenseManagementActionComplete"/> event handler method.  In the
        /// sample application form source code (i.e. MainForm.cs), this method is similarly named PostProcessingUpdates.  This might be named
        /// something like licensingGui1_LicenseManagementActionComplete in your application.</para></remarks>
        /// <param name="lic">The <see cref="License"/> being processed.</param>
        /// <param name="licGui">The <see cref="LicensingGui"/> used to process updates for the license.</param>
        /// <param name="e">The post-processing event arguments returned from the <see cref="LicensingGui"/> object.</param>
        /// <param name="newValidationSuccessful">Whether or not validation after processing updates has passed successfully.</param>
        internal static void PostProcessingUpdates(ref SampleLicense lic, ref LicensingGui licGui, ref LicenseManagementActionCompleteEventArgs e, out bool newValidationSuccessful)
        {
            //If an action was processed, run some post-processing actions.
            if (LicenseManagementActionTypes.ManualTriggerCode == e.ActionType)
            {
                //A trigger code needs to be processed...
                if (!lic.ProcessTriggerCode(e.LicenseID, e.Password, e.TriggerCodeNumber, e.TriggerCodeEventData))
                {
                    //If trigger code processing failed, show the error in the license management dialog.
                    e.PostProcessingSuccessful = false;
                    e.LastError = lic.LastError;
                }

                //Reload and revalidate the license file.
                if (lic.LoadFile(LicenseConfiguration.LicenseFilePath))
                {
                    newValidationSuccessful = lic.Validate();
                }
                else
                {
                    newValidationSuccessful = false;
                }
            }
            else if (LicenseError.ERROR_WEBSERVICE_RETURNED_FAILURE == e.LastError.ErrorNumber)
            {
                //The last action failed because the license is no longer valid in SOLO Server, so remove the license from the system.
                if (ShouldLicenseBeRevoked(lic.LastError.ExtendedErrorNumber))
                {
                    RevokeLicense(ref lic, ref licGui);
                    newValidationSuccessful = false;
                }
                else
                {
                    //The web service returned an error, which should be disabled to the user.
                    e.PostProcessingSuccessful = false;
                    e.LastError = lic.LastError;
                }
            }
            else if (LicenseManagementActionTypes.OnlineDeactivation == e.ActionType ||
                LicenseManagementActionTypes.ManualDeactivation == e.ActionType)
            {
                RevokeLicense(ref lic, ref licGui);
                newValidationSuccessful = false;
                return;
            }
            else if (e.ProcessedSuccessfully &&
                (LicenseManagementActionTypes.OnlineActivation == e.ActionType ||
                LicenseManagementActionTypes.OnlineRefresh == e.ActionType ||
                LicenseManagementActionTypes.ManualActivation == e.ActionType ||
                LicenseManagementActionTypes.ManualRefresh == e.ActionType))
            {
                //This was a successful activation or license refresh attempt, which means we have a new license to process.
                lic.ProcessNewLicense(ref e, out newValidationSuccessful);
            }

            newValidationSuccessful = lic.Validate();
        }

        /// <summary>Revokes the license.</summary>
        /// <remarks><para>This method is called from <see cref="PostProcessingUpdates"/>.</para></remarks>
        /// <param name="lic">The <see cref="License"/> object being revoked.</param>
        /// <param name="licGui">The <see cref="LicensingGui"/> which references the <see cref="License"/> object.</param>
        internal static void RevokeLicense(ref SampleLicense lic, ref LicensingGui licGui)
        {
            //The license has been revoked, so remove it from the system.
            lic.RemoveLicense();

            //Create a new License to clear out any data (like the License ID and Installation ID) from memory.
            lic = SampleLicense.CreateNewLicense(licGui);

            //Now try to load the license file even though it doesn't exist.  This helps ensure the correct error
            //message is displayed in the license status entry if the license management form is opened again.
            lic.LoadFile(LicenseConfiguration.LicenseFilePath);

            //Update the reference to the License object in case the license management form is opened again.
            licGui.ApplicationLicense = lic;
        }

        /// <summary>Loads/restores a manual action's session-state.</summary>
        /// <remarks>
        /// <para>This method is called by the <see cref="LicensingGui.ManualActionLoadRequested"/> handler method.  In the sample application
        /// form source code (i.e. MainForm.cs), this method is similarly named ManualActionLoadRequested.  This might be named something like
        /// licensingGui1_ManualActionLoadRequested in your application.</para>
        /// </remarks>
        /// <returns>Returns the unencrypted contents of the session.</returns>
        internal static string LoadManualActionSessionState()
        {
            string data = "";
            try
            {
                //Load the data needed to restore the manual action's state, which allows the response to be processed after the application has been closed.
                data = File.ReadAllText(LicenseConfiguration.ManualActionSessionStateFilePath);

                //Delete the file after we have loaded it.
                File.Delete(LicenseConfiguration.ManualActionSessionStateFilePath);

                //TODO: IMPORTANT: Make sure you update the ManualActionIV and ManualActionKey properties with randomized data which should be unique for each of your applications.
                //Decrypt the file contents.
                data = DecryptManualActionData(data);
            }
            catch (Exception)
            {
                data = "";
            }

            //Return the data.
            return data;
        }

        /// <summary>Saves a manual action's session-state.</summary>
        /// <remarks>
        /// <para>This method is called by the <see cref="LicensingGui.ManualActionSaveRequested"/> handler method.  In the sample application
        /// form source code (i.e. MainForm.cs), this method is similarly named ManualActionLoadRequested.  This might be named something like
        /// licensingGui1_ManualActionSaveRequested in your application.</para>
        /// </remarks>
        /// <param name="data">The unencrypted contents to save.</param>
        internal static void SaveManualActionSessionState(string data)
        {
            try
            {
                //The user closed the dialog or clicked resume later, so try to save the manual action's state.
                File.WriteAllText(LicenseConfiguration.ManualActionSessionStateFilePath, EncryptManualActionData(data));
            }
            catch (Exception)
            {
                //do nothing
            }
        }

        /// <summary>Determines whether or not the current license should be revoked based on the result code returned from a SOLO Server web service.</summary>
        /// <remarks><para>This method is called from <see cref="PostProcessingUpdates"/> and in license refreshing and status checking logic in the
        /// license implementation class (or the class or classes that implement <see cref="License"/> or <see cref="WritableLicense"/>).</para></remarks>
        /// <param name="webServiceResultCode">The result code returned from a SOLO Server web service.</param>
        /// <returns>Returns true only if the license should be revoked.</returns>
        internal static bool ShouldLicenseBeRevoked(int webServiceResultCode)
        {
            bool shouldRevoke;

            switch (webServiceResultCode)
            {
                case 5010: //Invalid Product ID
                case 5015: //Invalid Installation ID
                case 5016: //Deactivated installation
                case 5017: //Invalid license status
                    //case 5022: //Uncomment this line to revoke the license when SOLO Server determines that the system time is invalid.  Enabling this extra check could be problematic if users accidentally or intentionally alter their system clock.
                    shouldRevoke = true;
                    break;
                default:
                    shouldRevoke = false;
                    break;
            }

            return shouldRevoke;
        }
        #endregion

        #region Private Static Methods
        /// <summary>Encrypts a string.</summary>
        /// <param name="data">The string to encrypt.</param>
        /// <remarks>
        /// <para>This method is called by <see cref="SaveManualActionSessionState"/>.</para>
        /// <para>If you run code analysis on this project, you may find a warning about the encryptedStream variable being
        /// disposed multiple times.  This warning is extraneous, is not indicative of any bugs, and should be ignored.  This warning
        /// may be suppressed safely using a SuppressMessage attribute like:
        /// </para>
        /// <para><code>[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]</code></para>
        /// <para>Additional code analysis warnings may occur with this method when running the analysis against the release build (which is not recommended).</para>
        /// </remarks>
        /// <returns>Returns an encrypted, base64 encoded string.</returns>
        private static string EncryptManualActionData(string data)
        {
            string encryptedString = "";
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                using (RijndaelManaged algorithm = new RijndaelManaged())
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(Convert.FromBase64String(LicenseConfiguration.ManualActionKey), Convert.FromBase64String(LicenseConfiguration.ManualActionIV)))
                using (MemoryStream encryptedStream = new MemoryStream())
                using (CryptoStream cipherStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                {
                    // write all data to the crypto stream and flush it.
                    cipherStream.Write(dataBytes, 0, dataBytes.Length);
                    cipherStream.FlushFinalBlock();

                    // get the encrypted byte array
                    encryptedString = Convert.ToBase64String(((MemoryStream)encryptedStream).ToArray());
                }
            }
            catch (Exception)
            {
                encryptedString = "";
            }

            return encryptedString;
        }

        /// <summary>Decrypts manual action data.</summary>
        /// <param name="data">A base64 encoded string containing data to decrypt.</param>
        /// <remarks>
        /// <para>This method is called by <see cref="LoadManualActionSessionState"/>.</para>
        /// <para>If you run code analysis on this project, you may find a warning about the decryptedStream variable being
        /// disposed multiple times.  This warning is extraneous, is not indicative of any bugs, and should be ignored.  This warning
        /// may be suppressed safely using a SuppressMessage attribute like:
        /// </para>
        /// <para><code>[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]</code></para>
        /// <para>Additional code analysis warnings may occur with this method when running the analysis against the release build (which is not recommended).</para>
        /// </remarks>
        /// <returns>Returns the decrypted data.</returns>
        private static string DecryptManualActionData(string data)
        {
            string decryptedString = "";
            try
            {
                byte[] dataBytes = Convert.FromBase64String(data);
                using (RijndaelManaged algorithm = new RijndaelManaged())
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor(Convert.FromBase64String(LicenseConfiguration.ManualActionKey), Convert.FromBase64String(LicenseConfiguration.ManualActionIV)))
                using (MemoryStream decryptedStream = new MemoryStream(dataBytes))
                using (CryptoStream cipherStream = new CryptoStream(decryptedStream, decryptor, CryptoStreamMode.Read))
                {
                    // read the data out of the crypto stream.
                    byte[] decryptedValue = new byte[dataBytes.Length];
                    cipherStream.Read(decryptedValue, 0, decryptedValue.Length);

                    StringBuilder plain = new StringBuilder();
                    foreach (char character in Encoding.UTF8.GetChars(decryptedValue))
                        plain.Append(character);
                    decryptedString = plain.ToString();
                }
            }
            catch (Exception)
            {
                decryptedString = "";
            }

            return decryptedString;
        }
        #endregion
    }
}
