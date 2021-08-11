using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Forms.Properties;
using Form = LSOne.DataLayer.BusinessObjects.Forms.Form;
using LSOne.Controls.Cells;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Controls;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;

namespace LSOne.ViewPlugins.Forms.Views
{
    public partial class FormsView : ViewBase
    {
        private static Guid BarSettingID = new Guid("5BC730D1-C9B9-4056-B061-B79795067A6F");

        private Setting searchBarSetting;
        private RecordIdentifier selectedID = "";
        private int indexToSelect = -1;

        public FormsView(RecordIdentifier selectedId)
            : this()
        {
            this.selectedID = selectedId.ToString();
        }

        public FormsView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.Help |
                ViewAttributes.ContextBar;

            lvForms.ContextMenuStrip = new ContextMenuStrip();
            lvForms.ContextMenuStrip.Opening += new CancelEventHandler(lvForms_Opening);

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit);

            searchBar.BuddyControl = lvForms;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("Forms", RecordIdentifier.Empty, Properties.Resources.Forms, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Forms;
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
            HeaderText = Properties.Resources.Forms;
            //HeaderIcon = Properties.Resources.FormsImage;
            LoadForms();
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
                case "DefaultData":
                case "Form":

                    if(changeHint == DataEntityChangeType.Delete || changeHint == DataEntityChangeType.MultiDelete)
                    {
                        SetIndexOnDelete();
                    }

                    LoadForms();
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id = PluginOperations.NewForm();

            if (id != RecordIdentifier.Empty)
            {
                selectedID = id;
            }
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowFormSheet(((Form)lvForms.Selection[0].Tag).ID);
        }
        
        void lvForms_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvForms.ContextMenuStrip;
            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
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
            menu.Items.Add(new ExtendedMenuItem("-", 2000));

            item = new ExtendedMenuItem(Resources.Import, 2010, btnImport_Click);
            item.Enabled = btnImport.Enabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(Resources.Export, 2020, btnExport_Click);
            item.Enabled = btnExport.Enabled;
            menu.Items.Add(item);


