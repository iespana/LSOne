using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement.Validity
{
    public class StoreValidity : DataEntity
    {
        public bool TerminalExists { get; set; }
        public bool FunctionalityProfileExist { get; set; }
        public bool ButtonLayoutExists { get; set; }
        public bool PaymentTypesExists { get; set; }
        public bool PriceSettingsMatched { get; set; }
    }
}
