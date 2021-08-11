using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.POS.Core;
using LSOne.POS.Processes.WinFormsTouch;
using LSOne.Services.Interfaces.SupportClasses.IDialog;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using PriceHandlings = LSOne.Services.Interfaces.Enums.PriceHandlings;

namespace LSOne.Services.WinFormsTouch
{
    public partial class frmPopUpDialog : TouchBaseForm
    {
        PopUpFormData popUpFromData = new PopUpFormData();
        MenuButton selectedGroupButton = null;
        Group selectedGroup = null;
        bool clearQty = false;

        Color groupbuttonColor = ColorPalette.POSBackgroundColor;
        Color groupbuttonColorSelected = ColorPalette.POSButtonBlackButton;
        Color groupbuttonBorderColor = ColorPalette.POSControlBorderColor;
        Color groupbuttonTextDark = ColorPalette.POSTextForDarkButton;
        Color groupbuttonTextLight = ColorPalette.POSTextForLightButton;

        Color itembuttonColor = ColorPalette.NormalButton;
        Color itembuttonTextColor = ColorPalette.POSTextForLightButton;
        Color itembuttonColorSelected = ColorPalette.POSControlBorderColor;
        Color itembuttonBorderColor = ColorPalette.POSControlBorderColor;

        Font groupButtonFont = new Font("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Point);
        Font groupButtonSelectedFont = new Font("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Point);
        Font itemButtonFont = new Font("Segoe UI", 12, FontStyle.Regular, GraphicsUnit.Point);
        int buttonSpacing = 5;

        string textSelected = "Sel. ";
        string textRequired = "Req. ";
        string textNoCharge = "No Charge";
        string textExtraCharge = "Extra Charge:";
        string textDiscount = "Discount:";
        string textNormalPrice = "Normal price";

        public frmPopUpDialog(ref PopUpFormData popUpFormData) 
        {
            //
            // Get all text through the Translation function in the ApplicationLocalizer
            //
            // TextID's for frmPopUpDialog are reserved at 2800 - 2849            
            //

            InitializeComponent();

            textSelected = Properties.Resources.Sel;
            textRequired = Properties.Resources.Req;
            textNoCharge = Properties.Resources.NoCharge;
            textExtraCharge = Properties.Resources.Price + ":";
            textDiscount = Properties.Resources.Discount;
            textNormalPrice = Properties.Resources.NormalPrice;

            //Set the size of the form the half the size of the main form
            this.Width = DLLEntry.Settings.MainFormInfo.MainWindowWidth / 2;
            this.Height = DLLEntry.Settings.MainFormInfo.MainWindowHeight / 2;

            this.popUpFromData = popUpFormData;
            banner.BannerText = popUpFromData.HeaderText;
            SetButtonColorOverrides();
            SetInitialLayout();
        }

        private void SetButtonColorOverrides()
        {
            if(!RecordIdentifier.IsEmptyOrNull(DLLEntry.Settings.VisualProfile.ActionButtonStyleID))
            {
                PosStyle style = Providers.PosStyleData.Get(DLLEntry.DataModel, DLLEntry.Settings.VisualProfile.ActionButtonStyleID);
                groupbuttonColorSelected = Color.FromArgb(style.BackColor);
                groupbuttonTextDark = Color.FromArgb(style.ForeColor);

                FontStyle fontStyle = FontStyle.Regular;

                if(style.FontBold) { fontStyle |= FontStyle.Bold; }
                if(style.FontItalic) { fontStyle |= FontStyle.Italic; }

                groupButtonSelectedFont = new Font(style.FontName, style.FontSize, fontStyle, GraphicsUnit.Point);
            }

            if (!RecordIdentifier.IsEmptyOrNull(DLLEntry.Settings.VisualProfile.OtherButtonStyleID))
            {
                PosStyle style = Providers.PosStyleData.Get(DLLEntry.DataModel, DLLEntry.Settings.VisualProfile.OtherButtonStyleID);
                groupbuttonColor = Color.FromArgb(style.BackColor);
                groupbuttonTextLight = Color.FromArgb(style.ForeColor);

                FontStyle fontStyle = FontStyle.Regular;

                if (style.FontBold) { fontStyle |= FontStyle.Bold; }
                if (style.FontItalic) { fontStyle |= FontStyle.Italic; }

                groupButtonFont = new Font(style.FontName, style.FontSize, fontStyle, GraphicsUnit.Point);
            }

            if (!RecordIdentifier.IsEmptyOrNull(DLLEntry.Settings.VisualProfile.NormalButtonStyleID))
            {
                PosStyle style = Providers.PosStyleData.Get(DLLEntry.DataModel, DLLEntry.Settings.VisualProfile.NormalButtonStyleID);
                itembuttonColor = Color.FromArgb(style.BackColor);
                itembuttonTextColor = Color.FromArgb(style.ForeColor);

                FontStyle fontStyle = FontStyle.Regular;

                if (style.FontBold) { fontStyle |= FontStyle.Bold; }
                if (style.FontItalic) { fontStyle |= FontStyle.Italic; }

                itemButtonFont = new Font(style.FontName, style.FontSize, fontStyle, GraphicsUnit.Point);
            }
        }

        /// <summary>
        /// Set the initial layout of the form, group buttons and items buttons
        /// </summary>
        void SetInitialLayout()
        {
            //SuspendLayout 
            tableLayoutPanelItems.SuspendLayout();
            tableLayoutPanelGroups.SuspendLayout();

            //Clear the format for the group buttons
            tableLayoutPanelGroups.RowStyles.Clear();
            tableLayoutPanelGroups.Controls.Clear();

            //Populate the group button list
            List<Group> groups = popUpFromData.GetGroups();
            tableLayoutPanelGroups.RowCount = groups.Count;

            //Add the group buttons the group panel.
            int i = 0;
            foreach (Group group in groups)
            {
                //Create new rowstyles
                tableLayoutPanelGroups.RowStyles.Add(new RowStyle(SizeType.Percent, (100 / groups.Count)));
                //Add the buttons
                MenuButton btnGroup = new MenuButton();
                btnGroup.Dock = DockStyle.Fill;
                if (i == 0)
                {
                    btnGroup.ButtonColor = groupbuttonColorSelected;
                    btnGroup.ButtonColor2 = groupbuttonColorSelected;
                    btnGroup.ForeColor = groupbuttonTextDark;
                    btnGroup.Glyph1Color = groupbuttonTextDark;
                    btnGroup.Font = groupButtonSelectedFont;
                    selectedGroupButton = btnGroup;
                }
                else
                {
                    btnGroup.ButtonColor = groupbuttonColor;
                    btnGroup.ButtonColor2 = groupbuttonColor;
                    btnGroup.ForeColor = groupbuttonTextLight;
                    btnGroup.Glyph1Color = groupbuttonTextLight;
                    btnGroup.Font = groupButtonFont;
                }

                btnGroup.BorderColor = groupbuttonBorderColor;
                btnGroup.GradientMode = GradientModeEnum.None;
                btnGroup.Shape = ShapeEnum.Rectangle;
                btnGroup.Margin = new Padding(0, (i == 0) ? 0 : buttonSpacing, 0, 0);
                btnGroup.Text = group.Text;
                btnGroup.Tag = group.GroupId;
                btnGroup.Glyph1Text = textSelected + " " + group.NumberOfClicks.ToString();
                btnGroup.FocusHighlighting = false;

                if (group.InputRequired == true)
                {
                    btnGroup.Glyph3Text = textRequired + " " + group.MinSelection.ToString();
                }
                btnGroup.Click += new EventHandler(btnGroup_Click);
                tableLayoutPanelGroups.Controls.Add(btnGroup, 0, i);
                i++;
            }

            //Hide the group column if there are no groups, else set the item/variant buttons for the initial group.
            if (groups.Count == 0)
            {
                tableLayoutPanelGroups.Visible = false;
                tableLayoutPanelItems.Width += tableLayoutPanelGroups.Width + 5;
                SetItemButtons("");
                lblInfo.Text = Properties.Resources.MultipleSelection;
            }
            else
            {
                selectedGroup = groups[0];
                //If the group is a variant group, draw the variant buttons, else the item buttons.
                if (selectedGroup.IsVariantGroup)
                {
                    SetVariantButtons(selectedGroup);
                }
                else
                {
                    SetItemButtons(selectedGroup.GroupId);
                }
            }

            //Resume the layout.
            tableLayoutPanelItems.ResumeLayout();
            tableLayoutPanelGroups.ResumeLayout();
        }

        /// <summary>
        /// Draw up the variant item buttons
        /// </summary>
        /// <param name="GroupId">The varient group selected</param>
        void SetVariantButtons(Group Group)
        {
            //Set the header info text
            lblInfo.Text = Properties.Resources.SelectionRequired;
            
            //Get variant items that fits the dimension selection
            List<string> items = null;
            switch (Group.Dimension)
            {
                case Group.Dimensions.Dimension1: items = popUpFromData.GetVariantsItemsDim1(popUpFromData.Dimension1Selected, popUpFromData.Dimension2Selected, popUpFromData.Dimension3Selected);
                    break;
                case Group.Dimensions.Dimension2: items = popUpFromData.GetVariantsItemsDim2(popUpFromData.Dimension1Selected, popUpFromData.Dimension2Selected, popUpFromData.Dimension3Selected);
                    break;
                case Group.Dimensions.Dimension3: items = popUpFromData.GetVariantsItemsDim3(popUpFromData.Dimension1Selected, popUpFromData.Dimension2Selected, popUpFromData.Dimension3Selected);
                    break;
            }

            //Calculate number of columns and rows to be displayed.  
            int numberOfItems = items.Count();
            double sqrtTableRows = Math.Sqrt(numberOfItems);
            int numberOfColumns = (int)Math.Truncate(sqrtTableRows);
            int numberOfRows = 0;
            if (sqrtTableRows == (double)numberOfColumns)
                numberOfRows = numberOfColumns;
            else if (Math.Truncate((decimal)(numberOfItems / numberOfColumns)) == 0.0M)
                numberOfRows = numberOfItems / numberOfColumns;
            else
                numberOfRows = (int)Math.Ceiling((decimal)(numberOfItems) / (decimal)numberOfColumns);

            //Remove previous column and row format.
            tableLayoutPanelItems.RowStyles.Clear();
            tableLayoutPanelItems.ColumnStyles.Clear();
            //Remove any previous controls
            tableLayoutPanelItems.Controls.Clear();

            //Set the number of columns and rows
            tableLayoutPanelItems.RowCount = numberOfRows;
            tableLayoutPanelItems.ColumnCount = numberOfColumns;

            //Set the format of the row
            for (int i = 0; i < numberOfRows; i++)
            {
                tableLayoutPanelItems.RowStyles.Add(new RowStyle(SizeType.Percent, (100 / numberOfRows)));
            }
            //Set the format of the columns
            for (int i = 0; i < numberOfColumns; i++)
            {
                tableLayoutPanelItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (100 / numberOfColumns)));
            }

            //Add the buttons
            int id = 0;
            for (int row = 0; row < numberOfRows; row++)
            {
                for (int col = 0; col < numberOfColumns; col++)
                {
                    MenuButton btnItem = new MenuButton();
                    btnItem.Dock = DockStyle.Fill;
                    btnItem.ButtonColor = itembuttonColor;
                    btnItem.ButtonColor2 = itembuttonColor;
                    btnItem.BorderColor = itembuttonBorderColor;
                    btnItem.GradientMode = GradientModeEnum.None;
                    btnItem.Shape = ShapeEnum.Rectangle;
                    btnItem.Margin = new Padding((col == 0) ? 0 : buttonSpacing, (row == 0) ? 0 : buttonSpacing, 0, 0);
                    btnItem.Font = itemButtonFont;
                    btnItem.ForeColor = itembuttonTextColor;
                    btnItem.Glyph1Color = itembuttonTextColor;
                    btnItem.Glyph3Color = itembuttonTextColor;
                    btnItem.FocusHighlighting = false;
                    btnItem.Text = items.ElementAt(id);
                    GroupAndItem groupAndItem = new GroupAndItem(Group.GroupId, items.ElementAt(id),true);
                    btnItem.Tag = groupAndItem;
                    btnItem.Click += new EventHandler(btnItem_Click);
                    tableLayoutPanelItems.Controls.Add(btnItem, col, row);
                    id++;
                    if (id >= items.Count()) break;
                }
            }
        }

