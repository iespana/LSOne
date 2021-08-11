using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Store.ViewPages
{
    public partial class StorePaymentMethodPage : UserControl, ITabView
    {
        RecordIdentifier storeID;
        LSOne.DataLayer.BusinessObjects.StoreManagement.Store store;

        WeakReference paymentTypeEditor;

        public StorePaymentMethodPage()
        {
            IPlugin plugin;

            InitializeComponent();

            DoubleBuffered = true;
           
            lvPaymentMethods.ContextMenuStrip = new ContextMenuStrip();
            lvPaymentMethods.ContextMenuStrip.Opening += lvPaymentMethods_ContextMenuStripOpening;

            plugin = PluginEntry.Framework.FindImplementor(this,"CanEditPaymentTypes",null);

            paymentTypeEditor = (plugin != null) ? new WeakReference(plugin) : null;

            if (plugin != null)
            {
                btnPaymentTypes.Visible = true;
            }

            
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StorePaymentMethodPage();
        }


        #region ITabPanel Members

       

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            List<DataEntity> tenderList;
            ListViewItem listViewItem;

            storeID = context;
            store = (LSOne.DataLayer.BusinessObjects.StoreManagement.Store)internalContext;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.StoreEdit) && PluginEntry.DataModel.HasPermission(Permission.ManageAllowedPaymentSettings) &&
                (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty || PluginEntry.DataModel.CurrentStoreID == context);

            btnsContextButtons.RemoveButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageAllowedPaymentSettings);

            lvPaymentMethods.SuspendLayout();

            lvPaymentMethods.Items.Clear();

            tenderList = Providers.StorePaymentMethodData.GetList(PluginEntry.DataModel, storeID).OrderBy(o => o.Text).ToList();

            foreach (DataEntity item in tenderList)
            {
                listViewItem = new ListViewItem(item.ID.ToString());
                listViewItem.SubItems.Add(item.Text);
                listViewItem.Tag = new RecordIdentifier(storeID,item.ID);
                listViewItem.ImageIndex = 0;

                lvPaymentMethods.Add(listViewItem);
            }

            lvPaymentMethods.ResumeLayout();

            lvPaymentMethods_SelectedIndexChanged(null, EventArgs.Empty);

            lvPaymentMethods.BestFitColumns();
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
            switch (objectName)
            {
                case "StorePaymentMethod":
                    if ((changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Delete) && changeIdentifier.PrimaryID == storeID)
                    {
                        LoadData(false, storeID, store);
                        PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.InitialConfigurationDashboardID);
                    }
                    break;

                case "PaymentMethod":
                    LoadData(false, storeID, store);
                    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.InitialConfigurationDashboardID);
                    break;
            }
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvPaymentMethods.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void lvPaymentMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnPaymentTypes.Enabled = PluginEntry.DataModel.HasPermission(Permission.ManageAllowedPaymentSettings);
            
            btnsContextButtons.EditButtonEnabled = lvPaymentMethods.SelectedItems.Count > 0 &&
                                                   PluginEntry.DataModel.HasPermission(Permission.StoreEdit);

            btnsContextButtons.RemoveButtonEnabled = lvPaymentMethods.SelectedItems.Count > 0 && 
                                                     PluginEntry.DataModel.HasPermission(Permission.StoreEdit) && 
                                                     PluginEntry.DataModel.HasPermission(Permission.ManageAllowedPaymentSettings);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowStoreTender((RecordIdentifier)lvPaymentMethods.SelectedItems[0].Tag, store.Text);
        }

        private void lvPaymentMethods_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        void lvPaymentMethods_ContextMenuStripOpening(object sender, CancelEventArgs e)
        {
            lvPaymentMethods.ContextMenuStrip.Items.Clear();


            ExtendedMenuItem item;

            item = new ExtendedMenuItem(
                    Properties.Resources.EditPaymentMethod,
                    ContextButtons.GetEditButtonImage(),
                    100,
                    btnEdit_Click);
            item.Default = true;
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            lvPaymentMethods.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.NewPaymentMethod,
                    ContextButtons.GetAddButtonImage(),
                    110,
                    btnAdd_Click);
            item.Enabled = PluginEntry.DataModel.HasPermission(Permission.ManageAllowedPaymentSettings);
            lvPaymentMethods.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.DeletePaymentMethod,
                    ContextButtons.GetRemoveButtonImage(),
                    120,
                    btnRemove_Click);
            item.Enabled = PluginEntry.DataModel.HasPermission(Permission.ManageAllowedPaymentSettings);

            lvPaymentMethods.ContextMenuStrip.Items.Add(item);

            if (PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsView))
            {
            lvPaymentMethods.ContextMenuStrip.Items.Add(new ExtendedMenuItem("-", 140));
            }

            item = new ExtendedMenuItem(
                   btnPaymentTypes.Text,
                   null,
                   160,
                   new EventHandler(btnPaymentTypes_Click));
            item.Enabled = PluginEntry.DataModel.HasPermission(Permission.ManageAllowedPaymentSettings);
            item.Visible = PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsView);
           
            lvPaymentMethods.ContextMenuStrip.Items.Add(item);


            PluginEntry.Framework.ContextMenuNotify("StorePaymentMethods", lvPaymentMethods.ContextMenuStrip, lvPaymentMethods);

            e.Cancel = false;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RecordIdentifier itemID = (RecordIdentifier)lvPaymentMethods.SelectedItems[0].Tag;


            PluginOperations.DeleteStorePaymentMethod(itemID);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (Dialogs.NewStorePaymentMethodDialog dlg = new Dialogs.NewStorePaymentMethodDialog(storeID))
            {

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadData(false, storeID, store);
                    PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.InitialConfigurationDashboardID);
                    PluginOperations.ShowStoreTender(new RecordIdentifier(storeID, dlg.PaymentMethodID), store.Text);
                }
            }
        }

        private void btnPaymentTypes_Click(object sender, EventArgs e)
        {
            if (paymentTypeEditor.IsAlive)
            {
                ((IPlugin)paymentTypeEditor.Target).Message(this, "EditPaymentTypes", null);
            }
        }
    }
}
