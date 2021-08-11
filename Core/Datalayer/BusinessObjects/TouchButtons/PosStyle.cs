#if !MONO
#endif
using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.IO;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Properties;
using LSOne.Utilities.GUI;
using System.Collections.Generic;
using LSOne.DataLayer.KDSBusinessObjects;

namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    /// <summary>
    /// A business object for a style setup
    /// </summary>
    [Serializable]
    public class PosStyle : DataEntity, ISerializable
    {
        /// <summary>
        /// Constructor for an existing Style
        /// </summary>
        /// <param name="id">The style ID</param>
        /// <param name="text">The description of the style</param>
        public PosStyle(RecordIdentifier id, string text)
           : this()
        {
            ID = id;
            Text = text;
        }

        /// <summary>
        /// Default contructor, sets all variables to their default value
        /// </summary>
        public PosStyle()
            : base()
        {
            Text = "";
            Columns = 0;
            Rows = 0;
            MenuColor = 0;
            FontName = "Tahoma";
            FontSize = 14;
            FontBold = false;
            FontStrikethrough = false;
#if !MONO
            ForeColor = Color.Black.ToArgb();
            BackColor = Color.White.ToArgb();
            BackColor2 = Color.White.ToArgb();
#endif
            FontItalic = false;
            FontCharset = 0;
            TextPosition = Position.Center;
            UseNavOperation = false;
            AppliesTo = PosMenuHeader.AppliesToEnum.None;
            GradientMode = GradientModeEnum.None;
            Shape = ShapeEnum.RoundRectangle;
            MenuType = MenuTypeEnum.Hospitality;
            ID = "";
            KitchenDisplay = false;

            Guid = Guid.Empty;
            ImportDateTime = null;
        }

        /// <summary>
        /// Receives style information in a serialized form and instantiates the style object
        /// </summary>
        /// <param name="info">The serialized style info</param>
        /// <param name="context">The source and destination of the serialized stream </param>
        public PosStyle(SerializationInfo info, StreamingContext context)
        {
            Columns = SerializationHelper.GetInt32(info, "Columns");
            Rows = SerializationHelper.GetInt32(info, "Rows");
            MenuColor = SerializationHelper.GetInt32(info, "MenuColor");
            FontName = SerializationHelper.GetString(info, "FontName");
            FontSize = SerializationHelper.GetInt32(info, "FontSize");
            FontBold = SerializationHelper.GetBool(info, "FontBold");
            ForeColor = SerializationHelper.GetInt32(info, "ForeColor");
            BackColor = SerializationHelper.GetInt32(info, "BackColor");
            FontItalic = SerializationHelper.GetBool(info, "FontItalic");
            FontCharset = SerializationHelper.GetInt32(info, "FontCharset");
            FontStrikethrough = SerializationHelper.GetBool(info, "FontStrikethrough");
            TextPosition = (Position)SerializationHelper.GetInt32(info, "TextPosition");
            UseNavOperation = SerializationHelper.GetBool(info, "UseNavOperation");
            AppliesTo = (PosMenuHeader.AppliesToEnum)SerializationHelper.GetInt32(info, "AppliesTo");
            BackColor2 = SerializationHelper.GetInt32(info, "BackColor2");
            GradientMode = (GradientModeEnum)SerializationHelper.GetInt32(info, "GradientMode");
            Shape = (ShapeEnum)SerializationHelper.GetInt32(info, "Shape");
            StyleType = (StyleType)SerializationHelper.GetInt32(info, "StyleType");
            MenuType = (MenuTypeEnum)SerializationHelper.GetInt32(info, "MenuType");
            ID = SerializationHelper.GetString(info, "ID");
            KitchenDisplay = SerializationHelper.GetBool(info, "KitchenDisplay");
        }

        /// <summary>
        /// Serializes the <see cref="PosStyle"/> object
        /// </summary>
        /// <param name="info">The serialized object</param>
        /// <param name="context">The source and destination of the serialized stream</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Columns", Columns);
            info.AddValue("Rows", Rows);
            info.AddValue("MenuColor", MenuColor);
            info.AddValue("FontName", FontName);
            info.AddValue("FontSize", FontSize);
            info.AddValue("FontBold", FontBold);
            info.AddValue("ForeColor", ForeColor);
            info.AddValue("BackColor", BackColor);
            info.AddValue("FontItalic", FontItalic);
            info.AddValue("FontCharset", Columns);
            info.AddValue("FontStrikethrough", FontStrikethrough);
            info.AddValue("TextPosition", TextPosition);
            info.AddValue("UseNavOperation", UseNavOperation);
            info.AddValue("AppliesTo", (int)AppliesTo);
            info.AddValue("BackColor2", BackColor2);
            info.AddValue("GradientMode", (int)GradientMode);
            info.AddValue("Shape", (int)Shape);
            info.AddValue("StyleType", StyleType);
            info.AddValue("MenuType", (int)MenuType);
            info.AddValue("ID", ID);
            info.AddValue("KitchenDisplay", KitchenDisplay);
        }

        /// <summary>
        /// Is this style a system style?
        /// </summary>
        public bool IsSystemStyle { get; set; }
        /// <summary>
        /// Number of button columns
        /// </summary>
        public int Columns { get; set; }
        /// <summary>
        /// Number of button rows
        /// </summary>
        public int Rows { get; set; }
        /// <summary>
        /// The color of the menu
        /// </summary>
        public int MenuColor { get; set; }
        /// <summary>
        /// The style font name
        /// </summary>
        public string FontName { get; set; }
        /// <summary>
        /// The style font size
        /// </summary>
        public int FontSize { get; set; }
        /// <summary>
        /// Is the style font in bold
        /// </summary>
        public bool FontBold { get; set; }
        /// <summary>
        /// The font color
        /// </summary>
        public int ForeColor { get; set; }
        /// <summary>
        /// The button color
        /// </summary>
        public int BackColor { get; set; }
        /// <summary>
        /// Is the style font in italics
        /// </summary>
        public bool FontItalic { get; set; }
        /// <summary>
        /// The character set of the font
        /// </summary>
        public int FontCharset { get; set; }
        /// <summary>
        /// Is the style font with strikthrough
        /// </summary>
        public bool FontStrikethrough { get; set; }
        /// <summary>
        /// Alignment of text
        /// </summary>
        public Position TextPosition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool UseNavOperation { get; set; }
        public PosMenuHeader.AppliesToEnum AppliesTo { get; set; }
        /// <summary>
        /// The second button color. Is used if <see cref="GradientModeEnum"/> is not None
        /// </summary>
        public int BackColor2 { get; set; }
        /// <summary>
        /// Sets the gradient mode of the button
        /// </summary>
        public GradientModeEnum GradientMode { get; set; }
        /// <summary>
        /// Sets the shape of the button
        /// </summary>
        public ShapeEnum Shape { get; set; }
        /// <summary>
        /// Style Type
        /// </summary>
        public StyleType StyleType { get; set; }
        public string StyleTypeString
        {
            get
            {
                switch (StyleType)
                {
                    case StyleType.Normal:
                        return Resources.StyleType_Normal;
                    case StyleType.DualDisplayTotal:
                        return Resources.StyleType_DualDisplayTotal;
                    case StyleType.DualDisplayLine:
                        return Resources.StyleType_DualDisplayLine;
                    case StyleType.DualDisplayLineSub:
                        return Resources.StyleType_DualDisplayLineSub;
                    case StyleType.PosTotalsLabels:
                        return Resources.StyleType_PosTotalsLabels;
                    case StyleType.PosTotalsValues:
                        return Resources.StyleType_PosTotalsValues;
                    case StyleType.PosTotalsBalanceValue:
                        return Resources.StyleType_PosTotalsBalanceValue;
                    case StyleType.SystemNumPadButtonStyle:
                        return Resources.StyleType_SystemNumPadButtonStyle;
                    case StyleType.SystemNumPadDigitButtonStyle:
                        return Resources.StyleType_SystemNumPadDigitButtonStyle;
                    case StyleType.PosTotalsBalanceLabel:
                        return Resources.StyleType_PosTotalsBalanceLabel;
                    case StyleType.PosTotalsBalanceLabelNegative:
                        return Resources.StyleType_PosTotalsBalanceLabelNegative;
                    case StyleType.PosTotalsBalanceValueNegative:
                        return Resources.StyleType_PosTotalsBalanceValueNegative;
                    case StyleType.PosReceiptSaleTotalLabel:
                        return Resources.StyleType_PosReceiptSaleTotalLabel;
                    case StyleType.PosReceiptSaleTotalLabelNegative:
                        return Resources.StyleType_PosReceiptSaleTotalLabelNegative;
                    case StyleType.PosReceiptSaleTotalValue:
                        return Resources.StyleType_PosReceiptSaleTotalValue;
                    case StyleType.PosReceiptSaleTotalValueNegative:
                        return Resources.StyleType_PosReceiptSaleTotalValueNegative;
                    case StyleType.PosReceiptLineSub:
                        return Resources.StyleType_PosReceiptLineSub;
                    case StyleType.DualDisplayTotalLabelNegative:
                        return Resources.StyleType_DualDisplayTotalLabelNegative;
                    case StyleType.DualDisplayTotalValue:
                        return Resources.StyleType_DualDisplayTotalValue;
                    case StyleType.DualDisplayTotalValueNegative:
                        return Resources.StyleType_DualDisplayTotalValueNegative;
                    case StyleType.DualDisplayTotalsBalanceLabel:
                        return Resources.StyleType_DualDisplayTotalsBalanceLabel;
                    case StyleType.DualDisplayTotalsBalanceLabelNegative:
                        return Resources.StyleType_DualDisplayTotalsBalanceLabelNegative;
                    case StyleType.DualDisplayTotalsBalanceValueNegative:
                        return Resources.StyleType_DualDisplayTotalsBalanceValueNegative;
                    case StyleType.DualDisplayTotalsLabels:
                        return Resources.StyleType_DualDisplayTotalsLabels;
                    case StyleType.DualDisplayTotalsValues:
                        return Resources.StyleType_DualDisplayTotalsValues;
                    case StyleType.DualDisplayTotalsBalanceValue:
                        return Resources.StyleType_DualDisplayTotalsBalanceValue;
                    case StyleType.SystemKeyboardButtonStyle:
                        return Resources.StyleType_SystemKeyboardByttonStyle;
                    case StyleType.PosReceiptSaleLineStyle:
                        return Resources.StyleType_PosReceiptSaleLineStyle;
                    default:
                        return string.Empty;
                }
            }
        }
        /// <summary>
        /// Sets the type of the menu (<see cref="MenuTypeEnum"/>)
        /// </summary>
        public MenuTypeEnum MenuType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool KitchenDisplay { get; set; }
        /// <summary>
        /// Guid for the style
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// Time and date of import or null if style wasn't imported
        /// </summary>
        public DateTime? ImportDateTime { get; set; }

        #region Enums
        public enum AppliesToEnum
        {
            None = 0,
            Window = 1,
            Table = 2,
            Guest = 3
        }
        #endregion

        public void CopyStyleTo(PosStyle destination)
        {
            destination.FontName = FontName;
            destination.FontSize = FontSize;
            destination.FontBold = FontBold;
            destination.FontItalic = FontItalic;
            destination.FontCharset = FontCharset;
            destination.FontStrikethrough = FontStrikethrough;
            destination.TextPosition = TextPosition;
            destination.ForeColor = ForeColor;
            destination.BackColor = BackColor;
            destination.BackColor2 = BackColor2;
            destination.GradientMode = GradientMode;
            destination.Shape = Shape;
            destination.StyleType = StyleType;
        }

        public Style ConvertToStyle()
        {
            var style = new Style()
            {
                FontName = FontName,
                FontSize = FontSize,
                FontBold = FontBold,
                FontItalic = FontItalic,
                FontCharset = FontCharset,
                ForeColor = Color.FromArgb(ForeColor),
                BackColor = Color.FromArgb(BackColor),
                BackColor2 = Color.FromArgb(BackColor2),
                GradientMode = GradientMode,
                Shape = Shape,
                TextPosition = TextPosition
            };

            return style;
        }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "text":
                                Text = current.Value;
                                break;
                            case "guid":
                                Guid = Guid.Parse(current.Value);
                                break;
                            case "id":
                                ID = current.Value;
                                break;
                            case "isSystemStyle":
                                IsSystemStyle = Convert.ToBoolean(current.Value);
                                break;
                            case "columns":
                                Columns = Convert.ToInt32(current.Value);
                                break;
                            case "rows":
                                Rows = Convert.ToInt32(current.Value);
                                break;
                            case "menuColor":
                                MenuColor = Convert.ToInt32(current.Value);
                                break;
                            case "fontName":
                                FontName = current.Value;
                                break;
                            case "fontSize":
                                FontSize = Convert.ToInt32(current.Value);
                                break;
                            case "fontBold":
                                FontBold = current.Value == "true";
                                break;
                            case "fontStrikethrough":
                                FontStrikethrough = current.Value == "true";
                                break;
                            case "foreColor":
                                ForeColor = Convert.ToInt32(current.Value);
                                break;
                            case "backColor":
                                BackColor = Convert.ToInt32(current.Value);
                                break;
                            case "backColor2":
                                BackColor2 = Convert.ToInt32(current.Value);
                                break;
                            case "fontItalic":
                                FontItalic = current.Value == "true";
                                break;
                            case "fontCharset":
                                FontCharset = Convert.ToInt32(current.Value);
                                break;
                            case "textPosition":
                                TextPosition = (Position)Convert.ToInt32(current.Value);
                                break;
                            case "useNavOperation":
                                UseNavOperation = current.Value == "true";
                                break;
                            case "appliesTo":
                                AppliesTo = (PosMenuHeader.AppliesToEnum)Convert.ToInt32(current.Value);
                                break;
                            case "gradientMode":
                                GradientMode = (GradientModeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "shape":
                                Shape = (ShapeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "styleType":
                                StyleType = (StyleType)Convert.ToInt32(current.Value);
                                break;
                            case "menuType":
                                MenuType = (MenuTypeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "kitchenDisplay":
                                KitchenDisplay = current.Value == "true";
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error,
                                                   current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("posStyle",
                                        new XElement("id", (string)ID),
                                        new XElement("guid", Guid.ToString()),
                                        new XElement("text", Text),
                                        new XElement("isSystemStyle", IsSystemStyle.ToString()),
                                        new XElement("columns", Columns),
                                        new XElement("rows", Rows),
                                        new XElement("menuColor", MenuColor),
                                        new XElement("fontName", FontName),
                                        new XElement("fontSize", FontSize),
                                        new XElement("fontBold", FontBold),
                                        new XElement("fontStrikethrough", FontStrikethrough),
                                        new XElement("foreColor", ForeColor),
                                        new XElement("backColor", BackColor),
                                        new XElement("backColor2", BackColor2),
                                        new XElement("fontItalic", FontItalic),
                                        new XElement("fontCharset", FontCharset),
                                        new XElement("textPosition", (int)TextPosition),
                                        new XElement("useNavOperation", UseNavOperation),
                                        new XElement("appliesTo", (int)AppliesTo),
                                        new XElement("gradientMode", (int)GradientMode),
                                        new XElement("shape", (int)Shape),
                                        new XElement("styleType", (int)StyleType),
                                        new XElement("menuType", (int)MenuType),
                                        new XElement("kitchenDisplay", KitchenDisplay));
            return xml;
        }

        public static Font FontFromStyle(PosStyle style)
        {
            if (style == null || string.IsNullOrEmpty(style.FontName))
                return null;

            var fontStyle = FontStyle.Regular;
            if (style.FontBold) { fontStyle = fontStyle | FontStyle.Bold; }
            if (style.FontItalic) { fontStyle = fontStyle | FontStyle.Italic; }
            if (style.FontStrikethrough) { fontStyle = fontStyle | FontStyle.Strikeout; }

            return new Font(
                style.FontName,
                style.FontSize == 0 ? (float)8.25 : (float)style.FontSize,
                fontStyle);
        }

        public static UIStyle ToUIStyle(PosStyle style)
        {
            if(style == null)
            {
                return new UIStyle();
            }

            UIStyle uiStyle = new UIStyle();
            uiStyle.ID = style.ID;
            uiStyle.Text = style.Text;
            uiStyle.Style = ToBaseStyle(style);
            uiStyle.Style.FontStyle = null;

            return uiStyle;
        }

        public static List<UIStyle> ToUIStyle(List<PosStyle> styles)
        {
            return styles.ConvertAll(style => ToUIStyle(style));
        }

        public static UIStyle ToKDSStyle(PosStyle style)
        {
            if (style == null)
            {
                return new UIStyle();
            }

            UIStyle kdsStyle = new UIStyle();
            kdsStyle.ID = style.ID;
            kdsStyle.Text = style.Text;
            kdsStyle.Style = ToBaseStyle(style);
            kdsStyle.Style.Height = kdsStyle.Style.Font.Height;
            kdsStyle.Style.FontStyle = null;

            return kdsStyle;
        }

        public static List<UIStyle> ToKDSStyle(List<PosStyle> styles)
        {
            return styles.ConvertAll(style => ToKDSStyle(style));
        }

        public static BaseStyle ToBaseStyle(PosStyle style)
        {
            BaseStyle baseStyle = new BaseStyle();
            baseStyle.BackColor = Color.FromArgb(style.BackColor);
            baseStyle.BackColor2 = Color.FromArgb(style.BackColor2);
            baseStyle.ForeColor = Color.FromArgb(style.ForeColor);
            baseStyle.GradientMode = style.GradientMode;
            baseStyle.Shape = style.Shape;
            baseStyle.Font = FontFromStyle(style);
            baseStyle.FontStyle = FontStyle.Regular;
            if (style.FontBold) { baseStyle.FontStyle = FontStyle.Bold; }
            if (style.FontItalic) { baseStyle.FontStyle = FontStyle.Italic; }
            if (style.FontStrikethrough) { baseStyle.FontStyle = FontStyle.Strikeout; }

            return baseStyle;
        }
    }
}