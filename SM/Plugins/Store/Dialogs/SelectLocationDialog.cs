using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Store.Dialogs
{
    public partial class SelectLocationDialog : DialogBase
    {
        RecordIdentifier storeID;
        bool cannotCancel;

        public SelectLocationDialog(bool cannotCancel)
            : this()
        {
            this.cannotCancel = cannotCancel;
        }

        public SelectLocationDialog()
        {
            cannotCancel = false;

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            List<DataEntity> stores;

            base.OnLoad(e);

            stores = Providers.StoreData.GetList(PluginEntry.DataModel);

            foreach (DataEntity store in stores)
            {
                cmbStore.Items.Add(store);

                if (store.ID == PluginEntry.DataModel.CurrentStoreID)
                {
                    cmbStore.SelectedIndex = cmbStore.Items.Count - 1;
                }
            }

            if (cmbStore.SelectedIndex < 0 && PluginEntry.DataModel.CurrentStoreID != 0)
            {
                cmbStore.SelectedIndex = 0;
            }

            if (cannotCancel)
            {
                btnCancel.Enabled = false;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbStore.SelectedIndex == 0)
            {
                storeID = RecordIdentifier.Empty;
            }
            else
            {
                storeID = ((DataEntity)cmbStore.SelectedItem).ID;
            }

            PluginEntry.DataModel.CurrentStoreID = storeID;
            PluginEntry.Framework.SaveConfigurations();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public RecordIdentifier StoreID
        {
            get { return storeID; }
        }

        private void cmbStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (cmbStore.SelectedIndex >= 0);
        }
    }
}
