using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using LSRetail.StoreController.SharedDatabase;
using LSRetail.StoreController.SharedDatabase.DataProviders;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.SharedDatabase.Interfaces;
using LSRetail.Utilities.DataTypes;
using System.Runtime.Serialization.Formatters.Binary;
using LSRetailPosis.Transaction;

namespace LSRetail.StoreController.ReceiptBrowser.Datalayer
{
    internal class ReceiptData : DataProviderBase
    {
        private static void PopulateReceiptListItem(SqlDataReader dr, DataEntities.ReceiptListItem item)
        {
            item.ID = (string)dr["TRANSACTIONID"];
            item.Text = (string)dr["TRANSACTIONID"];
            
            item.ReceiptID = (string)dr["RECEIPTID"];
            item.TransactionDate = new Date(dr["TRANSDATE"]);

            item.StoreID = (string)dr["STORE"];
            item.TerminalID = (string)dr["TERMINAL"];
            item.EmployeeID = (string)dr["STAFF"];

            item.StoreDescription = (string)dr["StoreName"];
            item.TerminalDescription = (string)dr["TerminalName"];
            item.EmployeeDescription = (string)dr["StaffName"];
        }

        public static List<Datalayer.DataEntities.ReceiptListItem> Find(IConnectionManager entry, Date dateFrom,Date dateTo,string receiptID,RecordIdentifier employeeID, RecordIdentifier storeID, RecordIdentifier terminalID,string sort)
        {
            SqlCommand cmd = new SqlCommand();

            ValidateSecurity(entry);
            

            cmd.CommandText = "Select Top 501 t.TRANSACTIONID,ISNULL(t.RECEIPTID,'') as RECEIPTID,t.TRANSDATE,"+
                "ISNULL(t.STORE,'') as STORE, ISNULL(t.TERMINAL,'') as TERMINAL,ISNULL(t.STAFF,'') as STAFF," +
                "ISNULL(term.NAME,'') as TerminalName,ISNULL(store.NAME,'') as StoreName,ISNULL(staff.Name,'') as StaffName " +
                "from RBOTRANSACTIONTABLE t " +
                "left outer join RBOTERMINALTABLE term on t.TERMINAL = term.TerminalID and term.DATAAREAID = t.DATAAREAID " +
                "left outer join RBOSTORETABLE store on t.STORE = store.STOREID and store.DATAAREAID = t.DATAAREAID " +
                "left outer join RBOSTAFFTABLE staff on t.STAFF = staff.STAFFID and staff.DATAAREAID = staff.DATAAREAID " +
                "where t.DATAAREAID = @dataAreaId and EntryStatus = 0 and (Type = 2 or Type = 3)";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

            if (receiptID != "")
            {
                cmd.CommandText += " and t.RECEIPTID like @receiptID";
                MakeParam(cmd, "receiptID", "%" + receiptID + "%");
            }

            if (dateFrom != Date.Empty)
            {
                cmd.CommandText += " and t.TRANSDATE >= @dateFrom";
                MakeParam(cmd, "dateFrom", dateFrom.DateTime.Date,SqlDbType.DateTime);
            }

            if (dateTo != Date.Empty)
            {
                cmd.CommandText += " and t.TRANSDATE <= @dateTo";
                MakeParam(cmd, "dateTo", dateTo.DateTime.Date, SqlDbType.DateTime);
            }

            if (employeeID != RecordIdentifier.Empty && employeeID != "")
            {
                cmd.CommandText += " and t.STAFF = @staffID";
                MakeParam(cmd, "staffID",(string)employeeID);
            }

            if (storeID != RecordIdentifier.Empty && storeID != "")
            {
                cmd.CommandText += " and t.STORE = @storeID";
                MakeParam(cmd, "storeID", (string)storeID);
            }

            if (terminalID != RecordIdentifier.Empty && terminalID != "")
            {
                cmd.CommandText += " and t.TERMINAL = @terminalID";
                MakeParam(cmd, "terminalID", (string)terminalID);
            }
    
           
            cmd.CommandText += (" order by " + sort.Replace("x.","t."));

            return Execute<DataEntities.ReceiptListItem>(entry, cmd, CommandType.Text, PopulateReceiptListItem);
        }
    }
}
