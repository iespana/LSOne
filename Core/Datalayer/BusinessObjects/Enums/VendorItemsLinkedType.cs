using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// A vendor is linked to items if it is the default vendor for at least one item or has items assigned.
    /// </summary>
    public enum VendorItemsLinkedType: byte
    {
        /// <summary>
        /// Vendor is not linked to any item.
        /// </summary>
        None = 1,
        /// <summary>
        /// Vendor is the default vendor for at least one item.
        /// </summary>
        DefaultVendor = 2,
        /// <summary>
        /// Vendor has items assigned.
        /// </summary>
        VendorItems = 4,
        /// <summary>
        /// Vendor is the default vendor and has items assigned.
        /// </summary>
        DefaultVendorAndVendorItems = DefaultVendor & VendorItems
    }
}
