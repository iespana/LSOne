using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TouchButtons
{
    public class ButtonGridButtonsData : SqlServerDataProviderBase, IButtonGridButtonsData
    {
        private static string BaseSelectString
        {
            get
            {
                return @"select 
                        p.ID, 
                        ISNULL(p.COL,0) as COL, 
                        ISNULL(p.COLSPAN,0) as COLSPAN, 
                        ISNULL(p.ROWNUMBER,0) as ROWNUMBER, 
                        ISNULL(p.ROWSPAN,0) as ROWSPAN, 
                        ISNULL(p.ACTION,0) as ACTION, 
                        ISNULL(p.ACTIONPROPERTY,'') as ACTIONPROPERTY, 
                        ISNULL(p.PICTUREID,-1) as PICTUREID, 
                        ISNULL(p.DISPLAYTEXT,'') as DISPLAYTEXT , 
                        ISNULL(p.COLOUR,-1) as COLOUR, 
                        ISNULL(p.FONTSIZE,0) as FONTSIZE, 
                        ISNULL(p.FONTSTYLE,0) as FONTSTYLE, 
                        ISNULL(p.BUTTONGRIDID,'') as BUTTONGRIDID, 
                        ISNULL(p.IMAGEALIGNMENT,0) as IMAGEALIGNMENT,
                        pimg.PICTURE as PICTURE
                        from POSISBUTTONGRIDBUTTONS p
                        left outer join IMAGES pimg on pimg.DATAAREAID = p.DATAAREAID and pimg.PICTUREID = p.PICTUREID ";
            }
        }

        private static void PopulateButtonGridButtons(IDataReader dr, ButtonGridButton buttonGridButtons)
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
            buttonGridButtons.Picture = dr["PICTURE"] is DBNull ? null : ((byte[])dr["PICTURE"]).Length > 10 ? Image.FromStream(new System.IO.MemoryStream((byte[])dr["PICTURE"])) : null;
        }

        /// <summary>
        /// Gets a list of all ButtonGridButtons
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of ButtonGridButtons objects containing all button grid buttons</returns>
        public virtual List<ButtonGridButton> GetList(IConnectionManager entry)
        {
            var cmd = entry.Connection.CreateCommand();

            cmd.CommandText =
                BaseSelectString +
                "where p.DATAAREAID = @dataAreaId";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

            return Execute<ButtonGridButton>(entry, cmd, CommandType.Text, PopulateButtonGridButtons);
        }

        /// <summary>
        /// Gets a list of all button grid buttons belonging to a given button grid id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridID">The ID of the button grid</param>
        /// <returns>A list of ButtonGridButtons objects containing all button grid buttons for the given button grid ID</returns>
        public virtual List<ButtonGridButton> GetList(IConnectionManager entry, RecordIdentifier buttonGridID)
        {
            var cmd = entry.Connection.CreateCommand();

            cmd.CommandText =
                BaseSelectString +
                "where p.DATAAREAID = @dataAreaId and p.BUTTONGRIDID = @buttonGridId " + 
                "order by p.ROWNUMBER asc, COL asc";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            MakeParam(cmd, "buttonGridId", (string)buttonGridID);

            return Execute<ButtonGridButton>(entry, cmd, CommandType.Text, PopulateButtonGridButtons);
        }

        /// <summary>
        /// Gets a button grid button with the given id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridButtonsID">The ID of the button grid buttons to get</param>
        /// <returns>A ButtonGridButtons object containing the button grid button with the given ID</returns>
        public virtual ButtonGridButton Get(IConnectionManager entry, RecordIdentifier buttonGridButtonsID)
        {
            var cmd = entry.Connection.CreateCommand();

            cmd.CommandText =
                BaseSelectString +
                "where p.DATAAREAID = @dataAreaId and ID = @id";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            MakeParam(cmd, "id", (string)buttonGridButtonsID);

            var result = Execute<ButtonGridButton>(entry, cmd, CommandType.Text, PopulateButtonGridButtons);
            return result.Count > 0 ? result[0] : null;
        }

        /// <summary>
        /// Checks if a button grid button with the given id exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridButtonsID">The ID of the button grid button to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier buttonGridButtonsID)
        {
            return RecordExists(entry, "POSISBUTTONGRIDBUTTONS", "ID", buttonGridButtonsID);
        }

        public virtual int GetNextButtonID(IConnectionManager entry)
        {
            var cmd = entry.Connection.CreateCommand();

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
        public virtual void Delete(IConnectionManager entry, RecordIdentifier buttonGridButtonsID)
        {
            DeleteRecord(entry, "POSISBUTTONGRIDBUTTONS", "ID", buttonGridButtonsID, BusinessObjects.Permission.ManageTouchButtonLayout);
        }

        public virtual void Save(IConnectionManager entry, ButtonGridButton buttonGridButtons)
        {
            var statement = new SqlServerStatement("POSISBUTTONGRIDBUTTONS");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageTouchButtonLayout);

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

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
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
