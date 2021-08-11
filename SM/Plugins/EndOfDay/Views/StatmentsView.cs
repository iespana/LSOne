using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraReports.UI;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Statements;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities;
using ListView = LSOne.Controls.ListView;

namespace LSOne.ViewPlugins.EndOfDay.Views
{
    public partial class StatementsView : ViewBase
    {
        private RecordIdentifier selectedID;
        private StatementTypeEnum statementType;
        private WeakReference storeSettingsHandler;
        private IEndOfDayBackOfficeService endOfDayService;
        private Parameters paramsData;
        private DecimalLimit priceLimiter;

        public StatementsView(RecordIdentifier statementID, StatementTypeEnum statementType)
            : this(statementType)
        {
            selectedID = statementID;
        }

        public StatementsView(StatementTypeEnum statementType)
        {
            InitializeComponent();

            IPlugin plugin;

            Attributes = ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Audit;

            plugin = PluginEntry.Framework.FindImplementor(this, "DisplayStoreStatementSettings", null);
            storeSettingsHandler = plugin != null ? new WeakReference(plugin) : null;

            priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            lvItems.SmallImageList = PluginEntry.Framework.GetImageList();

            this.statementType = statementType;

            // Select the store we are working one. If we are on Head Office, select the first store from a list of them
            RecordIdentifier storeID = PluginEntry.DataModel.CurrentStoreID;
            if (storeID != RecordIdentifier.Empty)
            {
                cmbStore.SelectedData = Providers.StoreData.Get(PluginEntry.DataModel, (string)storeID);
                cmbStore.Enabled = false;
            }
            else
            {
                cmbStore.SelectedData = Providers.StoreData.GetList(PluginEntry.DataModel)[0];
            }

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvItems_Opening;

            lvItems.Columns[0].Tag = StatementInfoSorting.ID;
            lvItems.Columns[1].Tag = StatementInfoSorting.StartingTime;
            lvItems.Columns[2].Tag = StatementInfoSorting.EndingTime;
            lvItems.Columns[3].Tag = StatementInfoSorting.CalculatedTime;
            lvItems.Columns[4].Tag = StatementInfoSorting.PostingDate;

            if (statementType == StatementTypeEnum.UnpostedStatements)
            {
                lvItems.Columns.RemoveAt(4);
                btnEdit.Visible = false;
            }

            lvValues.SelectionStyle = ListView.SelectionStyleEnum.GradientRounded;

            if (endOfDayService == null)
            {
                endOfDayService = (IEndOfDayBackOfficeService)PluginEntry.DataModel.Service(ServiceType.EndOfDayBackOfficeService);
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            RecordIdentifier storeID = cmbStore.SelectedData.ID;

            contexts.Add(new AuditDescriptor("Statements", storeID, Properties.Resources.Statements, false));
            contexts.Add(new AuditDescriptor("StatementLines", storeID, Properties.Resources.StatementLines, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                switch (statementType)
                {
                    case StatementTypeEnum.UnpostedStatements:
                        return Properties.Resources.UnpostedStatements;
                    case StatementTypeEnum.PostedStatements:
                        return Properties.Resources.PostedStatements;
                    default:
                        return "";
                }
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return (int)statementType;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            switch (statementType)
            {
                case StatementTypeEnum.UnpostedStatements:
                    HeaderText = Properties.Resources.UnpostedStatements;
                    btnShowReport.Visible = false;
                    break;
                case StatementTypeEnum.PostedStatements:
                    HeaderText = Properties.Resources.PostedStatements;
                    
                    btnPost.Visible = false;
                    btnCalculate.Visible = false;
                    btnRecalculate.Visible = false;
                    btnEdit.Visible = false;
                    btnStatementButtons.Visible = false;
                    btnShowReport.Visible = PluginEntry.DataModel.HasPermission(Permission.ViewFinancialReports);
                    break;
            }

            LoadItems();
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
                {
                    if (storeSettingsHandler != null)
                    {
                        arguments.Add(new ContextBarItem(Properties.Resources.StatementSettings, EditStatementSettings), 100);
                    }
                }
            }
        }

        private RecordIdentifier SelectedStatementID
        {
            get
            {
                return (lvItems.SelectedItems.Count > 0) ? ((StatementInfo) lvItems.SelectedItems[0].Tag).ID : "";
            }
        }

        public override List<IDataEntity> GetListSelection()
        {
            return lvItems.GetSelectedDataEntities();
        }

        private void LoadItems()
        {
            List<StatementInfo> statements;
            ListViewItem item;

            lvItems.Items.Clear();

            statements = Providers.StatementInfoData.GetStatements(PluginEntry.DataModel,
                cmbStore.SelectedData.ID,
                (StatementInfoSorting)lvItems.Columns[lvItems.SortColumn].Tag,
                lvItems.SortedBackwards,
                statementType);

            foreach (StatementInfo statement in statements)
            {
                item = new ListViewItem((string)statement.ID);
                item.SubItems.Add(statement.StartingTime.ToString());
                item.SubItems.Add(statement.EndingTime.ToString());
                item.SubItems.Add(statement.Calculated ? statement.CalculatedTime.ToString() : "");

                if (statementType == StatementTypeEnum.PostedStatements)
                {
                    item.SubItems.Add(statement.PostingDate.ToShortDateString());
                }

                item.Tag = statement;
                item.ImageIndex = -1;

                lvItems.Add(item);

                if (selectedID == ((StatementInfo)item.Tag).ID)
                {
                    item.Selected = true;
                }
            }

            lvItems_SelectedIndexChanged(this, EventArgs.Empty);
            lvItems.Columns[lvItems.SortColumn].ImageIndex = (lvItems.SortedBackwards ? 1 : 0);
            lvItems.BestFitColumns();
            if (SelectedStatementID != "")
            {
                LoadLines();
            }
        }

        private void LoadLines()
        {
            lvValues.ClearRows();

            List<StatementLine> statementLines;

            statementLines = Providers.StatementLineData.GetStatementLines(PluginEntry.DataModel, SelectedStatementID);

            foreach (StatementLine statementLine in statementLines)
            {
                decimal displayBankedAmount = (statementLine.BankedAmount*-1);
                decimal displaySafeAmount = (statementLine.SafeAmount*-1);
                Row row = new Row();

                row.AddCell(new CollapsableCell(true));

                row.AddText(statementLine.TerminalID);
                row.AddText(statementLine.TenderDescription);
                row.AddText(statementLine.TransactionAmount.FormatWithLimits(priceLimiter));
                row.AddText(displayBankedAmount.FormatWithLimits(priceLimiter));
                row.AddText(displaySafeAmount.FormatWithLimits(priceLimiter));
                row.AddText(statementLine.CountedAmount.FormatWithLimits(priceLimiter));
                row.AddText(statementLine.Difference.FormatWithLimits(priceLimiter));                 
                
                row.Tag = statementLine;

                switch (statementLine.StatementStatus)
                {
                    case AllowEODEnums.DisallowSuspendedTransaction:
                    {
                        if (VScroll)
                        {
                            
                        }
                        var errorIcon = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider), "", true);
                        row.AddCell(new IconButtonCell(errorIcon, IconButtonCell.IconButtonIconAlignmentEnum.Left, Properties.Resources.TerminalHasSuspendedTransactions));
                        break;
                    }
                    case AllowEODEnums.DisallowEodMarkMissingOnTerminal:
                    {
                        var errorIcon = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedError), "", true);
                        row.AddCell(new IconButtonCell(errorIcon, IconButtonCell.IconButtonIconAlignmentEnum.Left, Properties.Resources.EndOfDayTransactionMissingForTerminal));
                        break;
                    }
                }

