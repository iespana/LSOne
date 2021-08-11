using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Expressions;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    /// <summary>
    /// Data provider class for purchase order lines
    /// </summary>
    public class PurchaseOrderLineData : SqlServerDataProviderBase, IPurchaseOrderLineData
    {
        private static List<TableColumn> purchaseOrderColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "PURCHASEORDERID " , TableAlias = "t"},
            new TableColumn {ColumnName = "LINENUMBER " , TableAlias = "t"},
            new TableColumn {ColumnName = "RETAILITEMID " , TableAlias = "t"},
            new TableColumn {ColumnName = "ISNULL(v.VENDORITEMID,'') " ,ColumnAlias = "VENDORITEMID"},
            new TableColumn {ColumnName = "UNITID " , TableAlias = "t"},
            new TableColumn {ColumnName = "ISNULL(u.TXT, '') ", ColumnAlias = "UNITNAME" },
            new TableColumn {ColumnName = "UNITDECIMALS" , TableAlias = "u",ColumnAlias = "MAXUNITDECIMALS"},
            new TableColumn {ColumnName = "ISNULL(u.MINUNITDECIMALS,0)" , ColumnAlias = "MINUNITDECIMALS"},
            new TableColumn {ColumnName = "QUANTITY" , TableAlias = "t"},
            new TableColumn {ColumnName = "PRICE" , TableAlias = "t"},
            new TableColumn {ColumnName = "ISNULL(a.ITEMNAME,'')" , ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "ISNULL(a.VARIANTNAME,'')" , ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "a.DELETED" , ColumnAlias = "ITEMDELETED"},
            new TableColumn {ColumnName = "ITEMTYPE" , ColumnAlias = "ITEMTYPE"},
            new TableColumn {ColumnName = "ISNULL(t.DISCOUNTAMOUNT,0)" , ColumnAlias = "DISCOUNTAMOUNT"},
            new TableColumn {ColumnName = "ISNULL(t.DISCOUNTPERCENTAGE,0)" , ColumnAlias = "DISCOUNTPERCENTAGE"},
            new TableColumn {ColumnName = "ISNULL(t.TAXAMOUNT,0)" , ColumnAlias = "TAXAMOUNT"},
            new TableColumn {ColumnName = "ISNULL(t.TAXCALCULATIONMETHOD,0)", ColumnAlias  = "TAXCALCULATIONMETHOD"},
            new TableColumn {ColumnName = "ISNULL(t.PICTUREID, '')", ColumnAlias = "PICTUREID"},
            new TableColumn {ColumnName = "ISNULL(t.OMNILINEID, '')", ColumnAlias = "OMNILINEID"},
            new TableColumn {ColumnName = "ISNULL(t.OMNITRANSACTIONID, '')", ColumnAlias = "OMNITRANSACTIONID"},
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "p.PURCHASEORDERID = t.PURCHASEORDERID",
                JoinType = "LEFT OUTER",
                Table = "PURCHASEORDERS",
                TableAlias = "P"
            },
            new Join
            {
                Condition = "T.UNITID = U.UNITID",
                JoinType = "LEFT OUTER",
                Table = "UNIT",
                TableAlias = "U"
            },
            new Join
            {
                Condition = " T.RETAILITEMID = V.RETAILITEMID and P.VENDORID = V.VENDORID AND V.UNITID = T.UNITID ",
                JoinType = "LEFT OUTER",
                Table = "VENDORITEMS",
                TableAlias = "V"
            },
            new Join
            {
                Condition = "A.ITEMID = T.RETAILITEMID",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "A"
            },
            new Join
            {
                Condition = "p.DATAAREAID = c.DATAAREAID and p.STOREID = c.STOREID",
                Table = "RBOSTORETABLE",
                TableAlias = "c"
            },
        };

        /// <summary>
        /// Should be used for all "normal" SQL statements in this class
        /// </summary>
        /// <param name="sort">The sort columns that should be returned</param>
        /// <returns>The sort column ready to be used for a SQL statement</returns>
        private static string ResolveSort(PurchaseOrderLineSorting sort)
        {
            return ResolveSort(sort, string.Empty);
        }

        /// <summary>
        /// Should ONLY be used for paging SQL statements in this class and then only for the "external" statement
        /// </summary>
        /// <param name="sort">The sort columns that should be returned</param>
        /// <returns>The sort column ready to be used for a SQL statement</returns>
        /// <param name="alias">The table alias used for the result set from the internal SQL statement</param>
        /// <returns></returns>
        private static string ResolveSort(PurchaseOrderLineSorting sort, string alias)
        {
            string tAlias = string.IsNullOrEmpty(alias) ? "t" : alias;
            string aAlias = string.IsNullOrEmpty(alias) ? "a" : alias;

            switch (sort)
            {
                case PurchaseOrderLineSorting.PurchaseOrderID:
                    return tAlias + ".PURCHASEORDERID";
                case PurchaseOrderLineSorting.LineNumber:
                    return tAlias + ".LINENUMBER";
                case PurchaseOrderLineSorting.ItemID:
                    return tAlias + ".RETAILITEMID";
                case PurchaseOrderLineSorting.VendorItemID:
                    return tAlias + ".VENDORITEMID";
                case PurchaseOrderLineSorting.UnitID:
                    return tAlias + ".UNITID";
                case PurchaseOrderLineSorting.Quantity:
                    return tAlias + ".QUANTITY";
                case PurchaseOrderLineSorting.Price:
                    return tAlias + ".PRICE";
                case PurchaseOrderLineSorting.ItemName:
                    return aAlias + ".ITEMNAME";
                case PurchaseOrderLineSorting.DiscountAmount:
                    return "DiscountAmount";
                case PurchaseOrderLineSorting.DiscountPercentage:
                    return "DiscountPercentage";
                case PurchaseOrderLineSorting.TaxAmount:
                    return "TaxAmount";
            }

            return "";
        }

        private static void PopulatePurchaseOrderLineWithRowCount(IConnectionManager entry, IDataReader dr, PurchaseOrderLine purchaseOrderLine, ref int rowCount)
        {
            PopulatePurchaseOrderLine(entry, dr, purchaseOrderLine, false);

            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
               entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["ROW_COUNT"];
            }
        }

		private static void PopulatePurchaseOrderLine(IConnectionManager entry, IDataReader dr, PurchaseOrderLine purchaseOrderLine, object includeReportFormatting)
		{
			purchaseOrderLine.PurchaseOrderID = (string)dr["PURCHASEORDERID"];
			purchaseOrderLine.LineNumber = (string)dr["LINENUMBER"];
			purchaseOrderLine.ItemID = (string)dr["RETAILITEMID"];
			purchaseOrderLine.ItemName = (string)dr["ITEMNAME"];
			purchaseOrderLine.VariantName = (string)dr["VARIANTNAME"];
			purchaseOrderLine.ItemDeleted = (bool)dr["ITEMDELETED"];
			purchaseOrderLine.ItemType = (ItemTypeEnum)((byte)dr["ITEMTYPE"]);
			purchaseOrderLine.VendorItemID = (string)dr["VENDORITEMID"] == "" ? purchaseOrderLine.ItemID : (string)dr["VENDORITEMID"];

			purchaseOrderLine.UnitID = (string)dr["UNITID"];
			purchaseOrderLine.UnitName = (string)dr["UNITNAME"];
			purchaseOrderLine.Quantity = (decimal)dr["QUANTITY"];
			purchaseOrderLine.UnitPrice = (decimal)dr["PRICE"];
			purchaseOrderLine.DiscountAmount = (decimal)dr["DISCOUNTAMOUNT"];
			purchaseOrderLine.DiscountPercentage = (decimal)dr["DISCOUNTPERCENTAGE"];
			purchaseOrderLine.TaxAmount = (decimal)dr["TAXAMOUNT"];
			purchaseOrderLine.TaxCalculationMethod = (TaxCalculationMethodEnum)dr["TAXCALCULATIONMETHOD"];
			purchaseOrderLine.PictureID = (string)dr["PICTUREID"];
			purchaseOrderLine.OmniLineID = (string)dr["OMNILINEID"];
			purchaseOrderLine.OmniTransactionID = (string)dr["OMNITRANSACTIONID"];

			if ((bool)includeReportFormatting)
			{
				if (dr["MAXUNITDECIMALS"] == DBNull.Value)
				{
					purchaseOrderLine.QuantityLimiter = entry.GetDecimalSetting(DecimalSettingEnum.Quantity);
				}
				else
				{
					var maxDecimals = (int)dr["MAXUNITDECIMALS"];
					var minDecimals = (int)dr["MINUNITDECIMALS"];

					purchaseOrderLine.QuantityLimiter = new DecimalLimit(minDecimals, maxDecimals);
				}

				purchaseOrderLine.PriceLimiter = entry.GetDecimalSetting(DecimalSettingEnum.Prices);
				purchaseOrderLine.PercentageLimiter = entry.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);
			}
		}

		/// <summary>
		/// Gets a list of PurchaseOrderLines for a given PurchaseOrderID. The list is sorted by the parameter 'sort'
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="purchaseOrderID">The purchase order ID to get purchase order lines by</param>
		/// <param name="storeID">The store that the purchar order lines are tied to</param>
		/// <param name="sortBy">A enum that defines how the result should be sorted</param>
		/// <param name="sortBackwards">Set to true if wanting backwards sort</param>
		/// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param> 
		/// <returns>A list of PurchaseOrderLines for a given PurchaseOrderID</returns>
		public virtual List<PurchaseOrderLine> GetPurchaseOrderLines(IConnectionManager entry, RecordIdentifier purchaseOrderID, RecordIdentifier storeID, PurchaseOrderLineSorting sortBy, bool sortBackwards, bool includeReportFormatting)
		{
			ValidateSecurity(entry);

			List<PurchaseOrderLine> result;

			List<Condition> internalConditions = new List<Condition>();

			internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "t.PURCHASEORDERID = @PURCHASEORDERID " });
			internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

			using (var cmd = entry.Connection.CreateCommand())
			{

				cmd.CommandText = string.Format(
					QueryTemplates.BaseQuery("PURCHASEORDERLINE", "t"),
					QueryPartGenerator.InternalColumnGenerator(purchaseOrderColumns),
					QueryPartGenerator.JoinGenerator(listJoins),
					QueryPartGenerator.ConditionGenerator(internalConditions),
					"ORDER BY " + ResolveSort(sortBy) + (sortBackwards ? " DESC" : " ASC")
					);

				MakeParam(cmd, "PURCHASEORDERID", (string)purchaseOrderID.PrimaryID);

				result = Execute<PurchaseOrderLine>(entry, cmd, CommandType.Text, includeReportFormatting, PopulatePurchaseOrderLine);
			}

			return result;
		}

		public virtual List<PurchaseOrderLine> GetPurchaseOrderLines(IConnectionManager entry, RecordIdentifier purchaseOrderID, RecordIdentifier storeID, bool includeReportFormatting)
		{
			return GetPurchaseOrderLines(entry, purchaseOrderID, storeID, PurchaseOrderLineSorting.ItemName, false, includeReportFormatting);
		}

		public virtual List<PurchaseOrderLine> GetPurchaseOrderLines(IConnectionManager entry, PurchaseOrderLineSearch searchCriteria,
			PurchaseOrderLineSorting sortBy, bool sortBackwards, out int totalRecordsMatching)
		{            

			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				string defaultSort = "ITEMNAME";

				/***************
				   Create the external (outer) columns
				*****************/
				List<TableColumn> externalColumns = new List<TableColumn>(purchaseOrderColumns);

				externalColumns.Add(new TableColumn
				{
					ColumnName = "ROW"
				});

				externalColumns.Add(new TableColumn
				{
					ColumnName = "ROW_COUNT"
				});

				/***************
				   Add the WHERE for the paging
				*****************/
				List<Condition> externalConditions = new List<Condition>();
				externalConditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = "ex.ROW BETWEEN @ROWFROM AND @ROWTO"
				});
				MakeParam(cmd, "ROWFROM", searchCriteria.StartRecord, SqlDbType.Int);
				MakeParam(cmd, "ROWTO", searchCriteria.EndRecord, SqlDbType.Int);

				/***************
				   Add the internal columns - the actual search SQL statement
				*****************/
				List<TableColumn> internalColumns = new List<TableColumn>(purchaseOrderColumns);

				internalColumns.Add(new TableColumn
				{
					ColumnName = string.Format("ROW_NUMBER() OVER(ORDER BY {0})",
								sortBy == PurchaseOrderLineSorting.None ? defaultSort : ResolveSort(sortBy)),
					ColumnAlias = "ROW"
				});

				internalColumns.Add(new TableColumn
				{
					ColumnName = string.Format(
								"COUNT(1) OVER ( ORDER BY {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
								sortBy == PurchaseOrderLineSorting.None ? defaultSort : ResolveSort(sortBy)),
					ColumnAlias = "ROW_COUNT"
				});

				/***************
				   Add the internal WHERE conditions - the actual search SQL statement
				*****************/
				List<Condition> internalConditions = new List<Condition>();

				internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "t.PURCHASEORDERID = @PURCHASEORDERID " });
				MakeParam(cmd, "PURCHASEORDERID", (string)searchCriteria.PurchaseOrderID.PrimaryID);

				internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "t.DATAAREAID = @DATAAREAID " });
				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

				if (!string.IsNullOrWhiteSpace(searchCriteria.StoreID.StringValue))
				{
					internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "P.STOREID = @STOREID" });
					MakeParam(cmd, "STOREID", (string)searchCriteria.StoreID);
				}

				if (searchCriteria.ItemNameSearch != null && searchCriteria.ItemNameSearch.Count > 0)
				{
					List<Condition> searchConditions = new List<Condition>();
					for (int index = 0; index < searchCriteria.ItemNameSearch.Count; index++)
					{
						var searchToken = PreProcessSearchText(searchCriteria.ItemNameSearch[index], true, searchCriteria.DescriptionBeginsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							searchConditions.Add(new Condition
							{
								ConditionValue =
									$@" (A.ITEMNAME Like @ITEMNAME{index} 
										OR t.RETAILITEMID LIKE @ITEMNAME{index} 
										OR v.VENDORITEMID LIKE @ITEMNAME{index} 
										OR A.VARIANTNAME LIKE @ITEMNAME{index}) ",
								Operator = "AND"

							});

							MakeParam(cmd, $"ITEMNAME{index}", searchToken);
						}
					}
					internalConditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue =
							$" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
					});
				}

				if (searchCriteria.VariantSearch != null && searchCriteria.VariantSearch.Count > 0)
				{
					List<Condition> searchConditions = new List<Condition>();
					for (int index = 0; index < searchCriteria.VariantSearch.Count; index++)
					{
						var searchToken = PreProcessSearchText(searchCriteria.VariantSearch[index], true, searchCriteria.VariantSearchBeginsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							searchConditions.Add(new Condition
							{
								ConditionValue =
									$@" (A.VARIANTNAME LIKE @VARIANT{index}) ",
								Operator = "AND"

							});

							MakeParam(cmd, $"VARIANT{index}", searchToken);
						}
					}
					internalConditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue =
							$" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
					});
				}

				if (searchCriteria.Quantity != null)
				{
					if (searchCriteria.QuantityOperator == DoubleValueOperator.Equals)
					{
						internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "t.QUANTITY = @QUANTITY" });
					}
					else if (searchCriteria.QuantityOperator == DoubleValueOperator.GreaterThan)
					{
						internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "t.QUANTITY > @QUANTITY" });
					}
					else if (searchCriteria.QuantityOperator == DoubleValueOperator.LessThan)
					{
						internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "t.QUANTITY < @QUANTITY" });
					}

					MakeParam(cmd, "QUANTITY", searchCriteria.Quantity, SqlDbType.Int);
				}

				if (searchCriteria.HasDiscount != null)
				{
					if ((bool)searchCriteria.HasDiscount)
					{
						internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "(t.DISCOUNTAMOUNT <> 0 OR t.DISCOUNTPERCENTAGE <> 0)" });
					}
					else
					{
						internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "(t.DISCOUNTAMOUNT = 0 AND t.DISCOUNTPERCENTAGE = 0)" });
					}
				}

				if (!searchCriteria.ShowDeleted)
				{
					internalConditions.Add(new Condition { Operator = "AND", ConditionValue = "(A.DELETED = 0)" });
				}

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("PURCHASEORDERLINE", "t", "ex", 0, true),
				   QueryPartGenerator.ExternalColumnGenerator(externalColumns, "ex"),
				   QueryPartGenerator.InternalColumnGenerator(internalColumns),
				   QueryPartGenerator.JoinGenerator(listJoins),
				   QueryPartGenerator.ConditionGenerator(internalConditions),
				   QueryPartGenerator.ConditionGenerator(externalConditions),
				   "ORDER BY " + ResolveSort(sortBy, "ex") + (sortBackwards ? " DESC" : " ASC"));

                totalRecordsMatching = 0;
				return Execute<PurchaseOrderLine, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulatePurchaseOrderLineWithRowCount);
			}
		}

		/// <summary>
		/// Checks if a purchase order line with a given ID exists in the database.
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="purchaseOrderLineID">The ID of the purchase order line to check for</param>
		/// <returns>Whether a purchase order line with a given ID exists in the database</returns>
		public virtual bool Exists(IConnectionManager entry, RecordIdentifier purchaseOrderLineID)
		{
			return RecordExists(entry, "PURCHASEORDERLINE", new[] { "PURCHASEORDERID", "LINENUMBER" }, purchaseOrderLineID);
		}

		private static bool LineNumberExists(IConnectionManager entry, RecordIdentifier lineNumber)
		{
			return RecordExists(entry, "PURCHASEORDERLINE", "LINENUMBER", lineNumber);
		}

		/// <summary>
		/// Deletes a purchase order line with a given ID
		/// </summary>
		/// <remarks>Requires the 'ManagePurchaseOrders' permission</remarks>
		/// <param name="entry">The entry into the database</param>
		/// <param name="purchaseOrderLineID">The ID of the purchase order line to delete</param>
		public virtual void Delete(IConnectionManager entry, RecordIdentifier purchaseOrderLineID)
		{
			DeleteRecord(entry, "PURCHASEORDERLINE", new[] { "PURCHASEORDERID", "LINENUMBER" }, purchaseOrderLineID, BusinessObjects.Permission.ManagePurchaseOrders);
		}

		/// <summary>
		/// Gets a purchase order line with a given ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="purchaseOrderLineID">The ID of the purchase order line to get</param>
		/// <param name="storeID">The ID of the store that we are ordering for</param>
		/// <param name="includeReportFormatting">Set to true if you want price and quantity formatting, usually for reports</param>
		/// <returns>A purchase order line with a given ID</returns>
		public virtual PurchaseOrderLine Get(IConnectionManager entry, RecordIdentifier purchaseOrderLineID, RecordIdentifier storeID, bool includeReportFormatting)
		{
			ValidateSecurity(entry);

			List<PurchaseOrderLine> result;
			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.PURCHASEORDERID = @PURCHASEORDERID " });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.LINENUMBER = @LINENUMBER " });

				cmd.CommandText = string.Format(
				 QueryTemplates.BaseQuery("PURCHASEORDERLINE", "t"),
				 QueryPartGenerator.InternalColumnGenerator(purchaseOrderColumns),
				 QueryPartGenerator.JoinGenerator(listJoins),
				 QueryPartGenerator.ConditionGenerator(conditions),
				 string.Empty
				 );

				MakeParam(cmd, "PURCHASEORDERID", (string)purchaseOrderLineID.PrimaryID);
				MakeParam(cmd, "LINENUMBER", (string)purchaseOrderLineID.SecondaryID);

				result = Execute<PurchaseOrderLine>(entry, cmd, CommandType.Text, includeReportFormatting, PopulatePurchaseOrderLine);
			}

			return (result.Count > 0) ? result[0] : null;
		}

		/// <summary>
		/// Saves a given purchase order line into the database
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="purchaseOrderLine">The Purchase order line to save</param>
		public virtual void Save(IConnectionManager entry, PurchaseOrderLine purchaseOrderLine)
		{
			var statement = new SqlServerStatement("PURCHASEORDERLINE", false);

			ValidateSecurity(entry, BusinessObjects.Permission.ManagePurchaseOrders);

			if (purchaseOrderLine.LineNumber == null || !Exists(entry, purchaseOrderLine.ID))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddKey("PURCHASEORDERID", (string)purchaseOrderLine.PurchaseOrderID);
				if (purchaseOrderLine.LineNumber == null)
				{
					purchaseOrderLine.LineNumber = DataProviderFactory.Instance.GenerateNumber<IPurchaseOrderLineData, PurchaseOrderLine>(entry);
				}
				statement.AddKey("LINENUMBER", (string)purchaseOrderLine.LineNumber);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("PURCHASEORDERID", (string)purchaseOrderLine.PurchaseOrderID);
				statement.AddCondition("LINENUMBER", (string)purchaseOrderLine.LineNumber);
			}

			statement.AddField("RETAILITEMID", purchaseOrderLine.ItemID);
			statement.AddField("VENDORITEMID", purchaseOrderLine.VendorItemID);
			statement.AddField("UNITID", purchaseOrderLine.UnitID);
			statement.AddField("QUANTITY", purchaseOrderLine.Quantity, SqlDbType.Decimal);
			statement.AddField("PRICE", purchaseOrderLine.UnitPrice, SqlDbType.Decimal);
			statement.AddField("DISCOUNTAMOUNT", purchaseOrderLine.DiscountAmount, SqlDbType.Decimal);
			statement.AddField("DISCOUNTPERCENTAGE", purchaseOrderLine.DiscountPercentage, SqlDbType.Decimal);
			statement.AddField("TAXAMOUNT", purchaseOrderLine.TaxAmount, SqlDbType.Decimal);
			statement.AddField("TAXCALCULATIONMETHOD", (int)purchaseOrderLine.TaxCalculationMethod, SqlDbType.Int);
			statement.AddField("PICTUREID", (string)purchaseOrderLine.PictureID);
			statement.AddField("OMNILINEID", purchaseOrderLine.OmniLineID);
			statement.AddField("OMNITRANSACTIONID", purchaseOrderLine.OmniTransactionID);

			entry.Connection.ExecuteStatement(statement);
		}

		public virtual void CopyLinesBetweenPurchaseOrders(IConnectionManager entry, RecordIdentifier fromPurchaseOrderID, PurchaseOrder toPurchaseOrder, RecordIdentifier storeID, TaxCalculationMethodEnum taxCalculationMethod)
		{
			var purchaseOrderLines = GetPurchaseOrderLines(entry, fromPurchaseOrderID, storeID, PurchaseOrderLineSorting.ItemName, false, false);

			purchaseOrderLines = (from p in purchaseOrderLines
								  orderby (string)p.LineNumber
								  select p).Where(el => !el.ItemInventoryExcluded).ToList();

			foreach (var line in purchaseOrderLines)
			{
				line.PurchaseOrderID = toPurchaseOrder.ID;
				line.DiscountAmount = toPurchaseOrder.DefaultDiscountAmount;
				line.DiscountPercentage = toPurchaseOrder.DefaultDiscountPercentage;
				line.TaxCalculationMethod = taxCalculationMethod;
				Save(entry, line);
			}
		}

		public bool PurchaseOrderLineWithSameItemExists(
			IConnectionManager entry,
			RecordIdentifier purchaseOrderID,
			RecordIdentifier retailItemID,
			RecordIdentifier unitID)
		{
			var lines = GetPurchaseOrderLinesfromItemInfo(entry, purchaseOrderID, retailItemID, unitID);
			return (lines.Count > 0);
		}

		public string GetPurchaseOrderLineNumberFromItemInfo(
			IConnectionManager entry,
			RecordIdentifier purchaseOrderID,
			RecordIdentifier retailItemID,
			RecordIdentifier unitID)
		{
			var lines = GetPurchaseOrderLinesfromItemInfo(entry, purchaseOrderID, retailItemID, unitID);

			return (lines.Count > 0) ? (string)lines[0].LineNumber : "0";
		}

		private static List<PurchaseOrderLine> GetPurchaseOrderLinesfromItemInfo(
			IConnectionManager entry,
			RecordIdentifier purchaseOrderID,
			RecordIdentifier retailItemID,
			RecordIdentifier unitID
			)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.PURCHASEORDERID = @PURCHASEORDERID " });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.RETAILITEMID = @RETAILITEMID " });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.UNITID = @UNITID " });

				cmd.CommandText = string.Format(
				 QueryTemplates.BaseQuery("PURCHASEORDERLINE", "t"),
				 QueryPartGenerator.InternalColumnGenerator(purchaseOrderColumns),
				 QueryPartGenerator.JoinGenerator(listJoins),
				 QueryPartGenerator.ConditionGenerator(conditions),
				 string.Empty
				 );

				MakeParam(cmd, "PURCHASEORDERID", (string)purchaseOrderID);
				MakeParam(cmd, "RETAILITEMID", (string)retailItemID);
				MakeParam(cmd, "UNITID", (string)unitID);

				return Execute<PurchaseOrderLine>(entry, cmd, CommandType.Text, false, PopulatePurchaseOrderLine);
			}
		}

		/// <summary>
		/// Tells you if a given purchase order line has corresponding goods receiving lines
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="purchaseOrderLineID">The ID of the purchase order line to check for goods receiving lines</param>
		/// <returns>Whether a given purchase order line has corresponding goods receiving lines</returns>
		public virtual bool GoodsReceivingLineExists(IConnectionManager entry, RecordIdentifier purchaseOrderLineID)
		{
			var grdls = Providers.GoodsReceivingDocumentData.GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(entry, purchaseOrderLineID);

			return grdls.Count > 0;
		}

		/// <summary>
		/// Returns a the number items on order per store
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The item ID to check for</param>
		/// <param name="storeID">The store ID to check for - if empty then the total sum for all stores is returned</param>
		/// <param name="includeReportFormatting"></param>
		/// <param name="inventoryUnitId"></param>
		/// <returns></returns>
		public decimal GetSumofOrderedItembyStore(IConnectionManager entry,
			RecordIdentifier itemID,
			RecordIdentifier storeID,
			bool includeReportFormatting,
			RecordIdentifier inventoryUnitId)
		{
			decimal ordered = 0;
			decimal received = 0;
			ValidateSecurity(entry);

			List<PurchaseOrderLine> lines;
			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Join> selectionJoins = new List<Join>(listJoins);

				selectionJoins.Add(new Join
				{
					Condition = "P.PURCHASEORDERID = GR.PURCHASEORDERID ",
					JoinType = "LEFT OUTER",
					Table = "GOODSRECEIVING",
					TableAlias = "GR"
				});

				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "T.RETAILITEMID = @ITEMID " });

				if (!string.IsNullOrEmpty(storeID.StringValue))
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.STOREID = @STOREID " });

					MakeParam(cmd, "STOREID", (string)storeID);
				}

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "ISNULL(GR.STATUS, 0) = 0" });

				cmd.CommandText = string.Format(
				QueryTemplates.InternalQuery("PURCHASEORDERLINE", "t", "Source"),
				QueryPartGenerator.ExternalColumnGenerator(purchaseOrderColumns, "Source"),
				QueryPartGenerator.InternalColumnGenerator(purchaseOrderColumns),
				QueryPartGenerator.JoinGenerator(selectionJoins),
				QueryPartGenerator.ConditionGenerator(conditions),
				string.Empty,
				string.Empty,
				string.Empty,
				@"
					GROUP BY 
						PURCHASEORDERID,
						LINENUMBER,
						RETAILITEMID,
						VENDORITEMID,
						UNITID,
						UNITNAME,
						MAXUNITDECIMALS,
						MINUNITDECIMALS,
						QUANTITY,
						PRICE,
						ITEMNAME,
						VARIANTNAME,
						DISCOUNTAMOUNT,
						DISCOUNTPERCENTAGE,
						TAXAMOUNT,
						TAXCALCULATIONMETHOD,
						ITEMDELETED,
						ITEMTYPE,
						PICTUREID,
						OMNILINEID,
						OMNITRANSACTIONID");

				MakeParam(cmd, "ITEMID", (string)itemID);

				lines = Execute<PurchaseOrderLine>(entry, cmd, CommandType.Text, includeReportFormatting, PopulatePurchaseOrderLine);
			}

			if (lines != null)
			{
				foreach (PurchaseOrderLine line in lines)
				{
					var orderedQuantityInInventoryUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, itemID, line.UnitID,
																							inventoryUnitId,
																							line.Quantity);
					ordered += orderedQuantityInInventoryUnit;
					if (HasPostedGoodsReceivingDocumentLines(entry, line.ID))
					{
						var receivedQuantity = Providers.GoodsReceivingDocumentLineData.GetReceivedItemQuantity(entry, line.PurchaseOrderID, line.LineNumber, false);
						var receivedQuantityInInventoryUnit = Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, itemID, line.UnitID, inventoryUnitId, receivedQuantity);
						received += receivedQuantityInInventoryUnit;
					}
				}
			}
			var result = ordered - received;
			return result;
		}

		public virtual bool HasPostedGoodsReceivingDocumentLines(IConnectionManager entry, RecordIdentifier purchaseOrderLineID)
		{
			var goodsReceivingDocumentLinesForPurchaseOrderLine = Providers.GoodsReceivingDocumentData.GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(entry, purchaseOrderLineID);

			if (goodsReceivingDocumentLinesForPurchaseOrderLine.Count > 0)
			{
				return goodsReceivingDocumentLinesForPurchaseOrderLine.Any(line => Providers.GoodsReceivingDocumentLineData.IsPosted(entry, line.ID));
			}

			return false;
		}

		public void ChangeDiscountsForPurchaseOrderLines(
			IConnectionManager entry,
			RecordIdentifier purchaseOrderID,
			RecordIdentifier storeID,
			decimal? discountPercentage,
			decimal? discountAmount)
		{
			var poLines = GetPurchaseOrderLines(entry, purchaseOrderID, storeID, false);

			foreach (var poLine in poLines)
			{
				if (discountPercentage != null)
				{
					poLine.DiscountPercentage = (decimal)discountPercentage;
				}

				if (discountAmount != null)
				{
					poLine.DiscountAmount = (decimal)discountAmount;
				}
				Save(entry, poLine);
			}
		}

		public List<InventoryTotals> GetOrderedTotals(IConnectionManager entry, int numberOfDocuments)
		{
			ValidateSecurity(entry);

			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				List<TableColumn> totalColumns = new List<TableColumn>();
				totalColumns.Add(new TableColumn { ColumnName = "PURCHASEORDERID", TableAlias = "a" });
				totalColumns.Add(new TableColumn { ColumnName = "SUM(QUANTITY)", ColumnAlias = "QUANTITY" });

				cmd.CommandText = string.Format(
					QueryTemplates.BaseQuery("PURCHASEORDERLINE", "a", numberOfDocuments),
					QueryPartGenerator.InternalColumnGenerator(totalColumns),
					QueryPartGenerator.JoinGenerator(new List<Join>()),
					QueryPartGenerator.ConditionGenerator(new Condition { ConditionValue = "a.DATAAREAID = @DATAAREAID", Operator = "AND" }),
					"GROUP BY PURCHASEORDERID");

				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

				return Execute<InventoryTotals>(entry, cmd, CommandType.Text, PopulateOrderedTotals);
			}
		}

		private static void PopulateOrderedTotals(IDataReader dr, InventoryTotals total)
		{
			total.ID = (string)dr["PURCHASEORDERID"];
			total.Quantity = (decimal)dr["QUANTITY"];
		}

		/// <summary>
		/// Updates a single line with a picture ID based on the transaction ID and line IDs from the inventory app
		/// </summary>
		/// <param name="entry">The connection to the database</param>
		/// <param name="omniTransactionID">The ID of the transaction in the inventory app that this line was created on</param>
		/// <param name="omniLineID">The ID of the line that was assigned to it by the inventory app</param>
		/// <param name="pictureID">The ID of the picture to set on the line</param>
		public virtual void SetPictureIDForOmniLine(IConnectionManager entry, string omniTransactionID, string omniLineID, RecordIdentifier pictureID)
		{
			var statement = new SqlServerStatement("PURCHASEORDERLINE");

			statement.StatementType = StatementType.Update;
			statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
			statement.AddCondition("OMNITRANSACTIONID", omniTransactionID);
			statement.AddCondition("OMNILINEID", omniLineID);

			statement.AddField("PICTUREID", (string)pictureID);

			entry.Connection.ExecuteStatement(statement);

		}

		#region ISequenceable Members

		public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
		{
			return LineNumberExists(entry, id);
		}

		public RecordIdentifier SequenceID
		{
			get { return "PurchaseOrderLine"; }
		}

		public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
		{
			return GetExistingRecords(entry, "PURCHASEORDERLINE", "LINENUMBER", sequenceFormat, startingRecord, numberOfRecords);
		}

		#endregion
	}
}