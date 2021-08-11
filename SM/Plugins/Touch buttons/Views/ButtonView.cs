using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.OperationButtons;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlDDDataProviders;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.TouchButtons.Properties;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.ViewPlugins.TouchButtons.Views
{
    public partial class ButtonView : ViewBase
    {
        private RecordIdentifier posMenuLineID = RecordIdentifier.Empty;
        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;
        private LookupTypeEnum currentLookupType;
        private PosMenuHeader parentPosMenuHeader;
        private PosStyle parentStyle;        
        private PosOperation noOperation;
        private bool useOperationName;
        private Currency companyCurrency; // Used when looking up cash denominations for amounts

        private TabControl.Tab generalTab;
        private TabControl.Tab fontTab;
        private TabControl.Tab attributesTab;
        private TabControl.Tab imageTab;
        private TabControl.Tab glyphsTopLeft;
        private TabControl.Tab glyphsTopRight;
        private TabControl.Tab glyphsBottomLeft;
        private TabControl.Tab glyphsBottomRight;
        

        private ErrorProvider errorProvider;
        private IEnumerable<IDataEntity> recordBrowsingContext;
        private int selectedTabIndex = -1;
        private DataEntity noSelection;

        public ButtonView(RecordIdentifier posMenuLineID, IEnumerable<IDataEntity> recordBrowsingContext, int selectedTabIndex)
            : this(posMenuLineID, recordBrowsingContext)
        {
            this.selectedTabIndex = selectedTabIndex;
        }

        public ButtonView(RecordIdentifier posMenuLineID, IEnumerable<IDataEntity> recordBrowsingContext)
            : this(posMenuLineID)
        {
            this.recordBrowsingContext = recordBrowsingContext;
        }

        public ButtonView(RecordIdentifier posMenuLineID)
            : this()
        {
            this.posMenuLineID = posMenuLineID;
            ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditPosMenus);            
        }

        public ButtonView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.RecordCursor;

            noOperation = new PosOperation();
            noOperation.ID = RecordIdentifier.Empty;
            noOperation.Text = Properties.Resources.NoOperation;
            posMenuLineCopy = new PosMenuLine();
            useOperationName = false;
            noSelection = new DataEntity("", "");

            errorProvider = new ErrorProvider();

            cmbStyle.AutoSelectOnEmpty = false;
        }

        protected override ViewBase GetRecordCursorView(int cursorIndex)
        {
            if (recordBrowsingContext != null)
            {
                RecordIdentifier id = recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID;

                if (Providers.PosMenuLineData.Exists(PluginEntry.DataModel, id))
                {
                    return new ButtonView(recordBrowsingContext.ElementAt<IDataEntity>(cursorIndex).ID, recordBrowsingContext, tabSheetTabs.SelectedTabIndex);
                }
            }

            return null;
        }

        protected override void GetRecordCursorInfo(ContextBarCursorEventArguments args)
        {
            if (recordBrowsingContext != null)
            {
                args.Position = 0;
                args.Count = recordBrowsingContext.Count<IDataEntity>();

                foreach (IDataEntity entity in recordBrowsingContext)
                {
                    if (entity.ID == posMenuLineID)
                    {
                        return;
                    }

                    args.Position++;
                }
            }
            else
            {
                args.Count = 1;
                args.Position = 0;
            }
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("PosMenuLine", posMenuLineID, Properties.Resources.PosMenuButton, true));
        }

        public string Description
        {
            get
            {
                return Properties.Resources.PosMenuButton + ": " + tbKeyNo.Text + " - " + tbDescription.Text;
            }
        }

        public override string ContextDescription
        {
            get
            {
                return tbDescription.Text;
            }
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.PosMenuButton;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return posMenuLineID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            btnMenuButtonPreview.Visible = false;

            if (!isRevert)
            {
                generalTab = new TabControl.Tab(Properties.Resources.General, ViewPages.PosButtonGridMenuLineGeneralPage.CreateInstance);
                fontTab = new TabControl.Tab(Properties.Resources.Font, ViewPages.PosButtonGridMenuLineFontPage.CreateInstance);
                attributesTab = new TabControl.Tab(Properties.Resources.Attributes, ViewPages.PosButtonGridMenuLineAttributesPage.CreateInstance); 
                imageTab = new TabControl.Tab(Properties.Resources.Image, ViewPages.PosButtonGridMenuLineImagePage.CreateInstance);
                glyphsTopLeft = new TabControl.Tab(Properties.Resources.GlyphsTopLeft, ViewPages.PosButtonGridMenuLineGlyphsTopLeftPage.CreateInstance);
                glyphsTopRight = new TabControl.Tab(Properties.Resources.GlyphsTopRight, ViewPages.PosButtonGridMenuLineGlyphsTopRightPage.CreateInstance);
                glyphsBottomLeft = new TabControl.Tab(Properties.Resources.GlyphsBottomLeft, ViewPages.PosButtonGridMenuLineGlyphsBottomLeftPage.CreateInstance);
                glyphsBottomRight = new TabControl.Tab(Properties.Resources.GlyphsBottomRight, ViewPages.PosButtonGridMenuLineGlyphsBottomRightPage.CreateInstance);

                tabSheetTabs.AddTab(generalTab);
                tabSheetTabs.AddTab(fontTab);
                tabSheetTabs.AddTab(attributesTab);                
                tabSheetTabs.AddTab(imageTab);
                tabSheetTabs.AddTab(glyphsTopLeft);
                tabSheetTabs.AddTab(glyphsTopRight);
                tabSheetTabs.AddTab(glyphsBottomLeft);
                tabSheetTabs.AddTab(glyphsBottomRight);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, posMenuLineID);
            }

            companyCurrency = Providers.CurrencyData.GetCompanyCurrency(PluginEntry.DataModel);

            posMenuLine = Providers.PosMenuLineData.Get(PluginEntry.DataModel, posMenuLineID);
            parentPosMenuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuLine.MenuID);
            parentStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, parentPosMenuHeader.StyleID);

            posMenuLineCopy = PosMenuLine.Clone(posMenuLine);

            tbKeyNo.Text = posMenuLine.KeyNo.ToString();
            tbDescription.Text = posMenuLine.Text;
            cmbOperation.SelectedData = Providers.PosOperationData.Get(PluginEntry.DataModel, posMenuLine.Operation) ?? new DataEntity(RecordIdentifier.Empty, Properties.Resources.NoOperation);
            cmbStyle.SelectedData = noSelection;

            if (posMenuLine.StyleID != "")
            {
                cmbStyle.SelectedData = Providers.PosStyleData.Get(PluginEntry.DataModel, posMenuLine.StyleID);
            }
            
            if (cmbOperation.SelectedData.ID != RecordIdentifier.Empty)
            {
                currentLookupType = ((PosOperation)cmbOperation.SelectedData).LookupType;
                InitParameterSelectedData(true);
            }

            HeaderText = Description;
            
            tabSheetTabs.SetData(isRevert, posMenuLineID, posMenuLine);

            SetPreviewButton(posMenuLine);

            if (selectedTabIndex != -1)
            {
                tabSheetTabs.SelectedTab = tabSheetTabs[selectedTabIndex];
            }

            btnMenuButtonPreview.Visible = true;

            tabSheetTabs.LoadAllTabs();
        }

        protected override bool DataIsModified()
        {
            errorProvider.Clear();

            posMenuLine.Dirty = false;

            bool tabsModified = tabSheetTabs.IsModified();

            posMenuLine.Dirty = posMenuLine.Dirty |
                (tbDescription.Text != posMenuLine.Text) |
                (cmbOperation.SelectedData.ID != posMenuLine.Operation) |
                (posMenuLine.StyleID != null || cmbStyle.SelectedData.ID != posMenuLine.StyleID);           

            if (currentLookupType == LookupTypeEnum.TextInput || currentLookupType == LookupTypeEnum.NumericInput || currentLookupType == LookupTypeEnum.Amount)
            {
                posMenuLine.Dirty = posMenuLine.Dirty | (cmbParameter.Text != posMenuLine.Parameter);
            }
            else if (currentLookupType == LookupTypeEnum.BlankOperations)
            {
                string blankParameter = cmbParameter.SelectedData != null ? (string)cmbParameter.SelectedData.ID : "";
                blankParameter += ";" + cmbParameter2.Text;

                if (blankParameter.Trim() == ";")
                {
                    blankParameter = "";
                }

                posMenuLine.Dirty = posMenuLine.Dirty | (blankParameter != posMenuLine.Parameter);
            }
            else if (currentLookupType == LookupTypeEnum.PosMenuAndButtonGrid)
            {
                string posButtonGrid = cmbParameter.SelectedData != null ? (string)cmbParameter.SelectedData.ID : "";
                posButtonGrid += ";" + (cmbParameter2.SelectedData != null ? cmbParameter2.SelectedData.ID.ToString() : "");

                if (posButtonGrid.Trim() == ";")
                {
                    posButtonGrid = "";
                }

                posMenuLine.Dirty = posMenuLine.Dirty | (posButtonGrid != posMenuLine.Parameter);
            }
            else if (currentLookupType == LookupTypeEnum.StorePaymentTypeAndAmount)
            {
                string paymentType = cmbParameter.SelectedData != null ? (string)cmbParameter.SelectedData.ID : "";
                paymentType += ";" + cmbParameter2.Text;

                if (paymentType.Trim() == ";")
                {
                    paymentType = "";
                }

                posMenuLine.Dirty = posMenuLine.Dirty | (paymentType != posMenuLine.Parameter);
            }
            else if (currentLookupType == LookupTypeEnum.RetailItems)
            {
                // Check for selected barcode
                if (cmbParameter2.SelectedData != null && (cmbParameter2.SelectedData.ID != "" && cmbParameter2.SelectedData.ID != RecordIdentifier.Empty))
                {
                    posMenuLine.Dirty = posMenuLine.Dirty | ( (string)((BarCode)cmbParameter2.SelectedData).ItemBarCode != posMenuLine.Parameter);
                }
                else
                {
                    posMenuLine.Dirty = posMenuLine.Dirty | (cmbParameter.SelectedData != null && (string)cmbParameter.SelectedData.ID != posMenuLine.Parameter);
                }
            }
            else if (currentLookupType == LookupTypeEnum.StorePaymentTypeAndAmount)
            {
                string reasonCodeType = cmbParameter.SelectedData != null ? (string)cmbParameter.SelectedData.ID : "";
                reasonCodeType += ";" + cmbParameter2.Text;

                if (reasonCodeType.Trim() == ";")
                {
                    reasonCodeType = "";
                }

                posMenuLine.Dirty = posMenuLine.Dirty | (reasonCodeType != posMenuLine.Parameter);
            }
            else
            {
                posMenuLine.Dirty = posMenuLine.Dirty | (cmbParameter.SelectedData != null && (string)cmbParameter.SelectedData.ID != posMenuLine.Parameter);
            }

            return tabsModified | posMenuLine.Dirty;
        }

        protected override bool SaveData()
        {
            tabSheetTabs.GetData();

            if (posMenuLine.Dirty)
            {
                if (ParametersContainErrors())
                {
                    return false;
                }

                posMenuLine.Text = tbDescription.Text;
                posMenuLine.Operation = cmbOperation.SelectedData != null ? cmbOperation.SelectedData.ID : 0;
                posMenuLine.StyleID = cmbStyle.SelectedData != null ? cmbStyle.SelectedData.ID : "";

                if (currentLookupType == LookupTypeEnum.Boolean)
                {
                    posMenuLine.Parameter = (string) cmbParameter.SelectedDataID;
                }
                else if (currentLookupType == LookupTypeEnum.NumericInput ||
                         currentLookupType == LookupTypeEnum.TextInput || currentLookupType == LookupTypeEnum.Amount)
                {
                    if (currentLookupType != LookupTypeEnum.TextInput)
                    {
                        if (!string.IsNullOrWhiteSpace(cmbParameter.Text) && !StringIsNumeric(cmbParameter.Text))
                        {
                            errorProvider.SetError(cmbParameter, Properties.Resources.ParameterMustBeNumeric);
                            return false;
                        }
                    }

                    posMenuLine.Parameter = cmbParameter.Text;
                }
                else if (currentLookupType == LookupTypeEnum.BlankOperations)
                {
                    posMenuLine.Parameter = cmbParameter.SelectedData != null
                        ? (string) cmbParameter.SelectedData.ID
                        : "";
                    posMenuLine.Parameter += ";" + cmbParameter2.Text;
                }
                else if (currentLookupType == LookupTypeEnum.PosMenuAndButtonGrid)
                {
                    posMenuLine.Parameter = cmbParameter.SelectedData != null
                        ? (string) cmbParameter.SelectedData.ID
                        : "";
                    posMenuLine.Parameter += ";" +
                                             (cmbParameter2.SelectedData != null
                                                 ? cmbParameter2.SelectedData.ID.ToString()
                                                 : "");
                }
                else if (currentLookupType == LookupTypeEnum.StorePaymentTypeAndAmount)
                {
                    if (!string.IsNullOrWhiteSpace(cmbParameter2.Text) && !StringIsNumeric(cmbParameter2.Text))
                    {
                        errorProvider.SetError(cmbParameter2, Properties.Resources.ParameterMustBeNumeric);
                        return false;
                    }

                    posMenuLine.Parameter = cmbParameter.SelectedData != null
                        ? (string) cmbParameter.SelectedData.ID
                        : "";
                    posMenuLine.Parameter += ";" + cmbParameter2.Text;
                }
                else if (currentLookupType == LookupTypeEnum.RetailItems)
                {
                    if (cmbParameter2.SelectedData.ID != "" &&
                        cmbParameter2.SelectedData.ID != RecordIdentifier.Empty)
                    {
                        posMenuLine.Parameter = (string) ((BarCode) cmbParameter2.SelectedData).ItemBarCode;
                    }
                    else
                    {
                        posMenuLine.Parameter = cmbParameter.SelectedData != null &&
                                                cmbParameter.SelectedData.ID != ""
                            ? cmbParameter.SelectedData.ID.ToString()
                            : "";
                    }
                }
                else if (currentLookupType == LookupTypeEnum.ItemSearch)
                {
                    posMenuLine.Parameter = string.Format("{0};{1}",
                        cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != ""
                            ? cmbParameter.SelectedData.ID.ToString()
                            : "",
                        cmbParameter2.SelectedData != null && cmbParameter2.SelectedData.ID != ""
                            ? cmbParameter2.SelectedData.ID.ToString()
                            : "");
                }
                else if (currentLookupType == LookupTypeEnum.SalesPerson)
                {
                    posMenuLine.Parameter = string.Format("{0};{1}",
                        cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != ""
                            ? (string) cmbParameter.SelectedData.ID
                            : "",
                        cmbParameter2.SelectedData != null && cmbParameter2.SelectedData.ID != ""
                            ? (string) cmbParameter2.SelectedData.ID
                            : "");
                }
                else if (currentLookupType == LookupTypeEnum.LookupJob)
                {
                    lblParameter1.Text = Resources.RunJob;
                    DataProviderFactory.Instance.Register<IDesignData, DesignData, JscTableDesign>();
                    DataProviderFactory.Instance.Register<IInfoData, InfoData, JscInfo>();
                    DataProviderFactory.Instance.Register<IJobData, JobData, JscJob>();
                    DataProviderFactory.Instance
                        .Register<IReplicationActionData, ReplicationActionData, ReplicationAction>();
                    DataProviderFactory.Instance.Register<ILocationData, LocationData, JscLocation>();

                    cmbParameter.SetData(
                        DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobs(PluginEntry.DataModel, false),
                        null);

                    if (posMenuLine.Parameter != "")
                    {
                        JscJob job = DataProviderFactory.Instance.Get<IJobData, JscJob>()
                            .GetJob(PluginEntry.DataModel, new Guid(posMenuLine.Parameter), false);
                        cmbParameter.SelectedData = job;
                    }
                }
                else if (currentLookupType == LookupTypeEnum.Boolean)
                {
                    // Label text depends on the operation being run
                    if ((int) posMenuLine.Operation == (int) POSOperations.PrintZ)
                    {
                        lblParameter1.Text = Properties.Resources.RunEndOfDayLabelText;
                    }
                    else if ((int) posMenuLine.Operation == (int) POSOperations.CustomerOrders)
                    {
                        lblParameter1.Text = Resources.ReserveStock;
                    }
                    else if ((int)posMenuLine.Operation == (int)POSOperations.PriceCheck)
                    {
                        lblParameter1.Text = Resources.ShowInventoryStatus;
                    }
                    else if ((int)posMenuLine.Operation == (int)POSOperations.InventoryLookup)
                    {
                        lblParameter1.Text = Resources.ShowPrice;
                    }

                    string parameterText = string.IsNullOrEmpty(posMenuLine.Parameter) ||
                                           posMenuLine.Parameter == "N"
                        ? Properties.Resources.No
                        : Properties.Resources.Yes;

                    // This way we can alwas assume either "N" or "Y" when dealing with the selected data in the combobox
                    string parameterValue = string.IsNullOrEmpty(posMenuLine.Parameter)
                        ? "N"
                        : posMenuLine.Parameter;

                    cmbParameter.SelectedData = new DataEntity(parameterValue, parameterText);
                }
                else if (currentLookupType == LookupTypeEnum.ReprintReceipt)
                {
                    if (cmbParameter.SelectedData.ID == "" || cmbParameter.SelectedData.ID == RecordIdentifier.Empty)
                    {
                        errorProvider.SetError(cmbParameter, Properties.Resources.ParameterCannotBeEmpty);
                        return false;
                    }
                    if (cmbParameter.SelectedData.ID == "4" && cmbParameter2.Text == "")
                    {
                        errorProvider.SetError(cmbParameter2, Properties.Resources.ParameterCannotBeEmpty);
                        return false;
                    }
                    posMenuLine.Parameter = string.Format("{0};{1}",
                        cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? (string)cmbParameter.SelectedData.ID : "",
                        cmbParameter2.SelectedData != null && cmbParameter2.Text != "" ? (string)cmbParameter2.Text : "");
                }
                else if (currentLookupType == LookupTypeEnum.LastSaleOrReceiptID)
                {
                    if (cmbParameter.SelectedData.ID == "" || cmbParameter.SelectedData.ID == RecordIdentifier.Empty)
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterCannotBeEmpty);
                        return false;
                    }
                    posMenuLine.Parameter = cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? (string)cmbParameter.SelectedData.ID : "";
                }
                else if (currentLookupType == LookupTypeEnum.ReasonCode)
                {
                    if (cmbParameter.SelectedData.ID == "2" && cmbParameter2.Text == "")
                    {
                        errorProvider.SetError(cmbParameter2, Resources.ParameterCannotBeEmpty);
                        return false;
                    }

                    posMenuLine.Parameter = string.Format("{0};{1}",
                        cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? (string)cmbParameter.SelectedData.ID : "",
                        cmbParameter2.SelectedData != null && cmbParameter2.SelectedData.ID != "" ? (string)cmbParameter2.SelectedData.ID : "");
                }
                else if (currentLookupType == LookupTypeEnum.InfocodeOnRequest)
                {
                    if (cmbParameter.SelectedData.ID == "1" && cmbParameter2.Text == "")
                    {
                        errorProvider.SetError(cmbParameter2, Properties.Resources.ParameterCannotBeEmpty);
                        return false;
                    }

                    posMenuLine.Parameter = string.Format("{0};{1}",
                        cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? (string)cmbParameter.SelectedData.ID : "",
                        cmbParameter2.SelectedData != null && cmbParameter2.SelectedData.ID != "" ? (string)cmbParameter2.SelectedData.ID : "");
                }
                else
                {
                    posMenuLine.Parameter = cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != ""
                        ? cmbParameter.SelectedData.ID.ToString()
                        : "";
                }

                if (posMenuLine.Parameter.Trim() == ";")
                {
                    posMenuLine.Parameter = "";
                }

                if (posMenuLine.Parameter.EndsWith(";"))
                {
                    posMenuLine.Parameter = posMenuLine.Parameter.Substring(0, posMenuLine.Parameter.Length - 1);
                }

                if (posMenuLine.Parameter.Length > 500)
                {
                    errorProvider.SetError(cmbParameter2.Visible ? cmbParameter2 : cmbParameter, Resources.ParameterTooLong);
                    return false;
                }

                Providers.PosMenuLineData.Save(PluginEntry.DataModel, posMenuLine);

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PosButtonGridMenuLine", posMenuLine.ID, null);
            }
            return true;
        }

        private bool ParametersContainErrors()
        {
            switch (currentLookupType)
            {
                case  LookupTypeEnum.NumericInput:
                    if (!string.IsNullOrWhiteSpace(cmbParameter.Text) && !StringIsNumeric(cmbParameter.Text))
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterMustBeNumeric);
                        return true;
                    }
                    break;

                case LookupTypeEnum.Amount:
                    if (!string.IsNullOrWhiteSpace(cmbParameter.Text) && !StringIsNumeric(cmbParameter.Text))
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterMustBeNumeric);
                        return true;
                    }
                    break;

                case LookupTypeEnum.StorePaymentTypeAndAmount:
                    if (!string.IsNullOrWhiteSpace(cmbParameter2.Text) && !StringIsNumeric(cmbParameter2.Text))
                    {
                        errorProvider.SetError(cmbParameter2, Resources.ParameterMustBeNumeric);
                        return true;
                    }
                    break;

                case LookupTypeEnum.TaxGroupInfocodes:
                    if (string.IsNullOrWhiteSpace(cmbParameter.Text))
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    break;

                case LookupTypeEnum.SuspendedTransactionTypes:
                    if (string.IsNullOrWhiteSpace(cmbParameter.Text))
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    break;

                case LookupTypeEnum.PosMenuAndButtonGrid:
                    //skip validation for operation parameter if it's the menu for mobile apps
                    if (parentPosMenuHeader.DeviceType == DeviceTypeEnum.MobileInventory)
                    {
                        if (((PosOperation)cmbOperation.SelectedData).ID == ((int)POSOperations.OpenMenu).ToString() && string.IsNullOrWhiteSpace(cmbParameter.Text)) //Open menu
                        {
                            errorProvider.SetError(cmbParameter, Resources.ParameterCannotBeEmpty);
                            return true;
                        }

                        return false;
                    }

                    if (string.IsNullOrWhiteSpace(cmbParameter.Text))
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    else if (string.IsNullOrWhiteSpace(cmbParameter2.Text))
                    {
                        errorProvider.SetError(cmbParameter2, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    break;
                case LookupTypeEnum.SalesPerson:
                    if (string.IsNullOrWhiteSpace(cmbParameter2.Text))
                    {
                        errorProvider.SetError(cmbParameter2, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    break;
                case LookupTypeEnum.LookupJob:
                    if (string.IsNullOrWhiteSpace(cmbParameter.Text))
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    break;
                case LookupTypeEnum.ReasonCode:
                    if ((string)cmbParameter.SelectedDataID == "2" && (string)cmbParameter2.SelectedDataID == "")
                    {
                        errorProvider.SetError(cmbParameter2, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    break;
                case LookupTypeEnum.InfocodeOnRequest:
                    if (cmbParameter.SelectedDataID == "1" && cmbParameter2.Text == "")
                    {
                        errorProvider.SetError(cmbParameter2, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    break;
            }

            return false;
        }

        protected override void OnDelete()
        {
            PluginOperations.DeletePosButtonGridMenuLine(posMenuLineID);
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Resources.ViewPosButtonGridMenus, new ContextbarClickEventHandler(PluginOperations.ShowPosButtonGridMenusListView)), 200);

                if(PluginEntry.Framework.CanRunOperation("ShowImageBank"))
                {
                    arguments.Add(new ContextBarItem(Resources.ImageBank, new ContextbarClickEventHandler(PluginOperations.ShowImageBank)), 300);
                }
            }
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (changeHint == DataEntityChangeType.VariableChanged)
            {
                if (objectName == "PosButtonGridMenuLineUseOperationName")
                {
                    useOperationName = (bool)param;

                    tbDescription.Enabled = !useOperationName;

                    if (useOperationName && currentLookupType != LookupTypeEnum.RetailItems)
                    {
                        tbDescription.Text = cmbOperation.SelectedData.Text;
                    }
                }
            }

            switch (objectName)
            {
                case "PosButtonGridMenuLine":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == posMenuLineID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;

                case "ButtonGridPreviewMenuButton":
                    PosMenuLine copy = (PosMenuLine)param;

                    // Copy from tabs
                    // General
                    posMenuLineCopy.Transparent = copy.Transparent;
                    posMenuLineCopy.StyleID = copy.StyleID;
                    PosStyle posStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, posMenuLineCopy.StyleID);                   

                    posMenuLineCopy.UseHeaderFont = copy.UseHeaderFont;
                    posMenuLineCopy.FontName = copy.FontName;
                    posMenuLineCopy.FontSize = copy.FontSize;
                    posMenuLineCopy.FontBold = copy.FontBold;
                    posMenuLineCopy.FontItalic = copy.FontItalic;
                    posMenuLineCopy.FontStrikethrough = copy.FontStrikethrough;
                    posMenuLineCopy.FontUnderline = copy.FontUnderline;
                    posMenuLineCopy.ForeColor = copy.ForeColor;
                    posMenuLineCopy.FontCharset = copy.FontCharset;                
                    posMenuLineCopy.TextPosition = copy.TextPosition;

                    posMenuLineCopy.UseHeaderAttributes = copy.UseHeaderAttributes;
                    posMenuLineCopy.GradientMode = copy.GradientMode;
                    posMenuLineCopy.BackColor = copy.BackColor;
                    posMenuLineCopy.BackColor2 = copy.BackColor2;
                    posMenuLineCopy.Shape = copy.Shape;

                    // Glyphs
                    // Glyph 1
                    posMenuLineCopy.Glyph = copy.Glyph;
                    posMenuLineCopy.GlyphText = copy.GlyphText;
                    posMenuLineCopy.GlyphTextFont = copy.GlyphTextFont;
                    posMenuLineCopy.GlyphTextFontSize = copy.GlyphTextFontSize;
                    posMenuLineCopy.GlyphTextForeColor = copy.GlyphTextForeColor;
                    posMenuLineCopy.GlyphOffSet = copy.GlyphOffSet;

                    // Glyph 2
                    posMenuLineCopy.Glyph2 = copy.Glyph2;
                    posMenuLineCopy.GlyphText2 = copy.GlyphText2;
                    posMenuLineCopy.GlyphText2Font = copy.GlyphText2Font;
                    posMenuLineCopy.GlyphText2FontSize = copy.GlyphText2FontSize;
                    posMenuLineCopy.GlyphText2ForeColor = copy.GlyphText2ForeColor;
                    posMenuLineCopy.Glyph2OffSet = copy.Glyph2OffSet;

                    // Glyph 3
                    posMenuLineCopy.Glyph3 = copy.Glyph3;
                    posMenuLineCopy.GlyphText3 = copy.GlyphText3;
                    posMenuLineCopy.GlyphText3Font = copy.GlyphText3Font;
                    posMenuLineCopy.GlyphText3FontSize = copy.GlyphText3FontSize;
                    posMenuLineCopy.GlyphText3ForeColor = copy.GlyphText3ForeColor;
                    posMenuLineCopy.Glyph3OffSet = copy.Glyph3OffSet;

                    // Glyph 4
                    posMenuLineCopy.Glyph4 = copy.Glyph4;
                    posMenuLineCopy.GlyphText4 = copy.GlyphText4;
                    posMenuLineCopy.GlyphText4Font = copy.GlyphText4Font;
                    posMenuLineCopy.GlyphText4FontSize = copy.GlyphText4FontSize;
                    posMenuLineCopy.GlyphText4ForeColor = copy.GlyphText4ForeColor;
                    posMenuLineCopy.Glyph4OffSet = copy.Glyph4OffSet;

                    //Image tab
                    posMenuLineCopy.PictureID = copy.PictureID;
                    posMenuLineCopy.ImagePosition = copy.ImagePosition;
                    posMenuLineCopy.UseImageFont = copy.UseImageFont;
                    posMenuLineCopy.ImageFontText = copy.ImageFontText;
                    posMenuLineCopy.ImageFontName = copy.ImageFontName;
                    posMenuLineCopy.ImageFontColor = copy.ImageFontColor;
                    posMenuLineCopy.ImageFontBold = copy.ImageFontBold;
                    posMenuLineCopy.ImageFontItalic = copy.ImageFontItalic;
                    posMenuLineCopy.ImageFontUnderline = copy.ImageFontUnderline;
                    posMenuLineCopy.ImageFontStrikethrough = copy.ImageFontStrikethrough;
                    posMenuLineCopy.ImageFontCharset = copy.ImageFontCharset;
                    posMenuLineCopy.ImageFontSize = copy.ImageFontSize;

                    SetPreviewButton(posMenuLineCopy);
                    break;

            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        private void SetPreviewButton(PosMenuLine posMenuLineInfo)
        {
            PosStyle buttonStyle = OperationButtons.FindStyleForButton(PluginEntry.DataModel, parentStyle, new PosStyle(posMenuLineInfo.StyleID, ""));
            OperationButtons.SetButtonProperties(PluginEntry.DataModel, parentPosMenuHeader, posMenuLineInfo, buttonStyle, btnMenuButtonPreview, 0, false);
        }

        private char GetWindingsGlyphChar(PosMenuLine.GlyphEnum glyph)
        {
            int charNumber = 0;

            switch (glyph)
            {
                case PosMenuLine.GlyphEnum.One:
                    charNumber = 232;
                    break;

                case PosMenuLine.GlyphEnum.Two:
                    charNumber = 238;
                    break;

                case PosMenuLine.GlyphEnum.Three:
                    charNumber = 224;
                    break;

                case PosMenuLine.GlyphEnum.Four:
                    charNumber = 230;
                    break;

                case PosMenuLine.GlyphEnum.Five:
                    charNumber = 236;
                    break;

                case PosMenuLine.GlyphEnum.Six:
                    charNumber = 248;
                    break;

                case PosMenuLine.GlyphEnum.Seven:
                    charNumber = 177;
                    break;

                case PosMenuLine.GlyphEnum.Eight:
                    charNumber = 162;
                    break;

                case PosMenuLine.GlyphEnum.Nine:
                    charNumber = 112;
                    break;

                case PosMenuLine.GlyphEnum.Ten:
                    charNumber = 117;
                    break;

                case PosMenuLine.GlyphEnum.Eleven:
                    charNumber = 118;
                    break;

                case PosMenuLine.GlyphEnum.Twelve:
                    charNumber = 164;
                    break;
            }

            return (char)charNumber;
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");            
        }

        private void cmbOperation_RequestData(object sender, EventArgs e)
        {
            List<PosOperation> operationList = parentPosMenuHeader.DeviceType == DeviceTypeEnum.POS 
                        ? Providers.PosOperationData.GetUserOperations(PluginEntry.DataModel, PosOperationSorting.OperationName, false).OrderBy(x => x.Text).ToList()
                        : Providers.PosOperationData.GetList(PluginEntry.DataModel, OperationTypeEnum.OMNIInventoryOperation).OrderBy(x => x.Text).ToList();

            operationList.Insert(0, noOperation);

            cmbOperation.SetData(operationList, null);
        }

        private void cmbOperation_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (cmbOperation.SelectedData.ID != RecordIdentifier.Empty)
            {          
                currentLookupType = ((PosOperation)cmbOperation.SelectedData).LookupType;
                cmbParameter.Enabled = currentLookupType != LookupTypeEnum.None;

                cmbParameter.Enabled = currentLookupType != LookupTypeEnum.None;

                lblParameter2.Enabled = lblParameter2.Visible = (currentLookupType == LookupTypeEnum.PosMenuAndButtonGrid || 
                                                                 currentLookupType == LookupTypeEnum.BlankOperations || 
                                                                 currentLookupType == LookupTypeEnum.StorePaymentTypeAndAmount ||
                                                                 currentLookupType == LookupTypeEnum.RetailItems ||
                                                                 currentLookupType == LookupTypeEnum.ItemSearch ||
                                                                currentLookupType == LookupTypeEnum.SalesPerson ||
                                                                currentLookupType == LookupTypeEnum.ReprintReceipt ||
                                                                currentLookupType == LookupTypeEnum.ReasonCode ||
                                                                currentLookupType == LookupTypeEnum.InfocodeOnRequest
                                                                );

                cmbParameter2.Enabled = cmbParameter2.Visible = lblParameter2.Enabled;

                cmbParameter.EnableTextBox = (currentLookupType == LookupTypeEnum.TextInput || currentLookupType == LookupTypeEnum.NumericInput || currentLookupType == LookupTypeEnum.Amount);
                cmbParameter.ShowDropDownOnTyping = currentLookupType == LookupTypeEnum.RetailItems;

                cmbParameter.SelectedData = cmbParameter.Enabled ? noSelection : new DataEntity("", "");
                cmbParameter2.SelectedData = new DataEntity("", "");

                switch (currentLookupType)
                {
                    case LookupTypeEnum.Amount:
                        lblParameter1.Text = Properties.Resources.AmountLabelText;
                        break;   

                    case LookupTypeEnum.BlankOperations:
                        lblParameter1.Text = Properties.Resources.BlankOperationLabelText;
                        lblParameter2.Text = Properties.Resources.ParameterLabelText;

                        cmbParameter2.EnableTextBox = true;
                        break;

                    case LookupTypeEnum.ExpenseAccounts:
                        lblParameter1.Text = Properties.Resources.ExpenseAccountLabelText;
                        break;

                    case LookupTypeEnum.IncomeAccounts:
                        lblParameter1.Text = Properties.Resources.IncomeAccountLabelText;
                        break;

                    case LookupTypeEnum.PosMenu:
                        lblParameter1.Text = Properties.Resources.PosMenuLabelText;
                        break;

                    case LookupTypeEnum.PosMenuAndButtonGrid:
                        lblParameter1.Text = Properties.Resources.PosMenuLabelText;
                        lblParameter2.Text = Properties.Resources.ButtonGridLabelText;

                        cmbParameter2.EnableTextBox = false;
                        break;

                    case LookupTypeEnum.RetailItems:
                        lblParameter1.Text = Properties.Resources.ItemLabelText;
                        lblParameter2.Text = Properties.Resources.BarCodeLabelText;

                        cmbParameter2.EnableTextBox = false;
                        break;                    

                    case LookupTypeEnum.StorePaymentTypeAndAmount:
                        lblParameter1.Text = Properties.Resources.PaymentTypeLabelText;
                        lblParameter2.Text = Properties.Resources.AmountLabelText;

                        cmbParameter2.EnableTextBox = true;
                        break;                                     

                    case LookupTypeEnum.StorePaymentTypes:
                        lblParameter1.Text = Properties.Resources.PaymentTypeLabelText;
                        break;

                    case LookupTypeEnum.SuspendedTransactionTypes:
                        lblParameter1.Text = Properties.Resources.SuspensionTypeLabelText;
                        break;

                    case LookupTypeEnum.TaxGroupInfocodes:
                        lblParameter1.Text = Properties.Resources.TaxGroupInfocodeLabelText;
                        break;

                    case LookupTypeEnum.ItemSearch:
                        lblParameter1.Text = Properties.Resources.SearchMode;
                        lblParameter2.Text = Properties.Resources.RetailGroupLabelText;
                        break;

                    case LookupTypeEnum.ManuallyTriggerPeriodicDiscount:
                        lblParameter1.Text = Properties.Resources.PeriodicDiscountLabelText;
                        break;

                    case LookupTypeEnum.SalesPerson:
                        lblParameter1.Text = Properties.Resources.SalesPerson;
                        lblParameter2.Text = Properties.Resources.Hereafter;
                        break;
                    case LookupTypeEnum.LookupJob:
                        lblParameter1.Text = Resources.RunJob;
                        break;
                    case LookupTypeEnum.Boolean:
                        if (int.Parse(cmbOperation.SelectedDataID.StringValue) == (int) POSOperations.PrintZ)
                        {
                            lblParameter1.Text = Properties.Resources.RunEndOfDayLabelText;
                            cmbParameter.SelectedData = new DataEntity("N", Properties.Resources.No);
                        }
                        else if (int.Parse(cmbOperation.SelectedDataID.StringValue) == (int)POSOperations.CustomerOrders)
                        {
                            lblParameter1.Text = Properties.Resources.ReserveStock;
                            cmbParameter.SelectedData = new DataEntity("Y", Properties.Resources.Yes);
                        }
                        else if (int.Parse(cmbOperation.SelectedDataID.StringValue) == (int)POSOperations.PriceCheck)
                        {
                            lblParameter1.Text = Properties.Resources.ShowInventoryStatus;
                            cmbParameter.SelectedData = new DataEntity("N", Properties.Resources.No);
                        }
                        else if (int.Parse(cmbOperation.SelectedDataID.StringValue) == (int)POSOperations.InventoryLookup)
                        {
                            lblParameter1.Text = Properties.Resources.ShowPrice;
                            cmbParameter.SelectedData = new DataEntity("N", Properties.Resources.No);
                        }

                        break;
                    case LookupTypeEnum.ReprintReceipt:
                        lblParameter1.Text = Resources.PrintType;
                        lblParameter2.Text = Resources.CustomText;
                        cmbParameter2.EnableTextBox = true;
                        break;
                    case LookupTypeEnum.LastSaleOrReceiptID:
                        lblParameter1.Text = Resources.ParameterLabelText;
                        break;
                    case LookupTypeEnum.ReasonCode:
                        lblParameter1.Text = Resources.SelectionType;
                        lblParameter2.Text = Resources.ReasonCode;
                        cmbParameter2.EnableTextBox = false;
                        cmbParameter2.Enabled = false;
                        break;
                    case LookupTypeEnum.InfocodeOnRequest:
                        lblParameter1.Text = Resources.TriggerFor;
                        lblParameter2.Text = Resources.Infocode;
                        cmbParameter2.EnableTextBox = false;
                        cmbParameter2.Enabled = false;
                        break;
                    default:
                        lblParameter1.Text = Properties.Resources.ParameterLabelText;

                        cmbParameter2.EnableTextBox = false;
                        break;
                }

            }

            cmbParameter.SkipIDColumn = !(cmbParameter.OnlyDisplayID = currentLookupType == LookupTypeEnum.BlankOperations);

            InitParameterSelectedData(false);

            // We do not overwrite an item sale item name
            if (useOperationName && currentLookupType != LookupTypeEnum.RetailItems)
            {
                tbDescription.Text = cmbOperation.SelectedData.Text;
            }
        }

        private void cmbParameter_RequestData(object sender, EventArgs e)
        {
            switch (currentLookupType)
            {
                case LookupTypeEnum.RetailItems:                    
                    break;

                case LookupTypeEnum.StorePaymentTypes:
                    IEnumerable<StorePaymentMethod> paymentMethods = Providers.StorePaymentMethodData.GetTenderForOperation(PluginEntry.DataModel, DataProviderFactory.Instance.Get<IStoreData, Store>().GetDefaultStoreID(PluginEntry.DataModel), cmbOperation.SelectedData.ID);                    
                    cmbParameter.SetData(paymentMethods, null);                    
                    break;

                case LookupTypeEnum.PosMenu:
                    PosMenuHeaderFilter filter = new PosMenuHeaderFilter();
                    filter.MenuType = (int)MenuTypeEnum.POSButtonGrid;
                    filter.SortBy = PosMenuHeaderSorting.MenuDescription;
                    filter.DeviceType = (int)parentPosMenuHeader.DeviceType;
                    cmbParameter.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, filter), null);
                    break;

                case LookupTypeEnum.TaxGroupInfocodes:
                    cmbParameter.SetData(Providers.InfocodeData.GetTaxGroupInfocodes(PluginEntry.DataModel), null);
                    break;

                case LookupTypeEnum.PosMenuAndButtonGrid:
                    PosMenuHeaderFilter buttonFilter = new PosMenuHeaderFilter();
                    buttonFilter.MenuType = (int)MenuTypeEnum.POSButtonGrid;
                    buttonFilter.SortBy = PosMenuHeaderSorting.MenuDescription;
                    buttonFilter.DeviceType = (int)parentPosMenuHeader.DeviceType;
                    cmbParameter.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, buttonFilter), null);
                    break;

                case LookupTypeEnum.SuspendedTransactionTypes:
                    cmbParameter.SetData(Providers.SuspendedTransactionTypeData.GetList(PluginEntry.DataModel), null);
                    break;

                case LookupTypeEnum.BlankOperations:
                    cmbParameter.SetData(Providers.BlankOperationData.GetBlankOperations(PluginEntry.DataModel), null);                    
                    break;

                case LookupTypeEnum.IncomeAccounts:
                    if (PluginEntry.DataModel.IsHeadOffice)
                    {
                        cmbParameter.SetData(Providers.IncomeExpenseAccountData.GetList(PluginEntry.DataModel, IncomeExpenseAccount.AccountTypeEnum.IncomeAccount), null);
                    }
                    else
                    {
                        cmbParameter.SetData(Providers.IncomeExpenseAccountData.GetListForStore(PluginEntry.DataModel, IncomeExpenseAccount.AccountTypeEnum.IncomeAccount, PluginEntry.DataModel.CurrentStoreID), null);
                    }
                    break;

                case LookupTypeEnum.ExpenseAccounts:
                    if (PluginEntry.DataModel.IsHeadOffice)
                    {
                        cmbParameter.SetData(Providers.IncomeExpenseAccountData.GetList(PluginEntry.DataModel, IncomeExpenseAccount.AccountTypeEnum.ExpenseAccount), null);
                    }
                    else
                    {
                        cmbParameter.SetData(Providers.IncomeExpenseAccountData.GetListForStore(PluginEntry.DataModel, IncomeExpenseAccount.AccountTypeEnum.ExpenseAccount, PluginEntry.DataModel.CurrentStoreID), null);
                    }
                    break;

                case LookupTypeEnum.StorePaymentTypeAndAmount:
                    IEnumerable<StorePaymentMethod> paymentMethods2 = Providers.StorePaymentMethodData.GetTenderForOperation(PluginEntry.DataModel, DataProviderFactory.Instance.Get<IStoreData, Store>().GetDefaultStoreID(PluginEntry.DataModel), cmbOperation.SelectedData.ID);                    
                    cmbParameter.SetData(paymentMethods2, null);
                    break;

                case LookupTypeEnum.Amount:
                    cmbParameter.SetWidth(200);

                    cmbParameter.SetHeaders(new string[] { Properties.Resources.Amount }, new int[] { 1 });

                    List<CashDenominator> denominators = Providers.CashDenominatorData.GetCashDenominators(PluginEntry.DataModel, companyCurrency.ID, 1, false);

                    foreach (CashDenominator cd in denominators)
                    {
                        cd.FormattedAmount = cd.Amount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        cd.Text = cd.FormattedAmount;
                    }

                    cmbParameter.SetData(denominators, null);
                    break;
                case LookupTypeEnum.ItemSearch:
                    {
                        List<DataEntity> list = new List<DataEntity>
                        {
                            new DataEntity(((int)ItemSearchViewModeEnum.Default).ToString(), Properties.Resources.SearchModeDefault),
                            new DataEntity(((int)ItemSearchViewModeEnum.List).ToString(), Properties.Resources.SearchModeList),
                            new DataEntity(((int)ItemSearchViewModeEnum.Images).ToString(), Properties.Resources.SearchModeImages),
                        };
                        cmbParameter.SetData(list, null);
                    }
                    break;

                case LookupTypeEnum.ManuallyTriggerPeriodicDiscount:
                    cmbParameter.SetData(Providers.DiscountOfferData.GetManuallyTriggeredPeriodicDiscounts(PluginEntry.DataModel, DiscountOfferSorting.Description, false), null);
                    break;

                case LookupTypeEnum.SalesPerson:
                    cmbParameter.SetData(Providers.POSUserData.GetList(PluginEntry.DataModel, RecordIdentifier.Empty, UsageIntentEnum.Normal), null);
                    break;
                case LookupTypeEnum.LookupJob:
                    DataProviderFactory.Instance.Register<IDesignData, DesignData, JscTableDesign>();
                    DataProviderFactory.Instance.Register<IInfoData, InfoData, JscInfo>();
                    DataProviderFactory.Instance.Register<IJobData, JobData, JscJob>();
                    DataProviderFactory.Instance
                        .Register<IReplicationActionData, ReplicationActionData, ReplicationAction>();
                    DataProviderFactory.Instance.Register<ILocationData, LocationData, JscLocation>();

                    cmbParameter.SetData(
                        DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobs(PluginEntry.DataModel, false), null);
                    break;

                case LookupTypeEnum.Boolean:                
                    List<DataEntity> yesNo = new List<DataEntity>();
                    yesNo.Add(new DataEntity("Y", Properties.Resources.Yes));
                    yesNo.Add(new DataEntity("N", Properties.Resources.No));
                    cmbParameter.SetData(yesNo, null);                    
                    break;
                case LookupTypeEnum.ReprintReceipt:
                    List<DataEntity> reprintEntities = new List<DataEntity>();
                    reprintEntities.Add(new DataEntity("1", Resources.GiftReceipt));
                    reprintEntities.Add(new DataEntity("2", Resources.CopyLastReceipt));
                    reprintEntities.Add(new DataEntity("3", Resources.TaxFreeReceipt));
                    reprintEntities.Add(new DataEntity("4", Resources.CustomReceipt));
                    cmbParameter.SetData(reprintEntities, null);
                    break;
                case LookupTypeEnum.LastSaleOrReceiptID:
                    List<DataEntity> lastSaleOrReceitIDEntities = new List<DataEntity>();
                    lastSaleOrReceitIDEntities.Add(new DataEntity("1", Resources.LastReceipt));
                    lastSaleOrReceitIDEntities.Add(new DataEntity("2", Resources.SelectAReceiptID));
                    cmbParameter.SetData(lastSaleOrReceitIDEntities, null);
                    break;
                case LookupTypeEnum.ReasonCode:
                    List<DataEntity> reasonTypes = new List<DataEntity>();
                    reasonTypes.Add(new DataEntity("0", Resources.Default));
                    reasonTypes.Add(new DataEntity("1", Resources.List));
                    reasonTypes.Add(new DataEntity("2", Resources.Specific));
                    cmbParameter.SetData(reasonTypes, null);
                    break;
                case LookupTypeEnum.InfocodeOnRequest:
                    List<DataEntity> triggerFor = new List<DataEntity>();
                    triggerFor.Add(new DataEntity("0", Resources.InfocodeOnRequest_Item));
                    triggerFor.Add(new DataEntity("1", Resources.InfocodeOnRequest_Sale));
                    cmbParameter.SetData(triggerFor, null);
                    break;
                case LookupTypeEnum.PrintGroup:
                    cmbParameter.SetData(ItemSaleReportGroupHelper.GetList(), null);
                    break;
                case LookupTypeEnum.TransferRequests:
                case LookupTypeEnum.TransferOrders:
                    List<DataEntity> transferType = new List<DataEntity>();
                    transferType.Add(new DataEntity("Incoming", Resources.Incoming));
                    transferType.Add(new DataEntity("Outgoing", Resources.Outgoing));
                    cmbParameter.SetData(transferType, null);
                    break;
            }            
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            posMenuLineCopy.Text = tbDescription.Text;
            btnMenuButtonPreview.Text = tbDescription.Text;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ButtonGridPreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }

        private void FormatData_Operation(object sender, DropDownFormatDataArgs e)
        {

        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            btnMenuButtonPreview.Dispose();

            base.OnClose();
        }

        private void InitParameterSelectedData(bool runOperationSelectionEvent)
        {
            if (runOperationSelectionEvent)
            {
                cmbOperation_SelectedDataChanged(this, EventArgs.Empty);
            }

            string[] parameters = null;

            switch (currentLookupType)
            {
                case LookupTypeEnum.None:
                    
                    break;

                case LookupTypeEnum.RetailItems:                    
                    if (string.IsNullOrEmpty(posMenuLine.Parameter.Trim()))
                    {
                        cmbParameter.SelectedData = noSelection;
                        cmbParameter2.SelectedData = noSelection;
                        break;
                    }

                    // Search by barcode
                    Dictionary<RetailItemSearchEnum, string> advancedSearchParams = new Dictionary<RetailItemSearchEnum, string>();
                    advancedSearchParams.Add(RetailItemSearchEnum.BarCode, "%" + posMenuLine.Parameter + "%");

                    List<SimpleRetailItem> advSearchResults = Providers.RetailItemData.AdvancedSearch(PluginEntry.DataModel, "", 1, 10, false, SortEnum.Description, advancedSearchParams);

                    // Check only for item id
                    RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, posMenuLine.Parameter);

                    if (item == null && advSearchResults.Count == 0)
                    {
                        cmbParameter.SelectedData = noSelection;
                        cmbParameter2.SelectedData = noSelection;
                    }
                    else if (item == null && advSearchResults.Count > 0)
                    {
                        // The parameter is a barcode, so the advanced search result returns a value
                        cmbParameter.SelectedData = Providers.RetailItemData.Get(PluginEntry.DataModel, advSearchResults[0].ID);
                        cmbParameter2.SelectedData = Providers.BarCodeData.Get(PluginEntry.DataModel, posMenuLine.Parameter);
                    }
                    else if (item != null)
                    {
                        cmbParameter.SelectedData = item;                       
                    }
                    
                    break;

                case LookupTypeEnum.StorePaymentTypes:
                    cmbParameter.SelectedData = Providers.PaymentMethodData.Get(PluginEntry.DataModel, posMenuLine.Parameter);
                    break;

                case LookupTypeEnum.PosMenu:
                    cmbParameter.SelectedData = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuLine.Parameter);
                    break;

                case LookupTypeEnum.TaxGroupInfocodes:
                    cmbParameter.SelectedData = Providers.InfocodeData.Get(PluginEntry.DataModel, posMenuLine.Parameter);
                    break;

                case LookupTypeEnum.PosMenuAndButtonGrid:
                    {
                        parameters = posMenuLine.Parameter.Split(';');
                        cmbParameter.SelectedData = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, parameters[0]);

                        if (parameters.Length == 2)
                        {
                            int buttonGridNo = Convert.ToInt32(parameters[1]);
                            DataEntity data = new DataEntity(buttonGridNo, "Button grid" + buttonGridNo);
                            cmbParameter2.SelectedData = data;
                        }
                    }
                    break;
                
                case LookupTypeEnum.SuspendedTransactionTypes:
                    cmbParameter.SelectedData = Providers.SuspendedTransactionTypeData.Get(PluginEntry.DataModel, posMenuLine.Parameter);
                    break;

                case LookupTypeEnum.BlankOperations:
                    {
                        parameters = posMenuLine.Parameter.Split(';');
                        cmbParameter.SelectedData = Providers.BlankOperationData.Get(PluginEntry.DataModel,
                            parameters[0]);

                        if (parameters.Length == 2)
                        {
                            cmbParameter2.Text = parameters[1];
                        }
                    }
                    break;

                case LookupTypeEnum.IncomeAccounts:
                    cmbParameter.SelectedData = Providers.IncomeExpenseAccountData.Get(PluginEntry.DataModel, posMenuLine.Parameter);
                    break;

                case LookupTypeEnum.ExpenseAccounts:
                    cmbParameter.SelectedData = Providers.IncomeExpenseAccountData.Get(PluginEntry.DataModel, posMenuLine.Parameter);
                    break;

                case LookupTypeEnum.TextInput:
                    cmbParameter.SelectedData = new DataEntity(posMenuLine.Parameter, "");
                    cmbParameter.Text = posMenuLine.Parameter;
                    break;

                case LookupTypeEnum.StorePaymentTypeAndAmount:
                    {
                        parameters = posMenuLine.Parameter.Split(';');
                        cmbParameter.SelectedData = Providers.PaymentMethodData.Get(PluginEntry.DataModel, parameters[0]);

                        if (parameters.Length == 2)
                        {
                            cmbParameter2.Text = parameters[1];
                        }
                    }
                    break;

                case LookupTypeEnum.Amount:
                    cmbParameter.SelectedData = new DataEntity(posMenuLine.Parameter, posMenuLine.Parameter);
                    cmbParameter.Text = posMenuLine.Parameter;
                    break;

                case LookupTypeEnum.NumericInput:
                    cmbParameter.SelectedData = new DataEntity(posMenuLine.Parameter, posMenuLine.Parameter);
                    cmbParameter.Text = posMenuLine.Parameter;
                    break;

                case LookupTypeEnum.ItemSearch:
                    {
                        if (string.IsNullOrEmpty(posMenuLine.Parameter))
                        {
                            cmbParameter.SelectedData = noSelection;
                            cmbParameter2.SelectedData = noSelection;
                            break;
                        }

                        parameters = posMenuLine.Parameter.Split(';');

                        ItemSearchViewModeEnum viewMode = ItemSearchViewModeEnum.Default;
                        try
                        {
                            viewMode = (ItemSearchViewModeEnum)Int32.Parse(parameters[0]);
                        }
                        catch { }

                        switch (viewMode)
                        {
                            case ItemSearchViewModeEnum.List:
                                cmbParameter.SelectedData = new DataEntity(((int)viewMode).ToString(), Properties.Resources.SearchModeList);
                                break;
                            case ItemSearchViewModeEnum.Images:
                                cmbParameter.SelectedData = new DataEntity(((int)viewMode).ToString(), Properties.Resources.SearchModeImages);
                                break;
                            default:
                                cmbParameter.SelectedData = new DataEntity(((int)viewMode).ToString(), Properties.Resources.SearchModeDefault);
                                break;
                        }

                        if (parameters.Length == 2)
                            cmbParameter2.SelectedData = Providers.RetailGroupData.Get(PluginEntry.DataModel, parameters[1]);
                    }
                    break;
                case LookupTypeEnum.ManuallyTriggerPeriodicDiscount:
                    DataEntity selectedEntity = noSelection;
                    if (!string.IsNullOrEmpty(posMenuLine.Parameter.Trim()))
                    {
                        DiscountOffer selectedDiscount = Providers.DiscountOfferData.Get(PluginEntry.DataModel, posMenuLine.Parameter, DiscountOffer.PeriodicDiscountOfferTypeEnum.All);
                        if (selectedDiscount != null)
                        {
                            selectedEntity = new DataEntity(selectedDiscount.ID, selectedDiscount.Text);
                        }
                    }
                    cmbParameter.SelectedData = selectedEntity;
                    break;

                case LookupTypeEnum.SalesPerson:
                    cmbParameter.SelectedData = noSelection;
                    cmbParameter2.SelectedData = noSelection;

                    if (!string.IsNullOrEmpty(posMenuLine.Parameter.Trim()))
                    {
                        parameters = posMenuLine.Parameter.Split(';');
                        if (parameters.Length > 0)
                        {
                            POSUser salesPerson = Providers.POSUserData.Get(PluginEntry.DataModel, parameters[0], UsageIntentEnum.Normal);
                            if (salesPerson != null)
                            {
                                cmbParameter.SelectedData = salesPerson;
                            }
                        }

                        if (parameters.Length > 1)
                        {
                            bool limitToStore = (parameters[1] == "Y");
                            DataEntity boolParam = new DataEntity(parameters[1], limitToStore ? Properties.Resources.YesHereafter : Properties.Resources.NoHereafter);
                            if (boolParam != null)
                            {
                                cmbParameter2.SelectedData = boolParam;
                            }
                        }
                    }
                    break;
                case LookupTypeEnum.LookupJob:
                    DataProviderFactory.Instance.Register<IDesignData, DesignData, JscTableDesign>();
                    DataProviderFactory.Instance.Register<IInfoData, InfoData, JscInfo>();
                    DataProviderFactory.Instance.Register<IJobData, JobData, JscJob>();
                    DataProviderFactory.Instance
                        .Register<IReplicationActionData, ReplicationActionData, ReplicationAction>();
                    DataProviderFactory.Instance.Register<ILocationData, LocationData, JscLocation>();

                    cmbParameter.SetData(DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobs(PluginEntry.DataModel, false), null);

                    if (posMenuLine.Parameter != "")
                    {
                        JscJob job = DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJob(PluginEntry.DataModel, new Guid(posMenuLine.Parameter), false);
                        cmbParameter.SelectedData = job;
                    }
                    break;

                case LookupTypeEnum.Boolean:
                    string parameterText = string.IsNullOrEmpty(posMenuLine.Parameter) || posMenuLine.Parameter == "N"
                        ? Properties.Resources.No
                        : Properties.Resources.Yes;

                    // This way we can alwas assume either "N" or "Y" when dealing with the selected data in the combobox
                    string parameterValue = string.IsNullOrEmpty(posMenuLine.Parameter) ? "N" : posMenuLine.Parameter;

                    cmbParameter.SelectedData = new DataEntity(parameterValue, parameterText);

                    break;
                case LookupTypeEnum.ReprintReceipt:
                    lblParameter1.Text = Resources.PrintType;
                    lblParameter2.Text = Resources.CustomText;
                    cmbParameter2.EnableTextBox = true;

                    List<DataEntity> reprintEntities = new List<DataEntity>();
                    reprintEntities.Add(new DataEntity("1", Resources.GiftReceipt));
                    reprintEntities.Add(new DataEntity("2", Resources.CopyLastReceipt));
                    reprintEntities.Add(new DataEntity("3", Resources.TaxFreeReceipt));
                    reprintEntities.Add(new DataEntity("4", Resources.CustomReceipt));
                    cmbParameter.SetData(reprintEntities, null);

                    if (posMenuLine.Parameter != "")
                    {
                        string[] split = posMenuLine.Parameter.Split(';');
                        if (split.Length > 0)
                        {
                            cmbParameter.SelectedData = reprintEntities.Find(type => type.ID == split[0]);
                        }
                        if (split.Length > 1)
                        {
                            cmbParameter2.Text = split[1];
                        }
                    }
                    break;
                case LookupTypeEnum.LastSaleOrReceiptID:
                    lblParameter1.Text = Resources.ParameterLabelText;

                    List<DataEntity> lastSaleOrReceitIDEntities = new List<DataEntity>();
                    lastSaleOrReceitIDEntities.Add(new DataEntity("1", Resources.LastReceipt));
                    lastSaleOrReceitIDEntities.Add(new DataEntity("2", Resources.SelectAReceiptID));
                    cmbParameter.SetData(lastSaleOrReceitIDEntities, null);

                    if (posMenuLine.Parameter != "")
                    {
                        cmbParameter.SelectedData = lastSaleOrReceitIDEntities.Find(type => type.ID == posMenuLine.Parameter);
                    }
                    break;
                case LookupTypeEnum.ReasonCode:
                    lblParameter1.Text = Resources.SelectionType;
                    lblParameter2.Text = Resources.ReasonCode;

                    cmbParameter.SelectedData = noSelection;
                    cmbParameter2.SelectedData = noSelection;

                    List<DataEntity> reasonTypes = new List<DataEntity>();
                    reasonTypes.Add(new DataEntity("0", Resources.Default));
                    reasonTypes.Add(new DataEntity("1", Resources.List));
                    reasonTypes.Add(new DataEntity("2", Resources.Specific));
                    cmbParameter.SetData(reasonTypes, null);
                    cmbParameter2.Enabled = false;

                    if (posMenuLine.Parameter != "")
                    {
                        string[] split = posMenuLine.Parameter.Split(';');

                        if (split.Length > 0)
                        {
                            cmbParameter.SelectedData = reasonTypes.Find(type => type.ID == split[0]);

                            if (split[0] == "2") //Specific
                            {
                                cmbParameter2.Enabled = true;
                            }
                        }

                        if (split.Length > 1)
                        {
                            ReasonCode code = Providers.ReasonCodeData.GetReasonById(PluginEntry.DataModel, split[1]);
                            if (code != null)
                            {
                                cmbParameter2.SelectedData = new DataEntity(code.ID, code.Text);
                            }
                        }
                    }

                    break;
                case LookupTypeEnum.InfocodeOnRequest:
                    lblParameter1.Text = Resources.TriggerFor;
                    lblParameter2.Text = Resources.Infocode;

                    cmbParameter.SelectedData = noSelection;
                    cmbParameter2.SelectedData = noSelection;

                    List<DataEntity> triggerFor = new List<DataEntity>();
                    triggerFor.Add(new DataEntity("0", Resources.InfocodeOnRequest_Item));
                    triggerFor.Add(new DataEntity("1", Resources.InfocodeOnRequest_Sale));
                    cmbParameter.SetData(triggerFor, null);
                    cmbParameter2.Enabled = false;

                    if (posMenuLine.Parameter != "")
                    {
                        string[] split = posMenuLine.Parameter.Split(';');

                        if (split.Length > 0)
                        {
                            cmbParameter.SelectedData = triggerFor.Find(type => type.ID == split[0]);

                            if (split[0] == "1") //Item
                            {
                                cmbParameter2.Enabled = true;
                            }
                        }

                        if (split.Length > 1)
                        {
                            DataEntity code = Providers.InfocodeData.Get(PluginEntry.DataModel, split[1]);
                            if (code != null)
                            {
                                cmbParameter2.SelectedData = new DataEntity(code.ID, code.Text);
                            }
                        }
                    }

                    break;
                case LookupTypeEnum.PrintGroup:
                    int printGroupIndex = 0;
                    int.TryParse(posMenuLine.Parameter, out printGroupIndex);
                    cmbParameter.SelectedData = new DataEntity(printGroupIndex.ToString(), ItemSaleReportGroupHelper.ItemSaleReportGroupToString((ItemSaleReportGroupEnum)printGroupIndex));
                    break;
                case LookupTypeEnum.TransferRequests:
                case LookupTypeEnum.TransferOrders:
                    cmbParameter.SelectedData = new DataEntity(posMenuLine.Parameter, posMenuLine.Parameter);
                    break;
            }
        }

        private void cmbParameter_DropDown(object sender, DropDownEventArgs e)
        {
            switch (currentLookupType)
            {
                case LookupTypeEnum.RetailItems:
                    RecordIdentifier initialSearchText;
                    bool textInitallyHighlighted;
                    if (e.DisplayText != "")
                    {
                        initialSearchText = e.DisplayText;
                        textInitallyHighlighted = false;
                    }
                    else
                    {
                        initialSearchText = ((DataEntity)cmbParameter.SelectedData).Text;
                        textInitallyHighlighted = true;
                    }

                    e.ControlToEmbed = new SingleSearchPanel(
                    PluginEntry.DataModel,
                    false,
                    initialSearchText,
                    SearchTypeEnum.RetailItems,
                    textInitallyHighlighted);
                    break;
            }
        }

        private void cmbParameter_RequestClear(object sender, EventArgs e)
        {
            if (currentLookupType != LookupTypeEnum.TextInput && 
                currentLookupType != LookupTypeEnum.NumericInput && 
                currentLookupType != LookupTypeEnum.StorePaymentTypeAndAmount && 
                currentLookupType != LookupTypeEnum.Amount &&
                currentLookupType != LookupTypeEnum.BlankOperations &&
                currentLookupType != LookupTypeEnum.SalesPerson &&
                currentLookupType != LookupTypeEnum.PrintGroup)
            {
                ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");      
            }
        }

        private void cmbParameter2_RequestData(object sender, EventArgs e)
        {            
            switch (currentLookupType)
            {
                case LookupTypeEnum.PosMenuAndButtonGrid:
                    cmbParameter2.SetData(Providers.TouchLayoutData.GetButtonGrids(), null);
                    break;

                case LookupTypeEnum.StorePaymentTypeAndAmount:
                    cmbParameter2.SetWidth(200);

                    cmbParameter2.SetHeaders(new string[] { Properties.Resources.Amount }, new int[] { 1 });

                    List<CashDenominator> denominators = Providers.CashDenominatorData.GetCashDenominators(PluginEntry.DataModel, companyCurrency.ID, 1, false);

                    foreach (CashDenominator cd in denominators)
                    {
                        cd.FormattedAmount = cd.Amount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        cd.Text = cd.FormattedAmount;
                    }

                    cmbParameter2.SetData(denominators, null);
                    break;

                case LookupTypeEnum.BlankOperations:
                    // Add the operation parameter so you have a way of selecting the originally pre-defined parameter
                    BlankOperation currentOperation = Providers.BlankOperationData.Get(PluginEntry.DataModel, cmbParameter.SelectedData.ID);

                    if (currentOperation == null)
                    {
                        break;
                    }

                    DataEntity operationParameter = new DataEntity("", currentOperation.OperationParameter);
                    List<DataEntity> operationParameterList = new List<DataEntity>() { operationParameter };
                    
                    cmbParameter2.SetData(operationParameterList, null);
                    break;

                case LookupTypeEnum.RetailItems:
                    List<BarCode> barCodes = Providers.BarCodeData.GetList(PluginEntry.DataModel, cmbParameter.SelectedData.ID, BarCodeSorting.ItemBarCode, false);

                    cmbParameter2.SetHeaders(new string[] { Properties.Resources.BarCode, 
                                                            Properties.Resources.Size, 
                                                            Properties.Resources.Color, 
                                                            Properties.Resources.Style, 
                                                            Properties.Resources.Unit, 
                                                            Properties.Resources.Quantity }, 
                                             new int[] { 1, 2, 3, 4, 5, 6 });

                    cmbParameter2.SetData(barCodes, null);
                    break;

                case LookupTypeEnum.ItemSearch:
                    List<DataEntity> retailGroups = Providers.RetailGroupData.GetList(PluginEntry.DataModel, RetailGroupSorting.RetailGroupName);
                    cmbParameter2.SetData(retailGroups, null);
                    break;

                case LookupTypeEnum.SalesPerson:
                    List<DataEntity> yesNo = new List<DataEntity>();
                    yesNo.Add(new DataEntity("Y", Properties.Resources.YesHereafter));
                    yesNo.Add(new DataEntity("N", Properties.Resources.NoHereafter));
                    cmbParameter2.SetData(yesNo, null);
                    break;
                case LookupTypeEnum.ReasonCode:
                    List<ReasonCode> reasonCodes = Providers.ReasonCodeData.GetReasonCodesForReturn(PluginEntry.DataModel);
                    cmbParameter2.SetData(reasonCodes, null);
                    break;
                case LookupTypeEnum.InfocodeOnRequest:
                    List<DataEntity> infoCodes = Providers.InfocodeData.GetList(PluginEntry.DataModel, TriggeringEnum.Manual);
                    cmbParameter2.SetData(infoCodes, null);
                    break;
            }
        }

        private void cmbParameter_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();

            switch (currentLookupType)
            {
                case LookupTypeEnum.RetailItems:
                    if (cmbParameter.SelectedData != null)
                    {
                        tbDescription.Text = cmbParameter.SelectedData.Text;                        

                        List<BarCode> barCodes = Providers.BarCodeData.GetList(PluginEntry.DataModel, cmbParameter.SelectedData.ID, BarCodeSorting.ItemBarCode, false);

                        // Find the default barcode
                        if (barCodes.Count > 0)
                        {
                            bool defaultBarcodeFound = false;

                            foreach (BarCode barCode in barCodes)
                            {
                                if (barCode.ShowForItem)
                                {
                                    cmbParameter2.SelectedData = barCode;
                                    defaultBarcodeFound = true;
                                    break;
                                }
                            }

                            if (!defaultBarcodeFound)
                            {
                                cmbParameter2.SelectedData = barCodes[0];
                            }
                        }
                        else
                        {
                            cmbParameter2.SelectedData = noSelection;
                        }
                    }
                    break;

                case LookupTypeEnum.BlankOperations:
                    if (cmbParameter.SelectedData != null)
                    {
                        BlankOperation currentOperation = Providers.BlankOperationData.Get(PluginEntry.DataModel, cmbParameter.SelectedData.ID);

                        if (currentOperation != null && !string.IsNullOrEmpty(currentOperation.OperationParameter))
                        {
                            cmbParameter2.Text = currentOperation.OperationParameter;
                        }
                    }
                    break;
                case LookupTypeEnum.SalesPerson:
                    //Do nothing - the param 2 should remain as is
                    break;
                case LookupTypeEnum.ReasonCode:
                    if (cmbParameter.SelectedData.ID == "2")
                    {
                        cmbParameter2.Enabled = true;
                    }
                    else
                    {
                        cmbParameter2.Enabled = false;
                        cmbParameter2.SelectedData = new DataEntity("", "");
                    }
                    break;
                case LookupTypeEnum.InfocodeOnRequest:
                    if (cmbParameter.SelectedData.ID == "1")
                    {
                        cmbParameter2.Enabled = true;
                    }
                    else
                    {
                        cmbParameter2.Enabled = false;
                        cmbParameter2.SelectedData = new DataEntity("", "");
                    }
                    break;
                default:
                    cmbParameter2.SelectedData = new DataEntity("", "");
                    break;
            }
        }

        /// <summary>
        /// Checks if a string is a numeric value. This is used for NumericInput and Amount looup types error checking
        /// </summary>
        /// <param name="numString">The string to examine</param>
        /// <returns></returns>
        private bool StringIsNumeric(string numString)
        {
            double test = 0;

            return Double.TryParse(numString, out test);
        }

        private void cmbParameter2_FormatData(object sender, DropDownFormatDataArgs e)
        {
            switch (currentLookupType)
            {
                case LookupTypeEnum.RetailItems:
                    cmbParameter2.Text = (string)cmbParameter2.SelectedData[1];
                    break;

                default:
                    cmbParameter2.Text = cmbParameter2.SelectedData.Text;
                    break;
            }
        }

        private void cmbParameter2_SelectedDataChanged(object sender, EventArgs e)
        {
            errorProvider.Clear();
        }        

        private void cmbStyle_RequestData(object sender, EventArgs e)
        {
            List<PosStyle> styleList = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME ASC");
            cmbStyle.SetData(styleList, null);
        }

        private void cmbStyle_SelectedDataChanged(object sender, EventArgs e)
        {            
            posMenuLineCopy.StyleID = cmbStyle.SelectedData.ID;
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ButtonGridPreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
            SetPreviewButton(posMenuLineCopy);            
        }

        private void cmbStyle_RequestClear(object sender, EventArgs e)
        {            
            cmbStyle.SelectedData = new PosStyle(RecordIdentifier.Empty, "");
            posMenuLineCopy.StyleID = cmbStyle.SelectedData.ID;
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "ButtonGridPreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);                
        }
    }
}
