namespace LSOne.Services.Interfaces.SupportClasses.ImportExport
{
    public class WorksheetHandle
    {
        public WorksheetHandle(object internalWorksheet)
        {
            this.InternalWorksheet = internalWorksheet;
        }

        public object InternalWorksheet {get; private set;}
    }
}
