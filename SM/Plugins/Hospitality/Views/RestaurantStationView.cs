using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.Hospitality.Dialogs;
using LSRetail.PrintingStationClient;

namespace LSOne.ViewPlugins.Hospitality.Views
{
    public partial class RestaurantStationView : ViewBase
    {
        private RecordIdentifier restaurantStationID = "";
        private PrintingStation printingStation;

        public RestaurantStationView(RecordIdentifier restaurantStationID) : this()
        {
            this.restaurantStationID = restaurantStationID;
            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting);
        }

        private RestaurantStationView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close;

            cmbRemoteHost.Visible = PluginEntry.Framework.CanRunOperation("EditPrintingStations");
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("RestaurantStation", restaurantStationID, Properties.Resources.RestaurantStationText, true));
        }

        public string Description
        {
            get
            {
                return Properties.Resources.PrintingStation + ": " + restaurantStationID + " - " + tbStationName.Text;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return tbStationName.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.PrintingStation;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return restaurantStationID;
            }
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteRestaurantStation(restaurantStationID);
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {                
                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, restaurantStationID);
            }

            printingStation = Providers.PrintingStationData.Get(PluginEntry.DataModel, restaurantStationID);

            StationPrintingHost host = new StationPrintingHost();
            if (printingStation.StationPrintingHostID != "")
            {
                host = Providers.StationPrintingHostData.Get(PluginEntry.DataModel,
                                                                       printingStation.StationPrintingHostID);
            }

            if (host == null)
            {
                host = new StationPrintingHost();
            }

            tbID.Text = (string)printingStation.ID;
            tbStationName.Text = printingStation.Text;
            cmbStationType.SelectedIndex = (int)printingStation.StationType;
            WindowsPrinterConfiguration config = Providers.WindowsPrinterConfigurationData.Get(PluginEntry.DataModel, printingStation.WindowsPrinterConfigurationID);
            cmbWindowsPrinter.SelectedData = config == null ? new DataEntity("", "") : new DataEntity(config.ID, config.Text);

            cmbRemoteHost.SelectedData = new DataEntity(printingStation.StationPrintingHostID, host.Text);
            cmbDeviceName.SelectedData = new DataEntity("", printingStation.PrinterDeviceName);
            cmbDeviceName.Enabled = !string.IsNullOrEmpty(cmbRemoteHost.Text);

            cmbHorizontalFontSize.SelectedIndex = printingStation.OPOSFontSizeH - 1;
            cmbVerticalFontSize.SelectedIndex = printingStation.OPOSFontSizeV - 1;
            ntbMaxCharacters.Value = printingStation.OPOSMaxChars;

            btnEditWindowsPrinter.Visible = PluginEntry.Framework.CanRunOperation("EditWindowsPrinters");
            HeaderText = Description;
            tabSheetTabs.SetData(isRevert, restaurantStationID, printingStation);
            cmbStationType_SelectedIndexChanged(this, EventArgs.Empty);
        }

        protected override bool SaveData()
        {
            printingStation.Text = tbStationName.Text;
            printingStation.StationType = (PrintingStation.StationTypeEnum)cmbStationType.SelectedIndex;
            printingStation.WindowsPrinter = cmbWindowsPrinter.Text;
            printingStation.WindowsPrinterConfigurationID = cmbWindowsPrinter.SelectedDataID ?? RecordIdentifier.Empty;
            printingStation.StationPrintingHostID = cmbRemoteHost.SelectedData.ID;
            printingStation.PrinterDeviceName = cmbDeviceName.Text;
            printingStation.OPOSFontSizeH = cmbHorizontalFontSize.SelectedIndex + 1;
            printingStation.OPOSFontSizeV = cmbVerticalFontSize.SelectedIndex + 1;
            printingStation.OPOSMaxChars = (int)ntbMaxCharacters.Value;

            tabSheetTabs.GetData();

            Providers.PrintingStationData.Save(PluginEntry.DataModel, printingStation);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "RestaurantStation", printingStation.ID, null);

            return true;
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        protected override bool DataIsModified()
        {
            if (tabSheetTabs.IsModified()) return true;

            if (tbID.Text != (string)printingStation.ID) return true;
            if (tbStationName.Text != printingStation.Text) return true;
            if (cmbStationType.SelectedIndex != (int)printingStation.StationType) return true;
            if (cmbWindowsPrinter.SelectedDataID != printingStation.WindowsPrinterConfigurationID) return true;
            if (cmbRemoteHost.SelectedData.ID != printingStation.StationPrintingHostID) return true;
            if (cmbDeviceName.Text != printingStation.PrinterDeviceName) return true;
            if (cmbVerticalFontSize.SelectedIndex != printingStation.OPOSFontSizeV - 1) return true;
            if (cmbHorizontalFontSize.SelectedIndex != printingStation.OPOSFontSizeH - 1) return true;
            if (ntbMaxCharacters.Value != printingStation.OPOSMaxChars) return true;

            return false;
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();
            base.OnClose();
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.ViewAllRestaurantStations, null, (ContextbarClickEventHandler)PluginOperations.ShowPrintingStationsListView), 200);
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "RestaurantStation":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == restaurantStationID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }
        }

        private void cmbWindowsPrinter_RequestData(object sender, EventArgs e)
        {
            cmbWindowsPrinter.SetData(Providers.WindowsPrinterConfigurationData.GetDataEntityList(PluginEntry.DataModel), null);
        }

        private void cmbRemoteHost_RequestData(object sender, EventArgs e)
        {
            cmbRemoteHost.SetData(Providers.StationPrintingHostData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbRemoteHost_SelectedDataChanged(object sender, EventArgs e)
        {
            StationPrintingHost host = Providers.StationPrintingHostData.Get(PluginEntry.DataModel, cmbRemoteHost.SelectedData.ID);
            if (host != null)
            {
                printingStation.StationPrintingHostAddress = host.Address;
                printingStation.StationPrintingHostPort = host.Port;
            }
            cmbDeviceName.Clear();
            cmbDeviceName.Text = string.Empty;
            cmbDeviceName.Enabled = !string.IsNullOrEmpty(cmbRemoteHost.Text);
        }

        private void cmbDeviceName_RequestData(object sender, EventArgs e)
        {
            PrintingStationCli cli = new PrintingStationCli(printingStation.StationPrintingHostAddress, printingStation.StationPrintingHostPort);
            string[] list = cli.GetPrinterList();

            if (list.Length == 0)
            {
                string err = string.Format(Properties.Resources.ErrorCannotLoadDeviceList, printingStation.StationPrintingHostAddress);
                MessageDialog.Show(string.Format("{0} [{1}]", err, cli.GetLastPrintMessage()));
                cmbDeviceName.Text = string.Empty;
                cmbDeviceName.Clear();
                return;
            }

            List<DataEntity> devlist = new List<DataEntity>();
            foreach (string dev in list)
            {
                devlist.Add(new DataEntity("", dev));
            }
            cmbDeviceName.SetData(devlist, null);
        }

        private void cmbStationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((PrintingStation.StationTypeEnum)cmbStationType.SelectedIndex)
            {
                case PrintingStation.StationTypeEnum.WindowsPrinter:
                    cmbRemoteHost.Enabled = false;
                    cmbWindowsPrinter.Enabled = true;
                    cmbRemoteHost.SelectedData = new DataEntity("","");
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = false;
                    btnEditRemoteHost.Enabled = false;
                    cmbHorizontalFontSize.Enabled = false;
                    cmbVerticalFontSize.Enabled = false;
                    ntbMaxCharacters.Enabled = false;
                    btnEditWindowsPrinter.Enabled = true;
                    break;

                case PrintingStation.StationTypeEnum.OPOSPrinter:
                    cmbRemoteHost.Enabled = true;
                    cmbWindowsPrinter.Enabled = false;
                    cmbWindowsPrinter.SelectedData = new DataEntity("", "");
                    cmbDeviceName.Enabled = true;
                    btnEditRemoteHost.Enabled = true;
                    cmbHorizontalFontSize.Enabled = true;
                    cmbVerticalFontSize.Enabled = true;
                    ntbMaxCharacters.Enabled = true;
                    cmbDeviceName.Enabled = !string.IsNullOrEmpty(cmbRemoteHost.Text);
                    btnEditWindowsPrinter.Enabled = false;
                    break;

                case PrintingStation.StationTypeEnum.HardwareProfilePrinter:
                    cmbRemoteHost.Enabled = false;
                    cmbWindowsPrinter.Enabled = false;
                    cmbWindowsPrinter.SelectedData = new DataEntity("", "");
                    cmbRemoteHost.SelectedData = new DataEntity("","");
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = false;
                    btnEditRemoteHost.Enabled = false;
                    cmbHorizontalFontSize.Enabled = true;
                    cmbVerticalFontSize.Enabled = true;
                    ntbMaxCharacters.Enabled = true;
                    btnEditWindowsPrinter.Enabled = false;
                    break;
            }
        }

        private void btnAddRemoteHost_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.RunOperation("EditPrintingStations", this, new PluginOperationArguments(cmbRemoteHost.SelectedData.ID, null, false));
        }

        private void btnEditWindowsPrinter_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.RunOperation("EditWindowsPrinters", this, new PluginOperationArguments(cmbWindowsPrinter.SelectedDataID ?? RecordIdentifier.Empty, null, false));
        }
    }
}
