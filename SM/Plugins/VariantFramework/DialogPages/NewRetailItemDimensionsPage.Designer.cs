namespace LSOne.ViewPlugins.VariantFramework.DialogPages
{
    partial class NewRetailItemDimensionsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewRetailItemDimensionsPage));
            this.lblDimensions = new System.Windows.Forms.Label();
            this.lblDimensionAttribute = new System.Windows.Forms.Label();
            this.btnsEditAddRemoveDimension = new LSOne.Controls.ContextButtons();
            this.btnsEditAddRemoveDimensionAttribute = new LSOne.Controls.ContextButtons();
            this.gbDescriptionsPreview = new LSOne.Controls.DoubleBufferGroupBox();
            this.lblDescriptionPreview = new System.Windows.Forms.Label();
            this.btnMoveUpDimension = new LSOne.Controls.ContextButton();
            this.btnMoveDownDimension = new LSOne.Controls.ContextButton();
            this.btnMoveUpDimensionAttribute = new LSOne.Controls.ContextButton();
            this.btnMoveDownDimensionAttribute = new LSOne.Controls.ContextButton();
            this.lvDimensionAttribute = new LSOne.Controls.ListView();
            this.dimensionAttributeColumn1 = new LSOne.Controls.Columns.Column();
            this.dimensionAttributeColumn2 = new LSOne.Controls.Columns.Column();
            this.lvDimensions = new LSOne.Controls.ListView();
            this.dimensionsListViewColumn1 = new LSOne.Controls.Columns.Column();
            this.gbDescriptionsPreview.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDimensions
            // 
            resources.ApplyResources(this.lblDimensions, "lblDimensions");
            this.lblDimensions.Name = "lblDimensions";
            // 
            // lblDimensionAttribute
            // 
            resources.ApplyResources(this.lblDimensionAttribute, "lblDimensionAttribute");
            this.lblDimensionAttribute.Name = "lblDimensionAttribute";
            // 
            // btnsEditAddRemoveDimension
            // 
            this.btnsEditAddRemoveDimension.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemoveDimension, "btnsEditAddRemoveDimension");
            this.btnsEditAddRemoveDimension.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemoveDimension.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemoveDimension.EditButtonEnabled = false;
            this.btnsEditAddRemoveDimension.Name = "btnsEditAddRemoveDimension";
            this.btnsEditAddRemoveDimension.RemoveButtonEnabled = false;
            this.btnsEditAddRemoveDimension.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimension_EditButtonClicked);
            this.btnsEditAddRemoveDimension.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimension_AddButtonClicked);
            this.btnsEditAddRemoveDimension.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimension_RemoveButtonClicked);
            // 
            // btnsEditAddRemoveDimensionAttribute
            // 
            this.btnsEditAddRemoveDimensionAttribute.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemoveDimensionAttribute, "btnsEditAddRemoveDimensionAttribute");
            this.btnsEditAddRemoveDimensionAttribute.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemoveDimensionAttribute.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemoveDimensionAttribute.EditButtonEnabled = false;
            this.btnsEditAddRemoveDimensionAttribute.Name = "btnsEditAddRemoveDimensionAttribute";
            this.btnsEditAddRemoveDimensionAttribute.RemoveButtonEnabled = false;
            this.btnsEditAddRemoveDimensionAttribute.EditButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimensionAttribute_EditButtonClicked);
            this.btnsEditAddRemoveDimensionAttribute.AddButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimensionAttribute_AddButtonClicked);
            this.btnsEditAddRemoveDimensionAttribute.RemoveButtonClicked += new System.EventHandler(this.btnsEditAddRemoveDimensionAttribute_RemoveButtonClicked);
            // 
            // gbDescriptionsPreview
            // 
            resources.ApplyResources(this.gbDescriptionsPreview, "gbDescriptionsPreview");
            this.gbDescriptionsPreview.Controls.Add(this.lblDescriptionPreview);
            this.gbDescriptionsPreview.Name = "gbDescriptionsPreview";
            this.gbDescriptionsPreview.TabStop = false;
            // 
            // lblDescriptionPreview
            // 
            resources.ApplyResources(this.lblDescriptionPreview, "lblDescriptionPreview");
            this.lblDescriptionPreview.Name = "lblDescriptionPreview";
            // 
            // btnMoveUpDimension
            // 
            this.btnMoveUpDimension.Context = LSOne.Controls.ButtonType.MoveUp;
            resources.ApplyResources(this.btnMoveUpDimension, "btnMoveUpDimension");
            this.btnMoveUpDimension.Name = "btnMoveUpDimension";
            this.btnMoveUpDimension.Click += new System.EventHandler(this.btnMoveUpDimension_Click);
            // 
            // btnMoveDownDimension
            // 
            this.btnMoveDownDimension.Context = LSOne.Controls.ButtonType.MoveDown;
            resources.ApplyResources(this.btnMoveDownDimension, "btnMoveDownDimension");
            this.btnMoveDownDimension.Name = "btnMoveDownDimension";
            this.btnMoveDownDimension.Click += new System.EventHandler(this.btnMoveDownDimension_Click);
            // 
            // btnMoveUpDimensionAttribute
            // 
            resources.ApplyResources(this.btnMoveUpDimensionAttribute, "btnMoveUpDimensionAttribute");
            this.btnMoveUpDimensionAttribute.Context = LSOne.Controls.ButtonType.MoveUp;
            this.btnMoveUpDimensionAttribute.Name = "btnMoveUpDimensionAttribute";
            this.btnMoveUpDimensionAttribute.Click += new System.EventHandler(this.btnMoveUpDimensionAttribute_Click);
            // 
            // btnMoveDownDimensionAttribute
            // 
            resources.ApplyResources(this.btnMoveDownDimensionAttribute, "btnMoveDownDimensionAttribute");
            this.btnMoveDownDimensionAttribute.Context = LSOne.Controls.ButtonType.MoveDown;
            this.btnMoveDownDimensionAttribute.Name = "btnMoveDownDimensionAttribute";
            this.btnMoveDownDimensionAttribute.Click += new System.EventHandler(this.btnMoveDownDimensionAttribute_Click);
            // 
            // lvDimensionAttribute
            // 
            resources.ApplyResources(this.lvDimensionAttribute, "lvDimensionAttribute");
            this.lvDimensionAttribute.BuddyControl = null;
            this.lvDimensionAttribute.Columns.Add(this.dimensionAttributeColumn1);
            this.lvDimensionAttribute.Columns.Add(this.dimensionAttributeColumn2);
            this.lvDimensionAttribute.ContentBackColor = System.Drawing.Color.White;
            this.lvDimensionAttribute.DefaultRowHeight = ((short)(22));
            this.lvDimensionAttribute.DimSelectionWhenDisabled = true;
            this.lvDimensionAttribute.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvDimensionAttribute.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDimensionAttribute.HeaderHeight = ((short)(25));
            this.lvDimensionAttribute.Name = "lvDimensionAttribute";
            this.lvDimensionAttribute.OddRowColor = System.Drawing.Color.White;
            this.lvDimensionAttribute.RowLineColor = System.Drawing.Color.LightGray;
            this.lvDimensionAttribute.SecondarySortColumn = ((short)(-1));
            this.lvDimensionAttribute.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvDimensionAttribute.SortSetting = "0:1";
            this.lvDimensionAttribute.SelectionChanged += new System.EventHandler(this.lvDimensionAttribute_SelectionChanged);
            this.lvDimensionAttribute.DoubleClick += new System.EventHandler(this.lvDimensionAttribute_DoubleClick);
            // 
            // dimensionAttributeColumn1
            // 
            this.dimensionAttributeColumn1.AutoSize = true;
            this.dimensionAttributeColumn1.Clickable = false;
            this.dimensionAttributeColumn1.DefaultStyle = null;
            resources.ApplyResources(this.dimensionAttributeColumn1, "dimensionAttributeColumn1");
            this.dimensionAttributeColumn1.MaximumWidth = ((short)(0));
            this.dimensionAttributeColumn1.MinimumWidth = ((short)(10));
            this.dimensionAttributeColumn1.SecondarySortColumn = ((short)(-1));
            this.dimensionAttributeColumn1.Sizable = false;
            this.dimensionAttributeColumn1.Tag = null;
            this.dimensionAttributeColumn1.Width = ((short)(50));
            // 
            // dimensionAttributeColumn2
            // 
            this.dimensionAttributeColumn2.AutoSize = true;
            this.dimensionAttributeColumn2.Clickable = false;
            this.dimensionAttributeColumn2.DefaultStyle = null;
            resources.ApplyResources(this.dimensionAttributeColumn2, "dimensionAttributeColumn2");
            this.dimensionAttributeColumn2.MaximumWidth = ((short)(0));
            this.dimensionAttributeColumn2.MinimumWidth = ((short)(10));
            this.dimensionAttributeColumn2.SecondarySortColumn = ((short)(-1));
            this.dimensionAttributeColumn2.Sizable = false;
            this.dimensionAttributeColumn2.Tag = null;
            this.dimensionAttributeColumn2.Width = ((short)(50));
            // 
            // lvDimensions
            // 
            resources.ApplyResources(this.lvDimensions, "lvDimensions");
            this.lvDimensions.BuddyControl = null;
            this.lvDimensions.Columns.Add(this.dimensionsListViewColumn1);
            this.lvDimensions.ContentBackColor = System.Drawing.Color.White;
            this.lvDimensions.DefaultRowHeight = ((short)(22));
            this.lvDimensions.DimSelectionWhenDisabled = true;
            this.lvDimensions.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvDimensions.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvDimensions.HeaderHeight = ((short)(25));
            this.lvDimensions.Name = "lvDimensions";
            this.lvDimensions.OddRowColor = System.Drawing.Color.White;
            this.lvDimensions.RowLineColor = System.Drawing.Color.LightGray;
            this.lvDimensions.SecondarySortColumn = ((short)(-1));
            this.lvDimensions.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvDimensions.SortSetting = "0:1";
            this.lvDimensions.SelectionChanged += new System.EventHandler(this.lvDimensions_SelectionChanged);
            this.lvDimensions.DoubleClick += new System.EventHandler(this.lvDimensions_DoubleClick);
            // 
            // dimensionsListViewColumn1
            // 
            this.dimensionsListViewColumn1.AutoSize = true;
            this.dimensionsListViewColumn1.Clickable = false;
            this.dimensionsListViewColumn1.DefaultStyle = null;
            resources.ApplyResources(this.dimensionsListViewColumn1, "dimensionsListViewColumn1");
            this.dimensionsListViewColumn1.MaximumWidth = ((short)(0));
            this.dimensionsListViewColumn1.MinimumWidth = ((short)(10));
            this.dimensionsListViewColumn1.SecondarySortColumn = ((short)(-1));
            this.dimensionsListViewColumn1.Sizable = false;
            this.dimensionsListViewColumn1.Tag = null;
            this.dimensionsListViewColumn1.Width = ((short)(50));
            // 
            // NewRetailItemDimensionsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnMoveDownDimensionAttribute);
            this.Controls.Add(this.btnMoveUpDimensionAttribute);
            this.Controls.Add(this.btnMoveDownDimension);
            this.Controls.Add(this.btnMoveUpDimension);
            this.Controls.Add(this.gbDescriptionsPreview);
            this.Controls.Add(this.btnsEditAddRemoveDimension);
            this.Controls.Add(this.btnsEditAddRemoveDimensionAttribute);
            this.Controls.Add(this.lvDimensionAttribute);
            this.Controls.Add(this.lblDimensionAttribute);
            this.Controls.Add(this.lvDimensions);
            this.Controls.Add(this.lblDimensions);
            this.DoubleBuffered = true;
            this.Name = "NewRetailItemDimensionsPage";
            this.gbDescriptionsPreview.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblDimensions;
        private Controls.ListView lvDimensions;
        private Controls.ListView lvDimensionAttribute;
        private System.Windows.Forms.Label lblDimensionAttribute;
        private Controls.ContextButtons btnsEditAddRemoveDimensionAttribute;
        private Controls.ContextButtons btnsEditAddRemoveDimension;
        private Controls.Columns.Column dimensionsListViewColumn1;
        private Controls.Columns.Column dimensionAttributeColumn1;
        private Controls.Columns.Column dimensionAttributeColumn2;
        private LSOne.Controls.DoubleBufferGroupBox gbDescriptionsPreview;
        private System.Windows.Forms.Label lblDescriptionPreview;
        private Controls.ContextButton btnMoveUpDimension;
        private Controls.ContextButton btnMoveDownDimension;
        private Controls.ContextButton btnMoveUpDimensionAttribute;
        private Controls.ContextButton btnMoveDownDimensionAttribute;
    }
}
