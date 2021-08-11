using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Retail item assembly
    /// </summary>
    public class RetailItemAssembly : DataEntity
    {
        private decimal margin;

        public RetailItemAssembly()
        {
            ItemID = RecordIdentifier.Empty;
            StoreID = RecordIdentifier.Empty;
            AssemblyComponents = new List<RetailItemAssemblyComponent>();
            StartingDate = Date.Empty;
            StoreName = "";
        }

        /// <summary>
        /// Item ID linked to the assembly
        /// </summary>
        public RecordIdentifier ItemID { get; set; }

        /// <summary>
        /// The store ID on which the assembly is enabled or empty for all stores
        /// </summary>
        public RecordIdentifier StoreID { get; set; }

        /// <summary>
        /// The name of the store for the assembly
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// True if the assembly is enabled and can be sold on the POS
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// True if the price of the assembly is calculated from the price of all components
        /// </summary>
        public bool CalculatePriceFromComponents { get; set; }

        /// <summary>
        /// Detemrmines if and how assembly components are sent to KDS
        /// </summary>
        public KitchenDisplayAssemblyComponentType SendAssemblyComponentsToKds { get; set; }

        /// <summary>
        /// If true, each assembly component is sent as a separate item to KDS.
        /// </summary>
        public bool SendComponentsToKdsAsSeparateItems { get { return SendAssemblyComponentsToKds == KitchenDisplayAssemblyComponentType.SendAsSeparateItems; } }

        /// <summary>
        /// If true, each assembly component is sent as a item modifiers to KDS.
        /// </summary>
        public bool SendComponentsToKdsAsItemModifiers { get { return SendAssemblyComponentsToKds == KitchenDisplayAssemblyComponentType.SendAsItemModifiers; } }

        public ExpandAssemblyLocation ExpandAssembly { get; set; }

        public bool ShallDisplayWithComponents(ExpandAssemblyLocation expandLocation)
        {
            return (ExpandAssembly & expandLocation) == expandLocation;
        }

        public void SetDisplayWithComponents(ExpandAssemblyLocation expandLocation, bool value)
        {
            ExpandAssembly = value 
                ? ExpandAssembly | expandLocation 
                : ExpandAssembly & ~expandLocation;
        }

        public string GetDisplayWithComponentsString()
        {
            if (ExpandAssembly == ExpandAssemblyLocation.None)
            {
                return Properties.Resources.None;
            }

            List<string> lst = new List<string>();
            if (ShallDisplayWithComponents(ExpandAssemblyLocation.OnPOS))
            {
                lst.Add(Properties.Resources.POS);
            }

            if (ShallDisplayWithComponents(ExpandAssemblyLocation.OnReceipt))
            {
                lst.Add(Properties.Resources.Receipt);
            }

            return string.Join(", ", lst);
        }

        /// <summary>
        /// Price of the assembly
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Total sales price of the assembly if the price is calculated from components
        /// </summary>
        public decimal TotalSalesPrice { get; set; }

        /// <summary>
        /// Profit margin of the assembly. NOT SAVED
        /// </summary>
        public decimal Margin
        {
            get { return margin; }
            set
            {
                if(value < 0)
                {
                    margin = 0;
                }
                else if (value > 100)
                {
                    margin = 100;
                }
                else
                {
                    margin = value;
                }
            }
        }

        /// <summary>
        /// List of assembly components
        /// </summary>
        public List<RetailItemAssemblyComponent> AssemblyComponents { get; set; }

        /// <summary>
        /// The date from which the assembly is active
        /// </summary>
        public Date StartingDate { get; set; }

        /// <summary>
        /// Current status of the assembly. NOT SAVED.
        /// </summary>
        public RetailItemAssemblyStatus Status;

        /// <summary>
        /// Total cost of the assembly. NOT SAVED.
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Return the price of the assembly based on the calculate from components setting
        /// </summary>
        /// <returns></returns>
        public decimal GetDisplayPrice()
        {
            return CalculatePriceFromComponents ? TotalSalesPrice : Price;
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
                XElement xSaleItem = new XElement("RetailItemAssembly",
                    new XElement("ID", ID.StringValue),
                    new XElement("itemID", ItemID.StringValue),
                    new XElement("storeID", StoreID.StringValue),
                    new XElement("storeName", StoreName),
                    new XElement("enabled", Conversion.ToXmlString(Enabled)),
                    new XElement("calculatePriceFromComponents", Conversion.ToXmlString(CalculatePriceFromComponents)),
                    new XElement("expandAssembly", Conversion.ToXmlString((int)ExpandAssembly)),
                    new XElement("sendAssemblyComponentsToKds", Conversion.ToXmlString((int)SendAssemblyComponentsToKds)),
                    new XElement("price", Conversion.ToXmlString(Price)),
                    new XElement("totalSalesPrice", Conversion.ToXmlString(TotalSalesPrice)),
                    new XElement("margin", Conversion.ToXmlString(Margin)),
                    new XElement("startingDate", Conversion.ToXmlString(StartingDate.DateTime)),
                    ListToXML(AssemblyComponents)
                );

                xSaleItem.Add(base.ToXML(errorLogger));
                return xSaleItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "RetailItemAssembly.ToXml", ex);

                throw;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    List<XElement> classVariables = xItem.Elements().ToList();

                    if(classVariables.Count == 1 && classVariables[0].Name == "RetailItemAssembly")
                    {
                        classVariables = classVariables[0].Elements().ToList();
                    }

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
                                    case "itemID":
                                        ItemID = xVariable.Value;
                                        break;
                                    case "storeID":
                                        StoreID = xVariable.Value;
                                        break;
                                    case "storeName":
                                        StoreName = xVariable.Value;
                                        break;
                                    case "enabled":
                                        Enabled = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "calculatePriceFromComponents":
                                        CalculatePriceFromComponents = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "expandAssembly":
                                        ExpandAssembly = (ExpandAssemblyLocation)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "sendAssemblyComponentsToKds":
                                        SendAssemblyComponentsToKds = (KitchenDisplayAssemblyComponentType)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "price":
                                        Price = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "totalSalesPrice":
                                        TotalSalesPrice = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "margin":
                                        Margin = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "startingDate":
                                        StartingDate = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                        break;
                                    case "assemblyComponents":
                                        AssemblyComponents = ListToClass(xVariable);
                                        break;
                                    default:
                                        base.ToClass(xVariable);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "RetailItemAssembly:" + xVariable.Name, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "RetailItemAssembly.ToClass", ex);
                throw;
            }
        }

        private XElement ListToXML(List<RetailItemAssemblyComponent> componentsList)
        {
            XElement componentElement = new XElement("assemblyComponents");

            if (componentsList == null)
                return componentElement;

            foreach (RetailItemAssemblyComponent c in componentsList)
            {
                componentElement.Add(c.ToXML());
            }

            return componentElement;
        }

        private List<RetailItemAssemblyComponent> ListToClass(XElement xItems, IErrorLog errorLogger = null)
        {
            List<RetailItemAssemblyComponent> componentsList = new List<RetailItemAssemblyComponent>();

            if (xItems.HasElements)
            {
                foreach (XElement xVariable in xItems.Elements())
                {
                    if (!xVariable.IsEmpty && xVariable.Name.ToString() == "RetailItemAssemblyComponent")
                    {
                        try
                        {
                            RetailItemAssemblyComponent newComp = new RetailItemAssemblyComponent();
                            newComp.ToClass(xVariable);
                            componentsList.Add(newComp);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return componentsList;
        }
    }
}
