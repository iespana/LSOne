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
using LSOne.DataLayer.DataProviders.Vouchers;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CreditVouchers.Views
{
	public partial class CreditVoucherView : ViewBase
	{
        CreditVoucher creditVoucher;
        RecordIdentifier creditVoucherID;

        IEnumerable<IDataEntity> recordBrowsingContext;

	    private SiteServiceProfile siteServiceProfile;
        private IPlugin storeViewer;
        private IPlugin terminalViewer;
        private IPlugin userViewer;

        public CreditVoucherView(RecordIdentifier creditVoucherID, IEnumerable<IDataEntity> recordBrowsingContext)
            : this()
	    {
            this.creditVoucherID = creditVoucherID;

            this.recordBrowsingContext = recordBrowsingContext;
	    }       

		public CreditVoucherView()
		{
            ViewAttributes attr;

            creditVoucherID = RecordIdentifier.Empty;

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
            HeaderText = Properties.Resources.CreditMemo;

            lvUsageLog.ContextMenuStrip = new ContextMenuStrip();
            lvUsageLog.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            lvUsageLog.SortColumn = 1;
            lvUsageLog.SortedBackwards = false;

            var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            storeViewer = PluginEntry.Framework.FindImplementor(this, "CanEditStore", null);
            terminalViewer = PluginEntry.Framework.FindImplementor(this, "ViewTerminal", null);
            userViewer = PluginEntry.Framework.FindImplementor(this, "ViewUser", null);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("CreditVoucher", creditVoucherID, Properties.Resources.CreditMemo, true));
            contexts.Add(new AuditDescriptor("CreditVoucherLine", creditVoucherID, Properties.Resources.Usage, false));
        }

        protected override ViewBase GetRecordCursorView(int cursorIndex)
        {
            if (recordBrowsingContext != null)
            {
                RecordIdentifier id = recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID;

                if (Providers.CreditVoucherData.Exists(PluginEntry.DataModel, id))
                {
                    return new CreditVoucherView(recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID, recordBrowsingContext);
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
                    if (entity.ID == creditVoucherID)
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
				return Properties.Resources.CreditMemo;
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
            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

	        try
	        {
	            creditVoucher = service.GetCreditVoucher(PluginEntry.DataModel, siteServiceProfile, creditVoucherID, true);
	        }
	        catch (Exception ex)
	        {
	            MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
	        }

	        if (creditVoucher != null)
	        {
	            tbID.Text = (string) creditVoucher.ID;
	            ntbBallance.SetValueWithLimit(creditVoucher.Balance, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
	            tbCurrency.Text = (string) creditVoucher.Currency;

	            LoadItems();
	        }
	    }

	    public override void DisplayComplete()
        {
            if (creditVoucher == null)
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
                case "CreditVoucher":
                    if(changeHint == DataEntityChangeType.Delete && changeIdentifier == creditVoucherID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if(changeHint==DataEntityChangeType.MultiDelete && ((List<RecordIdentifier>)param).Contains(creditVoucherID))
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    else if (changeHint == DataEntityChangeType.VariableChanged && changeIdentifier == creditVoucherID)
                    {
                        if((string)((object[])param)[0] == "Amount")
                        {
                            creditVoucher.Balance = (decimal)((object[])param)[1];
                            ntbBallance.SetValueWithLimit(creditVoucher.Balance, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                            LoadItems();
                        }
                    }
                    break;
            }           
        }

        private void AddToCreditVoucher(object sender, EventArgs args)
        {
            PluginOperations.AddToCreditVoucher(creditVoucher.ID, siteServiceProfile);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (creditVoucher != null)
            {
                /*if (arguments.CategoryKey == GetType().ToString() + ".View")
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.AddToCreditVoucher, AddToCreditVoucher), 410);
                }
                else*/ if (arguments.CategoryKey == GetType().ToString() + ".Related")
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.ViewCreditMemos, PluginOperations.ShowCreditVouchersView), 400);
                }
            }
        }

	    private void LoadItems()
	    {
	        List<CreditVoucherLine> lines = null;

	        lvUsageLog.Items.Clear();

	        // We are not on head office so we need to use the Store server
            ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

	        try
	        {
	            lines = service.GetCreditVoucherLines(PluginEntry.DataModel, siteServiceProfile, creditVoucherID, true);
	        }
	        catch (Exception ex)
	        {
	            MessageDialog.Show((service != null) ? service.GetExceptionDisplayText(ex) : ex.Message);
	            return;
	        }

	        DecimalLimit limiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            string name = "";
            foreach (CreditVoucherLine line in lines)
	        {
                name = "";
	            ListViewItem listItem = new ListViewItem(line.StoreName);
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

	    protected override void OnClose()
        {
            lvUsageLog.SmallImageList = null;

            base.OnClose();
        }

        private void ViewStore(object sender, EventArgs args)
        {
            storeViewer.Message(this, "EditStore", ((CreditVoucherLine)(lvUsageLog.SelectedItems[0].Tag)).StoreID);
        }

        private void ViewTerminal(object sender, EventArgs args)
        {
            RecordIdentifier terminalID = new RecordIdentifier(((CreditVoucherLine)(lvUsageLog.SelectedItems[0].Tag)).TerminalID, ((CreditVoucherLine)(lvUsageLog.SelectedItems[0].Tag)).StoreID);
            terminalViewer.Message(this, "ViewTerminal", terminalID);
        }

        private void ViewUser(object sender, EventArgs args)
        {
            userViewer.Message(this, "ViewUser", ((CreditVoucherLine)(lvUsageLog.SelectedItems[0].Tag)).UserID);
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

            PluginEntry.Framework.ContextMenuNotify("CreditVoucherUsageLog", lvUsageLog.ContextMenuStrip, lvUsageLog);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteCreditVoucher(creditVoucherID, siteServiceProfile);
        }

        private void lvUsageLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool selected = lvUsageLog.SelectedItems.Count > 0;

            btnViewStore.Enabled = selected && storeViewer != null && ((CreditVoucherLine)(lvUsageLog.SelectedItems[0].Tag)).StoreID != "";
            btnViewTerminal.Enabled = selected && terminalViewer != null && ((CreditVoucherLine)(lvUsageLog.SelectedItems[0].Tag)).TerminalID != "";
            btnViewUser.Enabled = selected && userViewer != null && ((CreditVoucherLine)(lvUsageLog.SelectedItems[0].Tag)).UserID != Guid.Empty;
        }
    }
}
