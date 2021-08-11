using System.Collections.Generic;
using System.Data;
using System.Linq;

using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Tax
{
	/// <summary>
	/// Data provider class for sales tax groups
	/// </summary>
	public class SalesTaxGroupData : SqlServerDataProviderBase, ISalesTaxGroupData
	{
		private static string ResolveTaxGroupSort(SalesTaxGroup.SortEnum sortEnum, bool sortBackwards)
		{
			string sortString = "";

			switch (sortEnum)
			{
				case SalesTaxGroup.SortEnum.ID:
					sortString = " Order By TAXGROUP ASC";
					break;
				case SalesTaxGroup.SortEnum.Description:
					sortString = " Order By TAXGROUPNAME ASC";
					break;
				case SalesTaxGroup.SortEnum.CountryRegion:
					sortString = " Order By SEARCHFIELD1 ASC";
					break;
				case SalesTaxGroup.SortEnum.CountyPurpose:
					sortString = " Order By SEARCHFIELD2 ASC";
					break;
				case SalesTaxGroup.SortEnum.IsForEU:
					sortString = " Order By ISFOREU ASC";
					break;
			}

			if (sortBackwards)
			{
				sortString = sortString.Replace("ASC","DESC");
			}

			return sortString;
		}

		private static string ResolveTaxCodeInSalesTaxGroupSort(TaxCodeInSalesTaxGroup.SortEnum sortEnum, bool sortBackwards)
		{
			string sortString = "";

			switch (sortEnum)
			{
				case TaxCodeInSalesTaxGroup.SortEnum.SalesTaxCode:
					sortString = " Order By tgd.TAXCODE ASC";
					break;
				case TaxCodeInSalesTaxGroup.SortEnum.Description:
					sortString = " Order By tt.TAXNAME ASC";
					break;
				case TaxCodeInSalesTaxGroup.SortEnum.PercentageAmount:
					sortString = " Order By td.TAXVALUE ASC";
					break;
			}

			if (sortBackwards)
			{
				sortString = sortString.Replace("ASC", "DESC");
			}

			return sortString;
		}

		private void PopulateDataEntity(IDataReader dr, DataEntity taxGroup)
		{
			taxGroup.ID = (string)dr["TAXGROUP"];
			taxGroup.Text = (string)dr["TAXGROUPNAME"];
		}

		private static void PopulateSalesTaxGroup(IDataReader dr, SalesTaxGroup salesTaxGroup)
		{
			salesTaxGroup.ID = (string)dr["TAXGROUP"];
			salesTaxGroup.Text = (string)dr["TAXGROUPNAME"];
			salesTaxGroup.SearchField1 = (string)dr["SEARCHFIELD1"];
			salesTaxGroup.SearchField2 = (string)dr["SEARCHFIELD2"];
			salesTaxGroup.IsForEU = (bool)dr["ISFOREU"];
		}

		private static void PopulateTaxCodeInSalesTaxGroup(IDataReader dr, TaxCodeInSalesTaxGroup line)
		{
			line.SalesTaxGroup = (string)dr["TAXGROUP"];
			line.TaxCode = (string)dr["TAXCODE"];
			line.Text = (string)dr["TAXNAME"];
			line.TaxValue = (decimal)dr["TAXVALUE"];
		}

		private static bool TaxCodeIsInSalesTaxGroup(IConnectionManager entry, RecordIdentifier id)
		{
			return RecordExists(entry, "TAXGROUPDATA", new[] { "TAXGROUP", "TAXCODE" }, id);
		}

		/// <summary>
		/// Check if Tax Group is in use in a store, a customer or a sales type
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="salesTaxGroupID">The ID of the sales tax group to get</param>
		/// <returns>True if Tax Group is in use</returns>
		public virtual bool TaxGroupInUse(IConnectionManager entry, RecordIdentifier salesTaxGroupID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"select ((select count(taxgroup) from customer where TAXGROUP = @taxGroup) +
					  (select count(taxgroup) from RBOSTORETABLE where TAXGROUP = @taxGroup) +
					  (select count(taxgroupid) from SALESTYPE where TAXGROUPID = @taxGroup))";

				MakeParam(cmd, "taxGroup", (string)salesTaxGroupID);
				return (int)entry.Connection.ExecuteScalar(cmd) > 0;
			}
		}

		/// <summary>
		/// Gets an item sales tax group by a given ID
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="salesTaxGroupID">The ID of the sales tax group to get</param>
		/// <param name="cacheType">Cache</param>
		/// <returns>A sales tax group by a given ID</returns>
		public virtual SalesTaxGroup Get(IConnectionManager entry, RecordIdentifier salesTaxGroupID, CacheType cacheType = CacheType.CacheTypeNone)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"SELECT g.TAXGROUP,ISNULL(g.TAXGROUPNAME,'') as TAXGROUPNAME , ISNULL(g.SEARCHFIELD1,'') as SEARCHFIELD1, 
							ISNULL(g.SEARCHFIELD2,'') as SEARCHFIELD2, g.ISFOREU  
					FROM TAXGROUPHEADING g 
					WHERE g.TAXGROUP = @taxGroup AND g.DATAAREAID = @dataAreaId";

				MakeParam(cmd, "taxGroup", (string)salesTaxGroupID);
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				return Get<SalesTaxGroup>(entry, cmd, salesTaxGroupID, PopulateSalesTaxGroup, cacheType, UsageIntentEnum.Normal);
			}
		}

		/// <summary>
		/// Gets a list of data entities containing IDs and names of all sales tax groups
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="groupType">Filter results by group type</param>
		/// <returns>A list of data entities containing IDs and names of all sales tax groups</returns>
		public virtual List<DataEntity> GetList(IConnectionManager entry, TaxGroupTypeFilter groupType = TaxGroupTypeFilter.All)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
				   @"SELECT TH.TAXGROUP, TH.TAXGROUPNAME
					 FROM TAXGROUPHEADING TH
					 WHERE TH.DATAAREAID = @dataAreaId ";

				if(groupType != TaxGroupTypeFilter.All)
				{
					cmd.CommandText += $"AND TH.ISFOREU = {(groupType == TaxGroupTypeFilter.Standard ? 0 : 1)}";
				}

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				return GetList<DataEntity>(entry, cmd, RecordIdentifier.Empty, PopulateDataEntity, CacheType.CacheTypeNone);
			}
		}

		/// <summary>
		/// Gets a list of data entities containing IDs and names of all sales tax groups that have sales tax codes
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="groupType">Filter results by group type</param>
		/// <returns>A list of data entities containing IDs and names of all sales tax groups</returns>
		public virtual List<DataEntity> GetListWithTaxCodes(IConnectionManager entry, TaxGroupTypeFilter groupType = TaxGroupTypeFilter.All)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
				   $@"select distinct TH.TAXGROUP, TH.TAXGROUPNAME
					 from TAXGROUPHEADING TH
					 join TAXGROUPDATA TD on TD.TAXGROUP = TH.TAXGROUP AND TD.DATAAREAID = TH.DATAAREAID
					 where TH.DATAAREAID = @dataAreaId ";

				if (groupType != TaxGroupTypeFilter.All)
				{
					cmd.CommandText += $"AND TH.ISFOREU = {(groupType == TaxGroupTypeFilter.Standard ? 0 : 1)}";
				}

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				return GetList<DataEntity>(entry, cmd, RecordIdentifier.Empty, PopulateDataEntity, CacheType.CacheTypeNone);
			}
		}

		/// <summary>
		/// Gets a list of data entities containing IDs and names of all sales tax groups, ordered by 
		/// a sort enum and a reversed ordering based on a parameter
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="sortEnum">The enum that determines what to sort by</param>
		/// <param name="backwardsSort">Whether the result set is ordered backwards</param>
		/// <param name="cacheType">Cache</param>
		/// <returns>A list of data entities containing IDs and names of all sales tax groups, meeting the above criteria</returns>
		public virtual List<SalesTaxGroup> GetSalesTaxGroups(IConnectionManager entry, SalesTaxGroup.SortEnum sortEnum, bool backwardsSort, CacheType cacheType = CacheType.CacheTypeNone)
		{
			ValidateSecurity(entry);
			RecordIdentifier id = new RecordIdentifier("GetSalesTaxGroup", new RecordIdentifier((int)sortEnum, backwardsSort));

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
				   @"SELECT TAXGROUP, ISNULL(TAXGROUPNAME,'') as TAXGROUPNAME , ISNULL(SEARCHFIELD1,'') as SEARCHFIELD1,
							ISNULL(SEARCHFIELD2,'') as SEARCHFIELD2, ISFOREU 
					FROM TAXGROUPHEADING 
					WHERE DATAAREAID = @dataAreaId" + ResolveTaxGroupSort(sortEnum, backwardsSort);

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				return GetList<SalesTaxGroup>(entry, cmd, id, PopulateSalesTaxGroup, cacheType);
			}
		}

		/// <summary>
		/// Checks if a sales tax group with a given ID exists in the database
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="salesTaxGroupID">The ID of the sales tax group to check for</param>
		/// <returns>Whether a sales tax group with a given ID exists in the database</returns>
		public virtual bool Exists(IConnectionManager entry, RecordIdentifier salesTaxGroupID)
		{
			return RecordExists(entry, "TAXGROUPHEADING", "TAXGROUP", salesTaxGroupID);
		}

		/// <summary>
		/// Saves a given sales tax group to the database
		/// </summary>
		/// <remarks>Requires the 'Edit sales tax setup' permission</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="salesTaxGroup">The sales tax group to save</param>
		public virtual void Save(IConnectionManager entry, SalesTaxGroup salesTaxGroup)
		{
			var statement = new SqlServerStatement("TAXGROUPHEADING");

			ValidateSecurity(entry, Permission.EditSalesTaxSetup);

			bool isNew = false;
			if (salesTaxGroup.ID.IsEmpty)
			{
				isNew = true;
				salesTaxGroup.ID = DataProviderFactory.Instance.GenerateNumber<ISalesTaxGroupData, SalesTaxGroup>(entry);
			}

			if (isNew || !Exists(entry, salesTaxGroup.ID))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddKey("TAXGROUP", (string)salesTaxGroup.ID);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("TAXGROUP", (string)salesTaxGroup.ID);                
            }

			statement.AddField("TAXGROUPNAME", salesTaxGroup.Text);
			statement.AddField("SEARCHFIELD1", salesTaxGroup.SearchField1);
			statement.AddField("SEARCHFIELD2", salesTaxGroup.SearchField2);
			statement.AddField("ISFOREU", salesTaxGroup.IsForEU, SqlDbType.Bit);

			entry.Connection.ExecuteStatement(statement);

            if (!isNew)
            {
                // Updating the sales tax group data means that we have to invalidate the entire cache
                entry.Cache.InvalidateEntityCache();
            }
		}

		/// <summary>
		/// Deletes a sales tax group by a given ID
		/// </summary>
		/// <remarks>Requires the 'Edit sales tax setup' permission</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="salesTaxGroupID">The ID of the sales tax group to delete</param>
		public virtual void Delete(IConnectionManager entry, RecordIdentifier salesTaxGroupID)
		{
			DeleteRecord(entry, "TAXGROUPHEADING", "TAXGROUP", salesTaxGroupID, Permission.EditSalesTaxSetup);
			DeleteRecord(entry, "TAXGROUPDATA", "TAXGROUP", salesTaxGroupID, Permission.EditSalesTaxSetup);

            // Deletion of tax data means that we have to invalidate the entire cache
            entry.Cache.InvalidateEntityCache();
        }

		/// <summary>
		/// Gets a list either of tax codes in a sales tax group or tax codes not in a sales tax group. Returns data entities of the tax codes.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="salesTaxGroupID">The ID of the item sales tax group</param>
		/// <param name="inTaxGroup">Whether to search for tax codes in group or tax codes not in group</param>
		/// <param name="cacheType">Cache</param>
		public virtual List<DataEntity> GetTaxCodesInSalesTaxGroupList(IConnectionManager entry, RecordIdentifier salesTaxGroupID, bool inTaxGroup, CacheType cacheType = CacheType.CacheTypeNone)
		{
			RecordIdentifier id = new RecordIdentifier("GetTaxCodesInSalesTaxGroupList", new RecordIdentifier(salesTaxGroupID, inTaxGroup));

			if (cacheType != CacheType.CacheTypeNone)
			{
				CacheBucket bucket = (CacheBucket)entry.Cache.GetEntityFromCache(typeof(CacheBucket), id);
				if (bucket != null)
				{
					return (List<DataEntity>)bucket.BucketData;
				}                   
            }
			List<DataEntity> result = null;
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = "Select t.TAXCODE,ISNULL(t.TAXNAME,'') as TAXNAME from TAXTABLE t " +
								  "where ";
				if (!inTaxGroup)
				{
					cmd.CommandText += " not ";
				}

				cmd.CommandText +=
					"Exists(Select 'x' from TAXGROUPDATA tt where TAXGROUP = @taxGroup and t.TAXCODE = tt.TAXCODE and tt.DATAAREAID = @dataAreaId) " +
					"and t.DATAAREAID = @dataAreaId order by t.TAXNAME";

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "taxGroup", (string) salesTaxGroupID);

				result = Execute<DataEntity>(entry, cmd, CommandType.Text, "TAXNAME", "TAXCODE");
			}

			if (result != null && cacheType != CacheType.CacheTypeNone)
			{
				entry.Cache.AddEntityToCache(id, new CacheBucket(id, "", result), cacheType);
		}
			return result;
		}


		/// <summary>
		/// Gets a list of TaxCode-SalesTaxGroup connections for a given sales tax group
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="salesTaxGroupID">ID of the item sales tax group</param>
		/// <param name="sortEnum">Determines the sort ordering of the results</param>
		/// <param name="backwardsSort">Determines if the results will be in reversed ordering</param>
		/// <param name="cacheType">Cache</param>
		/// <returns>A list of TaxCode-SalesTaxGroup connections for a given sales tax group</returns>
		public virtual List<TaxCodeInSalesTaxGroup> GetTaxCodesInSalesTaxGroup(IConnectionManager entry, RecordIdentifier salesTaxGroupID, TaxCodeInSalesTaxGroup.SortEnum sortEnum, bool backwardsSort, CacheType cacheType = CacheType.CacheTypeNone)
		{
			ValidateSecurity(entry);

			RecordIdentifier id = new RecordIdentifier("GetTaxCodesInSalesTaxGroup", new RecordIdentifier(salesTaxGroupID, new RecordIdentifier((int)sortEnum, backwardsSort)));

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"select tgd.TAXCODE, ISNULL(tgd.EXEMPTTAX,0) as EXEMPTTAX,
					ISNULL(tt.TAXNAME,0) as TAXNAME, ISNULL(td.TAXVALUE,-1) as TAXVALUE , tgd.TAXGROUP  
					from TAXGROUPDATA tgd 
					left outer join TAXTABLE tt on tgd.TAXCODE = tt.TAXCODE and tgd.DATAAREAID = tt.DATAAREAID 
					left outer join (select TAXCODE, TAXVALUE, DATAAREAID from TAXDATA 
									 where ((TAXFROMDATE = '1900-01-01 00:00:00.000' and TAXTODATE = '1900-01-01 00:00:00.000') or 
										   (GETDATE() >= TAXFROMDATE and TAXTODATE = '1900-01-01 00:00:00.000') or 
										   ( GETDATE() >= TAXFROMDATE and GETDATE() <= TAXTODATE))) 
									td on td.TAXCODE = tgd.TAXCODE and td.DATAAREAID = tgd.DATAAREAID 
					where tgd.TAXGROUP = @taxGroup and tgd.DATAAREAID = @dataAreaId " + ResolveTaxCodeInSalesTaxGroupSort(sortEnum, backwardsSort) ;

				MakeParam(cmd, "taxGroup", (string)salesTaxGroupID);
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				return GetList<TaxCodeInSalesTaxGroup>(entry, cmd, id, PopulateTaxCodeInSalesTaxGroup, cacheType);
			}
		}

		/// <summary>
		/// Adds a tax code to a sales tax group.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="item">Contains IDs of the tax code and the item sales tax group</param>
		public virtual void AddTaxCodeToSalesTaxGroup(IConnectionManager entry, TaxCodeInSalesTaxGroup item)
		{
			var statement = new SqlServerStatement("TAXGROUPDATA");

			ValidateSecurity(entry, Permission.EditSalesTaxSetup);

			if (TaxCodeIsInSalesTaxGroup(entry, item.ID)) //nothing to update
			{
				return;
			}
			statement.StatementType = StatementType.Insert;

			statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
			statement.AddKey("TAXGROUP", (string) item.SalesTaxGroup);
			statement.AddKey("TAXCODE", (string) item.TaxCode);

			entry.Connection.ExecuteStatement(statement);
		}

		/// <summary>
		/// Removes a tax code from a sales tax group
		/// </summary>
		/// <remarks>Requires the 'Edit sales tax setup' permission</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="id">Contains (sales tax group ID, tax code ID)</param>
		public virtual void RemoveTaxCodeFromSalesTaxGroup(IConnectionManager entry, RecordIdentifier id)
		{
			DeleteRecord(entry, "TAXGROUPDATA", new[] { "TAXGROUP", "TAXCODE" }, id, BusinessObjects.Permission.EditSalesTaxSetup);

            // Deletion of tax data means that we have to invalidate the entire cache
            entry.Cache.InvalidateEntityCache();
        }

		public virtual bool TaxCodeIsInDefaultStoreTaxGroup(IConnectionManager entry, RecordIdentifier taxCodeID, CacheType cacheType = CacheType.CacheTypeNone)
		{
			RecordIdentifier id = new RecordIdentifier("TaxCodeIsInDefaultStoreTaxGroup", taxCodeID);
			if (cacheType != CacheType.CacheTypeNone)
			{
				DataEntity entity = (DataEntity)entry.Cache.GetEntityFromCache(typeof (DataEntity), id);
				if (entity != null)
				{
					return (bool)entity.ID;
				}
			}

			var defaultStoresTaxGroupID = Providers.StoreData.GetDefaultStoreSalesTaxGroup(entry);
			var taxCodesInDefaultStoreGroup = GetTaxCodesInSalesTaxGroupList(entry, defaultStoresTaxGroupID, true);

			bool result = taxCodesInDefaultStoreGroup.Any(x => x.ID == taxCodeID);
			if (cacheType != CacheType.CacheTypeNone)
			{
				entry.Cache.AddEntityToCache(id, new DataEntity(new RecordIdentifier(result), ""), cacheType);
		}
			return result;
		}

		/// <summary>
		/// Gets all the items from the database matching the list of <see cref="itemsToCompare" />
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="itemsToCompare">List of items you want to get from the database matching items <see cref="itemsToCompare"</param>
		/// <returns>List of items</returns>
		public virtual List<SalesTaxGroup> GetCompareList(IConnectionManager entry, List<SalesTaxGroup> itemsToCompare)
		{
			var columns = new List<TableColumn>
			{
				new TableColumn {ColumnName = "TAXGROUP", TableAlias = "T"},
				new TableColumn {ColumnName = "TAXGROUPNAME", TableAlias = "T"},
				new TableColumn {ColumnName = "SEARCHFIELD1", TableAlias = "T"},
				new TableColumn {ColumnName = "SEARCHFIELD2", TableAlias = "T"},
				new TableColumn {ColumnName = "ISFOREU", TableAlias = "T"},
			};

			return GetCompareListInBatches(entry, itemsToCompare, "TAXGROUPHEADING", "TAXGROUP", columns, null, PopulateSalesTaxGroup);
		}

		#region ISequenceable Members

		/// <summary>
		/// Wether a sales tax group with the given ID exists
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="id">ID of the sales tax group</param>
		/// <returns>Wether a sales tax group with the given ID exists</returns>
		public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
		{
			return Exists(entry, id);
		}

		/// <summary>
		/// Returns the name of the sequence
		/// </summary>
		public RecordIdentifier SequenceID
		{
			get { return "TAXGROUP"; }
		}

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "TAXGROUPHEADING", "TAXGROUP", sequenceFormat, startingRecord, numberOfRecords);
        }

		#endregion
	}
}