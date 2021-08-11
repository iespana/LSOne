using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class ItemDiscDialog : DialogBase
    {
        PriceDiscGroupEnum type;
        PriceDiscountGroup group;

        internal ItemDiscDialog(PriceDiscGroupEnum type,RecordIdentifier id)
            : this(type)
        {
            group = Providers.PriceDiscountGroupData.Get(PluginEntry.DataModel, id);

            tbDescription.Text = group.Text;
            chkIncludeTax.Checked = group.IncludeTax;
        }

        internal ItemDiscDialog(PriceDiscGroupEnum type)
            : this()
        {
            this.type = type;

            switch (type)
            {
                case PriceDiscGroupEnum.LineDiscountGroup:
                    Text = Properties.Resources.ItemLineDiscountGroup;
                    break;
                case PriceDiscGroupEnum.MultilineDiscountGroup:
                    Text = Properties.Resources.ItemMultilineDiscountGroup;
                    break;
                default:
                    break;
            }
        }

        public ItemDiscDialog()
        {
            group = null;

            InitializeComponent();
        }

        public RecordIdentifier GroupId
        {
            get { return (group != null) ? group.ID : ""; }
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

            if (group == null)
            {
                // group is null if we are creating a new group

                group = new PriceDiscountGroup();
                group.Type = type;
                group.Module = PriceDiscountModuleEnum.Item;

            }

            group.Text = tbDescription.Text;
            group.IncludeTax = chkIncludeTax.Checked;

            Providers.PriceDiscountGroupData.Save(PluginEntry.DataModel, group);

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
