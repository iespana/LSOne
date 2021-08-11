using System;
using System.Collections.Generic;
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
    public partial class SetLineDisplayPage : UserControl, IHardwareValidator
    {

        public string OPOSType { get; set; }
        private List<string> OPOSLineDisplayList;
        private List<DataEntity> OPOSLineDisplayDropDownList;

        public HardwareConfigurationDialog ContainerDialog { get; set; }

        public string DetectedDevice { get; set; }
        public SetLineDisplayPage(string type)
        {
            InitializeComponent();
            OPOSType = type;
            OPOSLineDisplayDropDownList = new List<DataEntity>();
            LoadDevices();
            SetStyle(ControlStyles.Selectable, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void LoadDevices()
        {
            cmbDeviceName.SelectedData = new DataEntity(-1, "");
            OPOSLineDisplayList = HardwareConfigurationDialog.GetRegistryStrings(OPOSType);
            if (OPOSLineDisplayList != null)
            {
                OPOSLineDisplayDropDownList = GetLineDisplayList();
            }
        }

        List<DataEntity> GetLineDisplayList()
        {
            List<DataEntity> lineDispays = new List<DataEntity>();
            foreach (string item in OPOSLineDisplayList)
            {
                OPOSLineDisplayDropDownList.Add(new DataEntity("", item));
            }
            return lineDispays;
        }

        public bool ValiddateInput()
        {
            if (ContainerDialog.Profile.LineDisplayDeviceType != (HardwareProfile.LineDisplayDeviceTypes)(int)cmbDisplay.SelectedDataID ||
                ContainerDialog.Profile.DisplayDeviceName != cmbDeviceName.Text ||
                ContainerDialog.Profile.DisplayTotalText != tbDisplayTotalText.Text ||
                ContainerDialog.Profile.DisplayBalanceText != tbDisplayBalanceText.Text ||
                ContainerDialog.Profile.DisplayClosedLine1 != tbTillClosedLine1.Text ||
                ContainerDialog.Profile.DisplayClosedLine2 != tbTillClosedLine2.Text ||
                ContainerDialog.Profile.DisplayCharacterSet != int.Parse(ntbCharset.Text) ||
                ContainerDialog.Profile.DisplayBinaryConversion != chkLineDisplayBinaryConversion.Checked)
            {
                ContainerDialog.HardwareProfileIsModified = true;

                ContainerDialog.Profile.LineDisplayDeviceType = (HardwareProfile.LineDisplayDeviceTypes)(int)cmbDisplay.SelectedDataID;
                ContainerDialog.Profile.DisplayDeviceName = cmbDeviceName.Text;
                ContainerDialog.Profile.DisplayTotalText = tbDisplayTotalText.Text;
                ContainerDialog.Profile.DisplayBalanceText = tbDisplayBalanceText.Text;
                ContainerDialog.Profile.DisplayClosedLine1 = tbTillClosedLine1.Text;
                ContainerDialog.Profile.DisplayClosedLine2 = tbTillClosedLine2.Text;
                ContainerDialog.Profile.DisplayCharacterSet = int.Parse(ntbCharset.Text);
                ContainerDialog.Profile.DisplayBinaryConversion = chkLineDisplayBinaryConversion.Checked;
            }
            return ValidateDeviceName();
        }

        public void LoadProfile(HardwareProfile profile)
        {
            cmbDisplay.SelectedData = new DataEntity((int)profile.LineDisplayDeviceType, profile.LineDisplayDeviceType.ToLocalizedString());
            cmbDeviceName.Text = profile.DisplayDeviceName;
            tbDisplayTotalText.Text = profile.DisplayTotalText;
            tbDisplayBalanceText.Text = profile.DisplayBalanceText;
            tbTillClosedLine1.Text = profile.DisplayClosedLine1;
            tbTillClosedLine2.Text = profile.DisplayClosedLine2;
            ntbCharset.Text = profile.DisplayCharacterSet.ToString();
            chkLineDisplayBinaryConversion.Checked = profile.DisplayBinaryConversion;
        }

        private void cmbDisplay_SelectedDataChanged(object sender, EventArgs e)
        {
            btnTest.Enabled = (HardwareProfile.LineDisplayDeviceTypes)(int)cmbDisplay.SelectedDataID == HardwareProfile.LineDisplayDeviceTypes.OPOS;
            
            switch ((HardwareProfile.LineDisplayDeviceTypes)(int)cmbDisplay.SelectedDataID)
            {
                case HardwareProfile.LineDisplayDeviceTypes.OPOS:
                    OPOSLineDisplayDropDownList = new List<DataEntity>();
                    OPOSLineDisplayDropDownList = GetLineDisplayList();
                    cmbDeviceName.SelectedData = new DataEntity("","");
                    cmbDeviceName.Enabled = true;
                    break;
                case HardwareProfile.LineDisplayDeviceTypes.VirtualDisplay:
                case HardwareProfile.LineDisplayDeviceTypes.None:
                    cmbDeviceName.SelectedData = new DataEntity("","");
                    cmbDeviceName.Enabled = false;
                    break;
                default:
                    break;
            }
            ValidateDeviceName();
        }


        private bool ValidateDeviceName()
        {
            if (cmbDeviceName.Enabled && string.IsNullOrEmpty(cmbDeviceName.Text))
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


        private void SetLineDisplay_Load(object sender, EventArgs e)
        {
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                LineDisplay.posLineDisplay = new OPOSLineDisplay();
                LineDisplay.posLineDisplay.DeviceName = cmbDeviceName.Text;
                LineDisplay.posLineDisplay.CharacterSet = int.Parse(ntbCharset.Text);
                LineDisplay.posLineDisplay.Initialize();

                LineDisplay.DisplayText("Testing");


                LSOne.Controls.TouchMessageDialog.Show(ContainerDialog, Properties.Resources.TestSuccessful);
            }
            catch (Exception ex)
            {
                LSOne.Controls.TouchMessageDialog.Show(ContainerDialog,
                    Properties.Resources.TestFailed.Replace("#1", ex.Message));
            }
            finally
            {
                LineDisplay.ClearText();
                try
                {
                    LineDisplay.posLineDisplay.Finalise();
                }
                catch (Exception x)
                {

                    DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Test Device: " + Environment.NewLine + this.Name, x);
                }
            }
        }

        private bool TestDevice(string deviceName)
        {
            try
            {
                LineDisplay.posLineDisplay = new OPOSLineDisplay();
                LineDisplay.posLineDisplay.DeviceName = deviceName;
                LineDisplay.posLineDisplay.CharacterSet = 0;
                LineDisplay.posLineDisplay.Initialize();

                LineDisplay.DisplayText("Testing");

                return true;
            }
            catch (Exception x)
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Test Device: " + Environment.NewLine + this.Name, x);
                return false;
            }
            finally
            {
                LineDisplay.ClearText();
                try
                {
                    LineDisplay.posLineDisplay.Finalise();
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

                if (OPOSLineDisplayList != null )
                {
                    foreach (var lineDisplay in OPOSLineDisplayList)
                    {
                        if (TestDevice(lineDisplay))
                        {
                            DetectedDevice = lineDisplay;

                            reply = string.Format(Resources.AutoDetectOPOS_Successfully, OPOSType, lineDisplay);

                            dlg.Messages.Enqueue(reply);
                            return reply;
                        }

                        dlg.Messages.Enqueue(string.Format(Resources.AutoDetectOPOS_Failed, OPOSType, lineDisplay));
                       
                        if (dlg.Cancel )
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

        private void ntbCharset_TextChanged(object sender, EventArgs e)
        {

        }
        public void EnableTest(bool enabled)
        {
            btnTest.Enabled = enabled;
        }


        public void SetDetectedDevice()
        {
            if (!string.IsNullOrEmpty(DetectedDevice))
            {
                cmbDisplay.SelectedData = new DataEntity((int)HardwareProfile.LineDisplayDeviceTypes.OPOS, HardwareProfile.LineDisplayDeviceTypes.OPOS.ToLocalizedString());
                cmbDeviceName.SelectedData = new DataEntity("", DetectedDevice);
            }
        }

        private void cmbDisplay_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                GetDisplayList(),
                null,
                true,
                cmbDisplay.SkipIDColumn,
                false,
                50,
                false);

            panelToEmbed.Touch = true;
            e.ControlToEmbed = panelToEmbed;
        }

        private List<DataEntity> GetDisplayList()
        {
            List<DataEntity> displayList = new List<DataEntity>();
            foreach (HardwareProfile.LineDisplayDeviceTypes item in (HardwareProfile.LineDisplayDeviceTypes[])Enum.GetValues(typeof(HardwareProfile.LineDisplayDeviceTypes)))
            {
                displayList.Add(new DataEntity((int)item, item.ToLocalizedString()));
            }
            return displayList;
        }

        private void cmbDeviceName_DropDown(object sender, DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                OPOSLineDisplayDropDownList,
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
