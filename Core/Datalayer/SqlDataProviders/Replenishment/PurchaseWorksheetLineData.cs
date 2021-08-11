using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.SqlConnector.QueryHelpers;

namespace LSOne.DataLayer.SqlDataProviders.Replenishment
{
    public class PurchaseWorksheetLineData : SqlServerDataProviderBase, IPurchaseWorksheetLineData
    {
        private static List<TableColumn> SelectionColumns = new List<TableColumn>
        {
            new TableColumn { ColumnName = "ID", TableAlias = "PWL"},
            new TableColumn { ColumnName = "PURCHASEWORKSHEETID", TableAlias = "PWL"},
            new TableColumn { ColumnName = "BARCODENUMBER", TableAlias = "PWL"},
            new TableColumn { ColumnName = "ITEMID", TableAlias = "PWL"},
            new TableColumn { ColumnName = "QUANTITY", TableAlias = "PWL"},
            new TableColumn { ColumnName = "DELETED", TableAlias = "PWL"},
            new TableColumn { ColumnName = "UNITID", TableAlias = "PWL"},
            new TableColumn { ColumnName = "TXT", TableAlias = "U", IsNull = true, NullValue = "''", ColumnAlias = "UNITNAME"},
            new TableColumn { ColumnName = "VENDORID", TableAlias = "PWL"},
            new TableColumn { ColumnName = "NAME", TableAlias = "V", IsNull = true, NullValue = "''", ColumnAlias = "VENDORNAME"},
            new TableColumn { ColumnName = "MANUALLYEDITED", TableAlias = "PWL"},
            new TableColumn { ColumnName = "SUGGESTEDQUANTITY", TableAlias = "PWL"},
            new TableColumn { ColumnName = "STOREID", TableAlias = "PW"},
            new TableColumn { ColumnName = "ITEMNAME", TableAlias = "I", IsNull = true, NullValue = "''", ColumnAlias = "ITEMNAME"},
            new TableColumn { ColumnName = "VARIANTNAME", TableAlias = "I", IsNull = true, NullValue = "''", ColumnAlias = "VARIANTNAME"},
            new TableColumn { ColumnName = "INVENTORYUNITID", TableAlias = "I"},
            new TableColumn { ColumnName = "TXT", TableAlias = "IU", IsNull = true, NullValue = "''", ColumnAlias = "INVENTORYUNITNAME"},
            new TableColumn { ColumnName = "ITEMTYPE", TableAlias = "I", IsNull = true, NullValue = "0", ColumnAlias = "ITEMTYPE"},
            new TableColumn { ColumnName = "COALESCE(IRS1.REORDERPOINT, IRS2.REORDERPOINT, 0)", ColumnAlias = "REORDERPOINT"},
            new TableColumn { ColumnName = "COALESCE(IRS1.MAXIMUMINVENTORY, IRS2.MAXIMUMINVENTORY, 0)", ColumnAlias = "MAXIMUMINVENTORY"},
        };

        private static List<Join> Joins = new List<Join>
        {
            new Join { JoinType = "LEFT OUTER", Table = "RETAILITEM", TableAlias = "I", Condition = "PWL.ITEMID = I.ITEMID" },
            new Join { JoinType = "LEFT OUTER", Table = "UNIT", TableAlias = "U", Condition = "PWL.UNITID = U.UNITID AND PWL.DATAAREAID = U.DATAAREAID" },
            new Join { JoinType = "LEFT OUTER", Table = "UNIT", TableAlias = "IU", Condition = "I.INVENTORYUNITID = IU.UNITID AND PWL.DATAAREAID = IU.DATAAREAID" },
            new Join { JoinType = "LEFT OUTER", Table = "VENDTABLE", TableAlias = "V", Condition = "PWL.VENDORID = V.ACCOUNTNUM AND PWL.DATAAREAID = V.DATAAREAID" },
            new Join { JoinType = "LEFT OUTER", Table = "PURCHASEWORKSHEET", TableAlias = "PW", Condition = "PWL.PURCHASEWORKSHEETID = PW.ID AND PWL.DATAAREAID = PW.DATAAREAID" },
            new Join { JoinType = "LEFT OUTER", Table = "INVENTORYTEMPLATE", TableAlias = "IT", Condition = "IT.ID = PW.INVENTORYTEMPLATEID AND PWL.DATAAREAID = IT.DATAAREAID" },
            new Join { JoinType = "LEFT OUTER", Table = "ITEMREPLENISHMENTSETTING", TableAlias = "IRS1", Condition = "PWL.ITEMID = IRS1.ITEMID AND PW.STOREID = IRS1.STOREID AND PWL.DATAAREAID = IRS1.DATAAREAID" },
            new Join { JoinType = "LEFT OUTER", Table = "ITEMREPLENISHMENTSETTING", TableAlias = "IRS2", Condition = "PWL.ITEMID = IRS2.ITEMID AND IRS2.STOREID = '' AND PWL.DATAAREAID = IRS2.DATAAREAID" },
        };

