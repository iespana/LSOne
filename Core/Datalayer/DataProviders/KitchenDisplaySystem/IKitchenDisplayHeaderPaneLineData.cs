using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayHeaderPaneLineData : IDataProvider<HeaderPaneLine>
    {
        /// <summary>
        /// Get the header pane line with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the header pane line</param>
        /// <returns>The header pane line with the given ID</returns>
        HeaderPaneLine Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get a list of all header pane lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all header pane lines</returns>
        List<HeaderPaneLine> GetList(IConnectionManager entry);

        /// <summary>
        /// Get a list of all header pane lines connected to a certain header pane
        /// </summary>
        /// <param name="entry">The entry into the database</param
        /// <param name="headerPaneID">ID of the header pane</param>
        /// <returns>A list of all header pane lines for the header pane</returns>
        List<HeaderPaneLine> GetList(IConnectionManager entry, RecordIdentifier headerPaneID);

        /// <summary>
        /// Delete all records connected to a header pane
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="headerPaneID">ID of the header pane</param>
        void DeleteByHeaderPane(IConnectionManager entry, RecordIdentifier headerPaneID);

        /// <summary>
        /// Updates the line number value for one header pane line in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="headerPaneLine">Header pane line object containing the data to be updated</param>
        void SaveLineNumber(IConnectionManager entry, HeaderPaneLine headerPaneLine);
    }
}