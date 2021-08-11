using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.EventArguments;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface IEventService : IService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="e"></param>
        void ReceiptSaleItemDataChange(IConnectionManager entry, SaleItemDataChangeArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="e"></param>
        void ReceiptCustomerDepositDataChange(IConnectionManager entry, CustomerDepositDataChangeArgs e);


        /// <summary>
        /// If true then the Totals amount box will broadcast the TotalsDataChange event to the Events service
        /// </summary>
        bool BroadcastTotalsDataChangeEnabled();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="e"></param>
        void TotalsDataChange(IConnectionManager entry, TotalsDataChangeArgs e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="customer"></param>
        /// <param name="posTransaction"></param>
        void CustomerVisualComponentDataChanged(IConnectionManager entry, Customer customer, IPosTransaction posTransaction);

        /// <summary>
        /// Is called before an item is added to the receipt panel on the POS. If the item should not be displayed return DisplayReceiptItem as false
        /// </summary>
        /// <param name="e"></param>
        void PreDisplayReceiptItem(PreDisplayReceiptItemArgs e);

        /// <summary>
        /// Called before refreshing the POS status bar allowing to override the terminal and operator status
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="e"></param>
        void PreRefreshStatusStrip(IConnectionManager entry, PreRefreshStatusStripArgs e);

        /// <summary>
        /// Get an HTML string with information to be displayed in the HTML information panel
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="posTransaction">Current transaction</param>
        /// <returns></returns>
        string GetHTMLInformation(IConnectionManager entry, IPosTransaction posTransaction);
    }
}
