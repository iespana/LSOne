using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.CustomerOrders
{
    public class CustomerOrderItem : DataEntity
    {
        public const string XmlElementName = "CustomerOrderItem";

        /// <summary>
        /// The type of order this is <see cref="CustomerOrderType"/>
        /// </summary>
        public CustomerOrderType OrderType { get; set; }

        /// <summary>
        /// If a quote is being converted to a customer order then this is set while the 
        /// quote has not been saved as a customer order to head office
        /// </summary>
        public CustomerOrderType ConvertedFrom { get; set; }

        /// <summary>
        /// The minimum deposit required according to configurations and what items are on the order
        /// </summary>
        public decimal MinimumDeposit { get; set; }

        /// <summary>
        /// The amount to be paid now - if the order has been retrieved and no items have been added this value would be zero.
        /// </summary>
        public decimal DepositToBePaid { get; set; }

        /// <summary>
        /// The total deposit paid untill now
        /// </summary>
        public decimal PaidDeposit { get; set; }

        /// <summary>
        /// If a deposit for an item has already been paid and then the item is being voided then this value will tell the POS how much to pay the customer back
        /// </summary>
        public decimal DepositToBeReturned { get; set; }

        /// <summary>
        /// When calculating the deposit each item is given a deposit and that value is then rounded. This can create a rounding difference between the total on the order and each item
        /// </summary>
        public decimal RoundingDifference { get; set; }

        /// <summary>
        /// If true then the user has changed the calculated deposit
        /// </summary>
        public bool DepositOverriden { get; set; }
        /// <summary>
        /// If true the user has added an additional payment
        /// </summary>
        public bool HasAdditionalPayment { get; set; }

        public decimal AdditionalPayment { get; set; }

        public RecordIdentifier Reference { get; set; }

        public CustomerOrderAdditionalConfigurations Delivery { get; set; }

        public CustomerOrderAdditionalConfigurations Source { get; set; }

        public Date ExpirationDate { get; set; }

        public RecordIdentifier DeliveryLocation { get; set; }
        
        public string DeliveryLocationText { get; set; }

        public string Comment { get; set; }

        public CustomerOrderStatus Status { get; set; }

        public CustomerOrderAction CurrentAction { get; set; }

        public bool UpdateStock { get; set; }

        public List<PaymentItem> AdditionalPaymentLines { get; set; }

        public RecordIdentifier CreatedAtStoreID { get; set; }

        public RecordIdentifier CreatedAtTerminalID { get; set; }

        public RecordIdentifier CreatedByStaffID { get; set; }

        public CustomerOrderItem()
        {
            Delivery = new CustomerOrderAdditionalConfigurations();
            Source = new CustomerOrderAdditionalConfigurations();
            AdditionalPaymentLines = new List<PaymentItem>();
            Clear();
        }

        public CustomerOrderItem(CustomerOrderType orderType) : this()
        {
            OrderType = orderType;
        }

        public void Clear()
        {
            OrderType = CustomerOrderType.None;
            ConvertedFrom = CustomerOrderType.None;
            MinimumDeposit = decimal.Zero;
            DepositToBePaid = decimal.Zero;
            PaidDeposit = decimal.Zero;
            DepositToBeReturned = decimal.Zero;
            RoundingDifference = decimal.Zero;
            DepositOverriden = false;
            HasAdditionalPayment = false;
            AdditionalPayment = decimal.Zero;
            Delivery.Clear();
            Source.Clear();
            ExpirationDate = Date.Now;
            DeliveryLocation = RecordIdentifier.Empty;
            Reference = RecordIdentifier.Empty;
            Comment = string.Empty;
            UpdateStock = true;
            Status = CustomerOrderStatus.New;
            CurrentAction = CustomerOrderAction.None;
            DeliveryLocationText = string.Empty;
            AdditionalPaymentLines.Clear();
            CreatedAtStoreID = RecordIdentifier.Empty;
            CreatedAtTerminalID = RecordIdentifier.Empty;
            CreatedByStaffID = RecordIdentifier.Empty;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public override object Clone()
        {
            var item = new CustomerOrderItem();
            base.Populate(item);
            Populate(item);
            return item;
        }

        public bool Empty()
        {
            return OrderType == CustomerOrderType.None;
        }

        public void Populate(CustomerOrderItem item)
        {
            item.OrderType = OrderType;
            item.ConvertedFrom = ConvertedFrom;
            item.MinimumDeposit = MinimumDeposit;
            item.DepositToBePaid = DepositToBePaid;
            item.PaidDeposit = PaidDeposit;
            item.DepositToBeReturned = DepositToBeReturned;
            item.RoundingDifference = RoundingDifference;
            item.Delivery = Delivery;
            item.Source = Source;
            item.ExpirationDate = ExpirationDate;
            item.DeliveryLocation = DeliveryLocation;
            item.Reference = Reference;
            item.Comment = Comment;
            item.DepositOverriden = DepositOverriden;
            item.AdditionalPayment = AdditionalPayment;
            item.HasAdditionalPayment = HasAdditionalPayment;
            item.Status = Status;
            item.CurrentAction = CurrentAction;
            item.DeliveryLocationText = DeliveryLocationText;
            item.UpdateStock = UpdateStock;
            item.AdditionalPaymentLines = CloneAdditionalPayments();
            item.CreatedAtStoreID = CreatedAtStoreID;
            item.CreatedAtTerminalID = CreatedAtTerminalID;
            item.CreatedByStaffID = CreatedByStaffID;
        }

        private List<PaymentItem> CloneAdditionalPayments()
        {
            return AdditionalPaymentLines.Select(item => (PaymentItem) item.Clone()).ToList();
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                var xCustomerOrder = new XElement(XmlElementName,
                    new XElement("OrderType", Conversion.ToXmlString((int)OrderType)),
                    new XElement("ConvertedFrom", Conversion.ToXmlString((int)ConvertedFrom)),
                    new XElement("MinimumDeposit", Conversion.ToXmlString(MinimumDeposit)),
                    new XElement("DepositToBePaid", Conversion.ToXmlString(DepositToBePaid)),
                    new XElement("PaidDeposit", Conversion.ToXmlString(PaidDeposit)),
                    new XElement("DepositToBeReturned", Conversion.ToXmlString(DepositToBeReturned)),
                    new XElement("RoundingDifference", Conversion.ToXmlString(RoundingDifference)),
                    new XElement("DeliveryLocation", (string)DeliveryLocation),
                    new XElement("Reference", (string)Reference),
                    new XElement("Source", Source.ToXML()),
                    new XElement("Delivery", Delivery.ToXML()),
                    new XElement("ExpirationDate", Conversion.ToXmlString(ExpirationDate.DateTime)),
                    new XElement("Comment", Comment),
                    new XElement("DeliveryLocationText", DeliveryLocationText),
                    new XElement("Status", Conversion.ToXmlString((int)Status)),
                    new XElement("CurrentAction", Conversion.ToXmlString((int)CurrentAction)),
                    new XElement("DepositOverriden", Conversion.ToXmlString(DepositOverriden)),
                    new XElement("HasAdditionalPayment", Conversion.ToXmlString(HasAdditionalPayment)),
                    new XElement("AdditionalPayment", Conversion.ToXmlString(AdditionalPayment)),
                    new XElement("CreatedAtStoreID", (string)CreatedAtStoreID),
                    new XElement("CreatedAtTerminalID", (string)CreatedAtTerminalID),
                    new XElement("CreatedByStaff", (string)CreatedByStaffID),
                    new XElement("UpdateStock", Conversion.ToXmlString(UpdateStock))

                );

                #region Additional payments
                XElement xPayments = new XElement("AdditionalPaymentLines");
                foreach (PaymentItem item in AdditionalPaymentLines)
                {
                    xPayments.Add(item.ToXML());
                }
                xCustomerOrder.Add(xPayments);

                #endregion

                xCustomerOrder.Add(base.ToXML(errorLogger));
                return xCustomerOrder;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, ex.Message, ex);
                throw;
            }
        }

        public override void ToClass(XElement xCustomerOrder, IErrorLog errorLogger = null)
        {
            try
            {
                if (xCustomerOrder.HasElements)
                {
                    var orderElements = xCustomerOrder.Elements(XmlElementName);
                    foreach (XElement order in orderElements)
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
                                            case "OrderType":
                                                OrderType = (CustomerOrderType)Conversion.XmlStringToInt(elem.Value);
                                                break;
                                            case "ConvertedFrom":
                                                ConvertedFrom = (CustomerOrderType)Conversion.XmlStringToInt(elem.Value);
                                                break;
                                            case "Status":
                                                Status = (CustomerOrderStatus)Conversion.XmlStringToInt(elem.Value);
                                                break;
                                            case "CurrentAction":
                                                CurrentAction = (CustomerOrderAction)Conversion.XmlStringToInt(elem.Value);
                                                break;
                                            case "MinimumDeposit":
                                                MinimumDeposit = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "DepositToBePaid":
                                                DepositToBePaid = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "PaidDeposit":
                                                PaidDeposit = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "DepositToBeReturned":
                                                DepositToBeReturned = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "RoundingDifference":
                                                RoundingDifference = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "DeliveryLocation":
                                                DeliveryLocation = elem.Value;
                                                break;
                                            case "Reference":
                                                Reference = elem.Value;
                                                break;
                                            case "Source":
                                                Source.ToClass(elem);
                                                break;
                                            case "Delivery":
                                                Delivery.ToClass(elem);
                                                break;
                                            case "Comment":
                                                Comment = elem.Value;
                                                break;
                                            case "DeliveryLocationText":
                                                DeliveryLocationText = elem.Value;
                                                break;
                                            case "ExpirationDate":
                                                ExpirationDate = new Date(Conversion.XmlStringToDateTime(elem.Value));
                                                break;
                                            case "DepositOverriden":
                                                DepositOverriden = Conversion.XmlStringToBool(elem.Value);
                                                break;
                                            case "HasAdditionalPayment":
                                                HasAdditionalPayment = Conversion.XmlStringToBool(elem.Value);
                                                break;
                                            case "AdditionalPayment":
                                                AdditionalPayment = Conversion.XmlStringToDecimal(elem.Value);
                                                break;
                                            case "UpdateStock":
                                                UpdateStock = Conversion.XmlStringToBool(elem.Value);
                                                break;
                                            case "AdditionalPaymentLines":
                                                AdditionalPaymentLines = CreateAdditionalPaymentLines(elem);
                                                break;
                                            case "CreatedAtStoreID":
                                                CreatedAtStoreID = elem.Value;
                                                break;
                                            case "CreatedAtTerminalID":
                                                CreatedAtTerminalID = elem.Value;
                                                break;
                                            case "CreatedByStaff":
                                                CreatedByStaffID = elem.Value;
                                                break;
                                            default:
                                                base.ToClass(elem);
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        errorLogger?.LogMessage(LogMessageType.Error, elem.Name.ToString(), ex);
                                    }
                                }
                            }

                            if (ID.DBType == SqlDbType.NVarChar && !string.IsNullOrEmpty(ID.StringValue))
                            {
                                ID = new Guid((string)ID);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, xCustomerOrder.Name.ToString(), ex);
            }
        }

        public List<PaymentItem> CreateAdditionalPaymentLines(XElement xPayments)
        {
            List<PaymentItem> paymentLines = new List<PaymentItem>();

            if (xPayments.HasElements)
            {
                IEnumerable<XElement> xDeposits = xPayments.Elements();
                foreach (XElement xDep in xDeposits)
                {
                    if (xDep.HasElements)
                    {
                        PaymentItem payment = new PaymentItem();
                        payment.ToClass(xDep);
                        paymentLines.Add(payment);
                    }
                }
            }

            return paymentLines;
        }
    }
}
