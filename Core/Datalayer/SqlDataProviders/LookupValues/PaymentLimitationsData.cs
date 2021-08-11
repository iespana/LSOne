using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.LookupValues
{
    public class PaymentLimitationsData : SqlServerDataProviderBase, IPaymentLimitationsData
    {
        private static List<TableColumn> paymentLimitationColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "MASTERID ", TableAlias = "P"},
            new TableColumn {ColumnName = "LIMITATIONMASTERID ", TableAlias = "P"},
            new TableColumn {ColumnName = "TENDERID ", TableAlias = "P"},
            new TableColumn {ColumnName = "RESTRICTIONCODE ", TableAlias = "P"},
            new TableColumn {ColumnName = "[TYPE] ", TableAlias = "P"},
            new TableColumn {ColumnName = "RELATIONMASTERID ", TableAlias = "P"},
            new TableColumn {ColumnName = "TAXEXEMPT ", TableAlias = "P"},
            new TableColumn {ColumnName = "ISNULL(P.INCLUDE,0) ", ColumnAlias = "INCLUDE"},
            new TableColumn {ColumnName = "ISNULL(I.ITEMNAME,'') ", ColumnAlias = "ITEMNAME"},
            new TableColumn {ColumnName = "ISNULL(I.VARIANTNAME,'') ", ColumnAlias = "VARIANTNAME"},
            new TableColumn {ColumnName = "ISNULL(I.ITEMID,'') ", ColumnAlias = "ITEMID"},
            new TableColumn {ColumnName = "ISNULL(IG.NAME,'') ", ColumnAlias = "RETAILGROUPNAME"},
            new TableColumn {ColumnName = "ISNULL(IG.GROUPID,'') ", ColumnAlias = "RETAILGROUPID"},
            new TableColumn {ColumnName = "ISNULL(RD.NAME,'') ", ColumnAlias = "RETAILDEPARTMENTNAME"},
            new TableColumn {ColumnName = "ISNULL(RD.DEPARTMENTID,'') ", ColumnAlias = "RETAILDEPARTMENTID"},
            new TableColumn {ColumnName = "ISNULL(SG.NAME,'') ", ColumnAlias = "SPECIALGROUPNAME"},
            new TableColumn {ColumnName = "ISNULL(SG.GROUPID,'') ", ColumnAlias = "SPECIALGROUPID"}
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "P.RELATIONMASTERID = I.MASTERID AND P.TYPE = 0 ",
                JoinType = "LEFT OUTER",
                Table = "RETAILITEM",
                TableAlias = "I"
            },
            new Join
            {
                Condition = "P.RELATIONMASTERID = IG.MASTERID AND P.TYPE = 1 ",
                JoinType = "LEFT OUTER",
                Table = "RETAILGROUP",
                TableAlias = "IG"
            },
            new Join
            {
                Condition = "P.RELATIONMASTERID = SG.MASTERID AND P.TYPE = 2 ",
                JoinType = "LEFT OUTER",
                Table = "SPECIALGROUP",
                TableAlias = "SG"
            },
            new Join
            {
                Condition = "P.RELATIONMASTERID = RD.MASTERID AND P.TYPE = 3 ",
                JoinType = "LEFT OUTER",
                Table = "RETAILDEPARTMENT",
                TableAlias = "RD"
            }
        };

        private static void PopulateRestrictionCode(IDataReader dr, DataEntity code)
        {
            code.ID = (Guid)dr["LIMITATIONMASTERID"];
            code.Text = (string)dr["RESTRICTIONCODE"];
        }

        private static void PopulateLimitation(IDataReader dr, PaymentMethodLimitation limitation)
        {
            limitation.ID = (Guid)dr["MASTERID"];
            limitation.LimitationMasterID = (Guid)dr["LIMITATIONMASTERID"];
            limitation.TenderID = (string)dr["TENDERID"];
            limitation.RestrictionCode = (string)dr["RESTRICTIONCODE"];
            limitation.Type = (LimitationType)(int)dr["TYPE"];
            limitation.RelationMasterID = (Guid)dr["RELATIONMASTERID"];
            limitation.Include = ((int)dr["INCLUDE"]) > 0;
            limitation.TaxExempt = (bool)dr["TAXEXEMPT"];

            switch (limitation.Type)
            {
                case LimitationType.Everything:
                    limitation.Description = "";
                    break;
                case LimitationType.Item:
                    limitation.Description = (string)dr["ITEMNAME"];
                    limitation.VariantDescription = (string)dr["VARIANTNAME"];
                    limitation.RelationReadableID = (string)dr["ITEMID"];
                    break;
                case LimitationType.RetailDepartment:
                    limitation.Description = (string)dr["RETAILDEPARTMENTNAME"];
                    limitation.RelationReadableID = (string)dr["RETAILDEPARTMENTID"];
                    break;
                case LimitationType.RetailGroup:
                    limitation.Description = (string)dr["RETAILGROUPNAME"];
                    limitation.RelationReadableID = (string)dr["RETAILGROUPID"];
                    break;
                case LimitationType.SpecialGroup:
                    limitation.Description = (string)dr["SPECIALGROUPNAME"];
                    limitation.RelationReadableID = (string)dr["SPECIALGROUPID"];
                    break;
                default:
                    limitation.Description = "";
                    break;
            }
        }

        public virtual List<DataEntity> GetRestrictionCodeListForTender(IConnectionManager entry, RecordIdentifier tenderID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "DISTINCT RESTRICTIONCODE, LIMITATIONMASTERID "}
                };

                List<Condition> conditions = new List<Condition>();

                if (tenderID != RecordIdentifier.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.TENDERID = @TENDERID " });

                    MakeParam(cmd, "TENDERID", tenderID);
                }


                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("PAYMENTLIMITATIONS", "P"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    conditions.Any() ? QueryPartGenerator.ConditionGenerator(conditions) : string.Empty,
                    string.Empty);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, PopulateRestrictionCode);
            }
        }

        public virtual List<PaymentMethodLimitation> GetListForTender(IConnectionManager entry, RecordIdentifier tenderID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.TENDERID = @TENDERID " });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("PAYMENTLIMITATIONS", "P"),
                    QueryPartGenerator.InternalColumnGenerator(paymentLimitationColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY P.RESTRICTIONCODE, P.TYPE");

                MakeParam(cmd, "TENDERID", tenderID);

                return Execute<PaymentMethodLimitation>(entry, cmd, CommandType.Text, PopulateLimitation);
            }
        }

        public virtual List<PaymentMethodLimitation> GetList(IConnectionManager entry, List<string> retailItemIDs, List<string> retailGroupIDs, List<string> specialGroupIDs, List<string> retailDepartmentIDs)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                Action<List<string>, string, string> createCondition = (listOfIDs, filterColumn, alias) =>
                {
                    if (listOfIDs.Any())
                    {
                        conditions.Add(new Condition { Operator = "OR", ConditionValue = GetListCondition(listOfIDs, filterColumn, cmd, alias) });
                    }
                };

                createCondition(retailItemIDs, "ITEMID", "I");
                createCondition(retailGroupIDs, "GROUPID", "IG");
                createCondition(retailDepartmentIDs, "DEPARTMENTID", "RD");
                createCondition(specialGroupIDs, "GROUPID", "SG");

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("PAYMENTLIMITATIONS", "P"),
                    QueryPartGenerator.InternalColumnGenerator(paymentLimitationColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "");

                return Execute<PaymentMethodLimitation>(entry, cmd, CommandType.Text, PopulateLimitation);
            }
        }

        public virtual List<PaymentMethodLimitation> GetListForRestrictionCode(IConnectionManager entry, RecordIdentifier limitationMasterID, RecordIdentifier tenderID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                if (tenderID != RecordIdentifier.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.TENDERID = @TENDERID " });
                    MakeParam(cmd, "TENDERID", tenderID);
                }

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.LIMITATIONMASTERID = @LIMITATIONMASTERID " });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("PAYMENTLIMITATIONS", "P"),
                    QueryPartGenerator.InternalColumnGenerator(paymentLimitationColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY P.RESTRICTIONCODE, P.TYPE");

                MakeParam(cmd, "LIMITATIONMASTERID", (Guid)limitationMasterID, SqlDbType.UniqueIdentifier);
                
                return GetList<PaymentMethodLimitation>(entry, cmd, new RecordIdentifier("PaymentLimitations", limitationMasterID, tenderID), PopulateLimitation, cacheType);
            }
        }

        public virtual RecordIdentifier GetLimitationMasterID(IConnectionManager entry, RecordIdentifier restrictionCode)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.RESTRICTIONCODE = @RESTRICTIONCODE " });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("PAYMENTLIMITATIONS", "P"),
                    QueryPartGenerator.InternalColumnGenerator(paymentLimitationColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY P.RESTRICTIONCODE, P.TYPE");

                MakeParam(cmd, "RESTRICTIONCODE", restrictionCode);

                List<PaymentMethodLimitation> result = Execute<PaymentMethodLimitation>(entry, cmd, CommandType.Text, PopulateLimitation);

                return result.Count > 0 ? result[0].LimitationMasterID : RecordIdentifier.Empty;
            }
        }

        public virtual PaymentMethodLimitation Get(IConnectionManager entry, RecordIdentifier ID, CacheType cacheType = CacheType.CacheTypeNone)
        {            
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.MASTERID = @MASTERID " });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("PAYMENTLIMITATIONS", "P"),
                    QueryPartGenerator.InternalColumnGenerator(paymentLimitationColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY P.RESTRICTIONCODE, P.TYPE");

                MakeParam(cmd, "MASTERID", (Guid)ID, SqlDbType.UniqueIdentifier);

                List<PaymentMethodLimitation> result = Execute<PaymentMethodLimitation>(entry, cmd, CommandType.Text, PopulateLimitation);

                return Get<PaymentMethodLimitation>(entry, cmd, ID, PopulateLimitation, cacheType, UsageIntentEnum.Normal) ?? new PaymentMethodLimitation();                
            }
        }

        public virtual bool RestrictionCodeExists(IConnectionManager entry, RecordIdentifier restrictionCode)
        {
            return RecordExists(entry, "PAYMENTLIMITATIONS", "RESTRICTIONCODE", restrictionCode, false);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "PAYMENTLIMITATIONS", "MASTERID", (Guid)id, false);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier tenderID, RecordIdentifier restrictionCode, LimitationType type, RecordIdentifier relationMasterID)
        {
            return RecordExists(entry, "PAYMENTLIMITATIONS", new string[] { "TENDERID", "RESTRICTIONCODE", "TYPE", "RELATIONMASTERID" }, new RecordIdentifier(tenderID, restrictionCode, (int)type, relationMasterID), false);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "PAYMENTLIMITATIONS", "MASTERID", (Guid)id, Permission.PaymentMethodsEdit, false);
        }

        public virtual void Save(IConnectionManager entry, PaymentMethodLimitation limitation)
        {
            SqlServerStatement statement = new SqlServerStatement("PAYMENTLIMITATIONS");

            ValidateSecurity(entry, Permission.PaymentMethodsEdit);

            bool isNew = string.IsNullOrEmpty(limitation.ID.StringValue) || limitation.ID == Guid.Empty;

            if (isNew || !Exists(entry, limitation.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("MASTERID", isNew ? Guid.NewGuid() : (Guid)limitation.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("MASTERID", (Guid)limitation.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("LIMITATIONMASTERID", (Guid)limitation.LimitationMasterID, SqlDbType.UniqueIdentifier);
            statement.AddField("TENDERID", (string)limitation.TenderID);
            statement.AddField("RESTRICTIONCODE", (string)limitation.RestrictionCode);
            statement.AddField("RELATIONMASTERID", (Guid)limitation.RelationMasterID, SqlDbType.UniqueIdentifier);
            statement.AddField("TYPE", (int)limitation.Type, SqlDbType.Int);
            statement.AddField("INCLUDE", limitation.Include ? 1 : 0, SqlDbType.Int);
            statement.AddField("TAXEXEMPT", limitation.TaxExempt, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SetTaxExemptStatus(IConnectionManager entry, RecordIdentifier limitationMasterID, RecordIdentifier restrictionCode, bool taxExempt)
        {
            ValidateSecurity(entry, Permission.PaymentMethodsEdit);

            SqlServerStatement statement = new SqlServerStatement("PAYMENTLIMITATIONS");
            statement.StatementType = StatementType.Update;

            statement.AddCondition("LIMITATIONMASTERID", (Guid)limitationMasterID, SqlDbType.UniqueIdentifier);
            statement.AddCondition("RESTRICTIONCODE", (string)restrictionCode);

            statement.AddField("TAXEXEMPT", taxExempt, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        public List<PaymentMethodLimitationRestrictionCode> GetRestrictionCodesForTender(IConnectionManager entry, RecordIdentifier tenderID)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "TENDERID"},
                    new TableColumn {ColumnName = "LIMITATIONMASTERID"},
                    new TableColumn {ColumnName = "RESTRICTIONCODE"},
                    new TableColumn {ColumnName = "TAXEXEMPT"}
                };

                List<Condition> conditions = new List<Condition>()
                {
                    new Condition { ConditionValue = "TENDERID = @TENDERID " }
                };

                MakeParam(cmd, "TENDERID", (string)tenderID);
                


                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("PAYMENTLIMITATIONS", "", distinct:true),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                return Execute<PaymentMethodLimitationRestrictionCode>(entry, cmd, CommandType.Text, PopulatePaymentMethodLimitationRestrictionCode);
            }
        }

        private void PopulatePaymentMethodLimitationRestrictionCode(IDataReader dr, PaymentMethodLimitationRestrictionCode restrictionCode)
        {
            restrictionCode.ID = (string)dr["RESTRICTIONCODE"];
            restrictionCode.TenderID = (string)dr["TENDERID"];
            restrictionCode.TaxExempt = (bool)dr["TAXEXEMPT"];
            restrictionCode.LimitationMasterID = (Guid)dr["LIMITATIONMASTERID"];
        }
    }
}