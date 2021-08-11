using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services
{
    public partial class InventoryService
    {
        public virtual ReasonCode GetReasonById(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier id, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetReasonById(entry, siteServiceProfile, id, closeConnection);
        }

        public virtual List<ReasonCode> SearchReasonList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, string description, bool descriptionBeginsWith, ReasonActionEnum? action, DateTime? beginDate, DateTime? endDate, bool? isSystemCode, ReasonCodeSorting sortBy, bool sortedBackwards, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).SearchReasonList(entry, siteServiceProfile, description, descriptionBeginsWith, action, beginDate, endDate, isSystemCode, sortBy, sortedBackwards, closeConnection);
        }

        public virtual void DeleteReasonCode(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier id, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).DeleteReasonCode(entry, siteServiceProfile, id, closeConnection);
        }

        public virtual List<ReasonCode> GetReasonCodesList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetReasonCodesList(entry, siteServiceProfile, closeConnection);
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
            return Interfaces.Services.SiteServiceService(entry).GetReasonCodesList(entry, siteServiceProfile, closeConnection, actions, forPOS, open);
        }

        public virtual List<ReasonCode> GetOpenReasonCodesList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetOpenReasonCodesList(entry, siteServiceProfile, closeConnection);
        }

        public virtual void SaveReasonCode(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ReasonCode reasonCode, bool closeConnection)
        {
            Interfaces.Services.SiteServiceService(entry).SaveReasonCode(entry, siteServiceProfile, reasonCode, closeConnection);
        }

		public virtual bool ReasonCodeIsInUse(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reasonID, bool closeConnection)
        {
			return Interfaces.Services.SiteServiceService(entry).ReasonCodeIsInUse(entry, siteServiceProfile, reasonID, closeConnection);
        }

        public virtual List<ReasonCode> GetReasonCodesForReturn(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection)
        {
            return Interfaces.Services.SiteServiceService(entry).GetReasonCodesForReturn(entry, siteServiceProfile, closeConnection);
        }
    }
}
