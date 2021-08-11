using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Development;

namespace LSOne.DataLayer.SqlDataProviders.BarCodes
{
	public class BarCodeData : SqlServerDataProviderBase, IBarCodeData
	{
		private static string ResolveSort(BarCodeSorting sort,bool backwards)
		{
			var direction = backwards ? " DESC" : " ASC"; 

			switch (sort)
			{
				case BarCodeSorting.ItemBarCode:
					return "i.ITEMBARCODE" + direction;

				case BarCodeSorting.VariantID:
					return "i.RBOVARIANTID" + direction;

				case BarCodeSorting.SizeID:
					return "ic.INVENTSIZEID" + direction;

				case BarCodeSorting.ColorID:
					return "ic.INVENTCOLORID" + direction;

				case BarCodeSorting.StyleID:
					return "ic.INVENTSTYLEID" + direction;

				case BarCodeSorting.BarcodeSetupID:
					return "i.BARCODESETUPID" + direction;

				case BarCodeSorting.ShowForItem:
					return "i.RBOSHOWFORITEM" + direction;

				case BarCodeSorting.UseForInput:
					return "i.USEFORINPUT" + direction;

				case BarCodeSorting.UseForPrinting:
					return "i.USEFORPRINTING" + direction;

				case BarCodeSorting.Unit:
					return "u.TXT" + direction;

				case BarCodeSorting.ItemID:
					return "i.ITEMID" + direction;
			}

			return "";
		}

		private string BaseSQL =
			@"Select i.ITEMBARCODE, 
					 i.ITEMID, 
					 i.ITEMBARCODEID,
					 i.RBOVARIANTID,
					 ISNULL(i.BARCODESETUPID,'') as BARCODESETUPID,
					 ISNULL(i.RBOSHOWFORITEM,0) as RBOSHOWFORITEM,
					 ISNULL(i.USEFORINPUT,0) as USEFORINPUT, 
					 ISNULL(i.USEFORPRINTING,0) as USEFORPRINTING,                     
					 ISNULL(bs.DESCRIPTION,'') as BARCODESETUPDESCRIPTION,
					 ISNULL(i.UNITID,'') as UNITID,
					 ISNULL(u.TXT,'') as UNITDESCRIPTION, 
					 ISNULL(i.QTY, 0) as QTY, 
					 rit.ITEMNAME,
					 rit.VARIANTNAME,
                     i.DELETED
			  from INVENTITEMBARCODE i               
			  left outer join BARCODESETUP bs on i.BARCODESETUPID = bs.BARCODESETUPID and i.DATAAREAID = bs.DATAAREAID 
			  left outer join UNIT u on i.UNITID = u.UNITID and i.DATAAREAID = u.DATAAREAID 
			  left join RETAILITEM rit on rit.ITEMID = i.ITEMID ";

		public RecordIdentifier SequenceID => "ITEMBARCODE";

		protected virtual void  PopulateBarCodeWithCount(IConnectionManager entry, IDataReader dr, BarCode barCode, ref int rowCount)

		{
			PopulateBarCode( dr,barCode);
			PopulateRowCount(entry, dr, ref rowCount);

		}

		protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
		{
			if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
				entry.Connection.DatabaseVersion == ServerVersion.Unknown)
			{
				rowCount = (int)dr["Row_Count"];
			}
		}

		private static void PopulateBarCode(IDataReader dr, BarCode barCode)
		{
			barCode.ItemBarCode = (string)dr["ITEMBARCODE"];
			barCode.ItemID = (string)dr["ITEMID"];
			barCode.ItemBarcodeID = (string)dr["ITEMBARCODEID"];

			barCode.BarCodeSetupID = (string)dr["BARCODESETUPID"];
			barCode.BarCodeSetupDescription = (string)dr["BARCODESETUPDESCRIPTION"];
			barCode.VariantID = (string)dr["RBOVARIANTID"];

			barCode.ShowForItem = ((byte)dr["RBOSHOWFORITEM"] != 0);
			barCode.UseForInput = ((byte)dr["USEFORINPUT"] != 0);
			barCode.UseForPrinting = ((byte)dr["USEFORPRINTING"] != 0);

			barCode.UnitID = (string)dr["UNITID"];
			barCode.UnitDescription = (string)dr["UNITDESCRIPTION"];

			barCode.Quantity = (decimal)dr["QTY"];

			barCode.ItemName = (string) dr["ITEMNAME"];
			barCode.VariantName = (string) dr["VARIANTNAME"];
            barCode.Deleted = (bool)dr["DELETED"];
		}

