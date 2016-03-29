using System;
using System.Windows.Forms;

namespace NetworkFloatingLicense
{
    public partial class NetworkLicenseInvalidCountdownDlg : Form
    {
        private int m_timeLeft;
        
        public NetworkLicenseInvalidCountdownDlg(int timeTillExit)
        {
            InitializeComponent();

            m_timeLeft = timeTillExit;
            labelTimeLeft.Text = m_timeLeft.ToString();
        }

        private void NetworkSeatInvalidCountdownDialog_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

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

        private void SetLabelText(string text)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new SetControlText(SetLabelText), new Object[] { text });
            }
            else
                labelTimeLeft.Text = text;
        }

    }
}
