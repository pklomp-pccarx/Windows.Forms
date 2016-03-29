using System;
using System.Collections.Generic;
using System.IO;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.Utils;
using com.softwarekey.Client.WebService.XmlActivationService;
using com.softwarekey.Client.WebService.XmlLicenseFileService;

namespace com.softwarekey.Client.Sample
{
    /// <summary>A sample <see cref="WritableLicense"/>
    /// <see href="http://www.softwarekey.com/help/plus5/#StartTopic=Content/PLUSManaged_License_Implementation.htm">implementation class</see>,
    /// which uses self-signed (writable) licenses.</summary>
    public class SampleSelfSignedLicense : WritableLicense, SampleLicense
    {
        #region Member Variables
        private static WebServiceHelper m_WebServiceHelper = new WebServiceHelper();
        #endregion

        #region Constructors
        /// <summary>Creates a new <see cref="SampleSelfSignedLicense"/> object.</summary>
        public SampleSelfSignedLicense()
            : base(LicenseConfiguration.EncryptionKey, true, true, LicenseConfiguration.ThisProductID, LicenseConfiguration.ThisProductVersion, LicenseConfiguration.SystemIdentifierAlgorithms)
        {
            foreach (LicenseAlias alias in LicenseConfiguration.Aliases)
                AddAlias(alias);
        }
        #endregion

        #region Public Properties
        /// <summary>Gets whether or not this is an evaluation license.</summary>
        public bool IsEvaluation
        {
            get { return true; }
        }
        #endregion

        #region Public Methods
        /// <summary>Activates the license online.</summary>
        /// <param name="licenseId">The License ID issued by SOLO Server.</param>
        /// <param name="password">The customer password.</param>
        /// <returns>Returns true if the system was activated successfully. If false is returned, evaluate the <see cref="License.LastError"/> property to determine the reason why activation failed.</returns>
        public bool ActivateOnline(Int32 licenseId, string password)
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

        /// <summary>Calculates a new EffectiveEndDate.</summary>
        /// <param name="duration">The number of days the license should last.</param>
        /// <param name="extendExisting">If true, the existing, non-expired license will be extended by the number of days specified in the duration argument.</param>
        /// <returns>The new EffectiveEndDate</returns>
        internal DateTime CalculateNewEffectiveEndDate(int duration, bool extendExisting)
        {
            if (duration > 0 && extendExisting)
            {
                int currentDaysLeft = (int)EffectiveEndDate.Subtract(DateTime.UtcNow.Date).TotalDays;
                if (currentDaysLeft > 0)
                    duration += currentDaysLeft;
            }

            return DateTime.UtcNow.Date.AddDays(duration);
        }

        /// <summary>Creates a fresh trial license and returns true if successful</summary>
        /// <returns>Returns true if the fresh evaluation license file was created successfully.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool CreateFreshEvaluation()
        {
            return CreateEvaluation(LicenseConfiguration.FreshEvaluationDuration, true, false);
        }

        /// <summary>Creates an expired evaluation license.</summary>
        /// <returns>Returns true if the expired evaluation license file was created successfully.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool CreateExpiredEvaluation()
        {
            return CreateEvaluation(-1, false, false);
        }

