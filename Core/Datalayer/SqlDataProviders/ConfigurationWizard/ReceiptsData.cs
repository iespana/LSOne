using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.DataProviders.ConfigurationWizard;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ConfigurationWizard
{
    /// <summary>
    /// Data prover class for Receipts.
    /// </summary>
    public class ReceiptsData : SqlServerDataProviderBase, IReceiptsData
    {
        /// <summary>
        /// Data populator for GetRecieptLayoutList function.
        /// </summary>
        /// <param name="entry">entry to database</param>
        /// <param name="dr">data reader</param>
        /// <param name="receiptLayout">object of Receipts entity</param>
        /// <param name="includeReportFormatting">parameter</param>
        private static void PopulateReceiptLayout(IConnectionManager entry, IDataReader dr, Receipts receiptLayout, object includeReportFormatting)
        {
            receiptLayout.LineNum = (int)dr["LINENUM"];
            receiptLayout.FormLayoutID = (string)dr["FORMLAYOUTID"];
            receiptLayout.Image = (dr["IMAGE"] == DBNull.Value) ? null : (byte[])dr["IMAGE"];
        }

        /// <summary>
        /// Get receipt layout list from database based on provided templateId.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns></returns>
        public virtual List<Receipts> GetReceiptLayoutList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(ID, '') as ID,
                                    ISNULL(LINENUM, 0) as LINENUM,
                                    ISNULL(FORMLAYOUTID, '') as FORMLAYOUTID,
                                    IMAGE 
                                    FROM WIZARDTEMPLATEFORMLAYOUTS
                                    WHERE ID = @ID
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                return Execute<Receipts>(entry, cmd, CommandType.Text, null, PopulateReceiptLayout);
            }
        }

        /// <summary>
        /// Get selected layout from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">layoutId</param>
        /// <returns>Form Layout</returns>
        public virtual Form Get(IConnectionManager entry, RecordIdentifier id)
        {
            var cmd = entry.Connection.CreateCommand();

            ValidateSecurity(entry);

            cmd.CommandText =
                   @"select ID, 
                            ISNULL(TITLE,'') as TITLE,
                            ISNULL(DESCRIPTION,'') as DESCRIPTION,
                            ISNULL(HEADERXML,'') as HEADERXML,
                            ISNULL(LINESXML,'') as LINESXML,
                            ISNULL(FOOTERXML,'') as FOOTERXML,
                            ISNULL(PRINTASSLIP,0) as PRINTASSLIP,
                            ISNULL(LINECOUNTPRPAGE,0) as LINECOUNTPRPAGE,
                            ISNULL(USEWINDOWSPRINTER,'') as USEWINDOWSPRINTER,                            
                            ISNULL(WINDOWSPRINTERCONFIGURATIONID,'') as WINDOWSPRINTERCONFIGURATIONID,
                            ISNULL(PRINTBEHAVIOUR,0) as PRINTBEHAVIOUR 
                    from POSISFORMLAYOUT 
                    where DATAAREAID = @DATAAREAID and ID = @ID";

            MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
            MakeParam(cmd, "ID", Convert.ToInt32(id.StringValue), SqlDbType.Int);

            return Execute<Form>(entry, cmd, CommandType.Text, null, PopulateForm)[0];
        }

        /// <summary>
        /// Data populator for Get function
        /// </summary>
        /// <param name="entry">entry to database</param>
        /// <param name="dr">data reader</param>
        /// <param name="form">object of Form entity</param>
        /// <param name="obj">parameter</param>
        private static void PopulateForm(IConnectionManager entry, IDataReader dr, Form form, object obj)
        {
            form.ID = Convert.ToString(dr["ID"]);
            form.Text = (string)dr["DESCRIPTION"];

            form.HeaderXml = (string)dr["HEADERXML"];
            form.LineXml = (string)dr["LINESXML"];
            form.FooterXml = (string)dr["FOOTERXML"];
            form.PrintAsSlip = ((byte)dr["PRINTASSLIP"] != 0);
            form.LineCountPerPage = (int)dr["LINECOUNTPRPAGE"];
            form.UseWindowsPrinter = ((byte)dr["USEWINDOWSPRINTER"] != 0);            
            form.WindowsPrinterConfigurationID = (string)dr["WINDOWSPRINTERCONFIGURATIONID"];
            form.PrintBehavior = (PrintBehaviors)(int)dr["PRINTBEHAVIOUR"];
        }

        /// <summary>
        /// Save all selected layouts into database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="layoutList">list of layouts</param>
        public virtual void SaveLayouts(IConnectionManager entry, List<Receipts> layoutList)
        {
            Delete(entry, layoutList.Where(item => item.ID != RecordIdentifier.Empty).FirstOrDefault().ID);

            foreach (var layout in layoutList)
            {
                if (layout.ID != RecordIdentifier.Empty)
                {
                    var statement = new SqlServerStatement("WIZARDTEMPLATEFORMLAYOUTS")
                        {
                            StatementType = StatementType.Insert
                        };

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                    statement.AddKey("ID", (string)layout.ID);

                    statement.AddField("FORMLAYOUTID", layout.FormLayoutID.StringValue, SqlDbType.NVarChar);

                    statement.AddField("LINENUM", layout.LineNum, SqlDbType.Int);

                    statement.AddField("IMAGE", layout.Image, SqlDbType.Image);

                    entry.Connection.ExecuteStatement(statement);
                }
            }
        }

        /// <summary>
        /// Delete a entity declaration from given table with a given id.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <param name="table">Table name</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATEFORMLAYOUTS")
        {
            DeleteRecord(entry, table, "ID", id, BusinessObjects.Permission.FormsEdit);
        }

        /// <summary>
        /// Data populator of GetSelectedReceipts function
        /// </summary>
        /// <param name="dr">data reader</param>
        /// <param name="form">object of Form entity</param>
        private static void PopulateForm(IDataReader dr, Form form)
        {
            form.ID = (string)dr["ID"];
            form.Text = (string)dr["DESCRIPTION"];
            form.HeaderXml = (string)dr["HEADERXML"];
            form.LineXml = (string)dr["LINESXML"];
            form.FooterXml = (string)dr["FOOTERXML"];
            form.PrintAsSlip = ((byte)dr["PRINTASSLIP"] != 0);
            form.LineCountPerPage = (int)dr["LINECOUNTPRPAGE"];
            form.UseWindowsPrinter = ((byte)dr["USEWINDOWSPRINTER"] != 0);            
            form.WindowsPrinterConfigurationID = (string)dr["WINDOWSPRINTERCONFIGURATIONID"];
            form.PrintBehavior = (PrintBehaviors)(int)dr["PRINTBEHAVIOUR"];
            form.PromptQuestion = (byte)dr["PROMPTQUESTION"] != 0;
            form.PromptText = (string)dr["PROMPTTEXT"];
            form.UpperCase = (byte)dr["UPPERCASE"] != 0;
        }

        /// <summary>
        /// Get selected list of form layouts from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">form layout id list</param>
        /// <returns>List of form layouts</returns>
        public virtual List<Form> GetSelectedReceipts(IConnectionManager entry, List<RecordIdentifier> idList)
        {
            string ids = "";
            foreach (var id in idList)
            {
                ids += "'" + id.StringValue + "',";
            }
            ids = ids.Remove(ids.LastIndexOf(','));

            using (var cmd = entry.Connection.CreateCommand())
            {

                ValidateSecurity(entry);

                cmd.CommandText =
                    @"SELECT ID, 
                             ISNULL(TITLE,'') as TITLE,
                             ISNULL(DESCRIPTION,'') as DESCRIPTION, 
                             ISNULL(UPPERCASE, 0) AS UPPERCASE, 
                             ISNULL(HEADERXML,'') as HEADERXML, 
                             ISNULL(LINESXML,'') as LINESXML,
                             ISNULL(FOOTERXML,'') as FOOTERXML,
                             ISNULL(PRINTASSLIP,0) as PRINTASSLIP,
                             ISNULL(LINECOUNTPRPAGE,0) as LINECOUNTPRPAGE,
                             ISNULL(USEWINDOWSPRINTER,'') as USEWINDOWSPRINTER,                                
                             ISNULL(WINDOWSPRINTERCONFIGURATIONID, '') as WINDOWSPRINTERCONFIGURATIONID,
                             ISNULL(PROMPTQUESTION, 0) as PROMPTQUESTION, 
                             ISNULL(PROMPTTEXT, '') AS PROMPTTEXT,
                             ISNULL(PRINTBEHAVIOUR,0) as PRINTBEHAVIOUR
                         FROM POSISFORMLAYOUT " +
                    "WHERE DATAAREAID = @DATAAREAID and ID in (" + ids + ")";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                
                List<Form> result = Execute<Form>(entry, cmd, CommandType.Text, PopulateForm);

                return result.Count > 0 ? result : null;
            }
        }
    }
}
