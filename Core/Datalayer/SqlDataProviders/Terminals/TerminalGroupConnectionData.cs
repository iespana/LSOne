using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.DataProviders.Terminals;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Terminals
{
    public class TerminalGroupConnectionData : SqlServerDataProviderBase, ITerminalGroupConnectionData
    {
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOTERMINALTABLE", "NAME", "TERMINALID","NAME");
        }

        private static void PopulateTerminalDetailedGroup(IConnectionManager entry, IDataReader dr, TerminalGroupConnection terminalGroup, object param)
        {
            terminalGroup.TerminalGroupId = (string)dr["TERMINALGROUPID"];
            terminalGroup.TerminalId = (string)dr["TERMINALID"];
            terminalGroup.TerminalDescription = (string)dr["TERMINALDESCRIPTION"];
            terminalGroup.StoreId = (string)dr["STOREID"];
            terminalGroup.Location = (string)dr["LOCATION"];
        }

        private static string ResolveSort(TerminalGroupConnection.SortEnum sortEnum, bool sortBackwards)
        {
            string sortString = "";

            switch (sortEnum)
            {
                case TerminalGroupConnection.SortEnum.TerminalId:
                    sortString = "order by TG.TERMINALID ASC ";
                    break;
                case TerminalGroupConnection.SortEnum.Location:
                    sortString = "order by LOCATION ASC ";
                    break;
                case TerminalGroupConnection.SortEnum.TerminalDescription:
                    sortString = "order by TERMINALDESCRIPTION ASC ";
                    break;
            }
            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void Populate(IDataReader dr, DataEntity entity)
        {
            RecordIdentifier id = new RecordIdentifier((string)dr["TERMINALID"], (string)dr["STOREID"]);
            entity.ID = id;
            entity.Text = (string)dr["NAME"];
        }

        public virtual List<DataEntity> GetTerminalsForDropDown(IConnectionManager entry, RecordIdentifier groupId)
        {            
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"select t.TERMINALID, t.STOREID, ISNULL(t.NAME + ' - ' + rst.NAME, '') AS NAME  from RBOTERMINALTABLE t 
                    left outer join RBOSTORETABLE RST on t.STOREID = RST.STOREID and t.DATAAREAID = RST.DATAAREAID AND t.DATAAREAID = @dataAreaID 
                    WHERE 
                    t.TERMINALID not in 
                    (select terminalid from RBOTERMINALGROUPCONNECTION where TERMINALID = t.terminalid and STOREID = t.STOREID and TERMINALGROUPID = @groupID AND DATAAREAID =  @dataAreaID) and
                    t.STOREID not in
                    (select STOREID from RBOTERMINALGROUPCONNECTION WHERE TERMINALID = t.TERMINALID and STOREID = t.STOREID and TERMINALGROUPID = @groupID AND DATAAREAID =  @dataAreaID)";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "groupID", (string)groupId);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, Populate);
            }
        }

        public virtual List<TerminalGroupConnection> GetTerminalsList(IConnectionManager entry, RecordIdentifier groupId, bool sortAscending = true , TerminalGroupConnection.SortEnum sortEnum = TerminalGroupConnection.SortEnum.ID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"Select
                                    TG.TERMINALGROUPID
                                    ,TG.TERMINALID 
                                    ,TG.STOREID
                                    ,ISNULL (RBT.NAME, '') AS TERMINALDESCRIPTION  
                                    ,ISNULL (RST.NAME, '') AS LOCATION                                                     
                                    from RBOTERMINALGROUPCONNECTION TG 
                                    left outer join RBOTERMINALTABLE RBT on TG.TERMINALID = RBT.TERMINALID and TG.STOREID = RBT.STOREID and TG.DATAAREAID = RBT.DATAAREAID 
                                    left outer join RBOSTORETABLE RST on RBT.STOREID = RST.STOREID and RBT.DATAAREAID = RST.DATAAREAID
                                   
                                    WHERE TG.DATAAREAID = @dataAreaID AND TG.TERMINALGROUPID = @GROUPID " +
                                    ResolveSort(sortEnum, sortAscending);

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "GROUPID", (string)groupId);

                return Execute<TerminalGroupConnection>(entry, cmd, CommandType.Text, null, PopulateTerminalDetailedGroup);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier terminalId, RecordIdentifier groupId, RecordIdentifier storeId)
        {
            DeleteRecord(
                entry,
                "RBOTERMINALGROUPCONNECTION",
                new []{"TERMINALGROUPID", "TERMINALID", "STOREID"},
                new RecordIdentifier(groupId,terminalId, storeId),
                LSOne.DataLayer.BusinessObjects.Permission.TerminalEdit);
        }

        public virtual void DeleteGroup(IConnectionManager entry, RecordIdentifier terminalId)
        {
            DeleteRecord(
                entry,
                "RBOTERMINALGROUPCONNECTION",
                "TERMINALGROUPID",
                terminalId,
                LSOne.DataLayer.BusinessObjects.Permission.TerminalEdit);
        }

        public virtual void Save(IConnectionManager entry, TerminalGroupConnection terminalGroup)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.TerminalEdit);

            var statement = new SqlServerStatement("RBOTERMINALGROUPCONNECTION") {StatementType = StatementType.Insert};

            statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddKey("TERMINALID", (string) terminalGroup.TerminalId);
            statement.AddKey("STOREID", (string)terminalGroup.StoreId);
            statement.AddKey("TERMINALGROUPID", (string)terminalGroup.TerminalGroupId);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
