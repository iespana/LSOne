using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SalesTax.Properties;

namespace LSOne.ViewPlugins.SalesTax.Views
{
    public partial class SalesTaxGroupView : ViewBase
    {

        private RecordIdentifier selectedTaxGroup;
        private RecordIdentifier selectedTaxCode;
        private bool showIsForEUcolumn;

        public SalesTaxGroupView(RecordIdentifier id)
            : this()
        {
            selectedTaxGroup = id;
        }

        public SalesTaxGroupView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;

            var imageList = PluginEntry.Framework.GetImageList();
            lvGroups.SmallImageList = imageList;
            lvValues.SmallImageList = imageList;

            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += lvGroups_Opening;

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);

            btnsContextButtons.AddButtonEnabled = btnsContextButtonsItems.AddButtonEnabled = !ReadOnly;

            lvGroups.Columns[0].Tag = SalesTaxGroup.SortEnum.ID;
            lvGroups.Columns[1].Tag = SalesTaxGroup.SortEnum.Description;
            lvGroups.Columns[2].Tag = SalesTaxGroup.SortEnum.CountryRegion;
            lvGroups.Columns[3].Tag = SalesTaxGroup.SortEnum.CountyPurpose;

            lvValues.Columns[0].Tag = TaxCodeInSalesTaxGroup.SortEnum.SalesTaxCode;
            lvValues.Columns[1].Tag = TaxCodeInSalesTaxGroup.SortEnum.Description;
            lvValues.Columns[2].Tag = TaxCodeInSalesTaxGroup.SortEnum.PercentageAmount;

            IPlugin sapPlugin = PluginEntry.Framework.FindImplementor(null, "SAPBusinessOne", null);

            showIsForEUcolumn = sapPlugin != null;

