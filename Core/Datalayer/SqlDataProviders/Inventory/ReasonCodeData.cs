using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.DataLayer.DataProviders;
using System.Globalization;
using LSOne.DataLayer.GenericConnector.Enums;
using System.Linq;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    public class ReasonCodeData : SqlServerDataProviderBase, IReasonCodeData
    {
        private static List<TableColumn> reasonDataColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "REASONID " , ColumnAlias = "REASONID", TableAlias = "r"},
            new TableColumn {ColumnName = "REASONTEXT " , ColumnAlias = "REASONTEXT", TableAlias = "r"},
            new TableColumn {ColumnName = "ACTION " , ColumnAlias = "ACTION", TableAlias = "r"},
            new TableColumn {ColumnName = "BEGINDATE " , ColumnAlias = "BEGINDATE", TableAlias = "r"},
            new TableColumn {ColumnName = "ENDDATE " , ColumnAlias = "ENDDATE", TableAlias = "r"},
            new TableColumn {ColumnName = "ISSYSTEMREASONCODE " , ColumnAlias = "ISSYSTEMREASONCODE", TableAlias = "r"},
            new TableColumn {ColumnName = "SHOWONPOS " , ColumnAlias = "SHOWONPOS", TableAlias = "r"}
        };

        private static string ResolveSort(ReasonCodeSorting sort, bool backwards)
        {
            string direction = backwards ? " DESC" : " ASC";

            switch (sort)
            {
                case ReasonCodeSorting.ReasonId:
                    return "ORDER BY REASONID" + direction;
                case ReasonCodeSorting.ReasonText:
                    return "ORDER BY REASONTEXT" + direction;
                case ReasonCodeSorting.Action:
                    return "ORDER BY ACTION" + direction;
                case ReasonCodeSorting.BeginDate:
                    return "ORDER BY BEGINDATE" + direction;
                case ReasonCodeSorting.EndDate:
                    return "ORDER BY ENDDATE" + direction;
                case ReasonCodeSorting.SystemReason:
                    return "ORDER BY ISSYSTEMREASONCODE" + direction;
                case ReasonCodeSorting.ShowOnPos:
                    return "ORDER BY SHOWONPOS" + direction;
            }

            return "";
        }

        private static void PopulateReasonInfoMinimal(IDataReader dr, ReasonCode inventoryTransactionReasonInfo)
        {
            inventoryTransactionReasonInfo.ID = (string)dr["REASONID"];
            inventoryTransactionReasonInfo.Text = (string)dr["REASONTEXT"];
        }

        private static void PopulateReasonInfo(IDataReader dr, ReasonCode inventoryTransactionReasonInfo)
        {
            PopulateReasonInfoMinimal(dr, inventoryTransactionReasonInfo);

            inventoryTransactionReasonInfo.Action = (ReasonActionEnum)Enum.Parse(typeof(ReasonActionEnum), ((int)dr["ACTION"]).ToString());
            inventoryTransactionReasonInfo.BeginDate = (DateTime)dr["BEGINDATE"];
            inventoryTransactionReasonInfo.EndDate = (dr["ENDDATE"] == DBNull.Value) ? null : (DateTime?)dr["ENDDATE"];
            inventoryTransactionReasonInfo.IsSystemReasonCode = AsBool(dr["ISSYSTEMREASONCODE"]);
            inventoryTransactionReasonInfo.ShowOnPos = AsBool(dr["SHOWONPOS"]);
        }

        public RecordIdentifier SequenceID
        {
            get
            {
                return "Reasons";
            }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "INVENTTRANSREASON", "REASONID", sequenceFormat, startingRecord, numberOfRecords);
        }

        /// <summary>
        /// Checks if a REASON row with a given REASONID exists in the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the REASON row to check for</param>
        /// <returns>Whether a REASON row with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "INVENTTRANSREASON", "REASONID", id);
        }

        public virtual ReasonCode GetReasonById(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                Condition condition = new Condition
                {
                    ConditionValue = "r.DATAAREAID = @DATAAREAID AND r.REASONID = @REASONID",
                    Operator = "AND"
                };

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("INVENTTRANSREASON", "r"),
                  QueryPartGenerator.InternalColumnGenerator(reasonDataColumns),
                  string.Empty,
                  QueryPartGenerator.ConditionGenerator(condition),
                  string.Empty
                  );

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "REASONID", (string)id);

                return Get<ReasonCode>(entry, cmd, id, PopulateReasonInfo, CacheType.CacheTypeNone, UsageIntentEnum.Normal);
            }
        }

        public virtual List<ReasonCode> GetReasonCodesForReturn(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition
                {
                    ConditionValue = "r.BEGINDATE <= @NOW AND (r.ENDDATE IS NULL OR r.ENDDATE >= @NOW)",
                    Operator = "AND"
                });

                conditions.Add(new Condition
                {
                    ConditionValue = "r.SHOWONPOS = 1",
                    Operator = "AND"
                });

                conditions.Add(new Condition
                {
                    ConditionValue = $"r.ACTION IN ({(int)ReasonActionEnum.MainInventory}, {(int)ReasonActionEnum.ParkedInventory})",
                    Operator = "AND"
                });

                conditions.Add(new Condition
                {
                    ConditionValue = "r.DATAAREAID = @DATAAREAID",
                    Operator = "AND"
                });

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("INVENTTRANSREASON", "r"),
                  QueryPartGenerator.InternalColumnGenerator(reasonDataColumns),
                  string.Empty,
                  QueryPartGenerator.ConditionGenerator(conditions),
                  "ORDER BY r.REASONTEXT ASC"
                  );

                MakeParam(cmd, "NOW", DateTime.Now.Date, SqlDbType.DateTime);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<ReasonCode>(entry, cmd, CommandType.Text, PopulateReasonInfo);
            }
        }

        public virtual List<ReasonCode> GetList(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                Condition condition = new Condition
                {
                    ConditionValue = "r.DATAAREAID = @DATAAREAID",
                    Operator = "AND"
                };

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("INVENTTRANSREASON", "r"),
                  QueryPartGenerator.InternalColumnGenerator(reasonDataColumns),
                  string.Empty,
                  QueryPartGenerator.ConditionGenerator(condition),
                  string.Empty
                  );

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<ReasonCode>(entry, cmd, CommandType.Text, PopulateReasonInfo);
            }
        }

        /// <summary>
        /// Returns a list of reason codes filtered by the given parameters and sorted by description.
        /// </summary>
        /// <param name="entry">The entry into the database></param>
        /// <param name="actions">Filtering criteria - list of reason actions. If null, it returns all reason codes, disregarding their action.</param>
        /// <param name="forPOS">Filtering criteria - if true returns only reason codes with SHOWONPOS = 1</param>
        /// <param name="open">Filtering criteria - if true returns only open reason codes</param>
        /// <returns>List of <see cref="ReasonCode"/></returns>
        public virtual List<ReasonCode> GetList(IConnectionManager entry, List<ReasonActionEnum> actions, bool? forPOS = null, bool open = true)
        {

            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                if (open)
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = "r.BEGINDATE <= @NOW AND (r.ENDDATE IS NULL OR r.ENDDATE >= @NOW)",
                        Operator = "AND"
                    });
                    MakeParam(cmd, "NOW", DateTime.Now.Date, SqlDbType.DateTime);
                }

                if (forPOS.HasValue)
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = forPOS.Value == true ? "r.SHOWONPOS = 1" : "r.SHOWONPOS = 0",
                        Operator = "AND"
                    });
                }

                if (actions != null && actions.Count > 0)
                {
                    string inValues = String.Join(",", actions.Select(a => ((int)a).ToString()));

                    conditions.Add(new Condition
                    {
                        ConditionValue = $"r.ACTION IN ({inValues})",
                        Operator = "AND"
                    });
                }

                conditions.Add(new Condition
                {
                    ConditionValue = "r.DATAAREAID = @DATAAREAID",
                    Operator = "AND"
                });

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("INVENTTRANSREASON", "r"),
                  QueryPartGenerator.InternalColumnGenerator(reasonDataColumns),
                  string.Empty,
                  QueryPartGenerator.ConditionGenerator(conditions),
                  "ORDER BY r.REASONTEXT ASC"
                  );

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<ReasonCode>(entry, cmd, CommandType.Text, PopulateReasonInfo);
            }
        }

        public virtual List<ReasonCode> GetOpenReasonCodesList(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                Condition condition = new Condition
                {
                    ConditionValue = "r.DATAAREAID = @DATAAREAID AND " + $"r.BEGINDATE <= @NOW AND " + $"(r.ENDDATE IS NULL OR r.ENDDATE >= @NOW)",
                    Operator = "AND"
                };

                MakeParam(cmd, "NOW", DateTime.Now.Date, SqlDbType.DateTime);

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("INVENTTRANSREASON", "r"),
                  QueryPartGenerator.InternalColumnGenerator(reasonDataColumns),
                  string.Empty,
                  QueryPartGenerator.ConditionGenerator(condition),
                  string.Empty
                  );

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<ReasonCode>(entry, cmd, CommandType.Text, PopulateReasonInfo);
            }
        }

        /// <summary>
        /// Deletes a REASON row with a given ID
        /// </summary>
        /// <remarks>Requires the 'ManageOfflineInventory' permission</remarks>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the REASON row to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "INVENTTRANSREASON", "REASONID", id, Permission.ManageParkedInventory);
        }

        /// <summary>
        /// Search for reason codes
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="description">Description to search for</param>
        /// <param name="descriptionBeginsWith">True if the description should begin with the description parameter</param>
        /// <param name="action">Reason action for search for</param>
        /// <param name="beginDate">The start date of the reason</param>
        /// <param name="endDate">The end date of the reason</param>
        /// <param name="isSystemCode">True if the reasons should be system codes</param>
        /// <param name="sortBy">Sort enum to sort by</param>
        /// <param name="sortedBackwards">True if the reasons should be sorted backwards</param>
        /// <returns>A filtered list of reason codes</returns>
        public virtual List<ReasonCode> SearchReasonList(IConnectionManager entry,
                                                         string description,
                                                         bool descriptionBeginsWith,
                                                         ReasonActionEnum? action,
                                                         DateTime? beginDate,
                                                         DateTime? endDate,
                                                         bool? isSystemCode,
                                                         ReasonCodeSorting sortBy,
                                                         bool sortedBackwards)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                if (!string.IsNullOrEmpty(description))
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = "r.REASONTEXT LIKE @DESCRIPTION",
                        Operator = "AND"
                    });

                    MakeParam(cmd, "DESCRIPTION", PreProcessSearchText(description, true, descriptionBeginsWith));
                }

                if (action != null)
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = $"r.ACTION = {(int)action}",
                        Operator = "AND"
                    });
                }

                if (isSystemCode != null)
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = $"r.ISSYSTEMREASONCODE = {(isSystemCode.Value ? 1 : 0)}",
                        Operator = "AND"
                    });
                }

                if(beginDate == null && endDate == null)
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = $"r.BEGINDATE <= @now AND (r.ENDDATE IS NULL OR r.ENDDATE >= @NOW)",
                        Operator = "AND"
                    });

                    MakeParam(cmd, "NOW", DateTime.Now.Date, SqlDbType.DateTime);
                }
                else
                {
                    if(beginDate != null && endDate != null)
                    {
                        conditions.Add(new Condition
                        {
                            ConditionValue = $"((r.BEGINDATE >= @BEGINDATE OR r.ENDDATE IS NULL) AND ((r.ENDDATE IS NOT NULL AND r.ENDDATE <= @ENDDATE) OR r.BEGINDATE <= @ENDDATE))",
                            Operator = "AND"
                        });

                        MakeParam(cmd, "BEGINDATE", beginDate.Value.Date, SqlDbType.DateTime);
                        MakeParam(cmd, "ENDDATE", endDate.Value.Date, SqlDbType.DateTime);
                    }
                    else
                    {
                        if(beginDate != null)
                        {
                            conditions.Add(new Condition
                            {
                                ConditionValue = $"(r.BEGINDATE >= @BEGINDATE OR r.ENDDATE IS NULL)",
                                Operator = "AND"
                            });

                            MakeParam(cmd, "BEGINDATE", beginDate.Value.Date, SqlDbType.DateTime);
                        }

                        if(endDate != null)
                        {
                            conditions.Add(new Condition
                            {
                                ConditionValue = $"((r.ENDDATE IS NOT NULL AND r.ENDDATE <= @ENDDATE) OR (r.ENDDATE IS NULL AND r.BEGINDATE <= @ENDDATE))",
                                Operator = "AND"
                            });

                            MakeParam(cmd, "ENDDATE", endDate.Value.Date, SqlDbType.DateTime);
                        }
                    }
                }

                conditions.Add(new Condition
                {
                    ConditionValue = "r.DATAAREAID = @DATAAREAID",
                    Operator = "AND"
                });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("INVENTTRANSREASON", "r"),
                    QueryPartGenerator.InternalColumnGenerator(reasonDataColumns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(sortBy, sortedBackwards)
                    );

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<ReasonCode>(entry, cmd, CommandType.Text, PopulateReasonInfo);
            }
        }

        /// <summary>
        /// Saves a given Inventory Transaction Reason to the database
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="inventoryTransactionReason">An InventoryTransactionReason</param>
        public virtual void Save(IConnectionManager entry, ReasonCode inventoryTransactionReason)
        {
            var statement = new SqlServerStatement("INVENTTRANSREASON");

            ValidateSecurity(entry, Permission.ManageParkedInventory);

            bool isNew = false;

            if (inventoryTransactionReason.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                inventoryTransactionReason.ID = DataProviderFactory.Instance.GenerateNumber<IReasonCodeData, ReasonCode>(entry);
            }

            if (isNew || !Exists(entry, inventoryTransactionReason.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("REASONID", (string)inventoryTransactionReason.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("REASONID", (string)inventoryTransactionReason.ID);
            }

            statement.AddField("REASONTEXT", inventoryTransactionReason.Text);
            statement.AddField("ACTION", (int)inventoryTransactionReason.Action, SqlDbType.Int);
            statement.AddField("SHOWONPOS", inventoryTransactionReason.ShowOnPos, SqlDbType.Bit);
            statement.AddField("ISSYSTEMREASONCODE", inventoryTransactionReason.IsSystemReasonCode, SqlDbType.Bit);
            statement.AddField("BEGINDATE", inventoryTransactionReason.BeginDate, SqlDbType.DateTime);

            if (inventoryTransactionReason.EndDate.HasValue)
            {
                statement.AddField("ENDDATE", inventoryTransactionReason.EndDate.Value.Date, SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("ENDDATE", DBNull.Value, SqlDbType.DateTime);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// Returns true if the reason with the given id is in use
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="reasonID">ID of the reason to check for usage</param>
        /// <returns>True if the reason is used somewhere else false</returns>
        public virtual bool ReasonCodeIsInUse(IConnectionManager entry, RecordIdentifier reasonID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT TOP 1 REASONREFRECID FROM INVENTJOURNALTRANS WHERE REASONREFRECID = @REASONID and DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "REASONID", (string)reasonID);

                var items = Execute<DataEntity>(entry, cmd, CommandType.Text, "REASONREFRECID",  "REASONREFRECID");

                return (items.Count > 0);
            }
        }
    }
}
