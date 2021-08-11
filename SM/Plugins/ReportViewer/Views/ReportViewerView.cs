using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlDataProviders.Reports;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSRetail.SiteManager.Plugins.ReportViewer;
using Microsoft.Reporting.WinForms;
using LSOne.Services.Interfaces;
using LSOne.Utilities.IO.JSON;
using Report = LSOne.DataLayer.BusinessObjects.Reports.Report;

namespace LSOne.ViewPlugins.ReportViewer.Views
{
    public partial class ReportViewerView : ViewBase
    {
        IList<string> dataSourceNames;
        RecordIdentifier uniqueID;
        ReportParameterInfoCollection reportParameters;
        StockParameters stockParameters;
        Dictionary<string, ProcedureParameter> userInputParameters;
        Dictionary<string, ProcedureParameter> preSetValues;
        Report report;
        private bool showPreview;

        public ReportViewerView()
        {
            uniqueID = RecordIdentifier.Empty;
            report = null;
            showPreview = false;
            InitializeComponent();

            Attributes = ViewAttributes.Help | ViewAttributes.Close | ViewAttributes.Print | ViewAttributes.PageSetup;
        }

        internal ReportViewerView(RecordIdentifier reportID)
            : this()
        {
            preSetValues = new Dictionary<string, ProcedureParameter>();
            report = Providers.ReportData
                .Get(PluginEntry.DataModel, reportID);

            uniqueID = reportID;

            this.HeaderText = report.Text;
            this.HeaderIcon = Properties.Resources.Report16;
        }

        internal ReportViewerView(RecordIdentifier reportID,Dictionary<string, ProcedureParameter> preSetValues)
            : this(reportID)
        {
            this.preSetValues = preSetValues;
        }
        internal ReportViewerView(RecordIdentifier reportID, Dictionary<string, ProcedureParameter> preSetValues, bool preview, bool print = false)
          : this(reportID, preSetValues)
        {
            showPreview = preview;
            reportParameterControl1.ForceShowReport = showPreview;
            if (print)
            {
                this.reportViewer1.RenderingComplete += ReportViewer1OnRenderingComplete;
            }
        }

        private void ReportViewer1OnRenderingComplete(object sender, RenderingCompleteEventArgs renderingCompleteEventArgs)
        {
            OnPrint();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.ShowProgress();

            timer1.Start();
        }

