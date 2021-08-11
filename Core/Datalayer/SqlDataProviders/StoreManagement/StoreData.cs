using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.StoreManagement.Validity;
using LSOne.DataLayer.SqlConnector.QueryHelpers;

namespace LSOne.DataLayer.SqlDataProviders.StoreManagement
{
    public class StoreData : SqlServerDataProviderBase, IStoreData
    {
        private static string ResolveSort(StoreSorting sort, bool backwards)
        {
            string sortString = "";

            switch (sort)
            {
                case StoreSorting.ID:
                    return backwards ? "ORDER BY LEN(S.STOREID) DESC, S.STOREID DESC" : " ORDER BY LEN(S.STOREID), S.STOREID";
                case StoreSorting.Name:
                    sortString = "S.NAME";
                    break;
                case StoreSorting.City:
                    sortString = "S.CITY";
                    break;
                case StoreSorting.Region:
                    sortString = "RG.DESCRIPTION";
                    break;
                case StoreSorting.DefaultCustomer:
                    sortString = "C.NAME";
                    break;
                case StoreSorting.Currency:
                    sortString = "CR.TXT";
                    break;
                case StoreSorting.FunctionalityProfile:
                    sortString = "FP.NAME";
                    break;
                case StoreSorting.FormProfile:
                    sortString = "RP.DESCRIPTION";
                    break;
                case StoreSorting.SiteServiceProfile:
                    sortString = "TP.NAME";
                    break;
                case StoreSorting.TouchButtons:
                    sortString = "LP.NAME";
                    break;
                case StoreSorting.SalesTaxGroup:
                    sortString = "TGH.TAXGROUPNAME";
                    break;
                case StoreSorting.PriceSetting:
                    sortString = "S.STOREPRICESETTING";
                    break;
                case StoreSorting.Terminals:
                    sortString = "TERMINALCOUNT";
                    break;
                default: return "";
            }

            return "ORDER BY " + sortString + (backwards ? " DESC" : " ASC");
        }

        private static void PopulateStoreValidity(IDataReader dr, StoreValidity validity)
        {
            validity.ID = (string)dr["STOREID"];
            validity.Text = (string)dr["NAME"];

            if (dr["TerminalExists"] is DBNull)
            {
                validity.TerminalExists = false;
            }
            else
            {
                validity.TerminalExists = ((string)dr["TerminalExists"] != "");
            }

            if (dr["FunctionalityProfileExist"] is DBNull)
            {
                validity.FunctionalityProfileExist = false;
            }
            else
            {
                validity.FunctionalityProfileExist = ((string)dr["FunctionalityProfileExist"] != "");
            }

            if (dr["ButtonLayoutExists"] is DBNull)
            {
                validity.ButtonLayoutExists = false;
            }
            else
            {
                validity.ButtonLayoutExists = ((string)dr["ButtonLayoutExists"] != "");
            }

            if (dr["PaymentTypesExists"] is DBNull)
            {
                validity.PaymentTypesExists = false;
            }
            else
            {
                validity.PaymentTypesExists = ((string)dr["PaymentTypesExists"] != "");
            }
            
            validity.PriceSettingsMatched = AsBool(dr["PriceSettingsMatched"]);
        }

