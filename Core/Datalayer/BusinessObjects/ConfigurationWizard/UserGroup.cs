using System;
using System.Xml.Linq;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard
{
    /// <summary>
    /// Business entity class of POSUsers page
    /// </summary>
    public class UserGroup : DataEntity
    {
        public UserGroup()
        {
            PermissionGroupID = string.Empty;
        }

        /// <summary>
        /// Selected permission group id
        /// </summary>
        public string PermissionGroupID { get; set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var currencyElements = element.Elements();
            foreach (XElement current in currencyElements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "ID":
                                ID = current.Value;
                                break;
                            case "Text":
                                Text = current.Value;
                                break;
                            case "GroupGuid":
                                PermissionGroupID = current.Value;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Creates an xml element from all the variables in the POSUsers class
        /// </summary>
        /// <param name="errorLogger"></param>
        /// <returns>An XML element</returns>
        public override XElement ToXML(IErrorLog errorLogger = null)
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
            XElement userGroup = new XElement("UserGroup",
                            new XElement("ID", ID),
                            new XElement("Text", Text),
                            new XElement("GroupGuid", PermissionGroupID));

            return userGroup;
        }
    }
}
