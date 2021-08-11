using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Tax
{
    /// <summary>
    /// Used by LS POS. Does not get saved to database.
    /// </summary>
    [Serializable]
    public class TaxItem : ICloneable
    {
        #region Member variables
        
        private RecordIdentifier itemTaxGroup;             // The taxgroup, one per item
        private string itemTaxGroupDisplay;      // Used to display the tax group on the receipt
        private RecordIdentifier taxCode;              // The taxcode, can be many per item  
        private string taxCodeDisplay;       // Used to display the tax code on the receipt
        private decimal percentage;          // The tax percentage 
        private decimal amount;              // The tax amount 
        private decimal priceWithTax;        // The amount that is taxed
        private decimal taxRoundOff;         // The round off value for the tax group
        private int taxRoundOffType;         // The round off type for the tax group
        #endregion

        public TaxItem()
        {
            itemTaxGroup = string.Empty;
            itemTaxGroupDisplay = string.Empty;
            taxCode = string.Empty;
            taxCodeDisplay = string.Empty;
            percentage = decimal.Zero;
            amount = decimal.Zero;
            priceWithTax = decimal.Zero;
            TaxExemptionCode = string.Empty;
            
        }

        #region Properties

        /// <summary>
        /// The rounding value for the tax group
        /// </summary>
        public decimal TaxRoundOff
        {
            get { return taxRoundOff; }
            set { taxRoundOff = value; }
        }

        /// <summary>
        /// The round off type for the tax group
        /// </summary>
        public int TaxRoundOffType
        {
            get { return taxRoundOffType; }
            set { taxRoundOffType = value; }
        }

        /// <summary>
        /// The taxgroup, one per item
        /// </summary>
        public RecordIdentifier ItemTaxGroup
        {
            get { return itemTaxGroup; }
            set { itemTaxGroup = value; }
        }

        /// <summary>
        /// Used to display the tax group on the receipt
        /// </summary>
        public string ItemTaxGroupDisplay
        {
            get { return itemTaxGroupDisplay; }
            set { itemTaxGroupDisplay = value; }
        }

        /// <summary>
        /// The taxcode, can be many per item 
        /// </summary>
        public RecordIdentifier TaxCode
        {
            get { return taxCode; }
            set { taxCode = value; }
        }

        /// <summary>
        /// Used to display the tax code on the receipt
        /// </summary>
        public string TaxCodeDisplay
        {
            get { return taxCodeDisplay; }
            set { taxCodeDisplay = value; }
        }

        /// <summary>
        /// The tax percentage 
        /// </summary>
        public decimal Percentage
        {
            get { return percentage; }
            set { percentage = value; }
        }

        /// <summary>
        /// The tax amount 
        /// </summary>
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        /// <summary>
        /// The amount that is taxed
        /// </summary>
        public decimal PriceWithTax
        {
            get { return priceWithTax; }
            set { priceWithTax = value; }
        }

        /// <summary>
        /// If true then this tax line is part of a sales line that has been tax exempted
        /// </summary>
        public bool TaxExempt { get; set; }

        /// <summary>
        /// The tax exemption code
        /// </summary>
        public string TaxExemptionCode { get; set; }

        /// <summary>
        /// The tax group on the Store, Customer or Hospitality type depending on the settings for the transaction
        /// </summary>
        public RecordIdentifier StoreTaxGroup { get; set; }
            

        #endregion

        public object Clone()
        {
            TaxItem item = new TaxItem();
            Populate(item);
            return item;
        }

        protected void Populate(TaxItem item)
        {
            item.itemTaxGroup = (RecordIdentifier)itemTaxGroup.Clone();
            item.taxCode = (RecordIdentifier)taxCode.Clone();
            item.percentage = percentage;
            item.amount = amount;
            item.priceWithTax = priceWithTax;
            item.taxRoundOff = taxRoundOff;
            item.taxRoundOffType = taxRoundOffType;
            item.itemTaxGroupDisplay = itemTaxGroupDisplay;
            item.taxCodeDisplay = taxCodeDisplay;
            item.TaxExempt = TaxExempt;
            item.TaxExemptionCode = TaxExemptionCode;
            item.StoreTaxGroup = StoreTaxGroup;
        }

        public XElement ToXML(IErrorLog transLog = null)
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
                XElement xTax = new XElement("TaxItem",
                    new XElement("taxGroup", (string)itemTaxGroup),
                    new XElement("taxCode", (string)taxCode),
                    new XElement("percentage", Conversion.ToXmlString(percentage)),
                    new XElement("amount", Conversion.ToXmlString(amount)),
                    new XElement("priceWithTax", Conversion.ToXmlString(priceWithTax)),
                    new XElement("taxRoundOff", Conversion.ToXmlString(taxRoundOff)),
                    new XElement("taxRoundOffType", Conversion.ToXmlString(taxRoundOffType)),
                    new XElement("taxGroupDisplay", itemTaxGroupDisplay),
                    new XElement("taxCodeDisplay", taxCodeDisplay),
                    new XElement("taxExempt", Conversion.ToXmlString(TaxExempt)),
                    new XElement("taxExemptionCode", TaxExemptionCode),
                    new XElement("storeTaxGroup", StoreTaxGroup)
                );

                return xTax;
            }
            catch (Exception ex)
            {
                transLog?.LogMessage(LogMessageType.Error, "TaxItem", ex);
                throw;
            }
        }

        public void ToClass(XElement xItem, IErrorLog transLog = null)
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
                                    case "taxGroup":
                                        itemTaxGroup = xVariable.Value;
                                        break;
                                    case "taxCode":
                                        taxCode = xVariable.Value;
                                        break;
                                    case "percentage":
                                        percentage = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "amount":
                                        amount = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "priceWithTax":
                                        priceWithTax = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "taxRoundOff":
                                        taxRoundOff = Conversion.XmlStringToDecimal(xVariable.Value);
                                        break;
                                    case "taxRoundOffType":
                                        taxRoundOffType = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "taxGroupDisplay":
                                        itemTaxGroupDisplay = xVariable.Value;
                                        break;
                                    case "taxCodeDisplay":
                                        taxCodeDisplay = xVariable.Value;
                                        break;
                                    case "taxExempt":
                                        TaxExempt = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "taxExemptionCode":
                                        TaxExemptionCode = xVariable.Value;
                                        break;
                                    case "storeTaxGroup":
                                        StoreTaxGroup = xVariable.Value;
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                transLog?.LogMessage(LogMessageType.Error,"TaxItem: " + xVariable, ex);
                            }
                        }
                    }

                }
            } 
            catch (Exception ex)
            {
                transLog?.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }
        }
    }
}
