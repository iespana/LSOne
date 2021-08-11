using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.TransactionObjects.EOD
{
    /// <summary>
    /// Represents a single customer deposit line from the POS. Is used in X/Z reports
    /// </summary>
    public class CustomerDepositLine
    {
        public RecordIdentifier Account { get; set; }
        public Name Name { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
    }
    
}
