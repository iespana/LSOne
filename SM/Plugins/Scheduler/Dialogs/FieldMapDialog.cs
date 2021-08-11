using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class FieldMapDialog : DialogBase
    {
        private JscFieldMap currentFieldMap;
        private bool dataChanged;

        public FieldMapDialog()
        {
            InitializeComponent();
            PopulateConversionMethods();
        }

        private void PopulateConversionMethods()
        {
            cmbConversionMethod.Items.Clear();
            var conversionMethodArray = Enum.GetValues(typeof(ConversionMethod));
            for (int i = 0; i < conversionMethodArray.Length; i++)
            {
                int value = Convert.ToInt32(conversionMethodArray.GetValue(i));
                if (Utils.Utils.EnumResourceString<ConversionMethod>(
                    Properties.Resources.ResourceManager,
                    (ConversionMethod) value) != null)
                {
                    
                    cmbConversionMethod.Items.Add
                        (
                            new DataSelector
                            {
                                IntId = value,
                                Text =
                                    Utils.Utils.EnumResourceString<ConversionMethod>(
                                        Properties.Resources.ResourceManager,
                                        (ConversionMethod) value)
                            }
                        );
                }
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, JscFieldMap fieldMap, Guid fromTableDesign, Guid toTableDesign)
        {

            AssertFieldsLoaded(cmbFromField, fromTableDesign);
            AssertFieldsLoaded(cmbToField, toTableDesign);

            currentFieldMap = fieldMap;
            if (currentFieldMap != null)
            {
                FieldMapToForm(currentFieldMap);
                Text = Properties.Resources.FieldMapEdit;
            }
            else
            {
                ClearForm();
                Text = Properties.Resources.FieldMapNew;
            }

            dataChanged = false;

            cmbFromField.Focus();

            UpdateActions();

            return ShowDialog(owner);
        }

        private void AssertFieldsLoaded(ComboBox comboBox, Guid tableDesignId)
        {
            if (comboBox.Tag == null || (Guid)comboBox.Tag != tableDesignId)
            {
                comboBox.Items.Clear();
            }

            if (comboBox.Items.Count == 0)
            {
                foreach (var field in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesignsOrderedByFieldName(PluginEntry.DataModel, tableDesignId))
                {
                    comboBox.Items.Add(new DataSelector { GuidId = (Guid) field.ID, Text = field.FieldName, Object = field });
                }

                comboBox.Tag = tableDesignId;
            }
        }

        private void FieldMapToForm(JscFieldMap fieldMap)
        {
            SetComboSelectedFieldItem(cmbFromField, fieldMap.FromJscFieldDesign);
            SetComboSelectedFieldItem(cmbToField, fieldMap.ToJscFieldDesign);
            ComboUtils.SetComboSelection(cmbConversionMethod, (int)fieldMap.ConversionMethod);
            tbConversionValue.Text = fieldMap.ConversionValue;

            btnOK.Enabled = false;
        }

        private void FieldMapFromForm(JscFieldMap fieldMap)
        {
            fieldMap.FromJscFieldDesign = GetComboSelectedFieldItem(cmbFromField);
            fieldMap.ToJscFieldDesign = GetComboSelectedFieldItem(cmbToField);
            fieldMap.ConversionMethod = (ConversionMethod)ComboUtils.GetComboSelectionInt(cmbConversionMethod);
            fieldMap.ConversionValue = tbConversionValue.Text;
        }

        private void ClearForm()
        {
            cmbFromField.SelectedIndex = -1;
            cmbToField.SelectedIndex = -1;
            cmbConversionMethod.SelectedIndex = -1;
            tbConversionValue.Clear();
        }

        private void cmbFromField_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataChanged = true;
            UpdateActions();
        }

        private void cmbToField_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataChanged = true;
            UpdateActions();
        }

        private void cmbConversionMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataChanged = true;
            UpdateActions();
        }

        private void tbConversionValue_TextChanged(object sender, EventArgs e)
        {
            dataChanged = true;
            UpdateActions();
        }

        private void UpdateActions()
        {
            btnOK.Enabled =
                dataChanged &&
                cmbFromField.SelectedIndex >= 0 &&
                cmbToField.SelectedIndex >= 0 &&
                cmbConversionMethod.SelectedIndex >= 0;
        }

        private void SetComboSelectedFieldItem(ComboBox cmb, JscFieldDesign fieldDesign)
        {
            if (fieldDesign != null)
            {
                for (int i = 0; i < cmb.Items.Count; i++)
                {
                    DataSelector selector = cmb.Items[i] as DataSelector;
                    if (((JscFieldDesign)selector.Object).ID == fieldDesign.ID)
                    {
                        cmb.SelectedIndex = i;
                        return;
                    }
                }
            }

            cmb.SelectedIndex = -1;
        }

        private JscFieldDesign GetComboSelectedFieldItem(ComboBox cmb)
        {
            return (JscFieldDesign)((DataSelector)cmb.SelectedItem).Object;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (currentFieldMap == null)
            {
                currentFieldMap = new JscFieldMap();
            }

            FieldMapFromForm(currentFieldMap);

            DialogResult = DialogResult.OK;
        }

        public JscFieldMap FieldMap
        {
            get { return currentFieldMap; }
        }

    }
}
