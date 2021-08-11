using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.CustomerLoyalty.Dialogs;
using LSOne.ViewPlugins.CustomerLoyalty.Properties;

namespace LSOne.ViewPlugins.CustomerLoyalty.Views
{
	public partial class LoyaltyCardsView : ViewBase
	{
		private RecordIdentifier selectedId;
		private LoyaltyCustomerParams loyaltyParams;
		private List<LoyaltyMSRCard> loyaltyCards;

	    private Setting searchBarSetting;
	    private const string SettingsGuid = "BAF1136D-F749-4228-B637-0F486E24F33B";

	    private Setting sortSetting;
	    private const string SortSettingGuid = "3EE64FD2-17D6-4857-BF0D-07E0E617A0AA";

	    private List<DataEntity> addList;
	    private List<DataEntity> removeList;
	    private List<DataEntity> selectedList;

	    private SiteServiceProfile siteServiceProfile;
	    private Parameters paramsData;

		// Constructor that opens the view with a line with Id == selectedId choosen
		public LoyaltyCardsView(RecordIdentifier selectedId)
			: this()
		{
			this.selectedId = selectedId;
		}

        // Constructor that opens the view without a line selected
        public LoyaltyCardsView()
        {
            InitializeComponent();
            searchBar1.BuddyControl = lvCards;

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.CardsEdit);

            paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;

            HeaderText = Properties.Resources.LoyaltyCardsView; // view header text
            btnsContextButtons.AddButtonEnabled = !ReadOnly;

            lvCards.Columns[0].Tag = LoyaltyMSRCardSorting.CardNumber;
            lvCards.Columns[1].Tag = LoyaltyMSRCardSorting.Type;

            lvCards.ContextMenuStrip = new ContextMenuStrip();
            lvCards.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            loyaltyParams = Providers.LoyaltyCustomerParamsData.Get(PluginEntry.DataModel); 

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CardsEdit);

