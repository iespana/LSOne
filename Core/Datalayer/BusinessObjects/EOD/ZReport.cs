using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.EOD
{    
    public class ZReport : DataEntity
    {
        #region Properties

        /// <summary>
        /// The unique ID for the Z Report
        /// </summary>
        public override RecordIdentifier ID { get; set; }
        /// <summary>
        /// Date when the Z Report was created
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// The Store where the Z report was created
        /// </summary>
        public RecordIdentifier StoreID { get; set; }
        /// <summary>
        /// The Terminal where the Z report was created
        /// </summary>
        public RecordIdentifier TerminalID { get; set; }
        /// <summary>
        /// The staff who was logged on when the Z report was created
        /// </summary>
        public RecordIdentifier StaffID { get; set; }
        /// <summary>
        /// The sum total of all sales
        /// </summary>
        public decimal TotalGrossAmount { get; set; }
        /// <summary>
        /// The sum total of all sales
        /// </summary>
        public decimal TotalNetAmount { get; set; }
        /// <summary>
        /// The sum total of all returns including tax
        /// </summary>
        public decimal TotalReturnGrossAmount { get; set; }
        /// <summary>
        /// The sum total of all returns
        /// </summary>
        public decimal TotalReturnNetAmount { get; set; }
        /// <summary>
        /// The total of all sales included in the Z report
        /// </summary>
        public decimal GrossAmount { get; set; }
        /// <summary>
        /// The total of all sales included in the Z report
        /// </summary>
        public decimal NetAmount { get; set; }
        /// <summary>
        /// The total of all returns included in the Z report
        /// </summary>
        public decimal ReturnGrossAmount { get; set; }
        /// <summary>
        /// The total of all returns included in the Z report
        /// </summary>
        public decimal ReturnNetAmount { get; set; }
        /// <summary>
        /// How the Z report was created
        /// </summary>
        public int EntryType { get; set; }
        /// <summary>
        /// To force the next Z Report ID. Only used by operation Initialize Z Report in LS POS.
        /// </summary>
        public int InitZReportID { get; set; }

        /// <summary>
        /// Login of the employee that created the Z report. Only used for display.
        /// </summary>
        public string Login { get; set; }
        

        #endregion

        #region Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="ZReport"/> class.
        /// </summary>
        public ZReport() : base()
        {
            CreatedDate = DateTime.Now;
            StoreID = "";
            TerminalID = "";
            StaffID = "";
            TotalGrossAmount = decimal.Zero;
            TotalNetAmount = decimal.Zero;
            GrossAmount = decimal.Zero;
            NetAmount = decimal.Zero;
            ReturnGrossAmount = decimal.Zero;
            ReturnNetAmount = decimal.Zero;
            ID = RecordIdentifier.Empty;
            EntryType = 1;
            Login = "";
        }

        #endregion

    }
}
