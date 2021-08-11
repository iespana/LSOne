using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders.CustomerOrders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.CustomerOrders
{
    public partial class CustomerOrderAdditionalConfigurationData : SqlServerDataProviderBase, ICustomerOrderAdditionalConfigurationData
    {
        /// <summary>
        /// Select statement format: 
        ///     Select 
        ///     -- Columns 
        ///     {0} 
        ///     FROM CUSTOMERORDERSADDITIONALCONFIG A  
        /// </summary>
        private string NoCondition
        {
            get
            {
                return @"
    Select 
        -- Columns
        {0}
    FROM CUSTOMERORDERSADDITIONALCONFIG A         
    ";
            }
        }

        /// <summary>
        /// Select statement format: 
        ///     Select 
        ///     -- Columns 
        ///     {0} 
        ///     FROM CUSTOMERORDERSADDITIONALCONFIG A  
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
    FROM CUSTOMERORDERSADDITIONALCONFIG A  
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
                        new TableColumn {ColumnName = "ID", TableAlias = "A"},
                        new TableColumn {ColumnName = "TYPE", TableAlias = "A"},
                        new TableColumn {ColumnName = "DESCRIPTION", TableAlias = "A"}
                    });
                }
                return selectionColumns;
            }
        }

        public virtual List<CustomerOrderAdditionalConfigurations> GetList(IConnectionManager entry)
        {
            List<CustomerOrderAdditionalConfigurations> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = SelectionColumns[ColumnPopulation.DataEntity].ToList();

                string commandText = NoCondition;

                cmd.CommandText = string.Format(commandText, QueryPartGenerator.InternalColumnGenerator(columns));

                records = Execute<CustomerOrderAdditionalConfigurations>(entry, cmd, CommandType.Text, Populate);
            }

            return records;
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry, ConfigurationType type)
        {
            List<CustomerOrderAdditionalConfigurations> list = GetList(entry);

            List<DataEntity> result = new List<DataEntity>();

            foreach (var item in list.Where(w => w.AdditionalType == type))
            {
                DataEntity newItem = new DataEntity();
                newItem.ID = item.ID;
                newItem.Text = item.Text;
                result.Add(newItem);
            }

            return result;
        }

        public virtual CustomerOrderAdditionalConfigurations Get(IConnectionManager entry, RecordIdentifier ID)
        {
            List<CustomerOrderAdditionalConfigurations> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = SelectionColumns[ColumnPopulation.DataEntity].ToList();

                List<Condition> conditions = new List<Condition> { new Condition { Operator = "AND", ConditionValue = "A.ID = @ID" } };
                string commandText = WithCondition;

                cmd.CommandText = string.Format(commandText,
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.ConditionGenerator(conditions));

                MakeParam(cmd, "ID", ID);

                records = Execute<CustomerOrderAdditionalConfigurations>(entry, cmd, CommandType.Text, Populate);
            }

            return records.Count > 0 ? records.FirstOrDefault() : new CustomerOrderAdditionalConfigurations();
        }

        protected virtual void Populate(IDataReader dr, CustomerOrderAdditionalConfigurations config)
        {
            config.ID = (Guid)dr["ID"];
            config.AdditionalType = (ConfigurationType)(int)dr["TYPE"];
            config.Text = (string)dr["DESCRIPTION"];
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            var statement = new SqlServerStatement("CUSTOMERORDERSADDITIONALCONFIG");

            if (Exists(entry, ID))
            {
                statement.StatementType = StatementType.Delete;
                statement.AddCondition("ID", (string)ID);

                entry.Connection.ExecuteStatement(statement);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "CUSTOMERORDERSADDITIONALCONFIG", "ID", ID, false);
        }

        public virtual bool ConfigIsInUse(IConnectionManager entry, CustomerOrderAdditionalConfigurations config)
        {
            return RecordExists(entry, "CUSTOMERORDERS", config.AdditionalType == ConfigurationType.Delivery ? "DELIVERY" : "SOURCE", config.ID, false);
        }

        public virtual void Save(IConnectionManager entry, CustomerOrderAdditionalConfigurations item)
        {
            if (item == null)
            {
                return;
            }

            if (item.ID == Guid.Empty)
            {
                item.ID = Guid.NewGuid();
            }

            var statement = new SqlServerStatement("CUSTOMERORDERSADDITIONALCONFIG");

            if (!Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (string)item.ID);

            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (string)item.ID);
            }

            statement.AddField("TYPE", (int)item.AdditionalType, SqlDbType.Int);
            statement.AddField("DESCRIPTION", (string)item.Text);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
