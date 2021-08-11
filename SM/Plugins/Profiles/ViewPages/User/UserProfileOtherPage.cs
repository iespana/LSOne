using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.ViewPages.User
{
    public partial class UserProfileOtherPage : UserControl, ITabView
    {
        private UserProfile userProfile;

        Dictionary<string, string[]> keyboardCodes = new Dictionary<string, string[]>();

        IPlugin storeEditor;
        IPlugin visualProfileEditor;
        IPlugin layoutEditor;

        int storeImageIndex = -1;

        public UserProfileOtherPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new UserProfileOtherPage();
        }

        public bool DataIsModified()
        {
            if (cmbStore.SelectedData.ID != userProfile.StoreID) return true;
            if (cmbTouchLayout.SelectedData.ID != userProfile.LayoutID) return true;
            if (cmbVisualProfile.SelectedData.ID != userProfile.VisualProfileID) return true;

            string selectedLanguage = (cmbLanguage.SelectedIndex <= 0) ? "" : cmbLanguage.SelectedItem.ToString();
            if (selectedLanguage != userProfile.LanguageCode) return true;

            if (cmbKeyboard.Text != Properties.Resources.StoreDefault)
            {
                if (keyboardCodes[cmbKeyboard.SelectedItem.ToString()][0] != userProfile.KeyboardLayoutName) return true;
                if (keyboardCodes[cmbKeyboard.SelectedItem.ToString()][1] != userProfile.KeyboardCode) return true;
            }
            else
            {
                if (userProfile.KeyboardLayoutName != "") return true;
                if (userProfile.KeyboardCode != "") return true;
            }

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("UserProfile", userProfile.ID, Properties.Resources.UserProfile, true));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if(!isRevert)
            {
                userProfile = (UserProfile)internalContext;
                storeEditor = PluginEntry.Framework.FindImplementor(this, "CanEditStore", null);
                visualProfileEditor = PluginEntry.Framework.FindImplementor(this, "CanEditVisualProfiles", null);
                layoutEditor = PluginEntry.Framework.FindImplementor(this, "CanEditLayouts", null);

                if (storeEditor != null)
                {
                    storeImageIndex = (int)storeEditor.Message(this, "StoreImageIndex", null);
                }

                btnEditStore.Visible = (storeEditor != null);
                btnEditVisualProfile.Visible = (visualProfileEditor != null);
                btnEditTouchButtons.Visible = (layoutEditor != null);
            }


            cmbStore.SelectedData = new DataEntity(userProfile.StoreID, userProfile.StoreID != "" ? userProfile.StoreName : Properties.Resources.NoSelection);
            cmbVisualProfile.SelectedData = new DataEntity(userProfile.VisualProfileID, userProfile.VisualProfileID != "" ? userProfile.VisualProfileName : Properties.Resources.NoSelection);
            cmbTouchLayout.SelectedData = new DataEntity(userProfile.LayoutID, userProfile.LayoutID != "" ? userProfile.LayoutName : Properties.Resources.NoSelection);

            cmbLanguage.Text = userProfile.LanguageCode;
            cmbKeyboard.Items.Add(Properties.Resources.StoreDefault);
            if (userProfile.KeyboardLayoutName == "" && userProfile.KeyboardCode == "")
            {
                cmbKeyboard.SelectedItem = Properties.Resources.StoreDefault;
            }

            InputLanguageCollection languages = InputLanguage.InstalledInputLanguages;
            foreach (InputLanguage language in languages)
            {
                string keyboardCode = language.LayoutName + " - (" + language.Culture + ")";
                keyboardCodes[keyboardCode] = new[] { language.LayoutName, language.Culture.ToString() };
                cmbKeyboard.Items.Add(keyboardCode);
                if (language.LayoutName == userProfile.KeyboardLayoutName && language.Culture.ToString() == userProfile.KeyboardCode)
                {
                    cmbKeyboard.SelectedItem = keyboardCode;
                }
            }
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(changeIdentifier == userProfile.ID)
            {
                if(objectName == "UserProfileStore")
                {
                    DataEntity entity = (DataEntity)param;

                    if (entity.ID == "")
                        entity.Text = Properties.Resources.NoSelection;

                    cmbStore.SelectedData = entity;
                    userProfile.StoreID = cmbStore.SelectedData.ID;
                }

                if(objectName == "UserProfileVisualProfile")
                {
                    DataEntity entity = (DataEntity)param;

                    if (entity.ID == "")
                        entity.Text = Properties.Resources.NoSelection;

                    cmbVisualProfile.SelectedData = entity;
                    userProfile.VisualProfileID = cmbVisualProfile.SelectedData.ID;
                }

                if (objectName == "UserProfileLayout")
                {
                    DataEntity entity = (DataEntity)param;

                    if (entity.ID == "")
                        entity.Text = Properties.Resources.NoSelection;

                    cmbTouchLayout.SelectedData = entity;
                    userProfile.LayoutID = cmbTouchLayout.SelectedData.ID;
                }
            }
        }

        public bool SaveData()
        {
            userProfile.StoreID = cmbStore.SelectedData.ID;
            userProfile.LayoutID = cmbTouchLayout.SelectedData.ID;
            userProfile.VisualProfileID = cmbVisualProfile.SelectedData.ID;
            userProfile.LanguageCode = (cmbLanguage.SelectedIndex <= 0) ? "" : cmbLanguage.SelectedItem.ToString();
            if (cmbKeyboard.Text != Properties.Resources.StoreDefault)
            {
                userProfile.KeyboardLayoutName = keyboardCodes[cmbKeyboard.SelectedItem.ToString()][0];
                userProfile.KeyboardCode = keyboardCodes[cmbKeyboard.SelectedItem.ToString()][1];
            }
            else
            {
                userProfile.KeyboardLayoutName = "";
                userProfile.KeyboardCode = "";
            }

            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void btnEditStore_Click(object sender, EventArgs e)
        {
            if (cmbStore.SelectedData.ID != "" && cmbStore.SelectedData.ID != RecordIdentifier.Empty)
            {
                storeEditor.Message(this, "EditStore", cmbStore.SelectedData.ID);
            }
            else
            {
                storeEditor.Message(this, "ViewStores", null);
            }
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = Providers.StoreData.GetList(PluginEntry.DataModel);
            cmbStore.SetData(items, PluginEntry.Framework.GetImageList().Images[storeImageIndex], false);
        }

        private void cmbVisualProfile_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = Providers.VisualProfileData.GetList(PluginEntry.DataModel);
            cmbVisualProfile.SetData(items, null, false);
        }

        private void cmbTouchLayout_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = Providers.TouchLayoutData.GetList(PluginEntry.DataModel);

            cmbTouchLayout.SetData(items, null, false);
        }

        private void btnEditVisualProfile_Click(object sender, EventArgs e)
        {
            visualProfileEditor.Message(this, "EditVisualProfile", cmbVisualProfile.SelectedData.ID);
        }

        private void btnEditTouchButtons_Click(object sender, EventArgs e)
        {
            layoutEditor.Message(this, "EditLayout", cmbTouchLayout.SelectedData.ID);
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", Properties.Resources.NoSelection);
        }
    }
}
