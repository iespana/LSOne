using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class EditRestaurantDiningTableDialog : DialogBase
    {
        RecordIdentifier restaurantDiningTableID;
        RestaurantDiningTable restaurantDiningTable;
        HospitalityType hospitalityType;
        //bool initialFocus;


        public EditRestaurantDiningTableDialog(RecordIdentifier restaurantDiningTableID)
        {
            InitializeComponent();

            this.restaurantDiningTableID = restaurantDiningTableID;
            tbDescription.TextChanged += new EventHandler(tbDescription_TextChanged);
        }        

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadData();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckOkEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = IsModified();
        }

        private void LoadData()
        {
            restaurantDiningTable = Providers.RestaurantDiningTableData.Get(PluginEntry.DataModel, restaurantDiningTableID);
            hospitalityType = Providers.HospitalityTypeData.Get(PluginEntry.DataModel, restaurantDiningTable.RestaurantID, restaurantDiningTable.SalesType); 

            ntbTableNo.Value = (int)restaurantDiningTable.DineInTableNo;
            tbDescription.Text = restaurantDiningTable.Text;
            tbDescriptionOnButton.Text = restaurantDiningTable.GetDescription(hospitalityType.TableButtonDescription);
            chkNonSmoking.Checked = restaurantDiningTable.NonSmoking;
            cmbAvailability.SelectedIndex = (int)restaurantDiningTable.Availability;

        }

        private bool IsModified()
        {
            if (tbDescription.Text != restaurantDiningTable.Text) return true;
            if (tbDescriptionOnButton.Text != restaurantDiningTable.DescriptionOnButton) return true;
            if (chkNonSmoking.Checked != restaurantDiningTable.NonSmoking) return true;
            if (cmbScreenNo.SelectedData != null && cmbScreenNo.SelectedData.ID != "" && ((DiningTableLayoutScreen)cmbScreenNo.SelectedData).ScreenNo != restaurantDiningTable.LayoutScreenNo) return true;
            if (cmbAvailability.SelectedIndex != (int)restaurantDiningTable.Availability) return true;

            return false;
        }

        private void SaveData()
        {
            restaurantDiningTable.Text = tbDescription.Text;
            restaurantDiningTable.DescriptionOnButton = tbDescriptionOnButton.Text;
            restaurantDiningTable.NonSmoking = chkNonSmoking.Checked;
            restaurantDiningTable.LayoutScreenNo = cmbScreenNo.SelectedData != null && cmbScreenNo.SelectedData.ID != "" ? ((DiningTableLayoutScreen)cmbScreenNo.SelectedData).ScreenNo : RecordIdentifier.Empty;
            restaurantDiningTable.Availability = (RestaurantDiningTable.AvailabilityEnum)cmbAvailability.SelectedIndex;

            Providers.RestaurantDiningTableData.Save(PluginEntry.DataModel, restaurantDiningTable);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RestaurantDiningTable", restaurantDiningTable.ID, null);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {   

            SaveData();

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
            PluginOperations.ShowDiningTableLayoutScreens(restaurantDiningTableID, cmbScreenNo.SelectedData != null ? cmbScreenNo.SelectedData.ID : RecordIdentifier.Empty);
        }

        private void cmbScreenNo_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbScreenNo_RequestData(object sender, EventArgs e)
        {
            cmbScreenNo.SetData(Providers.DiningTableLayoutScreenData.GetList(PluginEntry.DataModel, restaurantDiningTable.RestaurantID, restaurantDiningTable.Sequence, restaurantDiningTable.SalesType, restaurantDiningTable.DiningTableLayoutID), null);
        }

        private void cmbScreenNo_RequestClear(object sender, EventArgs e)
        {
            cmbScreenNo.SelectedData = new DataEntity("", "");
        }

        void tbDescription_TextChanged(object sender, EventArgs e)
        {
            tbDescriptionOnButton.Text = restaurantDiningTable.GetDescription(hospitalityType.TableButtonDescription, tbDescription.Text);
        }
    }
}
