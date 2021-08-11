using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.ColorPicker;
using LSOne.Controls.Columns;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.Controls.Themes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.UserInterface;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using LSOne.Utilities.IO;
using LSOne.Utilities.IO.JSON;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.UserInterfaceStyles.CellExtensions;
using LSOne.ViewPlugins.UserInterfaceStyles.Dialogs;

namespace LSOne.ViewPlugins.UserInterfaceStyles.Controls
{
    public partial class ContextStyleControl : UserControl
    {
        private UIStyle uiStyle;
        private bool hasErrors;
        private bool isNew;
        private ContextStyleDescriptor contextDescription;
        private int valueCount;

        public event EventHandler ValueChanged;

        public ContextStyleControl()
        {
            InitializeComponent();

            lvAttributes.ApplyTheme(new LSOneTheme());
            lvAttributes.HorizontalScrollbar = false;

            DoubleBuffered = true;
            hasErrors = false;

            lvAttributes.ContextMenuStrip = new ContextMenuStrip();
            lvAttributes.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvAttributes.ContextMenuStrip;
            menu.Items.Clear();

            // Each item is a line in the right click menu. 
            // Usually there is not much that needs to be changed here

            var item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   (EventHandler)null);

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            item.DropDownItems.AddRange(GetToolStripItemsForAdd(new EventHandler(ContextMenu_ItemClick)));
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

           

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        public void SetValues(UIStyle uiStyle, ContextStyleDescriptor contextDescription, bool isNew, string parentStyleDescription)
        {
            Row row;

            this.contextDescription = contextDescription;
            this.uiStyle = uiStyle;

            lvAttributes.ClearRows();

            this.isNew = isNew;

            tbParentStyle.Text = parentStyleDescription;

            if (uiStyle.ParentStyleID != RecordIdentifier.Empty && uiStyle.ParentStyleID != null)
            {
                btnEditParent.Visible = true;
            }

            ContextStyleAttribute[] values = (ContextStyleAttribute[])Enum.GetValues(typeof(ContextStyleAttribute));

            // Set up mandatory pieces
            if (uiStyle.ParentStyleID == RecordIdentifier.Empty)
            {
                foreach (ContextStyleAttribute value in values)
                {
                    if ((contextDescription.Mandatory & value) == value && value != ContextStyleAttribute.None)
                    {
                        row = new Row();

                        row.AddCell(new IconButtonCell(new IconButton(Properties.Resources.LockDarkGray16, Properties.Resources.RequiredProperty, true, false), IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter));
                        row.AddText(contextDescription.GetAttributeName(value));
                        row.AddCell(CreatePrimaryCell(value));

                        if (value == ContextStyleAttribute.Texture)
                        {
                            row.Height = 105;
                        }

                        row.Tag = value;

                        lvAttributes.AddRow(row);
                    }
                }
            }
            else
            {
                foreach (ContextStyleAttribute value in values)
                {
                    if ((contextDescription.Mandatory & value) == value && value != ContextStyleAttribute.None)
                    {
                        if (AttributeIsSet(value))
                        {
                            AddOptionalRow(value);
                        }
                    }
                }
            }


            // Optional pieces that might be set
            // Set up mandatory pieces
            foreach (ContextStyleAttribute value in values)
            {
                if ((contextDescription.Optional & value) == value && value != ContextStyleAttribute.None)
                {
                    if (AttributeIsSet(value))
                    {
                        AddOptionalRow(value);
                    }
                }
            }

            CheckColumnSizes();

            valueCount = lvAttributes.RowCount;

            OnValueChanged();

            CheckEnabled();

            
        }

        private void CheckColumnSizes()
        {
            lvAttributes.AutoSizeColumns();

            int colWidth = 0;

            foreach (Column column in lvAttributes.Columns)
            {
                colWidth += column.Width;
            }

            lvAttributes.Columns[2].Width += (short)((short)lvAttributes.Width - (short)colWidth);
        }

        private void AddOptionalRow(ContextStyleAttribute value)
        {
            Row row = new Row();

            row.AddText("");
            row.AddText(contextDescription.GetAttributeName(value));
            row.AddCell(CreatePrimaryCell(value));

            if (value == ContextStyleAttribute.Texture)
            {
                row.Height = 105;
            }

            row.Tag = value;

            lvAttributes.AddRow(row);
        }

