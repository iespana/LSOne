using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Infocodes
{
    public class InfocodeData : SqlServerDataProviderBase, IInfocodeData
    {
        private static string BaseSelectString
        {
            get
            {
                return
                    "SELECT rit.INFOCODEID," +
                    "ISNULL(DESCRIPTION,'') as DESCRIPTION," +
                    "ISNULL(PROMPT,'') as PROMPT," +
                    "ISNULL(ONCEPERTRANSACTION, 0) as ONCEPERTRANSACTION," +
                    "ISNULL(VALUEISAMOUNTORQUANTITY, 0) as VALUEISAMOUNTORQUANTITY," +
                    "ISNULL(PRINTPROMPTONRECEIPT, 0) as PRINTPROMPTONRECEIPT," +
                    "ISNULL(PRINTINPUTONRECEIPT, 0) as PRINTINPUTONRECEIPT," +
                    "ISNULL(PRINTINPUTNAMEONRECEIPT, 0) as PRINTINPUTNAMEONRECEIPT," +
                    "ISNULL(INPUTTYPE, 0) as INPUTTYPE," +
                    "ISNULL(MINIMUMVALUE, 0) as MINIMUMVALUE," +
                    "ISNULL(MAXIMUMVALUE, 0) as MAXIMUMVALUE," +
                    "ISNULL(MINIMUMLENGTH, 0) as MINIMUMLENGTH," +
                    "ISNULL(MAXIMUMLENGTH, 0) as MAXIMUMLENGTH," +
                    "ISNULL(rit.INPUTREQUIRED, 0) as INPUTREQUIRED," +
                    "ISNULL(LINKEDINFOCODEID, '') as LINKEDINFOCODEID," +
                    "ISNULL(RANDOMFACTOR, 0) as RANDOMFACTOR," +
                    "ISNULL(RANDOMCOUNTER, 0) as RANDOMCOUNTER," +
                    "ISNULL(ADDITIONALCHECK, 0) as ADDITIONALCHECK," +
                    "ISNULL(DISPLAYOPTION, 0) as DISPLAYOPTION," +
                    "ISNULL(LINKITEMLINESTOTRIGGERLINE, 0) as LINKITEMLINESTOTRIGGERLINE," +
                    "ISNULL(MULTIPLESELECTION, 0) as MULTIPLESELECTION," +
                    "ISNULL(rit.TRIGGERING, 0) as TRIGGERING," +
                    "ISNULL(MINSELECTION, 0) as MINSELECTION," +
                    "ISNULL(MAXSELECTION, 0) as MAXSELECTION," +
                    "ISNULL(CREATEINFOCODETRANSENTRIES, 0) as CREATEINFOCODETRANSENTRIES," +
                    "ISNULL(EXPLANATORYHEADERTEXT, '') as EXPLANATORYHEADERTEXT," +
                    "ISNULL(OKPRESSEDACTION, 0) as OKPRESSEDACTION," +
                    "ISNULL(rit.USAGECATEGORY, 0) as USAGECATEGORY " +
                    "FROM RBOINFOCODETABLE rit ";
            }
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOINFOCODETABLE", "DESCRIPTION", "INFOCODEID", "DESCRIPTION");
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry, TriggeringEnum triggering)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT t.INFOCODEID, ISNULL(t.DESCRIPTION, '') AS DESCRIPTION 
                                    FROM RBOINFOCODETABLE t  
                                    WHERE TRIGGERING = @TRIGGERING
                                    AND DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "TRIGGERING", (int)triggering, SqlDbType.TinyInt);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "INFOCODEID");
            }
        }

        public virtual List<DataEntity> GetListWithoutTriggerFunctions(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select t.INFOCODEID,ISNULL(t.DESCRIPTION, '')AS DESCRIPTION "+
                " from RBOINFOCODETABLE t " +
                " where Exists(Select 'x' from RBOINFORMATIONSUBCODETABLE s where s.INFOCODEID = t.INFOCODEID and s.DATAAREAID = t.DATAAREAID and s.TRIGGERFUNCTION = 0)  "+
                " or not Exists(Select 'x' from RBOINFORMATIONSUBCODETABLE s where s.INFOCODEID = t.INFOCODEID and s.DATAAREAID = t.DATAAREAID)" ;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "INFOCODEID");
            }
        }

        private static string ResolveSort(InfocodeSorting sort, bool backwards)
        {
            switch (sort)
            {
                case InfocodeSorting.InfocodeID:
                    return backwards ? "INFOCODEID DESC" : "INFOCODEID ASC";

                case InfocodeSorting.InfocodeDescription:
                    return backwards ? "DESCRIPTION DESC" : "DESCRIPTION ASC";
            }

            return "";
        }

        private static void PopulateInfocode(IDataReader dr, Infocode infocode)
        {
            infocode.ID = (string)dr["INFOCODEID"];
            infocode.Text = (string)dr["DESCRIPTION"];          
            //infocode.InfocodeId = (string)dr["INFOCODEID"];
            //infocode.Description = (string)dr["DESCRIPTION"];
            infocode.Prompt = (string)dr["PROMPT"];
            infocode.OncePerTransaction = ((byte)dr["ONCEPERTRANSACTION"] != 0);
            infocode.ValueIsAmountOrQuantity = ((byte)dr["VALUEISAMOUNTORQUANTITY"] != 0);
            infocode.PrintPromptOnReceipt = ((byte)dr["PRINTPROMPTONRECEIPT"] != 0);
            infocode.PrintInputOnReceipt = ((byte)dr["PRINTINPUTONRECEIPT"] != 0);
            infocode.PrintInputNameOnReceipt = ((byte)dr["PRINTINPUTNAMEONRECEIPT"] != 0);
            infocode.InputType = (InputTypesEnum)dr["INPUTTYPE"];
            infocode.MinimumValue = (decimal)dr["MINIMUMVALUE"];
            infocode.MaximumValue = (decimal)dr["MAXIMUMVALUE"];
            infocode.MinimumLength = (int)dr["MINIMUMLENGTH"];
            infocode.MaximumLength = (int)dr["MAXIMUMLENGTH"];
            infocode.InputRequired = ((byte)dr["INPUTREQUIRED"] != 0);
            infocode.LinkedInfocodeId = new RecordIdentifier((string)dr["LINKEDINFOCODEID"]);
            infocode.RandomFactor = (decimal)dr["RANDOMFACTOR"];
            infocode.RandomCounter = (decimal)dr["RANDOMCOUNTER"];
            infocode.AdditionalCheck = ((int)dr["ADDITIONALCHECK"] != 0);
            infocode.DisplayOption = ((DisplayOptions)dr["DISPLAYOPTION"]);
            infocode.LinkItemLinesToTriggerLine = ((byte)dr["LINKITEMLINESTOTRIGGERLINE"] != 0);
            infocode.MultipleSelection = ((byte)dr["MULTIPLESELECTION"] != 0);
            infocode.Triggering = (TriggeringEnum)Convert.ToInt32(dr["TRIGGERING"]);
            infocode.MinSelection = (int)dr["MINSELECTION"];
            infocode.MaxSelection = (int)dr["MAXSELECTION"];
            infocode.CreateInfocodeTransEntries = ((byte)dr["CREATEINFOCODETRANSENTRIES"] != 0);
            infocode.ExplanatoryHeaderText = (string)dr["EXPLANATORYHEADERTEXT"];
            infocode.OkPressedAction = (OKPressedActions)dr["OKPRESSEDACTION"];
            infocode.UsageCategory = (UsageCategoriesEnum)dr["USAGECATEGORY"];
        }
      
        public virtual List<Infocode> GetInfocodes(IConnectionManager entry, UsageCategoriesEnum[] usageCategories, RefTableEnum refTable)
        {
            return GetInfocodes(entry, usageCategories, true, new InputTypesEnum[] { }, true, null, refTable);
        }

        public virtual List<Infocode> GetInfocodes(IConnectionManager entry, UsageCategoriesEnum[] usageCategories, bool includeCategories, RecordIdentifier refRelation, RefTableEnum refTable)
        {
            return GetInfocodes(entry, usageCategories, true, new InputTypesEnum[] { }, includeCategories, refRelation, refTable);
        }

        public virtual List<Infocode> GetInfocodes(IConnectionManager entry, InputTypesEnum[] inputTypes, InfocodeSorting sortBy, bool sortBackwards, RefTableEnum refTable)
        {
            return GetInfocodes(entry, new UsageCategoriesEnum[] { }, true, inputTypes, true, sortBy, sortBackwards, null, refTable);
        }

        public virtual List<Infocode> GetInfocodes(IConnectionManager entry, InputTypesEnum[] inputTypes, bool includeInputTypes, RefTableEnum refTable)
        {
            return GetInfocodes(entry, new UsageCategoriesEnum[] { }, true, inputTypes, includeInputTypes, null, refTable);
        }

        /// <summary>
        /// Gets a list of infocodes  including / excluding the given range of input types and sorted by the given sort enum.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="inputTypes">Range of input types</param>
        /// <param name="includeInputTypes">Including(true) or excluding(false) the range</param>
        /// <param name="sortBy">Sort by infocode or description</param>
        /// <param name="refTable"></param>
        /// <param name="sortBackwards"></param>
        /// <returns></returns>
        public List<Infocode> GetInfocodes(IConnectionManager entry,
            InputTypesEnum[] inputTypes,
            bool includeInputTypes,
            InfocodeSorting sortBy, 
            bool sortBackwards,
            RefTableEnum refTable)
        {
            //return GetInfocodes(entry, new UsageCategories[] { }, true, inputTypes, includeInputTypes, null);
            return GetInfocodes(entry, 
                new UsageCategoriesEnum[] { },
                true, inputTypes, 
                includeInputTypes, 
                sortBy, sortBackwards, 
                null,
                refTable);
        }

        public virtual List<Infocode> GetInfocodes(IConnectionManager entry, UsageCategoriesEnum[] usageCategories, InputTypesEnum[] inputTypes, RefTableEnum refTable)
        {
            return GetInfocodes(entry, usageCategories, true, inputTypes, true,null, refTable);
        }

        public virtual List<Infocode> GetInfocodes(IConnectionManager entry, UsageCategoriesEnum[] usageCategories, InputTypesEnum[] inputTypes, InfocodeSorting sortBy, bool sortBackwards, RefTableEnum refTable)
        {
            return GetInfocodes(entry, usageCategories, true, inputTypes, true,sortBy, sortBackwards, null, refTable);
        }

        public  List<Infocode> GetInfocodes(
            IConnectionManager entry
            , UsageCategoriesEnum[] usageCategories
            , bool includeCategories
            , InputTypesEnum[] inputTypes
            , bool includeInputTypes
            , RecordIdentifier refRelation
            , RefTableEnum refTable)
        {
            return GetInfocodes(entry, usageCategories, includeCategories, inputTypes, includeInputTypes, InfocodeSorting.InfocodeDescription, false, refRelation, refTable);
        }
        /// <summary>
        /// Gets a list of infocodes including / excluding the given range of usage categories and including / excluding the given range of input types.
        /// </summary>
        /// <param name="entry">The database object</param>
        /// <param name="usageCategories">Range of usage categories</param>
        /// <param name="includeCategories">Including(true) or excluding(false) the range</param>
        /// <param name="inputTypes">Range of input types</param>
        /// <param name="includeInputTypes">Including(true) or excluding(false) the range</param>
        /// <param name="sortBy">Sort by infocode or describtion</param>
        /// <param name="sortBackwards">Sort backwards(true) or not</param>
        /// <param name="refRelation"></param>
        /// <param name="refTable"></param>
        /// <returns>A list of all infocodes for the given criteria</returns>
        private static List<Infocode> GetInfocodes(
            IConnectionManager entry
            , UsageCategoriesEnum[] usageCategories
            , bool includeCategories
            , InputTypesEnum[] inputTypes
            , bool includeInputTypes
            , InfocodeSorting sortBy
            , bool sortBackwards
            , RecordIdentifier refRelation
            , RefTableEnum refTable)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                if (usageCategories.Length > 0)
                    cmd.CommandText += " AND";

                for (int i = 0; i < usageCategories.Length; i++)
                {
                    if (includeCategories)
                    {
                        cmd.CommandText += " USAGECATEGORY = @usageCategories" + i;
                        // If we have not reached the last entry
                        if (i != usageCategories.Length - 1)
                            cmd.CommandText += " or";
                    }
                    else
                    {
                        cmd.CommandText += " USAGECATEGORY != @usageCategories" + i;
                        // If we have not reached the last entry
                        if (i != usageCategories.Length - 1)
                            cmd.CommandText += " and";

                    }
                    MakeParam(cmd, "usageCategories" + i, (int) usageCategories[i], SqlDbType.Int);
                }

                if (inputTypes.Length > 0)
                    cmd.CommandText += " AND";

                for (int i = 0; i < inputTypes.Length; i++)
                {
                    if (includeInputTypes)
                    {
                        cmd.CommandText += " INPUTTYPE = @inputType" + i;
                        // If we have not reached the last entry
                        if (i != inputTypes.Length - 1)
                            cmd.CommandText += " or";
                    }
                    else
                    {
                        cmd.CommandText += " INPUTTYPE != @inputType" + i;
                        // If we have not reached the last entry
                        if (i != inputTypes.Length - 1)
                            cmd.CommandText += " and";

                    }
                    MakeParam(cmd, "inputType" + i, (int) inputTypes[i], SqlDbType.Int);
                }

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                if (refRelation != null)
                {
                    cmd.CommandText += " and not exists (select 'x' from RBOINFOCODETABLESPECIFIC sp ";
                    cmd.CommandText +=
                        " where sp.REFRELATION = @REFRELATION  and sp.REFRELATION2 = @REFRELATION2 and sp.REFRELATION3 = @REFRELATION3 and sp.INFOCODEID = rit.INFOCODEID and sp.DATAAREAID = rit.DATAAREAID ";

                    MakeParam(cmd, "REFRELATION", (string) refRelation[0]);
                    MakeParam(cmd, "REFRELATION2", (string) refRelation[1]);
                    MakeParam(cmd, "REFRELATION3", (string) refRelation[2]);

                    if (refTable != RefTableEnum.All)
                    {
                        cmd.CommandText += "and sp.REFTABLEID = @REFTABLEID";

                        MakeParam(cmd, "REFTABLEID", (int) refTable, SqlDbType.Int);
                    }

                    cmd.CommandText += ")";
                }

                cmd.CommandText += " order by " + ResolveSort(sortBy, sortBackwards);

                return Execute<Infocode>(entry, cmd, CommandType.Text, PopulateInfocode);
            }
        }

        public virtual List<DataEntity> GetTaxGroupInfocodes(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select DISTINCT I.INFOCODEID as INFOCODEID, I.DESCRIPTION as DESCRIPTION " +
                    "from RBOINFOCODETABLE I " +
                    "join RBOINFORMATIONSUBCODETABLE S on S.TRIGGERFUNCTION = 6 and S.INFOCODEID = I.INFOCODEID and I.DATAAREAID = S.DATAAREAID " +
                    "where S.DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "INFOCODEID");
            }
        }

        /// <summary>
        /// Checks if an infocode is being used.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocode">The infocode being checked</param>
        /// <param name="operations">List of operations to check for</param>
        /// <returns>True if infocode is in use</returns>
        public virtual bool InfocodeInUseByOperation(IConnectionManager entry, Infocode infocode, List<string> operations)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select count(*) " +
                    "from POSFUNCTIONALITYPROFILE FP " +
                    "join RBOINFOCODETABLE IC on ";

                // Set join (Operations)
                for(int i = 0; i < operations.Count; i++)
                {
                    cmd.CommandText += "IC.INFOCODEID = FP." + operations[i] + " or ";
                }
                // Cut off the last OR
                cmd.CommandText = cmd.CommandText.Remove(cmd.CommandText.Length - 3);

                cmd.CommandText += "where ";
                cmd.CommandText += "(IC.INFOCODEID = @infocodeId) ";
                cmd.CommandText += "and (FP.DATAAREAID = @dataAreaId) ";
                MakeParam(cmd, "infocodeId", (string) infocode.ID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return (int)entry.Connection.ExecuteScalar(cmd) > 0;
            }
        }

        /// <summary>
        /// Gets an infocode with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeId">The ID of the infocode to get</param>
        /// <returns>The infocode with the given ID</returns>
        public virtual Infocode Get(IConnectionManager entry, RecordIdentifier infocodeId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and INFOCODEID = @infocodeId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "infocodeId", (string) infocodeId);

                var result = Execute<Infocode>(entry, cmd, CommandType.Text, PopulateInfocode);
                return result.Count > 0 ? result[0] : null;
            }
        }        
                
        /// <summary>
        /// Checks if an infocode with the given Id exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the infocode to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOINFOCODETABLE", "INFOCODEID", id);
        }

        /// <summary>
        /// Deletes the infocode with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the infocode to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBOINFOCODETABLE", "INFOCODEID", id, BusinessObjects.Permission.InfocodeEdit);
            DeleteRecord(entry, "RBOINFORMATIONSUBCODETABLE", "INFOCODEID", id, BusinessObjects.Permission.InfocodeEdit);
        }
        
        /// <summary>
        /// Saves an Infocode object to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocode">The Infocode to be saved</param>
        public virtual void Save(IConnectionManager entry, Infocode infocode)
        {
            var statement = new SqlServerStatement("RBOINFOCODETABLE");

            ValidateSecurity(entry, BusinessObjects.Permission.InfocodeEdit);

            var isNew = false;
            if (infocode.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                infocode.ID = DataProviderFactory.Instance.GenerateNumber<IInfocodeData, Infocode>(entry); 
            }

            if (isNew || !Exists(entry, infocode.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("INFOCODEID", (string)infocode.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("INFOCODEID", (string)infocode.ID);
            }

            statement.AddField("DESCRIPTION", infocode.Text);
            statement.AddField("PROMPT", infocode.Prompt);
            statement.AddField("ONCEPERTRANSACTION", infocode.OncePerTransaction ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VALUEISAMOUNTORQUANTITY", infocode.ValueIsAmountOrQuantity ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PRINTPROMPTONRECEIPT", infocode.PrintPromptOnReceipt ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PRINTINPUTONRECEIPT", infocode.PrintInputOnReceipt ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PRINTINPUTNAMEONRECEIPT", infocode.PrintInputNameOnReceipt ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("INPUTTYPE", (int)infocode.InputType, SqlDbType.Int);
            statement.AddField("MINIMUMVALUE", infocode.MinimumValue, SqlDbType.Decimal);
            statement.AddField("MAXIMUMVALUE", infocode.MaximumValue, SqlDbType.Decimal);
            statement.AddField("MINIMUMLENGTH", infocode.MinimumLength, SqlDbType.Int);
            statement.AddField("MAXIMUMLENGTH", infocode.MaximumLength, SqlDbType.Int);
            statement.AddField("INPUTREQUIRED", infocode.InputRequired ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LINKEDINFOCODEID", infocode.LinkedInfocodeId.PrimaryID.ToString());
            statement.AddField("RANDOMFACTOR", infocode.RandomFactor, SqlDbType.Decimal);
            statement.AddField("RANDOMCOUNTER", infocode.RandomCounter, SqlDbType.Decimal);
            statement.AddField("ADDITIONALCHECK", infocode.AdditionalCheck ? 1 : 0, SqlDbType.Int);
            statement.AddField("DISPLAYOPTION", (int)infocode.DisplayOption, SqlDbType.Int);
            statement.AddField("LINKITEMLINESTOTRIGGERLINE", infocode.LinkItemLinesToTriggerLine ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("MULTIPLESELECTION", infocode.MultipleSelection ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TRIGGERING", (byte)infocode.Triggering, SqlDbType.TinyInt);
            statement.AddField("MINSELECTION", infocode.MinSelection, SqlDbType.Int);
            statement.AddField("MAXSELECTION", infocode.MaxSelection, SqlDbType.Int);
            statement.AddField("CREATEINFOCODETRANSENTRIES", infocode.CreateInfocodeTransEntries ? 1 : 0, SqlDbType.Int);
            statement.AddField("EXPLANATORYHEADERTEXT", infocode.ExplanatoryHeaderText);
            statement.AddField("USAGECATEGORY", (int)infocode.UsageCategory, SqlDbType.Int);
            statement.AddField("OKPRESSEDACTION", (int)infocode.OkPressedAction, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
        
        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "INFOCODE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOINFOCODETABLE", "INFOCODEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
