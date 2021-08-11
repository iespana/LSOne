using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Transactions;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.CentralSuspensions.Views
{
	public partial class SuspendedTransactionTypeView : ViewBase
	{
        SuspendedTransactionType suspendedTransactionType;
        List<SuspensionTypeAdditionalInfo> suspendedTypesAdditionalInfo;
        
        RecordIdentifier suspendedTransactionTypeID;

        public SuspendedTransactionTypeView(RecordIdentifier suspendedTransactionTypeID)
            : this()
	    {
            this.suspendedTransactionTypeID = suspendedTransactionTypeID;
            
	    }       

		public SuspendedTransactionTypeView()
		{
            
            suspendedTransactionTypeID = RecordIdentifier.Empty;

			InitializeComponent();

            Attributes = ViewAttributes.Delete |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help |
                ViewAttributes.RecordCursor |
                ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Audit;

            //HeaderIcon = Properties.Resources.Transaction16;
            HeaderText = Properties.Resources.SuspensionsType;

            lvAddinfo.ContextMenuStrip = new ContextMenuStrip();
            lvAddinfo.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            lvAddinfo.SortColumn = 1;
            lvAddinfo.SortedBackwards = false;

            btnDown.Enabled = true;
            btnUp.Enabled = true;

            
		}

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("SuspendedTransactionAddInfo", suspendedTransactionTypeID, Properties.Resources.SuspensionsType, false));
            
        }

   		protected override string LogicalContextName
		{
			get
			{
				return Properties.Resources.SuspensionsType;
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
            
            suspendedTransactionType = Providers.SuspendedTransactionTypeData.Get(PluginEntry.DataModel, suspendedTransactionTypeID);
            if (suspendedTransactionType != null)
            {
                tbDescription.Text = (string)suspendedTransactionType.Text;
                cmbAllowEOD.Text = (string)suspendedTransactionType.EndOfDayCodeText;
            }           
            
            LoadItems();             
		}

        private void LoadItems()
        {
            ListViewItem listItem;
            lvAddinfo.Items.Clear();           
          
            suspendedTypesAdditionalInfo = Providers.SuspensionTypeAdditionalInfoData.GetList(PluginEntry.DataModel, suspendedTransactionTypeID);

            foreach (SuspensionTypeAdditionalInfo suspendedTypeAdditionalInfo in suspendedTypesAdditionalInfo)
            {
                listItem = new ListViewItem(suspendedTypeAdditionalInfo.Text);

                listItem.SubItems.Add((string)suspendedTypeAdditionalInfo.InfotypeText);
                listItem.SubItems.Add((string)suspendedTypeAdditionalInfo.InfoTypeSelectionDescription);
                listItem.SubItems.Add(suspendedTypeAdditionalInfo.Required == true ? Properties.Resources.Yes : Properties.Resources.No);

                listItem.Tag = suspendedTypeAdditionalInfo;
                lvAddinfo.Add(listItem);

            }

            lvAddinfo.BestFitColumns();
            btnDown.Enabled = false;
            btnUp.Enabled = false;
            lvAddinfo_SelectedIndexChanged(this, EventArgs.Empty);

        }

		protected override bool DataIsModified()
		{
            if (tbDescription.Text != suspendedTransactionType.Text) return true;
            if (cmbAllowEOD.Text != suspendedTransactionType.EndOfDayCodeText) return true;

            return false;

		}

		protected override bool SaveData()
		{
            suspendedTransactionType.Text = tbDescription.Text;
            suspendedTransactionType.EndofDayCode = (SuspendedTransactionsStatementPostingEnum)cmbAllowEOD.SelectedIndex;

            Providers.SuspendedTransactionTypeData.Save(PluginEntry.DataModel, suspendedTransactionType);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "SuspensionsType", suspendedTransactionType.ID, null);

            return true;
		}

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "SuspensionsType":
                    PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    break;
                    
                case "SuspensionsTypeAdditionalInfo":                   
                    LoadItems();
                    break;
            }

        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (suspendedTransactionType != null)
            {
                if (arguments.CategoryKey == base.GetType().ToString() + ".View")
                {

                }
                else if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.ViewSuspendedTransactionTypes, new ContextbarClickEventHandler(PluginOperations.ShowSuspendedTransactionsTypeView)), 400);
                }
            }
        }

        protected override void OnClose()
        {
            lvAddinfo.SmallImageList = null;

            base.OnClose();
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {

            ContextMenuStrip menu = lvAddinfo.ContextMenuStrip;

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

            item = new ExtendedMenuItem(
                     Properties.Resources.MoveUp,
                     400,
                     new EventHandler(btnUp_Click));

            item.Image = ContextButtons.GetMoveUpButtonImage();
            item.Enabled = btnUp.Enabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.MoveDown,
                    500,
                    new EventHandler(btnDown_Click));

            item.Image = ContextButtons.GetMoveDownButtonImage();
            item.Enabled = btnDown.Enabled;

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteSuspensionType(suspendedTransactionTypeID);

                        
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.EditSuspensionAdditionalInfo(((SuspensionTypeAdditionalInfo)lvAddinfo.SelectedItems[0].Tag).ID, suspendedTransactionTypeID);
        }        

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvAddinfo.SelectedItems.Count == 1)
            {
                PluginOperations.DeleteSuspensionAdditionalInfo(((SuspensionTypeAdditionalInfo)lvAddinfo.SelectedItems[0].Tag).ID);
            }
            else
            {               
                PluginOperations.DeleteSuspensionsAdditionalInfo(lvAddinfo.GetSelectedDataEntities());
            }
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewSuspensionTypeAdditionalInfo(suspendedTransactionTypeID);           

        }

        private void lvAddinfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int currItemIndex;
                       
            btnsEditAddRemove.EditButtonEnabled = (lvAddinfo.SelectedItems.Count == 1);
            btnsEditAddRemove.RemoveButtonEnabled = (lvAddinfo.SelectedItems.Count >= 1);

            if (lvAddinfo.SelectedItems.Count == 1)
            {
                
                currItemIndex = lvAddinfo.FocusedItem.Index;
            }
            else
            {
                currItemIndex = 0;
            }

            if (currItemIndex == 0)
            {
                btnDown.Enabled = true;
                btnUp.Enabled = false;
            }
            else
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;

            }
            if (currItemIndex == lvAddinfo.Items.Count - 1)
            {
                btnDown.Enabled = false;
                btnUp.Enabled = true;               
            }
            if (lvAddinfo.Items.Count == 1 && currItemIndex == 0)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
            if (lvAddinfo.SelectedItems.Count > 1 || lvAddinfo.SelectedItems.Count == 0)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;

            }
                
        }

        private void lvAddinfo_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            
            // Get entity of selected line
            // Get entity of line above selected line
            if(lvAddinfo.SelectedItems.Count == 0)
            return;

            SuspensionTypeAdditionalInfo selectedEntity = (SuspensionTypeAdditionalInfo)lvAddinfo.SelectedItems[0].Tag;
            SuspensionTypeAdditionalInfo lineAboveEntity = (SuspensionTypeAdditionalInfo)lvAddinfo.Items[lvAddinfo.SelectedIndices[0] - 1].Tag;
               
            // Decrease selected line priority
            // Increase priority of entity above selected entity

            selectedEntity.Order--;
            lineAboveEntity.Order++;

            // Save both entities
            Providers.SuspensionTypeAdditionalInfoData.Save(PluginEntry.DataModel, selectedEntity);
            Providers.SuspensionTypeAdditionalInfoData.Save(PluginEntry.DataModel, lineAboveEntity);
            LoadItems();

            
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lvAddinfo.SelectedItems.Count == 0)
                return;

            SuspensionTypeAdditionalInfo selectedEntity = (SuspensionTypeAdditionalInfo)lvAddinfo.SelectedItems[0].Tag;
            SuspensionTypeAdditionalInfo lineBelowEntity = (SuspensionTypeAdditionalInfo)lvAddinfo.Items[lvAddinfo.SelectedIndices[0] + 1].Tag;

            selectedEntity.Order++;
            lineBelowEntity.Order--;

            Providers.SuspensionTypeAdditionalInfoData.Save(PluginEntry.DataModel, selectedEntity);
            Providers.SuspensionTypeAdditionalInfoData.Save(PluginEntry.DataModel, lineBelowEntity);

            
            LoadItems();

        }

       
	}
}
