using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Functionality
{
    public partial class FunctionalProfileReplicationPage : UserControl, ITabView
    {
        private FunctionalityProfile functionalityProfile;
        private bool newDDJobSelect = false; //In case replication data is not on this location

        public FunctionalProfileReplicationPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new FunctionalProfileReplicationPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            functionalityProfile = (FunctionalityProfile)internalContext;

   

    

            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "GetDataDirectorJobs", null);
            if (plugin != null)
            {
                List<DataEntity> jobs = (List<DataEntity>)plugin.Message(this, "GetDataDirectorJobs", null);
                cmbDDJobs.Items.Add(new DataEntity(RecordIdentifier.Empty, "None"));
                foreach (DataEntity dataEntity in jobs)
                {
                    int index = cmbDDJobs.Items.Add(dataEntity);
                    if (functionalityProfile.PostTransactionDDJob == dataEntity.ID.ToString())
                    {
                        cmbDDJobs.SelectedIndex = index;
                    }
                }
            }
            
            ddcbSchedulerLocation.Text = functionalityProfile.DDSchedulerLocation;
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
          
            if (newDDJobSelect) return true;
            if (functionalityProfile.DDSchedulerLocation != ddcbSchedulerLocation.Text) return true;

            return false;
        }

        public bool SaveData()
        {
        

            functionalityProfile.DDSchedulerLocation = ddcbSchedulerLocation.Text;

            functionalityProfile.PostTransactionDDJob = cmbDDJobs.SelectedItem != null ? ((DataEntity) cmbDDJobs.SelectedItem).ID.ToString() : "";

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

     

     

        private void ddcbSchedulerLocation_RequestData(object sender, EventArgs e)
        {
            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "GetDataDirectorJobs", null);
            if (plugin != null)
            {
                List<DataEntity> locations = (List<DataEntity>)plugin.Message(this, "GetDataDirectorLocations", null);
                List<DataEntity> locationsText = new List<DataEntity>();
                foreach (DataEntity dataEntity in locations)
                {
                    locationsText.Add(new DataEntity(dataEntity.ID.SecondaryID, dataEntity.Text));
                }

                ddcbSchedulerLocation.SetData(locationsText, null);
            }
        }

        private void ddcbSchedulerLocation_RequestClear(object sender, EventArgs e)
        {
            ddcbSchedulerLocation.Text = "";
        }

        private void ddcbSchedulerLocation_SelectedDataChanged(object sender, EventArgs e)
        {
            if (ddcbSchedulerLocation.SelectedData != null)
            {
                ddcbSchedulerLocation.Text = (string) ddcbSchedulerLocation.SelectedData.ID;
            }
        }

        private void cmbDDJobs_SelectedValueChanged(object sender, EventArgs e)
        {
            newDDJobSelect = cmbDDJobs.SelectedItem != null && ((DataEntity) cmbDDJobs.SelectedItem).ID != functionalityProfile.DDSchedulerLocation;
        }
    }
}
