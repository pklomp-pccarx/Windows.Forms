namespace com.softwarekey.Client.Sample
{
    partial class MainForm
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
            this.activateManuallyButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.activateButton = new System.Windows.Forms.Button();
            this.statusTextLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.refreshLicenseButton = new System.Windows.Forms.Button();
            this.deactivateButton = new System.Windows.Forms.Button();
            this.usersLabel = new System.Windows.Forms.Label();
            this.userCountLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // activateManuallyButton
            // 
            this.activateManuallyButton.Location = new System.Drawing.Point(235, 135);
            this.activateManuallyButton.Name = "activateManuallyButton";
            this.activateManuallyButton.Size = new System.Drawing.Size(111, 23);
            this.activateManuallyButton.TabIndex = 9;
            this.activateManuallyButton.Text = "Activate &Manually";
            this.activateManuallyButton.UseVisualStyleBackColor = true;
            this.activateManuallyButton.Click += new System.EventHandler(this.activateManuallyButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(469, 135);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 8;
            this.exitButton.Text = "E&xit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // activateButton
            // 
            this.activateButton.Location = new System.Drawing.Point(352, 135);
            this.activateButton.Name = "activateButton";
            this.activateButton.Size = new System.Drawing.Size(111, 23);
            this.activateButton.TabIndex = 7;
            this.activateButton.Text = "Activate &Online";
            this.activateButton.UseVisualStyleBackColor = true;
            this.activateButton.Click += new System.EventHandler(this.activateButton_Click);
            // 
            // statusTextLabel
            // 
            this.statusTextLabel.Location = new System.Drawing.Point(58, 31);
            this.statusTextLabel.Name = "statusTextLabel";
            this.statusTextLabel.Size = new System.Drawing.Size(486, 96);
            this.statusTextLabel.TabIndex = 6;
            this.statusTextLabel.Text = "[License Status]";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 31);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(40, 13);
            this.statusLabel.TabIndex = 5;
            this.statusLabel.Text = "Status:";
            // 
            // refreshLicenseButton
            // 
            this.refreshLicenseButton.Enabled = false;
            this.refreshLicenseButton.Location = new System.Drawing.Point(43, 135);
            this.refreshLicenseButton.Name = "refreshLicenseButton";
            this.refreshLicenseButton.Size = new System.Drawing.Size(105, 23);
            this.refreshLicenseButton.TabIndex = 10;
            this.refreshLicenseButton.Text = "&Refresh License";
            this.refreshLicenseButton.UseVisualStyleBackColor = true;
            this.refreshLicenseButton.Click += new System.EventHandler(this.refreshLicenseButton_Click);
            // 
            // deactivateButton
            // 
            this.deactivateButton.Enabled = false;
            this.deactivateButton.Location = new System.Drawing.Point(154, 135);
            this.deactivateButton.Name = "deactivateButton";
            this.deactivateButton.Size = new System.Drawing.Size(75, 23);
            this.deactivateButton.TabIndex = 11;
            this.deactivateButton.Text = "Deactivate";
            this.deactivateButton.UseVisualStyleBackColor = true;
            this.deactivateButton.Click += new System.EventHandler(this.deactivateButton_Click);
            // 
            // usersLabel
            // 
            this.usersLabel.AutoSize = true;
            this.usersLabel.Location = new System.Drawing.Point(12, 9);
            this.usersLabel.Name = "usersLabel";
            this.usersLabel.Size = new System.Drawing.Size(37, 13);
            this.usersLabel.TabIndex = 12;
            this.usersLabel.Text = "Users:";
            // 
            // userCountLabel
            // 
            this.userCountLabel.AutoSize = true;
            this.userCountLabel.Location = new System.Drawing.Point(58, 9);
            this.userCountLabel.Name = "userCountLabel";
            this.userCountLabel.Size = new System.Drawing.Size(66, 13);
            this.userCountLabel.TabIndex = 13;
            this.userCountLabel.Text = "[User Count]";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 167);
            this.Controls.Add(this.userCountLabel);
            this.Controls.Add(this.usersLabel);
            this.Controls.Add(this.deactivateButton);
            this.Controls.Add(this.refreshLicenseButton);
            this.Controls.Add(this.activateManuallyButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.activateButton);
            this.Controls.Add(this.statusTextLabel);
            this.Controls.Add(this.statusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sample Network Floating Licensing Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button activateManuallyButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button activateButton;
        private System.Windows.Forms.Label statusTextLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button refreshLicenseButton;
        private System.Windows.Forms.Button deactivateButton;
        private System.Windows.Forms.Label usersLabel;
        private System.Windows.Forms.Label userCountLabel;
    }
}