            PluginEntry.Framework.ContextMenuNotify("FormsList", lvForms.ContextMenuStrip, lvForms);
            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvForms.Selection.Count == 1)
            {
                Form selectedForm = (Form)lvForms.Selection[0].Tag;
                List<FormProfileLine> profileLinesUsingForm = Providers.FormProfileLineData.GetLinesByFormLayoutId(PluginEntry.DataModel, selectedForm.ID);

                if (selectedForm.IsSystemLayout)
                {
                    MessageDialog.Show(Resources.SystemLayoutCannotBeDeleted);
                }
                else if (profileLinesUsingForm.Count != 0)
                {
                    FormProfile profile = Providers.FormProfileData.Get(PluginEntry.DataModel, profileLinesUsingForm[0].ProfileID);
                    MessageDialog.Show(Properties.Resources.FormUsedInProfile.Replace("#1", Providers.FormData.Get(PluginEntry.DataModel, selectedForm.ID).Text).Replace("#2", profile.Text));
                }
                else
                {
                    PluginOperations.DeleteForm(selectedForm.ID);
                }
            }
            else
            {
                List<RecordIdentifier> idsToRemove = new List<RecordIdentifier>();

                bool formNotDeletable = false;
                for (int i = 0; i < lvForms.Selection.Count; i++)
                {
                    Form selectedForm = (Form)lvForms.Selection[i].Tag;
                    List<FormProfileLine> profileLinesUsingForm = Providers.FormProfileLineData.GetLinesByFormLayoutId(PluginEntry.DataModel, selectedForm.ID);

                    if(selectedForm.IsSystemLayout)
                    {
                        formNotDeletable = true;
                        MessageDialog.Show(Resources.SystemLayoutCannotBeDeleted);
                        break;
                    }
                    else if (profileLinesUsingForm.Count == 0)
                    {
                        idsToRemove.Add(((Form)lvForms.Selection[i].Tag).ID);
                    }
                    else
                    {
                        formNotDeletable = true;
                        FormProfile profile = Providers.FormProfileData.Get(PluginEntry.DataModel, profileLinesUsingForm[0].ProfileID);
                        MessageDialog.Show(Properties.Resources.FormUsedInProfile.Replace("#1", Providers.FormData.Get(PluginEntry.DataModel, selectedForm.ID).Text).Replace("#2", profile.Text));
                        break;
                    }
                }

                if (!formNotDeletable)
                {
                    PluginOperations.DeleteForms(idsToRemove);
                }
            }
        }

        private void LoadForms()
        {
            RecordIdentifier oldSelectedID = selectedID;
            FormSorting sortBy = (FormSorting)lvForms.Columns.IndexOf(lvForms.SortColumn);

            string description = "";
            bool beginsWith = true;
            bool? isSystemLayout = null;

            int selectedRowIndex = -1;

            lvForms.ClearRows();

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        description = result.StringValue;
                        beginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "System":
                        isSystemLayout = result.CheckedValues[0];
                        break;
                }
            }

            List<Form> forms = Providers.FormData.SearchForms(PluginEntry.DataModel, description, beginsWith, isSystemLayout, sortBy, !lvForms.SortedAscending);

            foreach (Form form in forms)
            {
                Row row = new Row();
                row.AddText(form.SystemType != FormSystemType.UserDefinedType ? form.FormsText : Providers.FormTypeData.Get(PluginEntry.DataModel, form.FormTypeID).Text);
                row.AddText(form.Text);
                row.AddCell(new CheckBoxCell(form.IsSystemLayout, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.Tag = form;
                lvForms.AddRow(row);

                if (oldSelectedID == form.ID)
                {
                    selectedRowIndex = lvForms.Rows.IndexOf(row);
                }
            }

            if(indexToSelect != -1)
            {
                lvForms.Selection.Set(0, indexToSelect);
                lvForms.CalculateLayout();
                lvForms.ScrollRowIntoView(indexToSelect);
            }
            else if (selectedRowIndex != -1)
            {
                lvForms.Selection.Set(0, selectedRowIndex);
                lvForms.CalculateLayout();
                lvForms.ScrollRowIntoView(selectedRowIndex);
            }

            lvForms.AutoSizeColumns();
            lvForms_SelectionChanged(this, EventArgs.Empty);
            indexToSelect = -1;
        }

        private void SetIndexOnDelete()
        {
            if (lvForms.Selection.Count > 0)
            {
                indexToSelect = lvForms.Rows.IndexOf(lvForms.Selection[0]);

                if (indexToSelect > 0)
                    indexToSelect--;
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".Related")
            {
                PluginEntry.Framework.FindImplementor(this, "CanInsertDefaultData", arguments);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            PluginOperations.ImportLayouts();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            List<RecordIdentifier> formIds = new List<RecordIdentifier>();
            bool selectedFormNotExported = false;
            for (int i = 0; i < lvForms.Selection.Count; i++)
            {
                Form current = Providers.FormData.Get(PluginEntry.DataModel, ((Form)lvForms.Selection[i].Tag).ID);
                if (current.SystemType != FormSystemType.UserDefinedType)
                {
                    formIds.Add(((Form) lvForms.Selection[i].Tag).ID);
                }
                else
                {
                    selectedFormNotExported = true;
                }
            }

            PluginOperations.ExportLayouts(formIds, selectedFormNotExported);
        }

        private void lvForms_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = (lvForms.Selection.Count > 0) ? ((Form)lvForms.Selection[0].Tag).ID : new RecordIdentifier("");
            btnsContextButtons.EditButtonEnabled = (lvForms.Selection.Count == 1) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit);
            btnsContextButtons.RemoveButtonEnabled = (lvForms.Selection.Count != 0) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit);
            btnExport.Enabled = (lvForms.Selection.Count != 0);
        }

        private void lvForms_HeaderClicked(object sender, ColumnEventArgs args)
        {
            lvForms.SetSortColumn(args.Column, lvForms.SortColumn != args.Column || !lvForms.SortedAscending);
            LoadForms();
        }

        private void lvForms_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (lvForms.Selection.Count > 0 && btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
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
            LoadForms();
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            LoadForms();
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.SystemLayout, "System", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Yes, false));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }
    }
}
