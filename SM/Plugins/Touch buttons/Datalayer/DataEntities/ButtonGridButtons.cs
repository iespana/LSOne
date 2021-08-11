using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.BusinessObjects;

namespace LSRetail.StoreController.TouchButtons.Datalayer.DataEntities
{
    internal class ButtonGridButtons : DataEntity
    {
        
        public ButtonGridButtons()
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

    }
}
