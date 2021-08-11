using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
	/// <summary>
	/// Data provider class for special groups
	/// </summary>
	public class SpecialGroupData : SqlServerDataProviderBase, ISpecialGroupData
	{
		private static List<TableColumn> itemColumns = new List<TableColumn>
		{
			new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
			new TableColumn {ColumnName = "ItemName", TableAlias = "A"},
		};

		private static List<TableColumn> listColumns = new List<TableColumn>
		{
			new TableColumn {ColumnName = "GROUPID", TableAlias = "sg"},
			new TableColumn {ColumnName = "NAME", TableAlias = "sg"},
		};

		private static List<TableColumn> masterIDListColumns = new List<TableColumn>
		{
			new TableColumn {ColumnName = "GROUPID", TableAlias = "sg"},
			new TableColumn {ColumnName = "NAME", TableAlias = "sg"},
			new TableColumn {ColumnName = "MASTERID", TableAlias = "sg"},
		};
		private static string ResolveItemSort(SpecialGroupItemSorting sort, bool backwards)
		{
			switch (sort)
			{
				case SpecialGroupItemSorting.ItemID:
					return "a.ITEMID" + (backwards ? " DESC" : " ASC");

				case SpecialGroupItemSorting.ItemName:
					return "a.ITEMNAME" + (backwards ? " DESC" : " ASC");
			}

			return "";
		}

		private static string ResolveSort(SpecialGroupSorting sort, bool backwards)
		{
			switch (sort)
			{
				case SpecialGroupSorting.GroupID:
					return "GROUPID"  + (backwards ? " DESC" : " ASC");

				case SpecialGroupSorting.GroupName:
					return "NAME" + (backwards ? " DESC" : " ASC");
			}

			return "";
		}
		private static void PopulateSpecialGroup(IDataReader dr, SpecialGroup group)
		{
			group.ID = (string)dr["GROUPID"];
			group.Text = (string)dr["NAME"];
			group.MasterID = (Guid)dr["MASTERID"];
		}
		private static void PopulateListMasterID(IDataReader dr, MasterIDEntity group)
		{
			group.ReadadbleID = (string)dr["GROUPID"];
			group.ID = (Guid)dr["MASTERID"];
			group.Text = (string)dr["NAME"];
		}

		private static void PopulateGroupList(IDataReader dr, DataEntity group)
		{
			group.ID = (string)dr["GROUPID"];
			group.Text = "";
		}

		private static void PopulateItemInGroup(IDataReader dr, ItemInGroup item)
		{
			item.ID = (string)dr["ITEMID"];
			item.Text = (string)dr["ITEMNAME"];
			item.Group = (string)dr["GROUPNAME"];
			item.VariantName = (string)dr["VARIANTNAME"];
		}

		private void AddItemToGroup(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier groupID)
		{
			if (ItemInSpecialGroup(entry, groupID, itemID)) return;

			var statement = new SqlServerStatement("SPECIALGROUPITEMS") { StatementType = StatementType.Insert };

			statement.AddKey("MEMBERMASTERID", GetMasterID(entry, itemID, "RETAILITEM", "ITEMID"), SqlDbType.UniqueIdentifier);
			statement.AddKey("GROUPMASTERID", GetMasterID(entry, groupID, "SPECIALGROUP", "GROUPID"), SqlDbType.UniqueIdentifier);
			statement.AddField("ITEMID", itemID.ToString());
			statement.AddField("GROUPID", (string)groupID);

			entry.Connection.ExecuteStatement(statement);
		}

		/// <summary>
		/// Gets a list of data entities containing IDs and names for all special groups, ordered by a chosen field
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="sortBy">A enumeration that defines how the result should be sorted</param>
		/// <param name="sortBackwords">A enumeration that defines how the result should be sorted</param>
		/// <returns>A list of all special groups, ordered by a chosen field</returns>
		public virtual List<DataEntity> GetList(IConnectionManager entry, SpecialGroupSorting sortBy, bool sortBackwords)
		{
			if (sortBy != SpecialGroupSorting.GroupID && sortBy != SpecialGroupSorting.GroupName)
			{
				throw new NotSupportedException();
			}

			return GetList<DataEntity>(entry, false,"SPECIALGROUP", "NAME", "GROUPID", ResolveSort(sortBy, sortBackwords),false);
		}
		public virtual List<MasterIDEntity> GetMasterIDList(IConnectionManager entry)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{

				List<TableColumn> columns = new List<TableColumn>();

				columns.Add(new TableColumn { ColumnName = "MASTERID", TableAlias = "A" });
				columns.Add(new TableColumn { ColumnName = "GROUPID", TableAlias = "A" });
				columns.Add(new TableColumn { ColumnName = "ISNULL(A.NAME,'')", TableAlias = "", ColumnAlias = "NAME" });


				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("SPECIALGROUP", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					string.Empty,
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);


				return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateListMasterID);
			}
		}
		/// <summary>
		/// Gets a list of of data entities containing IDs and names for all special groups, ordered by special group names
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <returns>A list of all special groups</returns>
		public virtual List<DataEntity> GetList(IConnectionManager entry)
		{
			return GetList<DataEntity>(entry, false,"SPECIALGROUP", "NAME", "GROUPID", "NAME",false);
		}

		/// <summary>
		/// Gets a data entity containing the id and name of a special group by a given ID
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="specialGroupId">The ID of the special group to fetch. This needs to be the readable group ID, not the GUID master ID</param>
		/// <returns>A special group entity or null if not found</returns>
		public virtual DataEntity Get(IConnectionManager entry, RecordIdentifier specialGroupId)
		{
			return GetDataEntity<DataEntity>(entry, "SPECIALGROUP", "NAME", "GROUPID", specialGroupId, false);
		}

		/// <summary>
		/// Gets a <see cref="MasterIDEntity"/> containing the master ID and name of the special group by a given master ID
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="specialGroupMasterId">The master ID of the special group to fetch.</param>
		/// <returns>A special group master entity or null if not found</returns>
		public virtual MasterIDEntity GetMasterIDEntity(IConnectionManager entry, RecordIdentifier specialGroupMasterId)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{

				List<TableColumn> columns = new List<TableColumn>();

				columns.Add(new TableColumn { ColumnName = "MASTERID", TableAlias = "A" });
				columns.Add(new TableColumn { ColumnName = "GROUPID", TableAlias = "A" });
				columns.Add(new TableColumn { ColumnName = "ISNULL(A.NAME,'')", TableAlias = "", ColumnAlias = "NAME" });


				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });
				conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.MASTERID = @masterID"});

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("SPECIALGROUP", "A"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					string.Empty,
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);

				MakeParam(cmd, "masterID", (Guid) specialGroupMasterId);

				List<MasterIDEntity> results = Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateListMasterID);

				return results.Count > 0 ? results[0] : null;
			}
		}


		public virtual SpecialGroup GetSpecialGroup(IConnectionManager entry, RecordIdentifier ID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				List<Condition> conditions = new List<Condition>();

                if(ID.IsGuid)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "sg.MASTERID = @MASTERID" });
                    MakeParam(cmd, "MASTERID", (Guid)ID, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "sg.GROUPID = @GROUPID" });
                    MakeParam(cmd, "GROUPID", ID.StringValue);
                }

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "sg.DELETED = 0 " });

				cmd.CommandText = string.Format(
					QueryTemplates.BaseQuery("SPECIALGROUP", "sg"),
					QueryPartGenerator.InternalColumnGenerator(masterIDListColumns),
					string.Empty,
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty
					);

				var specialGroups = Execute<SpecialGroup>(entry, cmd, CommandType.Text, PopulateSpecialGroup);
				return specialGroups.Count > 0 ? specialGroups[0] : null;
			}
		}

		/// <summary>
		/// Gets the number of items in a special group
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="groupId">The master ID of the special group</param>
		/// <returns></returns>
		public virtual int GetItemsInSpecialGroupCount(IConnectionManager entry, RecordIdentifier groupId)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				string conditionValue = string.Empty;
				if (groupId.DBType == SqlDbType.UniqueIdentifier)
				{
					conditionValue = "GROUPMASTERID = @groupID ";
					MakeParam(cmd, "groupID", (Guid)groupId, SqlDbType.UniqueIdentifier);
				}
				else
				{
					conditionValue = "GROUPID = @groupID ";
					MakeParam(cmd, "groupID", groupId);
				}

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("SPECIALGROUPITEMS", "SGI"),
					new TableColumn { ColumnName = "COUNT(*)" },
					string.Empty,
					new Condition{ Operator = "WHERE", ConditionValue = conditionValue },
					string.Empty);

				return (int) entry.Connection.ExecuteScalar(cmd);
			}            
		}

		/// <summary>
		/// Gets a list of data entities containing IDs and names of retail items in a given special group. The list is ordered by retail item names, 
		/// and items numbered between recordFrom and recordTo will be returned
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="groupId">The ID of the special group</param>
		/// <param name="recordFrom">The result number of the first item to be retrieved</param>
		/// <param name="recordTo">The result number of the last item to be retrieved</param>
		/// <param name="sortBy">Defines the column to sort the result set by</param>
		/// <param name="sortedBackwards">Set to true if wanting to sort the result set backwards</param>
		/// <returns>A list of data entities containing IDs and names of retail items in a given special group meeting the above criteria</returns>
		public virtual List<ItemInGroup> ItemsInSpecialGroup(IConnectionManager entry, RecordIdentifier groupId,
			int recordFrom, int recordTo, SpecialGroupItemSorting sortBy,
			bool sortedBackwards)
		{
			ValidateSecurity(entry);

			string sort = ResolveItemSort(sortBy, sortedBackwards);


			List<TableColumn> columns = new List<TableColumn>();
			List<Condition> conditions = new List<Condition>();
			List<Condition> externalConditions = new List<Condition>();

			using (var cmd = entry.Connection.CreateCommand())
			{
				foreach (var selectionColumn in itemColumns)
				{
					columns.Add(selectionColumn);
				}
				columns.Add(new TableColumn
				{
					ColumnName = "''",
					ColumnAlias = "GROUPNAME"
				});

				columns.Add(new TableColumn { ColumnName = "VARIANTNAME", TableAlias = "A" });
				columns.Add(new TableColumn
				{
					ColumnName = string.Format("ROW_NUMBER() OVER(order by {0})", sort),
					ColumnAlias = "ROW"
				});

				externalConditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = "item.ROW between @recordFrom and @recordTo"
				});

				conditions.Add(new Condition {Operator = "AND", ConditionValue = "sgi.GROUPID = @groupID " });

				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.DELETED = 0 " });
				Join join = new Join
				{
					Condition = "A.MASTERID = sgi.MEMBERMASTERID",
					Table = "SPECIALGROUPITEMS",
					TableAlias = "sgi"
				};

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEM", "A", "item"),
					QueryPartGenerator.ExternalColumnGenerator(columns, "item"),
					QueryPartGenerator.InternalColumnGenerator(columns),
					join,
					QueryPartGenerator.ConditionGenerator(conditions),
					QueryPartGenerator.ConditionGenerator(externalConditions),
					string.Empty);


				MakeParam(cmd, "groupID", groupId);
				MakeParam(cmd, "recordFrom", recordFrom, SqlDbType.Int);
				MakeParam(cmd, "recordTo", recordTo, SqlDbType.Int);

				return Execute<ItemInGroup>(entry, cmd, CommandType.Text, PopulateItemInGroup);
			}
		}

		/// <summary>
		/// Get a list of retail items not in a selected special group, that contain a given search text.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="groupId">The ID of the special group to exclude items for</param>
		/// <param name="searchText">The text to search for. Searches in item name, the ID field and the name alias field</param>
		/// <param name="numberOfRecords">The number of items to return. Items are ordered by retail item name</param>
		/// <param name="excludedGroupId">The ID of the special group to exclude</param>
		/// <returns>A list of retail items meeting the above criteria</returns>
		public virtual
			List<ItemInGroup> ItemsNotInSpecialGroup(
			IConnectionManager entry,
			RecordIdentifier groupId,
			string searchText,
			int numberOfRecords)
		{
			ValidateSecurity(entry);
			List<TableColumn> columns = new List<TableColumn>();
			List<Join> joins = new List<Join>();
			List<Condition> conditions = new List<Condition>();

			using (var cmd = entry.Connection.CreateCommand())
			{
				foreach (var selectionColumn in itemColumns)
				{
					columns.Add(selectionColumn);
				}
				columns.Add(new TableColumn
				{
					ColumnName = "''",  // Keep the column GROUPNAME for the sake of simplicity; without this column we would need to populators
					ColumnAlias = "GROUPNAME"
				});

				columns.Add(new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "A"});
				joins.Add(new Join
				{
					Condition = "A.MASTERID = sgi.MEMBERMASTERID",
					Table = "SPECIALGROUPITEMS",
					TableAlias = "sgi",
					JoinType = "LEFT OUTER"

				});
				joins.Add(new Join
				{
					Condition = "sgi.GROUPID = sg.GROUPID",
					Table = "SPECIALGROUP",
					TableAlias = "sg",
					JoinType = "LEFT OUTER"
				});

				conditions.Add(new Condition {Operator = "AND", ConditionValue = "(a.ITEMID like @searchString or a.ITEMNAME like @searchString  or a.NAMEALIAS like @searchString) "});
				conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.DELETED = 0 "});
				conditions.Add(new Condition {Operator = "AND", ConditionValue = "A.MASTERID NOT IN (SELECT sgiFilter.MEMBERMASTERID FROM SPECIALGROUPITEMS sgiFilter WHERE sgiFilter.GROUPID = @groupId) " });

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEM", "A", numberOfRecords, true),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(joins),
					QueryPartGenerator.ConditionGenerator(conditions),
					"ORDER BY A.ITEMNAME");

				MakeParam(cmd, "searchString", searchText + "%");
				MakeParam(cmd, "groupId", (string)groupId);

				return Execute<ItemInGroup>(entry, cmd, CommandType.Text, PopulateItemInGroup);
			}
		}

		public virtual List<DataEntity> GetSpecialGroupsForItem(IConnectionManager entry, RecordIdentifier itemID)
		{
			ValidateSecurity(entry);

			List<TableColumn> columns = new List<TableColumn>();
			List<Join> joins = new List<Join>();
			List<Condition> conditions = new List<Condition>();

			using (var cmd = entry.Connection.CreateCommand())
			{
				columns.Add(new TableColumn
				{
					ColumnName = "ISNULL(GROUPID, '')",
					ColumnAlias =  "GROUPID"
				});

				if (itemID.DBType == SqlDbType.UniqueIdentifier)
				{
					conditions.Add(new Condition {Operator = "AND", ConditionValue = "MEMBERMASTERID = @ITEMID "});
					MakeParam(cmd, "ITEMID", (Guid)itemID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "ITEMID = @ITEMID " });
					MakeParam(cmd, "ITEMID", (string)itemID);
				}

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("SPECIALGROUPITEMS", "A", 0, true),
					QueryPartGenerator.InternalColumnGenerator(columns),
					QueryPartGenerator.JoinGenerator(joins),
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);

				return Execute<DataEntity>(entry, cmd, CommandType.Text, PopulateGroupList);
			}
		}

		public virtual bool ItemInSpecialGroup(IConnectionManager entry, RecordIdentifier groupId, RecordIdentifier itemId)
		{
			if (itemId.DBType == SqlDbType.UniqueIdentifier)
			{
				return RecordExists(entry, "SPECIALGROUPITEMS", new[] { "GROUPID", "MEMBERMASTERID" }, new RecordIdentifier(groupId, itemId), false);
			}
			
			return RecordExists(entry, "SPECIALGROUPITEMS", new[]{"GROUPID","ITEMID"}, new RecordIdentifier(groupId,itemId),false);
		}

		/// <summary>
		/// Searches for the given search text, and returns the results as a list of <see
		/// cref="DataEntity" />
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="searchString">The value to search for</param>
		/// <param name="rowFrom">The number of the first row to fetch</param>
		/// <param name="rowTo">The number of the last row to fetch</param>
		/// <param name="beginsWith">Specifies if the search text is the beginning of the
		/// text or if the text may contain the search text.</param>
		public virtual List<DataEntity> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith)
		{
			List<TableColumn> columns = new List<TableColumn>();
			List<Condition> conditions = new List<Condition>();
			List<Condition> externalConditions = new List<Condition>();

			using (var cmd = entry.Connection.CreateCommand())
			{
				string modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";
				foreach (var selectionColumn in listColumns)
				{
					columns.Add(selectionColumn);
				}
				columns.Add(new TableColumn
				{
					ColumnName = "ROW_NUMBER() OVER(ORDER BY SG.NAME)",
					ColumnAlias = "ROW"
				});

				conditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = " SG.NAME LIKE @searchString "
				});
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "SG.DELETED = 0 " });

				externalConditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = "S.ROW BETWEEN @rowFrom AND @rowTo"
				});

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("SPECIALGROUP", "SG", "S"),
				   QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
				   QueryPartGenerator.InternalColumnGenerator(columns),
				   string.Empty,
				   QueryPartGenerator.ConditionGenerator(conditions),
				   QueryPartGenerator.ConditionGenerator(externalConditions),
				   string.Empty);

				MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
				MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
				MakeParam(cmd, "searchString", modifiedSearchString);

				return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "GROUPID");
			}
		}

		/// <summary>
		/// Searches for the given search text, and returns the results as a list of <see
		/// cref="DataEntity" />
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="searchString">The value to search for</param>
		/// <param name="rowFrom">The number of the first row to fetch</param>
		/// <param name="rowTo">The number of the last row to fetch</param>
		/// <param name="beginsWith">Specifies if the search text is the beginning of the
		/// text or if the text may contain the search text.</param>
		public virtual List<MasterIDEntity> SearchMasterID(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith)
		{
			List<TableColumn> columns = new List<TableColumn>();
			List<Condition> conditions = new List<Condition>();
			List<Condition> externalConditions = new List<Condition>();

			using (var cmd = entry.Connection.CreateCommand())
			{
				string modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";
				columns.AddRange(masterIDListColumns);
				columns.Add(new TableColumn
				{
					ColumnName = "ROW_NUMBER() OVER(ORDER BY SG.NAME)",
					ColumnAlias = "ROW"
				});

				conditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = " SG.NAME LIKE @searchString "
				});
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "SG.DELETED = 0 " });

				externalConditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = "S.ROW BETWEEN @rowFrom AND @rowTo"
				});

				cmd.CommandText = string.Format(QueryTemplates.PagingQuery("SPECIALGROUP", "SG", "S"),
				   QueryPartGenerator.ExternalColumnGenerator(columns, "S"),
				   QueryPartGenerator.InternalColumnGenerator(columns),
				   string.Empty,
				   QueryPartGenerator.ConditionGenerator(conditions),
				   QueryPartGenerator.ConditionGenerator(externalConditions),
				   string.Empty);

				MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
				MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
				MakeParam(cmd, "searchString", modifiedSearchString);

				return Execute<MasterIDEntity>(entry, cmd, CommandType.Text, PopulateListMasterID);
			}
		}

		/// <summary>
		/// Deletes a special group by a given ID
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="specialGroupId">The ID of the group to delete</param>
		public virtual void Delete(IConnectionManager entry, RecordIdentifier specialGroupId)
		{

			ValidateSecurity(entry, Permission.ManageSpecialGroups);
			var statement = new SqlServerStatement("SPECIALGROUP");

			var masterID = specialGroupId.IsGuid ? specialGroupId : GetMasterID(entry, specialGroupId);

			statement.StatementType = StatementType.Update;

			statement.AddCondition("MASTERID",(Guid)masterID, SqlDbType.UniqueIdentifier);
			statement.AddField("DELETED", true, SqlDbType.Bit);

			entry.Connection.ExecuteStatement(statement);
		}

		public virtual Guid GetMasterID(IConnectionManager entry, RecordIdentifier ID)
		{
			return GetMasterID(entry, ID, "SPECIALGROUP", "GROUPID");
		}

		/// <summary>
		/// Removes a retail item from a special group
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemId">ID of the item to remove</param>
		/// <param name="groupId">ID of the special group</param>
		public virtual void RemoveItemFromSpecialGroup(IConnectionManager entry, RecordIdentifier itemId, RecordIdentifier groupId)
		{
			ValidateSecurity(entry, Permission.ManageSpecialGroups);
			var statement = new SqlServerStatement("SPECIALGROUPITEMS", StatementType.Delete);

			statement.AddCondition("GROUPMASTERID", GetMasterID(entry, groupId), SqlDbType.UniqueIdentifier);
			statement.AddCondition("MEMBERMASTERID", GetMasterID(entry, itemId, "RETAILITEM", "ITEMID"), SqlDbType.UniqueIdentifier);

			entry.Connection.ExecuteStatement(statement);

		}

		/// <summary>
		/// Adds a retail item to a special group
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">ID of the item to add</param>
		/// <param name="groupID">ID of the special group</param>
		public virtual void AddItemToSpecialGroup(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier groupID)
		{
			ValidateSecurity(entry, Permission.ItemsEdit);
			AddItemToGroup(entry, itemID, groupID);
		}

		/// <summary>
		/// Add multiple retail items to a special group
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemIDs">IDs of the items to add</param>
		/// <param name="groupID">ID of the special group</param>
		public virtual void AddItemsToSpecialGroup(IConnectionManager entry, List<RecordIdentifier> itemIDs, RecordIdentifier groupID)
		{
			ValidateSecurity(entry, Permission.ItemsEdit);
			foreach(RecordIdentifier itemID in itemIDs)
			{
				AddItemToGroup(entry, itemID, groupID);
			}
		}
		
		/// <summary>
		/// Checks if a special group with a given ID exists in the database
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="groupID">ID of the special group to check for</param>
		/// <returns>Whether the special group exists or not</returns>
		public virtual bool Exists(IConnectionManager entry, RecordIdentifier groupID)
		{
			return RecordExists(entry, "SPECIALGROUP", "GROUPID", groupID, false);
		}


		/// <summary>
		/// Saves the given special group to the database
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="specialGroup">Special group to save</param>
		public virtual void Save(IConnectionManager entry, SpecialGroup specialGroup)
		{
			var statement = new SqlServerStatement("SPECIALGROUP");

			ValidateSecurity(entry, Permission.ManageSpecialGroups);

			bool isNew = false;
			if (specialGroup.ID == RecordIdentifier.Empty)
			{
				isNew = true;
				specialGroup.ID = DataProviderFactory.Instance.GenerateNumber<ISpecialGroupData, DataEntity>(entry);
			}

			if (isNew || !Exists(entry, specialGroup.ID))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("MASTERID", Guid.NewGuid(), SqlDbType.UniqueIdentifier);
				statement.AddField("GROUPID", (string)specialGroup.ID);
			}
			else
			{
				statement.StatementType = StatementType.Update;
				statement.AddCondition("MASTERID", (Guid)specialGroup.MasterID, SqlDbType.UniqueIdentifier);
			}

			statement.AddField("NAME", specialGroup.Text);

			entry.Connection.ExecuteStatement(statement);
		}

		/// <summary>
		/// Saves the given special group to the database
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="specialGroup">Special group to save</param>
		public virtual void Save(IConnectionManager entry, DataEntity specialGroup)
		{
			var statement = new SqlServerStatement("SPECIALGROUP");

			ValidateSecurity(entry, Permission.ManageSpecialGroups);

			bool isNew = false;
			if (specialGroup.ID == RecordIdentifier.Empty)
			{
				isNew = true;
				specialGroup.ID = DataProviderFactory.Instance.GenerateNumber<ISpecialGroupData, DataEntity>(entry);
			}

			if (isNew || !Exists(entry, specialGroup.ID))
			{
				statement.StatementType = StatementType.Insert;
				
				statement.AddKey("MASTERID", Guid.NewGuid(), SqlDbType.UniqueIdentifier);
				statement.AddField("GROUPID", (string)specialGroup.ID);
			}
			else
			{
				statement.StatementType = StatementType.Update;
				Guid masterID = GetMasterID(entry, specialGroup.ID);
				statement.AddCondition("MASTERID", masterID, SqlDbType.UniqueIdentifier);
			}

			statement.AddField("NAME", specialGroup.Text);
			
			entry.Connection.ExecuteStatement(statement);
		}

		/// <summary>
		/// Gets a list of SpecialGroupItem entities, one for each special group. Each entity contains information about a special group, 
		/// an item with the given ID and whether the item is in the group or not.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemId">The ID of the item we want to see if is contained in the groups</param>
		/// <returns>A list of data entities that show information about all the special groups and whether the item with the given ID is contained in each group</returns>
		public virtual List<SpecialGroupItem> GetItemsGroupInformation(IConnectionManager entry, RecordIdentifier itemId)
		{
			var groups = GetList(entry);
			var groupResults = new List<SpecialGroupItem>();

			ValidateSecurity(entry);

			foreach (var group in groups)
			{
				bool itemIsInGroup = ItemInSpecialGroup(entry,group.ID, itemId);
				groupResults.Add(new SpecialGroupItem { GroupId = group.ID, GroupName = group.Text, ItemIsInGroup = itemIsInGroup });
			}

			return groupResults;
		}

		#region ISequenceable Members

		/// <summary>
		/// Returns true if information about the given class exists in the database
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="id">The unique sequence ID to search for</param>
		public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
		{
			return Exists(entry, id);
		}

		/// <summary>
		/// Returns a unique ID for the class
		/// </summary>
		public RecordIdentifier SequenceID
		{
			get { return "SPECIALGROUP"; }
		}

		public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
		{
			return GetExistingRecords(entry, "SPECIALGROUP", "GROUPID", sequenceFormat, startingRecord, numberOfRecords);
		}

		#endregion
	}
}
