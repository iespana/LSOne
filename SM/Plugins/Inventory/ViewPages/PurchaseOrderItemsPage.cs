using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Expressions;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Dialogs;
using LSOne.ViewPlugins.Inventory.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class PurchaseOrderItemsPage : UserControl, ITabView
    {
        private static Guid BarSettingID = new Guid("1A53C2AE-F175-476B-A687-6FB76D34B891");

        RecordIdentifier purchaseOrderID;
        PurchaseOrder purchaseOrder;
        private Setting searchBarSetting;

        PurchaseStatusEnum purchaseOrderStatus;

        private SiteServiceProfile siteServiceProfile;
        private PurchaseOrderLineSearch searchCriteria;
        private List<PurchaseOrderLine> purchaseOrderLines;

        private ExtendedMenuItem viewItemMenuItem;

        private bool canEditImages = false;
        private bool canDeleteImages = false;

        private PurchaseOrderItemsPage()
        {
            InitializeComponent();

            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += lvItems_Opening;

            purchaseOrderLines = new List<PurchaseOrderLine>();

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders);

            searchBar1.BuddyControl = lvItems;
            searchBar1.FocusFirstInput();

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;

            siteServiceProfile = PluginOperations.GetSiteServiceProfile();

            canEditImages = PluginEntry.Framework.CanRunOperation("AddEditImage");
            canDeleteImages = PluginEntry.Framework.CanRunOperation("DeleteImage");

            viewItemMenuItem = null;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new PurchaseOrderItemsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            purchaseOrderID = context;
            purchaseOrder = (PurchaseOrder)internalContext;

            purchaseOrderStatus = purchaseOrder.PurchaseStatus;

            LoadItems(null, true);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PurchaseOrderStatus" && changeIdentifier.PrimaryID == purchaseOrderID)
            {
                purchaseOrderStatus = (PurchaseStatusEnum)param;
                purchaseOrderStatusChanged();
                PluginEntry.Framework.SetDashboardItemDirty(PluginEntry.PurchaseOrderDashBoardItemID);
            }
            else if (objectName == "PurchaseOrder" && changeIdentifier == purchaseOrderID)
            {
                //PurchaseOrder changedOrder = ((PurchaseOrder)param);
            }
            else if (objectName == "PurchaseOrderTaxSettings")
            {
                LoadItems(null, true);
            }
            else if (objectName == "PurchaseOrderChangeDiscount" && changeIdentifier == purchaseOrderID)
            {
                LoadItems(null, true);
            }
            else if (objectName == "VendorChanged" && changeIdentifier == purchaseOrder.VendorID)
            {
                LoadItems(null, true);
            }
            else if (objectName == "PurchaseOrderLine")
            {
                LoadItems(null, true);
            }
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void purchaseOrderStatusChanged()
        {
            //This done to enable or disable the normal add and edit buttons
            lvItems_SelectionChanged(this, EventArgs.Empty);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PurchaseOrderLine purchaseOrderLine = (PurchaseOrderLine)lvItems.Selection[0].Tag;

            if (purchaseOrderLine == null)
            {
                return;
            }

            PurchaseOrderLineDialog dlg = new PurchaseOrderLineDialog(purchaseOrder, purchaseOrderLine);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(null, false);
            }
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PurchaseOrderLineDialog dlg = new PurchaseOrderLineDialog(purchaseOrder);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(null, false);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PurchaseOrderLine purchaseOrderLine = (PurchaseOrderLine)lvItems.Selection[0].Tag;

            if (purchaseOrderLine == null)
            {
                return;
            }

            List<PurchaseOrderLine> selectedLines = GetSelectedLines();

            List<PurchaseOrderLine> notDeleted = PluginOperations.DeletePurchaseOrderLine(selectedLines);

            LoadItems(null, false, notDeleted);

            try
            {
                purchaseOrder.HasLines = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).PurchaseOrderHasPurchaseOrderLines(PluginEntry.DataModel, siteServiceProfile, purchaseOrder.PurchaseOrderID, true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrder", purchaseOrder.PurchaseOrderID, purchaseOrder);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }

        }

        private List<PurchaseOrderLine> GetSelectedLines()
        {
            List<PurchaseOrderLine> selection = new List<PurchaseOrderLine>();
            for (int i = 0; i < lvItems.Selection.Count; i++)
            {
                PurchaseOrderLine line = (PurchaseOrderLine)lvItems.Selection[i].Tag;
                selection.Add(line);
            }

            return selection;
        }

        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnEdit_Click);

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnsEditAddRemove_AddButtonClicked);

            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsEditAddRemove.AddButtonEnabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnRemove_Click);

            if(viewItemMenuItem != null)
			{
                viewItemMenuItem.Enabled = lvItems.Selection.Count > 0;
                menu.Items.Add(viewItemMenuItem);
			}

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            if (canDeleteImages)
            {
                menu.Items.Add(new ExtendedMenuItem("-", 600));

                item = new ExtendedMenuItem(Resources.DeleteImages, 650, DeleteImageHandler);
                item.Enabled = btnsEditAddRemove.RemoveButtonEnabled && LinesWithImagesSelected();
                menu.Items.Add(item);
            }

            PluginEntry.Framework.ContextMenuNotify("PurchaseOrderLines", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private PurchaseOrderLineSearch GetSearchBarResults()
        {
            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            searchCriteria = new PurchaseOrderLineSearch();

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        searchCriteria.ItemNameSearch = result.StringValue.Tokenize();
                        searchCriteria.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Quantity":
                        searchCriteria.Quantity = (decimal)result.DoubleValue;
                        searchCriteria.QuantityOperator = result.SearchModification == SearchParameterResult.SearchModificationEnum.Equals ? DoubleValueOperator.Equals :
                                                          result.SearchModification == SearchParameterResult.SearchModificationEnum.GreaterThan ? DoubleValueOperator.GreaterThan : DoubleValueOperator.LessThan;
                        break;
                    case "HasDiscount":
                        if (result.CheckedValues[0] && result.CheckedValues[1])
                        {
                            searchCriteria.HasDiscount = null;
                        }
                        else if (result.CheckedValues[0] && !result.CheckedValues[1])
                        {
                            searchCriteria.HasDiscount = true;
                        }
                        else if (!result.CheckedValues[0] && result.CheckedValues[1])
                        {
                            searchCriteria.HasDiscount = false;
                        }
                        break;
                    case "Variant":
                        searchCriteria.VariantSearch = result.StringValue.Tokenize();
                        searchCriteria.VariantSearchBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                }
            }

            return searchCriteria;
        }

        private void LoadItems(PurchaseOrderLineSearch searchCriteria, bool resetPages)
        {
            LoadItems(searchCriteria, resetPages, new List<PurchaseOrderLine>());
        }

        public void AddOpenInItemViewMenuItem(ExtendedMenuItem item)
		{
            viewItemMenuItem = item;
        }

        public List<IDataEntity> GetSelectedItems()
        {
            var res = new List<IDataEntity>();
            for (int i = 0; i < lvItems.Selection.Count; i++)
            {
                PurchaseOrderLine pol = (PurchaseOrderLine)lvItems.Selection[i].Tag;
                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, pol.ItemID);
                res.Add(item);
                int salesUnitQuantity = (int)Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, item.ID, item.PurchaseUnitID, item.SalesUnitID, pol.Quantity);
                res[i].ID.SecondaryID = salesUnitQuantity;
            }
            return res;
        }

        private void LoadItems(PurchaseOrderLineSearch searchCriteria, bool resetPages, List<PurchaseOrderLine> toSelect)
        {
            try
            {
                if (resetPages)
                {
                    itemDataScroll.Reset();
                }

                if (searchCriteria == null)
                {
                    searchCriteria = GetSearchBarResults();
                }

                searchCriteria.PurchaseOrderID = purchaseOrder.PurchaseOrderID;
                searchCriteria.StartRecord = itemDataScroll.StartRecord;
                searchCriteria.EndRecord = itemDataScroll.EndRecord + 1;
                searchCriteria.ShowDeleted = true;
                int totalRecordsMatching = 0;

                try
                {
                    purchaseOrderLines = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetPurchaseOrderLines(
                    PluginEntry.DataModel,
                    siteServiceProfile,
                    searchCriteria,
                    PurchaseOrderLineSorting.ItemName,
                    false,
                    out totalRecordsMatching,
                    true);
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                    return;
                }

                LoadListView(purchaseOrderLines, toSelect, totalRecordsMatching);
            }
            finally
            {
                ((ViewBase)Parent.Parent.Parent).HideProgress();
            }
        }

        private void LoadListView(List<PurchaseOrderLine> purchaseOrderLines, List<PurchaseOrderLine> toSelect, int totalRecords)
        {
            Style strikeThroughStyle = new Style(lvItems.DefaultStyle);
            strikeThroughStyle.Font = new Font(strikeThroughStyle.Font, FontStyle.Strikeout);

            lvItems.ClearRows();

            itemDataScroll.RefreshState(purchaseOrderLines, totalRecords);

            Row row;
            int count = 0;

            DecimalLimit priceLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            DecimalLimit percentageLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

            foreach (PurchaseOrderLine purchaseOrderLine in purchaseOrderLines)
            {
                DecimalLimit quantityLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, purchaseOrderLine.UnitName), CacheType.CacheTypeApplicationLifeTime);
                bool itemInverntoryExcluded = purchaseOrderLine.ItemInventoryExcluded;

                row = new Row();
                purchaseOrderLine.SetReportFormatting(priceLimit, percentageLimit, quantityLimit);

                //If the item ID and vendor item id are the same then there is no vendor item ID
                if (purchaseOrderLine.VendorItemID.Equals(purchaseOrderLine.ItemID, StringComparison.InvariantCultureIgnoreCase))
                {
                    row.AddCell(new Cell(purchaseOrderLine.ItemID, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle));
                }
                else
                {
                    Style style = new Style(lvItems.DefaultStyle);
                    if (itemInverntoryExcluded)
                    {
                        style.Font = new Font(strikeThroughStyle.Font, FontStyle.Strikeout);
                    }
                    else
                    {
                        style.Font = lvItems.Font;
                    }
                    style.TextColor = ColorPalette.Violet;

                    row.AddCell(new Cell(purchaseOrderLine.VendorItemID, style));
                }

                decimal calculatedDiscount = purchaseOrderLine.CalculatedDiscount();


                row.AddCell(new Cell(purchaseOrderLine.ItemName, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle));
                row.AddCell(new Cell(purchaseOrderLine.VariantName, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle));
                row.AddCell(new NumericCell(purchaseOrderLine.FormattedQuantity + " " + purchaseOrderLine.UnitName, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle, purchaseOrderLine.Quantity));
                row.AddCell(new NumericCell(purchaseOrderLine.FormattedUnitPrice, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle, purchaseOrderLine.UnitPrice));
                row.AddCell(new NumericCell(purchaseOrderLine.FormattedTaxAmount, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle, purchaseOrderLine.TaxAmount));

                if (purchaseOrderLine.CalculatedDiscount() < decimal.Zero)
                {
                    Style style = new Style(lvItems.DefaultStyle);
                    if (itemInverntoryExcluded)
                    {
                        style.Font = new Font(strikeThroughStyle.Font, FontStyle.Strikeout);
                    }
                    else
                    {
                        style.Font = lvItems.Font;
                    }
                    style.TextColor = ColorPalette.NegativeNumber;

                    row.AddCell(new NumericCell(purchaseOrderLine.FormattedCalculatedDiscount, style, purchaseOrderLine.CalculatedDiscount()));
                }
                else
                {
                    row.AddText("");
                }

                row.AddCell(new NumericCell(purchaseOrderLine.FormattedFinalUnitPrice, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle, purchaseOrderLine.FinalUnitPrice()));
                row.AddCell(new NumericCell(purchaseOrderLine.FormattedDiscountAmount, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle, purchaseOrderLine.DiscountAmount));
                row.AddCell(new NumericCell(purchaseOrderLine.FormattedDiscountPercentage, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle, purchaseOrderLine.DiscountPercentage));
                row.AddCell(new Cell(
                    purchaseOrderLine.TaxCalculationMethod == TaxCalculationMethodEnum.IncludeTax ?
                    Resources.UnitPriceIncludesTax :
                        purchaseOrderLine.TaxCalculationMethod == TaxCalculationMethodEnum.AddTax ?
                        Resources.UnitPriceAddTax :
                        Resources.NoTax, itemInverntoryExcluded ? strikeThroughStyle : lvItems.DefaultStyle));

                if (!RecordIdentifier.IsEmptyOrNull(purchaseOrderLine.PictureID) && canEditImages)
                {
                    IconButton button = new IconButton(Resources.Camera24, Properties.Resources.EditImage);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter, ""));
                }

                row.Tag = purchaseOrderLine;

                lvItems.AddRow(row);

                if (toSelect.Any() && toSelect.FirstOrDefault(f => f.ID.PrimaryID == purchaseOrderLine.ID.PrimaryID && f.ID.SecondaryID == purchaseOrderLine.ID.SecondaryID) != null)
                {
                    lvItems.Selection.AddRows(count, count);
                }
                count++;
            }

            lvItems.AutoSizeColumns();

            lvItems_SelectionChanged(this, EventArgs.Empty);
        }

        private void cmbItem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                // Check if the entered value is a barcode representing an item
                e.Handled = true;
            }
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            var expresion = (lvItems.Selection.Count != 0)
                && purchaseOrderStatus == PurchaseStatusEnum.Open
                && PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders);

            btnsEditAddRemove.EditButtonEnabled =
                expresion
                && !(lvItems.Selection[0].Tag as PurchaseOrderLine).ItemInventoryExcluded
                && lvItems.Selection.Count == 1;
            btnsEditAddRemove.RemoveButtonEnabled = expresion;

            btnsEditAddRemove.AddButtonEnabled = (purchaseOrderStatus == PurchaseStatusEnum.Open) && PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders);
        }

        private void lvItems_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (lvItems.Selection.Count != 0 && PluginEntry.DataModel.HasPermission(Permission.ManagePurchaseOrders) && btnsEditAddRemove.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_Variant, "Variant", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_Quantity, "Quantity", ConditionType.ConditionTypeEnum.Numeric));
            searchBar1.AddCondition(new ConditionType(Resources.SearchBar_HasDiscount, "HasDiscount", ConditionType.ConditionTypeEnum.Checkboxes,
                Resources.SearchBar_HasDiscount_Yes, false,
                Resources.SearchBar_HasDiscount_No, false));

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

            ((ViewBase)Parent.Parent.Parent).ShowTimedProgress(searchBar1.GetLocalizedSavingText());

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
            if (Parent == null)
            {
                return;
            }

            itemDataScroll.Reset();
            ((ViewBase)Parent.Parent.Parent).ShowProgress((sender1, e1) => LoadItems(null, true), ((ViewBase)Parent.Parent.Parent).GetLocalizedSearchingText());
        }

        private void itemDataScroll_PageChanged(object sender, EventArgs e)
        {
            if (Parent == null)
            {
                return;
            }

            ((ViewBase)Parent.Parent.Parent).ShowProgress((sender1, e1) => LoadItems(null, false), ((ViewBase)Parent.Parent.Parent).GetLocalizedSearchingText());
        }

        /// <summary>
        /// Checks if any of the selected lines has a picture ID
        /// </summary>
        /// <returns></returns>
        private bool LinesWithImagesSelected()
        {
            if (lvItems.Selection.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < lvItems.Selection.Count; i++)
            {
                if (!RecordIdentifier.IsEmptyOrNull(((PurchaseOrderLine)lvItems.Selection[i].Tag).PictureID))
                {
                    return true;
                }
            }

            return false;
        }

        private void DeleteImageHandler(object sender, EventArgs args)
        {
            List<PurchaseOrderLine> modifiedPurchaseOrderLines = new List<PurchaseOrderLine>();

            if (lvItems.Selection.Count == 1)
            {
                // We don't have to check for an image here since the "Delete images" button is only enabled for a single line if it has a picture ID
                if (QuestionDialog.Show(Properties.Resources.DeleteImageQuestion) == DialogResult.Yes)
                {
                    PurchaseOrderLine purchaseOrderLine = ((PurchaseOrderLine)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag);
                    modifiedPurchaseOrderLines.Add(purchaseOrderLine);

                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).DeleteImage(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), purchaseOrderLine.PictureID, false);
                }
            }
            else
            {
                List<PurchaseOrderLine> purchaseOrderLinesWithPictureIDs= new List<PurchaseOrderLine>();

                for (int i = 0; i < lvItems.Selection.Count; i++)
                {
                    PurchaseOrderLine purchaseOrderLine= ((PurchaseOrderLine)lvItems.Selection[i].Tag);

                    if (!RecordIdentifier.IsEmptyOrNull(purchaseOrderLine.PictureID))
                    {
                        purchaseOrderLinesWithPictureIDs.Add(purchaseOrderLine);
                    }
                }

                if (QuestionDialog.Show(Properties.Resources.DeleteImagesQuestion) == DialogResult.Yes)
                {
                    var pictureIDs = from purchaseOrderLine in purchaseOrderLinesWithPictureIDs select purchaseOrderLine.PictureID;
                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).DeleteImageList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), pictureIDs.ToList(), false);

                    modifiedPurchaseOrderLines = purchaseOrderLinesWithPictureIDs;
                }
            }

            int totalCount = modifiedPurchaseOrderLines.Count;
            using (ProgressDialog dlg = new ProgressDialog(Resources.SavingPurchaseOrderLines, Resources.SavingCounter, totalCount))
            {
                Action saveAction = () =>
                {
                    int count = 1;

                    foreach (PurchaseOrderLine purchaseOrderLine in modifiedPurchaseOrderLines)
                    {
                        purchaseOrderLine.PictureID = RecordIdentifier.Empty;
                        Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).SavePurchaseOrderLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), purchaseOrderLine, false);

                        dlg.Report(count, totalCount);
                        count++;
                    }

                    Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel).Disconnect(PluginEntry.DataModel);
                };

                dlg.ProgressTask = Task.Run(saveAction);
                dlg.ShowDialog(PluginEntry.Framework.MainWindow);
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrderLine", purchaseOrder.PurchaseOrderID, purchaseOrder);
        }

        private void LvItems_CellAction(object sender, CellEventArgs args)
        {
            if (args.Cell is IconButtonCell)
            {
                RecordIdentifier pictureID = ((PurchaseOrderLine)lvItems.Row(args.RowNumber).Tag).PictureID;

                if (!Providers.ImageData.Exists(PluginEntry.DataModel, pictureID))
                {
                    MessageDialog.Show(Properties.Resources.CannotEditImageLocally);
                    return;
                }

                PluginEntry.Framework.RunOperation("AddEditImage", this, new ViewCore.EventArguments.PluginOperationArguments(pictureID, null));
            }
        }
    }
}
