using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Data entity class for a retail department.
    /// </summary>
    public class RetailDepartment : OptimizedUpdateDataEntity
    {
        /// <summary>
        /// Used to determine how to sort this class in a list.
        /// </summary>
        public enum SortEnum
        {
            /// <summary>
            /// Sort by retail department
            /// </summary>
            RetailDepartment,
            /// <summary>
            /// Sort by description
            /// </summary>
            Description,
        }

        private RecordIdentifier retailDivisionID;
        private string nameAlias;
        private RecordIdentifier retailDivisionMasterID;
        private RecordIdentifier masterID;

        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public RetailDepartment() 
            : base()
        {
            nameAlias = "";
            retailDivisionID = RecordIdentifier.Empty;
            retailDivisionMasterID = RecordIdentifier.Empty;
        }

        /// <summary>
        /// A search alias for the retail department
        /// </summary>
        public string NameAlias
        {
            get { return nameAlias; }
            set
            {
                if (nameAlias != value)
                {
                    nameAlias = value;
                    PropertyChanged("NAMEALIAS", value);
                }
            }
        }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier RetailDepartmentId { get; set; }
    
        /// <summary>
        /// The description of the retail department name
        /// </summary>
        public string RetailDepartmentName { get; internal set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier RetailDivisionID
        {
            get { return retailDivisionID; }
            set
            {
                if (retailDivisionID != value)
                {
                    retailDivisionID = value;
                    PropertyChanged("DIVISIONID", value);
                }
            }
        }

        public string RetailDivisionName { get; set; }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier RetailDivisionMasterID
        {
            get { return retailDivisionMasterID; }
            set
            {
                if (retailDivisionMasterID != value)
                {
                    retailDivisionMasterID = value;
                    PropertyChanged("DIVISIONMASTERID", value);
                }
            }
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier MasterID
        {
            get { return masterID; }
            set
            {
                masterID = value;
            }
        }

        [StringLength(60)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
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

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            // Call this to support older serialization from when this class didn't override ToClass
            base.ToClass(element, errorLogger);

            var currencyElements = element.Elements();
            foreach (XElement storeElem in currencyElements)
            {
                if (!storeElem.IsEmpty)
                {
                    try
                    {
                        switch (storeElem.Name.ToString())
                        {
                            case "departmentID":
                                RetailDepartmentId = storeElem.Value;
                                break;
                            case "departmentName":
                                RetailDepartmentName = storeElem.Value;
                                break;
                            case "divisionID":
                                RetailDivisionID = storeElem.Value;
                                break;
                            case "nameAlias":
                                NameAlias = storeElem.Value;
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
            var xml = new XElement("RetailDepartment",
                    new XElement("departmentID", (string)ID),
                    new XElement("departmentName", Text),
                    new XElement("divisionID", RetailDivisionID),
                    new XElement("nameAlias", NameAlias),
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