        /// <summary>Creates an evaluation for the specified duration.</summary>
        /// <param name="evaluationDuration">The duration (in days) of the evaluation.</param>
        /// <param name="shouldCheckAliases">Whether or not aliases should be checked before creating the new evaluation license.</param>
        /// <param name="extendExisting">Whether or not any existing trial period should be extended.</param>
        /// <returns>Returns true if successful.  If false is returned, check the <see cref="License.LastError"/> property for details.</returns>
        internal bool CreateEvaluation(int evaluationDuration, bool shouldCheckAliases, bool extendExisting)
        {
            if (shouldCheckAliases)
            {
                //Start by loading and checking all the aliases
                int numAliases, numValidAliases;
                this.CheckAliases(out numAliases, out numValidAliases);

                //If we found any aliases, write the most recent one as the license file
                LicenseAlias mostRecent = LicenseAlias.GetMostCurrentAlias(this.Aliases);
                if (mostRecent.LastUpdated != DateTime.MinValue)
                {
                    this.WriteAliasToLicenseFile(mostRecent, LicenseConfiguration.LicenseFilePath);
                    this.Load(mostRecent.Contents);
                    int aliasesToWrite, aliasesWritten;
                    this.WriteAliases(out aliasesToWrite, out aliasesWritten);
                    return true;
                }
            }

            //Set the Product ID so this evaluation license cannot be used to update or extend another application's evaluation period.
            Product.ProductID = ThisProductID;

            //Evaluations that are not established through activation should have no License ID, Installation ID, or Installation Name.
            LicenseID = 0;
            InstallationID = "";
            InstallationName = "";
            
            //In the SimpleTextEditor samples, this makes it so all features can be used during the evaluation period.
            UserDefinedNumber1 = Int32.MaxValue;

            if (evaluationDuration > 0)
            {
                //We are creating an evaluation that expires in the future, so set the effective start date to today's date.
                this.EffectiveStartDate = DateTime.UtcNow.Date;
            }
            else
            {
                //If we get into this code block, then we are creating an expired evaluation, and we should just make the start date the same as the end date.
                this.EffectiveStartDate = DateTime.UtcNow.Date.AddDays(evaluationDuration);
            }

            //Now set the evaluation's expiration date.
            this.EffectiveEndDate = CalculateNewEffectiveEndDate(evaluationDuration, extendExisting);

            //Write the aliases.
            int filesToWrite, filesWritten;
            this.WriteAliases(out filesToWrite, out filesWritten);

            //TODO: you can add your own logic here to set your own requirements for how many aliases must be written
            //      ...for this example, we only require 1
            if (filesWritten < 1)
            {
                return false;
            }

            //Write the new license file.
            return this.WriteLicenseFile(LicenseConfiguration.LicenseFilePath);
        }

        /// <summary>Since self-signed licenses are only used for un-activated, evaluation license in the "SelfSignedTrial" samples, deactivation is not supported for this license implementation.</summary>
        /// <returns>Always returns false.</returns>
        public bool DeactivateOnline()
        {
            LastError = new LicenseError(LicenseError.ERROR_INVALID_LICENSE_TYPE);
            return false;
        }

        /// <summary>Since self-signed licenses are only used for un-activated, evaluation license in the "SelfSignedTrial" samples, a refresh is not supported for this license implementation.</summary>
        /// <returns>Always returns false.</returns>
        public bool RefreshLicense()
        {
            LastError = new LicenseError(LicenseError.ERROR_INVALID_LICENSE_TYPE);
            return false;
        }

        /// <summary>Saves a new license file to the file system. (Used by  <see cref="ManualActivationForm"/>.)</summary>
        /// <param name="lfContent">The new content to write to the license file.</param>
        /// <returns>Returns true if the license file was saved successfully. If false is returned, evaluate the <see cref="License.LastError"/> property to determine the reason why the file could not be saved.</returns>
        public bool SaveLicenseFile(string lfContent)
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
        public bool Validate()
        {
            //Create a list of validations to perform.
            List<SystemValidation> validations = new List<SystemValidation>();

            //Add a validation to make sure there is no active system clock tampering taking place.
            validations.Add(new SystemClockValidation());

            //Validate the aliases.
            validations.Add(new LicenseAliasValidation(this));

            //Validate the Product ID authorized in the license to make sure the license file was issued for this application.
            validations.Add(new LicenseProductValidation(this, ThisProductID));

            //Add a validation to make sure this system is authorized to use the activated license.  (This implements copy-protection.)
            validations.Add(new SystemIdentifierValidation(AuthorizedIdentifiers, CurrentIdentifiers, SystemIdentifierValidation.REQUIRE_EXACT_MATCH));

            //If the license is time-limited or an evaluation, make sure it is within its effective date/time period.
            validations.Add(new LicenseEffectiveDateValidation(this));

            //Validate the system date and time using Network Time Protocol (un-comment the SystemDateTimeValidation below).
            //IMPORTANT: Read the documentation for more details about using NTP!  Do NOT use the NTP servers shown in the
            //           sample code immediately below this comment block.
            //--------------------------------------------------------------------------------------------------------------
            //SystemDateTimeValidation ntpValidation = new SystemDateTimeValidation();
            //ntpValidation.AddTimeServerCheck("time.windows.com");
            //ntpValidation.AddTimeServerCheck("time.nist.gov");
            //validations.Add(ntpValidation);

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
