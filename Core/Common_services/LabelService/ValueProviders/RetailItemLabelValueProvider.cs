using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.ValueProviders
{
    internal partial class RetailItemLabelValueProvider : LabelValueProviderBase
    {
        private static List<string> macros;

        public override List<string> SupportedMacros
        {
            get
            {
                if (macros == null)
                {
                    var macroList = new List<string>
                        {
                            "${ID}",
                            "${DESCRIPTION}",
                            "${GROUP}",
                            "${DEPARTMENT}",
                            "${SALESUNIT}",
                            "${INVENTORYUNIT}",
                            "${PURCHASEUNIT}",
                            "${BARCODE}",
                            "${PRICE}",
                            "${PRICEWITHTAX}"
                        };

                    AddExtraMacros(macroList);

                    macros = new List<string>();
                    macros.AddRange(SortMacros(macroList));
                }

                return macros;
            }
        }

        public override string ApplyMacros(IConnectionManager entry, int numLabels, string form, IDataEntity entity)
        {
            var item = entity as RetailItem;

            var res = form;
            res = ApplyMacro(res, "${ID}", item.ID.ToString());
            res = ApplyMacro(res, "${DESCRIPTION}", item.Text);
            res = ApplyMacro(res, "${GROUP}", item.RetailGroupName);
            res = ApplyMacro(res, "${DEPARTMENT}", item.RetailDepartmentName);
            res = ApplyMacro(res, "${SALESUNIT}", item.SalesUnitName);
            res = ApplyMacro(res, "${INVENTORYUNIT}", item.InventoryUnitName);
            res = ApplyMacro(res, "${PURCHASEUNIT}", item.PurchaseUnitName);
            //res = ApplyMacro(res, "${QUANTITY}", numQuantity.Value.ToString());

            if (res.IndexOf("${BARCODE}", StringComparison.CurrentCulture) > 0)
            {
                var defaultBarCode = "";
                var defaultBarCodeList = Providers.BarCodeData.GetList(entry, item.ID,
                                                                BarCodeSorting.ItemBarCode, false);
                string firstBarCode = null;
                foreach (BarCode items in defaultBarCodeList)
                {
                    if (string.IsNullOrEmpty(firstBarCode))
                    {
                        firstBarCode = (string)items.ItemBarCode;
                    }
                    if (items.ShowForItem)
                    {
                        defaultBarCode = (string)items.ItemBarCode;
                    }
                }

                if (string.IsNullOrEmpty(defaultBarCode))
                {
                    // Use first barcode, if no default barcode
                    defaultBarCode = firstBarCode;
                }

                res = ApplyMacro(res, "${BARCODE}", defaultBarCode);
            }
            if (res.IndexOf("${PRICEWITHTAX", StringComparison.CurrentCulture) > 0)
            {
                res = ApplyMacro(res, "${PRICEWITHTAX}", item.SalesPriceIncludingTax, "n0");
            }
            if (res.IndexOf("${PRICE", StringComparison.CurrentCulture) > 0)
            {
                res = ApplyMacro(res, "${PRICE}", item.SalesPrice, "n0");
            }

            ApplyExtraMappings(ref res);

            return base.ApplyMacros(entry, numLabels, res, entity);
        }
    }
}
