using System;
using System.Collections.Generic;
using System.Data;

using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Replenishment
{
	public class InventoryTemplateItemFilterData : SqlServerDataProviderBase, IInventoryTemplateItemFilterData
	{
		private static void Populate(IDataReader dr, InventoryTemplateFilterListItem item)
		{
			item.ID = (string)dr["ITEMID"];
			item.Text = (string)dr["ITEMNAME"];
			item.RetailGroupName = (string)dr["RETAILGROUPNAME"];
			item.RetailDepartmentName = (string)dr["RETAILDEPARTMENTNAME"];
			item.InventoryUnitId = (string)dr["INVENTORYUNIT"];
			item.InventoryUnitDescription = (string)dr["INVENTORYUNITDESCRIPTION"];
			item.PurchaseUnitId = (string)dr["PURCHASEUNIT"];
			item.PurchaseUnitDescription = (string)dr["PURCHASEUNITDESCRIPTION"];
			item.SalesUnitId = (string)dr["SALESUNIT"];
			item.SalesUnitDescription = (string)dr["SALESUNITDESCRIPTION"];
			item.VendorId = (string)dr["VENDORID"];
			item.VendorDescription = (string)dr["VENDORTXT"];
			item.DefaultItemBarcode = (string)dr["DEFAULTBARCODE"];
			item.HasSetting = AsBool(dr["HASSETTING"]);
			item.VariantName = (string)dr["VARIANTNAME"];
		}

		private static void PopulateExtended(IDataReader dr, InventoryTemplateFilterListItem item)
		{
			Populate(dr, item);
			
			item.HeaderItemId = (string) dr["HEADERITEMID"];
			item.VendorItemId = (string) dr["VENDORITEMID"];
		}

		private static void PopulateWithRowCount(IDataReader dr, InventoryTemplateFilterListItem item)
		{
			Populate(dr, item);
			item.TotalNumberOfRecords = (int)dr["Row_Count"];
		}

		private static string BuildMultipleParameters(IDbCommand cmd, List<RecordIdentifier> entities, string condition, string variableName)
		{
			string combinedCondition = "";

			if (entities != null && entities.Count > 0)
			{
				combinedCondition += "(";

				for (int i = 0; i < entities.Count; i++)
				{
					if (i != 0)
					{
						combinedCondition += " or ";
					}

					combinedCondition += condition + " @" + variableName + i;

					MakeParamNoCheck(cmd, variableName + i, (string)entities[i]);
				}

				combinedCondition += ")";
			}

			return combinedCondition;
		}

		public virtual List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(IConnectionManager entry, InventoryTemplateFilterContainer filter)
		{
			if (filter.RetailDepartments.Count == 0
				&& filter.RetailGroups.Count == 0
				&& filter.SpecialGroups.Count == 0
				&& filter.Vendors.Count == 0
                && !filter.FilterByInventoryOnHand)
			{
				// No condition found so we return empty list.
				return new List<InventoryTemplateFilterListItem>();
			}

            var needRowAndRowCount = !filter.LimitToFirst50Rows;

            using (var cmd = entry.Connection.CreateCommand())
			{
				List<TableColumn> columns = new List<TableColumn>
				{
					new TableColumn { ColumnName = "ITEMID", ColumnAlias = "ITEMID", TableAlias = "IT", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "ITEMNAME", ColumnAlias = "ITEMNAME", TableAlias = "IT", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "VARIANTNAME", ColumnAlias = "VARIANTNAME", TableAlias = "IT", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "NAME", ColumnAlias = "RETAILGROUPNAME", TableAlias = "RITRETGRP", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "NAME", ColumnAlias = "RETAILDEPARTMENTNAME", TableAlias = "RITDEP", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "INVENTORYUNITID", ColumnAlias = "INVENTORYUNIT", TableAlias = "IT", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "TXT", ColumnAlias = "INVENTORYUNITDESCRIPTION", TableAlias = "IU", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "PURCHASEUNITID", ColumnAlias = "PURCHASEUNIT", TableAlias = "IT", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "TXT", ColumnAlias = "PURCHASEUNITDESCRIPTION", TableAlias = "PU", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "SALESUNITID", ColumnAlias = "SALESUNIT", TableAlias = "IT", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "TXT", ColumnAlias = "SALESUNITDESCRIPTION", TableAlias = "SU", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "COALESCE(selectedVendor.ACCOUNTNUM, primaryVendor.ACCOUNTNUM, '')", ColumnAlias = "VENDORID", TableAlias = ""},
					new TableColumn { ColumnName = "COALESCE(selectedVendor.NAME, primaryVendor.NAME, '')", ColumnAlias = "VENDORTXT", TableAlias = ""},
					new TableColumn { ColumnName = "ITEMBARCODE", ColumnAlias = "DEFAULTBARCODE", TableAlias = "barcode", IsNull = true, NullValue = "''"},
					new TableColumn { ColumnName = "(SELECT COUNT(*) FROM ITEMREPLENISHMENTSETTING itemSetting where itemSetting.ITEMID = it.ITEMID)", ColumnAlias = "HASSETTING", TableAlias = ""}
				};

                if (needRowAndRowCount)
                {
                    columns.Add(new TableColumn { ColumnName = "ROW_NUMBER() OVER(ORDER BY IT.ITEMID ASC)", ColumnAlias = "ROW", TableAlias = "" });
                }

				List<Join> joins = new List<Join>
				{
					new Join { JoinType = "LEFT OUTER", Table = "RETAILGROUP", TableAlias = "RITRETGRP", Condition = "IT.RETAILGROUPMASTERID = RITRETGRP.MASTERID"},
					new Join { JoinType = "LEFT OUTER", Table = "RETAILDEPARTMENT", TableAlias = "RITDEP", Condition = "RITRETGRP.DEPARTMENTMASTERID = RITDEP.MASTERID"},
					new Join { JoinType = "LEFT OUTER", Table = "VENDORITEMS", TableAlias = "vi1", Condition = "vi1.RETAILITEMID = IT.ITEMID AND vi1.VENDORID = @selectedVendorId"},
					new Join { JoinType = "LEFT OUTER", Table = "VENDTABLE", TableAlias = "selectedVendor", Condition = "selectedVendor.ACCOUNTNUM = vi1.VENDORID"},
					new Join { JoinType = "LEFT OUTER", Table = "VENDORITEMS", TableAlias = "vi2", Condition = "vi2.VENDORID = IT.DEFAULTVENDORID AND vi2.RETAILITEMID = IT.ITEMID AND vi2.UNITID = IT.INVENTORYUNITID"},
					new Join { JoinType = "LEFT OUTER", Table = "VENDTABLE", TableAlias = "primaryVendor", Condition = "primaryVendor.ACCOUNTNUM = vi2.VENDORID"},
					new Join { JoinType = "LEFT OUTER", Table = "(SELECT *, ROW_NUMBER() OVER (PARTITION BY ITEMID ORDER BY RBOSHOWFORITEM DESC, ITEMID) AS BARCODE_ROW FROM INVENTITEMBARCODE WHERE DELETED = 0)", TableAlias = "barcode", Condition = "barcode.ITEMID = IT.ITEMID AND barcode.BARCODE_ROW = 1"},
					new Join { JoinType = "LEFT OUTER", Table = "UNIT", TableAlias = "IU", Condition = "IT.INVENTORYUNITID = IU.UNITID"},
					new Join { JoinType = "LEFT OUTER", Table = "UNIT", TableAlias = "PU", Condition = "IT.PURCHASEUNITID = PU.UNITID"},
					new Join { JoinType = "LEFT OUTER", Table = "UNIT", TableAlias = "SU", Condition = "IT.SALESUNITID = SU.UNITID"},
				};

				List<Condition> conditions = new List<Condition>
				{
					new Condition { Operator = "AND", ConditionValue = "IT.ITEMTYPE <> 3"},
					new Condition { Operator = "AND", ConditionValue = "IT.ITEMTYPE <> 2"},
					new Condition { Operator = "AND", ConditionValue = "IT.DELETED = 0"},
				};

				List<Condition> externalConditions = new List<Condition>();

                // If we only need the first 50 rows, we don't want to use the condition to filter by rows because it would slow down the query
                if (filter.LimitToFirst50Rows)
                {
                    filter.LimitRows = false;
                }

				if (filter.LimitRows)
				{
					externalConditions.Add(new Condition { ConditionValue = $"CTE.ROW >= {filter.RowFrom} AND CTE.ROW < {filter.RowTo} ", Operator = "AND" });
				}

				if (filter.RetailGroups.Count > 0)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = BuildMultipleParameters(cmd, filter.RetailGroups, "RITRETGRP.GROUPID = ", "retailGroup") });
				}

				if (filter.RetailDepartments.Count > 0)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = BuildMultipleParameters(cmd, filter.RetailDepartments, "RITDEP.DEPARTMENTID = ", "retailDepartment") });
				}

				if (filter.Vendors.Count == 0)
				{
					MakeParam(cmd, "selectedVendorId", "");
				}
				else
				{
					MakeParam(cmd, "selectedVendorId", (string)filter.Vendors[0]);
					conditions.Add(new Condition { Operator = "AND", ConditionValue = BuildMultipleParameters(cmd, filter.Vendors, "selectedVendor.ACCOUNTNUM = ", "vendorID") });
				}

				if (filter.SpecialGroups.Count > 0)
				{
					joins.Add(new Join { JoinType = "LEFT OUTER", Table = "SPECIALGROUPITEMS", TableAlias = "sg", Condition = "sg.ITEMID = it.ITEMID" });
					string specialGroupsConditions = BuildMultipleParameters(cmd, filter.SpecialGroups, "sg.GROUPID = ", "specialGroupID");
					conditions.Add(new Condition { Operator = "AND", ConditionValue = specialGroupsConditions });
				}

                string baseQuery = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "IT", filter.LimitToFirst50Rows ? 50 : 0),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                string selectClause = $"SELECT {QueryPartGenerator.ExternalColumnGenerator(columns, "CTE")}";
                if (needRowAndRowCount)
                {
                    selectClause += ", ROW_COUNT = tCountTransactions.ROW_COUNT ";
                }
                else
                {
                    selectClause += ", ROW_COUNT = -1 ";
                }

                string joinClause = "FROM CTE ";
                if (needRowAndRowCount)
                {
                    joinClause += "CROSS JOIN (SELECT Count(*) AS ROW_COUNT FROM CTE) AS tCountTransactions ";
                }

                cmd.CommandText = $";WITH CTE AS ({baseQuery}) " +
                    selectClause +
                    joinClause +
                    QueryPartGenerator.ConditionGenerator(externalConditions) +
                    "ORDER BY CTE.ITEMNAME";

                return Execute<InventoryTemplateFilterListItem>(entry, cmd, CommandType.Text, PopulateWithRowCount);
			}
		}

		/// <summary>
		/// Returns all items that match the item filter for given inventory template.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="templateId">Id of the <see cref="BusinessObjects.Replenishment.InventoryTemplate"/></param>
        /// <param name="storeID">Store ID used in case of filtering by inventory on hand</param>
        /// <param name="getItemsWithNoVendor">If true, items that have no vendor will also be included</param>
		/// <returns></returns>
		public virtual List<InventoryTemplateFilterListItem> GetInventoryTemplateItems(IConnectionManager entry, RecordIdentifier templateId, RecordIdentifier storeID, bool getItemsWithNoVendor)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = "spINVENTORY_GetItemsFromTemplateFilter";
				MakeParam(cmd, "TEMPLATEID", (string)templateId, SqlDbType.NVarChar);
				MakeParam(cmd, "STOREID", (string)storeID, SqlDbType.NVarChar);
				MakeParam(cmd, "GETITEMSWITHNOVENDOR", getItemsWithNoVendor, SqlDbType.Bit);

                var items = Execute<InventoryTemplateFilterListItem>(entry, cmd, CommandType.StoredProcedure, PopulateExtended);
				
				return items;
			}
		}
	}
}