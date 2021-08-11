using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Store.Dialogs.WizardPages;

namespace LSOne.ViewPlugins.Store.Dialogs
{
    public partial class CreateStoreWizard : WizardBase
    {

        public CreateStoreWizard(IConnectionManager connection)
            : base(connection)
        {
            InitializeComponent();

            HasHelp = false;

            AddPage(new CreateStoreFirstPage(this));
        }

        public CreateStoreWizard()
        {
            InitializeComponent();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnFinish(List<IWizardPage> pages, ref bool cancelAction)
        {
            /*SCLicenseService.ServiceSoapClient client;
            SCLicenseService.ArrayOfGuid guids;
            WizardPages.ActivationListInstancesPage listPage;
            WizardPages.ActivationManualResponsePage phonePage;
            string licences1;
            string licences2;
            int intLicense1;
            int intLicense2;

            if (pages[pages.Count - 1] is WizardPages.ActivationManualResponsePage)
            {
                phonePage = (WizardPages.ActivationManualResponsePage)pages[pages.Count - 1];

                licences1 = phonePage.Licences1;
                licences2 = phonePage.Licences2;

                string securityCode = HMAC_SHA1.GetValue(phonePage.InstanceCode + licences1 + licences2 + phonePage.MachineNameHash, "bbfbe460-bf3a-11df-851a-0800200c9a66").Left(8);

                if (phonePage.SecurityCode != securityCode)
                {
                    MessageDialog.Show(Properties.Resources.IncorrectPhoneActivation);
                    cancelAction = true;
                    return;
                }

                customerKey = phonePage.CustomerKey;
                instanceNumber = phonePage.InstanceNumber;

                intLicense1 = Int32.Parse(licences1, System.Globalization.NumberStyles.HexNumber);
                intLicense2 = Int32.Parse(licences2, System.Globalization.NumberStyles.HexNumber);

                licenseGuids = new List<Guid>();

                if ((intLicense1 & 0x1) == 0x1) licenseGuids.Add(new Guid("0040e960-b1e7-11df-94e2-0800200c9a66"));  // Barcodes
                if ((intLicense1 & 0x2) == 0x2) licenseGuids.Add(new Guid("2ae35ef0-b1e7-11df-94e2-0800200c9a66"));  // Customer 
                if ((intLicense1 & 0x4) == 0x4) licenseGuids.Add(new Guid("5957eb20-b1e7-11df-94e2-0800200c9a66"));  // Dimensions 
                if ((intLicense1 & 0x8) == 0x8) licenseGuids.Add(new Guid("75c01ad0-b1e7-11df-94e2-0800200c9a66"));  // End of day
                if ((intLicense1 & 0x10) == 0x10) licenseGuids.Add(new Guid("aede7320-b1e7-11df-94e2-0800200c9a66"));  // Forms
                if ((intLicense1 & 0x20) == 0x20) licenseGuids.Add(new Guid("cd4f9230-b1e7-11df-94e2-0800200c9a66"));  // Hospitality
                if ((intLicense1 & 0x40) == 0x40) licenseGuids.Add(new Guid("18b30a40-b1e8-11df-94e2-0800200c9a66"));  // Periodic discounts
                if ((intLicense1 & 0x80) == 0x80) licenseGuids.Add(new Guid("746ddbd0-b1e8-11df-94e2-0800200c9a66"));  // POS User
                if ((intLicense1 & 0x100) == 0x100) licenseGuids.Add(new Guid("a36357d0-b1e8-11df-94e2-0800200c9a66"));  // Profiles
                if ((intLicense1 & 0x200) == 0x200) licenseGuids.Add(new Guid("d291e530-b1e8-11df-94e2-0800200c9a66"));  // Receipt browser
                if ((intLicense1 & 0x400) == 0x400) licenseGuids.Add(new Guid("3d90cb80-b1e9-11df-94e2-0800200c9a66"));  // DD Sceduler
                if ((intLicense1 & 0x800) == 0x800) licenseGuids.Add(new Guid("6f46c710-b1e9-11df-94e2-0800200c9a66"));  // Retail items
                if ((intLicense1 & 0x1000) == 0x1000) licenseGuids.Add(new Guid("84dbd520-b1e9-11df-94e2-0800200c9a66"));  // Sales Tax
                if ((intLicense1 & 0x2000) == 0x2000) licenseGuids.Add(new Guid("9ff69d40-b1e9-11df-94e2-0800200c9a66"));  // Store
                if ((intLicense1 & 0x4000) == 0x4000) licenseGuids.Add(new Guid("b7fd2080-b1e9-11df-94e2-0800200c9a66"));  // Touch buttons
                if ((intLicense1 & 0x8000) == 0x8000) licenseGuids.Add(new Guid("e2e52a90-b1e9-11df-94e2-0800200c9a66"));  // Trade agreements
                if ((intLicense1 & 0x10000) == 0x10000) licenseGuids.Add(new Guid("10915db0-b1ea-11df-94e2-0800200c9a66"));  // Xtra Reports viewer
                if ((intLicense1 & 0x20000) == 0x20000) licenseGuids.Add(new Guid("3e9bc6f0-b41a-11df-8d81-0800200c9a66"));  // Lookup values
                if ((intLicense1 & 0x40000) == 0x40000) licenseGuids.Add(new Guid("85eaa120-b41a-11df-8d81-0800200c9a66"));  // LS Licensing
                if ((intLicense1 & 0x80000) == 0x80000) licenseGuids.Add(new Guid("2fab3fe0-b1ea-11df-94e2-0800200c9a66"));  // Inventory
                if ((intLicense1 & 0x100000) == 0x100000) licenseGuids.Add(new Guid("365f8ee0-b1ea-11df-94e2-0800200c9a66"));  // Infocodes
                if ((intLicense1 & 0x200000) == 0x200000) licenseGuids.Add(new Guid("3c161cf0-b1ea-11df-94e2-0800200c9a66"));  // DD Monitoring
                if ((intLicense1 & 0x400000) == 0x400000) licenseGuids.Add(new Guid("4239ebc0-b1ea-11df-94e2-0800200c9a66"));  // Excel Importer
                if ((intLicense1 & 0x800000) == 0x800000) licenseGuids.Add(new Guid("48f65110-b1ea-11df-94e2-0800200c9a66"));  // US configuration
                if ((intLicense1 & 0x1000000) == 0x1000000) licenseGuids.Add(new Guid("4fe9f300-b1ea-11df-94e2-0800200c9a66"));  // Unused6
                if ((intLicense1 & 0x2000000) == 0x2000000) licenseGuids.Add(new Guid("5a876310-b1ea-11df-94e2-0800200c9a66"));  // Unused7
                if ((intLicense1 & 0x4000000) == 0x4000000) licenseGuids.Add(new Guid("64adce60-b1ea-11df-94e2-0800200c9a66"));  // Unused8
                if ((intLicense1 & 0x8000000) == 0x8000000) licenseGuids.Add(new Guid("6b971010-b1ea-11df-94e2-0800200c9a66"));  // Unused9
                if ((intLicense1 & 0x10000000) == 0x10000000) licenseGuids.Add(new Guid("75be8cd0-b1ea-11df-94e2-0800200c9a66"));  // Unused10


            }
            else if (pages[pages.Count - 1] is WizardPages.ActivationListInstancesPage)
            {
                listPage = (WizardPages.ActivationListInstancesPage)pages[pages.Count - 1];

                customerKey = listPage.CustomerKey;
                instanceNumber = listPage.InstanceNumber;

                try
                {
                    EndpointAddress endpointAddress = new EndpointAddress("http://dotnetlicense.lsretail.com/Service.asmx");
                    BasicHttpBinding serviceBinding = new BasicHttpBinding { SendTimeout = new TimeSpan(0, 0, 60), ReceiveTimeout = new TimeSpan(0, 0, 60) };

                    client = new SCLicenseService.ServiceSoapClient(serviceBinding, endpointAddress);

                    guids = client.TakeInstance(listPage.InstanceNumber, listPage.CustomerKey, Hardware.GetMACAddress(), System.Environment.MachineName);

                    licenseGuids = new List<Guid>();

                    foreach (Guid guid in guids)
                    {
                        licenseGuids.Add(guid);
                    }
                }
                catch
                {
                    cancelAction = true;
                    MessageDialog.Show(Properties.Resources.CouldNotConnectToLicenseWebService);
                }
            }*/
        }

       
     
    }
}