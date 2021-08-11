using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Services.Datalayer.BusinessObjects.Dimensions
{
    /// <summary>
    /// Each dimension combination has both a unique dimension ID and a variant number which is used for internal processing in the POS
    /// </summary>
    public class OldDimension : OldDimensionCombination, ICloneable
    {
        RecordIdentifier variantNumber;
        RecordIdentifier dimensionID;

        /// <summary>
        /// Initializes a new instance of the <see cref="OldDimension"/> class.
        /// </summary>
        public OldDimension()
            : base()
        {
            variantNumber = new RecordIdentifier("");
            dimensionID = "";
        }

        /// <summary>
        /// A unique ID for the dimension used in the internal processes of the LS POS
        /// </summary>
        public RecordIdentifier VariantNumber
        {
            get { return variantNumber; }
            set { variantNumber = value; }
        }

        /// <summary>
        /// The unique ID for the dimension
        /// </summary>
        public RecordIdentifier DimensionID
        {
            get { return dimensionID; }
            set { dimensionID = value; }
        }

        public bool EnterDimensions { get; set; }

        public override string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return (string)variantNumber;

                    default:
                        return base[index];
                }

                
            }
        }

        /// <summary>
        /// Returns a string that includes all the dimensions selected
        /// </summary>
        public string FormattedVariantDescription
        {
            get
            {
                string variantDescription = "";

                if (ColorName == "" && SizeName == "" && StyleName == "") return variantDescription;

                if (ColorName == "")
                {
                    if (SizeName == "")
                    {
                        variantDescription = Properties.Resources.Style + ": " + StyleName;
                    }
                    else if (StyleName == "")
                    {
                        variantDescription = Properties.Resources.Size + ": " + SizeName;
                    }
                    else
                    {
                        variantDescription = Properties.Resources.Size + ": " + SizeName + " - " + Properties.Resources.Style + ": " + StyleName;
                    }
                }
                else
                {
                    variantDescription += Properties.Resources.Color + ": " + ColorName;

                    if (SizeName != "")
                    {
                        variantDescription += " - " + Properties.Resources.Size + ": " + SizeName;
                    }

                    if (StyleName != "")
                    {
                        variantDescription += " - " +Properties.Resources.Style + ": " + StyleName;
                    }
                }
                return variantDescription;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string Description
        {
            get
            {
                string colorSizeStyle = "";
                if (variantNumber != "" && variantNumber != null)
                {
                    if (Conversion.ToStr(ColorName) != "")
                    {
                        colorSizeStyle = ColorName;
                    }
                    if (Conversion.ToStr(SizeName) != "")
                    {
                        if (colorSizeStyle == "")
                        {
                            colorSizeStyle = SizeName;
                        }
                        else
                        {
                            colorSizeStyle += " - " + SizeName;
                        }
                    }
                    if (Conversion.ToStr(StyleName) != "")
                    {
                        if (colorSizeStyle == "")
                        {
                            colorSizeStyle = StyleName;
                        }
                        else
                        {
                            colorSizeStyle += " - " + StyleName;
                        }
                    }
                }
                return colorSizeStyle;
            }
        }

        public bool Exists()
        {
            return (VariantNumber != "");
        }

        public override object Clone()
        {
            OldDimension dimensions = new OldDimension();
            Populate(dimensions);
            return dimensions;
        }

        protected void Populate(OldDimension dimensions)
        {
            dimensions.Text = Text;
            dimensions.variantNumber = (RecordIdentifier)variantNumber.Clone();
            dimensions.ItemID = (RecordIdentifier)ItemID.Clone();
            dimensions.ColorID = (RecordIdentifier)ColorID.Clone();
            dimensions.ColorName = ColorName;
            dimensions.SizeID = (RecordIdentifier)SizeID.Clone();
            dimensions.SizeName = SizeName;
            dimensions.StyleID = (RecordIdentifier)StyleID.Clone();
            dimensions.StyleName = StyleName;
            dimensions.EnterDimensions = EnterDimensions;
        }


        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
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
                XElement xDimension = new XElement("Dimensions",
                    new XElement("variantId", variantNumber),
                    new XElement("colorId", ColorID),
                    new XElement("colorName", ColorName),
                    new XElement("sizeId", SizeID),
                    new XElement("sizeName", SizeName),
                    new XElement("styleId", StyleID),
                    new XElement("styleName", StyleName),
                    new XElement("enterDimensions", EnterDimensions)
                );

                return xDimension;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "Dimensions.ToXML", ex);
                }

                throw ex;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classElements = xItem.Elements("Dimensions");
                    foreach (XElement xClass in classElements)
                    {
                        if (xClass.HasElements)
                        {
                            IEnumerable<XElement> classVariables = xClass.Elements();
                            foreach (XElement xVariable in classVariables)
                            {
                                if (!xVariable.IsEmpty)
                                {
                                    try
                                    {
                                        switch (xVariable.Name.ToString())
                                        {
                                            case "variantId":
                                                variantNumber = xVariable.Value;
                                                break;
                                            case "colorId":
                                                ColorID = xVariable.Value;
                                                break;
                                            case "colorName":
                                                ColorName = xVariable.Value;
                                                break;
                                            case "sizeId":
                                                SizeID = xVariable.Value;
                                                break;
                                            case "sizeName":
                                                SizeName = xVariable.Value;
                                                break;
                                            case "styleId":
                                                StyleID = xVariable.Value;
                                                break;
                                            case "styleName":
                                                StyleName = xVariable.Value;
                                                break;
                                            case "enterDimensions":
                                                EnterDimensions = Conversion.ToBool(xVariable.Value);
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        if (errorLogger != null)
                                        {
                                            errorLogger.LogMessage(LogMessageType.Error, "Dimensions:" + xVariable.Name, ex);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "Dimensions.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
