using System;
using System.Collections.Generic;
using System.Threading;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public class WeekdayCharm
    {
        /// <summary>
        /// The enum values for the weekday charm.
        /// </summary>
        private enum EnumValue
        {
            // NOTE: If this enum type is changed, the corresponding
            // resource strings must be changed accordingly (see ToString() below)
            Day,
            Sunday,
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
        }

        private EnumValue enumValue;

        private WeekdayCharm(EnumValue enumValue)
        {
            this.enumValue = enumValue;
        }


        public override string ToString()
        {
            if (enumValue == EnumValue.Day)
            {
                return Properties.Resources.Recurrence_WeekdayCharm_EnumValue_Day;
            }
            return Thread.CurrentThread.CurrentUICulture.DateTimeFormat.GetDayName(EnumValueToDayOfWeek(enumValue));
        }

        public bool IsDayOfWeek()
        {
            return (int)enumValue >= (int)EnumValue.Sunday;
        }

        public DayOfWeek ToDayOfWeek()
        {
            return EnumValueToDayOfWeek(this.enumValue);
        }

        public static WeekdayCharm FromDayOfWeek(DayOfWeek dayOfWeek)
        {
            EnumValue enumValue = EnumValueFromDayOfWeek(dayOfWeek);
            return values[IndexOfEnumValue(enumValue)];
        }

        private static DayOfWeek EnumValueToDayOfWeek(EnumValue enumValue)
        {
            return (DayOfWeek)((int)enumValue - (int)EnumValue.Sunday);
        }

        private static EnumValue EnumValueFromDayOfWeek(DayOfWeek dayOfWeek)
        {
            return (EnumValue)((int)dayOfWeek + (int)EnumValue.Sunday);
        }

        private static EnumValue[] enumValues;
        private static WeekdayCharm[] values;

        static WeekdayCharm()
        {
           enumValues = (EnumValue[])Enum.GetValues(typeof(EnumValue));
           values = new WeekdayCharm[enumValues.Length];
            for (int i = 0; i < enumValues.Length; i++)
            {
                values[i] = new WeekdayCharm((EnumValue)enumValues[i]);
            }
        }

        private static int IndexOfEnumValue(EnumValue enumValue)
        {
            for (int i = 0; i < enumValues.Length; i++)
            {
                if (enumValues[i] == enumValue)
                    return i;
            }

            throw new ArgumentOutOfRangeException();
        }

        public static IEnumerable<WeekdayCharm> AllValues
        {
            get { return values; }
        }

        public static WeekdayCharm Day
        {
            get { return values[IndexOfEnumValue(EnumValue.Day)]; }
        }

        public static WeekdayCharm Sunday
        {
            get { return values[IndexOfEnumValue(EnumValue.Sunday)]; }
        }

        public static WeekdayCharm Monday
        {
            get { return values[IndexOfEnumValue(EnumValue.Monday)]; }
        }

        public static WeekdayCharm Tuesday
        {
            get { return values[IndexOfEnumValue(EnumValue.Tuesday)]; }
        }

        public static WeekdayCharm Wednesday
        {
            get { return values[IndexOfEnumValue(EnumValue.Wednesday)]; }
        }

        public static WeekdayCharm Thursday
        {
            get { return values[IndexOfEnumValue(EnumValue.Thursday)]; }
        }

        public static WeekdayCharm Friday
        {
            get { return values[IndexOfEnumValue(EnumValue.Friday)]; }
        }

        public static WeekdayCharm Saturday
        {
            get { return values[IndexOfEnumValue(EnumValue.Saturday)]; }
        }
    }
}
