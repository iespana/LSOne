using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    public partial class ReceiptListDialog : TouchBaseForm
    {
        public RecordIdentifier SelectedStoreID { get; set; }
        public RecordIdentifier SelectedTerminalID { get; set; }
        
        private List<DataEntity> stores;
        private List<DataEntity> terminals;
        private IsDisplayed isDisplayed;
        private List<ReceiptListItem> receiptList;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        private enum IsDisplayed
        {
            Stores,
            Terminals
        }
        
        public ReceiptListDialog(IConnectionManager entry, List<ReceiptListItem> list)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            receiptList = list;
            stores = new List<DataEntity>();
            terminals = new List<DataEntity>();
            SelectedStoreID = new RecordIdentifier(RecordIdentifier.Empty);
            SelectedTerminalID = new RecordIdentifier(RecordIdentifier.Empty);

            lblReceiptID.Text = receiptList[0].ID.StringValue;
            CreateLists();

            if(stores.Count == 1)
            {
                SelectedStoreID = stores[0].ID;
                LoadTerminalButtons();
                lblStore.Text = stores[0].Text;
                lblStore.Visible = true;
            }
            else
            {
                LoadStoreButtons();
                isDisplayed = IsDisplayed.Stores;
            }
        }

        private void CreateLists()
        {
            List<DataEntity> storeDescriptions = new List<DataEntity>();
            List<TerminalListItem> terminalDescriptions = new List<TerminalListItem>();

            if (receiptList.Count > 0)
            {
                storeDescriptions = Providers.StoreData.GetList(dlgEntry);
                terminalDescriptions = Providers.TerminalData.GetList(dlgEntry);
            }

            foreach (ReceiptListItem rli in receiptList)
            {
                if (stores.Count(c => c.ID == rli.StoreID) == 0)
                {
                    DataEntity descr = storeDescriptions.FirstOrDefault(f => f.ID == rli.StoreID);
                    DataEntity store = new DataEntity(rli.StoreID, descr == null ? (string)rli.StoreID : descr.Text);
                    stores.Add(store);
                }

                if (terminals.Count(c => c.ID == rli.TerminalID) == 0)
                {
                    TerminalListItem termDescr = terminalDescriptions.FirstOrDefault(f => f.ID == rli.TerminalID);
                    DataEntity terminal = new DataEntity(rli.TerminalID, termDescr == null ? (string)rli.TerminalID : termDescr.Text);
                    terminals.Add(terminal);
                }
            }
        }

        private void LoadStoreButtons()
        {
            panel.Clear();
            foreach (DataEntity de in stores)
            {
                panel.AddButton(de.Text + " (" + de.ID + ")", de.ID, (string)de.ID);
            }
        }

        private void LoadTerminalButtons()
        {
            panel.Clear();
            foreach (DataEntity de in terminals)
            {
                Terminal terminal = Providers.TerminalData.Get(dlgEntry, de.ID, SelectedStoreID, CacheType.CacheTypeApplicationLifeTime);

                if (terminal != null)
                {
                    panel.AddButton(de.Text + " (" + de.ID + ")", de.ID, (string)de.ID);
                }
            }
            isDisplayed = IsDisplayed.Terminals; 
        }

        private void panel_Click(object sender, Controls.SupportClasses.ScrollButtonEventArguments args)
        {
            if (isDisplayed == IsDisplayed.Stores)
            {
                SelectedStoreID = (RecordIdentifier)args.Tag;

                if (receiptList.Count(c => c.StoreID == SelectedStoreID) == 1)
                {
                    ReceiptListItem rli = receiptList.FirstOrDefault(f => f.StoreID == SelectedStoreID);
                    if (rli != null)
                    {
                        SelectedTerminalID = rli.TerminalID;
                        this.DialogResult = DialogResult.OK;
                        Close();
                        return;
                    }
                }

                lblStore.Text = stores.Find(x => x.ID == SelectedStoreID).Text;
                lblStore.Visible = true;

                LoadTerminalButtons();
                return;
            }

            SelectedTerminalID = (RecordIdentifier)args.Tag;
            this.DialogResult = DialogResult.OK;
            Close();
        }
    }
}
