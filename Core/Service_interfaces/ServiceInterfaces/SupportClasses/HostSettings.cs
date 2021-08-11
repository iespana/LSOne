using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.SupportClasses
{
    public class HostSettings
    {
        public RecordIdentifier CurrentStoreID;
        public RecordIdentifier CurrentTerminalID;
        public RecordIdentifier StoreCurrencyID;
        public bool TaxIncludedInPrice;
        public AggregateItemsModes AggregateItems;
        public bool AggregatePayments;
        public bool IsHospitality;
        public UseTaxGroupFromEnum UseTaxGroupFrom;
        public RecordIdentifier StorePriceGroup;
        public RecordIdentifier CompanyCurrencyID;
        public RecordIdentifier DefaultCustomerID;

        public HostSettings()
        {
            CurrentStoreID = RecordIdentifier.Empty;
            StoreCurrencyID = RecordIdentifier.Empty;
            CurrentTerminalID = RecordIdentifier.Empty;
            TaxIncludedInPrice = false;
            AggregateItems = AggregateItemsModes.Normal;
            IsHospitality = false;
            UseTaxGroupFrom = UseTaxGroupFromEnum.Store;
            CompanyCurrencyID = RecordIdentifier.Empty;
            DefaultCustomerID = RecordIdentifier.Empty;
        }
    }
}
