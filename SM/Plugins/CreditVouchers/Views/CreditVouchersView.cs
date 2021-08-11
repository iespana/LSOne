using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.GenericConnector;
using LSOne.ViewPlugins.CreditVouchers.Properties;
using LSOne.DataLayer.GenericConnector.DataEntities;
using System.Drawing;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;

namespace LSOne.ViewPlugins.CreditVouchers.Views
{
	public partial class CreditVouchersView : ViewBase
	{
        private static Guid BarSettingID = new Guid("acd57cc0-f3a7-4596-a9d2-7d362c767800");

        List<CreditVoucher> creditVouchers;

        RecordIdentifier selectedID = "";

	    private SiteServiceProfile siteServiceProfile;
        private Setting searchBarSetting;

        public CreditVouchersView()
		{
			InitializeComponent();

			Attributes = 
				ViewAttributes.Help |
				ViewAttributes.Close |
				ViewAttributes.ContextBar;

            //HeaderIcon = Properties.Resources.GiftCard16;
            HeaderText = Resources.CreditMemos;

            lvCreditVouchers.ContextMenuStrip = new ContextMenuStrip();
            lvCreditVouchers.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            lvCreditVouchers.Columns[0].Tag = CreditVoucher.SortEnum.ID;
            lvCreditVouchers.Columns[1].Tag = CreditVoucher.SortEnum.Balance;
            lvCreditVouchers.Columns[2].Tag = CreditVoucher.SortEnum.Currency;
            lvCreditVouchers.Columns[3].Tag = CreditVoucher.SortEnum.CreatedDate;
            lvCreditVouchers.Columns[4].Tag = CreditVoucher.SortEnum.LastUsedDate;

            lvCreditVouchers.SetSortColumn(0, true);

            searchBar.BuddyControl = lvCreditVouchers;

            dataScroll.PageSize = PluginEntry.DataModel.PageSize;
            dataScroll.Reset();

            searchBar.FocusFirstInput();

            var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
        }

		protected override string LogicalContextName
		{
			get
			{
				return Resources.CreditMemos;
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
            switch (objectName)
            {
                case "CreditVoucher":
                    ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    break;
            }
		}

        private void LoadItems()
        {
            CreditVoucherFilter filter = new CreditVoucherFilter();

            RecordIdentifier currentlySelectedID = selectedID; 
            lvCreditVouchers.ClearRows();
            selectedID = currentlySelectedID;

            if (lvCreditVouchers.SortColumn == null)
            {
                lvCreditVouchers.SetSortColumn(lvCreditVouchers.Columns[0], true);
            }

            filter.Sort = (CreditVoucher.SortEnum)lvCreditVouchers.SortColumn.Tag;
            filter.SortAscending = lvCreditVouchers.SortedAscending;
            filter.RowFrom = dataScroll.StartRecord;
            filter.RowTo = dataScroll.EndRecord + 1;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;
            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Credit memo ID":
                        filter.VoucherID = result.StringValue;
                        filter.VoucherIDBeginsWith = result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith;
                        break;
                    case "Status":
                        switch (result.ComboSelectedIndex)
                        {
                            case 0:
                                filter.Status = CreditVoucherStatusEnum.NotEmpty;
                                break;
                            case 1:
                                filter.Status = CreditVoucherStatusEnum.Empty;
                                break;
                            case 2:
                                filter.Status = CreditVoucherStatusEnum.All;
                                break;
                        }
                        break;
                    case "Currency":
                        filter.CurrencyID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "CreatedDate":
                        filter.FromCreatedDate = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        filter.ToCreatedDate = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                    case "LastUsedDate":
                        filter.FromLastUsedDate = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        filter.ToLastUsedDate = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                }
            }

            int itemCount;
            ISiteServiceService service = null;
            
