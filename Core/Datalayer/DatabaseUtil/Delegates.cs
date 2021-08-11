namespace LSOne.DataLayer.DatabaseUtil
{
    /// <summary>
    /// Event to trigger when counter hit
    /// </summary>
    /// <param name="Sender"></param>
    /// <param name="dbVersion"></param>
    /// <returns></returns>
    public delegate bool PerformTrigger(string Sender, int dbVersion);

    /// <summary>
    /// When using the Database Utility the program can subscribe to the MessageCallback function which will receive information about progress and what is going on within the DB Utility. This is done instead of having a user interface with progress information.
    /// </summary>
    /// <param name="Sender">A string identifying the source of the message</param>
    /// <param name="Msg">The message being sent</param>
    public delegate void MessageCallback(string Sender, string Msg);
  
    /// <summary>
    /// When installing the SQL 2017 Express server all information regarding the installation can be viewed by subscribing to this message callback
    /// </summary>
    /// <param name="Sender">A string identifying the source of the message</param>
    /// <param name="Msg">The message being sent</param>
    public delegate void InstallingSQL2017MessageCallback(string Sender, string Msg);

    /// <summary>
    /// Provides information on where in the database create/update process we currently are
    /// </summary>
    /// <param name="headerMessage">The header message or caption describing the current step</param>
    /// <param name="message">The message for the current step</param>
    /// <param name="total">The total number of steps that need to be performed</param>
    /// <param name="current">The number of the current step</param>
    public delegate void UpdateDatabaseProgressCallback(string headerMessage, string message, int total, int current);
}