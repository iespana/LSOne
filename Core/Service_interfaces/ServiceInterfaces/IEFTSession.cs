using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// This interface allows extending IEFT implementations with  intialization on POS start and shutdown  on POS exit
    /// </summary>
    public interface IEFTSession
    {
        /// <summary>
        /// Initialize communication with the EFT device
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">Settings object</param>
        void Initialize(IConnectionManager entry, ISettings settings);

        /// <summary>
        /// Shutdown communication with the EFT device
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">Settings object</param>
        void Shutdown(IConnectionManager entry, ISettings settings);

        /// <summary>
        /// Called when the POS begins a new transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The active transaction</param>
        void OnBeginTransaction(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Called when the POS finalizes a transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="transaction">The active transaction</param>
        void OnEndTransaction(IConnectionManager entry, IPosTransaction transaction);

    }
}
