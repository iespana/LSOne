using System;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.DataDirector
{
    public interface IDataDirectorClient : IDisposable
    {
        void SetConnTimeOut(int nTimeOut);
        void SetCopyright(string stCopyright);
        void CreateConnection(string stServer, string stConnString, bool bInteractive);
        void SetRequestType(short nReqType);
        void SetResultType(short nResType);
        void SetTable(string stTableName);
        void AddField(string stFieldName, short nFieldType);
        void AddParamField(string stFieldName, short nFieldType, short nOperator);
        void AddParam(object vParamValue);
        bool Send();
        bool HasAnswer();
        bool NextValueList();
        object GetValueNo(int nValueNo);
        void Disconnect();
        void SetDefaultPort(int nDefPort);
        void SetMsgId(string stMsgId);
        void SetMaxRec(int nMaxRecs);
        int GetLastError();
    }
}
