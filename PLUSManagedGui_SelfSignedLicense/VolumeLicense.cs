using System;
using System.Collections.Generic;
using System.IO;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.WebService.XmlLicenseFileService;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Example read-only license implementation, which uses licenses issued, encrypted, and digitally signed by SOLO Server or License Manager.</summary>
    /// <remarks>
    /// <note type="caution">
    /// <para>IMPORTANT: his code depends on the data from the LicenseConfiguration class.  Make sure you update the members in that class if you copy it
    /// into your application! This is necessary to ensure your application is properly secured, and prevents your application from being
    /// activated with licenses generated for other applications.</para>
    /// </note>
    /// <para>This class is typically only used when a writable/self-signed license implementation is used.  This is because the example ReadOnlyLicense
    /// implementation already includes support for volume licenses, which are always read-only.</para>
    /// </remarks>
    internal class VolumeLicense : License
    {
        //TODO: IMPORTANT: This code depends on the data from the LicenseConfiguration class.  Make sure you update the members in that class if you copy it into your application!
        #region Constructors
        /// <summary>Creates a new <see cref="VolumeLicense"/>.</summary>
        internal VolumeLicense()
            : base(LicenseConfiguration.EncryptionKey, true, true, LicenseConfiguration.ThisProductID, LicenseConfiguration.ThisProductVersion, LicenseConfiguration.VolumeSystemIdentifierAlgorithms)
        {
        }
        #endregion

        #region Public Override Methods
        /// <summary>Load method override which initializes the <see cref="LicenseIDIdentifier"/> after loading new license content.</summary>
        /// <param name="data">The license content being loaded.</param>
        /// <returns>Returns true if successful.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        public override bool Load(string data)
        {
            bool result = base.Load(data);

            //Initialize the identifier needed to validate the volume license.
            LicenseIDIdentifierAlgorithm algorithm = new LicenseIDIdentifierAlgorithm();
            CurrentIdentifiers.Add(algorithm.GetIdentifier(LicenseID)[0]);

            return result;
        }

        /// <summary>Loads the volume license file.</summary>
        /// <param name="path">The absolute path to the volume license file.</param>
        /// <returns>Returns true if successful.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        public bool LoadVolumeLicenseFile(string path)
        {
            bool successful = base.LoadFile(path);
            if (!successful && LastError.ErrorNumber == LicenseError.ERROR_COULD_NOT_LOAD_LICENSE)
            {
                LastError = new LicenseError(LicenseError.ERROR_COULD_NOT_LOAD_VOLUME_DOWNLOADABLE_LICENSE, LastError.ErrorException);
            }

            return successful;
        }
        #endregion

        #region Internal Properties
        /// <summary>Determines the type of license issued.</summary>
        internal LicenseTypes TypeOfLicense
        {
            get { return SampleLicense.DetermineLicenseType(this); }
        }
        #endregion

        #region Internal Methods
        /// <summary>Saves a new license file to the file system.</summary>
        /// <param name="licenseContent">The new license file content to save to disk.</param>
        /// <returns>Returns true if the license file was saved successfully.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool SaveLicenseFile(string licenseContent)
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

        /// <summary>Refreshes the license file.</summary>
        /// <returns>Returns true if the license file was refreshed successfully.  If false is returned, see the <see cref="License.LastError"/> property for details.</returns>
        internal bool RefreshLicense(XmlLicenseFileService webservice)
        {
            if (null == webservice)
            {
                LastError = new LicenseError(LicenseError.ERROR_WEBSERVICE_INVALID_CONFIGURATION);
                return false;
            }

            string licenseContent = "";
            if (!base.RefreshLicense(webservice, ref licenseContent))
            {
                if (SampleLicense.ShouldLicenseBeRevoked(LastError.ExtendedErrorNumber))
                {
                    if (!RemoveLicense())
                        return false;
                }
                return false;
            }

            //try to save the license file to the file system
            if (!SaveLicenseFile(licenseContent))
                return false;

            return true;
        }

        /// <summary>Removes a license from the system.  This should be done when the license is deactivated or is found to be invalid on SOLO Server.</summary>
        /// <returns>Returns true if it was completed successfully.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool RemoveLicense()
        {
            bool successful = true;
            try
            {
                File.Delete(LicenseConfiguration.VolumeLicenseFilePath);
            }
            catch (Exception ex)
            {
                LastError = new LicenseError(LicenseError.ERROR_COULD_NOT_DELETE_FILE, ex);
                successful = false;
            }

            return successful;
        }

        /// <summary>Validates the license.</summary>
        /// <returns>Returns true if validation is successful and the license is valid.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool Validate()
        {
            //Create a list of validations to perform.
            List<SystemValidation> validations = new List<SystemValidation>();

            //Add a validation to make sure there is no active system clock tampering taking place.
            validations.Add(new SystemClockValidation());

            //Add a validation to make sure this system is authorized to use the activated license.
            validations.Add(new SystemIdentifierValidation(
                AuthorizedIdentifiers,
                CurrentIdentifiers,
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
        #endregion
    }
}
