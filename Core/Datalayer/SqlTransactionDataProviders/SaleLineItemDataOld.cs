using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlDataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts;
using LSOne.DataLayer.SqlDataProviders.Units;
using LSOne.DataLayer.SqlDataProviders.UserManagement;
using LSOne.DataLayer.SqlTransactionDataProviders.Properties;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.CreditMemoItem;
using LSOne.DataLayer.TransactionObjects.Line.GiftCertificateItem;
using LSOne.DataLayer.TransactionObjects.Line.IncomeExpenseItem;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class SaleLineItemData : SqlServerDataProviderBase, ISaleLineItemData
    {
        //order by linenum
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
            saleLine.DiscountsWereRemoved = false; //Is never given value in code - is always false                    
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
            saleLine.LineWasDiscounted = ((byte)dr["LINEWASDISCOUNTED"] != 0);
            saleLine.ScaleItem = ((byte)dr["SCALEITEM"] != 0);
            saleLine.WeightInBarcode = ((byte)dr["WEIGHTITEM"] != 0);
            saleLine.TenderRestrictionId = (string)dr["TENDERRESTRICTIONID"];
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

            transaction.UpdateDiscountAmounts(saleLine);

            return saleLine;
        }

        private static ItemSale PopulateAditionalInfoForSaleLineItem(IConnectionManager entry, IDataReader dr, ISaleLineItem orginalItem)
        {
            orginalItem.Found = true;
            orginalItem.ItemId = (string)dr["ITEMID"];
            orginalItem.ScaleItem = ((byte) dr["SCALEITEM"] != 0);
            orginalItem.DimensionGroupId = (string)dr["DIMGROUPID"];
            orginalItem.DescriptionAlias = (string)dr["NAMEALIAS"];
            if (!(dr["TRANSLATEDNAME"] is DBNull))
            {
                orginalItem.Description = (string)dr["TRANSLATEDNAME"];
            }
            else
            {
                orginalItem.Description = (string)dr["ITEMNAME"];
            }
            orginalItem.MarkupAmount = (decimal)dr["MARKUP"];
            orginalItem.OriginalPrice = (decimal)dr["PRICE"];
            orginalItem.OriginalPriceWithTax = (decimal)dr["LASTKNOWNPRICEWITHTAX"];
            orginalItem.Price = orginalItem.Price == 0M ? orginalItem.OriginalPrice : orginalItem.Price;
            orginalItem.PriceWithTax = orginalItem.PriceWithTax == 0M ? orginalItem.OriginalPriceWithTax : orginalItem.PriceWithTax;
            orginalItem.Description = (string)dr["ITEMNAME"];
            orginalItem.ItemType = (ItemTypeEnum)dr["ITEMTYPE"];
            orginalItem.LineDiscountGroup = (string)dr["LINEDISC"];
            orginalItem.MultiLineDiscountGroup = (string)dr["MULTILINEDISC"];
            orginalItem.IncludedInTotalDiscount = ((byte)dr["ENDDISC"] != 0);
            orginalItem.RetailItemGroupId = (string)dr["ITEMGROUP"];
            orginalItem.QtyBecomesNegative = ((byte)dr["QTYBECOMESNEGATIVE"] != 0);
            orginalItem.ZeroPriceValid = ((byte)dr["ZEROPRICEVALID"] != 0);
            orginalItem.NoDiscountAllowed = ((byte)dr["NODISCOUNTALLOWED"] != 0);
            orginalItem.Blocked = ((byte)dr["BLOCKEDONPOS"] != 0);
            orginalItem.DateToBeBlocked = (DateTime)dr["DATETOBEBLOCKED"];
            orginalItem.DateToActivateItem = new Date((DateTime)dr["DATETOACTIVATEITEM"]);
            orginalItem.KeyInPrice = (KeyInPrices)dr["KeyingInPrice"];
            orginalItem.KeyInQuantity = (KeyInQuantities)dr["KeyingInQty"];
            orginalItem.MustKeyInComment = ((byte)dr["MustKeyInComment"] != 0);
            orginalItem.MustSelectUOM = ((byte)dr["MustSelectUOM"] != 0);
            orginalItem.DiscountUnsuccessfullyApplied = false;
            //orginalItem.Dimension.DimensionID = (string)dr["DIMITEMID"];
            //orginalItem.Dimension.EnterDimensions = string.IsNullOrEmpty((string)orginalItem.Dimension.VariantNumber) ? orginalItem.Dimension.DimensionID != "" : orginalItem.Dimension.EnterDimensions;
            orginalItem.ItemDepartmentId = (string)dr["ITEMDEPARTMENT"];
            orginalItem.ValidationPeriod = (string)dr["POSPERIODICID"];
            if (string.IsNullOrEmpty(orginalItem.SalesOrderUnitOfMeasure))
            {
                orginalItem.SalesOrderUnitOfMeasure = (string)dr["UNITID"];
                orginalItem.SalesOrderUnitOfMeasureName = (string)dr["UNITNAME"];
            }
            else
            {
                orginalItem.OrgUnitOfMeasure = (string)dr["UNITID"];
                orginalItem.OrgUnitOfMeasureName = (string) dr["UNITNAME"];
            }
            
            var result = new ItemSale
            {
                Item = orginalItem,
                SerialActive = ((byte) dr["SERIALACTIVE"] != 0),
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
                    if (saleLine.Dimension.DimensionID != "")
                    {
                        saleLine.Dimension = Providers.DimensionData.GetByVariantID(entry, saleLine.Dimension.VariantNumber);
                    }
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
                                    ISNULL(NETAMOUNT, 0) AS NETAMOUNT, NETAMOUNTINCLTAX, ISNULL(QTY, 0) AS QTY, ISNULL(TRANSDATE, '1900-01-01') AS TRANSDATE,
                                    ISNULL(ITEMIDSCANNED, 0) AS ITEMIDSCANNED, ISNULL(STAFF, '') AS STAFF, ISNULL(UNIT, '') AS UNIT, ISNULL(UNITQTY, 1) AS UNITQTY, 
                                    ISNULL(INVENTSERIALID, '') AS INVENTSERIALID, ISNULL(RFIDTAGID, '') AS RFIDTAGID, TAXINCLINPRICE, 
                                    ISNULL(VARIANTID, '') AS VARIANTID, ISNULL(INVENTBATCHID, '') AS INVENTBATCHID, 
                                    ISNULL(INVENTBATCHEXPDATE, '1900-01-01') AS INVENTBATCHEXPDATE, ISNULL(OILTAX, 0) AS OILTAX, 
                                    ISNULL(RETURNLINEID, 0) AS RETURNLINEID, ISNULL(RETURNQTY, 0) AS RETURNQTY, ISNULL(LINKEDITEMNOTORIGINAL, 0) AS LINKEDITEMNOTORIGINAL, 
                                    ISNULL(RETURNRECEIPTID, '') as RETURNRECEIPTID,
                                    ISNULL(RETURNSTOREID, '') as RETURNSTOREID,
                                    ISNULL(RETURNTERMINALID, '') as RETURNTERMINALID,
                                    ISNULL(ORIGINALOFLINKEDITEMLIST, 0) AS ORIGINALOFLINKEDITEMLIST, ISNULL(RETURNTRANSACTIONID, '') AS RETURNTRANSACTIONID, 
                                    ISNULL(ISINFOCODEITEM, 0) AS ISINFOCODEITEM, ISNULL(ITEMDEPARTMENTID, '') AS ITEMDEPARTMENTID, 
                                    ISNULL(ITEMGROUPID, '') AS ITEMGROUPID, ISNULL(PRICEINBARCODE, 0) AS PRICEINBARCODE, ISNULL(PRICECHANGE, 0) AS PRICECHANGE, 
                                    ISNULL(ORIGINALPRICE, 0) as ORIGINALPRICE,
                                    ISNULL(ORIGINALPRICEWITHTAX, 0) as ORIGINALPRICEWITHTAX,
                                    ISNULL(WEIGHTMANUALLYENTERED, 0) AS WEIGHTMANUALLYENTERED, ISNULL(LINEWASDISCOUNTED, 0) AS LINEWASDISCOUNTED, 
                                    ISNULL(SCALEITEM, 0) AS SCALEITEM, ISNULL(WEIGHTITEM, 0) AS WEIGHTITEM, ISNULL(TENDERRESTRICTIONID, '') AS TENDERRESTRICTIONID, 
                                    ISNULL(PRICEUNIT, 0) AS PRICEUNIT, ISNULL(TOTALDISCAMOUNT, 0) AS TOTALDISCAMOUNT, ISNULL(TOTALDISCPCT, 0) AS TOTALDISCPCT, 
                                    ISNULL(TOTALDISCAMOUNTWITHTAX, 0) AS TOTALDISCAMOUNTWITHTAX, ISNULL(LINEDSCAMOUNT, 0) AS LINEDSCAMOUNT, 
                                    ISNULL(LINEDISCPCT, 0) AS LINEDISCPCT, ISNULL(PERIODICDISCTYPE, 0) AS PERIODICDISCTYPE, ISNULL(DESCRIPTION, '') AS DESCRIPTION,
                                    ISNULL(LINEDISCAMOUNTWITHTAX, 0) AS LINEDISCAMOUNTWITHTAX, ISNULL(PERIODICDISCAMOUNT, 0) AS PERIODICDISCAMOUNT, 
                                    ISNULL(PERIODICDISCAMOUNTWITHTAX, 0) AS PERIODICDISCAMOUNTWITHTAX, ISNULL(DISCOFFERID, '') AS DISCOFFERID, TAXEXEMPT 
                                    FROM RBOTRANSACTIONSALESTRANS 
                                    WHERE DATAAREAID = @dataAreaID AND STORE = @storeID AND TERMINALID = @terminalID AND TRANSACTIONID = @transactionID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", transaction.StoreId);
                MakeParam(cmd, "terminalID", transaction.TerminalId);
                MakeParam(cmd, "transactionID", transaction.TransactionId);
                Execute<SaleLineItem, RetailTransaction, object>(entry, items, cmd, CommandType.Text, transaction, null, PopulateSaleLineItem);
            }

            UpdateReturnedQuantity(entry, items, transaction);
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

        private static void UpdateReturnedQuantity(IConnectionManager entry, LinkedList<SaleLineItem> items, RetailTransaction transaction)
        {
            //if (items.Count(c => c.ReturnedQty > 0) == 0)
            //{
            //    return;
            //}

            //List<SaleLineItem> returnedItems = items.Select(sli => (SaleLineItem) sli.Clone()).ToList();

            //foreach (var returnedItem in returnedItems.Where(w => w.ReturnLineId != 0))
            //{
            //    var item = items.FirstOrDefault(i => i.LineId == returnedItem.ReturnLineId && i.ItemId == returnedItem.ItemId);
            //    if (item != null) item.ReturnedQty += returnedItem.ReturnedQty;
            //}
        }

        public virtual ItemSale GetAdditionalItemInfo(IConnectionManager entry, ISaleLineItem saleLineItem, string currentCultureName)
        {
            bool includeTranslation = !string.IsNullOrEmpty(currentCultureName);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT DISTINCT 
                      i.ITEMID, 
                      ISNULL(DIM.ITEMID, '') AS DIMITEMID, 
                      ISNULL(I.ITEMNAME, '') AS ITEMNAME, 
                      I.NAMEALIAS, 
                      I.DIMGROUPID, 
                      I.ITEMTYPE, 
                      M.LINEDISC, 
                      M.MULTILINEDISC, 
                      M.ENDDISC, 
                      ISNULL(M.MARKUP, 0) AS MARKUP, 
                      ISNULL(M.PRICEINCLTAX, 0) AS LASTKNOWNPRICEWITHTAX, 
                      ISNULL(M.PRICE, 0) AS PRICE, 
                      ISNULL(M.UNITID, '') AS UNITID, 
                      ISNULL(U.TXT, '') AS UNITNAME, 
                      ISNULL(T.ITEMGROUP, '') As ITEMGROUP, 
                      ISNULL(T.ITEMDEPARTMENT, '') As ITEMDEPARTMENT, 
                      QTYBECOMESNEGATIVE, 
                      ZEROPRICEVALID, 
                      NODISCOUNTALLOWED, 
                      BLOCKEDONPOS, 
                      DATETOBEBLOCKED, 
                      ISNULL(DATETOACTIVATEITEM, 0) AS DATETOACTIVATEITEM, 
                      KEYINGINQTY, 
                      KEYINGINPRICE, 
                      ISNULL(MUSTKEYINCOMMENT, 0) AS MUSTKEYINCOMMENT, 
                      ISNULL(MUSTSELECTUOM, 0) AS MUSTSELECTUOM, 
                      ISNULL(T.SCALEITEM, 0) AS SCALEITEM, 
                      ISNULL(DIMGROUP.ACTIVE, 0) AS SERIALACTIVE, 
                      ISNULL(T.POSPERIODICID, '') As POSPERIODICID, ";
                cmd.CommandText += includeTranslation ? @"TRANS.[DESCRIPTION] AS TRANSLATEDNAME, " : @"NULL AS TRANSLATEDNAME, ";
                cmd.CommandText += @"ICS.TRIGGERING AS PRESALEINFOCODE, 
                                   ICS2.TRIGGERING AS SALEINFOCODE 
                                   FROM INVENTTABLE I 
                                   INNER JOIN INVENTTABLEMODULE M ON M.ITEMID = I.ITEMID AND M.MODULETYPE = " + (int)RetailItem.UnitTypeEnum.Sales + " AND M.DATAAREAID = I.DATAAREAID " +
                                   @"INNER JOIN RBOINVENTTABLE T ON T.ITEMID = I.ITEMID AND T.DATAAREAID = I.DATAAREAID 
                                   LEFT OUTER JOIN INVENTDIMCOMBINATION DIM ON DIM.ITEMID = I.ITEMID AND DIM.DATAAREAID = I.DATAAREAID 
                                   LEFT OUTER JOIN UNIT U ON U.UNITID = M.UNITID AND U.DATAAREAID = I.DATAAREAID 
                                   LEFT OUTER JOIN INVENTDIMSETUP DIMGROUP ON DIMGROUP.DIMGROUPID = I.DIMGROUPID AND DIMGROUP.DIMFIELDID = 5 AND DIMGROUP.DATAAREAID = I.DATAAREAID ";
                cmd.CommandText += includeTranslation ? @"LEFT OUTER JOIN RBOINVENTTRANSLATIONS TRANS ON TRANS.ITEMID = I.ITEMID AND TRANS.CULTURENAME = @cultureName AND TRANS.DATAAREAID = I.DATAAREAID " : "";
                cmd.CommandText += @"LEFT OUTER JOIN RBOINFOCODETABLESPECIFIC ICS ON ICS.REFRELATION = I.ITEMID AND ICS.REFTABLEID = " + (int)InfoCodeLineItem.TableRefId.PreItem + " AND ICS.DATAAREAID = I.DATAAREAID " +
                                   @"LEFT OUTER JOIN RBOINFOCODETABLESPECIFIC ICS2 ON ICS2.REFRELATION = I.ITEMID AND ICS2.REFTABLEID = " + (int)InfoCodeLineItem.TableRefId.Item + " AND ICS2.DATAAREAID = I.DATAAREAID " +
                                   @"WHERE i.ITEMID = @itemID 
                                   AND i.DATAAREAID = @dataAreaID";

                //DimensionGroupData.DimensionEnum.Serial
                MakeParam(cmd, "itemID", saleLineItem.ItemId);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                if (includeTranslation)
                {
                    MakeParam(cmd, "cultureName", currentCultureName);
                }
                List<ItemSale> result = Execute<ItemSale, ISaleLineItem>(entry, cmd, CommandType.Text, saleLineItem, PopulateAditionalInfoForSaleLineItem);
                return result.Count > 0 ? result[0] : null;
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
            statement.AddField("PRICE", saleItem.PriceWithTax, SqlDbType.Decimal);  //Basic price with tax
            statement.AddField("NETPRICE", saleItem.Price, SqlDbType.Decimal);      //Basic price
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
            if ((saleItem.EntryType == BarCode.BarcodeEntryType.SingleScanned) || (saleItem.EntryType == BarCode.BarcodeEntryType.MultiScanned))
            {
                statement.AddField("ITEMIDSCANNED", 1, SqlDbType.TinyInt);
                statement.AddField("KEYBOARDITEMENTRY", 0, SqlDbType.TinyInt);
            }
            else
            {
                statement.AddField("ITEMIDSCANNED", 0, SqlDbType.TinyInt);
                statement.AddField("KEYBOARDITEMENTRY", 1, SqlDbType.TinyInt);
            }
            statement.AddField("PRICEINBARCODE",saleItem.PriceInBarcode ? 1 : 0, SqlDbType.TinyInt);

            // Price override information
            statement.AddField("PRICECHANGE", saleItem.PriceOverridden ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ORIGINALPRICE", saleItem.OriginalPrice, SqlDbType.Decimal);
            statement.AddField("ORIGINALPRICEWITHTAX", saleItem.OriginalPriceWithTax, SqlDbType.Decimal);

            statement.AddField("WEIGHTMANUALLYENTERED", saleItem.WeightManuallyEntered ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LINEWASDISCOUNTED", saleItem.LineWasDiscounted ? 1 : 0, SqlDbType.TinyInt);
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
            //statement.AddField("VARIANTID", (string)saleItem.Dimension.VariantNumber);
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
            statement.AddField("RETURNQTY", saleItem.ReturnedQty, SqlDbType.Decimal);
            statement.AddField("RETURNLINEID", saleItem.ReturnLineId, SqlDbType.Decimal);
            statement.AddField("TAXINCLINPRICE", saleItem.TaxIncludedInItemPrice ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DESCRIPTION", saleItem.Description);
            statement.AddField("OILTAX", saleItem.Oiltax, SqlDbType.Decimal);
            statement.AddField("ISINFOCODEITEM", saleItem.IsInfoCodeItem ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TENDERRESTRICTIONID", saleItem.TenderRestrictionId);
            statement.AddField("PRICEUNIT", saleItem.PriceUnit, SqlDbType.Decimal);

            //Fields added for Dynamics AX For Retail customization
            statement.AddField("STORETAXGROUP", ((RetailTransaction)saleItem.Transaction).StoreTaxGroup);
            statement.AddField("TAXITEMGROUP", saleItem.TaxGroupId ?? "");
            statement.AddField("ORIGINALTAXGROUP", ((RetailTransaction)saleItem.Transaction).StoreTaxGroup);
            statement.AddField("ORIGINALTAXITEMGROUP", saleItem.TaxGroupId ?? "");
            statement.AddField("CURRENCY", saleItem.Transaction.StoreCurrencyCode);

            statement.AddField("TAXEXEMPT", saleItem.TaxExempt ? 1 : 0, SqlDbType.TinyInt);
            

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
