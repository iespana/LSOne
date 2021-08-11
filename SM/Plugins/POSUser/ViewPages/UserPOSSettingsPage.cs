using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.ViewPlugins.POSUser.Dialogs;
using LSOne.DataLayer.BusinessObjects.Profiles;

namespace LSOne.ViewPlugins.POSUser.ViewPages
{
    public partial class UserPOSSettingsPage : UserControl, ITabView
    {
        DataLayer.BusinessObjects.UserManagement.POSUser posUser;
        DataLayer.BusinessObjects.UserManagement.User user;
        Dictionary<string, string[]> keyboardCodes = new Dictionary<string, string[]>();

        IPlugin userProfileEditor;

        public UserPOSSettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new UserPOSSettingsPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            var pricesLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            var percentageLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

            if (!isRevert)
            {
                user = (DataLayer.BusinessObjects.UserManagement.User)internalContext;
                posUser = Providers.POSUserData.
                    Get(PluginEntry.DataModel, user.StaffID, UsageIntentEnum.Normal);

                userProfileEditor = PluginEntry.Framework.FindImplementor(this, "CanEditUserProfile", null);
                btnEditUserProfile.Visible = (userProfileEditor != null);
            }

            btnEditUserProfile.Enabled = (posUser.UserProfileID != "" && posUser.UserProfileID != RecordIdentifier.Empty);
             
            tbNameOnReceipt.Text = posUser.NameOnReceipt;
            txtEmail.Text = user.Email;

            UserProfile userProfile = Providers.UserProfileData.Get(PluginEntry.DataModel, posUser.UserProfileID);
            cmbUserProfile.SelectedData = userProfile == null ? new DataEntity("", Properties.Resources.NoSelection) : new DataEntity(userProfile.ID, userProfile.Text);

            if(!PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.SecurityEditUser))
            {
                btnEditEmail.Enabled =
                tbNameOnReceipt.Enabled =
                cmbUserProfile.Enabled = false;
            }
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if(txtEmail.Text != user.Email) return true;
            if (tbNameOnReceipt.Text != posUser.NameOnReceipt) return true;
            if (cmbUserProfile.SelectedData.ID != posUser.UserProfileID) return true;

            return false;
        }

        public bool SaveData()
        {
            posUser.NameOnReceipt = tbNameOnReceipt.Text;
            posUser.UserProfileID = cmbUserProfile.SelectedData.ID;

            Providers.POSUserData.Save(PluginEntry.DataModel, posUser);

            if(txtEmail.Text != user.Email)
            {
                user.Email = txtEmail.Text;
                Providers.UserData.Save(PluginEntry.DataModel, user);
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "User", user.ID, null);

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("POSUsers", posUser.ID, Properties.Resources.POSUserText, true));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(changeIdentifier == user.ID)
            {
                if(objectName == "UserProfileUser")
                {
                    DataEntity entity = (DataEntity)param;

                    if (entity.ID == "")
                        entity.Text = Properties.Resources.NoSelection;

                    cmbUserProfile.SelectedData = entity;
                    posUser.UserProfileID = cmbUserProfile.SelectedData.ID;
                }
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnEditUserProfile_Click(object sender, EventArgs e)
        {
            userProfileEditor.Message(this, "EditUserProfile", cmbUserProfile.SelectedData.ID);
        }

        private void cmbUserProfile_SelectedDataChanged(object sender, EventArgs e)
        {
            btnEditUserProfile.Enabled = (cmbUserProfile.SelectedData.ID != "" && cmbUserProfile.SelectedData.ID != RecordIdentifier.Empty);
        }

        private void cmbUserProfile_RequestData(object sender, EventArgs e)
        {
            List<UserProfile> items = Providers.UserProfileData.GetList(PluginEntry.DataModel);
            cmbUserProfile.SetData(items, null);
        }

        private void btnEditEmail_Click(object sender, EventArgs e)
        {
            EditEmailDialog dlg = new EditEmailDialog(txtEmail.Text);
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                txtEmail.Text = dlg.Email;
            }
        }
    }
}
