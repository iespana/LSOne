using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    public class WeekdaySequence
    {
        private enum EnumValue
        {
            First,
            Second,
            Third,
            Fourth,
            Last
        }

        private EnumValue enumValue;

        private WeekdaySequence(EnumValue enumValue)
        {
            this.enumValue = enumValue;
        }


        public override string ToString()
        {
            string resourceName = "Recurrence_WeekdaySequence_EnumValue_" + Enum.GetName(typeof(EnumValue), enumValue);
            return Properties.Resources.ResourceManager.GetString(resourceName);
        }



        private static EnumValue[] enumValues;
        private static WeekdaySequence[] values;

        static WeekdaySequence()
        {
           enumValues = (EnumValue[])Enum.GetValues(typeof(EnumValue));
            values = new WeekdaySequence[enumValues.Length];
            for (int i = 0; i < enumValues.Length; i++)
            {
                values[i] = new WeekdaySequence((EnumValue)enumValues[i]);
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

        public static WeekdaySequence First
        {
            get { return values[IndexOfEnumValue(EnumValue.First)]; }
        }

        public static WeekdaySequence Second
        {
            get { return values[IndexOfEnumValue(EnumValue.Second)]; }
        }

        public static WeekdaySequence Third
        {
            get { return values[IndexOfEnumValue(EnumValue.Third)]; }
        }

        public static WeekdaySequence Fourth
        {
            get { return values[IndexOfEnumValue(EnumValue.Fourth)]; }
        }

        public static WeekdaySequence Last
        {
            get { return values[IndexOfEnumValue(EnumValue.Last)]; }
        }

        public static IEnumerable<WeekdaySequence> AllValues
        {
            get { return values; }
        }


        public static WeekdaySequence FromDay(int day)
        {
            // 1-7 = first      (1-1)/7 = 0; (7-1)/7 = 0
            // 8-14 = second    (8-1)/7 = 1; (14-1)/7 = 1
            // 15-21 = third    ...
            // 22-28 = fourth
            // 29-31 = last

            int week = (day - 1) / 7;
            switch (week)
            {
                case 0:
                    return First;
                case 1:
                    return Second;
                case 2:
                    return Third;
                default:
                    return Fourth;
            }

        }
    }
}
