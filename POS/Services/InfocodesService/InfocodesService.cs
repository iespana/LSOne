using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Dialogs;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.TransactionObjects.DiscountItems;
using LSOne.DataLayer.TransactionObjects.Line.Hospitality;
using LSOne.DataLayer.TransactionObjects.Line.SaleItem;
using LSOne.POS.Core.Exceptions;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportClasses.IDialog;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.WinFormsTouch;
using LSOne.Triggers;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using OKPressedActions = LSOne.Services.Interfaces.Enums.OKPressedActions;
using PriceHandlings = LSOne.DataLayer.BusinessObjects.Infocodes.PriceHandlings;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Controls.SupportClasses;
using LSOne.Controls.Dialogs.SelectionDialog;

namespace LSOne.Services
{
    public partial class InfocodesService : IInfocodesService
    {
        private bool overrideInfocodeSettings = false;
        private bool inputRequired = false;
        private string prompt = "";        

        public virtual bool ProcessCentralSuspensionInfocode(IConnectionManager entry, ISession session, IRetailTransaction retailTransaction, string infocodeId, bool inputRequired, string prompt)
        {
            overrideInfocodeSettings = true;
            this.inputRequired = inputRequired;
            this.prompt = prompt;
            
            bool result = ProcessInfocode(entry, session, (PosTransaction)retailTransaction, 0, 0, "", "", "", InfoCodeLineItem.TableRefId.None, infocodeId, null, InfoCodeLineItem.InfocodeType.Header, true);
            overrideInfocodeSettings = false;
            return result;
        }

        public virtual bool ProcessInfocode(IConnectionManager entry, ISession session, IPosTransaction posTransaction, decimal quantity, decimal amount, string refRelation, string refRelation2, string refRelation3, InfoCodeLineItem.TableRefId tableRefId, string linkedInfoCodeId, InfoCodeLineItem orgInfocode, InfoCodeLineItem.InfocodeType infocodeType, bool automaticTriggering)
        {
            ISaleLineItem saleLineItem = null;
            return ProcessInfocode(entry,session, posTransaction, quantity, amount, refRelation, refRelation2, refRelation3, tableRefId, linkedInfoCodeId, orgInfocode, infocodeType, ref saleLineItem, automaticTriggering);
        }

        public virtual bool ProcessInfocode(IConnectionManager entry,
            ISession session,
            IPosTransaction posTransaction,
            decimal quantity,
            decimal amount,
            string refRelation,
            string refRelation2,
            string refRelation3,
            InfoCodeLineItem.TableRefId tableRefId,
            string linkedInfoCodeId,
            InfoCodeLineItem orgInfocode,
            InfoCodeLineItem.InfocodeType infocodeType,
            ref ISaleLineItem saleLineItem,
            bool automaticTriggering)
        {
            return ProcessInfocode(entry, session,posTransaction, quantity, amount, refRelation, refRelation2, refRelation3, tableRefId, linkedInfoCodeId, orgInfocode, infocodeType, ref saleLineItem, automaticTriggering, -1);
        }

