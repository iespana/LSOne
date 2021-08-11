using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.SqlDataProviders.StoreManagement
{
    public class StorePaymentMethodData : SqlServerDataProviderBase, IStorePaymentMethodData
    {
        public virtual List<DataEntity> GetList(IConnectionManager entry, RecordIdentifier storeID)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "SELECT T.TENDERTYPEID, ISNULL(T.NAME,'') AS NAME FROM RBOSTORETENDERTYPETABLE T " +
                                  "WHERE T.DATAAREAID = @DATAAREAID AND T.STOREID=@STOREID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string)storeID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "TENDERTYPEID");
            }
        }

        /// <summary>
        /// Gets the list of valid return payment types. This excludes Pay customer account(202), Pay gift card(214), and Pay currency(203).
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The store that the payments types are assigned to</param>
        /// <returns>Valid return payments</returns>
        public virtual List<DataEntity> GetReturnPaymentList(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "SELECT T.TENDERTYPEID, ISNULL(S.NAME,'') AS NAME FROM RBOSTORETENDERTYPETABLE T " +
                                  "LEFT OUTER JOIN RBOTENDERTYPETABLE S ON S.DATAAREAID = T.DATAAREAID AND T.TENDERTYPEID = S.TENDERTYPEID " +
                                  "WHERE T.DATAAREAID = @DATAAREAID AND T.STOREID = @STOREID "+
                                  "AND T.POSOPERATION <> 202 AND T.POSOPERATION <> 203";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string)storeID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "TENDERTYPEID");
            }
        }

        public virtual void CopyPaymentMethodToStore(IConnectionManager entry, StorePaymentMethod paymentMethod, RecordIdentifier storeID)
        {
            ValidateSecurity(entry, Permission.StoreEdit);

            if (paymentMethod.StoreID == storeID) 
                return;

            var oldStoreID = paymentMethod.StoreID;

            paymentMethod.StoreID = storeID;

            Save(entry, paymentMethod, paymentMethod.PaymentTypeID);

            var cardList = Providers.PaymentTypeCardTypesData
                .GetCardListForTenderType(
                    entry,
                    oldStoreID,
                    paymentMethod.PaymentTypeID);

            foreach (StoreCardType card in cardList)
            {
                card.StoreID = storeID;
                Providers.PaymentTypeCardTypesData.Save(entry, card);
            }
        }

        /// <summary>
        /// Gets unused tender types for a store.
        /// If the RecordIdentifier includes a TenderTypeID as secondary identifier then the result will include
        /// that ID.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="storeAndPaymentTypeID"></param>
        /// <returns></returns>
        public virtual List<DataEntity> GetUnusedList(IConnectionManager entry, RecordIdentifier storeAndPaymentTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                if (storeAndPaymentTypeID.SecondaryID != null)
                {
                    cmd.CommandText = "SELECT T.TENDERTYPEID, ISNULL(T.NAME,'') AS NAME " +
                                      "FROM RBOTENDERTYPETABLE T " +
                                      "LEFT OUTER JOIN RBOSTORETENDERTYPETABLE S ON S.DATAAREAID = T.DATAAREAID AND T.TENDERTYPEID = S.TENDERTYPEID AND S.STOREID = @STOREID " +
                                      "WHERE T.DATAAREAID = @DATAAREAID AND (S.STOREID IS NULL OR (T.TENDERTYPEID=@TENDERTYPEID))";

                    MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                    MakeParam(cmd, "STOREID", (string) storeAndPaymentTypeID);
                    MakeParam(cmd, "TENDERTYPEID", (string) storeAndPaymentTypeID.SecondaryID);
                }
                else
                {
                    cmd.CommandText = "SELECT T.TENDERTYPEID, ISNULL(T.NAME,'') AS NAME " +
                                      "FROM RBOTENDERTYPETABLE T " +
                                      "LEFT OUTER JOIN RBOSTORETENDERTYPETABLE S ON S.DATAAREAID = T.DATAAREAID AND T.TENDERTYPEID = S.TENDERTYPEID AND S.STOREID = @STOREID " +
                                      "WHERE t.DATAAREAID = @DATAAREAID AND (s.STOREID IS NULL)";

                    MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                    MakeParam(cmd, "STOREID", (string) storeAndPaymentTypeID);
                }

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "TENDERTYPEID");
            }
        }

        private static void PopulatePayment(IDataReader dr, StorePaymentMethod paymentMethod)
        {
            paymentMethod.PaymentTypeID = (string)dr["TENDERTYPEID"];
            paymentMethod.StoreID = (string)dr["STOREID"];
            paymentMethod.Text = (string)dr["NAME"];
            paymentMethod.InheritedName = (string)dr["INHERITEDNAME"];
            paymentMethod.ChangeTenderID = (string)dr["CHANGETENDERID"];
            paymentMethod.AboveMinimumTenderID = (string)dr["ABOVEMINIMUMTENDERID"];
            paymentMethod.ChangeTenderName = (string)dr["CHANGETENDERNAME"];
            paymentMethod.AboveMinimumTenderName = (string)dr["ABOVEMINIMUMTENDERNAME"];
            paymentMethod.MinimumChangeAmount = (decimal)dr["MINIMUMCHANGEAMOUNT"];
            paymentMethod.PosOperation = (int)dr["POSOPERATION"];
            paymentMethod.Rounding = (decimal)dr["ROUNDING"] == 0M ? 0.01M : (decimal)dr["ROUNDING"];
            paymentMethod.RoundingMethod = (StorePaymentMethod.PaymentRoundMethod) dr["ROUNDINGMETHOD"];
            paymentMethod.OpenDrawer = (byte)dr["OPENDRAWER"] != 0;
            paymentMethod.AllowOverTender = (byte)dr["ALLOWOVERTENDER"] != 0;
            paymentMethod.AllowUnderTender = (byte)dr["ALLOWUNDERTENDER"] != 0;
            paymentMethod.MaximumOverTenderAmount = (decimal)dr["MAXIMUMOVERTENDERAMOUNT"];
            paymentMethod.UnderTenderAmount = (decimal)dr["UNDERTENDERAMOUNT"];
            paymentMethod.MinimumAmountAllowed = (decimal)dr["MINIMUMAMOUNTALLOWED"];
            paymentMethod.MinimumAmountEntered = (decimal)dr["MINIMUMAMOUNTENTERED"];
            paymentMethod.MaximumAmountAllowed = (decimal)dr["MAXIMUMAMOUNTALLOWED"];
            paymentMethod.MaximumAmountEntered = (decimal)dr["MAXIMUMAMOUNTENTERED"];
            paymentMethod.CountingRequired = (byte)dr["COUNTINGREQUIRED"] != 0;
            paymentMethod.AllowFloat = (byte)dr["ALLOWFLOAT"] != 0;
            paymentMethod.AllowBankDrop = (byte)dr["ALLOWBANKDROP"] != 0;
            paymentMethod.AllowSafeDrop = (byte)dr["ALLOWSAFEDROP"] != 0;
            paymentMethod.MaxRecount = (int)dr["MAXRECOUNT"];
            paymentMethod.MaxCountingDifference = (decimal)dr["MAXCOUNTINGDIFFERENCE"];
            paymentMethod.AllowNegativePaymentAmounts = (byte)dr["ALLOWRETURNNEGATIVE"] == 1;
            paymentMethod.PaymentTypeCanBeVoided = (byte)dr["ALLOWVOIDING"] == 1;
            paymentMethod.AnotherMethodIfAmountHigher = (bool)dr["ANOTHERMETHODIFAMOUNTHIGHER"];
            paymentMethod.PaymentTypeCanBePartOfSplitPayment = (byte)dr["ALLOWSPLITPAYMENT"] == 1;
            paymentMethod.DefaultFunction = (PaymentMethodDefaultFunctionEnum)((int)dr["DEFAULTFUNCTION"]);
            paymentMethod.BlindRecountAllowed = (byte) dr["BLINDRECOUNTALLOWED"] == 1;
            paymentMethod.MaximumAmountInPOSEnabled = (byte) dr["MAXIMUMAMOUNTINPOSENABLED"] == 1;
            paymentMethod.MaximumAmountInPOS = (decimal) dr["MAXIMUMAMOUNTINPOS"];
            paymentMethod.ForceRefundToThisPaymentType = (bool)dr["FORCEREFUNDTOTHISPAYMENTTYPE"];
        }

        public List<StorePaymentMethod> GetRecords(IConnectionManager entry, RecordIdentifier storeAndPaymentTypeID, bool useSecondary, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "SELECT S.STOREID, S.TENDERTYPEID, ISNULL(T.NAME,ISNULL(S.NAME,'')) AS INHERITEDNAME, ISNULL(S.NAME,ISNULL(T.NAME,'')) AS NAME, " +
                    "ISNULL(T2.TENDERTYPEID,'') AS CHANGETENDERID,ISNULL(T3.TENDERTYPEID,'') AS ABOVEMINIMUMTENDERID," +
                    "ISNULL(T2.NAME,'') AS CHANGETENDERNAME,ISNULL(T3.NAME,'') AS ABOVEMINIMUMTENDERNAME," +
                    "ISNULL(S.MINIMUMCHANGEAMOUNT,0) AS MINIMUMCHANGEAMOUNT, ISNULL(S.POSOPERATION,0) AS POSOPERATION," +
                    "ISNULL(S.ROUNDING,0.01) AS ROUNDING, ISNULL(S.ROUNDINGMETHOD,0) AS ROUNDINGMETHOD," +
                    "ISNULL(S.OPENDRAWER,0) AS OPENDRAWER,ISNULL(S.ALLOWOVERTENDER,0) AS ALLOWOVERTENDER, ISNULL(S.ALLOWUNDERTENDER,0) AS ALLOWUNDERTENDER," +
                    "ISNULL(S.MAXIMUMOVERTENDERAMOUNT,0) AS MAXIMUMOVERTENDERAMOUNT,ISNULL(S.UNDERTENDERAMOUNT,0) AS UNDERTENDERAMOUNT," +
                    "ISNULL(S.MINIMUMAMOUNTALLOWED,0) AS MINIMUMAMOUNTALLOWED,ISNULL(S.MINIMUMAMOUNTENTERED,0) AS MINIMUMAMOUNTENTERED," +
                    "ISNULL(S.MAXIMUMAMOUNTALLOWED,0) AS MAXIMUMAMOUNTALLOWED,ISNULL(S.MAXIMUMAMOUNTENTERED,0) AS MAXIMUMAMOUNTENTERED, " +
                    "ISNULL(S.COUNTINGREQUIRED, 1) AS COUNTINGREQUIRED, ISNULL(S.ALLOWFLOAT, 0) AS ALLOWFLOAT, " +
                    "ISNULL(S.TAKENTOBANK, 0) AS ALLOWBANKDROP, ISNULL(S.TAKENTOSAFE, 0) AS ALLOWSAFEDROP," +
                    "ISNULL(S.MAXRECOUNT, 0) AS MAXRECOUNT, ISNULL(S.MAXCOUNTINGDIFFERENCE, 0) AS MAXCOUNTINGDIFFERENCE, " +
                    "ISNULL(S.ALLOWVOIDING, 0) AS ALLOWVOIDING, ISNULL(S.ANOTHERMETHODIFAMOUNTHIGHER, 0) AS ANOTHERMETHODIFAMOUNTHIGHER, ISNULL(S.ALLOWRETURNNEGATIVE, 0) AS ALLOWRETURNNEGATIVE, " +
                    "ISNULL(S.ALLOWSPLITPAYMENT,0) AS ALLOWSPLITPAYMENT, ISNULL(T.DEFAULTFUNCTION, 0) AS DEFAULTFUNCTION, " +
                    "ISNULL(S.BLINDRECOUNTALLOWED,1) AS BLINDRECOUNTALLOWED," +
                    "ISNULL(S.MAXIMUMAMOUNTINPOSENABLED, 0) AS MAXIMUMAMOUNTINPOSENABLED, " +
                    "ISNULL(S.MAXIMUMAMOUNTINPOS, 0) AS MAXIMUMAMOUNTINPOS, "+
                    "ISNULL(S.FORCEREFUNDTOTHISPAYMENTTYPE, 0) AS FORCEREFUNDTOTHISPAYMENTTYPE " +
                    "FROM RBOSTORETENDERTYPETABLE S " +
                    "LEFT OUTER JOIN RBOTENDERTYPETABLE T ON S.TENDERTYPEID = T.TENDERTYPEID AND S.DATAAREAID = T.DATAAREAID " +
                    "LEFT OUTER JOIN RBOTENDERTYPETABLE T2 ON S.CHANGETENDERID = T2.TENDERTYPEID AND S.DATAAREAID = T2.DATAAREAID " +
                    "LEFT OUTER JOIN RBOTENDERTYPETABLE T3 ON S.ABOVEMINIMUMTENDERID = T3.TENDERTYPEID AND S.DATAAREAID = T3.DATAAREAID " +
                    "WHERE S.DATAAREAID = @DATAAREAID AND S.STOREID = @STOREID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string) storeAndPaymentTypeID);

                if (useSecondary && storeAndPaymentTypeID.HasSecondaryID)
                {
                    cmd.CommandText += " AND S.TENDERTYPEID = @PAYMENTTYPEID";

                    MakeParam(cmd, "PAYMENTTYPEID", (string) storeAndPaymentTypeID.SecondaryID);
                }
                
                List<StorePaymentMethod> result = GetList<StorePaymentMethod>(entry, cmd, new RecordIdentifier("StorePaymentMethod", storeAndPaymentTypeID), PopulatePayment, cacheType);

                if (useSecondary && storeAndPaymentTypeID.HasSecondaryID)
                {
                    PopulatePaymentLimitations(entry, result, storeAndPaymentTypeID, storeAndPaymentTypeID.SecondaryID, cacheType);
                }

                return result;
            }
        }

        protected virtual void PopulatePaymentLimitations(IConnectionManager entry, List<StorePaymentMethod> paymentMethods, RecordIdentifier storeID, RecordIdentifier tenderTypeID, CacheType cacheType)
        {
            foreach (StorePaymentMethod storePaymentMethod in paymentMethods)
            {
                storePaymentMethod.PaymentLimitations = Providers.StorePaymentLimitationData.GetListForStoreTender(entry, storeID, tenderTypeID, cacheType) ?? new List<StorePaymentLimitation>();
            }
        }

        protected virtual void PopulatePaymentLimitations(IConnectionManager entry, StorePaymentMethod paymentMethod, RecordIdentifier storeID, RecordIdentifier tenderTypeID)
        {
            paymentMethod.PaymentLimitations = Providers.StorePaymentLimitationData.GetListForStoreTender(entry, storeID, tenderTypeID) ?? new List<StorePaymentLimitation>();
        }

        public List<StorePaymentMethod> GetTenderForOperation(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier operationID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                int operation = Convert.ToInt32(operationID.StringValue);

                Dictionary<POSOperations, POSOperations> actionsWithInvalidTenderTypesToValidTenderTypesMapper = new Dictionary<POSOperations, POSOperations>
                {
                    [POSOperations.PayCashQuick] = POSOperations.PayCash,   // No tender type has the action PayCashQuick, so it should use the PayCash tender types
                    [POSOperations.IssueCreditMemo] = POSOperations.PayCreditMemo,
                    [POSOperations.IssueGiftCertificate] = POSOperations.PayGiftCertificate,
                    [POSOperations.LoyaltyPointDiscount] = POSOperations.PayLoyalty,
                    [POSOperations.AuthorizeCardQuick] = POSOperations.AuthorizeCard
                };

                POSOperations posOperations = (POSOperations)operation;

                if (actionsWithInvalidTenderTypesToValidTenderTypesMapper.ContainsKey(posOperations))
                {
                    operation = (int)actionsWithInvalidTenderTypesToValidTenderTypesMapper[posOperations];
                }

                cmd.CommandText = @"SELECT S.TENDERTYPEID, ISNULL(S.NAME,ISNULL(T.NAME,'')) AS NAME 
                                  FROM RBOSTORETENDERTYPETABLE S 
                                  LEFT OUTER JOIN RBOTENDERTYPETABLE T ON S.TENDERTYPEID = T.TENDERTYPEID AND S.DATAAREAID = T.DATAAREAID 
                                  WHERE S.DATAAREAID = @DATAAREAID AND S.STOREID = @STOREID AND POSOPERATION = @POSOPERATION
                                  AND T.DEFAULTFUNCTION NOT IN (4, 5) ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string) storeID);
                MakeParam(cmd, "POSOPERATION", operation, SqlDbType.Int);

                return Execute<StorePaymentMethod>(entry, cmd, CommandType.Text, PopulatePartialInfoPayment);
            }
        }

        private static void PopulatePartialInfoPayment(IDataReader dr, StorePaymentMethod paymentMethod)
        {
            paymentMethod.ID = (string)dr["TENDERTYPEID"];
            paymentMethod.PaymentTypeID = (string)dr["TENDERTYPEID"];
            paymentMethod.Text = (string)dr["NAME"];            
        }

        public virtual StorePaymentMethod Get(IConnectionManager entry, RecordIdentifier storeID, PaymentMethodDefaultFunctionEnum defaultFunction)
        {
            RecordIdentifier tenderTypeID = GetTenderForFunction(entry, storeID, defaultFunction);
            return Get(entry, new RecordIdentifier(storeID, tenderTypeID), CacheType.CacheTypeApplicationLifeTime);
        }

        /// <summary>
        /// Gets a specific payment type for a given store or all payment types for a given store.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="storeAndPaymentTypeID">Has to contain store, buy payment type is optional, if payment type is empty then all payment methods for the given store are returned</param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public virtual StorePaymentMethod Get(IConnectionManager entry, RecordIdentifier storeAndPaymentTypeID, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT S.STOREID, S.TENDERTYPEID, ISNULL(T.NAME,ISNULL(S.NAME,'')) AS INHERITEDNAME, ISNULL(S.NAME,ISNULL(T.NAME,'')) AS NAME, 
                        ISNULL(T2.TENDERTYPEID,'') AS CHANGETENDERID,ISNULL(T3.TENDERTYPEID,'') AS ABOVEMINIMUMTENDERID,
                        ISNULL(T2.NAME,'') AS CHANGETENDERNAME,ISNULL(T3.NAME,'') AS ABOVEMINIMUMTENDERNAME,ISNULL(S.MINIMUMCHANGEAMOUNT,0) AS MINIMUMCHANGEAMOUNT, ISNULL(S.POSOPERATION,0) AS POSOPERATION,
                        ISNULL(S.ROUNDING,0.01) AS ROUNDING, ISNULL(S.ROUNDINGMETHOD,0) AS ROUNDINGMETHOD,
                        ISNULL(S.OPENDRAWER,0) AS OPENDRAWER,ISNULL(S.ALLOWOVERTENDER,0) AS ALLOWOVERTENDER,
                        ISNULL(S.ALLOWUNDERTENDER,0) AS ALLOWUNDERTENDER,ISNULL(S.MAXIMUMOVERTENDERAMOUNT,0) AS MAXIMUMOVERTENDERAMOUNT,
                        ISNULL(S.UNDERTENDERAMOUNT,0) AS UNDERTENDERAMOUNT,ISNULL(S.MINIMUMAMOUNTALLOWED,0) AS MINIMUMAMOUNTALLOWED,
                        ISNULL(S.MINIMUMAMOUNTENTERED,0) AS MINIMUMAMOUNTENTERED,ISNULL(S.MAXIMUMAMOUNTALLOWED,0) AS MAXIMUMAMOUNTALLOWED,
                        ISNULL(S.MAXIMUMAMOUNTENTERED,0) AS MAXIMUMAMOUNTENTERED, ISNULL(S.COUNTINGREQUIRED, 1) AS COUNTINGREQUIRED, 
                        ISNULL(S.ALLOWFLOAT, 0) AS ALLOWFLOAT, ISNULL(S.TAKENTOBANK, 0) AS ALLOWBANKDROP, ISNULL(S.TAKENTOSAFE, 0) AS ALLOWSAFEDROP,
                        ISNULL(S.MAXRECOUNT, 0) AS MAXRECOUNT, ISNULL(S.MAXCOUNTINGDIFFERENCE, 0) AS MAXCOUNTINGDIFFERENCE, ISNULL(S.ALLOWVOIDING, 0) AS ALLOWVOIDING,
                        ISNULL(S.ANOTHERMETHODIFAMOUNTHIGHER, 0) AS ANOTHERMETHODIFAMOUNTHIGHER,
                        ISNULL(S.ALLOWRETURNNEGATIVE, 0) AS ALLOWRETURNNEGATIVE, ISNULL(S.ALLOWSPLITPAYMENT,0) AS ALLOWSPLITPAYMENT,
                        ISNULL(T.DEFAULTFUNCTION, 0) AS DEFAULTFUNCTION, ISNULL(S.BLINDRECOUNTALLOWED,1) AS BLINDRECOUNTALLOWED,
                        ISNULL(S.MAXIMUMAMOUNTINPOSENABLED, 0) AS MAXIMUMAMOUNTINPOSENABLED,
                        ISNULL(S.MAXIMUMAMOUNTINPOS, 0) AS MAXIMUMAMOUNTINPOS,
                        ISNULL(S.FORCEREFUNDTOTHISPAYMENTTYPE, 0) AS FORCEREFUNDTOTHISPAYMENTTYPE
                        FROM RBOSTORETENDERTYPETABLE S
                        LEFT OUTER JOIN RBOTENDERTYPETABLE T ON S.TENDERTYPEID = T.TENDERTYPEID AND S.DATAAREAID = T.DATAAREAID 
                        LEFT OUTER JOIN RBOTENDERTYPETABLE T2 ON S.CHANGETENDERID = T2.TENDERTYPEID AND S.DATAAREAID = T2.DATAAREAID 
                        LEFT OUTER JOIN RBOTENDERTYPETABLE T3 ON S.ABOVEMINIMUMTENDERID = T3.TENDERTYPEID AND S.DATAAREAID = T3.DATAAREAID 
                        WHERE S.DATAAREAID = @DATAAREAID AND S.STOREID = @STOREID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string)storeAndPaymentTypeID);

                if (storeAndPaymentTypeID.HasSecondaryID)
                {
                    cmd.CommandText += " AND S.TENDERTYPEID = @PAYMENTTYPEID";

                    MakeParam(cmd, "PAYMENTTYPEID", (string)storeAndPaymentTypeID.SecondaryID);
                }
                StorePaymentMethod storePaymentMethod = Get<StorePaymentMethod>(entry, cmd, new RecordIdentifier("StorePaymentMethod", storeAndPaymentTypeID), PopulatePayment, cache, UsageIntentEnum.Normal);

                if (storePaymentMethod != null && storeAndPaymentTypeID.HasSecondaryID)
                {
                    PopulatePaymentLimitations(entry, storePaymentMethod, storeAndPaymentTypeID, storeAndPaymentTypeID.SecondaryID);
                }

                return storePaymentMethod;
            }
        }

        public virtual StorePaymentMethod Get(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier limitationMasterID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT S.STOREID, S.TENDERTYPEID, ISNULL(T.NAME,ISNULL(S.NAME,'')) AS INHERITEDNAME, ISNULL(S.NAME,ISNULL(T.NAME,'')) AS NAME, 
                        ISNULL(T2.TENDERTYPEID,'') AS CHANGETENDERID,ISNULL(T3.TENDERTYPEID,'') AS ABOVEMINIMUMTENDERID,
                        ISNULL(T2.NAME,'') AS CHANGETENDERNAME,ISNULL(T3.NAME,'') AS ABOVEMINIMUMTENDERNAME,ISNULL(S.MINIMUMCHANGEAMOUNT,0) AS MINIMUMCHANGEAMOUNT, ISNULL(S.POSOPERATION,0) AS POSOPERATION,
                        ISNULL(S.ROUNDING,0.01) AS ROUNDING, ISNULL(S.ROUNDINGMETHOD,0) AS ROUNDINGMETHOD,
                        ISNULL(S.OPENDRAWER,0) AS OPENDRAWER,ISNULL(S.ALLOWOVERTENDER,0) AS ALLOWOVERTENDER,
                        ISNULL(S.ALLOWUNDERTENDER,0) AS ALLOWUNDERTENDER,ISNULL(S.MAXIMUMOVERTENDERAMOUNT,0) AS MAXIMUMOVERTENDERAMOUNT,
                        ISNULL(S.UNDERTENDERAMOUNT,0) AS UNDERTENDERAMOUNT,ISNULL(S.MINIMUMAMOUNTALLOWED,0) AS MINIMUMAMOUNTALLOWED,
                        ISNULL(S.MINIMUMAMOUNTENTERED,0) AS MINIMUMAMOUNTENTERED,ISNULL(S.MAXIMUMAMOUNTALLOWED,0) AS MAXIMUMAMOUNTALLOWED,
                        ISNULL(S.MAXIMUMAMOUNTENTERED,0) AS MAXIMUMAMOUNTENTERED, ISNULL(S.COUNTINGREQUIRED, 1) AS COUNTINGREQUIRED, 
                        ISNULL(S.ALLOWFLOAT, 0) AS ALLOWFLOAT, ISNULL(S.TAKENTOBANK, 0) AS ALLOWBANKDROP, ISNULL(S.TAKENTOSAFE, 0) AS ALLOWSAFEDROP,
                        ISNULL(S.MAXRECOUNT, 0) AS MAXRECOUNT, ISNULL(S.MAXCOUNTINGDIFFERENCE, 0) AS MAXCOUNTINGDIFFERENCE, ISNULL(S.ALLOWVOIDING, 0) AS ALLOWVOIDING,
                        ISNULL(S.ANOTHERMETHODIFAMOUNTHIGHER, 0) AS ANOTHERMETHODIFAMOUNTHIGHER,
                        ISNULL(S.ALLOWRETURNNEGATIVE, 0) AS ALLOWRETURNNEGATIVE, ISNULL(S.ALLOWSPLITPAYMENT,0) AS ALLOWSPLITPAYMENT,
                        ISNULL(T.DEFAULTFUNCTION, 0) AS DEFAULTFUNCTION, ISNULL(S.BLINDRECOUNTALLOWED,1) AS BLINDRECOUNTALLOWED,
                        ISNULL(S.MAXIMUMAMOUNTINPOSENABLED, 0) AS MAXIMUMAMOUNTINPOSENABLED,
                        ISNULL(S.MAXIMUMAMOUNTINPOS, 0) AS MAXIMUMAMOUNTINPOS,
                        ISNULL(S.FORCEREFUNDTOTHISPAYMENTTYPE, 0) AS FORCEREFUNDTOTHISPAYMENTTYPE
                        FROM RBOSTORETENDERTYPETABLE S
                        LEFT OUTER JOIN RBOTENDERTYPETABLE T ON S.TENDERTYPEID = T.TENDERTYPEID AND S.DATAAREAID = T.DATAAREAID 
                        LEFT OUTER JOIN RBOTENDERTYPETABLE T2 ON S.CHANGETENDERID = T2.TENDERTYPEID AND S.DATAAREAID = T2.DATAAREAID 
                        LEFT OUTER JOIN RBOTENDERTYPETABLE T3 ON S.ABOVEMINIMUMTENDERID = T3.TENDERTYPEID AND S.DATAAREAID = T3.DATAAREAID 
                        JOIN RBOSTORETENDERTYPEPAYMENTLIMITATIONTABLE SL ON S.TENDERTYPEID = SL.TENDERTYPEID
                        WHERE S.DATAAREAID = @DATAAREAID AND S.STOREID = @STOREID AND SL.LIMITATIONMASTERID = @LIMITATIONMASTERID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string)storeID);
                MakeParam(cmd, "LIMITATIONMASTERID", (Guid)limitationMasterID, SqlDbType.UniqueIdentifier);

                StorePaymentMethod storePaymentMethod = Get<StorePaymentMethod>(entry, cmd, new RecordIdentifier("StorePaymentMethodLimitation", storeID, limitationMasterID), PopulatePayment, cacheType, UsageIntentEnum.Normal);

                return storePaymentMethod;
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier storeAndTenderIdentifier)
        {
            DeleteRecord(entry, "RBOSTORETENDERTYPETABLE", new[] { "STOREID", "TENDERTYPEID" }, storeAndTenderIdentifier, Permission.StoreEdit);
            DeleteRecord(entry, "RBOSTORETENDERTYPECARDTABLE", new[] { "STOREID", "TENDERTYPEID" }, storeAndTenderIdentifier, Permission.StoreEdit);
        }

        public virtual bool Exists(IConnectionManager entry, StorePaymentMethod paymentMethod, RecordIdentifier originalTenderTypeID)
        {
            var identifier = new RecordIdentifier(paymentMethod.StoreID, originalTenderTypeID);

            return RecordExists(entry, "RBOSTORETENDERTYPETABLE", new[] { "STOREID", "TENDERTYPEID" }, identifier);
        }

        public virtual void Save(IConnectionManager entry, StorePaymentMethod paymentMethod, RecordIdentifier originalTenderTypeID)
        {
            var statement = new SqlServerStatement("RBOSTORETENDERTYPETABLE");

            ValidateSecurity(entry, Permission.StoreEdit);

            if (!Exists(entry, paymentMethod,originalTenderTypeID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STOREID", (string)paymentMethod.StoreID);

                statement.AddKey("TENDERTYPEID", (string)paymentMethod.PaymentTypeID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STOREID", (string)paymentMethod.StoreID);
                statement.AddCondition("TENDERTYPEID", (string)originalTenderTypeID);

                statement.AddField("TENDERTYPEID", (string)paymentMethod.PaymentTypeID);
            }
            
            statement.AddField("NAME", paymentMethod.Text);
            statement.AddField("CHANGETENDERID", (string)paymentMethod.ChangeTenderID);
            statement.AddField("ABOVEMINIMUMTENDERID", (string)paymentMethod.AboveMinimumTenderID);
            statement.AddField("MINIMUMCHANGEAMOUNT", paymentMethod.MinimumChangeAmount , SqlDbType.Decimal);
            statement.AddField("POSOPERATION", Convert.ToInt32(paymentMethod.PosOperation.StringValue), SqlDbType.Int);

            statement.AddField("ROUNDING", paymentMethod.Rounding == 0 ? (decimal) 0.01 : paymentMethod.Rounding, SqlDbType.Decimal);
            statement.AddField("ROUNDINGMETHOD", (int)paymentMethod.RoundingMethod, SqlDbType.Int);
            statement.AddField("OPENDRAWER", paymentMethod.OpenDrawer ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWOVERTENDER", paymentMethod.AllowOverTender, SqlDbType.TinyInt);
            statement.AddField("ALLOWUNDERTENDER", paymentMethod.AllowUnderTender, SqlDbType.TinyInt);
            statement.AddField("MAXIMUMOVERTENDERAMOUNT", paymentMethod.MaximumOverTenderAmount, SqlDbType.Decimal);
            statement.AddField("UNDERTENDERAMOUNT", paymentMethod.UnderTenderAmount, SqlDbType.Decimal);
            statement.AddField("MINIMUMAMOUNTALLOWED", paymentMethod.MinimumAmountAllowed, SqlDbType.Decimal);
            statement.AddField("MINIMUMAMOUNTENTERED", paymentMethod.MinimumAmountEntered, SqlDbType.Decimal);
            statement.AddField("MAXIMUMAMOUNTALLOWED", paymentMethod.MaximumAmountAllowed, SqlDbType.Decimal);
            statement.AddField("MAXIMUMAMOUNTENTERED", paymentMethod.MaximumAmountEntered, SqlDbType.Decimal);
            statement.AddField("COUNTINGREQUIRED", paymentMethod.CountingRequired, SqlDbType.TinyInt);
            statement.AddField("ALLOWFLOAT", paymentMethod.AllowFloat, SqlDbType.TinyInt);
            statement.AddField("TAKENTOBANK", paymentMethod.AllowBankDrop, SqlDbType.TinyInt);
            statement.AddField("TAKENTOSAFE", paymentMethod.AllowSafeDrop, SqlDbType.TinyInt);
            statement.AddField("MAXRECOUNT", paymentMethod.MaxRecount, SqlDbType.Int);
            statement.AddField("MAXCOUNTINGDIFFERENCE", paymentMethod.MaxCountingDifference, SqlDbType.Decimal);
            statement.AddField("ALLOWVOIDING", paymentMethod.PaymentTypeCanBeVoided, SqlDbType.TinyInt);
            statement.AddField("ANOTHERMETHODIFAMOUNTHIGHER", paymentMethod.AnotherMethodIfAmountHigher, SqlDbType.Bit);
            statement.AddField("ALLOWRETURNNEGATIVE", paymentMethod.AllowNegativePaymentAmounts, SqlDbType.TinyInt);
            statement.AddField("ALLOWSPLITPAYMENT", paymentMethod.PaymentTypeCanBePartOfSplitPayment, SqlDbType.TinyInt);
            statement.AddField("FUNCTION_", (int)paymentMethod.DefaultFunction, SqlDbType.TinyInt);
            statement.AddField("BLINDRECOUNTALLOWED", paymentMethod.BlindRecountAllowed, SqlDbType.TinyInt);
            statement.AddField("MAXIMUMAMOUNTINPOSENABLED", paymentMethod.MaximumAmountInPOSEnabled, SqlDbType.TinyInt);
            statement.AddField("MAXIMUMAMOUNTINPOS", paymentMethod.MaximumAmountInPOS, SqlDbType.Decimal);
            statement.AddField("FORCEREFUNDTOTHISPAYMENTTYPE", paymentMethod.ForceRefundToThisPaymentType, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);

            List<StorePaymentLimitation> list = Providers.StorePaymentLimitationData.GetListForStoreTender(entry, paymentMethod.StoreID, paymentMethod.PaymentTypeID);
            foreach (StorePaymentLimitation limitation in list)
            {
                Providers.StorePaymentLimitationData.Delete(entry, limitation.StoreID, limitation.TenderTypeID, limitation.LimitationMasterID);
            }

            if (paymentMethod.PaymentLimitations.Any())
            {
                foreach (StorePaymentLimitation limitation in paymentMethod.PaymentLimitations)
                {
                    Providers.StorePaymentLimitationData.Save(entry, limitation);
                }
            }
        }

        /// <summary>
        /// Gets the first tendertypeID for the given posOperation number.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="posOperation">Pos operation number</param>
        /// <param name="storeID">The store that the tendertype belongs to</param>
        /// <returns>TendertypeID</returns>
        public virtual RecordIdentifier GetTenderType(IConnectionManager entry, RecordIdentifier posOperation, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TENDERTYPEID FROM RBOSTORETENDERTYPETABLE WHERE DATAAREAID = @DATAAREAID AND POSOPERATION = @POSOPERATIONID AND STOREID = @STOREID";
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "POSOPERATIONID", posOperation);
                MakeParam(cmd, "STOREID", storeID);
                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual RecordIdentifier GetTenderForFunction(IConnectionManager entry, RecordIdentifier storeID, PaymentMethodDefaultFunctionEnum paymentMethod)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT S.TENDERTYPEID 
                                    FROM RBOSTORETENDERTYPETABLE S JOIN RBOTENDERTYPETABLE T ON S.TENDERTYPEID = T.TENDERTYPEID AND S.DATAAREAID = T.DATAAREAID
                                    WHERE T.DEFAULTFUNCTION = @FUNCTION 
                                    AND S.STOREID = @STOREID 
                                    AND S.DATAAREAID = @DATAAREAID";
                MakeParam(cmd, "FUNCTION", paymentMethod, SqlDbType.Int);
                MakeParam(cmd, "STOREID", storeID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual RecordIdentifier GetChangeTenderForFunction(IConnectionManager entry, RecordIdentifier storeID, PaymentMethodDefaultFunctionEnum paymentMethod)
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT S.CHANGETENDERID FROM RBOSTORETENDERTYPETABLE S JOIN RBOTENDERTYPETABLE T 
                                    ON S.TENDERTYPEID = T.TENDERTYPEID AND S.DATAAREAID = T.DATAAREAID
                                    WHERE T.DEFAULTFUNCTION = @FUNCTION AND S.STOREID = @STOREID AND S.DATAAREAID = @DATAAREAID";
                MakeParam(cmd, "FUNCTION", paymentMethod, SqlDbType.Int);
                MakeParam(cmd, "STOREID", storeID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual RecordIdentifier GetCashTender(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT TENDERTYPEID FROM RBOSTORETENDERTYPETABLE 
                                    WHERE STOREID = @STOREID AND DATAAREAID = @DATAAREAID 
                                    AND POSOPERATION = '200' AND ALLOWFLOAT = 0 AND FUNCTION_ = 0 
                                    ORDER BY TENDERTYPEID";
                MakeParam(cmd, "STOREID", storeID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                return new RecordIdentifier(entry.Connection.ExecuteScalar(cmd) ?? "-1");
            }
        }

        public virtual bool CountingRequiredForTender(IConnectionManager entry, RecordIdentifier tenderTypeID, RecordIdentifier storeID)
        {
            bool result = true;
            IDataReader dr = null;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT COUNTINGREQUIRED " +
                    "FROM RBOSTORETENDERTYPETABLE " +
                    "WHERE TENDERTYPEID = @TENDERTYPEID AND DATAAREAID = @DATAAREAID " +
                    "AND STOREID = @STOREID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "TENDERTYPEID", (string)tenderTypeID);
                MakeParam(cmd, "STOREID", (string)storeID);

                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        result = (byte)dr["COUNTINGREQUIRED"] == 1;
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }

            return result;
        }

        public virtual bool AnyChangeTenderOfType(IConnectionManager entry, POSOperations operation, RecordIdentifier storeID)
        {
            //Payment operations all have IDs between 200 and 299
            if ((int)operation < 200 && (int)operation >= 300)
            {
                return false;
            }

            bool result = true;
            IDataReader dr = null;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @" SELECT CAST( 
                            CASE WHEN EXISTS(SELECT 1 
                                FROM RBOSTORETENDERTYPETABLE S
                                JOIN RBOTENDERTYPETABLE T ON T.TENDERTYPEID = S.TENDERTYPEID 
                                LEFT JOIN RBOSTORETENDERTYPETABLE S1 ON S.CHANGETENDERID = S1.TENDERTYPEID 
                                LEFT JOIN RBOSTORETENDERTYPETABLE S2 ON S.ABOVEMINIMUMTENDERID = S2.TENDERTYPEID 
                                WHERE T.DEFAULTFUNCTION IN(0, 1, 2, 3) 
                                AND S.STOREID = @STOREID
                                AND (S.CHANGETENDERID <> '' OR S.ABOVEMINIMUMTENDERID <> '') 
                                AND S.DATAAREAID = @DATAAREAID 
                                AND (S1.POSOPERATION = @OPERATION OR S2.POSOPERATION = @OPERATION)) THEN 1 
                            ELSE 0 
                            END
                        AS INT) AS TRUE ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "OPERATION", (int)operation, SqlDbType.Int);
                MakeParam(cmd, "STOREID", (string)storeID);

                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        result = (int)dr["TRUE"] == 1;
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }

            return result;
        }

        public virtual bool GetForceRefundToThisPaymentType(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier limitationMasterID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"  SELECT TOP 1 FORCEREFUNDTOTHISPAYMENTTYPE
                        FROM RBOSTORETENDERTYPETABLE STTT INNER JOIN PAYMENTLIMITATIONS PL ON STTT.TENDERTYPEID = PL.TENDERID
                        WHERE PL.LIMITATIONMASTERID = @LIMITATIONMASTERID AND STTT.STOREID = @STOREID AND STTT.DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string)storeID);
                MakeParam(cmd, "LIMITATIONMASTERID", (Guid)limitationMasterID);

                object result = entry.Connection.ExecuteScalar(cmd);

                return AsBool(result);
            }
        }        
    }
}