        private static void PopulatePurchaseWorksheetLine(IConnectionManager entry, IDataReader dr, PurchaseWorksheetLine worksheetLine, object param)
        {
            worksheetLine.ID = (Guid)dr["ID"];
            worksheetLine.PurchaseWorksheetID = (string)dr["PURCHASEWORKSHEETID"];
            worksheetLine.BarCodeNumber = (string)dr["BARCODENUMBER"];
            worksheetLine.Item = new InventoryTemplateFilterListItem((string)dr["ITEMID"], (string)dr["ITEMNAME"]);
            worksheetLine.VariantName = (string)dr["VARIANTNAME"];
            worksheetLine.Unit = new DataEntity((string)dr["UNITID"], (string)dr["UNITNAME"]);
            worksheetLine.InventoryUnit = new DataEntity((string)dr["INVENTORYUNITID"], (string)dr["INVENTORYUNITNAME"]);
            worksheetLine.Vendor = new DataEntity((string)dr["VENDORID"], (string)dr["VENDORNAME"]);
            worksheetLine.Quantity = (decimal)dr["QUANTITY"];
            worksheetLine.SuggestedQuantity = (decimal)dr["SUGGESTEDQUANTITY"];
            worksheetLine.ReorderPoint = (decimal)dr["REORDERPOINT"];
            worksheetLine.MaximumInventory = (decimal)dr["MAXIMUMINVENTORY"];
            worksheetLine.EffectiveInventory = (decimal)dr["EFFECTIVEINVENTORY"];
            worksheetLine.StoreID = (string)dr["STOREID"];
            worksheetLine.Deleted = (bool)dr["DELETED"];
            worksheetLine.ManuallyEdited = (bool)dr["MANUALLYEDITED"];
            worksheetLine.ItemType = (ItemTypeEnum)((byte)dr["ITEMTYPE"]);

            if (dr.GetSchemaTable().Columns.Contains("ROW_COUNT"))
            {
                worksheetLine.TotalNumberOfRows = (int)dr["ROW_COUNT"];
            }
        }

        private static string ResolveSort(PurchaseWorksheetLineSortEnum sort, bool backwards, bool useTableAlias)
        {
            var sortString = "ORDER BY ";

            switch (sort)
            {
                case PurchaseWorksheetLineSortEnum.Barcode:
                    sortString += $"{(useTableAlias ? "PWL." : "")}BARCODENUMBER";
                    break;
                case PurchaseWorksheetLineSortEnum.Description:
                    sortString += "ITEMNAME";
                    break;
                case PurchaseWorksheetLineSortEnum.VariantName:
                    sortString += "VARIANTNAME";
                    break;
                case PurchaseWorksheetLineSortEnum.ItemId:
                    sortString += $"{(useTableAlias ? "PWL." : "")}ITEMID";
                    break;
                case PurchaseWorksheetLineSortEnum.ReorderPoint:
                    sortString += $"{(useTableAlias ? "IRS1." : "")}REORDERPOINT";
                    break;
                case PurchaseWorksheetLineSortEnum.MaximumInventory:
                    sortString += $"{(useTableAlias ? "IRS1." : "")}MAXIMUMINVENTORY";
                    break;
                case PurchaseWorksheetLineSortEnum.OrderingQuantity:
                    sortString += $"{(useTableAlias ? "PWL." : "")}QUANTITY";
                    break;
                case PurchaseWorksheetLineSortEnum.SuggestedQuantity:
                    sortString += $"{(useTableAlias ? "PWL." : "")}SUGGESTEDQUANTITY";
                    break;
                case PurchaseWorksheetLineSortEnum.UnitName:
                    sortString += useTableAlias ? "U.TXT" : "UNITNAME";
                    break;
                case PurchaseWorksheetLineSortEnum.VendorName:
                    sortString += useTableAlias ? "V.NAME" : "VENDORNAME";
                    break;
            }

            sortString += (backwards) ? " DESC" : " ASC";

            return sortString;
        }

