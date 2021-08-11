using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class HeaderPaneColumnDialog : DialogBase
    {
        private LSOneHeaderPaneLineColumn headerPaneColumn;
        private RecordIdentifier headerPaneLineID;
        private RecordIdentifier headerPaneID;

        public HeaderPaneColumnDialog(RecordIdentifier headerPaneColumnID, RecordIdentifier headerPaneLineID, RecordIdentifier headerPaneID)
            : this()
        {
            this.headerPaneLineID = headerPaneLineID;
            this.headerPaneID = headerPaneID;

            headerPaneColumn = Providers.KitchenDisplayHeaderPaneLineColumnData.Get(PluginEntry.DataModel, headerPaneColumnID);

            tbDescription.Text = headerPaneColumn.Text;
            cmbColumnType.SelectedIndex = (int)headerPaneColumn.ColumnType;
            cmbColumnAlignment.SelectedIndex = (int)headerPaneColumn.ColumnAlignment;
            cmbStyle.SelectedData = headerPaneColumn.Style;
            btnsStyle.SetBuddyControl(cmbStyle);

            CheckEnabled(null, null);
        }

        public HeaderPaneColumnDialog(RecordIdentifier headerPaneLineID, RecordIdentifier headerPaneID)
            : this()
        {
            this.headerPaneLineID = headerPaneLineID;
            this.headerPaneID = headerPaneID;
        }

        public HeaderPaneColumnDialog()
        {
            InitializeComponent();
            cmbStyle.Tag = Guid.Empty;
            btnsStyle.SetBuddyControl(cmbStyle);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = cmbColumnType.Text.Length > 0 &&
                            cmbColumnAlignment.Text.Length > 0 &&
                            cmbStyle.Text.Length > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (headerPaneColumn == null)
            {
                headerPaneColumn = new LSOneHeaderPaneLineColumn();
                headerPaneColumn.ColumnNumber = Providers.KitchenDisplayHeaderPaneLineColumnData.GetNextColumnNumber(PluginEntry.DataModel, headerPaneLineID);
            }

            headerPaneColumn.Text = tbDescription.Text;
            headerPaneColumn.ColumnType = (HdrPnColumnTypeEnum)cmbColumnType.SelectedIndex;
            headerPaneColumn.ColumnAlignment = (HdrPnColumnAlignmentEnum)cmbColumnAlignment.SelectedIndex;
            headerPaneColumn.Style = (PosStyle)cmbStyle.SelectedData;
            headerPaneColumn.LineId = headerPaneLineID;
            headerPaneColumn.HeaderProfileId = headerPaneID;

            Providers.KitchenDisplayHeaderPaneLineColumnData.Save(PluginEntry.DataModel, headerPaneColumn);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenDisplayHeaderPaneColumn", headerPaneColumn.ID, null);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmb_RequestData(object sender, EventArgs e)
        {
            DualDataComboBox comboBox = (DualDataComboBox)sender;
            comboBox.SetData(Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME"), null);
        }
    }
}