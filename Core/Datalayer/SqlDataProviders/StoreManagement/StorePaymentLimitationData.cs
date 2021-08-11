using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.StoreManagement
{
    public class StorePaymentLimitationData : SqlServerDataProviderBase, IStorePaymentLimitationData
    {
        private static List<TableColumn> storePaymentLimitationColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "STOREID " , TableAlias = "P"},
            new TableColumn {ColumnName = "TENDERTYPEID " , TableAlias = "P"},
            new TableColumn {ColumnName = "LIMITATIONMASTERID " , TableAlias = "P"},
            new TableColumn {ColumnName = "ISNULL(PL.RESTRICTIONCODE, '') ", ColumnAlias = "RESTRICTIONCODE"}
        };
        
        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "PL.LIMITATIONMASTERID = P.LIMITATIONMASTERID AND P.DATAAREAID = @DATAAREAID  ",
                JoinType = "LEFT OUTER",
                Table = "PAYMENTLIMITATIONS",
                TableAlias = "PL"
            }
        };
        
        private static void PopulateLimitation(IDataReader dr, StorePaymentLimitation limitation)
        {
            limitation.ID = (Guid)dr["LIMITATIONMASTERID"];
            limitation.TenderTypeID = (string)dr["TENDERTYPEID"];
            limitation.RestrictionCode = (string)dr["RESTRICTIONCODE"];
            limitation.StoreID = (string)dr["STOREID"];
        }

        public virtual List<StorePaymentLimitation> GetList(IConnectionManager entry)
        {
            return GetListForStoreTender(entry, RecordIdentifier.Empty, RecordIdentifier.Empty);
        }

        public virtual List<StorePaymentLimitation> GetListForStoreTender(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier tenderTypeID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();
                if (storeID != RecordIdentifier.Empty)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "P.STOREID = @STOREID " });
                    MakeParam(cmd, "STOREID", storeID);
                }

                if (tenderTypeID != RecordIdentifier.Empty)
                {
                    conditions.Add(new Condition {Operator = "AND", ConditionValue = "P.TENDERTYPEID = @TENDERTYPEID "});
                    MakeParam(cmd, "TENDERTYPEID", tenderTypeID);
                }
                
                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE", "P", 0, true),
                    QueryPartGenerator.InternalColumnGenerator(storePaymentLimitationColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty);

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                return GetList<StorePaymentLimitation>(entry, cmd, new RecordIdentifier(storeID, tenderTypeID), PopulateLimitation, cacheType);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier tenderTypeID, RecordIdentifier limitationMasterID)
        {
            return RecordExists(entry, "RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE", new string[] {"STOREID", "TENDERTYPEID", "LIMITATIONMASTERID"}, 
                new RecordIdentifier(storeID, tenderTypeID, limitationMasterID));
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier tenderTypeID, RecordIdentifier limitationMasterID)
        {
            DeleteRecord(entry, "RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE", new string[] { "STOREID", "TENDERTYPEID", "LIMITATIONMASTERID" }, 
                new RecordIdentifier(storeID, tenderTypeID, limitationMasterID), new string[] { Permission.PaymentMethodsEdit, Permission.StoreEdit});
        }
        
        public virtual void Save(IConnectionManager entry, StorePaymentLimitation paymentLimitation)
        {
            SqlServerStatement statement = new SqlServerStatement("RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE");

            ValidateSecurity(entry, new string[] { Permission.PaymentMethodsEdit, Permission.StoreEdit});
            
            if (!Exists(entry, paymentLimitation.StoreID, paymentLimitation.TenderTypeID, paymentLimitation.LimitationMasterID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("STOREID", (string)paymentLimitation.StoreID);
                statement.AddKey("TENDERTYPEID", (string)paymentLimitation.TenderTypeID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("LIMITATIONMASTERID", (Guid)paymentLimitation.LimitationMasterID, SqlDbType.UniqueIdentifier);

                entry.Connection.ExecuteStatement(statement);
            }
        }
    }
}