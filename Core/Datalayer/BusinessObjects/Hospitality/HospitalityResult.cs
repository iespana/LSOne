using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class HospitalityResult
    {
        public HospitalityOperationResult OperationResult { get; set; }
        public int Guest { get; set; }

        public HospitalityResult()
        {
            OperationResult = HospitalityOperationResult.Cancel;
            Guest = 0;
        }
    }
}
