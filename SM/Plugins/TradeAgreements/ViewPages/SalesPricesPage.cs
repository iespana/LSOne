using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    public partial class SalesPricesPage : UserControl, ITabView
    {
        WeakReference owner;

        public SalesPricesPage(TabControl owner)
        : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new SalesPricesPage();
        }

        public SalesPricesPage()
        {
            InitializeComponent();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                tabSheetTabs.AddTab(new TabControl.Tab(Properties.Resources.SalesPrices, ViewPages.CustomerSalesPricesPage.CreateInstance));
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, context);
            }
            tabSheetTabs.SetData(isRevert, context, internalContext);
        }

        public bool DataIsModified()
        {
            return tabSheetTabs.IsModified();
        }

        public bool SaveData()
        {
            tabSheetTabs.GetData();
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }
    }
}
