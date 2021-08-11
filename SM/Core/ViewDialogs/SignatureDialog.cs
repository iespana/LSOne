using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Cryptography;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewCore.Dialogs
{
    public partial class SignatureDialog : DialogBase
    {
        private Guid actionGuid;
        private IConnectionManager connection;
        private IApplicationCallbacks callbacks;

        public SignatureDialog(IConnectionManager connection,IApplicationCallbacks callbacks, Guid actionGuid)
            : base()
        {
            this.actionGuid = actionGuid;
            this.connection = connection;
            this.callbacks = callbacks;

            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return callbacks;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.FromArgb(222, 222, 222));

            e.Graphics.DrawLine(pen, 1, 1, panel2.Width, 1);

            pen.Dispose();
        }

       
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidatePassword())
            {
                return;
            }

            var cmd = connection.Connection.CreateCommand("spSECURITY_SignAction_1_0");

            connection.Connection.MakeParam(cmd, "dataareaID", connection.Connection.DataAreaId);
            connection.Connection.MakeParam(cmd,"contextGuid",(Guid)connection.CurrentUser.ID);
            connection.Connection.MakeParam(cmd,"actionGuid",actionGuid);
            connection.Connection.MakeParam(cmd,"reason",tbReason.Text);

            connection.Connection.ExecuteNonQuery(cmd,false);

            DialogResult = DialogResult.OK;

            Close();
        }

        private bool ValidatePassword()
        {
            errorProvider1.Clear();

            if(!connection.IsSameAsCurrentPassword(SecureStringHelper.FromString(tbPassword.Text)))
            {
                errorProvider1.SetError(tbPassword, Properties.Resources.PasswordIsIncorrect);

                return false;
            }

            return true;
        }

        private void tbPassword_Validating(object sender, CancelEventArgs e)
        {
            ValidatePassword();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = tbReason.Text.Length > 0 && connection.IsSameAsCurrentPassword(SecureStringHelper.FromString(tbPassword.Text));

            if (btnOK.Enabled)
            {
                errorProvider1.Clear();
            }
        }

       
    }
}