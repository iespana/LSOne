using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.RMSMigration.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSItem : RetailItem
    {
        public int RMS_ID { get; set; }
        public string ItemNumber { get; set; }
        public int? RMS_CustomerID { get; set; }

        public int RMS_RetailGroupID { get; set; }

        public int RMS_DepartmentID { get; set; }

        public int? RMS_SalesTaxItemGroupID { get; set; }

        public int RMS_MasterItemID { get; set; }

        public string VariantCode { get; set; }

        public string Barcode { get; set; }

        public string InventoryUnitOfMeasure { get; set; }
        public string SalesUnitOfMeasure { get; set; }


        private bool _ItemCannotBeSold = false;
        public bool ItemCannotBeSold
        {
            get
            {
                return _ItemCannotBeSold;
            }

            set
            {
                _ItemCannotBeSold = value;
            }
        }
        public int BlockSalesType { get; set; }

        public DateTime? BlockSalesAfterDate { get; set; }
        public DateTime? BlockSalesBeforeDate { get; set; }

        public int ReorderPoint { get; set; }
        public int RestockLevel { get; set; }

        public int TagAlongItem { get; set; }
        public decimal TagAlongQuantity { get; set; }
        public string Dimension1 { get; set; }
        public string Dimension2 { get; set; }
        public string Dimension3 { get; set; }

        public string DimensionAttribute1 { get; set; }
        public string DimensionAttribute2 { get; set; }
        public string DimensionAttribute3 { get; set; }

        public int TaxSystem { get; set; }

        public string PictureName { get; set; }

        public void CalculatePrice()
        {
            DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            RecordIdentifier itemSalesTaxGroupID = SalesTaxItemGroupID;

            RecordIdentifier defaultStoreTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel);

            if (TaxSystem == 2)
            {
                SalesPriceIncludingTax = SalesPrice;
                SalesPrice = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceFromPriceWithTax(
                    PluginEntry.DataModel,
                    SalesPriceIncludingTax,
                    itemSalesTaxGroupID,
                    defaultStoreTaxGroup
                   );
            }
            else
            {
                SalesPriceIncludingTax = Services.Interfaces.Services.TaxService(PluginEntry.DataModel).CalculatePriceWithTax(
                    PluginEntry.DataModel,
                    SalesPrice,
                    itemSalesTaxGroupID,
                    defaultStoreTaxGroup,
                    false,
                    0.0m,
                    priceLimiter);
            }
        }

        public void AddBarcode(RecordIdentifier barCodeSetupID)
        {
            if (string.IsNullOrEmpty(Barcode))
            {
                return;
            }

            BarCode barCode = new BarCode();
            barCode.ItemID = ID;
            barCode.ItemBarCode = Barcode;
            barCode.UnitID = "";

            barCode.BarCodeSetupID = barCodeSetupID;
            barCode.ShowForItem = true;
            barCode.UseForInput = false;
            barCode.UseForPrinting = false;
            barCode.Quantity = 0;

            Providers.BarCodeData.Save(PluginEntry.DataModel, barCode);
        }

        public void SetUnitOfMeasure(ILookupManager lookupManager)
        {
            if (lookupManager.UnitOfMeasure.ContainsKey(InventoryUnitOfMeasure))
            {
                InventoryUnitID = lookupManager.UnitOfMeasure[InventoryUnitOfMeasure];
            }
            else if (!string.IsNullOrWhiteSpace(InventoryUnitOfMeasure))
            {
                CreateUnitOfMeasure(InventoryUnitOfMeasure, lookupManager);
                InventoryUnitID = lookupManager.UnitOfMeasure[InventoryUnitOfMeasure];
            }
            else
            {
                InventoryUnitID = lookupManager.DefaultUnitOfMeasureID;
            }

            if (lookupManager.UnitOfMeasure.ContainsKey(SalesUnitOfMeasure))
            {
                SalesUnitID = lookupManager.UnitOfMeasure[SalesUnitOfMeasure];
            }
            else if (!string.IsNullOrWhiteSpace(SalesUnitOfMeasure))
            {
                CreateUnitOfMeasure(SalesUnitOfMeasure, lookupManager);
                SalesUnitID = lookupManager.UnitOfMeasure[SalesUnitOfMeasure];
            }
            else
            {
                SalesUnitID = lookupManager.DefaultUnitOfMeasureID;
            }
        }

        public void CreateUnitOfMeasure(string name, ILookupManager lookupManager)
        {
            Unit each = new Unit() { Text = name };
            Providers.UnitData.Save(PluginEntry.DataModel, each);
            if (!lookupManager.UnitOfMeasure.ContainsKey(name))
            {
                lookupManager.UnitOfMeasure.Add(name, each.ID);
            }
        }
    }
}
