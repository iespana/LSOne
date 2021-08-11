using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.Reports
{
    public partial class ReportItemSales : DevExpress.XtraReports.UI.XtraReport
    {
        
        public ReportItemSales()
        {
            InitializeComponent();
        }

        public ReportItemSales(List<ItemSaleLine> itemSaleLines, CompanyInfo companyInfo, List<SalesTotalAmounts> totals) : this()
        {
            this.itemSaleLines.DataSource = itemSaleLines;
            this.companyInfo.DataSource = companyInfo;
            this.bsTotals.DataSource = totals;
        }


        public string PeriodDefinition
        {
            get { return periodDefinition.Text; }
            set { periodDefinition.Text = value; }
        }
        public string CreationDate
        {
            get { return lblDateTime.Text; }
            set { lblDateTime.Text = value; }
        }
        public string ItemID
        {
            get { return itemID.Text; }
            set { itemID.Text = value; }
        }
        public string ItemDesc
        {
            get { return itemDescription.Text; }
            set { itemDescription.Text = value; }
        }
        public string VariantID
        {
            get { return variantID.Text; }
            set { variantID.Text = value; }
        }
        public string Quantity
        {
            get { return quantity.Text; }
            set { quantity.Text = value; }
        }
        public string Amount
        {
            get { return amount.Text; }
            set { amount.Text = value; }
        }
        
        public string StoreID
        {
            get { return xrLblStoreID.Text; }
            set { xrLblStoreID.Text = value; }
        }
        public string StoreIDLabel
        {
            get { return xrLblStoreIDH.Text; }
            set { xrLblStoreIDH.Text = value; }
        }
    }
}
