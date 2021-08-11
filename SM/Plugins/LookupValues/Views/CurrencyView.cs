using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.LookupValues.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls.Rows;
using System.Drawing;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class CurrencyView : ViewBase
    {

        WeakReference companyCurrencyEditor;
        private RecordIdentifier selectedID = "";
        
        private TabControl.Tab exchangeRateTab;
        private TabControl.Tab cashDeclarationTab;


        public CurrencyView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID;
        }

        public CurrencyView()
        {
            InitializeComponent();

            IPlugin plugin;

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            lvCurrencies.ContextMenuStrip = new ContextMenuStrip();
            lvCurrencies.ContextMenuStrip.Opening += lvCurrencyProfiles_Opening;

            plugin = PluginEntry.Framework.FindImplementor(this, "CanEditCompanyInfo", null);

            companyCurrencyEditor = (plugin != null) ? new WeakReference(plugin) : null;

            if (plugin != null)
            {
                btnEditCompanyCurrency.Visible = true;
            }

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit);
            btnsContextButtons.AddButtonEnabled = !ReadOnly;

        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("CURRENCIES", RecordIdentifier.Empty, Properties.Resources.Currencies, false));

            tabSheetTabs.GetAuditContexts(contexts);

        }

        protected override string LogicalContextName
        {
            get
            {
                return Resources.EditCurrencies;
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
            if (!isRevert)
            {
                exchangeRateTab = new TabControl.Tab(Properties.Resources.ExchangeRate, ViewPages.CurrencyExchangeRatePage.CreateInstance);
                cashDeclarationTab = new TabControl.Tab(Properties.Resources.CurrencyUnits, ViewPages.CashDenominationPage.CreateInstance);

                tabSheetTabs.AddTab(exchangeRateTab);
                tabSheetTabs.AddTab(cashDeclarationTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, null);
            }

            HeaderText = Properties.Resources.EditCurrencies;
            //HeaderIcon = Properties.Resources.currency16;

            lvCurrencies.SetSortColumn(1, true);

            LoadCurrencies();

            
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
                case "DefaultData":
                    
                case "Currencies":
                    if (changeHint == DataEntityChangeType.Add)
                    {
                        selectedID = changeIdentifier;
                    }
                    LoadCurrencies();
                    break;
            }


          
        }

        private void lvCurrencies_SelectionChanged(object sender, EventArgs e)
        {
            bool isCompanyCurrency = (lvCurrencies.Selection.Count > 0) ? (lvCurrencies.Rows[lvCurrencies.Selection.FirstSelectedRow][2].Text == Properties.Resources.Yes) : false;

            selectedID = (lvCurrencies.Selection.Count > 0) ? (RecordIdentifier)lvCurrencies.Rows[lvCurrencies.Selection.FirstSelectedRow].Tag : "";

            btnsContextButtons.EditButtonEnabled = (lvCurrencies.Selection.Count > 0) && PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit);
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled && !isCompanyCurrency;

            if (lvCurrencies.Selection.Count > 0)
            {
                if (lblNoSelection.Visible)
                {
                    tabSheetTabs.Visible = true;
                    lblNoSelection.Visible = false;
                }

                tabSheetTabs.SetData(false, selectedID, isCompanyCurrency);
                //tabSheetTabs.BroadcastChangeInformation(DataEntityChangeType.VariableChanged, "Currency", selectedID, null);
            }
            else if (!lblNoSelection.Visible)
            {
                tabSheetTabs.Visible = false;
                lblNoSelection.Visible = true;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.EditCurrency(this, (RecordIdentifier)lvCurrencies.Rows[lvCurrencies.Selection.FirstSelectedRow].Tag);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id;

            id = PluginOperations.NewCurrency();

            if (id != RecordIdentifier.Empty)
            {
                selectedID = (string)id;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteCurrency((RecordIdentifier)lvCurrencies.Rows[lvCurrencies.Selection.FirstSelectedRow].Tag);
        }

       

        void lvCurrencyProfiles_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvCurrencies.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("CurrencyList", lvCurrencies.ContextMenuStrip, lvCurrencies);

            e.Cancel = (menu.Items.Count == 0);
        }
       
        private void LoadCurrencies()
        {
            Row row;

            lvCurrencies.ClearRows();

            string companyCurrencyCode = Providers.CompanyInfoData.CompanyCurrencyCode(PluginEntry.DataModel);
            List<DataEntity> currencies = Providers.CurrencyData.GetList(PluginEntry.DataModel, CurrencySorting.Description, false);

            foreach (DataEntity currency in currencies)
            {
                row = new Row();

                row.AddText((string)currency.ID);
                row.AddText(currency.Text);
                row.AddText(currency.ID == companyCurrencyCode ? Properties.Resources.Yes : Properties.Resources.No);
                row.Tag = currency.ID;

                if (currency.ID == companyCurrencyCode)
                {
                    Style style = new Style(lvCurrencies.DefaultStyle);
                    style.Font = new Font(style.Font, FontStyle.Bold);

                    row[0].SetStyle(style);
                    row[1].SetStyle(style);
                    row[2].SetStyle(style);
                }

                lvCurrencies.AddRow(row);

                if (selectedID == currency.ID)
                {
                    lvCurrencies.Selection.Set(lvCurrencies.RowCount - 1);
                }
            }

            lvCurrencies.AutoSizeColumns();

            lvCurrencies.Sort();

            lvCurrencies_SelectionChanged(this, EventArgs.Empty);
        }
        
        
       
        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Add, ContextButtons.GetAddButtonImage(), btnAdd_Click), 10);
                }
            }

            if (arguments.CategoryKey == GetType().ToString() + ".Related")
            {
                PluginEntry.Framework.FindImplementor(this, "CanInsertDefaultData", arguments);
            }
        }

       

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

       
        private void lvCurrencies_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void btnEditCompanyCurrency_Click(object sender, EventArgs e)
        {
            ((IPlugin)companyCurrencyEditor.Target).Message(this, "EditCompanyInfo", null);
        }
    }
}
