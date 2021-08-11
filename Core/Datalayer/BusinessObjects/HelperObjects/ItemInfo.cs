using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.HelperObjects
{
    #region ReceiptItemType

    /// <summary>
    /// The Receipt items available in the ReceiptControl - the numeric value of each enum corresponds to the translation id for each column
    /// </summary>
    public enum ReceiptItemType
    {
        BarcodeId = 72,
        ItemId = 73,
        ItemName = 50,
        ItemNameAlias = 120,
        Amount = 104,
        AmountWithoutTax = 105,
        Quantity = 51,
        TotalAmount = 54,
        TotalAmountWithoutTax = 55,
        Comment = 0,
        TaxCode = 74,
        TaxAmount = 52,
        TaxRatePct = 53,
        Voided = 56,
        LineId = 75,
        OriginalPriceWithTax = 76,
        StandardRetailPriceWithTax = 77,
        ColorName = 78,
        SizeName = 79,
        StyleName = 80,
        OfferId = 81,
        LinkedItem = 82,
        InfoCodeItem = 3604,
        //CorporateCard = 106,
        ItemGroup = 107,
        BatchId = 108,
        BatchExpDate = 109,
        PaymentIndex = 88,
        PaymentName = 89,
        //ShouldBeManuallyRemoved = 29, 
        SerialId = 110,
        Found = 111,
        Blocked = 112,
        Activated = 113,
        RFID = 114,
        UOM = 102,
        Selected = 115,
        ItemType = 116,
        DiscountVoucherVoided = 117,
        MenuType = 118,
        SentToStationPrinter = 119,
        ExtraInfo1 = 122,
        ExtraInfo2 = 123,
        ExtraInfo3 = 124,
        ExtraInfo4 = 125,
        ExtraInfo5 = 126,
        ExtraInfo6 = 127,
        ExtraInfo7 = 128,
        ExtraInfo8 = 129,
        ExtraInfo9 = 130,
        ExtraInfo10 = 131,
        ExtraInfo11 = 132,
        ExtraInfo12 = 133,
        ExtraInfo13 = 134,
        ExtraInfo14 = 135,
        ExtraInfo15 = 136,
        ExtraInfo16 = 137,
        ExtraInfo17 = 138,
        ExtraInfo18 = 139,
        ExtraInfo19 = 140,
        ExtraInfo20 = 141
    }

    #endregion

    public class ReceiptItemInfo
    {
        public List<ColumnInfo> Columns { get; set; }


        public ReceiptItemInfo()
        {
            Columns = new List<ColumnInfo>();
        }

        public void ClearColumnInfo()
        {
            foreach (ColumnInfo col in Columns.Where(w => w.ColumnType == ColumnVariableType.Boolean))
            {
                string columnName = ColumnName(col.ReceiptItem);
                if (columnName != "")
                {
                    col.UpdateColumnInfo(false);
                }

            }

            foreach (ColumnInfo col in Columns.Where(w => w.ColumnType != ColumnVariableType.Boolean))
            {
                string columnName = ColumnName(col.ReceiptItem);
                if (columnName != "")
                {
                    col.UpdateColumnInfo(Conversion.ToStr(""));
                }
            }

            ResetUpdated();
            //ResetValueSet();
        }

        public void ResetUpdated()
        {
            foreach (ColumnInfo col in Columns.Where(w => w.Updated == true))
            {
                col.ResetUpdated();
            }
        }

        //public void ResetValueSet()
        //{
        //    foreach (ColumnInfo col in Columns.Where(w => w.ValueSet == true))
        //    {
        //        col.ValueSet = false;
        //    }
        //}

        public void CreateItemColumns()
        {
            Columns.Clear();

            ColumnInfo colInfo = new ColumnInfo(ReceiptItemType.BarcodeId, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.ItemId, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.ItemName, "", ColumnVariableType.String, true);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.ItemNameAlias, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.Amount, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.AmountWithoutTax, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.Quantity, "", ColumnVariableType.Decimal, true);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.TotalAmount, "", ColumnVariableType.String, true);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.TotalAmountWithoutTax, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.Comment, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.TaxCode, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.TaxAmount, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.TaxRatePct, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.Voided, "", ColumnVariableType.Boolean, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.LineId, "", ColumnVariableType.Integer, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.OriginalPriceWithTax, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.StandardRetailPriceWithTax, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.OfferId, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.LinkedItem, "", ColumnVariableType.Boolean, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.InfoCodeItem, "", ColumnVariableType.Boolean, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.ItemGroup, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.BatchId, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.BatchExpDate, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.PaymentIndex, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.PaymentName, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.SerialId, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.Found, "", ColumnVariableType.Boolean, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.Blocked, "", ColumnVariableType.Boolean, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.Activated, "", ColumnVariableType.Boolean, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.RFID, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.UOM, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.Selected, "", ColumnVariableType.Boolean, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.ItemType, "", ColumnVariableType.Integer, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.DiscountVoucherVoided, "", ColumnVariableType.Boolean, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.MenuType, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.SentToStationPrinter, "", ColumnVariableType.Boolean, false);
            Columns.Add(colInfo);

            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo1, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo2, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo3, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo4, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo5, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo6, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo7, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo8, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo9, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo10, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo11, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo12, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo13, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo14, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo15, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo16, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo17, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo18, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo19, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
            colInfo = new ColumnInfo(ReceiptItemType.ExtraInfo20, "", ColumnVariableType.String, false);
            Columns.Add(colInfo);
        }

        public void WrapItemRowData(DataRow itemRow)
        {
            foreach (ColumnInfo col in Columns.Where(w => w.ColumnType == ColumnVariableType.Boolean))
            {
                string columnName = ColumnName(col.ReceiptItem);
                if (columnName != "")
                {
                    col.UpdateColumnInfo(Conversion.ToBool(itemRow[columnName]));
                }

            }

            foreach (ColumnInfo col in Columns.Where(w => w.ColumnType != ColumnVariableType.Boolean))
            {
                string columnName = ColumnName(col.ReceiptItem);
                if (columnName != "")
                {
                    col.UpdateColumnInfo(Conversion.ToStr(itemRow[columnName]));
                }
            }
        }

        public void UpdateItemRowData(ReceiptItemInfo itemInfo, DataRow itemRow)
        {
            foreach (ColumnInfo col in Columns.Where(w => w.Updated == true))
            {
                string colName = ColumnName(col.ReceiptItem);
                if (colName != "")
                {
                    itemRow[colName] = col.ColumnValue;
                }
            }
        }

        public string ColumnName(ReceiptItemType ReceiptItem)
        {
            switch (ReceiptItem)
            {
                case ReceiptItemType.BarcodeId:
                    return "BarcodeId";

                case ReceiptItemType.ItemId:
                    return "ItemId";

                case ReceiptItemType.ItemName:
                    return "ItemName";

                case ReceiptItemType.ItemNameAlias:
                    return "ItemNameAlias";

                case ReceiptItemType.Amount:
                    return "Amount";

                case ReceiptItemType.AmountWithoutTax:
                    return "AmountWithoutTax";

                case ReceiptItemType.Quantity:
                    return "Quantity";

                case ReceiptItemType.TotalAmount:
                    return "TotalAmount";

                case ReceiptItemType.TotalAmountWithoutTax:
                    return "TotalAmountWithoutTax";

                case ReceiptItemType.Comment:
                    return "Comment";

                case ReceiptItemType.TaxCode:
                    return "TaxCode";

                case ReceiptItemType.TaxAmount:
                    return "TaxAmount";

                case ReceiptItemType.TaxRatePct:
                    return "TaxRatePct";

                case ReceiptItemType.Voided:
                    return "Voided";

                case ReceiptItemType.LineId:
                    return "LineId";

                case ReceiptItemType.OriginalPriceWithTax:
                    return "OriginalPriceWithTax";

                case ReceiptItemType.StandardRetailPriceWithTax:
                    return "StandardRetailPriceWithTax";

                case ReceiptItemType.OfferId:
                    return "OfferId";

                case ReceiptItemType.LinkedItem:
                    return "LinkedItem";

                case ReceiptItemType.InfoCodeItem:
                    return "InfoCodeItem";

                case ReceiptItemType.ItemGroup:
                    return "ItemGroup";

                case ReceiptItemType.BatchId:
                    return "BatchId";

                case ReceiptItemType.BatchExpDate:
                    return "BatchExpDate";

                case ReceiptItemType.PaymentIndex:
                    return "PaymentIndex";

                case ReceiptItemType.PaymentName:
                    return "PaymentName";

                case ReceiptItemType.SerialId:
                    return "SerialId";

                case ReceiptItemType.Found:
                    return "Found";

                case ReceiptItemType.Blocked:
                    return "Blocked";

                case ReceiptItemType.Activated:
                    return "Activated";

                case ReceiptItemType.RFID:
                    return "RFID";

                case ReceiptItemType.UOM:
                    return "UOM";

                case ReceiptItemType.Selected:
                    return "Selected";

                case ReceiptItemType.ItemType:
                    return "ItemType";

                case ReceiptItemType.DiscountVoucherVoided:
                    return "DiscountVoucherVoided";

                case ReceiptItemType.MenuType:
                    return "MenuType";

                case ReceiptItemType.SentToStationPrinter:
                    return "SentToStationPrinter";

                case ReceiptItemType.ExtraInfo1:
                    return "ExtraInfo1";

                case ReceiptItemType.ExtraInfo2:
                    return "ExtraInfo2";

                case ReceiptItemType.ExtraInfo3:
                    return "ExtraInfo3";

                case ReceiptItemType.ExtraInfo4:
                    return "ExtraInfo4";

                case ReceiptItemType.ExtraInfo5:
                    return "ExtraInfo5";

                case ReceiptItemType.ExtraInfo6:
                    return "ExtraInfo6";

                case ReceiptItemType.ExtraInfo7:
                    return "ExtraInfo7";

                case ReceiptItemType.ExtraInfo8:
                    return "ExtraInfo8";

                case ReceiptItemType.ExtraInfo9:
                    return "ExtraInfo9";

                case ReceiptItemType.ExtraInfo10:
                    return "ExtraInfo10";

                case ReceiptItemType.ExtraInfo11:
                    return "ExtraInfo11";

                case ReceiptItemType.ExtraInfo12:
                    return "ExtraInfo12";

                case ReceiptItemType.ExtraInfo13:
                    return "ExtraInfo13";

                case ReceiptItemType.ExtraInfo14:
                    return "ExtraInfo14";

                case ReceiptItemType.ExtraInfo15:
                    return "ExtraInfo15";

                case ReceiptItemType.ExtraInfo16:
                    return "ExtraInfo16";

                case ReceiptItemType.ExtraInfo17:
                    return "ExtraInfo17";

                case ReceiptItemType.ExtraInfo18:
                    return "ExtraInfo18";

                case ReceiptItemType.ExtraInfo19:
                    return "ExtraInfo19";

                case ReceiptItemType.ExtraInfo20:
                    return "ExtraInfo20";
            }
            return "";
        }
    }
}
