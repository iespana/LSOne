using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using System.Security;
using System.Data.SqlClient;
using System.Linq;

namespace LSOne.DataLayer.SqlDataProviders.TouchButtons
{
    /// <summary>
    /// A data provider for the <see cref="PosStyle"/> business object
    /// </summary>
    public class PosStyleData : SqlServerDataProviderBase, IPosStyleData
    {
        private static string BaseSelectString
        {
            get
            {
                return
                       @"SELECT ID, GUID, 
                       ISNULL (NAME,'') as NAME, 
                       ISNULL (SYSTEMSTYLE,0) as SYSTEMSTYLE, 
                       ISNULL (FONTNAME,'') as FONTNAME, 
                       ISNULL (FONTSIZE, 0) AS FONTSIZE, 
                       ISNULL (FONTBOLD, 0) AS FONTBOLD, 
                       ISNULL (FONTSTRIKETHROUGH, 0) AS FONTSTRIKETHROUGH, 
                       ISNULL (FORECOLOR, 0) AS FORECOLOR, 
                       ISNULL (BACKCOLOR, 0) AS BACKCOLOR, 
                       ISNULL (FONTITALIC, 0) AS FONTITALIC, 
                       ISNULL (FONTCHARSET, 0) AS FONTCHARSET, 
                       ISNULL (BACKCOLOR2, 0) AS BACKCOLOR2, 
                       ISNULL (GRADIENTMODE, 0) AS GRADIENTMODE, 
                       ISNULL (SHAPE, 0) AS SHAPE, 
                       ISNULL (STYLETYPE, 0) AS STYLETYPE,
                       TEXTPOSITION,
                       IMPORTDATETIME 
                       FROM POSSTYLE ";
            }
        }

        private static void PopulateProfile(IDataReader dr, PosStyle profile)
        {
            profile.ID = (string)dr["ID"];
            profile.ID.SerializationData = (string)dr["ID"];
            profile.Guid = (Guid)dr["GUID"];
            profile.Text = (string)dr["NAME"];
            profile.IsSystemStyle = AsBool(dr["SYSTEMSTYLE"]);
            profile.FontName = (string)dr["FONTNAME"] == "" ? "Tahoma" : (string)dr["FONTNAME"]; //We have seen examples where styles have no font name. We have to provide a name
            profile.FontSize = (int)dr["FONTSIZE"];
            profile.FontBold = ((byte)dr["FONTBOLD"] != 0);
            profile.FontStrikethrough = ((byte)dr["FONTSTRIKETHROUGH"] != 0);
            profile.ForeColor = (int)dr["FORECOLOR"];
            profile.BackColor = (int)dr["BACKCOLOR"];
            profile.FontItalic = (byte)dr["FONTITALIC"] != 0;
            profile.FontCharset = (int)dr["FONTCHARSET"];
            profile.BackColor2 = (int)dr["BACKCOLOR2"];
            profile.GradientMode = (GradientModeEnum)((int)dr["GRADIENTMODE"]);
            profile.Shape = (ShapeEnum)((int)dr["SHAPE"]);
            profile.StyleType = (StyleType)((int)dr["STYLETYPE"]);
            profile.TextPosition = (Position)((int)dr["TEXTPOSITION"]);
            profile.ImportDateTime = dr["IMPORTDATETIME"] == DBNull.Value ? profile.ImportDateTime : (DateTime)dr["IMPORTDATETIME"];
        }

        /// <summary>
        /// Gets a styleProfile with a particular id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">id of styleprofile to get</param>
        /// <param name="cache">cachetype</param>
        /// <returns>A StyleProfile</returns>
        public virtual PosStyle Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            if (((string)id).Trim() == "")
            {
                return new PosStyle(RecordIdentifier.Empty, "");
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                       BaseSelectString +
                       "where DATAAREAID = @dataAreaId and ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                return Get<PosStyle>(entry, cmd, id, PopulateProfile, cache, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Gets a styleProfile with a particular name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="name">The style name</param>
        /// <returns>A StyleProfile</returns>
        public virtual PosStyle GetByName(IConnectionManager entry, string name)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                       BaseSelectString +
                       "where DATAAREAID = @dataAreaId and NAME = @name";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "name", name);