        protected override string LogicalContextName
        {
            get
            {
                return HeaderText;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        {
                return uniqueID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            
        }

      

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        protected override void OnPrint()
        {
            printTimer.Enabled = true;
        }

        protected override void OnPageSetup()
        {
            if (reportViewer1.PageSetupDialog() == DialogResult.OK)
            {
                // nothing really we do here for now
            }
        }

        private void reportViewerControl1_CloseSheet(object sender, EventArgs e)
        {
            this.ManualClose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            userInputParameters = new Dictionary<string, ProcedureParameter>();

            timer1.Stop();

            stockParameters = new StockParameters(PluginEntry.DataModel);

            try
            {
                if (!report.SqlDataInstalled)
                {
                    try
                    {
                        Providers.ReportData.InsertSQLProcedures(PluginEntry.DataModel, report);
                    }
                    catch(Exception ex)
                    {
                        MessageDialog.Show(Properties.Resources.ErrorInsertingSQLCode + " " + ex.Message + (ex.InnerException != null ? " - " + ex.InnerException.Message : ""));

                        this.HideProgress();

                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);

                        return;
                    }
                }

                MemoryStream stream = new MemoryStream(report.ReportData);

                this.reportViewer1.LocalReport.ReportEmbeddedResource = "";
                this.reportViewer1.LocalReport.LoadReportDefinition(stream);
                
                // Collect information from the report
                // ---------------------------------------------------------------------------------------------------------------------
                reportParameters = this.reportViewer1.LocalReport.GetParameters();

                dataSourceNames = this.reportViewer1.LocalReport.GetDataSourceNames();

                this.reportViewer1.LocalReport.DataSources.Clear();

                this.reportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;

                foreach (string dataSourceName in dataSourceNames)
                {
                    if(dataSourceName.Equals("CURRENTCOMPANY", StringComparison.OrdinalIgnoreCase) 
                        || dataSourceName.Equals("CURRENTUSER", StringComparison.OrdinalIgnoreCase) 
                        || dataSourceName.Equals("CURRENTSTORE", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    List<ProcedureParameter> parameters = Providers.ProcedureParameterData
                        .GetParameters(PluginEntry.DataModel, dataSourceName);

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

                            // if we do not know the parameter allready then we will add it to our user input collection
                            if (!userInputParameters.ContainsKey(parameter.Name.ToUpperInvariant()))
                            {
                                userInputParameters.Add(parameter.Name.ToUpperInvariant(), parameter);
                            }
                        }
                    }
                }

                

                foreach (ReportParameterInfo param in reportParameters)
                {
                    ProcedureParameter procedureParam = new ProcedureParameter("@" + param.Name.ToUpperInvariant());

                    if (stockParameters.IsStockParameter(procedureParam))
                    {
                        stockParameters.SetValue(procedureParam);
                    }
                    else
                    {
                        if (!userInputParameters.ContainsKey(procedureParam.Name))
                        {
                            userInputParameters.Add(procedureParam.Name, procedureParam);
                        }
                    }
                }

                // End of collecting information from the report-------------------------------------------------------------------------

                
                // Tell the parameter control of all the parameters we have -------------------------------------------------------------
                reportParameterControl1.SetParameterSet(uniqueID,userInputParameters, preSetValues);
                // End of telling parameter control of all the parameters ---------------------------------------------------------------
            }
            catch(Exception ex)
            {
                MessageDialog.Show(ex.Message);

                if (ex.InnerException != null)
                {
                    MessageDialog.Show(ex.InnerException.Message);

                    if (ex.InnerException.InnerException != null)
                    {
                        MessageDialog.Show(ex.InnerException.InnerException.Message);
                    }
                }
            }

            this.HideProgress();
        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            IList<string> dataSources = e.DataSourceNames;
            Report report = Providers.ReportData.Get(PluginEntry.DataModel, uniqueID);

            int datasourceNumber = 0;
            if (report.SiteServiceReport)
            {
                ReportManifest manifest = new ReportManifest();
                manifest.ReportGuid = uniqueID;
                foreach (string dataSourceName in dataSources)
                {
                    manifest.DataSourceNames.Add(dataSourceName);
                }

                ReportResult result = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel)
                    .ReportRun(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), manifest,
                        true);
                if (result.ReportResultStatus == ReportResultStatusEnum.Success)
                {
                    foreach (DataSourceKeyValuePair dataSourceKeyValuePair in result.DataSources)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(dataSourceKeyValuePair.Value);
                        e.DataSources.Add(new ReportDataSource(dataSourceKeyValuePair.Key, dt));
                    }
                }
            }
            else
            {
                foreach (string dataSourceName in dataSources)
                {
                    // 1. Look through mappings from Manifest and substitude if needed.
                    // like if spGetUser = GetUser
                    // (Note when manifest is read then mappings will need to be added to some new table !!)


                    // 2. If mapping was found then check if we know the mapping
                    // Only mapping that we know is valid.
                    // Loop through the report parameters and check if we get the parameters that our WCF call expects.

                    // 3. Bellow would be the else if its not a mapped to WCF procedure

                    ////--------------SiteService 
                    IProcedureParameterData procedureProvider = Providers.ProcedureParameterData;

                    List<ProcedureParameter> parameters = procedureProvider.GetParameters(PluginEntry.DataModel,
                        dataSourceName);
                    // WFC equal to this will be hard coded list that we know the WFC procedure wants

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

                            foreach (var param in e.Parameters)
                                // For WFC this will be the same as in the parameters from the report just come in same way.
                            {
                                if ("@" + param.Name.ToUpperInvariant() == parameter.Name.ToUpperInvariant())
                                {
                                    parameter.Value = param.Values[0];
                                    parameter.DataType = SqlDbType.NVarChar;
                                    break;
                                }
                            }

                        }

                    }

                    DataTable dt = procedureProvider.ExecuteReportQuery(PluginEntry.DataModel, dataSourceName,
                        parameters);

