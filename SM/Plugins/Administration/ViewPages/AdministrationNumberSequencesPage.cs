using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Administration.Properties;
using ContainerControl = LSOne.Controls.ContainerControl;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    public partial class AdministrationNumberSequencesPage : ContainerControl, ITabViewV2
    {
        RecordIdentifier selectedSequenceID = "";
        private List<Row> displayedRows;
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("29CB176C-B510-466D-81D0-D416619A1899");

        public AdministrationNumberSequencesPage()
        {
            InitializeComponent();

            selectedSequenceID = "";
            displayedRows = new List<Row>();

            lvNumberSequences.ContextMenuStrip = new ContextMenuStrip();
            lvNumberSequences.ContextMenuStrip.Opening += lvNumberSequences_Opening;

            btnsEditAddDelete.EditButtonEnabled = false;
            btnsEditAddDelete.RemoveButtonEnabled = false;
            btnsEditAddDelete.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageNumberSequences);

            searchBar.BuddyControl = lvNumberSequences;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new AdministrationNumberSequencesPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            searchBar.FocusFirstInput();
            lvNumberSequences.VerticalScrollbarValue = 0;
            LoadItems();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("AdministrationNumberSequencesPage", 1, Properties.Resources.NumberSequences, true));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "NumberSequence":
                    if (changeHint == DataEntityChangeType.Add || changeHint == DataEntityChangeType.Edit)
                    {
                        selectedSequenceID = changeIdentifier;
                    }
                    LoadItems();
                    break;
            }
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            LoadItems();
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }
        private void LoadItems()
        {
            List<NumberSequence> numberSequences = Providers.NumberSequenceData.Get(PluginEntry.DataModel, NumberSequenceSorting.ID, false);

            CheckBoxCell cellEmbedStoreID;
            CheckBoxCell cellEmbedTerminalID;
            displayedRows.Clear();
            Row row;
            //int selectedSequenceIndex = 0;
            foreach (NumberSequence sequence in numberSequences)
            {
                row = new Row();

                cellEmbedStoreID = new CheckBoxCell();
                cellEmbedStoreID.CheckBoxAlignment = CheckBoxCell.CheckBoxAlignmentEnum.Center;
                cellEmbedStoreID.CheckState = sequence.EmbedStoreID ? CheckState.Checked : CheckState.Unchecked;
                cellEmbedStoreID.Enabled = false;

                cellEmbedTerminalID = new CheckBoxCell();
                cellEmbedTerminalID.CheckBoxAlignment = CheckBoxCell.CheckBoxAlignmentEnum.Center;
                cellEmbedTerminalID.CheckState = sequence.EmbedTerminalID ? CheckState.Checked : CheckState.Unchecked;
                cellEmbedTerminalID.Enabled = false;


                row.AddCell(new ExtendedCell((string)sequence.ID, sequence.CanBeDeleted ? null : PluginEntry.Framework.GetImage((ImageEnum.EmbeddedLock))));
                row.AddText(sequence.Text);
                row.AddCell(new NumericCell(sequence.Highest.ToString(), sequence.Highest));
                row.AddCell(new NumericCell(sequence.NextRecord.ToString(),sequence.NextRecord));
                row.AddText(sequence.Format);
                row.AddCell(cellEmbedStoreID);
                row.AddCell(cellEmbedTerminalID);

                row.Tag = sequence;

                displayedRows.Add(row);
            }
            searchBar_SearchClicked(null, EventArgs.Empty);
        }

        void lvNumberSequences_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvNumberSequences.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddDelete_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddDelete.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Add,
                200,
                btnsEditAddDelete_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsEditAddDelete.AddButtonEnabled,
                Default = false
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                300,
                btnsEditAddDelete_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddDelete.RemoveButtonEnabled,
                Default = false
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("NumberSequenceList", lvNumberSequences.ContextMenuStrip, lvNumberSequences);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsEditAddDelete_EditButtonClicked(object sender, EventArgs e)
        {
            var id = ((NumberSequence) lvNumberSequences.Selection[0].Tag).ID;
            var dlg = new Dialogs.EditNumberSequenceDialog(id);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                selectedSequenceID = id;
                LoadItems();
            }
        }

        private void btnsEditAddDelete_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewNumberSequence();
        }

        private void btnsEditAddDelete_RemoveButtonClicked(object sender, EventArgs e)
        {
            bool delete = PluginOperations.DeleteNumberSequence(selectedSequenceID);
            if (delete)
            {
                LoadItems();
            }
        }

        private void lvNumberSequences_SelectionChanged(object sender, EventArgs e)
        {
            btnsEditAddDelete.EditButtonEnabled = lvNumberSequences.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(Permission.ManageNumberSequences);
            btnsEditAddDelete.RemoveButtonEnabled = (lvNumberSequences.Selection.Count > 0) && 
                                                    ((NumberSequence)lvNumberSequences.Selection[0].Tag).CanBeDeleted &&
                                                    PluginEntry.DataModel.HasPermission(Permission.ManageNumberSequences);

            selectedSequenceID = lvNumberSequences.Selection.Count > 0 ? ((NumberSequence)lvNumberSequences.Rows[lvNumberSequences.Selection.FirstSelectedRow].Tag).ID : "";
        }

        private void lvNumberSequences_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddDelete.EditButtonEnabled)
            {
                btnsEditAddDelete_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            Filter();
        }

        private void Filter()
        {
            RecordIdentifier selectedSequence = selectedSequenceID;
            lvNumberSequences.ClearRows();
            int selectedSequenceIndex = 0;
            bool addItem;
            bool searchConditionMatch;
            List <SearchParameterResult> result = searchBar.SearchParameterResults;
            string searchCondition = result.Count > 0 ? result[0].StringValue : "";
            SearchParameterResult.SearchModificationEnum searchModification = result.Count > 0 ? result[0].SearchModification : SearchParameterResult.SearchModificationEnum.BeginsWith;
            foreach (var row in displayedRows)
            {
                addItem = true;
                if (searchCondition != "")
                {
                    searchConditionMatch = false;
                    searchModification = result[0].SearchModification;
                    for (uint i = 0; i <= lvNumberSequences.Columns.Count - 1; i++)
                    {
                        if (searchModification == SearchParameterResult.SearchModificationEnum.BeginsWith)
                        {
                            if ((string.Compare(row[i].Text.Left(searchCondition.Length), searchCondition, CultureInfo.CurrentCulture, CompareOptions.IgnoreCase)) == 0)
                            {
                                searchConditionMatch = true;
                                break;
                            }
                        }
                        else
                        {
                            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(row[i].Text, searchCondition, CompareOptions.IgnoreCase) >= 0)
                            {
                                searchConditionMatch = true;
                                break;
                            }
                        }
                    }
                    if (!searchConditionMatch)
                    {
                        addItem = false;
                    }
                }

                if (addItem)
                {
                    lvNumberSequences.AddRow(row);

                    if (selectedSequence == ((NumberSequence)row.Tag).ID)
                    {
                        lvNumberSequences.Selection.Set(selectedSequenceIndex);
                    }
                    selectedSequenceIndex++;
                }
            }
            lvNumberSequences.ScrollRowIntoView(lvNumberSequences.Selection.FirstSelectedRow);
            lvNumberSequences.AutoSizeColumns();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.IDOrDescription, "ID or description", ConditionType.ConditionTypeEnum.Text));

            searchBar_LoadDefault(this, EventArgs.Empty);
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
    }
}
