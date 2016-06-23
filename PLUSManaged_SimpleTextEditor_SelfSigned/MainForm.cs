using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using com.softwarekey.Client.Licensing;

namespace com.softwarekey.Client.Sample
{
    /// <summary>MainForm form class implementation</summary>
    public partial class MainForm : Form
    {
        #region Private Member Variables
        private string m_DocumentName = "";
        private bool m_IsContentModified = false;
        private Features m_Features = new Features();
        private FindAndReplaceForm m_FindReplaceForm = null;
        private FindAndReplaceForm m_FindForm = null;
        private string[] m_PrintLines = { "" };
        private int m_LinesPrinted = 0;
        Encoding m_Encoder = Encoding.UTF8;
        private SampleLicense m_License = null;
        private string m_LicenseStatus = "";
        private bool m_IsLicenseValid = false;
        private string m_WarningMessage = "";
        #endregion

        #region Public Constructors
        /// <summary>Default constructor</summary>
        public MainForm()
        {
            this.Hide();
            Thread splashthread = new Thread(new ThreadStart(SplashScreen.ShowSplashScreen));
            splashthread.IsBackground = true;
            splashthread.Start();
            InitializeComponent();
        }
        #endregion

        #region Public Properties
        /// <summary>Gets the application's features.</summary>
        public Features AllFeatures
        {
            get { return m_Features; }
        }

        /// <summary>Gets whether or not the license is valid.</summary>
        public bool IsLicenseValid
        {
            get { return m_IsLicenseValid; }
        }

        /// <summary>Gets the application's license.</summary>
        public SampleLicense License
        {
            get { return m_License; }
        }

        /// <summary>Gets or sets the license status.</summary>
        public string LicenseStatus
        {
            get { return m_LicenseStatus; }
            set { m_LicenseStatus = value; }
        }

