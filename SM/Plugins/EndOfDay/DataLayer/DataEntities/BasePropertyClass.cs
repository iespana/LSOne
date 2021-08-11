using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.EndOfDay.DataLayer.DataEntities
{
    public abstract class BasePropertyClass
    {
        private string[] tableNames;
        private string[] keyFieldNames;

        public string[] TableNames
        {
            get { return tableNames; }
            set { tableNames = value; }
        }
        public string[] KeyFieldNames
        {
            get { return keyFieldNames; }
            set { keyFieldNames = value; }
        }

        public abstract string[] GetIDs();

        public BasePropertyClass()
        {
        }

        public BasePropertyClass(string[] tableNames, string [] keyFieldNames)
        {
            this.tableNames = tableNames;
            this.keyFieldNames = keyFieldNames;
        }
        public BasePropertyClass(string[] tableNames)
        {
            this.tableNames = tableNames;
        }
    }
}
