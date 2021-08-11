using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using System.Collections.Generic;

namespace LSOne.Services.Interfaces
{
    public interface IRFIDService : IService
    {
        bool NewTags { get; set; }
        /// <summary>
        /// Gets the unprocessed RFID tags that will then be processed as items and sold on the POS
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        List<ScanInfo> GetUnProcessedRFIDs(IConnectionManager entry);

        /// <summary>
        /// Is called after all the RFID tags have been processed and sold on the POS to mark the tags as sold/processed. The tags sold can be viewed on each SaleLineItem on the transaction in property RFIDTagId
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">the current transaction</param>
        void MarkProcessedRFIDs(IConnectionManager entry, IRetailTransaction transaction);

        /// <summary>
        /// Here any processing of RFID tags that needs to be done after the sale has concluded should be done
        ///for example send the RFID tags to the security gates or surveilance systems.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        void ConcludeRFIDs(IConnectionManager entry);

        /// <summary>
        /// Is called from the POS core when the POS is loading in all the devices. This function is called through the RFIDScanner part of the Peripherals service.
        /// </summary>
        void Load();

        /// <summary>
        /// Is called from the POS core when the POS is unloading in all the devices. This function is called through the RFIDScanner part of the Peripherals service.
        /// </summary>
        void Unload();

        /// <summary>
        /// Is called from the POS core when POS input is being disable for example when a dialog is being displayed or an operation is running
        /// </summary>
        void DisableForScan();

        /// <summary>
        /// Is called from the POS core when POS input is being enabled for example when a dialog has been closed or an operation has finished running
        /// </summary>
        void ReEnableForScan();

    }
}
