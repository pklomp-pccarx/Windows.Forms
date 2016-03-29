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
            this.manageLicenseButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.sampleLicensingGui = new com.softwarekey.Client.Gui.LicensingGui();
            this.statusLabel = new System.Windows.Forms.Label();
            this.criticalFeatureButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // manageLicenseButton
            // 
            this.manageLicenseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.manageLicenseButton.Location = new System.Drawing.Point(321, 112);
            this.manageLicenseButton.Name = "manageLicenseButton";
            this.manageLicenseButton.Size = new System.Drawing.Size(125, 23);
            this.manageLicenseButton.TabIndex = 1;
            this.manageLicenseButton.Text = "Manage License";
            this.manageLicenseButton.UseVisualStyleBackColor = true;
            this.manageLicenseButton.Click += new System.EventHandler(this.manageLicenseButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(452, 112);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "E&xit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // sampleLicensingGui
            // 
            this.sampleLicensingGui.AllowManualTriggerCode = true;
            this.sampleLicensingGui.ApplicationLicense = null;
            this.sampleLicensingGui.LicenseManagementActionsAllowed = 127;
            this.sampleLicensingGui.SplashImage = global::com.softwarekey.Client.Sample.Properties.Resources.splashscreen;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(12, 9);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(90, 13);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Status: Loading...";
            // 
            // criticalFeatureButton
            // 
            this.criticalFeatureButton.Enabled = false;
            this.criticalFeatureButton.Location = new System.Drawing.Point(216, 112);
            this.criticalFeatureButton.Name = "criticalFeatureButton";
            this.criticalFeatureButton.Size = new System.Drawing.Size(99, 23);
            this.criticalFeatureButton.TabIndex = 5;
            this.criticalFeatureButton.Text = "Critical Feature";
            this.criticalFeatureButton.UseVisualStyleBackColor = true;
            this.criticalFeatureButton.Click += new System.EventHandler(this.criticalFeatureButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.exitButton;
            this.ClientSize = new System.Drawing.Size(539, 147);
            this.Controls.Add(this.criticalFeatureButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.manageLicenseButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLUSManagedGui Sample Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button manageLicenseButton;
        private System.Windows.Forms.Button exitButton;
        private com.softwarekey.Client.Gui.LicensingGui sampleLicensingGui;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Button criticalFeatureButton;
    }
}
