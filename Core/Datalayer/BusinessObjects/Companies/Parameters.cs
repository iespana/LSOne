#if !MONO
#endif
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Companies
{
    public class Parameters : DataEntity
    {
        public Parameters()
            : base()
        {
            LocalStore = "";
            LocalStoreName = "";
            ReceiptSettings = ReceiptSettingsEnum.Printed;
            Dirty = false;
            TaxExemptTaxGroup = RecordIdentifier.Empty;

            ManuallyEnterCustomerID = false;
            ManuallyEnterItemID = false;
            ManuallyEnterVendorID = false;
            
            SiteServiceProfile = "";

            ScaleGramUnit = RecordIdentifier.Empty;
            ScaleKiloGramUnit = RecordIdentifier.Empty;
            ScaleOunceUnit = RecordIdentifier.Empty;
            ScalePoundUnit = RecordIdentifier.Empty;
        }

        public bool Dirty { get; set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier LocalStore { get; set; }
        [StringLength(60)]
        public string LocalStoreName { get; internal set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier TaxExemptTaxGroup { get; set; }
        /// <summary>
        /// Should the receipt for the transaction be printed, emailed or even both.
        /// </summary>
        public ReceiptSettingsEnum ReceiptSettings { get; set; }

        public RecordIdentifier SiteServiceProfile { get; set; }

        public bool ManuallyEnterItemID { get; set; }
        public bool ManuallyEnterCustomerID { get; set; }
        public bool ManuallyEnterVendorID { get; set; }
        public bool ManuallyEnterStoreID { get; set; }
        public bool ManuallyEnterTerminalID { get; set; }
        public bool ManuallyEnterUnitID { get; set; }

        /// <summary>
        /// Gets or sets entering tax code id manually
        /// </summary>
        public bool ManuallyEnterTaxCodeID { get; set; }

        /// <summary>
        /// Gets or sets entering tax group id manually
        /// </summary>
        public bool ManuallyEnterTaxGroupID { get; set; }

        /// <summary>
        /// If set to true the user can enter manually a gift card ID
        /// </summary>
        public bool ManuallyEnterGiftCardID { get; set; }

        [StringLength(20)]
        public string DefaultDimention { get; set; }

        public RecordIdentifier CurrentLocation { get; set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier ScaleGramUnit { get; set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier ScaleKiloGramUnit { get; set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier ScaleOunceUnit { get; set; }

        [RecordIdentifierValidation(20)]
        public RecordIdentifier ScalePoundUnit { get; set; }

        public bool IsScaleUnit(string unit) => new[] { ScaleGramUnit, ScaleKiloGramUnit, ScaleOunceUnit, ScalePoundUnit }.Contains(unit);
    }
}