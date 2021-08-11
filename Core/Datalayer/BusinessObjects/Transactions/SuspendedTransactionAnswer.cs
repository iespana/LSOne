#if !MONO
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Localization;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    [KnownType(typeof(Date))]
    [KnownType(typeof(Name))]
    public class SuspendedTransactionAnswer : DataEntity, ICloneable
    {
        public SuspendedTransactionAnswer ()
	    {
            RecordID = RecordIdentifier.Empty;
            TransactionID = RecordIdentifier.Empty;
            Prompt = string.Empty;
            FieldOrder = 0;
            InformationType = SuspensionTypeAdditionalInfo.InfoTypeEnum.Text;
            InfoCodeResult = string.Empty;
            SerializationTextResult1 = string.Empty;
            SerializationTextResult2 = string.Empty;
            SerializationTextResult3 = string.Empty;
            SerializationTextResult4 = string.Empty;
            SerializationTextResult5 = string.Empty;
            SerializationTextResult6 = string.Empty;
            DateResult = Date.Empty;
            CustomerName = new Name();
            CustomerSingleFieldName = string.Empty;
            InfoCodeTypeSelection = string.Empty;
            
	    }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid, Depth = 2)]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(RecordID, TransactionID);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        [RecordIdentifierConstruction(typeof(Guid))]
        public RecordIdentifier RecordID
        {
            get;
            set;
        }

        [DataMember]
        [RecordIdentifierValidation(40)]
        public RecordIdentifier TransactionID
        {
            get;
            set;
        }

        [DataMember]
        [StringLength(60)]
        public string Prompt
        {
            get;
            set;
        }

        [DataMember]
        public int FieldOrder
        {
            get;
            set;
        }

        [DataMember]
        public SuspensionTypeAdditionalInfo.InfoTypeEnum InformationType
        {
            get;
            set;
        }
        
        public string InfoCodeResult
        {
            get
            {
                return SerializationTextResult1;
            }
            set
            {
                SerializationTextResult1 = value;
            }
        }

        [DataMember]
        [StringLength(20)]
        public string InfoCodeTypeSelection
        {
            get;
            set;
        }

        [DataMember]
        [StringLength(255)]
        public string SerializationTextResult1
        {
            get;
            set;
        }

        [DataMember]
        [StringLength(255)]
        public string SerializationTextResult2
        {
            get;
            set;
        }

        [DataMember]
        [StringLength(60)]
        public string SerializationTextResult3
        {
            get;
            set;
        }

        [DataMember]
        [StringLength(10)]
        public string SerializationTextResult4
        {
            get;
            set;
        }

        [DataMember]
        [StringLength(30)]
        public string SerializationTextResult5
        {
            get;
            set;
        }

        [DataMember]
        [StringLength(20)]
        public string SerializationTextResult6
        {
            get;
            set;
        }

        [DataMember]
        public Date DateResult
        {
            get;
            set;
        }

        [DataMember]
        public Name CustomerName
        {
            get;
            set;
        }

        [DataMember]
        public string CustomerSingleFieldName
        {
            get;
            set;
        }

        [DataMember]
        public RecordIdentifier CustomerID
        {
            get
            {
                return SerializationTextResult1;
            }
            set
            {
                SerializationTextResult1 = (string)value;
            }
        }

        public override string Text
        {
            get
            {
                return SerializationTextResult1;
            }
            set
            {
                SerializationTextResult1 = value;
            }
        }

        public Name Name
        {
            get
            {
                return new Name(SerializationTextResult4, SerializationTextResult1, SerializationTextResult3, SerializationTextResult2, SerializationTextResult5);
            }
            set
            {
                if (value != null)
                {
                    SerializationTextResult4 = value.Prefix;
                    SerializationTextResult1 = value.First;
                    SerializationTextResult3 = value.Middle;
                    SerializationTextResult2 = value.Last;
                    SerializationTextResult5 = value.Suffix;
                }
            }
        }

        public Address Address
        {
            get
            {
                var address = new Address
                    {
                        Address1 = SerializationTextResult1,
                        Address2 = SerializationTextResult2,
                        City = SerializationTextResult3,
                        Zip = SerializationTextResult4,
                        State = SerializationTextResult5,
                        Country = SerializationTextResult6,
                        AddressFormat = AddressFormat
                    };

                return address;
            }
            set
            {
                if (value != null)
                {
                    SerializationTextResult1 = value.Address1;
                    SerializationTextResult2 = value.Address2;
                    SerializationTextResult3 = value.City;
                    SerializationTextResult4 = value.Zip;
                    SerializationTextResult5 = value.State;
                    SerializationTextResult6 = (string)value.Country;
                    AddressFormat = value.AddressFormat;
                }
            }
        }

        public Address.AddressFormatEnum AddressFormat { get; set; } 

        public string ToString(LocalizationContext localizationContext)
        {
            switch (InformationType)
            {
                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Text:
                    return SerializationTextResult1;

                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Address:
                    return localizationContext.FormatSingleLine(Address,localizationContext.CountryResolver);
              
                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Name:
                    return localizationContext.NameFormatter.Format(Name);

                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Infocode:
                    return InfoCodeResult;

                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Date:
                    if (localizationContext.CultureInfo != null)
                    {
                        return DateResult.IsEmpty ? "" : DateResult.DateTime.ToString((IFormatProvider)localizationContext.CultureInfo.DateTimeFormat);
                    }
                    
                    return DateResult.IsEmpty ? "" : DateResult.DateTime.ToShortDateString();

                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Customer:
                    if (CustomerName != null)
                    {
                        return (CustomerName.First != "") ? localizationContext.NameFormatter.Format(CustomerName) : CustomerSingleFieldName;
                    }
                    return CustomerSingleFieldName;

                default:
                    return SerializationTextResult1;
            }
        }

        public bool IsEmpty()
        {
            switch (InformationType)
            {
                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Customer:
                    return (CustomerName == null && CustomerSingleFieldName == "") || CustomerName.First == "" || CustomerSingleFieldName == "";
                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Name:
                    return Name == null || (Name.First == "" && Name.Last == "");
                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Address:
                    return Address.IsEmpty;
                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Infocode:
                    return InfoCodeResult == "";
                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Date:
                    return DateResult.IsEmpty;
                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Text:
                case SuspensionTypeAdditionalInfo.InfoTypeEnum.Other:
                default:
                    return SerializationTextResult1 == "";
            }
        }

        public override object Clone()
        {
            var answer = new SuspendedTransactionAnswer();
            Populate(answer);
            return answer;
        }

        protected void Populate(SuspendedTransactionAnswer answer)
        {
            answer.RecordID = (RecordIdentifier)RecordID.Clone();
            answer.TransactionID = (RecordIdentifier)TransactionID.Clone();
            answer.Prompt = Prompt;
            answer.FieldOrder = FieldOrder;
            answer.InformationType = InformationType;
            answer.InfoCodeResult = InfoCodeResult;
            answer.SerializationTextResult1 = SerializationTextResult1;
            answer.SerializationTextResult2 = SerializationTextResult2;
            answer.SerializationTextResult3 = SerializationTextResult3;
            answer.SerializationTextResult4 = SerializationTextResult4;
            answer.SerializationTextResult5 = SerializationTextResult5;
            answer.SerializationTextResult6 = SerializationTextResult6;
            answer.DateResult = new Date(DateResult.DateTime);
            answer.CustomerName = new Name
                {
                    First = CustomerName.First,
                    Last = CustomerName.Last,
                    Middle = CustomerName.Middle,
                    Prefix = CustomerName.Prefix,
                    Suffix = CustomerName.Suffix
                };
            answer.CustomerSingleFieldName = CustomerSingleFieldName;
            answer.InfoCodeTypeSelection = InfoCodeTypeSelection;
            answer.AddressFormat = AddressFormat;
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
                var xAnswer = new XElement("SuspendedTransactionAnswer",
                    new XElement("RecordID", (string)RecordID),
                    new XElement("TransactionID", (string)TransactionID),
                    new XElement("Prompt", Prompt),
                    new XElement("FieldOrder", Conversion.ToXmlString(FieldOrder)),
                    new XElement("InformationType", Conversion.ToXmlString((int)InformationType)),
                    new XElement("InfoCodeResult", InfoCodeResult),
                    new XElement("SerializationTextResult1", SerializationTextResult1),
                    new XElement("SerializationTextResult2", SerializationTextResult2),
                    new XElement("SerializationTextResult3", SerializationTextResult3),
                    new XElement("SerializationTextResult4", SerializationTextResult4),
                    new XElement("SerializationTextResult5", SerializationTextResult5),
                    new XElement("SerializationTextResult6", SerializationTextResult6),
                    new XElement("DateResult", DateResult.ToXmlString()),
                    new XElement("CustomerName.First", CustomerName.First),
                    new XElement("CustomerName.Last", CustomerName.Last),
                    new XElement("CustomerName.Middle", CustomerName.Middle),
                    new XElement("CustomerName.Prefix", CustomerName.Prefix),
                    new XElement("CustomerName.Suffix", CustomerName.Suffix),
                    new XElement("CustomerSingleFieldName", CustomerSingleFieldName),
                    new XElement("InfoCodeTypeSelection", InfoCodeTypeSelection),
                    new XElement("AddressFormat", Conversion.ToXmlString((int)AddressFormat)) 
                    
                );

                return xAnswer;

            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "SuspendedTransactionAnswer.ToXml", ex);
            }

            return null;
        }

        public override void ToClass(XElement xmlAnswer,IErrorLog errorLogger = null)
        {
            try
            {
                if (xmlAnswer.HasElements)
                {                    
                    IEnumerable<XElement> answerVariables = xmlAnswer.Elements();
                    foreach (XElement answerElem in answerVariables)
                    {
                        if (!answerElem.IsEmpty)
                        {
                            try
                            {
                                switch (answerElem.Name.ToString())
                                {
                                    case "RecordID":
                                        RecordID = Conversion.XmlStringToGuid(answerElem.Value);
                                        break;
                                    case "TransactionID":
                                        TransactionID = answerElem.Value;
                                        break;
                                    case "Prompt":
                                        Prompt = answerElem.Value;
                                        break;
                                    case "FieldOrder":
                                        FieldOrder = Conversion.XmlStringToInt(answerElem.Value);
                                        break;
                                    case "InformationType":
                                        InformationType = (SuspensionTypeAdditionalInfo.InfoTypeEnum)Conversion.XmlStringToInt(answerElem.Value);
                                        break;
                                    case "InfoCodeResult":
                                        InfoCodeResult = answerElem.Value;
                                        break;
                                    case "SerializationTextResult1":
                                        SerializationTextResult1 = answerElem.Value;
                                        break;
                                    case "SerializationTextResult2":
                                        SerializationTextResult2 = answerElem.Value;
                                        break;
                                    case "SerializationTextResult3":
                                        SerializationTextResult3 = answerElem.Value;
                                        break;
                                    case "SerializationTextResult4":
                                        SerializationTextResult4 = answerElem.Value;
                                        break;
                                    case "SerializationTextResult5":
                                        SerializationTextResult5 = answerElem.Value;
                                        break;
                                    case "SerializationTextResult6":
                                        SerializationTextResult6 = answerElem.Value;
                                        break;
                                    case "DateResult":
                                        DateResult = new Date(Date.XmlStringToDateTime(answerElem.Value));
                                        break;                                        
                                    case "CustomerName.First":                                        
                                        CustomerName.First = answerElem.Value;
                                        break;
                                    case "CustomerName.Last":
                                        CustomerName.Last = answerElem.Value;
                                        break;
                                    case "CustomerName.Middle":
                                        CustomerName.Middle = answerElem.Value;
                                        break;
                                    case "CustomerName.Prefix":
                                        CustomerName.Prefix = answerElem.Value;
                                        break;
                                    case "CustomerName.Suffix":
                                        CustomerName.Suffix = answerElem.Value;
                                        break;
                                    case "CustomerSingleFieldName":
                                        CustomerSingleFieldName = answerElem.Value;
                                        break;
                                    case "InfoCodeTypeSelection":
                                        InfoCodeTypeSelection = answerElem.Value;
                                        break;
                                    case "AddressFormat":
                                        AddressFormat = (Address.AddressFormatEnum)Conversion.XmlStringToInt(answerElem.Value);
                                        break;
                                }
                            }
                            catch(Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error,answerElem.Name.ToString(),ex);
                            }
                        }
                    }                      
                }
            }
            catch(Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "SuspendedTransactionAnswer.ToClass", ex);
            }
        }
    }
}
