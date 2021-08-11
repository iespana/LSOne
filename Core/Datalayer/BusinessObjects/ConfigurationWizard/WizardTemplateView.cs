using System;
using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard
{
    /// <summary>
    /// Business entity class of WizardTemplateView page
    /// </summary>
    public class WizardTemplateView : DataEntity
    {
        /// <summary>
        /// Template status enum
        /// </summary>
        public enum TemplateStatus
        {
            New,
            InProgress,
            Finished
        };

        public WizardTemplateView()
        {
            ID = RecordIdentifier.Empty;
            Status = 0;
            DataAreaID = string.Empty;
            Text = string.Empty;
            StoreId = RecordIdentifier.Empty;
            TerminalId = RecordIdentifier.Empty;
            TemplateImage = null;
            LastExportDate = DateTime.Now;
            Name = 1;
            Address = 0;
            PriceCalculation = 1;
            ChooseNameAddress = 1;
            ChoosePriceCalculation = 1;
            VisualProfileID = RecordIdentifier.Empty;
            FunctionalityProfileID = RecordIdentifier.Empty;
            StoreServerProfileID = RecordIdentifier.Empty;
            DefaultCurrency = string.Empty;
            ChoosePayments = 1;
            HardwareProfileID = RecordIdentifier.Empty;
            ChoosePeripherals = 1;
            ChooseTillLayouts = 1;
            ChooseReceipts = 1;
            ChooseRetailGroups = 1;
            ChoosePermissionGroup = 1;
        }

        /// <summary>
        /// Status of the template
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// The data area id 
        /// </summary>
        public string DataAreaID { get; set; }

        /// <summary>
        /// StoreId of the template
        /// </summary>
        public RecordIdentifier StoreId { get; set; }

        /// <summary>
        /// TerminalId of the template
        /// </summary>
        public RecordIdentifier TerminalId { get; set; }

        /// <summary>
        /// An image of the template
        /// </summary>
        public byte[] TemplateImage { get; set; }

        /// <summary>
        /// Last export date of the template
        /// </summary>
        public DateTime LastExportDate { get; set; }

        /// <summary>
        /// How the name should be displayed in the SC and/or POS
        /// </summary>
        public int? Name { get; set; }

        /// <summary>
        /// How the address should be displayed in the SC and/or POS
        /// </summary>
        public int? Address { get; set; }

        /// <summary>
        /// How the POS should handle prices including or excluding tax. 
        /// </summary>
        public int? PriceCalculation { get; set; }

        /// <summary>
        /// Should the user be able to select the name and address conventions
        /// </summary>
        public byte ChooseNameAddress { get; set; }

        /// <summary>
        /// Should the user be able to select how prices are calculated
        /// </summary>
        public byte ChoosePriceCalculation { get; set; }

        /// <summary>
        /// Visual profile id of selected template
        /// </summary>
        public RecordIdentifier VisualProfileID { get; set; }

        /// <summary>
        /// Functionality profile id of selected template
        /// </summary>
        public RecordIdentifier FunctionalityProfileID { get; set; }

        /// <summary>
        /// Store server profile id of selected template
        /// </summary>
        public RecordIdentifier StoreServerProfileID { get; set; }

        /// <summary>
        /// default currency for the store
        /// </summary>
        public string DefaultCurrency { get; set; }

        /// <summary>
        /// Should the user be able to select the payments and currencies
        /// </summary>
        public byte ChoosePayments { get; set; }

        /// <summary>
        /// Selected Hardware profile id
        /// </summary>
        public RecordIdentifier HardwareProfileID { get; set; }

        /// <summary>
        /// Should the user be able to select the peripherals
        /// </summary>
        public byte ChoosePeripherals { get; set; }

        /// <summary>
        /// Should the user be able to select the till layout
        /// </summary>
        public byte ChooseTillLayouts { get; set; }

        /// <summary>
        /// Should the user be able to select the receipt layout
        /// </summary>
        public byte ChooseReceipts { get; set; }

        /// <summary>
        /// Should the user be able to select the retail group
        /// </summary>
        public byte ChooseRetailGroups { get; set; }

        /// <summary>
        /// Should the user be able to select the permission group
        /// </summary>
        public byte ChoosePermissionGroup { get; set; }

        /// <summary>
        /// Sets all variables in the TemplateView class with the values in the xml
        /// </summary>
        /// <param name="xWizardSetting">The xml element with the wizard setting values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xWizardSetting, IErrorLog errorLogger = null)
        {
            if (xWizardSetting.HasElements)
            {
                var templateVariables = xWizardSetting.Elements();
                foreach (var templateElem in templateVariables)
                {
                    //No template Id -> no template -> no need to go any further
                    if (templateElem.Name.ToString() == "templateId" && templateElem.Value == "")
                    {
                        return;
                    }

                    if (!templateElem.IsEmpty)
                    {
                        try
                        {
                            switch (templateElem.Name.ToString())
                            {
                                case "templateId":
                                    ID = templateElem.Value;
                                    break;
                                case "description":
                                    Text = templateElem.Value;
                                    break;
                                case "status":
                                    Status = Convert.ToInt32(templateElem.Value);
                                    break;
                                case "dataAreaID":
                                    DataAreaID = templateElem.Value;
                                    break;
                                case "storeId":
                                    StoreId = templateElem.Value;
                                    break;
                                case "terminalId":
                                    TerminalId = templateElem.Value;
                                    break;
                                case "templateImage":
                                    TemplateImage = Convert.FromBase64String(templateElem.Value);
                                    break;
                                case "name":
                                    Name = Convert.ToInt32(templateElem.Value);
                                    break;
                                case "address":
                                    Address = Convert.ToInt32(templateElem.Value);
                                    break;
                                case "priceCalculation":
                                    PriceCalculation = Convert.ToInt32(templateElem.Value);
                                    break;
                                case "chooseNameAddress":
                                    ChooseNameAddress = Convert.ToByte(templateElem.Value);
                                    break;
                                case "choosePriceCalculation":
                                    ChoosePriceCalculation = Convert.ToByte(templateElem.Value);
                                    break;
                                case "visualProfileID":
                                    VisualProfileID = templateElem.Value;
                                    break;
                                case "functionalityProfileID":
                                    FunctionalityProfileID = templateElem.Value;
                                    break;
                                case "storeServerProfileID":
                                    StoreServerProfileID = templateElem.Value;
                                    break;
                                case "defaultCurrency":
                                    DefaultCurrency = templateElem.Value;
                                    break;
                                case "choosePayments":
                                    ChoosePayments = Convert.ToByte(templateElem.Value);
                                    break;
                                case "hardwareProfileID":
                                    HardwareProfileID = templateElem.Value;
                                    break;
                                case "choosePeripherals":
                                    ChoosePeripherals = Convert.ToByte(templateElem.Value);
                                    break;
                                case "chooseTillLayouts":
                                    ChooseTillLayouts = Convert.ToByte(templateElem.Value);
                                    break;
                                case "chooseReceipts":
                                    ChooseReceipts = Convert.ToByte(templateElem.Value);
                                    break;
                                case "chooseRetailGroups":
                                    ChooseRetailGroups = Convert.ToByte(templateElem.Value);
                                    break;
                                case "choosePermissionGroup":
                                    ChoosePermissionGroup = Convert.ToByte(templateElem.Value);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error,
                                                       templateElem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an xml element from all the variables in the StoreSetting class
        /// </summary>
        /// <param name="errorLogger"></param>
        /// <returns>An XML element</returns>
        public override XElement ToXML(IErrorLog errorLogger = null)
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
            var xStoreSetting = new XElement("WizardTemplate",
                new XElement("templateId", (string)ID),
                new XElement("description", Text),
                new XElement("status", Status.ToString()),
                new XElement("dataAreaID", DataAreaID),
                new XElement("storeId", (string)StoreId),
                new XElement("terminalId", (string)TerminalId),
                new XElement("templateImage", Convert.ToBase64String(TemplateImage)),// System.Text.Encoding.ASCII.GetString(TEMPLATEIMAGE)),
                new XElement("name", Name.ToString()),
                new XElement("address", Address.ToString()),
                new XElement("priceCalculation", PriceCalculation),
                new XElement("chooseNameAddress", ChooseNameAddress.ToString()),
                new XElement("choosePriceCalculation", ChoosePriceCalculation.ToString()),
                new XElement("visualProfileID", (string)VisualProfileID),
                new XElement("functionalityProfileID", (string)FunctionalityProfileID),
                new XElement("storeServerProfileID", (string)StoreServerProfileID),
                new XElement("defaultCurrency", DefaultCurrency),
                new XElement("choosePayments", ChoosePayments.ToString()),
                new XElement("hardwareProfileID", (string)HardwareProfileID),
                new XElement("choosePeripherals", ChoosePeripherals.ToString()),
                new XElement("chooseTillLayouts", ChooseTillLayouts.ToString()),
                new XElement("chooseReceipts", ChooseReceipts.ToString()),
                new XElement("chooseRetailGroups", ChooseRetailGroups.ToString()),
                new XElement("choosePermissionGroup", ChoosePermissionGroup.ToString())
            );

            return xStoreSetting;
        }

        public override object Clone()
        {
            var template = new WizardTemplateView();
            Populate(template);
            return template;
        }

        protected void Populate(WizardTemplateView template)
        {
            template.ID = (RecordIdentifier)ID.Clone();
            template.Text = Text;
            template.Status = Status;
            template.DataAreaID = DataAreaID;
            template.StoreId = (RecordIdentifier)StoreId.Clone();
            template.TerminalId = (RecordIdentifier)TerminalId.Clone();
            template.TemplateImage = TemplateImage;
            template.LastExportDate = LastExportDate;
            template.Name = Name;
            template.Address = Address;
            template.PriceCalculation = PriceCalculation;
            template.ChooseNameAddress = ChooseNameAddress;
            template.ChoosePriceCalculation = ChoosePriceCalculation;
            template.VisualProfileID = (RecordIdentifier)VisualProfileID.Clone();
            template.FunctionalityProfileID = (RecordIdentifier)FunctionalityProfileID.Clone();
            template.StoreServerProfileID = (RecordIdentifier)StoreServerProfileID.Clone();
            template.DefaultCurrency = "dmp"; // DefaultCurrency;
            template.ChoosePayments = ChoosePayments;
            template.HardwareProfileID = (RecordIdentifier)HardwareProfileID.Clone();
            template.ChoosePeripherals = ChoosePeripherals;
            template.ChooseTillLayouts = ChooseTillLayouts;
            template.ChooseReceipts = ChooseReceipts;
            template.ChooseRetailGroups = ChooseRetailGroups;
            template.ChoosePermissionGroup = ChoosePermissionGroup;
        }
    }
}
