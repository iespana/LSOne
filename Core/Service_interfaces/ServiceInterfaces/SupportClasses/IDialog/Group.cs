using LSOne.Services.Interfaces.Enums;

namespace LSOne.Services.Interfaces.SupportClasses.IDialog
{
    public class Group
    {
        public Group()
        {
            GroupId = "";
            Text = "";
            NumberOfClicks = 0;
            MinSelection = 0;
            MaxSelection = 0;
            Index = -1;
            selectionCompleted = false;
            IsVariantGroup = false;
            MultipleSelection = true;
        }

        private int numberOfClicks;
        private bool selectionCompleted;
        private Dimensions dimension;

        public enum Dimensions { Dimension1, Dimension2, Dimension3 }

        public int Index { get; set; }
        public string GroupId { get; set; }
        public string Text { get; set; }
        public string GroupHeader { get; set; }
        public bool InputRequired { get; set; }
        public int PrevSelection { get; set; }
        public int MaxSelection { get; set; }
        public int MinSelection { get; set; }
        public bool IsVariantGroup { get; set; }
        public bool MultipleSelection { get; set; }
        public UsageCategories UsageCategory { get; set; }
        public OKPressedActions OKPressdAction { get; set; }
        public Dimensions Dimension
        {
            get { return dimension; }
            set
            {
                dimension = value;
                switch (dimension)
                {
                    case Dimensions.Dimension1: GroupId = "Dim1";
                        break;
                    case Dimensions.Dimension2: GroupId = "Dim2";
                        break;
                    case Dimensions.Dimension3: GroupId = "Dim3";
                        break;
                    default:
                        break;
                }
            }
        }

        public bool SelectionCompleted
        {
            get { return selectionCompleted; }
        }

        public int NumberOfClicks
        {
            get { return numberOfClicks; }
            set
            {
                numberOfClicks = value;
                if ((InputRequired && numberOfClicks >= MinSelection) || !InputRequired)
                    selectionCompleted = true;
            }
        }
    }
}
