using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.SqlConnector.DataProviders;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector;
using System.Data;
using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;

namespace LSOne.DataLayer.SqlDataProviders.StoreManagement
{
    public class RegionData : SqlServerDataProviderBase, IRegionData
    {
        public RecordIdentifier SequenceID
        {
            get
            {
                return "REGION";
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            DeleteRecord(entry, "REGION", "ID", ID, Permission.StoreEdit, false);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "REGION", "ID", ID, false);
        }

        public virtual Region Get(IConnectionManager entry, RecordIdentifier ID)
        {
            return GetDataEntity<Region>(entry, "REGION", "DESCRIPTION", "ID", ID, false);
        }

        public virtual List<Region> GetList(IConnectionManager entry, Region.SortEnum sortBy, bool sortDescending)
        {
            return GetList<Region>(entry, "REGION", "DESCRIPTION", "ID", GetRegionSort(sortBy, sortDescending), false);
        }

        public List<DataEntity> GetStoresByRegion(IConnectionManager entry, RecordIdentifier regionID, Region.SortEnum sortBy, bool sortDescending)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT STOREID, NAME FROM RBOSTORETABLE WHERE REGIONID = @regionID " + GetStoreSort(sortBy, sortDescending);
                MakeParam(cmd, "regionID", regionID);
                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "STOREID");
            }
        }

        public virtual void Save(IConnectionManager entry, Region item)
        {
            var statement = new SqlServerStatement("REGION");

            bool isNew = false;
            if (item.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                item.ID = DataProviderFactory.Instance.GenerateNumber<IRegionData, Region>(entry);
            }

            if (isNew || !Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("ID", (string)item.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (string)item.ID);
            }

            statement.AddField("DESCRIPTION", item.Text);
            Save(entry, item, statement);
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "REGION", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        private string GetRegionSort(Region.SortEnum sortBy, bool sortDescending)
        {
            string sort = "";
            switch(sortBy)
            {
                case Region.SortEnum.ID:
                    sort += "ID ";
                    break;
                case Region.SortEnum.Description:
                    sort += "DESCRIPTION ";
                    break;
            }
            sort += sortDescending ? "DESC" : "ASC";
            return sort;
        }

        private string GetStoreSort(Region.SortEnum sortBy, bool sortDescending)
        {
            string sort = "ORDER BY ";
            switch (sortBy)
            {
                case Region.SortEnum.ID:
                    sort += "STOREID ";
                    break;
                case Region.SortEnum.Description:
                    sort += "NAME ";
                    break;
            }
            sort += sortDescending ? "DESC" : "ASC";
            return sort;
        }
    }
}
