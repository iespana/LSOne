using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    internal class KDSHelper
    {
        /// <summary>
        /// Maps the contents of the given <see cref="BusinessObjects.UserInterface.UIStyle"/> to a <see cref="KDSBusinessObjects.UIStyle"/>
        /// </summary>
        /// <param name="lsOneUIStyle">The LS One UI style</param>
        /// <returns>A populated KDSBusinessObjects.UIStyle which matches the given LS One style</returns>
        internal static KDSBusinessObjects.UIStyle MapToKDSUIStyle(BusinessObjects.UserInterface.UIStyle lsOneUIStyle)
        {
            if(lsOneUIStyle == null)
            {
                return null;
            }

            KDSBusinessObjects.UIStyle kdsStyle = new KDSBusinessObjects.UIStyle();            
            kdsStyle.ID = lsOneUIStyle.ID;
            kdsStyle.Text = lsOneUIStyle.Text;
            kdsStyle.ParentStyleID = lsOneUIStyle.ParentStyleID;
            kdsStyle.ContextID = lsOneUIStyle.ContextID;
            kdsStyle.Style = lsOneUIStyle.Style;
            kdsStyle.Deleted = lsOneUIStyle.Deleted;

            return kdsStyle;
        }
    }
}
