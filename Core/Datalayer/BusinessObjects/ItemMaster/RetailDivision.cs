#if !MONO
#endif
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Data entity class for a retail division
    /// </summary>
    public class RetailDivision : OptimizedUpdateDataEntity
    {
        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public RetailDivision()
            : base()
        {
        }

        /// <summary>
        /// The unique ID of the retail division
        /// </summary>
        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get { return base.ID; }
            set { base.ID = value; }
        }

        /// <summary>
        /// The description of the retail division
        /// </summary>
        [StringLength(60)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    PropertyChanged("NAME", value);
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

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier MasterID
        {
            get;
            set;
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
                            case "divisionID":
                                ID = storeElem.Value;
                                break;
                            case "name":
                                Text = storeElem.Value;
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
                            errorLogger.LogMessage(LogMessageType.Error,
                                                   storeElem.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            var xml = new XElement("RetailDivision",
                    new XElement("divisionID", (string)ID),
                    new XElement("name", Text),
                    new XElement("created", Conversion.ToXmlString(Created)),
                    new XElement("modified", Conversion.ToXmlString(Modified)));
            return xml;
        }

        protected override void PropertyChanged(string columnName, object value = null)
        {
            base.PropertyChanged(columnName, value);

            AddColumnInfo("MODIFIED");
        }
    }
} 