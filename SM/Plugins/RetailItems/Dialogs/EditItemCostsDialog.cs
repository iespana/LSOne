using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
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

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    public partial class EditItemCostsDialog : DialogBase
    {
        private List<DataEntity> selectedStores;
        private decimal initialCostPrice;

        private bool anyStoreSelected;

        public EditItemCostsDialog(List<DataEntity> selectedStores, decimal initialCostPrice)
        {
            InitializeComponent();
            this.selectedStores = selectedStores;
            this.initialCostPrice = initialCostPrice;

            DecimalLimit priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            ntbCostPrice.SetValueWithLimit(initialCostPrice, priceLimiter);
        }

        public decimal CostPrice { get; private set; }
        public string Reason { get; private set; }
        public List<RecordIdentifier> StoreIDs { get; private set; }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void cmbStores_SelectedDataChanged(object sender, EventArgs e)
        {
            DataEntitySelectionList selectionList = ((DualDataComboBox)sender).SelectedData as DataEntitySelectionList;
            anyStoreSelected = selectionList != null && selectionList.GetSelectedItems().Any();
            btnOK.Enabled = (decimal)ntbCostPrice.Value != initialCostPrice && anyStoreSelected;
        }

        private void cmbStores_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            DataEntitySelectionList selectionList = cmbStores.SelectedData as DataEntitySelectionList;

            if (selectionList != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel(selectionList);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DataEntitySelectionList selectionList = cmbStores.SelectedData as DataEntitySelectionList;
            StoreIDs = selectionList.GetSelectedItems().Select(x => x.ID).ToList();
            CostPrice = (decimal)ntbCostPrice.Value;
            Reason = string.IsNullOrEmpty(txtReason.Text) ? txtReason.GhostText : string.Format("{0}: {1}", Properties.Resources.Manual, txtReason.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void EditItemCostsDialog_Load(object sender, EventArgs e)
        {
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            DataEntitySelectionList selectionList = new DataEntitySelectionList(stores);
            selectionList.SelectSome(selectedStores);
            cmbStores.SelectedData = selectionList;
            anyStoreSelected = true;
        }

        private void EditItemCostsDialog_Shown(object sender, EventArgs e)
        {
            ntbCostPrice.Focus();
            ntbCostPrice.SelectAll();
        }

        private void ntbCostPrice_ValueChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (decimal)ntbCostPrice.Value != initialCostPrice && anyStoreSelected;
        }
    }
}
