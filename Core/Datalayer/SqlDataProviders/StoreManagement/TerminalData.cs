using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using LSOne.DataLayer.BusinessObjects;
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

namespace LSOne.DataLayer.SqlDataProviders.StoreManagement
{
    public class TerminalData : SqlServerDataProviderBase, ITerminalData
    {

        private string BaseListSQL
        {
            get
            {
                return @"Select t.TerminalID, 
                             ISNULL(t.NAME,'') as NAME,
                             ISNULL(s.STOREID,'') as STOREID, 
                             ISNULL(s.NAME,'') as STORENAME,
                             ISNULL(t.ACTIVATED, 0) as ACTIVATED,
                             ISNULL(t.LASTACTIVATEDDATE, 0) as LASTACTIVATEDDATE 
                        from RBOTERMINALTABLE t 
                        right outer join RBOSTORETABLE s on t.STOREID =  s.STOREID and t.DATAAREAID = s.DATAAREAID ";
            }
        }

        private string BaseFullSQL(UsageIntentEnum usageIntent)
        {
            if (usageIntent == UsageIntentEnum.Normal || usageIntent == UsageIntentEnum.Reporting)
            {
                return @"Select t.TerminalID, 
                            ISNULL(t.NAME, '') as NAME, 
                            ISNULL(s.STOREID, '') as STOREID, 
                            t.SUSPENDALLOWEOD,
                            ISNULL(s.NAME, '') as StoreName,
                            ISNULL(t.AutoLogOffTimeout, 0) as AutoLogOffTimeout,
                            ISNULL(hp.PROFILEID, '') as HardwareProfile,
                            ISNULL(vp.PROFILEID, '') as VisualProfile,
                            ISNULL(vp.NAME, '') as VisualProfileName,
                            ISNULL(hp.NAME, '') as HardwareProfileName,
                            ISNULL(fp.PROFILEID, '') as FunctionalityProfile,
                            ISNULL(fp.NAME, '') as FunctionalityProfileName,
                            ISNULL(t.CustomerDisplayText1, '') as CustomerDisplayText1,
                            ISNULL(t.CustomerDisplayText2, '') as CustomerDisplayText2,
                            ISNULL(t.OPENDRAWERATLILO, 0) as OPENDRAWERATLILO,
                            ISNULL(lp.LAYOUTID, '') as LAYOUTID,
                            ISNULL(lp.Name, '') as LayoutName,
                            ISNULL(t.STANDALONE, 0) as STANDALONE,
                            ISNULL(t.EXITAFTEREACHTRANSACTION, 0) as EXITAFTEREACHTRANSACTION,
                            ISNULL(t.UpdateServicePort, 0) as UpdateServicePort,
                            ISNULL(t.transactionIDNumberSequence, '') as transactionIDNumberSequence,
                            ISNULL(t.TRANSACTIONSERVICEPROFILE, '') as TRANSACTIONSERVICEPROFILE,
                            ISNULL(tp1.NAME, '') as TRANSACTIONSERVICEPROFILENAME,
                            ISNULL(t.HOSPTRANSSTORESERVERPROFILE, '') as HOSPTRANSSTORESERVERPROFILE,
                            ISNULL(tp2.NAME, '') as HOSPTRANSSTORESERVERPROFILENAME,
                            ISNULL(t.IPAddress, '') as IPAddress,
                            ISNULL(t.EFTTERMINALID, '') as EFTTERMINALID,
                            ISNULL(t.EFTSTOREID, '') as EFTSTOREID, 
                            ISNULL(t.EFTCUSTOMFIELD1, '') as EFTCUSTOMFIELD1, 
                            ISNULL(t.EFTCUSTOMFIELD2, '') as EFTCUSTOMFIELD2, 
                            LSPAYUSELOCALSERVER,
                            LSPAYSERVERNAME,
                            LSPAYSERVERPORT,
                            LSPAYPLUGINID,
                            LSPAYPLUGINNAME,
                            LSPAYSUPPORTREFREFUND,
                            ISNULL(t.SALESTYPEFILTER, '') as SALESTYPEFILTER, 
                            ISNULL(t.AUTOLOCKTERMINALTIMEOUT, 0) as AUTOLOCKTERMINALTIMEOUT, 
                            ISNULL(t.RECEIPTIDNUMBERSEQUENCE, '') as RECEIPTIDNUMBERSEQUENCE,
                            ISNULL(t.DatabaseName, '') as DatabaseName,
                            ISNULL(t.DatabaseServer, '') as DatabaseServer,
                            ISNULL(t.DatabaseUserName, '') as DatabaseUserName,
                            ISNULL(t.DatabasePassword, '') as DatabasePassword,
                            ISNULL(t.KITCHENMANAGERPROFILEID, CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER)) as KITCHENMANAGERPROFILEID,
                            ISNULL(t.FORMINFOFIELD1, '') as FORMINFOFIELD1,
                            ISNULL(t.FORMINFOFIELD2, '') as FORMINFOFIELD2,
                            ISNULL(t.FORMINFOFIELD3, '') as FORMINFOFIELD3,
                            ISNULL(t.FORMINFOFIELD4, '') as FORMINFOFIELD4,
                            ISNULL(t.SWITCHUSERWHENENTERINGPOS, 0) AS SWITCHUSERWHENENTERINGPOS,
                            ISNULL(kp.NAME, '') as KITCHENMANAGERPROFILENAME,
                            ISNULL(t.STATEMENTPOSTING, 0) as STATEMENTPOSTING,
                            ISNULL(fp.OMNIMAINMENU, '') AS OMNIMAINMENU,
                            t.INCLUDEINSTATEMENT,
                            ISNULL(t.ACTIVATED, 0) as ACTIVATED,
                            ISNULL(t.LASTACTIVATEDDATE, 0) as LASTACTIVATEDDATE
                            from RBOTERMINALTABLE t
                            left outer join RBOSTORETABLE s on t.STOREID = s.STOREID and t.DATAAREAID = s.DATAAREAID
                            left outer join POSVISUALPROFILE vp on t.VisualProfile = vp.PROFILEID and vp.DATAAREAID = t.DATAAREAID
                            left outer join POSHARDWAREPROFILE hp on t.HardwareProfile = hp.PROFILEID and hp.DATAAREAID = t.DATAAREAID
                            left outer join POSISTILLLAYOUT lp on t.LAYOUTID = lp.LAYOUTID and lp.DATAAREAID = t.DATAAREAID
                            left outer join POSTRANSACTIONSERVICEPROFILE tp1 on t.TRANSACTIONSERVICEPROFILE = tp1.PROFILEID and tp1.DATAAREAID = t.DATAAREAID
                            left outer join POSTRANSACTIONSERVICEPROFILE tp2 on t.HOSPTRANSSTORESERVERPROFILE = tp2.PROFILEID and tp2.DATAAREAID = t.DATAAREAID
                            left outer join POSFUNCTIONALITYPROFILE fp on t.FUNCTIONALITYPROFILE = fp.PROFILEID and fp.DATAAREAID = t.DATAAREAID
                            left outer join KITCHENDISPLAYTRANSACTIONPROFILE kp on t.KITCHENMANAGERPROFILEID = kp.ID and kp.DATAAREAID = t.DATAAREAID ";
            }           
            else
            {
                return @"Select t.TerminalID, 
                            ISNULL(t.NAME,'') as NAME, 
                            ISNULL(t.STOREID,'') as STOREID, 
                            t.SUSPENDALLOWEOD,
                            ISNULL(t.AUTOLOGOFFTIMEOUT,0) as AutoLogOffTimeout,
                            ISNULL(t.HARDWAREPROFILE,'') as HardwareProfile,
                            ISNULL(t.VISUALPROFILE,'') as VisualProfile,
                            ISNULL(t.FUNCTIONALITYPROFILE,'') as FunctionalityProfile,
                            ISNULL(t.CUSTOMERDISPLAYTEXT1,'') as CustomerDisplayText1,
                            ISNULL(t.CUSTOMERDISPLAYTEXT2,'') as CustomerDisplayText2,
                            ISNULL(t.OPENDRAWERATLILO,0) as OPENDRAWERATLILO,
                            ISNULL(t.LAYOUTID,'') as LAYOUTID,
                            ISNULL(t.STANDALONE,0) as STANDALONE,
                            ISNULL(t.EXITAFTEREACHTRANSACTION,0) as EXITAFTEREACHTRANSACTION,
                            ISNULL(t.UPDATESERVICEPORT,0) as UpdateServicePort,
                            ISNULL(t.TRANSACTIONIDNUMBERSEQUENCE,'') as transactionIDNumberSequence,
                            ISNULL(t.TRANSACTIONSERVICEPROFILE,'') as TRANSACTIONSERVICEPROFILE,
                            ISNULL(t.TRANSACTIONSERVICEPROFILE,'') as TRANSACTIONSERVICEPROFILENAME,
                            ISNULL(t.HOSPTRANSSTORESERVERPROFILE,'') as HOSPTRANSSTORESERVERPROFILE,
                            ISNULL(t.IPAddress,'') as IPAddress,
                            ISNULL(t.EFTTERMINALID,'') as EFTTERMINALID,
                            ISNULL(t.EFTSTOREID,'') as EFTSTOREID, 
                            ISNULL(t.EFTCUSTOMFIELD1,'') as EFTCUSTOMFIELD1, 
                            ISNULL(t.EFTCUSTOMFIELD2,'') as EFTCUSTOMFIELD2, 
                            LSPAYUSELOCALSERVER,
                            LSPAYSERVERNAME,
                            LSPAYSERVERPORT,
                            LSPAYPLUGINID,
                            LSPAYPLUGINNAME,
                            LSPAYSUPPORTREFREFUND,
                            ISNULL(t.SALESTYPEFILTER,'') as SALESTYPEFILTER, 
                            ISNULL(t.AUTOLOCKTERMINALTIMEOUT,0) as AUTOLOCKTERMINALTIMEOUT, 
                            ISNULL(t.RECEIPTIDNUMBERSEQUENCE,'') as RECEIPTIDNUMBERSEQUENCE,
                            ISNULL(t.DatabaseName,'') as DatabaseName,
                            ISNULL(t.DatabaseServer,'') as DatabaseServer,
                            ISNULL(t.DatabaseUserName,'') as DatabaseUserName,
                            ISNULL(t.DatabasePassword,'') as DatabasePassword,
                            ISNULL(t.KITCHENMANAGERPROFILEID, CAST('00000000-0000-0000-0000-000000000000' AS UNIQUEIDENTIFIER)) as KITCHENMANAGERPROFILEID ,
                            ISNULL(t.FORMINFOFIELD1,'') as FORMINFOFIELD1,
                            ISNULL(t.FORMINFOFIELD2,'') as FORMINFOFIELD2,
                            ISNULL(t.FORMINFOFIELD3,'') as FORMINFOFIELD3,
                            ISNULL(t.FORMINFOFIELD4,'') as FORMINFOFIELD4,
                            ISNULL(t.ACTIVATED, 0) as ACTIVATED,
                            ISNULL(t.LASTACTIVATEDDATE, 0) as LASTACTIVATEDDATE 
                            from RBOTERMINALTABLE t ";
            }     
        }

