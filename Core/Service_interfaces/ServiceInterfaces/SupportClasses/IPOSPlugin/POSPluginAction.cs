using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.SupportClasses.IPOSPlugin
{
    /// <summary>
    /// Used by <see cref="IPOSPluginSetupProvider"/> to describe a single task and it's parameter type. This is used when configuring an <see cref="IPOSPlugin"/> on a button to allow the user
    /// select the type of parameter rather than manually entering all the information.
    /// </summary>
    public class POSPluginTask
    {
        /// <summary>
        /// The description of the action
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// The ID of the action. This is passed to <see cref="LSOne.Services.Interfaces.IPOSPlugin.RunTask(DataLayer.GenericConnector.Interfaces.IConnectionManager, SupportInterfaces.ISession, SupportInterfaces.ISettings, SupportInterfaces.IPosTransaction, DataLayer.BusinessObjects.OperationInfo, string, List{string})"/>
        /// </summary>
        public string TaskID { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="LookupTypeEnum"/> that should be used when configuring this task. This will set what kind of parameter the user can configure in button configuration dialog for the POS.
        /// </summary>
        public LookupTypeEnum LookupType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="POSOperations"/> that should be used when configuring this task. This will set which parameters will be shown in the button cofiguration dialog according to the <see cref="LookupType"/>
        /// </summary>
        public POSOperations PosOperation { get; set; }
    }
}
