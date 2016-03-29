﻿using System;
using System.Collections.Generic;
using System.IO;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.WebService.XmlActivationService;
using com.softwarekey.Client.WebService.XmlLicenseFileService;

namespace com.softwarekey.Client.Sample
{
    /// <summary>A sample <see cref="License"/>
    /// <see href="http://www.softwarekey.com/help/plus5/#StartTopic=Content/PLUSManaged_License_Implementation.htm">implementation class</see>,
    /// which uses read-only licenses, and uses semaphore files for network floating licensing.</summary>
    /// <remarks>
    /// <note type="caution"><para>IMPORTANT: When using Network Floating Licensing features (via the NetworkSemaphore class) with protected applications,
    /// Windows Vista/Server 2008 or later is required for both the client computers accessing the protected application, and the file server hosting the
    /// share where the semaphore files will be stored.</para></note>
    /// </remarks>
    public class SampleLicense : License
    {
        #region Private Member Variables
        private static WebServiceHelper m_WebServiceHelper = new WebServiceHelper();
        #endregion

        #region Constructors
        /// <summary>Creates a new <see cref="SampleLicense"/> object.</summary>
        public SampleLicense()
            : base(LicenseConfiguration.EncryptionKey, true, true, LicenseConfiguration.ThisProductID, LicenseConfiguration.ThisProductVersion,
                    new List<SystemIdentifierAlgorithm> (new SystemIdentifierAlgorithm[] { new NetworkNameIdentifierAlgorithm(Path.GetDirectoryName(LicenseConfiguration.LicenseFilePath.ToLowerInvariant())) }))
        {
        }
        #endregion

        #region Private Properties
        /// <summary>Gets whether a license refresh attempt is due.</summary>
        private bool IsRefreshLicenseAttemptDue
        {
            get
            {
                //Calculate the date difference between signature date and the current date
                TimeSpan dateDiff = DateTime.UtcNow.Subtract(this.SignatureDate);

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

        /// <summary>Gets whether the a license refresh is required.</summary>
        private bool IsRefreshLicenseRequired
        {
            get
            {
                //Calculate the date difference between signature date and the current date
                TimeSpan dateDiff = DateTime.UtcNow.Subtract(this.SignatureDate);

                if (LicenseConfiguration.RefreshLicenseAlwaysRequired ||
                    (LicenseConfiguration.RefreshLicenseRequireFrequency > 0 && dateDiff.TotalDays > LicenseConfiguration.RefreshLicenseRequireFrequency))
                {
                    return true;
                }

                return false;
            }
        }
        #endregion

        #region Internal Methods
        /// <summary>Activates the license online.</summary>
        /// <param name="licenseId">The License ID issued by SOLO Server.</param>
        /// <param name="password">The customer password.</param>
        /// <returns>Returns true if the system was activated successfully. If false is returned, evaluate the <see cref="License.LastError"/> property to determine the reason why activation failed.</returns>
        internal bool ActivateOnline(Int32 licenseId, string password)
        {
            string licenseContent = "";

            //initialize the object used for calling the web service method
            XmlActivationService ws = m_WebServiceHelper.CreateXmlActivationServiceObject();
            if (null == ws)
            {
                this.LastError = m_WebServiceHelper.LastError;
                return false;
            }

            //activate online using our endpoint configuration from app.config
            if (!this.ActivateInstallationLicenseFile(licenseId, password, ws, ref licenseContent))
                return false;

            return this.SaveLicenseFile(licenseContent);
        }

        /// <summary>Deactivates the installation online.</summary>
        /// <returns>Returns true if the system was deactivated successfully. If false is returned, evaluate the <see cref="License.LastError"/> property to determine the reason why deactivation failed.</returns>
        internal bool DeactivateOnline()
        {
            //initialize the object used for calling the web service method
            XmlActivationService ws = m_WebServiceHelper.CreateXmlActivationServiceObject();
            if (null == ws)
            {
                this.LastError = m_WebServiceHelper.LastError;
                return false;
            }

            if (this.DeactivateInstallation(ws) || this.LastError.ExtendedErrorNumber == 5010 || this.LastError.ExtendedErrorNumber == 5015 || this.LastError.ExtendedErrorNumber == 5016 || this.LastError.ExtendedErrorNumber == 5017)
            {
                File.Delete(LicenseConfiguration.LicenseFilePath);
                return true;
            }

            return false;
        }

        /// <summary>Refreshes the license file with the latest data from SOLO Server.</summary>
        /// <returns>Returns true if the license file was refreshed successfully. If false is returned, evaluate the <see cref="License.LastError"/> property to determine the reason why the refresh failed.</returns>
        internal bool RefreshLicense()
        {
            //initialize the object used for calling the web service method
            XmlLicenseFileService ws = m_WebServiceHelper.CreateXmlLicenseFileServiceObject();
            if (null == ws)
            {
                this.LastError = m_WebServiceHelper.LastError;
                return false;
            }

            string licenseContent = "";
            if (!base.RefreshLicense(ws, ref licenseContent))
            {
                if (this.LastError.ExtendedErrorNumber == 5010 || this.LastError.ExtendedErrorNumber == 5015 || this.LastError.ExtendedErrorNumber == 5016 || this.LastError.ExtendedErrorNumber == 5017)
                {
                    File.Delete(LicenseConfiguration.LicenseFilePath);
                    return true;
                }
                return false;
            }

            //try to save the license file to the file system
            if (!SaveLicenseFile(licenseContent))
                return false;

            return true;
        }

        /// <summary>Saves a new license file to the file system.</summary>
        /// <param name="lfContent">The new content to write to the license file.</param>
        /// <returns>Returns true if the license file was saved successfully. If false is returned, evaluate the <see cref="License.LastError"/> property to determine the reason why the file could not be saved.</returns>
        internal bool SaveLicenseFile(string lfContent)
        {
            //try to save the license file to the file system
            try
            {
                File.WriteAllText(LicenseConfiguration.LicenseFilePath, lfContent);
            }
            catch (Exception ex)
            {
                this.LastError = new LicenseError(LicenseError.ERROR_COULD_NOT_SAVE_LICENSE, ex);
                return false;
            }

            return true;
        }

        /// <summary>Validates the license.</summary>
        /// <returns>Returns true if the license is valid.  If the license is not valid, evaluate the <see cref="License.LastError"/> property to determine the reason why validation failed.</returns>
        internal bool Validate()
        {
            //Refresh the license if date difference is greater than m_RefreshLicenseAttemptFrequency (days).
            if (IsRefreshLicenseAttemptDue)
            {
                //If refresh license fails and date difference is greater than m_RefreshLicenseRequireFrequency (days) set license as invalid.
                if (!RefreshLicense() && IsRefreshLicenseRequired)
                {
                    return false;
                }
            }

            //Create a list of validations to perform.
            List<SystemValidation> validations = new List<SystemValidation>();

            //Add a validation to make sure there is no active system clock tampering taking place.
            validations.Add(new SystemClockValidation());

            //Validate the Product ID authorized in the license to make sure the license file was issued for this application.
            validations.Add(new LicenseProductValidation(this, ThisProductID));

            //Add a validation to make sure this system is authorized to use the activated license.  (This implements copy-protection.)
            validations.Add(new SystemIdentifierValidation(AuthorizedIdentifiers, CurrentIdentifiers, SystemIdentifierValidation.REQUIRE_EXACT_MATCH));

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
