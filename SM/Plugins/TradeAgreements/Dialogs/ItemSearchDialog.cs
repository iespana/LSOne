using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TradeAgreements.Properties;

namespace LSOne.ViewPlugins.TradeAgreements.Dialogs
{
    public partial class ItemSearchDialog : DialogBase
    {
        RetailItem retailItem;
        private RecordIdentifier[] itemIDs;

        private IConnectionManager connection;
        private IApplicationCallbacks callbacks;

        private int groupType;
        private string groupId;

        private List<RecordIdentifier> addedTargets;
        public ItemSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks, int groupType, string groupId, bool multiLineEnabled)
            :this()
        {
            this.connection = connection;
            this.callbacks = callbacks;
            this.groupType = groupType;
            this.groupId = groupId;
            lvItems.AutoSizeColumns();
        }


        public ItemSearchDialog(IConnectionManager connection, IApplicationCallbacks callbacks)
            :this()
        {
            this.connection = connection;
            this.callbacks = callbacks;
        }

        public ItemSearchDialog()
        {
            InitializeComponent();      
            addedTargets = new List<RecordIdentifier>();      
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return callbacks;
        }

        public RecordIdentifier[] GetItemIDs
        {
            get { return itemIDs; }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
          
                int i = 0;
                itemIDs = new RecordIdentifier[lvItems.Rows.Count+1];

                foreach (var item in lvItems.Rows   )
                {
                    itemIDs[i] = (RecordIdentifier)item.Tag;
                    i++;
                }
            
            RecordIdentifier target;
            if (cmbVariantNumber.SelectedData != null && cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty && cmbVariantNumber.SelectedData.ID != "")
            {
                target = (cmbVariantNumber.SelectedData as MasterIDEntity).ReadadbleID;

            }
            else
            {
                target = retailItem.ID;
            }
            if (target != null)
            {
                itemIDs[i] = target;
            }
            DialogResult = DialogResult.OK;
            btnAdd.Enabled = false;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lblToManyError.Visible = false;
            errorProvider1.Clear();
            RecordIdentifier target;
            if (cmbVariantNumber.SelectedData != null && cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty && cmbVariantNumber.SelectedData.ID != "")
            {
                target = (cmbVariantNumber.SelectedData as MasterIDEntity).ReadadbleID;

            }
            else
            {
                target = retailItem.ID;
            }
            if (!Providers.ItemInPriceDiscountGroupData.ItemIsInGroup(connection, target, groupType, groupId) &&
                !addedTargets.Contains(target))
            {
                addedTargets.Add(target);
                Row row = new Row();

                row.AddText((string) retailItem.ID); // Variant info - not applicable here
                row.AddText(retailItem.Text);

                row.AddText(retailItem.VariantName);
                row.Tag = retailItem.ID;

                IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete),
                    Properties.Resources.Delete, true);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", true));

                lvItems.AddRow(row);

                lvItems.AutoSizeColumns();
                cmbRelation.SelectedData = new DataEntity("", "");
                cmbVariantNumber.SelectedData = new DataEntity(RecordIdentifier.Empty, ""); ;
                btnAdd.Enabled = false;
            }
            else
            {
                errorProvider1.SetError(cmbVariantNumber.Enabled && cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty ? cmbVariantNumber : cmbRelation,
                    Resources.ItemAlreadyAdded);
            }
        }

     

        private void cmbRelation_DropDown(object sender, DropDownEventArgs e)
        {

            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = cmbRelation.SelectedData == null ? cmbRelation.Text : ((DataEntity)cmbRelation.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, true, initialSearchText,
                SearchTypeEnum.RetailItemsMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);

        }

        private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
        {
            retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedData.ID);

            if (retailItem == null)
            {
                throw new DataIntegrityException(typeof(RetailItem), cmbRelation.SelectedData.ID);
            }
            if (!RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID))
            {

                cmbRelation.SelectedData = new DataEntity(retailItem.HeaderItemID, retailItem.Text);
                cmbVariantNumber.SelectedData = new DataEntity(retailItem.MasterID, retailItem.VariantName);

            }
            else
            {

                cmbVariantNumber.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            }

            cmbVariantNumber.Enabled = retailItem.ItemType == ItemTypeEnum.MasterItem || !RecordIdentifier.IsEmptyOrNull(retailItem.HeaderItemID);
            lblVariantNumber.ForeColor = cmbVariantNumber.Enabled
                ? SystemColors.ControlText
                : SystemColors.GrayText;
            btnOK.Enabled = true;
            btnAdd.Enabled = true;
            errorProvider1.Clear();
        }

        private void cmbVariantNumber_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = cmbVariantNumber.SelectedData == null ? cmbVariantNumber.Text :((DataEntity)cmbVariantNumber.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, new List<RecordIdentifier>(), textInitallyHighlighted, true);
        }

        private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
        {
            retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbVariantNumber.SelectedData.ID);

            if (retailItem == null)
            {
                throw new DataIntegrityException(typeof(RetailItem), cmbRelation.SelectedData.ID);
            }

            btnAdd.Enabled = true;
            btnOK.Enabled = true;
            errorProvider1.Clear();
        }

        private void lvItems_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            lvItems.RemoveRow(args.RowNumber);

            lvItems.AutoSizeColumns();
            
        }
    }
}
