using System;
using System.Windows.Forms;

namespace com.softwarekey.Client.Sample
{
    /// <summary>Countdown dialog shown when the application will be forced to close due to an invalid network session.</summary>
    public partial class LicenseInvalidCountdownForm : Form
    {
        #region Private Member Variables
        private int m_timeLeft;
        #endregion

        #region Public Constructors
        /// <summary>License invalid countdown dialog</summary>
        /// <param name="timeTillExit">Time till exit</param>
        public LicenseInvalidCountdownForm(int timeTillExit)
        {
            InitializeComponent();

            m_timeLeft = timeTillExit;
            timeLeftLabel.Text = m_timeLeft.ToString();
        }
        #endregion

        #region Private Methods
        /// <summary>Network seat invalid countdown dialog event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void NetworkSeatInvalidCountdownDialog_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        /// <summary>Timer tick event handler</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (m_timeLeft > 1)
            {
                m_timeLeft--;
                SetLabelText(m_timeLeft.ToString());
            }
            else
                Application.Exit();
        }

        private delegate void SetControlText(string text);

        /// <summary>Set label text</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void SetLabelText(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetControlText(SetLabelText), new Object[] { text });
            }
            else
                timeLeftLabel.Text = text;
        }
        #endregion
    }
}