                return Get<PosStyle>(entry, cmd, new RecordIdentifier(), PopulateProfile,
                    CacheType.CacheTypeNone, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Runs an unsecured stored procedure to retrieves a POS style from the database
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="name"></param>
        /// <param name="dataSource"></param>
        /// <param name="windowsAuthentication"></param>
        /// <param name="sqlServerLogin"></param>
        /// <param name="sqlServerPassword"></param>
        /// <param name="databaseName"></param>
        /// <param name="connectionType"></param>
        /// <param name="dataAreaID"></param>
        /// <returns></returns>
        public virtual PosStyle GetByNameUnsecure(IConnectionManager entry, string name, string dataSource, bool windowsAuthentication,
            string sqlServerLogin, SecureString sqlServerPassword, string databaseName, ConnectionType connectionType, string dataAreaID)
        {
            using (IDbCommand cmd = new SqlCommand("spGetPOSStyleByName_1_0")) //No connection exists on this point to create the command.
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MakeParam(cmd, "NAME", name);

                try
                {
                    return entry.UnsecureExecuteReader<PosStyle>(dataSource, windowsAuthentication, sqlServerLogin, sqlServerPassword, databaseName, connectionType, dataAreaID, cmd, PopulateProfile).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a styleProfile with a particular name
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="guid">The guid of the style</param>
        /// <returns>A StyleProfile</returns>
        public virtual PosStyle GetByGuid(IConnectionManager entry, Guid guid)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                       BaseSelectString +
                       "where DATAAREAID = @dataAreaId and GUID = @guid";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "guid", guid);

                return Get<PosStyle>(entry, cmd, new RecordIdentifier(), PopulateProfile,
                    CacheType.CacheTypeNone, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Returns true if the GUID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="guid">The GUID to check</param>
        /// <returns>True if the GUID exists otherwise false is returned</returns>
        public virtual bool GuidExists(IConnectionManager entry, Guid guid)
        {
            return RecordExists<PosStyle>(entry, "POSSTYLE", "GUID", guid);
        }

        /// <summary>
        /// Returns true if the ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The ID to check</param>
        /// <returns>True if the ID exists otherwise false is returned</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<PosStyle>(entry, "POSSTYLE", "ID", id);
        }

        /// <summary>
        /// Deletes a specific style ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The ID to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<PosStyle>(entry, "POSSTYLE", "ID", id, BusinessObjects.Permission.ManageUIStyleSetup);
        }

        /// <summary>
        /// Gets a list of all style profiles
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">How the list should by sorted. If empty then no specific sorting is applied</param>
        /// <param name="cache">The type of cache that should be used for this list</param>
        /// <returns>A list of all style profiles</returns>
        public virtual List<PosStyle> GetList(IConnectionManager entry, string sort = "", CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                       BaseSelectString +
                       "where DATAAREAID = @dataAreaId";

                if (sort != "")
                {
                    cmd.CommandText += " ORDER BY " + sort;
                }

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosStyle>(entry, cmd, CommandType.Text, PopulateProfile);
            }
        }

        /// <summary>
        /// Gets a list of all style profiles filtered by style type, shape type and gradient
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        ///  <param name="selectedType">The type of the style</param>
        ///   <param name="description">The type of the style</param>
        /// <param name="beginsWith">The type of the style</param>
        /// <param name="selectedShape">The shape of the style</param>
        /// <param name="selectedGradient">The gradient of the style</param>
        /// <param name="sort">How the list should by sorted. If empty then no specific sorting is applied</param>
        /// <param name="cache">The type of cache that should be used for this list</param>
        /// <returns>A list of all style profiles</returns>
        public virtual List<PosStyle> GetListByFilters(IConnectionManager entry, string description, bool beginsWith, StyleType? selectedType, ShapeEnum? selectedShape, GradientModeEnum? selectedGradient, string sort = "", CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>();

                if (selectedType.HasValue)
                {
                    bool isSystemStyle = selectedType.Value != StyleType.Normal;
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "SYSTEMSTYLE = @SYSTEMSTYLE" });
                    MakeParam(cmd, "SYSTEMSTYLE", isSystemStyle, SqlDbType.Bit);
                }

