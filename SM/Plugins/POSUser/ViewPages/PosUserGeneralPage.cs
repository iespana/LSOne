using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.POSUser.ViewPages
{
    public partial class PosUserGeneralPage : UserControl, ITabView
    {

        LSOne.DataLayer.BusinessObjects.UserManagement.POSUser posUser;

        IPlugin storeEditor = null;
        IPlugin visualProfileEditor = null;
        IPlugin layoutEditor = null;

        int storeImageIndex = -1;

        public PosUserGeneralPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PosUserGeneralPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                posUser = (LSOne.DataLayer.BusinessObjects.UserManagement.POSUser)internalContext;

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

            //btnEditStore.Enabled = (posUser.StoreID != "" && posUser.StoreID != RecordIdentifier.Empty);

            tbPassword.Text = posUser.Password;
            chkChangePassNextLogon.Checked = posUser.NeedsPasswordChange;
            tbNameOnReceipt.Text = posUser.NameOnReceipt;

            //cmbStore.SelectedData = new DataEntity(posUser.StoreID, posUser.StoreID == "" ? Properties.Resources.NoSelection : posUser.StoreDescription);
            //cmbTouchLayout.SelectedData = new DataEntity(posUser.LayoutID, posUser.LayoutID == "" ? Properties.Resources.NoSelection : posUser.LayoutDescription);
            //cmbVisualProfile.SelectedData = new DataEntity(posUser.VisualProfileID, posUser.VisualProfileID == "" ? Properties.Resources.NoSelection : posUser.VisualProfileDescription);

            //if (posUser.LanguageCode == "")
            //{
            //    cmbLanguage.SelectedIndex = 0;
            //}
            //else
            //{
            //    foreach (object item in cmbLanguage.Items)
            //    {
            //        if (item.ToString() == posUser.LanguageCode)
            //        {
            //            cmbLanguage.SelectedItem = item;
            //            break;
            //        }
            //    }
            //}
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if (tbPassword.Text != posUser.Password) return true;
            if (tbNameOnReceipt.Text != posUser.NameOnReceipt) return true;
            if (chkChangePassNextLogon.Checked != posUser.NeedsPasswordChange) return true;
            //if (cmbStore.SelectedData.ID != posUser.StoreID) return true;
            //if (cmbTouchLayout.SelectedData.ID != posUser.LayoutID) return true;
            //if (cmbVisualProfile.SelectedData.ID != posUser.VisualProfileID) return true;

            string selectedLanguage = (cmbLanguage.SelectedIndex <= 0) ? "" : cmbLanguage.SelectedItem.ToString();
            //if (selectedLanguage != posUser.LanguageCode) return true;

            return false;
        }

        public bool SaveData()
        {
            posUser.Password = tbPassword.Text;
            posUser.NameOnReceipt = tbNameOnReceipt.Text;
            posUser.NeedsPasswordChange = chkChangePassNextLogon.Checked;
            //posUser.StoreID = cmbStore.SelectedData.ID;
            //posUser.LayoutID = cmbTouchLayout.SelectedData.ID;
            //posUser.VisualProfileID = cmbVisualProfile.SelectedData.ID;
            //posUser.LanguageCode = (cmbLanguage.SelectedIndex <= 0) ? "" : cmbLanguage.SelectedItem.ToString();

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void btnEditStore_Click(object sender, EventArgs e)
        {
            storeEditor.Message(this, "EditStore", cmbStore.SelectedData.ID);
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            btnEditStore.Enabled = (cmbStore.SelectedData.ID != "" && cmbStore.SelectedData.ID != RecordIdentifier.Empty);
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {

            List<DataEntity> items = Providers.StoreData.GetList(PluginEntry.DataModel);

            cmbStore.SetData(items,
                PluginEntry.Framework.GetImageList().Images[storeImageIndex],
                true);
        }

        private void cmbVisualProfile_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = Providers.VisualProfileData.GetList(PluginEntry.DataModel);

            cmbVisualProfile.SetData(items,null,true);
        }

        private void cmbTouchLayout_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = Providers.TouchLayoutData.GetList(PluginEntry.DataModel);

            cmbTouchLayout.SetData(items,null,false);
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
            ((DualDataComboBox)sender).SelectedData = new DataEntity();
        }

    }
}
