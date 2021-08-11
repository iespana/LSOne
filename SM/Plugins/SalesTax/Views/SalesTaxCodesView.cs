using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.SalesTax.Views
{
    public partial class SalesTaxCodesView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public SalesTaxCodesView(RecordIdentifier id)
            :this()
        {
            selectedID = id;
        }

        public SalesTaxCodesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Audit;

            lvGroups.SmallImageList = PluginEntry.Framework.GetImageList();
            lvValues.SmallImageList = PluginEntry.Framework.GetImageList();

            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += lvGroups_Opening;

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);

            btnsContextButtons.AddButtonEnabled = btnsContextButtonsItems.AddButtonEnabled = !ReadOnly;

            lvValues.Columns[0].Tag = TaxCodeValue.SortEnum.FromDate;
            lvValues.Columns[1].Tag = TaxCodeValue.SortEnum.ToDate;
            lvValues.Columns[2].Tag = TaxCodeValue.SortEnum.Value;

            lvGroups.Columns[0].Tag = TaxCode.SortEnum.SalesTaxCode;
            lvGroups.Columns[1].Tag = TaxCode.SortEnum.Description;
            lvGroups.Columns[2].Tag = TaxCode.SortEnum.RoundOff;
            lvGroups.Columns[3].Tag = TaxCode.SortEnum.RoundingType;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("TaxCodes", RecordIdentifier.Empty, Properties.Resources.SalesTaxCodes, false));
            contexts.Add(new AuditDescriptor("TaxCodesLines", RecordIdentifier.Empty, Properties.Resources.SalesTaxCodeValues, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.SalesTaxCodes;
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

            LoadItems(0, false, true, selectedID);
        }

        protected override void LoadData(bool isRevert)
        {
            HeaderText = Properties.Resources.SalesTaxCodes;
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
          
        }

        private void LoadItems(int sortBy, bool backwards, bool doBestFit, RecordIdentifier idToSelect)
        {
            List<TaxCode> codes;
            ListViewItem listItem;
            
            lvGroups.Items.Clear();

            codes = Providers.TaxCodeData.GetTaxCodes(PluginEntry.DataModel, (TaxCode.SortEnum)lvGroups.Columns[lvGroups.SortColumn].Tag, lvGroups.SortedBackwards);

            foreach (TaxCode code in codes)
            {
                listItem = new ListViewItem((string)code.ID);
                listItem.SubItems.Add(code.Text);
                listItem.SubItems.Add(code.TaxRoundOff.FormatTruncated());

                listItem.SubItems.Add(code.TaxRoundOffTypeFormatted); 

                listItem.Tag = code.ID;
                listItem.ImageIndex = -1;

                lvGroups.Add(listItem);

                if (idToSelect == (RecordIdentifier)listItem.Tag)
                {
                    listItem.Selected = true;
                }
            }

            lvGroups.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvGroups.SortColumn = sortBy;

            lvGroups_SelectedIndexChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvGroups.BestFitColumns();
            }
        }

        private void LoadLines(int sortBy,bool backwards,bool doBestFit)
        {
            List<TaxCodeValue> lines;
            ListViewItem listItem;

            if (lvGroups.SelectedItems.Count == 0)
            {
                return;
            }

            lvValues.Items.Clear();

            lines = Providers.TaxCodeValueData.GetTaxCodeValues(PluginEntry.DataModel, SelectedTaxCode, (TaxCodeValue.SortEnum)lvValues.Columns[lvValues.SortColumn].Tag, lvValues.SortedBackwards);

            foreach (TaxCodeValue line in lines)
            {
                listItem = new ListViewItem(line.FromDate.ToShortDateString());
                listItem.SubItems.Add(line.ToDate.ToShortDateString());
                listItem.SubItems.Add(line.Value.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Tax)) + " %"); 

                listItem.Tag = line.ID;
                listItem.ImageIndex = -1;

                lvValues.Add(listItem);
            }

            lvValues.Columns[sortBy].ImageIndex = (backwards ? 1 : 0);
            lvValues.SortColumn = sortBy;

            if (doBestFit)
            {
                lvValues.BestFitColumns();
            }

            lvValues_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private RecordIdentifier SelectedTaxCode
        {
            get { return (RecordIdentifier)lvGroups.SelectedItems[0].Tag; }
        }

        private void lvGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvGroups.SelectedItems.Count > 0) ? (RecordIdentifier)lvGroups.SelectedItems[0].Tag : "";
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = 
                (lvGroups.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);

            if (lvGroups.SelectedItems.Count > 0)
            {
                if (lblGroupHeader.Visible == false)
                {
                    lblGroupHeader.Visible = true;
                    lvValues.Visible = true;
                    btnsContextButtonsItems.Visible = true;
                    lblNoSelection.Visible = false;
                }

                LoadLines(lvValues.SortColumn,lvValues.SortedBackwards,true);
            }
            else if (lblGroupHeader.Visible == true)
            {
                lblGroupHeader.Visible = false;
                lvValues.Visible = false;
                btnsContextButtonsItems.Visible = false;
                lblNoSelection.Visible = true;
            }
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
                }
                lvGroups.SortedBackwards = false;
            }

            LoadItems(e.Column, lvGroups.SortedBackwards, false, selectedID);
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
                }
                lvValues.SortedBackwards = false;
            }

            LoadLines(e.Column, lvValues.SortedBackwards,true);
        }

        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvGroups.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditText + "...",
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("SalesTaxCodesList", lvGroups.ContextMenuStrip, lvGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lvValues_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditText + "...",
                    100,
                    btnEditValue_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtonsItems.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    btnAddValue_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtonsItems.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    btnRemoveValue_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtonsItems.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("SalesTaxCodesLineList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.SalesTax.Views.SalesTaxCodesView.Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.ItemSalesTaxGroup, new ContextbarClickEventHandler(PluginOperations.ShowItemSalesTaxGroupView)),10);
                arguments.Add(new ContextBarItem(Properties.Resources.SalesTaxGroup, new ContextbarClickEventHandler(PluginOperations.ShowSalesTaxGroupView)), 20);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            RecordIdentifier id = SelectedTaxCode;

            Dialogs.EditTaxCodeDialog dlg = new Dialogs.EditTaxCodeDialog(id);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(lvGroups.SortColumn, lvGroups.SortedBackwards, true, id);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Dialogs.NewTaxCodeDialog dlg = new Dialogs.NewTaxCodeDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(lvGroups.SortColumn, lvGroups.SortedBackwards, true, dlg.TaxCodeID);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteSalesTaxCodeQuestion,
                Properties.Resources.DeleteSalesTaxCode) == DialogResult.Yes)
            {
                var defaultStoresTaxGroupID = Providers.StoreData.GetDefaultStoreSalesTaxGroup(PluginEntry.DataModel);
                var taxCodesInDefaultStore = Providers.SalesTaxGroupData.GetTaxCodesInSalesTaxGroupList(PluginEntry.DataModel, defaultStoresTaxGroupID, true);

                Providers.TaxCodeData.Delete(PluginEntry.DataModel, SelectedTaxCode);

                // We only want to call the change price dialog if the deleted tax code was in the default stores tax group. And because 
                // we just deleted the tax code we can't know which items had the tax code so we are forced to call using all items with tax group
                if (taxCodesInDefaultStore.Where(x => x.ID == SelectedTaxCode).Count() > 0)
                {
                    ShowChangePriceDialog(UpdateItemTaxPricesEnum.AllItems, RecordIdentifier.Empty);    
                }

                LoadItems(lvGroups.SortColumn, lvGroups.SortedBackwards, true, null);
            }
        }

        private void ShowChangePriceDialog(UpdateItemTaxPricesEnum updateItemTaxPricesEnum, RecordIdentifier selectedTaxCode)
        {
            UpdateItemPricesTaxQuestionDialog dlg = new UpdateItemPricesTaxQuestionDialog();
            if (dlg.Show(PluginEntry.DataModel) == DialogResult.Yes)
            {
                // Update item prices within a progress indicator
                ShowProgress(delegate(System.Object o, System.EventArgs ea)
                    {
                        int updatedItemsCount;
                        int updatedTradeAgreementsCount;
                        int updatedPromotionOfferLinesCount;

                        Services.Interfaces.Services.TaxService(PluginEntry.DataModel).UpdatePrices(
                            PluginEntry.DataModel,
                            selectedTaxCode,
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

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            Dialogs.ValueInTaxCodeDialog dlg = new Dialogs.ValueInTaxCodeDialog(SelectedTaxCode,(RecordIdentifier)lvValues.SelectedItems[0].Tag);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (Providers.SalesTaxGroupData.TaxCodeIsInDefaultStoreTaxGroup(PluginEntry.DataModel, SelectedTaxCode))
                {
                    ShowChangePriceDialog(UpdateItemTaxPricesEnum.TaxCode, SelectedTaxCode);
                }

                LoadLines(lvValues.SortColumn, lvValues.SortedBackwards, true);
            }
        }

        private void btnAddValue_Click(object sender, EventArgs e)
        {
            Dialogs.ValueInTaxCodeDialog dlg = new Dialogs.ValueInTaxCodeDialog(SelectedTaxCode);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (Providers.SalesTaxGroupData.TaxCodeIsInDefaultStoreTaxGroup(PluginEntry.DataModel, SelectedTaxCode))
                {
                    ShowChangePriceDialog(UpdateItemTaxPricesEnum.TaxCode, SelectedTaxCode);
                }
                
                LoadLines(lvValues.SortColumn, lvValues.SortedBackwards, true);
            }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteSalesTaxCodeValueQuestion,
                Properties.Resources.DeleteSalesTaxCodeValue) == DialogResult.Yes)
            {
                Providers.TaxCodeValueData.Delete(PluginEntry.DataModel, (RecordIdentifier)lvValues.SelectedItems[0].Tag);

                if (Providers.SalesTaxGroupData.TaxCodeIsInDefaultStoreTaxGroup(PluginEntry.DataModel, SelectedTaxCode))
                {
                    ShowChangePriceDialog(UpdateItemTaxPricesEnum.TaxCode, SelectedTaxCode);
                }

                LoadLines(lvValues.SortColumn, lvValues.SortedBackwards, true);
            }
        }

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

        private void lvValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtonsItems.RemoveButtonEnabled = btnsContextButtonsItems.EditButtonEnabled = 
                lvValues.SelectedItems.Count > 0 && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditSalesTaxSetup);
        }

        protected override void OnClose()
        {
            lvGroups.SmallImageList = null;
            lvValues.SmallImageList = null;

            base.OnClose();
        }
    }
}
