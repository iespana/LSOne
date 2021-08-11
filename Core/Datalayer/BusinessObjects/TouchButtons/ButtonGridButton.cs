using System.Drawing;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    public class ButtonGridButton : DataEntity
    {
        
        public ButtonGridButton()
        {
            Col = 0;
            ColSpan = 0;
            RowNumber = 0;
            RowSpan = 0;
            Action = 0;
            ActionProperty = "";
            PictureID = -1; // buttons with -1 have no picture
            DisplayText = "";
            Colour = 0;
            FontSize = 0;
            FontStyle = 0;
            ButtonGridID = "";
            ImageAlignment = 0;
            Picture = null;
        }

        public int Col { get; set; }
        public int ColSpan { get; set; }
        public int RowNumber { get; set; }
        public int RowSpan { get; set; }
        public int Action { get; set; }
        public string ActionProperty { get; set; }
        public RecordIdentifier PictureID { get; set; }
        public string DisplayText { get; set; }
        public int Colour { get; set; }
        public int FontSize { get; set; }
        public int FontStyle { get; set; }
        public RecordIdentifier ButtonGridID { get; set; }
        public int ImageAlignment { get; set; }
        public Image Picture { get; set; }
    }
}
