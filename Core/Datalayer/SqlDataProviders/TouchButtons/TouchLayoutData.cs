using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Sequences;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TouchButtons
{
    public class TouchLayoutData : SqlServerDataProviderBase, ITouchLayoutData
    {
        private static string BaseSelectString
        {
            get
            {
                return @"select 
                    LAYOUTID, 
                    ISNULL(NAME,'') as NAME, 
                    ISNULL(WIDTH, 0) as WIDTH, 
                    ISNULL(HEIGHT,0) as HEIGHT, 
                    ISNULL(BUTTONGRID1,'') as BUTTONGRID1, 
                    ISNULL(BUTTONGRID2,'') as BUTTONGRID2, 
                    ISNULL(BUTTONGRID3,'') as BUTTONGRID3, 
                    ISNULL(BUTTONGRID4,'') as BUTTONGRID4, 
                    ISNULL(BUTTONGRID5,'') as BUTTONGRID5, 
                    ISNULL(RECEIPTID,'') as RECEIPTID, 
                    ISNULL(TOTALID,'') as TOTALID, 
                    ISNULL(LOGOPICTUREID, '') as LOGOPICTUREID, 
                    IMG_CUSTOMERLAYOUTXML, 
                    IMG_RECEIPTITEMSLAYOUTXML, 
                    IMG_RECEIPTPAYMENTLAYOUTXML, 
                    IMG_TOTALSLAYOUTXML, 
                    IMG_LAYOUTXML, 
                    ISNULL(RECEIPTITEMSLAYOUTXML,'') as RECEIPTITEMSLAYOUTXML, 
                    ISNULL(RECEIPTPAYMENTLAYOUTXML,'') as RECEIPTPAYMENTLAYOUTXML, 
                    ISNULL(TOTALSLAYOUTXML,'') as TOTALSLAYOUTXML, 
                    ISNULL(LAYOUTXML,'') as LAYOUTXML, 
                    IMG_CASHCHANGERLAYOUTXML, 
                    ISNULL(CASHCHANGERLAYOUTXML,'') as CASHCHANGERLAYOUTXML, 
                    GUID, 
                    IMPORTDATETIME 
                    from POSISTILLLAYOUT ";
            }
        }

        private static string ResolveSort(TouchLayoutSorting sort, bool backwards)
        {
            switch(sort)
            {
                case TouchLayoutSorting.ID:
                    return backwards ? "LAYOUTID DESC" : "LAYOUTID ASC";

                case TouchLayoutSorting.Description:
                    return backwards ? "NAME DESC" : "NAME ASC";
                case TouchLayoutSorting.ImportDateTime:
                    return backwards ? "IMPORTDATETIME DESC" : "IMPORTDATETIME ASC";
                default:
                    return "";
            }            
        }

        private static void PopulateTouchLayout(IDataReader dr, TouchLayout touchLayout)
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
            touchLayout.LogoPictureID = (string)dr["LOGOPICTUREID"];
            touchLayout.ImgCustomerLayoutXML = dr["IMG_CUSTOMERLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_CUSTOMERLAYOUTXML"];
            touchLayout.ImgReceiptItemsLayoutXML = dr["IMG_RECEIPTITEMSLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_RECEIPTITEMSLAYOUTXML"]; 
            touchLayout.ImgReceiptPaymentLayoutXML = dr["IMG_RECEIPTPAYMENTLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_RECEIPTPAYMENTLAYOUTXML"];
            touchLayout.ImgTotalsLayoutXML = dr["IMG_TOTALSLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_TOTALSLAYOUTXML"]; 
            touchLayout.ImgLayoutXML = dr["IMG_LAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_LAYOUTXML"];
            touchLayout.ReceiptItemsLayoutXML = (string)dr["RECEIPTITEMSLAYOUTXML"];
            touchLayout.ReceiptPaymentLayoutXML = (string)dr["RECEIPTPAYMENTLAYOUTXML"];
            touchLayout.TotalsLayoutXML = (string)dr["TOTALSLAYOUTXML"];
            touchLayout.LayoutXML = (string)dr["LAYOUTXML"];
            touchLayout.ImgCashChangerLayoutXML = dr["IMG_CASHCHANGERLAYOUTXML"] == DBNull.Value ? null : (string)dr["IMG_CASHCHANGERLAYOUTXML"]; 
            touchLayout.CashChangerLayoutXML = (string)dr["CASHCHANGERLAYOUTXML"];
            touchLayout.Guid = (Guid) dr["GUID"];
            touchLayout.ImportDateTime = dr["IMPORTDATETIME"] == DBNull.Value ? touchLayout.ImportDateTime : (DateTime) dr["IMPORTDATETIME"];
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList(entry, "NAME");
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry,string sort)
        {
            return GetList<DataEntity>(entry, "POSISTILLLAYOUT", "NAME", "LAYOUTID", sort);
        }

        /// <summary>
        /// Returns a list of all touch layouts ordered by the specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>An ordered list of touch layouts</returns>
        public virtual List<TouchLayout> GetTouchLayouts(IConnectionManager entry, TouchLayoutSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId " +
                    "order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<TouchLayout>(entry, cmd, CommandType.Text, PopulateTouchLayout);
            }
        }

        /* Not used
        private static void PopulateLayout(IDataReader dr, TouchLayout layout)
        {
            layout.ID = (string)dr["LAYOUTID"];
            layout.Text = (string)dr["NAME"];
        }*/

        public virtual TouchLayout Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and LAYOUTID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                return Get<TouchLayout>(entry, cmd, id, PopulateTouchLayout, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual TouchLayout GetByGuid(IConnectionManager entry, Guid guid, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and GUID = @guid";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "guid", guid);

                return Get<TouchLayout>(entry, cmd, new RecordIdentifier(), PopulateTouchLayout, cacheType, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Returns a lit of the first 5 button grids
        /// </summary>
        /// <returns></returns>
        public virtual List<DataEntity> GetButtonGrids()
        {
            var buttonGrids = new List<DataEntity>();

            var buttonGrid = new DataEntity {ID = (int) ButtonGridsEnum.ButtonGrid1, Text = "Button grid 1"};
            buttonGrids.Add(buttonGrid);

            buttonGrid = new DataEntity {ID = (int) ButtonGridsEnum.ButtonGrid2, Text = "Button grid 2"};
            buttonGrids.Add(buttonGrid);

            buttonGrid = new DataEntity {ID = (int) ButtonGridsEnum.ButtonGrid3, Text = "Button grid 3"};
            buttonGrids.Add(buttonGrid);

            buttonGrid = new DataEntity {ID = (int) ButtonGridsEnum.ButtonGrid4, Text = "Button grid 4"};
            buttonGrids.Add(buttonGrid);

            buttonGrid = new DataEntity {ID = (int) ButtonGridsEnum.ButtonGrid5, Text = "Button grid 5"};
            buttonGrids.Add(buttonGrid);

            return buttonGrids;
        }

        /// <summary>
        /// Gets the ID of the touch layout that should be used. The hierarchy of the layouts is as follows:
        ///  1. POS user
        ///  2. Active hospitality type
        ///  3. Terminal
        ///  4. Store        
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posUserID">The pos user</param>        
        /// <param name="hospitalitySalesTypeID">The ID of the sales type that is in use by the current hospitality type</param>
        /// <param name="terminalID">The terminal</param>
        /// <param name="storeID">The store</param>
        /// <returns>RecordIdentifier.Empty if no touch layout is defined, the ID of the correct touch layout otherwise</returns>
        public virtual RecordIdentifier GetPOSTouchLayoutID(IConnectionManager entry,RecordIdentifier posUserID, RecordIdentifier hospitalitySalesTypeID, RecordIdentifier terminalID, RecordIdentifier storeID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                 @" SELECT COALESCE(PROFILE.LAYOUTID, HOSPITALITY.LAYOUTID, TERMINAL.LAYOUTID, STORE.LAYOUTID, '') AS LAYOUTID
                    FROM 
                    (RBOSTAFFTABLE STAFF  
					JOIN POSUSERPROFILE PROFILE ON STAFF.USERPROFILE = PROFILE.PROFILEID
                    JOIN POSISTILLLAYOUT LAYOUT ON PROFILE.LAYOUTID = LAYOUT.LAYOUTID AND STAFF.DATAAREAID = LAYOUT.DATAAREAID AND PROFILE.LAYOUTID != ''  AND STAFF.STAFFID = @posUserID AND STAFF.DATAAREAID = @dataAreaID) 
                    FULL OUTER JOIN
                    (HOSPITALITYTYPE HOSPITALITY
                    JOIN POSISTILLLAYOUT LAYOUT1 ON HOSPITALITY.LAYOUTID = LAYOUT1.LAYOUTID AND HOSPITALITY.DATAAREAID = LAYOUT1.DATAAREAID AND HOSPITALITY.SALESTYPE = @activeHospitalityTypeID AND HOSPITALITY.DATAAREAID = @dataAreaID) ON 1 = 1
                    FULL OUTER JOIN 
                    (RBOTERMINALTABLE TERMINAL  
                    JOIN POSISTILLLAYOUT LAYOUT2 ON TERMINAL.LAYOUTID = LAYOUT2.LAYOUTID AND TERMINAL.DATAAREAID = LAYOUT2.DATAAREAID AND TERMINAL.TERMINALID = @terminalID AND TERMINAL.STOREID = @storeID AND TERMINAL.LAYOUTID != '' AND TERMINAL.DATAAREAID = @dataAreaID) ON 1 = 1
                    FULL OUTER JOIN
                    (RBOSTORETABLE STORE
                    JOIN POSISTILLLAYOUT LAYOUT3 ON STORE.LAYOUTID = LAYOUT3.LAYOUTID AND STORE.DATAAREAID = LAYOUT3.DATAAREAID AND STORE.STOREID = @storeID AND STORE.LAYOUTID != '' AND STORE.DATAAREAID = @dataAreaID) ON 1 = 1";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);                
                MakeParam(cmd, "posUserID", (string)posUserID);
                MakeParam(cmd, "activeHospitalityTypeID", (string)hospitalitySalesTypeID);
                MakeParam(cmd, "terminalID", (string)terminalID);                
                MakeParam(cmd, "storeID", (string)storeID);

                return (string)entry.Connection.ExecuteScalar(cmd) ?? RecordIdentifier.Empty;
            }            
        }

        public virtual bool GuidExists(IConnectionManager entry, Guid guid)
        {
            return RecordExists(entry, "POSISTILLLAYOUT", "GUID", guid);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "POSISTILLLAYOUT", "LAYOUTID", id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "POSISTILLLAYOUT", "LAYOUTID", id, BusinessObjects.Permission.ManageTouchButtonLayout);
        }

        /// <summary>
        /// Copies the given TouchLayout and returns the ID of the newly created layout
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="newLayout">The layout to copy from</param>
        /// <param name="copyFromID">The ID of the layout to copy from</param>
        /// <returns></returns>
        public virtual RecordIdentifier CreateNewAndCopyFrom(IConnectionManager entry, TouchLayout newLayout, RecordIdentifier copyFromID)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageTouchButtonLayout);

            // Get the layout containing the data to copy from
            TouchLayout layoutToCopy = Get(entry, copyFromID);
            //layoutToCopy.ButtonGrid1 = newLayout.ButtonGrid1;
            //layoutToCopy.ButtonGrid2 = newLayout.ButtonGrid2;
            //layoutToCopy.ButtonGrid3 = newLayout.ButtonGrid3;
            //layoutToCopy.ButtonGrid4 = newLayout.ButtonGrid4;
            //layoutToCopy.ButtonGrid5 = newLayout.ButtonGrid5;
            layoutToCopy.Name = newLayout.Text;
            layoutToCopy.ID = DataProviderFactory.Instance.
                    Get<INumberSequenceData, NumberSequence>()
                    .GenerateNumberFromSequence(entry, new TouchLayoutData()); 
            layoutToCopy.Guid = Guid.Empty;
            layoutToCopy.ImportDateTime = null;

            Save(entry, layoutToCopy);

            return layoutToCopy.ID;
        }

        /* Not used
        private static void CopyButtonGridAndButtons(IConnectionManager entry, ButtonGrid buttonGridToCopy, List<ButtonGridButton> buttonGridButtonsToCopy, 
            string newLayoutName)
        {
            // Give new id's to the button grids and button grid buttons that we intend to copy
            if (buttonGridToCopy != null)
            {
                buttonGridToCopy.ID = entry.GenerateNumberFromSequence(new ButtonGridData());

                //Give the new grid a name that includes the new layout name otherwise the user
                //gets two button grids with the same name in the list with no way to know which grid is which
                string newGridName = newLayoutName + "-" + buttonGridToCopy.Text;
                if (newGridName.Length > 50)
                {
                    newGridName = newGridName.Remove(49);
                }

                buttonGridToCopy.Text = newGridName;                
                int maxButtonGridButtonID = DataProviderFactory.Instance.GetProvider<IButtonGridButtonsData, ButtonGridButtons>().GetNextButtonID(entry);

                // Replace each buttonGridID field in buttonGrid1Buttons with the new button grid ID
                for (int i = 0; i < buttonGridButtonsToCopy.Count; i++)
                {
                    buttonGridButtonsToCopy[i].ButtonGridID = buttonGridToCopy.ID;
                    buttonGridButtonsToCopy[i].ID = maxButtonGridButtonID + i;
                    DataProviderFactory.Instance.GetProvider<IButtonGridButtonsData, ButtonGridButtons>().Save(entry, buttonGridButtonsToCopy[i]);
                }

                // Save the new button grid and button grid buttons
                DataProviderFactory.Instance.GetProvider<IButtonGridData, ButtonGrid>().Save(entry, buttonGridToCopy);
            }
        } */

        /* Not used
        private static void DeleteButtonGridAndButtons(IConnectionManager entry, ButtonGrid buttonGridToDelete)
        {
            if (buttonGridToDelete != null)
            {
                foreach (ButtonGridButton buttons in DataProviderFactory.Instance.GetProvider<IButtonGridButtonsData, ButtonGridButtons>().GetList(entry, buttonGridToDelete.ID))
                {
                    DataProviderFactory.Instance.GetProvider<IButtonGridButtonsData, ButtonGridButtons>().Delete(entry, buttons.ID);
                }

                DataProviderFactory.Instance.GetProvider<IButtonGridData, ButtonGrid>().Delete(entry, buttonGridToDelete.ID);
            }
        } */

        /// <summary>
        /// Used to save only the header information. This is used when the TillLayoutDesigner has handled saving
        /// the rest of the layout information and we only need to handle the saving of header information.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="layout">The layout to save</param>
        public virtual void SaveHeader(IConnectionManager entry, TouchLayout layout)
        {
            var statement = new SqlServerStatement("POSISTILLLAYOUT");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageTouchButtonLayout);

            statement.StatementType = StatementType.Update;

            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("LAYOUTID", (string)layout.ID);

            statement.AddField("WIDTH", layout.Width, SqlDbType.Int);
            statement.AddField("HEIGHT", layout.Height, SqlDbType.Int);

            statement.AddField("NAME", layout.Text);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves a touch layout into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="layout">The layout to save</param>
        public virtual void Save(IConnectionManager entry, TouchLayout layout)
        {
            var statement = new SqlServerStatement("POSISTILLLAYOUT");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageTouchButtonLayout);

            var isNew = false;
            if (layout.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                layout.ID = DataProviderFactory.Instance.GenerateNumber<ITouchLayoutData, TouchLayout>(entry); 
            }

            if (layout.Guid == Guid.Empty)
            {
                layout.Guid = Guid.NewGuid();
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

            statement.AddField("GUID", layout.Guid, SqlDbType.UniqueIdentifier);

            if (layout.ImportDateTime != null) { statement.AddField("IMPORTDATETIME", layout.ImportDateTime, SqlDbType.DateTime); }

            // The tillLayoutDesigner will not show the layout if these fields contain an empty string
            if (layout.ReceiptID != "") { statement.AddField("RECEIPTID", layout.ReceiptID); }
            if (layout.TotalID != "") { statement.AddField("TOTALID", layout.TotalID); }
            if (layout.LogoPictureID != "") { statement.AddField("LOGOPICTUREID", (string)layout.LogoPictureID); }

            if (layout.ImgCustomerLayoutXML != null) { statement.AddField("IMG_CUSTOMERLAYOUTXML", layout.ImgCustomerLayoutXML, SqlDbType.Image); }
            if (layout.ImgReceiptItemsLayoutXML != null) { statement.AddField("IMG_RECEIPTITEMSLAYOUTXML", layout.ImgReceiptItemsLayoutXML, SqlDbType.Image); }
            if (layout.ImgReceiptPaymentLayoutXML != null) { statement.AddField("IMG_RECEIPTPAYMENTLAYOUTXML", layout.ImgReceiptPaymentLayoutXML, SqlDbType.Image); }
            if (layout.ImgTotalsLayoutXML != null) { statement.AddField("IMG_TOTALSLAYOUTXML", layout.ImgTotalsLayoutXML, SqlDbType.Image); }
            if (layout.ImgLayoutXML != null) { statement.AddField("IMG_LAYOUTXML", layout.ImgLayoutXML, SqlDbType.Image); }

            if (layout.ReceiptItemsLayoutXML != null) statement.AddField("RECEIPTITEMSLAYOUTXML", layout.ReceiptItemsLayoutXML, SqlDbType.Text);
            if (layout.ReceiptPaymentLayoutXML != null) statement.AddField("RECEIPTPAYMENTLAYOUTXML", layout.ReceiptPaymentLayoutXML, SqlDbType.Text);
            if (layout.TotalsLayoutXML != null) statement.AddField("TOTALSLAYOUTXML", layout.TotalsLayoutXML, SqlDbType.Text);
            if (layout.LayoutXML != null) statement.AddField("LAYOUTXML", layout.LayoutXML, SqlDbType.Text);

            if (layout.ImgCashChangerLayoutXML != null) { statement.AddField("IMG_CASHCHANGERLAYOUTXML", layout.ImgCashChangerLayoutXML, SqlDbType.Image); }

            if (layout.CashChangerLayoutXML != null) statement.AddField("CASHCHANGERLAYOUTXML", layout.CashChangerLayoutXML, SqlDbType.Text);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "TOUCHLAYOUT"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSISTILLLAYOUT", "LAYOUTID", sequenceFormat, startingRecord, numberOfRecords);
        }
        #endregion
    }
}
