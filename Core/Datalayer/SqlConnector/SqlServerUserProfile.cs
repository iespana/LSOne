using System;
using System.Data;
using System.IO;
using LSOne.DataLayer.GenericConnector;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.IO;

namespace LSOne.DataLayer.SqlConnector
{
    public class SqlServerUserProfile : UserProfileBase
    {
        #region Constructors
        public SqlServerUserProfile()
        {
        }

        public SqlServerUserProfile(IDataReader dr)
            : this()
        {
            string serial = "";

            while (dr.Read())
            {
                Settings.Add((Guid)dr["GUID"], (string)dr["Value"]);
            }

            if (Settings.ContainsKey(GenericConnector.Constants.Settings.DatabaseSerialNumber))
            {
                serial = Settings[GenericConnector.Constants.Settings.DatabaseSerialNumber];
            }

            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Permissions.Add(
                        (bool)dr["CodeIsEncrypted"] ? 
                            Cipher.Decrypt((string)dr["PermissionCode"], serial) :    
                            (string)dr["PermissionCode"],
                        null);
                }
            }

            Valid = true;
        }

        /*public UserProfile(UserPermissions userPermissions, SortedList<string, string> settings)
        {
            valid = true;
            permissions = userPermissions.Permissions;
            settingsValues = settings;
        }*/

        private SqlServerUserProfile(FolderItem userProfileFile)
            : this()
        {
            FileStream stream = null;
            BinaryReader reader = null;

            try
            {
                stream = File.OpenRead(userProfileFile.AbsolutePath);
                reader = new BinaryReader(stream);

                int count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    Permissions.Add(reader.ReadString(), null);
                }

                count = reader.ReadInt32();

                for (int i = 0; i < count; i++)
                {
                    Settings.Add(new Guid(reader.ReadString()), reader.ReadString());
                }
            }
            catch
            {
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                if (stream != null)
                {
                    stream.Close();
                }
            }

            Valid = true;
        }
        #endregion // Constructors

        

        public static SqlServerUserProfile LoadProfile(string userName)
        {
            var f = GetProfileLocation(userName);
            return (f.Exists) ? new SqlServerUserProfile(f) : new SqlServerUserProfile();
        }

        private static FolderItem GetProfileLocation(string userName)
        {
            var specialFolders = new[]
                                     {
                                         Environment.SpecialFolder.ApplicationData,
                                         Environment.SpecialFolder.LocalApplicationData,
                                         Environment.SpecialFolder.CommonApplicationData
                                     };
            int folder = 0;
            FolderItem userProfileDirectory = null;
            while (userProfileDirectory == null && folder < specialFolders.Length)
            {
                userProfileDirectory = FolderItem.GetSpecialFolder(specialFolders[folder++]);
                userProfileDirectory = userProfileDirectory.Child("LSRetail");
            }

            if (userProfileDirectory == null)
            {
                string folderList = "";
                foreach (var specialFolder in specialFolders)
                {
                    if (folderList.Length > 0)
                        folderList += ", ";
                    folderList += specialFolder.ToString();
                }
                throw new ApplicationException(string.Format("User '{0}' does not have access to any of the following special folders '{1}'",
                    userName, folderList));
            }

            if (!userProfileDirectory.Exists)
            {
                userProfileDirectory.CreateAsFolder();
            }

            userProfileDirectory = userProfileDirectory.Child("Client User Profiles");

            if (!userProfileDirectory.Exists)
            {
                userProfileDirectory.CreateAsFolder();
            }

            var userProfileFile = userProfileDirectory.Child(userName + ".profile");

            return userProfileFile;
        }

        

        public void Save(string userName)
        {
            FileStream stream = null;
            BinaryWriter writer = null;

            try
            {
                var f = GetProfileLocation(userName);

                if (f.Exists)
                {
                    f.Delete();
                }

                stream = File.Create(f.AbsolutePath);
                writer = new BinaryWriter(stream);

                writer.Write(Permissions.Count);

                foreach (string value in Permissions.Keys)
                {
                    writer.Write(value);
                }

                writer.Write(Settings.Count);

                foreach (Guid key in Settings.Keys)
                {
                    writer.Write(key.ToString());
                    writer.Write(Settings[key]);
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }

                if (stream != null)
                {
                    stream.Close();
                }
            }
        }
      
    }
}
