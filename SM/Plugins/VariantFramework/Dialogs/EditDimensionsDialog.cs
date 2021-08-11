using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.VariantFramework.Properties;
using LSOne.Controls;

namespace LSOne.ViewPlugins.VariantFramework.Dialogs
{
    public partial class EditDimensionsDialog : DialogBase
    {
        private RetailItem item;
        private List<RetailItemDimension> dimensions;
        private OrderedDictionary retailItemDimensionAttributes;
        private Dictionary<RecordIdentifier, DimensionAttribute> dimensionAttributes;
        private RecordIdentifier selectedDimensionID;
        private RecordIdentifier selectedAttributeID;
        private bool isTemplate;

        private struct ItemDimensions
        {
            public RetailItemDimension Dimension;
            public List<DimensionAttribute> Attributes;
        }

        public EditDimensionsDialog(RetailItem item)
        {
            InitializeComponent();
            retailItemDimensionAttributes = new OrderedDictionary();
            dimensionAttributes = new Dictionary<RecordIdentifier, DimensionAttribute>();
            selectedDimensionID = RecordIdentifier.Empty;
            selectedAttributeID = RecordIdentifier.Empty;
            this.item = item;
            lvDimensions.ContextMenuStrip = new ContextMenuStrip();
            lvDimensionAttribute.ContextMenuStrip = new ContextMenuStrip();

            lvDimensions.ContextMenuStrip.Opening += DimensionsContextMenuStrip_Opening;
            lvDimensionAttribute.ContextMenuStrip.Opening += AttributesContextMenuStrip_Opening;

            lvDimensions.AutoSizeColumns();
            lvDimensionAttribute.AutoSizeColumns();

            btnsEditAddRemoveDimensionAttribute.AddButtonEnabled = false;

        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int dimensionSequence = 0;
            foreach (DictionaryEntry retailItemDimensionAttribute in retailItemDimensionAttributes)
            {
                dimensionSequence += 10;
                int attributeSequence = 0;
                ((ItemDimensions) retailItemDimensionAttribute.Value).Dimension.Sequence = dimensionSequence;
                Providers.RetailItemDimensionData.Save(PluginEntry.DataModel, ((ItemDimensions)retailItemDimensionAttribute.Value).Dimension);
                foreach (DimensionAttribute attribute in ((ItemDimensions)retailItemDimensionAttribute.Value).Attributes)
                {
                    attributeSequence += 10;
                    attribute.Sequence = attributeSequence;
                    if (RecordIdentifier.IsEmptyOrNull(attribute.DimensionID))
                    {
                        attribute.DimensionID = ((ItemDimensions) retailItemDimensionAttribute.Value).Dimension.ID;
                    }
                    Providers.DimensionAttributeData.Save(PluginEntry.DataModel, attribute);
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            dimensions = Providers.RetailItemDimensionData.GetListForRetailItem(PluginEntry.DataModel, item.MasterID);
            foreach (RetailItemDimension dimension in dimensions)
            {
                ItemDimensions itemDimensions = new ItemDimensions();
                itemDimensions.Dimension = dimension;
                itemDimensions.Attributes = Providers.DimensionAttributeData.GetListForRetailItemDimension(PluginEntry.DataModel, dimension.ID);
                retailItemDimensionAttributes.Add(dimension.ID, itemDimensions);
            }
            LoadDimensionListViews();
            lvDimensions.Selection.Set(0);
        }

        private void LoadDimensionListViews()
        {
            lvDimensions.ClearRows();
            Row row;
            foreach (DictionaryEntry retailItemDimensionAttribute in retailItemDimensionAttributes)
            {
                row = new Row();
                row.AddText(((ItemDimensions)retailItemDimensionAttribute.Value).Dimension.Text);
                row.Tag = retailItemDimensionAttribute.Key;
                lvDimensions.AddRow(row);
                if (((ItemDimensions)retailItemDimensionAttribute.Value).Dimension.ID == selectedDimensionID)
                {
                    lvDimensions.Selection.Set(lvDimensions.RowCount - 1);
                }
                PopulateDictionaries(((ItemDimensions)retailItemDimensionAttribute.Value).Dimension.ID);
            }
            lvDimensions.AutoSizeColumns();
        }

        private void LoadAttributeListView()
        {
            lvDimensionAttribute.ClearRows();
            if (lvDimensions.Selection.Count == 1)
            {
                List<DimensionAttribute> attributes = ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes;
                foreach (var attribute in attributes)
                {
                    Row row = new Row();
                    row.AddText(attribute.Text);
                    row.AddText(attribute.Code);
                    row.Tag = attribute.ID;
                    lvDimensionAttribute.AddRow(row);


                }
                lvDimensionAttribute.AutoSizeColumns();
                lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void PopulateDictionaries(RecordIdentifier retailItemDimensionID)
        {
            List<DimensionAttribute> attributes = ((ItemDimensions)retailItemDimensionAttributes[retailItemDimensionID]).Attributes;
            foreach (var attribute in attributes)
            {
                if (!dimensionAttributes.ContainsKey(attribute.ID))
                {
                    dimensionAttributes.Add(attribute.ID, attribute);
                }
            }
        }

        private void btnsEditAddRemoveDimension_AddButtonClicked(object sender, EventArgs e)
        {
            List<RetailItemDimension> retailItemDimensions = GetDimensions();
            Dialogs.AddNewDimensionDialog dlg = new AddNewDimensionDialog(retailItemDimensions);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                isTemplate = dlg.IsTemplate;
                RetailItemDimension retailItemDimension = dlg.RetailItemDimension;
                if (retailItemDimension != null)
                {
                    if (retailItemDimension.ID == RecordIdentifier.Empty)
                    {
                        retailItemDimension.ID = Guid.NewGuid();
                        retailItemDimension.RetailItemMasterID = item.MasterID;
                    }

                    ItemDimensions itemDimensions = new ItemDimensions();
                    itemDimensions.Dimension = retailItemDimension;
                    itemDimensions.Attributes = dlg.DimensionAttributes;
                    retailItemDimensionAttributes.Add(retailItemDimension.ID, itemDimensions);
                    PopulateDictionaries(retailItemDimension.ID);
                    Row row = new Row();
                    row.AddText(retailItemDimension.Text);
                    row.Tag = retailItemDimension.ID;
                    int latestDimensionSequence = lvDimensions.RowCount > 0
                                                    ? ((ItemDimensions)retailItemDimensionAttributes[(RecordIdentifier)lvDimensions.Row(lvDimensions.RowCount - 1).Tag]).Dimension.Sequence
                                                    : 0;
                    itemDimensions.Dimension.Sequence = latestDimensionSequence + 10;
                    lvDimensions.AddRow(row);
                    lvDimensions.AutoSizeColumns();
                    lvDimensions.Selection.Set(lvDimensions.RowCount - 1);
                    lvDimensions_SelectionChanged(this, EventArgs.Empty);
                }
            }

        }

        private void btnsEditAddRemoveDimensionAttribute_AddButtonClicked(object sender, EventArgs e)
        {
            List<DimensionAttribute> dimensionAttributes = ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes;
            Dialogs.AddNewDimensionAttributeDialog dlg = new AddNewDimensionAttributeDialog(dimensionAttributes);
            dlg.CreateWithoutClosing += CreateAttributeWithoutClosing;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                AddNewDimensionAttribute(dlg);
            }
            dlg.CreateWithoutClosing -= CreateAttributeWithoutClosing;
        }

        private void CreateAttributeWithoutClosing(object sender, EventArgs e)
        {
            AddNewDimensionAttribute((AddNewDimensionAttributeDialog)sender);
        }

        private void AddNewDimensionAttribute(AddNewDimensionAttributeDialog dlg)
        {
            DimensionAttribute dimensionAttribute = dlg.DimensionAttribute;
            if (dimensionAttribute != null)
            {
                Row row = new Row();
                row.AddText(dimensionAttribute.Text);
                row.AddText(dimensionAttribute.Code);
                dimensionAttribute.ID = Guid.NewGuid();
                dimensionAttribute.DimensionID = selectedDimensionID;
                row.Tag = dimensionAttribute.ID;
                ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes.Add(dimensionAttribute);
                if (lvDimensionAttribute.RowCount != 0)
                {
                    int latestAttributeSequence = dimensionAttributes[(RecordIdentifier) lvDimensionAttribute.Row(lvDimensionAttribute.RowCount - 1).Tag].Sequence;
                    dimensionAttribute.Sequence = latestAttributeSequence + 10;
                }
                else
                {
                    dimensionAttribute.Sequence = 10;
                }
                lvDimensionAttribute.AddRow(row);
                dimensionAttributes.Add(dimensionAttribute.ID, dimensionAttribute);
                lvDimensionAttribute.AutoSizeColumns();
                lvDimensionAttribute.Selection.Set(lvDimensionAttribute.RowCount - 1);
                lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private List<RetailItemDimension> GetDimensions()
        {
            List<RetailItemDimension> dimensions = new List<RetailItemDimension>();
            foreach (var row in lvDimensions.Rows)
            {
                dimensions.Add(((ItemDimensions)retailItemDimensionAttributes[(RecordIdentifier) row.Tag]).Dimension);
            }

            return dimensions;
        }

        private void lvDimensionAttribute_SelectionChanged(object sender, EventArgs e)
        {
            if (lvDimensionAttribute.Selection.FirstSelectedRow < 0)
            {
                return;
            }
            RecordIdentifier currentAttribute = (RecordIdentifier)lvDimensionAttribute.Row(lvDimensionAttribute.Selection.FirstSelectedRow).Tag;
            if (currentAttribute != selectedAttributeID)
            {
                selectedAttributeID = currentAttribute;
            }
            btnsEditAddRemoveDimensionAttribute.EditButtonEnabled = (lvDimensionAttribute.Selection.Count == 1);
            btnsEditAddRemoveDimensionAttribute.RemoveButtonEnabled = (lvDimensionAttribute.Selection.Count == 1) && (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit));
            btnMoveUpDimensionAttribute.Enabled = (lvDimensionAttribute.Selection.Count == 1) && (lvDimensionAttribute.RowCount > 1) && (lvDimensionAttribute.Selection.FirstSelectedRow != 0);
            btnMoveDownDimensionAttribute.Enabled = (lvDimensionAttribute.Selection.Count == 1) && (lvDimensionAttribute.RowCount > 1) && (lvDimensionAttribute.Selection.FirstSelectedRow != lvDimensionAttribute.RowCount - 1);
            
            CheckEnabled();
        }

        private void lvDimensions_SelectionChanged(object sender, EventArgs e)
        {
            if (lvDimensions.Selection.Count == 0)
            {
                return;
            }
            RecordIdentifier currentDimension = (RecordIdentifier) lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag;
            if (currentDimension != selectedDimensionID)
            {
                selectedDimensionID = currentDimension;
                LoadAttributeListView();
            }
            btnsEditAddRemoveDimension.EditButtonEnabled = (lvDimensions.Selection.Count == 1);
            btnsEditAddRemoveDimension.RemoveButtonEnabled = (lvDimensions.Selection.Count == 1);
            btnMoveUpDimension.Enabled = (lvDimensions.Selection.Count == 1) && (lvDimensions.RowCount > 1) && (lvDimensions.Selection.FirstSelectedRow != 0);
            btnMoveDownDimension.Enabled = (lvDimensions.Selection.Count == 1) && (lvDimensions.RowCount > 1) && (lvDimensions.Selection.FirstSelectedRow != lvDimensions.RowCount - 1);
            btnsEditAddRemoveDimensionAttribute.AddButtonEnabled = (lvDimensions.Selection.Count == 1);
            btnMoveUpDimensionAttribute.Enabled = false;
            btnMoveDownDimensionAttribute.Enabled = false;
            btnsEditAddRemoveDimensionAttribute.EditButtonEnabled = false;
            btnsEditAddRemoveDimensionAttribute.RemoveButtonEnabled = false;
            CheckEnabled();
        }

        private void btnMoveUpDimension_Click(object sender, EventArgs e)
        {
            ItemDimensions value = (ItemDimensions)retailItemDimensionAttributes[(RecordIdentifier)lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag];
            retailItemDimensionAttributes.RemoveAt(lvDimensions.Selection.FirstSelectedRow);
            retailItemDimensionAttributes.Insert(lvDimensions.Selection.FirstSelectedRow - 1, (RecordIdentifier)lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag, value);
            lvDimensions.SwapRows(lvDimensions.Selection.FirstSelectedRow, lvDimensions.Selection.FirstSelectedRow - 1);
            lvDimensions_SelectionChanged(this, EventArgs.Empty);
        }

        private void btnMoveDownDimension_Click(object sender, EventArgs e)
        {          
            ItemDimensions value = (ItemDimensions)retailItemDimensionAttributes[(RecordIdentifier)lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag];
            retailItemDimensionAttributes.RemoveAt(lvDimensions.Selection.FirstSelectedRow);
            retailItemDimensionAttributes.Insert(lvDimensions.Selection.FirstSelectedRow + 1, (RecordIdentifier)lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag, value);
            lvDimensions.SwapRows(lvDimensions.Selection.FirstSelectedRow, lvDimensions.Selection.FirstSelectedRow + 1);
            lvDimensions_SelectionChanged(this, EventArgs.Empty);
            if (!btnMoveDownDimension.Enabled)
            {
                btnMoveUpDimension.Focus();
            }
        }

        private void btnMoveUpDimensionAttribute_Click(object sender, EventArgs e)
        {
            DimensionAttribute tempAttribute = ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes[lvDimensionAttribute.Selection.FirstSelectedRow];
            ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes.RemoveAt(lvDimensionAttribute.Selection.FirstSelectedRow);
            ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes.Insert(lvDimensionAttribute.Selection.FirstSelectedRow - 1, tempAttribute);

            lvDimensionAttribute.SwapRows(lvDimensionAttribute.Selection.FirstSelectedRow, lvDimensionAttribute.Selection.FirstSelectedRow - 1);
            
            lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
        }

        private void btnMoveDownDimensionAttribute_Click(object sender, EventArgs e)
        {
            DimensionAttribute tempAttribute = ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes[lvDimensionAttribute.Selection.FirstSelectedRow];
            ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes.RemoveAt(lvDimensionAttribute.Selection.FirstSelectedRow);
            ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes.Insert(lvDimensionAttribute.Selection.FirstSelectedRow + 1, tempAttribute);

            lvDimensionAttribute.SwapRows(lvDimensionAttribute.Selection.FirstSelectedRow, lvDimensionAttribute.Selection.FirstSelectedRow + 1);
            lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
            if (!btnMoveDownDimensionAttribute.Enabled)
            {
                btnMoveUpDimensionAttribute.Focus();
            }
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = ValidateFields();
        }

        private bool ValidateFields()
        {
            foreach (var row in lvDimensions.Rows)
            {
                List<DimensionAttribute> attributes = ((ItemDimensions)retailItemDimensionAttributes[(RecordIdentifier)row.Tag]).Attributes;

                if (attributes.Count == 0)
                {
                    return false;
                }
            }

            if (lvDimensions.RowCount == 0)
            {
                return false;
            }
            return true;
        }

        private void btnsEditAddRemoveDimension_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.DeleteDimensionQuestion, Resources.DeleteDimension) == DialogResult.Yes)
            {
                int selectedRow = lvDimensions.Selection.FirstSelectedRow;
                retailItemDimensionAttributes.Remove((RecordIdentifier) lvDimensions.Row(selectedRow).Tag);
                lvDimensions.RemoveRow(selectedRow);

                if (selectedRow < lvDimensions.RowCount)
                {
                    lvDimensions.Selection.Set(selectedRow);
                }
                else if (lvDimensions.RowCount > 0)
                {
                    lvDimensions.Selection.Set(lvDimensions.RowCount - 1);
                }
                CheckEnabled();
            }
            
        }

