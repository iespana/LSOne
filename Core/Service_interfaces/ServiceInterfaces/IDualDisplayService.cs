using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    public interface IDualDisplayService : IService
    {
        void ShowTransaction(IPosTransaction transaciton);
        void Show();
        void Close();
        void UpdateTotalAmountsLayout(string layoutXML);
    }
}
