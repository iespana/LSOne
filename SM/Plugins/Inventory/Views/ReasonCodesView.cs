using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.Controls.Rows;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.ViewCore.EventArguments;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.DataProviders.Inventory;
using System.Globalization;
using LSOne.Controls.Cells;

namespace LSOne.ViewPlugins.Inventory.Views
{
    public partial class ReasonCodesView : ViewBase
    {
        private static Guid BarSettingID = new Guid("0D5387A9-7CE1-4597-8E98-3DB20C43438A");

        private RecordIdentifier selectedID = "";
        private List<ReasonCode> reasonCodes;
        private Setting searchBarSetting;

        public ReasonCodesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.ReasonCodes;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory);

            searchBar1.BuddyControl = lvReasons;

            lvReasons.ContextMenuStrip = new ContextMenuStrip();
            lvReasons.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            lvReasons.SetSortColumn(0, true);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory);
        }

        public ReasonCodesView(RecordIdentifier reasonCodeID)
            :this()
        {
            selectedID = reasonCodeID;
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("INVENTTRANSREASON", RecordIdentifier.Empty, Properties.Resources.ReasonCodes, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.ReasonCodes;
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
            LoadItems();
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "ReasonCode")
            {
                LoadItems();
            }
        }

        private void LoadItems()
        {
            string reasonDescription = null;
            bool descriptionBeginsWith = false;
            bool? isSystemCode = null;
            DateTime? beginDate = null;
            DateTime? endDate = null;
            ReasonActionEnum? action = null;

            RecordIdentifier currentlySelectedID = selectedID;
            lvReasons.ClearRows();
            selectedID = currentlySelectedID;

            ReasonCodeSorting sortBy = GetSort();

            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            foreach(SearchParameterResult result in results)
            {
                switch(result.ParameterKey)
                {
                    case "Description":
                        reasonDescription = result.StringValue;
                        descriptionBeginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Action":
                        switch (result.ComboSelectedIndex)
                        {
                            case 0:
                                action = null;
                                break;
                            case 1:
                                action = ReasonActionEnum.MainInventory;
                                break;
                            case 2:
                                action = ReasonActionEnum.ParkedInventory;
                                break;
                            case 3:
                                action = ReasonActionEnum.StockReservation;
                                break;
                            case 4:
                                action = ReasonActionEnum.Adjustment;
                                break;
                        }
                        break;
                    case "Date":
                        if(result.Date.Checked)
                        {
                            beginDate = result.Date.Value;
                        }

                        if(result.DateTo.Checked)
                        {
                            endDate = result.DateTo.Value;
                        }
                        break;
                    case "SystemReasonCode":
                        isSystemCode = result.CheckedValues[0];
                        break;
                }
            }

            try
            {
                IInventoryService service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

                reasonCodes = service.SearchReasonList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), 
                                                        reasonDescription, 
                                                        descriptionBeginsWith, 
                                                        action, 
                                                        beginDate, 
                                                        endDate, 
                                                        isSystemCode, 
                                                        sortBy, 
                                                        !lvReasons.SortedAscending, 
                                                        true);

                Row row;
                foreach (ReasonCode reason in reasonCodes)
                {
                    row = new Row();
                    row.AddText(reason.Text);
                    row.AddText(ReasonActionHelper.ReasonActionEnumToString(reason.Action));
                    row.AddText(reason.BeginDate.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern));
                    row.AddText(reason.EndDate.HasValue ? reason.EndDate.Value.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) : "");
                    row.AddCell(new CheckBoxCell(reason.IsSystemReasonCode, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                    row.AddCell(new CheckBoxCell(reason.ShowOnPos, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                    row.Tag = reason;

                    lvReasons.AddRow(row);

                    if (reason.ID == selectedID)
                    {
                        lvReasons.Selection.Set(lvReasons.RowCount - 1);
                    }
                }

                lvReasons.AutoSizeColumns();
                lvReasons_SelectionChanged(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private ReasonCodeSorting GetSort()
        {
            if (lvReasons.SortColumn == null)
            {
                lvReasons.SetSortColumn(0, true);
            }

            int sortColumnIndex = lvReasons.Columns.IndexOf(lvReasons.SortColumn);

            switch(sortColumnIndex)
            {
                case 0: return ReasonCodeSorting.ReasonText;
                case 1: return ReasonCodeSorting.Action;
                case 2: return ReasonCodeSorting.BeginDate;
                case 3: return ReasonCodeSorting.EndDate;
                case 4: return ReasonCodeSorting.SystemReason;
                case 5: return ReasonCodeSorting.ShowOnPos;
                default: return ReasonCodeSorting.ReasonText;
            }
        }

        private void lvReasons_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = lvReasons.Selection.Count == 1 ? ((ReasonCode)lvReasons.Selection[0].Tag).ID : "";

            bool canEdit = lvReasons.Selection.Count > 0 && 
                            PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory) && 
                            (GetSelectedReasons().Any(x => !x.IsSystemReasonCode) || lvReasons.Selection.Count == 1);
            bool canDelete = lvReasons.Selection.Count > 0 && 
                            PluginEntry.DataModel.HasPermission(Permission.ManageParkedInventory) && 
                            GetSelectedReasons().Any(x => !x.IsSystemReasonCode);
            btnsEditAddRemove.EditButtonEnabled = canEdit;
            btnsEditAddRemove.RemoveButtonEnabled = canDelete;
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            List<ReasonCode> selectedReasons = GetSelectedReasons();

            if(selectedReasons.Count > 1 && selectedReasons.Any(x => x.IsSystemReasonCode))
            {
                if (QuestionDialog.Show(Properties.Resources.EditSystemCodeQuestion + Environment.NewLine + Properties.Resources.DoYouWantToContinue) == DialogResult.No)
                {
                    return;
                }
            }

            if(selectedReasons.Count == 1)
            {
                PluginOperations.EditReasonCode(selectedReasons[0].ID);
            }
            else
            {
                PluginOperations.EditReasonCodes(selectedReasons.Select(x => x.ID).ToList());
            }
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewReasonCode();
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            List<ReasonCode> selectedReasons = GetSelectedReasons();

            if (selectedReasons.Any(x => x.IsSystemReasonCode))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteSystemCodeQuestion + Environment.NewLine + Properties.Resources.DoYouWantToContinue) == DialogResult.No)
                    return;

                selectedReasons.RemoveAll(x => x.IsSystemReasonCode);
            }

            if (selectedReasons.Count == 1)
            {
                PluginOperations.DeleteReasonCode(selectedReasons[0].ID);
            }
            else
            {
                PluginOperations.DeleteReasonCodes(selectedReasons.Select(x => x.ID).ToList());
            }
        }

        private List<ReasonCode> GetSelectedReasons()
        {
            if (lvReasons.Selection.Count == 0)
            {
                return new List<ReasonCode>();
            }

            List<ReasonCode> selection = new List<ReasonCode>();
            for (int i = 0; i < lvReasons.Selection.Count; i++)
            {
                ReasonCode line = (ReasonCode)lvReasons.Selection[i].Tag;
                selection.Add(line);
            }

            return selection;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvReasons.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvReasons_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvReasons_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvReasons.SortColumn == args.Column)
            {
                lvReasons.SetSortColumn(args.Column, !lvReasons.SortedAscending);
            }
            else
            {
                lvReasons.SetSortColumn(args.Column, true);
            }

            LoadItems();
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusTypes = new List<object>
            {
                Properties.Resources.All, Properties.Resources.ReasonActionMainInventory, Properties.Resources.ReasonActionParkedInventory, Properties.Resources.ReasonActionStockReservation, Properties.Resources.ReasonActionAdjustment
            };

            searchBar1.AddCondition(new ConditionType(Properties.Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Properties.Resources.Action, "Action", ConditionType.ConditionTypeEnum.ComboBox, statusTypes, 0, 0, false));
            searchBar1.AddCondition(new ConditionType(Properties.Resources.Active, "Date", ConditionType.ConditionTypeEnum.DateRange, true, DateTime.Now.Date, true, DateTime.Now.Date));
            searchBar1.AddCondition(new ConditionType(Properties.Resources.SystemReasonCode, "SystemReasonCode", ConditionType.ConditionTypeEnum.Checkboxes, Properties.Resources.Yes, false));

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar1.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }
        }

        private void searchBar1_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void searchBar1_SearchOptionChanged(object sender, EventArgs e)
        {
            LoadItems();
        }
    }
}
