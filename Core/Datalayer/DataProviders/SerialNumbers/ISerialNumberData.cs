using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.SerialNumbers
{
    public interface ISerialNumberData : IDataProvider<SerialNumber>, ISequenceable
    {
        /// <summary>
        /// Get list of all serial number instances
        /// </summary>
        /// <param name="entry">connection</param>
        /// <returns></returns>
        List<SerialNumber> GetList(IConnectionManager entry);
        /// <summary>
        /// Get list of serial number instances that are filter based on serial number filter
        /// </summary>
        /// <param name="entry">connection</param>
        /// <param name="filter">serial number filter</param>
        /// <param name="itemsCount">will return the total number of items</param>
        /// <returns></returns>
        List<SerialNumber> GetListByFilter(IConnectionManager entry, SerialNumberFilter filter, out int itemsCount);
        /// <summary>
        /// Get one serial number by id
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="ID">serial number id</param>
        /// <returns></returns>
        SerialNumber Get(IConnectionManager entry, RecordIdentifier ID);

        /// <summary>
        /// Saves a serial number to the database
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="serialNumber"></param>
        /// <param name="excelImport">If excel import, then in case of existing serial number (based on Item Id and Serial No, only the serial type is being updated)</param>
        /// <returns>Returns false if item and serial number already exist</returns>
        bool Save(IConnectionManager entry, SerialNumber serialNumber, bool excelImport = false);

        /// <summary>
        /// Get a serial number item for a specific item and serial number
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="itemMasterID">item master id</param>
        /// <param name="serialNumber">serial number</param>
        /// <returns></returns>
        SerialNumber GetByItemAndSerialNumber(IConnectionManager entry, RecordIdentifier itemMasterID, string serialNumber);

        /// <summary>
        /// Gets all active serial numbers for a specific item. Active serial numbers = not manually entered and not sold
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="itemMasterID"> Item master id</param>
        /// <param name="rowForm">start from row</param>
        /// <param name="rowTo">to row</param>
        /// <param name="sortBy">sort by specific column</param>
        /// <param name="sortAscending">if true sort ascending, otherwise descending </param>
        /// <param name="totalRecordsMatching">the total number of records</param>
        /// <returns></returns>
        List<SerialNumber> GetActiveSerialNumbersByItem(IConnectionManager entry, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching);

        /// <summary>
        /// Gets all sold serial numbers for a specific item.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="itemMasterID"> Item master id</param>
        /// <param name="rowForm">start from row</param>
        /// <param name="rowTo">to row</param>
        /// <param name="sortBy">sort by specific column</param>
        /// <param name="sortAscending">if true sort ascending, otherwise descending </param>
        /// <param name="totalRecordsMatching">the total number of records</param>
        /// <returns></returns>
        List<SerialNumber> GetSoldSerialNumbersByItem(IConnectionManager entry, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching);

        /// <summary>
        /// Marks the items in the list as being sold (used). Part of the transaction conclude.
        /// If one of the items has an already used serial number, the operation will be aborded and the list of the items + serial number that are already used are retrived.
        /// Items are marked as used, set if manually entered and set the receiptId.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="serialNumbers">The list of serial numbers to be verified and updated</param>
        /// <returns>All the items that have specific serial numbers already used</returns>
        void UseSerialNumbers(IConnectionManager entry, List<SerialNumber> serialNumbers);

        void Reserve(IConnectionManager entry, SerialNumber serialNumber);
        void ClearReserve(IConnectionManager entry, List<SerialNumber> serialNumbers);
    }
}
