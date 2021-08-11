using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayStationData : SqlServerDataProviderBase, IKitchenDisplayStationData
    {
        private static string ResolveSort(KitchenDisplayStationSortingEnum sort, bool backwards)
        {
            switch (sort)
            {
                case KitchenDisplayStationSortingEnum.KitchenDisplayStationId:
                    return backwards ? "kds.ID DESC" : "kds.ID ASC";

                case KitchenDisplayStationSortingEnum.KitchenDisplayStationName:
                    return backwards ? "kds.NAME DESC" : "kds.NAME ASC";
            }

            return "";
        }

        private static string BaseSelectString
        {
            get
            {
                return @"select
                    UPPER(kds.ID) as ID
                    ,kds.NAME
                    ,kds.SCREENNUMBER
                    ,kds.KITCHENDISPLAYFUNCTIONALPROFILEID" +
                    @",ISNULL(kfp.NAME, '') as KITCHENDISPLAYFUNCTIONALPROFILENAME
                    ,kds.KITCHENDISPLAYSTYLEPROFILEID
                    ,ISNULL(ksp.NAME, '') as KITCHENDISPLAYSTYLEPROFILENAME
                    ,kds.KITCHENDISPLAYVISUALPROFILEID
                    ,UPPER(ISNULL(kds.NEXTSTATIONID,'')) as NEXTSTATIONID
                    ,ISNULL(kds2.NAME,'') as NEXTSTATIONNAME
                    ,ISNULL(kds.ROUTETOSTATIONID,'') as ROUTETOSTATIONID
                    ,ISNULL(kds3.NAME,'') as ROUTETOSTATIONNAME
                    ,ISNULL(kvp.NAME, '') as KITCHENDISPLAYVISUALPROFILENAME
                    ,ISNULL(kds.USEEXTERNALROUTING, 0) as USEEXTERNALROUTING
                    ,ISNULL(kdp.DISPLAYMODE, 0) as DISPLAYMODE" +
                    //OldDisplayMode() +
                    @",ISNULL(kds.STATIONTYPE, 0) as STATIONTYPE
                    ,ISNULL(kds.DISCOVERSBUMPBAR, 0) as DISCOVERSBUMPBAR
                    ,ISNULL(kds.DISCOVERKEY, 0) as DISCOVERKEY 
                    ,ISNULL(kds.STATIONLETTER, '') as STATIONLETTER
                    ,ISNULL(kds.FULLSCREEN, 0) as FULLSCREEN
                    ,ISNULL(kds.HORIZONTALLOCATION, 0) as HORIZONTALLOCATION
                    ,ISNULL(kds.VERTICALLOCATION, 0) as VERTICALLOCATION
                    ,kds.DISPLAYPROFILEID
                    ,ISNULL(kdp.NAME, '') as DISPLAYPROFILENAME
                    ,ISNULL(kds.RECALLFILESMAX, 100) as RECALLFILESMAX
                    ,ISNULL(kds.GROUPBYSEATNO, 0) as GROUPBYSEATNO
                    ,ISNULL(kds.SEATNOGROUPTEXT, '') as SEATNOGROUPTEXT
                    ,ISNULL(kds.USEAGGREGATEGROUPS, 0) as USEAGGREGATEGROUPS
                    ,ISNULL(kds.AGGREGATEPROFILEID, '') as AGGREGATEPROFILEID
                    ,ISNULL(kds.SHOWONLYWHENBUMPEDONPRIOR, 0) as SHOWONLYWHENBUMPEDONPRIOR
                    ,ISNULL(kds.TRANSFERTOSTATIONID, '') as TRANSFERTOSTATIONID
                    ,ISNULL(kds4.NAME,'') as TRANSFERTOSTATIONNAME
                    ,ISNULL(kds.FALLBACKSTATIONID, '') as FALLBACKSTATIONID
                    ,ISNULL(kds.EXPEDITORBUMPSTATIONID, '') as EXPEDITORBUMPSTATIONID
                    from KITCHENDISPLAYSTATION kds " +
                    "left outer join KITCHENDISPLAYFUNCTIONALPROFILE kfp on kfp.ID = kds.KITCHENDISPLAYFUNCTIONALPROFILEID and kfp.DATAAREAID = kds.DATAAREAID " +
                    "left outer join KITCHENDISPLAYSTYLEPROFILE ksp on ksp.ID = kds.KITCHENDISPLAYSTYLEPROFILEID and ksp.DATAAREAID = kds.DATAAREAID " +
                    "left outer join KITCHENDISPLAYVISUALPROFILE kvp on kvp.ID = kds.KITCHENDISPLAYVISUALPROFILEID and kvp.DATAAREAID = kds.DATAAREAID " +
                    "left outer join KITCHENDISPLAYPROFILE kdp on kdp.ID = kds.DISPLAYPROFILEID and kdp.DATAAREAID = kds.DATAAREAID " +
                    "left outer join KITCHENDISPLAYAGGREGATEPROFILE kap on kap.PROFILEID = kds.AGGREGATEPROFILEID and kap.DATAAREAID = kds.DATAAREAID " +
                    "left outer join KITCHENDISPLAYSTATION kds2 on kds.NEXTSTATIONID = kds2.ID and kvp.DATAAREAID = kds.DATAAREAID " +
                    "left outer join KITCHENDISPLAYSTATION kds3 on kds.ROUTETOSTATIONID = kds3.ID and kvp.DATAAREAID = kds.DATAAREAID " +
                    "left outer join KITCHENDISPLAYSTATION kds4 on kds.TRANSFERTOSTATIONID = kds4.ID and kvp.DATAAREAID = kds.DATAAREAID ";
;
            }
        }

        private static void PopulateKitchenDisplayStation(IDataReader dr, KitchenDisplayStation kds)
        {
            kds.ID = (string)dr["ID"];
            kds.ID.SerializationData = (string)dr["ID"];
            kds.Text = (string)dr["NAME"];
            kds.KitchenDisplayFunctionalProfileId = (Guid)dr["KITCHENDISPLAYFUNCTIONALPROFILEID"];
            kds.KitchenDisplayFunctionalProfileDescription = (string)dr["KITCHENDISPLAYFUNCTIONALPROFILENAME"];
            kds.KitchenDisplayStyleProfileId = (Guid)dr["KITCHENDISPLAYSTYLEPROFILEID"];
            kds.KitchenDisplayStyleProfileDescription = (string)dr["KITCHENDISPLAYSTYLEPROFILENAME"];
            kds.KitchenDisplayVisualProfileId = (Guid)dr["KITCHENDISPLAYVISUALPROFILEID"];
            kds.NextStationId = (string)dr["NEXTSTATIONID"];
            kds.NextStationName = (string)dr["NEXTSTATIONNAME"];
            kds.RouteToStationId = (string)dr["ROUTETOSTATIONID"];
            kds.RouteToStationName = (string)dr["ROUTETOSTATIONNAME"];
            kds.KitchenDisplayVisualProfileDescription = (string)dr["KITCHENDISPLAYVISUALPROFILENAME"];
            kds.ScreenNumber = (KitchenDisplayStation.ScreenNumberEnum)dr["SCREENNUMBER"];
            kds.DisplayMode = (KitchenDisplayStation.DisplayModeEnum)AsInt(dr["DISPLAYMODE"]);
            kds.KitchenDisplayProfileDescription = (string)dr["DISPLAYPROFILENAME"];
            if (dr["DISPLAYPROFILEID"] == DBNull.Value)
            {
                kds.KitchenDisplayProfileId = Guid.Parse("{00000000-0000-0000-0000-000000000000}");
            }
            else
            {
                kds.KitchenDisplayProfileId = (Guid)dr["DISPLAYPROFILEID"];
            }
            kds.UseExternalRouting = AsBool(dr["USEEXTERNALROUTING"]);
            kds.StationType = (KitchenDisplayStation.StationTypeEnum)((byte)dr["STATIONTYPE"]);
            kds.DiscoverBumpBar = AsBool(dr["DISCOVERSBUMPBAR"]);
            kds.DiscoverKey = (KitchenDisplayButton.ButtonActionEnum)dr["DISCOVERKEY"];
            kds.StationLetter = (string)dr["STATIONLETTER"];
            kds.FullScreen = AsBool(dr["FULLSCREEN"]);
            kds.HorizontalPosition = (KitchenDisplayStation.HorizontalPositionEnum)dr["HORIZONTALLOCATION"];
            kds.VerticalPosition = (KitchenDisplayStation.VerticalPositionEnum)dr["VERTICALLOCATION"];
            kds.RecallFilesMax = AsInt(dr["RECALLFILESMAX"]);
            if (kds.RecallFilesMax == 0)
            {
                kds.RecallFilesMax = 100;
            }
            kds.GroupBySeatNo = AsBool(dr["GROUPBYSEATNO"]);
            kds.SeatNoGroupText = (string)dr["SEATNOGROUPTEXT"];
            kds.UseAggregateGroups = AsBool(dr["USEAGGREGATEGROUPS"]);
            kds.AggregateProfileId = (string)dr["AGGREGATEPROFILEID"];
            kds.ShowOnlyWhenBumpedOnPriorStations = AsBool(dr["SHOWONLYWHENBUMPEDONPRIOR"]);
            kds.TransferToStationID = (string)dr["TRANSFERTOSTATIONID"];
            kds.TransferToStationName = (string)dr["TRANSFERTOSTATIONNAME"];
            kds.FallbackStationId = (string)dr["FALLBACKSTATIONID"];
            kds.ExpeditorBumpStationID = (string)dr["EXPEDITORBUMPSTATIONID"];
        }

        /// <summary>
        /// Gets all kitchen display stations
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of kitchen display station objects containing all kitchen display stations station</returns>
        public virtual List<KitchenDisplayStation> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where kds.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<KitchenDisplayStation>(entry, cmd, CommandType.Text, PopulateKitchenDisplayStation);
            }
        }

        /// <summary>
        /// Gets all kitchen display stations ordered by the specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of all kitchen display stations, ordered by a chosen field</returns>
        public virtual List<KitchenDisplayStation> GetList(IConnectionManager entry, KitchenDisplayStationSortingEnum sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where kds.DATAAREAID = @dataAreaId order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<KitchenDisplayStation>(entry, cmd, CommandType.Text, PopulateKitchenDisplayStation);
            }
        }

        /// <summary>
        /// Gets all kitchen display stations that belong to a store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeId">Store id to filter by</param>
        /// <returns>A list of kitchen display station objects containing all kitchen display stations station</returns>
        public virtual List<KitchenDisplayStation> GetList(IConnectionManager entry, string storeId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    @"left join KITCHENDISPLAYSTATIONSTORE kdss on kdss.KITCHENDISPLAYSTATIONID = kds.ID where kds.DATAAREAID = @dataAreaId and kdss.STOREID = @storeId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeId", storeId);

                return Execute<KitchenDisplayStation>(entry, cmd, CommandType.Text, PopulateKitchenDisplayStation);
            }
        }

        /// <summary>
        /// Gets a kitchen display station with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the kitchen display station to get</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        /// <returns>A KitchenDisplayStation object containing the kitchen display station with the given ID</returns>
        public virtual KitchenDisplayStation Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where kds.DATAAREAID = @dataAreaId and kds.ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                var result = Get<KitchenDisplayStation>(entry, cmd, id, PopulateKitchenDisplayStation, cacheType, UsageIntentEnum.Normal);

                if (result == null)
                {
                    //Table is empty, fill profile with minimum default data

                    //    KDebug.WriteDbg(DebugLevel.Detail, string.Format("Query in KitchenDisplayStation.Get() resulted in NULL: @dataAreaId={0}, @id={1}",
                    //       entry.Connection.DataAreaId, (string)id));
                }
                return result;
            }
        }

        /// <summary>
        /// Checks if a kitchen display station with the given id exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the kitchen display station to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYSTATION", "ID", id);
        }

        /// <summary>
        /// Deletes the kitchen display station with the given id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the kitchen display station to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYSTATION", "ID", id, BusinessObjects.Permission.ManageKitchenDisplayStations);

            // Remove the display station from the kds section station routing table
            Providers.KitchenDisplaySectionStationRoutingData.RemoveStation(entry, id);

            //Remove all routing information
            Providers.KitchenDisplayItemRoutingConnectionData.DeleteByStation(entry, id);
            Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.DeleteByStation(entry, id);
            Providers.KitchenDisplayTerminalRoutingConnectionData.DeleteByStation(entry, id);
        }

        public virtual void ClearRoutingTo(IConnectionManager entry)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYSTATION", StatementType.Update);
            statement.AddField("ROUTETOSTATIONID", string.Empty);
            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves a kitchen display station into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="kitchenDisplayStation">The kitchen display station object to save</param>
        public virtual void Save(IConnectionManager entry, KitchenDisplayStation kitchenDisplayStation)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYSTATION");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenDisplayStations);

            bool isNew = false;
            if (kitchenDisplayStation.ID.IsEmpty)
            {
                isNew = true;
                kitchenDisplayStation.ID = DataProviderFactory.Instance.GenerateNumber<IKitchenDisplayStationData, KitchenDisplayStation>(entry);
            }

            if (isNew || !Exists(entry, kitchenDisplayStation.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", ((string)kitchenDisplayStation.ID).ToUpper());
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (string)kitchenDisplayStation.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("NAME", kitchenDisplayStation.Text);
            statement.AddField("SCREENNUMBER ", kitchenDisplayStation.ScreenNumber, SqlDbType.Int);
            statement.AddField("KITCHENDISPLAYFUNCTIONALPROFILEID", (Guid)kitchenDisplayStation.KitchenDisplayFunctionalProfileId, SqlDbType.UniqueIdentifier);
            statement.AddField("KITCHENDISPLAYSTYLEPROFILEID", (Guid)kitchenDisplayStation.KitchenDisplayStyleProfileId, SqlDbType.UniqueIdentifier);
            statement.AddField("KITCHENDISPLAYVISUALPROFILEID ", (Guid)kitchenDisplayStation.KitchenDisplayVisualProfileId, SqlDbType.UniqueIdentifier);
            statement.AddField("NEXTSTATIONID", kitchenDisplayStation.NextStationId.ToUpper());
            statement.AddField("ROUTETOSTATIONID", kitchenDisplayStation.RouteToStationId);
            statement.AddField("USEEXTERNALROUTING ", kitchenDisplayStation.UseExternalRouting ? 1 : 0, SqlDbType.TinyInt);
            //statement.AddField("DISPLAYMODE ", (int)kitchenDisplayStation.DisplayMode, SqlDbType.TinyInt);
            statement.AddField("STATIONTYPE", (int)kitchenDisplayStation.StationType, SqlDbType.TinyInt);
            statement.AddField("DISCOVERSBUMPBAR", kitchenDisplayStation.DiscoverBumpBar, SqlDbType.TinyInt);
            statement.AddField("DISCOVERKEY", (int)kitchenDisplayStation.DiscoverKey, SqlDbType.Int);
            statement.AddField("STATIONLETTER", kitchenDisplayStation.StationLetter);
            statement.AddField("FULLSCREEN", kitchenDisplayStation.FullScreen, SqlDbType.TinyInt);
            statement.AddField("HORIZONTALLOCATION", (int)kitchenDisplayStation.HorizontalPosition, SqlDbType.Int);
            statement.AddField("VERTICALLOCATION", (int)kitchenDisplayStation.VerticalPosition, SqlDbType.Int);
            //statement.AddField("BUTTONPROFILEID", kitchenDisplayStation.KitchenDisplayButtonProfileId, SqlDbType.UniqueIdentifier);
            statement.AddField("RECALLFILESMAX", (int)kitchenDisplayStation.RecallFilesMax, SqlDbType.Int);
            statement.AddField("GROUPBYSEATNO", kitchenDisplayStation.GroupBySeatNo, SqlDbType.TinyInt);
            statement.AddField("SEATNOGROUPTEXT", kitchenDisplayStation.SeatNoGroupText);
            statement.AddField("USEAGGREGATEGROUPS", kitchenDisplayStation.UseAggregateGroups, SqlDbType.TinyInt);
            statement.AddField("AGGREGATEPROFILEID", kitchenDisplayStation.AggregateProfileId);
            statement.AddField("SHOWONLYWHENBUMPEDONPRIOR", kitchenDisplayStation.ShowOnlyWhenBumpedOnPriorStations, SqlDbType.TinyInt);
            statement.AddField("TRANSFERTOSTATIONID", kitchenDisplayStation.TransferToStationID);
            statement.AddField("FALLBACKSTATIONID", kitchenDisplayStation.FallbackStationId);
            statement.AddField("EXPEDITORBUMPSTATIONID", kitchenDisplayStation.ExpeditorBumpStationID);

            if (kitchenDisplayStation.KitchenDisplayProfileId != null)
            {
                statement.AddField("DISPLAYPROFILEID", (Guid)kitchenDisplayStation.KitchenDisplayProfileId, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("DISPLAYPROFILEID", DBNull.Value, SqlDbType.UniqueIdentifier);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "KITCHENDISPLAYSTATION", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        public RecordIdentifier SequenceID
        {
            get { return "RestaurantStation"; }
        }

        #endregion
    }
}
