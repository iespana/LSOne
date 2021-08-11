using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.GUI;

namespace LSOne.DataLayer.SqlDataProviders.TouchButtons
{
    public class PosMenuLineData : SqlServerDataProviderBase, IPosMenuLineData
    {
        private static string BaseSelectString
        {
            get
            {
                return @"select 
                    p.MENUID, 
                    p.SEQUENCE,
                    ISNULL(p.KEYNO,0) as KEYNO, 
                    ISNULL(p.DESCRIPTION,'') as DESCRIPTION, 
                    ISNULL(p.OPERATION,0) as OPERATION, 
                    ISNULL(p.PARAMETER,'') as PARAMETER, 
                    ISNULL(p.PARAMETERTYPE,0) as PARAMETERTYPE, 
                    ISNULL(p.FONTNAME,'') as FONTNAME, 
                    ISNULL(p.FONTSIZE,0) as FONTSIZE, 
                    ISNULL(p.FONTBOLD,0) as FONTBOLD, 
                    ISNULL(p.FORECOLOR,0) as FORECOLOR, 
                    ISNULL(p.BACKCOLOR,0) as BACKCOLOR, 
                    ISNULL(p.FONTITALIC,0) as FONTITALIC, 
                    ISNULL(p.FONTCHARSET,0) as FONTCHARSET, 
                    ISNULL(p.DISABLED,0) as DISABLED, 
                    ISNULL(p.PICTUREID, '') as PICTUREID,
                    ISNULL(p.HIDEDESCRONPICTURE,0) as HIDEDESCRONPICTURE, 
                    ISNULL(p.FONTSTRIKETHROUGH,0) as FONTSTRIKETHROUGH, 
                    ISNULL(p.FONTUNDERLINE,0) as FONTUNDERLINE, 
                    ISNULL(p.COLUMNSPAN,0) as COLUMNSPAN, 
                    ISNULL(p.ROWSPAN,0) as ROWSPAN, 
                    ISNULL(p.NAVOPERATION,'') as NAVOPERATION, 
                    ISNULL(p.HIDDEN,0) as HIDDEN, 
                    ISNULL(p.SHADEWHENDISABLED,0) as SHADEWHENDISABLED, 
                    ISNULL(p.BACKGROUNDHIDDEN,0) as BACKGROUNDHIDDEN, 
                    ISNULL(p.TRANSPARENT,0) as TRANSPARENT, 
                    ISNULL(p.GLYPH,0) as GLYPH, 
                    ISNULL(p.GLYPH2,0) as GLYPH2, 
                    ISNULL(p.GLYPH3,0) as GLYPH3, 
                    ISNULL(p.GLYPH4,0) as GLYPH4, 
                    ISNULL(p.GLYPHTEXT,'') as GLYPHTEXT, 
                    ISNULL(p.GLYPHTEXT2,'') as GLYPHTEXT2, 
                    ISNULL(p.GLYPHTEXT3,'') as GLYPHTEXT3, 
                    ISNULL(p.GLYPHTEXT4,'') as GLYPHTEXT4, 
                    ISNULL(p.GLYPHTEXTFONT,'') as GLYPHTEXTFONT, 
                    ISNULL(p.GLYPHTEXT2FONT,'') as GLYPHTEXT2FONT, 
                    ISNULL(p.GLYPHTEXT3FONT,'') as GLYPHTEXT3FONT,  
                    ISNULL(p.GLYPHTEXT4FONT,'') as GLYPHTEXT4FONT,  
                    ISNULL(p.GLYPHTEXTFONTSIZE,0) as GLYPHTEXTFONTSIZE, 
                    ISNULL(p.GLYPHTEXT2FONTSIZE,0) as GLYPHTEXT2FONTSIZE,   
                    ISNULL(p.GLYPHTEXT3FONTSIZE,0) as GLYPHTEXT3FONTSIZE, 
                    ISNULL(p.GLYPHTEXT4FONTSIZE,0) as GLYPHTEXT4FONTSIZE, 
                    ISNULL(p.GLYPHTEXTFORECOLOR,0) as GLYPHTEXTFORECOLOR, 
                    ISNULL(p.GLYPHTEXT2FORECOLOR,0) as GLYPHTEXT2FORECOLOR, 
                    ISNULL(p.GLYPHTEXT3FORECOLOR,0) as GLYPHTEXT3FORECOLOR, 
                    ISNULL(p.GLYPHTEXT4FORECOLOR,0) as GLYPHTEXT4FORECOLOR, 
                    ISNULL(p.GLYPHOFFSET,0) as GLYPHOFFSET, 
                    ISNULL(p.GLYPH2OFFSET,0) as GLYPH2OFFSET, 
                    ISNULL(p.GLYPH3OFFSET,0) as GLYPH3OFFSET, 
                    ISNULL(p.GLYPH4OFFSET,0) as GLYPH4OFFSET, 
                    ISNULL(p.BACKCOLOR2,0) as BACKCOLOR2, 
                    ISNULL(p.GRADIENTMODE,0) as GRADIENTMODE, 
                    ISNULL(p.SHAPE,0) as SHAPE, 
                    ISNULL(p.USEHEADERFONT,0) as USEHEADERFONT, 
                    ISNULL(p.USEHEADERATTRIBUTES,0) as USEHEADERATTRIBUTES, 
                    ISNULL(p.IMAGEPOSITION,0) as IMAGEPOSITION, 
                    p.TEXTPOSITION,
                    ISNULL(posop.DESCRIPTION,'') as POSOPERATIONNAME,
                    ISNULL(hospop.OPERATIONNAME,'') as HOSPITALITYOPERATIONNAME,
                    ISNULL(p.STYLEID, '') as STYLEID,
                    ISNULL(p.KEYMAPPING,'0') as KEYMAPPING,
                    p.USEIMAGEFONT,
                    ISNULL(p.IMAGEFONTTEXT, '') AS IMAGEFONTTEXT,
                    ISNULL(p.IMAGEFONTNAME, '') AS IMAGEFONTNAME,
                    ISNULL(p.IMAGEFONTSIZE, 0) AS IMAGEFONTSIZE,
                    ISNULL(p.IMAGEFONTBOLD, 0) AS IMAGEFONTBOLD,
                    ISNULL(p.IMAGEFONTITALIC, 0) AS IMAGEFONTITALIC,
                    ISNULL(p.IMAGEFONTUNDERLINE, 0) AS IMAGEFONTUNDERLINE,
                    ISNULL(p.IMAGEFONTSTRIKETHROUGH, 0) AS IMAGEFONTSTRIKETHROUGH,
                    ISNULL(p.IMAGEFONTCHARSET, 0) AS IMAGEFONTCHARSET,
                    ISNULL(p.IMAGEFONTCOLOR, 0) AS IMAGEFONTCOLOR,
                    ISNULL(p.PARAMETERITEMID,'') AS PARAMETERITEMID
                    from POSMENULINE p
                    left outer join OPERATIONS posop on p.OPERATION = posop.ID 
                    left outer join POSISHOSPITALITYOPERATIONS hospop on p.DATAAREAID = hospop.DATAAREAID and p.OPERATION = hospop.OPERATIONID ";
            }
        }

