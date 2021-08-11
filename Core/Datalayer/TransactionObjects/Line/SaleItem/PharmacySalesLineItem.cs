using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects.Line.SaleItem
{
    /// <summary>
    /// A pharmacy item transaction line.
    /// </summary>
    [Serializable]
    public class PharmacySalesLineItem : SaleLineItem
    {
        #region Member variables
        private bool prescriptionAsOneLine;                 //If true, the item is a whole prescription, else the item is an item on a prescription
        private int prescriptionLineId;                     //The line id on the prescription, set as zero it the item is a whole prescription.
        private string prescriptionId;                      //The pharmacy prescription id
        private DateTime prescriptionDate = DateTime.ParseExact("01.01.1900", "dd.MM.yyyy", null);                  //Date of the prescription
        private string patientId;                           //A national id or another unique id of the patient
        private string patientName;                         //The patients name
        private decimal patientAmount;                      //The amount to be paid by the patient.
        private decimal insuranceCompanyAmount;             //The amount paid by the insurance company/government 
        private string dosageType;                          //Dosage type,i.e pills, capules,etc.
        private decimal dosageStrength;                     //Dosage strength
        private string dosageStrengthUnit;                  //The unit the dosage strength is measured in, i.e mg,ml, etc
        private decimal dosageUnitQuantiy;                  //The unit quantity
        #endregion

        #region Properties
        /// <summary>
        /// If true, the item is a whole prescription, else the item is an item on a prescription
        /// </summary>
        public bool PrescriptionAsOneLine
        {
            get { return prescriptionAsOneLine; }
            set { prescriptionAsOneLine = value; }
        }
        /// <summary>
        /// The line id on the prescription, set as zero it the item is a whole prescription.
        /// </summary>
        public int PrescriptionLineId
        {
            get { return prescriptionLineId; }
            set { prescriptionLineId = value; }
        }
        /// <summary>
        /// The pharmacy prescription id
        /// </summary>
        public string PrescriptionId
        {
            get { return prescriptionId; }
            set { prescriptionId = value; }
        }
        /// <summary>
        /// Date of the prescription
        /// </summary>
        public DateTime PrescriptionDate
        {
            get { return prescriptionDate; }
            set { prescriptionDate = value; }
        }
        /// <summary>
        /// A national id or another unique id of the patient
        /// </summary>
        public string PatientId
        {
            get { return patientId; }
            set { patientId = value; }
        }
        /// <summary>
        /// The patients name
        /// </summary>
        public string PatientName
        {
            get { return patientName; }
            set { patientName = value; }
        }
        /// <summary>
        /// The amount to be paid by the patient.
        /// </summary>
        public decimal PatientAmount
        {
            get { return patientAmount; }
            set { patientAmount = value; }
        }
        /// <summary>
        /// The amount paid by the insurance company/government 
        /// </summary>
        public decimal InsuranceCompanyAmount
        {
            get { return insuranceCompanyAmount; }
            set { insuranceCompanyAmount = value; }
        }
        /// <summary>
        /// Dosage type,i.e pills, capules,etc.
        /// </summary>
        public string DosageType
        {
            get { return dosageType; }
            set { dosageType = value; }
        }
        /// <summary>
        /// Dosage strength
        /// </summary>
        public decimal DosageStrength
        {
            get { return dosageStrength; }
            set { dosageStrength = value; }
        }
        /// <summary>
        /// The unit the dosage strength is measured in, i.e mg, ml, etc.
        /// </summary>
        public string DosageStrengthUnit
        {
            get { return dosageStrengthUnit; }
            set { dosageStrengthUnit = value; }
        }
        /// <summary>
        /// The unit quantity
        /// </summary>
        public decimal DosageUnitQuantiy
        {
            get { return dosageUnitQuantiy; }
            set { dosageUnitQuantiy = value; }
        }
        #endregion

        public PharmacySalesLineItem(RetailTransaction transaction)
            : base(transaction)
        {
            this.ItemClassType = SalesTransaction.ItemClassTypeEnum.PharmacySalesLineItem;
        }

        protected override SalesTransaction.ItemClassTypeEnum GetItemClassType(SaleLineItem saleItem)
        {
            return SalesTransaction.ItemClassTypeEnum.PharmacySalesLineItem;
        }

        public override object Clone()
        {
            PharmacySalesLineItem item = new PharmacySalesLineItem((RetailTransaction)Transaction);
            Populate(item, Transaction);
            return item;
        }

        public override SaleLineItem Clone(IRetailTransaction transaction)
        {
            var item = new PharmacySalesLineItem((RetailTransaction)transaction);
            Populate(item, transaction);
            return item;
        }

        protected void Populate(PharmacySalesLineItem item, IRetailTransaction transaction)
        {
            base.Populate(item, transaction);
            item.prescriptionAsOneLine = prescriptionAsOneLine;
            item.prescriptionDate = prescriptionDate;
            item.prescriptionId = prescriptionId;
            item.prescriptionLineId = prescriptionLineId;
            item.patientAmount = patientAmount;
            item.patientId = patientId;
            item.patientName = patientName;
            item.insuranceCompanyAmount = insuranceCompanyAmount;
            item.dosageStrength = dosageStrength;
            item.dosageStrengthUnit = dosageStrengthUnit;
            item.dosageType = dosageType;
            item.dosageUnitQuantiy = dosageUnitQuantiy;
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
                * DateTime     added with ToString()
                * 
                * Enums        added with an (int) cast   
                * 
               */
                XElement xPharmacy = new XElement("PharmacySalesLineItem",
                    new XElement("prescriptionAsOneLine", prescriptionAsOneLine),
                    new XElement("prescriptionLineId", prescriptionLineId),
                    new XElement("prescriptionId", prescriptionId),
                    new XElement("prescriptionDate", Conversion.ToXmlString(prescriptionDate)),
                    new XElement("patientId", patientId),
                    new XElement("patientName", patientName),
                    new XElement("patientAmount", patientAmount.ToString()),
                    new XElement("insuranceCompanyAmount", insuranceCompanyAmount.ToString()),
                    new XElement("dosageType", dosageType),
                    new XElement("dosageStrength", dosageStrength.ToString()),
                    new XElement("dosageStrengthUnit", dosageStrengthUnit),
                    new XElement("dosageUnitQuantiy", dosageUnitQuantiy.ToString())
                );

                xPharmacy.Add(base.ToXML(errorLogger));
                return xPharmacy;

            }
            catch (Exception ex)
            {
                if (errorLogger != null)
                {
                    errorLogger.LogMessage(LogMessageType.Error, "PharmacySalesLineItem.ToXml", ex);
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
                                    case "prescriptionAsOneLine":
                                        prescriptionAsOneLine = Conversion.ToBool(xVariable.Value);
                                        break;
                                    case "prescriptionLineId":
                                        prescriptionLineId = Convert.ToInt32(xVariable.Value);
                                        break;
                                    case "prescriptionId":
                                        prescriptionId = xVariable.Value;
                                        break;
                                    case "prescriptionDate":
                                        prescriptionDate = Conversion.XmlStringToDateTime(xVariable.Value);
                                        break;
                                    case "patientId":
                                        patientId = xVariable.Value;
                                        break;
                                    case "patientName": 
                                        patientName = xVariable.Value;
                                        break;
                                    case "patientAmount":
                                        patientAmount = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "insuranceCompanyAmount":
                                        insuranceCompanyAmount = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "dosageType":
                                        dosageType = xVariable.Value;
                                        break;
                                    case "dosageStrength":
                                        dosageStrength = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    case "dosageStrengthUnit":
                                        dosageStrengthUnit = xVariable.Value;
                                        break;                                        
                                    case "dosageUnitQuantiy":
                                        dosageUnitQuantiy = Convert.ToDecimal(xVariable.Value);
                                        break;
                                    default:
                                        base.ToClass(xVariable, errorLogger);
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                if (errorLogger != null)
                                {
                                    errorLogger.LogMessage(LogMessageType.Error, "PharmacySalesLineItem:" + xVariable.Name.ToString(), ex);
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
                    errorLogger.LogMessage(LogMessageType.Error, "GiftCertificateTenderLineItem.ToClass", ex);
                }

                throw ex;
            }
        }
    }
}
