using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Auditing;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces.SupportClasses
{
    public class PermissionInfo
    {
        public string GUID { get; private set; }
        public POSOperations OperationID { get; private set; }
        public OperationAuditEnum AuditSetting { get; private set; }
        public int LineID { get; private set; }
        public IPosTransaction Transaction { get; private set; }
        public bool LockPermissionOverride { get; set; }

        /// <summary>
        /// Create a permission info for query by guid only
        /// </summary>
        /// <param name="GUID">Permission guid</param>
        public PermissionInfo(string GUID)
        {
            this.GUID = GUID;
        }

        /// <summary>
        /// Create a permission info for query by guid and an operation
        /// </summary>
        /// <param name="GUID">Permission guid</param>
        /// <param name="operationID">Operation name</param>
        /// <param name="auditSetting">Audit settings for the operation</param>
        /// <param name="lineID">Current sale line item id</param>
        /// <param name="transaction">The current transaction</param>
        public PermissionInfo(string GUID, POSOperations operationID, OperationAuditEnum auditSetting, int lineID, IPosTransaction transaction)
            : this(GUID)
        {
            OperationID = operationID;
            AuditSetting = auditSetting;
            Transaction = transaction;
            LineID = lineID;
        }

        public PermissionInfo(string GUID, POSOperations operationID, OperationAuditEnum auditSetting, int lineID, 
            IPosTransaction transaction, bool lockPermissionOverride)
            : this(GUID, operationID, auditSetting, lineID, transaction)
        {
            LockPermissionOverride = lockPermissionOverride;
        }

        public void PerformAudit(IConnectionManager entry, string managerID)
        {
            if (Transaction.AuditingLines == null)
                Transaction.AuditingLines = new List<OperationAuditing>();

            var auditing = new OperationAuditing
            {
                TransactionID = Transaction.TransactionId,
                StoreID = entry.CurrentStoreID,
                TerminalID = entry.CurrentTerminalID,
                UserID = entry.CurrentUser.Text,
                ManagerID = managerID,
                OperationID = OperationID,
                CreatedDate = DateTime.Now,
                LineNum = LineID
            };

            Transaction.AuditingLines.Add(auditing);
        }
    }
}