using LSOne.DataLayer.BusinessObjects.TaxFree;
using System.Drawing;

namespace LSOne.Services.DialogPanels
{
    public partial class FlightInformationPanel : DialogPageBase
    {
        public FlightInformationPanel()
            : base()
        {
            InitializeComponent();
        }

        public override void GetData(Tourist tourist)
        {
            tourist.FlightNumber = tbDepartureFlight.Text;
            tourist.ArrivalDate = dtArrivalDate.Value;
            tourist.DepartureDate = dtDepartureDate.Value;
        }

        public override bool ValidateData()
        {
            string error = null;

            if (dtArrivalDate.Value.Day > dtDepartureDate.Value.Day)
                error = Properties.Resources.FlightDatesError;

            if (error == null)
            {
                errorProvider.Visible = false;
                return true;
            }

            errorProvider.Visible = true;
            errorProvider.ErrorText = error;
            return false;
        }
    }
}
