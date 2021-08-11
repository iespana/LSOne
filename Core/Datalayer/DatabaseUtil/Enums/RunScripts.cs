
namespace LSOne.DataLayer.DatabaseUtil.Enums
{    

    /// <summary>
    /// Which SQL scripts should be run, one or many options can be selected
    /// </summary>
    public enum RunScripts : int
    {
        /// <summary>
        /// All scripts available (when applicable) will be run
        /// </summary>
        All = 0x1DE,
        /// <summary>
        /// The Create Database script will be run providing that the database to be created doesn't already exist
        /// </summary>
        CreateDatabase = 0x2,        
        /// <summary>
        /// The Logic Scripts will be run. The Logic scripts will be run by default if UpdateDatabase is selected
        /// </summary>
        LogicScripts = 0x4,
        /// <summary>
        /// The database will be updated if necessary using the update SQL scripts
        /// </summary>
        UpdateDatabase = 0x8,
        /// <summary>
        /// The User scripts will be run
        /// </summary>
        Users = 0x10,
        /// <summary>
        /// Cloud scripts will be run
        /// </summary>
        CloudScripts = 0xA,
        /// <summary>
        /// Language Text script will be run
        /// </summary>
        LanguageText = 0x20,
        /// <summary>
        /// The demo data scripts will be run
        /// </summary>
        DemoData = 0x40,
        /// <summary>
        /// The Admin Scripts will be run
        /// </summary>
        AdminScript = 0x80,
        /// <summary>
        /// The default data scripts - never run automatically. Have to be run specifically using RunSpecificScript function
        /// </summary>
        DefaultData = 0x100,
        /// <summary>
        /// The system data scripts will be run
        /// </summary>
        SystemData = 0x200
    }

    /// <summary>
    /// Some scripts should only apply to Audit databases. 
    /// </summary>
    public enum ScriptSubType
    { 
        /// <summary>
        /// Scripts that only apply to Normal database
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Scripts that only apply to Audit database
        /// </summary>
        Audit = 1,
        /// <summary>
        /// Only applied on the cloud database
        /// </summary>
        HBOOnly = 2
    }
     
}
