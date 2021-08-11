namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    public class ButtonGrid : DataEntity
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
