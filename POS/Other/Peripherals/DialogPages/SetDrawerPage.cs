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
    public partial class SetDrawerPage : UserControl, IHardwareValidator
    {
        public string OPOSType { get; set; }
        private List<string> OPOSDrawerList;
        private List<DataEntity> OPOSDrawerDropDownList;
        public HardwareConfigurationDialog ContainerDialog { get; set; }
        public string DetectedDevice { get; set; }
        
        public SetDrawerPage(string type)
        {
            InitializeComponent();
            OPOSType = type;
            OPOSDrawerDropDownList = new List<DataEntity>();
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
            OPOSDrawerList = HardwareConfigurationDialog.GetRegistryStrings(OPOSType);
            if (OPOSDrawerList != null)
            {
                foreach (string drawer in OPOSDrawerList)
                {
                    OPOSDrawerDropDownList.Add(new DataEntity("", drawer));
                }
            }
        }

        public bool ValiddateInput()
        {
            if (ContainerDialog.Profile.DrawerDeviceType !=
                (chkDrawerConnected.Checked ? HardwareProfile.DeviceTypes.OPOS : HardwareProfile.DeviceTypes.None) ||
                ContainerDialog.Profile.DrawerDeviceName != cmbDeviceName.Text ||
                ContainerDialog.Profile.DrawerOpenText != tbOpenText.Text
                )
            {
                ContainerDialog.HardwareProfileIsModified = true;
                ContainerDialog.Profile.DrawerDeviceType = chkDrawerConnected.Checked
                    ? HardwareProfile.DeviceTypes.OPOS
                    : HardwareProfile.DeviceTypes.None;
                ContainerDialog.Profile.DrawerDeviceName = cmbDeviceName.Text;
                ContainerDialog.Profile.DrawerOpenText = tbOpenText.Text;
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
            cmbDeviceName.Enabled = tbOpenText.Enabled = btnTest.Enabled = chkDrawerConnected.Checked;

            ValidateDeviceName();
        }

        public void LoadProfile(HardwareProfile profile)
        {
            chkDrawerConnected.Checked = profile.DrawerDeviceType == HardwareProfile.DeviceTypes.OPOS;
            cmbDeviceName.Text = profile.DrawerDeviceName;
            tbOpenText.Text = profile.DrawerOpenText;
        }

        public string AutoDetectOPOS(DetectionDialog dlg)
        {
            string reply = string.Empty;
            DetectedDevice = string.Empty;
            try
            {
                LoadDevices();
                
                if (OPOSDrawerList != null )
                {
                    foreach (var drawer in OPOSDrawerList)
                    {
                        if (TestDevice(drawer))
                        {
                            DetectedDevice = drawer;

                            reply = string.Format(Resources.AutoDetectOPOS_Successfully, OPOSType, drawer);

                            dlg.Messages.Enqueue(reply);
                            return reply;
                           
                        }

                        dlg.Messages.Enqueue(string.Format(Resources.AutoDetectOPOS_Failed, OPOSType, drawer));
                      
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

        private bool TestDevice(string deviceName)
        {
            try
            {
                CashDrawer.posCashDrawer = new OPOSCashDrawer(deviceName);
                CashDrawer.posCashDrawer.Initialize();
                CashDrawer.OpenDrawer();
               
                return true;
            }
            catch (Exception x )
            {
                DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Test Device: " + Environment.NewLine + this.Name, x);
                return false;
            }
            finally
            {
                try
                {
                    CashDrawer.posCashDrawer.Finalise();
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
                CashDrawer.posCashDrawer = new OPOSCashDrawer(cmbDeviceName.Text);
                CashDrawer.posCashDrawer.Initialize();
                CashDrawer.OpenDrawer();

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
                    CashDrawer.posCashDrawer.Finalise();
                }
                catch (Exception x)
                {

                    DLLEntry.DataModel.ErrorLogger.LogMessage(LogMessageType.Error, "Test Device: " + Environment.NewLine + this.Name, x);
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
                chkDrawerConnected.Checked = true;
                cmbDeviceName.SelectedData = new DataEntity("", DetectedDevice);
            }
        }

        private void cmbDeviceName_DropDown(object sender, Controls.DropDownEventArgs e)
        {
            DualDataPanel panelToEmbed = new DualDataPanel(
                "",
                OPOSDrawerDropDownList,
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