        private void btnsEditAddRemoveDimensionAttribute_RemoveButtonClicked(object sender, EventArgs e)
        {
            bool deleteAttribute = false;

            if (QuestionDialog.Show(Resources.DeleteAttributeQuestion, Resources.DeleteAttribute) == DialogResult.Yes)
            {
                deleteAttribute = true;
            }

            if (deleteAttribute)
            {
                int selectedRow = lvDimensionAttribute.Selection.FirstSelectedRow;
                lvDimensionAttribute.RemoveRow(selectedRow);

                if (selectedRow < lvDimensionAttribute.RowCount)
                {
                    lvDimensionAttribute.Selection.Set(selectedRow);
                }
                else if (lvDimensionAttribute.RowCount > 0)
                {
                    lvDimensionAttribute.Selection.Set(lvDimensionAttribute.RowCount - 1);
                }
                    ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes.RemoveAt(selectedRow);
                CheckEnabled();
            }
        }

        private void btnsEditAddRemoveDimension_EditButtonClicked(object sender, EventArgs e)
        {
            RetailItemDimension description = ((ItemDimensions)retailItemDimensionAttributes[(RecordIdentifier) lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag]).Dimension;
            List<RetailItemDimension> retailItemDimensions = GetDimensions();

            Dialogs.EditDimensionDialog dlg = new Dialogs.EditDimensionDialog(description, retailItemDimensions);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow)[0].Text = description.Text;
                lvDimensions.Invalidate();
                lvDimensions.AutoSizeColumns();
                lvDimensionAttribute.AutoSizeColumns();
            }
        }

        private void btnsEditAddRemoveDimensionAttribute_EditButtonClicked(object sender, EventArgs e)
        {
            DimensionAttribute attribute = (((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes)[lvDimensionAttribute.Selection.FirstSelectedRow];
            List<DimensionAttribute> dimensionAttributes = ((ItemDimensions)retailItemDimensionAttributes[selectedDimensionID]).Attributes;

            Dialogs.AddNewDimensionAttributeDialog dlg = new Dialogs.AddNewDimensionAttributeDialog(attribute, dimensionAttributes);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lvDimensionAttribute.Row(lvDimensionAttribute.Selection.FirstSelectedRow)[0].Text = attribute.Text;
                lvDimensionAttribute.Row(lvDimensionAttribute.Selection.FirstSelectedRow)[1].Text = attribute.Code;
                lvDimensionAttribute.Invalidate();
                lvDimensionAttribute.AutoSizeColumns();
            }
        }

        private void DimensionsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvDimensions.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnsEditAddRemoveDimension_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemoveDimension.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.AddDimension,
                    200,
                    btnsEditAddRemoveDimension_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsEditAddRemoveDimension.AddButtonEnabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.RemoveDimension,
                    300,
                    btnsEditAddRemoveDimension_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemoveDimension.RemoveButtonEnabled
            };

            menu.Items.Add(item);
        }

        private void AttributesContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvDimensionAttribute.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnsEditAddRemoveDimensionAttribute_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemoveDimensionAttribute.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.AddAttribute,
                    200,
                    btnsEditAddRemoveDimensionAttribute_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsEditAddRemoveDimensionAttribute.AddButtonEnabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.RemoveAttribute,
                    300,
                    btnsEditAddRemoveDimensionAttribute_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemoveDimensionAttribute.RemoveButtonEnabled
            };

            menu.Items.Add(item);
        }

        private void lvDimensionAttribute_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemoveDimensionAttribute.EditButtonEnabled)
            {
                btnsEditAddRemoveDimensionAttribute_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvDimensions_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemoveDimension.EditButtonEnabled)
            {
                btnsEditAddRemoveDimension_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
