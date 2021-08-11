namespace LSOne.Services.Interfaces.SupportClasses.ImportExport
{
    public class WorksheetCell
    {
        public WorksheetCell()
        {
            Options = new CellOptions();
        }

        public int Row { get; set; }
        public int Column { get; set; }

        /// <summary>
        /// E.g. A1, B2, A1:C4
        /// </summary>
        public string CellReference { get; set; }

        public CellOptions Options { get; set; }

        public object Value { get; set; }
    }
}
