using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.SalesTax.Dialogs
{
    public partial class ItemSalesTaxCodeInGroupDialog : DialogBase
    {

        private RecordIdentifier TaxItemGroup { get; set; }
        public RecordIdentifier SelectedTaxCode { get; set; }

        private TaxCodeInItemSalesTaxGroup line;

        private List<DataEntity> taxCodes { get; set; }

        private bool anotherAdded;


        public ItemSalesTaxCodeInGroupDialog(RecordIdentifier taxItemGroup)
            : this()
        {
            TaxItemGroup = taxItemGroup;

            cmbTaxCode.SelectedData = new DataEntity("", "");
        }

        private ItemSalesTaxCodeInGroupDialog()
        {
            InitializeComponent();

            taxCodes = new List<DataEntity>();
            anotherAdded = false;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = anotherAdded ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbTaxCode.SelectedData.ID.StringValue == "")
            {
                return;
            }

            line = new TaxCodeInItemSalesTaxGroup { TaxItemGroup = TaxItemGroup, TaxCode = cmbTaxCode.SelectedData.ID };

            Providers.ItemSalesTaxGroupData.AddTaxCodeToItemSalesTaxGroup(PluginEntry.DataModel, line);

            SelectedTaxCode = cmbTaxCode.SelectedData.ID;

            if (!chkAddAnother.Checked)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                anotherAdded = true;
                if (taxCodes.Count == 2)
                {
                    DataEntity lastOne = taxCodes.FirstOrDefault(f => f.ID != SelectedTaxCode);
                    if (lastOne != null)
                    {
                        taxCodes.Clear();
                        taxCodes.Add(lastOne);
                    }
                    cmbTaxCode.SelectedData = lastOne ?? new DataEntity();

                }
                else
                {
                    cmbTaxCode.SelectedData = new DataEntity();
                }
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "ItemSalesTaxCodeInGroup", TaxItemGroup, line);
            CheckEnabled();
        }

        private void cmbTaxCode_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "" || ((DataEntity)e.Data).ID == RecordIdentifier.Empty)
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = ((DataEntity)e.Data).ID + " - " + ((DataEntity)e.Data).Text;
            }
        }

        private void cmbTaxCode_RequestData(object sender, EventArgs e)
        {
            taxCodes = Providers.ItemSalesTaxGroupData.GetTaxCodesInItemSalesTaxGroupList(PluginEntry.DataModel, TaxItemGroup);
            cmbTaxCode.SetData(taxCodes, null);
        }

        private void cmbTaxCode_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = cmbTaxCode.SelectedData.ID.StringValue != "";
        }

        private void btnAddSalesTaxCode_Click(object sender, EventArgs e)
        {
            NewTaxCodeDialog dlg = new NewTaxCodeDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {

            }
        }


    }
}
