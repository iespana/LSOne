using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.Reports
{
    public partial class ReportSalesPerTerminal : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportSalesPerTerminal()
        {
            InitializeComponent();
        }
        public ReportSalesPerTerminal(List<TerminalSaleLine> terminalSaleLines, CompanyInfo companyInfo)
            : this()
        {
            this.bsTerminalSales.DataSource = terminalSaleLines;
            this.bsCompanyInfo.DataSource = companyInfo;
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
        public string StoreIDLabel
        {
            get { return xrLblStoreIDH.Text; }
            set { xrLblStoreIDH.Text = value; }
        }
        public string StoreID
        {
            get { return xrLblStoreID.Text; }
            set { xrLblStoreID.Text = value; }
        }
    }
}
