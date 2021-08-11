using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum TypeOfSerial
    {
        /// <summary>
        /// Serial number type
        /// </summary>
        SerialNumber = 0,
        /// <summary>
        /// RFID tag type
        /// </summary>
        RFIDTag = 1
    }

    /// <summary>
    /// A enum that defines sorting for the serial numbers
    /// </summary>
    public enum SerialNumberSorting
    {
        /// <summary>
        /// Sort by Item ID
        /// </summary>
        ItemID,
        /// <summary>
        /// Sort by item description
        /// </summary>
        ItemDescription,
        /// <summary>
        /// Sort by item variant description
        /// </summary>
        ItemVariant,
        /// <summary>
        /// Sort by serial number
        /// </summary>
        SerialNumber,
        /// <summary>
        /// Sort by serial type
        /// </summary>
        TypeOfSerial,
        /// <summary>
        /// Sort by sold date
        /// </summary>
        Sold,
        /// <summary>
        /// Sort by reference
        /// </summary>
        Reference,
        /// <summary>
        /// Sort by manual entry
        /// </summary>
        ManualEntry
    };
}
