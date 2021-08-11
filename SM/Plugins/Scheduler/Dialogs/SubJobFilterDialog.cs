using System;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.Utils;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class SubJobFilterDialog : DialogBase
    {
        private JscSubJobFromTableFilter fromTableFilter;

        public SubJobFilterDialog()
        {
            InitializeComponent();

            ComboUtils.PopulateComboBoxItems<LookupCode>(cmbFilterType, Properties.Resources.ResourceManager, lookcode => (lookcode != LookupCode.Count && lookcode != LookupCode.Max && lookcode != LookupCode.Min && lookcode != LookupCode.None));
            tbValue1.Enabled = false;
            tbValue2.Enabled = false;
        }
        
        public JscSubJobFromTableFilter FromTableFilter
        {
            get { return fromTableFilter; }
            set
            {
                fromTableFilter = value;
                ItemToForm();
            }
        }

        private void ItemToForm()
        {
            int selectedIndex = -1;
            cmbFieldDesign.Items.Clear();
            var tableFromGuid = fromTableFilter.JscSubJob.TableFrom;
            if (tableFromGuid != null && !tableFromGuid.IsEmpty)
            {
                cmbFieldDesign.Enabled = true;

                var fields = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetFieldDesignsOrderedBySequence(PluginEntry.DataModel, tableFromGuid);
                foreach (var field in fields)
                {
                    var index = cmbFieldDesign.Items.Add(new DataSelector {Object = field, Text = field.FieldName});
                    if (field.ID == fromTableFilter.Field)
                    {
                        selectedIndex = index;
                    }
                }
            }
            else
            {

                cmbFieldDesign.Enabled = false;
            }
            cmbFieldDesign.SelectedIndex = selectedIndex;
            ComboUtils.SetComboSelection(cmbFilterType, (int)fromTableFilter.FilterType);
            tbValue1.Text = fromTableFilter.Value1;
            tbValue2.Text = fromTableFilter.Value2;
            ComboUtils.SetComboSelection(cmbApplyFilter, (int)fromTableFilter.ApplyFilter);

            UpdateActions();
        }

        private void ItemFromForm()
        {
            var fieldSelector = cmbFieldDesign.SelectedItem as DataSelector;
            if (fieldSelector == null)
            {
                throw new InvalidOperationException("No field selected");
            }

            fromTableFilter.JscField = (JscFieldDesign)fieldSelector.Object;
            fromTableFilter.FilterType = (LookupCode)ComboUtils.GetComboSelectionInt(cmbFilterType);
            fromTableFilter.Value1 = tbValue1.Text;
            fromTableFilter.Value2 = tbValue2.Text;
            fromTableFilter.ApplyFilter = (LinkedFilterType)ComboUtils.GetComboSelectionInt(cmbApplyFilter);
        }

        private void cmbFieldDesign_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void UpdateActions()
        {
            btnOK.Enabled = cmbFieldDesign.SelectedItem != null;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ItemFromForm();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void cmbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LookupCode code = (LookupCode)ComboUtils.GetComboSelectionInt(cmbFilterType);
            switch (code)
            {
                case LookupCode.Equal:
                    ComboUtils.PopulateComboBoxItems<LinkedFilterType>(cmbApplyFilter, Properties.Resources.ResourceManager, ltype => (ltype != LinkedFilterType.Field && ltype != LinkedFilterType.Filter && ltype != LinkedFilterType.FromLoc && ltype != LinkedFilterType.ToLoc));
                    break;
                case LookupCode.Higher:
                case LookupCode.Less:
                    ComboUtils.PopulateComboBoxItems<LinkedFilterType>(cmbApplyFilter, Properties.Resources.ResourceManager, ltype => (ltype == LinkedFilterType.Constant || ltype == LinkedFilterType.DateFormula));
                    break;
                case LookupCode.Filter:
                case LookupCode.Like:
                case LookupCode.Range:
                    ComboUtils.PopulateComboBoxItems<LinkedFilterType>(cmbApplyFilter, Properties.Resources.ResourceManager, ltype => (ltype == LinkedFilterType.Constant));
                    break;
            }
            cmbApplyFilter.SelectedIndex = 0;
        }

        private void cmbApplyFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            LookupCode code = (LookupCode)ComboUtils.GetComboSelectionInt(cmbFilterType);
            LinkedFilterType ltype = (LinkedFilterType)ComboUtils.GetComboSelectionInt(cmbApplyFilter);

            tbValue1.Enabled = true;
            tbValue2.Enabled = false;
            tbValue2.Text = string.Empty;

            if (code == LookupCode.Range)
            {
                tbValue2.Enabled = true;
            }
            if (ltype == LinkedFilterType.DateFormula)
            {
                if (tbValue1.Text == string.Empty)
                    tbValue1.Text = "D-xx";
            }
            if (ltype == LinkedFilterType.FromLoc || ltype == LinkedFilterType.ToLoc)
            {
                tbValue1.Text = string.Empty;
                tbValue1.Enabled = false;
            }
        }
    }
}
