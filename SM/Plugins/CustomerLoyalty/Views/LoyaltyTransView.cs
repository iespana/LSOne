using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.CustomerLoyalty.Properties;

namespace LSOne.ViewPlugins.CustomerLoyalty.Views
{
    public partial class LoyaltyTransView : ViewBase
	{
		private LoyaltyCustomerParams loyaltyParams;
		private List<LoyaltyMSRCardTrans> loyaltyTrans;

        private Setting searchBarSetting;
        private const string SettingsGuid = "EFC86AB2-777C-47D7-ABAE-570443CDBE85";

        private Setting sortSetting;
        private const string SortSettingGuid = "53419F60-ECE6-4143-A3D7-F996B02EF524";

	    private DataEntity cmbStoreSelectedItem;
	    private DataEntity cmbCustomerSelectedItem;

	    private SiteServiceProfile siteServiceProfile;
	    private Parameters paramsData;

		// Constructor that opens the view without a line selected
        public LoyaltyTransView(LoyaltyCustomerParams loyaltyParamsValue)
		{
			InitializeComponent();

		    searchBar1.BuddyControl = lvTransactions;

			Attributes = 
				ViewAttributes.Close |
				ViewAttributes.ContextBar |
				ViewAttributes.Help;

			HeaderText = Resources.LoyaltyTransView; // view header text

            loyaltyParams = loyaltyParamsValue;

            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;

            paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
        }

		// This code decides what should appear when the user views the audit log for this view (presses F6)
		// If you don't have any audit information then this can be empty.
		public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
		{
		}

		protected override string LogicalContextName
		{
			get
			{
				return Resources.LoyaltyTransView;//"Text above context bar";
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
                SettingType.UISetting, lvTransactions.CreateSortSetting(0, true));

            lvTransactions.SortSetting = sortSetting.Value;
            ShowProgress((sender1, e1) => LoadDataInner(), GetLocalizedSearchingText());
		}

        public override void SaveUserInterface()
        {
            base.SaveUserInterface();
            string newSortSetting = lvTransactions.SortSetting;

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

		    Date fromExpDate = Date.Empty;
		    Date toExpDate = Date.Empty;

            Date fromDate = Date.Empty;
            Date toDate = Date.Empty;

            string selectedStore = "";
            string selectedTerminal = "";
            string selectedCard = "";
            string selectedScheme = "";
            string receipt = "";
		    string selectedCustomer = "";
            int type = -1;
            int open = -1;

            List<DataEntity> schemes = new List<DataEntity>();
            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            if (paramsData == null || siteServiceProfile == null)
            {
                paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
            }

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Store":
                        selectedStore = ((DualDataComboBox)result.UnknownControl).SelectedData != null ? (string)((DualDataComboBox)result.UnknownControl).SelectedData.ID : null;
                        break;
                    case "Terminal":
                        selectedTerminal = ((DualDataComboBox)result.UnknownControl).SelectedData != null ? (string)((DualDataComboBox)result.UnknownControl).SelectedData.ID : null;
                        break;
                    case "LoyaltyCard":
                        selectedCard = ((DualDataComboBox)result.UnknownControl).SelectedData != null ? (string)((DualDataComboBox)result.UnknownControl).SelectedData.ID : null;
                        break;
                    case "Scheme":
                        if (((DualDataComboBox)result.UnknownControl).SelectedData != null)
                        {
                            if (((DualDataComboBox)result.UnknownControl).SelectedData is DataEntitySelectionList)
                            {
                                schemes = ((DataEntitySelectionList)((DualDataComboBox)result.UnknownControl).SelectedData).GetSelectedItems();
                            }
                            else
                            {
                                schemes = new List<DataEntity>();
                                schemes.Add((DataEntity)((DualDataComboBox)result.UnknownControl).SelectedData);
                            }
                        }
                        else
                        {
                            schemes = null;
                        }
                        break;
                    case "Customer":
                        selectedCustomer = ((DualDataComboBox)result.UnknownControl).SelectedData != null ? (string)((DualDataComboBox)result.UnknownControl).SelectedData.ID : null;
                        break;
                    case "ReceiptNumber":
                        receipt = result.SearchModification == SearchParameterResult.SearchModificationEnum.Contains ? "%" : "";
                        receipt += result.StringValue;
                        break;
                    case "Date":
                        fromDate = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        toDate = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                    case "ExpirationDate":
                        fromExpDate = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        toExpDate = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
                        break;
                    case "Status":
                        open = result.CheckedValues[1] && !result.CheckedValues[0] ? 0 : result.CheckedValues[0] && !result.CheckedValues[1] ? 1 : -1;
                        break;
                    case "Types":
                        if (result.CheckedValues[0] && result.CheckedValues[1] && result.CheckedValues[2])
                        {
                            break;
                        }
                        LoyaltyMSRCardTransTypeSearchEnum search = 0;
                        search |= result.CheckedValues[1] ? LoyaltyMSRCardTransTypeSearchEnum.IssuePoints : 0;
                        search |= result.CheckedValues[2] ? LoyaltyMSRCardTransTypeSearchEnum.UsePoints : 0;
                        search |= result.CheckedValues[0] ? LoyaltyMSRCardTransTypeSearchEnum.ExpirePoints : 0;
                        type = (int)search;
                        break;
                }
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
		   
