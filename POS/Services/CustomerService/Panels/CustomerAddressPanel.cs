using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.Panels
{
    public partial class CustomerAddressPanel : UserControl
    {
        private IConnectionManager entry;
        private ISettings settings;

        public event EventHandler ContentStateChanged;
        public bool NextEnabled { get; set; }

        public CustomerAddressPanel(IConnectionManager entry)
        {
            NextEnabled = false;

            InitializeComponent();

            this.entry = entry;
            settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            addressControlTouch.DataModel = entry;

            SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;
        }

        public Address AddressRecord => addressControlTouch.AddressRecord;

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            addressControlTouch.Focus();
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            bool enabled = addressControlTouch.Enabled;

            if (enabled != NextEnabled)
            {
                NextEnabled = enabled;

                if (ContentStateChanged != null)
                {
                    ContentStateChanged(this, EventArgs.Empty);
                }
            }
        }                

        public bool ValidateData()
        {
            bool reply = true;
            touchErrorProvider.Clear();

            if (settings.SiteServiceProfile.CustomerAddressIsMandatory && addressControlTouch.AddressRecord.IsEmpty)
            {
                reply = false;
                touchErrorProvider.AddErrorMessage(Properties.Resources.AddressMandatoryError);
            }

            touchErrorProvider.Visible = !reply;
            return reply;
        }

        private void addressControlTouch_ValueChanged(object sender, EventArgs e)
        {
            touchErrorProvider.Clear();
            touchErrorProvider.Visible = false;
        }
    }
}