		private static void PopulateBarCodeForItemList(IDataReader dr, BarCode barCode)
		{
			barCode.ItemBarCode = (string)dr["ITEMBARCODE"];
			barCode.ItemID = (string)dr["ITEMID"];
			barCode.ItemName = (string)dr["ITEMNAME"];

		}

		private static void PopulateBarCodeVariant(IDataReader dr, BarCode barCode)
		{
			barCode.ItemBarCode = (string)dr["ITEMBARCODE"];
			barCode.ItemID = (string)dr["ITEMID"];
			barCode.ItemBarcodeID = (string)dr["ITEMBARCODEID"];

			barCode.BarCodeSetupID = (string)dr["BARCODESETUPID"];
			barCode.BarCodeSetupDescription = (string)dr["BARCODESETUPDESCRIPTION"];
			barCode.VariantID = (string)dr["RBOVARIANTID"];

			barCode.ShowForItem = ((byte)dr["RBOSHOWFORITEM"] != 0);
			barCode.UseForInput = ((byte)dr["USEFORINPUT"] != 0);
			barCode.UseForPrinting = ((byte)dr["USEFORPRINTING"] != 0);

			barCode.UnitID = (string)dr["UNITID"];
			barCode.UnitDescription = (string)dr["UNITDESCRIPTION"];

			barCode.Quantity = (decimal)dr["QTY"];
		}

		private static void PopulateMaskSegment(IDataReader dr, BarcodeMaskSegment segment)
		{
			segment.Decimals = (int)dr["DECIMALS"];
			segment.Length = (int)(decimal)dr["LENGTH"];
			segment.Type = (BarcodeSegmentType)dr["TYPE"];
			segment.MaskId = (string)dr["MASKID"];
			segment.SegmentChar = (string)dr["CHAR"];
			segment.SegmentNum = (int)dr["SEGMENTNUM"];
		}

		private static object PopulateAdditionalInformationToBarcode(IConnectionManager entry, IDataReader dr, BarCode barCode)
		{
			barCode.ItemID = (string)dr["ITEMID"];
			barCode.Description = (string)dr["DESCRIPTION"];
			barCode.VariantID = (string)dr["RBOVARIANTID"];
			barCode.Blocked = ((byte)dr["BLOCKED"] != 0);
			barCode.UnitID = (string)dr["UNITID"];
			barCode.QtySold = (decimal)dr["QTY"];
			barCode.ItemBarcodeID = (string)dr["ITEMBARCODEID"];
			barCode.Message = "";
			barCode.Found = true;
			return new object();
		}

		public virtual BarCode GetBarCodeForItem(IConnectionManager entry, RecordIdentifier itemID, CacheType cache = CacheType.CacheTypeNone)
		{
			ValidateSecurity(entry);
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = 
					@"SELECT 
					 ITEMBARCODE, 
					 ITEMBARCODEID,
					 ISNULL(BARCODESETUPID,'') as BARCODESETUPID, 
					 ISNULL(UNITID,'') as UNITID  
					FROM INVENTITEMBARCODE 
					WHERE DATAAREAID = @dataAreaID AND ITEMID = @itemID AND BLOCKED != 1 AND DELETED = 0
					ORDER BY RBOSHOWFORITEM DESC,QTY ASC";
				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "itemID", itemID);

