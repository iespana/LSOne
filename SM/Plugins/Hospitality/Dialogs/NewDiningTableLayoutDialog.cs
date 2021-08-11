using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class NewDiningTableLayoutDialog : DialogBase
    {
        DiningTableLayout emptyDiningTableLayout;
        HospitalityType parentHospitalityType;
        RecordIdentifier diningTableLayoutID;
        DataEntity diningTableLayoutEntity;

        public NewDiningTableLayoutDialog(RecordIdentifier restaurantID, RecordIdentifier sequence, RecordIdentifier salesType)
        {
            InitializeComponent();

            diningTableLayoutID = new RecordIdentifier(restaurantID,
                                new RecordIdentifier(sequence,
                                new RecordIdentifier(salesType, RecordIdentifier.Empty)));            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            emptyDiningTableLayout = new DiningTableLayout();
            emptyDiningTableLayout.LayoutID = RecordIdentifier.Empty;
            emptyDiningTableLayout.Text = Properties.Resources.DoNotCopy;

            cmbCopyFrom.SelectedData = emptyDiningTableLayout;

            parentHospitalityType = Providers.HospitalityTypeData.Get(PluginEntry.DataModel, diningTableLayoutID[0], diningTableLayoutID[2]);
            tbHospitalityTypeDescription.Text = parentHospitalityType.Text;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier DiningTableLayoutID
        {
            get { return diningTableLayoutID; }
        }

        public DataEntity DiningTableLayoutEntity
        {
            get
            {
                return diningTableLayoutEntity;
            }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text.Length > 0;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = (tbDescription.Text.Length > 0);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DiningTableLayout diningTableLayout;
            List<RestaurantDiningTable> restaurantDiningTablesCopy = new List<RestaurantDiningTable>();

            //if (Providers.DiningTableLayoutData.LayoutIDExists(PluginEntry.DataModel, tbLayoutID.Text))
            //{
            //    errorProvider1.SetError(tbLayoutID, Properties.Resources.LayoutIDExists);
            //    return;
            //}
            
            diningTableLayout = new DiningTableLayout();
            diningTableLayout.RestaurantID = diningTableLayoutID[0];
            diningTableLayout.Sequence = diningTableLayoutID[1];
            diningTableLayout.SalesType = diningTableLayoutID[2];
            diningTableLayout.LayoutID = diningTableLayoutID[3];
            diningTableLayout.Text = tbDescription.Text;

            // Check for copy from
            if (((DiningTableLayout)cmbCopyFrom.SelectedData).LayoutID != RecordIdentifier.Empty)
            {
                DiningTableLayout layoutToCopy = ((DiningTableLayout)cmbCopyFrom.SelectedData);

                Providers.DiningTableLayoutData.CopyDataFromLayout(PluginEntry.DataModel, layoutToCopy.ID, diningTableLayout);

                // Copy all RestaurantDiningTables records as well, but we cannot copy the primary key yet since we haven't gotten the LayoutID
                // from the seaquence generator. 
                restaurantDiningTablesCopy = Providers.RestaurantDiningTableData.GetList(PluginEntry.DataModel, layoutToCopy.RestaurantID, layoutToCopy.Sequence, layoutToCopy.SalesType, layoutToCopy.LayoutID);
            }
            else
            {
                diningTableLayout.DiningTableRows = (int)ntbDiningTableRows.Value;
                diningTableLayout.DiningTableColumns = (int)ntbDiningTableColumns.Value;
            }

            Providers.DiningTableLayoutData.Save(PluginEntry.DataModel, diningTableLayout);

            // Check again for "copy from", and fill out the primary key data for the restaurant dining tables.
            // The reason for this is that not until "Save" is called, a LayoutID sequence has not been generated
            // for the current DiningTableLayout object
            if (((DiningTableLayout)cmbCopyFrom.SelectedData).LayoutID != RecordIdentifier.Empty)
            {
                for (int i = 0; i < restaurantDiningTablesCopy.Count; i++)
                {
                    restaurantDiningTablesCopy[i].RestaurantID = diningTableLayout.RestaurantID;
                    restaurantDiningTablesCopy[i].SalesType = diningTableLayout.SalesType;
                    restaurantDiningTablesCopy[i].Sequence = diningTableLayout.Sequence;
                    restaurantDiningTablesCopy[i].DiningTableLayoutID = diningTableLayout.LayoutID;

                    Providers.RestaurantDiningTableData.Save(PluginEntry.DataModel, restaurantDiningTablesCopy[i]);
                }
            }

            diningTableLayoutID = diningTableLayout.ID;
            diningTableLayoutEntity = new DataEntity(diningTableLayoutID, tbDescription.Text);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            cmbCopyFrom.SetWidth(350);

            cmbCopyFrom.SetHeaders(new string[] {Properties.Resources.HospitalityTypeText, Properties.Resources.DiningTableLayout },
                                   new int[] { 2, 1});            

            List<DiningTableLayout> list = Providers.DiningTableLayoutData.GetList(PluginEntry.DataModel);
            list.Insert(0, emptyDiningTableLayout);

            cmbCopyFrom.SetData(list, null);
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            ntbDiningTableColumns.Enabled = ntbDiningTableRows.Enabled = (((DiningTableLayout)cmbCopyFrom.SelectedData).LayoutID == RecordIdentifier.Empty);
        }
    }
}
