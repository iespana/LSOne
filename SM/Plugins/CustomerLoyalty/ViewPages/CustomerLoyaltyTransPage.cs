using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
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
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.CustomerLoyalty.Properties;
using ContainerControl = LSOne.Controls.ContainerControl;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.CustomerLoyalty.ViewPages
{
    public partial class CustomerLoyaltyTransPage : ContainerControl, ITabView
	{
		private Customer customer;
		private LoyaltyCustomerParams loyaltyParams;
		private List<LoyaltyMSRCardTrans> loyaltyTrans;

	    private Setting searchBarSetting;
	    private const string SettingsGuid = "EC076F1F-6B50-4B75-B97D-1D61F225C312";

	    private Setting sortSetting;
	    private const string SortSettingGuid = "A56C0201-25A5-4AF7-B229-6B4EC2A9E874";

	    private SiteServiceProfile siteServiceProfile;

		public CustomerLoyaltyTransPage()
		{
			InitializeComponent();
		    searchBar1.BuddyControl = lvTransactions;

		    DoubleBuffered = true;

			loyaltyParams = Providers.LoyaltyCustomerParamsData.Get(PluginEntry.DataModel);
		
            itemDataScroll.PageSize = PluginEntry.DataModel.PageSize;

            var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
        }

		private void LoadDataInner(bool reset = true)
		{
		    if (reset)
		    {
                itemDataScroll.Reset();
		    }

		    Date fromDate = Date.Empty;
		    Date toDate = Date.Empty;

		    string selectedStore = "";
		    string selectedTerminal = "";
		    string selectedCard = "";
		    string selectedScheme = "";
		    string receipt = "";
		    int type = -1;
		    int open = -1;

		    List<SearchParameterResult> results = searchBar1.SearchParameterResults;

            foreach (SearchParameterResult result in results)
		    {
		        switch (result.ParameterKey)
		        {
                    case "Store" :
		                selectedStore = ((DualDataComboBox) result.UnknownControl).SelectedData != null ? (string)((DualDataComboBox) result.UnknownControl).SelectedData.ID: null;
		                break;
                    case "Terminal" :
                        selectedTerminal = ((DualDataComboBox)result.UnknownControl).SelectedData != null ? (string)((DualDataComboBox)result.UnknownControl).SelectedData.ID : null;
		                break;
                    case "LoyaltyCard" :
                        selectedCard = ((DualDataComboBox)result.UnknownControl).SelectedData != null ? (string)((DualDataComboBox)result.UnknownControl).SelectedData.ID : null;
		                break;
                    case "Scheme" :
                        selectedScheme = ((DualDataComboBox)result.UnknownControl).SelectedData != null ? (string)((DualDataComboBox)result.UnknownControl).SelectedData.ID : null;
		                break;
                    case "ReceiptNumber" :
                        receipt = result.SearchModification == SearchParameterResult.SearchModificationEnum.Contains ? "%" : "";
		                receipt += result.StringValue;
		                break;
                    case "Date" :
                        fromDate = new Date(!result.Date.Checked, result.Date.Value).GetDateWithoutTime();
                        toDate = new Date(!result.DateTo.Checked, result.DateTo.Value.AddDays(1)).GetDateWithoutTime();
		                break;
                    case "Status" :
                        open = result.CheckedValues[1] && !result.CheckedValues[0] ? 0 : result.CheckedValues[0] && !result.CheckedValues[1] ? 1 : -1;
		                break;
                    case "Types" :
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
		
			try
			{
				var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
				loyaltyTrans = service.GetLoyaltyTrans(PluginEntry.DataModel,
                    siteServiceProfile,
                    selectedStore,
					selectedTerminal,
					selectedCard,
					selectedScheme,
					type,
                    open,
					(int)LoyaltyMSRCardTrans.EntryTypeEnum.None,
					(string)customer.ID,
					receipt,
					fromDate,
					toDate,
					Date.Empty,
					Date.Empty,
					itemDataScroll.StartRecord, itemDataScroll.EndRecord + 1, false, true);
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
				foreach (LoyaltyMSRCardTrans trans in loyaltyTrans)
				{
					Row row = new Row();
					row.AddText(trans.TypeAsString);
					row.AddText((string)trans.ReceiptID);
					row.AddText(trans.Points.FormatWithLimits(qtyLimit));
					row.AddText(trans.RemainingPoints.FormatWithLimits(qtyLimit));
					//row.AddCell(new CheckBoxCell(trans.Open, false));
					row.AddText(trans.StatusAsString);
					row.AddText(trans.CreatedDate.ToShortDateString());
					row.AddText(trans.DateOfIssue.ToShortDateString());
					row.AddText(trans.StoreName);
					row.AddText(trans.TerminalName);
					row.AddText(trans.CardNumber);
					row.AddText(trans.SchemeDescription);
					row.AddText(trans.ExpirationDate.ToShortDateString());
					row.AddText((string)trans.StaffID);
					lvTransactions.AddRow(row);
				}
			}
			lvTransactions.AutoSizeColumns();
            ((ViewBase)Parent.Parent.Parent).HideProgress();
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
			return new CustomerLoyaltyTransPage();
		}

		#region ITabPanel Members

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			customer = (Customer)internalContext;
            sortSetting = PluginEntry.DataModel.Settings.GetSetting(
                PluginEntry.DataModel,
                new Guid(SortSettingGuid),
                SettingType.UISetting, lvTransactions.CreateSortSetting(0, true));

            lvTransactions.SortSetting = sortSetting.Value;
			LoadDataInner();
		}

		public bool DataIsModified()
		{
		    return false;
		}

		public bool SaveData()
		{
			return true;
		}

		public void GetAuditDescriptors(List<AuditDescriptor> contexts)
		{
		}

		public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
		{
		}

		public void OnClose()
		{
		}

        public void SaveUserInterface()
        {
            string newSortSetting = lvTransactions.SortSetting;

            if (newSortSetting != sortSetting.Value)
            {
                sortSetting.Value = newSortSetting;
                sortSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, new Guid(SortSettingGuid), SettingsLevel.User, sortSetting);
            }
        }

		#endregion

		private void DualDataComboBox_RequestClear(object sender, EventArgs e)
		{
			((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
		}

		private void dcbStore_RequestData(object sender, EventArgs e)
		{
            ((DualDataComboBox)sender).SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
		}

		private void dcbTerminal_RequestData(object sender, EventArgs e)
		{
            RecordIdentifier storeID = GetSelectedStore();
			if (!RecordIdentifier.IsEmptyOrNull(storeID))
			{
                ((DualDataComboBox)sender).SetData(Providers.TerminalData.GetList(PluginEntry.DataModel, storeID), null);
			}
			else
			{
                ((DualDataComboBox)sender).SetData(Providers.TerminalData.GetList(PluginEntry.DataModel), null);
			}
		}

		private void dcbCard_RequestData(object sender, EventArgs e)
		{
            ((DualDataComboBox)sender).SetData(Providers.LoyaltyMSRCardData.GetList(PluginEntry.DataModel, 
                                                                          new List<DataEntity>{new DataEntity(customer.ID, "")}, 
                                                                          null, 
                                                                          null,
                                                                          null, 
                                                                          -1, 
                                                                          null, 
                                                                          LoyaltyMSRCardInequality.GreaterThan, 
                                                                          -1, 
                                                                          -1, 
                                                                          LoyaltyMSRCardSorting.CardNumber, 
                                                                          false),
				null);
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

		private void OnPageScrollPageChanged(object sender, EventArgs e)
		{
			LoadDataInner(false);
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
            ((ViewBase)Parent.Parent.Parent).ShowTimedProgress(searchBar1.GetLocalizedSavingText());
        }

        private void searchBar1_SearchClicked(object sender, EventArgs e)
        {
            ((ViewBase)Parent.Parent.Parent).ShowProgress((sender1, e1) => LoadDataInner(), ((ViewBase)Parent.Parent.Parent).GetLocalizedSearchingText());
        }

        private void searchBar1_SetupConditions(object sender, EventArgs e)
        {
            searchBar1.AddCondition(new ConditionType(Resources.Date, "Date", ConditionType.ConditionTypeEnum.DateRange, false, DateTime.Now.AddMonths(-1), false, DateTime.Now));
            searchBar1.AddCondition(new ConditionType(Resources.ReceiptNumber, "ReceiptNumber", ConditionType.ConditionTypeEnum.Text));
            searchBar1.AddCondition(new ConditionType(Resources.Store, "Store", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Terminal, "Terminal", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.LoyaltyCard, "LoyaltyCard", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Scheme, "Scheme", ConditionType.ConditionTypeEnum.Unknown));
            searchBar1.AddCondition(new ConditionType(Resources.Types, "Types", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Expired, true, Resources.Issued, true, Resources.Used, true));
            searchBar1.AddCondition(new ConditionType(Resources.Status, "Status", ConditionType.ConditionTypeEnum.Checkboxes, Resources.Open, true, Resources.Closed, true));

            searchBar1_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar1_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store" :
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;

                    if(loyaltyParams != null)
                    {
                        var store = Providers.StoreData.Get(PluginEntry.DataModel, loyaltyParams.DefaultStore);
                        ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity(loyaltyParams.DefaultStore, store == null ? (string)loyaltyParams.DefaultStore : store.Text);
                    }

                    ((DualDataComboBox) args.UnknownControl).RequestData += dcbStore_RequestData;
                    ((DualDataComboBox) args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;

                    break;
                case "Terminal" :
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;

                    if (loyaltyParams != null)
                    {
                        var terminal = Providers.TerminalData.Get(PluginEntry.DataModel, loyaltyParams.DefaultTerminal, loyaltyParams.DefaultStore);
                        ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity(loyaltyParams.DefaultTerminal, terminal == null ? (string)loyaltyParams.DefaultTerminal : terminal.Text);
                    }

                    ((DualDataComboBox) args.UnknownControl).RequestData += dcbTerminal_RequestData;
                    ((DualDataComboBox) args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;

                    break;
                case "LoyaltyCard" :
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox) args.UnknownControl).ShowDropDownOnTyping = true;

                    ((DualDataComboBox)args.UnknownControl).SelectedData = new DataEntity("", "");

                    ((DualDataComboBox) args.UnknownControl).RequestData += dcbCard_RequestData;
                    ((DualDataComboBox) args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;
                    break;
                case "Scheme" :
                    args.UnknownControl = new DualDataComboBox();
                    args.UnknownControl.Size = new Size(200, 21);
                    args.MaxSize = 200;
                    args.AutoSize = false;
                    ((DualDataComboBox)args.UnknownControl).ShowDropDownOnTyping = true;

                    if (loyaltyParams != null)
                    {
                        var scheme = Providers.LoyaltySchemesData.Get(PluginEntry.DataModel, loyaltyParams.DefaultLoyaltyScheme);
                        ((DualDataComboBox) args.UnknownControl).SelectedData = new DataEntity(loyaltyParams.DefaultLoyaltyScheme, scheme == null ? (string)loyaltyParams.DefaultLoyaltyScheme : scheme.Text);
                    }

                    ((DualDataComboBox) args.UnknownControl).DropDown += dcbScheme_DropDown;
                    ((DualDataComboBox) args.UnknownControl).RequestClear += DualDataComboBox_RequestClear;
                    break;
            }
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
                    entity = Providers.TerminalData.Get(PluginEntry.DataModel, args.Selection, RecordIdentifier.Empty);
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
            }
            if (args.TypeKey != "Scheme")
            {
                ((DualDataComboBox)args.UnknownControl).SelectedData = entity ?? new DataEntity("", "");
            }
        }

        private void searchBar1_UnknownControlRemove(object sender, UnknownControlArguments args)
        {
            switch (args.TypeKey)
            {
                case "Store":
                    ((DualDataComboBox) args.UnknownControl).RequestData -= dcbStore_RequestData;
                    ((DualDataComboBox) args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;
                case "Terminal" :
                    ((DualDataComboBox) args.UnknownControl).RequestData -= dcbTerminal_RequestData;
                    ((DualDataComboBox) args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;
                case "LoyaltyCard" :
                    ((DualDataComboBox) args.UnknownControl).RequestData -= dcbCard_RequestData;
                    ((DualDataComboBox) args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;
                case "Scheme" :
                    ((DualDataComboBox) args.UnknownControl).DropDown -= dcbScheme_DropDown;
                    ((DualDataComboBox) args.UnknownControl).RequestClear -= DualDataComboBox_RequestClear;
                    break;
            }
        }

	    private RecordIdentifier GetSelectedStore()
	    {
            SearchParameterResult searchBand = searchBar1.SearchParameterResults.SingleOrDefault(x => x.ParameterKey == "Store");
            return searchBand == null ? RecordIdentifier.Empty : ((DualDataComboBox)searchBand.UnknownControl).SelectedData.ID;
        }

	}
}