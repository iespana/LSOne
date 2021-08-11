using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSOne.SiteService.Plugins.IntegrationFrameworkRetailItem
{
	public partial class IntegrationFrameworkRetailItemPlugin
	{
		/// <summary>
		/// Gets all item images for a valid item Id or an empty collection.
		/// </summary>
		/// <param name="itemID">A <see cref="Guid"/> or a <see cref="System.String"/> item id.</param>
		/// <returns></returns>
		public virtual List<ItemImage> GetItemImages(RecordIdentifier itemID)
		{
			if(RecordIdentifier.IsEmptyOrNull(itemID))
			{
				return new List<ItemImage>();
			}

			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					return Providers.RetailItemData.GetImages(dataModel, itemID);
				}
				finally
				{
					ReturnConnection(dataModel, out dataModel);
				}
			}
			catch (Exception e)
			{
				ThrowChannelError(e);
				return null;
			}
		}

		/// <summary>
		/// Saves the image by image name for the item with the next index available if provided <paramref name="base64Image"/> is not null or empty.
		/// </summary>
		/// <param name="itemImage"></param>
		/// <returns></returns>
		public virtual SaveResult SaveItemImage(ItemImage itemImage, bool removeExisting)
		{
			SaveResult result = new SaveResult
			{
				Success = true,
				ErrorInfos = new List<ErrorInfo>()
			};

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();

			//stop execution as early as possible if input data is incorrect.
			if (itemImage == null || !itemImage.HasValidItem())
			{
				stopwatch.Stop();
				result.ExecutionTime = stopwatch.Elapsed.TotalSeconds;

				return result;
			}

			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();

				try
				{
					//delete if no image is provided
					if(itemImage.Image == null)
					{
						if (removeExisting)
						{
							Providers.RetailItemData.DeleteImages(dataModel, itemImage.HasValidItemMasterID() ? itemImage.ItemMasterID : itemImage.ID);
						}
						else
						{
							Providers.RetailItemData.DeleteImage(dataModel, itemImage);
						}

						return result;
					}

					if(removeExisting)
					{
						Providers.RetailItemData.DeleteImages(dataModel, itemImage.HasValidItemMasterID() ? itemImage.ItemMasterID : itemImage.ID);
						Providers.RetailItemData.SaveImage(dataModel, itemImage);
						return result;
					}

					List<ItemImage> existingImages = Providers.RetailItemData.GetImages(dataModel, itemImage.ID);

					if (existingImages == null || existingImages.Count == 0)
					{
						//item has no image assigned
						Providers.RetailItemData.SaveImage(dataModel, itemImage);
					}
					else
					{
						//find if item already exists first by ImageID, then by ImageName - if not empty and last by image content
						ItemImage existingImage = existingImages.SingleOrDefault(i => i.ImageID == itemImage.ImageID || (i.ImageName == itemImage.ImageName && !string.IsNullOrWhiteSpace(i.ImageName)) || i.SameImageAs(itemImage.Image));

						if (existingImage != null)
						{
							//use the existing ImageID if possible, else delete the old record
							if (RecordIdentifier.IsEmptyOrNull(itemImage.ImageID))
							{
								itemImage.ImageID = existingImage.ImageID;
							}
							else
							{
								Providers.RetailItemData.DeleteImage(dataModel, existingImage);
							}

							existingImages.RemoveAll(i => i.ImageID == existingImage.ImageID);

							//if image index exceeds the number of item images then reset it to get the next available index or to an existing index.
							if (itemImage.ImageIndex == -1 || itemImage.ImageIndex > existingImages.Count)
							{
								itemImage.ImageIndex = existingImage.ImageIndex;
							}
							else
							{
								ResequenceImages(existingImages, existingImage.ImageIndex, -1, itemImage.ImageIndex, 1);
							}

							existingImages.Add(itemImage);
							Providers.RetailItemData.SaveImages(dataModel, existingImages);
						}
						else
						{
							Providers.RetailItemData.SaveImage(dataModel, itemImage);
						}
					}
				}
				catch (Exception ex)
				{
					result.Success = false;
					result.ErrorInfos.Add(new ErrorInfo(itemImage.ID.StringValue, "", ex.ToString()));
				}
				finally
				{
					ReturnConnection(dataModel, out dataModel);
				}
			}
			catch (Exception e)
			{
				result.Success = false;
				ThrowChannelError(e);
			}
			finally
			{
				stopwatch.Stop();
				result.ExecutionTime = stopwatch.Elapsed.TotalSeconds;
			}

			return result;
		}

		/// <summary>
		/// Deletes all images of an item
		/// </summary>
		/// <param name="itemID">A <see cref="Guid"/> or a <see cref="String"/> item id.</param>
		public virtual void DeleteItemImages(RecordIdentifier itemID)
		{
			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					RecordIdentifier itemGid;
					if (!itemID.IsGuid)
					{
						itemGid = Providers.RetailItemData.GetMasterIDFromItemID(dataModel, itemID);
					}
					else
					{
						itemGid = itemID;
					}

					Providers.RetailItemData.DeleteImages(dataModel, itemGid);
				}
				finally
				{
					ReturnConnection(dataModel, out dataModel);
				}
			}
			catch (Exception e)
			{
				ThrowChannelError(e);
			}
		}

		public virtual void DeleteItemImagesList(List<RecordIdentifier> itemIDs)
		{
			if(itemIDs == null || itemIDs.Count == 0)
			{
				return;
			}

			try
			{
				IConnectionManager dataModel = GetConnectionManagerIF();
				try
				{
					RecordIdentifier itemGid;
					foreach (RecordIdentifier itemID in itemIDs)
					{
						if (!itemID.IsGuid)
						{
							itemGid = Providers.RetailItemData.GetMasterIDFromItemID(dataModel, itemID);
						}
						else
						{
							itemGid = itemID;
						}

						Providers.RetailItemData.DeleteImages(dataModel, itemGid);
					}
				}
				finally
				{
					ReturnConnection(dataModel, out dataModel);
				}
			}
			catch(Exception e)
			{
				ThrowChannelError(e);
			}
		}

		#region protected methods

		protected void ResequenceImages(List<ItemImage> itemImages, int startIndex1, int increment1 = 1, int startIndex2 = 0, int increment2 = 0)
		{
			int minIndex = 0;
			int maxIndex = 0;
			int minIncrement = 0;
			int maxIncrement = 0;

			if (startIndex1 <= startIndex2)
			{
				minIndex = startIndex1;
				minIncrement = increment1;
				maxIndex = startIndex2;
				maxIncrement = increment2;
			}
			else
			{
				minIndex = startIndex2;
				minIncrement = increment2;
				maxIndex = startIndex1;
				maxIncrement = increment1;
			}

			foreach (ItemImage itemImage in itemImages)
			{
				if (itemImage.ImageIndex >= minIndex && itemImage.ImageIndex < maxIndex)
				{
					itemImage.ImageIndex += minIncrement;
				}
				else if(itemImage.ImageIndex > maxIndex)
				{
					itemImage.ImageIndex += minIncrement + maxIncrement;
				}
			}
		}

		#endregion

	}
}
