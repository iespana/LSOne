using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportClasses.Forecourt;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Interfaces.SupportInterfaces.Forecourt;
using LSRetail.Forecourt;
using Grade = LSOne.Services.Interfaces.SupportClasses.Forecourt.Grade;

namespace LSOne.DataLayer.TransactionObjects.Forecourt
{    
    public class FcConfig : IFcConfig
    {
        private int fpPriceDecPointPosition;
        //Specifies posistion of the decimal point of the unit price. A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.

        private int fpMoneyDecPointPosition;
        //Specifies posistion of the decimal point of the amount value. A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.

        private int fpVolumeDecPointPosition;
        //Specifies posistion of the decimal point of the volume value. A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.

        private int fpMoneyTotalDecPointPosition;
        //Specifies posistion of the decimal point in money totals. A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5

        private int fpVolumeTotalDecPointPosition;
        //Specifies posistion of the decimal point in volume totals. A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5

        private int tgDecPointPosition;
        //Specifies posistion of the decimal point of the tank gauge data. A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.

        private int tgTemperatureDecPointPosition;
        //Specifies posistion of the decimal point in tank gauge temperatures. A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5

        private ForecourtFlag1 forecourtFlag1;
        private ForecourtFlag2 forecourtFlag2;
        private bool connected; //Stores the connection status of POS to the forecourt controller.
        private SupportedOperations fcSupportedOperations; //Operations supported by the forecourt controller.
        private bool synchronizeTimeOnControllerWithPOS = false; //Should time be on forecourt controller be synchronized with the POS system?

        #region Properties

        /// <summary>
        /// Specifies posistion of the decimal point of the unit price. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.
        /// </summary>
        public int FpPriceDecPointPosition
        {
            get { return fpPriceDecPointPosition; }
            set { fpPriceDecPointPosition = value; }
        }

        /// <summary>
        /// Specifies posistion of the decimal point of the amount value. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.
        /// </summary>
        public int FpMoneyDecPointPosition
        {
            get { return fpMoneyDecPointPosition; }
            set { fpMoneyDecPointPosition = value; }
        }

        /// <summary>
        /// Specifies posistion of the decimal point of the volume value. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.
        /// </summary>
        public int FpVolumeDecPointPosition
        {
            get { return fpVolumeDecPointPosition; }
            set { fpVolumeDecPointPosition = value; }
        }

        /// <summary>
        /// Specifies posistion of the decimal point in money totals. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5
        /// </summary>
        public int FpMoneyTotalDecPointPosition
        {
            get { return fpMoneyTotalDecPointPosition; }
            set { fpMoneyTotalDecPointPosition = value; }
        }

        /// <summary>
        /// Specifies posistion of the decimal point in volume totals. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5
        /// </summary>
        public int FpVolumeTotalDecPointPosition
        {
            get { return fpVolumeTotalDecPointPosition; }
            set { fpVolumeTotalDecPointPosition = value; }
        }

        /// <summary>
        /// Specifies posistion of the decimal point of the tank gauge data. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5.
        /// </summary>
        public int TgDecPointPosition
        {
            get { return tgDecPointPosition; }
            set { tgDecPointPosition = value; }
        }

        /// <summary>
        /// Specifies posistion of the decimal point in tank gauge temperatures. 
        /// A value of zero indicates the rightmost position, equalling no decimals. Valid range 0-5
        /// </summary>
        public int TgTemperatureDecPointPosition
        {
            get { return tgTemperatureDecPointPosition; }
            set { tgTemperatureDecPointPosition = value; }
        }

        public ForecourtFlag1 ForecourtFlag1
        {
            get { return forecourtFlag1; }
            set { forecourtFlag1 = value; }
        }

        public ForecourtFlag2 ForecourtFlag2
        {
            get { return forecourtFlag2; }
            set { forecourtFlag2 = value; }
        }

        /// <summary>
        /// Stores the connection status of POS to the forecourt controller.
        /// </summary>
        public bool Connected
        {
            get { return connected; }
            set { connected = value; }
        }

