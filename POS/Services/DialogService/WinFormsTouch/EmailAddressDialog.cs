using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.WinFormsTouch
{
    public partial class EmailAddressDialog : TouchBaseForm
    {
        public string EmailAddress { get; set; }

        private RecordIdentifier ClearEmail = new Guid("42C3E7A9-D124-41FE-8BD3-CF4B4F011A70");
        private ISettings dlgSettings;
        private List<ShorthandItem> shorthandButtons;
        
        public EmailAddressDialog()
        {
            InitializeComponent();

            dlgSettings = (ISettings) DLLEntry.DataModel.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            CreateShorthandButtons();
        }

        private void CreateShorthandButtons()
        {
            shorthandButtons = Providers.ShortHandItemData.GetList(DLLEntry.DataModel, dlgSettings.SiteServiceProfile.ID);
            if (shorthandButtons == null || !shorthandButtons.Any())
            {
                shorthandButtons = new List<ShorthandItem>();
                return;
            }

            foreach (ShorthandItem button in shorthandButtons.OrderBy(o => o.Text))
            {
                touchShorthandButtons.AddButton(button.Text, button.ID, "");
            }

            touchShorthandButtons.AddButton(Properties.Resources.ClearEmail, ClearEmail, ClearEmail.ToString(), TouchButtonType.Action, DockEnum.DockEnd);
            touchShorthandButtons.SetButtonEnabled(ClearEmail.ToString(), tbEmailAddress.Text != "");
        }
        

        private void DisplayEmailAddress()
        {
            tbEmailAddress.Text = EmailAddress;
        }

        private void ReceiptEmail_Shown(object sender, EventArgs e)
        {
            DisplayEmailAddress();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!tbEmailAddress.Text.IsEmail())
            {
                errorProvider1.SetError(tbEmailAddress, Properties.Resources.FieldDoesNotContainValidEmail);
            }

            EmailAddress = tbEmailAddress.Text;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void tbEmailAddress_Enter(object sender, EventArgs e)
        {
            touchKeyboard1.BuddyControl = tbEmailAddress;
            touchKeyboard1.DelayedEnabled = true;
            touchKeyboard1.KeystrokeMode = true;
        }

        private void tbEmailAddress_Leave(object sender, EventArgs e)
        {
            touchKeyboard1.BuddyControl = null;
            touchKeyboard1.DelayedEnabled = false;
        }

        private void touchKeyboard1_ObtainCultureName(object sender, LSOne.Controls.EventArguments.CultureEventArguments args)
        {
            if (dlgSettings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = dlgSettings.UserProfile.KeyboardCode;
                args.LayoutName = dlgSettings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = dlgSettings.Store.KeyboardCode;
                args.LayoutName = dlgSettings.Store.KeyboardLayoutName;
            }
        }

        private void touchShorthandButtons_Click(object sender, ScrollButtonEventArguments args)
        {
            if (args.Tag == null)
            {
                return;
            }

            RecordIdentifier masterID = new Guid(((RecordIdentifier)args.Tag).StringValue);

            if (masterID == ClearEmail)
            {
                tbEmailAddress.Text = "";
                tbEmailAddress.Focus();

            }
            
            ShorthandItem button = shorthandButtons.FirstOrDefault(f => f.ID == masterID);
            if (button != null)
            {
                tbEmailAddress.Text += button.Text;
                tbEmailAddress.SelectionStart = tbEmailAddress.TextLength;
            }
        }

        private void tbEmailAddress_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = tbEmailAddress.Text != "";
            touchShorthandButtons.SetButtonEnabled(ClearEmail.ToString(), tbEmailAddress.Text != "");
        }
    }
}
