using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Tax")]
namespace LSOne.DataLayer.BusinessObjects.Tax
{
    /// <summary>
    /// Connects a tax code to an item sales tax group. 
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class TaxCodeInItemSalesTaxGroup : DataEntity
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
        /// ID of the item sales tax group
        /// </summary>
        [DataMember(IsRequired = true)]
        public RecordIdentifier TaxItemGroup { set; get; }

        /// <summary>
        /// ID of the tax code
        /// </summary>        
        [DataMember(IsRequired = true)]
        public RecordIdentifier TaxCode { set; get; }

        /// <summary>
        /// Value of the tax code value. Does not get saved.
        /// </summary>
        public decimal TaxValue { internal set; get; }

        /// <summary>
        /// ID of this class. Composed of the ID of the item sales tax group and ID of the tax code
        /// </summary>
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(TaxItemGroup, TaxCode);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                XElement xAnswer = new XElement("taxCodeInItemSalesTaxGroup",
                        new XElement("taxItemGroup", (string)TaxItemGroup),
                        new XElement("Text", Text),
                        new XElement("taxCode", (string)TaxCode),
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
                                    case "taxItemGroup":
                                        TaxItemGroup = answerElem.Value;
                                        break;
                                    case "Text":
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
