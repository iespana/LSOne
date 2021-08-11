using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;

namespace LSOne.Services.Interfaces
{
    public partial interface IInventoryService
    {
        /// <summary>
        /// Gets a list of ReasonCodes sorted by description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="closeConnection">Close connection</param>
        /// <returns>Gets a list of ReasonCodes sorted by description</returns>
        List<ReasonCode> GetReasonCodesList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

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
        List<ReasonCode> GetReasonCodesList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection,
                                            List<ReasonActionEnum> actions, bool? forPOS = null, bool open = true);

        /// <summary>
        /// Gets a list of ReasonCodes that are open, sorted by description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="closeConnection">Close connection</param>
        /// <returns>Gets a list of open ReasonCodes sorted by description</returns>
        List<ReasonCode> GetOpenReasonCodesList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);

        /// <summary>
        /// Get a ReasonCode by id
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="id">Id of the reason code</param>
        /// <param name="closeConnection">Close connection</param>
        /// <returns>Reason</returns>
        ReasonCode GetReasonById(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier id, bool closeConnection);

        /// <summary>
        /// Search for reason codes
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="description">Description to search for</param>
        /// <param name="descriptionBeginsWith">True if the description should begin with the description parameter</param>
        /// <param name="action">Reason action for search for</param>
        /// <param name="beginDate">The start date of the reason</param>
        /// <param name="endDate">The end date of the reason</param>
        /// <param name="isSystemCode">True if the reasons should be system codes</param>
        /// <param name="sortBy">Sort enum to sort by</param>
        /// <param name="sortedBackwards">True if the reasons should be sorted backwards</param>
        /// <param name="closeConnection">Close connection</param>
        /// <returns>A filtered list of reason codes</returns>
        List<ReasonCode> SearchReasonList(IConnectionManager entry, 
                                          SiteServiceProfile siteServiceProfile,
                                          string description,
                                          bool descriptionBeginsWith,
                                          ReasonActionEnum? action,
                                          DateTime? beginDate,
                                          DateTime? endDate,
                                          bool? isSystemCode,
                                          ReasonCodeSorting sortBy,
                                          bool sortedBackwards, 
                                          bool closeConnection);

        /// <summary>
        /// Returns true if the reason with the given id is in use
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="reasonID">ID of the reason to check for usage</param>
        /// <param name="closeConnection">Close connection</param>
        /// <returns>True if the reason is used somewhere else false</returns>
        bool ReasonCodeIsInUse(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reasonID, bool closeConnection);

        /// <summary>
        /// Saves a reason code
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="reasonCode">Reason code to save</param>
        /// <param name="closeConnection">Close connection</param>
        void SaveReasonCode(IConnectionManager entry, SiteServiceProfile siteServiceProfile, ReasonCode reasonCode, bool closeConnection);

        /// <summary>
        /// Deletes a reason code
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="reasonId">Reason id to delete</param>
        /// <param name="closeConnection">Close connection</param>
        void DeleteReasonCode(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier reasonId, bool closeConnection);

        /// <summary>
        /// Returns a list of reasons codes that can be used to return items
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="siteServiceProfile">Site service profile</param>
        /// <param name="closeConnection">Close connection</param>
        /// <returns>Returns a list of reasons codes that can be used to return items</returns>
        List<ReasonCode> GetReasonCodesForReturn(IConnectionManager entry, SiteServiceProfile siteServiceProfile, bool closeConnection);
    }
}
