using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DatabaseUtil;
using LSOne.DataLayer.DatabaseUtil.Enums;
using LSOne.DataLayer.DatabaseUtil.ScriptInformation;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Administration.DataLayer;
using LSOne.ViewPlugins.Administration.DataLayer.DataEntities;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using System.Linq;

namespace LSOne.ViewPlugins.Administration.Dialogs
{
    public partial class InsertDefaultDataDialog : DialogBase
    {
        private Thread thread;

        public InsertDefaultDataDialog()
        {
            List<ScriptInfo> scriptInfos;

            InitializeComponent();

            lvScripts.ContextMenuStrip = new ContextMenuStrip();
            lvScripts.ContextMenuStrip.Opening += lvScripts_ContextMenuStripOpening;

            if (PluginEntry.DataModel.IsCloud)
            {
                var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

                scriptInfos = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).GetAvailableDefaultData(PluginEntry.DataModel, siteServiceProfile);
            }
            else
            {
                scriptInfos = DatabaseUtility.GetSQLScriptInfo(LSOne.DataLayer.DatabaseUtil.Enums.RunScripts.DefaultData);
                scriptInfos.AddRange(DatabaseUtility.GetExternalSQLScriptInfo());
            }

            scriptInfos = scriptInfos.OrderBy(x => x.ScriptName).ToList();

            foreach (ScriptInfo info in scriptInfos)
            {
                Row row = new Row();
                row.AddCell(new CheckBoxCell(false, true, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.AddText(info.IsExternalScript ? info.ScriptName : GetLocalizedScriptName(info));
                row.AddCell(new CheckBoxCell(!info.IsExternalScript, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));

                row.Tag = info;

                lvScripts.AddRow(row);
            }

            lvScripts.Sort(lvScripts.Columns[1], true);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<ScriptInfo> selectedList = new List<ScriptInfo>();
            foreach (Row item in lvScripts.Rows)
            {
                if (((CheckBoxCell)item[0]).Checked)
                {
                    selectedList.Add((ScriptInfo)item.Tag);
                }
            }

            if(PluginEntry.DataModel.IsCloud)
            {
                thread = new Thread(() => RunScriptsOnCloud(selectedList));
            }
            else
            {
                thread = new Thread(() => RunScripts(selectedList));
            }

            thread.Start();
        }

        //runs in thread
        private void RunScripts(IEnumerable<ScriptInfo> selected)
        {
            string scriptData = "";


            Invoke((Action) (() =>
                {
                    lblProgress.Visible = progressBar.Visible = true;
                    btnOK.Enabled = btnCancel.Enabled = lvScripts.Enabled = false;
                }));

            try
            {
                foreach (ScriptInfo scriptInfo in selected)
                {
                    if(!scriptInfo.IsExternalScript)
                    {
                        scriptData = DatabaseUtility.GetSpecificScript(PluginEntry.DataModel, scriptInfo);
                    }

                    RunXmlScript(scriptInfo, scriptData);
                }

            }
            catch (Exception)
            {

            }
            Invoke((Action) (() =>
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "DefaultData", RecordIdentifier.Empty, null);
                    btnCancel.Enabled = btnOK.Enabled = lvScripts.Enabled = true;
                    DialogResult = DialogResult.OK;
                    Close();
                }));

            PluginEntry.Framework.SetAllDashboardItemsDirty();
        }


