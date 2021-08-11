using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.LookupValues
{
    public interface IPaymentLimitationsData : IDataProviderBase<PaymentMethodLimitation>
    {
        /// <summary>
        /// Returns a list of limitations for a specific tender / payment method
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tenderID">The tender ID</param>
        /// <returns>A list of limitations</returns>
        List<PaymentMethodLimitation> GetListForTender(IConnectionManager entry, RecordIdentifier tenderID);

        /// <summary>
        /// Returns a limitation list for a specific list of retail items, retail groups, special groups and retail departments
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItemIDs">A list of retail item ID to provide the limitation list for</param>
        /// <param name="retailGroupIDs">A list of retail group ID to provide the limitation list for</param>
        /// <param name="specialGroupIDs">A list of special group ID to provide the limitation list for</param>
        /// <param name="retailDepartmentIDs">A list of retail department ID to provide the limitation list for</param>
        /// <returns></returns>
        List<PaymentMethodLimitation> GetList(IConnectionManager entry, List<string> retailItemIDs, List<string> retailGroupIDs, List<string> specialGroupIDs, List<string> retailDepartmentIDs);

        /// <summary>
        /// Returns a list of restriction codes that are attached to a specific tender
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tenderID">The tender ID</param>
        /// <returns>A list of reason codes</returns>
        List<DataEntity> GetRestrictionCodeListForTender(IConnectionManager entry, RecordIdentifier tenderID);

        /// <summary>
        /// Gets a list of restriction codes for a given tender ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tenderID">The ID of the tender type</param>
        /// <returns></returns>
        List<PaymentMethodLimitationRestrictionCode> GetRestrictionCodesForTender(IConnectionManager entry, RecordIdentifier tenderID);

        /// <summary>
        /// Returns a limitation list for a specific restriction code and tender id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tenderID">The tender ID</param>
        /// <param name="reasonCode">The limitation master ID</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns></returns>
        List<PaymentMethodLimitation> GetListForRestrictionCode(IConnectionManager entry, RecordIdentifier reasonCode, RecordIdentifier tenderID, CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns the limitation master ID for a specific restriction code
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restrictionCode">The restriction code being checked</param>
        /// <returns></returns>
        RecordIdentifier GetLimitationMasterID(IConnectionManager entry, RecordIdentifier restrictionCode);

        /// <summary>
        /// Returns a specific limitation line. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="ID">The limitation Master ID</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>A specific limitation line</returns>
        PaymentMethodLimitation Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone);
        /// <summary>
        /// Returns true if a specific limitation line exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The limitation Master ID that is to be checked</param>
        /// <returns>True if the line exists</returns>
        bool Exists(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Returns true if a specific limitation line exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="tenderID">The tender ID to be checked</param>
        /// <param name="restrictionCode">The restriction code to be checked</param>
        /// <param name="type">The limitation type to be checked</param>
        /// <param name="relationMasterID">The relation master ID (depends on the Type) to be checked</param>
        /// <returns></returns>
        bool Exists(IConnectionManager entry, RecordIdentifier tenderID, RecordIdentifier restrictionCode, LimitationType type, RecordIdentifier relationMasterID);

        /// <summary>
        /// Returns true if the specific restriction code already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restrictionCode">The code to be checked</param>
        /// <returns></returns>
        bool RestrictionCodeExists(IConnectionManager entry, RecordIdentifier restrictionCode);

        /// <summary>
        /// Deletes a specific limitation line.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The limitation Master ID that is to be deleted</param>
        void Delete(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Saves a payment limitation. If the limitation Master ID already exists the line is updated. If the Master ID is empty a new ID is created
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="limitation">The information that is to be saved</param>
        void Save(IConnectionManager entry, PaymentMethodLimitation limitation);

        /// <summary>
        /// Sets the tax exempt status for a given limitation and restriction code. This updates all lines for the given limitation and restriction
        /// code combination.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="limitationMasterID">The master ID of the limitation to set the status for</param>
        /// <param name="restrictionCode">The restriction code to set the tax exempt status for</param>
        /// <param name="taxExempt">The tax exempt status to set</param>        
        void SetTaxExemptStatus(IConnectionManager entry, RecordIdentifier limitationMasterID, RecordIdentifier restrictionCode, bool taxExempt);
    }
}