        private static string ListItemSelectString
        {
            get
            {
                return @"select p.KEYNO,
	                            p.MENUID,
	                            p.SEQUENCE,
                                p.OPERATION,
	                            ISNULL(p.DESCRIPTION, '') as DESCRIPTION,
	                            ISNULL(posop.DESCRIPTION, '') as OPERATIONNAME,
                                ISNULL(posop.LOOKUPTYPE, 0) as OPERATIONLOOKUPTYPE,
	                            ISNULL(p.PARAMETER, '') as PARAMETER,
                                ISNULL(p.KEYMAPPING, 0) as KEYMAPPING,  
                                ISNULL(p.STYLEID, '') as STYLEID,
	                            -- RetailItems
	                            ISNULL(it.ITEMNAME, '') as ITEMNAME,
	                            -- StorePaymentTypes
	                            ISNULL(rbotend.NAME, '') as PAYMENTTYPENAME,
	                            --PosMenu
	                            ISNULL(pheader.DESCRIPTION, '') as POSMENUNAME,
	                            --TaxGroupInfocode
	                            ISNULL(rboinfo.DESCRIPTION, '') as INFOCODENAME,
	                            --PosMenuAndButtonGrid
	                            ISNULL(pheader2.DESCRIPTION, '') as POSMENUNAME2,
	                            --SuspendTransactionTypes
	                            ISNULL(possusp.DESCRIPTION, '') as SUSPENSIONTYPENAME,
	                            --BlankOperations
	                            ISNULL(posblankop.OPERATIONDESCRIPTION, '') as BLANKOPERATIONNAME,
	                            ISNULL(posblankop.OPERATIONPARAMETER, '') as BLANKOPERATIONPARAMETER,
	                            --IncomeAccount / ExpenseAccount
	                            ISNULL(rboinexp.NAME, '') as INCOMEEXPENSEACCOUNTNAME,
	                            --StorePaymentTypeAndAmount
	                            ISNULL(rbotend2.NAME, '') as PAYMENTTYPENAME2,
                                ISNULL(hospop.OPERATIONNAME,'') as HOSPITALITYOPERATIONNAME,
                                --TriggerPeriodicDiscount
                                ISNULL(pd.DESCRIPTION, '') as PERIODICDISCOUNTNAME,
                                --ParameterItemID
                                ISNULL(p.PARAMETERITEMID, '') as PARAMETERITEMID
                        from POSMENULINE p
                        left outer join OPERATIONS posop on p.OPERATION = posop.ID
                        left outer join POSISHOSPITALITYOPERATIONS hospop on p.DATAAREAID = hospop.DATAAREAID and p.OPERATION = hospop.OPERATIONID

                        --RetailItems
                        left outer join INVENTITEMBARCODE itbarcode on itbarcode.DATAAREAID = p.DATAAREAID and p.PARAMETER = itbarcode.ITEMBARCODE
                        left outer join retailitem it on it.ITEMID = p.PARAMETER or it.ITEMID = itbarcode.ITEMID

                        --StorePaymentTypes
                        left outer join RBOTENDERTYPETABLE rbotend on rbotend.DATAAREAID = p.DATAAREAID and rbotend.TENDERTYPEID = p.PARAMETER

                        --PosMenu
                        left outer join POSMENUHEADER pheader on pheader.DATAAREAID = p.DATAAREAID and pheader.MENUID = p.PARAMETER

                        --TaxGroupInfocode
                        left outer join RBOINFOCODETABLE rboinfo on rboinfo.DATAAREAID = p.DATAAREAID and rboinfo.INFOCODEID = p.PARAMETER

                        --PosMenuAndButtonGrid
                        left outer join POSMENUHEADER pheader2 on pheader2.DATAAREAID = p.DATAAREAID and pheader2.MENUID = IIF(CHARINDEX(';', p.PARAMETER) = 0, p.PARAMETER, SUBSTRING(p.PARAMETER, 0, CHARINDEX(';', p.PARAMETER)))

                        --SuspendTransactionTypes
                        left outer join POSISSUSPENSIONTYPE possusp on possusp.DATAAREAID = p.DATAAREAID and possusp.ID = p.PARAMETER

                        --BlankOperations
                        left outer join POSISBLANKOPERATIONS posblankop on posblankop.DATAAREAID = p.DATAAREAID and posblankop.ID = SUBSTRING(p.PARAMETER, 0, CHARINDEX(';', p.PARAMETER))

                        --IncomeAccount / ExpenseAccounts
                        left outer join RBOINCOMEEXPENSEACCOUNTTABLE rboinexp on rboinexp.DATAAREAID = p.DATAAREAID and rboinexp.ACCOUNTNUM = p.PARAMETER

                        -- StorePaymentTypeAndAmount
                        left outer join RBOTENDERTYPETABLE rbotend2 on rbotend2.DATAAREAID = p.DATAAREAID and rbotend2.TENDERTYPEID = SUBSTRING(p.PARAMETER, 0, CHARINDEX(';', p.PARAMETER))

                        --TriggerPeriodicDiscount
                        left outer join PERIODICDISCOUNT pd on  pd.OFFERID = p.PARAMETER ";
            }
        }

