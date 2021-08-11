using LSOne.Services.Interfaces.Delegates;
using System.Windows.Forms;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IAuthorizeOperationPage
    {
        event LogonFormEventHandler OperationPerformed;

        UserControl Panel
        {
            get;
        }

        void DisableBarcodeScanner();
    }
}
