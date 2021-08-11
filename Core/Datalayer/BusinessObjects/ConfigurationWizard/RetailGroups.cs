using System;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.ConfigurationWizard
{
    /// <summary>
    /// Business entity class of RetailGroups page
    /// </summary>
    public class RetailGroups : DataEntity
    {
        public RetailGroups()
        {
            RetailGroupID = string.Empty;
            RetailGroup = new RetailGroup();
            RetailItem = new RetailItemOld();
            RetailItemModule = new RetailItemOld.RetailItemModule();
        }

        /// <summary>
        /// RetailGroupID used in WIZARDTEMPLATERETAILGROUPS table
        /// </summary>
        public string RetailGroupID { get; set; }

        /// <summary>
        /// Object of standard RetailGroup class.
        /// </summary>
        public RetailGroup RetailGroup { get; set; }

        /// <summary>
        /// Creating object of standard RetailItem class.
        /// </summary>
        public RetailItemOld RetailItem { get; set; }

        /// <summary>
        /// Creating object of standard RetailItemModule class.
        /// </summary>
        public RetailItemOld.RetailItemModule RetailItemModule { get; set; }

        /// <summary>
        /// Sets all variables in the RetailGroup class with the values in the xml
        /// </summary>
        /// <param name="xRetailGroups">The xml element with the retail group  values</param>
        /// <param name="errorLogger"></param>
        public override void ToClass(XElement xRetailGroups, IErrorLog errorLogger = null)
        {
            if (xRetailGroups.Name == "RetailGroup")
            {
                var storeVariables = xRetailGroups.Elements();
                foreach (var retailElem in storeVariables)
                {
                    //No retail group id -> no Retail group setting -> no need to go any further
                    if (retailElem.Name.ToString() == "groupID" && retailElem.Value == "")
                    {
                        return;
                    }

                    if (!retailElem.IsEmpty)
                    {
                        try
                        {
                            switch (retailElem.Name.ToString())
                            {
                                case "groupID":
                                    RetailGroup.ID = retailElem.Value;
                                    break;
                                case "name":
                                    RetailGroup.Text = retailElem.Value;
                                    break;
                                case "retailDepartmentId":
                                    RetailGroup.RetailDepartmentID = retailElem.Value;
                                    break;
                                case "retailDepartmentName":
                                    RetailGroup.RetailDepartmentName = retailElem.Value;
                                    break;
                                case "itemSalesTaxGroupId":
                                    RetailGroup.ItemSalesTaxGroupId = retailElem.Value;
                                    break;
                                case "itemSalesTaxGroupName":
                                    RetailGroup.ItemSalesTaxGroupName = retailElem.Value;
                                    break;
                                case "profitMargin":
                                    RetailGroup.ProfitMargin = Convert.ToDecimal(retailElem.Value);
                                    break;
                                case "tareWeight":
                                    RetailGroup.TareWeight = Convert.ToInt32(retailElem.Value);
                                    break;
                                case "validationPeriod":
                                    RetailGroup.ValidationPeriod = retailElem.Value;
                                    break;
                                case "validationPeriodDescription":
                                    RetailGroup.ValidationPeriodDescription = retailElem.Value;
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, retailElem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }


            if (xRetailGroups.Name == "RetailItem")
            {
                var storeVariables = xRetailGroups.Elements();
                foreach (var retailElem in storeVariables)
                {
                    //No retail group id -> no Retail group setting -> no need to go any further
                    if (retailElem.Name.ToString() == "groupID" && retailElem.Value == "")
                    {
                        return;
                    }

                    if (!retailElem.IsEmpty)
                    {
                        try
                        {
                            switch (retailElem.Name.ToString())
                            {
                                case "itemId":
                                    RetailItem.ID = retailElem.Value;
                                    break;
                                case "itemName":
                                    RetailItem.Text = retailElem.Value;
                                    break;
                                case "nameAlias":
                                    RetailItem.NameAlias = retailElem.Value;
                                    break;
                                case "itemType":
                                    RetailItem.ItemType = (ItemTypeEnum)Convert.ToInt32(retailElem.Value);
                                    break;
                                case "notes":
                                    RetailItem.Notes = retailElem.Value;
                                    break;
                                case "modelGroupID":
                                    RetailItem.ModelGroupID = retailElem.Value;
                                    break;
                                case "dimensionGroupID":
                                    RetailItem.DimensionGroupID = retailElem.Value;
                                    break;
                                case "dimensionGroupName":
                                    RetailItem.DimensionGroupName = retailElem.Value;
                                    break;
                                case "retailItemType":
                                    RetailItem.RetailItemType = (RetailItemOld.RetailItemTypeEnum)Convert.ToInt32(retailElem.Value);
                                    break;
                                case "dateToBeBlocked":
                                    RetailItem.DateToBeBlocked = new Date(Date.XmlStringToDateTime(retailElem.Value));
                                    break;
                                case "barCodeSetupID":
                                    RetailItem.BarCodeSetupID = retailElem.Value;
                                    break;
                                case "barCodeSetupDescription":
                                    RetailItem.BarCodeSetupDescription = retailElem.Value;
                                    break;
                                case "scaleItem":
                                    RetailItem.ScaleItem = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "keyInPrice":
                                    RetailItem.KeyInPrice = (RetailItemOld.KeyInPriceEnum)Convert.ToInt32(retailElem.Value);
                                    break;
                                case "keyInQuantity":
                                    RetailItem.KeyInQuantity = (RetailItemOld.KeyInQuantityEnum)Convert.ToInt32(retailElem.Value);
                                    break;
                                case "mustKeyInComment":
                                    RetailItem.MustKeyInComment = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "mustSelectUOM":
                                    RetailItem.MustSelectUOM = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "zeroPriceValid":
                                    RetailItem.ZeroPriceValid = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "quantityBecomesNegative":
                                    RetailItem.QuantityBecomesNegative = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "noDiscountAllowed":
                                    RetailItem.NoDiscountAllowed = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "dateToActivateItem":
                                    RetailItem.DateToActivateItem = new Date(Date.XmlStringToDateTime(retailElem.Value));
                                    break;
                                case "isFuelItem":
                                    RetailItem.IsFuelItem = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "gradeID":
                                    RetailItem.GradeID = retailElem.Value;
                                    break;
                                case "sizeGroupId":
                                    RetailItem.SizeGroupID = retailElem.Value;
                                    break;
                                case "colorGroupID":
                                    RetailItem.ColorGroupID = retailElem.Value;
                                    break;
                                case "styleGroupID":
                                    RetailItem.StyleGroupID = retailElem.Value;
                                    break;
                                case "printVariantsShelfLabels":
                                    RetailItem.PrintVariantsShelfLabels = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "defaultVendorID":
                                    RetailItem.DefaultVendorItemId = retailElem.Value;
                                    break;                                
                                case "validationPeriod":
                                    RetailItem.ValidationPeriod = retailElem.Value;
                                    break;
                                case "sizeGroupName":
                                    RetailItem.SizeGroupName = retailElem.Value;
                                    break;
                                case "colorGroupName":
                                    RetailItem.ColorGroupName = retailElem.Value;
                                    break;
                                case "styleGroupName":
                                    RetailItem.StyleGroupName = retailElem.Value;
                                    break;
                                case "retailDepartmentID":
                                    RetailItem.RetailDepartmentID = retailElem.Value;
                                    break;                                
                                case "retailGroupID":
                                    RetailItem.RetailGroupID = retailElem.Value;
                                    break;
                                case "retailDepartmentName":
                                    RetailItem.RetailDepartmentName = retailElem.Value;
                                    break;
                                case "retailGroupName":
                                    RetailItem.RetailGroupName = retailElem.Value;
                                    break;                                
                                case "profitMargin":
                                    RetailItem.ProfitMargin = Convert.ToDecimal(retailElem.Value);
                                    break;
                                case "dirty":
                                    RetailItem.Dirty = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "validationPeriodDescription":
                                    RetailItem.ValidationPeriodDescription = retailElem.Value;
                                    break;
                                case "blockedOnPOS":
                                    RetailItem.BlockedOnPOS = Convert.ToBoolean(retailElem.Value);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, retailElem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }

            if (xRetailGroups.Name == "RetailItemModule")
            {
                var storeVariables = xRetailGroups.Elements();
                foreach (var retailElem in storeVariables)
                {
                    //No retail group id -> no Retail group setting -> no need to go any further
                    if (retailElem.Name.ToString() == "groupID" && retailElem.Value == "")
                    {
                        return;
                    }

                    if (!retailElem.IsEmpty)
                    {
                        try
                        {
                            switch (retailElem.Name.ToString())
                            {
                                case "itemID":
                                    RetailItemModule.ItemID = retailElem.Value;
                                    break;
                                case "moduleType":
                                    RetailItemModule.ModuleType = (RetailItemOld.ModuleTypeEnum)Convert.ToInt32(retailElem.Value);
                                    break;
                                case "allocateMarkup":
                                    RetailItemModule.AllocateMarkup = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "dirty":
                                    RetailItemModule.Dirty = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "lastKnownPriceWithTax":
                                    RetailItemModule.LastKnownPriceWithTax = Convert.ToDecimal(retailElem.Value);
                                    break;
                                case "lineDiscount":
                                    RetailItemModule.LineDiscount = retailElem.Value;
                                    break;
                                case "lineDiscountName":
                                    RetailItemModule.LineDiscountName = retailElem.Value;
                                    break;
                                case "markup":
                                    RetailItemModule.Markup = Convert.ToDecimal(retailElem.Value);
                                    break;
                                case "multilineDiscount":
                                    RetailItemModule.MultilineDiscount = retailElem.Value;
                                    break;
                                case "multiLineDiscountName":
                                    RetailItemModule.MultiLineDiscountName = retailElem.Value;
                                    break;
                                case "price":
                                    RetailItemModule.Price = Convert.ToDecimal(retailElem.Value);
                                    break;
                                case "priceDate":
                                    RetailItemModule.PriceDate = new Date(Date.XmlStringToDateTime(retailElem.Value));
                                    break;
                                case "priceQty":
                                    RetailItemModule.PriceQty = Convert.ToDecimal(retailElem.Value);
                                    break;
                                case "priceUnit":
                                    RetailItemModule.PriceUnit = Convert.ToDecimal(retailElem.Value);
                                    break;
                                case "taxItemGroupID":
                                    RetailItemModule.TaxItemGroupID = retailElem.Value;
                                    break;
                                case "taxItemGroupName":
                                    RetailItemModule.TaxItemGroupName = retailElem.Value;
                                    break;
                                case "totalDiscount":
                                    RetailItemModule.TotalDiscount = Convert.ToBoolean(retailElem.Value);
                                    break;
                                case "unit":
                                    RetailItemModule.Unit = retailElem.Value;
                                    break;
                                case "unitText":
                                    RetailItemModule.UnitText = retailElem.Value;
                                    break;
                                case "usageIntent":
                                    RetailItemModule.UsageIntent = (UsageIntentEnum)Convert.ToInt32(retailElem.Value);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            if (errorLogger != null)
                            {
                                errorLogger.LogMessage(LogMessageType.Error, retailElem.Name.ToString(), ex);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an xml element from all the variables in the RetailGroup class
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
            XElement retailGroup = null;
            if (RetailGroup.ID != RecordIdentifier.Empty)
            {
                retailGroup = new XElement("RetailGroup",
                    new XElement("groupID", (string)RetailGroup.ID),
                    new XElement("name", RetailGroup.Text),
                    new XElement("retailDepartmentId", RetailGroup.RetailDepartmentID),
                    new XElement("retailDepartmentName", RetailGroup.RetailDepartmentName),
                    new XElement("itemSalesTaxGroupId", (string)RetailGroup.ItemSalesTaxGroupId),
                    new XElement("itemSalesTaxGroupName", RetailGroup.ItemSalesTaxGroupName),
                    new XElement("profitMargin", RetailGroup.ProfitMargin),
                    new XElement("validationPeriod", RetailGroup.ValidationPeriod),
                    new XElement("validationPeriodDescription", RetailGroup.ValidationPeriodDescription),
                    new XElement("tareWeight", RetailGroup.TareWeight)
                );
            }
            if (RetailItem.ID != RecordIdentifier.Empty)
            {
                retailGroup = new XElement("RetailItem",
                    new XElement("itemId", (string)RetailItem.ID),
                    new XElement("itemName", RetailItem.Text),
                    new XElement("nameAlias", RetailItem.NameAlias),
                    new XElement("itemType", (int)RetailItem.ItemType),
                    new XElement("notes", RetailItem.Notes),
                    new XElement("modelGroupID", RetailItem.ModelGroupID),
                    new XElement("dimensionGroupID", RetailItem.DimensionGroupID),
                    new XElement("dimensionGroupName", RetailItem.DimensionGroupName),
                    new XElement("retailItemType", (int)RetailItem.RetailItemType),                    
                    new XElement("dateToBeBlocked", RetailItem.DateToBeBlocked.ToXmlString()),
                    new XElement("barCodeSetupID", RetailItem.BarCodeSetupID),
                    new XElement("barCodeSetupDescription", RetailItem.BarCodeSetupDescription),                    
                    new XElement("scaleItem", RetailItem.ScaleItem),
                    new XElement("keyInPrice", (int)RetailItem.KeyInPrice),
                    new XElement("keyInQuantity", (int)RetailItem.KeyInQuantity),
                    new XElement("mustKeyInComment", RetailItem.MustKeyInComment),
                    new XElement("mustSelectUOM", RetailItem.MustSelectUOM),
                    new XElement("zeroPriceValid", RetailItem.ZeroPriceValid),
                    new XElement("quantityBecomesNegative", RetailItem.QuantityBecomesNegative),
                    new XElement("noDiscountAllowed", RetailItem.NoDiscountAllowed),
                    new XElement("dateToActivateItem", RetailItem.DateToActivateItem.ToXmlString()),
                    new XElement("isFuelItem", RetailItem.IsFuelItem),
                    new XElement("gradeID", RetailItem.GradeID),
                    new XElement("sizeGroupID", RetailItem.SizeGroupID),
                    new XElement("colorGroupID", RetailItem.ColorGroupID),
                    new XElement("styleGroupID", RetailItem.StyleGroupID),
                    new XElement("printVariantsShelfLabels", RetailItem.PrintVariantsShelfLabels),
                    new XElement("defaultVendorID", RetailItem.DefaultVendorItemId),
                    new XElement("validationPeriod", RetailItem.ValidationPeriod),
                    new XElement("sizeGroupName", RetailItem.SizeGroupName),
                    new XElement("colorGroupName", RetailItem.ColorGroupName),
                    new XElement("styleGroupName", RetailItem.StyleGroupName),
                    new XElement("retailDepartmentID", RetailItem.RetailDepartmentID),
                    new XElement("retailGroupID", RetailItem.RetailGroupID),
                    new XElement("retailDepartmentName", RetailItem.RetailDepartmentName),
                    new XElement("retailGroupName", RetailItem.RetailGroupName),
                    new XElement("profitMargin", RetailItem.ProfitMargin),
                    new XElement("dirty", RetailItem.Dirty),
                    new XElement("validationPeriodDescription", RetailItem.ValidationPeriodDescription),
                    new XElement("blockedOnPOS", RetailItem.BlockedOnPOS)
                    );
            }
            if (RetailItemModule.ID.StringValue != string.Empty)
            {
                retailGroup = new XElement("RetailItemModule",
                    new XElement("itemID", (string)RetailItemModule.ItemID),
                    new XElement("moduleType", (int)RetailItemModule.ModuleType),
                    new XElement("allocateMarkup", RetailItemModule.AllocateMarkup),
                    new XElement("dirty", RetailItemModule.Dirty),
                    new XElement("lastKnownPriceWithTax", RetailItemModule.LastKnownPriceWithTax),
                    new XElement("lineDiscount", (string)RetailItemModule.LineDiscount),
                    new XElement("lineDiscountName", RetailItemModule.LineDiscountName),
                    new XElement("markup", RetailItemModule.Markup),
                    new XElement("multilineDiscount", (string)RetailItemModule.MultilineDiscount),
                    new XElement("multiLineDiscountName", RetailItemModule.MultiLineDiscountName),
                    new XElement("price", RetailItemModule.Price),
                    new XElement("priceDate", RetailItemModule.PriceDate.ToXmlString()),
                    new XElement("priceQty", RetailItemModule.PriceQty),
                    new XElement("priceUnit", RetailItemModule.PriceUnit),
                    new XElement("taxItemGroupID", (string)RetailItemModule.TaxItemGroupID),
                    new XElement("taxItemGroupName", RetailItemModule.TaxItemGroupName),
                    new XElement("totalDiscount", RetailItemModule.TotalDiscount),
                    new XElement("unit", (string)RetailItemModule.Unit),
                    new XElement("unitText", RetailItemModule.UnitText),
                    new XElement("usageIntent", (int)RetailItemModule.UsageIntent)
                    );
            }
            return retailGroup;
        }
    }
}
