using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Terminals.ViewPages
{
    public partial class StoreTerminalsPage : UserControl, ITabView
    {
        RecordIdentifier storeID;
        Store store;

        public StoreTerminalsPage()
        {
            InitializeComponent();

            DoubleBuffered = true;

            lvTerminals.SmallImageList = PluginEntry.Framework.GetImageList();

            lvTerminals.ContextMenuStrip = new ContextMenuStrip();
            lvTerminals.ContextMenuStrip.Opening += lvTerminals_ContextMenuStripOpening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new StoreTerminalsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            ListViewItem item;
            List<TerminalListItem> terminals;

            storeID = context;
            store = (Store)internalContext;

            btnContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.TerminalEdit) &&
                (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == storeID);

            lvTerminals.SuspendLayout();
            lvTerminals.Items.Clear();
            
            

            terminals = Providers.TerminalData.GetList(PluginEntry.DataModel, storeID);

            foreach (TerminalListItem terminal in terminals)
            {
                item = new ListViewItem((string)terminal.ID,PluginEntry.TerminalImageIndex);
                item.SubItems.Add(terminal.Text);
                item.Tag = new RecordIdentifier(terminal.ID, terminal.StoreID);

                lvTerminals.Add(item);
            }

            lvTerminals.BestFitColumns();

            lvTerminals_SelectedIndexChanged(this, EventArgs.Empty);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Terminal":
                    LoadData(false, storeID, store);
                    break;
            }
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvTerminals.SmallImageList = null;
        }

        #endregion

        private void lvTerminals_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnContextButtons.EditButtonEnabled = btnContextButtons.RemoveButtonEnabled =
                lvTerminals.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.TerminalView);
        }

        private void lvTerminals_DoubleClick(object sender, EventArgs e)
        {
            if (btnContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            RecordIdentifier terminalID = (RecordIdentifier)lvTerminals.SelectedItems[0].Tag;
            PluginOperations.ShowTerminal(terminalID[0], terminalID[1]);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewTerminal(storeID, store.Text);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteTerminal((RecordIdentifier)lvTerminals.SelectedItems[0].Tag);
        }

        void lvTerminals_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            lvTerminals.ContextMenuStrip.Items.Clear();

         
            ExtendedMenuItem item;

            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    ContextButtons.GetEditButtonImage(),
                    100,
                    btnEdit_Click);
            item.Default = true;
            item.Enabled = btnContextButtons.EditButtonEnabled;
            lvTerminals.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    ContextButtons.GetAddButtonImage(),
                    110,
                    btnAdd_Click);
            item.Enabled = btnContextButtons.AddButtonEnabled;
            lvTerminals.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    ContextButtons.GetRemoveButtonImage(),
                    120,
                    btnRemove_Click);
            item.Enabled = btnContextButtons.RemoveButtonEnabled;
            lvTerminals.ContextMenuStrip.Items.Add(item);  

            PluginEntry.Framework.ContextMenuNotify("StoreTerminals", lvTerminals.ContextMenuStrip, lvTerminals);

            e.Cancel = false;
        }

        
    }
}
