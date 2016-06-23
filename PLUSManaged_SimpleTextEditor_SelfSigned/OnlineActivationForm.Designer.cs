namespace com.softwarekey.Client.Sample
{
    partial class OnlineActivationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OnlineActivationForm));
            this.activateButton = new DevExpress.XtraEditors.SimpleButton();
            this.exitButton = new DevExpress.XtraEditors.SimpleButton();
            this.licenseIDTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.MaskedTextBox();
            this.installationNameLabel = new System.Windows.Forms.Label();
            this.installationNameTextBox = new System.Windows.Forms.TextBox();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.formAssistant1 = new DevExpress.XtraBars.FormAssistant();
            this.layoutConverter1 = new DevExpress.XtraLayout.Converter.LayoutConverter(this.components);
            this.OnlineActivationFormConvertedLayout = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.OnlineActivationFormConvertedLayout)).BeginInit();
            this.OnlineActivationFormConvertedLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // activateButton
            // 
            this.activateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.activateButton.Location = new System.Drawing.Point(427, 367);
            this.activateButton.Name = "activateButton";
            this.activateButton.Size = new System.Drawing.Size(126, 22);
            this.activateButton.StyleController = this.OnlineActivationFormConvertedLayout;
            this.activateButton.TabIndex = 3;
            this.activateButton.Text = "&Activate";
            this.activateButton.Click += new System.EventHandler(this.activateButton_Click);
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(276, 367);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(147, 22);
            this.exitButton.StyleController = this.OnlineActivationFormConvertedLayout;
            this.exitButton.TabIndex = 2;
            this.exitButton.Text = "&Cancel";
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // licenseIDTextBox
            // 
            this.licenseIDTextBox.Location = new System.Drawing.Point(349, 160);
            this.licenseIDTextBox.MaxLength = 10;
            this.licenseIDTextBox.Name = "licenseIDTextBox";
            this.licenseIDTextBox.Size = new System.Drawing.Size(192, 20);
            this.licenseIDTextBox.TabIndex = 1;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(349, 184);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(192, 27);
            this.passwordTextBox.TabIndex = 3;
            // 
            // installationNameLabel
            // 
            this.installationNameLabel.Location = new System.Drawing.Point(24, 160);
            this.installationNameLabel.Name = "installationNameLabel";
            this.installationNameLabel.Size = new System.Drawing.Size(174, 191);
            this.installationNameLabel.TabIndex = 4;
            this.installationNameLabel.Text = "Installation Name:";
            // 
            // installationNameTextBox
            // 
            this.installationNameTextBox.Location = new System.Drawing.Point(202, 215);
            this.installationNameTextBox.MaxLength = 50;
            this.installationNameTextBox.Name = "installationNameTextBox";
            this.installationNameTextBox.Size = new System.Drawing.Size(192, 20);
            this.installationNameTextBox.TabIndex = 5;
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.instructionsLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.instructionsLabel.Location = new System.Drawing.Point(24, 42);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(517, 72);
            this.instructionsLabel.TabIndex = 0;
            this.instructionsLabel.Text = resources.GetString("instructionsLabel.Text");
            // 
            // OnlineActivationFormConvertedLayout
            // 
            this.OnlineActivationFormConvertedLayout.Controls.Add(this.instructionsLabel);
            this.OnlineActivationFormConvertedLayout.Controls.Add(this.installationNameTextBox);
            this.OnlineActivationFormConvertedLayout.Controls.Add(this.passwordTextBox);
            this.OnlineActivationFormConvertedLayout.Controls.Add(this.licenseIDTextBox);
            this.OnlineActivationFormConvertedLayout.Controls.Add(this.installationNameLabel);
            this.OnlineActivationFormConvertedLayout.Controls.Add(this.exitButton);
            this.OnlineActivationFormConvertedLayout.Controls.Add(this.activateButton);
            this.OnlineActivationFormConvertedLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OnlineActivationFormConvertedLayout.Location = new System.Drawing.Point(0, 0);
            this.OnlineActivationFormConvertedLayout.Name = "OnlineActivationFormConvertedLayout";
            this.OnlineActivationFormConvertedLayout.Root = this.layoutControlGroup1;
            this.OnlineActivationFormConvertedLayout.Size = new System.Drawing.Size(565, 401);
            this.OnlineActivationFormConvertedLayout.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlGroup3,
            this.layoutControlItem1,
            this.layoutControlItem2,
            this.emptySpaceItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(565, 401);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "groupBox1item";
            this.layoutControlGroup2.Size = new System.Drawing.Size(545, 118);
            this.layoutControlGroup2.Text = "Licenses Information";
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.emptySpaceItem2});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 118);
            this.layoutControlGroup3.Name = "credentialsGroupBoxitem";
            this.layoutControlGroup3.Size = new System.Drawing.Size(545, 237);
            this.layoutControlGroup3.Text = "Credentials";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.exitButton;
            this.layoutControlItem1.Location = new System.Drawing.Point(264, 355);
            this.layoutControlItem1.Name = "exitButtonitem";
            this.layoutControlItem1.Size = new System.Drawing.Size(151, 26);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.activateButton;
            this.layoutControlItem2.Location = new System.Drawing.Point(415, 355);
            this.layoutControlItem2.Name = "activateButtonitem";
            this.layoutControlItem2.Size = new System.Drawing.Size(130, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.instructionsLabel;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem3.Name = "instructionsLabelitem";
            this.layoutControlItem3.Size = new System.Drawing.Size(521, 76);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.installationNameTextBox;
            this.layoutControlItem4.Location = new System.Drawing.Point(178, 55);
            this.layoutControlItem4.Name = "installationNameTextBoxitem";
            this.layoutControlItem4.Size = new System.Drawing.Size(343, 24);
            this.layoutControlItem4.Text = "(Optional - e.g.: \"My Laptop\")";
            this.layoutControlItem4.TextLocation = DevExpress.Utils.Locations.Right;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(143, 13);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.passwordTextBox;
            this.layoutControlItem5.Location = new System.Drawing.Point(178, 24);
            this.layoutControlItem5.Name = "passwordTextBoxitem";
            this.layoutControlItem5.Size = new System.Drawing.Size(343, 31);
            this.layoutControlItem5.Text = "Password:";
            this.layoutControlItem5.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(143, 13);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.licenseIDTextBox;
            this.layoutControlItem6.Location = new System.Drawing.Point(178, 0);
            this.layoutControlItem6.Name = "licenseIDTextBoxitem";
            this.layoutControlItem6.Size = new System.Drawing.Size(343, 24);
            this.layoutControlItem6.Text = "License ID:";
            this.layoutControlItem6.TextLocation = DevExpress.Utils.Locations.Left;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(143, 13);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.installationNameLabel;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem7.Name = "installationNameLabelitem";
            this.layoutControlItem7.Size = new System.Drawing.Size(178, 195);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 355);
            this.emptySpaceItem1.MaxSize = new System.Drawing.Size(264, 26);
            this.emptySpaceItem1.MinSize = new System.Drawing.Size(264, 26);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(264, 26);
            this.emptySpaceItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(178, 79);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(343, 116);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // OnlineActivationForm
            // 
            this.AcceptButton = this.activateButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.exitButton;
            this.ClientSize = new System.Drawing.Size(565, 401);
            this.Controls.Add(this.OnlineActivationFormConvertedLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "OnlineActivationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activate Online";
            this.Shown += new System.EventHandler(this.OnlineActivationForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.OnlineActivationFormConvertedLayout)).EndInit();
            this.OnlineActivationFormConvertedLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal DevExpress.XtraEditors.SimpleButton activateButton;
        internal DevExpress.XtraEditors.SimpleButton exitButton;
        internal System.Windows.Forms.TextBox licenseIDTextBox;
        internal System.Windows.Forms.MaskedTextBox passwordTextBox;
        internal System.Windows.Forms.Label installationNameLabel;
        internal System.Windows.Forms.TextBox installationNameTextBox;
        internal System.Windows.Forms.Label instructionsLabel;
        private DevExpress.XtraBars.FormAssistant formAssistant1;
        private DevExpress.XtraLayout.LayoutControl OnlineActivationFormConvertedLayout;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.Converter.LayoutConverter layoutConverter1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
    }
}