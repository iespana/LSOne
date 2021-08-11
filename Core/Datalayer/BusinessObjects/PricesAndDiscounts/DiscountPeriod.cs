using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    /// <summary>
    /// Data provider class for a discount period
    /// </summary>
    public class DiscountPeriod : DataEntity
    {
        public DiscountPeriod()
            : base()
        {
            Date start = Date.Now;
            StartingDate = start;
            EndingDate = start;
            TimeWithinBounds = true;
            EndingTimeAfterMidnight = false;

            TimeSpan startingTime = new TimeSpan(0, 0, 0);
            TimeSpan endingTime = new TimeSpan(1, 0, 0, 0);

            StartingTime = startingTime;
            EndingTime = endingTime;

            TimeSpan weekdayStartingTime = new TimeSpan(0, 0, 0);
            TimeSpan weekdayEndingTime = new TimeSpan(0, 0, 0);

            MonStartingTime = weekdayStartingTime;
            MonEndingTime = weekdayEndingTime;
            MonTimeWithinBounds = true;
            MonEndingTimeAfterMidnight = false;

            TueStartingTime = weekdayStartingTime;
            TueEndingTime = weekdayEndingTime;
            TueTimeWithinBounds = true;
            TueEndingTimeAfterMidnight = false;

            WedStartingTime = weekdayStartingTime;
            WedEndingTime = weekdayEndingTime;
            WedTimeWithinBounds = true;
            WedEndingTimeAfterMidnight = false;

            ThuStartingTime = weekdayStartingTime;
            ThuEndingTime = weekdayEndingTime;
            ThuTimeWithinBounds = true;
            ThuEndingTimeAfterMidnight = false;

            FriStartingTime = weekdayStartingTime;
            FriEndingTime = weekdayEndingTime;
            FriTimeWithinBounds = true;
            FriEndingTimeAfterMidnight = false;

            SatStartingTime = weekdayStartingTime;
            SatEndingTime = weekdayEndingTime;
            SatTimeWithinBounds = true;
            SatEndingTimeAfterMidnight = false;

            SunStartingTime = weekdayStartingTime;
            SunEndingTime = weekdayEndingTime;
            SunTimeWithinBounds = true;
            SunEndingTimeAfterMidnight = false;

            

        }

        // Starting and Ending times only contain time elements 

        
        /// <summary>
        /// 
        /// </summary>
        public Date StartingDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Date EndingDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan StartingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan EndingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool TimeWithinBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool EndingTimeAfterMidnight { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public TimeSpan MonStartingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan MonEndingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool MonTimeWithinBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool MonEndingTimeAfterMidnight { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public TimeSpan TueStartingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan TueEndingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool TueTimeWithinBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool TueEndingTimeAfterMidnight { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public TimeSpan WedStartingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan WedEndingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool WedTimeWithinBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool WedEndingTimeAfterMidnight { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public TimeSpan ThuStartingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan ThuEndingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ThuTimeWithinBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ThuEndingTimeAfterMidnight { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public TimeSpan FriStartingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan FriEndingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool FriTimeWithinBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool FriEndingTimeAfterMidnight { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public TimeSpan SatStartingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan SatEndingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool SatTimeWithinBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool SatEndingTimeAfterMidnight { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public TimeSpan SunStartingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan SunEndingTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool SunTimeWithinBounds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool SunEndingTimeAfterMidnight { get; set; }
    }
}
