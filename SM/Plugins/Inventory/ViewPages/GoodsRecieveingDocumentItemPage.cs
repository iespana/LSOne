using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
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
    public partial class GoodsRecieveingDocumentItemPage : UserControl, ITabView
    {
        private SiteServiceProfile siteServiceProfile;

        IInventoryService service;

        private GoodsReceivingDocument goodsReceivingDocument;

        private ExtendedMenuItem viewItemMenuItem;

        public GoodsRecieveingDocumentItemPage()
        {
            InitializeComponent();


            lvItems.ContextMenuStrip = new ContextMenuStrip();
            lvItems.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            siteServiceProfile = PluginOperations.GetSiteServiceProfile();

            goodsReceivingDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            goodsReceivingDataScroll.Reset();
            searchBar1.BuddyControl = lvItems;

            viewItemMenuItem = null;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new GoodsRecieveingDocumentItemPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            goodsReceivingDocument = (GoodsReceivingDocument)internalContext;
            LoadItems();
        }

        public bool DataIsModified()
        {
            return false;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvItems.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnEdit_Click);

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);
            item = new ExtendedMenuItem(
                 Resources.Add,
                 200,
                 btnAdd_Click);

            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsEditAddRemove.AddButtonEnabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnRemove_Click);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            if (viewItemMenuItem != null)
            {
                viewItemMenuItem.Enabled = lvItems.Selection.Count > 0;
                menu.Items.Add(viewItemMenuItem);
            }

            menu.Items.Add(new ExtendedMenuItem("-", 450));

            item = new ExtendedMenuItem(
                  Resources.PostSelectedItem,
                  500,
                  btnPostLine_Click);


            item.Enabled = btnPostLine.Enabled;

            menu.Items.Add(item);
            item = new ExtendedMenuItem(
                Resources.PostAllLines,
                600,
                btnPostAllLines_Click);

            item.Enabled = btnPostAllLines.Enabled;

            menu.Items.Add(item);
            PluginEntry.Framework.ContextMenuNotify("GoodsReceivingDocument", lvItems.ContextMenuStrip, lvItems);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadItems()
        {
            List<GoodsReceivingDocumentLine> goodsReceivingDocumentLines;

            lvItems.ClearRows();
            GoodsReceivingDocumentLineSearch searchCriteria = new GoodsReceivingDocumentLineSearch();
            searchCriteria.DocumentID = goodsReceivingDocument.ID;

            searchCriteria.RecordsFrom = goodsReceivingDataScroll.StartRecord;
            searchCriteria.RecordsTo = goodsReceivingDataScroll.EndRecord + 1;
            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            GoodsReceivingDocumentLineSorting sort;
            GoodsReceivingDocumentLineSorting?[] columns = new GoodsReceivingDocumentLineSorting?[]
            {
                    GoodsReceivingDocumentLineSorting.ItemID,
                    GoodsReceivingDocumentLineSorting.ItemName,
                    GoodsReceivingDocumentLineSorting.Variant,
                    GoodsReceivingDocumentLineSorting.ReceiveQuantity,
                    GoodsReceivingDocumentLineSorting.ReceiveDate,
                    GoodsReceivingDocumentLineSorting.OrderedQuantity,
                    GoodsReceivingDocumentLineSorting.Posted,
            };
            if (lvItems.SortColumn == null)
            {
                lvItems.SetSortColumn(lvItems.Columns[1], true);
            }

            int sortColumnIndex = lvItems.Columns.IndexOf(lvItems.SortColumn);

            sort = (GoodsReceivingDocumentLineSorting)((int)columns[sortColumnIndex]);

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        searchCriteria.ItemNameSearch = result.StringValue.Tokenize();
                        searchCriteria.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;

                    case "Variant":
                        searchCriteria.VariantSearch = result.StringValue.Tokenize();
                        searchCriteria.VariantSearchBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;

                    case "Status":
                        searchCriteria.Posted = result.CheckedValues[0];
                        break;

                    case "RecievedDate":
                        if (result.Date.Checked && !result.Time.Checked)
                        {
                            searchCriteria.ReceivedFrom = new Date(result.Date.Value);
                        }
                        else if (result.Date.Checked && result.Time.Checked)
                        {
                            searchCriteria.ReceivedFrom = new Date(new DateTime(result.Date.Value.Year, result.Date.Value.Month,
                                result.Date.Value.Day, result.Time.Value.Hour, result.Time.Value.Minute,
                                result.Time.Value.Second));
                        }

                        if (result.DateTo.Checked && !result.TimeTo.Checked)
                        {
                            searchCriteria.ReceivedTo = new Date(result.DateTo.Value.Date.AddDays(1).AddSeconds(-1));
                        }

                        else if (result.DateTo.Checked && result.TimeTo.Checked)
                        {
                            searchCriteria.ReceivedTo = new Date(new DateTime(result.DateTo.Value.Year, result.DateTo.Value.Month,
                                result.DateTo.Value.Day, result.TimeTo.Value.Hour, result.TimeTo.Value.Minute,
                                result.TimeTo.Value.Second));
                        }
                        break;

                    case "RecievedQuantity":
                        searchCriteria.ReceivedQuantity = (decimal)result.DoubleValue;
                        if (result.SearchModification == SearchParameterResult.SearchModificationEnum.Equals)
                        {
                            searchCriteria.ReceivedQuantityOperator = DoubleValueOperator.Equals;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.GreaterThan)
                        {
                            searchCriteria.ReceivedQuantityOperator = DoubleValueOperator.GreaterThan;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.LessThan)
                        {
                            searchCriteria.ReceivedQuantityOperator = DoubleValueOperator.LessThan;
                        }
                        break;

                    case "OrderedQuantity":
                        searchCriteria.OrderedQuantity = (decimal)result.DoubleValue;
                        if (result.SearchModification == SearchParameterResult.SearchModificationEnum.Equals)
                        {
                            searchCriteria.OrderedQuantityOperator = DoubleValueOperator.Equals;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.GreaterThan)
                        {
                            searchCriteria.OrderedQuantityOperator = DoubleValueOperator.GreaterThan;
                        }
                        else if (result.SearchModification == SearchParameterResult.SearchModificationEnum.LessThan)
                        {
                            searchCriteria.OrderedQuantityOperator = DoubleValueOperator.LessThan;
                        }

                        break;
                }
            }
            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            int totalRecords;
            try
            {
                Style strikeThroughStyle = new Style(lvItems.DefaultStyle);
                strikeThroughStyle.Font = new Font(strikeThroughStyle.Font, FontStyle.Strikeout);

                goodsReceivingDocumentLines = service.SearchGoodsReceivingDocumentLines(PluginEntry.DataModel, siteServiceProfile, searchCriteria, sort, !lvItems.SortedAscending, true, out totalRecords);

                goodsReceivingDataScroll.RefreshState(goodsReceivingDocumentLines, totalRecords);
                Row row;
                Style rowStyle;
                Style receivingCellStyle;
                foreach (GoodsReceivingDocumentLine goodsReceivingDocumentLine in goodsReceivingDocumentLines)
                {
                    rowStyle = goodsReceivingDocumentLine.purchaseOrderLine.ItemInventoryExcluded
                                    ? strikeThroughStyle
                                    : lvItems.DefaultStyle;

                    receivingCellStyle = new Style(rowStyle);
                    var receivingDiff = goodsReceivingDocumentLine.ReceivedQuantity
                                            - goodsReceivingDocumentLine.purchaseOrderLine.Quantity;
                    if (receivingDiff != 0)
                    {
                        receivingCellStyle.TextColor = receivingDiff > 0 ? ColorPalette.GreenLight : ColorPalette.RedLight;
                    }

                    string unitString = goodsReceivingDocumentLine.purchaseOrderLine.UnitName;

                    string orderedQtyWithUnit =
                        goodsReceivingDocumentLine.purchaseOrderLine.FormattedQuantity + " " + unitString;

                    string receivedQuantityWithUnit =
                        goodsReceivingDocumentLine.FormatedReceivedQuantity + " " + unitString;

                    row = new Row();
                    if (goodsReceivingDocumentLine.purchaseOrderLine.VendorItemID.Equals(goodsReceivingDocumentLine.purchaseOrderLine.ItemID, StringComparison.InvariantCultureIgnoreCase))
                    {
                        row.AddCell(new Cell(goodsReceivingDocumentLine.purchaseOrderLine.ItemID,
                                             rowStyle));
                    }
                    else
                    {
                        Style style = new Style(rowStyle);
                        style.TextColor = ColorPalette.Violet;

                        row.AddCell(new Cell(goodsReceivingDocumentLine.purchaseOrderLine.VendorItemID, style));
                    }
                    row.AddCell(new Cell(goodsReceivingDocumentLine.purchaseOrderLine.ItemName,
                                        rowStyle));
                    row.AddCell(new Cell(goodsReceivingDocumentLine.purchaseOrderLine.VariantName,
                                        rowStyle));
                    row.AddCell(new NumericCell(receivedQuantityWithUnit,
                                                receivingCellStyle, 
                                                goodsReceivingDocumentLine.ReceivedQuantity));
                    row.AddCell(new DateTimeCell(goodsReceivingDocumentLine.ReceivedDate.ToShortDateString(), 
                                                goodsReceivingDocumentLine.ReceivedDate,
                                                rowStyle));
                    row.AddCell(new NumericCell(orderedQtyWithUnit,
                                                rowStyle, 
                                                goodsReceivingDocumentLine.purchaseOrderLine.Quantity));
                    row.AddCell(new Cell((goodsReceivingDocumentLine.Posted) ? Resources.Yes : Resources.No,
                                          rowStyle));
                    row.Tag = goodsReceivingDocumentLine;

                    lvItems.AddRow(row);
                }

                btnPostAllLines.Enabled = goodsReceivingDocumentLines.Any(x => !x.Posted);

                lvItems.AutoSizeColumns();
                
                lvItems_SelectionChanged(this, EventArgs.Empty);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.None, "GoodsReceivingDocumentRefreshSearch", goodsReceivingDocument.ID, null);

            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvItems.Selection.Count == 1)
            {
                GoodsReceivingDocumentLine goodsReceivingDocumentLine = (GoodsReceivingDocumentLine)lvItems.Selection[0].Tag;

                GoodsReceivingDocumentLineDialog dlg = new GoodsReceivingDocumentLineDialog(goodsReceivingDocument, goodsReceivingDocumentLine);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    LoadItems();
                }
            }
            else
            {
                List<GoodsReceivingDocumentLine> goodsReceivingDocumentLines = new List<GoodsReceivingDocumentLine>();

                for (int i = 0; i < lvItems.Selection.Count; i++)
                {
                    GoodsReceivingDocumentLine currentLine = ((GoodsReceivingDocumentLine)lvItems.Row(lvItems.Selection.GetRowIndex(i)).Tag);
                    if (!currentLine.Posted)
                    {
                        goodsReceivingDocumentLines.Add(currentLine);
                    }
                }

                if (goodsReceivingDocumentLines.Count == 1)
                {
                    GoodsReceivingDocumentLine goodsReceivingDocumentLine = goodsReceivingDocumentLines[0];
                    GoodsReceivingDocumentLineDialog dlg = new GoodsReceivingDocumentLineDialog(goodsReceivingDocument, goodsReceivingDocumentLine);

                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        LoadItems();
                    }
                }
                else
                {
                    GoodsRecieveingLineDialogMulti dlg = new GoodsRecieveingLineDialogMulti(goodsReceivingDocument.ID, goodsReceivingDocumentLines);
                    dlg.ShowDialog();
                    LoadItems();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            GoodsReceivingDocumentLineDialog dlg = new GoodsReceivingDocumentLineDialog(goodsReceivingDocument);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Enable, "GoodsReceivingDocumentLine", goodsReceivingDocument.ID, null);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            bool continueRemoving = false;
            if (lvItems.Selection.Count > 1)
            {
                continueRemoving = QuestionDialog.Show(Resources.SureToRemoveMultipleItems, Resources.RemoveSelectedLine) == DialogResult.Yes;
            }
            else if (lvItems.Selection.Count == 1)
            {
                continueRemoving = QuestionDialog.Show(Resources.SureToRemoveItem, Resources.RemoveSelectedLine) == DialogResult.Yes;
            }

            if (continueRemoving)
            {
                service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                while (lvItems.Selection.Count > 0)
                {
                    GoodsReceivingDocumentLine grdl = ((GoodsReceivingDocumentLine)lvItems.Row(lvItems.Selection.FirstSelectedRow).Tag);

                    if (grdl.Posted == false)
                    {
                        try
                        {
                            service.DeleteGoodsReceivingDocumentLine(PluginEntry.DataModel, siteServiceProfile, grdl.ID, lvItems.Selection.Count == 1);
                        }
                        catch (Exception ex)
                        {
                            MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                            return;
                        }
                    }
                    lvItems.RemoveRow(lvItems.Selection.FirstSelectedRow);
                }

                if (lvItems.RowCount == 0)
                {
                    goodsReceivingDataScroll.ScrollOneLeft();
                }
            }
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
                case "GoodsReceivingDocument":
                    if (changeHint == DataEntityChangeType.Edit && changeIdentifier == goodsReceivingDocument.ID)
                    {
                        goodsReceivingDocument = (GoodsReceivingDocument)param;
                        if (goodsReceivingDocument.Status == GoodsReceivingStatusEnum.Posted)
                        {
                            btnsEditAddRemove.AddButtonEnabled = false;
                            btnsEditAddRemove.EditButtonEnabled = false;
                            btnsEditAddRemove.RemoveButtonEnabled = false;
                        }
                    }
                    break;
                case "GoodsReceivingDocumentLine":
                    LoadItems();
                    break;
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion
        private RecordIdentifier SelectedLineID
        {
            get
            {
                return ((GoodsReceivingDocumentLine)lvItems.Selection[0].Tag).ID;
            }
        }

        private void lvItems_SelectionChanged(object sender, EventArgs e)
        {
            bool lineIsPosted = false;
            if (lvItems.Selection.Count != 0)
            {
                RecordIdentifier goodsReceivingDocumentLineID = SelectedLineID;
                try
                {
                    GoodsReceivingDocumentLine grdl = service.GetGoodsReceivingDocumentLine(PluginEntry.DataModel,
                        siteServiceProfile, goodsReceivingDocumentLineID, PluginEntry.DataModel.CurrentStoreID, true);

                    lineIsPosted = grdl.Posted;
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                }
            }

            bool expression = goodsReceivingDocument.Status == GoodsReceivingStatusEnum.Active &&
                (lvItems.Selection.Count != 0) &&
                !lineIsPosted &&
                PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments);

            btnsEditAddRemove.EditButtonEnabled = 
               expression &&
                !(lvItems.Selection[0].Tag as GoodsReceivingDocumentLine).purchaseOrderLine.ItemInventoryExcluded;
            btnsEditAddRemove.RemoveButtonEnabled = expression;

            btnsEditAddRemove.AddButtonEnabled = !service.GoodsReceivingDocumentFullyReceived(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.ID, true);

            btnPostLine.Enabled = btnsEditAddRemove.EditButtonEnabled;
        }

        // Note: if saving algorithm for GRD line changes, that change must be applied to Applications\Integration Tests\TestRunner.Actions\RunCreateGoodsReceivingDocument()
        private void PostSingleGRDItem(RecordIdentifier goodsReceivingDocumentLineID)
        {
            try
            {
                GoodsReceivingDocumentLine grdLine = service.GetGoodsReceivingDocumentLine(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocumentLineID, goodsReceivingDocument.StoreID, true);

                DataEntity itemDataEntity = new DataEntity(grdLine.purchaseOrderLine.ItemID, grdLine.purchaseOrderLine.ItemName);
                string unitId = grdLine.purchaseOrderLine.UnitID;

                // Check if a unit conversion rule exists between the selected unit and the item's inventory unit
                bool unitConversionExists = PluginOperations.UnitConversionWithInventoryUnitExists(itemDataEntity, unitId);

                if (!unitConversionExists)
                {
                    MessageDialog.Show(Resources.ConversionRuleMissing);
                    return;
                }
                GoodsReceivingPostResult result = service.PostGoodsReceivingLine(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocumentLineID, true);

                switch(result)
                {
                    case GoodsReceivingPostResult.AlreadyPosted:
                        MessageDialog.Show(Resources.GoodsReceivingLineAlreadyPosted);
                        break;
                    case GoodsReceivingPostResult.MissingUnitConversion:
                        MessageDialog.Show(Resources.ConversionRuleMissing);
                        break;
                }

                // Get the Goods receiving document line so that I can post a message saying the selected item's inventory has been updated
                GoodsReceivingDocumentLine grdl = service.GetGoodsReceivingDocumentLine(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocumentLineID, PluginEntry.DataModel.CurrentStoreID, true);

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ItemInventory", grdl.purchaseOrderLine.ItemID, null);

                bool statusChanged = service.UpdateGoodsReceivingDocumentStatus(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.ID, true);

                // If status needs to be changed, change it from either 0 -> 1 or 1 -> 0
                if (statusChanged)
                {
                    goodsReceivingDocument.Status = goodsReceivingDocument.Status == GoodsReceivingStatusEnum.Posted ? GoodsReceivingStatusEnum.Active : GoodsReceivingStatusEnum.Posted;
                    ClosePurchaseOrder();
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "GoodsReceivingDocument", goodsReceivingDocument.ID, goodsReceivingDocument);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        // Note: if saving algorithm for GRD line changes, that change must be applied to Applications\Integration Tests\TestRunner.Actions\RunCreateGoodsReceivingDocument()
        private void ClosePurchaseOrder()
        {
            if (goodsReceivingDocument.Status != GoodsReceivingStatusEnum.Posted) return;

            try
            {
                PurchaseOrder purchaseOrder = service.GetPurchaseOrder(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.PurchaseOrderID, false);
                if (purchaseOrder.PurchaseStatus != PurchaseStatusEnum.Closed)
                {
                    purchaseOrder.PurchaseStatus = PurchaseStatusEnum.Closed;
                    service.SavePurchaseOrder(PluginEntry.DataModel, siteServiceProfile, purchaseOrder, true);
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PurchaseOrderStatus", purchaseOrder.ID, PurchaseStatusEnum.Closed);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
            finally
            {
                service.Disconnect(PluginEntry.DataModel);
            }
        }

        private void btnPostLine_Click(object sender, EventArgs e)
        {
            try
            {
                if (QuestionDialog.Show(Resources.SureToPostItem, Resources.PostSelectedItem) != DialogResult.Yes)
                {
                    return;
                }

                PostSingleGRDItem(SelectedLineID);
                LoadItems();
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void lvItems_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (lvItems.Selection.Count != 0 && PluginEntry.DataModel.HasPermission(Permission.ManageGoodsReceivingDocuments) && btnsEditAddRemove.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnPostAllLines_Click(object sender, EventArgs e)
        {
            try
            {
                if (QuestionDialog.Show(Resources.SureToPostAllLines, Resources.PostAllLines) != DialogResult.Yes)
                {
                    return;
                }

                GoodsReceivingPostResult result = service.PostGoodsReceivingDocument(PluginEntry.DataModel, siteServiceProfile, goodsReceivingDocument.GoodsReceivingID, true);

                PluginOperations.DisplayGoodsReceivingPostingResult(result);

                if(result >= GoodsReceivingPostResult.NotFound)
                {
                    return;
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ItemInventory", RecordIdentifier.Empty, null);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "GoodsReceivingDocument", goodsReceivingDocument.ID,  goodsReceivingDocument);
                LoadItems();

            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
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
                GoodsReceivingDocumentLine grdl = (GoodsReceivingDocumentLine)lvItems.Selection[i].Tag;
                PurchaseOrderLine pol = grdl.purchaseOrderLine;
                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, pol.ItemID);
                res.Add(item);
                int salesUnitQuantity = (int)Providers.UnitConversionData.ConvertQtyBetweenUnits(PluginEntry.DataModel, item.ID, item.PurchaseUnitID, item.SalesUnitID, grdl.ReceivedQuantity);
                res[i].ID.SecondaryID = salesUnitQuantity;
            }
            return res;
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.VariantNumber, "Variant", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.ReceivedDate, "RecievedDate", ConditionType.ConditionTypeEnum.DateAndTimeRange));

            searchBar1.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Posted, true));

            searchBar1.AddCondition(new ConditionType(Resources.ReceivedQuantity, "RecievedQuantity", ConditionType.ConditionTypeEnum.Numeric));
            searchBar1.AddCondition(new ConditionType(Resources.OrderedQuantity, "OrderedQuantity", ConditionType.ConditionTypeEnum.Numeric));
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void lvItems_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvItems.SortColumn == args.Column)
            {
                lvItems.SetSortColumn(args.Column, !lvItems.SortedAscending);
            }
            else
            {
                lvItems.SetSortColumn(args.Column, true);
            }

            //SetSecondarySort(args.Column.SecondarySortColumn);

            goodsReceivingDataScroll.Reset();

            LoadItems();
        }

        private void stocCountDataScroll_PageChanged(object sender, EventArgs e)
        {
            LoadItems();
        }
    }
}