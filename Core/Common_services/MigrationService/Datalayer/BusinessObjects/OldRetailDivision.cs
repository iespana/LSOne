#if !MONO
#endif
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.Services.Datalayer.BusinessObjects
{
    /// <summary>
    /// Data entity class for a retail division
    /// </summary>
    public class OldRetailDivision : DataEntity
    {
        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public OldRetailDivision()
            : base()
        {
        }

        /// <summary>
        /// The unique ID of the retail group
        /// </summary>
        [RecordIdentifierValidation(20)]
        public override RecordIdentifier ID
        {
            get {return base.ID;}
            set {base.ID = value;}
        }

        /// <summary>
        /// The description of the retail group
        /// </summary>
        [StringLength(60)]
        public override string Text
        {
            get {return base.Text;}
            set {base.Text = value;}
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
                    new XElement("name", Text));
            return xml;
        }
    }
}
 