using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.DataLayer.SqlDataProviders.ItemMaster;
using LSOne.DataLayer.SqlDataProviders.Transactions;
using LSOne.DataLayer.SqlDataProviders.Units;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
	public class InventoryTransactionData : SqlServerDataProviderBase, IInventoryTransactionData
	{
		private static List<TableColumn> inventoryTransactionColumns = new List<TableColumn>
		{
			new TableColumn {ColumnName = "GUID ", TableAlias = "A"},
			new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "IT"},
			new TableColumn {ColumnName = "ISNULL(A.POSTINGDATE,'01.01.1900')", ColumnAlias = "POSTINGDATE"},
			new TableColumn {ColumnName = "ISNULL(A.ITEMID,'')", ColumnAlias = "ITEMID"},
			new TableColumn {ColumnName = "ISNULL(A.STOREID,'')", ColumnAlias = "STOREID"},
			new TableColumn {ColumnName = "ISNULL(A.ADJUSTMENT,'0')", ColumnAlias = "ADJUSTMENT"},
			new TableColumn {ColumnName = "ISNULL(A.UNITID,'')", ColumnAlias = "UNITID"},
			new TableColumn {ColumnName = "ISNULL(A.TYPE,'0')", ColumnAlias = "TYPE"},
			new TableColumn {ColumnName = "ISNULL(A.OFFERID,'')", ColumnAlias = "OFFERID"},
			new TableColumn {ColumnName = "ISNULL(A.COSTPRICEPERITEM,'0')", ColumnAlias = "COSTPRICEPERITEM"},
			new TableColumn {ColumnName = "ISNULL(A.COMPATIBILITY,'INCOMPATIBLE')", ColumnAlias = "COMPATIBILITY"},
			new TableColumn
			{
				ColumnName = "ISNULL(A.SALESPRICEWITHOUTTAXPERITEM,'0')",
				ColumnAlias = "SALESPRICEWITHOUTTAXPERITEM"
			},
			new TableColumn
			{
				ColumnName = "ISNULL(A.SALESPRICEWITHTAXPERITEM,'0')",
				ColumnAlias = "SALESPRICEWITHTAXPERITEM"
			},
			new TableColumn {ColumnName = "ISNULL(A.REASONCODE,'')", ColumnAlias = "REASONCODE"},
			new TableColumn {ColumnName = "ISNULL(A.DISCOUNTAMOUNTPERITEM,'0')", ColumnAlias = "DISCOUNTAMOUNTPERITEM"},
			new TableColumn
			{
				ColumnName = "ISNULL(A.OFFERDISCOUNTAMOUNTPERITEM,'0')",
				ColumnAlias = "OFFERDISCOUNTAMOUNTPERITEM"
			},
			 new TableColumn {ColumnName = "ISNULL(A.REFERENCE,'')", ColumnAlias = "OFFERID"},
			 new TableColumn {ColumnName = "ISNULL(IT.ITEMTYPE, 0)", ColumnAlias = "ITEMTYPE"},
		};

		private static List<Join> listJoins = new List<Join>
		{
			new Join
			{
				Condition = " A.ITEMID = IT.ITEMID",
				JoinType = "LEFT OUTER",
				Table = "RETAILITEM",
				TableAlias = "IT"
			},
		};

		
		private static void PopulateInventoryTransaction(IDataReader dr, InventoryTransaction inventoryTransaction)
		{
			inventoryTransaction.Initialize((string)dr["ITEMID"], (ItemTypeEnum)((byte)dr["ITEMTYPE"]));
			inventoryTransaction.Guid = Guid.NewGuid();
			inventoryTransaction.PostingDate = (DateTime)dr["POSTINGDATE"];			
			inventoryTransaction.VariantName = (string)dr["VARIANTNAME"];
			inventoryTransaction.StoreID = (string)dr["STOREID"];
			inventoryTransaction.Type = (InventoryTypeEnum)dr["TYPE"];
			inventoryTransaction.OfferID = (string)dr["OFFERID"];
			inventoryTransaction.AdjustmentUnitID = (string)dr["UNITID"];
			inventoryTransaction.Adjustment = (decimal)dr["ADJUSTMENT"];
			inventoryTransaction.CostPricePerItem = (decimal)dr["PURCHASEPRICE"];
			inventoryTransaction.SalesPriceWithTaxPerItem = (decimal)dr["SALESPRICEWITHTAXPERITEM"];
			inventoryTransaction.SalesPriceWithoutTaxPerItem = (decimal)dr["SALESPRICEWITHOUTTAXPERITEM"];
			inventoryTransaction.DiscountAmountPerItem = (decimal)dr["DISCOUNTAMOUNTPERITEM"];
			inventoryTransaction.OfferDiscountAmountPerItem = (decimal)dr["OFFERDISCOUNTAMOUNTPERITEM"];
			inventoryTransaction.ReasonCode = (string)dr["REASONCODE"];
			inventoryTransaction.Reference = (string)dr["REFERENCE"];
			inventoryTransaction.Compatibility = (string)dr["COMPATIBILITY"];
		}

		public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
		{
			DeleteRecord(entry, "INVENTTRANS", "GUID", id, BusinessObjects.Permission.RunEndOfDay);
		}

		public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
		{
			return RecordExists(entry, "INVENTTRANS", "GUID", id);
		}

		public virtual InventoryTransaction Get(IConnectionManager entry, RecordIdentifier inventoryTransactionID)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.GUID = @GUID" });

				cmd.CommandText = string.Format(
				 QueryTemplates.BaseQuery("INVENTTRANS", "A"),
				 QueryPartGenerator.InternalColumnGenerator(inventoryTransactionColumns),
				 QueryPartGenerator.JoinGenerator(listJoins),
				 QueryPartGenerator.ConditionGenerator(conditions),
				 string.Empty
				 );


				MakeParam(cmd, "GUID", (Guid)inventoryTransactionID, SqlDbType.UniqueIdentifier);

				var result = Execute<InventoryTransaction>(entry, cmd, CommandType.Text, PopulateInventoryTransaction);
				return (result.Count > 0) ? result[0] : null;
			}
		}

		public virtual List<InventoryTransaction> GetList(IConnectionManager entry, InventoryTypeEnum inventoryType, string storeID)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.TYPE = @INVENTORYTYPE" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.STOREID = @STOREID" });

				cmd.CommandText = string.Format(
				 QueryTemplates.BaseQuery("INVENTTRANS", "A"),
				 QueryPartGenerator.InternalColumnGenerator(inventoryTransactionColumns),
				 QueryPartGenerator.JoinGenerator(listJoins),
				 QueryPartGenerator.ConditionGenerator(conditions),
				 string.Empty
				 );

				MakeParam(cmd, "INVENTORYTYPE", (int)inventoryType, SqlDbType.Int);
				MakeParam(cmd, "STOREID", storeID);

				return Execute<InventoryTransaction>(entry, cmd, CommandType.Text, PopulateInventoryTransaction);
			}
		}

		public List<InventoryTransaction> GetList(IConnectionManager entry, RecordIdentifier itemID,
			int rowFrom, int rowTo,
			string storeID, DateTime startDate, DateTime endDate)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.ITEMID = @ITEMID"});
				conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.POSTINGDATE >= @STARTDATE"});
				conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.POSTINGDATE <= @ENDDATE"});

				if (!string.IsNullOrEmpty(storeID))
				{
					conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.STOREID = @STOREID"});
					MakeParam(cmd, "STOREID", storeID);
				}

				cmd.CommandText = string.Format(
					QueryTemplates.BaseQuery("INVENTTRANS", "A"),
					QueryPartGenerator.InternalColumnGenerator(inventoryTransactionColumns),
					QueryPartGenerator.JoinGenerator(listJoins),
					QueryPartGenerator.ConditionGenerator(conditions),
					"ORDER BY(POSTINGDATE)"
					);




				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
				MakeParam(cmd, "ITEMID", (string) itemID);
				MakeParam(cmd, "STARTDATE", startDate < new DateTime(1900, 1, 1) ? new DateTime(1900, 1, 1) : startDate,
					SqlDbType.DateTime);
				MakeParam(cmd, "ENDDATE", endDate, SqlDbType.DateTime);

				return Execute<InventoryTransaction>(entry, cmd, CommandType.Text, PopulateInventoryTransaction);
			}
		}

		public virtual void Save(IConnectionManager entry, InventoryTransaction inventoryTransaction)
		{
			Save(entry, inventoryTransaction, true);
		}

		public virtual void Save(IConnectionManager entry, InventoryTransaction inventoryTransaction, bool shouldBeReplicated)
		{
			if (inventoryTransaction.ItemType == ItemTypeEnum.Service)
			{
				return;                
			}

			var statement = new SqlServerStatement("INVENTTRANS", shouldBeReplicated);
			
			// Add every permission to the array that gives the user access to this operation.
			ValidateSecurity(entry, new[] { Permission.PostStatement, Permission.EditInventoryAdjustments, Permission.ManageInventoryAdjustments, Permission.ManageInventoryAdjustmentsForAllStores });

			if (!Exists(entry, inventoryTransaction.ID))
			{
				statement.StatementType = StatementType.Insert;
				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddKey("GUID", (Guid)inventoryTransaction.Guid, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.StatementType = StatementType.Update;
				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("GUID", (Guid)inventoryTransaction.Guid, SqlDbType.UniqueIdentifier);
			}

			statement.AddField("POSTINGDATE", inventoryTransaction.PostingDate, SqlDbType.DateTime);
			statement.AddField("ITEMID", (string)inventoryTransaction.ItemID);
			statement.AddField("REFERENCE", inventoryTransaction.Reference);
			statement.AddField("STOREID", (string)inventoryTransaction.StoreID);
			statement.AddField("TYPE", (int)inventoryTransaction.Type, SqlDbType.Int);
			statement.AddField("OFFERID", inventoryTransaction.OfferID);
			statement.AddField("ADJUSTMENT", inventoryTransaction.Adjustment, SqlDbType.Decimal);
			statement.AddField("ADJUSTMENTININVENTORYUNIT", inventoryTransaction.AdjustmentInInventoryUnit, SqlDbType.Decimal);
			statement.AddField("COSTPRICEPERITEM", inventoryTransaction.CostPricePerItem, SqlDbType.Decimal);
			statement.AddField("SALESPRICEWITHTAXPERITEM", inventoryTransaction.SalesPriceWithTaxPerItem, SqlDbType.Decimal);
			statement.AddField("SALESPRICEWITHOUTTAXPERITEM", inventoryTransaction.SalesPriceWithoutTaxPerItem, SqlDbType.Decimal);
			statement.AddField("DISCOUNTAMOUNTPERITEM", inventoryTransaction.DiscountAmountPerItem, SqlDbType.Decimal);
			statement.AddField("OFFERDISCOUNTAMOUNTPERITEM", inventoryTransaction.OfferDiscountAmountPerItem, SqlDbType.Decimal);
			statement.AddField("REASONCODE", (string)inventoryTransaction.ReasonCode);
			statement.AddField("UNITID", (string)inventoryTransaction.AdjustmentUnitID);
			statement.AddField("COMPATIBILITY", inventoryTransaction.Compatibility);

			entry.Connection.ExecuteStatement(statement);
		}

		public virtual void PostStatementToInventory(IConnectionManager entry, RecordIdentifier statementID, RecordIdentifier storeID, DateTime postingDate)
		{
			var inventoryTransactions = CreateInventoryTransactionsFromStatement(entry, statementID, storeID, postingDate);

			foreach (var inventoryTransaction in inventoryTransactions)
			{
				Save(entry, inventoryTransaction);
			}
		}

		private static IEnumerable<InventoryTransaction> CreateInventoryTransactionsFromStatement(
			IConnectionManager entry, 
			RecordIdentifier statementID, 
			RecordIdentifier storeID, 
			DateTime postingDate)
		{
			ValidateSecurity(entry, BusinessObjects.Permission.PostStatement);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<TableColumn> columns = new List<TableColumn>
				{
					new TableColumn
					{
						ColumnName = "ITEMID",
						TableAlias = "B"
					},


					new TableColumn
					{
						ColumnName = "ISNULL(IT.VARIANTNAME,'')",
						ColumnAlias = "VARIANTNAME"
					},
					new TableColumn
					{
						ColumnName = "DISCOFFERID",
						ColumnAlias = "OFFERID",
						TableAlias = "B"
					},

					new TableColumn
					{
						ColumnName = "UNIT",
						ColumnAlias = "UNITID",
						TableAlias = "B"
					},

					new TableColumn
					{
						ColumnName = "NETAMOUNT/ B.QTY",
						ColumnAlias = "SALESPRICEWITHOUTTAXPERITEM",
						TableAlias = "B"
					},

					new TableColumn
					{
						ColumnName = "NETAMOUNTINCLTAX/ B.QTY",
						ColumnAlias = "SALESPRICEWITHTAXPERITEM",
						TableAlias = "B"
					},

					new TableColumn
					{
						ColumnName = "DISCAMOUNT/ B.QTY",
						ColumnAlias = "DISCOUNTAMOUNTPERITEM",
						TableAlias = "B"
					},
					new TableColumn
					{
						ColumnName = "PERIODICDISCAMOUNT/ B.QTY",
						ColumnAlias = "OFFERDISCOUNTAMOUNTPERITEM",
						TableAlias = "B"
					},
					new TableColumn
					{
						ColumnName = "'2016.1'",
						ColumnAlias = "COMPATIBILITY",
						
					},
					new TableColumn
					{
						ColumnName = "ISNULL(IT.ITEMTYPE, 0)",
						ColumnAlias = "ITEMTYPE"
					}
				};
				List<TableColumn> selectionColumns = new List<TableColumn>(columns);
				selectionColumns.Add(
					new TableColumn
					{

						ColumnName = @"B.QTY",
						ColumnAlias = "ADJUSTMENT",
						AggregateExternally = AggregationSetting.ExternalAggregation,
						AggregateFunction = "SUM"
					});

			   selectionColumns.Add(
				   new TableColumn
					{
						ColumnName = @"
	  
		ISNULL(
			CASE 
				WHEN B.UNIT = IT.SALESUNITID THEN 1
				WHEN CSUTSU.FACTOR IS NOT NULL THEN CSUTSU.FACTOR
				WHEN CSUTSUGEN.FACTOR IS NOT NULL THEN CSUTSUGEN.FACTOR
				ELSE NULL
			END , 
			1
		)
		*
		ISNULL(
			CASE 
				WHEN  IT.SALESUNITID  =  IT.INVENTORYUNITID THEN 1
				WHEN CSUTIU.FACTOR IS NOT NULL THEN CSUTIU.FACTOR
				WHEN CSUTIUGEN.FACTOR IS NOT NULL THEN CSUTIUGEN.FACTOR
				ELSE NULL
			END,
			1
		)
		*
		ISNULL(
			CASE 
				WHEN IT.INVENTORYUNITID = IT.PURCHASEUNITID THEN 1
				WHEN  CIUTPU.FACTOR IS NOT NULL THEN CIUTPU.FACTOR
				WHEN  CIUTPUGEN.FACTOR IS NOT NULL THEN CIUTPUGEN.FACTOR
			ELSE NULL
			END,
			1
		)
		*
		IT.PURCHASEPRICE",
						ColumnAlias = "PURCHASEPRICE",
						AggregateExternally = AggregationSetting.ExternalAggregation,
						AggregateFunction = "SUM"
					}
				);
			List<TableColumn> fixedColumns = new List<TableColumn>
				{

					new TableColumn {ColumnName = "@STOREID", ColumnAlias = "STOREID"},
					new TableColumn {ColumnName = "@POSTINGDATE", ColumnAlias = "POSTINGDATE"},
					new TableColumn {ColumnName = "0", ColumnAlias = "TYPE"},
					new TableColumn {ColumnName = "''", ColumnAlias = " REASONCODE"},
					new TableColumn {ColumnName = "@STATEMENTID", ColumnAlias = "REFERENCE"}
				};
				List< Join> internalJoins = new List<Join>
				{
					new Join {Table = "RBOTRANSACTIONTABLE", TableAlias = "A", Condition ="A.TRANSACTIONID = B.TRANSACTIONID and A.TERMINAL = B.TERMINALID" },
					new Join {Table = "RETAILITEM", TableAlias = "IT", Condition ="B.ITEMID = IT.ITEMID" },


					new Join {Table = @"
(
SELECT  [FROMUNIT]
	  ,[TOUNIT]
	  ,[FACTOR]
	  ,[ROUNDOFF]
	  ,[ITEMID]
	  ,[DATAAREAID]
	  ,[MARKUP]
  FROM [DBO].[UNITCONVERT]  
  UNION ALL 
  SELECT [TOUNIT]
	  ,[FROMUNIT]
	  ,1/[FACTOR]
	  ,[ROUNDOFF]
	  ,[ITEMID]
	  ,[DATAAREAID]
	  ,[MARKUP]
  FROM [DBO].[UNITCONVERT]
)",
						Condition ="(B.UNIT = CSUTSU.TOUNIT AND CSUTSU.FROMUNIT = IT.SALESUNITID ) AND  CSUTSU.ITEMID = B.ITEMID",
						TableAlias = "CSUTSU",
						JoinType = "LEFT"
					},
					new Join{Table = @"
(
SELECT  [FROMUNIT]
	  ,[TOUNIT]
	  ,[FACTOR]
	  ,[ROUNDOFF]
	  ,[ITEMID]
	  ,[DATAAREAID]
	  ,[MARKUP]
  FROM [DBO].[UNITCONVERT]
  UNION ALL 
  SELECT [TOUNIT]
	  ,[FROMUNIT]
	  ,1/[FACTOR]
	  ,[ROUNDOFF]
	  ,[ITEMID]
	  ,[DATAAREAID]
	  ,[MARKUP]
  FROM [DBO].[UNITCONVERT]
)",
						Condition ="(IT.SALESUNITID = CSUTIU.TOUNIT AND CSUTIU.FROMUNIT = IT.INVENTORYUNITID) AND  CSUTIU.ITEMID = B.ITEMID",
						TableAlias = "CSUTIU",
						JoinType = "LEFT"
					},
					new Join{Table = @"
(
SELECT  [FROMUNIT]
	  ,[TOUNIT] 
	  ,[FACTOR]
	  ,[ROUNDOFF]
	  ,[ITEMID]
	  ,[DATAAREAID]
	  ,[MARKUP]
  FROM [DBO].[UNITCONVERT] 
  UNION ALL 
  SELECT [TOUNIT]
	  ,[FROMUNIT]
	  ,1/[FACTOR]
	  ,[ROUNDOFF]
	  ,[ITEMID]
	  ,[DATAAREAID]
	  ,[MARKUP]
  FROM [DBO].[UNITCONVERT]
)",
						Condition = "(IT.INVENTORYUNITID = CIUTPU.TOUNIT AND CIUTPU.FROMUNIT = IT.PURCHASEUNITID) AND  CIUTPU.ITEMID = B.ITEMID",
						TableAlias = "CIUTPU",
						JoinType = "LEFT"
					},
					 new Join {Table = "GetConversions('')",
						Condition ="(B.UNIT = CSUTSUGEN.TOUNIT AND CSUTSUGEN.FROMUNIT = IT.SALESUNITID )",
						TableAlias = "CSUTSUGEN",
						JoinType = "LEFT"
					},
					new Join{Table = "GetConversions('')",
						Condition ="(IT.SALESUNITID = CSUTIUGEN.TOUNIT AND CSUTIUGEN.FROMUNIT = IT.INVENTORYUNITID)",
						TableAlias = "CSUTIUGEN",
						JoinType = "LEFT"
					},
					new Join{Table = "GetConversions('')",
						Condition = "(IT.INVENTORYUNITID = CIUTPUGEN.TOUNIT AND CIUTPUGEN.FROMUNIT = IT.PURCHASEUNITID)",
						TableAlias = "CIUTPUGEN",
						JoinType = "LEFT"
					},
					new Join
					{
						Table = "INVENTTRANSREASON",
						Condition = "B.REASONCODEID = rc.REASONID",
						TableAlias = "rc",
						JoinType = "LEFT"
					}

				};

				List<Condition> conditions = new List<Condition>
				{
					new Condition {ConditionValue = "a.ENTRYSTATUS = 0", Operator = "AND"},
					new Condition {ConditionValue = "b.TRANSACTIONSTATUS = 0", Operator = "AND"},
					new Condition {ConditionValue = "b.STORE = @storeID", Operator = "AND"},
					new Condition {ConditionValue = "b.STATEMENTID = @statementID", Operator = "AND"},
					new Condition {ConditionValue = "a.INVENTORYUPDATED = 0", Operator = "AND"},
					new Condition {ConditionValue = "it.ITEMTYPE <> 2", Operator = "AND"},
					new Condition{ConditionValue = "( rc.ACTION IS NULL OR rc.ACTION <> 1)", Operator = "AND"}
				};
				
				cmd.CommandText = string.Format(QueryTemplates.InternalQuery("RBOTRANSACTIONSALESTRANS", "B", "R"),
				  QueryPartGenerator.ExternalColumnGenerator(selectionColumns, "R")
					+","
					+QueryPartGenerator.InternalColumnGenerator(fixedColumns),
				  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
				  QueryPartGenerator.JoinGenerator(internalJoins),
				  QueryPartGenerator.ConditionGenerator(conditions),
				  string.Empty,
				  string.Empty,
				  string.Empty,
				  "GROUP BY " + QueryPartGenerator.GroupByColumnGenerator(columns,true));

				

				MakeParam(cmd, "STATEMENTID", (string)statementID);
				MakeParam(cmd, "STOREID", (string)storeID);
				MakeParam(cmd, "POSTINGDATE", postingDate, SqlDbType.DateTime);

				var result = Execute<InventoryTransaction>(entry, cmd, CommandType.Text, PopulateInventoryTransaction);
				UpdateInventoryTransactionQuantity(entry, result);
				return result;
			}
		}

		// Used when creating statements
		private static void UpdateInventoryTransactionQuantity(IConnectionManager entry, IEnumerable<InventoryTransaction> inventoryTransactions)
		{
			// For each Inventory transaction, get the inventory unit and conversion rate between the original unit and inventory unit.
			// Then update the adjustmentInInventoryUnit quantity based on this conversion rate
			foreach (var inventTrans in inventoryTransactions)
			{
				var inventoryUnit = Providers.RetailItemData.GetItemUnitID(entry, inventTrans.ItemID, RetailItem.UnitTypeEnum.Inventory);
				inventTrans.InventoryUnitID = inventoryUnit;

				// If the inventory and orignal unit are the same then no conversions are necessary
				if (inventoryUnit != inventTrans.AdjustmentUnitID)
				{
					var unitConversionID = new RecordIdentifier(inventTrans.ItemID, new RecordIdentifier(inventTrans.AdjustmentUnitID, inventoryUnit));
					var unitConversion = Providers.UnitConversionData.Get(entry, unitConversionID);

					//In case units have been set that don't have a conversion then use 1 - otherwise the statement will exit with an exception without explanation
					var conversionFactor = unitConversion != null ? unitConversion.Factor : 1;                                        
					inventTrans.AdjustmentInInventoryUnit = inventTrans.Adjustment/conversionFactor;
				}
				else
				{
					inventTrans.AdjustmentInInventoryUnit = inventTrans.Adjustment;
				}
			}
		}

		public virtual void UpdateInventoryFromTransaction(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID)
		{
			var inventoryTransactions = new List<InventoryTransaction>();

			inventoryTransactions = GetInventoryTransactionsFromTransaction(entry, transactionID, storeID, terminalID);

			foreach (var inventTrans in inventoryTransactions)
			{
				Save(entry, inventTrans);
			}
		}

		public virtual List<InventoryTransaction> GetInventoryTransactionsFromTransaction(IConnectionManager entry, RecordIdentifier transactionID, RecordIdentifier storeID, RecordIdentifier terminalID)
		{
			var itemsInTransaction = Providers.SalesTransactionData.GetRetailTransactionItems(entry, transactionID, storeID, terminalID);
			var inventoryTransactions = new List<InventoryTransaction>();

			foreach (var item in itemsInTransaction)
			{
				var inventTrans = new InventoryTransaction((string)item.ItemID, ItemTypeEnum.Item)
					{
						Guid = Guid.NewGuid(),
						PostingDate = DateTime.Now,						
						Adjustment = item.Quantity,
						StoreID = storeID,
						Type = InventoryTypeEnum.Sale,
						OfferID = "",
						ReasonCode = "",
						Reference = "",
						CostPricePerItem = item.Price,
						AdjustmentUnitID = item.Unit
					};

				inventoryTransactions.Add(inventTrans);
			}

			// Update the adjustment quantity based on salesUnit, InventoryUnit and conversion factor between
			UpdateInventoryTransactionQuantity(entry, inventoryTransactions);
			return inventoryTransactions;
		}
	}
}
