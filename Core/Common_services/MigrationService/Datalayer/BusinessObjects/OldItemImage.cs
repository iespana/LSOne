using LSOne.DataLayer.BusinessObjects;
using System.Drawing;
namespace LSOne.Services.Datalayer.BusinessObjects
{
    /// <summary>
    /// Data entity class for item images
    /// </summary>
    public class OldItemImage : DataEntity
    {
        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public OldItemImage()
        {
        }

        /// <summary>
        /// The index of the image. The default image has index 1
        /// </summary>
        public int ImageIndex { get; set; }

        /// <summary>
        /// The image
        /// </summary>
        public Image Image { set; get; }
    }
}
 