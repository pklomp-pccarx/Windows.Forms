using System;
using System.Windows.Forms;
using com.softwarekey.Client.Licensing.Network;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Network license search progress dialog</summary>
    public partial class NetworkLicenseSearchForm : Form
    {
        #region Private Member Variables
        private NetworkSemaphore m_semaphore;
        #endregion

        #region Public Methods
        /// <summary>Network license search dialog event handler</summary>
        /// <param name="semaphore">semaphore</param>
        public NetworkLicenseSearchForm(NetworkSemaphore semaphore)
        {
            m_semaphore = semaphore;

            InitializeComponent();
        }
        #endregion

        #region Private Methods
        /// <summary>Network license search dialog event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void NetworkLicenseSearchDlg_Load(object sender, EventArgs e)
        {
            m_semaphore.SearchProgress += new NetworkSemaphore.SearchProgressEventHandler(SearchProgress);
            m_semaphore.SearchCompleted += new NetworkSemaphore.SearchCompletedEventHandler(SearchCompleted);
        
            m_semaphore.Search();
        }

        private delegate void SearchThreadCompletedDelegate(object sender, SearchCompletedEventArgs e);

        /// <summary>Search completed</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        void SearchCompleted(object sender, SearchCompletedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SearchThreadCompletedDelegate(SearchCompleted), new Object[] { sender, e }); // invoke this method using our UI thread delegate
            }
            else
            {
                if (e.SeatOpened)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }

        private delegate void SearchThreadProgressDelegate(object sender, SearchProgressEventArgs e);

        /// <summary>Search progress</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void SearchProgress(object sender, SearchProgressEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SearchThreadProgressDelegate(SearchProgress), new Object[] { sender, e }); // invoke this method using our UI thread delegate
            }
            else
            {
                progressBar.Visible = true;
                progressBar.Value = e.ProgressPercentage;
            }
        }

        /// <summary>Network license search dialog event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void NetworkLicenseSearchDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_semaphore.CancelSearch();
        }
        #endregion
    }
}
