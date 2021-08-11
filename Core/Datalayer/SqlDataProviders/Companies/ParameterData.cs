using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Data;

namespace LSOne.DataLayer.SqlDataProviders.Companies
{
    public class ParameterData : SqlServerDataProviderBase, IParameterData
    {
        public virtual Parameters Get(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            var parameters = new Parameters();
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select ISNULL(p.LOCALSTOREID,'') as LOCALSTOREID,
                    ISNULL(s.NAME,'') as LOCALSTORENAME,
                    ISNULL(p.RECEIPTOPTION,0) as RECEIPTOPTION,
                    ISNULL(p.DEFAULTDIMENSION, '') as DEFAULTDIMENSION,
                    ISNULL(p.SITESERVICEPROFILE,'') AS SITESERVICEPROFILE,
                    ISNULL(P.MANUALLYENTERCUSTID, 0) as MANUALLYENTERCUSTID, 
                    ISNULL(P.MANUALLYENTERITEMID, 0) as MANUALLYENTERITEMID,
                    ISNULL(P.MANUALLYENTERVENDORID, 0) as MANUALLYENTERVENDORID,
                    ISNULL(P.MANUALLYENTERSTOREID, 0) as MANUALLYENTERSTOREID,
                    ISNULL(P.MANUALLYENTERTERMINALID, 0) as MANUALLYENTERTERMINALID,
                    ISNULL(P.MANUALLYENTERUNITID, 0) as MANUALLYENTERUNITID,
                    ISNULL(P.MANUALLYENTERTAXCODEID, 0) as MANUALLYENTERTAXCODEID,
                    ISNULL(P.MANUALLYENTERTAXGROUPID, 0) as MANUALLYENTERTAXGROUPID,
                    ISNULL(p.MANUALLYENTERGIFTCARDID, 0) as MANUALLYENTERGIFTCARDID, 
                    ISNULL(P.SCALEGRAMUNIT, '') AS SCALEGRAMUNIT,
                    ISNULL(P.SCALEKILOGRAMUNIT, '') AS SCALEKILOGRAMUNIT, 
                    ISNULL(P.SCALEOUNCEUNIT, '') AS SCALEOUNCEUNIT, 
                    ISNULL(P.SCALEPOUNDUNIT, '') AS SCALEPOUNDUNIT, 
                    ISNULL(P.TAXEXEMPTTAXGROUP, '') AS TAXEXEMPTTAXGROUP,
                    p.CURRENTLOCATION
                    from RBOPARAMETERS p 
                    left outer join RBOSTORETABLE s on p.LOCALSTOREID = s.STOREID and p.DATAAREAID = s.DATAAREAID 
                    where p.DATAAREAID = @dataAreaID and p.KEY_ = 0";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        parameters.DefaultDimention = (string) dr["DEFAULTDIMENSION"];
                        parameters.LocalStore = (string) dr["LOCALSTOREID"];
                        parameters.TaxExemptTaxGroup = (string)dr["TAXEXEMPTTAXGROUP"];
                        parameters.LocalStoreName = (string) dr["LOCALSTORENAME"];
                        parameters.SiteServiceProfile = (string) dr["SITESERVICEPROFILE"];
                        parameters.ReceiptSettings = (ReceiptSettingsEnum) dr["RECEIPTOPTION"];
                        parameters.ManuallyEnterItemID = Conversion.ToBool(dr["MANUALLYENTERITEMID"]);
                        parameters.ManuallyEnterCustomerID = Conversion.ToBool(dr["MANUALLYENTERCUSTID"]);
                        parameters.ManuallyEnterVendorID = Conversion.ToBool(dr["MANUALLYENTERVENDORID"]);
                        parameters.ManuallyEnterStoreID = Conversion.ToBool(dr["MANUALLYENTERSTOREID"]);
                        parameters.ManuallyEnterTerminalID = Conversion.ToBool(dr["MANUALLYENTERTERMINALID"]);
                        parameters.ManuallyEnterUnitID = Conversion.ToBool(dr["MANUALLYENTERUNITID"]);
                        parameters.ManuallyEnterTaxCodeID = Conversion.ToBool(dr["MANUALLYENTERTAXCODEID"]);
                        parameters.ManuallyEnterTaxGroupID = Conversion.ToBool(dr["MANUALLYENTERTAXGROUPID"]);
                        parameters.ManuallyEnterGiftCardID = Conversion.ToBool(dr["MANUALLYENTERGIFTCARDID"]); 
                        parameters.ScaleGramUnit = (string)dr["SCALEGRAMUNIT"];
                        parameters.ScaleKiloGramUnit = (string)dr["SCALEKILOGRAMUNIT"];
                        parameters.ScaleOunceUnit = (string)dr["SCALEOUNCEUNIT"];
                        parameters.ScalePoundUnit = (string)dr["SCALEPOUNDUNIT"];
                        parameters.CurrentLocation = dr["CURRENTLOCATION"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["CURRENTLOCATION"];
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
            SiteServiceProfile profile = null;
            parameters.Dirty = false;
            // Update the cached values
            if (parameters.SiteServiceProfile != null && !parameters.SiteServiceProfile.IsEmpty)
            {
                profile = Providers.SiteServiceProfileData.Get(entry, parameters.SiteServiceProfile);
            }

            if (profile != null)
            {
                entry.SiteServiceAddress = profile.SiteServiceAddress;
                entry.SiteServicePortNumber = (ushort) profile.SiteServicePortNumber;
            }            

            return parameters;
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "RBOPARAMETERS", "KEY_", ID);
        }

