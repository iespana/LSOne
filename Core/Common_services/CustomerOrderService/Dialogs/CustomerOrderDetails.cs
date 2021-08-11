using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.POS.Processes;
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
    public partial class CustomerOrderDetails : TouchBaseForm
    {
        /// <summary>
        /// If true then changes were done to the Customer order/ Quote and the information needs to be saved again
        /// </summary>
        public bool SaveChanges;

        private Control activePanel;
        private IConnectionManager dlgEntry;
        private ISettings settings;
        private IRetailTransaction retailTransaction;

        private Point panelLocation;
        private Size panelSize;

        private InformationPanel infoPanel;
        private EditDetailsPanel editDetailsPanel;

        private CustomerOrderItem orgCustomerOrder;
        private bool newCustomerOrder;
        private CustomerOrderSettings orderSettings;

        /// <summary>
        /// The constructor for the Customer order details dialog
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current transaction</param>
        /// <param name="orderType">What type of customer order is being created/viewed; Quote or Customer order</param>
        /// <param name="action">What is the action that was selected (if the dialog is being displayed after an action was selected)</param>
        public CustomerOrderDetails(IConnectionManager entry, IRetailTransaction retailTransaction, CustomerOrderType orderType, CustomerOrderAction action)
        {
            InitializeComponent();

            dlgEntry = entry;
            settings = (ISettings) entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
            this.retailTransaction = retailTransaction;
            orgCustomerOrder = (CustomerOrderItem) retailTransaction.CustomerOrder.Clone();
            newCustomerOrder = retailTransaction.CustomerOrder.OrderType == CustomerOrderType.None;
            orderSettings = Providers.CustomerOrderSettingsData.Get(entry, orderType);

            SaveChanges = false;
            
            //A Quote can be changed to a customer order but not the other way around
            if (retailTransaction.CustomerOrder.OrderType != CustomerOrderType.CustomerOrder)
            {
                retailTransaction.CustomerOrder.OrderType = orderType;
            }

            panelLocation = new Point(1, 60);
            panelSize = new Size(1022, 698);
            
            infoPanel = null;
            editDetailsPanel = null;

            if (orgCustomerOrder.Empty())
            {
                retailTransaction.CustomerOrder.DeliveryLocation = dlgEntry.CurrentStoreID;
                AttachEditDetailsPanel();
            }
            else
            {
                AttachInfoPanel(action);
            }
        }

        private void CustomerOrderDetails_Load(object sender, EventArgs e)
        {
            banner.BannerText = retailTransaction.CustomerOrder.OrderType == CustomerOrderType.CustomerOrder ? Properties.Resources.CustomerOrderHeaderText : Properties.Resources.QuoteHeaderText;
        }

        private void AttachInfoPanel(CustomerOrderAction action = CustomerOrderAction.None)
        {
            if (infoPanel != null)
            {
                infoPanel.SetCustomerOrderInfo();
            }
            else
            {
                infoPanel = new InformationPanel(dlgEntry, retailTransaction, action, orderSettings);
            }
            
            infoPanel.Location = panelLocation;
            infoPanel.Size = panelSize;
            
            Controls.Add(infoPanel);

            activePanel = infoPanel;

            SetActivePanelVisible();
            
        }

        private void AttachEditDetailsPanel()
        {
            if (editDetailsPanel != null)
            {
                orgCustomerOrder = (CustomerOrderItem) retailTransaction.CustomerOrder.Clone();
                editDetailsPanel.CustomerOrder = retailTransaction.CustomerOrder;
                editDetailsPanel.SetCustomerOrderDetails();
            }
            else
            {
                editDetailsPanel = new EditDetailsPanel(dlgEntry, retailTransaction, newCustomerOrder);
            }

            editDetailsPanel.Location = panelLocation;
            editDetailsPanel.Size = panelSize;

            Controls.Add(editDetailsPanel);

            activePanel = editDetailsPanel;

            SetActivePanelVisible();
        }

        internal decimal GetAmount(bool isAdditionalPayment)
        {
            decimal input = decimal.Zero;
            string promptText = isAdditionalPayment ? Properties.Resources.AdditionalPayment : Properties.Resources.MinimumDeposit;
            
            if (Interfaces.Services.DialogService(dlgEntry).NumpadInput(settings, ref input, promptText, Properties.Resources.Amount, true, DecimalSettingEnum.Prices) == DialogResult.OK)
            {
                if (input > retailTransaction.TransSalePmtDiff)
                {
                    string msg = isAdditionalPayment ? Properties.Resources.AdditionalPaymentCannotBeHigherThanBalance : Properties.Resources.OverrideDepositCannotBeHigherThanBalance;
                    Interfaces.Services.DialogService(dlgEntry).ShowMessage(msg);
                    return decimal.MinusOne;
                }

                return Conversion.ToDecimal(input);
            }

            return decimal.MinusOne;
        }

        internal void ClickAdditionalPayments()
        {
            if (retailTransaction.SaleItems.Any(a => a.Order.ToPickUp > 0))
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Resources.AnAdditionalPaymentCannotBeAddedWhenPickingUpItems);
                return;
            }
            decimal input = GetAmount(true);

            if (input > decimal.MinusOne)
            {
                retailTransaction.CustomerOrder.AdditionalPayment = input;
                retailTransaction.CustomerOrder.HasAdditionalPayment = input != decimal.Zero;

                if (activePanel == infoPanel)
                {
                    infoPanel.SetCustomerOrderInfo();
                }
            }
        }


        internal void ClickOverrideDeposit()
        {
            decimal input = GetAmount(false);

            if (input > decimal.MinusOne)
            {
                retailTransaction.CustomerOrder.MinimumDeposit = input;
                retailTransaction.CustomerOrder.DepositOverriden = true;

                if (activePanel == infoPanel)
                {
                    infoPanel.SetCustomerOrderInfo();
                }

            }
        }

        internal void ClickEditDetails()
        {
            AttachEditDetailsPanel();
        }

        internal void ClickCancel()
        {
            retailTransaction.CustomerOrder = (CustomerOrderItem) orgCustomerOrder.Clone();

            if (retailTransaction.CustomerOrder.Status != CustomerOrderStatus.New && activePanel == editDetailsPanel)
            {
                AttachInfoPanel();
            }
            else 
            {
                if (retailTransaction.CustomerOrder.Status == CustomerOrderStatus.New)
                {
                    retailTransaction.CustomerOrder.Clear();
                    foreach (ISaleLineItem item in retailTransaction.SaleItems)
                    {
                        item.Order.Clear();
                    }
                }

                //Update the receipt control to reflect the changed sale
                ISettings settings = (ISettings)dlgEntry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                ((POSApp)settings.POSApp).SetTransaction(retailTransaction);

                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void CheckCustomerInfo()
        {
            if (retailTransaction.Customer.ID == RecordIdentifier.Empty)
            {
                if (activePanel != editDetailsPanel)
                {
                    return;
                }

                Interfaces.Services.CustomerService(dlgEntry).Search(dlgEntry, retailTransaction, false);

                if (retailTransaction.Customer.ID != RecordIdentifier.Empty)
                {
                    editDetailsPanel.SetCustomer(retailTransaction.Customer);
                }
            }
        }

        private bool CheckDeliveryLocationInfo(CustomerOrderItem order)
        {
            if (order.DeliveryLocation == RecordIdentifier.Empty)
            {
                Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.DeliveryLocationMustBeSelected);
                return false;
            }

            return true;
        }

        internal void ClickSave()
        {
            if (activePanel == editDetailsPanel)
            {
                CheckCustomerInfo();

                if (retailTransaction.Customer.ID == RecordIdentifier.Empty)
                    return;

                if (CheckDeliveryLocationInfo(editDetailsPanel.CustomerOrder))
                {

                    retailTransaction.CustomerOrder = editDetailsPanel.CustomerOrder;
                    SaveChanges = true;
                    SetDefaultAction();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        internal void ClickNext()
        {
            if (activePanel == editDetailsPanel)
            {
                CheckCustomerInfo();

                if (retailTransaction.Customer.ID == RecordIdentifier.Empty)
                    return;

                if (CheckDeliveryLocationInfo(editDetailsPanel.CustomerOrder))
                {
                    newCustomerOrder = false;
                    retailTransaction.CustomerOrder = editDetailsPanel.CustomerOrder;
                    AttachInfoPanel();
                }
            }
        }

        internal void ClickFinish()
        {
            if(infoPanel != null)
            {
                infoPanel.SplitItems();
            }

            SetDefaultAction();
            if (activePanel == infoPanel)
            {
                SaveChanges = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        

        private void SetDefaultAction()
        {
            if (retailTransaction.CustomerOrder.HasAdditionalPayment)
            {
                retailTransaction.CustomerOrder.CurrentAction = CustomerOrderAction.AdditionalPayment;
            }
            else if (retailTransaction.SaleItems.Count(c => !c.Voided && c.Order.ToPickUp > 0) == 0)
            {
                retailTransaction.CustomerOrder.CurrentAction = CustomerOrderAction.PayDeposit;
            }
            else if (retailTransaction.SaleItems.Count(c => !c.Voided && c.Order.ToPickUp > 0) > 0)
            {
                retailTransaction.CustomerOrder.CurrentAction = CustomerOrderAction.PartialPickUp;
            }
        }

        private void SetActivePanelVisible()
        {
            if (infoPanel != null)
            {
                infoPanel.Visible = false;
            }

            if (editDetailsPanel != null)
            {
                editDetailsPanel.Visible = false;
            }

            activePanel.Visible = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            if(editDetailsPanel != null)
            {
                editDetailsPanel.DisableScanner();
            }

            base.OnClosed(e);
        }
    }
}
