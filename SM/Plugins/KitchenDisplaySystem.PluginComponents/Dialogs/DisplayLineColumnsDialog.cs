using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.KDSBusinessObjects;
using DisplayModeEnum = LSOne.DataLayer.KDSBusinessObjects.KitchenDisplayStation.DisplayModeEnum;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class DisplayLineColumnsDialog : DialogBase
    {
        private RecordIdentifier displayLineColumnID;
        private RecordIdentifier displayLineID;
        private KitchenDisplayLineColumn displayLineColumn;
        private DisplayModeEnum displayMode;

        internal DisplayLineColumnsDialog(RecordIdentifier displayLineColumnID, RecordIdentifier displayLineID, DisplayModeEnum displayMode)
        {
            InitializeComponent();
            cmbColumnStyle.Tag = PluginEntry.ChitHeaderFooterPartGuid;
            this.displayLineColumnID = displayLineColumnID;
            this.displayLineID = displayLineID;
            this.displayMode = displayMode;

            if (displayMode == DisplayModeEnum.ChitDisplay)
            {
                lblCaption.Visible 
                    = tbCaption.Visible 
                    = false;
            }

            AddToComboBoxes();
            LoadData();
            SetOKButtonEnabled();
        }

        private void AddToComboBoxes()
        {
            cmbAlignment.Clear();
            cmbColumnStyle.Clear();
            btnsColumnStyle.SetBuddyControl(cmbColumnStyle);
        }

        private void LoadData ()
        {
            if (displayLineColumnID == null)
            {
                displayLineColumn = new KitchenDisplayLineColumn();
                displayLineColumn.LineNumberID = displayLineID;
                cmbAlignment.SelectedData = new DataEntity((int)PartAlignmentEnum.Automatic, KitchenDisplayHeaderFooterRowPart.GetPartAlignmentText(PartAlignmentEnum.Automatic));
                cmbDataType.SelectedData = KitchenDisplayColumnField.GetDefaultType();
                cmbDataType_SelectedDataChanged(this, null);
            }
            else
            {
                displayLineColumn = Providers.KitchenDisplayLineColumnData.Get(PluginEntry.DataModel, displayLineColumnID);

                tbCaption.Text = displayLineColumn.Caption;
                tbRelativeSize.Text = displayLineColumn.RelativeSize.ToString();
                cmbAlignment.SelectedData = new DataEntity((int)displayLineColumn.Alignment, KitchenDisplayHeaderFooterRowPart.GetPartAlignmentText(displayLineColumn.Alignment));

                if (displayLineColumn.StyleID != Guid.Empty)
                {
                    PosStyle style = Providers.PosStyleData.GetByGuid(PluginEntry.DataModel, displayLineColumn.StyleID);
                    displayLineColumn.Style = PosStyle.ToUIStyle(style);

                    cmbColumnStyle.SelectedData = style;
                    btnsColumnStyle.EditButtonEnabled = true;
                }

                KitchenDisplayColumnField fieldData = new KitchenDisplayColumnField(displayLineColumn);
                cmbDataType.SelectedData = new DataEntity((int)fieldData.Type, fieldData.GetDataTypeName());
                cmbFieldData.SelectedData = fieldData;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void OnControlValuesChanged(object sender, EventArgs e)
        {
            SetOKButtonEnabled();
        }

        private void SetOKButtonEnabled()
        {
            KitchenDisplayColumnField selectedFieldData = (KitchenDisplayColumnField)cmbFieldData.SelectedData;

            btnOK.Enabled = selectedFieldData != null &&
                            cmbFieldData.SelectedDataID != "" &&
                            tbCaption.Text.Length > 0 &&
                            tbRelativeSize.Text.Length > 0 &&
                            !RecordIdentifier.IsEmptyOrNull(cmbColumnStyle.SelectedDataID) &&
                            (   displayLineColumn.Type != selectedFieldData.KDSType ||
                                displayLineColumn.OrderProperty != selectedFieldData.KDSOrderProperty ||
                                displayLineColumn.MappingKey != selectedFieldData.KDSMappingKey ||
                                displayLineColumn.Caption != tbCaption.Text ||
                                displayLineColumn.Alignment != (PartAlignmentEnum)(int)cmbAlignment.SelectedDataID ||
                                displayLineColumn.RelativeSize != Convert.ToInt16(tbRelativeSize.Text) ||
                                displayLineColumn.StyleID != ((PosStyle)cmbColumnStyle.SelectedData).Guid   );
        }

        private bool Save()
        {
            displayLineColumn.Caption = tbCaption.Text;
            if (tbRelativeSize.Text.Length > 0)
            {
                displayLineColumn.RelativeSize = Convert.ToInt16(tbRelativeSize.Text);
            }
            displayLineColumn.Alignment = (PartAlignmentEnum)(int)cmbAlignment.SelectedDataID;
            displayLineColumn.StyleID = ((PosStyle)cmbColumnStyle.SelectedData).Guid;

            KitchenDisplayColumnField selectedFieldData = (KitchenDisplayColumnField)cmbFieldData.SelectedData;
            displayLineColumn.Type = selectedFieldData.KDSType;
            displayLineColumn.MappingKey = selectedFieldData.KDSMappingKey;
            displayLineColumn.OrderProperty = selectedFieldData.KDSOrderProperty;

            Providers.KitchenDisplayLineColumnData.Save(PluginEntry.DataModel, displayLineColumn);
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbColumnStyle_RequestData(object sender, EventArgs e)
        {
            var comboBox = (DualDataComboBox)sender;
            var styles = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME");
            comboBox.SetData(styles, null);
        }

        private void cmbDataType_RequestData(object sender, EventArgs e)
        {
            cmbDataType.SetData(KitchenDisplayColumnField.GetTypeList(), null);
        }

        private void cmbDataType_SelectedDataChanged(object sender, EventArgs e)
        {
            cmbFieldData.SelectedData = new KitchenDisplayColumnField();
            cmbFieldData_SelectedDataChanged(null, null);
        }

        private void cmbFieldData_RequestData(object sender, EventArgs e)
        {
            cmbFieldData.SetData(KitchenDisplayColumnField.GetList(cmbDataType.SelectedDataID, displayMode), null);
        }

        private void cmbFieldData_SelectedDataChanged(object sender, EventArgs e)
        {
            tbCaption.Text = cmbFieldData.SelectedData.Text;
            SetOKButtonEnabled();
        }

        private void cmbAlignment_RequestData(object sender, EventArgs e)
        {
            List<IDataEntity> data = new List<IDataEntity>();

            foreach (PartAlignmentEnum partAlignment in Enum.GetValues(typeof(PartAlignmentEnum)))
            {
                data.Add(new DataEntity((int)partAlignment, KitchenDisplayHeaderFooterRowPart.GetPartAlignmentText(partAlignment)));
            }

            cmbAlignment.SetData(data, null);
        }
    }
}
