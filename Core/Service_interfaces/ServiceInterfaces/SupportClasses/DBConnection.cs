using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.Services.Interfaces.SupportClasses
{
    public class DBConnection
    {
        public string DBServer { get; set; }
        public bool WindowsAuthentication { get; set; }
        public string DBUser { get; set; }
        public SecureString DBPassword { get; set; }
        public string DBName { get; set; }
        public string SystemUser { get; set; }
        public SecureString SystemPassword { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public ConnectionUsageType ConnectionUsageType { get; set; }
        public string DataAreaID { get; set; }


    }
}
