using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportClasses.Employee;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ISettings
    {
        Dictionary<int, KeyboardMapping> KeyboardMapping
        {
            get;
        }

        DiscountCalculation DiscountCalculation
        {
            get;
        }

        /// <summary>
        /// Gets or sets wether the terminal is in training mode
        /// </summary>
        bool TrainingMode
        {
            get;
            set;
        }

        bool ForceHospitalityExit 
        { 
            get; 
            set; 
        }
        
        bool FinalizeSplitBill 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The unique ID when running the split bill operation to make sure that multiple terminals that are all using the same transaction ID 
        /// numbersequence don't overwrite each others split bill information
        /// </summary>
        RecordIdentifier SplitBillID
        {
            get; 
            set;
        }

        /// <summary>
        /// This field contains the default Price Group for the POS terminal.
        /// If you select a price group in this field, the POS terminal system checks whether there exists an item price with this price group when an item is sold on this POS terminal. If it finds the price, the item is sold at that price, otherwise, it is sold at the normal unit price.
        /// If you leave this field blank, there are no special item prices for the POS terminal.
        /// </summary>
        RecordIdentifier StorePriceGroup
        {
            get;
        }

        /// <summary>
        /// Is set to true if the tax is included in price.
        /// </summary>
        bool TaxIncludedInPrice
        {
            get;
        }

        Store Store
        {
            get;
        }

        Terminal Terminal
        {
            get;
        }

        HardwareProfile HardwareProfile
        {
            get;
        }

        FunctionalityProfile FunctionalityProfile
        {
            get;
        }

        VisualProfile VisualProfile
        {
            get;
        }

        UserProfile UserProfile
        {
            get;
        }

        SiteServiceProfile SiteServiceProfile
        {
            get;
        }

        KitchenServiceProfile KitchenServiceProfile
        {
            get;
        }

        SiteServiceProfile HospitalitySiteServiceProfile
        {
            get;
        }

        Parameters Parameters
        {
            get;
        }

        POSUser POSUser
        {
            get;
            set;
        }

        CompanyInfo CompanyInfo
        {
            get;
        }

        CultureInfo CultureInfo
        {
            get;
        }

        MainFormInfo MainFormInfo
        {
            get;
        }

        ILicenseValidator License
        {
            get;
        }

        IPOSApp POSApp
        {
            get;
        }

        void ApplyCultureSettings();

        string CultureName
        {
            get;
        }

        /// <summary>
        /// Loads all profiles for the application
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="userID">The pos user login name. This is used to check for profiles on the user</param>
        /// <returns>True if all profiles were loaded, false if en error occurred. Use GetProfileLoadErrors function to retrieve error messages</returns>
        bool LoadProfiles(IConnectionManager entry, RecordIdentifier userID);

        /// <summary>
        /// Checks if there are profiles/profile information missing and returns each error message
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        List<string> GetProfileLoadErrors(IConnectionManager entry);

        /// <summary>
        /// Used to only load the visual profile. This is used in cases when the visual profile information might have changed, i.e when a new 
        /// user logs in.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="errorMessage">The message to display to the user if the profile could not be loaded</param>
        /// <returns>True if the visual profile was loaded successfully, false if the visual profile could not be loaded</returns>
        bool LoadVisualProfile(IConnectionManager entry, out string errorMessage);

        DateTime BusinessDay { get; }

        DateTime BusinessSystemDay { get; }

        void ResetToPreviousState();

        void ResetPOSUser();

        /// <summary>
        /// Local state of the application, is not saved to the settings file
        /// </summary>
        bool NeedsEFTSetup { get; set; }

        /// <summary>
        /// Used to convey need of setup of services. Local state of the application, is not saved to the settings file
        /// </summary>
        bool HardwareProfileNeedsSetup { get; set; }

        /// <summary>
        /// Indicates if the hardware devices must be restarted. Local state of the application, is not saved to the settings file
        /// </summary>
        bool RestartDevices { get; set; }

        /// <summary>
        /// Gets or sets wether the POS is configured for a BASIC installation
        /// </summary>
        bool IsBasic { get; set; }

        /// <summary>
        /// Gets or sets wether the POS should suppress UI dialogs and forms. This is for example used when the POS Engine is running on a server where
        /// UI cannot be displayed.
        /// </summary>
        bool SuppressUI { get; set; }

        /// <summary>
        /// Returns the image used when no item image is available
        /// </summary>
        Image DefaultItemImage { get; }

        /// <summary>
        /// Add an employee to the recently used employees list
        /// Holds maximum 5 entries
        /// </summary>
        /// <param name="employee">The employee to add</param>
        void AddEmployeeToRecentList(Employee employee);

        /// <summary>
        /// Get a list of recently used employees
        /// </summary>
        /// <returns></returns>
        List<Employee> GetRecentEmployees();
    }
}
