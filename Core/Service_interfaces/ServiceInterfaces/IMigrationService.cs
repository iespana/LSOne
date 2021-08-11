using System.Collections.Generic;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// When using the Database Utility the program can subscribe to the MessageCallback function which will receive information about progress and what is going on within the DB Utility. This is done instead of having a user interface with progress information.
    /// </summary>
    /// <param name="Sender">A string identifying the source of the message</param>
    /// <param name="Msg">The message being sent</param>
    public delegate void MessageCallback(string Sender, string Msg);
    public interface IMigrationService : IService
    {
        /// <summary>
        /// When using the Database Utility the program can subscribe to the MessageCallback function which will receive information about progress and what is going on within the DB Utility. This is done instead of having a user interface with progress information.
        /// </summary>
        /// 
        event MessageCallback MessageCallbackHandler;
        /// <summary>
        /// gets all supported targets
        /// </summary>
        List<int> MigrationTargets();

        /// <summary>
        /// Executes the relevant code for the supplied target
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="target"></param>
        bool ExecuteMigrationTarget(IConnectionManager entry, int target);

        /// <summary>
        /// Determines
        /// </summary>
        /// <param name="version">current version</param>
        /// <param name="maxVersion">targetversion</param>
        /// <returns></returns>
        bool MigrationWillBeRun(string version, string maxVersion);

    }
}
