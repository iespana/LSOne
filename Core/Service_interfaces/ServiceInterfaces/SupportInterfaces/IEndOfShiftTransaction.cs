using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IEndOfShiftTransaction : IPosTransaction
    {
        bool Cancelled { get; set; }
    }
}
