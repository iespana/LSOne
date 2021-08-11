using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders.ConfigurationWizard;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;

namespace LSOne.DataLayer.SqlDataProviders.ConfigurationWizard
{
    /// <summary>
    /// Data prover class for TouchButtonLayout.
    /// </summary>
    public class TouchButtonLayoutData : SqlServerDataProviderBase, ITouchButtonLayoutData
    {
        private static void PopulateTouchButtonLayout(IConnectionManager entry, IDataReader dr, TouchButtonLayout touchButtonLayout, object obj)
        {
            touchButtonLayout.LineNum = (int)dr["LINENUM"];
            touchButtonLayout.TillLayoutID = (string)dr["TILLLAYOUTID"];
            touchButtonLayout.RetailGrid = (string)dr["RETAILGRID"];
            touchButtonLayout.ItemGridID = (string)dr["ITEMGRIDID"];
            touchButtonLayout.Image = (dr["IMAGE"] == DBNull.Value) ? null : (byte[])dr["IMAGE"];
            touchButtonLayout.ID = (string)dr["ID"];
        }

        /// <summary>
        /// Get layout list of selected template from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>Layout list</returns>
        public virtual List<TouchButtonLayout> GetTouchButtonLayoutList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID,
                                    ISNULL(LINENUM, 0) as LINENUM,
                                    ISNULL(TILLLAYOUTID, '') as TILLLAYOUTID,
                                    ISNULL(RETAILGRID, '') as RETAILGRID,
                                    ISNULL(ITEMGRIDID, '') as ITEMGRIDID,
                                    IMAGE
                                    FROM WIZARDTEMPLATETILLLAYOUTS 
                                    WHERE ID = @ID
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                return Execute<TouchButtonLayout>(entry, cmd, CommandType.Text, null, PopulateTouchButtonLayout);
            }
        }

        /// <summary>
        /// Save layouts into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="layoutList">Layout list</param>
        public virtual void SaveLayouts(IConnectionManager entry, List<TouchButtonLayout> layoutList)
        {
            Delete(entry, layoutList.Where(item => item.ID != RecordIdentifier.Empty).FirstOrDefault().ID);

            foreach (TouchButtonLayout layout in layoutList)
            {
                if (layout.ID != RecordIdentifier.Empty)
                {
                    var statement = new SqlServerStatement("WIZARDTEMPLATETILLLAYOUTS")
                        {
                            StatementType = StatementType.Insert
                        };

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                    statement.AddKey("ID", (string)layout.ID);

                    statement.AddField("RETAILGRID", layout.RetailGrid, SqlDbType.NVarChar);

                    statement.AddField("ITEMGRIDID", layout.ItemGridID, SqlDbType.NVarChar);

                    statement.AddField("LINENUM", layout.LineNum, SqlDbType.Int);

                    statement.AddField("TILLLAYOUTID", (string)layout.TillLayoutID, SqlDbType.NVarChar);

                    statement.AddField("IMAGE", layout.Image, SqlDbType.Image);

                    entry.Connection.ExecuteStatement(statement);
                }
            }
        }

        /// <summary>
        /// Delete a entity declaration from given table with a given id.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <param name="table">Table name</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATETILLLAYOUTS")
        {
            DeleteRecord(entry, table, "ID", id, BusinessObjects.Permission.ManageTouchButtonLayout);
        }

        private static void PopulateTouchLayout(IDataReader dr, TouchLayout touchLayout)
        {
            touchLayout.ID = (string)dr["LAYOUTID"];
            touchLayout.Name = (string)dr["NAME"];
            touchLayout.Text = touchLayout.Name;
            touchLayout.Width = (int)dr["WIDTH"];
            touchLayout.Height = (int)dr["HEIGHT"];
            touchLayout.ButtonGrid1 = (string)dr["BUTTONGRID1"];
            touchLayout.ButtonGrid2 = (string)dr["BUTTONGRID2"];
            touchLayout.ButtonGrid3 = (string)dr["BUTTONGRID3"];
            touchLayout.ButtonGrid4 = (string)dr["BUTTONGRID4"];
            touchLayout.ButtonGrid5 = (string)dr["BUTTONGRID5"];
            touchLayout.ReceiptID = (string)dr["RECEIPTID"];
            touchLayout.TotalID = (string)dr["TOTALID"];
            touchLayout.LogoPictureID = (string)dr["LOGOPICTUREID"];
            touchLayout.ImgCustomerLayoutXML = dr["IMG_CUSTOMERLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_CUSTOMERLAYOUTXML"];
            touchLayout.ImgReceiptItemsLayoutXML = dr["IMG_RECEIPTITEMSLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_RECEIPTITEMSLAYOUTXML"];
            touchLayout.ImgReceiptPaymentLayoutXML = dr["IMG_RECEIPTPAYMENTLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_RECEIPTPAYMENTLAYOUTXML"];
            touchLayout.ImgTotalsLayoutXML = dr["IMG_TOTALSLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_TOTALSLAYOUTXML"];
            touchLayout.ImgLayoutXML = dr["IMG_LAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_LAYOUTXML"];
            touchLayout.ReceiptItemsLayoutXML = (string)dr["RECEIPTITEMSLAYOUTXML"];
            touchLayout.ReceiptPaymentLayoutXML = (string)dr["RECEIPTPAYMENTLAYOUTXML"];
            touchLayout.TotalsLayoutXML = (string)dr["TOTALSLAYOUTXML"];
            touchLayout.LayoutXML = (string)dr["LAYOUTXML"];
            touchLayout.ImgCashChangerLayoutXML = dr["IMG_CASHCHANGERLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_CASHCHANGERLAYOUTXML"];
            touchLayout.CashChangerLayoutXML = (string)dr["CASHCHANGERLAYOUTXML"];
        }

        /// <summary>
        /// Get layout list with all the values from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">LayoutIds</param>
        /// <param name="cache">CacheType</param>
        /// <returns>List of Layouts</returns>
        public virtual List<TouchLayout> GetTouchLayout(IConnectionManager entry, List<RecordIdentifier> idList, CacheType cache = CacheType.CacheTypeNone)
        {
            string ids = "";
            foreach (RecordIdentifier id in idList)
            {
                ids += "'" + id.StringValue + "',";
            }
            ids = ids.Remove(ids.LastIndexOf(','));
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "select " +
                    "LAYOUTID, " +
                    "ISNULL(NAME,'') as NAME, " +
                    "ISNULL(WIDTH, 0) as WIDTH, " +
                    "ISNULL(HEIGHT,0) as HEIGHT, " +
                    "ISNULL(BUTTONGRID1,'') as BUTTONGRID1, " +
                    "ISNULL(BUTTONGRID2,'') as BUTTONGRID2, " +
                    "ISNULL(BUTTONGRID3,'') as BUTTONGRID3, " +
                    "ISNULL(BUTTONGRID4,'') as BUTTONGRID4, " +
                    "ISNULL(BUTTONGRID5,'') as BUTTONGRID5, " +
                    "ISNULL(RECEIPTID,'') as RECEIPTID, " +
                    "ISNULL(TOTALID,'') as TOTALID, " +
                    "ISNULL(LOGOPICTUREID,-1) as LOGOPICTUREID, " +
                    "IMG_CUSTOMERLAYOUTXML, " +
                    "IMG_RECEIPTITEMSLAYOUTXML, " +
                    "IMG_RECEIPTPAYMENTLAYOUTXML, " +
                    "IMG_TOTALSLAYOUTXML, " +
                    "IMG_LAYOUTXML, " +
                    "ISNULL(RECEIPTITEMSLAYOUTXML,'') as RECEIPTITEMSLAYOUTXML, " +
                    "ISNULL(RECEIPTPAYMENTLAYOUTXML,'') as RECEIPTPAYMENTLAYOUTXML, " +
                    "ISNULL(TOTALSLAYOUTXML,'') as TOTALSLAYOUTXML, " +
                    "ISNULL(LAYOUTXML,'') as LAYOUTXML, " +
                    "IMG_CASHCHANGERLAYOUTXML, " +
                    "ISNULL(CASHCHANGERLAYOUTXML,'') as CASHCHANGERLAYOUTXML " +
                    "from POSISTILLLAYOUT " +
                    "where DATAAREAID = @DATAAREAID and LAYOUTID in (" + ids + ")";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                List<TouchLayout> result = Execute<TouchLayout>(entry, cmd, CommandType.Text, PopulateTouchLayout);

                return result.Count > 0 ? result : null;
            }
        }

        private static void PopulatePosMenuHeader(IDataReader dr, PosMenuHeader posMenuHeader)
        {
            posMenuHeader.ID = (string)dr["MENUID"];
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
            posMenuHeader.DeviceType = (DeviceTypeEnum)((int)dr["DEVICETYPE"]);
            posMenuHeader.MainMenu = AsBool(dr["MAINMENU"]);
        }

        /// <summary>
        /// Get PosMenuHeader list with all the values from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuHeaderIDs">posMenuHeaderIds</param>
        /// <param name="cacheType">CacheType</param>
        /// <returns>PosMenuHeader list</returns>
        public virtual List<PosMenuHeader> GetPosMenuHeader(IConnectionManager entry, List<RecordIdentifier> posMenuHeaderIDs, CacheType cacheType = CacheType.CacheTypeNone)
        {
            string ids = "";
            foreach (RecordIdentifier id in posMenuHeaderIDs)
            {
                ids += "'" + id.StringValue + "',";
            }
            ids = ids.Remove(ids.LastIndexOf(','));
            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText =
                    @"select
                        MENUID,
                        ISNULL(DESCRIPTION,'') as DESCRIPTION,
                        ISNULL(COLUMNS,0) as COLUMNS,
                        ISNULL(ROWS,0) as ROWS,
                        ISNULL(MENUCOLOR,0) as MENUCOLOR,
                        ISNULL(FONTNAME,'') as FONTNAME,
                        ISNULL(FONTSIZE,0) as FONTSIZE,
                        ISNULL(FONTBOLD,0) as FONTBOLD,
                        ISNULL(FORECOLOR,0) as FORECOLOR,
                        ISNULL(BACKCOLOR,0) as BACKCOLOR,
                        BORDERCOLOR,
                        BORDERWIDTH,
                        MARGIN,
                        TEXTPOSITION,
                        ISNULL(FONTITALIC,0) as FONTITALIC,
                        ISNULL(FONTCHARSET,0) as FONTCHARSET,
                        ISNULL(USENAVOPERATION,0) as USENAVOPERATION, 
                        ISNULL(APPLIESTO,0) as APPLIESTO, 
                        ISNULL(BACKCOLOR2,0) as BACKCOLOR2,
                        ISNULL(GRADIENTMODE,0) as GRADIENTMODE, 
                        ISNULL(SHAPE,0) as SHAPE, 
                        ISNULL(MENUTYPE,0) as MENUTYPE, 
                        ISNULL(STYLEID,'') as STYLEID,
                        ISNULL(DEVICETYPE,0) as DEVICETYPE,
                        ISNULL(MAINMENU,0) as MAINMENU
                        from POSMENUHEADER " +
                    "where DATAAREAID = @DATAAREAID and MENUID in (" + ids + ")";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                List<PosMenuHeader> result = Execute<PosMenuHeader>(entry, cmd, CommandType.Text, PopulatePosMenuHeader);

                return result.Count > 0 ? result : null;
            }
        }

        private static void PopulatePosMenuLine(IDataReader dr, PosMenuLine posMenuLine)
        {
            posMenuLine.MenuID = (string)dr["MENUID"];
            posMenuLine.Sequence = (string)dr["SEQUENCE"];
            posMenuLine.KeyNo = (int)dr["KEYNO"];
            posMenuLine.Text = (string)dr["DESCRIPTION"];
            posMenuLine.Operation = (int)dr["OPERATION"];
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
        }

        /// <summary>
        /// Get PosMenuLine list with all the values from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posMenuLineIDs">PosMenuLineIds</param>
        /// <returns>PosMenuLine list</returns>
        public virtual List<PosMenuLine> GetPosMenuLine(IConnectionManager entry, List<RecordIdentifier> posMenuLineIDs)
        {
            string ids = "";
            foreach (RecordIdentifier id in posMenuLineIDs)
            {
                ids += "'" + id.StringValue + "',";
            }
            ids = ids.Remove(ids.LastIndexOf(','));

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select 
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
                    ISNULL(p.PICTUREID,'') as PICTUREID,
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
                    ISNULL(p.KEYMAPPING,'0') as KEYMAPPING,
                    ISNULL(p.IMAGEFONTTEXT, '') AS IMAGEFONTTEXT,
                    ISNULL(p.IMAGEFONTNAME, '') AS IMAGEFONTNAME,
                    ISNULL(p.IMAGEFONTSIZE, 0) AS IMAGEFONTSIZE,
                    ISNULL(p.IMAGEFONTBOLD, 0) AS IMAGEFONTBOLD,
                    ISNULL(p.IMAGEFONTITALIC, 0) AS IMAGEFONTITALIC,
                    ISNULL(p.IMAGEFONTUNDERLINE, 0) AS IMAGEFONTUNDERLINE,
                    ISNULL(p.IMAGEFONTSTRIKETHROUGH, 0) AS IMAGEFONTSTRIKETHROUGH,
                    ISNULL(p.IMAGEFONTCHARSET, 0) AS IMAGEFONTCHARSET,
                    ISNULL(p.IMAGEFONTCOLOR, 0) AS IMAGEFONTCOLOR                                         
                    from POSMENULINE p
                    left outer join OPERATIONS posop on p.OPERATION = posop.ID 
                    left outer join POSISHOSPITALITYOPERATIONS hospop on p.DATAAREAID = hospop.DATAAREAID and p.OPERATION = hospop.OPERATIONID " +
                    "where p.DATAAREAID = @DATAAREAID and p.MENUID in (" + ids + ")";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                List<PosMenuLine> result = Execute<PosMenuLine>(entry, cmd, CommandType.Text, PopulatePosMenuLine);

                return result.Count > 0 ? result : null;
            }
        }

        /// <summary>
        /// Get list of PosMenuHeader with MenuId, Description values.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="condition">condition</param>
        /// <returns>list of PosMenuHeader</returns>
        public virtual List<DataEntity> Get(IConnectionManager entry, string condition)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT MENUID, [DESCRIPTION] 
                                    FROM POSMENUHEADER 
                                    WHERE DEFAULTOPERATION = @CONDITION
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "CONDITION", condition);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "MENUID");
            }
        }
    }
}
