using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.DDBusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class LocationSelectPanel : UserControl, IControlClosable
    {
        // A cached array of all locations
        private JscLocation[] locations;

        #pragma warning disable 0067 // We suppress this warning until we actually implement the RequestClear on all forms but its needed for the interface to have it in until then.
        public event EventHandler RequestClear;
        public event EventHandler RequestNoChange;
#pragma warning restore 0067

        public LocationSelectPanel()
        {
            InitializeComponent();
        }

        public LocationSelectPanel(IEnumerable<JscLocation> locations, Guid? preselectedLocationId, bool enableTreeView = true)
        {
            InitializeComponent();
            this.locations = locations.ToArray();
            this.EnableTreeView = enableTreeView;
            LoadData(preselectedLocationId);
        }


        public bool SelectNoneAllowed { get; set; }
        public bool NoChangeAllowed { get; set; }

        public bool EnableTreeView { get; set; }

        private void LoadData(Guid? preselectedLocationId)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (EnableTreeView)
            {
                ShowTreeView();
                locationTreeControl.LoadData(locations);
                if (preselectedLocationId.HasValue)
                {
                    locationTreeControl.SelectedLocationId = preselectedLocationId.Value;
                }
            }
            else
            {
                ShowListView();
                locationListControl.LoadData(GetFilteredLocations());
                if (preselectedLocationId.HasValue)
                {
                    locationListControl.SelectedLocationId = preselectedLocationId.Value;
                }
            }
        }


        private void UpdateActions()
        {
            if (locationTreeControl.Visible)
            {
                btnOK.Enabled = locationTreeControl.SelectedLocation != null;
            }
            else
            {
                btnOK.Enabled = locationListControl.SelectedLocation != null;
            }
        }



        private void ShowTreeView()
        {
            this.SuspendLayout();
            locationListControl.Visible = false;
            locationTreeControl.Visible = true;
            locationTreeControl.Dock = DockStyle.Fill;
            locationTreeControl.BringToFront();
            this.ResumeLayout();
        }


        private void ShowListView()
        {
            this.SuspendLayout();
            locationTreeControl.Visible = false;
            locationListControl.Visible = true;
            locationListControl.Dock = DockStyle.Fill;
            locationListControl.BringToFront();
            this.ResumeLayout();
        }



        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbSearch.TextLength == 0 && EnableTreeView)
            {
                ShowTreeView();
            }
            else
            {
                ShowListView();
                locationListControl.LoadData(GetFilteredLocations());
            }
        }

        private IEnumerable<JscLocation> GetFilteredLocations()
        {
            foreach (var location in locations)
            {
                if (TextMatch(location, tbSearch.Text))
                    yield return location;
            }
        }

        private bool TextMatch(JscLocation location, string searchText)
        {
            return
                location.Text != null && location.Text.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                location.ExCode != null && location.ExCode.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private void locationTreeControl_LocationSelected(object sender, LocationSelectedEventArgs e)
        {
            UpdateActions();
        }

        private void locationListControl_LocationsSelected(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            IDropDownForm form = ((IDropDownForm)this.FindForm());
            if (locationTreeControl.Visible)
            {
                form.SelectedData = locationTreeControl.SelectedLocation;
            }
            else
            {
                form.SelectedData = locationListControl.SelectedLocation;
            }
            form.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            IDropDownForm form = ((IDropDownForm)this.FindForm());
            form.Close();
        }

        void IControlClosable.OnClose()
        {
        }

        Control IControlClosable.EmbeddedControl
        {
            get { return this; }
        }

        
        private void locationListControl_DoubleClick(object sender, EventArgs e)
        {
            if (btnOK.Enabled)
            {
                btnOK_Click(btnOK, e);
            }
        }

        private void locationTreeControl_LocationDoubleClicked(object sender, LocationSelectedEventArgs e)
        {
            if (btnOK.Enabled)
            {
                btnOK_Click(btnOK, e);
            }
        }
    }




    
}
