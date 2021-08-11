using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.SaleItem.Dimensions
{
    /// <summary>
    /// Holds the different dimensions of the item
    /// </summary>
    [Serializable]
    public class Dimensions : ICloneable 
    {
        #region Member variables
        private RecordIdentifier variantID;               //The variant code id.
        private RecordIdentifier colorID;                 //The id of the color
        private string colorName;               //The name of the color
        private RecordIdentifier sizeID;                  //The id of the size
        private string sizeName;                //The name of the size
        private RecordIdentifier styleID;                 //The id of the style
        private string styleName;               //The name of the style
        private bool enterDimensions;                     //Is set true if color,style,size info found, else false
        #endregion

        #region Properties
        /// <summary>
        /// The variant code id.
        /// </summary>
        public RecordIdentifier VariantID
        {
            get { return variantID; }
            set { variantID = value; }
        }
        /// <summary>
        /// The id of the color        
        /// </summary>
        public RecordIdentifier ColorID
        {
            get { return colorID; }
            set { colorID = value; }
        }
        /// <summary>
        /// The name of the color
        /// </summary>
        public string ColorName
        {
            get { return colorName; }
            set { colorName = value; }
        }
        /// <summary>
        /// The id of the size
        /// </summary>
        public RecordIdentifier SizeID
        {
            get { return sizeID; }
            set { sizeID = value; }
        }
        /// <summary>
        /// The name of the size
        /// </summary>
        public string SizeName
        {
            get { return sizeName; }
            set { sizeName = value; }
        }
        /// <summary>
        /// The id of the style
        /// </summary>
        public RecordIdentifier StyleID
        {
            get { return styleID; }
            set { styleID = value; }
        }
        /// <summary>
        /// The name of the style
        /// </summary>
        public string StyleName
        {
            get { return styleName; }
            set { styleName = value; }
        }
        /// <summary>
        /// Is set true if color,style,size info found, else false
        /// </summary>
        public bool EnterDimensions
        {
            get { return enterDimensions; }
            set { enterDimensions = value; }
        }
        #endregion

        /// <summary>
        /// Constructor sets VariantId to an empty string
        /// </summary>
        public Dimensions()
        {
            variantID = "";
            styleID = "";
            colorID = "";
        }

        /// <summary>
        /// If any of the following fields are filled then the function returns true otherwise it returns false; VariantId, ColorID, SizeId
        /// or StyleId
        /// </summary>
        /// <returns>True if any of the ids are filled out otherwise false</returns>
        public bool Exists()
        {
            return (Conversion.ToStr(variantID) != "");                   
        }

        public string Description
        {
            get
            {
                string colorSizeStyle = "";
                if (VariantID != "" && VariantID != null)
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

        public virtual object Clone()
        {
            Dimensions dimensions = new Dimensions();
            Populate(dimensions);
            return dimensions;
        }

        protected void Populate(Dimensions dimensions)
        {
            dimensions.variantID = variantID;
            dimensions.colorID = colorID;
            dimensions.colorName = colorName;
            dimensions.sizeID = sizeID;
            dimensions.sizeName = sizeName;
            dimensions.styleID = styleID;
            dimensions.styleName = styleName;
            dimensions.enterDimensions = enterDimensions;
        }

        public XElement ToXML(IErrorLog errorLogger = null)
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
                    new XElement("variantId", variantID),
                    new XElement("colorId", colorID),
                    new XElement("colorName", colorName),
                    new XElement("sizeId", sizeID),
                    new XElement("sizeName", sizeName),
                    new XElement("styleId", styleID),
                    new XElement("styleName", styleName),
                    new XElement("enterDimensions", enterDimensions)
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

        public void ToClass(XElement xItem, IErrorLog errorLogger = null)
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
                                                variantID = xVariable.Value;
                                                break;
                                            case "colorId":
                                                colorID = xVariable.Value;
                                                break;
                                            case "colorName":
                                                colorName = xVariable.Value;
                                                break;
                                            case "sizeId":
                                                sizeID = xVariable.Value;
                                                break;
                                            case "sizeName":
                                                sizeName = xVariable.Value;
                                                break;
                                            case "styleId":
                                                styleID = xVariable.Value;
                                                break;
                                            case "styleName":
                                                styleName = xVariable.Value;
                                                break;
                                            case "enterDimensions":
                                                enterDimensions = Conversion.ToBool(xVariable.Value);
                                                break;
                                        }
                                    }
                                    catch (Exception ex)                                        
                                    {
                                        if (errorLogger != null)
                                        {
                                            errorLogger.LogMessage(LogMessageType.Error, "Dimensions:" + xVariable.Name.ToString(), ex);
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
