using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.GUI;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard
{
    /// <summary>
    /// Business entity class of TouchButtonLayout page
    /// </summary>
    public class TouchButtonLayout : DataEntity
    {
        public TouchButtonLayout()
        {
            ID = RecordIdentifier.Empty;
            LineNum = int.MinValue;
            TillLayoutID = RecordIdentifier.Empty;
            RetailGrid = string.Empty;
            ItemGridID = string.Empty;
            Image = null;

            TouchLayout = new TouchLayout();
            MenuHeader = new PosMenuHeader();
            MenuLine = new PosMenuLine();
        }

        /// <summary>
        /// The layout number; 1, 2, 3 or 4
        /// </summary>
        public int? LineNum { get; set; }

        /// <summary>
        /// The selected layout
        /// </summary>
        public RecordIdentifier TillLayoutID { get; set; }

        /// <summary>
        /// The retail group button grid
        /// </summary>
        public string RetailGrid { get; set; }

        /// <summary>
        /// The item button grid
        /// </summary>
        public string ItemGridID { get; set; }

        /// <summary>
        /// The image of the layout
        /// </summary>
        public byte[] Image { get; set; }

        public TouchLayout TouchLayout { get; set; }

        public PosMenuHeader MenuHeader { get; set; }

        public PosMenuLine MenuLine { get; set; }

        /// <summary>
        /// Sets all variables in the TouchButtonSetting class with the values in the xml
        /// </summary>
        /// <param name="xTouchButtonSetting">The xml element with the touch layout setting values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xTouchButtonSetting, IErrorLog errorLogger = null)
        {
            if (xTouchButtonSetting.Name.ToString() == "layoutImage")
            {
                var storeVariables = xTouchButtonSetting.Attributes();
                foreach (var storeElem in storeVariables)
                {
                    //No till Layout ID -> no touch setting -> no need to go any further
                    if (storeElem.Name.ToString() == "tillLayoutID" && storeElem.Value == "")
                    {
                        return;
                    }
                    if (storeElem.Value.Length > 0)
                    {
                        try
                        {
                            switch (storeElem.Name.ToString())
                            {
                                case "tillLayoutID":
                                    TillLayoutID = storeElem.Value;
                                    Image = Convert.FromBase64String(xTouchButtonSetting.Value);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }
            if (xTouchButtonSetting.HasElements)
            {
                if (xTouchButtonSetting.Name.ToString() == "tillLayout")
                {
                    foreach (var storeElem in xTouchButtonSetting.Elements())
                    {
                        if (storeElem.Name.ToString() == "tillLayoutID" && storeElem.Value == "")
                        {
                            return;
                        }
                        if (!storeElem.IsEmpty)
                        {
                            try
                            {
                                switch (storeElem.Name.ToString())
                                {
                                    case "tillLayoutID":
                                        TouchLayout.ID = storeElem.Value;
                                        break;
                                    case "name":
                                        TouchLayout.Name = storeElem.Value;
                                        break;
                                    case "width":
                                        TouchLayout.Width = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "height":
                                        TouchLayout.Height = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "buttonGrid1":
                                        TouchLayout.ButtonGrid1 = storeElem.Value;
                                        break;
                                    case "buttonGrid2":
                                        TouchLayout.ButtonGrid2 = storeElem.Value;
                                        break;
                                    case "buttonGrid3":
                                        TouchLayout.ButtonGrid3 = storeElem.Value;
                                        break;
                                    case "buttonGrid4":
                                        TouchLayout.ButtonGrid4 = storeElem.Value;
                                        break;
                                    case "buttonGrid5":
                                        TouchLayout.ButtonGrid5 = storeElem.Value;
                                        break;
                                    case "receiptID":
                                        TouchLayout.ReceiptID = storeElem.Value;
                                        break;
                                    case "totalID":
                                        TouchLayout.TotalID = storeElem.Value;
                                        break;
                                    case "logoPictureID":
                                        TouchLayout.LogoPictureID = storeElem.Value;
                                        break;
                                    case "imgCustomerLayoutXML":
                                        TouchLayout.ImgCustomerLayoutXML = storeElem.Value;
                                        break;
                                    case "imgReceiptItemsLayoutXML":
                                        TouchLayout.ImgReceiptItemsLayoutXML = storeElem.Value;
                                        break;
                                    case "imgReceiptPaymentLayoutXML":
                                        TouchLayout.ImgReceiptPaymentLayoutXML = storeElem.Value;
                                        break;
                                    case "imgTotalsLayoutXML":
                                        TouchLayout.ImgTotalsLayoutXML = storeElem.Value;
                                        break;
                                    case "imgLayoutXML":
                                        TouchLayout.ImgLayoutXML = storeElem.Value;
                                        break;
                                    case "receiptItemsLayoutXML":
                                        TouchLayout.ReceiptItemsLayoutXML = storeElem.Value;
                                        break;
                                    case "receiptPaymentLayoutXML":
                                        TouchLayout.ReceiptPaymentLayoutXML = storeElem.Value;
                                        break;
                                    case "totalsLayoutXML":
                                        TouchLayout.TotalsLayoutXML = storeElem.Value;
                                        break;
                                    case "layoutXML":
                                        TouchLayout.LayoutXML = storeElem.Value;
                                        break;
                                    case "imgCashChangerLayoutXML":
                                        TouchLayout.ImgCashChangerLayoutXML = storeElem.Value;
                                        break;
                                    case "cashChangerLayoutXML":
                                        TouchLayout.CashChangerLayoutXML = storeElem.Value;
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }
                if (xTouchButtonSetting.Name.ToString() == "posMenuHeader")
                {
                    var storeVariables = xTouchButtonSetting.Elements();
                    foreach (var storeElem in storeVariables)
                    {
                        //No menu id -> no touch button setting -> no need to go any further
                        if (storeElem.Name.ToString() == "menuID" && storeElem.Value == "")
                        {
                            return;
                        }

                        if (!storeElem.IsEmpty)
                        {
                            try
                            {
                                switch (storeElem.Name.ToString())
                                {
                                    case "menuID":
                                        MenuHeader.ID = storeElem.Value;
                                        break;
                                    case "description":
                                        MenuHeader.Text = storeElem.Value;
                                        break;
                                    case "columns":
                                        MenuHeader.Columns = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "rows":
                                        MenuHeader.Rows = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "menuColor":
                                        MenuHeader.MenuColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "fontName":
                                        MenuHeader.FontName = storeElem.Value;
                                        break;
                                    case "fontSize":
                                        MenuHeader.FontSize = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "fontBold":
                                        MenuHeader.FontBold = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "foreColor":
                                        MenuHeader.ForeColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "backColor":
                                        MenuHeader.BackColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "borderColor":
                                        MenuHeader.BorderColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "borderWidth":
                                        MenuHeader.BorderWidth = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "margin":
                                        MenuHeader.Margin = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "textPosition":
                                        MenuHeader.TextPosition = (Position)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "fontItalic":
                                        MenuHeader.FontItalic = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "fontCharset":
                                        MenuHeader.FontCharset = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "useNavOperation":
                                        MenuHeader.UseNavOperation = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "appliesTo":
                                        MenuHeader.AppliesTo = (PosMenuHeader.AppliesToEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "backColor2":
                                        MenuHeader.BackColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "gradientMode":
                                        MenuHeader.GradientMode = (GradientModeEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "shape":
                                        MenuHeader.Shape = (ShapeEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "menuType":
                                        MenuHeader.MenuType = (MenuTypeEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "styleID":
                                        MenuHeader.StyleID = storeElem.Value;
                                        break;
                                    case "deviceType":
                                        MenuHeader.DeviceType = (DeviceTypeEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "mainMenu":
                                        MenuHeader.MainMenu = storeElem.Value != "false";
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }

                if (xTouchButtonSetting.Name.ToString() == "posMenuLine")
                {
                    var storeVariables = xTouchButtonSetting.Elements();
                    foreach (var storeElem in storeVariables)
                    {
                        //No menu id -> no touch button setting -> no need to go any further
                        if (storeElem.Name.ToString() == "menuID" && storeElem.Value == "")
                        {
                            return;
                        }
                        if (!storeElem.IsEmpty)
                        {
                            try
                            {
                                switch (storeElem.Name.ToString())
                                {
                                    case "menuID":
                                        MenuLine.MenuID = storeElem.Value;
                                        break;
                                    case "sequence":
                                        MenuLine.Sequence = storeElem.Value;
                                        break;
                                    case "keyNo":
                                        MenuLine.KeyNo = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "operation":
                                        MenuLine.Operation = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "parameter":
                                        MenuLine.Parameter = storeElem.Value;
                                        break;
                                    case "parameterType":
                                        MenuLine.ParameterType = (PosMenuLine.ParameterTypeEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "fontName":
                                        MenuLine.FontName = storeElem.Value;
                                        break;
                                    case "fontSize":
                                        MenuLine.FontSize = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "fontBold":
                                        MenuLine.FontBold = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "foreColor":
                                        MenuLine.ForeColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "backColor":
                                        MenuLine.BackColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "fontItalic":
                                        MenuLine.FontItalic = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "fontCharset":
                                        MenuLine.FontCharset = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "disabled":
                                        MenuLine.Disabled = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "pictureID":
                                        MenuLine.PictureID = storeElem.Value;
                                        break;
                                    case "hideDescrOnPicture":
                                        MenuLine.HideDescrOnPicture = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "fontStrikethrough":
                                        MenuLine.FontStrikethrough = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "fontUnderline":
                                        MenuLine.FontUnderline = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "columnSpan":
                                        MenuLine.ColumnSpan = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "rowSpan":
                                        MenuLine.RowSpan = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "navOperation":
                                        MenuLine.NavOperation = storeElem.Value;
                                        break;
                                    case "hidden":
                                        MenuLine.Hidden = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "shadeWhenDisabled":
                                        MenuLine.ShadeWhenDisabled = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "backgroundHidden":
                                        MenuLine.BackgroundHidden = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "transparent":
                                        MenuLine.Transparent = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "glyph":
                                        MenuLine.Glyph = (PosMenuLine.GlyphEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyph2":
                                        MenuLine.Glyph2 = (PosMenuLine.GlyphEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyph3":
                                        MenuLine.Glyph3 = (PosMenuLine.GlyphEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyph4":
                                        MenuLine.Glyph4 = (PosMenuLine.GlyphEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyphText":
                                        MenuLine.GlyphText = storeElem.Value;
                                        break;
                                    case "glyphText2":
                                        MenuLine.GlyphText2 = storeElem.Value;
                                        break;
                                    case "glyphText3":
                                        MenuLine.GlyphText3 = storeElem.Value;
                                        break;
                                    case "glyphText4":
                                        MenuLine.GlyphText4 = storeElem.Value;
                                        break;
                                    case "glyphTextFont":
                                        MenuLine.GlyphTextFont = storeElem.Value;
                                        break;
                                    case "glyphText2Font":
                                        MenuLine.GlyphText2Font = storeElem.Value;
                                        break;
                                    case "glyphText3Font":
                                        MenuLine.GlyphText3Font = storeElem.Value;
                                        break;
                                    case "glyphText4Font":
                                        MenuLine.GlyphText4Font = storeElem.Value;
                                        break;
                                    case "glyphTextForeColor":
                                        MenuLine.GlyphTextForeColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyphText2ForeColor":
                                        MenuLine.GlyphText2ForeColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyphText3ForeColor":
                                        MenuLine.GlyphText3ForeColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyphText4ForeColor":
                                        MenuLine.GlyphText4ForeColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyphOffSet":
                                        MenuLine.GlyphOffSet = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyph2OffSet":
                                        MenuLine.Glyph2OffSet = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyph3OffSet":
                                        MenuLine.Glyph3OffSet = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "glyph4OffSet":
                                        MenuLine.Glyph4OffSet = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "backColor2":
                                        MenuLine.BackColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "gradientMode":
                                        MenuLine.GradientMode = (GradientModeEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "shape":
                                        MenuLine.Shape = (ShapeEnum)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "useHeaderFont":
                                        MenuLine.UseHeaderFont = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "useHeaderAttributes":
                                        MenuLine.UseHeaderAttributes = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "imagePosition":
                                        MenuLine.ImagePosition = (Position)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "textPosition":
                                        MenuLine.TextPosition = (Position)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "keyMapping":
                                        MenuLine.KeyMapping = (Keys)Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "posOperationName":
                                        MenuLine.PosOperationName = storeElem.Value;
                                        break;
                                    case "hospitalityOperationName":
                                        MenuLine.HospitalityOperationName = storeElem.Value;
                                        break;
                                    case "dirty":
                                        MenuLine.Dirty = Convert.ToBoolean(storeElem.Value);
                                        break;
                                    case "useImageFont":
                                        MenuLine.UseImageFont = storeElem.Value != "false";
                                        break;
                                    case "imageFontText":
                                        MenuLine.ImageFontText = storeElem.Value;
                                        break;
                                    case "imageFontName":
                                        MenuLine.ImageFontName = storeElem.Value;
                                        break;
                                    case "imageFontSize":
                                        MenuLine.ImageFontSize = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "imageFontBold":
                                        MenuLine.ImageFontBold = storeElem.Value != "false";
                                        break;
                                    case "imageFontItalic":
                                        MenuLine.ImageFontItalic = storeElem.Value != "false";
                                        break;
                                    case "imageFontUnderline":
                                        MenuLine.ImageFontUnderline = storeElem.Value != "false";
                                        break;
                                    case "imageFontStrikethrough":
                                        MenuLine.ImageFontStrikethrough = storeElem.Value != "false";
                                        break;
                                    case "imageFontCharset":
                                        MenuLine.ImageFontCharset = Convert.ToInt32(storeElem.Value);
                                        break;
                                    case "imageFontColor":
                                        MenuLine.ImageFontColor = Convert.ToInt32(storeElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, storeElem.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an xml element from all the variables in the TouchButtonSetting class
        /// </summary>
        /// <param name="errorLogger"></param>
        /// <returns>An XML element</returns>
        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast 
                * 
               */
            XElement xStoreSetting = null;
            if (Image != null)
            {
                xStoreSetting = new XElement("layoutImage", Convert.ToBase64String(Image),
                    new XAttribute("tillLayoutID", (string)TillLayoutID)
                    );
            }
            if (TouchLayout.ID.StringValue != string.Empty)
            {
                xStoreSetting = new XElement("tillLayout",
                    new XElement("tillLayoutID", (string)TouchLayout.ID),
                    new XElement("name", TouchLayout.Name),
                    new XElement("width", TouchLayout.Width),
                    new XElement("height", TouchLayout.Height),
                    new XElement("buttonGrid1", (string)TouchLayout.ButtonGrid1),
                    new XElement("buttonGrid2", (string)TouchLayout.ButtonGrid2),
                    new XElement("buttonGrid3", (string)TouchLayout.ButtonGrid3),
                    new XElement("buttonGrid4", (string)TouchLayout.ButtonGrid4),
                    new XElement("buttonGrid5", (string)TouchLayout.ButtonGrid5),
                    new XElement("receiptID", TouchLayout.ReceiptID),
                    new XElement("totalID", TouchLayout.TotalID),
                    new XElement("logoPictureID", (string)TouchLayout.LogoPictureID),
                    new XElement("imgCustomerLayoutXML", TouchLayout.ImgCustomerLayoutXML),
                    new XElement("imgReceiptItemsLayoutXML", TouchLayout.ImgReceiptItemsLayoutXML),
                    new XElement("imgReceiptPaymentLayoutXML", TouchLayout.ImgReceiptPaymentLayoutXML),
                    new XElement("imgTotalsLayoutXML", TouchLayout.ImgTotalsLayoutXML),
                    new XElement("imgLayoutXML", TouchLayout.ImgLayoutXML),
                    new XElement("receiptItemsLayoutXML", TouchLayout.ReceiptItemsLayoutXML),
                    new XElement("receiptPaymentLayoutXML", TouchLayout.ReceiptPaymentLayoutXML),
                    new XElement("totalsLayoutXML", TouchLayout.TotalsLayoutXML),
                    new XElement("layoutXML", TouchLayout.LayoutXML),
                    new XElement("imgCashChangerLayoutXML", TouchLayout.ImgCashChangerLayoutXML),
                    new XElement("cashChangerLayoutXML", TouchLayout.CashChangerLayoutXML)
                    );
            }
            if (TouchLayout.Name == string.Empty && MenuLine.Sequence == RecordIdentifier.Empty && Image == null)
            {
                xStoreSetting = new XElement("posMenuHeader",
                    new XElement("menuID", (string)MenuHeader.ID),
                    new XElement("description", MenuHeader.Text),
                    new XElement("columns", MenuHeader.Columns),
                    new XElement("rows", MenuHeader.Rows),
                    new XElement("menuColor", MenuHeader.MenuColor),
                    new XElement("fontName", MenuHeader.FontName),
                    new XElement("fontSize", MenuHeader.FontSize),
                    new XElement("fontBold", MenuHeader.FontBold),
                    new XElement("foreColor", MenuHeader.ForeColor),
                    new XElement("backColor", MenuHeader.BackColor),
                    new XElement("borderColor", MenuHeader.BorderColor),
                    new XElement("borderWidth", MenuHeader.BorderWidth),
                    new XElement("margin", MenuHeader.Margin),
                    new XElement("textPosition", MenuHeader.TextPosition),
                    new XElement("fontItalic", MenuHeader.FontItalic),
                    new XElement("fontCharset", MenuHeader.FontCharset),
                    new XElement("useNavOperation", MenuHeader.UseNavOperation),
                    new XElement("appliesTo", (int)MenuHeader.AppliesTo),
                    new XElement("backColor2", MenuHeader.BackColor2),
                    new XElement("gradientMode", (int)MenuHeader.GradientMode),
                    new XElement("shape", (int)MenuHeader.Shape),
                    new XElement("menuType", (int)MenuHeader.MenuType),
                    new XElement("styleID", (string)MenuHeader.StyleID),
                    new XElement("deviceType", (int)MenuHeader.DeviceType),
                    new XElement("mainMenu", MenuHeader.MainMenu)
                    );
            }
            if (TouchLayout.Name == string.Empty && MenuLine.Sequence != RecordIdentifier.Empty)
            {
                xStoreSetting = new XElement("posMenuLine",
                    new XElement("text", MenuLine.Text),
                    new XElement("menuID", (string)MenuLine.MenuID),
                    new XElement("sequence", (string)MenuLine.Sequence),
                    new XElement("keyNo", MenuLine.KeyNo),
                    new XElement("operation", (int)MenuLine.Operation),
                    new XElement("parameter", MenuLine.Parameter),
                    new XElement("parameterType", (int)MenuLine.ParameterType),
                    new XElement("fontName", MenuLine.FontName),
                    new XElement("fontSize", MenuLine.FontSize),
                    new XElement("fontBold", MenuLine.FontBold),
                    new XElement("foreColor", MenuLine.ForeColor),
                    new XElement("backColor", MenuLine.BackColor),
                    new XElement("fontItalic", MenuLine.FontItalic),
                    new XElement("fontCharset", MenuLine.FontCharset),
                    new XElement("disabled", MenuLine.Disabled),
                    new XElement("pictureID", MenuLine.PictureID),
                    new XElement("hideDescrOnPicture", MenuLine.HideDescrOnPicture),
                    new XElement("fontStrikethrough", MenuLine.FontStrikethrough),
                    new XElement("fontUnderline", MenuLine.FontUnderline),
                    new XElement("columnSpan", MenuLine.ColumnSpan),
                    new XElement("rowSpan", MenuLine.RowSpan),
                    new XElement("navOperation", MenuLine.NavOperation),
                    new XElement("hidden", MenuLine.Hidden),
                    new XElement("shadeWhenDisabled", MenuLine.ShadeWhenDisabled),
                    new XElement("backgroundHidden", MenuLine.BackgroundHidden),
                    new XElement("transparent", MenuLine.Transparent),
                    new XElement("glyph", (int)MenuLine.Glyph),
                    new XElement("glyph2", (int)MenuLine.Glyph2),
                    new XElement("glyph3", (int)MenuLine.Glyph3),
                    new XElement("glyph4", (int)MenuLine.Glyph4),
                    new XElement("glyphText", MenuLine.GlyphText),
                    new XElement("glyphText2", MenuLine.GlyphText2),
                    new XElement("glyphText3", MenuLine.GlyphText3),
                    new XElement("glyphText4", MenuLine.GlyphText4),
                    new XElement("glyphTextFont", MenuLine.GlyphTextFont),
                    new XElement("glyphText2Font", MenuLine.GlyphText2Font),
                    new XElement("glyphText3Font", MenuLine.GlyphText3Font),
                    new XElement("glyphText4Font", MenuLine.GlyphText4Font),
                    new XElement("glyphTextFontSize", MenuLine.GlyphTextFontSize),
                    new XElement("glyphText2FontSize", MenuLine.GlyphText2FontSize),
                    new XElement("glyphText3FontSize", MenuLine.GlyphText3FontSize),
                    new XElement("glyphText4FontSize", MenuLine.GlyphText4FontSize),
                    new XElement("glyphTextForeColor", MenuLine.GlyphTextForeColor),
                    new XElement("glyphText2ForeColor", MenuLine.GlyphText2ForeColor),
                    new XElement("glyphText3ForeColor", MenuLine.GlyphText3ForeColor),
                    new XElement("glyphText4ForeColor", MenuLine.GlyphText4ForeColor),
                    new XElement("glyphOffSet", MenuLine.GlyphOffSet),
                    new XElement("glyph2OffSet", MenuLine.Glyph2OffSet),
                    new XElement("glyph3OffSet", MenuLine.Glyph3OffSet),
                    new XElement("glyph4OffSet", MenuLine.Glyph4OffSet),
                    new XElement("backColor2", MenuLine.BackColor2),
                    new XElement("gradientMode", (int)MenuLine.GradientMode),
                    new XElement("shape", (int)MenuLine.Shape),
                    new XElement("useHeaderFont", MenuLine.UseHeaderFont),
                    new XElement("useHeaderAttributes", MenuLine.UseHeaderAttributes),
                    new XElement("imagePosition", (int)MenuLine.ImagePosition),
                    new XElement("textPosition", (int)MenuLine.TextPosition),
                    new XElement("keyMapping", (int)MenuLine.KeyMapping),
                    new XElement("posOperationName", MenuLine.PosOperationName),
                    new XElement("hospitalityOperationName", MenuLine.HospitalityOperationName),
                    new XElement("dirty", MenuLine.Dirty),
                    new XElement("useImageFont", MenuLine.UseImageFont),
                    new XElement("imageFontText", MenuLine.ImageFontText),
                    new XElement("imageFontName", MenuLine.ImageFontName),
                    new XElement("imageFontSize", MenuLine.ImageFontSize),
                    new XElement("imageFontBold", MenuLine.ImageFontBold),
                    new XElement("imageFontItalic", MenuLine.ImageFontItalic),
                    new XElement("imageFontUnderline", MenuLine.ImageFontUnderline),
                    new XElement("imageFontStrikethrough", MenuLine.ImageFontStrikethrough),
                    new XElement("imageFontCharset", MenuLine.ImageFontCharset),
                    new XElement("imageFontColor", MenuLine.ImageFontColor)
                    );
            }
            return xStoreSetting;
        }        
    }
}
