using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Expressions;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Replenishment.Dialogs;
using LSOne.ViewPlugins.Replenishment.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    public partial class InventoryTemplateFilterPage : UserControl, ITabView
    {
        DataEntitySelectionList retailGroupList;
        DataEntitySelectionList retailDepartmentGroupList;
        DataEntitySelectionList specialGroupList;

        List<InventoryTemplateSectionSelection> selectedRetailDepartments;
        List<InventoryTemplateSectionSelection> selectedRetailGroups;
        List<InventoryTemplateSectionSelection> selectedSpecialGroups;
        InventoryTemplateSectionSelection selectedVendor;

        int inventoryOnHand = int.MinValue;

        string oldLayout;
        bool resetting;
        bool layoutChanged;
        InventoryTemplate template;
        private bool readOnly;

        private IPlugin itemEditor;
        
        public InventoryTemplateFilterPage(TabControl owner, InventoryTemplate template)
            : this()
        {
            resetting = false;
            DoubleBuffered = true;
            this.template = template;
            retailGroupList = null;
            retailDepartmentGroupList = null;
            specialGroupList = null;
            oldLayout = "";
        }

        public InventoryTemplateFilterPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new InventoryTemplateFilterPage((TabControl)sender, (InventoryTemplate)tab.Tag);
        }

        private bool ValuesAreDifferent(List<InventoryTemplateSectionSelection> originals, List<DataEntity> newValues)
        {
            if (originals.Count != newValues.Count)
            {
                return true;
            }

            foreach (var original in originals)
            {
                bool found = false;

                foreach (var newValue in newValues)
                {
                    if (original.ID.PrimaryID == newValue.ID.PrimaryID)
                    {
                        found = true;
                    }
                }

                if (found == false)
                {
                    return true;
                }
            }

            return false;
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            layoutChanged = false;

            if (oldLayout != searchBar1.LayoutString)
            {
                layoutChanged = true;
                return true;
            }

            var searchParams = searchBar1.SearchParameterResults;

            foreach (var param in searchParams)
            {
                switch (param.ParameterKey)
                {
                    case "RetailDepartment":
                        if (selectedRetailDepartments != null)
                        {
                            if (ValuesAreDifferent(selectedRetailDepartments, ((DataEntitySelectionList)(((DualDataComboBox)param.UnknownControl).SelectedData)).GetSelectedItems()))
                            {
                                return true;
                            }
                        }
                        break;

                    case "RetailGroup":
                        if (selectedRetailGroups != null)
                        {
                            if (ValuesAreDifferent(selectedRetailGroups, ((DataEntitySelectionList)(((DualDataComboBox)param.UnknownControl).SelectedData)).GetSelectedItems()))
                            {
                                return true;
                            }
                        }
                        break;

                    case "SpecialGroup":
                        if (selectedSpecialGroups != null)
                        {
                            if (ValuesAreDifferent(selectedSpecialGroups, ((DataEntitySelectionList)(((DualDataComboBox)param.UnknownControl).SelectedData)).GetSelectedItems()))
                            {
                                return true;
                            }
                        }
                        break;

                    case "Vendor":
                        if (selectedVendor != null)
                        {
                            if (selectedVendor.EntityID != ((DualDataComboBox) param.UnknownControl).SelectedData.ID)
                            {
                                return true;
                            }
                        }
                        break;
                    case "InventoryOnHand":
                        if(inventoryOnHand != Convert.ToInt32(param.DoubleValue))
                        {
                            return true;
                        }
                        break;
                }
            }

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //TODO Handle auditing
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            template = (InventoryTemplate)((List<object>)internalContext)[0];
            lvPreview.AutoSizeColumns();
            itemEditor = PluginEntry.Framework.FindImplementor(this, "CanViewRetailItem", null);
            readOnly = (bool)((List<object>)internalContext)[1];
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {

        }

        public bool SaveData()
        {
            var rank = 0;
            try
            {
                if (layoutChanged)
                {
                    var sectionKeys = searchBar1.GetUsedSectionKeys();

                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteInventoryTemplateSection(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID,
                                                    false);

                    foreach (string sectionKey in sectionKeys)
                    {
                        rank++;

                        var section = new InventoryTemplateSection();

                        section.SectionID = sectionKey;
                        section.TemplateID = template.ID;
                        section.SortRank = rank;

                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).InsertInventoryTemplateSection(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), section,
                                                    false);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
            var searchParams = searchBar1.SearchParameterResults;

            foreach (var param in searchParams)
            {
                switch (param.ParameterKey)
                {
                    case "SpecialGroup":
                        SaveComboValues(param, param.ParameterKey,selectedSpecialGroups);
                        break;

                    case "RetailDepartment":
                        SaveComboValues(param, param.ParameterKey,selectedRetailDepartments);
                        break;

                    case "RetailGroup":
                        SaveComboValues(param, param.ParameterKey,selectedRetailGroups);
                        break;

                    case "Vendor":
                        var singleInstanceVendorList = new List<InventoryTemplateSectionSelection>();
                        singleInstanceVendorList.Add(selectedVendor);
                        SaveComboValues(param, param.ParameterKey, singleInstanceVendorList, false);
                        break;

                    case "InventoryOnHand":
                        SaveInventoryOnHandFilter(param);
                        break;
                }
            }

            return true;
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void SaveInventoryOnHandFilter(SearchParameterResult param)
        {
            try
            {
                if(!layoutChanged)
                {
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteInventoryTemplateSectionSelection(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID, param.ParameterKey, false);
                }

                InventoryTemplateSectionSelection sectionSelection = new InventoryTemplateSectionSelection();
                sectionSelection.TemplateID = template.ID;
                sectionSelection.SectionID = param.ParameterKey;
                // We need to substract 2 in order to convert from SearchModificationEnum to DoubleValueOperator enum
                sectionSelection.EntityID = ((int)param.SearchModification - 2).ToString() + "|" + Convert.ToInt32(param.DoubleValue).ToString();

                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).InsertInventoryTemplateSectionSelection(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), sectionSelection, true);
                inventoryOnHand = Convert.ToInt32(param.DoubleValue);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private void SaveComboValues(SearchParameterResult param, RecordIdentifier sectionID, List<InventoryTemplateSectionSelection> selection, bool isSelectionList = true)
        {
            var changed = false;

            if (selection != null)
            {
                if (isSelectionList)
                {
                    if (ValuesAreDifferent(selection, ((DataEntitySelectionList)(((DualDataComboBox)param.UnknownControl).SelectedData)).GetSelectedItems()))
                    {
                        changed = true;
                    }
                }
                else
                {
                    var previousSelectedId = (selection[0] != null) ? selection[0].EntityID : "";
                    if (previousSelectedId != ((DualDataComboBox)param.UnknownControl).SelectedData.ID)
                    {
                        changed = true;
                    }
                }
            }

            if (layoutChanged || changed) // if the layout changed then we got no choice but to write
            {
                try
                {
                    if (!layoutChanged) // if the layout changed then the data has already been deleted
                    {
                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteInventoryTemplateSectionSelection(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID,sectionID,
                                                    false);
                    }

                    List<DataEntity> selectedItems;

                    if (isSelectionList)
                    {
                        selectedItems =
                            ((DataEntitySelectionList) (((DualDataComboBox) param.UnknownControl).SelectedData))
                                .GetSelectedItems();
                    }
                    else
                    {
                        selectedItems = new List<DataEntity>();
                        selectedItems.Add((DataEntity) ((DualDataComboBox) param.UnknownControl).SelectedData);
                    }

                    foreach (var item in selectedItems)
                    {
                        InventoryTemplateSectionSelection sectionSelection = new InventoryTemplateSectionSelection();
                        sectionSelection.TemplateID = template.ID;
                        sectionSelection.SectionID = sectionID;
                        sectionSelection.EntityID = item.ID;

                        Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).InsertInventoryTemplateSectionSelection(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), sectionSelection,
                                                    false);
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    return;
                }
                finally
                {
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
                }
            }
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.RetailGroup, "RetailGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.RetailDepartment, "RetailDepartment", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.SpecialGroup, "SpecialGroup", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Vendor, "Vendor", ConditionType.ConditionTypeEnum.Unknown));

            if (template.TemplateEntryType == TemplateEntryTypeEnum.StockCounting)
            {
                searchBar1.AddCondition(new ConditionType(Resources.InventoryOnHand, "InventoryOnHand", ConditionType.ConditionTypeEnum.Numeric));
            }

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (readOnly)
            {
                searchBar1_SearchClicked(null, EventArgs.Empty);
                searchBar1.Enabled = false;
            }
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            try
            {
                switch (args.TypeKey)
                {
                    case "RetailGroup":
                        var retailGroups = Providers.RetailGroupData.GetList(PluginEntry.DataModel);
                        retailGroupList = new DataEntitySelectionList(retailGroups);

                        selectedRetailGroups = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateSectionSelectionList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID, "RetailGroup",
                                                        true);

                        if (!resetting)
                        {
                            retailGroupList.SelectSome(selectedRetailGroups);
                        }
                        else
                        {
                            retailGroupList.SelectNone();
                        }
                        resetting = false;

                        args.UnknownControl = new DualDataComboBox();
                        args.UnknownControl.Size = new Size(200, 21);
                        args.MaxSize = 200;
                        args.AutoSize = false;

                        ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                        ((DualDataComboBox)args.UnknownControl).SelectedData = retailGroupList;

                        ((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(RetailGroup_DropDown);
                        break;

                    case "RetailDepartment":
                        if (retailDepartmentGroupList == null)
                        {
                            var retailDepartments = Providers.RetailDepartmentData.GetList(PluginEntry.DataModel);
                            retailDepartmentGroupList = new DataEntitySelectionList(retailDepartments);

                            selectedRetailDepartments = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateSectionSelectionList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID, "RetailDepartment",
                                                        true);

                            retailDepartmentGroupList.SelectSome(selectedRetailDepartments);
                        }
                        else
                        {
                            retailDepartmentGroupList.SelectNone();
                        }

                        args.UnknownControl = new DualDataComboBox();
                        args.UnknownControl.Size = new Size(200, 21);
                        args.MaxSize = 200;
                        args.AutoSize = false;
                        ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                        ((DualDataComboBox)args.UnknownControl).SelectedData = retailDepartmentGroupList;

                        ((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(RetailDepartments_DropDown);
                        break;

                    case "SpecialGroup":
                        if (specialGroupList == null)
                        {
                            var specialGroups = Providers.SpecialGroupData.GetList(PluginEntry.DataModel);
                            specialGroupList = new DataEntitySelectionList(specialGroups);

                            selectedSpecialGroups = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateSectionSelectionList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID, "SpecialGroup",
                                                        true);

                            specialGroupList.SelectSome(selectedSpecialGroups);
                        }
                        else
                        {
                            specialGroupList.SelectNone();
                        }

                        args.UnknownControl = new DualDataComboBox();
                        args.UnknownControl.Size = new Size(200, 21);
                        args.MaxSize = 200;
                        args.AutoSize = false;
                        ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                        ((DualDataComboBox)args.UnknownControl).SelectedData = specialGroupList;

                        ((DualDataComboBox)args.UnknownControl).DropDown += new DropDownEventHandler(SpecialGroups_DropDown);
                        break;

                    case "Vendor":
                        var vendorList = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateSectionSelectionList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID, "Vendor",
                                                        false);
                        Vendor selectedVendorEntity = new Vendor();
                        if (vendorList.Count > 0)
                        {
                            selectedVendor = vendorList[0];
                            selectedVendorEntity = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), selectedVendor.EntityID,
                                                        true);
                        }

                        args.UnknownControl = new DualDataComboBox();
                        args.UnknownControl.Size = new Size(200, 21);
                        args.MaxSize = 200;
                        args.AutoSize = false;
                        ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                        ((DualDataComboBox)args.UnknownControl).SelectedData = selectedVendorEntity;

                        ((DualDataComboBox)args.UnknownControl).RequestData += Vendors_RequestData;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
            finally
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
            }
        }

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = true;
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                case "RetailDepartment":
                case "SpecialGroup":
                case "Vendor":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
            }
        }

        private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
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
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "RetailGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= RetailGroup_DropDown;
                    selectedRetailGroups = new List<InventoryTemplateSectionSelection>();
                    break;

                case "RetailDepartment":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= RetailDepartments_DropDown;
                    selectedRetailDepartments = new List<InventoryTemplateSectionSelection>();
                    break;

                case "SpecialGroup":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= SpecialGroups_DropDown;
                    selectedSpecialGroups = new List<InventoryTemplateSectionSelection>();
                    break;

                case "Vendor":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= Vendors_RequestData;
                    selectedVendor = new InventoryTemplateSectionSelection();
                    break;
            }
        }

        void RetailDepartments_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList)((DualDataComboBox)sender).SelectedData);
            }
        }

        void RetailGroup_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList)((DualDataComboBox)sender).SelectedData);
            }
        }

        void SpecialGroups_DropDown(object sender, DropDownEventArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList)((DualDataComboBox)sender).SelectedData);
            }
        }

        private void searchBar1_AdjustSize(object sender, AdjustSizeArguments args)
        {
            lblFilterResults.Top = lblFilterResults.Top + +args.Adjustment;
            lblFirstShown.Top = lblFilterResults.Top;

            groupPanel1.Top = groupPanel1.Top + args.Adjustment;
            groupPanel1.Height = groupPanel1.Height - args.Adjustment;
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            InventoryTemplateFilterContainer filter = new InventoryTemplateFilterContainer();
            List<InventoryTemplateFilterListItem> items = new List<InventoryTemplateFilterListItem>();

            lvPreview.ClearRows();

            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "RetailGroup":
                        filter.RetailGroups.AddRange(((DataEntitySelectionList)((DualDataComboBox)result.UnknownControl).SelectedData)
                                .GetSelectedItems().Select(x => x.ID));
                        break;

                    case "RetailDepartment":
                        filter.RetailDepartments.AddRange(((DataEntitySelectionList)((DualDataComboBox)result.UnknownControl).SelectedData)
                                .GetSelectedItems().Select(x => x.ID));
                        break;

                    case "SpecialGroup":
                        filter.SpecialGroups.AddRange(((DataEntitySelectionList)((DualDataComboBox)result.UnknownControl).SelectedData)
                                .GetSelectedItems().Select(x => x.ID));
                        break;

                    case "Vendor":
                        filter.Vendors.Add(((DataEntity)((DualDataComboBox)result.UnknownControl).SelectedData).ID);
                        break;
                    case "InventoryOnHand":
                        filter.FilterByInventoryOnHand = true;
                        filter.InventoryOnHand = Convert.ToInt32(result.DoubleValue);
                        // We need to substract 2 in order to convert from SearchModificationEnum to DoubleValueOperator enum
                        filter.InventoryOnHandComparison = (DoubleValueOperator)((int)result.SearchModification - 2);
                        break;
                }
            }

            filter.LimitToFirst50Rows = true;

            SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, () =>
            {
                // We try to get one more record then we intend to display, in order to see if there are more records available
                items = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateItems(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true, filter);
            });

            dlg.ShowDialog();

            lblFirstShown.Visible = items.Count >= 50;

            foreach (InventoryTemplateFilterListItem item in items)
            {
                var row = new Row();

                if (itemEditor == null)
                    row.AddText((string)item.ID);
                else
                    row.AddCell(new LinkCell((string)item.ID));

                row.AddText(item.Text);
                row.AddText(item.VariantName);

                if (item.InventoryUnitDescription == "")
                {
                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedError), Resources.NoPurchaseUnitFound, true);
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

                bool isAssemblyItem = Providers.RetailItemData.Get(PluginEntry.DataModel, item.ID).IsAssemblyItem;
                if (item.VendorId == "" && template.TemplateEntryType == TemplateEntryTypeEnum.PurchaseOrder && !isAssemblyItem)
                {
                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedError), Resources.NoVendorFound, true);
                    var cell = new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Left, "", false)
                        {
                            Tag = item.ID
                        };
                    row.AddCell(cell);
                }
                else
                {
                    row.AddText(item.VendorDescription);
                }

                if (isAssemblyItem)
                {
                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedInformation), Resources.AssemblyItemWillBeReplacedWithComponents, true);
                    var cell = new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Left, "", false)
                    {
                        Tag = item.ID
                    };
                    row.AddCell(cell);
                }
                else if (!item.HasSetting && template.TemplateEntryType == TemplateEntryTypeEnum.PurchaseOrder)
                {
                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedError), Resources.NoSettingFound, true);
                    var cell = new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Left, "", false)
                        {
                            Tag = item.ID
                        };
                    row.AddCell(cell);
                }

                row.Tag = item.ID;

                lvPreview.AddRow(row);
            }

            lvPreview.AutoSizeColumns();
        }

        private void searchBar1_ResetSections(object sender, EventArgs e)
        {
            resetting = true;
        }

        private void lvPreview_CellAction(object sender, CellEventArgs args)
        {
            if (args.ColumnNumber == lvPreview.Columns.IndexOf(clmID) && args.Cell is LinkCell)
            {
                itemEditor.Message(this, "ViewItem", new RecordIdentifier((args.Cell as LinkCell).Text));
            }
            else if (args.ColumnNumber == lvPreview.Columns.IndexOf(clmVendor) && args.Cell is IconButtonCell)
            {
                var dialog = new SetVendorDialog((RecordIdentifier)((IconButtonCell)args.Cell).Tag);
                dialog.ShowDialog();
                searchBar1_SearchClicked(sender, args);
            }
            else if (args.ColumnNumber == lvPreview.Columns.IndexOf(clmInventoryUnit) && args.Cell is IconButtonCell)
            {
                var plugin = PluginEntry.Framework.FindImplementor(this, "CanChangeItemsUnits", null);
                if (plugin != null)
                {
                    plugin.Message(this, "ChangeItemsUnits", ((IconButtonCell)args.Cell).Tag);
                    searchBar1_SearchClicked(this, EventArgs.Empty);
                }
            }
        }

        void Vendors_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            try
            {
                VendorSearch search = new VendorSearch {Deleted = false};
                ((DualDataComboBox) sender).SetData(
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetVendors(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), search,true)
                    , null);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            string setupString = "";
            List<InventoryTemplateSection> sections;
            InventoryTemplateSectionSelection inventoryOnHandSelection;
            try
            {
                sections = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateSectionList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID, false);
                List<InventoryTemplateSectionSelection> inventoryOnHandSelectionList =
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateSectionSelectionList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID, "InventoryOnHand",  true);

                inventoryOnHandSelection = inventoryOnHandSelectionList.Count > 0 ? inventoryOnHandSelectionList[0] : null;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }

            for (int i = 0; i < sections.Count; i++)
            {
                setupString += (string)sections[i].SectionID;

                if (i != sections.Count - 1)
                {
                    setupString += ";";
                }
            }

            if (setupString != "")
            {
                // The EntityID for the inventory on hand is composed of an integer representing a DoubleValueOperator enum and the actual value that was set in the filter
                // This is a workaround to avoid complications of adding columns in the database just for a single value
                // If we need multiple values like this in the future, it would be wise to create a new "parameter" column for the filter sections
                if(inventoryOnHandSelection != null)
                {
                    int index = setupString.IndexOf(inventoryOnHandSelection.SectionID.StringValue) + inventoryOnHandSelection.SectionID.StringValue.Length;
                    string[] inventoryOnHandParts = inventoryOnHandSelection.EntityID.StringValue.Split('|');

                    inventoryOnHand = Convert.ToInt32(inventoryOnHandParts[1]);

                    // Here we load the setup string with the populated value, but save the old layout without the value, 
                    // so it matches the layout string of the search bar when checking if data has changed
                    setupString = setupString.Insert(index, "|" + inventoryOnHandParts[0]);
                    index += 2;
                    searchBar1.LoadFromSetupString(setupString.Insert(index, "|" + inventoryOnHandParts[1]));
                }
                else
                {
                    searchBar1.LoadFromSetupString(setupString);
                }

                oldLayout = setupString;
            }
            else
            {
                oldLayout = "";
            }
        }
    }
}