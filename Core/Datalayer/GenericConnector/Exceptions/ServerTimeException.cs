using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.GenericConnector.Exceptions
{
    [global::System.Serializable]
    public class ServerTimeException : Exception
    {
        public ServerTimeException(long serverTicks, long clientTicks)
            : base(string.Format(Properties.Resources.ServerTimeException, new DateTime(clientTicks), new DateTime(serverTicks)))
        {

        }
    }
}
