using System;

namespace LSOne.DataLayer.BusinessObjects
{
    public class DecimalSetting : DataEntity
    {
        public DecimalSetting()
            : base()
        {

        }

        /// <summary>
        /// Constructor that takes a string of decimal places. The string must be at least 3 characters and contain the letter : i.e. 2:3 or 0:2
        /// </summary>
        /// <param name="decimalPlaces">The decimal places as a string (0:2, 2:2 and etc)</param>
        public DecimalSetting(string decimalPlaces)
        {
            // The minimum length of this is x:x = 3 letters, and it must contain the letter ':'
            if (decimalPlaces.Length >= 3 && decimalPlaces.Contains(":"))
            {
                string[] minAndMax = decimalPlaces.Split(':');
                if (minAndMax.Length >= 2)
                {
                    Min = Convert.ToInt32(minAndMax[0]);
                    Max = Convert.ToInt32(minAndMax[1]);
                }
            }
        }      

        public int Min
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }
    }
}
