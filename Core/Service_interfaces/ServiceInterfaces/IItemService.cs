using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;

namespace LSOne.Services.Interfaces
{
    public interface IItemService : IService
    {
        /// <summary>
        /// This function is implemented by Item.cs. There three different functions are called within 'ProcessItem':
        /// 
        /// 1. GetInventTableModuleInfo(ref saleLineItem); //queries the datatable 'InventTable' for the following item attributes:
        ///     
        ///    <code>SELECT I.[ItemType], I.[ItemName], I.[ItemGroupID] FROM InventTable I WHERE I.ItemID = @ItemID AND I.DATAAREAID=@DATAAREAID ";</code>
        /// 
        ///     ItemType, in order to set saleLineItem.ItemType
        ///     ItemName, in order to set the ItemDescription = ItemName if the description has been empty
        ///     ItemGroupID, in order to set saleLineItem.ItemGroupId.
        /// 
        /// 2. GetRBOInventTableInfo(ref saleLineItem);
        ///     <code>SELECT M.[LineDisc],M.[MultiLineDisc],M.[EndDisc],M.[UnitId] FROM InventTableModule M WHERE (...) </code>
        ///     These attributes are assigned as follows: <code>
        ///         saleLineItem.LineDiscountGroup = Utility.ToStr(reader[reader.GetOrdinal("LineDisc")]);
        ///         
        /// 
        ///         saleLineItem.MultiLineDiscountGroup = Utility.ToStr(reader[reader.GetOrdinal("MultiLineDisc")]);
        ///         
        /// 
        ///         saleLineItem.IncludedInTotalDiscount = Utility.ToBool(reader[reader.GetOrdinal("EndDisc")]);</code>
        ///         <remarks>Whether an item is included in the total discount granted. Implementation not yet finished.</remarks>
        /// 
        /// 3. GetInventDimInfo(ref saleLineItem);
        ///     <code>SELECT ItemID FROM InventDimCombination I WHERE I.ItemID = @ItemID AND I.DATAAREAID=@DATAAREAID</code>
        ///         Sets the boolean <code>saleLineItem.Dimension.EnterDimensions</code> to true or false according to whether the query has returned rows or not.    
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem">A standard sale item line in a transaction.</param>
        /// <param name="transaction">Transaction</param>
        IItemSale ProcessItem(IConnectionManager entry, ISaleLineItem saleLineItem, IPosTransaction transaction);

        /// <summary>
        /// Displays the Item Search dialog. Returns false if the user pressed cancel. Returns true if the user did choose to sell a selected item.         
        /// </summary>
        /// <param name="selectedItemId">The item id selected in the dialog</param>
        /// <param name="numberOfDisplayedRows">How many rows should be displayed in the dialog</param>
        /// <param name="viewMode">View mode to use</param>
        /// <param name="retailGroup">Retail group to filter by</param>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posTransaction">The active transaction</param>
        /// <param name="operationInfo"></param>
        /// <returns></returns>
        bool ItemSearch(IConnectionManager entry, ref string selectedItemId, int numberOfDisplayedRows, ItemSearchViewModeEnum viewMode, RecordIdentifier retailGroup, IPosTransaction posTransaction, OperationInfo operationInfo = null);

        /// <summary>
        /// Display a list of barcodes to select from
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="barcodes"></param>
        /// <param name="selectedBarcode"></param>
        /// <param name="itemName">Name of the item for which a barcode must be selected. Used as display header.</param>
        /// <returns>True if a barcode has been selected</returns>
        bool BarcodeSelect(IConnectionManager entry, List<BarCode> barcodes, ref BarCode selectedBarcode, string itemName);

        /// <summary>
        /// Returns a string representation for the display about how the quantity and unitprice is multiplied together.  This is used to comply with 
        /// regulations from the National Measurement Office
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="saleLineItem"></param>
        /// <param name="lineLength"></param>
        /// <returns></returns>
        string GetScaleDisplayInformation(IConnectionManager entry, ISaleLineItem saleLineItem, int lineLength);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="validation"></param>
        /// <returns></returns>
        string Validate(IConnectionManager entry, string validation);

        bool UseSerialNumbers(IConnectionManager entry, IPosTransaction transaction);

        /// <summary>
        /// Get the purchase price for an item and store
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="storeID">ID of the store for which to retrieve the cost. Empty ID will return an average cost of all stores</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        RetailItemCost GetRetailItemCost(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RecordIdentifier storeID, bool closeConnection);

        /// <summary>
        /// Get a list purchase prices for an item, for each store including an average for all stores
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemID">ID of the item for which to retrieve the cost</param>
        /// <param name="filter">Search filter</param>
        /// <param name="totalCount">Total items found</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns></returns>
        List<RetailItemCost> GetRetailItemCostList(IConnectionManager entry, SiteServiceProfile siteServiceProfile, RecordIdentifier itemID, RetailItemCostFilter filter, out int totalCount, bool closeConnection);

        /// <summary>
        /// Insert a list of retail item costs
        /// </summary>
        /// <param name="entry">Database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="itemCosts">List of item costs to insert</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        void InsertRetailItemCosts(IConnectionManager entry, SiteServiceProfile siteServiceProfile, List<RetailItemCost> itemCosts, bool closeConnection);
    }
}
