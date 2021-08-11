using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems
{
    /// <summary>
    /// Extended store information used for display
    /// </summary>
    public class StoreListItemExtended : StoreListItem
    {
        public StoreListItemExtended() : base()
        {
            Currency = "";
            DefaultCustomer = "";
            FormProfile = "";
            FunctionalityProfile = "";
            PriceSetting = Store.StorePriceSettingsEnum.UsePriceGroupSettings;
            Region = "";
            SalesTaxGroup = "";
            SiteServiceProfile = "";
            TerminalsCount = 0;
            TouchButtons = "";
        }

        /// <summary>
        /// Currency name of the store
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Name of the default customer of the store
        /// </summary>
        public string DefaultCustomer { get; set; }

        /// <summary>
        /// Name of the store's form profile
        /// </summary>
        public string FormProfile { get; set; }

        /// <summary>
        /// Name of the store's functionality profile
        /// </summary>
        public string FunctionalityProfile { get; set; }

        /// <summary>
        /// Store's price setting
        /// </summary>
        public Store.StorePriceSettingsEnum PriceSetting { get; set; }

        /// <summary>
        /// Name of the store's region
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Name of the store's sales tax group
        /// </summary>
        public string SalesTaxGroup { get; set; }

        /// <summary>
        /// Name of the store's site service profile
        /// </summary>
        public string SiteServiceProfile { get; set; }

        /// <summary>
        /// Number of terminals assigned to this store
        /// </summary>
        public int TerminalsCount { get; set; }

        /// <summary>
        /// Name of the store's touch button layout
        /// </summary>
        public string TouchButtons { get; set; }
    }

    public class StoreListSearchFilterExtended : StoreListSearchFilter
    {
        public StoreListSearchFilterExtended() : base()
        {
            CurrencyCode = RecordIdentifier.Empty;
            CustomerID = RecordIdentifier.Empty;
            FormProfileID = RecordIdentifier.Empty;
            FunctionalityProfileID = RecordIdentifier.Empty;
            RegionID = RecordIdentifier.Empty;
            SalesTaxGroupID = RecordIdentifier.Empty;
            SiteServiceProfileID = RecordIdentifier.Empty;
            TouchButtonsLayoutID = RecordIdentifier.Empty;
        }

        /// <summary>
        /// Filter stores by currency code
        /// </summary>
        public RecordIdentifier CurrencyCode { get; set; }

        /// <summary>
        /// Filter stores by default customer ID
        /// </summary>
        public RecordIdentifier CustomerID { get; set; }

        /// <summary>
        /// Filter stores by form profile ID
        /// </summary>
        public RecordIdentifier FormProfileID { get; set; }

        /// <summary>
        /// Filter stores by functionality profile ID
        /// </summary>
        public RecordIdentifier FunctionalityProfileID { get; set; }

        /// <summary>
        /// Filter stores by region ID
        /// </summary>
        public RecordIdentifier RegionID { get; set; }

        /// <summary>
        /// Filter stores by sales tax group ID
        /// </summary>
        public RecordIdentifier SalesTaxGroupID { get; set; }

        /// <summary>
        /// Filter stores by site service profile ID
        /// </summary>
        public RecordIdentifier SiteServiceProfileID { get; set; }

        /// <summary>
        /// Filter stores by touch button layout ID
        /// </summary>
        public RecordIdentifier TouchButtonsLayoutID { get; set; }
    }
}
