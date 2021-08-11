using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.VariantFramework.Dialogs;
using LSOne.ViewPlugins.VariantFramework.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.VariantFramework.ViewPages
{
    public partial class ItemViewVariantItemsPage : UserControl, ITabView
    {
        private RetailItem item;
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("A863C870-9E86-4040-BB0C-A3E29E38F9E0");
        private Dictionary<RecordIdentifier, List<DimensionAttribute>> variantItems;
        private List<RetailItemDimension> itemDimensions;
        private List<Row> displayedRows;
        private bool hasPermissions;
        private Image variantItem16;
        private List<SimpleRetailItem> items;

        public ItemViewVariantItemsPage()
        {
            variantItem16 = Resources.itemVariant_16;

            InitializeComponent();
            hasPermissions = PluginEntry.DataModel.HasPermission(Permission.ItemsEdit) && PluginEntry.DataModel.HasPermission(Permission.ManageItemDimensions);

            btnEditDimension.Enabled = hasPermissions;

            searchBar.BuddyControl = lvVariantItems;

            lvVariantItems.ContextMenuStrip = new ContextMenuStrip();
            lvVariantItems.ContextMenuStrip.Opening += DimensionsContextMenuStrip_Opening;
            

        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ItemViewVariantItemsPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)internalContext;

            LoadListView();
        }

        private void LoadListView()
        {

            variantItems = Providers.DimensionAttributeData.GetRetailItemDimensionAttributeRelations(PluginEntry.DataModel, item.MasterID, false);
            itemDimensions = Providers.RetailItemDimensionData.GetListForRetailItem(PluginEntry.DataModel, item.MasterID);
            //imported variants do not have dimensions but we still need to show them
            items = Providers.RetailItemData.GetItemVariants(PluginEntry.DataModel, item.MasterID, SortEnum.IDDesc);

            displayedRows = new List<Row>();
            lvVariantItems.Rows.Clear();
            lvVariantItems.Columns.Clear();

            Column idColumn = new Column();
            idColumn.HeaderText = Resources.ItemID;
            idColumn.AutoSize = true;
            idColumn.Clickable = true;
            idColumn.InternalSort = true;
            lvVariantItems.Columns.Add(idColumn);

            //Description column is only needed for imported variants
            Column descrColumn = new Column
            {
                HeaderText = Resources.ItemDescription,
                AutoSize = true,
                Clickable = true,
                InternalSort = true
            };
            lvVariantItems.Columns.Add(descrColumn);

            if (variantItems == null || items == null)
                return;

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

            Row row;
            SimpleRetailItem vItem;
            foreach (KeyValuePair<RecordIdentifier, List<DimensionAttribute>> variantItem in variantItems)
            {
                row = new Row();
                row.AddCell(new ExtendedCell((string)variantItem.Key.SecondaryID, variantItem16));

                vItem = items.FirstOrDefault(i => i.ID == variantItem.Key.SecondaryID);
                row.AddText(item == null ? "" : vItem.VariantName);
                items.Remove(vItem); //remove variant with dimensions from variants list not to show duplicates

                foreach (var attribute in variantItem.Value)
                {
                    row.AddText(attribute.Text);
                }
                row.Tag = variantItem.Key;

                displayedRows.Add(row);
            }

            //whatever items remained in the list they are imported variants with no dimensions
            if (items.Count > 0)
            {
                foreach (var variant in items)
                {
                    row = new Row();
                    row.AddCell(new ExtendedCell((string)variant.ID, variantItem16));
                    row.AddText(variant.VariantName);
                    row.Tag = variant.ID;

                    displayedRows.Add(row);
                }
            }

            Filter();
        }

        private void Filter()
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
                                for (uint i = 0; i < lvVariantItems.Columns.Count; i++)
                                {
                                    if (row[i] != null && row[i].Text != "")
                                    {
                                        if (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith)
                                        {
                                            
                                            if ((string.Compare(row[i].Text.Left(result.StringValue.Length), result.StringValue, CultureInfo.CurrentCulture,CompareOptions.IgnoreCase)) == 0)
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
                                }
                                if (!textCondition)
                                {
                                    addItem = false;
                                }
                            }
                            break;
                    }
                }

                if (addItem)
                {
                    lvVariantItems.AddRow(row);
                }
     

            }

            // If there are no imported variant items we hide the description column
            if (items.Count <= 0)
            {
                lvVariantItems.Columns[1].MaximumWidth = 1;
                lvVariantItems.Columns[1].MinimumWidth = 1;
            }

            lvVariantItems.AutoSizeColumns();
        }

        public bool DataIsModified()
        {
            return true;
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
                case "VariantItem":
                    LoadListView();
                    break;
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        private void DimensionsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvVariantItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnEdit_Click)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.AddVariantItem,
                    200,
                    btnAdd_Click)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsContextButtons.AddButtonEnabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.RemoveVariantItem,
                    300,
                    btnRemove_Click)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsContextButtons.RemoveButtonEnabled
            };

            menu.Items.Add(item);
        }

        private void lvVariantItems_SelectionChanged(object sender, EventArgs e)
        {
            bool hasViewPermission = PluginEntry.DataModel.HasPermission(Permission.ItemsView);
            bool hasEditPermission = PluginEntry.DataModel.HasPermission(Permission.ItemsEdit);
            bool hasBulkEditPermission = PluginEntry.DataModel.HasPermission(Permission.MultiEditItems) && PluginEntry.DataModel.HasPermission(Permission.ItemsEdit);

            btnsContextButtons.EditButtonEnabled = ((lvVariantItems.Selection.Count == 1) && hasViewPermission) || ((lvVariantItems.Selection.Count > 1) && hasBulkEditPermission);
            btnsContextButtons.RemoveButtonEnabled = (lvVariantItems.Selection.Count >= 1) && hasEditPermission;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvVariantItems.Selection.Count == 1)
            {
                if (PluginEntry.Framework.CanRunOperation("EditRetailItem"))
                {
                    RecordIdentifier variantID = Providers.RetailItemData.GetItemIDFromMasterID(PluginEntry.DataModel, (RecordIdentifier)lvVariantItems.Selection[0].Tag);
                    var args = new PluginOperationArguments(variantID, null, true);

                    PluginEntry.Framework.RunOperation("EditRetailItem", this, args);
                    if (args.ResultView != null)
                    {
                        PluginEntry.Framework.ViewController.Add(args.ResultView);
                    }
                }
            }
            else
            {
                IEnumerable<IDataEntity> selectedItems;
                List<RecordIdentifier> selectedItemIDs = new List<RecordIdentifier>();

                if (PluginEntry.Framework.CanRunOperation("EditRetailItems"))
                {
                    for (int i = 0; i < lvVariantItems.Selection.Count; i++)
                    {
                        selectedItemIDs.Add(((RecordIdentifier)lvVariantItems.Selection[i].Tag));
                    }

                    selectedItems = Providers.RetailItemData.GetSpecificItemVariants(PluginEntry.DataModel, item.MasterID, selectedItemIDs);


                    var args = new PluginOperationArguments(null, selectedItems, true);

                    PluginEntry.Framework.RunOperation("EditRetailItems", this, args);
                    if (args.ResultView != null)
                    {
                        PluginEntry.Framework.ViewController.Add(args.ResultView);
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddVariantItemsDialog dlg = new AddVariantItemsDialog(item);

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                LoadListView();
            }
        }

        private void btnEditDimension_Click(object sender, EventArgs e)
        {
            EditDimensionsDialog dlg = new EditDimensionsDialog(item);
            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                LoadListView();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvVariantItems.Selection.Count == 1)
            {
                PluginOperations.DeleteItem(((RecordIdentifier)lvVariantItems.Selection[0].Tag).SecondaryID, ((RecordIdentifier)lvVariantItems.Selection[0].Tag));
            }
            else
            {
                List<RecordIdentifier> selectedIDs = new List<RecordIdentifier>();

                for (int i = 0; i < lvVariantItems.Selection.Count; i++)
                {
                    selectedIDs.Add(((RecordIdentifier)lvVariantItems.Selection[i].Tag));
                }

                PluginOperations.DeleteItems(selectedIDs);
            }
            if (lvVariantItems.Selection.FirstSelectedRow != 0)
            {
                if (lvVariantItems.Selection.FirstSelectedRow == lvVariantItems.RowCount)
                {
                    lvVariantItems.Selection.Set(lvVariantItems.Selection.FirstSelectedRow - 1);
                }
                else
                {
                    lvVariantItems.Selection.Set(lvVariantItems.Selection.FirstSelectedRow);
                }
            }
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));

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

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            Filter();
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

        private void lvVariantItems_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }
    }
}
