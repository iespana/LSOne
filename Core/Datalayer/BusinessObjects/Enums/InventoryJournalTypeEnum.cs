using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [DataContract(Name = "InventoryJournalTypeEnum")]
    public enum InventoryJournalTypeEnum
    {
        [EnumMember]
        Movement = 0,
        /// <summary>
        /// Inventory adjustment journal type.
        /// </summary>
        [EnumMember]
        Adjustment = 1,
        [EnumMember]
        Transfer = 2,
        [EnumMember]
        BOM = 3,
        [EnumMember]
        Counting = 4,
        [EnumMember]
        Project = 5,
        [EnumMember]
        TagCounting = 6,
        [EnumMember]
        Asset = 7,
        /// <summary>
        /// Stock counting journal type.
        /// </summary>
        [EnumMember]
        Reservation = 8,
        /// <summary>
        /// Parked / offline inventory journal type.
        /// </summary>
        [EnumMember]
        Parked = 9,
        /// <summary>
        /// Receiving (Goods receiving/Purchase order) journal type
        /// </summary>
        [EnumMember]
        Receiving = 10
    }
}
