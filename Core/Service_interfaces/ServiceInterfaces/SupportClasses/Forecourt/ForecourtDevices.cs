using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSRetail.Forecourt;

namespace LSOne.Services.Interfaces.SupportClasses.Forecourt
{
    public abstract class Device
    {
        private int id;     // The Id of the device

        /// <summary>
        /// The Id of the device
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }

    #region Fuelling Point
    public class FuellingData
    {
        private Decimal volume;     //The volume the fuelling point has reached while fuelling.
        private Decimal amount;     //The amount the fuelling point has reached while fuelling.

        #region Properties
        /// <summary>
        /// The volume the fuelling point has reached while fuelling.
        /// </summary>
        public Decimal Volume
        {
            get { return volume; }
            set { volume = value; }
        }

        /// <summary>
        /// The amount the fuelling point has reached while fuelling.
        /// </summary>
        public Decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        #endregion
    }



    public class FpTotals
    {
        private decimal grandVolTotal;   //The grand volume total.
        private decimal grandAmountTotal; //The grand money total.
        private TypeOfTotals typeOfTotal;//Type of totals (fuelling point totals/pump totals)

        #region Properties
        /// <summary>
        /// The grand volume total.
        /// </summary>
        public decimal GrandVolTotal
        {
            get { return grandVolTotal; }
            set { grandVolTotal = value; }
        }

        /// <summary>
        /// The grand money total.
        /// </summary>
        public decimal GrandAmountTotal
        {
            get { return grandAmountTotal; }
            set { grandAmountTotal = value; }
        }

        /// <summary>
        /// Type of totals (fuelling point totals/pump totals)
        /// </summary>
        public TypeOfTotals TypeOfTotal
        {
            get { return typeOfTotal; }
            set { typeOfTotal = value; }
        }
        #endregion
    }

    public class FpError
    {
        private int errorCode;      //The error code from the fuelling point.
        private int protocolId;     //The protocol id of the pump that experienced the error.
        private string errorText;   //A text descriping the fuel point error.
        private int errorSeqId;     //The sequence id of the alarm.

        #region Properties
        /// <summary>
        /// The error code from the fuelling point.
        /// </summary>
        public int ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
        /// <summary>
        /// The protocol id of the pump that experienced the error.
        /// </summary>
        public int ProtocolId
        {
            get { return protocolId; }
            set { protocolId = value; }
        }
        /// <summary>
        /// A text descriping the fuel point error.
        /// </summary>
        public string ErrorText
        {
            get { return errorText; }
            set { errorText = value; }
        }
        /// <summary>
        /// The sequence id of the alarm.
        /// </summary>
        public int ErrorSeqId
        {
            get { return errorSeqId; }
            set { errorSeqId = value; }
        }
        #endregion
    }

    public class FuellingPoint : Device
    {
        private FpMainStates fpMainState;   //
        private FpMainStates fpPreviousMainState;
        private FpSubStates fpSubState;     //
        private FpSubStates2 fpSubStates2;  //
        private FpError fpError;            //
        private FuellingData fuellingdata;   //The fuelling data the fuelling point has reached while fuelling.

        #region Properties
        public FpError FpError
        {
            get { return fpError; }
            set { fpError = value; }
        }
        public FpMainStates FpMainState
        {
            get { return fpMainState; }
            set
            {
                fpPreviousMainState = fpMainState;
                fpMainState = value;
            }
        }
        public FpMainStates FpPreviousMainState
        {
            get { return fpPreviousMainState; }
            set { fpPreviousMainState = value; }
        }
        public FpSubStates FpSubState
        {
            get { return fpSubState; }
            set { fpSubState = value; }
        }
        public FpSubStates2 FpSubStates2
        {
            get { return fpSubStates2; }
            set { fpSubStates2 = value; }
        }

        /// <summary>
        /// The fuelling data the fuelling point has reached while fuelling.
        /// </summary>
        public FuellingData Fuellingdata
        {
            get { return fuellingdata; }
            set { fuellingdata = value; }
        }

        #endregion

        public LinkedList<IFuellingPointTransaction> Fptransactions;

        public FuellingPoint()
        {
            fuellingdata = new FuellingData();
            Fptransactions = new LinkedList<IFuellingPointTransaction>();
            fpError = new FpError();
            fpPreviousMainState = FpMainStates.Idle;
        }

        ~FuellingPoint()
        {
            fuellingdata = null;
            Fptransactions.Clear();
            fpError = null;
        }

