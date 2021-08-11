using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.ViewPlugins.Inventory.Reports
{
    public partial class GRDPickingListReport : DevExpress.XtraReports.UI.XtraReport
    {
        public GRDPickingListReport()
        {
            InitializeComponent();
        }

        public GRDPickingListReport(GoodsReceivingDocument goodsReceivingDocument, List<GoodsReceivingDocumentLine> goodsReceivingDocumentLines)
            :this()
        {
            this.goodsReceivingDocument.DataSource = goodsReceivingDocument;
            //this.POMiscChargesLines.DataSource = goodsReceivingDocumentLines;
        }



    }
}
