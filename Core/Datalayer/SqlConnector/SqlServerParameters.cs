using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlConnector
{
    public class SqlServerParameters
    {
        [Conditional("DEBUG")]
        private static void ParamTest(IDbCommand cmd, string name)
        {
            if(cmd.CommandType == CommandType.Text && cmd.CommandText != "")
            {
                if(!cmd.CommandText.Contains("@"+name))
                {
                    if(!cmd.CommandText.Contains("@"))
                    {
                        return;
                    }

                    var msg = string.Format("Failed parameter casing test on parameter: {0}\r\n\r\nStack trace:\r\n{1}",
                                            name, new StackTrace().ToString());

                    MessageBox.Show(msg, "FIX IT!!!");
                }
            }
        }

        public static SqlParameter MakeParam(IDbCommand cmd, string name, object value, SqlDbType type)
        {
            var prm = new SqlParameter("@" + name, type) {Value = value};

            cmd.Parameters.Add(prm);

            ParamTest(cmd, name);

            return prm;
        }

        public static SqlParameter MakeParamNoCheck(IDbCommand cmd, string name, object value, SqlDbType type)
        {
            var prm = new SqlParameter("@" + name, type) { Value = value };

            cmd.Parameters.Add(prm);

            return prm;
        }

        public static SqlParameter MakeParam(IDbCommand cmd, string name, object value, SqlDbType type, ParameterDirection direction)
        {
            var prm = new SqlParameter("@" + name, type) {Value = value, Direction = direction};

            cmd.Parameters.Add(prm);

            ParamTest(cmd, name);

            return prm;
        }

        public static SqlParameter MakeParam(IDbCommand cmd, string name, string value)
        {
            var prm = new SqlParameter("@" + name, SqlDbType.NVarChar) {Value = value};

            cmd.Parameters.Add(prm);

            ParamTest(cmd, name);

            return prm;
        }

        public static SqlParameter MakeParamNoCheck(IDbCommand cmd, string name, string value)
        {
            var prm = new SqlParameter("@" + name, SqlDbType.NVarChar) { Value = value };

            cmd.Parameters.Add(prm);

            return prm;
        }

        public static SqlParameter MakeParam(IDbCommand cmd, string name, Guid value)
        {
            var prm = new SqlParameter("@" + name, SqlDbType.UniqueIdentifier);

            prm.Value = (value == null || value == Guid.Empty) ? (object)DBNull.Value : (object)value;

            cmd.Parameters.Add(prm);

            ParamTest(cmd, name);

            return prm;
        }

        public static SqlParameter MakeParam(IDbCommand cmd, string name, RecordIdentifier value)
        {
            SqlDbType type;
            object data;
            
            if (value == null || value.IsEmpty)
            {
                throw new NotSupportedException();
            }

            if (value.IsInteger)
            {
                type = SqlDbType.Int;
                data = (int)value;
            }
            else if (value.IsBigInt)
            {
                type = SqlDbType.BigInt;
                data = (Int64)value;
            }
            else if (value.IsGuid)
            {
                type = SqlDbType.UniqueIdentifier;
                data = (Guid) value;
            }
            else
            {
                type = SqlDbType.NVarChar;
                data = (string)value;
            }
            
            var prm = new SqlParameter("@" + name, type) {Value = data};

            cmd.Parameters.Add(prm);

            ParamTest(cmd, name);

            return prm;
        }

        public static SqlParameter MakeParam(IDbCommand cmd, string name, object value, SqlDbType type, ParameterDirection direction, int length)
        {
            var prm = new SqlParameter("@" + name, type) {Value = value, Direction = direction};

            if (direction == ParameterDirection.Output || direction == ParameterDirection.InputOutput)
            {
                prm.Size = length;
            }

            cmd.Parameters.Add(prm);

            ParamTest(cmd, name);

            return prm;
        }
    }
}
