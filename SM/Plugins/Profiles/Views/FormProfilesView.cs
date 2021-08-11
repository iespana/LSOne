using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Profiles.Properties;
using System.Linq;
using LSOne.DataLayer.DatabaseUtil.ScriptInformation;
using LSOne.DataLayer.DatabaseUtil;
using LSOne.Controls.Cells;

namespace LSOne.ViewPlugins.Profiles.Views
{
    public partial class FormProfilesView : ViewBase
    {
        private RecordIdentifier selectedID = "";
        private RecordIdentifier selectedLineID = "";
        private List<FormProfile> profiles;

        public FormProfilesView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID;

        }

        public FormProfilesView()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                Attributes = ViewAttributes.ContextBar |
                             ViewAttributes.Audit |
                             ViewAttributes.Help |
                             ViewAttributes.Close;

                lvFormProfiles.ContextMenuStrip = new ContextMenuStrip();
                lvFormProfiles.ContextMenuStrip.Opening += lvReceiptProfiles_Opening;

                lvProfileLines.ContextMenuStrip = new ContextMenuStrip();
                lvProfileLines.ContextMenuStrip.Opening += lvProfileLines_Opening;
                //lvFormProfiles.SmallImageList = PluginEntry.Framework.GetImageList();

                btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.FormProfileEdit);
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("FormProfiles", RecordIdentifier.Empty, Properties.Resources.FormProfiles, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.FormProfiles;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
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
            switch (objectName)
            {
                case "FormProfile":
                    if (changeHint == DataEntityChangeType.Add || changeHint == DataEntityChangeType.Edit || changeHint == DataEntityChangeType.Delete)
                    {
                        LoadData(false);
                    }
                    break;

            }
        }
        
        void lvReceiptProfiles_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvFormProfiles.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ReceiptList", lvFormProfiles.ContextMenuStrip, lvFormProfiles);

            e.Cancel = (menu.Items.Count == 0);
        }

        void lvProfileLines_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvProfileLines.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnsLineContextButtons_EditButtonClicked);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsLineContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnsLineContextButtons_AddButtonClicked);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsLineContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsLineContextButtons_RemoveButtonClicked);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsLineContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("ReceiptLineList", lvProfileLines.ContextMenuStrip, lvProfileLines);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            RecordIdentifier selectedProfileID = PluginOperations.EditFormProfile(selectedID);

            if (selectedProfileID != RecordIdentifier.Empty)
            {
                selectedID = selectedProfileID;
                LoadFormProfiles(lvFormProfiles.SortedAscending);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier selectedProfileID = PluginOperations.NewFormProfile();

            if (selectedProfileID != RecordIdentifier.Empty)
            {
                selectedID = selectedProfileID;
                LoadFormProfiles(lvFormProfiles.SortedAscending);
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (isRevert)
            {

            }

            HeaderText = Properties.Resources.FormProfiles;
            //HeaderIcon = Properties.Resources.Profiles16;

            LoadFormProfiles(true);
        }


        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteFormProfile(selectedID);
        }

        private void LoadFormProfiles(bool ascending)
        {
            var oldSelectedID = selectedID;
            int sortBy = lvFormProfiles.Columns.IndexOf(lvFormProfiles.SortColumn);
            int selectedRowIndex = -1;
            lvFormProfiles.ClearRows();
            profiles = Providers.FormProfileData.GetList(PluginEntry.DataModel, (FormProfileSorting)sortBy, !ascending);

            foreach (DataEntity profile in profiles)
            {
                Row row = new Row();
                row.AddText(profile.Text);
                row.AddCell(new CheckBoxCell(IsSystemProfile(profile.ID), false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.Tag = profile.ID;

                lvFormProfiles.AddRow(row);
                if (oldSelectedID == (RecordIdentifier)row.Tag)
                {
                    selectedRowIndex = lvFormProfiles.Rows.IndexOf(row);
                }
            }

            if (selectedRowIndex != -1)
            {
                lvFormProfiles.Selection.Set(0, selectedRowIndex);
                lvFormProfiles.CalculateLayout();
                lvFormProfiles.ScrollRowIntoView(selectedRowIndex);
            }
            lvFormProfiles.AutoSizeColumns();
            lvFormProfiles_SelectionChanged(null, EventArgs.Empty);
        }

        private void LoadProfileLines(bool ascending)
        {
            List<FormProfileLine> lines;
            var oldSelectedLineID = selectedLineID;
            int sortBy = lvProfileLines.Columns.IndexOf(lvProfileLines.SortColumn);
            int selectedRowIndex = -1;
            lvProfileLines.ClearRows();

            lines = Providers.FormProfileLineData.GetProfileLines(PluginEntry.DataModel, (RecordIdentifier)lvFormProfiles.Selection[0].Tag, (FormProfileLineSorting)sortBy, !ascending);

            foreach (var line in lines)
            {
                Row row = new Row();
                row.AddText(line.TypeDescription);
                row.AddText(line.Text);
                row.AddText(line.PrintAsSlip ? Properties.Resources.Yes : Properties.Resources.No);
                row.AddText(GetLocalizedPrintBehavior(line.PrintBehavior));
                row.AddText(line.NumberOfCopies.ToString());
                row.AddCell(new CheckBoxCell(line.IsSystemProfileLine, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));

                row.Tag = line;
                lvProfileLines.AddRow(row);

                if (oldSelectedLineID == line.ID)
                {
                    selectedRowIndex = lvProfileLines.Rows.IndexOf(row);
                }
            }

            if (selectedRowIndex != -1)
            {
                lvProfileLines.Selection.Set(0, selectedRowIndex);
                lvProfileLines.CalculateLayout();
                lvProfileLines.ScrollRowIntoView(selectedRowIndex);
            }
            lvProfileLines.AutoSizeColumns();
            lvProfileLines_SelectionChanged(null, EventArgs.Empty);
        }

        private static string GetLocalizedPrintBehavior(PrintBehaviors printBehaviors)
        {
            var result = "";
            switch (printBehaviors)
            {
                case PrintBehaviors.AlwaysPrint:
                    result = Properties.Resources.AlwaysPrint;
                    break;
                case PrintBehaviors.NeverPrint:
                    result = Properties.Resources.NeverPrint;
                    break;
                case PrintBehaviors.PromptUser:
                    result = Properties.Resources.PromptUser;
                    break;
                /*case PrintBehaviors.ShowPreview:
                    result = Properties.Resources.ShowPreview;
                    break;
                 */ 
            }
            return result;
        }

        protected override void OnClose()
        {
            
            base.OnClose();
        }

        private void lvFormProfiles_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = lvFormProfiles.Selection.Count > 0;
            lvProfileLines.Visible = btnsLineContextButtons.Visible = hasSelection;
            selectedID = hasSelection ? (RecordIdentifier)lvFormProfiles.Selection[0].Tag : "";
            bool isProfileInUse = selectedID == string.Empty ? false : profiles.Single(x => x.ID == selectedID).ProfileIsUsed;
            btnsContextButtons.EditButtonEnabled = hasSelection && PluginEntry.DataModel.HasPermission(Permission.FormProfileView) && !IsSystemProfile(selectedID);
            btnsContextButtons.RemoveButtonEnabled = hasSelection && PluginEntry.DataModel.HasPermission(Permission.FormProfileEdit) && !IsSystemProfile(selectedID) && !isProfileInUse;
            if (hasSelection)
            {
                LoadProfileLines(true);
                lblFormProfileHeader.Visible = true;
            }
            else
            {
                lblFormProfileHeader.Visible = false;
            }
        }

        private void lvFormProfiles_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (args.ColumnNumber == 1) //System profile
                return;

            lvFormProfiles.SetSortColumn(args.Column, lvFormProfiles.SortColumn != args.Column || !lvFormProfiles.SortedAscending);
            LoadFormProfiles(lvFormProfiles.SortedAscending);
        }

        private void lvProfileLines_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (args.ColumnNumber == 4 || args.ColumnNumber == 5) //System profile, Number of copies
                return;

            lvProfileLines.SetSortColumn(args.Column, lvProfileLines.SortColumn != args.Column || !lvProfileLines.SortedAscending);
            LoadProfileLines(lvProfileLines.SortedAscending);
        }

        private void btnsLineContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier newLineID = PluginOperations.CreateProfileLine(selectedID);

            if (newLineID != RecordIdentifier.Empty)
            {
                selectedLineID = newLineID;
                LoadProfileLines(lvProfileLines.SortedAscending);
            }
        }

        private void btnsLineContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier editedLineID = PluginOperations.EditProfileLine(selectedID, selectedLineID.SecondaryID);

            if (editedLineID != RecordIdentifier.Empty)
            {
                selectedLineID = editedLineID;
                LoadProfileLines(lvProfileLines.SortedAscending);
            }
        }

        private void btnsLineContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.DeleteFormProfileLine(selectedLineID.PrimaryID, selectedLineID.SecondaryID);
            LoadProfileLines(lvProfileLines.SortedAscending);
        }

        private void lvProfileLines_SelectionChanged(object sender, EventArgs e)
        {
            bool hasSelection = lvProfileLines.Selection.Count > 0;
            btnsLineContextButtons.EditButtonEnabled = hasSelection;
            btnsLineContextButtons.RemoveButtonEnabled = hasSelection && (!IsSystemProfile(selectedID) || !((FormProfileLine)lvProfileLines.Selection[0].Tag).IsSystemProfileLine);
            btnsLineContextButtons.AddButtonEnabled = true;

            if (hasSelection)
            {
                selectedLineID = new RecordIdentifier(selectedID, ((FormProfileLine)lvProfileLines.Selection[0].Tag).ID.SecondaryID);
            }
        }
        
        private void lvFormProfiles_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (lvFormProfiles.Selection.Count > 0 && btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(sender, EventArgs.Empty);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit))
                {
                    arguments.Add(new ContextBarItem(Resources.Add, ContextButtons.GetAddButtonImage(), btnAdd_Click), 10);
                }
            }
            else if (arguments.CategoryKey == GetType() + ".Related")
            {
                arguments.Add(new ContextBarItem(Resources.Related_ResetDefaultProfile, "ResetSystemFormProfile", true, ResetSystemFormProfile), 5000);
            }
        }

        private void lvProfileLines_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (lvProfileLines.Selection.Count > 0 && btnsLineContextButtons.EditButtonEnabled)
            {
                btnsLineContextButtons_EditButtonClicked(sender, EventArgs.Empty);
            }
        }

        private void ResetSystemFormProfile(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
               Resources.ResetDefaultProfileQuestion,
               Resources.ResetDefaultProfileHeader) == DialogResult.Yes)
            { 
                //DELETE FORM PROFILE LINES FOR DEFAULT PROFILE
                List<FormProfileLine> profileLines = Providers.FormProfileLineData.GetProfileLines(PluginEntry.DataModel, FormProfile.DefaultProfileID, FormProfileLineSorting.Description, false);
                profileLines.ForEach(x => Providers.FormProfileLineData.DeleteProfileLine(PluginEntry.DataModel, x.ID.PrimaryID, x.ID.SecondaryID));

                //DELETE FORM PROFILE LINES FOR EMAIL PROFILE
                List<FormProfileLine> emailProfileLines = Providers.FormProfileLineData.GetProfileLines(PluginEntry.DataModel, FormProfile.EmailProfileID, FormProfileLineSorting.Description, false);
                emailProfileLines.ForEach(x => Providers.FormProfileLineData.DeleteProfileLine(PluginEntry.DataModel, x.ID.PrimaryID, x.ID.SecondaryID));

                //DELETE SYSTEM FORM LAYOUTS
                List<DataLayer.BusinessObjects.Forms.Form> formLayouts = Providers.FormData.GetSystemProfileForms(PluginEntry.DataModel);
                formLayouts.ForEach(x => Providers.FormData.Delete(PluginEntry.DataModel, x.ID));

                List<ScriptInfo> scriptInfo = DatabaseUtility.GetSQLScriptInfo(DataLayer.DatabaseUtil.Enums.RunScripts.SystemData);
                foreach (ScriptInfo defaultProfileScript in scriptInfo.Where(w => w.ResourceName.Contains(ScriptInfo.DefaultFormProfileConst)).OrderBy(o => o.ResourceName))
                {
                    DatabaseUtility.RunSpecificScript(PluginEntry.DataModel, defaultProfileScript, null);
                }
                LoadData(false);
            }
        }

        private bool IsSystemProfile(RecordIdentifier profileID)
        {
            return profileID == FormProfile.DefaultProfileID || profileID == FormProfile.EmailProfileID;
        }
    }
}
