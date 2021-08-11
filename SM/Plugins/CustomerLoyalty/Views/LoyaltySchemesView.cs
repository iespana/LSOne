using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.CustomerLoyalty.Dialogs;
using LSOne.ViewPlugins.CustomerLoyalty.Properties;
using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerLoyalty.Views
{
	public partial class LoyaltySchemesView : ViewBase
	{
		private RecordIdentifier selectedId;
        private LoyaltyCustomerParams loyaltyParams;
	    private SiteServiceProfile siteServiceProfile;
	    private Parameters paramsData;


		public LoyaltySchemesView(RecordIdentifier selectedId)
			: this()
		{
			this.selectedId = selectedId;
		}


        public LoyaltySchemesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Help |
                         ViewAttributes.Close |
                         ViewAttributes.ContextBar;

            loyaltyParams = Providers.LoyaltyCustomerParamsData.Get(PluginEntry.DataModel);

            var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.SchemesEdit);

            HeaderText = Properties.Resources.LoyaltySchemesView; // view header text

            btnsContextButtons.AddButtonEnabled = btnsContextButtonsItems.AddButtonEnabled && PluginEntry.DataModel.HasPermission(Permission.SchemesEdit);
            btnsContextButtonsItems.AddButtonEnabled = btnsContextButtons.AddButtonEnabled;

            // These four lines allow you to capture right click-ing on the lists. The functions lvGroups_Opening and lvValues_Opening 
            // are run when the right click is performed
            gridObjects.ContextMenuStrip = new ContextMenuStrip();
            gridObjects.ContextMenuStrip.Opening += gridObjects_Opening;
            gridLines.ContextMenuStrip = new ContextMenuStrip();
            gridLines.ContextMenuStrip.Opening += gridLines_Opening;

            lblNoSelection.BringToFront();

            selectedId = RecordIdentifier.Empty;
        }

        public LoyaltySchemesView(LoyaltyCustomerParams loyaltyParamsValue)
		{
			InitializeComponent();

		    Attributes = ViewAttributes.Help |
		                 ViewAttributes.Close |
		                 ViewAttributes.ContextBar;

            loyaltyParams = loyaltyParamsValue;

			ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.SchemesEdit);

			HeaderText = Properties.Resources.LoyaltySchemesView; // view header text

			btnsContextButtons.AddButtonEnabled = btnsContextButtonsItems.AddButtonEnabled && PluginEntry.DataModel.HasPermission(Permission.SchemesEdit);
			btnsContextButtonsItems.AddButtonEnabled = btnsContextButtons.AddButtonEnabled;

			// These four lines allow you to capture right click-ing on the lists. The functions lvGroups_Opening and lvValues_Opening 
			// are run when the right click is performed
			gridObjects.ContextMenuStrip = new ContextMenuStrip();
			gridObjects.ContextMenuStrip.Opening += gridObjects_Opening;
			gridLines.ContextMenuStrip = new ContextMenuStrip();
			gridLines.ContextMenuStrip.Opening += gridLines_Opening;

			lblNoSelection.BringToFront();

			selectedId = RecordIdentifier.Empty;
		}

		// This code decides what should appear when the user views the audit log for this view (presses F6)
		// If you don't have any audit information then this can be empty.
		public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
		{
			if ((selectedId == null) || (selectedId.IsEmpty))
			{
				contexts.Add(new AuditDescriptor("LoyaltySchemes", selectedId, Properties.Resources.LoyaltySchemesView, false));
			}
			else
			{
				contexts.Add(new AuditDescriptor("LoyaltyScheme", selectedId, Properties.Resources.LoyaltySchemesView, true));
			}
		}

		protected override string LogicalContextName
		{
			get
			{
				return Properties.Resources.LoyaltySchemesView; // "Text above context bar";
			}
		}

		public override RecordIdentifier ID
		{
			get 
			{ 
				return RecordIdentifier.Empty;
			}
		}

        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "LoyaltySchemeRule")
            {
                LoadObjects();
            }
        }

		protected override void LoadData(bool isRevert)
		{
			LoadObjects();
		}

		protected override bool DataIsModified()
		{
			return false;
		}

		protected override bool SaveData()
		{
			return true;
		}

		private void LoadObjects()
        {
			gridObjects.ClearRows();

		    List<LoyaltySchemes> dataObjects;

            if (paramsData == null || siteServiceProfile == null)
            {
                paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
            }

            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            dataObjects = service.GetLoyaltySchemes(PluginEntry.DataModel, siteServiceProfile, true);
	
			int rowidx = -1;
			foreach (var dataObject in dataObjects)
			{
				var row = new Row();
				rowidx++;
				row.AddText((string)dataObject.ID);
				row.AddText(dataObject.Description);
				row.AddText(dataObject.ExpireTimeValue.ToString() + " " + dataObject.ExpirationTimeUnitAsString);
				row.AddText(dataObject.UseLimit.ToString());
				row.AddText(dataObject.CalculationTypeAsString);
				row.Tag = dataObject.ID;
				gridObjects.AddRow(row);

				if (selectedId == (RecordIdentifier)row.Tag)
				{
					gridObjects.Selection.Set(rowidx);
				}
			}

			gridObjects.AutoSizeColumns();
			gridObjects_SelectionChanged(this, EventArgs.Empty);
		}

		private void LoadLines()
		{
			if (gridObjects.Selection.Count == 0)
			{
				return;
			}
			if (gridObjects.Selection.FirstSelectedRow < 0)
			{
				return;
			}

            if (paramsData == null || siteServiceProfile == null)
            {
                paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
                siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);
            }

            var qtyLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
			var priceLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

			gridLines.ClearRows();

			var selectedObjectId = (RecordIdentifier)gridObjects.Row(gridObjects.Selection.FirstSelectedRow).Tag;
		    List<LoyaltyPoints> lines;

            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            lines = service.GetLoyaltySchemeRules(PluginEntry.DataModel, siteServiceProfile, selectedObjectId, true);

			int rowidx = -1;
			foreach (var line in lines)
			{
				var row = new Row();
				rowidx++;
				row.AddText(line.TypeAsString);
				row.AddText((string)line.SchemeRelation);
				row.AddText(line.SchemeRelationName);
				row.AddText(line.StartingDate.ToShortDateString());
				row.AddText(line.EndingDate.ToShortDateString());
				switch (line.BaseCalculationOn)
				{
					case CalculationTypeBase.Amounts:
						row.AddText(line.QtyAmountLimit.FormatWithLimits(priceLimit));
						break;
					case CalculationTypeBase.Quantity:
						row.AddText(line.QtyAmountLimit.FormatWithLimits(qtyLimit));
						break;
					default:
						row.AddText(line.QtyAmountLimit.ToString());
						break;
				}
				row.AddText(line.BaseCalculationOnAsString);
				row.AddText(line.Points.FormatWithLimits(qtyLimit));
				row.Tag = line.ID;

				gridLines.AddRow(row);

				if (selectedId == (RecordIdentifier)row.Tag)
				{
					gridLines.Selection.AddRows(rowidx, rowidx);
				}
			}

			gridLines.AutoSizeColumns();
			gridLines_SelectionChanged(this, EventArgs.Empty);
		}

		void gridObjects_Opening(object sender, CancelEventArgs e)
		{
			ContextMenuStrip menu = gridObjects.ContextMenuStrip;
			menu.Items.Clear();

			// We can optionally add our own items right here
			var item = new ExtendedMenuItem(
					Properties.Resources.Edit + "...",
					100,
					btnEdit_Click)
							{
								Enabled = btnsContextButtons.EditButtonEnabled,
								Image = ContextButtons.GetEditButtonImage(),
								Default = true
							};

			menu.Items.Add(item);

			item = new ExtendedMenuItem(
					Properties.Resources.Add + "...",
					200,
					btnAdd_Click)
						{
							Enabled = btnsContextButtons.AddButtonEnabled,
							Image = ContextButtons.GetAddButtonImage()
						};

			menu.Items.Add(item);

			item = new ExtendedMenuItem(
					Properties.Resources.Delete + "...",
					300,
					btnRemove_Click)
						{
							Enabled = btnsContextButtons.RemoveButtonEnabled,
							Image = ContextButtons.GetRemoveButtonImage()
						};

			menu.Items.Add(item);

			e.Cancel = (menu.Items.Count == 0);
		}

		void gridLines_Opening(object sender, CancelEventArgs e)
		{
			var menu = gridLines.ContextMenuStrip;

			menu.Items.Clear();

			// We can optionally add our own items right here
			var item = new ExtendedMenuItem(
					Properties.Resources.Edit + "...",
					100,
					btnEditLine_Click)
			{
				Enabled = btnsContextButtonsItems.EditButtonEnabled,
				Image = ContextButtons.GetEditButtonImage(),
				Default = true
			};

			menu.Items.Add(item);

			item = new ExtendedMenuItem(
					Properties.Resources.Add + "...",
					200,
					btnAddLine_Click)
						{
							Enabled = btnsContextButtonsItems.AddButtonEnabled && (gridObjects.Selection.Count != 0) && (gridObjects.Selection.FirstSelectedRow >= 0),
							Image = ContextButtons.GetAddButtonImage()
						};

			menu.Items.Add(item);

			item = new ExtendedMenuItem(
					Properties.Resources.Delete + "...",
					300,
					btnRemoveLine_Click)
						{
							Enabled = btnsContextButtonsItems.RemoveButtonEnabled,
							Image = ContextButtons.GetRemoveButtonImage()
						};

			menu.Items.Add(item);

			e.Cancel = (menu.Items.Count == 0);
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			bool objectSelected = (gridObjects.Selection.Count != 0) && (gridObjects.Selection.FirstSelectedRow >= 0);
			if (objectSelected)
			{
				var selectedObjectId = (RecordIdentifier)gridObjects.Row(gridObjects.Selection.FirstSelectedRow).Tag;
                var dlg = new LoyaltySchemeDialog(selectedObjectId,  siteServiceProfile);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					LoadObjects();
				}
			}
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			var dlg = new LoyaltySchemeDialog( siteServiceProfile);
			if (dlg.ShowDialog() == DialogResult.OK)
			{
			    selectedId = dlg.LoyaltyScheme.ID;
				LoadObjects();
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			bool objectSelected = (gridObjects.Selection.Count != 0) && (gridObjects.Selection.FirstSelectedRow >= 0);
			if (objectSelected)
			{
                var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                var selectedObjectId = (RecordIdentifier)gridObjects.Row(gridObjects.Selection.FirstSelectedRow).Tag;
			    if (service.LoyaltyCardExistsForLoyaltyScheme(PluginEntry.DataModel, siteServiceProfile, selectedObjectId, true))
			    {
			        MessageDialog.Show(Resources.LoyaltySchemeInUse);
			        return;
			    }
				if (QuestionDialog.Show(Properties.Resources.DeleteSchemeQuestion, Properties.Resources.LoyaltySchemesView) == DialogResult.Yes)
				{
				    
                    
                    service.DeleteLoyaltyScheme(PluginEntry.DataModel, siteServiceProfile, selectedObjectId, true);
				

					LoadObjects();
				}
			}
		}

		private void btnEditLine_Click(object sender, EventArgs e)
		{
			bool objectSelected = (gridLines.Selection.Count != 0) && (gridLines.Selection.FirstSelectedRow >= 0);
			if (objectSelected)
			{
				var selectedObjectId = (RecordIdentifier)gridLines.Row(gridLines.Selection.FirstSelectedRow).Tag;
				var dlg = new LoyaltySchemeRuleDialog(selectedObjectId.PrimaryID, selectedObjectId.SecondaryID, loyaltyParams, siteServiceProfile);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					LoadObjects();
				}
			}
		}

		private void btnAddLine_Click(object sender, EventArgs e)
		{
			bool objectSelected = (gridObjects.Selection.Count != 0) && (gridObjects.Selection.FirstSelectedRow >= 0);
			if (objectSelected)
			{
				var selectedObjectId = (RecordIdentifier)gridObjects.Row(gridObjects.Selection.FirstSelectedRow).Tag;
				var dlg = new LoyaltySchemeRuleDialog(selectedObjectId, loyaltyParams, siteServiceProfile);
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					LoadObjects();
				}
			}
		}

		private void btnRemoveLine_Click(object sender, EventArgs e)
		{
			bool objectSelected = (gridLines.Selection.Count != 0) && (gridLines.Selection.FirstSelectedRow >= 0);
			if (objectSelected)
			{
				if (QuestionDialog.Show(Properties.Resources.DeleteSchemeRuleQuestion, Properties.Resources.LoyaltySchemesView) == DialogResult.Yes)
				{
					var selectedObjectId = (RecordIdentifier)gridLines.Row(gridLines.Selection.FirstSelectedRow).Tag;

                    var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
                    service.DeleteLoyaltySchemeRule(PluginEntry.DataModel, siteServiceProfile, selectedObjectId, true);
                    
					LoadObjects();
				}
			}
		}

		private void gridObjects_SelectionChanged(object sender, EventArgs e)
		{
            int rowsSelected = gridObjects.Selection.Count;
            if (rowsSelected <= 0)
            {
                lblNoSelection.Visible = true;
            }
            else if (rowsSelected == 1)
            {
                selectedId = (RecordIdentifier) gridObjects.Row(gridObjects.Selection.FirstSelectedRow).Tag;
                lblNoSelection.Visible = false;
                
                LoadLines();
            }
            else
            {
                selectedId = RecordIdentifier.Empty;
                lblNoSelection.Visible = true;
            }

            btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled =
                    PluginEntry.DataModel.HasPermission(Permission.SchemesEdit) && rowsSelected > 0;

            gridLines.Visible = !lblNoSelection.Visible;
            btnsContextButtonsItems.Visible = gridLines.Visible;
            lblGroupHeader.Visible = gridLines.Visible;

		}

		private void gridLines_SelectionChanged(object sender, EventArgs e)
		{
			bool lineSelected = (gridLines.Selection.Count != 0) && (gridLines.Selection.FirstSelectedRow >= 0);
			btnsContextButtonsItems.EditButtonEnabled = btnsContextButtonsItems.RemoveButtonEnabled = lineSelected
				&& PluginEntry.DataModel.HasPermission(Permission.SchemesEdit);
		}

        private void gridObjects_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        private void gridLines_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsContextButtonsItems.EditButtonEnabled)
            {
                btnEditLine_Click(this, EventArgs.Empty);
            }
        }
	}
}
