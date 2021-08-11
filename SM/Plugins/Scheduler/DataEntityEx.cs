using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler
{
    public class DataEntityEx<T> : DataEntity
    {
        private T value;

        public DataEntityEx()
        {
        }

        public DataEntityEx(RecordIdentifier id, string text)
            : base(id, text)
        {
        }

        public DataEntityEx(RecordIdentifier id, string text, T value)
            : base(id, text)
        {
            this.value = value;
        }

        public DataEntityEx(string code, string text, T value)
            : base(code, text)
        {
            this.value = value;
        }

        public T Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
