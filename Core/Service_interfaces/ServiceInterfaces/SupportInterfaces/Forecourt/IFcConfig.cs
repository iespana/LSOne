using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportClasses.Forecourt;
using LSRetail.Forecourt;

namespace LSOne.Services.Interfaces.SupportInterfaces.Forecourt
{
    public interface IFcConfig
    {
        /// <summary>
        /// Specifies posistion of the decimal point of the unit price. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.
        /// </summary>
        int FpPriceDecPointPosition { get; set; }

        /// <summary>
        /// Specifies posistion of the decimal point of the amount value. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.
        /// </summary>
        int FpMoneyDecPointPosition { get; set; }

        /// <summary>
        /// Specifies posistion of the decimal point of the volume value. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.
        /// </summary>
        int FpVolumeDecPointPosition { get; set; }

        /// <summary>
        /// Specifies posistion of the decimal point in money totals. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5
        /// </summary>
        int FpMoneyTotalDecPointPosition { get; set; }

        /// <summary>
        /// Specifies posistion of the decimal point in volume totals. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5
        /// </summary>
        int FpVolumeTotalDecPointPosition { get; set; }

        /// <summary>
        /// Specifies posistion of the decimal point of the tank gauge data. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.
        /// </summary>
        int TgDecPointPosition { get; set; }

        /// <summary>
        /// Specifies posistion of the decimal point in tank gauge temperatures. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5
        /// </summary>
        int TgTemperatureDecPointPosition { get; set; }

        ForecourtFlag1 ForecourtFlag1 { get; set; }
        ForecourtFlag2 ForecourtFlag2 { get; set; }

        /// <summary>
        /// Stores the connection status of POS to the forecourt controller.
        /// </summary>
        bool Connected { get; set; }

        /// <summary>
        /// Operations supported by the forecourt controller.
        /// </summary>
        SupportedOperations FcSupportedOperations { get; set; }

        /// <summary>
        /// Should time be on forecourt controller be synchronized with the POS system?
        /// </summary>
        bool SynchronizeTimeOnControllerWithPOS { get; set; }

        FuellingPoint GetFuellingPoint(int fpId);
        Terminal GetTerminal(int terminalId);
        TankGauge GetTankGauge(int tankGaugeId);
        PricePole GetPricePole(int pricePoleId);
        void Add(FuellingPoint fuellingPoint);
        void Add(Terminal terminal);
        void Add(TankGauge tankGauge);
        void Add(PricePole pricePole);
        IFuellingPointTransaction GetFuellingTransaction(int fpId, int transactionId);
    }
}
