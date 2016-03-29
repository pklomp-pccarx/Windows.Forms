using System;
using System.Collections.Generic;
using System.IO;
using com.softwarekey.Client.Gui;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.WebService.XmlLicenseFileService;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Example read-only license implementation, which uses licenses issued, encrypted, and digitally signed by SOLO Server or License Manager.</summary>
    /// <remarks>
    /// <note type="caution">
    /// <para>IMPORTANT: his code depends on the data from the LicenseConfiguration class.  Make sure you update the members in LicenseConfiguration
    /// if you copy it into your application! This is necessary to ensure your application is properly secured, and prevents your application from
    /// being activated with licenses generated for other applications.</para>
    /// </note>
    /// </remarks>
    internal partial class SampleLicense : License
    {
        //TODO: IMPORTANT: This code depends on the data from the LicenseConfiguration class.  Make sure you update the members in LicenseConfiguration if you copy it into your application!
        #region Constructors
        /// <summary>Creates a new <see cref="SampleLicense"/>.</summary>
        /// <param name="settings">The <see cref="LicensingGui"/> object, which may be used to obtain proxy authentication credentials from the user.</param>
        internal SampleLicense(LicensingGui settings)
            : base(LicenseConfiguration.EncryptionKey, true, true, LicenseConfiguration.ThisProductID, LicenseConfiguration.ThisProductVersion, LicenseConfiguration.SystemIdentifierAlgorithms)
        {
            m_Settings = settings;
        }
        #endregion

        #region Internal Properties
        /// <summary>Gets whether or not this type of license is writable.</summary>
        internal bool IsWritable
        {
            get { return false; }
        }
        #endregion

        #region Internal Methods
        /// <summary>Loads and initializes the license.</summary>
        /// <returns>Returns true if successful.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool InitializeLicense()
        {
            //Load the license file...
            //The code below will load the volume license file if it exists, or the  regular license file
            //if no volume license file is present. This is primarily here because the sample SOLO Server
            //product option specifies a name of "VolumeLicenseFile.xml" for volume licenses.  So for
            //example, the ReadOnlyLicense will properly handle a volume license file named LicenseFile.xml
            //instead of VolumeLicenseFile.xml.  The use of a separate file name has more to do with the
            //need to save the read-only, volume license files separately in other samples that also make
            //use of writable/self-signed license implementations.
            bool successful = LoadFile((File.Exists(LicenseConfiguration.VolumeLicenseFilePath) ?
                LicenseConfiguration.VolumeLicenseFilePath :
                LicenseConfiguration.LicenseFilePath));

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

        /// <summary>Processes a trigger code.</summary>
        /// <param name="tcLicenseID">The License ID entered by the user.</param>
        /// <param name="tcPassword">The password entered by the user.</param>
        /// <param name="tcNumber">The trigger code number to process.</param>
        /// <param name="tcEventData">The trigger code event data.</param>
        /// <returns>Returns true if successful.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool ProcessTriggerCode(int tcLicenseID, string tcPassword, int tcNumber, int tcEventData)
        {
            //Trigger codes are not supported with read-only licenses, so return false.
            LastError = new LicenseError(LicenseError.ERROR_TRIGGER_CODE_INVALID);
            return false;
        }

        /// <summary>Removes a license from the system.  This should be done when the license is deactivated or is found to be invalid on SOLO Server.</summary>
        /// <returns>Returns true if it was completed successfully.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
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

        /// <summary>Validates the license.</summary>
        /// <returns>Returns true if validation is successful and the license is valid.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool Validate()
        {
            //Make sure the option type is supported.
            if (ProductOption.OptionType == LicenseProductOption.ProductOptionType.DownloadableLicenseWithTriggerCodeValidation)
            {
                LastError = new LicenseError(LicenseError.ERROR_INVALID_LICENSE_TYPE);
                return false;
            }

            //TODO: If you wish to have your volume licenses automatically refresh with SOLO Server, you may
            //      enable this by removing the second expression in the 'if' statement immediately below.
            if (IsRefreshLicenseAttemptDue &&
                ProductOption.OptionType != LicenseProductOption.ProductOptionType.VolumeLicense)
            {
                //If a refresh attempt should be made, try to perform a license refresh with SOLO Server.
                if (!RefreshLicense() &&
                    (LastError.ErrorNumber != LicenseError.ERROR_WEBSERVICE_CALL_FAILED || IsRefreshLicenseRequired))
                {
                    //The refresh failed and was required, or SOLO Server returned an error.
                    return false;
                }
            }

            //Create a list of validations to perform.
            List<SystemValidation> validations = new List<SystemValidation>();

            //Add a validation to make sure there is no active system clock tampering taking place.
            validations.Add(new SystemClockValidation());

            //Validate the Product ID authorized in the license to make sure the license file was issued for this application.
            validations.Add(new LicenseProductValidation(this, ThisProductID));

            //Validate the current identifiers by default.
            List<SystemIdentifier> validationIdentifiers = CurrentIdentifiers;
            if (ProductOption.OptionType == LicenseProductOption.ProductOptionType.VolumeLicense)
            {
                //This is a volume license, so only validate a single identifier for the License ID.
                LicenseIDIdentifierAlgorithm alg = new LicenseIDIdentifierAlgorithm();
                validationIdentifiers = alg.GetIdentifier(LicenseID);
            }

            //Add a validation to make sure this system is authorized to use the activated license.  (This implements copy-protection.)
            validations.Add(new SystemIdentifierValidation(
                AuthorizedIdentifiers,
                validationIdentifiers,
                SystemIdentifierValidation.REQUIRE_EXACT_MATCH));

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

        internal void UnloadLicense()
        {
            //Do nothing since this is a read-only license.  This is where aliases are updated before the application is closed when using writable licenses.
        }
        #endregion
    }
}
