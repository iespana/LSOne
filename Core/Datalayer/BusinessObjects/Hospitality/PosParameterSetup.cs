using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Hospitality
{
    public class PosParameterSetup : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(OperationID, ParameterCode);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public PosParameterSetup()
            : base()
        {
            OperationID = 0;
            ParameterCode = "";
            Text = "";
        }

        public RecordIdentifier OperationID { get; set; }
        public RecordIdentifier ParameterCode { get; set; }        

        public override string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return OperationID.ToString();

                    case 1:
                        return (string)ParameterCode;

                    case 2:
                        return Text;

                    default:
                        return base[index];
                }


            }
        }
    }
}
