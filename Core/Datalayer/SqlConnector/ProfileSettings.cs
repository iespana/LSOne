using System;
using System.Collections.Generic;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Constants;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Localization;

namespace LSOne.DataLayer.SqlConnector
{
    public class ProfileSettings : IProfileSettings
    {
        private SortedList<Guid, string> settings;
        private NameFormat namingFormat;
        private LocalizationContext localizationContext;
        private Dictionary<string,object> applicationSettings;

        public ProfileSettings(SortedList<Guid, string> settings, ICountryResolver countryResolver)
        {
            Populate(settings, countryResolver);
        }

        public ProfileSettings()
        {
        }

        public void Populate(SortedList<Guid, string> settings, ICountryResolver countryResolver)
        {
            this.settings = settings;

            localizationContext = new LocalizationContext(new FirstNameFirstFormatter(), null, countryResolver);

            if(settings.ContainsKey(Settings.NamingFormat))
            {
                NameFormat = (NameFormat)Convert.ToInt32(settings[Settings.NamingFormat]);
            }
            else
            {
                NameFormat = NameFormat.FirstNameFirst;
            }
        }

        public LocalizationContext LocalizationContext
        {
            get
            {
                if (localizationContext.AddressFormatter == null)
                {
                    localizationContext = new LocalizationContext(NameFormatter, SystemAddressFormatter, localizationContext.CountryResolver);
                }

                return localizationContext;
            }
        }

        public Address.AddressFormatEnum AddressFormat
        {
            get
            {
                switch(Convert.ToInt32(settings[Settings.AddressFormat]))
                {
                    case 2:
                        return Address.AddressFormatEnum.GenericWithoutState;
                    case 3:
                        return Address.AddressFormatEnum.US;
                    case 4:
                        return Address.AddressFormatEnum.Canadian;
                    case 5:
                        return Address.AddressFormatEnum.Indian;
                    case 6:
                        return Address.AddressFormatEnum.UK;
                    default:
                        return Address.AddressFormatEnum.GenericWithState;
                }
            }
        }

        public NameFormat NameFormat
        {
            get
            {
                return namingFormat;
            }
            set
            {
                localizationContext = new LocalizationContext(
                    (value == NameFormat.FirstNameFirst) ?
                    new FirstNameFirstFormatter() :
                    (INameFormatter)new LastNameFirstFormatter(),
                    localizationContext.AddressFormatter,localizationContext.CountryResolver);

                namingFormat = value;
            }
        }

        public INameFormatter NameFormatter
        {
            get
            {
                return localizationContext.NameFormatter;
            }
        }

        public IAddressFormatter SystemAddressFormatter
        {
            get
            {
                if (localizationContext.AddressFormatter == null)
                {
                    localizationContext = new LocalizationContext(localizationContext.NameFormatter, LocalizationContext.GetAddressFormatter(AddressFormat), localizationContext.CountryResolver);
                }

                return localizationContext.AddressFormatter;
            }
        }

        public void SaveSetting(IConnectionManager entry, Guid settingID, SettingsLevel level, Setting setting)
        {
#if !MONO

            if (level == SettingsLevel.System)
            {
                DataProviders.SystemData.SetSystemSetting(
                    entry,
                    settingID,
                    setting.SystemSetting,
                    setting.SettingType); // This UUID represents Generic
            }
            else
            {
                if (setting.UserSettingExists)
                {
                    DataProviders.UserData.SetUserSetting(
                        entry,
                        entry.CurrentUser.ID,
                        settingID,
                        setting.SettingType,
                        setting.Value,
                        setting.LongUserSetting);
                }
                else
                {
                    DataProviders.UserData.ClearUserSetting(
                        entry,
                        entry.CurrentUser.ID,
                        settingID);
                }
                if(setting.SettingType == SettingType.Generic)
                {
                    string value = setting.Value;

                    settings[settingID] = value;

                    // See if the setting is a setting that requires special attention
                    if (settingID.Equals(Settings.NamingFormat))
                    {
                        NameFormat = (NameFormat)Convert.ToInt32(value);
                    }
                }
            }
#endif
        }

        public Setting GetSetting(IConnectionManager entry, Guid settingID)
        {
#if !MONO
            if (entry.CurrentUser.ID == Guid.Empty)
            {
                return DataProviders.SystemData.GetSystemSetting(entry, settingID);
            }
            return DataProviders.UserData.GetSetting(entry, entry.CurrentUser.ID, settingID);
#else
            return null;
#endif
        }

        public Setting GetSetting(IConnectionManager entry, Guid settingID, SettingsLevel level)
        {
#if !MONO
            if (level == SettingsLevel.System)
            {
                return DataProviders.SystemData.GetSystemSetting(entry, settingID);
            }
            return DataProviders.UserData.GetSetting(entry, entry.CurrentUser.ID, settingID);
#else
            return null;
#endif
        }

        public Setting GetSetting(IConnectionManager entry, Guid settingID, SettingType type, string defaultSetting)
        {
#if !MONO
            Setting setting = DataProviders.UserData.GetSetting(entry, entry.CurrentUser.ID, settingID);
            if (setting.Value == "")
            {
                setting.Value = defaultSetting;
            }
            setting.SettingType = type;
            return setting;
#else
            return null;
#endif
        }

        public Setting GetLongSetting(IConnectionManager entry, Guid settingID, SettingType type, string defaultSetting)
        {
#if !MONO
            Setting setting = DataProviders.UserData.GetSetting(entry, entry.CurrentUser.ID, settingID);
            if (setting.LongUserSetting == null || setting.LongUserSetting == "")
            {
                setting.LongUserSetting = defaultSetting;
            }
            setting.SettingType = type;
            return setting;
#else
            return null;
#endif
        }

        public object GetApplicationSettings(string applicationKey)
        {
            if (applicationSettings == null)
            {
                return null;
            }

            if (applicationSettings.ContainsKey(applicationKey))
            {
                return applicationSettings[applicationKey];
            }
            return null;
        }

        public void SetApplicationSettings(string applicationKey, object settings)
        {
            if (applicationSettings == null)
            {
                applicationSettings = new Dictionary<string, object>();
            }

            if (applicationSettings.ContainsKey(applicationKey))
            {
                applicationSettings[applicationKey] = settings;
            }
            else
            {
                applicationSettings.Add(applicationKey, settings);
            }
        }

    }
}
