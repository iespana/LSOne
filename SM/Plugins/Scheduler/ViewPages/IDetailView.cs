using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Scheduler.ViewPages
{
    public interface IDetailView : ITabView
    {
        void Show();
        void Hide();
    }
}
