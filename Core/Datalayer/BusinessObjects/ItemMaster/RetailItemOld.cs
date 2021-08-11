using System;
using System.Collections.Generic;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster.ListItems;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// Data entity class for a retail item
    /// </summary>
    public partial class RetailItemOld : ItemListItemOld
    {        
        //TODO Delete this Enum
        /// <summary>
        /// Different types of retail item types that can be created
        /// </summary>
        public enum RetailItemTypeEnum
        {
            /// <summary>
            /// Standard item that cannot be sold on the POS
            /// </summary>
            StandardItem = 0,
            /// <summary>
            /// Standard retail item that can be sold on the POS
            /// </summary>
            RetailItem = 1,
            /// <summary>
            /// The item is a retail department
            /// </summary>
            RetailDepartment = 2,
            /// <summary>
            /// The item is a retail group
            /// </summary>
            RetailGroup = 3
        }

        /// <summary>
        /// Different types of configurations for entering a price for an item
        /// </summary>
        public enum KeyInPriceEnum
        {
            /// <summary>
            /// Keying in the price is not mandatory. Default configuration for a new item
            /// </summary>
            NotMandatory = 0,
            /// <summary>
            /// The cashier must key in a new price when the item is sold
            /// </summary>
            MustKeyInNewPrice = 1,
            /// <summary>
            /// The cashier must key in a higher or an equal price that the item already has
            /// </summary>
            MustKeyInHigherEqualPrice = 2,
            /// <summary>
            /// The cashier must key in a lowe ror an equal price that the item already has
            /// </summary>
            MustKeyInLowerEqualPrice = 3,
            /// <summary>
            /// The cashier cannot enter a new price
            /// </summary>
            MustNotKeyInNewPrice = 4
        }

        /// <summary>
        /// Different types of configurations for entering a quantity for an item
        /// </summary>
        public enum KeyInQuantityEnum
        {
            /// <summary>
            /// Keying in the quantity is not mandatory. Default configuration for a new item
            /// </summary>
            NotMandatory = 0,
            /// <summary>
            /// The cashier must key in a new quantity when the item is sold
            /// </summary>
            MustKeyInQuantity = 1,
            /// <summary>
            /// The cashier cannot enter a new quantity
            /// </summary>
            MustNotKeyInQuantity = 2
        }

        string notes;
        RecordIdentifier modelGroupID;
        RecordIdentifier dimensionGroupID;
        string dimensionGroupName;

        RecordIdentifier standardInventSizeID;
        string standardInventSizeName;
        ItemTypeEnum itemType;

        // From RBOINVENTTABLE
        RetailItemTypeEnum retailItemType;
        Date dateToBeBlocked;
        Date dateToActivateItem;
        RecordIdentifier barCodeSetupID;
        string barCodeSetupDescription;
        bool scaleItem;
        KeyInPriceEnum keyInPrice;
        KeyInQuantityEnum keyInQuantity;
        bool mustKeyInComment;
        bool mustSelectUOM;
        bool zeroPriceValid;
        bool quantityBecomesNegative;
        bool noDiscountAllowed;
        bool isFuelItem;
        string gradeID;
        RecordIdentifier sizeGroupID;
        string sizeGroupName;
        RecordIdentifier colorGroupID;
        string colorGroupName;
        RecordIdentifier styleGroupID;
        string styleGroupName;
        bool printVariantsShelfLabels;
        bool dirty;        

        RetailItemModule[] modules;

        /// <summary>
        /// Retail item constructor sets all variables to their default values
        /// </summary>
        public RetailItemOld()
            : base()
        {
            modules = new RetailItemModule[3];

            modules[0] = new RetailItemModule(ModuleTypeEnum.Inventory);
            modules[1] = new RetailItemModule(ModuleTypeEnum.Purchase);
            modules[2] = new RetailItemModule(ModuleTypeEnum.Sales);

            notes = "";

            modelGroupID = "";
            dimensionGroupID = "";
            dimensionGroupName = "";

            standardInventSizeID = "";
            standardInventSizeName = "";

            itemType = ItemTypeEnum.Item;
            retailItemType = RetailItemTypeEnum.RetailItem;
            dateToBeBlocked = Date.Empty;
            dateToActivateItem = Date.Empty;
            barCodeSetupID = RecordIdentifier.Empty;
            barCodeSetupDescription = "";
            scaleItem = false;
            keyInPrice = KeyInPriceEnum.NotMandatory;
            keyInQuantity = KeyInQuantityEnum.NotMandatory;
            mustKeyInComment = false;
            mustSelectUOM = false;
            zeroPriceValid = false;
            quantityBecomesNegative = false;
            noDiscountAllowed = false;
            isFuelItem = false;
            gradeID = "";
            sizeGroupID = "";
            colorGroupID = "";
            styleGroupID = "";
            printVariantsShelfLabels = false;
            sizeGroupName = "";
            colorGroupName = "";
            styleGroupName = "";
            RetailDivisionID = "";
            RetailDepartmentID = "";
            RetailGroupID = "";
            DefaultVendorItemId = "";
            dirty = false;

            //SetDefaults();
        }

        /// <summary>
        /// The unique ID of the retail item
        /// </summary>
        public override RecordIdentifier ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                modules[0].ItemID = value;
                modules[1].ItemID = value;
                modules[2].ItemID = value;

                base.ID = value;
            }
        }
                
        public string ValidationPeriodDescription { get; set; }

        /// <summary>
        /// Marks the master retail item record as dirty. The record will not be saved when calling Save if 
        /// the Dirty property is not true. If Dirty is false and Save is called then sub-records might be saved
        /// if one or more Dirty flags are true.
        /// </summary>
        public bool Dirty
        {
            get { return dirty; }
            set { dirty = value; }
        }

        /// <summary>
        /// The long note field on the Retail item card where notes about the item can be written.
        /// </summary>
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        /// <summary>
        /// Not currently used
        /// </summary>
        public RecordIdentifier ModelGroupID
        {
            get { return modelGroupID; }
            set { modelGroupID = value; }
        }

        /// <summary>
        /// The dimension group ID selected on the item
        /// </summary>
        public RecordIdentifier DimensionGroupID
        {
            get { return dimensionGroupID; }
            set { dimensionGroupID = value; }
        }

        /// <summary>
        /// The selected dimension group description
        /// </summary>
        public string DimensionGroupName
        {
            get { return dimensionGroupName; }
            internal set { dimensionGroupName = value; }
        }

        /// <summary>
        /// Not currently used
        /// </summary>
        public RecordIdentifier StandardInventSizeID
        {
            get { return standardInventSizeID; }
            set { standardInventSizeID = value; }
        }

        /// <summary>
        /// Not currently used
        /// </summary>
        public string StandardInventSizeName
        {
            get { return standardInventSizeName; }
            set { standardInventSizeName = value; }
        }

        /// <summary>
        /// The item cannot be sold on the POS on or after this date.
        /// </summary>
        public Date DateToBeBlocked
        {
            get { return dateToBeBlocked; }
            set { dateToBeBlocked = value; }
        }

        /// <summary>
        /// Item types that can be created on the Site Manager to be sold on the LS POS. Currently the Site Manager only supports Item
        /// </summary>
        public new ItemTypeEnum ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }

        /// <summary>
        /// Not currently used.
        /// </summary>
        public RetailItemTypeEnum RetailItemType
        {
            get { return retailItemType; }
            set { retailItemType = value; }
        }

        /// <summary>
        /// The currently selected barcode setup on the retail item
        /// </summary>
        public RecordIdentifier BarCodeSetupID
        {
            get { return barCodeSetupID; }
            set { barCodeSetupID = value; }
        }

        /// <summary>
        /// The barcode setup description
        /// </summary>
        public string BarCodeSetupDescription
        {
            get { return barCodeSetupDescription; }
            set { barCodeSetupDescription = value; }
        }

        /// <summary>
        /// If true the item is a scale item and will get the quantity from a scale
        /// </summary>
        public bool ScaleItem
        {
            get { return scaleItem; }
            set { scaleItem = value; }
        }

        /// <summary>
        /// When the item is sold this configuration controls how or if the price can be changed manually
        /// </summary>
        public KeyInPriceEnum KeyInPrice
        {
            get { return keyInPrice; }
            set { keyInPrice = value; }
        }

        /// <summary>
        /// When the item is sold this configuration controls how or if the quantity can be changed manually
        /// </summary>
        public KeyInQuantityEnum KeyInQuantity
        {
            get { return keyInQuantity; }
            set { keyInQuantity = value; }
        }

        /// <summary>
        /// If true the cashier must enter a comment when the item is sold
        /// </summary>
        public bool MustKeyInComment
        {
            get { return mustKeyInComment; }
            set { mustKeyInComment = value; }
        }

        /// <summary>
        /// If true the cashier must select a UOM when the item is sold
        /// </summary>
        public bool MustSelectUOM
        {
            get { return mustSelectUOM; }
            set { mustSelectUOM = value; }
        }

        /// <summary>
        /// If true then the item can have a price of zero
        /// </summary>
        public bool ZeroPriceValid
        {
            get { return zeroPriceValid; }
            set { zeroPriceValid = value; }
        }

        /// <summary>
        /// If true the quantity of the item automatically becomes negative when the item is sold
        /// </summary>
        public bool QuantityBecomesNegative
        {
            get { return quantityBecomesNegative; }
            set { quantityBecomesNegative = value; }
        }

        /// <summary>
        /// If true no discounts are allowed on the item
        /// </summary>
        public bool NoDiscountAllowed
        {
            get { return noDiscountAllowed; }
            set { noDiscountAllowed = value; }
        }

        /// <summary>
        /// The item cannot be sold on the POS until on or after this date.
        /// </summary>
        public Date DateToActivateItem
        {
            get { return dateToActivateItem; }
            set { dateToActivateItem = value; }
        }

        /// <summary>
        /// If true then the item is a fuel item
        /// </summary>
        public bool IsFuelItem
        {
            get { return isFuelItem; }
            set { isFuelItem = value; }
        }

        /// <summary>
        /// If the item is a fuel item the fuel grade needs to be set
        /// </summary>
        public string GradeID
        {
            get { return gradeID; }
            set { gradeID = value; }
        }

        /// <summary>
        /// If the item has a size variant this is the ID of the size group that the combinations are created from
        /// </summary>
        public RecordIdentifier SizeGroupID
        {
            get { return sizeGroupID; }
            set { sizeGroupID = value; }
        }

        /// <summary>
        /// If the item has a color variant this is the ID of the color group that the combinations are created from
        /// </summary>
        public RecordIdentifier ColorGroupID
        {
            get { return colorGroupID; }
            set { colorGroupID = value; }
        }

        /// <summary>
        /// The style group description
        /// </summary>
        public string StyleGroupName
        {
            get { return styleGroupName; }
            set { styleGroupName = value; }
        }

        /// <summary>
        /// The size group description
        /// </summary>
        public string SizeGroupName
        {
            get { return sizeGroupName; }
            set { sizeGroupName = value; }
        }

        /// <summary>
        /// The color group description
        /// </summary>
        public string ColorGroupName
        {
            get { return colorGroupName; }
            set { colorGroupName = value; }
        }

        /// <summary>
        /// If the item has a style variant this is the ID of the style group that the combinations are created from
        /// </summary>
        public RecordIdentifier StyleGroupID
        {
            get { return styleGroupID; }
            set { styleGroupID = value; }
        }

        /// <summary>
        /// Not currently used
        /// </summary>
        public bool PrintVariantsShelfLabels
        {
            get { return printVariantsShelfLabels; }
            set { printVariantsShelfLabels = value; }
        }
        /// <summary>
        /// The vendor item that has been selected as the default vendor item on the item
        /// </summary>
        public RecordIdentifier DefaultVendorItemId { get; set; }

        /// <summary>
        /// The ID of the retail sub-group the item belongs to
        /// </summary>
        public RecordIdentifier RetailDivisionID { get; set; }
        /// <summary>
        /// The ID of the retail group the item belongs to
        /// </summary>
        public RecordIdentifier RetailGroupID { get; set; }
        /// <summary>
        /// The ID of the retail department the item belongs to
        /// </summary>
        public RecordIdentifier RetailDepartmentID { get; set; }
        /// <summary>
        /// The profit margin of the product
        /// </summary>
        public decimal ProfitMargin { get; set; }
        public bool BlockedOnPOS { get; set; }


        public RetailItemModule this[ModuleTypeEnum moduleType]
        {
            get
            {
                return modules[(int)moduleType];
            }
        }

        internal void AddModule(RetailItemModule module)
        {
            modules[(int)module.ModuleType] = module;
        }

        public override object this[string field]
        {
            get
            {
                if (field == "BarCodeSetupID")
                {
                    return barCodeSetupID;
                }

                return null;
            }
            set
            {
                if (field == "BarCodeSetupID")
                {
                    barCodeSetupID = (RecordIdentifier)value;
                }
            }
        }

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
                            case "itemId":
                                ID = current.Value;
                                break;
                            case "itemName":
                                Text = current.Value;
                                break;
                            case "nameAlias":
                                NameAlias = current.Value;
                                break;
                            case "itemType":
                                ItemType = (ItemTypeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "dimensionGroupID":
                                DimensionGroupID = current.Value;
                                break;
                            case "dimensionGroupName":
                                DimensionGroupName = current.Value;
                                break;
                            case "retailItemType":
                                RetailItemType = (RetailItemTypeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "dateToBeBlocked":
                                DateToBeBlocked = new Date(Date.XmlStringToDateTime(current.Value));
                                break;
                            case "barCodeSetupID":
                                barCodeSetupID = current.Value;
                                break;
                            case "barCodeSetupDescription":
                                barCodeSetupDescription =current.Value;
                                break;
                            case "scaleItem":
                                ScaleItem = current.Value == "true";
                                break;
                            case "keyInPrice":
                                KeyInPrice = (KeyInPriceEnum)Convert.ToInt32(current.Value);
                                break;
                            case "keyInQuantity":
                                KeyInQuantity = (KeyInQuantityEnum)Convert.ToInt32(current.Value);
                                break;
                            case "mustKeyInComment":
                                MustKeyInComment = current.Value == "true";
                                break;
                            case "mustSelectUOM":
                                mustSelectUOM = current.Value == "true";
                                break;
                            case "zeroPriceValid":
                                ZeroPriceValid = current.Value == "true";
                                break;
                            case "quantityBecomesNegative":
                                QuantityBecomesNegative = current.Value == "true";
                                break;
                            case "noDiscountAllowed":
                                NoDiscountAllowed = current.Value == "true";
                                break;
                            case "dateToActivateItem":
                                DateToActivateItem = new Date(Date.XmlStringToDateTime(current.Value));
                                break;
                            case "isFuelItem":
                                IsFuelItem = current.Value == "true";
                                break;
                            case "gradeID":
                                GradeID = current.Value;
                                break;
                            case "sizeGroupID":
                                SizeGroupID = current.Value;
                                break;
                            case "colorGroupID":
                                ColorGroupID = current.Value;
                                break;
                            case "styleGroupID":
                                StyleGroupID = current.Value;
                                break;
                            case "printVariantsShelfLabels":
                                PrintVariantsShelfLabels = current.Value == "true";
                                break;
                            case "defaultVendorID":
                                DefaultVendorItemId = current.Value;
                                break;
                            case "validationPeriod":
                                ValidationPeriod = current.Value;
                                break;
                            case "sizeGroupName":
                                SizeGroupName = current.Value;
                                break;
                            case "colorGroupName":
                                colorGroupName = current.Value;
                                break;
                            case "styleGroupName":
                                styleGroupName = current.Value;
                                break;
                            case "retailDepartmentID":
                                RetailDepartmentID = current.Value;
                                break;
                            case "retailGroupID":
                                RetailGroupID = current.Value;
                                break;
                            case "retailDepartmentName":
                                RetailDepartmentName = current.Value;
                                break;
                            case "retailGroupName":
                                RetailGroupName = current.Value;
                                break;
                            case "profitMargin":
                                ProfitMargin = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "validationPeriodDescription":
                                ValidationPeriodDescription = current.Value;
                                break;
                            case "blockedOnPOS":
                                BlockedOnPOS = current.Value == "true";
                                break;
                            case "CustomFields":
                                //CustomFieldsToClass(current);
                                break;

                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(),
                                                   ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("RetailItem",
                    new XElement("itemId", (string)ID),
                    new XElement("itemName", Text),
                    new XElement("nameAlias", NameAlias),
                    new XElement("itemType", (int)ItemType),
                    new XElement("dimensionGroupID", DimensionGroupID),
                    new XElement("dimensionGroupName", DimensionGroupName),
                    new XElement("retailItemType", (int)RetailItemType),
                    new XElement("dateToBeBlocked", new Date(DateToBeBlocked).ToXmlString()),
                    new XElement("barCodeSetupID", barCodeSetupID),
                    new XElement("barCodeSetupDescription", BarCodeSetupDescription),
                    new XElement("scaleItem", ScaleItem),
                    new XElement("keyInPrice", (int)KeyInPrice),
                    new XElement("keyInQuantity", (int)KeyInQuantity),
                    new XElement("mustKeyInComment", MustKeyInComment),
                    new XElement("mustSelectUOM", mustSelectUOM),
                    new XElement("zeroPriceValid", ZeroPriceValid),
                    new XElement("quantityBecomesNegative", QuantityBecomesNegative),
                    new XElement("noDiscountAllowed", NoDiscountAllowed),
                    new XElement("dateToActivateItem", new Date(DateToActivateItem).ToXmlString()),
                    new XElement("isFuelItem", IsFuelItem),
                    new XElement("gradeID", GradeID),
                    new XElement("sizeGroupID", SizeGroupID),
                    new XElement("colorGroupID", ColorGroupID),
                    new XElement("styleGroupID", StyleGroupID),
                    new XElement("printVariantsShelfLabels", PrintVariantsShelfLabels),
                    new XElement("defaultVendorID", DefaultVendorItemId),
                    new XElement("validationPeriod", ValidationPeriod),
                    new XElement("sizeGroupName", sizeGroupName),
                    new XElement("colorGroupName", colorGroupName),
                    new XElement("styleGroupName", styleGroupName),
                    new XElement("retailDepartmentID", RetailDepartmentID),
                    new XElement("retailGroupID", RetailGroupID),
                    new XElement("retailDepartmentName", RetailDepartmentName),
                    new XElement("retailGroupName", RetailGroupName),
                    new XElement("noDiscountAllowed", NoDiscountAllowed),
                    new XElement("profitMargin", ProfitMargin.ToString(XmlCulture)),
                    new XElement("validationPeriod", ValidationPeriod),
                    new XElement("validationPeriodDescription", ValidationPeriodDescription),
                    new XElement("blockedOnPOS", BlockedOnPOS));

            //CustomFieldsToXML(xml);

            return xml;
        }
    }
}
