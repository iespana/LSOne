using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class RestaurantMenuType : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(RestaurantID, MenuOrder);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public RestaurantMenuType()
        {
            RestaurantID = "";
            MenuOrder = 0;
            Text = "";
            CodeOnPos = "";
        }

        public RecordIdentifier RestaurantID { get; set; }
        public RecordIdentifier MenuOrder { get; set; }        
        public string CodeOnPos { get; set; }

        public string DisplayText
        {
            get
            {
                string txt = Text;
                if (CodeOnPos != "")
                {
                    txt += " (" + CodeOnPos + ")";
                }

                return txt;
            }
        }
    }
}
