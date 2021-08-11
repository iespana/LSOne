using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.EndOfDay.Reports
{
    public partial class ReportFinPayment : DevExpress.XtraReports.UI.XtraReport
    {
        public ReportFinPayment()
        {
            InitializeComponent();
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            
        }

        private void Detail1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            System.Data.DataRowView row = (System.Data.DataRowView)this.GetCurrentRow();
            if (row != null)
            {
                string tenderID = (string)row[0];
                System.Data.DataRow[] rows = this.datasetPayments1.Tables[1].Select("TenderID='" + tenderID + "'");
                if (rows.Length < 1)
                {
                    e.Cancel = true;                    
                }
            }
        }
        
        
    }
}
