using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    /// <summary>
    /// Specifies how a reason code is selected when returning items. Used when setting up the operation in the pos and Site Manager
    /// </summary>
    public enum ReasonCodeSelectionTypeEnum
    {
        Default = 0,
        List = 1,
        Specific = 1
    }
}
