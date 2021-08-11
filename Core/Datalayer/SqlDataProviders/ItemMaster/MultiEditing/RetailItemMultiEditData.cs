using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.DataProviders.ItemMaster.MultiEditing;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster.MultiEditing
{
    public class RetailItemMultiEditData : IRetailItemMultiEditData
    {
        private SqlServerStatement savedStatement;

        public void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }

        public void PrepareStatement(IConnectionManager entry, RetailItemMultiEdit item)
        {
            savedStatement = new SqlServerStatement("RETAILITEM");

            //ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit); // TODO Items Multi Edit perm

            savedStatement.StatementType = StatementType.Update;
            savedStatement.AddCondition("MASTERID", Guid.NewGuid(), SqlDbType.UniqueIdentifier);

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.RetailGroupMasterID) == RetailItemMultiEdit.FieldSelectionEnum.RetailGroupMasterID)
            {
                if (item.RetailGroupMasterID.IsEmpty)
                {
                    savedStatement.AddField("RETAILGROUPMASTERID", DBNull.Value, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    savedStatement.AddField("RETAILGROUPMASTERID", (Guid)item.RetailGroupMasterID, SqlDbType.UniqueIdentifier);
                }
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesTaxItemGroupID) == RetailItemMultiEdit.FieldSelectionEnum.SalesTaxItemGroupID)
            {
                savedStatement.AddField("SALESTAXITEMGROUPID", (string)item.SalesTaxItemGroupID);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.IsFuelItem) == RetailItemMultiEdit.FieldSelectionEnum.IsFuelItem)
            {
                savedStatement.AddField("FUELITEM", item.IsFuelItem, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.GradeID) == RetailItemMultiEdit.FieldSelectionEnum.GradeID)
            {
                savedStatement.AddField("GRADEID", item.GradeID);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.ScaleItem) == RetailItemMultiEdit.FieldSelectionEnum.ScaleItem)
            {
                savedStatement.AddField("SCALEITEM", item.ScaleItem, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.MustKeyInComment) == RetailItemMultiEdit.FieldSelectionEnum.MustKeyInComment)
            {
                savedStatement.AddField("MUSTKEYINCOMMENT", item.MustKeyInComment, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.ZeroPriceValid) == RetailItemMultiEdit.FieldSelectionEnum.ZeroPriceValid)
            {
                savedStatement.AddField("ZEROPRICEVALID", item.ZeroPriceValid, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.QuantityBecomesNegative) == RetailItemMultiEdit.FieldSelectionEnum.QuantityBecomesNegative)
            {
                savedStatement.AddField("QTYBECOMESNEGATIVE", item.QuantityBecomesNegative, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.NoDiscountAllowed) == RetailItemMultiEdit.FieldSelectionEnum.NoDiscountAllowed)
            {
                savedStatement.AddField("NODISCOUNTALLOWED", item.NoDiscountAllowed, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.MustSelectUOM) == RetailItemMultiEdit.FieldSelectionEnum.MustSelectUOM)
            {
                savedStatement.AddField("MUSTSELECTUOM", item.MustSelectUOM, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.KeyInPrice) == RetailItemMultiEdit.FieldSelectionEnum.KeyInPrice)
            {
                savedStatement.AddField("KEYINPRICE", item.KeyInPrice, SqlDbType.TinyInt);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.KeyInQuantity) == RetailItemMultiEdit.FieldSelectionEnum.KeyInQuantity)
            {
                savedStatement.AddField("KEYINQTY", item.KeyInQuantity, SqlDbType.TinyInt);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.KeyInSerialNumber) == RetailItemMultiEdit.FieldSelectionEnum.KeyInSerialNumber)
            {
                savedStatement.AddField("KEYINSERIALNUMBER", item.KeyInSerialNumber, SqlDbType.TinyInt);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.DateToBeBlocked) == RetailItemMultiEdit.FieldSelectionEnum.DateToBeBlocked)
            {
                savedStatement.AddField("DATETOBEBLOCKED", item.DateToBeBlocked.ToAxaptaSQLDate(), SqlDbType.DateTime);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.DateToActivateItem) == RetailItemMultiEdit.FieldSelectionEnum.DateToActivateItem)
            {
                savedStatement.AddField("DATETOACTIVATEITEM", item.DateToActivateItem.ToAxaptaSQLDate(), SqlDbType.DateTime);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.ValidationPeriodID) == RetailItemMultiEdit.FieldSelectionEnum.ValidationPeriodID)
            {
                savedStatement.AddField("VALIDATIONPERIODID", item.ValidationPeriodID);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesAllowTotalDiscount) == RetailItemMultiEdit.FieldSelectionEnum.SalesAllowTotalDiscount)
            {
                savedStatement.AddField("SALESALLOWTOTALDISCOUNT", item.SalesAllowTotalDiscount, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesLineDiscount) == RetailItemMultiEdit.FieldSelectionEnum.SalesLineDiscount)
            {
                savedStatement.AddField("SALESLINEDISC", (string)item.SalesLineDiscount);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesMultiLineDiscount) == RetailItemMultiEdit.FieldSelectionEnum.SalesMultiLineDiscount)
            {
                savedStatement.AddField("SALESMULTILINEDISC", (string)item.SalesMultiLineDiscount);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.ExtendedDescription) == RetailItemMultiEdit.FieldSelectionEnum.ExtendedDescription)
            {
                savedStatement.AddField("EXTENDEDDESCRIPTION", item.ExtendedDescription);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SearchKeywords) == RetailItemMultiEdit.FieldSelectionEnum.SearchKeywords)
            {
                savedStatement.AddField("SEARCHKEYWORDS", item.SearchKeywords);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.NameAlias) == RetailItemMultiEdit.FieldSelectionEnum.NameAlias)
            {
                savedStatement.AddField("NAMEALIAS", item.NameAlias);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesMarkup) == RetailItemMultiEdit.FieldSelectionEnum.SalesMarkup)
            {
                savedStatement.AddField("SALESMARKUP", item.SalesMarkup, SqlDbType.Decimal);
            }


            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin) == RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin)
            {
                savedStatement.AddField("PROFITMARGIN", item.ProfitMargin, SqlDbType.Decimal);
            }


            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice) == RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice)
            {
                savedStatement.AddField("PURCHASEPRICE", item.PurchasePrice, SqlDbType.Decimal);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesPrice) == RetailItemMultiEdit.FieldSelectionEnum.SalesPrice)
            {
                savedStatement.AddField("SALESPRICE", item.SalesPrice, SqlDbType.Decimal);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax) == RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax)
            {
                savedStatement.AddField("SALESPRICEINCLTAX", item.SalesPriceIncludingTax, SqlDbType.Decimal);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.TareWeight) == RetailItemMultiEdit.FieldSelectionEnum.TareWeight)
            {
                savedStatement.AddField("TAREWEIGHT", item.TareWeight, SqlDbType.Int);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.Returnable) == RetailItemMultiEdit.FieldSelectionEnum.Returnable)
            {
                savedStatement.AddField("RETURNABLE", item.Returnable, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.CanBeSold) == RetailItemMultiEdit.FieldSelectionEnum.CanBeSold)
            {
                savedStatement.AddField("CANBESOLD", item.CanBeSold, SqlDbType.Bit);
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.ProductionTime) == RetailItemMultiEdit.FieldSelectionEnum.ProductionTime)
            {
                savedStatement.AddField("PRODUCTIONTIME", item.ProductionTime, SqlDbType.Int);
            }
        }

        public virtual void UpdatePriceFields(RetailItemMultiEdit item)
        {
            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin) == RetailItemMultiEdit.FieldSelectionEnum.ProfitMargin)
            {
                if (savedStatement.HasField("PROFITMARGIN"))
                {
                    savedStatement.UpdateField("PROFITMARGIN", item.ProfitMargin);
                }
                else
                {
                    savedStatement.AddFieldNoCheck("PROFITMARGIN", item.ProfitMargin, SqlDbType.Decimal);
                }
            }


            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice) == RetailItemMultiEdit.FieldSelectionEnum.PurchasePrice)
            {
                if (savedStatement.HasField("PURCHASEPRICE"))
                {
                    savedStatement.UpdateField("PURCHASEPRICE", item.PurchasePrice);
                }
                else
                {
                    savedStatement.AddFieldNoCheck("PURCHASEPRICE", item.PurchasePrice, SqlDbType.Decimal);
                }
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesPrice) == RetailItemMultiEdit.FieldSelectionEnum.SalesPrice)
            {
                if (savedStatement.HasField("SALESPRICE"))
                {
                    savedStatement.UpdateField("SALESPRICE", item.SalesPrice);
                }
                else
                {
                    savedStatement.AddFieldNoCheck("SALESPRICE", item.SalesPrice, SqlDbType.Decimal);
                }
            }

            if ((item.FieldSelection & RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax) == RetailItemMultiEdit.FieldSelectionEnum.SalesPriceIncludingTax)
            {
                if (savedStatement.HasField("SALESPRICEINCLTAX"))
                {
                    savedStatement.UpdateField("SALESPRICEINCLTAX", item.SalesPriceIncludingTax);
                }
                else
                {
                    savedStatement.AddFieldNoCheck("SALESPRICEINCLTAX", item.SalesPriceIncludingTax, SqlDbType.Decimal);
                }
            }
        }

        public virtual void Save(IConnectionManager entry, RetailItemMultiEdit item)
        {
            PrepareStatement(entry, item);
            Save(entry, item.MasterID);
        }

        public virtual void Save(IConnectionManager entry, RecordIdentifier masterID)
        {
            if(savedStatement != null)
            {
                savedStatement.UpdateCondition(0, "MASTERID", (Guid)masterID);

                entry.Connection.ExecuteStatement(savedStatement);
            }

        }

        public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            throw new NotImplementedException();
        }
    }
}
