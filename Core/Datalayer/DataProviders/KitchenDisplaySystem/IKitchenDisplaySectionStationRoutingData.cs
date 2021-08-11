using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplaySectionStationRoutingData : IDataProvider<KitchenDisplaySectionStationRouting>
    {
        /// <summary>
        /// Get the section station routing with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the section station routing</param>
        /// <returns>The section station routing with the given ID</returns>
        KitchenDisplaySectionStationRouting Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get a list of all section station routing
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all section station routings</returns>
        List<KitchenDisplaySectionStationRouting> GetList(IConnectionManager entry);

        /// <summary>
        /// Checks if a section station routing already exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="routing">The section station routing to check if exists</param>
        /// <returns>True if the section station routing already exists</returns>
        bool Exists(IConnectionManager entry, KitchenDisplaySectionStationRouting routing);

        /// <summary>
        /// Remove a production section from section station routings
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sectionId">ID of the production section to remove</param>
        void RemoveSection(IConnectionManager entry, RecordIdentifier sectionId);

        /// <summary>
        /// Remove a restaurant from section station routings
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantId">ID of the restaurant to remove</param>
        void RemoveRestaurant(IConnectionManager entry, RecordIdentifier restaurantId);

        /// <summary>
        /// Remove a hospitality type from section station routings
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hosTypeId">ID of the hospitality type to remove</param>
        void RemoveHospitalityType(IConnectionManager entry, RecordIdentifier hosTypeId);

        /// <summary>
        /// Remove a kitchen station from section station routings
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stationId">ID of the kitchen station to remove</param>
        void RemoveStation(IConnectionManager entry, RecordIdentifier stationId);
    }
}