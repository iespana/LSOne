using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.ViewPlugins.Inventory.Reports
{
    public partial class PurchaseOrderReport : DevExpress.XtraReports.UI.XtraReport
    {
        public PurchaseOrderReport()
        {
            InitializeComponent();
        }

        public PurchaseOrderReport(
            PurchaseOrder purchaseOrder, 
            List<PurchaseOrderLine> purchaseOrderLines, 
            List<PurchaseOrderMiscCharges> miscCharges, 
            CompanyInfo companyInfo)
            :this()
        {
            this.purchaseOrder.DataSource = purchaseOrder;
            this.purchaseOrderLines.DataSource = purchaseOrderLines;
            this.companyInfo.DataSource = companyInfo;

            this.xrSubreport1.ReportSource = new POMiscChargesSubReport(miscCharges);
        }


        public string TotalPrice
        {
            get { return lblTotalPrice.Text; }
            set { lblTotalPrice.Text = value; }
        }
        
    }
}
