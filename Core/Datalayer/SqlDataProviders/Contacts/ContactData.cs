using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Contacts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Contacts
{
    public class ContactData : SqlServerDataProviderBase, IContactData
    {
        private static string ResolveSort(IConnectionManager entry,ContactSorting sort,bool backwards)
        {
            var sortString = "";

            switch (sort)
            {
                case ContactSorting.Name:
                    sortString = (entry.Settings.NameFormat == NameFormat.FirstNameFirst) ?
                        "FirstName ASC,LastName ASC, MiddleName ASC, NamePrefix ASC, NameSuffix ASC, NAME ASC " :
                        "LastName ASC,FirstName ASC, MiddleName ASC, NamePrefix ASC, NameSuffix ASC, NAME ASC ";

                    break;

                case ContactSorting.CompanyName:
                    sortString = "COMPANYNAME ASC";
                    break;

                case ContactSorting.Phone:
                    sortString = "PHONE ASC";
                    break;

                case ContactSorting.Address:
                    sortString = "STREET ASC,ADDRESS ASC,ZIPCODE ASC,CITY ASC,STATE ASC,COUNTRY ASC";
                    break;
            }

            if (backwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        private static void PopulateContactInfo(IConnectionManager entry, IDataReader dr, Contact contact, object includeReportFormatting)
        {
            var address = contact.Address;
            var name = contact.Name;

            contact.ID = (string)dr["CONTACTID"];
            contact.OwnerID = (string)dr["OWNERID"];
            contact.OwnerType = (ContactRelationTypeEnum)dr["OWNERTYPE"];
            contact.ContactType = (TypeOfContactEnum)dr["CONTACTTYPE"];
            contact.CompanyName = (string)dr["COMPANYNAME"];
            name.First = (string)dr["FirstName"];
            name.Middle = (string)dr["MiddleName"];
            name.Last = (string)dr["LastName"];
            name.Prefix = (string)dr["NamePrefix"];
            name.Suffix = (string)dr["NameSuffix"];
            contact.Text = (string)dr["NAME"];

            address.Address1 = (string)dr["STREET"];
            address.Address2 = (string)dr["ADDRESS"];
            address.AddressFormat = (dr["ADDRESSFORMAT"] == DBNull.Value) ? entry.Settings.AddressFormat : (Address.AddressFormatEnum)((int)(dr["ADDRESSFORMAT"]));
            address.Zip = (string)dr["ZIPCODE"];
            address.City = (string)dr["CITY"];
            address.State = (string)dr["STATE"];
            address.Country = (string)dr["COUNTRY"];

            contact.Phone = (string)dr["PHONE"];
            contact.Phone2 = (string)dr["PHONE2"];
            contact.Fax = (string)dr["FAX"];
            contact.Email = (string)dr["EMAIL"];
        }

        /// <summary>
        /// Gets a contact with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="contactID">The ID of the contact to get</param>
        /// <returns>A contact with a given ID</returns>
        public virtual Contact Get(IConnectionManager entry, RecordIdentifier contactID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT CONTACTID,OWNERID,OWNERTYPE,CONTACTTYPE," +
                    "COMPANYNAME,FirstName,MiddleName,LastName,NamePrefix,NameSuffix, ISNULL(NAME, '') AS NAME, " +
                    "ISNULL(ADDRESS,'') as ADDRESS,ISNULL(STREET,'') as STREET, " +
                    "ISNULL(ZIPCODE,'') as ZIPCODE, ISNULL(CITY,'') as CITY, ISNULL(COUNTY,'') as COUNTY," +
                    "ISNULL(STATE,'') as STATE,ISNULL(COUNTRY,'') as COUNTRY,ISNULL(PHONE,'') as PHONE," +
                    "ISNULL(PHONE2,'') as PHONE2,ISNULL(FAX,'') as FAX," +
                    "ISNULL(EMAIL,'') as EMAIL, " +
                    "c.ADDRESSFORMAT " +
                    "from CONTACTTABLE c " +
                    "WHERE c.DATAAREAID = @dataareaid AND CONTACTID = @contactID";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "contactID", (string)contactID);

                var result = Execute<Contact>(entry, cmd, CommandType.Text, false, PopulateContactInfo);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        public virtual List<Contact> GetList(IConnectionManager entry, ContactRelationTypeEnum ownerType, RecordIdentifier ownerID,ContactSorting sortBy,bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT CONTACTID, OWNERID, OWNERTYPE, CONTACTTYPE," +
                    "COMPANYNAME, FirstName, MiddleName, LastName, NamePrefix, NameSuffix, ISNULL(NAME, '') AS NAME, " +
                    "ISNULL(ADDRESS,'') as ADDRESS,ISNULL(STREET,'') as STREET, " +
                    "ISNULL(ZIPCODE,'') as ZIPCODE, ISNULL(CITY,'') as CITY, ISNULL(COUNTY,'') as COUNTY," +
                    "ISNULL(STATE,'') as STATE, ISNULL(COUNTRY,'') as COUNTRY, ISNULL(PHONE,'') as PHONE," +
                    "ISNULL(PHONE2,'') as PHONE2, ISNULL(FAX,'') as FAX," +
                    "ISNULL(EMAIL,'') as EMAIL, " +
                    "c.ADDRESSFORMAT " +
                    "from CONTACTTABLE c " +
                    "WHERE c.DATAAREAID = @dataareaid and OWNERID = @ownerID and OWNERTYPE = @ownerType " +
                    "order by " + ResolveSort(entry, sortBy, sortBackwards);

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "ownerID", (string)ownerID);
                MakeParam(cmd, "ownerType", (int)ownerType);

                return Execute<Contact>(entry, cmd, CommandType.Text, false, PopulateContactInfo);
            }
        }

        [Obsolete("Use overridden Delete method", true)]
        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Deletes a contact with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="contactID">The ID of the contact to delete</param>
        /// <param name="ownerType">Owner type of the contact to be deleted</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier contactID, ContactRelationTypeEnum ownerType)
        {
            string permission;

            switch (ownerType)
            {
                case ContactRelationTypeEnum.Vendor:
                    permission = Permission.VendorEdit;
                    break;
                case ContactRelationTypeEnum.Customer:
                    permission = Permission.CustomerEdit;
                    break;
                default:
                    throw new PermissionException();
            }

            DeleteRecord(entry, "CONTACTTABLE", "CONTACTID", contactID, permission);
        }

        /// <summary>
        /// Checks if a contact by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="contactID">ID of the contact to check for</param>
        /// <returns>True if the contact exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier contactID)
        {
            return RecordExists(entry, "CONTACTTABLE", "CONTACTID", contactID);
        }

        /// <summary>
        /// Saves a contact to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="contact">The vendor to be saved</param>
        public virtual void Save(IConnectionManager entry, Contact contact)
        {
            var statement = new SqlServerStatement("CONTACTTABLE");

            if (contact.OwnerType == ContactRelationTypeEnum.Vendor)
            {
                ValidateSecurity(entry, BusinessObjects.Permission.VendorEdit);
            }
            if (contact.OwnerType == ContactRelationTypeEnum.Customer)
            {
                ValidateSecurity(entry, BusinessObjects.Permission.CustomerEdit);
            }

            if (contact.ID == "" || contact.ID == RecordIdentifier.Empty)
            {
                contact.ID = DataProviderFactory.Instance.GenerateNumber<IContactData,Contact>(entry);
            }

            contact.Validate();

            if (!Exists(entry, contact.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("CONTACTID", (string)contact.ID);

            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CONTACTID", (string)contact.ID);
            }

            statement.AddField("OWNERID", (string)contact.OwnerID);
            statement.AddField("OWNERTYPE", (int)contact.OwnerType,SqlDbType.Int);
            statement.AddField("CONTACTTYPE", (int)contact.ContactType,SqlDbType.Int);
            statement.AddField("COMPANYNAME", contact.CompanyName);
            statement.AddField("FirstName",contact.Name.First);
            statement.AddField("MiddleName",contact.Name.Middle);
            statement.AddField("LastName",contact.Name.Last);
            statement.AddField("NamePrefix",contact.Name.Prefix);
            statement.AddField("NameSuffix",contact.Name.Suffix);
            statement.AddField("ADDRESS", contact.Address.Address2);
            statement.AddField("STREET", contact.Address.Address1);
            statement.AddField("CITY", contact.Address.City);
            statement.AddField("ZIPCODE", contact.Address.Zip);
            statement.AddField("STATE", contact.Address.State);
            statement.AddField("COUNTRY", (string)contact.Address.Country);
            statement.AddField("PHONE", contact.Phone);
            statement.AddField("PHONE2", contact.Phone2);
            statement.AddField("FAX", contact.Fax);
            statement.AddField("EMAIL", contact.Email);
            statement.AddField("ADDRESSFORMAT", (int)contact.Address.AddressFormat, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry,id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "Contacts"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "CONTACTTABLE", "CONTACTID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
