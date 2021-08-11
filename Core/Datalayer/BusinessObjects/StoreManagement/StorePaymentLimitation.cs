using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.Utilities.Attributes;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    [RecordIdentifierConstruction("ID", typeof(string), typeof(string))]
    public class StorePaymentLimitation : DataEntity, ICloneable
    {
        public RecordIdentifier StoreID { get; set; }

        public RecordIdentifier TenderTypeID { get; set; }

        public RecordIdentifier LimitationMasterID { get; set; }

        public RecordIdentifier RestrictionCode { get; set; }

        public StorePaymentLimitation()
        {
            StoreID = RecordIdentifier.Empty;
            TenderTypeID = RecordIdentifier.Empty;
            LimitationMasterID = RecordIdentifier.Empty;
            RestrictionCode = RecordIdentifier.Empty;
        }

        public override RecordIdentifier ID
        {
            get
            {
                return LimitationMasterID;
            }
            set
            {
                LimitationMasterID = value;
            }
        }

        public override object Clone()
        {
            var storePaymentLimitation = new StorePaymentLimitation();
            Populate(storePaymentLimitation);
            return storePaymentLimitation;
        }

        public void Populate(StorePaymentLimitation storePaymentLimitation)
        {
            storePaymentLimitation.StoreID = (RecordIdentifier)StoreID.Clone();
            storePaymentLimitation.TenderTypeID = (RecordIdentifier)TenderTypeID.Clone();
            storePaymentLimitation.LimitationMasterID = (RecordIdentifier)LimitationMasterID.Clone();
            storePaymentLimitation.RestrictionCode = (RecordIdentifier)RestrictionCode.Clone();
        }

    }
}
