using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using Form = System.Windows.Forms.Form;
using LSOne.Controls;

namespace LSOne.ViewPlugins.Forms.Views
{
    public partial class FormTypesView : ViewBase
    {
        private RecordIdentifier selectedId = 0;

        public FormTypesView(RecordIdentifier selectedId)
            : this()
        {
            this.selectedId = selectedId.ToString();
        }

        public FormTypesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.Help |
                ViewAttributes.ContextBar;

            lvFormTypes.ContextMenuStrip = new ContextMenuStrip();
            lvFormTypes.ContextMenuStrip.Opening += new CancelEventHandler(lvForms_Opening);

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("FormTypes", RecordIdentifier.Empty, Properties.Resources.FormTypes, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.FormTypes;
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
            HeaderText = Properties.Resources.FormTypes;
            //HeaderIcon = Properties.Resources.FormsImage;
            LoadForms(true);
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

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id = PluginOperations.NewFormType();

            if (id != RecordIdentifier.Empty)
            {
                selectedId = id;
            }

            LoadForms(lvFormTypes.SortedAscending);
        }
        
        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.EditFormType(((FormType)lvFormTypes.Selection[0].Tag).ID);
            LoadForms(lvFormTypes.SortedAscending);
        }

        void lvForms_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvFormTypes.ContextMenuStrip;
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

            PluginEntry.Framework.ContextMenuNotify("FormsList", lvFormTypes.ContextMenuStrip, lvFormTypes);
            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvFormTypes.Selection.Count == 1)
            {
                var formsUsingType = Providers.FormData.
                    GetFormsOfType(PluginEntry.DataModel, ((FormType)lvFormTypes.Selection[0].Tag).ID, 
                    FormSorting.Type, false);
                if (formsUsingType.Count != 0)
                {
                    MessageDialog.Show(Properties.Resources.FormTypeInUseOn.Replace("#1", ((FormType)lvFormTypes.Selection[0].Tag).Text).Replace("#2", formsUsingType[0].Text));
                }
                else
                {
                    PluginOperations.DeleteFormType(((FormType) lvFormTypes.Selection[0].Tag).ID);
                }
            }
            else
            {
                List<RecordIdentifier> idsToRemove = new List<RecordIdentifier>();

                bool formTypeNotDeletable = false;
                for (int i = 0; i < lvFormTypes.Selection.Count; i++)
                {
                    var formsUsingType = Providers.FormData.GetFormsOfType(PluginEntry.DataModel, ((FormType)lvFormTypes.Selection[i].Tag).ID, FormSorting.Type, false);
                    if (formsUsingType.Count == 0)
                    {
                        idsToRemove.Add(((FormType)lvFormTypes.Selection[i].Tag).ID);
                    }
                    else
                    {
                        MessageDialog.Show(Properties.Resources.FormTypeInUseOn.Replace("#1", ((FormType)lvFormTypes.Selection[i].Tag).Text).Replace("#2", formsUsingType[i].Text));
                        formTypeNotDeletable = true;
                        break;
                    }
                }

                if (!formTypeNotDeletable)
                {
                    PluginOperations.DeleteFormTypes(idsToRemove);
                }
            }

            LoadForms(lvFormTypes.SortedAscending);
        }

        private void LoadForms(bool ascending)
        {
            var oldSelectedID = selectedId;
            int sortBy = lvFormTypes.Columns.IndexOf(lvFormTypes.SortColumn);
            int selectedRowIndex = -1;
            lvFormTypes.ClearRows();
            List<FormType> formTypes = Providers.FormTypeData.GetFormTypes(PluginEntry.DataModel, (FormTypeSorting)sortBy, !ascending);

            foreach (var formType in formTypes)
            {
                Row row = new Row();
                row.AddText(formType.Text);
                row.AddText(formType.SystemTypeText);
                row.Tag = formType;
                lvFormTypes.AddRow(row);

                if (oldSelectedID == ((FormType)row.Tag).ID)
                {
                    selectedRowIndex = lvFormTypes.Rows.IndexOf(row);
                }
            }

            if (selectedRowIndex != -1)
            {
                lvFormTypes.Selection.Set(0, selectedRowIndex);
                lvFormTypes.CalculateLayout();
                lvFormTypes.ScrollRowIntoView(selectedRowIndex);
            }

            lvFormTypes.AutoSizeColumns();
            lvForms_SelectionChanged(this, EventArgs.Empty);
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".Related")
            {
                PluginEntry.Framework.FindImplementor(this, "CanInsertDefaultData", arguments);
            }
        }

        private void lvForms_SelectionChanged(object sender, EventArgs e)
        {
            selectedId = (lvFormTypes.Selection.Count > 0) ? ((FormType)lvFormTypes.Selection[0].Tag).ID : RecordIdentifier.Empty;
            var formType = Providers.FormTypeData.Get(PluginEntry.DataModel, selectedId);
            btnsContextButtons.EditButtonEnabled = (lvFormTypes.Selection.Count == 1) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit)
                                                    && formType.SystemType == 0;
            btnsContextButtons.RemoveButtonEnabled = (lvFormTypes.Selection.Count != 0) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit);
        }

        private void lvForms_HeaderClicked(object sender, ColumnEventArgs args)
        {
            lvFormTypes.SetSortColumn(args.Column, lvFormTypes.SortColumn != args.Column || !lvFormTypes.SortedAscending);

            LoadForms(lvFormTypes.SortedAscending);
        }

        private void lvFormTypes_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (lvFormTypes.Selection.Count > 0 && btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }
    }
}
