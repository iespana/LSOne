using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Inventory.Properties;

namespace LSOne.ViewPlugins.Inventory.Dialogs
{
    public partial class ItemVendorMultiEditDialog : DialogBase
    {
        private RecordIdentifier vendorID;
        private List<MasterIDEntity> selectedLines;
        private List<MasterIDEntity> linesToAdd;
        private List<MasterIDEntity> linesToRemove;
        private Style redStrikeThroughStyle;
        private Style greenStyle;
        private Style redStyle;

        private struct RowValue
        {
            public RowTypeEnum RowType;
            public MasterIDEntity Value;
        }

        public ItemVendorMultiEditDialog(RecordIdentifier vendorID)
        {
            this.vendorID = vendorID;
            InitializeComponent();


            selectedLines = new List<MasterIDEntity>();
            linesToAdd = new List<MasterIDEntity>();
            linesToRemove = new List<MasterIDEntity>();

            redStrikeThroughStyle = new Style(lvlEditPreview.DefaultStyle);
            redStrikeThroughStyle.Font = new Font(redStrikeThroughStyle.Font, FontStyle.Strikeout);
            redStrikeThroughStyle.TextColor = ColorPalette.RedDark;

            greenStyle = new Style(lvlEditPreview.DefaultStyle) { TextColor = ColorPalette.GreenDark };

            redStyle = new Style(lvlEditPreview.DefaultStyle) { TextColor = ColorPalette.RedDark };
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void VendorItemMultiEditDialog_Load(object sender, EventArgs e)
        {
            var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);
            try
            {
                selectedLines = new List<MasterIDEntity>(service.GetDistinctRetailItemsForVendor(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendorID, true));
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }
            LoadPreviewLines();
        }

        private void LoadPreviewLines()
        {
            lvlEditPreview.ClearRows();

            AddSelectedAndRemovedPreviewRows();
            AddNewPreviewRows();

            lvlEditPreview.AutoSizeColumns(true);
        }

        private void AddNewPreviewRows()
        {
            foreach (var line in linesToAdd)
            {
                var row = new Row();
                row.AddCell(new Cell(line.Text, greenStyle));
                row.AddCell(new Cell(line.ExtendedText, greenStyle));
                row.AddCell(new Cell(Properties.Resources.Add, greenStyle));
                row.Tag = new RowValue() { RowType = RowTypeEnum.LineToAdd, Value = line };

                var button = new IconButton(Properties.Resources.RevertSmallImage, Properties.Resources.Undo, true);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                lvlEditPreview.AddRow(row);
            }
        }

