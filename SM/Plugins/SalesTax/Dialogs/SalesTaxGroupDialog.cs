using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SalesTax.Properties;

namespace LSOne.ViewPlugins.SalesTax.Dialogs
{
    public partial class SalesTaxGroupDialog : DialogBase
    {
        private SalesTaxGroup salesTaxGroup;
        private RecordIdentifier salesTaxGroupID = RecordIdentifier.Empty;
        private bool manuallyEnterId = false;

        // Edit a sales tax group
        public SalesTaxGroupDialog(RecordIdentifier taxCode)
            : this()
        {
            salesTaxGroup = Providers.SalesTaxGroupData.Get(PluginEntry.DataModel, taxCode);
            salesTaxGroupID = salesTaxGroup.ID;

            tbID.Text = (string)salesTaxGroup.ID;
            tbID.ReadOnly = true;
            tbID.BackColor = ColorPalette.BackgroundColor;
            tbID.ForeColor = ColorPalette.DisabledTextColor;
            tbDescription.Text = salesTaxGroup.Text;
            tbSearchField1.Text = salesTaxGroup.SearchField1;
            tbSearchField2.Text = salesTaxGroup.SearchField2;
            chkEU.Checked = salesTaxGroup.IsForEU;

            Text = Resources.EditSalesTaxGroup;
            Header = Resources.EditDescriptionAndRegionalSettingsForTheSelectedTaxGroup;
        }

        // New sales tax group
        public SalesTaxGroupDialog()
        {
            InitializeComponent();

            var parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
            manuallyEnterId = parameters.ManuallyEnterTaxGroupID;

            tbID.Visible = manuallyEnterId;
            lblID.Visible = manuallyEnterId;

            if(PluginEntry.Framework.FindImplementor(null, "SAPBusinessOne", null) != null)
            {
                lblEU.Visible = chkEU.Visible = true;
            }
        }

        public RecordIdentifier SalesTaxGroupID
        {
            get { return salesTaxGroup != null ? salesTaxGroup.ID : RecordIdentifier.Empty; }
        }

        public SalesTaxGroup GetSalesTaxGroup
        {
            get { return salesTaxGroup; }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckChanged(this, EventArgs.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            bool isNew = false;
            bool IsValid = true;

            if (manuallyEnterId)
            {
                if (tbID.Text.Trim() == "")
                {
                    DialogResult blnDialogResult = QuestionDialog.Show(Resources.IDMissingQuestion, Resources.IDMissing);
                    if (blnDialogResult != DialogResult.Yes)
                    {
                        IsValid = false;
                    }
                    else if (blnDialogResult == DialogResult.Yes)
                    {
                        IsValid = ValidateID(IsValid, true);
                    }
                }
                else
                {
                    IsValid = ValidateID(IsValid, false);
                }
            }
            else
            {
                IsValid = ValidateID(IsValid, true);
            }


            if (IsValid)
            {
                if (salesTaxGroup == null)
                {
                    // group is null if we are creating a new group
                    salesTaxGroup = new SalesTaxGroup();
                    salesTaxGroup.ID = salesTaxGroupID;
                    isNew = true;
                }

                salesTaxGroup.Text = tbDescription.Text;
                salesTaxGroup.SearchField1 = tbSearchField1.Text;
                salesTaxGroup.SearchField2 = tbSearchField2.Text;
                salesTaxGroup.IsForEU = chkEU.Visible ? chkEU.Checked : false;

                Providers.SalesTaxGroupData.Save(PluginEntry.DataModel, salesTaxGroup);
                salesTaxGroupID = salesTaxGroup.ID;

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, isNew ? DataEntityChangeType.Add : DataEntityChangeType.Edit, "SalesTaxGroup", salesTaxGroup.ID, null);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            if (salesTaxGroup == null)
            {// new sales tax group
                btnOK.Enabled = tbDescription.Text != "";
            }
            else
            {// edit an item sales tax group
                btnOK.Enabled = tbDescription.Text != "" &&
                    (tbDescription.Text != salesTaxGroup.Text ||
                     tbSearchField1.Text != salesTaxGroup.SearchField1 ||
                     tbSearchField2.Text != salesTaxGroup.SearchField2 ||
                     chkEU.Checked != salesTaxGroup.IsForEU);
            }
        }

        private bool ValidateID(bool IsValid, bool blnDialogBox)
        {
            if (!tbID.ReadOnly)
            {
                if (!tbID.Text.IsAlphaNumeric())
                {
                    errorProvider1.SetError(tbID, Resources.OnlyCharAndNumbers);
                    IsValid = false;
                }
            }

            if (!blnDialogBox)
            {
                salesTaxGroupID = tbID.Text.Trim();
            }

            if (!tbID.ReadOnly)
            {
                if (Providers.SalesTaxGroupData.Exists(PluginEntry.DataModel, salesTaxGroupID))
                {
                    errorProvider1.SetError(tbID, Resources.TaxGroupIDExists);
                    IsValid = false;
                }
            }

            return IsValid;
        }


    }
}
