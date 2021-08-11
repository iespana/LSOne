using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [DataContract(Name = "SalesOrderResult")]
    public enum SalesOrderResult
    {
        /// <summary>
        /// Sales order function finished successfully
        /// </summary>
        [EnumMember]
        Success,
        /// <summary>
        /// The customer in the request is not found
        /// </summary>
        [EnumMember]
        CustomerNotFound,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_1,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_2,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_3,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_4,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_5,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_6,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_7,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_8,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_9,
        /// <summary>
        /// A result that can be used during implementation of this functionality
        /// Not used by LS One implementations
        /// </summary>
        [EnumMember]
        Customized_Result_10,
        /// <summary>
        /// There was some error during the sales order functionality
        /// </summary>
        [EnumMember]
        ErrorHandlingSalesOrder
    }   
    
}
