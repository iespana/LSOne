using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Peripherals.Dialogs;
using LSOne.Peripherals.Interfaces;
using LSOne.Peripherals.Properties;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LSOne.Peripherals.DialogPages
{
    public partial class SetETaxFiscalDevicePage : UserControl, IHardwareValidator
    {
        public string OPOSType { get; set; }
        public string DetectedDevice { get; set; }
        public HardwareConfigurationDialog ContainerDialog { get; set; }
        
        private List<DataEntity> comPortsList;
        private ETaxSerialPort eTaxSerialPort;
        private string eTaxPortName;

        private bool isLoadingProfile = false;

        public SetETaxFiscalDevicePage(string type)
        {
            InitializeComponent();
            OPOSType = type;
            comPortsList = new List<DataEntity>();
            LoadCOMPorts();
            SetStyle(ControlStyles.Selectable, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void LoadCOMPorts()
        {
            comPortsList.Add(new DataEntity(string.Empty, string.Empty));
            var ports = SerialPort.GetPortNames();
            if (ports != null)
            {
                foreach (string item in ports)
                {
                    comPortsList.Add(new DataEntity(item, item));
                }
            }
        }

        public bool ValiddateInput()
        {
            ContainerDialog.Profile.ETaxConnected = chkDeviceConnected.Checked;
            ContainerDialog.Profile.ETaxPortName = cmbETaxComPortName.SelectedData.Text;

            return ValidateCOMPortName();
        }

        private bool ValidateCOMPortName()
        {
            if (cmbETaxComPortName.Enabled && string.IsNullOrEmpty(cmbETaxComPortName.SelectedData.Text))
            {
                errorProvider.ErrorText = Resources.DeviceNameCannotBeEmpty;
                errorProvider.Visible = true;
                cmbETaxComPortName.HasError = true;
                return false;
            }
            else
            {
                errorProvider.ErrorText = string.Empty;
                errorProvider.Visible = false;
                cmbETaxComPortName.HasError = false;
                return true;
            }
        }

        private void cmbDeviceName_SelectedDataChanged(object sender, EventArgs e)
        {
            ValidateCOMPortName();
        }

        private void cmbDeviceName_Leave(object sender, EventArgs e)
        {
            ValidateCOMPortName();
        }

        private void chkDrawerConnected_CheckedChanged(object sender, EventArgs e)
        {
            if (isLoadingProfile) return;
            
            cmbETaxComPortName.Enabled = chkDeviceConnected.Checked;
            btnTest.Enabled = chkDeviceConnected.Checked;
            
            if (!chkDeviceConnected.Checked)
            {
                cmbETaxComPortName.SelectedData = new DataEntity(string.Empty, string.Empty);
            }
            
            ValidateCOMPortName();
        }

        public void LoadProfile(HardwareProfile profile)
        {
            isLoadingProfile = true;
            
            chkDeviceConnected.Checked = profile.ETaxConnected;
            cmbETaxComPortName.SelectedData = new DataEntity(profile.ETaxPortName, profile.ETaxPortName);

            isLoadingProfile = false;
        }

        public string AutoDetectOPOS(DetectionDialog dlg)
        {
            string reply = string.Empty;
            DetectedDevice = string.Empty;

            var ports = SerialPort.GetPortNames();
            if (ports != null )
            {
                foreach (var port in ports)
                {
                    if (TestDeviceOnPort(port))
                    {
                        DetectedDevice = port;
                        reply = string.Format(Resources.AutoDetectOPOS_Successfully, OPOSType, port);

                        dlg.Messages.Enqueue(reply);
                        return reply;
                    }

                    dlg.Messages.Enqueue(string.Format(Resources.AutoDetectOPOS_Failed, OPOSType, port));

                    if (dlg.Cancel)
                    {
                        break;
                    }
                }
            }

            reply = string.Format(Resources.NoOPOSDevice, OPOSType);
            dlg.Messages.Enqueue(reply);
            return reply;
        }

        private bool TestDeviceOnPort(string portName)
        {
            try
            {
                eTaxPortName = portName;
                string info = GetDeviceInfo(true);
                LogDeviceDetailsAndInitialize(info);

                return !string.IsNullOrEmpty(info);
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Test Device: " + Environment.NewLine + this.Name , x);
                return false;
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
                chkDeviceConnected.Checked = true;
                cmbETaxComPortName.SelectedData = new DataEntity(DetectedDevice, DetectedDevice);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                eTaxPortName = cmbETaxComPortName.Text;
                string info = GetDeviceInfo(false);
                LogDeviceDetailsAndInitialize(info, true);
            }
            catch (Exception ex)
            {
                LSOne.Controls.TouchMessageDialog.Show(ContainerDialog,
                    Resources.TestFailed.Replace("#1", ex.Message));
            }
        }

        private void LogDeviceDetailsAndInitialize(string deviceInfo, bool showMessage = false)
        {
            bool isConnectionEstablished = !string.IsNullOrEmpty(deviceInfo);
            if (isConnectionEstablished)
            {
                string msg = Resources.SucessfullyConnectedToETax + Environment.NewLine + Environment.NewLine + GetDeviceInfoFormatted(deviceInfo);
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Trace, msg);

                Initialize(eTaxPortName, isConnectionEstablished);
                if (showMessage)
                {
                    LSOne.Controls.TouchMessageDialog.Show(ContainerDialog, msg);
                }
            }
        }

        private string GetDeviceInfoFormatted(string deviceInfo)
        {
            return Regex.Replace(deviceInfo, "[ ]{2,}", " "); //replace multiple spaces in info with single space
        }

        private void Initialize(string portName, bool isConnected)
        {
            if (isConnected)
            {
                ConfigureHardwareFlowControlIfNeeded();
            }
        }

        private string GetDeviceInfo(bool isAutomaticPortScan)
        {
            if (String.IsNullOrEmpty(eTaxPortName))
            {
                LSOne.Controls.TouchMessageDialog.Show(ContainerDialog, Resources.PleaseSelectACOMPort);
                return string.Empty;
            }

            try
            {
                OpenPort();
                return ETax.GetInfo(eTaxSerialPort);
            }
            catch
            {
                var msg = Resources.UnableToConnectToETaxOnPort + eTaxPortName;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, msg);
                if (!isAutomaticPortScan)
                {
                    LSOne.Controls.TouchMessageDialog.Show(ContainerDialog, msg);
                }
                return string.Empty;
            }
            finally
            {
                ClosePort();
            }
        }

        private void ConfigureHardwareFlowControlIfNeeded()
        {
            try
            {
                OpenPort();
                string testFiscalSignatureHash = ETax.GetTestFiscalSignature(eTaxSerialPort);
                bool isFailureToGetHash = string.IsNullOrEmpty(testFiscalSignatureHash);

                if (isFailureToGetHash)
                {
                    ETax.SetNewPortSettings(eTaxSerialPort);

                    var msg = Resources.ETaxSettingsChangedMustRebootDevice;
                    DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, msg + Environment.NewLine + this.Name);
                    MessageBox.Show(msg);
                }
            }
            catch
            {
                var msg = Resources.ETaxConfigChangeFail;
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, msg + Environment.NewLine + this.Name);
                MessageBox.Show(msg);
            }
            finally
            {
                ClosePort();
            }
        }

        private void OpenPort()
        {
            ClosePort();
            eTaxSerialPort = ETaxSerialPort.Open(eTaxPortName, PortSettings.GetDefault());
        }

        private void ClosePort()
        {
            var p = eTaxSerialPort;
            eTaxSerialPort = null;
            if (p != null)
                p.Dispose();
        }

        private void cmbETaxComPortName_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                comPortsList,
                null,
                true,
                cmbETaxComPortName.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }
    }
}
