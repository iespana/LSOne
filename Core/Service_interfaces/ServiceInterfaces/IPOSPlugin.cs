using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses.IPOSPlugin;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Defines a plugin for the POS that can be executed from a button when running the operation "Execute POS plugin"
    /// </summary>
    public interface IPOSPlugin
    {
        /// <summary>
        /// Called by the POS when the user runs the "Execute POS plugin". The POS will supply the database connection, session, current transaction and the task- and parameters that were selected on the button.
        /// </summary>
        /// <param name="entry">The connection to the database</param>
        /// <param name="session">The current session which the transaction belongs to</param>
        /// <param name="settings">Application settings for the running application</param>
        /// <param name="transaction">The current transaction</param>
        /// <param name="operationInfo">Contains various information from the POS about the current state for the operation</param>
        /// <param name="task">The ID of the task to run. This will be what the user selected in the button properties dialog</param>
        /// <param name="args">Zero or more arguments as selected- or entered by the user</param>
        /// <returns></returns>
        IPosTransaction RunTask(IConnectionManager entry, ISession session, ISettings settings, IPosTransaction transaction, OperationInfo operationInfo, string task, List<string> args);
    }

    /// <summary>
    /// Provides information about an <see cref="IPOSPlugin"/> to display to the user when he is configuring a button for the POS. If a <see cref="IPOSPlugin"/> does not implement this interface then the user must
    /// manually enter all the relevant information.
    /// </summary>
    public interface IPOSPluginSetupProvider
    {
        /// <summary>
        /// Returns the list of tasks that the <see cref="IPOSPlugin"/> can execute.
        /// </summary>
        List<POSPluginTask> PluginTasks { get; }
    }
}
