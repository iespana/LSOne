using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.TaxRefund.Dialogs;
using LSOne.Controls;

namespace LSOne.ViewPlugins.TaxRefund.Views
{
    public partial class TaxRefundRangesView : ViewBase
    {
        private List<TaxRefundRange> knownRanges;

        public TaxRefundRangesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Close |
                         ViewAttributes.ContextBar;

            lvTaxRefundRanges.ContextMenuStrip = new ContextMenuStrip();
            lvTaxRefundRanges.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.TaxRefund;
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
            HeaderText = Properties.Resources.TaxRefundRanges;
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

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            lvTaxRefundRanges.ContextMenuStrip.Items.Clear();

            var item = new ExtendedMenuItem(Properties.Resources.Edit + "...", ContextButtons.GetEditButtonImage(), 10, btnsContextButtons_EditButtonClicked);
            item.Default = true;
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            lvTaxRefundRanges.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(Properties.Resources.Add + "...", ContextButtons.GetAddButtonImage(), 20, btnsContextButtons_AddButtonClicked);
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            lvTaxRefundRanges.ContextMenuStrip.Items.Add(item);

            item = new ExtendedMenuItem(Properties.Resources.Delete + "...", ContextButtons.GetRemoveButtonImage(), 30, btnsContextButtons_RemoveButtonClicked);
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            lvTaxRefundRanges.ContextMenuStrip.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("TaxFreeRanges", lvTaxRefundRanges.ContextMenuStrip, lvTaxRefundRanges);

            e.Cancel = false;
        }

        private void LoadItems()
        {
            lvTaxRefundRanges.ClearRows();
            knownRanges = Providers.TaxRefundRangeData.GetAll(PluginEntry.DataModel);
            foreach (TaxRefundRange TaxRefundRange in knownRanges)
            {
                var row = new Row {Tag = TaxRefundRange};
                row.AddCell(new NumericCell(TaxRefundRange.ValueFrom.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)),
                    TaxRefundRange.ValueFrom));
                row.AddCell(new NumericCell(TaxRefundRange.ValueTo.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)),
                    TaxRefundRange.ValueTo));
                row.AddCell(new NumericCell(TaxRefundRange.TaxRefund.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices)),
                    TaxRefundRange.TaxRefund));
                row.AddCell(new NumericCell(TaxRefundRange.TaxRefundPercentage.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Tax)),
                    TaxRefundRange.TaxRefundPercentage));

                lvTaxRefundRanges.AddRow(row);
            }
            lvTaxRefundRanges.AutoSizeColumns();

            lvTaxRefundRanges.Sort();
        }

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            using (var dialog = new EditTaxRefundRangeDialog(null, knownRanges))
            {
                dialog.ShowDialog();
            }
            LoadItems();
        }

        private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            var range = (TaxRefundRange) lvTaxRefundRanges.Row(lvTaxRefundRanges.Selection.FirstSelectedRow).Tag;
            using (var dialog = new EditTaxRefundRangeDialog(range, knownRanges))
            {
                dialog.ShowDialog();
            }
            LoadItems();
        }

        private void btnsContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            var range = (TaxRefundRange)lvTaxRefundRanges.Row(lvTaxRefundRanges.Selection.FirstSelectedRow).Tag;
            Providers.TaxRefundRangeData.Delete(PluginEntry.DataModel, range.ID);
            LoadItems();
        }

        private void OnListSelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = lvTaxRefundRanges.Selection.Count > 0;
        }

        private void OnListRowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
                btnsContextButtons_EditButtonClicked(sender, EventArgs.Empty);
        }
    }
}
