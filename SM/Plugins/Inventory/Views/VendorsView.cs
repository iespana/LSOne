using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class VendorsView : ViewBase
    {
        private static Guid BarSettingID = new Guid("6C02CDD0-9C3F-4DFC-8885-590627AE1B1D");

        private RecordIdentifier selectedID = "";
        private List<Vendor> vendors;
        private Setting searchBarSetting;
        private VendorSearch searchCriteria;
        private bool lockEvents = true;

        public VendorsView(RecordIdentifier vendorId)
            : base()
        {
            selectedID = vendorId;
        }

        public VendorsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.Vendors;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.VendorEdit);

            searchBar1.BuddyControl = lvVendors;

            lvVendors.ContextMenuStrip = new ContextMenuStrip();
            lvVendors.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.VendorEdit);

            lockEvents = false;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Vendors", 0, Properties.Resources.Vendors, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Vendors;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            lockEvents = true;
            LoadItems();
            lockEvents = false;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Vendor":
                    if (changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Add || changeHint == DataEntityChangeType.MultiAdd)
                    {
                        if (changeHint != DataEntityChangeType.MultiAdd)
                        {
                            selectedID = changeIdentifier;
                        }
                        LoadItems();
                    }
                    else if (changeHint == DataEntityChangeType.Delete || changeHint == DataEntityChangeType.MultiDelete)
                    {
                        if (changeHint != DataEntityChangeType.MultiDelete)
                        {
                            selectedID = changeIdentifier;
                        }
                        LoadItems();
                        RefreshContextBar();
                    }
                    break;
            }

        }

        private VendorSearch GetSearchBarResults()
        {
            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            searchCriteria = new VendorSearch();

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Name":
                        searchCriteria.Description = result.StringValue.Tokenize();
                        searchCriteria.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Deleted":
                        if (result.CheckedValues[0])
                        {
                            searchCriteria.Deleted = true;
                        }
                        break;
                }
            }

            return searchCriteria;
        }

        private void LoadItems(VendorSearch searchCriteria = null)
        {
            lvVendors.ClearRows();

            try
            {
                if (searchCriteria == null)
                {
                    searchCriteria = GetSearchBarResults();
                }

                var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                vendors = service.GetVendors(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), searchCriteria, true);

                Row row;
                foreach (Vendor vendor in vendors)
                {
                    row = new Row();
                    row.AddText((string)vendor.ID);
                    row.AddText(vendor.Text);
                    row.AddText(vendor.Phone);
                    row.AddText(vendor.GetFormattedAddress(PluginEntry.DataModel));
                    row.Tag = vendor;

                    lvVendors.AddRow(row);

                    if (vendor.ID == selectedID)
                    {
                        lvVendors.Selection.AddRows(lvVendors.RowCount - 1, lvVendors.RowCount - 1);
                    }
                }

                lvVendors.Sort(lvVendors.SortColumn, lvVendors.SortedAscending);
                lvVendors.AutoSizeColumns();
                lvVendors_SelectedIndexChanged(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void lvVendors_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = lvVendors.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.VendorEdit);
            btnsEditAddRemove.RemoveButtonEnabled = lvVendors.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.VendorEdit);
            RefreshContextBar();
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            Vendor vendor = (Vendor)lvVendors.Rows[lvVendors.Selection.FirstSelectedRow].Tag;
            PluginOperations.ShowVendorView(vendor.ID, vendors);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewVendor(this, EventArgs.Empty);
        }

        private List<Vendor> GetSelectedVendors()
        {
            if (lvVendors.Selection.Count == 0)
            {
                return new List<Vendor>();
            }

            List<Vendor> selection = new List<Vendor>();
            for (int i = 0; i < lvVendors.Selection.Count; i++)
            {
                Vendor line = (Vendor)lvVendors.Selection[i].Tag;
                selection.Add(line);
            }

            return selection;
        }

        private List<RecordIdentifier> GetSelectedIDs()
        {
            List<Vendor> vendors = GetSelectedVendors();
            return vendors.Select(vendor => vendor.ID).ToList();
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteVendors(GetSelectedVendors());
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvVendors.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                //Image = Properties.Resources.EditImage,
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

            PluginEntry.Framework.ContextMenuNotify("VendorList", lvVendors.ContextMenuStrip, lvVendors);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvVendors_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.VendorName, "Name", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_Deleted, "Deleted", ConditionType.ConditionTypeEnum.Checkboxes, Resources.SearchBar_Deleted_Yes, false));

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar1.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }

            ShowTimedProgress(searchBar1.GetLocalizedSavingText());
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {

            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.VendorEdit))
                {
                    //Get all the vendors that are selected and if at least one is deleted then display the link as enabled
                    List<Vendor> selectedVendors = GetSelectedVendors();
                    bool restoreLinkEnabled = selectedVendors.Any(a => a.Deleted);

                    arguments.Add(new ContextBarItem(Properties.Resources.Related_RestoreVendor, "RestoreVendor", restoreLinkEnabled, RestoreVendor), 5000000);
                }
            }
        }

        private void RestoreVendor(object sender, EventArgs e)
        {
            if (lvVendors.Selection.Count == 0)
            {
                return;
            }


            List<Vendor> selectedVendors = GetSelectedVendors();
            if (selectedVendors.Any(a => a.Deleted))
            {
                PluginOperations.RestoreVendors(GetSelectedIDs());
                RefreshContextBar();
            }
        }

        private void RefreshContextBar()
        {
            if (!lockEvents)
            {
                if (PluginEntry.DataModel.HasPermission(Permission.VendorEdit))
                {
                    Vendor selectedVendor = lvVendors.Selection.Count == 0 ? null : (Vendor)lvVendors.Rows[lvVendors.Selection.FirstSelectedRow].Tag;
                    bool restoreLinkEnabled = lvVendors.Selection.Count == 1 && selectedVendor != null && selectedVendor.Deleted;

                    SetContextBarItemEnabled(this.GetType().ToString() + ".View", "RestoreVendor", restoreLinkEnabled);
                }

                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }

        }
    }
}
