using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.TaxFree;
using LSOne.DataLayer.DataProviders.TaxFree;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TaxFree
{
    public class TouristData : SqlServerDataProviderBase, ITouristData
    {
        private string BaseSql
        {
            get
            {
                return "SELECT ID, " +
                       "ISNULL(NAME, '') AS NAME, " +
                       "ISNULL(NATIONALITY,'') AS NATIONALITY, " +
                       "ISNULL(EMAIL, '') AS EMAIL, " +
                       "ISNULL(PASSPORTNUMBER, '') AS PASSPORTNUMBER, " +
                       "ISNULL(PASSPORTISSUEDBY, '') AS PASSPORTISSUEDBY, " +
                       "ISNULL(PASSPORTISSUEDON, '1900-01-01') AS PASSPORTISSUEDON, " +
                       "ISNULL(ADDRESS, '') AS ADDRESS, " +
                       "ISNULL(STREET, '') AS STREET, " +
                       "ISNULL(ZIPCODE, '') AS ZIPCODE, " +
                       "ISNULL(CITY, '') AS CITY, " +
                       "ISNULL(STATE,'') AS STATE, " +
                       "ISNULL(COUNTY, '') AS COUNTY, " +
                       "ISNULL(COUNTRY, '') AS COUNTRY, " +
                       "ISNULL(FLIGHTNUMBER, '') AS FLIGHTNUMBER, " +
                       "ISNULL(ARRIVALEDATE, '1900-01-01') AS ARRIVALEDATE, " +
                       "ISNULL(DEPARTUREDATE, '1900-01-01') AS DEPARTUREDATE " +
                       "FROM TOURIST ";
            }
        }
        public virtual void Populate(IDataReader dr, Tourist item)
        {
            item.ID = (Guid) dr["ID"];
            item.Name = (string) dr["NAME"];
            item.Nationality = (string) dr["NATIONALITY"];
            item.Email = (string) dr["EMAIL"];
            item.PassportNumber = (string) dr["PASSPORTNUMBER"];
            item.PassportIssuedBy = (string) dr["PASSPORTISSUEDBY"];
            item.PassportIssuedOn = (DateTime) dr["PASSPORTISSUEDON"];
            item.Address = new Address();
            item.Address.Address1 = (string) dr["ADDRESS"];
            item.Address.Address2 = (string) dr["STREET"];
            item.Address.Zip = (string) dr["ZIPCODE"];
            item.Address.City = (string) dr["CITY"];
            item.Address.State = (string) dr["STATE"];
            item.Address.County = (string) dr["COUNTY"];
            item.Address.Country = (string) dr["COUNTRY"];
            item.FlightNumber = (string)dr["FLIGHTNUMBER"];
            item.ArrivalDate = AsDateTime(dr["ARRIVALEDATE"]);
            item.DepartureDate = AsDateTime(dr["DEPARTUREDATE"]);
        }

        public virtual Tourist Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                                  "WHERE ID = @id";
                MakeParam(cmd, "id", id.DBValue, id.DBType);
                return Get<Tourist>(entry, cmd, id, Populate, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual List<Tourist> GetByPassportID(IConnectionManager entry, RecordIdentifier id)
        {
            if (id == null || id.IsEmpty)
            {
                return null;
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                                  "WHERE PASSPORTNUMBER = @id";
                MakeParam(cmd, "id", id.DBValue, id.DBType);
                return Execute<Tourist>(entry, cmd, CommandType.Text, Populate);
            }
        }
        
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select ID FROM TOURIST WHERE ID = @id";

                MakeParam(cmd, "id", id.DBValue, id.DBType);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }
        }

        public virtual void Save(IConnectionManager entry, Tourist item)
        {
            ValidateSecurity(entry);
            var statement = entry.Connection.CreateStatement("TOURIST");
            bool isNew = false;
            if (item.ID == null || item.ID.IsEmpty)
            {
                isNew = true;
                item.ID = Guid.NewGuid();
            }
            if (isNew || !Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", item.ID.DBValue, item.ID.DBType);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ID", item.ID.DBValue, item.ID.DBType);
            }

            statement.AddField("NAME", item.Name);
            statement.AddField("NATIONALITY", item.Nationality);
            statement.AddField("EMAIL", item.Email);
            statement.AddField("PASSPORTNUMBER", item.PassportNumber);
            statement.AddField("PASSPORTISSUEDBY", item.PassportIssuedBy);
            statement.AddField("PASSPORTISSUEDON", ToDateTime(item.PassportIssuedOn), SqlDbType.Date);
            statement.AddField("ADDRESS", item.Address.Address1);
            statement.AddField("STREET", item.Address.Address2);
            statement.AddField("ZIPCODE", item.Address.Zip);
            statement.AddField("CITY", item.Address.City);
            statement.AddField("STATE", item.Address.State);
            statement.AddField("COUNTY", item.Address.County);
            statement.AddField("COUNTRY", (string)item.Address.Country);
            statement.AddField("FLIGHTNUMBER", item.FlightNumber);
            statement.AddField("ARRIVALEDATE", ToDateTime(item.ArrivalDate), SqlDbType.DateTime);
            statement.AddField("DEPARTUREDATE", ToDateTime(item.DepartureDate), SqlDbType.DateTime);

            Save(entry, item, statement);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            ValidateSecurity(entry);

            entry.Cache.DeleteEntityFromCache(typeof(Tourist), ID);

            var statement = entry.Connection.CreateStatement("TOURIST", StatementType.Delete);

            statement.AddCondition("ID", ID.DBValue, ID.DBType);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
