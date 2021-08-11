using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    /// <summary>
    /// A DataProvider class for restaurant dining tables
    /// </summary>
    public class RestaurantDiningTableData : SqlServerDataProviderBase, IRestaurantDiningTableData
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "RESTAURANTID, " +
                    "SALESTYPE, " +
                    "DINEINTABLENO, " +
                    "SEQUENCE, " +
                    "DININGTABLELAYOUTID, " +
                    "ISNULL(DESCRIPTION,'') as DESCRIPTION, " +
                    "ISNULL(TYPE,0) as TYPE, " +
                    "ISNULL(NONSMOKING,0) as NONSMOKING, " +
                    "ISNULL(NOOFGUESTS,0) as NOOFGUESTS, " +
                    "ISNULL(JOINEDTABLE,0) as JOINEDTABLE, " +
                    "ISNULL(X1POSITION,0) as X1POSITION, " +
                    "ISNULL(X2POSITION,0) as X2POSITION, " +
                    "ISNULL(Y1POSITION,0) as Y1POSITION, " +
                    "ISNULL(Y2POSITION,0) as Y2POSITION, " +
                    "ISNULL(LINKEDTODINEINTABLE,0) as LINKEDTODINEINTABLE, " +
                    "ISNULL(DININGTABLESJOINED,0) as DININGTABLESJOINED, " +
                    "ISNULL(AVAILABILITY,0) as AVAILABILITY, " +
                    "ISNULL(LAYOUTSCREENNO,0) as LAYOUTSCREENNO, " +
                    "ISNULL(DESCRIPTIONONBUTTON,'') as DESCRIPTIONONBUTTON, " +
                    "ISNULL(SHAPE,0) as SHAPE, " +
                    "ISNULL(X1POSITIONDESIGN,0) as X1POSITIONDESIGN, " +
                    "ISNULL(X2POSITIONDESIGN,0) as X2POSITIONDESIGN, " +
                    "ISNULL(Y1POSITIONDESIGN,0) as Y1POSITIONDESIGN, " +
                    "ISNULL(Y2POSITIONDESIGN,0) as Y2POSITIONDESIGN, " +
                    "ISNULL(LAYOUTSCREENNODESIGN,0) as LAYOUTSCREENNODESIGN, " +
                    "ISNULL(JOINEDTABLEDESIGN,0) as JOINEDTABLEDESIGN, " +
                    "ISNULL(DININGTABLESJOINEDDESIGN,0) DININGTABLESJOINEDDESIGN, " +
                    "ISNULL(DELETEINOTHERLAYOUTS,0) as DELETEINOTHERLAYOUTS " +
                    "from RESTAURANTDININGTABLE ";
            }
        }

        /// <summary>
        /// Populates the given RestaurantDiningTable object with fields from the given SqlDataReader
        /// </summary>
        /// <param name="dr">The data reader containing the data</param>
        /// <param name="diningTable">The RestaurantDiningTable object to populate</param>
        private static void PopulateRestaurantDiningTable(IDataReader dr, RestaurantDiningTable diningTable)
        {
            diningTable.RestaurantID = (string)dr["RESTAURANTID"];
            diningTable.SalesType = (string)dr["SALESTYPE"];
            diningTable.DineInTableNo = (int)dr["DINEINTABLENO"];
            diningTable.Text = (string)dr["DESCRIPTION"];
            diningTable.Type = (RestaurantDiningTable.TypeEnum)((int)dr["TYPE"]);
            diningTable.NonSmoking = ((byte)dr["NONSMOKING"] != 0);
            diningTable.NoOfGuests = (int)dr["NOOFGUESTS"];
            diningTable.JoinedTable = (int)dr["JOINEDTABLE"];
            diningTable.X1Position = (int)dr["X1POSITION"];
            diningTable.X2Position = (int)dr["X2POSITION"];
            diningTable.Y1Position = (int)dr["Y1POSITION"];
            diningTable.Y2Position = (int)dr["Y2POSITION"];
            diningTable.LinkedToDineInTable = (int)dr["LINKEDTODINEINTABLE"];
            diningTable.DiningTablesJoined = ((byte)dr["DININGTABLESJOINED"] != 0);
            diningTable.Sequence = (int)dr["SEQUENCE"];
            diningTable.Availability = (RestaurantDiningTable.AvailabilityEnum)((int)dr["AVAILABILITY"]);
            diningTable.DiningTableLayoutID = (string)dr["DININGTABLELAYOUTID"];
            diningTable.LayoutScreenNo = (int)dr["LAYOUTSCREENNO"];
            diningTable.DescriptionOnButton = (string)dr["DESCRIPTIONONBUTTON"];
            diningTable.Shape = (RestaurantDiningTable.ShapeEnum)((int)dr["SHAPE"]);
            diningTable.X1PositionDesign = (int)dr["X1POSITIONDESIGN"];
            diningTable.X2PositionDesign = (int)dr["X2POSITIONDESIGN"];
            diningTable.Y1PositionDesign = (int)dr["Y1POSITIONDESIGN"];
            diningTable.Y2PositionDesign = (int)dr["Y2POSITIONDESIGN"];
            diningTable.LayoutScreenNoDesign = (int)dr["LAYOUTSCREENNODESIGN"];
            diningTable.JoinedTableDesign = (int)dr["JOINEDTABLEDESIGN"];
            diningTable.DiningTablesJoinedDesign = ((byte)dr["DININGTABLESJOINEDDESIGN"] != 0);
            diningTable.DeleteInOtherLayouts = ((byte)dr["DELETEINOTHERLAYOUTS"] != 0);
        }

        /// <summary>
        /// Returns all restaurant dining tables
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of RestaurantDiningTable objects containig all restaurant dining tables</returns>
        public virtual List<RestaurantDiningTable> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<RestaurantDiningTable>(entry, cmd, CommandType.Text, PopulateRestaurantDiningTable);
            }
        }

        /// <summary>
        /// Gets all restaurant dining table for a given hospitality type and dining table layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant of the hospitality type</param>
        /// <param name="sequence">The sequence of the hospitality type</param>
        /// <param name="salesType">The sales type of the hospitality type</param>
        /// <param name="diningTableLayoutID">The dining table layout id</param>
        /// <param name="cacheType"></param>
        /// <returns>All restaurant dining tables for the given id's</returns>
        public virtual List<RestaurantDiningTable> GetList(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier sequence, RecordIdentifier salesType, RecordIdentifier diningTableLayoutID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            if (cacheType != CacheType.CacheTypeNone)
            {
                var bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof(CacheBucket), new RecordIdentifier(restaurantID,sequence,salesType,diningTableLayoutID));

                if (bucket != null)
                {
                    return (List<RestaurantDiningTable>)bucket.BucketData;
                }
            }
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaID and RESTAURANTID = @restaurantId and SEQUENCE = @sequence and SALESTYPE = @salesType and DININGTABLELAYOUTID = @diningTableLayoutId";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) restaurantID);
                MakeParam(cmd, "sequence", (int) sequence, SqlDbType.Int);
                MakeParam(cmd, "salesType", (string) salesType);
                MakeParam(cmd, "diningTableLayoutId", (string) diningTableLayoutID);

                List<RestaurantDiningTable> list = Execute<RestaurantDiningTable>(entry, cmd, CommandType.Text, PopulateRestaurantDiningTable);
                if (cacheType != CacheType.CacheTypeNone)
                {
                    CacheBucket bucket = new CacheBucket(new RecordIdentifier(restaurantID, sequence, salesType, diningTableLayoutID), "", list);

                    entry.Cache.AddEntityToCache(bucket.ID, bucket, cacheType);
                }
                return list;
            }
        }

        /// <summary>
        /// Gets a specific restaurant dining table
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantDiningTableID">The ID of the restaurant dinig table (restaurantID, sequence, salestype, diningTableLayoutID, dineIntableNo)</param>
        /// <returns>The restaurant dining table</returns>
        public virtual RestaurantDiningTable Get(IConnectionManager entry, RecordIdentifier restaurantDiningTableID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaID and RESTAURANTID = @restaurantId and SEQUENCE = @sequence and SALESTYPE = @salesType and DININGTABLELAYOUTID = @diningTableLayoutId and DINEINTABLENO = @dineInTableNo";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) restaurantDiningTableID[0]);
                MakeParam(cmd, "sequence", (int) restaurantDiningTableID[1], SqlDbType.Int);
                MakeParam(cmd, "salesType", (string) restaurantDiningTableID[2]);
                MakeParam(cmd, "diningTableLayoutId", (string) restaurantDiningTableID[3]);
                MakeParam(cmd, "dineInTableNo", (int) restaurantDiningTableID[4], SqlDbType.Int);

                var result = Execute<RestaurantDiningTable>(entry, cmd, CommandType.Text,
                    PopulateRestaurantDiningTable);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Returns the lowest table number for the given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The lowest restaurant dining table number for the given layout ID</returns>
        public virtual int GetStartingTableNumber(IConnectionManager entry, RecordIdentifier diningTableLayoutID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select ISNULL(MIN(DINEINTABLENO),0) " +
                    "from RESTAURANTDININGTABLE " +
                    "where DATAAREAID = @dataAreaID and RESTAURANTID = @restaurantId and SEQUENCE = @sequence and SALESTYPE = @salesType and DININGTABLELAYOUTID = @diningTableLayoutId";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) diningTableLayoutID[0]);
                MakeParam(cmd, "sequence", (int) diningTableLayoutID[1], SqlDbType.Int);
                MakeParam(cmd, "salesType", (string) diningTableLayoutID[2]);
                MakeParam(cmd, "diningTableLayoutId", (string) diningTableLayoutID[3]);

                return (int) entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Returns the highest restaurant dining table number for the given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The highest restaurant dining table number for the given layout ID</returns>
        public virtual int GetEndingTableNumber(IConnectionManager entry, RecordIdentifier diningTableLayoutID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select ISNULL(MAX(DINEINTABLENO),0) " +
                    "from RESTAURANTDININGTABLE " +
                    "where DATAAREAID = @dataAreaID and RESTAURANTID = @restaurantId and SEQUENCE = @sequence and SALESTYPE = @salesType and DININGTABLELAYOUTID = @diningTableLayoutId";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) diningTableLayoutID[0]);
                MakeParam(cmd, "sequence", (int) diningTableLayoutID[1], SqlDbType.Int);
                MakeParam(cmd, "salesType", (string) diningTableLayoutID[2]);
                MakeParam(cmd, "diningTableLayoutId", (string) diningTableLayoutID[3]);

                return (int) entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Gets the current number of restaurant dining tables assigned to a given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The current number fo restaurant dining tables for the given dining table layout</returns>
        public virtual int GetNumberOfTables(IConnectionManager entry, RecordIdentifier diningTableLayoutID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select ISNULL(COUNT(DINEINTABLENO), 0) " +
                    "from RESTAURANTDININGTABLE " +
                    "where DATAAREAID = @dataAreaID and RESTAURANTID = @restaurantId and SEQUENCE = @sequence and SALESTYPE = @salesType and DININGTABLELAYOUTID = @diningTableLayoutId";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) diningTableLayoutID[0]);
                MakeParam(cmd, "sequence", (int) diningTableLayoutID[1], SqlDbType.Int);
                MakeParam(cmd, "salesType", (string) diningTableLayoutID[2]);
                MakeParam(cmd, "diningTableLayoutId", (string) diningTableLayoutID[3]);

                return (int) entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Checks wether a restaurant dining table with the given id exists in tha database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantDiningTableID">The id of the restaurant dining table</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier restaurantDiningTableID)
        {
            return RecordExists(entry, "RESTAURANTDININGTABLE", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE", "DININGTABLELAYOUTID", "DINEINTABLENO" }, restaurantDiningTableID);
        }

        /// <summary>
        /// Checks wether any restaurant dining tables exists within the given range for a specific dining table layout.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The id of the dining table layout (restaurantId, sequence, salestype, diningTableLayoutId)</param>
        /// <param name="rangeFrom">The start of the range</param>
        /// <param name="rangeTo">The end of the range</param>
        /// <returns>True if any records exist within the given range, false otherwise</returns>
        public virtual bool DineInTableRangeExists(IConnectionManager entry, RecordIdentifier diningTableLayoutID, int rangeFrom, int rangeTo)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select COUNT(DINEINTABLENO) " +
                    "from RESTAURANTDININGTABLE " +
                    "where DATAAREAID = @dataAreaId and RESTAURANTID = @restaurantId and SEQUENCE = @sequence and SALESTYPE = @salesType and DININGTABLELAYOUTID = @diningTableLayoutId " +
                    "and DINEINTABLENO >= @rangeFrom and DINEINTABLENO <= @rangeTo";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string)diningTableLayoutID[0]);
                MakeParam(cmd, "sequence", (int)diningTableLayoutID[1], SqlDbType.Int);
                MakeParam(cmd, "salesType", (string)diningTableLayoutID[2]);
                MakeParam(cmd, "diningTableLayoutId", (string)diningTableLayoutID[3]);
                MakeParam(cmd, "rangeFrom", rangeFrom, SqlDbType.Int);
                MakeParam(cmd, "rangeTo", rangeTo, SqlDbType.Int);                

                return (int)entry.Connection.ExecuteScalar(cmd) > 0;
            }
        }

        /// <summary>
        /// Deletes a restaurant dining table with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantDiningTableID">The ID of the restaurant dining table to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier restaurantDiningTableID)
        {
            DeleteRecord(entry, "RESTAURANTDININGTABLE", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE", "DININGTABLELAYOUTID", "DINEINTABLENO" }, restaurantDiningTableID, BusinessObjects.Permission.ManageDiningTableLayouts);
        }

        /// <summary>
        /// Deletes all restaurant dining tables for a given dining table layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        public virtual void DeleteForDiningTableLayout(IConnectionManager entry, RecordIdentifier diningTableLayoutID)
        {
            DeleteRecord(entry, "RESTAURANTDININGTABLE", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE", "DININGTABLELAYOUTID" }, diningTableLayoutID, BusinessObjects.Permission.ManageDiningTableLayouts);
        }

        /// <summary>
        /// Deletes all restaurant dining tables belonging to a specific hospitality type (restaurantid + salestype)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <param name="salesType">The ID of the sales type</param>
        public virtual void DeleteForHospitalityType(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesType)
        {
            var hospitalityTypeID = new RecordIdentifier(restaurantID, salesType);

            DeleteRecord(entry, "RESTAURANTDININGTABLE", new[] { "RESTAURANTID", "SALESTYPE" }, hospitalityTypeID, BusinessObjects.Permission.ManageDiningTableLayouts);
        }

        /// <summary>
        /// Saves a restaurant dining table into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantDiningTable">The RestaurantDiningTable object to save</param>
        public virtual void Save(IConnectionManager entry, RestaurantDiningTable restaurantDiningTable)
        {
            var statement = new SqlServerStatement("RESTAURANTDININGTABLE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageDiningTableLayouts);

            if (!Exists(entry, restaurantDiningTable.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("RESTAURANTID", (string)restaurantDiningTable.RestaurantID);
                statement.AddKey("SEQUENCE", (int)restaurantDiningTable.Sequence, SqlDbType.Int);
                statement.AddKey("SALESTYPE", (string)restaurantDiningTable.SalesType);
                statement.AddKey("DININGTABLELAYOUTID", (string)restaurantDiningTable.DiningTableLayoutID);
                statement.AddKey("DINEINTABLENO", (int)restaurantDiningTable.DineInTableNo, SqlDbType.Int);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("RESTAURANTID", (string)restaurantDiningTable.RestaurantID);
                statement.AddCondition("SEQUENCE", (int)restaurantDiningTable.Sequence, SqlDbType.Int);
                statement.AddCondition("SALESTYPE", (string)restaurantDiningTable.SalesType);
                statement.AddCondition("DININGTABLELAYOUTID", (string)restaurantDiningTable.DiningTableLayoutID);
                statement.AddCondition("DINEINTABLENO", (int)restaurantDiningTable.DineInTableNo, SqlDbType.Int);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DESCRIPTION", restaurantDiningTable.Text);
            statement.AddField("TYPE", (int)restaurantDiningTable.Type, SqlDbType.Int);
            statement.AddField("NONSMOKING", restaurantDiningTable.NonSmoking ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("NOOFGUESTS", restaurantDiningTable.NoOfGuests, SqlDbType.Int);
            statement.AddField("JOINEDTABLE", restaurantDiningTable.JoinedTable, SqlDbType.Int);
            statement.AddField("X1POSITION", restaurantDiningTable.X1Position, SqlDbType.Int);
            statement.AddField("X2POSITION", restaurantDiningTable.X2Position, SqlDbType.Int);
            statement.AddField("Y1POSITION", restaurantDiningTable.Y1Position, SqlDbType.Int);
            statement.AddField("Y2POSITION", restaurantDiningTable.Y2Position, SqlDbType.Int);
            statement.AddField("LINKEDTODINEINTABLE", restaurantDiningTable.LinkedToDineInTable, SqlDbType.Int);
            statement.AddField("DININGTABLESJOINED", restaurantDiningTable.DiningTablesJoined ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("AVAILABILITY", (int)restaurantDiningTable.Availability, SqlDbType.Int);
            statement.AddField("LAYOUTSCREENNO", (int)restaurantDiningTable.LayoutScreenNo, SqlDbType.Int);
            statement.AddField("DESCRIPTIONONBUTTON", restaurantDiningTable.DescriptionOnButton);
            statement.AddField("SHAPE", (int)restaurantDiningTable.Shape, SqlDbType.Int);
            statement.AddField("X1POSITIONDESIGN", restaurantDiningTable.X1PositionDesign, SqlDbType.Int);
            statement.AddField("X2POSITIONDESIGN", restaurantDiningTable.X2PositionDesign, SqlDbType.Int);
            statement.AddField("Y1POSITIONDESIGN", restaurantDiningTable.Y1PositionDesign, SqlDbType.Int);
            statement.AddField("Y2POSITIONDESIGN", restaurantDiningTable.Y2PositionDesign, SqlDbType.Int);
            statement.AddField("LAYOUTSCREENNODESIGN", (int)restaurantDiningTable.LayoutScreenNoDesign, SqlDbType.Int);
            statement.AddField("JOINEDTABLEDESIGN", restaurantDiningTable.JoinedTableDesign, SqlDbType.Int);
            statement.AddField("DININGTABLESJOINEDDESIGN", restaurantDiningTable.DiningTablesJoinedDesign ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DELETEINOTHERLAYOUTS", restaurantDiningTable.DeleteInOtherLayouts ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
