namespace com.softwarekey.Client.Sample
{
    partial class LicenseInvalidCountdownForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timeLeftLabel = new System.Windows.Forms.Label();
            this.shutdownTimer = new System.Windows.Forms.Timer(this.components);
            this.shutdownLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelTimeLeft
            // 
            this.timeLeftLabel.AutoSize = true;
            this.timeLeftLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.timeLeftLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLeftLabel.Location = new System.Drawing.Point(112, 64);
            this.timeLeftLabel.Name = "timeLeftLabel";
            this.timeLeftLabel.Size = new System.Drawing.Size(153, 108);
            this.timeLeftLabel.TabIndex = 0;
            this.timeLeftLabel.Text = "60";
            this.timeLeftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.shutdownTimer.Interval = 1000;
            this.shutdownTimer.Tick += new System.EventHandler(this.shutdownTimer_Tick);
            // 
            // label1
            // 
            this.shutdownLabel.AutoSize = true;
            this.shutdownLabel.BackColor = System.Drawing.Color.White;
            this.shutdownLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.shutdownLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shutdownLabel.Location = new System.Drawing.Point(8, 33);
            this.shutdownLabel.Name = "shutdownLabel";
            this.shutdownLabel.Size = new System.Drawing.Size(327, 42);
            this.shutdownLabel.TabIndex = 1;
            this.shutdownLabel.Text = "The Sample Network Floating Licensing\nApplication will shutdown in...";
            this.shutdownLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LicenseInvalidCountdownDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(341, 176);
            this.Controls.Add(this.shutdownLabel);
            this.Controls.Add(this.timeLeftLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LicenseInvalidCountdownDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetworkSeatInvalidDialog";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Load += new System.EventHandler(this.NetworkSeatInvalidCountdownDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timeLeftLabel;
        private System.Windows.Forms.Timer shutdownTimer;
        private System.Windows.Forms.Label shutdownLabel;
    }
}