		    try
		    {
		        var service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
		        loyaltyTrans = service.GetLoyaltyTrans(PluginEntry.DataModel,
                                                        siteServiceProfile,
                                                        selectedStore,
		                                                selectedTerminal,
		                                                selectedCard,
		                                                selectedScheme,
		                                                type,
		                                                open,
		                                                (int) LoyaltyMSRCardTrans.EntryTypeEnum.None,
		                                                selectedCustomer,
		                                                receipt,
		                                                fromDate,
		                                                toDate,
		                                                fromExpDate,
		                                                toExpDate,
		                                                itemDataScroll.StartRecord,
		                                                itemDataScroll.EndRecord + 1,
		                                                false,
		                                                true);
		    }
		    catch (Exception)
		    {
		        MessageDialog.Show(Resources.ErrorConnectingToSiteService, MessageBoxIcon.Error);
		        return;
		    }

		    lvTransactions.ClearRows();
            itemDataScroll.RefreshState(loyaltyTrans);
			if (loyaltyTrans != null)
			{
				var qtyLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
				foreach (var trans in loyaltyTrans)
				{
					var row = new Row();
					row.AddText(trans.TypeAsString);
					row.AddText((string)trans.ReceiptID);
					row.AddText(trans.Points.FormatWithLimits(qtyLimit));
					row.AddText(trans.RemainingPoints.FormatWithLimits(qtyLimit));
					row.AddText(trans.StatusAsString);
					row.AddText(trans.CreatedDate.ToShortDateString());
					row.AddText(trans.DateOfIssue.ToShortDateString());
					row.AddText(trans.StoreName);
					row.AddText(trans.TerminalName);
					row.AddText(trans.CardNumber);
					row.AddText(trans.SchemeDescription);
					row.AddText(trans.ExpirationDate.ToShortDateString());
					row.AddText(trans.CustomerName);
					row.AddText((string)trans.StaffID + " - " + trans.StaffName);
                    lvTransactions.AddRow(row);
				}
			}
			lvTransactions.AutoSizeColumns();
            HideProgress();
		}

		private void dcbStore_RequestData(object sender, EventArgs e)
		{
			((DualDataComboBox)sender).SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
		}

		private void dcbTerminal_RequestData(object sender, EventArgs e)
		{
			if (cmbStoreSelectedItem != null && cmbStoreSelectedItem.ID != "")
			{
                ((DualDataComboBox)sender).SetData(Providers.TerminalData.GetList(PluginEntry.DataModel, cmbStoreSelectedItem.ID), null);
			}
			else
			{
                ((DualDataComboBox)sender).SetData(Providers.TerminalData.GetList(PluginEntry.DataModel), null);
			}
		}

