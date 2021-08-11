using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security;
using LSOne.Utilities.Cryptography;

namespace LSOne.DataLayer.GenericConnector
{
    public class ActiveDirectory
    {
        private static DirectoryEntry GetDirectoryObject(string userName, SecureString password, string domainIP)
        {
            DirectoryEntry entry;

            entry = new DirectoryEntry("LDAP://" + domainIP, userName, SecureStringHelper.ToString(password), AuthenticationTypes.Secure);

            return entry;
        }

        public static bool AuthenticateUser(string userName, SecureString password, string domainIP)
        {
            try
            {
                DirectoryEntry de = GetDirectoryObject(userName, password, domainIP);
                object o = de.NativeObject;
                de.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal static List<string> GetUserGroups(string userName, SecureString password, string domainIP, ref string fullName)
        {
            List<string> groups;
            string dn;
            DirectoryEntry de = GetDirectoryObject(userName, password, domainIP);
            DirectorySearcher deSearch = new DirectorySearcher();
            deSearch.SearchRoot = de;
            deSearch.Filter = "(&(objectCategory=person)(objectClass=user) (samaccountname=" + userName + "))";
            deSearch.PropertiesToLoad.Add("memberOf"); // Once found, get a list of Groups

            groups = new List<string>();

            try
            {
                SearchResult result = deSearch.FindOne();

                dn = (string)result.Path;
                dn = dn.Substring(dn.IndexOf("CN=") + 3); // Since the record provides raw information remove the unneeded information to get the true group name.
                fullName = dn.Substring(0, dn.IndexOf(","));

                int groupCount = result.Properties["memberOf"].Count; // Gets number of groups this user is a member of.

                for (int propertyCounter = 0; propertyCounter < groupCount; propertyCounter++) // Iterate through each record.
                {
                    dn = (string)result.Properties["memberOf"][propertyCounter]; // Assign dn to the record.
                    int equalsIndex = dn.IndexOf("=", 1);
                    int commaIndex = dn.IndexOf(",", 1);

                    if (equalsIndex == -1)
                    {

                    }
                    else
                    {
                        groups.Add(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    }


                }

                return groups;
            }
            catch (Exception)
            {
                return groups;
            }
        }
    }
}