                if (!string.IsNullOrEmpty(description))
                {
                    var searchToken = PreProcessSearchText(description, true, beginsWith);

                    if (!string.IsNullOrEmpty(searchToken))
                    {
                        conditions.Add(new Condition
                        {
                            ConditionValue = "NAME like @DESCRIPTION ",
                            Operator = "AND"

                        });

                        MakeParam(cmd, "DESCRIPTION", searchToken);
                    }
                }

                if (selectedShape.HasValue)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "SHAPE = @SHAPE" });
                    MakeParam(cmd, "SHAPE", (int)selectedShape.Value);
                }

                if (selectedGradient.HasValue)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "GRADIENTMODE = @GRADIENTMODE" });
                    MakeParam(cmd, "GRADIENTMODE", (int)selectedGradient.Value);
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "DATAAREAID = @dataAreaId" });
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                cmd.CommandText += BaseSelectString + QueryPartGenerator.ConditionGenerator(conditions);

                if (sort != "")
                {
                    cmd.CommandText += " ORDER BY " + sort;
                }

                return Execute<PosStyle>(entry, cmd, CommandType.Text, PopulateProfile);
            }
        }

        /// <summary>
        /// Saves an instance of <see cref="PosStyle"/> to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posStyle">The <see cref=" PosStyle"/> instance to be saved</param>
        public virtual void Save(IConnectionManager entry, PosStyle posStyle)
        {
            var statement = new SqlServerStatement("POSSTYLE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageUIStyleSetup);

            bool isNew = false;
            if (posStyle.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                posStyle.ID = DataProviderFactory.Instance.GenerateNumber<IPosStyleData, PosStyle>(entry);
            }

            if (posStyle.Guid == Guid.Empty)
            {
                posStyle.Guid = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, posStyle.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (string)posStyle.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", (string)posStyle.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("SYSTEMSTYLE", posStyle.IsSystemStyle ? 1 : 0, SqlDbType.Bit);
            statement.AddField("NAME", posStyle.Text);
            statement.AddField("FONTNAME", posStyle.FontName);
            statement.AddField("FONTSIZE", posStyle.FontSize, SqlDbType.Int);
            statement.AddField("FONTBOLD", posStyle.FontBold ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FONTSTRIKETHROUGH", posStyle.FontStrikethrough ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FORECOLOR", posStyle.ForeColor, SqlDbType.Int);
            statement.AddField("BACKCOLOR", posStyle.BackColor, SqlDbType.Int);
            statement.AddField("FONTITALIC", posStyle.FontItalic ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FONTCHARSET", posStyle.FontCharset, SqlDbType.Int);
            statement.AddField("BACKCOLOR2", posStyle.BackColor2, SqlDbType.Int);
            statement.AddField("GRADIENTMODE", (int)posStyle.GradientMode, SqlDbType.Int);
            statement.AddField("SHAPE", (int)posStyle.Shape, SqlDbType.Int);
            statement.AddField("TEXTPOSITION", (int)posStyle.TextPosition, SqlDbType.Int);
            statement.AddField("GUID", posStyle.Guid, SqlDbType.UniqueIdentifier);

            if (posStyle.ImportDateTime != null)
            {
                statement.AddField("IMPORTDATETIME", posStyle.ImportDateTime, SqlDbType.DateTime);
            }

            Save(entry, posStyle, statement);
        }

        #region ISequenceable Members
        /// <summary>
        /// Returns true if the sequence exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The sequence ID to check</param>
        /// <returns>True if the sequence exists otherwise false is returned</returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// The SequenceID name used for <see cref="PosStyle"/>
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "BTNSTYLES"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSSTYLE", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }
        #endregion
    }
}
