using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Tax
{
    /// <summary>
    /// Connects a tax code to a sales tax group. 
    /// </summary>
    public class TaxCodeInSalesTaxGroup : DataEntity
    {
        /// <summary>
        /// Used to determine how to sort this class in a list.
        /// </summary>
        public enum SortEnum
        {
            /// <summary>
            /// Sort by salex tax code ID
            /// </summary>
            SalesTaxCode,
            /// <summary>
            /// Sort by sales tax code name
            /// </summary>
            Description,
            /// <summary>
            /// Sort by tax code value
            /// </summary>
            PercentageAmount
        }

        /// <summary>
        /// ID of the sales tax group
        /// </summary>
        public RecordIdentifier SalesTaxGroup { set; get; }

        /// <summary>
        /// ID of the tax code
        /// </summary>
        public RecordIdentifier TaxCode { set; get; }

        /// <summary>
        /// Value of the tax code value. Does not get saved.
        /// </summary>
        public decimal TaxValue { set; get; }

        /// <summary>
        /// ID of this class. Composed of the ID of the  sales tax group and ID of the tax code
        /// </summary>
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(SalesTaxGroup, TaxCode);
            }
            set
            {
                throw new NotSupportedException();
            }
        }
        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                XElement xAnswer = new XElement("taxCodeInSalesTaxGroup",
                        new XElement("salesTaxGroup", (string)SalesTaxGroup),
                        new XElement("taxCode", TaxCode),
                        new XElement("text", Text),
                        new XElement("taxValue", XmlConvert.ToString(TaxValue))
                    );

                return xAnswer;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "DataEntity.ToXml", ex);
                }
            }

            return null;
        }

        public override void ToClass(XElement xmlAnswer, IErrorLog errorLogger = null)
        {
            try
            {
                if (xmlAnswer.HasElements)
                {
                    IEnumerable<XElement> answerVariables = xmlAnswer.Elements();
                    foreach (XElement answerElem in answerVariables)
                    {
                        if (!answerElem.IsEmpty)
                        {
                            try
                            {
                                switch (answerElem.Name.ToString())
                                {
                                    case "salesTaxGroup":
                                        SalesTaxGroup = answerElem.Value;
                                        break;
                                    case "text":
                                        Text = answerElem.Value;
                                        break;
                                    case "taxCode":
                                        TaxCode = answerElem.Value;
                                        break;
                                    case "taxValue":
                                        TaxValue = XmlConvert.ToDecimal(answerElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, answerElem.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "DataEntity.ToClass", ex);
                }
            }
        }

    }
}
