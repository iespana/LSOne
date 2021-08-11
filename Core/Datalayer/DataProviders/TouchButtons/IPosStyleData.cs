using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;
using static LSOne.DataLayer.BusinessObjects.Hospitality.RestaurantDiningTable;
using LSOne.Utilities.Enums;
using System.Security;

namespace LSOne.DataLayer.DataProviders.TouchButtons
{
    public interface IPosStyleData : IDataProvider<PosStyle>, ISequenceable
    {
        /// <summary>
        /// Gets a styleProfile with a particular name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="name">The style name</param>
        /// <returns>A StyleProfile</returns>
        PosStyle GetByName(IConnectionManager entry, string name);

        /// <summary>
        /// Gets a styleProfile with a particular name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="guid">The guid of the style</param>
        /// <returns>A StyleProfile</returns>
        PosStyle GetByGuid(IConnectionManager entry, Guid guid);

        /// <summary>
        /// Returns true if the GUID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="guid">The GUID to check</param>
        /// <returns>True if the GUID exists otherwise false is returned</returns>
        bool GuidExists(IConnectionManager entry, Guid guid);

        /// <summary>
        /// Gets a list of all style profiles
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">How the list should by sorted. If empty then no specific sorting is applied</param>
        /// <param name="cache">The type of cache that should be used for this list</param>
        /// <returns>A list of all style profiles</returns>
        List<PosStyle> GetList(IConnectionManager entry, string sort = "", CacheType cache = CacheType.CacheTypeNone);

        PosStyle Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone);

        List<PosStyle> GetListByFilters(IConnectionManager entry, string description, bool beginsWith, StyleType? selectedType, Utilities.Enums.ShapeEnum? selectedShape, GradientModeEnum? selectedGradient, string sort = "", CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Runs an unsecured stored procedure to retrieves a POS style from the database
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="name"></param>
        /// <param name="dataSource"></param>
        /// <param name="windowsAuthentication"></param>
        /// <param name="sqlServerLogin"></param>
        /// <param name="sqlServerPassword"></param>
        /// <param name="databaseName"></param>
        /// <param name="connectionType"></param>
        /// <param name="dataAreaID"></param>
        /// <returns></returns>
        PosStyle GetByNameUnsecure(IConnectionManager entry, string name, string dataSource, bool windowsAuthentication, string sqlServerLogin, SecureString sqlServerPassword, string databaseName, ConnectionType connectionType, string dataAreaID);
    }
}