        public void Add(IFuellingPointTransaction newFpTransaction)
        {
            bool found = false;
            foreach (IFuellingPointTransaction fpTransaction in Fptransactions)
            {
                if (fpTransaction.TransSeqID == newFpTransaction.TransSeqID)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
            {
                this.Fptransactions.AddLast(newFpTransaction);
            }
        }

        //public IFuellingPointTransaction GetFuellingTransaction(int transSeqId)
        //{
        //    FuellingPointTransaction result = new FuellingPointTransaction();

        //    foreach (IFuellingPointTransaction fptransaction in this.Fptransactions)
        //    {
        //        if (fptransaction.TransSeqID == transSeqId)
        //        {
        //            return fptransaction;
        //        }
        //    }
        //    return result;
        //}
    }

    #endregion

    /*
    public class Gradetotals
    {
        private int gradeOptionNo;      //Grade option number
        private int gradeId;            //The id of the grade
        private int gradeVolTotal;      //The total volume for the grade
    }
    */

    public class Grade
    {
        private int id;                 //The id of the grade
        private string name;            //The name of the grade

        #region Properties
        /// <summary>
        /// The id of the grade
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// The name of the grade
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion
    }

    public class GradePrice
    {
        private int id;                 //The id of this grade   
        private decimal price;          //The unit price for the grade. 

        #region Properties
        /// <summary>
        /// The id of this grade   
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// The unit price for the grade. 
        /// </summary>
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        #endregion
    }

    public class PriceGroup
    {
        public LinkedList<GradePrice> gradePrices;

        public PriceGroup()
        {
            gradePrices = new LinkedList<GradePrice>();
        }

        ~PriceGroup()
        {
            gradePrices.Clear();
        }

        public void Add(GradePrice gradePrice)
        {
            this.gradePrices.AddLast(gradePrice);
        }
    }


    public class Terminal : Device
    {
        TerminalMainStates terminalMainState;
        TerminalSubStates terminalSubState;
        TerminalErrorStates terminalErrorState;

        public TerminalMainStates TerminalMainState
        {
            get { return terminalMainState; }
            set { terminalMainState = value; }
        }

        public TerminalSubStates TerminalSubState
        {
            get { return terminalSubState; }
            set { terminalSubState = value; }
        }

        public TerminalErrorStates TerminalErrorState
        {
            get { return terminalErrorState; }
            set { terminalErrorState = value; }
        }

    }

    public class TankGauge : Device
    {
        private decimal grossObservedVolume;
        private decimal grossSTDVolume;
        private decimal waterVolume;
        private decimal capacity;
        private decimal maximumSafeFillCapacity;
        private decimal temperature;
        private decimal waterLevel;
        private decimal fuelLevel;
        private DateTime dataLastUpdated;
        private TankGaugeMainStates mainState;
        private TankGaugeSubStates subState;
        private TankGaugeAlarmStatus alarmStatus;
        #region Properties
        public decimal GrossObservedVolume
        {
            get { return grossObservedVolume; }
            set
            {
                grossObservedVolume = value;
                UpdateLevel();
            }
        }

        public decimal GrossSTDVolume
        {
            get { return grossSTDVolume; }
            set { grossSTDVolume = value; }
        }

        public decimal WaterVolume
        {
            get { return waterVolume; }
            set
            {
                waterVolume = value;
                UpdateLevel();
            }
        }

        public decimal Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        public decimal MaximumSafeFillCapacity
        {
            get { return maximumSafeFillCapacity; }
            set { maximumSafeFillCapacity = value; }
        }

        public decimal Temperature
        {
            get { return temperature; }
            set { temperature = value; }
        }

        public decimal WaterLevel
        {
            get { return waterLevel; }
        }

        public decimal FuelLevel
        {
            get { return fuelLevel; }
        }

        public DateTime DataLastUpdated
        {
            get { return dataLastUpdated; }
            set { dataLastUpdated = value; }
        }

        public TankGaugeMainStates MainState
        {
            get { return mainState; }
            set { mainState = value; }
        }

        public TankGaugeSubStates SubState
        {
            get { return subState; }
            set { subState = value; }
        }

        public TankGaugeAlarmStatus AlarmStatus
        {
            get { return alarmStatus; }
            set { alarmStatus = value; }
        }
        #endregion

        private void UpdateLevel()
        {
            fuelLevel = Math.Round((grossObservedVolume - waterVolume) / capacity * 100, 2);
            waterLevel = Math.Round(waterVolume / capacity * 100, 2);
        }
    }

    public class PricePole : Device
    {
        private PricePoleMainStates mainState;
        private PricePoleSubStates subState;
        private PricePoleErrorStates errorState;

        #region Properties
        public PricePoleMainStates MainState
        {
            get { return mainState; }
            set { mainState = value; }
        }

        public PricePoleSubStates SubState
        {
            get { return subState; }
            set { subState = value; }
        }


        public PricePoleErrorStates ErrorState
        {
            get { return errorState; }
            set { errorState = value; }
        }
        #endregion
    }
}
