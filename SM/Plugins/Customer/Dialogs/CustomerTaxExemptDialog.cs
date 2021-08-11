using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Customer.Dialogs
{
    public partial class CustomerTaxExemptDialog : DialogBase
    {
        private List<CustomerListItemAdvanced> customerList;        
        private SalesTaxGroup taxExemptGroup;

        public CustomerTaxExemptDialog(List<CustomerListItemAdvanced> customers) : this()
        {
            customerList = customers;
            if (customerList.Count == 1)
            {
                DataLayer.BusinessObjects.Customers.Customer customer = Providers.CustomerData.Get(PluginEntry.DataModel, customerList[0].ID, UsageIntentEnum.Normal);
                cmbTaxExempt.SelectedIndex = (customer.TaxExempt == TaxExemptEnum.EU && cmbTaxExempt.Items.Count == 2) ? 0 : (int)customer.TaxExempt;
                cmbSalesTaxGroup.SelectedData = new DataEntity(customer.TaxGroup, customer.TaxGroupName);                
            }
            else if (customerList.Count > 1)
            {
                if(cmbTaxExempt.Items.Count == 3 && customers.All(a => a.TaxExempt == TaxExemptEnum.EU))
                {
                    cmbTaxExempt.SelectedIndex = 2; 
                }
                else if (customers.All(a => a.TaxExempt == TaxExemptEnum.Yes))
                {
                    cmbTaxExempt.SelectedIndex = 1;
                }
                else
                {
                    cmbTaxExempt.SelectedIndex = 0;
                }
            }
        }        

        public CustomerTaxExemptDialog()
        {
            InitializeComponent();

            taxExemptGroup = PluginOperations.GetTaxExemptInformation();

            IPlugin sapPlugin = PluginEntry.Framework.FindImplementor(null, "SAPBusinessOne", null);

            List<string> taxOptions = new List<string>
            {
                Properties.Resources.No,
                Properties.Resources.Yes
            };

            if (sapPlugin != null)
            {
                taxOptions.Add(Properties.Resources.EU);
            }

            cmbTaxExempt.Items.AddRange(taxOptions.ToArray());
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<RecordIdentifier> customerIDs = customerList.Select(s => s.ID).ToList();
            try
            {
                Cursor.Current = Cursors.WaitCursor;                
                Providers.CustomerData.SaveTaxInformation(PluginEntry.DataModel, customerIDs, (TaxExemptEnum)cmbTaxExempt.SelectedIndex, cmbSalesTaxGroup.SelectedDataID);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Customer", RecordIdentifier.Empty, null);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbSalesTaxGroup_RequestData(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SetData(Providers.SalesTaxGroupData.GetListWithTaxCodes(PluginEntry.DataModel, cmbTaxExempt.SelectedIndex == 2 ? TaxGroupTypeFilter.EU : TaxGroupTypeFilter.Standard), null);
        }

        private void cmbSalesTaxGroup_RequestClear(object sender, EventArgs e)
        {
            cmbSalesTaxGroup.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
        }

        private void cmbTaxExempt_SelectedIndexChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            cmbSalesTaxGroup.Enabled = cmbTaxExempt.SelectedIndex != 1;

            cmbSalesTaxGroup.SelectedData = new DataEntity(RecordIdentifier.Empty, "");

            if (!cmbSalesTaxGroup.Enabled && taxExemptGroup != null)
            {
                cmbSalesTaxGroup.SelectedData = new DataEntity(taxExemptGroup.ID, taxExemptGroup.Text);
            }
            else if (!cmbSalesTaxGroup.Enabled && taxExemptGroup == null)
            {
                errorProvider1.Icon = Icon.FromHandle(((Bitmap)PluginEntry.Framework.GetImage(ImageEnum.InformationErrorProvider)).GetHicon());
                errorProvider1.SetError(cmbSalesTaxGroup, Properties.Resources.DefaultTaxExemptionGroupCanBeSet);
            }
        }
    }
}
