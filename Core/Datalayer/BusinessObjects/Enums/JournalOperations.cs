using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    [Flags]
    public enum JournalOperations
    {
        Select = 1,
        Return = 2,
        Receipt = 4,
        Invoice = 8,
        TaxFree = 16,
        NotDownToTerminal = 32
    }
}
