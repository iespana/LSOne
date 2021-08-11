using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.LoginPanel
{
    internal class DLLEntry
    {
        internal static IConnectionManager DataModel;
        internal static ISettings Settings;
        internal static string StoreLanguage;
        internal static string StoreKeyboardCode;
        internal static string StoreKeyboardLayoutName;
    }
}
