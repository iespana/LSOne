using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Hospitality.ListItems;
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
    public partial class HospitalityRestaurantHospitalityTypesPage : UserControl, ITabView
    {

        /*
         * This tab is intended to show the Hospitality Types created for this restaurant. The data on this
         * tab does not affect the Store record itself, it is in effect a small version of the 
         * hospitality types list view inside the store view.
         */

        private Store store;

        public HospitalityRestaurantHospitalityTypesPage()
        {
            InitializeComponent();

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes);

            lvHospitalityTypes.SmallImageList = PluginEntry.Framework.GetImageList();
            lvHospitalityTypes.ContextMenuStrip = new ContextMenuStrip();
            lvHospitalityTypes.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityRestaurantHospitalityTypesPage();
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
            //throw new NotImplementedException();
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //throw new NotImplementedException();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "HospitalityType":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        //selectedID = changeIdentifier;
                    }

                    LoadItems();
                    break;
            }
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvHospitalityTypes.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        /// <summary>
        /// Loads the hospitality type list items to the list view
        /// </summary>
        private void LoadItems()
        {
            List<HospitalityTypeListItem> hosptitalityTypeListItems;
            ListViewItem listItem;

            lvHospitalityTypes.Items.Clear();

            // Get all sales types
            hosptitalityTypeListItems = Providers.HospitalityTypeData.GetHostpitalityTypesForRestaurant(PluginEntry.DataModel, store.ID);

            foreach (var hospitalityType in hosptitalityTypeListItems)
            {
                listItem = new ListViewItem((string)hospitalityType.RestaurantID);
                listItem.SubItems.Add(hospitalityType.SalesTypeDescription);
                listItem.SubItems.Add(hospitalityType.Sequence.ToString());
                listItem.SubItems.Add(hospitalityType.Text);
                listItem.ImageIndex = -1;

                listItem.Tag = hospitalityType.ID;

                lvHospitalityTypes.Add(listItem);

                //if (selectedID == (RecordIdentifier)listItem.Tag)
                //{
                //    listItem.Selected = true;
                //}
            }

            //lvHospitalityTypes.Columns[lvHospitalityTypes.SortColumn].ImageIndex = (lvHospitalityTypes.SortedBackwards ? 1 : 0);

            lvHospitalityTypes_SelectedIndexChanged(this, EventArgs.Empty);

            lvHospitalityTypes.BestFitColumns();
        }

        private void lvHospitalityTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = btnsEditAddRemove.RemoveButtonEnabled = lvHospitalityTypes.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvHospitalityTypes.ContextMenuStrip;

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

            PluginEntry.Framework.ContextMenuNotify("SalesTypesList", lvHospitalityTypes.ContextMenuStrip, lvHospitalityTypes);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewHospitalityType(store);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowHospitalityType((RecordIdentifier)lvHospitalityTypes.SelectedItems[0].Tag);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteHospitalityType((RecordIdentifier)lvHospitalityTypes.SelectedItems[0].Tag);
        }

        private void lvHospitalityTypes_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
