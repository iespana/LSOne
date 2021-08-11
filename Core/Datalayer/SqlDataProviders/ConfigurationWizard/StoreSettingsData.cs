using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ConfigurationWizard;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders.ConfigurationWizard;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.ConfigurationWizard
{
    /// <summary>
    /// Data prover class for StoreSettings.
    /// </summary>
    public class StoreSettingsData : SqlServerDataProviderBase, IStoreSettingsData
    {
        private static void PopulateStoreSalesTaxGroup(IDataReader dr, StoreSettings saleTaxGroup)
        {
            saleTaxGroup.TaxGroupId = (string)dr["TAXGROUP"];
            saleTaxGroup.TaxGroupName = (string)dr["TAXGROUPNAME"];
            saleTaxGroup.TaxGroupType = 0;
        }

        /// <summary>
        /// Get SalesTaxGroup list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of SalesTaxGroup</returns>
        public virtual List<StoreSettings> GetSalesTaxGroupList(IConnectionManager entry)
        {
            return GetList<StoreSettings>(entry, "TAXGROUPHEADING", "TAXGROUPNAME", "TAXGROUP", "TAXGROUP", PopulateStoreSalesTaxGroup);
        }

        private static void PopulateStoreItemTaxGroup(IDataReader dr, StoreSettings itemTaxGroup)
        {
            itemTaxGroup.TaxGroupId = (string)dr["TAXITEMGROUP"];
            itemTaxGroup.TaxGroupName = (string)dr["NAME"];
            itemTaxGroup.TaxGroupType = 1;
        }

        /// <summary>
        /// Get ItemTaxGroup list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of ItemTaxGroup</returns>
        public virtual List<StoreSettings> GetItemTaxGroupList(IConnectionManager entry)
        {
            return GetList<StoreSettings>(entry, "TAXITEMGROUPHEADING", "NAME", "TAXITEMGROUP", "TAXITEMGROUP", PopulateStoreItemTaxGroup);
        }

        private static void PopulateUnits(IDataReader dr, Unit unit)
        {
            unit.ID = (string)dr["UNITID"];
            unit.Text = (string)dr["TXT"];
        }

        /// <summary>
        /// Get Unit list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>List of Units</returns>
        public virtual List<Unit> GetUnitList(IConnectionManager entry)
        {
            return GetList<Unit>(entry, "UNIT", "TXT", "UNITID", "TXT", PopulateUnits);
        }

        /// <summary>
        /// Save TaxGroups and Units into the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeList">StoreSettings object list</param>
        public virtual void SaveTaxGroups(IConnectionManager entry, List<StoreSettings> storeList)
        {
            if (storeList.Count == 0)
                return;
            Delete(entry, storeList.First().TemplateID);
            DeleteRecord(entry, "WIZARDTEMPLATEUNITS", "ID", storeList.First().TemplateID, BusinessObjects.Permission.EditSalesTaxSetup);

            foreach (var storeItem in storeList)
            {
                if (storeItem.TaxGroupId != RecordIdentifier.Empty)
                {
                    var statement = new SqlServerStatement("WIZARDTEMPLATETAX") {StatementType = StatementType.Insert};

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                    statement.AddKey("ID", storeItem.TemplateID.StringValue);

                    statement.AddField("TAXGROUPTYPE", storeItem.TaxGroupType, SqlDbType.Int);

                    statement.AddField("TAXGROUP", storeItem.TaxGroupId.StringValue, SqlDbType.NVarChar);

                    entry.Connection.ExecuteStatement(statement);
                }

                if (storeItem.ID.StringValue != string.Empty)
                {
                    var statement = new SqlServerStatement("WIZARDTEMPLATEUNITS") {StatementType = StatementType.Insert};

                    statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);

                    statement.AddKey("ID", (string)storeItem.TemplateID);

                    statement.AddField("UNITID", (string)storeItem.ID, SqlDbType.NVarChar);

                    entry.Connection.ExecuteStatement(statement);
                }
            }            
        }

        /// <summary>
        /// Check entry of selected entity into database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">Id of Template</param>
        /// <returns>boolean result</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "WIZARDTEMPLATETAX", "ID", id);
        }

        /// <summary>
        /// Delete a entity declaration from given table with a given id.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <param name="table">Table name</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id, string table = "WIZARDTEMPLATETAX")
        {
            DeleteRecord(entry, table, "ID", id, BusinessObjects.Permission.EditSalesTaxSetup);
        }

        private static void PopulateTaxItems(IConnectionManager entry, IDataReader dr, StoreSettings store, Object obj)
        {
            store.TaxGroupId = Convert.ToString(dr["TAXGROUP"]);
            store.TaxGroupType = Convert.ToInt32(dr["TAXGROUPTYPE"]);
            store.TemplateID = Convert.ToString(dr["ID"]);
        }

        /// <summary>
        /// Get TaxGroup list from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>TaxGroup List</returns>
        public virtual  List<StoreSettings> GetTaxList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, TAXGROUPTYPE, TAXGROUP 
                                    FROM WIZARDTEMPLATETAX 
                                    WHERE ID = @ID
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                return Execute<StoreSettings>(entry, cmd, CommandType.Text, null, PopulateTaxItems);
            }
        }

        private static void PopulateStoreUnitItems(IConnectionManager entry, IDataReader dr, StoreSettings unit, object obj)
        {
            unit.ID = Convert.ToString(dr["UNITID"]);            
        }

        /// <summary>
        /// Get Unit list from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">TemplateId</param>
        /// <returns>List Of Units</returns>
        public virtual List<StoreSettings> GetUnitList(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT UNITID 
                                    FROM WIZARDTEMPLATEUNITS 
                                    WHERE ID = @ID
                                    AND DATAAREAID = @DATAAREAID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                return Execute<StoreSettings>(entry, cmd, CommandType.Text, null, PopulateStoreUnitItems);
            }
        }

        private static void PopulateSalesTaxGroup(IDataReader dr, SalesTaxGroup saleTaxGroup)
        {
            saleTaxGroup.ID = (string)dr["TAXGROUP"];
            saleTaxGroup.Text = (string)dr["TAXGROUPNAME"];
            saleTaxGroup.SearchField1 = (string)dr["SEARCHFIELD1"];
            saleTaxGroup.SearchField2 = (string)dr["SEARCHFIELD2"];
        }

        /// <summary>
        /// Get selected SalesTaxGroups from database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">SalesTaxGroupId list</param>
        /// <returns>SalesTaxGroup list</returns>
        public virtual List<SalesTaxGroup> GetSelectedSalesTaxGroupList(IConnectionManager entry, List<RecordIdentifier> idList)
        {
            List<SalesTaxGroup> result;
            string ids = "";
            foreach (RecordIdentifier id in idList)
            {
                ids += "'" + id.StringValue + "',";
            }

            ids = ids.Remove(ids.LastIndexOf(','));
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT g.TAXGROUP,ISNULL(g.TAXGROUPNAME,'') AS TAXGROUPNAME , ISNULL(g.SEARCHFIELD1,'') AS SEARCHFIELD1, ISNULL(g.SEARCHFIELD2,'') AS SEARCHFIELD2 
                    FROM TAXGROUPHEADING g 
                    WHERE g.DATAAREAID = @DATAAREAID AND g.TAXGROUP IN (" + ids + ")";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                result = Execute<SalesTaxGroup>(entry, cmd, CommandType.Text, PopulateSalesTaxGroup);
            }

            return (result.Count > 0) ? result : null;
        }

        private static void PopulateItemTaxGroup(IDataReader dr, ItemSalesTaxGroup itemTaxGroup)
        {
            itemTaxGroup.ID = (string)dr["TAXITEMGROUP"];
            itemTaxGroup.Text = (string)dr["NAME"];
            itemTaxGroup.ReceiptDisplay = (string)dr["RECEIPTDISPLAY"];
        }

        /// <summary>
        /// Get selected ItemTaxGroup list from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">ItemTaxGroupId list</param>
        /// <returns>ItemTaxGroup list</returns>
        public virtual List<ItemSalesTaxGroup> GetSelectedItemTaxGroupList(IConnectionManager entry, List<RecordIdentifier> idList)
        {
            List<ItemSalesTaxGroup> result;
            string ids = "";
            foreach (RecordIdentifier id in idList)
            {
                ids += "'" + id.StringValue + "',";
            }
            ids = ids.Remove(ids.LastIndexOf(','));
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT g.TAXITEMGROUP,ISNULL(g.NAME,'') AS NAME, " +
                       "ISNULL(g.RECEIPTDISPLAY,'') AS RECEIPTDISPLAY " +
                        "FROM TAXITEMGROUPHEADING g " +
                    "WHERE g.DATAAREAID = @DATAAREAID AND g.TAXITEMGROUP IN (" + ids + ")";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                result = Execute<ItemSalesTaxGroup>(entry, cmd, CommandType.Text, PopulateItemTaxGroup);
            }

            return (result.Count > 0) ? result : null;
        }

        private static void PopulateUnitItems(IConnectionManager entry, IDataReader dr, Unit unit, object obj)
        {
            unit.ID = Convert.ToString(dr["UNITID"]);
            unit.Text = Convert.ToString(dr["DESCRIPTION"]);
            unit.MinimumDecimals = Convert.ToInt32(dr["MINUNITDECIMALS"]);
            unit.MaximumDecimals = Convert.ToInt32(dr["UNITDECIMALS"]);
        }

        /// <summary>
        /// Get selected Unit list from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="idList">UnitId List</param>
        /// <returns>Unit list</returns>
        public virtual List<Unit> GetSelectedUnitList(IConnectionManager entry, List<RecordIdentifier> idList)
        {
            List<Unit> result;
            string ids = "";
            foreach (RecordIdentifier id in idList)
            {
                ids += "'" + id.StringValue + "',";
            }
            ids = ids.Remove(ids.LastIndexOf(','));
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT UNITID, ISNULL(TXT,'') AS DESCRIPTION, ISNULL(UNITDECIMALS,0) AS UNITDECIMALS,ISNULL(MINUNITDECIMALS,0) AS MINUNITDECIMALS 
                      FROM UNIT 
                      WHERE DATAAREAID = @DATAAREAID 
                      AND UNITID IN (" + ids + ")";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                result = Execute<Unit>(entry, cmd, CommandType.Text, null, PopulateUnitItems);
            }

            return (result.Count > 0) ? result : null;
        }

        /// <summary>
        /// Get selected VisualProfile from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">VisualProfileId</param>
        /// <returns>VisualProfile</returns>
        public virtual VisualProfile GetSelectedVisualProfileList(IConnectionManager entry, RecordIdentifier id)
        {
            List<VisualProfile> result;
            
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT " +
                        "V.PROFILEID, " +
                        "ISNULL(V.NAME,'') as NAME, " +
                        "ISNULL(RESOLUTION, 0) as RESOLUTION, " +
                        "ISNULL(TERMINALTYPE, 0) as TERMINALTYPE, " +
                        "ISNULL(HIDECURSOR, 0) as HIDECURSOR, " +
                        "ISNULL(DESIGNALLOWEDONPOS, 0) as DESIGNALLOWEDONPOS, " +
                        "ISNULL(OPAQUEBACKGROUNDFORM, 0) as OPAQUEBACKGROUNDFORM, " +
                        "ISNULL(OPACITY, 0) as OPACITY, " +
                        "ISNULL(USEFORMBACKGROUNDIMAGE, 0) as USEFORMBACKGROUNDIMAGE,  " +
                        "ISNULL(SHOWCURRENCYSYMBOLONCOLUMNS, 0) as SHOWCURRENCYSYMBOLONCOLUMNS,  " +
                        "ISNULL(SCREENINDEX,0) as SCREENINDEX,  " +
                        "ISNULL(RECEIPTPAYMENTLINESSIZE, 30) as RECEIPTPAYMENTLINESSIZE, " +
                        "ISNULL(RECEIPTRETURNBACKGROUNDIMAGEID, '') as RECEIPTRETURNBACKGROUNDIMAGEID, " +
                        "RECEIPTRETURNBACKGROUNDIMAGELAYOUT, " +
                        "RECEIPTRETURNBORDERCOLOR, " +
                        "ISNULL(CONFIRMBUTTONSTYLEID, '') as CONFIRMBUTTONSTYLEID," +
                        "ISNULL(CANCELBUTTONSTYLEID, '') as CANCELBUTTONSTYLEID," +
                        "ISNULL(ACTIONBUTTONSTYLEID, '') as ACTIONBUTTONSTYLEID," +
                        "ISNULL(NORMALBUTTONSTYLEID, '') as NORMALBUTTONSTYLEID," +
                        "ISNULL(OTHERBUTTONSTYLEID, '') as OTHERBUTTONSTYLEID," +
                        "ISNULL(OVERRIDEPOSCONTROLBORDERCOLOR, 0) as OVERRIDEPOSCONTROLBORDERCOLOR," +
                        $"ISNULL(POSCONTROLBORDERCOLOR, {ColorPalette.POSControlBorderColor.ToArgb()}) as POSCONTROLBORDERCOLOR," +
                        "ISNULL(OVERRIDEPOSSELECTEDROWCOLOR, 0) as OVERRIDEPOSSELECTEDROWCOLOR," +
                        $"ISNULL(POSSELECTEDROWCOLOR, {ColorPalette.POSSelectedRowColor.ToArgb()}) as POSSELECTEDROWCOLOR " +
                        " FROM POSVISUALPROFILE V " +
                    "WHERE V.PROFILEID = @PROFILEID and V.DATAAREAID = @DATAAREAID ORDER BY PROFILEID";

                MakeParam(cmd, "PROFILEID", (string)id);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                result = Execute<VisualProfile>(entry, cmd, CommandType.Text, PopulateVisualProfile);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        private static void PopulateVisualProfile(IDataReader dr, VisualProfile visualProfile)
        {
            visualProfile.ID = (string)dr["PROFILEID"];
            visualProfile.Text = (string)dr["NAME"];
            visualProfile.Resolution = (ResolutionsEnum)dr["RESOLUTION"];
            visualProfile.TerminalType = (VisualProfile.HardwareTypes)dr["TERMINALTYPE"];
            visualProfile.HideCursor = ((byte)dr["HIDECURSOR"]) != 0;
            visualProfile.DesignAllowedOnPos = ((byte)dr["DESIGNALLOWEDONPOS"]) != 0;
            visualProfile.OpaqueBackgroundForm = ((byte)dr["OPAQUEBACKGROUNDFORM"]) != 0;
            visualProfile.Opacity = Convert.ToInt32((decimal)dr["OPACITY"]);
            visualProfile.UseFormBackgroundImage = ((byte)dr["USEFORMBACKGROUNDIMAGE"]) != 0;
            visualProfile.ScreenNumber = (ScreenNumberEnum)dr["SCREENINDEX"];
            visualProfile.ShowCurrencySymbolOnColumns = ((byte)dr["SHOWCURRENCYSYMBOLONCOLUMNS"]) != 0;
            visualProfile.ReceiptPaymentLinesSize = (int)dr["RECEIPTPAYMENTLINESSIZE"];
            visualProfile.ReceiptReturnBackgroundImageID = (string)dr["RECEIPTRETURNBACKGROUNDIMAGEID"];
            visualProfile.ReceiptReturnBackgroundImageLayout = (ImageLayout)(int)dr["RECEIPTRETURNBACKGROUNDIMAGELAYOUT"];
            visualProfile.ReceiptReturnBorderColor = (int)dr["RECEIPTRETURNBORDERCOLOR"];
            visualProfile.ConfirmButtonStyleID = (string)dr["CONFIRMBUTTONSTYLEID"];
            visualProfile.CancelButtonStyleID = (string)dr["CANCELBUTTONSTYLEID"];
            visualProfile.ActionButtonStyleID = (string)dr["ACTIONBUTTONSTYLEID"];
            visualProfile.NormalButtonStyleID = (string)dr["NORMALBUTTONSTYLEID"];
            visualProfile.OtherButtonStyleID = (string)dr["OTHERBUTTONSTYLEID"];
            visualProfile.OverridePOSControlBorderColor = (bool)dr["OVERRIDEPOSCONTROLBORDERCOLOR"];
            visualProfile.POSControlBorderColor = (int)dr["POSCONTROLBORDERCOLOR"];
            visualProfile.OverridePOSSelectedRowColor = (bool)dr["OVERRIDEPOSSELECTEDROWCOLOR"];
            visualProfile.POSSelectedRowColor = (int)dr["POSSELECTEDROWCOLOR"];
        }

        /// <summary>
        /// Get selected FuntionalityProfile from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">FuntionalityProfileId</param>
        /// <returns>FuntionalityProfile</returns>
        public virtual FunctionalityProfile GetSelectedFuntionalityProfileList(IConnectionManager entry, RecordIdentifier id)
        {
            List<FunctionalityProfile> result;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select PROFILEID, ISNULL(NAME,'') as NAME,ISNULL(AGGREGATEITEMS,0) as AGGREGATEITEMS," +
                       "ISNULL(AGGREGATEPAYMENTS,0) as AGGREGATEPAYMENTS,ISNULL(TSCENTRALTABLESERVER,0) as TSCENTRALTABLESERVER," +
                       "ISNULL(CENTRALTABLESERVER,'') as CENTRALTABLESERVER,ISNULL(CENTRALTABLESERVERPORT,0) as CENTRALTABLESERVERPORT," +
                       "ISNULL(TSCUSTOMER,0) as TSCUSTOMER,ISNULL(TSSTAFF,0) as TSSTAFF,ISNULL(TSSUSPENDRETRIEVETRANSACTIONS,0) as TSSUSPENDRETRIEVETRANSACTIONS," +
                       "ISNULL(LOGLEVEL,0) as LOGLEVEL,ISNULL(AGGREGATEITEMSFORPRINTING,0) as AGGREGATEITEMSFORPRINTING," +
                       "ISNULL(SHOWSTAFFLISTATLOGON,0) as SHOWSTAFFLISTATLOGON,ISNULL(LIMITSTAFFLISTTOSTORE,0) as LIMITSTAFFLISTTOSTORE," +
                       "ISNULL(ALLOWSALESIFDRAWERISOPEN,0) as ALLOWSALESIFDRAWERISOPEN, " +
                       "ISNULL(STAFFBARCODELOGON,0) as STAFFBARCODELOGON, " +
                       "ISNULL(STAFFCARDLOGON,0) as STAFFCARDLOGON, " +
                       "ISNULL(MUSTKEYINPRICEIFZERO,0) as MUSTKEYINPRICEIFZERO, " +
                       "ISNULL(MINIMUMPASSWORDLENGTH,6) as MINIMUMPASSWORDLENGTH, " + // 6 sounds like a good default value
                       "ISNULL(ISHOSPITALITYPROFILE,0) as ISHOSPITALITYPROFILE, " +
                       "ISNULL(SKIPHOSPITALITYTABLEVIEW,0) as SKIPHOSPITALITYTABLEVIEW, " +
                       "ISNULL(STARTOFTRANSACTION,'') as STARTOFTRANSACTION, " +
                       "ISNULL(ENDOFTRANSACTION,'') as ENDOFTRANSACTION, " +
                       "ISNULL(NEGATIVEADJUSTMENT,'') as NEGATIVEADJUSTMENT, " +
                       "ISNULL(VOIDISPRESSED,'') as VOIDISPRESSED, " +
                       "ISNULL(VOIDTRANSACTION,'') as VOIDTRANSACTION, " +
                       "ISNULL(VOIDPAYMENT,'') as VOIDPAYMENT, " +
                       "ISNULL(REFUNDSALE,'') as REFUNDSALE, " +
                       "ISNULL(MARKDOWN,'') as MARKDOWN, " +
                       "ISNULL(MARKUP,'') as MARKUP, " +
                       "ISNULL(OVERRIDEPRICE,'') as OVERRIDEPRICE, " +
                       "ISNULL(DISCOUNTATTOTAL,'') as DISCOUNTATTOTAL, " +
                       "ISNULL(TENDERDECLARATION,'') as TENDERDECLARATION, " +
                       "ISNULL(SERIALNUMBER,'') as SERIALNUMBER, " +
                       "ISNULL(SALESPERSON,'') as SALESPERSON, " +
                       "ISNULL(OPENDRAWER,'') as OPENDRAWER, " +
                       "ISNULL(ITEMNOTONFILE,'') as ITEMNOTONFILE, " +
                       "ISNULL(NEGATIVESALESLINE,'') as NEGATIVESALESLINE, " +
                       "ISNULL(NUMPADENTRYSTARTSINDECIMALS, 0) as NUMPADENTRYSTARTSINDECIMALS, " +
                       "ISNULL(SAFEDROPUSESDENOMINATION, 0) as SAFEDROPUSESDENOMINATION, " +
                       "ISNULL(SAFEDROPREVUSESDENOMINATION, 0) as SAFEDROPREVUSESDENOMINATION, " +
                       "ISNULL(BANKDROPUSESDENOMINATION, 0) as BANKDROPUSESDENOMINATION, " +
                       "ISNULL(BANKDROPREVUSESDENOMINATION, 0) as BANKDROPREVUSESDENOMINATION, " +
                       "ISNULL(TENDERDECLUSESDENOMINATION, 0) as TENDERDECLUSESDENOMINATION, " +
                       "ISNULL(NUMPADAMOUNTOFDECIMALS, 2) as NUMPADAMOUNTOFDECIMALS, " +
                       "ISNULL(MAXIMUMQTY, 0) AS  MAXIMUMQTY, " +
                       "ISNULL(MAXIMUMPRICE, 0) AS  MAXIMUMPRICE, " +
                       "ISNULL(POLLINGINTERVAL, 0) AS POLLINGINTERVAL, " +
                       "ISNULL(SYNCTRANSACTIONS, 0) AS SYNCTRANSACTIONS, " +
                       "ISNULL(ALWAYSASKFORPASSWORD, 0) AS ALWAYSASKFORPASSWORD, " +
                       "ISNULL(PRICEDECIMALPLACES,'') AS PRICEDECIMALPLACES, " +
                       "ISNULL(SKIPHOSPITALITYTABLEVIEW, 0) as SKIPHOSPITALITYTABLEVIEW " +
                       "ISNULL(CUSTOMERREQUIREDONRETURN, 0) as CUSTOMERREQUIREDONRETURN" +
                       "ISNULL(KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT, 0) as KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT" +
                       "from POSFUNCTIONALITYPROFILE " +
                       "where DATAAREAID = @DATAAREAID and PROFILEID = @ID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                result = Execute<FunctionalityProfile>(entry, cmd, CommandType.Text, PopulateFunctionalityProfile);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        private static void PopulateFunctionalityProfile(IDataReader dr, FunctionalityProfile profile)
        {
            profile.ID = (string)dr["PROFILEID"];
            profile.Text = (string)dr["NAME"];
            profile.AggregateItems = (AggregateItemsModes)dr["AGGREGATEITEMS"];
            profile.AggregatePayments = ((byte)dr["AGGREGATEPAYMENTS"] != 0);
            profile.TsCentralTableServer = ((byte)dr["TSCENTRALTABLESERVER"] != 0);
            profile.CentralTableServer = (string)dr["CENTRALTABLESERVER"];
            profile.CentralTableServerPort = Convert.ToInt32(String.IsNullOrEmpty((string)dr["CENTRALTABLESERVERPORT"]) ? "0" : (string)dr["CENTRALTABLESERVERPORT"]); // Not sure why this is NVarchar(50) in the database
            profile.TsCustomer = ((byte)dr["TSCUSTOMER"] != 0);
            profile.TsStaff = ((byte)dr["TSSTAFF"] != 0);
            profile.TsSuspendRetrieveTransactions = ((byte)dr["TSSUSPENDRETRIEVETRANSACTIONS"] != 0);
            profile.LogLevel = (FunctionalityProfile.LogTraceLevel)(int)dr["LOGLEVEL"];
            profile.AggregateItemsForPrinting = ((byte)dr["AGGREGATEITEMSFORPRINTING"] != 0);
            profile.ShowStaffListAtLogon = ((byte)dr["SHOWSTAFFLISTATLOGON"] != 0);
            profile.LimitStaffListToStore = ((byte)dr["LIMITSTAFFLISTTOSTORE"] != 0);
            profile.AllowSalesIfDrawerIsOpen = ((byte)dr["ALLOWSALESIFDRAWERISOPEN"] != 0);
            profile.StaffBarcodeLogon = ((byte)dr["STAFFBARCODELOGON"] != 0);
            profile.StaffCardLogon = ((byte)dr["STAFFCARDLOGON"] != 0);
            profile.MustKeyInPriceIfZero = ((byte)dr["MUSTKEYINPRICEIFZERO"] != 0);
            //profile.MinimumPasswordLength = (int)dr["MINIMUMPASSWORDLENGTH"];
            profile.IsHospitality = ((byte)dr["ISHOSPITALITYPROFILE"] != 0);
            profile.InfocodeStartOfTransaction = (string)dr["STARTOFTRANSACTION"];
            profile.InfocodeEndOfTransaction = (string)dr["ENDOFTRANSACTION"];
            profile.InfocodeTenderDecl = (string)dr["TENDERDECLARATION"];
            profile.InfocodeItemNotFound = (string)dr["ITEMNOTONFILE"];
            profile.InfocodeItemDiscount = (string)dr["MARKDOWN"];
            profile.InfocodeTotalDiscount = (string)dr["DISCOUNTATTOTAL"];
            profile.InfocodePriceOverride = (string)dr["OVERRIDEPRICE"];
            profile.InfocodeReturnItem = (string)dr["NEGATIVESALESLINE"];
            profile.InfocodeReturnTransaction = (string)dr["REFUNDSALE"];
            profile.InfocodeVoidItem = (string)dr["VOIDISPRESSED"];
            profile.InfocodeVoidPayment = (string)dr["VOIDPAYMENT"];
            profile.InfocodeVoidTransaction = (string)dr["VOIDTRANSACTION"];
            profile.InfocodeAddSalesPerson = (string)dr["SALESPERSON"];
            profile.InfocodeOpenDrawer = (string)dr["OPENDRAWER"];
            profile.EntryStartsInDecimals = ((byte)dr["NUMPADENTRYSTARTSINDECIMALS"] != 0);
            profile.DecimalsInNumpad = (int)dr["NUMPADAMOUNTOFDECIMALS"];
            profile.SafeDropUsesDenomination = ((byte)dr["SAFEDROPUSESDENOMINATION"] != 0);
            profile.SafeDropRevUsesDenomination = ((byte)dr["SAFEDROPREVUSESDENOMINATION"] != 0);
            profile.BankDropUsesDenomination = ((byte)dr["BANKDROPUSESDENOMINATION"] != 0);
            profile.BankDropRevUsesDenomination = ((byte)dr["BANKDROPREVUSESDENOMINATION"] != 0);
            profile.TenderDeclUsesDenomination = ((byte)dr["TENDERDECLUSESDENOMINATION"] != 0);
            profile.CustomerRequiredOnReturn = AsBool(dr["CUSTOMERREQUIREDONRETURN"]);
            profile.KeepDailyJournalOpenAfterPrintingReceipt = AsBool(dr["KEEPDAILYJOURNALOPENAFTERPRINTINGRECEIPT"]);
            profile.PollingInterval = (int)dr["POLLINGINTERVAL"];
            profile.MaximumQTY = (decimal)dr["MAXIMUMQTY"];
            profile.MaximumPrice = (decimal)dr["MAXIMUMPRICE"];
            //profile.AlwaysAskForPassword = ((byte)dr["ALWAYSASKFORPASSWORD"] != 0);
            profile.SyncTransactions = ((byte)dr["SYNCTRANSACTIONS"] != 0);
            profile.PriceDecimalPlaces = (string)dr["PRICEDECIMALPLACES"];
            profile.SkipHospitalityTableView = ((byte)dr["SKIPHOSPITALITYTABLEVIEW"] != 0);

        }

        /// <summary>
        /// Get selected StoreServerProfile from database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">StoreServerProfileId</param>
        /// <returns>StoreServerProfile</returns>
        public virtual SiteServiceProfile GetSelectedStoreServerProfileList(IConnectionManager entry, RecordIdentifier id)
        {
            List<SiteServiceProfile> result;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select PROFILEID, ISNULL(NAME,'') as NAME," +
                       "ISNULL(AOSINSTANCE,'') as AOSINSTANCE," +
                       "ISNULL(AOSSERVER,'') as AOSSERVER," +
                       "ISNULL(AOSPORT,'0') as AOSPORT," +
                       "ISNULL(CENTRALTABLESERVER,'') as CENTRALTABLESERVER," +
                       "ISNULL(CENTRALTABLESERVERPORT,'0') as CENTRALTABLESERVERPORT," +
                       "ISNULL(USERNAME,'') as USERNAME," +
                       "ISNULL(PASSWORD,'') as PASSWORD," +
                       "ISNULL(COMPANY,'') as COMPANY," +
                       "ISNULL(DOMAIN,'') as DOMAIN,ISNULL(AXVERSION,-1) as AXVERSION," +
                       "ISNULL(CONFIGURATION,'') as CONFIGURATION," +
                       "ISNULL(LANGUAGE,'') as LANGUAGE," +
                       "ISNULL(TSCUSTOMER,0) as TSCUSTOMER," +
                       "ISNULL(TSSTAFF,0) as TSSTAFF," +
                       "ISNULL(TSINVENTORYLOOKUP,0) as TSINVENTORYLOOKUP, " +
                       "ISNULL(ISSUEGIFTCARDOPTION,0) as ISSUEGIFTCARDOPTION, " +
                       "ISNULL(USEGIFTCARDS,0) as USEGIFTCARDS, " +
                       "ISNULL(USECENTRALSUSPENSION,0) as USECENTRALSUSPENSION, " +
                       "ISNULL(USERCONFIRMATION,0) as USERCONFIRMATION, " +
                       "ISNULL(USECREDITVOUCHERS,0) as USECREDITVOUCHERS, " +
                       "ISNULL(CUSTOMERADDDEFAULTTAXGROUPNAME, '') AS CUSTOMERADDDEFAULTTAXGROUPNAME, " +
                       "ISNULL(CUSTOMERADDDEFAULTTAXGROUP, '') AS CUSTOMERADDDEFAULTTAXGROUP, " +
                       "ISNULL(CASHCUSTOMERSETTING, 0) AS CASHCUSTOMERSETTING " +
                       "from POSTRANSACTIONSERVICEPROFILE " +
                       "where DATAAREAID = @DATAAREAID and PROFILEID = @ID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", (string)id);

                result = Execute<SiteServiceProfile>(entry, cmd, CommandType.Text, PopulateStoreServerProfile);
            }

            return (result.Count > 0) ? result[0] : null;
        }

        private static void PopulateStoreServerProfile(IDataReader dr, SiteServiceProfile profile)
        {
            profile.ID = (string)dr["PROFILEID"];
            profile.Text = (string)dr["NAME"];
            profile.AosInstance = (string)dr["AOSINSTANCE"];
            profile.AosServer = (string)dr["AOSSERVER"];
            if (profile.AosServer != string.Empty)
            {
                profile.UseAxTransactionServices = true;
            }
            try
            {
                profile.AosPort = Convert.ToInt32((string)dr["AOSPORT"]);
            }
            catch (Exception)
            {
                profile.AosPort = 0;
            }

            profile.SiteServiceAddress = (string)dr["CENTRALTABLESERVER"];

            try
            {
                profile.SiteServicePortNumber = Convert.ToInt32((string)dr["CENTRALTABLESERVERPORT"]);
            }
            catch (Exception)
            {
                profile.SiteServicePortNumber = 0;
            }

            profile.UserName = (string)dr["USERNAME"];
            profile.Password = (string)dr["PASSWORD"];
            profile.Company = (string)dr["COMPANY"];
            profile.Domain = (string)dr["DOMAIN"];
            profile.AxVersion = (int)dr["AXVERSION"];
            profile.Configuration = (string)dr["CONFIGURATION"];
            profile.Language = (string)dr["LANGUAGE"];

            profile.CheckCustomer = ((byte)dr["TSCUSTOMER"] != 0);
            profile.CheckStaff = ((byte)dr["TSSTAFF"] != 0);
            profile.UseInventoryLookup = ((byte)dr["TSINVENTORYLOOKUP"] != 0);
            profile.IssueGiftCardOption = (SiteServiceProfile.IssueGiftCardOptionEnum)((int)dr["ISSUEGIFTCARDOPTION"]);
            profile.UseGiftCards = ((byte)dr["USEGIFTCARDS"] != 0);
            profile.UseCentralSuspensions = ((byte)dr["USECENTRALSUSPENSION"] != 0);
            profile.UserConfirmation = ((byte)dr["USERCONFIRMATION"] != 0);
            profile.UseCreditVouchers = ((byte)dr["USECREDITVOUCHERS"] != 0);
            profile.NewCustomerDefaultTaxGroup = (string)dr["CUSTOMERADDDEFAULTTAXGROUP"];
            profile.NewCustomerDefaultTaxGroupName = (string)dr["CUSTOMERADDDEFAULTTAXGROUPNAME"];
            profile.CashCustomerSetting = (SiteServiceProfile.CashCustomerSettingEnum)dr["CASHCUSTOMERSETTING"];
        }
    }
}
