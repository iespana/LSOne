using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.Services.Datalayer.BusinessObjects
{
    /// <summary>
    /// Data entity class for a retail department.
    /// </summary>
    public class OldRetailDepartment : DataEntity
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

        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public OldRetailDepartment() 
            : base()
        {
            NameAlias = "";
            RetailDivisionID = "";
        }

        /// <summary>
        /// A search alias for the retail department
        /// </summary>
        public string NameAlias { get; set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier RetailDepartmentId { get; set; }
    
        /// <summary>
        /// The description of the retail department name
        /// </summary>
        public string RetailDepartmentName { get; internal set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier RetailDivisionID { get; set; }

        public string RetailDivisionName { get; set; }

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
                    new XElement("nameAlias", NameAlias));
            return xml;
        }
    }
}
