using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Contacts;
using LSOne.DataLayer.DataProviders.CustomerOrders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.CustomerOrders
{
    public partial class CustomerOrderSettingsData : SqlServerDataProviderBase, ICustomerOrderSettings
    {
        //private const string ConstCustomerOrder = "24BF119B-B909-48B4-A494-022EB6B93FD7";
        //private const string ConstQuote = "A7F9FBA4-03B6-431A-B318-188EC5735809";

        /// <summary>
        /// Select statement format: 
        ///     Select 
        ///     -- Columns 
        ///     {0} 
        ///     FROM CUSTOMERORDERSETTINGS A  
        /// </summary>
        private string NoCondition
        {
            get
            {
                return @"
    Select 
        -- Columns
        {0}
    FROM CUSTOMERORDERSETTINGS A         
    ";
            }
        }

        /// <summary>
        /// Select statement format: 
        ///     Select 
        ///     -- Columns 
        ///     {0} 
        ///     FROM CUSTOMERORDERSETTINGS A  
        ///     --Conditions 
        ///     {1}       
        /// </summary>
        private string WithCondition
        {
            get
            {
                return @"
    Select 
        -- Columns
        {0}
    FROM CUSTOMERORDERSETTINGS A  
        --Conditions
        {1}       
    ";
            }
        }
        

        private static Dictionary<ColumnPopulation, List<TableColumn>> selectionColumns = new Dictionary<ColumnPopulation, List<TableColumn>>();

        private Dictionary<ColumnPopulation, List<TableColumn>> SelectionColumns
        {
            get
            {
                if (selectionColumns.Count == 0)
                {
                    selectionColumns.Add(ColumnPopulation.DataEntity, new List<TableColumn>
                    {
                        new TableColumn {ColumnName = "ORDERTYPE", TableAlias = "A"},
                        new TableColumn {ColumnName = "ACCEPTSDEPOSITS", TableAlias = "A"},
                        new TableColumn {ColumnName = "MINIMUMDEPOSITS", TableAlias = "A"},
                        new TableColumn {ColumnName = "SOURCE", TableAlias = "A"},
                        new TableColumn {ColumnName = "DELIVERY", TableAlias = "A"},
                        new TableColumn {ColumnName = "EXPIRATIONTIMEVALUE", TableAlias = "A"},
                        new TableColumn {ColumnName = "EXPIRATIONTIMEUNIT", TableAlias = "A"},
                        new TableColumn {ColumnName = "NUMBERSERIES", TableAlias = "A"},

                    });
                }
                return selectionColumns;
            }
        }

        public void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "CUSTOMERORDERSETTINGS", "ORDERTYPE", ID, false);
        }

        public List<CustomerOrderSettings> GetList(IConnectionManager entry)
        {
            List<CustomerOrderSettings> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = SelectionColumns[ColumnPopulation.DataEntity].ToList();

                string commandText = NoCondition;

                cmd.CommandText = string.Format(commandText,
                    QueryPartGenerator.InternalColumnGenerator(columns));

                records = Execute<CustomerOrderSettings>(entry, cmd, CommandType.Text, Populate);
            }

            return records;
        }

        public CustomerOrderSettings Get(IConnectionManager entry, CustomerOrderType orderType)
        {
            return Get(entry, orderType == CustomerOrderType.CustomerOrder ? CustomerOrderSettingsConstants.ConstCustomerOrder : CustomerOrderSettingsConstants.ConstQuote);
        }

        public CustomerOrderSettings Get(IConnectionManager entry, RecordIdentifier ID)
        {
            List<CustomerOrderSettings> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = SelectionColumns[ColumnPopulation.DataEntity].ToList();

                List<Condition> conditions = new List<Condition> { new Condition { Operator = "AND", ConditionValue = "A.ORDERTYPE = @ID" } };
                string commandText = WithCondition;

                cmd.CommandText = string.Format(commandText,
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.ConditionGenerator(conditions));

                MakeParam(cmd, "ID", ID);
                
                records = Execute<CustomerOrderSettings>(entry, cmd, CommandType.Text, Populate);
            }

            return records.Count > 0 ? records.FirstOrDefault() : new CustomerOrderSettings();
        }


        public void Save(IConnectionManager entry, BusinessObjects.CustomerOrders.CustomerOrderSettings item)
        {
            if (item == null)
            {
                return;
            }

            item.ID = item.SettingsType == CustomerOrderType.CustomerOrder ? CustomerOrderSettingsConstants.ConstCustomerOrder : CustomerOrderSettingsConstants.ConstQuote;

            var statement = new SqlServerStatement("CUSTOMERORDERSETTINGS");

            if (!Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ORDERTYPE", (string)item.ID);

            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ORDERTYPE", (string)item.ID);
            }

            statement.AddField("ACCEPTSDEPOSITS", item.AcceptsDeposits ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MINIMUMDEPOSITS", item.MinimumDeposits, SqlDbType.Decimal);
            statement.AddField("SOURCE", item.SelectSource ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DELIVERY", item.SelectDelivery ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("EXPIRATIONTIMEVALUE", (int)item.ExpireTimeValue, SqlDbType.Int);
            statement.AddField("EXPIRATIONTIMEUNIT", (int)item.ExpirationTimeUnit, SqlDbType.Int);
            statement.AddField("NUMBERSERIES", (string) item.NumberSeries);

            entry.Connection.ExecuteStatement(statement);
        }

        protected virtual void Populate(IDataReader dr, CustomerOrderSettings settings)
        {
            settings.ID = (Guid) dr["ORDERTYPE"];
            settings.SettingsType = settings.ID.StringValue.ToUpperInvariant() == CustomerOrderSettingsConstants.ConstCustomerOrder.ToUpperInvariant() ? CustomerOrderType.CustomerOrder : CustomerOrderType.Quote;

            settings.AcceptsDeposits = (byte) dr["ACCEPTSDEPOSITS"] == 1;
            settings.MinimumDeposits = (decimal) dr["MINIMUMDEPOSITS"];
            settings.SelectSource = (byte)dr["SOURCE"] == 1;
            settings.SelectDelivery = (byte)dr["DELIVERY"] == 1;
            settings.ExpireTimeValue = (int) dr["EXPIRATIONTIMEVALUE"];
            settings.ExpirationTimeUnit = (TimeUnitEnum)(int)dr["EXPIRATIONTIMEUNIT"];
            settings.NumberSeries = (string)dr["NUMBERSERIES"];
        }
        
    }
}
