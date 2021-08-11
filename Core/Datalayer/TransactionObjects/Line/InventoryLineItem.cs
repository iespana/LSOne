using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line
{
    /// <summary>
    /// A inventory item used for the inventory transaction.
    /// </summary>
    [Serializable]
    public class InventoryLineItem : LineItem
    {
        public const string XmlElementName = "InventoryLineItem";

        #region Enums
        public enum ItemEntryType
        {
            /// <summary>
            /// 0, Them item was scanned.
            /// </summary>
            Scanned = 0,
            /// <summary>
            /// 1, The item id or barcode was entered into the system
            /// </summary>
            ManuallyEntered = 1, 
            /// <summary>
            /// 2, The item was select from the screen or a speedbutton.
            /// </summary>
            Selected = 2        
        }
        #endregion

        private ItemEntryType entryType;            //The method of item entry, scanned,keyboard,selected
        //Item 
        private string itemId;                      //The item id as stored in the ERP system
        private string barcodeId;                   //The barcode as stored in the ERP system
        private string itemGroupId;                 //The item group
        private decimal quantity;                   //The quantity counted

        /// <summary>
        /// The method of item entry, scanned,keyboard,selected
        /// </summary>
        public ItemEntryType EntryType
        {
            get { return entryType; }
            set { entryType = value; }
        }
        /// <summary>
        /// The item id as stored in the ERP system.
        /// </summary>
        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }
        /// <summary>
        /// The barcode as stored in the ERP system.
        /// </summary>
        public string BarcodeId
        {
            get { return barcodeId; }
            set { barcodeId = value; }
        }
        /// <summary>
        /// //The item group for the item.
        /// </summary>
        public string ItemGroupId
        {
            get { return itemGroupId; }
            set { itemGroupId = value; }
        }
        /// <summary>
        /// The quantity counted.
        /// </summary>
        public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public void Populate(InventoryLineItem item)
        {
            base.Populate(item);
            item.entryType = entryType;
            item.itemId = itemId;
            item.barcodeId = barcodeId;
            item.itemGroupId = itemGroupId;
            item.quantity = quantity;
        }
        
        public override object Clone()
        {
            InventoryLineItem item = new InventoryLineItem();
            Populate(item);
            return item;
        }
        
        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                /*
                * Strings      added as is
                * Int          added as is
                * Bool         added as is
                * 
                * Decimal      added with ToString()
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString(),

                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xInventory = new XElement(XmlElementName,
                    new XElement("entryType", (int)entryType),
                    new XElement("itemId", itemId),
                    new XElement("barcodeId", barcodeId),
                    new XElement("itemGroupId", itemGroupId),
                    new XElement("quantity", quantity.ToString())
                );

                xInventory.Add(base.ToXML());
                return xInventory; 
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "InventoryLineItem.ToXml", ex);
                }

                throw ex;
            }
        }

        public override void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> classVariables = xItem.Elements();
                    foreach (XElement xVariable in classVariables)
                    {
                        if (!xVariable.IsEmpty)
                        {
                            try
                            {
                                switch (xVariable.Name.ToString())
                                {
                                    case "entryType":
                                        entryType = (ItemEntryType)Convert.ToInt32(xVariable.Value.ToString());
                                        break;
                                    case "itemId":
                                        itemId = xVariable.Value.ToString();
                                        break;
                                    case "barcodeId":
                                        barcodeId = xVariable.Value.ToString();
                                        break;
                                    case "itemGroupId":
                                        itemGroupId = xVariable.Value.ToString();
                                        break;
                                    case "quantity":
                                        quantity = Convert.ToDecimal(xVariable.Value.ToString());
                                        break;
                                    default:
                                        base.ToClass(xVariable);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "InventoryLineItem:" + xVariable.Name.ToString(), ex);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "InventoryLineItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
