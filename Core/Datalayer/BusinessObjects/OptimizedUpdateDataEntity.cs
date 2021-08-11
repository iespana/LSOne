#if !MONO
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Exceptions;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.GUI;
using LSOne.Utilities.Helpers;

namespace LSOne.DataLayer.BusinessObjects
{
    /// <summary>
    /// Use this class when you want a business object to implement the <see cref="IOptimizedUpdate"/> interface. This class provides the base implementation and utility
    /// functions needed to complete the implementation.
    /// </summary>
    [Serializable]
    [DataContract]
    public class OptimizedUpdateDataEntity : DataEntity, IOptimizedUpdate
    {
        public OptimizedUpdateDataEntity(RecordIdentifier id, string text)
           : base(id,text)
        {
        }

        public OptimizedUpdateDataEntity()
            :base()
        {
        }

        public bool DataIsEqualTo(IOptimizedUpdate optimizedUpdateEntity)
        {
            var dataIsEqual = true;

            if(optimizedUpdateEntity.ColumnUpdateValues == null)
            {
                optimizedUpdateEntity.ColumnUpdateValues = new Dictionary<string, OptimizedUpdateColumn>();
            }

            foreach (string columnName in ColumnUpdateValues.Keys)
            {
                if (new [] { "CREATED", "MODIFIED" }.Contains(columnName))
                {
                    continue;
                }

                if (!optimizedUpdateEntity.ColumnUpdateValues.ContainsKey(columnName) ||
                    ((optimizedUpdateEntity.ColumnUpdateValues[columnName].Value == null) ^ (ColumnUpdateValues[columnName].Value == null)) ||
                    ((optimizedUpdateEntity.ColumnUpdateValues[columnName].Value != null) && !optimizedUpdateEntity.ColumnUpdateValues[columnName].Value.Equals(ColumnUpdateValues[columnName].Value)))
                {
                    dataIsEqual = false;
                    break;
                }
            }

            // If one field is null or empty (and therefore is not part of the ColumnUpdateValues list) and 
            // the other is not (and therefore is part of the ColumnUpdateValues list), then the data is not equal

            // Make sure the null or empty value is added in the ColumnUpdateValues list, so the query is correctly generated and the
            // corresponding field is updated with this null or empty value; we need this when the value that exists in the DB is not null/empty
            // Some entities need special attention:
            //  eg: RetailItem has InventoryUnitId not nullable; it is updated by default with a default value (input value null becomes "LS1 DEFAULT" in DB)
            //      the RetailItem that comes from the web request may have a null/empty value
            //      in this case, the below query doesn't know that the existing value in the DB is the real default value (it thinks that "LS1 DEFAULT" is a non default/not null/not empty value),
            //      so we need to threat this differently since empty/null would eventually be converted to this default value
            //      so we just ignore these fields to optimize SaveList()
            // Sometimes optimizedUpdateEntity.ColumnUpdateValues.Count == ColumnUpdateValues.Count, but the involved fields are different,
            // in this case make sure the entity is marked as different, so it gets updated
            var missingColumnInfos = optimizedUpdateEntity.ColumnUpdateValues.Keys
                                                                                .Except(ColumnUpdateValues.Keys)
                                                                                .Except(optimizedUpdateEntity.GetIgnoredColumns())
                                                                                .Where(x => !x.Contains("MASTERID"))
                                                                                .ToList();

            missingColumnInfos.ForEach(columnName => PropertyChanged(columnName, optimizedUpdateEntity.ColumnUpdateValues[columnName].Value));

            if (missingColumnInfos.Any())
            {
                dataIsEqual = false;
            }

            return dataIsEqual && NestedDataIsEqualTo(optimizedUpdateEntity);
        }

        /// <summary>
        /// Return a list with the columns we want to ignore when we determine whether two entities are different.
        /// One way to determine whether two entities are different is to compare the number of columns contained by ColumnUpdateValues dictionary.
        /// If the number of contained columns in the ColumnUpdateValues dictionary are different, then the two entities are different.
        /// However, there are a few exception to this rule:
        /// 1. If the incoming entity contains a string.Empty for example, in the DB this value might be stored as a default value which is not equal to string.Empty;
        ///    Example: the unitId; the received UnitID could be string.Empty; but in the DB the associated column is not nullable, so we store the value "LS1 DEFAULT"
        ///    In this case we would have the UnitID in the DB list ColumnUpdateValues. But since the database value "LS1 DEFAULT" is similar to string.Empty from the web request
        ///    we choose to ignore this column
        /// 2. If the incoming entity contains a calculated column, like TradeAgreementEntry; in the DB this value would have a value,
        ///    so the associated column would be part of the ColumnUpdateValues dictionary; but the request won't have this value set, so the corresponding column would be missed.
        ///    In this case we ignore this column because it will be calculated anyway, and the values of this property will be different only if the other dependency fields will be different.
        ///    So we will be able to spot the difference between the two object anyway, when we compare the other fields.
        /// </summary>
        /// <returns></returns>
        public virtual List<string> GetIgnoredColumns() => new List<string>();

        protected virtual bool NestedDataIsEqualTo(IOptimizedUpdate optimizedUpdateEntity) => true;

        [IgnoreDataMember]
        public Dictionary<string, OptimizedUpdateColumn> ColumnUpdateValues { get; set; }

        /// <summary>
        /// Adds the column name to <see cref="IOptimizedUpdate.ColumnUpdateValues"/> with the given <see cref="OptimizedUpdateColumnAction"/> and value
        /// </summary>
        /// <param name="columnName">The name of the column in the underlying database table</param>
        /// <param name="columnAction">(Optional) What action should be taken in case the column does not match any of the columns in the table when trying to do an update</param>
        /// <param name="value">(Optional) value to be assigned to the column in the underlying database table. If set then the value is recorded into <see cref="ColumnUpdateValues"/></param>
        protected void AddColumnInfo(string columnName, OptimizedUpdateColumnAction columnAction = OptimizedUpdateColumnAction.Error, object value = null)
        {
            if (ColumnUpdateValues == null)
            {
                ColumnUpdateValues = new Dictionary<string, OptimizedUpdateColumn>();
            }

            if (ColumnUpdateValues.ContainsKey(columnName))
            {
                ColumnUpdateValues[columnName].Value = value;
            }
            else
            {
                ColumnUpdateValues.Add(columnName, new OptimizedUpdateColumn(columnName, columnAction, value));
            }
        }

        protected override void OnDeserializing()
        {
            InitializeBase();
            Initialize();
        }

        /// <summary>
        /// Initializes all private variables that are backing fields for "optimized" properties. I.e any property that calls <see cref="AddColumnInfo"/> needs to have it's backing field
        /// initialized here so that they get properly initialized when the object is deserialized.
        /// </summary>
        protected virtual void Initialize()
        {
            
        }

        /// <summary>
        /// Should perform the same logic as <see cref="Initialize"/> but this should only be overridden on base business object classes such as <see cref="CustomerListItem"/> or <see cref="SimpleRetailItem"/>
        /// </summary>
        protected virtual void InitializeBase()
        {
            
        }

        /// <summary>
        /// Perform logic that is related to property changes like adding column info
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value">(Optional) value to be assigned to the column in the underlying database table. If set then the value is recorded into <see cref="ColumnUpdateValues"/></param>
        protected virtual void PropertyChanged(string columnName, object value = null)
        {
            AddColumnInfo(columnName, value: value);
        }
    }
}