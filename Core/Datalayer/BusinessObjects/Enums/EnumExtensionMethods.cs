using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public static class EnumExtensionMethods
    {
        public static string ToLocalizedString(this ReceiptSettingsEnum receiptSettings)
        {
            switch (receiptSettings)
            {
                case ReceiptSettingsEnum.Printed:
                    return Properties.Resources.PrintOnPOS;
                case ReceiptSettingsEnum.Email:
                    return Properties.Resources.SendEmail;
                case ReceiptSettingsEnum.PrintAndEmail:
                    return Properties.Resources.Both;
                case ReceiptSettingsEnum.Ignore:
                    return Properties.Resources.Ignore;
                default:
                    return receiptSettings.ToString();
            }
        }

        public static string ToLocalizedString(this GenderEnum gender)
        {
            switch (gender)
            {
                case GenderEnum.None:
                    return Properties.Resources.NotAvailable;
                case GenderEnum.Female:
                    return Properties.Resources.Female;
                case GenderEnum.Male:
                    return Properties.Resources.Male;
                default:
                    return gender.ToString();
            }
        }

        public static string ToLocalizedString(this BlockedEnum blocking)
        {
            switch (blocking)
            {
                case BlockedEnum.Nothing:
                    return Properties.Resources.CustomerNotBlocked;
                case BlockedEnum.Invoice:
                    return Properties.Resources.CustomerLimitedToInvoices;
                case BlockedEnum.All:
                    return Properties.Resources.CustomerBlocked;
                default:
                    return blocking.ToString();
            }
        }
        
        public static string ToLocalizedString(this HardwareProfile.DeviceTypes deviceTypes)
        {
            switch (deviceTypes)
            {
                case HardwareProfile.DeviceTypes.None:
                    return Properties.Resources.None;
                case HardwareProfile.DeviceTypes.OPOS:
                    return Properties.Resources.OPOS;
                case HardwareProfile.DeviceTypes.USB:
                    return Properties.Resources.Keyboard;
                default:
                    return deviceTypes.ToString();
            }
        }
        
        public static string ToLocalizedString(this HardwareProfile.LineDisplayDeviceTypes displayDeviceTypes)
        {
            switch (displayDeviceTypes)
            {
                case HardwareProfile.LineDisplayDeviceTypes.None:
                    return Properties.Resources.None;
                case HardwareProfile.LineDisplayDeviceTypes.OPOS:
                    return Properties.Resources.OPOS;
                case HardwareProfile.LineDisplayDeviceTypes.VirtualDisplay:
                    return Properties.Resources.VirtualDisplay;
                default:
                    return displayDeviceTypes.ToString();
            }
        }
        
        public static string ToLocalizedString(this HardwareProfile.PrinterHardwareTypes printerHardwareTypes)
        {
            switch (printerHardwareTypes)
            {
                case HardwareProfile.PrinterHardwareTypes.None:
                    return Properties.Resources.None;
                case HardwareProfile.PrinterHardwareTypes.OPOS:
                    return Properties.Resources.OPOS;
                case HardwareProfile.PrinterHardwareTypes.Windows:
                    return Properties.Resources.WindowsPrinter;
                case HardwareProfile.PrinterHardwareTypes.PrintingStation:
                    return Properties.Resources.PrintingStation;
                default:
                    return printerHardwareTypes.ToString();
            }
        }
        
        public static string ToLocalizedString(this ConnectionType connectionType)
        {
            switch (connectionType)
            {
                case ConnectionType.SharedMemory:
                    return Properties.Resources.SharedMemory;
                case ConnectionType.TCP_IP:
                    return Properties.Resources.TCPIP;
                case ConnectionType.NamedPipes:
                    return Properties.Resources.NamedPipes;
                default:
                    return connectionType.ToString();
            }
        }

        public static string ToEnglishString(this ConnectionType connectionType)
        {
            switch (connectionType)
            {
                case ConnectionType.SharedMemory:
                    return "Shared memory";
                case ConnectionType.TCP_IP:
                    return "TCP/IP";
                case ConnectionType.NamedPipes:
                    return "Named pipes";
                default:
                    return connectionType.ToString();
            }
        }

        public static ConnectionType ToConnectionType(this string connectionType)
        {
            switch(connectionType)
            {
                case "Named pipes":
                    return ConnectionType.NamedPipes;
                case "TCP/IP":
                    return ConnectionType.TCP_IP;
                case "Shared memory":
                    return ConnectionType.SharedMemory;
                default:
                    return ConnectionType.SharedMemory;
            }
        }

        public static string ToLocalizedString(this ConnectionAuthenticationType connectionAuthenticationType)
        {
            switch (connectionAuthenticationType)
            {
                case ConnectionAuthenticationType.WindowsAuthentication:
                    return Properties.Resources.WindowsAuthentication;
                case ConnectionAuthenticationType.SQLServerAuthentication:
                    return Properties.Resources.SQLServerAuthentication;
                default:
                    return connectionAuthenticationType.ToString();
            }
        }
    }
}
