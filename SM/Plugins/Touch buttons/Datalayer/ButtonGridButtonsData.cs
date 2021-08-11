using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LSRetail.StoreController.SharedDatabase;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.SharedDatabase.DataProviders;
using LSRetail.StoreController.SharedDatabase.Interfaces;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.TouchButtons.Datalayer.DataEntities;


namespace LSRetail.StoreController.TouchButtons.Datalayer
{
    internal class ButtonGridButtonsData : DataProviderBase //, ISequenceable
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "ID, " +
                    "ISNULL(COL,0) as COL, " +
                    "ISNULL(COLSPAN,0) as COLSPAN, " +
                    "ISNULL(ROWNUMBER,0) as ROWNUMBER, " +
                    "ISNULL(ROWSPAN,0) as ROWSPAN, " +
                    "ISNULL(ACTION,0) as ACTION, " +
                    "ISNULL(ACTIONPROPERTY,'') as ACTIONPROPERTY, " +
                    "ISNULL(PICTUREID,-1) as PICTUREID, " +
                    "ISNULL(DISPLAYTEXT,'') as DISPLAYTEXT , " +
                    "ISNULL(COLOUR,0) as COLOUR, " +
                    "ISNULL(FONTSIZE,0) as FONTSIZE, " +
                    "ISNULL(FONTSTYLE,0) as FONTSTYLE, " +
                    "ISNULL(BUTTONGRIDID,'') as BUTTONGRIDID, " +
                    "ISNULL(IMAGEALIGNMENT,0) as IMAGEALIGNMENT " +
                    "from POSISBUTTONGRIDBUTTONS ";
            }
        }

        private static void PopulateButtonGridButtons(SqlDataReader dr, ButtonGridButtons buttonGridButtons)
        {
            buttonGridButtons.ID = (int)dr["ID"];
            buttonGridButtons.Col = (int)dr["COL"];
            buttonGridButtons.ColSpan = (int)dr["COLSPAN"];
            buttonGridButtons.RowNumber = (int)dr["ROWNUMBER"];
            buttonGridButtons.RowSpan = (int)dr["ROWSPAN"];
            buttonGridButtons.Action = (int)dr["ACTION"];
            buttonGridButtons.ActionProperty = (string)dr["ACTIONPROPERTY"];
            buttonGridButtons.PictureID = (int)dr["PICTUREID"]; 
            buttonGridButtons.DisplayText = (string)dr["DISPLAYTEXT"];
            buttonGridButtons.Colour = (int)dr["COLOUR"];
            buttonGridButtons.FontSize = (int)dr["FONTSIZE"];
            buttonGridButtons.FontStyle = (int)dr["FONTSTYLE"];
            buttonGridButtons.ButtonGridID = (string)dr["BUTTONGRIDID"];
            buttonGridButtons.ImageAlignment = (int)dr["IMAGEALIGNMENT"];
        }

        /// <summary>
        /// Gets a list of all ButtonGridButtons
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of ButtonGridButtons objects containing all button grid buttons</returns>
        public static List<ButtonGridButtons> GetList(IConnectionManager entry)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText =
                BaseSelectString +
                "where DATAAREAID = @dataAreaId";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

            return Execute<ButtonGridButtons>(entry, cmd, CommandType.Text, PopulateButtonGridButtons);
        }

        /// <summary>
        /// Gets a list of all button grid buttons belonging to a given button grid id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridID">The ID of the button grid</param>
        /// <returns>A list of ButtonGridButtons objects containing all button grid buttons for the given button grid ID</returns>
        public static List<ButtonGridButtons> GetList(IConnectionManager entry, RecordIdentifier buttonGridID)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText =
                BaseSelectString +
                "where DATAAREAID = @dataAreaId and BUTTONGRIDID = @buttonGridId";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            MakeParam(cmd, "buttonGridId", (string)buttonGridID);

            return Execute<ButtonGridButtons>(entry, cmd, CommandType.Text, PopulateButtonGridButtons);
        }

        /// <summary>
        /// Gets a button grid button with the given id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridButtonsID">The ID of the button grid buttons to get</param>
        /// <returns>A ButtonGridButtons object containing the button grid button with the given ID</returns>
        public static ButtonGridButtons Get(IConnectionManager entry, RecordIdentifier buttonGridButtonsID)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText =
                BaseSelectString +
                "where DATAAREAID = @dataAreaId and ID = @id";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            MakeParam(cmd, "id", (string)buttonGridButtonsID);

            List<ButtonGridButtons> result = Execute<ButtonGridButtons>(entry, cmd, CommandType.Text, PopulateButtonGridButtons);
            return result.Count > 0 ? result[0] : null;
        }

        /// <summary>
        /// Checks if a button grid button with the given id exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridButtonsID">The ID of the button grid button to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public static bool Exists(IConnectionManager entry, RecordIdentifier buttonGridButtonsID)
        {
            return RecordExists(entry, "POSISBUTTONGRIDBUTTONS", "ID", buttonGridButtonsID);
        }

        public static int GetNextButtonID(IConnectionManager entry)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "select " +
                "MAX(ID) as MAXID " +
                "from POSISBUTTONGRIDBUTTONS " +
                "where DATAAREAID = @dataAreaId";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

            return (int)entry.Connection.ExecuteScalar(cmd) + 1;
        }

        /// <summary>
        /// Deletes the button grid button with the given id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridButtonsID">The ID of the button grid button to delete</param>
        public static void Delete(IConnectionManager entry, RecordIdentifier buttonGridButtonsID)
        {
            DeleteRecord(entry, "POSISBUTTONGRIDBUTTONS", "ID", buttonGridButtonsID, Permission.TouchButtonLayoutEdit);
        }

        public static void Save(IConnectionManager entry, ButtonGridButtons buttonGridButtons)
        {
            Statement statement = new Statement("POSISBUTTONGRIDBUTTONS");
            SqlCommand cmd = new SqlCommand();

            ValidateSecurity(entry, Permission.TouchButtonLayoutEdit);

            if (!Exists(entry, buttonGridButtons.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (int)buttonGridButtons.ID, SqlDbType.Int);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (int)buttonGridButtons.ID, SqlDbType.Int);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("COL", buttonGridButtons.Col, SqlDbType.Int);
            statement.AddField("COLSPAN", buttonGridButtons.ColSpan, SqlDbType.Int);
            statement.AddField("ROWNUMBER", buttonGridButtons.RowNumber, SqlDbType.Int);
            statement.AddField("ROWSPAN", buttonGridButtons.RowSpan, SqlDbType.Int);
            statement.AddField("ACTION", buttonGridButtons.Action, SqlDbType.Int);
            statement.AddField("ACTIONPROPERTY", buttonGridButtons.ActionProperty);
            statement.AddField("PICTUREID", (int)buttonGridButtons.PictureID, SqlDbType.Int);
            statement.AddField("DISPLAYTEXT", buttonGridButtons.DisplayText);
            statement.AddField("COLOUR", buttonGridButtons.Colour, SqlDbType.Int);
            statement.AddField("FONTSIZE", buttonGridButtons.FontSize, SqlDbType.Int);
            statement.AddField("FONTSTYLE", buttonGridButtons.FontStyle, SqlDbType.Int);
            statement.AddField("BUTTONGRIDID", (string)buttonGridButtons.ButtonGridID);
            statement.AddField("IMAGEALIGNMENT", buttonGridButtons.ImageAlignment, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
        /*
        #region ISequenceable Members

        public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "ButtonGridButtons"; }
        }

        #endregion
         */
    }
}
