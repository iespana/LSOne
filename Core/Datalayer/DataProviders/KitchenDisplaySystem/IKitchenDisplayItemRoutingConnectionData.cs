using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayItemRoutingConnectionData : IDataProvider<LSOneKitchenDisplayItemRoutingConnection>
    {
        /// <summary>
        /// Gets specified item routing
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="itemRoutingId">The ID of the routing to get</param>
        /// <returns></returns>
        LSOneKitchenDisplayItemRoutingConnection Get(IConnectionManager entry, RecordIdentifier itemRoutingId);

        List<LSOneKitchenDisplayItemRoutingConnection> GetForKds(IConnectionManager entry);
        List<LSOneKitchenDisplayItemRoutingConnection> GetForKds(IConnectionManager entry,
            RecordIdentifier kdsId, List<string> connectedStations);

        LSOneKitchenDisplayItemRoutingConnection GetForKdsAndItemID(IConnectionManager entry,
            RecordIdentifier kdsId, DataEntity item);

        LSOneKitchenDisplayItemRoutingConnection GetForKdsAndRetailGroupID(IConnectionManager entry, RecordIdentifier kdsId, DataEntity item);
        LSOneKitchenDisplayItemRoutingConnection GetForKdsAndSpecialGroupID(IConnectionManager entry, RecordIdentifier kdsId, DataEntity item);

        List<DataEntity> SearchItemsNotConnectedToKds(
            IConnectionManager entry,
            RecordIdentifier kdsId,
            string searchString,
            int rowFrom,
            int rowTo,
            bool beginsWith);

        List<DataEntity> ItemsConnectedToKds(
            IConnectionManager entry,
            RecordIdentifier kdsId);

        List<DataEntity> RetailGroupsNotConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId);
        List<DataEntity> RetailGroupsConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId);
        List<DataEntity> SpecialGroupsNotConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId);
        List<DataEntity> SpecialGroupsConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId);
        void MakeSureAllTypeConnectionExists(IConnectionManager entry, string kdsId);
        void MakeSureAllTypeConnectionDoesntExists(IConnectionManager entry, RecordIdentifier kdsId);
        void DeleteByStation(IConnectionManager entry, RecordIdentifier stationId);
    }
}
