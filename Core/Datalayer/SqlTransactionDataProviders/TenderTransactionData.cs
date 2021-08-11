using System;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Card;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlDataProviders.Card;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.EFT;
using LSOne.DataLayer.TransactionObjects.Line.Loyalty;
using LSOne.DataLayer.TransactionObjects.Line.TenderItem;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class TenderTransactionData : SqlServerDataProviderBase, ITenderTransactionData
    {
        public virtual void Insert(IConnectionManager entry, ITenderLineItem tenderItem, PosTransaction transaction)
        {
            string storeCurrencyCode = "";

            ValidateSecurity(entry);

            var statement = new SqlServerStatement("RBOTRANSACTIONPAYMENTTRANS", StatementType.Insert, false);

            statement.AddKey("TRANSACTIONID", transaction.TransactionId);
            statement.AddKey("LINENUM", (decimal)tenderItem.LineId, SqlDbType.Decimal);

            statement.AddKey("STORE", transaction.StoreId);
            statement.AddKey("TERMINAL", transaction.TerminalId);
            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId); 

            statement.AddField("RECEIPTID", transaction.ReceiptId);
            statement.AddField("STATEMENTCODE", "");

            statement.AddField("EXCHRATE", string.IsNullOrEmpty(tenderItem.CurrencyCode) ? 100 : tenderItem.ExchangeRate, SqlDbType.Decimal);

            statement.AddField("TENDERTYPE", tenderItem.TenderTypeId); 
            
            if ((transaction.GetType() == typeof(SafeDropTransaction)) || (transaction.GetType() == typeof(BankDropTransaction)))
            {
                statement.AddField("AMOUNTMST", tenderItem.CompanyCurrencyAmount * -1, SqlDbType.Decimal);

                var amtTendered = tenderItem.Amount * -1;

                statement.AddField("AMOUNTTENDERED", amtTendered, SqlDbType.Decimal);

                if (transaction.GetType() == typeof(SafeDropTransaction))
                {
                    storeCurrencyCode = transaction.StoreCurrencyCode;
                    if (string.IsNullOrEmpty(tenderItem.CurrencyCode))
                    {
                        statement.AddField("CURRENCY", storeCurrencyCode);
                    }
                    else
                    {
                        statement.AddField("CURRENCY", tenderItem.CurrencyCode);
                    }

                }
                else if (transaction.GetType() == typeof(BankDropTransaction))
                {
                    storeCurrencyCode = transaction.StoreCurrencyCode;
                    if (string.IsNullOrEmpty(tenderItem.CurrencyCode))
                    {
                        statement.AddField("CURRENCY", storeCurrencyCode);
                    }
                    else
                    {
                        statement.AddField("CURRENCY", tenderItem.CurrencyCode);
                    }
                }

                if (tenderItem.ForeignCurrencyAmount == 0)
                {
                    statement.AddField("AMOUNTCUR", amtTendered, SqlDbType.Decimal);
                }
                else
                {
                    statement.AddField("AMOUNTCUR", (tenderItem.ForeignCurrencyAmount * -1), SqlDbType.Decimal);
                }
            }
            else if ((transaction.GetType() == typeof(SafeDropReversalTransaction)) || 
                (transaction.GetType() == typeof(BankDropReversalTransaction)))
            {
                if (transaction.GetType() == typeof(SafeDropReversalTransaction))
                {
                    storeCurrencyCode = ((SafeDropReversalTransaction)transaction).StoreCurrencyCode;
                    if (string.IsNullOrEmpty(tenderItem.CurrencyCode))
                    {
                        statement.AddField("CURRENCY", storeCurrencyCode);
                    }
                    else
                    {
                        statement.AddField("CURRENCY", tenderItem.CurrencyCode);
                    }
                }
                else if (transaction.GetType() == typeof(BankDropReversalTransaction))
                {
                    storeCurrencyCode = transaction.StoreCurrencyCode;
                    if (string.IsNullOrEmpty(tenderItem.CurrencyCode))
                    {
                        statement.AddField("CURRENCY", storeCurrencyCode);
                    }
                    else
                    {
                        statement.AddField("CURRENCY", tenderItem.CurrencyCode);
                    }
                }

                statement.AddField("AMOUNTMST", tenderItem.CompanyCurrencyAmount, SqlDbType.Decimal);
                var amtTendered = tenderItem.Amount;
                statement.AddField("AMOUNTTENDERED", amtTendered, SqlDbType.Decimal);

                if (tenderItem.ForeignCurrencyAmount == 0)
                {
                    statement.AddField("AMOUNTCUR", amtTendered, SqlDbType.Decimal);
                }
                else
                {
                    statement.AddField("AMOUNTCUR", (tenderItem.ForeignCurrencyAmount), SqlDbType.Decimal);
                }
            }
            else if ((transaction.GetType() == typeof(SafeDropReversalTransaction)) || (transaction.GetType() == typeof(BankDropReversalTransaction)))
            {
                if (transaction.GetType() == typeof(SafeDropReversalTransaction))
                {
                    storeCurrencyCode = transaction.StoreCurrencyCode;
                    if (string.IsNullOrEmpty(tenderItem.CurrencyCode))
                    {
                        statement.AddField("CURRENCY", storeCurrencyCode);
                    }
                    else
                    {
                        statement.AddField("CURRENCY", (string)tenderItem.CurrencyCode);
                    }
                }
                else if (transaction.GetType() == typeof(BankDropReversalTransaction))
                {
                    storeCurrencyCode = transaction.StoreCurrencyCode;
                    if (string.IsNullOrEmpty(tenderItem.CurrencyCode))
                    {
                        statement.AddField("CURRENCY", storeCurrencyCode);
                    }
                    else
                    {
                        statement.AddField("CURRENCY", (string)tenderItem.CurrencyCode);
                    }
                }

                statement.AddField("AMOUNTMST", tenderItem.CompanyCurrencyAmount, SqlDbType.Decimal);
                var amtTendered = tenderItem.Amount;
                statement.AddField("AMOUNTTENDERED", amtTendered, SqlDbType.Decimal);
                if (tenderItem.ForeignCurrencyAmount == 0)
                {
                    statement.AddField("AMOUNTCUR", amtTendered, SqlDbType.Decimal);
                }
                else
                {
                    statement.AddField("AMOUNTCUR", (tenderItem.ForeignCurrencyAmount), SqlDbType.Decimal);
                }
            }
            else  //this is for all other transaction types
            {
                statement.AddField("AMOUNTMST", tenderItem.CompanyCurrencyAmount, SqlDbType.Decimal);
                var amtTendered = tenderItem.Amount;
                statement.AddField("AMOUNTTENDERED", amtTendered, SqlDbType.Decimal);
                if (tenderItem.ForeignCurrencyAmount == 0)
                {
                    statement.AddField("AMOUNTCUR", amtTendered, SqlDbType.Decimal);
                }
                else
                {
                    statement.AddField("AMOUNTCUR", (tenderItem.ForeignCurrencyAmount), SqlDbType.Decimal);
                }
            }

            statement.AddField("EXCHRATEMST", tenderItem.ExchrateMST, SqlDbType.Decimal);

            if (transaction is IRetailTransaction) // TODO: Temp
            {
                storeCurrencyCode = ((IRetailTransaction)transaction).StoreCurrencyCode;
                if (string.IsNullOrEmpty(tenderItem.CurrencyCode))
                {
                    statement.AddField("CURRENCY", storeCurrencyCode);
                }
                else
                {
                    statement.AddField("CURRENCY", tenderItem.CurrencyCode);
                }

                if (tenderItem.GetType() == typeof(CorporateCardTenderLineItem))
                {
                    var amtTendered = tenderItem.Amount - ((IRetailTransaction)transaction).IMarkupItem.Amount;
                }
                statement.AddField("MARKUPAMOUNT", ((IRetailTransaction)transaction).IMarkupItem.Amount, SqlDbType.Decimal);
            }
            else if (transaction.GetType() == typeof(CustomerPaymentTransaction))
            {
                storeCurrencyCode = transaction.StoreCurrencyCode;
                if (string.IsNullOrEmpty(tenderItem.CurrencyCode))
                {
                    statement.AddField("CURRENCY", storeCurrencyCode);
                }
                else
                {
                    statement.AddField("CURRENCY", (string)tenderItem.CurrencyCode);
                }
            }

            statement.AddField("TRANSDATE", tenderItem.BeginDateTime, SqlDbType.DateTime);
            statement.AddField("TRANSTIME", Conversion.TimeToInt(tenderItem.BeginDateTime), SqlDbType.Int);

            statement.AddField("SHIFT", (string)transaction.ShiftId);
            if (string.IsNullOrEmpty(transaction.ShiftId))
            {
                statement.AddField("SHIFTDATE", 0, SqlDbType.Int);
            }
            else
            {
                statement.AddField("SHIFTDATE", transaction.ShiftDate, SqlDbType.DateTime);
            }

            statement.AddField("STAFF", (string)transaction.Cashier.ID);

            if (tenderItem is DepositTenderLineItem)
            {
                statement.AddField("TRANSACTIONSTATUS", tenderItem.Voided ? (int) TransactionStatus.Voided :
                    ((DepositTenderLineItem) tenderItem).RedeemedDeposit ? (int) TransactionStatus.Redeemed : (int) TransactionStatus.Normal,
                    SqlDbType.Int);
            }
            else
            {
                statement.AddField("TRANSACTIONSTATUS", tenderItem.Voided ? (int) TransactionStatus.Voided : (int) TransactionStatus.Normal, SqlDbType.Int);
            }

            statement.AddField("STATEMENTID", "");
            statement.AddField("MANAGERKEYLIVE", 0, SqlDbType.TinyInt);
            statement.AddField("CHANGELINE",  tenderItem.ChangeBack ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("COUNTER", tenderItem.LineId, SqlDbType.Int);
            statement.AddField("MESSAGENUM", 0, SqlDbType.Int);
            statement.AddField("REPLICATED", 0, SqlDbType.TinyInt);
            statement.AddField("QTY", (decimal)1, SqlDbType.Decimal);
            statement.AddField("ZREPORTID", "");

            string cardOrAccount = "";
            string cardTypeId = "";
            string eftAuthCode = "";
            string batchId = "";

            //Going through the diference between the different types of tender types
            if ((tenderItem.GetType() == typeof(CardTenderLineItem)))
            {
                var card = (CardTenderLineItem)tenderItem;
                cardOrAccount = card.CardNumber;  // if the cardnumber is trimmed from EFT service
                cardTypeId = card.CardTypeID;

                eftAuthCode = card.EFTInfo.AuthCode;
                batchId = card.EFTInfo.BatchCode;

                statement.AddField("CARDNUMBERHIDDEN", card.CardNumberHidden ? 1: 0 , SqlDbType.TinyInt);
                statement.AddField("CARDNAME", card.CardName);
                statement.AddField("CARDISSUER", card.IssuerName);
                statement.AddField("EXPIRYDATE", card.ExpiryDate);
                statement.AddField("CARDHOLDERNAME", card.CardHolderName);
                statement.AddField("CARDENTRYTYPE", (int)card.EFTInfo.CardEntryType, SqlDbType.Int);
                statement.AddField("CARDBIN", card.CardBin);
                statement.AddField("MERCHANTID", card.MerchantID);
                statement.AddField("AUTHCODE", card.AuthCode);
                statement.AddField("MESSAGE", card.Message);
                statement.AddField("RESPONSECODE", card.ResponseCode);
                if (card.EFTInfo != null)
                {
                    card.EFTInfo.LineNumber = (decimal)tenderItem.LineId;
                    card.EFTInfo.TransactionID = transaction.TransactionId;
                    card.EFTInfo.StoreID = transaction.StoreId;
                    card.EFTInfo.TerminalID = transaction.TerminalId;
                    card.EFTInfo.AmountInCents = tenderItem.Amount * 100;
                    
                    TransactionProviders.EFTInfoData.Save(entry, (EFTInfo)card.EFTInfo);
                }
            }
            else if ((tenderItem.GetType() == typeof(CorporateCardTenderLineItem)))
            {
                CardTenderLineItem card = (CorporateCardTenderLineItem)tenderItem;
                cardOrAccount = card.CardNumber;  // if the cardnumber is trimmed from EFT service
                cardTypeId = card.CardTypeID;

                eftAuthCode = card.EFTInfo.AuthCode;
                batchId = Convert.ToString(card.EFTInfo.BatchCode);

                statement.AddField("DRIVERID", ((CorporateCardTenderLineItem)tenderItem).DriverId);
                statement.AddField("VEHICLEID", ((CorporateCardTenderLineItem)tenderItem).VehicleId);
                statement.AddField("ODOMETERREADING", ((CorporateCardTenderLineItem)tenderItem).OdometerReading, SqlDbType.Int);
            }
            else if ((tenderItem.GetType() == typeof(CustomerTenderLineItem)))
            {
                var customer = (CustomerTenderLineItem)tenderItem;
                cardOrAccount = customer.CustomerId;

                if (!string.IsNullOrEmpty(customer.ManualAuthCode))
                {
                    statement.AddField("AUTHENTICATIONCODE", customer.ManualAuthCode);
                }
            }
            else if ((tenderItem.GetType() == typeof(CreditMemoTenderLineItem)))
            {
                statement.AddField("CREDITVOUCHERID", ((CreditMemoTenderLineItem)tenderItem).SerialNumber);
            }
            else if ((tenderItem.GetType() == typeof(GiftCertificateTenderLineItem)))
            {
                statement.AddField("GIFTCARDID", ((GiftCertificateTenderLineItem)tenderItem).SerialNumber);
            }
            else if ((tenderItem.GetType() == typeof(LoyaltyTenderLineItem)))
            {
                statement.AddField("LOYALTYCARDID", ((LoyaltyTenderLineItem)tenderItem).CardNumber);
                statement.AddField("LOYALTYPOINTS", ((LoyaltyTenderLineItem)tenderItem).Points, SqlDbType.Decimal);
            }
            else if (tenderItem is ChequeTenderLineItem)
            {
                statement.AddField("CHECKID", ((ChequeTenderLineItem)tenderItem).CheckID);
            }

            statement.AddField("CARDTYPEID", cardTypeId);
            statement.AddField("CARDORACCOUNT", cardOrAccount);
            statement.AddField("BATCHID", batchId);
            statement.AddField("EFTAUTHENTICATIONCODE", eftAuthCode);
            statement.AddField("COMMENT", tenderItem.Comment);
            statement.AddField("DESCRIPTION", tenderItem.Description);
            statement.AddField("TENDERLINETYPE", (int)tenderItem.TypeOfTender, SqlDbType.TinyInt);


            entry.Connection.ExecuteStatement(statement);

            //Go through all the infocode lines on the tender item and save them to the database                
            foreach (InfoCodeLineItem infocodeLine in tenderItem.InfoCodeLines)
            {
                TransactionProviders.InfocodeTransactionData.Insert(entry, infocodeLine, transaction, tenderItem.LineId);
            }

            if (tenderItem.LoyaltyPoints.PointsAdded)
            {
                TransactionProviders.LoyaltyTransactionData.Insert(entry, transaction, tenderItem);
            }
        }
    }
}
