using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Financials;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

//using LSRetail.Utilities.Locale;
//using LSRetail.StoreController.Common.Settings;


namespace LSOne.ViewPlugins.Store.ViewPages
{
    public partial class IncomeExpenseAccountPage : UserControl, ITabView
    {
       
        private List<IncomeExpenseAccount> incomeExpenceItem;
        RecordIdentifier storeID;
        
        IncomeExpenseAccount.AccountTypeEnum selectedAccountTypeEnum;

        public IncomeExpenseAccountPage()
        {
            selectedAccountTypeEnum = IncomeExpenseAccount.AccountTypeEnum.All;          

            InitializeComponent();    
            cmbIncomeAccount.SelectedIndex = (int)IncomeExpenseAccount.AccountTypeEnum.All;
            lvIncomeExpense.ContextMenuStrip = new ContextMenuStrip();
            lvIncomeExpense.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);
          
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.IncomeExpenseAccountPage();
        }
       
        #region ITabPanel Members



        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
          
            storeID = context;

            lvIncomeExpense.Items.Clear();
            incomeExpenceItem = Providers.IncomeExpenseAccountData.GetListForStore(PluginEntry.DataModel, selectedAccountTypeEnum, storeID);

            ListViewItem listviewItem;
            foreach (IncomeExpenseAccount item in incomeExpenceItem)
            {
                listviewItem = new ListViewItem((string)(item.Name));
                listviewItem.SubItems.Add((string)item.NameAlias);
                listviewItem.SubItems.Add((string)item.LedgerAccount);
                listviewItem.SubItems.Add((string)item.AccountTypeText);
                

                listviewItem.Tag = item;
                lvIncomeExpense.Add(listviewItem);
            }

            lvIncomeExpense.BestFitColumns();

            lvIncomeExpense_SelectedIndexChanged(null, EventArgs.Empty);

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

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {               
                case "IncomeExpense":
                    LoadData(false, storeID, null);
                    break;
            }

        }

        public void OnClose()
        {
            // Avoid Microsoft memory leak bug in ListViews
           
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void lvCardTypes_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;  
        }

        public static void EditIncomeExpenseAccount(RecordIdentifier incomeExpenseAccountId)
        {
            
        }

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            Dialogs.IncomeExpenseDialog dlg = new Dialogs.IncomeExpenseDialog(storeID);

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "IncomeExpense", null, null);

            }
        }

        private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.IncomeExpenseView))
            {
               RecordIdentifier selectedID = ((IncomeExpenseAccount)lvIncomeExpense.SelectedItems[0].Tag).ID;
                  
                Dialogs.IncomeExpenseDialog dlg = new Dialogs.IncomeExpenseDialog(selectedID,storeID);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    selectedID = dlg.IncomeExpenseId;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "IncomeExpense", selectedID, null);
                }
            }
        }

        private void cmbIncomeAccount_SelectedIndexChanged(object sender, EventArgs e)
        {           
            
            selectedAccountTypeEnum = (IncomeExpenseAccount.AccountTypeEnum)cmbIncomeAccount.SelectedIndex;
            if (storeID != null)
            {
                LoadData(false, storeID, null);
            }
            

        }

        private void lvIncomeExpense_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnsContextButtons.EditButtonEnabled = (lvIncomeExpense.SelectedItems.Count == 1);
            btnsContextButtons.RemoveButtonEnabled = (lvIncomeExpense.SelectedItems.Count >= 1);

        }

        private void lvIncomeExpense_DoubleClick(object sender, EventArgs e)
        {
            btnsContextButtons_EditButtonClicked(this, null);
        }

        private void btnsContextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            RecordIdentifier selectedID = ((IncomeExpenseAccount)lvIncomeExpense.SelectedItems[0].Tag).ID;

            if (PluginEntry.DataModel.HasPermission(Permission.IncomeExpenseView))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteIncomeExpenseAccountQuestion, Properties.Resources.DeleteIncomeExpenseAccount) == DialogResult.Yes)
                {
                    Providers.IncomeExpenseAccountData.Delete(PluginEntry.DataModel, selectedID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "IncomeExpense", selectedID, null);
                }
            }
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

            ContextMenuStrip menu = lvIncomeExpense.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    new EventHandler(btnsContextButtons_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsContextButtons_AddButtonClicked));

            item.Enabled = btnsContextButtons.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsContextButtons_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;

            menu.Items.Add(item);



            e.Cancel = (menu.Items.Count == 0);
        }
         
    }
}
