using System.Collections.Generic;
using System.ServiceModel;

using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;

using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSRetail.SiteService.IntegrationFrameworkRetailItemInterface
{
	public partial interface IIntegrationFrameworkRetailItemService
	{
		/// <summary>
		/// Gets all item images.
		/// </summary>
		/// <param name="itemID">A <see cref="System.Guid"/> or a <see cref="System.String"/> item id.</param>
		/// <returns></returns>
		[OperationContract]
		List<ItemImage> GetItemImages(RecordIdentifier itemID);

		/// <summary>
		/// Saves the image for the item with the next index available.
		/// </summary>
		/// <param name="itemImage">An <see cref="ItemImage"/> object containing an item id, image and image name</param>
		/// <param name="removeExisting">If existing images must be removed first or the given <paramref name="itemImage"/> must be merged into existing ones</param>
		/// <returns></returns>
		[OperationContract]
		SaveResult SaveItemImage(ItemImage itemImage, bool removeExisting);

		/// <summary>
		/// Deletes all images of an item
		/// </summary>
		/// <param name="itemID">A <see cref="System.Guid"/> item id.</param>
		[OperationContract]
		void DeleteItemImages(RecordIdentifier itemID);

		/// <summary>
		/// Deletes all images of the given items
		/// </summary>
		/// <param name="itemIDs">A <see cref="List{T}"/> containing item ids.</param>
		[OperationContract]
		void DeleteItemImagesList(List<RecordIdentifier> itemIDs);
	}
}