                    PreProcessData(dt);

                    e.DataSources.Add(new ReportDataSource(dataSourceName, dt));

                    datasourceNumber++;
                }
            }
        }

        protected override void OnClose()
        {
            if (reportViewer1 != null)
            {
                // Prevent a documented bug in ReportViewer control that causes can't unload app domain exception
                reportViewer1.LocalReport.ReleaseSandboxAppDomain();
            }
            
            base.OnClose();
        }

        private void printTimer_Tick(object sender, EventArgs e)
        {
            // This has to be done to prevent a known bug in ReportViewer when printing on other printer than default printer.
            printTimer.Stop();
            
            if (!reportViewer1.CurrentStatus.CanPrint)
            {
                MessageDialog.Show(Properties.Resources.ReportCannotBePrinted);
                return;
            }

            try
            {
                reportViewer1.PrintDialog();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Properties.Resources.UnknownError + ": " + ex.Message);
            }
        }

        private void PreProcessData(DataTable dt)
        {
            string prefix;
            DataRow row;

            for(int i = 0; i < dt.Columns.Count ; i++)
            {
                string upper = dt.Columns[i].Caption.ToUpperInvariant();

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
                                row[prefix + "FULLNAME"] = PluginEntry.DataModel.Settings.NameFormatter.Format(
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
                                    row[prefix + "FULLNAME"] = PluginEntry.DataModel.Settings.NameFormatter.Format(
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

                    Address address = new Address(PluginEntry.DataModel.Settings.AddressFormat);

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
                            row[prefix + destColumnName] = PluginEntry.DataModel.Settings.LocalizationContext.AddressFormatter.FormatSingleLine(
                                        address,
                                        PluginEntry.DataModel.Cache);

                        }
                        else
                        {
                            row[prefix + destColumnName] = PluginEntry.DataModel.Settings.LocalizationContext.AddressFormatter.FormatMultipleLines(
                                        address,
                                        PluginEntry.DataModel.Cache,
                                        "\n");
                        }
                    }
                }
            }
        }

        private void reportParameterControl1_ViewReport(object sender, EventArgs e)
        {
            try
            {
                Report report = Providers.ReportData.Get(PluginEntry.DataModel, uniqueID);

                List<ReportParameter> paramList = new List<ReportParameter>();

                foreach (ReportParameterInfo param in reportParameters)
                {
                    string paramName = "@" + param.Name.ToUpperInvariant();

                    if (userInputParameters[paramName].Value is string[])
                    {
                        paramList.Add(new ReportParameter(param.Name, (string[])userInputParameters[paramName].Value));
                    }
                    else
                    {
                        paramList.Add(new ReportParameter(param.Name, userInputParameters[paramName].ParameterFormatedValue));
                    }
                }

                this.reportViewer1.LocalReport.SetParameters(paramList);

                this.reportViewer1.LocalReport.DataSources.Clear();
                ReportManifest manifest = new ReportManifest();
               
                if (report.SiteServiceReport)
                {
                    manifest.UserInputParameters = userInputParameters.Values.ToList();
                    foreach (ProcedureParameter procedureParameter in userInputParameters.Values.ToList())
                    {

                        if (procedureParameter.Value is string[])
                        {
                            procedureParameter.StringValue = JsonConvert.SerializeObject(procedureParameter.Value);
                            procedureParameter.IsStringArray = true;
                        }
                        else
                        {
                            procedureParameter.IsStringArray = false;
                            switch (procedureParameter.DataType)
                            {

                                case SqlDbType.BigInt:
                                    procedureParameter.StringValue =
                                        ((long) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.Bit:
                                    procedureParameter.StringValue =
                                        ((bool) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.Char:
                                    break;
                                case SqlDbType.DateTime:
                                case SqlDbType.Date:
                                case SqlDbType.Time:
                                case SqlDbType.DateTime2:
                                    procedureParameter.StringValue =
                                        ((DateTime) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.Decimal:
                                    procedureParameter.StringValue =
                                        ((decimal) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.Float:
                                    procedureParameter.StringValue =
                                        ((float) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.TinyInt:
                                case SqlDbType.Int:
                                    procedureParameter.StringValue =
                                        ((int) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.NChar:
                                case SqlDbType.VarChar:
                                case SqlDbType.NVarChar:
                                    procedureParameter.StringValue =
                                        ((string) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.Real:
                                    procedureParameter.StringValue =
                                        ((float) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.UniqueIdentifier:
                                    procedureParameter.StringValue = procedureParameter.Value.ToString();
                                    break;
                                case SqlDbType.SmallDateTime:
                                    procedureParameter.StringValue =
                                        ((DateTime) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.SmallInt:
                                    procedureParameter.StringValue =
                                        ((int) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.VarBinary:
                                    procedureParameter.StringValue =
                                        ((int) procedureParameter.Value).ToString(CultureInfo.InvariantCulture);
                                    break;
                                case SqlDbType.Money:
                                case SqlDbType.Image:
                                case SqlDbType.Binary:
                                case SqlDbType.NText:
                                case SqlDbType.SmallMoney:
                                case SqlDbType.Text:
                                case SqlDbType.Timestamp:
                                case SqlDbType.Variant:
                                case SqlDbType.Xml:
                                case SqlDbType.Udt:
                                case SqlDbType.Structured:
                                case SqlDbType.DateTimeOffset:
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }
                    manifest.ReportGuid = uniqueID;
                }
                foreach (string dataSourceName in dataSourceNames)
                {
                    switch (dataSourceName.ToUpperInvariant())
                    {
                        case "CURRENTCOMPANY":
                        {
                            List<CompanyInfo> result = new List<CompanyInfo>();

                            CompanyInfo ci = Providers.CompanyInfoData.Get(PluginEntry.DataModel, true);

                            if (ci != null)
                            {
                                result.Add(ci);
                            }

                            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(dataSourceName, result));
                        }
                            break;

                        case "CURRENTSTORE":
                        {
                            List<Store> result = new List<Store>() {Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID, CacheType.CacheTypeApplicationLifeTime, includeReportFormatting: true) ?? new Store()};
                            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(dataSourceName, result));
                        }
                            break;

                        case "CURRENTUSER":
                        {
                            var result = new List<User>();

                            var user = Providers.UserData.Get(PluginEntry.DataModel, (Guid) PluginEntry.DataModel.CurrentUser.ID);

                            if (user != null)
                            {
                                user.FormattedName = PluginEntry.DataModel.Settings.NameFormatter.Format(user.Name);
                                result.Add(user);
                            }

                            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(dataSourceName, result));
                        }
                            break;

                        default:
                            if (report.SiteServiceReport)
                            {
                                manifest.DataSourceNames.Add(dataSourceName);
                            }
                            else
                            {
                                var procedureProvider = Providers.ProcedureParameterData;

                                List<ProcedureParameter> parameters = procedureProvider.GetParameters(PluginEntry.DataModel, dataSourceName);

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

                                        ProcedureParameter prm = userInputParameters[parameter.Name.ToUpperInvariant()];

                                        parameter.Value = prm.Value;
                                        parameter.DataType = prm.DataType;
                                    }
                                }

                                DataTable dt = procedureProvider.ExecuteReportQuery(PluginEntry.DataModel, dataSourceName, parameters);

                                PreProcessData(dt);

                                this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(dataSourceName, dt));
                           }
                            break;
                    }
                }
                if (report.SiteServiceReport)
                {
                    ReportResult result = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).ReportRun(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), manifest, true);

                    foreach (DataSourceKeyValuePair dataSourceKeyValuePair in result.DataSources)
                    {
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(dataSourceKeyValuePair.Value);
                        this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource(dataSourceKeyValuePair.Key, dt));
                    }
                }

                this.reportViewer1.Messages = new ReportViewerLocalizer();

                this.reportViewer1.RefreshReport();
             
            }
            catch (Exception ex)
            {
                MessageDialog.Show(ex.Message);

                if (ex.InnerException != null)
                {
                    MessageDialog.Show(ex.InnerException.Message);

                    if (ex.InnerException.InnerException != null)
                    {
                        MessageDialog.Show(ex.InnerException.InnerException.Message);
                    }
                }
            }
        }
    }
}
