using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System.Collections.Generic;
using System.Linq;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class UnitDialog : DialogBase
    {
        Unit unit;
        bool manuallyEnterId;

        public UnitDialog(RecordIdentifier selectedID)
            : this()
        {
            unit = Providers.UnitData.Get(PluginEntry.DataModel, selectedID);

            tbID.Text = (string)unit.ID;
            tbID.ReadOnly = true;
            tbID.BackColor = ColorPalette.BackgroundColor;
			tbID.ForeColor = ColorPalette.DisabledTextColor;
            tbDescription.Text = unit.Text;
            ntbMinimumDecimals.Value = unit.MinimumDecimals;
            ntbMaximumDecimals.Value = unit.MaximumDecimals;
        }

        public UnitDialog()
            : base()
        {
            unit = null;

            InitializeComponent();

            var parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
            manuallyEnterId = parameters.ManuallyEnterUnitID;

            tbID.Visible = manuallyEnterId;
            lblID.Visible = manuallyEnterId;
        }

        public RecordIdentifier UnitID
        {
            get { return unit != null ? unit.ID : RecordIdentifier.Empty; }
        }

        public Unit Unit
        {
            get { return unit; }
        }


        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            if ((int)ntbMaximumDecimals.Value < (int)ntbMinimumDecimals.Value)
            {
                errorProvider1.SetError(ntbMinimumDecimals, Properties.Resources.MinCannotBeHigherThanMax);
                return;
            }
            
            if(unit == null)
            {
                unit = new Unit();
            }

            bool IsValid = true;

            if (manuallyEnterId)
            {
                if (tbID.Text.Trim() == "")
                {
                    System.Windows.Forms.DialogResult blnDialogResult = QuestionDialog.Show(Properties.Resources.IDMissingQuestion, Properties.Resources.IDMissing);
                    if (blnDialogResult != System.Windows.Forms.DialogResult.Yes)
                    {
                        IsValid = false;
                    }
                    else if (blnDialogResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        IsValid = ValidationIDandDescription(IsValid, true);
                    }
                }
                else
                {
                    IsValid = ValidationIDandDescription(IsValid, false);
                }
            }
            else 
            {
                IsValid = ValidationIDandDescription(IsValid, true);
            }

            if (IsValid)
            {
                unit.Text = tbDescription.Text;
                unit.MaximumDecimals = (int)ntbMaximumDecimals.Value;
                unit.MinimumDecimals = (int)ntbMinimumDecimals.Value;

                Providers.UnitData.Save(PluginEntry.DataModel, unit);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (unit == null)
            {
                btnOK.Enabled = tbDescription.Text != "";
            }
            else
            {
                btnOK.Enabled = tbDescription.Text != "" && (tbDescription.Text != unit.Text || ntbMaximumDecimals.Value != unit.MaximumDecimals || ntbMinimumDecimals.Value != unit.MinimumDecimals) ;
            }

            errorProvider1.Clear();
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckEnabled(this, EventArgs.Empty);
        }

        private bool ValidationIDandDescription(bool IsValid, bool blnDialogBox)
        {
            if (!tbID.ReadOnly)
            {
                if (!tbID.Text.IsAlphaNumeric())
                {
                    errorProvider1.SetError(tbID, Properties.Resources.OnlyCharAndNumbers);
                    IsValid = false;
                }
            }

            if (!blnDialogBox)
            {
                unit.ID = tbID.Text.Trim();
            }

            if (!tbID.ReadOnly)
            {
                if (Providers.UnitData.Exists(PluginEntry.DataModel, unit.ID))
                {
                    errorProvider1.SetError(tbID, Properties.Resources.UnitIDExists);
                    IsValid = false;
                }
            }
            
            if (IsValid && !unit.Text.Equals(tbDescription.Text, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Providers.UnitData.UnitDescriptionExists(PluginEntry.DataModel, tbDescription.Text))
                {
                    errorProvider1.SetError(tbDescription, Properties.Resources.UnitDescExists);
                    IsValid = false;                    
                }
            }
            return IsValid;
        }
        
    }
}
