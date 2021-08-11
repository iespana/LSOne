using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace LSRetail.SiteService.SiteServiceInterface
{
    partial interface ISiteService
    {
        /// <summary>
        /// Gets all active serial numbers for an item. A filter can be applied for serial numbers containing a specific description.
        /// Active serial numbers are those that were not manually entered and are not sold nor reserved.
        /// </summary>
        /// <param name="logonInfo">>The login information for the database</param>
        /// <param name="itemMasterID">Item Master ID</param>
        /// <param name="serialNumber">serial number used for filter</param>
        /// <param name="rowFrom">first row</param>
        /// <param name="rowTo">last row</param>
        /// <param name="sortBy">sort by column</param>
        /// <param name="sortAscending">if true sort ascending, otherwise sort descending</param>
        /// <param name="totalRecordsMatching">output the total number of records</param>
        /// <returns></returns>
        [OperationContract]
        List<SerialNumber> GetActiveSerialNumbersByItem(LogonInfo logonInfo, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching);

        /// <summary>
        /// Gets all sold serial numbers for an item.
        /// </summary>
        /// <param name="logonInfo">>The login information for the database</param>
        /// <param name="itemMasterID">Item Master ID</param>
        /// <param name="serialNumber">serial number used for filter</param>
        /// <param name="rowFrom">first row</param>
        /// <param name="rowTo">last row</param>
        /// <param name="sortBy">sort by column</param>
        /// <param name="sortAscending">if true sort ascending, otherwise sort descending</param>
        /// <param name="totalRecordsMatching">output the total number of records</param>
        /// <returns></returns>
        [OperationContract]
        List<SerialNumber> GetSoldSerialNumbersByItem(LogonInfo logonInfo, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching);

        /// <summary>
        /// Marks the items in the list as being sold (used). Part of the transaction conclude.
        /// Items are marked as used, set if manually entered and set the receiptId.
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="serialNumbers">The list of serial numbers to be verified and updated</param>
        /// <returns>All the items that have specific serial numbers already used</returns>
        [OperationContract]
        void UseSerialNumbers(LogonInfo logonInfo, List<SerialNumber> serialNumbers);

        /// <summary>
        /// Serial number is marked as being reserved. If it is manually entered, then the serial number will be added to the database and marked as reserved.
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="serialNumber"></param>
        [OperationContract]
        void ReserveSerialNumber(LogonInfo logonInfo, SerialNumber serialNumber);

        /// <summary>
        /// Get serial number by item master id and serial number
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="itemMasterID"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        [OperationContract]
        SerialNumber GetSerialNumber(LogonInfo logonInfo, RecordIdentifier itemMasterID, string serialNumber);

        /// <summary>
        /// The list of serial numbers will be cleared. If they were reserved than this flag is removed, if they were manually entered than they will be deleted.
        /// </summary>
        /// <param name="logonInfo"></param>
        /// <param name="serialNumbers"></param>
        [OperationContract]
        void ClearReservedSerialNumbers(LogonInfo logonInfo, List<SerialNumber> serialNumbers);
    }
}
