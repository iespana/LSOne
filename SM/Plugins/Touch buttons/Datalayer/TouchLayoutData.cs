using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LSRetail.StoreController.SharedDatabase;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.SharedDatabase.DataProviders;
using LSRetail.StoreController.SharedDatabase.Interfaces;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.TouchButtons.Datalayer.DataEntities;
using System.Drawing;
using LSRetail.StoreController.BusinessObjects;


namespace LSRetail.StoreController.TouchButtons.Datalayer
{
    internal class TouchLayoutData : DataProviderBase, ISequenceable
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "LAYOUTID, " +
                    "ISNULL(NAME,'') as NAME, " +
                    "ISNULL(WIDTH, 0) as WIDTH, " +
                    "ISNULL(HEIGHT,0) as HEIGHT, " +
                    "ISNULL(BUTTONGRID1,'') as BUTTONGRID1, " +
                    "ISNULL(BUTTONGRID2,'') as BUTTONGRID2, " +
                    "ISNULL(BUTTONGRID3,'') as BUTTONGRID3, " +
                    "ISNULL(BUTTONGRID4,'') as BUTTONGRID4, " +
                    "ISNULL(BUTTONGRID5,'') as BUTTONGRID5, " +
                    "ISNULL(RECEIPTID,'') as RECEIPTID, " +
                    "ISNULL(TOTALID,'') as TOTALID, " +
                    "ISNULL(CUSTOMERLAYOUTID,'') as CUSTOMERLAYOUTID, " +
                    "ISNULL(LOGOPICTUREID,0) as LOGOPICTUREID, " +
                    "IMG_CUSTOMERLAYOUTXML, " +
                    "IMG_RECEIPTITEMSLAYOUTXML, " +
                    "IMG_RECEIPTPAYMENTLAYOUTXML, " +
                    "IMG_TOTALSLAYOUTXML, " +
                    "IMG_LAYOUTXML, " +
                    "ISNULL(CUSTOMERLAYOUTXML,'') as CUSTOMERLAYOUTXML, " +
                    "ISNULL(RECEIPTITEMSLAYOUTXML,'') as RECEIPTITEMSLAYOUTXML, " +
                    "ISNULL(RECEIPTPAYMENTLAYOUTXML,'') as RECEIPTPAYMENTLAYOUTXML, " +
                    "ISNULL(TOTALSLAYOUTXML,'') as TOTALSLAYOUTXML, " +
                    "ISNULL(LAYOUTXML,'') as LAYOUTXML, " +
                    "IMG_CASHCHANGERLAYOUTXML, " +
                    "ISNULL(CASHCHANGERLAYOUTXML,'') as CASHCHANGERLAYOUTXML " +
                    "from POSISTILLLAYOUT ";
            }
        }

        private static void PopulateTouchLayout(SqlDataReader dr, TouchLayout touchLayout)
        {
            touchLayout.ID = (string)dr["LAYOUTID"];
            touchLayout.Name = (string)dr["NAME"];
            touchLayout.Text = touchLayout.Name;
            touchLayout.Width = (int)dr["WIDTH"];
            touchLayout.Height = (int)dr["HEIGHT"];
            touchLayout.ButtonGrid1 = (string)dr["BUTTONGRID1"];
            touchLayout.ButtonGrid2 = (string)dr["BUTTONGRID2"];
            touchLayout.ButtonGrid3 = (string)dr["BUTTONGRID3"];
            touchLayout.ButtonGrid4 = (string)dr["BUTTONGRID4"];
            touchLayout.ButtonGrid5 = (string)dr["BUTTONGRID5"];
            touchLayout.ReceiptID = (string)dr["RECEIPTID"];
            touchLayout.TotalID = (string)dr["TOTALID"];
            touchLayout.CustomerLayoutID = (string)dr["CUSTOMERLAYOUTID"];
            touchLayout.LogoPictureID = (int)dr["LOGOPICTUREID"];
            touchLayout.ImgCustomerLayoutXML = dr["IMG_CUSTOMERLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_CUSTOMERLAYOUTXML"];
            touchLayout.ImgReceiptItemsLayoutXML = dr["IMG_RECEIPTITEMSLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_RECEIPTITEMSLAYOUTXML"]; 
            touchLayout.ImgReceiptPaymentLayoutXML = dr["IMG_RECEIPTPAYMENTLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_RECEIPTPAYMENTLAYOUTXML"];
            touchLayout.ImgTotalsLayoutXML = dr["IMG_TOTALSLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_TOTALSLAYOUTXML"]; 
            touchLayout.ImgLayoutXML = dr["IMG_LAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_LAYOUTXML"];
            touchLayout.CustomerLayoutXML = (string)dr["CUSTOMERLAYOUTXML"];
            touchLayout.ReceiptItemsLayoutXML = (string)dr["RECEIPTITEMSLAYOUTXML"];
            touchLayout.ReceiptPaymentLayoutXML = (string)dr["RECEIPTPAYMENTLAYOUTXML"];
            touchLayout.TotalsLayoutXML = (string)dr["TOTALSLAYOUTXML"];
            touchLayout.LayoutXML = (string)dr["LAYOUTXML"];
            touchLayout.ImgCashChangerLayoutXML = dr["IMG_CASHCHANGERLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_CASHCHANGERLAYOUTXML"]; 
            touchLayout.CashChangerLayoutXML = (string)dr["CASHCHANGERLAYOUTXML"];
        }

        public static List<DataEntity> GetList(IConnectionManager entry,string sort)
        {
            return GetList<DataEntity>(entry, "POSISTILLLAYOUT", "NAME", "LAYOUTID", sort);
        }

        private static void PopulateLayout(SqlDataReader dr, DataEntities.TouchLayout layout)
        {
            layout.ID = (string)dr["LAYOUTID"];
            layout.Text = (string)dr["NAME"];
        }

        public static DataEntities.TouchLayout Get(IConnectionManager entry, RecordIdentifier id)
        {
            SqlCommand cmd = new SqlCommand();

            ValidateSecurity(entry);

            //cmd.CommandText = "select LAYOUTID, ISNULL(NAME,'') as NAME from POSISTILLLAYOUT " +
            //       "where DataAreaId = @dataAreaId and LAYOUTID = @id";
            cmd.CommandText = 
                BaseSelectString +
                "where DATAAREAID = @dataAreaId and LAYOUTID = @id";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            MakeParam(cmd, "id", (string)id);

            return Execute<DataEntities.TouchLayout>(entry, cmd, CommandType.Text, PopulateTouchLayout)[0];
        }

        public static bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "POSISTILLLAYOUT", "LAYOUTID", id);
        }

        public static void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "POSISTILLLAYOUT", "LAYOUTID", id, Permission.TouchButtonLayoutEdit);

        }

        public static void CreateNewAndCopyFrom(IConnectionManager entry, DataEntities.TouchLayout layout, RecordIdentifier copyFromID)
        {
            //SqlDataReader dr = null;
            SqlCommand cmd = new SqlCommand();
            //Statement statement = null;

            ValidateSecurity(entry, Permission.TouchButtonLayoutEdit);

            // Get the layout containing the data to copy from
            TouchLayout layoutToCopy = Get(entry, copyFromID);

            // Get the buttonGrids that we need to copy. There are potentially 5 button grids that a layout can have
            ButtonGrid buttonGrid1ToCopy = Providers.ButtonGridData.Get(entry, layoutToCopy.ButtonGrid1);
            ButtonGrid buttonGrid2ToCopy = Providers.ButtonGridData.Get(entry, layoutToCopy.ButtonGrid2);
            ButtonGrid buttonGrid3ToCopy = Providers.ButtonGridData.Get(entry, layoutToCopy.ButtonGrid3);
            ButtonGrid buttonGrid4ToCopy = Providers.ButtonGridData.Get(entry, layoutToCopy.ButtonGrid4);
            ButtonGrid buttonGrid5ToCopy = Providers.ButtonGridData.Get(entry, layoutToCopy.ButtonGrid5);

            // Get all the buttons that belong to each button grid that we need to copy
            List<ButtonGridButtons> buttonGrid1Buttons = buttonGrid1ToCopy != null ? Providers.ButtonGridButtonsData.GetList(entry, buttonGrid1ToCopy.ID) : new List<ButtonGridButtons>();
            List<ButtonGridButtons> buttonGrid2Buttons = buttonGrid2ToCopy != null ? Providers.ButtonGridButtonsData.GetList(entry, buttonGrid2ToCopy.ID) : new List<ButtonGridButtons>();
            List<ButtonGridButtons> buttonGrid3Buttons = buttonGrid3ToCopy != null ? Providers.ButtonGridButtonsData.GetList(entry, buttonGrid3ToCopy.ID) : new List<ButtonGridButtons>();
            List<ButtonGridButtons> buttonGrid4Buttons = buttonGrid4ToCopy != null ? Providers.ButtonGridButtonsData.GetList(entry, buttonGrid4ToCopy.ID) : new List<ButtonGridButtons>();
            List<ButtonGridButtons> buttonGrid5Buttons = buttonGrid5ToCopy != null ? Providers.ButtonGridButtonsData.GetList(entry, buttonGrid5ToCopy.ID) : new List<ButtonGridButtons>();

            // Give new id's to the button grids and button grid buttons that we intend to copy
            CopyButtonGridAndButtons(entry, buttonGrid1ToCopy, buttonGrid1Buttons);
            CopyButtonGridAndButtons(entry, buttonGrid2ToCopy, buttonGrid2Buttons);
            CopyButtonGridAndButtons(entry, buttonGrid3ToCopy, buttonGrid3Buttons);
            CopyButtonGridAndButtons(entry, buttonGrid4ToCopy, buttonGrid4Buttons);
            CopyButtonGridAndButtons(entry, buttonGrid5ToCopy, buttonGrid5Buttons);

            // Now update the layoutToCopy with the new information
            layoutToCopy.ButtonGrid1 = buttonGrid1ToCopy != null ? (string)buttonGrid1ToCopy.ID : "";
            layoutToCopy.ButtonGrid2 = buttonGrid2ToCopy != null ? (string)buttonGrid2ToCopy.ID : "";
            layoutToCopy.ButtonGrid3 = buttonGrid3ToCopy != null ? (string)buttonGrid3ToCopy.ID : "";
            layoutToCopy.ButtonGrid4 = buttonGrid4ToCopy != null ? (string)buttonGrid4ToCopy.ID : "";
            layoutToCopy.ButtonGrid5 = buttonGrid5ToCopy != null ? (string)buttonGrid5ToCopy.ID : "";

            if (layout.ID == RecordIdentifier.Empty)
            {
                layout.ID = entry.GenerateNumberFromSequence(new TouchLayoutData());
            }

            // Finally save the new layout
            layoutToCopy.ID = layout.ID;
            layoutToCopy.Name = layout.Text;

            Save(entry, layoutToCopy);

        }

        private static void CopyButtonGridAndButtons(IConnectionManager entry, ButtonGrid buttonGridToCopy, List<ButtonGridButtons> buttonGridButtonsToCopy)
        {
            // Give new id's to the button grids and button grid buttons that we intend to copy
            if (buttonGridToCopy != null)
            {
                buttonGridToCopy.ID = PluginEntry.DataModel.GenerateNumberFromSequence(new ButtonGridData());
                int maxButtonGridButtonID = Providers.ButtonGridButtonsData.GetNextButtonID(entry);

                // Replace each buttonGridID field in buttonGrid1Buttons with the new button grid ID
                for (int i = 0; i < buttonGridButtonsToCopy.Count; i++)
                {
                    buttonGridButtonsToCopy[i].ButtonGridID = buttonGridToCopy.ID;
                    buttonGridButtonsToCopy[i].ID = maxButtonGridButtonID + i;
                    Providers.ButtonGridButtonsData.Save(entry, buttonGridButtonsToCopy[i]);
                }

                // Save the new button grid and button grid buttons
                Providers.ButtonGridData.Save(entry, buttonGridToCopy);
            }
        }

        private static void DeleteButtonGridAndButtons(IConnectionManager entry, ButtonGrid buttonGridToDelete)
        {
            if (buttonGridToDelete != null)
            {
                foreach (ButtonGridButtons buttons in Providers.ButtonGridButtonsData.GetList(entry, buttonGridToDelete.ID))
                {
                    Providers.ButtonGridButtonsData.Delete(entry, buttons.ID);
                }

                Providers.ButtonGridData.Delete(entry, buttonGridToDelete.ID);
            }
        }

        public static void SaveHeader(IConnectionManager entry, DataEntities.TouchLayout layout)
        {
            Statement statement = new Statement("POSISTILLLAYOUT");

            ValidateSecurity(entry, Permission.TouchButtonLayoutEdit);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("LAYOUTID", (string)layout.ID);

            statement.AddField("WIDTH", layout.Width, SqlDbType.Int);
            statement.AddField("HEIGHT", layout.Height, SqlDbType.Int);

            statement.AddField("NAME", layout.Text);        
        }

        public static void Save(IConnectionManager entry, DataEntities.TouchLayout layout)
        {
            bool isNew = false;

            Statement statement = new Statement("POSISTILLLAYOUT");
            SqlCommand cmd = new SqlCommand();

            ValidateSecurity(entry, Permission.TouchButtonLayoutEdit);

            if (layout.ID == RecordIdentifier.Empty)
            {
                layout.ID = entry.GenerateNumberFromSequence(new TouchLayoutData());
            }

            if (isNew || !Exists(entry, layout.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("LAYOUTID", (string)layout.ID);

                statement.AddField("WIDTH", 800, SqlDbType.Int);
                statement.AddField("HEIGHT", 600, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("LAYOUTID", (string)layout.ID);

                statement.AddField("WIDTH", layout.Width, SqlDbType.Int);
                statement.AddField("HEIGHT", layout.Height, SqlDbType.Int);

            }

            statement.AddField("NAME", layout.Text);            
            statement.AddField("BUTTONGRID1", (string)layout.ButtonGrid1);
            statement.AddField("BUTTONGRID2", (string)layout.ButtonGrid2);
            statement.AddField("BUTTONGRID3", (string)layout.ButtonGrid3);
            statement.AddField("BUTTONGRID4", (string)layout.ButtonGrid4);
            statement.AddField("BUTTONGRID5", (string)layout.ButtonGrid5);

            // The tillLayoutDesigner will not show the layout if these fields contain an empty string
            if (layout.ReceiptID != "") { statement.AddField("RECEIPTID", layout.ReceiptID); }
            if (layout.TotalID != "") { statement.AddField("TOTALID", layout.TotalID); }
            if (layout.CustomerLayoutID != "") { statement.AddField("CUSTOMERLAYOUTID", layout.CustomerLayoutID); }
            statement.AddField("LOGOPICTUREID", layout.LogoPictureID, SqlDbType.Int);

            if (layout.ImgCustomerLayoutXML != null) { statement.AddField("IMG_CUSTOMERLAYOUTXML", layout.ImgCustomerLayoutXML, SqlDbType.Image); }
            if (layout.ImgReceiptItemsLayoutXML != null) { statement.AddField("IMG_RECEIPTITEMSLAYOUTXML", layout.ImgReceiptItemsLayoutXML, SqlDbType.Image); }
            if (layout.ImgReceiptPaymentLayoutXML != null) { statement.AddField("IMG_RECEIPTPAYMENTLAYOUTXML", layout.ImgReceiptPaymentLayoutXML, SqlDbType.Image); }
            if (layout.ImgTotalsLayoutXML != null) { statement.AddField("IMG_TOTALSLAYOUTXML", layout.ImgTotalsLayoutXML, SqlDbType.Image); }
            if (layout.ImgLayoutXML != null) { statement.AddField("IMG_LAYOUTXML", layout.ImgLayoutXML, SqlDbType.Image); }

            if (layout.CustomerLayoutXML != null) statement.AddField("CUSTOMERLAYOUTXML", layout.CustomerLayoutXML, SqlDbType.Text);
            if (layout.ReceiptItemsLayoutXML != null) statement.AddField("RECEIPTITEMSLAYOUTXML", layout.ReceiptItemsLayoutXML, SqlDbType.Text);
            if (layout.ReceiptPaymentLayoutXML != null) statement.AddField("RECEIPTPAYMENTLAYOUTXML", layout.ReceiptPaymentLayoutXML, SqlDbType.Text);
            if (layout.TotalsLayoutXML != null) statement.AddField("TOTALSLAYOUTXML", layout.TotalsLayoutXML, SqlDbType.Text);
            if (layout.LayoutXML != null) statement.AddField("LAYOUTXML", layout.LayoutXML, SqlDbType.Text);

            if (layout.ImgCashChangerLayoutXML != null) { statement.AddField("IMG_CASHCHANGERLAYOUTXML", layout.ImgCashChangerLayoutXML, SqlDbType.Image); }

            if (layout.CashChangerLayoutXML != null) statement.AddField("CASHCHANGERLAYOUTXML", layout.CashChangerLayoutXML, SqlDbType.Text);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Providers.TouchLayoutData.Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "TOUCHLAYOUT"; }
        }

        #endregion
    }
}
