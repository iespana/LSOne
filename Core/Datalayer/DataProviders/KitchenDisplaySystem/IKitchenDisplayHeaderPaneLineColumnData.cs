using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayHeaderPaneLineColumnData : IDataProvider<LSOneHeaderPaneLineColumn>
    {
        /// <summary>
        /// Get the header pane line column with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the header pane line column</param>
        /// <returns>The header pane line column with the given ID</returns>
        LSOneHeaderPaneLineColumn Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get a list of all header pane line columns
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all header pane line columns</returns>
        List<LSOneHeaderPaneLineColumn> GetList(IConnectionManager entry);

        /// <summary>
        /// Get a list of all header pane line columns connected to a certain header pane line
        /// </summary>
        /// <param name="entry">The entry into the database</param
        /// <param name="headerPaneLineID">ID of the header pane line</param>
        /// <returns>A list of all header pane lines for the header pane line</returns>
        List<LSOneHeaderPaneLineColumn> GetList(IConnectionManager entry, RecordIdentifier headerPaneLineID);

        /// <summary>
        /// Delete all records connected to a header pane line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineID">ID of the header pane line</param>
        void DeleteByHeaderPaneLine(IConnectionManager entry, RecordIdentifier lineID);

        /// <summary>
        /// Delete all records connected to a header pane
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="headerPaneID">ID of the header pane</param>
        void DeleteByHeaderPane(IConnectionManager entry, RecordIdentifier headerPaneID);

        /// <summary>
        /// Get the next column number for a header pane line.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="lineID">ID of the header pane line</param>
        /// <returns></returns>
        int GetNextColumnNumber(IConnectionManager entry, RecordIdentifier lineID);
    }
}