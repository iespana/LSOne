using System;

namespace LSOne.DataLayer.GenericConnector.DataEntities
{
    public class SettingType
    {
        private const string generic = "C79AE480-7EE1-11DB-9FE1-0800200C9A66";
        private const string uIFieldVisisbility = "DAEE3FF0-7EE1-11DB-9FE1-0800200C9A66";
        private const string uiSetting = "F43A3360-B404-11E2-9E96-0800200C9A66";
        private const string omniSetting = "2E8B5791-7DFC-4998-A5B2-C5828DDD8864";

        public Guid Type { get; private set; }

        private SettingType(string guid)
        {
            Type = new Guid(guid);
        }

        public static SettingType Generic
        {
            get
            {
                return new SettingType(generic);
            }
        }

        public static SettingType UIFieldVisisbility
        {
            get
            {
                return new SettingType(uIFieldVisisbility);
            }
        }

        public static SettingType UISetting
        {
            get
            {
                return new SettingType(uiSetting);
            }
        }

        public static SettingType OmniSetting
        {
            get
            {
                return new SettingType(omniSetting);
            }
        }

        public static SettingType Resolve(string guid)
        {
            if (guid == generic)
                return Generic;

            if (guid == uIFieldVisisbility)
                return UIFieldVisisbility;

            if (guid == omniSetting)
                return OmniSetting;

            if (guid == uiSetting)
                return UISetting;

            return null;
        }

        public static SettingType Resolve(Guid guid)
        {
            return Resolve(guid.ToString().ToUpperInvariant());
        }

        public static bool operator ==(SettingType a, SettingType b)
        {
            return ReferenceEquals(a, b) || !((ReferenceEquals(a, null) || ReferenceEquals(b, null)) || a.Type != b.Type);
        }

        public static bool operator !=(SettingType a, SettingType b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is SettingType && this == (SettingType) obj;
        }
    }
}