        private static void PopulatePosMenuLine(IDataReader dr, PosMenuLine posMenuLine)
        {
            posMenuLine.MenuID = (string)dr["MENUID"];
            posMenuLine.MenuID.SerializationData = (string)dr["MENUID"];
            posMenuLine.Sequence = (string)dr["SEQUENCE"];
            posMenuLine.Sequence.SerializationData = (string)dr["SEQUENCE"];
            posMenuLine.KeyNo = (int)dr["KEYNO"];
            posMenuLine.Text = (string)dr["DESCRIPTION"];
            posMenuLine.Operation = (int)dr["OPERATION"];
            posMenuLine.Operation.SerializationData = (int)dr["OPERATION"];
            posMenuLine.Parameter = (string)dr["PARAMETER"];
            posMenuLine.ParameterType = (PosMenuLine.ParameterTypeEnum)((int)dr["PARAMETERTYPE"]);
            posMenuLine.FontName = (string)dr["FONTNAME"];
            posMenuLine.FontSize = (int)dr["FONTSIZE"];
            posMenuLine.FontBold = (byte)dr["FONTBOLD"] != 0;
            posMenuLine.ForeColor = (int)dr["FORECOLOR"];
            posMenuLine.BackColor = (int)dr["BACKCOLOR"];
            posMenuLine.FontItalic = (byte)dr["FONTITALIC"] != 0;
            posMenuLine.FontCharset = (int)dr["FONTCHARSET"];
            posMenuLine.Disabled = (byte)dr["DISABLED"] != 0;
            posMenuLine.PictureID = (string)dr["PICTUREID"];
            posMenuLine.HideDescrOnPicture = (byte)dr["HIDEDESCRONPICTURE"] != 0;
            posMenuLine.FontStrikethrough = (byte)dr["FONTSTRIKETHROUGH"] != 0;
            posMenuLine.FontUnderline = (byte)dr["FONTUNDERLINE"] != 0;
            posMenuLine.ColumnSpan = (int)dr["COLUMNSPAN"];
            posMenuLine.RowSpan = (int)dr["ROWSPAN"];
            posMenuLine.NavOperation = (string)dr["NAVOPERATION"];
            posMenuLine.Hidden = (byte)dr["HIDDEN"] != 0;
            posMenuLine.ShadeWhenDisabled = (byte)dr["SHADEWHENDISABLED"] != 0;
            posMenuLine.BackgroundHidden = (byte)dr["BACKGROUNDHIDDEN"] != 0;
            posMenuLine.Transparent = (byte)dr["TRANSPARENT"] != 0;
            posMenuLine.Glyph = (PosMenuLine.GlyphEnum)((int)dr["GLYPH"]);
            posMenuLine.Glyph2 = (PosMenuLine.GlyphEnum)((int)dr["GLYPH2"]);
            posMenuLine.Glyph3 = (PosMenuLine.GlyphEnum)((int)dr["GLYPH3"]);
            posMenuLine.Glyph4 = (PosMenuLine.GlyphEnum)((int)dr["GLYPH4"]);
            posMenuLine.GlyphText = (string)dr["GLYPHTEXT"];
            posMenuLine.GlyphText2 = (string)dr["GLYPHTEXT2"];
            posMenuLine.GlyphText3 = (string)dr["GLYPHTEXT3"];
            posMenuLine.GlyphText4 = (string)dr["GLYPHTEXT4"];
            posMenuLine.GlyphTextFont = (string)dr["GLYPHTEXTFONT"];
            posMenuLine.GlyphText2Font = (string)dr["GLYPHTEXT2FONT"];
            posMenuLine.GlyphText3Font = (string)dr["GLYPHTEXT3FONT"];
            posMenuLine.GlyphText4Font = (string)dr["GLYPHTEXT4FONT"];
            posMenuLine.GlyphTextFontSize = (int)dr["GLYPHTEXTFONTSIZE"];
            posMenuLine.GlyphText2FontSize = (int)dr["GLYPHTEXT2FONTSIZE"];
            posMenuLine.GlyphText3FontSize = (int)dr["GLYPHTEXT3FONTSIZE"];
            posMenuLine.GlyphText4FontSize = (int)dr["GLYPHTEXT4FONTSIZE"];
            posMenuLine.GlyphTextForeColor = (int)dr["GLYPHTEXTFORECOLOR"];
            posMenuLine.GlyphText2ForeColor = (int)dr["GLYPHTEXT2FORECOLOR"];
            posMenuLine.GlyphText3ForeColor = (int)dr["GLYPHTEXT3FORECOLOR"];
            posMenuLine.GlyphText4ForeColor = (int)dr["GLYPHTEXT4FORECOLOR"];
            posMenuLine.GlyphOffSet = (int)dr["GLYPHOFFSET"];
            posMenuLine.Glyph2OffSet = (int)dr["GLYPH2OFFSET"];
            posMenuLine.Glyph3OffSet = (int)dr["GLYPH3OFFSET"];
            posMenuLine.Glyph4OffSet = (int)dr["GLYPH4OFFSET"];
            posMenuLine.BackColor2 = (int)dr["BACKCOLOR2"];
            posMenuLine.GradientMode = (GradientModeEnum)((int)dr["GRADIENTMODE"]);
            posMenuLine.Shape = (ShapeEnum)((int)dr["SHAPE"]);
            posMenuLine.UseHeaderFont = ((byte)dr["USEHEADERFONT"] != 0);
            posMenuLine.UseHeaderAttributes = ((byte)dr["USEHEADERATTRIBUTES"] != 0);
            posMenuLine.ImagePosition = (Position)((int)dr["IMAGEPOSITION"]);
            posMenuLine.TextPosition = (Position)((int)dr["TEXTPOSITION"]);
            posMenuLine.PosOperationName = (string)dr["POSOPERATIONNAME"];
            posMenuLine.HospitalityOperationName = (string)dr["HOSPITALITYOPERATIONNAME"];
            posMenuLine.StyleID = (string)dr["STYLEID"];
            posMenuLine.KeyMapping = (Keys)dr["KEYMAPPING"];
            posMenuLine.UseImageFont = (bool)dr["USEIMAGEFONT"];
            posMenuLine.ImageFontText = (string)dr["IMAGEFONTTEXT"];
            posMenuLine.ImageFontName = (string)dr["IMAGEFONTNAME"];
            posMenuLine.ImageFontSize = (int)dr["IMAGEFONTSIZE"];
            posMenuLine.ImageFontBold = (bool)dr["IMAGEFONTBOLD"];
            posMenuLine.ImageFontItalic = (bool)dr["IMAGEFONTITALIC"];
            posMenuLine.ImageFontUnderline = (bool)dr["IMAGEFONTUNDERLINE"];
            posMenuLine.ImageFontStrikethrough = (bool)dr["IMAGEFONTSTRIKETHROUGH"];
            posMenuLine.ImageFontCharset = (int)dr["IMAGEFONTCHARSET"];
            posMenuLine.ImageFontColor = (int)dr["IMAGEFONTCOLOR"];
            posMenuLine.ParameterItemID = (string)dr["PARAMETERITEMID"];
        }

