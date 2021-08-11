using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.BusinessObjects;

namespace LSRetail.StoreController.TouchButtons.Datalayer.DataEntities
{
    internal class ButtonGrid : DataEntity
    {
        public ButtonGrid()
        {
            Font = "";
            KeyboardUsed = "";
        }

        public int SpaceBetweenButtons { get; set; }
        public string Font { get; set; }
        public string KeyboardUsed { get; set; }
        public int DefaultColor { get; set; }
        public int DefaultFontSize { get; set; }
        public int DefaultFontStyle { get; set; }

    }
}
