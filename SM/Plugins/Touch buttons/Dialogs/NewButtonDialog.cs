using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.Financials;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.DataLayer.DataProviders.Operations;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.DataLayer.DataProviders.UserManagement;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.SqlDDDataProviders;
using LSOne.Services.Interfaces.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TouchButtons.Properties;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    public partial class NewButtonDialog : DialogBase
    {
        RecordIdentifier posMenuHeaderID;
        RecordIdentifier posMenuLineID;
        LookupTypeEnum currentLookupType;
        PosOperation noOperation;
        List<PosMenuLine> createdLines;
        private Currency companyCurrency; // Used when looking up cash denominations for amounts
        private DataEntity noSelection;
        ErrorProvider errorProvider;
        PosMenuLine emptyItem;
        PosMenuHeader parentHeader;

        public NewButtonDialog(RecordIdentifier posMenuHeaderID)
        {
            InitializeComponent();

            this.posMenuHeaderID = posMenuHeaderID;
            posMenuLineID = RecordIdentifier.Empty;

            noOperation = new PosOperation();
            noOperation.ID = RecordIdentifier.Empty;
            noOperation.Text = Properties.Resources.NoOperation;

            createdLines = new List<PosMenuLine>();
            noSelection = new DataEntity("", "");

            emptyItem = new PosMenuLine();
            emptyItem.ID = RecordIdentifier.Empty;
            emptyItem.Sequence = "";
            emptyItem.Text = Properties.Resources.DoNotCopy;


            errorProvider = new ErrorProvider();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            companyCurrency = Providers.CurrencyData.GetCompanyCurrency(PluginEntry.DataModel);
            cmbOperation.SelectedData = noOperation;
            ntbNoOfLinesToCreate.Value = ntbNoOfLinesToCreate.MinValue;

            parentHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuHeaderID);
            int maxNoOfLines = parentHeader.Rows * parentHeader.Columns;
            int currentNoOfLines = Providers.PosMenuLineData.GetList(PluginEntry.DataModel, posMenuHeaderID).Count;

            ntbNoOfLinesToCreate.MaxValue = maxNoOfLines - currentNoOfLines;
            cmbCopyFrom.SelectedData = emptyItem;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier PosMenuLineID => posMenuLineID;

        public bool CreateMultipleLines => chkCreateMultipleLines.Checked;

        public int NoOfLinesToCreate => (int)ntbNoOfLinesToCreate.Value;

        public List<PosMenuLine> CreatedLines => createdLines;

        private bool ParametersContainErrors()
        {
            switch (currentLookupType)
            {
                case LookupTypeEnum.Amount:
                case LookupTypeEnum.NumericInput:
                    if (!string.IsNullOrWhiteSpace(cmbParameter.Text) && !cmbParameter.Text.IsNumeric())
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterMustBeNumeric);
                        return true;
                    }
                    break;

                case LookupTypeEnum.StorePaymentTypeAndAmount:
                    if (!string.IsNullOrWhiteSpace(cmbParameter2.Text) && !cmbParameter2.Text.IsNumeric())
                    {
                        errorProvider.SetError(cmbParameter2, Resources.ParameterMustBeNumeric);
                        return true;
                    }
                    break;

                case LookupTypeEnum.PosMenuAndButtonGrid:
                    //skip validation for operation parameter if it's the menu for mobile apps
                    if (parentHeader.DeviceType == DeviceTypeEnum.MobileInventory)
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
                    if (string.IsNullOrWhiteSpace(cmbParameter2.Text))
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
                case LookupTypeEnum.TaxGroupInfocodes:
                case LookupTypeEnum.SuspendedTransactionTypes:
                case LookupTypeEnum.LastSaleOrReceiptID:
                case LookupTypeEnum.LookupJob:
                case LookupTypeEnum.RetailItems:
                case LookupTypeEnum.PrintGroup:
                case LookupTypeEnum.TransferOrders:
                case LookupTypeEnum.TransferRequests:
                    if (string.IsNullOrWhiteSpace(cmbParameter.Text))
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    break;
                case LookupTypeEnum.ReprintReceipt:
                    if (string.IsNullOrWhiteSpace(cmbParameter.Text))
                    {
                        errorProvider.SetError(cmbParameter, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    if (cmbParameter.SelectedDataID == "4" && cmbParameter2.Text == "")
                    {
                        errorProvider.SetError(cmbParameter2, Resources.ParameterCannotBeEmpty);
                        return true;
                    }
                    break;
                case LookupTypeEnum.ReasonCode:
                    if(cmbParameter.SelectedDataID == "2" && cmbParameter2.Text == "")
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
                case LookupTypeEnum.None:
                    break;
                case LookupTypeEnum.StorePaymentTypes:
                    break;
                case LookupTypeEnum.PosMenu:
                    break;
                case LookupTypeEnum.BlankOperations:
                    break;
                case LookupTypeEnum.IncomeAccounts:
                    break;
                case LookupTypeEnum.ExpenseAccounts:
                    break;
                case LookupTypeEnum.TextInput:
                    break;
                case LookupTypeEnum.ItemSearch:
                    break;
                case LookupTypeEnum.ManuallyTriggerPeriodicDiscount:
                    break;
                case LookupTypeEnum.Boolean:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        private PosMenuLine CreateNewPosMenuLine()
        {
            PosMenuLine posMenuLine = new PosMenuLine();
            PosMenuHeader posMenuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuHeaderID);

            posMenuLine.Sequence = RecordIdentifier.Empty;
            posMenuLine.MenuID = posMenuHeaderID;
            posMenuLine.Text = tbDescription.Text;

            posMenuLine.FontName = posMenuHeader.FontName;
            posMenuLine.FontSize = posMenuHeader.FontSize;
            posMenuLine.FontBold = posMenuHeader.FontBold;
            posMenuLine.ForeColor = posMenuHeader.ForeColor;
            posMenuLine.BackColor = posMenuHeader.BackColor;
            posMenuLine.FontItalic = posMenuHeader.FontItalic;
            posMenuLine.FontCharset = posMenuHeader.FontCharset;
            posMenuLine.BackColor2 = posMenuHeader.BackColor2;
            posMenuLine.GradientMode = posMenuHeader.GradientMode;
            posMenuLine.Shape = posMenuHeader.Shape;
            posMenuLine.UseHeaderAttributes = true;
            posMenuLine.UseHeaderFont = true;
            posMenuLine.ImageFontName = posMenuHeader.FontName;
            posMenuLine.ImageFontSize = posMenuHeader.FontSize;
            posMenuLine.ImageFontBold = posMenuHeader.FontBold;
            posMenuLine.ImageFontItalic = posMenuHeader.FontItalic;
            posMenuLine.ImageFontCharset = posMenuHeader.FontCharset;
            posMenuLine.ImageFontColor = posMenuHeader.ForeColor;

            posMenuLine.Operation = cmbOperation.SelectedData != null ? cmbOperation.SelectedData.ID : 0;

            if (currentLookupType == LookupTypeEnum.Boolean)
            {
                posMenuLine.Parameter = (string)cmbParameter.SelectedDataID;
            }
            else if (currentLookupType == LookupTypeEnum.NumericInput || currentLookupType == LookupTypeEnum.TextInput || currentLookupType == LookupTypeEnum.Amount)
            {
                if (currentLookupType != LookupTypeEnum.TextInput)
                {
                    if (!string.IsNullOrWhiteSpace(cmbParameter.Text) && !cmbParameter.Text.IsNumeric())
                    {
                        errorProvider.SetError(cmbParameter, Properties.Resources.ParameterMustBeNumeric);
                        return posMenuLine;
                    }
                }

                posMenuLine.Parameter = cmbParameter.Text;
            }
            else if (currentLookupType == LookupTypeEnum.BlankOperations)
            {
                if (cmbParameter.SelectedData == null || cmbParameter.SelectedData.ID != cmbParameter.Text)
                {
                    Guid context = Guid.NewGuid();
                    PluginEntry.DataModel.BeginPermissionOverride(context, new HashSet<string> { Permission.AdministrationMaintainSettings });
                    BlankOperation bo = new BlankOperation
                    {
                        ID = cmbParameter.Text,
                        OperationParameter = cmbParameter2.Text,
                        CreatedOnPOS = true
                    };
                    Providers.BlankOperationData.Save(PluginEntry.DataModel, bo);
                    posMenuLine.Parameter = (string)bo.ID;
                    PluginEntry.DataModel.EndPermissionOverride(context);
                }
                else
                {
                    posMenuLine.Parameter = (string)cmbParameter.SelectedData.ID;
                }
                posMenuLine.Parameter += ";" + cmbParameter2.Text;
            }
            else if (currentLookupType == LookupTypeEnum.PosMenuAndButtonGrid)
            {
                posMenuLine.Parameter = cmbParameter.SelectedData != null ? (string)cmbParameter.SelectedData.ID : "";
                posMenuLine.Parameter += ";" + (cmbParameter2.SelectedData != null ? cmbParameter2.SelectedData.ID.ToString() : "");
            }
            else if (currentLookupType == LookupTypeEnum.StorePaymentTypeAndAmount)
            {
                if (!string.IsNullOrWhiteSpace(cmbParameter2.Text) && !cmbParameter2.Text.IsNumeric())
                {
                    errorProvider.SetError(cmbParameter2, Properties.Resources.ParameterMustBeNumeric);
                    return posMenuLine;
                }

                posMenuLine.Parameter = cmbParameter.SelectedData != null ? (string)cmbParameter.SelectedData.ID : "";
                posMenuLine.Parameter += ";" + cmbParameter2.Text;
            }
            else if (currentLookupType == LookupTypeEnum.RetailItems)
            {
                if (cmbParameter.SelectedData.ID == "" || cmbParameter.SelectedData.ID == RecordIdentifier.Empty)
                {
                    errorProvider.SetError(cmbParameter, Properties.Resources.ParameterCannotBeEmpty);
                    return posMenuLine;
                }
                if (cmbParameter2.SelectedData.ID != "" && cmbParameter2.SelectedData.ID != RecordIdentifier.Empty)
                {
                    posMenuLine.Parameter = (string)((BarCode)cmbParameter2.SelectedData).ItemBarCode;
                }
                else
                {
                    posMenuLine.Parameter = cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? cmbParameter.SelectedData.ID.ToString() : "";
                }
            }
            else if (currentLookupType == LookupTypeEnum.ItemSearch)
            {
                posMenuLine.Parameter = string.Format("{0};{1}", cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? cmbParameter.SelectedData.ID.ToString() : "", cmbParameter2.SelectedData != null && cmbParameter2.SelectedData.ID != "" ? cmbParameter2.SelectedData.ID.ToString() : "");
            }
            else if (currentLookupType == LookupTypeEnum.SalesPerson)
            {
                posMenuLine.Parameter = string.Format("{0};{1}", cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? (string)cmbParameter.SelectedData.ID : "", cmbParameter2.SelectedData != null && cmbParameter2.SelectedData.ID != "" ? (string)cmbParameter2.SelectedData.ID : "");
            }
            else if (currentLookupType == LookupTypeEnum.ReprintReceipt)
            {
                if (cmbParameter.SelectedData.ID == "" || cmbParameter.SelectedData.ID == RecordIdentifier.Empty)
                {
                    errorProvider.SetError(cmbParameter, Properties.Resources.ParameterCannotBeEmpty);
                    return posMenuLine;
                }
                if (cmbParameter.SelectedData.ID == "4" && cmbParameter2.Text == "")
                {
                    errorProvider.SetError(cmbParameter2, Properties.Resources.ParameterCannotBeEmpty);
                    return posMenuLine;
                }
                posMenuLine.Parameter = string.Format("{0};{1}", cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? (string)cmbParameter.SelectedData.ID : "", cmbParameter2.SelectedData != null && cmbParameter2.Text != "" ? (string)cmbParameter2.Text : "");
            }
            else if (currentLookupType == LookupTypeEnum.LastSaleOrReceiptID)
            {
                if (cmbParameter.SelectedData.ID == "" || cmbParameter.SelectedData.ID == RecordIdentifier.Empty)
                {
                    errorProvider.SetError(cmbParameter, Properties.Resources.ParameterCannotBeEmpty);
                    return posMenuLine;
                }
                posMenuLine.Parameter = cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? (string)cmbParameter.SelectedData.ID : "";
            }
            else if (currentLookupType == LookupTypeEnum.ReasonCode)
            {
                if (cmbParameter.SelectedData.ID == "2" && cmbParameter2.Text == "")
                {
                    errorProvider.SetError(cmbParameter2, Properties.Resources.ParameterCannotBeEmpty);
                    return posMenuLine;
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
                    return posMenuLine;
                }

                posMenuLine.Parameter = string.Format("{0};{1}",
                    cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? (string)cmbParameter.SelectedData.ID : "",
                    cmbParameter2.SelectedData != null && cmbParameter2.SelectedData.ID != "" ? (string)cmbParameter2.SelectedData.ID : "");
            }
            else
            {
                posMenuLine.Parameter = cmbParameter.SelectedData != null && cmbParameter.SelectedData.ID != "" ? cmbParameter.SelectedData.ID.ToString() : "";
            }

            if (posMenuLine.Parameter.Trim() == ";")
            {
                posMenuLine.Parameter = "";
            }

            if (posMenuLine.Parameter.EndsWith(";"))
            {
                posMenuLine.Parameter = posMenuLine.Parameter.Substring(0, posMenuLine.Parameter.Length - 1);
            }

            return posMenuLine;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (ParametersContainErrors())
            {
                return;
            }

            PosMenuLine posMenuLine;
            if (CreateMultipleLines)
            {
                for (int i = 0; i < (int)ntbNoOfLinesToCreate.Value; i++)
                {
                    if (cmbCopyFrom.SelectedData.ID != emptyItem.ID)
                    {
                        posMenuLine = Providers.PosMenuLineData.Get(PluginEntry.DataModel, cmbCopyFrom.SelectedData.ID);
                        posMenuLine.ID = RecordIdentifier.Empty;
                        posMenuLine.Text = tbDescription.Text;
                        posMenuLine.Sequence = RecordIdentifier.Empty;
                        posMenuLine.KeyNo = 0;
                    }
                    else
                    {
                        posMenuLine = CreateNewPosMenuLine();
                    }
                    Providers.PosMenuLineData.Save(PluginEntry.DataModel, posMenuLine);

                    if (posMenuLineID == RecordIdentifier.Empty)
                    {
                        posMenuLineID = posMenuLine.ID;
                    }
                    createdLines.Add(posMenuLine);
                }
            }
            else
            {
                if (cmbCopyFrom.SelectedData.ID != emptyItem.ID)
                {
                    posMenuLine = Providers.PosMenuLineData.Get(PluginEntry.DataModel, cmbCopyFrom.SelectedData.ID);
                    posMenuLine.ID = RecordIdentifier.Empty;
                    posMenuLine.Text = tbDescription.Text;
                    posMenuLine.Sequence = RecordIdentifier.Empty;
                    posMenuLine.KeyNo = 0;
                }
                else
                {
                    posMenuLine = CreateNewPosMenuLine();
                }

                Providers.PosMenuLineData.Save(PluginEntry.DataModel, posMenuLine);
                posMenuLineID = posMenuLine.ID;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbOperation_RequestData(object sender, EventArgs e)
        {
            List<PosOperation> operationList = parentHeader.DeviceType == DeviceTypeEnum.POS
               ? Providers.PosOperationData.GetUserOperations(PluginEntry.DataModel).OrderBy(x => x.Text).ToList()
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

                lblParameter2.Enabled = 
                    lblParameter2.Visible = 
                    (currentLookupType == LookupTypeEnum.PosMenuAndButtonGrid || 
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
                        if (int.Parse(cmbOperation.SelectedDataID.StringValue) == (int)POSOperations.PrintZ)
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

                    e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailItems, textInitallyHighlighted);
                    break;
            }
        }

        private void cmbParameter_RequestClear(object sender, EventArgs e)
        {
            if (currentLookupType != LookupTypeEnum.TextInput && currentLookupType != LookupTypeEnum.NumericInput && currentLookupType != LookupTypeEnum.StorePaymentTypeAndAmount && currentLookupType != LookupTypeEnum.Amount && currentLookupType != LookupTypeEnum.BlankOperations)
            {
                ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
            }
        }

        private void cmbParameter_RequestData(object sender, EventArgs e)
        {
            switch (currentLookupType)
            {
                case LookupTypeEnum.RetailItems:
                    break;

                case LookupTypeEnum.StorePaymentTypes:
                    //IEnumerable<PaymentMethod> paymentMethods = Providers.PaymentMethodData.GetList(PluginEntry.DataModel).Where(p => p.DefaultFunction == DataProviderFactory.Instance.Get<IPaymentMethodData, PaymentMethod>().GetDefaultFunctionFromPOSOperation(cmbOperation.SelectedData.ID));
                    IEnumerable<StorePaymentMethod> paymentMethods = Providers.StorePaymentMethodData.GetTenderForOperation(PluginEntry.DataModel, DataProviderFactory.Instance.Get<IStoreData, Store>().GetDefaultStoreID(PluginEntry.DataModel), cmbOperation.SelectedData.ID);
                    cmbParameter.SetData(paymentMethods, null);
                    break;

                case LookupTypeEnum.PosMenu:
                    PosMenuHeaderFilter filter = new PosMenuHeaderFilter();
                    filter.MenuType = (int)MenuTypeEnum.POSButtonGrid;
                    filter.SortBy = PosMenuHeaderSorting.MenuDescription;
                    filter.DeviceType = (int)parentHeader.DeviceType;
                    cmbParameter.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, filter), null);
                    break;

                case LookupTypeEnum.TaxGroupInfocodes:
                    cmbParameter.SetData(Providers.InfocodeData.GetTaxGroupInfocodes(PluginEntry.DataModel), null);
                    break;

                case LookupTypeEnum.PosMenuAndButtonGrid:
                    PosMenuHeaderFilter buttonFilter = new PosMenuHeaderFilter();
                    buttonFilter.MenuType = (int)MenuTypeEnum.POSButtonGrid;
                    buttonFilter.SortBy = PosMenuHeaderSorting.MenuDescription;
                    buttonFilter.DeviceType = (int)parentHeader.DeviceType;
                    cmbParameter.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, buttonFilter), null);
                    break;

                case LookupTypeEnum.SuspendedTransactionTypes:
                    cmbParameter.SetData(Providers.SuspendedTransactionTypeData.GetList(PluginEntry.DataModel), null);
                    break;

                case LookupTypeEnum.BlankOperations:
                    cmbParameter.SetData(Providers.BlankOperationData.GetBlankOperations(PluginEntry.DataModel), null);
                    break;

                case LookupTypeEnum.IncomeAccounts:
                    if(PluginEntry.DataModel.IsHeadOffice)
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
                    IEnumerable<PaymentMethod> paymentMethods2 = Providers.PaymentMethodData.GetList(PluginEntry.DataModel).Where(p => p.DefaultFunction == DataProviderFactory.Instance.Get<IPaymentMethodData, PaymentMethod>().GetDefaultFunctionFromPOSOperation(cmbOperation.SelectedData.ID));
                    cmbParameter.SetData(paymentMethods2, null);
                    break;

                case LookupTypeEnum.Amount:
                    cmbParameter.SetWidth(200);

                    cmbParameter.SetHeaders(new[] { Properties.Resources.Amount }, new[] { 1 });

                    List<CashDenominator> denominators = Providers.CashDenominatorData.GetCashDenominators(PluginEntry.DataModel, companyCurrency.ID, 1, false);

                    foreach (CashDenominator cd in denominators)
                    {
                        cd.FormattedAmount = cd.Amount.FormatWithLimits(PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
                        cd.Text = cd.FormattedAmount;
                    }

                    cmbParameter.SetData(denominators, null);
                    break;
                case LookupTypeEnum.ManuallyTriggerPeriodicDiscount:
                    cmbParameter.SetData(Providers.DiscountOfferData.GetManuallyTriggeredPeriodicDiscounts(PluginEntry.DataModel, DiscountOfferSorting.Description, false), null);
                    break;
                case LookupTypeEnum.SalesPerson:
                    cmbParameter.SetData(Providers.POSUserData.GetList(PluginEntry.DataModel, RecordIdentifier.Empty, UsageIntentEnum.Normal), null);
                    break;
                case LookupTypeEnum.ItemSearch:
                    {
                        List<DataEntity> list = new List<DataEntity>
                    {
                        new DataEntity(((int) ItemSearchViewModeEnum.Default).ToString(), Properties.Resources.SearchModeDefault), new DataEntity(((int) ItemSearchViewModeEnum.List).ToString(), Properties.Resources.SearchModeList), new DataEntity(((int) ItemSearchViewModeEnum.Images).ToString(), Properties.Resources.SearchModeImages),
                    };
                        cmbParameter.SetData(list, null);
                    }
                    break;
                case LookupTypeEnum.LookupJob:
                    DataProviderFactory.Instance.Register<IDesignData, DesignData, JscTableDesign>();
                    DataProviderFactory.Instance.Register<IInfoData, InfoData, JscInfo>();
                    DataProviderFactory.Instance.Register<IJobData, JobData, JscJob>();
                    DataProviderFactory.Instance.Register<IReplicationActionData, ReplicationActionData, ReplicationAction>();
                    DataProviderFactory.Instance.Register<ILocationData, LocationData, JscLocation>();

                    cmbParameter.SetData(DataProviderFactory.Instance.Get<IJobData, JscJob>().GetJobs(PluginEntry.DataModel, false), null);
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

                        if (currentOperation!= null && !string.IsNullOrEmpty(currentOperation.OperationParameter))
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

        private void cmbParameter2_RequestData(object sender, EventArgs e)
        {
            switch (currentLookupType)
            {
                case LookupTypeEnum.PosMenuAndButtonGrid:
                    cmbParameter2.SetData(Providers.TouchLayoutData.GetButtonGrids(), null);
                    break;

                case LookupTypeEnum.StorePaymentTypeAndAmount:
                    cmbParameter2.SetWidth(200);

                    cmbParameter2.SetHeaders(new[] { Properties.Resources.Amount }, new[] { 1 });

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
                    List<DataEntity> operationParameterList = new List<DataEntity> { operationParameter };

                    cmbParameter2.SetData(operationParameterList, null);
                    break;

                case LookupTypeEnum.RetailItems:
                    List<BarCode> barCodes = Providers.BarCodeData.GetList(PluginEntry.DataModel, cmbParameter.SelectedData.ID, BarCodeSorting.ItemBarCode, false);

                    cmbParameter2.SetHeaders(new[]
                    {
                        Properties.Resources.BarCode, Properties.Resources.Size, Properties.Resources.Color, Properties.Resources.Style, Properties.Resources.Unit, Properties.Resources.Quantity
                    }, new[] { 1, 2, 3, 4, 5, 6 });

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

        private void chkCreateMultipleLines_CheckedChanged(object sender, EventArgs e)
        {
            ntbNoOfLinesToCreate.Enabled = chkCreateMultipleLines.Checked;
        }

        private void cmbParameter2_FormatData(object sender, DropDownFormatDataArgs e)
        {
            switch (currentLookupType)
            {
                case LookupTypeEnum.RetailItems:
                    cmbParameter2.Text = cmbParameter2.SelectedData[1];
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

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            List<PosMenuLine> list = Providers.PosMenuLineData.GetList(PluginEntry.DataModel, posMenuHeaderID);

            list.Insert(0, emptyItem);
            cmbCopyFrom.SetData(list, null, true);
        }
    }
}