        private static void PopulateListItem(IDataReader dr, PosMenuLineListItem posMenuLine)
        {
            posMenuLine.MenuID = (string)dr["MENUID"];
            posMenuLine.Sequence = (string)dr["SEQUENCE"];
            posMenuLine.Text = (string)dr["DESCRIPTION"];
            posMenuLine.KeyNo = (int)dr["KEYNO"];
            posMenuLine.Operation = (int)dr["OPERATION"];
            posMenuLine.OperationName = (string)dr["OPERATIONNAME"];
            posMenuLine.OperationLookupType = (LookupTypeEnum)((int)dr["OPERATIONLOOKUPTYPE"]);
            posMenuLine.Parameter = (string)dr["PARAMETER"];
            posMenuLine.ItemName = (string)dr["ITEMNAME"];
            posMenuLine.PaymentTypeName = (string)dr["PAYMENTTYPENAME"];
            posMenuLine.PosMenuName = (string)dr["POSMENUNAME"];
            posMenuLine.InfocodeName = (string)dr["INFOCODENAME"];
            posMenuLine.PosMenuAndButtonGridPosMenuName = (string)dr["POSMENUNAME2"];
            posMenuLine.SuspensionTypeName = (string)dr["SUSPENSIONTYPENAME"];
            posMenuLine.BlankOperationName = (string)dr["BLANKOPERATIONNAME"];
            posMenuLine.BlankOperationParameter = (string)dr["BlANKOPERATIONPARAMETER"];
            posMenuLine.IncomeExpenseAccountName = (string)dr["INCOMEEXPENSEACCOUNTNAME"];
            posMenuLine.StorePaymentAndAmountPaymentTypeName = (string)dr["PAYMENTTYPENAME2"];
            posMenuLine.KeyMapping = (Keys)dr["KEYMAPPING"];
            posMenuLine.StyleID = (string)dr["STYLEID"];
            posMenuLine.HospitalityOperationName = (string)dr["HOSPITALITYOPERATIONNAME"];
            posMenuLine.ManuallyTriggeredPeriodicDiscountName = (string)dr["PERIODICDISCOUNTNAME"];
            posMenuLine.ParameterItemID = (string)dr["PARAMETERITEMID"];
            
        }

