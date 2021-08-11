using System.Drawing;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.POS
{
    public class POSKeyboardButtonControl : DataEntity
    {
        public POSKeyboardButtonControl()
            : base()
        {
            ButtonControlID = RecordIdentifier.Empty;
            RowNumber = 0;
            Type = RecordIdentifier.Empty;
            OperationID = RecordIdentifier.Empty;
            ShowMenuID = RecordIdentifier.Empty;
            Action = 0;
            ActionProperty = "";
            DisplayText = "";
            Colour = "";
            FontSize = 0;
        }

        public RecordIdentifier ButtonControlID { get; set; }
        public int RowNumber { get; set; }
        public RecordIdentifier Type { get; set; }
        public RecordIdentifier OperationID { get; set; }
        public RecordIdentifier ShowMenuID { get; set; }
        public int Action { get; set; }
        public string ActionProperty { get; set; }
        public Image Picture { get; set; }
        public string DisplayText { get; set; }
        public string Colour { get; set; }
        public int FontSize { get; set; }
        public int FontStyle { get; set; }
        public string Alles { get; set; }

    }
}
