using LSOne.DataLayer.DatabaseUtil.Enums;

namespace LSOne.DataLayer.DatabaseUtil.ScriptInformation
{
    /// <summary>
    /// Holds information about each embedded SQL Script
    /// </summary>
    public class ScriptInfo
    {
        #region Properties

        /// <summary>
        /// The string that all system styles scripts must have in their name
        /// </summary>
        public const string SystemStylesConst = "SystemStyles";
        /// <summary>
        /// The string that all defult form profile scripts must have in their name
        /// </summary>
        public const string DefaultFormProfileConst = "DefaultFormProfile";

        /// <summary>
        /// Resource name of the script
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// The name of the script itself
        /// </summary>
        public string ScriptName { get; set; }

        /// <summary>
        /// If upgrade script then which version is the upgrade script
        /// </summary>
        public DatabaseVersion Version { get; set; }

        /// <summary>
        /// When should the script be run
        /// </summary>
        public RunScripts ScriptType { get; set; }

        /// <summary>
        /// The subtype of the script i.e. Audit or Normal
        /// </summary>
        public ScriptSubType ScriptSubType { get; set; }

        /// <summary>
        /// If true it's a Logic script
        /// </summary>
        public bool IsLogicScript { get; set; }

        /// <summary>
        /// True if this is an external script
        /// </summary>
        public bool IsExternalScript { get; set; }


        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor. 
        /// </summary>
        public ScriptInfo()
        {
            ScriptName = "";
            Version = new DatabaseVersion();
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Parses the resource name and sets the Script info depending on what type of SQL script it is i.e. update script, logic script and etc.
        /// </summary>
        /// <param name="resourceName"></param>
        public void ParseResourceName(string resourceName)
        {
            string strSQLScripts = ".SQLScripts.";
            string strUpdateDatabase = strSQLScripts + "Update_Database.";
            string strUpdateAuditDatabase = strSQLScripts + "Update_Database_Audit.";
            string strLogicScripts = strSQLScripts + "Logic_Scripts.";
            string strCloudScripts = strSQLScripts + "Cloud_Scripts.";
            string strCloudScriptsBackOffice = strSQLScripts + "Cloud_Scripts_Back_Office.";
            string strLogicScriptsAudit = strSQLScripts + "Logic_Scripts_Audit.";
            string strCreateDatabase = strSQLScripts + "Create_Database.";
            string strCreateDatabaseAudit = strSQLScripts + "Create_Database_Audit.";
            //string strLanguageTexts = strSQLScripts + "Language_Texts.";
            string strUsers = strSQLScripts + "Users.";
            string strUsersAudit = strSQLScripts + "Users_Audit.";
            string strAdmin = strSQLScripts + "Create_Admin.";

            string strImportData = ".ImportData.";
            string strSystemData = strImportData + "SystemData.";
            string strDefaultData = strImportData + "DefaultData.";

            string strVersion2011 = strUpdateDatabase + "LS_Retail_.NET_2011.";
            string strVersion2011_1 = strUpdateDatabase + "LS_Retail_.NET_2011._1.";
            string strVersion2011_1_1 = strUpdateDatabase + "LS_Retail_.NET_2011._1._1.";
            string strVersion2011_2 = strUpdateDatabase + "LS_Retail_.NET_2011._2.";
            string strVersion2011_2_1 = strUpdateDatabase + "LS_Retail_.NET_2011._2._1.";
            string strVersion2012 = strUpdateDatabase + "LS_Retail_.NET_2012.";
            string strVersion2012_1 = strUpdateDatabase + "LS_Retail_.NET_2012._1.";
            string strVersion2013 = strUpdateDatabase + "LS_Retail_.NET_2013.";
            string strVersion2013_1 = strUpdateDatabase + "LS_One_2013._1.";
            string strVersion2013_2 = strUpdateDatabase + "LS_One_2013._2.";
            string strVersion2014 = strUpdateDatabase + "LS_One_2014.";
            string strVersion2015 = strUpdateDatabase + "LS_One_2015.";
            string strVersion2016 = strUpdateDatabase + "LS_One_2016.";
            string strVersion2016_1 = strUpdateDatabase + "LS_One_2016._1.";
            string strVersion2017 = strUpdateDatabase + "LS_One_2017.";
            string strVersion2017_1 = strUpdateDatabase + "LS_One_2017._1.";
            string strVersion2017_2 = strUpdateDatabase + "LS_One_2017._2.";
            string strVersion2019 = strUpdateDatabase + "LS_One_2019.";
            string strVersion2019_1 = strUpdateDatabase + "LS_One_2019._1.";
            string strVersion2020 = strUpdateDatabase + "LS_One_2020.";
            string strVersion2020_1 = strUpdateDatabase + "LS_One_2020._1.";
            string strVersion2021 = strUpdateDatabase + "LS_One_2021.";

            #region SQL Scripts


            if (resourceName.Contains(strSQLScripts))
            {
                this.ResourceName = resourceName;
                this.IsLogicScript = false;
                this.ScriptSubType = ScriptSubType.Normal;

                //Update database scripts
                if (resourceName.Contains(strUpdateDatabase))
                {
                    this.ScriptType = RunScripts.UpdateDatabase;

                    //When adding new folders, make sure that the if statements will not check a string that may be contained in multiple folders
                    //Ex: 2017.1 must be checked before 2017
                    if (resourceName.Contains(strVersion2011_2_1))
                    {
                        strUpdateDatabase = strVersion2011_2_1;
                    }
                    else if (resourceName.Contains(strVersion2011_2))
                    {
                        strUpdateDatabase = strVersion2011_2;
                    }
                    else if (resourceName.Contains(strVersion2011_1_1))
                    {
                        strUpdateDatabase = strVersion2011_1_1;
                    }
                    else if (resourceName.Contains(strVersion2011_1))
                    {
                        strUpdateDatabase = strVersion2011_1;
                    }
                    else if (resourceName.Contains(strVersion2011))
                    {
                        strUpdateDatabase = strVersion2011;
                    }
                    else if (resourceName.Contains(strVersion2012_1))
                    {
                        strUpdateDatabase = strVersion2012_1;
                    }
                    else if (resourceName.Contains(strVersion2012))
                    {
                        strUpdateDatabase = strVersion2012;
                    }
                    else if (resourceName.Contains(strVersion2013))
                    {
                        strUpdateDatabase = strVersion2013;
                    }
                    else if (resourceName.Contains(strVersion2013_1))
                    {
                        strUpdateDatabase = strVersion2013_1;
                    }
                    else if (resourceName.Contains(strVersion2013_2))
                    {
                        strUpdateDatabase = strVersion2013_2;
                    }
                    else if (resourceName.Contains(strVersion2014))
                    {
                        strUpdateDatabase = strVersion2014;
                    }
                    else if (resourceName.Contains(strVersion2015))
                    {
                        strUpdateDatabase = strVersion2015;
                    }
                    else if (resourceName.Contains(strVersion2016_1))
                    {
                        strUpdateDatabase = strVersion2016_1;
                    }
                    else if (resourceName.Contains(strVersion2016))
                    {
                        strUpdateDatabase = strVersion2016;
                    }
                    else if (resourceName.Contains(strVersion2017_1))
                    {
                        strUpdateDatabase = strVersion2017_1;
                    }
                    else if (resourceName.Contains(strVersion2017_2))
                    {
                        strUpdateDatabase = strVersion2017_2;
                    }
                    else if (resourceName.Contains(strVersion2017))
                    {
                        strUpdateDatabase = strVersion2017;
                    }
                    else if (resourceName.Contains(strVersion2019_1))
                    {
                        strUpdateDatabase = strVersion2019_1;
                    }
                    else if (resourceName.Contains(strVersion2019))
                    {
                        strUpdateDatabase = strVersion2019;
                    }
                    else if (resourceName.Contains(strVersion2020_1))
                    {
                        strUpdateDatabase = strVersion2020_1;
                    }
                    else if (resourceName.Contains(strVersion2020))
                    {
                        strUpdateDatabase = strVersion2020;
                    }
                    else if (resourceName.Contains(strVersion2021))
                    {
                        strUpdateDatabase = strVersion2021;
                    }

                    //Figure out the script name itself
                    int start = resourceName.IndexOf(strUpdateDatabase) + strUpdateDatabase.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);

                    string version = this.ScriptName.ToUpperInvariant().Replace(".SQL", "");

                    //From the script name - figure out the version
                    this.Version.ParseVersion(version);
                }

                else if (resourceName.Contains(strUpdateAuditDatabase))
                {
                    this.ScriptType = RunScripts.UpdateDatabase;
                    this.ScriptSubType = ScriptSubType.Audit;

                    //Figure out the script name itself
                    int start = resourceName.IndexOf(strUpdateAuditDatabase) + strUpdateAuditDatabase.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);

                    string version = this.ScriptName.ToUpperInvariant().Replace(".SQL", "");

                    //From the script name - figure out the version
                    this.Version.ParseVersion(version);
                }

                //Logic scripts or Stored procedures
                else if (resourceName.Contains(strLogicScripts))
                {
                    this.IsLogicScript = true;
                    this.ScriptType = RunScripts.LogicScripts;

                    int start = resourceName.IndexOf(strLogicScripts) + strLogicScripts.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);

                }

