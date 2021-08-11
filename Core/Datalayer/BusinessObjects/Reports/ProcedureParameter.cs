using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.IO.JSON;

namespace LSOne.DataLayer.BusinessObjects.Reports
{
    [DataContract]
    public class ProcedureParameter : DataEntity
    {
        public ProcedureParameter()
            : base()
        {
            StockParameter = false;
        }

        public ProcedureParameter(string name)
            : base()
        {
            Name = name;
        }

        public string Name
        {
            get
            {
                return (string)base.ID;
            }
            set
            {
                ID = value;
            }
        }

        public override string Text
        {
            get
            {
                return (string)ID;
            }
            set
            {
                ID = value;
            }
        }
        [DataMember]
        public SqlDbType DataType { get; set; }
        [DataMember]
        public bool IsStringArray { get; set; }
        public int MaxLength { get; set; }
        public bool IsInput { get; set; }
        public bool IsResult { get; set; }
        public object Value { get; set; }
        public bool StockParameter { get; set; }

        [DataMember]
        public string StringValue {  get; set; }

        public object ParseStringValue
        {
            get
            {
                try
                {
                    if (IsStringArray)
                    {
                        return JsonConvert.DeserializeObject<string[]>(StringValue);
                    }                    
                    switch (DataType)
                    {
                        case SqlDbType.BigInt:
                            return long.Parse(StringValue);
                        case SqlDbType.Binary:
                            throw new NotImplementedException();
                        case SqlDbType.Bit:
                            return bool.Parse(StringValue);
                        case SqlDbType.Char:
                            return StringValue[0];
                        case SqlDbType.DateTime:
                            return DateTime.Parse(StringValue);
                        case SqlDbType.Decimal:
                            return decimal.Parse(StringValue);
                        case SqlDbType.Float:
                            return float.Parse(StringValue);
                        case SqlDbType.Image:
                            throw new NotImplementedException();
                        case SqlDbType.Int:
                            return int.Parse(StringValue);
                        case SqlDbType.Money:
                            throw new NotImplementedException();
                        case SqlDbType.NChar:
                            return StringValue[0];
                        case SqlDbType.NText:
                            return StringValue;
                        case SqlDbType.NVarChar:
                            return StringValue;
                        case SqlDbType.Real:
                            return float.Parse(StringValue);
                        case SqlDbType.UniqueIdentifier:
                            return Guid.Parse(StringValue);
                        case SqlDbType.SmallDateTime:
                            return DateTime.Parse(StringValue);
                        case SqlDbType.SmallInt:
                            return int.Parse(StringValue);
                        case SqlDbType.SmallMoney:
                            throw new NotImplementedException();
                        case SqlDbType.Text:
                            return StringValue;
                        case SqlDbType.Timestamp:
                            throw new NotImplementedException();
                        case SqlDbType.TinyInt:
                            return int.Parse(StringValue);
                        case SqlDbType.VarBinary:
                            throw new NotImplementedException();
                        case SqlDbType.VarChar:
                            return StringValue;
                        case SqlDbType.Variant:
                            throw new NotImplementedException();
                        case SqlDbType.Xml:
                            throw new NotImplementedException();
                        case SqlDbType.Udt:
                            throw new NotImplementedException();
                        case SqlDbType.Structured:
                            throw new NotImplementedException();
                        case SqlDbType.Date:
                            return DateTime.Parse(StringValue);
                        case SqlDbType.Time:
                            return DateTime.Parse(StringValue);
                        case SqlDbType.DateTime2:
                            return DateTime.Parse(StringValue);
                        case SqlDbType.DateTimeOffset:
                            throw new NotImplementedException();
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public string ParameterFormatedValue
        {
            get
            {
                switch (DataType)
                {
                    case SqlDbType.DateTime:
                        return ((DateTime) Value).ToShortDateString();

                    case SqlDbType.NVarChar:
                        return (string) Value;

                    case SqlDbType.Int:
                        return Value.ToString();

                    default:
                        return "";
                }
            }
        }
    }
}
