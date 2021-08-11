using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Peripherals.Dialogs;
using LSOne.Peripherals.Interfaces;
using LSOne.Peripherals.OPOS;
using LSOne.Peripherals.Properties;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals.DialogPages
{
    public partial class SetBarcodeReaderPage : UserControl, IHardwareValidator
    {
        public string OPOSType { get; set; }
        public string DetectedDevice { get; set; }

        private List<string> OPOSScannerList;
        private List<DataEntity> OPOSScannerDropDownList;
        public HardwareConfigurationDialog ContainerDialog { get; set; }

        public SetBarcodeReaderPage(string type)
        {
            InitializeComponent();
            OPOSType = type;
            OPOSScannerDropDownList = new List<DataEntity>();
            LoadDevices();
            SetStyle(ControlStyles.Selectable, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void LoadDevices()
        {
            cmbDeviceName.SelectedData = new DataEntity(-1,"");
            OPOSScannerList = HardwareConfigurationDialog.GetRegistryStrings(OPOSType);

            if (OPOSScannerList != null)
            {
                foreach (string item in OPOSScannerList)
                {
                    OPOSScannerDropDownList.Add(new DataEntity("", item));
                }
            }
        }



        public bool ValiddateInput()
        {
            if (ContainerDialog.Profile.ScannerConnected != chkDeviceConnected.Checked ||
                ContainerDialog.Profile.ScannerDeviceName != cmbDeviceName.Text)
            {
                ContainerDialog.Profile.ScannerConnected = chkDeviceConnected.Checked;
                ContainerDialog.Profile.ScannerDeviceName = cmbDeviceName.Text;
            }

            return ValidateDeviceName();
        }


        private bool ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled && string.IsNullOrEmpty(cmbDeviceName.SelectedData.Text))
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

        private void cmbDeviceName_SelectedDataChanged(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }

        private void cmbDeviceName_Leave(object sender, EventArgs e)
        {
            ValidateDeviceName();
        }

        private void chkDrawerConnected_CheckedChanged(object sender, EventArgs e)
        {
            cmbDeviceName.SelectedData = new DataEntity("", "");
            cmbDeviceName.Enabled = chkDeviceConnected.Checked;
            btnTest.Enabled = chkDeviceConnected.Checked;
            ValidateDeviceName();
        }

        public void LoadProfile(HardwareProfile profile)
        {
            chkDeviceConnected.Checked = profile.ScannerConnected;
            cmbDeviceName.Text = profile.ScannerDeviceName;
        }

        public string AutoDetectOPOS(DetectionDialog dlg)
        {
            string reply = string.Empty;
            DetectedDevice = string.Empty;
            
            if (OPOSScannerList != null )
            {
                foreach (var scanner in OPOSScannerList)
                {
                    if (TestDevice(scanner))
                    {
                        DetectedDevice = scanner;

                        reply = string.Format(Resources.AutoDetectOPOS_Successfully, OPOSType, scanner);

                        dlg.Messages.Enqueue(reply);
                        return reply;
                    }

                    dlg.Messages.Enqueue(string.Format(Resources.AutoDetectOPOS_Failed, OPOSType, scanner));

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

        private bool TestDevice(string deviceName)
        {
            try
            {
                Scanner.posScanner = new OPOSScanner(deviceName);

                Scanner.posScanner.Initialize();
                return true;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Test Device: " + Environment.NewLine + this.Name , x);
                return false;
            }
            finally
            {
                try
                {
                    Scanner.posScanner.Finalise();
                }
                catch (Exception x)
                {

                    DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Test Device: " + Environment.NewLine + this.Name, x);
                }
            }
        }


        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {


                Scanner.posScanner = new OPOSScanner(cmbDeviceName.Text);

                Scanner.posScanner.Initialize();
                LSOne.Controls.TouchMessageDialog.Show(ContainerDialog, Properties.Resources.TestSuccessful);
            }
            catch (Exception ex)
            {
                LSOne.Controls.TouchMessageDialog.Show(ContainerDialog,
                    Properties.Resources.TestFailed.Replace("#1", ex.Message));
            }
            finally
            {
                try
                {
                    Scanner.posScanner.Finalise();
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
                chkDeviceConnected.Checked = true;
                cmbDeviceName.SelectedData = new DataEntity("", DetectedDevice);
            }
        }

        private void cmbDeviceName_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                OPOSScannerDropDownList,
                null,
                true,
                cmbDeviceName.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }
    }
}
