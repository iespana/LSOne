using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.SupportInterfaces.Forecourt;
using LSRetail.Forecourt;

namespace LSOne.Services.Interfaces
{
    public interface IForecourtService : IService
    {
        void Connect(IConnectionManager entry, string hostName);
        void Initialize(IConnectionManager entry);
        void EmergencyStop(IConnectionManager entry);
        void EmergencyStopRecall(IConnectionManager entry);
        void FpEmergencyStop(IConnectionManager entry, int fpId);
        void FpEmergencyStopRecall(IConnectionManager entry, int fpId);
        void FpAuthorize(IConnectionManager entry, int fpId);
        void FpAuthorizeCancel(IConnectionManager entry, int fpId);
        void FpClose(IConnectionManager entry, int fpId);
        void FpErrorClear(IConnectionManager entry, int fpId);
        void FpErrorRead(IConnectionManager entry, int fpId);
        void FpOpen(IConnectionManager entry, int fpId);
        void FpPresetAmount(IConnectionManager entry, int fpId, decimal amount);
        void FpPresetVolume(IConnectionManager entry, int fpId, decimal volume);
        void FpPrepayAmount(IConnectionManager entry, int fpId, decimal amount, int gradeId);
        decimal GetGradePrice(IConnectionManager entry, int gradeId, int priceGroupId);
        event FuellingPointCfgChangedDelegate FuellingPointCfgChanged;
        event FuellingPointDataChangedDelegate FuellingPointDataChanged;
        event FuellingPointStatusChangedDelegate FuellingPointStatusChanged;
        event FuellingPointTransactionChangedDelegate FuellingPointTransactionChanged;
        event TermianlCfgChangedDelegate TerminalCfgChanged;
        event TerminalStatusChangedDelegate TerminalStatusChanged;
        event TankGaugeCfgChangedDelegate TankGaugeCfgChanged;
        event TankGaugeStatusChangedDelegate TankGaugeStatusChanged;
        event MessageRejectedDelegate MessageRejected;
        event FcOperationModeChangedDelegate FcOperationModeChanged;
        event FcStatusChangedDelegate FcStatusChanged;
        event ConnectionLostDelegate ConnectionLost;
        event PricePoleCfgChangedDelegate PricePoleCfgChanged;
        event PricePoleStatusChangedDelegate PricePoleStatusChanged;
        void OpenTerminal(IConnectionManager entry, int terminalId);
        void CloseTerminal(IConnectionManager entry, int terminalId);
        void ClearTerminalError(IConnectionManager entry, int terminalId);
        void SetForecourtMode(IConnectionManager entry, ForecourtMode forecourtMode);
        void TransactionClear(IConnectionManager entry, IFuellingPointTransaction fpTransaction, int posId);
        IFuellingPointTransaction TransactionLock(IConnectionManager entry, IFuellingPointTransaction fpTransaction, int posId);
        void TransactionUnlock(IConnectionManager entry, IFuellingPointTransaction fpTransaction, int posId);
        DataTable GetPrices(IConnectionManager entry);
        void SetPrices(IConnectionManager entry, DataTable priceTable);
        void PrintTotals(IConnectionManager entry);
        void PricePoleOpen(IConnectionManager entry, int pricePoleId);
        void PricePoleClose(IConnectionManager entry, int pricePoleId);
        SupportedOperations GetSupportedOperations(IConnectionManager entry);
        void InitializeForecourt(IFcConfig fcConfig, string hostName, int terminalId, object forecourtControl);
    }
}
