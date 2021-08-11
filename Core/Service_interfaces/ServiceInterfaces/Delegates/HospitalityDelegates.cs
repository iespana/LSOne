using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.Services.Interfaces.Delegates
{
    public delegate void SetMainViewIndexDelegate(int index);
    public delegate bool RunOperationDelegate(POSOperations operationID, Object extraInfo, OperationInfo opInfo);
    public delegate void SetTransactionDelegate(IPosTransaction transaction);
    public delegate void SetInputAbilityDelegate(bool abilityState);
    public delegate void LoadPosDesignDelegate();
    public delegate void UpdateTableStatus();
    public delegate void VisibleChangedDelegate(IConnectionManager entry);
    public delegate void LogOffUserDelegate();
}
