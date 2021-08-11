using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders.ConfigurationWizard;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ConfigurationWizard
{
    /// <summary>
    /// Data prover class for payments and currency.
    /// </summary>
    public class PaymentsAndCurrencyData : SqlServerDataProviderBase, IPaymentsAndCurrencyData
    {
        /// <summary>
        /// Get tender list of selected template from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns></returns>
        public virtual List<PaymentsAndCurrency> GetTenderList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, TENDERTYPEID 
                                    FROM WIZARDTEMPLATETENDERS 
                                    WHERE ID = @ID
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                return Execute<PaymentsAndCurrency>(entry, cmd, CommandType.Text, null, PopulateTenderItems);
            }
        }

        /// <summary>
        /// Data populator of GetTenderList method
        /// </summary>
        /// <param name="entry">entry to database</param>
        /// <param name="dr">data reader</param>
        /// <param name="payment">object of PaymentsAndCurrency entity</param>
        /// <param name="obj">parameters</param>
        private static void PopulateTenderItems(IConnectionManager entry, IDataReader dr, PaymentsAndCurrency payment, object obj)
        {
            payment.TenderTypeID = Convert.ToString(dr["TENDERTYPEID"]);
            payment.ID = Convert.ToString(dr["ID"]);
        }

        /// <summary>
        /// Get currency list of selected template from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns></returns>
        public virtual List<PaymentsAndCurrency> GetCurrencyList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(CURRENCY, '') as CURRENCY
                                    FROM WIZARDTEMPLATECURRENCY
                                    WHERE ID = @ID
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                return Execute<PaymentsAndCurrency>(entry, cmd, CommandType.Text, null, PopulateCurrencyItems);
            }
        }

        /// <summary>
        /// Data populator of GetCurrencyList method
        /// </summary>
        /// <param name="entry">entry to database</param>
        /// <param name="dr">data reader</param>
        /// <param name="currency">object of PaymentsAndCurrency entity</param>
        /// <param name="obj">parameters</param>
        private static void PopulateCurrencyItems(IConnectionManager entry, IDataReader dr, PaymentsAndCurrency currency, object obj)
        {
            currency.CurrencyCode = Convert.ToString(dr["CURRENCY"]);
            //currency.ID = Convert.ToString(dr["ID"]);
        }

        /// <summary>
        /// Delete a entity declaration from given table with a given id.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <param name="table">table name</param>
        /// <returns></returns>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id, string table)
        {
            DeleteRecord(entry, table, "ID", id, BusinessObjects.Permission.PaymentMethodsEdit);
        }

        /// <summary>
        /// Save a given tender declaration into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="paymentsAndCurrencyList">PaymentsAndCurrency</param>
        public virtual void SaveTenderCurrency(IConnectionManager entry, List<PaymentsAndCurrency> paymentsAndCurrencyList)
        {
            Delete(entry, paymentsAndCurrencyList.First().ID, "WIZARDTEMPLATETENDERS");
            Delete(entry, paymentsAndCurrencyList.First().ID, "WIZARDTEMPLATECURRENCY");

            foreach (var paymentsAndCurrencyItem in paymentsAndCurrencyList)
            {
                if (paymentsAndCurrencyItem.TenderTypeID != null && paymentsAndCurrencyItem.TenderTypeID != RecordIdentifier.Empty)
                {
                    var statement = new SqlServerStatement("WIZARDTEMPLATETENDERS")
                        {
                            StatementType = StatementType.Insert
                        };

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                    statement.AddKey("ID", (string)paymentsAndCurrencyItem.ID);

                    statement.AddField("TENDERTYPEID", (string)paymentsAndCurrencyItem.TenderTypeID, SqlDbType.NVarChar);

                    entry.Connection.ExecuteStatement(statement);
                }

                if (paymentsAndCurrencyItem.CurrencyCode != null && paymentsAndCurrencyItem.CurrencyCode != RecordIdentifier.Empty)
                {
                    var statement = new SqlServerStatement("WIZARDTEMPLATECURRENCY")
                        {
                            StatementType = StatementType.Insert
                        };

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                    statement.AddKey("ID", (string)paymentsAndCurrencyItem.ID);

                    statement.AddField("CURRENCY", (string)paymentsAndCurrencyItem.CurrencyCode, SqlDbType.NVarChar);

                    entry.Connection.ExecuteStatement(statement);
                }
            }
        }

        /// <summary>
        /// Data populator of GetSelectedPaymentMethodList method
        /// </summary>
        /// <param name="dr">data reader</param>
        /// <param name="paymentMethod">object of PaymentMethod entity</param>
        private static void PopulatePaymentType(IDataReader dr, PaymentMethod paymentMethod)
        {
            paymentMethod.ID = (string)dr["TENDERTYPEID"];
            paymentMethod.Text = (string)dr["NAME"];
            paymentMethod.DefaultFunction = (PaymentMethodDefaultFunctionEnum)((int)dr["DEFAULTFUNCTION"]);
        }

        /// <summary>
        /// Get selected payment method list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">payment method ids</param>
        /// <returns>payment method list</returns>
        public virtual List<PaymentMethod> GetSelectedPaymentMethodList(IConnectionManager entry, List<RecordIdentifier> idList)
        {
            string ids = "";
            foreach (var id in idList)
            {
                ids += "'" + id.StringValue + "',";
            }
            ids = ids.Remove(ids.LastIndexOf(','));
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select TENDERTYPEID, ISNULL(NAME,'') as NAME, " +
                                  "ISNULL(DEFAULTFUNCTION, '') as DEFAULTFUNCTION " +
                                  "From RBOTENDERTYPETABLE where DATAAREAID = @DATAAREAID and TENDERTYPEID in (" + ids + ")";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<PaymentMethod>(entry, cmd, CommandType.Text, PopulatePaymentType);
            }
        }

        /// <summary>
        /// Data populator of GetSelectedStorePaymentMethodList method
        /// </summary>
        /// <param name="dr">data reader</param>
        /// <param name="paymentMethod">object of StorePaymentMethod entity</param>
        private static void PopulatePayment(IDataReader dr, StorePaymentMethod paymentMethod)
        {
            paymentMethod.PaymentTypeID = (string)dr["TENDERTYPEID"];
            paymentMethod.StoreID = (string)dr["STOREID"];
            paymentMethod.Text = (string)dr["NAME"];
            paymentMethod.InheritedName = (string)dr["INHERITEDNAME"];
            paymentMethod.ChangeTenderID = (string)dr["ChangeTenderID"];
            paymentMethod.AboveMinimumTenderID = (string)dr["AboveMinimumTenderID"];
            paymentMethod.ChangeTenderName = (string)dr["ChangeTenderName"];
            paymentMethod.AboveMinimumTenderName = (string)dr["AboveMinimumTenderName"];
            paymentMethod.MinimumChangeAmount = (decimal)dr["MinimumChangeAmount"];
            paymentMethod.PosOperation = (int)dr["POSOperation"];
            paymentMethod.Rounding = (decimal)dr["Rounding"];
            paymentMethod.RoundingMethod = (StorePaymentMethod.PaymentRoundMethod)dr["RoundingMethod"];
            paymentMethod.OpenDrawer = ((byte)dr["OPENDRAWER"] != 0);
            paymentMethod.AllowOverTender = ((byte)dr["ALLOWOVERTENDER"] != 0);
            paymentMethod.AllowUnderTender = ((byte)dr["ALLOWUNDERTENDER"] != 0);
            paymentMethod.MaximumOverTenderAmount = (decimal)dr["MAXIMUMOVERTENDERAMOUNT"];
            paymentMethod.UnderTenderAmount = (decimal)dr["UNDERTENDERAMOUNT"];
            paymentMethod.MinimumAmountAllowed = (decimal)dr["MinimumAmountAllowed"];
            paymentMethod.MinimumAmountEntered = (decimal)dr["MinimumAmountEntered"];
            paymentMethod.MaximumAmountAllowed = (decimal)dr["MaximumAmountAllowed"];
            paymentMethod.MaximumAmountEntered = (decimal)dr["MaximumAmountEntered"];
            paymentMethod.CountingRequired = ((byte)dr["COUNTINGREQUIRED"] != 0);
            paymentMethod.AllowFloat = ((byte)dr["ALLOWFLOAT"] != 0);
            paymentMethod.AllowBankDrop = ((byte)dr["ALLOWBANKDROP"] != 0);
            paymentMethod.AllowSafeDrop = ((byte)dr["ALLOWSAFEDROP"] != 0);
            paymentMethod.MaxRecount = (int)dr["MAXRECOUNT"];
            paymentMethod.MaxCountingDifference = (decimal)dr["MAXCOUNTINGDIFFERENCE"];
            paymentMethod.AllowNegativePaymentAmounts = ((byte)dr["ALLOWRETURNNEGATIVE"] == 1);
            paymentMethod.PaymentTypeCanBeVoided = ((byte)dr["ALLOWVOIDING"] == 1);
            paymentMethod.PaymentTypeCanBePartOfSplitPayment = ((byte)dr["ALLOWSPLITPAYMENT"] == 1);
            paymentMethod.DefaultFunction = (PaymentMethodDefaultFunctionEnum)((int)dr["DEFAULTFUNCTION"]);
        }

        /// <summary>
        /// Get selected store payment method list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeAndPaymentTypeID"></param>
        /// <returns>store payment method</returns>
        public virtual StorePaymentMethod GetSelectedStorePaymentMethodList(IConnectionManager entry, RecordIdentifier storeAndPaymentTypeID)
        {
            List<StorePaymentMethod> result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select s.STOREID, s.TENDERTYPEID, ISNULL(t.NAME,ISNULL(s.Name,'')) as INHERITEDNAME, ISNULL(s.NAME,ISNULL(t.Name,'')) as NAME, " +
                        "ISNULL(t2.TENDERTYPEID,'') as ChangeTenderID,ISNULL(t3.TENDERTYPEID,'') as AboveMinimumTenderID," +
                        "ISNULL(t2.Name,'') as ChangeTenderName,ISNULL(t3.NAME,'') as AboveMinimumTenderName," +
                        "ISNULL(s.MinimumChangeAmount,0) as MinimumChangeAmount, ISNULL(s.POSOperation,0) as POSOperation," +
                        "ISNULL(s.Rounding,0) as Rounding, ISNULL(s.RoundingMethod,0) as RoundingMethod," +
                        "ISNULL(s.OPENDRAWER,0) as OPENDRAWER,ISNULL(s.ALLOWOVERTENDER,0) as ALLOWOVERTENDER, ISNULL(s.ALLOWUNDERTENDER,0) as ALLOWUNDERTENDER," +
                        "ISNULL(s.MAXIMUMOVERTENDERAMOUNT,0) as MAXIMUMOVERTENDERAMOUNT,ISNULL(s.UNDERTENDERAMOUNT,0) as UNDERTENDERAMOUNT," +
                        "ISNULL(s.MinimumAmountAllowed,0) as MinimumAmountAllowed,ISNULL(s.MinimumAmountEntered,0) as MinimumAmountEntered," +
                        "ISNULL(s.MaximumAmountAllowed,0) as MaximumAmountAllowed,ISNULL(s.MaximumAmountEntered,0) as MaximumAmountEntered, " +
                        "ISNULL(s.COUNTINGREQUIRED, 1) as COUNTINGREQUIRED, ISNULL(s.ALLOWFLOAT, 0) as ALLOWFLOAT, " +
                        "ISNULL(s.TAKENTOBANK, 0) as ALLOWBANKDROP, ISNULL(s.TAKENTOSAFE, 0) as ALLOWSAFEDROP," +
                        "ISNULL(s.MAXRECOUNT, 0) as MAXRECOUNT, ISNULL(s.MAXCOUNTINGDIFFERENCE, 0) as MAXCOUNTINGDIFFERENCE, " +
                        "ISNULL(s.ALLOWVOIDING, 0) as ALLOWVOIDING, ISNULL(s.ALLOWRETURNNEGATIVE, 0) as ALLOWRETURNNEGATIVE, " +
                        "ISNULL(s.ALLOWSPLITPAYMENT,0) as ALLOWSPLITPAYMENT, ISNULL(t.DEFAULTFUNCTION, 0) as DEFAULTFUNCTION " +
                        "from RBOSTORETENDERTYPETABLE s " +
                        "left outer join RBOTENDERTYPETABLE t on s.TENDERTYPEID = t.TENDERTYPEID and s.DATAAREAID = t.DATAAREAID " +
                        "left outer join RBOTENDERTYPETABLE t2 on s.ChangeTenderID = t2.TENDERTYPEID and s.DATAAREAID = t2.DATAAREAID " +
                        "left outer join RBOTENDERTYPETABLE t3 on s.AboveMinimumTenderID = t3.TENDERTYPEID and s.DATAAREAID = t3.DATAAREAID " +
                        "where s.DATAAREAID = @DATAAREAID and s.STOREID = @STOREID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string)storeAndPaymentTypeID);

                if (storeAndPaymentTypeID.HasSecondaryID)
                {
                    cmd.CommandText += " and s.TENDERTYPEID = @PAYMENTTYPEID";

                    MakeParam(cmd, "PAYMENTTYPEID", (string)storeAndPaymentTypeID.SecondaryID);
                }

                result = Execute<StorePaymentMethod>(entry, cmd, CommandType.Text, PopulatePayment);
                if (result != null && result.Count > 0)
                {
                    return result[0];
                }
                return null;
            }
        }

        /// <summary>
        /// Get selected currency list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="currencyCodeList">currencyId list</param>
        /// <returns>A list of selected currency</returns>
        public virtual List<Currency> GetSelectedCurrencyList(IConnectionManager entry, List<RecordIdentifier> currencyCodeList)
        {
            string ids = "";
            foreach (RecordIdentifier id in currencyCodeList)
            {
                ids += "'" + id.StringValue + "',";
            }
            ids = ids.Remove(ids.LastIndexOf(','));

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT CURRENCYCODE, ";
                cmd.CommandText += "ISNULL(TXT,'') AS TXT, ";
                cmd.CommandText += "ISNULL(ROUNDOFFSALES, 0) as ROUNDOFFSALES, ";
                cmd.CommandText += "ISNULL(ROUNDOFFPURCH, 0) as ROUNDOFFPURCH, ";
                cmd.CommandText += "ISNULL(ROUNDOFFAMOUNT,0.0) as ROUNDOFFAMOUNT, ";
                cmd.CommandText += "ISNULL(ROUNDOFFTYPESALES, 0) as ROUNDOFFTYPESALES, ";
                cmd.CommandText += "ISNULL(ROUNDOFFTYPEAMOUNT, 0) as ROUNDOFFTYPEAMOUNT, ";
                cmd.CommandText += "ISNULL(ROUNDOFFTYPEPURCH, 0) as ROUNDOFFTYPEPURCH, ";
                cmd.CommandText += "ISNULL(SYMBOL,'') AS SYMBOL, ";
                cmd.CommandText += "ISNULL(CURRENCYPREFIX,'') AS CURRENCYPREFIX, ";
                cmd.CommandText += "ISNULL(CURRENCYSUFFIX,'') AS CURRENCYSUFFIX ";
                cmd.CommandText += "FROM CURRENCY ";
                cmd.CommandText += "WHERE DATAAREAID = @DATAAREAID AND CURRENCYCODE in (" + ids + ")";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<Currency>(entry, cmd, CommandType.Text, PopulateCurrencyInfo);
            }
        }

        /// <summary>
        /// Data populator of PopulateCurrencyInfo method
        /// </summary>
        /// <param name="dr">data reader</param>
        /// <param name="currencyInfo">object of Currency entity</param>
        private static void PopulateCurrencyInfo(IDataReader dr, Currency currencyInfo)
        {
            currencyInfo.ID = (string)dr["CURRENCYCODE"];
            currencyInfo.Text = (string)dr["TXT"];
            currencyInfo.Symbol = (string)dr["SYMBOL"];
            currencyInfo.CurrencyPrefix = (string)dr["CURRENCYPREFIX"];
            currencyInfo.CurrencySuffix = (string)dr["CURRENCYSUFFIX"];
            currencyInfo.RoundOffAmount = (decimal)dr["ROUNDOFFAMOUNT"];
            currencyInfo.RoundOffTypeAmount = (int)dr["ROUNDOFFTYPEAMOUNT"];
            currencyInfo.RoundOffPurchase = (decimal)dr["ROUNDOFFPURCH"];
            currencyInfo.RoundOffTypePurchase = (int)dr["ROUNDOFFTYPEPURCH"];
            currencyInfo.RoundOffSales = (decimal)dr["ROUNDOFFSALES"];
            currencyInfo.RoundOffTypeSales = (int)dr["ROUNDOFFTYPESALES"];
        }
    }
}
