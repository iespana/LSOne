using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Transactions.Line
{

    /// <summary>
    /// A basic abstract class for tender- and salesitems.
    /// </summary>
    [Serializable]
    public abstract class LineItem : ILineItem
    {
        private int lineId;                     //The unique id of each line in the transaction
        //SaleDetails
        private bool voided;                    //Is set to true if line has been voided
        private bool changedForPreparation;     //Has the item been changed since it was delivered for preparation
        private string description;             //The item description/text
        private string descriptionAlias;        //The item description/text alias
        private string descriptionAltLanguage;  //The item description/text in an alternate language
        //Timestamps
        private Date beginDateTime = Date.Empty;         //The start date and time of the transaction
        private Date endDateTime = Date.Empty;           //The end date and time of the transaction   

        /// <summary>
        /// The unique id of each line in the transaction
        /// </summary>
        public int LineId
        {
            get { return lineId; }
            set { lineId = value; }
        }
        /// <summary>
        /// Is set to true if line has been voided
        /// </summary>
        public bool Voided
        {
            get { return voided; }
            set { voided = value; }
        }
        /// <summary>
        /// Has the item been changed since it was delivered for preparation
        /// </summary>
        public bool ChangedForPreparation
        {
            get { return changedForPreparation; }
            set { changedForPreparation = value; }
        }
        /// <summary>
        /// A description of the line item
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        /// A description alias of the line item
        /// </summary>
        public string DescriptionAlias
        {
            get { return descriptionAlias; }
            set { descriptionAlias = value; }
        }
        /// <summary>
        /// A description of the line item in an alternate language
        /// </summary>
        public string DescriptionAltLanguage
        {
            get { return descriptionAltLanguage; }
            set { descriptionAltLanguage = value; }
        }
        /// <summary>
        /// The start date and time of the transaction.
        /// </summary>
        public DateTime BeginDateTime
        {
            get { return beginDateTime.DateTime; }
            set { beginDateTime = value > Date.Empty.DateTime ? new Date(value) : Date.Empty; }
        }
        /// <summary>
        /// The end date and time of the transaction.
        /// </summary>
        public DateTime EndDateTime
        {
            get { return endDateTime.DateTime; }
            set { endDateTime = value > Date.Empty.DateTime ? new Date(value) : Date.Empty; }
        }        

                
        public List<InfoCodeLineItem> InfoCodeLines { get; set; }

        public ExcludedActions ExcludedActions { get; set; }

        public RecordIdentifier ValidationPeriod { get; set; }

        public LineItem()
        {
            this.BeginDateTime = DateTime.Now;
            InfoCodeLines = new List<InfoCodeLineItem>();
            description = string.Empty;
            descriptionAlias = string.Empty;
            descriptionAltLanguage = string.Empty;
            ExcludedActions = 0;
            ValidationPeriod = RecordIdentifier.Empty;
        }

        ~LineItem()
        {
            if (InfoCodeLines != null)
                InfoCodeLines.Clear();
        }

        public abstract object Clone();

        protected void Populate(LineItem item)
        {
            item.lineId = lineId;
            item.voided = voided;
            item.changedForPreparation = changedForPreparation;
            item.description = description;
            item.descriptionAlias = descriptionAlias;
            item.descriptionAltLanguage = descriptionAltLanguage;
            item.BeginDateTime = BeginDateTime;
            item.EndDateTime = EndDateTime;
            item.InfoCodeLines = CollectionHelper.Clone<InfoCodeLineItem, List<InfoCodeLineItem>>(InfoCodeLines);
            item.ExcludedActions = ExcludedActions;
            item.ValidationPeriod = ValidationPeriod;            
        }

        public void Add(InfoCodeLineItem infoCodeLineItem)
        {
            infoCodeLineItem.EndDateTime = DateTime.Now;
            infoCodeLineItem.LineId = this.InfoCodeLines.Count + 1;
            this.InfoCodeLines.Add(infoCodeLineItem);
        }

        public virtual XElement ToXML(IErrorLog errorLogger = null)
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
                XElement xLineItem = new XElement("LineItem",
                     new XElement("lineId", Conversion.ToXmlString(lineId)),
                     new XElement("voided", Conversion.ToXmlString(voided)),
                     new XElement("NoChangeActionsAllowed", Conversion.ToXmlString((int)ExcludedActions)),
                     new XElement("ValidationPeriod", ValidationPeriod),
                     new XElement("changedForPreparation", Conversion.ToXmlString(changedForPreparation)),
                     new XElement("description", description),
                     new XElement("descriptionAlias", descriptionAlias),
                     new XElement("descriptionAltLanguage", descriptionAltLanguage),
                     new XElement("beginDateTime", Conversion.ToXmlString(beginDateTime.DateTime)),
                     new XElement("endDateTime", Conversion.ToXmlString(endDateTime.DateTime))
                     );

                return xLineItem;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "LineItem.ToXml", ex);

                throw;
            }
        }

        public virtual void ToClass(XElement xItem, IErrorLog errorLogger = null)
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
                                    case "lineId":
                                        lineId = Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "voided":
                                        voided = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "NoChangeActionsAllowed":
                                        ExcludedActions = (ExcludedActions)Conversion.XmlStringToInt(xVariable.Value);
                                        break;
                                    case "ValidationPeriod":
                                        ValidationPeriod = xVariable.Value;
                                        break;
                                    case "changedForPreparation":
                                        changedForPreparation = Conversion.XmlStringToBool(xVariable.Value);
                                        break;
                                    case "description":
                                        description = xVariable.Value;
                                        break;
                                    case "descriptionAlias":
                                        descriptionAlias = xVariable.Value;
                                        break;
                                    case "descriptionAltLanguage":
                                        descriptionAltLanguage = xVariable.Value;
                                        break;
                                    case "beginDateTime":
                                        beginDateTime = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                        break;
                                    case "endDateTime":
                                        endDateTime = new Date(Conversion.XmlStringToDateTime(xVariable.Value));
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error, "LineItem:" + xVariable.Name, ex);
                            }
                        }
                    }                   
                }
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "LineItem.ToClass", ex);

                throw;
            }
        }        
    }
}


