namespace CloudControlledNetworkLicensing
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
            this.components = new System.ComponentModel.Container();
            this.sessionDetailsGroupBox = new System.Windows.Forms.GroupBox();
            this.usePollTimerCheckBox = new System.Windows.Forms.CheckBox();
            this.totalSeatsTextBox = new System.Windows.Forms.TextBox();
            this.totalSeatsLabel = new System.Windows.Forms.Label();
            this.seatsAvailableTextBox = new System.Windows.Forms.TextBox();
            this.seatsRemainingLabel = new System.Windows.Forms.Label();
            this.allocatedUntilDateTextBox = new System.Windows.Forms.TextBox();
            this.allocatedUntilDateLabel = new System.Windows.Forms.Label();
            this.sessionIDTextBox = new System.Windows.Forms.TextBox();
            this.sessionIDLabel = new System.Windows.Forms.Label();
            this.closeSessionButton = new System.Windows.Forms.Button();
            this.pollSessionButton = new System.Windows.Forms.Button();
            this.openSessionButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.licenseIDTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.licenseIDLabel = new System.Windows.Forms.Label();
            this.checkoutButton = new System.Windows.Forms.Button();
            this.checkoutStatusLabel = new System.Windows.Forms.Label();
            this.checkInButton = new System.Windows.Forms.Button();
            this.checkoutDurationTextBox = new System.Windows.Forms.TextBox();
            this.checkoutMinMaxLabel = new System.Windows.Forms.Label();
            this.checkInOutGroupBox = new System.Windows.Forms.GroupBox();
            this.checkoutDurationLabel = new System.Windows.Forms.Label();
            this.pollTimer = new System.Windows.Forms.Timer(this.components);
            this.checkoutExpiredTimer = new System.Windows.Forms.Timer(this.components);
            this.sessionDetailsGroupBox.SuspendLayout();
            this.checkInOutGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // sessionDetailsGroupBox
            // 
            this.sessionDetailsGroupBox.Controls.Add(this.usePollTimerCheckBox);
            this.sessionDetailsGroupBox.Controls.Add(this.totalSeatsTextBox);
            this.sessionDetailsGroupBox.Controls.Add(this.totalSeatsLabel);
            this.sessionDetailsGroupBox.Controls.Add(this.seatsAvailableTextBox);
            this.sessionDetailsGroupBox.Controls.Add(this.seatsRemainingLabel);
            this.sessionDetailsGroupBox.Controls.Add(this.allocatedUntilDateTextBox);
            this.sessionDetailsGroupBox.Controls.Add(this.allocatedUntilDateLabel);
            this.sessionDetailsGroupBox.Controls.Add(this.sessionIDTextBox);
            this.sessionDetailsGroupBox.Controls.Add(this.sessionIDLabel);
            this.sessionDetailsGroupBox.Controls.Add(this.closeSessionButton);
            this.sessionDetailsGroupBox.Controls.Add(this.pollSessionButton);
            this.sessionDetailsGroupBox.Controls.Add(this.openSessionButton);
            this.sessionDetailsGroupBox.Controls.Add(this.passwordTextBox);
            this.sessionDetailsGroupBox.Controls.Add(this.licenseIDTextBox);
            this.sessionDetailsGroupBox.Controls.Add(this.passwordLabel);
            this.sessionDetailsGroupBox.Controls.Add(this.licenseIDLabel);
            this.sessionDetailsGroupBox.Location = new System.Drawing.Point(12, 12);
            this.sessionDetailsGroupBox.Name = "sessionDetailsGroupBox";
            this.sessionDetailsGroupBox.Size = new System.Drawing.Size(370, 252);
            this.sessionDetailsGroupBox.TabIndex = 19;
            this.sessionDetailsGroupBox.TabStop = false;
            this.sessionDetailsGroupBox.Text = "Session Details";
            // 
            // usePollTimerCheckBox
            // 
            this.usePollTimerCheckBox.AutoSize = true;
            this.usePollTimerCheckBox.Checked = true;
            this.usePollTimerCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.usePollTimerCheckBox.Location = new System.Drawing.Point(231, 85);
            this.usePollTimerCheckBox.Name = "usePollTimerCheckBox";
            this.usePollTimerCheckBox.Size = new System.Drawing.Size(94, 17);
            this.usePollTimerCheckBox.TabIndex = 3;
            this.usePollTimerCheckBox.Text = "Use Poll Timer";
            this.usePollTimerCheckBox.UseVisualStyleBackColor = true;
            // 
            // totalSeatsTextBox
            // 
            this.totalSeatsTextBox.AcceptsTab = true;
            this.totalSeatsTextBox.Location = new System.Drawing.Point(133, 190);
            this.totalSeatsTextBox.Name = "totalSeatsTextBox";
            this.totalSeatsTextBox.ReadOnly = true;
            this.totalSeatsTextBox.Size = new System.Drawing.Size(190, 20);
            this.totalSeatsTextBox.TabIndex = 7;
            // 
            // totalSeatsLabel
            // 
            this.totalSeatsLabel.AutoSize = true;
            this.totalSeatsLabel.Location = new System.Drawing.Point(66, 193);
            this.totalSeatsLabel.Name = "totalSeatsLabel";
            this.totalSeatsLabel.Size = new System.Drawing.Size(61, 13);
            this.totalSeatsLabel.TabIndex = 18;
            this.totalSeatsLabel.Text = "Total Seats";
            // 
            // seatsAvailableTextBox
            // 
            this.seatsAvailableTextBox.AcceptsTab = true;
            this.seatsAvailableTextBox.Location = new System.Drawing.Point(133, 164);
            this.seatsAvailableTextBox.Name = "seatsAvailableTextBox";
            this.seatsAvailableTextBox.ReadOnly = true;
            this.seatsAvailableTextBox.Size = new System.Drawing.Size(190, 20);
            this.seatsAvailableTextBox.TabIndex = 6;
            // 
            // seatsRemainingLabel
            // 
            this.seatsRemainingLabel.AutoSize = true;
            this.seatsRemainingLabel.Location = new System.Drawing.Point(47, 167);
            this.seatsRemainingLabel.Name = "seatsRemainingLabel";
            this.seatsRemainingLabel.Size = new System.Drawing.Size(80, 13);
            this.seatsRemainingLabel.TabIndex = 17;
            this.seatsRemainingLabel.Text = "Seats Available";
            // 
            // allocatedUntilDateTextBox
            // 
            this.allocatedUntilDateTextBox.AcceptsTab = true;
            this.allocatedUntilDateTextBox.Location = new System.Drawing.Point(133, 136);
            this.allocatedUntilDateTextBox.Name = "allocatedUntilDateTextBox";
            this.allocatedUntilDateTextBox.ReadOnly = true;
            this.allocatedUntilDateTextBox.Size = new System.Drawing.Size(190, 20);
            this.allocatedUntilDateTextBox.TabIndex = 5;
            // 
            // allocatedUntilDateLabel
            // 
            this.allocatedUntilDateLabel.AutoSize = true;
            this.allocatedUntilDateLabel.Location = new System.Drawing.Point(26, 139);
            this.allocatedUntilDateLabel.Name = "allocatedUntilDateLabel";
            this.allocatedUntilDateLabel.Size = new System.Drawing.Size(101, 13);
            this.allocatedUntilDateLabel.TabIndex = 16;
            this.allocatedUntilDateLabel.Text = "Allocated Until Date";
            // 
            // sessionIDTextBox
            // 
            this.sessionIDTextBox.AcceptsTab = true;
            this.sessionIDTextBox.Location = new System.Drawing.Point(133, 110);
            this.sessionIDTextBox.Name = "sessionIDTextBox";
            this.sessionIDTextBox.ReadOnly = true;
            this.sessionIDTextBox.Size = new System.Drawing.Size(190, 20);
            this.sessionIDTextBox.TabIndex = 4;
            // 
            // sessionIDLabel
            // 
            this.sessionIDLabel.AutoSize = true;
            this.sessionIDLabel.Location = new System.Drawing.Point(69, 113);
            this.sessionIDLabel.Name = "sessionIDLabel";
            this.sessionIDLabel.Size = new System.Drawing.Size(58, 13);
            this.sessionIDLabel.TabIndex = 15;
            this.sessionIDLabel.Text = "Session ID";
            // 
            // closeSessionButton
            // 
            this.closeSessionButton.Enabled = false;
            this.closeSessionButton.Location = new System.Drawing.Point(231, 216);
            this.closeSessionButton.Name = "closeSessionButton";
            this.closeSessionButton.Size = new System.Drawing.Size(92, 23);
            this.closeSessionButton.TabIndex = 9;
            this.closeSessionButton.Text = "Close Session";
            this.closeSessionButton.UseVisualStyleBackColor = true;
            this.closeSessionButton.Click += new System.EventHandler(this.closeSessionButton_Click);
            // 
            // pollSessionButton
            // 
            this.pollSessionButton.Enabled = false;
            this.pollSessionButton.Location = new System.Drawing.Point(133, 216);
            this.pollSessionButton.Name = "pollSessionButton";
            this.pollSessionButton.Size = new System.Drawing.Size(92, 23);
            this.pollSessionButton.TabIndex = 8;
            this.pollSessionButton.Text = "Poll Session";
            this.pollSessionButton.UseVisualStyleBackColor = true;
            this.pollSessionButton.Click += new System.EventHandler(this.pollSessionButton_Click);
            // 
            // openSessionButton
            // 
            this.openSessionButton.Location = new System.Drawing.Point(133, 81);
            this.openSessionButton.Name = "openSessionButton";
            this.openSessionButton.Size = new System.Drawing.Size(92, 23);
            this.openSessionButton.TabIndex = 2;
            this.openSessionButton.Text = "Open Session";
            this.openSessionButton.UseVisualStyleBackColor = true;
            this.openSessionButton.Click += new System.EventHandler(this.openSessionButton_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.AcceptsTab = true;
            this.passwordTextBox.Location = new System.Drawing.Point(133, 55);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(190, 20);
            this.passwordTextBox.TabIndex = 1;
            // 
            // licenseIDTextBox
            // 
            this.licenseIDTextBox.AcceptsTab = true;
            this.licenseIDTextBox.Location = new System.Drawing.Point(133, 29);
            this.licenseIDTextBox.Name = "licenseIDTextBox";
            this.licenseIDTextBox.Size = new System.Drawing.Size(190, 20);
            this.licenseIDTextBox.TabIndex = 0;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(71, 58);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(56, 13);
            this.passwordLabel.TabIndex = 14;
            this.passwordLabel.Text = "Password ";
            // 
            // licenseIDLabel
            // 
            this.licenseIDLabel.AutoSize = true;
            this.licenseIDLabel.Location = new System.Drawing.Point(66, 32);
            this.licenseIDLabel.Name = "licenseIDLabel";
            this.licenseIDLabel.Size = new System.Drawing.Size(61, 13);
            this.licenseIDLabel.TabIndex = 13;
            this.licenseIDLabel.Text = "License ID ";
            // 
            // checkoutButton
            // 
            this.checkoutButton.Enabled = false;
            this.checkoutButton.Location = new System.Drawing.Point(133, 79);
            this.checkoutButton.Name = "checkoutButton";
            this.checkoutButton.Size = new System.Drawing.Size(92, 23);
            this.checkoutButton.TabIndex = 11;
            this.checkoutButton.Text = "Checkout";
            this.checkoutButton.UseVisualStyleBackColor = true;
            this.checkoutButton.Click += new System.EventHandler(this.checkoutButton_Click);
            // 
            // checkoutStatusLabel
            // 
            this.checkoutStatusLabel.AutoSize = true;
            this.checkoutStatusLabel.Location = new System.Drawing.Point(6, 16);
            this.checkoutStatusLabel.Name = "checkoutStatusLabel";
            this.checkoutStatusLabel.Size = new System.Drawing.Size(89, 13);
            this.checkoutStatusLabel.TabIndex = 20;
            this.checkoutStatusLabel.Text = "Status: Unknown";
            // 
            // checkInButton
            // 
            this.checkInButton.Enabled = false;
            this.checkInButton.Location = new System.Drawing.Point(231, 79);
            this.checkInButton.Name = "checkInButton";
            this.checkInButton.Size = new System.Drawing.Size(92, 23);
            this.checkInButton.TabIndex = 12;
            this.checkInButton.Text = "Check In";
            this.checkInButton.UseVisualStyleBackColor = true;
            this.checkInButton.Click += new System.EventHandler(this.checkInButton_Click);
            // 
            // checkoutDurationTextBox
            // 
            this.checkoutDurationTextBox.Enabled = false;
            this.checkoutDurationTextBox.Location = new System.Drawing.Point(146, 40);
            this.checkoutDurationTextBox.Name = "checkoutDurationTextBox";
            this.checkoutDurationTextBox.Size = new System.Drawing.Size(177, 20);
            this.checkoutDurationTextBox.TabIndex = 10;
            // 
            // checkoutMinMaxLabel
            // 
            this.checkoutMinMaxLabel.AutoSize = true;
            this.checkoutMinMaxLabel.Location = new System.Drawing.Point(143, 63);
            this.checkoutMinMaxLabel.Name = "checkoutMinMaxLabel";
            this.checkoutMinMaxLabel.Size = new System.Drawing.Size(157, 13);
            this.checkoutMinMaxLabel.TabIndex = 22;
            this.checkoutMinMaxLabel.Text = "Must be between 0 and 1 hours";
            this.checkoutMinMaxLabel.Visible = false;
            // 
            // checkInOutGroupBox
            // 
            this.checkInOutGroupBox.Controls.Add(this.checkInButton);
            this.checkInOutGroupBox.Controls.Add(this.checkoutButton);
            this.checkInOutGroupBox.Controls.Add(this.checkoutStatusLabel);
            this.checkInOutGroupBox.Controls.Add(this.checkoutDurationTextBox);
            this.checkInOutGroupBox.Controls.Add(this.checkoutMinMaxLabel);
            this.checkInOutGroupBox.Controls.Add(this.checkoutDurationLabel);
            this.checkInOutGroupBox.Location = new System.Drawing.Point(12, 270);
            this.checkInOutGroupBox.Name = "checkInOutGroupBox";
            this.checkInOutGroupBox.Size = new System.Drawing.Size(367, 114);
            this.checkInOutGroupBox.TabIndex = 23;
            this.checkInOutGroupBox.TabStop = false;
            this.checkInOutGroupBox.Text = "Session Checkin/Checkout";
            // 
            // checkoutDurationLabel
            // 
            this.checkoutDurationLabel.AutoSize = true;
            this.checkoutDurationLabel.Location = new System.Drawing.Point(6, 43);
            this.checkoutDurationLabel.Name = "checkoutDurationLabel";
            this.checkoutDurationLabel.Size = new System.Drawing.Size(134, 13);
            this.checkoutDurationLabel.TabIndex = 21;
            this.checkoutDurationLabel.Text = "Checkout Duration (hours):";
            // 
            // pollTimer
            // 
            this.pollTimer.Tick += new System.EventHandler(this.pollTimer_Tick);
            // 
            // checkoutExpiredTimer
            // 
            this.checkoutExpiredTimer.Tick += new System.EventHandler(this.checkoutExpiredTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 396);
            this.Controls.Add(this.sessionDetailsGroupBox);
            this.Controls.Add(this.checkInOutGroupBox);
            this.Name = "MainForm";
            this.Text = "Cloud-Controlled Network Floating Example";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.sessionDetailsGroupBox.ResumeLayout(false);
            this.sessionDetailsGroupBox.PerformLayout();
            this.checkInOutGroupBox.ResumeLayout(false);
            this.checkInOutGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox sessionDetailsGroupBox;
        private System.Windows.Forms.TextBox totalSeatsTextBox;
        private System.Windows.Forms.Label totalSeatsLabel;
        private System.Windows.Forms.TextBox seatsAvailableTextBox;
        private System.Windows.Forms.Label seatsRemainingLabel;
        private System.Windows.Forms.TextBox allocatedUntilDateTextBox;
        private System.Windows.Forms.Label allocatedUntilDateLabel;
        private System.Windows.Forms.TextBox sessionIDTextBox;
        private System.Windows.Forms.Label sessionIDLabel;
        private System.Windows.Forms.Button closeSessionButton;
        private System.Windows.Forms.Button pollSessionButton;
        private System.Windows.Forms.Button openSessionButton;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.TextBox licenseIDTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Label licenseIDLabel;
        private System.Windows.Forms.Button checkoutButton;
        private System.Windows.Forms.Label checkoutStatusLabel;
        private System.Windows.Forms.Button checkInButton;
        private System.Windows.Forms.TextBox checkoutDurationTextBox;
        private System.Windows.Forms.Label checkoutMinMaxLabel;
        private System.Windows.Forms.GroupBox checkInOutGroupBox;
        private System.Windows.Forms.Label checkoutDurationLabel;
        private System.Windows.Forms.Timer pollTimer;
        private System.Windows.Forms.CheckBox usePollTimerCheckBox;
        private System.Windows.Forms.Timer checkoutExpiredTimer;
    }
}

