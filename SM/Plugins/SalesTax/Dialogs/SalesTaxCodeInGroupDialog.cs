using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SalesTax.Properties;

namespace LSOne.ViewPlugins.SalesTax.Dialogs
{
    public partial class SalesTaxCodeInGroupDialog : DialogBase
    {
        public RecordIdentifier TaxGroup { get; set;}
        public RecordIdentifier TaxCode { get; set; }

        private TaxCodeInSalesTaxGroup line;

        private List<DataEntity> taxCodes;

        private bool anotherAdded;

        // New sales tax code for taxGroup
        public SalesTaxCodeInGroupDialog(RecordIdentifier taxGroup)
            : this()
        {
            TaxGroup = taxGroup;
        }

        // Edit sales tax code taxCode in tax group taxGroup
        public SalesTaxCodeInGroupDialog(RecordIdentifier taxGroup, RecordIdentifier taxCode)
            : this()
        {
            TaxGroup = taxGroup;
            TaxCode = taxCode;

            Text = Resources.EditTaxCodeForSalesTaxGroup;
            Header = Resources.ConfigureGroupSettingsForTheSelectedTaxCode;

            // Show the taxcode that is being edited
            DataEntity taxCodeInstance = Providers.TaxCodeData.Get(PluginEntry.DataModel, taxCode);
            cmbTaxCode.SelectedData = taxCodeInstance;
            cmbTaxCode.Enabled = false;

            btnOK.Enabled = true;
        }

        private SalesTaxCodeInGroupDialog()
        {
            InitializeComponent();

            cmbTaxCode.SelectedData = new DataEntity("", "");
            taxCodes = new List<DataEntity>();
            anotherAdded = false;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
            
            line = new TaxCodeInSalesTaxGroup() {SalesTaxGroup = TaxGroup, 
                                              TaxCode = cmbTaxCode.SelectedData.ID
                                             };

            Providers.SalesTaxGroupData.AddTaxCodeToSalesTaxGroup(PluginEntry.DataModel, line);

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
                    DataEntity lastOne = taxCodes.FirstOrDefault(f => f.ID != line.TaxCode);
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

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "SalesTaxCodeInGroup", TaxGroup,  line);
            CheckEnabled();
        }

        private void cmbTaxCode_RequestData(object sender, EventArgs e)
        {
            taxCodes = Providers.SalesTaxGroupData.GetTaxCodesInSalesTaxGroupList(PluginEntry.DataModel,
                                                                                          TaxGroup, false);
            
            cmbTaxCode.SetData(taxCodes,null);
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
            //    LoadItems(lvGroups.SortColumn, lvGroups.SortedBackwards, true, selectedTaxItemGroup);
            }
        }

       
    }
}
