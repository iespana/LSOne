using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityRestaurantMenuTypesPage : UserControl, ITabView
    {

        private Store store;

        public HospitalityRestaurantMenuTypesPage()
        {
            InitializeComponent();

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageRestaurantMenuTypes);

            lvRestaurantMenuTypes.SmallImageList = PluginEntry.Framework.GetImageList();
            lvRestaurantMenuTypes.ContextMenuStrip = new ContextMenuStrip();
            lvRestaurantMenuTypes.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityRestaurantMenuTypesPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            store = (Store)internalContext;

            LoadItems();

        }

        public bool DataIsModified()
        {
            //throw new NotImplementedException();
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
                case "RestaurantMenuType":
                    LoadItems();
                    break;
            }
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvRestaurantMenuTypes.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        /// <summary>
        /// Loads the restaurant menu types
        /// </summary>
        private void LoadItems()
        {
            List<RestaurantMenuType> restaurantMenuTypes;
            ListViewItem listItem;

            lvRestaurantMenuTypes.Items.Clear();

            // Get all sales types
            restaurantMenuTypes = Providers.RestaurantMenuTypeData.GetList(PluginEntry.DataModel, store.ID);

            foreach (var restaurantMenuType in restaurantMenuTypes)
            {
                listItem = new ListViewItem(restaurantMenuType.MenuOrder.ToString());
                listItem.SubItems.Add(restaurantMenuType.Text);
                listItem.SubItems.Add(restaurantMenuType.CodeOnPos);
                listItem.ImageIndex = -1;

                listItem.Tag = restaurantMenuType.ID;

                lvRestaurantMenuTypes.Add(listItem);
            }

            lvRestaurantMenuTypes_SelectedIndexChanged(this, EventArgs.Empty);

            lvRestaurantMenuTypes.BestFitColumns();
        }

        private void lvRestaurantMenuTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = lvRestaurantMenuTypes.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageRestaurantMenuTypes);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvRestaurantMenuTypes.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("RestaurantMenuTypeList", lvRestaurantMenuTypes.ContextMenuStrip, lvRestaurantMenuTypes);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewRestaurantMenuType(store.ID);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowRestaurantMenuType((RecordIdentifier)lvRestaurantMenuTypes.SelectedItems[0].Tag);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteRestaurantMenuType((RecordIdentifier)lvRestaurantMenuTypes.SelectedItems[0].Tag);
        }

        private void lvRestaurantMenuTypes_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
