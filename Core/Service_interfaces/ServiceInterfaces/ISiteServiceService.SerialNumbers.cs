using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService : IService
    {
        /// <summary>
        ///  Get list of all serial numbers for a specific item master id. Only active serial numbers will be retrieved. Sold and reserved serial numbers will not be part of the list.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="itemMasterID"></param>
        /// <param name="serialNumber"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortAscending"></param>
        /// <param name="totalRecordsMatching"></param>
        /// <param name="closeConnection"></param>
        /// <returns></returns>
        List<SerialNumber> GetActiveSerialNumbersByItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching, bool closeConnection);

        /// <summary>
        ///  Get list of all sold serial numbers for a specific item master id.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="itemMasterID"></param>
        /// <param name="serialNumber"></param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortAscending"></param>
        /// <param name="totalRecordsMatching"></param>
        /// <param name="closeConnection"></param>
        /// <returns></returns>
        List<SerialNumber> GetSoldSerialNumbersByItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching, bool closeConnection);

        /// <summary>
        /// Marks the items in the list as being sold (used). Part of the transaction conclude.
        /// Items are marked as used, set if manually entered and set the receiptId.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="serialNumbers"></param>
        void UseSerialNumbers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<SerialNumber> serialNumbers);
        
        /// <summary>
        /// Serial number is marked as being reserved. If it is manually entered, then the serial number will be added to the database and marked as reserved.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="serialNumber"></param>
        void ReserveSerialNumber(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SerialNumber serialNumber);

        /// <summary>
        /// Get serial number by item master id and serial number
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="itemMasterID"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        SerialNumber GetSerialNumber(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemMasterID, string serialNumber);

        /// <summary>
        /// The list of serial numbers will be cleared. If they were reserved than this flag is removed, if they were manually entered than they will be deleted.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="serialNumbers"></param>
        void ClearReservedSerialNumbers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<SerialNumber> serialNumbers);
    }
}