        private void CheckEnabled()
        {
            int unsetOptionalCount = 0;

            ContextStyleAttribute[] values = (ContextStyleAttribute[])Enum.GetValues(typeof(ContextStyleAttribute));

            if (uiStyle.ParentStyleID == RecordIdentifier.Empty)
            {
                foreach (ContextStyleAttribute value in values)
                {
                    if ((contextDescription.Optional & value) == value && value != ContextStyleAttribute.None)
                    {
                        if (!AttributeIsSet(value))
                        {
                            unsetOptionalCount++;
                        }
                    }
                }
            }
            else
            {
                foreach (ContextStyleAttribute value in values)
                {
                    if (value != ContextStyleAttribute.None)
                    {
                        if (!AttributeIsSet(value))
                        {
                            unsetOptionalCount++;
                        }
                    }
                }
            }

            btnsEditAddRemove.AddButtonEnabled = unsetOptionalCount > 0;

            if (lvAttributes.Selection.Count == 1)
            {
                if (lvAttributes.Selection[0][0] is IconButtonCell)
                {
                    btnsEditAddRemove.RemoveButtonEnabled = false;
                }
                else
                {
                    btnsEditAddRemove.RemoveButtonEnabled = true;
                }
            }
            else
            {
                btnsEditAddRemove.RemoveButtonEnabled = false;
            }
        }

       

        private Cell CreatePrimaryCell(ContextStyleAttribute attribute)
        {
            switch (attribute)
            {
                case ContextStyleAttribute.BackColor:
                    return new ColorBoxCell(4, uiStyle.Style.BackColor, Color.Gray, true) { MaxWidth = 30 };

                case ContextStyleAttribute.BackColor3:
                    return new ColorBoxCell(4, uiStyle.Style.BackColor3, Color.Gray, true) { MaxWidth = 30 };

                case ContextStyleAttribute.ForeColor:

                    return new ColorBoxCell(4, uiStyle.Style.ForeColor, Color.Gray, true) { MaxWidth = 30 };

                case ContextStyleAttribute.ForeColor2:
                    return new ColorBoxCell(4, uiStyle.Style.ForeColor2, Color.Gray, true) { MaxWidth = 30 };

                    
                case ContextStyleAttribute.BorderColor:
                    return new ColorBoxCell(4, uiStyle.Style.BorderColor, Color.Gray, true) { MaxWidth = 30 };

                case ContextStyleAttribute.BorderColor2:
                    return new ColorBoxCell(4, uiStyle.Style.BorderColor2, Color.Gray, true) { MaxWidth = 30 };
 
                case ContextStyleAttribute.OverlayColor:
                   return new ColorBoxCell(4, uiStyle.Style.OverlayColor, Color.Gray, true) { MaxWidth = 30 };

                case ContextStyleAttribute.LineTokenColor:
                   return new ColorBoxCell(4, uiStyle.Style.LineTokenColor, Color.Gray, true) { MaxWidth = 30 };

                case ContextStyleAttribute.Font:
                    return new FontCell(uiStyle.Style.Font);

                case ContextStyleAttribute.FontStyleOverride:
                    return new FontStyleCell((uiStyle.Style.FontStyle == null) ? FontStyle.Regular : (FontStyle)uiStyle.Style.FontStyle);

                case ContextStyleAttribute.RowHeight:
                    return new EditableNumericCell(uiStyle.Style.Height == null ? 24 : (int)uiStyle.Style.Height, new DecimalLimit(0, 0));


                case ContextStyleAttribute.Texture:
                    return new PictureCell(uiStyle.Style.BackgroundTexture);

                case ContextStyleAttribute.LineToken:
                    return new TokenCell(uiStyle.Style.LineToken != null ? (BaseStyle.LineTokenEnum)uiStyle.Style.LineToken : BaseStyle.LineTokenEnum.NoToken);
            }

            return null;
        }