        /// <summary>
        /// Draw up the item buttons
        /// </summary>
        /// <param name="GroupId">The id of the selected group</param>
        void SetItemButtons(string GroupId)
        {
            //Set the header and info text
            if (selectedGroup != null)
            {
                if (selectedGroup.InputRequired)
                    lblInfo.Text = Properties.Resources.SelectionRequired;
                else if (selectedGroup.MultipleSelection)
                    lblInfo.Text = Properties.Resources.MultipleSelection;
                else
                    lblInfo.Text = String.Format(Properties.Resources.SelectMax, selectedGroup.MaxSelection.ToString());
            }
            else
            {
                lblInfo.Text = Properties.Resources.MultipleSelection;
            }

            List<Item> items = popUpFromData.GetItems(GroupId);

            //Calculate number of columns and rows to be displayed.  
            double sqrtTableRows = Math.Sqrt(items.Count);
            int numberOfColumns = (int)Math.Truncate(sqrtTableRows);
            int numberOfRows = 0;
            if (sqrtTableRows == (double)numberOfColumns)
                numberOfRows = numberOfColumns;
            else if (Math.Truncate((decimal)(items.Count / numberOfColumns)) == 0.0M)
                numberOfRows = items.Count / numberOfColumns;
            else
                numberOfRows = (int)Math.Ceiling((decimal)(items.Count) / (decimal)numberOfColumns);

            //Remove previous column and row format.
            tableLayoutPanelItems.RowStyles.Clear();
            tableLayoutPanelItems.ColumnStyles.Clear();
            //Clear any previous controls.
            tableLayoutPanelItems.Controls.Clear();

            //Set the number of columns and rows
            tableLayoutPanelItems.RowCount = numberOfRows;
            tableLayoutPanelItems.ColumnCount = numberOfColumns;

            //Set the format of the row
            for (int i = 0; i < numberOfRows; i++)
            {
                tableLayoutPanelItems.RowStyles.Add(new RowStyle(SizeType.Percent, (100 / numberOfRows)));
            }
            //Set the format of the columns
            for (int i = 0; i < numberOfColumns; i++)
            {
                tableLayoutPanelItems.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, (100 / numberOfColumns)));
            }

