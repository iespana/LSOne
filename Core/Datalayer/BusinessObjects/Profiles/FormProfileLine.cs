using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public class FormProfileLine : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(ProfileID, ReceiptTypeID);
            }
            set
            {
                base.ID = value;
            }
        }        

        public FormProfileLine()
            : base()
        {
            ProfileID = RecordIdentifier.Empty;
            ReceiptTypeID = RecordIdentifier.Empty;
            FormLayoutID = RecordIdentifier.Empty;
            TypeDescription = "";
            PrintAsSlip = false;
            PrintBehavior = PrintBehaviors.AlwaysPrint;
            LineCountPrPage = 55;
            Text = "";
            IsSystemProfileLine = false;
            NumberOfCopies = 1;
        }

        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier ProfileID { get; set; }
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier ReceiptTypeID { get; set; }
        public RecordIdentifier FormLayoutID { get; set; }
        public string TypeDescription { get; set; }
        public bool PrintAsSlip { get; set; }
        public PrintBehaviors PrintBehavior { get; set; }
        public int LineCountPrPage { get; set; }
        public bool IsSystemProfileLine { get; set; }
        public int NumberOfCopies { get; set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "id":
                                ID = current.Value;
                                break;
                            case "description":
                                Text = current.Value;
                                break;
                            case "profileID":
                                ProfileID = new Guid(current.Value);
                                break;
                            case "formTypeID":
                                ReceiptTypeID = new Guid(current.Value);
                                break;
                            case "formLayoutID":
                                FormLayoutID = current.Value;
                                break;
                            case "typeDescription":
                                TypeDescription = current.Value;
                                break;
                            case "printAsSlip":
                                PrintAsSlip = current.Value != "false";
                                break;
                            case "printBehaviour":
                                PrintBehavior = (PrintBehaviors)Convert.ToInt32(current.Value);
                                break;
                            case "lineCountPerPage":
                                LineCountPrPage = Convert.ToInt32(current.Value);
                                break;
                            case "isSystemProfileLine":
                                IsSystemProfileLine = current.Value != "false";
                                break;
                            case "numberOfCopies":
                                NumberOfCopies = Convert.ToInt32(current.Value);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }

                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("formProfileLine",
                    new XElement("id", ID),
                    new XElement("description", Text),
                    new XElement("profileID", ProfileID),
                    new XElement("formTypeID", ReceiptTypeID),
                    new XElement("formLayoutID", FormLayoutID),
                    new XElement("typeDescription", TypeDescription),
                    new XElement("printAsSlip", PrintAsSlip),
                    new XElement("printBehaviour", (int)PrintBehavior),
                    new XElement("lineCountPerPage", LineCountPrPage),
                    new XElement("isSystemProfileLine", IsSystemProfileLine),
                    new XElement("numberOfCopies", NumberOfCopies)
                    );
            return xml;
        }
    }
}
