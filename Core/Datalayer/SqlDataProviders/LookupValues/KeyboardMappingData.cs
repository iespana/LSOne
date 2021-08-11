using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.LookupValues
{
    public class KeyboardMappingData : SqlServerDataProviderBase, IKeyboardMappingData
    {
        private static void PopulateKeyboardMapping(IDataReader dr, KeyboardMapping mapping)
        {
            mapping.ID = new RecordIdentifier((string)dr["KEYBOARDMAPPINGID"], (int)dr["ASCIIVALUE"]);
            mapping.Action = (int)dr["ACTION"];
            mapping.ActionProperty = (string)dr["ACTIONPROPERTY"];
        }

        public virtual List<KeyboardMapping> GetMappings(IConnectionManager entry, RecordIdentifier profileID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT KEYBOARDMAPPINGID,[ASCIIVALUE],ISNULL([ACTION],0) as ACTION,ISNULL([ACTIONPROPERTY],'') as ACTIONPROPERTY  
                                    FROM POSISKEYBOARDMAPPINGTRANS WHERE DATAAREAID = @dataAreaID and  KEYBOARDMAPPINGID = @keyboardmappingID";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "keyboardmappingID", (string)profileID);

                return Execute<KeyboardMapping>(entry, cmd, CommandType.Text, PopulateKeyboardMapping);
            }
        }

        public virtual Dictionary<int,KeyboardMapping> GetMappingDictionary(IConnectionManager entry, RecordIdentifier profileID)
        {
            var result = new Dictionary<int, KeyboardMapping>();
            var mappings = GetMappings(entry, profileID);

            foreach (var mapping in mappings)
            {
                result.Add(mapping.ASCIIValue, mapping);
            }

            return result;
        }
    }
}