        /// <summary>
        /// Gets a list of pos menu line list items for the given pos menu ID. This is intended for displaying pos menu lines only.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuID">The ID of the menu to get the menu lines for</param>
        /// <returns></returns>
        public virtual List<PosMenuLineListItem> GetListItems(IConnectionManager entry, RecordIdentifier posMenuID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    ListItemSelectString +
                    @"where p.MENUID = @menuId and p.DATAAREAID = @dataAreaId
                     ORDER BY p.KEYNO, p.SEQUENCE ";

                MakeParam(cmd, "menuId", (string)posMenuID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosMenuLineListItem>(entry, cmd, CommandType.Text, PopulateListItem);
            }
        }

        /// <summary>
        /// Gets a list of all pos menu lines.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of all pos menu lines</returns>
        public virtual List<PosMenuLine> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSelectString;
                return Execute<PosMenuLine>(entry, cmd, CommandType.Text, PopulatePosMenuLine);
            }
        }

        /// <summary>
        /// Returns a list of menu buttons that are using a specific style
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="styleID">The style ID to look for</param>
        /// <returns>A list of menu buttons that are using the style ID</returns>
        public virtual List<PosMenuLine> AreUsingStyle(IConnectionManager entry, RecordIdentifier styleID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = 
                    BaseSelectString +                        
                    @"WHERE STYLEID = @STYLEID";

                MakeParam(cmd, "STYLEID", (string)styleID);

                return Execute<PosMenuLine>(entry, cmd, CommandType.Text, PopulatePosMenuLine);
            }
        }

      
        /// <summary>
        /// Gets a list of all pos menu lines for a given pos menu ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuID">The ID of the menu to get the menu lines for</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>A list of PosMenuLine objects containing all menu lines for the given menu ID</returns>
        public virtual List<PosMenuLine> GetList(IConnectionManager entry, RecordIdentifier posMenuID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            if (cacheType != CacheType.CacheTypeNone)
            {
                var bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof(CacheBucket), "PosMenuLines" + (string)posMenuID);

                if (bucket != null)
                {
                    return (List<PosMenuLine>)bucket.BucketData;
                }
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where p.DATAAREAID = @dataAreaId and p.MENUID = @menuId" +
                    " ORDER BY p.KEYNO, p.SEQUENCE ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "menuId", (string)posMenuID);

                var result = Execute<PosMenuLine>(entry, cmd, CommandType.Text, PopulatePosMenuLine);

                if (cacheType != CacheType.CacheTypeNone)
                {
                    var bucket = new CacheBucket("PosMenuLines" + (string)posMenuID, "", result);

                    entry.Cache.AddEntityToCache(bucket.ID, bucket, cacheType);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets a pos menu line with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuLineID">The ID of the pos menu line to get (menuId,sequence)</param>
        /// <returns>The pos menu line with the given ID</returns>
        public virtual PosMenuLine Get(IConnectionManager entry, RecordIdentifier posMenuLineID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where p.DATAAREAID = @dataAreaId and p.MENUID = @menuId and p.SEQUENCE = @sequence";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "menuId", (string)posMenuLineID[0]);
                MakeParam(cmd, "sequence", (string)posMenuLineID[1]);

                var result = Execute<PosMenuLine>(entry, cmd, CommandType.Text, PopulatePosMenuLine);

                return result.Count > 0 ? result[0] : null;                
            }
        }

        /// <summary>
        /// Gets the next KeyNo value for the given menu ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuID">The id of the pos menu (the header ID)</param>
        /// <returns>The next KeyNo, which is the highest key number + 1</returns>
        public virtual int GetNextKeyNo(IConnectionManager entry, RecordIdentifier posMenuID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select ISNULL(MAX(KEYNO),0) as MAXKEYNO " +
                    "from POSMENULINE " +
                    "where DATAAREAID = @dataAreaId and MENUID = @menuId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "menuId", (string)posMenuID);

                return (int)entry.Connection.ExecuteScalar(cmd) + 1;
            }
        }

        public virtual List<int> GetOperations(IConnectionManager entry, RecordIdentifier posMenuHeaderID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT OPERATION
                                    FROM POSMENULINE
                                    WHERE MENUID = @menuID";

                MakeParam(cmd, "menuID", posMenuHeaderID);

                var result = Execute(entry, cmd, CommandType.Text, "OPERATION");
                return result.ConvertAll(r => (int)r);
            }
        }

        /// <summary>
        /// Checks if a pos menu line with the given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuLineID">The ID of the pos menu line to check for (menuId,sequence)</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier posMenuLineID)
        {
            return RecordExists(entry, "POSMENULINE", new[] { "MENUID", "SEQUENCE" }, posMenuLineID);
        }

        /// <summary>
        /// Checks if a given sequence exists int he pos menu line table
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sequence">The sequence to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        private static bool PosMenuLineSequenceExists(IConnectionManager entry, RecordIdentifier sequence)
        {
            return RecordExists(entry, "POSMENULINE", "SEQUENCE", sequence);
        }

        /// <summary>
        /// Deletes a pos menu line with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuLineID">The ID of the pos menu line to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier posMenuLineID)
        {
            DeleteRecord(entry, "POSMENULINE", new[] { "MENUID", "SEQUENCE" }, posMenuLineID, BusinessObjects.Permission.EditPosMenus);

            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "PosMenuLines" + (string)posMenuLineID);
        }

        /// <summary>
        /// Deletes all pos menu lines for a given menu header ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeaderID">The header ID</param>
        public virtual void DeleteForHeaderID(IConnectionManager entry, RecordIdentifier posMenuHeaderID)
        {
            DeleteRecord(entry, "POSMENULINE", "MENUID", posMenuHeaderID, BusinessObjects.Permission.EditPosMenus);
        }

        /// <summary>
        /// Saves a pos menu line into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuLine">The pos menu line to save</param>
        public virtual void Save(IConnectionManager entry, PosMenuLine posMenuLine)
        {
            var statement = new SqlServerStatement("POSMENULINE");

            ValidateSecurity(entry, BusinessObjects.Permission.EditPosMenus);

            bool isNew = false;
            if (posMenuLine.Sequence == RecordIdentifier.Empty)
            {
                isNew = true;
                posMenuLine.Sequence = DataProviderFactory.Instance.GenerateNumber<IPosMenuLineData, PosMenuLine>(entry);
                if (posMenuLine.KeyNo == 0)
                {
                    posMenuLine.KeyNo = GetNextKeyNo(entry, posMenuLine.MenuID);
                }
            }

            if (isNew || !Exists(entry, posMenuLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("MENUID", (string)posMenuLine.MenuID);
                statement.AddKey("SEQUENCE", (string)posMenuLine.Sequence);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("MENUID", (string)posMenuLine.MenuID);
                statement.AddCondition("SEQUENCE", (string)posMenuLine.Sequence);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("KEYNO", posMenuLine.KeyNo, SqlDbType.Int);
            statement.AddField("DESCRIPTION", posMenuLine.Text);
            statement.AddField("OPERATION", posMenuLine.Operation.StringValue == "" ? 0 : Convert.ToInt32(posMenuLine.Operation.StringValue), SqlDbType.Int);
            statement.AddField("PARAMETER", posMenuLine.Parameter);
            statement.AddField("PARAMETERTYPE", (int)posMenuLine.ParameterType, SqlDbType.Int);
            statement.AddField("FONTNAME", posMenuLine.FontName);
            statement.AddField("FONTSIZE", posMenuLine.FontSize, SqlDbType.Int);
            statement.AddField("FONTBOLD", posMenuLine.FontBold ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FORECOLOR", posMenuLine.ForeColor, SqlDbType.Int);
            statement.AddField("BACKCOLOR", posMenuLine.BackColor, SqlDbType.Int);
            statement.AddField("FONTITALIC", posMenuLine.FontItalic ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FONTCHARSET", posMenuLine.FontCharset, SqlDbType.Int);
            statement.AddField("DISABLED", posMenuLine.Disabled ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PICTUREID", (string)posMenuLine.PictureID);
            statement.AddField("HIDEDESCRONPICTURE", posMenuLine.HideDescrOnPicture ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FONTSTRIKETHROUGH", posMenuLine.FontStrikethrough ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("FONTUNDERLINE", posMenuLine.FontUnderline ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("COLUMNSPAN", posMenuLine.ColumnSpan, SqlDbType.Int);
            statement.AddField("ROWSPAN", posMenuLine.RowSpan, SqlDbType.Int);
            statement.AddField("NAVOPERATION", posMenuLine.NavOperation);
            statement.AddField("HIDDEN", posMenuLine.Hidden ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("SHADEWHENDISABLED", posMenuLine.ShadeWhenDisabled ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("BACKGROUNDHIDDEN", posMenuLine.BackgroundHidden ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TRANSPARENT", posMenuLine.Transparent ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("GLYPH", (int)posMenuLine.Glyph, SqlDbType.Int);
            statement.AddField("GLYPH2", (int)posMenuLine.Glyph2, SqlDbType.Int);
            statement.AddField("GLYPH3", (int)posMenuLine.Glyph3, SqlDbType.Int);
            statement.AddField("GLYPH4", (int)posMenuLine.Glyph4, SqlDbType.Int);
            statement.AddField("GLYPHTEXT", posMenuLine.GlyphText);
            statement.AddField("GLYPHTEXT2", posMenuLine.GlyphText2);
            statement.AddField("GLYPHTEXT3", posMenuLine.GlyphText3);
            statement.AddField("GLYPHTEXT4", posMenuLine.GlyphText4);
            statement.AddField("GLYPHTEXTFONT", posMenuLine.GlyphTextFont);
            statement.AddField("GLYPHTEXT2FONT", posMenuLine.GlyphText2Font);
            statement.AddField("GLYPHTEXT3FONT", posMenuLine.GlyphText3Font);
            statement.AddField("GLYPHTEXT4FONT", posMenuLine.GlyphText4Font);
            statement.AddField("GLYPHTEXTFONTSIZE", posMenuLine.GlyphTextFontSize, SqlDbType.Int);
            statement.AddField("GLYPHTEXT2FONTSIZE", posMenuLine.GlyphText2FontSize, SqlDbType.Int);
            statement.AddField("GLYPHTEXT3FONTSIZE", posMenuLine.GlyphText3FontSize, SqlDbType.Int);
            statement.AddField("GLYPHTEXT4FONTSIZE", posMenuLine.GlyphText4FontSize, SqlDbType.Int);
            statement.AddField("GLYPHTEXTFORECOLOR", posMenuLine.GlyphTextForeColor, SqlDbType.Int);
            statement.AddField("GLYPHTEXT2FORECOLOR", posMenuLine.GlyphText2ForeColor, SqlDbType.Int);
            statement.AddField("GLYPHTEXT3FORECOLOR", posMenuLine.GlyphText3ForeColor, SqlDbType.Int);
            statement.AddField("GLYPHTEXT4FORECOLOR", posMenuLine.GlyphText4ForeColor, SqlDbType.Int);
            statement.AddField("GLYPHOFFSET", posMenuLine.GlyphOffSet, SqlDbType.Int);
            statement.AddField("GLYPH2OFFSET", posMenuLine.Glyph2OffSet, SqlDbType.Int);
            statement.AddField("GLYPH3OFFSET", posMenuLine.Glyph3OffSet, SqlDbType.Int);
            statement.AddField("GLYPH4OFFSET", posMenuLine.Glyph4OffSet, SqlDbType.Int);
            statement.AddField("BACKCOLOR2", posMenuLine.BackColor2, SqlDbType.Int);
            statement.AddField("GRADIENTMODE", (int)posMenuLine.GradientMode, SqlDbType.Int);
            statement.AddField("SHAPE", (int)posMenuLine.Shape, SqlDbType.Int);
            statement.AddField("USEHEADERFONT", posMenuLine.UseHeaderFont ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("USEHEADERATTRIBUTES", posMenuLine.UseHeaderAttributes ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("IMAGEPOSITION", (int)posMenuLine.ImagePosition, SqlDbType.Int);
            statement.AddField("TEXTPOSITION", (int)posMenuLine.TextPosition, SqlDbType.Int);
            statement.AddField("STYLEID", (string)posMenuLine.StyleID);
            statement.AddField("KEYMAPPING", (int)posMenuLine.KeyMapping, SqlDbType.Int);
            statement.AddField("USEIMAGEFONT", posMenuLine.UseImageFont, SqlDbType.Bit);
            statement.AddField("IMAGEFONTTEXT", posMenuLine.ImageFontText);
            statement.AddField("IMAGEFONTNAME", posMenuLine.ImageFontName);
            statement.AddField("IMAGEFONTSIZE", posMenuLine.ImageFontSize, SqlDbType.Int);
            statement.AddField("IMAGEFONTBOLD", posMenuLine.ImageFontBold, SqlDbType.Bit);
            statement.AddField("IMAGEFONTITALIC", posMenuLine.ImageFontItalic, SqlDbType.Bit);
            statement.AddField("IMAGEFONTUNDERLINE", posMenuLine.ImageFontUnderline, SqlDbType.Bit);
            statement.AddField("IMAGEFONTSTRIKETHROUGH", posMenuLine.ImageFontStrikethrough, SqlDbType.Bit);
            statement.AddField("IMAGEFONTCHARSET", posMenuLine.ImageFontCharset, SqlDbType.Int);
            statement.AddField("IMAGEFONTCOLOR", posMenuLine.ImageFontColor, SqlDbType.Int);
            statement.AddField("PARAMETERITEMID", posMenuLine.ParameterItemID, SqlDbType.NVarChar);

            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "PosMenuLines" + (string)posMenuLine.MenuID);
            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SaveOrder(IConnectionManager entry, PosMenuLineListItem posMenuLineListOrder)
        {
            var statement = new SqlServerStatement("POSMENULINE") {StatementType = StatementType.Update};

            statement.AddCondition("MENUID", (string)posMenuLineListOrder.MenuID);
            statement.AddCondition("SEQUENCE", (string)posMenuLineListOrder.Sequence);
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);

            statement.AddField("KEYNO", posMenuLineListOrder.KeyNo, SqlDbType.Int);
            entry.Cache.DeleteEntityFromCache(typeof(CacheBucket), "PosMenuLines" + (string)posMenuLineListOrder.MenuID);
            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return PosMenuLineSequenceExists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "PosMenuLN"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSMENULINE", "SEQUENCE", sequenceFormat, startingRecord, numberOfRecords);
        }
        #endregion
    }
}
