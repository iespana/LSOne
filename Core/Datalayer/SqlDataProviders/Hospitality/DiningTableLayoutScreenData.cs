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
    public class DiningTableLayoutScreenData : SqlServerDataProviderBase, IDiningTableLayoutScreenData
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "RESTAURANTID, " +
                    "SEQUENCE, " +
                    "SALESTYPE, " +
                    "LAYOUTID, " +
                    "SCREENNO, " +
                    "ISNULL(SCREENDESCRIPTION,'') as SCREENDESCRIPTION, " +
                    "BACKGROUNDIMAGE " +
                    "from DININGTABLELAYOUTSCREEN ";
            }
        }

        /// <summary>
        /// Populates the fields of a DiningTableLayout object
        /// </summary>
        /// <param name="dr">The SqlDataReader holding the data for the object</param>
        /// <param name="layoutScreen">The DiningTableLayoutScreen object to populate</param>
        private static void PopulateDiningTableLayoutScreen(IDataReader dr, DiningTableLayoutScreen layoutScreen)
        {
            layoutScreen.RestaurantID = (string)dr["RESTAURANTID"];
            layoutScreen.Sequence = (int)dr["SEQUENCE"];
            layoutScreen.SalesType = (string)dr["SALESTYPE"];
            layoutScreen.LayoutID = (string)dr["LAYOUTID"];
            layoutScreen.ScreenNo = (int)dr["SCREENNO"];
            layoutScreen.Text = (string)dr["SCREENDESCRIPTION"];
            // TODO: add a background image
        }

        /// <summary>
        /// Gets all dining table layout screens
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of DiningTableLayoutScreen objects containing all dining table layout screens</returns>
        public virtual List<DiningTableLayoutScreen> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DiningTableLayoutScreen>(entry, cmd, CommandType.Text, PopulateDiningTableLayoutScreen);
            }
        }

        /// <summary>
        /// Gets all dining table layout screens for a given hospitality type and dining table layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant of the hospitality type</param>
        /// <param name="sequence">The sequence of the hospitality type</param>
        /// <param name="salesType">The sales type of the hospitality type</param>
        /// <param name="diningTableLayoutID">The dining table layout id</param>
        /// <returns>All dining table layout screens for the given id's</returns>
        public virtual List<DiningTableLayoutScreen> GetList(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier sequence, RecordIdentifier salesType, RecordIdentifier diningTableLayoutID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaID and RESTAURANTID = @restaurantId and SEQUENCE = @sequence and SALESTYPE = @salesType and LAYOUTID = @diningTableLayoutId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) restaurantID);
                MakeParam(cmd, "sequence", (int) sequence, SqlDbType.Int);
                MakeParam(cmd, "salesType", (string) salesType);
                MakeParam(cmd, "diningTableLayoutId", (string) diningTableLayoutID);

                return Execute<DiningTableLayoutScreen>(entry, cmd, CommandType.Text, PopulateDiningTableLayoutScreen);
            }
        }

        /// <summary>
        /// Gets a specific dining table layout screen with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutScreenID">The ID of the dining table layout screen to get</param>
        /// <returns>A DiningTableLayoutScreen object containing the layout screen</returns>
        public virtual DiningTableLayoutScreen Get(IConnectionManager entry, RecordIdentifier diningTableLayoutScreenID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaID and RESTAURANTID = @restaurantId and SEQUENCE = @sequence and SALESTYPE = @salesType and LAYOUTID = @diningTableLayoutId and SCREENNO = @screenNo";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) diningTableLayoutScreenID[0]);
                MakeParam(cmd, "sequence", (int) diningTableLayoutScreenID[1], SqlDbType.Int);
                MakeParam(cmd, "salesType", (string) diningTableLayoutScreenID[2]);
                MakeParam(cmd, "diningTableLayoutId", (string) diningTableLayoutScreenID[3]);
                MakeParam(cmd, "screenNo", (int) diningTableLayoutScreenID[4]);

                var result = Execute<DiningTableLayoutScreen>(entry, cmd, CommandType.Text,
                    PopulateDiningTableLayoutScreen);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Returns the number of dining table layout screens for a given dining table layout ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>The number of screens for the given dining table layout</returns>
        public virtual int GetNumberOfScreens(IConnectionManager entry, RecordIdentifier diningTableLayoutID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select COUNT(SCREENNO) " +
                    "from DININGTABLELAYOUTSCREEN " +
                    "where DATAAREAID = @dataAreaId and RESTAURANTID = @restaurantId AND SEQUENCE = @sequence and SALESTYPE=@salesType AND LAYOUTID = @layoutId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) diningTableLayoutID[0]);
                MakeParam(cmd, "sequence", (int) diningTableLayoutID[1], SqlDbType.Int);
                MakeParam(cmd, "salesType", (string) diningTableLayoutID[2]);
                MakeParam(cmd, "layoutId", (string) diningTableLayoutID[3]);

                return (int) entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Checks wether a dining table layout screen exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutScreenID">The ID of the dining table layout screen to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier diningTableLayoutScreenID)
        {
            return RecordExists(entry, "DININGTABLELAYOUTSCREEN", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE", "LAYOUTID", "SCREENNO" }, diningTableLayoutScreenID);            
        }

        /// <summary>
        /// Checks wether the given layout screen is in use by a dining table layout 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="diningTableLayoutScreenID"></param>
        /// <returns></returns>
        public virtual bool ScreenIsInUse(IConnectionManager entry, RecordIdentifier diningTableLayoutScreenID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT COUNT(d.SCREENNO) " +
                    " FROM DININGTABLELAYOUTSCREEN d " +
                    " INNER JOIN RESTAURANTDININGTABLE r ON r.LAYOUTSCREENNO = d.SCREENNO AND r.DININGTABLELAYOUTID = d.LAYOUTID " +
                    " WHERE d.DATAAREAID = @dataAreaId AND d.RESTAURANTID = @restaurantId AND d.SEQUENCE = @sequence AND d.SALESTYPE = @salesType AND d.LAYOUTID = @layoutId AND d.SCREENNO = @screenNo";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", diningTableLayoutScreenID[0]);
                MakeParam(cmd, "sequence", diningTableLayoutScreenID[1]);
                MakeParam(cmd, "salesType", diningTableLayoutScreenID[2]);
                MakeParam(cmd, "layoutId", diningTableLayoutScreenID[3]);
                MakeParam(cmd, "screenNo", diningTableLayoutScreenID[4]);

                return (int) entry.Connection.ExecuteScalar(cmd) > 0;
            }
        }

        /// <summary>
        /// Deletes a specific dining table layout screen
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutScreenID"></param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier diningTableLayoutScreenID)
        {
            DeleteRecord(entry, "DININGTABLELAYOUTSCREEN", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE", "LAYOUTID", "SCREENNO" }, diningTableLayoutScreenID, BusinessObjects.Permission.ManageDiningTableLayouts);            
        }

        /// <summary>
        /// Deletes all dining table layout screens for a given dining table layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        public virtual void DeleteForDiningTableLayout(IConnectionManager entry, RecordIdentifier diningTableLayoutID)
        {
            DeleteRecord(entry, "DININGTABLELAYOUTSCREEN", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE", "LAYOUTID" }, diningTableLayoutID, BusinessObjects.Permission.ManageDiningTableLayouts);
        }

        /// <summary>
        /// Deletes all dining table layout screens belonging to a specific hospitality type (restaurantid + salestype)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <param name="salesType">The ID of the sales type</param>
        public virtual void DeleteForHospitalityType(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesType)
        {
            var hospitalityTypeID = new RecordIdentifier(restaurantID, salesType);

            DeleteRecord(entry, "DININGTABLELAYOUTSCREEN", new[] { "RESTAURANTID", "SALESTYPE" }, hospitalityTypeID, BusinessObjects.Permission.ManageDiningTableLayouts);
        }

        public virtual void Save(IConnectionManager entry, DiningTableLayoutScreen layoutScreen)
        {
            var statement = new SqlServerStatement("DININGTABLELAYOUTSCREEN");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageDiningTableLayouts);

            if (!Exists(entry, layoutScreen.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("RESTAURANTID", (string)layoutScreen.RestaurantID);
                statement.AddKey("SEQUENCE", (int)layoutScreen.Sequence, SqlDbType.Int);
                statement.AddKey("SALESTYPE", (string)layoutScreen.SalesType);
                statement.AddKey("LAYOUTID", (string)layoutScreen.LayoutID);
                statement.AddKey("SCREENNO", (int)layoutScreen.ScreenNo, SqlDbType.Int);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("RESTAURANTID", (string)layoutScreen.RestaurantID);
                statement.AddCondition("SEQUENCE", (int)layoutScreen.Sequence, SqlDbType.Int);
                statement.AddCondition("SALESTYPE", (string)layoutScreen.SalesType);
                statement.AddCondition("LAYOUTID", (string)layoutScreen.LayoutID);
                statement.AddCondition("SCREENNO", (int)layoutScreen.ScreenNo, SqlDbType.Int);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("SCREENDESCRIPTION", layoutScreen.Text);
            //TODO: add backgroundimage

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
