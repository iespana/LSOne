using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Store.Dialogs
{
    public partial class NewStorePaymentMethodDialog : DialogBase
    {
        DataEntity emptyItem;
        RecordIdentifier storeID;
        RecordIdentifier paymentMethodID;
        WeakReference paymentAdder;

        public NewStorePaymentMethodDialog(RecordIdentifier storeID)
            : this()
        {
            this.storeID = storeID;

            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "CanAddPayments", null);
            btnAddPayment.Visible = (plugin != null);
            paymentAdder = new WeakReference(plugin);
            
        }


        public NewStorePaymentMethodDialog()
        {
            InitializeComponent();

            this.paymentMethodID = RecordIdentifier.Empty;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            emptyItem = new DataEntity(RecordIdentifier.Empty, Properties.Resources.DoNotCopy);
            cmbStore.SelectedData = emptyItem;

        }

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbPaymentMethod_RequestData(object sender, EventArgs e)
        {
            cmbPaymentMethod.SetData(Providers.StorePaymentMethodData.GetUnusedList(PluginEntry.DataModel, storeID.PrimaryID), null);
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = Providers.StoreData.
                GetList(PluginEntry.DataModel,storeID);

            items.Insert(0, emptyItem);

            cmbStore.SetData(items,
                PluginEntry.Framework.GetImageList().Images[PluginEntry.StoreImageIndex],
                true);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            StorePaymentMethod existingPaymentMethod = null;
            StorePaymentMethod paymentMethod = null;

            if (cmbStore.SelectedData.ID != RecordIdentifier.Empty)
            {
                // See if the selected store to copy from has a payment method of that type

                existingPaymentMethod = Providers.StorePaymentMethodData.Get(
                    PluginEntry.DataModel, 
                     new RecordIdentifier(cmbStore.SelectedData.ID, cmbPaymentMethod.SelectedData.ID));

                if (existingPaymentMethod == null)
                {
                    errorProvider1.SetError(cmbStore, Properties.Resources.StoreDoesNotHavePaymentType);
                    cmbStore.Focus();
                    return;
                }
                else
                {
                    Providers.StorePaymentMethodData.CopyPaymentMethodToStore(PluginEntry.DataModel, existingPaymentMethod, storeID);
                }
            }
            else
            {
                paymentMethod = new StorePaymentMethod();

                paymentMethod.StoreID = storeID;
                paymentMethod.PaymentTypeID = cmbPaymentMethod.SelectedData.ID;
                paymentMethod.Text = cmbPaymentMethod.SelectedData.Text;
                paymentMethod.PosOperation = cmbActions.SelectedID;
                paymentMethod.AllowOverTender = true;
                paymentMethod.AllowUnderTender = true;              

                paymentMethod.DefaultFunction = Providers.PaymentMethodData.Get(PluginEntry.DataModel, paymentMethod.PaymentTypeID).DefaultFunction;

                if (paymentMethod.DefaultFunction == PaymentMethodDefaultFunctionEnum.DepositTender)
                {
                    paymentMethod.BlindRecountAllowed = false;
                }

                Providers.StorePaymentMethodData.Save(PluginEntry.DataModel, paymentMethod, paymentMethod.PaymentTypeID);

            }

            paymentMethodID = cmbPaymentMethod.SelectedData.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmbPaymentMethod_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMethod.SelectedData != null)
            {
                cmbActions.LoadData(Providers.PosOperationData.GetPaymentOperations(PluginEntry.DataModel), new RecordIdentifier());

                cmbActions.Enabled = true;
            }
            CheckEnabled();
        }

        private void CheckEnabled()
        {
            bool proceed = cmbPaymentMethod.SelectedData.ID != RecordIdentifier.Empty;
            proceed = proceed && cmbActions.SelectedIndex >= 0;
            btnOK.Enabled = proceed;
        }

        public RecordIdentifier PaymentMethodID
        {
            get
            {
                return paymentMethodID;
            }
        }

        private void cmbActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void cmbStore_SelectedDataChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void btnAddUnit_Click(object sender, EventArgs e)
        {
            ((IPlugin)paymentAdder.Target).Message(this, "AddPayments", null);
        }
       
    }
}
