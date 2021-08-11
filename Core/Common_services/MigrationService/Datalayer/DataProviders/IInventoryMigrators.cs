using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.DataProviders
{
    public interface IInventoryMigrators : IDataProviderBase<InventoryTransaction>
    {
        

        List<InventoryTransaction> GetInventoryTransactionsForVariant(IConnectionManager entry, RecordIdentifier variantID);

     
        void SaveInventoryTransaction(IConnectionManager entry, InventoryTransaction offer);

        List<InventoryJournalTransaction> GetJournalTransactionListForVariant(IConnectionManager entry,
            RecordIdentifier variantID);

        void SaveInventoryJournal(IConnectionManager entry, InventoryJournalTransaction inventoryJournalTrans);

        List<InventoryTransferOrderLine> GetInventoryTransferOrderForVariant(
            IConnectionManager entry,
            RecordIdentifier variantID);

        List<InventoryTransferRequestLine> GetInventoryTransferRequestListForVariant(
            IConnectionManager entry,
            RecordIdentifier variantID);

        void SaveInventoryTransferOrder(IConnectionManager entry, InventoryTransferOrderLine inventoryTransferOrderLine);

        void SaveInventoryTransferRequest(IConnectionManager entry,
            InventoryTransferRequestLine inventoryTransferRequestLine);

        List<PurchaseOrderLine> GetPurchaseOrderLinesForVariant(IConnectionManager entry, RecordIdentifier variantID);

        void SavePurchaseOrderLines(IConnectionManager entry, PurchaseOrderLine purchaseOrderLine);

        List<VendorItem> GetVendorItemsForVariant(IConnectionManager entry, RecordIdentifier variantID);

        void SaveVendorItem(IConnectionManager entry, VendorItem vendorItem);

        void DropVariantIDColumnFromInventSum(IConnectionManager entry);
    }
}