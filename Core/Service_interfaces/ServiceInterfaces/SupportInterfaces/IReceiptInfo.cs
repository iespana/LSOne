using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IReceiptInfo : IDataEntity
    {
        int LineID { get; set; }

        /// <summary>
        /// The unique identifier of the form type that is being printed
        /// </summary>
        RecordIdentifier FormType { get; set; }

        /// <summary>
        /// The OPOS print string that was printed
        /// </summary>
        string PrintString { get; set; }

        /// <summary>
        /// The name of the document created with the receipt f.ex. when a PDF file is created
        /// </summary>
        string DocumentName { get; set; }

        /// <summary>
        /// The file location of the document created
        /// </summary>
        string DocumentLocation { get; set; }

        /// <summary>
        /// The width of the form that was used to create this receipt. Necessary for the WinPrinter
        /// </summary>
        int FormWidth { get; set; }

        /// <summary>
        /// True if this receipt was generated using the email profile and is meant to be sent via email
        /// </summary>
        bool IsEmailReceipt { get; set; }
    }
}
