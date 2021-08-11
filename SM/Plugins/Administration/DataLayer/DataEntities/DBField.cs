using System.Data;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Administration.DataLayer.DataEntities
{
    internal class DBField
    {
        public DBField()
        {

        }

        public string Name { get; internal set; }
        public SqlDbType DBType { get; internal set; }
        public bool IsPrimaryKey { get; internal set; }
        public bool IsIdentity { get; set; }
        public object Value { get; set; }
    }
}
