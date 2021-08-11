using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSItemSalePrice : TradeAgreementEntry
    {
        public int RMS_ID { get; set; }

        public int RMS_ItemID { get; set; }
        public string RMS_VariantCode { get; set; }

        public DateTime RMS_StartDate { get; set; }
        public DateTime RMS_EndDate { get; set; }
    }
}
