using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services
{
    public partial class SiteServiceService
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
        public virtual List<SerialNumber> GetActiveSerialNumbersByItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching, bool closeConnection)
        {
            totalRecordsMatching = 0;

            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                return server.GetActiveSerialNumbersByItem(CreateLogonInfo(entry), itemMasterID, serialNumber, rowFrom, rowTo, sortBy, sortAscending, out totalRecordsMatching);
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
            finally
            {
                if (closeConnection)
                {
                    Disconnect(entry);
                }
            }
        }

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
        public virtual List<SerialNumber> GetSoldSerialNumbersByItem(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching, bool closeConnection)
        {
            totalRecordsMatching = 0;

            try
            {
                if (isClosed)
                {
                    Connect(entry, siteServiceProfile);
                }

                return server.GetSoldSerialNumbersByItem(CreateLogonInfo(entry), itemMasterID, serialNumber, rowFrom, rowTo, sortBy, sortAscending, out totalRecordsMatching);
            }
            catch (Exception)
            {
                isClosed = true;
                Disconnect(entry);
                throw;
            }
            finally
            {
                if (closeConnection)
                {
                    Disconnect(entry);
                }
            }
        }

        /// <summary>
        /// Marks the items in the list as being sold (used). Part of the transaction conclude.
        /// Items are marked as used, set if manually entered and set the receiptId.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="serialNumbers"></param>
        public virtual void UseSerialNumbers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<SerialNumber> serialNumbers)
        {
            if (!serialNumbers.Any())
            {
                return;
            }

            DoRemoteWork(entry, siteServiceProfile, () => server.UseSerialNumbers(CreateLogonInfo(entry), serialNumbers), true);
        }

        /// <summary>
        /// Serial number is marked as being reserved. If it is manually entered, then the serial number will be added to the database and marked as reserved.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="serialNumber"></param>
        public virtual void ReserveSerialNumber(IConnectionManager entry, SiteServiceProfile siteServiceProfile, SerialNumber serialNumber)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.ReserveSerialNumber(CreateLogonInfo(entry), serialNumber), true);
        }

        /// <summary>
        /// Get serial number by item master id and serial number
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="itemMasterID"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public virtual SerialNumber GetSerialNumber(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemMasterID, string serialNumber)
        {
            SerialNumber result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetSerialNumber(CreateLogonInfo(entry), itemMasterID, serialNumber), true);
            return result;
        }

        /// <summary>
        /// The list of serial numbers will be cleared. If they were reserved than this flag is removed, if they were manually entered than they will be deleted.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile"></param>
        /// <param name="serialNumbers"></param>
        public virtual void ClearReservedSerialNumbers(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<SerialNumber> serialNumbers)
        {
            if (!serialNumbers.Any())
            {
                return;
            }

            DoRemoteWork(entry, siteServiceProfile, () => server.ClearReservedSerialNumbers(CreateLogonInfo(entry), serialNumbers), true);
        }
    }
}
