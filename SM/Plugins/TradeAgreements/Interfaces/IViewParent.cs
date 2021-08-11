using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore;

namespace LSOne.ViewPlugins.TradeAgreements.Interfaces
{
    internal interface IViewParent
    {
        ViewBase ParentView
        {
            get;
        }

        PluginEntry.Mode Mode
        {
            get;
        }

        string GroupText
        {
            get;
        }
    }
}
