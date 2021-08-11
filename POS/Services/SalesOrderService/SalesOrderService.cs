using System;
using System.Linq;
using System.Globalization;
using System.Windows.Forms;
using LSOne.Controls.Dialogs;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.SalesOrderItem;
using LSOne.POS.Core;
using LSOne.POS.Core.Exceptions;
using LSOne.POS.Processes.WinFormsKeyboard;
using LSOne.Services.Dialogs;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using SalesOrderInterface;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Services.Interfaces.Constants;
using LSOne.Utilities.DataTypes;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.BusinessObjects.SalesOrder;
using System.Collections.Generic;
using LSOne.Controls.SupportClasses;

namespace LSOne.Services
{
    public partial class SalesOrderService : ISalesOrderService
    {


        #region ISalesOrder Members

        public virtual bool SiteServiceIsNeeded(IConnectionManager entry, IPosTransaction transaction)
        {
            if (transaction is RetailTransaction)
            {
                if (((RetailTransaction)transaction).SalesOrderAmounts != decimal.Zero)
                {
                    return ((RetailTransaction)transaction).SaleItems.Any(a => a is SalesOrderLineItem);
                }
            }            
            
            return false;
        }

        public virtual void SalesOrders(IConnectionManager entry, IPosTransaction posTransaction)
        {
            try
            {
                SalesOrderLineItem soLineItem = new SalesOrderLineItem((RetailTransaction)posTransaction);

                using (SalesOrderDialog dialog = new SalesOrderDialog(entry, (RetailTransaction)posTransaction))
                {
                    dialog.ShowDialog();                   
                }                
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual SalesOrderResult CreatePickingList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest)
        {
            return Interfaces.Services.SiteServiceService(entry).CreatePickingList(entry, siteServiceProfile, salesOrderRequest, true);
        }

        public virtual SalesOrderResult CreatePackingSlip(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest)
        {
            return Interfaces.Services.SiteServiceService(entry).CreatePackingSlip(entry, siteServiceProfile, salesOrderRequest, true);
        }

        public virtual SalesOrderResult GetSalesOrderList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SalesOrderRequest salesOrderRequest, List<SalesOrder> salesOrders)
        {
            return Interfaces.Services.SiteServiceService(entry).GetSalesOrderList(entry, siteServiceProfile, salesOrderRequest, salesOrders, true);
        }

        public virtual bool PriceOverride(IConnectionManager entry, IPosTransaction posTransaction, OperationInfo operationInfo)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                //Get the item to be overridden.
                SalesOrderLineItem saleLineItem = null;
                
                saleLineItem = (SalesOrderLineItem)((RetailTransaction)posTransaction).GetItem(operationInfo.ItemLineId);

