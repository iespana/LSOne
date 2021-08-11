using System.Windows.Forms;
using LSOne.Services.Interfaces.Delegates;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ILoginPanelPage
    {
        event LogonFormEventHandler OperationPerformed;

        UserControl Panel
        {
            get;
        }

        string Login
        {
            get;
        }

        string Password
        {
            get;
        }

        void ClearUser();

        bool TokenLogin
        {
            get; 
        }

        void EnableBarcodeScanner();
        void DisableBarcodeScanner();
    }
}
