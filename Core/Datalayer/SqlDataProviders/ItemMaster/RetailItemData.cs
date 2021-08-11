using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Properties;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    /// <summary>
    /// Data provider class for a retail item.
    /// </summary>
    public partial class RetailItemData : SqlServerDataProviderBase, IRetailItemData
	{
		private static Dictionary<ColumnPopulation, List<TableColumn>> selectionColumns = new Dictionary<ColumnPopulation, List<TableColumn>>();

		private static object threadLock = new object();

		private static List<Join> itemJoins = new List<Join>
		{
			new Join
			{
				Condition = " A.RETAILGROUPMASTERID = iir.MASTERID AND IIR.DELETED =0",
				JoinType = "LEFT OUTER",
				Table = "RETAILGROUP",
				TableAlias = "IIR"
			},
			new Join
			{
				Condition = " IIR.DEPARTMENTMASTERID = IID.MASTERID AND IID.DELETED =0",
				JoinType = "LEFT OUTER",
				Table = "RETAILDEPARTMENT",
				TableAlias = "IID"
			},
			new Join
			{
				Condition = " IID.DIVISIONMASTERID = DIV.MASTERID AND DIV.DELETED =0",
				JoinType = "LEFT OUTER",
				Table = "RETAILDIVISION",
				TableAlias = "DIV"
			},
			new Join
			{
				Condition = " A.SALESTAXITEMGROUPID = tgh.TAXITEMGROUP",
				JoinType = "LEFT OUTER",
				Table = "TAXITEMGROUPHEADING",
				TableAlias = "tgh"
			},
			new Join
			{
				Condition = " A.SALESUNITID = su.UNITID",
				JoinType = "LEFT OUTER",
				Table = "UNIT",
				TableAlias = "su"
			},
			new Join
			{
				Condition = " A.PURCHASEUNITID = pu.UNITID",
				JoinType = "LEFT OUTER",
				Table = "UNIT",
				TableAlias = "pu"
			},
			new Join
			{
				Condition = " A.INVENTORYUNITID = iu.UNITID",
				JoinType = "LEFT OUTER",
				Table = "UNIT",
				TableAlias = "iu"
			},
			new Join
			{
				Condition = " A.SALESLINEDISC = ld.GROUPID and ld.TYPE = 1 and ld.MODULE = 0",
				JoinType = "LEFT OUTER",
				Table = "PRICEDISCGROUP",
				TableAlias = "ld"
			},
			new Join
			{
				Condition = " A.SALESMULTILINEDISC = mld.GROUPID and mld.TYPE = 2 and mld.MODULE = 0",
				JoinType = "LEFT OUTER",
				Table = "PRICEDISCGROUP",
				TableAlias = "mld"
			},
			new Join
			{
				Condition = " A.VALIDATIONPERIODID = pdv.ID",
				JoinType = "LEFT OUTER",
				Table = "POSDISCVALIDATIONPERIOD",
				TableAlias = "pdv"
			},
		};

		private static List<Join> simpleJoins = new List<Join>
		{
			new Join
			{
				Condition = " A.RETAILGROUPMASTERID = iir.MASTERID AND IIR.DELETED =0",
				JoinType = "LEFT OUTER",
				Table = "RETAILGROUP",
				TableAlias = "IIR"
			},
			new Join
			{
				Condition = " IIR.DEPARTMENTMASTERID = IID.MASTERID AND IID.DELETED =0",
				JoinType = "LEFT OUTER",
				Table = "RETAILDEPARTMENT",
				TableAlias = "IID"
			}
		};

		private static Dictionary<SortEnum, TableColumn> SortColumns = new Dictionary<SortEnum, TableColumn>
		{
			{SortEnum.None, new TableColumn()},
			{SortEnum.Alias, new TableColumn {ColumnName = "NAMEALIAS"}},
			{SortEnum.Description, new TableColumn {ColumnName = "ITEMNAME"}},
			{SortEnum.ID, new TableColumn {ColumnName = "ITEMID", TableAlias = "A"}},
			{SortEnum.PriceIncludingTax, new TableColumn {ColumnName = "SALESPRICEINCLTAX"}},
			{
				SortEnum.RetailGroup,
				new TableColumn {ColumnName = "NAME", TableAlias = "IIR", ColumnAlias = "RETAILGROUPNAME"}
			},
			{
				SortEnum.RetailDepartment,
				new TableColumn {ColumnName = "NAME", TableAlias = "IID", ColumnAlias = "RETAILDEPARTMENTNAME"}
			},
			{SortEnum.TaxGroup, new TableColumn {ColumnName = "NAME", TableAlias = "TGH", ColumnAlias = "TAXGROUPNAME"}},
			{SortEnum.VariantName, new TableColumn {ColumnName = "VARIANTNAME"}},

			{SortEnum.AliasDesc, new TableColumn {ColumnName = "NAMEALIAS", SortDescending = true}},
			{SortEnum.DescriptionDesc, new TableColumn {ColumnName = "ITEMNAME", SortDescending = true}},
			{SortEnum.IDDesc, new TableColumn {ColumnName = "ITEMID", TableAlias = "A",SortDescending = true}},
			{SortEnum.PriceIncludingTaxDesc, new TableColumn {ColumnName = "SALESPRICEINCLTAX", SortDescending = true}},
			{
				SortEnum.RetailGroupDesc,
				new TableColumn
				{
					ColumnName = "NAME",
					TableAlias = "IIR",
					ColumnAlias = "RETAILGROUPNAME",
					SortDescending = true
				}
			},
			{
				SortEnum.RetailDepartmentDesc,
				new TableColumn
				{
					ColumnName = "NAME",
					TableAlias = "IID",
					ColumnAlias = "RETAILDEPARTMENTNAME",
					SortDescending = true
				}
			},
			{
				SortEnum.TaxGroupDesc,
				new TableColumn
				{
					ColumnName = "NAME",
					TableAlias = "TGH",
					ColumnAlias = "TAXGROUPNAME",
					SortDescending = true
				}
			},
			{
				SortEnum.VariantNameDesc,
				new TableColumn
				{
					ColumnName = "VARIANTNAME",
					SortDescending = true
				}
			},
		};

		private static Dictionary<ColumnPopulation, List<TableColumn>> SelectionColumns
		{
			get
			{
				lock (threadLock)
				{
					if (selectionColumns.Count == 0)
					{
						selectionColumns.Add(ColumnPopulation.IDOnly, new List<TableColumn>
						{
							new TableColumn {ColumnName = "ITEMID", TableAlias = "A"}
						});
						selectionColumns.Add(ColumnPopulation.DataEntity, new List<TableColumn>
						{
							new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
							new TableColumn {ColumnName = "ITEMNAME", TableAlias = "A"},
							new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "A"}
						});
						selectionColumns.Add(ColumnPopulation.MasterDataEntity, new List<TableColumn>
						{
							new TableColumn {ColumnName = "MASTERID", TableAlias = "A"},
							new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
							new TableColumn {ColumnName = "ITEMNAME", TableAlias = "A"},
							new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "A"}
						});
						selectionColumns.Add(ColumnPopulation.Simple, new List<TableColumn>
						{
							new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
							new TableColumn {ColumnName = "MASTERID", TableAlias = "A"},
							new TableColumn {ColumnName = "HEADERITEMID", TableAlias = "A"},
							new TableColumn {ColumnName = "ITEMNAME", TableAlias = "A"},
							new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "A"},
							new TableColumn {ColumnName = "ITEMTYPE", TableAlias = "A"},
							new TableColumn {ColumnName = "NAMEALIAS", TableAlias = "A"},
							new TableColumn {ColumnName = "RETAILGROUPMASTERID", TableAlias = "A"},
							new TableColumn {ColumnName = "MASTERID", ColumnAlias = "RETAILDEPARTMENTMASTERID", TableAlias = "IID"},
							new TableColumn {ColumnName = "DIVISIONMASTERID", ColumnAlias = "RETAILDIVISIONMASTERID", TableAlias = "IID"},
							new TableColumn {ColumnName = "SALESPRICEINCLTAX", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESPRICE", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESTAXITEMGROUPID", TableAlias = "A"},
							new TableColumn {ColumnName = "DELETED", TableAlias = "A"},
						});
						selectionColumns.Add(ColumnPopulation.POS, new List<TableColumn>
						{
							new TableColumn {ColumnName = "ITEMID", TableAlias = "ss"},
							new TableColumn {ColumnName = "ITEMNAME", TableAlias = "ss"}
						});
						selectionColumns.Add(ColumnPopulation.SiteManager, new List<TableColumn>
						{
							new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
							new TableColumn {ColumnName = "ITEMNAME", TableAlias = "A"},
							new TableColumn {ColumnName = "ITEMTYPE", TableAlias = "A"},
							new TableColumn {ColumnName = "NAMEALIAS", TableAlias = "A"},
							new TableColumn {ColumnName = "HEADERITEMID", TableAlias = "A"},
							new TableColumn {ColumnName = "RETAILGROUPMASTERID", TableAlias = "A"},
							new TableColumn {ColumnName = "MASTERID", ColumnAlias = "RETAILDEPARTMENTMASTERID", TableAlias = "IID"},
							new TableColumn {ColumnName = "MASTERID", ColumnAlias = "RETAILDIVISIONMASTERID", TableAlias = "DIV"},
							new TableColumn {ColumnName = "MASTERID", TableAlias = "A"},
							new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "A"},
							new TableColumn {ColumnName = "DEFAULTVENDORID", TableAlias = "A"},
							new TableColumn {ColumnName = "ISNULL(A.EXTENDEDDESCRIPTION,'') ", TableAlias = "", ColumnAlias = "EXTENDEDDESCRIPTION"},
							new TableColumn {ColumnName = "ISNULL(A.SEARCHKEYWORDS,'') ", TableAlias = "", ColumnAlias = "SEARCHKEYWORDS"},
                            new TableColumn {ColumnName = "ZEROPRICEVALID", TableAlias = "A"},
							new TableColumn {ColumnName = "QTYBECOMESNEGATIVE", TableAlias = "A"},
							new TableColumn {ColumnName = "NODISCOUNTALLOWED", TableAlias = "A"},
							new TableColumn {ColumnName = "KEYINPRICE", TableAlias = "A"},
							new TableColumn {ColumnName = "KEYINSERIALNUMBER", TableAlias = "A"},
							new TableColumn {ColumnName = "SCALEITEM", TableAlias = "A"},
							new TableColumn {ColumnName = "TAREWEIGHT", TableAlias = "A"},
                            new TableColumn {ColumnName = "KEYINQTY", TableAlias = "A"},
							new TableColumn {ColumnName = "BLOCKEDONPOS", TableAlias = "A"},
							new TableColumn {ColumnName = "BARCODESETUPID", TableAlias = "A"},
							new TableColumn {ColumnName = "PRINTVARIANTSSHELFLABELS", TableAlias = "A"},
							new TableColumn {ColumnName = "FUELITEM", TableAlias = "A"},
							new TableColumn {ColumnName = "GRADEID", TableAlias = "A"},
							new TableColumn {ColumnName = "MUSTKEYINCOMMENT", TableAlias = "A"},
							new TableColumn {ColumnName = "DATETOBEBLOCKED", TableAlias = "A"},
							new TableColumn {ColumnName = "DATETOACTIVATEITEM", TableAlias = "A"},
							new TableColumn {ColumnName = "PROFITMARGIN", TableAlias = "A"},
							new TableColumn {ColumnName = "VALIDATIONPERIODID", TableAlias = "A"},
							new TableColumn
							{
								ColumnName = "ISNULL(pdv.DESCRIPTION,'')",
								ColumnAlias = "VALIDATIONPERIODIDNAME"
							},
							new TableColumn {ColumnName = "MUSTSELECTUOM", TableAlias = "A"},
							new TableColumn {ColumnName = "INVENTORYUNITID", TableAlias = "A"},
							new TableColumn {ColumnName = "PURCHASEUNITID", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESUNITID", TableAlias = "A"},
							new TableColumn {ColumnName = "PURCHASEPRICE", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESPRICE", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESPRICEINCLTAX", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESMARKUP", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESLINEDISC", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESMULTILINEDISC", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESALLOWTOTALDISCOUNT", TableAlias = "A"},
							new TableColumn {ColumnName = "SALESTAXITEMGROUPID", TableAlias = "A"},
							new TableColumn {ColumnName = "ISNULL(tgh.NAME,'')", ColumnAlias = "SALESTAXITEMGROUPNAME"},
							new TableColumn {ColumnName = "ISNULL(iir.NAME,'')", ColumnAlias = "RETAILGROUPNAME"},
							new TableColumn {ColumnName = "iir.GROUPID", ColumnAlias = "RETAILGROUPID"},
							new TableColumn {ColumnName = "ISNULL(iid.NAME,'')", ColumnAlias = "RETAILDEPARTMENTNAME"},
							new TableColumn {ColumnName = "iid.DEPARTMENTID", ColumnAlias = "RETAILDEPARTMENTID"},
							new TableColumn {ColumnName = "ISNULL(div.NAME,'')", ColumnAlias = "RETAILDIVISIONNAME"},
							new TableColumn {ColumnName = "ISNULL(su.TXT,'')", ColumnAlias = "SALESUNITNAME"},
							new TableColumn {ColumnName = "ISNULL(pu.TXT,'')", ColumnAlias = "PURCHASEUNITNAME"},
							new TableColumn {ColumnName = "ISNULL(iu.TXT,'')", ColumnAlias = "INVENTORYUNITNAME"},
							new TableColumn {ColumnName = "ISNULL(ld.NAME,'')", ColumnAlias = "SALESLINEDISCNAME"},
							new TableColumn {ColumnName = "ISNULL(mld.NAME,'')", ColumnAlias = "SALESMULTILINEDISCNAME"},
							new TableColumn {ColumnName = "DELETED", TableAlias = "A"},
							new TableColumn {ColumnName = "RETURNABLE", TableAlias = "A"},
							new TableColumn {ColumnName = "CANBESOLD", TableAlias = "A"},
							new TableColumn {ColumnName = "PRODUCTIONTIME", TableAlias = "A"}
						});
					}
					return selectionColumns;
				}
			}
		}

		private static void PopulateRetailItemPrice(IDataReader dr, RetailItemPrice price)
		{
			price.ID = (Guid)dr["MASTERID"];
			price.PurchasePrice = (decimal)dr["PURCHASEPRICE"];
			price.ProfitMargin = (decimal)dr["PROFITMARGIN"];
			price.SalesPrice = (decimal)dr["SALESPRICE"];
			price.SalesPriceIncludingTax = (decimal)dr["SALESPRICEINCLTAX"];
			price.SalesTaxItemGroupID = (string)dr["SALESTAXITEMGROUPID"];
		}

		protected virtual void PopulateItem(IDataReader dr, RetailItem item)
		{
			PopulateSimpleItem(dr, item);
			item.DefaultVendorID = (string)dr["DEFAULTVENDORID"];
			item.ExtendedDescription = (string)dr["EXTENDEDDESCRIPTION"];
			item.SearchKeywords = (string)dr["SEARCHKEYWORDS"];
            item.ZeroPriceValid = (bool)dr["ZEROPRICEVALID"];
			item.QuantityBecomesNegative = (bool)dr["QTYBECOMESNEGATIVE"];
			item.NoDiscountAllowed = (bool)dr["NODISCOUNTALLOWED"];
			item.KeyInPrice = (RetailItem.KeyInPriceEnum)(byte)dr["KEYINPRICE"];
			item.KeyInSerialNumber = (RetailItem.KeyInSerialNumberEnum)(byte)dr["KEYINSERIALNUMBER"];
			item.ScaleItem = (bool)dr["SCALEITEM"];
			item.TareWeight = (int)dr["TAREWEIGHT"];
            item.KeyInQuantity = (RetailItem.KeyInQuantityEnum)(byte)dr["KEYINQTY"];
			item.BlockedOnPOS = (bool)dr["BLOCKEDONPOS"];
			item.BarCodeSetupID = (string)dr["BARCODESETUPID"];
			item.PrintVariantsShelfLabels = (bool)dr["PRINTVARIANTSSHELFLABELS"];
			item.IsFuelItem = (bool)dr["FUELITEM"];
			item.GradeID = (string)dr["GRADEID"];
			item.MustKeyInComment = (bool)dr["MUSTKEYINCOMMENT"];
			item.DateToBeBlocked = Date.FromAxaptaDate(dr["DATETOBEBLOCKED"]);
			item.DateToActivateItem = Date.FromAxaptaDate(dr["DATETOACTIVATEITEM"]);
			item.ProfitMargin = (decimal)dr["PROFITMARGIN"];
			item.ValidationPeriodID = (string)dr["VALIDATIONPERIODID"];
			item.ValidationPeriodDescription = (string)dr["VALIDATIONPERIODIDNAME"];
			item.MustSelectUOM = (bool)dr["MUSTSELECTUOM"];
			item.InventoryUnitID = (string)dr["INVENTORYUNITID"];
			item.InventoryUnitName = (string)dr["INVENTORYUNITNAME"];
			item.PurchaseUnitID = string.IsNullOrEmpty((string)dr["PURCHASEUNITID"]) ? item.InventoryUnitID : (string)dr["PURCHASEUNITID"];
			item.PurchaseUnitName = string.IsNullOrEmpty((string)dr["PURCHASEUNITNAME"]) ? item.InventoryUnitName : (string)dr["PURCHASEUNITNAME"];
			item.SalesUnitID = (string)dr["SALESUNITID"];
			item.SalesUnitName = (string)dr["SALESUNITNAME"];
			item.PurchasePrice = (decimal)dr["PURCHASEPRICE"];
			item.SalesPrice = (decimal)dr["SALESPRICE"];
			item.SalesPriceIncludingTax = (decimal)dr["SALESPRICEINCLTAX"];
			item.SalesMarkup = (decimal)dr["SALESMARKUP"];
			item.SalesLineDiscount = (string)dr["SALESLINEDISC"];
			item.SalesLineDiscountName = (string)dr["SALESLINEDISCNAME"];
			item.SalesMultiLineDiscount = (string)dr["SALESMULTILINEDISC"];
			item.SalesMultiLineDiscountName = (string)dr["SALESMULTILINEDISCNAME"];
			item.SalesAllowTotalDiscount = (bool)dr["SALESALLOWTOTALDISCOUNT"];
			item.SalesTaxItemGroupName = (string)dr["SALESTAXITEMGROUPNAME"];
			item.RetailDepartmentName = (string)dr["RETAILDEPARTMENTNAME"];
			item.RetailGroupName = (string)dr["RETAILGROUPNAME"];
			item.RetailDivisionName = (string)dr["RETAILDIVISIONNAME"];
            item.RetailGroupID = (dr["RETAILGROUPID"] is DBNull) ? RecordIdentifier.Empty : (string)dr["RETAILGROUPID"];
            item.RetailDepartmentID = (dr["RETAILDEPARTMENTID"] is DBNull) ? RecordIdentifier.Empty : (string)dr["RETAILDEPARTMENTID"];
            item.Returnable = (bool)dr["RETURNABLE"];
            item.CanBeSold = (bool)dr["CANBESOLD"];
			item.ProductionTime = (int)dr["PRODUCTIONTIME"];

			item.Deleted = (bool)dr["DELETED"];
		}

		protected virtual void PopulateItemWithCount(IConnectionManager entry, IDataReader dr, RetailItem item, ref int rowCount)
		{
			PopulateItem(dr, item);
			PopulateRowCount(entry, dr, ref rowCount);
		}

		protected virtual void PopulateSimpleItem(IDataReader dr, SimpleRetailItem item)
		{
			item.ID = (string)dr["ITEMID"];
			item.MasterID = (Guid)dr["MASTERID"];
			item.HeaderItemID = dr["HEADERITEMID"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["HEADERITEMID"];
			item.Text = (string)dr["ITEMNAME"];
			item.VariantName = (string)dr["VARIANTNAME"];
			item.ItemType = (ItemTypeEnum)(byte)dr["ITEMTYPE"];
			item.NameAlias = (string)dr["NAMEALIAS"];
			item.SalesPrice = (decimal)dr["SALESPRICE"];
			item.SalesPriceIncludingTax = (decimal)dr["SALESPRICEINCLTAX"];
			item.SalesTaxItemGroupID = (string)dr["SALESTAXITEMGROUPID"];
			item.Deleted = (bool)dr["DELETED"];

			item.RetailGroupMasterID = (dr["RETAILGROUPMASTERID"] is DBNull) ? RecordIdentifier.Empty : (Guid)dr["RETAILGROUPMASTERID"];
			item.RetailDepartmentMasterID = (dr["RETAILDEPARTMENTMASTERID"] is DBNull) ? RecordIdentifier.Empty : (Guid)dr["RETAILDEPARTMENTMASTERID"];
			item.RetailDivisionMasterID = (dr["RETAILDIVISIONMASTERID"] is DBNull) ? RecordIdentifier.Empty : (Guid)dr["RETAILDIVISIONMASTERID"];
		}

		protected virtual void PopulateSimpleItemWithCount(IConnectionManager entry, IDataReader dr, SimpleRetailItem item, ref int rowCount)
		{
			PopulateSimpleItem(dr, item);
			PopulateRowCount(entry, dr, ref rowCount);
		}

		protected virtual void PopulateItemID(IDataReader dr, out RecordIdentifier id)
		{
			id = (string)dr["ITEMID"];
		}

		protected virtual void PopulateItemIDWithCount(IConnectionManager entry, IDataReader dr, RecordIdentifier id, ref int rowCount)
		{
			PopulateItemID(dr, out id);
			PopulateRowCount(entry, dr, ref rowCount);
		}

		protected virtual void PopulateDataEntityWithCount(IConnectionManager entry, IDataReader dr, DataEntity item, ref int rowCount)
		{
			PopulateDataEntity(dr, item);

			PopulateRowCount(entry, dr, ref rowCount);
		}

		protected virtual void PopulateDataEntity(IDataReader dr, DataEntity item)
		{
			item.ID = (string)dr["ITEMID"];
			item.Text = (string)dr["ITEMNAME"];
			if (dr["VARIANTNAME"] != DBNull.Value && !string.IsNullOrEmpty((string)dr["VARIANTNAME"]))
			{
				item.Text += " - " + (string)dr["VARIANTNAME"];
			}
		}
		protected virtual void PopulateMasterDataEntityWithCount(IConnectionManager entry, IDataReader dr, MasterIDEntity item,
		ref int rowCount)
		{
			PopulateMasterDataEntity(dr, item);

			PopulateRowCount(entry, dr, ref rowCount);
		}

		protected virtual void PopulateMasterDataEntity(IDataReader dr, MasterIDEntity item)
		{
			item.ID = (Guid)dr["MASTERID"];
			item.ReadadbleID = (string)dr["ITEMID"];
			item.Text = (string)dr["ITEMNAME"];
			if (dr["VARIANTNAME"] != DBNull.Value && !string.IsNullOrEmpty((string)dr["VARIANTNAME"]))
			{
				item.ExtendedText = (string)dr["VARIANTNAME"];
			}
		}

		protected virtual void PopulateVariantWithCount(IConnectionManager entry, IDataReader dr, MasterIDEntity item, ref int rowCount)
		{
			PopulateVariant(dr, item);

			PopulateRowCount(entry, dr, ref rowCount);
		}

		protected virtual void PopulateVariant(IDataReader dr, MasterIDEntity item)
		{
			item.ID = (Guid)dr["MASTERID"];
			item.ReadadbleID = (string)dr["ITEMID"];
			item.Text = (string)dr["VARIANTNAME"];
		}

		protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
		{
			if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
				entry.Connection.DatabaseVersion == ServerVersion.Unknown)
			{
				rowCount = (int)dr["Row_Count"];
			}
		}

		public virtual Dictionary<string, string> GetUnitIDsForItems(IConnectionManager entry, RetailItem.UnitTypeEnum unitType)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				Dictionary<string, string> itemIDs = new Dictionary<string, string>();

				string column;

				switch (unitType)
				{
					case RetailItem.UnitTypeEnum.Inventory:
						column = "INVENTORYUNITID";
						break;

					case RetailItem.UnitTypeEnum.Purchase:
						column = "PURCHASEUNITID";
						break;

					default:
						column = "SALESUNITID";
						break;
				}

				cmd.CommandType = CommandType.Text;

				cmd.CommandText =
					$@"select ITEMID,
					   {column} as UNITID
					   from RETAILITEM";

				using (IDataReader dr = entry.Connection.ExecuteReader(cmd, CommandType.Text))
				{
					while (dr.Read())
					{
						itemIDs.Add((string) dr["ITEMID"], (string)dr["UNITID"]);
					}
				}
				return itemIDs;
			}
		}

		public virtual Dictionary<string, string> GetTaxCodesForItems(IConnectionManager entry, List<RecordIdentifier> itemIDs)
		{
			var taxCodes = new Dictionary<string, string>();

			// Normally, MS SQL supports 2100 parameters; but, for some reasons, sometimes, this doesn't work with neither 2100 or 2099 params
			// So just choose 2000 so we are safe (hopefully)
			var batchSize = 2000;
			var numberOfBatches = Math.Ceiling((double)itemIDs.Count / batchSize);

			for (int i = 0; i < numberOfBatches; i++)
			{
				var batch = itemIDs.Skip(i * batchSize).Take(batchSize).ToList();
				GetTaxForSmallListOfItems(entry, batch, taxCodes);
			}

			return taxCodes;
		}

		/// <summary>
		/// Maximum 2000 items are allowed
		/// </summary>
		private static void GetTaxForSmallListOfItems(IConnectionManager entry, List<RecordIdentifier> itemIDs, Dictionary<string, string> taxCodes)
		{
			if (itemIDs.Count == 0)
			{
				return;
			}

			using (var cmd = entry.Connection.CreateCommand())
			{
				string[] itemIDArray = itemIDs.Select((x, i) => "@itemID" + i).ToArray();

				cmd.CommandText = string.Format("select ITEMID, SALESTAXITEMGROUPID from RETAILITEM where ITEMID in ({0})", string.Join(",", itemIDArray));

				for (int i = 0; i < itemIDs.Count; ++i)
				{
					MakeParam(cmd, "itemID" + i, itemIDs[i]);
				}

				using (IDataReader dr = entry.Connection.ExecuteReader(cmd, CommandType.Text))
				{
					while (dr.Read())
					{
						taxCodes.Add((string)dr["ITEMID"], (string)dr["SALESTAXITEMGROUPID"]);
					}
				}
			}
		}

		/// <summary>
		/// Looks up the unit id for a item by a given id. The type of unit depends on the module type.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">Id of the retail item in the database, this can be either the ID or the Master ID, its resolved depending on if the RecordIdentifier contains GUID or string</param>
		/// <param name="unitType">Module type enum which determines what type of unit id we are returning</param>
		/// <returns>The unit ID of an item depending on the unit type</returns>
		public virtual RecordIdentifier GetItemUnitID(IConnectionManager entry, RecordIdentifier itemID, RetailItem.UnitTypeEnum unitType)
		{
			string column;

			using (var cmd = entry.Connection.CreateCommand())
			{
				switch (unitType)
				{
					case RetailItem.UnitTypeEnum.Inventory:
						column = "INVENTORYUNITID";
						break;

					case RetailItem.UnitTypeEnum.Purchase:
						column = "PURCHASEUNITID";
						break;

					default:
						column = "SALESUNITID";
						break;
				}

				cmd.CommandType = CommandType.Text;
				cmd.CommandText = string.Format("select {0} from RETAILITEM where ITEMID = @itemID", column);

				if (itemID.IsGuid)
				{
					cmd.CommandText = string.Format("select {0} from RETAILITEM where MASTERID = @itemID", column);

					MakeParam(cmd, "itemID", (Guid)itemID);
				}
				else
				{
					cmd.CommandText = string.Format("select {0} from RETAILITEM where ITEMID = @itemID", column);

					MakeParam(cmd, "itemID", (string)itemID);
				}

				string unitID = (string)entry.Connection.ExecuteScalar(cmd);
				if (unitType == RetailItem.UnitTypeEnum.Purchase && string.IsNullOrEmpty(unitID))
				{
					unitID = (string)GetItemUnitID(entry, itemID, RetailItem.UnitTypeEnum.Inventory);
				}

				return unitID;
			}
		}

		/// <summary>
		/// Gets sales unit from given itemID.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">itemID, this can either be normal itemID or MasterID</param>
		/// <returns></returns>
		public RecordIdentifier GetSalesUnitID(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;

				if (itemID.IsGuid)
				{
					cmd.CommandText = "select SALESUNITID from RETAILITEM where MASTERID = @itemID";

					MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					cmd.CommandText = "select SALESUNITID from RETAILITEM where ITEMID = @itemID";

					MakeParam(cmd, "itemID", (string)itemID);
				}

				object result = entry.Connection.ExecuteScalar(cmd);

				return result == null ? RecordIdentifier.Empty : (string)result;
			}
		}

		/// <summary>
		/// Gets the item ID for the given master ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="masterID">The master ID for the item</param>
		/// <returns></returns>
		public virtual RecordIdentifier GetItemIDFromMasterID(IConnectionManager entry, RecordIdentifier masterID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"select ITEMID
					  from RETAILITEM
					  where MASTERID = @masterID";

				MakeParam(cmd, "masterID", (Guid)masterID, SqlDbType.UniqueIdentifier);

				object result = entry.Connection.ExecuteScalar(cmd);

				return result == null ? RecordIdentifier.Empty : (string)result;
			}
		}

		/// <summary>
		/// Gets the header item ID for an item
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The master ID for the item</param>
		/// <param name="cacheType">Type of cache to be used</param>
		/// <returns></returns>
		public virtual RecordIdentifier GetHeaderItemID(IConnectionManager entry, RecordIdentifier itemID, CacheType cacheType = CacheType.CacheTypeNone)
		{			
			if (cacheType != CacheType.CacheTypeNone)
			{
				DataEntity cacheEntity = (DataEntity)entry.Cache.GetEntityFromCache(typeof(DataEntity), $"GetHeaderItemID:{itemID}");

				if (cacheEntity != null)
				{
					return cacheEntity.ID;
				}
			}

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"select ri2.ITEMID
					  from RETAILITEM ri1
					  inner join RETAILITEM ri2 
					  on ri1.HEADERITEMID = ri2.MASTERID
					  where ri1.ITEMID = @itemID";

				MakeParam(cmd, "itemID", (string)itemID);

				object dbResult = entry.Connection.ExecuteScalar(cmd);

				RecordIdentifier result = dbResult == null ? RecordIdentifier.Empty : (string)dbResult;

				if (dbResult != null && cacheType != CacheType.CacheTypeNone)
				{
					entry.Cache.AddEntityToCache($"GetHeaderItemID:{itemID}", new DataEntity(result, ""), cacheType);
				}

				return result;
			}
		}

		/// <summary>
		/// Updates the unit information on a specific item
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The unique ID of the item to update</param>
		/// <param name="unitID">The new unit ID information</param>
		/// <param name="module">Which module information to update (inventory, purchase,
		/// sales)</param>
		public virtual void UpdateUnitID(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID, RetailItem.UnitTypeEnum module)
		{
			RetailItem item = Get(entry, itemID);

			if (item != null)
			{
				switch (module)
				{
					case RetailItem.UnitTypeEnum.Inventory:
						if (item.InventoryUnitID != unitID)
						{
							item.InventoryUnitID = unitID;
							item.Dirty = true;
							Save(entry, item);
						}
						break;

					case RetailItem.UnitTypeEnum.Purchase:
						if (item.PurchaseUnitID != unitID)
						{
							item.PurchaseUnitID = unitID;
							item.Dirty = true;
							Save(entry, item);
						}
						break;

					case RetailItem.UnitTypeEnum.Sales:
						if (item.SalesUnitID != unitID)
						{
							item.SalesUnitID = unitID;
							item.Dirty = true;
							Save(entry, item);
						}
						break;
				}
			}
		}

        /// <summary>
        /// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="SimpleRetailItem"/>
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchString">The text to search for. Searches in item name, in the ID field and the search alias field</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        /// <param name="includeVariants"></param>
        /// <returns>A list of found items</returns>
        public virtual List<SimpleRetailItem> Search(IConnectionManager entry, string searchString, int rowFrom,
			int rowTo, bool beginsWith, SortEnum sort, bool includeVariants = true)
		{
			int totalRecordsMatched;
			return AdvancedSearchOptimized<SimpleRetailItem>(entry, rowFrom, rowTo, sort, SortEnum.None, true, out totalRecordsMatched,
				ColumnPopulation.Simple, new List<string> { searchString }, beginsWith, includeVariants);
		}

		public List<RecordIdentifier> AdvancedSearchIDOnly(IConnectionManager entry,
			string idOrDescription = null,
			bool idOrDescriptionBeginsWith = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null, // TODO remove this one
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			string variantDescription = null,
			bool variantDescriptionBeginsWith = false,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false,
			List<SearchFlagEntity> searchFlags = null,
			bool isSearchBarControlSearch = false
			)
		{
			int totalRecordsMatching = 0;
			return AdvancedSearchOptimized<RecordIdentifier>(entry, 0, int.MaxValue, SortEnum.None, SortEnum.None, false,
				out totalRecordsMatching,
				ColumnPopulation.IDOnly, new List<string> { idOrDescription },
				idOrDescriptionBeginsWith,
				includeVariants,
				retailGroupID,
				retailDepartmentID,
				taxGroupID,
				variantGroupID,
				vendorID,
				barCode,
				barCodeBeginsWith,
				specialGroup, null,
				variantDescription,
				variantDescriptionBeginsWith,
				attributeDescription,
				attributeDescriptionStartsWith,
				searchFlags,
				isSearchBarControlSearch: isSearchBarControlSearch);
		}

		public List<RecordIdentifier> AdvancedSearchIDOnly(IConnectionManager entry, 
			int rowFrom,
			int rowTo,
			out int totalRecords,
			string idOrDescription = null,
			bool idOrDescriptionBeginsWith = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null, // TODO remove this one
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			string variantDescription = null,
			bool variantDescriptionBeginsWith = false,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false,
			List<SearchFlagEntity> searchFlags = null,
			bool isSearchBarControlSearch = false
			)
		{
			totalRecords = 0;

			return AdvancedSearchOptimized<RecordIdentifier>(entry, 
				rowFrom, 
				rowTo,
				SortEnum.None, 
				SortEnum.None, 
				true,
				out totalRecords,
				ColumnPopulation.IDOnly, new List<string> { idOrDescription },
				idOrDescriptionBeginsWith,
				includeVariants,
				retailGroupID,
				retailDepartmentID,
				taxGroupID,
				variantGroupID,
				vendorID,
				barCode,
				barCodeBeginsWith,
				specialGroup, null,
				variantDescription,
				variantDescriptionBeginsWith,
				attributeDescription,
				attributeDescriptionStartsWith,
				searchFlags,
				isSearchBarControlSearch: isSearchBarControlSearch);
		}

		public List<DataEntity> AdvancedSearchDataEntity(IConnectionManager entry,
			int rowFrom,
			int rowTo,
			string idOrDescription,
			bool idOrDescriptionBeginsWith = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null,
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeVariants = true)
		{
			int totalRecordsMatching = 0;
			return AdvancedSearchOptimized<DataEntity>(entry, rowFrom, rowTo, SortEnum.None, SortEnum.None, false,
				out totalRecordsMatching,
				ColumnPopulation.IDOnly, new List<string> { idOrDescription },
				idOrDescriptionBeginsWith, includeVariants, retailGroupID, retailDepartmentID, taxGroupID,
				variantGroupID, vendorID, barCode, barCodeBeginsWith, specialGroup);
		}

		public List<SimpleRetailItem> AdvancedSearchOptimized(
			IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			out int totalRecordsMatching,
			string idOrDescription = null,
			bool idOrDescriptionBeginsWith = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null,
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			SortEnum secondarySearch = SortEnum.None)
		{
			return AdvancedSearchOptimized<SimpleRetailItem>(entry, rowFrom, rowTo, sort, secondarySearch, true, out totalRecordsMatching,
				ColumnPopulation.Simple, new List<string> { idOrDescription }, idOrDescriptionBeginsWith,
				includeVariants, retailGroupID, retailDepartmentID,
				taxGroupID, variantGroupID, vendorID, barCode, barCodeBeginsWith, specialGroup);
		}

		public List<DataEntity> TokenSearchDataEntity(
			IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			out int totalRecordsMatching,
			List<string> searchTokens,
			bool idOrDescriptionBeginsWith = true,
			string culture = null,
			bool includeVariants = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null,
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
            bool includeHeaders = true,
            bool includeServiceItems = true)
		{
			return AdvancedSearchOptimized<DataEntity>(entry, rowFrom, rowTo, sort, SortEnum.None, true, out totalRecordsMatching,
				ColumnPopulation.DataEntity, searchTokens, idOrDescriptionBeginsWith, includeVariants,
				retailGroupID, retailDepartmentID,
				taxGroupID, variantGroupID, vendorID, barCode, barCodeBeginsWith, specialGroup, culture, includeHeaders: includeHeaders, includeServiceItems: includeServiceItems);
		}

		public List<MasterIDEntity> TokenSearchMasterDataEntity(
            IConnectionManager entry,
		    int rowFrom,
		    int rowTo,
		    SortEnum sort,
		    out int totalRecordsMatching,
		    List<string> searchTokens,
		    bool idOrDescriptionBeginsWith = true,
		    string culture = null,
		    bool includeVariants = true,
		    RecordIdentifier retailGroupID = null,
		    RecordIdentifier retailDepartmentID = null,
		    RecordIdentifier taxGroupID = null,
		    RecordIdentifier variantGroupID = null,
		    RecordIdentifier vendorID = null,
		    string barCode = null,
		    bool barCodeBeginsWith = true,
		    RecordIdentifier specialGroup = null,
		    bool includeHeaders = true,
		    bool includeServiceItems = true)
		{
			return AdvancedSearchOptimized<MasterIDEntity>(entry, rowFrom, rowTo, sort, SortEnum.None, true, out totalRecordsMatching,
				ColumnPopulation.MasterDataEntity, searchTokens, idOrDescriptionBeginsWith, includeVariants,
				retailGroupID, retailDepartmentID,
				taxGroupID, variantGroupID, vendorID, barCode, barCodeBeginsWith, specialGroup, culture, includeHeaders: includeHeaders, includeServiceItems: includeServiceItems);
		}

		public List<SimpleRetailItem> TokenSearch(
			IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			out int totalRecordsMatching,
			List<string> searchTokens,
			bool idOrDescriptionBeginsWith = true,
			string culture = null,
			bool includeVariants = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null,
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null)
		{
			return AdvancedSearchOptimized<SimpleRetailItem>(entry, rowFrom, rowTo, sort, SortEnum.None, true, out totalRecordsMatching,
				ColumnPopulation.Simple, searchTokens, idOrDescriptionBeginsWith, includeVariants,
				retailGroupID, retailDepartmentID,
				taxGroupID, variantGroupID, vendorID, barCode, barCodeBeginsWith, specialGroup, culture);
		}

		public List<SearchFlagEntity> ItemSearchFlags
		{
			get
			{
				List<SearchFlagEntity> flags = new List<SearchFlagEntity>();

				flags.Add(new SearchFlagEntity
				{
					ColumnName = "A.MUSTKEYINCOMMENT",
					Value = FlagCheckValue.Indeterminate,
					EnableThreeState = true,
					Text = Resources.MustKeyInComment,
					ControlsText = true,
					BitColumn = true
				});
				flags.Add(new SearchFlagEntity
				{
					ColumnName = "A.SCALEITEM",
					Value = FlagCheckValue.Indeterminate,
					EnableThreeState = true,
					Text = Resources.ScaleItem,
					ControlsText = true,
					BitColumn = true
				});
				flags.Add(new SearchFlagEntity
				{
					ColumnName = "A.ZEROPRICEVALID",
					Value = FlagCheckValue.Indeterminate,
					EnableThreeState = true,
					Text = Resources.ZeroPriceValid,
					ControlsText = true,
					BitColumn = true
				});
				flags.Add(new SearchFlagEntity
				{
					ColumnName = "A.QTYBECOMESNEGATIVE",
					Value = FlagCheckValue.Indeterminate,
					EnableThreeState = true,
					Text = Resources.QuantityBecomesNegative,
					ControlsText = true,
					BitColumn = true
				});
				flags.Add(new SearchFlagEntity
				{
					ColumnName = "A.NODISCOUNTALLOWED",
					Value = FlagCheckValue.Indeterminate,
					EnableThreeState = true,
					Text = Resources.NoDiscountAllowed,
					ControlsText = true,
					BitColumn = true
				});
				flags.Add(new SearchFlagEntity
				{
					ColumnName = "A.MUSTSELECTUOM",
					Value = FlagCheckValue.Indeterminate,
					EnableThreeState = true,
					Text = Resources.MustSelectUnitOfMeasure,
					ControlsText = true,
					BitColumn = true
				});
				flags.Add(new SearchFlagEntity
				{
					ColumnName = "GETDATE() < A.DATETOACTIVATEITEM or (GETDATE() >A.DATETOBEBLOCKED and A.DATETOBEBLOCKED >1)",
					Value = FlagCheckValue.Indeterminate,
					EnableThreeState = true,
					Text = Resources.Inactive,
					ControlsText = true,
					BitColumn = false,
					UncheckedValue = "GETDATE() > A.DATETOACTIVATEITEM and  (GETDATE() <A.DATETOBEBLOCKED or A.DATETOBEBLOCKED <1)"
				});
				flags.Add(new SearchFlagEntity
				{
					ColumnName = "A.DELETED",
					Value = FlagCheckValue.Unchecked,
					EnableThreeState = false,
					Text = Resources.Deleted,
					ControlsText = false,
					BitColumn = true
				});
				flags.Add(new SearchFlagEntity
				{
					ColumnName = "A.RETURNABLE",
					Value = FlagCheckValue.Indeterminate,
					EnableThreeState = true,
					Text = Resources.Returnable,
					ControlsText = true,
					BitColumn = true
				});
				flags.Add(new SearchFlagEntity
				{
					ColumnName = "A.CANBESOLD",
					Value = FlagCheckValue.Indeterminate,
					EnableThreeState = true,
					Text = Resources.CanBeSold,
					ControlsText = true,
					BitColumn = true
				});

				return flags;
			}
		}

		private List<T> AdvancedSearchOptimized<T>(
			IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			SortEnum secondarySort,
			bool doCount,
			out int totalRecordsMatching,
			ColumnPopulation populationMethod,
			List<string> idOrDescription = null,
			bool idOrDescriptionBeginsWith = true,
			bool includeVariants = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null, //TODO delete this one
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			string culture = null,
			string variantDescription = null,
			bool VariantDescriptionbeginsWith = false,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false,
			List<SearchFlagEntity> searchFlags = null,
			bool includeHeaders = true,
			bool includeServiceItems = true,
			ItemTypeEnum? itemTypeFilter = null, 
			bool excludeItemswithSerialNumber = false,
			bool includeCannotBeSold = true,
			bool isSearchBarControlSearch = false)
		{
			List<TableColumn> columns = new List<TableColumn>();
			List<Condition> conditions = new List<Condition>();
			List<Join> joins = new List<Join>();
			List<Condition> externalConditions = new List<Condition>();

			string defaultSort = populationMethod == ColumnPopulation.IDOnly ? "ITEMID" : "ITEMNAME";

			using (var cmd = entry.Connection.CreateCommand())
			{
				columns.AddRange(SelectionColumns[populationMethod]);

				if (populationMethod == ColumnPopulation.SiteManager)
				{
					joins.AddRange(itemJoins);
				}
				if (populationMethod == ColumnPopulation.Simple)
				{
					joins.AddRange(simpleJoins);
				}
				if (doCount &&
					(entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
					 entry.Connection.DatabaseVersion == ServerVersion.Unknown))
				{
					columns.Add(new TableColumn
					{
						ColumnName =
							string.Format("ROW_NUMBER() OVER(order by {0}{1})",
								sort == SortEnum.None ? defaultSort : SortColumns[sort].ToSortString(),
								secondarySort == SortEnum.None ? string.Empty : "," + SortColumns[secondarySort].ToSortString()),
						ColumnAlias = "ROW"
					});
					columns.Add(new TableColumn
					{
						ColumnName =
							string.Format(
								"COUNT(1) OVER ( ORDER BY {0}{1} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
								sort == SortEnum.None ? defaultSort : SortColumns[sort].ToSortString(),
								secondarySort == SortEnum.None ? string.Empty : "," + SortColumns[secondarySort].ToSortString()),
						ColumnAlias = "ROW_COUNT"
					});
					externalConditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "ss.ROW between @rowFrom and @rowTo"
					});
					MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
					MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
				}

				if (!includeVariants)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = " HEADERITEMID is null " });
				}
				if (!includeCannotBeSold)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = " CANBESOLD <> 0 " });
				}
				if (!includeHeaders)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = " ITEMTYPE <> 3 " });
				}
				if (!includeServiceItems)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = " ITEMTYPE <> 2 " });
				}

				if (itemTypeFilter != null)
				{
					if (itemTypeFilter == ItemTypeEnum.Item)
					{
						conditions.Add(new Condition { Operator = "AND", ConditionValue = " ITEMTYPE <> 2 AND ITEMTYPE <> 4 " });
					}
					else
					{
						conditions.Add(new Condition { Operator = "AND", ConditionValue = " ITEMTYPE = @itemType" });
						MakeParam(cmd, "itemType", (int)itemTypeFilter, SqlDbType.Int);
					}
				}

				if (idOrDescription != null && idOrDescription.Count == 1 && !string.IsNullOrEmpty(idOrDescription[0]))
				{
					string searchString = PreProcessSearchText(idOrDescription[0], true, idOrDescriptionBeginsWith);

					Condition nameSearchCondition = new Condition
					{
						Operator = "AND",
						ConditionValue = " ( {0} or A.ITEMID Like @description or A.SEARCHKEYWORDS Like @description) "
					};

					string variantSearchCondition = "";

					if (isSearchBarControlSearch)
					{
						if (string.IsNullOrEmpty(variantDescription))
						{
							variantSearchCondition = includeVariants ? "(A.ITEMNAME LIKE @description OR A.NAMEALIAS LIKE @description) OR (A.HEADERITEMID IS NOT NULL AND A.VARIANTNAME LIKE @description)"
																	: "A.ITEMNAME LIKE @description OR A.NAMEALIAS LIKE @description";
						}
						else
						{
							variantSearchCondition = "(A.HEADERITEMID IS NOT NULL AND (A.ITEMNAME LIKE @description OR A.NAMEALIAS LIKE @description))";
						}
					}
                    else
                    {
						variantSearchCondition = includeVariants ? "(A.HEADERITEMID IS NULL AND (A.ITEMNAME LIKE @description OR A.NAMEALIAS LIKE @description)) OR (A.HEADERITEMID IS NOT NULL AND A.VARIANTNAME LIKE @description)"
												: "A.ITEMNAME LIKE @description OR A.NAMEALIAS LIKE @description";
					}

					nameSearchCondition.ConditionValue = string.Format(nameSearchCondition.ConditionValue, variantSearchCondition);

					conditions.Add(nameSearchCondition);

					MakeParam(cmd, "description", searchString);
				}
				else if (idOrDescription != null && idOrDescription.Count > 1)
				{
					List<Condition> searchConditions = new List<Condition>();
					for (int index = 0; index < idOrDescription.Count; index++)
					{
						var searchToken = PreProcessSearchText(idOrDescription[index], true, idOrDescriptionBeginsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							searchConditions.Add(new Condition
							{
								ConditionValue =
									$@" (A.ITEMNAME Like @description{index
										} 
										or A.ITEMID Like @description{index
										} 
										or A.VARIANTNAME Like @description{
										index
										} 
										or A.NAMEALIAS Like @description{
										index}) ",
								Operator = "AND"

							});

							MakeParam(cmd, $"description{index}", searchToken);
						}
					}
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue =
							$" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
					});
				}
				if (retailGroupID != null)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.RETAILGROUPMASTERID = @retailGroup " });

					MakeParam(cmd, "retailGroup", (Guid)retailGroupID, SqlDbType.UniqueIdentifier);
				}

				if (retailDepartmentID != null)
				{
					if (!joins.Exists(x => x.TableAlias == "IIR"))
					{
						joins.Add(
							new Join
							{
								Condition = " A.RETAILGROUPMASTERID = iir.MASTERID AND IIR.DELETED =0",
								JoinType = "LEFT OUTER",
								Table = "RETAILGROUP",
								TableAlias = "IIR"
							});
					}
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "IIR.DEPARTMENTMASTERID = @retailDepartment "
					});

					MakeParam(cmd, "retailDepartment", (Guid)retailDepartmentID, SqlDbType.UniqueIdentifier);
				}

				if (taxGroupID != null)
				{
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "A.SALESTAXITEMGROUPID = @taxGroup "
					});

					MakeParamNoCheck(cmd, "taxGroup", (string)taxGroupID);
				}

				if (vendorID != null)
				{
					joins.Add(new Join
					{
						Condition = "VI.RETAILITEMID = A.ITEMID",
						JoinType = "INNER",
                        Table = "(SELECT DISTINCT RETAILITEMID FROM VENDORITEMS WHERE VENDORID = @vendorID)",
                        TableAlias = "VI"
					});

					MakeParamNoCheck(cmd, "vendorID", (string)vendorID);
				}

				if (barCode != null)
				{
					joins.Add(new Join
					{
						Condition = "ibarcode.ITEMBARCODE = (SELECT MIN(ITEMBARCODE) FROM INVENTITEMBARCODE WHERE ITEMID = A.ITEMID AND DELETED = 0 AND ITEMBARCODE LIKE @barCodeSearchString)",
						JoinType = "LEFT OUTER",
						Table = "INVENTITEMBARCODE",
						TableAlias = "ibarcode"
					});

                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ibarcode.ITEMID IS NOT NULL "
                    });

                    barCode = (barCodeBeginsWith ? "" : "%") + barCode + "%";
					MakeParamNoCheck(cmd, "barCodeSearchString", barCode);
                }

				if (specialGroup != null)
				{
					joins.Add(new Join
					{
						Condition = "SG.MEMBERMASTERID = A.MASTERID AND SG.GROUPID = @specialGroupID",
						JoinType = "INNER",
						Table = "SPECIALGROUPITEMS",
						TableAlias = "SG"
					});

					MakeParamNoCheck(cmd, "specialGroupID", (string)specialGroup);
				}
				if (culture != null && culture != "" && (idOrDescription != null && idOrDescription.Count > 0))
				{
					string searchString = string.Empty;
					for (int index = 0; index < idOrDescription.Count; index++)
					{
						string token = idOrDescription[index];
						searchString += token;
						if (index < idOrDescription.Count - 1)
						{
							searchString += " ";
						}
					}
					joins.Add(new Join
					{
						Table = "RBOINVENTTRANSLATIONS",
						Condition =
							"tr.ITEMID = it.ITEMID and tr.CULTURENAME = @culture",
						JoinType = "LEFT OUTER",
						TableAlias = "tr"
					});
					MakeParamNoCheck(cmd, "culture", culture);
					MakeParamNoCheck(cmd, "searchString", searchString);
					conditions.Add(new Condition
					{
						Operator = "OR",
						ConditionValue = " tr.DESCRIPTION like @searchString "
					});
				}
				if (!string.IsNullOrEmpty(variantDescription))
				{
					string searchString = PreProcessSearchText(variantDescription, true, VariantDescriptionbeginsWith);
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue =
							" A.VARIANTNAME Like @variantDescription "
					});

					MakeParam(cmd, "variantDescription", searchString);
				}

				if (attributeDescription != null && attributeDescription.Count == 1 && !string.IsNullOrEmpty(attributeDescription[0]))
				{
					List<Condition> searchConditions = new List<Condition>();

					joins.Add(new Join
					{
						Table = "RETAILITEMDIMENSIONATTRIBUTE",
						Condition = "A.MASTERID = rda.RETAILITEMID",
						JoinType = "",
						TableAlias = "rda"
					});

					string searchString = PreProcessSearchText(attributeDescription[0], true, attributeDescriptionStartsWith);
					searchConditions.Add(new Condition
					{
						ConditionValue = "da.ID = rda.DIMENSIONATTRIBUTEID",
						Operator = "AND"
					});
					searchConditions.Add(new Condition
					{
						ConditionValue = "(da.description like @attributeDescription OR da.CODE like @attributeDescription) ",
						Operator = "AND"
					});

					joins.Add(new Join
					{
						Table = "DIMENSIONATTRIBUTE",
						Condition = QueryPartGenerator.ConditionGenerator(searchConditions, true),
						JoinType = "",
						TableAlias = "da"
					});
					MakeParam(cmd, "attributeDescription", searchString);
				}
				else if (attributeDescription != null && attributeDescription.Count > 1)
				{
					for (int index = 0; index < attributeDescription.Count; index++)
					{
						List<Condition> searchConditions = new List<Condition>();

						var searchToken = PreProcessSearchText(attributeDescription[index], true, attributeDescriptionStartsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							joins.Add(new Join
							{
								Table = "RETAILITEMDIMENSIONATTRIBUTE",
								Condition = $"A.MASTERID = rda{index}.RETAILITEMID",
								JoinType = "",
								TableAlias = $"rda{index}"
							});
							searchConditions.Add(new Condition
							{
								ConditionValue = $"da{index}.ID = rda{index}.DIMENSIONATTRIBUTEID",
								Operator = "AND"
							});
							searchConditions.Add(new Condition
							{
								ConditionValue = $"(da{index}.description like @attributeDescription{index} OR da{index}.CODE like @attributeDescription{index}) ",
								Operator = "AND"
							});

							MakeParam(cmd, $"attributeDescription{index}", searchToken);

							joins.Add(new Join
							{
								Table = "DIMENSIONATTRIBUTE",
								Condition = QueryPartGenerator.ConditionGenerator(searchConditions, true),
								JoinType = "",
								TableAlias = $"da{index}"
							});
						}
					}
				}
				if (excludeItemswithSerialNumber)
				{
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
						ConditionValue = " A.KEYINSERIALNUMBER = 0 "
                    });
				}

				if (searchFlags == null)
				{
					searchFlags = ItemSearchFlags;
				}
				QueryPartGenerator.AddSearchFlagsToCondition(searchFlags, conditions);

				if (sort == SortEnum.RetailGroup
					|| sort == SortEnum.RetailGroupDesc
					|| secondarySort == SortEnum.RetailGroup
					|| secondarySort == SortEnum.RetailGroupDesc)
				{
					if (!joins.Exists(x => x.TableAlias == "IIR"))
					{
						joins.Add(
							new Join
							{
								Condition = " A.RETAILGROUPMASTERID = iir.MASTERID AND IIR.DELETED =0",
								JoinType = "LEFT OUTER",
								Table = "RETAILGROUP",
								TableAlias = "IIR"
							});
					}

					columns.Add(new TableColumn
					{
						ColumnAlias = "RetailGroupName",
						ColumnName = "Name",
						TableAlias = "iir"
					});
				}
				else if (sort == SortEnum.RetailDepartment
					|| sort == SortEnum.RetailDepartmentDesc
					|| secondarySort == SortEnum.RetailDepartment
					|| secondarySort == SortEnum.RetailDepartmentDesc)
				{
					if (!joins.Exists(x => x.TableAlias == "IIR"))
					{
						joins.Add(
							new Join
							{
								Condition = " A.RETAILGROUPMASTERID = iir.MASTERID AND IIR.DELETED =0",
								JoinType = "LEFT OUTER",
								Table = "RETAILGROUP",
								TableAlias = "IIR"
							});
					}
					if (!joins.Exists(x => x.TableAlias == "IID"))
					{
						joins.Add(new Join
						{
							Condition = " IIR.DEPARTMENTMASTERID = IID.MASTERID AND IID.DELETED =0",
							JoinType = "LEFT OUTER",
							Table = "RETAILDEPARTMENT",
							TableAlias = "IID"
						});
					}

					columns.Add(new TableColumn
					{
						ColumnAlias = "RetailDepartmentName",
						ColumnName = "Name",
						TableAlias = "iid"
					});
				}
				else if (sort == SortEnum.TaxGroup
					|| sort == SortEnum.TaxGroupDesc
					|| secondarySort == SortEnum.TaxGroup
					|| secondarySort == SortEnum.TaxGroupDesc)
				{
					joins.Add(new Join
					{
						Condition = " A.SALESTAXITEMGROUPID = tgh.TAXITEMGROUP",
						JoinType = "LEFT OUTER",
						Table = "TAXITEMGROUPHEADING",
						TableAlias = "tgh"
					});
					columns.Add(new TableColumn { ColumnAlias = "TaxGroupName", ColumnName = "Name", TableAlias = "tgh" });
				}

				string sortstring = string.Format("{0}{1}",
					sort == SortEnum.None ? defaultSort : SortColumns[sort].ToSortString(true),
					secondarySort == SortEnum.None ? string.Empty : "," + SortColumns[secondarySort].ToSortString(true)
					);

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEM", "A", "ss"),
					QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(joins),
					QueryPartGenerator.ConditionGenerator(conditions),
					QueryPartGenerator.ConditionGenerator(externalConditions),
					string.Format("ORDER BY {0}", sortstring));

				totalRecordsMatching = 0;

				switch (populationMethod)
				{
					case ColumnPopulation.IDOnly:
						List<RecordIdentifier> recordidentifiers;
						if (doCount)
						{
							recordidentifiers = Execute<RecordIdentifier, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateItemIDWithCount);
						}
						else
						{
							recordidentifiers = Execute<RecordIdentifier>(entry, cmd, CommandType.Text, PopulateItemID);
						}
						return CollectionHelper.ForceConvertList<T, RecordIdentifier>(recordidentifiers);

					case ColumnPopulation.POS:
						throw new NotImplementedException();

					case ColumnPopulation.SiteManager:
						List<RetailItem> items;
						if (doCount)
						{
							items = Execute<RetailItem, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateItemWithCount);
						}
						else
						{
							items = Execute<RetailItem>(entry, cmd, CommandType.Text, PopulateItem);
						}

						return CollectionHelper.ForceConvertList<T, SimpleRetailItem>(items);

					case ColumnPopulation.Simple:
						List<SimpleRetailItem> simpleItems;
						if (doCount)
						{
							simpleItems = Execute<SimpleRetailItem, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateSimpleItemWithCount);
						}
						else
						{
							simpleItems = Execute<SimpleRetailItem>(entry, cmd, CommandType.Text, PopulateSimpleItem);
						}

						return CollectionHelper.ForceConvertList<T, SimpleRetailItem>(simpleItems);

					case ColumnPopulation.DataEntity:
						List<DataEntity> entities;
						if (doCount)
						{
							entities = Execute<DataEntity, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateDataEntityWithCount);
						}
						else
						{
							entities = Execute<DataEntity>(entry, cmd, CommandType.Text, PopulateDataEntity);
						}

						return CollectionHelper.ForceConvertList<T, DataEntity>(entities);

					case ColumnPopulation.MasterDataEntity:
						List<MasterIDEntity> masterEntities;
						if (doCount)
						{
							masterEntities = Execute<MasterIDEntity, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateMasterDataEntityWithCount);
						}
						else
						{
							masterEntities = Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateMasterDataEntity);
						}

						return CollectionHelper.ForceConvertList<T, MasterIDEntity>(masterEntities);
				}
			}
			return null;
		}

		public virtual List<DataEntity> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo,
			string culture, bool beginsWith, bool includeVariants = true, bool includeServiceItems = true)
		{
			int totalRecordsMatched;
			return AdvancedSearchOptimized<DataEntity>(entry, rowFrom, rowTo, SortEnum.None, SortEnum.None, true,
				out totalRecordsMatched,
				ColumnPopulation.DataEntity, new List<string> { searchString }, beginsWith, includeVariants,
				null, null, null, null, null, null, false, null, culture, includeServiceItems: includeServiceItems);

		}

		public List<RetailItem> AdvancedSearchFull(IConnectionManager entry,
            int rowFrom,
	        int rowTo,
	        SortEnum sort,
	        out int totalRecordsMatching,
			string idOrDescription = null,
			bool idOrDescriptionBeginsWith = true,
			bool includeHeaders = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null, // TODO delete this one
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			string variantDescription = null,
			bool variantDescriptionBeginsWith = false,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false,
			List<SearchFlagEntity> searchFlags = null,
			bool excludeItemswithSerialNumber = false)
		{
			return AdvancedSearchOptimized<RetailItem>(
				 entry,
				 rowFrom,
				 rowTo,
				 sort,
				 SortEnum.None,
				 true,
				 out totalRecordsMatching,
				 ColumnPopulation.SiteManager,
				 new List<string> { idOrDescription },
				 idOrDescriptionBeginsWith,
				 includeVariants,
				 retailGroupID,
				 retailDepartmentID,
				 taxGroupID,
				 variantGroupID,
				 vendorID,
				 barCode,
				 barCodeBeginsWith,
				 specialGroup,
				 null,
				 variantDescription,
				 variantDescriptionBeginsWith,
				 attributeDescription,
				 attributeDescriptionStartsWith,
				 searchFlags,
				 includeHeaders,
				 excludeItemswithSerialNumber:excludeItemswithSerialNumber
				);
		}

		public List<SimpleRetailItem> AdvancedSearch(IConnectionManager entry,
			int rowFrom,
			int rowTo,
			SortEnum sort,
			out int totalRecordsMatching,
			string idOrDescription = null,
			bool idOrDescriptionBeginsWith = true,
			bool includeHeaders = true,
			RecordIdentifier retailGroupID = null,
			RecordIdentifier retailDepartmentID = null,
			RecordIdentifier taxGroupID = null,
			RecordIdentifier variantGroupID = null, //TODO delete this one
			RecordIdentifier vendorID = null,
			string barCode = null,
			bool barCodeBeginsWith = true,
			RecordIdentifier specialGroup = null,
			bool includeVariants = true,
			string variantDescription = null,
			bool variantDescriptionBeginsWith = false,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false,
			List<SearchFlagEntity> searchFlags = null,
			SortEnum secondarySearch = SortEnum.None,
			ItemTypeEnum? itemTypeFilter = null,
			bool excludeItemswithSerialNumber = false,
			bool includeCannotBeSold = true,
			bool isSearchBarControlSearch = false
			)
		{
			return AdvancedSearchOptimized<SimpleRetailItem>(
				entry,
				rowFrom,
				rowTo,
				sort,
				secondarySearch,
				true,
				out totalRecordsMatching,
				ColumnPopulation.Simple,
				new List<string> { idOrDescription },
				idOrDescriptionBeginsWith,
				includeVariants,
				retailGroupID,
				retailDepartmentID,
				taxGroupID,
				variantGroupID,
				vendorID,
				barCode,
				barCodeBeginsWith,
				specialGroup,
				null,
				variantDescription,
				variantDescriptionBeginsWith,
				attributeDescription,
				attributeDescriptionStartsWith,
				searchFlags,
				includeHeaders,
				true,
				itemTypeFilter,
				excludeItemswithSerialNumber,
				includeCannotBeSold,
				isSearchBarControlSearch);
		}

        /// <summary>
        /// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="SimpleRetailItem"/> . 
        /// Additionally this search function accepts a Dictionary with a combination of <see cref="RetailItemSearchEnum"/> and a string and resolves this list to add additional 
        /// search filters to the query
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The item id or item name to search for</param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="beginsWith">Indicates wether you only want to search for items that begin with the given item name or item id</param>
        /// <param name="sort">The name of the column you want to sort by</param>
        /// <param name="advancedSearchParameters">Additional search parameters</param>
        /// <param name="includeVariants"></param>
        /// <returns></returns>
        public virtual List<SimpleRetailItem> AdvancedSearch(
			IConnectionManager entry,
			string searchString,
			int rowFrom,
			int rowTo,
			bool beginsWith,
			SortEnum sort,
			Dictionary<RetailItemSearchEnum, string> advancedSearchParameters,
			bool includeVariants = true)
		{
			RecordIdentifier retailGroupID = null;
			RecordIdentifier retailDepartmentID = null;
			RecordIdentifier taxGroupID = null;
			RecordIdentifier vendorID = null;
			string barCode = null;
			bool barCodeBeginsWith = true;
			RecordIdentifier specialGroup = null;

			// Go through the advanced search conditions
			foreach (var pair in advancedSearchParameters)
			{
				switch (pair.Key)
				{
					case RetailItemSearchEnum.RetailGroup:
						retailGroupID = pair.Value;
						break;

					case RetailItemSearchEnum.RetailDepartment:
						retailDepartmentID = pair.Value;
						break;

					case RetailItemSearchEnum.TaxGroup:
						taxGroupID = pair.Value;
						break;
						
					case RetailItemSearchEnum.Vendor:
						vendorID = pair.Value;
						break;

					case RetailItemSearchEnum.BarCode:
						barCode = pair.Value;
						break;

					case RetailItemSearchEnum.SpecialGroup:
						specialGroup = pair.Value;
						break;
				}
			}

			int totalRecordsMatching;

			return AdvancedSearchOptimized<SimpleRetailItem>(
				entry,
				rowFrom,
				rowTo,
				sort,
				SortEnum.None,
				false,
				out totalRecordsMatching,
				ColumnPopulation.Simple,
				new List<string> { searchString },
				beginsWith,
				includeVariants,
				retailGroupID,
				retailDepartmentID,
				taxGroupID,
				null,
				vendorID,
				barCode,
				barCodeBeginsWith,
				specialGroup);
		}

		public virtual List<DataEntity> GetForecourtItems(IConnectionManager entry, RecordIdentifier gradeID)
		{
			ValidateSecurity(entry);
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"Select 
										R.[ITEMID], 
										R.[ITEMNAME] 
									FROM 
										RETAILITEM R
									WHERE 
										R.GRADEID=@gradeID";
				MakeParam(cmd, "gradeID", gradeID);
				return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
			}
		}

		public RetailItemPrice GetItemPrice(IConnectionManager entry, RecordIdentifier itemID)
		{
			RetailItemPrice result;

			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);
				List<TableColumn> columns = new List<TableColumn>();

				columns.Add(new TableColumn { ColumnName = "MASTERID", TableAlias = "A" });
				columns.Add(new TableColumn { ColumnName = "PROFITMARGIN", TableAlias = "A" });
				columns.Add(new TableColumn { ColumnName = "SALESTAXITEMGROUPID", TableAlias = "A" });
				columns.Add(new TableColumn { ColumnName = "SALESPRICE", TableAlias = "A" });
				columns.Add(new TableColumn { ColumnName = "SALESPRICEINCLTAX", TableAlias = "A" });
				columns.Add(new TableColumn { ColumnName = "PURCHASEPRICE", TableAlias = "A" });
				List<Condition> conditions = new List<Condition>();

				// Handle both the old item ID and the new master ID
				if (itemID.IsGuid)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.MASTERID = @itemID" });
					MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @itemID" });
					MakeParam(cmd, "itemID", (string)itemID);
				}

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					string.Empty,
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);


				var records = Execute<RetailItemPrice>(entry, cmd, CommandType.Text, PopulateRetailItemPrice);

				result = (records.Count > 0) ? records[0] : null;
			}

			return result ?? new RetailItemPrice();
		}

		/// <summary>
		/// Gets a retail item from the database by a given ID.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">The id of the retail item to fetch. This can be either the ID or the Master ID, its resolved depending on if the RecordIdentifier contains GUID or string</param>
		/// <param name="cacheType">Cache</param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual RetailItem Get(IConnectionManager entry, RecordIdentifier itemID, CacheType cacheType = CacheType.CacheTypeNone)
		{
			if (string.IsNullOrEmpty(itemID.StringValue))
			{
				return null;
			}

			RetailItem result;
			if (cacheType != CacheType.CacheTypeNone)
			{
				result = (RetailItem)entry.Cache.GetEntityFromCache(typeof(RetailItem), itemID);
				if (result != null)
				{
					return result;
				}
			}

			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);
				List<TableColumn> columns = new List<TableColumn>();
				foreach (var selectionColumn in SelectionColumns[ColumnPopulation.SiteManager])
				{
					columns.Add(selectionColumn);
				}
				List<Condition> conditions = new List<Condition>();

				// Handle both the old item ID and the new master ID
				if (itemID.IsGuid)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.MASTERID = @itemID" });
					MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @itemID" });
					MakeParam(cmd, "itemID", (string)itemID);
				}

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0" });

				AddCustomHandling(ref columns, ref conditions);

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(itemJoins),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);

				var records = Execute<RetailItem>(entry, cmd, CommandType.Text, PopulateItem);

				result = (records.Count > 0) ? records[0] : null;
			}

			if (result != null && cacheType != CacheType.CacheTypeNone)
			{
				entry.Cache.AddEntityToCache(itemID, result, cacheType);
			}

			return result;
		}

		/// <summary>
		/// Gets a retail item from the database by a given ID.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">The id of the retail item to fetch. This can be either the ID or the Master ID, its resolved depending on if the RecordIdentifier contains GUID or string</param>
		/// <param name="evenIfDeleted"></param>
		/// <param name="cacheType">Cache</param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual RetailItem Get(IConnectionManager entry, RecordIdentifier itemID, bool evenIfDeleted, CacheType cacheType = CacheType.CacheTypeNone)
		{
			RetailItem result;
			if (cacheType != CacheType.CacheTypeNone)
			{
				result = (RetailItem)entry.Cache.GetEntityFromCache(typeof(RetailItem), itemID);
				if (result != null)
				{
					return result;
				}
			}

			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);
				List<TableColumn> columns = new List<TableColumn>();
				foreach (var selectionColumn in SelectionColumns[ColumnPopulation.SiteManager])
				{
					columns.Add(selectionColumn);
				}
				List<Condition> conditions = new List<Condition>();

				// Handle both the old item ID and the new master ID
				if (itemID.IsGuid)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.MASTERID = @itemID" });
					MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @itemID" });
					MakeParam(cmd, "itemID", (string)itemID);
				}

				if (!evenIfDeleted)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0" });
				}

				AddCustomHandling(ref columns, ref conditions);

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(itemJoins),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);

				var records = Execute<RetailItem>(entry, cmd, CommandType.Text, PopulateItem);

				result = (records.Count > 0) ? records[0] : null;
			}

			if (result != null && cacheType != CacheType.CacheTypeNone)
			{
				entry.Cache.AddEntityToCache(itemID, result, cacheType);
			}

			return result;
		}

		public virtual List<InventoryRetailItem> GetInventoryRetailItems(IConnectionManager entry, List<RecordIdentifier> itemIDs, List<RecordIdentifier> barcodes, RecordIdentifier storeID, bool includeInventoryOnHand)
		{
			if (itemIDs.Count == 0 && barcodes.Count == 0)
			{
				return new List<InventoryRetailItem>();
			}

			if (includeInventoryOnHand && RecordIdentifier.IsEmptyOrNull(storeID)) throw new ArgumentNullException(nameof(storeID));

			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				List<TableColumn> columns = new List<TableColumn>
				{
					new TableColumn {ColumnName = "ITEMID", TableAlias = "R"},
					new TableColumn {ColumnName = "ITEMNAME", TableAlias = "R"},
					new TableColumn {ColumnName = "MASTERID", TableAlias = "R"},
					new TableColumn {ColumnName = "HEADERITEMID", TableAlias = "R"},
					new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "R"},
					new TableColumn {ColumnName = "INVENTORYUNITID", TableAlias = "R"},
					new TableColumn {ColumnName = "ITEMTYPE", TableAlias = "R"},
					new TableColumn {ColumnName = "DELETED", TableAlias = "R"},
					new TableColumn {ColumnName = "BARCODE", TableAlias = "ItemIDs"}
				};

				List<Join> joins = new List<Join>
				{
					new Join {JoinType = "INNER", Table = "ItemIDs", Condition = "R.ITEMID = ItemIDs.ID"}
				};

				List<Condition> conditions = new List<Condition>();

				cmd.CommandText = $@";WITH ItemIDs (ID, BARCODE) AS
									(
										SELECT COALESCE(T1.ID, T2.ID), COALESCE(T2.BARCODE, T1.BARCODE) FROM
										(SELECT V.ID, '' AS BARCODE FROM (VALUES ('{string.Join("'), ('", itemIDs)}')) AS V (ID)) AS T1
										FULL OUTER JOIN 
										(SELECT IB.ITEMID AS ID, IB.ITEMBARCODE AS BARCODE FROM INVENTITEMBARCODE IB WHERE IB.DELETED = 0 AND IB.ITEMBARCODE IN ('{string.Join("','", barcodes)}')) AS T2 ON T1.ID = T2.ID
									)
									";

				if(includeInventoryOnHand)
				{
					columns.Add(new TableColumn { ColumnName = "ISNULL(INS.QUANTITY, 0)", ColumnAlias = "QUANTITY" });

					conditions.Add(new Condition { Operator = "AND", ConditionValue = $"RST.STOREID = '{storeID.StringValue}'" });

					joins.Add(new Join { JoinType = "CROSS", Table = "RBOSTORETABLE", TableAlias = "RST" });
					joins.Add(new Join { JoinType = "LEFT OUTER", Table = "VINVENTSUM", TableAlias = "INS", Condition = "INS.ITEMID = R.ITEMID AND INS.STOREID = RST.STOREID" });
				}
				else
				{
					columns.Add(new TableColumn { ColumnName = "0.0", ColumnAlias = "QUANTITY" });
				}

				joins.Add(new Join { JoinType = "LEFT OUTER", Table = "UNIT", TableAlias = "U", Condition = "R.INVENTORYUNITID = U.UNITID" });

				cmd.CommandText += string.Format(QueryTemplates.BaseQuery("RETAILITEM", "R"),
									QueryPartGenerator.InternalColumnGenerator(columns),
									QueryPartGenerator.JoinGenerator(joins),
									QueryPartGenerator.ConditionGenerator(conditions),
									string.Empty);

				return Execute<InventoryRetailItem>(entry, cmd, CommandType.Text, PopulateInventoryRetailItem);
			}
		}

		private void PopulateInventoryRetailItem(IDataReader dr, InventoryRetailItem item)
		{
			item.ID = (string)dr["ITEMID"];
			item.MasterID = (Guid)dr["MASTERID"];
			item.HeaderItemID = dr["HEADERITEMID"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["HEADERITEMID"];
			item.Text = (string)dr["ITEMNAME"];
			item.VariantName = (string)dr["VARIANTNAME"];
			item.InventoryUnitID = (string)dr["INVENTORYUNITID"];
			item.InventoryOnHand = (decimal)dr["QUANTITY"];
			item.ItemType = (ItemTypeEnum)((byte)dr["ITEMTYPE"]);
			item.Deleted = (bool)dr["DELETED"];
			item.Barcode = (string)dr["BARCODE"];
		}

		/// <summary>
		/// Gets retail items in the system for a specific ID using a LIKE query
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">Item ID</param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual List<RetailItem> GetItemsByItemPattern(IConnectionManager entry, string itemID)
		{
			// TODO This is redundant
			List<RetailItem> records;

			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);
				List<TableColumn> columns = new List<TableColumn>();
				foreach (var selectionColumn in SelectionColumns[ColumnPopulation.SiteManager])
				{
					columns.Add(selectionColumn);
				}
				List<Condition> conditions = new List<Condition>();

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID LIKE @itemID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

				AddCustomHandling(ref columns, ref conditions);
				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);

				MakeParam(cmd, "itemId", itemID + "%");

				records = Execute<RetailItem>(entry, cmd, CommandType.Text, PopulateItem);
			}

			return records ?? new List<RetailItem>();
		}

		/// <summary>
		/// Gets all variants for a header item as a list of DataEntity
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID"></param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual List<DataEntity> GetItemVariantList(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);
				List<TableColumn> columns = new List<TableColumn>();
				foreach (var selectionColumn in SelectionColumns[ColumnPopulation.DataEntity])
				{
					columns.Add(selectionColumn);
				}
				List<Condition> conditions = new List<Condition>();

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.HEADERITEMID = @itemID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

				AddCustomHandling(ref columns, ref conditions);
				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(itemJoins),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);
				MakeParam(cmd, "itemID", (string)itemID);

				return Execute<DataEntity>(entry, cmd, CommandType.Text, PopulateDataEntity);
			}
		}

		/// <summary>
		/// Gets all variants for a header item as a list of DataEntity
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID"></param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual List<MasterIDEntity> GetItemVariantMasterIDList(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				List<TableColumn> columns = new List<TableColumn>();

				foreach (var selectionColumn in SelectionColumns[ColumnPopulation.MasterDataEntity])
				{
					columns.Add(selectionColumn);
				}

				List<Condition> conditions = new List<Condition>();

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.HEADERITEMID = @itemID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

				AddCustomHandling(ref columns, ref conditions);
				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(itemJoins),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);
				MakeParam(cmd, "itemID", (string)itemID);

				return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateMasterDataEntity);
			}
		}

		/// <summary>
		/// Gets all variants for a header item as a list of MasterDataEntity
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID"></param>
		/// <param name="showDeleted"></param>
		/// <param name="sort"></param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual List<SimpleRetailItem> GetItemVariants(IConnectionManager entry,
			RecordIdentifier itemID,
			SortEnum sort,
			bool showDeleted = false)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);
				List<TableColumn> columns = new List<TableColumn>(SelectionColumns[ColumnPopulation.Simple]);

				List<Condition> conditions = new List<Condition>();

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.HEADERITEMID = @itemID" });
				if (!showDeleted)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });
				}
				List<Join> joins = new List<Join>(simpleJoins);

				if (sort == SortEnum.RetailGroup || sort == SortEnum.RetailGroupDesc)
				{
					columns.Add(new TableColumn
					{
						ColumnAlias = "RetailGroupName",
						ColumnName = "Name",
						TableAlias = "iir"
					});
				}
				else if (sort == SortEnum.RetailDepartment || sort == SortEnum.RetailDepartmentDesc)
				{
					columns.Add(new TableColumn
					{
						ColumnAlias = "RetailDepartmentName",
						ColumnName = "Name",
						TableAlias = "iid"
					});
				}

				else if (sort == SortEnum.TaxGroup || sort == SortEnum.TaxGroupDesc)
				{
					joins.Add(new Join
					{
						Condition = " A.SALESTAXITEMGROUPID = tgh.TAXITEMGROUP",
						JoinType = "LEFT OUTER",
						Table = "TAXITEMGROUPHEADING",
						TableAlias = "tgh"
					});
					columns.Add(new TableColumn { ColumnAlias = "TaxGroupName", ColumnName = "Name", TableAlias = "tgh" });
				}

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(joins),
					QueryPartGenerator.ConditionGenerator(conditions),
					sort == SortEnum.None
						? string.Empty
						: string.Format("ORDER BY {0}", SortColumns[sort].ToSortString(true)));
				MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);

				return Execute<SimpleRetailItem>(entry, cmd, CommandType.Text, PopulateSimpleItem);
			}
		}

        private string CompoundIDs(List<RecordIdentifier> IDs, bool stringBased = true)
		{
			StringBuilder sb = new StringBuilder("(");
			for (int i = 0; i < IDs.Count; i++)
			{
				var recordIdentifier = IDs[i];
				if (stringBased)
				{
					sb.AppendLine($"'{(string)recordIdentifier}'");
				}
				else
				{
					sb.AppendLine($"{(string)recordIdentifier}");
				}
				if (i < IDs.Count - 1)
				{
					sb.Append(",");
				}
				else
				{

					sb.Append(")");
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Gets all variants for a header item as a list of MasterDataEntity
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID"></param>
		/// <param name="variants"></param>
		/// <param name="showDeleted"></param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual List<SimpleRetailItem> GetSpecificItemVariants(IConnectionManager entry,
			RecordIdentifier itemID,
			List<RecordIdentifier> variants,
			bool showDeleted = false)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);
				List<TableColumn> columns = new List<TableColumn>(SelectionColumns[ColumnPopulation.Simple]);

				List<Condition> conditions = new List<Condition>();
				if (!RecordIdentifier.IsEmptyOrNull(itemID))
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.HEADERITEMID = @itemID" });
				}
				if (!showDeleted)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });
				}
				if (variants.Count > 0)
				{
					string column = string.Empty;
					if (variants[0].IsGuid)
					{
						column = "MASTERID";
					}
					else
					{
						column = "ITEMID";
					}
					conditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = $"A.{column} IN{CompoundIDs(variants)} "
					});
				}

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					 QueryPartGenerator.JoinGenerator(simpleJoins),
					QueryPartGenerator.ConditionGenerator(conditions),
					 string.Empty);
				MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);

				return Execute<SimpleRetailItem>(entry, cmd, CommandType.Text, PopulateSimpleItem);
			}
		}

		/// <summary>
		/// Gets all variants for a header item as list of SimpleRetailItem
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID"></param>
		/// <param name="attributeDescription"></param>
		/// <param name="attributeDescriptionStartsWith"></param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual List<SimpleRetailItem> GetItemVariants(IConnectionManager entry, RecordIdentifier itemID,
			string attributeDescription = null,
			bool attributeDescriptionStartsWith = false)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Join> joins = new List<Join>(simpleJoins);
				if (!string.IsNullOrEmpty(attributeDescription))
				{
					string searchString = PreProcessSearchText(attributeDescription, true, attributeDescriptionStartsWith);
					joins.Add(new Join
					{
						Table = "RETAILITEMDIMENSION",
						Condition =
						   "A.MASTERID = rd.RETAILITEMID",
						JoinType = "",
						TableAlias = "rd"
					});
					List<Condition> attributeConditions = new List<Condition>();
					attributeConditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue =
							"da.DIMENSIONID = rd.ID"
					});
					attributeConditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue =
						   "  da.description like @attributeDescription "
					});
					joins.Add(new Join
					{
						Table = "DIMENSIONATTRIBUTE",
						Condition = QueryPartGenerator.ConditionGenerator(attributeConditions, true),
						JoinType = "",
						TableAlias = "da"
					});

					MakeParam(cmd, "attributeDescription", searchString);
				}

				List<TableColumn> columns = new List<TableColumn>();
				foreach (var selectionColumn in SelectionColumns[ColumnPopulation.Simple])
				{
					columns.Add(selectionColumn);
				}
				List<Condition> conditions = new List<Condition>();

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.HEADERITEMID = @itemID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

				AddCustomHandling(ref columns, ref conditions);
				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(joins),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);
				MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);

				return Execute<SimpleRetailItem>(entry, cmd, CommandType.Text, PopulateSimpleItem);
			}
		}

		/// <summary>
		/// Gets all variants for a header item as list of MasterIDEntity
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID"></param>
		/// <param name="attributeDescription"></param>
		/// <param name="attributeDescriptionStartsWith"></param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual List<MasterIDEntity> GetItemVariantsMasterID(IConnectionManager entry, RecordIdentifier itemID,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.HEADERITEMID = @itemID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });
				List<Join> joins = new List<Join>();
				if (attributeDescription != null && attributeDescription.Count == 1 && !string.IsNullOrEmpty(attributeDescription[0]))
				{
					List<Condition> searchConditions = new List<Condition>();

					joins.Add(new Join
					{
						Table = "RETAILITEMDIMENSIONATTRIBUTE",
						Condition =
				   "A.MASTERID = rda.RETAILITEMID",
						JoinType = "",
						TableAlias = "rda"
					});

					string searchString = PreProcessSearchText(attributeDescription[0], true, attributeDescriptionStartsWith);
					searchConditions.Add(new Condition
					{
						ConditionValue =
								  "da.ID = rda.DIMENSIONATTRIBUTEID",
						Operator = "AND"

					});
					searchConditions.Add(new Condition
					{
						ConditionValue =
							"(da.description like @attributeDescription OR da.CODE like @attributeDescription) ",
						Operator = "AND"

					});

					joins.Add(new Join
					{
						Table = "DIMENSIONATTRIBUTE",
						Condition = QueryPartGenerator.ConditionGenerator(searchConditions, true),
						JoinType = "",
						TableAlias = "da"
					});
					MakeParam(cmd, "attributeDescription", searchString);
				}
				else if (attributeDescription != null && attributeDescription.Count > 1)
				{
					for (int index = 0; index < attributeDescription.Count; index++)
					{
						List<Condition> searchConditions = new List<Condition>();

						var searchToken = PreProcessSearchText(attributeDescription[index], true, attributeDescriptionStartsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							joins.Add(new Join
							{
								Table = "RETAILITEMDIMENSIONATTRIBUTE",
								Condition = $"A.MASTERID = rda{index}.RETAILITEMID",
								JoinType = "",
								TableAlias = $"rda{index}"
							});
							searchConditions.Add(new Condition
							{
								ConditionValue = $"da{index}.ID = rda{index}.DIMENSIONATTRIBUTEID",
								Operator = "AND"
							});
							searchConditions.Add(new Condition
							{
								ConditionValue = $"(da{index}.description like @attributeDescription{index} OR da{index}.CODE like @attributeDescription{index}) ",
								Operator = "AND"
							});

							MakeParam(cmd, $"attributeDescription{index}", searchToken);

							joins.Add(new Join
							{
								Table = "DIMENSIONATTRIBUTE",
								Condition = QueryPartGenerator.ConditionGenerator(searchConditions, true),
								JoinType = "",
								TableAlias = $"da{index}"
							});
						}
					}
				}

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					QueryPartGenerator.InternalColumnGenerator(SelectionColumns[ColumnPopulation.MasterDataEntity]),
					QueryPartGenerator.JoinGenerator(joins),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);
				MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);

				return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateVariant);
			}
		}

		/// <summary>
		/// Gets all variants for a header item as list of MasterIDEntity
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="totalRecordsMatching"></param>
		/// <param name="itemID"></param>
		/// <param name="attributeDescription"></param>
		/// <param name="attributeDescriptionStartsWith"></param>
		/// <param name="rowFrom"></param>
		/// <param name="rowTo"></param>
		/// <param name="doCount"></param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual List<MasterIDEntity> GetItemVariantsMasterID(IConnectionManager entry,
			int rowFrom,
			int rowTo,
			bool doCount,
			out int totalRecordsMatching,
			RecordIdentifier itemID,
			List<string> attributeDescription = null,
			bool attributeDescriptionStartsWith = false)
		{
			ValidateSecurity(entry);

			List<Condition> externalConditions = new List<Condition>();

			List<TableColumn> columns = new List<TableColumn>();
			using (var cmd = entry.Connection.CreateCommand())
			{
				columns.AddRange(SelectionColumns[ColumnPopulation.MasterDataEntity]);

				if (doCount &&
					(entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
					 entry.Connection.DatabaseVersion == ServerVersion.Unknown))
				{
					columns.Add(new TableColumn
					{
						ColumnName = "ROW_NUMBER() OVER(order by ITEMNAME)",
						ColumnAlias = "ROW"
					});
					columns.Add(new TableColumn
					{
						ColumnName = "COUNT(1) OVER ( ORDER BY ITEMNAME RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
						ColumnAlias = "ROW_COUNT"
					});
					externalConditions.Add(new Condition
					{
						Operator = "AND",
						ConditionValue = "ss.ROW between @rowFrom and @rowTo"
					});
					MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
					MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
				}
				List<Condition> conditions = new List<Condition>();

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.HEADERITEMID = @itemID" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });
				List<Join> joins = new List<Join>();
				if (attributeDescription != null && attributeDescription.Count == 1 && !string.IsNullOrEmpty(attributeDescription[0]))
				{
					List<Condition> searchConditions = new List<Condition>();

					joins.Add(new Join
					{
						Table = "RETAILITEMDIMENSIONATTRIBUTE",
						Condition = "A.MASTERID = rda.RETAILITEMID",
						JoinType = "",
						TableAlias = "rda"
					});

					string searchString = PreProcessSearchText(attributeDescription[0], true, attributeDescriptionStartsWith);
					searchConditions.Add(new Condition
					{
						ConditionValue = "da.ID = rda.DIMENSIONATTRIBUTEID",
						Operator = "AND"
					});
					searchConditions.Add(new Condition
					{
						ConditionValue = "(da.description like @attributeDescription OR da.CODE like @attributeDescription) ",
						Operator = "AND"
					});

					joins.Add(new Join
					{
						Table = "DIMENSIONATTRIBUTE",
						Condition = QueryPartGenerator.ConditionGenerator(searchConditions, true),
						JoinType = "",
						TableAlias = "da"
					});
					MakeParam(cmd, "attributeDescription", searchString);
				}
				else if (attributeDescription != null && attributeDescription.Count > 1)
				{
					for (int index = 0; index < attributeDescription.Count; index++)
					{
						List<Condition> searchConditions = new List<Condition>();

						var searchToken = PreProcessSearchText(attributeDescription[index], true, attributeDescriptionStartsWith);

						if (!string.IsNullOrEmpty(searchToken))
						{
							joins.Add(new Join
							{
								Table = "RETAILITEMDIMENSIONATTRIBUTE",
								Condition = $"A.MASTERID = rda{index}.RETAILITEMID",
								JoinType = "",
								TableAlias = $"rda{index}"
							});
							searchConditions.Add(new Condition
							{
								ConditionValue = $"da{index}.ID = rda{index}.DIMENSIONATTRIBUTEID",
								Operator = "AND"
							});
							searchConditions.Add(new Condition
							{
								ConditionValue = $"(da{index}.description like @attributeDescription{index} OR da{index}.CODE like @attributeDescription{index}) ",
								Operator = "AND"
							});

							MakeParam(cmd, $"attributeDescription{index}", searchToken);

							joins.Add(new Join
							{
								Table = "DIMENSIONATTRIBUTE",
								Condition = QueryPartGenerator.ConditionGenerator(searchConditions, true),
								JoinType = "",
								TableAlias = $"da{index}"
							});
						}
					}
				}

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEM", "A", "ss"),
					QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(joins),
					QueryPartGenerator.ConditionGenerator(conditions),
					QueryPartGenerator.ConditionGenerator(externalConditions),
					"ORDER BY ITEMNAME");
				MakeParam(cmd, "itemID", (Guid)itemID, SqlDbType.UniqueIdentifier);
				totalRecordsMatching = 0;
				return Execute<MasterIDEntity, int>(entry, cmd, CommandType.Text,
								ref totalRecordsMatching,
								PopulateVariantWithCount);
			}
		}

		/// <summary>
		/// Gets variant items
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <returns>The retail item, or null if not found</returns>
		public virtual List<SimpleRetailItem> GetHeaderItems(IConnectionManager entry)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);
				List<TableColumn> columns = new List<TableColumn>();
				foreach (var selectionColumn in SelectionColumns[ColumnPopulation.Simple])
				{
					columns.Add(selectionColumn);
				}
				List<Condition> conditions = new List<Condition>();

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMTYPE = 3" });
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

				AddCustomHandling(ref columns, ref conditions);
				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(simpleJoins),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);

				return Execute<SimpleRetailItem>(entry, cmd, CommandType.Text, PopulateSimpleItem);
			}
		}
		
		public virtual List<SimpleRetailItem> GetSimpleItems(IConnectionManager entry)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);
				List<TableColumn> columns = new List<TableColumn>();
				foreach (var selectionColumn in SelectionColumns[ColumnPopulation.Simple])
				{
					columns.Add(selectionColumn);
				}
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
                        QueryPartGenerator.InternalColumnGenerator(columns),
						QueryPartGenerator.JoinGenerator(simpleJoins),
						QueryPartGenerator.ConditionGenerator(conditions),
						string.Empty);

				return Execute<SimpleRetailItem>(entry, cmd, CommandType.Text, PopulateSimpleItem);
			}
		}

		/// <summary>
		/// Deletes a retail item by a given ID.
		/// </summary>
		/// <remarks>Edit retail items permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="id">The ID of the retail item to delete</param>
		public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
		{
			if (entry.HasPermission(Permission.ItemsEdit))
			{
				if (id.IsGuid)
				{
					MarkAsDeleted(entry, "RETAILITEM", "MASTERID", id, Permission.ItemsEdit);
				}
				else
				{
					throw new ArgumentException("id should be RecordIdentifier of Guid type");
				}
			}
			else
			{
				throw new PermissionException(Permission.ItemsEdit);
			}
		}

		public virtual int ItemCount(IConnectionManager entry)
		{
			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT COUNT(1) FROM RETAILITEM";

				return (int)entry.Connection.ExecuteScalar(cmd);
			}
		}

		/// <summary>
		///  Checks if a retail item with a given ID exists in the database.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">The ID of the retail item to check for</param>
		/// <returns></returns>
		public virtual bool Exists(IConnectionManager entry, RecordIdentifier itemID)
		{
			if (itemID.IsGuid)
			{
				return RecordExists(entry, "RETAILITEM", "MASTERID", itemID, false);
			}

			return RecordExists(entry, "RETAILITEM", "ITEMID", itemID, false);
		}

		/// <summary>
		/// Checks if any item is using a given tax group id.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="taxgroupID">ID of the tax group</param>
		/// <returns>True if any item uses the tax group, else false</returns>
		public virtual bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID)
		{
			return RecordExists(entry, "RETAILITEM", "SALESTAXITEMGROUPID", taxgroupID, false);
		}

		private static bool ItemRecordExists(IConnectionManager entry, RecordIdentifier itemID)
		{
			return RecordExists(entry, "RETAILITEM", "ITEMID", itemID, false);
		}

		/// <summary>
		/// Saves the retail item and its related module records, it only saves those that are in need of saving.
		/// (Thus master record or sub record that has the Dirty property as true get saved)
		/// </summary>
		/// <remarks>Edit retail items permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="item">The retail item to save</param>
		public virtual void Save(IConnectionManager entry, RetailItem item)
		{
			var statement = new SqlServerStatement("RETAILITEM");
			statement.UpdateColumnOptimizer = item;

			string[] permissions = {Permission.ItemsEdit, Permission.ManagePurchaseOrders};

			ValidateSecurity(entry, permissions);

			item.ID = string.IsNullOrWhiteSpace((string)item.ID) ? RecordIdentifier.Empty : item.ID;
			if (item.ID == RecordIdentifier.Empty || !Exists(entry, item.ID))
			{
				statement.StatementType = StatementType.Insert;
				if (item.MasterID == null || item.MasterID == RecordIdentifier.Empty)
				{
					item.MasterID = Guid.NewGuid();
				}
				statement.AddKey("MASTERID", (Guid)item.MasterID, SqlDbType.UniqueIdentifier);
				if (item.ID == RecordIdentifier.Empty)
				{
					// Should we create a new item or update what exists
					item.ID = DataProviderFactory.Instance.GenerateNumber<IRetailItemData, RetailItem>(entry);
				}
				statement.AddField("ITEMID", (string)item.ID);

				// ONE-8779: if the item is marked as ScaleItem, then we are good; if it is not, check whether the sales unit is a scale unit and if it is, mark the item accordingly
				var parameters = Providers.ParameterData.Get(entry);
				item.ScaleItem = item.ScaleItem || parameters.IsScaleUnit(item.SalesUnitID.StringValue);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				// This can happen if we are getting the retail item from the Integration Framework, where the external caller does not have information
				// about our master IDs
				if (RecordIdentifier.IsEmptyOrNull(item.MasterID))
				{
					item.MasterID = GetMasterID(entry, item.ID, "RETAILITEM", "ITEMID");
				}

				statement.AddCondition("MASTERID", (Guid) item.MasterID, SqlDbType.UniqueIdentifier);                
			}

			if (item.HeaderItemID.IsEmpty)
			{
				statement.AddField("HEADERITEMID", DBNull.Value, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.AddField("HEADERITEMID", (Guid)item.HeaderItemID, SqlDbType.UniqueIdentifier);
			}

			statement.AddField("ITEMNAME", item.Text);
			statement.AddField("VARIANTNAME", item.VariantName);
			statement.AddField("ITEMTYPE", (byte)item.ItemType, SqlDbType.TinyInt);
			statement.AddField("DEFAULTVENDORID", (string)item.DefaultVendorID);
			statement.AddField("NAMEALIAS", item.NameAlias);
			statement.AddField("EXTENDEDDESCRIPTION", item.ExtendedDescription);
			statement.AddField("SEARCHKEYWORDS", item.SearchKeywords);

            if (item.RetailGroupMasterID == null || item.RetailGroupMasterID.IsEmpty)
			{
				statement.AddField("RETAILGROUPMASTERID", DBNull.Value, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.AddField("RETAILGROUPMASTERID", (Guid)item.RetailGroupMasterID, SqlDbType.UniqueIdentifier);
			}

			statement.AddField("ZEROPRICEVALID", item.ZeroPriceValid, SqlDbType.Bit);
			statement.AddField("QTYBECOMESNEGATIVE", item.QuantityBecomesNegative, SqlDbType.Bit);
			statement.AddField("NODISCOUNTALLOWED", item.NoDiscountAllowed, SqlDbType.Bit);
			statement.AddField("KEYINPRICE", item.KeyInPrice, SqlDbType.TinyInt);
			statement.AddField("KEYINSERIALNUMBER", item.KeyInSerialNumber, SqlDbType.TinyInt);
			statement.AddField("SCALEITEM", item.ScaleItem, SqlDbType.Bit);
			statement.AddField("TAREWEIGHT", item.TareWeight, SqlDbType.Int);
            statement.AddField("KEYINQTY", item.KeyInQuantity, SqlDbType.TinyInt);
			statement.AddField("BLOCKEDONPOS", item.BlockedOnPOS, SqlDbType.Bit);
			statement.AddField("BARCODESETUPID", (string)item.BarCodeSetupID);
			statement.AddField("PRINTVARIANTSSHELFLABELS", item.PrintVariantsShelfLabels, SqlDbType.Bit);
			statement.AddField("FUELITEM", item.IsFuelItem, SqlDbType.Bit);
			statement.AddField("GRADEID", item.GradeID);
			statement.AddField("MUSTKEYINCOMMENT", item.MustKeyInComment, SqlDbType.Bit);
			statement.AddField("DATETOBEBLOCKED", item.DateToBeBlocked.ToAxaptaSQLDate(), SqlDbType.DateTime);
			statement.AddField("DATETOACTIVATEITEM", item.DateToActivateItem.ToAxaptaSQLDate(), SqlDbType.DateTime);
			statement.AddField("PROFITMARGIN", item.ProfitMargin, SqlDbType.Decimal);
			statement.AddField("VALIDATIONPERIODID", (string)item.ValidationPeriodID);
			statement.AddField("MUSTSELECTUOM", item.MustSelectUOM, SqlDbType.Bit);
			statement.AddField("INVENTORYUNITID", (string)item.InventoryUnitID);
			statement.AddField("PURCHASEUNITID", item.PurchaseUnitID != null && !string.IsNullOrEmpty((string)item.PurchaseUnitID) ? (string)item.PurchaseUnitID : (string)item.InventoryUnitID);
			statement.AddField("SALESUNITID", (string)item.SalesUnitID);
			statement.AddField("PURCHASEPRICE", item.PurchasePrice, SqlDbType.Decimal);
			statement.AddField("SALESPRICE", item.SalesPrice, SqlDbType.Decimal);
			statement.AddField("SALESPRICEINCLTAX", item.SalesPriceIncludingTax, SqlDbType.Decimal);
			statement.AddField("SALESMARKUP", item.SalesMarkup, SqlDbType.Decimal);
			statement.AddField("SALESLINEDISC", (string)item.SalesLineDiscount);
			statement.AddField("SALESMULTILINEDISC", (string)item.SalesMultiLineDiscount);
			statement.AddField("SALESALLOWTOTALDISCOUNT", item.SalesAllowTotalDiscount, SqlDbType.Bit);
			statement.AddField("SALESTAXITEMGROUPID", (string)item.SalesTaxItemGroupID);
			statement.AddField("RETURNABLE", item.Returnable, SqlDbType.Bit);
			statement.AddField("CANBESOLD", item.CanBeSold, SqlDbType.Bit);
			statement.AddField("PRODUCTIONTIME", item.ProductionTime, SqlDbType.Int);

			statement.AddField("MODIFIED", DateTime.Now, SqlDbType.DateTime2);

            SaveCustomA(entry, statement, item);

			entry.Connection.ExecuteStatement(statement);
		}

		/// <summary>
		/// Gets an items item sales tax group. Returns an empty record identifier if item has no item sales tax group
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">The items ID</param>
		/// <returns>Items item sales tax group</returns>
		public virtual RecordIdentifier GetItemsItemSalesTaxGroupID(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = @"
SELECT 
	SALESTAXITEMGROUPID 
FROM 
	RETAILITEM 
WHERE 
	ITEMID = @itemID ";

				MakeParam(cmd, "itemID", (string)itemID);

				return (string)entry.Connection.ExecuteScalar(cmd);
			}
		}

		/// <summary>
		/// Gets the latest purchase price of an item
		/// </summary>
		/// <param name="entry">The entry to the database</param>
		/// <param name="itemID">The item who's purchase price we want</param>
		/// <returns>The latest purchase price of an item</returns>
		public virtual decimal GetLatestPurchasePrice(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;
				cmd.CommandText = "Select ISNULL(SALESPRICE, 0) as SALESPRICE from RETAILITEM " +
								  "where ITEMID = @itemID ";

				MakeParam(cmd, "itemID", (string)itemID);

				object result = entry.Connection.ExecuteScalar(cmd);

				if (result == null)
				{
					return decimal.Zero;
				}

				return (decimal)result;
			}
		}

		/// <summary>
		/// Returns the default vendor for a given item
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The unique ID of the item, or MasterID</param>
		/// <returns>
		/// Returns the vendor ID
		/// </returns>
		public virtual RecordIdentifier GetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandType = CommandType.Text;

				if (itemID.IsGuid)
				{
					cmd.CommandText = "Select DEFAULTVENDORID from RETAILITEM " +
						"where MASTERID = @itemID ";

					MakeParam(cmd, "itemID", (Guid)itemID);
				}
				else
				{
					cmd.CommandText = "Select DEFAULTVENDORID from RETAILITEM " +
						"where ITEMID = @itemID ";

					MakeParam(cmd, "itemID", (string)itemID);
				}

				return (string)entry.Connection.ExecuteScalar(cmd);
			}
		}

		/// <summary>
		/// Returns true if the item has a default vendor
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The unique ID of the item or MasterID</param>
		/// <returns>
		/// Returns true if the item has a default vendor
		/// </returns>
		public virtual bool ItemHasDefaultVendor(IConnectionManager entry, RecordIdentifier itemID)
		{
			RecordIdentifier defaultVendor = GetItemsDefaultVendor(entry, itemID);

			return (defaultVendor != RecordIdentifier.Empty && defaultVendor.StringValue != "");
		}

		/// <summary>
		/// Sets a vendor as a default on a given item
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The unique ID of the item or MasterID</param>
		/// <param name="vendorID">The unique ID of the vendor</param>
		public virtual void SetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier vendorID)
		{
			var statement = new SqlServerStatement("RETAILITEM");

			ValidateSecurity(entry, Permission.CurrencyEdit);

			if (Exists(entry, itemID))
			{
				statement.StatementType = StatementType.Update;

				var masterID = itemID.IsGuid ? itemID : GetMasterIDFromItemID(entry, itemID);

				statement.AddCondition("MASTERID", (Guid)masterID, SqlDbType.UniqueIdentifier);

				statement.AddField("DEFAULTVENDORID", (string)vendorID);

				entry.Connection.ExecuteStatement(statement);
			}
		}

		public virtual RetailItemOld.RetailItemModule GetPriceModule(IConnectionManager entry, RecordIdentifier itemID)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns item ids for all items that have a tax group with the given tax code ID
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="taxCodeID">ID of the tax code</param>
		/// <param name="rowFrom">startrow</param>
		/// <param name="rowTo">endrow</param>
		/// <param name="totalRecordsMatching">Total row count</param>
		public virtual List<RetailItem> GetItemsFromTaxCode(IConnectionManager entry,
			RecordIdentifier taxCodeID,
			int rowFrom,
			int rowTo,
			out int totalRecordsMatching)
		{
			// TODO Needs fix
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				totalRecordsMatching = 0;
				List<Condition> externalConditions = new List<Condition>();
				List<TableColumn> columns = new List<TableColumn>(SelectionColumns[ColumnPopulation.SiteManager]);
				columns.Add(new TableColumn
				{
					ColumnName =
					"ROW_NUMBER() OVER(order by ITEMNAME)",
					ColumnAlias = "ROW"
				});
				columns.Add(new TableColumn
				{
					ColumnName = "COUNT(1) OVER ( ORDER BY ITEMNAME RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
					ColumnAlias = "ROW_COUNT"
				});
				externalConditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = "ss.ROW between @rowFrom and @rowTo"
				});
				MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
				MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);

				Condition condition = new Condition
				{
					ConditionValue = "TOI.TAXCODE = @TAXCODEID",
					Operator = "AND"
				};
				List<Join> joins = new List<Join>(itemJoins);
				joins.Add(
				    new Join
				    {
					    Condition = " A.SALESTAXITEMGROUPID = TOI.TAXITEMGROUP",
					    TableAlias = "TOI",
					    Table = "TAXONITEM"
				    });

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEM", "A", "ss"),
				   QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
				   QueryPartGenerator.InternalColumnGenerator(columns),
				   QueryPartGenerator.JoinGenerator(joins),
				   QueryPartGenerator.ConditionGenerator(condition),
				   QueryPartGenerator.ConditionGenerator(externalConditions),
				   "order by ITEMNAME"
				   );

				MakeParam(cmd, "TAXCODEID", (string)taxCodeID);

				return Execute<RetailItem, int>(entry, cmd, CommandType.Text,
								ref totalRecordsMatching,
								PopulateItemWithCount);
			}
		}

		public virtual List<RetailItem> GetItemsFromTaxGroup(IConnectionManager entry,
			RecordIdentifier itemSalesTaxGroupID,
			int rowFrom,
			int rowTo,
			out int totalRecordsMatching)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				totalRecordsMatching = 0;
				List<Condition> externalConditions = new List<Condition>();
				List<TableColumn> columns = new List<TableColumn>(SelectionColumns[ColumnPopulation.SiteManager]);
				columns.Add(new TableColumn
				{
					ColumnName =
					"ROW_NUMBER() OVER(order by ITEMNAME)",
					ColumnAlias = "ROW"
				});
				columns.Add(new TableColumn
				{
					ColumnName = "COUNT(1) OVER ( ORDER BY ITEMNAME RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
					ColumnAlias = "ROW_COUNT"
				});
				externalConditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = "ss.ROW between @rowFrom and @rowTo"
				});
				MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
				MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);

				Condition condition = new Condition
				{
					ConditionValue = "A.SALESTAXITEMGROUPID = @itemSalesTaxGroupID",
					Operator = "AND"
				};

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEM", "A", "ss"),
					QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(itemJoins),
					QueryPartGenerator.ConditionGenerator(condition),
					QueryPartGenerator.ConditionGenerator(externalConditions),
					"order by ITEMNAME"
					);

				MakeParam(cmd, "itemSalesTaxGroupID", (string)itemSalesTaxGroupID);

				return Execute<RetailItem, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateItemWithCount);
			}
		}

		public virtual List<RetailItem> GetItemsWithTaxGroup(IConnectionManager entry, int rowFrom,
			int rowTo,
			out int totalRecordsMatching)
		{
			ValidateSecurity(entry);
			using (var cmd = entry.Connection.CreateCommand())
			{
				totalRecordsMatching = 0;
				List<Condition> externalConditions = new List<Condition>();
				List<TableColumn> columns = new List<TableColumn>(SelectionColumns[ColumnPopulation.SiteManager]);
				columns.Add(new TableColumn
				{
					ColumnName =
					"ROW_NUMBER() OVER(order by ITEMNAME)",
					ColumnAlias = "ROW"
				});
				columns.Add(new TableColumn
				{
					ColumnName = "COUNT(1) OVER ( ORDER BY ITEMNAME RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",
					ColumnAlias = "ROW_COUNT"
				});
				externalConditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = "ss.ROW between @rowFrom and @rowTo"
				});
				MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
				MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);

				Condition condition = new Condition
				{
					ConditionValue = " A.SALESTAXITEMGROUPID <> ''",
					Operator = "AND"
				};

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEM", "A", "ss"),
					QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(itemJoins),
					QueryPartGenerator.ConditionGenerator(condition),
					QueryPartGenerator.ConditionGenerator(externalConditions),
					"order by ITEMNAME"
					);

				return Execute<RetailItem, int>(entry, cmd, CommandType.Text,
							   ref totalRecordsMatching,
							   PopulateItemWithCount);
			}
		}

		public virtual List<DataEntity> FindItem(IConnectionManager entry, string searchText)
		{
			// TODO Needs fix

			int totalRecordsMatching = 0;
			return AdvancedSearchOptimized<DataEntity>(entry, 0, 0, SortEnum.None, SortEnum.None, false,
				out totalRecordsMatching,
				ColumnPopulation.IDOnly, new List<string> { searchText });
		}

		public virtual List<DataEntity> FindItemDepartment(IConnectionManager entry, string searchText)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = "select GROUPID, NAME from RETAILGROUP where (GROUPID Like @searchParam or Name Like @searchParam) order by GROUPID";

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "searchParam", "%" + searchText + "%");

				return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "GROUPID");
			}
		}

		/// <summary>
		/// Adds an item - attribute connection for the given item master ID and attribute ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="variantItemMasterID">The variant item master ID</param>
		/// <param name="attributeID">The ID of the dimension attribute</param>
		public virtual void AddDimensionAttribute(IConnectionManager entry, RecordIdentifier variantItemMasterID, RecordIdentifier attributeID)
		{
		    string[] permissions = { Permission.ItemsEdit, Permission.ManagePurchaseOrders };
		    ValidateSecurity(entry, permissions);
			bool exists;
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"
				SELECT 
					RETAILITEMID,
					DIMENSIONATTRIBUTEID
				  FROM RETAILITEMDIMENSIONATTRIBUTE
				  WHERE 
					RETAILITEMID = @variantItemMasterID AND
					DIMENSIONATTRIBUTEID = @attributeID";
				MakeParam(cmd, "variantItemMasterID", (Guid)variantItemMasterID, SqlDbType.UniqueIdentifier);
				MakeParam(cmd, "attributeID", (Guid)attributeID, SqlDbType.UniqueIdentifier);
				IDataReader dr = null;
				try
				{
					dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

					exists = dr.Read();
				}
				finally
				{
					if (dr != null)
					{
						dr.Close();
						dr.Dispose();
					}
				}
			}

			var statement = new SqlServerStatement("RETAILITEMDIMENSIONATTRIBUTE");
			if (!exists)
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("RETAILITEMID", (Guid)variantItemMasterID, SqlDbType.UniqueIdentifier);
				statement.AddKey("DIMENSIONATTRIBUTEID", (Guid)attributeID, SqlDbType.UniqueIdentifier);
				entry.Connection.ExecuteStatement(statement);
			}
		}

		/// <summary>
		/// Removes the item - attribute connection for the given item master ID and attribute ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="retailItemID">The retail item master ID</param>
		/// <param name="attributeID">The ID of the dimension attribute</param>
		public virtual void RemoveDimensionAttribute(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier attributeID)
		{
			ValidateSecurity(entry, Permission.ItemsEdit);

			var statement = new SqlServerStatement("RETAILITEMDIMENSIONATTRIBUTE", StatementType.Delete);

			statement.AddCondition("RETAILITEMID", (Guid)retailItemID, SqlDbType.UniqueIdentifier);
			statement.AddCondition("DIMENSIONATTRIBUTEID", (Guid)attributeID, SqlDbType.UniqueIdentifier);

			entry.Connection.ExecuteStatement(statement);
		}

		public RecordIdentifier GetMasterIDFromAttributes(IConnectionManager entry, List<RecordIdentifier> attributesIDs)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Join> joins = new List<Join>();
				List<string> joinTableAliases = new List<string>();
				List<Condition> conditions = new List<Condition>();

				for (int i = 0; i < attributesIDs.Count; i++)
				{
					joinTableAliases.Add("r" + i);
				}

				for (int i = 0; i < attributesIDs.Count; i++)
				{
					Join join = new Join();
					join.TableAlias = joinTableAliases[i];
					join.Condition =
						$"r.RETAILITEMID = {join.TableAlias}.RETAILITEMID AND {join.TableAlias}.DIMENSIONATTRIBUTEID = @{join.TableAlias}_attributeID";
					join.Table = "RETAILITEMDIMENSIONATTRIBUTE";

					joins.Add(join);

					MakeParam(cmd, $"{join.TableAlias}_attributeID", (Guid)attributesIDs[i], SqlDbType.UniqueIdentifier);
				}

				cmd.CommandText =
					$@"select r.RETAILITEMID
					  from RETAILITEMDIMENSIONATTRIBUTE r
					  -- Joins
					  {
						QueryPartGenerator.JoinGenerator(joins)}";

				object result = entry.Connection.ExecuteScalar(cmd);

				return new RecordIdentifier((Guid)result);
			}
		}

		public List<SimpleRetailItem> GetRetailItemsFromAttribute(IConnectionManager entry, RecordIdentifier attributesID)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Join> joins = new List<Join>(simpleJoins);
				joins.Add(new Join
				{
					TableAlias = "RDA",
					Condition = "A.MASTERID = RDA.RETAILITEMID AND RDA.DIMENSIONATTRIBUTEID = @attributeID",
					Table = "RETAILITEMDIMENSIONATTRIBUTE"
				});

				MakeParam(cmd, "attributeID", (Guid)attributesID, SqlDbType.UniqueIdentifier);

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A"),
					 QueryPartGenerator.InternalColumnGenerator(SelectionColumns[ColumnPopulation.Simple]),
					  QueryPartGenerator.JoinGenerator(joins),
					 string.Empty,
					 string.Empty);

				return Execute<SimpleRetailItem>(entry, cmd, CommandType.Text, PopulateSimpleItem);
			}
		}

		/// <summary>
		/// Gets the master ID for the given item ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The item ID to get the master ID for</param>
		/// <returns></returns>
		public virtual RecordIdentifier GetMasterIDFromItemID(IConnectionManager entry, RecordIdentifier itemID)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"select MASTERID
					  from RETAILITEM
					  where ITEMID = @itemID";

				MakeParam(cmd, "itemID", (string)itemID);
				var reply = entry.Connection.ExecuteScalar(cmd);
				if (reply == null)
				{
					return null;
				}
				return (Guid)reply;
			}
		}

		/// <summary>
		/// Undeletes the given item. This set the DELETED flag on the item to 0
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The ID of the item to undelete</param>
		public virtual void Undelete(IConnectionManager entry, RecordIdentifier itemID)
		{
			ValidateSecurity(entry, Permission.ItemsEdit);

			SqlServerStatement statement = new SqlServerStatement("RETAILITEM");
			statement.StatementType = StatementType.Update;

			statement.AddField("DELETED", false, SqlDbType.Bit);
			statement.AddCondition(itemID.IsGuid ? "MASTERID" : "ITEMID", itemID.DBValue, itemID.DBType);

			entry.Connection.ExecuteStatement(statement);
		}

		/// <summary>
		/// Returns true if the item is marked as deleted
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The ID of the item to check</param>
		/// <returns>True if the item is marked as deleted, false otherwise</returns>
		public bool IsDeleted(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				string idColumnName = itemID.IsGuid ? "MASTERID" : "ITEMID";

				cmd.CommandText =
					$@"select DELETED
					   from RETAILITEM
					   where {idColumnName} = @itemID";

				MakeParam(cmd, "itemID", itemID.DBValue, itemID.DBType);

				return (bool)entry.Connection.ExecuteScalar(cmd);
			}
		}

		#region Custom overrideables

		protected virtual void AddCustomHandling(ref List<TableColumn> columns, ref List<Condition> conditions)
		{
			//Add columns and conditions
		}

		protected virtual void SaveCustomA(IConnectionManager entry, SqlServerStatement statement, RetailItem retailItem)
		{
		}

		protected virtual void SaveCustomB(IConnectionManager entry, SqlServerStatement statement, RetailItem retailItem)
		{
		}

		#endregion

		#region ISequenceable Members

		/// <summary>
		/// Returns true if information about the given class exists in the database
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="id">The unique sequence ID to search for</param>
		public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
		{
			return ItemRecordExists(entry, id);
		}

		/// <summary>
		/// Returns a unique ID for the class
		/// </summary>
		public RecordIdentifier SequenceID
		{
			get { return "ITEM"; }
		}

		public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
		{
			return GetExistingRecords(entry, "RETAILITEM", "ITEMID", sequenceFormat, startingRecord, numberOfRecords);
		}

		#endregion

		/// <summary>
		/// Returns the item type<see cref="ItemTypeEnum"/> for the given item ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The ID of the item to get the type for</param>
		/// <returns></returns>
		public ItemTypeEnum GetItemType(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				string idColumnName = itemID.IsGuid ? "MASTERID" : "ITEMID";

				cmd.CommandText =
					$@"select ITEMTYPE
					   from RETAILITEM
					   where {idColumnName} = @itemID";

				MakeParam(cmd, "itemID", itemID.DBValue, itemID.DBType);

				var x = entry.Connection.ExecuteScalar(cmd);
				if (x != null)
				{
					return (ItemTypeEnum)int.Parse(x.ToString());
				}

				return ItemTypeEnum.Item;
			}
		}

		/// <summary>
		/// Updates the type on a specific item
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The unique ID of the item to update</param>
		/// <param name="itemType">The new type</param>
		public virtual void UpdateItemType(IConnectionManager entry, RecordIdentifier itemID, ItemTypeEnum itemType)
		{
			RetailItem item = Get(entry, itemID);

			if (item != null && item.ItemType != itemType)
			{
				item.ItemType = itemType;
				item.Dirty = true;
				Save(entry, item);
			}
		}

		/// <summary>
		/// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemsToCompare">List of items you want to get from the database matching items</param>
		/// <returns>List of items</returns>
		public virtual List<RetailItem> GetCompareList(IConnectionManager entry, List<RetailItem> itemsToCompare)
		{
			return GetCompareListInBatches(entry, itemsToCompare, "RETAILITEM", "ITEMID", SelectionColumns[ColumnPopulation.SiteManager], itemJoins, PopulateItem);
		}
	}
}