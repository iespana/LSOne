using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Reports;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.Reports;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSRetail.SiteManager.Plugins.ReportViewer;

namespace LSOne.ViewPlugins.ReportViewer.Controls
{
    public partial class ReportParameterControl : UserControl
    {
        public event EventHandler ViewReport;

        Dictionary<string,ProcedureParameter> parameterSet;
        Dictionary<string, ParameterPair> pairedParameterSet; // In this set we have paired parameters so that for example StoreID and StoreName stand as one parameter.

        DateTimePicker startDateControl;
        DateTimePicker endDateControl;
        DateTimePicker startTimeControl;
        DateTimePicker endTimeControl;

        DualDataComboBox statementControl;
        DualDataComboBox storeControl;

        public ReportParameterControl()
        {
            InitializeComponent();
        }

        public bool ForceShowReport { get; set; }

        private ParameterPair AddToPair(ProcedureParameter parameter, string pairKey, string pairName)
        {
            ParameterPair pair;

            if (!pairedParameterSet.ContainsKey(pairName))
            {
                pair = new ParameterPair();

                pairedParameterSet.Add(pairName, pair);
            }
            else
            {
                pair = pairedParameterSet[pairName];
            }

            pair[pairKey] = parameter;

            pair.Name = pairName;

            return pair;
        }

        private ParameterPair AddToPair(ProcedureParameter parameter, bool primary, string pairName)
        {
            ParameterPair pair;

            if (!pairedParameterSet.ContainsKey(pairName))
            {
                pair = new ParameterPair();

                pairedParameterSet.Add(pairName, pair);
            }
            else
            {
                pair = pairedParameterSet[pairName];
            }

            if (primary)
            {
                pair.Primary = parameter;
            }
            else
            {
                pair.Secondary = parameter;
            }

            pair.Name = pairName;

            return pair;
        }


        private List<DataEntity> GetStores()
        {
            List<DataEntity> entities;

            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                entities = new List<DataEntity>();

                Store store = Providers.StoreData.Get(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID);

                if (store != null)
                {
                    entities.Add(store);
                }
            }
            else
            {
                entities = Providers.StoreData.GetList(PluginEntry.DataModel);
            }

            return entities;
        }


        

        private List<DataEntity> GetTerminals()
        {
            List<DataEntity> entities;

            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                entities = new List<DataEntity>(Providers.TerminalData.GetTerminals(PluginEntry.DataModel, PluginEntry.DataModel.CurrentStoreID));
            }
            else
            {
                entities = new List<DataEntity>(Providers.TerminalData.GetList(PluginEntry.DataModel)); 
            }

