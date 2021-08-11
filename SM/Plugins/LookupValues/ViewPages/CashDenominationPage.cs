using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;
using LSOne.DataLayer.GenericConnector.Enums;

namespace LSOne.ViewPlugins.LookupValues.ViewPages
{
    public partial class CashDenominationPage : UserControl, ITabView
    {
        RecordIdentifier currencyCode;

        public CashDenominationPage()
        {
            InitializeComponent();

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += new CancelEventHandler(lvValues_Opening);

            lvValues.SortColumn = 0;
            lvValues.SortedBackwards = false;

        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.CashDenominationPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            currencyCode = context;
            LoadLines(currencyCode);
        }

        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("CashDenomination", RecordIdentifier.Empty, Properties.Resources.CurrencyUnits, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            //switch (objectName)
            //{
            //    case "Currency":
            //        if (changeHint == DataEntityChangeType.VariableChanged)
            //        {
            //            currencyCode = changeIdentifier;
            //            LoadLines(currencyCode);
            //        }
            //        break;
            //}
        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
            lvValues.SmallImageList = null;
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        void lvValues_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd + "...",
                    100,
                    new EventHandler(btnsValueButtons_EditButtonClicked))
            {
                Enabled = btnsValueButtons.EditButtonEnabled,
                Image = ContextButtons.GetEditButtonImage()
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    new EventHandler(btnAddValue_Click))
            {
                Enabled = btnsValueButtons.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };

            menu.Items.Add(item);


            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    new EventHandler(btnRemoveValue_Click))
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsValueButtons.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("CashDenominationList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadLines(RecordIdentifier currencyCode)
        {
            ListViewItem listItem;

            if (currencyCode.IsEmpty)
            {
                return;
            }

            lvValues.Items.Clear();

            List<CashDenominator> lines = Providers.CashDenominatorData.GetCashDenominators(PluginEntry.DataModel, currencyCode, lvValues.SortColumn, lvValues.SortedBackwards);

            var rounding = Services.Interfaces.Services.RoundingService(PluginEntry.DataModel);

            DecimalLimit decimalSetting = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

            foreach (var line in lines)
            {
                listItem = new ListViewItem(line.CashType == CashDenominator.Type.Coin ? Properties.Resources.Coin : Properties.Resources.Bill);
                listItem.SubItems.Add(rounding.RoundString(PluginEntry.DataModel, line.Amount, (line.Amount < 1 ? decimalSetting.Max : decimalSetting.Min), false, line.CurrencyCode));
                listItem.SubItems.Add(line.Denomination);

                listItem.Tag = line;
                listItem.ImageIndex = -1;

                lvValues.Add(listItem);
            }

            lvValues.Columns[lvValues.SortColumn].ImageIndex = (lvValues.SortedBackwards ? 1 : 0);

            lvValues_SelectedIndexChanged(this, EventArgs.Empty);

            lvValues.BestFitColumns();

        }

        private void lvValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsValueButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CurrencyEdit);
            btnsValueButtons.RemoveButtonEnabled = btnsValueButtons.EditButtonEnabled = (lvValues.SelectedItems.Count > 0) && btnsValueButtons.AddButtonEnabled;
            
        }

        private void lvValues_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvValues.SortColumn == e.Column)
            {
                lvValues.SortedBackwards = !lvValues.SortedBackwards;
            }
            else
            {
                if (lvValues.SortColumn != -1)
                {
                    // Clear the old sorting
                    lvValues.Columns[lvValues.SortColumn].ImageIndex = 2;
                    lvValues.SortColumn = e.Column;
                }
                lvValues.SortedBackwards = false;
            }

            LoadLines(currencyCode);
        }

        private void btnAddValue_Click(object sender, EventArgs e)
        {
            Dialogs.CashDenominatorDialog dlg = new Dialogs.CashDenominatorDialog(currencyCode);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadLines(currencyCode);
            }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeleteCashDeclarationQuestion,
                Properties.Resources.DeleteCashDeclaration) == DialogResult.Yes)
            {
                Providers.CashDenominatorData.Delete(PluginEntry.DataModel, ((CashDenominator)lvValues.SelectedItems[0].Tag).ID);

                LoadLines(currencyCode);
            }
        }

        private void btnsValueButtons_EditButtonClicked(object sender, EventArgs e)
        {
            Dialogs.CashDenominatorDialog dlg = new Dialogs.CashDenominatorDialog((CashDenominator)lvValues.SelectedItems[0].Tag);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadLines(currencyCode);
            }
        }
    }
}
