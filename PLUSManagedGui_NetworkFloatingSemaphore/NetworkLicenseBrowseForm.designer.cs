namespace com.softwarekey.Client.Sample
{
    partial class NetworkLicenseBrowseForm
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
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.cancelDialogButton = new System.Windows.Forms.Button();
            this.invalidPathLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // pathTextBox
            // 
            this.pathTextBox.Location = new System.Drawing.Point(31, 23);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(306, 20);
            this.pathTextBox.TabIndex = 1;
            this.pathTextBox.Text = "Example: \\\\server\\share";
            this.pathTextBox.Click += new System.EventHandler(this.textBoxPath_Click);
            this.pathTextBox.Enter += new System.EventHandler(this.textBoxPath_Enter);
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(363, 22);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(24, 20);
            this.browseButton.TabIndex = 0;
            this.browseButton.Text = "...";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(122, 70);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // cancelDialogButton
            // 
            this.cancelDialogButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelDialogButton.Location = new System.Drawing.Point(203, 70);
            this.cancelDialogButton.Name = "cancelDialogButton";
            this.cancelDialogButton.Size = new System.Drawing.Size(75, 23);
            this.cancelDialogButton.TabIndex = 3;
            this.cancelDialogButton.Text = "Cancel";
            this.cancelDialogButton.UseVisualStyleBackColor = true;
            // 
            // invalidPathLabel
            // 
            this.invalidPathLabel.AutoSize = true;
            this.invalidPathLabel.Location = new System.Drawing.Point(127, 46);
            this.invalidPathLabel.Name = "invalidPathLabel";
            this.invalidPathLabel.Size = new System.Drawing.Size(210, 13);
            this.invalidPathLabel.TabIndex = 4;
            this.invalidPathLabel.Text = "The specified path is invalid or unavailable.";
            this.invalidPathLabel.Visible = false;
            // 
            // NetworkLicenseBrowseForm
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelDialogButton;
            this.ClientSize = new System.Drawing.Size(399, 113);
            this.ControlBox = false;
            this.Controls.Add(this.invalidPathLabel);
            this.Controls.Add(this.cancelDialogButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.pathTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NetworkLicenseBrowseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select network license location...";
            this.Click += new System.EventHandler(this.SimpleOpenFileFolderDialog_Click);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button cancelDialogButton;
        private System.Windows.Forms.Label invalidPathLabel;
    }
}