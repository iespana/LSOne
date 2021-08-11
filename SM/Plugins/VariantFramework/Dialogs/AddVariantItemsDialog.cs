using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.VariantFramework.Properties;

namespace LSOne.ViewPlugins.VariantFramework.Dialogs
{
    public partial class AddVariantItemsDialog : DialogBase
    {
        private bool changingAllCells;
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("4A021E8F-7445-4866-837B-C68F912A8636");
        private RetailItem item;
        private List<Row> displayedRows;
        private List<RetailItemDimension> itemDimensions;
        private Dictionary<RecordIdentifier,List<DimensionAttribute>> dimensionAttributes;
        private Dictionary<RecordIdentifier, List<DimensionAttribute>> existingVariantItems;
        private Dictionary<string,bool> dimensionAttributesCombinationsCompare;

        public AddVariantItemsDialog(RetailItem masterItem)
        {
            InitializeComponent();

            lvVariantItems.ContextMenuStrip = new ContextMenuStrip();
            lvVariantItems.ContextMenuStrip.Opening += VariantsContextMenuStrip_Opening;

            searchBar.BuddyControl = lvVariantItems;
            searchBar.SearchButtonTextOverride = Resources.Filter;

            item = masterItem;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            searchBar.FocusFirstInput();

            SpinnerDialog dlg = new SpinnerDialog("", () => LoadListView(true));
            dlg.ShowDialog();
            btnSelectAll.Enabled = lvVariantItems.RowCount > 0;
            btnSelectNone.Enabled = lvVariantItems.RowCount > 0;
            lvVariantItems.AutoSizeColumns();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void LoadListView(bool loadingFromThread)
        {
            displayedRows = new List<Row>();
            dimensionAttributesCombinationsCompare = new Dictionary<string, bool>();
            itemDimensions = Providers.RetailItemDimensionData.GetListForRetailItem(PluginEntry.DataModel, item.MasterID);
            dimensionAttributes = Providers.DimensionAttributeData.GetHeaderItemDimensionsAndAttributes(PluginEntry.DataModel, item.MasterID);
            existingVariantItems = Providers.DimensionAttributeData.GetRetailItemDimensionAttributeRelations(PluginEntry.DataModel, item.MasterID);

            Column selectedColumn = new Column();
            selectedColumn.HeaderText = Resources.Selected;
            selectedColumn.AutoSize = true;
            selectedColumn.Clickable = true;
            selectedColumn.InternalSort = true;
            lvVariantItems.Columns.Add(selectedColumn);


            Column column;
            foreach (RetailItemDimension itemDimension in itemDimensions)
            {
                column = new Column();
                column.HeaderText = itemDimension.Text;
                column.AutoSize = true;
                column.Clickable = true;
                column.InternalSort = true;
                lvVariantItems.Columns.Add(column);
            }

            List<VariantItems> allVariantItems = CreateVariantItemsList(dimensionAttributes);
            List<VariantItems> remainingVariantItems = new List<VariantItems>();

            string key = "";
            foreach (KeyValuePair<RecordIdentifier, List<DimensionAttribute>> variantItem in existingVariantItems)
            {
                foreach (var attribute in variantItem.Value)
                {
                    key += attribute.ID;
                }
                dimensionAttributesCombinationsCompare.Add(key, true);
                key = "";
            }

            foreach (VariantItems variantItem in allVariantItems)
            {
                foreach (DimensionAttribute attribute in variantItem.attributes)
                {
                    key += attribute.ID;
                }

                if (!dimensionAttributesCombinationsCompare.ContainsKey(key))
                {
                    remainingVariantItems.Add(variantItem);
                }
                key = "";
            }

            Row row;
            Controls.Cells.CheckBoxCell cell;

            foreach (var variantItem in remainingVariantItems)
            {
                cell = new Controls.Cells.CheckBoxCell();
                cell.CheckBoxAlignment = LSOne.Controls.Cells.CheckBoxCell.CheckBoxAlignmentEnum.Center;
                row = new Row();
                row.AddCell(cell);

                foreach (var attribute in variantItem.attributes)
                {
                    row.AddText(attribute.Text);
                }
                row.Tag = variantItem;
                displayedRows.Add(row);
            }
            Filter(loadingFromThread);

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

        }
        
        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            Filter(false);
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
                                        if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(row[i].Text, result.StringValue, CompareOptions.IgnoreCase) >= 0)
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
                            if (((CheckBoxCell)row[0]).Checked != result.CheckedValues[0])
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

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Selected, "Selected", ConditionType.ConditionTypeEnum.Checkboxes));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RetailItem variantItem;
            string variantDescription;
            int counter = 1;
            int totalVariantItems = displayedRows.Count(p => ((CheckBoxCell) p[0]).Checked);

            using (ProgressDialog dlg = new ProgressDialog(Resources.CreatingVariantItems, Resources.CreatingVariantItemsCount, totalVariantItems))
            {
                Action action = () =>
                {
                    foreach (var row in displayedRows)
                    {

                        if (((CheckBoxCell) row[0]).Checked)
                        {
                            dlg.Report(counter, totalVariantItems);
                            variantDescription = "";
                            variantItem = new RetailItem();
                            variantItem.Text = item.Text;
                            variantItem.MasterID = Guid.NewGuid();
                            variantItem.HeaderItemID = item.MasterID;

                            foreach (var attribute in ((VariantItems) row.Tag).attributes)
                            {
                                if (variantDescription != "")
                                {
                                    variantDescription += " ";
                                }
                                variantDescription += attribute.Text;

                                Providers.RetailItemData.AddDimensionAttribute(PluginEntry.DataModel, variantItem.MasterID, attribute.ID);
                            }

                            variantItem.VariantName = variantDescription.Length > 60 ? variantDescription.Left(60) : variantDescription;
                            variantItem.ExtendedDescription = item.ExtendedDescription;
                            variantItem.ItemType = ItemTypeEnum.Item;
                            variantItem.ExtendedDescription = item.ExtendedDescription;
                            variantItem.RetailGroupMasterID = item.RetailGroupMasterID;
                            variantItem.RetailDepartmentMasterID = item.RetailDepartmentMasterID;
                            variantItem.RetailDivisionMasterID = item.RetailDivisionMasterID;
                            variantItem.SalesTaxItemGroupID = item.SalesTaxItemGroupID;
                            variantItem.InventoryUnitID = item.InventoryUnitID;
                            variantItem.SalesUnitID = item.SalesUnitID;

                            variantItem.SalesPrice = item.SalesPrice;
                            variantItem.SalesPriceIncludingTax = item.SalesPriceIncludingTax;

                            Providers.RetailItemData.Save(PluginEntry.DataModel, variantItem);
                            counter ++;
                        }
                    }
                };
                dlg.ProgressTask = Task.Run(action);
                dlg.ShowDialog();
            }
            PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.RetailItemsDashboardItemID);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void select_Click(object sender, EventArgs e)
        {
            changingAllCells = true;
            for (int i = 0; i < lvVariantItems.Selection.Count; i++)
            {
                Row row = lvVariantItems.Selection[i];
                ((CheckBoxCell)row[0]).Checked = true;
            }
            changingAllCells = false;
            btnOK.Enabled = true;
            lvVariantItems.InvalidateContent();
        }

        private void deselect_Click(object sender, EventArgs e)
        {
            changingAllCells = true;
            for (int i = 0; i < lvVariantItems.Selection.Count; i++)
            {
                Row row = lvVariantItems.Selection[i];
                ((CheckBoxCell)row[0]).Checked = false;
            }
            changingAllCells = false;

            btnOK.Enabled = false;
            foreach (var row in displayedRows)
            {
                if (((CheckBoxCell) row[0]).Checked)
                {
                    btnOK.Enabled = true;
                    break;
                }
            }

            lvVariantItems.InvalidateContent();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            changingAllCells = true;
            foreach (Row row in lvVariantItems.Rows)
            {
                ((CheckBoxCell)row[0]).Checked = true;
            }
            changingAllCells = false;
            btnOK.Enabled = true;

            lvVariantItems.InvalidateContent();
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            changingAllCells = true;
            foreach (Row row in lvVariantItems.Rows)
            {
                ((CheckBoxCell)row[0]).Checked = false;
            }
            changingAllCells = false;

            btnOK.Enabled = false;
            foreach (var row in displayedRows)
            {
                if (((CheckBoxCell)row[0]).Checked)
                {
                    btnOK.Enabled = true;
                    break;
                }
            }

            lvVariantItems.InvalidateContent();
        }

        private void lvVariantItems_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            btnOK.Enabled = ValidateCells(args);
        }

        private bool ValidateCells(Controls.EventArguments.CellEventArgs args)
        {
            if (changingAllCells)
            {
                return false;
            }
            if (((CheckBoxCell)args.Cell).Checked)
            {
                return true;
            }
            foreach (var row in displayedRows)
            {
                if (((CheckBoxCell)row[0]).Checked)
                {
                    return true;

                }
            }
            return false;
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
        }

        private List<VariantItems> CreateVariantItemsList(Dictionary<RecordIdentifier, List<DimensionAttribute>> dimensionAttributes)
        {
            List<List<DimensionAttribute>> attributeLists = new List<List<DimensionAttribute>>();

            foreach (var dimension in dimensionAttributes)
            {
                List<DimensionAttribute> attributes = new List<DimensionAttribute>();
                attributes = dimension.Value;
                attributeLists.Add(attributes);
            }
            List<VariantItems> variantItemList = createVariantItems(attributeLists);
            return variantItemList;
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

        class VariantItems
        {
            public VariantItems()
            {
                attributes = new List<DimensionAttribute>();
            }
            internal List<DimensionAttribute> attributes { get; }
        }
    }
}
