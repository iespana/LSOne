using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class AddRestaurantDiningTablesDialog : DialogBase
    {
        RecordIdentifier diningTableLayoutID;
        int maximumNumberOfTables;

        public AddRestaurantDiningTablesDialog(RecordIdentifier diningTableLayoutID, int maximumNumberOfTables)
        {
            InitializeComponent();
            
            this.diningTableLayoutID = diningTableLayoutID;
            this.maximumNumberOfTables = maximumNumberOfTables;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier DiningTableLayoutID
        {
            get { return diningTableLayoutID; }
        }


        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = ntbNoOfTablesToAdd.Value > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            RestaurantDiningTable restaurantDiningTable;

            if (Providers.RestaurantDiningTableData.DineInTableRangeExists(PluginEntry.DataModel, diningTableLayoutID, (int)ntbStartingFrom.Value, (int)ntbStartingFrom.Value + ((int)ntbNoOfTablesToAdd.Value - 1)))
            {
                errorProvider1.SetError(ntbNoOfTablesToAdd, Properties.Resources.RestaurantDiningTableRangeExists);
                errorProvider1.SetError(ntbStartingFrom, Properties.Resources.RestaurantDiningTableRangeExists);
                ntbStartingFrom.Focus();
                return;
            }

            // Check if we can actually add those tables
            if ((int)ntbNoOfTablesToAdd.Value >
                (maximumNumberOfTables - Providers.RestaurantDiningTableData.GetNumberOfTables(PluginEntry.DataModel, diningTableLayoutID)))
            {
                errorProvider1.SetError(ntbNoOfTablesToAdd, Properties.Resources.NumberOfRestaurantDiningTablesExceeds);
                errorProvider1.SetError(ntbStartingFrom, Properties.Resources.NumberOfRestaurantDiningTablesExceeds);
                ntbNoOfTablesToAdd.Focus();
                return;
            }

            for (int i = (int)ntbStartingFrom.Value; i < (int)ntbStartingFrom.Value + (int)ntbNoOfTablesToAdd.Value; i++)
            {
                restaurantDiningTable = new RestaurantDiningTable();
                restaurantDiningTable.RestaurantID = diningTableLayoutID[0];
                restaurantDiningTable.Sequence = diningTableLayoutID[1];
                restaurantDiningTable.SalesType = diningTableLayoutID[2];
                restaurantDiningTable.DiningTableLayoutID = diningTableLayoutID[3];

                restaurantDiningTable.DineInTableNo = i;
                restaurantDiningTable.LayoutScreenNo = cmbScreenNo.SelectedData != null && cmbScreenNo.SelectedData.ID != "" ? ((DiningTableLayoutScreen)cmbScreenNo.SelectedData).ScreenNo : RecordIdentifier.Empty;

                restaurantDiningTable.Text = tbDescription.Text;                
                restaurantDiningTable.NonSmoking = chkNonSmoking.Checked;

                Providers.RestaurantDiningTableData.Save(PluginEntry.DataModel, restaurantDiningTable);
            }            

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnEditScreen_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowDiningTableLayoutScreens(diningTableLayoutID, cmbScreenNo.SelectedData != null ? cmbScreenNo.SelectedData.ID : RecordIdentifier.Empty);
        }

        private void cmbScreenNo_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbScreenNo_RequestData(object sender, EventArgs e)
        {
            cmbScreenNo.SetData(Providers.DiningTableLayoutScreenData.GetList(PluginEntry.DataModel, diningTableLayoutID[0], diningTableLayoutID[1], diningTableLayoutID[2], diningTableLayoutID[3]), null);
        }

        private void cmbScreenNo_RequestClear(object sender, EventArgs e)
        {
            cmbScreenNo.SelectedData = new DataEntity("", "");
        }
    }
}