        private string GetStoreSQL(UsageIntentEnum usageIntent)
        {
            if (usageIntent == UsageIntentEnum.Normal || usageIntent == UsageIntentEnum.Reporting)
            {
                return @"Select s.STOREID, 
                        ISNULL(s.NAME,'') as NAME, 
                        ISNULL(s.ADDRESS,'') as ADDRESS, 
                        s.ADDRESSFORMAT, 
                        s.SUSPENDALLOWEOD, 
                        ISNULL(s.STREET,'') as STREET,
                        ISNULL(s.CITY,'') as CITY, 
                        ISNULL(s.ZIPCODE,'') as ZIPCODE,
                        ISNULL(s.STATE,'') as STATE,
                        ISNULL(s.COUNTRY,'') as COUNTRY,
                        ISNULL(s.FUNCTIONALITYPROFILE,'') as FUNCTIONALITYPROFILE,
                        ISNULL(fp.NAME,'') as FUNCTIONALITYPROFILEDESCRIPTION,
                        ISNULL(s.CURRENCY,'') as  CURRENCY,
                        ISNULL(s.CULTURENAME,'') as CULTURENAME,
                        ISNULL(s.TAXGROUP,'') as TAXGROUP,
                        ISNULL(tgh.TAXGROUPNAME,'') as TAXGROUPNAME, 
                        ISNULL(s.SQLSERVERNAME,'') as SQLSERVERNAME, 
                        ISNULL(s.DATABASENAME,'') as DATABASENAME,
                        ISNULL(s.WINDOWSAUTHENTICATION,1) as WINDOWSAUTHENTICATION,
                        ISNULL(s.USERNAME,'') as USERNAME,
                        ISNULL(s.PASSWORD,'') as PASSWORD,
                        ISNULL(c.ACCOUNTNUM,'') as DEFAULTCUSTACCOUNT,
                        ISNULL(s.USEDEFAULTCUSTACCOUNT, 0) as USEDEFAULTCUSTACCOUNT,
                        ISNULL(c.NAME,'') as DEFAULTCUSTACCOUNTNAME,
                        ISNULL(s.KEYEDINPRICECONTAINSTAX, 1) as KEYEDINPRICECONTAINSTAX, 
                        ISNULL(cur.TXT, '') as CURRENCYDESCRIPTION,
                        ISNULL(s.LAYOUTID,'') as LAYOUTID,
                        ISNULL(lp.NAME,'') as TILLLAYOUTNAME, 
                        ISNULL(s.MAXIMUMPOSTINGDIFFERENCE, 0) as MAXIMUMPOSTINGDIFFERENCE,
                        ISNULL(s.MAXTRANSACTIONDIFFERENCEAMOUNT,0) as MAXTRANSACTIONDIFFERENCEAMOUNT, 
                        ISNULL(s.TENDERDECLARATIONCALCULATION,0) as TENDERDECLARATIONCALCULATION, 
                        ISNULL(s.CALCDISCFROM , 0) as CALCDISCFROM ,
                        ISNULL(s.DISPLAYDISCOUNTINCLTAX,1) as DISPLAYDISCOUNTINCLTAX, 
                        ISNULL(s.STOREPRICESETTING, 0) as STOREPRICESETTING, 
                        ISNULL(s.TRANSACTIONSERVICEPROFILE, 0) as TRANSACTIONSERVICEPROFILE, 
                        ISNULL(tp.NAME,'') as TRANSACTIONSERVICEPROFILENAME,
                        ISNULL(s.USETAXGROUPFROM, 0) as USETAXGROUPFROM, 
                        ISNULL(s.STYLEPROFILE, '') as STYLEPROFILE,
                        ISNULL(s.KEYBOARDCODE, '') as KEYBOARDCODE,
                        ISNULL(s.LAYOUTNAME, '') as LAYOUTNAME,
                        ISNULL(s.KITCHENMANAGERPROFILEID, CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER)) as KITCHENMANAGERPROFILEID,
                        ISNULL(kp.NAME,'') as KITCHENMANAGERPROFILENAME,
                        ISNULL(s.USETAXROUNDING, 0) as USETAXROUNDING,
                        ISNULL(s.DISPLAYBALANCEWITHTAX, 1) as DISPLAYBALANCEWITHTAX,
                        s.RECEIPTPROFILEID,
                        S.RECEIPTEMAILPROFILEID,
                        ISNULL(rp.DESCRIPTION, '') as RECEIPTPROFILEDESCRIPTION,
                        ISNULL(ep.DESCRIPTION, '') as EMAILPROFILEDESCRIPTION,
                        ISNULL(s.RETURNREASONCODEID, '') as RETURNREASONCODEID,
                        ISNULL(s.REGIONID, '') as REGIONID,
                        ISNULL(rg.DESCRIPTION, '') as REGIONDESCRIPTION,
                        ISNULL(s.PICTUREID, '') AS PICTUREID,
                        ISNULL(s.FORMINFOFIELD1,'') as FORMINFOFIELD1,
                        ISNULL(s.FORMINFOFIELD2,'') as FORMINFOFIELD2,
                        ISNULL(s.FORMINFOFIELD3,'') as FORMINFOFIELD3,
                        ISNULL(s.FORMINFOFIELD4,'') as FORMINFOFIELD4,
                        ISNULL(s.BARCODESYMBOLOGY, 2) as BARCODESYMBOLOGY,
                        ISNULL(s.OPERATIONAUDITSETTING, 0) as OPERATIONAUDITSETTING,
                        ISNULL(s.STARTAMOUNT, 0) AS STARTAMOUNT,
                        ISNULL(s.RETURNSPRINTEDTWICE, 0) AS RETURNSPRINTEDTWICE,
                        ISNULL(s.TENDERRECEIPTSAREREPRINTED, 0) AS TENDERRECEIPTSAREREPRINTED,
                        ISNULL(fp.OMNIMAINMENU, '') AS OMNIMAINMENU,
                        s.RECEIPTLOGOSIZE as RECEIPTLOGOSIZE,
                        s.DEFAULTDELIVERYTIME as DEFAULTDELIVERYTIME,
                        s.DELIVERYDAYSTYPE as DELIVERYDAYSTYPE
                        from RBOSTORETABLE s 
                        left outer join CUSTOMER c on s.DEFAULTCUSTACCOUNT = c.ACCOUNTNUM and s.DATAAREAID = c.DATAAREAID 
                        left outer join POSFUNCTIONALITYPROFILE fp on s.FUNCTIONALITYPROFILE = fp.PROFILEID and s.DATAAREAID = fp.DATAAREAID 
                        left outer join TAXGROUPHEADING tgh on s.TAXGROUP = tgh.TAXGROUP and s.DATAAREAID = tgh.DATAAREAID 
                        left outer join POSTRANSACTIONSERVICEPROFILE tp on s.TRANSACTIONSERVICEPROFILE = tp.PROFILEID and tp.DATAAREAID = s.DATAAREAID 
                        left outer join CURRENCY cur on s.CURRENCY = cur.CURRENCYCODE and s.DATAAREAID = cur.DATAAREAID 
                        left outer join POSISTILLLAYOUT lp on s.LAYOUTID = lp.LAYOUTID and lp.DATAAREAID = s.DATAAREAID 
                        left outer join KITCHENDISPLAYTRANSACTIONPROFILE kp on s.KITCHENMANAGERPROFILEID = kp.ID and s.DATAAREAID = kp.DATAAREAID 
                        left outer join POSFORMPROFILE rp on s.RECEIPTPROFILEID = rp.PROFILEID and s.DATAAREAID = rp.DATAAREAID 
                        left outer join POSFORMPROFILE ep on s.RECEIPTEMAILPROFILEID = ep.PROFILEID and s.DATAAREAID = rp.DATAAREAID
                        left outer join REGION rg on s.REGIONID = rg.ID ";
            }
            else
            {
                return @"Select s.STOREID, 
                        ISNULL(s.NAME,'') as NAME, 
                        ISNULL(s.ADDRESS,'') as ADDRESS, 
                        s.ADDRESSFORMAT, 
                        s.SUSPENDALLOWEOD, 
                        ISNULL(s.STREET,'') as STREET,
                        ISNULL(s.CITY,'') as CITY, 
                        ISNULL(s.ZIPCODE,'') as ZIPCODE,
                        ISNULL(s.STATE,'') as STATE,
                        ISNULL(s.COUNTRY,'') as COUNTRY,
                        ISNULL(s.FUNCTIONALITYPROFILE,'') as FUNCTIONALITYPROFILE,
                        ISNULL(s.CURRENCY,'') as  CURRENCY,
                        ISNULL(s.CULTURENAME,'') as CULTURENAME,
                        ISNULL(s.TAXGROUP,'') as TAXGROUP,
                        ISNULL(s.SQLSERVERNAME,'') as SQLSERVERNAME, 
                        ISNULL(s.DATABASENAME,'') as DATABASENAME,
                        ISNULL(s.WINDOWSAUTHENTICATION,1) as WINDOWSAUTHENTICATION,
                        ISNULL(s.USERNAME,'') as USERNAME,
                        ISNULL(s.PASSWORD,'') as PASSWORD,
                        ISNULL(s.USEDEFAULTCUSTACCOUNT, 0) as USEDEFAULTCUSTACCOUNT,
                        ISNULL(s.KEYEDINPRICECONTAINSTAX, 1) as KEYEDINPRICECONTAINSTAX, 
                        ISNULL(s.LAYOUTID,'') as LAYOUTID,
                        ISNULL(s.MAXIMUMPOSTINGDIFFERENCE, 0) as MAXIMUMPOSTINGDIFFERENCE,
                        ISNULL(s.MAXTRANSACTIONDIFFERENCEAMOUNT,0) as MAXTRANSACTIONDIFFERENCEAMOUNT, 
                        ISNULL(s.TENDERDECLARATIONCALCULATION,0) as TENDERDECLARATIONCALCULATION, 
                        ISNULL(s.CALCDISCFROM , 0) as CALCDISCFROM ,
                        ISNULL(s.DISPLAYDISCOUNTINCLTAX,1) as DISPLAYDISCOUNTINCLTAX, 
                        ISNULL(s.STOREPRICESETTING, 0) as STOREPRICESETTING, 
                        ISNULL(s.TRANSACTIONSERVICEPROFILE, 0) as TRANSACTIONSERVICEPROFILE,
                        ISNULL(s.DEFAULTCUSTACCOUNT, '') as DEFAULTCUSTACCOUNT,
                        ISNULL(s.USETAXGROUPFROM, 0) as USETAXGROUPFROM, 
                        ISNULL(s.STYLEPROFILE, '') as STYLEPROFILE,
                        ISNULL(s.KEYBOARDCODE, '') as KEYBOARDCODE,
                        ISNULL(s.LAYOUTNAME, '') as LAYOUTNAME,
                        ISNULL(s.KITCHENMANAGERPROFILEID, CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER)) as KITCHENMANAGERPROFILEID,
                        ISNULL(s.USETAXROUNDING, 0) as USETAXROUNDING,
                        ISNULL(s.DISPLAYBALANCEWITHTAX, 0) as DISPLAYBALANCEWITHTAX,
                        s.RECEIPTPROFILEID,
                        s.RECEIPTEMAILPROFILEID,
                        ISNULL(s.PICTUREID, '') AS PICTUREID,
                        ISNULL(s.RETURNREASONCODEID, '') as RETURNREASONCODEID,
                        ISNULL(s.REGIONID, '') as REGIONID,
                        ISNULL(s.FORMINFOFIELD1,'') as FORMINFOFIELD1,
                        ISNULL(s.FORMINFOFIELD2,'') as FORMINFOFIELD2,
                        ISNULL(s.FORMINFOFIELD3,'') as FORMINFOFIELD3,
                        ISNULL(s.FORMINFOFIELD4,'') as FORMINFOFIELD4,
                        ISNULL(s.BARCODESYMBOLOGY, 2) as BARCODESYMBOLOGY,
                        ISNULL(s.OPERATIONAUDITSETTING, 0) as OPERATIONAUDITSETTING,
                        s.RECEIPTLOGOSIZE as RECEIPTLOGOSIZE,
                        s.DEFAULTDELIVERYTIME as DEFAULTDELIVERYTIME,
                        s.DELIVERYDAYSTYPE as DELIVERYDAYSTYPE
                        from RBOSTORETABLE s ";
            }
        }

        public virtual List<StoreValidity> CheckStoreValidity(IConnectionManager entry) 
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT 
                                    	s.STOREID, 
                                    	ISNULL(s.NAME,'') AS NAME,
                                    	t.TERMINALID AS TerminalExists, 
                                    	s.FUNCTIONALITYPROFILE AS FunctionalityProfileExist, 
                                    	s.LAYOUTID AS ButtonLayoutExists, 
                                    	p.STOREID AS PaymentTypesExists,
                                    	IIF(s.CALCDISCFROM + 1 = s.StorePriceSetting, 1, 0) AS PriceSettingsMatched
                                    FROM RBOSTORETABLE s
                                    OUTER APPLY
                                    (
                                        SELECT TOP 1 TERMINALID FROM RBOTERMINALTABLE  WHERE s.STOREID = STOREID AND s.DATAAREAID = DATAAREAID
                                    ) t
                                    OUTER APPLY
                                    (
                                    	SELECT TOP 1 STOREID FROM RBOSTORETENDERTYPETABLE WHERE s.STOREID = STOREID AND s.DATAAREAID = DATAAREAID
                                    ) p
                                    WHERE s.DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return Execute<StoreValidity>(entry, cmd, CommandType.Text, PopulateStoreValidity);
            }
        }
        

        /// <summary>
        /// Gets a list of all stores
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all stores</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOSTORETABLE", "NAME", "STOREID", "NAME");
        }

        /// <summary>
        /// Gets a list of all stores except one specific one
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="excludeStoreID">The store to be excluded</param>
        /// <returns>A list of all store except one that was specified</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry,RecordIdentifier excludeStoreID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select STOREID, ISNULL(NAME,'') as NAME from RBOSTORETABLE where DATAAREAID = @dataAreaId and STOREID <> @storeID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string)excludeStoreID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "STOREID");
            }
        }

        public virtual List<DataEntity> GetListForTerminal(IConnectionManager entry, RecordIdentifier currentTerminal, RecordIdentifier currentStore)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select STOREID, ISNULL(NAME,'') as NAME from RBOSTORETABLE where STOREID not in (select Storeid from RBOTERMINALTABLE where TERMINALID = @terminalID and STOREID <> @storeID and DATAAREAID = @dataAreaId) and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "terminalID", (string)currentTerminal);
                MakeParam(cmd, "storeID", (string)currentStore);
                
                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "STOREID");
            }
        }


        /// <summary>
        /// Gets one specific store as simple data entity, that is ID and description
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The ID of the store to fetch</param>
        /// <returns>The requested store or null if not found</returns>
        public virtual DataEntity GetStoreEntity(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {

                ValidateSecurity(entry);

                cmd.CommandText = "Select STOREID, ISNULL(NAME,'') as NAME from RBOSTORETABLE where DATAAREAID = @dataAreaId and STOREID = @storeID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string)storeID);

                var result = Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "STOREID");
                return (result.Count > 0) ? result[0] : null;
            }
        } 

        public virtual void Delete(IConnectionManager entry, RecordIdentifier storeID)
        {
            DeleteRecord(entry, "RBOSTORETABLE", "STOREID", storeID, Permission.StoreEdit);
            DeleteRecord(entry, "RBOSTORETENDERTYPETABLE", "STOREID", storeID, Permission.StoreEdit);
            DeleteRecord(entry, "RBOSTORETENDERTYPECARDTABLE", "STOREID", storeID, Permission.StoreEdit);

            // Remove the store from the kds section station routing table
            Providers.KitchenDisplaySectionStationRoutingData.RemoveRestaurant(entry, storeID);
        }

        private static void PopulateStoreListItem(IDataReader dr, StoreListItem item)
        {
            item.ID = (string)dr["STOREID"];
            item.Text = (string)dr["NAME"];
            item.City = (string)dr["CITY"];
        }

        private static void PopulateStoreListItemExtended(IDataReader dr, StoreListItemExtended item)
        {
            PopulateStoreListItem(dr, item);

            item.Currency = (string)dr["CURRENCYNAME"];
            item.DefaultCustomer = (string)dr["CUSTOMERNAME"];
            item.FormProfile = (string)dr["FORMPROFILENAME"];
            item.FunctionalityProfile = (string)dr["FUNCTIONALITYPROFILENAME"];
            item.PriceSetting = (Store.StorePriceSettingsEnum)dr["STOREPRICESETTING"];
            item.Region = (string)dr["REGIONNAME"];
            item.SalesTaxGroup = (string)dr["TAXGROUPNAME"];
            item.SiteServiceProfile = (string)dr["SITESERVICEPROFILENAME"];
            item.TerminalsCount = (int)dr["TERMINALCOUNT"];
            item.TouchButtons = (string)dr["TOUCHLAYOUTNAME"];
        }

        private static void PopulateCountableStorePayments(IDataReader dr, Payment item)
        {
            item.ID = (string)dr["TENDERTYPEID"];
            item.Text = (string)dr["NAME"];
            item.IsForeignCurrency = (Convert.ToInt32(dr["POSOPERATION"]) == 203);
            item.RoundingMethod = (int)dr["ROUNDINGMETHOD"];
            item.RoundingValue = (decimal)dr["ROUNDING"];
        }

        private static void PopulateStorePayment(IDataReader dr, Payment payment)
        {
            payment.ID = (string) dr["TENDERTYPEID"];
            payment.Text = (string) dr["NAME"];
            payment.PosOperation = (int) dr["POSOPERATION"];
            payment.IsForeignCurrency = (payment.PosOperation == 203);
            payment.RoundingMethod = (int)dr["ROUNDINGMETHOD"];
            payment.RoundingValue = (decimal)dr["ROUNDING"];
        }

        public virtual List<StoreListItem> Search(IConnectionManager entry, StoreListSearchFilter filter)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "STOREID", ColumnAlias = "STOREID", TableAlias = "S"},
                    new TableColumn {ColumnName = "NAME", ColumnAlias = "NAME", IsNull = true, NullValue = "''", TableAlias = "S"},
                    new TableColumn {ColumnName = "CITY", ColumnAlias = "CITY", IsNull = true, NullValue = "''", TableAlias = "S"},
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = "S.DATAAREAID = @DATAAREAID", Operator = "AND"}
                };

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if(filter.DescriptionOrID != string.Empty)
                {
                    if(filter.City == filter.DescriptionOrID)
                    {
                        conditions.Add(new Condition { ConditionValue = "(S.STOREID LIKE @DESCRIPTION OR S.NAME LIKE @DESCRIPTION OR S.CITY LIKE @DESCRIPTION)", Operator = "AND" });
                    }
                    else
                    {
                        conditions.Add(new Condition { ConditionValue = "(S.STOREID LIKE @DESCRIPTION OR S.NAME LIKE @DESCRIPTION)", Operator = "AND" });
                    }

                    MakeParam(cmd, "DESCRIPTION", PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith));
                }

                if(filter.City != string.Empty && filter.DescriptionOrID != filter.City)
                {
                    conditions.Add(new Condition { ConditionValue = "S.CITY LIKE @CITY", Operator = "AND" });
                    MakeParam(cmd, "CITY", PreProcessSearchText(filter.City, true, filter.CityBeginsWith));
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RBOSTORETABLE", "S", filter.MaxCount),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    "",
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(filter.Sort, filter.SortBackwards));

                return Execute<StoreListItem>(entry, cmd, CommandType.Text, PopulateStoreListItem);
            }
        }

        public virtual List<StoreListItemExtended> SearchExtended(IConnectionManager entry, StoreListSearchFilterExtended filter)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>
                {
                    new TableColumn {ColumnName = "STOREID", ColumnAlias = "STOREID", TableAlias = "S"},
                    new TableColumn {ColumnName = "NAME", ColumnAlias = "NAME", IsNull = true, NullValue = "''", TableAlias = "S"},
                    new TableColumn {ColumnName = "CITY", ColumnAlias = "CITY", IsNull = true, NullValue = "''", TableAlias = "S"},
                    new TableColumn {ColumnName = "STOREPRICESETTING", ColumnAlias = "STOREPRICESETTING", IsNull = true, NullValue = "0", TableAlias = "S"},
                    new TableColumn {ColumnName = "NAME", ColumnAlias = "CUSTOMERNAME", IsNull = true, NullValue = "''", TableAlias = "C"},
                    new TableColumn {ColumnName = "NAME", ColumnAlias = "FUNCTIONALITYPROFILENAME", IsNull = true, NullValue = "''", TableAlias = "FP"},
                    new TableColumn {ColumnName = "TAXGROUPNAME", ColumnAlias = "TAXGROUPNAME", IsNull = true, NullValue = "''", TableAlias = "TGH"},
                    new TableColumn {ColumnName = "NAME", ColumnAlias = "SITESERVICEPROFILENAME", IsNull = true, NullValue = "''", TableAlias = "TP"},
                    new TableColumn {ColumnName = "TXT", ColumnAlias = "CURRENCYNAME", IsNull = true, NullValue = "''", TableAlias = "CR"},
                    new TableColumn {ColumnName = "NAME", ColumnAlias = "TOUCHLAYOUTNAME", IsNull = true, NullValue = "''", TableAlias = "LP"},
                    new TableColumn {ColumnName = "DESCRIPTION", ColumnAlias = "FORMPROFILENAME", IsNull = true, NullValue = "''", TableAlias = "RP"},
                    new TableColumn {ColumnName = "DESCRIPTION", ColumnAlias = "REGIONNAME", IsNull = true, NullValue = "''", TableAlias = "RG"},
                    new TableColumn {ColumnName = "COALESCE(TC.TERMINALCOUNT, 0)", ColumnAlias = "TERMINALCOUNT"},
                };

                List<Join> joins = new List<Join>
                {
                    new Join {JoinType = "LEFT", Table = "CUSTOMER", TableAlias = "C", Condition = "S.DEFAULTCUSTACCOUNT = C.ACCOUNTNUM"},
                    new Join {JoinType = "LEFT", Table = "POSFUNCTIONALITYPROFILE", TableAlias = "FP", Condition = "S.FUNCTIONALITYPROFILE = FP.PROFILEID"},
                    new Join {JoinType = "LEFT", Table = "TAXGROUPHEADING", TableAlias = "TGH", Condition = "S.TAXGROUP = TGH.TAXGROUP"},
                    new Join {JoinType = "LEFT", Table = "POSTRANSACTIONSERVICEPROFILE", TableAlias = "TP", Condition = "S.TRANSACTIONSERVICEPROFILE = TP.PROFILEID"},
                    new Join {JoinType = "LEFT", Table = "CURRENCY", TableAlias = "CR", Condition = "S.CURRENCY = CR.CURRENCYCODE"},
                    new Join {JoinType = "LEFT", Table = "POSISTILLLAYOUT", TableAlias = "LP", Condition = "S.LAYOUTID = LP.LAYOUTID"},
                    new Join {JoinType = "LEFT", Table = "POSFORMPROFILE", TableAlias = "RP", Condition = "S.RECEIPTPROFILEID = RP.PROFILEID"},
                    new Join {JoinType = "LEFT", Table = "REGION", TableAlias = "RG", Condition = "S.REGIONID = RG.ID"},
                    new Join {JoinType = "LEFT", Table = "(SELECT T.STOREID, COUNT(*) AS TERMINALCOUNT FROM RBOTERMINALTABLE T GROUP BY T.STOREID) TC", TableAlias = "", Condition = "TC.STOREID = S.STOREID"},
                };

                List<Condition> conditions = new List<Condition>
                {
                    new Condition {ConditionValue = "S.DATAAREAID = @DATAAREAID", Operator = "AND"}
                };

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                if (filter.DescriptionOrID != string.Empty)
                {
                    conditions.Add(new Condition { ConditionValue = "(S.STOREID LIKE @DESCRIPTION OR S.NAME LIKE @DESCRIPTION)", Operator = "AND" });
                    MakeParam(cmd, "DESCRIPTION", PreProcessSearchText(filter.DescriptionOrID, true, filter.DescriptionOrIDBeginsWith));
                }

                if (filter.City != string.Empty)
                {
                    conditions.Add(new Condition { ConditionValue = "S.CITY LIKE @CITY", Operator = "AND" });
                    MakeParam(cmd, "CITY", PreProcessSearchText(filter.City, true, filter.CityBeginsWith));
                }

                if(!RecordIdentifier.IsEmptyOrNull(filter.CurrencyCode))
                {
                    conditions.Add(new Condition { ConditionValue = "S.CURRENCY = @CURRENCY", Operator = "AND" });
                    MakeParam(cmd, "CURRENCY", filter.CurrencyCode);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.CustomerID))
                {
                    conditions.Add(new Condition { ConditionValue = "S.DEFAULTCUSTACCOUNT = @CUSTACCOUNT", Operator = "AND" });
                    MakeParam(cmd, "CUSTACCOUNT", filter.CustomerID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.FormProfileID))
                {
                    conditions.Add(new Condition { ConditionValue = "S.RECEIPTPROFILEID = @FORMPROFILEID", Operator = "AND" });
                    MakeParam(cmd, "FORMPROFILEID", filter.FormProfileID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.FunctionalityProfileID))
                {
                    conditions.Add(new Condition { ConditionValue = "S.FUNCTIONALITYPROFILE = @FUNCTIONALITYPROFILEID", Operator = "AND" });
                    MakeParam(cmd, "FUNCTIONALITYPROFILEID", filter.FunctionalityProfileID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.RegionID))
                {
                    conditions.Add(new Condition { ConditionValue = "S.REGIONID = @REGIONID", Operator = "AND" });
                    MakeParam(cmd, "REGIONID", filter.RegionID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.SalesTaxGroupID))
                {
                    conditions.Add(new Condition { ConditionValue = "S.TAXGROUP = @TAXGROUP", Operator = "AND" });
                    MakeParam(cmd, "TAXGROUP", filter.SalesTaxGroupID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.SiteServiceProfileID))
                {
                    conditions.Add(new Condition { ConditionValue = "S.TRANSACTIONSERVICEPROFILE = @SITESERVICEPROFILE", Operator = "AND" });
                    MakeParam(cmd, "SITESERVICEPROFILE", filter.SiteServiceProfileID);
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.TouchButtonsLayoutID))
                {
                    conditions.Add(new Condition { ConditionValue = "S.LAYOUTID = @LAYOUTID", Operator = "AND" });
                    MakeParam(cmd, "LAYOUTID", filter.TouchButtonsLayoutID);
                }

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RBOSTORETABLE", "S", filter.MaxCount),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    ResolveSort(filter.Sort, filter.SortBackwards));

                return Execute<StoreListItemExtended>(entry, cmd, CommandType.Text, PopulateStoreListItemExtended);
            }
        }

        public virtual List<Store> GetStores(IConnectionManager entry, UsageIntentEnum usageIntent = UsageIntentEnum.Normal)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = GetStoreSQL(usageIntent) + " where s.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                if(usageIntent == UsageIntentEnum.Minimal)
                {
                    return Execute<Store>(entry, cmd, CommandType.Text, false, PopulateStoreMinimal);
                }
                else
                {
                    return Execute<Store>(entry, cmd, CommandType.Text, false, PopulateStoreNormal);
                }
            }
        }


        public virtual bool WarnOnStatementPostingIfSuspendedTransExists(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select SUSPENDALLOWEOD " +
                        "from RBOSTORETABLE s " +         
                        "where DATAAREAID = @dataAreaId and STOREID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)storeID);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {  
                        var allowEOD = ((int)dr["SUSPENDALLOWEOD"] == (int)SuspendedTransactionsStatementPostingEnum.Yes);
                        return allowEOD;
                    }

                    return true;
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }
        }
        
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier storeID)
        {
            return RecordExists(entry, "RBOSTORETABLE", "STOREID", storeID);
        }

        /// <summary>
        /// Checks if any store is using a given tax group id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxgroupID">ID of the tax group</param>
        /// <returns>True if any store uses the tax group, else false</returns>
        public virtual bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID)
        {
            return RecordExists(entry, "RBOSTORETABLE", "TAXGROUP", taxgroupID);
        }

        public virtual void Save(IConnectionManager entry, Store store)
        {
            var statement = new SqlServerStatement("RBOSTORETABLE");

            ValidateSecurity(entry, BusinessObjects.Permission.StoreEdit);

            bool isNew = false;
            if (store.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                store.ID = DataProviderFactory.Instance.GenerateNumber<IStoreData, Store>(entry);
            }

            bool defaultStoreNeedsToBeSet = false;
            if (isNew || !Exists(entry, store.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STOREID", (string)store.ID);

                var storeList = GetList(entry);
                defaultStoreNeedsToBeSet = storeList.Count == 0;
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STOREID", (string)store.ID);
            }

            statement.AddField("NAME", store.Text);
            statement.AddField("ADDRESS", store.Address.Address2);
            statement.AddField("STREET", store.Address.Address1);
            statement.AddField("CITY", store.Address.City);
            statement.AddField("ZIPCODE", store.Address.Zip);
            statement.AddField("STATE", store.Address.State);
            statement.AddField("COUNTRY", (string)store.Address.Country);
            statement.AddField("FUNCTIONALITYPROFILE", store.FunctionalityProfile);
            statement.AddField("CURRENCY", store.Currency);
            statement.AddField("CULTURENAME", store.LanguageCode);
            statement.AddField("TAXGROUP", store.TaxGroup);
            statement.AddField("SQLSERVERNAME", store.BackupDatabaseServer);
            statement.AddField("DATABASENAME", store.BackupDatabaseName);
            statement.AddField("WINDOWSAUTHENTICATION", store.BackupDatabaseWindowsAuthentication ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("USERNAME", store.BackupDatabaseUser);
            statement.AddField("PASSWORD", store.BackupDatabasePassword);
            statement.AddField("DEFAULTCUSTACCOUNT", (string)store.DefaultCustomerAccount);
            statement.AddField("USEDEFAULTCUSTACCOUNT", store.UseDefaultCustomerAccount ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LAYOUTID", (string)store.LayoutID);
            statement.AddField("TENDERDECLARATIONCALCULATION", (int)store.TenderDeclarationCalculation, SqlDbType.Int);
            statement.AddField("MAXIMUMPOSTINGDIFFERENCE", store.MaximumPostingDifference, SqlDbType.Decimal);
            statement.AddField("MAXTRANSACTIONDIFFERENCEAMOUNT", store.MaximumTransactionDifference, SqlDbType.Decimal);
            statement.AddField("ADDRESSFORMAT", (int)store.Address.AddressFormat, SqlDbType.Int);
            statement.AddField("KEYEDINPRICECONTAINSTAX", store.KeyedInPriceIncludesTax ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("CALCDISCFROM", (int)store.CalculateDiscountsFrom, SqlDbType.Int);
            statement.AddField("DISPLAYDISCOUNTINCLTAX", store.DisplayAmountsWithTax ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DISPLAYBALANCEWITHTAX", store.DisplayBalanceWithTax ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("USETAXGROUPFROM", (int)store.UseTaxGroupFrom, SqlDbType.Int);
            statement.AddField("SUSPENDALLOWEOD", store.AllowEOD ? 2 : 3, SqlDbType.Int);
            statement.AddField("TRANSACTIONSERVICEPROFILE", store.TransactionServiceProfileID);            
            statement.AddField("STOREPRICESETTING", (int)store.StorePriceSetting, SqlDbType.Int);
            statement.AddField("STYLEPROFILE", store.StyleProfile);
            statement.AddField("KEYBOARDCODE", store.KeyboardCode);
            statement.AddField("LAYOUTNAME", store.KeyboardLayoutName);
            statement.AddField("KITCHENMANAGERPROFILEID", (Guid)store.KitchenServiceProfileID, SqlDbType.UniqueIdentifier);
            statement.AddField("USETAXROUNDING", store.UseTaxRounding ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("RECEIPTPROFILEID", (Guid)store.FormProfileID, SqlDbType.UniqueIdentifier);
            statement.AddField("RECEIPTEMAILPROFILEID", (Guid)store.EmailFormProfileID, SqlDbType.UniqueIdentifier);
            statement.AddField("RETURNREASONCODEID", (string)store.ReturnReasonCodeID);
            statement.AddField("REGIONID", (string)store.RegionID);
            statement.AddField("PICTUREID", (string)store.PictureID);
            statement.AddField("FORMINFOFIELD1", store.FormInfoField1);
            statement.AddField("FORMINFOFIELD2", store.FormInfoField2);
            statement.AddField("FORMINFOFIELD3", store.FormInfoField3);
            statement.AddField("FORMINFOFIELD4", store.FormInfoField4);
            statement.AddField("BARCODESYMBOLOGY", store.BarcodeSymbology, SqlDbType.Int);
            statement.AddField("OPERATIONAUDITSETTING", store.OperationAuditSetting, SqlDbType.Int);
            statement.AddField("STARTAMOUNT", store.StartAmount, SqlDbType.Decimal);
            statement.AddField("RETURNSPRINTEDTWICE", store.ReturnsPrintedTwice ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TENDERRECEIPTSAREREPRINTED", store.TenderReceiptAreReprinted ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("RECEIPTLOGOSIZE", (byte)store.LogoSize, SqlDbType.TinyInt);
            statement.AddField("DEFAULTDELIVERYTIME", store.StoreTransferDefaultDeliveryTime, SqlDbType.Int);
            statement.AddField("DELIVERYDAYSTYPE", (int)store.StoreTransferDeliveryDaysType, SqlDbType.Int);


            entry.Connection.ExecuteStatement(statement);

            // If no default store exists, make this store the default store
            if (defaultStoreNeedsToBeSet)
            {
                var defaultStoreParameters = new Parameters {LocalStore = store.ID};
                Providers.ParameterData.Save(entry, defaultStoreParameters);
            }
        }

        public virtual RecordIdentifier GetDefaultStoreID(IConnectionManager entry)
        {
            RecordIdentifier defaultStoreId = "";

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select LOCALSTOREID " +
                    "from RBOPARAMETERS P " +
                    "JOIN RBOSTORETABLE S ON P.DATAAREAID = S.DATAAREAID AND P.LOCALSTOREID = S.STOREID " +
                    "where P.DATAAREAID = @dataAreaID and KEY_ = 0";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    // If no record is returned, no default store exists.
                    if (dr.Read())
                    {
                        if (dr["LOCALSTOREID"] != DBNull.Value)
                        {
                            defaultStoreId = (string)dr["LOCALSTOREID"];
                        }
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

            return defaultStoreId;
        }

        public virtual RecordIdentifier GetStoresSalesTaxGroupID(IConnectionManager entry, RecordIdentifier storeID)
        {
            RecordIdentifier salesTaxGroupID = "";

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select ISNULL(TAXGROUP,'') as TAXGROUP " +
                    "from RBOSTORETABLE " +
                    "where STOREID = @storeId and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeId", (string)storeID);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        salesTaxGroupID = (string)dr["TAXGROUP"];
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }

                return salesTaxGroupID;
            }
        }

        public virtual RecordIdentifier GetDefaultStoreSalesTaxGroup(IConnectionManager entry)
        {
            return GetStoresSalesTaxGroupID(entry, GetDefaultStoreID(entry)) ;
        }

        public virtual List<Payment> GetCountableStorePayments(IConnectionManager entry, string storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select TENDERTYPEID, ISNULL(NAME,'') as NAME, POSOPERATION, " +
                                  " ISNULL(ROUNDINGMETHOD,0) as ROUNDINGMETHOD, ISNULL(ROUNDING,0) as ROUNDING " +
                                  " from RBOSTORETENDERTYPETABLE where STOREID = @storeID and COUNTINGREQUIRED = @countingRequired and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "storeID", storeID);
                MakeParam(cmd, "countingRequired", 1);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<Payment>(entry, cmd, CommandType.Text, PopulateCountableStorePayments);
            }
        }

        /// <summary>
        /// Returns a list of all payment types for the given store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <returns>A list of all payment types for the given store ID</returns>
        public virtual List<Payment> GetStorePayments(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select TENDERTYPEID, ISNULL(NAME,'') as NAME, ISNULL(POSOPERATION,0) as POSOPERATION, " +
                                  " ISNULL(ROUNDINGMETHOD,0) as ROUNDINGMETHOD, ISNULL(ROUNDING,0) as ROUNDING " +
                                  " from RBOSTORETENDERTYPETABLE where STOREID = @storeID and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "storeID", (string)storeID);                
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<Payment>(entry, cmd, CommandType.Text, PopulateStorePayment);
            }
        }

        /// <summary>
        /// Gets the store payment type that the given card number belongs to
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="cardNumber">The first 6 numbers of the card number</param>
        /// <returns>The store payment type that this card type belongs to</returns>
        public virtual Payment GetStorePaymentFromCardNumber(IConnectionManager entry, RecordIdentifier storeID, string cardNumber)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select t.TENDERTYPEID as TENDERTYPEID, " +
                                  "ISNULL(t.NAME,'') as NAME, " +
                                  "ISNULL(t.POSOPERATION,0) as POSOPERATION, " +
                                  "ISNULL(t.ROUNDINGMETHOD,0) as ROUNDINGMETHOD, " +
                                  "ISNULL(t.ROUNDING,0) as ROUNDING " +
                                  "from RBOTENDERTYPECARDNUMBERS s " +
                                  "left outer join RBOSTORETENDERTYPECARDTABLE r on r.CARDTYPEID = s.CARDTYPEID and r.DATAAREAID = s.DATAAREAID " +
                                  "left outer join RBOSTORETENDERTYPETABLE t on t.TENDERTYPEID = r.TENDERTYPEID and t.DATAAREAID = s.DATAAREAID " +
                                  "where @cardNumber between s.CARDNUMBERFROM and s.CARDNUMBERTO and r.STOREID = @storeId and s.DATAAREAID =@dataAreaId";

                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "cardNumber", cardNumber);

                var result = Execute<Payment>(entry, cmd, CommandType.Text, PopulateStorePayment);

                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual string GetStoreCurrencyCode(IConnectionManager entry, RecordIdentifier storeID)
        {
            return Get(entry, storeID, usageIntent: UsageIntentEnum.Minimal).Currency;
        }

        public virtual Payment GetStorePayment(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier paymentID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select TENDERTYPEID, ISNULL(NAME,'') as NAME, POSOPERATION, ROUNDINGMETHOD, ROUNDING from RBOSTORETENDERTYPETABLE where STOREID = @storeID and TenderTypeID = @paymentID and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "paymentID", (string)paymentID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Get<Payment>(entry, cmd, new RecordIdentifier(storeID, paymentID), PopulateCountableStorePayments, cacheType,UsageIntentEnum.Normal);
            }
        }

        public virtual Payment GetStoreCashPayment(IConnectionManager entry, RecordIdentifier storeID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select TENDERTYPEID, ISNULL(NAME,'') as NAME, POSOPERATION, ROUNDINGMETHOD, ROUNDING from RBOSTORETENDERTYPETABLE where STOREID = @storeID and POSOPERATION=200 and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "storeID", (string)storeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Get<Payment>(entry, cmd, new RecordIdentifier(storeID, "CASH200"), PopulateCountableStorePayments, cacheType,UsageIntentEnum.Normal);
            }
        }
        
        public virtual Store Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone, UsageIntentEnum usageIntent = UsageIntentEnum.Normal, bool includeReportFormatting = false)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                if (usageIntent == UsageIntentEnum.Normal || usageIntent == UsageIntentEnum.Reporting)
                {
                        cmd.CommandText = GetStoreSQL(usageIntent) + " \r\n" + 
                        "where s.DATAAREAID = @dataAreaId and s.STOREID = @id";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "id", (string)id);

                    return Get<Store>(entry, cmd, CommandType.Text, id, includeReportFormatting, PopulateStoreNormal, cache, usageIntent);
                }
                
                if (usageIntent == UsageIntentEnum.Minimal)
                {
                    cmd.CommandText = GetStoreSQL(usageIntent ) + " \r\n" +
                        "where s.DATAAREAID = @dataAreaId and s.STOREID = @id";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "id", (string)id);

                    return Get<Store>(entry, cmd, CommandType.Text, id, includeReportFormatting, PopulateStoreMinimal, cache, usageIntent);
                }
                return null;
            }
        }
        protected virtual void PopulateStoreNormalWithCount(IConnectionManager entry, IDataReader dr, Store store, ref int rowCount, object defaultFormat)

        {
            PopulateStoreNormal(entry, dr, store, defaultFormat); 
            PopulateRowCount(entry, dr, ref rowCount);

        }
        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }
        public virtual void PopulateStoreNormal(IConnectionManager entry, IDataReader dr,Store store, object param)
        {
            PopulateStoreMinimal(entry, dr, store, param);

            store.CurrencyDescription = (string)dr["CURRENCYDESCRIPTION"];
            store.TaxGroupName = (string)dr["TAXGROUPNAME"];
            store.DefaultCustomerAccountDescription = (string)dr["DEFAULTCUSTACCOUNTNAME"];
            store.FunctionalityProfileDescription = (string)dr["FUNCTIONALITYPROFILEDESCRIPTION"];
            store.LayoutDescription = (string)dr["TILLLAYOUTNAME"];
            store.TransactionServiceProfileName = (string) dr["TRANSACTIONSERVICEPROFILENAME"];
            store.KitchenServiceProfileName = (string)dr["KITCHENMANAGERPROFILENAME"];
            store.FormProfileDescription = AsString(dr["RECEIPTPROFILEDESCRIPTION"]);
            store.EmailFormProfileDescription = AsString(dr["EMAILPROFILEDESCRIPTION"]);
            store.StartAmount = (decimal) dr["STARTAMOUNT"];
            store.ReturnsPrintedTwice = ((byte)dr["RETURNSPRINTEDTWICE"] == 1);
            store.TenderReceiptAreReprinted = ((byte)dr["TENDERRECEIPTSAREREPRINTED"] == 1);
            store.RegionDescription = (string)dr["REGIONDESCRIPTION"];
            store.InventoryMainMenuID = (string)dr["OMNIMAINMENU"];

        }

        public virtual void PopulateStoreMinimal(IConnectionManager entry, IDataReader dr, Store store, object param)
        {
            store.ID = (string)dr["STOREID"];
            store.Text = (string)dr["NAME"];

            store.Address.Address1 = (string)dr["STREET"];
            store.Address.Address2 = (string)dr["ADDRESS"];
            store.Address.Zip = (string)dr["ZIPCODE"];
            store.Address.City = (string)dr["CITY"];
            store.Address.State = (string)dr["STATE"];
            store.Address.Country = (string)dr["COUNTRY"];
            store.Address.AddressFormat = (dr["ADDRESSFORMAT"] == DBNull.Value) ? entry.Settings.AddressFormat : (Address.AddressFormatEnum)((int)(dr["ADDRESSFORMAT"]));

            store.FunctionalityProfile = (string)dr["FUNCTIONALITYPROFILE"];
            store.Currency = (string)dr["CURRENCY"];
            store.LanguageCode = (string)dr["CULTURENAME"];
            store.TaxGroup = (string)dr["TAXGROUP"];
            store.BackupDatabaseName = (string)dr["DATABASENAME"];
            store.BackupDatabaseWindowsAuthentication = ((byte)dr["WINDOWSAUTHENTICATION"] != 0);
            store.BackupDatabaseServer = (string)dr["SQLSERVERNAME"];
            store.BackupDatabaseUser = (string)dr["USERNAME"];
            store.BackupDatabasePassword = (string)dr["PASSWORD"];
            store.UseDefaultCustomerAccount = ((byte)dr["USEDEFAULTCUSTACCOUNT"] != 0);
            store.DefaultCustomerAccount = (string)dr["DEFAULTCUSTACCOUNT"];
            store.LayoutID = (string)dr["LAYOUTID"];
            store.TransactionServiceProfileID = (string)dr["TRANSACTIONSERVICEPROFILE"];
            store.TenderDeclarationCalculation = (TenderDeclarationCalculation)dr["TENDERDECLARATIONCALCULATION"];
            store.MaximumPostingDifference = (decimal)dr["MAXIMUMPOSTINGDIFFERENCE"];
            store.MaximumTransactionDifference = (decimal)dr["MAXTRANSACTIONDIFFERENCEAMOUNT"];
            store.KeyedInPriceIncludesTax = ((byte)dr["KEYEDINPRICECONTAINSTAX"] == 1);
            store.CalculateDiscountsFrom = (Store.CalculateDiscountsFromEnum)dr["CALCDISCFROM"];
            store.DisplayAmountsWithTax = ((byte)dr["DISPLAYDISCOUNTINCLTAX"] == 1);
            store.DisplayBalanceWithTax = ((byte)dr["DISPLAYBALANCEWITHTAX"] == 1);
            store.UseTaxGroupFrom = (UseTaxGroupFromEnum)((int)dr["USETAXGROUPFROM"]);
            store.AllowEOD = ((int)dr["SUSPENDALLOWEOD"] == 2);
            store.StorePriceSetting = (Store.StorePriceSettingsEnum)(dr["STOREPRICESETTING"]);
            store.StyleProfile = (string)dr["STYLEPROFILE"];
            store.KeyboardCode = (string)dr["KEYBOARDCODE"];
            store.KeyboardLayoutName = (string)dr["LAYOUTNAME"];
            store.KitchenServiceProfileID = (Guid)dr["KITCHENMANAGERPROFILEID"];
            store.UseTaxRounding = ((byte)dr["USETAXROUNDING"] == 1);
            store.FormProfileID = AsGuid(dr["RECEIPTPROFILEID"]);
            store.EmailFormProfileID = AsGuid(dr["RECEIPTEMAILPROFILEID"]);
            store.ReturnReasonCodeID = (string)dr["RETURNREASONCODEID"];
            store.RegionID = (string)dr["REGIONID"];
            store.PictureID = (string)dr["PICTUREID"];

            store.FormInfoField1 = AsString(dr["FORMINFOFIELD1"]);
            store.FormInfoField2 = AsString(dr["FORMINFOFIELD2"]);
            store.FormInfoField3 = AsString(dr["FORMINFOFIELD3"]);
            store.FormInfoField4 = AsString(dr["FORMINFOFIELD4"]);

            store.BarcodeSymbology = (BarcodeType) AsInt(dr["BARCODESYMBOLOGY"]);
            store.OperationAuditSetting = (OperationAuditEnum) AsInt(dr["OPERATIONAUDITSETTING"]);
            store.LogoSize = (StoreLogoSizeType)AsByte(dr["RECEIPTLOGOSIZE"]);
            store.StoreTransferDefaultDeliveryTime = AsInt(dr["DEFAULTDELIVERYTIME"]);
            store.StoreTransferDeliveryDaysType = (DeliveryDaysTypeEnum)AsInt(dr["DELIVERYDAYSTYPE"]);


            if ((bool)param)
            {
                store.AddressFormatted = entry.Settings.LocalizationContext.FormatMultipleLines(store.Address, entry.Cache, "\n");
            }
        }

        /// <summary>
        /// Returns if store has a price setting where prices should be fetched with or without tax
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="storeID">The ID of the store</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>True and store has price with tax, false and store has price without tax</returns>
        public virtual bool GetPriceWithTaxForStore(IConnectionManager entry, RecordIdentifier storeID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            var store = Get(entry, storeID,cacheType,UsageIntentEnum.Minimal);

            return GetPriceWithTaxForStore(entry, store);
        }

        /// <summary>
        /// Returns if store has a price setting where prices should be fetched with or without tax
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="store">The store</param>
        /// <returns>True and store has price with tax, false and store has price without tax</returns>
        public virtual bool GetPriceWithTaxForStore(IConnectionManager entry, Store store)
        {
            switch (store.StorePriceSetting)
            {
                case Store.StorePriceSettingsEnum.UsePriceGroupSettings:
                    var listOfStoresPriceGroups = Providers.PriceDiscountGroupData.GetPriceGroupsForStore(entry, store.ID);
                    if (listOfStoresPriceGroups.Count > 0)
                    {
                        var firstPriceGroup = (from x in listOfStoresPriceGroups where x.Level == listOfStoresPriceGroups.Min(y => y.Level) select x).First();
                        return firstPriceGroup.IncludeTaxForPriceGroup;
                    }
                    return false;

                case Store.StorePriceSettingsEnum.PricesIncludeTax:
                    return true;
                case Store.StorePriceSettingsEnum.PricesExcludeTax:
                default:
                    return false;
            }
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }        

        public RecordIdentifier SequenceID
        {
            get { return "R-STORE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOSTORETABLE", "STOREID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

        public List<Store> LoadStores(
              IConnectionManager entry,
              int rowFrom,
              int rowTo,
              out int totalRecordsMatching)
        {

            List<TableColumn> listColumns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "STOREID", TableAlias = "S"},
                new TableColumn {ColumnName = "NAME", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "ADDRESS", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "ADDRESSFORMAT", TableAlias = "S"},
                new TableColumn {ColumnName = "SUSPENDALLOWEOD", TableAlias = "S"},
                new TableColumn {ColumnName = "STREET", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "CITY", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "ZIPCODE", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "STATE", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "COUNTRY", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "FUNCTIONALITYPROFILE", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "NAME", TableAlias = "FP",IsNull = true,NullValue = "''",ColumnAlias = "FUNCTIONALITYPROFILEDESCRIPTION"},
                new TableColumn {ColumnName = "CURRENCY", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "CULTURENAME", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "TAXGROUP", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "TAXGROUPNAME", TableAlias = "TGH",IsNull = true,NullValue = "''",ColumnAlias = "TAXGROUPNAME"},
                new TableColumn {ColumnName = "SQLSERVERNAME", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "DATABASENAME", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "WINDOWSAUTHENTICATION", TableAlias = "S",IsNull = true,NullValue = "1"},
                new TableColumn {ColumnName = "USERNAME", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "PASSWORD", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "ACCOUNTNUM", TableAlias = "C",IsNull = true,NullValue = "''",ColumnAlias ="DEFAULTCUSTACCOUNT" },
                new TableColumn {ColumnName = "USEDEFAULTCUSTACCOUNT", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "NAME", TableAlias = "C",IsNull = true,NullValue = "''",ColumnAlias = "DEFAULTCUSTACCOUNTNAME"},
                new TableColumn {ColumnName = "KEYEDINPRICECONTAINSTAX", TableAlias = "S",IsNull = true,NullValue = "1"},
                new TableColumn {ColumnName = "TXT", TableAlias = "CUR",IsNull = true,NullValue = "''",ColumnAlias = "CURRENCYDESCRIPTION"},
                new TableColumn {ColumnName = "LAYOUTID", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "NAME", TableAlias = "LP",IsNull = true,NullValue = "''", ColumnAlias = "TILLLAYOUTNAME"},
                new TableColumn {ColumnName = "MAXIMUMPOSTINGDIFFERENCE", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "MAXTRANSACTIONDIFFERENCEAMOUNT", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "TENDERDECLARATIONCALCULATION", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "CALCDISCFROM", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "DISPLAYDISCOUNTINCLTAX", TableAlias = "S",IsNull = true,NullValue = "1"},
                new TableColumn {ColumnName = "STOREPRICESETTING", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "TRANSACTIONSERVICEPROFILE", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "NAME", TableAlias = "TP",IsNull = true,NullValue = "''",ColumnAlias = "TRANSACTIONSERVICEPROFILENAME"},
                new TableColumn {ColumnName = "USETAXGROUPFROM", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "STYLEPROFILE", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "KEYBOARDCODE", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "LAYOUTNAME", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "KITCHENMANAGERPROFILEID", TableAlias = "S",IsNull = true,NullValue = "CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER)"},
                new TableColumn {ColumnName = "NAME", TableAlias = "KP",IsNull = true,NullValue = "''", ColumnAlias = "KITCHENMANAGERPROFILENAME"},
                new TableColumn {ColumnName = "USETAXROUNDING", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "DISPLAYBALANCEWITHTAX", TableAlias = "S",IsNull = true,NullValue = "1"},
                new TableColumn {ColumnName = "RECEIPTPROFILEID", TableAlias = "S"},
                new TableColumn {ColumnName = "RECEIPTEMAILPROFILEID", TableAlias = "S"},
                new TableColumn {ColumnName = "DESCRIPTION", TableAlias = "RP",IsNull = true,NullValue = "''", ColumnAlias ="RECEIPTPROFILEDESCRIPTION" },
                new TableColumn {ColumnName = "DESCRIPTION", TableAlias = "EP",IsNull = true,NullValue = "''",ColumnAlias = "EMAILPROFILEDESCRIPTION"},
                new TableColumn {ColumnName = "RETURNREASONCODEID", TableAlias = "S", IsNull = true, NullValue = "''"},
                new TableColumn {ColumnName = "REGIONID", TableAlias = "S", IsNull = true, NullValue = "''"},
                new TableColumn {ColumnName = "DESCRIPTION", TableAlias = "RG", IsNull = true, NullValue = "''", ColumnAlias = "REGIONDESCRIPTION"},
                new TableColumn {ColumnName = "PICTUREID", TableAlias = "S"},
                new TableColumn {ColumnName = "FORMINFOFIELD1", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "FORMINFOFIELD2", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "FORMINFOFIELD3", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "FORMINFOFIELD4", TableAlias = "S",IsNull = true,NullValue = "''"},
                new TableColumn {ColumnName = "BARCODESYMBOLOGY", TableAlias = "S",IsNull = true,NullValue = "2"},
                new TableColumn {ColumnName = "OPERATIONAUDITSETTING", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "STARTAMOUNT", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "RETURNSPRINTEDTWICE", TableAlias = "S",IsNull = true,NullValue = "0"},
                new TableColumn {ColumnName = "TENDERRECEIPTSAREREPRINTED", TableAlias = "S",IsNull = true,NullValue = "0"},                
                new TableColumn {ColumnName = "RECEIPTLOGOSIZE", TableAlias = "S"},
                new TableColumn {ColumnName = "DEFAULTDELIVERYTIME", TableAlias = "S"},
                new TableColumn {ColumnName = "DELIVERYDAYSTYPE", TableAlias = "S"},
                new TableColumn {ColumnName = "OMNIMAINMENU", TableAlias = "FP", IsNull = true, NullValue = "''", ColumnAlias = "OMNIMAINMENU"},
        };
                       

            using (var cmd = entry.Connection.CreateCommand())
            {

                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in listColumns)
                {
                    columns.Add(selectionColumn);
                }
                columns.Add(new TableColumn
                {
                    ColumnName = "ROW_NUMBER() OVER(order by S.STOREID)",
                    ColumnAlias = "ROW"
                });
                columns.Add(new TableColumn
                {
                    ColumnName =
                        "COUNT(1) OVER(ORDER BY S.STOREID RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING)",
                    ColumnAlias = "ROW_COUNT"
                });


                List<Condition> externalConditions = new List<Condition>();
                externalConditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "columns.ROW BETWEEN @rowFrom AND @rowTo"
                });
                List<Join> joins = new List<Join>();

                joins.Add(new Join
                {
                    Condition = " S.DEFAULTCUSTACCOUNT = C.ACCOUNTNUM AND S.DATAAREAID = C.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "CUSTOMER",
                    TableAlias = "C"
                });

                joins.Add(new Join
                {
                    Condition = " S.FUNCTIONALITYPROFILE = FP.PROFILEID AND S.DATAAREAID = FP.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "POSFUNCTIONALITYPROFILE",
                    TableAlias = "FP"
                });
                joins.Add(new Join
                {
                    Condition = " S.TAXGROUP = TGH.TAXGROUP AND S.DATAAREAID = TGH.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "TAXGROUPHEADING",
                    TableAlias = "TGH"
                });
                joins.Add(new Join
                {
                    Condition = " S.TRANSACTIONSERVICEPROFILE = TP.PROFILEID AND TP.DATAAREAID = S.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "POSTRANSACTIONSERVICEPROFILE",
                    TableAlias = "TP"
                });
                joins.Add(new Join
                {
                    Condition = " S.CURRENCY = CUR.CURRENCYCODE AND S.DATAAREAID = CUR.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "CURRENCY",
                    TableAlias = "CUR"
                });
                joins.Add(new Join
                {
                    Condition = " S.LAYOUTID = LP.LAYOUTID AND LP.DATAAREAID = S.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "POSISTILLLAYOUT",
                    TableAlias = "LP"
                });
                joins.Add(new Join
                {
                    Condition = " S.KITCHENMANAGERPROFILEID = KP.ID AND S.DATAAREAID = KP.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "KITCHENDISPLAYTRANSACTIONPROFILE",
                    TableAlias = "KP"
                });
                joins.Add(new Join
                {
                    Condition = " S.RECEIPTPROFILEID = RP.PROFILEID AND S.DATAAREAID = RP.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "POSFORMPROFILE",
                    TableAlias = "RP"
                });
                joins.Add(new Join
                {
                    Condition = " S.RECEIPTEMAILPROFILEID = EP.PROFILEID AND S.DATAAREAID = RP.DATAAREAID",
                    JoinType = "LEFT OUTER",
                    Table = "POSFORMPROFILE",
                    TableAlias = "EP"
                });
                joins.Add(new Join
                {
                    Condition = " S.REGIONID = RG.ID",
                    JoinType = "LEFT OUTER",
                    Table = "REGION",
                    TableAlias = "RG"
                });


                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RBOSTORETABLE", "S", "columns"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "columns"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),                   
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Empty);

                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                int matchingRecords = 0;

                var reply = Execute<Store, int>(entry, cmd, CommandType.Text, ref matchingRecords, false,
                    PopulateStoreNormalWithCount);

                totalRecordsMatching = matchingRecords;
                return reply;
            }
        }

        public bool KdsProfileInUse(IConnectionManager entry, RecordIdentifier kdsProfileId)
        {
            return RecordExists(entry, "RBOSTORETABLE", "KITCHENMANAGERPROFILEID", kdsProfileId);
        }
    }
}
