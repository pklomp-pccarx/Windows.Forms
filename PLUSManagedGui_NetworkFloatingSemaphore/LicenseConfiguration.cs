using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using com.softwarekey.Client.Licensing;
using com.softwarekey.Client.Utils;
using Microsoft.Win32;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Contains data used for PLUSManaged samples.</summary>
    /// <remarks><note type="caution"><para>If you copy this code into your application, it is VERY IMPORTANT that you UPDATE THE CONFIGURATION PROPERTIES BELOW!!!</para></note></remarks>
    internal static partial class LicenseConfiguration
    {
        // TODO: update this constant to a registry location of your choosing (located under HKEY_CURRENT_USER).
        #region Private Constant Variables
        private const string PATH_REGISTRY_LOCATION = "Software\\Concept Software\\Protection PLUS\\Samples\\NetworkFloatingSemaphore";
        #endregion

        #region Private Static Variables
        private static string _LicenseFilePath = "";
        #endregion

        //TODO: IMPORTANT: If you copy this code into your application, update the configuration settings in the regions below!!!
        #region Encryption Settings
        /// <summary>Gets the encryption key data used to read the license and communicate with SOLO Server.</summary>
        /// <remarks><note type="caution"><para>TODO: IMPORTANT: Update this to use your SOLO Server account's Encryption Key data.
        /// If you only use License Manager (without SOLO Server), contact us (http://www.softwarekey.com/contact/) for an envelope.
        /// NEVER COPY THE ENVELOPE DIRECTLY FROM LICENSE MANAGER, as this contains key data that should never be known by your applications.</para></note></remarks>
        internal static AuthorEncryptionKey EncryptionKey
        {
            get
            {
                return new AuthorEncryptionKey("3VgAbahygs9XgkqNKtmqd88ntjjaBR0yEGEpZMcS1ZgGPmpHS5UJAmFCm3D/xc+H", // The string before this comment is the Envelope Key
                    "YN3+WBuQkoRpPaXb/QNFvX1SlsfXSNVGy1E49nxWMgWzDT9ZYCcKIGXUn+YEc/reoNp2eqnHxSI6vBpwGjBJ1lbKdZU" +
                    "Qn+AS2V3kXtgizUYJqKx6q0CRPA+TjhWdQwek8W0EyfLOfHJh3rVtJEv6vIXq2DM2h7phwHfezGqo6nzwYxEnBRIyOl" +
                    "QhY4RtXBXrl3qkk1IyJYEU76Fe3+FE+D6s/GbvqucMsRG/yHTyU+L2UmtTTJqoEKBFipTw+MiGErtf+GLZAhA3+XhS6" +
                    "uxkukkSwsrlvQzgO+TmUxk+3tIDA8t7vbvYT7wGX541QzsQ6QWl5HeGl1h73z9W3vGlVHoM55MJMlKh2r29caQFVHyp" +
                    "AITwoieGJJfVCCG+NkGa6QGiFRMGRlDTAAQHwbL+wZx6Qu+ToyN3aEz9XxnGuJBtsV3cYFUq+AfehyFmbJGrDSCovIN" +
                    "ARuJZMvuEnvVd3vLxVeCyCdtMg//3027VJ5zQXgz5AHj0exmpfsTVgcbsFUyrQF1CvTqNn99lN/VjLs6GsgfNiKkBRa" +
                    "Vpr+OUA/PmPO3KJ/xRUE6XLZ9bnhJUK5tav8kYgc4A9cB3Z5bf4R/uSNiFjpnyaTZCKBESB9IKqhYCwO2Dx1RVYEGiE" +
                    "dc/w5eyy4rdl8mS6XbW4qWYhYi//8M9ZdAVZwGR4ADEwYtuSjKTIIzbHcYDxbFbkCOkDVYYTHdiyWHxPGWQjVe3+Zn1" +
                    "DCTPe2kI5JRhWbi7gu9LryBYbY5sE6eyt7iZ7BFSMA6fCYTYJQYv6/2SCFU3Wc7YmtHeNaeey/NfFkFNblhwmeR6VIM" +
                    "BM9c6XiM2dwigY6ZC17x8pUo120DIG+cpEZVhgOo/3vy8iy6pNq53ASlZoyPfnbx6edMy9BmVRc5Mm5hQG0MQmFVlfa" +
                    "eeSkb1uuanNckvopgGGcfJPyXVs0AUez7Hq+a6q9dZ4faKFXKyeoyV/y0E3zdLTaczweMHbWa0kB90i11+eNMDX9AJ+" +
                    "NKcVNWmNaAtCWVSJ4sEatKoOxJZSYOktjAq8FS293fxXa1vnPqrenYT9Jofqm/IRy1EmGTYAiAzLi5aL9gA9joHqLcE" +
                    "v0BySHLKz4yJqD88rZLrMtyvxu5WJ5HGEecOLyTHyIaBekgTfA7iNcWxdWK+t9zrpIp5ZaokQRGY8AH9em49NqQIajv" +
                    "LfjxW08WTjbmIYtQ2ldZeE1vTgC1OOgXZJnD5IpweEbBrULwOOUzlX6xqGHpQ7XXe4xE6LAXovJEFxanZcbgZIMA/bE" +
                    "EL7mtEgljvsWmxHiFnKt6ZXhEq7PAWdDaAwhuMwbjQi2C5IUcpHuOiGBObPIsmuxjrZsKtOlV4ssXFh1VJ+V37WZ2Y2" +
                    "mQWFSRioKMJZEoZRuGJg3hkvsusqART8FWgAXWNBhTUiG1j7tsxrhS2pmwTwkbsBHZRzIBb8ttOInblOA/e2bUmuefq" +
                    "gqfIR5SkElEMZFCovWf5ww/LLBTP2XrrNVq2+UfBCHmup9+e0pmrzOXXPpKQDJemHzBZdx5P3Nz1BHVIppP4YGl66Ds" +
                    "+7s5cBc000qDp30jODrBeJBr7rzl1iTUiKXI6QvZ55+bUu36EPNVHQHrbdE1fkWGkWUr55X1NcG/6gGUrWhM0u0vKyU" +
                    "udIURuKq4s+S08NMK5FzXBIX2Tx/QRT8WV1MLeNewDdeoLb60QhOC0RAE4JoIKF4eMKbgpvikCDT5vsuw9WTy3k6tnI" +
                    "Xhq7OKZUiMBsLGyV1wy5WoPGIGRtySR+D7wUJcNSfLJWGz5IEwuKE1x3gDFoOc8dqjby4fk8OhJ5OrzK98OBpIKHPwM" +
                    "gFwir392nqBV+Fn1YpLPnVFgtgqRgq2vfU3Ukg/NvFEuZT7rYzR2dtIKcBXv7VNSTHjohnXzsD0gDrb4Pe9NALFcWJD" +
                    "W9KIfuMtGKwMLfg/Lke1J9JXiwAU/ZcA3fZ3U6yUOVnTvU/TTSr8hk/hm5D8Yag2WJtHSN22jQl5fV3nHRCUDjYmHrI" +
                    "kS1SwT5JC2JkEfYHg595CLtfIblhu3s1880H0j6LYL3JfQK3oGc8O1XsWAPVDbvmbV3Gmccfae/mmFX5I6vlRMt12Fd" +
                    "ehGDqtXqGzM+B46YUuHdmpIK2z1yg5717uZB95URQ==", //The string before this comment is the Envelope.
                    false);
                //IMPORTANT: Passing false in the last argument above will cause PLUSManaged to use the User Key Store
                //           for encryption.  This is recommended for most desktop or Dialog/Form applications.  If
                //           you are protecting a web application, or an application that runs as a service, you
                //           may need to pass true for this argument to use the Machine Key Store.  Note that using
                //           the Machine Key Store typically requires permissions that may not be sufficient with
                //           the typical/default user configuration (which includes application pool identities and
                //           impersonated identities).
            }
        }

        /// <summary>Gets the initialization vector used when encrypting and decrypting manual action session-states.</summary>
        /// <remarks><note type="caution"><para>TODO: IMPORTANT: Create a new, random, unique, initialization vector for each of your applications!</para></note>
        /// <example>This example shows how to generate a new initialization vector (IV) and key for manual requests:
        /// <code language="cs">
        /// using (System.Security.Cryptography.RijndaelManaged alg = new System.Security.Cryptography.RijndaelManaged())
        /// {
        ///     alg.KeySize = 256;
        ///     alg.GenerateIV();
        ///     alg.GenerateKey();
        /// 
        ///     string iv = Convert.ToBase64String(alg.IV);
        ///     string key = Convert.ToBase64String(alg.Key);
        ///     //TODO: Save your IV and key.  You can just put a breakpoint on the next line and inspect the values of iv and key while debugging.
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        internal static string ManualActionIV
        {
            get { return "IZWGSERAx1h9zzpCGBmRXg=="; }
        }

        /// <summary>Gets the encryption key used when encrypting and decrypting manual action session-states.</summary>
        /// <remarks><note type="caution"><para>TODO: IMPORTANT: Create a new, random, unique, encryption key for each of your applications!</para></note></remarks>
        internal static string ManualActionKey
        {
            get { return "ajOsfy2RCC66qD5D9bCiJIK5zJVYUbJwLXwwC6fW4SI="; }
        }

        /// <summary>Gets the seed value used to decode \"Activation Code 2\" values when activating manually.  The decoded value will contain up to 14 bits of additional numeric data.</summary>
        /// <remarks><para>TODO: IMPORTANT: Each application should have its own, unique value for RegKey2Seed.  This MUST be a value between 1 and 255.</para></remarks>
        internal static Int32 RegKey2Seed
        {
            get { return 123; }
        }

        /// <summary>Gets the seed value used for validating \"Activation Code 1\" values when activating manually.</summary>
        /// <remarks><para>TODO: IMPORTANT: Each application should have its own, unique value for TriggerCodeSeed.  This MUST be a value between 1 and 65535.</para></remarks>
        internal static Int32 TriggerCodeSeed
        {
            get { return 400; }
        }
        #endregion

        #region Application and Product Settings
        /// <summary>Gets the absolute path to the directory in which the application (executable file) is located.</summary>
        /// <remarks><note type="caution"><para>TODO: IMPORTANT: You may need to update this code for your application depending on where it needs to store the license file.</para></note></remarks>
        internal static string ApplicationDirectory
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        /// <summary>Gets this application's Product ID (typically determined by SOLO Server).  Make your own unique value if you are using License Manager only.</summary>
        /// <remarks><note type="caution"><para>TODO: IMPORTANT: Update this to use your Product ID</para></note></remarks>
        internal static int ThisProductID
        {
            get { return 212488; }
        }

        /// <summary>Gets this application's product version number.</summary>
        /// <remarks>
        /// <note type="caution">
        /// <para>
        /// TODO: IMPORTANT: You may need to update this code for your application, especially if it is not a ".exe" file.
        /// You may also hard-code this data.
        /// </para>
        /// </note>
        /// </remarks>
        internal static string ThisProductVersion
        {
            get { return IOHelper.GetAssemblyFileVersion(Assembly.GetExecutingAssembly()); }
        }
        #endregion

        #region License File and Alias Settings
        /// <summary>Gets a list of aliases used with any samples that use writable license files.</summary>
        /// <remarks>
        /// <note type="caution"><para>TODO: IMPORTANT: You need to update this code so the alias locations are unique to your application!!!  Failing to do so could cause conflicts
        /// with the samples and other applications.</para></note>
        /// </remarks>
        internal static List<LicenseAlias> Aliases
        {
            get
            {
                return new List<LicenseAlias>(
                    new LicenseAlias[] {
                        new LicenseFileSystemAlias(Path.Combine(ApplicationDirectory, "LicenseAlias1.lfx"), EncryptionKey, true),
                        new LicenseFileSystemAlias(Path.Combine(ApplicationDirectory, "LicenseAlias2.lfx"), EncryptionKey, true)/*,
                        new LicenseWindowsRegistryAlias("Software\\Concept Software\\Protection PLUS\\Samples\\License", EncryptionKey, true, RegistryHive.CurrentUser, "LicenseAlias3"),
                        new LicenseWindowsRegistryAlias("Software\\Concept Software\\Protection PLUS\\Samples\\License", EncryptionKey, true, RegistryHive.CurrentUser, "LicenseAlias4")*/ });
            }
        }

        /// <summary>Gets or sets the absolute path to the application's license file.</summary>
        /// <remarks><note type="caution"><para>TODO: IMPORTANT: You may need to update this code for your application depending on where it needs to store the license file.</para></note></remarks>
        internal static string LicenseFilePath
        {
            set { _LicenseFilePath = Path.Combine(value, "LicenseFile.lfx"); }
            get
            {
                if(string.IsNullOrEmpty(_LicenseFilePath))
                    return Path.Combine(ApplicationDirectory, "LicenseFile.lfx");
                return _LicenseFilePath;
            }
        }

        /// <summary>Gets the absolute path to the manual action session state file.</summary>
        internal static string ManualActionSessionStateFilePath
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location) + "ManualAction.xml");
            }
        }

        /// <summary>Gets or Sets/Creates the File Path in the Registry Path specified.</summary>
        internal static string PathRegistryValue
        {
            get
            {
                RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(PATH_REGISTRY_LOCATION);
                return (string)pathKey.GetValue("LicensePath");
            }
            set
            {
                RegistryKey pathKey = Registry.CurrentUser.OpenSubKey(PATH_REGISTRY_LOCATION, true);

                if (pathKey == null)
                {
                    pathKey = Registry.CurrentUser.CreateSubKey(PATH_REGISTRY_LOCATION);
                }

                pathKey.SetValue("LicensePath", value);
            }
        }

        /// <summary>Gets a string name to use as the file name for the Network Semaphore Files</summary>
        internal static string NetworkSemaphorePrefix
        {
            get { return "sema"; }
        }
        #endregion

        #region Licensing Restrictions and Settings
        /// <summary>Gets the number of days to allow unlicensed users to evaluate the product.</summary>
        /// <remarks>
        /// <para>
        /// This setting is only applicable for samples which use a writable license, as it is not possible
        /// for an application to issue an evaluation license automatically using read-only licenses.
        /// </para>
        /// <note type="implementnotes"><para>TODO: Set this value per your licensing requirements!</para></note>
        /// </remarks>
        internal static int FreshEvaluationDuration
        {
            get { return 30; }
        }

        /// <summary>Gets whether or not a license file refresh will be required every time the application runs.</summary>
        /// <remarks><note type="implementnotes"><para>TODO: Set this value per your licensing requirements!</para></note></remarks>
        internal static bool RefreshLicenseAlwaysRequired
        {
            get { return false; }
        }

        /// <summary>Gets the number of days to wait before attempts to refresh and validate the license against SOLO Server begin. (Only applicable if <see cref="RefreshLicenseAlwaysRequired"/> is false.)</summary>
        /// <remarks><note type="implementnotes"><para>TODO: Set this value per your licensing requirements!</para></note></remarks>
        /// <note type="implementnotes"><para>Setting this to zero when <see cref="RefreshLicenseAlwaysRequired"/> is false means refreshing
        /// successfully is never attempted unless it is eventually required in the <see cref="RefreshLicenseRequireFrequency"/> property.</para></note>
        internal static int RefreshLicenseAttemptFrequency
        {
            get { return 7; }
        }

        /// <summary>Gets whether or not license refreshing is enabled at all.</summary>
        internal static bool RefreshLicenseEnabled
        {
            get { return (RefreshLicenseAlwaysRequired || RefreshLicenseAttemptFrequency != 0 || RefreshLicenseRequireFrequency != 0); }
        }

        /// <summary>Gets the number of days to wait before attempts to refresh and validate the license against SOLO Server are required.  (Only applicable when <see cref="RefreshLicenseAlwaysRequired"/> is false.)</summary>
        /// <remarks>
        /// <note type="implementnotes"><para>TODO: Set this value per your licensing requirements!</para></note>
        /// <note type="implementnotes"><para>If <see cref="RefreshLicenseAttemptFrequency"/> is used, then this property's value should be larger.</para></note>
        /// <note type="implementnotes"><para>Setting this to zero when <see cref="RefreshLicenseAlwaysRequired"/> is false means refreshing
        /// successfully is never required (though it may still be attempted depending on <see cref="RefreshLicenseAttemptFrequency"/>).</para></note>
        /// <para>An example of how this can be used would be to set <see cref="RefreshLicenseAttemptFrequency"/> to 7 days to start trying
        /// to phone-home after a week, while also setting <see cref="RefreshLicenseRequireFrequency"/> to 14 to require those attempts to
        /// succeed after two weeks.</para>
        /// </remarks>
        internal static int RefreshLicenseRequireFrequency
        {
            get { return 0; }
        }

        /// <summary>The amount of time (in seconds) to allow the system clock to be back-dated during run-time.</summary>
        /// <remarks>
        /// <para>This is used by SampleLicense.IsClockBackdatedAtRuntime to avoid raising "false alarms" when a system synchronizes its
        /// clock with Internet time (NTP).</para>
        /// <para>TODO: If you find your users run into issues where the back-date check is tripped after NTP synchronization occurs, you may relax the value here
        /// further.  The default value provided with the samples (300 seconds, or 5 minutes) is based on feedback.</para>
        /// </remarks>
        internal static int RuntimeBackdateThresholdSeconds
        {
            get { return 300; }
        }

        /// <summary>Gets the SystemIdentifierAlgorithms to use in the licensed application.</summary>
        /// <remarks><note type="implementnotes">TODO: Change or adjust the SystemIdentifierAlgorithm implementations you want to use to authorize licenses for a system.</note></remarks>
        internal static List<SystemIdentifierAlgorithm> SystemIdentifierAlgorithms
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    //Algorithms for Windows:
                    return new List<SystemIdentifierAlgorithm>(
                        new SystemIdentifierAlgorithm[] {
                            new NicIdentifierAlgorithm(),
                            new HardDiskVolumeSerialIdentifierAlgorithm(HardDiskVolumeSerialFilterType.OperatingSystemRootVolume),
                            new ComputerNameIdentifierAlgorithm(),
                            new BiosUuidIdentifierAlgorithm(),
                            new ProcessorIdentifierAlgorithm(new ProcessorIdentifierAlgorithmTypes[] { ProcessorIdentifierAlgorithmTypes.ProcessorName, ProcessorIdentifierAlgorithmTypes.ProcessorVendor, ProcessorIdentifierAlgorithmTypes.ProcessorVersion }) });
                }

                //Algorithms for other platforms:
                return new List<SystemIdentifierAlgorithm>(
                    new SystemIdentifierAlgorithm[] {
                        new HardDiskVolumeSerialIdentifierAlgorithm(HardDiskVolumeSerialFilterType.OperatingSystemRootVolume),
                        new NicIdentifierAlgorithm(),
                        new ComputerNameIdentifierAlgorithm() });
            }
        }

        /// <summary>Gets the number of days left on a time-limited license when the user should be warned about the limited amount of time left.</summary>
        /// <remarks><para>If this is set to 7, for example, this the user will be warned that the time-limited license will expire soon during the last week (7 days) in which the license is valid.</para></remarks>
        internal static int TimeLimitedWarningDays
        {
            get { return 7; }
        }
        #endregion

        #region Volume and Downloadable License File Settings
        //* Volume licenses are comprised of a read-only license file, which contains data necessary to uniquely identify a license. However, these licenses do not contain any data that
        //  uniquely identifies a licensed system. The benefit is that your customers can freely copy and use the volume license file to use your application without the need to activate.
        //* Downloadable licenses are very similar to volume licenses, in that they are read-only license files that only contain data capable of uniquely identifying the license. The
        //  difference, however, is that these licenses require trigger code validation to activate a separate, writable license file on each system on which the application is run.

        /// <summary>Gets whether or not application users are allowed to download an updated license file from the customer service portal and manually overwrite the license file used by the application.</summary>
        /// <remarks><note type="caution"><para>Enabling this feature can cause data previously stored in the writable license file copy to get overwritten.  You should evaluate the
        /// LicenseConfiguration.InitializeVolumeLicense method and update it as-needed to prevent this from being a problem for your application.</para></note></remarks>
        internal static bool DownloadableLicenseOverwriteWithNewerAllowed
        {
            get { return true; }
        }

        /// <summary>Gets whether or not application users can restore a previously download license file and restore it over the volume license file used by the application.</summary>
        /// <remarks><note type="caution"><para>Enabling this feature can cause data previously stored in the writable license file copy to get overwritten.  You should evaluate the
        /// LicenseConfiguration.InitializeVolumeLicense method and update it as-needed to prevent this from being a problem for your application.</para></note></remarks>
        internal static bool DownloadableLicenseOverwriteWithOlderAllowed
        {
            get { return true; }
        }

        /// <summary>Gets whether or not overwriting a downloaded license file with an older version of the file will require activation. (Only applicable when <see cref="DownloadableLicenseOverwriteWithNewerAllowed"/> is true.)</summary>
        internal static bool DownloadableLicenseOverwriteWithNewerRequiresActivation
        {
            get { return false; }
        }

        /// <summary>Gets whether or not overwriting a downloaded license file with an older version of the file will require activation. (Only applicable when <see cref="DownloadableLicenseOverwriteWithOlderAllowed"/> is true.)</summary>
        internal static bool DownloadableLicenseOverwriteWithOlderRequiresActivation
        {
            get { return true; }
        }

        /// <summary>Gets the absolute path to the application's downloadable/volume license file.</summary>
        /// <remarks><note type="caution"><para>TODO: IMPORTANT: You may need to update this code for your application depending on where it needs to store the license file.</para></note></remarks>
        internal static string VolumeLicenseFilePath
        {
            get
            {
                //TODO: Update your downloadable/volume license file name or path if necessary.
                return Path.Combine(ApplicationDirectory, "VolumeLicenseFile.lfx");
            }
        }

        /// <summary>Gets the <see cref="SystemIdentifierAlgorithm"/> objects to use when validating a downloadable/volume license.</summary>
        internal static List<SystemIdentifierAlgorithm> VolumeSystemIdentifierAlgorithms
        {
            get
            {
                return new List<SystemIdentifierAlgorithm>(
                    new SystemIdentifierAlgorithm[] {
                        new LicenseIDIdentifierAlgorithm() });
            }
        }
        #endregion
    }
}
