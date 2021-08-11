using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IDiningTableLayoutScreenData : IDataProvider<DiningTableLayoutScreen>
    {
        /// <summary>
        /// Gets all dining table layout screens
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of DiningTableLayoutScreen objects containing all dining table layout screens</returns>
        List<DiningTableLayoutScreen> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets all dining table layout screens for a given hospitality type and dining table layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant of the hospitality type</param>
        /// <param name="sequence">The sequence of the hospitality type</param>
        /// <param name="salesType">The sales type of the hospitality type</param>
        /// <param name="diningTableLayoutID">The dining table layout id</param>
        /// <returns>All dining table layout screens for the given id's</returns>
        List<DiningTableLayoutScreen> GetList(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier sequence, RecordIdentifier salesType, RecordIdentifier diningTableLayoutID);

        /// <summary>
        /// Returns the number of dining table layout screens for a given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The number of screens for the given dining table layout</returns>
        int GetNumberOfScreens(IConnectionManager entry, RecordIdentifier diningTableLayoutID);

        /// <summary>
        /// Checks wether the given layout screen is in use by a dining table layout 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="diningTableLayoutScreenID"></param>
        /// <returns></returns>
        bool ScreenIsInUse(IConnectionManager entry, RecordIdentifier diningTableLayoutScreenID);

        /// <summary>
        /// Deletes all dining table layout screens for a given dining table layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        void DeleteForDiningTableLayout(IConnectionManager entry, RecordIdentifier diningTableLayoutID);

        /// <summary>
        /// Deletes all dining table layout screens belonging to a specific hospitality type (restaurantid + salestype)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <param name="salesType">The ID of the sales type</param>
        void DeleteForHospitalityType(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesType);

        DiningTableLayoutScreen Get(IConnectionManager entry, RecordIdentifier diningTableLayoutScreenID);
    }
}