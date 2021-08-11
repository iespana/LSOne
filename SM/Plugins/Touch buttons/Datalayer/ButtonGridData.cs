using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.SharedDatabase.Interfaces;
using LSRetail.StoreController.SharedDatabase.DataProviders;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedDatabase;
using System.Data.SqlClient;
using System.Data;
using LSRetail.StoreController.TouchButtons.Datalayer.DataEntities;
using LSRetail.StoreController.BusinessObjects;

namespace LSRetail.StoreController.TouchButtons.Datalayer
{
    /// <summary>
    /// Data provider class for Button Grids
    /// </summary>
    internal class ButtonGridData : DataProviderBase, ISequenceable
    {
        private static string BaseSQL
        {
            get
            {
                return
                   "SELECT BUTTONGRIDID" +
                   ",ISNULL(NAME,'') as NAME" +
                   ",ISNULL(SPACEBETWEENBUTTONS,0) as SPACEBETWEENBUTTONS" +
                   ",ISNULL(FONT,'') as FONT" +
                   ",ISNULL(KEYBOARDUSED,'') as KEYBOARDUSED" +
                   ",DATAAREAID" +
                   ",ISNULL(DEFAULTCOLOR,0) as DEFAULTCOLOR" +
                   ",ISNULL(DEFAULTFONTSIZE,0) as DEFAULTFONTSIZE" +
                   ",ISNULL(DEFAULTFONTSTYLE,0) as DEFAULTFONTSTYLE " +
              "FROM POSISBUTTONGRID ";
            }
        }


        private static void PopulateButtonGridInfo(SqlDataReader dr, ButtonGrid buttonGrid)
        {
            buttonGrid.ID = (string)dr["BUTTONGRIDID"];
            buttonGrid.Text = (string)dr["NAME"];
            buttonGrid.SpaceBetweenButtons = (int)dr["SPACEBETWEENBUTTONS"];
            buttonGrid.Font = (string)dr["FONT"];
            buttonGrid.KeyboardUsed = (string)dr["KEYBOARDUSED"];
            buttonGrid.DefaultColor = (int)dr["DEFAULTCOLOR"];
            buttonGrid.DefaultFontSize = (int)dr["DEFAULTFONTSIZE"];
            buttonGrid.DefaultFontStyle = (int)dr["DEFAULTFONTSTYLE"];
        }

        /// <summary>
        /// Gets a list of DataEntity that contains button grid ID and button grid Description. The list is sorted by button grid description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of DataEntity that contains button grid and button grid Description</returns>
        public static List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "POSISBUTTONGRID", "NAME", "BUTTONGRIDID", "NAME");
        }

        /// <summary>
        /// Gets a button grid with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridID">The ID of the button grid to get</param>
        /// <returns>A button grid with a given ID</returns>
        public static ButtonGrid Get(IConnectionManager entry, RecordIdentifier buttonGridID)
        {
            List<ButtonGrid> result;
            SqlCommand cmd;

            ValidateSecurity(entry);

            using (cmd = new SqlCommand())
            {
                cmd.CommandText = BaseSQL +
                    "WHERE DATAAREAID = @dataareaid AND BUTTONGRIDID = @buttonGridID";

               
                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "buttonGridID", (string)buttonGridID);

                result = Execute<ButtonGrid>(entry, cmd, CommandType.Text, PopulateButtonGridInfo);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        /// <summary>
        /// Deletes a button grid by a given ID.
        /// </summary>
        /// <remarks>You need 'TouchButtonLayoutEdit' to run this operation</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="buttonGridID">The ID of the button grid to be deleted</param>
        public static void Delete(IConnectionManager entry, RecordIdentifier buttonGridID)
        {
            DeleteRecord(entry, "POSISBUTTONGRID", "BUTTONGRIDID", buttonGridID, Permission.TouchButtonLayoutEdit);
        }

        /// <summary>
        /// Checks if a button grid by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="buttonGridID">ID of the vendor to check for</param>
        /// <returns>True if the vendor exists, else false</returns>
        public static bool Exists(IConnectionManager entry, RecordIdentifier buttonGridID)
        {
            return RecordExists(entry, "POSISBUTTONGRID", "BUTTONGRIDID", buttonGridID);
        }

        /// <summary>
        /// Saves a button grid to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <remarks>You need 'TouchButtonLayoutEdit' to run this operation</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="buttonGrid">The button grid to be saved</param>
        public static void Save(IConnectionManager entry, ButtonGrid buttonGrid)
        {
            Statement statement = new Statement("POSISBUTTONGRID");
            SqlCommand cmd = new SqlCommand();

            ValidateSecurity(entry, Permission.TouchButtonLayoutEdit);

            if (!Exists(entry, buttonGrid.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("BUTTONGRIDID", (string)buttonGrid.ID);

            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("BUTTONGRIDID", (string)buttonGrid.ID);
            }

            statement.AddField("NAME", buttonGrid.Text);
            statement.AddField("SPACEBETWEENBUTTONS", buttonGrid.SpaceBetweenButtons, SqlDbType.Int);
            statement.AddField("FONT", buttonGrid.Font);
            statement.AddField("KEYBOARDUSED", buttonGrid.KeyboardUsed);
            statement.AddField("DEFAULTCOLOR", buttonGrid.DefaultColor, SqlDbType.Int);
            statement.AddField("DEFAULTFONTSIZE", buttonGrid.DefaultFontSize, SqlDbType.Int);
            statement.AddField("DEFAULTFONTSTYLE", buttonGrid.DefaultFontStyle, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        /// <summary>
        /// Checks if a sequence with a given ID exists for vendors
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID to check for</param>
        /// <returns>True if it exists, else false</returns>
        public bool SequenceExists(IConnectionManager entry,RecordIdentifier id)
        {
            return Providers.ButtonGridData.Exists(entry, id);
        }

        /// <summary>
        /// ID into the sequence manager.
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "ButtonGrid"; }
        }

        #endregion
    }
}
