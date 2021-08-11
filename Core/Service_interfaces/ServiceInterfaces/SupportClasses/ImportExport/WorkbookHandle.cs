namespace LSOne.Services.Interfaces.SupportClasses.ImportExport
{
    public class WorkbookHandle
    {
        public WorkbookHandle(object internalWorkbook)
        {
            this.InternalWorkbook = internalWorkbook;
        }

        public object InternalWorkbook {get; private set;}
    }
}
