using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.Inventory
{
    public interface IReasonCodeData : IDataProvider<ReasonCode>, ISequenceable
    {
        /// <summary>
        /// Gets a list of ReasonCodes sorted by description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of ReasonCodes sorted by description</returns>
        List<ReasonCode> GetList(IConnectionManager entry);

        /// <summary>
        /// Returns a list of reason codes filtered by the given parameters and sorted by description.
        /// </summary>
        /// <param name="entry">The entry into the database></param>
        /// <param name="actions">Filtering criteria - list of reason actions. If null, it returns all reason codes, disregarding their action.</param>
        /// <param name="forPOS">Filtering criteria - if true returns only reason codes with SHOWONPOS = 1</param>
        /// <param name="open">Filtering criteria - if true returns only open reason codes</param>
        /// <returns>List of <see cref="ReasonCode"/></returns>
        List<ReasonCode> GetList(IConnectionManager entry, List<ReasonActionEnum> actions, bool? forPOS = null, bool open = true);

        /// <summary>
        /// Gets a list of ReasonCodes that are open, sorted by description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of open ReasonCodes sorted by description</returns>
        List<ReasonCode> GetOpenReasonCodesList(IConnectionManager entry);

        /// <summary>
        /// Get a ReasonCode by id
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="id">Id of the reason code</param>
        /// <returns>Reason</returns>
        ReasonCode GetReasonById(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Search for reason codes
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="description">Description to search for</param>
        /// <param name="descriptionBeginsWith">True if the description should begin with the description parameter</param>
        /// <param name="action">Reason action for search for</param>
        /// <param name="beginDate">The start date of the reason</param>
        /// <param name="endDate">The end date of the reason</param>
        /// <param name="isSystemCode">True if the reasons should be system codes</param>
        /// <param name="sortBy">Sort enum to sort by</param>
        /// <param name="sortedBackwards">True if the reasons should be sorted backwards</param>
        /// <returns>A filtered list of reason codes</returns>
        List<ReasonCode> SearchReasonList(IConnectionManager entry,
                                          string description,
                                          bool descriptionBeginsWith,
                                          ReasonActionEnum? action,
                                          DateTime? beginDate,
                                          DateTime? endDate,
                                          bool? isSystemCode,
                                          ReasonCodeSorting sortBy,
                                          bool sortedBackwards);

        /// <summary>
        /// Returns true if the reason with the given id is in use
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="reasonID">ID of the reason to check for usage</param>
        /// <returns>True if the reason is used somewhere else false</returns>
        bool ReasonCodeIsInUse(IConnectionManager entry, RecordIdentifier reasonID);

        /// <summary>
        /// Returns a list of reasons codes that can be used to return items
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>Returns a list of reasons codes that can be used to return items</returns>
        List<ReasonCode> GetReasonCodesForReturn(IConnectionManager entry);
    }
}
