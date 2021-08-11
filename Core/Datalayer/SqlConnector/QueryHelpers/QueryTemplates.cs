namespace LSOne.DataLayer.SqlConnector.QueryHelpers
{
    public class QueryTemplates
    {
        ///  <summary>
        ///  select TOP
        ///     -- External columns
        ///     {0}
        /// from(
        ///     Select
        ///         -- Internal Columns
        ///         {1}
        ///     FROM TABLE ALIAS
        ///     -- Joins
        ///     {2}
        ///     -- Conditions
        ///     {3}
        ///     -- Internal Group by
        ///     {4}
        /// ) ss
        /// -- Paging
        /// {5}
        /// -- Sort
        /// {6}
        ///  </summary>
        ///  <param name="table"></param>
        ///  <param name="alias"></param>
        /// <param name="externalAlias"></param>
        /// <param name="top"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static string PagingQueryWithGroup(string table, string alias, string externalAlias, int top = 0, bool distinct = false)
        {
            return @"
select <DISTINCT> <TOP>
-- External columns
{0}
from(
    Select 
        -- Internal Columns
        {1}
    FROM <TABLE> <ALIAS> 
    -- Joins
    {2}
        -- Conditions
    {3}
        -- Internal Group by
    GROUP BY {4}
    ) <EXTERNALALIAS>
-- Paging
{5}
-- Sort
{6}".Replace("<TABLE>", table)
                .Replace("<ALIAS>", alias)
                .Replace("<EXTERNALALIAS>", externalAlias)
                .Replace("<TOP>", top == 0 ? string.Empty : string.Format("TOP {0}", top))
                .Replace("<DISTINCT>", distinct ? "DISTINCT" : string.Empty);
        }

        ///  <summary>
        ///  select TOP
        ///     -- External columns
        ///     {0}
        /// from(
        ///     Select
        ///         -- Internal Columns
        ///         {1}
        ///     FROM TABLE ALIAS
        ///     -- Joins
        ///     {2}
        ///     -- Conditions
        ///     {3}
        /// ) ss
        /// -- Paging
        /// {4}
        /// -- Sort
        /// {5}
        ///  </summary>
        ///  <param name="table"></param>
        ///  <param name="alias"></param>
        /// <param name="externalAlias"></param>
        /// <param name="top"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static string PagingQuery(string table, string alias, string externalAlias, int top = 0, bool distinct = false)
        {
            return @"
select <DISTINCT> <TOP>
-- External columns
{0}
from(
    Select 
        -- Internal Columns
        {1}
    FROM <TABLE> <ALIAS> 
    -- Joins
    {2}
        -- Conditions
    {3}
    ) <EXTERNALALIAS>
-- Paging
{4}
-- Sort
{5}".Replace("<TABLE>", table)
                .Replace("<ALIAS>", alias)
                .Replace("<EXTERNALALIAS>", externalAlias)
                .Replace("<TOP>", top == 0 ? string.Empty : string.Format("TOP {0}", top))
                .Replace("<DISTINCT>", distinct ? "DISTINCT" : string.Empty);
        }

        ///  <summary>
        ///  select TOP
        ///     -- External columns
        ///     {0}
        /// from(
        ///     Select
        ///         -- Internal Columns
        ///         {1},
        ///         {2}
        ///     FROM TABLE ALIAS
        ///     -- Joins
        ///     {3}
        ///     -- Conditions
        ///     {4}
        /// ) ss
        /// -- Paging
        /// {5}
        /// -- Sort
        /// {6}
        ///  </summary>
        ///  <param name="table"></param>
        ///  <param name="alias"></param>
        /// <param name="externalAlias"></param>
        /// <param name="top"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static string PagingQueryWithSepparateRowColumn(string table, string alias, string externalAlias, int top = 0, bool distinct = false)
        {
            return @"
select <DISTINCT> <TOP>
-- External columns
{0}
from(
    Select 
        -- Internal Columns
        {1},
        {2}
    FROM <TABLE> <ALIAS> 
    -- Joins
    {3}
        -- Conditions
    {4}
    ) <EXTERNALALIAS>
-- Paging
{5}
-- Sort
{6}".Replace("<TABLE>", table)
                .Replace("<ALIAS>", alias)
                .Replace("<EXTERNALALIAS>", externalAlias)
                .Replace("<TOP>", top == 0 ? string.Empty : string.Format("TOP {0}", top))
                .Replace("<DISTINCT>", distinct ? "DISTINCT" : string.Empty);
        }

        ///  <summary>
        ///  select TOP
        ///     -- External columns
        ///     {0}
        /// from(
        ///     Select
        ///         -- Internal Columns
        ///         {1}
        ///     FROM TABLE ALIAS
        ///     -- Joins
        ///     {2}
        ///     -- Conditions
        ///     {3}
        ///     -- Internal Group
        ///     {4}
        /// ) ss
        /// -- Joins
        ///    {5}
        ///-- Paging
        ///{6}
        ///-- Sort/Group
        ///{7}
        ///  </summary>
        ///  <param name="table"></param>
        ///  <param name="alias"></param>
        /// <param name="externalAlias"></param>
        /// <param name="top"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static string InternalQuery(string table, string alias, string externalAlias, int top = 0, bool distinct = false)
        {
            return @"
select <DISTINCT> <TOP>
-- External columns
{0}
from(
    Select 
        -- Internal Columns
        {1}
    FROM <TABLE> <ALIAS> 
    --joins
    {2}
        -- Conditions
    {3}
         --Internal Group
        {4}
    ) <EXTERNALALIAS>
    -- Joins
    {5}
-- Paging
{6}
-- Sort/Group
{7}".Replace("<TABLE>", table)
                .Replace("<ALIAS>", alias)
                .Replace("<EXTERNALALIAS>", externalAlias)
                .Replace("<TOP>", top == 0 ? string.Empty : string.Format("TOP {0}", top))
                .Replace("<DISTINCT>", distinct ? "DISTINCT" : string.Empty);
        }

        ///  <summary>
        ///  Select DISTINCT TOP
        /// -- Columns
        /// {0}
        /// FROM TABLE ALIAS
        /// -- Joins
        /// {1}
        ///  -- Conditions
        /// {2}
        /// -- Sort
        /// {3}
        ///  </summary>
        ///  <param name="table"></param>
        ///  <param name="alias"></param>
        /// <param name="top"></param>
        /// <param name="distinct"></param>
        /// <returns></returns>
        public static string BaseQuery(string table, string alias, int top = 0, bool distinct = false)
        {
            return
                @"
Select <DISTINCT> <TOP> 
    -- Columns
    {0}
FROM <TABLE> <ALIAS> 
-- Joins
{1}
    -- Conditions
{2}
-- Sort
{3}".Replace("<TABLE>", table)
                    .Replace("<ALIAS>", alias)
                    .Replace("<TOP>", top == 0 ? string.Empty : string.Format("TOP {0}", top))
                    .Replace("<DISTINCT>", distinct ? "DISTINCT": string.Empty);
        }

        /// <summary>
        /// select TOP
        ///-- External columns
        ///        {0}
        ///from(
        ///    { 1}
        ///        UNION
        ///    {2}
        ///    ) EXTERNALALIAS
        ///-- Paging
        ///{3}
        ///-- Sort
        ///{4}
        /// </summary>
        /// <param name="externalAlias"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static string UnionQuery(string externalAlias, int top = 0)
        {
            return @"
select <TOP>
-- External columns
{0}
from(
    {1}
    UNION
    {2}
    ) <EXTERNALALIAS>
-- Paging
{3}
-- Sort
{4}".Replace("<EXTERNALALIAS>", externalAlias).Replace("<TOP>", top == 0 ? string.Empty : string.Format("TOP {0}", top));
        }
    }
}