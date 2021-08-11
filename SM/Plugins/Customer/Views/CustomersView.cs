using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Customer.Properties;
using LSOne.ViewPlugins.Customer.Dialogs;

namespace LSOne.ViewPlugins.Customer.Views
{
    public partial class CustomersView : ViewBase
    {
        private static Guid BarSettingID = new Guid("57387DB4-85E3-4618-B384-2672672D6237");
        private static Guid SortSettingID = new Guid("11345818-8E99-4E03-BFFD-039E773D04F2");
        private HashSet<RecordIdentifier> lastSelection;
        private Dictionary<RecordIdentifier, string> groupNameCache;
        private Dictionary<RecordIdentifier, string> taxGroupNameCache;
        List<CustomerListItemAdvanced> customers;
        private Setting searchBarSetting;
        Setting sortSetting;
        private int lastSelectionCount;

        public CustomersView(RecordIdentifier customerID)
            : this()
        {
            lastSelection.Add(customerID);
        }

        public CustomersView()
        {
            InitializeComponent();
            lastSelection = new HashSet<RecordIdentifier>();

            lastSelectionCount = 0;
            groupNameCache = Providers.PriceDiscountGroupData.GetGroupDictionary(PluginEntry.DataModel);
            taxGroupNameCache = Providers.TaxItemData.GetTaxGroupDictionary(PluginEntry.DataModel);
            Providers.CustomerData.RefreshCache(PluginEntry.DataModel);

            Attributes = ViewAttributes.ContextBar
                | ViewAttributes.Close
                | ViewAttributes.Help;

            HeaderText = Resources.Customers;

            lvCustomers.ContextMenuStrip = new ContextMenuStrip();
            lvCustomers.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            btnsEditAddRemove.EditButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CustomerView);
            btnsEditAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);

            lvCustomers.Columns[0].Tag = CustomerSorting.ID;
            lvCustomers.Columns[1].Tag = CustomerSorting.Name;
            lvCustomers.Columns[2].Tag = CustomerSorting.CashCustomer;
            lvCustomers.Columns[3].Tag = CustomerSorting.SalesTaxGroup;
            //lvCustomers.Columns[4].Tag = CustomerSorting.TaxExempt;   // TaxExempt does not exist in CustomerSorting; this column is not sortable anyway
            lvCustomers.Columns[5].Tag = CustomerSorting.PriceGroup;
            lvCustomers.Columns[6].Tag = CustomerSorting.LineDiscountGroup;
            lvCustomers.Columns[7].Tag = CustomerSorting.TotalDiscountGroup;
            lvCustomers.Columns[8].Tag = CustomerSorting.CreditLimit;
            lvCustomers.Columns[9].Tag = CustomerSorting.Blocked;

            searchBar1.BuddyControl = lvCustomers;
            customerDataScroll.PageSize = PluginEntry.DataModel.PageSize;

            customerDataScroll.Reset();

