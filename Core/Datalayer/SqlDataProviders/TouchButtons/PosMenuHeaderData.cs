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

namespace LSOne.DataLayer.SqlDataProviders.TouchButtons
{
    /// <summary>
    /// Data provider class for pos menus
    /// </summary>
    public class PosMenuHeaderData : SqlServerDataProviderBase, IPosMenuHeaderData
    {
        private static string BaseSelectString
        {
            get { return @"select
                        H.MENUID,
                        H.GUID,
                        ISNULL(H.DESCRIPTION,'') as DESCRIPTION,
                        ISNULL(H.COLUMNS,0) as COLUMNS,
                        ISNULL(H.ROWS,0) as ROWS,
                        ISNULL(H.MENUCOLOR,0) as MENUCOLOR,
                        ISNULL(H.FONTNAME,'') as FONTNAME,
                        ISNULL(H.FONTSIZE,0) as FONTSIZE,
                        ISNULL(H.FONTBOLD,0) as FONTBOLD,
                        ISNULL(H.FORECOLOR,0) as FORECOLOR,
                        ISNULL(H.BACKCOLOR,0) as BACKCOLOR,
                        BORDERCOLOR,
                        BORDERWIDTH,
                        MARGIN,
                        H.TEXTPOSITION,
                        ISNULL(H.FONTITALIC,0) as FONTITALIC,
                        ISNULL(H.FONTCHARSET,0) as FONTCHARSET,
                        ISNULL(H.USENAVOPERATION,0) as USENAVOPERATION, 
                        ISNULL(H.APPLIESTO,0) as APPLIESTO, 
                        ISNULL(H.BACKCOLOR2,0) as BACKCOLOR2,
                        ISNULL(H.GRADIENTMODE,0) as GRADIENTMODE, 
                        ISNULL(H.SHAPE,0) as SHAPE, 
                        ISNULL(H.MENUTYPE,0) as MENUTYPE, 
                        ISNULL(H.STYLEID,'') as STYLEID,
                        ISNULL(H.DEFAULTOPERATION, 0) as DEFAULTOPERATION,
						ISNULL(S.NAME, '') AS STYLENAME,
                        ISNULL(H.DEVICETYPE,0) AS DEVICETYPE,
                        ISNULL(H.MAINMENU,0) AS MAINMENU,
                        H.IMPORTDATETIME
                        from POSMENUHEADER H 
						LEFT JOIN POSSTYLE S ON H.STYLEID=S.ID and H.DATAAREAID=S.DATAAREAID "; }
        }

        private static string ResolveSort(PosMenuHeaderSorting sort, bool backwards)
        {
            switch (sort)
            {
                case PosMenuHeaderSorting.MenuID:
                    return backwards ? "MENUID DESC" : "MENUID ASC";

                case PosMenuHeaderSorting.MenuDescription:
                    return backwards ? "DESCRIPTION DESC" : "DESCRIPTION ASC";
                case PosMenuHeaderSorting.StyleName:
                    return backwards ? "STYLENAME DESC" : "STYLENAME ASC";
                case PosMenuHeaderSorting.ImportDateTime:
                    return backwards ? "IMPORTDATETIME DESC" : "IMPORTDATETIME ASC";
            }

            return "";
        }

        private static void PopulatePosMenuHeader(IDataReader dr, PosMenuHeader posMenuHeader)
        {
            posMenuHeader.ID = (string)dr["MENUID"];
            posMenuHeader.ID.SerializationData = (string)dr["MENUID"];
            posMenuHeader.Guid = (Guid) dr["GUID"];
            posMenuHeader.Text = (string)dr["DESCRIPTION"];
            posMenuHeader.Columns = (int)dr["COLUMNS"];
            posMenuHeader.Rows = (int)dr["ROWS"];
            posMenuHeader.MenuColor = (int)dr["MENUCOLOR"];
            posMenuHeader.FontName = (string)dr["FONTNAME"];
            posMenuHeader.FontSize = (int)dr["FONTSIZE"];
            posMenuHeader.FontBold = ((byte)dr["FONTBOLD"] != 0);
            posMenuHeader.ForeColor = (int)dr["FORECOLOR"];
            posMenuHeader.BackColor = (int)dr["BACKCOLOR"];
            posMenuHeader.BorderColor = (int)dr["BORDERCOLOR"];
            posMenuHeader.BorderWidth = (int)dr["BORDERWIDTH"];
            posMenuHeader.Margin = (int)dr["MARGIN"];
            posMenuHeader.TextPosition = (Position)(int)dr["TEXTPOSITION"];
            posMenuHeader.FontItalic = (byte)dr["FONTITALIC"] != 0;
            posMenuHeader.FontCharset = (int)dr["FONTCHARSET"];
            posMenuHeader.UseNavOperation = (byte)dr["USENAVOPERATION"] != 0;
            posMenuHeader.AppliesTo = (PosMenuHeader.AppliesToEnum)((int)dr["APPLIESTO"]);
            posMenuHeader.BackColor2 = (int)dr["BACKCOLOR2"];
            posMenuHeader.GradientMode = (GradientModeEnum)((int)dr["GRADIENTMODE"]);
            posMenuHeader.Shape = (ShapeEnum)((int)dr["SHAPE"]);
            posMenuHeader.MenuType = (MenuTypeEnum)((int)dr["MENUTYPE"]);
            posMenuHeader.StyleID = (string)dr["STYLEID"];
            posMenuHeader.DefaultOperation = (int) dr["DEFAULTOPERATION"];
            posMenuHeader.ImportDateTime = dr["IMPORTDATETIME"] == DBNull.Value ? posMenuHeader.ImportDateTime : (DateTime)dr["IMPORTDATETIME"];
            posMenuHeader.DeviceType = (DeviceTypeEnum)((int)dr["DEVICETYPE"]);
            posMenuHeader.MainMenu = AsBool(dr["MAINMENU"]);
        }

        /// <summary>
        /// Returns a list of menu headers that are using a specific style ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleID">The style ID to check</param>
        /// <returns>A list of menu header using the style ID</returns>
        public virtual List<PosMenuHeader> AreUsingStyle(IConnectionManager entry, RecordIdentifier styleID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    @"WHERE STYLEID = @STYLEID";

                MakeParam(cmd, "STYLEID", (string)styleID);

                return Execute<PosMenuHeader>(entry, cmd, CommandType.Text, PopulatePosMenuHeader);
            }
        }

