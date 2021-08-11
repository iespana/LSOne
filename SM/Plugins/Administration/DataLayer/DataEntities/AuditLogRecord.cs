using System;
using System.Collections;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Administration.DataLayer.DataEntities
{
    [Serializable()]
    public class AuditLogRecord
    {
        private int       id;
        private Guid      userGuid;
        private string    userLogin;
        private DateTime  date;
        private ArrayList fieldValues;

        public AuditLogRecord(int id,Guid userGuid,string userLogin,DateTime date,ArrayList fieldValues)
        {
            this.id          = id;
            this.userGuid    = userGuid;
            this.userLogin   = userLogin;
            this.date        = date;
            this.fieldValues = fieldValues;
        }

        public int ID
        {
            get { return id; }
            private set { id = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            private set { userGuid = value; }
        }

        public string UserLogin
        {
            get { return userLogin; }
            private set { userLogin = value; }
        }

        public DateTime Date
        {
            get { return date; }
            private set { date = value; }
        }

        public ArrayList FieldValues
        {
            get { return fieldValues; }
            private set { fieldValues = value; }
        }
    }
}

