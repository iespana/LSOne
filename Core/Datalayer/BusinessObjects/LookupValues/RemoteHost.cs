using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.LookupValues
{
    public class RemoteHost : DataEntity
    {

        public RemoteHost()
            : base()
        {
            ID = RecordIdentifier.Empty;
            Text = "";
            Address = "";
            Port = 0;
        }

        public string Address { get; set; }
        public int Port { get; set; }
    }
}
