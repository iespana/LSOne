using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml.Linq;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.GUI;

namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    [DataContract]
    public class PosMenuLine : DataEntity
    {
        [DataMember]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(MenuID, Sequence);
            }
            set
            {
                base.ID = value;
            }
        }        

        public PosMenuLine()
            : base()
        {
            MenuID = RecordIdentifier.Empty;
            Sequence = RecordIdentifier.Empty;
            KeyNo = 0;
            Text = "";
            Operation = 0;
            Parameter = "";
            ParameterType = ParameterTypeEnum.None;
            FontName = "Segoe UI";
            FontSize = 14;
            FontBold = false;
            ForeColor = ColorPalette.Black.ToArgb();
            BackColor = ColorPalette.White.ToArgb();
            FontItalic = false;
            FontCharset = 0;
            Disabled = false;
            TextPosition = Position.Center;
            PictureID = RecordIdentifier.Empty;
            HideDescrOnPicture = false;
            FontStrikethrough = false;
            FontUnderline = false;
            ColumnSpan = 1;
            RowSpan = 1;
            NavOperation = "";
            Hidden = false;
            ShadeWhenDisabled = false;
            BackgroundHidden = false;
            Transparent = false;
            Glyph = GlyphEnum.None;
            Glyph2 = GlyphEnum.None;
            Glyph3 = GlyphEnum.None;
            Glyph4 = GlyphEnum.None;
            GlyphText = "";
            GlyphText2 = "";
            GlyphText3 = "";
            GlyphText4 = "";
            GlyphTextFont = "";
            GlyphText2Font = "";
            GlyphText3Font = "";
            GlyphText4Font = "";
            GlyphTextFontSize = 5;
            GlyphText2FontSize = 5;
            GlyphText3FontSize = 5;
            GlyphText4FontSize = 5;
            GlyphTextForeColor = ColorPalette.Black.ToArgb();
            GlyphText2ForeColor = ColorPalette.Black.ToArgb();
            GlyphText3ForeColor = ColorPalette.Black.ToArgb();
            GlyphText4ForeColor = ColorPalette.Black.ToArgb();
            GlyphOffSet = 1;
            Glyph2OffSet = 1;
            Glyph3OffSet = 1;
            Glyph4OffSet = 1;
            BackColor2 = ColorPalette.White.ToArgb();
            GradientMode = GradientModeEnum.None;
            Shape = ShapeEnum.RoundRectangle;
            UseHeaderFont = true;
            UseHeaderAttributes = true;
            ImagePosition = Position.Center;
            PosOperationName = "";
            HospitalityOperationName = "";
            StyleID = RecordIdentifier.Empty;
            ImageFontText = "";
            ImageFontName = "Segoe UI";
            ImageFontSize = 14;
            ImageFontCharset = 0;
            ImageFontColor = ColorPalette.Black.ToArgb();
            ParameterItemID = "";
        }

        [DataMember]
        public RecordIdentifier MenuID { get; set; }
        [DataMember]
        public RecordIdentifier Sequence { get; set; }
        [DataMember]
        public int KeyNo { get; set; }
        [DataMember]
        [RecordIdentifierConstruction(typeof(int))]
        public RecordIdentifier Operation { get; set; }
        [DataMember]
        public string Parameter { get; set; }
        [DataMember]
        public ParameterTypeEnum ParameterType { get; set; }
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
        public bool Disabled { get; set; }
        [DataMember]
        public RecordIdentifier PictureID { get; set; }
        [DataMember]
        public bool HideDescrOnPicture { get; set; }
        [DataMember]
        public bool FontStrikethrough { get; set; }
        [DataMember]
        public bool FontUnderline { get; set; }
        [DataMember]
        public int ColumnSpan { get; set; }
        [DataMember]
        public int RowSpan { get; set; }
        [DataMember]
        public string NavOperation { get; set; }
        [DataMember]
        public bool Hidden { get; set; }
        [DataMember]
        public bool ShadeWhenDisabled { get; set; }
        [DataMember]
        public bool BackgroundHidden { get; set; }
        [DataMember]
        public bool Transparent { get; set; }
        [DataMember]
        public GlyphEnum Glyph { get; set; }
        [DataMember]
        public GlyphEnum Glyph2 { get; set; }
        [DataMember]
        public GlyphEnum Glyph3 { get; set; }
        [DataMember]
        public GlyphEnum Glyph4 { get; set; }
        [DataMember]
        public string GlyphText { get; set; }
        [DataMember]
        public string GlyphText2 { get; set; }
        [DataMember]
        public string GlyphText3 { get; set; }
        [DataMember]
        public string GlyphText4 { get; set; }
        [DataMember]
        public string GlyphTextFont { get; set; }
        [DataMember]
        public string GlyphText2Font { get; set; }
        [DataMember]
        public string GlyphText3Font { get; set; }
        [DataMember]
        public string GlyphText4Font { get; set; }
        [DataMember]
        public int GlyphTextFontSize { get; set; }
        [DataMember]
        public int GlyphText2FontSize { get; set; }
        [DataMember]
        public int GlyphText3FontSize { get; set; }
        [DataMember]
        public int GlyphText4FontSize { get; set; }
        [DataMember]
        public int GlyphTextForeColor { get; set; }
        [DataMember]
        public int GlyphText2ForeColor { get; set; }
        [DataMember]
        public int GlyphText3ForeColor { get; set; }
        [DataMember]
        public int GlyphText4ForeColor { get; set; }
        [DataMember]
        public int GlyphOffSet { get; set; }
        [DataMember]
        public int Glyph2OffSet { get; set; }
        [DataMember]
        public int Glyph3OffSet { get; set; }
        [DataMember]
        public int Glyph4OffSet { get; set; }
        [DataMember]
        public int BackColor2 { get; set; }
        [DataMember]
        public GradientModeEnum GradientMode { get; set; }
        [DataMember]
        public ShapeEnum Shape { get; set; }
        [DataMember]
        public bool UseHeaderFont { get; set; }
        [DataMember]
        public bool UseHeaderAttributes { get; set; }
        [DataMember]
        public Position ImagePosition { get; set; }
        [DataMember]
        public RecordIdentifier StyleID { get; set; }
        [DataMember]
        public Keys KeyMapping { get; set; }
        [DataMember]
        public bool UseImageFont { get; set; }
        [DataMember]
        public string ImageFontText { get; set; }
        [DataMember]
        public string ImageFontName { get; set; }
        [DataMember]
        public int ImageFontSize { get; set; }
        [DataMember]
        public bool ImageFontBold { get; set; }
        [DataMember]
        public bool ImageFontItalic { get; set; }
        [DataMember]
        public bool ImageFontUnderline { get; set; }
        [DataMember]
        public bool ImageFontStrikethrough { get; set; }
        [DataMember]
        public int ImageFontCharset { get; set; }
        [DataMember]
        public int ImageFontColor { get; set; }

        /// <summary>
        /// Contains the name of the pos operation name
        /// </summary>
        [DataMember]
        public string PosOperationName { get; set; }

        /// <summary>
        /// Contains the name of the hospitality operation name
        /// </summary>
        [DataMember]
        public string HospitalityOperationName { get; set; }

        /// <summary>
        /// Indicates if the data in this data entity is dirty
        /// </summary>
        [DataMember]
        public bool Dirty { get; set; }

        //TODO: KDS add read/write to data provider
        [DataMember]
        public int ChitCellNoToBump { get; set; }

        /// <summary>
        /// Get/set the parameter item id corresponding to a Diary item
        /// </summary>
        [DataMember]
        public string ParameterItemID { get; set; }


        #region Enums
        public enum GlyphEnum
        {
            None = 0,
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Eleven = 11,
            Twelve = 12,
            F1 = 13,
            F2 = 14,
            F3 = 15,
            F4 = 16,
            F5 = 17,
            F6 = 18,
            F7 = 19,
            F8 = 20,
            F9 = 21,
            F10 = 22,
            F11 = 23,
            F12 = 24,
            Text = 25            
        }              


        public enum ParameterTypeEnum
        {
            None = 0,
            SubMenu = 1
        }

        #endregion

        public static PosMenuLine Clone(PosMenuLine lineToCopy)
        {
            PosMenuLine newLine = new PosMenuLine();

            newLine.MenuID = lineToCopy.MenuID;
            newLine.Sequence = lineToCopy.Sequence;
            newLine.KeyNo = lineToCopy.KeyNo;
            newLine.Text = lineToCopy.Text;
            newLine.Operation = lineToCopy.Operation;
            newLine.Parameter = lineToCopy.Parameter;
            newLine.ParameterType = lineToCopy.ParameterType;
            newLine.FontName = lineToCopy.FontName == "" ? "Segoe UI" : lineToCopy.FontName;
            newLine.FontSize = lineToCopy.FontSize == 0 ? 14 : lineToCopy.FontSize;
            newLine.FontBold = lineToCopy.FontBold;
            newLine.ForeColor = lineToCopy.ForeColor;
            newLine.BackColor = lineToCopy.BackColor;
            newLine.FontItalic = lineToCopy.FontItalic;
            newLine.FontCharset = lineToCopy.FontCharset;
            newLine.Disabled = lineToCopy.Disabled;
            newLine.PictureID = lineToCopy.PictureID;
            newLine.HideDescrOnPicture = lineToCopy.HideDescrOnPicture;
            newLine.FontStrikethrough = lineToCopy.FontStrikethrough;
            newLine.FontUnderline = lineToCopy.FontUnderline;
            newLine.ColumnSpan = lineToCopy.ColumnSpan;
            newLine.RowSpan = lineToCopy.RowSpan;
            newLine.NavOperation = lineToCopy.NavOperation;
            newLine.Hidden = lineToCopy.Hidden;
            newLine.ShadeWhenDisabled = lineToCopy.ShadeWhenDisabled;
            newLine.BackgroundHidden = lineToCopy.BackgroundHidden;
            newLine.Transparent = lineToCopy.Transparent;
            newLine.Glyph = lineToCopy.Glyph;
            newLine.Glyph2 = lineToCopy.Glyph2;
            newLine.Glyph3 = lineToCopy.Glyph3;
            newLine.Glyph4 = lineToCopy.Glyph4;
            newLine.GlyphText = lineToCopy.GlyphText;
            newLine.GlyphText2 = lineToCopy.GlyphText2;
            newLine.GlyphText3 = lineToCopy.GlyphText3;
            newLine.GlyphText4 = lineToCopy.GlyphText4;
            newLine.GlyphTextFont = lineToCopy.GlyphTextFont;
            newLine.GlyphText2Font = lineToCopy.GlyphText2Font;
            newLine.GlyphText3Font = lineToCopy.GlyphText3Font;
            newLine.GlyphText4Font = lineToCopy.GlyphText4Font;
            newLine.GlyphTextFontSize = lineToCopy.GlyphTextFontSize;
            newLine.GlyphText2FontSize = lineToCopy.GlyphText2FontSize;
            newLine.GlyphText3FontSize = lineToCopy.GlyphText3FontSize;
            newLine.GlyphText4FontSize = lineToCopy.GlyphText4FontSize;
            newLine.GlyphTextForeColor = lineToCopy.GlyphTextForeColor;
            newLine.GlyphText2ForeColor = lineToCopy.GlyphText2ForeColor;
            newLine.GlyphText3ForeColor = lineToCopy.GlyphText3ForeColor;
            newLine.GlyphText4ForeColor = lineToCopy.GlyphText4ForeColor;
            newLine.GlyphOffSet = lineToCopy.GlyphOffSet;
            newLine.Glyph2OffSet = lineToCopy.Glyph2OffSet;
            newLine.Glyph3OffSet = lineToCopy.Glyph3OffSet;
            newLine.Glyph4OffSet = lineToCopy.Glyph4OffSet;
            newLine.BackColor2 = lineToCopy.BackColor2;
            newLine.GradientMode = lineToCopy.GradientMode;
            newLine.Shape = lineToCopy.Shape;
            newLine.UseHeaderAttributes = lineToCopy.UseHeaderAttributes;
            newLine.UseHeaderFont = lineToCopy.UseHeaderFont;
            newLine.ImagePosition = lineToCopy.ImagePosition;
            newLine.TextPosition = lineToCopy.TextPosition;
            newLine.StyleID = lineToCopy.StyleID;
            newLine.UseImageFont = lineToCopy.UseImageFont;
            newLine.ImageFontText = lineToCopy.ImageFontText;
            newLine.ImageFontName = lineToCopy.ImageFontName == "" ? "Segoe UI" : lineToCopy.ImageFontName;
            newLine.ImageFontSize = lineToCopy.ImageFontSize == 0 ? 14 : lineToCopy.ImageFontSize;
            newLine.ImageFontBold = lineToCopy.ImageFontBold;
            newLine.ImageFontItalic = lineToCopy.ImageFontItalic;
            newLine.ImageFontUnderline = lineToCopy.ImageFontUnderline;
            newLine.ImageFontStrikethrough = lineToCopy.ImageFontStrikethrough;
            newLine.ImageFontCharset = lineToCopy.ImageFontCharset;
            newLine.ImageFontColor = lineToCopy.ImageFontColor;
            newLine.ParameterItemID = lineToCopy.ParameterItemID;
            return newLine;
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
                            case "menuID":
                                MenuID = current.Value;
                                break;
                            case "sequence":
                                Sequence = current.Value;
                                break;
                            case "keyNo":
                                KeyNo = Convert.ToInt32(current.Value);
                                break;
                            case "operation":
                                Operation = Convert.ToInt32(current.Value);
                                break;
                            case "parameter":
                                Parameter = current.Value;
                                break;
                            case "parameterType":
                                ParameterType = (ParameterTypeEnum)Convert.ToInt32(current.Value);
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
                            case "fontItalic":
                                FontItalic = current.Value != "false";
                                break;
                            case "fontCharset":
                                FontCharset = Convert.ToInt32(current.Value);
                                break;
                            case "disabled":
                                Disabled = current.Value != "false";
                                break;
                            case "pictureID":
                                PictureID = current.Value;
                                break;
                            case "hideDescrOnPicture":
                                HideDescrOnPicture = current.Value != "false";
                                break;
                            case "fontStrikethrough":
                                FontStrikethrough = current.Value != "false";
                                break;
                            case "fontUnderline":
                                FontUnderline = current.Value != "false";
                                break;
                            case "columnSpan":
                                ColumnSpan = Convert.ToInt32(current.Value);
                                break;
                            case "rowSpan":
                                RowSpan = Convert.ToInt32(current.Value);
                                break;
                            case "navOperation":
                                NavOperation = current.Value;
                                break;
                            case "hidden":
                                Hidden = current.Value != "false";
                                break;
                            case "shadeWhenDisabled":
                                ShadeWhenDisabled = current.Value != "false";
                                break;
                            case "backgroundHidden":
                                BackgroundHidden = current.Value != "false";
                                break;
                            case "transparent":
                                Transparent = current.Value != "false";
                                break;
                            case "glyph":
                                Glyph = (GlyphEnum)Convert.ToInt32(current.Value);
                                break;
                            case "glyph2":
                                Glyph2 = (GlyphEnum)Convert.ToInt32(current.Value);
                                break;
                            case "glyph3":
                                Glyph3 = (GlyphEnum)Convert.ToInt32(current.Value);
                                break;
                            case "glyph4":
                                Glyph4 = (GlyphEnum)Convert.ToInt32(current.Value);
                                break;
                            case "glyphText":
                                GlyphText = current.Value;
                                break;
                            case "glyphText2":
                                GlyphText2 = current.Value;
                                break;
                            case "glyphText3":
                                GlyphText3 = current.Value;
                                break;
                            case "glyphText4":
                                GlyphText4 = current.Value;
                                break;
                            case "glyphTextFont":
                                GlyphTextFont = current.Value;
                                break;
                            case "glyphText2Font":
                                GlyphText2Font = current.Value;
                                break;
                            case "glyphText3Font":
                                GlyphText3Font = current.Value;
                                break;
                            case "glyphText4Font":
                                GlyphText4Font = current.Value;
                                break;
                            case "glyphTextForeColor":
                                GlyphTextForeColor = Convert.ToInt32(current.Value);
                                break;
                            case "glyphText2ForeColor":
                                GlyphText2ForeColor = Convert.ToInt32(current.Value);
                                break;
                            case "glyphText3ForeColor":
                                GlyphText3ForeColor = Convert.ToInt32(current.Value);
                                break;
                            case "glyphText4ForeColor":
                                GlyphText4ForeColor = Convert.ToInt32(current.Value);
                                break;
                            case "glyphOffSet":
                                GlyphOffSet = Convert.ToInt32(current.Value);
                                break;
                            case "glyph2OffSet":
                                Glyph2OffSet = Convert.ToInt32(current.Value);
                                break;
                            case "glyph3OffSet":
                                Glyph3OffSet = Convert.ToInt32(current.Value);
                                break;
                            case "glyph4OffSet":
                                Glyph4OffSet = Convert.ToInt32(current.Value);
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
                            case "useHeaderFont":
                                UseHeaderFont = current.Value != "false";
                                break;
                            case "useHeaderAttributes":
                                UseHeaderAttributes = current.Value != "false";
                                break;
                            case "imagePosition":
                                ImagePosition = (Position)Convert.ToInt32(current.Value);
                                break;
                            case "textPosition":
                                TextPosition = (Position)Convert.ToInt32(current.Value);
                                break;
                            case "styleID":
                                StyleID = new RecordIdentifier(current.Value);
                                break;
                            case "keyMapping":
                                KeyMapping = (Keys)Convert.ToInt32(current.Value);
                                break;
                            case "posOperationName":
                                PosOperationName = current.Value;
                                break;
                            case "hospitalityOperationName":
                                HospitalityOperationName = current.Value;
                                break;
                            case "dirty":
                                Dirty = current.Value != "false";
                                break;
                            case "glyphTextFontSize" :
                                GlyphTextFontSize = Convert.ToInt32(current.Value);
                                break;
                            case "glyphText2FontSize" :
                                GlyphText2FontSize = Convert.ToInt32(current.Value);
                                break;
                            case "glyphText3FontSize" :
                                GlyphText3FontSize = Convert.ToInt32(current.Value);
                                break;
                            case "glyphText4FontSize" :
                                GlyphText4FontSize = Convert.ToInt32(current.Value);
                                break;
                            case "useImageFont":
                                UseImageFont = current.Value != "false";
                                break;
                            case "imageFontText":
                                ImageFontText = current.Value;
                                break;
                            case "imageFontName":
                                ImageFontName = current.Value;
                                break;
                            case "imageFontSize":
                                ImageFontSize = Convert.ToInt32(current.Value);
                                break;
                            case "imageFontBold":
                                ImageFontBold = current.Value != "false";
                                break;
                            case "imageFontItalic":
                                ImageFontItalic = current.Value != "false";
                                break;
                            case "imageFontUnderline":
                                ImageFontUnderline = current.Value != "false";
                                break;
                            case "imageFontStrikethrough":
                                ImageFontStrikethrough = current.Value != "false";
                                break;
                            case "imageFontCharset":
                                ImageFontCharset = Convert.ToInt32(current.Value);
                                break;
                            case "imageFontColor":
                                ImageFontColor = Convert.ToInt32(current.Value);
                                break;
                            case "parameterItemID":
                                ParameterItemID = current.Value;
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
            XElement xml = new XElement("posMenuLine",
                    new XElement("text", Text),
                    new XElement("menuID", (string)MenuID),
                    new XElement("sequence", (string)Sequence),
                    new XElement("keyNo", KeyNo),
                    new XElement("operation", (int)Operation),
                    new XElement("parameter", Parameter),
                    new XElement("parameterType", (int)ParameterType),
                    new XElement("fontName", FontName),
                    new XElement("fontSize", FontSize),
                    new XElement("fontBold", FontBold),
                    new XElement("foreColor", ForeColor),
                    new XElement("backColor", BackColor),
                    new XElement("fontItalic", FontItalic),
                    new XElement("fontCharset", FontCharset),
                    new XElement("disabled", Disabled),
                    new XElement("pictureID", PictureID),
                    new XElement("hideDescrOnPicture", HideDescrOnPicture),
                    new XElement("fontStrikethrough", FontStrikethrough),
                    new XElement("fontUnderline", FontUnderline),
                    new XElement("columnSpan", ColumnSpan),
                    new XElement("rowSpan", RowSpan),
                    new XElement("navOperation", NavOperation),
                    new XElement("hidden", Hidden),
                    new XElement("shadeWhenDisabled", ShadeWhenDisabled),
                    new XElement("backgroundHidden", BackgroundHidden),
                    new XElement("transparent", Transparent),
                    new XElement("glyph", (int)Glyph),
                    new XElement("glyph2", (int)Glyph2),
                    new XElement("glyph3", (int)Glyph3),
                    new XElement("glyph4", (int)Glyph4),
                    new XElement("glyphText", GlyphText),
                    new XElement("glyphText2", GlyphText2),
                    new XElement("glyphText3", GlyphText3),
                    new XElement("glyphText4", GlyphText4),
                    new XElement("glyphTextFont", GlyphTextFont),
                    new XElement("glyphText2Font", GlyphText2Font),
                    new XElement("glyphText3Font", GlyphText3Font),
                    new XElement("glyphText4Font", GlyphText4Font),
                    new XElement("glyphTextFontSize", GlyphTextFontSize),
                    new XElement("glyphText2FontSize", GlyphText2FontSize),
                    new XElement("glyphText3FontSize", GlyphText3FontSize),
                    new XElement("glyphText4FontSize", GlyphText4FontSize),
                    new XElement("glyphTextForeColor", GlyphTextForeColor),
                    new XElement("glyphText2ForeColor", GlyphText2ForeColor),
                    new XElement("glyphText3ForeColor", GlyphText3ForeColor),
                    new XElement("glyphText4ForeColor", GlyphText4ForeColor),
                    new XElement("glyphOffSet", GlyphOffSet),
                    new XElement("glyph2OffSet", Glyph2OffSet),
                    new XElement("glyph3OffSet", Glyph3OffSet),
                    new XElement("glyph4OffSet", Glyph4OffSet),
                    new XElement("backColor2", BackColor2),
                    new XElement("gradientMode", (int)GradientMode),
                    new XElement("shape", (int)Shape),
                    new XElement("useHeaderFont", UseHeaderFont),
                    new XElement("useHeaderAttributes", UseHeaderAttributes),
                    new XElement("imagePosition", (int)ImagePosition),
                    new XElement("textPosition", (int)TextPosition),
                    new XElement("styleID", StyleID.ToString()),
                    new XElement("keyMapping", (int)KeyMapping),
                    new XElement("posOperationName", PosOperationName),
                    new XElement("hospitalityOperationName", HospitalityOperationName),
                    new XElement("dirty", Dirty),
                    new XElement("useImageFont", UseImageFont),
                    new XElement("imageFontText", ImageFontText),
                    new XElement("imageFontName", ImageFontName),
                    new XElement("imageFontSize", ImageFontSize),
                    new XElement("imageFontBold", ImageFontBold),
                    new XElement("imageFontItalic", ImageFontItalic),
                    new XElement("imageFontUnderline", ImageFontUnderline),
                    new XElement("imageFontStrikethrough", ImageFontStrikethrough),
                    new XElement("imageFontCharset", ImageFontCharset),
                    new XElement("imageFontColor", ImageFontColor),
                    new XElement("parameterItemID", ParameterItemID));
            return xml;
        }
    }
}
