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
    public class RestaurantMenuTypeData : SqlServerDataProviderBase, IRestaurantMenuTypeData
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "RESTAURANTID, " +
                    "MENUORDER, " +
                    "ISNULL(DESCRIPTION,'') as DESCRIPTION, " +
                    "ISNULL(CODEONPOS,'') as CODEONPOS " +
                    "from RESTAURANTMENUTYPE ";
            }
        }

        private static void PopulateRestaurantMenuType(IDataReader dr, RestaurantMenuType restaurantMenuType)
        {
            restaurantMenuType.RestaurantID = (string)dr["RESTAURANTID"];
            restaurantMenuType.MenuOrder = (int)dr["MENUORDER"];
            restaurantMenuType.Text = (string)dr["DESCRIPTION"];
            restaurantMenuType.CodeOnPos = (string)dr["CODEONPOS"];
        }

        /// <summary>
        /// Gets a list of all restaurant menu types
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of RestaurantMenuType objects containing all restaurant menu types</returns>
        public virtual List<RestaurantMenuType> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<RestaurantMenuType>(entry, cmd, CommandType.Text, PopulateRestaurantMenuType);
            }
        }

        /// <summary>
        /// Gets a list of all restaurant menu types for the given restaurant id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <returns>A list of RestaurantMenuType objects containing all restaurant menu types for the given restaurant id</returns>
        public virtual List<RestaurantMenuType> GetList(IConnectionManager entry, RecordIdentifier restaurantID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and RESTAURANTID = @restaurantId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) restaurantID);

                return Execute<RestaurantMenuType>(entry, cmd, CommandType.Text, PopulateRestaurantMenuType);
            }
        }

        /// <summary>
        /// Gets a restaurant menu type with the given id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantMenuTypeID">The id of the restaurant meny type to get</param>
        /// <returns>The restaurant menu type with the given ID</returns>
        public virtual RestaurantMenuType Get(IConnectionManager entry, RecordIdentifier restaurantMenuTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and RESTAURANTID = @restaurantId and MENUORDER = @menuOrder";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string)restaurantMenuTypeID[0]);
                MakeParam(cmd, "menuOrder", (int)restaurantMenuTypeID[1]);

                var result = Execute<RestaurantMenuType>(entry, cmd, CommandType.Text, PopulateRestaurantMenuType);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Checks if a given restaurant meny type exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantMenuTypeID">The id of the restaurant meny type to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier restaurantMenuTypeID)
        {
            return RecordExists(entry, "RESTAURANTMENUTYPE", new[] { "RESTAURANTID", "MENUORDER" }, restaurantMenuTypeID);
        }

        /// <summary>
        /// Cheks if a given restaurant menu type and codeOnPos combination exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantMenuTypeID">The id of the restaurant meny type to check for</param>
        /// <param name="codeOnPos">The code on pos value to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier restaurantMenuTypeID, string codeOnPos)
        {
            var combination = new RecordIdentifier(restaurantMenuTypeID[0], 
                new RecordIdentifier(restaurantMenuTypeID[1], codeOnPos));

            return RecordExists(entry, "RESTAURANTMENUTYPE", new[] { "RESTAURANTID", "MENUORDER", "CODEONPOS" }, combination);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier restaurantMenuTypeID)
        {
            DeleteRecord(entry, "RESTAURANTMENUTYPE", new[] { "RESTAURANTID", "MENUORDER" }, restaurantMenuTypeID, BusinessObjects.Permission.ManageRestaurantMenuTypes);
        }

        /// <summary>
        /// Inserts a new restaurant menu type or updates an existing one
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantMenuType">The RestaurantMenuType object to save</param>
        public virtual void Save(IConnectionManager entry, RestaurantMenuType restaurantMenuType)
        {
            var statement = new SqlServerStatement("RESTAURANTMENUTYPE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageSalesTypes);

            if (!Exists(entry, restaurantMenuType.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("RESTAURANTID", (string)restaurantMenuType.RestaurantID);
                statement.AddKey("MENUORDER", (int)restaurantMenuType.MenuOrder, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("RESTAURANTID", (string)restaurantMenuType.RestaurantID);
                statement.AddCondition("MENUORDER", (int)restaurantMenuType.MenuOrder, SqlDbType.Int);
            }
            
            statement.AddField("DESCRIPTION", restaurantMenuType.Text);
            statement.AddField("CODEONPOS", restaurantMenuType.CodeOnPos);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
