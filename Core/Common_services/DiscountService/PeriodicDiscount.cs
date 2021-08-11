using System;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.Services.Interfaces;

namespace LSOne.Services
{
    public class PeriodicDiscount : DataEntity
    {
        public string TransactionID { get; set; }
        public string OfferId { get; set; } 
        public string UnitId { get; set; } 
        public string Description { get; set; }
        public PeriodicDiscOfferType PDType { get; set; } 
        public int Priority { get; set; }
        public string DiscValidPeriodId { get; set; }
        public DiscountService.DiscountType DiscountType { get; set; } 
        public int NoOfLinesToTrigger { get; set; }
        public decimal DealPriceValue { get; set; }
        public decimal DiscountPctValue { get; set; }
        public decimal DiscountAmountValue { get; set; }
        public int NoOfLeastExpItems { get; set; }
       // public int NoOfTimesApplicable { get; set; }
        public int LineId { get; set; }
        public int ProductType { get; set; } 
        public string PriceGroup { get; set; }
        public decimal DealPriceOrDiscPct { get; set; }
        public string LineGroup { get; set; }
        public DiscountService.DiscountType DiscType { get; set; } 
        public int NoOfItemsNeeded { get; set; }
        public string ItemId { get; set; }
        public string RetailGroupId { get; set; }
        public string BarcodeId { get; set; }
        public string RetailDepartmentId { get; set; }
        public string SpecialGroup { get; set; }
        public string VariationNumber { get; set; }
        public DiscountOffer.AccountCodeEnum AccountCode { get; set; }
        public RecordIdentifier AccountRelation { get; set; }
        public DiscountOffer.TriggeringEnum Triggering { get; set; }
        public Guid TargetMasterID { get; set; }
        public PeriodicDiscount(string transactionID):this()
        {
            TransactionID = transactionID;
        }


        public PeriodicDiscount()
        { 
            OfferId = "";
            Description = "";
            PDType = PeriodicDiscOfferType.None;
            Priority = 0;
            DiscValidPeriodId = "";
            DiscountType = DiscountService.DiscountType.None;
            //SameDiffMMLines = 0;
            NoOfLinesToTrigger = 0;
            DealPriceValue = decimal.Zero;
            DiscountPctValue = decimal.Zero;
            DiscountAmountValue = decimal.Zero;
            NoOfLeastExpItems = 0;
            //NoOfTimesApplicable = 0;
            LineId = 0;
            ProductType = 0;
            PriceGroup = "";
            DealPriceOrDiscPct = decimal.Zero;
            LineGroup = "";
            DiscType = DiscountService.DiscountType.None;
            NoOfItemsNeeded = 0;
            ItemId = "";
            RetailGroupId = "";
            BarcodeId = "";
            RetailDepartmentId = "";
            SpecialGroup = "";
            AccountCode = DiscountOffer.AccountCodeEnum.None;
            AccountRelation = "";
            VariationNumber = "";
            Triggering = DiscountOffer.TriggeringEnum.Automatic;
        }

