using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.Reports
{
    public partial class ReportSalesPerEmployee : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportSalesPerEmployee()
        {
            InitializeComponent();
        }

        public ReportSalesPerEmployee(List<EmployeeSaleLine> employeeSaleLines, CompanyInfo companyInfo)
            : this()
        {
            this.bsEmployeeSales.DataSource = employeeSaleLines;
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
        public string TerminalID
        {
            get { return employeeID.Text; }
            set { employeeID.Text = value; }
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
        public string LastName
        {
            get { return lastName.Text; }
            set { lastName.Text = value; }
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
