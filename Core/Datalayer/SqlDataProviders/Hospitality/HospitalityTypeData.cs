using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Hospitality.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class HospitalityTypeData : SqlServerDataProviderBase, IHospitalityTypeData
    {
        #region sql select string
        private static string BaseListItemSelectString
        {
            get
            {
                return
                    @"select h.RESTAURANTID, 
                             h.SALESTYPE, 
                             h.SEQUENCE, 
                             ISNULL(h.DESCRIPTION,'') as DESCRIPTION, 
                             ISNULL(r.NAME,'') as RESTAURANTNAME,
                             ISNULL(s.DESCRIPTION, '') as SALESTYPEDESCRIPTION 
                      from HOSPITALITYTYPE h 
                      left outer join RBOSTORETABLE r on h.DATAAREAID = r.DATAAREAID and r.STOREID = h.RESTAURANTID 
                      left join SALESTYPE s on s.CODE = h.SALESTYPE and s.DATAAREAID = h.DATAAREAID ";
            }
        }

        private static string BaseSelectString
        {
            get
            {
                return "select " +
                "h.RESTAURANTID, " +
                "h.SEQUENCE, " +
                "h.SALESTYPE, " +
                    // TODO: add graphical layout here
                "ISNULL(h.DESCRIPTION,'') as DESCRIPTION, " +
                "ISNULL(h.OVERVIEW,0) as OVERVIEW, " +
                "ISNULL(h.UPDATETABLEFROMPOS,0) as UPDATETABLEFROMPOS, " +
                "ISNULL(h.REQUESTNOOFGUESTS,0) as REQUESTNOOFGUESTS, " +
                "ISNULL(h.STATIONPRINTING,0) as STATIONPRINTING, " +
                "ISNULL(h.ACCESSTOOTHERRESTAURANT,'') as ACCESSTOOTHERRESTAURANT, " +
                "ISNULL(h.POSLOGONMENUID,'') as POSLOGONMENUID, " +
                "ISNULL(h.ALLOWNEWENTRIES,0) as ALLOWNEWENTRIES, " +
                "ISNULL(h.TIPSAMTLINE1,'') as TIPSAMTLINE1, " +
                "ISNULL(h.TIPSAMTLINE2,'') as TIPSAMTLINE2, " +
                "ISNULL(h.TIPSTOTALLINE,'') as TIPSTOTALLINE, " +
                "ISNULL(h.STAYINPOSAFTERTRANS,0) as STAYINPOSAFTERTRANS, " +
                "ISNULL(h.SENDVOIDEDTOSTATION, 1) AS SENDVOIDEDTOSTATION, " +
                "ISNULL(h.SENDTRANSFERSTOSTATION, 1) AS SENDTRANSFERSTOSTATION, " +
                "ISNULL(h.SENDSUSPENSIONSTOSTATION, 1) AS SENDSUSPENSIONSTOSTATION, " +
                "ISNULL(h.SENDORDERNOTOSTATION, 1) AS SENDORDERNOTOSTATION, " +
                "ISNULL(h.TIPSINCOMEACC1,'') as TIPSINCOMEACC1, " +
                "ISNULL(h.TIPSINCOMEACC2,'') as TIPSINCOMEACC2, " +
                "ISNULL(h.NOOFDINEINTABLES,0) as NOOFDINEINTABLES, " +
                "ISNULL(h.TABLEBUTTONPOSMENUID,'') as TABLEBUTTONPOSMENUID, " +
                "ISNULL(h.TABLEBUTTONDESCRIPTION,0) as TABLEBUTTONDESCRIPTION, " +
                "ISNULL(h.TABLEBUTTONSTAFFDESCRIPTION,0) as TABLEBUTTONSTAFFDESCRIPTION, " +
                "ISNULL(h.STAFFTAKEOVERINTRANS,0) as STAFFTAKEOVERINTRANS, " +
                "ISNULL(h.MANAGERTAKEOVERINTRANS,0) as MANAGERTAKEOVERINTRANS, " +
                "ISNULL(h.VIEWSALESSTAFF,0) as VIEWSALESSTAFF, " +
                "ISNULL(h.VIEWTRANSDATE,0) as VIEWTRANSDATE, " +
                "ISNULL(h.VIEWTRANSTIME,0) as VIEWTRANSTIME, " +
                "ISNULL(h.VIEWDELIVERYADDRESS,0) as VIEWDELIVERYADDRESS, " +
                "ISNULL(h.VIEWLISTTOTALS,0) as VIEWLISTTOTALS, " +
                "ISNULL(h.ORDERBY,0) as ORDERBY, " +
                "ISNULL(h.VIEWRESTAURANT,0) as VIEWRESTAURANT, " +
                "ISNULL(h.VIEWGRID,0) as VIEWGRID, " +
                "ISNULL(h.VIEWCOUNTDOWN,0) as VIEWCOUNTDOWN, " +
                "ISNULL(h.VIEWPROGRESSSTATUS,0) as VIEWPROGRESSSTATUS, " +
                "ISNULL(h.DIRECTEDITOPERATION,0) as DIRECTEDITOPERATION, " +
                "ISNULL(h.SETTINGSFROMHOSPTYPE,'') as SETTINGSFROMHOSPTYPE, " +
                "ISNULL(h.SETTINGSFROMSEQUENCE,0) as SETTINGSFROMSEQUENCE, " +
                "ISNULL(h.SHARINGSALESTYPEFILTER,'') as SHARINGSALESTYPEFILTER, " +
                "ISNULL(h.SETTINGSFROMRESTAURANT,'') as SETTINGSFROMRESTAURANT, " +
                "ISNULL(h.GUESTBUTTONS,0) as GUESTBUTTONS, " +
                "ISNULL(h.MAXGUESTBUTTONSSHOWN,0) as MAXGUESTBUTTONSSHOWN, " +
                "ISNULL(h.MAXGUESTSPERTABLE, 12) as MAXGUESTSPERTABLE, " +
                "ISNULL(h.MAXNOOFSPLITS, 5) AS MAXNOOFSPLITS, " +
                "ISNULL(h.SPLITBILLLOOKUPID,'') as SPLITBILLLOOKUPID, " +
                "ISNULL(h.SELECTGUESTONSPLITTING,0) as SELECTGUESTONSPLITTING, " +
                "ISNULL(h.COMBINESPLITLINESACTION,0) as COMBINESPLITLINESACTION, " +
                "ISNULL(h.TRANSFERLINESLOOKUPID,'') as TRANSFERLINESLOOKUPID, " +
                "ISNULL(h.PRINTTRAININGTRANSACTIONS,0) as PRINTTRAININGTRANSACTIONS, " +
                "ISNULL(h.DEFAULTTYPE,0) as DEFAULTTYPE, " +
                "ISNULL(h.LAYOUTID,'') as LAYOUTID, " +
                "ISNULL(h.TOPPOSMENUID,'') as TOPPOSMENUID, " +
                "ISNULL(h.DININGTABLELAYOUTID,'') as DININGTABLELAYOUTID, " +
                "ISNULL(h.AUTOMATICJOININGCHECK,'') as AUTOMATICJOININGCHECK," +
                "ISNULL(h.PROMPTFORCUSTOMER, 0) AS PROMPTFORCUSTOMER, " +
                "ISNULL(h.DISPLAYCUSTOMERONTABLE, 0) as DISPLAYCUSTOMERONTABLE, " +
                "ISNULL(s.DESCRIPTION, '') as SALESTYPEDESCRIPTION " +
                "from HOSPITALITYTYPE h " +
                "left join SALESTYPE s on s.CODE = h.SALESTYPE and s.DATAAREAID = h.DATAAREAID ";
            }
        }
        #endregion
        private static string ResolveSort(HospitalityTypeSorting sort, bool backwards)
        {
            string sortString = "";

            switch (sort)
            {
                case HospitalityTypeSorting.Restaurant:
                    sortString = "R.NAME";
                    break;
                case HospitalityTypeSorting.SalesType:
                    sortString = "S.DESCRIPTION";
                    break;
                case HospitalityTypeSorting.Description:
                    sortString = "H.DESCRIPTION";
                    break;
                default:
                    return "";
            }

            return " ORDER BY " + sortString + (backwards ? " DESC" : " ASC");
        }

        private static void PopulateHospitalityType(IDataReader reader, HospitalityType hospitalityType)
        {
            hospitalityType.RestaurantID = (string)reader["RESTAURANTID"];
            hospitalityType.Sequence = (int)reader["SEQUENCE"];
            //hospitalityType.GraphicalLayout = null; // TODO: the excact type of the graphicalLayout has yet to be decided
            hospitalityType.Text = (string)reader["DESCRIPTION"];
            hospitalityType.Overview = (HospitalityType.OverviewEnum)(int)reader["OVERVIEW"];
            hospitalityType.SalesType = (string)reader["SALESTYPE"];
            hospitalityType.UpdateTableFromPOS = ((byte)reader["UPDATETABLEFROMPOS"] != 0);
            hospitalityType.RequestNoOfGuests = ((byte)reader["REQUESTNOOFGUESTS"] != 0);
            hospitalityType.StationPrinting = (HospitalityType.StationPrintingEnum)(int)reader["STATIONPRINTING"];
            hospitalityType.AccessToOtherRestaurant = (string)reader["ACCESSTOOTHERRESTAURANT"];
            hospitalityType.PosLogonMenuID = (string)reader["POSLOGONMENUID"];
            hospitalityType.AllowNewEntries = ((byte)reader["ALLOWNEWENTRIES"] != 0);
            hospitalityType.TipsAmtLine1 = (string)reader["TIPSAMTLINE1"];
            hospitalityType.TipsAmtLine2 = (string)reader["TIPSAMTLINE2"];
            hospitalityType.TipsTotalLine = (string)reader["TIPSTOTALLINE"];
            hospitalityType.StayInPosAfterTrans = ((byte)reader["STAYINPOSAFTERTRANS"] != 0);
            hospitalityType.SendVoidedItemsToStation = ((byte) reader["SENDVOIDEDTOSTATION"] != 0);
            hospitalityType.SendTransfersToStation = ((byte) reader["SENDTRANSFERSTOSTATION"] != 0);
            hospitalityType.SendOrderNoToStation = ((byte)reader["SENDORDERNOTOSTATION"] != 0);
            hospitalityType.SendSuspensionsToStation = ((byte)reader["SENDSUSPENSIONSTOSTATION"] != 0);
            hospitalityType.TipsIncomeAcc1 = (string)reader["TIPSINCOMEACC1"];
            hospitalityType.TipsIncomeAcc2 = (string)reader["TIPSINCOMEACC2"];
            hospitalityType.NoOfDineInTables = (int)reader["NOOFDINEINTABLES"];
            hospitalityType.TableButtonPosMenuID = (string)reader["TABLEBUTTONPOSMENUID"];
            hospitalityType.TableButtonDescription = (HospitalityType.TableButtonDescriptionEnum)(int)reader["TABLEBUTTONDESCRIPTION"];
            hospitalityType.TableButtonStaffDescription = (HospitalityType.TableButtonStaffDescriptionEnum)(int)reader["TABLEBUTTONSTAFFDESCRIPTION"];
            hospitalityType.StaffTakeOverInTrans = (HospitalityType.StaffTakeOverInTransEnum)(int)reader["STAFFTAKEOVERINTRANS"];
            hospitalityType.ManagerTakeOverInTrans = (HospitalityType.ManagerTakeOverInTransEnum)(int)reader["MANAGERTAKEOVERINTRANS"];
            hospitalityType.ViewSalesStaff = ((byte)reader["VIEWSALESSTAFF"] != 0);
            hospitalityType.ViewTransDate = ((byte)reader["VIEWTRANSDATE"] != 0);
            hospitalityType.ViewTransTime = ((byte)reader["VIEWTRANSTIME"] != 0);
            hospitalityType.ViewDeliveryAddress = ((byte)reader["VIEWDELIVERYADDRESS"] != 0);
            hospitalityType.ViewListTotals = ((byte)reader["VIEWLISTTOTALS"] != 0);
            hospitalityType.OrderBy = (HospitalityType.OrderByEnum)(int)reader["ORDERBY"];
            hospitalityType.ViewRestaurant = ((byte)reader["VIEWRESTAURANT"] != 0);
            hospitalityType.ViewGrid = ((byte)reader["VIEWGRID"] != 0);
            hospitalityType.ViewCountDown = ((byte)reader["VIEWCOUNTDOWN"] != 0);
            hospitalityType.ViewProgressStatus = ((byte)reader["VIEWPROGRESSSTATUS"] != 0);
            hospitalityType.DirectEditOperation = (int)reader["DIRECTEDITOPERATION"];
            hospitalityType.SettingsFromHospType = (string)reader["SETTINGSFROMHOSPTYPE"];
            hospitalityType.SettingsFromSequence = (int)reader["SETTINGSFROMSEQUENCE"];
            hospitalityType.SharingSalesTypeFilter = (string)reader["SHARINGSALESTYPEFILTER"];
            hospitalityType.SettingsFromRestaurant = (string)reader["SETTINGSFROMRESTAURANT"];
            hospitalityType.GuestButtons = (HospitalityType.GuestButtonsEnum)(int)reader["GUESTBUTTONS"];
            hospitalityType.MaxGuestButtonsShown = (int)reader["MAXGUESTBUTTONSSHOWN"];
            hospitalityType.MaxGuestsPerTable = (int)reader["MAXGUESTSPERTABLE"];
            hospitalityType.MaxNumberOfSplits = (int)reader["MAXNOOFSPLITS"];
            hospitalityType.SplitBillLookupID = (string)reader["SPLITBILLLOOKUPID"];
            hospitalityType.SelectGuestOnSplitting = ((byte)reader["SELECTGUESTONSPLITTING"] != 0);
            hospitalityType.CombineSplitLinesAction = (HospitalityType.CombineSplitLinesActionEnum)(int)reader["COMBINESPLITLINESACTION"];
            hospitalityType.TransferLinesLookupID = (string)reader["TRANSFERLINESLOOKUPID"];
            hospitalityType.PrintTrainingTransactions = ((byte)reader["PRINTTRAININGTRANSACTIONS"] != 0);
            hospitalityType.DefaultType = ((byte)reader["DEFAULTTYPE"] != 0);
            hospitalityType.LayoutID = (string)reader["LAYOUTID"];
            hospitalityType.TopPosMenuID = (string)reader["TOPPOSMENUID"];
            hospitalityType.DiningTableLayoutID = (string)reader["DININGTABLELAYOUTID"];
            hospitalityType.AutomaticJoiningCheck = ((byte)reader["AUTOMATICJOININGCHECK"] != 0);
            hospitalityType.SalesTypeDescription = (string)reader["SALESTYPEDESCRIPTION"];
            hospitalityType.PromptForCustomer = ((byte)reader["PROMPTFORCUSTOMER"] != 0);
            hospitalityType.DisplayCustomerOnTable = (HospitalityType.CustomerOnTable)(int)reader["DISPLAYCUSTOMERONTABLE"];
        }

        private static void PopulateHospitalityTypeListItem(IDataReader reader, HospitalityTypeListItem hospitalityTypeListItem)
        {
            hospitalityTypeListItem.RestaurantID = (string)reader["RESTAURANTID"];
            hospitalityTypeListItem.RestaurantName = (string)reader["RESTAURANTNAME"];
            hospitalityTypeListItem.SalesType = (string)reader["SALESTYPE"];
            hospitalityTypeListItem.Sequence = (int)reader["SEQUENCE"];
            hospitalityTypeListItem.Text = (string)reader["DESCRIPTION"];
            hospitalityTypeListItem.SalesTypeDescription = (string)reader["SALESTYPEDESCRIPTION"];
        }

        /// <summary>
        /// Gets all HospitalityTypes
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of HospitalityListItems</returns>
        public virtual List<HospitalityTypeListItem> GetHospitalityTypes(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseListItemSelectString +
                    "where h.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<HospitalityTypeListItem>(entry, cmd, CommandType.Text, PopulateHospitalityTypeListItem);
            }
        }

        /// <summary>
        /// Gets all HospitalityTypes with sorting
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">Which field to sort by</param>
        /// <param name="sortBackwards">Sort ascending or descending</param>
        /// <returns>A list of HospitalityListItems</returns>
        public virtual List<HospitalityTypeListItem> GetHospitalityTypes(IConnectionManager entry, HospitalityTypeSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseListItemSelectString +
                    "where h.DATAAREAID = @dataAreaId " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<HospitalityTypeListItem>(entry, cmd, CommandType.Text, PopulateHospitalityTypeListItem);
            }
        }

        /// <summary>
        /// Gets the default hospitality type for the given store
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The id of the store</param>
        /// <returns>A list of HospitalityListItems</returns>
        public virtual string GetDefaultHospitalitySalesTypes(IConnectionManager entry, string storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where RESTAURANTID = @restaurantId and h.DATAAREAID = @dataAreaId and DEFAULTTYPE = 1";

                MakeParam(cmd, "restaurantId", storeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var hospitalityTypes = Execute<HospitalityType>(entry, cmd, CommandType.Text,
                    PopulateHospitalityType);

                // if there is no salestype found set to default, then I simply take the first one found
                if (hospitalityTypes.Count < 1)
                {
                    cmd.CommandText =
                        BaseSelectString +
                        "where RESTAURANTID = @restaurantId and h.DATAAREAID = @dataAreaId";

                    hospitalityTypes = Execute<HospitalityType>(entry, cmd, CommandType.Text, PopulateHospitalityType);
                }

                return hospitalityTypes.Count > 0 ? hospitalityTypes[0].SalesType.ToString() : "";
            }
        }

        /// <summary>
        /// Returns a list if all hospitality types
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of HospitalityType objects</returns>
        public virtual List<HospitalityType> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where h.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<HospitalityType>(entry, cmd, CommandType.Text, PopulateHospitalityType);
            }
        }

        /// <summary>
        /// Gets a hospitality type with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityTypeID">The ID of the hospitality type to get</param>
        /// <returns>A HospitalityType object with the given ID</returns>
        public virtual HospitalityType Get(IConnectionManager entry, RecordIdentifier hospitalityTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where RESTAURANTID = @restaurantId and SALESTYPE = @salesType and SEQUENCE = @sequence and h.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "restaurantId", hospitalityTypeID.PrimaryID);
                MakeParam(cmd, "sequence", hospitalityTypeID.SecondaryID.PrimaryID);
                MakeParam(cmd, "salesType", hospitalityTypeID.SecondaryID.SecondaryID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var hospitalityTypes = Execute<HospitalityType>(entry, cmd, CommandType.Text,
                    PopulateHospitalityType);

                return hospitalityTypes.Count > 0 ? hospitalityTypes[0] : null;
            }
        }

        /// <summary>
        /// Gets a hospitality type for a given restaurant and sales type ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The ID of the restaurant</param>
        /// <param name="salesTypeID">The ID of the sales type</param>
        /// <returns>A HospitalityType object for the given restaurant and sales type combination</returns>
        public virtual HospitalityType Get(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where RESTAURANTID = @restaurantId and SALESTYPE = @salesType and h.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "restaurantId", restaurantID);
                MakeParam(cmd, "salesType", salesTypeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var hospitalityTypes = Execute<HospitalityType>(entry, cmd, CommandType.Text,
                    PopulateHospitalityType);

                return hospitalityTypes.Count > 0 ? hospitalityTypes[0] : null;
            }
        }
       

        /// <summary>
        /// Checks if a hospitality type with a given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityTypeID">The ID of the hospitality type to check for</param>
        /// <returns>True if the hospitality type exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier hospitalityTypeID)
        {
            return RecordExists(entry, "HOSPITALITYTYPE", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE" }, hospitalityTypeID);
        }

        /// <summary>
        /// Deletes a hospitality type with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityTypeID">The ID of the hospitality type to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier hospitalityTypeID)
        {
            DeleteRecord(entry, "HOSPITALITYTYPE", new[] { "RESTAURANTID", "SEQUENCE", "SALESTYPE" }, hospitalityTypeID, BusinessObjects.Permission.ManageHospitalityTypes);

            // Remove the hospitality type from the kds section station routing table
            Providers.KitchenDisplaySectionStationRoutingData.RemoveHospitalityType(entry, hospitalityTypeID);
        }

        /// <summary>
        /// Returns the next Sequence value for the given hospitality type.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityTypeID">The ID of the hospitality type. Note that only the Restaurant ID field is requiered</param>
        /// <returns>The next Sequence number</returns>
        public virtual int GetNextSequence(IConnectionManager entry, RecordIdentifier hospitalityTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select ISNULL((MAX(SEQUENCE)),0) as MAXSEQUENCE " +
                    "from HOSPITALITYTYPE " +
                    "where RESTAURANTID = @restaurantId and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "restaurantId", hospitalityTypeID.PrimaryID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return (int) entry.Connection.ExecuteScalar(cmd) + 1;
            }
        }

        /// <summary>
        /// Gets a list of hospitality types for a given restaurant
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <returns>A list of all hospitality types for the given restaurant</returns>
        public virtual List<HospitalityTypeListItem> GetHospitalityTypesForRestaurant(IConnectionManager entry, RecordIdentifier restaurantID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseListItemSelectString +
                    "where r.DATAAREAID = @dataAreaId and h.RESTAURANTID = @restaurantId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) restaurantID);

                return Execute<HospitalityTypeListItem>(entry, cmd, CommandType.Text, PopulateHospitalityTypeListItem);
            }
        }

        /// <summary>
        /// Gets a lit of hospitality types for a given restaurant and overview combination. This is commonly used when 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <param name="overview">The type of overview to get</param>
        /// <returns>A list of all hospitality types for the </returns>
        public virtual List<HospitalityTypeListItem> GetHostpitalityTypesForRestaurant(IConnectionManager entry, RecordIdentifier restaurantID, HospitalityType.OverviewEnum overview = HospitalityType.OverviewEnum.ButtonFormat)
        {
            if (overview == HospitalityType.OverviewEnum.Listing)
            {
                return null;
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseListItemSelectString +
                    "where r.DATAAREAID = @dataAreaId and h.RESTAURANTID = @restaurantId and h.OVERVIEW = @overview";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "restaurantId", (string) restaurantID);
                MakeParam(cmd, "overview", (int) overview);

                return Execute<HospitalityTypeListItem>(entry, cmd, CommandType.Text, PopulateHospitalityTypeListItem);
            }
        }

        /// <summary>
        /// Checks if a hospitality type with the given restaurant id and sales type combination exists.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <param name="salesTypeID">The id of the sales type</param>
        /// <returns>True if the given combination exists, false otherwise</returns>
        public virtual bool RestaurantSalesTypeCombinationExists(IConnectionManager entry, RecordIdentifier restaurantID, RecordIdentifier salesTypeID)
        {
            var combinationID = new RecordIdentifier(restaurantID, salesTypeID);

            return RecordExists(entry, "HOSPITALITYTYPE", new[] { "RESTAURANTID", "SALESTYPE" }, combinationID);
        }


        /// <summary>
        /// Saves a hospitality type into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="hospitalityType">The HospitalityType object to save</param>
        public virtual void Save(IConnectionManager entry, HospitalityType hospitalityType)
        {
            var statement = new SqlServerStatement("HOSPITALITYTYPE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageHospitalityTypes);

            if (hospitalityType.DefaultType)
            {
                // Initializing the default value of the rows for the current restaurant to make sure only the current one can be set as default
                statement.StatementType = StatementType.Update;
                statement.AddCondition("RESTAURANTID", (string)hospitalityType.RestaurantID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                // Set default type as "false" for all
                statement.AddField("DEFAULTTYPE", 0, SqlDbType.TinyInt);
                entry.Connection.ExecuteStatement(statement);
            }

            statement = new SqlServerStatement("HOSPITALITYTYPE");
            if (!Exists(entry, hospitalityType.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("RESTAURANTID", (string)hospitalityType.RestaurantID);
                statement.AddKey("SEQUENCE", GetNextSequence(entry, hospitalityType.ID), SqlDbType.Int);
                statement.AddKey("SALESTYPE", (string)hospitalityType.SalesType);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("RESTAURANTID", (string)hospitalityType.RestaurantID);
                statement.AddCondition("SEQUENCE", (int)hospitalityType.Sequence, SqlDbType.Int);
                statement.AddCondition("SALESTYPE", (string)hospitalityType.SalesType);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DESCRIPTION", hospitalityType.Text);
            statement.AddField("OVERVIEW", (int)hospitalityType.Overview, SqlDbType.Int);
            statement.AddField("UPDATETABLEFROMPOS", hospitalityType.UpdateTableFromPOS ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("REQUESTNOOFGUESTS", hospitalityType.RequestNoOfGuests ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PROMPTFORCUSTOMER", hospitalityType.PromptForCustomer ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DISPLAYCUSTOMERONTABLE", (int)hospitalityType.DisplayCustomerOnTable, SqlDbType.TinyInt);
            statement.AddField("STATIONPRINTING", (int)hospitalityType.StationPrinting, SqlDbType.Int);
            statement.AddField("ACCESSTOOTHERRESTAURANT", (string)hospitalityType.AccessToOtherRestaurant);
            statement.AddField("POSLOGONMENUID", (string)hospitalityType.PosLogonMenuID);
            statement.AddField("ALLOWNEWENTRIES", hospitalityType.AllowNewEntries ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TIPSAMTLINE1", hospitalityType.TipsAmtLine1);
            statement.AddField("TIPSAMTLINE2", hospitalityType.TipsAmtLine2);
            statement.AddField("TIPSTOTALLINE", hospitalityType.TipsTotalLine);
            statement.AddField("STAYINPOSAFTERTRANS", hospitalityType.StayInPosAfterTrans ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("SENDVOIDEDTOSTATION", hospitalityType.SendVoidedItemsToStation ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("SENDTRANSFERSTOSTATION", hospitalityType.SendTransfersToStation ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("SENDORDERNOTOSTATION", hospitalityType.SendOrderNoToStation ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("SENDSUSPENSIONSTOSTATION", hospitalityType.SendSuspensionsToStation ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("TIPSINCOMEACC1", (string)hospitalityType.TipsIncomeAcc1);
            statement.AddField("TIPSINCOMEACC2", (string)hospitalityType.TipsIncomeAcc2);
            statement.AddField("NOOFDINEINTABLES", hospitalityType.NoOfDineInTables, SqlDbType.Int);
            statement.AddField("TABLEBUTTONPOSMENUID", (string)hospitalityType.TableButtonPosMenuID);
            statement.AddField("TABLEBUTTONDESCRIPTION", (int)hospitalityType.TableButtonDescription, SqlDbType.Int);
            statement.AddField("TABLEBUTTONSTAFFDESCRIPTION", (int)hospitalityType.TableButtonStaffDescription, SqlDbType.Int);
            statement.AddField("STAFFTAKEOVERINTRANS", (int)hospitalityType.StaffTakeOverInTrans, SqlDbType.Int);
            statement.AddField("MANAGERTAKEOVERINTRANS", (int)hospitalityType.ManagerTakeOverInTrans, SqlDbType.Int);
            statement.AddField("VIEWSALESSTAFF", hospitalityType.ViewSalesStaff ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VIEWTRANSDATE", hospitalityType.ViewTransDate ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VIEWTRANSTIME", hospitalityType.ViewTransTime ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VIEWDELIVERYADDRESS", hospitalityType.ViewDeliveryAddress ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VIEWLISTTOTALS", hospitalityType.ViewListTotals ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ORDERBY", (int)hospitalityType.OrderBy, SqlDbType.Int);
            statement.AddField("VIEWRESTAURANT", hospitalityType.ViewRestaurant ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VIEWGRID", hospitalityType.ViewGrid ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VIEWCOUNTDOWN", hospitalityType.ViewCountDown ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("VIEWPROGRESSSTATUS", hospitalityType.ViewProgressStatus ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DIRECTEDITOPERATION", hospitalityType.DirectEditOperation, SqlDbType.Int);
            statement.AddField("SETTINGSFROMHOSPTYPE", (string)hospitalityType.SettingsFromHospType);
            statement.AddField("SETTINGSFROMSEQUENCE", (int)hospitalityType.SettingsFromSequence, SqlDbType.Int);
            statement.AddField("SHARINGSALESTYPEFILTER", hospitalityType.SharingSalesTypeFilter);
            statement.AddField("SETTINGSFROMRESTAURANT", (string)hospitalityType.SettingsFromRestaurant);
            statement.AddField("GUESTBUTTONS", (int)hospitalityType.GuestButtons, SqlDbType.Int);
            statement.AddField("MAXGUESTBUTTONSSHOWN", hospitalityType.MaxGuestButtonsShown, SqlDbType.Int);
            statement.AddField("MAXGUESTSPERTABLE", hospitalityType.MaxGuestsPerTable, SqlDbType.Int);
            statement.AddField("MAXNOOFSPLITS", hospitalityType.MaxNumberOfSplits, SqlDbType.Int);
            statement.AddField("SPLITBILLLOOKUPID", (string)hospitalityType.SplitBillLookupID);
            statement.AddField("SELECTGUESTONSPLITTING", hospitalityType.SelectGuestOnSplitting ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("COMBINESPLITLINESACTION", (int)hospitalityType.CombineSplitLinesAction, SqlDbType.Int);
            statement.AddField("TRANSFERLINESLOOKUPID", (string)hospitalityType.TransferLinesLookupID);
            statement.AddField("PRINTTRAININGTRANSACTIONS", hospitalityType.PrintTrainingTransactions ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("DEFAULTTYPE", hospitalityType.DefaultType ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("LAYOUTID", (string)hospitalityType.LayoutID);
            statement.AddField("TOPPOSMENUID", (string)hospitalityType.TopPosMenuID);
            statement.AddField("DININGTABLELAYOUTID", (string)hospitalityType.DiningTableLayoutID);
            statement.AddField("AUTOMATICJOININGCHECK", hospitalityType.AutomaticJoiningCheck ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
