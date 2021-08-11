using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.Services.Interfaces.SupportInterfaces.EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ICardTenderLineItem : ITenderLineItem
    {
        IEFTInfo EFTInfo { get; set; }

        /// <summary>
        /// The id of the cardtype.
        /// </summary>
        string CardTypeID { get; set; }
    }
}
