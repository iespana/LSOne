
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    /// <summary>
    /// Has all information about the hospitality that is needed in the POS f.ex. table ID, hospitality type and etc.
    /// </summary>
    public class HospitalityItem
    {
        /// <summary>
        /// Which hospitality type is currently active
        /// </summary>
        public RecordIdentifier ActiveHospitalitySalesType { get; set; }               
        /// <summary>
        /// Information about the table that the sale is attached to
        /// </summary>
        public TableInfo TableInformation { get; set; }
        /// <summary>
        /// The station name that is being used to print the kitchen slit
        /// </summary>
        public string RestaurantStation { get; set; }

        /// <summary>
        /// The number of guests on the table
        /// </summary>
        public int NoOfGuests { get; set; }        

        public HospitalityItem()
        {
            Clear();
        }

        public HospitalityItem(HospitalityItem hospitalityItem)
        {
            Populate(hospitalityItem);
        }

        /// <summary>
        /// REturns true if there is no hospitality information available
        /// </summary>
        /// <returns></returns>
        public bool Empty()
        {
            return ActiveHospitalitySalesType == RecordIdentifier.Empty && TableInformation.Empty();
        }

        /// <summary>
        /// Clears all hospitality information
        /// </summary>
        public void Clear()
        {
            ActiveHospitalitySalesType = RecordIdentifier.Empty;
            TableInformation = new TableInfo();
            RestaurantStation = "";
            NoOfGuests = 0;
        }

        protected void Populate(HospitalityItem hospitalityItem)
        {
            hospitalityItem.ActiveHospitalitySalesType = ActiveHospitalitySalesType;
            hospitalityItem.TableInformation = (TableInfo) TableInformation.Clone();
            hospitalityItem.RestaurantStation = RestaurantStation;
            hospitalityItem.NoOfGuests = NoOfGuests;
        }

        public virtual object Clone()
        {
            HospitalityItem op = new HospitalityItem();
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
                XElement xItem = new XElement("HospitalityItem",
                    new XElement("ActiveHospitalitySalesType", ActiveHospitalitySalesType),
                    TableInformation.ToXML(),
                    new XElement("RestaurantStation", RestaurantStation),
                    new XElement("NoOfGuests", Conversion.ToXmlString(NoOfGuests))
                );

                return xItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "HospitalityItem.ToXML", ex);

                throw;
            }
        }

        public void ToClass(XElement xItem, IErrorLog errorLogger = null)
        {
            try
            {
                if (xItem.HasElements)
                {
                    IEnumerable<XElement> hospitalityElements = xItem.Elements();
                    foreach (XElement hospitalityElem in hospitalityElements)
                    {
                        if (!hospitalityElem.IsEmpty)
                        {
                            try
                            {
                                switch (hospitalityElem.Name.ToString())
                                {
                                    case "ActiveHospitalitySalesType":
                                        ActiveHospitalitySalesType = hospitalityElem.Value;
                                        break;
                                    case "RestaurantStation":
                                        RestaurantStation = hospitalityElem.Value;
                                        break;
                                    case "TableInfo":
                                        TableInformation.ToClass(hospitalityElem, errorLogger);
                                        break;
                                    case "NoOfGuests":
                                        NoOfGuests = Conversion.XmlStringToInt(hospitalityElem.Value);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "HospitalityItem:" + hospitalityElem.Name, ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "HospitalityItem.ToClass", ex);

                throw;
            }
        }
    }
}
