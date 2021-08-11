using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses.ImportExport;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.EMail;
using LSOne.Utilities.IO;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.CustomerLedger.Properties;
using ContainerControl = LSOne.Controls.ContainerControl;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.CustomerLedger.ViewPages
{
    public partial class CustomerLedgerPage : ContainerControl, ITabView
	{
        private Customer customer;
        private Parameters paramsData;

        private List<CustomerLedgerEntries> custLedgerLs;

        private DecimalLimit priceLimit;

	    private WeakReference owner;

	    private Setting searchBarSetting;
	    private const string SettingsGuid = "332C5F2C-1593-4089-8377-A03A1C1715B8";

	    private RecordIdentifier storeID;

	    private Dictionary<string, string> stores;
	    private Dictionary<string, string> terminals;

        private SmtpServerData smtpServer;

	    private SiteServiceProfile siteServiceProfile;

        public CustomerLedgerPage(TabControl owner)
		{
			InitializeComponent();

            DoubleBuffered = true;

            if (paramsData == null)
            {
                paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            }

            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            btnAddPayment.Enabled = PluginEntry.DataModel.HasPermission(Permission.CustomerLedgerEntriesEdit);
            tbCreditLimit.Enabled = PluginEntry.DataModel.HasPermission(Permission.CustomerLedgerEntriesEdit);

            lvCustomerLedger.ContextMenuStrip = new ContextMenuStrip();
            lvCustomerLedger.ContextMenuStrip.Opening += lvObjects_Opening;

            priceLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            this.owner = new WeakReference(owner.Parent.Parent);

            searchBar1.BuddyControl = lvCustomerLedger;
            stores = new Dictionary<string, string>();
            terminals = new Dictionary<string, string>();

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
		}

		private void LoadDataInner(bool isRevert, RecordIdentifier context, bool reset = true)
		{
            if (reset)
                itemDataScroll.Reset();
            
            decimal custBalance = 0;
            decimal custTotalSales = 0;

            CustomerLedgerFilter filter = new CustomerLedgerFilter();

		    List<SearchParameterResult> parameters = searchBar1.SearchParameterResults;
            foreach (SearchParameterResult result in parameters)
		    {
		        switch (result.ParameterKey)
		        {
                    case "Date":
                        filter.FromDate = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        filter.ToDate = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
		                break;
                    case "Store" :
		                filter.StoreID = ((DualDataComboBox) result.UnknownControl).SelectedData != null ? ((DualDataComboBox) result.UnknownControl).SelectedData.ID : null;
		                break;
                    case "Terminal" :
                        filter.TerminalID = ((DualDataComboBox)result.UnknownControl).SelectedData != null ? ((DualDataComboBox)result.UnknownControl).SelectedData.ID : null;
		                break;
                    case "Types" :
		                filter.Types |= (byte)(result.CheckedValues[0] ? 1 : 0);
                        filter.Types |= (byte)(result.CheckedValues[1] ? 2 : 0);
                        filter.Types |= (byte)(result.CheckedValues[2] ? 4 : 0);
		                break;
                    case "DocumentNumber" :
		                filter.Receipt = result.SearchModification == SearchParameterResult.SearchModificationEnum.Contains && result.StringValue != "" ? "%" : "";
                        filter.Receipt += result.StringValue;
		                break;
		        }
		    }

            ISiteServiceService service = null;
            try
            {
                paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);

                bool useCentralCustomer = PluginOperations.UseCentralCustomer(paramsData.SiteServiceProfile);

                service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                custBalance = service.GetCustomerBalance(PluginEntry.DataModel, siteServiceProfile, customer.ID, useCentralCustomer, false);
                custTotalSales = service.GetCustomerTotalSales(PluginEntry.DataModel, siteServiceProfile, customer.ID, useCentralCustomer, false);
                int totalRecords = 0;
                filter.RowFrom = itemDataScroll.StartRecord;
                filter.RowTo = itemDataScroll.EndRecord + 1;

                custLedgerLs = service.GetCustomerLedgerEntriesList(PluginEntry.DataModel,
                    siteServiceProfile,
                    customer.ID,
                    out totalRecords,
                    useCentralCustomer,
                    filter, false);

                btnSendViaEmail.Enabled = custLedgerLs != null && custLedgerLs.Count > 0 && service.IsEMailSetupForStore(PluginEntry.DataModel, siteServiceProfile, PluginEntry.DataModel.CurrentStoreID, useCentralCustomer);
                if (btnSendViaEmail.Enabled)
                {
                    smtpServer = service.GetEMailSetupForStore(PluginEntry.DataModel, siteServiceProfile, PluginEntry.DataModel.CurrentStoreID, useCentralCustomer).ToSmtpServerData();
                }
                service.Disconnect(PluginEntry.DataModel);
            }
            catch (Exception ex)
            {
                MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                ((ViewBase)owner.Target).HideProgress();
                return;
            }

            tbBalance.Text = custBalance.FormatWithLimits(priceLimit);
            tbTotalSales.Text = (custTotalSales * -1).FormatWithLimits(priceLimit);

		    lvCustomerLedger.Items.Clear();

            itemDataScroll.RefreshState(custLedgerLs);
            if (custLedgerLs != null)
            {
                foreach (CustomerLedgerEntries rec in custLedgerLs)
                {
                    var item = new ListViewItem(rec.PostingDate.ToShortDateString());

                    switch (rec.EntryType)
                    {
                        case CustomerLedgerEntries.TypeEnum.Invoice:
                            item.ForeColor = ColorPalette.RedLight;
                            break;
                        case CustomerLedgerEntries.TypeEnum.Payment:
                            item.ForeColor = ColorPalette.GreenLight;
                            break;
                        case CustomerLedgerEntries.TypeEnum.CreditMemo:
                            item.ForeColor = ColorPalette.GreenLight;
                            break;
                    }

                    string paymentType = CustomerLedgerEntries.AsString(rec.EntryType);

                    if ((rec.Amount < 0 && rec.EntryType == CustomerLedgerEntries.TypeEnum.Payment) ||
                        (rec.Amount > 0 && rec.EntryType == CustomerLedgerEntries.TypeEnum.Sale))
                    {
                        paymentType = Resources.CreditNote;
                        item.ForeColor = ColorPalette.RedLight;
                    }

                    item.SubItems.Add(paymentType);
                    item.SubItems.Add(rec.Amount.FormatWithLimits(priceLimit, true));
                    item.SubItems.Add(rec.RemainingAmount.FormatWithLimits(priceLimit, true));

                    item.SubItems.Add(CustomerLedgerEntries.AsString(rec.Status));

                    if (stores.ContainsKey((string)rec.StoreId))
                    {
                        item.SubItems.Add(stores[(string) rec.StoreId]);
                    }
                    else
                    {
                        rec.StoreId = rec.StoreId == "" ? RecordIdentifier.Empty : rec.StoreId;
                       
                        DataEntity storeDescription = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, rec.StoreId);
                        storeDescription = storeDescription ?? new DataEntity(RecordIdentifier.Empty, Resources.HeadOffice);
                        
                        stores.Add((string) rec.StoreId, storeDescription.Text);
                        
                        item.SubItems.Add(storeDescription.Text);
                    }

                    if (terminals.ContainsKey((string) rec.TerminalId))
                    {
                        item.SubItems.Add(terminals[(string) rec.TerminalId]);
                    }
                    else
                    {
                        string terminalDescription = Providers.TerminalData.GetName(PluginEntry.DataModel, (string) rec.TerminalId);
                        terminals.Add((string)rec.TerminalId, terminalDescription);
                        item.SubItems.Add(terminalDescription);
                    }

                    item.SubItems.Add((rec.ReceiptId.ToString()==""?rec.DocumentNo.ToString():rec.ReceiptId.ToString()));

                    item.SubItems.Add(rec.Description);

                    item.Tag = rec.ID;

                    lvCustomerLedger.Add(item);
                }

                lvCustomerLedger.BestFitColumns();
            }
            ((ViewBase)owner.Target).HideProgress();
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
            return new CustomerLedgerPage((TabControl)sender);
		}

		#region ITabPanel Members

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
            customer = (Customer)internalContext;

            if (!PluginOperations.UseCentralCustomer(paramsData.SiteServiceProfile))
            {
                tbCreditLimit.Value = (double)customer.MaxCredit;
            }
            else
            {
                try
                {
                    var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                    var serverCustomer = service.GetCustomer(PluginEntry.DataModel, siteServiceProfile, ((Customer)internalContext).ID, PluginOperations.UseCentralCustomer(paramsData.SiteServiceProfile), true);
                    tbCreditLimit.Value = (double)serverCustomer.MaxCredit;
                }
                catch (Exception)
                {
                    MessageDialog.Show(Resources.ErrorConnectingToSiteService, MessageBoxIcon.Error);
                    return;
                }
            }
            ((ViewBase)owner.Target).ShowProgress((sender1, e1) => LoadDataInner(isRevert, context, true), ((ViewBase)owner.Target).GetLocalizedSearchingText());
		}

		public bool DataIsModified()
		{
            if ((tbCreditLimit.Value != (double)customer.MaxCredit))
            {
                customer.Dirty = true;
                return true;
            }

			return false;
		}

		public bool SaveData()
		{
            customer.MaxCredit = (decimal)tbCreditLimit.Value;

            if (PluginOperations.UseCentralCustomer(paramsData.SiteServiceProfile))
            {
                ISiteServiceService service = null;
                try
                {
                    service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                    service.SetCustomerCreditLimit(PluginEntry.DataModel, siteServiceProfile, customer.ID, customer.MaxCredit, true);
                }
                catch (Exception ex)
                {
                    MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                    return false;
                }
            }

            return true;
		}

		public void GetAuditDescriptors(List<AuditDescriptor> contexts)
		{
		}

		public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
		{
		}

		public void OnClose()
		{
		}

        public void SaveUserInterface()
        {
        }

		#endregion

		private void OnPageScrollPageChanged(object sender, EventArgs e)
		{
            ((ViewBase)owner.Target).ShowProgress((sender1, e1) => LoadDataInner(false, customer.ID, false),
                ((ViewBase)owner.Target).GetLocalizedSearchingText());
		}

        private void cmbStore_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            var items = Providers.StoreData.GetList(PluginEntry.DataModel);

            ((DualDataComboBox) sender).SetData(items, null);
        }

        private void cmbTerminal_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbTerminal_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items;

            if (storeID != null && storeID != "")
            {
                items = new List<DataEntity>(Providers.TerminalData.GetList(PluginEntry.DataModel, storeID));
            }
            else
            {
                items = new List<DataEntity>(Providers.TerminalData.GetList(PluginEntry.DataModel));
            }

            ((DualDataComboBox) sender).SetData(items, null);
        }

        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.LedgerCardDialog(customer.ID, paramsData, siteServiceProfile);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadDataInner(false, customer.ID);
            }
        }

        void lvObjects_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvCustomerLedger.ContextMenuStrip;
            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                btnAddPayment.Text + "...",
                100,
                btnAddPayment_Click)
                {
                    Enabled = btnAddPayment.Enabled,
                
                    Image =  ContextButtons.GetAddButtonImage(),
                    Default = true
                };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                btnSendViaEmail.Text + "...",
                200,
                btnSendViaEmail_Click)
            {
                Enabled = btnSendViaEmail.Enabled
            };

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, new Guid(SettingsGuid), SettingType.UIFieldVisisbility, "");
            if (searchBarSetting.LongUserSetting != "")
            {
                searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar1_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar1.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, new Guid(SettingsGuid), SettingsLevel.User, searchBarSetting);
            }
            ((ViewBase)owner.Target).ShowTimedProgress(searchBar1.GetLocalizedSavingText());
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.Date, "Date", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.Date.AddMonths(-1).AddDays(-1), false, DateTime.Now.Date.AddDays(-1)));
            searchBar1.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Terminal, "Terminal", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Types, "Types", ConditionType.ConditionTypeEnum.Checkboxes,
                Resources.PaymentTypesIntoAccount, true,
                Resources.PaymentTypesCharged, true,
                Resources.PaymentTypesOtherTenders, true));
            searchBar1.AddCondition(new ConditionType(Resources.DocumentNumber, "DocumentNumber", ConditionType.ConditionTypeEnum.Text));

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestData += cmbStore_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += cmbStore_RequestClear;
                    ((DualDataComboBox)args.UnknownControl).SelectedDataChanged += cmbStoreSelctionChanged;
                    break;
                case "Terminal" :
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestData += cmbTerminal_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += cmbTerminal_RequestClear;
                    break;
            }
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= cmbStore_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= cmbStore_RequestClear;
                    ((DualDataComboBox)args.UnknownControl).SelectedDataChanged -= cmbStoreSelctionChanged;
                    break;
                case "Terminal":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= cmbTerminal_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= cmbTerminal_RequestClear;
                    break;
            }
        }

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = true;
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            if (args.TypeKey == "Store")
            {
                args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
            }
            else if (args.TypeKey == "Terminal")
            {
                args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
            }
        }

        private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            if (args.TypeKey == "Store")
                entity = PluginEntry.DataModel.IsHeadOffice
                             ? null
                             : Providers.StoreData.Get(PluginEntry.DataModel, args.Selection);
                
            else if (args.TypeKey == "Terminal")
                entity = Providers.TerminalData.Get(PluginEntry.DataModel, args.Selection, storeID);
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            LoadDataInner(false, customer.ID);
        }

        private void cmbStoreSelctionChanged(object sender, EventArgs e)
        {
            storeID = ((DualDataComboBox) sender).SelectedData != null ? ((DualDataComboBox) sender).SelectedData.ID : null;
        }

        private void btnSendViaEmail_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.ViewController.CurrentView.ShowProgress(ExportCustomerLedger, Resources.ExportingLedgerAndEMail);
        }

        private void ExportCustomerLedger(object sender, EventArgs e)
        {
            try
            {
                var excelFile = FolderItem.GetTempFile((string) customer.ID, ".xlsx");

                var excelService = Services.Interfaces.Services.ExcelService(PluginEntry.DataModel);
                var workBook = excelService.CreateWorkbook(excelFile, "Ledger");
                var sheet = excelService.GetWorksheet(workBook, "Ledger");

                int row = 0;

                var parameters = searchBar1.SearchParameterResults;
                foreach (var result in parameters)
                {
                    switch (result.ParameterKey)
                    {
                        case "Date":
                            if (result.Date.Checked)
                            {
                                excelService.SetCellValue(sheet, row, 0, "Date from:");
                                excelService.SetCellValue(sheet, row, 1, result.Date.Value.Date);
                                row++;
                            }
                            if (result.DateTo.Checked)
                            {
                                excelService.SetCellValue(sheet, row, 0, "Date to:");
                                excelService.SetCellValue(sheet, row, 1, result.DateTo.Value.Date);
                                row++;
                            }
                            break;
                        case "Store":
                        case "Terminal":
                        case "Types":
                        case "DocumentNumber":
                            break;
                    }
                }
                row++; // One empty line

                // Set header row
                var headerTexts = new[]
                    {
                        clmhDate.Text, clmhType.Text, clmhAmount.Text, clmhForPayment.Text,
                        clmhStatus.Text, clmhStore.Text, clmhTerminal.Text, clmhReceiptID.Text, clmDescription.Text
                    };
                var widths = new[]
                    {
                        100, 100, 150, 150,
                        100, 150, 150, 200, 300
                    };
                var woptions = new WorksheeetOptions { ColumnWidths = new Dictionary<int, int>() };

                int column = 0;
                for (column = 0; column < headerTexts.Length; column++)
                {
                    woptions.ColumnWidths[column] = widths[column];

                    var cell = new WorksheetCell
                    {
                        Value = headerTexts[column],
                        Row = row,
                        Column = column,
                        Options =
                        {
                            BackColor = ColorPalette.MustardLight,
                            FormatEnum = CellFormatEnum.Bold
                        }
                    };
                    excelService.SetCell(sheet, cell);
                }

                row++;

                foreach (var rec in custLedgerLs)
                {
                    column = 0;

                    excelService.SetCellValue(sheet, row, column++, rec.PostingDate);
                    excelService.SetCellValue(sheet, row, column++, CustomerLedgerEntries.AsString(rec.EntryType));
                    excelService.SetCellValue(sheet, row, column++, rec.Amount);
                    excelService.SetCellValue(sheet, row, column++, rec.RemainingAmount);
                    excelService.SetCellValue(sheet, row, column++, CustomerLedgerEntries.AsString(rec.Status));
                    excelService.SetCellValue(sheet, row, column++, stores[(string)rec.StoreId]);
                    excelService.SetCellValue(sheet, row, column++, terminals[(string) rec.TerminalId]);
                    excelService.SetCellValue(sheet, row, column++, (rec.ReceiptId.ToString() == ""
                                            ? rec.DocumentNo.ToString()
                                            : rec.ReceiptId.ToString()));
                    excelService.SetCellValue(sheet, row, column++, rec.Description);

                    row++;
                }

                excelService.SetWorksheetOptions(sheet, woptions);
                excelService.Save(workBook, excelFile);

                var message = new EMailMessage
                    {
                        To = customer.Email,
                        Subject = Resources.ExportingLedgerSubject,
                        Body = Resources.ExportingLedgerBody
                    };

                PluginEntry.Framework.ViewController.CurrentView.HideProgress();

                var dlg = new SendTestEMailDialog(message)
                    {
                        IncludeMessageDetails = true,
                        Text = Resources.LedgerEmailCaption
                    };
                if (DialogResult.OK == dlg.ShowDialog())
                {
                    var email = new EMailQueueEntry(message) {BodyIsHTML = true};
                    var attachment = new EMailQueueAttachment(excelFile.AbsolutePath);
                    var attachments = new List<EMailQueueAttachment> {attachment};
                    
                    var siteService = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                    siteService.QueueEMailEntry(PluginEntry.DataModel, siteServiceProfile, email, attachments, PluginOperations.UseCentralCustomer(paramsData.SiteServiceProfile));

                    if (string.IsNullOrEmpty(customer.Email))
                    {
                        if (DialogResult.Yes == MessageDialog.Show(
                                string.Format(Resources.UpdateCustomersEmailAddress, message.To),
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                        {
                            customer.Email = message.To;
                            customer.Dirty = true;
                            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", customer.ID, customer);
                        }
                    }
                }

                excelFile.Wipe();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                PluginEntry.Framework.ViewController.CurrentView.HideProgress();
            }
        }
	}
}