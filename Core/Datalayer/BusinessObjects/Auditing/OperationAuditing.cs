using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Auditing
{
    [Serializable]
    public class OperationAuditing : ICloneable
    {
        public const string XmlElementName = "OperationAudit";

        private const string Test = "test";
        public OperationAuditing()
        {
            UniqueID = Guid.Empty;
            TransactionID = RecordIdentifier.Empty;
            StoreID = RecordIdentifier.Empty;
            TerminalID = RecordIdentifier.Empty;
            LineNum = 0;
            UserID = RecordIdentifier.Empty;
            ManagerID = RecordIdentifier.Empty;
            OperationID = POSOperations.NoOperation;
            CreatedDate = DateTime.Now;
        }

        #region Properties

        /// <summary>
        /// A unique ID for the entry
        /// </summary>
        public Guid UniqueID { get; set; }
        /// <summary>
        /// The transaction ID of the loyalty points
        /// </summary>
        public RecordIdentifier TransactionID { get; set; }
        /// <summary>
        /// The Store ID where the points were created
        /// </summary>
        public RecordIdentifier StoreID { get; set; }
        /// <summary>
        /// The Terminal ID where the points were created
        /// </summary>
        public RecordIdentifier TerminalID { get; set; }
        
        /// <summary>
        /// Line number of sale line, if any
        /// </summary>
        public int LineNum { get; set; }
        
        /// <summary>
        /// The id of user logged on to the POS
        /// </summary>
        public RecordIdentifier UserID { get; set; }
		
        /// <summary>
        /// The id of the manager/user that override user permissions to allow the operation to run
        /// </summary>
        public RecordIdentifier ManagerID { get; set; }
		
        /// <summary>
        /// The id of the operation that resulted in the audit
        /// </summary>
        public POSOperations OperationID { get; set; }

        public string OperationName { get; set; }

		/// <summary>
        /// The date when the audit was generated
        /// </summary>
        public DateTime CreatedDate { get; set; }
        #endregion

        public virtual object Clone()
        {
            var item = new OperationAuditing();
            Populate(item);
            return item;
        }

        protected void Populate(OperationAuditing item)
        {
            item.UniqueID = UniqueID;
            item.TransactionID = TransactionID;
            item.StoreID = StoreID;
            item.TerminalID = TerminalID;
            item.LineNum = LineNum;
            item.UserID = UserID;
            item.ManagerID = ManagerID;
            item.OperationID = OperationID;
            item.CreatedDate = CreatedDate;
            item.OperationName = OperationName;
        }

        public XElement ToXML(IErrorLog errorLogger = null)
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

                var xAudit = new XElement(XmlElementName,
                    new XElement("uniqueID", Conversion.ToXmlString(UniqueID)),
                    new XElement("transactionID", TransactionID),
                    new XElement("storeID", StoreID),
                    new XElement("terminalID", TerminalID),
                    new XElement("lineNum", Conversion.ToXmlString(LineNum)),
                    new XElement("userID", UserID),
                    new XElement("managerID", ManagerID),
                    new XElement("operationID", Conversion.ToXmlString((int)OperationID)),
                    new XElement("operationName", OperationName),
                    new XElement("createdDate", Conversion.ToXmlString(CreatedDate))
                );

                return xAudit;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }
        }

        public void ToClass(XElement xAudit, IErrorLog errorLogger = null)
        {           
            try
            {
                if (xAudit.HasElements)
                {
                    var auditElements = xAudit.Elements();
                    foreach (var elem in auditElements)
                    {                        
                        if (!elem.IsEmpty)
                        {
                            try
                            {
                                switch (elem.Name.ToString())
                                {
                                    case "uniqueID":
                                        UniqueID = Conversion.XmlStringToGuid(elem.Value);
                                        break;
                                    case "transactionID":
                                        TransactionID = elem.Value;
                                        break;
                                    case "storeID":
                                        StoreID = elem.Value;
                                        break;
                                    case "terminalID":
                                        TerminalID = elem.Value;
                                        break;
                                    case "lineNum":
                                        LineNum = Conversion.XmlStringToInt(elem.Value);
                                        break;
                                    case "userID":
                                        UserID = elem.Value;
                                        break;
                                    case "managerID":
                                        ManagerID = elem.Value;
                                        break;
                                    case "operationID":
                                        try
                                        {
                                            OperationID = (POSOperations)Conversion.XmlStringToInt(elem.Value);
                                        }
                                        catch
                                        {
                                            POSOperations oper;
                                            Enum.TryParse(elem.Value, false, out oper);
                                            OperationID = oper;
                                        }
                                        break;
                                    case "operationName":
                                        OperationName = elem.Value;
                                        break;
                                    case "createdDate":
                                        CreatedDate = Conversion.XmlStringToDateTime(elem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, elem.Name.ToString(), ex);
                            }
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, xAudit.Name.ToString(), ex);
            }
        }
    }
}

