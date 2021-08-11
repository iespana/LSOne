using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.ViewPlugins.Inventory.Reports
{
    public partial class POMiscChargesSubReport : DevExpress.XtraReports.UI.XtraReport
    {
        public POMiscChargesSubReport()
        {
            InitializeComponent();
        }

        public POMiscChargesSubReport(List<PurchaseOrderMiscCharges> POMiscCharges)
            :this()
        {
            this.POMiscCharges.DataSource = POMiscCharges;
        }



    }
}
