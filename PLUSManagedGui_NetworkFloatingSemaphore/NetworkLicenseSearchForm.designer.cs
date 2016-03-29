namespace com.softwarekey.Client.Sample
{
    partial class NetworkLicenseSearchForm
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.cancelDialogButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(31, 23);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(337, 23);
            this.progressBar.TabIndex = 0;
            this.progressBar.Visible = false;
            // 
            // cancelDialogButton
            // 
            this.cancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelDialogButton.Location = new System.Drawing.Point(162, 70);
            this.cancelDialogButton.Name = "cancelDialogButton";
            this.cancelDialogButton.Size = new System.Drawing.Size(75, 23);
            this.cancelDialogButton.TabIndex = 3;
            this.cancelDialogButton.Text = "Cancel";
            this.cancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // NetworkLicenseSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelDialogButton;
            this.ClientSize = new System.Drawing.Size(399, 113);
            this.ControlBox = false;
            this.Controls.Add(this.cancelDialogButton);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetworkLicenseSearchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Searching for an available seat...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetworkLicenseSearchDlg_FormClosing);
            this.Load += new System.EventHandler(this.NetworkLicenseSearchDlg_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button cancelDialogButton;
    }
}