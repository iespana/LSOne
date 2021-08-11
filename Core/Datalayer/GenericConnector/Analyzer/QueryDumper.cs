using System;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace LSOne.DataLayer.GenericConnector.Analyzer
{
    internal class QueryDumper
    {
        private static string GetFileName()
        {
            string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                @"LS Retail\Queries\" + ConnectionBase.SessionDumpName);
            Directory.CreateDirectory(directory);
            string fileName = Path.Combine(directory,
                DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
            char c = 'A';
            var extra = "";
            var levelExtra = "";
            while (File.Exists(fileName + levelExtra + extra + ".sql"))
            {
                extra = "" + c;
                c++;
                if (c > 'Z')
                {
                    levelExtra += 'A';
                    c = 'A';
                }
            }

            return fileName + levelExtra + extra + ".sql";
        }

        private static void DumpStack(StreamWriter sw)
        {
            var st = new StackTrace();
            sw.WriteLine("/*");
            sw.WriteLine(st.ToString());
            sw.WriteLine("*/");
            sw.WriteLine("");
        }

        public static void DumpQuery(string query)
        {
            var fileName = GetFileName();
            using (var sw = new StreamWriter(fileName))
            {
                DumpStack(sw);

                sw.WriteLine();
                sw.WriteLine(query);
            }            
        }

        public static void DumpQuery(IDbCommand cmd)
        {
            var fileName = GetFileName();
            using (var sw = new StreamWriter(fileName))
            {
                try
                {
                    DumpStack(sw);

                    if (cmd.CommandType == CommandType.StoredProcedure)
                        sw.Write("exec " + cmd.CommandText);
                    else
                        sw.WriteLine("-- declare parameters");

                    int paramIndex = 0;
                    foreach (IDataParameter param in cmd.Parameters)
                    {
                        paramIndex++;

                        string declare = "declare " + param.ParameterName + " as ";
                        string setter = "";

                        switch (param.DbType)
                        {
                            case DbType.Byte:
                            case DbType.Boolean:
                            case DbType.Int16:
                            case DbType.Int32:
                            case DbType.Int64:
                            case DbType.SByte:
                            case DbType.Single:
                            case DbType.UInt16:
                            case DbType.UInt32:
                            case DbType.UInt64:
                                declare += "int";
                                setter += param.Value;
                                break;
                            case DbType.Currency:
                            case DbType.Decimal:
                            case DbType.Double:
                            case DbType.VarNumeric:
                                declare += "numeric(19,5)";
                                setter += param.Value;
                                break;
                            case DbType.Date:
                                break;
                            case DbType.DateTime:
                            case DbType.DateTime2:
                                declare += "datetime";
                                setter += string.Format("'{0}'", ((DateTime) param.Value).ToString("yyyy-MM-dd"));
                                break;
                            case DbType.AnsiString:
                            case DbType.AnsiStringFixedLength:
                            case DbType.StringFixedLength:
                            case DbType.String:
                                if (param.Value == null)
                                {
                                    declare += "nvarchar(100)";
                                    setter = setter.Substring(0, setter.Length - 2) + " as NULL";
                                }
                                else
                                {
                                    declare += "nvarchar(" + (param.Value.ToString().Length + 10) + ")";
                                    setter += "'" + param.Value + "'";
                                }
                                break;
                            case DbType.Time:
                            case DbType.DateTimeOffset:
                            case DbType.Binary:
                            case DbType.Guid:
                            case DbType.Object:
                            case DbType.Xml:
                            default:
                                declare += param.DbType.ToString();
                                setter += param.Value;
                                break;
                        }

                        if (cmd.CommandType == CommandType.StoredProcedure)
                        {
                            if (paramIndex > 1)
                            {
                                sw.Write(", ");
                            }
                            sw.Write(' ');
                            sw.Write(param.ParameterName);
                            sw.Write('=');
                            sw.Write(setter);
                        }
                        else
                        {
                            sw.WriteLine(declare + ";");
                            sw.Write("set " + param.ParameterName + " = ");
                            sw.WriteLine(setter + ";");
                        }
                    }

                    sw.WriteLine();
                    if (cmd.CommandType != CommandType.StoredProcedure)
                        sw.WriteLine(cmd.CommandText);
                }
                catch (Exception ex)
                {
                    sw.WriteLine("Exception occurred in dumper");
                    while (ex != null)
                    {
                        sw.WriteLine(ex.Message);
                        ex = ex.InnerException;
                    }
                }
            }
        }
    }
}
