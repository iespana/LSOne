using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Scheduler.Controls;
using LSRetail.DD.Common;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public partial class LocationReplicationPage : UserControl, ITabView
    {

        private RecordIdentifier locationID;
        private JscLocation locationItem;
        private Dialogs.DataDirectorDialog dataDirectorDialog = new Dialogs.DataDirectorDialog();

        public class InternalContext
        {
            public JscLocation LocationItem { get; set; }
        }

        public LocationReplicationPage()
        {
            InitializeComponent();
        }


        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new LocationReplicationPage();
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
                locationItem = ((InternalContext)internalContext).LocationItem;
                databaseStringFieldsControl.DatabaseDriverTypes = PluginUtils.LoadSchedulerDriverTypes();
            }

            LocationToForm();
            UpdateDatabaseControls();
        }

        private void LocationToForm()
        {
            tbDDHost.Text = locationItem.DDHost;
            tbDDPort.Text = locationItem.DDPort;
            cmbNetMode.SelectedIndex = (int) locationItem.DDNetMode;
            
            chkDBServerIsUsed.Checked = locationItem.DBServerIsUsed;
            databaseStringFieldsControl.IsDDString = true;
            databaseStringFieldsControl.DatabaseString = new DatabaseString(true, locationItem.DBConnectionString);
            if (locationItem.DatabaseDesign == null)
            {
                cmbDatabaseDesign.DatabaseDesignId = null;
            }
            else
            {
                cmbDatabaseDesign.DatabaseDesignId = (Guid) locationItem.DatabaseDesign;
            }
        }

        private void LocationFromFrom()
        {
            locationItem.DDHost = string.IsNullOrEmpty(tbDDHost.Text) ? null : tbDDHost.Text;
            locationItem.DDPort = string.IsNullOrEmpty(tbDDPort.Text) ? null : tbDDPort.Text;
            locationItem.DDNetMode = (NetMode)cmbNetMode.SelectedIndex;

            locationItem.DBServerIsUsed = chkDBServerIsUsed.Checked;
            locationItem.DBDriverType = null;

            locationItem.Company = null;
            locationItem.UserId = null;
            locationItem.Password = null;
            locationItem.DBServerHost = null;
            locationItem.DBPathName = null;
            var databaseString = databaseStringFieldsControl.DatabaseString;
            if (databaseString != null)
            {
                locationItem.DBConnectionString = databaseString.ToString(true);
            }
            else
            {
                locationItem.DBConnectionString = null;
            }
            locationItem.DatabaseDesign = cmbDatabaseDesign.DatabaseDesignId ?? Guid.Empty;
        }

        public bool DataIsModified()
        {
            if (locationItem.DDHost != (string.IsNullOrEmpty(tbDDHost.Text) ? null : tbDDHost.Text))
                return true;

            if (locationItem.DDPort != (string.IsNullOrEmpty(tbDDPort.Text) ? null : tbDDPort.Text))
                return true;

            if (locationItem.DDNetMode != (NetMode) cmbNetMode.SelectedIndex)
                return true;

            if (locationItem.DBServerIsUsed != chkDBServerIsUsed.Checked)
                return true;

            if (!DatabaseString.Equals(databaseStringFieldsControl.DatabaseString, locationItem.DBConnectionString))
            {
                return true;
            }

            if (locationItem.DatabaseDesign != cmbDatabaseDesign.DatabaseDesignId)
                return true;

            return false;
        }



        public bool SaveData()
        {
            Validate();
            LocationFromFrom();
            return true;
        }



        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
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


        private void UpdateDatabaseControls()
        {
            databaseStringFieldsControl.Enabled = chkDBServerIsUsed.Checked;
            cmbDatabaseDesign.Enabled = chkDBServerIsUsed.Checked;
            OnDatabaseConnectionChanged(databaseStringFieldsControl.DatabaseString != null);
        }




        private void OnDatabaseConnectionChanged(bool isValid)
        {
            if (DatabaseConnectionChanged != null)
            {
                DatabaseConnectionChanged(this, new DatabaseConnectionChangedEventArgs { IsValid = isValid });
            }
        }


        private void chkDBServerIsUsed_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDatabaseControls();
        }


        internal void TestConnection()
        {
            dataDirectorDialog.TestConnection(PluginEntry.Framework.MainWindow, locationItem);
        }

        internal bool ReadDesign()
        {
            // Determine if we allow the user to update or merge new data into existing design or if we
            // simply create a new one.
            bool allowUpdateExising = false;
            string existingDescription = null;
            if (locationItem.DatabaseDesign != null && !locationItem.DatabaseDesign.IsEmpty)
            {
                JscDatabaseDesign databaseDesign = DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetDatabaseDesign(PluginEntry.DataModel, locationItem.DatabaseDesign);
                existingDescription = databaseDesign.Description;
                allowUpdateExising = true;
            }

            // Show a dialog prompting for read options
            bool readTablesAndFields;
            bool updateExisting;
            string newDescription = null;
            using (Dialogs.ReadDesignDialog readDesignDialog = new Dialogs.ReadDesignDialog())
            {
                if (readDesignDialog.ShowDialog(PluginEntry.Framework.MainWindow, allowUpdateExising, existingDescription) == DialogResult.Cancel)
                {
                    return false;
                }
                readTablesAndFields = readDesignDialog.ReadTablesAndFields;
                updateExisting = readDesignDialog.UpdateExistingDatabaseDesign;
                if (!updateExisting)
                {
                    newDescription = readDesignDialog.NewDescription;
                }
            }

            bool result;

            if (readTablesAndFields)
                result = dataDirectorDialog.ReadTablesAndFieldsDesign(this, locationItem, updateExisting, newDescription);
            else
                result = dataDirectorDialog.ReadTablesDesign(this, locationItem, updateExisting, newDescription);
            
            if (result && !updateExisting)
            {
                cmbDatabaseDesign.DatabaseDesignId = (Guid)locationItem.DatabaseDesign;
                cmbDatabaseDesign.InvalidateData();
            }

            return result;
        }


        internal event EventHandler<DatabaseConnectionChangedEventArgs> DatabaseConnectionChanged;


        private void cmbNetMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            NetMode netMode = (NetMode) cmbNetMode.SelectedIndex;

            int defaultPort = AppConfig.GetRouterPortByMode(netMode);
            tbDDPort.GhostText = defaultPort.ToString();
        }


        private void databaseStringFieldsControl_DatabaseStringChanged(object sender, EventArgs e)
        {
            OnDatabaseConnectionChanged(databaseStringFieldsControl.DatabaseString != null);
        }


    }


    internal class DatabaseConnectionChangedEventArgs : EventArgs
    {
        public bool IsValid { get; set; }
    }

}
