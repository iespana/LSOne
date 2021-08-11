using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    /// <summary>
    /// An object that holds information about the deposits that to be paid on the item and if they have been paid or not
    /// </summary>
    public interface IDepositItem : ICloneable, ISerializable
    {
        /// <summary>
        /// The calculated deposit on the item
        /// </summary>
        decimal Deposit { get; set; }
        /// <summary>
        /// If true then the deposit has already been paid
        /// </summary>
        bool DepositPaid { get; set; }

        /// <summary>
        /// The current status of the deposit
        /// </summary>
        DepositsStatus Status { get; set; }

        /// <summary>
        /// If true then the deposit instance is empty
        /// </summary>
        /// <returns></returns>
        bool Empty();

        /// <summary>
        /// Resets the values of all the variables in the object
        /// </summary>
        void Clear();
    }
}
