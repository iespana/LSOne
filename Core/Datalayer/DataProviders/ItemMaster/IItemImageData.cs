using System.Collections.Generic;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.ItemMaster
{
	public partial interface IRetailItemData
	{
		/// <summary>
		/// Fetches the default image for a given retail item.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">ID of the retail item to fetch the image for</param>
		/// <returns>The image for the given retail item or null if no image was found</returns>
		Image GetDefaultImage(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Fetches all images for a given retail item
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">ID of the retail item to fetch the image for</param>
		/// <returns>The image for the given retail item or null if no image was found</returns>
		List<ItemImage> GetImages(IConnectionManager entry, RecordIdentifier itemID);

		/// <summary>
		/// Fetches an image for a given imageID
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="imageID">ID of the image</param>
		/// <returns>The image for a given imageID or NULL if it doesn't exist</returns>
		ItemImage GetImage(IConnectionManager entry, RecordIdentifier imageID);

		/// <summary>
		/// Fetches default images for the given list of retail items. This will also look up the item image for an item variant. This means
		/// that the item image entries for header items will also be included.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemIDs">IDs for the items to get the images for</param>
		/// <returns></returns>
		List<ItemImage> GetDefaultImages(IConnectionManager entry, List<RecordIdentifier> itemIDs);

		/// <summary>
		/// Fetches default image for the given retail item. This will also look up the item image for an item variant. This means
		/// that the item image entries for header items will also be included.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemMasterID">The master ID for the item to get the images for</param>
		/// <returns></returns>
		ItemImage GetItemImage(IConnectionManager entry, RecordIdentifier itemMasterID);

		/// <summary>
		/// Saves an image as the default image for a given retail item
		/// </summary>
		/// <remarks>Edit retail items permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemMasterID">MasterID of the retail item to save the image for. Can be RecordIdentifier.Empty if you only know the normal ID</param>
		/// <param name="itemID">ID of the retail item to save the image for</param>
		/// /// <param name="image">The image to be saved</param>
		void SaveImage(IConnectionManager entry, RecordIdentifier itemMasterID, RecordIdentifier itemID, Image image);

		/// <summary>
		/// Saves an image as the default image for a given retail item
		/// </summary>
		/// <remarks>Edit retail items permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemMasterID">MasterID of the retail item to save the image for. Can be RecordIdentifier.Empty if you only know the normal ID</param>
		/// <param name="itemID">ID of the retail item to save the image for</param>
		/// /// <param name="image">The image to be saved</param>
		/// <param name="index">Index of image. If set to -1, the next available index is found and used</param>
		void SaveImage(IConnectionManager entry, RecordIdentifier itemMasterID, RecordIdentifier itemID, Image image, int index);

		/// <summary>
		/// Saves an image as the default image for a given retail item.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemImage">The item image to be saved</param>
		/// <remarks>
		/// Edit retail items permission is needed to execute this method.
		/// If it has no valid item (master ID or ID) then the method does nothing.
		/// If the item image is null then it will delete the image (by image ID, if available or by image name)
		/// It will try to save first by image ID, if available, or by image name.
		/// </remarks>
		void SaveImage(IConnectionManager entry, ItemImage itemImage);

		/// <summary>
		/// Saves all images for a particular item
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemImages">Images to save</param>
		void SaveImages(IConnectionManager entry, List<ItemImage> itemImages);

		/// <summary>
		/// Delete an item image from the database
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemImage"></param>
		/// <remarks>It will try to delete first by image ID, if available, or by image name</remarks>
		void DeleteImage(IConnectionManager entry, ItemImage itemImage);

		/// <summary>
		/// Delete all images for an item
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">If this is the ItemMasterID then this needs to be a Guid otherwise it is assume that this parameter is an ItemID </param>
		void DeleteImages(IConnectionManager entry, RecordIdentifier itemID);
	}
}
