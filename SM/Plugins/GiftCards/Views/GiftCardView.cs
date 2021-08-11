using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.DataProviders.Vouchers;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.GiftCards.Views
{
	public partial class GiftCardView : ViewBase
	{
        GiftCard giftCard;
        RecordIdentifier giftCardID;

        IEnumerable<IDataEntity> recordBrowsingContext;

	    private SiteServiceProfile siteServiceProfile;
        private IPlugin storeViewer;
        private IPlugin terminalViewer;
        private IPlugin userViewer;

        public GiftCardView(RecordIdentifier giftCardID, IEnumerable<IDataEntity> recordBrowsingContext)
            : this()
	    {
            this.giftCardID = giftCardID;

            this.recordBrowsingContext = recordBrowsingContext;
	    }       

		public GiftCardView()
		{
            ViewAttributes attr;

            giftCardID = RecordIdentifier.Empty;

			InitializeComponent();

            attr = ViewAttributes.Delete |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help |
                ViewAttributes.RecordCursor;

            if (PluginEntry.DataModel.CurrentStoreID == RecordIdentifier.Empty)
            {
                attr |= ViewAttributes.Audit;
            }

            Attributes = attr;

            //HeaderIcon = Properties.Resources.GiftCard16;
            HeaderText = Properties.Resources.GiftCard;

            lvUsageLog.ContextMenuStrip = new ContextMenuStrip();
            lvUsageLog.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvUsageLog.SmallImageList = PluginEntry.Framework.GetImageList();

            var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
            storeViewer = PluginEntry.Framework.FindImplementor(this, "CanEditStore", null);
            terminalViewer = PluginEntry.Framework.FindImplementor(this, "ViewTerminal", null);
            userViewer = PluginEntry.Framework.FindImplementor(this, "ViewUser", null);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("GiftCard", giftCardID, Properties.Resources.GiftCard, true));
            contexts.Add(new AuditDescriptor("GiftCardLine", giftCardID, Properties.Resources.Usage, false));
        }

        protected override ViewBase GetRecordCursorView(int cursorIndex)
        {
            if (recordBrowsingContext != null)
            {
                RecordIdentifier id = recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID;

                if (Providers.GiftCardData.Exists(PluginEntry.DataModel, id))
                {
                    return new GiftCardView(recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID, recordBrowsingContext);
                }
            }
            return null;
        }

        protected override void GetRecordCursorInfo(ContextBarCursorEventArguments args)
        {
            if (recordBrowsingContext != null)
            {
                args.Position = 0;
                args.Count = recordBrowsingContext.Count<IDataEntity>();

                foreach (IDataEntity entity in recordBrowsingContext)
                {
                    if (entity.ID == giftCardID)
                    {
                        return;
                    }

                    args.Position++;
                }
            }
            else
            {
                args.Count = 1;
                args.Position = 0;
            }
        }

   		protected override string LogicalContextName
		{
			get
			{
				return Properties.Resources.GiftCard;
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
	        // We are not on head office so we need to talk to the Store Server
	        ISiteServiceService service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

	        try
	        {
	            giftCard = service.GetGiftCard(PluginEntry.DataModel, siteServiceProfile, giftCardID, true);
	        }
	        catch (Exception ex)
	        {
	            MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
	        }

	        if (giftCard != null)
	        {
	            tbID.Text = (string) giftCard.ID;
	            ntbBallance.SetValueWithLimit(giftCard.Balance, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
	            tbCurrency.Text = (string) giftCard.Currency;
	            chkActive.Checked = giftCard.Active;
	            chkRefillable.Checked = giftCard.Refillable;


	            LoadItems();
	        }
	    }

	    public override void DisplayComplete()
        {
            if (giftCard == null)
            {
                PluginEntry.Framework.ViewController.DiscardCurrentView(this);
            }
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
                    if(changeHint == DataEntityChangeType.Delete && changeIdentifier == giftCardID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if(changeHint==DataEntityChangeType.MultiDelete && ((List<DataEntity>)param).Find(f => f.ID == giftCardID) != null)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if (changeHint == DataEntityChangeType.VariableChanged && changeIdentifier == giftCardID)
                    {
                        if((string)((object[])param)[0] == "Active")
                        {
                            giftCard.Active = (bool)((object[])param)[1];
                            chkActive.Checked = giftCard.Active;

                            LoadItems();
                            PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
                        }
                        else if((string)((object[])param)[0] == "Amount")
                        {
                            giftCard.Balance = (decimal)((object[])param)[1];
                            ntbBallance.SetValueWithLimit(giftCard.Balance, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                            LoadItems();
                        }
                    }
                    break;

            }      
        }

	    private void LoadItems()
	    {
	        List<GiftCardLine> lines = null;
	        ListViewItem listItem;
	        DecimalLimit limiter;

	        lvUsageLog.Items.Clear();
	        // We are not on head office so we need to use the Store server
            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

	        try
	        {
	            lines = service.GetGiftCardLines(PluginEntry.DataModel, siteServiceProfile, giftCardID, true);
	        }
	        catch (Exception ex)
	        {
	            MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
	            return;
	        }

	        lines = SortLines(lines);
	        limiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
	        string name = "";
	        foreach (GiftCardLine line in lines)
	        {
	            name = "";
	            listItem = new ListViewItem(line.StoreName);
	            listItem.SubItems.Add(line.TerminalName);
	            listItem.SubItems.Add((string) line.ReceiptID);
	            if (line.UserID != Guid.Empty)
	            {
                    var user = Providers.UserData.Get(PluginEntry.DataModel, (Guid)line.UserID);
                    if (user != null)
                    {
                        name = PluginEntry.DataModel.Settings.NameFormatter.Format(user.Name);
                    }
	            }
	            listItem.SubItems.Add(name);

	            listItem.SubItems.Add(line.TransactionDateTime.ToShortDateString() + " - " + line.TransactionDateTime.ToShortTimeString());
	            listItem.SubItems.Add(line.Amount.FormatWithLimits(limiter));
	            listItem.SubItems.Add(line.OperationText);
	            listItem.Tag = line;

	            lvUsageLog.Add(listItem);
	        }

            lvUsageLog_SelectedIndexChanged(this, EventArgs.Empty);
	        lvUsageLog.BestFitColumns();

	    }

	    private List<GiftCardLine> SortLines(List<GiftCardLine> lines)
        {
            List<GiftCardLine> sortedLines = new List<GiftCardLine>();

            switch (lvUsageLog.SortColumn)
            {
                case 0:
                    sortedLines = (from l in lines
                             orderby l.StoreName
                             select l).ToList();
                    break;
                case 1:
                    sortedLines = (from l in lines
                            orderby l.TerminalName
                            select l).ToList();
                    break;
                case 2:
                    sortedLines = (from l in lines
                            orderby (string)l.TransactionID
                            select l).ToList();
                    break;
                case 3:
                    sortedLines = (from l in lines
                            orderby l.StaffName
                            select l).ToList();
                    break;
                case 4:
                    sortedLines = (from l in lines
                            orderby l.TransactionDateTime
                            select l).ToList();
                    break;
                case 5:
                    sortedLines = (from l in lines
                            orderby l.Amount
                            select l).ToList();
                    break;
                case 6:
                    sortedLines = (from l in lines
                            orderby l.OperationText
                            select l).ToList();
                    break;
                default:
                    return null;
            }

            if (lvUsageLog.SortedBackwards)
            {
                sortedLines.Reverse();
            }

            return sortedLines;
        }

	    private void Deactivate(object sender, EventArgs args)
	    {
	        bool success = false;
	        // We are not on head office so we need to talk to the Store Server
            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
	        try
	        {
	            success = service.DeactivateGiftCard(PluginEntry.DataModel, siteServiceProfile, giftCard.ID, true);
	        }
	        catch (Exception ex)
	        {
	            MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
	            return;
	        }

	        if (success)
	        {
	            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "GiftCard", giftCardID, new object[] {"Active", false});
	        }
	    }

	    private void Activate(object sender, EventArgs args)
	    {
	        bool success = false;
	        // We are not on head office so we need to talk to the Store Server
            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
	        try
	        {
	            success = service.ActivateGiftCard(PluginEntry.DataModel, siteServiceProfile, giftCard.ID, "", "", true);
	        }
	        catch (Exception ex)
	        {
	            MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
	            return;
	        }

	        if (success)
	        {
	            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "GiftCard", giftCardID, new object[] {"Active", true});
	        }
	    }

	    private void AddToGiftCard(object sender,EventArgs args)
        {
            PluginOperations.AddToGiftCard(giftCard.ID, siteServiceProfile);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (giftCard != null)
            {
                if (arguments.CategoryKey == GetType() + ".View")
                {
                    arguments.Add(giftCard.Active ? new ContextBarItem(Properties.Resources.Deactivate, Deactivate) : new ContextBarItem(Properties.Resources.Activate, Activate), 400);

                    if(giftCard.Refillable)
                    {
                    arguments.Add(new ContextBarItem(Properties.Resources.AddToGiftCard, AddToGiftCard), 410);
                    }
                }
                else if (arguments.CategoryKey == GetType() + ".Related")
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.ViewGiftCards, PluginOperations.ShowGiftCardsView), 400);
                }
            }
        }

        protected override void OnClose()
        {
            lvUsageLog.SmallImageList = null;

            base.OnClose();
        }

        private void ViewStore(object sender, EventArgs args)
        {
            storeViewer.Message(this, "EditStore", ((GiftCardLine)(lvUsageLog.SelectedItems[0].Tag)).StoreID);
        }

        private void ViewTerminal(object sender, EventArgs args)
        {
            RecordIdentifier terminalID = new RecordIdentifier(((GiftCardLine)(lvUsageLog.SelectedItems[0].Tag)).TerminalID, ((GiftCardLine)(lvUsageLog.SelectedItems[0].Tag)).StoreID);
            terminalViewer.Message(this, "ViewTerminal", terminalID);
        }

        private void ViewUser(object sender, EventArgs args)
        {
            userViewer.Message(this,"ViewUser",((GiftCardLine)(lvUsageLog.SelectedItems[0].Tag)).UserID);
        }
        
        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvUsageLog.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(Properties.Resources.ViewStore, 100, ViewStore)
            {
                Enabled = btnViewStore.Enabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(Properties.Resources.ViewTerminal, 200, ViewTerminal)
            {
                Enabled = btnViewTerminal.Enabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(Properties.Resources.ViewUser, 300, ViewUser)
            {
                Enabled = btnViewUser.Enabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("GiftCardUsageLog", lvUsageLog.ContextMenuStrip, lvUsageLog);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteGiftCard(giftCardID, siteServiceProfile);
            ManualClose();
        }

        private void lvUsageLog_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == lvUsageLog.SortColumn)
            {
                lvUsageLog.SortedBackwards = !lvUsageLog.SortedBackwards;
                lvUsageLog.Columns[e.Column].ImageIndex = (lvUsageLog.SortedBackwards) ? 1 : 0;
            }
            else
            {
                lvUsageLog.Columns[lvUsageLog.SortColumn].ImageIndex = 2;
                lvUsageLog.Columns[e.Column].ImageIndex = 0;
                lvUsageLog.SortColumn = e.Column;
                lvUsageLog.SortedBackwards = false;
            }
            LoadItems();
        }

        private void lvUsageLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool selected = lvUsageLog.SelectedItems.Count > 0;

            btnViewStore.Enabled = selected && storeViewer != null && ((GiftCardLine)(lvUsageLog.SelectedItems[0].Tag)).StoreID != "";
            btnViewTerminal.Enabled = selected && terminalViewer != null && ((GiftCardLine)(lvUsageLog.SelectedItems[0].Tag)).TerminalID != "";
            btnViewUser.Enabled = selected && userViewer != null && ((GiftCardLine)(lvUsageLog.SelectedItems[0].Tag)).UserID != Guid.Empty;
        }
    }
}
