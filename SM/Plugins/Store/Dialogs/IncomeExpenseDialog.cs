using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Financials;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Store.Dialogs
{
	public partial class IncomeExpenseDialog : DialogBase
	{
		DataEntity emptyItem;
		RecordIdentifier storeID;
		RecordIdentifier incomeExpenseId;
		IncomeExpenseAccount incomeExpenseAccount;
		bool isnew;
		

		public IncomeExpenseDialog(RecordIdentifier incomeExpenseId, RecordIdentifier storeID)
			: this(storeID)
		{
			this.incomeExpenseId = incomeExpenseId;
			isnew = false;

			incomeExpenseAccount = Providers.IncomeExpenseAccountData.Get(PluginEntry.DataModel, incomeExpenseId);
			tbName.Text = incomeExpenseAccount.Name;
			tbNameAlias.Text = incomeExpenseAccount.NameAlias;
			cmbAccountType.Text = (string)incomeExpenseAccount.AccountTypeText;            
			tbLedgerAccount.Text = (string)incomeExpenseAccount.LedgerAccount;         


		}
		
		public IncomeExpenseDialog()
		{
			incomeExpenseId = RecordIdentifier.Empty;
			incomeExpenseAccount = null;
			isnew = true;
			
			InitializeComponent();
		}

		public IncomeExpenseDialog(RecordIdentifier storeID)
			: this()
		{
			this.storeID = storeID;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);          

			emptyItem = new DataEntity(RecordIdentifier.Empty, Properties.Resources.DoNotCopyExistingIncomeExpense);
			cmbCopyFrom.SelectedData = emptyItem;
			tbStoreId.Text = (string)storeID;

		}

		public RecordIdentifier StoreID
		{
			get { return storeID; }
		}

		public RecordIdentifier IncomeExpenseId
		{
			get { return incomeExpenseId; }
			set { incomeExpenseId = value; }
		}

		private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
		{
			DataEntity item = ((DataEntity)e.Data);
			e.TextToDisplay = (item.ID == "" || item.ID == RecordIdentifier.Empty ? "" : item.ID.ToString() + " - ") + item.Text;
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{

			if (incomeExpenseAccount == null)
			{
				incomeExpenseAccount = new IncomeExpenseAccount();
			   
			}
		   
				incomeExpenseAccount.Name = tbName.Text;
				incomeExpenseAccount.StoreID = storeID;
				incomeExpenseAccount.NameAlias = tbNameAlias.Text;
				incomeExpenseAccount.AccountType = (IncomeExpenseAccount.AccountTypeEnum)cmbAccountType.SelectedIndex;
				incomeExpenseAccount.LedgerAccount = tbLedgerAccount.Text;

			
			Providers.IncomeExpenseAccountData.Save(PluginEntry.DataModel, incomeExpenseAccount, isnew);           

			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void CheckEnabled(object sender, EventArgs e)
		{
			if (isnew)
			{
				btnOK.Enabled = tbName.Text != "" && tbNameAlias.Text != ""  && cmbAccountType.SelectedIndex >= 0;
			}
			else
			{
				btnOK.Enabled = tbName.Text != "" && tbNameAlias.Text != "" && tbLedgerAccount.Text != "" && cmbAccountType.SelectedIndex >= 0 
				&& tbName.Text != incomeExpenseAccount.Name
				|| tbNameAlias.Text != incomeExpenseAccount.NameAlias
				|| (IncomeExpenseAccount.AccountTypeEnum)cmbAccountType.SelectedIndex != incomeExpenseAccount.AccountType
				|| tbLedgerAccount.Text != (string)incomeExpenseAccount.LedgerAccount;    
			}
			 

													

		}

		private void cmbCopyFrom_RequestData(object sender, EventArgs e)
		{
			List<DataEntity> list = Providers.IncomeExpenseAccountData
												.GetListForStore(PluginEntry.DataModel, IncomeExpenseAccount.AccountTypeEnum.All, storeID)
												.Cast<DataEntity>()
												.ToList();

			list.Insert(0, emptyItem);

			cmbCopyFrom.SetData(list,
				PluginEntry.Framework.GetImageList().Images[PluginEntry.StoreImageIndex], true);
		}

		private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
		{

			if (cmbCopyFrom.SelectedData.ID != RecordIdentifier.Empty)
			{
				incomeExpenseAccount = Providers.IncomeExpenseAccountData.Get(PluginEntry.DataModel, (string)cmbCopyFrom.SelectedData.ID);

				tbNameAlias.Text = incomeExpenseAccount.NameAlias;
				cmbAccountType.SelectedIndex = (int)incomeExpenseAccount.AccountType;
				tbLedgerAccount.Text = (string)incomeExpenseAccount.LedgerAccount;

				isnew = true;

				
			}

		}
	}
}
