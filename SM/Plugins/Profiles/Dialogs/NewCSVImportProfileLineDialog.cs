using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.ViewPlugins.Profiles.Views;
using System.Collections.Generic;
using System.Linq;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Profiles.Datalayer;

namespace LSOne.ViewPlugins.Profiles.Dialogs
{
    public partial class NewCSVImportProfileLineDialog : DialogBase
    {
        private RecordIdentifier lineId = "";

        private readonly RecordIdentifier parentImportProfileId;

        private int newSequenceValue = 0;

        public NewCSVImportProfileLineDialog(RecordIdentifier parentImportProfileId, RecordIdentifier lineId)
            : this()
        {
            bool isEdit = (lineId != RecordIdentifier.Empty);
            string selectedField = "";

            if (isEdit)
            {
                ImportProfileLine importProfileLine = Providers.ImportProfileLineData.Get(PluginEntry.DataModel, lineId);
                this.parentImportProfileId = importProfileLine.ImportProfileMasterId;
                selectedField = CSVImportProfileView.FieldNames.FirstOrDefault(el => el.Field == importProfileLine.Field).FieldName;
                chkCreateAnother.Checked = false;
                chkCreateAnother.Visible = false;
                newSequenceValue = importProfileLine.Sequence;
            }
            else
            {
                this.parentImportProfileId = parentImportProfileId;
                chkCreateAnother.Checked = true;
            }
            this.lineId = lineId;

            if (isEdit)
            {
                FillComboValues(selectedField);
                cmbField.SelectedItem = selectedField;
            }
            else
            {
                FillComboValues();
            }
        }

        private void FillComboValues(string currentEditSelection = "")
        {
            cmbField.Items.Clear();
            HashSet<Field> unavailableFields;
            if (parentImportProfileId != RecordIdentifier.Empty)
            {
                unavailableFields = new HashSet<Field>(
                    Providers.ImportProfileLineData.GetUnavailableFieldList(PluginEntry.DataModel, parentImportProfileId));
            }
            else
            {
                unavailableFields = new HashSet<Field>();
            }
            foreach (var fieldName in CSVImportProfileView.FieldNames)
            {
                if (!unavailableFields.Contains(fieldName.Field) || fieldName.FieldName == currentEditSelection)
                {
                    cmbField.Items.Add(fieldName.FieldName);
                }
            }
        }

        public NewCSVImportProfileLineDialog()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SaveImportProfileLine();
            if (chkCreateAnother.Checked)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "CSVImportProfile", parentImportProfileId, null);
                lineId = RecordIdentifier.Empty;

                cmbField.Items.Remove(cmbField.SelectedItem);
                cmbField.SelectedIndex = -1;
                cmbField.Focus();
                ToggleOKButton();
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void SaveImportProfileLine()
        {
            if (lineId == RecordIdentifier.Empty)
            {
                newSequenceValue =
                    Providers.ImportProfileLineData.GetSelectList(PluginEntry.DataModel, parentImportProfileId)
                    .Select(a => a.Sequence)
                    .DefaultIfEmpty(0)
                    .Max()
                    + 10;
            }

            FieldItem selectedFieldItem = CSVImportProfileView.FieldNames.FirstOrDefault(x => x.FieldName == (string)cmbField.SelectedItem);
            ImportProfileLine importProfileLine = new ImportProfileLine
            {
                Field = selectedFieldItem.Field,
                FieldType = selectedFieldItem.FieldType,
                ImportProfileMasterId = parentImportProfileId,
                Sequence = newSequenceValue,
                MasterId = lineId
            };

            Providers.ImportProfileLineData.Save(PluginEntry.DataModel, importProfileLine);
            lineId = importProfileLine.MasterId;
        }

        public RecordIdentifier ProfileID
        {
            get { return lineId; }
        }

        private void cmbField_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleOKButton();
        }

        private void cmbFieldType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleOKButton();
        }

        private void ToggleOKButton()
        {
            btnOK.Enabled = (cmbField.SelectedIndex != -1);
        }
    }
}
