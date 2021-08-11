using LSOne.Controls;
using LSOne.Controls.EventArguments;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using System;
using System.Windows.Forms;

namespace LSOne.Services
{
    public partial class InformationDialog : TouchBaseForm
    {
        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public string DriverId { get; private set; }

        public string VehicleId { get; private set; }

        public string Odometer { get; private set; }

        public InformationDialog(IConnectionManager entry, bool allowCancel)
        {
            try
            {
                InitializeComponent();

                dlgEntry = entry;
                dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                if(!allowCancel)
                {
                    btnCancel.Visible = false;
                    btnOk.Location = btnCancel.Location;
                }
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
            }
        }

        private void tbDriverID_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbDriverID;
            touchKeyboard.DelayedEnabled = true;
        }

        private void tbDriverID_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void tbVehicleId_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbVehicleId;
            touchKeyboard.DelayedEnabled = true;
        }

        private void tbVehicleId_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void tbOdometer_Enter(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = tbOdometer;
            touchKeyboard.DelayedEnabled = true;
        }

        private void tbOdometer_Leave(object sender, EventArgs e)
        {
            touchKeyboard.BuddyControl = null;
            touchKeyboard.DelayedEnabled = false;
        }

        private void touchKeyboard1_EnterPressed(object sender, EventArgs e)
        {
            if (touchKeyboard.BuddyControl == tbDriverID)
            {
                tbVehicleId.Focus();
            }
            else if (touchKeyboard.BuddyControl == tbVehicleId)
            {
                tbOdometer.Focus();
            }
            else if (touchKeyboard.BuddyControl == tbOdometer)
            {
                btnOk.Focus();
            }
        }

        private void touchKeyboard1_ObtainCultureName(object sender, CultureEventArguments args)
        {
            if (dlgSettings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = dlgSettings.UserProfile.KeyboardCode;
                args.LayoutName = dlgSettings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = dlgSettings.Store.KeyboardCode;
                args.LayoutName = dlgSettings.Store.KeyboardLayoutName;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            VehicleId = tbVehicleId.Text;
            Odometer = tbOdometer.Text;
            DriverId = tbDriverID.Text;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}