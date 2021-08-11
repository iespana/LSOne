using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    public class RetailItemAssemblyComponentData : SqlServerDataProviderBase, IRetailItemAssemblyComponentData
    {
        private static List<TableColumn> AssemblyComponentColumns = new List<TableColumn>
        {
            new TableColumn { ColumnName = "ID", TableAlias = "AC" },
            new TableColumn { ColumnName = "ASSEMBLYID", TableAlias = "AC" },
            new TableColumn { ColumnName = "ITEMID", TableAlias = "AC" },
            new TableColumn { ColumnName = "ITEMID", TableAlias = "IH", ColumnAlias = "HEADERITEMID", IsNull = true, NullValue = "''" },
            new TableColumn { ColumnName = "QUANTITY", TableAlias = "AC" },
            new TableColumn { ColumnName = "UNITID", TableAlias = "AC" },
            new TableColumn { ColumnName = "TXT", TableAlias = "U", ColumnAlias = "UNITNAME" },
            new TableColumn { ColumnName = "ITEMNAME", TableAlias = "I" },
            new TableColumn { ColumnName = "VARIANTNAME", TableAlias = "I" },
            new TableColumn { ColumnName = $"{AssemblyItemQueries.AssemblyComponentCostQuery}", ColumnAlias = "COSTPRICE" },
        };

        public static List<Join> AssemblyComponentJoins = new List<Join>
        {
            new Join { JoinType = "INNER", Table = "RETAILITEMASSEMBLY", TableAlias = "A", Condition = "A.ID = AC.ASSEMBLYID" },
            new Join { JoinType = "LEFT", Table = "RETAILITEM", TableAlias = "I", Condition = "AC.ITEMID = I.ITEMID" },
            new Join { JoinType = "LEFT", Table = "RETAILITEM", TableAlias = "IH", Condition = "I.HEADERITEMID = IH.MASTERID" },
            new Join { JoinType = "LEFT", Table = "UNIT", TableAlias = "U", Condition = "AC.UNITID = U.UNITID" },
            new Join { JoinType = "OUTER", UseApply = true,
                Condition = @"(SELECT [Value] AS COSTCALCULATION FROM SYSTEMSETTINGS SS WHERE SS.GUID = '2BAB2653-C366-480E-8DC2-99107BC03D5F') CC" },
        };

        private static List<TableColumn> RetailItemLookupColumns = new List<TableColumn>
        {
            new TableColumn { ColumnName = "ITEMID", TableAlias = "I" },
            new TableColumn { ColumnName = "ITEMNAME", TableAlias = "I" },
        };

        private static List<Join> AssemblyFromComponentItemjoins = new List<Join>()
        {
            new Join {
                JoinType = "LEFT", Table = "RETAILITEMASSEMBLY", TableAlias = "A1",
                Condition = "A1.ID = AC.ASSEMBLYID AND (A1.ENABLED = 0 OR CAST(A1.STARTINGDATE AS DATE) > CAST(GETDATE() AS DATE))" },
            new Join {
                JoinType = "LEFT", Table = "RETAILITEMASSEMBLY", TableAlias = "A2",
                Condition = "A2.ID = AC.ASSEMBLYID AND A2.ENABLED = 1 AND CAST(A2.STARTINGDATE AS DATE) <= CAST(GETDATE() AS DATE)" },
            new Join {
                JoinType = "LEFT", Table = "RETAILITEMASSEMBLY", TableAlias = "A3",
                Condition = @"A3.ITEMID = A2.ITEMID AND A3.STOREID = A2.STOREID AND A3.ENABLED = 1" +
                "AND CAST(A3.STARTINGDATE AS DATE) <= CAST(GETDATE() AS DATE) AND A3.STARTINGDATE > A2.STARTINGDATE" },
        };

        private static Condition AssemblyFromComponentItemCondition = new Condition { 
            Operator = "AND", ConditionValue = "(A1.ID IS NOT NULL OR (A2.ID IS NOT NULL AND A3.ID IS NULL))" 
        };

        protected void PopulateAssemblyComponent(IDataReader dr, RetailItemAssemblyComponent assemblyComponent)
        {
            assemblyComponent.ID = (Guid)dr["ID"];
            assemblyComponent.AssemblyID = (Guid)dr["ASSEMBLYID"];
            assemblyComponent.ItemID = (string)dr["ITEMID"];
            assemblyComponent.HeaderItemID = (string)dr["HEADERITEMID"];
            assemblyComponent.ItemName = (string)dr["ITEMNAME"];
            assemblyComponent.VariantName = (string)dr["VARIANTNAME"];
            assemblyComponent.UnitID = (string)dr["UNITID"];
            assemblyComponent.UnitName = (string)dr["UNITNAME"];
            assemblyComponent.Quantity = (decimal)dr["QUANTITY"];
            assemblyComponent.CostPerUnit = (decimal)dr["COSTPRICE"];
        }

        public void Delete(IConnectionManager entry, RecordIdentifier assemblyComponentID)
        {
            DeleteRecord(entry, "RETAILITEMASSEMBLYCOMPONENTS", "ID", assemblyComponentID, "", false);
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier assemblyComponentID)
        {
            return RecordExists(entry, "RETAILITEMASSEMBLYCOMPONENTS", "ID", assemblyComponentID, false);
        }

        public RetailItemAssemblyComponent Get(IConnectionManager entry, RecordIdentifier componentID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "AC.ID = @COMPONENTID" });
                MakeParam(cmd, "COMPONENTID", (Guid)componentID, SqlDbType.UniqueIdentifier);

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMASSEMBLYCOMPONENTS", "AC"),
                    QueryPartGenerator.InternalColumnGenerator(AssemblyComponentColumns),
                    QueryPartGenerator.JoinGenerator(AssemblyComponentJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "");

                try
                {
                    var result = Execute<RetailItemAssemblyComponent>(entry, cmd, CommandType.Text, PopulateAssemblyComponent);
                    if (result == null || result.Count == 0)
                        return null;

                    return result[0];
                }
                catch
                {
                    return null;
                }
            }
        }

        public List<RetailItemAssemblyComponent> GetList(IConnectionManager entry, RecordIdentifier assemblyID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "AC.ASSEMBLYID = @ASSEMBLYID" });
                MakeParam(cmd, "ASSEMBLYID", (Guid)assemblyID, SqlDbType.UniqueIdentifier);

                // NB! Linking components to the correct assembly items in a sales transaction
                // depends on the sorting here (assembly items after non-assembly items)
                // The magic happens in RetailTransaction.ReApplyLineIDs()
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMASSEMBLYCOMPONENTS", "AC"),
                    QueryPartGenerator.InternalColumnGenerator(AssemblyComponentColumns),
                    QueryPartGenerator.JoinGenerator(AssemblyComponentJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY I.ITEMTYPE, I.ITEMID");

                try
                {
                    return Execute<RetailItemAssemblyComponent>(entry, cmd, CommandType.Text, PopulateAssemblyComponent);
                }
                catch
                {
                    return null;
                }
            }
        }

        public bool HasComponents(IConnectionManager entry, RecordIdentifier assemblyID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "AC.ASSEMBLYID = @ASSEMBLYID" });
                MakeParam(cmd, "ASSEMBLYID", (Guid)assemblyID, SqlDbType.UniqueIdentifier);

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMASSEMBLYCOMPONENTS", "AC", 1),
                    QueryPartGenerator.InternalColumnGenerator(AssemblyComponentColumns),
                    QueryPartGenerator.JoinGenerator(AssemblyComponentJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "");

                try
                {
                    return ExecuteSingleRow(entry, cmd, CommandType.Text).Count > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Save(IConnectionManager entry, RetailItemAssemblyComponent component)
        {
            var statement = new SqlServerStatement("RETAILITEMASSEMBLYCOMPONENTS");

            if(RecordIdentifier.IsEmptyOrNull(component.ID))
            {
                component.ID = Guid.NewGuid();
            }

            if(Exists(entry, component.ID))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)component.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (Guid)component.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("ASSEMBLYID", (Guid)component.AssemblyID, SqlDbType.UniqueIdentifier);
            statement.AddField("ITEMID", component.ItemID.StringValue, SqlDbType.NVarChar);
            statement.AddField("UNITID", component.UnitID.StringValue, SqlDbType.NVarChar);
            statement.AddField("QUANTITY", component.Quantity, SqlDbType.Decimal);

            entry.Connection.ExecuteStatement(statement);
        }

        public List<DataEntity> GetAssemblyItemsForComponentItem(IConnectionManager entry, RecordIdentifier componentItemId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>(){
                    AssemblyFromComponentItemCondition,
                    new Condition { Operator = "AND", ConditionValue = "AC.ITEMID = @ITEMID" },
                };

                MakeParam(cmd, "ITEMID", componentItemId);

                List<Join> joins = new List<Join>(AssemblyFromComponentItemjoins);
                joins.Add(new Join { JoinType = "LEFT", Table = "RETAILITEM", TableAlias = "I", Condition = "I.ITEMID = ISNULL(A1.ITEMID, A2.ITEMID)" });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMASSEMBLYCOMPONENTS", "AC", distinct: true),
                    QueryPartGenerator.InternalColumnGenerator(RetailItemLookupColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "");

                try
                {
                    return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
                }
                catch
                {
                    return new List<DataEntity>();
                }
            }
        }

        public List<DataEntity> WhichItemsAreComponentsOfAssemblies(IConnectionManager entry, List<RecordIdentifier> componentItemIds)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                    {
                        new TableColumn { ColumnName = "ITEMID", TableAlias = "I" },
                        new TableColumn { ColumnName = "ITEMNAME", TableAlias = "I" },
                    };

                List<string> orConditions = new List<string>();
                for (int n = 0; n < componentItemIds.Count; n++)
                {
                    orConditions.Add(String.Format("AC.ITEMID = @ITEMID{0}", n));
                    MakeParam(cmd, String.Format("ITEMID{0}", n), componentItemIds[n]);
                }

                List<Condition> conditions = new List<Condition>()
                {
                    AssemblyFromComponentItemCondition,
                    new Condition { Operator = "AND", ConditionValue = String.Format("({0})", String.Join(" OR ", orConditions)) },
                };

                List<Join> joins = new List<Join>(AssemblyFromComponentItemjoins);
                joins.Add(new Join { JoinType = "LEFT", Table = "RETAILITEM", TableAlias = "I", Condition = "I.ITEMID = AC.ITEMID" });

                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMASSEMBLYCOMPONENTS", "AC", distinct: true),
                    QueryPartGenerator.InternalColumnGenerator(RetailItemLookupColumns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "");

                try
                {
                    return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
                }
                catch
                {
                    return new List<DataEntity>();
                }
            }
        }

        public bool AssemblyItemCausesCircularReference(IConnectionManager entry, RecordIdentifier assemblyID, RecordIdentifier itemID, RecordIdentifier itemIDToCheck)
        {
            if(itemID == itemIDToCheck)
            {
                return true;
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = AssemblyItemQueries.AssemblyCircularReferenceQuery;

                MakeParam(cmd, "ITEMID", (string)itemID);
                MakeParam(cmd, "ITEMIDTOCHECK", (string)itemIDToCheck);
                MakeParam(cmd, "ASSEMBLYID", Guid.Parse((string)assemblyID), SqlDbType.UniqueIdentifier);

                Dictionary<string, object> result = ExecuteSingleRow(entry, cmd, CommandType.Text);

                return (bool)result.First().Value;
            }
        }
    }
}
