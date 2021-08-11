using System;
using System.Windows.Forms;

using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Dialogs.Interfaces;

namespace LSOne.ViewPlugins.Inventory.Dialogs.WizardPages
{
	public partial class NewStockCountingJournal : UserControl, IWizardPage
	{
		WizardBase parent;
		private InventoryTypeAction inventoryTypeAction;

		public event EventHandler RequestFinish;

		public NewStockCountingJournal(WizardBase parent, InventoryTypeAction inventoryTypeAction) : base()
		{
			InitializeComponent();

			this.parent = parent;
			this.inventoryTypeAction = inventoryTypeAction;

			btnImportStockCounting.Enabled = PluginEntry.DataModel.HasPermission(Permission.StockCounting);
		}

		#region IWizardPage Members

		public InventoryTypeAction InventoryTypeAction
		{
			get { return inventoryTypeAction; }
		}

		public bool HasFinish
		{
			get { return false; }
		}

		public bool HasForward
		{
			get { return false; }
		}

		public Control PanelControl
		{
			get { return this; }
		}

		public void Display()
		{
			
		}

		public bool NextButtonClick(ref bool canUseFromForwardStack)
		{
			return true;
		}

		public IWizardPage RequestNextPage()
		{
			return null;
		}

		public void ResetControls()
		{

		}

		#endregion

		private void btnImportStockCounting_Click(object sender, EventArgs e)
		{ 
			inventoryTypeAction.Action = InventoryActionEnum.ExcelImport;
			parent.AddPage(new ImportSCJFromFile(parent, inventoryTypeAction));
		}

		private void btnManageStockCounting_Click(object sender, EventArgs e)
		{
			PluginEntry.Framework.ViewController.Add(new Views.StockCountingView());
			inventoryTypeAction.Action = InventoryActionEnum.Manage;
			RequestFinish?.Invoke(this, EventArgs.Empty);
		}

		private void btnNewStockCounting_Click(object sender, EventArgs e)
		{
			inventoryTypeAction.Action = InventoryActionEnum.New;
			parent.AddPage(new NewEmptySCJ(parent, inventoryTypeAction));
		}

		private void btnGenerateStockCounting_Click(object sender, EventArgs e)
		{
			inventoryTypeAction.Action = InventoryActionEnum.GenerateFromExisting;
			parent.AddPage(new StockCountingJournalSearch(parent, inventoryTypeAction));
		}

		private void btnFilter_Click(object sender, EventArgs e)
		{
			inventoryTypeAction.Action = InventoryActionEnum.GenerateFromFilter;
			parent.AddPage(new CreateFromFilter(parent, inventoryTypeAction));
		}

		private void btnViewStockCountingTemplates_Click(object sender, EventArgs e)
		{
			inventoryTypeAction.Action = InventoryActionEnum.GenerateFromTemplate;
			parent.AddPage(new NewInventoryDocumentFromTemplate(parent, inventoryTypeAction));
		}
	}
}
