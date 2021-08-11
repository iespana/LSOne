using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class CustomerPriceDiscDialog : DialogBase
    {
        PriceDiscGroupEnum type;
        PriceDiscountGroup group;

        internal CustomerPriceDiscDialog(PriceDiscGroupEnum type,RecordIdentifier id)
            : this(type)
        {
            group = Providers.PriceDiscountGroupData.Get(PluginEntry.DataModel, id);

            tbDescription.Text = group.Text;
            chkIncludeTax.Checked = group.IncludeTax;
        }

        internal CustomerPriceDiscDialog(PriceDiscGroupEnum type)
            : this()
        {
            this.type = type;

            switch (type)
            {
                case PriceDiscGroupEnum.PriceGroup:
                    Text = Properties.Resources.CustomerPriceGroup;
                    lblPriceIncludeTax.Visible = true;
                    chkIncludeTax.Visible = true;
                    break;
                case PriceDiscGroupEnum.LineDiscountGroup:
                    Text = Properties.Resources.CustomerLineDiscountGroup;
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    Text = Properties.Resources.CustomerMultiLineDiscountGroup;
                    break;
                case PriceDiscGroupEnum.TotalDiscountGroup:
                    Text = Properties.Resources.CustomerTotalDiscountGroup;
                    break;
            }
        
        }

        public CustomerPriceDiscDialog()
        {
            group = null;

            InitializeComponent();
        }

        public RecordIdentifier ID
        {
            get { return group.ID; }
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
            bool isNew = false;

            if (group == null)
            {
                // group is null if we are creating a new group
                group = new PriceDiscountGroup();
                group.Type = type;
                group.Module = PriceDiscountModuleEnum.Customer;
                isNew = true;
            }

            group.Text = tbDescription.Text;
            group.IncludeTax = chkIncludeTax.Checked;

            Providers.PriceDiscountGroupData.Save(PluginEntry.DataModel, group);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, isNew ? DataEntityChangeType.Add : DataEntityChangeType.Edit, "PriceDiscountGroup", group.ID, null);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            if (group != null)
            {
                btnOK.Enabled = (tbDescription.Text.Length > 0) &&
                    (chkIncludeTax.Checked != group.IncludeTax || tbDescription.Text != group.Text);
            }
            else
            {
                btnOK.Enabled = (tbDescription.Text.Length > 0);
            }
        }

        private void chkIncludeTax_CheckedChanged(object sender, EventArgs e)
        {
            if (group != null)
            {
                btnOK.Enabled = (tbDescription.Text.Length > 0) &&
                    (chkIncludeTax.Checked != group.IncludeTax || tbDescription.Text != group.Text);
            }
        }


    }
}
