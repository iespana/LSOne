using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.DiscountItems
{
    /// <summary>
    /// Periodic discount information that can belong to a saleitem.
    /// </summary>
    [Serializable]
    public class PeriodicDiscountItem : LineDiscountItem, IDiscountItem, IPeriodicDiscountItem
    {
        public PeriodicDiscountItem()
        {
            DiscountType = DiscountTransTypes.Periodic;
        }

        #region Member variables
        private PeriodicDiscOfferType periodicDiscountType; //The type of a periodic discount, i.e multibuy, mix & match, offer.
        private string itemDiscountGroup;                   //The discount group of the item
        private decimal quantityDiscounted;                 //The quantity that has been discounted in a multibuy discount offer
        private string periodicDiscGroupId;                 //The id of the periodic discount group
        private bool sameDifferentMMLines;                  //Use different og same lines within transaction to activate the offer?        
        
        #endregion

        #region Properties

        
        /// <summary>
        /// The type of a periodic discount, i.e multibuy, mix and match, offer, promotion.
        /// </summary>
        public PeriodicDiscOfferType PeriodicDiscountType
        {
            get { return periodicDiscountType; }
            set { periodicDiscountType = value; }
        }
        /// <summary>
        /// The discount group of the item
        /// </summary>
        public string ItemDiscountGroup
        {
            get { return itemDiscountGroup; }
            set { itemDiscountGroup = value; }
        }
        /// <summary>
        /// The quantity that has been discounted in a multibuy discount offer
        /// </summary>
        public decimal QuantityDiscounted
        {
            get { return quantityDiscounted; }
            set { quantityDiscounted = value; }
        }
        /// <summary>
        /// The id of the periodic discount group
        /// </summary>
        public string PeriodicDiscGroupId
        {
            get { return periodicDiscGroupId; }
            set { periodicDiscGroupId = value; }
        }

        public bool SameDifferentMMLines
        {
            get { return sameDifferentMMLines; }
            set { sameDifferentMMLines = value; }
        }
        #endregion

        public override object Clone()
        {
            PeriodicDiscountItem item = new PeriodicDiscountItem();
            Populate(item);
            return item;
        }

        protected void Populate(PeriodicDiscountItem item)
        {
            base.Populate(item);
            item.periodicDiscountType = periodicDiscountType;
            item.itemDiscountGroup = itemDiscountGroup;
            item.quantityDiscounted = quantityDiscounted;
            item.periodicDiscGroupId = periodicDiscGroupId;
            item.sameDifferentMMLines = sameDifferentMMLines;
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
                XElement xPeriodicDisc = new XElement("PeriodicDiscountItem",
                    new XElement("periodicDiscountType", (int)periodicDiscountType),
                    new XElement("itemDiscountGroup", itemDiscountGroup),
                    new XElement("quantityDiscounted", quantityDiscounted.ToString()),
                    new XElement("periodicDiscGroupId", periodicDiscGroupId),
                    new XElement("sameDifferentMMLines", sameDifferentMMLines)
                );

                xPeriodicDisc.Add(base.ToXML(errorLogger));
                return xPeriodicDisc;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "PeriodicDiscountItem.ToXML", ex);
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
                                    case "periodicDiscountType":
                                        periodicDiscountType = (PeriodicDiscOfferType)Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "itemDiscountGroup":
                                        itemDiscountGroup = xVariable.Value;
                                        break;
                                    case "quantityDiscounted":
                                        quantityDiscounted = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "periodicDiscGroupId":
                                        periodicDiscGroupId = xVariable.Value;
                                        break;
                                    case "sameDifferentMMLines":
                                        sameDifferentMMLines = Conversion.ToBool(xVariable.Value);
                                        break;
                                    default:
                                        base.ToClass(xVariable,errorLogger);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "PeriodicDiscountItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "PeriodicDiscountItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
