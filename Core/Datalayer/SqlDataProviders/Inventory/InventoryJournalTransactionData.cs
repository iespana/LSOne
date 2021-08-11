using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.Expressions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
	public class InventoryJournalTransactionData : SqlServerDataProviderBase, IInventoryJournalTransactionData
	{
		

		private static List<TableColumn> journalColumns = new List<TableColumn>
		{
			new TableColumn {ColumnName = "JOURNALID " , TableAlias = "A"},
			new TableColumn {ColumnName = "LINENUM" , TableAlias = "A"},
			new TableColumn {ColumnName = "TRANSDATE " , TableAlias = "A"},
			new TableColumn {ColumnName = "ITEMID " , TableAlias = "A"},
			new TableColumn {ColumnName = "ISNULL(A.ADJUSTMENT, 0) " , ColumnAlias = "ADJUSTMENT"},
			new TableColumn {ColumnName = "COSTPRICE " , TableAlias = "A"},
			new TableColumn {ColumnName = "PRICEUNIT" , TableAlias = "A"},
			new TableColumn {ColumnName = "COSTMARKUP " , TableAlias = "A"},
			new TableColumn {ColumnName = "COSTAMOUNT " , TableAlias = "A"},
			new TableColumn {ColumnName = "SALESAMOUNT " , TableAlias = "A"},
			new TableColumn {ColumnName = "INVENTONHAND " , TableAlias = "A"},
			new TableColumn {ColumnName = "COUNTED " , TableAlias = "A"},
			new TableColumn {ColumnName = "POSTED " , TableAlias = "A"},
			new TableColumn {ColumnName = "POSTEDDATETIME " , TableAlias = "A", },
			new TableColumn {ColumnName = "ISNULL(rbo.NAME, '')  " , ColumnAlias ="STORENAME"},
			new TableColumn {ColumnName = "REASONREFRECID " , TableAlias = "A"},
			new TableColumn {ColumnName = "ISNULL(itr.REASONTEXT,'') " , ColumnAlias = "REASONTEXT"},
			new TableColumn {ColumnName = "ISNULL(a.UNITID,'')  " , ColumnAlias = "UNITID"},
			new TableColumn {ColumnName = "ISNULL(u.TXT,'')  " , ColumnAlias = "UNITDESCRIPTION"},
			new TableColumn {ColumnName = "ISNULL(u.UNITDECIMALS,0) " , ColumnAlias = "UNITDECIMALSMAX"},
			new TableColumn {ColumnName = "ISNULL(u.MINUNITDECIMALS,0) " , ColumnAlias = "UNITDECIMALSMIN"},
			new TableColumn {ColumnName = "ISNULL(inventUnit.UNITID,'')" , ColumnAlias = "INVENTUNITID"},
			new TableColumn {ColumnName = "ISNULL(inventUnit.TXT,'')" , ColumnAlias = "INVENTUNITDESCRIPTION"},
			new TableColumn {ColumnName = "ISNULL(it.ITEMNAME,'') " , ColumnAlias = "ITEMNAME"},
			new TableColumn {ColumnName = "ISNULL(it.VARIANTNAME,'') " , ColumnAlias = "VARIANTNAME"},
			new TableColumn {ColumnName = "it.DELETED" , ColumnAlias = "ITEMDELTED"},
			new TableColumn {ColumnName = "it.ITEMTYPE" , ColumnAlias = "ITEMTYPE"},
			new TableColumn {ColumnName = "ISNULL(rg.NAME,'') " , ColumnAlias = "RETAILGROPNAME"},
			new TableColumn {ColumnName = "ISNULL(rd.NAME,'') " , ColumnAlias = "RETAILDEPARTMENTNAME"},
			new TableColumn {ColumnName = "STOREID" , TableAlias = "IJT"},
			new TableColumn {ColumnName = "MASTERID", TableAlias = "A" },
			new TableColumn {ColumnName = "PARENTMASTERID", TableAlias = "A" },
			new TableColumn {ColumnName = "ISNULL(A.STAFFID, '') ", ColumnAlias = "STAFFID"},
			new TableColumn {ColumnName = "AREA", TableAlias = "A"},
			new TableColumn {ColumnName = "ISNULL(A.LINESTATUS, 2) ", ColumnAlias = "LINESTATUS"},
			new TableColumn {ColumnName = "ISNULL(S.NAME, '') ", ColumnAlias = "STAFFNAME"},
			new TableColumn {ColumnName = "ISNULL(USR.LOGIN, '') ", ColumnAlias = "STAFFLOGIN"},
			new TableColumn {ColumnName = "ISNULL(AR.DESCRIPTION, '')", ColumnAlias = "AREANAME"},
			new TableColumn {ColumnName = "ISNULL(A.PICTUREID, '')", ColumnAlias = "PICTUREID"},
			new TableColumn {ColumnName = "ISNULL(A.OMNILINEID, '')", ColumnAlias = "OMNILINEID"},
			new TableColumn {ColumnName = "ISNULL(A.OMNITRANSACTIONID, '')", ColumnAlias = "OMNITRANSACTIONID"},

        };
		private static List<Join> listJoins = new List<Join>
		{
			new Join
			{
				Condition = "A.JOURNALID = IJT.JOURNALID",
				Table = "INVENTJOURNALTABLE",
				TableAlias = "IJT"
			},
			new Join
			{
				Condition = "A.REASONREFRECID = ITR.REASONID",
				JoinType = "LEFT OUTER",
				Table = "INVENTTRANSREASON",
				TableAlias = "ITR"
			},
			new Join
			{
				Condition = "IJT.STOREID = RBO.STOREID",
				JoinType = "LEFT OUTER",
				Table = "RBOSTORETABLE",
				TableAlias = "RBO"
			},
			new Join
			{
				Condition = "U.UNITID = A.UNITID",
				JoinType = "LEFT OUTER",
				Table = "UNIT",
				TableAlias = "U"
			},

			new Join
			{
				Condition = "IT.ITEMID = A.ITEMID",
				JoinType = "LEFT OUTER",
				Table = "RETAILITEM",
				TableAlias = "IT"
			},
			new Join
			{
				Condition = " IT.RETAILGROUPMASTERID = RG.MASTERID AND RG.DELETED =0",
				JoinType = "LEFT OUTER",
				Table = "RETAILGROUP",
				TableAlias = "RG"
			},
			new Join
			{
				Condition = " RG.DEPARTMENTMASTERID = RD.MASTERID AND RD.DELETED =0",
				JoinType = "LEFT OUTER",
				Table = "RETAILDEPARTMENT",
				TableAlias = "RD"
			},
			new Join
			{
				Condition = "INVENTUNIT.UNITID = IT.INVENTORYUNITID",
				JoinType = "LEFT OUTER",
				Table = "UNIT",
				TableAlias = "INVENTUNIT"
			},
			new Join
			{
				Condition = "ISUM.ITEMID = A.ITEMID AND ISUM.STOREID = IJT.STOREID",
				JoinType = "LEFT OUTER",
				Table = "VINVENTSUM",
				TableAlias = "ISUM"
			},
			new Join
			{
				Condition = "A.STAFFID = S.STAFFID",
				JoinType = "LEFT OUTER",
				Table = "RBOSTAFFTABLE",
				TableAlias = "S"
			},
			new Join
			{
				Condition = "A.STAFFID = USR.STAFFID",
				JoinType = "LEFT OUTER",
				Table = "USERS",
				TableAlias = "USR"
			},
			new Join
			{
				Condition = "A.AREA = AR.MASTERID",
				JoinType = "LEFT OUTER",
				Table = "INVENTORYAREALINES",
				TableAlias = "AR"
			},
		};



		private static string ResolveSort(InventoryJournalTransactionSorting sort, bool backwards, bool uselias = true)
		{
			string direction = backwards ? " DESC" : " ASC";

			switch (sort)
			{
				case InventoryJournalTransactionSorting.IdentificationNumber:
					return (uselias ? "A." : string.Empty) + "JOURNALID" + direction;
				case InventoryJournalTransactionSorting.LineNumber:
					return (uselias ? "A." : string.Empty) + "LINENUM" + direction;
				case InventoryJournalTransactionSorting.TransactionDate:
					return (uselias ? "A." : string.Empty) + "TRANSDATE" + direction;
				case InventoryJournalTransactionSorting.ItemId:
					return (uselias ? "A." : string.Empty) + "ITEMID" + direction;
				case InventoryJournalTransactionSorting.Quantity:
					return (uselias ? "A." : string.Empty) + "ADJUSTMENT" + direction;
				case InventoryJournalTransactionSorting.CostPrice:
					return (uselias ? "A." : string.Empty) + "COSTPRICE" + direction;
				case InventoryJournalTransactionSorting.PriceUnit:
					return (uselias ? "A." : string.Empty) + "PRICEUNIT" + direction;
				case InventoryJournalTransactionSorting.CostMarkup:
					return (uselias ? "A." : string.Empty) + "COSTMARKUP" + direction;
				case InventoryJournalTransactionSorting.CostAmount:
					return (uselias ? "A." : string.Empty) + "COSTAMOUNT" + direction;
				case InventoryJournalTransactionSorting.SalesAmount:
					return (uselias ? "A." : string.Empty) + "SALESAMOUNT" + direction;
				case InventoryJournalTransactionSorting.InventOnHand:
					return (uselias ? "A." : string.Empty) + "INVENTONHAND" + direction;
				case InventoryJournalTransactionSorting.Counted:
					return (uselias ? "A." : string.Empty) + "COUNTED" + direction;
				case InventoryJournalTransactionSorting.Reason:
					return "REASONTEXT" + direction;
				case InventoryJournalTransactionSorting.ItemName:
					return (uselias ? "IT." : string.Empty) + "ITEMNAME" + direction;
				case InventoryJournalTransactionSorting.UnitId:
					return (uselias ? "A." : string.Empty) + "UNITID" + direction;
				case InventoryJournalTransactionSorting.Posted:
					return (uselias ? "A." : string.Empty) + "POSTED" + direction;
				case InventoryJournalTransactionSorting.PostedDate:
					return (uselias ? "A." : string.Empty) + "POSTEDDATETIME" + direction;
				case InventoryJournalTransactionSorting.Variant:
					return "VARIANTNAME" + direction;
				case InventoryJournalTransactionSorting.CountedDifference:
					return (uselias ? "A." : string.Empty) + "COUNTED- " + (uselias ? "A." : string.Empty) + "INVENTONHAND" + direction;
				case InventoryJournalTransactionSorting.CountedDifferencePercantage:
					return "CASE WHEN " + (uselias ? "A." : string.Empty) + "INVENTONHAND = 0 THEN 100 ELSE (" + (uselias ? "A." : string.Empty) + "COUNTED/" + (uselias ? "A." : string.Empty) + "INVENTONHAND*100)-100 END" + direction;
				case InventoryJournalTransactionSorting.RetailGroup:
					return uselias ? "RG.NAME" + direction : "RETAILGROPNAME" + direction;
				case InventoryJournalTransactionSorting.RetailDepartment:
					return uselias ? "RD.NAME" + direction : "RETAILDEPARTMENTNAME" + direction;
				case InventoryJournalTransactionSorting.Staff:
					return uselias ? "S.NAME" + direction : "STAFFNAME" + direction;
				case InventoryJournalTransactionSorting.Area:
					return uselias ? "AR.DESCRIPTION" + direction : "AREANAME" + direction;
				case InventoryJournalTransactionSorting.Barcode:
					return uselias ? "ITB.ITEMBARCODE" + direction : "ITEMBARCODE" + direction;


			}
			return "";
		}

		protected virtual void PopulateJournalInfoWithCount(IConnectionManager entry, IDataReader dr, InventoryJournalTransaction item, ref int rowCount)
		{
			PopulateJournalInfo(dr, item);
			rowCount = (int)dr["Row_Count"];
		}

		private static void PopulateJournalInfo(IDataReader dr, InventoryJournalTransaction inventoryJournalTransactionInfo)
		{
			inventoryJournalTransactionInfo.JournalId = (string)dr["JOURNALID"];
			inventoryJournalTransactionInfo.LineNum = (string)dr["LINENUM"];
			inventoryJournalTransactionInfo.CostAmount = (decimal)dr["COSTAMOUNT"];
			inventoryJournalTransactionInfo.CostMarkup = (decimal)dr["COSTMARKUP"];
			inventoryJournalTransactionInfo.CostPrice = (decimal)dr["COSTPRICE"];
			inventoryJournalTransactionInfo.Counted = (decimal)dr["COUNTED"];
			inventoryJournalTransactionInfo.InventOnHandInInventoryUnits = (decimal)dr["INVENTONHAND"];
			inventoryJournalTransactionInfo.ItemId = (string)dr["ITEMID"];
			inventoryJournalTransactionInfo.StoreId = (string)dr["STOREID"];
			inventoryJournalTransactionInfo.PriceUnit = (decimal)dr["PRICEUNIT"];
			inventoryJournalTransactionInfo.Adjustment = (decimal)dr["ADJUSTMENT"];
			inventoryJournalTransactionInfo.ReasonId = (string)dr["REASONREFRECID"];
			inventoryJournalTransactionInfo.ReasonText = (string)dr["REASONTEXT"];
			inventoryJournalTransactionInfo.SalesAmount = (decimal)dr["SALESAMOUNT"];
			inventoryJournalTransactionInfo.TransDate = (DateTime)dr["TRANSDATE"];
			inventoryJournalTransactionInfo.Posted = Convert.ToBoolean(dr["POSTED"]);
			inventoryJournalTransactionInfo.Status = (InventoryJournalStatus)((int)dr["POSTED"]);
			inventoryJournalTransactionInfo.PostedDateTime = (DateTime)dr["POSTEDDATETIME"];
			inventoryJournalTransactionInfo.UnitID = (string)dr["UNITID"];
			inventoryJournalTransactionInfo.UnitDescription = (string)dr["UNITDESCRIPTION"];
			inventoryJournalTransactionInfo.ItemName = (string)dr["ITEMNAME"];
			inventoryJournalTransactionInfo.InventoryUnitID = (string)dr["INVENTUNITID"];
			inventoryJournalTransactionInfo.InventoryUnitDescription = (string)dr["INVENTUNITDESCRIPTION"];
			inventoryJournalTransactionInfo.VariantName = (string)dr["VARIANTNAME"];
			inventoryJournalTransactionInfo.RetailGroup = (string)dr["RETAILGROPNAME"];
			inventoryJournalTransactionInfo.RetailDepartment = (string)dr["RETAILDEPARTMENTNAME"];
			inventoryJournalTransactionInfo.MasterID = (dr["MASTERID"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["MASTERID"]);
			inventoryJournalTransactionInfo.ParentMasterID = (dr["PARENTMASTERID"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["PARENTMASTERID"]);
			inventoryJournalTransactionInfo.StaffID = (string)dr["STAFFID"];
			inventoryJournalTransactionInfo.AreaID = dr["AREA"] == DBNull.Value ? Guid.Empty : AsGuid(dr["AREA"]);
			inventoryJournalTransactionInfo.StaffName = (string)dr["STAFFNAME"];
			inventoryJournalTransactionInfo.StaffLogin = (string)dr["STAFFLOGIN"];
			inventoryJournalTransactionInfo.AreaName = (string)dr["AREANAME"];
			inventoryJournalTransactionInfo.LineStatus = (JournalTransStatusEnum)(int)dr["LINESTATUS"];
            inventoryJournalTransactionInfo.PictureID = (string)dr["PICTUREID"];
            inventoryJournalTransactionInfo.OmniLineID = (string)dr["OMNILINEID"];
            inventoryJournalTransactionInfo.OmniTransactionID = (string)dr["OMNITRANSACTIONID"];

            if (dr.GetSchemaTable().Columns.Contains("MOVEDQTY"))
			{
				inventoryJournalTransactionInfo.MovedQuantity = (decimal)dr["MOVEDQTY"];
			}

			if (dr["ITEMDELTED"] != null && dr["ITEMDELTED"] is bool)
			{
				inventoryJournalTransactionInfo.ItemDeleted = (bool)dr["ITEMDELTED"];
			}
			if (dr["ITEMTYPE"] != null && dr["ITEMTYPE"] is byte && Enum.IsDefined(typeof(ItemTypeEnum), (ItemTypeEnum)(byte)dr["ITEMTYPE"]))
			{
				inventoryJournalTransactionInfo.ItemType = (ItemTypeEnum)((byte)dr["ITEMTYPE"]);
			}

			var unitDecimalsMax = (int)dr["UNITDECIMALSMAX"];
			var unitDecimalsMin = (int)dr["UNITDECIMALSMIN"];
			inventoryJournalTransactionInfo.UnitQuantityLimiter = new DecimalLimit(unitDecimalsMin, unitDecimalsMax);
		}

		private static void PopulateReservedStockInfo(IDataReader dr, InventoryJournalTransaction inventoryJournalTransactionInfo)
		{
			inventoryJournalTransactionInfo.Adjustment = (decimal)dr["ADJUSTMENT"];
			inventoryJournalTransactionInfo.UnitID = (string)dr["UNITID"];
		}

		private static void PopulateJournalInfoAdvanced(IDataReader dr, InventoryJournalTransaction inventoryJournalTransactionInfo)
		{
			PopulateJournalInfo(dr, inventoryJournalTransactionInfo);
			inventoryJournalTransactionInfo.MovedQuantity = dr["MOVEDQTY"] == DBNull.Value ? 0 : (decimal)dr["MOVEDQTY"];
			inventoryJournalTransactionInfo.Barcode = dr["ITEMBARCODE"] == DBNull.Value ? "" : (string)dr["ITEMBARCODE"];
		}

		private static void PopulateJournalInfoAdvancedWithCount(IConnectionManager entry, IDataReader dr, InventoryJournalTransaction inventoryJournalTransactionInfo, ref int rowCount)
		{
			PopulateJournalInfoAdvanced(dr, inventoryJournalTransactionInfo);
			rowCount = (int)dr["Row_Count"];
		}

		/// <summary>
		/// Checks if a Inventory journal with a given ID exists in the database.
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="id">The ID of the inventory journal to check for</param>
		/// <returns>Whether a Inventory journal with a given ID exists in the database</returns>
		public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
		{
			return RecordExists(entry, "INVENTJOURNALTRANS", new[] { "JOURNALID", "LINENUM" }, id);
		}

		public virtual bool JournalHasUnPostedLines(IConnectionManager entry, RecordIdentifier id)
		{
			SqlCommand cmd = new SqlCommand(); 
			cmd.CommandText = @"select CAST(COUNT(*) AS BIT) FROM INVENTJOURNALTRANS WHERE(POSTED = 0) AND JOURNALID = @JOURNALID";
			MakeParam(cmd, "JOURNALID", (string)id);
			var hasUnpostedLines = entry.Connection.ExecuteScalar(cmd);
			return (bool)hasUnpostedLines;
		}

		private static bool LineNumExists(IConnectionManager entry, RecordIdentifier id)
		{
			return RecordExists(entry, "INVENTJOURNALTRANS", "LINENUM", id);
		}

		/// <summary>
		/// Deletes a inventory journal with a given ID
		/// </summary>
		/// <remarks>Requires the 'EditInventoryAdjustments' permission</remarks>
		/// <param name="entry">The entry into the database</param>
		/// <param name="id">The ID of the inventory journal to delete</param>
		public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
		{
			DeleteRecord(entry, "INVENTJOURNALTRANS", new[] { "JOURNALID", "LINENUM" }, id, Permission.EditInventoryAdjustments);
		}

		/// <summary>
		/// Deletes multiple inventory journal transaction lines
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="IDs">The IDs of the inventory journal to delete</param>
		public virtual void DeleteMultipleLines(IConnectionManager entry, List<RecordIdentifier> IDs)
		{
			ValidateSecurity(entry, Permission.EditInventoryAdjustments);

			if(IDs.Select(x => (string)x.PrimaryID).Distinct().Count() > 1)
			{
				throw new Exception("DeleteMultipleLines() only supports one journal at a time.");
			}

			if (!IDs.Any())
			{
				return;
			}

			using (var cmd = entry.Connection.CreateCommand())
			{
				string journalID = (string)IDs.First().PrimaryID;
				string lineNumsToDelete = string.Join(", ", IDs.Select(x => string.Format("'{0}'", x.SecondaryID)));
				cmd.CommandText = $"DELETE FROM INVENTJOURNALTRANS WHERE DATAAREAID = '{entry.Connection.DataAreaId}' AND JOURNALID = '{journalID}' AND LINENUM IN ({lineNumsToDelete})";
				entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
			}
		}

		/// <summary>
		/// Gets all InventJournalTrans lines for a certain InventJournalTable row
		/// </summary>
		/// <param name="entry">The database connection</param>
		/// <param name="journalTransactionId">A row from the header table.</param>
		/// <param name="sortBy">The column index to sort by</param>
		/// <param name="noExcludedInventoryItems">If true then items that have been deleted since the transaction was created or are service items are not included in the list</param>
		/// <param name="sortedBackwards">Whether to sort the result backwards or not</param>
		/// <returns></returns>
		public virtual List<InventoryJournalTransaction> GetJournalTransactionList(IConnectionManager entry, RecordIdentifier journalTransactionId, InventoryJournalTransactionSorting sortBy, bool sortedBackwards, bool noExcludedInventoryItems)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.JOURNALID = @JOURNALID" });
				if (noExcludedInventoryItems)
				{
					conditions.Add(new Condition {Operator = "AND", ConditionValue = "it.DELETED = 0" });
					conditions.Add(new Condition {Operator = "AND", ConditionValue = "it.ITEMTYPE != 2" });
				}
				cmd.CommandText = string.Format(
				 QueryTemplates.BaseQuery("INVENTJOURNALTRANS", "A"),
				 QueryPartGenerator.InternalColumnGenerator(journalColumns),
				 QueryPartGenerator.JoinGenerator(listJoins),
				 QueryPartGenerator.ConditionGenerator(conditions),
				 "ORDER BY " + ResolveSort(sortBy, sortedBackwards)
				 );

				MakeParam(cmd, "JOURNALID", (string)journalTransactionId.PrimaryID);

				return Execute<InventoryJournalTransaction>(entry, cmd, CommandType.Text, PopulateJournalInfo);
			}
		}

		public virtual List<InventoryJournalTransaction> GetPostedJournalTransactionsForTransaction(IConnectionManager entry, RecordIdentifier journalTransactionId, RecordIdentifier itemID, string variantName, RecordIdentifier storeID)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.JOURNALID = @JOURNALID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @ITEMID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "it.VARIANTNAME = @VARIANTNAME" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "ijt.STOREID = @STOREID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.POSTED = 1" });

				cmd.CommandText = string.Format(
					QueryTemplates.BaseQuery("INVENTJOURNALTRANS", "A"),
					QueryPartGenerator.InternalColumnGenerator(journalColumns),
					QueryPartGenerator.JoinGenerator(listJoins),
					QueryPartGenerator.ConditionGenerator(conditions),
					"ORDER BY a.TRANSDATE DESC "
				);

				MakeParam(cmd, "JOURNALID", (string)journalTransactionId.PrimaryID);
				MakeParam(cmd, "ITEMID", (string)itemID);
				MakeParam(cmd, "VARIANTNAME", variantName);
				MakeParam(cmd, "STOREID", (string)storeID);

				return Execute<InventoryJournalTransaction>(entry, cmd, CommandType.Text, PopulateJournalInfo);
			}
		}

		public virtual List<InventoryJournalTransaction> GetJournalTransactionList(IConnectionManager entry, RecordIdentifier journalTransactionId)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.JOURNALID = @JOURNALID" });

				cmd.CommandText = string.Format(
				 QueryTemplates.BaseQuery("INVENTJOURNALTRANS", "A"),
				 QueryPartGenerator.InternalColumnGenerator(journalColumns),
				 QueryPartGenerator.JoinGenerator(listJoins),
				 QueryPartGenerator.ConditionGenerator(conditions),
				 "ORDER BY a.TRANSDATE DESC "
				 );

				MakeParam(cmd, "JOURNALID", (string)journalTransactionId.PrimaryID);

				return Execute<InventoryJournalTransaction>(entry, cmd, CommandType.Text, PopulateJournalInfo);
			}
		}

		/// <summary>
		/// Returns the number of transaction lines in the given inventory journal.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="journalID">Inventory journal identifier.</param>
		/// <param name="countInventoryExcludedItems">If true, lines that are excluded from inventory (like deleted or service items) will also be counted.</param>
		/// <returns>The number of transaction lines in the inventory journal or null if an error is encountered.</returns>
		/// <remarks>This method is guaranteed to not throw any exceptions but instead it returns null.</remarks>
		public virtual int? Count(IConnectionManager entry, RecordIdentifier journalID, bool countInventoryExcludedItems = false)
		{
			ValidateSecurity(entry);

			if (RecordIdentifier.IsEmptyOrNull(journalID))
			{
				return 0;
			}

			try
			{
				if (countInventoryExcludedItems)
				{
					return Count(entry, "INVENTJOURNALTRANS", "JOURNALID", journalID.PrimaryID, true);
				}
				else
				{
					using (var cmd = entry.Connection.CreateCommand())
					{
						List<TableColumn> countColumns = new List<TableColumn>
						{
							new TableColumn {ColumnName = "COUNT(*) "}
						};
						List<Condition> conditions = new List<Condition>
						{
							new Condition {Operator = "AND", ConditionValue = "A.JOURNALID = @JOURNALID"},
							new Condition {Operator = "AND", ConditionValue = "IT.DELETED = 0"},
							new Condition {Operator = "AND", ConditionValue = "IT.ITEMTYPE != 2"}
						};

                        List<Join> joins = new List<Join>
                        {
                            new Join {JoinType = "LEFT", Table = "RETAILITEM", TableAlias = "IT", Condition = "A.ITEMID = IT.ITEMID"}
                        };

						cmd.CommandText = string.Format(
							QueryTemplates.BaseQuery("INVENTJOURNALTRANS", "A"),
							QueryPartGenerator.InternalColumnGenerator(countColumns),
							QueryPartGenerator.JoinGenerator(joins),
							QueryPartGenerator.ConditionGenerator(conditions),
							string.Empty //sort order
						);

						MakeParam(cmd, "JOURNALID", (string)journalID.PrimaryID);

						return (int)entry.Connection.ExecuteScalar(cmd);
					}
				}
			}
			catch
			{
				//TODO: Log error
				return null;
			}
		}

		private string GetComparisonOperator(DoubleValueOperator doubleOperator)
		{
			switch (doubleOperator)
			{
				case DoubleValueOperator.Equals:
					return "=";
				case DoubleValueOperator.GreaterThan:
					return ">";
				case DoubleValueOperator.LessThan:
					return "<";
				default:
					return string.Empty;
			}
		}

		public virtual List<InventoryJournalTransaction> SearchJournalTransactions(IConnectionManager entry, InventoryJournalTransactionFilter filter, out int totalRecordsMatching)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				List<TableColumn> columns = new List<TableColumn>(journalColumns);

				List<Condition> externalConditions = new List<Condition>();

				columns.Add(new TableColumn
				{
					ColumnName = $"ROW_NUMBER() OVER(ORDER BY { ResolveSort(filter.Sort, filter.SortBackwards)})",
					ColumnAlias = "ROW"
				});
				externalConditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = "CTE.ROW BETWEEN @ROWFROM AND @ROWTO"
				});

				MakeParam(cmd, "ROWFROM", filter.RowFrom, SqlDbType.Int);
				MakeParam(cmd, "ROWTO", filter.RowTo, SqlDbType.Int);

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.JOURNALID = @JOURNALID" });
				if (filter.IdOrDescriptions.Count == 1 && !string.IsNullOrEmpty(filter.IdOrDescriptions[0]))
				{
					string searchString = PreProcessSearchText(filter.IdOrDescriptions[0], true, filter.IdOrDescriptionStartsWith);
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue =
                            " (IT.ITEMNAME LIKE @DESCRIPTION OR IT.ITEMID LIKE @DESCRIPTION OR IT.VARIANTNAME LIKE @DESCRIPTION OR IT.NAMEALIAS LIKE @DESCRIPTION) "
                    });

					MakeParam(cmd, "DESCRIPTION", searchString);
				}
				else if (filter.IdOrDescriptions.Count > 1)
				{
					List<Condition> searchConditions = new List<Condition>();
					for (int index = 0; index < filter.IdOrDescriptions.Count; index++)
					{
						var searchToken = PreProcessSearchText(filter.IdOrDescriptions[index], true, filter.IdOrDescriptionStartsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							searchConditions.Add(new Condition
							{
								ConditionValue =
									$@" (IT.ITEMNAME LIKE @DESCRIPTION{index
										} 
										OR IT.ITEMID LIKE @DESCRIPTION{index
										} 
										OR IT.VARIANTNAME LIKE @DESCRIPTION{
										index
										} 
										OR IT.NAMEALIAS LIKE @DESCRIPTION{
										index}) ",
								Operator = "AND"

							});

							MakeParam(cmd, $"DESCRIPTION{index}", searchToken);
						}
					}
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
					});
				}

				if (filter.Variants.Count == 1 && !string.IsNullOrEmpty(filter.Variants[0]))
				{
					string searchString = PreProcessSearchText(filter.Variants[0], true, filter.VariantStartsWith);
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = " IT.VARIANTNAME LIKE @VARIANT "
                    });

					MakeParam(cmd, "VARIANT", searchString);
				}
				else if (filter.Variants.Count > 1)
				{
					List<Condition> searchConditions = new List<Condition>();
					for (int index = 0; index < filter.Variants.Count; index++)
					{
						var searchToken = PreProcessSearchText(filter.Variants[index], true, filter.VariantStartsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							searchConditions.Add(new Condition
							{
								ConditionValue = $" IT.VARIANTNAME LIKE @VARIANT{index} ",
								Operator = "AND"
							});

							MakeParam(cmd, $"VARIANT{index}", searchToken);
						}
					}
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
					});
				}

				if (filter.RetailGroupID != null)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "IT.RETAILGROUPMASTERID = @RETAILGROUP " });

					MakeParam(cmd, "RETAILGROUP", (Guid)filter.RetailGroupID, SqlDbType.UniqueIdentifier);
				}

				if (filter.RetailDepartmentID != null)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "RG.DEPARTMENTMASTERID = @RETAILDEPARTMENT "
					});

					MakeParam(cmd, "RETAILDEPARTMENT", (Guid)filter.RetailDepartmentID, SqlDbType.UniqueIdentifier);
				}

				if (filter.StaffID != null)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "USR.LOGIN = @STAFFLOGIN "
					});
					MakeParam(cmd, "STAFFLOGIN", filter.StaffID);
				}

				if (filter.AreaID != null)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "A.AREA = @AREA "
					});
					MakeParam(cmd, "AREA", (Guid)filter.AreaID, SqlDbType.UniqueIdentifier);
				}

				if (filter.CountedSet)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.COUNTED {GetComparisonOperator(filter.CountedComparison) }  @COUNTED "
					});

					MakeParam(cmd, "COUNTED", filter.Counted, SqlDbType.Decimal);
				}

				if (filter.InventoryOnHandSet)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.INVENTONHAND {GetComparisonOperator(filter.InventoryOnHandComparison) }  @INVENTORYONHAND "
					});

					MakeParam(cmd, "INVENTORYONHAND", filter.InventoryOnHand, SqlDbType.Decimal);
				}

				if (filter.DifferenceSet)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.COUNTED - A.INVENTONHAND {GetComparisonOperator(filter.DifferenceComparison) }  @DIFFERENCE "
					});

					MakeParam(cmd, "DIFFERENCE", filter.Difference, SqlDbType.Decimal);
				}

				if (filter.DifferencePercentageSet)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = string.Format(@"CASE WHEN a.INVENTONHAND = 0 THEN 100
														 ELSE (a.COUNTED/a.INVENTONHAND *100) -100
														 END {0}  @DIFFERENCEPERCENTAGE ", GetComparisonOperator(filter.DifferencePercentageComparison))
					});

					MakeParam(cmd, "DIFFERENCEPERCENTAGE", filter.DifferencePercentage, SqlDbType.Decimal);
				}

				if (filter.PostedSet)
				{
					if (filter.Posted)
					{
						conditions.Add(new Condition
						{
							Operator = "AND",
							ConditionValue = $"A.POSTED = 1 "
						});
					}
					else
					{
						conditions.Add(new Condition
						{
							Operator = "AND",
							ConditionValue = $"A.POSTED = 0 "
						});
					}
				}

				if (filter.DateFrom != Date.Empty && filter.DateTo != Date.Empty)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.POSTEDDATETIME BETWEEN @DATEFROM AND @DATETO "
					});

					MakeParam(cmd, "DATEFROM", filter.DateFrom.DateTime, SqlDbType.DateTime);
					MakeParam(cmd, "DATETO", filter.DateTo.DateTime, SqlDbType.DateTime);
				}
				else if (filter.DateTo != Date.Empty)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.POSTEDDATETIME < @DATETO "
					});

					MakeParam(cmd, "DATETO", filter.DateTo.DateTime, SqlDbType.DateTime);
				}
				else if (filter.DateFrom != Date.Empty)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.POSTEDDATETIME > @DATEFROM "
					});

					MakeParam(cmd, "DATEFROM", filter.DateFrom.DateTime, SqlDbType.DateTime);
				}

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DATAAREAID = @DATAAREAID" });
				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

				var baseQuery = string.Format(QueryTemplates.BaseQuery("INVENTJOURNALTRANS", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(listJoins),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);

				cmd.CommandText = $";WITH CTE AS ({baseQuery}) " +
					$"SELECT {QueryPartGenerator.ExternalColumnGenerator(columns, "CTE")}, ROW_COUNT = tCountTransactions.ROW_COUNT " +
					$"FROM CTE CROSS JOIN (SELECT Count(*) AS ROW_COUNT FROM CTE) AS tCountTransactions " +
					$"{QueryPartGenerator.ConditionGenerator(externalConditions)}" +
					$"ORDER BY {ResolveSort(filter.Sort, filter.SortBackwards, false)}";

				totalRecordsMatching = 0;
				MakeParam(cmd, "JOURNALID", (string)filter.JournalTransactionID.PrimaryID);

				List<InventoryJournalTransaction> stockCounting = Execute<InventoryJournalTransaction, int>(entry, cmd, CommandType.Text,
								ref totalRecordsMatching, PopulateJournalInfoWithCount);

				return stockCounting;
			}
		}

		/// <summary>
		/// Updates inventory on hand for all items in a stock counting journal
		/// </summary>
		/// <param name="entry">The database connection</param>
		/// <param name="journalTransactionId">A row from the header table.</param>
		public virtual void RefreshStockCountingJournalInventoryOnHand(IConnectionManager entry, RecordIdentifier journalTransactionId)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"UPDATE
										IJTR
									SET
										IJTR.INVENTONHAND = ISNULL(INS.QUANTITY, 0)
									FROM
										INVENTJOURNALTRANS IJTR
										JOIN INVENTJOURNALTABLE IJT ON IJT.JOURNALID = IJTR.JOURNALID
										LEFT JOIN VINVENTSUM INS ON INS.ITEMID = IJTR.ITEMID AND INS.STOREID = IJT.STOREID
										WHERE IJTR.JOURNALID = @JOURNALID";

				MakeParam(cmd, "JOURNALID", (string)journalTransactionId.PrimaryID);

				entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
			}
		}

		/// <summary>
		/// Saves a given Journal entry (row) into the database
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="inventoryJournalTrans">The Inventory journal to save</param>
		public virtual void Save(IConnectionManager entry, InventoryJournalTransaction inventoryJournalTrans)
		{
			var statement = new SqlServerStatement("INVENTJOURNALTRANS", false);

			ValidateSecurity(entry, BusinessObjects.Permission.EditInventoryAdjustments);

			if (inventoryJournalTrans.LineNum == RecordIdentifier.Empty)
			{
				inventoryJournalTrans.LineNum = DataProviderFactory.Instance.GenerateNumber<IInventoryJournalTransactionData, InventoryJournalTransaction>(entry);
			}

			if (!Exists(entry, inventoryJournalTrans.ID))
			{
				statement.StatementType = StatementType.Insert;
				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddKey("JOURNALID", (string)inventoryJournalTrans.JournalId);
				statement.AddKey("LINENUM", (string)inventoryJournalTrans.LineNum);

				if (RecordIdentifier.IsEmptyOrNull(inventoryJournalTrans.MasterID))
				{
					inventoryJournalTrans.MasterID = Guid.NewGuid();
				}

				statement.AddField("MASTERID", (Guid)inventoryJournalTrans.MasterID, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.StatementType = StatementType.Update;
				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("JOURNALID", (string)inventoryJournalTrans.JournalId);
				statement.AddCondition("LINENUM", (string)inventoryJournalTrans.LineNum);
				if (inventoryJournalTrans.MasterID == null || RecordIdentifier.IsEmptyOrNull(inventoryJournalTrans.MasterID))
				{
					inventoryJournalTrans.MasterID = Guid.NewGuid();
					statement.AddField("MASTERID", (Guid)inventoryJournalTrans.MasterID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					statement.AddCondition("MASTERID", (Guid)inventoryJournalTrans.MasterID, SqlDbType.UniqueIdentifier);
				}
			}

			//Until Posted is removed, we need to take into account both fields.
			int posted = inventoryJournalTrans.Status == InventoryJournalStatus.PartialPosted
							? (int)inventoryJournalTrans.Status
							: (inventoryJournalTrans.Posted ? 1 : 0);
			statement.AddField("POSTED", posted, SqlDbType.Int);
			statement.AddField("POSTEDDATETIME", DateTime.Now, SqlDbType.DateTime);
			statement.AddField("TRANSDATE", inventoryJournalTrans.TransDate, SqlDbType.DateTime);
			statement.AddField("ITEMID", (string)inventoryJournalTrans.ItemId);
			statement.AddField("COSTPRICE", inventoryJournalTrans.CostPrice, SqlDbType.Decimal);
			statement.AddField("PRICEUNIT", inventoryJournalTrans.PriceUnit, SqlDbType.Decimal);
			statement.AddField("COSTMARKUP", inventoryJournalTrans.CostMarkup, SqlDbType.Decimal);
			statement.AddField("COSTAMOUNT", inventoryJournalTrans.CostAmount, SqlDbType.Decimal);
			statement.AddField("SALESAMOUNT", inventoryJournalTrans.SalesAmount, SqlDbType.Decimal);

			statement.AddField("INVENTONHAND", inventoryJournalTrans.InventOnHandInInventoryUnits, SqlDbType.Decimal);
			statement.AddField("COUNTED", inventoryJournalTrans.Counted, SqlDbType.Decimal);
			statement.AddField("ADJUSTMENT", inventoryJournalTrans.Adjustment, SqlDbType.Decimal);

			statement.AddField("REASONREFRECID", (string)inventoryJournalTrans.ReasonId);
			statement.AddField("UNITID", (string)inventoryJournalTrans.UnitID);
			statement.AddField("STAFFID", (string)inventoryJournalTrans.StaffID);
			statement.AddField("LINESTATUS", (int)inventoryJournalTrans.LineStatus, SqlDbType.Int);
            statement.AddField("PICTUREID", (string)inventoryJournalTrans.PictureID);
            statement.AddField("OMNILINEID", inventoryJournalTrans.OmniLineID);
            statement.AddField("OMNITRANSACTIONID", inventoryJournalTrans.OmniTransactionID);

            if (inventoryJournalTrans.AreaID == Guid.Empty)
			{
				statement.AddField("AREA", DBNull.Value, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.AddField("AREA", inventoryJournalTrans.AreaID, SqlDbType.UniqueIdentifier);
			}

			if (RecordIdentifier.IsEmptyOrNull(inventoryJournalTrans.ParentMasterID))
			{
				statement.AddField("PARENTMASTERID", DBNull.Value, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.AddField("PARENTMASTERID", (Guid)inventoryJournalTrans.ParentMasterID, SqlDbType.UniqueIdentifier);
			}

			entry.Connection.ExecuteStatement(statement);
		}

		/// <summary>
		/// Copies lines from one stock counting journal to another
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="fromStockCountingJournalID">Stock counting journal id from which to copy the lines.</param>
		/// <param name="toStockCountingJournalID">Stock counting journal id to which the lines should be copied</param>
		/// <param name="storeID">Store ID used by the destination journal</param>
		public virtual void CopyStockCountingJournalLines(IConnectionManager entry, RecordIdentifier fromStockCountingJournalID, RecordIdentifier toStockCountingJournalID, RecordIdentifier storeID)
		{
			using (SqlCommand cmd = new SqlCommand("spINVENTORY_CopyStockCountingLines"))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				MakeParam(cmd, "FROMADJUSTMENTID", fromStockCountingJournalID);
				MakeParam(cmd, "TOADJUSTMENTID", toStockCountingJournalID);
				MakeParam(cmd, "STOREID", storeID);
				MakeParam(cmd, "STAFFID", entry.CurrentUser.StaffID);
				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

				entry.Connection.ExecuteNonQuery(cmd, false);
			}
		}

		/// <summary>
		/// Creates stock counting lines based on a filter
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="stockCountingJournalID">Stock counting journal id for which to create the lines.</param>
		/// <param name="storeID">Store ID used by the destination journal</param>
		/// <param name="filter">Filter container</param>
		public virtual void CreateStockCountingJournalLinesFromFilter(IConnectionManager entry, RecordIdentifier stockCountingJournalID,  RecordIdentifier storeID, InventoryTemplateFilterContainer filter)
		{
			using (SqlCommand cmd = new SqlCommand("spINVENTORY_CreateStockCountingLinesFromFilter"))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				string filterDelimiter = ";";

				MakeParam(cmd, "ADJUSTMENTID", stockCountingJournalID);
				MakeParam(cmd, "STOREID", storeID);
				MakeParam(cmd, "STAFFID", entry.CurrentUser.StaffID);
				MakeParam(cmd, "RETAILGROUPS", filter.RetailGroups.Count == 0 ? "" : filter.RetailGroups.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
				MakeParam(cmd, "RETAILDEPARTMENTS", filter.RetailDepartments.Count == 0 ? "" : filter.RetailDepartments.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
				MakeParam(cmd, "VENDORS", filter.Vendors.Count == 0 ? "" : filter.Vendors.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y));
				MakeParam(cmd, "SPECIALGROUPS", filter.SpecialGroups.Count == 0 ? "" : filter.SpecialGroups.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
				MakeParam(cmd, "FILTERDELIMITER", filterDelimiter, SqlDbType.VarChar);
				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
				MakeParam(cmd, "FILTERBYINVENTONHAND", filter.FilterByInventoryOnHand, SqlDbType.Bit);
				MakeParam(cmd, "INVENTORYONHAND", filter.InventoryOnHand, SqlDbType.Int);
				MakeParam(cmd, "INVENTORYONHANDCOMPARISON", (int)filter.InventoryOnHandComparison, SqlDbType.Int);

				entry.Connection.ExecuteNonQuery(cmd, false);
			}
		}

		/// <summary>
		/// Post all lines from a stock counting journal
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
		/// <returns>Result of the operation</returns>
		public virtual PostStockCountingResult PostAllStockCountingLines(IConnectionManager entry, RecordIdentifier stockCountingJournalID)
		{
			//Create a separate connection to run this because it can take a long time
			IConnectionManagerTemporary postLinesEntry = null;
			try
			{
				postLinesEntry = entry.CreateTemporaryConnection();

				using (SqlCommand cmd = new SqlCommand("spINVENTORY_PostAllStockCountingLines"))
				{
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.CommandTimeout = 3600; //1 hour timeout, this operation can take a long time

					MakeParam(cmd, "JOURNALID", stockCountingJournalID.StringValue);
					MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
					SqlParameter postingResult = MakeParam(cmd, "POSTINGRESULT", "", SqlDbType.Int, ParameterDirection.Output, 4);

					postLinesEntry.Connection.ExecuteNonQuery(cmd, false);

					return (PostStockCountingResult)((int)postingResult.Value);
				}
			}
			catch(Exception x)
			{
				entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
				throw x;
			}
			finally
			{
				postLinesEntry?.Close();
			}
		}

		/// <summary>
		/// Imports stock counting lines from an xml file
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="xmlData">XML data to import</param>
		/// <param name="languageCode">Language code used for parsing the xml (ex. en-US). This is required because in some languages (ex. Icelandic) the default decimal separator is a comma and xml interprets it as a string instead of a numeric</param>
		public virtual int ImportStockCountingLinesFromXML(IConnectionManager entry, string xmlData, string languageCode)
		{
			using (SqlCommand cmd = new SqlCommand("spINVENTORY_ImportStockCountingLinesFromXML"))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandTimeout = 1200;

				MakeParam(cmd, "XMLDATA", xmlData, SqlDbType.Xml);
				MakeParam(cmd, "STAFFID", entry.CurrentUser.StaffID);
				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
				MakeParam(cmd, "LANGUAGECODE", languageCode);
				SqlParameter insertedRows = MakeParam(cmd, "INSERTEDRECORDS", "", SqlDbType.Int, ParameterDirection.Output, 4);

				entry.Connection.ExecuteNonQuery(cmd, false);

				return (int)insertedRows.Value;
			}
		}

		protected virtual SqlServerStatement PrepareStatement()
		{
			SqlServerStatement statement = new SqlServerStatement("INVENTJOURNALTRANS", StatementType.Insert, false);

			statement.AddKey("DATAAREAID", "");
			statement.AddKey("JOURNALID", "");
			statement.AddKey("LINENUM", "");

			statement.AddField("MASTERID", Guid.Empty, SqlDbType.UniqueIdentifier);
			statement.AddField("POSTED", 0, SqlDbType.Int);
			statement.AddField("POSTEDDATETIME", DateTime.Now, SqlDbType.DateTime);
			statement.AddField("TRANSDATE", DateTime.Now, SqlDbType.DateTime);
			statement.AddField("ITEMID", "");
			statement.AddField("COSTPRICE", decimal.Zero, SqlDbType.Decimal);
			statement.AddField("PRICEUNIT", decimal.Zero, SqlDbType.Decimal);
			statement.AddField("COSTMARKUP", decimal.Zero, SqlDbType.Decimal);
			statement.AddField("COSTAMOUNT", decimal.Zero, SqlDbType.Decimal);
			statement.AddField("SALESAMOUNT", decimal.Zero, SqlDbType.Decimal);

			statement.AddField("INVENTONHAND", decimal.Zero, SqlDbType.Decimal);
			statement.AddField("COUNTED", decimal.Zero, SqlDbType.Decimal);
			statement.AddField("ADJUSTMENT", decimal.Zero, SqlDbType.Decimal);

			statement.AddField("REASONREFRECID", "");
			statement.AddField("UNITID", "");
			statement.AddField("STAFFID", "");
			statement.AddField("LINESTATUS", 0, SqlDbType.Int);

			statement.AddField("AREA", DBNull.Value, SqlDbType.UniqueIdentifier);
			statement.AddField("PARENTMASTERID", DBNull.Value, SqlDbType.UniqueIdentifier);                

			return statement;
		}
		
		protected virtual SqlServerStatement Save(IConnectionManagerTransaction entryTransaction, InventoryJournalTransaction journalLine, SqlServerStatement statement)
		{
			if (statement.StatementType == StatementType.Insert)
			{
				statement.UpdateField("DATAAREAID", entryTransaction.Connection.DataAreaId);
				statement.UpdateField("JOURNALID", (string)journalLine.JournalId);
				statement.UpdateField("LINENUM", (string)journalLine.LineNum);
			}
			else
			{
				statement.UpdateCondition(0, "DATAAREAID", entryTransaction.Connection.DataAreaId);
				statement.UpdateCondition(1, "JOURNALID", (string)journalLine.JournalId);
				statement.UpdateCondition(2, "LINENUM", (string)journalLine.LineNum);
			}

			if (RecordIdentifier.IsEmptyOrNull(journalLine.MasterID))
			{
				journalLine.MasterID = Guid.NewGuid();
			}

			statement.UpdateField("MASTERID", (Guid)journalLine.MasterID);

			int posted = journalLine.Status == InventoryJournalStatus.PartialPosted
								? (int)journalLine.Status
								: (journalLine.Posted ? 1 : 0);

			statement.UpdateField("POSTED", posted);
			statement.UpdateField("POSTEDDATETIME", DateTime.Now);
			statement.UpdateField("TRANSDATE", journalLine.TransDate);
			statement.UpdateField("ITEMID", (string)journalLine.ItemId);
			statement.UpdateField("COSTPRICE", journalLine.CostPrice);
			statement.UpdateField("PRICEUNIT", journalLine.PriceUnit);
			statement.UpdateField("COSTMARKUP", journalLine.CostMarkup);
			statement.UpdateField("COSTAMOUNT", journalLine.CostAmount);
			statement.UpdateField("SALESAMOUNT", journalLine.SalesAmount);

			statement.UpdateField("INVENTONHAND", journalLine.InventOnHandInInventoryUnits);
			statement.UpdateField("COUNTED", journalLine.Counted);
			statement.UpdateField("ADJUSTMENT", journalLine.Adjustment);

			statement.UpdateField("REASONREFRECID", (string)journalLine.ReasonId);
			statement.UpdateField("UNITID", (string)journalLine.UnitID);
			statement.UpdateField("STAFFID", (string)journalLine.StaffID);
			statement.UpdateField("LINESTATUS", (int)journalLine.LineStatus);

			if (journalLine.AreaID == Guid.Empty)
			{
				statement.UpdateField("AREA", DBNull.Value);
			}
			else
			{
				statement.UpdateField("AREA", journalLine.AreaID);
			}

			if (RecordIdentifier.IsEmptyOrNull(journalLine.ParentMasterID))
			{
				statement.UpdateField("PARENTMASTERID", DBNull.Value);
			}
			else
			{
				statement.UpdateField("PARENTMASTERID", (Guid)journalLine.ParentMasterID);
			}

			entryTransaction.Connection.ExecuteStatement(statement);

			return statement;
		}

		public virtual void SaveMultipleLines(IConnectionManager entry, List<InventoryJournalTransaction> inventoryJournalTransactions)
		{
			ValidateSecurity(entry, Permission.EditInventoryAdjustments);

			SqlServerStatement statement = PrepareStatement();

			IConnectionManagerTransaction transaction = entry.CreateTransaction(IsolationLevel.ReadUncommitted);
			
			try
			{
				List<RecordIdentifier> lineNums = DataProviderFactory.Instance.GenerateNumbers<IInventoryJournalTransactionData, InventoryJournalTransaction>(transaction, inventoryJournalTransactions.Count(x => x.LineNum == RecordIdentifier.Empty));
				int lineNumIndex = 0;

				foreach (InventoryJournalTransaction journalLine in inventoryJournalTransactions)
				{
					if (journalLine.LineNum == RecordIdentifier.Empty)
					{
						journalLine.LineNum = lineNums[lineNumIndex++];
						statement.ChangeStatementType(StatementType.Insert);
					}
					else
					{
						statement.ChangeStatementType(StatementType.Update);
					}

					Save(transaction, journalLine, statement);
				}

				transaction.Commit();
			}
			catch
			{
				transaction.Rollback();
				throw new Exception("Error saving inventory journal transaction lines!");
			}
			finally
			{
				statement = null;
			}
		}

		public virtual void InsertMultipleLinesWithID(IConnectionManager entry, List<InventoryJournalTransaction> inventoryJournalTransactions)
		{
			ValidateSecurity(entry, Permission.EditInventoryAdjustments);

			SqlServerStatement statement = PrepareStatement();

			IConnectionManagerTransaction transaction = entry.CreateTransaction(IsolationLevel.ReadUncommitted);

			try
			{
				inventoryJournalTransactions.ForEach(x => Save(transaction, x, statement));
				transaction.Commit();
			}
			catch
			{
				transaction.Rollback();
				throw new Exception("Error saving inventory journal transaction lines!");
			}
			finally
			{
				statement = null;
			}
		}

		/// <summary>
		/// Gets a Journal entry with a given ID
		/// </summary>
		/// <param name="entry">The entry connection into the database</param>
		/// <param name="journalTransactionId">The ID of the Journal row to fetch</param>
		/// <returns>A Journal row with a given ID</returns>
		public virtual InventoryJournalTransaction Get(IConnectionManager entry, RecordIdentifier journalTransactionId)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.JOURNALID = @JOURNALID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "LINENUM = @LINENUM" });

				cmd.CommandText = string.Format(
				 QueryTemplates.BaseQuery("INVENTJOURNALTRANS", "A"),
				 QueryPartGenerator.InternalColumnGenerator(journalColumns),
				 QueryPartGenerator.JoinGenerator(listJoins),
				 QueryPartGenerator.ConditionGenerator(conditions),
				 "ORDER BY a.TRANSDATE DESC "
				 );

				MakeParam(cmd, "JOURNALID", (string)journalTransactionId[0]);
				MakeParam(cmd, "LINENUM", (string)journalTransactionId[1]);

				var result = Execute<InventoryJournalTransaction>(entry, cmd, CommandType.Text, PopulateJournalInfo);
				return (result.Count > 0) ? result[0] : null;
			}
		}

		public virtual decimal GetSumOfReservedItemByStore(IConnectionManager entry,
			RecordIdentifier itemID,
			RecordIdentifier storeID,
			RecordIdentifier inventoryUnitID,
			InventoryJournalTypeEnum journalType
			)
		{
			ValidateSecurity(entry);

			List<InventoryJournalTransaction> lines;
			decimal reserved = 0;

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT T.ADJUSTMENT, T.UNITID
									from INVENTJOURNALTRANS t
									join INVENTJOURNALTABLE j on j.JOURNALID = t.JOURNALID AND J.DATAAREAID = T.DATAAREAID
									where J.JOURNALTYPE = @JOURNALTYPE
									AND J.POSTED = @POSTED
									AND T.ITEMID = @ITEMID
									AND J.STOREID = @STOREID
									";

				MakeParam(cmd, "JOURNALTYPE", (int)journalType, SqlDbType.Int);
				MakeParam(cmd, "POSTED", (int)InventoryJournalStatus.Active, SqlDbType.Int);
				MakeParam(cmd, "ITEMID", (string)itemID, SqlDbType.NVarChar);
				MakeParam(cmd, "STOREID", (string)storeID, SqlDbType.NVarChar);

				lines = Execute<InventoryJournalTransaction>(entry, cmd, CommandType.Text, PopulateReservedStockInfo);
			}

			if (lines != null && lines.Count > 0)
			{
				reserved += lines.Sum(line => Providers.UnitConversionData.ConvertQtyBetweenUnits(entry, itemID, line.UnitID, inventoryUnitID, line.Adjustment));
			}
			return reserved;
		}

		/// <summary>
		/// Paginated search through inventory journal lines based on the given <see cref="InventoryJournalLineSearch">search criteria object</see>
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="searchCriteria">Object containing search criterias</param>
		/// <param name="totalRecords"></param>
		/// <returns></returns>
		public virtual List<InventoryJournalTransaction> AdvancedSearch(IConnectionManager entry, InventoryJournalLineSearch searchCriteria, out int totalRecords)
		{
			ValidateSecurity(entry);

			List<Condition> conditions = new List<Condition>();
			List<TableColumn> columns = new List<TableColumn>(journalColumns);
			List<Condition> externalConditions = new List<Condition>();

			using (var cmd = entry.Connection.CreateCommand())
			{
				columns.AddRange(new List<TableColumn>
				{
					new TableColumn {ColumnName = $"ROW_NUMBER() OVER(order by { ResolveSort(searchCriteria.SortBy, searchCriteria.SortBackwards)})", ColumnAlias = "ROW"},
					new TableColumn {ColumnName = "CASE WHEN IJT.JOURNALTYPE <> 9 THEN 0 ELSE (SELECT ISNULL(SUM(M.ADJUSTMENT), 0) FROM INVENTJOURNALTRANS M WHERE M.PARENTMASTERID = A.MASTERID) END", ColumnAlias = "MOVEDQTY"},
					new TableColumn {ColumnName = "ITEMBARCODE", ColumnAlias = "ITEMBARCODE", TableAlias = "ITB"},
				});

				if (!(searchCriteria.RowFrom == 0 && searchCriteria.RowTo == 0))
				{
					externalConditions.Add(new Condition { Operator = "AND", ConditionValue = "CTE.ROW BETWEEN @ROWFROM AND @ROWTO" });
					MakeParam(cmd, "ROWFROM", searchCriteria.RowFrom, SqlDbType.Int);
					MakeParam(cmd, "ROWTO", searchCriteria.RowTo, SqlDbType.Int);
				}

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.JOURNALID = @JOURNALID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "(IJT.JOURNALTYPE <> 9 OR (IJT.JOURNALTYPE = 9 AND A.PARENTMASTERID IS NULL))" }); //don't return child items for parked inventory journals

				if (searchCriteria.Description != null && searchCriteria.Description.Count == 1 && !string.IsNullOrEmpty(searchCriteria.Description[0]))
				{
					string searchString = PreProcessSearchText(searchCriteria.Description[0], true, searchCriteria.DescriptionBeginsWith);
					conditions.Add(new Condition { Operator = "AND", ConditionValue = " (IT.ITEMNAME LIKE @DESCRIPTION OR IT.ITEMID LIKE @DESCRIPTION OR IT.VARIANTNAME LIKE @DESCRIPTION OR IT.NAMEALIAS LIKE @DESCRIPTION OR ITB.ITEMBARCODE LIKE @DESCRIPTION) " });
					MakeParam(cmd, "DESCRIPTION", searchString);
				}
				else if (searchCriteria.Description != null && searchCriteria.Description.Count > 1)
				{
					List<Condition> searchConditions = new List<Condition>();
					for (int index = 0; index < searchCriteria.Description.Count; index++)
					{
						var searchToken = PreProcessSearchText(searchCriteria.Description[index], true, searchCriteria.DescriptionBeginsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							searchConditions.Add(new Condition { ConditionValue = $@" (IT.ITEMNAME LIKE @DESCRIPTION{index} OR IT.ITEMID LIKE @DESCRIPTION{index} OR IT.NAMEALIAS LIKE @DESCRIPTION{index} OR ITB.ITEMBARCODE LIKE @DESCRIPTION{index}) ", Operator = "AND" });
							MakeParam(cmd, $"DESCRIPTION{index}", searchToken);
						}
					}

					conditions.Add(new Condition { Operator = "AND", ConditionValue = $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) " });
				}

				if (searchCriteria.VariantDescription != null && searchCriteria.VariantDescription.Count == 1 && !string.IsNullOrEmpty(searchCriteria.VariantDescription[0]))
				{
					string searchString = PreProcessSearchText(searchCriteria.VariantDescription[0], true, searchCriteria.VariantDescriptionBeginsWith);
					conditions.Add(new Condition { Operator = "AND", ConditionValue = " IT.VARIANTNAME LIKE @VARIANT " });
					MakeParam(cmd, "VARIANT", searchString);
				}
				else if (searchCriteria.VariantDescription != null && searchCriteria.VariantDescription.Count > 1)
				{
					List<Condition> searchConditions = new List<Condition>();
					for (int index = 0; index < searchCriteria.VariantDescription.Count; index++)
					{
						var searchToken = PreProcessSearchText(searchCriteria.VariantDescription[index], true, searchCriteria.VariantDescriptionBeginsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							searchConditions.Add(new Condition { ConditionValue = $" IT.VARIANTNAME LIKE @VARIANT{index} ", Operator = "AND" });
							MakeParam(cmd, $"VARIANT{index}", searchToken);
						}
					}

					conditions.Add(new Condition { Operator = "AND", ConditionValue = $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) " });
				}

				if (searchCriteria.Barcode != null && searchCriteria.Barcode.Count == 1 && !string.IsNullOrEmpty(searchCriteria.Barcode[0]))
				{
					string searchString = PreProcessSearchText(searchCriteria.Barcode[0], true, searchCriteria.BarcodeBeginsWith);
					conditions.Add(new Condition { Operator = "AND", ConditionValue = " ITB.ITEMBARCODE LIKE @BARCODE " });
					MakeParam(cmd, "BARCODE", searchString);
				}
				else if (searchCriteria.Barcode != null && searchCriteria.Barcode.Count > 1)
				{
					List<Condition> searchConditions = new List<Condition>();
					for (int index = 0; index < searchCriteria.Barcode.Count; index++)
					{
						var searchToken = PreProcessSearchText(searchCriteria.Barcode[index], true, searchCriteria.BarcodeBeginsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							searchConditions.Add(new Condition { ConditionValue = $" ITB.ITEMBARCODE LIKE @BARCODE{index} ", Operator = "AND" });
							MakeParam(cmd, $"BARCODE{index}", searchToken);
						}
					}

					conditions.Add(new Condition { Operator = "AND", ConditionValue = $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) " });
				}

				if (!RecordIdentifier.IsEmptyOrNull(searchCriteria.UnitID))
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.UNITID = @UNITID "
					});
					MakeParam(cmd, "UNITID", searchCriteria.UnitID);
				}

				if (searchCriteria.Quantity.HasValue)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.ADJUSTMENT {GetComparisonOperator(searchCriteria.QuantityOperator) }  @ADJUSTMENT "
					});

					MakeParam(cmd, "ADJUSTMENT", searchCriteria.Quantity.Value, SqlDbType.Decimal);
				}

				if (searchCriteria.Status.HasValue)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.POSTED = {searchCriteria.Status.Value} "
					});
				}

				if(!RecordIdentifier.IsEmptyOrNull(searchCriteria.ReasonCodeID))
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "A.REASONREFRECID = @REASONID "
					});
					MakeParam(cmd, "REASONID", searchCriteria.ReasonCodeID);
				}

				if (searchCriteria.PostedDateTo != Date.Empty && searchCriteria.PostedDateFrom != Date.Empty)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.POSTEDDATETIME BETWEEN @DATEFROM AND @DATETO "
					});

					MakeParam(cmd, "DATEFROM", searchCriteria.PostedDateFrom.DateTime, SqlDbType.DateTime);
					MakeParam(cmd, "DATETO", searchCriteria.PostedDateTo.DateTime, SqlDbType.DateTime);
				}
				else if (searchCriteria.PostedDateTo != Date.Empty)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.POSTEDDATETIME < @DATETO "
					});

					MakeParam(cmd, "DATETO", searchCriteria.PostedDateTo.DateTime, SqlDbType.DateTime);
				}
				else if (searchCriteria.PostedDateFrom != Date.Empty)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.POSTEDDATETIME > @DATEFROM "
					});

					MakeParam(cmd, "DATEFROM", searchCriteria.PostedDateFrom.DateTime, SqlDbType.DateTime);
				}

				if (!RecordIdentifier.IsEmptyOrNull(searchCriteria.StaffLogin))
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "USR.LOGIN = @STAFFLOGIN "
					});
					MakeParam(cmd, "STAFFLOGIN", searchCriteria.StaffLogin);
				}

				if (searchCriteria.AreaID != Guid.Empty)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "A.AREA = @AREA "
					});
					MakeParam(cmd, "AREA", searchCriteria.AreaID, SqlDbType.UniqueIdentifier);
				}

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DATAAREAID = @DATAAREAID" });

				string joinCondition = QueryPartGenerator.JoinGenerator(listJoins);
				joinCondition += Environment.NewLine + "OUTER APPLY (SELECT TOP 1 ITEMBARCODE FROM INVENTITEMBARCODE IB WHERE IB.DATAAREAID = @DATAAREAID AND IB.ITEMID = IT.ITEMID AND IB.BLOCKED != 1 AND IB.DELETED = 0 ORDER BY IB.RBOSHOWFORITEM DESC, IB.QTY ASC) ITB";
				MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

				string baseQuery = string.Format(QueryTemplates.BaseQuery("INVENTJOURNALTRANS", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					joinCondition,
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);

				cmd.CommandText = $";WITH CTE AS ({baseQuery}) " +
					$"SELECT {QueryPartGenerator.ExternalColumnGenerator(columns, "CTE")}, ROW_COUNT = tCountTransactions.ROW_COUNT " +
					$"FROM CTE CROSS JOIN (SELECT Count(*) AS ROW_COUNT FROM CTE) AS tCountTransactions " +
					$"{QueryPartGenerator.ConditionGenerator(externalConditions)}" +
					$"ORDER BY {ResolveSort(searchCriteria.SortBy, searchCriteria.SortBackwards, false)}";

				totalRecords = 0;
				MakeParam(cmd, "JOURNALID", (string)searchCriteria.JournalId);

				return Execute<InventoryJournalTransaction, int>(entry, cmd, CommandType.Text, ref totalRecords, PopulateJournalInfoAdvancedWithCount);
			}
		}

		/// <summary>
		/// Updates the journal line status (Open, Posted or Partially Posted) and, if needed, the line's master id.
		/// </summary>
		/// <param name="entry">The entry connection into the database</param>
		/// <param name="journalId">Journal id to which this line belongs to</param>
		/// <param name="lineNum">Curent line id</param>
		/// <param name="masterId">Current line master id (if available)</param>
		/// <param name="newStatus">The new status</param>
		/// <returns>The <see cref="Guid">MasterID</see> of the updated journal line</returns>
		/// <remarks>Used by the Parked Inventory > Move to Inventory functionality</remarks>
		public virtual RecordIdentifier UpdateStatus(IConnectionManager entry,
													 RecordIdentifier journalId,
													 RecordIdentifier lineNum,
													 RecordIdentifier masterId,
													 InventoryJournalStatus newStatus)
		{
			var statement = new SqlServerStatement("INVENTJOURNALTRANS");  

			statement.StatementType = StatementType.Update;
			statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
			statement.AddCondition("JOURNALID", (string)journalId);
			statement.AddCondition("LINENUM", (string)lineNum);
			if (RecordIdentifier.IsEmptyOrNull(masterId))
			{
				masterId = Guid.NewGuid();
				statement.AddField("MASTERID", (Guid)masterId, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.AddCondition("MASTERID", (Guid)masterId, SqlDbType.UniqueIdentifier);
			}

			statement.AddField("POSTED", newStatus, SqlDbType.Int);

			entry.Connection.ExecuteStatement(statement);

			return masterId;
		}

        /// <summary>
        /// Updates a single journal line with a picture ID based on the transaction ID and line IDs from the inventory app
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="omniTransactionID">The ID of the transaction in the inventory app that this line was created on</param>
        /// <param name="omniLineID">The ID of the line that was assigned to it by the inventory app</param>
        /// <param name="pictureID">The ID of the picture to set on the line</param>
        public virtual void SetPictureIDForOmniLine(IConnectionManager entry, string omniTransactionID, string omniLineID, RecordIdentifier pictureID)
        {
            var statement = new SqlServerStatement("INVENTJOURNALTRANS");

            statement.StatementType = StatementType.Update;
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("OMNITRANSACTIONID", omniTransactionID);
            statement.AddCondition("OMNILINEID", omniLineID);

            statement.AddField("PICTUREID", (string)pictureID);

            entry.Connection.ExecuteStatement(statement);

        }

		/// <summary>
		/// Returns the already moved-2-inventory items for a given parked journal line
		/// </summary>
		/// <param name="entry">The entry connection into the database</param>
		/// <param name="journalId">Journal id to which this line belongs to</param>
		/// <param name="lineMasterId">The journal line Master id( of type <see cref="Guid"/>) </param>
		/// <param name="sortBy">The column index to sort by</param>
		/// <param name="sortBackwards">Whether to sort the result backwards or not</param>
		/// <returns></returns>
		public virtual List<InventoryJournalTransaction> GetMovedToInventoryLines(IConnectionManager entry,
																				RecordIdentifier journalId,
																				RecordIdentifier lineMasterId,
																				InventoryJournalTransactionSorting sortBy,
																				bool sortBackwards)
		{
			ValidateSecurity(entry);

			if (RecordIdentifier.IsEmptyOrNull(lineMasterId))
			{
				return new List<InventoryJournalTransaction>();
			}

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.JOURNALID = @JOURNALID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.PARENTMASTERID = @LINEMASTERID" });

				cmd.CommandText = string.Format(
				 QueryTemplates.BaseQuery("INVENTJOURNALTRANS", "A"),
				 QueryPartGenerator.InternalColumnGenerator(journalColumns),
				 QueryPartGenerator.JoinGenerator(listJoins),
				 QueryPartGenerator.ConditionGenerator(conditions),
				 "ORDER BY " + ResolveSort(sortBy, sortBackwards)
				 );

				MakeParam(cmd, "JOURNALID", (string)journalId);
				MakeParam(cmd, "LINEMASTERID", (Guid)lineMasterId, SqlDbType.UniqueIdentifier);

				return Execute<InventoryJournalTransaction>(entry, cmd, CommandType.Text, PopulateJournalInfo);
			}
		}

		public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
		{
			return LineNumExists(entry, id);
		}

		public RecordIdentifier SequenceID
		{
			get { return "JournalLineNum"; }
		}

		public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
		{
			return GetExistingRecords(entry, "INVENTJOURNALTRANS", "JOURNALID", sequenceFormat, startingRecord, numberOfRecords);
		}
	}
}
