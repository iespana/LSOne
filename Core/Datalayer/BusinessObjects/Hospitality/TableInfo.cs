using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.KDSBusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class TableInfo
    {
        public TableInfo()
        {
            Clear();
            
        }

        [DataMember]
        public int TableID { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DiningTableStatus DiningTableStatus { get; set; }
        [DataMember]
        public string TransactionXML { get; set; }
        [DataMember]
        public int NumberOfGuests { get; set; }
        [DataMember]
        public string TerminalID { get; set; }
        [DataMember]
        public string StaffID { get; set; }
        [DataMember]
        public string StoreID { get; set; }
        [DataMember]
        public string SalesType { get; set; }
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public string DiningTableLayoutID { get; set; }
        [DataMember]
        public bool Locked { get; set; }
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier CustomerID { get; set; }
        [DataMember]
        public KitchenOrderStatusEnum KitchenStatus { get; set; }

        public bool Empty()
        {
            return TableID == 0 && string.IsNullOrEmpty(Description) && string.IsNullOrEmpty(TerminalID) && string.IsNullOrEmpty(StoreID) && string.IsNullOrEmpty(StaffID);
        }

        public void Clear()
        {
            TableID = 0;
            Description = "";
            DiningTableStatus = DiningTableStatus.Available;
            TransactionXML = "";
            NumberOfGuests = 0;
            TerminalID = "";
            StaffID = "";
            StoreID = "";
            SalesType = "";
            Sequence = 0;
            DiningTableLayoutID = "";
            Locked = false;
            CustomerID = RecordIdentifier.Empty;
            KitchenStatus = KitchenOrderStatusEnum.None;
        }

        protected void Populate(TableInfo table)
        {
            table.TableID = TableID;
            table.Description = Description;
            table.DiningTableStatus = DiningTableStatus;
            table.TransactionXML = TransactionXML;
            table.NumberOfGuests = NumberOfGuests;
            table.TerminalID = TerminalID;
            table.StaffID = StaffID;
            table.StoreID = StoreID;
            table.SalesType = SalesType;
            table.Sequence = Sequence;
            table.DiningTableLayoutID = DiningTableLayoutID;
            table.Locked = Locked;
            table.CustomerID = CustomerID;
            table.KitchenStatus = KitchenStatus;
        }

        public virtual object Clone()
        {
            TableInfo op = new TableInfo();
            Populate(op);
            return op;
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
                XElement xTableInfo = new XElement("TableInfo",
                    new XElement("TableID", Conversion.ToXmlString(TableID)),
                    new XElement("Description", Description),
                    new XElement("DiningTableStatus", Conversion.ToXmlString((int)DiningTableStatus)),
                    //new XElement("TransactionXML", TransactionXML),
                    new XElement("NumberOfGuests", Conversion.ToXmlString(NumberOfGuests)),
                    new XElement("TerminalID", TerminalID),
                    new XElement("StaffID", StaffID),
                    new XElement("StoreID", StoreID),
                    new XElement("SalesType", SalesType),
                    new XElement("Sequence", Conversion.ToXmlString(Sequence)),
                    new XElement("DiningTableLayoutID", DiningTableLayoutID),
                    new XElement("Locked", Conversion.ToXmlString(Locked)),
                    new XElement("CustomerID", CustomerID),
                    new XElement("KitchenStatus", Conversion.ToXmlString((int)KitchenStatus))
                );

                return xTableInfo;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "TableInfo.ToXML", ex);

                throw;
            }
        }

        public void ToClass(XElement xTableInfo, IErrorLog errorLogger = null)
        {
            try
            {
                if (xTableInfo.HasElements)
                {
                    IEnumerable<XElement> tableElements = xTableInfo.Elements();
                    foreach (XElement tableElem in tableElements)
                    {
                        if (!tableElem.IsEmpty)
                        {
                            try
                            {
                                switch (tableElem.Name.ToString())
                                {
                                    case "TableID":
                                        TableID = Conversion.XmlStringToInt(tableElem.Value);
                                        break;
                                    case "Description":
                                        Description = tableElem.Value;
                                        break;
                                    case "DiningTableStatus":
                                        DiningTableStatus = (DiningTableStatus)Conversion.XmlStringToInt(tableElem.Value);
                                        break;
                                    case "NumberOfGuests":
                                        NumberOfGuests = Conversion.XmlStringToInt(tableElem.Value);
                                        break;
                                    case "TerminalID":
                                        TerminalID = tableElem.Value;
                                        break;
                                    case "StaffID":
                                        StaffID = tableElem.Value;
                                        break;
                                    case "StoreID":
                                        StoreID = tableElem.Value;
                                        break;
                                    case "SalesType":
                                        SalesType = tableElem.Value;
                                        break;
                                    case "Sequence":
                                        Sequence = Conversion.XmlStringToInt(tableElem.Value);
                                        break;
                                    case "DiningTableLayoutID":
                                        DiningTableLayoutID = tableElem.Value;
                                        break;
                                    case "Locked":
                                        Locked = Conversion.XmlStringToBool(tableElem.Value);
                                        break;
                                    case "CustomerID":
                                        CustomerID = tableElem.Value;
                                        break;
                                    case "KitchenStatus":
                                        KitchenStatus = (KitchenOrderStatusEnum)Conversion.XmlStringToInt(tableElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "TableInfo:" + tableElem.Name, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "TableInfo.ToClass", ex);

                throw;
            }
        }

    }
}