        public virtual void Save(IConnectionManager entry, Parameters parameters)
        {
            var statement = new SqlServerStatement("RBOPARAMETERS");

            ValidateSecurity(entry, BusinessObjects.Permission.TerminalEdit);

            parameters.Validate();

            if (!Exists(entry, 0))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("KEY_", 0, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("KEY_", 0, SqlDbType.Int);
            }

            statement.AddField("LOCALSTOREID", (string)parameters.LocalStore);
            statement.AddField("TAXEXEMPTTAXGROUP", (string)parameters.TaxExemptTaxGroup);
            statement.AddField("SITESERVICEPROFILE", (string) parameters.SiteServiceProfile);
            statement.AddField("MANUALLYENTERCUSTID", parameters.ManuallyEnterCustomerID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MANUALLYENTERITEMID", parameters.ManuallyEnterItemID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MANUALLYENTERVENDORID", parameters.ManuallyEnterVendorID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MANUALLYENTERSTOREID", parameters.ManuallyEnterStoreID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MANUALLYENTERTERMINALID", parameters.ManuallyEnterTerminalID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MANUALLYENTERUNITID", parameters.ManuallyEnterUnitID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MANUALLYENTERTAXCODEID", parameters.ManuallyEnterTaxCodeID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MANUALLYENTERTAXGROUPID", parameters.ManuallyEnterTaxGroupID ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MANUALLYENTERGIFTCARDID", parameters.ManuallyEnterGiftCardID? 1 : 0, SqlDbType.TinyInt);

            statement.AddField("SCALEGRAMUNIT", (string)parameters.ScaleGramUnit);
            statement.AddField("SCALEKILOGRAMUNIT", (string)parameters.ScaleKiloGramUnit);
            statement.AddField("SCALEOUNCEUNIT", (string)parameters.ScaleOunceUnit);
            statement.AddField("SCALEPOUNDUNIT", (string)parameters.ScalePoundUnit);
            statement.AddField("CURRENTLOCATION", parameters.CurrentLocation != null ? (object)(Guid) parameters.CurrentLocation : (object)DBNull.Value, SqlDbType.UniqueIdentifier);
            entry.Connection.ExecuteStatement(statement);

            // Update the cached values
            if ( parameters.SiteServiceProfile != null && !parameters.SiteServiceProfile.IsEmpty)
            {
                SiteServiceProfile profile = Providers.SiteServiceProfileData.Get(entry, parameters.SiteServiceProfile);
                if (profile != null)
                {
                    entry.SiteServiceAddress = profile.SiteServiceAddress;
                    entry.SiteServicePortNumber = (ushort) profile.SiteServicePortNumber;
                }
            }
            Providers.CustomerData.ClearCache();

            parameters.Dirty = false;
        }


        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }
    }
}
