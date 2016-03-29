using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using com.softwarekey.Client.Gui;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.Licensing.Network;
using com.softwarekey.Client.Utils;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Network floating license implementation, which implements the License class for read-only licenses (which must be created and signed by SOLO Server or License Manager).</summary>
    internal partial class SampleLicense : License
    {
        #region Constructors
        /// <summary>Default NetworkFloatingLicense constructor</summary>
        internal SampleLicense(LicensingGui gui)
            : base(LicenseConfiguration.EncryptionKey, true, true, LicenseConfiguration.ThisProductID, "", new List<SystemIdentifierAlgorithm>(new SystemIdentifierAlgorithm[] { new NetworkNameIdentifierAlgorithm(Path.GetDirectoryName(LicenseConfiguration.LicenseFilePath.ToLowerInvariant())) }))
        {
            this.ProductVersion = IOHelper.GetAssemblyFileVersion(System.Reflection.Assembly.GetEntryAssembly());

            m_Settings = gui;
        }
        #endregion

        #region Internal Property
        /// <summary>Gets if the license is writable or not</summary>
        internal bool IsWritable
        {
            get { return false; }
        }
        #endregion

        #region Internal Methods
        /// <summary>Generates a string containing the license status and network seats status for display on the main form.</summary>
        /// <param name="validationResult">whether or not last license validation was successful</param>
        /// <param name="semaphore">The NetworkSemaphore object that contains total seats count and active/used seats count</param>
        /// <returns>Returns a string that contains description of the license</returns>
        internal string GenerateNetworkLicenseStatusString(bool validationResult, NetworkSemaphore semaphore)
        {
            StringBuilder status = new StringBuilder();
            status.AppendLine(GenerateLicenseStatusString(validationResult));

            if (validationResult && semaphore != null)
            {
                status.AppendLine("Seats Used: " + semaphore.SeatsActive.ToString() + " out of " + semaphore.SeatsTotal.ToString());
            }
            return status.ToString();
        }

        /// <summary>Loads and validates the license</summary>
        /// <returns>Returns true if the initialization is successful, false when unable to load file.</returns>
        internal bool InitializeLicense()
        {
            //Load the license file.
            bool successful = LoadFile(LicenseConfiguration.LicenseFilePath);

            if (successful)
            {
                //The license was loaded, so run the validation.
                successful = Validate();
            }
            else
            {
                //Validation failed because the license could not even be loaded.
                successful = false;
            }

            return successful;
        }

        /// <summary>Generates the LicenseStatusEntry objects containing network seats status, which displayed in the license management form</summary>
        /// <param name="validationResult">whether or not last license validation was successful</param>
        /// <param name="licensingGui">The LicensingGui object which will display the status</param>
        /// <param name="semaphore">The NetworkSemaphore object that contains total seats count and active/used seats count</param>
        internal void InitializeNetworkLicenseStatusEntries(bool validationResult, LicensingGui licensingGui, NetworkSemaphore semaphore)
        {
            InitializeLicenseStatusEntries(validationResult, licensingGui);
            if (validationResult && semaphore != null)
            {
                licensingGui.LicenseStatusEntries.Add(new LicenseStatusEntry(LicenseStatusIcon.Information, "Network Seats", "Seats Used: " + semaphore.SeatsActive.ToString() + " out of " + semaphore.SeatsTotal.ToString()));
            }
        }

        /// <summary>Processes a Protection PLUS compatible trigger code.</summary>
        /// <param name="tcLicenseID">The License ID entered by the user.</param>
        /// <param name="tcPassword">The password entered by the user.</param>
        /// <param name="tcNumber">The trigger code number to process</param>
        /// <param name="tcEventData">The trigger code event data</param>
        /// <returns>Returns true if successful.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool ProcessTriggerCode(int tcLicenseID, string tcPassword, int tcNumber, int tcEventData)
        {
            //Trigger codes are not supported with read-only licenses, so return false.
            LastError = new LicenseError(LicenseError.ERROR_TRIGGER_CODE_INVALID);
            return false;
        }

        /// <summary>Deletes the license file from the Path where it is stored.</summary>
        /// <returns>Returns true if File deletion was successful, false otherwise</returns>
        internal bool RemoveLicense()
        {
            bool successful = true;
            try
            {
                File.Delete(LicenseConfiguration.LicenseFilePath);
            }
            catch (Exception ex)
            {
                LastError = new LicenseError(LicenseError.ERROR_COULD_NOT_DELETE_FILE, ex);
                successful = false;
            }

            return successful;
        }

        /// <summary>Saves a new license file to the file system.</summary>
        /// <param name="licenseContent">The new license file content to save to disk.</param>
        /// <returns>Returns true if the license file was saved successfully.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool SaveLicenseFile(string licenseContent)
        {
            return SaveLicenseFile(licenseContent, false);
        }

        /// <summary>Saves a new license file to the file system.</summary>
        /// <param name="licenseContent">The new license file content to save to disk.</param>
        /// <param name="forceAliasUpdates">Whether or not aliases should be updated even when the system clock appears to have been back-dated.  This parameter is ignored with read-only licenses.</param>
        /// <returns>Returns true if the license file was saved successfully.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool SaveLicenseFile(string licenseContent, bool forceAliasUpdates)
        {
            try
            {
                File.WriteAllText(LicenseConfiguration.LicenseFilePath, licenseContent);
            }
            catch (Exception ex)
            {
                LastError = new LicenseError(LicenseError.ERROR_COULD_NOT_SAVE_LICENSE, ex);
                return false;
            }

            return true;
        }

        /// <summary>Validates the license</summary>
        /// <returns>Returns true if the license is valid, false otherwise.</returns>
        internal bool Validate()
        {
            //If OptionType is Downloadable or Volume, return error.
            if (ProductOption.OptionType == LicenseProductOption.ProductOptionType.DownloadableLicenseWithTriggerCodeValidation || ProductOption.OptionType == LicenseProductOption.ProductOptionType.VolumeLicense)
            {
                LastError = new LicenseError(LicenseError.ERROR_INVALID_LICENSE_TYPE);
                return false;
            }

            //Refresh the license if date difference is greater than m_RefreshLicenseAttemptFrequency (days)
            if (this.IsRefreshLicenseAttemptDue)
            {
                //If refresh license fails and date difference is greater than m_RefreshLicenseRequireFrequency (days) set license as invalid
                if (!RefreshLicense() &&
                    (LastError.ErrorNumber != LicenseError.ERROR_WEBSERVICE_CALL_FAILED || IsRefreshLicenseRequired))
                {
                    return false;
                }
            }

            //Create a list of validations to perform.
            List<SystemValidation> validations = new List<SystemValidation>();

            //Make sure there is no active system clock tampering occurring.
            validations.Add(new SystemClockValidation());

            //Validate the Product ID authorized in the license to make sure the license file was issued for this application.
            validations.Add(new LicenseProductValidation(this, ThisProductID));

            //Validate the current identifiers.
            validations.Add(new SystemIdentifierValidation(
                AuthorizedIdentifiers,
                CurrentIdentifiers,
                SystemIdentifierValidation.REQUIRE_EXACT_MATCH));

            //Add a validation to make sure this system is authorized to use the activated license.  (This implements copy-protection.)
            if (TypeOfLicense == LicenseTypes.TimeLimited)
            {
                //If the license is time-limited, make sure it is within its effective date/time period.
                validations.Add(new LicenseEffectiveDateValidation(this));
            }

            //Run all of the validations (in the order they were added), and make sure all of them succeed.
            foreach (SystemValidation validation in validations)
            {
                if (!validation.Validate())
                {
                    LastError = validation.LastError;
                    return false;
                }
            }

            //If we got this far, all validations were successful, so return true to indicate success and a valid license.
            return true;
        }
        #endregion
    }
}