        public List<PurchaseWorksheetLine> GetList(
            IConnectionManager entry,
            RecordIdentifier worksheetId,
            bool includeDeletedItems)
        {
            return GetList(entry, worksheetId, includeDeletedItems, PurchaseWorksheetLineSortEnum.ItemId, false);
        }

        public List<PurchaseWorksheetLine> GetPagedList(
            IConnectionManager entry,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards,
            int rowFrom,
            int rowTo)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sortString = ResolveSort(sortEnum, sortBackwards, true);

                List<TableColumn> columns = new List<TableColumn>();
                columns.AddRange(SelectionColumns);
                columns.Add(new TableColumn { ColumnName = "dbo.GETEFFECTIVEINVENTORY(pwl.ITEMID, pw.STOREID)", ColumnAlias = "EFFECTIVEINVENTORY" });
                columns.Add(new TableColumn { ColumnName = $"ROW_NUMBER() OVER ({sortString})", ColumnAlias = "ROW" });
                columns.Add(new TableColumn { ColumnName = $"COUNT(1) OVER ({sortString} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING)", ColumnAlias = "ROW_COUNT" });

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "PWL.PURCHASEWORKSHEETID = @WORKSHEETID"},
                    new Condition { Operator = "AND", ConditionValue = "PWL.DATAAREAID = @DATAAREAID"},
                    new Condition { Operator = "AND", ConditionValue = "I.DELETED = 0"}
                };

                if (!includeDeletedItems)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "PWL.DELETED = 0" });
                }

                List<Condition> externalConditions = new List<Condition> { new Condition { Operator = "AND", ConditionValue = $" ss.ROW >= {rowFrom} AND ss.ROW <= {rowTo}" } };

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "WORKSHEETID", (string)worksheetId);

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("PURCHASEWORKSHEETLINE ", "PWL", "ss", distinct: true),
                   QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(Joins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   QueryPartGenerator.ConditionGenerator(externalConditions),
                   ResolveSort(sortEnum, sortBackwards, false));

                return Execute<PurchaseWorksheetLine>(entry, cmd, CommandType.Text, null, PopulatePurchaseWorksheetLine);
            }
        }

        public List<PurchaseWorksheetLine> GetList(
            IConnectionManager entry,
            RecordIdentifier worksheetId,
            bool includeDeletedItems,
            PurchaseWorksheetLineSortEnum sortEnum,
            bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.AddRange(SelectionColumns);
                columns.Add(new TableColumn { ColumnName = "0.0", ColumnAlias = "EFFECTIVEINVENTORY" });

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "PWL.PURCHASEWORKSHEETID = @WORKSHEETID"},
                    new Condition { Operator = "AND", ConditionValue = "PWL.DATAAREAID = @DATAAREAID"},
                    new Condition { Operator = "AND", ConditionValue = "I.DELETED = 0"}
                };

                if (!includeDeletedItems)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "PWL.DELETED = 0" });
                }

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "WORKSHEETID", (string)worksheetId);

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PURCHASEWORKSHEETLINE ", "PWL", distinct: true),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(Joins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   ResolveSort(sortEnum, sortBackwards, true));

                return Execute<PurchaseWorksheetLine>(entry, cmd, CommandType.Text, null, PopulatePurchaseWorksheetLine);
            }
        }

        public virtual PurchaseWorksheetLine Get(IConnectionManager entry, RecordIdentifier worksheetLineId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>();
                columns.AddRange(SelectionColumns);
                columns.Add(new TableColumn { ColumnName = "0.0", ColumnAlias = "EFFECTIVEINVENTORY" });

                List<Condition> conditions = new List<Condition>
                {
                    new Condition { Operator = "AND", ConditionValue = "PWL.ID = @ID"},
                    new Condition { Operator = "AND", ConditionValue = "PWL.DATAAREAID = @DATAAREAID"},
                    new Condition { Operator = "AND", ConditionValue = "I.DELETED = 0"}
                };

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (Guid)worksheetLineId);

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("PURCHASEWORKSHEETLINE ", "PWL", distinct: true),
                   QueryPartGenerator.InternalColumnGenerator(columns),
                   QueryPartGenerator.JoinGenerator(Joins),
                   QueryPartGenerator.ConditionGenerator(conditions),
                   string.Empty);

                List<PurchaseWorksheetLine> results = Execute<PurchaseWorksheetLine>(entry, cmd, CommandType.Text, null, PopulatePurchaseWorksheetLine);
                return results.Count == 1 ? results[0] : null;
            }
        }

        public virtual decimal GetEffectiveInventoryForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier storeID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT dbo.GETEFFECTIVEINVENTORY(@ITEMID, @STOREID)";

                MakeParam(cmd, "ITEMID", itemID);
                MakeParam(cmd, "STOREID", storeID);

                object returnValue = entry.Connection.ExecuteScalar(cmd);
                return returnValue is decimal ? (decimal)returnValue : 0;
            }
        }

        public virtual decimal CalculateSuggestedQuantity(IConnectionManager entry, decimal inventoryOnHand, ItemReplenishmentSetting replenishmentSetting)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT dbo.CALCULATESUGGESTEDQUANTITY(@CALCULATE, @INVENTONHAND, @ROUNDINGMETHOD, @MULTIPLE, @MAXINVENTORY)";

                MakeParam(cmd, "CALCULATE", true, SqlDbType.Bit);
                MakeParam(cmd, "INVENTONHAND", inventoryOnHand, SqlDbType.Decimal);
                MakeParam(cmd, "ROUNDINGMETHOD", (int)replenishmentSetting.PurchaseOrderMultipleRounding, SqlDbType.Int);
                MakeParam(cmd, "MULTIPLE", replenishmentSetting.PurchaseOrderMultiple, SqlDbType.Int);
                MakeParam(cmd, "MAXINVENTORY", replenishmentSetting.MaximumInventory, SqlDbType.Decimal);

                object returnValue = entry.Connection.ExecuteScalar(cmd);
                return returnValue is decimal ? (decimal)returnValue : 0;
            }
        }

        public virtual RecordIdentifier GetPurchaseOrderUnit(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier vendorId)
        {

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT COALESCE(vendorUnit.UNITID, IT.INVENTORYUNITID, '') as PURCHASEORDERUNIT
                                    FROM RETAILITEM IT
                                    left outer join VENDORITEMS vendorUnit on vendorUnit.RETAILITEMID = @itemId and vendorUnit.VENDORID = @vendorId and vendorUnit.DATAAREAID = @dataAreaId
                                    where inventoryunit.ITEMID = @itemId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemId", itemId);
                MakeParam(cmd, "vendorId", vendorId);

                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual void DeleteForPurchaseWorksheet(IConnectionManager entry, RecordIdentifier purchaseWorksheetID)
        {
            DeleteRecord(entry, "PURCHASEWORKSHEETLINE", "PURCHASEWORKSHEETID", purchaseWorksheetID, Permission.ManageReplenishment);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier worksheetLineId)
        {
            return RecordExists(entry, "PURCHASEWORKSHEETLINE", "ID", worksheetLineId);
        }

        public virtual RecordIdentifier Save(IConnectionManager entry, PurchaseWorksheetLine worksheetLine)
        {
            var statement = new SqlServerStatement("PURCHASEWORKSHEETLINE");

            if (!Exists(entry, worksheetLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)worksheetLine.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)worksheetLine.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("PURCHASEWORKSHEETID", (string)worksheetLine.PurchaseWorksheetID);
            statement.AddField("BARCODENUMBER", worksheetLine.BarCodeNumber);
            statement.AddField("ITEMID", (string)worksheetLine.Item.ID);
            statement.AddField("UNITID", (string)worksheetLine.Unit.ID);
            statement.AddField("VENDORID", (string)worksheetLine.Vendor.ID);
            statement.AddField("SUGGESTEDQUANTITY", worksheetLine.SuggestedQuantity, SqlDbType.Decimal);
            statement.AddField("QUANTITY", worksheetLine.Quantity, SqlDbType.Decimal);
            statement.AddField("MANUALLYEDITED", worksheetLine.ManuallyEdited, SqlDbType.Bit);
            statement.AddField("DELETED", worksheetLine.Deleted, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
            return worksheetLine.ID;
        }

        public virtual bool PurchaseWorksheetHasInventoryExcludedItems(IConnectionManager entry, RecordIdentifier purchaseWorksheetID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT CASE WHEN EXISTS (SELECT 1 FROM PURCHASEWORKSHEETLINE PWL LEFT OUTER JOIN RETAILITEM I ON PWL.ITEMID = I.ITEMID 
                         WHERE PWL.PURCHASEWORKSHEETID = @PURCHASEWORKSHEETID AND PWL.DATAAREAID = @DATAAREAID AND PWL.DELETED = 0 AND I.DELETED = 0 AND I.ITEMTYPE = 2)
	                     THEN 1 ELSE 0 END";

                MakeParam(cmd, "PURCHASEWORKSHEETID", (string)purchaseWorksheetID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return (int)entry.Connection.ExecuteScalar(cmd) == 1;
            }
        }
    }
}