using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public enum DisplayErrorHintEnum
    {
        None,
        Error,
        Stop
    }

    public enum EngineErrorResultCode
    {
        NA,
        CanceledByPreTrigger,
        OperationInvalidForThisTypeOfTransaction,
        
        
    }

    public interface IEngineResult
    {
        

        bool Value
        {
            get; set;
        }

        string ErrorText
        {
            get; set;
        }

        DisplayErrorHintEnum DisplayErrorHint
        {
            get; set;
        }

        EngineErrorResultCode ErrorResultCode
        {
            get; set;
        }
}
}
