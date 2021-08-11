using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Replenishment.Dialogs;
using LSOne.ViewPlugins.Replenishment.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls.Cells;
using LSOne.Controls;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects.ItemMaster.MultiEditing;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.ViewCore.Dialogs;
using LSOne.Utilities.ColorPalette;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    public partial class RetailItemReplenishmentPage : UserControl, ITabViewV2, IMultiEditTabExtension
    {
        private RecordIdentifier itemID;
        private ItemReplenishmentSetting setting;
        private List<ItemReplenishmentSetting> storeSettings;
        private RecordIdentifier selectedStoreSettingId;
        private bool hasPermission;
        private RetailItem item;
        private Unit itemInventoryUnit;

        // Multiedit
        ItemReplenishmentSetting multiEditReplenishmentRecord;
        bool ReplenishmentMethodChanged;
        bool ReorderPointChanged;
        bool MaximumInventoryChanged;
        bool PurchaseOrderMultipleChanged;
        bool PurchaseOrderMultipleRoundingChanged;
        bool BlockedChanged;
        bool BlockedDateChanged;

        private const int SAVEDCOLUMN = 8;

        private class ExtendedCellWithFlags : ExtendedCell
        {
            public bool Saved;
            public bool AddAction;
        }

        public RetailItemReplenishmentPage()
        {
            InitializeComponent();


            lvStoreSettings.ContextMenuStrip = new ContextMenuStrip();
            lvStoreSettings.ContextMenuStrip.Opening += lvStoreSettings_RightClick;

            hasPermission = PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment);
            btnsEditAddRemove.AddButtonEnabled = hasPermission;

            InitializeComboBoxes();
        }

        private void lvStoreSettings_RightClick(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvStoreSettings.ContextMenuStrip;
            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   btnsEditAddRemove_AddButtonClicked)
            {
                Enabled = btnsEditAddRemove.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Edit,
                   100,
                   btnsEditAddRemove_EditButtonClicked)
            {
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Image = ContextButtons.GetEditButtonImage(),
                Default = true
            };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Delete,
                   300,
                   btnsEditAddRemove_RemoveButtonClicked)
            {
                Enabled = btnsEditAddRemove.RemoveButtonEnabled,
                Image = ContextButtons.GetRemoveButtonImage()
            };
            menu.Items.Add(item);

            if (this.itemID == RecordIdentifier.Empty)
            {
                item = new ExtendedMenuItem(
                    Properties.Resources.RemoveAction,
                    330, new EventHandler(RemoveAction));

                item.Image = PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete);

                item.Enabled = lvStoreSettings.Selection.Count > 0 && (!((ExtendedCellWithFlags)lvStoreSettings.Row(lvStoreSettings.Selection.FirstSelectedRow)[SAVEDCOLUMN]).Saved);

                menu.Items.Add(item);
            }

            PluginEntry.Framework.ContextMenuNotify("ItemStoreReplenishmentList", lvStoreSettings.ContextMenuStrip, lvStoreSettings);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void RemoveAction(object sender, EventArgs args)
        {
            lvStoreSettings.RemoveRow(lvStoreSettings.Selection.FirstSelectedRow);
        }

        private void InitializeComboBoxes()
        {
            cmbReplenishmentMethod.Items.Add(
                new DataEntity(
                    (int)ReplenishmentMethodEnum.StockLevel,
                    ItemReplenishmentSetting.ReplenishmentMethodToText(ReplenishmentMethodEnum.StockLevel)));
            cmbReplenishmentMethod.Items.Add(
                new DataEntity(
                    (int)ReplenishmentMethodEnum.LikeForLike,
                    ItemReplenishmentSetting.ReplenishmentMethodToText(ReplenishmentMethodEnum.LikeForLike)));
            cmbReplenishmentMethod.SelectedIndex = 0;

            cmbBlocked.Items.Add(
                new DataEntity(
                    (int)BlockedForReplenishmentEnum.NotBlocked,
                    ItemReplenishmentSetting.BlockedForReplenishmentToText(BlockedForReplenishmentEnum.NotBlocked)));
            cmbBlocked.Items.Add(
                new DataEntity(
                    (int)BlockedForReplenishmentEnum.BlockedForReplenishment,
                    ItemReplenishmentSetting.BlockedForReplenishmentToText(BlockedForReplenishmentEnum.BlockedForReplenishment)));
            cmbBlocked.SelectedIndex = 0;

            cmbPurchaseOrderMultipleRounding.Items.Add(
                new DataEntity(
                    (int)PurchaseOrderMultipleRoundingEnum.Nearest,
                    ItemReplenishmentSetting.PurchaseOrderMultipleRoundingToText(PurchaseOrderMultipleRoundingEnum.Nearest)));
            cmbPurchaseOrderMultipleRounding.Items.Add(
                new DataEntity(
                    (int)PurchaseOrderMultipleRoundingEnum.Down,
                    ItemReplenishmentSetting.PurchaseOrderMultipleRoundingToText(PurchaseOrderMultipleRoundingEnum.Down)));
            cmbPurchaseOrderMultipleRounding.Items.Add(
                new DataEntity(
                    (int)PurchaseOrderMultipleRoundingEnum.Up,
                    ItemReplenishmentSetting.PurchaseOrderMultipleRoundingToText(PurchaseOrderMultipleRoundingEnum.Up)));
            cmbPurchaseOrderMultipleRounding.SelectedIndex = 0;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.RetailItemReplenishmentPage();
        }

        #region ITabView Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {
            itemID = context;
            item = (RetailItem)internalContext;

            if (itemID == RecordIdentifier.Empty)
            {
                ntbReorderPoint.Enabled = false;
                ntbMaximumInventory.Enabled = false;

                // In multiedit we cannot sort since that would distory the time order of the transactions.
                for (int i = 0; i < lvStoreSettings.Columns.Count; i++)
                {
                    lvStoreSettings.Columns[i].Clickable = false;
                    lvStoreSettings.Columns[i].InternalSort = false;
                }

                lvStoreSettings.Columns.Add(new LSOne.Controls.Columns.Column() { HeaderText = Properties.Resources.Action, Clickable = false, AutoSize = true, Sizable = false });
                lvStoreSettings.Columns.Add(new LSOne.Controls.Columns.Column() { HeaderText = "", Clickable = false, AutoSize = true, Sizable = false, MinimumWidth = 20 });

                lvStoreSettings.AutoSizeColumns();

                lvStoreSettings.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRoundedColorMerge;

                btnsEditAddRemove.RemoveButtonEnabled = true;

                lvStoreSettings.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowSingleSelection;
            }
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            try
            {
                itemID = context;
                setting = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetItemReplenishmentSettingForItem(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), itemID, true);
                item = (RetailItem)internalContext;
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }

            if (setting == null)
            {
                setting = new ItemReplenishmentSetting { ItemId = itemID };
                if (item != null && item.InventoryExcluded)
                {
                    setting.BlockedForReplenishment = BlockedForReplenishmentEnum.BlockedForReplenishment;
                    setting.BlockingDate = DateTime.Today;
                }
            }

            LoadDataToForm(setting);
            LoadStoreList();
        }

        private void LoadDataToForm(ItemReplenishmentSetting settingToLoad)
        {
            string replenishmentMethodText = settingToLoad.ReplenishmentMethodText;
            string blockedForReplenishmentText = settingToLoad.BlockedForReplenishmentText;
            string purchaseOrderMultipleRoundingText = settingToLoad.PurchaseOrderMultipleRoundingText;

            cmbReplenishmentMethod.SelectedIndex = cmbReplenishmentMethod.FindStringExact(replenishmentMethodText);
            cmbBlocked.SelectedIndex = cmbBlocked.FindStringExact(blockedForReplenishmentText);
            cmbPurchaseOrderMultipleRounding.SelectedIndex = cmbPurchaseOrderMultipleRounding.FindStringExact(purchaseOrderMultipleRoundingText);

            RecordIdentifier unitID;
            try
            {
                unitID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryUnitId(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), itemID, true);

            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
            var unitDescription = Providers.UnitData.GetUnitDescription(PluginEntry.DataModel, unitID);
            lblReorderPointUnit.Text = unitDescription;
            lblMaximumInventoryUnit.Text = unitDescription;

            // Load unit decimal settings
            RecordIdentifier inventoryUnitID = item.InventoryUnitID;

            itemInventoryUnit = Providers.UnitData.Get(PluginEntry.DataModel, inventoryUnitID);

            ntbReorderPoint.DecimalLetters = ntbMaximumInventory.DecimalLetters = itemInventoryUnit.Limit.Max;
            ntbReorderPoint.AllowDecimal =
                ntbMaximumInventory.AllowDecimal =
                    itemInventoryUnit.Limit.Min != 0 || itemInventoryUnit.Limit.Max != 0;

            ntbReorderPoint.SetValueWithLimit(settingToLoad.ReorderPoint, itemInventoryUnit.Limit);
            ntbMaximumInventory.SetValueWithLimit(settingToLoad.MaximumInventory, itemInventoryUnit.Limit);

            ntbPurchaseOrderMultiple.Value = settingToLoad.PurchaseOrderMultiple;
            dtpBlockedDate.Value = settingToLoad.BlockingDate;

            dtpBlockedDate.Enabled = (settingToLoad.BlockedForReplenishment == BlockedForReplenishmentEnum.BlockedForReplenishment);

            if (item != null && item.InventoryExcluded)
            {
                cmbBlocked.Enabled = false;
                dtpBlockedDate.Enabled = false;
            }
        }

        private void CheckAddButtonEnabled()
        {
            if (itemID != RecordIdentifier.Empty)
            {
                if (!PluginEntry.DataModel.IsHeadOffice)
                {
                    for (int i = 0; i < lvStoreSettings.RowCount; i++)
                    {
                        if (((ItemReplenishmentSetting)lvStoreSettings.Rows[i].Tag).StoreId == PluginEntry.DataModel.CurrentStoreID)
                        {
                            btnsEditAddRemove.AddButtonEnabled = false;
                            return;
                        }
                    }
                }
            }

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment);

            if (item != null && item.InventoryExcluded)
            {
                btnsEditAddRemove.AddButtonEnabled = false;
                btnsEditAddRemove.EditButtonEnabled = false;
            }

        }

        private void LoadStoreList()
        {
            lvStoreSettings.ClearRows();
            try
            {
                storeSettings =
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .GetItemReplenishmentSettingListForStores(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true,
                            itemID);


            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }

            foreach (ItemReplenishmentSetting storeSetting in storeSettings)
            {
                var row = new Row();
                row.AddText(storeSetting.StoreName);
                row.AddText(storeSetting.ReplenishmentMethodText);
                row.AddCell(new NumericCell(storeSetting.ReorderPoint.FormatWithLimits(itemInventoryUnit.Limit), storeSetting.ReorderPoint));
                row.AddCell(new NumericCell(storeSetting.MaximumInventory.FormatWithLimits(itemInventoryUnit.Limit), storeSetting.MaximumInventory));
                row.AddCell(new NumericCell(storeSetting.PurchaseOrderMultiple.ToString(), storeSetting.PurchaseOrderMultiple));
                row.AddText(storeSetting.PurchaseOrderMultipleRoundingText);
                row.AddText(storeSetting.BlockedForReplenishmentText);

                if (storeSetting.BlockedForReplenishment == BlockedForReplenishmentEnum.BlockedForReplenishment)
                {
                    row.AddCell(new DateTimeCell(storeSetting.BlockingDate.ToShortDateString(), storeSetting.BlockingDate));
                }

                lvStoreSettings.AddRow(row);
                row.Tag = storeSetting;
            }

            lvStoreSettings.AutoSizeColumns(true);
            CheckAddButtonEnabled();

        }

        public bool DataIsModified()
        {
            if (setting == null) return true;
            if ((int)((DataEntity)cmbReplenishmentMethod.SelectedItem).ID != (int)setting.ReplenishmentMethod) return true;
            if ((int)((DataEntity)cmbBlocked.SelectedItem).ID != (int)setting.BlockedForReplenishment) return true;
            if ((int)((DataEntity)cmbPurchaseOrderMultipleRounding.SelectedItem).ID != (int)setting.PurchaseOrderMultipleRounding) return true;
            if (ntbReorderPoint.Value != (double)setting.ReorderPoint) return true;
            if (ntbMaximumInventory.Value != (double)setting.MaximumInventory) return true;
            if (ntbPurchaseOrderMultiple.Value != setting.PurchaseOrderMultiple) return true;
            if (dtpBlockedDate.Value != setting.BlockingDate) return true;
            return false;
        }

        public bool SaveData()
        {
            if (setting == null)
            {
                setting = new ItemReplenishmentSetting { ItemId = itemID };
            }

            setting.ReplenishmentMethod = (ReplenishmentMethodEnum)((int)((DataEntity)cmbReplenishmentMethod.SelectedItem).ID);
            setting.BlockedForReplenishment = (BlockedForReplenishmentEnum)((int)((DataEntity)cmbBlocked.SelectedItem).ID);
            setting.PurchaseOrderMultipleRounding = (PurchaseOrderMultipleRoundingEnum)((int)((DataEntity)cmbPurchaseOrderMultipleRounding.SelectedItem).ID);
            setting.ReorderPoint = (decimal)ntbReorderPoint.Value;
            setting.MaximumInventory = (decimal)ntbMaximumInventory.Value;
            setting.PurchaseOrderMultiple = (int)ntbPurchaseOrderMultiple.Value;
            setting.BlockingDate = dtpBlockedDate.Value;
            try
            {
                Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                    .SaveItemReplenishmentSetting(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), setting, true);

            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return false;
            }

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (changeHint == DataEntityChangeType.Edit && objectName == "ItemType" && changeIdentifier == item.ID)
            {
                ItemTypeEnum newType = (ItemTypeEnum)(int)(RecordIdentifier)param;
                bool isService = newType == ItemTypeEnum.Service;
                if(isService)
                {
                    cmbBlocked.SelectedIndex = (int)BlockedForReplenishmentEnum.BlockedForReplenishment;
                    dtpBlockedDate.Value = DateTime.Today;
                }

                cmbBlocked.Enabled = !isService;
                dtpBlockedDate.Enabled = !isService;
                btnsEditAddRemove.AddButtonEnabled = !isService;
                btnsEditAddRemove.EditButtonEnabled = !isService;
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbBlocked_SelectedIndexChanged(object sender, System.EventArgs e)
        {

            if (cmbBlocked.SelectedItem is DataEntity)
            {

                int blocking = (int)((DataEntity)cmbBlocked.SelectedItem).ID;
                switch (blocking)
                {
                    case (int)BlockedForReplenishmentEnum.NotBlocked:
                        dtpBlockedDate.Enabled = false;
                        break;
                    case (int)BlockedForReplenishmentEnum.BlockedForReplenishment:
                        dtpBlockedDate.Enabled = true;
                        break;
                }
            }
            else
            {
                // If we got here then we are in multiediting, and the field has the value "Not Set".
                dtpBlockedDate.Enabled = false;
            }

        }

        private void AddMultiEditRecord(ItemReplenishmentSetting record, bool addAction)
        {
            Row row;
            DecimalLimit limit = new DecimalLimit(2, 2);

            row = new Row();
            row.BackColor = ColorPalette.MultiEditHighlight;

            row.AddText(record.StoreName);
            row.AddText(record.ReplenishmentMethodText);
            row.AddCell(new NumericCell(record.ReorderPoint.FormatWithLimits(limit), record.ReorderPoint));
            row.AddCell(new NumericCell(record.MaximumInventory.FormatWithLimits(limit), record.MaximumInventory));
            row.AddCell(new NumericCell(record.PurchaseOrderMultiple.ToString(), record.PurchaseOrderMultiple));
            row.AddText(record.PurchaseOrderMultipleRoundingText);
            row.AddText(record.BlockedForReplenishmentText);

            if (record.BlockedForReplenishment == BlockedForReplenishmentEnum.BlockedForReplenishment)
            {
                row.AddCell(new DateTimeCell(record.BlockingDate.ToShortDateString(), record.BlockingDate));
            }
            else
            {
                row.AddText("");
            }

            row.AddCell(new ExtendedCellWithFlags() { Image = addAction ? ContextButtons.GetAddButtonImage() : ContextButtons.GetRemoveButtonImage(), Saved = false, AddAction = addAction, Text = addAction ? Properties.Resources.Add : Properties.Resources.Delete });
            row.AddCell(new IconButtonCell(new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.RemoveAction), IconButtonCell.IconButtonIconAlignmentEnum.Left | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter));

            lvStoreSettings.AddRow(row);
            row.Tag = record;

            lvStoreSettings.AutoSizeColumns(true);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, System.EventArgs e)
        {
            List<RecordIdentifier> storesInUse = new List<RecordIdentifier>();

            if (itemID != RecordIdentifier.Empty)
            {
                // In single edit then you may only select each store once, while in multiedit its more of transaction based then selecting
                // same again just creates another update transaction and does not add second store.
                for (int i = 0; i < lvStoreSettings.RowCount; i++)
                {
                    storesInUse.Add(((ItemReplenishmentSetting)lvStoreSettings.Rows[i].Tag).StoreId);
                }
            }

            ItemStoreReplenishmentSettingDialog dlg = ItemStoreReplenishmentSettingDialog.CreateForAdding(itemID, storesInUse);



            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (itemID == RecordIdentifier.Empty)
                {
                    // Multi edit
                    AddMultiEditRecord(dlg.ItemReplenishmentSetting, true);
                }
                else
                {
                    LoadStoreList();
                }
            }

            CheckAddButtonEnabled();
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, System.EventArgs e)
        {
            List<RecordIdentifier> storesInUse = new List<RecordIdentifier>();

            for (int i = 0; i < lvStoreSettings.RowCount; i++)
            {
                if (lvStoreSettings.Selection.FirstSelectedRow != i)
                {
                    storesInUse.Add(((ItemReplenishmentSetting)lvStoreSettings.Rows[i].Tag).StoreId);
                }
            }

            ItemStoreReplenishmentSettingDialog dlg = ItemStoreReplenishmentSettingDialog.CreateForEditing(itemID, selectedStoreSettingId, storesInUse);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadStoreList();
            }
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, System.EventArgs e)
        {
            if (itemID == RecordIdentifier.Empty)
            {
                // Multiedit
                ItemStoreReplenishmentSettingDialog dlg = ItemStoreReplenishmentSettingDialog.CreateForDeleting(itemID);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    AddMultiEditRecord(dlg.ItemReplenishmentSetting, false);
                }
            }
            else
            {
                // Single edit
                if (lvStoreSettings.Selection.Count == 1)
                {
                    PluginOperations.DeleteReplenishmentSetting(((ItemReplenishmentSetting)lvStoreSettings.Selection[0].Tag).ID);
                }
                else
                {
                    var selectedIDs = new List<RecordIdentifier>();

                    for (int i = 0; i < lvStoreSettings.Selection.Count; i++)
                    {
                        selectedIDs.Add(((ItemReplenishmentSetting)lvStoreSettings.Selection[i].Tag).ID);
                    }

                    PluginOperations.DeleteReplenishmentSettings(selectedIDs);
                }

                LoadStoreList();
            }

            CheckAddButtonEnabled();
        }

        private void lvStoreSettings_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvStoreSettings_SelectionChanged(object sender, EventArgs e)
        {
            var count = lvStoreSettings.Selection.Count;

            if (itemID != RecordIdentifier.Empty)
            {
                // We are in single edit, we handle this differently in multiedit.

                if (count == 0)
                {
                    btnsEditAddRemove.EditButtonEnabled = false;
                    btnsEditAddRemove.RemoveButtonEnabled = false;
                }
                else if (count == 1)
                {
                    selectedStoreSettingId = ((ItemReplenishmentSetting)lvStoreSettings.Selection[0].Tag).ID;
                    btnsEditAddRemove.EditButtonEnabled = hasPermission;
                    btnsEditAddRemove.RemoveButtonEnabled = hasPermission;
                }
                else if (count > 0)
                {
                    selectedStoreSettingId = null;
                    btnsEditAddRemove.EditButtonEnabled = false;
                    btnsEditAddRemove.RemoveButtonEnabled = hasPermission;
                }
            }

        }

        private void cmbReplenishmentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(cmbReplenishmentMethod.SelectedItem is DataEntity))
            {
                // We are in multiediting and the value is not set
                ntbReorderPoint.Enabled = false;
                lblReorderPoint.Enabled = false;

                ntbMaximumInventory.Enabled = false;
                lblMaximumInventoryUnit.Enabled = false;

                ntbMaximumInventory_TextChanged(sender, e);

                if (Parent != null)
                {
                    ((ViewBase)Parent.Parent.Parent).MultiEditRevertField(ntbMaximumInventory);
                    ((ViewBase)Parent.Parent.Parent).MultiEditRevertField(ntbReorderPoint);
                }

            }
            else
            {

                if (((DataEntity)cmbReplenishmentMethod.SelectedItem).ID ==
                    (int)ReplenishmentMethodEnum.LikeForLike)
                {
                    ntbReorderPoint.Enabled = false;
                    lblReorderPoint.Enabled = false;

                    ntbMaximumInventory.Enabled = true;
                    lblMaximumInventoryUnit.Enabled = true;

                    if (ntbMaximumInventory.MaxLength > 0)
                    {
                        ntbMaximumInventory_TextChanged(sender, e);
                    }
                }
                else
                {
                    ntbReorderPoint.Enabled = true;
                    lblReorderPoint.Enabled = true;

                    ntbMaximumInventory.Enabled = true;
                    lblMaximumInventoryUnit.Enabled = true;
                }
            }
        }

        private void ntbMaximumInventory_TextChanged(object sender, EventArgs e)
        {
            if ((cmbReplenishmentMethod.SelectedItem is DataEntity))
            {

                if (((DataEntity)cmbReplenishmentMethod.SelectedItem).ID ==
                (int)ReplenishmentMethodEnum.LikeForLike)
                {
                    ntbReorderPoint.Value = Math.Max(ntbMaximumInventory.Value - 1, 0);
                }
            }
            else
            {

            }
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {

        }

        public void MultiEditCollectData(IDataEntity dataEntity, HashSet<int> changedControlHashes, object param)
        {
            ReplenishmentMethodChanged = (changedControlHashes.Contains(cmbReplenishmentMethod.GetHashCode()));
            ReorderPointChanged = (changedControlHashes.Contains(ntbReorderPoint.GetHashCode()));
            MaximumInventoryChanged = (changedControlHashes.Contains(ntbMaximumInventory.GetHashCode()));
            PurchaseOrderMultipleChanged = (changedControlHashes.Contains(ntbPurchaseOrderMultiple.GetHashCode()));
            PurchaseOrderMultipleRoundingChanged = (changedControlHashes.Contains(cmbPurchaseOrderMultipleRounding.GetHashCode()));
            BlockedChanged = (changedControlHashes.Contains(cmbBlocked.GetHashCode()));
            BlockedDateChanged = (changedControlHashes.Contains(dtpBlockedDate.GetHashCode()));

            // Because of thread safety then we have no choice but to collect the data here and store it until the 
            // MultiEditSaveSecondaryRecords is called.
            multiEditReplenishmentRecord = new ItemReplenishmentSetting();

            if (ReplenishmentMethodChanged)
            {
                multiEditReplenishmentRecord.ReplenishmentMethod = (ReplenishmentMethodEnum)((int)((DataEntity)cmbReplenishmentMethod.SelectedItem).ID);
            }

            if (ReorderPointChanged)
            {
                multiEditReplenishmentRecord.ReorderPoint = (decimal)ntbReorderPoint.Value;
            }

            if (MaximumInventoryChanged)
            {
                multiEditReplenishmentRecord.MaximumInventory = (decimal)ntbMaximumInventory.Value;
            }

            if (PurchaseOrderMultipleChanged)
            {
                multiEditReplenishmentRecord.PurchaseOrderMultiple = (int)ntbPurchaseOrderMultiple.Value;
            }

            if (PurchaseOrderMultipleRoundingChanged)
            {
                multiEditReplenishmentRecord.PurchaseOrderMultipleRounding = (PurchaseOrderMultipleRoundingEnum)((int)((DataEntity)cmbPurchaseOrderMultipleRounding.SelectedItem).ID);
            }

            if (BlockedChanged)
            {
                multiEditReplenishmentRecord.BlockedForReplenishment = (BlockedForReplenishmentEnum)((int)((DataEntity)cmbBlocked.SelectedItem).ID);
            }

            if (BlockedDateChanged)
            {
                multiEditReplenishmentRecord.BlockingDate = dtpBlockedDate.Value;
            }
        }

        public bool MultiEditValidateSaveUnknownControls()
        {
            foreach (Row row in lvStoreSettings.Rows)
            {
                if (!((ExtendedCellWithFlags)row[SAVEDCOLUMN]).Saved)
                {
                    return true;
                }
            }

            return false;
        }

        public void MultiEditSaveSecondaryRecords(IConnectionManager threadedConnection, IDataEntity dataEntity, RecordIdentifier primaryRecordID)
        {
            ItemReplenishmentSetting setting;

            // If we got header item then we just ignore it and move on
            if ((dataEntity as RetailItemMultiEdit).ItemType == DataLayer.BusinessObjects.Enums.ItemTypeEnum.MasterItem)
            {
                return;
            }
            try
            {

                // If and only if something changed then we load the record and do what is needed to be done.
                if (ReplenishmentMethodChanged || ReorderPointChanged || MaximumInventoryChanged ||
                    PurchaseOrderMultipleChanged ||
                    PurchaseOrderMultipleRoundingChanged || BlockedChanged || BlockedDateChanged)
                {
                    setting = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetItemReplenishmentSettingForItem(threadedConnection, PluginOperations.GetSiteServiceProfile(), primaryRecordID.SecondaryID, false);

                    // No record was found so we need to create a new one.
                    if (setting == null)
                    {
                        setting = new ItemReplenishmentSetting();
                        setting.ItemId = primaryRecordID.SecondaryID;
                    }

                    if (ReplenishmentMethodChanged)
                    {
                        setting.ReplenishmentMethod = multiEditReplenishmentRecord.ReplenishmentMethod;
                    }

                    if (ReorderPointChanged)
                    {
                        setting.ReorderPoint = multiEditReplenishmentRecord.ReorderPoint;
                    }

                    if (MaximumInventoryChanged)
                    {
                        setting.MaximumInventory = multiEditReplenishmentRecord.MaximumInventory;
                    }

                    if (PurchaseOrderMultipleChanged)
                    {
                        setting.PurchaseOrderMultiple = multiEditReplenishmentRecord.PurchaseOrderMultiple;
                    }

                    if (PurchaseOrderMultipleRoundingChanged)
                    {
                        setting.PurchaseOrderMultipleRounding =
                            multiEditReplenishmentRecord.PurchaseOrderMultipleRounding;
                    }

                    if (BlockedChanged)
                    {
                        setting.BlockedForReplenishment = multiEditReplenishmentRecord.BlockedForReplenishment;
                    }

                    if (BlockedDateChanged)
                    {
                        setting.BlockingDate = multiEditReplenishmentRecord.BlockingDate;
                    }

                    if (setting.ReplenishmentMethod == ReplenishmentMethodEnum.LikeForLike)
                    {
                        setting.ReorderPoint = Math.Max(setting.MaximumInventory - 1, 0);
                    }
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveItemReplenishmentSetting(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), setting, false);
                }


                // Deal with the list view
                // --------------------------------------------------------------------------------------------------------------------------------
                foreach (Row row in lvStoreSettings.Rows)
                {
                    if (!((ExtendedCellWithFlags)row[SAVEDCOLUMN]).Saved)
                    {
                        setting = ((ItemReplenishmentSetting)row.Tag);
                        setting.ID = RecordIdentifier.Empty;
                        setting.ItemId = primaryRecordID.SecondaryID;

                        if (((ExtendedCellWithFlags)row[SAVEDCOLUMN]).AddAction)
                        {
                            // We need to fetch the old ID so it is clear if we are updating existing record or inserting.
                            setting.ID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetItemReplenishmentSettingID(threadedConnection, PluginOperations.GetSiteServiceProfile(), setting.ItemId, setting.StoreId, false);
                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).SaveItemReplenishmentSetting(threadedConnection, PluginOperations.GetSiteServiceProfile(), setting, false);
                        }
                        else
                        {
                            // Delete action
                            Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).DeleteItemReplenishmentSettingByItemIDAndStoreID(threadedConnection, PluginOperations.GetSiteServiceProfile(), setting.ItemId, setting.StoreId, false);
                        }

                    }
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

        public void MultiEditSaveSecondaryRecordsFinalizer()
        {
            foreach (Row row in lvStoreSettings.Rows)
            {
                if (!((ExtendedCellWithFlags)row[SAVEDCOLUMN]).Saved)
                {
                    ((ExtendedCellWithFlags)row[SAVEDCOLUMN]).Saved = true;
                    row.BackColor = Color.Empty;
                }

                row[SAVEDCOLUMN + 1] = new Cell("");
            }

            lvStoreSettings.Invalidate();
        }

        public void MultiEditRevertUnknownControl(Control control, bool isRevertField, ref bool handled)
        {
            if (control == lvStoreSettings)
            {
                if (lvStoreSettings.Selection.Count > 0)
                {
                    lvStoreSettings.RemoveRow(lvStoreSettings.Selection.FirstSelectedRow);
                }

                lvStoreSettings.Invalidate();
            }


        }

        private void lvStoreSettings_CellAction(object sender, CellEventArgs args)
        {
            lvStoreSettings.RemoveRow(args.RowNumber);
        }
    }
}
