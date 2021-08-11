using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    public interface ILoginPanelService : IService
    {
        ILoginPanelPage CreateLoginPage(SQLServerLoginEntry loginEntry, ISettings settings, bool tokenLogin);
        bool PermissionOverrideDialog(PermissionInfo info, ref RecordIdentifier overriderID);
        bool PermissionOverrideDialog(PermissionInfo info);

        IAuthorizeOperationPage CreateAuthorizeOperationPage(SQLServerLoginEntry loginEntry, POSOperations posOperation);

        bool LockTerminal();
        bool SwitchUser();
    }
}
