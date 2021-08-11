using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Panels;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    /// <summary>
    /// A dialog that displays a list of customer orders / quotes that can be recalled or managed in the POS
    /// </summary>
    public partial class RecallCustomerOrder : TouchBaseForm
    {
        private Control activePanel;
        private IConnectionManager dlgEntry;
        private ISettings settings;
        private IRetailTransaction retailTransaction;

        private Point panelLocation;
        private Size panelSize;
        
        private SearchPanel searchPanel;
        
        private CustomerOrderType orderType;

        private CustomerOrderSearch searchCriteria;
        private bool searchCriteriaApplied;

        private bool displayFilters;
        private bool myLocationFilter;
        private bool displayActions;

        /// <summary>
        /// the customer order that was selected in the dialog
        /// </summary>
        public RetailTransaction SelectedTransaction;
        /// <summary>
        /// The customer order that was selected in the dialog
        /// </summary>
        public CustomerOrder SelectedCustomerOrder; 

        private enum Buttons
        {
            Select,
            Cancel,
            Search,
            ClearSearch,
            Filters,
            Filter_Closed,
            Filter_Open,
            Filter_Ready,
            Filter_Delivered,
            Filter_PickingList,
            Filter_MyLocation,
            PickingList,
            ReadyForDelivery,
            Delivered,
            Multiselection,
            SelectAll,
            Actions,
            PrintInformation,
            CreateSale
        }

        private enum ButtonGroup
        {
            SearchAndManage
        }

        /// <summary>
        /// The constructor tells the dialog which type of customer order is to be displayed
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">The type of customer order to be displayed</param>
        public RecallCustomerOrder(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType)
        {
            InitializeComponent();

            dlgEntry = entry;
            settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            this.retailTransaction = retailTransaction;
            
            this.orderType = orderType;

            displayFilters = false;
            displayActions = false;
            myLocationFilter = false;
            searchPanel = null;
            searchCriteria = new CustomerOrderSearch();
            
            InitializeButtons(ButtonGroup.SearchAndManage);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data. </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            banner.BannerText = orderType == CustomerOrderType.CustomerOrder ? Resources.RecallCustomerOrderHeaderText : Resources.RecallQuoteHeaderText;
            
            //Set the size of the form the same as the main form
            this.Width = settings.MainFormInfo.MainWindowWidth;
            this.Height = settings.MainFormInfo.MainWindowHeight;

            this.Top = settings.MainFormInfo.MainWindowTop;
            this.Left = settings.MainFormInfo.MainWindowLeft;

            panelLocation = new Point(1, 60);
            panelSize = new Size(this.Width - panelButtons.Width - panelButtons.Margin.Left - panelButtons.Margin.Right, this.Height - banner.Height - 15);

            AttachSearchPanel();
        }

        private void AttachSearchPanel()
        {
            if (searchPanel != null)
            {
                
            }
            else
            {
                searchPanel = new SearchPanel(dlgEntry, orderType);
            }

            searchPanel.Location = panelLocation;
            searchPanel.Size = panelSize;

            Controls.Add(searchPanel);

            activePanel = searchPanel;

            SetActivePanelVisible();
        }

        private void InitializeButtons(ButtonGroup group)
        {
            panelButtons.Clear();
            if (group == ButtonGroup.SearchAndManage)
            {
                panelButtons.AddButton(Resources.Search, Buttons.Search, "");
                panelButtons.AddButton(Resources.ClearSearch, Buttons.ClearSearch, Conversion.ToStr((int)Buttons.ClearSearch));
                panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), searchCriteriaApplied);

                if (!displayFilters)
                {
                    panelButtons.AddButton(Resources.Filters, Buttons.Filters, "", TouchButtonType.Action, DockEnum.DockNone, Resources.white_line_arrow_down_16, ImageAlignment.Left);
                }

                if (displayFilters)
                {

                    panelButtons.AddButton(Resources.Filters, Buttons.Filters, "", TouchButtonType.Action, DockEnum.DockNone, Resources.white_line_arrow_up_16, ImageAlignment.Left);
                    if (searchCriteria.Status == (int) CustomerOrderStatus.Open)
                    {
                        panelButtons.AddButton(Resources.Open, Buttons.Filter_Open, "", TouchButtonType.None);
                    }
                    else
                    {
                        panelButtons.AddButton(Resources.Open, Buttons.Filter_Open, ""); 
                    }

                    if (searchCriteria.Status == (int)CustomerOrderStatus.Closed)
                    {
                        panelButtons.AddButton(Resources.Closed, Buttons.Filter_Closed, "", TouchButtonType.None);
                    }
                    else
                    {
                        panelButtons.AddButton(Resources.Closed, Buttons.Filter_Closed, "");
                    }

                    //These statuses are only valid for a customer order
                    if (orderType == CustomerOrderType.CustomerOrder)
                    {
                        if (searchCriteria.Status == (int) CustomerOrderStatus.Printed)
                        {
                            panelButtons.AddButton(Resources.PickingList, Buttons.Filter_PickingList, "", TouchButtonType.None);
                        }
                        else
                        {
                            panelButtons.AddButton(Resources.PickingList, Buttons.Filter_PickingList, "");
                        }

                        if (searchCriteria.Status == (int) CustomerOrderStatus.Ready)
                        {
                            panelButtons.AddButton(Resources.ReadyForDelivery, Buttons.Filter_Ready, "", TouchButtonType.None);
                        }
                        else
                        {
                            panelButtons.AddButton(Resources.ReadyForDelivery, Buttons.Filter_Ready, "");
                        }

                        if (searchCriteria.Status == (int) CustomerOrderStatus.Delivered)
                        {
                            panelButtons.AddButton(Resources.Delivered, Buttons.Filter_Delivered, "", TouchButtonType.None);
                        }
                        else
                        {
                            panelButtons.AddButton(Resources.Delivered, Buttons.Filter_Delivered, "");
                        }
                    }


                    if (!myLocationFilter)
                    {
                        panelButtons.AddButton(Resources.MyLocation, Buttons.Filter_MyLocation, ""); 
                    }
                    else
                    {
                        panelButtons.AddButton(Resources.MyLocation, Buttons.Filter_MyLocation, "", TouchButtonType.None);
                    }
                }

                if (!displayActions)
                {
                    panelButtons.AddButton(Resources.Actions, Buttons.Actions, "", TouchButtonType.Action, DockEnum.DockNone, Resources.white_line_arrow_down_16, ImageAlignment.Left);
                }
                else
                {
                    panelButtons.AddButton(Resources.Actions, Buttons.Actions, "", TouchButtonType.Action, DockEnum.DockNone, Resources.white_line_arrow_up_16, ImageAlignment.Left);
                    panelButtons.AddButton(Resources.PrintInfo, Buttons.PrintInformation, "");

                    //These actions are only valid for a customer order
                    if (orderType == CustomerOrderType.CustomerOrder)
                    {
                        panelButtons.AddButton(Resources.PickingList, Buttons.PickingList, "");
                        panelButtons.AddButton(Resources.ReadyForDelivery, Buttons.ReadyForDelivery, "");
                        panelButtons.AddButton(Resources.Delivered, Buttons.Delivered, "");
                    }
                    else if(orderType == CustomerOrderType.Quote)
                    {
                        panelButtons.AddButton(Resources.CreateSale, Buttons.CreateSale, "");
                    }
                }

                if (searchPanel != null && searchPanel.MultiSelectionActive)
                {
                    panelButtons.AddButton(Resources.MultiSelection, Buttons.Multiselection, "", TouchButtonType.None);
                }
                else if (searchPanel == null || (searchPanel != null && !searchPanel.MultiSelectionActive))
                {
                    panelButtons.AddButton(Resources.MultiSelection, Buttons.Multiselection, "");
                }

                panelButtons.AddButton(Resources.SelectAll, Buttons.SelectAll, "");

                panelButtons.AddButton(Resources.Open, Buttons.Select, "", TouchButtonType.OK, DockEnum.DockEnd);
            }


            panelButtons.AddButton(Resources.Cancel, Buttons.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
        }

        private void panelButtons_Click(object sender, ScrollButtonEventArguments args)
        {
            switch ((Buttons)(int)args.Tag)
            {
                case Buttons.Cancel:
                    ClickCancel();
                    break;
                case Buttons.Select:
                    ClickSelect();
                    break;
                case Buttons.ClearSearch:
                    ClickClearSearch();
                    break;
                case Buttons.Search:
                    ClickSearch();
                    break;
                case Buttons.Delivered:
                case Buttons.ReadyForDelivery:
                    ClickChangeStatus((Buttons) (int) args.Tag);
                    break;
                case Buttons.PickingList:
                    ClickPickingList();
                    break;
                case Buttons.Filters:
                    ClickShowFilters();
                    break;
                case Buttons.Filter_Ready:
                case Buttons.Filter_PickingList:
                case Buttons.Filter_Delivered:
                case Buttons.Filter_MyLocation:
                case Buttons.Filter_Open:
                case Buttons.Filter_Closed:
                    ClickFilters((Buttons)(int)args.Tag);
                    break;
                case Buttons.Multiselection:
                    ClickMultiSelection();
                    break;
                case Buttons.SelectAll:
                    ClickSelectAll();
                    break;
                case Buttons.Actions:
                    ClickActions();
                    break;
                case Buttons.PrintInformation:
                    ClickPrintInformation();
                    break;
                case Buttons.CreateSale:
                    ClickCreateSale();
                    break;
            }
        }

        private void ClickActions()
        {
            displayActions = !displayActions;
            InitializeButtons(ButtonGroup.SearchAndManage);
        }

        private void ClickPrintInformation()
        {
            if (searchPanel != null)
            {
                List<CustomerOrder> selectedOrders = searchPanel.GetSelectedOrders();

                foreach (CustomerOrder order in selectedOrders)
                {
                    RetailTransaction transaction = searchPanel.GetTransactionFromOrder(order, true);
                    Interfaces.Services.CustomerOrderService(dlgEntry).PrintCustomerOrderInformation(dlgEntry, transaction, true);
                }
            }
        }

        private void ClickSelectAll()
        {
            if (searchPanel != null)
            {
                searchPanel.SelectAll = !searchPanel.SelectAll;
                if (!searchPanel.MultiSelectionActive)
                {
                    searchPanel.MultiSelectionActive = true;
                    InitializeButtons(ButtonGroup.SearchAndManage);
                }
            }
        }

        private void ClickMultiSelection()
        {
            if (searchPanel != null)
            {
                searchPanel.MultiSelectionActive = !searchPanel.MultiSelectionActive;

                //If the current setting for SelectAll is true and the multi selection has been turned off then unselect everything
                if (searchPanel.SelectAll && !searchPanel.MultiSelectionActive)
                {
                    searchPanel.SelectAll = false;
                }

                InitializeButtons(ButtonGroup.SearchAndManage);
            }
        }

        private void ClickFilters(Buttons button)
        {
            if (searchPanel != null)
            {
                if (button == Buttons.Filter_Delivered)
                {
                    searchCriteria.Status = (int) CustomerOrderStatus.Delivered;
                }
                else if (button == Buttons.Filter_PickingList)
                {
                    searchCriteria.Status = (int) CustomerOrderStatus.Printed;
                }
                else if (button == Buttons.Filter_Ready)
                {
                    searchCriteria.Status = (int)CustomerOrderStatus.Ready;
                }
                else if (button == Buttons.Filter_Open)
                {
                    searchCriteria.Status = (int)CustomerOrderStatus.Open;
                }
                else if (button == Buttons.Filter_Closed)
                {
                    searchCriteria.Status = (int)CustomerOrderStatus.Closed;
                }
                else if (button == Buttons.Filter_MyLocation)
                {
                    myLocationFilter = !myLocationFilter;
                }

                InitializeButtons(ButtonGroup.SearchAndManage);

                //If My location filter is on then always include that in the search criteria
                searchCriteria.DeliveryLocation = myLocationFilter ? dlgEntry.CurrentStoreID : RecordIdentifier.Empty;
                
                searchPanel.LoadOrders(searchCriteria);
            }
        }

        private void ClickShowFilters()
        {
            displayFilters = !displayFilters;
            InitializeButtons(ButtonGroup.SearchAndManage);
            if (!displayFilters)
            {
                searchCriteria = new CustomerOrderSearch();
                searchCriteriaApplied = false;
                panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), searchCriteriaApplied);
                searchPanel.LoadOrders(searchCriteria);
            }
        }

        private void ClickChangeStatus(Buttons buttonClicked)
        {
            if (searchPanel != null)
            {
                switch (buttonClicked)
                {
                    case Buttons.ReadyForDelivery:
                        searchPanel.SetStatusOnSelectedOrders(CustomerOrderStatus.Ready);
                        break;
                    case Buttons.Delivered:
                        searchPanel.SetStatusOnSelectedOrders(CustomerOrderStatus.Delivered);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(buttonClicked), buttonClicked, null);
                }
            }
        }

        private void ClickPickingList()
        {
            if (searchPanel != null)
            {
                searchPanel.PrintPickingList();
            }
        }

        private void ClickClearSearch()
        {
            searchCriteria = new CustomerOrderSearch();
            if (searchPanel != null)
            {
                searchPanel.LoadOrders(searchCriteria);
                searchCriteriaApplied = false;
                panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), searchCriteriaApplied);
            }
        }

        private void ClickSearch()
        {
            SearchCustomerOrders search = new SearchCustomerOrders(dlgEntry, searchCriteria, (IPosTransaction)retailTransaction, orderType);
            if (search.ShowDialog() == DialogResult.OK)
            {
                searchPanel.SearchCritiera = search.SearchCriteria;
                searchCriteriaApplied = true;
                panelButtons.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSearch), searchCriteriaApplied);
                searchPanel.LoadOrders(search.SearchCriteria);
            }
        }

        private void ClickSelect()
        {
            if (searchPanel != null)
            {
                SelectedCustomerOrder = searchPanel.SelectedOrder;
                SelectedTransaction = searchPanel.SelectedTransaction;

                if (SelectedTransaction == null || SelectedCustomerOrder == null)
                {
                    return;
                }

                SelectedTransaction.CustomerOrder.Comment = SelectedCustomerOrder.Comment;
                SelectedTransaction.StoreId = (string)dlgEntry.CurrentStoreID;
                SelectedTransaction.TerminalId = (string)dlgEntry.CurrentTerminalID;

                if (CanBeRecalledToPOS(SelectedTransaction, SelectedCustomerOrder))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.OrderCannotBeRecalledToPOS);
                }
            }
        }

        private void ClickCancel()
        {
            foreach (ISaleLineItem item in retailTransaction.SaleItems.Where(w => w.Order.ToPickUp > 0))
            {
                item.Order.ToPickUp = 0;
            }

            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ClickCreateSale()
        {
            if (searchPanel != null)
            {
                SelectedCustomerOrder = searchPanel.SelectedOrder;
                SelectedTransaction = searchPanel.SelectedTransaction;

                if(SelectedTransaction == null || SelectedCustomerOrder == null)
                {
                    return;
                }

                if (CanBeRecalledToPOS(SelectedTransaction, SelectedCustomerOrder))
                {
                    Interfaces.Services.CustomerOrderService(dlgEntry).SaveCustomerOrder(dlgEntry, SelectedTransaction, CustomerOrderStatus.Closed);
                    SelectedTransaction.CustomerOrder.Clear();
                    SelectedTransaction.BeginDateTime = DateTime.Now;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.OrderCannotBeRecalledToPOS);
                }
            }
        }


        private void SetActivePanelVisible()
        {
            if (searchPanel != null)
            {
                searchPanel.Visible = false;
            }

            activePanel.Visible = true;
        }

        private bool CanBeRecalledToPOS(IRetailTransaction retailTransaction, CustomerOrder order)
        {
            if (string.IsNullOrWhiteSpace(retailTransaction.Customer.ID.StringValue) && !string.IsNullOrWhiteSpace(order.CustomerID.StringValue))
            {
                Customer customer = Providers.CustomerData.Get(dlgEntry, order.CustomerID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);
                retailTransaction.Add(customer);
            }

            else if (retailTransaction.Customer.ID != order.CustomerID)
            {
                Customer transCustomer = Providers.CustomerData.Get(dlgEntry, retailTransaction.Customer.ID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);
                Customer orderCustomer = Providers.CustomerData.Get(dlgEntry, order.CustomerID, UsageIntentEnum.Normal, CacheType.CacheTypeApplicationLifeTime);

                string transCustomerName = transCustomer.FirstName != "" ? transCustomer.GetFormattedName( dlgEntry.Settings.NameFormatter) : transCustomer.Text;
                string orderCustomerName = orderCustomer.FirstName != "" ? orderCustomer.GetFormattedName( dlgEntry.Settings.NameFormatter) : orderCustomer.Text;

                if (Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.CustomerOnOrderIsNotSameAsOnTransaction.Replace("#1", orderCustomerName).Replace("#2", transCustomerName) + " " + Resources.DoYouWantToUpdateCustomerOnSale, MessageBoxButtons.YesNo, MessageDialogType.Attention) == DialogResult.Yes)
                {
                    retailTransaction.Add(orderCustomer);
                }
            }

            if (order.OrderType == CustomerOrderType.CustomerOrder && retailTransaction.SaleItems.Count(c => !c.Voided && !c.Order.FullyReceived) == 0)
            {
                return false;
            }

            if (order.OrderType == CustomerOrderType.Quote && retailTransaction.CustomerOrder.Status == CustomerOrderStatus.Closed)
            {
                return false;
            }

            return true;
        }
    }
}
