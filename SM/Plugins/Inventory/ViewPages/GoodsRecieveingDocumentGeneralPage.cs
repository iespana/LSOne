using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class GoodsRecieveingDocumentGeneralPage : UserControl, ITabView
    {
        private GoodsReceivingDocument goodsReceivingDocument;
        public GoodsRecieveingDocumentGeneralPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.GoodsRecieveingDocumentGeneralPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {


            goodsReceivingDocument = (GoodsReceivingDocument)internalContext;
            LoadData();
           
        }

        private void LoadData()
        {
            tbGoodsReceivingDocumentID.Text = (string)goodsReceivingDocument.GoodsReceivingID;
            cmbVendor.SelectedData = new DataEntity("", goodsReceivingDocument.VendorName);
            tbStatus.Text = goodsReceivingDocument.StatusText;
        }
        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "GoodsReceivingDocument":
                    if (changeIdentifier == goodsReceivingDocument.ID)
                    {
                        goodsReceivingDocument = (GoodsReceivingDocument) param;
                        LoadData();
                    }
                    break;
                case "GoodsReceivingDocumentRefreshSearch":
                    LoadData();
                    break;
              
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

      
    }
}
