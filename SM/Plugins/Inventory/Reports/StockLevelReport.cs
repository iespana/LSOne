using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;

namespace LSOne.ViewPlugins.Inventory.Reports
{
    public partial class StockLevelReport : DevExpress.XtraReports.UI.XtraReport
    {
        public StockLevelReport()
        {
            InitializeComponent();
        }

        public StockLevelReport(List<InventoryStatus> inventoryList, InventoryGroup inventoryFilterGroup, string filterGroupName, string storeName)
            :this()
        {
            inventoryStatusList.DataSource = inventoryList;

            lblStoreName.Text = storeName;

            switch (inventoryFilterGroup)
            {
                case InventoryGroup.RetailGroup:
                    lblFilterGroup.Text = Properties.Resources.RetailGroup + ": ";
                    break;
                case InventoryGroup.RetailDepartment:
                    lblFilterGroup.Text = Properties.Resources.RetailDepartment + ": ";
                    break;
                case InventoryGroup.Vendor:
                    lblFilterGroup.Text = Properties.Resources.Vendor + ": ";
                    break;
            }

            lblFilterName.Text = filterGroupName;
        }
        
    }
}