		private void dcbScheme_DropDown(object sender, DropDownEventArgs e)
		{
            if (((DualDataComboBox)sender).SelectedData == null || !((((DualDataComboBox)sender).SelectedData) is DataEntitySelectionList) || !((DataEntitySelectionList)((DualDataComboBox)sender).SelectedData).HasSelection)
            {
                List<LoyaltySchemes> schemes;

                var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

                schemes = service.GetLoyaltySchemes(PluginEntry.DataModel, siteServiceProfile, true);

                ((DualDataComboBox)sender).SelectedData = new DataEntitySelectionList(schemes);
            }
            e.ControlToEmbed = new CheckBoxSelectionListPanel((DataEntitySelectionList)((DualDataComboBox)sender).SelectedData);
		}

		private void DualDataComboBox_RequestClear(object sender, EventArgs e)
		{
			((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
		}

		private void dcbCard_RequestData(object sender, EventArgs e)
		{
		   
		    var service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            List<SearchParameterResult> results = searchBar1.SearchParameterResults;

		    foreach (SearchParameterResult result in results)
		    {
		        switch (result.ParameterKey)
		        {
		            case "Scheme" :
                        ((DualDataComboBox)result.UnknownControl).SetData(service.GetLoyaltySchemes(PluginEntry.DataModel, siteServiceProfile, true), null);
		                break;
		        }
		    }
                
		    ((DualDataComboBox) sender).SetData(
		        service.GetCustomerMSRCards(PluginEntry.DataModel,
                                            siteServiceProfile,
                                            cmbCustomerSelectedItem != null ? new List<DataEntity>{ new DataEntity(cmbCustomerSelectedItem.ID , "")}: null,
		                                    null,
		                                    null, 
                                            null, 
                                            null, 
                                            LoyaltyMSRCardInequality.GreaterThan, 
                                            -1, 
                                            -1, 
                                            -1, 
                                            LoyaltyMSRCardSorting.CardNumber, 
                                            false, 
                                            true)
		        , null);
		 
		}

        private void OnPageScrollPageChanged(object sender, EventArgs e)
		{
            ShowProgress((sender1, e1) => LoadDataInner(false), GetLocalizedSearchingText());
		}

		private void dcbCustomer_DropDown(object sender, DropDownEventArgs e)
		{
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.Customers);
		}

		private void LoyaltyTransView_Load(object sender, EventArgs e)
		{
			lvTransactions.AutoSizeColumns();
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
            searchBar1.AddCondition(new ConditionType(Resources.Date, "Date", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.AddMonths(-1), false, DateTime.Now));
            searchBar1.AddCondition(new ConditionType(Resources.ExpirationDate, "ExpirationDate", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.AddMonths(-1), false, DateTime.Now));
            searchBar1.AddCondition(new ConditionType(Resources.ReceiptNumber, "ReceiptNumber", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Terminal, "Terminal", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.LoyaltyCard, "LoyaltyCard", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Scheme, "Scheme", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Customer, "Customer", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Types, "Types", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Expired, true, Resources.Issued, true, Resources.Used, true));
            searchBar1.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Open, true, Resources.Closed, true));

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    if (loyaltyParams != null)
                    {
                        var store = Providers.StoreData.Get(PluginEntry.DataModel, loyaltyParams.DefaultStore);
                        ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity(loyaltyParams.DefaultStore, store == null ? (string)loyaltyParams.DefaultStore : store.Text);
                    }

                    ((DualDataComboBox)args.UnknownControl).RequestData += dcbStore_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;
                    ((DualDataComboBox)args.UnknownControl).SelectedDataChanged += cmbStoreSelctionChanged;

                    break;
                case "Terminal":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    if (loyaltyParams != null)
                    {
                        var terminal = Providers.TerminalData.Get(PluginEntry.DataModel, loyaltyParams.DefaultTerminal, loyaltyParams.DefaultStore);
                        ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity(loyaltyParams.DefaultTerminal, terminal == null ? (string)loyaltyParams.DefaultTerminal : terminal.Text);
                    }

                    ((DualDataComboBox)args.UnknownControl).RequestData += dcbTerminal_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;

