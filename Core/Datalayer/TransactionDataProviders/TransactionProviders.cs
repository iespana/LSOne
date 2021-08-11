using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Auditing;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.TransactionDataProviders.Auditing;
using LSOne.DataLayer.TransactionDataProviders.EFT;
using LSOne.DataLayer.TransactionDataProviders.EOD;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.EFT;
using LSOne.DataLayer.TransactionObjects.EOD;
using LSOne.DataLayer.TransactionObjects.Receipts;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionDataProviders
{
    public static class TransactionProviders
    {
        static public ITenderDeclarationData TenderDeclarationData { get { return DataProviderFactory.Instance.Get<ITenderDeclarationData, TransactionObjects.TenderDeclarationTransaction>();} }
        static public ITenderTransactionData TenderTransactionData { get { return DataProviderFactory.Instance.Get<ITenderTransactionData, BusinessObjects.DataEntity>();} }
        static public IInfocodeTransactionData InfocodeTransactionData { get { return DataProviderFactory.Instance.Get<IInfocodeTransactionData, BusinessObjects.Transactions.Line.InfoCodeLineItem>();} }
        static public ITenderLineItemData TenderLineItemData { get { return DataProviderFactory.Instance.Get<ITenderLineItemData, TransactionObjects.Line.TenderItem.TenderLineItem>();} }
        static public ILoyaltyTransactionData LoyaltyTransactionData { get { return DataProviderFactory.Instance.Get<ILoyaltyTransactionData, TransactionObjects.Line.Loyalty.LoyaltyItem>();} }
        static public ISaleLineItemData SaleLineItemData { get { return DataProviderFactory.Instance.Get<ISaleLineItemData, BusinessObjects.DataEntity>();} }
        static public ITaxTransactionData TaxTransactionData { get { return DataProviderFactory.Instance.Get<ITaxTransactionData, BusinessObjects.Tax.TaxItem>();} }        
        static public IDiningTableTransactionData DiningTableTransactionData { get { return DataProviderFactory.Instance.Get<IDiningTableTransactionData, TransactionObjects.DiningTableTransaction>();} }
        static public IOrderInvoiceTransactionData OrderInvoiceTransactionData { get { return DataProviderFactory.Instance.Get<IOrderInvoiceTransactionData, BusinessObjects.DataEntity>();} }
        static public IIncomeExpenseItemData IncomeExpenseItemData { get { return DataProviderFactory.Instance.Get<IIncomeExpenseItemData, BusinessObjects.DataEntity>();} }
        static public IRemoveTenderTransactionData RemoveTenderTransactionData { get { return DataProviderFactory.Instance.Get<IRemoveTenderTransactionData, TransactionObjects.Line.TenderItem.TenderLineItem>();} }
        static public IBankDropReversalTransactionData BankDropReversalTransactionData { get { return DataProviderFactory.Instance.Get<IBankDropReversalTransactionData, TransactionObjects.BankDropReversalTransaction>();} }
        static public ISafeDropReversalTransactionData SafeDropReversalTransactionData { get { return DataProviderFactory.Instance.Get<ISafeDropReversalTransactionData, TransactionObjects.SafeDropReversalTransaction>();} }
        static public ISafeDropTransactionData SafeDropTransactionData { get { return DataProviderFactory.Instance.Get<ISafeDropTransactionData, TransactionObjects.SafeDropTransaction>();} }
        static public IBankDropTransactionData BankDropTransactionData { get { return DataProviderFactory.Instance.Get<IBankDropTransactionData, TransactionObjects.BankDropTransaction>();} }
        static public IFuelSalesLineItemData FuelSalesLineItemData { get { return DataProviderFactory.Instance.Get<IFuelSalesLineItemData, BusinessObjects.DataEntity>();} }
        static public IDiscountTransactionData DiscountTransactionData { get { return DataProviderFactory.Instance.Get<IDiscountTransactionData, BusinessObjects.DataEntity>();} }
        static public IReceiptSequence ReceiptSequence { get { return DataProviderFactory.Instance.Get<IReceiptSequence, BusinessObjects.DataEntity>();} }
        static public IPosTransactionData PosTransactionData { get { return DataProviderFactory.Instance.Get<IPosTransactionData, TransactionObjects.PosTransaction>();} }
        static public IEODInfoData EODInfoData { get { return DataProviderFactory.Instance.Get<IEODInfoData, TransactionObjects.EOD.EODInfo>();} }
        static public IHospitalityTransactionData HospitalityTransactionData { get { return DataProviderFactory.Instance.Get<IHospitalityTransactionData, TransactionObjects.HospitalityTransaction>();} }
        static public IInventorySerialData InventorySerialData { get { return DataProviderFactory.Instance.Get<IInventorySerialData, TransactionObjects.InventorySerial>();} }
        static public ISerializedTransactionData SerializedTransactionData { get { return DataProviderFactory.Instance.Get<ISerializedTransactionData, TransactionObjects.PosTransaction>();} }
        static public IForecourtSoundData ForecourtSoundData { get { return DataProviderFactory.Instance.Get<IForecourtSoundData, TransactionObjects.ForecourtSound>();} }
        static public IOperationAuditingData OperationAuditingData { get { return DataProviderFactory.Instance.Get<IOperationAuditingData, OperationAuditing>(); } }
        static public IDataDirectorTransactionJobData DataDirectorTransactionJobData { get { return DataProviderFactory.Instance.Get<IDataDirectorTransactionJobData, DataDirectorTransactionJob>(); } }
        static public IStartOfDayData StartOfDayData { get { return DataProviderFactory.Instance.Get<IStartOfDayData, RecordIdentifier>(); } }
        static public IItemReportInfoData ItemReportInfoData { get { return DataProviderFactory.Instance.Get<IItemReportInfoData, ItemSaleReportLine>(); } }
        static public IReprintTransactionData ReprintTransactionData { get { return DataProviderFactory.Instance.Get<IReprintTransactionData, ReprintInfo>(); } }
        static public IReceiptTransactionData ReceiptTransactionData { get { return DataProviderFactory.Instance.Get<IReceiptTransactionData, ReceiptInfo>(); } }
        static public ILogTransactionData LogTransactionData { get { return DataProviderFactory.Instance.Get<ILogTransactionData, LogTransaction>(); } }
        static public IEFTInfoData EFTInfoData { get { return DataProviderFactory.Instance.Get<IEFTInfoData, EFTInfo>(); } }

    }
}
