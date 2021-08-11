using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IOrderItem : ICloneable, ISerializable
    {
        decimal Ordered { get; set; }
        decimal Received { get; set; }
        decimal ToPickUp { get; set; }
        List<IDepositItem> Deposits { get; set; }
        bool FullyReceived { get; set; }
        Date DateFullyReceived { get; set; }
        decimal ReservationQty { get; set; }
        Date DateReserved { get; set; }

        bool ReservationDone { get; set; }
        RecordIdentifier JournalID { get; set; }
        Guid SplitIdentifier { get; set; }

        bool Empty();

        void Clear();

        /// <summary>
        /// Returns all deposists with status Normal and Distributed (<see cref="DepositsStatus"/>) that have not yet been paid
        /// </summary>
        /// <returns></returns>
        decimal DepositToBePaid();

        /// <summary>
        /// Returns all deposists with a specific status (<see cref="DepositsStatus"/>) that have not yet been paid
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        decimal DepositToBePaid(DepositsStatus status);

        /// <summary>
        /// Returns all deposits with status Normal  and Distributed (<see cref="DepositsStatus"/>) that have already been paid
        /// </summary>
        /// <returns></returns>
        decimal DepositAlreadyPaid();

        /// <summary>
        /// Returns all deposists with a specific status (<see cref="DepositsStatus"/>) that have already been paid
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        decimal DepositAlreadyPaid(DepositsStatus status);
        decimal TotalDepositAmount();
        void SetAllDepositsAsPaid();
        void SetAllDepositsStatus(DepositsStatus status);

        bool NewDepositsToBePaid();

        void SetDeposit(decimal deposit);

        void SetDeposit(decimal deposit, bool depositPaid);
    }
}
