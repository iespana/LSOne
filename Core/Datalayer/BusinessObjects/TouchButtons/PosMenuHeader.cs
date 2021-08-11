using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.ErrorHandling;
#if !MONO

#endif

namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    [DataContract]
    public class PosMenuHeader : DataEntity
    {

        public PosMenuHeader()
            : base()
        {
            Text = "";
            Columns = 0;
            Rows = 0;
            MenuColor = 0;
            FontName = "Segoe UI";
            FontSize = 14;
            FontBold = false;
#if !MONO
            ForeColor = Color.Black.ToArgb();
            BackColor = Color.White.ToArgb();
            BackColor2 = Color.White.ToArgb();
            BorderColor = ColorPalette.POSControlBorderColor.ToArgb();
#endif
            BorderWidth = 1;
            Margin = 0;
            TextPosition = Position.Center;
            FontItalic = false;
            FontCharset = 0;
            UseNavOperation = false;
            AppliesTo = AppliesToEnum.None;            
            GradientMode = GradientModeEnum.None;
            Shape = ShapeEnum.RoundRectangle;
            MenuType = MenuTypeEnum.Hospitality;
            StyleID = RecordIdentifier.Empty;
            KitchenDisplay = false;
            DefaultOperation = RecordIdentifier.Empty;
            Guid = Guid.Empty;
            ImportDateTime = null;
            MainMenu = false;
            DeviceType = DeviceTypeEnum.POS;
        }

        [DataMember]
        public int Columns { get; set; }
        [DataMember]
        public int Rows { get; set; }
        [DataMember]
        public int MenuColor { get; set; }
        [DataMember]
        public string FontName { get; set; }
        [DataMember]
        public int FontSize { get; set; }
        [DataMember]
        public bool FontBold { get; set; }
        [DataMember]
        public int ForeColor { get; set; }
        [DataMember]
        public int BackColor { get; set; }
        [DataMember]
        public bool FontItalic { get; set; }
        [DataMember]
        public int FontCharset { get; set; }
        [DataMember]
        public Position TextPosition { get; set; }
        [DataMember]
        public int BorderColor { get; set; }
        [DataMember]
        public int BorderWidth { get; set; }
        [DataMember]
        public int Margin { get; set; }
        [DataMember]
        public bool UseNavOperation { get; set; }
        [DataMember]
        public AppliesToEnum AppliesTo { get; set; }
        [DataMember]
        public int BackColor2 { get; set; }
        [DataMember]
        public GradientModeEnum GradientMode { get; set; }
        [DataMember]
        public ShapeEnum Shape { get; set; }
        [DataMember]
        public MenuTypeEnum MenuType { get; set; }
        [DataMember]
        public RecordIdentifier StyleID { get; set; }
        [DataMember]
        public bool KitchenDisplay { get; set; }
        [DataMember]
        public RecordIdentifier DefaultOperation { get; set; }
        [DataMember]
        public Guid Guid { get; set; }
        /// <summary>
        /// The time and date of import or null if header wasn't imported
        /// </summary>
        [DataMember]
        public DateTime? ImportDateTime { get; set; }
        [DataMember]
        public bool MainMenu { get; set; }
        [DataMember]
        public DeviceTypeEnum DeviceType { get; set; }

        #region Enums
        public enum AppliesToEnum
        {
            None = 0,
            Window = 1,
            Table = 2,
            Guest = 3
        }        
        #endregion

        public override object Clone()
        {
            PosMenuHeader item = new PosMenuHeader();
            Populate(item);
            return item;
        }

        protected virtual void Populate(PosMenuHeader item)
        {
            item.ID = (RecordIdentifier)ID.Clone();
            item.Guid = Guid;
            item.Text = Text;
            item.Columns = Columns;
            item.Rows = Rows;
            item.MenuColor = MenuColor;
            item.FontName = FontName;
            item.FontSize = FontSize;
            item.FontBold = FontBold;
            item.ForeColor = ForeColor;
            item.BackColor = BackColor;
            item.BackColor2 = BackColor2;
            item.BorderColor = BorderColor;
            item.BorderWidth = BorderWidth;
            item.Margin = Margin;
            item.TextPosition = TextPosition;
            item.FontItalic = FontItalic;
            item.FontCharset = FontCharset;
            item.UseNavOperation = UseNavOperation;
            item.AppliesTo = AppliesTo;
            item.GradientMode = GradientMode;
            item.Shape = Shape;
            item.MenuType = MenuType;
            item.StyleID = (RecordIdentifier)StyleID.Clone();
            item.KitchenDisplay = KitchenDisplay;
            item.DefaultOperation = (RecordIdentifier)DefaultOperation.Clone();
            item.MainMenu = MainMenu;
            item.DeviceType = DeviceType;
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
                            case "menuID":
                                ID = current.Value;
                                break;
                            case "guid":
                                Guid = Guid.Parse(current.Value);
                                break;
                            case "description":
                                Text = current.Value;
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
                                FontBold = current.Value != "false";
                                break;
                            case "foreColor":
                                ForeColor = Convert.ToInt32(current.Value);
                                break;
                            case "backColor":
                                BackColor = Convert.ToInt32(current.Value);
                                break;
                            case "borderColor":
                                BorderColor = Convert.ToInt32(current.Value);
                                break;
                            case "borderWidth":
                                BorderWidth = Convert.ToInt32(current.Value);
                                break;
                            case "margin":
                                Margin = Convert.ToInt32(current.Value);
                                break;
                            case "textPosition":
                                TextPosition = (Position)Convert.ToInt32(current.Value);
                                break;
                            case "fontItalic":
                                FontItalic = current.Value != "false";
                                break;
                            case "fontCharset":
                                FontCharset = Convert.ToInt32(current.Value);
                                break;
                            case "useNavOperation":
                                UseNavOperation = current.Value != "false";
                                break;
                            case "appliesTo":
                                AppliesTo = (AppliesToEnum)Convert.ToInt32(current.Value);
                                break;
                            case "backColor2":
                                BackColor2 = Convert.ToInt32(current.Value);
                                break;
                            case "gradientMode":
                                GradientMode = (GradientModeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "shape":
                                Shape = (ShapeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "menuType":
                                MenuType = (MenuTypeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "styleID":
                                StyleID = current.Value;
                                break;
                            case "deviceType":
                                DeviceType = (DeviceTypeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "mainMenu":
                                MainMenu = current.Value != "false";
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("posMenuHeader",
                    new XElement("menuID", (string)ID),
                    new XElement("guid", Guid),
                    new XElement("description", Text),
                    new XElement("columns", Columns),
                    new XElement("rows", Rows),
                    new XElement("menuColor", MenuColor),
                    new XElement("fontName", FontName),
                    new XElement("fontSize", FontSize),
                    new XElement("fontBold", FontBold),
                    new XElement("foreColor", ForeColor),
                    new XElement("backColor", BackColor),
                    new XElement("borderColor", BorderColor),
                    new XElement("borderWidth", BorderWidth),
                    new XElement("margin", Margin),
                    new XElement("textPosition", TextPosition),
                    new XElement("fontItalic", FontItalic),
                    new XElement("fontCharset", FontCharset),
                    new XElement("useNavOperation", UseNavOperation),
                    new XElement("appliesTo", (int)AppliesTo),
                    new XElement("backColor2", BackColor2),
                    new XElement("gradientMode", (int)GradientMode),
                    new XElement("shape", (int)Shape),
                    new XElement("menuType", (int)MenuType),
                    new XElement("styleID", (string)StyleID),
                    new XElement("deviceType", (int)DeviceType),
                    new XElement("mainMenu", MainMenu));
            return xml;
        }
    }
}