            //Add the buttons
            int id = 0;
            for (int row = 0; row < numberOfRows; row++)
            {
                for (int col = 0; col < numberOfColumns; col++)
                {

                    MenuButton btnItem = new MenuButton();
                    btnItem.Dock = DockStyle.Fill;
                    btnItem.ButtonColor = itembuttonColor;
                    btnItem.ButtonColor2 = itembuttonColor;
                    btnItem.BorderColor = itembuttonColorSelected;
                    btnItem.GradientMode = GradientModeEnum.None;
                    btnItem.Shape = ShapeEnum.Rectangle;
                    btnItem.Margin = new Padding((col == 0) ? 0 : buttonSpacing, (row == 0) ? 0 : buttonSpacing, 0, 0);
                    btnItem.Font = itemButtonFont;
                    btnItem.ForeColor = itembuttonTextColor;
                    btnItem.Glyph1Color = itembuttonTextColor;
                    btnItem.Glyph3Color = itembuttonTextColor;
                    btnItem.FocusHighlighting = false;
                    btnItem.Text = items[id].Text;
                    if (items[id].NumberOfClicks > 0)
                    {
                        btnItem.ButtonColor = itembuttonColorSelected;
                        btnItem.ButtonColor2 = itembuttonColorSelected;
                        btnItem.Glyph1Text = textSelected + " " + items[id].NumberOfClicks.ToString();
                    }
                    else
                    {
                        btnItem.ButtonColor = itembuttonColor;
                        btnItem.ButtonColor2 = itembuttonColor;
                    }

                    if (items[id].PriceHandling == PriceHandlings.AlwaysCharge)
                    {
                        if (items[id].PriceType == PriceTypes.Percent)
                        {
                            btnItem.Glyph3Text = textDiscount + " " + items[id].AmountPercentage.ToString("N2") + "%";
                        }
                        else if (items[id].PriceType == PriceTypes.None)
                        {
                            btnItem.Glyph3Text = textNoCharge;
                        }
                        else if (items[id].PriceType == PriceTypes.FromItem)
                        {
                            btnItem.Glyph3Text = textNormalPrice;
                        }
                        else
                        {
                            btnItem.Glyph3Text = textExtraCharge + " " + items[id].AmountPercentage.ToString("N2");
                        }
                    }
                    if (items[id].PriceHandling == PriceHandlings.NoCharge)
                    {
                        btnItem.Glyph3Text = textNoCharge;
                    }

                    GroupAndItem groupAndItem = new GroupAndItem(items[id].GroupId, items[id].ItemId,false);
                    btnItem.Tag = groupAndItem;
                    btnItem.Click += new EventHandler(btnItem_Click);
                    tableLayoutPanelItems.Controls.Add(btnItem, col, row);
                    id++;
                    if (id >= items.Count) break;
                }
            }
        }

        /// <summary>
        /// Event fired when a item/variant button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnItem_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(MenuButton))
            {
                if (selectedGroup != null)
                {
                    if (clearQty) // If "Clear selection" was selected before the item/variant button was pressed.
                    {
                        GroupAndItem groupAndItem = (GroupAndItem)((MenuButton)sender).Tag;
                        popUpFromData.ClearQty(groupAndItem.Group, groupAndItem.Item);
                        Item item = popUpFromData.GetItem(groupAndItem.Group, groupAndItem.Item);
                        if (item.NumberOfClicks == 0)
                        {
                            ((MenuButton)sender).ButtonColor = itembuttonColor;
                            ((MenuButton)sender).ButtonColor2 = itembuttonColor;
                            ((MenuButton)sender).Glyph1Text = textSelected + " " + item.NumberOfClicks.ToString();
                        }
                        clearQty = false;
                    }
                    else
                    {
                        if ((selectedGroup.NumberOfClicks >= selectedGroup.MaxSelection) && (selectedGroup.MaxSelection > 0))
                        {
                            LSOne.Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(Properties.Resources.MaximumSelectionReached); //Maximum selection reached for the group                            
                        }
                        else
                        {
                            GroupAndItem groupAndItem = (GroupAndItem)((MenuButton)sender).Tag;
                            if (popUpFromData.ItemSelected(groupAndItem.Group, groupAndItem.Item, groupAndItem.DimensionItem))
                            {
                                Item item = popUpFromData.GetItem(groupAndItem.Group, groupAndItem.Item);
                                ((MenuButton)sender).ButtonColor = itembuttonColorSelected;
                                ((MenuButton)sender).Glyph1Text = textSelected + " " + item.NumberOfClicks.ToString();
                                ((MenuButton)sender).Glyph1Color = itembuttonTextColor;
                                selectedGroupButton.Glyph1Text = textSelected + " " + selectedGroup.NumberOfClicks.ToString();

                                if (item.PriceHandling == PriceHandlings.AlwaysCharge)
                                {
                                    if (item.PriceType == PriceTypes.Percent)
                                    {
                                        ((MenuButton)sender).Glyph3Text = textDiscount + " " + item.AmountPercentage.ToString("N2") + "%";
                                    }
                                    else if (item.PriceType == PriceTypes.None)
                                    {
                                        ((MenuButton)sender).Glyph3Text = textNoCharge;
                                    }
                                    else if (item.PriceType == PriceTypes.FromItem)
                                    {
                                        ((MenuButton)sender).Glyph3Text = textNormalPrice;
                                    }
                                    else
                                    {
                                        ((MenuButton)sender).Glyph3Text = textExtraCharge + " " + item.AmountPercentage.ToString("N2");
                                    }
                                }                                
                                else if (item.PriceHandling == PriceHandlings.NoCharge)
                                {
                                    ((MenuButton)sender).Glyph3Text = textNoCharge;
                                }

                                if (groupAndItem.DimensionItem)
                                {
                                    SetVariantButtons(selectedGroup);
                                }
                            }
                        }
                    }
                    
                }
                else
                { 
                    //do something.
                    //MessageBox.Show(((Button)sender).Text + " was clicked");
                }
            }
        }

        /// <summary>
        /// Event fired when a group button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnGroup_Click(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(MenuButton))
            {
                MenuButton btnGroup = (MenuButton)sender;

                if (selectedGroup.GroupId != (string)(btnGroup.Tag))
                {
                    if (selectedGroupButton != null)
                    {
                        selectedGroupButton.ButtonColor = groupbuttonColor;
                        selectedGroupButton.ForeColor = groupbuttonTextLight;
                        selectedGroupButton.Glyph1Color = groupbuttonTextLight;
                        selectedGroupButton.Font = groupButtonFont;
                    }
                    btnGroup.ButtonColor = groupbuttonColorSelected;
                    btnGroup.ForeColor = groupbuttonTextDark;
                    btnGroup.Font = groupButtonSelectedFont;
                    selectedGroup = popUpFromData.GetGroup((string)btnGroup.Tag);
                    selectedGroupButton = btnGroup;
                    
                    if (selectedGroup.IsVariantGroup)
                        SetVariantButtons(selectedGroup);
                    else
                        SetItemButtons((string)btnGroup.Tag);

                    btnGroup.Glyph1Text = textSelected + " " + selectedGroup.NumberOfClicks.ToString();
                    btnGroup.Glyph1Color = groupbuttonTextDark;
                    if (selectedGroup.InputRequired == true)
                    {
                        btnGroup.Glyph3Text = textRequired + " " + selectedGroup.MinSelection.ToString();
                    }
                }
            }
        }


        private void btnClearQty_Click(object sender, EventArgs e)
        {
            clearQty = true;
        }

        private void btnClearSelection_Click(object sender, EventArgs e)
        {
            if (selectedGroup != null)
            {
                popUpFromData.ClearSelection(selectedGroup);
                selectedGroupButton.Glyph1Text = textSelected + " " + selectedGroup.NumberOfClicks.ToString();

                if (selectedGroup.IsVariantGroup)
                        SetVariantButtons(selectedGroup);
                    else
                        SetItemButtons(selectedGroup.GroupId);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            popUpFromData.ClearAllSelection();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (popUpFromData.SelectionCompleted())
            {     
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
                    
            if (selectedGroup.InputRequired && !selectedGroup.SelectionCompleted)
            {
                LSOne.Services.Interfaces.Services.DialogService(DLLEntry.DataModel).ShowMessage(String.Format(Properties.Resources.MinimumRequirementIs, selectedGroup.MinSelection.ToString()), MessageBoxButtons.OK, MessageDialogType.Generic);           
            }
            else
            {
                SetNextGroupButton();
            }
        }

        private void SetNextGroupButton()
        {
            selectedGroup = popUpFromData.FindNextActionGroup(selectedGroup.GroupId);
            foreach (Control control in tableLayoutPanelGroups.Controls)
            {
                if (control.GetType() == typeof(MenuButton))
                {
                    if ((string)((MenuButton)control).Tag == selectedGroup.GroupId)
                    {
                        if (selectedGroupButton != null) selectedGroupButton.ButtonColor = groupbuttonColor;
                        selectedGroupButton = (MenuButton)control;
                        selectedGroupButton.ButtonColor = groupbuttonColorSelected;
                        selectedGroupButton.Font = groupButtonSelectedFont;

                        if (selectedGroup.IsVariantGroup)
                            SetVariantButtons(selectedGroup);
                        else
                            SetItemButtons((string)selectedGroupButton.Tag);

                        break;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// Struct to store information on buttom tag.
    /// </summary>
    struct GroupAndItem
    {
        public GroupAndItem(string Group, string Item, bool DimensionItem)
        {
            this.Group = Group;
            this.Item = Item;
            this.DimensionItem = DimensionItem;
        }

        public string Group;
        public string Item;
        public bool DimensionItem;
    }
}
