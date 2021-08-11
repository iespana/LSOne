using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.ViewCore;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.GenericConnector;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.ViewPlugins.Profiles.Dialogs;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Profiles.Properties;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class UserProfilesView : ViewBase
    {
        private static Guid BarSettingID = new Guid("0DE8B7E2-9356-4756-9B99-7801F2320622");
        private RecordIdentifier selectedID = "";
        private Setting searchBarSetting;
        private int lastSelectionCount;

        public UserProfilesView()
        {
            InitializeComponent();

            Attributes =
                    ViewAttributes.Help |
                    ViewAttributes.Close |
                    ViewAttributes.ContextBar;

            HeaderText = Resources.UserProfiles;

            lvUserProfiles.ContextMenuStrip = new ContextMenuStrip();
            lvUserProfiles.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvUserProfiles.SetSortColumn(0, true);

            searchBar.BuddyControl = lvUserProfiles;
            searchBar.FocusFirstInput();
            lastSelectionCount = 0;
            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.UserProfiles;
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
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "UserProfile")
            {
                ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
            }
        }

        private void LoadItems()
        {
            UserProfileFilter filter = GetUserProfileFilter();

            RecordIdentifier currentlySelectedID = selectedID;
            lvUserProfiles.ClearRows();
            selectedID = currentlySelectedID;

            List<UserProfile> userProfiles = Providers.UserProfileData.GetListAdvanced(PluginEntry.DataModel, filter);

            foreach(UserProfile profile in userProfiles)
            {
                Row row = new Row();

                row.AddText(profile.Text);
                row.AddText(profile.StoreName);
                row.AddText(profile.VisualProfileName);
                row.AddText(profile.LayoutName);
                row.AddText(profile.LanguageCode);
                row.AddText(string.IsNullOrEmpty(profile.KeyboardCode) ? Resources.StoreDefault : profile.KeyboardCode);
                row.AddCell(new CheckBoxCell(HasDiscounts(profile), false, CheckBoxCell.CheckBoxAlignmentEnum.Center));

                row.Tag = profile;
                lvUserProfiles.AddRow(row);

                if (selectedID == profile.ID)
                {
                    lvUserProfiles.Selection.Set(lvUserProfiles.RowCount - 1);
                }
            }

            lvUserProfiles_SelectionChanged(this, EventArgs.Empty);
            lvUserProfiles.AutoSizeColumns(false);
            HideProgress();
        }

        private bool HasDiscounts(UserProfile profile)
        {
            return profile.MaxLineDiscountAmount != 0.0m
                || profile.MaxLineDiscountPercentage != 0.0m
                || profile.MaxTotalDiscountAmount != 0.0m
                || profile.MaxTotalDiscountPercentage != 0.0m
                || profile.MaxLineReturnAmount != 0.0m
                || profile.MaxTotalReturnAmount != 0.0m;
        }

        public UserProfileFilter GetUserProfileFilter()
        {
            UserProfileFilter filter = new UserProfileFilter();
            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        filter.Description = result.StringValue;
                        filter.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "LanguageCode":
                        filter.LanguageCode = result.StringValue;
                        filter.LanguageCodeBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Store":
                        filter.StoreID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "VisualProfile":
                        filter.VisualProfileID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Layout":
                        filter.LayoutID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                }
            }

            if (lvUserProfiles.SortColumn == null)
            {
                lvUserProfiles.SetSortColumn(0, true);
            }

            int sortColumnIndex = lvUserProfiles.Columns.IndexOf(lvUserProfiles.SortColumn);
            filter.SortBackwards = !lvUserProfiles.SortedAscending;

            switch (sortColumnIndex)
            {
                case 0:
                    filter.Sort = UserProfileSortEnum.Description;
                    break;
                case 1:
                    filter.Sort = UserProfileSortEnum.Store;
                    break;
                case 2:
                    filter.Sort = UserProfileSortEnum.VisualProfile;
                    break;
                case 3:
                    filter.Sort = UserProfileSortEnum.Layout;
                    break;
                case 4:
                    filter.Sort = UserProfileSortEnum.LanguageCode;
                    break;
                case 5:
                    filter.Sort = UserProfileSortEnum.KeyboardLanguage;
                    break;
                default:
                    filter.Sort = UserProfileSortEnum.Description;
                    break;
            }

            return filter;
        }

        private List<UserProfile> GetSelectedUserProfiles()
        {
            List<UserProfile> selectedProfiles = new List<UserProfile>();

            for (int i = 0; i < lvUserProfiles.Selection.Count; i++)
            {
                UserProfile userProfile = (UserProfile)lvUserProfiles.Selection[i].Tag;
                selectedProfiles.Add(userProfile);
            }

            return selectedProfiles;
        }

        private void EditUserProfileVisualProfile(object sender, ContextBarClickEventArguments args)
        {
            if (lvUserProfiles.Selection.Count == 0)
                return;

            if (new EditUserProfileDialog(EditUserProfileBehaviourEnum.VisualProfile, GetSelectedUserProfiles()).ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void EditUserProfileLayout(object sender, ContextBarClickEventArguments args)
        {
            if (lvUserProfiles.Selection.Count == 0)
                return;

            if (new EditUserProfileDialog(EditUserProfileBehaviourEnum.Layout, GetSelectedUserProfiles()).ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void EditUserProfileStore(object sender, ContextBarClickEventArguments args)
        {
            if (lvUserProfiles.Selection.Count == 0)
                return;

            if(new EditUserProfileDialog(EditUserProfileBehaviourEnum.Store, GetSelectedUserProfiles()).ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void EditUserProfileVisualProfile(object sender, EventArgs args)
        {
            if (lvUserProfiles.Selection.Count == 0)
                return;

            if (new EditUserProfileDialog(EditUserProfileBehaviourEnum.VisualProfile, GetSelectedUserProfiles()).ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void EditUserProfileLayout(object sender, EventArgs args)
        {
            if (lvUserProfiles.Selection.Count == 0)
                return;

            if (new EditUserProfileDialog(EditUserProfileBehaviourEnum.Layout, GetSelectedUserProfiles()).ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        private void EditUserProfileStore(object sender, EventArgs args)
        {
            if (lvUserProfiles.Selection.Count == 0)
                return;

            if (new EditUserProfileDialog(EditUserProfileBehaviourEnum.Store, GetSelectedUserProfiles()).ShowDialog() == DialogResult.OK)
            {
                LoadItems();
            }
        }

        #region Events

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvUserProfiles.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Resources.EditCmd,
                    100,
                    btnsContextButtons_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtons.EditButtonEnabled,
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Add,
                    200,
                    btnsContextButtons_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsContextButtons.AddButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnsContextButtons_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsContextButtons.RemoveButtonEnabled,
            };

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 1000));

            item = new ExtendedMenuItem(
                    Resources.EditStore,
                    1010,
                    EditUserProfileStore)
            {
                Enabled = lvUserProfiles.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles)
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.EditVisualProfile,
                    1020,
                    EditUserProfileVisualProfile)
            {
                Enabled = lvUserProfiles.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles)
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.EditLayout,
                    1030,
                    EditUserProfileLayout)
            {
                Enabled = lvUserProfiles.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles)
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("UserProfilesList", lvUserProfiles.ContextMenuStrip, lvUserProfiles);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowUserProfileSheet(sender, selectedID);
        }

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            NewUserProfileDialog dlg = new NewUserProfileDialog();

            if(dlg.ShowDialog() == DialogResult.OK)
            {
                LoadItems();
                PluginOperations.ShowUserProfileSheet(dlg.ProfileID);
            }
        }

        private void btnsContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            List<UserProfile> eligibleProfilesToDelete = GetSelectedUserProfiles().Where(x => !x.ProfileIsUsed).ToList();

            if (eligibleProfilesToDelete.Count == 0)
            {
                MessageDialog.Show(Resources.UserProfilesInUse,
                    Resources.DeleteUserProfileCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult result = DialogResult.Cancel;

                if (lvUserProfiles.Selection.Count == eligibleProfilesToDelete.Count)
                {
                    result = QuestionDialog.Show(
                        eligibleProfilesToDelete.Count == 1
                            ? Resources.DeleteUserProfile
                            : Resources.DeleteUserProfiles, Resources.DeleteUserProfileCaption);
                }
                else
                {
                    result = QuestionDialog.Show(Properties.Resources.DeleteUserProfilesInUse,
                        Resources.DeleteUserProfileCaption);
                }

                if (result == DialogResult.Yes)
                {
                    foreach (UserProfile profile in eligibleProfilesToDelete)
                    {
                        Providers.UserProfileData.Delete(PluginEntry.DataModel, profile.ID);
                    }

                    LoadItems();
                }
            }
        }

        private void lvUserProfiles_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvUserProfiles.SortColumn == args.Column)
            {
                lvUserProfiles.SetSortColumn(args.Column, !lvUserProfiles.SortedAscending);
            }
            else
            {
                lvUserProfiles.SetSortColumn(args.Column, true);
            }

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void lvUserProfiles_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = (lvUserProfiles.Selection.Count == 1) ? ((UserProfile)lvUserProfiles.Selection[0].Tag).ID : RecordIdentifier.Empty;

            btnsContextButtons.EditButtonEnabled = lvUserProfiles.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles);
            btnsContextButtons.RemoveButtonEnabled = lvUserProfiles.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles); 

            if((lastSelectionCount == 0 && lvUserProfiles.Selection.Count > 0) || (lastSelectionCount != 0 && lvUserProfiles.Selection.Count == 0))
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }

            lastSelectionCount = lvUserProfiles.Selection.Count;
        }

        private void lvUserProfiles_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnsContextButtons_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.Profiles.Views.UserProfilesView.View")
            {
                bool enabled = lvUserProfiles.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles);
                arguments.Add(new ContextBarItem(Resources.EditStore, EditUserProfileStore) { Enabled = enabled }, 100);
                arguments.Add(new ContextBarItem(Resources.EditVisualProfile, EditUserProfileVisualProfile) { Enabled = enabled }, 200);
                arguments.Add(new ContextBarItem(Resources.EditLayout, EditUserProfileLayout) { Enabled = enabled }, 300);
            }
        }

        private void Layout_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.TouchLayoutData.GetList(PluginEntry.DataModel), null);
        }

        private void Store_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void VisualProfile_RequestData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.VisualProfileData.GetList(PluginEntry.DataModel), null);
        }

        #endregion Events

        #region SearchBar

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.VisualProfile, "VisualProfile", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Layout, "Layout", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.LanguageCode, "LanguageCode", ConditionType.ConditionTypeEnum.Text));
            searchBar_LoadDefault(this, EventArgs.Empty);
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

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "VisualProfile":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestClear += new EventHandler(DualDataComboBox_RequestClear);
                    ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(VisualProfile_RequestData);
                    break;

                case "Store":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestClear += new EventHandler(DualDataComboBox_RequestClear);
                    ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(Store_RequestData);
                    break;

                case "Layout":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).RequestClear += new EventHandler(DualDataComboBox_RequestClear);
                    ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(Layout_RequestData);
                    break;
            }
        }

        private void DualDataComboBox_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" 
                             && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "VisualProfile":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= VisualProfile_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;

                case "Store":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Store_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;

                case "Layout":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= Layout_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            switch (args.TypeKey)
            {
                case "VisualProfile":
                    entity = Providers.VisualProfileData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Store":
                    entity = Providers.StoreData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Layout":
                    entity = Providers.TouchLayoutData.Get(PluginEntry.DataModel, args.Selection);
                    break;
            }

            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        #endregion SearchBar
    }
}