using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// An enum with the screen numbers that are available to display the POS on.
    /// This value can be set in the Visual Profile in the Site Manager
    /// </summary>
    public enum ScreenNumberEnum
    {
        /// <summary>
        /// The POS will be displayed on which ever screen is configured to be the main screen in Windows
        /// </summary>
        MainScreen = 0,
        /// <summary>
        /// The POS will be displayed on which ever screen is configured to be Screen 1
        /// </summary>
        Screen1 = 1,
        /// <summary>
        /// The POS will be displayed on which ever screen is configured to be Screen 2
        /// </summary>
        Screen2 = 2,
        /// <summary>
        /// The POS will be displayed on which ever screen is configured to be Screen 3
        /// </summary>
        Screen3 = 3
    }

    /// <summary>
    /// Helper class for operations on <see cref="ItemTypeEnum">item type</see> enumeration.
    /// </summary>
    public static class ScreenNumberHelper
    {
        /// <summary>
        /// Gets the <see cref="ScreenNumberEnum">screen number</see> localized description.
        /// </summary>
        public static string ScreenNumberToString(ScreenNumberEnum screen)
        {
            switch (screen)
            {
                case ScreenNumberEnum.MainScreen:
                    return Properties.Resources.MainScreen;
                case ScreenNumberEnum.Screen1:
                    return Properties.Resources.Screen1;
                case ScreenNumberEnum.Screen2:
                    return Properties.Resources.Screen2;
                case ScreenNumberEnum.Screen3:
                    return Properties.Resources.Screen3;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Gets the <see cref="ScreenNumberEnum">screen number</see> from the localized description.
        /// </summary>
        public static ScreenNumberEnum StringToScreenNumber(string value)
        {
            if (value == Properties.Resources.MainScreen)
                return ScreenNumberEnum.MainScreen;
            if (value == Properties.Resources.Screen1)
                return ScreenNumberEnum.Screen1;
            if (value == Properties.Resources.Screen2)
                return ScreenNumberEnum.Screen2;
            if (value == Properties.Resources.Screen3)
                return ScreenNumberEnum.Screen3;

            return ScreenNumberEnum.MainScreen;
        }
    }
}
