using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        public virtual ReasonCode GetReasonById(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier id, bool closeConnection)
        {
            ReasonCode result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetReasonById(CreateLogonInfo(entry), id), closeConnection);
            return result;
        }

        public virtual List<ReasonCode> SearchReasonList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string description, bool descriptionBeginsWith, ReasonActionEnum? action, DateTime? beginDate, DateTime? endDate, bool? isSystemCode, ReasonCodeSorting sortBy, bool sortedBackwards, bool closeConnection)
        {
            List<ReasonCode> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchReasonList(CreateLogonInfo(entry), description, descriptionBeginsWith, action, beginDate, endDate, isSystemCode, sortBy, sortedBackwards), closeConnection);
            return result;
        }

        public virtual void DeleteReasonCode(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier id, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.DeleteReasonCode(CreateLogonInfo(entry), id), closeConnection);
        }

        public virtual List<ReasonCode> GetReasonCodesList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            List<ReasonCode> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetReasonCodesList(CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        /// <summary>
        /// Returns a list of reason codes filtered by the given parameters and sorted by description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="closeConnection">Close connection</param>
        /// <param name="actions">Filtering criteria - list of reason actions. If null, it returns all reason codes, disregarding their action.</param>
        /// <param name="forPOS">Filtering criteria - if true returns only reason codes with SHOWONPOS = 1</param>
        /// <param name="open">Filtering criteria - if true returns only open reason codes</param>
        /// <returns>List of <see cref="ReasonCode"/></returns>
        public virtual List<ReasonCode> GetReasonCodesList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection,
                                                            List<ReasonActionEnum> actions,
                                                            bool? forPOS = null, bool open = true)
        {
            List<ReasonCode> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetReasonCodesList(CreateLogonInfo(entry), actions, forPOS, open), closeConnection);
            return result;
        }

        public virtual List<ReasonCode> GetOpenReasonCodesList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            List<ReasonCode> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetOpenReasonCodesList(CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        public virtual void SaveReasonCode(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ReasonCode reason, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SaveReasonCode(CreateLogonInfo(entry), reason), closeConnection);
        }

        public virtual bool ReasonCodeIsInUse(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reasonID, bool closeConnection)
        {
            bool result = false;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.ReasonCodeIsInUse(CreateLogonInfo(entry), reasonID), closeConnection);
            return result;
        }

        public virtual List<ReasonCode> GetReasonCodesForReturn(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            List<ReasonCode> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetReasonCodesForReturn(CreateLogonInfo(entry)), closeConnection);
            return result;
        }
    }
}
