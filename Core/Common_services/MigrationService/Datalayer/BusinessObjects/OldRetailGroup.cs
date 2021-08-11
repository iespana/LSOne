#if !MONO
#endif
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.Services.Datalayer.BusinessObjects
{
    /// <summary>
    /// Data entity class for a retail group.
    /// </summary>
    public class OldRetailGroup : DataEntity
    {
        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public OldRetailGroup()
            : base()
        {
            RetailDepartmentId = "";
            RetailDepartmentName = "";
            SizeGroupId = "";
            SizeGroupName = "";
            ColorGroupId = "";
            ColorGroupName = "";
            StyleGroupId = "";
            StyleGroupName = "";
            DimensionGroupId = "";
            DimensionGroupName = "";
            ItemSalesTaxGroupId = "";
            ItemSalesTaxGroupName = "";
            ProfitMargin = 0;

            ValidationPeriod = "";
            ValidationPeriodDescription = "";
        }

        /// <summary>
        /// The unique ID of the retail group
        /// </summary>
        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get {return base.ID;}
            set {base.ID = value;}
        }

        /// <summary>
        /// The description of the retail group
        /// </summary>
        [StringLength(60)]
        public override string Text
        {
            get {return base.Text;}
            set {base.Text = value;}
        }

        /// <summary>
        /// If set the item that is added to the retail group will get this retail department as a default department
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier RetailDepartmentId { get; set; }


        /// <summary>
        /// The description of the retail department
        /// </summary>
        public string RetailDepartmentName { get; internal set; }

        /// <summary>
        /// If set the item that is added to the retail group will get this ID as it's default size group
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SizeGroupId { get; set; }

        /// <summary>
        /// The description of the size group
        /// </summary>
        public string SizeGroupName { get; internal set; }



        /// <summary>
        /// If set the item that is added to the retail group will get this ID as it's default color group
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier ColorGroupId { get; set; }

        /// <summary>
        /// The description of the color group
        /// </summary>
        public string ColorGroupName { get; internal set; }

        /// <summary>
        /// If set the item that is added to the retail group will get this ID as it's default style group
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier StyleGroupId { get; set; }

        /// <summary>
        /// The description of the style group
        /// </summary>
        public string StyleGroupName { get; internal set; }

        /// <summary>
        /// If set the item that is added to the retail group will get this dimension group as a default group ID
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier DimensionGroupId { get; set; }

        /// <summary>
        /// The description of the dimension group
        /// </summary>
        public string DimensionGroupName { get; internal set; }

        /// <summary>
        /// If set the item that is added to the retail group will get this tax group as it's default tax group
        /// </summary>
        [RecordIdentifierValidation(20)]
        public RecordIdentifier ItemSalesTaxGroupId { get; set; }

        /// <summary>
        /// The description of the tax group
        /// </summary>
        public string ItemSalesTaxGroupName { get; set; }

        /// <summary>
        /// The default profit margin
        /// </summary>
        public decimal ProfitMargin { get; set; }

        public string ValidationPeriod { get; set; }

        public string ValidationPeriodDescription { get; set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var currencyElements = element.Elements();
            foreach (XElement storeElem in currencyElements)
            {
                if (!storeElem.IsEmpty)
                {
                    try
                    {
                        switch (storeElem.Name.ToString())
                        {
                            case "groupID":
                               ID = storeElem.Value;
                                break;
                            case "name":
                               Text = storeElem.Value;
                                break;
                            case "retailDepartmentId":
                                RetailDepartmentId = storeElem.Value;
                                break;
                            case "retailDepartmentName":
                                RetailDepartmentName = storeElem.Value;
                                break;
                            case "sizeGroupId":
                                SizeGroupId = storeElem.Value;
                                break;
                            case "sizeGroupName":
                                SizeGroupName = storeElem.Value;
                                break;
                            case "colorGroupId":
                                ColorGroupId = storeElem.Value;
                                break;
                            case "colorGroupName":
                                ColorGroupName = storeElem.Value;
                                break;
                            case "styleGroupId":
                                StyleGroupId = storeElem.Value;
                                break;
                            case "styleGroupName":
                                StyleGroupName = storeElem.Value;
                                break;
                            case "dimensionGroupId":
                                DimensionGroupId = storeElem.Value;
                                break;
                            case "dimensionGroupName":
                                DimensionGroupName = storeElem.Value;
                                break;
                            case "itemSalesTaxGroupId":
                                ItemSalesTaxGroupId = storeElem.Value;
                                break;
                            case "itemSalesTaxGroupName":
                                ItemSalesTaxGroupName = storeElem.Value;
                                break;
                            case "profitMargin":
                                ProfitMargin = Convert.ToDecimal(storeElem.Value, XmlCulture);
                                break;
                            case "validationPeriod":
                                ValidationPeriod = storeElem.Value;
                                break;
                            case "validationPeriodDescription":
                                ValidationPeriodDescription = storeElem.Value;
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

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("RetailGroup",
                    new XElement("groupID", (string)ID),
                    new XElement("name", Text),
                    new XElement("retailDepartmentId", RetailDepartmentId),
                    new XElement("retailDepartmentName", RetailDepartmentName),
                    new XElement("sizeGroupId", (string)SizeGroupId),
                    new XElement("sizeGroupName", SizeGroupName),
                    new XElement("styleGroupId", (string)StyleGroupId),
                    new XElement("styleGroupName", StyleGroupName),
                    new XElement("colorGroupId", (string)ColorGroupId),
                    new XElement("colorGroupName", ColorGroupName),
                    new XElement("dimensionGroupId", (string)DimensionGroupId),
                    new XElement("dimensionGroupName", DimensionGroupName),
                    new XElement("itemSalesTaxGroupId", (string)ItemSalesTaxGroupId),
                    new XElement("itemSalesTaxGroupName", ItemSalesTaxGroupName),
                    new XElement("profitMargin", ProfitMargin.ToString(XmlCulture)),
                    new XElement("validationPeriod", ValidationPeriod),
                    new XElement("validationPeriodDescription", ValidationPeriodDescription));
            return xml;
        }
    }
}
 