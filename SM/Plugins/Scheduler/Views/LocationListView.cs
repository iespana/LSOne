using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Scheduler.Dialogs;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Views
{
    public partial class LocationListView : ViewBase
    {
        private static Guid BarSettingID = new Guid("A43DD493-04AA-4684-8CC0-C34FC0F4A676");
        private Setting searchBarSetting;

        private SelectLocationsDialog selectLocationsDialog = new SelectLocationsDialog();
        private NewLocationDialog newLocationDialog = new NewLocationDialog();
        private List<JscLocation> members;
        private List<JscLocation> locations;

        public LocationListView()
        {
            InitializeComponent();
            Attributes = 
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar;

            lcLocations.ContextMenuStrip = new ContextMenuStrip();
            lcLocations.ContextMenuStrip.Opening += new CancelEventHandler(lcLocations_Opening);

            lcMembers.ContextMenuStrip = new ContextMenuStrip();
            lcMembers.ContextMenuStrip.Opening += new CancelEventHandler(lcMembers_Opening);

            searchBar.BuddyControl = lcLocations;
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.LocationListDescription;
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
            HeaderText = Properties.Resources.LocationList;

            Synchronize();
            RefreshLocations();  
            UpdateActions();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "Location" || objectName == "Store" || objectName == "Terminal")
            {
                LoadData(false);
            }
        }

        private void Synchronize()
        {
            if (PluginEntry.DataModel.HasPermission(SchedulerPermissions.LocationEdit))
            {
                labelSynchronizing.Visible = true;
                DataProviderFactory.Instance.Get<ILocationData, JscLocation>().SynchronizeLocations(PluginEntry.DataModel);
                labelSynchronizing.Visible = false;
            }
        }

        private void RefreshLocations()
        {
            locations =
                DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetLocations(PluginEntry.DataModel, true).ToList();
            DisplayLocations();
        }

        private void RefreshMembers()
        {
            JscLocation location = lcLocations.SelectedLocation;
            if (location == null)
                return;
            members = DataProviderFactory.Instance.Get<ILocationData, JscLocation>()
                .GetMembers(PluginEntry.DataModel, location.ID);
            lcMembers.LoadData(members);
        }

        private void AddLocationClicked(object sender, EventArgs e)
        {
            if (newLocationDialog.ShowDialog(PluginEntry.Framework.MainWindow) != DialogResult.OK)
            {
                return;
            }

            locations.Add(newLocationDialog.LocationItem);
            DisplayLocations();

            // Open up the detail view page
            ShowLocation(newLocationDialog.LocationItem);
        }

        private void EditLocationClicked(object sender, EventArgs e)
        {
            JscLocation location = lcLocations.SelectedLocation;
            if (location != null)
            {
                ShowLocation(location);
            }
        }

        private void RemoveLocationClicked(object sender, EventArgs e)
        {
            JscLocation location = lcLocations.SelectedLocation;
            if (location == null)
            {
                return;
            }

            if (QuestionDialog.Show(
                Properties.Resources.LocationQuestionDelete,
                Properties.Resources.LocationDelete) != DialogResult.Yes)
            {
                return;
            }

            DataProviderFactory.Instance.Get<ILocationData, JscLocation>().DeleteLocationAndMemberships(PluginEntry.DataModel, location);
            locations.Remove(location);
            DisplayLocations();
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "Location", null, null);
        }

        private void ShowLocation(JscLocation location)
        {
            PluginEntry.Framework.ViewController.Add(new Views.LocationView((Guid) location.ID,locations));
        }

        private void AddMemberClicked(object sender, EventArgs e)
        {
            JscLocation location = lcLocations.SelectedLocation;
            if (location != null)
            {
                var locations = DataProviderFactory.Instance.Get<ILocationData, JscLocation>().GetNewMemberList(PluginEntry.DataModel, location);
                if (locations.Count == 0)
                {
                    MessageBox.Show(this, Properties.Resources.NoLocationsForMembershipMessage, Properties.Resources.AddMemberCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string header = string.Format(Properties.Resources.SelectMembersHeader, location.Text);
                selectLocationsDialog.MultiSelect = true;
                if (selectLocationsDialog.ShowDialog(PluginEntry.Framework.MainWindow, header, locations) == DialogResult.OK)
                {
                    DataProviderFactory.Instance.Get<ILocationData, JscLocation>().AddMembers(PluginEntry.DataModel, location, selectLocationsDialog.SelectedLocations);
                    lcLocations.UpdateLocation(location);
                }
                RefreshMembers();
            }
        }

        private void locationListControl_LocationsSelected(object sender, EventArgs e)
        {
            UpdateActions();
            RefreshMembers();
        }

        private void UpdateActions()
        {
            bool isItemSelected = lcLocations.SelectedLocation != null;
            bool hasPermission = PluginEntry.DataModel.HasPermission(SchedulerPermissions.LocationEdit);

            contextButtonsLocations.EditButtonEnabled = isItemSelected;
            contextButtonsLocations.AddButtonEnabled = hasPermission;
            contextButtonsLocations.RemoveButtonEnabled = hasPermission && isItemSelected;

            contextButtonsMembers.AddButtonEnabled = isItemSelected && hasPermission;
            contextButtonsMembers.RemoveButtonEnabled = hasPermission && lcMembers.SelectedLocations.Length > 0;
        }

        private void locationListControl_DoubleClick(object sender, EventArgs e)
        {
            if (contextButtonsLocations.EditButtonEnabled)
            {
                EditLocationClicked(sender, EventArgs.Empty);
            }
        }

        private void lcMembers_LocationsSelected(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void RemoveMemberClicked(object sender, EventArgs e)
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
                DataProviderFactory.Instance.Get<ILocationData, JscLocation>().RemoveMembers(PluginEntry.DataModel, lcLocations.SelectedLocation, lcMembers.SelectedLocations);
                lcLocations.UpdateLocation(lcLocations.SelectedLocation);
                RefreshMembers();
            }
        }


        private void lcLocations_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lcLocations.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    EditLocationClicked);

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = contextButtonsLocations.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    AddLocationClicked);

            item.Enabled = contextButtonsLocations.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    RemoveLocationClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = contextButtonsLocations.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("LocationListViewLocations", lcLocations.ContextMenuStrip, lcLocations);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lcMembers_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lcMembers.ContextMenuStrip;

            menu.Items.Clear();

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(AddMemberClicked));

            item.Enabled = contextButtonsMembers.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(RemoveMemberClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = contextButtonsMembers.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("LocationListViewMembers", lcMembers.ContextMenuStrip, lcMembers);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            DisplayLocations();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> comboBoxTypes = new List<object>()
            {
                Properties.Resources.General,
                Properties.Resources.Store,
                Properties.Resources.Terminal
            };

            searchBar.AddCondition(new ConditionType(Properties.Resources.IDOrDescription, "IDOrDescription", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Properties.Resources.Type, "Type", ConditionType.ConditionTypeEnum.ComboBox, comboBoxTypes, initialIndex: 0));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void DisplayLocations()
        {
            lcMembers.Clear();
            lcLocations.Clear();

            string idOrDescription = "";
            bool idOrDescriptionBeginsWith = false;
            LocationKind locationKind = LocationKind.Undefined;

            foreach (SearchParameterResult result in searchBar.SearchParameterResults)
            {
                switch (result.ParameterKey)
                {
                    case "IDOrDescription":
                        idOrDescription = result.StringValue;
                        idOrDescriptionBeginsWith = result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith;
                        break;
                    case "Type":
                        switch (result.ComboSelectedIndex)
                        {
                            case 0: locationKind = LocationKind.General; break;
                            case 1: locationKind = LocationKind.Store; break;
                            case 2: locationKind = LocationKind.Terminal; break;
                        }
                        break;
                }
            }

            List<JscLocation> filteredLocations = locations.Where(x => (locationKind == LocationKind.Undefined || x.LocationKind == locationKind)
                                                                && (idOrDescription == "" || (idOrDescriptionBeginsWith ? x.Text.StartsWith(idOrDescription, StringComparison.InvariantCultureIgnoreCase) : x.Text.IndexOf(idOrDescription, StringComparison.InvariantCultureIgnoreCase) >= 0)
                                                                                          || (idOrDescriptionBeginsWith ? x.ExCode.StartsWith(idOrDescription, StringComparison.InvariantCultureIgnoreCase) : x.ExCode.IndexOf(idOrDescription, StringComparison.InvariantCultureIgnoreCase) >= 0))).ToList();

            lcLocations.LoadData(filteredLocations);
        }

        private void lcLocations_SizeChanged(object sender, EventArgs e)
        {
            lblLocations.Top = lcLocations.Top + 3;
        }
    }
}