                lvValues.AddRow(row);
            }

            lvValues.AutoSizeColumns(true);
            lvValues.Sort(column1,true);
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData.ID != "")
            {
                e.TextToDisplay = ((DualDataComboBox)sender).SelectedData.ID + " - " + ((DualDataComboBox)sender).SelectedData.Text;
            }
            else
            {
                e.TextToDisplay = "";
            }
        }

        private void CalculateStatement(object sender, EventArgs e)
        {
            StatementInfo statement = (StatementInfo)lvItems.SelectedItems[0].Tag;
               
            DateTime startTime = statement.StartingTime;
            DateTime endTime = statement.EndingTime;
            RecordIdentifier statementID = statement.ID;
            RecordIdentifier storeID = statement.StoreID;

            endOfDayService.CalculateStatement(PluginEntry.DataModel, statementID, storeID, startTime, endTime);

            selectedID = statement.ID;

            LoadItems();
        }

        private void btnStatementButtons_AddButtonClicked(object sender, EventArgs e)
        {
            DataEntity store = GetStoreData();

            var dlg = new Dialogs.StatementDialog(store.ID, store.Text);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedID = dlg.GetSelectedId();
                LoadItems();
            }

           

        }

        private void btnStatementButtons_EditButtonClicked(object sender, EventArgs e)
        {
            DataEntity store = GetStoreData();

            RecordIdentifier selectedStatementID = SelectedStatementID;
           

            var dlg = new Dialogs.StatementDialog(selectedStatementID, store.ID, store.Text);
            selectedID = dlg.GetSelectedId();

            // Only recalculate is ok was pressed and the statement is already calculated
            if (dlg.ShowDialog() == DialogResult.OK && ((StatementInfo)lvItems.SelectedItems[0].Tag).Calculated)
            {
                MessageDialog.Show(Properties.Resources.StatementWillBeRecalculated);

                // Reload the lv tag so that the edited statement gets recalculated
                lvItems.SelectedItems[0].Tag = Providers.StatementInfoData.Get(PluginEntry.DataModel, SelectedStatementID);

                if (btnRecalculate.Enabled)
                {
                    RecalculateStatement(this, EventArgs.Empty);
                }
            }

            LoadItems();
        }

        private void btnStatementButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Properties.Resources.DeleteStatementQuestion, Properties.Resources.DeleteStatement) == DialogResult.Yes)
            {
                RecordIdentifier selectedStatementID = SelectedStatementID;

                StatementInfo statement = Providers.StatementInfoData.Get(PluginEntry.DataModel, selectedStatementID);

                Providers.StatementInfoData.Delete(PluginEntry.DataModel, selectedStatementID, statement.StoreID);
                LoadItems();
            }
        }

        private DataEntity GetStoreData()
        {
            DataEntity store = new DataEntity();

            if (cmbStore.SelectedData != null)
            {
                store.ID = cmbStore.SelectedData.ID;
                store.Text = cmbStore.SelectedData.Text;
            }

            return store;
        }

        private void lvItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvItems.SortColumn == e.Column)
            {
                lvItems.SortedBackwards = !lvItems.SortedBackwards;
            }
            else
            {
                if (lvItems.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvItems.Columns[lvItems.SortColumn].ImageIndex = 2;
                    lvItems.SortColumn = e.Column;
                }
                lvItems.SortedBackwards = false;
            }

            LoadItems();
        }

        private void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We dont want to display anything if we are viewing posted statements
            if (statementType == StatementTypeEnum.PostedStatements)
            {
                if(PluginEntry.DataModel.HasPermission(Permission.ViewFinancialReports))
                {
                    item = new ExtendedMenuItem(
                        Properties.Resources.ShowReport,
                        100,
                        btnShowReport_Click);

                    item.Enabled = btnShowReport.Enabled;
                    menu.Items.Add(item);
                }
            }
            else
            {
                // We can optionally add our own items right here
                item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnStatementButtons_EditButtonClicked);
                item.Image = ContextButtons.GetEditButtonImage();
                item.Enabled = btnStatementButtons.EditButtonEnabled;
                item.Default = true;
                menu.Items.Add(item);

                item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnStatementButtons_AddButtonClicked);
                item.Image = ContextButtons.GetAddButtonImage();
                item.Enabled = btnStatementButtons.AddButtonEnabled;
                menu.Items.Add(item);

                item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnStatementButtons_RemoveButtonClicked);
                item.Image = ContextButtons.GetRemoveButtonImage();
                item.Enabled = btnStatementButtons.RemoveButtonEnabled;
                menu.Items.Add(item);

                item = new ExtendedMenuItem(
                    Properties.Resources.CalculateStatement,
                    400,
                    CalculateStatement);

                item.Enabled = btnCalculate.Enabled;
                menu.Items.Add(item);

                item = new ExtendedMenuItem(
                    Properties.Resources.RecalculateStatement,
                    500,
                    RecalculateStatement);

                item.Enabled = btnRecalculate.Enabled;
                menu.Items.Add(item);

                item = new ExtendedMenuItem(
                    Properties.Resources.PostStatement,
                    600,
                    btnPost_Click);

                item.Enabled = btnPost.Enabled;
                menu.Items.Add(item);
            }
            PluginEntry.Framework.ContextMenuNotify("StatementList", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnStatementButtons.AddButtonEnabled =
                PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay) &&
                statementType == StatementTypeEnum.UnpostedStatements;

            btnStatementButtons.EditButtonEnabled =
                lvItems.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay) &&
                statementType == StatementTypeEnum.UnpostedStatements;

            btnStatementButtons.RemoveButtonEnabled = btnStatementButtons.EditButtonEnabled;

            btnCalculate.Enabled =
                btnStatementButtons.EditButtonEnabled &&
                !((StatementInfo)lvItems.SelectedItems[0].Tag).Calculated &&
                PluginEntry.DataModel.HasPermission(Permission.CalculateStatement);

            btnRecalculate.Enabled =
                btnStatementButtons.EditButtonEnabled &&
                ((StatementInfo)lvItems.SelectedItems[0].Tag).Calculated &&
                PluginEntry.DataModel.HasPermission(Permission.CalculateStatement);

            btnPost.Enabled =
                btnStatementButtons.EditButtonEnabled &&
                !btnCalculate.Enabled &&
                ((StatementInfo)lvItems.SelectedItems[0].Tag).Calculated &&
                !((StatementInfo)lvItems.SelectedItems[0].Tag).Posted &&
                PluginEntry.DataModel.HasPermission(Permission.PostStatement);

            btnShowReport.Enabled = lvItems.SelectedItems.Count > 0 && ((StatementInfo)lvItems.SelectedItems[0].Tag).Calculated &&
                ((StatementInfo) lvItems.SelectedItems[0].Tag).Posted;

            if (lvItems.SelectedItems.Count > 0)
            {
                if (lblGroupHeader.Visible == false)
                {
                    lblGroupHeader.Visible = true;
                    lvValues.Visible = true;
                    btnEdit.Visible = statementType == StatementTypeEnum.UnpostedStatements;
                    lblNoSelection.Visible = false;
                }

                lvValues.ClearRows();
                LoadLines();
            }
            else if (lblGroupHeader.Visible)
            {
                lblGroupHeader.Visible = false;
                lvValues.Visible = false;
                btnEdit.Visible = false;
                lblNoSelection.Visible = true;
            }
        }

        private void lvItems_DoubleClick(object sender, EventArgs e)
        {
            if (lvItems.SelectedItems.Count != 0
                && PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay)
                && statementType == StatementTypeEnum.UnpostedStatements)
            {
                btnStatementButtons_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled =
                lvValues.Selection.Count > 0 && //SelectedItems.Count > 0 &&
                PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay) &&
                statementType == StatementTypeEnum.UnpostedStatements;
        }

        private void lvValues_DoubleClick(object sender, EventArgs e)
        {
            if (lvValues.Selection.Count > 0 //SelectedItems.Count > 0
                && PluginEntry.DataModel.HasPermission(Permission.RunEndOfDay)
                && statementType == StatementTypeEnum.UnpostedStatements)
            {
                btnEditValue_Click(this, EventArgs.Empty);
            }
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {

            StatementLine statementLine = (StatementLine)(lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag);// SelectedItems[0].Tag;

            if (statementLine != null)
            {
                DataEntity store = GetStoreData();

                try
                {
                    var dlg = new Dialogs.StatementLineDialog(statementLine.StatementID, statementLine.LineNumber, store.ID, store.Text);

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        LoadItems();
                    }
                }
                catch (DataIntegrityException ex)
                {
                    if (ex.EntityType == typeof(StatementLine))
                    {
                        MessageDialog.Show(Properties.Resources.DataErrorTender);
                    }
                }
            }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            StatementInfo statement = (StatementInfo)lvItems.SelectedItems[0].Tag;
            PostStatement(statement);
        }

        private bool PostStatement(StatementInfo statement)
        {
            statement.PostingDate = Date.Now;
            RecordIdentifier storeID = statement.StoreID;
            DateTime postingDate = statement.PostingDate.DateTime;

            if (PluginOperations.PostStatement(statement, storeID, postingDate))
            {
                if (paramsData == null)
                {
                    paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                }

                ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                try
                {
                    SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

                    service.UpdateCustomerLedgerAtEOD(PluginEntry.DataModel, siteServiceProfile, statement.ID, UseCentralCustomer(paramsData.SiteServiceProfile));
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // This updates all inventory for every item on the view stack
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ItemInventory", RecordIdentifier.Empty, null);

                LoadItems();

                return true;
            }

            return false;
        }

        private bool UseCentralCustomer(RecordIdentifier siteServiceProfileID)
        {
            siteServiceProfileID = siteServiceProfileID == "" ? RecordIdentifier.Empty : siteServiceProfileID;

            if (siteServiceProfileID == RecordIdentifier.Empty)
            {
                return false;
            }

            SiteServiceProfile profile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, siteServiceProfileID, CacheType.CacheTypeApplicationLifeTime);
            return profile == null || profile.CheckCustomer;
        }

        private void lvValues_RowExpanded(object sender, RowEventArgs args)
        {
            var statement = (StatementInfo)lvItems.SelectedItems[0].Tag;
            var statementLine = (StatementLine)(lvValues.Row(args.RowNumber).Tag);
            var exchangeRateLimiter = new DecimalLimit(4, 4);

            var rows = new List<SubRow>();
            var exchRateColumn = new Column(Properties.Resources.ExchangeRate, true);
            var currencyAmountColumn = new Column(Properties.Resources.CurrencyAmount, true);
            var amountColumn = new Column(Properties.Resources.Amount, true);
            exchRateColumn.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            currencyAmountColumn.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            amountColumn.DefaultHorizontalAlignment = Column.HorizontalAlignmentEnum.Right;
            var subTableColumns = new ColumnCollection()
                {
                    new Column(Properties.Resources.Time, true),
                    new Column(Properties.Resources.TerminalID, true),
                    new Column(Properties.Resources.TransactionType, true),
                    new Column(Properties.Resources.ReceiptID, true),
                    new Column(Properties.Resources.Currency, true),
                    new Column(Properties.Resources.ExchangeRate, true),
                    new Column(Properties.Resources.CurrencyAmount, true),
                    new Column(Properties.Resources.Amount, true)
                };

            List<PaymentTransaction> transactions = Providers.PaymentTransactionData.GetForStatementAndTerminal(PluginEntry.DataModel, statementLine.ID, statement.StoreID, statementLine.TerminalID, statementLine.CurrencyCode, statementLine.TenderID);
                
            foreach (PaymentTransaction transaction in transactions)
            {
                var subRow = new SubRow(subTableColumns);
                subRow.AddText(transaction.TransactionDate.ToString());
                subRow.AddText(transaction.TerminalID.ToString());
                subRow.AddText(Enum.GetName(typeof(TypeOfTransaction), transaction.TransactionType));
                subRow.AddText(transaction.ReceiptID.ToString());
                subRow.AddText(transaction.Currency.ToString());
                subRow.AddText(((transaction.ExchangeRate)/100).FormatWithLimits(exchangeRateLimiter));
                subRow.AddText((transaction.AmountOfCurrency.FormatWithLimits(priceLimiter)));
                subRow.AddText((transaction.AmountTenderd.FormatWithLimits(priceLimiter)));
                rows.Add(subRow);
            }

            ((CollapsableCell)args.Row[0]).AddSubRows(subTableColumns, rows);

            ((CollapsableCell)args.Row[0]).AutoSizeColumns(lvValues);
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void RecalculateStatement(object sender, EventArgs e)
        {
            ClearStatement(SelectedStatementID, (string)cmbStore.SelectedData.ID);
            lvValues.ClearRows();

            CalculateStatement(this, EventArgs.Empty);
        }

        private void ClearStatement(RecordIdentifier statementID, RecordIdentifier storeID)
        {
            Providers.StatementInfoData.ClearCalculatedUnpostedStatement(
                PluginEntry.DataModel,
                statementID,
                storeID);
        }

        private void EditStatementSettings(object sender, EventArgs e)
        {
            if (storeSettingsHandler != null)
            {
                RecordIdentifier storeID = cmbStore.SelectedData.ID;

                ((IPlugin)storeSettingsHandler.Target).Message(this, "EditStore", new object[] { storeID, "StoreStatementSettings" });
            }
        }

        protected override void OnClose()
        {
            lvItems.SmallImageList = null;
            base.OnClose();
        }

        private void btnShowReport_Click(object sender, EventArgs e)
        {
            Guid reportID = new Guid(SystemReportConstants.FinancialReportByStatement);

            IPlugin reportViewer = PluginEntry.Framework.FindImplementor(null, "CanDisplayReport", reportID);

            if (reportViewer != null)
            {
                reportViewer.Message(null, "ShowFinancialReportByStatement", null);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.SystemReportNotFound.Replace("#1", Properties.Resources.FinancialReportByStatement), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private XtraReport GetReport()
        {
            StatementInfo statement = (StatementInfo)lvItems.SelectedItems[0].Tag;
            string headerText = Properties.Resources.EOD_ID + ": " + statement.ID;
            return PluginOperations.GetFinancialReport((string) statement.StoreID, statement.StartingTime, statement.EndingTime, statement.ID, ReportIntervalType.ByDate, headerText);
        }

    }
}
