using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
   
    public interface ISession
    {
        IPosEngine Engine
        {
            get;
        }

#region --- Trigger launchers ----

        PreTriggerResults PreLineDiscountAmount(IConnectionManager dataModel, IPosTransaction transaction, int lineId);        
        void PostLineDiscountAmount(IConnectionManager dataModel, IPosTransaction transaction);        

        PreTriggerResults PreLineDiscountPercent(IConnectionManager dataModel, IPosTransaction transaction, int lineId);

        void PostLineDiscountPercent(IConnectionManager dataModel, IPosTransaction transaction);

        PreTriggerResults PreProcessInfocode(IConnectionManager dataModel, IPosTransaction transaction, InfoCodeLineItem.TableRefId tableRefId);

        PreTriggerResults PrePayment(IConnectionManager dataModel, IPosTransaction transaction, POSOperations operation, string tenderID);

        void OnPayment(IConnectionManager dataModel, IPosTransaction transaction);

        PreTriggerResults PrePayCustomerAccount(IConnectionManager dataModel, IPosTransaction transaction, decimal tenderAmount);

        PreTriggerResults PrePriceOverride(IConnectionManager entry, IPosTransaction posTransaction, int lineId);

        void PostPriceOverride(IConnectionManager dataModel, IPosTransaction transaction);

        PreTriggerResults PreChangeUnitOfMeasure(IConnectionManager entry, IPosTransaction posTransaction, int lineId);

        void PostChangeUnitOfMeasure(IConnectionManager dataModel, IPosTransaction transaction);
        #endregion --- Trigger launchers ----


        void CalculateTotals(IConnectionManager dataModel);

        void ConcludeTransaction(IConnectionManager dataModel, IPosTransaction transaction);

        //bool InitialSaleLineConfigCheck(IConnectionManager dataModel, IRetailTransaction retailTransaction, ISaleLineItem saleLineItem, bool returnItems, ref ItemSaleCancelledEnum cancelReason);

        ItemSaleCancelledEnum CheckUserReturnPermissions(IConnectionManager dataModel, ISaleLineItem currentItem, IPosTransaction currentTransaction, bool transactionAlreadyCalculated);

        AddCustomerResultEnum SetCustomer(IConnectionManager dataModel, IPosTransaction currentTransaction, RecordIdentifier customerID);


        IPosTransaction CustomerSearch(IConnectionManager dataModel, IPosTransaction currentTransaction, string initialSearch);

        HostSettings GetHostSettings(IConnectionManager dataModel);

        /// <summary>
        /// The login of the user (i.e "admin")
        /// </summary>
        RecordIdentifier UserLogin { get; set; }

        /// <summary>
        /// The ID of the terminal this session is for
        /// </summary>
        RecordIdentifier TerminalID { get; set; }

        /// <summary>
        /// The ID of the store this session is for
        /// </summary>
        RecordIdentifier StoreID { get; set; }

        /// <summary>
        /// Gets the current settings for this session based on the user, store and terminal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        ISettings GetSettings(IConnectionManager entry);
    }
}
