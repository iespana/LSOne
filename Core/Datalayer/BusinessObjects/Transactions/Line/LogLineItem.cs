using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Transactions.Line
{
    /// <summary>
    /// Represents one line on a LogTransaction
    /// </summary>    
    public class LogLineItem
    {
        /// <summary>
        /// The unique id of each line in the transaction
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// The log text associated with this log line
        /// </summary>
        public string LogText { get; set; }
    }
}
