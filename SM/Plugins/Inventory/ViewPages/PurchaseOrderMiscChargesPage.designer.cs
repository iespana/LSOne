using LSOne.Controls;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    partial class PurchaseOrderMiscChargesPage
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseOrderMiscChargesPage));
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnsEditAddRemove = new LSOne.Controls.ContextButtons();
            this.lvItems = new LSOne.Controls.ListView();
            this.colType = new LSOne.Controls.Columns.Column();
            this.colReason = new LSOne.Controls.Columns.Column();
            this.colAmount = new LSOne.Controls.Columns.Column();
            this.colTaxAmount = new LSOne.Controls.Columns.Column();
            this.colTaxGroup = new LSOne.Controls.Columns.Column();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnsEditAddRemove
            // 
            this.btnsEditAddRemove.AddButtonEnabled = true;
            resources.ApplyResources(this.btnsEditAddRemove, "btnsEditAddRemove");
            this.btnsEditAddRemove.BackColor = System.Drawing.Color.Transparent;
            this.btnsEditAddRemove.Context = LSOne.Controls.ButtonTypes.EditAddRemove;
            this.btnsEditAddRemove.EditButtonEnabled = true;
            this.btnsEditAddRemove.Name = "btnsEditAddRemove";
            this.btnsEditAddRemove.RemoveButtonEnabled = true;
            this.btnsEditAddRemove.EditButtonClicked += new System.EventHandler(this.btnEdit_Click);
            this.btnsEditAddRemove.AddButtonClicked += new System.EventHandler(this.AddButtonClicked);
            this.btnsEditAddRemove.RemoveButtonClicked += new System.EventHandler(this.btnRemove_Click);
            // 
            // lvItems
            // 
            resources.ApplyResources(this.lvItems, "lvItems");
            this.lvItems.BuddyControl = null;
            this.lvItems.Columns.Add(this.colType);
            this.lvItems.Columns.Add(this.colReason);
            this.lvItems.Columns.Add(this.colAmount);
            this.lvItems.Columns.Add(this.colTaxAmount);
            this.lvItems.Columns.Add(this.colTaxGroup);
            this.lvItems.ContentBackColor = System.Drawing.Color.White;
            this.lvItems.DefaultRowHeight = ((short)(22));
            this.lvItems.DimSelectionWhenDisabled = true;
            this.lvItems.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvItems.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvItems.HeaderHeight = ((short)(25));
            this.lvItems.HorizontalScrollbar = true;
            this.lvItems.Name = "lvItems";
            this.lvItems.OddRowColor = System.Drawing.Color.White;
            this.lvItems.RowLineColor = System.Drawing.Color.LightGray;
            this.lvItems.SecondarySortColumn = ((short)(-1));
            this.lvItems.SelectionModel = LSOne.Controls.ListView.SelectionModelEnum.FullRowMultiSelection;
            this.lvItems.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvItems.SortSetting = "0:1";
            this.lvItems.SelectionChanged += new System.EventHandler(this.lvItems_SelectionChanged);
            this.lvItems.DoubleClick += new System.EventHandler(this.lvItems_DoubleClick);
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
            // colReason
            // 
            this.colReason.AutoSize = true;
            this.colReason.DefaultStyle = null;
            resources.ApplyResources(this.colReason, "colReason");
            this.colReason.InternalSort = true;
            this.colReason.MaximumWidth = ((short)(0));
            this.colReason.MinimumWidth = ((short)(10));
            this.colReason.SecondarySortColumn = ((short)(-1));
            this.colReason.Tag = null;
            this.colReason.Width = ((short)(50));
            // 
            // colAmount
            // 
            this.colAmount.AutoSize = true;
            this.colAmount.DefaultStyle = null;
            resources.ApplyResources(this.colAmount, "colAmount");
            this.colAmount.InternalSort = true;
            this.colAmount.MaximumWidth = ((short)(0));
            this.colAmount.MinimumWidth = ((short)(10));
            this.colAmount.SecondarySortColumn = ((short)(-1));
            this.colAmount.Tag = null;
            this.colAmount.Width = ((short)(50));
            // 
            // colTaxAmount
            // 
            this.colTaxAmount.AutoSize = true;
            this.colTaxAmount.DefaultStyle = null;
            resources.ApplyResources(this.colTaxAmount, "colTaxAmount");
            this.colTaxAmount.InternalSort = true;
            this.colTaxAmount.MaximumWidth = ((short)(0));
            this.colTaxAmount.MinimumWidth = ((short)(10));
            this.colTaxAmount.SecondarySortColumn = ((short)(-1));
            this.colTaxAmount.Tag = null;
            this.colTaxAmount.Width = ((short)(50));
            // 
            // colTaxGroup
            // 
            this.colTaxGroup.AutoSize = true;
            this.colTaxGroup.DefaultStyle = null;
            resources.ApplyResources(this.colTaxGroup, "colTaxGroup");
            this.colTaxGroup.InternalSort = true;
            this.colTaxGroup.MaximumWidth = ((short)(0));
            this.colTaxGroup.MinimumWidth = ((short)(10));
            this.colTaxGroup.SecondarySortColumn = ((short)(-1));
            this.colTaxGroup.Tag = null;
            this.colTaxGroup.Width = ((short)(50));
            // 
            // PurchaseOrderMiscChargesPage
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lvItems);
            this.Controls.Add(this.btnsEditAddRemove);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "PurchaseOrderMiscChargesPage";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ContextButtons btnsEditAddRemove;
        private ListView lvItems;
        private Controls.Columns.Column colType;
        private Controls.Columns.Column colReason;
        private Controls.Columns.Column colAmount;
        private Controls.Columns.Column colTaxAmount;
        private Controls.Columns.Column colTaxGroup;
    }
}