                //Price can not be overridden for sales orders retrieved for full payment.  Only for sales orders retrieved for prepayment...
                if (saleLineItem.SalesOrderItemType == SalesOrderItemType.FullPayment)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.PriceCannotBeChangedWhenSalesOrderIsBeingFullyPaidFor, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);                       
                    return false;
                }


                bool inputValid = true;
                do
                {
                    inputValid = true;
                    // Get the new price...
                    decimal price = 0M;

                    if (operationInfo != null && ((OperationInfo)operationInfo).NumpadValue != "")
                    {
                        // The user entered a specific quantity in the numpad prior to the operation
                        price = Convert.ToDecimal(((OperationInfo)operationInfo).NumpadValue, CultureInfo.CurrentCulture);
                    }
                    else if (settings.VisualProfile.TerminalType == VisualProfile.HardwareTypes.Touch)
                    {
                        using (NumpadAmountQtyDialog inputDialog = new NumpadAmountQtyDialog())
                        {
                            inputDialog.HasDecimals = true;
                            inputDialog.PromptText = Properties.Resources.EnterAmount;
                            inputDialog.GhostText = Properties.Resources.Amount;

                            // Quit if cancel is pressed...
                            if (inputDialog.ShowDialog() == DialogResult.Cancel)
                            {
                                return false;
                            }
                            if (inputDialog.HasInput)
                            {
                                price = (decimal)inputDialog.Value;
                            }
                            else
                            {
                                inputValid = false;
                            }

                        }
                    }
                    else
                    {
                        frmInputNumpad inputDialog = new frmInputNumpad(posTransaction);
                        inputDialog.EntryType = frmInputNumpad.EntryTypes.Price;
                        inputDialog.PromptText = Properties.Resources.EnterAmount;  // "Enter amount";
                        ((Control)settings.POSApp.POSMainWindow).Invoke(LSRetailPosis.ApplicationFramework.POSShowFormDelegate, new object[] { inputDialog });


                        // Quit if cancel is pressed...
                        if (inputDialog.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                        {
                            inputDialog.Dispose();                            
                            return false;                            
                        }
                        price = Convert.ToDecimal(inputDialog.InputText, CultureInfo.CurrentCulture);
                        inputDialog.Dispose();
                    }

                    if (price == 0)
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TheAmountCannotBeZero, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);   // The amount cannot be zero.                            
                        return false;
                    }

                    if (price < saleLineItem.PrepayAmount)
                    {
                        //The new price must not be lower then the calculated amount for prepayment
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TheAmountCannotBeLower, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);  // The amount cannot be lower then the calculated amount for prepayment.                            

                        inputValid = false;
                    }
                    else if (price > saleLineItem.Balance)
                    {
                        //The new price must not be higher then the balance of the sales order
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TheAmountCannotBeHigher, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);   // The amount cannot be higher then the balance of the sales order.                            

                        inputValid = false;
                    }
                    else
                    {
                        //Price can be overridden

                        //Update the total sales order amount on the sale - add up all the sales order lines - except the one that is being overriden
                        decimal existingSalesOrders = ((RetailTransaction)posTransaction).SaleItems.                                                        
                                                        Where(w => w is SalesOrderLineItem && !w.Voided && w.LineId != operationInfo.ItemLineId). 
                                                        Sum(s => ((SalesOrderLineItem)s).Amount);
                        ((RetailTransaction)posTransaction).SalesOrderAmounts = existingSalesOrders + price;

                        //((RetailTransaction)posTransaction).SalesOrderAmounts -= saleLineItem.Amount;  // Subtract the old prepay amount
                        //((RetailTransaction)posTransaction).SalesOrderAmounts += price;  // Then add the new prepay amount

                        // Then change the item line
                        saleLineItem.Amount = price;
                        saleLineItem.Price = price;
                        saleLineItem.PriceWithTax = price;
                        saleLineItem.StandardRetailPrice = price;
                    }

                } while (!inputValid);

                return true;

            }
            catch (POSException px)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                throw px;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }

        }

        public virtual SalesOrderResult ConcludeTransaction(IConnectionManager entry, ISettings settings, IPosTransaction posTransaction)
        {
            SalesOrderResult result = SalesOrderResult.Success;
            if (!(posTransaction is RetailTransaction))
            {
                return result;
            }

            if (!((RetailTransaction)posTransaction).SaleItems.Any(a => a is SalesOrderLineItem && !a.Voided))
            {
                return result;
            }

            foreach (ISaleLineItem item in ((RetailTransaction)posTransaction).SaleItems.Where(w => (w is SalesOrderLineItem && !w.Voided)))
            {
                try
                {
                    SalesOrderRequest salesOrderRequest = new SalesOrderRequest();

                    result = Interfaces.Services.SiteServiceService(entry).PaySalesOrder(entry, settings.SiteServiceProfile, ((SalesOrderLineItem)item).SalesOrderId, ((SalesOrderLineItem)item).Amount, posTransaction.TransactionId, salesOrderRequest, true);
                    if (result != SalesOrderResult.Success)
                    {
                        if (SalesOrderErrorResultMessage(entry, result, ""))
                        {
                            return result;
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    SalesOrderErrorResultMessage(entry, SalesOrderResult.ErrorHandlingSalesOrder, ex.Message);
                    return SalesOrderResult.ErrorHandlingSalesOrder;
                }

            }

            return result;
        }        

        protected virtual bool SalesOrderErrorResultMessage(IConnectionManager entry, SalesOrderResult result, string errorMsg)            
        {
            switch (result)
            {
                case SalesOrderResult.ErrorHandlingSalesOrder:
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ErrorProcessingSalesOrderPayment, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        entry.ErrorLogger.LogMessage(LogMessageType.Error, "Error processing the sales order payment. - Error msg: " + errorMsg, "SalesOrderService.ConcludeTransaction()");
                        return false;
                    }

                case SalesOrderResult.CustomerNotFound:
                    {
                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CustomerNotFound, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                        entry.ErrorLogger.LogMessage(LogMessageType.Error, "Customer on sale not found at head office. - Error msg: " + errorMsg, "SalesOrderService.ConcludeTransaction()");
                        return false;
                    }

                case SalesOrderResult.Customized_Result_1:
                    {
                        //Display a customized message for this result and the other 9 similar status that are available for customizations
                        break;
                    }                
            }

            return true;
        }


        #endregion

        public void Init(IConnectionManager entry)
        {
            #pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            #pragma warning restore 0612, 0618
        }

        public virtual IErrorLog ErrorLog { set; private get; }
    }
}