        /// <summary>Gets the customer's registration information.</summary>
        public string LicenseRegistrationInfo
        {
            get
            {
                StringBuilder registrationInfo = new StringBuilder();

                if (m_License.Customer.Unregistered ||
                    (!string.IsNullOrEmpty(m_License.Customer.FirstName) && m_License.Customer.FirstName.ToUpperInvariant() == "UNREGISTERED") ||
                    (!string.IsNullOrEmpty(m_License.Customer.LastName) && m_License.Customer.LastName.ToUpperInvariant() == "UNREGISTERED") ||
                    (!string.IsNullOrEmpty(m_License.Customer.CompanyName) && m_License.Customer.CompanyName.ToUpperInvariant() == "UNREGISTERED"))
                {
                    registrationInfo.Append("Unregistered");
                }
                else
                {
                    if (!string.IsNullOrEmpty(m_License.Customer.FirstName))
                        registrationInfo.Append(m_License.Customer.FirstName + " ");

                    if (!string.IsNullOrEmpty(m_License.Customer.LastName))
                        registrationInfo.Append(m_License.Customer.LastName + Environment.NewLine);

                    if (!string.IsNullOrEmpty(m_License.Customer.CompanyName))
                        registrationInfo.Append(m_License.Customer.CompanyName);
                }

                if (registrationInfo.Length == 0)
                    registrationInfo.Append("Unregistered");

                return registrationInfo.ToString();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>Save file method</summary>
        private bool SaveFile()
        {
            string strFileName = "";

            if (m_DocumentName.Length == 0)
            {
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    strFileName = saveFileDialog.FileName;
                }
                else
                {
                    return false;
                }
                m_DocumentName = strFileName;
            }

            //Save the document
            SaveDocument(m_DocumentName);
            m_IsContentModified = false;
            return true;
        }

        /// <summary>Save file method</summary>
        /// <param name="strFileName">string - file name</param>
        private void SaveDocument(string strFileName)
        {
            this.Text = "Simple Text Editor" + strFileName;
            EditorTextBox.Select(0, 0);

            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(strFileName, FileMode.Create, FileAccess.Write);
                Byte[] dataByte;

                dataByte = new Byte[EditorTextBox.Text.Length];
                dataByte.Initialize();
                dataByte = m_Encoder.GetBytes(EditorTextBox.Text.ToString());
                fileStream.Write(dataByte, 0, dataByte.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error :" + ex, "Simple Text Editor", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            this.Text = "Simple Text Editor" + strFileName;
            return;
        }
        #endregion

        #region Form Event Handler
        /// <summary>Form load implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void MainForm_Load(object sender, EventArgs e)
        {


            // this is Paul's test
            


            string msg = "Initializing Features";
            for (int i = 0; i < 6; i++)
            {
                Thread.Sleep(500);
                msg = msg + ".";
                SplashScreen.UdpateStatusText(msg);
            }

            bool successful = ReloadLicense();
            SplashScreen.UdpateStatusText("Features Loaded.......");
            Thread.Sleep(1000);
            this.Show();
            SplashScreen.CloseSplashScreen();
            ContextMenu blankContextMenu = new ContextMenu();
            EditorTextBox.ContextMenu = blankContextMenu;
            this.Activate();

            //Display the Evaluation Encryption Envelope warning message
            if (!string.IsNullOrEmpty(m_WarningMessage))
            {
                MessageBox.Show(this, m_WarningMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            //if License is not valid show the about dialog box
            if (!successful)
            {
                AboutForm aboutForm = null;
                try
                {
                    aboutForm = new AboutForm(this);
                    aboutForm.ShowDialog(this);
                }
                finally
                {
                    aboutForm.Dispose();
                }

                if (m_License.LastError.ErrorNumber == Licensing.LicenseError.ERROR_PLUS_EVALUATION_INVALID)
                {
                    mnuActivate.Enabled = false;
                    mnuOnline.Enabled = false;
                    mnuManual.Enabled = false;
                }
            }
        }

        /// <summary>Form closing implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">FormClosingEventArgs</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //If the document is not saved then prompt the user to save
            if (m_IsContentModified && (
                m_Features.CheckStatus(LicenseFeatures.Save.ToString())
                || m_Features.CheckStatus(LicenseFeatures.SaveAs.ToString())))
            {
                switch (MessageBox.Show(this, "The document is not saved. Do you wish to save now ?", "Simple Text Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                {
                    case DialogResult.Yes:
                        if (!SaveFile())
                            e.Cancel = true;
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }
        #endregion

        #region Menu Click Event Handler
        /// <summary>New menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuNew_Click(object sender, EventArgs e)
        {
            //If the document is not saved then prompt the user to save
            if (m_IsContentModified && (
                m_Features.CheckStatus(LicenseFeatures.Save.ToString())
                || m_Features.CheckStatus(LicenseFeatures.SaveAs.ToString())))
            {
                switch (MessageBox.Show(this, "The document is not saved. Do you wish to save now ?", "Simple Text Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                {
                    case DialogResult.Yes:
                        if (!SaveFile())
                            return;
                        EditorTextBox.Text = "";
                        this.Text = "Simple Text Editor";
                        break;
                    case DialogResult.No:
                        EditorTextBox.Text = "";
                        this.Text = "Simple Text Editor";
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            else
            {
                EditorTextBox.Text = "";
                this.Text = "Simple Text Editor";
            }
            m_DocumentName = "";
            saveFileDialog.FileName = "";
            m_IsContentModified = false;
        }

        /// <summary>Open menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            string strFileName = "";
            string strTemp = "";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                //If the document is not saved then prompt the user to save
                if (m_IsContentModified && (
                m_Features.CheckStatus(LicenseFeatures.Save.ToString())
                || m_Features.CheckStatus(LicenseFeatures.SaveAs.ToString())))
                {
                    switch (MessageBox.Show(this, "The document is not saved. Do you wish to save now ?", "Simple Text Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation))
                    {
                        case DialogResult.Yes:
                            if (!SaveFile())
                                return;
                            EditorTextBox.Text = "";
                            this.Text = "Simple Text Editor";
                            break;
                        case DialogResult.No:
                            EditorTextBox.Text = "";
                            this.Text = "Simple Text Editor";
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }
                strFileName = openFileDialog.FileName;
                m_DocumentName = strFileName;
            }
            else
            {
                return;
            }

            //Read the file
            FileStream fileStream = null;
            string strFileContents = "";
            try
            {
                strTemp = "";
                int iRetval = 1;

                fileStream = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
                Byte[] dataByte = new Byte[1024];
                dataByte.Initialize();
                for (; iRetval != 0; )
                {
                    dataByte.Initialize();
                    strTemp = "";
                    iRetval = fileStream.Read(dataByte, 0, 1024);
                    strTemp = m_Encoder.GetString(dataByte, 0, 1024);
                    strFileContents += strTemp;
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(this, "File not found or permission denied", "Simple Text Editor", MessageBoxButtons.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Error : " + ex.ToString(), "Simple Text Editor", MessageBoxButtons.OK);
                return;
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

            strTemp = strFileName.ToLower();
            EditorTextBox.Text = strFileContents;
            this.Text = "Simple Text Editor" + strFileName;
            EditorTextBox.Select(0, 0);
            m_IsContentModified = false;
        }

        /// <summary>About menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = null;
            try
            {
                aboutForm = new AboutForm(this);
                aboutForm.ShowDialog(this);
            }
            finally
            {
                aboutForm.Dispose();
            }
        }

        /// <summary>Save menu implementation</summary>
        /// <param name="sender">object</param>r
        /// <param name="e">EventArgs</param>
        private void mnuSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        /// <summary>Print menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuPrint_Click(object sender, EventArgs e)
        {
            // Allow the user to choose the page range he or she would
            // like to print.
            printDialog.AllowSomePages = true;

            // Show the help button.
            printDialog.ShowHelp = true;

            DialogResult result = printDialog.ShowDialog(this);

            // If the result is OK then print the document.
            if (result == DialogResult.OK)
            {
                docToPrint.Print();
            }
        }

        /// <summary>Save as menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            m_DocumentName = "";
            SaveFile();
        }

        /// <summary>Exit menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>Select all menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            EditorTextBox.SelectAll();
        }

        /// <summary>Cut menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuCut_Click(object sender, EventArgs e)
        {
            if (EditorTextBox.SelectedText != "")
            {
                EditorTextBox.Cut();
            }
        }

        /// <summary>Copy menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if (EditorTextBox.SelectedText != "")
            {
                EditorTextBox.Copy();
            }
        }

        /// <summary>Paste menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuPaste_Click(object sender, EventArgs e)
        {
            EditorTextBox.Paste();
        }

        /// <summary>Find menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuFind_Click(object sender, EventArgs e)
        {
            if (m_FindForm == null || m_FindForm.IsDisposed)
            {
                m_FindForm = new FindAndReplaceForm(true);
                m_FindForm.Owner = this;
                m_FindForm.Show();
                if (m_FindReplaceForm != null)
                {
                    m_FindReplaceForm.Dispose();
                    m_FindReplaceForm.Close();
                }
            }
            else
            {
                m_FindForm.Focus();
            }
        }

        /// <summary>Replace menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuReplace_Click(object sender, EventArgs e)
        {
            if (m_FindReplaceForm == null || m_FindReplaceForm.IsDisposed)
            {
                m_FindReplaceForm = new FindAndReplaceForm(false);
                m_FindReplaceForm.Owner = this;
                m_FindReplaceForm.Show();
                if (m_FindForm != null)
                {
                    m_FindForm.Dispose();
                    m_FindForm.Close();
                }
            }
            else
            {
                m_FindReplaceForm.Focus();
            }
        }

        /// <summary>Online menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuOnline_Click(object sender, EventArgs e)
        {
            using (OnlineActivationForm activationDialog = new OnlineActivationForm(this, m_License))
            {
                activationDialog.ShowDialog(this);
            }
        }

        /// <summary>Manual menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuManual_Click(object sender, EventArgs e)
        {
            using (ManualActivationForm activationDialog = new ManualActivationForm(this, m_License))
            {
                activationDialog.ShowDialog(this);
            }
        }

        /// <summary>Refresh License menu implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void mnuRefreshLicense_Click(object sender, EventArgs e)
        {
            if (m_License.RefreshLicense())
            {
                MessageBox.Show(this, "The license has been refreshed successfully.", "License Refresh", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "The license was not refreshed.  Error: (" + m_License.LastError.ErrorNumber + ")" + m_License.LastError.ErrorString, "License Refresh", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            ReloadLicense();
        }
        #endregion

        #region Tool Strip Button Click Event Handler
        /// <summary>New tool strip button implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            mnuNew_Click(sender, e);
        }

        /// <summary>Open tool strip button implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            mnuOpen_Click(sender, e);
        }

        /// <summary>Save tool strip button implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            mnuSave_Click(sender, e);
        }

        /// <summary>Cut tool strip button implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            mnuCut_Click(sender, e);
        }

        /// <summary>Copy tool strip button implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            mnuCopy_Click(sender, e);
        }

        /// <summary>Paste tool strip button implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            mnuPaste_Click(sender, e);
        }

        /// <summary>Print tool strip button implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void printToolStripButton_Click(object sender, EventArgs e)
        {
            mnuPrint_Click(sender, e);
        }

        /// <summary>Find tool strip button implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void findToolStripButton_Click(object sender, EventArgs e)
        {
            mnuFind_Click(sender, e);
        }
        #endregion

        #region Textbox Text Changed Event Handler
        /// <summary>Editor textbox text changed implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void EditorTextBox_TextChanged(object sender, EventArgs e)
        {
            m_IsContentModified = true;
        }
        #endregion

        #region Print Document Event Handler
        /// <summary>Print document's BeginPrint implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">PrintEventArgs</param>
        private void docToPrint_BeginPrint(object sender, PrintEventArgs e)
        {
            char[] param = { '\n' };

            if (printDialog.PrinterSettings.PrintRange == PrintRange.Selection)
            {
                m_PrintLines = EditorTextBox.SelectedText.Split(param);
            }
            else
            {
                m_PrintLines = EditorTextBox.Text.Split(param);
            }

            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in m_PrintLines)
            {
                m_PrintLines[i++] = s.TrimEnd(trimParam);
            }
        }

        /// <summary>Print document's PrintPage implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">PrintEventArgs</param>
        private void docToPrint_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;
            Brush brush = null;
            try
            {
                brush = new SolidBrush(EditorTextBox.ForeColor);
                while (m_LinesPrinted < m_PrintLines.Length)
                {
                    e.Graphics.DrawString(m_PrintLines[m_LinesPrinted++], EditorTextBox.Font, brush, x, y);
                    y += 15;
                    if (y >= e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }
                }
            }
            finally
            {
                brush.Dispose();
            }

            m_LinesPrinted = 0;
            e.HasMorePages = false;
        }
        #endregion

        #region Override Methods
        /// <summary>Process comand key</summary>
        /// <param name="msg">Message</param>
        /// <param name="keyData">Keys</param>
        /// <returns>bool</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Keys CopyKeys = Keys.Control | Keys.C;
            Keys PasteKeys = Keys.Control | Keys.V;
            Keys CutKeys = Keys.Control | Keys.X;
            Keys ReplaceKeys = Keys.Control | Keys.H;

            if ((keyData == CopyKeys && !m_Features.CheckStatus(LicenseFeatures.Copy.ToString()))
                || (keyData == PasteKeys && !m_Features.CheckStatus(LicenseFeatures.Paste.ToString()))
                || (keyData == CutKeys && !m_Features.CheckStatus(LicenseFeatures.Cut.ToString()))
                || (keyData == ReplaceKeys && !m_Features.CheckStatus(LicenseFeatures.Replace.ToString())))
            {
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Public Methods
        /// <summary>Updates the LicenseStatus property.</summary>
        public void UpdateLicenseStatusProperty()
        {
            StringBuilder status = new StringBuilder();

            switch (m_License.LastError.ErrorNumber)
            {
                case LicenseError.ERROR_COULD_NOT_LOAD_LICENSE:
                    status.Append(m_License.LastError.ErrorNumber);
                    status.Append(": ");
                    status.Append("License not found - activation is required.");
                    break;
                case LicenseError.ERROR_LICENSE_NOT_EFFECTIVE_YET:
                    status.Append(m_License.LastError.ErrorNumber);
                    status.Append(": ");
                    status.Append("License not effective until ");
                    DateTime local = m_License.EffectiveStartDate.ToLocalTime();

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
                    status.Append(m_License.LastError.ErrorNumber);
                    status.Append(": ");
                    status.Append("License invalid or expired.");
                    break;
                case LicenseError.ERROR_WEBSERVICE_RETURNED_FAILURE:
                    //Web service error message.
                    status.Append(m_License.LastError.ExtendedErrorNumber);
                    status.Append(": ");
                    status.Append(LicenseError.GetWebServiceErrorMessage(m_License.LastError.ExtendedErrorNumber));
                    break;
                default:
                    //Show a standard error message.
                    status.Append(m_License.LastError.ErrorNumber);
                    status.Append(": ");
                    status.Append(m_License.LastError.ToString());
                    break;
            }

            LicenseStatus = status.ToString();
        }

        /// <summary>Enables or disables features based on the status of the license.</summary>
        public void ToggleFeatures()
        {
            this.mnuNew.Enabled = newToolStripButton.Enabled = m_Features.CheckStatus(LicenseFeatures.New.ToString());
            this.mnuOpen.Enabled = openToolStripButton.Enabled = m_Features.CheckStatus(LicenseFeatures.Open.ToString());
            this.mnuPrint.Enabled = printToolStripButton.Enabled = m_Features.CheckStatus(LicenseFeatures.Print.ToString());
            this.mnuFind.Enabled = findToolStripButton.Enabled = m_Features.CheckStatus(LicenseFeatures.Find.ToString());
            this.mnuSave.Enabled = saveToolStripButton.Enabled = m_Features.CheckStatus(LicenseFeatures.Save.ToString());
            this.mnuSelectAll.Enabled = m_Features.CheckStatus(LicenseFeatures.SelectAll.ToString());
            this.mnuCut.Enabled = cutToolStripButton.Enabled = m_Features.CheckStatus(LicenseFeatures.Cut.ToString());
            this.mnuCopy.Enabled = copyToolStripButton.Enabled = m_Features.CheckStatus(LicenseFeatures.Copy.ToString());
            this.mnuPaste.Enabled = pasteToolStripButton.Enabled = m_Features.CheckStatus(LicenseFeatures.Paste.ToString());
            this.mnuSaveAs.Enabled = m_Features.CheckStatus(LicenseFeatures.SaveAs.ToString());
            this.mnuReplace.Enabled = m_Features.CheckStatus(LicenseFeatures.Replace.ToString());
        }

        /// <summary>Reloads the license file and refreshes the status on the main form.</summary>
        /// <returns>bool</returns>
        public bool ReloadLicense()
        {
            m_License = new SampleLicense();

            //Get the Evaluation Encryption envelope warning message.
            if (m_License.LastError.ErrorNumber == LicenseError.ERROR_PLUS_EVALUATION_WARNING)
            {
                m_WarningMessage = "Warning: (" + m_License.LastError.ErrorNumber + ") " + m_License.LastError.ErrorString;
            }

            bool successful = m_License.LoadFile(LicenseConfiguration.LicenseFilePath);
            if (!successful)
            {
                successful = m_License.CreateFreshEvaluation();
                if (successful)
                {
                    successful = m_License.LoadFile(LicenseConfiguration.LicenseFilePath);
                }
            }

            if (!successful)
            {
                mnuRefreshLicense.Enabled = false;
                UpdateLicenseStatusProperty();
                SplashScreen.InitializeFeatures(m_Features, m_License, false);
                ToggleFeatures();
                return false;
            }

            return RefreshLicenseStatus();
        }

        /// <summary>Refreshes the license status on the main form</summary>
        /// <returns>bool</returns>
        public bool RefreshLicenseStatus()
        {
            if (!m_License.IsEvaluation)
            {
                mnuRefreshLicense.Enabled = true;
            }
            else
            {
                mnuRefreshLicense.Enabled = false;
            }

            m_IsLicenseValid = m_License.Validate();

            //If license is trial and valid license, initializes trial features
            if (m_License.IsEvaluation && m_IsLicenseValid)
            {
                SplashScreen.InitializeTrialFeatures(m_Features, m_License);
            }
            else
            {
                SplashScreen.InitializeFeatures(m_Features, m_License, m_IsLicenseValid);
            }
            ToggleFeatures();

            if (!m_IsLicenseValid)
            {
                mnuRefreshLicense.Enabled = false;
                UpdateLicenseStatusProperty();
                return false;
            }

            if (m_License.IsEvaluation)
            {
                mnuRefreshLicense.Enabled = false;
                TimeSpan ts = m_License.EffectiveEndDate.ToLocalTime() - DateTime.Now.Date;
                LicenseStatus = "Evaluation expires in " + Math.Round(ts.TotalDays, 0).ToString() + " days.";
                return false;
            }
            else
            {
                mnuRefreshLicense.Enabled = true;
                LicenseStatus = "Fully Licensed.";
            }

            return true;
        }
        #endregion
    }
}