using System;
using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Transactions.Line
{
    [Flags]
    public enum ExcludedActions
    {
        VoidItem = 0x01,
        SetQuantity = 0x02,
        ClearQuantity = 0x04,
        ChangeUnitOfMeasure = 0x08,
        PriceOverride = 0x10,
        ChangeHospitalityMenuType = 0x20,
        ChangeItemComment = 0x40
    }

    public interface ILineItem : ICloneable
    {
        /// <summary>
        /// The unique id of each line in the transaction
        /// </summary>
        int LineId { get; set; }

        /// <summary>
        /// Is set to true if line has been voided
        /// </summary>
        bool Voided { get; set; }

        /// <summary>
        /// Has the item been changed since it was delivered for preparation
        /// </summary>
        bool ChangedForPreparation { get; set; }

        /// <summary>
        /// A description of the line item
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// A description alias of the line item
        /// </summary>
        string DescriptionAlias { get; set; }

        /// <summary>
        /// A description of the line item in an alternate language
        /// </summary>
        string DescriptionAltLanguage { get; set; }

        /// <summary>
        /// The start date and time of the transaction.
        /// </summary>
        DateTime BeginDateTime { get; set; }

        /// <summary>
        /// The end date and time of the transaction.
        /// </summary>
        DateTime EndDateTime { get; set; }

        List<InfoCodeLineItem> InfoCodeLines { get; set; }

        ExcludedActions ExcludedActions { get; set; }

        void Add(InfoCodeLineItem infoCodeLineItem);
    }
}
