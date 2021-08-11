using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using Image = LSOne.DataLayer.BusinessObjects.Images.Image;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Visual
{
    public partial class VisualProfileReceiptSettingsPage : UserControl, ITabView
    {
        private RecordIdentifier receiptReturnImageID;
        private VisualProfile visualProfile;

        public VisualProfileReceiptSettingsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new VisualProfileReceiptSettingsPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            visualProfile = (VisualProfile)internalContext;

            receiptReturnImageID = visualProfile.ReceiptReturnBackgroundImageID;
            cmbPaymentLines.SelectedItem = MapPaymentLinesPercentToText(visualProfile.ReceiptPaymentLinesSize);
            cmbReceiptReturnImageLayout.SelectedIndex = (int)visualProfile.ReceiptReturnBackgroundImageLayout;
            cwReceiptReturnBorderColor.SelectedColor = Color.FromArgb(visualProfile.ReceiptReturnBorderColor);
            chkShowCurrSymOnColumns.Checked = visualProfile.ShowCurrencySymbolOnColumns;

            Image img = Providers.ImageData.Get(PluginEntry.DataModel, visualProfile.ReceiptReturnBackgroundImageID);

            if (img != null)
            {
                txtReceiptReturnImage.Text = img.Text;
            }
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if (Convert.ToInt32(cmbPaymentLines.SelectedItem.ToString()) != visualProfile.ReceiptPaymentLinesSize) return true;
            if (receiptReturnImageID != visualProfile.ReceiptReturnBackgroundImageID) return true;
            if ((ImageLayout)cmbReceiptReturnImageLayout.SelectedIndex != visualProfile.ReceiptReturnBackgroundImageLayout) return true;
            if (cwReceiptReturnBorderColor.SelectedColor.ToArgb() != visualProfile.ReceiptReturnBorderColor) return true;
            if (chkShowCurrSymOnColumns.Checked != visualProfile.ShowCurrencySymbolOnColumns) return true;

            return false;
        }

        public bool SaveData()
        {
            visualProfile.ReceiptPaymentLinesSize = Convert.ToInt32(cmbPaymentLines.SelectedItem.ToString());
            visualProfile.ReceiptReturnBackgroundImageID = receiptReturnImageID;
            visualProfile.ReceiptReturnBackgroundImageLayout = (ImageLayout)cmbReceiptReturnImageLayout.SelectedIndex;
            visualProfile.ReceiptReturnBorderColor = cwReceiptReturnBorderColor.SelectedColor.ToArgb();
            visualProfile.ShowCurrencySymbolOnColumns = chkShowCurrSymOnColumns.Checked;

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

        private void btnEditReceiptReturnImage_Click(object sender, EventArgs e)
        {
            IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "ShowImageBankSelectDialog", null);

            if (plugin != null)
            {
                Image img = (Image)plugin.Message(null, "ShowImageBankSelectDialog", ImageTypeEnum.Other);

                if (img != null)
                {
                    txtReceiptReturnImage.Text = img.Text;
                    receiptReturnImageID = img.ID;
                }
            }
        }

        private string MapPaymentLinesPercentToText(int receiptPaymentLinesSize)
        {
            switch (receiptPaymentLinesSize)
            {
                case 30: return "30";
                case 40: return "40";
                case 50: return "50";
                case 60: return "60";
                default: return "30";
            }
        }
    }
}
