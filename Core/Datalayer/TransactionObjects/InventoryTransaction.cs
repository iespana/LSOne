using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.TransactionObjects.Line;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// Used for a transaction to register a negative inventory adjustments or a physical inventory counting.
    /// </summary>
    [Serializable]
    public class InventoryTransaction : PosTransaction
    {
        public enum InventoryTypes : int
        {
            /// <summary>
            /// 0
            /// </summary>
            NegativeAdjustment = 0,
            /// <summary>
            /// 1
            /// </summary>
            PhysicalInventory = 1
        }

        private InventoryTypes inventoryType;

        public InventoryTypes InventoryType
        {
            get { return inventoryType; }
            set { inventoryType = value; }
        }

        //TODO Þyrftu að vera listar af einföldum itemum - strikanr,vörunr og magn
        /// <summary>
        /// Collection of items that are counted
        /// </summary>
        public List<InventoryLineItem> InventoryItems { get; set; }

        public InventoryTransaction()
        {
            InventoryItems = new List<InventoryLineItem>();
        }

        ~InventoryTransaction()
        {
            InventoryItems.Clear();
        }

        public override void Save()
        {
        }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.Inventory;
        }

        public override object Clone()
        {
            InventoryTransaction transaction = new InventoryTransaction();
            Populate(transaction);
            return transaction;
        }

        protected void Populate(InventoryTransaction transaction)
        {
            base.Populate(transaction);
            transaction.inventoryType = inventoryType;
            transaction.InventoryItems = CollectionHelper.Clone<InventoryLineItem, List<InventoryLineItem>>(InventoryItems);
        }

        public void Add(InventoryLineItem inventoryItems)
        {
            this.EndDateTime = DateTime.Now;
            this.InventoryItems.Add(inventoryItems);
        }

        public List<InventoryLineItem> CreateInventoryItems(XElement xItems)
        {
            var inventoryItems = new List<InventoryLineItem>();

            if (xItems.HasElements)
            {
                var xInventoryItems = xItems.Elements();
                foreach (var xItem in xInventoryItems)
                {
                    if (xItem.HasElements)
                    {
                        switch (xItem.Name.ToString())
                        {
                            case InventoryLineItem.XmlElementName:
                                var ili = new InventoryLineItem();
                                ili.ToClass(xItem);
                                inventoryItems.Add(ili);
                                break;
                        }
                    }
                }
            }
            return inventoryItems;
        }

        public override void ToClass(System.Xml.Linq.XElement xmlTrans, IErrorLog errorLogger = null)
        {
            if (xmlTrans.HasElements)
            {
                IEnumerable<XElement> transElements = xmlTrans.Elements();
                foreach (XElement transElem in transElements)
                {
                    if (!transElem.IsEmpty)
                    {
                        try
                        {
                            switch (transElem.Name.ToString())
                            {
                                case "inventoryType":
                                    inventoryType = (InventoryTypes)Convert.ToInt32(transElem.Value.ToString());
                                    break;

                                case "InventoryItems":
                                    InventoryItems = CreateInventoryItems(transElem);
                                    break;

                                case "PosTransaction":
                                    base.ToClass(transElem, errorLogger);
                                    break;

                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, transElem.ToString(), ex);
                            }
                        }
                    }
                }
            }
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
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xInventory = new XElement("InventoryTransaction",
                    new XElement("inventoryType", (int)inventoryType)
                );

                XElement xItems = new XElement("InventoryItems");
                foreach (InventoryLineItem ili in InventoryItems)
                {
                    xItems.Add(ili.ToXML());
                }
                xInventory.Add(xItems);

                xInventory.Add(base.ToXML());
                return xInventory;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, ex.Message, ex);
                }
                throw ex;
            }
        }
    }
}
