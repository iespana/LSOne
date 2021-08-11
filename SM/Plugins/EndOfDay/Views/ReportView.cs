using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Statements;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.EndOfDay.Views
{
    public partial class ReportView : ViewBase
    {

        private Store storeInfo;
        private DecimalLimit decimalLimiter;// = new DecimalLimit(0, 2);   
        private RecordIdentifier statementID;
        private RecordIdentifier currentStore;

        public ReportView()
        {
            InitializeComponent();
            Attributes = ViewAttributes.Help | ViewAttributes.Close | ViewAttributes.ContextBar;

            if (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty)
            {
                var stores = Providers.StoreData.GetList(PluginEntry.DataModel);
                if (stores.Count != 1)
                    cmbStoreSelect.Enabled = true;
                else
                {
                    cmbStoreSelect.SelectedData = stores[0];
                    storeInfo = Providers.StoreData.Get(PluginEntry.DataModel, stores[0].ID);
                    cmbStoreSelect.Enabled = false;
                }
            }
            else
            {
                storeInfo = Providers.StoreData.Get(PluginEntry.DataModel, (string)PluginEntry.DataModel.CurrentStoreID);
                cmbStoreSelect.SelectedData = storeInfo;
                cmbStoreSelect.Enabled = false;
            }
            cmbStatement.SelectedData = new DataEntity("", "");
            statementID = RecordIdentifier.Empty;

            HeaderText = Properties.Resources.FinancialReport;
            decimalLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            currentStore = cmbStoreSelect.SelectedDataID;

        }


        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.FinancialReport;
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            IPlugin reportPlugin = PluginEntry.Framework.FindImplementor(this, "CanDisplayXtraReport", null);

            if (reportPlugin != null)
            {
                RecordIdentifier reportID = "1";

                reportPlugin.Message(this, "ShowReport", new object[] { reportID, GetFinancialReport((string)cmbStoreSelect.SelectedData.ID), Properties.Resources.FinancialReport, Properties.Resources.Symbol_Clock });
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
            dtpFromDate.Enabled = false;
            dtpToDate.Enabled = false;
            btnViewReport.Enabled = false;
            rbTodaysReport.Checked = !rbDateSpan.Checked;
            cmbStatement.Enabled = cmbStoreSelect.SelectedData != null && !rbTodaysReport.Checked;
            SetViewButtonAccess();
        }

        private void SetViewButtonAccess()
        {
            switch (FindCurrentlySelectedTab())
            {
                case 0:
                    {
                        btnViewReport.Enabled = (cmbStoreSelect.Text != "");
                        cmbStatement_RequestClear(null, EventArgs.Empty);
                        break;
                    }
                case 1:
                    {
                        btnViewReport.Enabled = (
                            cmbStoreSelect.Text != ""
                            && dtpFromDate.Text != "" 
                            && dtpToDate.Text != "" 
                            && dtpFromDate.Value <= dtpToDate.Value
                            );
                        dtpFromDate.Enabled = true;
                        dtpToDate.Enabled = true;
                        ClearStatement();
                        break;
                    }
                case 2:
                    {
                        btnViewReport.Enabled = (cmbStoreSelect.Text != "");
                        break;
                    }
            }
        }

        private void ClearStatement()
        {
            if (cmbStatement.SelectedData.ID != "")
            {
                var selectedStatement = Providers.StatementInfoData.Get(PluginEntry.DataModel, cmbStatement.SelectedData.ID);
                if (dtpFromDate.Value >= selectedStatement.StartingTime
                    || dtpToDate.Value <= selectedStatement.StartingTime
                    || dtpFromDate.Value >= selectedStatement.EndingTime
                    || dtpToDate.Value <= selectedStatement.EndingTime)
                {
                    cmbStatement_RequestClear(null, EventArgs.Empty);
                }
            }
        }

        private ReportIntervalType GetReportIntervalType()
        {
            if (rbTodaysReport.Checked)
            {
                return ReportIntervalType.CurrentSaleOnly;
            }
            if (rbDateSpan.Checked)
            {
                return ReportIntervalType.ByDate;
            }
            return ReportIntervalType.Unknown;
        }

        private string GetReportHeaderText()
        {
            switch (GetReportIntervalType())
            {
                case ReportIntervalType.ByDate:
                    string returnString = Properties.Resources.Period + ": " + this.dtpFromDate.Value.ToShortDateString() + " - " + this.dtpToDate.Value.ToShortDateString();   //"Tímabil: "
                    if (cmbStatement.SelectedData.ID != "")
                    {
                        returnString += " " + Properties.Resources.EOD_ID + ": " + this.cmbStatement.SelectedData.ID;
                    }
                    return returnString;
                case ReportIntervalType.CurrentSaleOnly:
                    return Properties.Resources.CurrentTurnover;
                default:
                    return Properties.Resources.UndefinedPeriod;
            }
        }

        private XtraReport GetFinancialReport(string storeID)
        {
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();

            if (rbTodaysReport.Checked) // Todays report
            {
                fromDate = DateTime.Now.Date;
                toDate = fromDate.AddDays(1);
            }
            else // Time period
            {
                fromDate = dtpFromDate.Value.Date;
                toDate = dtpToDate.Value.Date.AddDays(1);
            }
            
            RecordIdentifier statement = cmbStatement.SelectedData != null && (string)cmbStatement.SelectedData.ID != "" ? cmbStatement.SelectedData.ID : RecordIdentifier.Empty;

            return PluginOperations.GetFinancialReport(storeID, fromDate, toDate, statement, GetReportIntervalType(), GetReportHeaderText());
        }

        private void txtStmtFrom_KeyUp_1(object sender, KeyEventArgs e)
        {
            SetViewButtonAccess();
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
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
            if (storeInfo != null && rbDateSpan.Checked)
            {
                cmbStatement.Enabled = true;
            }
            if (cmbStoreSelect.SelectedDataID != currentStore)
            {
                cmbStatement.SelectedData = new DataEntity("", "");
            }
            currentStore = cmbStoreSelect.SelectedDataID;
        }

        private void cmbStatement_RequestData(object sender, EventArgs e)
        {
            List<StatementInfo> statements = Providers.StatementInfoData.GetStatementsForPeriod(PluginEntry.DataModel, storeInfo.ID, StatementInfoSorting.StartingTime, false, StatementTypeEnum.PostedStatements, dtpFromDate.Value, dtpToDate.Value);
            List<DataEntity> statementsEntities = statements.Select(s => new DataEntity(s.ID, s.StartingTime.ToLongDateString() + " - " + s.EndingTime.ToLongDateString())).ToList();
            cmbStatement.SetData(statementsEntities, null);
        }

        private void cmbStatement_RequestClear(object sender, EventArgs e)
        {
            cmbStatement.SelectedData = new DataEntity("", "");
        }

        private void cmbStatement_SelectedDataChanged(object sender, EventArgs e)
        {
            statementID = cmbStatement.SelectedData.ID != "" ? cmbStatement.SelectedData.ID : RecordIdentifier.Empty;
        }
    }
}
