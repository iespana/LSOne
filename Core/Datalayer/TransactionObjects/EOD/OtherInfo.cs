using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.TransactionObjects.EOD
{
    /// <summary>
    /// Declares the class NoSaleInfo. A linked list of that type is created in the constructor.
    /// </summary>
    public class OtherInfo
    {
        /// <summary>
        /// For text that shall appear in the Report but is not relevant to a certain sales operation.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// For amounts that shall appear in the Report but is not relevant to a certain sales operation.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// For qty that shall appear in the Report but is not relevant to a certain sales operation.
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// For amounts including tax that shall appear in the Report but is not relevant to a certain sales operation.
        /// </summary>
        public decimal AmountInclTax { get; set; }

        /// <summary>
        /// What type of information is this
        /// </summary>
        public ZReportOtherInfoEnum InfoType { get; set; }

        public OtherInfo()
        {
            Clear();
        }

        public void Clear()
        {
            Text = "";
            Amount = decimal.Zero;
            Qty = decimal.Zero;
            AmountInclTax = decimal.Zero;
            InfoType = ZReportOtherInfoEnum.None;
        }

    }
}
