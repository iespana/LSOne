using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.VariantFramework.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using TabControl = LSOne.ViewCore.Controls.TabControl;


namespace LSOne.ViewPlugins.VariantFramework.DialogPages
{
    public partial class NewRetailItemVariantsPage : UserControl, IDialogTabViewWithRequiredFields, IMessageTabExtension
    {
        private bool changingAllCells;
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("9FCBD3EF-CE79-481E-AA45-4135AF772518");
        public event EventHandler RequiredInputValidate;
        private RetailItem retailItem;
        private List<Row> displayedRows;  


        public NewRetailItemVariantsPage()
        {
            InitializeComponent();

            lvVariantItems.ContextMenuStrip = new ContextMenuStrip();
            lvVariantItems.ContextMenuStrip.Opening += VariantsContextMenuStrip_Opening;

            searchBar.BuddyControl = lvVariantItems;
            searchBar.SearchButtonTextOverride = Resources.Filter;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new NewRetailItemVariantsPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            retailItem = (RetailItem)internalContext;
        }

        private void OnTabBecomesVisible()
        {

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            OnTabBecomesVisible();
            
            searchBar.FocusFirstInput();

            SpinnerDialog dlg = new SpinnerDialog("", () => LoadListView(true));
            dlg.ShowDialog();
            btnSelectAll.Enabled = lvVariantItems.RowCount > 0;
            btnSelectNone.Enabled = lvVariantItems.RowCount > 0;
            lvVariantItems.AutoSizeColumns();
        }

        private void LoadListView(bool loadingFromThread)
        {
            displayedRows = new List<Row>();
            lvVariantItems.Rows.Clear();
            lvVariantItems.Columns.Clear();
            bool handled;

            List<Row> dimensionAttributesList = (List<Row>)((TabControl)this.Parent).SendViewPageMessage(this, "GetDimensions", null, out handled);

            if (!handled || dimensionAttributesList == null || (dimensionAttributesList != null && dimensionAttributesList.Count == 0))
            {
                return;
            }

            Column selectedColumn = new Column();
            selectedColumn.HeaderText = Resources.Selected;
            selectedColumn.AutoSize = true;
            selectedColumn.Clickable = true;
            selectedColumn.InternalSort = true;
            lvVariantItems.Columns.Add(selectedColumn);

            Column column;
            foreach (var dimension in dimensionAttributesList)
            {
                column = new Column();
                column.HeaderText = dimension[0].Text;
                column.AutoSize = true;
                column.Clickable = true;
                column.InternalSort = true;
                lvVariantItems.Columns.Add(column);
            }

            List<VariantItems> variantItemsList = CreateVariantItemsList(dimensionAttributesList);

            Row row;
            Controls.Cells.CheckBoxCell cell;

            foreach (var variantItem in variantItemsList)
            {
                cell = new Controls.Cells.CheckBoxCell();
                cell.CheckBoxAlignment = LSOne.Controls.Cells.CheckBoxCell.CheckBoxAlignmentEnum.Center;
                row = new Row();
                row.AddCell(cell);

                foreach (var item in variantItem.attributes)
                {
                    row.AddText(item.Text);
                }
                row.Tag = variantItem;
                displayedRows.Add(row);
            }
            Filter(loadingFromThread);
        }

