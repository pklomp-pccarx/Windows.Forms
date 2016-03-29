using System;
using System.Windows.Forms;

namespace com.softwarekey.Client.Sample
{
    /// <summary>The find and replace form.</summary>
    public partial class FindAndReplaceForm : Form
    {
        #region Private Member Variables
        private bool m_IsFindOnly = false;
        #endregion

        #region Public Constructors
        /// <summary>Creates a new <see cref="FindAndReplaceForm"/>.</summary>
        ///<param name="isFindOnly">Whether or not the dialog should only display options for finding text.</param>
        public FindAndReplaceForm(bool isFindOnly)
        {
            m_IsFindOnly = isFindOnly;
            InitializeComponent();
        }
        #endregion

        #region Button Event Handlers
        /// <summary>Replace button click event handler.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void replaceButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (((MainForm)this.Owner).EditorTextBox.SelectedText.Length != 0 && ((MainForm)this.Owner).EditorTextBox.SelectedText == findTextBox.Text)
                {
                    ((MainForm)this.Owner).EditorTextBox.SelectedText = replaceTextBox.Text;
                }
                else
                {
                    findNextButton_Click(sender, e);
                    if (((MainForm)this.Owner).EditorTextBox.SelectedText.Length != 0)
                    {
                        ((MainForm)this.Owner).EditorTextBox.SelectedText = replaceTextBox.Text;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>Find button click event handler.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void findNextButton_Click(object sender, EventArgs e)
        {
            try
            {
                int StartPosition = ((MainForm)this.Owner).EditorTextBox.SelectionStart + ((MainForm)this.Owner).EditorTextBox.SelectedText.Length;
                StringComparison SearchType;
                SearchType = StringComparison.OrdinalIgnoreCase;
                StartPosition = ((MainForm)this.Owner).EditorTextBox.Text.IndexOf(findTextBox.Text, StartPosition, SearchType);

                if (!(StartPosition > -1))
                {
                    MessageBox.Show(this, "Editor has finished searching the document", "Simple Text Editor", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    ((MainForm)this.Owner).EditorTextBox.SelectionStart = 0;
                    ((MainForm)this.Owner).EditorTextBox.SelectionLength = 0;
                    return;
                }

                ((MainForm)this.Owner).EditorTextBox.Select(StartPosition, findTextBox.Text.Length);
                ((MainForm)this.Owner).EditorTextBox.ScrollToCaret();
                this.Focus();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>Cancel button implementation</summary>
        /// <param name="sender">Object</param>
        /// <param name="e">EventArgs</param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>Replace all button click event handler.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void replaceAllButton_Click(object sender, EventArgs e)
        {
            string replacedText = "";
            string originalText = ((MainForm)this.Owner).EditorTextBox.Text;
            replacedText = ReplaceAll(originalText, findTextBox.Text, replaceTextBox.Text);
            ((MainForm)this.Owner).EditorTextBox.Text = replacedText;
        }
        #endregion

        #region Form Event Handler
        /// <summary>Form load event handler.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FindAndReplaceForm_Load(object sender, EventArgs e)
        {
            if (m_IsFindOnly)
            {
                this.Text = "Find";
                replaceWithLabel.Visible = false;
                replaceTextBox.Visible = false;
                replaceButton.Visible = false;
                replaceAllButton.Visible = false;
                findReplaceCancelButton.Location = new System.Drawing.Point(240, 40);
                this.Height = 100;
            }

            findTextBox.Text = ((MainForm)this.Owner).EditorTextBox.SelectedText;
        }

        /// <summary>Form key press event handler.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void FindAndReplaceForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
        #endregion

        #region Textbox Event Handler
        /// <summary>Find textbox text changed implementation</summary>
        /// <param name="sender">object</param>
        /// <param name="e">EventArgs</param>
        private void findTextBox_TextChanged(object sender, EventArgs e)
        {
            if (findTextBox.Text.Length > 0)
            {
                findNextButton.Enabled = true;
                replaceButton.Enabled = true;
                replaceAllButton.Enabled = true;
                replaceTextBox.Enabled = true;
            }
            else
            {
                findNextButton.Enabled = false;
                replaceButton.Enabled = false;
                replaceAllButton.Enabled = false;
                replaceTextBox.Enabled = false;
            }
        }

        /// <summary>Find textbox key press event handler.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void findTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                findNextButton_Click(sender, e);
            }
        }

        /// <summary>Replace textbox key press event handler.</summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">Event arguments.</param>
        private void replaceTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                replaceButton_Click(sender, e);
                this.Focus();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>Replaces all matching text with the specified replacement.</summary>
        /// <param name="original">The complete, original string.</param>
        /// <param name="pattern">The pattern to find and replace.</param>
        /// <param name="replacement">The new content that will replace the pattern found.</param>
        /// <returns>Returns the new, modified buffer with all instances of the specified pattern replaced with the specified replacement.</returns>
        private string ReplaceAll(string original, string pattern, string replacement)
        {
            int count, position0, position1;
            count = position0 = position1 = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                {
                    chars[count++] = original[i];
                }
                for (int i = 0; i < replacement.Length; ++i)
                {
                    chars[count++] = replacement[i];
                }
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
            {
                chars[count++] = original[i];
            }
            return new string(chars, 0, count);
        }
        #endregion
    }
}