using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.UserManagement;
using LSOne.Services.Interfaces.Enums;
using LSOne.Controls;
using LSOne.ViewCore.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    public partial class ButtonMenuButtonsPage : UserControl, ITabView
    {
        private PosMenuHeader posMenuHeader;
        private RecordIdentifier selectedID;
        private List<PosMenuLineListItem> posMenuLines;
        private int currItemIndex;

        public ButtonMenuButtonsPage()
        {
            InitializeComponent();

            lvPosMenuLines.ContextMenuStrip = new ContextMenuStrip();
            lvPosMenuLines.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            btnsEditAddRemovePosMenuLine.AddButtonEnabled = PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.EditPosMenus);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ButtonMenuButtonsPage();
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvPosMenuLines.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditText,
                    100,
                    btnsEditAddRemovePosMenuLine_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemovePosMenuLine.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsEditAddRemovePosMenuLine_AddButtonClicked);

            item.Enabled = btnsEditAddRemovePosMenuLine.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsEditAddRemovePosMenuLine_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemovePosMenuLine.RemoveButtonEnabled;

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                Properties.Resources.MoveLineUp,
                500,
                btnUp_Click);

            item.Image = ContextButtons.GetMoveUpButtonImage();
            item.Enabled = btnUp.Enabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.MoveLineDown,
                600,
                btnDown_Click);

            item.Image = ContextButtons.GetMoveDownButtonImage();
            item.Enabled = btnDown.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PosButtonGridMenuLineList", lvPosMenuLines.ContextMenuStrip, lvPosMenuLines);

            e.Cancel = (menu.Items.Count == 0);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuHeader = (PosMenuHeader)internalContext;
            LoadLines();
        }

        private void LoadLines()
        {
            RecordIdentifier oldSelectedId = selectedID;
            lvPosMenuLines.ClearRows();
            posMenuLines = Providers.PosMenuLineData.GetListItems(PluginEntry.DataModel, posMenuHeader.ID);

            foreach (var line in posMenuLines)
            {
                var row = new Row();
                row.AddText((line.KeyNo.ToString()));
                row.AddText(line.Text);
                row.AddText(line.OperationName);
                row.AddText(GetParameterText(line));
                if (line.StyleID != RecordIdentifier.Empty && line.StyleID != null)
                {
                    var style = Providers.PosStyleData.Get(PluginEntry.DataModel, line.StyleID);
                    if (style != null)
                        row.AddText(style.Text);
                }

                row.Tag = line;
                lvPosMenuLines.AddRow(row);

                if (line.ID == oldSelectedId)
                {
                    lvPosMenuLines.Selection.Set(lvPosMenuLines.RowCount - 1);
                }
            }

            lvPosMenuLines_SelectionChanged(this, EventArgs.Empty);
        }

        private string GetParameterText(PosMenuLineListItem listItem)
        {
            string returnValue;

            switch (listItem.OperationLookupType)
            {
                case LookupTypeEnum.None:
                    return "";

                case LookupTypeEnum.RetailItems:
                    return listItem.ItemName;

                case LookupTypeEnum.StorePaymentTypes:
                    return listItem.PaymentTypeName;

                case LookupTypeEnum.PosMenu:
                    return listItem.PosMenuName;

                case LookupTypeEnum.TaxGroupInfocodes:
                    return listItem.InfocodeName;

                case LookupTypeEnum.PosMenuAndButtonGrid:
                    string[] menuAndButtonGrid = listItem.Parameter.Split(';');
                    returnValue = listItem.PosMenuAndButtonGridPosMenuName;

                    if (menuAndButtonGrid.Length == 2)
                    {
                        List<DataEntity> buttonGrids = Providers.TouchLayoutData.GetButtonGrids();

                        DataEntity buttonGrid = buttonGrids.First(p => p.ID.ToString() == menuAndButtonGrid[1]);

                        returnValue += " - " + buttonGrid.Text;
                    }

                    return returnValue;

                case LookupTypeEnum.SuspendedTransactionTypes:
                    return listItem.SuspensionTypeName;

                case LookupTypeEnum.BlankOperations:
                    return listItem.BlankOperationParameter == "" ? listItem.BlankOperationName : listItem.BlankOperationName + " - " + listItem.BlankOperationParameter;

                case LookupTypeEnum.IncomeAccounts:
                    return listItem.IncomeExpenseAccountName;

                case LookupTypeEnum.ExpenseAccounts:
                    return listItem.IncomeExpenseAccountName;

                case LookupTypeEnum.TextInput:
                    return listItem.Parameter;

                case LookupTypeEnum.NumericInput:
                    return listItem.Parameter;

                case LookupTypeEnum.StorePaymentTypeAndAmount:
                    string[] paymentTypeAndAmount = listItem.Parameter.Split(';');

                    // This field can be an empty string if the parameter does not contain a ";"
                    returnValue = listItem.StorePaymentAndAmountPaymentTypeName != "" ? listItem.StorePaymentAndAmountPaymentTypeName : listItem.PaymentTypeName;

                    if (paymentTypeAndAmount.Length == 2)
                    {
                        returnValue += " - " + paymentTypeAndAmount[1];
                    }

                    return returnValue;

                case LookupTypeEnum.Amount:
                    return listItem.Parameter;

                case LookupTypeEnum.ManuallyTriggerPeriodicDiscount:
                    return listItem.ManuallyTriggeredPeriodicDiscountName;

                case LookupTypeEnum.ItemSearch:
                    returnValue = "";
                    string[] itemSearch = listItem.Parameter.Split(';');

                    if (itemSearch.Length > 0 && itemSearch[0] != "")
                    {
                        ItemSearchViewModeEnum viewMode;
                        if (!Enum.TryParse(itemSearch[0], false, out viewMode))
                            viewMode = ItemSearchViewModeEnum.Default;

                        switch (viewMode)
                        {
                            case ItemSearchViewModeEnum.Default:
                                returnValue = Properties.Resources.SearchModeDefault;
                                break;
                            case ItemSearchViewModeEnum.List:
                                returnValue = Properties.Resources.SearchModeList;
                                break;
                            case ItemSearchViewModeEnum.Images:
                                returnValue = Properties.Resources.SearchModeImages;
                                break;
                        }
                    }

                    if (itemSearch.Length > 1)
                    {
                        var group = Providers.RetailGroupData.Get(PluginEntry.DataModel, itemSearch[1]);
                        returnValue += ";" + (group == null ? itemSearch[1] : group.Text);
                    }

                    return returnValue;

                case LookupTypeEnum.SalesPerson:
                    returnValue = "";

                    string[] salesPerson = listItem.Parameter.Split(';');

                    if (salesPerson.Length > 0 && salesPerson[0] != "")
                    {
                        POSUser usr = Providers.POSUserData.Get(PluginEntry.DataModel, salesPerson[0], UsageIntentEnum.Normal);
                        returnValue = usr == null ? Properties.Resources.SalesPersonNotFound : PluginEntry.DataModel.Settings.NameFormatter.Format(usr.Name);
                    }

                    if (salesPerson.Length > 1)
                    {
                        returnValue = returnValue.Trim() != "" ? returnValue + " - " : returnValue;

                        returnValue += salesPerson[1] == "Y" ? Properties.Resources.YesHereafter : Properties.Resources.NoHereafter;
                    }

                    return returnValue;

                case LookupTypeEnum.Boolean:
                    returnValue = "";

                    if ((int)listItem.Operation == (int)POSOperations.PrintZ)
                    {
                        returnValue = Properties.Resources.RunEndOfDayLabelText + " ";
                    }
                    else if((int)listItem.Operation == (int)POSOperations.PriceCheck)
                    {
                        returnValue = Properties.Resources.ShowInventoryStatus + " ";
                    }
                    else if((int)listItem.Operation == (int)POSOperations.InventoryLookup)
                    {
                        returnValue = Properties.Resources.ShowPrice + " ";
                    }

                    returnValue += string.IsNullOrEmpty(listItem.Parameter) || listItem.Parameter == "N" ? Properties.Resources.No : Properties.Resources.Yes;

                    return returnValue;

                case LookupTypeEnum.LookupJob:
                    break;
                case LookupTypeEnum.ReprintReceipt:
                    returnValue = "";
                    List<DataEntity> reprintEntities = new List<DataEntity>();
                    reprintEntities.Add(new DataEntity("1", Properties.Resources.GiftReceipt));
                    reprintEntities.Add(new DataEntity("2", Properties.Resources.CopyLastReceipt));
                    reprintEntities.Add(new DataEntity("3", Properties.Resources.TaxFreeReceipt));
                    reprintEntities.Add(new DataEntity("4", Properties.Resources.CustomReceipt));
                    string[] strParams = listItem.Parameter.Split(';');

                    if (strParams.Length > 0 && strParams[0] != "")
                    {
                        DataEntity entity = reprintEntities.FirstOrDefault(f => f.ID == strParams[0]);
                        returnValue = entity == null ? "" : entity.Text;
                    }

                    if (strParams.Length > 1)
                    {
                        returnValue = returnValue.Trim() != "" ? returnValue + " - " : returnValue;
                        returnValue += strParams[1];

                    }
                    return returnValue;
                case LookupTypeEnum.ReasonCode:
                    returnValue = "";
                    if (!string.IsNullOrEmpty(listItem.Parameter))
                    {
                        string[] reasonParams = listItem.Parameter.Split(';');

                        if (reasonParams.Length > 0 && reasonParams[0] != "")
                        {
                            List<DataEntity> reasonTypes = new List<DataEntity>();
                            reasonTypes.Add(new DataEntity("0", Properties.Resources.Default));
                            reasonTypes.Add(new DataEntity("1", Properties.Resources.List));
                            reasonTypes.Add(new DataEntity("2", Properties.Resources.Specific));
                            DataEntity reasonEntity = reasonTypes.FirstOrDefault(f => f.ID == reasonParams[0]);
                            returnValue = reasonEntity == null ? "" : reasonEntity.Text;
                        }
                    }
                    return returnValue;
                case LookupTypeEnum.InfocodeOnRequest:
                    returnValue = "";
                    if (!string.IsNullOrEmpty(listItem.Parameter))
                    {
                        string[] reasonParams = listItem.Parameter.Split(';');

                        if (reasonParams.Length > 0 && reasonParams[0] != "")
                        {
                            List<DataEntity> triggerFor = new List<DataEntity>();
                            triggerFor.Add(new DataEntity("0", Properties.Resources.InfocodeOnRequest_Item));
                            triggerFor.Add(new DataEntity("1", Properties.Resources.InfocodeOnRequest_Sale));
                            DataEntity infocodeEntity = triggerFor.FirstOrDefault(f => f.ID == reasonParams[0]);
                            returnValue = infocodeEntity == null ? "" : infocodeEntity.Text;
                        }
                    }
                    return returnValue;
                case LookupTypeEnum.LastSaleOrReceiptID:
                    return listItem.Parameter == "1" ? Properties.Resources.LastReceipt : Properties.Resources.SelectAReceiptID;
                case LookupTypeEnum.PrintGroup:
                    int printGroupIndex = 0;
                    int.TryParse(listItem.Parameter, out printGroupIndex);
                    return ItemSaleReportGroupHelper.ItemSaleReportGroupToString((ItemSaleReportGroupEnum)printGroupIndex);
                case LookupTypeEnum.TransferRequests:
                case LookupTypeEnum.TransferOrders:
                    if (string.IsNullOrEmpty(listItem.Parameter))
                    {
                        return string.Empty;
                    }

                    return listItem.Parameter == "Incoming" ? Properties.Resources.Incoming : Properties.Resources.Outgoing;
                case LookupTypeEnum.MenuTypes:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return "";
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "PosButtonGridMenuLine")
            {
                LoadLines();
            }
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void lvPosMenuLines_SelectionChanged(object sender, EventArgs e)
        {
            if (lvPosMenuLines.Selection.FirstSelectedRow == -1)
            {
                btnDown.Enabled = false;
                btnUp.Enabled = false;
                btnsEditAddRemovePosMenuLine.EditButtonEnabled = false;
                btnsEditAddRemovePosMenuLine.RemoveButtonEnabled = false;
                return;
            }
            selectedID = (lvPosMenuLines.Selection.Count > 0) ? ((PosMenuLineListItem)(lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag)).ID : "";

            btnsEditAddRemovePosMenuLine.EditButtonEnabled = lvPosMenuLines.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.EditPosMenus);
            btnsEditAddRemovePosMenuLine.RemoveButtonEnabled = lvPosMenuLines.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.EditPosMenus);
            currItemIndex = ((PosMenuLineListItem)(lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag)).KeyNo;


            if (lvPosMenuLines.RowCount < 2 || lvPosMenuLines.Selection.Count == 0)
            {
                btnDown.Enabled = false;
                btnUp.Enabled = false;
                return;
            }

            if (currItemIndex == 1)
            {
                btnDown.Enabled = true;
                btnUp.Enabled = false;
            }
            else
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;
            }
            if (currItemIndex == lvPosMenuLines.RowCount)
            {
                btnDown.Enabled = false;
                btnUp.Enabled = true;
            }
            if (lvPosMenuLines.RowCount == 1 && currItemIndex == 1)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
            if (lvPosMenuLines.Selection.Count > 1 || lvPosMenuLines.Selection.Count == 0)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lvPosMenuLines.RowCount > 0)
            {
                var rowGoingUp = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag);
                var rowGoingDown = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow - 1).Tag);
                rowGoingUp.KeyNo--;
                rowGoingDown.KeyNo++;
                selectedID = rowGoingUp.ID;
                Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, rowGoingUp);
                Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, rowGoingDown);
            }

            LoadLines();
            lvPosMenuLines.Focus();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lvPosMenuLines.RowCount > 0)
            {
                var rowGoingDown = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag);
                var rowGoingUp = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow + 1).Tag);
                rowGoingUp.KeyNo--;
                rowGoingDown.KeyNo++;
                selectedID = rowGoingDown.ID;
                Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, rowGoingUp);
                Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, rowGoingDown);
            }

            LoadLines();
            lvPosMenuLines.Focus();
        }

        private void btnsEditAddRemovePosMenuLine_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewPosButtonGridMenuLine(posMenuHeader.ID, MenuTypeEnum.POSButtonGrid);
        }

        private void btnsEditAddRemovePosMenuLine_EditButtonClicked(object sender, EventArgs e)
        {
            List<PosMenuLine> recordBrowserContext = Providers.PosMenuLineData.GetList(PluginEntry.DataModel, posMenuHeader.ID);
            PluginOperations.ShowPosButtonGridMenuLine(selectedID, recordBrowserContext);
        }

        private void btnsEditAddRemovePosMenuLine_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvPosMenuLines.Selection.Count == 1)
            {
                var selectedPosMenusLineId = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag);

                foreach (var item in posMenuLines.Where(z => z.KeyNo > selectedPosMenusLineId.KeyNo))
                {
                    item.KeyNo--;
                    Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, item);
                }

                PluginOperations.DeletePosButtonGridMenuLine(selectedID);
            }
            else
            {
                var posMenuLineId = RecordIdentifier.Empty;
                if (lvPosMenuLines.Selection.Count > 0)
                {
                    if (
                        QuestionDialog.Show(Properties.Resources.DeletePosButtonGridMenuLineQuestion,
                                            Properties.Resources.DeletePosButtonGridMenuLine) == DialogResult.Yes)
                    {
                        for (int i = 0; i < lvPosMenuLines.Selection.Count; i++)
                        {
                            int selectedRowNumber = lvPosMenuLines.Selection.GetSelectedItem(i);
                            posMenuLineId = ((PosMenuLineListItem)(lvPosMenuLines.Row(selectedRowNumber).Tag)).ID;
                            Providers.PosMenuLineData.Delete(PluginEntry.DataModel, posMenuLineId);

                            var selectedPosMenusLineId = ((PosMenuLineListItem)lvPosMenuLines.Row(selectedRowNumber).Tag);
                            posMenuLines.Remove(selectedPosMenusLineId);

                        }

                        var newlist = posMenuLines.OrderBy(x => x.KeyNo);
                        int startNo = 0;
                        foreach (var posMenuLineListItem in newlist)
                        {
                            startNo++;
                            posMenuLineListItem.KeyNo = startNo;
                            Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, posMenuLineListItem);
                        }

                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete,
                                                                               "PosButtonGridMenuLine", posMenuLineId,
                                                                               null);
                    }
                }
            }
        }

        private void lvPosMenuLines_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (lvPosMenuLines.Selection.Count > 0 && btnsEditAddRemovePosMenuLine.EditButtonEnabled)
            {
                btnsEditAddRemovePosMenuLine_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
