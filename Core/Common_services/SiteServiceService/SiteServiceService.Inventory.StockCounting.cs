using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Inventory.Containers;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.BusinessObjects.FileImport;
using System.Data;
using System.Threading.Tasks;
using System.ServiceModel;
using LSRetail.SiteService.SiteServiceInterface;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.Development;

namespace LSOne.Services
{
    public partial class SiteServiceService
    {
        /// <summary>
        /// Searches for Stock  counting entries 
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="filter">Filter settings container</param>
        /// <param name="totalRecordsMatching">out: how many rows there are in total</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>The list of inventory journal transactions</returns>
        public virtual List<InventoryJournalTransaction> SearchJournalTransactions(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryJournalTransactionFilter filter, out int totalRecordsMatching, bool closeConnection)
        {
            List<InventoryJournalTransaction> result = new List<InventoryJournalTransaction>();
            int outRecordsMatching = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.SearchJournalTransactions(filter, out outRecordsMatching, CreateLogonInfo(entry)), closeConnection);
            totalRecordsMatching = outRecordsMatching;

            return result;
        }

        /// <summary>
        /// Updates the on hand inventory of all journal transactions in a stock counting journal 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalID">Entry of the stock counting journal</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void RefreshStockCountingJournalInventoryOnHand(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalID, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.RefreshStockCountingJournalInventoryOnHand(journalID, CreateLogonInfo(entry)), closeConnection);
        }

        /// <summary>
        /// Get stock counting
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalId">The journal ID to be get</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>The inventory adjustment</returns>
        public virtual InventoryAdjustment GetStockCounting(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier journalId,
            bool closeConnection
            )
        {
            InventoryAdjustment result = null;

            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetStockCounting(CreateLogonInfo(entry), journalId), closeConnection);

            return result;
        }

        /// <summary>
        /// Get a list of journals based on a filter
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="filter">Filter container</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="totalRecordsMatching">Total number of records that matched the search criteria</param>
        /// <returns></returns>
        public virtual List<InventoryAdjustment> GetJournalListAdvancedSearch(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryAdjustmentFilter filter, bool closeConnection, out int totalRecordsMatching)
        {
            List<InventoryAdjustment> result = new List<InventoryAdjustment>();

            int recordsMatching = 0;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetJournalListAdvancedSearch(CreateLogonInfo(entry), filter, out recordsMatching), closeConnection);
            totalRecordsMatching = recordsMatching;

            return result;
        }

        /// <summary>
        /// Get the current processing status for a journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalId">ID fo the journal to check</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual AdjustmentStatus GetAdjustmentStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalId, bool closeConnection)
        {
            AdjustmentStatus result = new AdjustmentStatus();
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetAdjustmentStatus(CreateLogonInfo(entry), journalId), closeConnection);
            return result;
        }

        /// <summary>
        /// Set the processing status for a journal
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalId">ID fo the journal to set</param>
        /// <param name="status">The status to set</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        public virtual void SetAdjustmentStatus(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier journalId, InventoryProcessingStatus status, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () => server.SetAdjustmentStatus(CreateLogonInfo(entry), journalId, status), closeConnection);
        }

        /// <summary>
        /// Deletes a stock counting journal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingID">The stock counting ID to delete</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Result of the operation</returns>
        public virtual DeleteJournalResult DeleteStockCounting(
            IConnectionManager entry,
            SiteServiceProfile siteServiceProfile,
            RecordIdentifier stockCountingID,
            bool closeConnection
            )
        {
            DeleteJournalResult result = DeleteJournalResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteStockCounting(CreateLogonInfo(entry), stockCountingID), closeConnection);
            return result;
        }

        /// <summary>
        /// Deletes multiple inventory journal transaction lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="IDs">The IDs of the inventory journal to delete</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual DeleteJournalTransactionsResult DeleteMultipleJournalTransactions(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RecordIdentifier> IDs, bool closeConnection)
        {
            DeleteJournalTransactionsResult result = DeleteJournalTransactionsResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.DeleteMultipleJournalTransactions(IDs, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        /// <summary>
        /// Posts multiple stock counting lines
        /// </summary>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <param name="ijTransactions">The lines to post</param>
        /// <param name="storeId">The store the lines are for</param>
        /// <returns>Status and indicates whether the journal still has unposted lines</returns>
        public virtual PostStockCountingLinesContainer PostMultipleStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
           List<InventoryJournalTransaction> ijTransactions, RecordIdentifier storeId, bool closeConnection)
        {
            PostStockCountingLinesContainer result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.PostMultipleStockCountingLines(ijTransactions, storeId, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        /// <summary>
        /// Post all lines from a stock counting journal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Result of the operation</returns>
        public virtual PostStockCountingLinesContainer PostAllStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingJournalID, bool closeConnection)
        {
            return DoRemoteWork(entry, siteServiceProfile, () => server.PostAllStockCountingLines(stockCountingJournalID, CreateLogonInfo(entry)), closeConnection);
        }

        /// <summary>
        /// Post all lines from a stock counting journal asynchronously
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingJournalID">The stock counting ID for which to post all lines</param>
        /// <returns>Result of the operation</returns>
        public virtual async Task<PostStockCountingLinesContainer> AsyncPostAllStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingJournalID)
        {
            ChannelFactory<ISiteService> asyncServerFactory = null;
            ISiteService asyncServer = null;

            try
            {
                asyncServerFactory = new ChannelFactory<ISiteService>(binding, new EndpointAddress("net.tcp://" + siteServiceProfile.SiteServiceAddress + ":" + siteServiceProfile.SiteServicePortNumber + "/" + SiteServiceConstants.EndPointName));
                asyncServer = asyncServerFactory.CreateChannel();
                ((IContextChannel)asyncServer).OperationTimeout = TimeSpan.MaxValue;
                LogonInfo logonInfo = CreateLogonInfo(entry, asyncServer);
                Task<PostStockCountingLinesContainer> task = Task<PostStockCountingLinesContainer>.Factory.FromAsync(asyncServer.BeginAsyncPostAllStockCountingLines, asyncServer.EndAsyncPostAllStockCountingLines, stockCountingJournalID, logonInfo, null);
                return await task;
            }
            catch
            {
                throw;
            }
            finally
            {
                asyncServerFactory?.Close();
                asyncServerFactory = null;
                asyncServer = null;
            }
        }

        /// <summary>
		/// Checks if an inventory adjustment exists
		/// </summary> 
		/// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="stockCountingID">The ID of the document</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns></returns>
        public virtual bool InventoryAdjustmentExists(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            RecordIdentifier stockCountingID, bool closeConnection)
        {
            bool reply = true;
            DoRemoteWork(entry, siteServiceProfile, () => reply = server.InventoryAdjustmentExists(stockCountingID, CreateLogonInfo(entry)), closeConnection);
            return reply;
        }

        /// <summary>
        /// Get inventory journal transaction by ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="journalTransactionId">The journal transaction ID</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Returns the inventory journal transaction for the supplied ID</returns>
        public virtual InventoryJournalTransaction GetInventoryJournalTransaction(IConnectionManager entry,
            SiteServiceProfile siteServiceProfile, RecordIdentifier journalTransactionId, bool closeConnection)
        {
            InventoryJournalTransaction result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.GetInventoryJournalTransaction(journalTransactionId, CreateLogonInfo(entry)), closeConnection);
            return result;
        }

        /// <summary>
        /// Saves a stock counting line
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="ijTransaction">The line to save</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        public virtual void SaveJournalTransaction(IConnectionManager entry, SiteServiceProfile siteServiceProfile,
            InventoryJournalTransaction ijTransaction, bool closeConnection)
        {
            DoRemoteWork(entry, siteServiceProfile, () =>  server.SaveJournalTransaction(ijTransaction, CreateLogonInfo(entry)), closeConnection);
        }

        /// <summary>
		/// Create a new stock counting journal and copy all lines from an existing journal
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="storeID">Store ID for the new journal</param>
		/// <param name="description">Description of the new journal</param>
		/// <param name="existingAdjustmentID">Existing journal from which to copy the lines</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns>Status and ID of the created journal</returns>
        public virtual CreateStockCountingContainer CreateStockCountingFromExistingAdjustment(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, string description, RecordIdentifier existingAdjustmentID, bool closeConnection)
        {
            CreateStockCountingContainer result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateStockCountingFromExistingAdjustment(CreateLogonInfo(entry), storeID, description, existingAdjustmentID), closeConnection);
            return result;
        }

        /// <summary>
		/// Create a new stock counting journal based on a given template
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation</param>
		/// <param name="data">Data to import, parsed from excel file</param>
		/// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
		/// <returns>A list of import results</returns>
        public virtual List<ImportLogItem> ImportStockCountingFromExcel(IConnectionManager entry, SiteServiceProfile siteServiceProfile, DataTable data, bool closeConnection)
        {
            List<ImportLogItem> result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.ImportStockCountingFromExcel(CreateLogonInfo(entry), data), closeConnection);
            return result;
        }

        /// <summary>
        /// Create a new stock counting journal and copy all lines from an existing journal.
        /// </summary>
        /// <param name="entry">The entry into the database.</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation.</param>
        /// <param name="storeID">Store ID for the new journal.</param>
        /// <param name="description">Description of the new journal.</param>
        /// <param name="filter">Container with desired item filters.</param>
        /// <param name="templateID">Optional: The ID of the stock counting template if this is created from a template filter.</param>
        public virtual CreateStockCountingContainer CreateStockCountingFromFilter(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier storeID, string description, InventoryTemplateFilterContainer filter, bool closeConnection)
        {
            CreateStockCountingContainer result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateStockCountingFromFilter(CreateLogonInfo(entry), storeID, description, filter), closeConnection);
            
            return result;
        }

        /// <summary>
        /// Create a new stock counting journal based on a given template.
        /// </summary>
        /// <param name="entry">The entry into the database.</param>
		/// <param name="siteServiceProfile">Which site service to use for this operation.</param>
        /// <param name="template">the stock counting template.</param>
        /// <returns>Status and ID of the created journal.</returns>
        public virtual CreateStockCountingContainer CreateStockCountingFromTemplate(IConnectionManager entry, SiteServiceProfile siteServiceProfile, TemplateListItem template, bool closeConnection)
        {
            CreateStockCountingContainer result = null;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CreateStockCountingFromTemplate(CreateLogonInfo(entry), template), closeConnection);

            return result;
        }

        /// <summary>
        /// Compresses all lines on a stock counting journal, that have the same item and unit
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="stockCountingID">Stock counting journal id</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Operation result</returns>
        public virtual CompressAdjustmentLinesResult CompressAllStockCountingLines(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier stockCountingID, bool closeConnection)
        {
            CompressAdjustmentLinesResult result = CompressAdjustmentLinesResult.Success;
            DoRemoteWork(entry, siteServiceProfile, () => result = server.CompressAllStockCountingLines(CreateLogonInfo(entry), stockCountingID), closeConnection);
            return result;
        }
    }
}