            selectedId = RecordIdentifier.Empty;
            addList = new List<DataEntity>();
            removeList = new List<DataEntity>();
            selectedList = new List<DataEntity>();
        }

		// Constructor that opens the view without a line selected
		public LoyaltyCardsView(LoyaltyCustomerParams loyaltyParamsValue)
		{
			InitializeComponent();
		    searchBar1.BuddyControl = lvCards;

			Attributes = ViewAttributes.Audit |
				ViewAttributes.Close |
				ViewAttributes.ContextBar |
				ViewAttributes.Help;

			ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.CardsEdit);

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;

			HeaderText = Properties.Resources.LoyaltyCardsView; // view header text
			btnsContextButtons.AddButtonEnabled = !ReadOnly;

			lvCards.Columns[0].Tag = LoyaltyMSRCardSorting.CardNumber;
			lvCards.Columns[1].Tag = LoyaltyMSRCardSorting.Type;

			lvCards.ContextMenuStrip = new ContextMenuStrip();
			lvCards.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

		    loyaltyParams = loyaltyParamsValue;

			btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CardsEdit);

			selectedId = RecordIdentifier.Empty;
            addList = new List<DataEntity>();
		    removeList = new List<DataEntity>();
		    selectedList = new List<DataEntity>();
		}

		// This code decides what should appear when the user views the audit log for this view (presses F6)
		// If you don't have any audit information then this can be empty.

		public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
		{
		}

		// The text that appears above the ContextBar
		protected override string LogicalContextName
		{
			get
			{
				return Properties.Resources.LoyaltyCardsView;//"Text above context bar";
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
            sortSetting = PluginEntry.DataModel.Settings.GetSetting(
                PluginEntry.DataModel,
                new Guid(SortSettingGuid),
                SettingType.UISetting, lvCards.CreateSortSetting(0, true));

            lvCards.SortSetting = sortSetting.Value;
            ShowProgress((sender1, e1) => LoadDataInner(), GetLocalizedSearchingText());
		}

        public override void SaveUserInterface()
        {
            base.SaveUserInterface();
            string newSortSetting = lvCards.SortSetting;

            if (newSortSetting != sortSetting.Value)
            {
                sortSetting.Value = newSortSetting;
                sortSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, new Guid(SortSettingGuid), SettingsLevel.User, sortSetting);
            }
        }

		// Here we load all of our items into the list 
		private void LoadDataInner(bool reset = true)
		{
			if (reset)
			{
                itemDataScroll.Reset();
			}

		    string cardNumber = "";
		    List<DataEntity> schemes = new List<DataEntity>();
		    int types = -1;
            LoyaltyMSRCardInequality statusInequality = LoyaltyMSRCardInequality.GreaterThan;
		    double? status = null;
		    bool? hasCustomer = null;

		    if (paramsData == null || siteServiceProfile == null)
		    {
		        paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
		        siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
		    }

		    List<SearchParameterResult> results = searchBar1.SearchParameterResults;

		    foreach (SearchParameterResult result in results)
		    {
		        switch (result.ParameterKey)
		        {
                    case "CardNumber" :
                        cardNumber = result.SearchModification == SearchParameterResult.SearchModificationEnum.Contains ? "%" : "";
                        cardNumber += result.StringValue;
		                break;
                    case "Type" :
                        LoyaltyMSRCardTypeSearchEnum search = 0;
                        search |= result.CheckedValues[0] ? LoyaltyMSRCardTypeSearchEnum.CardTender : 0;
                        search |= result.CheckedValues[1] ? LoyaltyMSRCardTypeSearchEnum.ContactTender : 0;
		                search |= result.CheckedValues[2] ? LoyaltyMSRCardTypeSearchEnum.NoTender : 0;
                        search |= result.CheckedValues[3] ? LoyaltyMSRCardTypeSearchEnum.Blocked : 0;
		                types = (int)search;
		                break;
                    case "Scheme" :
		                if (((DualDataComboBox) result.UnknownControl).SelectedData != null)
		                {
		                    if (((DualDataComboBox) result.UnknownControl).SelectedData is DataEntitySelectionList)
		                    {
		                        schemes = ((DataEntitySelectionList) ((DualDataComboBox) result.UnknownControl).SelectedData).GetSelectedItems();
		                    }
		                    else
		                    {
		                        schemes = new List<DataEntity>();
                                schemes.Add((DataEntity)((DualDataComboBox) result.UnknownControl).SelectedData);
		                    }
		                }
		                else
		                {
		                    schemes = null;
		                }
		                break;
                    case "Balance" :
		                status = result.DoubleValue;
		                statusInequality = result.SearchModification == SearchParameterResult.SearchModificationEnum.Equals ? LoyaltyMSRCardInequality.Equals 
                            : result.SearchModification == SearchParameterResult.SearchModificationEnum.GreaterThan ? LoyaltyMSRCardInequality.GreaterThan 
                            : LoyaltyMSRCardInequality.LessThan;
		                break;
                    case "HasCustomer":
		                hasCustomer = result.CheckedValues[0];
		                break;
		        }
		    }

            // Need to create a list of the correct for for the connection to the site service
		    var passableList = new List<DataEntity>();
            foreach (var customer in selectedList)
            {
                passableList.Add(new DataEntity(customer.ID, customer.Text));
            }

		    List<DataEntity> schemesData = null;
		    if (schemes != null)
		    {
		        schemesData = new List<DataEntity>();
		        foreach (DataEntity dataEntity in schemes)
		        {
                    schemesData.Add(new DataEntity(dataEntity.ID, dataEntity.Text));
		        }
		    }

		  
            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

            loyaltyCards = service.GetCustomerMSRCards(PluginEntry.DataModel,
                                                        siteServiceProfile,
                                                        passableList,
                                                        schemesData,
                                                        cardNumber, 
                                                        hasCustomer, 
                                                        status, 
                                                        statusInequality, 
                                                        types, 
                                                        itemDataScroll.StartRecord, 
                                                        itemDataScroll.EndRecord + 1, 
                                                        (LoyaltyMSRCardSorting)lvCards.SortColumn.Tag, 
                                                        !lvCards.SortedAscending, 
                                                        true);
          
			lvCards.ClearRows();
            itemDataScroll.RefreshState(loyaltyCards);
			if (loyaltyCards != null)
			{
                List<LoyaltyPoints> tenderRules = new List<LoyaltyPoints>();
			    LoyaltyPoints rule = null;
				var qtyLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
				var priceLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
				foreach (LoyaltyMSRCard card in loyaltyCards)
				{
					var row = new Row();
					row.AddText(card.CardNumber);
					row.AddText(card.TenderTypeAsString);
					row.AddText(card.CustomerName);
					row.AddText(card.SchemeDescription);

                    if (tenderRules.Count(c => c.SchemeID == card.SchemeID) == 0)
                    {
                        rule = Providers.LoyaltyPointsData.GetPointsExchangeRate(PluginEntry.DataModel, card.SchemeID);
                        if (rule != null)
                        {
                            rule.Points = rule.Points < decimal.Zero ? rule.Points * -1 : rule.Points;
                            tenderRules.Add(rule);
                        }
                    }
					
					row.AddText(card.StartingPoints == decimal.Zero ? "" : card.StartingPoints.FormatWithLimits(qtyLimit));
                    row.AddText(card.IssuedPoints == decimal.Zero ? "" : card.IssuedPoints.FormatWithLimits(qtyLimit));
                    row.AddText(card.UsedPoints == decimal.Zero ? "" : card.UsedPoints.FormatWithLimits(qtyLimit));
                    row.AddText(card.ExpiredPoints == decimal.Zero ? "" : card.ExpiredPoints.FormatWithLimits(qtyLimit));

				    decimal currentValue = decimal.Zero;

				    if (card.PointStatus != decimal.Zero)
				    {
				        rule = tenderRules.FirstOrDefault(f => f.SchemeID == card.SchemeID);
				        if (rule != null)
				        {
				            currentValue = (rule.QtyAmountLimit/rule.Points)*card.PointStatus;
				        }
				    }

                    row.AddText(currentValue == decimal.Zero ? "" : currentValue.FormatWithLimits(priceLimit, true));

                    row.AddText(card.PointStatus == decimal.Zero ? "" : card.PointStatus.FormatWithLimits(qtyLimit));
					row.Tag = card.ID;

				    switch (card.TenderType)
				    {
				        case LoyaltyMSRCard.TenderTypeEnum.NoTender:
				            row.BackColor = ColorPalette.MustardLight;
                            break;
				        case LoyaltyMSRCard.TenderTypeEnum.Blocked:
				            row.BackColor = ColorPalette.RedLight;
                            break;                        
				    }

				    lvCards.AddRow(row);

				    if (card.ID == selectedId)
				    {
				        lvCards.Selection.Set(lvCards.RowCount - 1);
				    }
				}
			}
		    lvCards.AutoSizeColumns();
		    HideProgress();
		}

	    private void DualDataComboBox_RequestClear(object sender, EventArgs e)
	    {
	        ((DualDataComboBox) sender).SelectedData = null;
	    }

	    private void dcbScheme_DropDown(object sender, DropDownEventArgs e)
	    {
	        if (((DualDataComboBox) sender).SelectedData == null || !((((DualDataComboBox) sender).SelectedData) is DataEntitySelectionList) || !((DataEntitySelectionList) ((DualDataComboBox) sender).SelectedData).HasSelection)
	        {
	            List<LoyaltySchemes> schemes;

	            var service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

	            schemes = service.GetLoyaltySchemes(PluginEntry.DataModel, siteServiceProfile, true);

	            ((DualDataComboBox) sender).SelectedData = new DataEntitySelectionList(schemes);
	        }
	        e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList) ((DualDataComboBox) sender).SelectedData);
	    }

	    private void OnPageScrollPageChanged(object sender, EventArgs e)
	    {
	        ShowProgress((sender1, e1) => LoadDataInner(false), GetLocalizedSearchingText());
	    }

	    private void lvCards_SelectionChanged(object sender, EventArgs e)
	    {
	        bool objectSelected = (lvCards.Selection.Count != 0) && (lvCards.Selection.FirstSelectedRow >= 0);
	        if (objectSelected)
	        {
	            var selectedObjectId = (RecordIdentifier) lvCards.Row(lvCards.Selection.FirstSelectedRow).Tag;
	            selectedId = selectedObjectId;
	        }

	        btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = objectSelected && PluginEntry.DataModel.HasPermission(Permission.CardsEdit);
	    }

	    private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
	    {
	        var dlg = new LoyaltyCardDialog(loyaltyParams, siteServiceProfile);
	        if (dlg.ShowDialog() == DialogResult.OK)
	        {
	            ShowProgress((sender1, e1) => LoadDataInner(), GetLocalizedSearchingText());
	        }
	    }

	    private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
	    {
	        bool objectSelected = (lvCards.Selection.Count != 0) && (lvCards.Selection.FirstSelectedRow >= 0);
	        if (objectSelected)
	        {
	            RecordIdentifier selectedObjectId = selectedId = (RecordIdentifier) lvCards.Row(lvCards.Selection.FirstSelectedRow).Tag;
	            var dlg = new LoyaltyCardDialog(selectedObjectId, loyaltyParams, siteServiceProfile);
	            if (dlg.ShowDialog() == DialogResult.OK)
	            {
	                ShowProgress((sender1, e1) => LoadDataInner(), GetLocalizedSearchingText());
	            }
	        }
	    }

	    private void btnsContextButtons_RemoveButtonClicked(object sender, EventArgs e)
	    {
	        bool objectSelected = (lvCards.Selection.Count != 0) && (lvCards.Selection.FirstSelectedRow >= 0);
	        if (objectSelected)
	        {
	            if (QuestionDialog.Show(Resources.DeleteCardQuestion, Properties.Resources.LoyaltyCardsView) == DialogResult.Yes)
	            {
	                var selectedObjectId = (RecordIdentifier) lvCards.Row(lvCards.Selection.FirstSelectedRow).Tag;

	                var service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

	                service.DeleteLoyaltyMSRCard(PluginEntry.DataModel, siteServiceProfile, selectedObjectId, true);

	                ShowProgress((sender1, e1) => LoadDataInner(), GetLocalizedSearchingText());
	            }
	        }
	    }

	    private void dcbCustomer_DropDown(object sender, DropDownEventArgs e)
	    {
	        e.ControlToEmbed = new MultiSearchPanel(PluginEntry.DataModel, selectedList, addList, removeList, SearchTypeEnum.Customers, false);
	    }

	    protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
	    {
	        if (arguments.CategoryKey == GetType() + ".View")
	        {
	            if (PluginEntry.DataModel.HasPermission(Permission.CardsEdit))
	            {
	                arguments.Add(new ContextBarItem(Resources.Add, ContextButtons.GetAddButtonImage(), btnsContextButtons_AddButtonClicked), 10);
	            }
	        }
	        else if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
	        {
	            if (PluginEntry.DataModel.HasPermission(Permission.SchemesView))
	            {
	                arguments.Add(new ContextBarItem(Properties.Resources.LoyaltySchemesView, new ContextbarClickEventHandler(PluginOperations.ShowLoyaltySchemesView)), 10);
	            }

	            arguments.Add(new ContextBarItem(Properties.Resources.LoyaltyTransView, new ContextbarClickEventHandler(PluginOperations.ShowLoyaltyTransView)), 20);
	        }
	    }

	    private void lvCards_HeaderClicked(object sender, ColumnEventArgs args)
	    {
	        lvCards.SetSortColumn(args.ColumnNumber, !lvCards.SortedAscending);
	        ShowProgress((sender1, e1) => LoadDataInner(), GetLocalizedSearchingText());
	    }

	    private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
	    {
	        ContextMenuStrip menu = lvCards.ContextMenuStrip;
	        menu.Items.Clear();

	        var item = new ExtendedMenuItem(Resources.Edit + "...", 100, btnsContextButtons_EditButtonClicked)
	        {
	            Enabled = btnsContextButtons.EditButtonEnabled, Image = ContextButtons.GetEditButtonImage(), Default = true
	        };

	        menu.Items.Add(item);

	        item = new ExtendedMenuItem(Resources.Add + "...", 200, btnsContextButtons_AddButtonClicked)
	        {
	            Enabled = btnsContextButtons.AddButtonEnabled, Image = ContextButtons.GetAddButtonImage()
	        };

	        menu.Items.Add(item);

	        item = new ExtendedMenuItem(Resources.Delete + "...", 300, btnsContextButtons_RemoveButtonClicked)
	        {
	            Enabled = btnsContextButtons.RemoveButtonEnabled, Image = ContextButtons.GetRemoveButtonImage()
	        };

	        menu.Items.Add(item);

	        e.Cancel = (menu.Items.Count == 0);
	    }

	    private void gridControl1_RowDoubleClick(object sender, RowEventArgs args)
	    {
	        if (btnsContextButtons.EditButtonEnabled)
	        {
	            btnsContextButtons_EditButtonClicked(this, EventArgs.Empty);
	        }
	    }

	    private void searchBar1_LoadDefault(object sender, EventArgs e)
	    {
	        searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, new Guid(SettingsGuid), SettingType.UIFieldVisisbility, "");
	        if (searchBarSetting.LongUserSetting != "")
	        {
	            searchBar1.LoadFromSetupString(searchBarSetting.LongUserSetting);
	        }
	    }

	    private void searchBar1_SaveAsDefault(object sender, EventArgs e)
	    {
	        string layoutString = searchBar1.LayoutStringWithData;

	        if (searchBarSetting.LongUserSetting != layoutString)
	        {
	            searchBarSetting.LongUserSetting = layoutString;
	            searchBarSetting.UserSettingExists = true;

	            PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, new Guid(SettingsGuid), SettingsLevel.User, searchBarSetting);
	        }
	        ShowTimedProgress(searchBar1.GetLocalizedSavingText());
	    }

	    private void searchBar1_SetupConditions(object sender, EventArgs e)
	    {
	        searchBar1.AddCondition(new ConditionType(Resources.CardNumber, "CardNumber", ConditionType.ConditionTypeEnum.Text));
	        searchBar1.AddCondition(new ConditionType(Resources.Types, "Type", ConditionType.ConditionTypeEnum.Checkboxes, Resources.CardTender, true, Resources.CustomerTender, true, Resources.NoTender, true, Resources.Blocked, true));
	        searchBar1.AddCondition(new ConditionType(Resources.Customer, "Customer", ConditionType.ConditionTypeEnum.Unknown));
	        searchBar1.AddCondition(new ConditionType(Resources.Scheme, "Scheme", ConditionType.ConditionTypeEnum.Unknown));
	        searchBar1.AddCondition(new ConditionType(Resources.CardHasCustomer, "HasCustomer", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Yes, true));
	        searchBar1.AddCondition(new ConditionType(Resources.Balance, "Balance", ConditionType.ConditionTypeEnum.Numeric));

	        searchBar1_LoadDefault(this, EventArgs.Empty);
	    }

	    private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
	    {
	        switch (args.TypeKey)
	        {
	            case "Customer":
	                args.UnknownControl = new DualDataComboBox();
	                args.UnknownControl.Name = "Customer";
	                args.UnknownControl.Size = new Size(200, 21);
	                args.MaxSize = 200;
	                args.AutoSize = false;
	                ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;

	                ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity(RecordIdentifier.Empty, "");

	                ((DualDataComboBox) args.UnknownControl).DropDown += dcbCustomer_DropDown;
	                ((DualDataComboBox) args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;
	                break;
	            case "Scheme":
	                args.UnknownControl = new DualDataComboBox();
	                args.UnknownControl.Size = new Size(200, 21);
	                args.MaxSize = 200;
	                args.AutoSize = false;
	                ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;

	                ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity(RecordIdentifier.Empty, "");

	                ((DualDataComboBox) args.UnknownControl).DropDown += dcbScheme_DropDown;
	                ((DualDataComboBox) args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;
	                break;
	        }
	    }

	    private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
	    {
	        switch (args.TypeKey)
	        {
	            case "Customer":
	                ((DualDataComboBox) args.UnknownControl).DropDown -= dcbCustomer_DropDown;
	                ((DualDataComboBox) args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
	                break;
	            case "Scheme":
	                ((DualDataComboBox) args.UnknownControl).DropDown -= dcbScheme_DropDown;
	                ((DualDataComboBox) args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
	                break;
	        }
	    }

	    private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
	    {
	        switch (args.TypeKey)
	        {
	            case "Customer":
	                args.HasSelection = ((DualDataComboBox) args.UnknownControl).SelectedData.ID != "" && ((DualDataComboBox) args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
	                break;
	            case "Scheme":
	                args.HasSelection = ((DualDataComboBox) args.UnknownControl).SelectedData is DataEntitySelectionList ? ((DataEntitySelectionList) ((DualDataComboBox) args.UnknownControl).SelectedData).HasSelection : ((DualDataComboBox) args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
	                break;
	        }
	    }

	    private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
	    {
	        switch (args.TypeKey)
	        {
	            case "Customer":
	                args.Selection = (string) ((DualDataComboBox) args.UnknownControl).SelectedData.ID == "MultiSelected" ? SearchBar.GetSelectionString(selectedList.Cast<IDataEntity>().ToList()) : (string) ((DualDataComboBox) args.UnknownControl).SelectedData.ID;
	                break;
	            case "Scheme":

	                args.Selection = ((DualDataComboBox) args.UnknownControl).SelectedData is DataEntitySelectionList ? SearchBar.GetSelectionString(((DataEntitySelectionList) ((DualDataComboBox) args.UnknownControl).SelectedData).GetSelectedItems().Cast<IDataEntity>().ToList()) : "";
	                break;
	        }
	    }


	    private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
	    {
	        DataEntity entity = null;
	        string[] selectedIDs = args.Selection != "" ? args.Selection.Split(',') : null;
	        switch (args.TypeKey)
	        {
	            case "Customer":
	                if (selectedIDs != null)
	                {
	                    foreach (string selectedID in selectedIDs)
	                    {
	                        entity = Providers.CustomerData.Get(PluginEntry.DataModel, selectedID, UsageIntentEnum.Normal);
	                        selectedList.Add(entity);
	                    }
	                    ((DualDataComboBox) args.UnknownControl).SelectedData = selectedIDs.Length > 1 ? DualDataComboBox.MultiSelectEntity : selectedIDs.Length == 1 ? selectedList[0] : new DataEntity(RecordIdentifier.Empty, "");
	                }
	                else
	                {
	                    ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity(RecordIdentifier.Empty, "");
	                }
	                break;
	            case "Scheme":
	                List<LoyaltySchemes> schemes = new List<LoyaltySchemes>();
	                if (selectedIDs != null)
	                {
	                    foreach (string selectedID in selectedIDs)
	                    {
	                        schemes.Add(Providers.LoyaltySchemesData.Get(PluginEntry.DataModel, selectedID));
	                    }
	                }

	                var schemesEntities = Providers.LoyaltySchemesData.GetList(PluginEntry.DataModel);
	                DataEntitySelectionList entitySelectionList = new DataEntitySelectionList(schemesEntities);
	                entitySelectionList.SelectSome(schemes);
	                ((DualDataComboBox) args.UnknownControl).SelectedData = entitySelectionList;
	                break;
	        }
	    }

	    private void searchBar1_SearchClicked(object sender, EventArgs e)
	    {
	        ShowProgress((sender1, e1) => LoadDataInner(), GetLocalizedSearchingText());
	    }

	    private void searchBar1_ControlRemoved(object sender, ControlEventArgs e)
	    {
	        if (e.Control is DualDataComboBox && e.Control.Name == "Customer")
	        {
	            addList.Clear();
	            removeList.Clear();
	            selectedList.Clear();
	        }
	    }
	}
}
