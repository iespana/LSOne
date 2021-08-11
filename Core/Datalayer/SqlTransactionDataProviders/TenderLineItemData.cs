using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Card;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlDataProviders.Card;
using LSOne.DataLayer.SqlDataProviders.StoreManagement;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class TenderLineItemData : SqlServerDataProviderBase, ITenderLineItemData
    {
        private static TenderLineItem Populate(IConnectionManager entry, IDataReader dr, PosTransaction transaction)
        {
            TenderLineItem tenderItem;
            string cardOrAccount = (string)dr["CARDORACCOUNT"];
            string cardTypeID = (string)dr["CARDTYPEID"];
            string creditVoucherID = (string)dr["CREDITVOUCHERID"];
            string giftCardID = (string)dr["GIFTCARDID"];
            string loyaltyCardID = (string)dr["LOYALTYCARDID"];
            string checkID = (string)dr["CHECKID"];
            TenderTypeEnum typeOfTender = (int) dr["TENDERLINETYPE"] == -1 ? TenderTypeEnum.None : (TenderTypeEnum) (int) dr["TENDERLINETYPE"];

            //If CardOrAccount is not empty but CardTypeId is then the payment was a Customer account
            if (cardOrAccount != "" && cardTypeID == "")
            {
                tenderItem = new CustomerTenderLineItem();
                ((CustomerTenderLineItem)tenderItem).ManualAuthCode = (string)dr["AUTHENTICATIONCODE"];
                ((CustomerTenderLineItem)tenderItem).CustomerId = cardOrAccount;
            }
            //If the CreditVoucherId is not empty then the payment was a Credit Memo
            else if (creditVoucherID != "")
            {
                tenderItem = new CreditMemoTenderLineItem();
                ((CreditMemoTenderLineItem)tenderItem).SerialNumber = creditVoucherID;
            }
            //If the GiftCardId is not empty then the payment was a Gift card
            else if (giftCardID != "")
            {
                tenderItem = new GiftCertificateTenderLineItem();
                ((GiftCertificateTenderLineItem)tenderItem).SerialNumber = giftCardID;
            }
            //If the LoyaltyCardId is not empty then the payment was a loyalty card
            else if (loyaltyCardID != "")
            {
                tenderItem = new LoyaltyTenderLineItem();
                ((LoyaltyTenderLineItem)tenderItem).CardNumber = loyaltyCardID;
                ((LoyaltyTenderLineItem)tenderItem).Points = 0;
                ((LoyaltyTenderLineItem)tenderItem).Points = (decimal)dr["LOYALTYPOINTS"];           
            }
            //If the CardTypeId is not empty then the payment was a credit card
            else if (cardTypeID != "")
            {
                CardTypesEnum cardType = (CardTypesEnum)dr["CARDTYPES"];

                if (cardType == CardTypesEnum.CorporateCard)
                {
                    string driverID = (string)dr["DRIVERID"];
                    tenderItem = new CorporateCardTenderLineItem();
                    ((CorporateCardTenderLineItem)tenderItem).DriverId = driverID;
                    ((CorporateCardTenderLineItem)tenderItem).VehicleId = (string)dr["VEHICLEID"];
                    ((CorporateCardTenderLineItem)tenderItem).OdometerReading = (int)dr["ODOMETERREADING"];
                }
                else
                {
                    tenderItem = new CardTenderLineItem();
                }
                ((CardTenderLineItem)tenderItem).CardNumber = cardOrAccount;
                ((CardTenderLineItem)tenderItem).CardTypeID = cardTypeID;
                ((CardTenderLineItem)tenderItem).CardNumberHidden = ((byte)dr["CARDNUMBERHIDDEN"] != 0);
                ((CardTenderLineItem)tenderItem).CardName = (string)dr["CARDNAME"];
                ((CardTenderLineItem)tenderItem).IssuerName = (string)dr["CARDISSUER"];
                ((CardTenderLineItem)tenderItem).ExpiryDate = (string)dr["EXPIRYDATE"];
                ((CardTenderLineItem)tenderItem).CardHolderName = (string)dr["CARDHOLDERNAME"];
                ((CardTenderLineItem)tenderItem).CardBin = (string)dr["CARDBIN"];
                ((CardTenderLineItem)tenderItem).MerchantID = (string)dr["MERCHANTID"];
                ((CardTenderLineItem)tenderItem).AuthCode = (string)dr["AUTHCODE"];
                ((CardTenderLineItem)tenderItem).Message = (string)dr["MESSAGE"];
                ((CardTenderLineItem)tenderItem).ResponseCode = (string)dr["RESPONSECODE"];
                if (tenderItem is CardTenderLineItem)
                {
                    var id = new RecordIdentifier(transaction.TransactionId,
                                            new RecordIdentifier((int)((decimal)dr["LINENUM"]),
                                            new RecordIdentifier(transaction.TerminalId, transaction.StoreId)));
                    ((CardTenderLineItem)tenderItem).EFTInfo = TransactionProviders.EFTInfoData.Get(entry.CreateTemporaryConnection(), id);
                }
            }
            else if (checkID != "")
            {
                tenderItem = new ChequeTenderLineItem();
                ((ChequeTenderLineItem)tenderItem).CheckID = checkID;
            }
            else if (typeOfTender == TenderTypeEnum.DepositTender)
            {
                tenderItem = new DepositTenderLineItem();
                ((DepositTenderLineItem)tenderItem).RedeemedDeposit = ((int)dr["TRANSACTIONSTATUS"] == (int)TransactionStatus.Redeemed);
            }
            else
            {
                tenderItem = new TenderLineItem();
            }                
            tenderItem.Transaction = transaction;
            tenderItem.LineId = (int)((decimal)dr["LINENUM"]);
            tenderItem.ExchangeRate = (decimal)dr["EXCHRATE"];
            tenderItem.TenderTypeId = (string)dr["TENDERTYPE"];
            tenderItem.Amount = (decimal)dr["AMOUNTTENDERED"];
            tenderItem.CurrencyCode = (string)dr["CURRENCY"];
            tenderItem.BeginDateTime = (DateTime)dr["TRANSDATE"];
            tenderItem.Voided = ((int)dr["TRANSACTIONSTATUS"] == (int)TransactionStatus.Voided);
            if (tenderItem.ExchangeRate != 100)
            {
                tenderItem.ForeignCurrencyAmount = (decimal)dr["AMOUNTCUR"];
            }
            tenderItem.CompanyCurrencyAmount = (decimal)dr["AMOUNTMST"];
            tenderItem.ExchrateMST = (decimal)dr["EXCHRATEMST"];
            tenderItem.Comment = (string)dr["COMMENT"];

            if (dr["DESCRIPTION"] == DBNull.Value)
            {
                StorePaymentMethod paymentMethod = DataProviderFactory.Instance.
                    Get<IStorePaymentMethodData, StorePaymentMethod>().Get(entry, 
                    new RecordIdentifier(entry.CurrentStoreID, 
                        new RecordIdentifier(tenderItem.TenderTypeId)), 
                    CacheType.CacheTypeApplicationLifeTime);
                tenderItem.Description = paymentMethod.Text;
            }
            else
            {
                tenderItem.Description = (string)dr["DESCRIPTION"];
            }

            tenderItem.ChangeBack = ((byte)dr["CHANGELINE"] == 1);

            return tenderItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">TransactionID, TerminalID, StoreID</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual List<TenderLineItem> Get(IConnectionManager entry, RecordIdentifier id, PosTransaction transaction)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT ISNULL(p.CARDORACCOUNT, '') AS CARDORACCOUNT, ISNULL(p.CARDTYPEID, '') AS CARDTYPEID,
                    ISNULL(p.CREDITVOUCHERID, '') AS CREDITVOUCHERID, ISNULL(p.AUTHENTICATIONCODE, '') AS AUTHENTICATIONCODE,
                    ISNULL(p.GIFTCARDID, '') AS GIFTCARDID, ISNULL(p.LOYALTYCARDID, '') AS LOYALTYCARDID,
                    ISNULL(p.LOYALTYPOINTS, 0) AS LOYALTYPOINTS, ISNULL(p.DRIVERID, '') AS DRIVERID, ISNULL(p.VEHICLEID, '') AS VEHICLEID,
                    ISNULL(p.ODOMETERREADING, 0) AS ODOMETERREADING, ISNULL(p.BATCHID, '') AS BATCHID, 
                    ISNULL(p.CARDNUMBERHIDDEN, 0) AS CARDNUMBERHIDDEN, ISNULL(p.CARDNAME, '') AS CARDNAME, ISNULL(p.CARDISSUER, '') AS CARDISSUER,
                    ISNULL(p.EXPIRYDATE, '') AS EXPIRYDATE, ISNULL(p.CARDHOLDERNAME, '') AS CARDHOLDERNAME,
                    ISNULL(p.CARDENTRYTYPE, 0) AS CARDENTRYTYPE, ISNULL(p.CARDBIN, '') AS CARDBIN, ISNULL(p.MERCHANTID, '') AS MERCHANTID,
                    ISNULL(p.AUTHCODE, '') AS AUTHCODE, ISNULL(p.MESSAGE, '') AS MESSAGE, ISNULL(p.RESPONSECODE, '') AS RESPONSECODE,
                    ISNULL(p.LINENUM, 0) AS LINENUM, ISNULL(p.EXCHRATE, 0) AS EXCHRATE, ISNULL(p.TENDERTYPE, '') AS TENDERTYPE,
                    ISNULL(p.AMOUNTTENDERED, 0) AS AMOUNTTENDERED, ISNULL(p.CURRENCY, '') AS CURRENCY, 
                    ISNULL(p.TRANSDATE, '1900-01-01') AS TRANSDATE, ISNULL(p.TRANSACTIONSTATUS, 0) AS TRANSACTIONSTATUS,
                    ISNULL(p.AMOUNTCUR, 0) AS AMOUNTCUR, ISNULL(p.AMOUNTMST, 0) AS AMOUNTMST, ISNULL(p.EXCHRATEMST, 0) AS EXCHRATEMST,
                    ISNULL(p.COMMENT, '') AS COMMENT, p.DESCRIPTION, 
                    ISNULL(p.CHECKID, '') as CHECKID,
                    ISNULL(p.CHANGELINE, 0) as CHANGELINE,                    
                    ISNULL(c.CARDTYPES, 0) as CARDTYPES,
                    ISNULL(P.TENDERLINETYPE, 0) AS TENDERLINETYPE
                    FROM RBOTRANSACTIONPAYMENTTRANS p
                    LEFT OUTER JOIN RBOTENDERTYPECARDTABLE c on c.CARDTYPEID = p.CARDTYPEID and c.DATAAREAID = p.DATAAREAID
                    WHERE p.DATAAREAID = @dataAreaID AND p.TRANSACTIONID = @transactionID AND
                    p.TERMINAL = @terminalID AND p.STORE = @storeID";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", id[0]);
                MakeParam(cmd, "terminalID", id[1]);
                MakeParam(cmd, "storeID", id[2]);
                
                var items =  Execute(entry, cmd, CommandType.Text, transaction, Populate);
                foreach (TenderLineItem tenderItem in items)
                {
                    var infoCodeID = new RecordIdentifier(transaction.TransactionId, new RecordIdentifier(tenderItem.LineId, new RecordIdentifier((int)InfoCodeLineItem.InfocodeType.Payment, new RecordIdentifier(transaction.TerminalId, transaction.StoreId))));
                    tenderItem.InfoCodeLines = TransactionProviders.InfocodeTransactionData.Get(entry, infoCodeID, transaction);
                }
                return items;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">TransactionID, TerminalID, StoreID</param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual List<TenderLineItem> HMGet(IConnectionManager entry, RecordIdentifier id, PosTransaction transaction)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT ISNULL(p.CARDORACCOUNT, '') AS CARDORACCOUNT, ISNULL(p.CARDTYPEID, '') AS CARDTYPEID,
                    ISNULL(p.CREDITVOUCHERID, '') AS CREDITVOUCHERID, ISNULL(p.AUTHENTICATIONCODE, '') AS AUTHENTICATIONCODE,
                    ISNULL(p.GIFTCARDID, '') AS GIFTCARDID, ISNULL(p.LOYALTYCARDID, '') AS LOYALTYCARDID,
                    ISNULL(p.LOYALTYPOINTS, 0) AS LOYALTYPOINTS, ISNULL(p.DRIVERID, '') AS DRIVERID, ISNULL(p.VEHICLEID, '') AS VEHICLEID,
                    ISNULL(p.ODOMETERREADING, 0) AS ODOMETERREADING, ISNULL(p.BATCHID, '') AS BATCHID, 
                    ISNULL(p.CARDNUMBERHIDDEN, 0) AS CARDNUMBERHIDDEN, ISNULL(p.CARDNAME, '') AS CARDNAME, ISNULL(p.CARDISSUER, '') AS CARDISSUER,
                    ISNULL(p.EXPIRYDATE, '') AS EXPIRYDATE, ISNULL(p.CARDHOLDERNAME, '') AS CARDHOLDERNAME,
                    ISNULL(p.CARDENTRYTYPE, 0) AS CARDENTRYTYPE, ISNULL(p.CARDBIN, '') AS CARDBIN, ISNULL(p.MERCHANTID, '') AS MERCHANTID,
                    ISNULL(p.AUTHCODE, '') AS AUTHCODE, ISNULL(p.MESSAGE, '') AS MESSAGE, ISNULL(p.RESPONSECODE, '') AS RESPONSECODE,
					MAX( ISNULL(p.LINENUM, 0)) AS LINENUM, ISNULL(p.EXCHRATE, 0) AS EXCHRATE, ISNULL(p.TENDERTYPE, '') AS TENDERTYPE,
					SUM( ISNULL(p.AMOUNTTENDERED, 0)) AS AMOUNTTENDERED, ISNULL(p.CURRENCY, '') AS CURRENCY, 
					MAX( ISNULL(p.TRANSDATE, '1900-01-01')) AS TRANSDATE, ISNULL(p.TRANSACTIONSTATUS, 0) AS TRANSACTIONSTATUS,
                    SUM(ISNULL(p.AMOUNTCUR, 0)) AS AMOUNTCUR, 
					SUM(ISNULL(p.AMOUNTMST, 0)) AS AMOUNTMST, ISNULL(p.EXCHRATEMST, 0) AS EXCHRATEMST,
                    ISNULL(p.COMMENT, '') AS COMMENT, p.DESCRIPTION, 
                    ISNULL(p.CHECKID, '') as CHECKID,
                    ISNULL(p.CHANGELINE, 0) as CHANGELINE,
                    ISNULL(c.CARDTYPES, 0) as CARDTYPES
                    FROM RBOTRANSACTIONPAYMENTTRANS p
                    LEFT OUTER JOIN RBOTENDERTYPECARDTABLE c on c.CARDTYPEID = p.CARDTYPEID and c.DATAAREAID = p.DATAAREAID
                    WHERE p.DATAAREAID = @dataAreaID AND p.TRANSACTIONID = @transactionID AND
                    p.TERMINAL = @terminalID AND p.STORE = @storeID AND TRANSACTIONSTATUS = 0
	                GROUP BY 
						CARDORACCOUNT, 
						p.CARDTYPEID,
						CREDITVOUCHERID, 
						AUTHENTICATIONCODE,
						GIFTCARDID, 
						LOYALTYCARDID,
						LOYALTYPOINTS, 
						DRIVERID, 
						VEHICLEID,
						ODOMETERREADING, 
						BATCHID, 
						CARDNUMBERHIDDEN, 
						CARDNAME, 
						p.CARDISSUER,
						EXPIRYDATE, 
						CARDHOLDERNAME,
						CARDENTRYTYPE, 
						CARDBIN,
						MERCHANTID,
						AUTHCODE, 
						MESSAGE,
						RESPONSECODE,
						EXCHRATE, 
						TENDERTYPE,
						CURRENCY, 
						TRANSACTIONSTATUS,
						EXCHRATEMST,
						COMMENT, 
						p.DESCRIPTION, 
						CHECKID,
                        p.CHANGELINE,
						CARDTYPES";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "transactionID", id[0]);
                MakeParam(cmd, "terminalID", id[1]);
                MakeParam(cmd, "storeID", id[2]);

                var items = Execute(entry, cmd, CommandType.Text, transaction, Populate);
                foreach (TenderLineItem tenderItem in items)
                {
                    var infoCodeID = new RecordIdentifier(transaction.TransactionId, new RecordIdentifier(tenderItem.LineId, new RecordIdentifier((int)InfoCodeLineItem.InfocodeType.Payment, new RecordIdentifier(transaction.TerminalId, transaction.StoreId))));
                    tenderItem.InfoCodeLines = TransactionProviders.InfocodeTransactionData.Get(entry, infoCodeID, transaction);
                }
                return items;
            }
        }
    
    }
}
