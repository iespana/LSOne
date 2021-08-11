using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Replenishment.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Replenishment.Views
{
    public partial class PurchaseWorksheetView : ViewBase
    {
        private RecordIdentifier selectedLineId;
        private RecordIdentifier purchaseWorksheetId;
        private InventoryTemplate inventoryTemplate;
        private IInventoryService service;

        private List<PurchaseWorksheetLine> purchaseWorksheetLines;

        private IPlugin itemEditor;

        public PurchaseWorksheetView(RecordIdentifier purchaseWorksheetId, RecordIdentifier selectedLineId)
            : this(purchaseWorksheetId)
        {
            this.selectedLineId = selectedLineId;
        }

        public PurchaseWorksheetView(RecordIdentifier purchaseWorksheetId)
            : this()
        {
            this.purchaseWorksheetId = purchaseWorksheetId;

            inventoryTemplate = service.GetInventoryTemplateForPOWorksheet(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), purchaseWorksheetId, true);
        }

        private PurchaseWorksheetView()
        {
            InitializeComponent();

            service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            Attributes = ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            lvPurchaseWorksheet.ContextMenuStrip = new ContextMenuStrip();
            lvPurchaseWorksheet.ContextMenuStrip.Opening += lvPurchaseWorksheets_RightClick;

            clBarcode.Tag = PurchaseWorksheetLineSortEnum.Barcode;
            clItemId.Tag = PurchaseWorksheetLineSortEnum.ItemId;
            clDescription.Tag = PurchaseWorksheetLineSortEnum.Description;
            colVariant.Tag = PurchaseWorksheetLineSortEnum.VariantName;
            clVendor.Tag = PurchaseWorksheetLineSortEnum.VendorName;
            clQuantity.Tag = PurchaseWorksheetLineSortEnum.OrderingQuantity;
            clSuggestedQuantity.Tag = PurchaseWorksheetLineSortEnum.SuggestedQuantity;
            clReorderPoint.Tag = PurchaseWorksheetLineSortEnum.ReorderPoint;
            clMaxInventory.Tag = PurchaseWorksheetLineSortEnum.MaximumInventory;
            lvPurchaseWorksheet.SetSortColumn(clDescription, true);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageReplenishment);

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            itemDataScroll.Reset();
        }

        protected override string LogicalContextName
        {
            get
            {
                return HeaderText;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PurchaseWorksheet")
            {
                selectedLineId = (changeAction == DataEntityChangeType.Edit) ? changeIdentifier : RecordIdentifier.Empty;
                ProcessWithLoadingScreen(LoadLines);
            }
            if (objectName == "PurchaseWorksheetLine")
            {
                ProcessWithLoadingScreen(LoadLines);
            }

            if (objectName == "InventoryTemplate" && changeAction == DataEntityChangeType.Delete)
            {
                ProcessWithLoadingScreen(LoadLines);
                btnsLines.AddButtonEnabled = false;
                btnsLines.EditButtonEnabled = false;
                btnsLines.RemoveButtonEnabled = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // This logic needs to run after the visual components are drawn on the screen
            if (!DesignMode)
            {
                lvPurchaseWorksheet.AutoSizeColumns(true);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (!DesignMode && lvPurchaseWorksheet != null)
            {
                lvPurchaseWorksheet.AutoSizeColumns(true);
            }
        }

        public override void DisplayComplete()
        {
            ProcessWithSpinnerDialog(delegate ()
            {
                PluginOperations.EnteringPurchaseWorksheetView(purchaseWorksheetId);
                itemEditor = PluginEntry.Framework.FindImplementor(this, "CanViewRetailItem", null);

            });

            ProcessWithLoadingScreen(LoadLines);
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        protected override void LoadData(bool isRevert)
        {
            ///Moved load logic to DisplayComplete method to show the view asap with the progress dialog
        }

        private void LoadLines()
        {
            Style strikeThroughStyle = new Style(lvPurchaseWorksheet.DefaultStyle);
            strikeThroughStyle.Font = new Font(strikeThroughStyle.Font, FontStyle.Strikeout);

            RecordIdentifier selectedID = selectedLineId;

            lvPurchaseWorksheet.ClearRows();
            selectedLineId = null;

            int itemCount = 0;
            purchaseWorksheetLines = service.GetPurchaseWorksheetLineDataPaged(
                            PluginEntry.DataModel,
                            PluginOperations.GetSiteServiceProfile(),
                            purchaseWorksheetId,
                            false,
                            (PurchaseWorksheetLineSortEnum)lvPurchaseWorksheet.SortColumn.Tag,
                            !lvPurchaseWorksheet.SortedAscending,
                            itemDataScroll.StartRecord,
                            itemDataScroll.EndRecord + 1,
                            true);

            if (purchaseWorksheetLines != null && purchaseWorksheetLines.Count > 0)
            {
                itemCount = purchaseWorksheetLines[0].TotalNumberOfRows;
            }

            itemDataScroll.RefreshState(purchaseWorksheetLines, itemCount);

            btnPost.Enabled = (purchaseWorksheetLines.Count > 0);

            bool displayVariant = false;
            if(lvPurchaseWorksheet.Columns.Contains(colVariant))
            {
                lvPurchaseWorksheet.Columns.Remove(colVariant);
            }

            if(purchaseWorksheetLines.Any(x => x.VariantName != ""))
            {
                displayVariant = true;
                lvPurchaseWorksheet.Columns.Insert(lvPurchaseWorksheet.Columns.IndexOf(clDescription) + 1, colVariant);
            }

            // Hide columns that are not to be shown
            if (!inventoryTemplate.DisplayBarcode)
            {
                if (lvPurchaseWorksheet.Columns.Contains(clBarcode))
                {
                    lvPurchaseWorksheet.Columns.Remove(clBarcode);
                }
            }
            if (!inventoryTemplate.DisplayMaximumInventory)
            {
                if (lvPurchaseWorksheet.Columns.Contains(clMaxInventory))
                {
                    lvPurchaseWorksheet.Columns.Remove(clMaxInventory);
                }
            }
            if (!inventoryTemplate.DisplayReorderPoint)
            {
                if (lvPurchaseWorksheet.Columns.Contains(clReorderPoint))
                {
                    lvPurchaseWorksheet.Columns.Remove(clReorderPoint);
                }
            }

            Style rowStyle = lvPurchaseWorksheet.DefaultStyle;
            foreach (PurchaseWorksheetLine line in purchaseWorksheetLines)
            {
                Row row = new Row();

                DecimalLimit quantityLimit = Providers.UnitData.GetNumberLimitForUnit(PluginEntry.DataModel, Providers.UnitData.GetIdFromDescription(PluginEntry.DataModel, line.Unit.Text), CacheType.CacheTypeApplicationLifeTime);

                rowStyle = line.IsInventoryExcluded() ? strikeThroughStyle : lvPurchaseWorksheet.DefaultStyle;

                if (itemEditor == null)
                {
                    row.AddCell(new Cell((string)line.Item.ID, rowStyle));
                }
                else
                {
                    row.AddCell(new LinkCell((string)line.Item.ID));
                }

                row.AddCell(new Cell(line.Item.Text, rowStyle));

                if (displayVariant)
                {
                    row.AddCell(new Cell(line.VariantName, rowStyle));
                }

                if (inventoryTemplate.DisplayBarcode)
                {
                    row.AddCell(new Cell(line.BarCodeNumber, rowStyle));
                }

                row.AddCell(new Cell(line.Vendor.Text, rowStyle));
                row.AddCell(new Cell(line.Quantity.FormatWithLimits(quantityLimit) + " " + line.Unit.Text, rowStyle));
                row.AddCell(new Cell(line.SuggestedQuantity.FormatWithLimits(quantityLimit) + " " + line.InventoryUnit.Text, rowStyle));
                row.AddCell(new Cell(line.EffectiveInventory.FormatWithLimits(quantityLimit) + " " + line.InventoryUnit.Text, rowStyle));

                if (inventoryTemplate.DisplayReorderPoint)
                {
                    row.AddCell(new Cell(line.ReorderPoint.FormatWithLimits(quantityLimit) + " " + line.InventoryUnit.Text, rowStyle));
                }
                if (inventoryTemplate.DisplayMaximumInventory)
                {
                    row.AddCell(new Cell(line.MaximumInventory.FormatWithLimits(quantityLimit) + " " + line.InventoryUnit.Text, rowStyle));
                }
                if (!line.IsInventoryExcluded())
                {
                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Resources.Delete, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));
                }

                row.Tag = line;
                lvPurchaseWorksheet.AddRow(row);

                if (line.ID == selectedID)
                {
                    lvPurchaseWorksheet.Selection.Set(lvPurchaseWorksheet.RowCount - 1);
                }
            }

            HideProgress();

            lvPurchaseWorksheet.AutoSizeColumns(true);
        }

        private void btnsLines_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.PurchaseWorksheetLineDialog(purchaseWorksheetId);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProcessWithLoadingScreen(LoadLines);
            }
        }

        private void btnsLines_EditButtonClicked(object sender, EventArgs e)
        {
            var dlg = new Dialogs.PurchaseWorksheetLineDialog(purchaseWorksheetId, selectedLineId);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ProcessWithLoadingScreen(LoadLines);
            }
        }

        private void btnsLines_RemoveButtonClicked(object sender, EventArgs e)
        {
            ProcessWithLoadingScreen(delegate ()
            {
                if (lvPurchaseWorksheet.Selection.Count == 1)
                {
                    PluginOperations.DeletePurchaseWorksheetLine(selectedLineId);
                }
                else if (lvPurchaseWorksheet.Selection.Count > 1)
                {
                    var selectedIDs = new List<RecordIdentifier>();

                    for (int i = 0; i < lvPurchaseWorksheet.Selection.Count; i++)
                    {
                        selectedIDs.Add(((PurchaseWorksheetLine)lvPurchaseWorksheet.Selection[i].Tag).ID);
                    }

                    PluginOperations.DeletePurchaseWorksheetLines(selectedIDs);
                }

                LoadLines();
            });
        }

        private void lvPurchaseWorksheets_SelectionChanged(object sender, EventArgs e)
        {
            btnsLines.EditButtonEnabled = lvPurchaseWorksheet.Selection.Count == 1;
            btnsLines.RemoveButtonEnabled = lvPurchaseWorksheet.Selection.Count > 0;

            if (lvPurchaseWorksheet.Selection.Count == 1)
            {
                selectedLineId = ((PurchaseWorksheetLine)lvPurchaseWorksheet.Row(lvPurchaseWorksheet.Selection.FirstSelectedRow).Tag).ID;
            }
            else if (lvPurchaseWorksheet.Selection.Count > 1)
            {
                selectedLineId = null;
            }

            if (lvPurchaseWorksheet.Selection.FirstSelectedRow > 0 && selectedLineId != null)
            {
                var selectedPwLine = (PurchaseWorksheetLine)lvPurchaseWorksheet.Row(lvPurchaseWorksheet.Selection.FirstSelectedRow).Tag;
                if (selectedPwLine.IsInventoryExcluded())
                {
                    btnsLines.EditButtonEnabled = false;
                }
            }
        }

        private void lvPurchaseWorksheets_RightClick(object sender, CancelEventArgs e)
        {
            var menu = lvPurchaseWorksheet.ContextMenuStrip;

            bool isServiceItem = false;
            if (selectedLineId != null)
            {
                var selectedPwLine = (PurchaseWorksheetLine)lvPurchaseWorksheet.Row(lvPurchaseWorksheet.Selection.FirstSelectedRow).Tag;
                isServiceItem = selectedPwLine == null ? false : selectedPwLine.IsInventoryExcluded();
            }

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                   Resources.Add,
                   100,
                   btnsLines_AddButtonClicked)
            {
                Enabled = btnsLines.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };
            menu.Items.Add(item);

            if (!isServiceItem)
            {
                item = new ExtendedMenuItem(
                       Resources.Edit,
                       200,
                       btnsLines_EditButtonClicked)
                {
                    Enabled = btnsLines.EditButtonEnabled,
                    Image = ContextButtons.GetEditButtonImage(),
                    Default = true
                };
                menu.Items.Add(item);
            }

            item = new ExtendedMenuItem(
                   Resources.Delete,
                   300,
                   btnsLines_RemoveButtonClicked)
            {
                Enabled = btnsLines.RemoveButtonEnabled,
                Image = ContextButtons.GetRemoveButtonImage()
            };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PurchaseWorksheetsList", lvPurchaseWorksheet.ContextMenuStrip, lvPurchaseWorksheet);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvPurchaseWorksheets_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsLines.EditButtonEnabled)
            {
                btnsLines_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                arguments.Add(new ContextBarItem(Resources.Refresh, Refresh), 100);
                arguments.Add(new ContextBarItem(Resources.Reset, Reset), 200);
            }
            else if (arguments.CategoryKey == GetType() + ".Related")
            {
                arguments.Add(new ContextBarItem(Resources.InventoryTemplate, null, ShowInventoryTemplate), 100);
            }
        }

        private void ShowInventoryTemplate(object sender, ContextBarClickEventArguments args)
        {
            PluginOperations.ShowInventoryTemplates(inventoryTemplate.ID);
        }

        private void Refresh(object sender, ContextBarClickEventArguments args)
        {
            ProcessWithSpinnerDialog(() => PluginOperations.RefreshPurchaseWorksheetLines(inventoryTemplate.ID, purchaseWorksheetId));
            ProcessWithLoadingScreen(LoadLines);
        }

        private void Reset(object sender, ContextBarClickEventArguments args)
        {
            if (QuestionDialog.Show(Resources.ResetWorksheetQuestion, Resources.ResetWorksheet) == DialogResult.Yes)
            {
                ProcessWithSpinnerDialog(() => PluginOperations.ResetPurchaseWorksheetLines(purchaseWorksheetId));
                ProcessWithLoadingScreen(LoadLines);
            }
        }

        private void btnPost_Click(object sender, EventArgs e)
        {
            PluginOperations.PostPurchaseWorksheet(purchaseWorksheetId);
            ProcessWithLoadingScreen(LoadLines);
        }

        private void lvPurchaseWorksheet_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvPurchaseWorksheet.SortColumn == args.Column)
            {
                lvPurchaseWorksheet.SetSortColumn(args.Column, !lvPurchaseWorksheet.SortedAscending);
            }
            else
            {
                lvPurchaseWorksheet.SetSortColumn(args.Column, true);
            }
            ProcessWithLoadingScreen(LoadLines);
        }

        private void lvPurchaseWorksheet_CellAction(object sender, CellEventArgs args)
        {
            if (args.ColumnNumber == lvPurchaseWorksheet.Columns.IndexOf(clItemId) && args.Cell is LinkCell)
            {
                itemEditor.Message(this, "ViewItem", new RecordIdentifier((args.Cell as LinkCell).Text));
            }
            else if (btnsLines.RemoveButtonEnabled)
            {
                PluginOperations.DeletePurchaseWorksheetLine(((PurchaseWorksheetLine)lvPurchaseWorksheet.Row(args.RowNumber).Tag).ID);
                ProcessWithLoadingScreen(LoadLines);
            }
        }

        /// <summary>
        /// Executes the given <see cref="System.Action"/> while displaying a loading screen (for long running operations). 
        /// </summary>
        /// <param name="processMethod"></param>
        private void ProcessWithLoadingScreen(Action processMethod, string messageText = null)
        {
            ShowProgress((sender1, e1) =>
            {
                try
                {
                    processMethod();
                }
                catch (Exception ex)
                {
                    MessageDialog.Show(ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    HideProgress();
                }
            }, messageText);
        }

        private void ProcessWithSpinnerDialog(Action processMethod)
        {
            try
            {
                SpinnerDialog dlg = new SpinnerDialog(Resources.FewMinutesMessage, processMethod);
                dlg.ShowDialog();
            }
            catch(Exception e)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + e.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void itemDataScroll_PageChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadLines(), GetLocalizedSearchingText());
        }
    }
}
