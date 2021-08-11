using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSRetail.DD.Common.Scheduler;

namespace LSOne.ViewPlugins.Scheduler.Interfaces
{
    /// <summary>
    /// Provides a <see cref="UserControl"/> and the paremeter dictionary required to configure a <see cref="ISchedulerAction"/> external command plugin.
    /// <para />
    /// The order in which the Site Manager calls the methods are:
    /// <para />
    /// 1: InitilizeParameters
    /// <para />
    /// 2: CreateSetupControl
    /// <para />
    /// 3: DataIsModified
    /// <para />
    /// 4: GetParemeters
    /// </summary>
    public interface ISchedulerActionSetupControlProvider
    {

        /// <summary>
        /// Initializes the dictionary in <paramref name="parameters"/> with default values if they are not present. This method should add and initialize all the default key/value pairs if they do not exist.
        /// </summary>
        /// <param name="parameters">The current setup parameter values. </param>
        void InitializeParameters(Dictionary<string, string> parameters);

        /// <summary>
        /// Creates an instance of the setup control and passes a connection to the database to it.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="parameters">The current setup parameter values. This is used to initialize the values on the control</param>
        /// <returns></returns>
        UserControl CreateSetupControl(IConnectionManager entry, Dictionary<string, string> parameters);

        /// <summary>
        /// Creates and returns the parameter dictionary based on the configuration on the control. This is called if <see cref="DataIsModified"/> returns true
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetParemeters();

        /// <summary>
        /// Goes through the current configuration of the setup-control and returns true if the configuration has changed from when <see cref="CreateSetupControl"/> was called. If this returns
        /// "true" then the function <see cref="GetParemeters"/> is called.
        /// </summary>
        /// <returns></returns>
        bool DataIsModified();
    }
}
