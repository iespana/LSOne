using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Gets a list of ReasonCodes sorted by description.
        /// </summary>
        /// <param name="logonInfo">Logon info</param>
        /// <returns>Gets a list of ReasonCodes sorted by description</returns>
        [OperationContract]
        List<ReasonCode> GetReasonCodesList(LogonInfo logonInfo);

        /// <summary>
        /// Returns a list of reason codes filtered by the given parameters and sorted by description.
        /// </summary>
        /// <param name="logonInfo">Logon info</param>
        /// <param name="actions">Filtering criteria - list of reason actions. If null, it returns all reason codes, disregarding their action.</param>
        /// <param name="forPOS">Filtering criteria - if true returns only reason codes with SHOWONPOS = 1</param>
        /// <param name="open">Filtering criteria - if true returns only open reason codes</param>
        /// <returns>List of <see cref="ReasonCode"/> sorted by description</returns>
        [OperationContract(Name = "GetReasonCodesListFiltered")]
        List<ReasonCode> GetReasonCodesList(LogonInfo logonInfo,
                                            List<ReasonActionEnum> actions, bool? forPOS = null, bool open = true);

        /// <summary>
        /// Gets a list of ReasonCodes that are open, sorted by description.
        /// </summary>
        /// <param name="logonInfo">Logon info</param>
        /// <returns>Gets a list of open ReasonCodes sorted by description</returns>
        [OperationContract]
        List<ReasonCode> GetOpenReasonCodesList(LogonInfo logonInfo);

        /// <summary>
        /// Get a ReasonCode by id
        /// </summary>
        /// <param name="logonInfo">Logon info</param>
        /// <param name="id">Id of the reason code</param>
        /// <returns>Reason</returns>
        [OperationContract]
        ReasonCode GetReasonById(LogonInfo logonInfo, RecordIdentifier id);

        /// <summary>
        /// Search for reason codes
        /// </summary>
        /// <param name="logonInfo">Logon info</param>
        /// <param name="description">Description to search for</param>
        /// <param name="descriptionBeginsWith">True if the description should begin with the description parameter</param>
        /// <param name="action">Reason action for search for</param>
        /// <param name="beginDate">The start date of the reason</param>
        /// <param name="endDate">The end date of the reason</param>
        /// <param name="isSystemCode">True if the reasons should be system codes</param>
        /// <param name="sortBy">Sort enum to sort by</param>
        /// <param name="sortedBackwards">True if the reasons should be sorted backwards</param>
        /// <returns>A filtered list of reason codes</returns>
        [OperationContract]
        List<ReasonCode> SearchReasonList(LogonInfo logonInfo,
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
        /// <param name="logonInfo">Logon info</param>
        /// <param name="reasonID">ID of the reason to check for usage</param>
        /// <returns>True if the reason is used somewhere else false</returns>
        [OperationContract]
        bool ReasonCodeIsInUse(LogonInfo logonInfo, RecordIdentifier reasonID);

        /// <summary>
        /// Saves a reason code
        /// </summary>
        /// <param name="logonInfo">Logon info</param>
        /// <param name="reasonCode">Reason code to save</param>
        [OperationContract]
        void SaveReasonCode(LogonInfo logonInfo, ReasonCode reasonCode);

        /// <summary>
        /// Deletes a reason code
        /// </summary>
        /// <param name="logonInfo">Logon info</param>
        /// <param name="reasonId">Reason id to delete</param>
        [OperationContract]
        void DeleteReasonCode(LogonInfo logonInfo, RecordIdentifier reasonId);

        /// <summary>
        /// Returns a list of reasons codes that can be used to return items
        /// </summary>
        /// <param name="logonInfo">Logon info</param>
        /// <returns>Returns a list of reasons codes that can be used to return items</returns>
        [OperationContract]
        List<ReasonCode> GetReasonCodesForReturn(LogonInfo logonInfo);
    }
}
