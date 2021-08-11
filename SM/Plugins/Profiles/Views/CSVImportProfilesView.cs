using System;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.DataLayer.BusinessObjects.Enums;
using System.Windows.Forms;
using System.ComponentModel;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class CSVImportProfilesView : ViewBase
    {
        private RecordIdentifier selectedId = RecordIdentifier.Empty;

        private static Dictionary<ImportType, string> importTypeNames = new Dictionary<ImportType, string>
        {
            { ImportType.StockCounting, Properties.Resources.CSVImportTypeStockCounting }
        };

        public CSVImportProfilesView(RecordIdentifier selectedId)
            : this()
        {
            this.selectedId = selectedId;
        }

        public CSVImportProfilesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            btnSetProfileAsDefault.Enabled = false;

            lvCSVImportProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvCSVImportProfiles.ContextMenuStrip.Opening += lvCSVImportProfiles_Opening;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit);
        }

        void lvCSVImportProfiles_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvCSVImportProfiles.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            menu.Items.Add("-");

            item = new ExtendedMenuItem(
                btnSetProfileAsDefault.Text, 1500, btnSetProfileAsDefault_Click);
            item.Enabled = btnSetProfileAsDefault.Enabled;
            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("ImportProfile", selectedId, Properties.Resources.CSVImportProfiles, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.CSVImportProfiles;
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
            HeaderText = Properties.Resources.CSVImportProfiles;
            //HeaderIcon = Properties.Resources.Profiles16;

            LoadCSVImportProfiles();
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
                case "CSVImportProfile":
                    LoadCSVImportProfiles();
                    if (changeIdentifier != RecordIdentifier.Empty)
                    {
                        int selectedIndex = lvCSVImportProfiles.Rows.FindIndex(p => (RecordIdentifier)p.Tag == changeIdentifier);
                        lvCSVImportProfiles.Selection.AddRows(selectedIndex, selectedIndex);
                    }
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id = PluginOperations.NewCSVImportProfile();
            if (id != RecordIdentifier.Empty)
            {
                selectedId = id;
            }
        }

        private void lvVisualProfiles_SizeChanged(object sender, EventArgs e)
        {
            lvCSVImportProfiles.Columns[1].Width = -2;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowCSVImportProfileSheet(this, selectedId);
        }

        private void lvVisualProfiles_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteCSVImportProfile(selectedId);
        }

        private void LoadCSVImportProfiles()
        {
            lvCSVImportProfiles.ClearRows();

            List<ImportProfile> profiles = Providers.ImportProfileData.GetSelectList(PluginEntry.DataModel);
            foreach (ImportProfile importProfile in profiles)
            {
                Row row = new Row();
                row.Tag = importProfile.MasterID;

                row.AddCell(new Cell((string)importProfile.ID));
                row.AddCell(new Cell(importProfile.Description));
                row.AddCell(new Cell(importProfile.ImportTypeString));
                row.AddCell(new CheckBoxCell(importProfile.HasHeaders, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));

                if (importProfile.Default == 1)
                {
                    row.BackColor = System.Drawing.Color.SkyBlue;
                }

                lvCSVImportProfiles.AddRow(row);
            }

            lvCSVImportProfiles.Sort();
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Add, ContextButtons.GetAddButtonImage(), btnAdd_Click), 10);
                }
            }
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void lvCSVImportProfiles_SelectionChanged(object sender, EventArgs e)
        {
            bool exactlyOneitemSelected = (lvCSVImportProfiles.Selection.Count == 1);
            bool hasPermissionForManageImportProfiles = PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles);

            selectedId = exactlyOneitemSelected ? (RecordIdentifier)lvCSVImportProfiles.Selection[0].Tag : RecordIdentifier.Empty;
            btnsContextButtons.EditButtonEnabled = exactlyOneitemSelected && hasPermissionForManageImportProfiles;
            btnsContextButtons.RemoveButtonEnabled = exactlyOneitemSelected && hasPermissionForManageImportProfiles;

            btnSetProfileAsDefault.Enabled = exactlyOneitemSelected
                && (lvCSVImportProfiles.Selection[0].BackColor != System.Drawing.Color.SkyBlue);
        }

        private void lvCSVImportProfiles_DoubleClick(object sender, EventArgs e)
        {
            if (lvCSVImportProfiles.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnSetProfileAsDefault_Click(object sender, EventArgs e)
        {
            PluginOperations.SetDefaultCSVImportProfile(selectedId);
        }
    }
}
