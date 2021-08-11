using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;

using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
	//TODO: Create own provider for ItemImage
	public partial class RetailItemData
	{
		private static List<TableColumn> itemImageColumns = new List<TableColumn>
		{
			new TableColumn { ColumnName = "ITEMID", TableAlias = "A" },
			new TableColumn { ColumnName = "ITEMMASTERID", TableAlias = "A" },
			new TableColumn { ColumnName = "IMAGEINDEX", TableAlias = "A" },
			new TableColumn { ColumnName = "IMAGEID", TableAlias = "A" },
			new TableColumn { ColumnName = "IMAGENAME", TableAlias = "A" },
			new TableColumn { ColumnName = "IMAGE", TableAlias = "A" }
		};

		protected void PopulateItemImage(IDataReader dr, ItemImage item)
		{
			item.ID = (string)dr["ITEMID"];
			item.ItemMasterID = (Guid)dr["ITEMMASTERID"];
			item.ImageIndex = AsInt(dr["IMAGEINDEX"]);
			item.ImageID = (Guid)(dr["IMAGEID"]);
			item.ImageName = AsString(dr["IMAGENAME"]);
			item.Image = AsImage(dr["IMAGE"]);
		}		

		/// <summary>
		/// Fetches the default image for a given retail item.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">ID of the retail item to fetch the image for</param>
		/// <returns>The image for the given retail item or null if no image was found</returns>
		public virtual Image GetDefaultImage(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				Condition idCondition;
				if (itemID.IsGuid)
				{
					idCondition = new Condition { Operator = "AND", ConditionValue = "A.ITEMMASTERID = @ItemId" };
					MakeParam(cmd, "ItemId", (Guid)itemID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					idCondition = new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @ItemId" };
					MakeParam(cmd, "ItemId", (string)itemID);

				}
				conditions.Add(idCondition);
				conditions.Add(new Condition
				{
					Operator = "AND",
					ConditionValue = $"IMAGEINDEX = (SELECT MIN(IMAGEINDEX) FROM RETAILITEMIMAGE {QueryPartGenerator.ConditionGenerator(idCondition)})"
				});
				AddCustomHandling(ref itemImageColumns, ref conditions);
				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMIMAGE", "A"),
					QueryPartGenerator.InternalColumnGenerator(itemImageColumns),
					string.Empty,
					QueryPartGenerator.ConditionGenerator(conditions),
					"ORDER BY IMAGEINDEX");

				try
				{
					var result = Execute<ItemImage>(entry, cmd, CommandType.Text, PopulateItemImage);
					if (result == null || result.Count == 0)
						return null;

					return result[0].Image;
				}
				catch
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Fetches all images for a given retail item
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">ID of the retail item to fetch the image for</param>
		/// <param name="includeGraphic">If true, the Image will be retrieved from database, otherwise only its hash</param>
		/// <returns>The image for the given retail item or null if no image was found</returns>
		public virtual List<ItemImage> GetImages(IConnectionManager entry, RecordIdentifier itemID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				if (itemID.IsGuid)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMMASTERID = @ItemId" });
					MakeParam(cmd, "ItemId", (Guid)itemID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @ItemId" });
					MakeParam(cmd, "ItemId", (string)itemID);

				}

				AddCustomHandling(ref itemImageColumns, ref conditions);
				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMIMAGE", "A"),
					QueryPartGenerator.InternalColumnGenerator(itemImageColumns),
					string.Empty,
					QueryPartGenerator.ConditionGenerator(conditions),
					"ORDER BY IMAGEINDEX");

				try
				{
					return Execute<ItemImage>(entry, cmd, CommandType.Text, PopulateItemImage);
				}
				catch
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Fetches an image for a given imageID
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="imageID">ID of the image</param>
		/// <param name="includeGraphic">If true, the Image will be retrieved from database, otherwise only its hash</param>
		/// <returns>The image for a given imageID or NULL if it doesn't exist</returns>
		public virtual ItemImage GetImage(IConnectionManager entry, RecordIdentifier imageID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				List<Condition> conditions = new List<Condition>();
				conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.IMAGEID = @imageID" });
				MakeParam(cmd, "imageID", (Guid)imageID, SqlDbType.UniqueIdentifier);

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMIMAGE", "A"),
					QueryPartGenerator.InternalColumnGenerator(itemImageColumns),
					string.Empty,
					QueryPartGenerator.ConditionGenerator(conditions),
					"");

				try
				{
					var result = Execute<ItemImage>(entry, cmd, CommandType.Text, PopulateItemImage);
					if (result == null || result.Count == 0)
						return null;

					return result[0];
				}
				catch
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Fetches default images for the given list of retail items. This will also look up the item image for an item variant. This means
		/// that the item image entries for header items will also be included.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemIDs">IDs for the items to get the images for</param>
		/// <returns></returns>
		public virtual List<ItemImage> GetDefaultImages(IConnectionManager entry, List<RecordIdentifier> itemIDs)
		{
			List<ItemImage> results = new List<ItemImage>();

			if (itemIDs.Count == 0)
			{
				return results;
			}

			ItemImage itemDefaultImage;

			foreach (RecordIdentifier itemID in itemIDs)
			{
				itemDefaultImage = GetItemImage(entry, itemID);
				results.Add(itemDefaultImage);
			}

			return results;
		}

		/// <summary>
		/// Fetches default image for the given retail item. This will also look up the item image for an item variant. This means
		/// that the item image entries for header items will also be included.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemMasterID">The master ID for the item to get the images for</param>
		/// <returns></returns>
		public virtual ItemImage GetItemImage(IConnectionManager entry, RecordIdentifier itemMasterID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"Select   
						ITEMID,
						ITEMMASTERID,
						IMAGE,
						IMAGEINDEX,
						IMAGEID,
						IMAGENAME,
						0 as INTERNALSORT
					  FROM RETAILITEMIMAGE A 
						  --Conditions
					  WHERE A.ITEMMASTERID = @itemMasterID
					  AND IMAGEINDEX = (SELECT MIN(IMAGEINDEX) FROM RETAILITEMIMAGE WHERE A.ITEMMASTERID = @itemMasterID)

					  union

					  Select   
					  A.ITEMID,
					  A.ITEMMASTERID,
					  A.IMAGE,
					  A.IMAGEINDEX,
					  A.IMAGEID,
					  A.IMAGENAME,
					  1 as INTERNALSORT
					  FROM RETAILITEMIMAGE A 
						  --Conditions
					  WHERE A.ITEMMASTERID in 
					  (
						  select r.MASTERID
						  from RETAILITEM r
						  where r.MASTERID in 
						  ( 
							  select r2.HEADERITEMID
							  from RETAILITEM r2
							  where r2.MASTERID = @itemMasterID
						  )
					  )
					  AND IMAGEINDEX = 
					  (
						  SELECT MIN(IMAGEINDEX) 
						  FROM RETAILITEMIMAGE 
						  WHERE A.ITEMMASTERID in 
						  (
							  select r2.HEADERITEMID
							  from RETAILITEM r2
							  where r2.MASTERID = @itemMasterID
						  )
					  )
					  --Sort
					  ORDER BY INTERNALSORT, IMAGEINDEX";

				MakeParam(cmd, "itemMasterID", (string)itemMasterID);

				try
				{
					var result = Execute<ItemImage>(entry, cmd, CommandType.Text, PopulateItemImage);
					if (result == null || result.Count == 0)
						return new ItemImage();

					return result[0];
				}
				catch
				{
					return new ItemImage();
				}
			}
		}

		/// <summary>
		/// Saves an image as the default image for a given retail item
		/// </summary>
		/// <remarks>Edit retail items permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemMasterID">MasterID of the retail item to save the image for. Can be RecordIdentifier.Empty if you only know the normal ID</param>
		/// <param name="itemID">ID of the retail item to save the image for</param>
		/// /// <param name="image">The image to be saved</param>
		public virtual void SaveImage(IConnectionManager entry, RecordIdentifier itemMasterID, RecordIdentifier itemID, Image image)
		{
			SaveImage(entry, itemMasterID, itemID, image, 1);
		}

		/// <summary>
		/// Saves an image as the default image for a given retail item
		/// </summary>
		/// <remarks>Edit retail items permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemMasterID">MasterID of the retail item to save the image for. Can be RecordIdentifier.Empty if you only know the normal ID</param>
		/// <param name="itemID">ID of the retail item to save the image for</param>
		/// /// <param name="image">The image to be saved</param>
		/// <param name="index">Index of image. If set to -1, the next available index is found and used</param>
		public virtual void SaveImage(IConnectionManager entry, RecordIdentifier itemMasterID, RecordIdentifier itemID, Image image, int index)
		{
			if (itemMasterID == RecordIdentifier.Empty)
			{
				itemMasterID = GetMasterIDFromItemID(entry, itemID);

				if (itemMasterID == null)
				{
					return;
				}
			}

			if (index == -1)
			{
				index = GetNextImageIndex(entry, itemMasterID);
			}
			SaveImage(entry, 
					new ItemImage {
						ItemMasterID = itemMasterID,
						ID = itemID,
						Image = image,
						ImageIndex = index
					});
		}

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
		public virtual void SaveImage(IConnectionManager entry, ItemImage itemImage)
		{
			ValidateSecurity(entry, Permission.ItemsEdit);

			//do nothing if null or if it has no item ID (master or simple ID)
			if (itemImage == null || !itemImage.HasValidItem())
			{
				return;
			}

			if (itemImage.Image == null)
			{
				DeleteImage(entry, itemImage);
				return;
			}

			//set item master ID and item ID
			if (!itemImage.HasValidItemMasterID())
			{
				itemImage.ItemMasterID = GetMasterIDFromItemID(entry, itemImage.ID);

				if (!itemImage.HasValidItemMasterID())
				{
					return;
				}
			}
			if (!itemImage.HasValidItemID())
			{
				itemImage.ID = GetItemIDFromMasterID(entry, itemImage.ItemMasterID);
			}

			//set ImageIndex
			if (itemImage.ImageIndex == -1)
			{
				itemImage.ImageIndex = GetNextImageIndex(entry, itemImage.ItemMasterID);
			}

			//save original image ID
			//set ImageID to a new Guid if not already set
			RecordIdentifier initialImageGuid = itemImage.ImageID;
			if (RecordIdentifier.IsEmptyOrNull(initialImageGuid))
			{
				itemImage.ImageID = Guid.NewGuid();
			}

			//save by name if no ImageID was provided but image name was
			if(RecordIdentifier.IsEmptyOrNull(initialImageGuid) && !string.IsNullOrWhiteSpace(itemImage.ImageName))
			{
				SaveImageByName(entry, itemImage);
			}
			else
			{
				SaveImageByImageID(entry, itemImage);
			}
		}

		/// <summary>
		/// Saves all images for a particular item.
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemImages">Images to save</param>
		public virtual void SaveImages(IConnectionManager entry, List<ItemImage> itemImages)
		{
			if (itemImages == null || itemImages.Count == 0)
				return;

			foreach (ItemImage itemImage in itemImages)
			{
				SaveImage(entry, itemImage);
			}
		}

		/// <summary>
		/// Delete an item image from the database
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemImage"></param>
		/// <remarks>It will try to delete first by image ID, if available, or by image name</remarks>
		public virtual void DeleteImage(IConnectionManager entry, ItemImage itemImage)
		{
			ValidateSecurity(entry, Permission.ItemsEdit);

			if (itemImage == null || !itemImage.HasValidItem())
			{
				return;
			}

			if(RecordIdentifier.IsEmptyOrNull(itemImage.ImageID))
			{
				DeleteByImageName(entry, itemImage);
			}
			else
			{
				DeleteByImageID(entry, itemImage);
			}
		}

		/// <summary>
		/// Delete all images for an item
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="itemID">If this is the ItemMasterID then this needs to be a Guid otherwise it is assume that this parameter is an ItemID </param>
		public virtual void DeleteImages(IConnectionManager entry, RecordIdentifier itemID)
		{
			ValidateSecurity(entry, Permission.ItemsEdit);

			if (RecordExists(entry, "RETAILITEMIMAGE", itemID.IsGuid ? "ITEMMASTERID" : "ITEMID", itemID, false))
			{
				var statement = new SqlServerStatement("RETAILITEMIMAGE") { StatementType = StatementType.Delete };
				if (itemID.IsGuid)
				{
					statement.AddCondition("ITEMMASTERID", (Guid)itemID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					statement.AddCondition("ITEMID", (string)itemID, SqlDbType.NVarChar);
				}

				entry.Connection.ExecuteStatement(statement);
			}
		}

		#region protected methods

		/// <summary>
		/// Gets the next image index available for the given item.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="itemID"></param>
		/// <returns></returns>
		protected int GetNextImageIndex(IConnectionManager entry, RecordIdentifier itemID)
		{
			if (RecordIdentifier.IsEmptyOrNull(itemID))
			{
				throw new ArgumentNullException(nameof(itemID));
			}

			int index = 0;

			using (var cmd = entry.Connection.CreateCommand())
			{
				TableColumn column = new TableColumn { ColumnName = "MAX(IMAGEINDEX)" };

				List<Condition> conditions = new List<Condition>();
				if (itemID.IsGuid)
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMMASTERID = @itemId" });
					MakeParam(cmd, "itemId", (Guid)itemID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.ITEMID = @itemId" });
					MakeParam(cmd, "itemId", (string)itemID);

				}

				cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMIMAGE", "A"),
					column,
					string.Empty,
					QueryPartGenerator.ConditionGenerator(conditions),
					string.Empty);


				var res = entry.Connection.ExecuteScalar(cmd);
				if (res == null || res == DBNull.Value)
					index = 0;
				else
					index = Convert.ToInt32(res);
				index++;
			}

			return index;
		}

		protected void SaveImageByImageID(IConnectionManager entry, ItemImage itemImage)
		{
			var statement = new SqlServerStatement("RETAILITEMIMAGE");

			if (RecordIdentifier.IsEmptyOrNull(itemImage.ImageID))
			{
				itemImage.ImageID = Guid.NewGuid();
			}

			statement.AddField("ITEMMASTERID", (Guid)itemImage.ItemMasterID, SqlDbType.UniqueIdentifier);
			statement.AddField("IMAGEINDEX", itemImage.ImageIndex, SqlDbType.Int);
			statement.AddField("IMAGE", FromImage(itemImage.Image), SqlDbType.VarBinary);
			statement.AddField("ITEMID", (string)itemImage.ID);
			statement.AddField("IMAGENAME", itemImage.ImageName);


			if (!RecordExists(entry, "RETAILITEMIMAGE", "IMAGEID", itemImage.ImageID, false))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("IMAGEID", (Guid)itemImage.ImageID, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("IMAGEID", (Guid)itemImage.ImageID, SqlDbType.UniqueIdentifier);
			}
			entry.Connection.ExecuteStatement(statement);
		}

		protected void SaveImageByName(IConnectionManager entry, ItemImage itemImage)
		{
			if (RecordIdentifier.IsEmptyOrNull(itemImage.ImageID))
			{
				itemImage.ImageID = Guid.NewGuid();
			}

			var statement = new SqlServerStatement("RETAILITEMIMAGE");

			statement.AddField("ITEMID", (string)itemImage.ID);
			statement.AddField("IMAGEINDEX", itemImage.ImageIndex, SqlDbType.Int);
			statement.AddField("IMAGE", FromImage(itemImage.Image), SqlDbType.VarBinary);
			statement.AddField("IMAGENAME", itemImage.ImageName);

			RecordIdentifier id = (RecordIdentifier)itemImage.ItemMasterID.Clone();
			id.SecondaryID = RecordIdentifier.FromObject(itemImage.ImageName);

			if (!RecordExists(entry, "RETAILITEMIMAGE", new string[] { "ITEMMASTERID", "IMAGENAME" }, id, false))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("IMAGEID", (Guid)itemImage.ImageID, SqlDbType.UniqueIdentifier);
				statement.AddField("ITEMMASTERID", (Guid)itemImage.ItemMasterID, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("ITEMMASTERID", (Guid)itemImage.ItemMasterID, SqlDbType.UniqueIdentifier);
				statement.AddCondition("IMAGENAME", itemImage.ImageName);
			}

			entry.Connection.ExecuteStatement(statement);
		}

		protected void DeleteByImageID(IConnectionManager entry, ItemImage itemImage)
		{
			if (RecordIdentifier.IsEmptyOrNull(itemImage.ImageID))
			{
				return;
			}

			if (RecordExists(entry, "RETAILITEMIMAGE", "IMAGEID", itemImage.ImageID, false))
			{
				var statement = new SqlServerStatement("RETAILITEMIMAGE") { StatementType = StatementType.Delete };
				statement.AddCondition("IMAGEID", (Guid)itemImage.ImageID, SqlDbType.UniqueIdentifier);
				entry.Connection.ExecuteStatement(statement);
			}
		}

		protected void DeleteByImageName(IConnectionManager entry, ItemImage itemImage)
		{
			if (string.IsNullOrWhiteSpace(itemImage.ImageName))
			{
				return;
			}

			bool itemMasterIDAvailable = itemImage.HasValidItemMasterID();
			RecordIdentifier id = itemMasterIDAvailable
									? (RecordIdentifier)itemImage.ItemMasterID.Clone()
									: (RecordIdentifier)itemImage.ID.Clone();
			id.SecondaryID = RecordIdentifier.FromObject(itemImage.ImageName);

			if (RecordExists(entry,
							"RETAILITEMIMAGE",
							new string[] { itemMasterIDAvailable ? "ITEMMASTERID" : "ITEMID", "IMAGENAME" },
							id,
							false))
			{
				var statement = new SqlServerStatement("RETAILITEMIMAGE") { StatementType = StatementType.Delete };
				statement.AddCondition("IMAGENAME", itemImage.ImageName);
				if (itemMasterIDAvailable)
				{
					statement.AddCondition("ITEMMASTERID", (Guid)itemImage.ItemMasterID, SqlDbType.UniqueIdentifier);
				}
				else
				{
					statement.AddCondition("ITEMID", (string)itemImage.ID);
				}
				entry.Connection.ExecuteStatement(statement);
			}
		}

		#endregion
	}
}
