using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TenderDeclaration;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.EndOfDay.ViewPages
{
    public partial class StoreTenderDeclarationPage : UserControl, ITabView
    {
        RecordIdentifier storeID;
        Store store;

        public StoreTenderDeclarationPage()
        {
            InitializeComponent();

            DoubleBuffered = true;

            lvTenderDeclarations.SmallImageList = PluginEntry.Framework.GetImageList();

            lvTenderDeclarations.ContextMenuStrip = new ContextMenuStrip();
            lvTenderDeclarations.ContextMenuStrip.Opening += lvTenderDeclarations_ContextMenuStripOpening;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.TenderDeclaration);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StoreTenderDeclarationPage();
        }

        public void LoadItems()
        {
            lvTenderDeclarations.Items.Clear();

            List<Tenderdeclaration> tenderDeclarations = Providers.TenderDeclarationData.GetTenderDeclarations(PluginEntry.DataModel, storeID);

            foreach (Tenderdeclaration td in tenderDeclarations)
            {
                var item = new ListViewItem(td.CountedTime.ToString());
                item.Tag = td.CountedTime;
                item.SubItems.Add(td.TerminalID);
                item.SubItems.Add((td.StatementID == null || td.StatementID == "") ? Properties.Resources.NotSpecified : td.StatementID.ToString());
                lvTenderDeclarations.Add(item);
            }

            lvTenderDeclarations_SelectedIndexChanged(this, EventArgs.Empty);
            lvTenderDeclarations.BestFitColumns();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            storeID = context;
            store = (Store)internalContext;

            LoadItems();
        }



        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvTenderDeclarations.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void lvTenderDeclarations_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);   
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ListViewItem item = lvTenderDeclarations.Items[lvTenderDeclarations.SelectedIndices[0]];
            Tenderdeclaration td = Providers.TenderDeclarationData.Get(PluginEntry.DataModel, (DateTime)item.Tag);
            Dialogs.TenderdeclarationDialog ntd = new Dialogs.TenderdeclarationDialog(td);
            if (ntd.ShowDialog() == DialogResult.OK)
            {
                Providers.TenderDeclarationData.Save(PluginEntry.DataModel, ntd.Tenderdeclaration);
                foreach (var SCTDLine in ntd.Tenderdeclaration.TenderDeclarationLines)
                {
                    Providers.TenderDeclarationLineData.Save(PluginEntry.DataModel, SCTDLine);
                }
            }
            LoadItems();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.TenderdeclarationDialog ntd = new Dialogs.TenderdeclarationDialog(store.ID);
            if (ntd.ShowDialog() == DialogResult.OK)
            {
                Providers.TenderDeclarationData.Save(PluginEntry.DataModel, ntd.Tenderdeclaration);
                foreach (var SCTDLine in ntd.Tenderdeclaration.TenderDeclarationLines)
                {
                    Providers.TenderDeclarationLineData.Save(PluginEntry.DataModel, SCTDLine);
                }
                LoadItems();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            // TODO: PluginOperations.DeleteTerminal((string)lvTerminals.SelectedItems[0].Tag);
            if (QuestionDialog.Show(Properties.Resources.DeleteDeclarationQuestion, Properties.Resources.DeleteDeclaration) == System.Windows.Forms.DialogResult.Yes)
            {
                ListViewItem item = lvTenderDeclarations.Items[lvTenderDeclarations.SelectedIndices[0]];
                Providers.TenderDeclarationData.Delete(PluginEntry.DataModel, (DateTime)item.Tag);

                LoadData(false, storeID, store);
            }
        }

        void lvTenderDeclarations_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvTenderDeclarations.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("StoreTenderDeclaration", lvTenderDeclarations.ContextMenuStrip, lvTenderDeclarations);
            e.Cancel = false;
        }

        private void lvTenderDeclarations_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = lvTenderDeclarations.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.TenderDeclaration);
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
        }
    }
}
