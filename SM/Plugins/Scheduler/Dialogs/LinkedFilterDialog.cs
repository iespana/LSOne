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
    public partial class LinkedFilterDialog : DialogBase
    {
        private JscLinkedFilter currentLinkedFilter;
        private bool dataChanged;

        public LinkedFilterDialog()
        {
            InitializeComponent();

            ComboUtils.PopulateComboBoxItems<LinkedFilterType>(cmbLinkType, Properties.Resources.ResourceManager);
        }


        public DialogResult ShowDialog(IWin32Window owner, JscLinkedFilter linkedFilter, JscTableDesign fromTableDesign, JscTableDesign toTableDesign)
        {

            AssertFieldsLoaded(cmbFromField, (Guid)fromTableDesign.ID);
            AssertFieldsLoaded(cmbToField, (Guid)toTableDesign.ID);

            currentLinkedFilter = linkedFilter;
            if (currentLinkedFilter != null)
            {
                LinkedFilterToForm(currentLinkedFilter);
                Text = Properties.Resources.LinkedFilterEdit;
            }
            else
            {
                ClearForm();
                Text = Properties.Resources.LinkedFilterNew;
            }

            tbFromTable.Text = fromTableDesign.TableName;
            tbToTable.Text = toTableDesign.TableName;

            dataChanged = false;
            cmbFromField.Focus();
            UpdateActions();

            return ShowDialog(owner);
        }

        public JscLinkedFilter LinkedFilter
        {
            get { return currentLinkedFilter; }
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
                    comboBox.Items.Add(new DataSelector { GuidId = (Guid)field.ID, Text = field.FieldName, Object = field });
                }

                comboBox.Tag = tableDesignId;
            }
        }


        private void LinkedFilterToForm(JscLinkedFilter linkedFilter)
        {
            SetComboSelectedFieldItem(cmbFromField, linkedFilter.LinkedJscFieldDesign);
            SetComboSelectedFieldItem(cmbToField, linkedFilter.ToJscFieldDesign);
            //ComboUtils.SetComboSelection(cmbLinkType, (int)linkedFilter.LinkType);
            //tbFilter.Text = linkedFilter.Filter;

            btnOK.Enabled = false;
        }

        private void LinkedFilterFromForm(JscLinkedFilter linkedFilter)
        {
            linkedFilter.LinkedJscFieldDesign = GetComboSelectedFieldItem(cmbFromField);
            linkedFilter.ToJscFieldDesign = GetComboSelectedFieldItem(cmbToField);
            linkedFilter.LinkType = LinkedFilterType.Field;
            linkedFilter.Filter = null;
        }

        private void ClearForm()
        {
            cmbFromField.SelectedIndex = -1;
            cmbToField.SelectedIndex = -1;
            ComboUtils.SetComboSelection(cmbLinkType, (int)LinkedFilterType.Constant);
            tbFilter.Clear();
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

        private void cmbLinkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataChanged = true;
            UpdateActions();
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            dataChanged = true;
            UpdateActions();
        }

        private void UpdateActions()
        {
            btnOK.Enabled =
                cmbFromField.SelectedIndex >= 0 &&
                cmbToField.SelectedIndex >= 0 &&
                //cmbLinkType.SelectedIndex >= 0 &&
                dataChanged;

            //if (cmbLinkType.SelectedItem != null)
            //{
            //    var linkType = (LinkedFilterType)ComboUtils.GetComboSelectionInt(cmbLinkType);
            //    cmbFromField.Enabled = linkType == LinkedFilterType.Field;
            //    tbFilter.Enabled = linkType != LinkedFilterType.Field;
            //}
        }


        private void SetComboSelectedFieldItem(ComboBox cmb, JscFieldDesign fieldDesign)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                DataSelector selector = cmb.Items[i] as DataSelector;
                if (((JscFieldDesign)selector.Object).ID == fieldDesign.ID)
                {
                    cmb.SelectedIndex = i;
                    break;
                }
            }
        }


        private JscFieldDesign GetComboSelectedFieldItem(ComboBox cmb)
        {
            return (JscFieldDesign)((DataSelector)cmb.SelectedItem).Object;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (currentLinkedFilter == null)
            {
                currentLinkedFilter = new JscLinkedFilter();
            }

            LinkedFilterFromForm(currentLinkedFilter);

            DialogResult = DialogResult.OK;
        }
    }
}
