using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.SqlConnector.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using System.Data;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Sequences;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class FloorLayoutData : SqlServerDataProviderBase, IFloorLayoutData, ISequenceable
    {
        private void PopulateFloorLayout(IDataReader dr, FloorLayout layout)
        {
            layout.ID = (string)dr["FLOORLAYOUTID"];
            layout.Text = (string)dr["DESCRIPTION"];

            if(dr["FLOORLAYOUTDESIGN"] is DBNull)
            {
                layout.JSONDesignData = null;
            }
            else
            {
                byte[] bytes = (byte[])dr["FLOORLAYOUTDESIGN"];

                layout.JSONDesignData = Encoding.UTF8.GetString(bytes);
            }
        }

        public virtual FloorLayout Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select FLOORLAYOUTID, DESCRIPTION, FLOORLAYOUTDESIGN from LSHFLOORLAYOUT 
                                    where FLOORLAYOUTID = @id and DATAAREAID = @dataAreaID;";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                return Get<FloorLayout>(entry, cmd, id, PopulateFloorLayout, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "LSHFLOORLAYOUT", "DESCRIPTION", "FLOORLAYOUTID", "DESCRIPTION");
        }

        public virtual void Save(IConnectionManager entry, FloorLayout layout)
        {
            SqlServerStatement statement = new SqlServerStatement("LSHFLOORLAYOUT");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageDiningTableLayouts);

            layout.Validate();

            bool isNew = false;
            if (layout.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                layout.ID = Providers.NumberSequenceData.GenerateNumberFromSequence(entry, this);
            }

            if (isNew || !Exists(entry, layout.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("FLOORLAYOUTID", (string)layout.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("FLOORLAYOUTID", (string)layout.ID);
            }

            statement.AddField("DESCRIPTION", layout.Text);

            if (layout.JSONDesignData == null)
            {
                statement.AddField("FLOORLAYOUTDESIGN", DBNull.Value, SqlDbType.Binary);
            }
            else
            {
                statement.AddField("FLOORLAYOUTDESIGN", Encoding.UTF8.GetBytes(layout.JSONDesignData), SqlDbType.Binary);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "LSHFLOORLAYOUT", "FLOORLAYOUTID", id, BusinessObjects.Permission.ManageDiningTableLayouts);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<FloorLayout>(entry, "LSHFLOORLAYOUT", "FLOORLAYOUTID", id);
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "FLOORLAYOUT"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "LSHFLOORLAYOUT", "FLOORLAYOUTID", sequenceFormat, startingRecord, numberOfRecords);
        }

        /// <summary>
        /// If the sections get connected to hospitality type then return true here else false. 
        /// (For LS One this would be true, for LS First this would be false since LS First links graphical section to
        /// some AX section data structure)
        /// </summary>
        public bool SectionRelationIsHospitalityType
        {
            get
            {
                return true;
            }
        }

        private static void PopulateSectionRelationType(IDataReader reader, SectionRelationType sectionType)
        {
            // For LS One the ID is composed of : RestaurantID, Sequence, Salestype, for LS First the ID will probably be something else
            sectionType.ID = new RecordIdentifier((string)reader["RESTAURANTID"], new RecordIdentifier((int)reader["SEQUENCE"], (string)reader["SALESTYPE"]));
            sectionType.Text = (string)reader["DESCRIPTION"];

            sectionType.Columns = 3; // TODO This will need to come from the table
            sectionType.Rows = 4; // TODO This will need to come from the table
            
        }

        /// <summary>
        /// Returns list of hospitality types on LS One but on LS First it should be returning list of sections
        /// (This list should only contain ID and Description, stored in DataEntity)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        public virtual List<SectionRelationType> GetSectionRelationTypes(IConnectionManager entry)
        {
            // Here LS First would query some internal table they have that represents a section.
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"select h.RESTAURANTID, 
                             h.SALESTYPE, 
                             h.SEQUENCE, 
                             ISNULL(h.DESCRIPTION,'') as DESCRIPTION 
                      from HOSPITALITYTYPE h 
                      where h.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<SectionRelationType>(entry, cmd, CommandType.Text, PopulateSectionRelationType);
            }


            /*List<DataEntity> returnValue = new List<DataEntity>();
            Providers.HospitalityTypeData.GetHospitalityTypes(entry).ForEach(e => returnValue.Add(e));
            return returnValue;*/
        }

        /// <summary>
        /// Gets a description for a given section backoffice ID. (For LS One this would be Hospitality Type Description, for LS First its Section Description or empty string if there is no description that applies)
        /// 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sectionBackOfficeID">ID of the Backoffice entity</param>
        /// <returns></returns>
        public virtual string GetSectionRelationTypeDescription(IConnectionManager entry, RecordIdentifier sectionBackOfficeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"Select DESCRIPTION from HOSPITALITYTYPE 
                      where RESTAURANTID = @resturantID and SEQUENCE = @sequenceID and SALESTYPE = @salesType and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "resturantID", (string)sectionBackOfficeID[0]);
                MakeParam(cmd, "sequenceID", (int)sectionBackOfficeID[1], SqlDbType.Int);
                MakeParam(cmd, "salesType", (string)sectionBackOfficeID[2]);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                object returnValue = entry.Connection.ExecuteScalar(cmd);

                if(returnValue !=null && !(returnValue is DBNull))
                {
                    return (string)returnValue;
                }
                else
                {
                    return "";
                }
            }
        }
    }
}
