using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.TransactionObjects.Line.CustomerOrder;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    public class DepositTransaction : RetailTransaction, IDepositTransaction
    {
        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.Deposit;
        }

        
        public DepositTransaction(RecordIdentifier storeID, RecordIdentifier storeCurrencyCode, bool taxIncludedInPrice)
            : base((string) storeID, (string) storeCurrencyCode, taxIncludedInPrice)
        {
            Initialize();

        }

        private void Initialize()
        {
            
        }

        public override object Clone()
        {
            DepositTransaction transaction = new DepositTransaction(StoreId, StoreCurrencyCode, TaxIncludedInPrice);
            base.Populate(transaction);
            return transaction;
        }

        public DepositTransaction Clone(RetailTransaction retailTransaction)
        {
            DepositTransaction transaction = new DepositTransaction(retailTransaction.StoreId, retailTransaction.StoreCurrencyCode, retailTransaction.TaxIncludedInPrice);
            transaction.ToClass(retailTransaction.ToXML());

            transaction.SaleItems.Clear();
            transaction.NetAmountWithTax = transaction.Payment;
            transaction.NetAmount = decimal.Zero;

            return transaction;
        }

        protected void Populate(DepositTransaction transaction)
        {
            
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            return base.ToXML(errorLogger);
        }

        public override void ToClass(XElement xmlTrans, IErrorLog errorLogger = null)
        {
            if (xmlTrans.HasElements)
            {
                base.ToClass(xmlTrans, errorLogger);
            }
        }
    }
}
