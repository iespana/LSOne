using System;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector.DataProviders
{
    internal class SystemData : SqlServerDataProviderBase
    {
        public static void SetSystemSetting(IConnectionManager entry, Guid settingIdentifier, string value,
                                            SettingType settingType)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.AdministrationMaintainSettings);

            var cmd = entry.Connection.CreateCommand("spMAINT_SetSystemSetting_1_0");

            MakeParam(cmd, "SettingGuid", settingIdentifier);
            MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
            MakeParam(cmd, "Type", settingType.Type);
            MakeParam(cmd, "Value", value);

            entry.Connection.ExecuteNonQuery(cmd, false);
        }

        public static Setting GetSystemSetting(IConnectionManager entry, Guid settingIdentifier)
        {
            var cmd = entry.Connection.CreateCommand("spMAINT_GetSystemSetting_1_0");

            MakeParam(cmd, "SettingGuid", settingIdentifier);
            MakeParam(cmd, "dataareaID", entry.Connection.DataAreaId);
            var typeParam = MakeParam(cmd, "Type", "", SqlDbType.UniqueIdentifier, ParameterDirection.Output);
            var valueParam = MakeParam(cmd, "Value", "", SqlDbType.NVarChar, ParameterDirection.Output, 50);

            entry.Connection.ExecuteNonQuery(cmd, true);

            return new Setting(false, "", (string) valueParam.Value, SettingType.Resolve((Guid) typeParam.Value));
        }

        internal static SiteServiceProfile GetSiteServiceProfile(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select PROFILEID, ISNULL(NAME,'') as NAME," +
                    "ISNULL(AOSINSTANCE,'') as AOSINSTANCE," +
                    "ISNULL(AOSSERVER,'') as AOSSERVER," +
                    "ISNULL(AOSPORT,'0') as AOSPORT," +
                    "ISNULL(CENTRALTABLESERVER,'') as CENTRALTABLESERVER," +
                    "ISNULL(CENTRALTABLESERVERPORT,'0') as CENTRALTABLESERVERPORT," +
                    "ISNULL(USERNAME,'') as USERNAME," +
                    "ISNULL(PASSWORD,'') as PASSWORD," +
                    "ISNULL(COMPANY,'') as COMPANY," +
                    "ISNULL(DOMAIN,'') as DOMAIN,ISNULL(AXVERSION,-1) as AXVERSION," +
                    "ISNULL(CONFIGURATION,'') as CONFIGURATION," +
                    "ISNULL(LANGUAGE,'') as LANGUAGE," +
                    "ISNULL(TSCUSTOMER,0) as TSCUSTOMER," +
                    "ISNULL(TSSTAFF,0) as TSSTAFF," +
                    "ISNULL(TSINVENTORYLOOKUP,0) as TSINVENTORYLOOKUP, " +
                    "ISNULL(ISSUEGIFTCARDOPTION,0) as ISSUEGIFTCARDOPTION, " +
                    "ISNULL(USEGIFTCARDS,0) as USEGIFTCARDS, " +
                    "ISNULL(USECENTRALSUSPENSION,0) as USECENTRALSUSPENSION, " +
                    "ISNULL(USERCONFIRMATION,0) as USERCONFIRMATION, " +
                    "ISNULL(USECREDITVOUCHERS,0) as USECREDITVOUCHERS, " +
                    "ISNULL(CUSTOMERADDDEFAULTTAXGROUPNAME, '') AS CUSTOMERADDDEFAULTTAXGROUPNAME, " +
                    "ISNULL(CUSTOMERADDDEFAULTTAXGROUP, '') AS CUSTOMERADDDEFAULTTAXGROUP, " +
                    "ISNULL(CASHCUSTOMERSETTING, 0) AS CASHCUSTOMERSETTING, " +
                    "ISNULL(GIFTCARDREFILLSETTING, 0) AS GIFTCARDREFILLSETTING, " +
                    "ISNULL(MAXIMUMGIFTCARDAMOUNT, 0) AS MAXIMUMGIFTCARDAMOUNT,  " +                    
                    "from POSTRANSACTIONSERVICEPROFILE " +
                    "where DATAAREAID = @dataAreaId and PROFILEID = @id";
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string) id);

                return Get<SiteServiceProfile>(entry, cmd, id, PopulateProfile, CacheType.CacheTypeNone,
                                               UsageIntentEnum.Normal);
            }
        }

        private static void PopulateProfile(IDataReader dr, SiteServiceProfile profile)
        {
            profile.ID = (string) dr["PROFILEID"];
            profile.Text = (string) dr["NAME"];
            profile.AosInstance = (string) dr["AOSINSTANCE"];
            profile.AosServer = (string) dr["AOSSERVER"];
            if (profile.AosServer != string.Empty)
            {
                profile.UseAxTransactionServices = true;
            }
            try
            {
                profile.AosPort = Convert.ToInt32((string) dr["AOSPORT"]);
            }
            catch (Exception)
            {
                profile.AosPort = 0;
            }

            profile.SiteServiceAddress = (string) dr["CENTRALTABLESERVER"];

            try
            {
                profile.SiteServicePortNumber = Convert.ToInt32((string) dr["CENTRALTABLESERVERPORT"]);
            }
            catch (Exception)
            {
                profile.SiteServicePortNumber = 0;
            }

            profile.UserName = (string) dr["USERNAME"];
            profile.Password = (string) dr["PASSWORD"];
            profile.Company = (string) dr["COMPANY"];
            profile.Domain = (string) dr["DOMAIN"];
            profile.AxVersion = (int) dr["AXVERSION"];
            profile.Configuration = (string) dr["CONFIGURATION"];
            profile.Language = (string) dr["LANGUAGE"];
            profile.CheckCustomer = ((byte) dr["TSCUSTOMER"] != 0);
            profile.CheckStaff = ((byte) dr["TSSTAFF"] != 0);
            profile.UseInventoryLookup = ((byte) dr["TSINVENTORYLOOKUP"] != 0);
            profile.IssueGiftCardOption = (SiteServiceProfile.IssueGiftCardOptionEnum) ((int) dr["ISSUEGIFTCARDOPTION"]);
            profile.UseGiftCards = ((byte) dr["USEGIFTCARDS"] != 0);
            profile.UseCentralSuspensions = ((byte) dr["USECENTRALSUSPENSION"] != 0);
            profile.UserConfirmation = ((byte) dr["USERCONFIRMATION"] != 0);
            profile.UseCreditVouchers = ((byte) dr["USECREDITVOUCHERS"] != 0);
            profile.NewCustomerDefaultTaxGroup = (string) dr["CUSTOMERADDDEFAULTTAXGROUP"];
            profile.NewCustomerDefaultTaxGroupName = (string) dr["CUSTOMERADDDEFAULTTAXGROUPNAME"];
            profile.CashCustomerSetting = (SiteServiceProfile.CashCustomerSettingEnum) dr["CASHCUSTOMERSETTING"];
            profile.GiftCardRefillSetting = (SiteServiceProfile.GiftCardRefillSettingEnum) dr["GIFTCARDREFILLSETTING"];
            profile.MaximumGiftCardAmount = (decimal) dr["MAXIMUMGIFTCARDAMOUNT"];            
        }

        internal static void GetStoreServerHost(IConnectionManager entry, ref string hostName, ref UInt16 port)
        {
            hostName = string.Empty;
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select  ISNULL(p.SITESERVICEPROFILE,'') AS SITESERVICEPROFILE " +
                                  "from RBOPARAMETERS p " +
                                  "where p.DATAAREAID = @dataAreaID and p.KEY_ = 0";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                IDataReader dr = null;
                string siteServiceProfile = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);
                    
                    if (dr.Read())
                    {
                        siteServiceProfile = (string) dr["SITESERVICEPROFILE"];
                       
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
                SiteServiceProfile profile = null;

                if (!string.IsNullOrEmpty(siteServiceProfile))
                {
                    profile = GetSiteServiceProfile(entry, siteServiceProfile);
                }
                if (profile != null)
                {
                    hostName = profile.SiteServiceAddress;
                    port = (ushort)profile.SiteServicePortNumber;
                }
            }
        }
    }
}
