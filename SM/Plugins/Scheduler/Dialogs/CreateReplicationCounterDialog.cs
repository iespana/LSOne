using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class CreateReplicationCounterDialog : DialogBase
    {
        private JscRepCounter repCounter;
        private JscLocation selectedLocation;
        private JscSubJob subJob;
        private JscJob job;
        private List<JscLocation> locations;
        private Guid locationFilterGuid;        

        /// <summary>
        /// Initializes the dialog with the given subjob and location filter ID. 
        /// </summary>
        /// <param name="subJob">The subjob to create the counter for</param>
        /// <param name="job">The job to create the counter for</param>
        public CreateReplicationCounterDialog(JscSubJob subJob, JscJob job)
        {
            InitializeComponent();
            
            this.subJob = subJob;
            repCounter = new JscRepCounter();
            locations = new List<JscLocation>();
            this.job = job;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            IEnumerable<JscRepCounter> existingCounters = DDProviders.JobData.GetReplicationCounters(PluginEntry.DataModel, subJob.ID);

            if (existingCounters.Any(x => x.Location == selectedLocation.ID))
            {
                errorProvider.SetError(cmbLocation, Properties.Resources.CounterAlreadyExistsForLocation);
                return;
            }

            JscRepCounter repCounter = new JscRepCounter();
            repCounter.ID = Guid.NewGuid();
            repCounter.Job = job.ID;
            repCounter.SubJob = subJob.ID;
            repCounter.Location = selectedLocation.ID;
            repCounter.Counter = Convert.ToInt32(ntbCounter.Value);

            DDProviders.JobData.Save(PluginEntry.DataModel, repCounter);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void cmbLocation_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> locations = new List<DataEntity>(DDProviders.LocationData.GetLocationsWhereConnectable(PluginEntry.DataModel));

            cmbLocation.SetData(locations, null, true);
        }

        private void cmbLocation_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbLocation.SelectedDataID != RecordIdentifier.Empty)
            {
                selectedLocation = (JscLocation)cmbLocation.SelectedData;
            }

            CheckEnabled(this, EventArgs.Empty);
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            errorProvider.Clear();
            btnOK.Enabled = cmbLocation.SelectedDataID != "" && ntbCounter.Value > 0;
        }

        private void CreateReplicationCounterDialog_Load(object sender, EventArgs e)
        {
            tbJob.Text = job.Text;
            tbSubJob.Text = subJob.Description;

            if (subJob.ReplicationMethod != ReplicationTypes.Procedure)
            {
                JscTableDesign tableDesign = DDProviders.DesignData.GetTableDesign(PluginEntry.DataModel, subJob.TableFrom);
                tbTable.Text = tableDesign.TableName;
            }

            locationFilterGuid = job.Source == null || job.Source == RecordIdentifier.Empty ? Guid.Empty : (Guid)job.JscSourceLocation.ID;

            if (locationFilterGuid != Guid.Empty)
            {
                JscLocation filterLocation = DDProviders.LocationData.GetLocation(PluginEntry.DataModel, new RecordIdentifier(locationFilterGuid));

                filterLocation.MemberLocations = DDProviders.LocationData.GetAllMemberships(PluginEntry.DataModel, filterLocation.ID, false);

                if (filterLocation.MemberLocations == null || filterLocation.MemberLocations.Count == 0)
                {
                    cmbLocation.SelectedData = filterLocation;
                    cmbLocation.Enabled = false;
                    selectedLocation = filterLocation;
                }
                else
                {
                    cmbLocation.SelectedData = new DataEntity("", "");
                }
            }
            else
            {
                cmbLocation.SelectedData = new DataEntity("", "");
            }

            if (subJob.ReplicationMethod == ReplicationTypes.Normal)
            {
                if (subJob.RepCounterField != RecordIdentifier.Empty)
                {
                    subJob.JscRepCounterField = DDProviders.DesignData.GetFieldDesign(PluginEntry.DataModel, subJob.RepCounterField);
                }
            }
        }

        private void btnReadCounter_Click(object sender, EventArgs e)
        {
            try
            {            
                if (subJob.ReplicationMethod == ReplicationTypes.Normal)
                {
                    JscTableDesign tableDesign = DDProviders.DesignData.GetTableDesign(PluginEntry.DataModel, subJob.TableFrom);

                    // Get the maximum counter value from the either local DB or remote location               
                    ntbCounter.Value = DDProviders.JobData.GetMaxReplicationCounterValue(PluginEntry.DataModel, tableDesign.TableName, subJob.JscRepCounterField.FieldName);
                }
                else
                {
                    ntbCounter.Value = DDProviders.ReplicationActionData.GetMaxActionID(PluginEntry.DataModel, subJob.JscActionTable.TableName);
                }
            }
            catch
            {
                MessageDialog.Show(Properties.Resources.CouldNotReadCounter, MessageBoxButtons.OK);
            }
        }

        private void cmbLocation_DropDown(object sender, LSOne.Controls.DropDownEventArgs e)
        {
            if (locations.Count == 0)
            {                
                if (locationFilterGuid != Guid.Empty)
                {
                    // The reason for checking only members is that if the given location is a "group". If the given location was 
                    // a single location then the combobox is not enabled.
                    locations = DDProviders.LocationData.GetMembers(PluginEntry.DataModel, new RecordIdentifier(locationFilterGuid));
                }
                else
                {
                    locations = DDProviders.LocationData.GetLocationsWhereConnectable(PluginEntry.DataModel).ToList();
                }
            }

            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            ((DualDataComboBox)sender).SkipIDColumn = true;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }

            var panel = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.Custom, showTextInitiallyHighlighted:textInitallyHighlighted, hideIDColumn:true);            
            panel.SetSearchHandler(SearchLocations, e.DisplayText);
            e.ControlToEmbed = panel;
        }

        private List<DataEntity> SearchLocations(object sender, SingleSearchArgs args)
        {
            List<DataEntity> locationEntities = locations.Cast<DataEntity>().ToList();

            if (string.IsNullOrEmpty(args.SearchText))
            {
                return locationEntities;
            }

            return locationEntities.Where(p => p.Text.IndexOf(args.SearchText, StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();           
        }
    }
}
