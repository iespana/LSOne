using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.Utilities.DataTypes;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.TaxFree
{
    /// <summary>
    /// The configuration for the tax free functionality is stored in this business object
    /// </summary>
    public class TaxFreeConfig : DataEntity
    {
        public bool PromptCustomerForInformation { get; set; }
        public TaxFreePrintoutEnum PrintoutType { get; set; }
        public bool PromptForPassport { get; set; }
        public bool PromptForFlightInfo { get; set; }
        public bool PromptForReportInfo { get; set; }
        public bool PromptForTouristInfo { get; set; }
        public int LineWidth { get; set; }
        public int DefaultPadding { get; set; }
        public decimal MinTaxRefundLimit { get; set; }

        public string CountryCode { get; set; }
        public string FormType { get; set; }
        public string TaxNumber { get; set; }

        public string Address { get; set; }
        public string PostcodeCity { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Web { get; set; }
        public string VatNumber { get; set; }

        public bool Dirty { get; set; }

        public TaxFreeConfig()
        {
            ID = 1;
        }
    }
}
