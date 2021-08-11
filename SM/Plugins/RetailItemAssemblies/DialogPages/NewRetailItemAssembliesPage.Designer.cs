namespace LSOne.ViewPlugins.RetailItemAssemblies.DialogPages
{
    partial class NewRetailItemAssembliesPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRetailItemAssembliesPage));
            this.btnsEditAddRemoveAssemblies = new LSOne.Controls.ContextButtons();
            this.btnsAddEditRemoveComponents = new LSOne.Controls.ContextButtons();
            this.grpComponents = new System.Windows.Forms.GroupBox();
            this.lvComponents = new LSOne.Controls.ListView();
            this.clmItemID = new LSOne.Controls.Columns.Column();
            this.clmItem = new LSOne.Controls.Columns.Column();
            this.clmVariant = new LSOne.Controls.Columns.Column();
            this.clmQuantity = new LSOne.Controls.Columns.Column();
            this.clmComponentUnit = new LSOne.Controls.Columns.Column();
            this.clmCostPerUnit = new LSOne.Controls.Columns.Column();
            this.clmTotalCost = new LSOne.Controls.Columns.Column();
            this.lvAssemblies = new LSOne.Controls.ListView();
            this.clmDescription = new LSOne.Controls.Columns.Column();
            this.clmStore = new LSOne.Controls.Columns.Column();
            this.clmStartingDate = new LSOne.Controls.Columns.Column();
            this.clmCostPrice = new LSOne.Controls.Columns.Column();
            this.clmPrice = new LSOne.Controls.Columns.Column();
            this.clmMargin = new LSOne.Controls.Columns.Column();
            this.clmShowComponents = new LSOne.Controls.Columns.Column();
            this.clmCalculatePrice = new LSOne.Controls.Columns.Column();
            this.grpComponents.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnsEditAddRemoveAssemblies
            // 
            this.btnsEditAddRemoveAssemblies.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemoveAssemblies, "btnsEditAddRemoveAssemblies");
            this.btnsEditAddRemoveAssemblies.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemoveAssemblies.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemoveAssemblies.EditButtonEnabled = false;
            this.btnsEditAddRemoveAssemblies.Name = "btnsEditAddRemoveAssemblies";
            this.btnsEditAddRemoveAssemblies.RemoveButtonEnabled = false;
            this.btnsEditAddRemoveAssemblies.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemoveAssemblies_EditButtonClicked);
            this.btnsEditAddRemoveAssemblies.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemoveAssemblies_AddButtonClicked);
            this.btnsEditAddRemoveAssemblies.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemoveAssemblies_RemoveButtonClicked);
            // 
            // btnsAddEditRemoveComponents
            // 
            this.btnsAddEditRemoveComponents.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsAddEditRemoveComponents, "btnsAddEditRemoveComponents");
            this.btnsAddEditRemoveComponents.BackColor = System.Drawing.Color.Transparent;
            this.btnsAddEditRemoveComponents.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsAddEditRemoveComponents.EditButtonEnabled = false;
            this.btnsAddEditRemoveComponents.Name = "btnsAddEditRemoveComponents";
            this.btnsAddEditRemoveComponents.RemoveButtonEnabled = false;
            this.btnsAddEditRemoveComponents.EditButtonClicked += new System.EventHandler(this.btnsAddEditRemoveComponents_EditButtonClicked);
            this.btnsAddEditRemoveComponents.AddButtonClicked += new System.EventHandler(this.btnsAddEditRemoveComponents_AddButtonClicked);
            this.btnsAddEditRemoveComponents.RemoveButtonClicked += new System.EventHandler(this.btnsAddEditRemoveComponents_RemoveButtonClicked);
            // 
            // grpComponents
            // 
            resources.ApplyResources(this.grpComponents, "grpComponents");
            this.grpComponents.Controls.Add(this.btnsAddEditRemoveComponents);
            this.grpComponents.Controls.Add(this.lvComponents);
            this.grpComponents.Name = "grpComponents";
            this.grpComponents.TabStop = false;
            // 
            // lvComponents
            // 
            resources.ApplyResources(this.lvComponents, "lvComponents");
            this.lvComponents.BorderColor = System.Drawing.Color.DarkGray;
            this.lvComponents.BuddyControl = null;
            this.lvComponents.Columns.Add(this.clmItemID);
            this.lvComponents.Columns.Add(this.clmItem);
            this.lvComponents.Columns.Add(this.clmVariant);
            this.lvComponents.Columns.Add(this.clmQuantity);
            this.lvComponents.Columns.Add(this.clmComponentUnit);
            this.lvComponents.Columns.Add(this.clmCostPerUnit);
            this.lvComponents.Columns.Add(this.clmTotalCost);
            this.lvComponents.ContentBackColor = System.Drawing.Color.White;
            this.lvComponents.DefaultRowHeight = ((short)(22));
            this.lvComponents.DimSelectionWhenDisabled = true;
            this.lvComponents.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvComponents.HeaderBackColor = System.Drawing.Color.White;
            this.lvComponents.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvComponents.HeaderHeight = ((short)(25));
            this.lvComponents.Name = "lvComponents";
            this.lvComponents.OddRowColor = System.Drawing.Color.White;
            this.lvComponents.RowLineColor = System.Drawing.Color.LightGray;
            this.lvComponents.SecondarySortColumn = ((short)(-1));
            this.lvComponents.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvComponents.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvComponents.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvComponents.SortSetting = "0:1";
            this.lvComponents.VerticalScrollbarValue = 0;
            this.lvComponents.VerticalScrollbarYOffset = 0;
            this.lvComponents.SelectionChanged += new System.EventHandler(this.lvComponents_SelectionChanged);
            this.lvComponents.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvComponents_RowDoubleClick);
            // 
            // clmItemID
            // 
            this.clmItemID.AutoSize = true;
            this.clmItemID.DefaultStyle = null;
            resources.ApplyResources(this.clmItemID, "clmItemID");
            this.clmItemID.InternalSort = true;
            this.clmItemID.MaximumWidth = ((short)(0));
            this.clmItemID.MinimumWidth = ((short)(10));
            this.clmItemID.SecondarySortColumn = ((short)(-1));
            this.clmItemID.Tag = null;
            this.clmItemID.Width = ((short)(50));
            // 
            // clmItem
            // 
            this.clmItem.AutoSize = true;
            this.clmItem.DefaultStyle = null;
            resources.ApplyResources(this.clmItem, "clmItem");
            this.clmItem.InternalSort = true;
            this.clmItem.MaximumWidth = ((short)(0));
            this.clmItem.MinimumWidth = ((short)(10));
            this.clmItem.SecondarySortColumn = ((short)(-1));
            this.clmItem.Tag = null;
            this.clmItem.Width = ((short)(50));
            // 
            // clmVariant
            // 
            this.clmVariant.AutoSize = true;
            this.clmVariant.DefaultStyle = null;
            resources.ApplyResources(this.clmVariant, "clmVariant");
            this.clmVariant.InternalSort = true;
            this.clmVariant.MaximumWidth = ((short)(0));
            this.clmVariant.MinimumWidth = ((short)(10));
            this.clmVariant.SecondarySortColumn = ((short)(-1));
            this.clmVariant.Tag = null;
            this.clmVariant.Width = ((short)(50));
            // 
            // clmQuantity
            // 
            this.clmQuantity.AutoSize = true;
            this.clmQuantity.DefaultStyle = null;
            resources.ApplyResources(this.clmQuantity, "clmQuantity");
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
            resources.ApplyResources(this.clmComponentUnit, "clmComponentUnit");
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
            resources.ApplyResources(this.clmCostPerUnit, "clmCostPerUnit");
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
            resources.ApplyResources(this.clmTotalCost, "clmTotalCost");
            this.clmTotalCost.InternalSort = true;
            this.clmTotalCost.MaximumWidth = ((short)(0));
            this.clmTotalCost.MinimumWidth = ((short)(10));
            this.clmTotalCost.SecondarySortColumn = ((short)(-1));
            this.clmTotalCost.Tag = null;
            this.clmTotalCost.Width = ((short)(50));
            // 
            // lvAssemblies
            // 
            resources.ApplyResources(this.lvAssemblies, "lvAssemblies");
            this.lvAssemblies.BorderColor = System.Drawing.Color.DarkGray;
            this.lvAssemblies.BuddyControl = null;
            this.lvAssemblies.Columns.Add(this.clmDescription);
            this.lvAssemblies.Columns.Add(this.clmStore);
            this.lvAssemblies.Columns.Add(this.clmStartingDate);
            this.lvAssemblies.Columns.Add(this.clmCostPrice);
            this.lvAssemblies.Columns.Add(this.clmPrice);
            this.lvAssemblies.Columns.Add(this.clmMargin);
            this.lvAssemblies.Columns.Add(this.clmShowComponents);
            this.lvAssemblies.Columns.Add(this.clmCalculatePrice);
            this.lvAssemblies.ContentBackColor = System.Drawing.Color.White;
            this.lvAssemblies.DefaultRowHeight = ((short)(22));
            this.lvAssemblies.DimSelectionWhenDisabled = true;
            this.lvAssemblies.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvAssemblies.HeaderBackColor = System.Drawing.Color.White;
            this.lvAssemblies.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvAssemblies.HeaderHeight = ((short)(25));
            this.lvAssemblies.Name = "lvAssemblies";
            this.lvAssemblies.OddRowColor = System.Drawing.Color.White;
            this.lvAssemblies.RowLineColor = System.Drawing.Color.LightGray;
            this.lvAssemblies.SecondarySortColumn = ((short)(-1));
            this.lvAssemblies.SelectedRowColor = System.Drawing.Color.LightGray;
            this.lvAssemblies.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvAssemblies.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvAssemblies.SortSetting = "0:1";
            this.lvAssemblies.VerticalScrollbarValue = 0;
            this.lvAssemblies.VerticalScrollbarYOffset = 0;
            this.lvAssemblies.SelectionChanged += new System.EventHandler(this.lvAssemblies_SelectionChanged);
            this.lvAssemblies.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvAssemblies_RowDoubleClick);
            // 
            // clmDescription
            // 
            this.clmDescription.AutoSize = true;
            this.clmDescription.DefaultStyle = null;
            resources.ApplyResources(this.clmDescription, "clmDescription");
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
            resources.ApplyResources(this.clmStore, "clmStore");
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
            resources.ApplyResources(this.clmStartingDate, "clmStartingDate");
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
            resources.ApplyResources(this.clmCostPrice, "clmCostPrice");
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
            resources.ApplyResources(this.clmPrice, "clmPrice");
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
            resources.ApplyResources(this.clmMargin, "clmMargin");
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
            resources.ApplyResources(this.clmShowComponents, "clmShowComponents");
            this.clmShowComponents.InternalSort = true;
            this.clmShowComponents.MaximumWidth = ((short)(0));
            this.clmShowComponents.MinimumWidth = ((short)(10));
            this.clmShowComponents.SecondarySortColumn = ((short)(-1));
            this.clmShowComponents.Tag = null;
            this.clmShowComponents.Width = ((short)(50));
            // 
            // clmCalculatePrice
            // 
            this.clmCalculatePrice.AutoSize = true;
            this.clmCalculatePrice.DefaultStyle = null;
            resources.ApplyResources(this.clmCalculatePrice, "clmCalculatePrice");
            this.clmCalculatePrice.InternalSort = true;
            this.clmCalculatePrice.MaximumWidth = ((short)(0));
            this.clmCalculatePrice.MinimumWidth = ((short)(10));
            this.clmCalculatePrice.SecondarySortColumn = ((short)(-1));
            this.clmCalculatePrice.Tag = null;
            this.clmCalculatePrice.Width = ((short)(50));
            // 
            // NewRetailItemAssembliesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnsEditAddRemoveAssemblies);
            this.Controls.Add(this.lvAssemblies);
            this.Controls.Add(this.grpComponents);
            this.DoubleBuffered = true;
            this.Name = "NewRetailItemAssembliesPage";
            this.grpComponents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Controls.ContextButtons btnsEditAddRemoveAssemblies;
        private Controls.ContextButtons btnsAddEditRemoveComponents;
        private System.Windows.Forms.GroupBox grpComponents;
        private Controls.ListView lvComponents;
        private Controls.Columns.Column clmItemID;
        private Controls.Columns.Column clmItem;
        private Controls.Columns.Column clmQuantity;
        private Controls.Columns.Column clmComponentUnit;
        private Controls.Columns.Column clmCostPerUnit;
        private Controls.Columns.Column clmTotalCost;
        private Controls.ListView lvAssemblies;
        private Controls.Columns.Column clmDescription;
        private Controls.Columns.Column clmStore;
        private Controls.Columns.Column clmStartingDate;
        private Controls.Columns.Column clmCostPrice;
        private Controls.Columns.Column clmPrice;
        private Controls.Columns.Column clmMargin;
        private Controls.Columns.Column clmShowComponents;
        private Controls.Columns.Column clmVariant;
        private Controls.Columns.Column clmCalculatePrice;
    }
}
