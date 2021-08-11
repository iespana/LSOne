using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls.Recurrence
{
    internal static class CronHelper
    {
        public static bool IsAny(string s)
        {
            return s == "*";
        }

        public static bool IsAnyStep(string s)
        {
            return s.IndexOf('/') > s.IndexOf('*');
        }

        public static bool IsFixedNumber(string s)
        {
            int value;
            return int.TryParse(s, out value);
            //return s.Length > 0 && char.IsDigit(s[0]);
        }


        public static int ParseToInt32(string s)
        {
            if (s == "*")
            {
                return 1;
            }
            else
            {
                int index = s.IndexOf("/");
                if (index >= 0)
                {
                    return int.Parse(s.Substring(index+1));
                }
                else
                {
                    return int.Parse(s);
                }
            }
        }
    }
}
