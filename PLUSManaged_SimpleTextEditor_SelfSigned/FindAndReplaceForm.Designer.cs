namespace com.softwarekey.Client.Sample
{
    partial class FindAndReplaceForm
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
            this.findTextBox = new System.Windows.Forms.TextBox();
            this.replaceTextBox = new System.Windows.Forms.TextBox();
            this.findNextButton = new System.Windows.Forms.Button();
            this.replaceButton = new System.Windows.Forms.Button();
            this.findReplaceCancelButton = new System.Windows.Forms.Button();
            this.findWhatLabel = new System.Windows.Forms.Label();
            this.replaceWithLabel = new System.Windows.Forms.Label();
            this.replaceAllButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // findTextBox
            // 
            this.findTextBox.Location = new System.Drawing.Point(78, 12);
            this.findTextBox.Name = "findTextBox";
            this.findTextBox.Size = new System.Drawing.Size(157, 20);
            this.findTextBox.TabIndex = 0;
            this.findTextBox.TextChanged += new System.EventHandler(this.findTextBox_TextChanged);
            this.findTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.findTextBox_KeyPress);
            // 
            // replaceTextBox
            // 
            this.replaceTextBox.Enabled = false;
            this.replaceTextBox.Location = new System.Drawing.Point(78, 40);
            this.replaceTextBox.Name = "replaceTextBox";
            this.replaceTextBox.Size = new System.Drawing.Size(157, 20);
            this.replaceTextBox.TabIndex = 1;
            this.replaceTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.replaceTextBox_KeyPress);
            // 
            // findNextButton
            // 
            this.findNextButton.Enabled = false;
            this.findNextButton.Location = new System.Drawing.Point(240, 10);
            this.findNextButton.Name = "findNextButton";
            this.findNextButton.Size = new System.Drawing.Size(75, 23);
            this.findNextButton.TabIndex = 2;
            this.findNextButton.Text = "&Find Next";
            this.findNextButton.UseVisualStyleBackColor = true;
            this.findNextButton.Click += new System.EventHandler(this.findNextButton_Click);
            // 
            // replaceButton
            // 
            this.replaceButton.Enabled = false;
            this.replaceButton.Location = new System.Drawing.Point(240, 40);
            this.replaceButton.Name = "replaceButton";
            this.replaceButton.Size = new System.Drawing.Size(75, 23);
            this.replaceButton.TabIndex = 3;
            this.replaceButton.Text = "&Replace";
            this.replaceButton.UseVisualStyleBackColor = true;
            this.replaceButton.Click += new System.EventHandler(this.replaceButton_Click);
            // 
            // findReplaceCancelButton
            // 
            this.findReplaceCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.findReplaceCancelButton.Location = new System.Drawing.Point(240, 100);
            this.findReplaceCancelButton.Name = "findReplaceCancelButton";
            this.findReplaceCancelButton.Size = new System.Drawing.Size(75, 23);
            this.findReplaceCancelButton.TabIndex = 5;
            this.findReplaceCancelButton.Text = "&Cancel";
            this.findReplaceCancelButton.UseVisualStyleBackColor = true;
            this.findReplaceCancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // findWhatLabel
            // 
            this.findWhatLabel.AutoSize = true;
            this.findWhatLabel.Location = new System.Drawing.Point(3, 15);
            this.findWhatLabel.Name = "findWhatLabel";
            this.findWhatLabel.Size = new System.Drawing.Size(59, 13);
            this.findWhatLabel.TabIndex = 5;
            this.findWhatLabel.Text = "Find What:";
            // 
            // replaceWithLabel
            // 
            this.replaceWithLabel.AutoSize = true;
            this.replaceWithLabel.Location = new System.Drawing.Point(3, 43);
            this.replaceWithLabel.Name = "replaceWithLabel";
            this.replaceWithLabel.Size = new System.Drawing.Size(75, 13);
            this.replaceWithLabel.TabIndex = 6;
            this.replaceWithLabel.Text = "Replace With:";
            // 
            // replaceAllButton
            // 
            this.replaceAllButton.Enabled = false;
            this.replaceAllButton.Location = new System.Drawing.Point(240, 70);
            this.replaceAllButton.Name = "replaceAllButton";
            this.replaceAllButton.Size = new System.Drawing.Size(75, 23);
            this.replaceAllButton.TabIndex = 4;
            this.replaceAllButton.Text = "Replace &All";
            this.replaceAllButton.UseVisualStyleBackColor = true;
            this.replaceAllButton.Click += new System.EventHandler(this.replaceAllButton_Click);
            // 
            // FindAndReplaceForm
            // 
            this.AcceptButton = this.findNextButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.findReplaceCancelButton;
            this.ClientSize = new System.Drawing.Size(319, 128);
            this.Controls.Add(this.replaceAllButton);
            this.Controls.Add(this.replaceWithLabel);
            this.Controls.Add(this.findWhatLabel);
            this.Controls.Add(this.findReplaceCancelButton);
            this.Controls.Add(this.replaceButton);
            this.Controls.Add(this.findNextButton);
            this.Controls.Add(this.replaceTextBox);
            this.Controls.Add(this.findTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FindAndReplaceForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find and Replace";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FindAndReplaceForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FindAndReplaceForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label findWhatLabel;
        private System.Windows.Forms.Label replaceWithLabel;
        public System.Windows.Forms.TextBox findTextBox;
        public System.Windows.Forms.TextBox replaceTextBox;
        public System.Windows.Forms.Button findNextButton;
        public System.Windows.Forms.Button replaceButton;
        public System.Windows.Forms.Button findReplaceCancelButton;
        public System.Windows.Forms.Button replaceAllButton;
    }
}