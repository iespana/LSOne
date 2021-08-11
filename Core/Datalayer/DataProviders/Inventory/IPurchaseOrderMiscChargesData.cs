using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IPurchaseOrderMiscChargesData : IDataProvider<PurchaseOrderMiscCharges>, ISequenceable
    {
        /// <summary>
        /// Gets a purchase order misc charge with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderMiscCharge">The ID of the purchase order misc charge to get</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <returns>A purchase order misc charge with a given ID</returns>
        PurchaseOrderMiscCharges Get(IConnectionManager entry, RecordIdentifier purchaseOrderMiscCharge, bool includeReportFormatting);

        /// <summary>
        /// Gets a purchase order misc charges for a given purchase order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="purchaseOrderID">The ID of the purchase order to get misc charges for</param>
        /// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
        /// <param name="sort">An enum that tells us which column to sort by</param>
        /// <param name="sortBackwards">Whether to reverse the result set</param>
        /// <returns>A purchase order misc charge with a given ID</returns>
        List<PurchaseOrderMiscCharges> GetMischChargesForPurchaseOrder(IConnectionManager entry, RecordIdentifier purchaseOrderID,
            PurchaseOrderMiscChargesSorting sort, bool sortBackwards, bool includeReportFormatting);

        /// <summary>
        /// Copies all misc charges from oldPurchaseOrderID to newPurchaseOrderID
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="oldPurchaseOrderID"></param>
        /// <param name="newPurchaseOrderID"></param>
        void CopyMiscChargesBetweenPOs(IConnectionManager entry, RecordIdentifier oldPurchaseOrderID, RecordIdentifier newPurchaseOrderID);
    }
}