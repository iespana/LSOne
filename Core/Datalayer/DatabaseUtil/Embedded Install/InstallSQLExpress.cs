namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// An abstract classs which InstallSQL2005Expr and InstallSQL2008Expr classes inherit from.
    /// </summary>
    public abstract class InstallSQLExpress
    {       

        #region Constructor

        /// <summary>
        /// A default constructor
        /// </summary>
        public InstallSQLExpress()
        {
           
        }         
        

        #endregion

        #region Virtual functions

        /// <summary>
        /// Uses the MessageCallbackHandler delegate to send messages to whoever is subscribing
        /// </summary>
        /// <param name="msg">The message to be sent</param>
        public virtual void SendMessage(string msg)
        {            
        }

        /// <summary>
        /// After the SQL Server has been installed this will return the instance name of the new SQL Server
        /// </summary>
        /// <returns></returns>
        public virtual string SqlServerName()
        {
            return "";
        }

        /// <summary>
        /// Returns the command prompt string created using the current settings
        /// </summary>
        /// <returns></returns>
        public virtual string GetCommandPromptString()
        {
            return "";
        }

        #endregion        

    }
}
