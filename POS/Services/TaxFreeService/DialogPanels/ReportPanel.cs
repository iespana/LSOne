using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Properties;
using System.Drawing;

namespace LSOne.Services.DialogPanels
{
    public partial class ReportPanel : DialogPageBase
    {
        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        
        public override bool FinishEnabled
        {
            get { return true; }
        }

        public ReportPanel(IConnectionManager entry)
            : base()
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
        }

        public override bool ValidateData()
        {
            errorProvider.Clear();
            errorProvider.Visible = false;

            if (IsTextFieldEmpty(tbBooking))
            {
                errorProvider.AddErrorMessage(Resources.BookingNumberRequried);
            }

            if (IsTextFieldEmpty(tbRunning))
            {
                errorProvider.AddErrorMessage(Resources.RunningNumberRequired);
            }

            if (Providers.TaxRefundData.RunningNumberExists(dlgEntry, tbBooking.Text, tbRunning.Text))
            {
                errorProvider.AddErrorMessage(Resources.RunningNumberUsed);
            }

            if (errorProvider.ErrorText == "")
            {
                return true;
            }

            errorProvider.Visible = true;
            return false;
        }

        public void GetData(TaxRefund refund)
        {
            refund.Booking = tbBooking.Text;
            refund.Running = tbRunning.Text;
        }
    }
}
