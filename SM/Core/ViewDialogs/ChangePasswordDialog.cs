using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewCore.Dialogs
{
    public partial class ChangePasswordDialog : DialogBase
    {
        private bool changePasswordForOther;
        private RecordIdentifier userID;

        private IConnectionManager connection;
        private IApplicationCallbacks callbacks = null;

        public ChangePasswordDialog(IConnectionManager connection, IApplicationCallbacks callbacks, RecordIdentifier userID)
            : base()
        {
            InitializeComponent();

            this.userID = userID;
            this.connection = connection;
            this.callbacks = callbacks;

            if (userID == connection.CurrentUser.ID)
            {
                // Changing password for our self
                changePasswordForOther = false;
            }
            else
            {
                // Changing password for other. 

                // Caller of the dialog is responsible for checking permission and the
                // server will do so also.

                changePasswordForOther = true;

                tbOldPassword.Enabled = false;
                tbOldPassword.BackColor = SystemColors.ButtonFace;

                var user = Providers.UserData.Get(connection, (Guid)userID);

                if (!user.IsServerUser)
                {
                    lblNextLogon.Visible = true;
                    chkNextLogon.Visible = true;
                    chkNextLogon.Checked = true;
                }       
            }
        }

        private ChangePasswordDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return callbacks;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            bool failed = false;

            if (changePasswordForOther)
            {
                if(!connection.ChangePasswordForOtherUser(
                    userID,
                    SecureStringHelper.FromString(tbNewPassword.Text),
                    chkNextLogon.Checked))
                {
                    failed = true;
                }
            }
            else
            {
                if(!connection.ChangePassword(
                    SecureStringHelper.FromString(tbOldPassword.Text), 
                    SecureStringHelper.FromString(tbNewPassword.Text)))
                {
                    failed = true;
                }
            }

            if (!failed)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageDialog.Show(Properties.Resources.ChangingPasswordFailed, 
                    this.Text, 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
            }
        }

        private void AnyTextBox_Validating(object sender, CancelEventArgs e)
        {
            errorProvider1.Clear();
            errorProvider2.Clear();
            errorProvider3.Clear();

            if (!changePasswordForOther)
            {
                if(!connection.IsSameAsCurrentPassword(SecureStringHelper.FromString(tbOldPassword.Text)))
                {
                    errorProvider3.Icon = System.Drawing.Icon.FromHandle(((Bitmap)callbacks.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                    errorProvider3.SetError(tbOldPassword, Properties.Resources.PasswordIsIncorrect);
                }
            }

            if (tbNewPassword.Text.Length != 0)
            {
                if (tbNewPassword.Text.Length < 4)
                {
                    errorProvider1.Icon = System.Drawing.Icon.FromHandle(((Bitmap)callbacks.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                    errorProvider1.SetError(tbNewPassword, Properties.Resources.PasswordMustBe4Letters);
                }

                if (!changePasswordForOther)
                {
                    if(connection.IsSameAsCurrentPassword(SecureStringHelper.FromString(tbNewPassword.Text)))
                    {
                        errorProvider1.Icon = System.Drawing.Icon.FromHandle(((Bitmap)callbacks.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                        errorProvider1.SetError(tbNewPassword, Properties.Resources.NewPasswordCannotBeSameAsOld);
                    }
                }
            }

            if (tbConfirmPassword.Text.Length != 0)
            {
                if (tbConfirmPassword.Text != tbNewPassword.Text)
                {
                    errorProvider2.Icon = System.Drawing.Icon.FromHandle(((Bitmap)callbacks.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                    errorProvider2.SetError(tbConfirmPassword, Properties.Resources.VerifyDoesNotMatchWithNew);
                }
            }
        }

        private void AnyTextBox_TextChanged(object sender, EventArgs e)
        {
            bool box1 = false, box2 = false, box3 = false;

            if (changePasswordForOther)
            {
                box1 = true;
            }
            else
            {
                if(connection.IsSameAsCurrentPassword(SecureStringHelper.FromString(tbOldPassword.Text)) &&
                    !connection.IsSameAsCurrentPassword(SecureStringHelper.FromString(tbNewPassword.Text)))
                {
                    box1 = true;
                }
            }

            if (tbNewPassword.Text.Length >= 4)
            {
                box2 = true;
            }

            if (tbConfirmPassword.Text == tbNewPassword.Text)
            {
                box3 = true;
            }

            btnChange.Enabled = box1 && box2 && box3;
        }

        

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(222, 222, 222));

            e.Graphics.DrawLine(pen, 1, 1, panel2.Width, 1);

            pen.Dispose();
        }
    }
}
