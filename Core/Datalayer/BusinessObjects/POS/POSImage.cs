using System.Drawing;

namespace LSOne.DataLayer.BusinessObjects.POS
{
    public class POSImage : DataEntity
    {
        public POSImage()
            : base()
        {
        }
        public Image Picture { get; set; }
    }
}
