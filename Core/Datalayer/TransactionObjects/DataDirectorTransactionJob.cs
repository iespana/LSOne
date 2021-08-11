using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionObjects
{
    public class DataDirectorTransactionJob : DataEntity
    {
        public DateTime CreatedTime { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public RecordIdentifier TerminalID { get; set; }
        public RecordIdentifier TransactionID { get; set; }
        public Guid JobID { get; set; }
        public bool IsNew { get; set; }

        public string Parameters
        {
            get
            {
                return (string)TransactionID + ";" + (string)TerminalID + ";" + (string)StoreID;
            }
            set
            {
                string[] parameters = value.Split(';');
                TransactionID = parameters[0];
                TerminalID = parameters[1];
                StoreID = parameters[2];
            }
        }
    }
}
