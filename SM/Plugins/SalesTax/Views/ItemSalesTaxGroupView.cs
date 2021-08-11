using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.SalesTax.Dialogs;
using LSOne.ViewPlugins.SalesTax.Properties;

namespace LSOne.ViewPlugins.SalesTax.Views
{
    public partial class ItemSalesTaxGroupView : ViewBase
    {

        private RecordIdentifier selectedTaxItemGroup;

        public ItemSalesTaxGroupView(RecordIdentifier id)
            : this()
        {
            selectedTaxItemGroup = id;
        }

        public ItemSalesTaxGroupView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;

            ImageList imageList = PluginEntry.Framework.GetImageList();
            lvGroups.SmallImageList = imageList;
            lvValues.SmallImageList = imageList;

            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += lvGroups_Opening;
            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);
            btnsContextButtons.AddButtonEnabled = btnsContextButtonsItems.AddButtonEnabled = !ReadOnly;

            lvGroups.Columns[0].Tag = ItemSalesTaxGroup.SortEnum.ID;
            lvGroups.Columns[1].Tag = ItemSalesTaxGroup.SortEnum.Description;

            lvValues.Columns[0].Tag = TaxCodeInItemSalesTaxGroup.SortEnum.SalesTaxCode;
            lvValues.Columns[1].Tag = TaxCodeInItemSalesTaxGroup.SortEnum.Description;
            lvValues.Columns[2].Tag = TaxCodeInItemSalesTaxGroup.SortEnum.PercentageAmount;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("ItemSalesTaxGroups", RecordIdentifier.Empty, Properties.Resources.ItemSalesTaxGroup, false));
            contexts.Add(new AuditDescriptor("ItemSalesTaxGroupCodes", RecordIdentifier.Empty, Properties.Resources.SalesTaxCodeInItemSalesTaxGroup, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.ItemSalesTaxGroup;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadItems();
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.ItemSalesTaxGroup;
            //HeaderIcon = Properties.Resources.Tax16;
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if ((changeHint == DataEntityChangeType.Add || changeHint == DataEntityChangeType.Edit) && objectName == "ItemSalesTaxCodeInGroup" && changeIdentifier == selectedTaxItemGroup)
            {
                LoadLines();
            }
        }

        private void LoadItems()
        {
            ListViewItem listItem;

            lvGroups.Items.Clear();

            List<ItemSalesTaxGroup> ISTgroups = Providers.ItemSalesTaxGroupData.GetItemSalesTaxGroups(
                PluginEntry.DataModel,
                (ItemSalesTaxGroup.SortEnum)lvGroups.Columns[lvGroups.SortColumn].Tag,
                lvGroups.SortedBackwards);

            foreach (ItemSalesTaxGroup group in ISTgroups)
            {
                listItem = new ListViewItem((string)group.ID);
                listItem.SubItems.Add(group.Text);
                listItem.Tag = group.ID;
                listItem.ImageIndex = -1;

                lvGroups.Add(listItem);

                if (selectedTaxItemGroup != null && selectedTaxItemGroup.StringValue == group.ID.StringValue)
                {
                    listItem.Selected = true;
                }
            }

            lvGroups.Columns[lvGroups.SortColumn].ImageIndex = (lvGroups.SortedBackwards ? 1 : 0);
            lvGroups.BestFitColumns();
            lvGroups.ShowSelectedItem();
            lvGroups_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void LoadLines()
        {
            ListViewItem listItem;

            if (lvGroups.SelectedItems.Count == 0)
                return;

            lvValues.Items.Clear();

            List<TaxCodeInItemSalesTaxGroup> lines = Providers.ItemSalesTaxGroupData.GetTaxCodesInItemSalesTaxGroup(
                PluginEntry.DataModel,
                SelectedItemSalesTaxGroup,
                (TaxCodeInItemSalesTaxGroup.SortEnum)lvValues.Columns[lvValues.SortColumn].Tag,
                lvValues.SortedBackwards);

            foreach (TaxCodeInItemSalesTaxGroup line in lines)
            {
                listItem = new ListViewItem((string)line.TaxCode);
                listItem.SubItems.Add(line.Text);
                listItem.SubItems.Add(line.TaxValue == -1 ?
                                      Properties.Resources.NotSet :
                                      line.TaxValue.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Tax)) + " %"
                                      );

                listItem.Tag = line.ID;
                listItem.ImageIndex = -1;

                lvValues.Add(listItem);
            }

            lvValues.Columns[lvValues.SortColumn].ImageIndex = (lvValues.SortedBackwards ? 1 : 0);
            lvValues.BestFitColumns();
            lvValues_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private RecordIdentifier SelectedItemSalesTaxGroup
        {
            get { return (RecordIdentifier)lvGroups.SelectedItems[0].Tag; }
        }

        private void lvGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTaxItemGroup = (lvGroups.SelectedItems.Count > 0) ? (RecordIdentifier)lvGroups.SelectedItems[0].Tag : "";
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled =
                (lvGroups.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);


            if (lvGroups.SelectedItems.Count > 0)
            {
                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible = true;
                    lvValues.Visible = true;
                    btnsContextButtonsItems.Visible = true;
                    btnEditTaxCode.Visible = true;
                    lblNoSelection.Visible = false;
                }

                LoadLines();
            }
            else if (lblGroupHeader.Visible)
            {
                lblGroupHeader.Visible = false;
                lvValues.Visible = false;
                btnsContextButtonsItems.Visible = false;
                btnEditTaxCode.Visible = false;
                lblNoSelection.Visible = true;
            }
        }

        private void lvValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditTaxCode.Enabled = btnsContextButtonsItems.RemoveButtonEnabled =
                lvValues.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);
        }

        private void lvGroups_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvGroups.SortColumn == e.Column)
            {
                lvGroups.SortedBackwards = !lvGroups.SortedBackwards;
            }
            else
            {
                if (lvGroups.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvGroups.Columns[lvGroups.SortColumn].ImageIndex = 2;
                    lvGroups.SortColumn = e.Column;
                }
                lvGroups.SortedBackwards = false;
            }

            LoadItems();
        }

        private void lvValues_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvValues.SortColumn == e.Column)
            {
                lvValues.SortedBackwards = !lvValues.SortedBackwards;
            }
            else
            {
                if (lvValues.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvValues.Columns[lvValues.SortColumn].ImageIndex = 2;
                    lvValues.SortColumn = e.Column;
                }
                lvValues.SortedBackwards = false;
            }

            LoadLines();
        }

        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvGroups.ContextMenuStrip;
            ExtendedMenuItem item;
            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditText + "...",
                    100,
                    new EventHandler(btnEdit_Click))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    new EventHandler(btnAdd_Click))
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsContextButtons.AddButtonEnabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    new EventHandler(btnRemove_Click))
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsContextButtons.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ItemSalesTaxGroupsList", lvGroups.ContextMenuStrip, lvGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lvValues_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    100,
                    new EventHandler(btnAddValue_Click))
            {
                Enabled = btnsContextButtonsItems.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    200,
                    new EventHandler(btnRemoveValue_Click))
            {
                Enabled = btnsContextButtonsItems.RemoveButtonEnabled,
                Image = ContextButtons.GetRemoveButtonImage()
            };

            menu.Items.Add(item);


            menu.Items.Add(new ExtendedMenuItem("-", 300));

            item = new ExtendedMenuItem(
                   btnEditTaxCode.Text + "...",
                   400,
                   new EventHandler(btnEditTaxCode_Click))
            {
                Enabled = btnEditTaxCode.Enabled,
                Default = true
            };

            menu.Items.Add(item);


            PluginEntry.Framework.ContextMenuNotify("ItemSalesTaxGroupLineList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.SalesTax.Views.ItemSalesTaxGroupView.Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.SalesTaxCodes, new ContextbarClickEventHandler(PluginOperations.ShowSalesTaxCodesView)), 10);
            }
        }

        private void lvGroups_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            ItemSalesTaxGroupDialog dlg = new Dialogs.ItemSalesTaxGroupDialog(selectedTaxItemGroup);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ItemSalesTaxGroupDialog dlg = new Dialogs.ItemSalesTaxGroupDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedTaxItemGroup = dlg.GetItemSalesTaxGroup.ID;
                LoadItems();
            }
        }

        private void ShowChangePriceDialog(UpdateItemTaxPricesEnum updateItemTaxPricesEnum, RecordIdentifier itemSalesTaxGroupID)
        {
            UpdateItemPricesTaxQuestionDialog dlg = new UpdateItemPricesTaxQuestionDialog();
            if (dlg.Show(PluginEntry.DataModel) == DialogResult.Yes)
            {
                // Update item prices within a progress indicator
                ShowProgress(delegate (System.Object o, System.EventArgs ea)
                    {
                        int updatedItemsCount;
                        int updatedTradeAgreementsCount;
                        int updatedPromotionOfferLinesCount;

                        Services.Interfaces.Services.TaxService(PluginEntry.DataModel).UpdatePrices(
                            PluginEntry.DataModel,
                            itemSalesTaxGroupID,
                            updateItemTaxPricesEnum,
                            out updatedItemsCount,
                            out updatedTradeAgreementsCount,
                            out updatedPromotionOfferLinesCount);

                        HideProgress();

                        MessageDialog.Show(Properties.Resources.ItemsUpdated + ": " + updatedItemsCount + " \n" +
                                           Properties.Resources.TradeAgreementsUpdated + ": " + updatedTradeAgreementsCount + "\n" +
                                           Properties.Resources.PromotionLinesUpdated + ": " + updatedPromotionOfferLinesCount + "\n");
                    }, Properties.Resources.Processing);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteItemSalesTaxGroupQuestion,
                Properties.Resources.DeleteItemSalesTaxGroupValue) == DialogResult.Yes)
            {
                // Check if any retail item is using this group
                if (Providers.RetailItemData.TaxGroupExists(PluginEntry.DataModel, selectedTaxItemGroup))
                {
                    if (QuestionDialog.Show(
                         Properties.Resources.OneOrMoreItemsUseTaxGroup,
                         Properties.Resources.DeleteItemSalesTaxGroupValue) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                List<TaxCode> taxCodesBothInDeletedGroupAndDefaultStoresTaxGroup =
                    Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetCommonTaxCodes(
                    PluginEntry.DataModel,
                    selectedTaxItemGroup,
                    Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel));

                Providers.ItemSalesTaxGroupData.Delete(PluginEntry.DataModel, selectedTaxItemGroup);

                // Only want to update prices if the deleted tax group had any tax codes in it that were also in the defaults stores
                // tax group. If this condition is not fulfilled then the prices could not have changed by deleting the group
                if (taxCodesBothInDeletedGroupAndDefaultStoresTaxGroup.Count > 0)
                {
                    ShowChangePriceDialog(UpdateItemTaxPricesEnum.AllItems, RecordIdentifier.Empty);
                }

                LoadItems();
            }
        }

        private void btnAddValue_Click(object sender, EventArgs e)
        {
            ItemSalesTaxCodeInGroupDialog dlg = new Dialogs.ItemSalesTaxCodeInGroupDialog(selectedTaxItemGroup);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (Providers.SalesTaxGroupData.TaxCodeIsInDefaultStoreTaxGroup(PluginEntry.DataModel, dlg.SelectedTaxCode))
                {
                    ShowChangePriceDialog(UpdateItemTaxPricesEnum.AllItems, (RecordIdentifier)lvGroups.SelectedItems[0].Tag);
                }

                LoadItems();
            }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            if (lvValues.Items.Count == 1 &&
                Providers.ItemSalesTaxGroupData.TaxGroupInUse(PluginEntry.DataModel, SelectedItemSalesTaxGroup))
            {
                MessageDialog.Show(Resources.CannotRemoveTaxCode + " " + Resources.AtLeastOneTaxCodeForRetailItemTaxGroup);
            }
            else
            {
                Providers.ItemSalesTaxGroupData.RemoveTaxCodeFromItemSalesTaxGroup(PluginEntry.DataModel, (RecordIdentifier)lvValues.SelectedItems[0].Tag);

                if (Providers.SalesTaxGroupData.TaxCodeIsInDefaultStoreTaxGroup(PluginEntry.DataModel, ((RecordIdentifier)lvValues.SelectedItems[0].Tag).SecondaryID))
                {
                    ShowChangePriceDialog(UpdateItemTaxPricesEnum.AllItems, (RecordIdentifier)lvGroups.SelectedItems[0].Tag);
                }

                LoadLines();
            }
        }

        private void btnEditTaxCode_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowSalesTaxCodesView(((RecordIdentifier)lvValues.SelectedItems[0].Tag).SecondaryID);
        }

        protected override void OnClose()
        {
            lvGroups.SmallImageList = null;
            lvValues.SmallImageList = null;

            base.OnClose();
        }

        private void lvValues_DoubleClick(object sender, EventArgs e)
        {
            if (btnEditTaxCode.Enabled)
            {
                btnEditTaxCode_Click(this, EventArgs.Empty);
            }
        }
    }
}