        private void RunScriptsOnCloud(IEnumerable<ScriptInfo> selected)
        {


            Invoke((Action) (() =>
            {
                progressBar.Style = ProgressBarStyle.Marquee;
                lblProgress.Visible = progressBar.Visible = true;
                btnOK.Enabled = btnCancel.Enabled = lvScripts.Enabled = false;
            }));

            try
            {
                var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                SiteServiceProfile siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

                foreach (ScriptInfo scriptInfo in selected)
                {

                    Guid taskGuid = Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel)
                        .RunDemoData(PluginEntry.DataModel, siteServiceProfile, scriptInfo);
                    do
                    {
                        Thread.Sleep(2000);
                    } while (
                        Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel)
                            .IsTaskActive(PluginEntry.DataModel, siteServiceProfile, taskGuid));
                }

            }
            catch (Exception)
            {

            }
            Invoke((Action) (() =>
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "DefaultData",
                    RecordIdentifier.Empty, null);
                btnCancel.Enabled = btnOK.Enabled = lvScripts.Enabled = true;
                DialogResult = DialogResult.OK;
                Close();
            }));

            PluginEntry.Framework.SetAllDashboardItemsDirty();
        }

        private string GetLocalizedScriptName(ScriptInfo scriptInfo)
        {
            string captionKey;
            string localizedContent;

            captionKey = scriptInfo.ScriptName.Replace(".xml", "").Replace(" ","");

            localizedContent = Properties.Resources.ResourceManager.GetString(captionKey, Properties.Resources.Culture);

            if (localizedContent != "" && localizedContent != null)
            {
                return localizedContent;
            }
            else
            {
                return scriptInfo.ScriptName;
            }

            
        }

        private string GetLocalizedScriptName(string scriptName)
        {
            string captionKey;
            string localizedContent;

            captionKey = scriptName.Replace(".xml", "").Replace(" ", "");

            localizedContent = Properties.Resources.ResourceManager.GetString(captionKey, Properties.Resources.Culture);

            if (localizedContent != "" && localizedContent != null)
            {
                return localizedContent;
            }
            else
            {
                return scriptName;
            }


        }

        private void SetfieldValue(DBField field, XmlNode record)
        {
            if (field.DBType == SqlDbType.Image || field.DBType == SqlDbType.VarBinary || field.DBType == SqlDbType.Binary)
            {
                field.Value = Convert.FromBase64String(record.SelectSingleNode(field.Name).InnerText);
            }
            else if (field.DBType == SqlDbType.Decimal)
            {
                field.Value = XmlConvert.ToDecimal(record.SelectSingleNode(field.Name).InnerText);
            }
            else if (field.DBType == SqlDbType.TinyInt)
            {
                field.Value = XmlConvert.ToByte(record.SelectSingleNode(field.Name).InnerText);
            }
            else if (field.DBType == SqlDbType.Int)
            {
                field.Value = XmlConvert.ToInt32(record.SelectSingleNode(field.Name).InnerText);
            }
            else if (field.DBType == SqlDbType.DateTime)
            {
                field.Value = XmlConvert.ToDateTime(record.SelectSingleNode(field.Name).InnerText,XmlDateTimeSerializationMode.Utc);
            }
            else if (field.DBType == SqlDbType.Time)
            {
                field.Value = XmlConvert.ToTimeSpan(record.SelectSingleNode(field.Name).InnerText);
            }
            else if (field.DBType == SqlDbType.UniqueIdentifier)
            {
                field.Value = XmlConvert.ToGuid(record.SelectSingleNode(field.Name).InnerText);
            }
            else
            {
                field.Value = record.SelectSingleNode(field.Name).InnerText;
            }
        }

        //runs in thread
        private void RunXmlScript(ScriptInfo scriptInfo, string scriptData)
        {
            XmlDocument document;
            XmlNode root;
            XmlNode record;
            XmlNode foundNode;
            List<DBField> primaryKeys = null;
            List<DBField> columns = null;
            string fieldName;
            Parameters param;
            bool hasPostedWarning = false;
            bool abortRecord = false;
            bool erasedPosisOperations = false;

            document = new XmlDocument();
            bool originalDisabledReplicationAction = PluginEntry.DataModel.DisableReplicationActionCreation;

            try
            {
                if(scriptInfo.IsExternalScript)
                {
                    document.Load(scriptInfo.ResourceName);
                }
                else
                {
                    document.LoadXml(scriptData);
                }

                param = Providers.ParameterData.Get(PluginEntry.DataModel);

                root = document.FirstChild.NextSibling;
                string lastTableName = null;

                PluginEntry.DataModel.DisableReplicationActionCreation = true;

                foreach (XmlNode node in root.ChildNodes)
                {
                    // We skip over the schema since we do not care for that thats why we call next sibling
                    record = node.FirstChild.NextSibling;

                    Invoke((Action) (() =>
                        {
                            progressBar.Maximum = node.ChildNodes.Count;
                            progressBar.Minimum = 0;
                            progressBar.Value = 0;
                            progressBar.Step = 1;
                        }));

                    var dbProvider = DataProviderFactory.Instance.Get<IDBFieldData, DBField>();
                    while (record != null)
                    {
                        //Application.DoEvents();

                        if (lastTableName != record.Name)
                        {
                            // We need to get primary key schema so that we can see if the record exists,
                            // we also need to get the full colum schema
                            primaryKeys = dbProvider.GetPrimaryFieldsForTable(PluginEntry.DataModel, record.Name);
                            columns = dbProvider.GetAllFieldsForTable(PluginEntry.DataModel, record.Name);

                            //MessageDialog.Show(record.Name);
                        }

                        if(record.Name == "POSISOPERATIONS" && !erasedPosisOperations)
                        {
                            dbProvider.ClearTable(PluginEntry.DataModel, record.Name);
                            erasedPosisOperations = true;
                        }

                        abortRecord = false;

                        foreach (DBField field in primaryKeys)
                        {
                            fieldName = field.Name.ToUpperInvariant();

                            foundNode = record.SelectSingleNode(field.Name);

                            if (foundNode != null)
                            {
                                SetfieldValue(field, record);
                            }
                            else
                            {
                                field.Value = DBNull.Value;
                            }

                            if (fieldName == "DATAAREAID")
                            {
                                field.Value = PluginEntry.DataModel.Connection.DataAreaId;
                            }
                            else if ((fieldName == "STOREID" || fieldName == "RESTAURANTID") 
                                && (field.Value == DBNull.Value || (string)field.Value != "")
                                && !scriptInfo.ScriptName.ToLower().Contains("demo data")
                                && !scriptInfo.ScriptName.ToLower().Contains("template data"))
                            // && record.Name != "RBOSTORETABLE" && record.Name != "RBOTERMINALTABLE" && record.Name != "RBOSTORETENDERTYPETABLE" && record.Name != "RBOLOCATIONPRICEGROUP"

                            {
                                if (param.LocalStore == "")
                                {
                                    if (!hasPostedWarning)
                                    {
                                        hasPostedWarning = true;
                                        MessageDialog.Show(Properties.Resources.NotAllCouldBeInserted);
                                    }

                                    abortRecord = true;
                                    break;
                                }

                                field.Value = (string)param.LocalStore;
                            }
                        }

                        if(!abortRecord && !dbProvider.Exists(PluginEntry.DataModel, record.Name,primaryKeys))
                        {
                            // Then if it did not exist then we insert the record. We need to remember to replace dataareaID with currently set dataarea ID
                            // also if we find store ID then we need to replace it with current store ID same goes for RESTAURANTID
                            foreach (DBField field in columns)
                            {
                                fieldName = field.Name.ToUpperInvariant();

                                foundNode = record.SelectSingleNode(field.Name);

                                if (foundNode != null)
                                {
                                    SetfieldValue(field, record);   
                                }
                                else
                                {
                                    field.Value = DBNull.Value;
                                }

                                if (fieldName == "DATAAREAID")
                                {
                                    field.Value = PluginEntry.DataModel.Connection.DataAreaId;
                                }
                                else if ((fieldName == "STOREID" || fieldName == "RESTAURANTID")
                                && (field.Value == DBNull.Value || (string)field.Value != "")
                                && !scriptInfo.ScriptName.ToLower().Contains("demo data")
                                && !scriptInfo.ScriptName.ToLower().Contains("template data")) 
                                {
                                    if (param.LocalStore == "")
                                    {
                                        if (!hasPostedWarning)
                                        {
                                            hasPostedWarning = true;
                                            MessageDialog.Show(Properties.Resources.NotAllCouldBeInserted);
                                        }

                                        abortRecord = true;
                                        break;
                                    }

                                    field.Value = (string)param.LocalStore;
                                }
                            }

                            if (!abortRecord)
                            {
                                dbProvider.Insert(PluginEntry.DataModel, record.Name, columns);
                            }
                        }

                        Invoke((Action)(() => progressBar.PerformStep()));
                        record = record.NextSibling;
                    }
                }
            }
            catch(Exception)
            {
                string message = Properties.Resources.ErrorLoadingPackage.Replace("#1", GetLocalizedScriptName(scriptInfo));

                if (InvokeRequired) //If this is running on another thread, invoke the message on the UI thread
                {
                    Invoke(new MethodInvoker(delegate () { MessageDialog.Show(message); }));
                }
                else
                {
                    MessageDialog.Show(message);
                }
            }
            finally
            {
                PluginEntry.DataModel.DisableReplicationActionCreation = originalDisabledReplicationAction;
            }
        }

        void lvScripts_ToggleCheck(object sender, EventArgs args)
        {
            ((CheckBoxCell)lvScripts.Selection[0][0]).Checked = !((CheckBoxCell)lvScripts.Selection[0][0]).Checked;
        }

        void lvScripts_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            lvScripts.ContextMenuStrip.Items.Clear();

            if (lvScripts.Selection.Count > 0)
            {
                if (((CheckBoxCell)lvScripts.Selection[0][0]).Checked)
                {
                    lvScripts.ContextMenuStrip.Items.Add(new ExtendedMenuItem(
                        Properties.Resources.Uncheck,
                        100,
                        new EventHandler(lvScripts_ToggleCheck)));
                }
                else
                {
                    lvScripts.ContextMenuStrip.Items.Add(new ExtendedMenuItem(
                        Properties.Resources.Check,
                        100,
                        new EventHandler(lvScripts_ToggleCheck)));
                }

                PluginEntry.Framework.ContextMenuNotify("InsertDefaultData", lvScripts.ContextMenuStrip, lvScripts);

                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        private void lvScripts_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            foreach (Row item in lvScripts.Rows)
            {
                if (((CheckBoxCell)item[0]).Checked)
                {
                    btnOK.Enabled = true;
                    return;
                }
            }

            btnOK.Enabled = false;
        }
    }
}
