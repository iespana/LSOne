using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.RetailItems.Views
{
    public partial class RetailGroupView : ViewBase
    {
        RecordIdentifier retailGroupID;
        RetailGroup retailGroup;

        private TabControl.Tab generalTab;
        //private LSRetail.StoreController.SharedCore.Controls.TabControl.Tab replenishmentTab;

        public RetailGroupView(RecordIdentifier groupID)
            : this()
        {
            retailGroupID = groupID;

            retailGroup = Providers.RetailGroupData.Get(PluginEntry.DataModel, retailGroupID);
        }

        public RetailGroupView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            retailGroup = new RetailGroup();

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageRetailGroups);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RetailGroup", retailGroupID, Properties.Resources.RetailGroup, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.RetailGroup;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        {
                return retailGroupID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                // Do any possible re-load on rever logic here.
                generalTab = new TabControl.Tab(Properties.Resources.General, ViewPages.RetailGroupGeneralPage.CreateInstance);

                tabSheetTabs.AddTab(generalTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, retailGroupID, retailGroup);
            }

            tbRetailGroup.Text = (string)retailGroup.ID;
            tbDescription.Text = retailGroup.Text;
                                        
            HeaderText = Properties.Resources.RetailGroup;
            //HeaderIcon = Properties.Resources.item_16;

            tabSheetTabs.SetData(isRevert, retailGroupID, retailGroup);
        }

        protected override bool DataIsModified()
        {
            if (tabSheetTabs.IsModified()) return true;
            if (tbRetailGroup.Text != retailGroup.ID) return true;
            if (tbDescription.Text != retailGroup.Text) return true;            

            return false;
        }

        protected override bool SaveData()
        {

            //retailGroup.ID           = tbRetailGroup.Text;
            retailGroup.Text         = tbDescription.Text;

            tabSheetTabs.GetData();

            Providers.RetailGroupData.Save(PluginEntry.DataModel, retailGroup);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "RetailGroup", retailGroup.ID, null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "RetailGroup":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == retailGroupID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    if (changeHint == DataEntityChangeType.Add)
                    {
                        LoadData(false);
                    }
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteRetailGroup(retailGroupID);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RetailGroup", retailGroupID, null);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
