using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ILicenseValidator
    {
        DataLayer.BusinessObjects.Enums.LicenseType LicenseType
        {
            get;
        }

        string LicenseTypeName
        {
            get;
        }

        string LicenseCompanyName
        {
            get;
        }

        string EncryptedLicenseCodeForCurrentHardware
        {
            get;
        }

        RecordIdentifier LicensePassword 
        { 
            set;
        }

        DateTime LicenseExpireDate 
        { 
            set; 
        }

        string Challenge(Guid guid);


    }
}
