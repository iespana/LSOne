using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Images;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    public partial class StoreFormSettingsPage : UserControl, ITabView
    {
        private LSOne.DataLayer.BusinessObjects.StoreManagement.Store store;
        private bool imageChanged;
        private string initialBarcodeSymbology;
        private string initialLogoSize;
        private Image currentLogo;

        public StoreFormSettingsPage()
        {
            InitializeComponent();

            cmbBarcodeSymbology.Items.Clear();

            foreach (object value in Enum.GetValues(typeof(BarcodeType)))
            {
                if ((int)value == 0)
                    continue;

                cmbBarcodeSymbology.Items.Add(Enum.GetName(typeof(BarcodeType), value));
            }

            cmbLogoSize.Items.Clear();
            foreach (object enumItem in Enum.GetValues(typeof(StoreLogoSizeType)))
            {
                cmbLogoSize.Items.Add(StoreLogoSizeTypeHelper.StoreLogoSizeTypeToString((StoreLogoSizeType)Convert.ToByte(enumItem)));
            }
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StoreFormSettingsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (isRevert)
            {
            }

            store = (DataLayer.BusinessObjects.StoreManagement.Store)internalContext;

            SetFormProfileData(store.FormProfileID, store.FormProfileDescription, cmbFormProfile);
            SetFormProfileData(store.EmailFormProfileID, store.EmailFormProfileDescription, cmbEmailFormProfile);

            if (store.PictureID != null && store.PictureID.StringValue != string.Empty)
            {
                currentLogo = Providers.ImageData.Get(PluginEntry.DataModel, store.PictureID);
                picLogoImage.Image = currentLogo.Picture;
                txtImage.Text = currentLogo.Text;
            }

            initialBarcodeSymbology = Enum.GetName(typeof(BarcodeType), store.BarcodeSymbology);
            cmbBarcodeSymbology.SelectedItem = initialBarcodeSymbology;

            txtFormInfo1.Text = store.FormInfoField1;
            txtFormInfo2.Text = store.FormInfoField2;
            txtFormInfo3.Text = store.FormInfoField3;
            txtFormInfo4.Text = store.FormInfoField4;

            chkReturnsPrintedTwice.Checked = store.ReturnsPrintedTwice;
            chkTenderReceiptsAreReprinted.Checked = store.TenderReceiptAreReprinted;

            initialLogoSize = StoreLogoSizeTypeHelper.StoreLogoSizeTypeToString(store.LogoSize);
            cmbLogoSize.SelectedItem = initialLogoSize;
        }

        private void SetFormProfileData(RecordIdentifier formProfileID, string formProfileDescription, DualDataComboBox comboBox)
        {
            DataEntity selectedProfileEntity;
            if (formProfileID == Guid.Empty)
            {
                FormProfile defaultProfile = Providers.FormProfileData.Get(PluginEntry.DataModel, FormProfile.DefaultProfileID);
                selectedProfileEntity = new DataEntity(defaultProfile.ID, defaultProfile.Text);
            }
            else
            {
                selectedProfileEntity = new DataEntity(formProfileID, formProfileDescription);
            }
            comboBox.SelectedData = selectedProfileEntity;
        }

        public bool DataIsModified()
        {
            if (cmbFormProfile.SelectedData.ID != store.FormProfileID)
                return true;
            if (cmbEmailFormProfile.SelectedDataID != store.EmailFormProfileID)
                return true;
            if (imageChanged)
                return true;
            if (initialBarcodeSymbology != cmbBarcodeSymbology.SelectedItem.ToString())
                return true;
            if (txtFormInfo1.Text != store.FormInfoField1)
                return true;
            if (txtFormInfo2.Text != store.FormInfoField2)
                return true;
            if (txtFormInfo3.Text != store.FormInfoField3)
                return true;
            if (txtFormInfo4.Text != store.FormInfoField4)
                return true;
            if (chkReturnsPrintedTwice.Checked != store.ReturnsPrintedTwice)
                return true;
            if (chkTenderReceiptsAreReprinted.Checked != store.TenderReceiptAreReprinted)
                return true;
            if (initialLogoSize != cmbLogoSize.SelectedItem.ToString())
                return true;

            return false;
        }

        public bool SaveData()
        {
            store.FormProfileID = (Guid)cmbFormProfile.SelectedData.ID;
            store.EmailFormProfileID = (Guid)cmbEmailFormProfile.SelectedData.ID;
            store.PictureID = currentLogo == null ? RecordIdentifier.Empty : currentLogo.ID;
            
            store.BarcodeSymbology = (BarcodeType)Enum.Parse(typeof(BarcodeType), cmbBarcodeSymbology.SelectedItem.ToString());
            store.FormInfoField1 = txtFormInfo1.Text;
            store.FormInfoField2 = txtFormInfo2.Text;
            store.FormInfoField3 = txtFormInfo3.Text;
            store.FormInfoField4 = txtFormInfo4.Text;
            store.ReturnsPrintedTwice = chkReturnsPrintedTwice.Checked;
            store.TenderReceiptAreReprinted = chkTenderReceiptsAreReprinted.Checked;
            store.LogoSize = StoreLogoSizeTypeHelper.StringToStoreLogoSizeType(cmbLogoSize.SelectedItem.ToString());

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

        private void cmbFormProfile_RequestData(object sender, EventArgs e)
        {
            cmbFormProfile.SetData(Providers.FormProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void btnAddImage_Click(object sender, EventArgs e)
        {
            Image img = PluginOperations.ShowImageBankSelectDialog(ImageTypeEnum.ReceiptLogo);

            if(img != null)
            {
                imageChanged = true;
                currentLogo = img;
                picLogoImage.Image = img.Picture;
                txtImage.Text = img.Text;
            }
        }

        private void btnDeleteImage_Click(object sender, EventArgs e)
        {
            if (picLogoImage.Image != null)
                imageChanged = true;
            currentLogo = null;
            picLogoImage.Image = null;
            txtImage.Text = "";

            cmbLogoSize.SelectedItem = StoreLogoSizeTypeHelper.StoreLogoSizeTypeToString(StoreLogoSizeType.Normal);
        }

        private void cmbEmailFormProfile_RequestData(object sender, EventArgs e)
        {
            cmbEmailFormProfile.SetData(Providers.FormProfileData.GetList(PluginEntry.DataModel), null);
        }
    }
}
