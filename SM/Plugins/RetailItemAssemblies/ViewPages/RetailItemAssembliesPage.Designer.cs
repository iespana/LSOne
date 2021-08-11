namespace LSOne.ViewPlugins.RetailItemAssemblies.ViewPages
{
    partial class RetailItemAssembliesPage
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
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpComponents = new System.Windows.Forms.GroupBox();
            this.btnsComponents = new LSOne.Controls.ContextButtons();
            this.lvComponents = new LSOne.Controls.ListView();
            this.clmItemID = new LSOne.Controls.Columns.Column();
            this.clmItemName = new LSOne.Controls.Columns.Column();
            this.clmVariantName = new LSOne.Controls.Columns.Column();
            this.clmQuantity = new LSOne.Controls.Columns.Column();
            this.clmComponentUnit = new LSOne.Controls.Columns.Column();
            this.clmCostPerUnit = new LSOne.Controls.Columns.Column();
            this.clmTotalCost = new LSOne.Controls.Columns.Column();
            this.lvAssemblies = new LSOne.Controls.ListView();
            this.clmStatus = new LSOne.Controls.Columns.Column();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmStore = new LSOne.Controls.Columns.Column();
            this.clmStartingDate = new LSOne.Controls.Columns.Column();
            this.clmCostPrice = new LSOne.Controls.Columns.Column();
            this.clmPrice = new LSOne.Controls.Columns.Column();
            this.clmMargin = new LSOne.Controls.Columns.Column();
            this.clmShowComponents = new LSOne.Controls.Columns.Column();
            this.clmSendToStations = new LSOne.Controls.Columns.Column();
            this.btnsAssemblies = new LSOne.Controls.ContextButtons();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnEnable = new System.Windows.Forms.Button();
            this.searchBar = new LSOne.Controls.SearchBar();
            this.clmCalculatePrice = new LSOne.Controls.Columns.Column();
            this.grpComponents.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpComponents
            // 
            this.grpComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.grpComponents, 2);
            this.grpComponents.Controls.Add(this.btnsComponents);
            this.grpComponents.Controls.Add(this.lvComponents);
            this.grpComponents.Location = new System.Drawing.Point(3, 282);
            this.grpComponents.Name = "grpComponents";
            this.grpComponents.Size = new System.Drawing.Size(791, 243);
            this.grpComponents.TabIndex = 3;
            this.grpComponents.TabStop = false;
            this.grpComponents.Text = "Components";
            // 
            // btnsComponents
            // 
            this.btnsComponents.AddButtonEnabled = true;
            this.btnsComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnsComponents.BackColor = System.Drawing.Color.Transparent;
            this.btnsComponents.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsComponents.EditButtonEnabled = true;
            this.btnsComponents.Location = new System.Drawing.Point(701, 213);
            this.btnsComponents.Name = "btnsComponents";
            this.btnsComponents.RemoveButtonEnabled = true;
            this.btnsComponents.Size = new System.Drawing.Size(84, 24);
            this.btnsComponents.TabIndex = 1;
            this.btnsComponents.EditButtonClicked += new System.EventHandler(this.btnsComponents_EditButtonClicked);
            this.btnsComponents.AddButtonClicked += new System.EventHandler(this.btnsComponents_AddButtonClicked);
            this.btnsComponents.RemoveButtonClicked += new System.EventHandler(this.btnsComponents_RemoveButtonClicked);
            // 
            // lvComponents
            // 
            this.lvComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvComponents.BorderColor = System.Drawing.Color.DarkGray;
            this.lvComponents.BuddyControl = null;
            this.lvComponents.Columns.Add(this.clmItemID);
            this.lvComponents.Columns.Add(this.clmItemName);
            this.lvComponents.Columns.Add(this.clmVariantName);
            this.lvComponents.Columns.Add(this.clmQuantity);
            this.lvComponents.Columns.Add(this.clmComponentUnit);
            this.lvComponents.Columns.Add(this.clmCostPerUnit);
            this.lvComponents.Columns.Add(this.clmTotalCost);
            this.lvComponents.ContentBackColor = System.Drawing.Color.White;
            this.lvComponents.DefaultRowHeight = ((short)(22));
            this.lvComponents.EvenRowColor = System.Drawing.Color.White;
            this.lvComponents.HeaderBackColor = System.Drawing.Color.White;
            this.lvComponents.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvComponents.HeaderHeight = ((short)(25));
            this.lvComponents.HorizontalScrollbar = true;
            this.lvComponents.Location = new System.Drawing.Point(6, 19);
            this.lvComponents.Name = "lvComponents";
            this.lvComponents.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvComponents.RowLineColor = System.Drawing.Color.LightGray;
            this.lvComponents.SecondarySortColumn = ((short)(-1));
            this.lvComponents.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvComponents.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvComponents.Size = new System.Drawing.Size(779, 188);
            this.lvComponents.SortSetting = "0:1";
            this.lvComponents.TabIndex = 0;
            this.lvComponents.VerticalScrollbarValue = 0;
            this.lvComponents.VerticalScrollbarYOffset = 0;
            this.lvComponents.SelectionChanged += new System.EventHandler(this.lvComponents_SelectionChanged);
            this.lvComponents.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvComponents_RowDoubleClick);
            // 
            // clmItemID
            // 
            this.clmItemID.AutoSize = true;
            this.clmItemID.DefaultStyle = null;
            this.clmItemID.HeaderText = "Item ID";
            this.clmItemID.InternalSort = true;
            this.clmItemID.MaximumWidth = ((short)(0));
            this.clmItemID.MinimumWidth = ((short)(10));
            this.clmItemID.SecondarySortColumn = ((short)(-1));
            this.clmItemID.Tag = null;
            this.clmItemID.Width = ((short)(50));
            // 
            // clmItemName
            // 
            this.clmItemName.AutoSize = true;
            this.clmItemName.DefaultStyle = null;
            this.clmItemName.HeaderText = "Item";
            this.clmItemName.InternalSort = true;
            this.clmItemName.MaximumWidth = ((short)(0));
            this.clmItemName.MinimumWidth = ((short)(10));
            this.clmItemName.SecondarySortColumn = ((short)(-1));
            this.clmItemName.Tag = null;
            this.clmItemName.Width = ((short)(50));
            // 
            // clmVariantName
            // 
            this.clmVariantName.AutoSize = true;
            this.clmVariantName.DefaultStyle = null;
            this.clmVariantName.HeaderText = "Variant";
            this.clmVariantName.InternalSort = true;
            this.clmVariantName.MaximumWidth = ((short)(0));
            this.clmVariantName.MinimumWidth = ((short)(10));
            this.clmVariantName.SecondarySortColumn = ((short)(-1));
            this.clmVariantName.Tag = null;
            this.clmVariantName.Width = ((short)(50));
            // 
            // clmQuantity
            // 
            this.clmQuantity.AutoSize = true;
            this.clmQuantity.DefaultStyle = null;
            this.clmQuantity.HeaderText = "Quantity";
            this.clmQuantity.InternalSort = true;
            this.clmQuantity.MaximumWidth = ((short)(0));
            this.clmQuantity.MinimumWidth = ((short)(10));
            this.clmQuantity.SecondarySortColumn = ((short)(-1));
            this.clmQuantity.Tag = null;
            this.clmQuantity.Width = ((short)(50));
            // 
            // clmComponentUnit
            // 
            this.clmComponentUnit.AutoSize = true;
            this.clmComponentUnit.DefaultStyle = null;
            this.clmComponentUnit.HeaderText = "Unit";
            this.clmComponentUnit.InternalSort = true;
            this.clmComponentUnit.MaximumWidth = ((short)(0));
            this.clmComponentUnit.MinimumWidth = ((short)(10));
            this.clmComponentUnit.SecondarySortColumn = ((short)(-1));
            this.clmComponentUnit.Tag = null;
            this.clmComponentUnit.Width = ((short)(50));
            // 
            // clmCostPerUnit
            // 
            this.clmCostPerUnit.AutoSize = true;
            this.clmCostPerUnit.DefaultStyle = null;
            this.clmCostPerUnit.HeaderText = "Cost per unit";
            this.clmCostPerUnit.InternalSort = true;
            this.clmCostPerUnit.MaximumWidth = ((short)(0));
            this.clmCostPerUnit.MinimumWidth = ((short)(10));
            this.clmCostPerUnit.SecondarySortColumn = ((short)(-1));
            this.clmCostPerUnit.Tag = null;
            this.clmCostPerUnit.Width = ((short)(50));
            // 
            // clmTotalCost
            // 
            this.clmTotalCost.AutoSize = true;
            this.clmTotalCost.DefaultStyle = null;
            this.clmTotalCost.HeaderText = "Total cost";
            this.clmTotalCost.InternalSort = true;
            this.clmTotalCost.MaximumWidth = ((short)(0));
            this.clmTotalCost.MinimumWidth = ((short)(10));
            this.clmTotalCost.SecondarySortColumn = ((short)(-1));
            this.clmTotalCost.Tag = null;
            this.clmTotalCost.Width = ((short)(50));
            // 
            // lvAssemblies
            // 
            this.lvAssemblies.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvAssemblies.BorderColor = System.Drawing.Color.DarkGray;
            this.lvAssemblies.BuddyControl = null;
            this.lvAssemblies.Columns.Add(this.clmStatus);
            this.lvAssemblies.Columns.Add(this.clmDescription);
            this.lvAssemblies.Columns.Add(this.clmStore);
            this.lvAssemblies.Columns.Add(this.clmStartingDate);
            this.lvAssemblies.Columns.Add(this.clmCostPrice);
            this.lvAssemblies.Columns.Add(this.clmPrice);
            this.lvAssemblies.Columns.Add(this.clmMargin);
            this.lvAssemblies.Columns.Add(this.clmShowComponents);
            this.lvAssemblies.Columns.Add(this.clmSendToStations);
            this.lvAssemblies.Columns.Add(this.clmCalculatePrice);
            this.tableLayoutPanel1.SetColumnSpan(this.lvAssemblies, 2);
            this.lvAssemblies.ContentBackColor = System.Drawing.Color.White;
            this.lvAssemblies.DefaultRowHeight = ((short)(22));
            this.lvAssemblies.EvenRowColor = System.Drawing.Color.White;
            this.lvAssemblies.HeaderBackColor = System.Drawing.Color.White;
            this.lvAssemblies.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAssemblies.HeaderHeight = ((short)(25));
            this.lvAssemblies.HorizontalScrollbar = true;
            this.lvAssemblies.Location = new System.Drawing.Point(3, 3);
            this.lvAssemblies.Name = "lvAssemblies";
            this.lvAssemblies.OddRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvAssemblies.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAssemblies.SecondarySortColumn = ((short)(-1));
            this.lvAssemblies.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvAssemblies.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvAssemblies.Size = new System.Drawing.Size(791, 242);
            this.lvAssemblies.SortSetting = "0:1";
            this.lvAssemblies.TabIndex = 0;
            this.lvAssemblies.VerticalScrollbarValue = 0;
            this.lvAssemblies.VerticalScrollbarYOffset = 0;
            this.lvAssemblies.SelectionChanged += new System.EventHandler(this.lvAssemblies_SelectionChanged);
            this.lvAssemblies.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvAssemblies_RowDoubleClick);
            // 
            // clmStatus
            // 
            this.clmStatus.AutoSize = true;
            this.clmStatus.Clickable = false;
            this.clmStatus.DefaultStyle = null;
            this.clmStatus.HeaderText = "Status";
            this.clmStatus.MaximumWidth = ((short)(0));
            this.clmStatus.MinimumWidth = ((short)(10));
            this.clmStatus.SecondarySortColumn = ((short)(-1));
            this.clmStatus.Tag = null;
            this.clmStatus.Width = ((short)(50));
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            this.clmDescription.HeaderText = "Description";
            this.clmDescription.InternalSort = true;
            this.clmDescription.MaximumWidth = ((short)(0));
            this.clmDescription.MinimumWidth = ((short)(10));
            this.clmDescription.SecondarySortColumn = ((short)(-1));
            this.clmDescription.Tag = null;
            this.clmDescription.Width = ((short)(50));
            // 
            // clmStore
            // 
            this.clmStore.AutoSize = true;
            this.clmStore.DefaultStyle = null;
            this.clmStore.HeaderText = "Store";
            this.clmStore.InternalSort = true;
            this.clmStore.MaximumWidth = ((short)(0));
            this.clmStore.MinimumWidth = ((short)(10));
            this.clmStore.SecondarySortColumn = ((short)(-1));
            this.clmStore.Tag = null;
            this.clmStore.Width = ((short)(50));
            // 
            // clmStartingDate
            // 
            this.clmStartingDate.AutoSize = true;
            this.clmStartingDate.DefaultStyle = null;
            this.clmStartingDate.HeaderText = "Starting date";
            this.clmStartingDate.InternalSort = true;
            this.clmStartingDate.MaximumWidth = ((short)(0));
            this.clmStartingDate.MinimumWidth = ((short)(10));
            this.clmStartingDate.SecondarySortColumn = ((short)(-1));
            this.clmStartingDate.Tag = null;
            this.clmStartingDate.Width = ((short)(50));
            // 
            // clmCostPrice
            // 
            this.clmCostPrice.AutoSize = true;
            this.clmCostPrice.DefaultStyle = null;
            this.clmCostPrice.HeaderText = "Cost price";
            this.clmCostPrice.InternalSort = true;
            this.clmCostPrice.MaximumWidth = ((short)(0));
            this.clmCostPrice.MinimumWidth = ((short)(10));
            this.clmCostPrice.SecondarySortColumn = ((short)(-1));
            this.clmCostPrice.Tag = null;
            this.clmCostPrice.Width = ((short)(50));
            // 
            // clmPrice
            // 
            this.clmPrice.AutoSize = true;
            this.clmPrice.DefaultStyle = null;
            this.clmPrice.HeaderText = "Price";
            this.clmPrice.InternalSort = true;
            this.clmPrice.MaximumWidth = ((short)(0));
            this.clmPrice.MinimumWidth = ((short)(10));
            this.clmPrice.SecondarySortColumn = ((short)(-1));
            this.clmPrice.Tag = null;
            this.clmPrice.Width = ((short)(50));
            // 
            // clmMargin
            // 
            this.clmMargin.AutoSize = true;
            this.clmMargin.DefaultStyle = null;
            this.clmMargin.HeaderText = "Profit margin (%)";
            this.clmMargin.InternalSort = true;
            this.clmMargin.MaximumWidth = ((short)(0));
            this.clmMargin.MinimumWidth = ((short)(10));
            this.clmMargin.SecondarySortColumn = ((short)(-1));
            this.clmMargin.Tag = null;
            this.clmMargin.Width = ((short)(50));
            // 
            // clmShowComponents
            // 
            this.clmShowComponents.AutoSize = true;
            this.clmShowComponents.DefaultStyle = null;
            this.clmShowComponents.HeaderText = "Show components on...";
            this.clmShowComponents.InternalSort = true;
            this.clmShowComponents.MaximumWidth = ((short)(0));
            this.clmShowComponents.MinimumWidth = ((short)(10));
            this.clmShowComponents.SecondarySortColumn = ((short)(-1));
            this.clmShowComponents.Tag = null;
            this.clmShowComponents.Width = ((short)(50));
            // 
            // clmSendToStations
            // 
            this.clmSendToStations.AutoSize = true;
            this.clmSendToStations.DefaultStyle = null;
            this.clmSendToStations.HeaderText = "Send to stations";
            this.clmSendToStations.InternalSort = true;
            this.clmSendToStations.MaximumWidth = ((short)(0));
            this.clmSendToStations.MinimumWidth = ((short)(10));
            this.clmSendToStations.SecondarySortColumn = ((short)(-1));
            this.clmSendToStations.Tag = null;
            this.clmSendToStations.Width = ((short)(50));
            
            // 
            // btnsAssemblies
            // 
            this.btnsAssemblies.AddButtonEnabled = true;
            this.btnsAssemblies.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnsAssemblies.BackColor = System.Drawing.Color.Transparent;
            this.btnsAssemblies.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsAssemblies.EditButtonEnabled = true;
            this.btnsAssemblies.Location = new System.Drawing.Point(710, 251);
            this.btnsAssemblies.Name = "btnsAssemblies";
            this.btnsAssemblies.RemoveButtonEnabled = true;
            this.btnsAssemblies.Size = new System.Drawing.Size(84, 25);
            this.btnsAssemblies.TabIndex = 2;
            this.btnsAssemblies.EditButtonClicked += new System.EventHandler(this.btnsAssemblies_EditButtonClicked);
            this.btnsAssemblies.AddButtonClicked += new System.EventHandler(this.btnsAssemblies_AddButtonClicked);
            this.btnsAssemblies.RemoveButtonClicked += new System.EventHandler(this.btnsAssemblies_RemoveButtonClicked);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lvAssemblies, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.grpComponents, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnsAssemblies, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnEnable, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 69);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(797, 528);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnEnable
            // 
            this.btnEnable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEnable.Location = new System.Drawing.Point(3, 251);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(75, 25);
            this.btnEnable.TabIndex = 1;
            this.btnEnable.Text = "Enable";
            this.btnEnable.UseVisualStyleBackColor = true;
            this.btnEnable.Click += new System.EventHandler(this.btnEnable_Click);
            // 
            // searchBar
            // 
            this.searchBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchBar.BackColor = System.Drawing.Color.Transparent;
            this.searchBar.BuddyControl = null;
            this.searchBar.Location = new System.Drawing.Point(3, 3);
            this.searchBar.Name = "searchBar";
            this.searchBar.SearchOptionEnabled = true;
            this.searchBar.Size = new System.Drawing.Size(791, 60);
            this.searchBar.TabIndex = 0;
            this.searchBar.SetupConditions += new System.EventHandler(this.searchBar_SetupConditions);
            this.searchBar.SearchClicked += new System.EventHandler(this.searchBar_SearchClicked);
            this.searchBar.SaveAsDefault += new System.EventHandler(this.searchBar_SaveAsDefault);
            this.searchBar.LoadDefault += new System.EventHandler(this.searchBar_LoadDefault);
            this.searchBar.UnknownControlAdd += new LSOne.Controls.UnknownControlCreatedDelegate(this.searchBar_UnknownControlAdd);
            this.searchBar.UnknownControlRemove += new LSOne.Controls.UnknownControlDelegate(this.searchBar_UnknownControlRemove);
            this.searchBar.UnknownControlHasSelection += new LSOne.Controls.UnknownControlSelectionDelegate(this.searchBar_UnknownControlHasSelection);
            this.searchBar.UnknownControlGetSelection += new LSOne.Controls.UnknownControlGetSelectionDelegate(this.searchBar_UnknownControlGetSelection);
            this.searchBar.UnknownControlSetSelection += new LSOne.Controls.UnknownControlSetSelectionDelegate(this.searchBar_UnknownControlSetSelection);
            // 
            // clmCalculatePrice
            // 
            this.clmCalculatePrice.AutoSize = true;
            this.clmCalculatePrice.DefaultStyle = null;
            this.clmCalculatePrice.HeaderText = "Calculate price";
            this.clmCalculatePrice.InternalSort = true;
            this.clmCalculatePrice.MaximumWidth = ((short)(0));
            this.clmCalculatePrice.MinimumWidth = ((short)(10));
            this.clmCalculatePrice.SecondarySortColumn = ((short)(-1));
            this.clmCalculatePrice.Tag = null;
            this.clmCalculatePrice.Width = ((short)(50));
            // 
            // RetailItemAssembliesPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.searchBar);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "RetailItemAssembliesPage";
            this.Size = new System.Drawing.Size(800, 600);
            this.grpComponents.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpComponents;
        private Controls.ListView lvAssemblies;
        private Controls.ListView lvComponents;
        private Controls.ContextButtons btnsAssemblies;
        private Controls.ContextButtons btnsComponents;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Controls.Columns.Column clmStatus;
        private Controls.Columns.Column clmDescription;
        private Controls.Columns.Column clmStore;
        private Controls.Columns.Column clmStartingDate;
        private Controls.Columns.Column clmCostPrice;
        private Controls.Columns.Column clmPrice;
        private Controls.Columns.Column clmMargin;
        private Controls.Columns.Column clmShowComponents;
        private Controls.Columns.Column clmSendToStations;
        private Controls.Columns.Column clmItemID;
        private Controls.Columns.Column clmItemName;
        private Controls.Columns.Column clmQuantity;
        private Controls.Columns.Column clmComponentUnit;
        private Controls.Columns.Column clmCostPerUnit;
        private Controls.Columns.Column clmTotalCost;
        private Controls.SearchBar searchBar;
        private System.Windows.Forms.Button btnEnable;
        private Controls.Columns.Column clmVariantName;
        private Controls.Columns.Column clmCalculatePrice;
    }
}
