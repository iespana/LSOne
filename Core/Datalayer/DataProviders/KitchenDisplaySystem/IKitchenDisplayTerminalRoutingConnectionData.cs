using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayTerminalRoutingConnectionData : IDataProvider<LSOneKitchenDisplayTerminalRoutingConnection>
    {
        /// <summary>
        /// Gets the specified terminal routing
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="terminalRoutingId">The ID of the routing to get</param>
        /// <returns></returns>
        LSOneKitchenDisplayTerminalRoutingConnection Get(IConnectionManager entry, RecordIdentifier terminalRoutingId);
        List<LSOneKitchenDisplayTerminalRoutingConnection> GetForKds(IConnectionManager entry);
        List<LSOneKitchenDisplayTerminalRoutingConnection> GetForKds(IConnectionManager entry,
            RecordIdentifier kdsId, List<string> connectedStations);

        List<DataEntity> TerminalsNotConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId);
        List<DataEntity> TerminalGroupsNotConnectedToKds(IConnectionManager entry, RecordIdentifier kdsId);
        void MakeSureAllTypeConnectionExists(IConnectionManager entry, string kdsId);
        void MakeSureAllTypeConnectionDoesntExists(IConnectionManager entry, RecordIdentifier kdsId);
        void DeleteByStation(IConnectionManager entry, RecordIdentifier stationId);
    }
}