        public virtual bool ProcessInfocode(IConnectionManager entry, 
                                    ISession session,
                                    IPosTransaction posTransaction, 
                                    decimal quantity, 
                                    decimal amount, 
                                    string refRelation, 
                                    string refRelation2, 
                                    string refRelation3, 
                                    InfoCodeLineItem.TableRefId tableRefId, 
                                    string linkedInfoCodeId, 
                                    InfoCodeLineItem orgInfocode, 
                                    InfoCodeLineItem.InfocodeType infocodeType, 
                                    ref ISaleLineItem saleLineItem, 
                                    bool automaticTriggering,
                                    decimal minimumValue)
        {
            try
            {
                ISettings settings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

                // Trigger: PreInfocode trigger for an infocode 
                PreTriggerResults results = session.PreProcessInfocode(entry, (PosTransaction)posTransaction, tableRefId);

                if(!results.RunOperation)
                {
                    return true;
                }

                refRelation = refRelation ?? "";
                refRelation2 = refRelation2 ?? "";
                refRelation3 = refRelation3 ?? "";

                List<InfoCodeLineItem> infoCodes = null;

                if (linkedInfoCodeId != "") //this is when one Infocode is linked to the next one
                {
                    infoCodes = Providers.InfoCodeLineData.GetInfoCodes(entry, linkedInfoCodeId);
                }
                else if (tableRefId == InfoCodeLineItem.TableRefId.FunctionalityProfile) //this applies to operational Infocodes, i.e. when an item is voided, a discount given etc.
                {
                    infoCodes = Providers.InfoCodeLineData.GetInfoCodes(entry, refRelation2);
                    if (minimumValue != -1)
                    {
                        infoCodes = infoCodes.Where(w => minimumValue >= w.MinSelection).ToList();
                    }
                    refRelation2 = "";
                }
                else if (tableRefId == InfoCodeLineItem.TableRefId.Transaction)
                {
                    infoCodes = Providers.InfoCodeLineData.GetInfoCodes(entry, refRelation);

                    //Because transaction infocodes are not connected to an item the infocode cannot have LinkItemLinesTotriggerLine as true
                    //as that will try to force the items being sold through the infocode to be connected to the selected line
                    //Transaction infocode is not connected to any item and can also be run without any items being on the transaction
                    foreach (InfoCodeLineItem item in infoCodes)
                    {
                        item.LinkItemLinesToTriggerLine = false;
                    }
                    refRelation = "";
                }
                else
                {
                    infoCodes = Providers.InfoCodeLineData.GetInfoCodes(entry, refRelation, refRelation2, refRelation3, tableRefId);
                }

                InfoCodeLineItem.Triggering triggering = InfoCodeLineItem.Triggering.Automatic;
                if (!automaticTriggering && !overrideInfocodeSettings)
                {
                    triggering = InfoCodeLineItem.Triggering.OnRequest;
                }                

                string uom = "";
                if (saleLineItem != null)
                {
                    uom = saleLineItem.SalesOrderUnitOfMeasure;
                }

                bool CalculateTotals = false;
                
                if (overrideInfocodeSettings)
                {
                    foreach (InfoCodeLineItem infoCode in infoCodes)
                    {                        
                        infoCode.InputRequired = inputRequired;
                        infoCode.Prompt = prompt;
                        infoCode.RandomFactor = 100;
                        infoCode.OncePerTransaction = false;
                        infoCode.Triggered = InfoCodeLineItem.Triggering.Automatic;
                    }
                }

                var infocodes = infoCodes.Where(w => w.Triggered == triggering //Only display infocodes that have the correct triggering
                                                     && (w.SalesTypeFilter == "" || w.SalesTypeFilter == ((RetailTransaction) posTransaction).Hospitality.ActiveHospitalitySalesType) //If a sales type is specified on the infocode then only display when the sales types match up
                                                     && (w.UnitOfMeasure == "" || w.UnitOfMeasure == uom) //If a UOM is specified on the infocode then only display when the UOM match up
                                                     && (w.RandomFactor > 0) //If Random factor is 0 then the infocode should not run
                                                     && (w.InfocodeId != null));

                foreach (InfoCodeLineItem infoCode in infocodes)
                {
                    infoCode.Type = infocodeType;
                    infoCode.RefRelation = refRelation;
                    infoCode.RefRelation2 = refRelation2;
                    infoCode.RefRelation3 = refRelation3;                    

                    switch (infoCode.Type)
                    {
                        case InfoCodeLineItem.InfocodeType.Header: infoCode.Amount = amount;
                            break;
                        case InfoCodeLineItem.InfocodeType.Sales: infoCode.Amount = (amount * -1);
                            break;
                        case InfoCodeLineItem.InfocodeType.Payment: infoCode.Amount = amount;
                            break;
                        case InfoCodeLineItem.InfocodeType.IncomeExpense: infoCode.Amount = amount;
                            break;
                    }

                    int randomFactor = (int)(100 / infoCode.RandomFactor); //infoCode.RandomFactor = 100 means ask 100% for a infocode
                    Random random = new Random();
                    int randomNumber = random.Next(randomFactor);  //Creates numbers from 0 to randomFactor-1

                    //Only get the infocode if randomfactor is set to zero or generated random number is the sama as the randomfactor-1
                    if (infoCode.RandomFactor == 100 || randomNumber == (randomFactor - 1))
                    {
                        Boolean infoCodeNeeded = true;
                        if (infoCode.OncePerTransaction)
                        {
                            if (posTransaction is TenderDeclarationTransaction)
                            {
                                infoCodeNeeded = ((TenderDeclarationTransaction)posTransaction).InfoCodeNeeded(infoCode.InfocodeId);
                            }
                            else if (posTransaction is NoSaleTransaction)
                            {
                                infoCodeNeeded = ((NoSaleTransaction)posTransaction).InfoCodeNeeded(infoCode.InfocodeId);
                            }
                            else
                            {
                                infoCodeNeeded = ((RetailTransaction)posTransaction).InfoCodeNeeded(infoCode.InfocodeId);
                            }
                        }
                        if (infoCodeNeeded)
                        {
                            // If the required type is negative but the quantity is positive, do not continue
                            if (infoCode.InputRequriedType == InfoCodeLineItem.InputRequriedTypes.Negative && quantity > 0)
                            {
                                infoCodeNeeded = false;
                            }
                            // If the required type is positive but the quantity is negative, do not continue
                            if (infoCode.InputRequriedType == InfoCodeLineItem.InputRequriedTypes.Positive && quantity < 0)
                            {
                                infoCodeNeeded = false;
                            }
                        }

                        // If there is some infocodeID existing, and infocod is needed
                        if (infoCode.InfocodeId != null && infoCodeNeeded)
                        {                            
                            if (infoCode.InputType == InfoCodeLineItem.InputTypes.Text || infoCode.InputType == InfoCodeLineItem.InputTypes.General)
                            {
                                #region Text and General
                                
                                bool inputValid = true;
                                // Get a infocode text
                                do
                                {
                                    inputValid = true;
                                    string inputText = "";

                                    //Show the input form
                                    DialogResult dialogResult = Interfaces.Services.DialogService(entry).KeyboardInput(ref inputText, infoCode.Prompt, Properties.Resources.Infocode, infoCode.MaximumLength, InputTypeEnum.Normal);

                                    #region TextInputValid

                                    if (dialogResult != DialogResult.OK && !infoCode.InputRequired)
                                    {
                                        break;
                                    }
                                    if (inputText != null)
                                    {
                                        string msgText = "";
                                        if (inputText.Length == 0 && infoCode.InputRequired)
                                        {
                                            msgText = Properties.Resources.txtRequired;
                                            inputValid = false;
                                        }
                                        if (inputValid && infoCode.MinimumLength > 0 && inputText.Length < infoCode.MinimumLength)
                                        {
                                            msgText = Properties.Resources.txtTooShort;
                                            inputValid = false;
                                        }
                                        if (inputValid && infoCode.MaximumLength > 0 && inputText.Length > infoCode.MaximumLength)
                                        {
                                            msgText = Properties.Resources.txtExceedsMaxLength;
                                            inputValid = false;
                                        }
                                        if (inputValid && infoCode.AdditionalCheck == 1)
                                        {
                                            inputValid = POS.Processes.Common.Utility.CheckKennitala(inputText);
                                            if (!inputValid)
                                            {
                                                msgText = Properties.Resources.txtNationalIdValidityFailed;
                                            }
                                        }
                                        if (!inputValid)
                                        {
                                           Interfaces.Services.DialogService(entry).ShowMessage(msgText, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                        }
                                    }
                                    #endregion TextInputValid
                                    else
                                    {
                                        inputValid = false;
                                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.AInputIsRequired);
                                    }
                                    infoCode.Information = inputText;
                                }
                                while (!inputValid);

                                #endregion
                            }
                            
                            else if (infoCode.InputType == InfoCodeLineItem.InputTypes.Date)
                            {
                                #region Date
                                Boolean inputValid = true;
                                do
                                {
                                    inputValid = true;


                                    string inputText = "";
                                    //Show the input form
                                    DialogResult dialogResult = Interfaces.Services.DialogService(entry).DateSelection(entry, ref inputText, infoCode.Prompt, infoCode.InputRequired);
                                    if (dialogResult == DialogResult.Cancel && !infoCode.InputRequired)
                                    {
                                        return true;
                                    }

                                    #region InputValid
                                    //Is input valid? 
                                    if ((inputText != null) && (inputText != ""))
                                    {
                                        string text = "";
                                        bool isDate = true;
                                        DateTime infoDate = DateTime.Now;
                                        try
                                        {
                                            infoDate = Convert.ToDateTime(inputText, settings.CultureInfo.DateTimeFormat);
                                        }
                                        catch { isDate = false; }

                                        if (!isDate)
                                        {
                                            text = Properties.Resources.InvalidDateFormat; //Date entered is not valid
                                            inputValid = false;
                                        }
                                        if (inputText.Length == 0 && infoCode.InputRequired)
                                        {
                                            text = Properties.Resources.NumberInputRequired; //A number input is required
                                            inputValid = false;
                                        }

                                        if (!inputValid)
                                        {
                                            Interfaces.Services.DialogService(entry).ShowMessage(text, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                        }
                                        else
                                        {
                                            //Setting the result to the infocode
                                            infoCode.Information = infoDate.ToString(settings.CultureInfo.DateTimeFormat.ShortDatePattern);
                                        }
                                    }
                                    #endregion InputValid
                                    else if (infoCode.InputRequired)
                                    {
                                        inputValid = false;                     
                                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NumberInputRequired, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);//A number input is required                                
                                    }
                                }
                                while (!inputValid);
                                #endregion
                            }
                            
                            else if (infoCode.InputType == InfoCodeLineItem.InputTypes.Numeric || infoCode.InputType == InfoCodeLineItem.InputTypes.Operator)
                            {
                                #region Numeric and Operator/Staff

                                Boolean inputValid = true;
                                do
                                {
                                    inputValid = true;

                                    string inputText = "";
                                    //Show the input form
                                    DialogResult dialogResult;
                                    using (NumpadAmountQtyDialog inputDialog = new NumpadAmountQtyDialog())
                                    {
                                        inputDialog.HasDecimals = false;
                                        inputDialog.PromptText = infoCode.Prompt;
                                        inputDialog.GhostText = Properties.Resources.Infocode;
                                        inputDialog.InputRequried = infoCode.InputRequired;
                                        inputDialog.SetMaxInputValue((double)infoCode.MaximumValue);

                                        dialogResult = inputDialog.ShowDialog();

                                        inputText = inputDialog.HasInput ? inputDialog.Value.ToString(settings.CultureInfo) : "";
                                    }
                                    #region InputValid
                                    if (!string.IsNullOrEmpty(inputText) && dialogResult != DialogResult.Cancel)
                                    {
                                        string text = "";
                                        if (inputText.Length == 0 && infoCode.InputRequired)
                                        {
                                            text = Properties.Resources.NumberInputRequired; //A number input is required
                                            inputValid = false;
                                        }
                                        if (infoCode.MinimumValue != 0 && Convert.ToDecimal(inputText) < infoCode.MinimumValue)
                                        {
                                            text = Properties.Resources.NumberTooLow; //The number is lower than the minimum value
                                            inputValid = false;
                                        }
                                        if (infoCode.MaximumValue != 0 && Convert.ToDecimal(inputText) > infoCode.MaximumValue)
                                        {
                                            text = Properties.Resources.NumberTooHigh; //The number exceeds the maximum value
                                            inputValid = false;
                                        }
                                        if (!inputValid)
                                        {
                                            Interfaces.Services.DialogService(entry).ShowMessage(text, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                        }
                                    }
                                    #endregion InputValid
                                    else if ((inputText == "" || dialogResult == DialogResult.Cancel) && infoCode.InputRequired)
                                    {
                                        inputValid = false;
                                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.NumberInputRequired, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);//A number input is required                                                               
                                    }
                                    //Setting the result to the infocode
                                    infoCode.Information = inputText;
                                }
                                while (!inputValid);

                                #endregion
                            }                            

                            else if (infoCode.InputType == InfoCodeLineItem.InputTypes.Customer)
                            {
                                #region Customer

                                bool inputValid = true;
                                do
                                {
                                    inputValid = true;
                                    posTransaction = Interfaces.Services.CustomerService(entry).Search(entry, posTransaction, true);
                                    if (((IRetailTransaction)posTransaction).Customer.ID != RecordIdentifier.Empty)
                                    {
                                        infoCode.Information = (string)((IRetailTransaction)posTransaction).Customer.ID;

                                        string customerName = ((IRetailTransaction)posTransaction).Customer.FirstName != "" ?
                                            ((IRetailTransaction)posTransaction).Customer.GetFormattedName(entry.Settings.NameFormatter):
                                            ((IRetailTransaction)posTransaction).Customer.Text;

                                        infoCode.Information2 = customerName;
                                        inputValid = true;

                                    }
                                    else
                                    {
                                        if (infoCode.InputRequired)
                                        {
                                            inputValid = false;
                                            Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.CustomerInputRequired, MessageBoxButtons.OK, MessageDialogType.ErrorWarning); //A customer needs to be selected
                                        }
                                    }
                                }
                                while (!inputValid);

                                #endregion
                            }
                            
                            else if (infoCode.InputType == InfoCodeLineItem.InputTypes.Item)
                            {
                                #region Item

                                Boolean inputValid = true;
                                do
                                {
                                    string selectedItemID = "";
                                    string selectedItemName = "";
                                    DialogResult dialogResult = Interfaces.Services.DialogService(entry).ItemSearch(100, ref selectedItemID, ref selectedItemName, ItemSearchViewModeEnum.Default, DataLayer.BusinessObjects.ItemMaster.RetailItemSearchEnum.RetailGroup, null, posTransaction);
                                    // Quit if cancel is pressed...
                                    if (dialogResult == DialogResult.Cancel && !infoCode.InputRequired)
                                    {
                                        return true;
                                    }

                                    if (selectedItemID != "" && selectedItemID != null)
                                    {
                                        infoCode.Information = selectedItemID;
                                        infoCode.Information2 = selectedItemName;
                                        inputValid = true;
                                        InfocodeSubcode subcode = new InfocodeSubcode();
                                        subcode.TriggerFunction = TriggerFunctions.Item;
                                        subcode.TriggerCode = selectedItemID;
                                        subcode.PriceHandling = PriceHandlings.AlwaysCharge;
                                        subcode.PriceType = PriceTypes.Percent;

                                        CreateSubcodeSaleLineItem(entry, infoCode, subcode, (RetailTransaction) posTransaction, saleLineItem, "", 1);
                                        CalculateTotals = (triggering == InfoCodeLineItem.Triggering.Automatic);                                        
                                    }
                                    else if (infoCode.InputRequired)
                                    {
                                        inputValid = false;
                                        Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ItemInputRequired, MessageBoxButtons.OK, MessageDialogType.ErrorWarning); //A items needs to be selected                                                    
                                    }

                                } while (!inputValid);

                                #endregion
                            }                            
                           
                            else if (infoCode.InputType == InfoCodeLineItem.InputTypes.SubCodeButtons)
                            {
                                #region SubCode
                                IEnumerable<InfocodeSubcode> data = Providers.InfocodeSubcodeData.GetListForInfocode(entry, infoCode.InfocodeId, InfocodeSubcodeSorting.Description, false);
                                using (InfocodeButtonDialog dialog = new InfocodeButtonDialog(data))
                                {
                                    dialog.InfocodePrompt = infoCode.Prompt;
                                    dialog.InputRequired = infoCode.InputRequired;
                                    if (dialog.ShowDialog() == DialogResult.OK)
                                    {
                                        InfocodeSubcode selectedSubcode = dialog.SelectedInfocode;
                                        infoCode.Information = selectedSubcode.Text;
                                        infoCode.Subcode = (string)selectedSubcode.SubcodeId;

                                        //TenderDeclarationTransaction also has infocodes but it can't have any sale items so no need
                                        //to go through this code.
                                        if (posTransaction is RetailTransaction)
                                        {
                                            //Look through the information on the subcode that was selected and see if an item is to be sold
                                            //and if a discount and/or price needs to be modified                                            
                                            if (selectedSubcode.TriggerFunction == TriggerFunctions.Item && selectedSubcode.TriggerCode != "")
                                            {
                                                CreateSubcodeSaleLineItem(entry, infoCode, selectedSubcode, (RetailTransaction)posTransaction, saleLineItem, "", 1);
                                                CalculateTotals = (triggering == InfoCodeLineItem.Triggering.Automatic);
                                            }
                                        }
                                    }     
                                }
                            }
                            else if(infoCode.InputType == InfoCodeLineItem.InputTypes.SubCodeList)
                            {
                                List<InfocodeSubcode> data = Providers.InfocodeSubcodeData.GetListForInfocode(entry, infoCode.InfocodeId, InfocodeSubcodeSorting.Description, false);

                                using (SelectionDialog dialog = new SelectionDialog(new InfocodeSelectionList(data), infoCode.Prompt, infoCode.InputRequired))
                                {
                                    if(dialog.ShowDialog() == DialogResult.OK)
                                    {
                                        InfocodeSubcode selectedSubcode = (InfocodeSubcode)dialog.SelectedItem;
                                        infoCode.Information = selectedSubcode.Text;
                                        infoCode.Subcode = (string)selectedSubcode.SubcodeId;
                                        //TenderDeclarationTransaction also has infocodes but it can't have any sale items so no need
                                        //to go through this code.
                                        if (posTransaction is RetailTransaction)
                                        {
                                            //Look through the information on the subcode that was selected and see if an item is to be sold
                                            //and if a discount and/or price needs to be modified                                            
                                            if (selectedSubcode.TriggerFunction == TriggerFunctions.Item && selectedSubcode.TriggerCode != "")
                                            {
                                                CreateSubcodeSaleLineItem(entry, infoCode, selectedSubcode, (RetailTransaction)posTransaction, saleLineItem, "", 1);
                                                CalculateTotals = (triggering == InfoCodeLineItem.Triggering.Automatic);
                                            }
                                        }
                                    }
                                }
                                
                                #endregion
                            }
                            
                            else if ((infoCode.InputType == InfoCodeLineItem.InputTypes.AgeLimit))
                            {
                                #region Age limit

                                decimal ageLimit = infoCode.MinimumValue;
                                if (ageLimit > 0M)
                                {
                                    decimal remainder = ageLimit % 1;
                                    if (remainder > 0M)
                                    {
                                        //TODO: Figure out the date and month and possibly year if the age limit is a decimal number i.e. 20,5
                                    }

                                    //Create a date time variable
                                    int absAgeLimit = Convert.ToInt16(Math.Abs(ageLimit));
                                    DateTime dtBirthDate = new DateTime(DateTime.Now.Year - absAgeLimit, DateTime.Now.Month, DateTime.Now.Day);

                                    //Convert the date time value according to the current culture information
                                    string birthDate = dtBirthDate.ToString(settings.CultureInfo.DateTimeFormat.ShortDatePattern);

                                    //Age Limit %1, check ID, the customer must be born before %2
                                    string msg = Properties.Resources.AgeLimit;
                                    msg = msg.Replace("%1", absAgeLimit.ToString());
                                    msg = msg.Replace("%2", birthDate);

                                    DialogResult result = Interfaces.Services.DialogService(entry).ShowMessage(msg, MessageBoxButtons.OKCancel, MessageDialogType.Generic);
                                    infoCode.Information = result == DialogResult.OK ? "1" : "0";

                                    if (result != DialogResult.OK)
                                    {
                                        return false;
                                    }

                                }

                                #endregion Age limit
                            }                            
                            
                            else if ((infoCode.InputType == InfoCodeLineItem.InputTypes.Group))
                            {
                                #region Group

                                PopUpFormData popUpFormData = GetPopUpFormData(entry, infoCode.InfocodeId, true, ref saleLineItem, ref posTransaction);

                                if (DialogResult.OK == Interfaces.Services.DialogService(entry).PopUpDialog(entry, ref popUpFormData))
                                {
                                    InfocodeSubcode subcode = new InfocodeSubcode();                                    

                                    foreach (Item item in popUpFormData.Items.Where(w => w.NumberOfClicks > 0))
                                    {
                                        PopupPrimaryKey popupPK = (PopupPrimaryKey)item.PrimaryKey;                                        

                                        if (item.UseageCategory == UsageCategories.None)
                                        {
                                            InfoCodeLineItem infoCodeLineItem = new InfoCodeLineItem();
                                            infoCodeLineItem.InfocodeId = item.GroupId;
                                            infoCodeLineItem.Subcode = item.ItemId; 
                                            infoCodeLineItem.Information = item.ItemId;
                                            saleLineItem.Add(infoCodeLineItem);
                                        }

                                        if (item.UseageCategory == UsageCategories.CrossSelling)
                                        {
                                            RecordIdentifier tempIdentifier = new RecordIdentifier(popupPK.InfocodeId, new RecordIdentifier(popupPK.SubCodeId));
                                            subcode = Providers.InfocodeSubcodeData.Get(entry, tempIdentifier);
                                            if (subcode.TriggerFunction == TriggerFunctions.None)
                                            {
                                                saleLineItem.Add(new InfoCodeLineItem {InfocodeId = subcode.InfocodeId.StringValue,
                                                    Subcode = subcode.SubcodeId.StringValue,
                                                    Information = subcode.SubcodeId.StringValue + " - " + subcode.Text
                                                });
                                                saleLineItem.Comment += subcode.Text + "\r\n";
                                            }
                                            else
                                            {
                                                saleLineItem = CreateSubcodeSaleLineItem(entry, infoCode, subcode, (RetailTransaction) posTransaction, saleLineItem, "", item.NumberOfClicks);
                                                CalculateTotals = (triggering == InfoCodeLineItem.Triggering.Automatic);
                                            }
                                        }

                                        if ((item.NumberOfClicks-item.PrevSelection) > 0 && item.UseageCategory == UsageCategories.ItemModifier)
                                        {                                            
                                            RecordIdentifier tempIdentifier = new RecordIdentifier(popupPK.InfocodeId, new RecordIdentifier(popupPK.SubCodeId));
                                            subcode = Providers.InfocodeSubcodeData.Get(entry, tempIdentifier);
                                            if (subcode.TriggerFunction == TriggerFunctions.None)
                                            {
                                                saleLineItem.Add(new InfoCodeLineItem
                                                {
                                                    InfocodeId = subcode.InfocodeId.StringValue,
                                                    Subcode = subcode.SubcodeId.StringValue,
                                                    Information = subcode.SubcodeId.StringValue +" - "+ subcode.Text
                                                });
                                                saleLineItem.Comment += subcode.Text + "\r\n";
                                            }
                                            else
                                            {
                                                saleLineItem = CreateSubcodeSaleLineItem(entry, infoCode, subcode, (RetailTransaction) posTransaction, saleLineItem, "", (item.NumberOfClicks - item.PrevSelection));
                                                CalculateTotals = (triggering == InfoCodeLineItem.Triggering.Automatic);
                                            }
                                        }
                                    }
                                    
                                }                                
                                #endregion
                            }
                            
                            else
                            {
                                Interfaces.Services.DialogService(entry).ShowMessage("This infocode type has not been implemented yet", MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                                return false;
                            }

                            // Add the infocode to the transaction
                            if (infoCode.Information != null && infoCode.Information.Length > 0)
                            {
                                string infoComment = "";
                                if (infoCode.PrintPromptOnReceipt || infoCode.PrintInputOnReceipt || infoCode.PrintInputNameOnReceipt)
                                {
                                    if (infoCode.PrintPromptOnReceipt && infoCode.Prompt != null && infoCode.Prompt != "")
                                    {
                                        infoComment = infoCode.Prompt;
                                    }
                                    if (infoCode.PrintInputOnReceipt && infoCode.Subcode != null && infoCode.Subcode != "")
                                    {
                                        if (infoComment != "")
                                            infoComment += " - " + infoCode.Subcode;
                                        else
                                            infoComment = infoCode.Subcode;
                                    }
                                    if (infoCode.PrintInputNameOnReceipt && infoCode.Information != null && infoCode.Information != "")
                                    {
                                        if (infoComment != "")
                                            infoComment += " - " + infoCode.Information;
                                        else
                                            infoComment = infoCode.Information;
                                    }
                                }

                                if (((tableRefId == InfoCodeLineItem.TableRefId.Item) || (tableRefId == InfoCodeLineItem.TableRefId.PreItem) || (tableRefId == InfoCodeLineItem.TableRefId.ItemGroup) || (tableRefId == InfoCodeLineItem.TableRefId.FunctionalityProfile)) && (saleLineItem != null))
                                {
                                    saleLineItem.Add(infoCode);

                                    saleLineItem.Comment += "\r\n" + infoComment.Trim();
                                    if (saleLineItem.Comment.StartsWith("\r\n"))
                                        saleLineItem.Comment = saleLineItem.Comment.Remove(0, 2);                                    
                                }
                                else if (tableRefId == InfoCodeLineItem.TableRefId.Tender || tableRefId == InfoCodeLineItem.TableRefId.CreditCard)
                                {
                                    var tenderLineItem = ((RetailTransaction) posTransaction).TenderLines.Count > 0
                                        ? ((RetailTransaction) posTransaction).TenderLines[
                                            ((RetailTransaction) posTransaction).TenderLines.Count - 1]
                                        : null;
                                    tenderLineItem.Add(infoCode);

                                    if (infoComment.Trim() != "")
                                    {
                                        tenderLineItem.Comment += "\r\n" + infoComment.Trim();
                                        if (tenderLineItem.Comment.StartsWith("\r\n"))
                                            tenderLineItem.Comment = tenderLineItem.Comment.Remove(0, 2);
                                    }
                                }
                                else
                                {   
                                    if (posTransaction is RetailTransaction)
                                    {
                                        ((RetailTransaction)posTransaction).Add(infoCode);
                                        if (infoComment.Trim() != "")
                                        {                                            
                                            ((RetailTransaction)posTransaction).InvoiceComment += "\r\n" + infoComment.Trim();
                                            if (((RetailTransaction)posTransaction).InvoiceComment.StartsWith("\r\n"))
                                                ((RetailTransaction)posTransaction).InvoiceComment = ((RetailTransaction)posTransaction).InvoiceComment.Remove(0, 2);
                                        }             
                                    }
                                    else if (posTransaction is NoSaleTransaction)
                                    {
                                        ((NoSaleTransaction)posTransaction).Add(infoCode);
                                    }
                                    else
                                    {
                                        ((TenderDeclarationTransaction)posTransaction).Add(infoCode);                                      
                                    }
                                }
                            }
                        }
                    }
                }

                //CalcTotals only needs to be called when the infocode is called "by request" and a new sales line was added
                //otherwise the functions that call the infocode takes care of this.
                if (CalculateTotals)
                {
                    Services.Interfaces.Services.CalculationService(entry).CalculateTotals(entry, (PosTransaction)posTransaction);
                }
                return true;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }
        protected virtual ISaleLineItem CreateSubcodeSaleLineItem(
            IConnectionManager entry,
            InfoCodeLineItem Infocode, 
            InfocodeSubcode selectedSubcode,
            IRetailTransaction transaction, 
            ISaleLineItem orgSaleLineItem, 
            string PrefixDescr, 
            decimal QtySelected)
        {
            try
            {
                ISaleLineItem itemToLinkTo = null;

                if (selectedSubcode.TriggerFunction != TriggerFunctions.Item)
                {
                    return itemToLinkTo;
                }                

                #region Link Item Lines to Trigger Line
                //If LinkItemLinestoTriggerLine is true and the sale line that triggered the infocode has qty greater than 1
                //create a new sale line with qty 1 and set the org sale line item qty down by 1.
                if (Infocode.LinkItemLinesToTriggerLine && Math.Abs(orgSaleLineItem.Quantity) > 1)
                {
                    SaleLineItem cloneItem = (SaleLineItem)orgSaleLineItem.Clone();

                    int qtyFactor = 1;
                    if (orgSaleLineItem.Quantity < 0)
                    {
                        qtyFactor = -1;
                    }

                    cloneItem.Quantity = (1 * qtyFactor);
                    cloneItem.QuantityDiscounted = 0;
                    cloneItem.DiscountLines.Clear();
                    transaction.Add(cloneItem);

                    orgSaleLineItem.Quantity -= (1 * qtyFactor);

                    itemToLinkTo = cloneItem;

                    // Set the property back to false so that if the user selected more than one subcode they don't 
                    // split the item again.
                    Infocode.LinkItemLinesToTriggerLine = false;
                }
                #endregion

                if (itemToLinkTo == null)
                {
                    itemToLinkTo = orgSaleLineItem;
                }

                /***************************************************************************

                  itemToLinkTo is null when a transaction (sale) infocode is being run and
                  that infocode is selling an item so all references to itemToLinkTo have to be
                  checked for null and default values are used instead

                 ***************************************************************************/

                SaleLineItem subcodeLineItem = new SaleLineItem(transaction);                
                subcodeLineItem.ItemId = selectedSubcode.TriggerCode.ToString();
                subcodeLineItem.Transaction = transaction;

                #region Set UOM                

                if (selectedSubcode.UnitOfMeasure != "")
                {
                    subcodeLineItem.SalesOrderUnitOfMeasure = selectedSubcode.UnitOfMeasure.ToString();

                    
                    Unit unit = Providers.UnitData.Get(entry, subcodeLineItem.SalesOrderUnitOfMeasure, CacheType.CacheTypeApplicationLifeTime);

                    subcodeLineItem.SalesOrderUnitOfMeasureName = (unit != null) ? unit.Text : "";
                    if (itemToLinkTo != null)
                    {
                        subcodeLineItem.UnitQuantityFactor = Providers.UnitConversionData.GetUnitOfMeasureConversionFactor(entry, itemToLinkTo.OrgUnitOfMeasure, subcodeLineItem.SalesOrderUnitOfMeasure, subcodeLineItem.ItemId);
                    }
                }
                else if (selectedSubcode.UnitOfMeasure == "" && itemToLinkTo != null)
                {                    
                    subcodeLineItem.SalesOrderUnitOfMeasure = itemToLinkTo.SalesOrderUnitOfMeasure;
                    subcodeLineItem.SalesOrderUnitOfMeasureName = itemToLinkTo.SalesOrderUnitOfMeasureName;
                    subcodeLineItem.MarkupAmount = itemToLinkTo.MarkupAmount;
                    subcodeLineItem.UnitQuantityFactor = itemToLinkTo.UnitQuantityFactor;
                    subcodeLineItem.UnitOfMeasureChanged = true;                    
                }
                
                #endregion

                #region ProcessItem
                Services.Interfaces.Services.ItemService(entry).ProcessItem(entry, subcodeLineItem, transaction);

                if (subcodeLineItem.SalesOrderUnitOfMeasure != subcodeLineItem.OrgUnitOfMeasure)
                {
                    RecordIdentifier unitConversionID = new RecordIdentifier(subcodeLineItem.ItemId, new RecordIdentifier(subcodeLineItem.SalesOrderUnitOfMeasure, subcodeLineItem.OrgUnitOfMeasure));
                    UnitConversion unitConversion = Providers.UnitConversionData.Get(entry, unitConversionID);

                    //if no unit conversion is found then set the original unit information back on the item
                    if (unitConversion == null)
                    {
                        subcodeLineItem.SalesOrderUnitOfMeasure = subcodeLineItem.OrgUnitOfMeasure;

                        Unit unit = Providers.UnitData.Get(entry, subcodeLineItem.SalesOrderUnitOfMeasure, CacheType.CacheTypeApplicationLifeTime);

                        subcodeLineItem.SalesOrderUnitOfMeasureName = (unit != null) ? unit.Text : "";
                        subcodeLineItem.UnitQuantityFactor = Providers.UnitConversionData.GetUnitOfMeasureConversionFactor(entry, subcodeLineItem.OrgUnitOfMeasure, subcodeLineItem.SalesOrderUnitOfMeasure, subcodeLineItem.ItemId);                        
                    }                    
                }

                if (subcodeLineItem.Dimension.EnterDimensions)
                {
                    Interfaces.Services.DimensionService(entry).SetDimensionOnItem(entry, subcodeLineItem);
                }

                if (PrefixDescr != "")
                {
                    subcodeLineItem.Description = PrefixDescr + " " + subcodeLineItem.Description;
                }

                if (!subcodeLineItem.Found)
                {
                    Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.TheItemIsNotFound);
                    return null;
                }
                #endregion                

                #region Set Quantity

                decimal quantity = QtySelected;
                decimal unitQuantity = 1M;

                //Qty. becomes negative 
                if (!subcodeLineItem.ScaleItem && subcodeLineItem.QtyBecomesNegative)
                {
                    quantity *= -1;
                }

                if (itemToLinkTo != null && selectedSubcode.QtyLinkedToTriggerLine)
                {
                    quantity = itemToLinkTo.Quantity;
                    unitQuantity = itemToLinkTo.UnitQuantity;
                }

                if (selectedSubcode.QtyPerUnitOfMeasure > 0M)
                {
                    quantity *= selectedSubcode.QtyPerUnitOfMeasure;                    
                }

                subcodeLineItem.Quantity = quantity;
                subcodeLineItem.UnitQuantity = subcodeLineItem.Quantity * subcodeLineItem.UnitQuantityFactor;



                #endregion

                //If itemToLinkTo is null then the subcode item is being sold as a separate item not a linked item
                subcodeLineItem.IsInfoCodeItem = itemToLinkTo != null;
                subcodeLineItem.IsLinkedItem = itemToLinkTo != null;
                subcodeLineItem.LinkedToLineId = itemToLinkTo != null ? itemToLinkTo.LineId : -1;
                subcodeLineItem.LinkedItemOrgQuantity = quantity;
                if (itemToLinkTo != null)
                {
                    itemToLinkTo.IsReferencedByLinkItems = true;
                }

                #region Set the price

                if (selectedSubcode.PriceHandling != PriceHandlings.NoCharge)
                {
                    if (selectedSubcode.PriceType == PriceTypes.Price)
                    {
                        subcodeLineItem = (SaleLineItem)transaction.PriceOverride(subcodeLineItem, selectedSubcode.AmountPercent);
                        subcodeLineItem.PriceOverridden = true;
                        subcodeLineItem.NoPriceCalculation = true;
                    }
                    else if (selectedSubcode.PriceType == PriceTypes.Percent)
                    {   
                        // Create the discount item and set the discount
                        LineDiscountItem discountItem = new LineDiscountItem();
                        discountItem.Percentage = selectedSubcode.AmountPercent;                        
                        subcodeLineItem.Add(discountItem);
                    }
                    else if (selectedSubcode.PriceType == PriceTypes.None)
                    {
                        subcodeLineItem = (SaleLineItem)transaction.PriceOverride(subcodeLineItem, 0.0m);
                        subcodeLineItem.PriceOverridden = true;
                        subcodeLineItem.NoPriceCalculation = true;
                    }
                }
                else if (selectedSubcode.PriceHandling == PriceHandlings.NoCharge)
                {
                    subcodeLineItem = (SaleLineItem)transaction.PriceOverride(subcodeLineItem, 0.0m);
                    subcodeLineItem.PriceOverridden = true;
                    subcodeLineItem.NoPriceCalculation = true;
                }

                #endregion

                #region Set Hospitality issues
                
                //If itemToLinkTo is null then set default values for the hospitality so that any item is printed
                subcodeLineItem.MenuTypeItem = itemToLinkTo != null ? itemToLinkTo.MenuTypeItem : new MenuTypeItem();

                // if the item can be printed...
                if (itemToLinkTo != null && itemToLinkTo.PrintingStatus != SalesTransaction.PrintStatus.Unprintable)
                {
                    // The both the subcode item and the parent item must be set (back?) to printable
                    subcodeLineItem.PrintingStatus = SalesTransaction.PrintStatus.Printable;
                    itemToLinkTo.PrintingStatus = SalesTransaction.PrintStatus.Printable;
                }
                else if (itemToLinkTo == null)
                {
                    subcodeLineItem.PrintingStatus = SalesTransaction.PrintStatus.Printable;
                }
                

                #endregion

                #region Add the item to the transaction

                SaleLineItem lastLine = transaction.SaleItems.Count > 0 ? (SaleLineItem) transaction.SaleItems.Last() : null;
                bool onlyCalculateLastLine = false;
                
                //If the last item line is linked to the same item as the subcode item should be linked to
                //then add the item as the last item in the transaction
                if (lastLine != null && lastLine.IsInfoCodeItem && lastLine.LinkedToLineId == itemToLinkTo.LineId)
                {
                    subcodeLineItem.WasChanged = true; //To trigger tax calculation
                    transaction.Add(subcodeLineItem);
                    onlyCalculateLastLine = true;
                }
                else //we need to find the item in the transaction and add it inbetween
                {
                    int qtySubcodeLines = itemToLinkTo != null ? transaction.SaleItems.Count(c => c.LinkedToLineId == itemToLinkTo.LineId) : 0;
                    if (qtySubcodeLines == 0 && itemToLinkTo != null) //no subcode lines found
                    {
                        transaction.Insert(itemToLinkTo, subcodeLineItem);
                    }
                    else if (qtySubcodeLines == 0 && itemToLinkTo == null) //no subcode lines found and no item to link to so just add the item directly
                    {
                        transaction.Add(subcodeLineItem);
                    }
                    else
                    {
                        SaleLineItem sli = (SaleLineItem) transaction.SaleItems.Where(w => w.LinkedToLineId == itemToLinkTo.LineId).OrderByDescending(o => o.LineId).FirstOrDefault();
                        if (sli != null)
                        {
                            transaction.Insert(sli, subcodeLineItem);
                        }
                    }
                }

                #endregion

                
                if (onlyCalculateLastLine)
                {
                    //Calculate price/tax/discount of last line added -> the subcode line
                    Interfaces.Services.TransactionService(entry).CalculatePriceTaxDiscount(entry, transaction);
                }
                else
                {
                    //Recalculate all prices
                    Interfaces.Services.TransactionService(entry).RecalculatePriceTaxDiscount(entry, transaction, true);
                }

                return itemToLinkTo;
            }            
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual bool ProcessLinkedInfocodes(IConnectionManager entry, ISession session, IPosTransaction posTransaction, ISaleLineItem saleLineItem, InfoCodeLineItem.TableRefId tableRefId, InfoCodeLineItem.InfocodeType infocodeType)
        {
            try
            {
                //Loop while a linkedInfoCode is found
                int i = 0; //Set as a stop for a infinite loop
                bool infoCodeContinue = true;
                InfoCodeLineItem infoCodeItem = saleLineItem.InfoCodeLines.Count > 0 ? saleLineItem.InfoCodeLines[saleLineItem.InfoCodeLines.Count - 1] : null;
                if (infoCodeItem != null)
                {
                    while (infoCodeItem.LinkedInfoCodeId != "" && infoCodeItem.LinkedInfoCodeId != null && i < 10)
                    {
                        infoCodeContinue = ProcessInfocode(entry, session, posTransaction, saleLineItem.Quantity, saleLineItem.GrossAmountWithTax, saleLineItem.ItemId, "", "", tableRefId, infoCodeItem.LinkedInfoCodeId, infoCodeItem, infocodeType, ref saleLineItem, true);
                        if (!infoCodeContinue)
                            return false;
                        // This is to prevent an infinite loop when infocodes link to themselves..
                        if (infoCodeItem.LinkedInfoCodeId == (saleLineItem.InfoCodeLines.Count > 0 ? saleLineItem.InfoCodeLines[saleLineItem.InfoCodeLines.Count - 1] : null).LinkedInfoCodeId)
                            break;
                        infoCodeItem = saleLineItem.InfoCodeLines.Count > 0 ? saleLineItem.InfoCodeLines[saleLineItem.InfoCodeLines.Count - 1] : null;
                        i++;
                    }
                }
                return true;
            }
            catch (POSException px)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                throw px;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual bool ProcessLinkedInfocodes(IConnectionManager entry, ISession session, IPosTransaction posTransaction, ITenderLineItem tenderLineItem, string storeId, InfoCodeLineItem.TableRefId tableRefId, InfoCodeLineItem.InfocodeType infocodeType)
        {
            try
            {
                //Loop while a linkedInfoCode is found
                int i = 0; //Set as a stop for a infinite loop
                bool infoCodeContinue = true;
                InfoCodeLineItem infoCodeItem = tenderLineItem.InfoCodeLines.Count > 0 ? tenderLineItem.InfoCodeLines[tenderLineItem.InfoCodeLines.Count - 1] : null;
                if (infoCodeItem != null)
                {
                    while (infoCodeItem.LinkedInfoCodeId != "" && infoCodeItem.LinkedInfoCodeId != null && i < 10)
                    {
                        infoCodeContinue = ProcessInfocode(entry, session, posTransaction, 0, tenderLineItem.Amount, storeId, tenderLineItem.TenderTypeId, tenderLineItem.CurrencyCode, tableRefId, infoCodeItem.LinkedInfoCodeId, infoCodeItem, infocodeType, true);
                        if (!infoCodeContinue)
                            return false;
                        // This is to prevent an infinite loop when infocodes link to themselves..
                        if (infoCodeItem.LinkedInfoCodeId == (tenderLineItem.InfoCodeLines.Count > 0 ? tenderLineItem.InfoCodeLines[tenderLineItem.InfoCodeLines.Count - 1] : null).LinkedInfoCodeId)
                            break;
                        infoCodeItem = tenderLineItem.InfoCodeLines.Count > 0 ? tenderLineItem.InfoCodeLines[tenderLineItem.InfoCodeLines.Count - 1] : null;
                        i++;
                    }
                }
                return true;
            }
            catch (POSException px)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), px);
                throw px;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual bool ProcessLinkedInfocodes(IConnectionManager entry, ISession session, IPosTransaction posTransaction, InfoCodeLineItem.TableRefId tableRefId, InfoCodeLineItem.InfocodeType infocodeType)
        {
            try
            {
                bool infoCodeContinue = true;
                if (posTransaction.GetType() == typeof(CustomerPaymentTransaction))
                {
                    //Loop while a linkedInfoCode is found
                    int i = 0; //Set as a stop for a infinite loop
                    InfoCodeLineItem infoCodeItem = ((CustomerPaymentTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((CustomerPaymentTransaction)posTransaction).InfoCodeLines[((CustomerPaymentTransaction)posTransaction).InfoCodeLines.Count - 1] : null;
                    if (infoCodeItem != null)
                    {
                        while (infoCodeItem.LinkedInfoCodeId != "" && infoCodeItem.LinkedInfoCodeId != null && i < 10)
                        {
                            infoCodeContinue = ProcessInfocode(entry, session, posTransaction, 0, 0, (string)((CustomerPaymentTransaction)posTransaction).Customer.ID, "", "", InfoCodeLineItem.TableRefId.Customer, infoCodeItem.LinkedInfoCodeId, infoCodeItem, InfoCodeLineItem.InfocodeType.Header, true);
                            if (!infoCodeContinue)
                                return false;
                            // This is to prevent an infinite loop when infocodes link to themselves..
                            if (infoCodeItem.LinkedInfoCodeId == (((CustomerPaymentTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((CustomerPaymentTransaction)posTransaction).InfoCodeLines[((CustomerPaymentTransaction)posTransaction).InfoCodeLines.Count - 1] : null).LinkedInfoCodeId)
                                break;
                            infoCodeItem = ((CustomerPaymentTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((CustomerPaymentTransaction)posTransaction).InfoCodeLines[((CustomerPaymentTransaction)posTransaction).InfoCodeLines.Count - 1] : null;
                            i++;
                        }
                    }
                }
                else if (posTransaction.GetType() == typeof(NoSaleTransaction))
                {
                    //Loop while a linkedInfoCode is found
                    int i = 0; //Set as a stop for a infinite loop                
                    InfoCodeLineItem infoCodeItem = ((NoSaleTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((NoSaleTransaction)posTransaction).InfoCodeLines[((NoSaleTransaction)posTransaction).InfoCodeLines.Count - 1] : null;
                    if (infoCodeItem != null)
                    {
                        while (infoCodeItem.LinkedInfoCodeId != "" && infoCodeItem.LinkedInfoCodeId != null && i < 10)
                        {
                            infoCodeContinue = ProcessInfocode(entry, session, posTransaction, 0, 0, "", "", "",
                                tableRefId, infoCodeItem.LinkedInfoCodeId, infoCodeItem, infocodeType, true);

                            if (!infoCodeContinue)
                                return false;
                            // This is to prevent an infinite loop when infocodes link to themselves..
                            if (infoCodeItem.LinkedInfoCodeId == (((NoSaleTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((NoSaleTransaction)posTransaction).InfoCodeLines[((NoSaleTransaction)posTransaction).InfoCodeLines.Count - 1] : null).LinkedInfoCodeId)
                                break;
                            infoCodeItem = ((NoSaleTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((NoSaleTransaction)posTransaction).InfoCodeLines[((NoSaleTransaction)posTransaction).InfoCodeLines.Count - 1] : null;
                            i++;
                        }
                    }
                }
                else if (posTransaction.GetType() == typeof(TenderDeclarationTransaction))
                {
                    //Loop while a linkedInfoCode is found
                    int i = 0; //Set as a stop for a infinite loop                
                    InfoCodeLineItem infoCodeItem = ((TenderDeclarationTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((TenderDeclarationTransaction)posTransaction).InfoCodeLines[((TenderDeclarationTransaction)posTransaction).InfoCodeLines.Count - 1] : null;
                    if (infoCodeItem != null)
                    {
                        while (infoCodeItem.LinkedInfoCodeId != "" && infoCodeItem.LinkedInfoCodeId != null && i < 10)
                        {
                            infoCodeContinue = ProcessInfocode(entry, session, posTransaction, 0, 0, "", "", "",
                                tableRefId, infoCodeItem.LinkedInfoCodeId, infoCodeItem, infocodeType, true);

                            if (!infoCodeContinue)
                                return false;
                            // This is to prevent an infinite loop when infocodes link to themselves..
                            if (infoCodeItem.LinkedInfoCodeId == (((TenderDeclarationTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((TenderDeclarationTransaction)posTransaction).InfoCodeLines[((TenderDeclarationTransaction)posTransaction).InfoCodeLines.Count - 1] : null).LinkedInfoCodeId)
                                break;
                            infoCodeItem = ((TenderDeclarationTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((TenderDeclarationTransaction)posTransaction).InfoCodeLines[((TenderDeclarationTransaction)posTransaction).InfoCodeLines.Count - 1] : null;
                            i++;
                        }
                    }
                }
                else
                {
                    //Loop while a linkedInfoCode is found
                    int i = 0; //Set as a stop for a infinite loop                
                    InfoCodeLineItem infoCodeItem = ((RetailTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((RetailTransaction)posTransaction).InfoCodeLines[((RetailTransaction)posTransaction).InfoCodeLines.Count - 1] : null;
                    if (infoCodeItem != null)
                    {
                        while (infoCodeItem.LinkedInfoCodeId != "" && infoCodeItem.LinkedInfoCodeId != null && i < 10)
                        {
                            infoCodeContinue = ProcessInfocode(entry, session, posTransaction, 0, 0, (string) ((RetailTransaction) posTransaction).Customer.ID, "", "", tableRefId, infoCodeItem.LinkedInfoCodeId, infoCodeItem, infocodeType, true);
                            
                            if (!infoCodeContinue)
                                return false;
                            // This is to prevent an infinite loop when infocodes link to themselves..
                            if (infoCodeItem.LinkedInfoCodeId == (((RetailTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((RetailTransaction)posTransaction).InfoCodeLines[((RetailTransaction)posTransaction).InfoCodeLines.Count - 1] : null).LinkedInfoCodeId)
                                break;
                            infoCodeItem = ((RetailTransaction)posTransaction).InfoCodeLines.Count > 0 ? ((RetailTransaction)posTransaction).InfoCodeLines[((RetailTransaction)posTransaction).InfoCodeLines.Count - 1] : null;
                            i++;
                        }
                    }
                }
                return true;
            }
            catch (Exception x)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), x);
                throw x;
            }
        }

        public virtual bool ChangeTaxGroup(IConnectionManager entry, IPosTransaction posTransaction, string infocodeID)
        {            
            bool transactionChanged = false;
            List<InfocodeSubcode> subcodes = Providers.InfocodeSubcodeData.GetListForInfocodeTaxGroupOnly(entry, infocodeID, InfocodeSubcodeSorting.Description, false);
            subcodes.Add(new InfocodeSubcode()
                             {
                                 SubcodeId = "Default",
                                 InfocodeId = infocodeID,
                                 Text = Properties.Resources.Default,
                                 PriceType = PriceTypes.None
                             });

            Infocode infoCode = Providers.InfocodeData.Get(entry, infocodeID);

            using (SelectionDialog dialog = new SelectionDialog(new InfocodeSelectionList(subcodes), infoCode.Prompt, infoCode.InputRequired))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    InfocodeSubcode selectedSubcode = (InfocodeSubcode)dialog.SelectedItem;

                    RetailTransaction transaction = (RetailTransaction)posTransaction;

                    if (selectedSubcode.SubcodeId == "Default")
                    {
                        transaction.UseOverrideTaxGroup = false;
                        transaction.OverrideTaxGroup = "";
                    }
                    else
                    {
                        transaction.UseOverrideTaxGroup = true;
                        transaction.OverrideTaxGroup = (string)selectedSubcode.TriggerCode;
                    }

                    transactionChanged = true;
                }
            }

            return transactionChanged;
        }

        protected virtual PopUpFormData GetPopUpFormData(IConnectionManager entry, string infoCodeId, bool automaticTriggering, ref ISaleLineItem saleLineItem, ref IPosTransaction posTransaction)
        {
            PopUpFormData popUpFormData = new PopUpFormData();

            Infocode infocode = Providers.InfocodeData.Get(entry, infoCodeId);
            List<InfocodeSubcode> subcodes = Providers.InfocodeSubcodeData.GetListForInfocode(entry, infoCodeId, InfocodeSubcodeSorting.ListType, false);
            List<InfocodeSubcode> subSubCodes;
            Item item;
            PopupPrimaryKey primaryKey;
            UsageCategories category;
            if(subcodes == null)
            {
                return popUpFormData;
            }
            popUpFormData.HeaderText = infocode.ExplanatoryHeaderText;
            category = (UsageCategories) infocode.UsageCategory;
            for (int i = 0 ; i < subcodes.Count; i++)
            {
                infocode = Providers.InfocodeData.Get(entry, subcodes[i].TriggerCode);
                Group group = new Group();
                group.Index = i;
                group.GroupId = (string)subcodes[i].TriggerCode;
                group.Text = subcodes[i].Text;
                group.GroupHeader = infocode.ExplanatoryHeaderText;
                group.MinSelection = infocode.MinSelection;
                group.MaxSelection = infocode.MaxSelection;
                group.MultipleSelection = infocode.MultipleSelection;
                group.InputRequired = infocode.InputRequired;
                group.UsageCategory = category;
                group.OKPressdAction = (OKPressedActions)infocode.OkPressedAction;
                group.NumberOfClicks = 0;
                popUpFormData.Groups.Add(group);
                subSubCodes = Providers.InfocodeSubcodeData.GetListForInfocode(entry, subcodes[i].TriggerCode, InfocodeSubcodeSorting.ListType, false);
                for(int j = 0; j < subSubCodes.Count; j++)
                {
                    item = new Item();
                    primaryKey = new PopupPrimaryKey();
                    primaryKey.SubCodeId = (string)subSubCodes[j].SubcodeId;
                    primaryKey.InfocodeId = (string)subcodes[i].TriggerCode;
                    item.PrimaryKey = primaryKey;
                    item.Index = j;
                    item.GroupId = (string)subcodes[i].TriggerCode;
                    item.UseageCategory = category;
                    item.Text = subSubCodes[j].Text;
                    item.MaxSelection = subSubCodes[j].MaxSelection;
                    item.PriceHandling = (LSOne.Services.Interfaces.Enums.PriceHandlings)subSubCodes[j].PriceHandling;
                    item.PriceType = subSubCodes[j].PriceType;
                    item.AmountPercentage = subSubCodes[j].AmountPercent;
                    item.NumberOfClicks = 0;
                    if((item.UseageCategory == UsageCategories.CrossSelling || item.UseageCategory == UsageCategories.ItemModifier) && subSubCodes[j].TriggerFunction != TriggerFunctions.None)
                    {
                        item.ItemId = (string)subSubCodes[j].TriggerCode;
                    }
                    else
                    {
                        item.ItemId = (string)subSubCodes[j].SubcodeId;
                    }
                    popUpFormData.Items.Add(item);
                }
            }

            //If the infocodes are triggered more than once, information about what has been triggered already has to be updated.
            if (!automaticTriggering)
            {
                foreach (SaleLineItem lineItem in ((RetailTransaction)posTransaction).SaleItems)
                {
                    if (lineItem.IsLinkedItem)
                    {
                        if(saleLineItem.LineId == lineItem.LinkedToLineId)
                        {
                            popUpFormData.AddNumberOfClicks(lineItem.ItemId);
                        }
                    }
                }
            }


            return popUpFormData;
        }

        public virtual IErrorLog ErrorLog { set; private get; }

        public void Init(IConnectionManager entry)
        {
#pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
#pragma warning restore 0612, 0618
        }
    }
}

