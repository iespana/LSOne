using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class InventoryAdjustmentView : ViewBase
    {
        private IProfileSettings settings = PluginEntry.DataModel.Settings;
        private InventoryJournalTypeEnum typeOfAdjustment;
        private RecordIdentifier journalID;

        public InventoryAdjustmentView(RecordIdentifier journalID)
            : this()
        {
            this.journalID = journalID;
        }

        public InventoryAdjustmentView(RecordIdentifier journalID, InventoryJournalTypeEnum typeOfAdjustment)
            : this(journalID)
        {
            this.typeOfAdjustment = typeOfAdjustment;
            lblGroupHeader.Text = typeOfAdjustment == InventoryJournalTypeEnum.Adjustment
                ? Properties.Resources.InventoryAdjustmentLines
                : Properties.Resources.StockReservationLines;
        }

        public InventoryAdjustmentView(InventoryJournalTypeEnum typeOfAdjustment)
            : this()
        {
            this.typeOfAdjustment = typeOfAdjustment;
            lblGroupHeader.Text = typeOfAdjustment == InventoryJournalTypeEnum.Adjustment
                ? Properties.Resources.InventoryAdjustmentLines
                : Properties.Resources.StockReservationLines;
        }

        public InventoryAdjustmentView()
        {
            InitializeComponent();

            Attributes =
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Audit |
                ViewAttributes.Help;

            typeOfAdjustment = InventoryJournalTypeEnum.Adjustment;

           
            lvJournalEntries.ContextMenuStrip = new ContextMenuStrip();
            lvJournalEntries.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvJournalTrans.ContextMenuStrip = new ContextMenuStrip();
            lvJournalTrans.ContextMenuStrip.Opening += ContextMenuStrip2_Opening;

            btnAddInventoryAdjustment.Enabled =
                PluginEntry.DataModel.HasPermission(Permission.EditInventoryAdjustments);
            btnAddAdjustmentLine.Enabled = btnAddInventoryAdjustment.Enabled;
        }

        protected override string LogicalContextName
        {
            get
            {
                return typeOfAdjustment == InventoryJournalTypeEnum.Adjustment
                    ? Properties.Resources.InventoryAdjustment
                    : Properties.Resources.StockReservation;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                // If our sheet would be multi-instance sheet then we would return context identifier UUID here,
                // such as User.GUID that identifies that particular User. For single instance sheets we return 
                // RecordIdentifier.Empty to tell the framework that there can only be one instace of this sheet, which will
                // make the framework make sure there is only one instance in the viewstack.
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (isRevert)
            {

            }

            LoadItems(false);

            HeaderText = typeOfAdjustment == InventoryJournalTypeEnum.Adjustment
                ? Properties.Resources.InventoryAdjustment
                : Properties.Resources.StockReservation;
        }

        private void LoadItems(bool addingNew)
        {
            List<InventoryAdjustment> items;

            lvJournalEntries.ClearRows();

            var service = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);

            items = service.GetInventoryAdjustmentJournalList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                PluginEntry.DataModel.CurrentStoreID,
                typeOfAdjustment,
                typeOfAdjustment == InventoryJournalTypeEnum.Adjustment ? -1 : (int) InventoryJournalStatus.Active,
                InventoryAdjustmentSorting.ID, false, true);

            Row row;
            foreach (InventoryAdjustment item in items)
            {
                row = new Row();
                row.AddText((string) item.ID);
                row.AddText(item.Text);
                row.AddText(item.CreatedDateTime.ToShortDateString() + " " + item.CreatedDateTime.ToShortTimeString());
                row.AddText(item.StoreName);

                if (item.Posted == InventoryJournalStatus.Posted)
                {
                    row.BackColor = ColorPalette.RedLight;
                }

                row.Tag = item;

                lvJournalEntries.AddRow(row);
            }

            if (addingNew)
            {
                lvJournalEntries.Selection.Set(lvJournalEntries.Rows.Count - 1);
            }

            lvJournalEntries_SelectedIndexChanged(this, EventArgs.Empty);

            lvJournalEntries.AutoSizeColumns();
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("InventJPLHeaderLines", RecordIdentifier.Empty,
                Properties.Resources.InventoryAdjustment, false));
            contexts.Add(new AuditDescriptor("InventoryJPLLines", RecordIdentifier.Empty, Properties.Resources.Lines,
                false));
        }

        private void lvJournalEntries_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LoadLines()
        {

            if (lvJournalEntries.Selection.Count == 0)
            {
                return;
            }

            lvJournalTrans.ClearRows();

            var service = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);

            List<InventoryJournalTransaction> lines = service.GetJournalTransactionList(
                PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                SelectedJournalEntry.ID,
                InventoryJournalTransactionSorting.ItemName,
                false,
                false,
                false);

            Row row;
            Style greenCellStyle = new Style(lvJournalTrans.DefaultStyle);
            greenCellStyle.TextColor = ColorPalette.GreenLight;
            Style redCellStyle = new Style(lvJournalTrans.DefaultStyle);
            redCellStyle.TextColor = ColorPalette.RedLight;

            foreach (var line in lines)
            {
                row = new Row();

                DataEntity currentReason = service.GetJournalTransactionReasonById(PluginEntry.DataModel,
                    PluginOperations.GetSiteServiceProfile(), line.ReasonId, false);

                row.AddText((string) line.ItemId);
                row.AddText(line.ItemName);
                row.AddText(line.VariantName);
                row.AddCell(new Cell(line.Adjustment.ToString("N2"), line.Adjustment > 0 ? greenCellStyle : redCellStyle));
                row.AddText(line.UnitDescription);
                row.AddText(currentReason.Text);
                row.AddCell(new DateTimeCell(line.PostedDateTime.ToString(), line.PostedDateTime));

                row.Tag = line.ID;

                lvJournalTrans.AddRow(row);
            }
            service.Disconnect(PluginEntry.DataModel);
            lvJournalTrans.AutoSizeColumns();
        }

        private InventoryAdjustment SelectedJournalEntry
        {
            get { return (InventoryAdjustment) lvJournalEntries.Row(lvJournalEntries.Selection.FirstSelectedRow).Tag; }
        }

        private void AddInventoryAdjustment(object sender, EventArgs e)
        {
            var dlg = new Dialogs.ProfitLossDialog(typeOfAdjustment);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems(true);
            }
        }

        private void AddAdjustmentLine(object sender, EventArgs e)
        {
            var dlg = new Dialogs.AdjustmentLineDialog(SelectedJournalEntry.ID, SelectedJournalEntry.StoreId, typeOfAdjustment);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (typeOfAdjustment == InventoryJournalTypeEnum.Reservation)
                {
                    var service = (IInventoryService) PluginEntry.DataModel.Service(ServiceType.InventoryService);

                    List<InventoryJournalTransaction> stockReservations = service.GetJournalTransactionList(
                        PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                        SelectedJournalEntry.ID,
                        InventoryJournalTransactionSorting.ItemName,
                        false,
                        false,
                        false);

                    decimal totalReservation =
                        stockReservations.Sum(
                            reservation =>
                                stockReservations.Where(w => w.ItemId == reservation.ItemId).Sum(s => s.Adjustment));

                    if (totalReservation == decimal.Zero)
                    {
                        if (QuestionDialog.Show(Properties.Resources.DoYouWantToCloseThisStockReservation) ==
                            DialogResult.Yes)
                        {
                            service.CloseInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(),
                                SelectedJournalEntry.ID, false);
                        }
                    }

                    service.Disconnect(PluginEntry.DataModel);

                }

                LoadLines();
            }
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvJournalEntries.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Add,
                200,
                AddInventoryAdjustment);

            item.Enabled = btnAddInventoryAdjustment.Enabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void ContextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvJournalTrans.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Add,
                200,
                AddAdjustmentLine);

            item.Enabled = btnAddAdjustmentLine.Enabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void lvJournalEntries_SelectionChanged(object sender, EventArgs e)
        {
            if (lvJournalEntries.Selection.Count > 0)
            {
                if (lblGroupHeader.Visible == false)
                {
                    lblGroupHeader.Visible = true;
                    lvJournalTrans.Visible = true;
                    btnAddAdjustmentLine.Visible = true;
                    lblNoSelection.Visible = false;
                }

                LoadLines();
            }
            else if (lblGroupHeader.Visible)
            {
                lblGroupHeader.Visible = false;
                lvJournalTrans.Visible = false;
                btnAddAdjustmentLine.Visible = false;
                lblNoSelection.Visible = true;
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "ItemInventory":
                    if (param != null)
                    {
                        InventoryJournalTransaction journalTransactionLine = (InventoryJournalTransaction) param;
                        RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, journalTransactionLine.ItemId,false);

                        Row row = new Row();
                        Style greenCellStyle = new Style(lvJournalTrans.DefaultStyle);
                        greenCellStyle.TextColor = ColorPalette.GreenLight;
                        Style redCellStyle = new Style(lvJournalTrans.DefaultStyle);
                        redCellStyle.TextColor = ColorPalette.RedLight;

                        row.AddText((string)journalTransactionLine.ItemId);
                        row.AddText(item.Text);
                        row.AddText(item.VariantName);
                        row.AddCell(new Cell(journalTransactionLine.Adjustment.ToString("N2"),
                                            journalTransactionLine.Adjustment > 0 ? greenCellStyle : redCellStyle));
                        row.AddText(journalTransactionLine.UnitDescription);
                        row.AddText(journalTransactionLine.ReasonText);
                        row.AddCell(new DateTimeCell(DateTime.Now.ToString(), DateTime.Now));

                        row.Tag = journalTransactionLine.ID;

                        lvJournalTrans.AddRow(row);
                        lvJournalTrans.AutoSizeColumns();
                    }
                    break;
            }
        }
    }
}
