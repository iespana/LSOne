using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.SalesOrder
{
    /// <summary>
    /// A class that holds all the information needed to get sales order information from the 3rd party ERP system
    /// Can be customized and changed if needed
    /// </summary>
    public class SalesOrderRequest : DataEntity
    {
        //string salesId, decimal amount, string posID, string storeId, string transactionId, 
        /// <summary>
        /// Any information needed for the Sales order functionality. This property will never be used by LS One implementations
        /// </summary>
        public object Information { get; set; }
        /// <summary>
        /// The customer that the search should be made for
        /// </summary>
        public RecordIdentifier CustomerID {get; set;}
        

        public SalesOrderRequest() : base()
        {
            ID = RecordIdentifier.Empty;
            Text = "";
            Information = null;
            CustomerID = RecordIdentifier.Empty;                             
        }
    }
}
