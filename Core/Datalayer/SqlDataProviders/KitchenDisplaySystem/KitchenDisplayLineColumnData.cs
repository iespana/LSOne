using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    /// <summary>
    /// Data provider class for KitchenDisplayLines
    /// </summary>
    public class KitchenDisplayLineColumnData : SqlServerDataProviderBase, IKitchenDisplayLineColumnData
    {
        private static string BaseSelectString
        {
            get { return @"select
                        lc.ID,
                        lc.LINENUMBERID,
                        lc.COLUMNNUMBER,
                        lc.COLUMNTYPE,
                        lc.ORDERPROPERTY,
                        lc.CAPTION,
                        lc.MAPPINGKEY,
                        lc.ALIGNMENT,
                        lc.RELATIVESIZE,
                        lc.COLORSTYLE,
                        ISNULL(ps.NAME, '') as DESCRIPTION
                        from KITCHENDISPLAYLINECOLUMN lc left outer join POSSTYLE ps on ps.GUID = lc.COLORSTYLE "; }
        }

        private static void PopulateKitchenDisplayLineColumn(IDataReader dr, KitchenDisplayLineColumn displayLineColumn)
        {
            displayLineColumn.ID = (Guid)dr["ID"];
            if (dr["LINENUMBERID"] != DBNull.Value)
            {
                displayLineColumn.LineNumberID = (Guid)dr["LINENUMBERID"];
            }
            displayLineColumn.No = (int)dr["COLUMNNUMBER"];
            displayLineColumn.Type = (PartTypeEnum)AsInt(dr["COLUMNTYPE"]);
            displayLineColumn.OrderProperty = (PartOrderPropertyEnum)AsInt(dr["ORDERPROPERTY"]);
            displayLineColumn.Caption = (string)dr["CAPTION"];
            displayLineColumn.MappingKey = (string)dr["MAPPINGKEY"];
            displayLineColumn.Alignment = (PartAlignmentEnum)AsInt(dr["ALIGNMENT"]);
            displayLineColumn.RelativeSize = (short)AsInt(dr["RELATIVESIZE"]);
            displayLineColumn.ColorStyle = new BaseStyle(); 
            if (dr["COLORSTYLE"] != DBNull.Value)
            {
                displayLineColumn.StyleID = Guid.Parse((string)dr["COLORSTYLE"]);
                displayLineColumn.StyleDescription = (string)dr["DESCRIPTION"];
            }
        }

        private void AddStyle(IConnectionManager entry, KitchenDisplayLineColumn column)
        {
            PosStyle style = Providers.PosStyleData.GetByGuid(entry, column.StyleID);
            column.ColorStyle = style == null ? new BaseStyle() : PosStyle.ToBaseStyle(style);
        }

        /// <summary>
        /// Gets a list of all linecolumns for the selected line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of linecolumn objects containing all linecolumn records</returns>
        public virtual List<KitchenDisplayLineColumn> GetList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where LINENUMBERID = @lineNoId order by COLUMNNUMBER";

                MakeParam(cmd, "lineNoId", (Guid)id.PrimaryID, SqlDbType.UniqueIdentifier);

                List<KitchenDisplayLineColumn> result = Execute<KitchenDisplayLineColumn>(entry, cmd, CommandType.Text, PopulateKitchenDisplayLineColumn);
                result.ForEach(r => AddStyle(entry, r));
                return result;
            }
        }

        /// <summary>
        /// Gets a list of all linecolumns
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of linecolumn objects containing all linecolumn records</returns>
        public virtual List<KitchenDisplayLineColumn> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSelectString + "order by LINENUMBERID, COLUMNNUMBER";

                List<KitchenDisplayLineColumn> result = Execute<KitchenDisplayLineColumn>(entry, cmd, CommandType.Text, PopulateKitchenDisplayLineColumn);
                result.ForEach(r => AddStyle(entry, r));
                return result;
            }
        }

        /// <summary>
        /// Gets a linecolumn with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeaderID">The ID of the linecolumn to get</param>
        /// <returns>A KitchenDisplayLineColumn object containing the column with the given ID</returns>
        public virtual KitchenDisplayLineColumn Get(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where lc.ID = @columnId";

                MakeParam(cmd, "columnId", (Guid)id.PrimaryID, SqlDbType.UniqueIdentifier);

                List<KitchenDisplayLineColumn> result = Execute<KitchenDisplayLineColumn>(entry, cmd, CommandType.Text, PopulateKitchenDisplayLineColumn);

                if (result.Count == 1)
                {
                    AddStyle(entry, result[0]);
                    return result[0];
                }

                return null;
            }
        }

        /// <summary>
        /// Checks if a kitchendisplayline with the given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the line to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id, int columnNo)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select * from KITCHENDISPLAYLINECOLUMN where LINENUMBERID = @lineNoId and COLUMNNUMBER = @colNo";

                MakeParam(cmd, "lineNoId", (Guid)id.PrimaryID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "colNo", columnNo, SqlDbType.Int);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the kitchendisplayline with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the pos menu header to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id, int columnNo)
        {
            SqlServerStatement statement = new SqlServerStatement("KITCHENDISPLAYLINECOLUMN", StatementType.Delete);

            statement.AddCondition("LINENUMBERID", id.PrimaryID, SqlDbType.UniqueIdentifier);
            statement.AddCondition("COLUMNNUMBER", columnNo, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual short MaxColumnNo(IConnectionManager entry, KitchenDisplayLineColumn displayColumn)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select max(COLUMNNUMBER) from KITCHENDISPLAYLINECOLUMN where LINENUMBERID=@lineID";

                MakeParam(cmd, "lineID", (Guid)displayColumn.LineNumberID, SqlDbType.UniqueIdentifier);

                object result;
                lock (entry)
                {
                    result = entry.Connection.ExecuteScalar(cmd);
                }

                if (result == DBNull.Value || result == null)
                {
                    return 0;
                }
                short i = Convert.ToInt16(result);
                return i;
            }
        }

        /// <summary>
        /// Saves a kitchendisplayline object into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeader"></param>
        public virtual void Save(IConnectionManager entry, KitchenDisplayLineColumn displayLineColumn)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYLINECOLUMN");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenDisplayProfiles);

            bool isNew = false;
            if (displayLineColumn.ID.IsEmpty)
            {
                displayLineColumn.ID = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, displayLineColumn.LineNumberID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (Guid)displayLineColumn.ID, SqlDbType.UniqueIdentifier);
                statement.AddKey("LINENUMBERID", ((string)displayLineColumn.LineNumberID).ToUpper());
                statement.AddField("COLUMNNUMBER", MaxColumnNo(entry, displayLineColumn) + 1, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (string)displayLineColumn.ID);
                statement.AddCondition("LINENUMBERID", (Guid)displayLineColumn.LineNumberID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("COLUMNTYPE", displayLineColumn.Type, SqlDbType.TinyInt);
            statement.AddField("ORDERPROPERTY", displayLineColumn.OrderProperty, SqlDbType.TinyInt);
            statement.AddField("CAPTION", displayLineColumn.Caption);
            statement.AddField("MAPPINGKEY", displayLineColumn.MappingKey);
            statement.AddField("ALIGNMENT", displayLineColumn.Alignment, SqlDbType.TinyInt);
            statement.AddField("RELATIVESIZE", displayLineColumn.RelativeSize, SqlDbType.TinyInt);
            statement.AddField("COLORSTYLE", displayLineColumn.StyleID, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SaveOrder(IConnectionManager entry, KitchenDisplayLineColumn displayLineColumn)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYLINECOLUMN");
            statement.StatementType = StatementType.Update;

            statement.AddCondition("ID", (string)displayLineColumn.ID);
            statement.AddCondition("LINENUMBERID", (Guid)displayLineColumn.LineNumberID, SqlDbType.UniqueIdentifier);
            statement.AddField("COLUMNNUMBER", displayLineColumn.No, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYLINECOLUMN", "LINENUMBERID", id, false);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYLINECOLUMN", "ID", id, BusinessObjects.Permission.ManageKitchenDisplayProfiles, false);
        }

        public virtual void DeleteByDisplayLine(IConnectionManager entry, RecordIdentifier lineId)
        {
            DeleteRecord(entry, "KITCHENDISPLAYLINECOLUMN", "LINENUMBERID", lineId, BusinessObjects.Permission.ManageKitchenDisplayProfiles, false);
        }
    }
}
