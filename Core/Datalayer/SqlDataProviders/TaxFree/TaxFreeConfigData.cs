using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders.TaxFree;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.SqlDataProviders.TaxFree
{
    public class TaxFreeConfigData : SqlServerDataProviderBase, ITaxFreeConfigData
    {
        private const string baseSQL = "SELECT Key_, NAME, ADDRESS, POSTCITY, COUNTRY, PHONE, WEB, VATNUMBER, " +
                                       "PRINTOUTTYPE, PROMPTCUSTFORINFO, PROMPTFORPASSPORT, PROMPTFORFLIGHT, PROMPTFORREPORT, PROMPTFORTOURIST, " +
                                       "LINEWIDTH, DEFAULTPADDING, MINTAXREFUNDLIMIT, TAXNUMBER, COUNTRYCODE, FORMTYPE " +
                                       "FROM TAXFREECONFIG ";

        private static void PopulateTaxFreeConfig(IDataReader dr, TaxFreeConfig config)
        {
            config.ID = (int)dr["Key_"];
            config.Text = (string)dr["NAME"];
            config.Address = (string)dr["ADDRESS"];
            config.PostcodeCity = (string)dr["POSTCITY"];
            config.Country = (string)dr["COUNTRY"];
            config.Phone = (string)dr["PHONE"];
            config.Web = (string)dr["WEB"];
            config.VatNumber = (string)dr["VATNUMBER"];

            config.PrintoutType = (TaxFreePrintoutEnum)dr["PRINTOUTTYPE"];
            config.PromptCustomerForInformation = ((byte)dr["PROMPTCUSTFORINFO"] != 0);
            config.PromptForPassport = ((byte)dr["PROMPTFORPASSPORT"] != 0);
            config.PromptForFlightInfo = ((byte)dr["PROMPTFORFLIGHT"] != 0);
            config.PromptForReportInfo = ((byte)dr["PROMPTFORREPORT"] != 0);
            config.PromptForTouristInfo = ((byte)dr["PROMPTFORTOURIST"] != 0);
            config.LineWidth = (int)dr["LINEWIDTH"];
            config.DefaultPadding = (int)dr["DEFAULTPADDING"];
            config.MinTaxRefundLimit = (decimal)dr["MINTAXREFUNDLIMIT"];

            config.TaxNumber = (string)dr["TAXNUMBER"];
            config.CountryCode = (string)dr["COUNTRYCODE"];
            config.FormType = (string)dr["FORMTYPE"];
        }

        public virtual TaxFreeConfig Get(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = baseSQL + "WHERE DATAAREAID = @dataAreaId";
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var results = Execute<TaxFreeConfig>(entry, cmd, CommandType.Text, PopulateTaxFreeConfig);
                return results.Count > 0 ? results[0] : new TaxFreeConfig();
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "TAXFREECONFIG", "KEY_", id);
        }

        public virtual bool HasEntries(IConnectionManager entry)
        {
            return RecordExists(entry, "TAXFREECONFIG", @"'1'", new RecordIdentifier(@"1"));
        }

        public virtual void Save(IConnectionManager entry, TaxFreeConfig config)
        {
            var statement = new SqlServerStatement("TAXFREECONFIG");

            ValidateSecurity(entry, BusinessObjects.Permission.AdministrationMaintainSettings);

            if (!Exists(entry, config.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddField("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddField("KEY_", (int)config.ID, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("KEY_", (int)config.ID, SqlDbType.Int);
            }

            statement.AddField("NAME", config.Text);
            statement.AddField("ADDRESS", config.Address);
            statement.AddField("POSTCITY", config.PostcodeCity);
            statement.AddField("COUNTRY", config.Country);
            statement.AddField("PHONE", config.Phone);
            statement.AddField("WEB", config.Web);
            statement.AddField("VATNUMBER", config.VatNumber);
            statement.AddField("PRINTOUTTYPE", (int)config.PrintoutType, SqlDbType.TinyInt);
            statement.AddField("PROMPTCUSTFORINFO", config.PromptCustomerForInformation, SqlDbType.TinyInt);
            statement.AddField("PROMPTFORPASSPORT", config.PromptForPassport, SqlDbType.TinyInt);
            statement.AddField("PROMPTFORFLIGHT", config.PromptForFlightInfo, SqlDbType.TinyInt);
            statement.AddField("PROMPTFORREPORT", config.PromptForReportInfo, SqlDbType.TinyInt);
            statement.AddField("PROMPTFORTOURIST", config.PromptForTouristInfo, SqlDbType.TinyInt);
            statement.AddField("LINEWIDTH", config.LineWidth, SqlDbType.Int);
            statement.AddField("DEFAULTPADDING", config.DefaultPadding, SqlDbType.Int);
            statement.AddField("MINTAXREFUNDLIMIT", config.MinTaxRefundLimit, SqlDbType.Decimal);
            statement.AddField("TAXNUMBER", config.TaxNumber);
            statement.AddField("COUNTRYCODE", config.CountryCode);
            statement.AddField("FORMTYPE", config.FormType);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