        /// <summary>
        /// Gets a list of all POS Menu headers
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of PosMenuHeader objects containing all pos menu header records</returns>
        public virtual List<PosMenuHeader> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where H.DATAAREAID = @dataAreaId order by H.DESCRIPTION";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosMenuHeader>(entry, cmd, CommandType.Text, PopulatePosMenuHeader);
            }
        }

        /// <summary>
        /// Gets a list of all POS Menu headers
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="menuType">The type of menu to get</param>
        /// <returns>A list of PosMenuHeader objects containing all pos menu header records</returns>
        public virtual List<PosMenuHeader> GetList(IConnectionManager entry, MenuTypeEnum menuType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where H.DATAAREAID = @dataAreaId and H.MENUTYPE = @menuType";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "menuType", (int)menuType, SqlDbType.Int);

                return Execute<PosMenuHeader>(entry, cmd, CommandType.Text, PopulatePosMenuHeader);
            }
        }

        /// <summary>
        /// Gets a list of all POS Menu headers ordered by the specific field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="menuType">The type of menu to get</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of PosMenuHeader objects conataining all pos menu header records, ordered by the chosen field</returns>
        public virtual List<PosMenuHeader> GetList(IConnectionManager entry, PosMenuHeaderFilter filter)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSelectString + "where H.DATAAREAID = @dataAreaId ";

                if (filter != null)
                {
                    if(filter.MenuType.HasValue)
                    {
                        cmd.CommandText += "and H.MENUTYPE = @menuType ";
                        MakeParam(cmd, "menuType", filter.MenuType.Value, SqlDbType.Int);
                    }
                    if (!string.IsNullOrEmpty(filter.Description))
                    {
                        cmd.CommandText += "and H.DESCRIPTION LIKE @description ";
                        MakeParam(cmd, "description", PreProcessSearchText(filter.Description, true, filter.DescriptionBeginsWith));
                    }

                    if (filter.MainMenu.HasValue)
                    {
                        cmd.CommandText += "and H.MAINMENU = @mainMenu ";
                        MakeParam(cmd, "mainMenu", filter.MainMenu, SqlDbType.Bit);
                    }

                    if (filter.DeviceType.HasValue)
                    {
                        cmd.CommandText += "and H.DEVICETYPE = @deviceType ";
                        MakeParam(cmd, "deviceType", filter.DeviceType.Value, SqlDbType.Int);
                    }

                    if(!RecordIdentifier.IsEmptyOrNull(filter.StyleID))
                    {
                        cmd.CommandText += "and H.STYLEID = @style ";
                        MakeParam(cmd, "style", (string)filter.StyleID);
                    }

                    cmd.CommandText += "order by " + ResolveSort(filter.SortBy, filter.SortBackwards);
                }
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosMenuHeader>(entry, cmd, CommandType.Text, PopulatePosMenuHeader);
            }
        }

        /// <summary>
        /// Gets as pos menu header with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeaderID">The ID of the pos menu header to get</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>A PosMenuHeader object containing the pos menu header with the given ID</returns>
        public virtual PosMenuHeader Get(IConnectionManager entry, RecordIdentifier posMenuHeaderID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where H.DATAAREAID = @dataAreaId and H.MENUID = @menuId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "menuId", (string)posMenuHeaderID);

                return Get<PosMenuHeader>(entry, cmd, posMenuHeaderID, PopulatePosMenuHeader, cacheType, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Gets as pos menu header with the given Guid
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="guid">The Guid of the pos menu header to get</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>A PosMenuHeader object containing the pos menu header with the given Guid</returns>
        public virtual PosMenuHeader GetByGuid(IConnectionManager entry, Guid guid, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where H.DATAAREAID = @dataAreaId and H.GUID = @guid";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "guid", guid);

                return Get<PosMenuHeader>(entry, cmd, new RecordIdentifier(), PopulatePosMenuHeader, cacheType, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Checks if a pos menu header with the given GUID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="guid">The guid of the restaurant station to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool GuidExists(IConnectionManager entry, Guid guid)
        {
            return RecordExists(entry, "POSMENUHEADER", "GUID", guid);
        }

        /// <summary>
        /// Checks if a pos menu header with the given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id of the restaurant station to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "POSMENUHEADER", "MENUID", id);
        }

        /// <summary>
        /// Deletes the pos menu header with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the pos menu header to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<PosMenuHeader>(entry, "POSMENUHEADER", "MENUID", id, BusinessObjects.Permission.EditPosMenus);
        }

        /// <summary>
        /// Saves a pos menu header object into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeader"></param>
        public virtual void Save(IConnectionManager entry, PosMenuHeader posMenuHeader)
        {
            var statement = new SqlServerStatement("POSMENUHEADER");

            ValidateSecurity(entry, BusinessObjects.Permission.EditPosMenus);

            bool isNew = false;
            if (posMenuHeader.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                posMenuHeader.ID = DataProviderFactory.Instance.GenerateNumber<IPosMenuHeaderData, PosMenuHeader>(entry);
            }

            if (posMenuHeader.Guid == Guid.Empty)
            {
                posMenuHeader.Guid = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, posMenuHeader.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("MENUID", (string) posMenuHeader.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("MENUID", (string) posMenuHeader.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DESCRIPTION", posMenuHeader.Text);
            statement.AddField("COLUMNS", posMenuHeader.Columns, SqlDbType.Int);
            statement.AddField("ROWS", posMenuHeader.Rows, SqlDbType.Int);
            statement.AddField("MENUCOLOR", posMenuHeader.MenuColor, SqlDbType.Int);
            statement.AddField("FONTNAME", posMenuHeader.FontName);
            statement.AddField("FONTSIZE", posMenuHeader.FontSize, SqlDbType.Int);
            statement.AddField("FONTBOLD", posMenuHeader.FontBold ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FORECOLOR", posMenuHeader.ForeColor, SqlDbType.Int);
            statement.AddField("BACKCOLOR", posMenuHeader.BackColor, SqlDbType.Int);
            statement.AddField("BORDERCOLOR", posMenuHeader.BorderColor, SqlDbType.Int);
            statement.AddField("BORDERWIDTH", posMenuHeader.BorderWidth, SqlDbType.Int);
            statement.AddField("MARGIN", posMenuHeader.Margin, SqlDbType.Int);
            statement.AddField("TEXTPOSITION", posMenuHeader.TextPosition, SqlDbType.Int);
            statement.AddField("FONTITALIC", posMenuHeader.FontItalic ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FONTCHARSET", posMenuHeader.FontCharset, SqlDbType.Int);
            statement.AddField("USENAVOPERATION", posMenuHeader.UseNavOperation ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("APPLIESTO", (int) posMenuHeader.AppliesTo, SqlDbType.Int);
            statement.AddField("BACKCOLOR2", posMenuHeader.BackColor2, SqlDbType.Int);
            statement.AddField("GRADIENTMODE", (int) posMenuHeader.GradientMode, SqlDbType.Int);
            statement.AddField("SHAPE", (int) posMenuHeader.Shape, SqlDbType.Int);
            statement.AddField("MENUTYPE", (int) posMenuHeader.MenuType, SqlDbType.Int);
            statement.AddField("STYLEID", (string) posMenuHeader.StyleID);
            statement.AddField("DEFAULTOPERATION", posMenuHeader.DefaultOperation.StringValue == "" ? 0 : Convert.ToInt32(posMenuHeader.DefaultOperation.StringValue), SqlDbType.Int);
            statement.AddField("GUID", posMenuHeader.Guid, SqlDbType.UniqueIdentifier);
            statement.AddField("DEVICETYPE", (int)posMenuHeader.DeviceType, SqlDbType.Int);
            statement.AddField("MAINMENU", posMenuHeader.MainMenu, SqlDbType.Bit);

            if (posMenuHeader.ImportDateTime != null)
            {
                statement.AddField("IMPORTDATETIME", posMenuHeader.ImportDateTime, SqlDbType.DateTime);
            }

            Save(entry, posMenuHeader, statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "PosMenuID"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSMENUHEADER", "MENUID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
