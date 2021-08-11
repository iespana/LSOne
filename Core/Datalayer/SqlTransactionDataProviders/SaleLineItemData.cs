using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.DataLayer.SqlTransactionDataProviders.Properties;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.CreditMemoItem;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.Price;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class SaleLineItemData : SqlServerDataProviderBase, ISaleLineItemData
    {
        private static SaleLineItem PopulateSaleLineItem(IConnectionManager entry, IDataReader dr, RetailTransaction transaction, object param2)
        {
            SaleLineItem saleLine = null;
            SalesTransaction.ItemClassTypeEnum itemType;
            if (dr["ITEMTYPE"] == DBNull.Value)
            {
                if ((byte)dr["GIFTCARD"] != 0)
                {
                    itemType = SalesTransaction.ItemClassTypeEnum.GiftCertificateItem;
                }
                else if ((string)dr["PRESCRIPTIONID"] != "")
                {
                   itemType = SalesTransaction.ItemClassTypeEnum.PharmacySalesLineItem;
                }
                else if ((int)dr["PUMPID"] != 0)
                {
                   itemType = SalesTransaction.ItemClassTypeEnum.FuelSalesLineItem;
                }
                else if ((int)dr["SOURCELINENUM"] > 0)
                {
                   itemType = SalesTransaction.ItemClassTypeEnum.DiscountVoucherItem;
                }
                else if ((string)dr["ITEMID"] == "")
                {
                   itemType = SalesTransaction.ItemClassTypeEnum.CreditMemo;
                }
                else
                {
                   itemType = SalesTransaction.ItemClassTypeEnum.SaleLineItem;
                }
            }
            else
            {
                itemType = (SalesTransaction.ItemClassTypeEnum)dr["ITEMTYPE"];
            }

            //Create the sale line item depending on the Item class type
            switch (itemType)
            {
                case SalesTransaction.ItemClassTypeEnum.DiscountVoucherItem:
                    saleLine = new DiscountVoucherItem(transaction, (string)dr["ITEMID"], (string)dr["DESCRIPTION"], (decimal)dr["PRICE"]);
                    ((DiscountVoucherItem)saleLine).SourceLineNum = (int)dr["SOURCELINENUM"];
                    saleLine.OriginalDiscountVoucherPriceWithTax = saleLine.PriceWithTax; //Price with tax already been set in the constructor
                    break;
                case SalesTransaction.ItemClassTypeEnum.CreditMemo:
                    saleLine = new CreditMemoItem(transaction);
                    ((CreditMemoItem)saleLine).CreditMemoNumber = (string)dr["CREDITMEMONUMBER"];
                    ((CreditMemoItem)saleLine).Amount = (decimal)dr["PRICE"];
                    break;
                case SalesTransaction.ItemClassTypeEnum.FuelSalesLineItem:
                    saleLine = new FuelSalesLineItem(new BarCode(), transaction);
                    saleLine = TransactionProviders.FuelSalesLineItemData.Get(entry, new RecordIdentifier(transaction.TransactionId, saleLine.LineId), transaction);
                    break;
                case SalesTransaction.ItemClassTypeEnum.GiftCertificateItem:
                    saleLine = new GiftCertificateItem(transaction);
                    ((GiftCertificateItem)saleLine).SerialNumber = dr["COMMENT"].ToString();
                    ((GiftCertificateItem)saleLine).Amount = Convert.ToDecimal(dr["PRICE"]);
                // Necessary property settings for the the gift certificate "item"...
                    saleLine.Price = ((GiftCertificateItem)saleLine).Amount;
                    saleLine.PriceWithTax = ((GiftCertificateItem)saleLine).Amount;
                    saleLine.StandardRetailPrice = ((GiftCertificateItem)saleLine).Amount;
                    saleLine.Quantity = 1;
                    saleLine.TaxRatePct = 0;
                    saleLine.Description = Resources.GiftCard;  // Gift Card
                    saleLine.Comment = ((GiftCertificateItem)saleLine).SerialNumber;
                    saleLine.NoDiscountAllowed = true;
                    saleLine.Found = true;
                    break;
                case SalesTransaction.ItemClassTypeEnum.PharmacySalesLineItem:
                    saleLine = new PharmacySalesLineItem(transaction);
                    ((PharmacySalesLineItem)saleLine).PrescriptionId = (string)dr["PRESCRIPTIONID"];
                    ((PharmacySalesLineItem)saleLine).DosageType = (string)dr["DOSAGETYPE"];
                    ((PharmacySalesLineItem)saleLine).DosageStrength = (decimal)dr["DOSAGESTRENGTH"];
                    ((PharmacySalesLineItem)saleLine).DosageStrengthUnit = (string)dr["DOSAGESTRENGTHUNIT"];
                    ((PharmacySalesLineItem)saleLine).DosageUnitQuantiy = (decimal)dr["DOSAGEUNITQUANTITY"];
                    break;
            }

            if (saleLine == null)
            {
                saleLine = new SaleLineItem(transaction);
            }
            saleLine.ItemClassType = itemType;
            saleLine.LineId = (int)(decimal)dr["LINENUM"];
            saleLine.Transaction = transaction;
            saleLine.Voided = ((int)dr["TRANSACTIONSTATUS"] == (int)TransactionStatus.Voided);
            saleLine.BarcodeId = (string)dr["BARCODE"];
            saleLine.PriceID = (string)dr["PRICEID"];
            saleLine.PriceType = (TradeAgreementPriceType)((int)dr["PRICETYPE"]);
            saleLine.ItemId = (string)dr["ITEMID"];
            saleLine.TaxGroupId = (string)dr["TAXGROUP"];
            saleLine.TaxAmount = (decimal)dr["TAXAMOUNT"] * -1;
            saleLine.PriceWithTax = (decimal)dr["PRICE"];
            saleLine.Price = (decimal)dr["NETPRICE"];
            saleLine.NetAmount = (decimal)dr["NETAMOUNT"] * -1;
            if (dr["NETAMOUNTINCLTAX"] == DBNull.Value)
            {
                saleLine.NetAmountWithTax = saleLine.NetAmount + saleLine.TaxAmount;
            }
            else
            {
                saleLine.NetAmountWithTax = Convert.ToDecimal(dr["NETAMOUNTINCLTAX"]) * -1;
            }
            saleLine.Quantity = (decimal)dr["QTY"] * -1;
            if (!string.IsNullOrEmpty(transaction.OrgTransactionId))
            {
                saleLine.OriginalQuantity = saleLine.Quantity;
            }
            saleLine.GrossAmount = saleLine.Price * saleLine.Quantity;
            saleLine.GrossAmountWithTax = saleLine.PriceWithTax * saleLine.Quantity;
            saleLine.BeginDateTime = (DateTime)dr["TRANSDATE"];
            saleLine.EntryType = BarCode.BarcodeEntryType.ManuallyEntered;
            if ((byte)dr["ITEMIDSCANNED"] != 0)
            {
                saleLine.EntryType = BarCode.BarcodeEntryType.SingleScanned;
            }
            saleLine.SalesPerson.ID = dr["STAFF"].ToString();                
            saleLine.SalesOrderUnitOfMeasure = (string)dr["UNIT"];
            saleLine.UnitQuantity = (decimal)dr["UNITQTY"] * -1;
            saleLine.SerialId = (string)dr["INVENTSERIALID"];
            saleLine.RFIDTagId = (string)dr["RFIDTAGID"];
            saleLine.Comment = dr["COMMENT"].ToString();

            saleLine.TaxIncludedInItemPrice = transaction.TaxIncludedInPrice;
            if (dr["TAXINCLINPRICE"] != DBNull.Value)
            {
                saleLine.TaxIncludedInItemPrice = ((byte)dr["TAXINCLINPRICE"] != 0);
            }
            saleLine.DiscountsWereRemoved = false; // is never given value in code - is always false                    
            //TODO remove variant id
            saleLine.Dimension.VariantNumber = (string)dr["VARIANTID"];
            saleLine.BatchId = (string)dr["INVENTBATCHID"];
            saleLine.BatchExpDate = (DateTime)dr["INVENTBATCHEXPDATE"];
            saleLine.Oiltax = (decimal)dr["OILTAX"];
            saleLine.ReturnLineId = (int)(decimal)dr["RETURNLINEID"];
            saleLine.ReturnedQty = (decimal)dr["RETURNQTY"];
            saleLine.IsLinkedItem = ((byte)dr["LINKEDITEMNOTORIGINAL"] != 0);
            saleLine.LinkedToLineId = (int)(decimal)dr["ORIGINALOFLINKEDITEMLIST"];
            saleLine.ReturnTransId = (string)dr["RETURNTRANSACTIONID"];
            saleLine.ReturnReceiptId = (string)dr["RETURNRECEIPTID"];
            saleLine.ReturnStoreId = (string)dr["RETURNSTOREID"];
            saleLine.ReturnTerminalId = (string)dr["RETURNTERMINALID"];
            saleLine.ReasonCode.ID = (string)dr["REASONCODEID"];       
            saleLine.IsInfoCodeItem = ((byte)dr["ISINFOCODEITEM"] != 0);

            //Make sure that the POS doesn't ask for the dimensions again.
            saleLine.Dimension.EnterDimensions = false;
            saleLine.ItemDepartmentId = (string)dr["ITEMDEPARTMENTID"];
            saleLine.RetailItemGroupId = (string)dr["ITEMGROUPID"];
            saleLine.PriceInBarcode = ((byte)dr["PRICEINBARCODE"] != 0);
            saleLine.PriceOverridden = ((byte)dr["PRICECHANGE"] != 0);
            saleLine.OriginalPrice = (decimal)dr["ORIGINALPRICE"];
            saleLine.OriginalPriceWithTax = (decimal)dr["ORIGINALPRICEWITHTAX"];
            saleLine.WeightManuallyEntered = ((byte)dr["WEIGHTMANUALLYENTERED"] != 0);
            saleLine.IsAssembly = ((bool)dr["ISASSEMBLY"]);
            saleLine.IsAssemblyComponent = ((bool)dr["ISASSEMBLYCOMPONENT"]);
            saleLine.AssemblyID = dr["ASSEMBLYID"] == DBNull.Value ? Guid.Empty : (Guid)dr["ASSEMBLYID"];
            saleLine.AssemblyComponentID = dr["ASSEMBLYCOMPONENTID"] == DBNull.Value ? Guid.Empty : (Guid)dr["ASSEMBLYCOMPONENTID"];
            saleLine.AssemblyParentLineID = dr["ASSEMBLYPARENTLINEID"] == DBNull.Value ? -1 : (int)dr["ASSEMBLYPARENTLINEID"];
            saleLine.LineWasDiscounted = ((byte)dr["LINEWASDISCOUNTED"] != 0);
            saleLine.ScaleItem = ((byte)dr["SCALEITEM"] != 0);
            saleLine.TareWeight = (int)dr["TAREWEIGHT"];
            saleLine.WeightInBarcode = ((byte)dr["WEIGHTITEM"] != 0);
            saleLine.LimitationMasterID = dr["LIMITATIONMASTERID"] == DBNull.Value ? Guid.Empty : (Guid)dr["LIMITATIONMASTERID"];
            saleLine.PriceUnit = (decimal)dr["PRICEUNIT"];
            saleLine.TotalDiscount = (decimal)dr["TOTALDISCAMOUNT"];
            saleLine.TotalPctDiscount = (decimal)dr["TOTALDISCPCT"];
            saleLine.TotalDiscountWithTax = (decimal)dr["TOTALDISCAMOUNTWITHTAX"];
            saleLine.LineDiscount = (decimal)dr["LINEDSCAMOUNT"];
            saleLine.LinePctDiscount = (decimal)dr["LINEDISCPCT"];
            saleLine.LineDiscountWithTax = (decimal)dr["LINEDISCAMOUNTWITHTAX"];
            saleLine.PeriodicDiscType = (LineEnums.PeriodicDiscountType)dr["PERIODICDISCTYPE"];
            saleLine.PeriodicDiscount = (decimal)dr["PERIODICDISCAMOUNT"];
            saleLine.PeriodicDiscountWithTax = (decimal)dr["PERIODICDISCAMOUNTWITHTAX"];
            saleLine.PeriodicDiscountOfferId = (string)dr["DISCOFFERID"];
            saleLine.PeriodicDiscountOfferName = "";
            saleLine.TaxExempt = dr["TAXEXEMPT"] == DBNull.Value ? false : (byte)dr["TAXEXEMPT"] == 1;
            saleLine.Returnable = (bool) dr["RETURNABLE"];
            saleLine.Description = (string)dr["DESCRIPTION"];
            saleLine.LimitationSplitParentLineId = (int)dr["LIMITATIONSPLITPARENTLINEID"];
            saleLine.LimitationSplitChildLineId = (int)dr["LIMITATIONSPLITCHILDLINEID"];

            transaction.UpdateDiscountAmounts(saleLine);

            return saleLine;
        }

        private static ItemSale PopulateAditionalInfoForSaleLineItem(IConnectionManager entry, IDataReader dr, ISaleLineItem originalItem)
        {
            ItemTypeEnum originalItemType = originalItem.ItemType;

            originalItem.Found = true;
            originalItem.MasterID = (Guid)dr["MASTERID"];
            originalItem.HeaderItemID = dr["HEADERITEMID"] is System.DBNull ? Guid.Empty : (Guid)dr["HEADERITEMID"];
            originalItem.VariantName = (string)dr["VARIANTNAME"];
            originalItem.ItemId = (string)dr["ITEMID"];
            originalItem.ScaleItem = (bool)dr["SCALEITEM"];
            originalItem.TareWeight = (int)dr["TAREWEIGHT"];
            originalItem.DimensionGroupId = (string)dr["DIMGROUPID"];
            originalItem.DescriptionAlias = (string)dr["NAMEALIAS"];
            if (!(dr["TRANSLATEDNAME"] is DBNull))
            {
                originalItem.Description = (string)dr["TRANSLATEDNAME"];
            }
            else
            {
                originalItem.Description = (string)dr["ITEMNAME"];
            }
            originalItem.MarkupAmount = (decimal)dr["SALESMARKUP"];
            originalItem.OriginalPrice = (decimal)dr["PRICE"];
            originalItem.OriginalPriceWithTax = (decimal)dr["LASTKNOWNPRICEWITHTAX"];
            originalItem.Price = originalItem.Price == 0M ? originalItem.OriginalPrice : originalItem.Price;
            originalItem.PriceWithTax = originalItem.PriceWithTax == 0M && !originalItem.PriceOverridden ? originalItem.OriginalPriceWithTax : originalItem.PriceWithTax;
            originalItem.CostPrice = (decimal)dr["PURCHASEPRICE"];
            originalItem.Description = (string)dr["ITEMNAME"];
            originalItem.ItemType = (ItemTypeEnum)((byte)dr["ITEMTYPE"]);
            originalItem.LineDiscountGroup = (string)dr["SALESLINEDISC"];
            originalItem.MultiLineDiscountGroup = (string)dr["SALESMULTILINEDISC"];
            originalItem.IncludedInTotalDiscount = (bool)dr["SALESALLOWTOTALDISCOUNT"];
            originalItem.RetailItemGroupId = (string)dr["RETAILGROUPID"];
            originalItem.QtyBecomesNegative = (bool)dr["QTYBECOMESNEGATIVE"];
            originalItem.ZeroPriceValid = (bool)dr["ZEROPRICEVALID"];
            originalItem.NoDiscountAllowed = (bool)dr["NODISCOUNTALLOWED"];
            originalItem.Blocked = (bool)dr["BLOCKEDONPOS"];
            originalItem.CanBeSold = (bool)dr["CANBESOLD"];
            originalItem.DateToBeBlocked = (DateTime)dr["DATETOBEBLOCKED"];
            originalItem.DateToActivateItem = new Date((DateTime)dr["DATETOACTIVATEITEM"]);
            originalItem.KeyInPrice = (KeyInPrices)((byte)dr["KEYINPRICE"]);
            originalItem.KeyInQuantity = (KeyInQuantities)((byte)dr["KEYINQTY"]);
            originalItem.MustKeyInComment = (bool)dr["MUSTKEYINCOMMENT"];
            originalItem.MustSelectUOM = (bool)dr["MUSTSELECTUOM"];
            originalItem.DiscountUnsuccessfullyApplied = false;
            originalItem.ItemDepartmentId = (string)dr["RETAILDEPARTMENTID"];
            originalItem.ValidationPeriod = (string)dr["VALIDATIONPERIODID"];
            originalItem.Returnable = (bool)dr["RETURNABLE"];
            originalItem.ProductionTime = (int)dr["PRODUCTIONTIME"];

            if (!string.IsNullOrEmpty(originalItem.SalesOrderUnitOfMeasure) || originalItemType == originalItem.ItemType)
            {
                originalItem.OrgUnitOfMeasure = originalItem.SalesOrderUnitOfMeasure;
                originalItem.OrgUnitOfMeasureName = originalItem.SalesOrderUnitOfMeasureName;
            }

            if (string.IsNullOrEmpty(originalItem.SalesOrderUnitOfMeasure))
            {
                originalItem.SalesOrderUnitOfMeasure = (string)dr["UNITID"];
                originalItem.SalesOrderUnitOfMeasureName = (string)dr["UNITNAME"];
            }

            var result = new ItemSale
            {
                Item = originalItem,
                KeyInSerialNumber = (RetailItem.KeyInSerialNumberEnum)((byte)dr["KEYINSERIALNUMBER"]),
                PreSaleInfocodes = !(dr["PRESALEINFOCODE"] is DBNull),
                SaleInfocodes = !(dr["SALEINFOCODE"] is DBNull)
            };

            return result;
        }

        private void AddToSaleLineItems(IConnectionManager entry, IEnumerable<SaleLineItem> items, bool limitStaffListToStore, RetailTransaction transaction)
        {
            foreach (var saleLine in items)
            {
                if (saleLine.ItemClassType != SalesTransaction.ItemClassTypeEnum.DiscountVoucherItem
                && saleLine.ItemClassType != SalesTransaction.ItemClassTypeEnum.CreditMemo
                && saleLine.ItemClassType != SalesTransaction.ItemClassTypeEnum.GiftCertificateItem
                )
                {
                    GetAdditionalItemInfo(entry, saleLine, "");
                    var unit = Providers.UnitData.Get(entry, saleLine.SalesOrderUnitOfMeasure, CacheType.CacheTypeApplicationLifeTime);
                    saleLine.SalesOrderUnitOfMeasureName = (unit != null) ? unit.Text : "";
                    saleLine.UnitQuantityFactor = Providers.UnitConversionData.GetUnitOfMeasureConversionFactor(entry, saleLine.OrgUnitOfMeasure, saleLine.SalesOrderUnitOfMeasure, saleLine.ItemId);
                }

                var user = Providers.POSUserData.GetPerStore(entry, saleLine.SalesPerson.ID, entry.CurrentStoreID, limitStaffListToStore, CacheType.CacheTypeApplicationLifeTime);
                if (user != null)
                {
                    saleLine.SalesPerson.Name = entry.Settings.NameFormatter.Format(user.Name);
                }

                saleLine.DiscountLines = TransactionProviders.DiscountTransactionData.GetDiscountItems(entry, saleLine.Transaction.TransactionId, saleLine.Transaction, saleLine, saleLine.LineId);
                if (saleLine.PeriodicDiscountOfferId != "" && saleLine.PeriodicDiscountOfferName == "")
                {
                    saleLine.PeriodicDiscountOfferName = Providers.DiscountOfferData.GetDiscountName(entry, saleLine.PeriodicDiscountOfferId);
                }

                if (saleLine.DiscountLines.Count(c => c.DiscountType == DiscountTransTypes.LoyaltyDisc) > 0)
                {
                    saleLine.LoyaltyDiscountWithTax = saleLine.DiscountLines.First().AmountWithTax;
                    saleLine.LoyaltyDiscount = saleLine.DiscountLines.First().Amount;
                    saleLine.LoyaltyPctDiscount = saleLine.DiscountLines.First().Percentage;
                    transaction.LoyaltyItem.Relation = LoyaltyPointsRelation.Discount;

                    saleLine.DiscountLines.First().Amount = decimal.Zero;
                    saleLine.DiscountLines.First().AmountWithTax = decimal.Zero;
                }

                if (saleLine.IsLinkedItem)
                {
                    var linkParentItem = items.First(x => x.LineId == saleLine.LinkedToLineId);
                    if (linkParentItem != null)
                    {
                        linkParentItem.IsReferencedByLinkItems = true;
                        saleLine.LinkedItemOrgQuantity = saleLine.Quantity / linkParentItem.Quantity;
                    }
                }

                if (saleLine.IsAssembly)
                {
                    saleLine.ItemAssembly = Providers.RetailItemAssemblyData.Get(entry, saleLine.AssemblyID);

                    if (saleLine.ItemAssembly != null)
                    {
                        saleLine.ItemAssembly.AssemblyComponents = Providers.RetailItemAssemblyComponentData.GetList(entry, saleLine.ItemAssembly.ID);
                    }
                }

                if (saleLine.IsAssemblyComponent)
                {
                    var parentAssemblyLine = items.First(x => x.IsAssembly && x.LineId == saleLine.AssemblyParentLineID);

                    saleLine.ParentAssembly = Providers.RetailItemAssemblyData.Get(entry, parentAssemblyLine.AssemblyID);
                    if (saleLine.ParentAssembly != null)
                    {
                        saleLine.ParentAssembly.AssemblyComponents = Providers.RetailItemAssemblyComponentData.GetList(entry, saleLine.ParentAssembly.ID);

                        // If an assembly item is not configured to calculate price from components,
                        // price must not be shown or calculated for any assembly component in the hierarchy below
                        // (i.e. when assembly items are components of assembly items)
                        saleLine.ParentAssembly.CalculatePriceFromComponents = parentAssemblyLine.ItemAssembly.CalculatePriceFromComponents;
                        if (saleLine.IsAssembly && !saleLine.ParentAssembly.CalculatePriceFromComponents)
                        {
                            saleLine.ItemAssembly.CalculatePriceFromComponents = false;
                        }
                    }
                }

                decimal taxRatePct;
                saleLine.TaxLines = TransactionProviders.TaxTransactionData.GetTaxItems(entry, saleLine.Transaction.TransactionId, saleLine.LineId, saleLine.Transaction, out taxRatePct);
                saleLine.TaxRatePct = taxRatePct;
                var infoCodeID = new RecordIdentifier(saleLine.Transaction.TransactionId, 
                                                new RecordIdentifier(saleLine.LineId, 
                                                new RecordIdentifier((int)InfoCodeLineItem.InfocodeType.Sales,
                                                new RecordIdentifier(saleLine.Transaction.TerminalId, saleLine.Transaction.StoreId))));
                saleLine.InfoCodeLines = TransactionProviders.InfocodeTransactionData.Get(entry, infoCodeID, (PosTransaction)saleLine.Transaction);
            }
        }

        public virtual LinkedList<SaleLineItem> GetLineItemsForRetailTransaction(IConnectionManager entry, RetailTransaction transaction, 
            bool limitStaffListToStore)
        {
            var items = new LinkedList<SaleLineItem>();
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ITEMTYPE, ISNULL(GIFTCARD, 0) AS GIFTCARD, ISNULL(PRESCRIPTIONID, '') AS PRESCRIPTIONID, 
                                    ISNULL(PUMPID, 0) AS PUMPID, ISNULL(SOURCELINENUM, 0) AS SOURCELINENUM, LINENUM, 
                                    ISNULL(CREDITMEMONUMBER, '') AS CREDITMEMONUMBER, ISNULL(PRICE, 0) AS PRICE, 
                                    ISNULL(COMMENT, '') AS COMMENT, ISNULL(DOSAGETYPE, '') AS DOSAGETYPE, ISNULL(DOSAGESTRENGTH, 0) AS DOSAGESTRENGTH, 
                                    ISNULL(DOSAGESTRENGTHUNIT, '') AS DOSAGESTRENGTHUNIT, ISNULL(DOSAGEUNITQUANTITY, 0) AS DOSAGEUNITQUANTITY, 
                                    ISNULL(TRANSACTIONSTATUS, 0) AS TRANSACTIONSTATUS, ISNULL(BARCODE, '') AS BARCODE, ISNULL(ITEMID, '') AS ITEMID, 
                                    ISNULL(TAXGROUP, '') AS TAXGROUP, ISNULL(TAXAMOUNT, 0) AS TAXAMOUNT, ISNULL(NETPRICE, 0) AS NETPRICE,
                                    ISNULL(PRICEID, '') AS PRICEID, ISNULL(PRICETYPE, 0) AS PRICETYPE, 
                                    ISNULL(NETAMOUNT, 0) AS NETAMOUNT, NETAMOUNTINCLTAX, ISNULL(QTY, 0) AS QTY, ISNULL(TRANSDATE, '1900-01-01') AS TRANSDATE,
                                    ISNULL(ITEMIDSCANNED, 0) AS ITEMIDSCANNED, ISNULL(STAFF, '') AS STAFF, ISNULL(UNIT, '') AS UNIT, ISNULL(UNITQTY, 1) AS UNITQTY, 
                                    ISNULL(INVENTSERIALID, '') AS INVENTSERIALID, ISNULL(RFIDTAGID, '') AS RFIDTAGID, TAXINCLINPRICE, 
                                    ISNULL(VARIANTID, '') AS VARIANTID, ISNULL(INVENTBATCHID, '') AS INVENTBATCHID, 
                                    ISNULL(INVENTBATCHEXPDATE, '1900-01-01') AS INVENTBATCHEXPDATE, ISNULL(OILTAX, 0) AS OILTAX, 
                                    ISNULL(RETURNLINEID, 0) AS RETURNLINEID, ISNULL(RETURNQTY, 0) AS RETURNQTY, ISNULL(LINKEDITEMNOTORIGINAL, 0) AS LINKEDITEMNOTORIGINAL, 
                                    ISNULL(RETURNRECEIPTID, '') as RETURNRECEIPTID,
                                    ISNULL(RETURNSTOREID, '') as RETURNSTOREID,
                                    ISNULL(RETURNTERMINALID, '') as RETURNTERMINALID,
                                    ISNULL(REASONCODEID, '') AS REASONCODEID,
                                    ISNULL(ORIGINALOFLINKEDITEMLIST, 0) AS ORIGINALOFLINKEDITEMLIST, ISNULL(RETURNTRANSACTIONID, '') AS RETURNTRANSACTIONID, 
                                    ISNULL(ISINFOCODEITEM, 0) AS ISINFOCODEITEM, ISNULL(ITEMDEPARTMENTID, '') AS ITEMDEPARTMENTID, 
                                    ISNULL(ITEMGROUPID, '') AS ITEMGROUPID, ISNULL(PRICEINBARCODE, 0) AS PRICEINBARCODE, ISNULL(PRICECHANGE, 0) AS PRICECHANGE, 
                                    ISNULL(ORIGINALPRICE, 0) as ORIGINALPRICE,
                                    ISNULL(ORIGINALPRICEWITHTAX, 0) as ORIGINALPRICEWITHTAX,
                                    ISNULL(WEIGHTMANUALLYENTERED, 0) AS WEIGHTMANUALLYENTERED, ISNULL(LINEWASDISCOUNTED, 0) AS LINEWASDISCOUNTED, 
                                    ISNULL(ISASSEMBLY, 0) AS ISASSEMBLY, 
                                    ISNULL(ISASSEMBLYCOMPONENT, 0) AS ISASSEMBLYCOMPONENT, ASSEMBLYID, ASSEMBLYCOMPONENTID, ASSEMBLYPARENTLINEID, 
                                    ISNULL(SCALEITEM, 0) AS SCALEITEM, ISNULL(WEIGHTITEM, 0) AS WEIGHTITEM, LIMITATIONMASTERID, TAREWEIGHT,
                                    ISNULL(PRICEUNIT, 0) AS PRICEUNIT, ISNULL(TOTALDISCAMOUNT, 0) AS TOTALDISCAMOUNT, ISNULL(TOTALDISCPCT, 0) AS TOTALDISCPCT, 
                                    ISNULL(TOTALDISCAMOUNTWITHTAX, 0) AS TOTALDISCAMOUNTWITHTAX, ISNULL(LINEDSCAMOUNT, 0) AS LINEDSCAMOUNT, 
                                    ISNULL(LINEDISCPCT, 0) AS LINEDISCPCT, ISNULL(PERIODICDISCTYPE, 0) AS PERIODICDISCTYPE, ISNULL(DESCRIPTION, '') AS DESCRIPTION,
                                    ISNULL(LINEDISCAMOUNTWITHTAX, 0) AS LINEDISCAMOUNTWITHTAX, ISNULL(PERIODICDISCAMOUNT, 0) AS PERIODICDISCAMOUNT, 
                                    ISNULL(PERIODICDISCAMOUNTWITHTAX, 0) AS PERIODICDISCAMOUNTWITHTAX, ISNULL(DISCOFFERID, '') AS DISCOFFERID, TAXEXEMPT,
                                    RETURNABLE,
                                    LIMITATIONSPLITPARENTLINEID,
                                    LIMITATIONSPLITCHILDLINEID
                                    FROM RBOTRANSACTIONSALESTRANS 
                                    WHERE DATAAREAID = @dataAreaID AND STORE = @storeID AND TERMINALID = @terminalID AND TRANSACTIONID = @transactionID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", transaction.StoreId);
                MakeParam(cmd, "terminalID", transaction.TerminalId);
                MakeParam(cmd, "transactionID", transaction.TransactionId);
                Execute<SaleLineItem, RetailTransaction, object>(entry, items, cmd, CommandType.Text, transaction, null, PopulateSaleLineItem);
            }

            AddToSaleLineItems(entry, items, limitStaffListToStore, transaction);

            #region Income / Expense accounts
            var incomeExpenseItems = TransactionProviders.IncomeExpenseItemData.Get(entry, transaction);
            foreach (IncomeExpenseItem item in incomeExpenseItems)
            {
                item.Quantity = 1;
                item.TaxRatePct = 0;
                item.NoDiscountAllowed = true;
                item.Found = true;
                item.Description = item.AccountName;
                /*
                    Income Expense items cannot have discount lines on them -> no need to check                    
                    Income Expense items cannot have tax lines on them -> no need to check
                    Get all infocode lines attached to this income expense line                     
                */
                var infoCodeID = new RecordIdentifier(transaction.TransactionId, 
                                                new RecordIdentifier(item.LineId,
                                                new RecordIdentifier((int)InfoCodeLineItem.InfocodeType.IncomeExpense,
                                                new RecordIdentifier(transaction.TerminalId, transaction.StoreId))));
                item.InfoCodeLines = TransactionProviders.InfocodeTransactionData.Get(entry, infoCodeID, transaction);
                items.AddLast(item);
            }
            #endregion

            items.AddRange(TransactionProviders.OrderInvoiceTransactionData.GetSalesOrderSalesInvoiceItems(entry, transaction.TransactionId, transaction, transaction.StoreCurrencyCode));
            return items;
        }

        public virtual ItemSale GetAdditionalItemInfo(IConnectionManager entry, ISaleLineItem saleLineItem, string currentCultureName)
        {
            bool includeTranslation = !string.IsNullOrEmpty(currentCultureName);
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.Add(new TableColumn { ColumnName = "MASTERID", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "ITEMID", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "HEADERITEMID", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "ITEMNAME", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "VARIANTNAME", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "NAMEALIAS", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "''", ColumnAlias = "DIMGROUPID" });
                columns.Add(new TableColumn { ColumnName = "ITEMTYPE", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "SALESLINEDISC", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "SALESMULTILINEDISC", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "SALESALLOWTOTALDISCOUNT", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "SALESMARKUP", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "SALESPRICEINCLTAX", TableAlias = "R" , ColumnAlias = "LASTKNOWNPRICEWITHTAX" });
                columns.Add(new TableColumn { ColumnName = "SALESPRICE", TableAlias = "R", ColumnAlias = "PRICE" });
                columns.Add(new TableColumn { ColumnName = "SALESUNITID", TableAlias = "R", ColumnAlias = "UNITID" });
                columns.Add(new TableColumn { ColumnName = "PURCHASEPRICE", TableAlias = "R", ColumnAlias = "PURCHASEPRICE" });
                columns.Add(new TableColumn { ColumnName = "ISNULL(U.TXT, '')", ColumnAlias = "UNITNAME" });
                columns.Add(new TableColumn { ColumnName = "GROUPID",ColumnAlias = "RETAILGROUPID", TableAlias = "G", IsNull = true});
                columns.Add(new TableColumn { ColumnName = "DEPARTMENTID",ColumnAlias = "RETAILDEPARTMENTID", TableAlias = "D", IsNull = true});
                columns.Add(new TableColumn { ColumnName = "QTYBECOMESNEGATIVE", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "ZEROPRICEVALID", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "NODISCOUNTALLOWED", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "BLOCKEDONPOS", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "CANBESOLD", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "DATETOBEBLOCKED", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "DATETOACTIVATEITEM", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "KEYINQTY", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "KEYINPRICE", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "KEYINSERIALNUMBER", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "MUSTKEYINCOMMENT", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "MUSTSELECTUOM", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "SCALEITEM", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "TAREWEIGHT", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "CAST(0 as bit)", ColumnAlias = "SERIALACTIVE" });
                columns.Add(new TableColumn { ColumnName = "VALIDATIONPERIODID", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "RETURNABLE", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "PRODUCTIONTIME", TableAlias = "R" });
                columns.Add(new TableColumn { ColumnName = "TRIGGERING", TableAlias = "ICS", ColumnAlias = "PRESALEINFOCODE" });
                columns.Add(new TableColumn { ColumnName = "TRIGGERING", TableAlias = "ICS2", ColumnAlias = "SALEINFOCODE" });

                List<Condition> conditions = new List<Condition>();

                if (saleLineItem.MasterID != Guid.Empty)
                {
                    conditions.Add( new Condition { Operator = "AND", ConditionValue = "R.MASTERID = @MASTERID" } );
                    MakeParam(cmd, "MASTERID", saleLineItem.MasterID);
                }
                else
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "R.ITEMID = @itemID" } );
                    MakeParam(cmd, "itemID", saleLineItem.ItemId);
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "R.DELETED = 0 " });

                List<Join> joins = new List<Join>();
                joins.Add(new Join
                {
                    Condition = "U.UNITID = R.SALESUNITID",
                    JoinType = "LEFT OUTER",
                    Table = "UNIT",
                    TableAlias = "U"
                });

                joins.Add(new Join
                {
                    Condition = " G.MASTERID = R.RETAILGROUPMASTERID",
                    JoinType = "LEFT OUTER",
                    Table = "RETAILGROUP",
                    TableAlias = "G"
                });

                joins.Add(new Join
                {
                    Condition = " D.MASTERID = G.DEPARTMENTMASTERID ",
                    JoinType = "LEFT OUTER",
                    Table = "RETAILDEPARTMENT",
                    TableAlias = "D"
                });

                joins.Add(new Join
                {
                    Condition = " (ICS.REFRELATION = R.ITEMID AND ICS.REFTABLEID = @preSaleinfoCodeID)",
                    JoinType = "LEFT OUTER",
                    Table = "RBOINFOCODETABLESPECIFIC",
                    TableAlias = "ICS"
                });

                joins.Add(new Join
                {
                    Condition = @"(ICS2.REFRELATION = R.ITEMID AND ICS2.REFTABLEID =  @itemInfoCodeID)  
                                OR (ICS2.REFRELATION= G.GROUPID AND ICS2.REFTABLEID = @groupInfoCodeID)  
                                OR (ICS2.REFRELATION= D.DEPARTMENTID AND ICS2.REFTABLEID = @departmentInfoCodeID)  ",
                    JoinType = "LEFT OUTER",
                    Table = "RBOINFOCODETABLESPECIFIC",
                    TableAlias = "ICS2"
                });

                MakeParam(cmd, "preSaleinfoCodeID", (int)InfoCodeLineItem.TableRefId.PreItem, SqlDbType.Int);
                MakeParam(cmd, "itemInfoCodeID", (int)InfoCodeLineItem.TableRefId.Item, SqlDbType.Int);
                MakeParam(cmd, "groupInfoCodeID", (int)InfoCodeLineItem.TableRefId.ItemGroup, SqlDbType.Int);
                MakeParam(cmd, "departmentInfoCodeID", (int)InfoCodeLineItem.TableRefId.ItemDepartment, SqlDbType.Int);
                if (includeTranslation)
                {
                    columns.Add(new TableColumn
                    {
                        ColumnName = "[DESCRIPTION]",
                        TableAlias = "TRANS",
                        ColumnAlias = "TRANSLATEDNAME"
                    });
                    joins.Add(new Join
                    {
                        Condition = "TRANS.ITEMID = R.ITEMID AND TRANS.CULTURENAME = @cultureName ",
                        JoinType = "LEFT OUTER",
                        Table = "RBOINVENTTRANSLATIONS",
                        TableAlias = "TRANS"
                    });
                    MakeParamNoCheck(cmd, "cultureName", currentCultureName);
                }
                else
                {
                    columns.Add(new TableColumn { ColumnName = "NULL",  ColumnAlias = "TRANSLATEDNAME" });
                }
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "R", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);
                
                List<ItemSale> result = Execute(entry, cmd, CommandType.Text, saleLineItem, PopulateAditionalInfoForSaleLineItem);
                ItemSale item = result.Count > 0 ? result[0] : new ItemSale();

                CostCalculation costCalculation = (CostCalculation)entry.Settings.GetSetting(entry, Settings.CostCalculation, SettingsLevel.System).IntValue;

                if (costCalculation != CostCalculation.Manual)
                {
                    RetailItemCost itemCost = Providers.RetailItemCostData.Get(entry, saleLineItem.ItemId, saleLineItem.Transaction.StoreId);
                    if (itemCost != null)
                    {
                        saleLineItem.CostPrice = itemCost.Cost;
                    }
                }

                return item;
            }
        }

        public virtual bool Exists(IConnectionManager entry, ISaleLineItem item)
        {
            string[] fields = { "TRANSACTIONID", "LINENUM", "STORE", "TERMINALID"};
            var id = new RecordIdentifier(item.Transaction.TransactionId, 
                                  new RecordIdentifier(item.LineId, 
                                  new RecordIdentifier(item.Transaction.StoreId, item.Transaction.TerminalId)));
            return RecordExists(entry, "RBOTRANSACTIONSALESTRANS", fields, id);
        }

        public virtual void Save(IConnectionManager entry, ISaleLineItem saleItem, PosTransaction transaction)
        {
            var statement = new SqlServerStatement("RBOTRANSACTIONSALESTRANS");

            if (Exists(entry, saleItem))
            {
                statement.AddCondition("TRANSACTIONID", saleItem.Transaction.TransactionId);
                statement.AddCondition("LINENUM", saleItem.LineId, SqlDbType.Decimal);
                statement.AddCondition("STORE", saleItem.Transaction.StoreId);
                statement.AddCondition("TERMINALID", saleItem.Transaction.TerminalId);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.AddKey("TRANSACTIONID", saleItem.Transaction.TransactionId);
                statement.AddKey("LINENUM", saleItem.LineId, SqlDbType.Decimal);
                statement.AddKey("STORE", saleItem.Transaction.StoreId);
                statement.AddKey("TERMINALID", saleItem.Transaction.TerminalId);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("RECEIPTID", transaction.ReceiptId);
            if (saleItem is FuelSalesLineItem)
            {
                var fuelItem = (FuelSalesLineItem)saleItem;

                if (fuelItem.FpType == FuellingPointType.FpTransaction)
                {
                    statement.AddField("BARCODE", fuelItem.FuellingPointTransaction.GradeID.ToString());
                }
                else if (fuelItem.FpType == FuellingPointType.ForecourtManager)
                {
                    statement.AddField("BARCODE", fuelItem.FuellingPointTransaction.GradeID.ToString());
                }
                statement.AddField("PumpId", fuelItem.FuelingPointId, SqlDbType.Int);
            }
            else
            {
                statement.AddField("BARCODE", saleItem.BarcodeId);
                statement.AddField("PumpId", 0, SqlDbType.Int);
            }
            statement.AddField("ITEMID", saleItem.ItemId);
            statement.AddField("ITEMDEPARTMENTID", saleItem.ItemDepartmentId);
            statement.AddField("ITEMGROUPID", saleItem.RetailItemGroupId);
            statement.AddField("PRICE", saleItem.PriceWithTax, SqlDbType.Decimal);  // basic price with tax
            statement.AddField("NETPRICE", saleItem.Price, SqlDbType.Decimal);      // basic price
            statement.AddField("PRICEID", saleItem.PriceID);
            statement.AddField("PRICETYPE", (int)saleItem.PriceType, SqlDbType.Int);
            statement.AddField("QTY", (saleItem.Quantity * -1), SqlDbType.Decimal);
            statement.AddField("TAXGROUP", saleItem.TaxGroupId ?? "");
            statement.AddField("TAXAMOUNT", saleItem.TaxAmount * -1, SqlDbType.Decimal);
            statement.AddField("TRANSACTIONSTATUS", saleItem.Voided ? 1 : 0, SqlDbType.Int);
            statement.AddField("DISCAMOUNT", saleItem.LineDiscount + saleItem.TotalDiscount + saleItem.PeriodicDiscount + saleItem.LoyaltyDiscount, SqlDbType.Decimal);
            statement.AddField("WHOLEDISCAMOUNTWITHTAX", saleItem.LineDiscountWithTax + saleItem.TotalDiscountWithTax + saleItem.PeriodicDiscountWithTax + saleItem.LoyaltyDiscountWithTax, SqlDbType.Decimal);
            statement.AddField("LINEDISCAMOUNTWITHTAX", saleItem.LineDiscountWithTax, SqlDbType.Decimal);
            statement.AddField("TOTALDISCAMOUNTWITHTAX", saleItem.TotalDiscountWithTax, SqlDbType.Decimal);
            statement.AddField("PERIODICDISCAMOUNTWITHTAX", saleItem.PeriodicDiscountWithTax, SqlDbType.Decimal);
            statement.AddField("COSTAMOUNT", saleItem.CostPrice, SqlDbType.Decimal);
            statement.AddField("TRANSDATE", saleItem.BeginDateTime, SqlDbType.DateTime);
            statement.AddField("TRANSTIME", (saleItem.BeginDateTime.Hour * 3600) + (saleItem.BeginDateTime.Minute * 60) + saleItem.BeginDateTime.Second, SqlDbType.Int);
            statement.AddField("SHIFT", ((RetailTransaction)saleItem.Transaction).ShiftId);
            if (string.IsNullOrEmpty(((RetailTransaction)saleItem.Transaction).ShiftId))
            {
                statement.AddField("SHIFTDATE", new DateTime(1900,01,01), SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("SHIFTDATE", ((RetailTransaction)saleItem.Transaction).ShiftDate, SqlDbType.DateTime);
            }
            statement.AddField("NETAMOUNT", (saleItem.NetAmount * -1), SqlDbType.Decimal); //Basic price - discounts
            statement.AddField("NETAMOUNTINCLTAX", (saleItem.NetAmountWithTax * -1), SqlDbType.Decimal);
            statement.AddField("DISCOFFERID", saleItem.PeriodicDiscountOfferId);
            statement.AddField("STDNETPRICE", 0, SqlDbType.Decimal);
            statement.AddField("DISCAMOUNTFROMSTDPRICE", 0, SqlDbType.Decimal);
            statement.AddField("STATEMENTID", "0");
            statement.AddField("CUSTACCOUNT", saleItem.Transaction.Customer.ID == RecordIdentifier.Empty ? "" : (string)saleItem.Transaction.Customer.ID);
            statement.AddField("SECTION", "");
            statement.AddField("SHELF", "");
            statement.AddField("STATEMENTCODE", "");
            statement.AddField("TRANSACTIONCODE", 0, SqlDbType.Int);

            bool isItemScanned = saleItem.EntryType == BarCode.BarcodeEntryType.SingleScanned || saleItem.EntryType == BarCode.BarcodeEntryType.MultiScanned;
            statement.AddField("ITEMIDSCANNED", isItemScanned ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("KEYBOARDITEMENTRY", isItemScanned ? 0 : 1, SqlDbType.TinyInt);

            statement.AddField("PRICEINBARCODE",saleItem.PriceInBarcode ? 1 : 0, SqlDbType.TinyInt);

            // Price override information
            statement.AddField("PRICECHANGE", saleItem.PriceOverridden ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ORIGINALPRICE", saleItem.OriginalPrice, SqlDbType.Decimal);
            statement.AddField("ORIGINALPRICEWITHTAX", saleItem.OriginalPriceWithTax, SqlDbType.Decimal);

            statement.AddField("WEIGHTMANUALLYENTERED", saleItem.WeightManuallyEntered ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ISASSEMBLY", saleItem.IsAssembly, SqlDbType.Bit);
            statement.AddField("ISASSEMBLYCOMPONENT", saleItem.IsAssemblyComponent, SqlDbType.Bit);
            statement.AddField("ASSEMBLYID", string.IsNullOrEmpty(saleItem.AssemblyID.StringValue) ? Guid.Empty : Guid.Parse(saleItem.AssemblyID.StringValue), SqlDbType.UniqueIdentifier);
            statement.AddField("ASSEMBLYCOMPONENTID", string.IsNullOrEmpty(saleItem.AssemblyComponentID.StringValue) ? Guid.Empty : Guid.Parse(saleItem.AssemblyComponentID.StringValue), SqlDbType.UniqueIdentifier);
            statement.AddField("ASSEMBLYPARENTLINEID", (int)saleItem.AssemblyParentLineID, SqlDbType.Int);
            statement.AddField("HASPRICE", saleItem.ShouldCalculateAndDisplayAssemblyPrice(), SqlDbType.Bit);
            statement.AddField("LINEWASDISCOUNTED", saleItem.LineWasDiscounted ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TAREWEIGHT", saleItem.TareWeight, SqlDbType.Int);
            statement.AddField("SCALEITEM", saleItem.ScaleItem ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("WEIGHTITEM", saleItem.WeightInBarcode ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("RETURNNOSALE", ((RetailTransaction)saleItem.Transaction).SaleIsReturnSale ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ITEMCORRECTEDLINE", 0, SqlDbType.TinyInt);
            statement.AddField("SALESTYPE", 0, SqlDbType.Int);
            statement.AddField("HOSPITALITYSALESTYPE", (string)saleItem.ActiveHospitalitySalesType);
            statement.AddField("LINKEDITEMNOTORIGINAL", saleItem.IsLinkedItem ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ORIGINALOFLINKEDITEMLIST", saleItem.LinkedToLineId, SqlDbType.Decimal);
            statement.AddField("STAFF", saleItem.SalesPerson.Exists ? (string)saleItem.SalesPerson.ID : "");
            statement.AddField("ITEMPOSTINGGROUP", "");
            statement.AddField("TOTALROUNDEDAMOUNT", 0, SqlDbType.Decimal);
            statement.AddField("COUNTER", saleItem.LineId, SqlDbType.Int);
            statement.AddField("REPLICATED", 0, SqlDbType.TinyInt);
            statement.AddField("CUSTDISCAMOUNT", 0, SqlDbType.Decimal);
            statement.AddField("INFOCODEDISCAMOUNT", 0, SqlDbType.Decimal);
            statement.AddField("CUSTINVOICEDISCAMOUNT", 0, SqlDbType.Decimal);
            statement.AddField("UNIT", saleItem.SalesOrderUnitOfMeasure);
            statement.AddField("UNITQTY", (saleItem.UnitQuantity * -1), SqlDbType.Decimal);
            statement.AddField("UNITPRICE", 0, SqlDbType.Decimal); /*saleItem.PriceUnitWithTax*/
            statement.AddField("TOTALDISCAMOUNT", saleItem.TotalDiscount, SqlDbType.Decimal);
            statement.AddField("TOTALDISCPCT", saleItem.TotalPctDiscount, SqlDbType.Decimal);
            statement.AddField("TOTALDISCINFOCODELINENUM", 0, SqlDbType.Decimal);
            statement.AddField("PERIODICDISCTYPE", (int)saleItem.PeriodicDiscType, SqlDbType.Int);
            statement.AddField("PERIODICDISCAMOUNT", saleItem.PeriodicDiscount, SqlDbType.Decimal);
            statement.AddField("LINEDSCAMOUNT", saleItem.LineDiscount, SqlDbType.Decimal);
            statement.AddField("LINEDISCPCT", saleItem.LinePctDiscount, SqlDbType.Decimal);
            statement.AddField("INVENTSERIALID", saleItem.SerialId);
            statement.AddField("RFIDTAGID", saleItem.RFIDTagId);
            statement.AddField("DISCOUNTAMOUNTFORPRINTING", 0, SqlDbType.Decimal);
            statement.AddField("STAFFID", (string)((RetailTransaction)saleItem.Transaction).Cashier.ID);
            statement.AddField("PERIODICDISCGROUP", saleItem.PeriodicDiscountOfferId);
            statement.AddField("INVENTTRANSID", "");
            statement.AddField("INVENTDIMID", "");
            statement.AddField("MODIFIEDDATE", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("MODIFIEDTIME", (DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60) + DateTime.Now.Second, SqlDbType.Int);
            statement.AddField("MODIFIEDTRANSACTIONID", 0, SqlDbType.Int);
            statement.AddField("CREATEDDATE", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("CREATEDTIME", (DateTime.Now.Hour * 3600) + (DateTime.Now.Minute * 60) + DateTime.Now.Second, SqlDbType.Int);
            statement.AddField("COMMENT", saleItem.Comment ?? "");
            statement.AddField("ITEMTYPE", (int)saleItem.ItemClassType, SqlDbType.Int);
            if (saleItem is DiscountVoucherItem)
            {
                statement.AddField("SOURCELINENUM", ((DiscountVoucherItem)saleItem).SourceLineNum, SqlDbType.Int);
            }
            //Pharmacy details
            if (saleItem is PharmacySalesLineItem)
            {
                statement.AddField("PRESCRIPTIONID", ((PharmacySalesLineItem)saleItem).PrescriptionId);
                statement.AddField("DOSAGETYPE", ((PharmacySalesLineItem)saleItem).DosageType);
                statement.AddField("DOSAGESTRENGTH", ((PharmacySalesLineItem)saleItem).DosageStrength, SqlDbType.Decimal);
                statement.AddField("DOSAGESTRENGTHUNIT", ((PharmacySalesLineItem)saleItem).DosageStrengthUnit);
                statement.AddField("DOSAGEUNITQUANTITY", ((PharmacySalesLineItem)saleItem).DosageUnitQuantiy, SqlDbType.Decimal);
            }

            //Credit memo details
            if (saleItem is CreditMemoItem)
            {
                statement.AddField("CREDITMEMONUMBER", ((CreditMemoItem)saleItem).CreditMemoNumber);
                statement.AddField("PRICE", ((CreditMemoItem)saleItem).Amount, SqlDbType.Decimal);
            }

            //Batch id
            statement.AddField("INVENTBATCHID", saleItem.BatchId);
            statement.AddField("INVENTBATCHEXPDATE", saleItem.BatchExpDate, SqlDbType.DateTime);

            statement.AddField("GIFTCARD", saleItem is GiftCertificateItem ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("RETURNTRANSACTIONID", saleItem.ReturnTransId);
            statement.AddField("RETURNSTOREID", saleItem.ReturnStoreId);
            statement.AddField("RETURNTERMINALID", saleItem.ReturnTerminalId);
            statement.AddField("RETURNRECEIPTID", saleItem.ReturnReceiptId);
            statement.AddField("REASONCODEID", (string)saleItem.ReasonCode.ID);
            statement.AddField("RETURNQTY", saleItem.ReturnedQty, SqlDbType.Decimal);
            statement.AddField("RETURNLINEID", saleItem.ReturnLineId, SqlDbType.Decimal);
            statement.AddField("TAXINCLINPRICE", saleItem.TaxIncludedInItemPrice ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DESCRIPTION", saleItem.Description);
            statement.AddField("OILTAX", saleItem.Oiltax, SqlDbType.Decimal);
            statement.AddField("ISINFOCODEITEM", saleItem.IsInfoCodeItem ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LIMITATIONMASTERID", string.IsNullOrEmpty(saleItem.LimitationMasterID.StringValue) ? Guid.Empty : (Guid)saleItem.LimitationMasterID, SqlDbType.UniqueIdentifier);
            statement.AddField("PRICEUNIT", saleItem.PriceUnit, SqlDbType.Decimal);

            //Fields added for Dynamics AX For Retail customization
            statement.AddField("STORETAXGROUP", ((RetailTransaction)saleItem.Transaction).StoreTaxGroup);
            statement.AddField("TAXITEMGROUP", saleItem.TaxGroupId ?? "");
            statement.AddField("ORIGINALTAXGROUP", ((RetailTransaction)saleItem.Transaction).StoreTaxGroup);
            statement.AddField("ORIGINALTAXITEMGROUP", saleItem.TaxGroupId ?? "");
            statement.AddField("CURRENCY", saleItem.Transaction.StoreCurrencyCode);

            statement.AddField("TAXEXEMPT", saleItem.TaxExempt ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("RETURNABLE", saleItem.Returnable, SqlDbType.Bit);

            statement.AddField("LIMITATIONSPLITPARENTLINEID", saleItem.LimitationSplitParentLineId, SqlDbType.Int);
            statement.AddField("LIMITATIONSPLITCHILDLINEID", saleItem.LimitationSplitChildLineId, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);

            if (saleItem is FuelSalesLineItem)
            {
                TransactionProviders.FuelSalesLineItemData.Save(entry, (FuelSalesLineItem)saleItem, (RetailTransaction)saleItem.Transaction);
            }

            //Go through all the infocode lines on the sales item and save them to the database                            
            foreach (var infocodeLine in saleItem.InfoCodeLines)
            {
                //If the sales item is aggregated in the receipt ctrl then GrossAmountWithTax is 0 when
                //the infocode line is created.
                infocodeLine.Amount = (saleItem.PriceWithTax * -1);

                TransactionProviders.InfocodeTransactionData.Insert(entry, infocodeLine, (PosTransaction)saleItem.Transaction, saleItem.LineId);
            }

            int counter = 1;
            foreach (var discountItem in saleItem.DiscountLines)
            {
                TransactionProviders.DiscountTransactionData.Insert(entry, saleItem, discountItem, counter);

                counter++;
            }

            counter = 1;
            foreach (TaxItem taxItem in saleItem.TaxLines)
            {
                TransactionProviders.TaxTransactionData.Insert(entry, saleItem, taxItem, counter);
                counter++;
            }

            if (saleItem.LoyaltyPoints.PointsAdded)
            {
                saleItem.LoyaltyPoints.CalculatedPoints *= saleItem.LoyaltyPoints.Relation == LoyaltyPointsRelation.Discount ? -1 : 1;
                TransactionProviders.LoyaltyTransactionData.Insert(entry, transaction, saleItem);
            }
        }

        public virtual void MarkItemsAsReturned(IConnectionManager entry, List<ISaleLineItem> returnedItems)
        {
            foreach (SaleLineItem returnedItem in returnedItems.Where(w => w.ReceiptReturnItem))
            {
                using (var cmd = entry.Connection.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE RBOTRANSACTIONSALESTRANS SET RETURNQTY = ((SELECT RETURNQTY FROM RBOTRANSACTIONSALESTRANS WHERE DATAAREAID = @dataAreaID " +
                                      @"AND TRANSACTIONID = @transactionID AND LINENUM = @lineNumber AND STORE = @storeID AND TERMINALID = @terminalID) - @returnQuantity) "+
                                      @" WHERE DATAAREAID = @dataAreaID AND TRANSACTIONID = @transactionID AND LINENUM = @lineNumber AND STORE = @storeID AND TERMINALID = @terminalID";
                    MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                    MakeParam(cmd, "transactionID", returnedItem.ReturnTransId);
                    MakeParam(cmd, "lineNumber", returnedItem.ReturnLineId, SqlDbType.Decimal);
                    MakeParam(cmd, "terminalID", returnedItem.ReturnTerminalId);
                    MakeParam(cmd, "storeID", returnedItem.ReturnStoreId);
                    MakeParam(cmd, "returnQuantity", returnedItem.ReturnedQty*-1, SqlDbType.Decimal);
                    entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
                }
            }
        }
    }
}