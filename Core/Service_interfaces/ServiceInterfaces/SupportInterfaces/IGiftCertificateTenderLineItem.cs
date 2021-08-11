using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IGiftCertificateTenderLineItem
    {
        /// <summary>
        /// The id of the store that issued the gift certificate.
        /// </summary>
        string IssuingStoreId { get; set; }

        /// <summary>
        /// The id of the terminal that issued the gift certificate.
        /// </summary>
        string IssuingTerminalId { get; set; }

        /// <summary>
        /// The serialnumber of the gift certificate. 
        /// </summary>
        string SerialNumber { get; set; }

        /// <summary>
        /// The date when the gift certificate was issued.
        /// </summary>
        DateTime IssuedDate { get; set; }

        /// <summary>
        /// The date when the gift certificate was applied.
        /// </summary>
        DateTime AppliedDate { get; set; }
    }
}
