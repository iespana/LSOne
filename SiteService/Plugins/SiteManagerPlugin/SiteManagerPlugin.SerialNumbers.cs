using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        /// <summary>
        /// Gets all active serial numbers for an item. A filter can be applied for serial numbers containing a specific description.
        /// Active serial numbers are those that were not manually entered and are not sold nor reserved
        /// </summary>
        /// <param name="logonInfo">>The login information for the database</param>
        /// <param name="itemMasterID">Item Master ID</param>
        /// <param name="serialNumber">serial number used for filter</param>
        /// <param name="rowFrom">first row</param>
        /// <param name="rowTo">last row</param>
        /// <param name="sortBy">sort by column</param>
        /// <param name="sortAscending">if true sort ascending, otherwise sort descending</param>
        /// <param name="totalRecordsMatching">output the total number of records</param>
        public virtual List<SerialNumber> GetActiveSerialNumbersByItem(LogonInfo logonInfo, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemMasterID)}: {itemMasterID}, {nameof(serialNumber)}: {serialNumber}");
                totalRecordsMatching = 0;
                return Providers.SerialNumberData.GetActiveSerialNumbersByItem(dataModel, itemMasterID, serialNumber, rowFrom, rowTo, sortBy, sortAscending, out totalRecordsMatching);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

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
        public virtual List<SerialNumber> GetSoldSerialNumbersByItem(LogonInfo logonInfo, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemMasterID)}: {itemMasterID}, {nameof(serialNumber)}: {serialNumber}");
                totalRecordsMatching = 0;
                return Providers.SerialNumberData.GetSoldSerialNumbersByItem(dataModel, itemMasterID, serialNumber, rowFrom, rowTo, sortBy, sortAscending, out totalRecordsMatching);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Serial number is marked as being reserved. If it is manually entered, then the serial number will be added to the database and marked as reserved.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="serialNumber"></param>
        public virtual void ReserveSerialNumber(LogonInfo logonInfo, SerialNumber serialNumber)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(serialNumber)}.ID: {serialNumber.ID}");
                Providers.SerialNumberData.Reserve(dataModel, serialNumber);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Get serial number by item master id and serial number
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="itemMasterID"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public virtual SerialNumber GetSerialNumber(LogonInfo logonInfo, RecordIdentifier itemMasterID, string serialNumber)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(itemMasterID)}: {itemMasterID}, {nameof(serialNumber)}: {serialNumber}");
                return Providers.SerialNumberData.GetByItemAndSerialNumber(dataModel, itemMasterID, serialNumber);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// The list of serial numbers will be cleared. If they were reserved than this flag is removed, if they were manually entered than they will be deleted.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="serialNumbers"></param>
        public virtual void ClearReservedSerialNumbers(LogonInfo logonInfo, List<SerialNumber> serialNumbers)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.SerialNumberData.ClearReserve(dataModel, serialNumbers);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }

        /// <summary>
        /// Marks the items in the list as being sold (used). Part of the transaction conclude.
        /// If one of the items has an already used serial number, the operation will be aborded and the list of the items + serial number that are already used are retrived.
        /// Items are marked as used, set if manually entered and set the receiptId.
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="serialNumbers">The list of serial numbers to be verified and updated</param>
        /// <returns>All the items that have specific serial numbers already used</returns>
        public virtual void UseSerialNumbers(LogonInfo logonInfo, List<SerialNumber> serialNumbers)
        {
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            try
            {
                Utils.Log(this, Utils.Starting);
                Providers.SerialNumberData.UseSerialNumbers(dataModel, serialNumbers);
            }
            catch (Exception e)
            {
                Utils.LogException(this, e);
                throw GetChannelError(e);
            }
            finally
            {
                ReturnConnection(dataModel, out dataModel);
                Utils.Log(this, Utils.Done);
            }
        }
    }
}