                //Logic scripts or Stored procedures for Audit db
                else if (resourceName.Contains(strLogicScriptsAudit))
                {
                    this.IsLogicScript = true;
                    this.ScriptType = RunScripts.LogicScripts;
                    this.ScriptSubType = Enums.ScriptSubType.Audit;

                    int start = resourceName.IndexOf(strLogicScriptsAudit) + strLogicScriptsAudit.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);

                }

                //Create database
                else if (resourceName.Contains(strCreateDatabase))
                {
                    this.ScriptType = RunScripts.CreateDatabase;

                    int start = resourceName.IndexOf(strCreateDatabase) + strCreateDatabase.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);
                }

                //Create audit database
                else if (resourceName.Contains(strCreateDatabaseAudit))
                {
                    this.ScriptType = RunScripts.CreateDatabase;
                    this.ScriptSubType = ScriptSubType.Audit;

                    int start = resourceName.IndexOf(strCreateDatabaseAudit) + strCreateDatabaseAudit.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);
                }

                     //Cloud Acripts
                else if (resourceName.Contains(strCloudScripts))
                {
                    this.ScriptType = RunScripts.CloudScripts;
                    this.ScriptSubType = Enums.ScriptSubType.Normal;

                    int start = resourceName.IndexOf(strCloudScripts) + strCloudScripts.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);

                }

                //Cloud scripts only on HBO
                else if (resourceName.Contains(strCloudScriptsBackOffice))
                {
                    this.ScriptType = RunScripts.CloudScripts;
                    this.ScriptSubType = ScriptSubType.HBOOnly;

                    int start = resourceName.IndexOf(strCloudScriptsBackOffice) + strCloudScriptsBackOffice.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);
                }


                //Language texts
                //else if (resourceName.Contains(strLanguageTexts))
                //{
                //    this.ScriptType = RunScripts.LanguageText;

                //    int start = resourceName.IndexOf(strLanguageTexts) + strLanguageTexts.Length;
                //    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);
                //}

                //DB Users
                else if (resourceName.Contains(strUsers))
                {
                    this.ScriptType = RunScripts.Users;

                    int start = resourceName.IndexOf(strUsers) + strUsers.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);
                }

                //Audit DB USers
                else if (resourceName.Contains(strUsersAudit))
                {
                    this.ScriptType = RunScripts.Users;
                    this.ScriptSubType = Enums.ScriptSubType.Audit;

                    int start = resourceName.IndexOf(strUsersAudit) + strUsersAudit.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);
                }

                //Admin script
                else if (resourceName.Contains(strAdmin))
                {
                    this.ScriptType = RunScripts.AdminScript;

                    int start = resourceName.IndexOf(strAdmin) + strAdmin.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);
                }
            }
            #endregion

            #region Import Data

            else if (resourceName.Contains(strImportData))
            {
                #region System Data
                if (resourceName.Contains(strSystemData))
                {
                    this.ResourceName = resourceName;
                    this.IsLogicScript = false;
                    this.ScriptType = RunScripts.SystemData;

                    //Figure out the script name itself
                    int start = resourceName.IndexOf(strSystemData) + strSystemData.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);
                }
                #endregion

                #region Default Data

                else if (resourceName.Contains(strDefaultData))
                {
                    this.ResourceName = resourceName;
                    this.IsLogicScript = false;
                    this.ScriptType = RunScripts.DefaultData;

                    //Figure out the script name itself
                    int start = resourceName.IndexOf(strDefaultData) + strDefaultData.Length;
                    this.ScriptName = resourceName.Substring(start, resourceName.Length - start);
                }

                #endregion
            }

            #endregion
        }

        #endregion

    }
}
