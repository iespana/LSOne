using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
    public partial class CreateStoreTransferFromFilter : UserControl, IWizardPage
    {
        private WizardBase parent;
        private StoreTransferTypeEnum transferType;
        private InventoryTemplateFilterContainer filter;

        public CreateStoreTransferFromFilter(WizardBase parent, StoreTransferTypeEnum transferType)
        {
            InitializeComponent();
            this.parent = parent;
            this.transferType = transferType;
            this.filter = new InventoryTemplateFilterContainer();
        }

        public void Display()
        {
            LoadItems();
            CheckState();
        }

        private void LoadItems()
        {
            filter = new InventoryTemplateFilterContainer();
            filter.LimitToFirst50Rows = true;
            List<InventoryTemplateFilterListItem> items;

            lvItems.ClearRows();

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "RetailGroup":
                        filter.RetailGroups.AddRange(((DataEntitySelectionList)((DualDataComboBox)result.UnknownControl).SelectedData)
                                .GetSelectedItems().Select(x => x.ID));
                        break;

                    case "RetailDepartment":
                        filter.RetailDepartments.AddRange(((DataEntitySelectionList) ((DualDataComboBox) result.UnknownControl).SelectedData)
                                .GetSelectedItems().Select(x => x.ID));
                        break;

                    case "SpecialGroup":
                        filter.SpecialGroups.AddRange(((DataEntitySelectionList) ((DualDataComboBox) result.UnknownControl).SelectedData)
                                .GetSelectedItems().Select(x => x.ID));
                        break;

                    case "Vendor":
                        filter.Vendors.AddRange(((DataEntitySelectionList)((DualDataComboBox)result.UnknownControl).SelectedData)
                                .GetSelectedItems().Select(x => x.ID));
                        break;
                }
            }

            items = new List<InventoryTemplateFilterListItem>();

            IInventoryService service = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);
            
            try
            {
                SpinnerDialog dlg = new SpinnerDialog(Properties.Resources.FewMinutesMessage, () =>
                {
                    // We try to get one more record then we intend to display, in order to see if there are more records available
                    items = service.GetInventoryTemplateItems(PluginEntry.DataModel,
                        PluginOperations.GetSiteServiceProfile(), true, filter);
                });

                dlg.ShowDialog();
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            lblFirstShown.Visible = items.Count >= 50;

            foreach (InventoryTemplateFilterListItem item in items)
            {
                var row = new Row();

                row.AddText((string) item.ID);
                row.AddText(item.Text);
                row.AddText(item.VariantName);

                if (item.InventoryUnitDescription == "")
                {
                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedError),Resources.NoPurchaseUnitFound, true);
                    var cell = new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Left, "", false)
                    {
                        Tag = Providers.RetailItemData.Get(PluginEntry.DataModel, item.ID)
                    };
                    row.AddCell(cell);
                }
                else
                {
                    row.AddText(item.InventoryUnitDescription);
                }

                row.AddText(item.RetailGroupName);
                row.AddText(item.RetailDepartmentName);
                row.AddText(item.VendorDescription);
                row.Tag = item.ID;

                lvItems.AddRow(row);
            }

            lvItems.AutoSizeColumns();
            CheckState();
        }

        private void CheckState()
        {
            parent.NextEnabled = lvItems.RowCount > 0;
        }

        public IWizardPage RequestNextPage()
        {
            return new NewStoreTransfer(parent, filter, transferType);
        }

        public void ResetControls()
        {

        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            return true;
        }

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

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.RetailGroup, "RetailGroup",
                ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.RetailDepartment, "RetailDepartment",
                ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.SpecialGroup, "SpecialGroup",
                ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Vendor, "Vendor", ConditionType.ConditionTypeEnum.Unknown));
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            try
            {
                switch (args.TypeKey)
                {
                    case "RetailGroup":
                        var retailGroups = Providers.RetailGroupData.GetList(PluginEntry.DataModel);
                        args.UnknownControl = new DualDataComboBox();
                        args.UnknownControl.Size = new Size(200, 21);
                        args.MaxSize = 200;
                        args.AutoSize = false;
                        ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                        ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntitySelectionList(retailGroups);
                        ((DualDataComboBox) args.UnknownControl).DropDown += RetailGroup_DropDown;
                        break;

                    case "RetailDepartment":
                        var retailDepartments = Providers.RetailDepartmentData.GetList(PluginEntry.DataModel);
                        args.UnknownControl = new DualDataComboBox();
                        args.UnknownControl.Size = new Size(200, 21);
                        args.MaxSize = 200;
                        args.AutoSize = false;
                        ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                        ((DualDataComboBox) args.UnknownControl).SelectedData =
                            new DataEntitySelectionList(retailDepartments);
                        ((DualDataComboBox) args.UnknownControl).DropDown += RetailDepartments_DropDown;
                        break;

                    case "SpecialGroup":
                        var specialGroups = Providers.SpecialGroupData.GetList(PluginEntry.DataModel);
                        args.UnknownControl = new DualDataComboBox();
                        args.UnknownControl.Size = new Size(200, 21);
                        args.MaxSize = 200;
                        args.AutoSize = false;
                        ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                        ((DualDataComboBox) args.UnknownControl).SelectedData =
                            new DataEntitySelectionList(specialGroups);
                        ((DualDataComboBox) args.UnknownControl).DropDown += SpecialGroups_DropDown;
                        break;

                    case "Vendor":
                    IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
                    List<DataEntity> vendorList = service.GetVendorList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true);
                        args.UnknownControl = new DualDataComboBox();
                        args.UnknownControl.Size = new Size(200, 21);
                        args.MaxSize = 200;
                        args.AutoSize = false;
                        ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;
                        ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntitySelectionList(vendorList);
                        ((DualDataComboBox) args.UnknownControl).DropDown += Vendors_DropDown;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

        }

        private void searchBar_UnknownControlGetSelection(object sender, Controls.UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                case "RetailDepartment":
                case "SpecialGroup":
                case "Vendor":
                    args.Selection = (string) ((DualDataComboBox) args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = true;
        }

        private void searchBar_UnknownControlRemove(object sender, Controls.UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                    ((DualDataComboBox) args.UnknownControl).DropDown -= RetailGroup_DropDown;
                    break;

                case "RetailDepartment":
                    ((DualDataComboBox) args.UnknownControl).DropDown -= RetailDepartments_DropDown;
                    break;

                case "SpecialGroup":
                    ((DualDataComboBox) args.UnknownControl).DropDown -= SpecialGroups_DropDown;
                    break;

                case "Vendor":
                    ((DualDataComboBox) args.UnknownControl).RequestData -= Vendors_RequestData;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, Controls.UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;

            try
            {
                switch (args.TypeKey)
                {
                    case "RetailGroup":
                        entity = Providers.RetailGroupData.Get(PluginEntry.DataModel, args.Selection);
                        break;
                    case "RetailDepartment":
                        entity = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, args.Selection);
                        break;
                    case "SpecialGroup":
                        entity = Providers.SpecialGroupData.Get(PluginEntry.DataModel, args.Selection);
                        break;
                    case "Vendor":
                        entity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).
                            GetVendor(
                                PluginEntry.DataModel,
                                PluginOperations.GetSiteServiceProfile(),
                                args.Selection,
                                true);
                        Providers.VendorData.Get(PluginEntry.DataModel, args.Selection);
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }


            ((DualDataComboBox) args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }


        private void RetailGroup_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox) sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList) ((DualDataComboBox) sender).SelectedData);
            }
        }

        private void RetailDepartments_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox) sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList) ((DualDataComboBox) sender).SelectedData);
            }
        }

        private void SpecialGroups_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox) sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList) ((DualDataComboBox) sender).SelectedData);
            }
        }

        private void Vendors_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox) sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList) ((DualDataComboBox) sender).SelectedData);
            }
        }

        private void Vendors_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox) sender).SkipIDColumn = true;

            ((DualDataComboBox) sender).SetData(Providers.VendorData.GetList(PluginEntry.DataModel), null);
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void searchBar_AdjustSize(object sender, AdjustSizeArguments args)
        {
            lblFilterResults.Top = lblFilterResults.Top + +args.Adjustment;
            lblFirstShown.Top = lblFilterResults.Top;

            lvItems.Top = lvItems.Top + args.Adjustment;
            lvItems.Height = lvItems.Height - args.Adjustment;
        }
    }
}
