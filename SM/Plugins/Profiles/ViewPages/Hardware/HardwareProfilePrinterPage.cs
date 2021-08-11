using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSRetail.PrintingStationClient;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    public partial class HardwareProfilePrinterPage : UserControl, ITabView
    {
        HardwareProfile profile;
        List<DataEntity> OPOSPrinterList;
        List<DataEntity> printerList;

        public HardwareProfilePrinterPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfilePrinterPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            OPOSPrinterList = new List<DataEntity>();
            List<string> printerListOPOS = PluginOperations.GetRegistryStrings("POSPrinter");
            printerListOPOS.ForEach(x => OPOSPrinterList.Add(new DataEntity("", x)));
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile) internalContext;

            cmbPrinter.SelectedIndex = (int) profile.Printer;
            cmbPrinter_SelectedIndexChanged(this, EventArgs.Empty);

            cmbPrintingHosts.SelectedData = Providers.StationPrintingHostData.Get(PluginEntry.DataModel, profile.StationPrintingHostID) ?? new DataEntity("", "");

            if(profile.Printer == HardwareProfile.PrinterHardwareTypes.PrintingStation)
            {
                cmbPrintingHosts_SelectedDataChanged(this, EventArgs.Empty);
            }

            if(profile.Printer == HardwareProfile.PrinterHardwareTypes.Windows)
            {
                cmbDeviceName.SelectedData = Providers.WindowsPrinterConfigurationData.Get(PluginEntry.DataModel, profile.WindowsPrinterConfigurationID) ?? new DataEntity("", "");
            }
            else
            {
                cmbDeviceName.SelectedData = new DataEntity("", profile.PrinterDeviceName);
            }

            tbDescription.Text = profile.PrinterDeviceDescription;
            chkPrintBinaryConversion.Checked = profile.PrintBinaryConversion;
            ntbCharset.Value = profile.PrinterCharacterSet;
            ntbExtraLines.Value = profile.PrinterExtraLines;
            btnEditPrintingHosts.Visible = PluginEntry.Framework.CanRunOperation("EditStationPrintingHosts");

            if (!btnEditPrintingHosts.Visible)
            {
                linkFields1.Location = new Point(linkFields1.Location.X - btnEditPrintingHosts.Width - (btnEditPrintingHosts.Margin.Right + btnEditPrintingHosts.Margin.Left), linkFields1.Location.Y);
            }

            ValidateDeviceName();
        }

        public bool DataIsModified()
        {
            if (cmbPrinter.SelectedIndex != (int)profile.Printer) return true;
            if (cmbDeviceName.SelectedData.Text != profile.PrinterDeviceName) return true;
            if (tbDescription.Text != profile.PrinterDeviceDescription) return true;
            if (chkPrintBinaryConversion.Checked != profile.PrintBinaryConversion) return true;
            if (ntbCharset.Value != profile.PrinterCharacterSet) return true;
            if (ntbExtraLines.Value != profile.PrinterExtraLines) return true;
            if (cmbDeviceName.SelectedDataID != profile.WindowsPrinterConfigurationID) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.Printer = (HardwareProfile.PrinterHardwareTypes)cmbPrinter.SelectedIndex;
            profile.PrinterDeviceName = cmbDeviceName.Text;
            profile.PrinterDeviceDescription = tbDescription.Text;
            profile.PrintBinaryConversion = chkPrintBinaryConversion.Checked;
            profile.PrinterCharacterSet = (int)ntbCharset.Value;
            profile.PrinterExtraLines = (int) ntbExtraLines.Value;
            if ((profile.Printer == HardwareProfile.PrinterHardwareTypes.OPOS && string.IsNullOrEmpty(profile.PrinterDeviceName)) ||
                 (profile.Printer == HardwareProfile.PrinterHardwareTypes.Windows && RecordIdentifier.IsEmptyOrNull(cmbDeviceName.SelectedDataID)))
            {
                PluginEntry.Framework.LogMessage(LogMessageType.Error, Properties.Resources.NoPrinterSelected);
            }

            profile.StationPrintingHostID = cmbPrintingHosts.SelectedDataID;
            profile.WindowsPrinterConfigurationID = cmbDeviceName.SelectedDataID;

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
        }

        #endregion

        private void cmbPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((HardwareProfile.PrinterHardwareTypes)cmbPrinter.SelectedIndex)
            {
                case HardwareProfile.PrinterHardwareTypes.Windows:
                    cmbPrintingHosts.SelectedData = new DataEntity("", "");
                    cmbDeviceName.SelectedData = new DataEntity("", "");
                    cmbDeviceName.Enabled = true;
                    ntbExtraLines.Enabled = false;
                    cmbPrintingHosts.Enabled = false;
                    btnEditPrintingHosts.Enabled = false;
                    btnEditDeviceName.Enabled = true;
                    break;
                case HardwareProfile.PrinterHardwareTypes.OPOS:
                    cmbPrintingHosts.SelectedData = new DataEntity("", "");
                    cmbDeviceName.SelectedData = new DataEntity("", "");
                    cmbDeviceName.Enabled = true;
                    ntbExtraLines.Enabled = true;
                    cmbPrintingHosts.Enabled = false;
                    btnEditPrintingHosts.Enabled = false;
                    btnEditDeviceName.Enabled = false;
                    break;
                case HardwareProfile.PrinterHardwareTypes.None:
                    cmbPrintingHosts.SelectedData = new DataEntity("", "");
                    cmbDeviceName.SelectedData = new DataEntity("", "");
                    cmbDeviceName.Enabled = false;
                    ntbExtraLines.Enabled = false;
                    cmbPrintingHosts.Enabled = false;
                    btnEditPrintingHosts.Enabled = false;
                    btnEditDeviceName.Enabled = false;
                    break;
                case HardwareProfile.PrinterHardwareTypes.PrintingStation:
                    cmbDeviceName.Enabled = true;
                    cmbDeviceName.SelectedData = new DataEntity("", "");
                    cmbPrintingHosts.Enabled = true;
                    btnEditPrintingHosts.Enabled = true;
                    btnEditDeviceName.Enabled = false;
                    break;
                default:
                    break;
            }

            ValidateDeviceName();
        }
        
        private void ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled &&
                (cmbDeviceName.SelectedData == null ||
                string.IsNullOrEmpty(cmbDeviceName.SelectedData.Text)))
            {
                errorProvider1.SetError(btnEditDeviceName, Properties.Resources.NoPrinterSelected);
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void cmbDeviceName_Leave(object sender, EventArgs e)
        {
           ValidateDeviceName();
        }

        private void cmbPrintingHosts_RequestData(object sender, EventArgs e)
        {
            cmbPrintingHosts.SetData(Providers.StationPrintingHostData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbPrintingHosts_RequestClear(object sender, EventArgs e)
        {
            cmbPrintingHosts.SelectedData = new DataEntity("", "");
            cmbDeviceName.SelectedData = new DataEntity("", "");
        }

        private void cmbPrintingHosts_SelectedDataChanged(object sender, EventArgs e)
        {
            // Refresh the device list on the combobox
            cmbDeviceName.Clear();

            if (cmbPrintingHosts.SelectedDataID == "")
            {
                return;
            }

            StationPrintingHost selectedHost = (StationPrintingHost) cmbPrintingHosts.SelectedData;

            PrintingStationCli cli = new PrintingStationCli(selectedHost.Address, selectedHost.Port);
            List<string> list = cli.GetPrinterList().ToList();

            if (list.Count == 0)
            {
                string err = string.Format(Properties.Resources.ErrorCannotLoadDeviceList, selectedHost.Address);
                MessageDialog.Show(string.Format("{0} [{1}]", err, cli.GetLastPrintMessage()));
                cmbDeviceName.Clear();
                return;
            }

            printerList = new List<DataEntity>();
            list.ForEach(x => printerList.Add(new DataEntity("", x)));
        }

        private void btnEditPrintingHosts_Click(object sender, EventArgs e)
        {
            PluginOperationArguments args;

            if (cmbPrintingHosts.SelectedDataID != "")
            {
                args = new PluginOperationArguments(cmbPrintingHosts.SelectedDataID, null);
            }
            else
            {
                args = PluginOperationArguments.Empty;
            }

            PluginEntry.Framework.RunOperation("EditStationPrintingHosts", this, args);
        }

        private void cmbDeviceName_RequestClear(object sender, EventArgs e)
        {
            cmbDeviceName.SelectedData = new DataEntity("", "");
        }

        private void cmbDeviceName_RequestData(object sender, EventArgs e)
        {
            switch ((HardwareProfile.PrinterHardwareTypes)cmbPrinter.SelectedIndex)
            {
                case HardwareProfile.PrinterHardwareTypes.Windows:
                    cmbDeviceName.SetData(Providers.WindowsPrinterConfigurationData.GetDataEntityList(PluginEntry.DataModel), null);
                    break;
                case HardwareProfile.PrinterHardwareTypes.OPOS:
                    cmbDeviceName.SetData(OPOSPrinterList, null);
                    break;
                case HardwareProfile.PrinterHardwareTypes.PrintingStation:
                    cmbDeviceName.SetData(printerList, null);
                    break;
            }
        }

        private void cmbDeviceName_SelectedDataChanged(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }

        private void btnEditDeviceName_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowWindowsPrinterConfigurationsView(cmbDeviceName.SelectedDataID ?? RecordIdentifier.Empty);
        }

        private void cmbDeviceName_SelectedDataCleared(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }
    }
}
