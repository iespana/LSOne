using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class HospitalitySetupView : ViewBase
    {

        private TabControl.Tab generalTab;
        //private TabControl.Tab dineInTab;
        //private TabControl.Tab deliveryAndTakeoutTab;
        //private TabControl.Tab reservationsTab;

        private HospitalitySetup hospitalitySetup;

        public HospitalitySetupView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Audit;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageHospitalitySetup);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("HospitalitySetup", RecordIdentifier.Empty, Properties.Resources.HospitalitySetup, true));
        }


        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.HospitalitySetup;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                 // If our sheet would be multi-instance sheet then we would return context identifier UUID here,
                 // such as User.GUID that identifies that particular User. For single instance sheets we return 
                // RecordIdentifier.Empty to tell the framework that there can only be one instace of this sheet, which will
                 // make the framework make sure there is only one instance in the viewstack.
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                // Do any possible re-load on rever logic here.
                generalTab = new TabControl.Tab(Properties.Resources.General, new PanelFactoryHandler(ViewPages.HospitalitySetupGeneralPage.CreateInstance));
                //dineInTab = new TabControl.Tab(Properties.Resources.DineIn, new PanelFactoryHandler(ViewPages.HospitalitySetupDineInPage.CreateInstance));
                //deliveryAndTakeoutTab = new TabControl.Tab(Properties.Resources.DeliveryAndTakeout, new PanelFactoryHandler(ViewPages.HospitalitySetupDeliveryAndTakeoutPage.CreateInstance));
                //reservationsTab = new TabControl.Tab(Properties.Resources.Reservations, new PanelFactoryHandler(ViewPages.HospitalitySetupReservationsPage.CreateInstance));

                tabSheetTabs.AddTab(generalTab);
                //tabSheetTabs.AddTab(dineInTab);
                //tabSheetTabs.AddTab(deliveryAndTakeoutTab);
                //tabSheetTabs.AddTab(reservationsTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, 0);
            }

            hospitalitySetup = Providers.HospitalitySetupData.Get(PluginEntry.DataModel);

            HeaderText = Properties.Resources.HospitalitySetup;
            //HeaderIcon = Properties.Resources.Hospitality16;

            tabSheetTabs.SetData(isRevert, "1", hospitalitySetup);
        }

        protected override bool DataIsModified()
        {
            // Here our sheet is supposed to figure out if something needs to be saved and return
            // true if something needs to be saved, else false.
            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            // Here we would let our sheet save our data.

            // We return true since saving was successful, if we would return false then
            // the viewstack will prevent other sheet from getting shown.
            tabSheetTabs.GetData();

            Providers.HospitalitySetupData.Save(PluginEntry.DataModel, hospitalitySetup);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "HospitalitySetup", hospitalitySetup.ID, null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            // We use this one if we want to listen to changes in the Viewstack, like was there a user 
            // changed on a user sheet in the viewstack ? And it matters to our sheet ? if so then no
            // probel we catch it here and react if needed

            if (objectName == "User")
            {
                if (changeHint == DataEntityChangeType.Delete)
                {
                    // User was deleted and we react on it here if it makes sense
                }
            }

        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {

        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                // In most case we would want to check permissions here since it would be likely that the
                // related sheets have different permission needs than the one your on, but its not always
                // the case.
                //if (PluginEntry.Connection.CheckPermission(Permission.SecurityManageUserPermissions))
                //{
                //}

            }
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