        private bool AttributeIsSet(ContextStyleAttribute attribute)
        {
            switch(attribute)
            {
                case ContextStyleAttribute.BackColor:
                    return !uiStyle.Style.BackColor.IsEmpty;

                case ContextStyleAttribute.BackColor3:
                    return !uiStyle.Style.BackColor3.IsEmpty;

                case ContextStyleAttribute.ForeColor:
                    return !uiStyle.Style.ForeColor.IsEmpty;

                case ContextStyleAttribute.ForeColor2:
                    return !uiStyle.Style.ForeColor2.IsEmpty;

                case ContextStyleAttribute.BorderColor:
                    return !uiStyle.Style.BorderColor.IsEmpty;

                case ContextStyleAttribute.BorderColor2:
                    return !uiStyle.Style.BorderColor2.IsEmpty;

                case ContextStyleAttribute.OverlayColor:
                    return !uiStyle.Style.OverlayColor.IsEmpty;

                case ContextStyleAttribute.LineTokenColor:
                    return !uiStyle.Style.LineTokenColor.IsEmpty;

                case ContextStyleAttribute.Font:
                    return uiStyle.Style.Font != null;

                case ContextStyleAttribute.FontStyleOverride:
                    return uiStyle.Style.FontStyle != null;

                case ContextStyleAttribute.RowHeight:
                    return uiStyle.Style.Height != null;

                case ContextStyleAttribute.Texture:
                    return uiStyle.Style.BackgroundTexture != null;

                case ContextStyleAttribute.LineToken:
                    return uiStyle.Style.LineToken != null;
            }

            return false;
        }

