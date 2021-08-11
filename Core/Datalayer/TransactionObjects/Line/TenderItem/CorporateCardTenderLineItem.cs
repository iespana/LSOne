using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.TenderItem
{
    /// <summary>
    /// A class extending the card payments, to payments with corporate cards.
    /// </summary>
    [Serializable]
    public class CorporateCardTenderLineItem : CardTenderLineItem
    {
        private string driverId;        //A unique id for the driver of the vehicle being fueled.
        private string vehicleId;       //A unique id for the vehicle being fueled.
        private int odometerReading;    //The odometer reading on the vehicle being fueled.
        private bool temporaryCar;      //Is the vehicle being filled, a temporary member of the fleet?

        #region Properties
        /// <summary>
        /// A unique id for the driver of the vehicle being fueled.
        /// </summary>
        public string DriverId
        {
            get { return driverId; }
            set { driverId = value; }
        }
        /// <summary>
        /// A unique id for the vehicle being fueled.
        /// </summary>
        public string VehicleId
        {
            get { return vehicleId; }
            set { vehicleId = value; }
        }
        /// <summary>
        /// The odometer reading on the vehicle being fueled.
        /// </summary>
        public int OdometerReading
        {
            get { return odometerReading; }
            set { odometerReading = value; }
        }
        /// <summary>
        /// Is the vehicle being filled, a temporary member of the fleet?
        /// </summary>
        public bool TemporaryCar
        {
            get { return temporaryCar; }
            set { temporaryCar = value; }
        }
        #endregion

        public CorporateCardTenderLineItem()
        {
            internalTenderType = TenderTypeEnum.CorporateCardTender;
        }
        
        protected void Populate(CorporateCardTenderLineItem item)
        {
            base.Populate(item);
            item.driverId = driverId;
            item.vehicleId = vehicleId;
            item.odometerReading = odometerReading;
            item.temporaryCar = temporaryCar;
            item.internalTenderType = TypeOfTender;
        }

        public override object Clone()
        {
            CorporateCardTenderLineItem item = new CorporateCardTenderLineItem();
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
                * DateTime     added with DevUtilities.Utility.DateTimeToXmlString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xCorporateCard = new XElement("CorporateCardTenderLineItem",
                    new XElement("driverId", driverId),
                    new XElement("vehicleId", vehicleId),
                    new XElement("odometerReading", odometerReading),
                    new XElement("temporaryCar", temporaryCar)
                );

                xCorporateCard.Add(base.ToXML(errorLogger));
                return xCorporateCard;
            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "CorporateCardTenderLineItem.ToXML", ex);
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
                                    case "driverId":
                                        driverId = xVariable.Value;
                                        break;
                                    case "vehicleId":
                                        vehicleId = xVariable.Value;
                                        break;
                                    case "odometerReading":
                                        odometerReading = Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "temporaryCar":
                                        temporaryCar = Conversion.ToBool(xVariable.Value);
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
                                    errorLogger.LogMessage(LogMessageType.Error, "CorporateCardTenderLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "CorporateCardTenderLineItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