                    break;
                case "LoyaltyCard":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox)args.UnknownControl).DropDown += LoyaltyCard_DropDown;
                    ((DualDataComboBox)args.UnknownControl).RequestData += dcbCard_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;

                    break;
                case "Scheme":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    if (loyaltyParams != null)
                    {
                        var scheme = Providers.LoyaltySchemesData.Get(PluginEntry.DataModel, loyaltyParams.DefaultLoyaltyScheme);
                        ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity(loyaltyParams.DefaultLoyaltyScheme, scheme == null ? (string)loyaltyParams.DefaultLoyaltyScheme : scheme.Text);
                    }

                    ((DualDataComboBox)args.UnknownControl).DropDown += dcbScheme_DropDown;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;

                    break;
                case "Customer":
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("","");

                    ((DualDataComboBox)args.UnknownControl).DropDown += dcbCustomer_DropDown;
                    ((DualDataComboBox)args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;
                    ((DualDataComboBox) args.UnknownControl).SelectedDataChanged += cmbCustomerSelectionChanged;

                    break;
            }
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= dcbStore_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    ((DualDataComboBox)args.UnknownControl).SelectedDataChanged -= cmbStoreSelctionChanged;
                    break;
                case "Terminal":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= dcbTerminal_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;
                case "LoyaltyCard":
                    ((DualDataComboBox)args.UnknownControl).RequestData -= dcbCard_RequestData;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;
                case "Scheme":
                    ((DualDataComboBox)args.UnknownControl).DropDown -= dcbScheme_DropDown;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;
                case "Customer" :
                    ((DualDataComboBox)args.UnknownControl).DropDown -= dcbCustomer_DropDown;
                    ((DualDataComboBox)args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    ((DualDataComboBox)args.UnknownControl).SelectedDataChanged -= cmbCustomerSelectionChanged;
                    break;
            }
        }

        private void LoyaltyCard_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;

            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }

            var panel = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.LoyaltyCards, "", textInitallyHighlighted);
            e.ControlToEmbed = panel;
        }

        private void searchBar1_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Scheme":
                    args.HasSelection = ((DualDataComboBox)args.UnknownControl).SelectedData is DataEntitySelectionList
                                                                ? ((DataEntitySelectionList)((DualDataComboBox)args.UnknownControl).SelectedData).HasSelection
                                                                : ((DualDataComboBox)args.UnknownControl).SelectedData.ID != RecordIdentifier.Empty;
                    break;
                default:
                    args.HasSelection = true;
                    break;
            }
        }

        private void searchBar1_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                case "Terminal":
                case "LoyaltyCard":
                case "Customer":
                    args.Selection = (string)((DualDataComboBox)args.UnknownControl).SelectedData.ID;
                    break;
                case "Scheme":
                    args.Selection = ((DualDataComboBox)args.UnknownControl).SelectedData is DataEntitySelectionList
                                                                ? SearchBar.GetSelectionString(((DataEntitySelectionList)((DualDataComboBox)args.UnknownControl).SelectedData).GetSelectedItems().Cast<IDataEntity>().ToList())
                                                                : "";
                    break;
            }
        }

        private void searchBar1_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {
            DataEntity entity = null;
            string[] selectedIDs = args.Selection != "" ? args.Selection.Split(',') : null;
            switch (args.TypeKey)
            {
                case "Store":
                    entity = Providers.StoreData.Get(PluginEntry.DataModel, args.Selection);
                    break;
                case "Terminal":
                    RecordIdentifier storeID = cmbStoreSelectedItem != null ? cmbStoreSelectedItem.ID : "";
                    entity = Providers.TerminalData.Get(PluginEntry.DataModel, args.Selection, storeID);
                    break;
                case "LoyaltyCard":
                    entity = Providers.LoyaltyMSRCardData.Get(PluginEntry.DataModel, args.Selection);
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
                case "Customer":
                    entity = Providers.CustomerData.Get(PluginEntry.DataModel, args.Selection, UsageIntentEnum.Normal);
                    break;
            }
            if (args.TypeKey != "Scheme")
            {
                ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
            }
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            ShowProgress((sender1, e1) => LoadDataInner(), GetLocalizedSearchingText());
        }

        private void cmbStoreSelctionChanged(object sender, EventArgs args)
        {
            cmbStoreSelectedItem = (DataEntity)((DualDataComboBox)sender).SelectedData;
        }

	    private void cmbCustomerSelectionChanged(object sender, EventArgs args)
	    {
	        cmbCustomerSelectedItem = (DataEntity) ((DualDataComboBox) sender).SelectedData;
	    }
	}
}