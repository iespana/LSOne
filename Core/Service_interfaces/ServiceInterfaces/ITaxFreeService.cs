using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Support for tax free / tax refund
    /// </summary>
    public interface ITaxFreeService : IService
    {
        /// <summary>
        /// Return true to show a button in the daily journal for adding tax free
        /// </summary>
        bool ShowInJournal { get; }

        /// <summary>
        /// Capture information on customer and print tax free slip
        /// </summary>
        /// <param name="entry">Entry into database</param>
        /// <param name="transaction">The transaction to capture</param>
        void CaptureSale(IConnectionManager entry, IPosTransaction transaction);

        void TaxRefundMulti(IConnectionManager entry, IPosTransaction transaction);

        bool CancelTaxRefund(IConnectionManager entry, RecordIdentifier refundID);
    }
}
