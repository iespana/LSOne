using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class SimpleDatabaseStringControl : UserControl
    {
        protected DatabaseString databaseString;
        private bool enableTextChangedEvent;

        private ButtonTextBox.ButtonItemCheckBox buttonItemShowPassword;

        public SimpleDatabaseStringControl()
        {
            InitializeComponent();

            buttonItemShowPassword = new ButtonTextBox.ButtonItemCheckBox();
            buttonItemShowPassword.Image = Properties.Resources.LockImage;
            buttonItemShowPassword.Text = null;
            buttonItemShowPassword.CheckedChanged += new EventHandler(buttonItemShowPassword_CheckedChanged);

            buttonTextBox.Buttons.Add(buttonItemShowPassword);
            IsDDString = true;
        }

        public DatabaseString DatabaseString
        {
            get
            {
                return databaseString;
            }
            set
            {
                databaseString = value;
                if (databaseString != null)
                {
                    IsDDString = databaseString.IsDDString;
                }
                SetTextBoxText();
                OnDatabaseStringChanged();
            }
        }

        public bool IsDDString { get; set; }

        public bool ShowPasswordCheckboxVisible
        {
            get { return buttonItemShowPassword.Visible; }
            set { buttonItemShowPassword.Visible = value; }
        }

        public string DatabaseStringTextBoxToolTip
        {
            get { return toolTip.GetToolTip(buttonTextBox); }
            set { toolTip.SetToolTip(buttonTextBox, value); }
        }

        public event EventHandler DatabaseStringChanged;

        protected virtual void UpdateActions()
        {
        }

        private void buttonTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateActions();

            if (!enableTextChangedEvent || buttonTextBox.TextLength == 0)
            {
                return;
            }

            bool oldIsDDString = IsDDString;
            string oldPassword = null;
            if (databaseString != null)
            {
                oldIsDDString = databaseString.IsDDString;
                if (databaseString.HasPassword)
                {
                    oldPassword = databaseString.Password;
                }
                
            }
            DatabaseString dbString;
            try
            {
                dbString = new DatabaseString(oldIsDDString, buttonTextBox.Text);
            }
            catch (ArgumentException)
            {
                dbString = null;
            }

            if (dbString != null && !buttonItemShowPassword.Checked && dbString.HasPassword && oldPassword != null && dbString.Password == DatabaseString.PasswordPlaceholder)
            {
                dbString.Password = oldPassword;
            }
            databaseString = dbString;
            OnDatabaseStringChanged();
        }

        protected void OnDatabaseStringChanged()
        {
            if (DatabaseStringChanged != null)
            {
                DatabaseStringChanged(this, EventArgs.Empty);
            }
        }

        private void buttonItemShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            SetTextBoxText();
        }

        protected void SetTextBoxText()
        {
            enableTextChangedEvent = false;
            if (databaseString != null)
            {
                buttonTextBox.Text = databaseString.ToString(buttonItemShowPassword.Checked);
            }
            else
            {
                buttonTextBox.Clear();
            }
            UpdateActions();
            enableTextChangedEvent = true;
        }

        private void buttonTextBox_Validating(object sender, CancelEventArgs e)
        {
            SetTextBoxText();
        }

        public override string Text
        {
            get
            {
                return buttonTextBox.Text;
            }
            set
            {
                buttonTextBox.Text = value;
            }
        }
    }
}
