using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlDataProviders.Reports;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO.JSON;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        public virtual bool ReportExists(RecordIdentifier reportID, LogonInfo logonInfo)
        {
            try
            {
                Utils.Log(this, $"{Utils.Starting} {nameof(reportID)}: {reportID}");

                IConnectionManager dataModel = GetConnectionManager(logonInfo);
                return Providers.ReportData.Exists(dataModel, reportID);
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }
        }

        public virtual ReportResult ReportRunSubReport(ReportManifest manifest, LogonInfo logonInfo)
        {
            ReportResult reply = new ReportResult();
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            StockParameters stockParameters = new StockParameters(dataModel);

            try
            {
                Utils.Log(this, Utils.Starting);

                if (!Providers.ReportData.Exists(dataModel, manifest.ReportGuid))
                {
                    reply.ReportResultStatus = ReportResultStatusEnum.NotExists;
                    return reply;
                }

                foreach (string dataSourceName in manifest.DataSourceNames)
                {

                    IProcedureParameterData procedureProvider = Providers.ProcedureParameterData;

                    List<ProcedureParameter> procedureParameters = procedureProvider.GetParameters(dataModel, dataSourceName);

                    for (int i = 0; i < procedureParameters.Count; i++)
                    {
                        ProcedureParameter parameter = procedureParameters[i];

                        if (stockParameters.IsStockParameter(parameter))
                        {
                            stockParameters.SetValue(parameter);
                        }
                        else
                        {
                            // This parameter needs a value so here we will add the parameter to some collection of parameters 
                            // that the UI is suppposed to supply.

                            //foreach (string param in  manifest.Parameters.Keys)
                            //    // For WFC this will be the same as in the parameters from the report just come in same way.
                            //{
                            //    if ("@" + param == parameter.Name.ToUpperInvariant())
                            //    {
                            //        parameter.Value = manifest.Parameters[param];
                            //        parameter.DataType = SqlDbType.NVarChar;
                            //        break;
                            //    }
                            //}
                        }
                    }

                    DataTable dt = procedureProvider.ExecuteReportQuery(dataModel, dataSourceName, procedureParameters);
                    
                    PreProcessData(dataModel, dt);
                    DataSourceKeyValuePair dataSource = new DataSourceKeyValuePair
                    {
                        Key = dataSourceName,
                        Value = JsonConvert.SerializeObject(dt)
                    };
                        
                    reply.DataSources.Add(dataSource);

                }
                reply.ReportResultStatus = ReportResultStatusEnum.Success;
            }
            catch (Exception exception)
            {
                Utils.LogException(this, exception);

                reply.ReportResultStatus = ReportResultStatusEnum.Failed;
                reply.ResultText = exception.ToString();
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }

            return reply;
        }

        public virtual ReportResult ReportRun(ReportManifest manifest, LogonInfo logonInfo)
        {
            ReportResult reply = new ReportResult();
            IConnectionManager dataModel = GetConnectionManager(logonInfo);

            StockParameters stockParameters = new StockParameters(dataModel);

            try
            {
                Utils.Log(this, Utils.Starting);

                if (!Providers.ReportData.Exists(dataModel, manifest.ReportGuid))
                {
                    reply.ReportResultStatus = ReportResultStatusEnum.NotExists;
                    return reply;
                }
                
                foreach (string dataSourceName in manifest.DataSourceNames)
                {
                    //MessageDialog.Show(dataSourceName);

                    var procedureProvider = Providers.ProcedureParameterData;

                    List<ProcedureParameter> parameters = procedureProvider.GetParameters(dataModel, dataSourceName);

                    List<ProcedureParameter> sentParameters = new List<ProcedureParameter>();

                    for (int i = 0; i < parameters.Count; i++)
                    {
                        ProcedureParameter parameter = parameters[i];

                        if (stockParameters.IsStockParameter(parameter))
                        {
                            stockParameters.SetValue(parameter);
                        }
                        else
                        {
                            // This parameter needs a value so here we will add the parameter to some collection of parameters 
                            // that the UI is suppposed to supply.

                            ProcedureParameter prm =
                                manifest.UserInputParameters.SingleOrDefault(
                                    param => param.Name.ToUpperInvariant() == parameter.Name.ToUpperInvariant());
                            // prm.DataType = (SqlDbType)prm.IntSqlDataType;
                            parameter.Value = prm.ParseStringValue;
                            parameter.DataType = prm.DataType;
                        }

                    }

                    DataTable dt = procedureProvider.ExecuteReportQuery(dataModel, dataSourceName, parameters);

                    PreProcessData(dataModel, dt);
                    DataSourceKeyValuePair dataSource = new DataSourceKeyValuePair
                    {
                        Key = dataSourceName,

                        Value = JsonConvert.SerializeObject(dt)
                    };
                    reply.DataSources.Add(dataSource);
                }
            }
            catch (Exception exception)
            {
                Utils.LogException(this, exception);

                reply.ReportResultStatus = ReportResultStatusEnum.Failed;
                reply.ResultText = exception.ToString();
            }
            finally
            {
                Utils.Log(this, Utils.Done);
            }

            return reply;
        }

        private void PreProcessData(IConnectionManager entry, DataTable dt)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string upper = dt.Columns[i].Caption.ToUpperInvariant();

                string prefix;
                DataRow row;
                if (upper.EndsWith("FULLNAME"))
                {
                    prefix = upper.Left(upper.Length - 8);

                    string nameField = prefix + "NAME"; // Corporate
                    string firstNameField = prefix + "FIRSTNAME";
                    string lastNameField = prefix + "LASTNAME";
                    string middleNameField = prefix + "MIDDLENAME";
                    string namePrefixField = prefix + "NAMEPREFIX";
                    string nameSuffixField = prefix + "NAMESUFFIX";

                    if (!dt.Columns.Contains(nameField))
                    {
                        nameField = "";
                    }

                    if (dt.Columns.Contains(firstNameField) && dt.Columns.Contains(lastNameField) && dt.Columns.Contains(middleNameField)
                        && dt.Columns.Contains(namePrefixField) && dt.Columns.Contains(nameSuffixField))
                    {
                        dt.Columns[prefix + "FULLNAME"].ReadOnly = false;
                        dt.Columns[prefix + "FULLNAME"].MaxLength = 400;

                        for (int n = 0; n < dt.Rows.Count; n++)
                        {
                            row = dt.Rows[n];

                            if (nameField == "")
                            {
                                row[prefix + "FULLNAME"] = entry.Settings.NameFormatter.Format(
                                    (string)row[namePrefixField],
                                    (string)row[firstNameField],
                                    (string)row[middleNameField],
                                    (string)row[lastNameField],
                                    (string)row[nameSuffixField]);
                            }
                            else
                            {
                                if ((string)row[firstNameField] == "")
                                {
                                    row[prefix + "FULLNAME"] = (string)row[nameField];
                                }
                                else
                                {
                                    row[prefix + "FULLNAME"] = entry.Settings.NameFormatter.Format(
                                    (string)row[namePrefixField],
                                    (string)row[firstNameField],
                                    (string)row[middleNameField],
                                    (string)row[lastNameField],
                                    (string)row[nameSuffixField]);
                                }
                            }
                        }
                    }
                }

                if (upper.EndsWith("FULLADDRESS_SINGLELINE") || upper.EndsWith("FULLADDRESS_MULTILINE"))
                {
                    string destColumnName;

                    if (upper.EndsWith("FULLADDRESS_SINGLELINE"))
                    {
                        prefix = upper.Left(upper.Length - 22);
                        destColumnName = "FULLADDRESS_SINGLELINE";
                    }
                    else
                    {
                        prefix = upper.Left(upper.Length - 21);
                        destColumnName = "FULLADDRESS_MULTILINE";
                    }

                    dt.Columns[prefix + destColumnName].ReadOnly = false;
                    dt.Columns[prefix + destColumnName].MaxLength = 400;

                    Address address = new Address(entry.Settings.AddressFormat);

                    string address1Field = prefix + "STREET"; // Corporate
                    string address2Field = prefix + "ADDRESS";
                    string zipField = prefix + "ZIPCODE";
                    string cityField = prefix + "CITY";
                    string stateField = prefix + "STATE";
                    string countryField = prefix + "COUNTRY";
                    string addressFormatField = prefix + "ADDRESSFORMAT";

                    for (int n = 0; n < dt.Rows.Count; n++)
                    {
                        row = dt.Rows[n];

                        address.Address1 = (string)row[address1Field];
                        address.Address2 = (string)row[address2Field];
                        address.Zip = (string)row[zipField];
                        address.City = (string)row[cityField];
                        address.State = (string)row[stateField];
                        address.Country = (string)row[countryField];
                        address.AddressFormat = (Address.AddressFormatEnum)(int)row[addressFormatField];

                        if (destColumnName == "FULLADDRESS_SINGLELINE")
                        {
                            row[prefix + destColumnName] = entry.Settings.LocalizationContext.AddressFormatter.FormatSingleLine(
                                        address,
                                        entry.Cache);

                        }
                        else
                        {
                            row[prefix + destColumnName] = entry.Settings.LocalizationContext.AddressFormatter.FormatMultipleLines(
                                        address,
                                        entry.Cache,
                                        "\n");
                        }
                    }
                }
            }
        }
    }
}