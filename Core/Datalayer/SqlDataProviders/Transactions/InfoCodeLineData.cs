using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;

namespace LSOne.DataLayer.SqlDataProviders.Transactions
{
    public class InfoCodeLineData : SqlServerDataProviderBase, IInfoCodeLineData
    {
        private static void BasePopulate(IDataReader dr, InfoCodeLineItem infoCode)
        {
            infoCode.InfocodeId = (string)dr["InfoCodeId"];
            infoCode.Description = (string)dr["Description"];
            infoCode.Prompt = (string)dr["Prompt"];
            infoCode.MinimumValue = (decimal)dr["MinimumValue"];
            infoCode.MaximumValue = (decimal)dr["MaximumValue"];
            infoCode.MinimumLength = (int)dr["MinimumLength"];
            infoCode.MaximumLength = (int)dr["MaximumLength"];
            infoCode.MaxSelection = (int)dr["MAXSELECTION"];
            infoCode.MinSelection = (int)dr["MINSELECTION"];
            infoCode.InputRequired = ((byte)dr["InputRequired"] != 0);
            infoCode.ValueIsAmountOrQuantity = ((byte)dr["ValueIsAmountOrQuantity"] != 0);
            infoCode.PrintPromptOnReceipt = ((byte)dr["PrintPromptOnReceipt"] != 0);
            infoCode.PrintInputOnReceipt = ((byte)dr["PrintInputOnReceipt"] != 0);
            infoCode.PrintInputNameOnReceipt = ((byte)dr["PrintInputNameOnReceipt"] != 0);
            infoCode.StandardValueIsOne = ((byte)dr["Std1InValue"] != 0);
            infoCode.LinkedInfoCodeId = (string)dr["LinkedInfoCodeId"];
            infoCode.RandomFactor = (decimal)dr["RandomFactor"];
            infoCode.InputType = (InfoCodeLineItem.InputTypes)(int)dr["InputType"];
            infoCode.AdditionalCheck = (int)dr["AdditionalCheck"];
            infoCode.Triggered = (InfoCodeLineItem.Triggering)(int)dr["Triggering"];
            infoCode.LinkItemLinesToTriggerLine = ((int)dr["LinkItemLinesToTriggerLine"] != 0);
        }

        private static void PopulateRelational(IDataReader dr, InfoCodeLineItem infoCode)
        {
            BasePopulate(dr, infoCode);
            infoCode.OncePerTransaction = ((byte)dr["OncePerTransaction"] != 0);
            infoCode.InputRequriedType = (InfoCodeLineItem.InputRequriedTypes) (int)dr["WhenRequired"];
            infoCode.CreateTransactionEntries = ((int)dr["CreateInfoCodeTransEntries"] != 0);
            infoCode.UnitOfMeasure = (string)dr["UnitOfMeasure"];
            infoCode.SalesTypeFilter = (string)dr["SalesTypeFilter"];
        }

        private static void Populate(IDataReader dr, InfoCodeLineItem infoCode)
        {
            BasePopulate(dr, infoCode);
            infoCode.OncePerTransaction = ((byte)dr["OncePerTransaction"] != 0);
            infoCode.InputRequriedType = (InfoCodeLineItem.InputRequriedTypes)(byte)dr["InputRequired"];
            infoCode.UnitOfMeasure = ""; 
            infoCode.SalesTypeFilter = ""; 
        }

        public virtual List<InfoCodeLineItem> GetInfoCodes(IConnectionManager entry, string refRelation, string refRelation2, string refRelation3, InfoCodeLineItem.TableRefId refTableId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @" SELECT S.WhenRequired, I.InfoCodeId, I.Description, I.Prompt, I.OncePerTransaction, I.ValueIsAmountOrQuantity,I.ValueIsAmountOrQuantity,I.PrintPromptOnReceipt,
                                  I.PrintInputOnReceipt,I.PrintInputNameOnReceipt,I.InputType, I.MinimumValue,I.MaximumValue,I.MinimumLength,I.MaximumLength,
                                  I.Std1InValue,I.LinkedInfoCodeId,I.RandomFactor, COALESCE(S.UnitOfMeasure, '') UnitOfMeasure, COALESCE(S.SalesTypeFilter, '') SalesTypeFilter,
                                  COALESCE(I.CreateInfoCodeTransEntries, 1) CreateInfoCodeTransEntries, ISNULL(I.AdditionalCheck, 0) AS AdditionalCheck, 
                                  COALESCE(I.LinkItemLinesToTriggerLine, 0) LinkItemLinesToTriggerLine, 
                                  ISNULL(S.InputRequired, ISNULL(I.InputRequired, 0)) AS InputRequired,
                                  COALESCE(I.MINSELECTION, 0) AS MINSELECTION, COALESCE(I.MAXSELECTION, 0) AS MAXSELECTION,   
                                  Triggering = CASE COALESCE(S.Triggering, -1) WHEN -1 THEN COALESCE(I.Triggering, 0) ELSE S.TRIGGERING END 
                                  FROM dbo.RBOINFOCODETABLE I, dbo.RBOINFOCODETABLESPECIFIC S 
                                  WHERE S.InfocodeId = I.InfoCodeId 
                                  AND S.DATAAREAID = @DATAAREAID 
                                  AND S.RefRelation = @RefRelation 
                                  AND S.RefRelation2 = @RefRelation2 
                                  AND S.RefRelation3 = @RefRelation3 
                                  AND S.RefTableId = @RefTableId 
                                  ORDER BY S.SEQUENCE ASC ";
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "RefRelation", refRelation);
                MakeParam(cmd, "RefRelation2", refRelation2);
                MakeParam(cmd, "RefRelation3", refRelation3);
                MakeParam(cmd, "RefTableId", refTableId, SqlDbType.Int);
                return Execute<InfoCodeLineItem>(entry, cmd, CommandType.Text, PopulateRelational);
            }
        }

        public virtual  List<InfoCodeLineItem> GetInfoCodes(IConnectionManager entry, string infoCodeId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @" SELECT I.InfoCodeId, I.Description, I.Prompt, I.OncePerTransaction, I.ValueIsAmountOrQuantity, 
                                      I.PrintPromptOnReceipt,I.PrintInputOnReceipt,I.PrintInputNameOnReceipt, 
                                      I.InputType, I.MinimumValue,I.MaximumValue,I.MinimumLength,I.MaximumLength,I.InputRequired,I.Std1InValue, 
                                      I.LinkedInfoCodeId,I.RandomFactor, COALESCE(I.Triggering, 0) Triggering, COALESCE(I.AdditionalCheck, 0) AdditionalCheck, 
                                      COALESCE(I.LinkItemLinesToTriggerLine, 0) LinkItemLinesToTriggerLine,
                                      COALESCE(I.MINSELECTION, 0) AS MINSELECTION, COALESCE(I.MAXSELECTION, 0) AS MAXSELECTION
                                      FROM dbo.RBOINFOCODETABLE I 
                                      WHERE I.DATAAREAID = @DATAAREAID 
                                      AND I.InfocodeId = @InfoCodeId ";
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "InfoCodeId", infoCodeId);
                return Execute<InfoCodeLineItem>(entry, cmd, CommandType.Text, Populate);
            }
        }
    }
}
