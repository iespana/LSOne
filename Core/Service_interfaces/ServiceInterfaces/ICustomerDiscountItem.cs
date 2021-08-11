using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSRetail.StoreController.SharedDatabase.Interfaces;

namespace LSRetail.ServiceInterfaces
{
    public enum CustomerDiscountTypes
    {
        LineDiscount = 0,
        MultiLineDiscount = 1,
        TotalDiscount = 2
    }
    public interface ICustomerDiscountItem : ILineDiscountItem
    {
        CustomerDiscountTypes CustomerDiscountType { get; set; }

    }
}
