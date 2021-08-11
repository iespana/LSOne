using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.ListViewExtensions;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.SupportClasses
{
    /// <summary>
    /// Used to manage the ItemImageCell cells on the item search dialog
    /// </summary>
    internal class ImageCellDictionaryManager
    {

        private Dictionary<int, ItemImageCell> imageRequests;
        //private Dictionary<int, ItemImageCell> priceRequests; 
        private object lockObject;
        private object invalidateLockObject;
        private IConnectionManager dataModel;
        private bool invalidated;
        private bool showPrice;

        public ImageCellDictionaryManager(IConnectionManager dataModel)
        {
            imageRequests = new Dictionary<int, ItemImageCell>();
            lockObject = new object();
            invalidateLockObject = new object();
            this.dataModel = dataModel;
            invalidated = false;
        }

        public void AddRequest(ItemImageCell cell)
        {

            lock (lockObject)
            {
                int cellHash = cell.GetHashCode();

                if (!imageRequests.ContainsKey(cellHash))
                {
                    imageRequests.Add(cellHash, cell);
                }
            }
        }

        public void ClearImageFromCell(ItemImageCell cell)
        {
            lock (lockObject)
            {
                int cellHash = cell.GetHashCode();

                if (imageRequests.ContainsKey(cellHash))
                {
                    imageRequests.Remove(cellHash);
                }

                if (cell.Image != null)
                {
                    cell.ClearImage();
                }
            }
        }

        /// <summary>
        /// Indicates wether the ItemImageCell cells should show price or not
        /// </summary>
        public bool ShowPrice
        {
            get
            {
                return showPrice;
            }
            set
            {
                showPrice = value;
            }
        }

        public bool HasRequests
        {
            get
            {
                lock (lockObject)
                {
                    return imageRequests.Count > 0;
                }
            }
        }

        public bool RequestExistsForCell(ItemImageCell cell)
        {
            lock (lockObject)
            {
                return imageRequests.ContainsKey(cell.GetHashCode());
            }
        }

        public bool Invalidated
        {        
            get
            {
                lock (invalidateLockObject)
                {
                    return invalidated;                    
                }
            }
            set
            {
                lock (invalidateLockObject)
                {
                    invalidated = value;
                }
            }
        }

        public void LoadImageRequests()
        {
            lock (lockObject)
            {
                if (imageRequests.Count == 0)
                {
                    return;
                }

                List<RecordIdentifier> itemIDs = new List<RecordIdentifier>();

                // Go through every Cell currently in the dictionary, load the image and then remove it
                foreach (KeyValuePair<int, ItemImageCell> requestPair in imageRequests)
                {
                    itemIDs.Add(requestPair.Value.RetailItem.MasterID);
                }

                List<ItemImage> images = Providers.RetailItemData.GetDefaultImages(dataModel.CreateTemporaryConnection(), itemIDs);
                ItemImage itemImage;
                int imageCounter = 0;
                foreach (KeyValuePair<int, ItemImageCell> requestPair in imageRequests)
                {
                    itemImage = null;
                    
                    if (images[imageCounter].ItemMasterID == requestPair.Value.RetailItem.MasterID || images[imageCounter].ItemMasterID == requestPair.Value.RetailItem.HeaderItemID)
                    {
                        itemImage = images[imageCounter];

                        requestPair.Value.Image = itemImage.Image ?? DLLEntry.Settings.DefaultItemImage;
                    }

                    if ( itemImage?.Image == null)
                    {
                        requestPair.Value.ShowNoImage = true;
                        requestPair.Value.Image = DLLEntry.Settings.DefaultItemImage;
                    }
                    imageCounter++;
                }

                imageRequests.Clear();

                invalidated = true;
            }
        }
    }
}
