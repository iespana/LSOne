using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class ItemSearchDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();

                invalidateTimer.Stop();
                invalidateTimer.Dispose();

                tickMarkImage.Dispose();
                tickMarkImage = null;
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemSearchDialog));
            this.panel = new LSOne.Controls.TouchScrollButtonPanel();
            this.tbSearch = new LSOne.Controls.ShadeTextBoxTouch();
            this.touchKeyboard1 = new LSOne.Controls.TouchKeyboard();
            this.lvItems = new LSOne.Controls.ListView();
            this.colItem = new LSOne.Controls.Columns.Column();
            this.colItemName = new LSOne.Controls.Columns.Column();
            this.colItemGroup = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // panel
            // 
            resources.ApplyResources(this.panel, "panel");
            this.panel.BackColor = System.Drawing.Color.Transparent;
            this.panel.ButtonHeight = 50;
            this.panel.Name = "panel";
            this.panel.Click += new LSOne.Controls.ScrollButtonEventHandler(this.panel_Click);
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.BackColor = System.Drawing.Color.White;
            this.tbSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbSearch.MaxLength = 999;
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyDown);
            // 
            // touchKeyboard1
            // 
            resources.ApplyResources(this.touchKeyboard1, "touchKeyboard1");
            this.touchKeyboard1.BackColor = System.Drawing.Color.Transparent;
            this.touchKeyboard1.BuddyControl = null;
            this.touchKeyboard1.Name = "touchKeyboard1";
            this.touchKeyboard1.TabStop = false;
            this.touchKeyboard1.EnterPressed += new System.EventHandler(this.touchKeyboard1_EnterPressed);
            this.touchKeyboard1.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard1_ObtainCultureName);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.ApplyVisualStyles = false;
            this.lvItems.BackColor = System.Drawing.Color.White;
            this.lvItems.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.colItem);
            this.lvItems.Columns.Add(this.colItemName);

            this.lvItems.Columns.Add(this.colItemGroup);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(50));
            this.lvItems.EvenRowColor = System.Drawing.Color.White;
            this.lvItems.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvItems.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvItems.HeaderHeight = ((short)(30));
            this.lvItems.HideHorizontalScrollbarWhenDisabled = true;
            this.lvItems.HideVerticalScrollbarWhenDisabled = true;
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvItems.RowLines = true;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "0:1";
            this.lvItems.TouchScroll = true;
            this.lvItems.UseFocusRectangle = false;
            this.lvItems.VerticalScrollbarValue = 0;
            this.lvItems.VerticalScrollbarYOffset = 0;
            this.lvItems.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvItems.HeaderClicked += new LSOne.Controls.HeaderDelegate(this.lvItems_HeaderClicked);
            this.lvItems.CellAction += new LSOne.Controls.CellActionDelegate(this.lvItems_CellAction);
            this.lvItems.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvItems_RowDoubleClick);
            this.lvItems.VerticalScrollValueChanged += new System.EventHandler(this.lvItems_VerticalScrollValueChanged);
            // 
            // colItem
            // 
            this.colItem.AutoSize = true;
            this.colItem.DefaultStyle = null;
            resources.ApplyResources(this.colItem, "colItem");
            this.colItem.MaximumWidth = ((short)(0));
            this.colItem.MinimumWidth = ((short)(10));
            this.colItem.RelativeSize = 25;
            this.colItem.SecondarySortColumn = ((short)(-1));
            this.colItem.Sizable = false;
            this.colItem.Tag = null;
            this.colItem.Width = ((short)(175));
            // 
            // colItemName
            // 
            this.colItemName.AutoSize = true;
            this.colItemName.DefaultStyle = null;
            resources.ApplyResources(this.colItemName, "colItemName");
            this.colItemName.MaximumWidth = ((short)(0));
            this.colItemName.MinimumWidth = ((short)(10));
            this.colItemName.RelativeSize = 12;
            this.colItemName.SecondarySortColumn = ((short)(-1));
            this.colItemName.Tag = null;
            this.colItemName.Width = ((short)(315));
            // 
            // colItemGroup
            // 
            this.colItemGroup.AutoSize = true;
            this.colItemGroup.DefaultStyle = null;
            resources.ApplyResources(this.colItemGroup, "colItemGroup");
            this.colItemGroup.MaximumWidth = ((short)(0));
            this.colItemGroup.MinimumWidth = ((short)(10));
            this.colItemGroup.RelativeSize = 30;
            this.colItemGroup.SecondarySortColumn = ((short)(-1));
            this.colItemGroup.Tag = null;
            this.colItemGroup.Width = ((short)(210));
            // 
            // ItemSearchDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.touchKeyboard1);
            this.Controls.Add(this.lvItems);
            this.KeyPreview = true;
            this.Name = "ItemSearchDialog";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ItemSearchDialog_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.TouchScrollButtonPanel panel;
        private LSOne.Controls.ShadeTextBoxTouch tbSearch;
        private LSOne.Controls.TouchKeyboard touchKeyboard1;
        private LSOne.Controls.ListView lvItems;
        private LSOne.Controls.Columns.Column colItem;
        private LSOne.Controls.Columns.Column colItemName;
        private LSOne.Controls.Columns.Column colItemGroup;
    }
}