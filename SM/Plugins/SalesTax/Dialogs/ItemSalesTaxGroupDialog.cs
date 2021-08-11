using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SalesTax.Properties;
using System;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.SalesTax.Dialogs
{
    public partial class ItemSalesTaxGroupDialog : DialogBase
    {
        ItemSalesTaxGroup itemSalesTaxGroup;
        private RecordIdentifier itemSalesTaxGroupID = RecordIdentifier.Empty;
        private bool manuallyEnterId = false;

        // Edit a item sales tax group
        public ItemSalesTaxGroupDialog(RecordIdentifier taxItemGroup)
            : this()
        {
            itemSalesTaxGroup = Providers.ItemSalesTaxGroupData.Get(PluginEntry.DataModel, taxItemGroup);
            itemSalesTaxGroupID = itemSalesTaxGroup.ID;

            tbID.Text = (string)itemSalesTaxGroup.ID;
            tbID.ReadOnly = true;
            tbID.BackColor = ColorPalette.BackgroundColor;
			tbID.ForeColor = ColorPalette.DisabledTextColor;
            tbDescription.Text = itemSalesTaxGroup.Text;
            tbReceiptDisplay.Text = itemSalesTaxGroup.ReceiptDisplay;

            Text = Resources.EditItemSalesTaxGroup;
            Header = Resources.EditDescriptionForTheSelectedTaxGroup;
        }

        // New item sales tax group
        public ItemSalesTaxGroupDialog()
        {
            InitializeComponent();

            var parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
            manuallyEnterId = parameters.ManuallyEnterTaxGroupID;

            tbID.Visible = manuallyEnterId;
            lblID.Visible = manuallyEnterId;
        }

        public RecordIdentifier SalesTaxGroupID
        {
            get { return itemSalesTaxGroup != null ? itemSalesTaxGroup.ID : RecordIdentifier.Empty; }
        }

        public ItemSalesTaxGroup GetItemSalesTaxGroup
        {
            get { return itemSalesTaxGroup; }
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
                if (itemSalesTaxGroup == null)
                {
                    // group is null if we are creating a new group
                    itemSalesTaxGroup = new ItemSalesTaxGroup();
                    itemSalesTaxGroup.ID = itemSalesTaxGroupID;
                    isNew = true;
                }

                itemSalesTaxGroup.Text = tbDescription.Text;
                itemSalesTaxGroup.ReceiptDisplay = tbReceiptDisplay.Text;

                Providers.ItemSalesTaxGroupData.Save(PluginEntry.DataModel, itemSalesTaxGroup);
                itemSalesTaxGroupID = itemSalesTaxGroup.ID;

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, isNew ? DataEntityChangeType.Add : DataEntityChangeType.Edit, "ItemSalesTaxGroup", itemSalesTaxGroup.ID, null);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            if (itemSalesTaxGroup == null)
            {// new item sales tax group
                btnOK.Enabled = (tbDescription.Text != "");
            }
            else
            {// edit an item sales tax group
                btnOK.Enabled = (tbDescription.Text != "") 
                    && tbDescription.Text != itemSalesTaxGroup.Text ||
                    tbReceiptDisplay.Text != itemSalesTaxGroup.ReceiptDisplay;
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
                itemSalesTaxGroupID = tbID.Text.Trim();
            }

            if (!tbID.ReadOnly)
            {
                if (Providers.ItemSalesTaxGroupData.Exists(PluginEntry.DataModel, itemSalesTaxGroupID))
                {
                    errorProvider1.SetError(tbID, Resources.TaxGroupIDExists);
                    IsValid = false;
                }
            }

            return IsValid;
        }

    }
}
