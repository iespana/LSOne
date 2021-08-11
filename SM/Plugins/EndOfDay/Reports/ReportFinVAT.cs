using System.Collections.Generic;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.EndOfDay.Reports
{
    public partial class ReportFinVAT : DevExpress.XtraReports.UI.XtraReport
    {

        public ReportFinVAT(List<FinancialReportTaxGroupLine> taxGroupLines, DecimalLimit priceLimiter)
        {
            InitializeComponent();

            this.taxGroupLines.DataSource = taxGroupLines;

            totalNetAmount.Text = (from tgl in taxGroupLines
                                   select tgl.NetAmount).Sum().FormatWithLimits(priceLimiter);

            totalTaxAmount.Text = (from tgl in taxGroupLines
                                   select tgl.TaxAmount).Sum().FormatWithLimits(priceLimiter);

            totalGrossAmount.Text = (from tgl in taxGroupLines
                                   select tgl.GrossAmount).Sum().FormatWithLimits(priceLimiter);
        }

    }
}
