using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class DiningTableLayoutData : SqlServerDataProviderBase, IDiningTableLayoutData
    {
        /// <summary>
        /// Returns the basic SQL select query string that is used throughout this dataprovider class
        /// </summary>
        /// <returns>The base SQL select query for DiningTableLayout</returns>
        private static string BaseSelectString
        {
            get
            {
                return "select d.RESTAURANTID as RESTAURANTID, " +
                "d.SEQUENCE as SEQUENCE, " +
                "d.SALESTYPE as SALESTYPE, " +
                "d.LAYOUTID as LAYOUTID, " +
                "ISNULL(d.DESCRIPTION,'') as DESCRIPTION, " +
                "ISNULL(d.NOOFSCREENS,0) as NOOFSCREENS, " +
                "ISNULL(d.STARTINGTABLENO,0) as STARTINGTABLENO, " +
                "ISNULL(d.NOOFDININGTABLES,0) as NOOFDININGTABLES, " +
                "ISNULL(d.ENDINGTABLENO,0) as ENDINGTABLENO, " +
                "ISNULL(d.DININGTABLEROWS,0) as DININGTABLEROWS, " +
                "ISNULL(d.DININGTABLECOLUMNS,0) as DININGTABLECOLUMNS, " +
                "ISNULL(d.CURRENTLAYOUT,0) as CURRENTLAYOUT, " +
                "ISNULL(h.DESCRIPTION,'') as HOSPITALITYTYPEDESCRIPTION " +
                "from DININGTABLELAYOUT d " +
                "left outer join HOSPITALITYTYPE h on h.RESTAURANTID = d.RESTAURANTID and h.SALESTYPE = d.SALESTYPE and h.DATAAREAID = d.DATAAREAID ";
            }
        }

        /// <summary>
        /// Populate a DiningTableLayout object
        /// </summary>
        /// <param name="reader">The datareader containing the data for the DiningTableLayout</param>
        /// <param name="diningTableLayout">The DiningTableLayout object to populate</param>
        private static void PopulateDiningTableLayout(IDataReader reader, DiningTableLayout diningTableLayout)
        {
            diningTableLayout.RestaurantID = (string)reader["RESTAURANTID"];
            diningTableLayout.Sequence = (int)reader["SEQUENCE"];
            diningTableLayout.SalesType = (string)reader["SALESTYPE"];
            diningTableLayout.LayoutID = (string)reader["LAYOUTID"];
            diningTableLayout.Text = (string)reader["DESCRIPTION"];
            diningTableLayout.NumberOfScreens = (int)reader["NOOFSCREENS"];
            diningTableLayout.StartingTableNumber = (int)reader["STARTINGTABLENO"];
            diningTableLayout.NumberOfDiningTables = (int)reader["NOOFDININGTABLES"];
            diningTableLayout.EndingTableNumber = (int)reader["ENDINGTABLENO"];
            diningTableLayout.DiningTableRows = (int)reader["DININGTABLEROWS"];
            diningTableLayout.DiningTableColumns = (int)reader["DININGTABLECOLUMNS"];
            diningTableLayout.CurrentLayout = ((byte)reader["CURRENTLAYOUT"] != 0);
            diningTableLayout.HospitalityTypeDescription = (string)reader["HOSPITALITYTYPEDESCRIPTION"];
        }

        /// <summary>
        /// Gets a list of all Dining Table Layouts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all DiningTableLayouts</returns>
        public virtual List<DiningTableLayout> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where d.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DiningTableLayout>(entry, cmd, CommandType.Text, PopulateDiningTableLayout);
            }
        }

        /// <summary>
        /// Gets all DiningTablesLayouts for the specific Restaurant and Sales Type combination
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id for the Restaurant</param>
        /// <param name="salesTypeID">The id for the Sales Type</param>
        /// <returns>A list of all DiningTableLayouts for the specidfic Restaurant and Sales Type</returns>
        public virtual List<DiningTableLayout> GetList(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where d.DATAAREAID = @dataAreaId and d.RESTAURANTID = @restaurantId and d.SALESTYPE = @salesTypeId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", restaurantID);
                MakeParam(cmd, "salesTypeId", salesTypeID);

                return Execute<DiningTableLayout>(entry, cmd, CommandType.Text, PopulateDiningTableLayout);
            }
        }

        /// <summary>
        /// Gets a specific DiningTableLayout with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the diningTableLayout to get</param>
        /// <param name="cacheType"></param>
        /// <returns>The DiningTableLayout matching the given ID</returns>
        public virtual DiningTableLayout Get(IConnectionManager entry, RecordIdentifier diningTableLayoutID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where d.DATAAREAID = @dataAreaId and d.RESTAURANTID = @restaurantId and d.SEQUENCE = @sequence and d.SALESTYPE = @salesType and d.LAYOUTID = @layoutId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", diningTableLayoutID[0]);
                MakeParam(cmd, "sequence", diningTableLayoutID[1]);
                MakeParam(cmd, "salesType", diningTableLayoutID[2]);
                MakeParam(cmd, "layoutId", diningTableLayoutID[3]);

                return Get<DiningTableLayout>(entry, cmd, diningTableLayoutID, PopulateDiningTableLayout, cacheType, UsageIntentEnum.Normal);
            }
        }


        /// <summary>
        /// Gets the maximum number of tables (diningtablerows * diningtablecolumns) that is allowed for the given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The maximum number of tables</returns>
        public virtual int GetMaximumNumberOfTables(IConnectionManager entry, RecordIdentifier diningTableLayoutID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select DININGTABLEROWS * DININGTABLECOLUMNS " +
                    "from DININGTABLELAYOUT " +
                    "where DATAAREAID = @dataAreaId and RESTAURANTID = @restaurantId and SEQUENCE = @sequence and SALESTYPE = @salesType and LAYOUTID = @layoutId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", diningTableLayoutID[0]);
                MakeParam(cmd, "sequence", diningTableLayoutID[1]);
                MakeParam(cmd, "salesType", diningTableLayoutID[2]);
                MakeParam(cmd, "layoutId", diningTableLayoutID[3]);

                return (int) entry.Connection.ExecuteScalar(cmd);
            }
        }        

        /// <summary>
        /// Checks if a DiningTableLayout with a given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the DiningTableLayout to check for</param>
        /// <returns>True if the DiningTableLayout exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier diningTableLayoutID)
        {
            return RecordExists(entry, "DININGTABLELAYOUT", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE", "LAYOUTID" }, diningTableLayoutID);
        }

        /// <summary>
        /// Checks if a DiningTableLayout with a given LayoutID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="layoutID">The LayoutID to check for</param>
        /// <returns>True if the DiningTableLayout exists, false otherwise</returns>
        public virtual bool LayoutIDExists(IConnectionManager entry, RecordIdentifier layoutID)
        {
            return RecordExists(entry, "DININGTABLELAYOUT", "LAYOUTID", layoutID);
        }

        /// <summary>
        /// Deletes a dining table layout with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier diningTableLayoutID)
        {
            DeleteRecord(entry, "DININGTABLELAYOUT", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE", "LAYOUTID" }, diningTableLayoutID, BusinessObjects.Permission.ManageDiningTableLayouts);

            // Also delete all restaurant dining tables for this diningtablelayoutid
        }

        /// <summary>
        /// Deletes all dining table layouts belonging to a specific hospitality type (restaurantid + salestype)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <param name="salesType">The ID of the sales type</param>
        public virtual void DeleteForHospitalityType(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesType)
        {
            var hospitalityTypeID = new RecordIdentifier(restaurantID, salesType);

            DeleteRecord(entry, "DININGTABLELAYOUT", new[] { "RESTAURANTID", "SALESTYPE" }, hospitalityTypeID, BusinessObjects.Permission.ManageDiningTableLayouts);
        }

        /// <summary>
        /// Checks if a dining table layout with the given restaurant id and sales type combination exists.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <param name="salesTypeID">The id of the sales type</param>
        /// <returns>True if the given combination exists, false otherwise</returns>
        public virtual bool RestaurantSalesTypeCombinationExists(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID)
        {
            var combinationID = new RecordIdentifier(restaurantID, salesTypeID);

            return RecordExists(entry, "DININGTABLELAYOUT", new[] { "RESTAURANTID", "SALESTYPE" }, combinationID);
        }

        /// <summary>
        /// Copies information from another dining table layout into the DiningTableLayout object.
        /// The fields that are copied are: NoOfScreens, StartingTableNo, NoOfDiningTables, EndingTableNo, DiningTableRows, DiningTableColumns, CurrentLayout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout to copy from</param>
        /// <param name="diningTableLayout">The DininTableLayout object to copy the data into</param>
        public virtual void CopyDataFromLayout(IConnectionManager entry, RecordIdentifier diningTableLayoutID, DiningTableLayout diningTableLayout)
        {
            var diningTableLayoutCopy = Get(entry, diningTableLayoutID);

            diningTableLayout.NumberOfScreens = diningTableLayoutCopy.NumberOfScreens;
            diningTableLayout.StartingTableNumber = diningTableLayoutCopy.StartingTableNumber;
            diningTableLayout.NumberOfDiningTables = diningTableLayoutCopy.NumberOfDiningTables;
            diningTableLayout.EndingTableNumber = diningTableLayoutCopy.EndingTableNumber;
            diningTableLayout.DiningTableRows = diningTableLayoutCopy.DiningTableRows;
            diningTableLayout.DiningTableColumns = diningTableLayoutCopy.DiningTableColumns;
            diningTableLayout.CurrentLayout = diningTableLayoutCopy.CurrentLayout;
        }

        /// <summary>
        /// Saves a dining table layout into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayout">The DiningTableLayout object to save</param>
        public virtual void Save(IConnectionManager entry, DiningTableLayout diningTableLayout)
        {
            var statement = new SqlServerStatement("DININGTABLELAYOUT");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageDiningTableLayouts);

            bool isNew = false;
            if (diningTableLayout.LayoutID == RecordIdentifier.Empty)
            {
                isNew = true;
                diningTableLayout.LayoutID = DataProviderFactory.Instance.GenerateNumber<IDiningTableLayoutData, DiningTableLayout>(entry);
            }

            if (isNew || !Exists(entry, diningTableLayout.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("RESTAURANTID", (string) diningTableLayout.RestaurantID);
                statement.AddKey("SEQUENCE", (int) diningTableLayout.Sequence, SqlDbType.Int);
                statement.AddKey("SALESTYPE", (string) diningTableLayout.SalesType);
                statement.AddKey("LAYOUTID", (string) diningTableLayout.LayoutID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("RESTAURANTID", (string) diningTableLayout.RestaurantID);
                statement.AddCondition("SEQUENCE", (int) diningTableLayout.Sequence, SqlDbType.Int);
                statement.AddCondition("SALESTYPE", (string) diningTableLayout.SalesType);
                statement.AddCondition("LAYOUTID", (string) diningTableLayout.LayoutID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DESCRIPTION", diningTableLayout.Text);
            statement.AddField("NOOFSCREENS", diningTableLayout.NumberOfScreens, SqlDbType.Int);
            statement.AddField("STARTINGTABLENO", diningTableLayout.StartingTableNumber, SqlDbType.Int);
            statement.AddField("NOOFDININGTABLES", diningTableLayout.NumberOfDiningTables, SqlDbType.Int);
            statement.AddField("ENDINGTABLENO", diningTableLayout.EndingTableNumber, SqlDbType.Int);
            statement.AddField("DININGTABLEROWS", diningTableLayout.DiningTableRows, SqlDbType.Int);
            statement.AddField("DININGTABLECOLUMNS", diningTableLayout.DiningTableColumns, SqlDbType.Int);
            statement.AddField("CURRENTLAYOUT", diningTableLayout.CurrentLayout ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return LayoutIDExists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "DiningTableLayout"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "DININGTABLELAYOUT", "LAYOUTID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
