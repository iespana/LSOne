using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;


namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayHospitalityTypeRoutingConnectionData : IDataProvider<LSOneKitchenDisplayHospitalityTypeRoutingConnection>
    {
        /// <summary>
        /// Gets the specified hospitality type routing
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="hospitalityTypeRoutingId">The ID of the routing to get</param>
        /// <returns></returns>
        LSOneKitchenDisplayHospitalityTypeRoutingConnection Get(IConnectionManager entry, RecordIdentifier hospitalityTypeRoutingId);
        List<LSOneKitchenDisplayHospitalityTypeRoutingConnection> GetForKds(IConnectionManager entry);
        List<LSOneKitchenDisplayHospitalityTypeRoutingConnection> GetForKds(IConnectionManager entry, string kdsId, List<string> connectedStations);
        List<DataEntity> HospitalitTypesNotConnectedToKds(IConnectionManager entry, string kdsId);
        void MakeSureAllTypeConnectionExists(IConnectionManager entry, string kdsId);
        void MakeSureAllTypeConnectionDoesntExists(IConnectionManager entry, string kdsId);
        void DeleteByStation(IConnectionManager entry, RecordIdentifier stationId);
    }
}
