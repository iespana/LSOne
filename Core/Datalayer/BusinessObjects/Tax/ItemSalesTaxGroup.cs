using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Tax")]
namespace LSOne.DataLayer.BusinessObjects.Tax
{
    /// <summary>
    /// Item sales tax groups are attached to items and contains tax codes (0..*). See TaxCodeInItemSalesTaxGroup class for tax codes in item sales tax group.
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class ItemSalesTaxGroup : OptimizedUpdateDataEntity
    {
        /// <summary>
        /// Used to determine how to sort item sales tax groups in a list.
        /// </summary>
        public enum SortEnum
        {
            /// <summary>
            /// Sort by ID (Column TAXITEMGROUP)
            /// </summary>
            ID,
            /// <summary>
            /// Sort by description (Column NAME)
            /// </summary>
            Description
        }

        private string receiptDisplay;

        public ItemSalesTaxGroup()
            : base()
        {            
            Initialize();
        }

        protected sealed override void Initialize()
        {
            receiptDisplay = "";
        }


        /// <summary>
        /// The description of the item sales tax group
        /// </summary>
        [DataMember]
        [StringLength(60)]
        public override string Text
        {
            get { return base.Text; }
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
        /// How item sales tax groups are displayed on receipts. This is f.x. displayed when you display tax information for an item on a customers receipt.
        /// </summary>
        [DataMember]
        public string ReceiptDisplay
        {
            get { return receiptDisplay; }
            set
            {
                if (receiptDisplay != value)
                {
                    PropertyChanged("RECEIPTDISPLAY", value);
                    receiptDisplay = value;
                }
            }
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
                                case "taxGroupID":
                                    ID = storeElem.Value;
                                    break;
                                case "taxGroupName":
                                    Text = storeElem.Value;
                                    break;
                                case "receiptDisplay":
                                    receiptDisplay = storeElem.Value;
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
            XElement xml = new XElement("itemSalesTaxGroup",
                        new XElement("taxGroupID", ID),
                        new XElement("taxGroupName", Text),
                        new XElement("receiptDisplay", ReceiptDisplay)
                    );
            return xml;
        }

        public override object Clone()
        {
            var o = new ItemSalesTaxGroup();
            Populate(o);
            return o;
        }

        protected void Populate(ItemSalesTaxGroup o)
        {
            o.ID = (RecordIdentifier)ID.Clone();
            o.Text = Text;
            o.ReceiptDisplay = ReceiptDisplay;
        }
    }
}
