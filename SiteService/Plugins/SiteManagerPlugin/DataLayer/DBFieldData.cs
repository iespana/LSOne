using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.SiteService.Plugins.SiteManager.DataLayer.DataEntities;

namespace LSOne.SiteService.Plugins.SiteManager.DataLayer
{
    internal class DBFieldData : SqlServerDataProviderBase, IDBFieldData
    {
        private static void PopulateField(IDataReader dr, DBField dbField)
        {
            dbField.Name = (string)dr["NAME"];
            
            SqlDbType dbType = GetDbTypeFromString((string)dr["DBTYPE"]);
            dbField.DBType = dbType;

            dbField.IsPrimaryKey = (dr["CONSTRAINTNAME"] != DBNull.Value);
            dbField.IsIdentity = Convert.ToInt32(dr["ISIDENTITY"]) != 0;
        }

        private static SqlDbType GetDbTypeFromString(string stringType)
        {
            switch (stringType)
            {
                case "bit":
                    return SqlDbType.Bit;
                case "date":
                    return SqlDbType.Date;
                case "datetime":
                    return SqlDbType.DateTime;
                case "bigint":
                    return SqlDbType.BigInt;
                case "decimal":
                    return SqlDbType.Decimal;
                case "image":
                    return SqlDbType.Image;
                case "int":
                    return SqlDbType.Int;
                case "ntext":
                    return SqlDbType.NText;
                case "numeric":
                    return SqlDbType.Decimal;
                case "nvarchar":
                    return SqlDbType.NVarChar;
                case "tinyint":
                    return SqlDbType.TinyInt;
                case "time":
                    return SqlDbType.Time;
                case "timestamp":
                    return SqlDbType.Timestamp;
                case "uniqueidentifier":
                    return SqlDbType.UniqueIdentifier;
                case "varbinary":
                    return SqlDbType.VarBinary;

                default:
                    return SqlDbType.NVarChar;
            }
        }

        private static List<DBField> GetFieldsForTable(IConnectionManager entry, string tableName)
        {
            List<DBField> result;
            SqlCommand cmd;

            ValidateSecurity(entry);

            using (cmd = new SqlCommand())
            {
                cmd.CommandText = "Select columns.COLUMN_NAME as NAME, columns.DATA_TYPE as DBTYPE, tc.CONSTRAINT_NAME as CONSTRAINTNAME, " +
                                    "COLUMNPROPERTY(object_id(columns.TABLE_NAME), columns.COLUMN_NAME, 'IsIdentity') as ISIDENTITY " +
                                    "From INFORMATION_SCHEMA.COLUMNS columns " +
                                    "Left outer join INFORMATION_SCHEMA.KEY_COLUMN_USAGE const on columns.TABLE_NAME = const.TABLE_NAME and columns.COLUMN_NAME = const.COLUMN_NAME " +
                                    "Left outer join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc on const.CONSTRAINT_NAME = tc.CONSTRAINT_NAME and tc.CONSTRAINT_TYPE = 'PRIMARY KEY'  " +
                                    "Where columns.TABLE_NAME = @tableName";

                MakeParam(cmd, "tableName", tableName);

                result = Execute<DBField>(entry, cmd, CommandType.Text, PopulateField);
            }

            return result;
        }

        public List<DBField> GetAllFieldsForTable(IConnectionManager entry, string tableName)
        {
            // Make sure we get each field only once - fields that are both primary keys and foreigns keys are reported twice by GetFieldsForTable(...)
            HashSet<string> fieldNames = new HashSet<string>();
            List<DBField> fields = new List<DBField>();
            foreach (var field in GetFieldsForTable(entry, tableName))
            {
                if (!fieldNames.Contains(field.Name))
                {
                    fieldNames.Add(field.Name);
                    fields.Add(field);
                }
            }

            return fields;
        }

        public List<DBField> GetPrimaryFieldsForTable(IConnectionManager entry, string tableName)
        {
            List<DBField> allFields = GetFieldsForTable(entry, tableName);
            List<DBField> primaryFields = new List<DBField>();

            foreach (DBField field in allFields)
            {
                if (field.IsPrimaryKey)
                {
                    primaryFields.Add(field);
                }   
            }

            return primaryFields;
        }

        public void Insert(IConnectionManager entry, string tableName, List<DBField> fields)
        {
            bool hasIdentityColumn = false;
            SqlServerStatement statement = new SqlServerStatement(tableName);

            ValidateSecurity(entry, Permission.AdministrationMaintainSettings);

            statement.StatementType = StatementType.Insert;

            foreach (DBField field in fields)
            {
                if (field.Value != DBNull.Value || field.Value == null)
                {

                    if (field.IsPrimaryKey)
                    {
                        statement.AddKey(field.Name, field.Value, field.DBType);
                    }
                    else
                    {
                        statement.AddField(field.Name, field.Value, field.DBType);
                    }

                    if (field.IsIdentity)
                    {
                        hasIdentityColumn = true;
                    }
                }
            }

            if (hasIdentityColumn)
            {
                SqlCommand cmd = new SqlCommand("SET IDENTITY_INSERT " + tableName + " ON");

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }

            //try
            //{
                entry.Connection.ExecuteStatement(statement);
            //}
            //catch(Exception e)
            //{
            //    MessageDialog.Show(e.Message);
            //}
 
            if (hasIdentityColumn)
            {
                SqlCommand cmd = new SqlCommand("SET IDENTITY_INSERT " + tableName + " OFF");

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }

        public bool Exists(IConnectionManager entry, string tableName, List<DBField> fields)
        {
            bool isFirst = true;
            IDataReader dr = null;

            using (SqlCommand cmd = new SqlCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select 'x' " +
                                  "from " + tableName + " where ";

                foreach (DBField field in fields)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        cmd.CommandText += " and ";
                    }

                    cmd.CommandText += field.Name + "=@" + field.Name;  
                    MakeParam(cmd, field.Name, field.Value, field.DBType);                    
                }

                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
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



