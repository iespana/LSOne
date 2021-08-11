using LSOne.Services.Interfaces.SupportInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.TouchButtons
{
    class SessionHandler : ISession
    {
        public IPosEngine Engine
        {
            get
            {
                return null;
            }
        }

        public RecordIdentifier StoreID
        {
            get
            {
                return RecordIdentifier.Empty;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public RecordIdentifier TerminalID
        {
            get
            {
                return RecordIdentifier.Empty;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public RecordIdentifier UserLogin
        {
            get
            {
                return RecordIdentifier.Empty;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void AddLinkedItems(IConnectionManager dataModel, IPosTransaction transaction, int itemLineID, OperationInfo operationInfo = null)
        {
            
        }


        

        public void CalculateTotals(IConnectionManager dataModel)
        {
           
        }

        public ItemSaleCancelledEnum CheckUserReturnPermissions(IConnectionManager dataModel, ISaleLineItem currentItem, IPosTransaction currentTransaction, bool transactionAlreadyCalculated)
        {
            return ItemSaleCancelledEnum.None;
        }

        public void ConcludeTransaction(IConnectionManager dataModel, IPosTransaction transaction)
        {
            
        }

        public IPosTransaction CustomerSearch(IConnectionManager dataModel, IPosTransaction currentTransaction, string initialString)
        {
            return null;
        }

        public HostSettings GetHostSettings(IConnectionManager dataModel)
        {
            return null;
        }

        public ISettings GetSettings(IConnectionManager entry)
        {
            return null;
        }

        public bool InitialSaleLineConfigCheck(IConnectionManager dataModel, IRetailTransaction retailTransaction, ISaleLineItem saleLineItem, bool returnItems, ref ItemSaleCancelledEnum cancelReason)
        {
            return true;
        }

        public void OnPayment(IConnectionManager dataModel, IPosTransaction transaction)
        {
            
        }

        public PreTriggerResults PrePayCustomerAccount(IConnectionManager dataModel, IPosTransaction transaction, decimal tenderAmount)
        {
            throw new NotImplementedException();
        }

        public PreTriggerResults PrePayment(IConnectionManager dataModel, IPosTransaction transaction, POSOperations operation, string tenderID)
        {
            return new PreTriggerResults();
        }

        public PreTriggerResults PreLineDiscountAmount(IConnectionManager dataModel, IPosTransaction transaction, int lineId)
        {
            return new PreTriggerResults();
        }

        public void PostLineDiscountAmount(IConnectionManager entry, IPosTransaction transaction)
        {

        }

        public PreTriggerResults PreLineDiscountPercent(IConnectionManager dataModel, IPosTransaction transaction, int lineId)
        {
            return new PreTriggerResults();
        }

        public void PostLineDiscountPercent(IConnectionManager dataModel, IPosTransaction transaction)
        {
            
        }

        public PreTriggerResults PreProcessInfocode(IConnectionManager dataModel, IPosTransaction transaction, InfoCodeLineItem.TableRefId tableRefId)
        {
            return new PreTriggerResults();
        }

        public AddCustomerResultEnum SetCustomer(IConnectionManager dataModel, IPosTransaction currentTransaction, RecordIdentifier customerID)
        {
            return AddCustomerResultEnum.Success;
        }

        public PreTriggerResults PrePriceOverride(IConnectionManager entry, IPosTransaction posTransaction, int lineId)
        {
            return new PreTriggerResults();
        }

        public void PostPriceOverride(IConnectionManager dataModel, IPosTransaction transaction)
        {
            
        }

        public PreTriggerResults PreChangeUnitOfMeasure(IConnectionManager entry, IPosTransaction posTransaction, int lineId)
        {
            return new PreTriggerResults();
        }

        public void PostChangeUnitOfMeasure(IConnectionManager dataModel, IPosTransaction transaction)
        {
            
        }
    }
}
