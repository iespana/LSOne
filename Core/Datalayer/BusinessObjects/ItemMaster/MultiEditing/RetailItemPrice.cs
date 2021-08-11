using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing
{
    public class RetailItemPrice : DataEntity
    {
        public RecordIdentifier SalesTaxItemGroupID;
        public decimal SalesPrice;
        public decimal SalesPriceIncludingTax;
        public decimal ProfitMargin;
        public decimal PurchasePrice;
    }
}
