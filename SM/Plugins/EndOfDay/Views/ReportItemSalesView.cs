using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.EndOfDay.DataLayer;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;
using LSOne.ViewPlugins.EndOfDay.Reports;

namespace LSOne.ViewPlugins.EndOfDay.Views
{
    public partial class ReportItemSalesView : ViewBase
    {
        private Store storeInfo = null;


        public ReportItemSalesView()
        {
            InitializeComponent();

            dtpFromDate.MaxDate = DateTime.Now;
            dtpToDate.MaxDate = DateTime.Now;

            Attributes = ViewAttributes.Help | ViewAttributes.Close | ViewAttributes.ContextBar;
            GrayHeaderHeight = 180;

            if (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty)
                cmbStoreSelect.Enabled = true;
            else
            {
                storeInfo = Providers.StoreData.Get(PluginEntry.DataModel, (string)PluginEntry.DataModel.CurrentStoreID);
                cmbStoreSelect.SelectedData = storeInfo;
                cmbStoreSelect.Enabled = false;
            }
            SetViewButtonAccess();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.ItemSalesReport;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.ItemSalesReport;            
            //HeaderIcon = null;

            rbTodaysReport_Click(null, null);
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }


        private void btnViewReport_Click(object sender, EventArgs e)
        {
            IPlugin reportPlugin = PluginEntry.Framework.FindImplementor(this, "CanDisplayXtraReport", null);

            if (reportPlugin != null)
            {
                string reportHeader = rgrReportSelect.Properties.Items[rgrReportSelect.SelectedIndex].Description;

                reportPlugin.Message(this, "ShowReport", new object[] { "ItemSalesReport", GetItemSalesReport(), reportHeader, Properties.Resources.Symbol_Clock });
            }
        }

        private int FindCurrentlySelectedTab()
        {
            foreach (Control ctrl in groupPanel1.Controls)
                if (ctrl is RadioButton)
                    if (((RadioButton)ctrl).Checked == true)
                        return Convert.ToInt16(((RadioButton)ctrl).Tag);
            return -1;
        }

        private void rbTodaysReport_Click(object sender, EventArgs e)
        {

        }

        private void SetViewButtonAccess()
        {           
            btnGetReport.Enabled = (               
            dtpFromDate.Value <= DateTime.Now
            && dtpToDate.Value <= DateTime.Now 
            && dtpFromDate.Value.Date <= dtpToDate.Value.Date
            && (cmbStoreSelect.SelectedData != null)          
            );                    
        }
      
        private string GetReportHeaderText()
        {
            return this.dtpFromDate.Value.ToShortDateString() + " - " + this.dtpToDate.Value.ToShortDateString();
        }

        private DevExpress.XtraReports.UI.XtraReport GetItemSalesReport()
        {
            ItemSaleLineData itemSalesData = new ItemSaleLineData(PluginEntry.DataModel.Connection.DataAreaId, dtpFromDate.Value, dtpToDate.Value, "", "", ReportIntervalType.ByDate);
            List<ItemSaleLine> itemSaleLines = itemSalesData.GetItemSaleLines(PluginEntry.DataModel, (string)storeInfo.ID, true);
            List<SalesTotalAmounts> totals = itemSalesData.GetTotals(PluginEntry.DataModel, (string)storeInfo.ID, true);
            CompanyInfo companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel,true);

            ReportItemSales itemSalesReport = new ReportItemSales(itemSaleLines, companyInfo, totals);

            itemSalesReport.PeriodDefinition = GetReportHeaderText();
            itemSalesReport.CreationDate = DateTime.Now.ToLongDateString();
            itemSalesReport.StoreID = storeInfo.Text;

            return itemSalesReport;
        }

        private void txtStmtFrom_KeyUp_1(object sender, KeyEventArgs e)
        {
            SetViewButtonAccess();
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            SetViewButtonAccess();
        }        

        private void btnSalesPerTerminal_Click(object sender, EventArgs e)
        {
            IPlugin reportPlugin = PluginEntry.Framework.FindImplementor(this, "CanDisplayXtraReport", null);

            if (reportPlugin != null)
            {
                string reportHeader = rgrReportSelect.Properties.Items[rgrReportSelect.SelectedIndex].Description;

                reportPlugin.Message(this, "ShowReport", new object[] { "ItemSalesTerminalReport", GetSalesPerTerminalReport(), reportHeader, Properties.Resources.Symbol_Clock });
            }
        }

