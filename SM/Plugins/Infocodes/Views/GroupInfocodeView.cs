using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.Infocodes.Views
{
    public partial class GroupInfocodeView : ViewBase
    {

        private RecordIdentifier infocodeID;
        private Infocode infocode;
        private TabControl.Tab generalTab;
        //private TabControl.Tab configurationTab;
        //private TabControl.Tab printingTab;
        private UsageCategoriesEnum usageCategory;

        public GroupInfocodeView(RecordIdentifier infocodeID, UsageCategoriesEnum usageCategory = UsageCategoriesEnum.None)
            : this()
        {
            this.infocodeID = infocodeID;
            this.usageCategory = usageCategory;
        }


        private GroupInfocodeView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                         ViewAttributes.Save |
                         ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar |
                         ViewAttributes.Audit;

            HeaderText = Properties.Resources.InfocodeGroupSetup;

            ReadOnly = !PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit);

        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Infocode", infocodeID, Properties.Resources.InfocodeGroupSetup, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.InfocodeGroupSetup;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                // If our sheet would be multi-instance sheet then we would return context identifier UUID here,
                // such as User.GUID that identifies that particular User. For single instance sheets we return 
                // RecordIdentifier.Empty to tell the framework that there can only be one instance of this sheet, which will
                // make the framework make sure there is only one instance in the viewstack.
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                // Do any possible re-load on rever logic here.
                generalTab = new TabControl.Tab(Properties.Resources.General, new PanelFactoryHandler(ViewPages.CrossAndModifierInfocodeGeneralPage.CreateInstance));
                tabSheetTabs.AddTab(generalTab);
                
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, 0);
            }

            infocode = Providers.InfocodeData.Get(PluginEntry.DataModel, infocodeID);

            tabSheetTabs.SetData(isRevert, infocodeID, infocode);

            //HeaderText = Properties.Resources.;

        }

        protected override bool DataIsModified()
        {
            if (tabSheetTabs.IsModified()) return true;

            return false;
        }

        protected override bool SaveData()
        {
            // Here we would let our sheet save our data.

            // We return true since saving was successful, if we would return false then
            // the viewstack will prevent other sheet from getting shown.
            tabSheetTabs.GetData();

            Providers.InfocodeData.Save(PluginEntry.DataModel, infocode);
            
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Infocode", infocode.ID, null);

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
