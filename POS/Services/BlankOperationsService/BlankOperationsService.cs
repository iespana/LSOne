using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Operations;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSRetailPosis;

namespace LSOne.Services
{
    /// <summary>
    /// The Blank operation service allows the partner to create multiple operations that can be attached to a button on the user interface
    /// </summary>
    public partial class BlankOperationsService : IBlankOperationsService
    {

        /// <summary>
        /// The entry into the BlankOperation service. The buttons on the POS UI that have the operation Blank operation all call this function
        /// with the parameters that are set and from here any and all functions that should be run from these buttons should be run
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="session">The current session within the POS</param>
        /// <param name="operationInfo">The operation information</param>
        /// <param name="posTransaction">The current transaction. If the Blank operation button is the first button to be selected (i.e. no items have been added) then this
        /// transaction will be of the type InternalTransaction. If that is the case a RetailTransaction (or any other transaction needed) needs to be created and then returned back to the POS
        /// once the transaction is done</param>
        /// <returns>The tranasction that was being used or created during the running of the blank operation </returns>
        public virtual IPosTransaction BlankOperation(IConnectionManager entry, ISession session, OperationInfo operationInfo, IPosTransaction posTransaction)
        {
            string comment = "This operation has not been implemented."
                    + "\r\n\r\n"
                    + "Operation Id received: " + operationInfo.OperationId
                    + "\r\n"
                    + "Parameter received: " + operationInfo.Parameter;

            BlankOperation blankOperation = Providers.BlankOperationData.Get(entry, operationInfo.OperationId);
            
            Interfaces.Services.DialogService(entry).ShowMessage(comment, blankOperation != null ? blankOperation.OperationDescription : "", MessageBoxButtons.OK, MessageDialogType.Generic);
            posTransaction.KeepRowSelectionOnBlankOperation = true;
            return posTransaction;
        }

        /// <summary>
        /// Access to the error log functionality
        /// </summary>
        public virtual IErrorLog ErrorLog
        {
            set 
            { 
            
            }
        }

        /// <summary>
        /// Initializes the blank operation service and sets the database connection for the service
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        public void Init(IConnectionManager entry)
        {
            #pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            #pragma warning restore 0612, 0618
        }
    }
}

