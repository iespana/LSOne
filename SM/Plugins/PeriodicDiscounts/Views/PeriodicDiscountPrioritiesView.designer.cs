using LSOne.Controls;
using LSOne.Controls.Columns;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Views
{
    partial class PeriodicDiscountPrioritiesView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PeriodicDiscountPrioritiesView));
            this.lvPeriodicDiscounts = new LSOne.Controls.ListView();
            this.column1 = new LSOne.Controls.Columns.Column();
            this.column2 = new LSOne.Controls.Columns.Column();
            this.column3 = new LSOne.Controls.Columns.Column();
            this.column4 = new LSOne.Controls.Columns.Column();
            this.column5 = new LSOne.Controls.Columns.Column();
            this.column6 = new LSOne.Controls.Columns.Column();
            this.btnMoveUp = new LSOne.Controls.ContextButton();
            this.btnMoveDown = new LSOne.Controls.ContextButton();
            this.btnEdit = new LSOne.Controls.ContextButton();
            this.btnEditDiscount = new System.Windows.Forms.Button();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnEditDiscount);
            this.pnlBottom.Controls.Add(this.btnEdit);
            this.pnlBottom.Controls.Add(this.btnMoveDown);
            this.pnlBottom.Controls.Add(this.btnMoveUp);
            this.pnlBottom.Controls.Add(this.lvPeriodicDiscounts);
            // 
            // lvPeriodicDiscounts
            // 
            resources.ApplyResources(this.lvPeriodicDiscounts, "lvPeriodicDiscounts");
            this.lvPeriodicDiscounts.BuddyControl = null;
            this.lvPeriodicDiscounts.Columns.Add(this.column1);
            this.lvPeriodicDiscounts.Columns.Add(this.column2);
            this.lvPeriodicDiscounts.Columns.Add(this.column3);
            this.lvPeriodicDiscounts.Columns.Add(this.column4);
            this.lvPeriodicDiscounts.Columns.Add(this.column5);
            this.lvPeriodicDiscounts.Columns.Add(this.column6);
            this.lvPeriodicDiscounts.ContentBackColor = System.Drawing.Color.White;
            this.lvPeriodicDiscounts.DefaultRowHeight = ((short)(22));
            this.lvPeriodicDiscounts.DimSelectionWhenDisabled = true;
            this.lvPeriodicDiscounts.EvenRowColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(236)))), ((int)(((byte)(226)))));
            this.lvPeriodicDiscounts.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvPeriodicDiscounts.HeaderHeight = ((short)(25));
            this.lvPeriodicDiscounts.Name = "lvPeriodicDiscounts";
            this.lvPeriodicDiscounts.OddRowColor = System.Drawing.Color.White;
            this.lvPeriodicDiscounts.RowLineColor = System.Drawing.Color.LightGray;
            this.lvPeriodicDiscounts.SecondarySortColumn = ((short)(-1));
            this.lvPeriodicDiscounts.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvPeriodicDiscounts.SortSetting = "0:1";
            this.lvPeriodicDiscounts.SelectionChanged += new System.EventHandler(this.lvPeriodicDiscounts_SelectionChanged);
            this.lvPeriodicDiscounts.RowDoubleClick += new LSOne.Controls.RowClickDelegate(this.lvPeriodicDiscounts_RowDoubleClick);
            // 
            // column1
            // 
            this.column1.AutoSize = true;
            this.column1.Clickable = false;
            this.column1.DefaultStyle = null;
            resources.ApplyResources(this.column1, "column1");
            this.column1.MaximumWidth = ((short)(0));
            this.column1.MinimumWidth = ((short)(10));
            this.column1.SecondarySortColumn = ((short)(-1));
            this.column1.Tag = null;
            this.column1.Width = ((short)(100));
            // 
            // column2
            // 
            this.column2.AutoSize = true;
            this.column2.Clickable = false;
            this.column2.DefaultStyle = null;
            resources.ApplyResources(this.column2, "column2");
            this.column2.MaximumWidth = ((short)(0));
            this.column2.MinimumWidth = ((short)(10));
            this.column2.SecondarySortColumn = ((short)(-1));
            this.column2.Tag = null;
            this.column2.Width = ((short)(100));
            // 
            // column3
            // 
            this.column3.AutoSize = true;
            this.column3.Clickable = false;
            this.column3.DefaultStyle = null;
            resources.ApplyResources(this.column3, "column3");
            this.column3.MaximumWidth = ((short)(0));
            this.column3.MinimumWidth = ((short)(80));
            this.column3.SecondarySortColumn = ((short)(-1));
            this.column3.Tag = null;
            this.column3.Width = ((short)(100));
            // 
            // column4
            // 
            this.column4.AutoSize = true;
            this.column4.Clickable = false;
            this.column4.DefaultStyle = null;
            resources.ApplyResources(this.column4, "column4");
            this.column4.MaximumWidth = ((short)(0));
            this.column4.MinimumWidth = ((short)(10));
            this.column4.SecondarySortColumn = ((short)(-1));
            this.column4.Tag = null;
            this.column4.Width = ((short)(50));
            // 
            // column5
            // 
            this.column5.AutoSize = true;
            this.column5.Clickable = false;
            this.column5.DefaultStyle = null;
            resources.ApplyResources(this.column5, "column5");
            this.column5.MaximumWidth = ((short)(0));
            this.column5.MinimumWidth = ((short)(10));
            this.column5.SecondarySortColumn = ((short)(-1));
            this.column5.Tag = null;
            this.column5.Width = ((short)(50));
            // 
            // column6
            // 
            this.column6.AutoSize = true;
            this.column6.Clickable = false;
            this.column6.DefaultStyle = null;
            resources.ApplyResources(this.column6, "column6");
            this.column6.MaximumWidth = ((short)(0));
            this.column6.MinimumWidth = ((short)(10));
            this.column6.NoTextWhenSmall = true;
            this.column6.SecondarySortColumn = ((short)(-1));
            this.column6.Tag = null;
            this.column6.Width = ((short)(50));
            // 
            // btnMoveUp
            // 
            resources.ApplyResources(this.btnMoveUp, "btnMoveUp");
            this.btnMoveUp.Context = LSOne.Controls.ButtonType.MoveUp;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            resources.ApplyResources(this.btnMoveDown, "btnMoveDown");
            this.btnMoveDown.Context = LSOne.Controls.ButtonType.MoveDown;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.Context = LSOne.Controls.ButtonType.Edit;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Click += new System.EventHandler(this.EditPriority);
            // 
            // btnEditDiscount
            // 
            resources.ApplyResources(this.btnEditDiscount, "btnEditDiscount");
            this.btnEditDiscount.BackColor = System.Drawing.Color.Transparent;
            this.btnEditDiscount.Name = "btnEditDiscount";
            this.btnEditDiscount.UseVisualStyleBackColor = true;
            this.btnEditDiscount.Click += new System.EventHandler(this.btnEditDiscount_Click);
            // 
            // PeriodicDiscountPrioritiesView
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PeriodicDiscountPrioritiesView";
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvPeriodicDiscounts;
        private Column column1;
        private Column column2;
        private Column column3;
        private Column column4;
        private ContextButton btnMoveDown;
        private ContextButton btnMoveUp;
        private ContextButton btnEdit;
        private Column column5;
        private Column column6;
        private System.Windows.Forms.Button btnEditDiscount;
    }
}
