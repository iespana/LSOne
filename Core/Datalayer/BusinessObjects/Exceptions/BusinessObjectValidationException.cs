using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#if !MONO

#else
using LSOne.Utilities.Validation;
#endif


namespace LSOne.DataLayer.BusinessObjects.Exceptions
{
    public class BusinessObjectValidationException  : Exception
    {
        List<ValidationResult> validationResults;

        public BusinessObjectValidationException(List<ValidationResult> validationResults)
        {
            this.validationResults = validationResults;
        }

        public override string Message
        {
            get
            {
                string message = "";

                foreach (ValidationResult result in validationResults)
                {
                    message += result.ErrorMessage + "\n";
                }

                return message;
            }
        }

        protected BusinessObjectValidationException( System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context ) 
            : base( info, context ) 
        { }    
    
    
    }   
}