        /// <summary>
        /// Operations supported by the forecourt controller.
        /// </summary>
        public SupportedOperations FcSupportedOperations
        {
            get { return fcSupportedOperations; }
            set { fcSupportedOperations = value; }
        }

        /// <summary>
        /// Should time be on forecourt controller be synchronized with the POS system?
        /// </summary>
        public bool SynchronizeTimeOnControllerWithPOS
        {
            get { return synchronizeTimeOnControllerWithPOS; }
            set { synchronizeTimeOnControllerWithPOS = value; }
        }

        #endregion

        //
        //Devices
        public LinkedList<FuellingPoint> FuellingPoints;
        public LinkedList<Terminal> Terminals;
        public LinkedList<TankGauge> TankGauges;
        public LinkedList<PricePole> PricePoles;
        //Grades
        public LinkedList<Grade> Grades;
        //Operations
        //public LinkedList<SupportedOperation> SupportedOperations;


        public FcConfig()
        {
            FuellingPoints = new LinkedList<FuellingPoint>();
            Terminals = new LinkedList<Terminal>();
            TankGauges = new LinkedList<TankGauge>();
            PricePoles = new LinkedList<PricePole>();
            Grades = new LinkedList<Grade>();
        }

        ~FcConfig()
        {
            FuellingPoints.Clear();
            Terminals.Clear();
            TankGauges.Clear();
            PricePoles.Clear();
            Grades.Clear();
        }

        public FuellingPoint GetFuellingPoint(int fpId)
        {
            foreach (FuellingPoint fuellingPoint in this.FuellingPoints)
            {
                if (fuellingPoint.Id == fpId)
                {
                    return fuellingPoint;
                }
            }
            FuellingPoint result = new FuellingPoint();
            return result;
        }

        public Terminal GetTerminal(int terminalId)
        {
            foreach (Terminal terminal in this.Terminals)
            {
                if (terminal.Id == terminalId)
                {
                    return terminal;
                }
            }
            Terminal result = new Terminal();
            return result;
        }

        public TankGauge GetTankGauge(int tankGaugeId)
        {
            foreach (TankGauge tankGauge in this.TankGauges)
            {
                if (tankGauge.Id == tankGaugeId)
                {
                    return tankGauge;
                }
            }
            TankGauge result = new TankGauge();
            return result;
        }

        public PricePole GetPricePole(int pricePoleId)
        {
            foreach (PricePole pricePole in this.PricePoles)
            {
                if (pricePole.Id == pricePoleId)
                {
                    return pricePole;
                }
            }
            PricePole result = new PricePole();
            return result;
        }

        public void Add(FuellingPoint fuellingPoint)
        {
            bool found = false;
            foreach (FuellingPoint fp in this.FuellingPoints)
            {
                if (fuellingPoint.Id == fp.Id)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                FuellingPoints.AddLast(fuellingPoint);
            }
        }

        public void Add(Terminal terminal)
        {
            bool found = false;
            foreach (Terminal tm in this.Terminals)
            {
                if (tm.Id == terminal.Id)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                Terminals.AddLast(terminal);
            }
        }

        public void Add(TankGauge tankGauge)
        {
            bool found = false;
            foreach (TankGauge tg in this.TankGauges)
            {
                if (tg.Id == tankGauge.Id)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                TankGauges.AddLast(tankGauge);
            }
        }

        public void Add(PricePole pricePole)
        {
            bool found = false;
            foreach (PricePole pp in this.PricePoles)
            {
                if (pp.Id == pricePole.Id)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                PricePoles.AddLast(pricePole);
            }
        }

        public IFuellingPointTransaction GetFuellingTransaction(int fpId, int transactionId)
        {
            FuellingPointTransaction result = new FuellingPointTransaction();

            FuellingPoint fuellingPoint = GetFuellingPoint(fpId);
            foreach (IFuellingPointTransaction fptransaction in fuellingPoint.Fptransactions)
            {

                if (fptransaction.ID == transactionId)
                {
                    return fptransaction;
                }
            }
            return result;
        }
    }
}
