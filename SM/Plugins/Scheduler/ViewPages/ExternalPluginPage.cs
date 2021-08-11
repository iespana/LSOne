using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO.JSON;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSOne.ViewPlugins.Scheduler.Interfaces;
using LSOne.ViewPlugins.Scheduler.Utils;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class ExternalPlugin : UserControl, ITabView
    {
        private JobViewPageContext internalContext;
        private bool dataIsModified;
        private Dictionary<string, string> parameters;
        private bool usingSetupControlProvider;
        private ISchedulerActionSetupControlProvider setupControlProvider = null;
        private string pluginFullPath = "";
        public string PluginPath { get; set; }
        public string PluginParameters { get; set; }

        private string DDInstallPath = "c:\\Program Files (x86)\\LS Retail\\Data Director 3\\bin\\plugins";

        public ExternalPlugin()
        {
            InitializeComponent();

            parameters = new Dictionary<string, string>();
            usingSetupControlProvider = false;

            lvParameters.ContextMenuStrip = new ContextMenuStrip();
            lvParameters.ContextMenuStrip.Opening += lvJobSubJobs_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ExternalPlugin();
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                this.internalContext = (JobViewPageContext)internalContext;
            }

            IdleOneShotProcessing.PostRun(ClearDataIsModified);

            tbPluginName.Text = this.internalContext.Job.PluginPath;

            // To be able to load the actual plugin file we'll assume that it's in the DD install folder. 
            // TODO: Add some other way of finding the file so we don't just *assume* that the file is located there
            pluginFullPath = DDInstallPath + "\\" + this.internalContext.Job.PluginPath;

            if (!string.IsNullOrEmpty(this.internalContext.Job.PluginArguments))
            {
                parameters = JsonConvert.DeserializeObject<Dictionary<string, string>>(this.internalContext.Job.PluginArguments);
            }

            if (File.Exists(pluginFullPath))
            {
                LoadSetupControlProvider(true);
            }

            if (!string.IsNullOrEmpty(this.internalContext.Job.PluginArguments) && !usingSetupControlProvider)
            {
                foreach (KeyValuePair<string, string> keyValuePair in parameters)
                {
                    var row = new Row();
                    var entity = new DataEntity()
                    {
                        ID = keyValuePair.Key,
                        Text = keyValuePair.Value
                    };
                    SetRow(row, entity);

                    if (!lvParameters.Rows.Select(x => x.Tag).Cast<DataEntity>().Any(x => x.ID == entity.ID))
                    {
                        lvParameters.AddRow(row);
                    }
                }                
            }

            lvParameters.AutoSizeColumns();
            UpdateActions();
        }

        private void SetRow(Row row, DataEntity entity)
        {
            row.Clear();
            row.AddText(entity.ID.ToString());
            row.AddText(entity.Text);

            row.Tag = entity;
        }

        private void ClearDataIsModified(object arg)
        {
            dataIsModified = false;
        }

        private void UpdateActions()
        {
            bool hasPermission = PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit);
            int selectedCount = lvParameters.Selection.Count;

            btnMoveUp.Enabled = selectedCount > 0 && lvParameters.Selection.GetRowIndex(0) > 0;
            btnMoveDown.Enabled = selectedCount > 0 && lvParameters.Selection.GetRowIndex(selectedCount - 1) < lvParameters.RowCount - 1;

            contextButtons.EditButtonEnabled = selectedCount == 1;
            contextButtons.AddButtonEnabled = hasPermission;
            contextButtons.RemoveButtonEnabled = hasPermission && selectedCount >= 1;
        }

        public bool DataIsModified()
        {
            if (usingSetupControlProvider)
            {
                return dataIsModified || tbPluginName.Text != internalContext.Job.PluginPath || setupControlProvider.DataIsModified();
            }

            return dataIsModified || tbPluginName.Text != internalContext.Job.PluginPath;
        }

        public bool SaveData()
        {
            PluginPath = tbPluginName.Text;

            var parameters = usingSetupControlProvider
                ? setupControlProvider.GetParemeters()
                : lvParameters.Rows.ToDictionary(row => ((DataEntity) row.Tag).ID.ToString(), row => ((DataEntity) row.Tag).Text);

            PluginParameters = JsonConvert.SerializeObject(parameters);
            internalContext.Job.PluginArguments = PluginParameters;
            internalContext.Job.PluginPath = PluginPath;
            dataIsModified = false;
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            // TODO: Implement
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        }

        private void contextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            if (lvParameters.Selection.Count != 1)
                return;

            var selectedRow = lvParameters.Rows[lvParameters.Selection.FirstSelectedRow];
            var parameter = selectedRow.Tag as DataEntity;

            var parameterDialog = new ExternalJobParameter {Parameter = parameter};
            if (parameterDialog.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                SetRow(selectedRow, parameterDialog.Parameter);
                lvParameters.AutoSizeColumns();
                dataIsModified = true;
            }
        }

        private void contextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            var parameterDialog = new ExternalJobParameter();
            if (parameterDialog.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                var row = new Row();
                SetRow(row, parameterDialog.Parameter);
                lvParameters.AddRow(row);
                lvParameters.AutoSizeColumns();
                dataIsModified = true;
            }
        }

        private void contextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            string msg = null;
            string caption = null;
            if (lvParameters.Selection.Count == 1)
            {
                msg = Properties.Resources.RemoveParameterQuestion.Replace("%1",
                    (string) ((DataEntity) lvParameters.Rows[lvParameters.Selection.FirstSelectedRow].Tag).ID);
                caption = Properties.Resources.RemoveParameter;
            }
            else if (lvParameters.Selection.Count > 1)
            {
                msg = Properties.Resources.RemoveParametersQuestion.Replace("%1", lvParameters.Selection.Count.ToString());
                caption = Properties.Resources.RemoveParameters;
            }

            if (msg != null)
            {
                if (QuestionDialog.Show(msg, caption) == DialogResult.Yes)
                {
                    var rowIndexes = new List<int>();
                    for (int i = 0; i < lvParameters.Selection.Count; i++)
                        rowIndexes.Add(lvParameters.Selection.GetRowIndex(i));
                    foreach (var rowIndex in rowIndexes)
                        lvParameters.RemoveRow(rowIndex);
                    dataIsModified = true;
                }
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (lvParameters.Selection.Count == 0)
                return;

            Debug.Assert(lvParameters.Selection.FirstSelectedRow > 0);

            for (int i = 0; i < lvParameters.Selection.Count; i++)
            {
                int selectedRowIndex = lvParameters.Selection.GetRowIndex(i);
                lvParameters.SwapRows(selectedRowIndex - 1, selectedRowIndex);
            }

            dataIsModified = true;
            UpdateActions();
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (lvParameters.Selection.Count == 0)
                return;

            Debug.Assert(lvParameters.Rows.IndexOf(lvParameters.Selection[lvParameters.Selection.Count - 1]) < lvParameters.RowCount - 1);

            for (int i = lvParameters.Selection.Count - 1; i >= 0; i--)
            {
                int selectedRowIndex = lvParameters.Selection.GetRowIndex(i);
                lvParameters.SwapRows(selectedRowIndex, selectedRowIndex + 1);
            }

            dataIsModified = true;
            UpdateActions();
        }

        private void lvJobSubJobs_SelectionChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void lvJobSubJobs_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (contextButtons.EditButtonEnabled)
            {
                contextButtons_EditButtonClicked(contextButtons, EventArgs.Empty);
            }
        }

        private void lvJobSubJobs_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvParameters.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                new EventHandler(contextButtons_EditButtonClicked));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = contextButtons.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Add,
                200,
                new EventHandler(contextButtons_AddButtonClicked));

            item.Enabled = contextButtons.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                300,
                new EventHandler(contextButtons_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = contextButtons.RemoveButtonEnabled;

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            string path = "";

            if (Directory.Exists(DDInstallPath))
            {
                path = DDInstallPath;
            }

            OpenFileDialog dlg = new OpenFileDialog
            {
                CheckFileExists = true,
                Multiselect = false,
                InitialDirectory = path,
                FileName = tbPluginName.Text
            };

            string pluginFileName = "";

            if (DialogResult.OK == dlg.ShowDialog())
            {
                pluginFileName = dlg.FileName;

                pluginFullPath = pluginFileName;
                tbPluginName.Text = Path.GetFileName(pluginFullPath);

                LoadSetupControlProvider(false);
            }
        }

        private void LoadSetupControlProvider(bool loadingData)
        {
            try
            {                
                Assembly assembly = Assembly.LoadFrom(pluginFullPath);
                bool setupControlProviderFound = false;

                Module[] moduleArray = assembly.GetModules();

                foreach (Module module in moduleArray)
                {
                    Type[] types = module.GetTypes();

                    foreach (Type type in types)
                    {
                        if (type.GetInterface(typeof(ISchedulerActionSetupControlProvider).FullName) != null)
                        {
                            string setupControlClassName = type.FullName;

                            ObjectHandle pluginHandle = Activator.CreateInstanceFrom(pluginFullPath, setupControlClassName, null);
                            setupControlProvider = (ISchedulerActionSetupControlProvider) pluginHandle.Unwrap();
                            setupControlProviderFound = true;
                            break;
                        }
                    }

                    if (setupControlProviderFound)
                    {
                        break;
                    }
                }

                if (setupControlProviderFound)
                {
                    usingSetupControlProvider = true;
                    pnlParameters.Visible = pnlParameters.Enabled = false;
                    setupControlProvider.InitializeParameters(parameters);
                    UserControl control = setupControlProvider.CreateSetupControl(PluginEntry.DataModel, parameters);
                    control.Dock = DockStyle.Fill;
                    pnlSetupControl.Controls.Add(control);
                    pnlSetupControl.Visible = pnlSetupControl.Enabled = true;
                }
                else
                {
                    usingSetupControlProvider = false;
                    pnlSetupControl.Visible = pnlSetupControl.Enabled = false;
                    pnlParameters.Visible = pnlParameters.Enabled = true;
                }
            }
            catch
            {
                // If it fails we just show the default setup view
                usingSetupControlProvider = false;
                pnlSetupControl.Visible = pnlSetupControl.Enabled = false;
                pnlParameters.Visible = pnlParameters.Enabled = true;
            }
        }
    }
}