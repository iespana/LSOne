using System;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.SqlConnector.MiniConnectionManager;
using LSOne.DataLayer.GenericConnector.Enums;
using System.Collections.Generic;
using System.Data;
using LSOne.ViewPlugins.RMSMigration.Model;
using LSOne.ViewPlugins.RMSMigration.Helper;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.Utilities.DataTypes;
using System.Linq;
using LSOne.ViewPlugins.RMSMigration;
using static LSOne.ViewPlugins.RMSMigration.Model.Enums;
using System.Threading;
using System.Threading.Tasks;
using LSOne.ViewPlugins.RMSMigration.Contracts;
using LSOne.ViewPlugins.RMSMigration.Model.Import;
using System.Drawing;
using LSOne.ViewPlugins.RMSMigration.Properties;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.ViewPlugins.RMSMigration.Dialogs;
using System.Diagnostics;

namespace LSOne.ViewPlugins.RMSMigation.Dialogs.WizardPages
{
    public partial class ImportPanel : UserControl, IWizardPage
    {
        WizardBase parent;
        private string RMSConnectionString { get; set; }

        public List<RMSMigrationItem> MigrationItems { get; set; }

        private List<RMSStore> StoreLookup { get; set; }
        private List<RMSTerminal> TerminalLookup { get; set; }

        private ILookupManager LookupManager = new LookupManager();

        public ImportPanel(WizardBase parent, string rmsConnectionString, List<RMSStore> storeLookup, List<RMSTerminal> terminalLookup)
            : this()
        {
            this.parent = parent;
            this.RMSConnectionString = rmsConnectionString;
            this.StoreLookup = storeLookup;
            this.TerminalLookup = terminalLookup;
            parent.NextEnabled = false;
            LookupManager.StoreLookup = StoreLookup.ToDictionary("StoreID", "LSOneStore");
            LookupManager.TerminalLookup = TerminalLookup.ToDictionary("TerminalID", "LSOneTerminal");
            SetDefaultUnitOfMeasure();

            DataGridViewButtonColumn logBtn = new DataGridViewButtonColumn();
            migrationItemsGrid.Columns.Add(logBtn);
            logBtn.HeaderText = Resources.Log;
            logBtn.Text = Resources.View;
            logBtn.Name = "Log";
            logBtn.UseColumnTextForButtonValue = true;
            logBtn.DefaultCellStyle.Padding = new Padding(8);
        }

        private ImportPanel()
        {
            InitializeComponent();
        }


        #region IWizardPage Members
        public bool HasFinish
        {
            get { return true; }
        }

