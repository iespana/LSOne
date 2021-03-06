using System;
using System.Collections.Generic;

namespace LSOne.DataLayer.SqlConnector
{
    internal class SqlReservedKeywords
    {
        private static Dictionary<string, short> reservedWords;
        private static object syncLock = new object();

        public static bool IsReserved(string name)
        {
            lock (syncLock)
            {
                if (reservedWords == null)
                    Populate();
            }
            return reservedWords.ContainsKey(name);
        }

        private static void Populate()
        {
            reservedWords = new Dictionary<string, short>(StringComparer.CurrentCultureIgnoreCase);
            reservedWords["ABSOLUTE"] = 0;
            reservedWords["ACTION"] = 0;
            reservedWords["ADD"] = 0;
            reservedWords["AFTER"] = 0;
            reservedWords["ALL"] = 0;
            reservedWords["ALLOCATE"] = 0;
            reservedWords["ALTER"] = 0;
            reservedWords["AND"] = 0;
            reservedWords["ANY"] = 0;
            reservedWords["ARE"] = 0;
            reservedWords["ARRAY"] = 0;
            reservedWords["AS"] = 0;
            reservedWords["ASC"] = 0;
            reservedWords["ASENSITIVE"] = 0;
            reservedWords["ASSERTION"] = 0;
            reservedWords["ASYMMETRIC"] = 0;
            reservedWords["AT"] = 0;
            reservedWords["ATOMIC"] = 0;
            reservedWords["AUTHORIZATION"] = 0;
            reservedWords["AVG"] = 0;
            reservedWords["BEFORE"] = 0;
            reservedWords["BEGIN"] = 0;
            reservedWords["BETWEEN"] = 0;
            reservedWords["BIGINT"] = 0;
            reservedWords["BINARY"] = 0;
            reservedWords["BIT"] = 0;
            reservedWords["BIT_LENGTH"] = 0;
            reservedWords["BLOB"] = 0;
            reservedWords["BOOLEAN"] = 0;
            reservedWords["BOTH"] = 0;
            reservedWords["BREADTH"] = 0;
            reservedWords["BY"] = 0;
            reservedWords["CALL"] = 0;
            reservedWords["CALLED"] = 0;
            reservedWords["CASCADE"] = 0;
            reservedWords["CASCADED"] = 0;
            reservedWords["CASE"] = 0;
            reservedWords["CAST"] = 0;
            reservedWords["CATALOG"] = 0;
            reservedWords["CHAR"] = 0;
            reservedWords["CHAR_LENGTH"] = 0;
            reservedWords["CHARACTER"] = 0;
            reservedWords["CHARACTER_LENGTH"] = 0;
            reservedWords["CHECK"] = 0;
            reservedWords["CLOB"] = 0;
            reservedWords["CLOSE"] = 0;
            reservedWords["COALESCE"] = 0;
            reservedWords["COLLATE"] = 0;
            reservedWords["COLLATION"] = 0;
            reservedWords["COLUMN"] = 0;
            reservedWords["COMMIT"] = 0;
            reservedWords["CONDITION"] = 0;
            reservedWords["CONNECT"] = 0;
            reservedWords["CONNECTION"] = 0;
            reservedWords["CONSTRAINT"] = 0;
            reservedWords["CONSTRAINTS"] = 0;
            reservedWords["CONSTRUCTOR"] = 0;
            reservedWords["CONTAINS"] = 0;
            reservedWords["CONTINUE"] = 0;
            reservedWords["CONVERT"] = 0;
            reservedWords["CORRESPONDING"] = 0;
            reservedWords["COUNT"] = 0;
            reservedWords["CREATE"] = 0;
            reservedWords["CROSS"] = 0;
            reservedWords["CUBE"] = 0;
            reservedWords["CURRENT"] = 0;
            reservedWords["CURRENT_DATE"] = 0;
            reservedWords["CURRENT_DEFAULT_TRANSFORM_GROUP"] = 0;
            reservedWords["CURRENT_PATH"] = 0;
            reservedWords["CURRENT_ROLE"] = 0;
            reservedWords["CURRENT_TIME"] = 0;
            reservedWords["CURRENT_TIMESTAMP"] = 0;
            reservedWords["CURRENT_TRANSFORM_GROUP_FOR_TYPE"] = 0;
            reservedWords["CURRENT_USER"] = 0;
            reservedWords["CURSOR"] = 0;
            reservedWords["CYCLE"] = 0;
            reservedWords["DATA"] = 0;
            reservedWords["DATE"] = 0;
            reservedWords["DAY"] = 0;
            reservedWords["DEALLOCATE"] = 0;
            reservedWords["DEC"] = 0;
            reservedWords["DECIMAL"] = 0;
            reservedWords["DECLARE"] = 0;
            reservedWords["DEFAULT"] = 0;
            reservedWords["DEFERRABLE"] = 0;
            reservedWords["DEFERRED"] = 0;
            reservedWords["DELETE"] = 0;
            reservedWords["DEPTH"] = 0;
            reservedWords["DEREF"] = 0;
            reservedWords["DESC"] = 0;
            reservedWords["DESCRIBE"] = 0;
            reservedWords["DESCRIPTOR"] = 0;
            reservedWords["DETERMINISTIC"] = 0;
            reservedWords["DIAGNOSTICS"] = 0;
            reservedWords["DISCONNECT"] = 0;
            reservedWords["DISTINCT"] = 0;
            reservedWords["DO"] = 0;
            reservedWords["DOMAIN"] = 0;
            reservedWords["DOUBLE"] = 0;
            reservedWords["DROP"] = 0;
            reservedWords["DYNAMIC"] = 0;
            reservedWords["EACH"] = 0;
            reservedWords["ELEMENT"] = 0;
            reservedWords["ELSE"] = 0;
            reservedWords["ELSEIF"] = 0;
            reservedWords["END"] = 0;
            reservedWords["EQUALS"] = 0;
            reservedWords["ESCAPE"] = 0;
            reservedWords["EXCEPT"] = 0;
            reservedWords["EXCEPTION"] = 0;
            reservedWords["EXEC"] = 0;
            reservedWords["EXECUTE"] = 0;
            reservedWords["EXISTS"] = 0;
            reservedWords["EXIT"] = 0;
            reservedWords["EXTERNAL"] = 0;
            reservedWords["EXTRACT"] = 0;
            reservedWords["FALSE"] = 0;
            reservedWords["FETCH"] = 0;
            reservedWords["FILTER"] = 0;
            reservedWords["FIRST"] = 0;
            reservedWords["FLOAT"] = 0;
            reservedWords["FOR"] = 0;
            reservedWords["FOREIGN"] = 0;
            reservedWords["FOUND"] = 0;
            reservedWords["FREE"] = 0;
            reservedWords["FROM"] = 0;
            reservedWords["FULL"] = 0;
            reservedWords["FUNCTION"] = 0;
            reservedWords["GENERAL"] = 0;
            reservedWords["GET"] = 0;
            reservedWords["GLOBAL"] = 0;
            reservedWords["GO"] = 0;
            reservedWords["GOTO"] = 0;
            reservedWords["GRANT"] = 0;
            reservedWords["GROUP"] = 0;
            reservedWords["GROUPING"] = 0;
            reservedWords["HANDLER"] = 0;
            reservedWords["HAVING"] = 0;
            reservedWords["HOLD"] = 0;
            reservedWords["HOUR"] = 0;
            // if here, then we can't get identity back from databaase reservedWords["IDENTITY"] = 0;
            reservedWords["IF"] = 0;
            reservedWords["IMMEDIATE"] = 0;
            reservedWords["IN"] = 0;
            reservedWords["INDICATOR"] = 0;
            reservedWords["INITIALLY"] = 0;
            reservedWords["INNER"] = 0;
            reservedWords["INOUT"] = 0;
            reservedWords["INPUT"] = 0;
            reservedWords["INSENSITIVE"] = 0;
            reservedWords["INSERT"] = 0;
            reservedWords["INT"] = 0;
            reservedWords["INTEGER"] = 0;
            reservedWords["INTERSECT"] = 0;
            reservedWords["INTERVAL"] = 0;
            reservedWords["INTO"] = 0;
            reservedWords["IS"] = 0;
            reservedWords["ISOLATION"] = 0;
            reservedWords["ITERATE"] = 0;
            reservedWords["JOIN"] = 0;
            reservedWords["KEY"] = 0;
            reservedWords["LANGUAGE"] = 0;
            reservedWords["LARGE"] = 0;
            reservedWords["LAST"] = 0;
            reservedWords["LATERAL"] = 0;
            reservedWords["LEADING"] = 0;
            reservedWords["LEAVE"] = 0;
            reservedWords["LEFT"] = 0;
            reservedWords["LEVEL"] = 0;
            reservedWords["LIKE"] = 0;
            reservedWords["LOCAL"] = 0;
            reservedWords["LOCALTIME"] = 0;
            reservedWords["LOCALTIMESTAMP"] = 0;
            reservedWords["LOCATOR"] = 0;
            reservedWords["LOOP"] = 0;
            reservedWords["LOWER"] = 0;
            reservedWords["MAP"] = 0;
            reservedWords["MATCH"] = 0;
            reservedWords["MAX"] = 0;
            reservedWords["MEMBER"] = 0;
            reservedWords["MERGE"] = 0;
            reservedWords["METHOD"] = 0;
            reservedWords["MIN"] = 0;
            reservedWords["MINUTE"] = 0;
            reservedWords["MODIFIES"] = 0;
            reservedWords["MODULE"] = 0;
            reservedWords["MONTH"] = 0;
            reservedWords["MULTISET"] = 0;
            reservedWords["NAMES"] = 0;
            reservedWords["NATIONAL"] = 0;
            reservedWords["NATURAL"] = 0;
            reservedWords["NCHAR"] = 0;
            reservedWords["NCLOB"] = 0;
            reservedWords["NEW"] = 0;
            reservedWords["NEXT"] = 0;
            reservedWords["NO"] = 0;
            reservedWords["NONE"] = 0;
            reservedWords["NOT"] = 0;
            reservedWords["NULL"] = 0;
            reservedWords["NULLIF"] = 0;
            reservedWords["NUMERIC"] = 0;
            reservedWords["OBJECT"] = 0;
            reservedWords["OCTET_LENGTH"] = 0;
            reservedWords["OF"] = 0;
            reservedWords["OLD"] = 0;
            reservedWords["ON"] = 0;
            reservedWords["ONLY"] = 0;
            reservedWords["OPEN"] = 0;
            reservedWords["OPTION"] = 0;
            reservedWords["OR"] = 0;
            reservedWords["ORDER"] = 0;
            reservedWords["ORDINALITY"] = 0;
            reservedWords["OUT"] = 0;
            reservedWords["OUTER"] = 0;
            reservedWords["OUTPUT"] = 0;
            reservedWords["OVER"] = 0;
            reservedWords["OVERLAPS"] = 0;
            reservedWords["PAD"] = 0;
            reservedWords["PARAMETER"] = 0;
            reservedWords["PARTIAL"] = 0;
            reservedWords["PARTITION"] = 0;
            reservedWords["PATH"] = 0;
            reservedWords["POSITION"] = 0;
            reservedWords["PRECISION"] = 0;
            reservedWords["PREPARE"] = 0;
            reservedWords["PRESERVE"] = 0;
            reservedWords["PRIMARY"] = 0;
            reservedWords["PRIOR"] = 0;
            reservedWords["PRIVILEGES"] = 0;
            reservedWords["PROCEDURE"] = 0;
            reservedWords["PUBLIC"] = 0;
            reservedWords["RANGE"] = 0;
            reservedWords["READ"] = 0;
            reservedWords["READS"] = 0;
            reservedWords["REAL"] = 0;
            reservedWords["RECURSIVE"] = 0;
            reservedWords["REF"] = 0;
            reservedWords["REFERENCES"] = 0;
            reservedWords["REFERENCING"] = 0;
            reservedWords["RELATIVE"] = 0;
            reservedWords["RELEASE"] = 0;
            reservedWords["REPEAT"] = 0;
            reservedWords["RESIGNAL"] = 0;
            reservedWords["RESTRICT"] = 0;
            reservedWords["RESULT"] = 0;
            reservedWords["RETURN"] = 0;
            reservedWords["RETURNS"] = 0;
            reservedWords["REVOKE"] = 0;
            reservedWords["RIGHT"] = 0;
            reservedWords["ROLE"] = 0;
            reservedWords["ROLLBACK"] = 0;
            reservedWords["ROLLUP"] = 0;
            reservedWords["ROUTINE"] = 0;
            reservedWords["ROW"] = 0;
            reservedWords["ROWS"] = 0;
            reservedWords["SAVEPOINT"] = 0;
            reservedWords["SCHEMA"] = 0;
            reservedWords["SCOPE"] = 0;
            reservedWords["SCROLL"] = 0;
            reservedWords["SEARCH"] = 0;
            reservedWords["SECOND"] = 0;
            reservedWords["SECTION"] = 0;
            reservedWords["SELECT"] = 0;
            reservedWords["SENSITIVE"] = 0;
            reservedWords["SESSION"] = 0;
            reservedWords["SESSION_USER"] = 0;
            reservedWords["SET"] = 0;
            reservedWords["SETS"] = 0;
            reservedWords["SIGNAL"] = 0;
            reservedWords["SIMILAR"] = 0;
            reservedWords["SIZE"] = 0;
            reservedWords["SMALLINT"] = 0;
            reservedWords["SOME"] = 0;
            reservedWords["SPACE"] = 0;
            reservedWords["SPECIFIC"] = 0;
            reservedWords["SPECIFICTYPE"] = 0;
            reservedWords["SQL"] = 0;
            reservedWords["SQLCODE"] = 0;
            reservedWords["SQLERROR"] = 0;
            reservedWords["SQLEXCEPTION"] = 0;
            reservedWords["SQLSTATE"] = 0;
            reservedWords["SQLWARNING"] = 0;
            reservedWords["START"] = 0;
            reservedWords["STATE"] = 0;
            reservedWords["STATIC"] = 0;
            reservedWords["SUBMULTISET"] = 0;
            reservedWords["SUBSTRING"] = 0;
            reservedWords["SUM"] = 0;
            reservedWords["SYMMETRIC"] = 0;
            reservedWords["SYSTEM"] = 0;
            reservedWords["SYSTEM_USER"] = 0;
            reservedWords["TABLE"] = 0;
            reservedWords["TABLESAMPLE"] = 0;
            reservedWords["TEMPORARY"] = 0;
            reservedWords["THEN"] = 0;
            reservedWords["TIME"] = 0;
            reservedWords["TIMESTAMP"] = 0;
            reservedWords["TIMEZONE_HOUR"] = 0;
            reservedWords["TIMEZONE_MINUTE"] = 0;
            reservedWords["TO"] = 0;
            reservedWords["TRAILING"] = 0;
            reservedWords["TRANSACTION"] = 0;
            reservedWords["TRANSLATE"] = 0;
            reservedWords["TRANSLATION"] = 0;
            reservedWords["TREAT"] = 0;
            reservedWords["TRIGGER"] = 0;
            reservedWords["TRIM"] = 0;
            reservedWords["TRUE"] = 0;
            reservedWords["UNDER"] = 0;
            reservedWords["UNDO"] = 0;
            reservedWords["UNION"] = 0;
            reservedWords["UNIQUE"] = 0;
            reservedWords["UNKNOWN"] = 0;
            reservedWords["UNNEST"] = 0;
            reservedWords["UNTIL"] = 0;
            reservedWords["UPDATE"] = 0;
            reservedWords["UPPER"] = 0;
            reservedWords["USAGE"] = 0;
            reservedWords["USER"] = 0;
            reservedWords["USING"] = 0;
            reservedWords["VALUE"] = 0;
            reservedWords["VALUES"] = 0;
            reservedWords["VARCHAR"] = 0;
            reservedWords["VARYING"] = 0;
            reservedWords["VIEW"] = 0;
            reservedWords["WHEN"] = 0;
            reservedWords["WHENEVER"] = 0;
            reservedWords["WHERE"] = 0;
            reservedWords["WHILE"] = 0;
            reservedWords["WINDOW"] = 0;
            reservedWords["WITH"] = 0;
            reservedWords["WITHIN"] = 0;
            reservedWords["WITHOUT"] = 0;
            reservedWords["WORK"] = 0;
            reservedWords["WRITE"] = 0;
            reservedWords["YEAR"] = 0;
            reservedWords["ZONE"] = 0;
        }
    }
}
