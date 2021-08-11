﻿using LSOne.DataLayer.BusinessObjects.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.RMSMigration.Model
{
    public class RMSPurchaseOrderHeader : PurchaseOrder
    {
        public int RMS_ID { get; set; }

        public int RMS_StoreID { get; set; }
        public int RMS_VendorID { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
