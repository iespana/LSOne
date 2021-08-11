using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.CustomerOrder
{
    [Serializable]
    public class OrderItem : IOrderItem
    {
        private string XmlElementName;

        public decimal Ordered { get; set; }
        public  decimal Received { get; set; }
        public decimal ToPickUp { get; set; }
        public List<IDepositItem> Deposits { get; set; }
        public bool FullyReceived { get; set; }
        public Date DateFullyReceived { get; set; }
        public decimal ReservationQty { get; set; }
        public Date DateReserved { get; set; }
        public bool ReservationDone { get; set; }
        public RecordIdentifier JournalID { get; set; }
        public Guid SplitIdentifier { get; set; }

        public OrderItem()
        {
            XmlElementName = "OrderItem";
            Clear();
        }

        public void Clear()
        {
            Ordered = decimal.Zero;
            Received = decimal.Zero;
            ToPickUp = decimal.Zero;
            Deposits = new List<IDepositItem>();
            FullyReceived = false;
            ReservationQty = decimal.Zero;
            DateFullyReceived = Date.Empty;
            ReservationDone = false;
            DateReserved = Date.Empty;
            JournalID = RecordIdentifier.Empty;
            SplitIdentifier = Guid.Empty;
        }

        public bool Empty()
        {
            return Ordered == decimal.Zero && ToPickUp == decimal.Zero;
        }

        public decimal DepositAlreadyPaid()
        {
            return DepositAlreadyPaid(DepositsStatus.Normal) +
                   DepositAlreadyPaid(DepositsStatus.Distributed);
        }

        public decimal DepositAlreadyPaid(DepositsStatus status)
        {
            return Deposits.Where(w => w.DepositPaid && w.Status == status).Sum(s => s.Deposit);
        }

        public decimal DepositToBePaid()
        {
            return DepositToBePaid(DepositsStatus.Normal) +
                   DepositToBePaid(DepositsStatus.Distributed);
        }

        public decimal DepositToBePaid(DepositsStatus status)
        {
            return Deposits.Where(w => !w.DepositPaid && w.Status == status).Sum(s => s.Deposit);
        }

        public bool NewDepositsToBePaid()
        {
            if (Deposits.Count == 0)
            {
                return true;
            }

            decimal alreadyPaid = DepositAlreadyPaid(DepositsStatus.Normal);
            decimal toBePaid = DepositToBePaid(DepositsStatus.Normal);

            return alreadyPaid < toBePaid;
        }
        
        
        public void SetAllDepositsAsPaid()
        {
            foreach (IDepositItem deposit in Deposits.Where(w => w.Status == DepositsStatus.Normal))
            {
                deposit.DepositPaid = true;
            }
        }

        public void SetAllDepositsStatus(DepositsStatus status)
        {
            foreach (IDepositItem deposit in Deposits)
            {
                deposit.Status = status;
            }
        }

        public decimal TotalDepositAmount()
        {
            return Deposits.Where(w => w.Status == DepositsStatus.Normal || w.Status == DepositsStatus.Distributed).Sum(s => s.Deposit);
        }

        public void SetDeposit(decimal deposit)
        {
            DepositItem item = new DepositItem(deposit);
            Deposits.Add(item);
        }

        public void SetDeposit(decimal deposit, bool depositPaid) 
        {
            DepositItem item = new DepositItem(deposit, depositPaid, DepositsStatus.Normal); 
            Deposits.Add(item);
        }

        public object Clone()
        {
            OrderItem item = new OrderItem();
            Populate(item);
            return item;
        }

        private void Populate(OrderItem item)
        {
            item.Ordered = Ordered;
            item.Received = Received;
            item.ToPickUp = ToPickUp;
            item.Deposits = CloneDeposits();
            item.FullyReceived = FullyReceived;
            item.ReservationQty = ReservationQty;
            item.DateFullyReceived = DateFullyReceived;
            item.ReservationDone = ReservationDone;
            item.DateReserved = DateReserved;
            item.JournalID = JournalID;
            item.SplitIdentifier = SplitIdentifier;
        }

        
        private List<IDepositItem> CloneDeposits()
        {
            var cloneItems = new List<IDepositItem>();

            foreach (IDepositItem item in Deposits)
            {
                cloneItems.Add((DepositItem)item.Clone());
            }

            return cloneItems;
        }

        public void ToClass(XElement xElement, IErrorLog errorLogger = null)
        {
            try
            {
                if (xElement.HasElements)
                {
                    var elements = xElement.Elements(XmlElementName);
                    foreach (XElement order in elements)
                    {
                        if (order.HasElements)
                        {
                            IEnumerable<XElement> orderVariables = order.Elements();
                            foreach (XElement elem in orderVariables)
                            {
                                if (!elem.IsEmpty)
                                {
                                    try
                                    {
                                        switch (elem.Name.ToString())
                                        {
                                            case "Received":
                                                Received = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "Ordered":
                                                Ordered = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "ToPickUp":
                                                ToPickUp = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "ReservationQty":
                                                ReservationQty = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "DepositLines":
                                                Deposits = CreateDepositLines(elem);
                                                break;
                                            case "FullyReceived":
                                                FullyReceived = Conversion.XmlStringToBool(elem.Value);
                                                break;
                                            case "JournalID":
                                                JournalID = elem.Value;
                                                break;
                                            case "DateFullyReceived":
                                                DateFullyReceived = new Date(Conversion.XmlStringToDateTime(elem.Value));
                                                break;
                                            case "DateReserved":
                                                DateReserved = new Date(Conversion.XmlStringToDateTime(elem.Value));
                                                break;
                                            case "ReservationDone":
                                                ReservationDone = Conversion.XmlStringToBool(elem.Value);
                                                break;
                                            case "SplitIdentifier":
                                                SplitIdentifier = Conversion.XmlStringToGuid(elem.Value);
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
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, xElement.Name.ToString(), ex);
            }
        }

        public List<IDepositItem> CreateDepositLines(XElement xItems)
        {
            List<IDepositItem> depositLines = new List<IDepositItem>();

            if (xItems.HasElements)
            {
                IEnumerable<XElement> xDeposits = xItems.Elements();
                foreach (XElement xDep in xDeposits)
                {
                    if (xDep.HasElements)
                    {
                        DepositItem item = new DepositItem();
                        item.ToClass(xDep);
                        depositLines.Add(item);
                    }
                }
            }

            return depositLines;
        }

        public XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                var xItem = new XElement(XmlElementName,
                    new XElement("Received", Conversion.ToXmlString(Received)),
                    new XElement("Ordered", Conversion.ToXmlString(Ordered)),
                    new XElement("ToPickUp", Conversion.ToXmlString(ToPickUp)),
                    new XElement("ReservationQty", Conversion.ToXmlString(ReservationQty)),
                    new XElement("FullyReceived", Conversion.ToXmlString(FullyReceived)),
                    new XElement("ReservationDone", Conversion.ToXmlString(ReservationDone)),
                    new XElement("JournalID", JournalID),
                    new XElement("DateReserved", Conversion.ToXmlString(DateReserved.DateTime)),
                    new XElement("DateFullyReceived", Conversion.ToXmlString(DateFullyReceived.DateTime)),
                    new XElement("SplitIdentifier", Conversion.ToXmlString(SplitIdentifier))
                );

                #region Deposits
                XElement xDeposits = new XElement("DepositLines");
                foreach (DepositItem item in Deposits)
                {
                    xDeposits.Add(item.ToXML());
                }
                xItem.Add(xDeposits);

                #endregion

                return xItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }
        }
    }
}
