using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSRetail.SiteService.IntegrationFrameworkRetailItemInterface.DTO")]
namespace LSRetail.SiteService.IntegrationFrameworkRetailItemInterface.DTO
{    
    /// <summary>
    /// Contains information about an assembly that should be saved via the integration framework
    /// </summary>
    public class Assembly
    {
        /// <summary>
        /// The item ID of the assembly item
        /// </summary>
        public string AssemblyItemID { get; set; }

        /// <summary>
        /// The list of components that belong to this assembly
        /// </summary>
        public List<AssemblyComponent> Components { get; set; }
    }
}
