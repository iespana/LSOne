using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class LocationMembersPage: UserControl, ITabView
    {

        private RecordIdentifier locationID;
        private JscLocation locationItem;
        private bool dataIsModified;
        //private bool sequenceModified;

        public LocationMembersPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new LocationMembersPage();
        }


        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "Location":
                    LoadData(true, locationID, locationItem);
                    break;
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (!isRevert)
            {
                locationID = context;
                locationItem = ((LocationReplicationPage.InternalContext)internalContext).LocationItem;

                if (locationItem == null)
                {
                    locationItem = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(PluginEntry.DataModel, this.locationID);
                }
                lrMembers.LoadData(Properties.Resources.LocationMember,DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetMembers(PluginEntry.DataModel, this.locationID ?? locationItem.ID));
                lrMembers.RelationLocation = locationItem;
                lrMembers.IsParent = true;
                lrParents.LoadData(Properties.Resources.LocationParent, DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetParents(PluginEntry.DataModel, this.locationID ?? locationItem.ID));
                lrParents.RelationLocation = locationItem;
                lrParents.IsParent = false;

            }
            
            
        }

        public bool DataIsModified()
        {
            return dataIsModified;
        }

        public bool SaveData()
        {
            
            dataIsModified = false;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //TODO implement
        }

        private void ClearDataIsModified(object arg)
        {
            dataIsModified = false;
        }
        
    }
}
