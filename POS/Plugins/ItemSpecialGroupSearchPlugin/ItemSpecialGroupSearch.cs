using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace ItemSpecialGroupSearchPlugin
{
    public class ItemSpecialGroupSearch : IPOSPlugin
    {
        public IPosTransaction RunTask(IConnectionManager entry, ISession session, ISettings settings, IPosTransaction transaction,
            OperationInfo operationInfo, string task, List<string> args)
        {

            string selectedItemID = "";
            string selectedItemName = "";            
            if (Services.DialogService(entry).ItemSearch(100, ref selectedItemID, ref selectedItemName, ItemSearchViewModeEnum.List, LSOne.DataLayer.BusinessObjects.ItemMaster.RetailItemSearchEnum.SpecialGroup, args[0], transaction, operationInfo) == DialogResult.OK)
            {
                PosTransaction posTransaction = (PosTransaction)transaction;

                if (posTransaction is InternalTransaction)
                {
                    IPosTransaction retailTransaction = new RetailTransaction(
                        (string)settings.Store.ID,
                        settings.Store.Currency,
                        settings.TaxIncludedInPrice);
                    retailTransaction.TransactionId = posTransaction.TransactionId;
                    ((PosTransaction)retailTransaction).TransactionIdNumberSequence = posTransaction.TransactionIdNumberSequence;
                    Services.TransactionService(entry).LoadTransactionStatus(entry, retailTransaction);
                    posTransaction = (PosTransaction)retailTransaction;
                }

                IPosTransaction itemSaleTransaction = posTransaction;
                settings.POSApp.RunOperation(POSOperations.ItemSale, selectedItemID, ref itemSaleTransaction);

                transaction = itemSaleTransaction;
            }

            return transaction;
        }
    }
}
