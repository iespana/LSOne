using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TouchButtons
{
    /// <summary>
    /// Data provider class for Button Grids
    /// </summary>
    public class ButtonGridData : SqlServerDataProviderBase, IButtonGridData
    {
        private static string BaseSql
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

        private static void PopulateButtonGridInfo(IDataReader dr, ButtonGrid buttonGrid)
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
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "POSISBUTTONGRID", "NAME", "BUTTONGRIDID", "NAME");
        }

        /// <summary>
        /// Gets a list of all button grids
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        public virtual List<ButtonGrid> GetButtonGrids(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                    "WHERE DATAAREAID = @dataareaid";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);

                return Execute<ButtonGrid>(entry, cmd, CommandType.Text, PopulateButtonGridInfo);
            }
        }

        /// <summary>
        /// Gets a button grid with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="buttonGridID">The ID of the button grid to get</param>
        /// <returns>A button grid with a given ID</returns>
        public virtual ButtonGrid Get(IConnectionManager entry, RecordIdentifier buttonGridID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                    "WHERE DATAAREAID = @dataareaid AND BUTTONGRIDID = @buttonGridID";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "buttonGridID", (string)buttonGridID);

                var result = Execute<ButtonGrid>(entry, cmd, CommandType.Text, PopulateButtonGridInfo);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Deletes a button grid by a given ID.
        /// </summary>
        /// <remarks>You need 'TouchButtonLayoutEdit' to run this operation</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="buttonGridID">The ID of the button grid to be deleted</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier buttonGridID)
        {
            DeleteRecord(entry, "POSISBUTTONGRID", "BUTTONGRIDID", buttonGridID, BusinessObjects.Permission.ManageTouchButtonLayout);
        }

        /// <summary>
        /// Checks if a button grid by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="buttonGridID">ID of the vendor to check for</param>
        /// <returns>True if the vendor exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier buttonGridID)
        {
            return RecordExists(entry, "POSISBUTTONGRID", "BUTTONGRIDID", buttonGridID);
        }

        /// <summary>
        /// Saves a button grid to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <remarks>You need 'TouchButtonLayoutEdit' to run this operation</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="buttonGrid">The button grid to be saved</param>
        public virtual void Save(IConnectionManager entry, ButtonGrid buttonGrid)
        {
            var statement = new SqlServerStatement("POSISBUTTONGRID");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageTouchButtonLayout);

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
        public virtual bool SequenceExists(IConnectionManager entry,RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// ID into the sequence manager.
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "ButtonGrid"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSISBUTTONGRID", "BUTTONGRIDID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
