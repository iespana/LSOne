using System;
using LSOne.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.CustomerOrders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.CustomerOrders.Properties;

namespace LSOne.ViewPlugins.CustomerOrders.Views
{
    public partial class CustomerOrdersView : ViewBase
    {
        private Setting searchBarSetting;
        private Setting sortSetting;
        private HashSet<RecordIdentifier> lastSelection;
        private int lastSelectionCount;
        private List<CustomerOrderAdditionalConfigurations> configurations;
        private List<DataEntity> stores;
        private List<Customer> customerList;
        private int numerOfRowsToReturn;

        private DataEntity customerSelectedItem;
        private DataEntity deliveryLocationSelectedItem;
        
        private static Guid BarSettingID = new Guid("97183066-28C6-44A8-8C79-A3F799080676");
        private static Guid SortSettingID = new Guid("DC3CFDA5-7B04-4F76-AB7F-B4296C041EE5");

        private CustomerOrderType orderType;

        private CustomerOrdersView()
        {
            InitializeComponent();

            lastSelection = new HashSet<RecordIdentifier>();

            lastSelectionCount = 0;
            numerOfRowsToReturn = PluginEntry.DataModel.PageSize;

            configurations = Providers.CustomerOrderAdditionalConfigData.GetList(PluginEntry.DataModel);
            stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            customerList = Providers.CustomerData.GetAllCustomers(PluginEntry.DataModel, UsageIntentEnum.Normal);

            Attributes = ViewAttributes.ContextBar
                         | ViewAttributes.Close
                         | ViewAttributes.Help;

            HeaderText = Resources.CustomerOrders;

            lvCustomerOrders.ContextMenuStrip = new ContextMenuStrip();
            lvCustomerOrders.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            btnsEditAddRemove.EditButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders);
            btnsEditAddRemove.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders);

            colSource.Tag = CustomerOrderSorting.Source;
            colDelivery.Tag = CustomerOrderSorting.Delivery;
            colComment.Tag = CustomerOrderSorting.Comment;
            colCreatedAt.Tag = CustomerOrderSorting.CreatedAt;
            colCreatedDate.Tag = CustomerOrderSorting.CreatedDate;
            colExpires.Tag = CustomerOrderSorting.Expires;
            colDeliveryLocation.Tag = CustomerOrderSorting.DeliveryLocation;
            colReference.Tag = CustomerOrderSorting.Reference;
            colCustomer.Tag = CustomerOrderSorting.Customer;

            lvCustomerOrders.SetSortColumn(colReference, true);

            searchBar1.BuddyControl = lvCustomerOrders;

