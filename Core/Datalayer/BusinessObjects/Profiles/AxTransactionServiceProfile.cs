namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public class AxTransactionServiceProfile
    {

        #region Member variables

        // Whether to use the TS
        private bool useTransactionServices;

        // TS Location
        private string hostName;
        private string port;

        // Logon & Configuration
        private string userName;
        private string password;
        private string domain;
        private string company;
        private string configuration;
        private string language;
        private string objectServer;
        private int axVersion;
        private string aosServer;
        private string aosInstance;
        private string aosPort;


        // Functionality settings
        private bool checkCustomerBalance;
        private bool inventoryLookup;
        private bool checkStaff;
        private bool suspendRetrieveTransactions;
        private bool centralizedReturns;
        private bool salesOrders;
        private bool salesInvoices;

        #endregion

        #region Properties

        public bool UseTransactionServices
        {
            get { return useTransactionServices; }
            set { useTransactionServices = value; }
        }

        public string HostName
        {
            get { return hostName; }
            set { hostName = value; }
        }

        public string Port
        {
            get { return port; }
            set { port = value; }
        }

        public int AXVersion
        {
            get { return axVersion; }
            set { axVersion = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Domain
        {
            get { return domain; }
            set { domain = value; }
        }

        public string Company
        {
            get { return company; }
            set { company = value; }
        }

        public string Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        public string Language
        {
            get { return language; }
            set { language = value; }
        }

        public string ObjectServer
        {
            get { return objectServer; }
            set { objectServer = value; }
        }

        public string AOSServer
        {
            get { return aosServer; }
            set { aosServer = value; }
        }

        public string AOSInstance
        {
            get { return aosInstance; }
            set { aosInstance = value; }
        }

        public string AOSPort
        {
            get { return aosPort; }
            set { aosPort = value; }
        }

        public bool CheckCustomerBalance
        {
            get { return checkCustomerBalance; }
            set { checkCustomerBalance = value; }
        }

        public bool InventoryLookup
        {
            get { return inventoryLookup; }
            set { inventoryLookup = value; }
        }

        public bool CheckStaff
        {
            get { return checkStaff; }
            set { checkStaff = value; }
        }

        public bool SuspendRetrieveTransactions
        {
            get { return suspendRetrieveTransactions; }
            set { suspendRetrieveTransactions = value; }
        }

        public bool CentralizedReturns
        {
            get { return centralizedReturns; }
            set { centralizedReturns = value; }
        }

        public bool SalesOrders
        {
            get { return salesOrders; }
            set { salesOrders = value; }
        }

        public bool SalesInvoices
        {
            get { return salesInvoices; }
            set { salesInvoices = value; }
        }


        #endregion
    }
}
