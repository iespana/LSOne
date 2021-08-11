using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Tax")]
namespace LSOne.DataLayer.BusinessObjects.Tax
{
    /// <summary>
    /// Tax code values are attached to tax codes and represent the value of the tax code. They also have a date range and a single tax code can have many none-overlapping tax code value
    /// Usually though a tax code only has one tax code value
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [RecordIdentifierConstruction("ID", typeof(Guid))]
    public class TaxCodeValue : OptimizedUpdateDataEntity
    {
        /// <summary>
        /// Used to determine how to sort this class in a list. 
        /// </summary>
        public enum SortEnum
        {
            /// <summary>
            /// Sort by from date
            /// </summary>
            FromDate,
            /// <summary>
            /// Sort by to date
            /// </summary>
            ToDate,
            /// <summary>
            /// Sort by value
            /// </summary>
            Value
        }

        private RecordIdentifier taxCode;
        private Date fromDate;
        private Date toDate;
        private decimal value;

        public TaxCodeValue()
            : base()
        {
            Initialize();
        }

        protected sealed override void Initialize()
        {
            taxCode = "";
            fromDate = Date.Empty;
            toDate = Date.Empty;
            value = 0m;
        }

        /// <summary>
        /// ID of the tax code that the tax code value is attached to
        /// </summary>
        [DataMember(IsRequired = true)]
        public RecordIdentifier TaxCode
        {
            get { return taxCode; }
            set
            {
                AddColumnInfo("TAXCODE");
                taxCode = value;
            }
        }

        /// <summary>
        /// Value of the tax code value. A value of 10 here means 10 % tax
        /// </summary>
        [DataMember]
        public decimal Value
        {
            get { return value;}
            set
            {
                AddColumnInfo("TAXVALUE");
                this.value = value;
            }
        }

        /// <summary>
        /// Date when tax code value becomes valid. A blank value means the tax code value has no start date (is always valid until the ToDate). 
        /// Because DateTime objects can not be nullable, the value '01.01.1900' is considered blank
        /// </summary>
        [DataMember]
        public Date FromDate
        {
            get { return fromDate;}
            set
            {
                AddColumnInfo("TAXFROMDATE");
                fromDate = value;
            }
        }

        /// <summary>
        /// Date when tax code value stops being valid. A blank value means the tax code value has no end date (is valid from the FromDate). 
        /// Because DateTime objects can not be nullable, the value '01.01.1900' is considered blank
        /// </summary>
        [DataMember]
        public Date ToDate
        {
            get { return toDate; }
            set
            {
                AddColumnInfo("TAXTODATE");
                toDate = value;
            }
        }

        /// <summary>
        /// Checks whether the date range defined in this instance intersects with the range in the given <paramref name="valueToCompare"/>
        /// </summary>
        /// <param name="valueToCompare">The tax code value to compare to</param>
        /// <returns>True if the range defined by this instances <see cref="FromDate"/> and <see cref="ToDate"/> intersects with the same properties in <paramref name="valueToCompare"/></returns>
        public bool RangeIntersects(TaxCodeValue valueToCompare)
        {
            return
                (FromDate == Date.Empty && ToDate == Date.Empty) || //Exising value covers all dates
                (FromDate != Date.Empty && ToDate != Date.Empty && valueToCompare.FromDate == Date.Empty && valueToCompare.ToDate == Date.Empty) || //The value we're saving is supposed to cover all ragnes but the exising range is smaller
                (FromDate.DateTime <= valueToCompare.FromDate.DateTime && ToDate.DateTime >= valueToCompare.FromDate.DateTime) ||
                (FromDate.DateTime <= valueToCompare.ToDate.DateTime && ToDate.DateTime >= valueToCompare.ToDate.DateTime) ||
                (FromDate.DateTime >= valueToCompare.FromDate.DateTime && ToDate.DateTime <= valueToCompare.ToDate.DateTime);
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
                            case "id":
                                ID = new Guid(current.Value);
                                break;
                            case "name":
                                Text = current.Value;
                                break;
                            case "taxCode" :
                                TaxCode = current.Value;
                                break;
                            case "value" :
                                Value = XmlConvert.ToDecimal(current.Value);
                                break;
                            case "fromDate":
                                FromDate = new Date(Date.XmlStringToDateTime(current.Value));
                                break;
                            case "toDate" : 
                                ToDate = new Date(Date.XmlStringToDateTime(current.Value));
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error,
                                                   current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("taxCodeLine",
                    new XElement("id", (Guid)ID),
                    new XElement("name", Text),
                    new XElement("taxCode", (string)TaxCode),
                    new XElement("value", XmlConvert.ToString(Value)),
                    new XElement("fromDate", FromDate.ToXmlString()),
                    new XElement("toDate", ToDate.ToXmlString()));
            return xml;
        }
    }
}
