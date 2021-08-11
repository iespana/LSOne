using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views
{
    public partial class PrintersView : ViewBase
    {
        RecordIdentifier selectedID = "";

        public PrintersView(RecordIdentifier kitchenPrinterId)
            : this()
        {
            selectedID = kitchenPrinterId;
        }

        public PrintersView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            lvKitchenPrinters.Columns[0].Tag = KitchenPrinterSortingEnum.Id;
            lvKitchenPrinters.Columns[1].Tag = KitchenPrinterSortingEnum.Description;
            lvKitchenPrinters.Columns[2].Tag = KitchenPrinterSortingEnum.Host;
            lvKitchenPrinters.Columns[3].Tag = KitchenPrinterSortingEnum.PrinterName;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageKitchenPrinters);

            lvKitchenPrinters.ContextMenuStrip = new ContextMenuStrip();
            lvKitchenPrinters.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageKitchenPrinters);
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvKitchenPrinters.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here.
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemove_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsEditAddRemove_AddButtonClicked);

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsEditAddRemove_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("KitchenPrintersList", lvKitchenPrinters.ContextMenuStrip, lvKitchenPrinters);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Printers;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadKitchenPrinters();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "KitchenPrinter":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadKitchenPrinters();
                    break;
            }

        }

        private void LoadKitchenPrinters()
        {
            lvKitchenPrinters.ClearRows();

            List<KitchenDisplayPrinter> printers = Providers.KitchenDisplayPrinterData.GetList(
                PluginEntry.DataModel,
                (KitchenPrinterSortingEnum)
                    lvKitchenPrinters.SortColumn.Tag, 
                lvKitchenPrinters.SortedAscending);

            foreach (var printer in printers)
            {
                var row = new Row { Tag = printer.ID };

                row.AddText((string)printer.ID);
                row.AddText(printer.Text);
                row.AddText(printer.HostDescription);
                row.AddText(printer.PrinterName);

                lvKitchenPrinters.AddRow(row);

                if (printer.ID == selectedID)
                {
                    lvKitchenPrinters.Selection.Set(lvKitchenPrinters.RowCount - 1);
                }
            }

            lvKitchenPrinters.AutoSizeColumns(true);
            lvKitchenPrinters_SelectionChanged(null, EventArgs.Empty);
        }

        private void lvKitchenPrinters_SelectionChanged(object sender, EventArgs e)
        {
            bool hasPermission = PluginEntry.DataModel.HasPermission(Permission.ManageKitchenPrinters);
            int rowsSelected = lvKitchenPrinters.Selection.Count;
            if (rowsSelected <= 0)
            {
                selectedID = null;
                btnsEditAddRemove.EditButtonEnabled = false;
                btnsEditAddRemove.RemoveButtonEnabled = false;
            }
            else if (rowsSelected == 1)
            {
                selectedID = (RecordIdentifier)lvKitchenPrinters.Row(lvKitchenPrinters.Selection.FirstSelectedRow).Tag;
                btnsEditAddRemove.RemoveButtonEnabled = hasPermission;
                btnsEditAddRemove.EditButtonEnabled = hasPermission;
            }
            else
            {
                selectedID = null;
                btnsEditAddRemove.EditButtonEnabled = false;
                btnsEditAddRemove.EditButtonEnabled = hasPermission;
            }
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.CreateKitchenPrinter();
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowKitchenPrinter(selectedID);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            for (int i = 0; i < lvKitchenPrinters.Selection.Count; i++)
            {
                PluginOperationsHelper.DeleteKitchenPrinter((RecordIdentifier)lvKitchenPrinters.Selection[0].Tag);
            }
        }

        private void lvKitchenPrinters_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }
        
    }
}