        public virtual List<TerminalListItem> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseListSQL + 
                    "where t.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<TerminalListItem>(entry, cmd, CommandType.Text, PopulateTerminaListItem);
            }            
        }

        public virtual string GetName(IConnectionManager entry, RecordIdentifier terminalID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(NAME, '') AS NAME
                                    FROM RBOTERMINALTABLE
                                    WHERE DATAAREAID = @dataAreaID AND TERMINALID = @terminalID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "terminalID", terminalID);
                object returnValue = entry.Connection.ExecuteScalar(cmd);
                return returnValue is DBNull ? null : (string)returnValue;
            }
        }

        private static void PopulateTerminalValidity(IDataReader dr, TerminalValidity validity)
        {
            validity.ID = (string)dr["TERMINALID"];
            validity.Text = (string)dr["NAME"];

            if (dr["VisualProfileExists"] is DBNull)
            {
                validity.VisualProfileExists = false;
            }
            else
            {
                validity.VisualProfileExists = ((string)dr["VisualProfileExists"] != "");
            }

            if (dr["HardwareProfileExists"] is DBNull)
            {
                validity.HardwareProfileExists = false;
            }
            else
            {
                validity.HardwareProfileExists = ((string)dr["HardwareProfileExists"] != "");
            }

            validity.StoreID = (string)dr["STOREID"];
        }

        public virtual List<TerminalValidity> CheckTerminalValidity(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"select t.TERMINALID, ISNULL(t.STOREID,'') as STOREID, ISNULL(t.NAME,'') as NAME,t.VISUALPROFILE as VisualProfileExists, t.HARDWAREPROFILE as HardwareProfileExists  
                                    from RBOTERMINALTABLE t";

                return Execute<TerminalValidity>(entry, cmd, CommandType.Text, PopulateTerminalValidity);
            }
        }

        public virtual List<TerminalListItem> GetList(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseListSQL +
                    "where t.DATAAREAID = @dataAreaId and t.STOREID = @storeID order by t.TERMINALID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string) storeID);

                return Execute<TerminalListItem>(entry, cmd, CommandType.Text, PopulateTerminaListItem);
            }
        }

        public virtual List<TerminalListItem> GetAvailableList(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseListSQL +
                    "where t.DATAAREAID = @dataAreaId and t.STOREID = @storeID and ISNULL(t.ACTIVATED, 0)  = 0 order by t.TERMINALID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string)storeID);

                return Execute<TerminalListItem>(entry, cmd, CommandType.Text, PopulateTerminaListItem);
            }
        }

        public virtual List<DataEntity> GetHospitalityTerminalList(IConnectionManager entry, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                
                cmd.CommandText = @"SELECT T.TERMINALID, ISNULL(T.NAME, '') AS NAME
                                    FROM RBOTERMINALTABLE T
                                    JOIN POSFUNCTIONALITYPROFILE F ON F.PROFILEID = T.FUNCTIONALITYPROFILE AND F.DATAAREAID = T.DATAAREAID
                                    WHERE T.STOREID = @STOREID
                                    AND F.ISHOSPITALITYPROFILE = 1
                                    AND T.DATAAREAID = @DATAAREAID
                                    ORDER BY TERMINALID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "STOREID", (string)storeID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "TERMINALID");
            }
        }

        private static void PopulateTerminaListItem(IDataReader dr, TerminalListItem item)
        {
            item.ID = (string)dr["TerminalID"];
            item.Text = (string)dr["NAME"];
            item.StoreID = (string)dr["STOREID"];
            item.StoreName = (string)dr["STORENAME"];
        }

        private static string ResolveSort(TerminalListItem.SortEnum sortEnum, bool sortBackwards)
        {
            string sortString = "";

            switch (sortEnum)
            {
                case TerminalListItem.SortEnum.STORENAME:
                    sortString = "order by STORENAME ASC ";
                    break;
                case TerminalListItem.SortEnum.ID:
                    sortString = "order by t.TERMINALID ASC ";
                    break;
                case TerminalListItem.SortEnum.NAME:
                    sortString = "order by NAME ASC ";
                    break;
            }
            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        public List<TerminalListItem> Search(IConnectionManager entry, RecordIdentifier id, string description,
                                                    int maxCount)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                //ValidateSecurity(entry, BusinessObjects.Permission.TerminalView);

                string maxCountSql = "";
                if (maxCount > 0)
                    maxCountSql = " Top " + maxCount;

                cmd.CommandText = "Select " + maxCountSql + " t.TerminalID, ISNULL(t.NAME,'') as NAME," +
                                    "ISNULL(s.STOREID,'') as STOREID, ISNULL(s.NAME,'') as STORENAME, " +
                                    "ISNULL(t.ACTIVATED, 0) as ACTIVATED, " +
                                    "ISNULL(t.LASTACTIVATEDDATE, 0) as LASTACTIVATEDDATE " +
                                    "from RBOTERMINALTABLE t " +
                                    "right outer join RBOSTORETABLE s on t.STOREID =  s.STOREID and t.DATAAREAID = s.DATAAREAID " +
                                    "where t.DATAAREAID = @dataAreaId and (t.TerminalID Like @id or t.NAME Like @name) order by t.TerminalID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", "%" + id + "%");
                MakeParam(cmd, "name", "%" + description + "%");

                return Execute<TerminalListItem>(entry, cmd, CommandType.Text, PopulateTerminaListItem);
            }
        }

        public virtual List<TerminalListItem> GetTerminals(IConnectionManager entry, RecordIdentifier storeid)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select t.TerminalID, ISNULL(t.NAME,'') as NAME," +
                                  "ISNULL(s.STOREID,'') as STOREID, ISNULL(s.NAME,'') as STORENAME, " +
                                  "ISNULL(t.ACTIVATED, 0) as ACTIVATED, " +
                                  "ISNULL(t.LASTACTIVATEDDATE, 0) as LASTACTIVATEDDATE "+
                                  "from RBOTERMINALTABLE t " +
                                  "right outer join RBOSTORETABLE s on t.STOREID =  s.STOREID and t.DATAAREAID = s.DATAAREAID " +
                                  "Where t.STOREID = @storeID AND t.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string) storeid);

                return Execute<TerminalListItem>(entry, cmd, CommandType.Text, PopulateTerminaListItem);
            }
        }

        public List<TerminalListItem> GetAllTerminals(IConnectionManager entry, bool sortAscending,
                                                             TerminalListItem.SortEnum sortEnum)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select t.TerminalID, ISNULL(t.NAME,'') as NAME, " +
                                  "ISNULL(s.STOREID,'') as STOREID, ISNULL(s.NAME,'') as STORENAME ," +
                                  "ISNULL(t.ACTIVATED, 0) as ACTIVATED, " +
                                  "ISNULL(t.LASTACTIVATEDDATE, 0) as LASTACTIVATEDDATE " +
                                  "from RBOTERMINALTABLE t " +
                                  "left outer join RBOSTORETABLE s on t.STOREID =  s.STOREID and t.DATAAREAID = s.DATAAREAID " +
                                  "Where t.DATAAREAID = @dataAreaId " +
                    ResolveSort(sortEnum, sortAscending);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<TerminalListItem>(entry, cmd, CommandType.Text, PopulateTerminaListItem);
            }
        }

        public virtual List<Terminal> GetAllTerminals(IConnectionManager entry, UsageIntentEnum usageIntent)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseFullSQL(usageIntent);

                if (usageIntent == UsageIntentEnum.Minimal)
                {
                    return Execute<Terminal>(entry, cmd, CommandType.Text, PopulateMinimal);
                }
                else
                {
                    return Execute<Terminal>(entry, cmd, CommandType.Text, PopulateTerminalData);
                }
            }
        }

        public virtual Terminal Get(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID, CacheType cache = CacheType.CacheTypeNone, UsageIntentEnum usageIntent = UsageIntentEnum.Normal)
        {
            if (usageIntent == UsageIntentEnum.Normal || usageIntent == UsageIntentEnum.Reporting)
            {
                using (var cmd = entry.Connection.CreateCommand())
                {
                    cmd.CommandText = BaseFullSQL(usageIntent) + "\r\n" + 
                            "where t.DATAAREAID = @dataAreaId and TerminalID = @terminalId";

                    if(storeID != null && storeID != RecordIdentifier.Empty)
                    {
                        cmd.CommandText += " and t.STOREID = @storeId";
                        MakeParam(cmd, "storeId", (string)storeID);
                    }

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "terminalId", (string)terminalID);
                    return Get<Terminal>(entry, cmd, new RecordIdentifier(terminalID, storeID), PopulateTerminalData, cache, usageIntent);
                }
            }

            if (usageIntent == UsageIntentEnum.Minimal)
            {
                using (var cmd = entry.Connection.CreateCommand())
                {
                    cmd.CommandText = BaseFullSQL(usageIntent) + "\r\n" + 
                            "where t.DATAAREAID = @dataAreaId and TerminalID = @terminalId";

                    if (storeID != RecordIdentifier.Empty)
                    {
                        cmd.CommandText += " and t.STOREID = @storeId";
                        MakeParam(cmd, "storeId", (string)storeID);
                    }

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "terminalId", (string)terminalID);
                    return Get<Terminal>(entry, cmd, new RecordIdentifier(terminalID, storeID), PopulateMinimal, cache, usageIntent);
                }
            }
            return null;
        }

        public virtual void PopulateTerminalData(IDataReader dr, Terminal terminal)
        {
            PopulateMinimal(dr, terminal);

            terminal.Name = (string)dr["NAME"];
            terminal.ID = (string)dr["TERMINALID"];
            terminal.StoreName = (string)dr["StoreName"];
            terminal.VisualProfileName = (string)dr["VisualProfileName"];
            terminal.HardwareProfileName = (string)dr["HardwareProfileName"];
            terminal.FunctionalityProfileName = (string)dr["FunctionalityProfileName"];
            terminal.LayoutName = (string)dr["LayoutName"];
            terminal.HospTransServiceProfileName = (string)dr["HOSPTRANSSTORESERVERPROFILENAME"];
            terminal.KitchenServiceProfileName = (string)dr["KITCHENMANAGERPROFILENAME"];
            terminal.SwitchUserWhenEnteringPOS = ((byte)dr["SWITCHUSERWHENENTERINGPOS"] != 0);
            terminal.AllowTerminalStatementPosting = (AllowTerminalStatementPostingEnum)((int)dr["STATEMENTPOSTING"]);
            terminal.IncludeTerminalInStatement = (bool)(dr["INCLUDEINSTATEMENT"]);
            terminal.InventoryMainMenuID = (string)dr["OMNIMAINMENU"];
        }

        public virtual void PopulateMinimal(IDataReader dr, Terminal terminal)
        {
            terminal.StoreID = (string)dr["STOREID"];
            terminal.AutoLogOffTimeout = (int)dr["AutoLogOffTimeout"];
            terminal.AutoLockTimeout = (int)dr["AUTOLOCKTERMINALTIMEOUT"];
            terminal.HardwareProfileID = (string)dr["HardwareProfile"];
            terminal.VisualProfileID = (string)dr["VisualProfile"];
            terminal.FunctionalityProfileID = (string)dr["FunctionalityProfile"];
            terminal.CustomerDisplayText1 = (string)dr["CustomerDisplayText1"];
            terminal.CustomerDisplayText2 = (string)dr["CustomerDisplayText2"];
            terminal.OpenDrawerAtLoginLogout = ((byte)dr["OPENDRAWERATLILO"] != 0);
            terminal.LayoutID = (string)dr["LayoutID"];            
            terminal.ExitAfterEachTransaction = ((byte)dr["EXITAFTEREACHTRANSACTION"] != 0);
            terminal.UpdateServicePort = (int)dr["UpdateServicePort"];
            terminal.TransactionServiceProfileID = (string)dr["TRANSACTIONSERVICEPROFILE"];
            terminal.TransactionServiceProfileName = (string)dr["TRANSACTIONSERVICEPROFILENAME"];
            terminal.HospTransServiceProfileID = (string)dr["HOSPTRANSSTORESERVERPROFILE"];
            terminal.TransactionIDNumberSequence = (string)dr["transactionIDNumberSequence"];
            terminal.IPAddress = (string)dr["IPAddress"];
            terminal.EftStoreID = (string)dr["EFTSTOREID"];
            terminal.EftTerminalID = (string)dr["EFTTERMINALID"];
            terminal.EftCustomField1 = (string)dr["EFTCUSTOMFIELD1"];
            terminal.EftCustomField2 = (string)dr["EFTCUSTOMFIELD2"];
            terminal.LSPayUseLocalServer = (bool)dr["LSPAYUSELOCALSERVER"];
            terminal.LSPayServerName = (string)dr["LSPAYSERVERNAME"];
            terminal.LSPayServerPort = (int)dr["LSPAYSERVERPORT"];
            terminal.LSPaySupportReferenceRefund = (bool)dr["LSPAYSUPPORTREFREFUND"];
            terminal.LSPayPlugin = new DataEntity((string)dr["LSPAYPLUGINID"], (string)dr["LSPAYPLUGINNAME"]);
            terminal.SalesTypeFilter = (string)dr["SALESTYPEFILTER"];
            terminal.SuspendedTransactionsStatementPosting = (SuspendedTransactionsStatementPostingEnum)dr["SUSPENDALLOWEOD"];
            terminal.ReceiptIDNumberSequence = (string)dr["RECEIPTIDNUMBERSEQUENCE"];
            terminal.DatabaseName = (string)dr["DatabaseName"];
            terminal.DatabaseServer = (string)dr["DatabaseServer"];
            terminal.DatabaseUserName = (string)dr["DatabaseUserName"];
            terminal.DatabaseUserPassword = (string)dr["DatabasePassword"];
            terminal.KitchenServiceProfileID = (Guid)dr["KITCHENMANAGERPROFILEID"];
            terminal.FormInfoField1 = AsString(dr["FORMINFOFIELD1"]);
            terminal.FormInfoField2 = AsString(dr["FORMINFOFIELD2"]);
            terminal.FormInfoField3 = AsString(dr["FORMINFOFIELD3"]);
            terminal.FormInfoField4 = AsString(dr["FORMINFOFIELD4"]);
            terminal.Activated = (bool)(dr["ACTIVATED"]);
            terminal.LastActivatedDate = (DateTime)dr["LASTACTIVATEDDATE"]; 
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier terminalAndStoreID)
        {
            DeleteRecord(entry, "RBOTERMINALTABLE", new string[]{"TERMINALID", "STOREID"}, terminalAndStoreID, BusinessObjects.Permission.TerminalEdit);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier terminalID)
        {
            return RecordExists<Terminal>(entry, "RBOTERMINALTABLE", "TerminalID", terminalID);
        }

        /// <summary>
        /// Checks wether a terminal exists with the given terminal and store combination
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminalID">The terminal ID to look for</param>
        /// <param name="storeID">The store ID to look for</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID)
        {
            return RecordExists(entry, "RBOTERMINALTABLE", new[] {"TERMINALID", "STOREID"}, new RecordIdentifier(terminalID.PrimaryID, storeID.PrimaryID));
        }

        public virtual void Save(IConnectionManager entry, Terminal terminal)
        {
            var statement = new SqlServerStatement("RBOTERMINALTABLE");

            ValidateSecurity(entry, Permission.TerminalEdit);

            bool isNew = false;
            if (terminal.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                terminal.ID = DataProviderFactory.Instance.GenerateNumber<ITerminalData, Terminal>(entry); 
            }

            if (isNew || !Exists(entry, terminal.ID, terminal.StoreID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("TerminalID", (string)terminal.ID);
                statement.AddKey("STOREID", (string)terminal.StoreID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("TerminalID", (string)terminal.ID);

                statement.AddCondition("STOREID", (string)terminal.StoreID);
            }

            statement.AddField("NAME", terminal.Text);            
            statement.AddField("AUTOLOGOFFTIMEOUT", terminal.AutoLogOffTimeout, SqlDbType.Int);
            statement.AddField("AUTOLOCKTERMINALTIMEOUT", terminal.AutoLockTimeout, SqlDbType.Int);
            statement.AddField("HARDWAREPROFILE", (string)terminal.HardwareProfileID);
            statement.AddField("FUNCTIONALITYPROFILE", (string)terminal.FunctionalityProfileID);
            statement.AddField("VISUALPROFILE", (string)terminal.VisualProfileID);
            statement.AddField("CUSTOMERDISPLAYTEXT1", terminal.CustomerDisplayText1);
            statement.AddField("CUSTOMERDISPLAYTEXT2", terminal.CustomerDisplayText2);
            statement.AddField("OPENDRAWERATLILO", terminal.OpenDrawerAtLoginLogout ? 1 : 0, SqlDbType.TinyInt);            
            statement.AddField("EXITAFTEREACHTRANSACTION", terminal.ExitAfterEachTransaction ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("UpdateServicePort", terminal.UpdateServicePort, SqlDbType.Int);
            statement.AddField("TRANSACTIONSERVICEPROFILE", (string)terminal.TransactionServiceProfileID);
            statement.AddField("HOSPTRANSSTORESERVERPROFILE", (string)terminal.HospTransServiceProfileID);
            statement.AddField("transactionIDNumberSequence", terminal.TransactionIDNumberSequence);
            statement.AddField("IPADDRESS", terminal.IPAddress);
            statement.AddField("EFTTERMINALID", terminal.EftTerminalID);
            statement.AddField("EFTSTOREID", terminal.EftStoreID);
            statement.AddField("EFTCUSTOMFIELD1", terminal.EftCustomField1);
            statement.AddField("EFTCUSTOMFIELD2", terminal.EftCustomField2);
            statement.AddField("LSPAYUSELOCALSERVER", terminal.LSPayUseLocalServer, SqlDbType.Bit);
            statement.AddField("LSPAYSERVERNAME", terminal.LSPayServerName);
            statement.AddField("LSPAYSUPPORTREFREFUND", terminal.LSPaySupportReferenceRefund, SqlDbType.Bit);
            statement.AddField("LSPAYSERVERPORT", terminal.LSPayServerPort, SqlDbType.Int);
            statement.AddField("LSPAYPLUGINID", (string)terminal.LSPayPlugin?.ID ?? "");
            statement.AddField("LSPAYPLUGINNAME", terminal.LSPayPlugin?.Text ?? "");
            statement.AddField("LAYOUTID", (string)terminal.LayoutID);
            statement.AddField("SALESTYPEFILTER", terminal.SalesTypeFilter);
            statement.AddField("SUSPENDALLOWEOD", (int)terminal.SuspendedTransactionsStatementPosting, SqlDbType.Int);
            statement.AddField("RECEIPTIDNUMBERSEQUENCE", (string)terminal.ReceiptIDNumberSequence);
            statement.AddField("KITCHENMANAGERPROFILEID", (Guid)terminal.KitchenServiceProfileID, SqlDbType.UniqueIdentifier);
            statement.AddField("FORMINFOFIELD1", terminal.FormInfoField1);
            statement.AddField("FORMINFOFIELD2", terminal.FormInfoField2);
            statement.AddField("FORMINFOFIELD3", terminal.FormInfoField3);
            statement.AddField("FORMINFOFIELD4", terminal.FormInfoField4);
            statement.AddField("SWITCHUSERWHENENTERINGPOS", terminal.SwitchUserWhenEnteringPOS ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("STATEMENTPOSTING", (int)terminal.AllowTerminalStatementPosting, SqlDbType.Int);
            statement.AddField("INCLUDEINSTATEMENT", terminal.IncludeTerminalInStatement, SqlDbType.Bit);
            statement.AddField("DatabaseName", terminal.DatabaseName);
            statement.AddField("DatabaseServer", terminal.DatabaseServer);
            statement.AddField("DatabaseUserName", terminal.DatabaseUserName);
            statement.AddField("DatabasePassword", terminal.DatabaseUserPassword);
            statement.AddField("ACTIVATED", terminal.Activated ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LASTACTIVATEDDATE", terminal.LastActivatedDate < new DateTime(1900, 1, 1) ? new DateTime(1900, 1, 1) : terminal.LastActivatedDate, SqlDbType.DateTime);

            Save(entry, terminal, statement);
        }

        public virtual SuspendedTransactionsStatementPostingEnum TerminalAllowsEOD(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID)
        {            
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select SUSPENDALLOWEOD " +
                        "from RBOTERMINALTABLE " +
                        "where DATAAREAID = @dataAreaId and TerminalID = @id and STOREID = @storeId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)terminalID);
                MakeParam(cmd, "storeId", (string)storeID);

                object result = entry.Connection.ExecuteScalar(cmd);

                return result == null ? SuspendedTransactionsStatementPostingEnum.Yes : (SuspendedTransactionsStatementPostingEnum)((int)result);
            }
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "Terminals"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOTERMINALTABLE", "TerminalID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion


        public void MarkAsActivated(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID)
        {
            Terminal terminal = Get(entry,terminalID,storeID);
            terminal.Activated = true;
            terminal.LastActivatedDate = DateTime.Now;
            Save(entry,terminal);
        }

        public void MarkAsActivated(IConnectionManager entry, Terminal terminal)
        {
            terminal.Activated = true;
            terminal.LastActivatedDate = DateTime.Now;
            Save(entry, terminal);
        }

        public void SetHardwareProfile(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier storeID, RecordIdentifier profileID)
        {
            Terminal terminal = Get(entry, terminalID, storeID);
            terminal.HardwareProfileID = profileID;
            Save(entry, terminal);
        }

        public void SetHardwareProfile(IConnectionManager entry, Terminal terminal, RecordIdentifier profileID)
        {
            terminal.HardwareProfileID = profileID;
            Save(entry, terminal);
        }

        
    }
}