        private void Filter(bool loadingFromThread)
        {
            lvVariantItems.Rows.Clear();
            bool addItem;
            bool textCondition;
            List<SearchParameterResult> results = searchBar.SearchParameterResults;
            foreach (var row in displayedRows)
            {
                addItem = true;
                // Figure out what to display based on search condtions
                foreach (var result in results)
                {
                    switch (result.ParameterKey)
                    {
                        case "Description":
                            if (result.StringValue != "")
                            {
                                textCondition = false;
                                for (uint i = 1; i < lvVariantItems.Columns.Count; i++)
                                {
                                    if (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith)
                                    {
                                        if ((string.Compare(row[i].Text.Left(result.StringValue.Length), result.StringValue, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase)) == 0)
                                        {
                                            textCondition = true;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if(CultureInfo.CurrentCulture.CompareInfo.IndexOf(row[i].Text, result.StringValue, CompareOptions.IgnoreCase) >= 0)
                                        {
                                            textCondition = true;
                                            break;
                                        }
                                    }
                                }
                                if (!textCondition)
                                {
                                    addItem = false;
                                }
                            }
                            break;

                        case "Selected":
                            if (((CheckBoxCell) row[0]).Checked != result.CheckedValues[0])
                            {
                                addItem = false;
                            }
                            break;
                    }
                }

                if (addItem)
                {
                    lvVariantItems.AddRow(row);
                }
            }

            lvVariantItems.AutoSizeColumns();

            if (!loadingFromThread)
            {
                btnSelectAll.Enabled = lvVariantItems.RowCount > 0;
                btnSelectNone.Enabled = lvVariantItems.RowCount > 0;
            }
        }

        private List<VariantItems> CreateVariantItemsList(List<Row> variantItems)
        {
            List<List<DimensionAttribute>> attributeLists = new List<List<DimensionAttribute>>();
            
            foreach (var variantItem in variantItems)
            {
                List<DimensionAttribute> attributes = new List<DimensionAttribute>();
                attributes = ((List<DimensionAttribute>) ((object[])variantItem.Tag)[1]);
                attributeLists.Add(attributes);
            }
            List<VariantItems> variantItemList = createVariantItems(attributeLists);
            return variantItemList;
        }

        class VariantItems
        {
            public VariantItems()
            {
                attributes = new List<DimensionAttribute>();
            }
            internal List<DimensionAttribute> attributes { get; }
        }

        private List<VariantItems> processVariants(List<DimensionAttribute> attributes, List<VariantItems> processedVariants)
        {
            List<VariantItems> variants = new List<VariantItems>();
            if (processedVariants.Count == 0)
            {
                foreach (var dimensionAttribute in attributes)
                {
                    VariantItems variantItems = new VariantItems();
                    variantItems.attributes.Add(dimensionAttribute);
                    variants.Add(variantItems);
                }
            }
            else
            {
                foreach (var dimensionAttribute in attributes)
                {
                    foreach (var processedVariant in processedVariants)
                    {
                        VariantItems variantItems = new VariantItems();
                        variantItems.attributes.Add(dimensionAttribute);
                        variantItems.attributes.AddRange(processedVariant.attributes);
                        variants.Add(variantItems);
                    }
                }
            }
            return variants;
        }

        private List<VariantItems> createVariantItems(List<List<DimensionAttribute>> attributeLists)
        {
            List<VariantItems> variants = new List<VariantItems>();

            for (int i = attributeLists.Count - 1; i >= 0; i--)
            {
                variants = processVariants(attributeLists[i], variants);
            }
            return variants;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible)
            {
                OnTabBecomesVisible();
            }
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;

        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (RequiredInputValidate != null)
            {
                RequiredInputValidate(this, EventArgs.Empty);
            }
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        public void SaveSecondaryRecords()
        {
            RetailItem variantItem;
            string variantDescription;
            foreach (var row in displayedRows)
            {
                if (((CheckBoxCell)row[0]).Checked)
                {
                    variantDescription = "";
                    variantItem = new RetailItem();
                    variantItem.Text = retailItem.Text;
                    variantItem.MasterID = Guid.NewGuid();
                    variantItem.HeaderItemID = retailItem.MasterID;

                    foreach (var attribute in ((VariantItems)row.Tag).attributes)
                    {
                        if (variantDescription != "")
                        {
                            variantDescription += " ";
                        }
                        variantDescription += attribute.Text;

                        Providers.RetailItemData.AddDimensionAttribute(PluginEntry.DataModel, variantItem.MasterID, attribute.ID);
                    }

                    variantItem.VariantName = variantDescription.Length > 60 ? variantDescription.Left(60) : variantDescription;
                    variantItem.ExtendedDescription = retailItem.ExtendedDescription;
                    variantItem.ItemType = ItemTypeEnum.Item;
                    variantItem.ExtendedDescription = retailItem.ExtendedDescription;
                    variantItem.RetailGroupMasterID = retailItem.RetailGroupMasterID;
                    variantItem.RetailDepartmentMasterID = retailItem.RetailDepartmentMasterID;
                    variantItem.RetailDivisionMasterID = retailItem.RetailDivisionMasterID;
                    variantItem.SalesTaxItemGroupID = retailItem.SalesTaxItemGroupID;
                    variantItem.InventoryUnitID = retailItem.InventoryUnitID;
                    variantItem.PurchaseUnitID = retailItem.InventoryUnitID;
                    variantItem.SalesUnitID = retailItem.SalesUnitID;

                    variantItem.SalesPrice = retailItem.SalesPrice;
                    variantItem.SalesPriceIncludingTax = retailItem.SalesPriceIncludingTax;

                    Providers.RetailItemData.Save(PluginEntry.DataModel, variantItem);
                }
            }
        }

        public void RequiredFieldsAreValid(FieldValidationArguments args)
        {
        }

        private bool ValidateVariantsPage()
        {
            foreach (Row row in displayedRows)
            {
                if (((CheckBoxCell)row[0]).Checked)
                {
                    return true;
                }
            }
            return false;
        }

        void IDialogTabViewWithRequiredFields.RequiredFieldsAreValid(FieldValidationArguments args)
        {
            bool variantsSelected = ValidateVariantsPage();
            if ((variantsSelected && retailItem.ItemType == ItemTypeEnum.MasterItem) || lvVariantItems.RowCount == 0)
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.Valid;
            }
            else if(variantsSelected && retailItem.ItemType != ItemTypeEnum.MasterItem)
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
                args.ResultDescription = Resources.NoProperItemType;
            }
            else
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
                args.ResultDescription = Resources.NoVariantItemsSelected;
            }
        }

        private void VariantsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvVariantItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Select,
                    50,
                    select_Click)
            {
                Enabled = lvVariantItems.RowCount > 0
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Deselect,
                    60,
                    deselect_Click)
            {
                Enabled = lvVariantItems.RowCount > 0
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.SelectAll,
                    100,
                    btnSelectAll_Click)
            {
                Enabled = lvVariantItems.RowCount > 0
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.SelectNone,
                    200,
                    btnSelectNone_Click)
            {
                Enabled = lvVariantItems.RowCount > 0
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("NewRetailItemVariantsList", lvVariantItems.ContextMenuStrip, this);

            e.Cancel = (menu.Items.Count == 0);
        }

        public object OnViewPageMessage(object sender, string message, object param, ref bool handled)
        {
            switch (message)
            {
                case "DimensionsChanged":
                    SpinnerDialog dlg = new SpinnerDialog("", () => LoadListView(true));
                    dlg.ShowDialog();
                    btnSelectAll.Enabled = lvVariantItems.RowCount > 0;
                    btnSelectNone.Enabled = lvVariantItems.RowCount > 0;
                    return null;
                case "CreateAnother":
                    lvVariantItems.ClearRows();
                    lvVariantItems.Columns.Clear();
                    displayedRows = new List<Row>();
                    searchBar_LoadDefault(sender, EventArgs.Empty);
                    return null;
                case "HasVariantsPage":
                    handled = true;
                    return null;



            }
            return null;
        }

        private void lvVariantItems_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            if (changingAllCells)
            {
                return;
            }
            int checkedCounter = 0;
            bool handled;
            if (((CheckBoxCell)args.Cell).Checked)
            {
                ((TabControl) this.Parent).SendViewPageMessage(this, "RetailItemIsMasterItem", null, out handled);
            }
            else
            {
                foreach (var row in lvVariantItems.Rows)
                {
                    if (((CheckBoxCell)row[0]).Checked)
                    {
                        checkedCounter ++;
                    }
                }
                if (checkedCounter == 0)
                {
                    ((TabControl)this.Parent).SendViewPageMessage(this, "RetailItemIsItem", null, out handled);
                }
            }
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }

            //ShowTimedProgress(searchBar.GetLocalizedSavingText());
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            Filter(false);
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Selected, "Selected", ConditionType.ConditionTypeEnum.Checkboxes));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void select_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvVariantItems.Selection.Count; i++)
            {
                Row row = lvVariantItems.Selection[i];
                ((CheckBoxCell)row[0]).Checked = true;
            }
            lvVariantItems.InvalidateContent();
        }

        private void deselect_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lvVariantItems.Selection.Count; i++)
            {
                Row row = lvVariantItems.Selection[i];
                ((CheckBoxCell)row[0]).Checked = false;
            }
            lvVariantItems.InvalidateContent();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            bool handled;
            changingAllCells = true;
            foreach (Row row in lvVariantItems.Rows)
            {
                ((CheckBoxCell)row[0]).Checked = true;
            }
            ((TabControl)this.Parent).SendViewPageMessage(this, "RetailItemIsMasterItem", null, out handled);
            changingAllCells = false;

            lvVariantItems.InvalidateContent();
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            bool handled;
            changingAllCells = true;
            foreach (Row row in lvVariantItems.Rows)
            {
                ((CheckBoxCell)row[0]).Checked = false;
            }
            ((TabControl)this.Parent).SendViewPageMessage(this, "RetailItemIsItem", null, out handled);

            foreach (Row displayedRow in displayedRows)
            {
                if (((CheckBoxCell)displayedRow[0]).Checked)
                {
                    ((TabControl)this.Parent).SendViewPageMessage(this, "RetailItemIsMasterItem", null, out handled);
                    break;
                }
            }
            changingAllCells = false;

            lvVariantItems.InvalidateContent();
        }
    }
}
