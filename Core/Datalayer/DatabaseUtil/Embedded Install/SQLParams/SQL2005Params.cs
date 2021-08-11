using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.DatabaseUtil.EmbeddedInstall
{
    /// <summary>
    /// All parameters available to use when installing SQL 2005 Express server. The parameters that have to be used during the install all have default values.
    /// </summary>
    public class SQL2005Params : SQLParams
    {
        #region Properties

        /// <summary>
        /// The default value for the ADDLOCAL parameter necessary for the silent install 
        /// </summary>
        public string AddLocal;

        /// <summary>
        /// This parameter is used for setting the startup type of the network protocols. It has the following 3 options:
        ///     0 - Shared memory = On, Named Pipes = On, TCP/IP = On
        ///     1 - Shared memory = On, Named Pipes = Off (local only), TCP/IP = Off
        ///     2 - Shared memory = On, Named Pipes = Off (local only), TCP/IP = On
        /// Default value is 0    
        /// </summary>
        public int DisableNetworkProtocols { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor - AddLocal is set to ALL and DisableNetWorkProtocols is set to 0
        /// </summary>
        public SQL2005Params()
            : base()
        {
            AddLocal = "All";
            DisableNetworkProtocols = 0;
        }

        /// <summary>
        /// A constructor that sets the setup file location parameter
        /// </summary>
        /// <param name="SetupFileLocation"></param>
        public SQL2005Params(string SetupFileLocation)
            : base(SetupFileLocation)
        {
            AddLocal = "All";
            DisableNetworkProtocols = 0;
        }

        #endregion

    }
}
