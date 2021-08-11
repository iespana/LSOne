using System;

using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Inventory.Properties;

using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.Views
{
	public partial class StockCountingDetailView : ViewBase
	{
		InventoryAdjustment stockCounting;
		private RecordIdentifier stockCountingID;
        private bool tabsLoaded;

		IInventoryService service = null;

		public StockCountingDetailView(RecordIdentifier stockCountingID)
			: this()
		{
			this.stockCountingID = stockCountingID;            
		}

		public StockCountingDetailView()
		{
			InitializeComponent();

			Attributes = ViewAttributes.Audit |
				ViewAttributes.Close |
				ViewAttributes.ContextBar |
                ViewAttributes.Refresh |
				ViewAttributes.Help;
		}

		protected override string LogicalContextName
		{
			get
			{
				return Resources.StockCounting;
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
			service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

			stockCounting = service.GetInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), stockCountingID, false);
		
			HeaderText = Resources.StockCounting + " - " + stockCounting.Text;

            if(!tabsLoaded)
            {
                TabControl.Tab gerneralTab = new TabControl.Tab(Resources.StockCountingDetailViewSummary, ViewPages.StockCountingGeneralPage.CreateInstance);
                TabControl.Tab itemsTab = new TabControl.Tab(Resources.StockCountingDetailViewItems, ViewPages.StockCountingItemPage.CreateInstance);

                tabSheetTabs.AddTab(itemsTab);
                tabSheetTabs.AddTab(gerneralTab);
                tabsLoaded = true;
            }

			tabSheetTabs.SetData(isRevert, stockCounting.ID, stockCounting);
		}
		protected override bool SaveData()
		{

			tabSheetTabs.GetData();
		 
			service = (IInventoryService)PluginEntry.DataModel.Service(ServiceType.InventoryService);

			service.SaveInventoryAdjustment(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), stockCounting, true);
			PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit,
					 "JournalTrans",
					 stockCounting.ID,
					 null);
			return true;
		}

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();
            base.OnClose();
        }

        protected override bool DataIsModified()
		{
			return tabSheetTabs.IsModified();
		}

		public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
		{
			if(objectName == "StockCounting" && changeIdentifier == stockCounting.ID)
			{
				stockCounting = (InventoryAdjustment)param;
                
                if(IsLoaded)
                {
				    PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
                }
			}

			tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
		}
        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".Actions")
            {
                if(PluginEntry.DataModel.HasPermission(Permission.EditInventoryAdjustments) && PluginEntry.DataModel.HasPermission(Permission.ResetJournalProcessingStatus))
                {
                    arguments.Add(new ContextBarItem(Resources.ResetProcessingStatus, null, stockCounting.ProcessingStatus != InventoryProcessingStatus.None, ResetProcessingStatus), 100);
                }                               
            }

            if (arguments.CategoryKey == GetType().ToString() + ".Related")
            {
                if (PluginEntry.Framework.CanRunOperation("ShowImageBank"))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.ImageBank, PluginOperations.ShowImageBankHandler), 100);
                }
            }

            base.OnSetupContextBarItems(arguments);
        }

        protected override void OnSetupContextBarHeaders(ContextBarHeaderConstructionArguments arguments)
        {
            arguments.Add(new ContextBarHeader(Resources.Action, GetType().ToString() + ".Actions"), 200);
            base.OnSetupContextBarHeaders(arguments);
        }

        private void ResetProcessingStatus(object sender, EventArgs e)
        {
            PluginOperations.ResetProcessingStatus(stockCounting.ID, stockCounting.ProcessingStatus);
        }
    }
}
