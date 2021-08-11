using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayHeaderPaneData : IDataProvider<HeaderPaneProfile>
    {
        /// <summary>
        /// Get the header pane with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the header pane</param>
        /// <returns>The header pane with the given ID</returns>
        HeaderPaneProfile Get(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Get a list of all header panes
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all header panes</returns>
        List<HeaderPaneProfile> GetList(IConnectionManager entry);
    }
}