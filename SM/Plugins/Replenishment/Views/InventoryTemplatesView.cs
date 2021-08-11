using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.Replenishment.Dialogs;
using LSOne.ViewPlugins.Replenishment.Properties;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.DataEntities;

namespace LSOne.ViewPlugins.Replenishment.Views
{
    public partial class InventoryTemplatesView : ViewBase
    {

        RecordIdentifier selectedTemplateId;
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("2EA17C52-245A-4DAF-BA64-DF71FD2A44F4");

        public InventoryTemplatesView(RecordIdentifier selectedTemplateId)
            :this()
        {
            this.selectedTemplateId = selectedTemplateId;
        }

        public InventoryTemplatesView()
        {
            InitializeComponent();
            
            //TODO Auditing
            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Save |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.InventoryTemplates;            

            lvTemplates.ContextMenuStrip = new ContextMenuStrip();
            lvTemplates.ContextMenuStrip.Opening += lvTemplates_Opening;

            searchBar.BuddyControl = lvTemplates;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (!DesignMode)
            {
                if (lvTemplates != null)
                {
                    lvTemplates.AutoSizeColumns(true);
                }
            }
        }

        private void lvTemplates_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvTemplates.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                btnsEditAddRemove_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Add,                
                200,
                btnsEditAddRemove_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsEditAddRemove.AddButtonEnabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                300,
                btnsEditAddRemove_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemove.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InventoryTemplatesList", lvTemplates.ContextMenuStrip, lvTemplates);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.InventoryTemplates;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "InventoryTemplate")
            {
               
                selectedTemplateId = changeAction == DataEntityChangeType.Delete ? RecordIdentifier.Empty : changeIdentifier;
                LoadInventoryTemplates();
            }
        }

        protected override void LoadData(bool isRevert)
        {
            LoadInventoryTemplates();
        }

        private void LoadInventoryTemplates()
        {
            List<InventoryTemplateListItem> templates;

            InventoryTemplateListFilter filter = new InventoryTemplateListFilter();

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Description":
                        filter.Description = result.StringValue;
                        filter.DescriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Type":
                        filter.EntryType = TemplateEntryTypeEnumHelper.ToEnum((string)result.ComboSelectedItem);
                        break;
                }
            }

            try
            {
                if (PluginEntry.DataModel.IsHeadOffice)
                {
                    templates = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), filter, true);
                }
                else
                {
                    templates = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateListForStore(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), filter, PluginEntry.DataModel.CurrentStoreID, false);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                return;
            }
            lvTemplates.ClearRows();

            foreach (InventoryTemplateListItem template in templates)
            {
                string storeDescription;

                if (template.AllStores)
                {
                    storeDescription = Properties.Resources.AllStores;
                }
                else if (template.StoreCount > 1)
                {
                    storeDescription = Properties.Resources.MultipleStores;
                }
                else
                {
                    storeDescription = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel).GetInventoryTemplateFirstStoreName(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.ID, true);
                }

                var row = new Row();
                row.AddText(template.Text);
                row.AddText(storeDescription);
                row.AddText(TemplateEntryTypeEnumHelper.ToString(template.Type));

                row.Tag = template.ID;
                lvTemplates.AddRow(row);

                if (template.ID == selectedTemplateId)
                {
                    lvTemplates.Selection.Set(lvTemplates.RowCount - 1);
                }
            }
            
            lvTemplates.AutoSizeColumns(true);
            lvTemplates_SelectionChanged(this, EventArgs.Empty);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new NewInventoryTemplateDialog();

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                selectedTemplateId = dlg.InventoryTemplateID;
                LoadInventoryTemplates();

                PluginEntry.Framework.ViewController.Add(new InventoryTemplateView(dlg.InventoryTemplateID));
            }
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            if (lvTemplates.Selection.Count == 1)
            {
                PluginEntry.Framework.ViewController.Add(new InventoryTemplateView((RecordIdentifier) lvTemplates.Selection[0].Tag));
            }
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvTemplates.Selection.Count == 1)
            {
                PluginOperations.DeleteInventoryTemplate(selectedTemplateId);
            }
            else if (lvTemplates.Selection.Count > 1)
            {
                var selectedIDs = new List<RecordIdentifier>();

                for (int i = 0; i < lvTemplates.Selection.Count; i++)
                {
                    selectedIDs.Add((RecordIdentifier)lvTemplates.Selection[i].Tag);
                }

                PluginOperations.DeleteInventoryTemplates(selectedIDs);
            }

            LoadInventoryTemplates();
        }

        private void lvTemplates_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (args.Row != null)
            {
                PluginEntry.Framework.ViewController.Add(new InventoryTemplateView((RecordIdentifier)args.Row.Tag));
            }
        }

        private void lvTemplates_SelectionChanged(object sender, EventArgs e)
        {
            btnsEditAddRemove.EditButtonEnabled = lvTemplates.Selection.Count == 1;
            btnsEditAddRemove.RemoveButtonEnabled = lvTemplates.Selection.Count > 0;

            if (lvTemplates.Selection.Count == 1)
            {
                selectedTemplateId = (RecordIdentifier)lvTemplates.Row(lvTemplates.Selection.FirstSelectedRow).Tag;
            }
            else if (lvTemplates.Selection.Count > 1)
            {
                selectedTemplateId = null;
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

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadInventoryTemplates();
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            LoadInventoryTemplates();
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

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> types = new List<object>
            {
                TemplateEntryTypeEnumHelper.ToString(TemplateEntryTypeEnum.PurchaseOrder),
                TemplateEntryTypeEnumHelper.ToString(TemplateEntryTypeEnum.StockCounting),
                TemplateEntryTypeEnumHelper.ToString(TemplateEntryTypeEnum.TransferStock)
            };

            searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Type, "Type", ConditionType.ConditionTypeEnum.ComboBox, types, 0, 0, false));
            searchBar_LoadDefault(this, EventArgs.Empty);
        }
    }
}
