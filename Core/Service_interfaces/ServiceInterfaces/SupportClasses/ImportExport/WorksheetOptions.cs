using System.Collections.Generic;

namespace LSOne.Services.Interfaces.SupportClasses.ImportExport
{
    public class WorksheeetOptions
    {
        // Magic numbers
        public static int AutoFit = -1000000; // Autofit will only work *after* values have been set to columns/rows

        public WorksheeetOptions()
        {
        }

        public bool IsHidden { get; set; }
        public OrientationEnum Orientation { get; set; }
        //
        // Summary:
        //     If true, MS Excel shows columns from right to left.
        public bool RightToLeft { get; set; }

        public int FrozenRow { get; set; }
        public int FrozenColumn { get; set; }

        public enum OrientationEnum
        {
            // Summary:
            //     Landscape worksheet orientation.
            Landscape = 0,
            //
            // Summary:
            //     Portrait worksheet orientation.
            Portrait = 2,
        }

        public Dictionary<int, int> RowHeights;
        public Dictionary<int, int> ColumnWidths;
    }
}