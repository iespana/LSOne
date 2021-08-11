using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Terminals.Dialogs
{
    public partial class NewTerminalDialog : DialogBase
    {
        DataEntity emptyItem;
        RecordIdentifier terminalId;
        RecordIdentifier storeID;
        private bool manuallyEnterID = false;
        string storeName;
        Terminal terminal;

        public NewTerminalDialog(RecordIdentifier storeID,string storeName)
            : this()
        {
            this.storeID = storeID;
            this.storeName = storeName;
        }

        public NewTerminalDialog()
        {
            storeID = RecordIdentifier.Empty;

            terminalId = RecordIdentifier.Empty;
            InitializeComponent();

            Parameters parameters = DataProviderFactory.Instance.Get<IParameterData, Parameters>().Get(PluginEntry.DataModel);
            manuallyEnterID = parameters.ManuallyEnterTerminalID;

            tbID.Visible = manuallyEnterID;
            lblID.Visible = manuallyEnterID;

            List<DataEntity> stores = Providers.StoreData.GetList(PluginEntry.DataModel);
            if (stores.Count == 1)
            {
                cmbStore.SelectedData = stores[0];
            }
            else
            {
                cmbStore.SelectedData = new DataEntity("", "");

            }
            cmbStore.Enabled = true;

            btnAddHardwareProfile.Visible = PluginEntry.Framework.CanRunOperation("AddHardwareProfile");
            btnAddVisualProfile.Visible = PluginEntry.Framework.CanRunOperation("AddVisualProfile");
           
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (storeID != RecordIdentifier.Empty)
            {
                cmbStore.Enabled = false;
                cmbStore.SelectedData = new DataEntity(storeID,storeName);
            }
           

            base.OnLoad(e);

            emptyItem = new DataEntity(RecordIdentifier.Empty, Properties.Resources.DoNotCopyExistingTerminal);

            cmbCopyFrom.SelectedData = emptyItem;

            List<DataEntity> hardwareProfiles = Providers.HardwareProfileData.GetList(PluginEntry.DataModel);
            List<DataEntity> visualProfiles = Providers.VisualProfileData.GetList(PluginEntry.DataModel);

            cmbHardwareProfile.SelectedData = hardwareProfiles.Count == 1 ? hardwareProfiles[0] : new DataEntity("", "");
            cmbVisualProfile.SelectedData = visualProfiles.Count == 1 ? visualProfiles[0] : new DataEntity("", "");
        }

        public RecordIdentifier TerminalID
        {
            get { return terminalId; }
        }

        public RecordIdentifier StoreID
        {
            get { return storeID; }
        }
        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (Save())
            {
                DialogResult = DialogResult.OK;
                Close();
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Terminal", null, null);
                PluginEntry.Framework.SetDashboardItemDirty(new Guid("f58ece32-5f38-45ac-8c67-70b7a762fe8c")); // Inititial configuration dashboard item
                PluginOperations.ShowTerminal(terminalId, storeID);
            }
        }

        private bool Save()
        {
            if (terminal == null)
            {

                if (cmbCopyFrom.SelectedData.ID != RecordIdentifier.Empty)
                {
                    terminal = Providers.TerminalData.Get(PluginEntry.DataModel, (string)cmbCopyFrom.SelectedData.ID, ((TerminalListItem)cmbCopyFrom.SelectedData).StoreID);
                    terminal.Activated = false;
                    terminal.LastActivatedDate = new DateTime(1900, 1, 1);
                    terminal.ID = RecordIdentifier.Empty;
                }
                else
                {
                    terminal = new Terminal();

                }
            }

            if (manuallyEnterID)
            {
                if (tbID.Text.Trim() == "")
                {
                    if (QuestionDialog.Show(Properties.Resources.IDMissingQuestion, Properties.Resources.IDMissing) != System.Windows.Forms.DialogResult.Yes)
                    {
                        return false;
                    }
                }
                else
                {
                    if (!tbID.Text.IsAlphaNumeric())
                    {
                        errorProvider1.SetError(tbID, Properties.Resources.OnlyCharAndNumbers);
                        return false;
                    }
                    terminal.ID = tbID.Text.Trim();
                    terminal.StoreID = cmbStore.SelectedDataID;                    

                    if (Providers.TerminalData.Exists(PluginEntry.DataModel, terminal.ID, terminal.StoreID))
                    {
                        errorProvider1.SetError(tbID, Properties.Resources.TerminalWithinStoreExists);
                        return false;
                    }
                }
            }


            terminal.Text = tbDescription.Text;
            terminal.StoreID = cmbStore.SelectedData.ID;
            terminal.VisualProfileID = cmbVisualProfile.SelectedDataID;
            terminal.HardwareProfileID = cmbHardwareProfile.SelectedDataID;

            Providers.TerminalData.Save(PluginEntry.DataModel, terminal);

            terminalId = terminal.ID;
            storeID = terminal.StoreID;
            return true;
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> list;

            list = new List<DataEntity>(Providers.TerminalData.GetList(PluginEntry.DataModel));

            list.Insert(0, emptyItem);

            cmbCopyFrom.SetData(list,
                PluginEntry.Framework.GetImageList().Images[PluginEntry.TerminalImageIndex],true);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (terminal == null)
            {
                btnOK.Enabled = (tbDescription.Text.Length > 0) && (!cmbStore.Enabled || cmbStore.SelectedData.ID != "");
            }
            else
            {
                btnOK.Enabled = (tbDescription.Text != terminal.Text) ||
                                (cmbStore.SelectedData.Text != terminal.StoreName);
            }



            btnOK.Enabled = btnOK.Enabled && 
                            cmbHardwareProfile.SelectedData.ID != "" &&
                            cmbVisualProfile.SelectedData.ID != "";            
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), PluginEntry.Framework.GetImageList().Images[PluginEntry.StoreImageIndex]);
        }

        private void cmbVisualProfile_RequestData(object sender, EventArgs e)
        {
            cmbVisualProfile.SetData(Providers.VisualProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbHardwareProfile_RequestData(object sender, EventArgs e)
        {
            cmbHardwareProfile.SetData(Providers.HardwareProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void btnAddVisualProfile_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.RunOperation("AddVisualProfile", this, PluginOperationArguments.Empty);
        }

        private void btnAddHardwareProfile_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.RunOperation("AddHardwareProfile", this, PluginOperationArguments.Empty);
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbCopyFrom.SelectedData is TerminalListItem)
            {
                var tempTerminal = Providers.TerminalData.Get(PluginEntry.DataModel,
                    (string) cmbCopyFrom.SelectedData.ID, ((TerminalListItem) cmbCopyFrom.SelectedData).StoreID);
                cmbHardwareProfile.SelectedData = new DataEntity(tempTerminal.HardwareProfileID,
                    tempTerminal.HardwareProfileName);

                cmbVisualProfile.SelectedData = new DataEntity(tempTerminal.VisualProfileID,
                    tempTerminal.VisualProfileName);
            }
            CheckEnabled(sender,e);

        }
    }
}
