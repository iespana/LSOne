using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class HospitalityOperation : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return OperationID;
            }
            set
            {
                OperationID = value;
            }
        }


        public HospitalityOperation()           
        {
            OperationID = 0;
            Text = "";
            PermissionID = 0;
            CheckUserAccess = false;
            AllowParameter = false;
            NavOperation = "";
            ParameterType = ParameterTypeEnum.None;
        }

        public RecordIdentifier OperationID { get; set; }        
        public int PermissionID { get; set; }
        public bool CheckUserAccess { get; set; }
        public bool AllowParameter { get; set; }        
        public string NavOperation { get; set; }
        public ParameterTypeEnum ParameterType { get; set; }

        #region enums
        public enum ParameterTypeEnum
        {
            None = 0,
            SubMenu = 1
        }
        #endregion
    }
}
