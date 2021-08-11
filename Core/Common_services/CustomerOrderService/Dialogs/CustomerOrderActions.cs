using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using LSOne.Utilities.ColorPalette;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    /// <summary>
    /// A dialog that is displayed when the user is paying for a transaction that displays what actions are available on the customer order/quote
    /// </summary>
    public partial class CustomerOrderActions : TouchBaseForm
    {
        private CustomerOrderItem customerOrder;
        private IConnectionManager dlgEntry;
        private IRetailTransaction retailTransaction;
        private CustomerOrderAction orgAction;

        /// <summary>
        /// What action was selected in the dialog see <see cref="CustomerOrderAction"/>
        /// </summary>
        public CustomerOrderAction SelectedAction;

        /// <summary>
        /// The constructor of the dialog
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailTransaction">The current retail transaction</param>
        public CustomerOrderActions(IConnectionManager entry, IRetailTransaction retailTransaction)
        {
            InitializeComponent();

            touchDialogBanner1.Location = new Point(1, 1);
            touchDialogBanner1.Width = Width - 2;

            dlgEntry = entry;
            this.customerOrder = retailTransaction.CustomerOrder;
            this.retailTransaction = retailTransaction;
            this.orgAction = customerOrder.CurrentAction;

            SelectedAction = CustomerOrderAction.None;

            InitializeButtons();

            if (customerOrder.OrderType == CustomerOrderType.Quote)
            {
                touchDialogBanner1.BannerText = Resources.QuoteActions;
            }

        }

        private void InitializeButtons()
        {
            panelOptions.Clear();

            //Option buttons
            if (customerOrder.OrderType == CustomerOrderType.CustomerOrder)
            {
                //Get all deposits to be paid
                decimal depositToBePaid = CalculationHelper.GetTotalDeposit(dlgEntry, CustomerOrderSummaries.ToPickUp, retailTransaction) +
                                          CalculationHelper.GetTotalDeposit(dlgEntry, CustomerOrderSummaries.OnOrder, retailTransaction);

                //Get amount of any items to be paid
                decimal pickupToBePaid = CalculationHelper.GetTotalToBePaid(dlgEntry, CustomerOrderSummaries.ToPickUp, retailTransaction, true);

                //Get any deposits that are to be returned
                decimal depositToBeReturned = CalculationHelper.GetTotalDepositToBeReturned(dlgEntry, retailTransaction);

                //If true then there have been other tender lines added in this session so the operation has already started
                bool underTendering = retailTransaction.TenderLines.Count(c => !c.Voided && !c.PaidDeposit) > 0;

                //Are there any items to manage on the transaction
                bool allowManageItems = (retailTransaction.SaleItems.Count(c => !c.Voided && !c.Order.FullyReceived) > 0);

                bool allowAdditionalPayments = retailTransaction.CustomerOrder.HasAdditionalPayment;

                bool depositDisplayed = false;

                if (allowAdditionalPayments)
                {
                    panelOptions.AddButton(Resources.PayAdditionalPayment, CustomerOrderAction.AdditionalPayment, "");
                }
                else
                {
                    //If there is a deposit to be returned then display the deposit options
                    if (depositToBeReturned != decimal.Zero && (depositToBePaid + pickupToBePaid) == decimal.Zero)
                    {
                        panelOptions.AddButton(Resources.ReturnFullDeposit, CustomerOrderAction.ReturnFullDeposit, "");
                    }

                    //If there is only deposit to be paid then PayDeposit should be displayed
                    if (allowManageItems && (depositToBePaid != decimal.Zero && pickupToBePaid == decimal.Zero && depositToBeReturned == decimal.Zero))
                    {
                        panelOptions.AddButton(Resources.PayDeposit, CustomerOrderAction.PayDeposit, "", TouchButtonType.OK);
                        depositDisplayed = true;
                    }

                    //If there is either a deposit or pickup payment to be paid then display Continue to payment
                    if (allowManageItems && (underTendering || (depositToBePaid != decimal.Zero && pickupToBePaid != decimal.Zero && depositToBeReturned == decimal.Zero)))
                    {
                        //if pay deposit has already been displayed then don't display Continue to payment
                        //only happens when paying a deposit with multiple tenders
                        if (!depositDisplayed)
                        {
                            panelOptions.AddButton(Resources.ContinueToPayment, CustomerOrderAction.ContinueToPayment, "", TouchButtonType.OK);
                        }
                    }

                    //If there is nothing to be paid then Update order can be displayed which only saves the order
                    if (depositToBePaid == decimal.Zero && pickupToBePaid == decimal.Zero && depositToBeReturned == decimal.Zero)
                    {
                        panelOptions.AddButton(Resources.SaveChanges, CustomerOrderAction.SaveChanges, "", TouchButtonType.OK);
                        panelOptions.AddButton(Resources.SaveChangesAndPrint, CustomerOrderAction.SaveChangesAndPrintReceiptCopy, "", TouchButtonType.OK);
                    }

                    if (allowManageItems && !underTendering)
                    {
                        panelOptions.AddButton(Resources.PickUpEntireOrder, CustomerOrderAction.FullPickup, "");
                        panelOptions.AddButton(Resources.PartialPickup, CustomerOrderAction.PartialPickUp, "");
                    }
                }

            }
            else if (customerOrder.OrderType == CustomerOrderType.Quote)
            {
                panelOptions.AddButton(Resources.SaveChanges, CustomerOrderAction.SaveChanges, "");
                panelOptions.AddButton(Resources.CancelQuote, CustomerOrderAction.CancelQuote, "");
            }

            panelOptions.AddButton(Resources.Cancel, CustomerOrderAction.Cancel, "", TouchButtonType.Cancel);

            panelOptions.Height = panelOptions.ButtonCount * panelOptions.ButtonHeight;
            this.Height = panelOptions.Height + 72;
        }

        private void panelClick(object sender, ScrollButtonEventArguments args)
        {
            switch ((CustomerOrderAction)(int)args.Tag)
            {
                case CustomerOrderAction.SaveChangesAndPrintReceiptCopy:
                case CustomerOrderAction.SaveChanges:
                case CustomerOrderAction.AdditionalPayment:
                case CustomerOrderAction.FullPickup:
                case CustomerOrderAction.PartialPickUp:
                case CustomerOrderAction.CancelQuote:
                case CustomerOrderAction.PayDeposit:
                case CustomerOrderAction.ContinueToPayment:
                case CustomerOrderAction.ReturnFullDeposit:
                case CustomerOrderAction.ReturnPartialDeposit:
                    ClickAction((CustomerOrderAction)(int)args.Tag);
                    break;
                case CustomerOrderAction.Cancel:
                    ClickCancel();
                    break;
            }
        }

        private void ClickAction(CustomerOrderAction action)
        {
            SelectedAction = action;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ClickCancel()
        {
            SelectedAction = CustomerOrderAction.Cancel;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
