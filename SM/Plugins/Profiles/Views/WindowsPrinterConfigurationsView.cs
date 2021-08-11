using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.ViewCore;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Enums;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.Controls.Rows;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class WindowsPrinterConfigurationsView : ViewBase
    {
        private RecordIdentifier selectedID = "";

        public WindowsPrinterConfigurationsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.Close;

            lvPrinterConfigurations.ContextMenuStrip = new ContextMenuStrip();
            lvPrinterConfigurations.ContextMenuStrip.Opening += lvPrinterConfigurations_Opening;

            btnsContextButtons.AddButtonEnabled = CanEditPrinterConfigurations();
        }

        protected override string LogicalContextName
        {
            get { return Properties.Resources.WindowsPrinters; }
        }

        public override RecordIdentifier ID
        {
            get { return RecordIdentifier.Empty; }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadPrinterConfigurations();
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
            switch (objectName)
            {
                case "WindowsPrinterConfiguration":
                    LoadPrinterConfigurations();
                    break;
            }
        }

        private void LoadPrinterConfigurations()
        {
            lvPrinterConfigurations.ClearRows();

            if(lvPrinterConfigurations.SortColumn == null)
            {
                lvPrinterConfigurations.SetSortColumn(0, true);
            }

            List<WindowsPrinterConfiguration> windowsPrinterConfigurations = Providers.WindowsPrinterConfigurationData.GetList(PluginEntry.DataModel);

            foreach (WindowsPrinterConfiguration configuration in windowsPrinterConfigurations)
            {
                Row row = new Row();

                row.AddText(configuration.ID.StringValue);
                row.AddText(configuration.Text);
                row.AddText(configuration.PrinterDeviceName);

                row.Tag = configuration;
                lvPrinterConfigurations.AddRow(row);

                if (selectedID == configuration.ID)
                {
                    lvPrinterConfigurations.Selection.Set(lvPrinterConfigurations.RowCount - 1);
                }
            }

            lvPrinterConfigurations_SelectionChanged(this, EventArgs.Empty);
            lvPrinterConfigurations.AutoSizeColumns();
            lvPrinterConfigurations.Sort(lvPrinterConfigurations.SortColumn, lvPrinterConfigurations.SortedAscending);
        }

        private void lvPrinterConfigurations_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvPrinterConfigurations.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnsContextButtons_EditButtonClicked);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnsContextButtons_AddButtonClicked);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsContextButtons_RemoveButtonClicked);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("WindowsPrinterConfigurationsList", lvPrinterConfigurations.ContextMenuStrip, lvPrinterConfigurations);

            e.Cancel = menu.Items.Count == 0;
        }

        public WindowsPrinterConfigurationsView(RecordIdentifier selectedID) : this()
        {
            this.selectedID = selectedID;
        }

        private bool CanEditPrinterConfigurations()
        {
            return PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.HardwareProfileEdit) || PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageStationPrinting);
        }

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowWindowsPrinterConfigurationDialog(RecordIdentifier.Empty);
        }

        private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowWindowsPrinterConfigurationDialog(selectedID);
        }

        private void btnsContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteWindowsPrinterConfiguration(selectedID);
        }

        private void lvPrinterConfigurations_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnsContextButtons_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvPrinterConfigurations_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = (lvPrinterConfigurations.Selection.Count == 1) ? ((WindowsPrinterConfiguration)lvPrinterConfigurations.Selection[0].Tag).ID : RecordIdentifier.Empty;

            bool canEdit = CanEditPrinterConfigurations() && lvPrinterConfigurations.Selection.Count == 1;
            btnsContextButtons.EditButtonEnabled = canEdit;
            btnsContextButtons.RemoveButtonEnabled = canEdit && !((WindowsPrinterConfiguration)lvPrinterConfigurations.Selection[0].Tag).ConfigurationUsed;
        }
    }
}
