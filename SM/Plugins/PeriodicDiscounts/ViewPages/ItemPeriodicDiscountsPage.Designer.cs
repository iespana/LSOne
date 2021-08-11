using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.ViewPages
{
    partial class ItemPeriodicDiscountsPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemPeriodicDiscountsPage));
            this.btnsContextButtonsItems = new LSOne.Controls.ContextButtons();
            this.lvValues = new LSOne.Controls.ListView();
            this.colOfferType = new LSOne.Controls.Columns.Column();
            this.colOffer = new LSOne.Controls.Columns.Column();
            this.colOfferPriority = new LSOne.Controls.Columns.Column();
            this.colStatus = new LSOne.Controls.Columns.Column();
            this.colType = new LSOne.Controls.Columns.Column();
            this.colItemID = new LSOne.Controls.Columns.Column();
            this.colDescription = new LSOne.Controls.Columns.Column();
            this.colVariant = new LSOne.Controls.Columns.Column();
            this.ColDiscountPerc = new LSOne.Controls.Columns.Column();
            this.colOfferPriceWithTax = new LSOne.Controls.Columns.Column();
            this.colDiscAmtWithTax = new LSOne.Controls.Columns.Column();
            this.btnShowOffer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnsContextButtonsItems
            // 
            this.btnsContextButtonsItems.AddButtonEnabled = false;
            resources.ApplyResources(this.btnsContextButtonsItems, "btnsContextButtonsItems");
            this.btnsContextButtonsItems.BackColor = System.Drawing.Color.Transparent;
            this.btnsContextButtonsItems.Context = LSOne.Controls.ButtonTypes.EditRemove;
            this.btnsContextButtonsItems.EditButtonEnabled = false;
            this.btnsContextButtonsItems.Name = "btnsContextButtonsItems";
            this.btnsContextButtonsItems.RemoveButtonEnabled = false;
            this.btnsContextButtonsItems.EditButtonClicked += new System.EventHandler(this.btnEditValue_Click);
            this.btnsContextButtonsItems.RemoveButtonClicked += new System.EventHandler(this.btnRemoveValue_Click);
            // 
            // lvValues
            // 
            resources.ApplyResources(this.lvValues, "lvValues");
            this.lvValues.BuddyControl = null;
            this.lvValues.Columns.Add(this.colOfferType);
            this.lvValues.Columns.Add(this.colOffer);
            this.lvValues.Columns.Add(this.colOfferPriority);
            this.lvValues.Columns.Add(this.colStatus);
            this.lvValues.Columns.Add(this.colType);
            this.lvValues.Columns.Add(this.colItemID);
            this.lvValues.Columns.Add(this.colDescription);
            this.lvValues.Columns.Add(this.colVariant);
            this.lvValues.Columns.Add(this.ColDiscountPerc);
            this.lvValues.Columns.Add(this.colOfferPriceWithTax);
            this.lvValues.Columns.Add(this.colDiscAmtWithTax);
            this.lvValues.ContentBackColor = System.Drawing.Color.White;
            this.lvValues.DefaultRowHeight = ((short)(22));
            this.lvValues.DimSelectionWhenDisabled = true;
            this.lvValues.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvValues.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvValues.HeaderHeight = ((short)(25));
            this.lvValues.HorizontalScrollbar = true;
            this.lvValues.Name = "lvValues";
            this.lvValues.OddRowColor = System.Drawing.Color.White;
            this.lvValues.RowLineColor = System.Drawing.Color.LightGray;
            this.lvValues.SecondarySortColumn = ((short)(-1));
            this.lvValues.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvValues.SortSetting = "0:1";
            this.lvValues.SelectionChanged += new System.EventHandler(this.lvValues_SelectionChanged);
            this.lvValues.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvValues_RowDoubleClick);
            // 
            // colOfferType
            // 
            this.colOfferType.AutoSize = true;
            this.colOfferType.DefaultStyle = null;
            resources.ApplyResources(this.colOfferType, "colOfferType");
            this.colOfferType.InternalSort = true;
            this.colOfferType.MaximumWidth = ((short)(0));
            this.colOfferType.MinimumWidth = ((short)(10));
            this.colOfferType.SecondarySortColumn = ((short)(-1));
            this.colOfferType.Tag = null;
            this.colOfferType.Width = ((short)(50));
            // 
            // colOffer
            // 
            this.colOffer.AutoSize = true;
            this.colOffer.DefaultStyle = null;
            resources.ApplyResources(this.colOffer, "colOffer");
            this.colOffer.InternalSort = true;
            this.colOffer.MaximumWidth = ((short)(0));
            this.colOffer.MinimumWidth = ((short)(10));
            this.colOffer.SecondarySortColumn = ((short)(-1));
            this.colOffer.Tag = null;
            this.colOffer.Width = ((short)(50));
            // 
            // colOfferPriority
            // 
            this.colOfferPriority.AutoSize = true;
            this.colOfferPriority.DefaultStyle = null;
            resources.ApplyResources(this.colOfferPriority, "colOfferPriority");
            this.colOfferPriority.InternalSort = true;
            this.colOfferPriority.MaximumWidth = ((short)(0));
            this.colOfferPriority.MinimumWidth = ((short)(10));
            this.colOfferPriority.SecondarySortColumn = ((short)(-1));
            this.colOfferPriority.Tag = null;
            this.colOfferPriority.Width = ((short)(50));
            // 
            // colStatus
            // 
            this.colStatus.AutoSize = true;
            this.colStatus.DefaultStyle = null;
            resources.ApplyResources(this.colStatus, "colStatus");
            this.colStatus.InternalSort = true;
            this.colStatus.MaximumWidth = ((short)(0));
            this.colStatus.MinimumWidth = ((short)(10));
            this.colStatus.SecondarySortColumn = ((short)(-1));
            this.colStatus.Tag = null;
            this.colStatus.Width = ((short)(50));
            // 
            // colType
            // 
            this.colType.AutoSize = true;
            this.colType.DefaultStyle = null;
            resources.ApplyResources(this.colType, "colType");
            this.colType.InternalSort = true;
            this.colType.MaximumWidth = ((short)(0));
            this.colType.MinimumWidth = ((short)(10));
            this.colType.SecondarySortColumn = ((short)(-1));
            this.colType.Tag = null;
            this.colType.Width = ((short)(50));
            // 
            // colItemID
            // 
            this.colItemID.AutoSize = true;
            this.colItemID.DefaultStyle = null;
            resources.ApplyResources(this.colItemID, "colItemID");
            this.colItemID.InternalSort = true;
            this.colItemID.MaximumWidth = ((short)(0));
            this.colItemID.MinimumWidth = ((short)(10));
            this.colItemID.SecondarySortColumn = ((short)(-1));
            this.colItemID.Tag = null;
            this.colItemID.Width = ((short)(50));
            // 
            // colDescription
            // 
            this.colDescription.AutoSize = true;
            this.colDescription.DefaultStyle = null;
            resources.ApplyResources(this.colDescription, "colDescription");
            this.colDescription.InternalSort = true;
            this.colDescription.MaximumWidth = ((short)(0));
            this.colDescription.MinimumWidth = ((short)(10));
            this.colDescription.SecondarySortColumn = ((short)(-1));
            this.colDescription.Tag = null;
            this.colDescription.Width = ((short)(50));
            // 
            // colVariant
            // 
            this.colVariant.AutoSize = true;
            this.colVariant.DefaultStyle = null;
            this.colVariant.HeaderAffectsAutoSize = false;
            resources.ApplyResources(this.colVariant, "colVariant");
            this.colVariant.InternalSort = true;
            this.colVariant.MaximumWidth = ((short)(0));
            this.colVariant.MinimumWidth = ((short)(10));
            this.colVariant.NoTextWhenSmall = true;
            this.colVariant.SecondarySortColumn = ((short)(-1));
            this.colVariant.Tag = null;
            this.colVariant.Width = ((short)(50));
            // 
            // ColDiscountPerc
            // 
            this.ColDiscountPerc.AutoSize = true;
            this.ColDiscountPerc.DefaultStyle = null;
            resources.ApplyResources(this.ColDiscountPerc, "ColDiscountPerc");
            this.ColDiscountPerc.InternalSort = true;
            this.ColDiscountPerc.MaximumWidth = ((short)(0));
            this.ColDiscountPerc.MinimumWidth = ((short)(10));
            this.ColDiscountPerc.SecondarySortColumn = ((short)(-1));
            this.ColDiscountPerc.Tag = null;
            this.ColDiscountPerc.Width = ((short)(50));
            // 
            // colOfferPriceWithTax
            // 
            this.colOfferPriceWithTax.AutoSize = true;
            this.colOfferPriceWithTax.DefaultStyle = null;
            resources.ApplyResources(this.colOfferPriceWithTax, "colOfferPriceWithTax");
            this.colOfferPriceWithTax.InternalSort = true;
            this.colOfferPriceWithTax.MaximumWidth = ((short)(0));
            this.colOfferPriceWithTax.MinimumWidth = ((short)(10));
            this.colOfferPriceWithTax.SecondarySortColumn = ((short)(-1));
            this.colOfferPriceWithTax.Tag = null;
            this.colOfferPriceWithTax.Width = ((short)(50));
            // 
            // colDiscAmtWithTax
            // 
            this.colDiscAmtWithTax.AutoSize = true;
            this.colDiscAmtWithTax.DefaultStyle = null;
            resources.ApplyResources(this.colDiscAmtWithTax, "colDiscAmtWithTax");
            this.colDiscAmtWithTax.InternalSort = true;
            this.colDiscAmtWithTax.MaximumWidth = ((short)(0));
            this.colDiscAmtWithTax.MinimumWidth = ((short)(10));
            this.colDiscAmtWithTax.SecondarySortColumn = ((short)(-1));
            this.colDiscAmtWithTax.Tag = null;
            this.colDiscAmtWithTax.Width = ((short)(50));
            // 
            // btnShowOffer
            // 
            resources.ApplyResources(this.btnShowOffer, "btnShowOffer");
            this.btnShowOffer.Name = "btnShowOffer";
            this.btnShowOffer.UseVisualStyleBackColor = true;
            this.btnShowOffer.Click += new System.EventHandler(this.showOffer);
            // 
            // ItemPeriodicDiscountsPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.btnShowOffer);
            this.Controls.Add(this.lvValues);
            this.Controls.Add(this.btnsContextButtonsItems);
            this.Name = "ItemPeriodicDiscountsPage";
            this.ResumeLayout(false);

        }

        #endregion
        private ContextButtons btnsContextButtonsItems;
        private ListView lvValues;
        private Controls.Columns.Column colOfferType;
        private Controls.Columns.Column colOffer;
        private Controls.Columns.Column colOfferPriority;
        private Controls.Columns.Column colStatus;
        private Controls.Columns.Column colType;
        private Controls.Columns.Column colItemID;
        private Controls.Columns.Column colDescription;
        private Controls.Columns.Column colVariant;
        private Controls.Columns.Column ColDiscountPerc;
        private Controls.Columns.Column colOfferPriceWithTax;
        private Controls.Columns.Column colDiscAmtWithTax;
        private System.Windows.Forms.Button btnShowOffer;
    }
}
