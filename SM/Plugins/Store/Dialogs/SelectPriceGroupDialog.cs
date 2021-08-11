using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Store.Dialogs
{
    public partial class SelectPriceGroupDialog : DialogBase
    {
        RecordIdentifier priceGroupID;
        RecordIdentifier storeID;
        WeakReference priceGroupEditor;

        private StoreInPriceGroup currentPriceGroupLine;        

        public StoreInPriceGroup CurrentPriceGroupLine
        {
            get
            {
                return currentPriceGroupLine;
            }
            set
            {
                currentPriceGroupLine = value;
            }
        }

        public SelectPriceGroupDialog(RecordIdentifier id, bool isStore)
            :this()
        {
            if (isStore)
            {
                storeID = id;
                DataEntity store = Providers.StoreData.GetStoreEntity(PluginEntry.DataModel, storeID);
                txtStore.Text = (string)store.ID + " - " + store.Text;
            }
            else
            {
                priceGroupID = id;

                RecordIdentifier combinedGroupID = new RecordIdentifier((int)PriceDiscountModuleEnum.Customer, new RecordIdentifier((int)PriceDiscGroupEnum.PriceGroup, priceGroupID));

                DataEntity priceGroup = Providers.PriceDiscountGroupData.Get(PluginEntry.DataModel, combinedGroupID);
                cmbPriceGroup.SelectedData = priceGroup;
                
                cmbPriceGroup.Enabled = false;

                btnAddPriceGroup.Visible = false;
            }            
        }


        public SelectPriceGroupDialog()
            : base()
        {
            InitializeComponent();

            cmbPriceGroup.SelectedData = new DataEntity("", "");
            currentPriceGroupLine = null;

            IPlugin plugin;
            plugin = PluginEntry.Framework.FindImplementor(this, "CanAddPriceGroup", null);

            priceGroupEditor = (plugin != null) ? new WeakReference(plugin) : null;

            btnAddPriceGroup.Visible = (priceGroupEditor != null);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier ID
        {
            get { return new RecordIdentifier(priceGroupID, storeID); }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {            
            priceGroupID = cmbPriceGroup.SelectedData.ID;

            if (Providers.PriceDiscountGroupData.StoreIsInPriceGroup(PluginEntry.DataModel, storeID, priceGroupID))
            {
                errorProvider1.SetError(txtStore, Properties.Resources.StoreAlreadyInPriceGroup);
                txtStore.Focus();
                return;
            }
            else
            {
                Providers.PriceDiscountGroupData.AddStoreToPriceGroup(PluginEntry.DataModel, storeID, priceGroupID);

                currentPriceGroupLine = Providers.PriceDiscountGroupData.GetStoreInPriceGroup(PluginEntry.DataModel, storeID, priceGroupID);

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = cmbPriceGroup.SelectedData.ID != "";

        }

        private void RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = (string)((DataEntity)e.Data).ID + " - " + ((DataEntity)e.Data).Text;
            }
        }

        //private void cmbStore_RequestData(object sender, EventArgs e)
        //{
        //    cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        //}

        private void cmbPriceGroup_RequestData(object sender, EventArgs e)
        {
            cmbPriceGroup.SetData(Providers.PriceDiscountGroupData.GetGroupList(PluginEntry.DataModel, PriceDiscountModuleEnum.Customer, PriceDiscGroupEnum.PriceGroup), null);
        }

        private void btnAddPriceGroup_Click(object sender, EventArgs e)
        {
            if (priceGroupEditor.IsAlive)
            {
                ((IPlugin)priceGroupEditor.Target).Message(this, "AddPriceGroup", null);
            }
        }
        
    }
}
