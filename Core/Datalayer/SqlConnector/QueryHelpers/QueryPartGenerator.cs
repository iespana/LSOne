using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.SqlConnector.QueryHelpers
{
    public class QueryPartGenerator
    {

        public static void AddSearchFlagsToCondition(List<SearchFlagEntity> searchFlags, List<Condition> conditions)
        {
            foreach (var itemSearchFlag in searchFlags)
            {
                if (itemSearchFlag.Value == FlagCheckValue.Checked)
                {
                    if (itemSearchFlag.BitColumn)
                    {
                        conditions.Add(new Condition
                        {
                            ConditionValue = $"{itemSearchFlag.ColumnName} = 1",
                            Operator = "AND"
                        });
                    }
                    else
                    {
                        conditions.Add(new Condition
                        {
                            ConditionValue = $"({itemSearchFlag.ColumnName})",
                            Operator = "AND"
                        });
                    }
                }
                else if (itemSearchFlag.Value == FlagCheckValue.Unchecked)
                {
                    if (itemSearchFlag.BitColumn)
                    {
                        conditions.Add(new Condition
                        {
                            ConditionValue = $"{itemSearchFlag.ColumnName} = 0",
                            Operator = "AND"
                        });
                    }
                    else
                    {
                        conditions.Add(new Condition
                        {
                            ConditionValue = $"({itemSearchFlag.UncheckedValue})",
                            Operator = "AND"
                        });
                    }
                }

            }
        }
        public static string ConditionGenerator(List<Condition> conditions, bool join = false)
        {
            string reply = string.Empty;
            string startToken = join ? string.Empty : "WHERE ";
            for (int i = 0; i < conditions.Count; i++)
            {
                var condition = conditions[i];
                if (i == 0)
                {
                    reply += startToken + condition.ConditionValue + Environment.NewLine;
                }
                else
                {
                    reply += condition + Environment.NewLine;
                }
            }
            return reply;
        }

        public static string ConditionGenerator(Condition condition)
        {
            return "WHERE " + condition.ConditionValue + Environment.NewLine; 
        }

        public static string ExternalColumnGenerator(List<TableColumn> columns, string alias = "ss")
        {
            string externalColumns = string.Empty;
            for (int i = 0; i < columns.Count; i++)
            {

                TableColumn column = columns[i];
                if (!column.ExcludeExternally)
                {
                    if (column.IsCustomExternalColumn)
                    {
                        externalColumns += string.Format("{0}" + Environment.NewLine,column.CustomText);
                    }
                    else
                    {
                        string aggregationStart = column.AggregateExternally == AggregationSetting.ExternalAggregation ? $"{column.AggregateFunction}(" : "";
                        string aggregationEnd = column.AggregateExternally == AggregationSetting.ExternalAggregation ? $") AS {column.ColumnAlias}" : "";

                        externalColumns += string.Format("{0}{1}.{2}{3}{4}" + Environment.NewLine, aggregationStart,
                            alias,
                            column.ColumnAlias, aggregationEnd,
                            i == columns.Count - 1 ? string.Empty : ",");
                    }
                }
                else if (i == columns.Count - 1)
                {
                    externalColumns = externalColumns.Substring(0,externalColumns.Length - 2);
                }
            }
            if (externalColumns.EndsWith(","))
            {
                externalColumns = externalColumns.Remove(externalColumns.Length - 1);
            }
            return externalColumns;
        }

        public static string InternalColumnGenerator(List<TableColumn> columns)
        {
            string internalColumns = string.Empty;
            for (int i = 0; i < columns.Count; i++)
            {
                TableColumn column = columns[i];
                if (!column.IsCustomExternalColumn)
                {
                    internalColumns += string.Format("{0}{1}" + Environment.NewLine, column,
                        i == columns.Count - 1 ? string.Empty : ",");
                }
            }
            if (internalColumns.EndsWith(","))
            {
                internalColumns = internalColumns.Remove(internalColumns.Length - 1);
            }
            return internalColumns;
        }

        public static string GroupByColumnGenerator(List<TableColumn> columns,bool external = false)
        {
            string internalColumns = string.Empty;
            for (int i = 0; i < columns.Count; i++)
            {
                TableColumn column = columns[i];
                internalColumns += string.Format("{0}{1}" + Environment.NewLine, column.ToGroupByString(external),
                    i == columns.Count - 1 ? string.Empty : ",");

            }
            return internalColumns;
        }
        public static string JoinGenerator(List<Join> joins)
        {
            string queryJoins = string.Empty;
            foreach (Join join in joins)
            {
                queryJoins += join + Environment.NewLine;
            }
            return queryJoins;
        }

    }
}