        public bool HasForward
        {
            get { return true; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void Display()
        {
            SetBaseImagePath();
            LoadData();
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            throw new NotImplementedException();
        }

        public void ResetControls()
        {

        }

        #endregion

        #region Private Methods

        private void LoadData()
        {
            List<IDataEntity> barCodeSetups = Providers.BarCodeSetupData.GetList(parent.Connection).Cast<IDataEntity>().ToList();
            cbBarCodeSetup.DataSource = barCodeSetups;
            cbBarCodeSetup.DisplayMember = "Description";
            cbBarCodeSetup.SelectedIndex = -1;
            MigrationItems = new List<RMSMigrationItem>();
            foreach (RMSMigrationItemType migrationType in Enum.GetValues(typeof(RMSMigrationItemType)))
            {
                RMSMigrationItem migrationItem = new RMSMigrationItem() { ItemType = migrationType, ProgressStatus = ProgressStatus.Ready };
                migrationItem.PropertyChanged += MigrationItem_PropertyChanged;
                MigrationItems.Add(migrationItem);
            }
            BindDataGrid();
            btnImport.Enabled = false;
        }

        private void MigrationItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.BeginInvoke( new MethodInvoker(() => { BindDataGrid(); }));
        }

        #endregion

        #region Control Events

        #endregion

        private async void btnImport_Click(object sender, EventArgs e)
        {
            string rmsMigrationFlag = Providers.PosisInfoData.Get(PluginEntry.DataModel, Constants.RMSMigrationFlag);
            if (!string.IsNullOrEmpty(rmsMigrationFlag))
            {
                MessageDialog.Show(Resources.MigrationAlreadyBeenRun, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }

            LookupManager.BaseImagePath = tbImageFolderPath.Text;
            btnImport.Enabled = false;
            cbBarCodeSetup.Enabled = false;
            LookupManager.SetupBarcodeID = ((IDataEntity)cbBarCodeSetup.SelectedItem).ID;

            Providers.PosisInfoData.Save(PluginEntry.DataModel, new DataLayer.BusinessObjects.DataEntity() { ID = Constants.RMSMigrationFlag, Text = "1" });

            await Task.Run(() =>
            {
                foreach (RMSMigrationItem migrationItem in MigrationItems.Where(mi => mi.ShouldImport))
                {
                    SetProgressSize(0);
                    migrationItem.ProgressStatus = ProgressStatus.Importing;
                    SafeBindDataGrid();

                    IImportManager importer = migrationItem.ItemType.ResolveImporter();
                    importer.ReportProgress += ReportProgress;
                    importer.SetProgressSize += SetProgressSize;
                    List<ImportLogItem> logItems = importer.Import(LookupManager, RMSConnectionString);
                    migrationItem.LogItems = logItems;
                    migrationItem.ProgressStatus = logItems.Count > 0 ? ProgressStatus.Error : ProgressStatus.Done;
                    SafeBindDataGrid();
                }
            });

            SetProgressSize(0);
            parent.NextEnabled = true;
            cbBarCodeSetup.Enabled = true;
        }

        private void SetProgressSize(int itemsNumber)
        {
            progressBar.Invoke(new MethodInvoker(() =>
            {
                progressBar.Minimum = 0;
                progressBar.Maximum = itemsNumber;
                progressBar.Value = 0;
                DisplayImportingMessage();
            }));
        }

        private void ReportProgress()
        {
            progressBar.Invoke(new MethodInvoker(() =>
            {
                if (progressBar.Maximum > progressBar.Value)
                {
                    DisplayImportingMessage();
                    progressBar.Value++;
                }
            }));
        }

        public Stopwatch sw = new Stopwatch();
        private void DisplayImportingMessage()
        {
            string remaining = string.Empty;
            if (progressBar.Value == 1)
            {
                sw.Start();
            }
            if (progressBar.Value == 2)
            {
                sw.Stop();
            }
            if (progressBar.Value >= 2)
            {
                var time = TimeSpan.FromTicks(sw.Elapsed.Ticks * (progressBar.Maximum - progressBar.Value));
                remaining = string.Format(" - ( " + Resources.Remaining + " {0} )", time.ToString(@"hh\:mm\:ss"));
            }
            lblInfo.Text = string.Format(Resources.ImportingMessage, progressBar.Value, progressBar.Maximum) +
                           (string.IsNullOrEmpty(remaining) ? string.Empty : remaining);
        }

        private void SafeBindDataGrid()
        {
            if (!migrationItemsGrid.IsHandleCreated) return;
            migrationItemsGrid.Invoke(new MethodInvoker(() =>
            {
                BindDataGrid();
            }));
        }
        private void BindDataGrid()
        {
            migrationItemsGrid.DataSource = null;
            migrationItemsGrid.DataSource = MigrationItems;
        }

        private void migrationItemsGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void migrationItemsGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            migrationItemsGrid.Invoke(new MethodInvoker(() =>
            {
                migrationItemsGrid.EndEdit();
            }));
        }

        private void migrationItemsGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in migrationItemsGrid.Rows)
            {
                RMSMigrationItem data = (RMSMigrationItem)row.DataBoundItem;
                if (data.ProgressStatus == ProgressStatus.Error)// Or your condition 
                {
                    row.DefaultCellStyle.ForeColor = Color.Red;
                }
                else if (data.ProgressStatus == ProgressStatus.Done)
                {
                    row.DefaultCellStyle.ForeColor = Color.Green;
                }
                else if (data.ProgressStatus == ProgressStatus.Ignored)
                {
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(220, 169, 0);
                }
                else if (data.ProgressStatus == ProgressStatus.Importing)
                {
                    row.DefaultCellStyle.ForeColor = Color.Blue;
                }
                else if (data.ProgressStatus == ProgressStatus.Ready)
                {
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
                if (!string.IsNullOrEmpty(data.ErrorMessage))
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.ToolTipText = data.ErrorMessage;
                    }
                }
            }
        }

        private void cbBarCodeSetup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBarCodeSetup.SelectedItem != null && cbBarCodeSetup.SelectedItem is IDataEntity)
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }
        }

        private void SetDefaultUnitOfMeasure()
        {
            List<Unit> units = Providers.UnitData.GetAllUnits(PluginEntry.DataModel);
            if (units == null || units.Find(el => el.Text.Equals(Constants.EachUnitOfMeasure)) == null)
            {
                Unit each = new Unit() { Text = Constants.EachUnitOfMeasure };
                Providers.UnitData.Save(PluginEntry.DataModel, each);
                LookupManager.DefaultUnitOfMeasureID = each.ID;
            }
            else
            {
                LookupManager.DefaultUnitOfMeasureID = units.Find(el => el.Text.Equals(Constants.EachUnitOfMeasure)).ID;
            }

            units = Providers.UnitData.GetAllUnits(PluginEntry.DataModel);
            LookupManager.UnitOfMeasure = new Dictionary<string, RecordIdentifier>();
            units.ForEach(u =>
            {
                if (!LookupManager.UnitOfMeasure.ContainsKey(u.Text))
                {
                    LookupManager.UnitOfMeasure.Add(u.Text, u.ID);
                }
            });
        }

        private void migrationItemsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1)
            {
                return;
            }
            DataGridViewColumn dc = migrationItemsGrid.Columns[e.ColumnIndex];
            if (dc.Name == "Log" && e.RowIndex != -1)
            {
                var row = migrationItemsGrid.Rows[e.RowIndex];
                RMSMigrationItem data = (RMSMigrationItem)row.DataBoundItem;
                ItemLogForm logForm = new ItemLogForm(data.LogItems);
                logForm.ShowDialog();
            }
        }

        private void selectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                tbImageFolderPath.Text = fbd.SelectedPath;
            }
        }

        private void SetBaseImagePath()
        {
            MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
            LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
            if (result != LoginResult.Success)
            {
                return;
            }
            DataTable dt = entry.Connection.ExecuteReader(Constants.GET_IMAGE_FOLDER_RELATIVE_PATH).ToDataTable();

            if (dt != null && dt.Rows.Count == 1 && dt.Columns.Count == 1)
            {
                if (dt.Rows[0][0] != null)
                {
                    tbImageFolderPath.Text = dt.Rows[0][0].ToString();
                }
            }
        }
    }
}
