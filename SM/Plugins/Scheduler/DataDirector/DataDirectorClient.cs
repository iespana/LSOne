using System;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.DataDirector
{
    internal class DataDirectorClient : IDataDirectorClient
    {
        private LSRetail.DD.Control.TransAutomClient client;
        public DataDirectorClient()
        {
            client = new LSRetail.DD.Control.TransAutomClient();
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (client != null)
                {
                    client.Disconnect();
                    client = null;
                }
            }
        }

        public void SetConnTimeOut(int nTimeOut)
        {
            client.SetConnTimeOut(nTimeOut);
        }

        public void SetCopyright(string stCopyright)
        {
            client.SetCopyright(stCopyright);
        }

        public void CreateConnection(string stServer, string stConnString, bool bInteractive)
        {
            client.CreateConnection(stServer, stConnString, bInteractive);
        }

        public void SetRequestType(short nReqType)
        {
            client.SetRequestType(nReqType);
        }

        public void SetResultType(short nResType)
        {
            client.SetResultType(nResType);
        }

        public void SetTable(string stTableName)
        {
            client.SetTable(stTableName);
        }

        public void AddField(string stFieldName, short nFieldType)
        {
            client.AddField(stFieldName, nFieldType);
        }

        public void AddParamField(string stFieldName, short nFieldType, short nOperator)
        {
            client.AddParamField(stFieldName, nFieldType, nOperator);
        }

        public void AddParam(object vParamValue)
        {
            client.AddParam(vParamValue);
        }

        public bool Send()
        {
            return client.Send();
        }

        public bool HasAnswer()
        {
            return client.HasAnswer();
        }

        public bool NextValueList()
        {
            return client.NextValueList();
        }

        public object GetValueNo(int nValueNo)
        {
            return client.GetValueNo(nValueNo);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        public void SetDefaultPort(int nDefPort)
        {
            client.SetDefaultPort(nDefPort);
        }

        public void SetMsgId(string stMsgId)
        {
            client.SetMsgId(stMsgId);
        }

        public void SetMaxRec(int nMaxRecs)
        {
            client.SetMaxRec(nMaxRecs);
        }

        public int GetLastError()
        {
            return client.GetLastError();
        }
    }
}
