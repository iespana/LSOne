using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class NewHospitalityTypeDialog : DialogBase
    {
        private RecordIdentifier hospitalityTypeID;
        WeakReference addRestaurantEditor; // A restaurant is a store

        public RecordIdentifier HospitalityTypeID
        {
            get { return hospitalityTypeID; }
        }

        public NewHospitalityTypeDialog(Store store)
            : this()
        {
            IPlugin plugin;
            hospitalityTypeID = RecordIdentifier.Empty;

            plugin = PluginEntry.Framework.FindImplementor(this, "CanCreateStore", null);

            addRestaurantEditor = (plugin != null) ? new WeakReference(plugin) : null;

            if (store != null)
            {
                cmbRestaurant.SelectedData = new DataEntity(store.ID, store.Text);
            }
            else
            {
                cmbRestaurant.SelectedData = new DataEntity("", "");
            }

            btnAddRestaurant.Visible = (addRestaurantEditor != null);
            btnAddSalesType.Visible = PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes);
        }

        public NewHospitalityTypeDialog()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
            cmbSalesType.SelectedData = new DataEntity("", "");
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0 && cmbRestaurant.SelectedData.ID != "" && cmbSalesType.SelectedData.ID != "");
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = (tbDescription.Text.Length > 0 && cmbRestaurant.SelectedData.ID != "" && cmbSalesType.SelectedData.ID != "");
        }

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbRestaurant_RequestData(object sender, EventArgs e)
        {
            cmbRestaurant.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbSalesType_RequestData(object sender, EventArgs e)
        {
            cmbSalesType.SetData(Providers.SalesTypeData.GetList(PluginEntry.DataModel), null);
        }

        private void btnAddRestaurant_Click(object sender, EventArgs e)
        {
            if (addRestaurantEditor.IsAlive)
            {
                ((IPlugin)addRestaurantEditor.Target).Message(this, "NewStore", null);
            }
        }

        private void btnAddSalesType_Click(object sender, EventArgs e)
        {
            PluginOperations.NewSalesType();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Providers.HospitalityTypeData.RestaurantSalesTypeCombinationExists(PluginEntry.DataModel, cmbRestaurant.SelectedData.ID, cmbSalesType.SelectedData.ID))
            {
                errorProvider1.SetError(cmbSalesType, Properties.Resources.RestaurantSalesTypeExists);
                errorProvider1.SetError(cmbRestaurant, Properties.Resources.RestaurantSalesTypeExists);
                //tbID.Focus();
                return;
            }

            HospitalityType hospitalityType = new HospitalityType();

            hospitalityType.RestaurantID = cmbRestaurant.SelectedData.ID;
            hospitalityType.SalesType = cmbSalesType.SelectedData.ID;
            hospitalityType.Sequence = Providers.HospitalityTypeData.GetNextSequence(PluginEntry.DataModel, hospitalityType.ID);
            hospitalityType.Text = tbDescription.Text;

            Providers.HospitalityTypeData.Save(PluginEntry.DataModel, hospitalityType);
            hospitalityTypeID = hospitalityType.ID;

            DialogResult = DialogResult.OK;
            Close();
        }
        
    }
}
