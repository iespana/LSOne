using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.SqlDataProviders.LookupValues
{
    public class PaymentMethodData : SqlServerDataProviderBase, IPaymentMethodData
    {
        /// <summary>
        /// Returns the corresponding default function intended for the given posoperation
        /// </summary>
        /// <param name="id">The ID of the POSOperation</param>
        /// <returns></returns>
        public virtual PaymentMethodDefaultFunctionEnum GetDefaultFunctionFromPOSOperation(RecordIdentifier id)
        {
            var operation = (POSOperations)(Convert.ToInt32(id.StringValue));

            switch (operation)
            {
                case POSOperations.PayCash:
                    return PaymentMethodDefaultFunctionEnum.Normal;

                case POSOperations.PayCustomerAccount:
                    return PaymentMethodDefaultFunctionEnum.Customer;

                case POSOperations.PayCurrency:
                    return PaymentMethodDefaultFunctionEnum.Normal;

                case POSOperations.PayCheque:
                    return PaymentMethodDefaultFunctionEnum.Check;

                case POSOperations.PayCashQuick:
                    return PaymentMethodDefaultFunctionEnum.Normal;

                case POSOperations.PayCorporateCard:
                    return PaymentMethodDefaultFunctionEnum.Card;

                case POSOperations.PayCreditMemo:
                    return PaymentMethodDefaultFunctionEnum.Normal;

                case POSOperations.PayGiftCertificate:
                    return PaymentMethodDefaultFunctionEnum.Card;

                case POSOperations.IssueCreditMemo:
                    return PaymentMethodDefaultFunctionEnum.Normal;

                case POSOperations.IssueGiftCertificate:
                    return PaymentMethodDefaultFunctionEnum.Card;

                default:
                    return PaymentMethodDefaultFunctionEnum.Normal;
            }
        }

        public virtual List<PaymentMethod> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT TENDERTYPEID, ISNULL(NAME,'') AS NAME,
                                  ISNULL(DEFAULTFUNCTION, '') AS DEFAULTFUNCTION, ISLOCALCURRENCY                                 
                                  FROM RBOTENDERTYPETABLE WHERE DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<PaymentMethod>(entry, cmd, CommandType.Text, PopulatePaymentType);
            }            
        }

        private static void PopulatePaymentType(IDataReader dr, PaymentMethod paymentMethod)
        {
            paymentMethod.ID = (string)dr["TENDERTYPEID"];
            paymentMethod.Text = (string)dr["NAME"];
            paymentMethod.DefaultFunction = (PaymentMethodDefaultFunctionEnum)((int)dr["DEFAULTFUNCTION"]);            
            paymentMethod.IsLocalCurrency = (bool)dr["ISLOCALCURRENCY"];
        }

        public virtual PaymentMethod Get(IConnectionManager entry, RecordIdentifier paymentMethodID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT TENDERTYPEID, ISNULL(NAME,'') AS NAME,
                                  ISNULL(DEFAULTFUNCTION, '') AS DEFAULTFUNCTION, ISLOCALCURRENCY 
                                  FROM RBOTENDERTYPETABLE WHERE DATAAREAID = @DATAAREAID AND TENDERTYPEID=@TENDERID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "TENDERID", (string)paymentMethodID);

                var result = Execute<PaymentMethod>(entry, cmd, CommandType.Text, PopulatePaymentType);

                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual RecordIdentifier GetAddRemoveFloatTenderID(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT T.TENDERTYPEID FROM RBOTENDERTYPETABLE T 
                                    LEFT OUTER JOIN RBOSTORETENDERTYPETABLE S
                                    ON T.TENDERTYPEID = S.TENDERTYPEID AND T.DATAAREAID = S.DATAAREAID
                                    WHERE T.DEFAULTFUNCTION = 4 AND S.STOREID = @STOREID AND 
                                    T.DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", storeID);
                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual List<PaymentMethod> GetListForFunction(IConnectionManager entry, RecordIdentifier storeID, PaymentMethodDefaultFunctionEnum function)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT T.TENDERTYPEID, ISNULL(T.NAME, '') AS NAME, ISNULL(T.DEFAULTFUNCTION, '') AS DEFAULTFUNCTION, ISLOCALCURRENCY FROM RBOTENDERTYPETABLE T
                                    JOIN RBOSTORETENDERTYPETABLE S ON T.TENDERTYPEID = S.TENDERTYPEID AND T.DATAAREAID = S.DATAAREAID
                                    WHERE T.DATAAREAID = @DATAAREAID AND S.STOREID = @STOREID AND T.DEFAULTFUNCTION = @FUNCTION";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", storeID);
                MakeParam(cmd, "FUNCTION", function, SqlDbType.Int);
                return Execute<PaymentMethod>(entry, cmd, CommandType.Text, PopulatePaymentType);
            }
        }

        public virtual int GetPaymentMethodCount(IConnectionManager entry)
        {
            ValidateSecurity(entry);
            return Count(entry, "RBOTENDERTYPETABLE");
        }

        public virtual bool InUse(IConnectionManager entry, RecordIdentifier tenderTypeID)
        {
            return RecordExists(entry, "RBOSTORETENDERTYPETABLE", "TENDERTYPEID", tenderTypeID);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier paymentMethodID)
        {
            return RecordExists(entry, "RBOTENDERTYPETABLE", "TENDERTYPEID", paymentMethodID);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBOTENDERTYPETABLE", "TENDERTYPEID", id, Permission.PaymentMethodsEdit);
            DeleteRecord(entry, "PAYMENTLIMITATIONS", "TENDERID", id, Permission.PaymentMethodsEdit, false);
        }

        public virtual void Save(IConnectionManager entry, PaymentMethod paymentMethod)
        {
            var statement = new SqlServerStatement("RBOTENDERTYPETABLE");

            ValidateSecurity(entry, Permission.CardTypesEdit);

            bool isNew = false;
            if (paymentMethod.ID.IsEmpty)
            {
                isNew = true;
                paymentMethod.ID = DataProviderFactory.Instance.GenerateNumber<IPaymentMethodData, PaymentMethod>(entry);
            }

            if (isNew || !Exists(entry, paymentMethod.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("TENDERTYPEID", (string)paymentMethod.ID);

            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("TENDERTYPEID", (string)paymentMethod.ID);
            }

            statement.AddField("NAME", paymentMethod.Text);
            statement.AddField("DEFAULTFUNCTION", (int)paymentMethod.DefaultFunction, SqlDbType.Int);
            statement.AddField("ISLOCALCURRENCY", paymentMethod.IsLocalCurrency, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SetAsLocalCurrency(IConnectionManager entry, RecordIdentifier paymentTypeID)
        {
            ValidateSecurity(entry, Permission.PaymentMethodsEdit);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"UPDATE RBOTENDERTYPETABLE SET ISLOCALCURRENCY = 0 WHERE DATAAREAID = @DATAAREAID AND ISLOCALCURRENCY = 1
                                    UPDATE RBOTENDERTYPETABLE SET ISLOCALCURRENCY = 1 WHERE TENDERTYPEID = @PAYMENTTYPEID AND DATAAREAID = @DATAAREAID
                
                                    DELETE FROM PAYMENTLIMITATIONS WHERE TENDERID = @PAYMENTTYPEID
                                    DELETE FROM RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE WHERE TENDERTYPEID = @PAYMENTTYPEID AND DATAAREAID = @DATAAREAID";
                
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "PAYMENTTYPEID", paymentTypeID);
                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "TENDERTYPE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOTENDERTYPETABLE", "TENDERTYPEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
