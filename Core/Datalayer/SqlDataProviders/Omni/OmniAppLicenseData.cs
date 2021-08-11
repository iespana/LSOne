using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.DataProviders.Omni;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Omni
{
    /// <summary>
    /// Data provider class for omni app license
    /// </summary>
    public class OmniAppLicenseData : SqlServerDataProviderBase, IOmniAppLicenseData
    {
        private List<TableColumn> selectColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "APPID"},
            new TableColumn {ColumnName = "DEVICEID"},
            new TableColumn {ColumnName = "TERMINALID"},
            new TableColumn {ColumnName = "STOREID"},
            new TableColumn {ColumnName = "LICENSEKEY"},
            new TableColumn {ColumnName = "ID"}
        };

        private List<Condition> Conditions => new List<Condition>() {new Condition() {Operator = "AND", ConditionValue = " TERMINALID = @terminalID "}};

        protected virtual void PopulateLicense(IDataReader dr, OmniLicense omniLicense)
        {
            omniLicense.AppID = (string) dr["APPID"];
            omniLicense.DeviceID = (string) dr["DEVICEID"];
            omniLicense.TerminalID = (string) dr["TERMINALID"];
            omniLicense.StoreID = (string) dr["STOREID"];
            omniLicense.Licensekey = (string) dr["LICENSEKEY"];
            omniLicense.ID = (Guid) dr["ID"];
        }

        public virtual OmniLicense Get(IConnectionManager entry, RecordIdentifier terminalID, RecordIdentifier appID = null)
        {
            OmniLicense license = new OmniLicense();

            List<Condition> conditions = Conditions;
            using (var cmd = entry.Connection.CreateCommand())
            {
                if (appID != null && !RecordIdentifier.IsEmptyOrNull(appID))
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = " APPID = @appID " });
                    MakeParam(cmd, "appID", (string)appID);
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("OMNIAPPLICENSES", "O"),
                    QueryPartGenerator.InternalColumnGenerator(selectColumns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "terminalID", (string)terminalID);

                var record = Execute<OmniLicense>(entry, cmd, CommandType.Text, PopulateLicense);
                license = (record.Count > 0) ? record[0] : null;
            }
            return license;
        }

        public virtual List<OmniLicense> GetOmniLicenses(IConnectionManager entry, RecordIdentifier storeID = null)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>();

                if (!RecordIdentifier.IsEmptyOrNull(storeID))
                {
                    conditions.Add(new Condition
                    {
                        ConditionValue = "STOREID = @storeID"
                    });

                    MakeParam(cmd, "storeID", storeID);
                }

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("OMNIAPPLICENSES", "O"),
                    QueryPartGenerator.InternalColumnGenerator(selectColumns),
                    string.Empty,
                    QueryPartGenerator.ConditionGenerator(conditions),
                    " order by APPID" 
                    );

                return Execute<OmniLicense>(entry, cmd, CommandType.Text, PopulateLicense);
            }
        }

        public virtual void Save(IConnectionManager entry, OmniLicense omniLicense)
        {
            SqlServerStatement statement = new SqlServerStatement("OMNIAPPLICENSES");

            ValidateSecurity(entry, Permission.TerminalEdit);

            if (omniLicense.ID == RecordIdentifier.Empty)
            {
                statement.StatementType = StatementType.Insert;
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)omniLicense.ID, SqlDbType.UniqueIdentifier);
            }

            if (omniLicense.Licensekey != "")
            {
                statement.AddField("LICENSEKEY", (string)omniLicense.Licensekey);
            }

            statement.AddField("APPID", (string)omniLicense.AppID);
            statement.AddField("DEVICEID", (string)omniLicense.DeviceID);
            statement.AddField("TERMINALID", (string)omniLicense.TerminalID);
            statement.AddField("STOREID", (string)omniLicense.StoreID);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool LicenseKeyRecordExists(IConnectionManager entry, RecordIdentifier licenseKey)
        {
            return RecordExists(entry, "OMNIAPPLICENSES", "LICENSEKEY", licenseKey, false);
        }

        public virtual void DeleteLicense(IConnectionManager entry, RecordIdentifier licenseKey)
        {
            ValidateSecurity(entry, Permission.ManageSerialNumbers);

            SqlServerStatement statement = new SqlServerStatement("OMNIAPPLICENSES", StatementType.Delete);
            statement.AddCondition("ID", (string)licenseKey);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
