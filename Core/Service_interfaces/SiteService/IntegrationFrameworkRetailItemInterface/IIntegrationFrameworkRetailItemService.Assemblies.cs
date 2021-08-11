using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using LSRetail.SiteService.IntegrationFrameworkRetailItemInterface.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkRetailItemInterface
{
    public partial interface IIntegrationFrameworkRetailItemService
    {
        /// <summary>
        /// Saves a single <see cref="Assembly"/> and it's components to the database. If no assembly exists then a new one is created. If an assembly already exists
        /// for <see cref="Assembly.AssemblyItemID"/> then a new assembly is created and the existing assembly becomes inactive.
        /// </summary>
        /// <param name="assembly">The assembly to save</param>
        [OperationContract]
        void SaveAssembly(Assembly assembly);

        /// <summary>
        /// Saves a list of <see cref="Assembly"/> objects to the database. If no assembly exists then a new one is created. If an assembly already exists
        /// for <see cref="Assembly.AssemblyItemID"/> then a new assembly is created and the existing assembly becomes inactive.
        /// </summary>
        /// <param name="assemblies">The list of assemblies to save</param>
        [OperationContract]
        SaveResult SaveAssemblyList(List<Assembly> assemblies);
    }
}
