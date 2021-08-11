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
    /// Represents a single compoment belonging to an assembly
    /// </summary>
    public class AssemblyComponent
    {
        /// <summary>
        /// The item ID of the component
        /// </summary>
        public string ComponentItemID { get; set; }

        /// <summary>
        /// The quantity of the component
        /// </summary>
        public decimal Quantity { get; set; }
    }
}
