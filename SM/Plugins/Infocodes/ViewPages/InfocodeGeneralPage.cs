using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Infocodes.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    public partial class InfocodeGeneralPage : UserControl, ITabView
    {
        private Infocode infocode;
        private List<DataEntity> typeOptions;
        private IDataEntity lastSelectedType;

        public InfocodeGeneralPage()
        {
            InitializeComponent();

            typeOptions = CreateTypeOptions();
            lastSelectedType = new DataEntity();
        }

        public List<DataEntity> CreateTypeOptions()
        {
            typeOptions = new List<DataEntity>();
            typeOptions.Add(new DataEntity((int)InputTypesEnum.Numeric, Resources.NumericInput));
            typeOptions.Add(new DataEntity((int)InputTypesEnum.Text, Resources.TextInput));
            typeOptions.Add(new DataEntity((int)InputTypesEnum.Date, Resources.DateInput));
            typeOptions.Add(new DataEntity((int)InputTypesEnum.Item, Resources.ItemSelection));
            typeOptions.Add(new DataEntity((int)InputTypesEnum.Customer, Resources.CustomerSelection));
            typeOptions.Add(new DataEntity((int)InputTypesEnum.AgeLimit, Resources.AgeLimit));
            typeOptions.Add(new DataEntity((int)InputTypesEnum.SubCodeList, Resources.SelectionList));
            typeOptions.Add(new DataEntity((int)InputTypesEnum.SubCodeButtons, Resources.SelectionButtons));

            return typeOptions;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.InfocodeGeneralPage();
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            if (tbDescription.Text != infocode.Text) return true;
            if (tbPrompt.Text != infocode.Prompt) return true;
            if (GetCurrentTypeSelection() != infocode.InputType) return true;
            if (cmbTriggering.SelectedIndex != (int)infocode.Triggering) return true;
            if (chkInputRequired.Checked != infocode.InputRequired) return true;
            if (chkOncePerTransaction.Checked != infocode.OncePerTransaction) return true;
            if (cmbLinkedInfocode.SelectedData.ID != infocode.LinkedInfocodeId) return true;
            if (ntbRandomFactor.Value != (double)infocode.RandomFactor) return true;
            if ((ntbMinLength.Value != infocode.MinimumLength) && ntbMinLength.Visible) return true;
            if ((ntbMaxLength.Value != infocode.MaximumLength) && ntbMaxLength.Visible) return true;
            if ((ntbMinValue.Value != (double)infocode.MinimumValue) && ntbMinValue.Visible) return true;
            if ((ntbMaxValue.Value != (double)infocode.MaximumValue) && ntbMaxValue.Visible) return true;
            if ((ntbMinSelection.Value != infocode.MinSelection) && ntbMinSelection.Visible) return true;
            if ((ntbMaxSelection.Value != infocode.MaxSelection) && ntbMaxSelection.Visible) return true;

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            infocode = (Infocode)internalContext;

            tbID.Text = infocode.ID.ToString();
            tbDescription.Text = infocode.Text;
            tbPrompt.Text = infocode.Prompt;

            DataEntity selectedType = typeOptions.FirstOrDefault(f => f.ID == (int) infocode.InputType) ?? new DataEntity((int)InputTypesEnum.SubCodeList, Resources.SelectionList);
            cmbType.SelectedData = selectedType;
            lastSelectedType = cmbType.SelectedData;

            cmbTriggering.SelectedIndex = (int)infocode.Triggering;
            chkInputRequired.Checked = infocode.InputRequired;
            chkOncePerTransaction.Checked = infocode.OncePerTransaction;
            Infocode linkedInfocde = Providers.InfocodeData.Get(PluginEntry.DataModel, infocode.LinkedInfocodeId);
            cmbLinkedInfocode.SelectedData = (linkedInfocde == null) ? new DataEntity("","") : new DataEntity(linkedInfocde.ID, linkedInfocde.Text);
            ntbRandomFactor.Value = (double)infocode.RandomFactor;
            ntbMinLength.Value = infocode.MinimumLength;
            ntbMaxLength.Value = infocode.MaximumLength;
            chkLinkedInfocode.Checked = (infocode.LinkedInfocodeId != null) && (infocode.LinkedInfocodeId != "");
            ntbMinValue.Value = (double)infocode.MinimumValue;
            ntbMaxValue.Value = (double)infocode.MaximumValue;
            ntbMinSelection.Value = infocode.MinSelection;
            ntbMaxSelection.Value = infocode.MaxSelection;
            SetUI();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            infocode.Text = tbDescription.Text;
            infocode.Prompt = tbPrompt.Text;

            DataEntity selectedType = typeOptions.FirstOrDefault(f => f.ID == (int)cmbType.SelectedDataID) ?? new DataEntity();
            infocode.InputType = (InputTypesEnum) (int) selectedType.ID;
            
            infocode.Triggering = (TriggeringEnum)cmbTriggering.SelectedIndex;
            infocode.InputRequired = chkInputRequired.Checked;
            infocode.OncePerTransaction = chkOncePerTransaction.Checked;
            infocode.LinkedInfocodeId = cmbLinkedInfocode.SelectedData.ID;
            infocode.RandomFactor = (decimal)ntbRandomFactor.Value;
            infocode.MinimumLength = (int)ntbMinLength.Value;
            infocode.MaximumLength = (int)ntbMaxLength.Value;
            infocode.MinimumValue = (decimal)ntbMinValue.Value;
            infocode.MaximumValue = (decimal)ntbMaxValue.Value;
            infocode.MinSelection = (int)ntbMinSelection.Value;
            infocode.MaxSelection = (int)ntbMaxSelection.Value;
            return true;
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void SetUI()
        {
            ntbMinLength.Visible = false;
            ntbMinLength.Enabled = true;

            ntbMaxLength.Visible = false;
            ntbMaxLength.Enabled = true;

            ntbMinValue.Visible = false;
            ntbMinValue.Enabled = true;

            ntbMaxValue.Visible = false;
            ntbMaxValue.Enabled = true;

            ntbMinSelection.Visible = false;
            ntbMinSelection.Enabled = true;

            ntbMaxSelection.Visible = false;
            ntbMaxSelection.Enabled = true;

            lblMinLength.Visible = true;
            lblMaxLength.Visible = true;

            DataEntity selectedType = typeOptions.FirstOrDefault(f => f.ID == (int)cmbType.SelectedDataID) ?? new DataEntity();

            if (selectedType.ID == (int)InputTypesEnum.Text)
            {
                lblMinLength.Text = Properties.Resources.MinimumLength;
                lblMaxLength.Text = Properties.Resources.MaximumLength;
                ntbMinLength.Visible = true;
                ntbMaxLength.Visible = true;
            }
            else if (selectedType.ID == (int)InputTypesEnum.Numeric)
            {
                lblMinLength.Text = Properties.Resources.MinimumValue;
                lblMaxLength.Text = Properties.Resources.MaximumValue;
                ntbMinValue.Visible = true;
                ntbMaxValue.Visible = true;
            }
            else if (selectedType.ID == (int)InputTypesEnum.AgeLimit)
            {
                lblMinLength.Text = Properties.Resources.MinimumValue;
                lblMaxLength.Text = Properties.Resources.MaximumValue;
                lblMaxLength.Visible = false;
                ntbMinValue.Visible = true;
            }
            else if (selectedType.ID == (int)InputTypesEnum.SubCodeButtons || selectedType.ID == (int)InputTypesEnum.SubCodeList)
            {
                lblMinLength.Text = Properties.Resources.MinimumSelection;
                lblMaxLength.Text = Properties.Resources.MaximumSelection;
                ntbMinSelection.Visible = true;
                ntbMaxSelection.Visible = true;
                ntbMaxSelection.AllowDecimal = true;
            }
            else
            {
                lblMinLength.Visible = false;
                lblMaxLength.Visible = false;
            }
            cmbLinkedInfocode.Enabled = chkLinkedInfocode.Checked;
        }

        private InputTypesEnum GetCurrentTypeSelection()
        {
            DataEntity selectedType = typeOptions.FirstOrDefault(f => f.ID == (int)cmbType.SelectedDataID) ?? new DataEntity();
            return (InputTypesEnum) (int)selectedType.ID;
        }

        private void cmbLinkedInfocode_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = ((DataEntity)e.Data).Text;
            }
        }

        private void cmbLinkedInfocode_RequestClear(object sender, EventArgs e)
        {
            //((DualDataComboBox)sender).SelectedData = new DataEntity("", ""); //TODO Properties.Resources.NoSelection);
            //cmbLinkedInfocode_SelectedDataChanged(this, EventArgs.Empty);
        }

        private void cmbLinkedInfocode_RequestData(object sender, EventArgs e)
        {
            cmbLinkedInfocode.SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new InputTypesEnum[]{ InputTypesEnum.Group}, false, RefTableEnum.All), null);
        }

        private void cmbLinkedInfocode_SelectedDataChanged(object sender, EventArgs e)
        {
            //TODO
        }

        private void chkLinkedInfocode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLinkedInfocode.Checked == false)
            {
                cmbLinkedInfocode.SelectedData = new DataEntity();
            }
            SetUI();
        }

        //private void rbSelectionButtons_CheckedChanged(object sender, EventArgs e)
        //{
        //    SetUI();
        //}

        private void cmbStatus_RequestData(object sender, EventArgs e)
        {
            cmbType.SetData(typeOptions, null);
        }

        private void cmbType_SelectedDataChanged(object sender, EventArgs e)
        {
            if (invalidInfoCodeInUse())
            {
                int selectedDataId = (int)cmbType.SelectedData.ID;
                if ((lastSelectedType.ID != cmbType.SelectedData.ID) && (selectedDataId == (int)InputTypesEnum.Item || selectedDataId == (int)InputTypesEnum.Customer))
                {
                    // Infocode being used by operation which does not support the input type. The type is set back to last selected type.
                    MessageDialog.Show(Properties.Resources.InfocodeSetToNotSupportedOperation + " " + Properties.Resources.InfocodeTypeSetBackToLast);
                    cmbType.SelectedData = lastSelectedType;
                }
            }
            SetUI();
            lastSelectedType = cmbType.SelectedData;
        }

        private bool invalidInfoCodeInUse()
        {
            // Operations to check for
            List<string> operations = new List<string>();
            operations.Add("OPENDRAWER");

            return Providers.InfocodeData.InfocodeInUseByOperation(PluginEntry.DataModel, infocode, operations);
        }
    }
}