        private void AddSelectedAndRemovedPreviewRows()
        {
            //Copy selected and removed to a new list. This is needed so we don't modify the existing lists in memory

            var selectedAndRemoved = new List<MasterIDEntity>();

            foreach (MasterIDEntity line in selectedLines)
            {
                selectedAndRemoved.Add(line);
            }

            foreach (MasterIDEntity line in linesToRemove)
            {
                selectedAndRemoved.Add(line);
            }

            selectedAndRemoved.Sort(CompareDataEntities);

            foreach (MasterIDEntity line in selectedAndRemoved)
            {
                if (linesToRemove.Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddCell(new Cell(line.Text, redStrikeThroughStyle));
                    row.AddCell(new Cell(line.ExtendedText, redStrikeThroughStyle));
                    row.AddCell(new Cell(Properties.Resources.Delete, redStyle));
                    row.Tag = new RowValue() { RowType = RowTypeEnum.LineToRemove, Value = line };

                    var button = new IconButton(Properties.Resources.RevertSmallImage, Properties.Resources.Undo, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                    lvlEditPreview.AddRow(row);
                }
                else if (!linesToAdd.Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddText(line.Text);
                    row.AddText(line.ExtendedText);
                    row.AddText(Properties.Resources.EditTypeNone);
                    row.Tag = new RowValue() { RowType = RowTypeEnum.SelectedLine, Value = line };

                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                    lvlEditPreview.AddRow(row);
                }
            }
        }

        private int CompareDataEntities(MasterIDEntity dataEntity, MasterIDEntity entity)
        {
            int reply = dataEntity.Text.CompareTo(entity.Text);
            if (dataEntity.ExtendedText != null && entity.ExtendedText != null)
            {
                reply += dataEntity.ExtendedText.CompareTo(entity.ExtendedText);
            }
            return reply;

        }

        private void lvlEditPreview_CellAction(object sender, CellEventArgs args)
        {
            var rowValue = (RowValue)lvlEditPreview.Row(args.RowNumber).Tag;

            // Row type
            switch (rowValue.RowType)
            {
                case RowTypeEnum.SelectedLine:
                    if (!linesToAdd.Exists(p => p.ID == rowValue.Value.ID))
                    {
                        selectedLines.Remove(rowValue.Value);
                        linesToRemove.Add(rowValue.Value);
                    }
                    break;
                case RowTypeEnum.LineToAdd:
                    linesToAdd.Remove(rowValue.Value);
                    selectedLines.Remove(rowValue.Value);
                    break;
                case RowTypeEnum.LineToRemove:
                    // Undo a remove
                    linesToRemove.Remove(rowValue.Value);
                    selectedLines.Add(rowValue.Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LoadPreviewLines();
            CheckEnabled();
        }

        private void cmbItems_DropDown(object sender, DropDownEventArgs e)
        {
            List<DataEntity> selectionList = new List<DataEntity>(selectedLines);
            List<DataEntity> addList = new List<DataEntity>(linesToAdd);
            List<DataEntity> removeList = new List<DataEntity>(linesToRemove);


            e.ControlToEmbed = new MultiSearchPanel(PluginEntry.DataModel,
                                            selectionList,
                                            addList,
                                            removeList,
                                            SearchTypeEnum.InventoryItemsMasterID,
                                            false, false, false);
            cmbItems.ShowDropDownOnTyping = true;
        }

        private void cmbItems_SelectedDataChanged(object sender, EventArgs e)
        {
            int prevAddedCount = linesToAdd.Count;

            selectedLines = cmbItems.SelectionList.Cast<MasterIDEntity>().ToList();
            linesToAdd = cmbItems.AddList.Cast<MasterIDEntity>().ToList();
            linesToRemove = cmbItems.RemoveList.Cast<MasterIDEntity>().ToList();

            int curAddedCount = linesToAdd.Count;

            LoadPreviewLines();
            CheckEnabled();

            if (curAddedCount > prevAddedCount && !lvlEditPreview.RowIsOnScreen(lvlEditPreview.RowCount - 1))
            {
                lvlEditPreview.ScrollRowIntoView(lvlEditPreview.RowCount - 1);
            }
        }

        private void CheckEnabled()
        {
            bool added = linesToAdd.Count > 0;

            bool removed = linesToRemove.Count > 0;

            btnOK.Enabled = added || removed;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

            try
            {

                foreach (MasterIDEntity line in linesToAdd)
                {
                    CreateAndSaveVendorItemLine(line.ReadadbleID, service);
                }

                service.DeleteVendorItemByRetailItemID(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), linesToRemove, vendorID, false);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer);
            }

            service.Disconnect(PluginEntry.DataModel);
        }

        private void CreateAndSaveVendorItemLine(RecordIdentifier itemID, IInventoryService service)
        {
            var vendorItem = new VendorItem();
            vendorItem.VendorID = vendorID;

            vendorItem.VendorItemID = "";
            vendorItem.RetailItemID = itemID;
            vendorItem.VariantName = "";
            vendorItem.UnitID = Providers.RetailItemData.GetItemUnitID(PluginEntry.DataModel, itemID, RetailItem.UnitTypeEnum.Inventory);

            vendorItem.ID = service.SaveVendorItem(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), vendorItem, false);

            // Check if this is the first item for the vendor, if so he should be the items default vendor
            if (!Providers.RetailItemData.ItemHasDefaultVendor(PluginEntry.DataModel, itemID))
            {
                Providers.RetailItemData.SetItemsDefaultVendor(PluginEntry.DataModel, itemID, vendorItem.VendorID);
            }
        }
    }
}
