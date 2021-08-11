using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.Services.Datalayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.SqlDataProviders
{
    public partial class OldRetailItemData
    {
        private static void PopulateItemImage(IDataReader dr, OldItemImage itemImage)
        {
            itemImage.ID = (string)dr["ITEMID"];
            itemImage.ImageIndex = AsInt(dr["IMAGEINDEX"]);
            itemImage.Image = AsImage(dr["IMAGE"]);
        }

        /// <summary>
        /// Fetches the default image for a given retail item.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item to fetch the image for</param>
        /// <returns>The image for the given retail item or null if no image was found</returns>
        public virtual Image GetDefaultImage(IConnectionManager entry, RecordIdentifier itemID)
        {
            var result = GetImages(entry, itemID);
            if (result == null || result.Count == 0)
                return null;

            return result[0].Image;
        }

        /// <summary>
        /// Fetches all images for a given retail item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item to fetch the image for</param>
        /// <returns>The image for the given retail item or null if no image was found</returns>
        public virtual List<OldItemImage> GetImages(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM RBOINVENTITEMIMAGE WHERE ITEMID=@ItemId AND DATAAREAID=@DATAAREAID ORDER BY IMAGEINDEX";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ItemId", (string)itemID);

                try
                {
                    return Execute<OldItemImage>(entry, cmd, CommandType.Text, PopulateItemImage);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Saves an image as the default image for a given retail item
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item to save the image for</param>
        /// /// <param name="image">The image to be saved</param>
        public virtual void SaveImage(IConnectionManager entry, RecordIdentifier itemID, Image image)
        {
            SaveImage(entry, itemID, image, 1);
        }

        /// <summary>
        /// Saves an image as the default image for a given retail item
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the retail item to save the image for</param>
        /// /// <param name="image">The image to be saved</param>
        /// <param name="index">Index of image. If set to -1, the next available index is found and used</param>
        public virtual void SaveImage(IConnectionManager entry, RecordIdentifier itemID, Image image, int index)
        {
            if (index == -1)
            {
                using (var cmd = entry.Connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT MAX(IMAGEINDEX) FROM RBOINVENTITEMIMAGE WHERE DATAAREAID = @dataAreaId AND ITEMID = @itemId";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "itemId", itemID);

                    var res = entry.Connection.ExecuteScalar(cmd);
                    if (res == null || res == DBNull.Value)
                        index = 0;
                    else
                        index = Convert.ToInt32(res);
                    index++;
                }
            }
            SaveImage(entry, new OldItemImage { ID = itemID, Image = image, ImageIndex = index });
        }

        /// <summary>
        /// Saves an image as the default image for a given retail item
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemImage">The item image to be saved</param>
        public virtual void SaveImage(IConnectionManager entry, OldItemImage itemImage)
        {
            var statement = new SqlServerStatement("RBOINVENTITEMIMAGE");

            if (itemImage.Image == null)
            {
                DeleteImage(entry, itemImage);
                return;
            }

            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ItemsEdit);

            var identifier = new RecordIdentifier(itemImage.ID, new RecordIdentifier(itemImage.ImageIndex));
            if (!RecordExists(entry, "RBOINVENTITEMIMAGE", new[] { "ITEMID", "IMAGEINDEX" }, identifier))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ITEMID", (string)itemImage.ID);
                statement.AddKey("IMAGEINDEX", itemImage.ImageIndex, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ITEMID", (string)itemImage.ID);
                statement.AddCondition("IMAGEINDEX", itemImage.ImageIndex, SqlDbType.Int);
            }
            statement.AddField("IMAGE", FromImage(itemImage.Image), SqlDbType.VarBinary);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Delete an item image from the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemImage"></param>
        public virtual void DeleteImage(IConnectionManager entry, OldItemImage itemImage)
        {
            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ItemsEdit);

            var identifier = new RecordIdentifier(itemImage.ID, new RecordIdentifier(itemImage.ImageIndex));
            if (RecordExists(entry, "RBOINVENTITEMIMAGE", new[] {"ITEMID", "IMAGEINDEX"}, identifier))
            {
                var statement = new SqlServerStatement("RBOINVENTITEMIMAGE") {StatementType = StatementType.Delete};
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ITEMID", (string)itemImage.ID);
                statement.AddCondition("IMAGEINDEX", itemImage.ImageIndex, SqlDbType.Int);
                entry.Connection.ExecuteStatement(statement);
            }
        }

        /// <summary>
        /// Delete all images for an item
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of retail item</param>
        public virtual void DeleteImages(IConnectionManager entry, RecordIdentifier itemID)
        {
            ValidateSecurity(entry, DataLayer.BusinessObjects.Permission.ItemsEdit);

            if (RecordExists(entry, "RBOINVENTITEMIMAGE", "ITEMID", itemID))
            {
                var statement = new SqlServerStatement("RBOINVENTITEMIMAGE") {StatementType = StatementType.Delete};
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ITEMID", (string)itemID);
                entry.Connection.ExecuteStatement(statement);
            }
        }

        /// <summary>
        /// Resequence all images for a particular item in the order they ara passed in itemImages
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemImages">Images to save</param>
        public virtual void ResequenceImages(IConnectionManager entry, List<OldItemImage> itemImages)
        {
            if (itemImages == null || itemImages.Count == 0)
                return;

            var itemID = itemImages[0].ID;
            for (int i = 1; i < itemImages.Count; i++)
            {
                if (itemID != itemImages[i].ID)
                    throw new ArgumentException("All images must belong to the same retail item");
            }

            DeleteImages(entry, itemID);

            for (int i = 0; i < itemImages.Count; i++)
            {
                itemImages[i].ImageIndex = i + 1;
                SaveImage(entry, itemImages[i]);
            }
        }
    }
}
