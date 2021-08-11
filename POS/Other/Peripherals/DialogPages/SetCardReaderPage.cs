using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Peripherals.Dialogs;
using LSOne.Peripherals.Interfaces;
using LSOne.Peripherals.OPOS;
using LSOne.Peripherals.Properties;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.Peripherals.DialogPages
{
    public partial class SetCardReaderPage : UserControl, IHardwareValidator
    {
        public string OPOSType { get; set; }
        public string DetectedDevice { get; set; }

        public HardwareConfigurationDialog ContainerDialog { get; set; }

        private List<string> OPOSMSRList;
        private List<DataEntity> OPOSMSRDropdownList;

        public SetCardReaderPage(string type)
        {
            InitializeComponent();
            SetStyle(ControlStyles.Selectable, true);
            OPOSType = type;
            OPOSMSRDropdownList = new List<DataEntity>();
            LoadDevices();
        }

        private void LoadDevices()
        {
            cmbDeviceName.SelectedData = new DataEntity(-1, "");
            OPOSMSRList = HardwareConfigurationDialog.GetRegistryStrings("MSR");
            if (OPOSMSRList != null)
            {
                foreach (var item in OPOSMSRList)
                {
                    OPOSMSRDropdownList.Add(new DataEntity("", item));
                }
            }
        }

        public bool ValiddateInput()
        {
            if (ContainerDialog.Profile.MsrDeviceType != (HardwareProfile.DeviceTypes)(int)cmbMSR.SelectedDataID ||
                ContainerDialog.Profile.MsrDeviceName != cmbDeviceName.Text ||
                ContainerDialog.Profile.StartTrack1 != tbStartDigit.Text ||
                ContainerDialog.Profile.Separator1 != tbDiffDigit.Text ||
                ContainerDialog.Profile.EndTrack1 != tbFinalDigit.Text)
            {
                ContainerDialog.Profile.MsrDeviceType = (HardwareProfile.DeviceTypes)(int)cmbMSR.SelectedDataID;
                ContainerDialog.Profile.MsrDeviceName = cmbDeviceName.Text;
                ContainerDialog.Profile.StartTrack1 = tbStartDigit.Text;
                ContainerDialog.Profile.Separator1 = tbDiffDigit.Text;
                ContainerDialog.Profile.EndTrack1 = tbFinalDigit.Text;
            }


            return ValidateDeviceName();
        }

        public void LoadProfile(HardwareProfile profile)
        {
            cmbMSR.SelectedData = new DataEntity((int)profile.MsrDeviceType, profile.MsrDeviceType.ToLocalizedString());
            cmbDeviceName.Text = profile.MsrDeviceName;
            tbStartDigit.Text = profile.StartTrack1;
            tbDiffDigit.Text = profile.Separator1;
            tbFinalDigit.Text = profile.EndTrack1;
            ValidateDeviceName();
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

        private bool TestDevice(string deviceName)
        {
            try
            {
                MSR.posMSR = new OPOSMSR(deviceName);

                MSR.posMSR.Initialize();
                return true;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Test Device: " + Environment.NewLine + this.Name, x);
                return false;
            }
            finally
            {
                try
                {
                    MSR.posMSR.Finalise();
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

                MSR.posMSR = new OPOSMSR(cmbDeviceName.Text);

                MSR.posMSR.Initialize();
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
                    MSR.posMSR.Finalise();
                }
                catch (Exception x)
                {

                    DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Test Device: " + Environment.NewLine + this.Name, x);
                }
            }
        }

        public string AutoDetectOPOS(DetectionDialog dlg)
        {
            string reply = string.Empty;
            DetectedDevice = string.Empty;

            try
            {
                LoadDevices();

                
                if (OPOSMSRList != null  )
                {
                    foreach (var msr in OPOSMSRList)
                    {
                        if (TestDevice(msr))
                        {
                            DetectedDevice = msr;

                            reply = string.Format(Resources.AutoDetectOPOS_Successfully, OPOSType, msr);

                            dlg.Messages.Enqueue(reply);
                            return reply;
                        }

                        dlg.Messages.Enqueue(string.Format(Resources.AutoDetectOPOS_Failed, OPOSType, msr));

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

        public void EnableTest(bool enabled)
        {
            btnTest.Enabled = enabled;
        }


        public void SetDetectedDevice()
        {
            if (!string.IsNullOrEmpty(DetectedDevice))
            {
                cmbMSR.SelectedData = OPOSMSRDropdownList[1];
                cmbDeviceName.SelectedData = new DataEntity("", DetectedDevice);
            }
        }

        private void cmbMSR_SelectedDataChanged(object sender, EventArgs e)
        {
            cmbDeviceName.SelectedData = new DataEntity("", "");
            cmbDeviceName.Enabled = (HardwareProfile.DeviceTypes)(int)cmbMSR.SelectedDataID == HardwareProfile.DeviceTypes.OPOS;
            btnTest.Enabled = cmbDeviceName.Enabled;
            ValidateDeviceName();
        }

        private void cmbMSR_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                GetMSRList(),
                null,
                true,
                cmbMSR.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private void cmbDeviceName_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                OPOSMSRDropdownList,
                null,
                true,
                cmbDeviceName.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private List<DataEntity> GetMSRList()
        {
            List<DataEntity> msrList = new List<DataEntity>();
            foreach (HardwareProfile.DeviceTypes item in (HardwareProfile.DeviceTypes[])Enum.GetValues(typeof(HardwareProfile.DeviceTypes)))
            {
                msrList.Add(new DataEntity(Convert.ToByte(item), item.ToLocalizedString()));
            }
            return msrList;
        }
    }
}
