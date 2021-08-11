using System;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.Services
{
    /// <summary>
    /// Provides access to the DataModel from within the Central Suspension service
    /// </summary>
    [Obsolete("The DLLEntry should not be used except when absolutely necessary. A IConnectionManager parameter should be used")]
    internal class DLLEntry
    {
        /// <summary>
        /// The entry into the database. This is accessable to every class within this assembly
        /// </summary>
        internal static IConnectionManager DataModel;
    }
}
