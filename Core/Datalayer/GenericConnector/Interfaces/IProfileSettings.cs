using System;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Localization;

namespace LSOne.DataLayer.GenericConnector.Interfaces
{
    public interface IProfileSettings
    {
        Address.AddressFormatEnum AddressFormat
        {
            get;
        }

        NameFormat NameFormat
        {
            get;
            set;
        }

        INameFormatter NameFormatter
        {
            get;
        }

        IAddressFormatter SystemAddressFormatter
        {
            get;
        }

        LocalizationContext LocalizationContext
        {
            get;
        }

        void SaveSetting(IConnectionManager entry, Guid settingID, SettingsLevel level, Setting setting);

        Setting GetSetting(IConnectionManager entry, Guid settingID);
        Setting GetSetting(IConnectionManager entry, Guid settingID, SettingsLevel level);
        Setting GetSetting(IConnectionManager entry, Guid settingID, SettingType type, string defaultValue);
        Setting GetLongSetting(IConnectionManager entry, Guid settingID, SettingType type, string defaultValue);

        object GetApplicationSettings(string applicationKey);
        void SetApplicationSettings(string applicationKey, object setttings);
    }
}