            searchBar1.FocusFirstInput();
        }

        public CustomerOrdersView(RecordIdentifier reference)
            : this()
        {
            lastSelection.Add(reference);
        }

        public CustomerOrdersView(CustomerOrderType orderType)
            : this()
        {
            this.orderType = orderType;

            HeaderText = orderType == CustomerOrderType.CustomerOrder ? Resources.CustomerOrders : Resources.Quotes;
        }

        protected override string LogicalContextName
        {
            get { return orderType == CustomerOrderType.CustomerOrder ? Resources.CustomerOrders : Resources.Quotes; }
        }

        public override RecordIdentifier ID
        {
            get { return RecordIdentifier.Empty; }
        }

        protected override void LoadData(bool isRevert)
        {
            sortSetting = PluginEntry.DataModel.Settings.GetSetting(
                PluginEntry.DataModel,
                SortSettingID,
                SettingType.UISetting, lvCustomerOrders.CreateSortSetting(1, true));

            lvCustomerOrders.SortSetting = sortSetting.Value;

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void LoadItems()
        {
            HashSet<RecordIdentifier> tmpLastSelection = lastSelection;
            List<CustomerOrder> customerOrders;

            lvCustomerOrders.ClearRows();

            if (lvCustomerOrders.SortColumn == null)
            {
                lvCustomerOrders.SetSortColumn(colReference, true);
            }

            int sortColumnIndex = lvCustomerOrders.Columns.IndexOf(lvCustomerOrders.SortColumn);

            CustomerOrderSorting sortBy = (CustomerOrderSorting) sortColumnIndex;

            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            CustomerOrderSearch searchCriteria = new CustomerOrderSearch();
            searchCriteria.Status = (int)CustomerOrderStatus.Open;
            searchCriteria.OrderType = orderType;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Reference":
                        searchCriteria.Reference = result.StringValue;
                        searchCriteria.ReferenceBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Comment":
                        searchCriteria.Comment = result.StringValue;
                        break;
                    case "Source":
                        searchCriteria.Source = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Delivery":
                        searchCriteria.Delivery = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Expired":
                        //0 = Yes --- 1 = No
                        if (result.CheckedValues[0] && result.CheckedValues[1])
                        {
                            searchCriteria.Expired = null;
                        }
                        else if (result.CheckedValues[0] && !result.CheckedValues[1])
                        {
                            searchCriteria.Expired = true;
                        }
                        else if (!result.CheckedValues[0] && result.CheckedValues[1])
                        {
                            searchCriteria.Expired = false;
                        }

                        break;
                    case "Status":
                        if (result.CheckedValues[0] && result.CheckedValues[1] && result.CheckedValues[2] && result.CheckedValues[3])
                        {
                            searchCriteria.Status = -1;
                            break;
                        }
                        CustomerOrderStatus status = 0;
                        status |= result.CheckedValues[0] ? CustomerOrderStatus.Open : 0;
                        status |= result.CheckedValues[1] ? CustomerOrderStatus.Closed : 0;
                        status |= result.CheckedValues[2] ? CustomerOrderStatus.Cancelled : 0;
                        status |= result.CheckedValues[3] ? CustomerOrderStatus.Printed : 0;
                        status |= result.CheckedValues[3] ? CustomerOrderStatus.Ready : 0;
                        status |= result.CheckedValues[3] ? CustomerOrderStatus.Delivered : 0;

                        searchCriteria.Status = (int) status;
                        break;
                }
            }

            searchCriteria.CustomerID = customerSelectedItem != null ? customerSelectedItem.ID : RecordIdentifier.Empty;
            searchCriteria.DeliveryLocation = deliveryLocationSelectedItem != null ? deliveryLocationSelectedItem.ID : RecordIdentifier.Empty;
            searchCriteria.RetrieveOrderXML = false;

            int orderCount;

            ISiteServiceService service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            Parameters paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);

            SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            customerOrders = service.CustomerOrderSearch(PluginEntry.DataModel, siteServiceProfile,
                out orderCount,
                numerOfRowsToReturn,
                searchCriteria
                );

            Row row;
            DataEntity store = null;
            CustomerOrderAdditionalConfigurations config = null;
            Customer customer = null;

            if (orderCount > numerOfRowsToReturn)
            {
                lblMsg.Text = Resources.NumberOfRowsReturned.Replace("#1", Conversion.ToStr(numerOfRowsToReturn)).Replace("#2", Conversion.ToStr(orderCount));
            }

            foreach (CustomerOrder order in customerOrders)
            {
                row = new Row();

                row.AddText((string) order.Reference);

                customer = customerList.FirstOrDefault(f => f.ID == order.CustomerID);
                row.AddText(customer == null ? "" : customer.Text);

                store = stores.FirstOrDefault(f => f.ID == order.DeliveryLocation);
                row.AddText(store == null ? "" : store.Text);

                config = configurations.FirstOrDefault(f => f.ID == order.Source);
                row.AddText(config == null ? "" : config.Text);

                config = configurations.FirstOrDefault(f => f.ID == order.Delivery);
                row.AddText(config == null ? "" : config.Text);

                row.AddCell(new DateTimeCell(order.ExpiryDate.ToShortDateString(), order.ExpiryDate.DateTime));

                row.AddText(order.Comment);

                row.AddText(order.StatusText());

                if (order.DeliveryLocation == order.StoreID)
                {
                    row.AddText(store == null ? "" : store.Text);
                }
                else
                {
                    store = stores.FirstOrDefault(f => f.ID == order.StoreID);
                    row.AddText(store == null ? "" : store.Text);
                }

                row.AddCell(new DateTimeCell(order.CreatedDate.ToShortDateString(), order.CreatedDate));

                if (order.ExpiryDate.DateTime.Date < DateTime.Now.Date)
                {
                    row.BackColor = ColorPalette.RedLight;
                }

                if (order.Status == CustomerOrderStatus.Cancelled)
                {
                    row.BackColor = ColorPalette.GrayDark;
                }

                if (order.Status == CustomerOrderStatus.Closed)
                {
                    row.BackColor = ColorPalette.RedDark;
                }

                row.Tag = order;
                lvCustomerOrders.AddRow(row);

                if (tmpLastSelection.Contains(((CustomerOrder) row.Tag).ID))
                {
                    lvCustomerOrders.Selection.AddRows(lvCustomerOrders.RowCount - 1, lvCustomerOrders.RowCount - 1);
                }
            }

            lvCustomerOrders_SelectionChanged(this, EventArgs.Empty);

            lvCustomerOrders.AutoSizeColumns(true);

            HideProgress();
        }

        private void lvCustomerOrders_SelectionChanged(object sender, EventArgs e)
        {
            lastSelection = new HashSet<RecordIdentifier>();
            lastSelectionCount = 0;

            for (int i = 0; i < lvCustomerOrders.Selection.Count; i++)
            {
                lastSelection.Add(((CustomerOrder) lvCustomerOrders.Selection[i].Tag).ID);
            }

            bool hasViewPermission = PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders);
            bool hasEditPermission = PluginEntry.DataModel.HasPermission(Permission.ManageCustomerOrders);

            btnsEditAddRemove.EditButtonEnabled = (lvCustomerOrders.Selection.Count >= 1) && hasViewPermission;
            btnsEditAddRemove.RemoveButtonEnabled = false; // (lvCustomerOrders.Selection.Count >= 1) && hasEditPermission;

            if ((lastSelectionCount == 0 && lvCustomerOrders.Selection.Count != 0) || (lastSelectionCount != 0 && lvCustomerOrders.Selection.Count == 0))
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }
            lastSelectionCount = lvCustomerOrders.Selection.Count;
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvCustomerOrders.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                Resources.EditString,
                100,
                btnsEditAddRemove_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Resources.DeleteString,
                300,
                btnsEditAddRemove_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemove.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("CustomerOrderMenuList", lvCustomerOrders.ContextMenuStrip, this);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            if (lvCustomerOrders.Selection.Count > 0)
            {
                List<CustomerOrder> toEdit = new List<CustomerOrder>();

                for (int i = 0; i < lvCustomerOrders.Selection.Count; i++)
                {
                    CustomerOrder iOrder = (CustomerOrder) lvCustomerOrders.Selection[i].Tag;
                    toEdit.Add(iOrder);
                }

                PluginOperations.EditCustomerOrdersDetails(toEdit);
            }
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvCustomerOrders.Selection.Count > 0)
            {
                List<CustomerOrder> toDelete = new List<CustomerOrder>();

                for (int i = 0; i < lvCustomerOrders.Selection.Count; i++)
                {
                    CustomerOrder iOrder = (CustomerOrder) lvCustomerOrders.Selection[i].Tag;
                    toDelete.Add(iOrder);
                }

                // Deleting customer orders that are in process needs to be confirmed but Quotes can be deleted at any time
                if (orderType == CustomerOrderType.CustomerOrder && (toDelete.Count(c => c.Status != CustomerOrderStatus.Cancelled && c.Status != CustomerOrderStatus.Closed) > 0))
                {
                    if (QuestionDialog.Show(Resources.OrderIsBeingProcessedDoYouWantToContinue, Resources.DeleteCustomerOrder) == DialogResult.No)
                    {
                        return;
                    }
                }

                PluginOperations.DeleteCustomerOrders(toDelete);
            }
        }

        #region Search bar

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            lastSelection = new HashSet<RecordIdentifier>();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Source":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.UnknownControl.Name = "Source";
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox) args.UnknownControl).SkipIDColumn = true;
                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox) args.UnknownControl).RequestData += new EventHandler(Source_RequestData);
                    break;
                case "Delivery":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.UnknownControl.Name = "Delivery";
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox) args.UnknownControl).SkipIDColumn = true;
                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox) args.UnknownControl).RequestData += new EventHandler(Delivery_RequestData);
                    break;
                case "DeliveryLocation":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox) args.UnknownControl).SkipIDColumn = true;
                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox) args.UnknownControl).Tag = args.TypeKey;
                    ((DualDataComboBox) args.UnknownControl).DropDown += DeliveryLocation_DropDown;
                    ((DualDataComboBox) args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;
                    ((DualDataComboBox) args.UnknownControl).SelectedDataChanged += DeliveryLocation_SelectionChanged;
                    break;
                case "Customer":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.UnknownControl.Name = "Customer";
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox) args.UnknownControl).SkipIDColumn = true;
                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity("", "");
                    ((DualDataComboBox) args.UnknownControl).DropDown += Customer_DropDown;
                    ((DualDataComboBox) args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;
                    ((DualDataComboBox) args.UnknownControl).SelectedDataChanged += Customer_SelectionChanged;
                    break;
            }
        }

        private List<DataEntity> GetConfigurationListForSearch(ConfigurationType configType)
        {
            return configurations.Where(w => w.AdditionalType == configType).Select(item => new DataEntity(item.ID, item.Text)).ToList();
        }

        private void Delivery_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;
            ((DualDataComboBox) sender).SetData(GetConfigurationListForSearch(ConfigurationType.Delivery), null);
        }

        private void Source_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;
            ((DualDataComboBox) sender).SetData(GetConfigurationListForSearch(ConfigurationType.Source), null);
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Source":
                    ((DualDataComboBox) args.UnknownControl).RequestData -= Source_RequestData;
                    break;
                case "Delivery":
                    ((DualDataComboBox) args.UnknownControl).RequestData -= Delivery_RequestData;
                    break;
                case "DeliveryLocation":
                    ((DualDataComboBox) args.UnknownControl).DropDown -= DeliveryLocation_DropDown;
                    ((DualDataComboBox) args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    ((DualDataComboBox) args.UnknownControl).SelectedDataChanged -= DeliveryLocation_SelectionChanged;
                    deliveryLocationSelectedItem = null;
                    break;
                case "Customer":
                    ((DualDataComboBox) args.UnknownControl).DropDown -= Customer_DropDown;
                    ((DualDataComboBox) args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    ((DualDataComboBox) args.UnknownControl).SelectedDataChanged -= Customer_SelectionChanged;
                    customerSelectedItem = null;
                    break;
            }
        }

        private void DeliveryLocation_SelectionChanged(object sender, EventArgs args)
        {
            deliveryLocationSelectedItem = (DataEntity) ((DualDataComboBox) sender).SelectedData;
        }

        private void Customer_SelectionChanged(object sender, EventArgs args)
        {
            customerSelectedItem = (DataEntity) ((DualDataComboBox) sender).SelectedData;
        }

        private void DualDataComboBox_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SelectedData = new DataEntity("", "");
        }

        private void Customer_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            ((DualDataComboBox) sender).SkipIDColumn = false;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity) ((DualDataComboBox) sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }

            var panel = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.Customers, "", textInitallyHighlighted);
            panel.SetSearchHandler(SearchDelivery, e.DisplayText);
            e.ControlToEmbed = panel;
        }

        private void DeliveryLocation_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            ((DualDataComboBox) sender).SkipIDColumn = true;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity) ((DualDataComboBox) sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }

            var panel = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.Custom, "", textInitallyHighlighted);
            panel.SetSearchHandler(SearchDeliveryLocation, e.DisplayText);
            e.ControlToEmbed = panel;

        }

        private List<DataEntity> SearchDeliveryLocation(object sender, SingleSearchArgs args)
        {
            if (string.IsNullOrEmpty(args.SearchText))
            {
                return stores;
            }

            if (args.SearchText != "" && args.BeginsWith)
            {
                return stores.Where(p => p.Text.StartsWith(args.SearchText, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }
            if (args.SearchText != "" && !args.BeginsWith)
            {
                return stores.Where(p => p.Text.IndexOf(args.SearchText, StringComparison.InvariantCultureIgnoreCase) > 0).ToList();
            }

            return stores;
        }

        private List<DataEntity> SearchDelivery(object sender, SingleSearchArgs args)
        {
            if (string.IsNullOrEmpty(args.SearchText))
            {
                List<CustomerOrderAdditionalConfigurations> delivery = configurations.Where(w => w.AdditionalType == ConfigurationType.Delivery).ToList();

                List<DataEntity> returnList = new List<DataEntity>();
                returnList.AddRange(delivery.Select(configuration => new DataEntity(configuration.ID, configuration.Text)));

                return returnList;
            }

            return new List<DataEntity>();
        }

        private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "Customer":
                    entity = Providers.RetailGroupData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Source":
                    entity = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Delivery":
                    entity = Providers.SpecialGroupData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "DeliveryLocation":
                    entity = Providers.VendorData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "TaxGroup":
                    entity = Providers.ItemSalesTaxGroupData.Get(PluginEntry.DataModel, args.Selection);
                    break;
            }
            ((DualDataComboBox) args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.ReferenceNumber, "Reference", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.Customer, "Customer", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.Checkboxes,
                Resources.Open, true,
                Resources.Closed, false,
                Resources.Cancelled, false,
                Resources.Other, true));
            searchBar1.AddCondition(new ConditionType(Resources.Source, "Source", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Delivery, "Delivery", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.DeliveryLocation, "DeliveryLocation", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(orderType == CustomerOrderType.CustomerOrder ? Resources.ExpiredOrders : Resources.ExpiredQuotes, "Expired", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Yes, false, Resources.No, false));
            searchBar1.AddCondition(new ConditionType(Resources.Comment, "Comment", ConditionType.ConditionTypeEnum.Text));

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

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Customer":
                case "Source":
                case "Delivery":
                case "DeliveryLocation":
                    args.HasSelection = ((DualDataComboBox) args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox) args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
            }
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Customer":
                case "Source":
                case "Delivery":
                case "DeliveryLocation":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        #endregion

        /// <summary>
        /// View should overload to listen to change broadcasts (NotifyDataChanged())
        /// </summary>
        /// <param name="changeAction">Enum that tells you the type of change</param><param name="objectName">Tells you what changed , f.x. "Store"</param><param name="changeIdentifier">The ID of the changed object</param><param name="param">Extra information</param>
        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "CustomerOrders":
                    if (changeAction == DataEntityChangeType.MultiDelete || changeAction == DataEntityChangeType.MultiAdd)
                    {
                        ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    }
                    break;
                default:
                    ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    break;
            }
        }

        protected override HelpSettings GetOnlineHelpSettings()
        {
            return orderType == CustomerOrderType.CustomerOrder ? new HelpSettings(this.Name) : new HelpSettings("QuotesView");
        }

        private void lvCustomerOrders_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}