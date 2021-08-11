using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Store.Dialogs
{
    public partial class AddStoresToRegionDialog : DialogBase
    {
        private Region region;
        private List<RecordIdentifier> excludedStores;

        public AddStoresToRegionDialog(Region region, List<RecordIdentifier> excludedStores)
        {
            InitializeComponent();
            this.region = region;
            this.excludedStores = excludedStores;
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
            DataEntitySelectionList selectionList = cmbStores.SelectedData as DataEntitySelectionList;
            List<RecordIdentifier> selectedStoreIDs = selectionList.GetSelectedItems().Select(x => x.ID).ToList();

            foreach(RecordIdentifier storeID in selectedStoreIDs)
            {
                DataLayer.BusinessObjects.StoreManagement.Store store = Providers.StoreData.Get(PluginEntry.DataModel, storeID);
                store.RegionID = region.ID;
                Providers.StoreData.Save(PluginEntry.DataModel, store);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Edit, "StoreRegion", store.ID, region);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmbStores_SelectedDataChanged(object sender, EventArgs e)
        {
            DataEntitySelectionList selectionList = ((DualDataComboBox)sender).SelectedData as DataEntitySelectionList;
            btnOK.Enabled = selectionList != null && selectionList.GetSelectedItems().Any();
        }

        private void cmbStores_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            cmbStores.SelectedData = new DataEntitySelectionList(stores.Where(x => !excludedStores.Contains(x.ID)).ToList());
        }

        private void cmbStores_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            DataEntitySelectionList selectionList = cmbStores.SelectedData as DataEntitySelectionList;

            if (selectionList != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel(selectionList);
            }
        }
    }
}
