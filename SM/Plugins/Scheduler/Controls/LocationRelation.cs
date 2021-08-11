using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class LocationRelation : UserControl
    {

        private SelectLocationsDialog selectLocationsDialog = new SelectLocationsDialog();
        public JscLocation RelationLocation { get; set; }
        public bool IsParent { get; set; }
        public List<JscLocation>  LocationsList { get; set; }
        public LocationRelation()
        {
            InitializeComponent();
            lcMembers.ContextMenuStrip = new ContextMenuStrip();
            lcMembers.ContextMenuStrip.Opening += lvMembersContextMenu_Opening;
        }

        public void LoadData(string caption, List<JscLocation> locations)
        {
            lblCaption.Text = caption;
            LocationsList = locations;
            RefreshMembers();
        }

        private void RefreshMembers()
        {
            lcMembers.LoadData(LocationsList);
        }

        private void contextButtonsMembers_AddButtonClicked(object sender, System.EventArgs e)
        {
            JscLocation location = RelationLocation;
            if (location != null)
            {
                if (IsParent)
                {
                    var locations =
                        DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                            .GetNewMemberList(PluginEntry.DataModel, location);
                    if (locations.Count == 0)
                    {
                        MessageBox.Show(this, Properties.Resources.NoLocationsForMembershipMessage,
                            Properties.Resources.AddMemberCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string header = string.Format(Properties.Resources.SelectMembersHeader, location.Text);
                    selectLocationsDialog.MultiSelect = true;
                    if (selectLocationsDialog.ShowDialog(PluginEntry.Framework.MainWindow, header, locations) ==
                        DialogResult.OK)
                    {
                        DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                            .AddMembers(PluginEntry.DataModel, location, selectLocationsDialog.SelectedLocations);
                    }
                    LocationsList =
                        DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                            .GetMembers(PluginEntry.DataModel, RelationLocation.ID);
                    RefreshMembers();
                }
                else
                {
                    var locations =
                       DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                           .GetNewMemberList(PluginEntry.DataModel, location);
                    if (locations.Count == 0)
                    {
                        MessageBox.Show(this, Properties.Resources.NoLocationsForMembershipMessage,
                            Properties.Resources.AddMemberCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string header = string.Format(Properties.Resources.SelectParentsHeader, location.Text);
                    selectLocationsDialog.MultiSelect = true;
                    if (selectLocationsDialog.ShowDialog(PluginEntry.Framework.MainWindow, header, locations) ==
                        DialogResult.OK)
                    {
                        DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                            .AddParents(PluginEntry.DataModel, location, selectLocationsDialog.SelectedLocations);
                    }
                    LocationsList =
                        DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                            .GetParents(PluginEntry.DataModel, RelationLocation.ID);
                    RefreshMembers();
                }
            }
        }

        private void contextButtonsMembers_RemoveButtonClicked(object sender, System.EventArgs e)
        {
            bool doRemove = false;
            int count = lcMembers.SelectedLocations.Length;

            if (count == 1)
            {
                doRemove =
                    QuestionDialog.Show
                    (
                        Properties.Resources.LocationRemoveMemberQuestion,
                        Properties.Resources.LocationRemoveMember
                    ) == DialogResult.Yes;
            }
            else if (count > 1)
            {
                doRemove =
                    QuestionDialog.Show
                    (
                        string.Format(Properties.Resources.LocationRemoveManyMembersQuestion, count),
                        Properties.Resources.LocationRemoveMember
                    ) == DialogResult.Yes;
            }

            if (doRemove)
            {
                if (IsParent)
                {
                    DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                        .RemoveMembers(PluginEntry.DataModel, RelationLocation, lcMembers.SelectedLocations);
                    LocationsList =
                       DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                           .GetMembers(PluginEntry.DataModel, RelationLocation.ID);
                    RefreshMembers();
                }
                else
                {
                    DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                        .RemoveParents(PluginEntry.DataModel, RelationLocation, lcMembers.SelectedLocations);
                    LocationsList =
                       DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                           .GetParents(PluginEntry.DataModel, RelationLocation.ID);
                    RefreshMembers();
                }
            }
        }

        private void lvMembersContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu = lcMembers.ContextMenuStrip;
            menu.Items.Clear();

            item = new ExtendedMenuItem(Properties.Resources.Add, 100, contextButtonsMembers_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(Properties.Resources.Delete, 200, contextButtonsMembers_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage()
            };

            menu.Items.Add(item);

            e.Cancel = menu.Items.Count == 0;
        }
    }
}