        private DevExpress.XtraReports.UI.XtraReport GetSalesPerTerminalReport()
        {

            TerminalSaleData terminalSaleData = new TerminalSaleData(PluginEntry.DataModel.Connection.DataAreaId, dtpFromDate.Value, dtpToDate.Value, "", "", ReportIntervalType.ByDate);
            List<TerminalSaleLine> terminalSaleLines = terminalSaleData.GetTerminalSaleLines(PluginEntry.DataModel, true, (string)storeInfo.ID);
            CompanyInfo companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel, true);
            //Store storeInfo = Providers.StoreData.Get(PluginEntry.DataModel, (string)storeInfo.ID);

            ReportSalesPerTerminal salesPerTerminalReport = new ReportSalesPerTerminal(terminalSaleLines, companyInfo);

            salesPerTerminalReport.PeriodDefinition = GetReportHeaderText();
            salesPerTerminalReport.CreationDate = DateTime.Now.ToLongDateString();
            salesPerTerminalReport.StoreID = storeInfo.Text;

            return salesPerTerminalReport;
        }

        private DevExpress.XtraReports.UI.XtraReport GetSalesPerEmployeeReport()
        {            
            EmployeeSaleData employeeSaleData = new EmployeeSaleData(PluginEntry.DataModel.Connection.DataAreaId, dtpFromDate.Value, dtpToDate.Value, "", "", ReportIntervalType.ByDate);
            List<EmployeeSaleLine> employeeSaleLines = employeeSaleData.GetEmployeeSaleLines(PluginEntry.DataModel, true, (string)storeInfo.ID);
            CompanyInfo companyInfo = Providers.CompanyInfoData.Get(PluginEntry.DataModel, true);
            //Store storeInfo = Providers.StoreData.Get(PluginEntry.DataModel, (string)storeInfo.ID);
       
            ReportSalesPerEmployee salesPerEmployeeReport = new ReportSalesPerEmployee(employeeSaleLines, companyInfo);
            
            salesPerEmployeeReport.PeriodDefinition = GetReportHeaderText();
            salesPerEmployeeReport.CreationDate = DateTime.Now.ToLongDateString();
            salesPerEmployeeReport.StoreID = storeInfo.Text;
            
            return salesPerEmployeeReport;
        }

        private void btnEmployeeSales_Click(object sender, EventArgs e)
        {
            IPlugin reportPlugin = PluginEntry.Framework.FindImplementor(this, "CanDisplayXtraReport", null);

            if (reportPlugin != null)
            {
                string reportHeader = rgrReportSelect.Properties.Items[rgrReportSelect.SelectedIndex].Description;

                reportPlugin.Message(this, "ShowReport", new object[] { "ItemSalesEmployeeReport", GetSalesPerEmployeeReport(), reportHeader, Properties.Resources.Symbol_Clock });
            }
        }
       
        private void btnGetReport_Click(object sender, EventArgs e)
        {
            switch (rgrReportSelect.SelectedIndex)
            {
                case 0:
                    {
                        btnViewReport_Click(this, EventArgs.Empty);                        
                        break;
                    }
                case 1:
                    {
                        btnSalesPerTerminal_Click(this, EventArgs.Empty);
                        break;
                    }
                case 2:
                    {
                        btnEmployeeSales_Click(this, EventArgs.Empty);
                        break;
                    }
            }
        }

        private void radioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetViewButtonAccess(); 
        }

        private void dtpFromDate_ValueChanged_1(object sender, EventArgs e)
        {
            SetViewButtonAccess();
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            SetViewButtonAccess();
        }

        private void cmbStoreSelect_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);            
            cmbStoreSelect.SetData(stores, null);            
        }

        private void cmbStoreSelect_SelectedDataChanged(object sender, EventArgs e)
        {
            storeInfo = Providers.StoreData.Get(PluginEntry.DataModel, (string)cmbStoreSelect.SelectedData.ID); 
            SetViewButtonAccess();
        }
    }
}
