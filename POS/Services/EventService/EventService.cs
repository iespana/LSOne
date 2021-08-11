using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.EventArguments;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services
{
    public partial class EventService : IEventService
    {
        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }

        public virtual IErrorLog ErrorLog
        {
            set
            {

            }
        }

        #region IEvents Members

        public virtual void ReceiptSaleItemDataChange(IConnectionManager entry, SaleItemDataChangeArgs e)
        {
        }

        public virtual void ReceiptCustomerDepositDataChange(IConnectionManager entry, CustomerDepositDataChangeArgs e)
        {
        }


        /// <summary>
        /// If true then the Totals amount box will broadcast the information to the Events service
        /// </summary>
        /// <returns></returns>
        public virtual bool BroadcastTotalsDataChangeEnabled()
        {
            return false; 
        }

        public virtual void TotalsDataChange(IConnectionManager entry, TotalsDataChangeArgs e)
        {
            //NOTE: If BroadcastTotalsDataChangeEnabled returns false then this event is never called
            //      The performance of the POS can be affected by this broadcast - especially if the expected basket of items is over 40-50 items
            //      which is why this broadcast has to be enabled.
        }

        public virtual void CustomerVisualComponentDataChanged(IConnectionManager entry, Customer customer, IPosTransaction posTransaction)
        {
        }

        public virtual void PreDisplayReceiptItem(PreDisplayReceiptItemArgs e)
        {
        }

        /// <summary>
        /// Called before setting the terminal and operator name on the status bar. Setting the terminal and operator status arguments will override the default values.
        /// Ex:  e.TerminalStatus = "Your custom text here";
        /// 
        /// Leaving the arguments empty will use the default LS One values.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="e"></param>
        public virtual void PreRefreshStatusStrip(IConnectionManager entry, PreRefreshStatusStripArgs e)
        {
            
        }

        /// <summary>
        /// Get an HTML string with information to be displayed in the HTML information panel
        /// If the POS layout contains an HTML information panel, this event is called everytime transaction data has changed
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="posTransaction">Current transaction</param>
        /// <returns></returns>
        public string GetHTMLInformation(IConnectionManager entry, IPosTransaction posTransaction)
        {
            //Example how to add the assembly to the HtmlRenderer if you want to use properties/method/resources inside the HTML (like a property that defines a CSS)
            //if(!HtmlRenderer.References.Contains(Assembly.GetExecutingAssembly()))
            //{
            //    HtmlRenderer.References.Add(Assembly.GetExecutingAssembly());
            //}

            var retailTransaction = posTransaction as RetailTransaction;

            if (retailTransaction != null)
            {
                ITenderRestrictionService tenderRestrictionService = Interfaces.Services.TenderRestrictionService(entry);

                var settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
                if (settings.FunctionalityProfile.DisplayLimitationsTotalsInPOS)
                {
                    retailTransaction.HTMLInformation = retailTransaction.IsReturnTransaction ?
                                                        tenderRestrictionService.GetRefundableAmountLimitedToPaymentTypeAsHtml(entry, settings, posTransaction) :
                                                        tenderRestrictionService.GetPaymentLimitationsAsHtml(entry, settings, posTransaction);
                }
                else
                {
                    retailTransaction.HTMLInformation = string.Empty;
                }

                return retailTransaction.HTMLInformation;
            }

            return "";
        }

        #endregion
    }
}
