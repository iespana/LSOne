using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class SelectLocationsDialog : DialogBase
    {
        private List<JscLocation> selectedLocations;

        public SelectLocationsDialog()
        {
            InitializeComponent();
        }

        private void UpdateButtonStates()
        {
            btnOK.Enabled = lcLocations.SelectedLocations.Length > 0;
        }

        public DialogResult ShowDialog(IWin32Window owner, string header, IEnumerable<JscLocation> locations)
        {
            Header = header;
            lcLocations.LoadData(locations);
            return ShowDialog(owner);
        }


        public bool MultiSelect
        {
            get { return lcLocations.MultiSelect; }
            set { lcLocations.MultiSelect = value; }
        }

        public IList<JscLocation> SelectedLocations
        {
            get
            {
                if (selectedLocations == null)
                {
                    selectedLocations = new List<JscLocation>();
                    foreach (var location in lcLocations.SelectedLocations)
                    {
                        selectedLocations.Add(location);
                    }
                }
                return selectedLocations;
            }
        }

        private void lcLocations_LocationsSelected(object sender, EventArgs e)
        {
            selectedLocations = null;
            UpdateButtonStates();
        }
    }
}
