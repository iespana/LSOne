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

namespace LSOne.ViewPlugins.RMSMigation.Dialogs.WizardPages
{
    public partial class TerminalLookupPanel : UserControl, IWizardPage
    {
        WizardBase parent;
        private string RMSConnectionString { get; set; }
        private List<RMSTerminal> RMSTerminals { get; set; }
        private List<TerminalListItem> LSOneTerminals { get; set; }
        private List<RMSStore> StoreLookup { get; set; }

        private int LSOneTerminalGridColumnIndex
        {
            get
            {
                return terminalsGrid.Columns["LSOneTerminal"].Index;
            }
        }

        public TerminalLookupPanel(WizardBase parent, string rmsConnectionString, List<RMSStore> storeLookup)
            : this()
        {
            this.parent = parent;
            this.RMSConnectionString = rmsConnectionString;
            this.StoreLookup = storeLookup;
            parent.NextEnabled = false;
            btnCreateTerminals.Enabled = false;
        }

        private TerminalLookupPanel()
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
            return new ImportPanel(parent, RMSConnectionString, StoreLookup, RMSTerminals);
        }

        public void ResetControls()
        {

        }

        #endregion

        #region Private Methods

        private void CanNextExecute()
        {
            parent.NextEnabled = RMSTerminals.All(s => s.LSOneTerminal != RecordIdentifier.Empty && s.LSOneTerminal != null);
        }

        private void LoadData()
        {
            LoadLSOneTerminals();
            LoadRMSTerminals();
            BindTerminal();
            if (RMSTerminals.Count == 0)
            {
                parent.NextEnabled = true;
            }
        }

        private void LoadRMSTerminals()
        {
            MiniSqlServerConnectionManager entry = new MiniSqlServerConnectionManager();
            LoginResult result = entry.Login(RMSConnectionString, ConnectionUsageType.UsageNormalClient, "XYZ", false, false);
            if (result != LoginResult.Success)
            {
                RMSTerminals = new List<RMSTerminal>();
                return;
            }

            RMSTerminals = entry.Connection.ExecuteReader(Constants.GET_ALL_REGISTERS_SQL).ToDataTable().ToList<RMSTerminal>();
        }

        private void LoadLSOneTerminals()
        {
            LSOneTerminals = Providers.TerminalData.GetAllTerminals(parent.Connection, false, TerminalListItem.SortEnum.NAME);
            LSOneTerminals.ForEach(s => s.DisplayName = string.Format("{0} - {1}", s.ID, s.Name));
            LSOneTerminals.Insert(0, new TerminalListItem() { ID = RecordIdentifier.Empty, Name = string.Empty, DisplayName = string.Empty });
            cbCreateTerminals.DataSource = LSOneTerminals;
            cbCreateTerminals.DisplayMember = "DisplayName";
        }

        private void BindTerminal()
        {
            BindTerminalColumn();
            terminalsGrid.DataSource = null;
            terminalsGrid.DataSource = RMSTerminals;
        }

        private void BindTerminalColumn()
        {
            DataGridViewComboBoxColumn terminalColumn = terminalsGrid.Columns[LSOneTerminalGridColumnIndex] as DataGridViewComboBoxColumn;
            terminalColumn.DisplayMember = "DisplayName";
            terminalColumn.ValueMember = "ID";
            terminalColumn.DataSource = LSOneTerminals;
        }

        #endregion

        #region Control Events
        private void terminalsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == LSOneTerminalGridColumnIndex && e.RowIndex != -1)
            {
                DataGridViewComboBoxCell combo = terminalsGrid[LSOneTerminalGridColumnIndex, e.RowIndex] as DataGridViewComboBoxCell;

                RMSTerminal editTerminal = (RMSTerminal)terminalsGrid.Rows[e.RowIndex].DataBoundItem;
                List<RecordIdentifier> selectedLSOneTerminals = RMSTerminals.Where(s => s.LSOneTerminal != RecordIdentifier.Empty && s.LSOneTerminal != editTerminal.LSOneTerminal).Select(s => s.LSOneTerminal).ToList();
                combo.DataSource = LSOneTerminals.Where(s => !selectedLSOneTerminals.Contains(s.ID)).ToList();
            }
        }

        private void terminalsGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (!(e.Control is ComboBox))
            {
                return;
            }
            ComboBox combo = (ComboBox)e.Control;
            combo.DropDownClosed += Combo_DropDownClosed;
        }

        private void Combo_DropDownClosed(object sender, EventArgs e)
        {
            terminalsGrid.BeginInvoke(new MethodInvoker(() =>
            {
                System.Threading.Thread.Sleep(1);
                terminalsGrid.EndEdit();
                CanNextExecute();
            }));
        }

        private void terminalsGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            CanNextExecute();
        }

        private void cbCreateTerminals_SelectedIndexChanged(object sender, EventArgs e)
        {
            TerminalListItem selectedTerminal = (TerminalListItem)cbCreateTerminals.SelectedItem;
            btnCreateTerminals.Enabled = selectedTerminal != null && !string.IsNullOrEmpty(selectedTerminal.ID.ToString());
        }

        private void terminalsGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnCreateTerminals_Click(object sender, EventArgs e)
        {
            TerminalListItem selectedTerminal = (TerminalListItem)cbCreateTerminals.SelectedItem;
            if (selectedTerminal == null || string.IsNullOrEmpty(selectedTerminal.ID.ToString()))
            {
                return;
            }
            foreach (RMSTerminal rmsTerminal in RMSTerminals.Where(s => s.LSOneTerminal == RecordIdentifier.Empty || s.LSOneTerminal == null))
            {
                LSOne.DataLayer.BusinessObjects.StoreManagement.Terminal terminal;
                terminal = Providers.TerminalData.Get(PluginEntry.DataModel, (string)selectedTerminal.ID, selectedTerminal.StoreID);
                terminal.ID = RecordIdentifier.Empty;
                terminal.Text = rmsTerminal.TerminalDescription;
                RMSStore store = StoreLookup.FirstOrDefault(el => el.StoreID == rmsTerminal.StoreID);
                if (store != null)
                {
                    terminal.StoreID = store.LSOneStore;
                }
                Providers.TerminalData.Save(PluginEntry.DataModel, terminal);
                TerminalListItem newTerminal = new TerminalListItem() { ID = terminal.ID, Name = terminal.Text };
                rmsTerminal.LSOneTerminal = newTerminal.ID;
            }
            LoadLSOneTerminals();
            BindTerminal();
            CanNextExecute();
        }
        #endregion
    }
}
