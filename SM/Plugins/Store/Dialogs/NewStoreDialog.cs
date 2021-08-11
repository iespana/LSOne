using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Store.Dialogs
{
    public partial class NewStoreDialog : DialogBase
    {
        DataEntity emptyItem;
        RecordIdentifier storeID;
        WeakReference addCurrencyEditor;              


        bool manuallyEnterID = false;

        public NewStoreDialog() : base()
        {
            IPlugin plugin;

            storeID = RecordIdentifier.Empty;

            InitializeComponent();

            Parameters parameters = DataProviderFactory.Instance.Get<IParameterData, Parameters>().Get(PluginEntry.DataModel);
            manuallyEnterID = parameters.ManuallyEnterStoreID;

            tbID.Visible = manuallyEnterID;
            lblID.Visible = manuallyEnterID;


            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddCurrency", null);
            addCurrencyEditor = (plugin != null) ? new WeakReference(plugin) : null;
            btnAddCurrency.Visible = (addCurrencyEditor != null);


            btnAddFunctionalityProfile.Visible = PluginEntry.Framework.CanRunOperation("AddFunctionalityProfile");
            btnAddTouchButtonLayout.Visible = PluginEntry.Framework.CanRunOperation("AddLayout");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            emptyItem = new DataEntity(RecordIdentifier.Empty, Properties.Resources.DoNotCopyExistingStore);

            cmbCopyFrom.SelectedData = emptyItem;

            cmbCurrency.SelectedData = new DataEntity("", "");
            cmbRegion.SelectedData = new DataEntity("", "");

            List<DataEntity> touchButtonLayouts = Providers.TouchLayoutData.GetList(PluginEntry.DataModel);
            List<DataEntity> functionalityProfiles = Providers.FunctionalityProfileData.GetList(PluginEntry.DataModel);

            cmbTouchButtonLayout.SelectedData = touchButtonLayouts.Count == 1 ? touchButtonLayouts[0] : new DataEntity("", "");
            cmbFunctionalityProfile.SelectedData = functionalityProfiles.Count == 1 ? functionalityProfiles[0] : new DataEntity("", "");
        }
        
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier StoreID
        {
            get { return storeID; }
        }

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {
            DataEntity item = ((DataEntity)e.Data);
            e.TextToDisplay = (item.ID == "" || item.ID == RecordIdentifier.Empty ? "" : item.ID.ToString() + " - ") + item.Text;
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> list;

            list = Providers.StoreData.GetList(PluginEntry.DataModel);

            list.Insert(0, emptyItem);

            cmbCopyFrom.SetData(list,
                PluginEntry.Framework.GetImageList().Images[PluginEntry.StoreImageIndex],
                true);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = (tbDescription.Text.Length > 0 && cmbCurrency.SelectedData.ID != "" && cmbTouchButtonLayout.SelectedData.ID != "" && cmbFunctionalityProfile.SelectedData.ID != "");
        }

        private bool Save()
        {
            LSOne.DataLayer.BusinessObjects.StoreManagement.Store store;


            if (cmbCopyFrom.SelectedData.ID != RecordIdentifier.Empty)
            {
                store = Providers.StoreData.Get(PluginEntry.DataModel, (string)cmbCopyFrom.SelectedData.ID);
                store.ID = RecordIdentifier.Empty;
                store.Text = tbDescription.Text;
            }
            else
            {
                store = new LSOne.DataLayer.BusinessObjects.StoreManagement.Store();
                store.Text = tbDescription.Text;
                store.Address.AddressFormat = PluginEntry.DataModel.Settings.AddressFormat;
                store.RegionID = cmbRegion.SelectedData.ID;
            }

            if (!manuallyEnterID && storeID != RecordIdentifier.Empty)
            {
                store.ID = storeID;
            }

            if (manuallyEnterID)
            {
                if (tbID.Text.Trim() == "")
                {
                    if (QuestionDialog.Show(Properties.Resources.IDMissingQuestion, Properties.Resources.IDMissing) != System.Windows.Forms.DialogResult.Yes)
                    {
                        return false;
                    }
                }
                else
                {
                    if (!tbID.Text.IsAlphaNumeric())
                    {
                        errorProvider1.SetError(tbID, Properties.Resources.OnlyCharAndNumbers);
                        return false;
                    }
                    store.ID = tbID.Text.Trim();

                    if (DataProviderFactory.Instance.Get<IStoreData, DataLayer.BusinessObjects.StoreManagement.Store>().Exists(PluginEntry.DataModel, store.ID))
                    {
                        errorProvider1.SetError(tbID, Properties.Resources.StoreIDExists);
                        return false;
                    }
                }
            }


            store.Currency = (string)cmbCurrency.SelectedData.ID;
            store.FunctionalityProfile = (string)cmbFunctionalityProfile.SelectedData.ID;
            store.LayoutID = cmbTouchButtonLayout.SelectedData.ID;

            Providers.StoreData.Save(PluginEntry.DataModel, store);

            if (cmbCopyFrom.SelectedData.ID != RecordIdentifier.Empty)
            {
                // Copy store tender types and card types on the tender types
                List<StorePaymentMethod> paymentMethods = Providers.StorePaymentMethodData.GetRecords(PluginEntry.DataModel, cmbCopyFrom.SelectedData.ID, true);

                foreach (StorePaymentMethod paymentMethod in paymentMethods)
                {
                    Providers.StorePaymentMethodData.CopyPaymentMethodToStore(PluginEntry.DataModel, paymentMethod, store.ID);
                }
            }

            storeID = store.ID;
            return true;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                DialogResult = DialogResult.OK;
                Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
           

            DialogResult = DialogResult.Cancel;
            Close();
        }

        

        private void cmbCurrency_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> list;

            list = Providers.CurrencyData.GetList(PluginEntry.DataModel);

            cmbCurrency.SetData(list, null);
        }

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            if (addCurrencyEditor.IsAlive)
            {
                ((IPlugin)addCurrencyEditor.Target).Message(this, "AddCurrency", null);
            }
        }

        private void btnAddTouchButtonLayout_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.RunOperation("AddLayout", this, PluginOperationArguments.Empty);
        }

        private void btnAddFunctionalityProfile_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.RunOperation("AddFunctionalityProfile", this, PluginOperationArguments.Empty);
        }

        private void cmbTouchButtonLayout_RequestData(object sender, EventArgs e)
        {
            cmbTouchButtonLayout.SetData(Providers.TouchLayoutData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbFunctionalityProfile_RequestData(object sender, EventArgs e)
        {
            cmbFunctionalityProfile.SetData(Providers.FunctionalityProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void btnAddRegion_Click(object sender, EventArgs e)
        {
            Region newRegion = PluginOperations.EditRegion(RecordIdentifier.Empty);

            if(newRegion != null)
            {
                cmbRegion.SelectedData = newRegion;
            }
        }

        private void cmbRegion_RequestData(object sender, EventArgs e)
        {
            cmbRegion.SetData(Providers.RegionData.GetList(PluginEntry.DataModel, DataLayer.BusinessObjects.StoreManagement.Region.SortEnum.Description, false), null);
        }
    }
}
