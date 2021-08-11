using System;
using System.Collections.Generic;
using System.Linq;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Scheduler.Properties;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class LocationView : ViewBase
    {
        private Guid locationId;
        private JscLocation locationItem;
        private TabControl.Tab replicationTab;
        private TabControl.Tab memberTab;

        IEnumerable<IDataEntity> recordBrowsingContext;

        private ViewPages.LocationReplicationPage lastLocationReplicationPage;

        private ContextBarItem testConnectionItem;
        private ContextBarItem readDesignItem;


        public LocationView(Guid locationId)
            : this()
        {
            this.locationId = locationId;
        }
        public LocationView(RecordIdentifier locationId)
            : this()
        {
            locationItem =  DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                .GetLocationByExId(PluginEntry.DataModel, locationId);
            this.locationId = (Guid) locationItem.ID;
        }

        public LocationView(Guid locationId, IEnumerable<IDataEntity> recordBrowsingContext)
            : this()
	    {
            this.recordBrowsingContext = recordBrowsingContext;
            this.locationId = locationId;
	    }

        private LocationView()
        {
            InitializeComponent();
            GrayHeaderHeight = tabControl.Location.Y - 4;
            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.RecordCursor;

        }
        protected override ViewBase GetRecordCursorView(int cursorIndex)
        {
            if (recordBrowsingContext != null)
            {
                var id = recordBrowsingContext.ElementAt(cursorIndex).ID;

                if (DataProviderFactory.Instance.Get<ILocationData, JscLocation>().Exists(PluginEntry.DataModel, id))
                {
                    return new LocationView((Guid)recordBrowsingContext.ElementAt(cursorIndex).ID, recordBrowsingContext);
                }
            }

            return null;
        }

        protected override void GetRecordCursorInfo(ContextBarCursorEventArguments args)
        {
            if (recordBrowsingContext != null)
            {
                args.Position = 0;
                args.Count = recordBrowsingContext.Count();

                foreach (IDataEntity entity in recordBrowsingContext)
                {
                    if (entity.ID == locationId)
                    {
                        return;
                    }

                    args.Position++;
                }
            }
            else
            {
                args.Count = 1;
                args.Position = 0;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.LocationDescription;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                HeaderText = Properties.Resources.Location;
                ReadOnly = !PluginEntry.DataModel.HasPermission(SchedulerPermissions.LocationEdit);

                replicationTab = new TabControl.Tab(Properties.Resources.LocationReplication, ViewPages.LocationReplicationPage.CreateInstance);
                tabControl.AddTab(replicationTab);
                memberTab = new TabControl.Tab(Properties.Resources.LocationMember,
                    ViewPages.LocationMembersPage.CreateInstance);

                tabControl.AddTab(memberTab);

                // Allow other plugins to extend this tab panel
                tabControl.Broadcast(this, locationId);

                ViewPages.LocationReplicationPage locationReplicationPage = replicationTab.View as ViewPages.LocationReplicationPage;
                if (!object.ReferenceEquals(lastLocationReplicationPage, locationReplicationPage))
                {
                    locationReplicationPage.DatabaseConnectionChanged += locationReplicationPage_DatabaseConnectionChanged;
                    lastLocationReplicationPage = locationReplicationPage;
                }
            }

            if (locationItem == null)
            {
                locationItem = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocation(PluginEntry.DataModel, this.locationId);
            }
            
            tbType.Text = Utils.Utils.EnumResourceString(Resources.ResourceManager, locationItem.LocationKind);
            tbName.Text = locationItem.Text;
            tbName.ReadOnly = this.ReadOnly || NameIsReadOnly(locationItem);
            tbExCode.Text = locationItem.ExCode;
            chkEnabled.Checked = locationItem.Enabled;
            tabControl.SetData(isRevert, null, new ViewPages.LocationReplicationPage.InternalContext { LocationItem = locationItem });
        }

        // A flag storing the validity of the connection to the DD, used when the ContextBarItem object have not yet been created.
        private bool connectionValid;

        private void locationReplicationPage_DatabaseConnectionChanged(object sender, ViewPages.DatabaseConnectionChangedEventArgs e)
        {
            connectionValid = e.IsValid;
            UpdateActions();
        }

        private void UpdateActions()
        {
            if (testConnectionItem != null)
            {
                testConnectionItem.Enabled = connectionValid;
            }
            if (readDesignItem != null)
            {
                readDesignItem.Enabled = connectionValid;
            }
        }

        
        private bool NameIsReadOnly(JscLocation locationItem)
        {
            return
                locationItem.LocationKind == LocationKind.Store ||
                locationItem.LocationKind == LocationKind.Terminal;
        }




        protected override bool DataIsModified()
        {
            if (tabControl.IsModified())
                return true;

            if (tbName.Text != (locationItem.Text ?? ""))
                return true;
            
            if (chkEnabled.Checked != locationItem.Enabled)
                return true;

            return false;
        }

        protected override bool SaveData()
        {
            locationItem.Text = tbName.Text;
            locationItem.Enabled = chkEnabled.Checked;

            if (tabControl.IsModified())
            {
                tabControl.GetData();
            }

            DataProviderFactory.Instance.Get<ILocationData, JscLocation>().Save(PluginEntry.DataModel,locationItem);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "Location", locationItem.ID, null);

            return true;
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                testConnectionItem = new ContextBarItem(Properties.Resources.LocationListViewTestConnection, TestConnectionHandler);
                arguments.Add(testConnectionItem, 300);

                if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.JobSubjobEdit) && PluginEntry.DataModel.HasPermission(SchedulerPermissions.DatabaseDesignEdit))
                {
                    readDesignItem = new ContextBarItem(Properties.Resources.LocationListViewReadDesign, ReadDesignHandler);
                    arguments.Add(readDesignItem, 300);
                }
            }
            base.OnSetupContextBarItems(arguments);
            UpdateActions();
        }


        private void TestConnectionHandler(object sender, ContextBarClickEventArguments args)
        {
            bool doTest = true;
            if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.LocationEdit))
            {
                doTest = SaveData();
            }

            if (doTest)
            {
                ((ViewPages.LocationReplicationPage)replicationTab.View).TestConnection();
            }
        }

        private void ReadDesignHandler(object sender, ContextBarClickEventArguments args)
        {
            if (SaveData())
            {
                if (((ViewPages.LocationReplicationPage)replicationTab.View).ReadDesign())
                {
                    SaveData();
                }
            }
        }
    }
}