        private void lvAttributes_CellAction(object sender, CellEventArgs args)
        {
            if (hasErrors)
            {
                ClearErrors();
            }

            if (args.Cell is ColorBoxCell)
            {
                ContextStyleAttribute currentAttribute = (ContextStyleAttribute)lvAttributes.Row(args.RowNumber).Tag;

                ColorPickerDialog dlg = new ColorPickerDialog(currentAttribute == ContextStyleAttribute.OverlayColor, true);

                if (((ColorBoxCell)args.Cell).BoxColor != Color.Empty && ((ColorBoxCell)args.Cell).BoxColor != Color.Transparent)
                {
                    dlg.SelectedColor = ((ColorBoxCell)args.Cell).BoxColor;
                }
                else
                {
                    dlg.SelectedColor = ColorPalette.White;
                }

                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    ((ColorBoxCell)args.Cell).BoxColor = dlg.SelectedColor;

                    lvAttributes.Invalidate();

                    OnValueChanged();
                } 
            }
            else if (args.Cell is FontCell)
            {
                FontDialog dlg = new FontDialog();

                if (((FontCell)args.Cell).Font != null)
                {
                    dlg.Font = ((FontCell)args.Cell).Font;
                }
                
                dlg.ShowEffects = true;

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    ((FontCell)args.Cell).Font = dlg.Font;

                    lvAttributes.Invalidate();

                    OnValueChanged();
                }

            }
            else if (args.Cell is FontStyleCell)
            {
                FontStyleDialog dlg = new FontStyleDialog(((FontStyleCell)args.Cell).FontStyle);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    ((FontStyleCell)args.Cell).FontStyle = dlg.FontStyle;

                    lvAttributes.Invalidate();

                    OnValueChanged();
                }
            }
            else if (args.Cell is EditableNumericCell || args.Cell is TokenCell)
            {
                OnValueChanged();
            }
            else if (args.Cell is PictureCell)
            {
                //Ask user to select file.
                OpenFileDialog dlg = new OpenFileDialog();

                dlg.Filter =
                    Properties.Resources.ImageFiles + " (*.bmp;*.jpg;*.png)|*.bmp;*.jpg;*.png|" +
                    Properties.Resources.PNGfiles + " (*.png)|*.png|" +
                    Properties.Resources.JPEGfiles + " (*.jpg)|*.jpg|" +
                    Properties.Resources.BMPfiles + "  (*.bmp)|*.bmp";

                DialogResult dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);
                if (dlgRes != DialogResult.Cancel)
                {
                    ((PictureCell)args.Cell).Image = BitmapHelper.GetBitmapFromFile(FolderItem.FromPath(dlg.FileName));

                    lvAttributes.Invalidate();

                    OnValueChanged();
                }
            }

            

        }

        private void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }

        private void ClearErrors()
        {
            hasErrors = false;

            foreach (var row in lvAttributes.Rows)
            {
                if(row[3] != null)
                {
                    row[3] = null;
                    lvAttributes.Invalidate();
                }
            }
        }

        public bool ValidateSave()
        {
            hasErrors = false;

            foreach (var row in lvAttributes.Rows)
            {
                ContextStyleAttribute currentAttribute = (ContextStyleAttribute)row.Tag;

                if (row[2] is ColorBoxCell)
                {
                    if (((ColorBoxCell)row[2]).BoxColor == Color.Empty)
                    {
                        hasErrors = true;

                        row[3] = new IconButtonCell(new IconButton(Properties.Resources.Error16, Properties.Resources.ColorValueMustBeSet,true,false), IconButtonCell.IconButtonIconAlignmentEnum.Left | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter);
                        lvAttributes.Invalidate();
                    }
                }
                else if (row[2] is FontCell)
                {
                    if (((FontCell)row[2]).Font == null)
                    {
                        hasErrors = true;

                        row[3] = new IconButtonCell(new IconButton(Properties.Resources.Error16, Properties.Resources.FontValueMustBeSet,true,false), IconButtonCell.IconButtonIconAlignmentEnum.Left | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter);
                        lvAttributes.Invalidate();
                    }
                }
                else if (row[2] is EditableNumericCell)
                {
                    if (((EditableNumericCell)row[2]).Value < 14 || ((EditableNumericCell)row[2]).Value > 100)
                    {
                        hasErrors = true;

                        row[3] = new IconButtonCell(new IconButton(Properties.Resources.Error16, Properties.Resources.ValueMustBeBetween14AND100, true, false), IconButtonCell.IconButtonIconAlignmentEnum.Left | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter);
                        lvAttributes.Invalidate();
                    }
                }
                else if (row[2] is PictureCell)
                {
                    if (((PictureCell)row[2]).Image == null)
                    {
                        hasErrors = true;

                        row[3] = new IconButtonCell(new IconButton(Properties.Resources.Error16, Properties.Resources.ImageMustBeSelected, true, false), IconButtonCell.IconButtonIconAlignmentEnum.Left | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter);
                        lvAttributes.Invalidate();
                    }
                    else
                    {
                        if (((PictureCell)row[2]).Image.Width > 512 || ((PictureCell)row[2]).Image.Height > 512)
                        {
                            hasErrors = true;

                            row[3] = new IconButtonCell(new IconButton(Properties.Resources.Error16, Properties.Resources.TextureMaxSize, true, false), IconButtonCell.IconButtonIconAlignmentEnum.Left | IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter);
                            lvAttributes.Invalidate();
                        }
                    }
                }
            }

            return hasErrors;
        }

        internal void PutValuesInStyle(BaseStyle style)
        {
            foreach (var row in lvAttributes.Rows)
            {
                ContextStyleAttribute currentAttribute = (ContextStyleAttribute)row.Tag;

                switch (currentAttribute)
                {
                    case ContextStyleAttribute.BackColor:
                        style.BackColor = ((ColorBoxCell)row[2]).BoxColor;
                        break;

                    case ContextStyleAttribute.BackColor3:
                        style.BackColor3 = ((ColorBoxCell)row[2]).BoxColor;
                        break;

                    case ContextStyleAttribute.BorderColor:
                        style.BorderColor = ((ColorBoxCell)row[2]).BoxColor;
                        break;

                    case ContextStyleAttribute.BorderColor2:
                        style.BorderColor2 = ((ColorBoxCell)row[2]).BoxColor;
                        break;

                    case ContextStyleAttribute.ForeColor:
                        style.ForeColor = ((ColorBoxCell)row[2]).BoxColor;
                        break;

                    case ContextStyleAttribute.ForeColor2:
                        style.ForeColor2 = ((ColorBoxCell)row[2]).BoxColor;
                        break;

                    case ContextStyleAttribute.OverlayColor:
                        style.OverlayColor = ((ColorBoxCell)row[2]).BoxColor;
                        break;

                    case ContextStyleAttribute.LineTokenColor:
                        style.LineTokenColor = ((ColorBoxCell)row[2]).BoxColor;
                        break;

                    case ContextStyleAttribute.LineToken:
                        style.LineToken = ((TokenCell)row[2]).Token;
                        break;

                    case ContextStyleAttribute.Font:
                        style.Font = ((FontCell)row[2]).Font;
                        break;

                    case ContextStyleAttribute.FontStyleOverride:
                        style.FontStyle = ((FontStyleCell)row[2]).FontStyle;
                        break;

                    case ContextStyleAttribute.RowHeight:
                        style.Height = (int)((EditableNumericCell)row[2]).Value;
                        break;

                    case ContextStyleAttribute.Texture:
                        style.BackgroundTexture = ((PictureCell)row[2]).Image;
                        break;
                }
            }
        }

        public bool Changed
        {
            get
            {
                if (uiStyle == null)
                {
                    return false;
                }

                if (isNew)
                {
                    return true;
                }

                BaseStyle style = new BaseStyle();

                PutValuesInStyle(style);

                string newStyleData = JsonConvert.SerializeObject(style);
                string oldStyleData = JsonConvert.SerializeObject(uiStyle.Style);

                return newStyleData != oldStyleData || valueCount != lvAttributes.RowCount;
            }
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            btnsEditAddRemove_AddButtonMouseDown(sender, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left,0,0,0,0));
        }

        private void lvAttributes_SelectionChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private ToolStripItem[] GetToolStripItemsForAdd(EventHandler itemClick)
        {
            bool found;

            List<ToolStripMenuItem> items = new List<ToolStripMenuItem>();

            ContextStyleAttribute[] values = (ContextStyleAttribute[])Enum.GetValues(typeof(ContextStyleAttribute));

            if (!(uiStyle.ParentStyleID == null || uiStyle.ParentStyleID == RecordIdentifier.Empty))
            {
                foreach (ContextStyleAttribute value in values)
                {
                    if ((contextDescription.Mandatory & value) == value && value != ContextStyleAttribute.None)
                    {
                        if (!AttributeIsSet(value))
                        {
                            // We need to do one more check and loop through the UI list to make sure we dont have unset value of same type
                            found = false;

                            foreach (Row row in lvAttributes.Rows)
                            {
                                if ((ContextStyleAttribute)row.Tag == value)
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                ToolStripMenuItem item = new ToolStripMenuItem(contextDescription.GetAttributeName(value)) { Tag = value };
                                
                                if (itemClick != null)
                                {
                                    item.Click += itemClick;
                                }

                                items.Add(item);
                            }
                        }
                    }
                }
            }

            foreach (ContextStyleAttribute value in values)
            {
                if ((contextDescription.Optional & value) == value && value != ContextStyleAttribute.None)
                {
                    if (!AttributeIsSet(value))
                    {
                        // We need to do one more check and loop through the UI list to make sure we dont have unset value of same type
                        found = false;

                        foreach (Row row in lvAttributes.Rows)
                        {
                            if ((ContextStyleAttribute)row.Tag == value)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            ToolStripMenuItem item = new ToolStripMenuItem(contextDescription.GetAttributeName(value)) { Tag = value };

                            if (itemClick != null)
                            {
                                item.Click += itemClick;
                            }

                            items.Add(item);
                        }
                    }
                }
            }

            if (items.Count == 0)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(Properties.Resources.NoAttributesToAdd) { Enabled = false };

                items.Add(item);
            }

            return items.OrderBy(x => x.Text).ToArray<ToolStripMenuItem>();
        }

        

        private void btnsEditAddRemove_AddButtonMouseDown(object sender, MouseEventArgs e)
        {
            
            addContextMenu.Items.Clear();

            addContextMenu.Items.AddRange(GetToolStripItemsForAdd(null));

            addContextMenu.Show((Button)sender, 0, ((Button)sender).Height);
        }

        private void ContextMenu_ItemClick(object sender,EventArgs args)
        {
            addContextMenu_ItemClicked(sender, new ToolStripItemClickedEventArgs((ToolStripItem)sender));
        }

        private void addContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            AddOptionalRow((ContextStyleAttribute)e.ClickedItem.Tag);
            OnValueChanged();
            CheckColumnSizes();
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            switch ((ContextStyleAttribute)lvAttributes.Selection[0].Tag)
            {
                case ContextStyleAttribute.BackColor:
                    uiStyle.Style.BackColor = Color.Empty;
                    break;

                case ContextStyleAttribute.BackColor3:
                    uiStyle.Style.BackColor3 = Color.Empty;
                    break;

                case ContextStyleAttribute.BorderColor:
                    uiStyle.Style.BorderColor = Color.Empty;
                    break;

                case ContextStyleAttribute.BorderColor2:
                    uiStyle.Style.BorderColor2 = Color.Empty;
                    break;

                case ContextStyleAttribute.Font:
                    uiStyle.Style.Font = null;
                    break;

                case ContextStyleAttribute.FontStyleOverride:
                    uiStyle.Style.FontStyle = null;
                    break;

                case ContextStyleAttribute.ForeColor:
                    uiStyle.Style.ForeColor = Color.Empty;
                    break;

                case ContextStyleAttribute.ForeColor2:
                    uiStyle.Style.ForeColor2 = Color.Empty;
                    break;

                case ContextStyleAttribute.LineToken:
                    uiStyle.Style.LineToken = null;
                    break;

                case ContextStyleAttribute.LineTokenColor:
                    uiStyle.Style.LineTokenColor = Color.Empty;
                    break;

                case ContextStyleAttribute.OverlayColor:
                    uiStyle.Style.OverlayColor = Color.Empty;
                    break;

                case ContextStyleAttribute.RowHeight:
                    uiStyle.Style.Height = null;
                    break;

                case ContextStyleAttribute.Texture:
                    uiStyle.Style.BackgroundTexture = null;
                    break;

            }

            lvAttributes.RemoveRow(lvAttributes.Selection.FirstSelectedRow);
            OnValueChanged();
            CheckColumnSizes();
        }

        private void lvAttributes_CellDropDown(object sender, CellDropDownEventArgs args)
        {
            if (args.Cell is TokenCell)
            {
                var item = new ToolStripTokenItem(BaseStyle.LineTokenEnum.NoToken);
                item.Checked = ((TokenCell)args.Cell).Token == BaseStyle.LineTokenEnum.NoToken;

                args.Items.Add(item);

                item = new ToolStripTokenItem(BaseStyle.LineTokenEnum.Circle);
                item.Checked = ((TokenCell)args.Cell).Token == BaseStyle.LineTokenEnum.Circle;
                args.Items.Add(item);

                item = new ToolStripTokenItem(BaseStyle.LineTokenEnum.Rectangle);
                item.Checked = ((TokenCell)args.Cell).Token == BaseStyle.LineTokenEnum.Rectangle;
                args.Items.Add(item);

                item = new ToolStripTokenItem(BaseStyle.LineTokenEnum.TriangleLeft);
                item.Checked = ((TokenCell)args.Cell).Token == BaseStyle.LineTokenEnum.TriangleLeft;
                args.Items.Add(item);

                item = new ToolStripTokenItem(BaseStyle.LineTokenEnum.TriangleRight);
                item.Checked = ((TokenCell)args.Cell).Token == BaseStyle.LineTokenEnum.TriangleRight;
                args.Items.Add(item);

                item = new ToolStripTokenItem(BaseStyle.LineTokenEnum.TriangleUp);
                item.Checked = ((TokenCell)args.Cell).Token == BaseStyle.LineTokenEnum.TriangleUp;
                args.Items.Add(item);

                item = new ToolStripTokenItem(BaseStyle.LineTokenEnum.TriangleDown);
                item.Checked = ((TokenCell)args.Cell).Token == BaseStyle.LineTokenEnum.TriangleDown;
                args.Items.Add(item);
            }
        }

        private void btnEditParent_Click(object sender, EventArgs e)
        {
            StyleDialog dlg = new StyleDialog(uiStyle.ParentStyleID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "UIStyle", uiStyle.ParentStyleID, dlg.UIStyle.ContextID);
            }
        }
    }
}
