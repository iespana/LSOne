using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportClasses.Forecourt;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSRetail.Forecourt;

namespace LSOne.Services.Interfaces.Delegates
{
    public delegate void FuellingPointCfgChangedDelegate(FuellingPoint fuellingPoint);
    public delegate void FuellingPointStatusChangedDelegate(FuellingPoint fuellingPoint);
    public delegate void FuellingPointDataChangedDelegate(FuellingPoint fuellingPoint);
    public delegate void FuellingPointTransactionChangedDelegate(IFuellingPointTransaction fpTransaction);
    public delegate void TermianlCfgChangedDelegate(Terminal terminal);
    public delegate void TerminalStatusChangedDelegate(Terminal terminal);
    public delegate void TankGaugeCfgChangedDelegate(TankGauge tankGauge);
    public delegate void TankGaugeStatusChangedDelegate(TankGauge tankGauge);
    public delegate void MessageRejectedDelegate(MessageRejectedEventArgs messageRejectedEventArgs);
    public delegate void FcOperationModeChangedDelegate(ForecourtMode forecourtMode);
    public delegate void FcStatusChangedDelegate();
    public delegate void ConnectionLostDelegate();
    public delegate void PricePoleCfgChangedDelegate(PricePole pricePole);
    public delegate void PricePoleStatusChangedDelegate(PricePole pricePole);
}
