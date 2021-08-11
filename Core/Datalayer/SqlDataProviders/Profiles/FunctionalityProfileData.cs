using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    public class FunctionalityProfileData : SqlServerDataProviderBase, IFunctionalityProfileData
    {
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList(entry, "NAME");
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry, string sort)
        {
            return GetList<DataEntity>(entry, "POSFUNCTIONALITYPROFILE", "NAME", "PROFILEID", sort);
        }

        public virtual List<FunctionalityProfile> GetFunctionalityProfileList(IConnectionManager entry, string sort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                @"SELECT PROFILEID, 
                         ISNULL(NAME,'') as NAME,
	                     CAST(CASE WHEN EXISTS (SELECT 1 FROM RBOTERMINALTABLE t WHERE t.FUNCTIONALITYPROFILE = pfp.PROFILEID)
	                     			 OR EXISTS (SELECT 1 FROM RBOSTORETABLE s WHERE s.FUNCTIONALITYPROFILE = pfp.PROFILEID)
	                     	THEN 1
	                     	ELSE 0
	                     END AS BIT) AS PROFILEISUSED
                       FROM POSFUNCTIONALITYPROFILE  pfp
                       WHERE DATAAREAID = @dataAreaId
                       ORDER BY " + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<FunctionalityProfile>(entry, cmd, CommandType.Text,PopulateFunctionalityProfile); 
            }
        }

        private static void PopulateFunctionalityProfile(IDataReader dr, FunctionalityProfile profile)
        {
            profile.ID = (string)dr["PROFILEID"];
            profile.Text = (string)dr["NAME"];
            profile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
        }

        private static void PopulateProfile(IDataReader dr, FunctionalityProfile profile)
        {
            profile.ID = (string)dr["PROFILEID"];
            profile.Text = (string)dr["NAME"];
            profile.AggregateItems = (AggregateItemsModes)dr["AGGREGATEITEMS"];
            profile.AggregatePayments = (byte)dr["AGGREGATEPAYMENTS"] != 0;
            profile.TsCentralTableServer = (byte)dr["TSCENTRALTABLESERVER"] != 0;
            profile.CentralTableServer = (string)dr["CENTRALTABLESERVER"];
            profile.CentralTableServerPort = Convert.ToInt32(String.IsNullOrEmpty((string)dr["CENTRALTABLESERVERPORT"]) ? "0" : (string)dr["CENTRALTABLESERVERPORT"]); // Not sure why this is NVarchar(50) in the database
            profile.TsCustomer = (byte)dr["TSCUSTOMER"] != 0;
            profile.TsStaff = (byte)dr["TSSTAFF"] != 0;
            profile.TsSuspendRetrieveTransactions = (byte)dr["TSSUSPENDRETRIEVETRANSACTIONS"] != 0;
            profile.LogLevel = (FunctionalityProfile.LogTraceLevel)(int)dr["LOGLEVEL"];
            profile.AggregateItemsForPrinting = (byte)dr["AGGREGATEITEMSFORPRINTING"] != 0;
            profile.ShowStaffListAtLogon = (byte)dr["SHOWSTAFFLISTATLOGON"] != 0;
            profile.LimitStaffListToStore = (byte)dr["LIMITSTAFFLISTTOSTORE"] != 0;
            profile.AllowSalesIfDrawerIsOpen = (byte)dr["ALLOWSALESIFDRAWERISOPEN"] != 0;
            profile.AllowItemChangesAfterSplitBill = (byte) dr["ALLOWCHANGESAFTERSPLITBILL"] != 0;
            profile.StaffBarcodeLogon = (byte)dr["STAFFBARCODELOGON"] != 0;
            profile.StaffCardLogon = (byte)dr["STAFFCARDLOGON"] != 0;
            profile.MustKeyInPriceIfZero = (byte)dr["MUSTKEYINPRICEIFZERO"] != 0;
            profile.ClearUserBetweenLogins = AsBool(dr["CLEARUSERBETWEENLOGINS"]);
            profile.DisplayLimitationsTotalsInPOS = AsBool(dr["DISPLAYLIMITATIONSTOTALSINPOS"]);
            profile.IsHospitality = (byte)dr["ISHOSPITALITYPROFILE"] != 0;
            profile.InfocodeStartOfTransaction = (string)dr["STARTOFTRANSACTION"];
            profile.InfocodeEndOfTransaction = (string)dr["ENDOFTRANSACTION"];
            profile.InfocodeTenderDecl = (string)dr["TENDERDECLARATION"];
            profile.InfocodeItemNotFound = (string)dr["ITEMNOTONFILE"];
            profile.InfocodeItemDiscount = (string)dr["MARKDOWN"];
            profile.InfocodeTotalDiscount = (string)dr["DISCOUNTATTOTAL"];
            profile.InfocodePriceOverride = (string)dr["OVERRIDEPRICE"];
            profile.InfocodeReturnItem = (string)dr["NEGATIVESALESLINE"];
            profile.InfocodeReturnTransaction = (string)dr["REFUNDSALE"];
            profile.InfocodeVoidItem = (string)dr["VOIDISPRESSED"];
            profile.InfocodeVoidPayment = (string)dr["VOIDPAYMENT"];
            profile.InfocodeVoidTransaction = (string)dr["VOIDTRANSACTION"];
            profile.InfocodeAddSalesPerson = (string)dr["SALESPERSON"];
            profile.InfocodeOpenDrawer = (string) dr["OPENDRAWER"];
            profile.EntryStartsInDecimals = ((byte)dr["NUMPADENTRYSTARTSINDECIMALS"] != 0);
            profile.DecimalsInNumpad = (int)dr["NUMPADAMOUNTOFDECIMALS"];
            profile.SafeDropUsesDenomination = (byte)dr["SAFEDROPUSESDENOMINATION"] != 0;
            profile.SafeDropRevUsesDenomination = ((byte)dr["SAFEDROPREVUSESDENOMINATION"] != 0);
            profile.BankDropUsesDenomination = (byte)dr["BANKDROPUSESDENOMINATION"] != 0;
            profile.BankDropRevUsesDenomination = (byte)dr["BANKDROPREVUSESDENOMINATION"] != 0;
            profile.TenderDeclUsesDenomination = (byte)dr["TENDERDECLUSESDENOMINATION"] != 0;
            profile.CustomerRequiredOnReturn = AsBool(dr["CUSTOMERREQUIREDONRETURN"]);
            profile.KeepDailyJournalOpenAfterPrintingReceipt = AsBool(dr["KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT"]);
            profile.PollingInterval = (int)dr["POLLINGINTERVAL"];
            profile.MaximumQTY = (decimal)dr["MAXIMUMQTY"];
            profile.MaximumPrice = (decimal)dr["MAXIMUMPRICE"];
            profile.SyncTransactions = (byte)dr["SYNCTRANSACTIONS"] != 0;
            profile.PriceDecimalPlaces = (string)dr["PRICEDECIMALPLACES"];
            profile.SkipHospitalityTableView = (byte)dr["SKIPHOSPITALITYTABLEVIEW"] != 0;
            profile.DisplayVoidedItems = (byte)dr["DISPLAYVOIDEDITEMS"] != 0;
            profile.DisplayVoidedPayments = (bool)dr["DISPLAYVOIDEDPAYMENTS"];
            profile.AllowImageViewInItemLookup = AsBool(dr["ALLOWIMAGEVIEWINITEMLOOKUP"]);
            profile.RememberListImageSelection = AsBool(dr["REMEMBERLISTIMAGESELECTION"]);
            profile.DefaultItemImage = AsImage(dr["DEFAULTITEMIMAGE"]);
            profile.POSSettingsClear = (FunctionalityProfile.SettingsClear)(int) dr["CLEARSETTING"];
            profile.SettingsClearGracePeriod = (int) dr["CLEARGRACEPERIOD"];
            profile.AllowSaleAndReturnInSameTransaction = AsBool(dr["ALLOWSALEANDRETURNINSAMETX"]);
            profile.PostTransactionDDJob = (string) dr["POSTTRANSACTIONDDJOB"];
            profile.DDSchedulerLocation = (string) dr["DDSCHEDULERLOCATION"];
            profile.UseStartOfDay = (bool) dr["UseStartOfDay"];
            profile.SalesPersonPrompt = (SalesPersonPrompt)(int)dr["SALESPERSONPROMPT"];
            profile.ZReportConfig.IncludeFloatInCashSummary = (byte)dr["ZRPTINCLUDEFLOATINCASH"] != 0;
            profile.ZReportConfig.CombineGrandTotalSalesandReturns = (byte)dr["ZRPTCOMBINESALESRETURNS"] != 0;
            profile.ZReportConfig.IncludeTenderDeclaration = (byte)dr["ZRPTINCLUDETENDERDECLARATION"] != 0;
            profile.ZReportConfig.DisplayReturnInfo = (byte)dr["ZRPTDISPLAYRETURNINFO"] != 0;
            profile.ZReportConfig.DisplaySuspendedInfo = (byte)dr["ZRPTDISPLAYSUSPENDED"] != 0;
            profile.ZReportConfig.DisplayOtherInfoSection = (byte)dr["ZRPTDISPLAYOTHERINFO"] != 0;
            profile.ZReportConfig.GrandTotalAmountDisplay = (GrandTotalAmtDisplay)(int)dr["ZRPTGRANDTOTALAMTDISPLAY"];
            profile.ZReportConfig.SalesReportAmountDisplay = (SalesReportAmtdisplay)(int)dr["ZRPTSALESRPTAMTDISPLAY"];
            profile.ZReportConfig.CombineSaleAndReturnXZReport = (byte)dr["ZRPTCOMBINESALESREPORTSALESRETURNS"] != 0;
            profile.ZReportConfig.DisplayDepositInfo = (byte)dr["ZRPTDISPLAYDEPOSITINFO"] != 0;
            profile.ZReportConfig.OrderByDepositInfo = (DepositOrderBy)(int)dr["ZRPTDEPOSITORDERBY"];
            profile.ZReportConfig.ReportWidth = (int)dr["ZRPTREPORTWIDTH"];
            profile.ZReportConfig.DisplayOverShortAmount = (bool)dr["ZRPTDISPLAYOVERSHORTAMOUNT"];
            profile.ZReportConfig.PrintGrandTotals = (bool)dr["ZRPTPRINTGRANDTOTALS"];
            profile.ZReportConfig.ShowIndividualDeposits = (bool)dr["ZRPTSHOWINDIVIDUALDEPOSITS"];
            profile.InfocodeStartOfTransactionType = (FunctionalityProfile.StartOfTransactionTypes)(int) dr["STARTOFTRANSACTIONINFOCODETYPE"];
            profile.DialogLimitationDisplayType = (FunctionalityProfile.LimitationDisplayType)(int) dr["LIMITATIONDISPLAYTYPE"];
            profile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
            profile.ShowPricesByDefault = (bool)dr["SHOWPRICESBYDEFAULT"];
            profile.DisplayItemIDInReturnDialog = (bool)dr["DISPLAYITEMIDINRETURNDIALOG"];
            
            //Omni functionality profile
            profile.OmniProfile.MainMenu = (string)dr["OMNIMAINMENU"];
            profile.OmniProfile.EnteringType = (EnteringTypeEnum)dr["OMNIENTERINGTYPE"];
            profile.OmniProfile.QuantityMethod = (QuantityMethodEnum)dr["OMNIQUANTITYMETHOD"];
            profile.OmniProfile.DefaultQuantity = (decimal)dr["OMNIDEFAULTQUANTITY"];
            profile.OmniProfile.SuspensionType = (string) dr["OMNISUSPENSIONTYPE"];
            profile.OmniProfile.PrintingStation = (string) dr["OMNIPRINTINGSTATIONID"];
            profile.OmniProfile.ItemImageLookupGroup = (Guid) dr["OMNIITEMIMAGELOOKUPGROUP"];
            profile.OmniProfile.AllowOfflineTransaction = AsBool(dr["OMNIALLOWOFFLINETRANS"]);
            profile.OmniProfile.ShowMobileInventory = AsBool(dr["OMNISHOWMINVENTORY"]);
        }

        public virtual FunctionalityProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                       @"SELECT PROFILEID, ISNULL(NAME,'') as NAME,ISNULL(AGGREGATEITEMS,0) as AGGREGATEITEMS,
                           ISNULL(AGGREGATEPAYMENTS,0) as AGGREGATEPAYMENTS,ISNULL(TSCENTRALTABLESERVER,0) as TSCENTRALTABLESERVER,
                           ISNULL(CENTRALTABLESERVER,'') as CENTRALTABLESERVER,ISNULL(CENTRALTABLESERVERPORT,0) as CENTRALTABLESERVERPORT,
                           ISNULL(TSCUSTOMER,0) as TSCUSTOMER,ISNULL(TSSTAFF,0) as TSSTAFF,ISNULL(TSSUSPENDRETRIEVETRANSACTIONS,0) as TSSUSPENDRETRIEVETRANSACTIONS,
                           ISNULL(LOGLEVEL,0) as LOGLEVEL,ISNULL(AGGREGATEITEMSFORPRINTING,0) as AGGREGATEITEMSFORPRINTING,
                           ISNULL(SHOWSTAFFLISTATLOGON,0) as SHOWSTAFFLISTATLOGON,ISNULL(LIMITSTAFFLISTTOSTORE,0) as LIMITSTAFFLISTTOSTORE,
                           ISNULL(ALLOWSALESIFDRAWERISOPEN,0) as ALLOWSALESIFDRAWERISOPEN, 
                           ISNULL(ALLOWCHANGESAFTERSPLITBILL,0) as ALLOWCHANGESAFTERSPLITBILL, 
                           ISNULL(STAFFBARCODELOGON,0) as STAFFBARCODELOGON,
                           ISNULL(STAFFCARDLOGON,0) as STAFFCARDLOGON, 
                           ISNULL(MUSTKEYINPRICEIFZERO,0) as MUSTKEYINPRICEIFZERO, 
                           ISNULL(CLEARUSERBETWEENLOGINS,0) as CLEARUSERBETWEENLOGINS,
                           ISNULL(DISPLAYLIMITATIONSTOTALSINPOS,0) as DISPLAYLIMITATIONSTOTALSINPOS,
                           ISNULL(ISHOSPITALITYPROFILE,0) as ISHOSPITALITYPROFILE,
                           ISNULL(SKIPHOSPITALITYTABLEVIEW,0) as SKIPHOSPITALITYTABLEVIEW, 
                           ISNULL(STARTOFTRANSACTION,'') as STARTOFTRANSACTION, 
                           ISNULL(ENDOFTRANSACTION,'') as ENDOFTRANSACTION, 
                           ISNULL(NEGATIVEADJUSTMENT,'') as NEGATIVEADJUSTMENT, 
                           ISNULL(VOIDISPRESSED,'') as VOIDISPRESSED, 
                           ISNULL(VOIDTRANSACTION,'') as VOIDTRANSACTION, 
                           ISNULL(VOIDPAYMENT,'') as VOIDPAYMENT, 
                           ISNULL(REFUNDSALE,'') as REFUNDSALE, 
                           ISNULL(MARKDOWN,'') as MARKDOWN, 
                           ISNULL(MARKUP,'') as MARKUP, 
                           ISNULL(OVERRIDEPRICE,'') as OVERRIDEPRICE, 
                           ISNULL(DISCOUNTATTOTAL,'') as DISCOUNTATTOTAL, 
                           ISNULL(TENDERDECLARATION,'') as TENDERDECLARATION, 
                           ISNULL(SERIALNUMBER,'') as SERIALNUMBER, 
                           ISNULL(SALESPERSON,'') as SALESPERSON,
                           ISNULL(OPENDRAWER,'') as OPENDRAWER, 
                           ISNULL(ITEMNOTONFILE,'') as ITEMNOTONFILE, 
                           ISNULL(NEGATIVESALESLINE,'') as NEGATIVESALESLINE, 
                           ISNULL(NUMPADENTRYSTARTSINDECIMALS, 0) as NUMPADENTRYSTARTSINDECIMALS, 
                           ISNULL(SAFEDROPUSESDENOMINATION, 0) as SAFEDROPUSESDENOMINATION, 
                           ISNULL(SAFEDROPREVUSESDENOMINATION, 0) as SAFEDROPREVUSESDENOMINATION, 
                           ISNULL(BANKDROPUSESDENOMINATION, 0) as BANKDROPUSESDENOMINATION, 
                           ISNULL(BANKDROPREVUSESDENOMINATION, 0) as BANKDROPREVUSESDENOMINATION, 
                           ISNULL(TENDERDECLUSESDENOMINATION, 0) as TENDERDECLUSESDENOMINATION, 
                           ISNULL(NUMPADAMOUNTOFDECIMALS, 2) as NUMPADAMOUNTOFDECIMALS, 
                           ISNULL(MAXIMUMQTY, 0) AS  MAXIMUMQTY, 
                           ISNULL(MAXIMUMPRICE, 0) AS  MAXIMUMPRICE, 
                           ISNULL(POLLINGINTERVAL, 0) AS POLLINGINTERVAL, 
                           ISNULL(SYNCTRANSACTIONS, 0) AS SYNCTRANSACTIONS, 
                           ISNULL(PRICEDECIMALPLACES,'') AS PRICEDECIMALPLACES, 
                           ISNULL(DISPLAYVOIDEDITEMS, 1) AS DISPLAYVOIDEDITEMS, 
                           ISNULL(DISPLAYVOIDEDPAYMENTS, 1) AS DISPLAYVOIDEDPAYMENTS, 
                           ISNULL(ALLOWIMAGEVIEWINITEMLOOKUP, 0) AS ALLOWIMAGEVIEWINITEMLOOKUP, 
                           ISNULL(REMEMBERLISTIMAGESELECTION, 0) AS REMEMBERLISTIMAGESELECTION, 
                           DEFAULTITEMIMAGE, 
                           ISNULL(SKIPHOSPITALITYTABLEVIEW, 0) as SKIPHOSPITALITYTABLEVIEW, 
                           ISNULL(CLEARSETTING, 1) AS CLEARSETTING, 
                           ISNULL(CLEARGRACEPERIOD, 60) AS CLEARGRACEPERIOD, 
                           ISNULL(ALLOWSALEANDRETURNINSAMETX, 1) AS ALLOWSALEANDRETURNINSAMETX, 
                           ISNULL(POSTTRANSACTIONDDJOB, '') AS POSTTRANSACTIONDDJOB, 
                           ISNULL(DDSCHEDULERLOCATION, '') AS DDSCHEDULERLOCATION, 
                           ISNULL(USESTARTOFDAY, 0) AS USESTARTOFDAY, 
                           SALESPERSONPROMPT,
                           ISNULL(ZRPTINCLUDEFLOATINCASH, 1) AS ZRPTINCLUDEFLOATINCASH,
                           ISNULL(ZRPTCOMBINESALESRETURNS, 1) AS ZRPTCOMBINESALESRETURNS,
                           ISNULL(ZRPTINCLUDETENDERDECLARATION, 0) AS ZRPTINCLUDETENDERDECLARATION,
                           ISNULL(ZRPTDISPLAYRETURNINFO, 1) AS ZRPTDISPLAYRETURNINFO, 
                           ISNULL(ZRPTDISPLAYSUSPENDED, 1) AS ZRPTDISPLAYSUSPENDED, 
                           ISNULL(ZRPTDISPLAYOTHERINFO, 1) AS ZRPTDISPLAYOTHERINFO, 
                           ISNULL(ZRPTGRANDTOTALAMTDISPLAY, 0) AS ZRPTGRANDTOTALAMTDISPLAY, 
                           ISNULL(ZRPTCOMBINESALESREPORTSALESRETURNS, 1) AS ZRPTCOMBINESALESREPORTSALESRETURNS, 
                           ISNULL(ZRPTSALESRPTAMTDISPLAY, 0) AS ZRPTSALESRPTAMTDISPLAY,
                           ISNULL(ZRPTDISPLAYDEPOSITINFO, 0) AS ZRPTDISPLAYDEPOSITINFO,
                           ISNULL(ZRPTDEPOSITORDERBY, 0) AS ZRPTDEPOSITORDERBY,
                           ISNULL(ZRPTREPORTWIDTH, 55) AS ZRPTREPORTWIDTH,
                           ISNULL(ZRPTDISPLAYOVERSHORTAMOUNT, 0) AS ZRPTDISPLAYOVERSHORTAMOUNT,
                           ISNULL(ZRPTPRINTGRANDTOTALS, 1) AS ZRPTPRINTGRANDTOTALS,
                           ISNULL(ZRPTSHOWINDIVIDUALDEPOSITS, 1) AS ZRPTSHOWINDIVIDUALDEPOSITS,
                           ISNULL(STARTOFTRANSACTIONINFOCODETYPE, 0) AS STARTOFTRANSACTIONINFOCODETYPE,
                           ISNULL(LIMITATIONDISPLAYTYPE, 0) AS LIMITATIONDISPLAYTYPE,
                           ISNULL(CUSTOMERREQUIREDONRETURN, 0) as CUSTOMERREQUIREDONRETURN,
                           ISNULL(KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT, 0) as KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT,
                           CAST(CASE WHEN EXISTS (SELECT 1 FROM RBOTERMINALTABLE T WHERE T.FUNCTIONALITYPROFILE = F.PROFILEID)
	                     	           OR EXISTS (SELECT 1 FROM RBOSTORETABLE S WHERE S.FUNCTIONALITYPROFILE = F.PROFILEID)
	                     	    THEN 1
	                     	    ELSE 0
	                       END AS BIT) AS PROFILEISUSED,
                           ISNULL(SHOWPRICESBYDEFAULT, 0) AS SHOWPRICESBYDEFAULT,
                           ISNULL(DISPLAYITEMIDINRETURNDIALOG, 0) AS DISPLAYITEMIDINRETURNDIALOG,
                           ISNULL(OMNIMAINMENU, '') as OMNIMAINMENU,
                           ISNULL(OMNIENTERINGTYPE, 0) as OMNIENTERINGTYPE,
                           ISNULL(OMNIQUANTITYMETHOD, 0) as OMNIQUANTITYMETHOD,
                           ISNULL(OMNIDEFAULTQUANTITY, 1) as OMNIDEFAULTQUANTITY,
                           ISNULL(OMNISUSPENSIONTYPE, '') AS OMNISUSPENSIONTYPE,
                           ISNULL(OMNIPRINTINGSTATIONID, '') AS OMNIPRINTINGSTATIONID,
                           ISNULL(OMNIITEMIMAGELOOKUPGROUP, '00000000-0000-0000-0000-000000000000') as OMNIITEMIMAGELOOKUPGROUP,
                           OMNIALLOWOFFLINETRANS,
                           OMNISHOWMINVENTORY
                       FROM POSFUNCTIONALITYPROFILE F
                       WHERE DATAAREAID = @dataAreaId AND PROFILEID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                return Get<FunctionalityProfile>(entry, cmd, id, PopulateProfile, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<FunctionalityProfile>(entry, "POSFUNCTIONALITYPROFILE", "PROFILEID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<FunctionalityProfile>(entry, "POSFUNCTIONALITYPROFILE", "PROFILEID", id, Permission.FunctionalProfileEdit);
        }

        public virtual void SetDisplayItemIDInReturnDialog(IConnectionManager entry, FunctionalityProfile profile, bool DisplayItemIDInReturnDialog)
        {
            var statement = new SqlServerStatement("POSFUNCTIONALITYPROFILE");

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("PROFILEID", (string)profile.ID);

            statement.AddField("DISPLAYITEMIDINRETURNDIALOG", DisplayItemIDInReturnDialog, SqlDbType.Bit);

            Save(entry, profile, statement);
        }

        public virtual void Save(IConnectionManager entry, FunctionalityProfile profile)
        {
            var statement = new SqlServerStatement("POSFUNCTIONALITYPROFILE");

            ValidateSecurity(entry, BusinessObjects.Permission.FunctionalProfileEdit);
            profile.Validate();

            bool isNew = false;
            if (profile.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                profile.ID = DataProviderFactory.Instance.GenerateNumber<IFunctionalityProfileData, FunctionalityProfile>(entry);
            }

            if (isNew || !Exists(entry, profile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("PROFILEID", (string)profile.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("PROFILEID", (string)profile.ID);
            }

            statement.AddField("NAME", profile.Text);
            statement.AddField("AGGREGATEITEMS", profile.AggregateItems, SqlDbType.Int);
            statement.AddField("AGGREGATEPAYMENTS", profile.AggregatePayments ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LOGLEVEL", (int)profile.LogLevel, SqlDbType.Int);
            statement.AddField("AGGREGATEITEMSFORPRINTING", profile.AggregateItemsForPrinting ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("SHOWSTAFFLISTATLOGON", profile.ShowStaffListAtLogon ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LIMITSTAFFLISTTOSTORE", profile.LimitStaffListToStore ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWSALESIFDRAWERISOPEN", profile.AllowSalesIfDrawerIsOpen ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWCHANGESAFTERSPLITBILL", profile.AllowItemChangesAfterSplitBill ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("STAFFBARCODELOGON", profile.StaffBarcodeLogon ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("STAFFCARDLOGON", profile.StaffCardLogon ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MUSTKEYINPRICEIFZERO", profile.MustKeyInPriceIfZero ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("CLEARUSERBETWEENLOGINS", profile.ClearUserBetweenLogins ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DISPLAYLIMITATIONSTOTALSINPOS", profile.DisplayLimitationsTotalsInPOS ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ISHOSPITALITYPROFILE", profile.IsHospitality ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("STARTOFTRANSACTION", (string)profile.InfocodeStartOfTransaction);
            statement.AddField("ENDOFTRANSACTION", (string)profile.InfocodeEndOfTransaction);
            statement.AddField("TENDERDECLARATION", (string)profile.InfocodeTenderDecl);
            statement.AddField("ITEMNOTONFILE", (string)profile.InfocodeItemNotFound);
            statement.AddField("MARKDOWN", (string)profile.InfocodeItemDiscount);
            statement.AddField("DISCOUNTATTOTAL", (string)profile.InfocodeTotalDiscount);
            statement.AddField("OVERRIDEPRICE", (string)profile.InfocodePriceOverride);
            statement.AddField("NEGATIVESALESLINE", (string)profile.InfocodeReturnItem);
            statement.AddField("REFUNDSALE", (string)profile.InfocodeReturnTransaction);
            statement.AddField("VOIDISPRESSED", (string)profile.InfocodeVoidItem);
            statement.AddField("VOIDPAYMENT", (string)profile.InfocodeVoidPayment);
            statement.AddField("VOIDTRANSACTION", (string)profile.InfocodeVoidTransaction);
            statement.AddField("SALESPERSON", (string)profile.InfocodeAddSalesPerson);
            statement.AddField("OPENDRAWER", (string)profile.InfocodeOpenDrawer);

            statement.AddField("NUMPADENTRYSTARTSINDECIMALS", profile.EntryStartsInDecimals ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("NUMPADAMOUNTOFDECIMALS", profile.DecimalsInNumpad, SqlDbType.Int);

            statement.AddField("SAFEDROPUSESDENOMINATION", profile.SafeDropUsesDenomination ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("SAFEDROPREVUSESDENOMINATION", profile.SafeDropRevUsesDenomination ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("BANKDROPUSESDENOMINATION", profile.BankDropUsesDenomination ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("BANKDROPREVUSESDENOMINATION", profile.BankDropRevUsesDenomination ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("CUSTOMERREQUIREDONRETURN", profile.CustomerRequiredOnReturn, SqlDbType.Bit);
            statement.AddField("KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT", profile.KeepDailyJournalOpenAfterPrintingReceipt, SqlDbType.Bit);
            statement.AddField("TENDERDECLUSESDENOMINATION", profile.TenderDeclUsesDenomination ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MAXIMUMQTY", profile.MaximumQTY, SqlDbType.Decimal);
            statement.AddField("MAXIMUMPRICE", profile.MaximumPrice, SqlDbType.Decimal);
            statement.AddField("POLLINGINTERVAL", profile.PollingInterval, SqlDbType.Int);
            statement.AddField("PRICEDECIMALPLACES", profile.PriceDecimalPlaces);
            statement.AddField("SYNCTRANSACTIONS", profile.SyncTransactions, SqlDbType.TinyInt);
            statement.AddField("SKIPHOSPITALITYTABLEVIEW", profile.SkipHospitalityTableView ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DISPLAYVOIDEDITEMS", profile.DisplayVoidedItems ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DISPLAYVOIDEDPAYMENTS", profile.DisplayVoidedPayments, SqlDbType.Bit);
            statement.AddField("ALLOWIMAGEVIEWINITEMLOOKUP", profile.AllowImageViewInItemLookup ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("REMEMBERLISTIMAGESELECTION", profile.RememberListImageSelection ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DEFAULTITEMIMAGE", FromImage(profile.DefaultItemImage), SqlDbType.VarBinary);
            statement.AddField("CLEARSETTING", profile.POSSettingsClear, SqlDbType.Int);
            statement.AddField("CLEARGRACEPERIOD", (int)profile.POSSettingsClear, SqlDbType.Int);
            statement.AddField("ALLOWSALEANDRETURNINSAMETX", profile.AllowSaleAndReturnInSameTransaction ? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("POSTTRANSACTIONDDJOB", profile.PostTransactionDDJob);
            statement.AddField("DDSCHEDULERLOCATION", profile.DDSchedulerLocation);
            statement.AddField("USESTARTOFDAY", profile.UseStartOfDay, SqlDbType.Bit);
            statement.AddField("SALESPERSONPROMPT", (int)profile.SalesPersonPrompt, SqlDbType.Int);
            statement.AddField("ZRPTINCLUDEFLOATINCASH", profile.ZReportConfig.IncludeFloatInCashSummary ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ZRPTCOMBINESALESRETURNS", profile.ZReportConfig.CombineGrandTotalSalesandReturns ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ZRPTINCLUDETENDERDECLARATION", profile.ZReportConfig.IncludeTenderDeclaration ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ZRPTDISPLAYRETURNINFO", profile.ZReportConfig.DisplayReturnInfo ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ZRPTDISPLAYSUSPENDED", profile.ZReportConfig.DisplaySuspendedInfo ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ZRPTDISPLAYOTHERINFO", profile.ZReportConfig.DisplayOtherInfoSection ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ZRPTGRANDTOTALAMTDISPLAY", (int)profile.ZReportConfig.GrandTotalAmountDisplay, SqlDbType.Int);
            statement.AddField("ZRPTSALESRPTAMTDISPLAY", (int)profile.ZReportConfig.SalesReportAmountDisplay, SqlDbType.Int);
            statement.AddField("ZRPTCOMBINESALESREPORTSALESRETURNS", profile.ZReportConfig.CombineSaleAndReturnXZReport ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ZRPTDISPLAYDEPOSITINFO", profile.ZReportConfig.DisplayDepositInfo ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ZRPTDEPOSITORDERBY", (int)profile.ZReportConfig.OrderByDepositInfo, SqlDbType.Int);
            statement.AddField("ZRPTREPORTWIDTH", profile.ZReportConfig.ReportWidth, SqlDbType.Int);
            statement.AddField("ZRPTDISPLAYOVERSHORTAMOUNT", profile.ZReportConfig.DisplayOverShortAmount, SqlDbType.Bit);
            statement.AddField("ZRPTPRINTGRANDTOTALS", profile.ZReportConfig.PrintGrandTotals, SqlDbType.Bit);
            statement.AddField("ZRPTSHOWINDIVIDUALDEPOSITS", profile.ZReportConfig.ShowIndividualDeposits, SqlDbType.Bit);
            statement.AddField("STARTOFTRANSACTIONINFOCODETYPE", (int)profile.InfocodeStartOfTransactionType, SqlDbType.Int);
            statement.AddField("LIMITATIONDISPLAYTYPE", (int)profile.DialogLimitationDisplayType, SqlDbType.Int);
            statement.AddField("SHOWPRICESBYDEFAULT", profile.ShowPricesByDefault, SqlDbType.Bit);
            statement.AddField("DISPLAYITEMIDINRETURNDIALOG", profile.DisplayItemIDInReturnDialog, SqlDbType.Bit);
            
            statement.AddField("OMNIMAINMENU", (string)profile.OmniProfile.MainMenu);
            statement.AddField("OMNIENTERINGTYPE", (int)profile.OmniProfile.EnteringType, SqlDbType.Int);
            statement.AddField("OMNIQUANTITYMETHOD", (int)profile.OmniProfile.QuantityMethod, SqlDbType.Int);
            statement.AddField("OMNIDEFAULTQUANTITY", profile.OmniProfile.DefaultQuantity, SqlDbType.Decimal);
            statement.AddField("OMNISUSPENSIONTYPE", (string)profile.OmniProfile.SuspensionType);
            statement.AddField("OMNIPRINTINGSTATIONID", (string)profile.OmniProfile.PrintingStation);
            statement.AddField("OMNIITEMIMAGELOOKUPGROUP", (Guid)profile.OmniProfile.ItemImageLookupGroup, SqlDbType.UniqueIdentifier);
            statement.AddField("OMNIALLOWOFFLINETRANS", profile.OmniProfile.AllowOfflineTransaction, SqlDbType.Bit);
            statement.AddField("OMNISHOWMINVENTORY", profile.OmniProfile.ShowMobileInventory, SqlDbType.Bit);

            Save(entry, profile, statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "FUNCTIONALITYPROFILE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSFUNCTIONALITYPROFILE", "PROFILEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
