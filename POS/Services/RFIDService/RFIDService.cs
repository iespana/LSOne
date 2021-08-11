using System;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ErrorHandling;
using System.Collections.Generic;

namespace LSOne.Services
{
    public partial class RFIDService : IRFIDService
    {
        public virtual bool NewTags { get; set;}

        public virtual List<ScanInfo> GetUnProcessedRFIDs(IConnectionManager entry)
        {
            try
            {
                return new List<ScanInfo>();
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, "RFID.ProcessRFID", ex);
                throw ex;
            }
        }


        public virtual void MarkProcessedRFIDs(IConnectionManager entry, IRetailTransaction transaction)
        {            
        }

        public virtual void ConcludeRFIDs(IConnectionManager entry)
        {
            /*
             * 
             * Here any processing of RFID tags that needs to be done after the sale has concluded should be done
             * for example send the RFID tags to the security gates or surveilance systems
             * 
             * */
        }

        public virtual void Load()
        {
        }

        public virtual void Unload()
        {
        }

        public virtual void DisableForScan()
        {

        }

        public virtual void ReEnableForScan()
        {

        }

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }

        public virtual IErrorLog ErrorLog { set; private get; }
    }
}
