using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class StockCountingGeneralPage : UserControl, ITabView
    {
        InventoryAdjustment stockCounting;
        public StockCountingGeneralPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StockCountingGeneralPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            stockCounting = (InventoryAdjustment)internalContext;
            cmbStore.SelectedData = new DataEntity(stockCounting.StoreId, stockCounting.StoreName);
            tbDescription.Text = stockCounting.Text;
            txtJournalID.Text = (string)stockCounting.ID;
            tbStatus.Text = stockCounting.Posted == 0 ? Properties.Resources.NotPosted : Properties.Resources.Posted;

        }

        public bool DataIsModified()
        {
          
            if (tbDescription.Text != stockCounting.Text)
            {
                return true;
            }
          
            return false;
        }

        public bool SaveData()
        {
            stockCounting.Text = tbDescription.Text;            
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "StockCountPosted":
                    tbStatus.Text = Properties.Resources.Posted;
                    break;
                case "StockCounting":
                    stockCounting = (InventoryAdjustment)param;
                    tbStatus.Text = stockCounting.Posted == 0 ? Properties.Resources.NotPosted : Properties.Resources.Posted;
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
