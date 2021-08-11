using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class StationSelectionData : SqlServerDataProviderBase, IStationSelectionData
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                       "s.TYPE as TYPE, " +
                       "s.CODE as CODE, " +
                       "s.STATIONID as STATIONID, " +
                       "s.SALESTYPE as SALESTYPE, " +
                       "s.RESTAURANTID as RESTAURANTID, " +
                       "ISNULL(s.DESCRIPTION,'') as DESCRIPTION, " +
                       "ISNULL(h.DESCRIPTION,'') as HOSPITALITYTYPEDESCRIPTION, " +
                       "ISNULL(r.STATIONNAME,'') as STATIONDESCRIPTION ," +
                       "CODENAME = CASE S.TYPE " +
                       "WHEN 0 THEN ''" +
                       "WHEN 1 THEN ISNULL(RG.NAME,'')" +
                       "WHEN 2 THEN ISNULL(I.ITEMNAME,'')" +
                       "WHEN 3 THEN ISNULL(SG.NAME, '')" +
                       "ELSE ''" +
                       "END " +
                       "from " +
                       "STATIONSELECTION s " +
                       "left outer join HOSPITALITYTYPE h on h.RESTAURANTID = s.RESTAURANTID and h.SALESTYPE = s.SALESTYPE and h.DATAAREAID = s.DATAAREAID " +
                       "left outer join RESTAURANTSTATION r on r.ID = s.STATIONID and r.DATAAREAID = s.DATAAREAID " +
                       "left outer join RETAILGROUP rg ON  s.CODE = rg.GROUPID " +
                       "left outer join RETAILITEM i on s.CODE = i.ITEMID " +
                       "left outer join SPECIALGROUP sg on s.CODE = sg.GROUPID ";
            }
        }

        private static void PopulateStationSelection(IDataReader dr, StationSelection stationSelection)
        {
            stationSelection.Type = (int)dr["TYPE"];
            stationSelection.Code = (string)dr["CODE"];
            stationSelection.StationID = (string)dr["STATIONID"];
            stationSelection.SalesType = (string)dr["SALESTYPE"];
            stationSelection.RestaurantID = (string)dr["RESTAURANTID"];
            stationSelection.Text = (string)dr["DESCRIPTION"];
            stationSelection.HospitalityTypeDescription = (string)dr["HOSPITALITYTYPEDESCRIPTION"];
            stationSelection.StationDescription = (string)dr["STATIONDESCRIPTION"];
            stationSelection.CodeName = (string)dr["CODENAME"];
        }

    

        /// <summary>
        /// Gets a list for a given station
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stationID">Station id</param>
        /// <returns>A list of StationSelection objects containing given station</returns>
        public virtual List<StationSelection> GetListForStation(IConnectionManager entry, RecordIdentifier stationID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where s.DATAAREAID = @dataAreaId and s.STATIONID = @stationID ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "stationID", (string) stationID);

                return Execute<StationSelection>(entry, cmd, CommandType.Text, PopulateStationSelection);
            }
        }

        /// <summary>
        /// Gets all station selections
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of StationSelection objects conaining all station selections</returns>
        public virtual List<StationSelection> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =

                 BaseSelectString
                    +
                    "where s.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<StationSelection>(entry, cmd, CommandType.Text, PopulateStationSelection);
            }
        }

        /// <summary>
        /// Gets all station selections for the given hospitality type (restaurantID, salesTypeID)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The restaurant ID of the hospitality type</param>
        /// <param name="salesTypeID">The sales type ID of the hospitality type</param>
        /// <returns></returns>
        public virtual List<StationSelection> GetList(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSelectString +
                                  "where s.DATAAREAID = @dataAreaId and s.RESTAURANTID = @restaurantId and s.SALESTYPE = @salesType";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string)restaurantID);
                MakeParam(cmd, "salesType", (string)salesTypeID);

                return Execute<StationSelection>(entry, cmd, CommandType.Text, PopulateStationSelection);
            }
        }

        /// <summary>
        /// Gets a station selection with the given id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stationSelectionID">The id of the station selection to get</param>
        /// <returns>A StationSelection object containing the station selection with the given id</returns>
        public virtual StationSelection Get(IConnectionManager entry, RecordIdentifier stationSelectionID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where s.DATAAREAID= @dataAreaId and s.TYPE = @type and s.CODE = @code and s.STATIONID = @stationId and s.SALESTYPE = @salesType and s.RESTAURANTID = @restaurantId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", stationSelectionID[0]);
                MakeParam(cmd, "salesType", stationSelectionID[1]);
                MakeParam(cmd, "type", stationSelectionID[2]);
                MakeParam(cmd, "code", stationSelectionID[3]);
                MakeParam(cmd, "stationId", stationSelectionID[4]);

                var result = Execute<StationSelection>(entry, cmd, CommandType.Text, PopulateStationSelection);
                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Checks if a station selection with the given id exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stationSelectionID">The id of the station selection(RestaurantID, SalesType, Type, Code, StationID)</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier stationSelectionID)
        {
            return RecordExists(entry, "STATIONSELECTION", new[] {"RESTAURANTID", "SALESTYPE", "TYPE", "CODE", "STATIONID"}, stationSelectionID);
        }

        /// <summary>
        /// Deletes the station selection with the given id
        /// </summary>
        /// <param name="entry">The enty into the database</param>
        /// <param name="stationSelectionID">The id of the station selectino to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier stationSelectionID)
        {
            DeleteRecord(entry, "STATIONSELECTION", new[] { "RESTAURANTID", "SALESTYPE", "TYPE", "CODE", "STATIONID" }, stationSelectionID, BusinessObjects.Permission.ManageStationPrinting);
        }

        /// <summary>
        /// Saves a StationSelection object into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="stationSelection">The StationSelection object to save</param>
        public virtual void Save(IConnectionManager entry, StationSelection stationSelection)
        {
            var statement = new SqlServerStatement("STATIONSELECTION");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageStationPrinting);

            if (!Exists(entry, stationSelection.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("RESTAURANTID", (string)stationSelection.RestaurantID);
                statement.AddKey("SALESTYPE", (string)stationSelection.SalesType);
                statement.AddKey("TYPE", (int)stationSelection.Type, SqlDbType.Int);
                statement.AddKey("CODE", (string)stationSelection.Code);
                statement.AddKey("STATIONID", (string)stationSelection.StationID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("RESTAURANTID", (string)stationSelection.RestaurantID);
                statement.AddCondition("SALESTYPE", (string)stationSelection.SalesType);
                statement.AddCondition("TYPE", (int)stationSelection.Type, SqlDbType.Int);
                statement.AddCondition("CODE", (string)stationSelection.Code);
                statement.AddCondition("STATIONID", (string)stationSelection.StationID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DESCRIPTION", stationSelection.Text);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
