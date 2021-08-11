using System;

namespace LSOne.DataLayer.DatabaseUtil.ScriptInformation
{
    /// <summary>
    /// Stores information about the SQL Script version; Major, Minor, Service Pack and Database Version
    /// </summary>
    public class DatabaseVersion
    {        

        /// <summary>
        /// The version the database should be set to after the current script has been run
        /// </summary>
        public int DbVersion { get; set; }

        /// <summary>
        /// The partner version the database should be set to after the current script has been run
        /// </summary>
        public int PartnerVersion { get; set; }

        /// <summary>
        /// Default constructor. Version set to 00000-00
        /// </summary>
        public DatabaseVersion()
        {            
            DbVersion = 0;
            PartnerVersion = 0;
        }

        /// <summary>
        /// Parses a string to fit into the different version types
        /// </summary>
        /// <param name="version">A version string such as "00051-09"</param>
        public void ParseVersion(string version)
        {
            if (version == "")
            {
                return;
            }

            string[] split = version.Split(new Char[] { '-' });

            for (int i = 0; i < split.Length; i++)
            {
                try
                {
                    if (i == 0) { DbVersion = Convert.ToInt32(split[i]); }
                    if (i == 1) { PartnerVersion = Convert.ToInt32(split[i]); }                    
                }
                catch (FormatException)
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Returns a version string on the format DDDDD-PP
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DbVersion.ToString().PadLeft(5, '0') + "-" + PartnerVersion.ToString().PadLeft(2, '0');
        }
    }

}
