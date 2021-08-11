using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Fiscal
{
    public class FiscalTrans : DataEntity
    {
        public FiscalTrans()
            : base()
        {
            ReceiptId = "";
            TransactionId = "";
            Store = "";
            Terminal = "";
            FiscalUnitId = "";
            FiscalControlId = "";
            DataAreaId = "";
            ReplicationCounter = 0;
            Replicated = 0;
            Type = 0;
            TransDate = new DateTime();
            GrossAmount = decimal.Zero;
            NetAmount = decimal.Zero;
            PrivateKeyVersion = "";
            Signature = "";
            EmployeeID = "";
            EmployeeDescription = "";
            Currency = "";
            StoreDescription = "";
            TerminalDescription = "";
        }

        public string ReceiptId { get; set; }
        public string TransactionId { get; set; }
        public string Store { get; set; }
        public string Terminal { get; set; }
        public string FiscalUnitId { get; set; }
        public string FiscalControlId { get; set; }
        public string DataAreaId { get; set; }
        public int ReplicationCounter { get; set; }
        public int Replicated { get; set; }

        public int Type { get; set; }
        public DateTime TransDate { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string PrivateKeyVersion { get; set; }
        public string Signature { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeDescription { get; set; }
        public string StoreDescription { get; set; }
        public string Currency { get; set; }
        public string TerminalDescription { get; set; }   
    }
}