            try
            {
                service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                creditVouchers = service.SearchCreditVouchers(PluginEntry.DataModel, siteServiceProfile, filter, out itemCount, true);
                dataScroll.RefreshState(creditVouchers, itemCount);

                foreach (CreditVoucher creditVoucher in creditVouchers)
                {
                    Row row = new Row();
                    row.AddText((string)creditVoucher.ID);
                    row.AddCell(new NumericCell(creditVoucher.Balance.FormatTruncated(), creditVoucher.Balance));
                    row.AddText((string)creditVoucher.Currency);
                    row.AddText(creditVoucher.CreatedDate == Date.Empty ? "-" : creditVoucher.CreatedDate.DateTime.ToString());
                    row.AddText(creditVoucher.LastUsedDate == Date.Empty ? "-" : creditVoucher.LastUsedDate.DateTime.ToString());

                    row.Tag = creditVoucher;
                    lvCreditVouchers.AddRow(row);

                    if (selectedID == creditVoucher.ID)
                    {
                        lvCreditVouchers.Selection.Set(lvCreditVouchers.RowCount - 1);
                    }
                }

                lvCreditVouchers_SelectionChanged(this, EventArgs.Empty);
                lvCreditVouchers.AutoSizeColumns(true);
            }
            catch (Exception ex)
            {
                MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
                return;
            }

            HideProgress();
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void lvCreditVouchers_SelectionChanged(object sender, EventArgs e)
        {
            selectedID = (lvCreditVouchers.Selection.Count == 1) ? ((CreditVoucher)lvCreditVouchers.Selection[0].Tag).ID : RecordIdentifier.Empty;

            btnsEditAddRemove.EditButtonEnabled = (lvCreditVouchers.Selection.Count == 1);
            btnsEditAddRemove.RemoveButtonEnabled = (lvCreditVouchers.Selection.Count >= 1);
        }

        private void lvCreditVouchers_HeaderClick(object sender, Controls.EventArguments.ColumnEventArgs e)
        {
            if (lvCreditVouchers.SortColumn == e.Column)
            {
                lvCreditVouchers.SetSortColumn(e.Column, !lvCreditVouchers.SortedAscending);
            }
            else
            {
                lvCreditVouchers.SetSortColumn(e.Column, true);
            }

            dataScroll.Reset();

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private List<IDataEntity> GetSelectedItems()
        {
            List<IDataEntity> selectedData = new List<IDataEntity>();

            for (int i = 0; i < lvCreditVouchers.Selection.Count; i++)
            {
                selectedData.Add((IDataEntity)lvCreditVouchers.Selection[i].Tag);
            }

            return selectedData;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvCreditVouchers.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("CreditVoucherList", lvCreditVouchers.ContextMenuStrip, lvCreditVouchers);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowCreditVoucherView(selectedID, creditVouchers.Cast<IDataEntity>());
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvCreditVouchers.Selection.Count == 1)
            {
                PluginOperations.DeleteCreditVoucher(selectedID, siteServiceProfile);

            }
            else
            {
                PluginOperations.DeleteCreditVouchers(GetSelectedItems(), siteServiceProfile);
            }          
        }

        private void lvCreditVouchers_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
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

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusTypes = new List<object>
            {
                Resources.NotEmpty, Resources.Empty, Resources.All
            };

            searchBar.AddCondition(new ConditionType(Resources.CreditMemoId, "Credit memo ID", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusTypes, 0, 0, false));
            searchBar.AddCondition(new ConditionType(Resources.Currency, "Currency", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.CreatedDate, "CreatedDate", ConditionType.ConditionTypeEnum.DateRange));
            searchBar.AddCondition(new ConditionType(Resources.LastUsedDate, "LastUsedDate", ConditionType.ConditionTypeEnum.DateRange));

            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            dataScroll.Reset();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            searchBar_SearchClicked(sender, e);
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
            if (args.TypeKey == "Currency")
            {
                args.UnknownControl = new DualDataComboBox();
                args.UnknownControl.Size = new Size(200, 21);
                args.MaxSize = 200;
                args.AutoSize = false;
                ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;
                ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");
                ((DualDataComboBox)args.UnknownControl).RequestData += new EventHandler(Currency_DropDown);
            }
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            if (args.TypeKey == "Currency")
            {
                args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            if (args.TypeKey == "Currency")
            {
                args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
            }
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            if (args.TypeKey == "Currency")
            {
                ((DualDataComboBox)args.UnknownControl).RequestData -= Currency_DropDown;
            }
        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;

            if (args.TypeKey == "Currency")
            {
                entity = Providers.CurrencyData.Get(PluginEntry.DataModel, args.Selection);
            }

            ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
        }

        private void Currency_DropDown(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SkipIDColumn = true;
            ((DualDataComboBox)sender).SetData(Providers.CurrencyData.GetList(PluginEntry.DataModel), null);
        }
    }
}