using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.LookupValues
{
    /// <summary>
    /// This class represents a restriction code that has been defined for a payment method limitation. Behind each restriction code
    /// there can be one or more records within a payment limitation.
    /// </summary>
    public class PaymentMethodLimitationRestrictionCode : DataEntity
    {
        public PaymentMethodLimitationRestrictionCode()
        {
            TaxExempt = false;
            TenderID = RecordIdentifier.Empty;
            LimitationMasterID = RecordIdentifier.Empty;
        }

        /// <summary>
        /// /// If true then items paid with the corresponding limitation will become tax exempt
        /// </summary>
        public bool TaxExempt { get; set; }

        /// <summary>
        /// The tender ID that this restriction code belongs to
        /// </summary>
        public RecordIdentifier TenderID { get; set; }

        /// <summary>
        /// The master ID of the payment method limitation that this restriction code belongs to
        /// </summary>
        public RecordIdentifier LimitationMasterID { get; set; }

        public override string Text
        {
            get => (string)ID;
            set => throw new NotImplementedException();
        }
    }
}
