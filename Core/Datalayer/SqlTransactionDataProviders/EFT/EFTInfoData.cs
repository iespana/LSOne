using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders.EFT;
using LSOne.DataLayer.TransactionObjects.EFT;
using LSOne.Services.Interfaces.Enums.EFT;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders.EFT
{
    public class EFTInfoData : SqlServerDataProviderBase, IEFTInfoData
    {
        private static void Populate(IDataReader dr, EFTInfo eftInfo)
        {
            eftInfo.TransactionID = (string)dr["TRANSACTIONID"];
            eftInfo.LineNumber = (decimal)dr["LINENUM"];
            eftInfo.StoreID = (string)dr["STORE"];
            eftInfo.TerminalID = (string)dr["TERMINAL"];
            eftInfo.Timeout = (int)dr["TIMEOUT"];
            eftInfo.MerchantNumber = (string)dr["MERCHANTNUMBER"];
            eftInfo.TerminalNumber = (string)dr["TERMINALNUMBER"];
            eftInfo.Track2 = (string)dr["TRACK2"];
            eftInfo.CardNumber = (string)dr["CARDNUMBER"];
            eftInfo.CardNumberHidden = ((byte)dr["CARDNUMBERHIDDEN"] != 0);
            eftInfo.ExpDate = (string)dr["EXPDATE"];
            eftInfo.CardEntryType = (CardEntryTypesEnum)(byte)dr["CARDENTRYTYPE"];
            eftInfo.CardName = (string)dr["CARDNAME"];
            eftInfo.Amount = (decimal)dr["AMOUNT"];
            eftInfo.AmountInCents = (decimal)dr["AMOUNTINCENTS"];
            eftInfo.StaffId = (string)dr["STAFFID"];
            eftInfo.TransactionNumber = (int)dr["TRANSACTIONNUMBER"];
            eftInfo.Authorized = ((byte)dr["AUTHORIZED"] != 0);
            eftInfo.AuthCode = (string)dr["AUTHCODE"];
            eftInfo.CardType = (EFTCardTypes)(int)dr["CARDTYPE"];
            eftInfo.TenderType = (string)dr["TENDERTYPE"];
            eftInfo.AcquirerName = (string)dr["ACQUIRERNAME"];
            eftInfo.TransactionDateTime = new Date((DateTime)dr["TRANSACTIONDATETIME"]);
            eftInfo.IssuerNumber = (int)dr["ISSUERNUMBER"];
            eftInfo.BatchNumber = (long)dr["BATCHNUMBER"];
            eftInfo.ResponseCode = (string)dr["RESPONSECODE"];
            eftInfo.AuthSourceCode = (string)dr["AUTHSOURCECODE"];
            eftInfo.EntrySourceCode = (string)dr["ENTRYSOURCECODE"];
            eftInfo.ProcessLocally = ((byte)dr["PROCESSLOCALLY"] != 0);
            eftInfo.AuthString = (string)dr["AUTHSTRING"];
            eftInfo.VisaAuthCode = (string)dr["VISAAUTHCODE"];
            eftInfo.EuropayAuthCode = (string)dr["EUROPAYAUTHCODE"];
            eftInfo.BatchCode = (string)dr["BATCHCODE"];
            eftInfo.SequenceCode = (string)dr["SEQUENCECODE"];
            eftInfo.NotAuthorizedText = (string)dr["NOTAUTHORIZEDTEXT"];
            eftInfo.AuthorizedText = (string)dr["AUTHORIZEDTEXT"];
            eftInfo.TransactionType = (TransactionType)dr["TRANSACTIONTYPE"];
            eftInfo.Message = (string)dr["MESSAGE"];
            eftInfo.IssuerName = (string)dr["ISSUERNAME"];
            eftInfo.ErrorCode = (string)dr["ERRORCODE"];
            eftInfo.TrackSeperator = (string)dr["TRACKSEPERATOR"];
            eftInfo.PreviousSequenceCode = (string)dr["PREVIOUSSEQUENCECODE"];
            eftInfo.ExternalCardReceipts = new List<string>();
            var extCardOriginal = (string)dr["EXTERNALCARDRECEIPTS"];
            if (extCardOriginal.Length > 0)
            {
                var extCardReceiptStrings = extCardOriginal.Split('\n');
                foreach (string str in extCardReceiptStrings)
                {
                    eftInfo.ExternalCardReceipts.Add(str);
                }
            }
            var signature = dr["SIGNATURE"];
            if (signature != null && signature is byte[])
            {
                eftInfo.Signature = (byte[]) signature;
            }
            eftInfo.ReceiptLines = AsString(dr["RECEIPTLINES"]);
            eftInfo.ReceiptPrinting = (EFTReceiptPrinting) AsInt(dr["RECEIPTPRINTING"]);
        }

        private static List<EFTInfo> DoGet(IConnectionManager entry, RecordIdentifier id, bool withLine, CacheType cache)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TRANSACTIONID, LINENUM, STORE, TERMINAL, ISNULL(TIMEOUT, 0) AS TIMEOUT, 
                                    ISNULL(MERCHANTNUMBER, '') AS MERCHANTNUMBER, ISNULL(TERMINALNUMBER, '') AS TERMINALNUMBER, 
                                    ISNULL(TRACK2, '') AS TRACK2, ISNULL(CARDNUMBER, '') AS CARDNUMBER, ISNULL(CARDNUMBERHIDDEN, 0) AS CARDNUMBERHIDDEN,
                                    ISNULL(EXPDATE, '') AS EXPDATE, ISNULL(CARDENTRYTYPE, 0) AS CARDENTRYTYPE, ISNULL(CARDNAME, '') AS CARDNAME,
                                    ISNULL(AMOUNT, 0) AS AMOUNT, ISNULL(AMOUNTINCENTS, 0) AS AMOUNTINCENTS, ISNULL(STAFFID, '') AS STAFFID,
                                    ISNULL(TRANSACTIONNUMBER, 0) AS TRANSACTIONNUMBER, ISNULL(AUTHORIZED, 0) AS AUTHORIZED, 
                                    ISNULL(AUTHCODE, '') AS AUTHCODE, ISNULL(CARDTYPE, 0) AS CARDTYPE, ISNULL(TENDERTYPE, '') AS TENDERTYPE,
                                    ISNULL(ACQUIRERNAME, '') AS ACQUIRERNAME, ISNULL(TRANSACTIONDATETIME, '1900-01-01')AS TRANSACTIONDATETIME,
                                    ISNULL(ISSUERNUMBER, 0) AS ISSUERNUMBER, ISNULL(BATCHNUMBER, 0) AS BATCHNUMBER, 
                                    ISNULL(RESPONSECODE, '') AS RESPONSECODE, ISNULL(AUTHSOURCECODE, '') AS AUTHSOURCECODE, 
                                    ISNULL(ENTRYSOURCECODE, '') AS ENTRYSOURCECODE, ISNULL(PROCESSLOCALLY, 0) AS PROCESSLOCALLY,
                                    ISNULL(AUTHSTRING, '') AS AUTHSTRING, ISNULL(VISAAUTHCODE, '') AS VISAAUTHCODE,
                                    ISNULL(EUROPAYAUTHCODE, '') AS EUROPAYAUTHCODE, ISNULL(BATCHCODE, '') AS BATCHCODE,
                                    ISNULL(SEQUENCECODE, '') AS SEQUENCECODE, ISNULL(NOTAUTHORIZEDTEXT, '') AS NOTAUTHORIZEDTEXT,
                                    ISNULL(AUTHORIZEDTEXT, '') AS AUTHORIZEDTEXT, ISNULL(TRANSACTIONTYPE, 0) AS TRANSACTIONTYPE,
                                    ISNULL(MESSAGE, '') AS MESSAGE, ISNULL(ISSUERNAME, '') AS ISSUERNAME, ISNULL(ERRORCODE, '') AS ERRORCODE,
                                    ISNULL(TRACKSEPERATOR, '') AS TRACKSEPERATOR, ISNULL(PREVIOUSSEQUENCECODE, '') AS PREVIOUSSEQUENCECODE,
                                    ISNULL(EXTERNALCARDRECEIPTS, '') AS EXTERNALCARDRECEIPTS,
                                    SIGNATURE,
                                    ISNULL(RECEIPTLINES, '') AS RECEIPTLINES,
                                    ISNULL(RECEIPTPRINTING, 0) AS RECEIPTPRINTING
                                    FROM RBOTRANSACTIONEFTINFOTRANS
                                    WHERE DATAAREAID = @dataAreaID AND TRANSACTIONID = @transactionID AND ";
                if (withLine)
                    cmd.CommandText += "LINENUM = @lineNumber AND ";
                cmd.CommandText += "TERMINAL = @terminal AND STORE = @store";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                int index = 0;
                MakeParam(cmd, "transactionID", (string)id[index++]);
                if (withLine)
                    MakeParam(cmd, "lineNumber", (int)id[index++], SqlDbType.Decimal);
                MakeParam(cmd, "terminal", (string)id[index++]);
                MakeParam(cmd, "store", (string)id[index++]);
                return Execute<EFTInfo>(entry, cmd, CommandType.Text, Populate);
            }
        }

        /// <summary>
        /// Get a single eft transaction line for a transaction
        /// </summary>
        /// <param name="entry">Entry into database</param>
        /// <param name="id">Transaction id + line number + terminal id + store id</param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public virtual EFTInfo Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            var lines = DoGet(entry, id, true, cache);
            if (lines != null && lines.Count > 0)
                return lines[0];

            return null;
        }

        /// <summary>
        /// Get all single eft transaction lines for a transaction
        /// </summary>
        /// <param name="entry">Entry into database</param>
        /// <param name="id">Transaction id + terminal id + store id</param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public virtual List<EFTInfo> GetAll(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            return DoGet(entry, id, false, cache);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            string[] fields = { "TRANSACTIONID", "LINENUM", "TERMINAL", "STORE" };
            return RecordExists(entry, "RBOTRANSACTIONEFTINFOTRANS", fields, id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            string[] fields = { "TRANSACTIONID", "LINENUM", "TERMINAL", "STORE" };
            DeleteRecord(entry, "RBOTRANSACTIONEFTINFOTRANS", fields, ID, "");
        }

        public virtual void Save(IConnectionManager entry, EFTInfo info)
        {
            info.Validate();
            var statement = new SqlServerStatement("RBOTRANSACTIONEFTINFOTRANS");
            if (Exists(entry, info.ID))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("TRANSACTIONID", (string)info.TransactionID);
                statement.AddCondition("LINENUM", (decimal)info.LineNumber, SqlDbType.Decimal);
                statement.AddCondition("TERMINAL", (string)info.TerminalID);
                statement.AddCondition("STORE", (string)info.StoreID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("TRANSACTIONID", (string)info.TransactionID);
                statement.AddKey("LINENUM", (decimal)info.LineNumber, SqlDbType.Decimal);
                statement.AddKey("TERMINAL", (string)info.TerminalID);
                statement.AddKey("STORE", (string)info.StoreID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("TIMEOUT", info.Timeout, SqlDbType.Int);
            statement.AddField("MERCHANTNUMBER", info.MerchantNumber);
            statement.AddField("TERMINALNUMBER", (string)info.TerminalNumber);
            statement.AddField("TRACK2", info.Track2);
            statement.AddField("CARDNUMBER", info.CardNumber);
            statement.AddField("CARDNUMBERHIDDEN",info.CardNumberHidden, SqlDbType.TinyInt);
            statement.AddField("EXPDATE", info.ExpDate);
            statement.AddField("CARDENTRYTYPE", info.CardEntryType, SqlDbType.TinyInt);
            statement.AddField("CARDNAME", info.CardName);
            statement.AddField("AMOUNT", info.Amount, SqlDbType.Decimal);
            statement.AddField("AMOUNTINCENTS", info.AmountInCents, SqlDbType.Decimal);
            statement.AddField("STAFFID", (string)info.StaffId);
            statement.AddField("TRANSACTIONNUMBER", info.TransactionNumber, SqlDbType.Int);
            statement.AddField("AUTHORIZED", info.Authorized, SqlDbType.TinyInt);
            statement.AddField("AUTHCODE", info.AuthCode);
            statement.AddField("CARDTYPE", info.CardType, SqlDbType.Int);
            statement.AddField("TENDERTYPE", info.TenderType);
            statement.AddField("ACQUIRERNAME", info.AcquirerName);
            statement.AddField("TRANSACTIONDATETIME", info.TransactionDateTime.DateTime, SqlDbType.DateTime);
            statement.AddField("ISSUERNUMBER", (int)info.IssuerNumber, SqlDbType.Int);
            statement.AddField("BATCHNUMBER", info.BatchNumber, SqlDbType.BigInt);
            statement.AddField("RESPONSECODE", info.ResponseCode);
            statement.AddField("AUTHSOURCECODE", info.AuthSourceCode);
            statement.AddField("ENTRYSOURCECODE", info.EntrySourceCode);
            statement.AddField("PROCESSLOCALLY", info.ProcessLocally, SqlDbType.TinyInt);
            statement.AddField("AUTHSTRING", info.AuthString);
            statement.AddField("VISAAUTHCODE", info.VisaAuthCode);
            statement.AddField("EUROPAYAUTHCODE", info.EuropayAuthCode);
            statement.AddField("BATCHCODE", info.BatchCode);
            statement.AddField("SEQUENCECODE", info.SequenceCode);
            statement.AddField("NOTAUTHORIZEDTEXT", info.NotAuthorizedText);
            statement.AddField("AUTHORIZEDTEXT", info.AuthorizedText);
            statement.AddField("TRANSACTIONTYPE", info.TransactionType, SqlDbType.Int);
            statement.AddField("MESSAGE", info.Message);
            statement.AddField("ISSUERNAME", info.IssuerName);
            statement.AddField("ERRORCODE", info.ErrorCode);
            statement.AddField("TRACKSEPERATOR", info.TrackSeperator);
            statement.AddField("PREVIOUSSEQUENCECODE", info.PreviousSequenceCode);
            string externalCardReceipts = "";
            foreach (string current in info.ExternalCardReceipts)
            {
                externalCardReceipts += current;
                externalCardReceipts += "\n";
            }
            if (externalCardReceipts.Length > 0)
            {
                externalCardReceipts = externalCardReceipts.Substring(0, externalCardReceipts.Length - 1);
            }
            statement.AddField("EXTERNALCARDRECEIPTS", externalCardReceipts);
            if (info.Signature == null)
                statement.AddField("SIGNATURE", DBNull.Value, SqlDbType.Binary);
            else
                statement.AddField("SIGNATURE", info.Signature, SqlDbType.Binary);
            if (string.IsNullOrEmpty(info.ReceiptLines))
                statement.AddField("RECEIPTLINES", DBNull.Value, SqlDbType.NVarChar);
            else
                statement.AddField("RECEIPTLINES", info.ReceiptLines);
            statement.AddField("RECEIPTPRINTING", (int)info.ReceiptPrinting, SqlDbType.Int);
            Save(entry, info, statement);
        }
    }
}
