#if !MONO
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.ItemMaster")]
namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Data entity class for a retail group.
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class RetailGroup : OptimizedUpdateDataEntity
    {
        private RecordIdentifier masterID;
        private RecordIdentifier retailDepartmentID;
        private RecordIdentifier retailDepartmentMasterID;
        private RecordIdentifier itemSalesTaxGroupId;
        private decimal profitMargin;
        private string validationPeriod;
        private int tareWeight;

        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public RetailGroup()
            : base()
        {
            Initialize();
        }

        protected sealed override void Initialize()
        {
            retailDepartmentID = RecordIdentifier.Empty;
            RetailDepartmentName = "";
            itemSalesTaxGroupId = RecordIdentifier.Empty;
            ItemSalesTaxGroupName = "";
            profitMargin = 0;

            validationPeriod = "";
            ValidationPeriodDescription = "";
        }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier MasterID
        {
            get { return masterID; }
            set
            {
                masterID = value;
            }

        }
        /// <summary>
        /// The unique ID of the retail group
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get {return base.ID;}
            set { base.ID = value; }
        }

        /// <summary>
        /// The description of the retail group
        /// </summary>
        [DataMember]
        [StringLength(60)]
        public override string Text
        {
            get {return base.Text;}
            set
            {
                if (base.Text != value)
                {
                    PropertyChanged("NAME", value);
                    base.Text = value;
                }
            }
        }

        /// <summary>
        /// If set the item that is added to the retail group will get this retail department as a default department
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier RetailDepartmentID
        {
            get { return retailDepartmentID; }
            set
            {
                if (retailDepartmentID != value)
                {
                    PropertyChanged("DEPARTMENTID", value);
                    retailDepartmentID = value;
                }
            }
        }

        [DataMember]
        [RecordIdentifierValidation(Utilities.DataTypes.RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier RetailDepartmentMasterID
        {
            get { return retailDepartmentMasterID; }
            set
            {
                if (retailDepartmentMasterID != value)
                {
                    PropertyChanged("DEPARTMENTMASTERID", value);
                    retailDepartmentMasterID = value;
                }
            }
        }

        /// <summary>
        /// The description of the retail department
        /// </summary>
        [DataMember]
        public string RetailDepartmentName { get; set; }


        /// <summary>
        /// If set the item that is added to the retail group will get this tax group as it's default tax group
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier ItemSalesTaxGroupId
        {
            get { return itemSalesTaxGroupId; }
            set
            {
                if (itemSalesTaxGroupId != value)
                {
                    PropertyChanged("SALESTAXITEMGROUP", value);
                    itemSalesTaxGroupId = value;
                }
            }
        }

        /// <summary>
        /// The description of the tax group
        /// </summary>
        [DataMember]
        public string ItemSalesTaxGroupName { get; set; }

        /// <summary>
        /// The default profit margin
        /// </summary>
        [DataMember]
        public decimal ProfitMargin
        {
            get { return profitMargin; }
            set
            {
                if (profitMargin != value)
                {
                    PropertyChanged("DEFAULTPROFIT", value);
                    profitMargin = value;
                }
            }
        }

        [DataMember]
        public string ValidationPeriod
        {
            get { return validationPeriod; }
            set
            {
                if (validationPeriod != value)
                {
                    PropertyChanged("POSPERIODICID", value);
                    validationPeriod = value;
                }
            }
        }

        [DataMember]
        public string ValidationPeriodDescription { get; set; }

        /// <summary>
        /// Packaging weight to be subtracted from weighted value
        /// </summary>
        [DataMember]
        public int TareWeight
        {
            get { return tareWeight; }
            set
            {
                if (tareWeight != value)
                {
                    PropertyChanged("TAREWEIGHT", value);
                    tareWeight = value;
                }
            }
        }

        /// <summary>
        /// Get the creation date
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Get the last modified date
        /// </summary>
        public DateTime Modified { get; private set; }

        public override List<string> GetIgnoredColumns()
        {
            return new List<string> { "DEPARTMENTID", "SALESTAXITEMGROUP" };
        }

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
                                RetailDepartmentID = storeElem.Value;
                                break;
                            case "retailDepartmentName":
                                RetailDepartmentName = storeElem.Value;
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
                            case "tareWeight":
                                TareWeight = Convert.ToInt32(storeElem.Value, XmlCulture);
                                break;
                            case "created":
                                Created = Conversion.XmlStringToDateTime(storeElem.Value);
                                break;
                            case "modified":
                                Modified = Conversion.XmlStringToDateTime(storeElem.Value);
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
                    new XElement("retailDepartmentId", RetailDepartmentID),
                    new XElement("retailDepartmentName", RetailDepartmentName),
                    new XElement("itemSalesTaxGroupId", (string)ItemSalesTaxGroupId),
                    new XElement("itemSalesTaxGroupName", ItemSalesTaxGroupName),
                    new XElement("profitMargin", ProfitMargin.ToString(XmlCulture)),
                    new XElement("validationPeriod", ValidationPeriod),
                    new XElement("validationPeriodDescription", ValidationPeriodDescription),
                    new XElement("tareWeight", TareWeight),
                    new XElement("created", Conversion.ToXmlString(Created)),
                    new XElement("modified", Conversion.ToXmlString(Modified)));
            return xml;
        }

        protected override void PropertyChanged(string columnName, object value)
        {
            base.PropertyChanged(columnName, value);

            AddColumnInfo("MODIFIED");
        }
    }
}