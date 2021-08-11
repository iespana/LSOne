using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// The options that can be set in the Site service profile on the behaviour of sending receipts through emails
    /// </summary>
    public enum ReceiptEmailOptionsEnum
    {
        /// <summary>
        /// Emails are never sent
        /// </summary>
        Never = 0,
        /// <summary>
        /// The user is always prompted if the receipt should be sent as email
        /// </summary>
        Always = 1,
        /// <summary>
        /// The user is prompted if a customer is on the sale
        /// </summary>
        OnlyToCustomers = 2,
        /// <summary>
        /// The user is never prompted but can be sent through an operation on the POS
        /// </summary>
        OnRequest = 3
    }
}
