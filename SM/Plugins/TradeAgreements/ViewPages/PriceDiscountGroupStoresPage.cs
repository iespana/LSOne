using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting.Contexts;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TradeAgreements.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.TradeAgreements.ViewPages
{
    public partial class PriceDiscountGroupStoresPage : UserControl, ITabView
    {
        private WeakReference storeViewer;

        PriceDiscGroupEnum displayType;

        WeakReference owner;
        RecordIdentifier groupID = "";
        
        public PriceDiscountGroupStoresPage()
        {
            displayType = PriceDiscGroupEnum.PriceGroup;

            InitializeComponent();

            lvStores.ContextMenuStrip = new ContextMenuStrip();
            lvStores.ContextMenuStrip.Opening += lvStores_Opening;

            var plugin = PluginEntry.Framework.FindImplementor(this, "CanEditStore", null);
            storeViewer = plugin != null ? new WeakReference(plugin) : null;

            contextButtonsStores.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EditPriceGroups);
        }

        internal PriceDiscountGroupStoresPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PriceDiscountGroupStoresPage((TabControl)sender);
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PriceDiscountGroups", 1, Properties.Resources.PriceDiscountGroups, false));
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            groupID = (RecordIdentifier)context;
            displayType = (PriceDiscGroupEnum)internalContext;

            LoadLines();
        }

        public void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Customer":
                    LoadLines();
                    break;
            }
        }

        public bool DataIsModified()
        {
            // Here our sheet is supposed to figure out if something needs to be saved and return
            // true if something needs to be saved, else false.
            return false;
        }

        public bool SaveData()
        {
            // Here we would let our sheet save our data.

            // We return true since saving was successful, if we would return false then
            // the viewstack will prevent other sheet from getting shown.
            return true;
        }

        private void LoadLines()
        {
            var groupType = GetSelectedType();

            // Set the Stores in group lines

            lvStores.ClearRows();

            RecordIdentifier priceGroupID = groupID.SecondaryID.SecondaryID;

            var stores = Providers.PriceDiscountGroupData.GetStoresInPriceGroup(PluginEntry.DataModel, priceGroupID);

            foreach (var store in stores)
            {
                
                Row row = new Row();

                row.AddText((string)store.ID);
                row.AddText((string)store.Text);
                row.AddText(store.Level.ToString());

                row.Tag = store.ID;
                lvStores.AddRow(row);
            }

            lvStores_SelectedIndexChanged(this, EventArgs.Empty);

            lvStores.AutoSizeColumns();
        }

        void lvStores_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvStores.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Add + "...",
                100,
                btnAddStore_Click)
                {
                    Enabled = contextButtonsStores.AddButtonEnabled,
                    Image = ContextButtons.GetAddButtonImage()
                };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    200,
                    btnRemoveStore_Click)
                {
                    Image = ContextButtons.GetRemoveButtonImage(),
                    Enabled = contextButtonsStores.RemoveButtonEnabled
                };
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 300));

            item = new ExtendedMenuItem(
                    Properties.Resources.ViewStore + "...",
                    400,
                    lvStores_DoubleClick)
                {
                    Enabled = btnViewStore.Enabled,
                    Default = true,
                    Image = ContextButtons.GetEditButtonImage()
                };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("StoresInPriceDiscGroupList", lvStores.ContextMenuStrip, lvStores);

            e.Cancel = (menu.Items.Count == 0);
        }

        private PriceDiscGroupEnum GetSelectedType()
        {
            return displayType;
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            LoadLines();
        }

        private void lvStores_SelectedIndexChanged(object sender, EventArgs e)
        {
            contextButtonsStores.RemoveButtonEnabled = (lvStores.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(Permission.EditPriceGroups);
            btnViewStore.Enabled = (lvStores.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(Permission.StoreView) && (storeViewer != null && storeViewer.IsAlive);
        }

        private void btnEditStore_Click(object sender, EventArgs e)
        {
            if (storeViewer != null && storeViewer.IsAlive)
            {          
                    ((IPlugin)storeViewer.Target).Message(this, "EditStore", ((RecordIdentifier)lvStores.Row(lvStores.Selection.FirstSelectedRow).Tag).PrimaryID);               
            }
        }

        private void btnAddStore_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.StoreInPriceGroupDialog(groupID, false);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadLines();
            }
        }

        private void btnRemoveStore_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
               Properties.Resources.DeleteStoreFromPriceGroupQuestion,
               Properties.Resources.DeleteStoreFromPriceGroup) == DialogResult.Yes)
            {
                Providers.PriceDiscountGroupData.RemoveStoreFromPriceGroup(PluginEntry.DataModel, (RecordIdentifier)lvStores.Row(lvStores.Selection.FirstSelectedRow).Tag, groupID.SecondaryID.SecondaryID);

                LoadLines();
            }
        }

        private void lvStores_DoubleClick(object sender, EventArgs e)
        {
            if (btnViewStore.Enabled)
            {
                btnEditStore_Click(this, null);
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        private void lvStores_SelectionChanged(object sender, EventArgs e)
        {
            contextButtonsStores.RemoveButtonEnabled = (lvStores.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(Permission.EditPriceGroups);
            btnViewStore.Enabled = (lvStores.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(Permission.StoreView) && (storeViewer != null && storeViewer.IsAlive);

        }

        private void lvStores_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnViewStore.Enabled)
            {
                btnEditStore_Click(this, null);
            }
        }
    }
}
