using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Peripherals.Dialogs;
using LSOne.Peripherals.Interfaces;
using LSOne.Peripherals.OPOS;
using LSOne.Peripherals.Properties;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals.DialogPages
{
    public partial class SetPrinterPage : UserControl, IHardwareValidator
    {
        List<DataEntity> OPOSPrinterList;
        List<DataEntity> windowsPrinterList;

        public string OPOSType { get; set; }
        public HardwareConfigurationDialog ContainerDialog { get; set; }
        public string DetectedDevice { get; set; }
        private delegate void SetDevice(string device);

        public SetPrinterPage(string type)
        {
            InitializeComponent();
            OPOSType = type;
            LoadDevices();
            SetStyle(ControlStyles.Selectable, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void LoadDevices()
        {
            OPOSPrinterList = HardwareConfigurationDialog.GetRegistryStrings(OPOSType).Select(x => new DataEntity("", x)).ToList();
            windowsPrinterList = Providers.WindowsPrinterConfigurationData.GetDataEntityList(DLLEntry.DataModel);
        }

        public bool ValiddateInput()
        {
            if (
                ContainerDialog.Profile.Printer != (HardwareProfile.PrinterHardwareTypes)(int)cmbPrinter.SelectedDataID ||
                ContainerDialog.Profile.PrinterDeviceName != cmbDeviceName.Text ||
                ContainerDialog.Profile.PrintBinaryConversion != chkDecimalConversion.Checked ||
                ContainerDialog.Profile.PrinterCharacterSet != int.Parse(ntbPrinterCharset.Text) ||
                ContainerDialog.Profile.PrinterExtraLines != int.Parse(ntbExtraLines.Text)
                )
            {
                ContainerDialog.HardwareProfileIsModified = true;
                ContainerDialog.Profile.Printer = (HardwareProfile.PrinterHardwareTypes)(int)cmbPrinter.SelectedDataID;
                ContainerDialog.Profile.PrinterDeviceName = cmbDeviceName.Text;
                ContainerDialog.Profile.WindowsPrinterConfigurationID = cmbDeviceName.SelectedDataID;
                ContainerDialog.Profile.PrintBinaryConversion = chkDecimalConversion.Checked;
                ContainerDialog.Profile.PrinterCharacterSet = int.Parse(ntbPrinterCharset.Text);
                ContainerDialog.Profile.PrinterExtraLines = int.Parse(ntbExtraLines.Text);
            }

            return ValidateDeviceName();
        }

        private void cmbPrinter_SelectedDataChanged(object sender, EventArgs e)
        {
            btnTest.Enabled = (HardwareProfile.PrinterHardwareTypes)(int)cmbPrinter.SelectedDataID == HardwareProfile.PrinterHardwareTypes.OPOS;
            cmbDeviceName.SelectedData = new DataEntity("", "");
            cmbDeviceName_RequestData(null, EventArgs.Empty);

            ValidateDeviceName();
        }

        private bool ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled 
                && (HardwareProfile.PrinterHardwareTypes)(int)cmbPrinter.SelectedDataID != HardwareProfile.PrinterHardwareTypes.None 
                && (cmbDeviceName.SelectedData == null || string.IsNullOrEmpty(cmbDeviceName.SelectedData.Text)))
            {
                errorProvider.ErrorText = Resources.DeviceNameCannotBeEmpty;
                errorProvider.Visible = true;
                cmbDeviceName.HasError = true;
                return false;
            }
            else
            {
                errorProvider.ErrorText = "";
                errorProvider.Visible = false;
                cmbDeviceName.HasError = false;
                return true;
            }
        }

        private void cmbDeviceName_Leave(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }

        public void LoadProfile(HardwareProfile profile)
        {
            cmbPrinter.SelectedData = new DataEntity((int)profile.Printer, profile.Printer.ToLocalizedString());
            cmbDeviceName_RequestData(this, EventArgs.Empty);

            if (profile.Printer == HardwareProfile.PrinterHardwareTypes.Windows)
            {
                cmbDeviceName.SelectedData = Providers.WindowsPrinterConfigurationData.Get(DLLEntry.DataModel, profile.WindowsPrinterConfigurationID) ?? new DataEntity("", "");
            }
            else
            {
                cmbDeviceName.SelectedData = new DataEntity("", profile.PrinterDeviceName);
            }

            chkDecimalConversion.Checked = profile.PrintBinaryConversion;
            ntbPrinterCharset.Text = profile.PrinterCharacterSet.ToString();
            ntbExtraLines.Text = profile.PrinterExtraLines.ToString();

            ValidateDeviceName();
        }

        public string AutoDetectOPOS(DetectionDialog dlg)
        {
            string reply = string.Empty;
            DetectedDevice = string.Empty;
            try
            {
                LoadDevices();

                if (OPOSPrinterList != null )
                {
                    foreach (var printer in OPOSPrinterList)
                    {
                        if (TestDevice(printer.Text))
                        {
                            DetectedDevice = printer.Text;

                            reply = string.Format(Resources.AutoDetectOPOS_Successfully, OPOSType, printer.Text);

                            dlg.Messages.Enqueue(reply);
                            return reply;
                        }
                        dlg.Messages.Enqueue(string.Format(Resources.AutoDetectOPOS_Failed, OPOSType, printer.Text));
                        
                        if (dlg.Cancel)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            reply = string.Format(Resources.NoOPOSDevice, OPOSType);
            dlg.Messages.Enqueue(reply);
            return reply;
        }

        private bool TestDevice(string deviceName)
        {
            try
            {
                Printer.posPrinter = new OPOSPrinter(deviceName, int.Parse(ntbPrinterCharset.Text));
                return Printer.posPrinter.Test(true);
            }
            catch (Exception x )
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.Devices.LoadDevices()", x);
                return false;
            }
            finally
            {
                try
                {
                    Printer.posPrinter.Finalise();
                }
                catch (Exception x)
                {
                    DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Peripherals.Devices.LoadDevices()", x);
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                Printer.posPrinter = new OPOSPrinter(cmbDeviceName.Text,
                    int.Parse(ntbPrinterCharset.Text));
                if (! Printer.posPrinter.Test(true))
                {
                    throw new Exception(Resources.CouldNotTestPrintingOfReceipt);
                }
              
                LSOne.Controls.TouchMessageDialog.Show(ContainerDialog, Properties.Resources.TestSuccessful);
            }
            catch (Exception ex)
            {
                LSOne.Controls.TouchMessageDialog.Show(ContainerDialog, Properties.Resources.TestFailed.Replace("#1", ex.Message));
            }
            finally
            {
                try
                {
                    Printer.posPrinter.Finalise();
                }
                catch (Exception)
                {
                    
                }
            }
        }

        public void EnableTest(bool enabled)
        {
            btnTest.Enabled = enabled;
        }

        public void SetDetectedDevice()
        {
            if (!string.IsNullOrEmpty(DetectedDevice))
            {
                cmbDeviceName.SelectedData.Text = DetectedDevice;
            }
        }

        private void cmbDeviceName_SelectedDataChanged(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }

        private void cmbDeviceName_SelectedDataCleared(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }

        private void cmbDeviceName_RequestData(object sender, EventArgs e)
        {
            switch ((HardwareProfile.PrinterHardwareTypes)(int)cmbPrinter.SelectedDataID)
            {
                case HardwareProfile.PrinterHardwareTypes.Windows:
                    cmbDeviceName.SetData(windowsPrinterList, null);
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = true;
                    ntbExtraLines.Enabled = false;
                    break;
                case HardwareProfile.PrinterHardwareTypes.OPOS:
                    cmbDeviceName.SetData(OPOSPrinterList, null);
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = true;
                    ntbExtraLines.Enabled = false;
                    break;
                case HardwareProfile.PrinterHardwareTypes.None:
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = false;
                    ntbExtraLines.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void cmbPrinter_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                GetPrinterTypeList(),
                null,
                true,
                cmbPrinter.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private List<DataEntity> GetPrinterTypeList()
        {
            List<DataEntity> printerList = new List<DataEntity>();
            foreach (HardwareProfile.PrinterHardwareTypes item in (HardwareProfile.PrinterHardwareTypes[])Enum.GetValues(typeof(HardwareProfile.PrinterHardwareTypes)))
            {
                printerList.Add(new DataEntity((int)item, item.ToLocalizedString()));
            }
            return printerList;
        }
    }
}
