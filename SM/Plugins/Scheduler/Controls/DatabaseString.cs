using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public class DatabaseString
    {
        public const string DatabaseParamsNullText = "none";    // Do not translate

        private Dictionary<string, string> properties = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        private DataSrvType dataSrvType = DataSrvType.Unknown;
        private string databaseParams;


        public DatabaseString(bool isDDString)
        {
            this.IsDDString = isDDString;
            this.PasswordPlaceholder = "****";
        }

        public DatabaseString(bool isDDString, string databaseString)
            : this(isDDString)
        {
            if (databaseString == null)
            {
                return;
            }

            string propertiesString;
            SplitMainParts(databaseString, out propertiesString, out dataSrvType, out databaseParams);

            var propertyParts = propertiesString.Split(';');
            foreach (var propertyPart in propertyParts)
            {
                var assignParts = propertyPart.Split('=');

                string name = null;
                string value = null;
                if (assignParts.Length >= 1)
                {
                    name = assignParts[0].Trim();
                    if (assignParts.Length >= 2)
                    {
                        value = assignParts[1].Trim();
                    }
                }
                if (!string.IsNullOrEmpty(name))
                {
                    properties.Add(name, value);
                }
            }
        }


        public bool IsDDString { get; private set; }

        public Dictionary<string, string> Properties
        {
            get { return properties; }
        }
        //public string this[string propertyName]
        //{
        //    get { return properties[propertyName]; }
        //    set { properties[propertyName] = value; }
        //}

        public bool HasPassword
        {
            get
            {
                return GetPasswordProperty() != null;
            }
        }

        public string Password
        {
            get
            {
                var property = GetPasswordProperty();
                if (property != null)
                {
                    return property.Value.Value;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                var property = GetPasswordProperty();
                if (property != null)
                {
                    properties[property.Value.Key] = value;
                }
                else
                {
                    throw new InvalidOperationException("No password property found.");
                }
            }
        }

        public DataSrvType DataSrvType
        {
            get { return dataSrvType; }
            set { dataSrvType = value; }
        }

        public string DatabaseParams
        {
            get { return databaseParams; }
            set { databaseParams = value; }
        }

        public string PasswordPlaceholder { get; set; }

        public override string ToString()
        {
            return ToString(false);
        }



        public string ToString(bool includePassword)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var property in properties)
            {
                sb.Append(property.Key);
                sb.Append("=");
                if (includePassword || !IsPassword(property.Key))
                {
                    sb.Append(property.Value);
                }
                else
                {
                    sb.Append(PasswordPlaceholder);
                }
                sb.Append(";");
            }

            if (IsDDString)
            {
                sb.Append("|");
                sb.Append(DataServerType.DataSrvTypeToString(dataSrvType));
                sb.Append("|");
                if (databaseParams != null)
                {
                    sb.Append(databaseParams);
                }
                else
                {
                    sb.Append(DatabaseParamsNullText);
                }
            }

            return sb.ToString();
        }


        public static bool Equals(DatabaseString databaseString, string s)
        {
            bool result;

            if (s == null)
            {
                result = databaseString == null;
            }
            else
            {
                if (databaseString != null)
                {
                    result = StringComparer.InvariantCultureIgnoreCase.Equals(databaseString.ToString(true), s);
                }
                else
                {
                    // s != null && databaseString == null
                    result = false;
                }
            }

            return result;
        }



        public string GetPropertyNameOfValue(string value)
        {
            foreach (var nvp in properties)
            {
                if (nvp.Value == value)
                {
                    return nvp.Key;
                }
            }

            return null;
        }



        private static void SplitMainParts(string databaseString, out string propertiesString, out DataSrvType dataSrvType, out string databaseParams)
        {
            //id={Code};company={Company};server={DBServerHost};nt={NetType};user={UserId};|fin|ndbcn@601
            //Provider=SQLOLEDB.1;Initial Catalog={DBPathName};Data Source={DBServerHost};User ID={UserId};Password={Password};|ms|none
            //Provider=SQLOLEDB.1;Initial Catalog={DBPathName};Data Source={DBServerHost};User ID={UserId};Password={Password};

            dataSrvType = DataSrvType.Unknown;
            databaseParams = null;

            var parts = databaseString.Split('|');
            if (parts.Length < 3)
            {
                propertiesString = databaseString;
                return;
            }

            int index = parts.Length - 1;
            databaseParams = parts[index--];
            dataSrvType = DataServerType.ParseDataSrvTypeString(parts[index--]);
            propertiesString = string.Concat(parts.Take(index + 1));
        }

        private bool IsPassword(string propertyName)
        {
            var sc = StringComparer.OrdinalIgnoreCase;
            return
                sc.Equals(propertyName, "password") ||
                sc.Equals(propertyName, "pwd") ||
                sc.Equals(propertyName, "database password");
        }


        private KeyValuePair<string, string>? GetPasswordProperty()
        {
            foreach (var property in properties)
            {
                if (IsPassword(property.Key))
                {
                    return property;
                }
            }

            return null;
        }


    }
}