            if (showIsForEUcolumn)
            {
                lvGroups.Columns.Add(new ColumnHeader
                {
                    Text = Resources.IsForEU,
                    Tag = SalesTaxGroup.SortEnum.IsForEU
                });
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("SalesTaxGroup", RecordIdentifier.Empty, Properties.Resources.SalesTaxGroup, false));
            contexts.Add(new AuditDescriptor("SalesTaxCodeInGroup", RecordIdentifier.Empty, Properties.Resources.SalesTaxCodeInItemSalesTaxGroup, false));
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.SalesTax.Views.SalesTaxGroupView.Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.SalesTaxCodes, new ContextbarClickEventHandler(PluginOperations.ShowSalesTaxCodesView)), 10);
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.SalesTaxGroup;
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
            HeaderText = Properties.Resources.SalesTaxGroup;
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
            
            if ((changeHint == DataEntityChangeType.Add || changeHint == DataEntityChangeType.Edit) && objectName == "SalesTaxCodeInGroup" && changeIdentifier == selectedTaxGroup)
            {
                LoadLines();
            }
        }

        private void LoadItems()
        {
            ListViewItem listItem;

            lvGroups.Items.Clear();

            List<SalesTaxGroup> groups = Providers.SalesTaxGroupData.GetSalesTaxGroups(PluginEntry.DataModel, (SalesTaxGroup.SortEnum)lvGroups.Columns[lvGroups.SortColumn].Tag, lvGroups.SortedBackwards);

            foreach (var group in groups)
            {
                listItem = new ListViewItem((string)group.ID);
                listItem.SubItems.Add(group.Text);
                listItem.SubItems.Add(group.SearchField1);
                listItem.SubItems.Add(group.SearchField2);

                if(showIsForEUcolumn)
                {
                    listItem.SubItems.Add(group.IsForEU ? Resources.Yes : Resources.No);
                }

                listItem.Tag = group.ID;
                listItem.ImageIndex = -1;

                lvGroups.Add(listItem);

                if (selectedTaxGroup == group.ID)
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

            if (lvGroups.SelectedItems.Count == 0) return;

            lvValues.Items.Clear();

            List<TaxCodeInSalesTaxGroup> lines = Providers.SalesTaxGroupData.GetTaxCodesInSalesTaxGroup(
                PluginEntry.DataModel,
                SelectedSalesTaxGroup,
                (TaxCodeInSalesTaxGroup.SortEnum)lvValues.Columns[lvValues.SortColumn].Tag,
                lvValues.SortedBackwards);

            foreach (var line in lines)
            {
                listItem = new ListViewItem((string)line.TaxCode);
                listItem.SubItems.Add(line.Text);
                //listItem.SubItems.Add(line.ExcemptTax ? Properties.Resources.Yes:Properties.Resources.No);
                //listItem.SubItems.Add(line.UseTax ? Properties.Resources.Yes : Properties.Resources.No);
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

        private RecordIdentifier SelectedSalesTaxGroup
        {
            get { return (RecordIdentifier)lvGroups.SelectedItems[0].Tag; }
        }

        private void lvGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTaxGroup = (lvGroups.SelectedItems.Count > 0) ? (RecordIdentifier)lvGroups.SelectedItems[0].Tag : "";
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = 
                (lvGroups.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);

            if (lvGroups.SelectedItems.Count > 0)
            {
                if (!lblGroupHeader.Visible)
                {
                    lblGroupHeader.Visible = true;
                    lvValues.Visible = true;
                    btnsContextButtonsItems.Visible = true;
                    lblNoSelection.Visible = false;
                }

                LoadLines();
            }
            else if (lblGroupHeader.Visible)
            {
                lblGroupHeader.Visible = false;
                lvValues.Visible = false;
                btnsContextButtonsItems.Visible = false;
                lblNoSelection.Visible = true;
            }
        }

        private void lvValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTaxCode = (lvValues.SelectedItems.Count > 0) ? ((RecordIdentifier)lvValues.SelectedItems[0].Tag).SecondaryID : "";
            btnsContextButtonsItems.EditButtonEnabled = btnsContextButtonsItems.RemoveButtonEnabled = 
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
                               Enabled = btnsContextButtons.EditButtonEnabled,
                               Image = ContextButtons.GetEditButtonImage(),
                               Default = true
                           };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    new EventHandler(btnAdd_Click))
                       {
                           Enabled = btnsContextButtons.AddButtonEnabled,
                           Image = ContextButtons.GetAddButtonImage()
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    new EventHandler(btnRemove_Click))
                       {
                           Enabled = btnsContextButtons.RemoveButtonEnabled,
                           Image = ContextButtons.GetRemoveButtonImage()
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
            var item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    new EventHandler(btnAddValue_Click))
                       {
                           Enabled = btnsContextButtonsItems.AddButtonEnabled,
                           Image = ContextButtons.GetAddButtonImage()
                       };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    new EventHandler(btnRemoveValue_Click))
                       {
                           Enabled = btnsContextButtonsItems.RemoveButtonEnabled,
                           Image = ContextButtons.GetRemoveButtonImage()
                       };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("SalesTaxGroupLineList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        //protected override void OnSetupContextBarItems(SharedCore.EventArguments.ContextBarItemConstructionArguments arguments)
        //{
        //    if (arguments.CategoryKey == "LSOne.ViewPlugins.SalesTax.Views.ItemSalesTaxGroupView.Related")
        //    {
        //        arguments.Add(new ContextBarItem(Properties.Resources.SalesTaxCodes, new ContextbarClickEventHandler(PluginOperations.ShowSalesTaxCodesView)), 10);
        //    }
        //}
        
        private void lvGroups_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void lvValues_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtonsItems.EditButtonEnabled)
            {
                btnEditValue_Click(this, EventArgs.Empty);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.SalesTaxGroupDialog(selectedTaxGroup);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.SalesTaxGroupDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedTaxGroup = dlg.SalesTaxGroupID;
                LoadItems();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteSalesTaxGroupQuestion,
                Properties.Resources.DeleteSalesTaxGroup) == DialogResult.Yes)
            {
                // Check if any customer is using this group
                if (Providers.CustomerData.TaxGroupExists(PluginEntry.DataModel, selectedTaxGroup))
                {
                   if (QuestionDialog.Show(
                        Properties.Resources.OneOrMoreCustomersUseTaxGroup,
                        Properties.Resources.DeleteSalesTaxGroup) != DialogResult.Yes)
                   {
                        return;
                   }
                }

                // Check if any store is using this group
                if (Providers.StoreData.TaxGroupExists(PluginEntry.DataModel, selectedTaxGroup))
                {
                    if (QuestionDialog.Show(
                         Properties.Resources.OneOrMoreStoreUseTaxGroup,
                         Properties.Resources.DeleteSalesTaxGroup) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                // Check if any vendor is using this group
                if (Providers.VendorData.TaxGroupExists(PluginEntry.DataModel, selectedTaxGroup))
                {
                    if (QuestionDialog.Show(
                         Properties.Resources.OneOrMoreVendorsUseTaxGroup,
                         Properties.Resources.DeleteSalesTaxGroup) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                bool deletingDefaultStoresTaxGroup = (Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel) == selectedTaxGroup);

                Providers.SalesTaxGroupData.Delete(PluginEntry.DataModel, selectedTaxGroup);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "SalesTaxGroup", selectedTaxGroup, null);
                if (deletingDefaultStoresTaxGroup)
                {
                    ShowChangePriceDialog();
                }

                LoadItems();
            }
        }

        private void btnEditValue_Click(object sender, EventArgs e)
        {

        }

        private void btnAddValue_Click(object sender, EventArgs e)
        {
            var dlg = new Dialogs.SalesTaxCodeInGroupDialog(selectedTaxGroup);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // Only if the affected tax group is the one default store is using
                if (Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel) == selectedTaxGroup)
                {
                    ShowChangePriceDialog();
                }
                LoadItems();
            }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            if (lvValues.Items.Count == 1 && Providers.SalesTaxGroupData.TaxGroupInUse(PluginEntry.DataModel, selectedTaxGroup))
            {
                MessageDialog.Show(Resources.CannotRemoveTaxCode + " " + Resources.AtLeastOneTaxCodeForCustomerStoreSalesTypeTaxGroup);
            }
            else
            {
                Providers.SalesTaxGroupData.RemoveTaxCodeFromSalesTaxGroup(PluginEntry.DataModel, (RecordIdentifier)lvValues.SelectedItems[0].Tag);

                // Only if the affected tax group is the one default store is using
                if (Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel) == selectedTaxGroup)
                {
                    ShowChangePriceDialog();
                }

                LoadLines();
            }
        }

        protected override void OnClose()
        {
            lvGroups.SmallImageList = null;
            lvValues.SmallImageList = null;

            base.OnClose();
        }

        private void ShowChangePriceDialog()
        {
            UpdateItemPricesTaxQuestionDialog dlg = new UpdateItemPricesTaxQuestionDialog();
            if (dlg.Show(PluginEntry.DataModel) == DialogResult.Yes)
            {
                // Update item prices within a progress indicator
                ShowProgress(delegate(System.Object o, System.EventArgs ea)
                    {
                        var defaultStoreID = Providers.StoreData.GetDefaultStoreID(PluginEntry.DataModel);

                        int updatedItemsCount;
                        int updatedTradeAgreementsCount;
                        int updatedPromotionOfferLinesCount;

                        Services.Interfaces.Services.TaxService(PluginEntry.DataModel).UpdatePrices(
                            PluginEntry.DataModel,
                            new RecordIdentifier(defaultStoreID,selectedTaxGroup),
                            UpdateItemTaxPricesEnum.DefaultStoreTaxGroup,
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
    }
}
