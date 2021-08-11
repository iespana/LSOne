using System;

namespace LSOne.DataLayer.GenericConnector.Constants
{
    public class Settings
    {
        public static Guid DatabaseSerialNumber = new Guid("0d44bba0-3041-11df-9aae-0800200c9a66");
        public static Guid WriteAuditing = new Guid("17e851c0-3037-11df-9aae-0800200c9a66");
        public static Guid OperationAuditing = new Guid("5eb2a48e-ffec-4acd-82ac-4f8929ee7d15");
        public static Guid NamingFormat = new Guid("2cf043aa-b7c2-4158-90ee-69ac5b7aec32");
        public static Guid AddressFormat = new Guid("f67862df-49a7-4da6-bf52-14632064a428");

        public static Guid ActiveDirectoryEnabled = new Guid("be5cd3f0-69c5-11db-bd13-0800200c9a66");
        public static Guid ActiveDirectoryDomain = new Guid("e489e620-a91a-11de-8a39-0800200c9a66");

        public static Guid PasswordExpires = new Guid("7cb84d26-b28b-4086-8dcf-646f68cef956");
        public static Guid PasswordLockoutThreshold = new Guid("6278ea02-cc60-4ad2-bea6-88cd0a8312ab");

        public static Guid MaxOverReceiveGoods = new Guid("2880A888-7EB6-49B3-AC1C-1024DE0F9EE4");
        public static Guid BlindReceivingPurchaseOrder = new Guid("47602dcc-ba81-45fe-acf8-8033e9fcc7a9");
        public static Guid CostCalculation = new Guid("2BAB2653-C366-480E-8DC2-99107BC03D5F");

        public static Guid DisableCreatingReplicationActions = new Guid("BA8B1D5B-8E5E-47E2-98BF-53BEDD6D53A8");

        public static Guid OmniJobUpdateInterval = new Guid("8E4FB9D9-ABDB-42F3-B625-02573B5022DC");
        public static Guid OmniJobMaxRetryCounter = new Guid("13B9DA71-5FC7-4D34-A3E5-BD1BECDAFD74");
    }

}
