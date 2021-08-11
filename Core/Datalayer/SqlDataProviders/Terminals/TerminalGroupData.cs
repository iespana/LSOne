using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Terminals;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Terminals
{
   public class TerminalGroupData : SqlServerDataProviderBase, ITerminalGroupData
   {
       public virtual List<DataEntity> GetList(IConnectionManager entry)
       {
           return GetList<DataEntity>(entry, "RBOTERMINALGROUP", "DESCRIPTION", "ID", "DESCRIPTION");
       }

       public virtual List<TerminalGroup> GetListForTerminalGroup(IConnectionManager entry, bool sortAscending, TerminalGroup.SortEnum sortEnum)
       {
           using (var cmd = entry.Connection.CreateCommand())
           {
               ValidateSecurity(entry);

               cmd.CommandText = @"Select ID, DESCRIPTION from RBOTERMINALGROUP 
                WHERE DATAAREAID = @dataAreaID " + ResolveSort(sortEnum, sortAscending);
                
               MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
               
               return Execute<TerminalGroup>(entry, cmd, CommandType.Text, null, PopulateTerminalGroup);
           }
       }

       private static string ResolveSort(TerminalGroup.SortEnum sortEnum, bool sortBackwards)
       {
           string sortString = "";

           switch (sortEnum)
           {
               case TerminalGroup.SortEnum.Description:
                   sortString = "order by DESCRIPTION ASC ";
                   break;
               case TerminalGroup.SortEnum.ID:
                   sortString = "order by ID ASC ";
                   break;
           }
           if (sortBackwards)
           {
               sortString = sortString.Replace("ASC", "DESC");
           }

           return sortString;
       }

       private static void PopulateTerminalGroup(IConnectionManager entry, IDataReader dr, TerminalGroup terminalGroup, object param)
       {
           terminalGroup.ID = (string)dr["ID"];
           terminalGroup.Text = (string)dr["DESCRIPTION"];
       }

       public TerminalGroup Get(
           IConnectionManager entry,
           RecordIdentifier groupId)
       {
           using (var cmd = entry.Connection.CreateCommand())
           {
               ValidateSecurity(entry);

               cmd.CommandText = @"Select ID, DESCRIPTION from RBOTERMINALGROUP 
                                    where ID = @ID and DATAAREAID = @dataAreaID ";

               MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
               MakeParam(cmd, "ID", (string)groupId);

               var terminalGroups = Execute<TerminalGroup>(entry, cmd, CommandType.Text, null, PopulateTerminalGroup);
               return terminalGroups.Count > 0 ? terminalGroups[0] : null;
           }
       }

       public virtual void Save(IConnectionManager entry, TerminalGroup terminalGroup)
       {
           ValidateSecurity(entry, BusinessObjects.Permission.TerminalEdit);

           terminalGroup.Validate();

           var statement = new SqlServerStatement("RBOTERMINALGROUP");

           if (terminalGroup.ID == RecordIdentifier.Empty)
           {
               terminalGroup.ID = DataProviderFactory.Instance.GenerateNumber<ITerminalGroupData, TerminalGroup>(entry);
           }

           if (!Exists(entry, terminalGroup.ID))
           {
               statement.StatementType = StatementType.Insert;

               statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
               statement.AddKey("ID", (string)terminalGroup.ID);
           }
           else
           {
               statement.StatementType = StatementType.Update;

               statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
               statement.AddCondition("ID", (string)terminalGroup.ID);
           }

           statement.AddField("DESCRIPTION", terminalGroup.Text);

           entry.Connection.ExecuteStatement(statement);
       }

       public virtual void Delete(IConnectionManager entry, RecordIdentifier terminalId)
       {
           DeleteRecord(
               entry,
               "RBOTERMINALGROUP",
               "ID",
               terminalId,
               LSOne.DataLayer.BusinessObjects.Permission.TerminalEdit);
       }

       public virtual bool Exists(IConnectionManager entry, RecordIdentifier terminalGroupId)
       {
           return RecordExists(
               entry,
               "RBOTERMINALGROUP",
               "ID",
               terminalGroupId);
       }

       public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
       {
           return Exists(entry, id);
       }

       public RecordIdentifier SequenceID
       {
           get { return "TERMINALSGROUPS"; }
       }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOTERMINALGROUP", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }
    }
}