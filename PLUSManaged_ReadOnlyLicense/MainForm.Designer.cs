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
            this.refreshLicenseButton = new System.Windows.Forms.Button();
            this.activateManuallyButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.activateButton = new System.Windows.Forms.Button();
            this.statusTextLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.deactivateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnRefreshLicense
            // 
            this.refreshLicenseButton.Enabled = false;
            this.refreshLicenseButton.Location = new System.Drawing.Point(43, 135);
            this.refreshLicenseButton.Name = "refreshLicenseButton";
            this.refreshLicenseButton.Size = new System.Drawing.Size(105, 23);
            this.refreshLicenseButton.TabIndex = 16;
            this.refreshLicenseButton.Text = "&Refresh License";
            this.refreshLicenseButton.UseVisualStyleBackColor = true;
            this.refreshLicenseButton.Click += new System.EventHandler(this.refreshLicenseButton_Click);
            // 
            // btnActivateManually
            // 
            this.activateManuallyButton.Location = new System.Drawing.Point(234, 135);
            this.activateManuallyButton.Name = "activateManuallyButton";
            this.activateManuallyButton.Size = new System.Drawing.Size(111, 23);
            this.activateManuallyButton.TabIndex = 15;
            this.activateManuallyButton.Text = "Activate &Manually";
            this.activateManuallyButton.UseVisualStyleBackColor = true;
            this.activateManuallyButton.Click += new System.EventHandler(this.activateManuallyButton_Click);
            // 
            // btnExit
            // 
            this.exitButton.Location = new System.Drawing.Point(468, 135);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 14;
            this.exitButton.Text = "E&xit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // btnActivate
            // 
            this.activateButton.Location = new System.Drawing.Point(351, 135);
            this.activateButton.Name = "activateButton";
            this.activateButton.Size = new System.Drawing.Size(111, 23);
            this.activateButton.TabIndex = 13;
            this.activateButton.Text = "Activate &Online";
            this.activateButton.UseVisualStyleBackColor = true;
            this.activateButton.Click += new System.EventHandler(this.activateButton_Click);
            // 
            // lblStatusText
            // 
            this.statusTextLabel.Location = new System.Drawing.Point(57, 9);
            this.statusTextLabel.Name = "statusTextLabel";
            this.statusTextLabel.Size = new System.Drawing.Size(486, 123);
            this.statusTextLabel.TabIndex = 12;
            this.statusTextLabel.Text = "[License Status]";
            // 
            // lblStatusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(11, 9);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(40, 13);
            this.statusLabel.TabIndex = 11;
            this.statusLabel.Text = "Status:";
            // 
            // DeactivateButton
            // 
            this.deactivateButton.Location = new System.Drawing.Point(154, 135);
            this.deactivateButton.Name = "deactivateButton";
            this.deactivateButton.Size = new System.Drawing.Size(75, 23);
            this.deactivateButton.TabIndex = 17;
            this.deactivateButton.Text = "&Deactivate";
            this.deactivateButton.UseVisualStyleBackColor = true;
            this.deactivateButton.Click += new System.EventHandler(this.deactivateButton_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 167);
            this.Controls.Add(this.deactivateButton);
            this.Controls.Add(this.refreshLicenseButton);
            this.Controls.Add(this.activateManuallyButton);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.activateButton);
            this.Controls.Add(this.statusTextLabel);
            this.Controls.Add(this.statusLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sample Licensing Application";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button refreshLicenseButton;
        private System.Windows.Forms.Button activateManuallyButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button activateButton;
        private System.Windows.Forms.Label statusTextLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button deactivateButton;
    }
}

