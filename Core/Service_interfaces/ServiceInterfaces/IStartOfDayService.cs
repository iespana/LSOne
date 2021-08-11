using System;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.Services.Interfaces
{
    public interface IStartOfDayService : IService
    {
        bool BusinessDaySet { get; set; }
        DateTime BusinessDay { get; set; }
        DateTime BusinessSystemDay { get; set; }
        bool FloatEntryRequired { get; set; }
        void RunStartOfDay();
    }
}
