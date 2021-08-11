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
using LSOne.ViewPlugins.RMSMigration.Properties;

namespace LSOne.ViewPlugins.RMSMigation.Dialogs.WizardPages
{
    public partial class StoreLookupPanel : UserControl, IWizardPage
    {
        WizardBase parent;
        private string RMSConnectionString { get; set; }
        private List<RMSStore> RMSStores { get; set; }
        private List<StoreListItem> LSOneStores { get; set; }

        public int LSOneStoreGridColumnIndex
        {
            get
            {
                return storesGrid.Columns["LSOneStore"].Index;
            }
        }

        public StoreLookupPanel(WizardBase parent, string rmsConnectionString)
            : this()
        {
            this.parent = parent;
            this.RMSConnectionString = rmsConnectionString;
            parent.NextEnabled = false;
            btnCreateStores.Enabled = false;
        }

        private StoreLookupPanel()
        {
            InitializeComponent();
        }


        #region IWizardPage Members
        public bool HasFinish
        {
            get { return false; }
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
            LoadData();
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            return new TerminalLookupPanel(parent, RMSConnectionString, RMSStores);
        }

        public void ResetControls()
        {

        }

        #endregion

        #region Private Methods

        private void CanNextExecute()
        {
            parent.NextEnabled = RMSStores.All(s => s.LSOneStore != RecordIdentifier.Empty && s.LSOneStore != null);
        }

        private void LoadData()
        {
            LoadLSOneStores();
            LoadRMSStores();
            BindStores();
            if (RMSStores.Count == 0)
            {
                parent.NextEnabled = true;
            }
        }

        private void LoadRMSStores()
        {
            MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
            LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
            if (result != LoginResult.Success)
            {
                RMSStores = new List<RMSStore>();
                return;
            }

            RMSStores = entry.Connection.ExecuteReader(Constants.GET_ALL_STORES_SQL).ToDataTable().ToList<RMSStore>();
            if (RMSStores.Count() == 0)
            {
                RMSStores.Add(new RMSStore() { Name = Resources.N_AStore });
            }
        }

        private void LoadLSOneStores()
        {
            LSOneStores = Providers.StoreData.Search(parent.Connection, new StoreListSearchFilter { Sort = StoreSorting.Name });
            LSOneStores.ForEach(s => s.DisplayName = string.Format("{0} - {1}", s.ID, s.Text));
            LSOneStores.Insert(0, new StoreListItem() { ID = RecordIdentifier.Empty, Text = string.Empty, DisplayName = string.Empty });
            cbCreateStores.DataSource = LSOneStores;
            cbCreateStores.DisplayMember = "DisplayName";
        }

        private void BindStores()
        {
            BindStoreColumn();
            storesGrid.DataSource = null;
            storesGrid.DataSource = RMSStores;
        }

        private void BindStoreColumn()
        {
            DataGridViewComboBoxColumn storeColumn = storesGrid.Columns[LSOneStoreGridColumnIndex] as DataGridViewComboBoxColumn;
            storeColumn.DisplayMember = "DisplayName";
            storeColumn.ValueMember = "ID";
            storeColumn.DataSource = LSOneStores;
        }

        #endregion

        #region Control Events
        private void storesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == LSOneStoreGridColumnIndex && e.RowIndex != -1)
            {
                DataGridViewComboBoxCell combo = storesGrid[LSOneStoreGridColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;

                RMSStore editStore = (RMSStore)storesGrid.Rows[e.RowIndex].DataBoundItem;
                List<RecordIdentifier> selectedLSOneStores = RMSStores.Where(s => s.LSOneStore != RecordIdentifier.Empty && s.LSOneStore != editStore.LSOneStore).Select(s => s.LSOneStore).ToList();
                combo.DataSource = LSOneStores.Where(s => !selectedLSOneStores.Contains(s.ID)).ToList();
            }
        }

        private void storesGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox combo = (ComboBox)e.Control;
            combo.DropDownClosed += Combo_DropDownClosed;
        }

        private void Combo_DropDownClosed(object sender, EventArgs e)
        {
            storesGrid.BeginInvoke(new MethodInvoker(() =>
            {
                System.Threading.Thread.Sleep(1);
                storesGrid.EndEdit();
                CanNextExecute();
            }));
        }

        private void storesGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CanNextExecute();
        }

        private void cbCreateStores_SelectedIndexChanged(object sender, EventArgs e)
        {
            StoreListItem selectedStore = (StoreListItem)cbCreateStores.SelectedItem;
            btnCreateStores.Enabled = selectedStore != null && !string.IsNullOrEmpty(selectedStore.ID.ToString());
        }

        private void storesGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnCreateStores_Click(object sender, EventArgs e)
        {
            StoreListItem selectedStore = (StoreListItem)cbCreateStores.SelectedItem;
            if (selectedStore == null || string.IsNullOrEmpty(selectedStore.ID.ToString()))
            {
                return;
            }
            foreach (RMSStore rmsStore in RMSStores.Where(s => s.LSOneStore == RecordIdentifier.Empty || s.LSOneStore == null))
            {
                LSOne.DataLayer.BusinessObjects.StoreManagement.Store store;
                store = Providers.StoreData.Get(PluginEntry.DataModel, (string)selectedStore.ID);
                store.ID = RecordIdentifier.Empty;
                store.Text = rmsStore.Name;
                Providers.StoreData.Save(PluginEntry.DataModel, store);
                StoreListItem newStore = new StoreListItem() { ID = store.ID, Text = store.Text };
                rmsStore.LSOneStore = newStore.ID;
            }
            LoadLSOneStores();
            BindStores();
            CanNextExecute();
        }
        #endregion

    }
}
