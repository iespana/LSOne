using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Data entity class for a retail item
    /// </summary>
    public partial class RetailItemOld
    {
        /// <summary>
        /// Determines the type of information that is being viewed. Default value is Sales
        /// </summary>
        public enum ModuleTypeEnum
        {
            /// <summary>
            /// Information that have to do with inventory pricing, storage, units and etc. Not currently used
            /// </summary>
            Inventory = 0,
            /// <summary>
            /// Information that have to do with purchase pricing, units and etc. Not currently used
            /// </summary>
            Purchase = 1,
            /// <summary>
            /// Information that have to do with sale pricing, storage, units and etc.
            /// </summary>
            Sales = 2
        }

        /// <summary>
        /// Information that is stored for each module of the retail item i.e. Inventory, Purchase and Sales
        /// </summary>
        [RecordIdentifierConstruction("ID", typeof(string), typeof(int))]
        public class RetailItemModule : DataEntity
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RetailItemModule"/> class.
            /// </summary>
            /// <param name="moduleType">Type of the module.</param>
            public RetailItemModule(ModuleTypeEnum moduleType)
                : this()
            {
                ID = new RecordIdentifier("", (int)moduleType);
                Unit = "";
                MultilineDiscount = "";
                LineDiscount = "";
                TaxItemGroupID = "";
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RetailItemModule"/> class.
            /// </summary>
            public RetailItemModule()
            {
                ID = new RecordIdentifier("", (int)ModuleTypeEnum.Inventory);
                Price = 0.0m;
                LastKnownPriceWithTax = 0.0m;
                PriceUnit = 0.0m;
                Markup = 0.0m;
                LineDiscount = "";
                LineDiscountName = "";
                MultilineDiscount = "";
                MultiLineDiscountName = "";
                TotalDiscount = true;
                PriceDate = Date.Empty;
                PriceQty = 0.0m;
                AllocateMarkup = false;
                Unit = "";
                UnitText = "";
                Dirty = true;
                TaxItemGroupID = "";
            }

            /// <summary>
            /// The unit ID of the item
            /// </summary>
            public RecordIdentifier Unit { get; set; }
            /// <summary>
            /// The description of the unit
            /// </summary>
            public string UnitText { get; set; }

            public bool Dirty { get; set; }
            /// <summary>
            /// Not currently used.
            /// </summary>
            public bool AllocateMarkup { get; set; }
            /// <summary>
            /// Not currently used
            /// </summary>
            public decimal PriceQty { get; set; }
            /// <summary>
            /// Not currently used
            /// </summary>
            public Date PriceDate { get; set; }
            /// <summary>
            /// If true then the item can be included in a total discount
            /// </summary>
            public bool TotalDiscount { get; set; }
            /// <summary>
            /// The ID of the multiline discount group the item is a part of
            /// </summary>
            public RecordIdentifier MultilineDiscount { get; set; }
            /// <summary>
            /// The ID of the line discount group the item is a part of
            /// </summary>
            public RecordIdentifier LineDiscount { get; set; }
            /// <summary>
            /// The description of the line discount group
            /// </summary>
            public string LineDiscountName { get; set; }
            /// <summary>
            /// The descriptino of the multiline discount group
            /// </summary>
            public string MultiLineDiscountName { get; set; }
            /// <summary>
            /// Not currently used
            /// </summary>
            public decimal Markup { get; set; }
            /// <summary>
            /// Not currently used
            /// </summary>
            public decimal PriceUnit { get; set; }
            /// <summary>
            /// The net price of the item
            /// </summary>
            public decimal Price { get; set; }
            /// <summary>
            /// This field is used for guiding purpose only to correct rounding issues if 10 for example was
            /// entered as Price with Tax, to ensure you get 10 out again if and only if calculated tax was rounded to
            /// close value. DO NOT BLINDLY use this value as Tax % may have changed
            /// </summary>
            public decimal LastKnownPriceWithTax { get; set; }
            /// <summary>
            /// The ID of the tax group the item belongs to
            /// </summary>
            public RecordIdentifier TaxItemGroupID { get; set; }
            /// <summary>
            /// The description of the tax group
            /// </summary>
            public string TaxItemGroupName { get; set; }

            /// <summary>
            /// The unique ID of the retail item module
            /// </summary>
            public RecordIdentifier ItemID
            {
                get { return ID.PrimaryID; }
                set { ID = new RecordIdentifier(value, ID.SecondaryID); }
            }

            /// <summary>
            /// The module type i.e. Inventory, Purchase or Sales
            /// </summary>
            public ModuleTypeEnum ModuleType
            {
                get { return (ModuleTypeEnum)(int)ID.SecondaryID; }
                set { ID = new RecordIdentifier(ID.PrimaryID, (int)value); }
            }

            public override void ToClass(XElement element, IErrorLog errorLogger = null)
            {
                IEnumerable<XElement> elements = element.Elements();
                foreach (XElement current in elements)
                {
                    if (!current.IsEmpty)
                    {
                        try
                        {
                            switch (current.Name.ToString())
                            {
                                case "itemID":
                                    ItemID = current.Value;
                                    break;
                                case "moduleType":
                                    ModuleType = (ModuleTypeEnum)Convert.ToInt32(current.Value);
                                    break;
                                case "allocateMarkup":
                                    AllocateMarkup = Convert.ToBoolean(current.Value, XmlCulture);
                                    break;
                                case "lastKnownPriceWithTax":
                                    LastKnownPriceWithTax = Convert.ToDecimal(current.Value, XmlCulture);
                                    break;
                                case "lineDiscount":
                                    LineDiscount = current.Value;
                                    break;
                                case "lineDiscountName":
                                    LineDiscountName = current.Value;
                                    break;
                                case "markup":
                                    Markup = Convert.ToDecimal(current.Value, XmlCulture);
                                    break;
                                case "multilineDiscount":
                                    MultilineDiscount = current.Value;
                                    break;
                                case "multiLineDiscountName":
                                    MultiLineDiscountName = current.Value;
                                    break;
                                case "price":
                                    Price = Convert.ToDecimal(current.Value, XmlCulture);
                                    break;
                                case "priceDate":
                                    PriceDate = new Date(Date.XmlStringToDateTime(current.Value));
                                    break;
                                case "priceQty":
                                    PriceQty = Convert.ToDecimal(current.Value, XmlCulture);
                                    break;
                                case "priceUnit":
                                    PriceUnit = Convert.ToDecimal(current.Value, XmlCulture);
                                    break;
                                case "taxItemGroupID":
                                    TaxItemGroupID = current.Value;
                                    break;
                                case "taxItemGroupName":
                                    TaxItemGroupName = current.Value;
                                    break;
                                case "totalDiscount":
                                    TotalDiscount = current.Value == "true";
                                    break;
                                case "unit":
                                    Unit = current.Value;
                                    break;
                                case "unitText":
                                    UnitText = current.Value;
                                    break;
                                case "usageIntent":
                                    UsageIntent = (UsageIntentEnum)Convert.ToInt32(current.Value);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(),
                                                       ex);
                            }
                        }
                    }
                }
            }

            public override XElement ToXML(IErrorLog errorLogger = null)
            {
                var xml = new XElement("RetailItemModule",
                        new XElement("itemID", (string)ItemID),
                        new XElement("moduleType", (int)ModuleType),
                        new XElement("allocateMarkup", AllocateMarkup),
                        new XElement("lastKnownPriceWithTax", LastKnownPriceWithTax.ToString(XmlCulture)),
                        new XElement("lineDiscount", (string)LineDiscount),
                        new XElement("lineDiscountName", LineDiscountName),
                        new XElement("markup", Markup.ToString(XmlCulture)),
                        new XElement("multilineDiscount", (string)MultilineDiscount),
                        new XElement("multiLineDiscountName", MultiLineDiscountName),
                        new XElement("price", Price.ToString(XmlCulture)),
                        new XElement("priceDate", PriceDate.ToXmlString()),
                        new XElement("priceQty", PriceQty.ToString(XmlCulture)),
                        new XElement("priceUnit", PriceUnit.ToString(XmlCulture)),
                        new XElement("taxItemGroupID", (string)TaxItemGroupID),
                        new XElement("taxItemGroupName", TaxItemGroupName),
                        new XElement("totalDiscount", TotalDiscount),
                        new XElement("unit", (string)Unit),
                        new XElement("unitText", UnitText),
                        new XElement("usageIntent", (int)UsageIntent));
                return xml;
            }

            public override object Clone()
            {
                var module = new RetailItemModule();
                Populate(module);
                return module;
            }

            protected void Populate(RetailItemModule module)
            {
                base.Populate(module);

                module.Unit = Unit;
                module.UnitText = UnitText;
                module.Dirty = Dirty;
                module.AllocateMarkup = AllocateMarkup;
                module.PriceQty = PriceQty;
                module.PriceDate = PriceDate;
                module.TotalDiscount = TotalDiscount;
                module.MultilineDiscount = MultilineDiscount;
                module.LineDiscount = LineDiscount;
                module.LineDiscountName = LineDiscountName;
                module.MultiLineDiscountName = MultiLineDiscountName;
                module.Markup = Markup;
                module.PriceUnit = PriceUnit;
                module.Price = Price;
                module.LastKnownPriceWithTax = LastKnownPriceWithTax;
                module.TaxItemGroupID = TaxItemGroupID;
                module.TaxItemGroupName = TaxItemGroupName;
                module.ItemID = ItemID;
                module.ModuleType = ModuleType;
            }
        }
    }
}
