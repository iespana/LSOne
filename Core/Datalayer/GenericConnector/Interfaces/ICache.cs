using System;
using System.Collections.Generic;

using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Localization;

namespace LSOne.DataLayer.GenericConnector.Interfaces
{
	public interface ICache : ICountryResolver
	{
        /// <summary>
        /// Returns a list of name prefixes such as Mr. Mrs. and etc.
        /// </summary>
        /// <returns></returns>
		List<string> GetNamePrefixes();

        /// <summary>
        /// Returns a list of countries
        /// </summary>
        /// <returns></returns>
		List<Country> GetCountries();

        /// <summary>
        /// Clears the cache that contains the system decimals settings
        /// </summary>
		void InvalidateSystemDecimalCache();

        /// <summary>
        /// Returns the cache for the system decimal settings
        /// </summary>
        /// <returns></returns>
		Dictionary<string, DecimalLimit> GetSystemDecimals();

        /// <summary>
        /// Clears the cache that is of a certain type; application, transaction, none
        /// </summary>
        /// <param name="cacheType"></param>
		void ClearEntityCache(CacheType cacheType);

        /// <summary>
        /// Invalidates the entire entity cache i.e. clears everything out
        /// </summary>
        void InvalidateEntityCache();

        /// <summary>
        /// Returns a specific item/entity from the cache
        /// </summary>
        /// <param name="type">The type of entity that is to be stored</param>
        /// <param name="id">The ID to be retrieved</param>
        /// <returns></returns>
		IDataEntity GetEntityFromCache(Type type, RecordIdentifier id);

        /// <summary>
        /// Add an entity to the cache
        /// </summary>
        /// <param name="id">The ID of the entity to be stored</param>
        /// <param name="entity">The entity that is being stored</param>
        /// <param name="cacheType">The cache type to store the entity in</param>
		void AddEntityToCache(RecordIdentifier id, IDataEntity entity, CacheType cacheType);
        /// <summary>
        /// Updats the entity in the cache
        /// </summary>
        /// <param name="entity">The updated entity to be stored</param>
		void UpdateEntityInCache(IDataEntity entity);
        /// <summary>
        /// Deletes a specific entity from the cache
        /// </summary>
        /// <param name="type">The type of entity that is to be deleted</param>
        /// <param name="id">The ID of the entity to be deleted</param>
		void DeleteEntityFromCache(Type type, RecordIdentifier id);
        /// <summary>
        /// Deletes all the cache for a specific type of entity
        /// </summary>
        /// <param name="type">The type of entity that is to be deleted</param>
		void DeleteTypeFromCache(Type type);

		/// <summary>
		/// Returns the states of the given country/>.
		/// </summary>
		/// <param name="countryID">Country ID.</param>
		/// <returns></returns>
		List<IDataEntity> GetStates(RecordIdentifier countryID);

		/// <summary>
		/// Returns the states of the given country. If they are not cached already they will be retrieved from database and cached.
		/// </summary>
		/// <param name="entry">Database connection</param>
		/// <param name="countryID">Country ID.</param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Country id is an invalid <see cref="RecordIdentifier"/>.</exception>
		List<IDataEntity> GetStates(IConnectionManager entry, RecordIdentifier countryID);

		/// <summary>
		/// Returns the state code (abbreviated name) for the given state name and country.
		/// </summary>
		/// <param name="countryID"></param>
		/// <param name="stateName"></param>
		/// <returns>A <see cref="RecordIdentifier"/> containing the state code (abbreviated name) or an empty <see cref="RecordIdentifier"/></returns>
		/// <exception cref="ArgumentNullException">Country id is invalid (null or empty <see cref="RecordIdentifier"/>).</exception>
		/// <exception cref="ArgumentNullException">State name is invalid (null or empty string).</exception>
		RecordIdentifier GetStateCode(RecordIdentifier countryID, string stateName);

        /// <summary>
        /// Get and sets the connection when the cache needs to retrieve information from the database
        /// </summary>
        IConnectionManager DataModel { get; set; }

    }
}
