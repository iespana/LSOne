using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    public partial class FunctionalProfileItemsPage : UserControl, ITabView
    {
        private FunctionalityProfile functionalityProfile;
        private bool defaultImageChanged;

        public FunctionalProfileItemsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new FunctionalProfileItemsPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            functionalityProfile = (FunctionalityProfile)internalContext;

            cmbItemLineAggregate.SelectedIndex = (int)functionalityProfile.AggregateItems;
            chkSalesLineAggregate.Checked = functionalityProfile.AggregateItemsForPrinting;
            chkDisplayVoidedItems.Checked = functionalityProfile.DisplayVoidedItems;
            chkAllowImagesInItemLookup.Checked = functionalityProfile.AllowImageViewInItemLookup;
            chkRememberItemSearchMode.Checked = functionalityProfile.RememberListImageSelection;
            picDefaultImage.Image = functionalityProfile.DefaultItemImage;
            chkShowPrices.Checked = functionalityProfile.ShowPricesByDefault;

            chkRememberItemSearchMode.Enabled = functionalityProfile.AllowImageViewInItemLookup;
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if (cmbItemLineAggregate.SelectedIndex != (int)functionalityProfile.AggregateItems) return true;
            if (chkSalesLineAggregate.Checked != functionalityProfile.AggregateItemsForPrinting) return true;
            if (chkDisplayVoidedItems.Checked != functionalityProfile.DisplayVoidedItems) return true;
            if (chkAllowImagesInItemLookup.Checked != functionalityProfile.AllowImageViewInItemLookup) return true;
            if (chkShowPrices.Checked != functionalityProfile.ShowPricesByDefault) return true;
            if (chkRememberItemSearchMode.Checked != functionalityProfile.RememberListImageSelection) return true;
            if (defaultImageChanged) return true;

            return false;
        }

        public bool SaveData()
        {
            functionalityProfile.AggregateItems = (AggregateItemsModes)cmbItemLineAggregate.SelectedIndex;
            functionalityProfile.AggregateItemsForPrinting = chkSalesLineAggregate.Checked;
            functionalityProfile.DisplayVoidedItems = chkDisplayVoidedItems.Checked;
            functionalityProfile.AllowImageViewInItemLookup = chkAllowImagesInItemLookup.Checked;
            functionalityProfile.RememberListImageSelection = chkRememberItemSearchMode.Checked;
            functionalityProfile.ShowPricesByDefault = chkShowPrices.Checked;
            functionalityProfile.DefaultItemImage = picDefaultImage.Image;

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

        private void btnSetDefaultImage_Click(object sender, EventArgs e)
        {
            string fileName = ImageUtils.BrowseForImageFile(this);
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return;

            defaultImageChanged = true;
            picDefaultImage.Image = ImageUtils.GetCopyOfImage(fileName);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            if (picDefaultImage.Image != null)
                defaultImageChanged = true;
            picDefaultImage.Image = null;
        }

        private void chkAllowImagesInItemLookup_CheckedChanged(object sender, EventArgs e)
        {
            chkRememberItemSearchMode.Enabled = chkAllowImagesInItemLookup.Checked;
        }
    }
}
