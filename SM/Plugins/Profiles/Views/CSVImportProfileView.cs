using System.Collections.Generic;
using LSOne.Controls.ScreenIdentity;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.ViewPlugins.Profiles.Datalayer;
using System.Linq;
using LSOne.ViewPlugins.Profiles.Properties;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class CSVImportProfileView : ViewBase
    {
        private readonly RecordIdentifier parentProfileId;

        private RecordIdentifier selectedId = RecordIdentifier.Empty;

        private readonly ImportProfile importProfile;

        public static List<FieldItem> FieldNames = new List<FieldItem>();

        private bool initialInitialize;

        public CSVImportProfileView(RecordIdentifier profileID)
            : this()
        {
            this.parentProfileId = profileID;
            importProfile = Providers.ImportProfileData.Get(PluginEntry.DataModel, profileID);

            LoadFields();

            cmbImportType.DisplayMember = "Text";
            cmbImportType.ValueMember = "Value";
            cmbImportType.Items.Add(new { Text = ImportTypeHelper.GetImportTypeString(importProfile.ImportType), Value = (int)importProfile.ImportType });
            cmbImportType.SelectedIndex = 0;

            chkHasHeaders.Checked = importProfile.HasHeaders;
        }

        private CSVImportProfileView()
        {
            InitializeComponent();

            lvCSVImportProfileLines.ContextMenuStrip = new ContextMenuStrip();
            lvCSVImportProfileLines.ContextMenuStrip.Opening += lvCSVImportProfileLines_Opening;

            Attributes =
                ViewAttributes.Close |
                ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Help |
                ViewAttributes.ContextBar;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles);
        }

        void lvCSVImportProfileLines_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvCSVImportProfileLines.ContextMenuStrip;

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

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void LoadData(bool isRevert)
        {
            if (!initialInitialize)
            {
                AddParentViewDescriptor(new ParentViewDescriptor(
                        parentProfileId,
                        Properties.Resources.CSVImportProfiles,
                        null, //Properties.Resources.Profiles16,
                        PluginOperations.ShowCSVImportProfilesSheet));

                initialInitialize = true;
            }

            HeaderText = Description;
            //HeaderIcon = Properties.Resources.Profiles16;

            tbDescription.Text = importProfile.Description;

            LoadCSVImportProfileLines();

            lvCSVImportProfileLines_SelectionChanged(this, EventArgs.Empty);
        }

        private void LoadCSVImportProfileLines()
        {
            lvCSVImportProfileLines.ClearRows();

            List<ImportProfileLine> profileLines = Providers.ImportProfileLineData.GetSelectList(PluginEntry.DataModel, parentProfileId);
            foreach (ImportProfileLine importProfileLine in profileLines)
            {
                Row row = new Row();
                row.Tag = importProfileLine.MasterId;

                string fieldName = "";
                fieldName = FieldNames.FirstOrDefault(el => el.Field == importProfileLine.Field).FieldName;
                row.AddCell(new Cell(fieldName));

                string fieldTypeName = "";
                fieldTypeName = FieldNames.FirstOrDefault(el => el.Field == importProfileLine.Field).FieldTypeName;
                row.AddCell(new Cell(fieldTypeName));

                lvCSVImportProfileLines.AddRow(row);
            }
        }

        protected override bool DataIsModified()
        {
            if ((tbDescription.Text != importProfile.Description) || (chkHasHeaders.Checked != importProfile.HasHeaders)) return true;
            return false;
        }

        protected override bool SaveData()
        {
            importProfile.Description = tbDescription.Text;
            importProfile.HasHeaders = chkHasHeaders.Checked;

            Providers.ImportProfileData.Save(PluginEntry.DataModel, importProfile);

            PluginEntry.Framework.ViewController.NotifyDataChanged(
                this,
                DataEntityChangeType.Edit,
                "CSVImportProfile",
                importProfile.MasterID,
                null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "CSVImportProfile":
                    LoadCSVImportProfileLines();
                    if (changeIdentifier != RecordIdentifier.Empty)
                    {
                        int selectedIndex = lvCSVImportProfileLines.Rows.FindIndex(p => (RecordIdentifier)p.Tag == changeIdentifier);
                        lvCSVImportProfileLines.Selection.AddRows(selectedIndex, selectedIndex);
                    }
                    break;
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("ImportProfileLines", selectedId, Properties.Resources.CSVImportProfile, true));
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteCSVImportProfile(parentProfileId);
        }

        public override RecordIdentifier ID
        {
            get
            {
                // If our sheet would be multi-instance sheet then we would return context identifier UUID here,
                // such as User.GUID that identifies that particular User. For single instance sheets we return 
                // RecordIdentifier.Empty to tell the framework that there can only be one instace of this sheet, which will
                // make the framework make sure there is only one instance in the viewstack.
                return parentProfileId;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.CSVImportProfile;
            }
        }

        private string Description
        {
            get
            {
                return Properties.Resources.CSVImportProfile +
                    (importProfile == null ? "" : ": " + importProfile.Description);
            }
        }

        private void lvCSVImportProfileLines_SelectionChanged(object sender, System.EventArgs e)
        {
            bool exactlyOneitemSelected = (lvCSVImportProfileLines.Selection.Count == 1);
            bool hasPermissionForManageImportProfiles = PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles);

            selectedId = exactlyOneitemSelected ? (RecordIdentifier)lvCSVImportProfileLines.Selection[0].Tag : RecordIdentifier.Empty;
            btnsContextButtons.EditButtonEnabled = exactlyOneitemSelected && hasPermissionForManageImportProfiles;
            btnsContextButtons.RemoveButtonEnabled = exactlyOneitemSelected && hasPermissionForManageImportProfiles;

            contextButtonMoveUp.Enabled = (exactlyOneitemSelected) && (lvCSVImportProfileLines.Selection.FirstSelectedRow != 0);
            contextButtonMoveDown.Enabled = (exactlyOneitemSelected) && (lvCSVImportProfileLines.Selection.FirstSelectedRow != lvCSVImportProfileLines.RowCount - 1);
        }

        private void btnsContextButtons_RemoveButtonClicked(object sender, System.EventArgs e)
        {
            PluginOperations.DeleteCSVImportProfileLine(selectedId);
        }

        private void btnsContextButtons_AddButtonClicked(object sender, System.EventArgs e)
        {
            RecordIdentifier id;
            id = PluginOperations.NewCSVImportProfileLine(parentProfileId);
            if (id != RecordIdentifier.Empty)
            {
                selectedId = id;
            }
        }

        private void btnsContextButtons_EditButtonClicked(object sender, System.EventArgs e)
        {
            PluginOperations.EditCSVImportProfileLine(selectedId);
        }

        private void contextButtonMoveUp_Click(object sender, System.EventArgs e)
        {
            SwapCSVImportLines(
                lvCSVImportProfileLines.Selection.FirstSelectedRow,
                lvCSVImportProfileLines.Selection.FirstSelectedRow - 1);
        }

        private void contextButtonMoveDown_Click(object sender, System.EventArgs e)
        {
            SwapCSVImportLines(
                lvCSVImportProfileLines.Selection.FirstSelectedRow,
                lvCSVImportProfileLines.Selection.FirstSelectedRow + 1);
        }

        private void SwapCSVImportLines(int firstRowIndex, int secondRowIndex)
        {
            lvCSVImportProfileLines.SwapRows(firstRowIndex, secondRowIndex);
            lvCSVImportProfileLines_SelectionChanged(this, EventArgs.Empty);

            Providers.ImportProfileLineData.SwapSequenceValues(PluginEntry.DataModel,
                (RecordIdentifier)lvCSVImportProfileLines.Row(firstRowIndex).Tag,
                (RecordIdentifier)lvCSVImportProfileLines.Row(secondRowIndex).Tag);
        }

        private void lvCSVImportProfileLines_DoubleClick(object sender, EventArgs e)
        {
            if (lvCSVImportProfileLines.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
            {
                btnsContextButtons_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void LoadFields()
        {
            FieldNames = new List<FieldItem>();
            switch (importProfile.ImportType)
            {
                case ImportType.StockCounting:
                    FieldNames.Add(new FieldItem() { Field = Field.Barcode, FieldName = Resources.CSVFieldBarcode, FieldType = FieldType.String, FieldTypeName = Resources.CSVFieldTypeString });
                    FieldNames.Add(new FieldItem() { Field = Field.ItemId, FieldName = Resources.CSVFieldItemId, FieldType = FieldType.String, FieldTypeName = Resources.CSVFieldTypeString });
                    FieldNames.Add(new FieldItem() { Field = Field.UnitId, FieldName = Resources.CSVFieldUnitId, FieldType = FieldType.String, FieldTypeName = Resources.CSVFieldTypeString });
                    FieldNames.Add(new FieldItem() { Field = Field.Counted, FieldName = Resources.CSVFieldCounted, FieldType = FieldType.Decimal, FieldTypeName = Resources.CSVFieldTypeDecimal });
                    FieldNames.Add(new FieldItem() { Field = Field.Description, FieldName = Resources.CSVFieldDescription, FieldType = FieldType.String, FieldTypeName = Resources.CSVFieldTypeString });
                    break;
                case ImportType.SerialNumbers:
                    FieldNames.Add(new FieldItem() { Field = Field.ItemId, FieldName = Resources.CSVFieldItemId, FieldType = FieldType.String, FieldTypeName = Resources.CSVFieldTypeString });
                    FieldNames.Add(new FieldItem() { Field = Field.SerialNumber, FieldName = Resources.CSVFieldSerialNumber, FieldType = FieldType.String, FieldTypeName = Resources.CSVFieldTypeString });
                    FieldNames.Add(new FieldItem() { Field = Field.TypeOfSerial, FieldName = Resources.CSVFieldSerialType, FieldType = FieldType.Integer, FieldTypeName = Resources.CSVFieldTypeInteger });
                    break;
                default:
                    break;
            }
        }
    }
}
