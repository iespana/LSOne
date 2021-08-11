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
using LSOne.ViewPlugins.GiftCards.Properties;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using System.Drawing;
using LSOne.Controls.Rows;
using LSOne.Controls.Cells;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.GiftCards.Views
{
	public partial class GiftCardsView : ViewBase
	{
        private static Guid BarSettingID = new Guid("32ec81e9-2969-4762-945f-ec72ef5ddbd9");

        List<GiftCard> giftCards;

        RecordIdentifier selectedID = "";

	    private SiteServiceProfile siteServiceProfile;
        private Setting searchBarSetting;

        private bool allowManualGiftCardIDEntry;

        public GiftCardsView()
		{
			InitializeComponent();

			Attributes = 
				ViewAttributes.Help |
				ViewAttributes.Close |
				ViewAttributes.ContextBar;

            HeaderText = Resources.GiftCards;

            lvGiftCards.ContextMenuStrip = new ContextMenuStrip();
            lvGiftCards.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvGiftCards.Columns[0].Tag = GiftCard.SortEnum.ID;
            lvGiftCards.Columns[1].Tag = GiftCard.SortEnum.Balance;
            lvGiftCards.Columns[2].Tag = GiftCard.SortEnum.Currency;
            lvGiftCards.Columns[3].Tag = GiftCard.SortEnum.Active;
            lvGiftCards.Columns[4].Tag = GiftCard.SortEnum.Refillable;
            lvGiftCards.Columns[5].Tag = GiftCard.SortEnum.CreatedDate;
            lvGiftCards.Columns[6].Tag = GiftCard.SortEnum.LastUsedDate;

            lvGiftCards.SetSortColumn(0, true);

            searchBar.BuddyControl = lvGiftCards;

            giftCardDataScroll.PageSize = PluginEntry.DataModel.PageSize;
            giftCardDataScroll.Reset();

            searchBar.FocusFirstInput();

            var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
            allowManualGiftCardIDEntry = paramsData.ManuallyEnterGiftCardID;
        }

		protected override string LogicalContextName
		{
			get
			{
				return Resources.GiftCards;
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
                case "GiftCard":
                    ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
                    break;
            }
		}

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewGiftCards(this, EventArgs.Empty);
        }

	    private void LoadItems()
	    {
            GiftCardFilter filter = new GiftCardFilter();

            RecordIdentifier currentlySelectedID = selectedID; // Preserve the selection since ClearRows() will clear the selection
            lvGiftCards.ClearRows();
            selectedID = currentlySelectedID;


            if (lvGiftCards.SortColumn == null)
            {
                lvGiftCards.SetSortColumn(lvGiftCards.Columns[0], true);
            }

            filter.Sort = (GiftCard.SortEnum)lvGiftCards.SortColumn.Tag;
            filter.SortAscending = lvGiftCards.SortedAscending;
            filter.RowFrom = giftCardDataScroll.StartRecord;
            filter.RowTo = giftCardDataScroll.EndRecord + 1;

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Gift card ID":
                        filter.VoucherID = result.StringValue;
                        filter.VoucherIDBeginsWith = result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith;
                        break;
                    case "Status":
                        switch (result.ComboSelectedIndex)
                        {
                            case 0:
                                filter.Status = GiftCardStatusEnum.Active;
                                break;
                            case 1:
                                filter.Status = GiftCardStatusEnum.Inactive;
                                break;
                            case 2:
                                filter.Status = GiftCardStatusEnum.NotEmpty;
                                break;
                            case 3:
                                filter.Status = GiftCardStatusEnum.Empty;
                                break;
                            case 4:
                                filter.Status = GiftCardStatusEnum.All;
                                break;
                        }
                        break;
                    case "Currency":
                        filter.CurrencyID = ((DualDataComboBox)result.UnknownControl).SelectedData.ID;
                        break;
                    case "Refillable":
                        filter.Refillable = result.CheckedValues[0];
                        break;
                    case "Balance":
                        filter.FromBalance = result.FromValue;
                        filter.ToBalance = result.ToValue;
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

                giftCards = service.SearchGiftCards(PluginEntry.DataModel, siteServiceProfile, filter, out itemCount, true);
                giftCardDataScroll.RefreshState(giftCards, itemCount);

                foreach(GiftCard giftCard in giftCards)
                {
                    Row row = new Row();
                    row.AddText((string)giftCard.ID);
                    row.AddCell(new NumericCell(giftCard.Balance.FormatTruncated(), giftCard.Balance));
                    row.AddText((string)giftCard.Currency);
                    row.AddText(giftCard.Active ? Resources.Yes : Resources.No);
                    row.AddText(giftCard.Refillable ? Resources.Yes : Resources.No);
                    row.AddText(giftCard.CreatedDate == Date.Empty ? "-" : giftCard.CreatedDate.DateTime.ToString());
                    row.AddText(giftCard.LastUsedDate == Date.Empty ? "-" : giftCard.LastUsedDate.DateTime.ToString());

                    row.Tag = giftCard;
                    lvGiftCards.AddRow(row);

                    if (selectedID == giftCard.ID)
                    {
                        lvGiftCards.Selection.Set(lvGiftCards.RowCount - 1);
                    }
                }

                lvGiftCards_SelectionChanged(this, EventArgs.Empty);
                lvGiftCards.AutoSizeColumns(true);
            }
	        catch (Exception ex)
	        {
                MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
            }

            HideProgress();
        }

        private void OnPageScrollPageChanged(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void lvGiftCards_SelectionChanged(object sender, EventArgs e)
        {
            bool? activated = null;

            selectedID = (lvGiftCards.Selection.Count == 1) ? ((GiftCard)lvGiftCards.Selection[0].Tag).ID : RecordIdentifier.Empty;

            btnsEditAddRemove.EditButtonEnabled = (lvGiftCards.Selection.Count == 1);
            btnsEditAddRemove.RemoveButtonEnabled = (lvGiftCards.Selection.Count >= 1);

            if(lvGiftCards.Selection.Count >= 1)
            {
                // See if all the gift cards have same activation status
                for(int i = 0; i < lvGiftCards.Selection.Count; i++)
                {
                    GiftCard card = (GiftCard)lvGiftCards.Selection[i].Tag;

                    if(!activated.HasValue)
                    {
                        activated = card.Active;
                    }
                    else if(card.Active != activated.Value)
                    {
                        activated = null;
                        break; // Operation not possible
                    }
                }
            }

            if(!activated.HasValue)
            {
                btnActivate.Text = Resources.Activate;
                btnActivate.Enabled = false;
            }
            else if (activated.Value)
            {
                btnActivate.Text = Resources.Deactivate;
                btnActivate.Enabled = (lvGiftCards.Selection.Count >= 1);
            }
            else
            {
                btnActivate.Text = Resources.Activate;
                btnActivate.Enabled = (lvGiftCards.Selection.Count >= 1);
            }
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvGiftCards.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnsEditAddRemove_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Resources.Add,
                   200,
                   btnsEditAddRemove_AddButtonClicked)
                {
                    Enabled = btnsEditAddRemove.AddButtonEnabled,
                    Image = ContextButtons.GetAddButtonImage()
                };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.Delete,
                    300,
                    btnsEditAddRemove_RemoveButtonClicked)
                {
                    Image = ContextButtons.GetRemoveButtonImage(),
                    Enabled = btnsEditAddRemove.RemoveButtonEnabled
                };

            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    btnActivate.Text,
                    500,
                    btnActivate_Click) {Enabled = btnActivate.Enabled};

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("GiftCardList", lvGiftCards.ContextMenuStrip, lvGiftCards);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowGiftCard(selectedID, giftCards.Cast<IDataEntity>());
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvGiftCards.Selection.Count == 1)
            {
                PluginOperations.DeleteGiftCard(selectedID, siteServiceProfile);
            }
            else
            {
                PluginOperations.DeleteGiftCards(GetSelectedItems(), siteServiceProfile);
            }          
        }

        private List<IDataEntity> GetSelectedItems()
        {
            List<IDataEntity> selectedData = new List<IDataEntity>();

            for(int i = 0; i < lvGiftCards.Selection.Count; i++)
            {
                selectedData.Add((IDataEntity)lvGiftCards.Selection[i].Tag);
            }

            return selectedData;
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            GiftCard card;
            RecordIdentifier storeID = (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty) ? "" : PluginEntry.DataModel.CurrentStoreID;

            for(int i = 0; i < lvGiftCards.Selection.Count; i++)
            {
                card = (GiftCard)lvGiftCards.Selection[i].Tag;
 
                var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                    try
                    {
                        if (card.Active)
                        {
                            if (service.DeactivateGiftCard(PluginEntry.DataModel, siteServiceProfile, card.ID, true))
                            {
                                card.Active = false;
                                lvGiftCards.Selection[i].WritableCell(3).Text = Resources.No;
                            }
                        }
                        else
                        {
                            if (service.ActivateGiftCard(PluginEntry.DataModel, siteServiceProfile, card.ID, "", "", true))
                            {
                                card.Active = true;
                                lvGiftCards.Selection[i].WritableCell(3).Text = Resources.Yes;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);

                        return;
                    }

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.VariableChanged, "GiftCard", card.ID, new object[] { "Active", card.Active }); 
            }

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void lvGiftCards_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvGiftCards_HeaderClicked(object sender, Controls.EventArguments.ColumnEventArgs args)
        {
            if (lvGiftCards.SortColumn == args.Column)
            {
                lvGiftCards.SetSortColumn(args.Column, !lvGiftCards.SortedAscending);
            }
            else
            {
                lvGiftCards.SetSortColumn(args.Column, true);
            }

            giftCardDataScroll.Reset();

            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> statusTypes = new List<object>
            {
                Resources.Active, Resources.Inactive, Resources.NotEmpty, Resources.Empty, Resources.All
            };

            searchBar.AddCondition(new ConditionType(Resources.GiftCardID, "Gift card ID", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.ComboBox, statusTypes, 4, 0, false));
            searchBar.AddCondition(new ConditionType(Resources.Currency, "Currency", ConditionType.ConditionTypeEnum.Unknown));
            searchBar.AddCondition(new ConditionType(Resources.Refillable, "Refillable", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Yes, false));
            searchBar.AddCondition(new ConditionType(Resources.Balance, "Balance", ConditionType.ConditionTypeEnum.NumericRange));
            searchBar.AddCondition(new ConditionType(Resources.CreatedDate, "CreatedDate", ConditionType.ConditionTypeEnum.DateRange));
            searchBar.AddCondition(new ConditionType(Resources.LastUsedDate, "LastUsedDate", ConditionType.ConditionTypeEnum.DateRange));

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

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            giftCardDataScroll.Reset();
            ShowProgress((sender1, e1) => LoadItems(), GetLocalizedSearchingText());
        }

        private void searchBar_SearchOptionChanged(object sender, EventArgs e)
        {
            searchBar_SearchClicked(sender, e);
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            if(args.TypeKey == "Currency")
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
            if(args.TypeKey == "Currency")
            {
                args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
            }
        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            if(args.TypeKey == "Currency")
            {
                args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
            }
        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            if(args.TypeKey == "Currency")
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

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".Related")
            {
                
            }

            if (arguments.CategoryKey == GetType().ToString() + ".Actions")
            {
                if (allowManualGiftCardIDEntry)
                {
                    arguments.Add(new ContextBarItem(Resources.NewGiftCardWithID, null, true, PluginOperations.NewGiftCardWithID), 100);
                }
            }

            base.OnSetupContextBarItems(arguments);
        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {
            if (allowManualGiftCardIDEntry)
            {
                arguments.Add(new ContextBarHeader(Resources.Action, GetType().ToString() + ".Actions"), 200);
            }

            base.OnSetupContextBarHeaders(arguments);          
        }
    }
}