				return Get(entry, cmd, itemID,
					delegate(IDataReader dr, BarCode barcode)
					{
						barcode.ItemBarCode = (string) dr["ITEMBARCODE"];
						barcode.BarCodeSetupID = (string)dr["BARCODESETUPID"];
						barcode.UnitID = (string)dr["UNITID"];
						barcode.ItemBarcodeID = (string)dr["ITEMBARCODEID"];
					}, cache, UsageIntentEnum.Normal);
			}
		}

		public virtual bool ShowForItemHasBeenUsed(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = @"select i.ITEMID
									from INVENTITEMBARCODE i
									where i.RBOSHOWFORITEM = 1
									and i.DATAAREAID = @dataAreaID
									and i.DELETED = 0
									and i.ITEMID in
									(
										select r.ITEMID
										from RETAILITEM r
										where r.MASTERID = @masterID
										union
										select r.ITEMID
										from RETAILITEM r
										where r.HEADERITEMID = @masterID	                                    
										union
										select r.ITEMID
										from RETAILITEM r
										where r.HEADERITEMID in
										(
											select r2.HEADERITEMID
											from RETAILITEM r2
											where r2.MASTERID = @masterID
										)
									)";

				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "masterID", (Guid)itemID);

				var result = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");
				return result.Count > 0;
			}
		}

		public virtual BarCode Get(IConnectionManager entry, RecordIdentifier barCodeID)
		{
			return Get(entry, barCodeID, CacheType.CacheTypeNone);
		}

		public virtual BarCode Get(IConnectionManager entry, RecordIdentifier barCodeID, CacheType cache = CacheType.CacheTypeNone)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText =
					@"Select i.ITEMBARCODE,
							i.ITEMBARCODEID, 
							i.ITEMID, 
							i.RBOVARIANTID,
							ISNULL(i.BARCODESETUPID,'') as BARCODESETUPID,
							ISNULL(i.RBOSHOWFORITEM,0) as RBOSHOWFORITEM,
							ISNULL(i.USEFORINPUT,0) as USEFORINPUT, 
							ISNULL(i.USEFORPRINTING,0) as USEFORPRINTING,
							ISNULL(i.INVENTDIMID,'') as INVENTDIMID,
							ISNULL(bs.DESCRIPTION,'') as BARCODESETUPDESCRIPTION, 
							ISNULL(i.UNITID,'') as UNITID,
							ISNULL(u.TXT,'') as UNITDESCRIPTION, 
							ISNULL(i.QTY,0) AS QTY,
							rit.ITEMNAME,
							rit.VARIANTNAME,
                            i.DELETED
					from INVENTITEMBARCODE i 
					left outer join BARCODESETUP bs on i.BARCODESETUPID = bs.BARCODESETUPID and i.DATAAREAID = bs.DATAAREAID 
					left outer join UNIT u on i.UNITID = u.UNITID and i.DATAAREAID = u.DATAAREAID 
					left join RETAILITEM rit on rit.ITEMID = i.ITEMID 
					where i.ITEMBARCODE = @barCodeID and i.DATAAREAID = @dataAreaID and i.DELETED = 0";

				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "barCodeID", (string)barCodeID);

				return Get<BarCode>(entry, cmd, barCodeID, PopulateBarCode, cache, UsageIntentEnum.Normal);
			}
		}

		public virtual List<BarCode> GetList(IConnectionManager entry, RecordIdentifier itemID, BarCodeSorting sortBy, bool backwardsSort, bool getDeletedBarcodes = false)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				string deletedBarcodes = getDeletedBarcodes ? "" : "and i.DELETED = 0 ";

				cmd.CommandText =
					$@"Select i.ITEMBARCODE, 
					i.ITEMBARCODEID, 
					i.ITEMID, 
					i.RBOVARIANTID,
					ISNULL(i.BARCODESETUPID,'') as BARCODESETUPID,
					ISNULL(i.RBOSHOWFORITEM,0) as RBOSHOWFORITEM,
					ISNULL(i.USEFORINPUT,0) as USEFORINPUT, 
					ISNULL(i.USEFORPRINTING,0) as USEFORPRINTING,
					ISNULL(i.INVENTDIMID,'') as INVENTDIMID,
					ISNULL(bs.DESCRIPTION,'') as BARCODESETUPDESCRIPTION,
					ISNULL(i.UNITID,'') as UNITID,
					ISNULL(u.TXT,'') as UNITDESCRIPTION, 
					ISNULL(i.QTY, 0) as QTY, 
					rit.ITEMNAME,
					rit.VARIANTNAME,
                    i.DELETED
					from INVENTITEMBARCODE i 
					left outer join BARCODESETUP bs on i.BARCODESETUPID = bs.BARCODESETUPID and i.DATAAREAID = bs.DATAAREAID 
					left outer join UNIT u on i.UNITID = u.UNITID and i.DATAAREAID = u.DATAAREAID 
					left join RETAILITEM rit on rit.ITEMID = i.ITEMID
					where i.ITEMID = @itemID and i.DATAAREAID = @dataAreaID {deletedBarcodes} 
					Order by " + ResolveSort(sortBy, backwardsSort);

				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "itemID", (string)itemID);

				return Execute<BarCode>(entry, cmd, CommandType.Text, PopulateBarCode);
			}
		}

		public virtual List<BarCode> GetListForVariant(IConnectionManager entry, RecordIdentifier variantID, bool getDeletedBarcodes = false)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				string deletedBarcodes = getDeletedBarcodes ? "" : "and i.DELETED = 0 ";

				cmd.CommandText =
					@"Select i.ITEMBARCODE, i.ITEMBARCODEID, i.ITEMID, i.RBOVARIANTID,
					ISNULL(i.BARCODESETUPID,'') as BARCODESETUPID,ISNULL(i.RBOSHOWFORITEM,0) as RBOSHOWFORITEM,
					ISNULL(i.USEFORINPUT,0) as USEFORINPUT, ISNULL(i.USEFORPRINTING,0) as USEFORPRINTING,
					ISNULL(ic.INVENTCOLORID,'') as COLORID,ISNULL(ic.INVENTSTYLEID,'') as STYLEID,
					ISNULL(ic.INVENTSIZEID,'') as SIZEID,ISNULL(c.NAME,'') as COLORNAME,
					ISNULL(s.NAME,'') as SIZENAME,ISNULL(t.NAME,'') as STYLENAME,
					ISNULL(i.INVENTDIMID,'') as INVENTDIMID,ISNULL(bs.DESCRIPTION,'') as BARCODESETUPDESCRIPTION,
					ISNULL(i.UNITID,'') as UNITID,ISNULL(u.TXT,'') as UNITDESCRIPTION, 
					ISNULL(i.QTY, 0) as QTY 
					from INVENTITEMBARCODE i 
					left outer join INVENTDIMCOMBINATION ic on  i.RBOVARIANTID = ic.RBOVARIANTID and i.DATAAREAID = ic.DATAAREAID 
					left outer join RBOCOLORS c ON ic.INVENTCOLORID = c.COLOR AND ic.DATAAREAID = c.DATAAREAID 
					left outer join RBOSIZES s ON ic.INVENTSIZEID = s.SIZE_ AND ic.DATAAREAID = s.DATAAREAID 
					left outer join RBOSTYLES t ON ic.INVENTSTYLEID = t.STYLE AND ic.DATAAREAID = t.DATAAREAID 
					left outer join BARCODESETUP bs on i.BARCODESETUPID = bs.BARCODESETUPID and i.DATAAREAID = bs.DATAAREAID 
					left outer join UNIT u on i.UNITID = u.UNITID and i.DATAAREAID = u.DATAAREAID 
					where i.RBOVARIANTID = @variantID and i.DATAAREAID = @dataAreaID " + deletedBarcodes;

				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "variantID", (string)variantID);

				return Execute<BarCode>(entry, cmd, CommandType.Text, PopulateBarCodeVariant);
			}
		}

		/// <summary>
		/// Returns a list of barcodes that exist for all variants of the given header item ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemID">The master ID of the header item</param>
		/// <param name="getDeletedBarcodes"></param>
		/// <returns></returns>
		public virtual List<BarCode> GetListForHeaderItem(IConnectionManager entry, RecordIdentifier itemID, bool getDeletedBarcodes = false)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				string deletedBarcodes = getDeletedBarcodes ? "" : " and i.DELETED = 0 ";

				cmd.CommandText =
					BaseSQL +
					@"where i.ITEMID in
					  ( select rit2.ITEMID
						  from RETAILITEM rit2
						  where rit2.HEADERITEMID = @itemID
					  )" + deletedBarcodes;

				MakeParam(cmd, "itemID", (Guid)itemID);

				return Execute<BarCode>(entry, cmd, CommandType.Text, PopulateBarCode);
			}
		}

		public virtual string GetBarcodeWithShowForItem(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = @"Select ITEMBARCODE from INVENTITEMBARCODE where ITEMID = @itemID and DELETED = 0 and DATAAREAID = @dataAreaID Order by RBOSHOWFORITEM DESC, ITEMBARCODE ASC";
				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "itemID", (string)itemID);

				using (var reader = entry.Connection.ExecuteReader(cmd, CommandType.Text, CommandBehavior.Default))
				{
					while (reader.Read())
					{
						return AsString(reader["ITEMBARCODE"]);
					}
				}
				return null;
			}
		}

		public virtual void AddInformationToBarcode(IConnectionManager entry, BarCode barCode)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				var barcodeString = (string)barCode.ItemBarCode;
				cmd.CommandText = @"SELECT B.ITEMID,
										   B.ITEMBARCODEID,
										   B.DESCRIPTION,
										   B.INVENTDIMID, 
										   ISNULL(B.RBOVARIANTID, '') RBOVARIANTID,
										   B.BLOCKED, 
										   B.UNITID, 
										   B.QTY                                           
									FROM INVENTITEMBARCODE B                                     
									WHERE B.ITEMBARCODE <COMPARISON> @ItemBarcode AND B.DELETED = 0 AND B.DATAAREAID=@dataAreaID "
					.Replace("<COMPARISON>", barcodeString.StartsWith("%") || barcodeString.EndsWith("%") ? "LIKE" : "=");

				if (!RecordIdentifier.IsEmptyOrNull(barCode.ItemID))
				{
					cmd.CommandText = String.Concat(cmd.CommandText, " AND B.ITEMID = @ItemID ");
					MakeParam(cmd, "ItemID", barCode.ItemID);
				}

				// Can have a significant impact on speed - the LIKE query will require a table scan, while the = query can use an index lookup
				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "ItemBarcode", barCode.ItemBarCode);
				barCode.Message = "Barcode not found";
				barCode.Found = false;

				Execute(entry, cmd, CommandType.Text, barCode, PopulateAdditionalInformationToBarcode);

			}

		}

		public virtual List<BarCode> GetListOfBarCodes(IConnectionManager entry, BarCode barCode)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				var barcodeString = (string)barCode.ItemBarCode;
				cmd.CommandText = @"Select DISTINCT i.ITEMBARCODE, 
										 i.ITEMID,
										 rit.ITEMNAME
								  from INVENTITEMBARCODE i
								  inner
								  join RETAILITEM rit on rit.ITEMID = i.ITEMID 
				
									WHERE i.ITEMBARCODE <COMPARISON> @ItemBarcode AND i.DELETED = 0 AND i.DATAAREAID=@dataAreaID "
									.Replace("<COMPARISON>", barcodeString.StartsWith("%") || barcodeString.EndsWith("%") ? "LIKE" : "=");

				if (!RecordIdentifier.IsEmptyOrNull(barCode.ItemID))
				{
					cmd.CommandText = String.Concat(cmd.CommandText, " AND i.ITEMID = @ItemID ");
					MakeParam(cmd, "ItemID", barCode.ItemID);
				}

				// Can have a significant impact on speed - the LIKE query will require a table scan, while the = query can use an index lookup
				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "ItemBarcode", barCode.ItemBarCode);
				barCode.Message = "Barcode not found";
				barCode.Found = false;

				return Execute<BarCode>(entry, cmd, CommandType.Text, PopulateBarCodeForItemList);
			
			}
		
		}


		/// <summary>
		/// Processes the different barcode segments
		/// </summary>
		public virtual List<BarcodeMaskSegment> GetBarcodeSegments(IConnectionManager entry, BarCode barCode)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = @"SELECT ISNULL(LENGTH, 0) AS LENGTH, ISNULL(TYPE, 0) AS TYPE, MASKID, 
									ISNULL(DECIMALS, 0) AS DECIMALS, ISNULL(CHAR, '') AS CHAR, SEGMENTNUM 
									FROM RBOBARCODEMASKSEGMENT 
									WHERE MASKID = @maskID AND DATAAREAID = @dataAreaID 
									ORDER BY SEGMENTNUM ";
				MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "maskID", barCode.MaskId);
				return Execute<BarcodeMaskSegment>(entry, cmd, CommandType.Text, PopulateMaskSegment);
			}
		}

		/// <summary>
		/// Deletes a item barcode with a given itemID
		/// </summary>
		/// <remarks>Requires the 'Edit items' permission</remarks>
		/// <param name="entry">The entry into the database</param>
		/// <param name="id">The ID of the bar code to delete</param>
		public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
		{
			MarkAsDeleted(entry, "INVENTITEMBARCODE", "ITEMBARCODEID", id, Permission.ItemsEdit);
		}

		/// <summary>
		/// Deletes a item barcode with a given itemID
		/// </summary>
		/// <remarks>Requires the 'Edit items' permission</remarks>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemId">The ID of the item that has the barcode to be deleted</param>
		public virtual void DeleteWithItemID(IConnectionManager entry, RecordIdentifier itemId)
		{
			MarkAsDeleted(entry, "INVENTITEMBARCODE", "ITEMID", itemId, Permission.ItemsEdit);
		}

		public virtual void UndeleteBarcodeWithItemID(IConnectionManager entry, RecordIdentifier itemID)
		{
			ValidateSecurity(entry, Permission.ItemsEdit);

			SqlServerStatement statement = new SqlServerStatement("INVENTITEMBARCODE");
			statement.StatementType = StatementType.Update;

			statement.AddField("DELETED", false, SqlDbType.Bit);
			statement.AddCondition("ITEMID", itemID.DBValue, itemID.DBType);

			entry.Connection.ExecuteStatement(statement);
		}

		public virtual void UndeleteBarcode(IConnectionManager entry, RecordIdentifier barcode)
		{
			ValidateSecurity(entry, Permission.ItemsEdit);

			SqlServerStatement statement = new SqlServerStatement("INVENTITEMBARCODE");
			statement.StatementType = StatementType.Update;

			statement.AddField("DELETED", false, SqlDbType.Bit);
			statement.AddCondition("ITEMBARCODE", barcode.DBValue, barcode.DBType);

			entry.Connection.ExecuteStatement(statement);
		}

		public bool IsDeleted(IConnectionManager entry, RecordIdentifier barcode)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"select DELETED
					  from INVENTITEMBARCODE
					  where ITEMBARCODE = @itemBarcode";

				MakeParam(cmd, "itemBarcode", barcode.DBValue, barcode.DBType);

				return (bool)entry.Connection.ExecuteScalar(cmd);
			}
		}

		/// <summary>
		/// Checks if a barcode with a given barcode string exists
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="barCode">The id of the barcode to checkfor</param>
		/// <returns></returns>
		public virtual bool Exists(IConnectionManager entry, RecordIdentifier barCode)
		{
			return RecordExists(entry, "INVENTITEMBARCODE", "ITEMBARCODE", barCode);
		}

		/// <summary>
		/// Checks if a barcode with a given ID exists
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="barCode">The id of the barcode to checkfor</param>
		/// <returns></returns>
		public virtual bool ExistsWithID(IConnectionManager entry, RecordIdentifier barCode)
		{
			return RecordExists(entry, "INVENTITEMBARCODE", "ITEMBARCODEID", barCode);
		}

		public virtual void Save(IConnectionManager entry, BarCode barCode)
		{
			var statement = new SqlServerStatement("INVENTITEMBARCODE");
			statement.UpdateColumnOptimizer = barCode;

			ValidateSecurity(entry, BusinessObjects.Permission.ManageItemBarcodes);

			barCode.Validate();

			bool isNew = false;
			if (barCode.ItemBarcodeID.IsEmpty)
			{
				isNew = true;
				barCode.ItemBarcodeID = DataProviderFactory.Instance.GenerateNumber<IBarCodeData, BarCode>(entry);
			}

			if (isNew || !ExistsWithID(entry, barCode.ItemBarcodeID))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddKey("ITEMBARCODEID", (string)barCode.ItemBarcodeID);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("ITEMBARCODEID", (string)barCode.ItemBarcodeID);
				statement.AddField("DELETED", false, SqlDbType.Bit);
			}

			statement.AddField("ITEMBARCODE", (string)barCode.ItemBarCode);
			statement.AddField("ITEMID", (string)barCode.ItemID);
			statement.AddField("RBOVARIANTID", (string)barCode.VariantID);
			statement.AddField("INVENTDIMID", (string)barCode.InventDimID);
			statement.AddField("BARCODESETUPID", (string)barCode.BarCodeSetupID);
			statement.AddField("RBOSHOWFORITEM", barCode.ShowForItem ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("USEFORINPUT", barCode.UseForInput ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("USEFORPRINTING", barCode.UseForPrinting ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("UNITID", (string)barCode.UnitID);
			statement.AddField("QTY", (double)barCode.Quantity, SqlDbType.Decimal);
			Save(entry, barCode, statement);
		}

		/// <summary>
		/// Returns a list of all barcodes
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <returns></returns>
		public List<BarCode> GetAllBarcodes(IConnectionManager entry)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = BaseSQL;

				return Execute<BarCode>(entry, cmd, CommandType.Text, PopulateBarCode);
			}
		}

        [LSOneUsage(CodeUsage.LSCommerce)]
		public List<BarCode> LoadBarCodes(IConnectionManager entry, int rowFrom, int rowTo, out int totalRecordsMatching)
		{
			List<TableColumn> listColumns = new List<TableColumn>
			{
				new TableColumn {ColumnName = "ITEMBARCODE", TableAlias = "I"},
				new TableColumn {ColumnName = "ITEMBARCODEID", TableAlias = "I"},
				new TableColumn {ColumnName = "CASE WHEN RIT.HEADERITEMID  IS NOT NULL THEN HEADER.ITEMID ELSE  RIT.ITEMID END ",ColumnAlias = "ITEMID"},
				new TableColumn {ColumnName = "CASE WHEN RIT.HEADERITEMID  IS NOT NULL THEN RIT.ITEMID ELSE RBOVARIANTID END ", ColumnAlias = "RBOVARIANTID"},
				new TableColumn {ColumnName = "BARCODESETUPID", TableAlias = "I",IsNull = true,NullValue = "''"},
				new TableColumn {ColumnName = "RBOSHOWFORITEM", TableAlias = "I",IsNull = true,NullValue = "0"},
				new TableColumn {ColumnName = "USEFORINPUT", TableAlias = "I",IsNull = true,NullValue = "0"},
				new TableColumn {ColumnName = "USEFORPRINTING", TableAlias = "I",IsNull = true,NullValue = "0"},
				new TableColumn {ColumnName = "DESCRIPTION", TableAlias = "BS",IsNull = true,NullValue = "''", ColumnAlias = "BARCODESETUPDESCRIPTION"},
				new TableColumn {ColumnName = "UNITID", TableAlias = "I",IsNull = true,NullValue = "''"},
				new TableColumn {ColumnName = "TXT", TableAlias = "U",IsNull = true,NullValue = "''",ColumnAlias = "UNITDESCRIPTION"},
				new TableColumn {ColumnName = "QTY", TableAlias = "I",IsNull = true,NullValue = "0"},
				new TableColumn {ColumnName = "ITEMNAME", TableAlias = "RIT"},
				new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "RIT"},
				new TableColumn {ColumnName = "DELETED", TableAlias = "I"},
            };

			using (var cmd = entry.Connection.CreateCommand())
			{
				List<TableColumn> columns = new List<TableColumn>();

				foreach (var selectionColumn in listColumns)
				{
					columns.Add(selectionColumn);
				}
				columns.Add(new TableColumn
				{
					ColumnName = "ROW_NUMBER() OVER(order by I.ITEMBARCODE)",
					ColumnAlias = "ROW"
				});
				columns.Add(new TableColumn
				{
					ColumnName =
						"COUNT(1) OVER(ORDER BY I.ITEMBARCODE RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING)",
					ColumnAlias = "ROW_COUNT"
				});

                List<Condition> internalConditions = new List<Condition>()
                {
                    new Condition { Operator = "AND", ConditionValue = "I.DELETED = 0"},
                    new Condition { Operator = "AND", ConditionValue = "RIT.ITEMID IS NOT NULL"},
                    new Condition { Operator = "AND", ConditionValue = "RIT.DELETED = 0"}
                };

				List<Condition> externalConditions = new List<Condition>();
				externalConditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = "S.ROW BETWEEN @rowFrom AND @rowTo"
				});

				List<Join> joins = new List<Join>();

				joins.Add(new Join
				{
					Condition = " I.BARCODESETUPID = BS.BARCODESETUPID AND I.DATAAREAID = BS.DATAAREAID",
					JoinType = "LEFT OUTER",
					Table = "BARCODESETUP",
					TableAlias = "BS"
				});
				joins.Add(new Join
				{
					Condition = "I.UNITID = U.UNITID AND I.DATAAREAID = U.DATAAREAID",
					JoinType = "LEFT OUTER",
					Table = "UNIT",
					TableAlias = "U"
				});
				joins.Add(new Join
				{
					Condition = "RIT.ITEMID = I.ITEMID",
					JoinType = "LEFT OUTER",
					Table = "RETAILITEM",
					TableAlias = "RIT"
				});
				joins.Add(new Join
				{
					Condition = "RIT.HEADERITEMID = HEADER.MASTERID",
					JoinType = "LEFT OUTER",
					Table = "RETAILITEM",
					TableAlias = "HEADER"
				});

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("INVENTITEMBARCODE", "I", "S"),
					QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(internalConditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
					string.Empty);

				MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
				MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
				int matchingRecords = 0;

                List<BarCode> results = Execute<BarCode, int>(entry, cmd, CommandType.Text, ref matchingRecords, PopulateBarCodeWithCount);

				totalRecordsMatching = matchingRecords;

				return results;
			}
		}

		public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
		{
			return ExistsWithID(entry, id);
		}

		/// <summary>
		/// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemsToCompare">List of items you want to get from the database matching items</param>
		/// <returns>List of items</returns>
		public virtual List<BarCode> GetCompareList(IConnectionManager entry, List<BarCode> itemsToCompare)
		{
			var columns = new List<TableColumn>
			{
				new TableColumn {ColumnName = "ITEMBARCODE", TableAlias = "I"},
				new TableColumn {ColumnName = "ITEMBARCODEID", TableAlias = "I"},
				new TableColumn {ColumnName = "CASE WHEN RIT.HEADERITEMID  IS NOT NULL THEN HEADER.ITEMID ELSE  RIT.ITEMID END ", ColumnAlias = "ITEMID"},
				new TableColumn {ColumnName = "CASE WHEN RIT.HEADERITEMID  IS NOT NULL THEN RIT.ITEMID ELSE RBOVARIANTID END ", ColumnAlias = "RBOVARIANTID"},
				new TableColumn {ColumnName = "BARCODESETUPID", TableAlias = "I",IsNull = true,NullValue = "''"},
				new TableColumn {ColumnName = "RBOSHOWFORITEM", TableAlias = "I",IsNull = true,NullValue = "0"},
				new TableColumn {ColumnName = "USEFORINPUT", TableAlias = "I",IsNull = true,NullValue = "0"},
				new TableColumn {ColumnName = "USEFORPRINTING", TableAlias = "I",IsNull = true,NullValue = "0"},
				new TableColumn {ColumnName = "DESCRIPTION", TableAlias = "BS",IsNull = true,NullValue = "''", ColumnAlias = "BARCODESETUPDESCRIPTION"},
				new TableColumn {ColumnName = "UNITID", TableAlias = "I",IsNull = true,NullValue = "''"},
				new TableColumn {ColumnName = "TXT", TableAlias = "U",IsNull = true,NullValue = "''", ColumnAlias = "UNITDESCRIPTION"},
				new TableColumn {ColumnName = "QTY", TableAlias = "I",IsNull = true,NullValue = "0"},
				new TableColumn {ColumnName = "ITEMNAME", TableAlias = "RIT"},
				new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "RIT"},
				new TableColumn {ColumnName = "DELETED", TableAlias = "I"},
            };

			List<Join> joins = new List<Join>();

			joins.Add(new Join
			{
				Condition = " I.BARCODESETUPID = BS.BARCODESETUPID AND I.DATAAREAID = BS.DATAAREAID",
				JoinType = "LEFT OUTER",
				Table = "BARCODESETUP",
				TableAlias = "BS"
			});
			joins.Add(new Join
			{
				Condition = "I.UNITID = U.UNITID AND I.DATAAREAID = U.DATAAREAID",
				JoinType = "LEFT OUTER",
				Table = "UNIT",
				TableAlias = "U"
			});
			joins.Add(new Join
			{
				Condition = "RIT.ITEMID = I.ITEMID",
				JoinType = "LEFT OUTER",
				Table = "RETAILITEM",
				TableAlias = "RIT"
			});
			joins.Add(new Join
			{
				Condition = "RIT.HEADERITEMID = HEADER.MASTERID",
				JoinType = "LEFT OUTER",
				Table = "RETAILITEM",
				TableAlias = "HEADER"
			});

			return GetCompareListInBatches(entry, itemsToCompare, "INVENTITEMBARCODE", "ITEMBARCODE", columns, joins, PopulateBarCode);
		}

        /// <summary>
        /// Remove all barcodes from the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        public void DeleteAll(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "TRUNCATE TABLE INVENTITEMBARCODE";

                entry.Connection.ExecuteNonQuery(cmd, false, CommandType.Text);
            }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "INVENTITEMBARCODE", "ITEMBARCODEID", sequenceFormat, startingRecord, numberOfRecords);
        }
    }
}