            return entities;
        }

        

        internal void SetParameterSet(RecordIdentifier reportID, Dictionary<string,ProcedureParameter> parameterSet,Dictionary<string, ProcedureParameter> preSetValues)
        {
            ParameterPair pair;
            string parameterName;
            int controlCount = 0;
            Label label;
            Control selector = null;
            DataEntitySelectionList selectionList = null;
            int selectedIndex = 0;
            DateTime tmp;

            this.parameterSet = parameterSet;
            pairedParameterSet = new Dictionary<string, ParameterPair>();

            hostPanel.Controls.Clear();

            foreach(ProcedureParameter parameter in parameterSet.Values)
            {
                //MessageDialog.Show("Param: " + parameter.Name.ToUpper());
                parameterName = parameter.Name.ToUpperInvariant();
                
                switch (parameterName)
                {
                    case "@GROUPBYPERIOD":
                        parameter.Value = "3";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, true, parameterName);

                        pair.LabelText = Properties.Resources.GroupBy;
                        pair.Priority = 100;
                        break;

                    case "@CUSTOMERID":
                    case "@CUSTOMERNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@CUSTOMERID"), "@CUSTOMER");

                        pair.LabelText = Properties.Resources.Customer + ":";
                        pair.Priority = 5;
                        break;

                    case "@STOREID":
                    case "@STORENAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@STOREID"), "@STORE");

                        pair.LabelText = Properties.Resources.Store + ":";
                        pair.Priority = 10;

                        break;

                    case "@STORESID":
                    case "@STORESNAME":
                        parameter.Value = new string[] { };
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@STORESID"), "@STORES");
                        pair.LabelText = Properties.Resources.Stores;
                        pair.Priority = 20;
                        
                        break;

                    case "@STAFFID": //TODO Remove this
                        parameter.Value = "101";
                        parameter.DataType = SqlDbType.NVarChar;

  
                        pair = AddToPair(parameter, true, "@STAFF");
                        pair.LabelText = Properties.Resources.Staff;
                        pair.Priority = 50;

                        break;

                    case "@USERID":
                    case "@USERNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@USERID"), "@USER");
                        pair.LabelText = Properties.Resources.Staff;
                        pair.Priority = 50;

                        break;

                    case "@USERSID":
                    case "@USERSNAME":
                        parameter.Value = new string[] {  };
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@USERSID"), "@USERS");
                        pair.LabelText = Properties.Resources.Staff;
                        pair.Priority = 50;

                        break;

                    case "@TERMINALID":
                    case "@TERMINALNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@TERMINALID"), "@TERMINAL");

                        pair.LabelText = Properties.Resources.Terminal;
                        pair.Priority = 30;

                        break;

                    case "@TERMINALSID":
                    case "@TERMINALSNAME":
                        parameter.Value = new string[] {};
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@TERMINALSID"), "@TERMINALS");

                        pair.LabelText = Properties.Resources.Terminals + ":";
                        pair.Priority = 30;

                        break;

                    case "@VENDORSID":
                    case "@VENDORSNAME":
                        parameter.Value = new string[] { };
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@VENDORSID"), "@VENDORS");

                        pair.LabelText = Properties.Resources.Vendors + ":";
                        pair.Priority = 40;

                        break;

                    case "@VENDORID":
                    case "@VENDORNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@VENDORID"), "@VENDOR");

                        pair.LabelText = Properties.Resources.Vendor + ":";
                        pair.Priority = 40;

                        break;


                    case "@RETAILITEMID":
                    case "@RETAILITEMNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@RETAILITEMID"), "@RETAILITEM");

                        pair.LabelText = Properties.Resources.RetailItem + ":";
                        pair.Priority = 40;

                        break;

                    case "@RETAILGROUPSID":
                    case "@RETAILGROUPSNAME":
                        parameter.Value = new string[] { };
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@RETAILGROUPSID"), "@RETAILGROUPS");

                        pair.LabelText = Properties.Resources.RetailGroups + ":";
                        pair.Priority = 42;

                        break;

                    case "@RETAILGROUPID":
                    case "@RETAILGROUPNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@RETAILGROUPID"), "@RETAILGROUP");

                        pair.LabelText = Properties.Resources.RetailGroup + ":";
                        pair.Priority = 42;

                        break;

                    case "@RETAILDEPARTMENTSID":
                    case "@RETAILDEPARTMENTSNAME":
                        parameter.Value = new string[] { };
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@RETAILDEPARTMENTSID"), "@RETAILDEPARTMENTS");

                        pair.LabelText = Properties.Resources.RetailDepartments + ":";
                        pair.Priority = 44;

                        break;

                    case "@RETAILDEPARTMENTID":
                    case "@RETAILDEPARTMENTNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@RETAILDEPARTMENTID"), "@RETAILDEPARTMENT");

                        pair.LabelText = Properties.Resources.RetailDepartment + ":";
                        pair.Priority = 44;

                        break;

                    case "@SPECIALGROUPSID":
                    case "@SPECIALGROUPSNAME":
                        parameter.Value = new string[] { };
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@SPECIALGROUPSID"), "@SPECIALGROUPS");

                        pair.LabelText = Properties.Resources.SpecialGroups + ":";
                        pair.Priority = 46;

                        break;

                    case "@SPECIALGROUPID":
                    case "@SPECIALGROUPNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@SPECIALGROUPID"), "@SPECIALGROUP");

                        pair.LabelText = Properties.Resources.SpecialGroup + ":";
                        pair.Priority = 46;

                        break;

                    case "@STATEMENTID":
                    case "@STATEMENTNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@STATEMENTID"), "@STATEMENT");
                        //pair = AddToPair(parameter, false, "@STATEMENT");

                        pair.LabelText = Properties.Resources.Statement + ":";
                        pair.Priority = 100;

                        break;

                    case "@TAXGROUPSID":
                    case "@TAXGROUPSNAME":
                        parameter.Value = new string[] { };
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@TAXGROUPSID"), "@TAXGROUPS");

                        pair.LabelText = Properties.Resources.TaxGroups + ":";
                        pair.Priority = 48;

                        break;

                    case "@TAXGROUPID":
                    case "@TAXGROUPNAME":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        pair = AddToPair(parameter, (parameterName == "@TAXGROUPID"), "@TAXGROUP");

                        pair.LabelText = Properties.Resources.TaxGroup + ":";
                        pair.Priority = 48;

                        break;

                    case "@STARTDATE":
                        parameter.Value = DateTime.Now.Date;
                        parameter.DataType = SqlDbType.DateTime;

                        pair = AddToPair(parameter, true, parameterName);

                        pair.LabelText = Properties.Resources.StartDate;
                        pair.Priority = 1;

                        break;

                    case "@ENDDATE":
                        tmp = DateTime.Now.Date;

                        parameter.Value = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59, 999);
                        parameter.DataType = SqlDbType.DateTime;

                        pair = AddToPair(parameter, true, parameterName);

                        pair.LabelText = Properties.Resources.EndDate;
                        pair.Priority = 3;

                        break;

                    case "@STARTTIME":
                        parameter.Value = new DateTime(DateTime.Now.Year,DateTime.Now.Month, DateTime.Now.Day,0,0,0,DateTimeKind.Unspecified);
                        parameter.DataType = SqlDbType.DateTime;

                        pair = AddToPair(parameter, true, parameterName);

                        pair.LabelText = Properties.Resources.StartTime;
                        pair.Priority = 2;
                        break;

                    case "@ENDTIME":
                        parameter.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, DateTimeKind.Unspecified);
                        parameter.DataType = SqlDbType.DateTime;

                 
                        pair = AddToPair(parameter, true, parameterName);

                        pair.LabelText = Properties.Resources.EndTime;
                        pair.Priority = 4;
                        break;

                    case "@GOODSRECEIVINGID":
                    case "@GOODSRECEIVINGVENDORID":
                    case "@GOODSRECEIVINGVENDORNAME":
                    case "@GOODSRECEIVINGSTATUS":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        if (parameterName == "@GOODSRECEIVINGID")
                        {
                            pair = AddToPair(parameter, true, "@GOODSRECEIVING");
                        }
                        else
                        {
                            pair = AddToPair(parameter, parameterName, "@GOODSRECEIVING");
                        }

                        pair.LabelText = Properties.Resources.GoodsReceivingDocument + ":";
                        pair.Priority = 50;
                        break;

                    case "@PURCHASEORDERID":
                    case "@PURCHASEORDERSTOREID":
                    case "@PURCHASEORDERSTORENAME":
                    case "@PURCHASEORDERVENDORID":
                    case "@PURCHASEORDERVENDORNAME":
                    case "@PURCHASEORDERSTATUS":
                    case "@PURCHASEORDERDELIVERYDATE":
                        parameter.Value = "";
                        parameter.DataType = SqlDbType.NVarChar;

                        if (parameterName == "@PURCHASEORDERID")
                        {
                            pair = AddToPair(parameter, true, "@PURCHASEORDER");

                        }
                        else
                        {
                            pair = AddToPair(parameter, parameterName, "@PURCHASEORDER");
                        }

                        pair.LabelText = Properties.Resources.PurchaseOrder + ":";
                        pair.Priority = 50;

                        break;

                    default:
                        // Check if its a enum.
                        var reportEnumProvider = Providers.ReportEnumValueData;

                        List<ReportEnumValue> enumValues = reportEnumProvider.GetEnumValues(PluginEntry.DataModel, reportID, parameterName.Replace("@", ""));

                        if (enumValues.Count > 0)
                        {
                            parameter.Value = -1;
                            parameter.DataType = SqlDbType.Int;

                            pair = AddToPair(parameter, true, parameterName);

                            pair.LabelText = enumValues[0].Label.EndsWith(":") ? enumValues[0].Label : enumValues[0].Label + ":";
                            pair.Priority = 1000;

                            pair.ExtraData = enumValues;
                        }
                        else
                        {
                            if (parameterName.ToUpperInvariant().EndsWith("NAME"))
                            {
                                enumValues = reportEnumProvider.GetEnumValues(PluginEntry.DataModel, reportID, parameterName.Left(parameterName.Length - 4).Replace("@", ""));

                                if (enumValues.Count > 0)
                                {
                                    parameter.Value = "";
                                    parameter.DataType = SqlDbType.NVarChar;

                                    pair = AddToPair(parameter, false, parameterName.Left(parameterName.Length - 4));

                                    pair.Priority = 1000;

                                    pair.ExtraData = enumValues;
                                }
                                else
                                {
                                    MessageDialog.Show(Properties.Resources.UnknownParameter + parameter.Name.ToUpperInvariant());
                                }
                            }
                            else
                            {
                                MessageDialog.Show(Properties.Resources.UnknownParameter + parameter.Name.ToUpperInvariant());
                            }
                        }

                        break;
                }

                if (preSetValues.ContainsKey(parameterName))
                {
                    parameter.Value = preSetValues[parameterName].Value;
                }
            }

            var orderedParams = from p in pairedParameterSet.Values
                                orderby p.Priority
                                select p;

            foreach (ParameterPair paramPair in orderedParams)
            {
                controlCount++;

                label = new Label();
                //label.Location = new System.Drawing.Point(controlCount % 2 == 1 ? 1 : 300, (((controlCount - 1) / 2) * 30) + 8);

                if (controlCount > (pairedParameterSet.Count + 1) / 2)
                {
                    label.Location = new System.Drawing.Point(310, ((((controlCount - 1) - ((pairedParameterSet.Count + 1) / 2))) * 30) + 8);
                }
                else
                {
                    label.Location = new System.Drawing.Point(1, (((controlCount - 1)) * 30) + 8);
                }
     
                label.Size = new System.Drawing.Size(160, 20);
                label.TabIndex = (controlCount * 2) - 2;
                label.Text = paramPair.LabelText;
                label.TextAlign = System.Drawing.ContentAlignment.TopRight;

                selector = null;

                switch (paramPair.Name)
                {
                    case "@GROUPBYPERIOD":
                        selector = new ComboBox();

                        ((ComboBox)selector).DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                        ((ComboBox)selector).FormattingEnabled = true;
                        ((ComboBox)selector).Size = new Size(150, 21);

                        ((ComboBox)selector).Items.Add(Properties.Resources.Day);
                        ((ComboBox)selector).Items.Add(Properties.Resources.Month);
                        ((ComboBox)selector).Items.Add(Properties.Resources.Quarter);
                        ((ComboBox)selector).Items.Add(Properties.Resources.Year);

                        selectedIndex = 3;
                        
                        ((ComboBox)selector).SelectedIndexChanged += new EventHandler(GroupByPeriod_SelectedIndexChanged);

                        break;


                    case "@STAFFID": //TODO Remove this
                        break;

                    case "@STATEMENT":
                        selector = new DualDataComboBox();
                        statementControl = (DualDataComboBox)selector;

                        ((DualDataComboBox)selector).MaxLength = 32767;
                        ((DualDataComboBox)selector).SelectedData = null;
                        ((DualDataComboBox)selector).SkipIDColumn = true;
                        ((DualDataComboBox)selector).Size = new Size(150, 21);

                        //((DualDataComboBox)selector).DropDown += new DropDownEventHandler(DualDataComboBox_DropDown);
                        ((DualDataComboBox)selector).SelectedDataChanged += new EventHandler(SingleComboSelector_SelectedDataChanged);

                        ((DualDataComboBox)selector).RequestData += StatementSelector_RequestData;

                        if ((string)paramPair.Primary.Value != "")
                        {
                            ((DualDataComboBox)selector).SelectedData = new DataEntity((string)paramPair.Primary.Value, (string)paramPair.Secondary.Value);
                        }

                        if (storeControl == null || storeControl.SelectedData == null || storeControl.SelectedData.ID == "" || startDateControl == null || endDateControl == null)
                        {
                            ((DualDataComboBox)selector).Enabled = false;
                        }
                        else
                        {
                            ((DualDataComboBox)selector).Enabled = true;
                        }

                       

                        break;

                    case "@STORE":
                    case "@USER":
                    case "@TERMINAL":
                    case "@CUSTOMER":
                    case "@VENDOR":
                    case "@RETAILITEM":
                    case "@RETAILGROUP":
                    case "@RETAILDEPARTMENT":
                    case "@SPECIALGROUP":
                    case "@TAXGROUP":
                    case "@PURCHASEORDER":
                    case "@GOODSRECEIVING":
                        selector = new DualDataComboBox();

                        ((DualDataComboBox)selector).MaxLength = 32767;
                        ((DualDataComboBox)selector).SelectedData = null;
                        ((DualDataComboBox)selector).SkipIDColumn = true;
                        ((DualDataComboBox)selector).Size = new Size(150, 21);
                        ((DualDataComboBox)selector).ShowDropDownOnTyping = true;

                        if (paramPair.Name == "@STORE")
                        {
                            storeControl = (DualDataComboBox)selector;
                        }

                        if ((string)paramPair.Primary.Value != "")
                        {

                            ((DualDataComboBox)selector).SelectedData = new DataEntity((string)paramPair.Primary.Value,

                               paramPair.Secondary == null ? (string)paramPair.Primary.Value:(string)paramPair.Secondary.Value);

                            if (paramPair.Name == "@STORE")
                            {
                                if(statementControl != null && startDateControl != null || endDateControl != null)
                                {
                                    statementControl.Enabled = true;
                                }
                            }
                        }
                        else
                        {
                            ((DualDataComboBox)selector).SelectedData = new DataEntity("", "");
                        }

                        ((DualDataComboBox)selector).SelectedDataChanged += new EventHandler(SingleComboSelector_SelectedDataChanged);

                        if (paramPair.Name == "@CUSTOMER")
                        {
                            ((DualDataComboBox)selector).DropDown += new DropDownEventHandler(CustomerComboSelector_DropDown);
                        }
                        else if (paramPair.Name == "@RETAILITEM")
                        {
                            ((DualDataComboBox)selector).DropDown += new DropDownEventHandler(RetailItemComboSelector_DropDown);
                        }
                        else
                        {
                            ((DualDataComboBox)selector).RequestData += SingleComboSelector_RequestData;
                        }

                        if (paramPair.Name == "@PURCHASEORDER")
                        {
                            ((DualDataComboBox)selector).FormatData += new DropDownFormatDataHandler(PurchaseOrderFormatter);
                        }
                        else if (paramPair.Name == "@GOODSRECEIVING")
                        {
                            ((DualDataComboBox)selector).FormatData += new DropDownFormatDataHandler(GoodsReceivingFormatter);
                        }

                        break;

                    case "@STORES":
                    case "@USERS":
                    case "@TERMINALS":
                    case "@VENDORS":
                    case "@RETAILGROUPS":
                    case "@RETAILDEPARTMENTS":
                    case "@SPECIALGROUPS":
                    case "@TAXGROUPS":
                        selector = new DualDataComboBox();
                        List<DataEntity> entities;

                        ((DualDataComboBox)selector).MaxLength = 32767;
                        ((DualDataComboBox)selector).SelectedData = null;
                        ((DualDataComboBox)selector).SkipIDColumn = true;
                        ((DualDataComboBox)selector).Size = new Size(150, 21);

                        ((DualDataComboBox)selector).DropDown += new DropDownEventHandler(DualDataComboBox_DropDown);
                        ((DualDataComboBox)selector).SelectedDataChanged += new EventHandler(DualDataComboBox_SelectedDataChanged);

                        if (paramPair.Name == "@STORES")
                        {
                            entities = GetStores();
                        }
                        else if (paramPair.Name == "@USERS")
                        {
                            entities = Providers.POSUserData.GetList(PluginEntry.DataModel);
                        }
                        else if (paramPair.Name == "@TERMINALS")
                        {
                            entities = GetTerminals();
                        }
                        else if (paramPair.Name == "@VENDORS")
                        {
                            entities = Providers.VendorData.GetList(PluginEntry.DataModel);
                        }
                        else if (paramPair.Name == "@RETAILGROUPS")
                        {
                            entities = Providers.RetailGroupData.GetList(PluginEntry.DataModel);
                        }
                        else if (paramPair.Name == "@RETAILDEPARTMENTS")
                        {
                            entities = Providers.RetailDepartmentData.GetList(PluginEntry.DataModel);
                        }
                        else if (paramPair.Name == "@SPECIALGROUPS")
                        {
                            entities = Providers.SpecialGroupData.GetList(PluginEntry.DataModel);
                        }
                        else
                        {
                            entities = Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel);
                        }

                        selectionList = new DataEntitySelectionList(entities);

                        // We only select all by default if the parameter was not preselected by a context
                        if (((string[])paramPair.Primary.Value).Length == 0)
                        {
                            selectionList.SelectAll();
                        }
                        else
                        {
                            selectionList.SelectSome((string[])paramPair.Primary.Value);
                        }

                        ((DualDataComboBox)selector).SelectedData = selectionList;

                        break;

                    case "@STARTDATE":
                        selector = new DateTimePicker();

                        ((DateTimePicker)selector).Format = System.Windows.Forms.DateTimePickerFormat.Short;
                        ((DateTimePicker)selector).Value = DateTime.Now.Date;
                        selector.Size = new System.Drawing.Size(150, 20);
                        ((DateTimePicker)selector).ValueChanged += new EventHandler(DateControl_ValueChanged);

                        startDateControl = (DateTimePicker)selector;

                        break;

                    case "@ENDDATE":
                        selector = new DateTimePicker();
                        tmp = DateTime.Now.Date;
                        ((DateTimePicker)selector).Format = System.Windows.Forms.DateTimePickerFormat.Short;
                        ((DateTimePicker)selector).Value = new DateTime(tmp.Year,tmp.Month,tmp.Day,23,59,59,999);
                        ((DateTimePicker)selector).ValueChanged += new EventHandler(DateControl_ValueChanged);
                        selector.Size = new System.Drawing.Size(150, 20);

                        endDateControl = (DateTimePicker)selector;

                        if (startDateControl != null)
                        {
                            endDateControl.MinDate = DateTime.Now.Date;
                        }

                        break;

                    case "@STARTTIME":
                        selector = new DateTimePicker();

                        ((DateTimePicker)selector).Format = System.Windows.Forms.DateTimePickerFormat.Time;
                        ((DateTimePicker)selector).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, DateTimeKind.Unspecified);
                        ((DateTimePicker)selector).ShowUpDown = true;
                        selector.Size = new System.Drawing.Size(150, 20);
                        ((DateTimePicker)selector).ValueChanged += new EventHandler(DateControl_ValueChanged);

                        startTimeControl = (DateTimePicker)selector;

                        break;

                    case "@ENDTIME":
                        selector = new DateTimePicker();

                        ((DateTimePicker)selector).Format = System.Windows.Forms.DateTimePickerFormat.Time;
                        ((DateTimePicker)selector).ShowUpDown = true;
                        ((DateTimePicker)selector).Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59, DateTimeKind.Unspecified);;
                        ((DateTimePicker)selector).ValueChanged += new EventHandler(DateControl_ValueChanged);
                        selector.Size = new System.Drawing.Size(150, 20);

                        endTimeControl = (DateTimePicker)selector;

                        if (startTimeControl != null)
                        {
                            endTimeControl.MinDate = DateTime.Now.Date;
                        }

                        break;

                    default:
                        // If we got here then we most likely have a enum
                        if (paramPair.ExtraData is List<ReportEnumValue>)
                        {
                            selector = new ComboBox();

                            ((ComboBox)selector).DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                            ((ComboBox)selector).FormattingEnabled = true;
                            ((ComboBox)selector).Size = new Size(150, 21);

                            foreach (ReportEnumValue value in (List<ReportEnumValue>)paramPair.ExtraData)
                            {
                                ((ComboBox)selector).Items.Add(value);
                            }

                            selectedIndex = 0;

                            ((ComboBox)selector).SelectedIndexChanged += new EventHandler(Enum_SelectedIndexChanged);
                        }

                        break;
                }

                hostPanel.Controls.Add(label);

                int deltaY = 4;

                if (selector != null)
                {
                    if (selector is DualDataComboBox)
                    {
                        deltaY = 5;
                    }

                    if (controlCount > (pairedParameterSet.Count + 1) / 2)
                    {
                        selector.Location = new System.Drawing.Point(473, ((((controlCount - 1) - ((pairedParameterSet.Count + 1) / 2))) * 30) + deltaY);
                    }
                    else
                    {
                        selector.Location = new System.Drawing.Point(163, (((controlCount - 1)) * 30) + deltaY);
                    }

                    selector.TabIndex = label.TabIndex = (controlCount * 2) - 1;
                    selector.Tag = new WeakReference(paramPair);

                    if (selector is DualDataComboBox && ((DualDataComboBox)selector).SelectedData is DataEntitySelectionList)
                    {
                        DualDataComboBox_SelectedDataChanged(selector, EventArgs.Empty);
                    }
                    else if (selector is ComboBox)
                    {
                        ((ComboBox)selector).SelectedIndex = selectedIndex;
                    }

                    hostPanel.Controls.Add(selector);
                }
            }

            hostPanel.Height = this.Height = (((pairedParameterSet.Count + 1) / 2) * 30) + 5;

            // If report has no parameters then we just display it as is.
        
            CheckEnabled();

            if ((parameterSet.Count == 0 || (ForceShowReport && btnViewReport.Enabled)) && ViewReport != null)
            {
                ViewReport(null, EventArgs.Empty);
            }
        }


        void GoodsReceivingFormatter(object sender, DropDownFormatDataArgs e)
        {
            if (e.Data == null || ((IDataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = ((GoodsReceivingDocument)e.Data).VendorName + " - " + ((GoodsReceivingDocument)e.Data).StatusText;
            }
        }

        void PurchaseOrderFormatter(object sender, DropDownFormatDataArgs e)
        {
            if (e.Data == null || ((IDataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = ((PurchaseOrder)e.Data).DeliveryDate.ToShortDateString() + " - " + ((PurchaseOrder)e.Data).VendorName;
            }
        }

        

        void CustomerComboSelector_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.Customers, textInitallyHighlighted);
        }

        void RetailItemComboSelector_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)((DualDataComboBox)sender).SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailItems, textInitallyHighlighted);
        }

        void StatementSelector_RequestData(object sender, EventArgs e)
        {
            DateTime fromDate = new DateTime(startDateControl.Value.Year, startDateControl.Value.Month, startDateControl.Value.Day, 0, 0, 0);
            DateTime toDate = new DateTime(endDateControl.Value.Year, endDateControl.Value.Month, endDateControl.Value.Day, 23, 59, 59);

            ((DualDataComboBox)sender).SetData(Providers.StatementInfoData.GetPostedStatementHeaders(PluginEntry.DataModel, storeControl.SelectedData.ID, fromDate, toDate), null); 
        }

        void SingleComboSelector_RequestData(object sender, EventArgs e)
        {
            ParameterPair pair = (ParameterPair)((WeakReference)((DualDataComboBox)sender).Tag).Target;

            switch (pair.Name)
            {
                case "@STORE":
                    ((DualDataComboBox)sender).SetData(Providers.StoreData.GetList(PluginEntry.DataModel,""),null);
                    break;
                case "@USER":
                    //TODO re-wire this when POS User and Users are merged
                    ((DualDataComboBox)sender).SetData(Providers.POSUserData.GetList(PluginEntry.DataModel), null);
                    break;
                case "@TERMINAL":
                    ((DualDataComboBox)sender).SetData(Providers.TerminalData.GetList(PluginEntry.DataModel), null);
                    break;

                case "@VENDOR":
                    ((DualDataComboBox)sender).SetData(Providers.VendorData.GetList(PluginEntry.DataModel), null);
                    break;

                case "@RETAILGROUP":
                    ((DualDataComboBox)sender).SetData(Providers.RetailGroupData.GetList(PluginEntry.DataModel), null);
                    break;

                case "@RETAILDEPARTMENT":
                    ((DualDataComboBox)sender).SetData(Providers.RetailDepartmentData.GetList(PluginEntry.DataModel), null);
                    break;

                case "@SPECIALGROUP":
                    ((DualDataComboBox)sender).SetData(Providers.SpecialGroupData.GetList(PluginEntry.DataModel), null);
                    break;

                case "@TAXGROUP":
                    ((DualDataComboBox)sender).SetData(Providers.SalesTaxGroupData.GetList(PluginEntry.DataModel), null);
                    break;

                case "@PURCHASEORDER":

                    ((DualDataComboBox)sender).SetWidth(600);

                    ((DualDataComboBox)sender).SetHeaders(new string[] { 
                        Properties.Resources.PurchaseOrderID, 
                        Properties.Resources.Store,
                        Properties.Resources.Vendor,
                        Properties.Resources.Status,
                        Properties.Resources.DeliveryDate},
                        new int[] { 0, 1, 2, 3, 4 });

                    ((DualDataComboBox)sender).SetData(Providers.PurchaseOrderData.GetPurchaseOrders(PluginEntry.DataModel,PurchaseOrderSorting.DeliveryDate,false,false).Cast<DataEntity>(), null);

                    break;

                case "@GOODSRECEIVING":

                    ((DualDataComboBox)sender).SetWidth(450);

                    ((DualDataComboBox)sender).SetHeaders(new string[] { 
                        Properties.Resources.ID, 
                        Properties.Resources.Store,
                        Properties.Resources.Vendor,
                        Properties.Resources.Status},
                        new int[] { 0, 1, 2, 3 });

                    ((DualDataComboBox)sender).SetData(Providers.GoodsReceivingDocumentData.GetGoodsReceivingDocuments(PluginEntry.DataModel, GoodsReceivingDocumentSorting.VendorName,false).Cast<DataEntity>(), null);

                    break;

            }
        }

        void Enum_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParameterPair pair = (ParameterPair)((WeakReference)((ComboBox)sender).Tag).Target;

            pair.Primary.Value = ((ReportEnumValue)(((ComboBox)sender).SelectedItem)).EnumValue;

            if (pair.Secondary != null)
            {
                pair.Secondary.Value = ((ReportEnumValue)(((ComboBox)sender).SelectedItem)).Text;
            }

            CheckEnabled();
        }

        void GroupByPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParameterPair pair = (ParameterPair)((WeakReference)((ComboBox)sender).Tag).Target;

            pair.Primary.Value = (((ComboBox)sender).SelectedIndex).ToString();
        }

        void SingleComboSelector_SelectedDataChanged(object sender, EventArgs e)
        {
            ParameterPair pair = (ParameterPair)((WeakReference)((DualDataComboBox)sender).Tag).Target;

            pair.Primary.Value = (string)((DualDataComboBox)sender).SelectedData.ID;

            if (pair.Name == "@PURCHASEORDER")
            {
                PurchaseOrder po = (PurchaseOrder)((DualDataComboBox)sender).SelectedData;
               
                if (pair["@PURCHASEORDERSTOREID"] != null)
                {
                    pair["@PURCHASEORDERSTOREID"].Value = (string)po.StoreID;
                }
                if (pair["@PURCHASEORDERSTORENAME"] != null)
                {
                    pair["@PURCHASEORDERSTORENAME"].Value = po.StoreName;
                }
                if (pair["@PURCHASEORDERVENDORID"] != null)
                {
                    pair["@PURCHASEORDERVENDORID"].Value = (string) po.VendorID;
                }
                if (pair["@PURCHASEORDERVENDORNAME"] != null)
                {
                    pair["@PURCHASEORDERVENDORNAME"].Value = po.VendorName;
                }
                if (pair["@PURCHASEORDERSTATUS"] != null)
                {
                    pair["@PURCHASEORDERSTATUS"].Value = po.PurchaseStatusText;
                }
                if (pair["@PURCHASEORDERDELIVERYDATE"] != null)
                {
                    pair["@PURCHASEORDERDELIVERYDATE"].Value = po.DeliveryDate.ToShortDateString();
                }

            }
                          
            else if (pair.Name == "@GOODSRECEIVING")
            {
                GoodsReceivingDocument po = (GoodsReceivingDocument)((DualDataComboBox)sender).SelectedData;

                pair["@GOODSRECEIVINGVENDORID"].Value = (string)po.VendorID;
                pair["@GOODSRECEIVINGVENDORNAME"].Value = po.VendorName;
                pair["@GOODSRECEIVINGSTATUS"].Value = po.StatusText;
            }
            else if(pair.Name == "@STORE")
            {
                if (pair.Secondary != null)
                {
                    pair.Secondary.Value = ((DualDataComboBox)sender).SelectedData.Text;
                }

                if(statementControl != null)
                {
                    statementControl.SelectedData = null;

                    if (startDateControl != null || endDateControl != null)
                    {
                        statementControl.Enabled = true;
                    }
                }
            }
            else
            {
                if (pair.Secondary != null)
                {
                    pair.Secondary.Value = ((DualDataComboBox)sender).SelectedData.Text;
                }
            }

            CheckEnabled();
        }

        void DualDataComboBox_SelectedDataChanged(object sender, EventArgs e)
        {
            List<DataEntity> selectedItems;
            DataEntitySelectionList selectionList;
            ParameterPair pair = (ParameterPair)((WeakReference)((DualDataComboBox)sender).Tag).Target;

            if (((DualDataComboBox)sender).SelectedData != null)
            {
                selectionList = (DataEntitySelectionList)((DualDataComboBox)sender).SelectedData;

                selectedItems = selectionList.GetSelectedItems();

                List<string> names = new List<string>();
                List<string> ids = new List<string>();

                pair.Primary.Value = new string[]{};
                pair.Secondary.Value = new string[] {};

                foreach (DataEntity item in selectedItems)
                {
                    ids.Add((string)item.ID);
                    names.Add(item.Text);
                }

                pair.Primary.Value = ids.ToArray();
                pair.Secondary.Value = names.ToArray();
            }

            CheckEnabled();
        }

        void DualDataComboBox_DropDown(object sender, DropDownEventArgs e)
        {
            DataEntitySelectionList list = ((DualDataComboBox)sender).SelectedData as DataEntitySelectionList;
            if (list != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel(list);
            }
        }

        void DateControl_ValueChanged(object sender, EventArgs e)
        {
            DateTime tmp;
            ParameterPair pair = (ParameterPair)((WeakReference)((DateTimePicker)sender).Tag).Target;

            if (sender == startDateControl)
            {
                if (endDateControl.Value < startDateControl.Value)
                {
                    endDateControl.Value =  new DateTime(startDateControl.Value.Year,startDateControl.Value.Month,startDateControl.Value.Day,23,59,59,999);
                }

                endDateControl.MinDate = startDateControl.Value;

                pair.Primary.DataType = SqlDbType.DateTime;
                pair.Primary.Value = startDateControl.Value.Date;
            }
            else if(sender == endDateControl)
            {
                tmp = endDateControl.Value;

                pair.Primary.DataType = SqlDbType.DateTime;
                pair.Primary.Value = new DateTime(tmp.Year, tmp.Month, tmp.Day, 23, 59, 59, 999);
            }
            if (sender == startTimeControl)
            {
                if (endTimeControl.Value < startTimeControl.Value)
                {
                    endTimeControl.Value = startTimeControl.Value;
                }

                endTimeControl.MinDate = startTimeControl.Value;

                pair.Primary.DataType = SqlDbType.DateTime;
                pair.Primary.Value = startTimeControl.Value;
            }
            else if (sender == endTimeControl)
            {
                pair.Primary.DataType = SqlDbType.DateTime;
                pair.Primary.Value = endTimeControl.Value;
            }

            if (statementControl != null)
            {
                statementControl.SelectedData = null;

                if (startDateControl != null || endDateControl != null)
                {
                    statementControl.Enabled = true;
                }
            }

            CheckEnabled();
        }

        void CheckEnabled()
        {
            bool enabled = true;

            foreach (Control control in hostPanel.Controls)
            {
                if (control is DualDataComboBox)
                {
                    if (((DualDataComboBox)control).SelectedData == null)
                    {
                        enabled = false;
                    }
                    else
                    {
                        if (((DualDataComboBox)control).SelectedData is DataEntitySelectionList)
                        {
                            // We have a multi selector with checkboxes
                            enabled = ((DataEntitySelectionList)(((DualDataComboBox)control).SelectedData)).HasSelection;
                        }
                        else
                        {
                            // We have a single selector
                            enabled = ((DataEntity)(((DualDataComboBox)control).SelectedData)).ID != "";
                        }
                    }
                }
                else if (control is ComboBox)
                {
                    enabled = (((ComboBox)control).SelectedIndex >= 0);
                }

                if (enabled == false)
                {
                    break;
                }
            }

            btnViewReport.Enabled = enabled;
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            if (ViewReport != null)
            {
                ViewReport(null, EventArgs.Empty);
            }
        }
    }
}