        public void AddRow(DataRow dataRow, string itemId)
        {            
            OfferId = (string)dataRow["OfferId"];
            Description = (string)dataRow["Description"];
            PDType = (PeriodicDiscOfferType)(int)dataRow["PDType"];
            Priority = (int)dataRow["Priority"];
            DiscValidPeriodId = (string)dataRow["DiscValidPeriodId"];
            DiscountType = (DiscountService.DiscountType)(int)dataRow["DiscountType"];
            //SameDiffMMLines = (int)dataRow["SameDiffMMLines"];
            NoOfLinesToTrigger = (int)dataRow["NoOfLinesToTrigger"];
            DealPriceValue = (decimal)dataRow["DealPriceValue"];
            DiscountPctValue = (decimal)dataRow["DiscountPctValue"];
            DiscountAmountValue = (decimal)dataRow["DiscountAmountValue"];
            NoOfLeastExpItems = (int)dataRow["NoOfLeastExpItems"];
            //NoOfTimesApplicable = (int)dataRow["NoOfTimesApplicable"];
            LineId = (int)dataRow["LineId"];
            ProductType = (int)dataRow["ProductType"];
            PriceGroup = (string)dataRow["PriceGroup"];
            AccountCode = (DiscountOffer.AccountCodeEnum)((dataRow["ACCOUNTCODE"] == System.DBNull.Value ? 0 : (int)dataRow["ACCOUNTCODE"]));
            AccountRelation = dataRow["ACCOUNTRELATION"] == System.DBNull.Value ? "" : (string)dataRow["ACCOUNTRELATION"];
            Triggering = (DiscountOffer.TriggeringEnum)((dataRow["TRIGGERED"] == System.DBNull.Value ? 0 : (int)dataRow["TRIGGERED"]));

            string relation = dataRow["Id"] == System.DBNull.Value ? "" : (string)dataRow["Id"];

            switch ((DiscountOfferLine.DiscountOfferTypeEnum)ProductType)
            {
                case DiscountOfferLine.DiscountOfferTypeEnum.Item:
                    ItemId = relation;
                    break;
                case DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup:
                    RetailGroupId = relation;
                    break;
                case DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment:
                    RetailDepartmentId = relation;
                    break;                
                case DiscountOfferLine.DiscountOfferTypeEnum.BarCodeBasedVariant:
                    BarcodeId = relation;
                    break;
                case DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup:
                    SpecialGroup = relation;
                    ItemId = itemId;
                    break;
                case DiscountOfferLine.DiscountOfferTypeEnum.Variant:
                    VariationNumber = relation;
                    ItemId = itemId;
                    break;
            }

            DealPriceOrDiscPct = (decimal)dataRow["DealPriceOrDiscPct"];
            LineGroup = (string)dataRow["LineGroup"];
            DiscType = (DiscountService.DiscountType)(int)dataRow["DiscType"];
            try
            {
                object value = dataRow["NoOfItemsNeeded"];
                NoOfItemsNeeded = (value == DBNull.Value) ? 0 : (int)value;
            }
            catch
            {
                NoOfItemsNeeded = 0;
            }
        }

        public string GetID()
        {
            switch ((DiscountOfferLine.DiscountOfferTypeEnum)ProductType)
            {
                case DiscountOfferLine.DiscountOfferTypeEnum.Item:
                    return ItemId;
                    
                case DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup:
                    return RetailGroupId;

                case DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment:
                    return RetailDepartmentId;

                case DiscountOfferLine.DiscountOfferTypeEnum.All:
                    return "";

                case DiscountOfferLine.DiscountOfferTypeEnum.BarCodeBasedVariant:
                    return BarcodeId;

                case DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup:
                    return SpecialGroup;

                case DiscountOfferLine.DiscountOfferTypeEnum.Variant:
                    return VariationNumber;

                default:
                    return "";
            }
        }

        public void AddItem(string itemID)
        {
            ItemId = itemID;
        }

        public void AddGroup(string groupdID)
        {
            RetailGroupId = groupdID;
        }

        public void AddDepartment(string departmentID)
        {
            RetailDepartmentId = departmentID;
        }

        public void AddBarcode(string barcode)
        {
            BarcodeId = barcode;
        }

        

        // NOTES TO MARY IN NRF

        // The old file had a Status flag in the function CreatePeriodDiscountTable() that was not present in the Add() function

        // This was part of the old LSRetail.POS.BusinessObjects.Transactions.MemoryTables.PeriodicDiscount.cs file. Not sure how to port this
        //if ((int)dataRow["ProductType"] == 5 || (int)dataRow["ProductType"] == 3) //DiscountOfferProductTypes.SpecialGroup
        //{
        //    row["Id"] = itemId;
        //}
        //else
        //{                    
        //    row["Id"] = dataRow["Id"];
        //}                

    }
}
