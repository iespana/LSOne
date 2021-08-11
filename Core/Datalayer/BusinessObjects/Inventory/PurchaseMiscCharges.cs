using System;
using LSOne.DataLayer.BusinessObjects.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    /// <summary>
    /// Data entity class for a misc charges connect with a Purchase Order
    /// </summary>
    public class PurchaseOrderMiscCharges : DataEntity, ITaxAmount
    {
        public enum PurchaseOrderMiscChargesEnum
        {
            Freight = 0,
            Handling = 1,
            Installation = 2,
            MiscItemCharges = 3
        }

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(PurchaseOrderID, LineNumber);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// If using the formatted amounts in this class this property needs to be set. Function SetReportFormatting can also be used to set this property
        /// </summary>
        public DecimalLimit PriceLimiter;

        /// <summary>
        /// The purchase order ID the charge is connected to
        /// </summary>
        public RecordIdentifier PurchaseOrderID;
        /// <summary>
        /// The unique line ID for the charge
        /// </summary>
        public RecordIdentifier LineNumber;
        /// <summary>
        /// What type of charge is this see <see cref="PurchaseOrderMiscChargesEnum"/>
        /// </summary>
        public PurchaseOrderMiscChargesEnum Type;
        /// <summary>
        /// When displaying the type of charge then use this property to get the translated text for each type
        /// </summary>
        public string TypeText
        {
            get
            {
                switch (Type)
                {
                    case PurchaseOrderMiscChargesEnum.Freight:
                        return Properties.Resources.Freight;
                    case PurchaseOrderMiscChargesEnum.Handling:
                        return Properties.Resources.Handling;
                    case PurchaseOrderMiscChargesEnum.Installation:
                        return Properties.Resources.Intallation;
                    case PurchaseOrderMiscChargesEnum.MiscItemCharges:
                        return Properties.Resources.MiscItemCharges;
                    default:
                        return ""; 
                }
            }
        }

        /// <summary>
        /// The reason for the charge
        /// </summary>
        public string Reason;
        /// <summary>
        /// The net amount of the charge
        /// </summary>
        public decimal Amount;
        /// <summary>
        /// The tax amount of the charge
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// The unique ID of tax group used to calculate the tax amount
        /// </summary>
        public RecordIdentifier TaxGroupID;

        /// <summary>
        /// The description of the tax group used to calculate the tax amount
        /// </summary>
        public string TaxGroupName;
        /// <summary>
        /// The gross amount of the charge i.e. net amount + tax amount
        /// </summary>
        public decimal TotalAmount => Amount + TaxAmount;

        /// <summary>
        /// This property returns the net amount as a formatted number text using the global decimal settings set within the Site Manager
        /// </summary>
        public string FormattedAmount => PriceLimiter != null ? Amount.FormatWithLimits(PriceLimiter) : "";

        /// <summary>
        /// This property returns the tax amount as a formatted number text using the global decimal settings set within the Site Manager
        /// </summary>
        public string FormattedTaxAmount
        {
            get
            {
                if (PriceLimiter != null)
                {
                    string formattedTaxAmount = TaxAmount.FormatWithLimits(PriceLimiter);
                    return formattedTaxAmount != "0" ? formattedTaxAmount : "-";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// This property returns the total amount as a formatted number text using the global decimal settings set within the Site Manager
        /// </summary>
        public string FormattedTotalAmount
        {
            get
            {
                if (PriceLimiter != null)
                {
                    return TotalAmount.FormatWithLimits(PriceLimiter);
                }
                else
                {
                    return "";
                }
            }
        }

        public PurchaseOrderMiscCharges()
        {
            PurchaseOrderID = RecordIdentifier.Empty;
            LineNumber = RecordIdentifier.Empty;
            TaxGroupID = RecordIdentifier.Empty;
            TaxGroupName = "";
        }

        public PurchaseOrderMiscCharges(RecordIdentifier purchaseOrderID) : 
            this()
        {
            PurchaseOrderID = purchaseOrderID;
            
        }

        /// <summary>
        /// Sets the decimal settings needed to for the formatted... properties
        /// </summary>
        /// <param name="priceLimiter"></param>
        public void SetReportFormatting(DecimalLimit priceLimiter)
        {
            PriceLimiter = priceLimiter;
        }
    }
}
 