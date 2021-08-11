using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;

namespace LSOne.DataLayer.SqlDataProviders.Reports
{
    public class ProcedureParameterData : SqlServerDataProviderBase, IProcedureParameterData
    {
        private static void PopulateParameter(IDataReader dr, ProcedureParameter param)
        {
            param.Name = (string)dr["PARAMETER_NAME"];
            param.MaxLength = (int)dr["CHARACTER_MAXIMUM_LENGTH"];
            param.IsInput = (((string)dr["PARAMETER_MODE"]) == "IN");
            param.IsResult = (((string)dr["IS_RESULT"]) != "NO");

            switch ((string)dr["DATA_TYPE"])
            {
                case "nvarchar":
                    param.DataType = SqlDbType.NVarChar;
                    break;

                case "bit":
                    param.DataType = SqlDbType.Bit;
                    break;

                case "tinyint":
                    param.DataType = SqlDbType.TinyInt;
                    break;

                case "uniqueidentifier":
                    param.DataType = SqlDbType.UniqueIdentifier;
                    break;

                // TODO add more types here as we go on
            }
        }

        public virtual List<ProcedureParameter> GetParameters(IConnectionManager entry, string procedureName)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select PARAMETER_NAME,DATA_TYPE,ISNULL(CHARACTER_MAXIMUM_LENGTH,0) as CHARACTER_MAXIMUM_LENGTH, PARAMETER_MODE, IS_RESULT " +
                    " from information_schema.parameters where specific_name=@name order by ORDINAL_POSITION";

                MakeParam(cmd, "name", procedureName);

                return Execute<ProcedureParameter>(entry, cmd, CommandType.Text, PopulateParameter);
            }
        }


        public virtual DataTable ExecuteReportQuery(IConnectionManager entry,string procedureName,List<ProcedureParameter> parameters)
        {
            string value;
            IDataReader dr = null;

            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                foreach(ProcedureParameter parameter in parameters)
                {
                    if (parameter.IsInput && !parameter.IsResult)
                    {
                        if (parameter.DataType == SqlDbType.NVarChar)
                        {
                            if (parameter.Value is string[])
                            {
                                value = "<p>";

                                for (int i = 0; i < ((string[])parameter.Value).Length; i++)
                                {
                                    //value += ("<v>" + ((string[])parameter.Value)[i] + "</v>");
                                    value += ("<v>" + ((string[])parameter.Value)[i].Replace("&", "&amp;") + "</v>");
                                }

                                value += "</p>";

                                MakeParam(cmd, parameter.Name.Replace("@", ""), value);
                            }
                            else
                            {
                                MakeParam(cmd, parameter.Name.Replace("@", ""), (string)parameter.Value);
                            }
                        }
                        else
                        {
                            MakeParam(cmd, parameter.Name.Replace("@", ""), parameter.Value, parameter.DataType);
                        }
                    }
                }

                try
                {
                    cmd.CommandTimeout = 300;
                    
                    dr = entry.Connection.ExecuteReader(cmd);

                    DataTable dt = new DataTable();

                    dt .Load(dr);

                    return dt;
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }
        }
    }
}
