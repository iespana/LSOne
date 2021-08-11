using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.CentralSuspensions.Views
{
	public partial class SuspendedTransactionsTypeView : ViewBase
	{
        List<SuspendedTransactionType> suspendedTransactions;
        
        
        RecordIdentifier selectedID = "";

		public SuspendedTransactionsTypeView()
		{
			InitializeComponent();

            Attributes =
                ViewAttributes.Help |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Audit;
                

            //HeaderIcon = Properties.Resources.Transaction16;
            HeaderText = Properties.Resources.SuspensionsTypes;

            lvSuspenedTransactionTypes.ContextMenuStrip = new ContextMenuStrip();
            lvSuspenedTransactionTypes.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);       
            


            lvSuspenedTransactionTypes.SortColumn = 0;
            lvSuspenedTransactionTypes.SortedBackwards = false;            

            //ResetRecordCounter();

            LoadItems();
		}
        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("SuspendedTransactionTypes", RecordIdentifier.Empty, Properties.Resources.SuspensionsType, true));

        }

		protected override string LogicalContextName
		{
			get
			{
				return Properties.Resources.SuspensionsTypes;
			}
		}

		public override RecordIdentifier ID
		{
			get 
			{ 
				return RecordIdentifier.Empty;
			}
		}

		protected override void LoadData(bool isRevert)
		{
		}

		protected override bool DataIsModified()
		{
			return false;
		}

		protected override bool SaveData()
		{
			return true;
		}

		public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
		{
            switch (objectName)
            {
                case "SuspensionsType":
                    LoadItems();
                    break;
            }

		}


        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewSuspensionType(this, EventArgs.Empty);
        }


        private void LoadItems()
        {
            ListViewItem listItem;

            lvSuspenedTransactionTypes.Items.Clear();       
            suspendedTransactions = Providers.SuspendedTransactionTypeData.GetList(PluginEntry.DataModel);

            foreach (SuspendedTransactionType suspendedTransaction in suspendedTransactions)
            {
               listItem = new ListViewItem(suspendedTransaction.Text);

               listItem.Tag = suspendedTransaction;

                lvSuspenedTransactionTypes.Add(listItem);
            }

            lvSuspenedTransactionTypes.Columns[lvSuspenedTransactionTypes.SortColumn].ImageIndex = (lvSuspenedTransactionTypes.SortedBackwards ? 1 : 0);
            lvSuspenedTransactionTypes_SelectedIndexChanged(this, EventArgs.Empty);
            lvSuspenedTransactionTypes.BestFitColumns();
            
        }

        protected override void OnClose()
        {
            lvSuspenedTransactionTypes.SmallImageList = null;

            base.OnClose();
        }

        private void lvSuspenedTransactionTypes_SelectedIndexChanged(object sender, EventArgs e)
        {

            selectedID = (lvSuspenedTransactionTypes.SelectedItems.Count == 1) ? ((SuspendedTransactionType)lvSuspenedTransactionTypes.SelectedItems[0].Tag).ID : RecordIdentifier.Empty;

            btnsEditAddRemove.EditButtonEnabled = (lvSuspenedTransactionTypes.SelectedItems.Count == 1);
            btnsEditAddRemove.RemoveButtonEnabled = (lvSuspenedTransactionTypes.SelectedItems.Count >= 1);          
                        
        }

        private void lvGiftCards_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (lvSuspenedTransactionTypes.SortColumn == e.Column)
            {
                lvSuspenedTransactionTypes.SortedBackwards = !lvSuspenedTransactionTypes.SortedBackwards;
            }
            else
            {
                lvSuspenedTransactionTypes.SortedBackwards = false;
            }



            if (lvSuspenedTransactionTypes.SortColumn != -1)
            {
                lvSuspenedTransactionTypes.Columns[lvSuspenedTransactionTypes.SortColumn].ImageIndex = 2;
                lvSuspenedTransactionTypes.SortColumn = e.Column;

            }

            //ResetRecordCounter();

            LoadItems();
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvSuspenedTransactionTypes.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked))
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemove.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);          
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("SuspensionTypeList", lvSuspenedTransactionTypes.ContextMenuStrip, lvSuspenedTransactionTypes);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowSuspendedTransactionType(selectedID);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {

            if (lvSuspenedTransactionTypes.SelectedItems.Count == 1)
            {

                PluginOperations.DeleteSuspensionType(selectedID);
            }
            else
            {
                PluginOperations.DeleteSuspensionTypes(lvSuspenedTransactionTypes.GetSelectedDataEntities());
            }

           
        }       

      

        private void lvSuspenedTransactionTypes_DoubleClick(object sender, EventArgs e)
        {
            
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
              
        }       
       

        
	}
}
