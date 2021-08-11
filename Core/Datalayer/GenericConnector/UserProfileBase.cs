using System;
using System.Collections.Generic;
using System.Text;
using LSOne.Utilities.Cryptography;

namespace LSOne.DataLayer.GenericConnector
{
    public abstract class UserProfileBase
    {
        protected bool Valid { get; set; }

        private readonly SortedList<string, object> permissions;
        private readonly SortedList<Guid, string> settingsValues;

        public UserProfileBase()
        {
            Valid = false;
            permissions = new SortedList<string, object>();
            settingsValues = new SortedList<Guid, string>();
        }

        public SortedList<string, object> Permissions
        {
            get
            {
                return permissions;
            }
        }

        public SortedList<Guid, string> Settings
        {
            get
            {
                return settingsValues;
            }
        }

        public string HashCode
        {
            get
            {
                if (Valid)
                {
                    var sb = new StringBuilder(4096);

                    foreach (string stringGuid in permissions.Keys)
                    {
                        sb.EnsureCapacity(sb.Length + stringGuid.Length + 1);
                        sb.Append(stringGuid);
                    }

                    foreach (var key in settingsValues.Keys)
                    {
                        var text = settingsValues[key];

                        sb.EnsureCapacity(sb.Length + text.Length + 41);
                        sb.Append(key.ToString());
                        sb.Append(text);
                    }

                    return MD5.GetValue(sb.ToString());
                }
                return "";
            }
        }
    }
}
