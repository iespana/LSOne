using System;
using System.Security;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.Cryptography;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportClasses
{
    public class SQLServerLoginEntry : ListViewItem
    {
        bool windowsAuthentication;
        string login;
        SecureString password;
        Guid id;
        string databaseName;
        ConnectionType connectionType;
        string dataAreaID;
        string serverName;
        RecordIdentifier storeID;
        RecordIdentifier terminalID;
        string connectionManagerType;
        private string description;


        public SQLServerLoginEntry()
            :this("")
        {
            connectionManagerType = "";
        }

        public SQLServerLoginEntry(string name)
            : this(Guid.NewGuid(), name, true, "", SecureStringHelper.FromString(""), "", "", "", false, "")
        {
        }

        // Site Manager constructor
        public SQLServerLoginEntry(Guid id, string name, bool windowsAuthentication, string login, SecureString password, string databaseName, string description)
        {
            this.id = id;
            this.description = description;
            Text = description;
            this.windowsAuthentication = windowsAuthentication;
            this.login = login;
            this.password = password;
            this.databaseName = databaseName;
            this.ImageIndex = 0;
            this.storeID = 0; // Zero means that no selection has been done, RecordIdentifier.Empty means head office, and other value indicate specific store
            this.serverName = name;
            connectionManagerType = "";
            connectionType = ConnectionType.NamedPipes;
            ServerName = serverName;
        }

        // POS constructor
        public SQLServerLoginEntry(
            Guid id, 
            string name, 
            bool windowsAuthentication, 
            string login, 
            SecureString password, 
            string databaseName, 
            RecordIdentifier storeID, 
            RecordIdentifier terminalID, 
            bool askBeforeUpdatingDatabase,
            string description)
            : this(id, name, windowsAuthentication, login, password, databaseName, description)
        {
            this.storeID = storeID;
            this.terminalID = terminalID;
            AskBeforeUpdatingDatabase = askBeforeUpdatingDatabase;
        }

        public string ServerName
        {
            get { return serverName; }
            set
            {
                serverName = value;
                if (Description == null || Description == "")
                {
                    Text = serverName + " - " + databaseName;
                }
            }
        }

        public bool WindowsAuthentication
        {
            get { return windowsAuthentication; }
            set { windowsAuthentication = value; }
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public SecureString Password
        {
            get { return password; }
            set { password = value; }
        }

        public ConnectionType ConnectionType
        {
            get { return connectionType; }
            set { this.connectionType = value; }
        }

        public string DatabaseName
        {
            get { return databaseName; }
            set
            {
                databaseName = value;
                if (Description == null || Description == "")
                {
                    Text = serverName + " - " + databaseName;
                }
            }
        }

        public string DataAreaID
        {
            get { return dataAreaID; }
            set { dataAreaID = value; }
        }

        public RecordIdentifier StoreID
        {
            get { return storeID; }
            set { storeID = value; }
        }

        public RecordIdentifier TerminalID
        {
            get { return terminalID; }
            set { terminalID = value; }
        }

        public string ConnectionManagerType
        {
            get { return connectionManagerType; }
            set { connectionManagerType = value; }
        }

        public Guid ID
        {
            get { return id; }
        }

        public bool AskBeforeUpdatingDatabase { get; set; }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                Text = description;
            }
        }

        public override object Clone()
        {
            SQLServerLoginEntry newEntry = new SQLServerLoginEntry(id, serverName, windowsAuthentication, login, password, databaseName, storeID, terminalID, AskBeforeUpdatingDatabase, Description);

            newEntry.ConnectionType = connectionType;
            newEntry.DataAreaID = dataAreaID;
            newEntry.StoreID = storeID;
            newEntry.ConnectionManagerType = connectionManagerType;

            return newEntry;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
