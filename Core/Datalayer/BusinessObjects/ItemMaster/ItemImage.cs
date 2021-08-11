using System.Drawing;
using System.Runtime.Serialization;

using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.ItemMaster")]
namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
	/// <summary>
	/// Data entity class for item images
	/// </summary>
	[DataContract]
	[KnownType(typeof(RecordIdentifier))]
	public class ItemImage : OptimizedUpdateDataEntity
	{
		private RecordIdentifier itemMasterID;
		private RecordIdentifier imageID;
		private int imageIndex;
		private Image image;
		private string imageName;

		/// <summary>
		/// The constructor for the class. All variables are set to default values
		/// </summary>
		public ItemImage()
		{
			itemMasterID = RecordIdentifier.Empty;
			imageID = RecordIdentifier.Empty;
			imageIndex = -1;
			image = null;
			imageName = string.Empty;
		}

		/// <summary>
		/// Gets or sets the unique identifier for the item
		/// </summary>
		[DataMember]
		public RecordIdentifier ItemMasterID
		{
			get
			{ return itemMasterID; }
			set
			{
				itemMasterID = value;
				PropertyChanged("ITEMMASTERID");
			}
		}

		/// <summary>
		/// Gets or sets the unique identifier for the image
		/// </summary>
		[DataMember]
		public RecordIdentifier ImageID
		{
			get
			{ return imageID; }
			set
			{
				imageID = value;
				PropertyChanged("IMAGEID");
			}
		}

		/// <summary>
		/// Gets or sets the index of the image. The default image has index 1.
		/// </summary>
		[DataMember]
		public int ImageIndex
		{
			get
			{ return imageIndex; }
			set
			{
				imageIndex = value;
				PropertyChanged("IMAGEINDEX");
			}
		}

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		/// <remarks>The internal image hash is also cleared during set.</remarks>
		public Image Image
		{
			get
			{ return image; }
			set
			{
				image = value;
				PropertyChanged("IMAGE");
			}
		}

		/// <summary>
		/// Gets or sets the file name of the image.
		/// </summary>
		[DataMember]
		public string ImageName
		{
			get
			{ return imageName; }
			set
			{
				imageName = value;
				PropertyChanged("IMAGENAME");
			}
		}

		/// <summary>
		/// Gets or sets <see cref="ItemImage.Image"/>, in base64 string format.
		/// </summary>
		/// <remarks>
		/// Used for binary serialization and deserialization, since <see cref="System.Drawing.Image"/> is abstract.
		/// </remarks>
		/// <exception>
		/// Property does not throw any exception - it will set <see cref="ItemImage.Image"/> to null if string is in incorrect format or an empty string.
		/// </exception>
		[DataMember]
		public string ImageAsString {
			set
			{
				image = FromBase64(value);
			}
			get {
				return ToBase64(this.Image);
			}
		}

		/// <summary>
		/// Validates if <see cref="ItemImage.ItemMasterID"/> has a valid <see cref="System.Guid"/> value.
		/// </summary>
		/// <returns></returns>
		public bool HasValidItemMasterID()
		{
			return !RecordIdentifier.IsEmptyOrNull(ItemMasterID) && ItemMasterID.IsGuid;
		}

		/// <summary>
		/// Validates if <see cref="DataEntity.ID"/> has a valid <see cref="System.String"/> value.
		/// </summary>
		/// <returns></returns>
		public bool HasValidItemID()
		{
			if(RecordIdentifier.IsEmptyOrNull(ID) || string.IsNullOrWhiteSpace((string)ID))
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Validates if this <see cref="ItemImage"/> has a valid item assigned  item ID or item master ID)
		/// </summary>
		/// <returns></returns>
		public bool HasValidItem()
		{
			return HasValidItemMasterID() || HasValidItemID();
		}

		/// <summary>
		/// Compares the content of the 2 images and returns true if the underlying byte arrays are equal.
		/// </summary>
		/// <param name="compareToImage"></param>
		/// <returns></returns>
		public bool SameImageAs(Image compareToImage)
		{
			byte[] currentImage = ImageUtils.ImageToByteArray(image);
			byte[] secondImage = ImageUtils.ImageToByteArray(compareToImage);

			return ImageUtils.CompareByteArray(currentImage, secondImage);
		}
	}
}
 