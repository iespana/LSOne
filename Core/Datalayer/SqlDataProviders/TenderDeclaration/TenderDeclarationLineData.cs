using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders.TenderDeclaration;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TenderDeclaration
{
    public class TenderDeclarationLineData : SqlServerDataProviderBase, ITenderDeclarationLineData
    {
        private static void PopulateTenderDeclarationLine(IDataReader dr, TenderdeclarationLine tdl)
        {
            //tdl.ID = new RecordIdentifier((DateTime)dr["COUNTEDDATETIME"], new RecordIdentifier((string)dr["PaymentID"], new RecordIdentifier((string)dr["CurrencyCode"])));
            tdl.CountedDateTime = (DateTime)dr["COUNTEDDATETIME"];
            tdl.PaymentTypeID = (string)dr["PaymentID"];
            tdl.Quantity = (int)dr["Quantity"];
            tdl.Denominator.Amount = (decimal)dr["DENOMINATORAMOUNT"];
            tdl.Denominator.CurrencyCode = (string)dr["CurrencyCode"];
            tdl.Denominator.CashType = (CashDenominator.Type)(int)dr["CASHTYPE"];
            tdl.IsLocalCurrency = Convert.ToBoolean(dr["IsLocalCurrency"]);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "SCTENDERDECLARATIONLINES", new[] { "COUNTEDDATETIME", "PAYMENTID", "CURRENCYCODE", "DENOMINATORAMOUNT" }, id);
        }

        public virtual void Save(IConnectionManager entry, TenderdeclarationLine tenderDeclarationLine)
        {
            var statement = new SqlServerStatement("SCTENDERDECLARATIONLINES");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageUnits);

            if (!Exists(entry, tenderDeclarationLine.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("COUNTEDDATETIME", (DateTime)tenderDeclarationLine.CountedDateTime, SqlDbType.DateTime);
                statement.AddKey("PAYMENTID", (string)tenderDeclarationLine.PaymentTypeID);
                statement.AddKey("CURRENCYCODE", tenderDeclarationLine.Denominator.CurrencyCode);
                statement.AddKey("DENOMINATORAMOUNT", tenderDeclarationLine.Denominator.Amount, SqlDbType.Decimal);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("COUNTEDDATETIME", (DateTime)tenderDeclarationLine.CountedDateTime, SqlDbType.DateTime);
                statement.AddCondition("PAYMENTID", (string)tenderDeclarationLine.PaymentTypeID);
                statement.AddCondition("CURRENCYCODE", tenderDeclarationLine.Denominator.CurrencyCode);
                statement.AddCondition("DENOMINATORAMOUNT", tenderDeclarationLine.Denominator.Amount, SqlDbType.Decimal);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("QUANTITY", tenderDeclarationLine.Quantity, SqlDbType.Int);
            statement.AddField("CASHTYPE", tenderDeclarationLine.Denominator.CashType, SqlDbType.Int);
            statement.AddField("ISLOCALCURRENCY", tenderDeclarationLine.IsLocalCurrency, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);

            //Statement statement = new Statement("SCTENDERDECLARATIONS");
            //SqlTransaction dbTrans = entry.Connection.NativeConnection.BeginTransaction(IsolationLevel.Serializable, "saveTD");
            //try
            //{
            //    DeleteRecord(entry, "SCTENDERDECLARATIONS", "COUNTEDDATETIME", new RecordIdentifier(tenderDeclarationLine.CountedTime), BusinessObjects.Permission.CustomerView, dbTrans);

            //    statement.StatementType = StatementType.Insert;
            //    statement.AddKey("COUNTEDDATETIME", tenderDeclarationLine.CountedTime, SqlDbType.DateTime);
            //    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            //    statement.AddField("STOREID", tenderDeclarationLine.StoreID);
            //    statement.AddField("TERMINALID", tenderDeclarationLine.TerminalID);
            //    statement.AddField("STAFFID", (Guid)entry.CurrentUser.ID, SqlDbType.UniqueIdentifier);
            //    entry.Connection.ExecuteStatement(statement, dbTrans);

            //    DeleteRecord(entry, "SCTENDERDECLARATIONLINES", "COUNTEDDATETIME", new RecordIdentifier(tenderDeclarationLine.CountedTime), BusinessObjects.Permission.CustomerView, dbTrans);
            //    foreach (TenderdeclarationLine cd in tenderDeclarationLine.TenderDeclarationLines)
            //    {
            //        statement = new Statement("SCTENDERDECLARATIONLINES");

            //        statement.StatementType = StatementType.Insert;
            //        statement.AddKey("COUNTEDDATETIME", tenderDeclarationLine.CountedTime, SqlDbType.DateTime);
            //        statement.AddKey("PAYMENTID", cd.PaymentTypeID);
            //        statement.AddKey("CURRENCYCODE", cd.Denominator.CurrencyCode);
            //        statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            //        statement.AddField("QUANTITY", cd.Quantity, SqlDbType.Int);
            //        statement.AddField("CASHTYPE", cd.Denominator.CashType, SqlDbType.Int);
            //        statement.AddField("DENOMINATORAMOUNT", cd.Denominator.Amount, SqlDbType.Decimal);
            //        statement.AddField("ISLOCALCURRENCY", cd.IsLocalCurrency, SqlDbType.Int);
            //        entry.Connection.ExecuteStatement(statement, dbTrans);
            //    }
            //    dbTrans.Commit();
            //}
            //catch (Exception e)
            //{
            //    dbTrans.Rollback();
            //    throw new Exception("Error caught: " + e.Message);
            //}
            //}
            //else
            //{
            //    statement.StatementType = StatementType.Update;
            //    statement.AddField("STOREID", tenderdeclaration.StoreID);
            //    statement.AddField("TERMINALID", tenderdeclaration.TerminalID);
            //    statement.AddField("STAFFID", (Guid)entry.CurrentUser.ID, SqlDbType.UniqueIdentifier);
            //    statement.AddCondition("COUNTEDDATETIME", tenderdeclaration.DateTime, SqlDbType.DateTime);
            //    statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            //    entry.Connection.ExecuteStatement(statement);
            //    foreach (TenderdeclarationLine cd in tenderdeclaration.TenderDeclarationLines)
            //    {
            //        statement = new Statement("SCTENDERDECLARATIONLINES");

            //        if (TenderDeclarationLineExists(entry, new RecordIdentifier(tenderdeclaration.DateTime, new RecordIdentifier(cd.PaymentType, new RecordIdentifier(cd.Denominator.CurrencyCode, new RecordIdentifier(cd.Denominator.Amount))))) == true)
            //        {
            //            statement.StatementType = StatementType.Update;
            //            statement.AddField("COUNTEDDATETIME", tenderdeclaration.DateTime, SqlDbType.DateTime);
            //            statement.AddField("PAYMENTID", cd.PaymentType);
            //            statement.AddField("CURRENCYCODE", cd.Denominator.CurrencyCode);

            //            statement.AddCondition("QUANTITY", cd.Quantity, SqlDbType.Int);
            //            statement.AddCondition("CASHTYPE", cd.Denominator.CashType, SqlDbType.Int);
            //            statement.AddCondition("DENOMINATORAMOUNT", cd.Denominator.Amount, SqlDbType.Decimal);
            //            statement.AddCondition("ISLOCALCURRENCY", cd.IsLocalCurrency, SqlDbType.Int);
            //            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            //        }
            //        else
            //        {
            //            statement.StatementType = StatementType.Insert;
            //            statement.AddKey("COUNTEDDATETIME", tenderdeclaration.DateTime, SqlDbType.DateTime);
            //            statement.AddKey("PAYMENTID", cd.PaymentType);
            //            statement.AddKey("CURRENCYCODE", cd.Denominator.CurrencyCode);
            //            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

            //            statement.AddField("QUANTITY", cd.Quantity, SqlDbType.Int);
            //            statement.AddField("CASHTYPE", cd.Denominator.CashType, SqlDbType.Int);
            //            statement.AddField("DENOMINATORAMOUNT", cd.Denominator.Amount, SqlDbType.Decimal);
            //            statement.AddField("ISLOCALCURRENCY", cd.IsLocalCurrency, SqlDbType.Int);
            //        }
            //        entry.Connection.ExecuteStatement(statement);
            //    }
            //}            
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "SCTENDERDECLARATIONLINES", new[] { "COUNTEDDATETIME", "PAYMENTID", "CURRENCYCODE", "DENOMINATORAMOUNT" }, id, BusinessObjects.Permission.ManageRetailGroups);
        }

        public virtual List<TenderdeclarationLine> GetTenderDeclarationLines(IConnectionManager entry, RecordIdentifier tenderDeclarationID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNTEDDATETIME, PAYMENTID, CURRENCYCODE, DENOMINATORAMOUNT, DATAAREAID, ";
                cmd.CommandText += " ISNULL(ISLOCALCURRENCY, 0) AS ISLOCALCURRENCY, ISNULL(QUANTITY, 0) AS QUANTITY, ISNULL(CASHTYPE, 0) AS CASHTYPE FROM SCTENDERDECLARATIONLINES ";
                cmd.CommandText += " WHERE COUNTEDDATETIME = @countedDateTime";
                cmd.CommandText += " and DATAAREAID = @DataAreaID";
                cmd.CommandText += " order by COUNTEDDATETIME desc";

                MakeParam(cmd, "countedDateTime", (DateTime)tenderDeclarationID, SqlDbType.DateTime);
                MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);
                return Execute<TenderdeclarationLine>(entry, cmd, CommandType.Text, PopulateTenderDeclarationLine);
            }
        }

        //public Tenderdeclaration Get(IConnectionManager entry, RecordIdentifier id)
        //{
        //    var cmd = entry.Connection.CreateCommand();

        //    cmd.CommandText = "SELECT * FROM SCTENDERDECLARATIONS ";
        //    cmd.CommandText += " WHERE COUNTEDDATETIME = @countedDateTime";
        //    cmd.CommandText += " and DATAAREAID = @DataAreaID";
        //    cmd.CommandText += " order by COUNTEDDATETIME desc";

        //    MakeParam(cmd, "countedDateTime", (DateTime)id.PrimaryID, SqlDbType.DateTime);
        //    MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);

        //    List<Tenderdeclaration> tdList = Execute<Tenderdeclaration>(entry, cmd, CommandType.Text, PopulateTenderDeclaration);
        //    if (tdList.Count > 0)
        //    {
        //        Tenderdeclaration td = tdList[0];
        //        td.TenderDeclarationLines = GetTenderDeclarationLines(entry, td.ID);
        //        return td;
        //    }
        //    else
        //        return null;
        //}
    }
}