            searchBar1.FocusFirstInput();
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Customer":
                    ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    break;
                case "PriceDiscountGroup":
                    groupNameCache = Providers.PriceDiscountGroupData.GetGroupDictionary(PluginEntry.DataModel);
                    ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    break;
                case "SalesTaxGroup":
                    taxGroupNameCache = Providers.TaxItemData.GetTaxGroupDictionary(PluginEntry.DataModel);
                    ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    break;                
            }            
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.Customers;
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
            sortSetting = PluginEntry.DataModel.Settings.GetSetting(
                PluginEntry.DataModel,
                SortSettingID,
                SettingType.UISetting, lvCustomers.CreateSortSetting(1, true));

            lvCustomers.SortSetting = sortSetting.Value;

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        public override void SaveUserInterface()
        {
            string newSortSetting = lvCustomers.SortSetting;

            if (newSortSetting != sortSetting.Value)
            {
                sortSetting.Value = newSortSetting;
                sortSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, SortSettingID, SettingsLevel.User, sortSetting);
            }
        }

        private void LoadItems()
        {
            string idOrDescription = null;
            bool idOrDescriptionBeginsWith = true;
            RecordIdentifier salesTaxGroupID = null;
            RecordIdentifier priceGroupID = null;
            RecordIdentifier lineDiscountGroupID = null;
            RecordIdentifier totalDiscountGroupID = null;
            RecordIdentifier invoiceCustomerID = null;
            BlockedEnum? isBlocked = null;
            bool? showDeleted = null;
            TaxExemptEnum? taxExempt = null;

            HashSet<RecordIdentifier> tmpLastSelection = lastSelection;
            lvCustomers.ClearRows(); // Triggers selection change which clears the lastSelection

            if (lvCustomers.SortColumn == null)
            {
                lvCustomers.SetSortColumn(lvCustomers.Columns[1], true);
            }

            CustomerSorting sortBy = (CustomerSorting)lvCustomers.SortColumn.Tag;

            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        idOrDescription = result.StringValue;
                        idOrDescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "SalesTaxGroup":
                        salesTaxGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "InvoiceCustomer":
                        invoiceCustomerID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "PriceGroup":
                        priceGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "LineDiscountGroup":
                        lineDiscountGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "TotalDiscountGroup":
                        totalDiscountGroupID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Blocked":
                        switch (result.ComboSelectedIndex)
                        {
                            case 1:
                                isBlocked = BlockedEnum.All;
                                break;
                            case 2:
                                isBlocked = BlockedEnum.Invoice;
                                break;
                            case 3:
                                isBlocked = BlockedEnum.Nothing;
                                break;
                            default:
                                isBlocked = null;
                                break;
                        }
                        break;
                    case "Deleted":
                        showDeleted = result.CheckedValues[0];
                        break;
                    case "TaxExempt":
                        switch (result.ComboSelectedIndex)
                        {
                            case 0:
                                taxExempt = TaxExemptEnum.No;
                                break;
                            case 1:
                                taxExempt = TaxExemptEnum.Yes;
                                break;
                            case 2:
                                taxExempt = TaxExemptEnum.EU;
                                break;
                            default:
                                taxExempt = null;
                                break;
                        }
                        break;
                }
            }

            int itemCount;

            customers = Providers.CustomerData.AdvancedSearch(PluginEntry.DataModel,
                                                                customerDataScroll.StartRecord,
                                                                customerDataScroll.EndRecord + 1,
                                                                out itemCount,
                                                                sortBy,
                                                                !lvCustomers.SortedAscending,
                                                                idOrDescription,
                                                                idOrDescriptionBeginsWith,
                                                                salesTaxGroupID,
                                                                priceGroupID,
                                                                lineDiscountGroupID,
                                                                totalDiscountGroupID,
                                                                invoiceCustomerID,
                                                                isBlocked,
                                                                showDeleted,
                                                                taxExempt
                                                                );

            customerDataScroll.RefreshState(customers, itemCount);

            Row row;
            Controls.Cells.CheckBoxCell cell;

            foreach (CustomerListItemAdvanced customer in customers)
            {
                row = new Row();
                row.AddText((string)customer.ID);
                row.AddText(customer.Text);

                cell = new Controls.Cells.CheckBoxCell(customer.CashCustomer);
                cell.Enabled = false;
                cell.CheckBoxAlignment = LSOne.Controls.Cells.CheckBoxCell.CheckBoxAlignmentEnum.Center;
                row.AddCell(cell);

                cell = new Controls.Cells.CheckBoxCell(customer.TaxExempt == TaxExemptEnum.Yes);
                cell.Enabled = false;
                cell.CheckBoxAlignment = LSOne.Controls.Cells.CheckBoxCell.CheckBoxAlignmentEnum.Center;
                row.AddCell(cell);

                // SalesTaxGroupID
                if (customer.SalesTaxGroupID != "" && taxGroupNameCache.ContainsKey(customer.SalesTaxGroupID))
                {
                    customer.SalesTaxGroupName = taxGroupNameCache[customer.SalesTaxGroupID];
                    row.AddText(customer.SalesTaxGroupName);
                }
                else
                {
                    row.AddText("");
                }
                // PriceGroupID
                RecordIdentifier groupID = new RecordIdentifier(1, 0, customer.PriceGroupID);
                if (customer.PriceGroupID != "" && groupNameCache.ContainsKey(groupID))
                {
                    customer.PriceGroupName = groupNameCache[groupID];
                    row.AddText(customer.PriceGroupName);
                }
                else
                {
                    row.AddText("");
                }
                // LineDiscountGroupID
                groupID = new RecordIdentifier(1, 1, customer.LineDiscountGroupID);
                if (customer.LineDiscountGroupID != "" && groupNameCache.ContainsKey(groupID))
                {
                    customer.LineDiscountGroupName = groupNameCache[groupID];
                    row.AddText(customer.LineDiscountGroupName);
                }
                else
                {
                    row.AddText("");
                }
                // TotalDiscountGroupID
                groupID = new RecordIdentifier(1, 3, customer.TotalDiscountGroupID);
                if (customer.TotalDiscountGroupID != "" && groupNameCache.ContainsKey(groupID))
                {
                    customer.TotalDiscountGroupName = groupNameCache[groupID];
                    row.AddText(customer.TotalDiscountGroupName);
                }
                else
                {
                    row.AddText("");
                }

                row.AddText(customer.CreditLimit.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)));

                switch (customer.Blocked)
                {
                    case BlockedEnum.All:
                        row.AddText(Resources.Blocked);
                        row.BackColor = ColorPalette.RedLight;
                        break;
                    case BlockedEnum.Invoice:
                        row.AddText(Resources.LimitedToInvoices);
                        row.BackColor = ColorPalette.MustardLight;
                        break;
                    case BlockedEnum.Nothing:
                        row.AddText(Resources.NotBlocked);
                        break;
                }

                row.Tag = customer;
                lvCustomers.AddRow(row);

                if (tmpLastSelection.Contains(((CustomerListItemAdvanced)row.Tag).ID))
                {
                    lvCustomers.Selection.AddRows(lvCustomers.RowCount - 1, lvCustomers.RowCount - 1);
                }
            }

            lvCustomers_SelectionChanged(this, EventArgs.Empty);

            lvCustomers.AutoSizeColumns(true);

            HideProgress();
        }

        private void lvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            lastSelection = new HashSet<RecordIdentifier>();
            for (int i = 0; i < lvCustomers.Selection.Count; i++)
            {
                lastSelection.Add(((CustomerListItemAdvanced)lvCustomers.Selection[i].Tag).ID);
            }
            bool Deleted = CheckDeleted();
            bool hasViewPermission = PluginEntry.DataModel.HasPermission(Permission.CustomerView);
            bool hasEditPermission = PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);

            btnsEditAddRemove.EditButtonEnabled = (lvCustomers.Selection.Count == 1) && hasViewPermission && !Deleted;
            btnsEditAddRemove.RemoveButtonEnabled = (lvCustomers.Selection.Count >= 1) && hasEditPermission && !Deleted;

            if ((lastSelectionCount == 0 && lvCustomers.Selection.Count != 0) || (lastSelectionCount != 0 && lvCustomers.Selection.Count == 0))
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }
            lastSelectionCount = lvCustomers.Selection.Count;
        }

        private bool CheckDeleted()
        {
            for (int i = 0; i < lvCustomers.Selection.Count; i++)
            {
                if (((CustomerListItemAdvanced)lvCustomers.Selection[i].Tag).Deleted)
                {
                    return true;
                }
            }
            return false;
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvCustomers.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditString,
                    100,
                    btnsEditAddRemove_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.AddCustomer,
                   200,
                   btnsEditAddRemove_AddButtonClicked)
            {
                Enabled = btnsEditAddRemove.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.DeleteCustomer,
                    300,
                    btnsEditAddRemove_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemove.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            if (PluginEntry.DataModel.HasPermission(Permission.CustomerEdit))
            {
                menu.Items.Add(new ExtendedMenuItem("-", 2000));

                item = new ExtendedMenuItem(
                    Resources.Blocking,
                    2010,
                    ActionsCustomerBlocking_Handler);

                menu.Items.Add(item);                

                item = new ExtendedMenuItem(
                    Resources.TaxSettings,
                    2020,
                    ActionsCustomerTaxInformation_Handler);

                menu.Items.Add(item);
            }

            PluginEntry.Framework.ContextMenuNotify("CustomersList", lvCustomers.ContextMenuStrip, lvCustomers);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewCustomer(this, EventArgs.Empty);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowCustomer(((CustomerListItemAdvanced)lvCustomers.Selection[0].Tag).ID, customers);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvCustomers.Selection.Count == 1)
            {
                PluginOperations.DeleteCustomer(((CustomerListItemAdvanced)lvCustomers.Selection[0].Tag).ID);
            }
            else
            {
                List<RecordIdentifier> selectedCustomers = CreateCustomerList();
                PluginOperations.DeleteCustomers(selectedCustomers);
            }
        }

        private List<RecordIdentifier> CreateCustomerList()
        {
            List<RecordIdentifier> selectedCustomers = new List<RecordIdentifier>();

            for (int i = 0; i < lvCustomers.Selection.Count; i++)
            {
                selectedCustomers.Add(((CustomerListItemAdvanced)lvCustomers.Selection[i].Tag).ID);
            }

            return selectedCustomers;
        }

        private void lvCustomers_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        public override int GetListSelectionCount()
        {
            return lvCustomers.Selection.Count;
        }

        public override List<IDataEntity> GetListSelection()
        {
            var res = new List<IDataEntity>();
            for (int i = 0; i < lvCustomers.Selection.Count; i++)
            {
                res.Add((CustomerListItemAdvanced)lvCustomers.Selection[i].Tag);
            }
            return res;
        }

        private void lvCustomers_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvCustomers.SortColumn == args.Column)
            {
                lvCustomers.SetSortColumn(args.Column, !lvCustomers.SortedAscending);
            }
            else
            {
                lvCustomers.SetSortColumn(args.Column, true);
            }

            customerDataScroll.Reset();

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.Customer.Views.CustomersView.Actions")
            {
                // Only if the user has edit permissions
                if (PluginEntry.DataModel.HasPermission(Permission.CustomerEdit))
                {
                    bool enabled = arguments.View.GetListSelectionCount() > 0;
                    arguments.Add(new ContextBarItem(Resources.Blocking, ActionsCustomerBlocking_Handler) { Enabled = enabled }, 1040);
                    arguments.Add(new ContextBarItem(Resources.TaxSettings, ActionsCustomerTaxInformation_Handler) { Enabled = enabled }, 1060);

                }
            }
        }

        private void ActionsCustomerBlocking_Handler(object sender, ContextBarClickEventArguments args)
        {
            if (!CheckDeleted())
            {
                List<CustomerListItemAdvanced> selectedCustomers = (PluginEntry.Framework.ViewController.CurrentView.GetListSelection()).Cast<CustomerListItemAdvanced>().ToList();
                var dialog = new CustomerBlockingDialog(selectedCustomers);
                dialog.ShowDialog();
            }
            else
            {
                MessageDialog.Show(Resources.CantChangeBlockingStatusDeletedCustomer);
            }
        }

        private void ActionsCustomerBlocking_Handler(object sender, EventArgs args)
        {
            if (!CheckDeleted())
            {
                List<CustomerListItemAdvanced> selectedCustomers = (PluginEntry.Framework.ViewController.CurrentView.GetListSelection()).Cast<CustomerListItemAdvanced>().ToList();
                var dialog = new CustomerBlockingDialog(selectedCustomers);
                dialog.ShowDialog();
            }
            else
            {
                MessageDialog.Show(Resources.CantChangeBlockingStatusDeletedCustomer);
            }
        }

        private void ActionsCustomerTaxInformation_Handler(object sender, EventArgs args)
        {
            if (!CheckDeleted())
            {
                List<CustomerListItemAdvanced> selectedCustomers = (PluginEntry.Framework.ViewController.CurrentView.GetListSelection()).Cast<CustomerListItemAdvanced>().ToList();
                var dialog = new CustomerTaxExemptDialog(selectedCustomers);
                dialog.ShowDialog();
            }
            else
            {
                MessageDialog.Show(Resources.CantChangeTaxSettingsDeletedCustomer);
            }
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            List<object> blockedList = new List<object>();
            blockedList.Add(Resources.AllStatuses);
            blockedList.Add(Resources.Blocked);
            blockedList.Add(Resources.LimitedToInvoices);
            blockedList.Add(Resources.NotBlocked);

            List<object> taxExemptList = new List<object>();
            taxExemptList.Add(Resources.No);
            taxExemptList.Add(Resources.Yes);
            
            if(PluginEntry.Framework.FindImplementor(null, "SAPBusinessOne", null) != null)
            {
                taxExemptList.Add(Resources.EU);
            }

            searchBar1.AddCondition(new ConditionType(Resources.IDOrDescription, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.InvoiceCustomer, "InvoiceCustomer", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.SalesTaxGroup, "SalesTaxGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.PriceGroup, "PriceGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.LineDiscountGroup, "LineDiscountGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.TotalDiscountGroup, "TotalDiscountGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.BlockingStatus, "Blocked", ConditionType.ConditionTypeEnum.ComboBox, blockedList, 0, 0, false));
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_TaxExempt, "TaxExempt", ConditionType.ConditionTypeEnum.ComboBox, taxExemptList, 0, 0, false));
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_Deleted, "Deleted", ConditionType.ConditionTypeEnum.Checkboxes, Resources.ShowDeleted, true));            

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            lastSelection = new HashSet<RecordIdentifier>();
            customerDataScroll.Reset();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar1_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar1.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }

            ShowTimedProgress(searchBar1.GetLocalizedSavingText());
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "SalesTaxGroup":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += SalesTaxGroup_DropDown;
                    break;
                case "InvoiceCustomer":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += InvoiceCustomer_DropDown;
                    break;
                case "PriceGroup":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += PriceGroup_DropDown;
                    break;
                case "LineDiscountGroup":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += LineDiscountGroup_DropDown;
                    break;
                case "TotalDiscountGroup":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += TotalDiscountGroup_DropDown;
                    break;
            }
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "SalesTaxGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= SalesTaxGroup_DropDown;
                    break;
                case "InvoiceCustomer":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= InvoiceCustomer_DropDown;
                    break;
                case "PriceGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= PriceGroup_DropDown;
                    break;
                case "LineDiscountGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= LineDiscountGroup_DropDown;
                    break;
                case "TotalDiscountGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= TotalDiscountGroup_DropDown;
                    break;
            }
        }

        private void InvoiceCustomer_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            ((DualDataComboBox)sender).SkipIDColumn = true;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.Customers, "", textInitallyHighlighted);
        }

        private void SalesTaxGroup_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            ((DualDataComboBox)sender).SkipIDColumn = true;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false,initialSearchText, SearchTypeEnum.SalesTaxGroup, "", textInitallyHighlighted);
        }

        private void PriceGroup_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            ((DualDataComboBox)sender).SkipIDColumn = true;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.PriceGroup, "", textInitallyHighlighted);
        }

        private void LineDiscountGroup_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            ((DualDataComboBox)sender).SkipIDColumn = true;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.LineDiscountGroup, "", textInitallyHighlighted);
        }

        private void TotalDiscountGroup_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            ((DualDataComboBox)sender).SkipIDColumn = true;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.TotalDiscountGroup, "", textInitallyHighlighted);
        }

        private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "SalesTaxGroup":
                    entity = Providers.SalesTaxGroupData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "InvoiceCustomer":
                    entity = Providers.CustomerData.Get(PluginEntry.DataModel, args.Selection, UsageIntentEnum.Minimal);
                    break;
                case "PriceGroup":
                    entity = Providers.PriceDiscountGroupData.GetGroupID(PluginEntry.DataModel, args.Selection);
                    break;
                case "LineDiscountGroup":
                    entity = Providers.PriceDiscountGroupData.GetGroupID(PluginEntry.DataModel, args.Selection);
                    break;
                case "TotalDiscountGroup":
                    entity = Providers.PriceDiscountGroupData.GetGroupID(PluginEntry.DataModel, args.Selection);
                    break;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "SalesTaxGroup":
                case "PriceGroup":
                case "LineDiscountGroup":
                case "TotalDiscountGroup":
                case "InvoiceCustomer":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "SalesTaxGroup":
                case "PriceGroup":
                case "LineDiscountGroup":
                case "TotalDiscountGroup":
                case "InvoiceCustomer":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }
    }
}