using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Retail item assembly component
    /// </summary>
    public class RetailItemAssemblyComponent : DataEntity
    {
        public RetailItemAssemblyComponent()
        {
            AssemblyID = RecordIdentifier.Empty;
            ItemID = RecordIdentifier.Empty;
            UnitID = RecordIdentifier.Empty;
            HeaderItemID = RecordIdentifier.Empty;
        }

        /// <summary>
        /// ID of the assembly
        /// </summary>
        public RecordIdentifier AssemblyID { get; set; }

        /// <summary>
        /// Item ID of the component
        /// </summary>
        public RecordIdentifier ItemID { get; set; }

        /// <summary>
        /// Header item ID in case of a variant item
        /// </summary>
        public RecordIdentifier HeaderItemID { get; set; }

        /// <summary>
        /// The name of the item
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// Item variant name
        /// </summary>
        public string VariantName { get; set; }

        /// <summary>
        /// Unit ID of the component
        /// </summary>
        public RecordIdentifier UnitID { get; set; }

        /// <summary>
        /// The name of the unit
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// Quantity of the component
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Cost per 1 unit of quantity. NOT SAVED.
        /// </summary>
        public decimal CostPerUnit { get; set; }

        /// <summary>
        /// Get the total cost of the component
        /// </summary>
        /// <returns>Decimal</returns>
        public decimal GetTotalCost()
        {
            return Quantity * CostPerUnit;
        }

        /// <summary>
        /// Sales price of the components. NOT SAVED. Only retrieved in Site Manager for calculation
        /// </summary>
        public decimal SalesPrice { get; set; }

        /// <summary>
        /// True if the sales price was already retrieved from the database from this component. Only used in Site Manager for calculation
        /// </summary>
        public bool SalesPriceRetrieved { get; set; }

        /// <summary>
        /// Get the total sales price of the components
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalSalesPrice()
        {
            return Quantity * SalesPrice;
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
                XElement xSaleItem = new XElement("RetailItemAssemblyComponent",
                    new XElement("ID", ID.StringValue),
                    new XElement("assemblyID", AssemblyID.StringValue),
                    new XElement("itemID", ItemID.StringValue),
                    new XElement("headerItemID", HeaderItemID.StringValue),
                    new XElement("itemName", ItemName),
                    new XElement("variantName", VariantName),
                    new XElement("unitID", UnitID.StringValue),
                    new XElement("unitName", UnitName),
                    new XElement("quantity", Conversion.ToXmlString(Quantity)),
                    new XElement("costPerUnit", Conversion.ToXmlString(CostPerUnit))
                );

                xSaleItem.Add(base.ToXML(errorLogger));
                return xSaleItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "RetailItemAssemblyComponent.ToXml", ex);

                throw;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classVariables = xItem.Elements();
                    foreach (XElement xVariable in classVariables)
                    {
                        if (!xVariable.IsEmpty)
                        {
                            try
                            {
                                switch (xVariable.Name.ToString())
                                {
                                    case "ID":
                                        ID = xVariable.Value;
                                        break;
                                    case "assemblyID":
                                        AssemblyID = xVariable.Value;
                                        break;
                                    case "itemID":
                                        ItemID = xVariable.Value;
                                        break;
                                    case "headerItemID":
                                        HeaderItemID = xVariable.Value;
                                        break;
                                    case "itemName":
                                        ItemName = xVariable.Value;
                                        break;
                                    case "variantName":
                                        VariantName = xVariable.Value;
                                        break;
                                    case "unitID":
                                        UnitID = xVariable.Value;
                                        break;
                                    case "unitName":
                                        UnitName = xVariable.Value;
                                        break;
                                    case "quantity":
                                        Quantity = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "costPerUnit":
                                        CostPerUnit = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    default:
                                        base.ToClass(xVariable);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "RetailItemAssemblyComponent:" + xVariable.Name, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "RetailItemAssemblyComponent.ToClass", ex);
                throw;
            }
        }
    }
}
