using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.VariantFramework.Dialogs;
using LSOne.ViewPlugins.VariantFramework.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.VariantFramework.DialogPages
{
    public partial class NewRetailItemDimensionsPage : UserControl, IDialogTabViewWithRequiredFields, IMessageTabExtension
    {

        public event EventHandler RequiredInputValidate;
        private RetailItem retailItem;

        public NewRetailItemDimensionsPage()
        {
            InitializeComponent();

            lvDimensions.ContextMenuStrip = new ContextMenuStrip();
            lvDimensionAttribute.ContextMenuStrip = new ContextMenuStrip();

            lvDimensions.ContextMenuStrip.Opening += DimensionsContextMenuStrip_Opening;
            lvDimensionAttribute.ContextMenuStrip.Opening += AttributesContextMenuStrip_Opening;

            lvDimensions.AutoSizeColumns();
            lvDimensionAttribute.AutoSizeColumns();

            btnsEditAddRemoveDimensionAttribute.AddButtonEnabled = false;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new NewRetailItemDimensionsPage();
        }


        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            retailItem = (RetailItem)internalContext;
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
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        public void SaveSecondaryRecords()
        {

            int dimensionSequense = 10;
            int attributeSequense;
            RetailItemDimension dimension;
            List<DimensionAttribute> attributes;

            foreach (var row in lvDimensions.Rows)
            {
                dimension = new RetailItemDimension();
                dimension.RetailItemMasterID = retailItem.MasterID;
                dimension.Text = row[0].Text;
                dimension.Sequence = dimensionSequense;

                Providers.RetailItemDimensionData.Save(PluginEntry.DataModel, dimension);

                dimensionSequense += 10;
                attributeSequense = 10;

                attributes = ((List<DimensionAttribute>)((object[])row.Tag)[1]);

                foreach (var attribute in attributes)
                {
                    attribute.DimensionID = dimension.ID;
                    attribute.Sequence = attributeSequense;

                    Providers.DimensionAttributeData.Save(PluginEntry.DataModel, attribute);

                    attributeSequense += 10;
                }
            }
        }

        void IDialogTabViewWithRequiredFields.RequiredFieldsAreValid(FieldValidationArguments args)
        {
            foreach (var row in lvDimensions.Rows)
            {
                List<DimensionAttribute> attributes = ((List<DimensionAttribute>)((object[])row.Tag)[1]);

                if (attributes.Count == 0)
                {

                    args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
                    args.ResultDescription = string.Format(Resources.DimensionAttributeMissing, args.TabName, row[0].Text);
                    return;
                }
            }

            if (lvDimensions.RowCount == 0 && retailItem.ItemType != DataLayer.BusinessObjects.Enums.ItemTypeEnum.MasterItem)
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.Valid;
            }
            else if (lvDimensions.RowCount == 0 && retailItem.ItemType == DataLayer.BusinessObjects.Enums.ItemTypeEnum.MasterItem)
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
                args.ResultDescription = Resources.NoDimensionsForVariantItem;
            }
            else if (retailItem.ItemType == DataLayer.BusinessObjects.Enums.ItemTypeEnum.Service && lvDimensions.RowCount > 0)
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
                args.ResultDescription = Resources.NoProperItemType;
            }
            else
            {
                bool handled;
                ((TabControl)this.Parent).SendViewPageMessage(this, "HasVariantsPage", null, out handled);
                if (handled)
                {
                args.Result = FieldValidationArguments.FieldValidationEnum.Valid;
                }
                else
                {
                    args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
                    args.ResultDescription = Resources.NoVariantItemsSelected;
                }
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

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    Resources.MoveUp,
                    500,
                    btnMoveUpDimension_Click)
            {
                Image = ContextButtons.GetMoveUpButtonImage(),
                Enabled = btnMoveUpDimension.Enabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.MoveDown,
                    600,
                    btnMoveDownDimension_Click)
            {
                Image = ContextButtons.GetMoveDownButtonImage(),
                Enabled = btnMoveDownDimension.Enabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("NewRetailItemDimensionsList", lvDimensions.ContextMenuStrip, this);

            e.Cancel = (menu.Items.Count == 0);
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

            menu.Items.Add(new ExtendedMenuItem("-", 400));

            item = new ExtendedMenuItem(
                    Resources.MoveUp,
                    500,
                    btnMoveUpDimensionAttribute_Click)
            {
                Image = ContextButtons.GetMoveUpButtonImage(),
                Enabled = btnMoveUpDimensionAttribute.Enabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.MoveDown,
                    600,
                    btnMoveDownDimensionAttribute_Click)
            {
                Image = ContextButtons.GetMoveDownButtonImage(),
                Enabled = btnMoveDownDimensionAttribute.Enabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("NewRetailItemDimensionsAttributeList", lvDimensionAttribute.ContextMenuStrip, this);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnsEditAddRemoveDimension_EditButtonClicked(object sender, EventArgs e)
        {
            RetailItemDimension description = (RetailItemDimension) ((object[]) lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag)[0];
            List<RetailItemDimension> retailItemDimensions = GetDimensions();

            Dialogs.EditDimensionDialog dlg = new Dialogs.EditDimensionDialog(description, retailItemDimensions);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow)[0].Text = description.Text;
                lvDimensions.Invalidate();
                lvDimensions.AutoSizeColumns();
                lvDimensionAttribute.AutoSizeColumns();
                UpdatePreview();
                DimensionDataChanged();
            }
        }

        private void btnsEditAddRemoveDimensionAttribute_EditButtonClicked(object sender, EventArgs e)
        {
            DimensionAttribute attribute = (DimensionAttribute) lvDimensionAttribute.Row(lvDimensionAttribute.Selection.FirstSelectedRow).Tag;
            List<DimensionAttribute> dimensionAttributes = ((List<DimensionAttribute>)((object[])lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag)[1]);

            Dialogs.AddNewDimensionAttributeDialog dlg = new Dialogs.AddNewDimensionAttributeDialog(attribute, dimensionAttributes);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lvDimensionAttribute.Row(lvDimensionAttribute.Selection.FirstSelectedRow)[0].Text = attribute.Text;
                lvDimensionAttribute.Row(lvDimensionAttribute.Selection.FirstSelectedRow)[1].Text = attribute.Code;
                lvDimensionAttribute.Invalidate();
                lvDimensionAttribute.AutoSizeColumns();
                UpdatePreview();
                DimensionDataChanged();
            }
        }

        private void btnsEditAddRemoveDimension_AddButtonClicked(object sender, EventArgs e)
        {
            List<RetailItemDimension> retailItemDimensions = GetDimensions();
            Dialogs.AddNewDimensionDialog dlg = new AddNewDimensionDialog(retailItemDimensions);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                RetailItemDimension retailItemDimension = dlg.RetailItemDimension;
                if (retailItemDimension != null)
                {
                    Row row = new Row();
                    row.AddText(retailItemDimension.Text);
                    List<DimensionAttribute> attributes = new List<DimensionAttribute>();
                    row.Tag = new object[] {retailItemDimension, attributes};

                    foreach (var attribute in dlg.DimensionAttributes)
                    {
                        attributes.Add(attribute);
                    }

                    lvDimensions.AddRow(row);
                    lvDimensions.AutoSizeColumns();
                    lvDimensions.Selection.Set(lvDimensions.RowCount - 1);
                    lvDimensions_SelectionChanged(this, EventArgs.Empty);
                }
                UpdatePreview();
                ValidateInput();
                DimensionDataChanged();
            }
            
        }

        private void btnsEditAddRemoveDimensionAttribute_AddButtonClicked(object sender, EventArgs e)
        {
            List<DimensionAttribute> dimensionAttributes = ((List<DimensionAttribute>)((object[])lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag)[1]);

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
                row.Tag = dimensionAttribute;
                ((List<DimensionAttribute>)((object[])lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag)[1]).Add(dimensionAttribute);
                lvDimensionAttribute.AddRow(row);
                lvDimensionAttribute.AutoSizeColumns();
                lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
            }
            UpdatePreview();
            ValidateInput();
            DimensionDataChanged();
        }

        private void btnsEditAddRemoveDimension_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.DeleteDimensionQuestion, Resources.DeleteDimension) == DialogResult.Yes)
            {
                int selectedRow = lvDimensions.Selection.FirstSelectedRow;
                lvDimensions.RemoveRow(selectedRow);

                if (selectedRow < lvDimensions.RowCount)
                {
                    lvDimensions.Selection.Set(selectedRow);
                }
                else if (lvDimensions.RowCount > 0)
                {
                    lvDimensions.Selection.Set(lvDimensions.RowCount - 1);
                }

                UpdatePreview();
                ValidateInput();
                DimensionDataChanged();
            }
        }

        private void btnsEditAddRemoveDimensionAttribute_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(Resources.DeleteAttributeQuestion, Resources.DeleteAttribute) == DialogResult.Yes)
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
                ((List<DimensionAttribute>) ((object[]) lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag)[1]).RemoveAt(selectedRow);
                UpdatePreview();
                ValidateInput();
                DimensionDataChanged();
            }
        }

        private void lvDimensions_SelectionChanged(object sender, EventArgs e)
        {
            btnsEditAddRemoveDimension.EditButtonEnabled = (lvDimensions.Selection.Count == 1);
            btnsEditAddRemoveDimension.RemoveButtonEnabled = (lvDimensions.Selection.Count == 1);
            btnMoveUpDimension.Enabled = (lvDimensions.Selection.Count == 1) && (lvDimensions.RowCount > 1) && (lvDimensions.Selection.FirstSelectedRow != 0);
            btnMoveDownDimension.Enabled = (lvDimensions.Selection.Count == 1) && (lvDimensions.RowCount > 1) && (lvDimensions.Selection.FirstSelectedRow != lvDimensions.RowCount - 1);
            btnsEditAddRemoveDimensionAttribute.AddButtonEnabled = (lvDimensions.Selection.Count == 1);
            lvDimensionAttribute.ClearRows();
            if (lvDimensions.Selection.Count == 1)
            {
                List<DimensionAttribute> attributes = (List<DimensionAttribute>)((object[])lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag)[1];
                foreach (var attribute in attributes)
                {
                    Row row = new Row();
                    row.AddText(attribute.Text);
                    row.AddText(attribute.Code);
                    row.Tag = attribute;
                    lvDimensionAttribute.AddRow(row);
                }
                lvDimensionAttribute.AutoSizeColumns();
                lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void lvDimensionAttribute_SelectionChanged(object sender, EventArgs e)
        {
            btnsEditAddRemoveDimensionAttribute.EditButtonEnabled = (lvDimensionAttribute.Selection.Count == 1);
            btnsEditAddRemoveDimensionAttribute.RemoveButtonEnabled = (lvDimensionAttribute.Selection.Count == 1);
            btnMoveUpDimensionAttribute.Enabled = (lvDimensionAttribute.Selection.Count == 1) && (lvDimensionAttribute.RowCount > 1) && (lvDimensionAttribute.Selection.FirstSelectedRow != 0);
            btnMoveDownDimensionAttribute.Enabled = (lvDimensionAttribute.Selection.Count == 1) && (lvDimensionAttribute.RowCount > 1) && (lvDimensionAttribute.Selection.FirstSelectedRow != lvDimensionAttribute.RowCount - 1);
        }

        private void btnMoveUpDimension_Click(object sender, EventArgs e)
        {
            lvDimensions.SwapRows(lvDimensions.Selection.FirstSelectedRow, lvDimensions.Selection.FirstSelectedRow - 1);
            lvDimensions_SelectionChanged(this, EventArgs.Empty);
            UpdatePreview();
            DimensionDataChanged();
        }

        private void btnMoveDownDimension_Click(object sender, EventArgs e)
        {
            lvDimensions.SwapRows(lvDimensions.Selection.FirstSelectedRow, lvDimensions.Selection.FirstSelectedRow + 1);
            lvDimensions_SelectionChanged(this, EventArgs.Empty);
            if (!btnMoveDownDimension.Enabled)
            {
                btnMoveUpDimension.Focus();
            }
            UpdatePreview();
            DimensionDataChanged();
        }

        private void btnMoveUpDimensionAttribute_Click(object sender, EventArgs e)
        {
            List<DimensionAttribute> attributes = ((List<DimensionAttribute>)((object[])lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag)[1]);
            DimensionAttribute tempAttribute = attributes[lvDimensionAttribute.Selection.FirstSelectedRow];
            attributes.RemoveAt(lvDimensionAttribute.Selection.FirstSelectedRow);
            attributes.Insert(lvDimensionAttribute.Selection.FirstSelectedRow - 1, tempAttribute);

            lvDimensionAttribute.SwapRows(lvDimensionAttribute.Selection.FirstSelectedRow, lvDimensionAttribute.Selection.FirstSelectedRow - 1);
            lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);

            UpdatePreview();
            DimensionDataChanged();
        }

        private void btnMoveDownDimensionAttribute_Click(object sender, EventArgs e)
        {
            List<DimensionAttribute> attributes = ((List<DimensionAttribute>)((object[])lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag)[1]);
            DimensionAttribute tempAttribute = attributes[lvDimensionAttribute.Selection.FirstSelectedRow];
            attributes.RemoveAt(lvDimensionAttribute.Selection.FirstSelectedRow);
            attributes.Insert(lvDimensionAttribute.Selection.FirstSelectedRow + 1, tempAttribute);

            lvDimensionAttribute.SwapRows(lvDimensionAttribute.Selection.FirstSelectedRow, lvDimensionAttribute.Selection.FirstSelectedRow + 1);
            lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
            if (!btnMoveDownDimensionAttribute.Enabled)
            {
                btnMoveUpDimensionAttribute.Focus();
            }
            UpdatePreview();
            DimensionDataChanged();
        }

        private void DimensionDataChanged()
        {
            bool handled;
            ((TabControl) this.Parent).SendViewPageMessage(this, "DimensionsChanged", null, out handled);
        }

        private void UpdatePreview()
        {
            string descriptionPreview = "";

            foreach (var row in lvDimensions.Rows)
            {
                List<DimensionAttribute> attributes = ((List<DimensionAttribute>)((object[])row.Tag)[1]);

                if (attributes.Count > 0)
                {
                    if (descriptionPreview != "")
                    {
                        descriptionPreview += " ";
                    }
                    descriptionPreview += attributes[0].Text;
                }
            }

            if (descriptionPreview.Length > 60)
            {
                descriptionPreview = descriptionPreview.Left(60);
            }

            lblDescriptionPreview.Text = descriptionPreview;
        }

        private void ValidateInput()
        {
            // Here we somehow want to trigger checkenabled on the parent
            if (RequiredInputValidate != null)
            {
                RequiredInputValidate(this, EventArgs.Empty);
            }
        }

        private List<RetailItemDimension> GetDimensions()
        {
            List<RetailItemDimension> dimensions = new List<RetailItemDimension>();
            foreach (var row in lvDimensions.Rows)
            {
                dimensions.Add((RetailItemDimension)((object[])row.Tag)[0]);
            }
            
            return dimensions;
        }

        public object OnViewPageMessage(object sender, string message, object param, ref bool handled)
        {
            switch (message)
            {
                case "GetDimensions":
                    List<Row> dimensions = new List<Row>();
                    foreach (var row in lvDimensions.Rows)
                    {
                        dimensions.Add(row);
                    }
                    handled = true;
                    return dimensions;

                case "CreateAnother":
                    lvDimensions.ClearRows();
                    lvDimensionAttribute.ClearRows();
                    lblDescriptionPreview.Text = "";
                    return null;
            }
            return null;
        }

        private void lvDimensions_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemoveDimension.EditButtonEnabled)
            {
                btnsEditAddRemoveDimension_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvDimensionAttribute_DoubleClick(object sender, EventArgs e)
        {
            if (btnsEditAddRemoveDimensionAttribute.EditButtonEnabled)
            {
                btnsEditAddRemoveDimensionAttribute_EditButtonClicked(this, EventArgs.Empty);
            }
        }
    }
}
